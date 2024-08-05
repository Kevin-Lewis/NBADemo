using Azure;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;
using Azure.AI.OpenAI.Assistants;

namespace NBADemo.Services
{
    public class AIService : IAIService
    {
        private readonly AssistantsClient _client;
        private readonly string _apiKey;
        private readonly string _assistantId;
        private readonly JsonSerializerOptions _serializerOptions;
        private readonly Dictionary<string, string> _sessionThreads;

        public AIService()
        {
            _apiKey = "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";
            _assistantId = "asst_q9mRJJd8n7n47Lvq7haMiCCU";

            _client = new AssistantsClient(_apiKey);

            _serializerOptions = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            };

            _sessionThreads = new Dictionary<string, string>();
        }

        public async Task<List<string>> WriteScoutingReport(string sessionId, string prompt)
        {
            try
            {
                string threadId;

                // Check if there's an existing thread for the session
                if (_sessionThreads.ContainsKey(sessionId))
                {
                    threadId = _sessionThreads[sessionId];
                }
                else
                {
                    // Step 1: Create a Thread
                    Response<AssistantThread> threadResponse = await _client.CreateThreadAsync();
                    AssistantThread thread = threadResponse.Value;
                    threadId = thread.Id;

                    // Save the thread ID for the session
                    _sessionThreads[sessionId] = threadId;
                }

                // Step 2: Add a Message to the Thread
                Response<ThreadMessage> messageResponse = await _client.CreateMessageAsync(
                    threadId,
                    MessageRole.User,
                    JsonSerializer.Serialize(prompt, _serializerOptions));
                ThreadMessage message = messageResponse.Value;

                // Step 3: Create a Run
                Response<ThreadRun> runResponse = await _client.CreateRunAsync(
                    threadId,
                    new CreateRunOptions(_assistantId));
                ThreadRun run = runResponse.Value;

                do
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));
                    runResponse = await _client.GetRunAsync(threadId, runResponse.Value.Id);
                }
                while (runResponse.Value.Status == RunStatus.Queued
                    || runResponse.Value.Status == RunStatus.InProgress);

                Response<PageableList<ThreadMessage>> afterRunMessagesResponse = await _client.GetMessagesAsync(threadId);
                IReadOnlyList<ThreadMessage> messages = afterRunMessagesResponse.Value.Data;

                var responseList = new List<string>();
                foreach (ThreadMessage threadMessage in messages.OrderBy(m => m.CreatedAt))
                {
                    var messageText = new StringBuilder();
                    messageText.Append($"{threadMessage.CreatedAt:yyyy-MM-dd HH:mm:ss} - {threadMessage.Role}: ");
                    foreach (Azure.AI.OpenAI.Assistants.MessageContent contentItem in threadMessage.ContentItems)
                    {
                        if (contentItem is MessageTextContent textItem)
                        {
                            messageText.Append(textItem.Text);
                        }
                        else if (contentItem is MessageImageFileContent imageFileItem)
                        {
                            messageText.Append($"Sorry! I am unable to generate a report at this time.");
                        }
                    }
                    responseList.Add(messageText.ToString());
                }

                return responseList;
            }
            catch (Exception ex)
            {
                return new List<string> { $"Sorry! I am unable to generate a report at this time." };
            }
        }

        public void EndSession(string sessionId)
        {
            if (_sessionThreads.ContainsKey(sessionId))
            {
                _sessionThreads.Remove(sessionId);
            }
        }
    }
}
