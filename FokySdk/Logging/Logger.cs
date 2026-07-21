using System.Text;
using FokySdk.Types.Settings;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace FokySdk.Logging
{
    public class Logger : ILogger
    {
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICollection<string> _excludedStrings;

        public Logger(LoggerSettings settings)
        {
            _excludedStrings = settings.ExcludedStrings;
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
            _logger.Info(PrepareMessage(message));
        }

        public void LogWarning(string message)
        {
            _logger.Warn(PrepareMessage(message));
        }

        public void LogError(string message)
        {
            _logger.Error(PrepareMessage(message));
        }

        private string PrepareMessage(string message)
        {
            var result = new StringBuilder(message);

            foreach (var excludedString in _excludedStrings)
            {
                result.Replace(excludedString, "*");
            }

            return result.ToString();
        }
    }
}
