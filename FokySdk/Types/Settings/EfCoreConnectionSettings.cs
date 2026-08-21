namespace FokySdk.Types.Settings
{
    public class EfCoreConnectionSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string Schema { get; set; }
            
        public static EfCoreConnectionSettings GetFromEnvironment()
        {
            var host = Environment.GetEnvironmentVariable("PG_HOST") ?? throw new ArgumentException("PG_HOST env variable is null");
            var port = Environment.GetEnvironmentVariable("PG_PORT") ??  throw new ArgumentException("PG_PORT env variable is null");
            var user = Environment.GetEnvironmentVariable("PG_USER") ??  throw new ArgumentException("PG_USER env variable is null");
            var password = Environment.GetEnvironmentVariable("PG_PASSWORD") ??  throw new ArgumentException("PG_PASSWORD env variable is null");
            var database = Environment.GetEnvironmentVariable("PG_DATABASE") ??  throw new ArgumentException("PG_DATABASE env variable is null");
            var schema = Environment.GetEnvironmentVariable("PG_SCHEMA") ?? "public";

            return new EfCoreConnectionSettings()
            {
                Host = host,
                Port = int.Parse(port),
                User = user,
                Password = password,
                Database = database,
                Schema = schema
            };
        }

        public string ToConnectionString()
        {
            return $"Host={Host};Port={Port};Database={Database};Username={User};Password={Password};IncludeErrorDetail=true;Search Path={Schema}";
        }
    }
}