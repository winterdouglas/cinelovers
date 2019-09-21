using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Cinelovers.Core.Infrastructure
{
    [ExcludeFromCodeCoverage]
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
