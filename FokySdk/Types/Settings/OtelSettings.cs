namespace FokySdk.Types.Settings
{
    public class OtelSettings
    {
        public bool UseAspNetCoreInstrumentation { get; set; }
        public bool UseHttpClientInstrumentation { get; set; }
        public bool UseEntityFrameworkInstrumentation { get; set; }
        public bool UseMassTransitInstrumentation { get; set; }
    }
}