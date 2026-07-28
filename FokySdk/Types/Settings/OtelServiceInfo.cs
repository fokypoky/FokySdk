namespace FokySdk.Types.Settings
{
    public class OtelServiceInfo
    {
        public string ServiceName { get; set; }
        public string GrpcEndpoint { get; set; }

        public OtelServiceInfo(string serviceName, string grpcEndpoint)
        {
            ServiceName = serviceName;
            GrpcEndpoint = grpcEndpoint;
        }

        public static OtelServiceInfo GetFromEnvironment()
        {
            var serviceName = Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? throw new ArgumentException("OTEL_SERVICE_NAME is null");
            var endpoint = Environment.GetEnvironmentVariable("OTEL_GRPC_ENDPOINT") ?? throw new ArgumentException("OTEL_GRPC_ENDPOINT is null");

            return new OtelServiceInfo(serviceName, endpoint);
        }
    }
}