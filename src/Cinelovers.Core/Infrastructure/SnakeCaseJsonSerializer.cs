using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Cinelovers.Core.Infrastructure
{
    public class SnakeCaseJsonSerializer : JsonSerializer
    {
        public SnakeCaseJsonSerializer()
        {
            ContractResolver = new SnakeCaseContractResolver();
            Converters.Add(new StringEnumConverter
            {
                AllowIntegerValues = true,
                NamingStrategy = new CamelCaseNamingStrategy()
            });
        }
    }
}
