using NLog;

namespace AE.AuditPlanning.Common.Logging
{
    public static class Logger
    {
        public static void LogDebug(string logger, string message, params object[] args)
        {
            LogManager.GetLogger(logger).Log(LogLevel.Debug, message, args);
        }

        public static void LogInfo(string logger, string message, params object[] args)
        {
            LogManager.GetLogger(logger).Log(LogLevel.Debug, message, args);
        }
    }
}
