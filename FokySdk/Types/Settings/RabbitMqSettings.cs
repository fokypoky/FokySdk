namespace FokySdk.Types.Settings
{
    /// <summary>
    /// Represents the configuration settings required to establish a connection
    /// to a RabbitMQ server. This includes details such as the host, port,
    /// credentials, virtual host, and methods to retrieve these settings from
    /// the environment.
    /// </summary>
    public class RabbitMqSettings
    {
        /// <summary>
        /// Gets or sets the hostname or IP address of the RabbitMQ server
        /// to which the application should connect. This property specifies
        /// the location of the RabbitMQ broker that will handle message
        /// processing and queuing operations.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets or sets the port number used to connect to the RabbitMQ server.
        /// This property determines the network port on which the RabbitMQ broker
        /// listens for incoming client connections.
        /// </summary>
        public ushort Port { get; set; }

        /// <summary>
        /// Gets or sets the username used for authenticating with the RabbitMQ server.
        /// This property specifies the credential required to establish a connection
        /// and perform operations on the RabbitMQ broker.
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the password used for authentication with the RabbitMQ server.
        /// This property defines the credential required to establish a secure connection
        /// to the RabbitMQ broker. Ensure that this value is properly secured and not exposed
        /// in logs or public configurations.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the virtual host used to segment and isolate messaging environments
        /// within a RabbitMQ server. This property specifies the logical grouping of resources,
        /// such as exchanges and queues, to provide a multi-tenant architecture or organize
        /// resources for specific applications or environments.
        /// </summary>
        public string Vhost { get; set; }

        /// <summary>
        /// Retrieves RabbitMQ settings from environment variables and initializes a new instance
        /// of the <see cref="RabbitMqSettings"/> class.
        /// Required environment variables: RABBIT_MQ_HOST, RABBIT_MQ_PORT, RABBIT_MQ_USER, RABBIT_MQ_PASSWORD
        /// </summary>
        /// <returns>
        /// A <see cref="RabbitMqSettings"/> instance containing the RabbitMQ configuration
        /// settings retrieved from environment variables.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if any required environment variables (RABBIT_MQ_HOST, RABBIT_MQ_PORT,
        /// RABBIT_MQ_USER, or RABBIT_MQ_PASSWORD) are not found or have null values.
        /// </exception>
        public static RabbitMqSettings GetFromEnvironment()
        {
            var host = Environment.GetEnvironmentVariable("RABBIT_MQ_HOST") ?? throw new ArgumentException("RABBIT_MQ_HOST env variable is null");
            var port = Environment.GetEnvironmentVariable("RABBIT_MQ_PORT") ?? throw new ArgumentException("RABBIT_MQ_PORT env variable is null");
            var user = Environment.GetEnvironmentVariable("RABBIT_MQ_USER") ?? throw new ArgumentException("RABBIT_MQ_USER env variable is null");
            var password = Environment.GetEnvironmentVariable("RABBIT_MQ_PASSWORD") ?? throw new ArgumentException("RABBIT_MQ_PASSWORD env variable is null");
            var vhost = Environment.GetEnvironmentVariable("RABBIT_MQ_VHOST") ?? "/";

            return new RabbitMqSettings(host, user, password, ushort.Parse(port ?? "5672"), vhost);
        }

        /// <summary>
        /// Represents the configuration settings necessary to establish a connection
        /// to a RabbitMQ server, including details such as the host, port, user credentials,
        /// and virtual host.
        /// </summary>
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
