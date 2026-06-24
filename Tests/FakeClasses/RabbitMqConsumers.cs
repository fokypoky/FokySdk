using MassTransit;

namespace Tests.FakeClasses
{
    public class ConsumedEntity
    {
        public string Field { get; set; }
    }

    public class TestConsumer : IConsumer<ConsumedEntity>
    {
        public Task Consume(ConsumeContext<ConsumedEntity> context)
        {
            return Task.CompletedTask;
        }
    }
}
