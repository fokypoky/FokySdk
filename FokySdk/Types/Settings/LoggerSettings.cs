using NLog;

namespace FokySdk.Types.Settings
{
    public class LoggerSettings
    {
        public string ConsoleLayout { get; set; } = "${time} ${level} ${message}";
        public string FileLayout { get; set; } = "${longdate} ${level} ${message} ${exception}";
        public string FileName { get; set; } = "app.log";
        public bool UseConsoleTarget { get; set; } = true;
        public string ConsoleTargetName { get; set; } = "console";
        public bool UseFileTarget { get; set; } = true;
        public string FileTargetName { get; set; } = "file";
        public LogLevel MinLevel { get; set; } = LogLevel.Info;
        public LogLevel MaxLevel { get; set; } = LogLevel.Fatal;

        public ICollection<string> ExcludedStrings { get; set; }
    }
}
