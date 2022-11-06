using Serilog;

namespace Jobsity.EwsChat.Shared
{
    public class LoggingService : ILoggingService
    {
        public void LogDebug(string message) => Log.Debug(message);
        public void LogError(string message, Exception exception) => Log.Error(message, exception);
        public void LogInfo(string message) => Log.Information(message);
        public void LogWarn(string message) => Log.Warning(message);
    }
}
