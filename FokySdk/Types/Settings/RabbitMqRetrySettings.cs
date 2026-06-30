namespace FokySdk.Types.Settings
{
    public class RabbitMqRetrySettings
    {
        public int RetryCount { get; set; }
        public TimeSpan Interval { get; set; }
    }
}