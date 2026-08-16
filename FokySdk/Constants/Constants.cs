using Newtonsoft.Json;

namespace FokySdk.Constants
{
    internal static class Constants
    {
        public static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            DateFormatString = "yyyy-MM-ddTHH:mm:ssZ"
        };
    }
}
