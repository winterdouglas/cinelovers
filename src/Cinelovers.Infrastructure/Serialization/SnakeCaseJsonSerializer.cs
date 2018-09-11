using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics.CodeAnalysis;

namespace Cinelovers.Infrastructure.Serialization
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
                CamelCaseText = false
            });
        }
    }
}
