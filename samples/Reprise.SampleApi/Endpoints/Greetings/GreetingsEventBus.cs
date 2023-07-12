namespace Reprise.SampleApi.Endpoints.Greetings
{
    public static class GreetingsEventBus
    {
        public static IReadOnlyList<string> Log => _Log;

        private static readonly List<string> _Log = new();

        public static void LogInformation(string message)
        {
            _Log.Add(message);
        }

        public static void ClearLog()
        {
            _Log.Clear();
        }
    }
}
