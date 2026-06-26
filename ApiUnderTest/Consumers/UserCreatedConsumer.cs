using MassTransit;
using MeepleHub.EventContracts;

namespace MeepleHub.EventContracts
{
    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "14.7.1.0 (NJsonSchema v11.6.1.0 (Newtonsoft.Json v13.0.0.0))")]
    public partial record UserCreatedEvent
    {

        /// <summary>
        /// ID пользователя
        /// </summary>
        [Newtonsoft.Json.JsonProperty("id", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.Guid Id { get; set; } = default!;

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [Newtonsoft.Json.JsonProperty("login", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public string Login { get; set; } = default!;

        /// <summary>
        /// Дата регистрации пользователя
        /// </summary>
        [Newtonsoft.Json.JsonProperty("created", Required = Newtonsoft.Json.Required.Always)]
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
        public System.DateTime Created { get; set; } = default!;

    }
}

namespace ApiUnderTest.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            return Task.CompletedTask;
        }
    }
}
