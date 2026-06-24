namespace FokySdk.Types.Settings
{
    public class RabbitMqSettings
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Vhost { get; set; }

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
