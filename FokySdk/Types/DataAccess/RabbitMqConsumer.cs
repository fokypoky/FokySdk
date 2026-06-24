namespace FokySdk.Types.DataAccess
{
    public class RabbitMqConsumer
    {
        public Type ConsumerType { get; set; }
        public string Queue { get; set; }
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public ExchangeType ExchangeType { get; set; }
    }
}
