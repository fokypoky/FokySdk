namespace FokySdk.Types.DataAccess
{
    public class RabbitMqPublisher
    {
        public string Exchange { get; set; }
        public ExchangeType ExchangeType { get; set; }
        public bool Durable { get; set; }

        public RabbitMqPublisher(string exchange, ExchangeType exchangeType = ExchangeType.Topic, bool durable = true)
        {
            Exchange = exchange;
            ExchangeType = exchangeType;
            Durable = durable;
        }
    }
}
