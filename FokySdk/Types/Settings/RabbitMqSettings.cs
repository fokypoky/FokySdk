namespace FokySdk.Types.Settings
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Vhost { get; set; }

        public static RabbitMqSettings GetFromEnvironment()
        {
            var host = Environment.GetEnvironmentVariable("RABBIT_MQ_HOST") ?? throw new ArgumentException("RABBIT_MQ_HOST env variable is null");
            var port = Environment.GetEnvironmentVariable("RABBIT_MQ_PORT") ?? throw new ArgumentException("RABBIT_MQ_PORT env variable is null");
            var user = Environment.GetEnvironmentVariable("RABBIT_MQ_USER") ?? throw new ArgumentException("RABBIT_MQ_USER env variable is null");
            var password = Environment.GetEnvironmentVariable("RABBIT_MQ_PASSWORD") ?? throw new ArgumentException("RABBIT_MQ_PASSWORD env variable is null");
            var vhost = Environment.GetEnvironmentVariable("RABBIT_MQ_VHOST") ?? "/";

            return new RabbitMqSettings(host, user, password, ushort.Parse(port ?? "5672"), vhost);
        }

        public RabbitMqSettings(string host, string user, string password, ushort port = 5672, string vhost = "/")
        {
            Host = host;
            Port = port;
            User = user;
            Password = password;
            Vhost = vhost;
        }
    }
}
