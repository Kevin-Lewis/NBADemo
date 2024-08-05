namespace NBADemo.Services
{
    public interface IAIService
    {
        public Task<List<string>> WriteScoutingReport(string sessionId, string prompt);
    }
}
