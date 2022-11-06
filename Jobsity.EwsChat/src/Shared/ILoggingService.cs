using Serilog;

namespace Jobsity.EwsChat.Shared
{
    public interface ILoggingService
    {
        void LogDebug(string message);
        void LogError(string message, Exception exception);
        void LogInfo(string message);
        void LogWarn(string message);
    }
}
