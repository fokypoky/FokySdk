using FokySdk.DataAccess;
using FokySdk.Types.Settings;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Tests.FakeClasses;

namespace Tests.DataAccess
{
    public class RabbitMqTests
    {
        private readonly IServiceCollection _serviceCollection;

        public RabbitMqTests()
        {
            _serviceCollection = new Mock<IServiceCollection>().Object;
        }

        [Fact]
        public void AddRabbitMq_CorrectConsumersType_ShouldBeAdded()
        {
            // Arrange
            List<Type> types =
            [
                typeof(TestConsumer)
            ];

            // Act
            _serviceCollection.AddRabbitMq(new RabbitMqSettings("host", "user", "password"), types);

            // Assert
        }
    }
}
