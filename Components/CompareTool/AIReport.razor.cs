using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using NBADemo.Services;
using NBAShared;

namespace NBADemo.Components.CompareTool
{
    public partial class AIReport
    {
        [Parameter]
        public CompareTool Parent { get; set; }
        [Parameter]
        public string PlayerName { get; set; }
        [Parameter]
        public int Season { get; set; }
        public string ReportText { get; set; }
        [Inject]
        IAIService AIService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            string prompt = $"{PlayerName} - {Season}";
            string response = (await AIService.WriteScoutingReport("session", prompt)).LastOrDefault() ?? string.Empty;
            string result = response.Contains("assistant:") ? response.Substring(response.IndexOf("assistant:") + "assistant:".Length).Trim() : response;
            ReportText = result;
        }
    }
}
