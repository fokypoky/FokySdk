using FokySdk.Types.Settings;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace FokySdk.Logging
{
    public class Logger : ILogger
    {
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        public Logger(LoggerSettings settings)
        {
            var config = new LoggingConfiguration();

            if (settings.UseConsoleTarget)
            {
                var consoleTarget = new ColoredConsoleTarget(settings.ConsoleTargetName)
                {
                    Layout = settings.ConsoleLayout
                };

                config.AddRule(settings.MinLevel, settings.MaxLevel, consoleTarget);
            }

            if (settings.UseFileTarget)
            {
                var fileTarget = new FileTarget(settings.FileTargetName)
                {
                    Layout = settings.ConsoleLayout
                };

                config.AddRule(settings.MinLevel, settings.MaxLevel, fileTarget);
            }

            LogManager.Configuration = config;
        }

        public void LogInfo(string message)
        {
            _logger.Info(message);
        }

        public void LogWarning(string message)
        {
            _logger.Warn(message);
        }

        public void LogError(string message)
        {
            _logger.Error(message);
        }
    }
}
