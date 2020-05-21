using Newtonsoft.Json.Serialization;
using System.Text.RegularExpressions;

namespace Cinelovers.Core.Infrastructure
{
    public class SnakeCaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return GetSnakeCase(propertyName);
        }

        private static string GetSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var buffer = input;

            buffer = Regex.Replace(buffer, @"([A-Z]+)([A-Z][a-z])", "$1_$2");
            buffer = Regex.Replace(buffer, @"([a-z\d])([A-Z])", "$1_$2");
            buffer = Regex.Replace(buffer, @"-", "_");
            buffer = Regex.Replace(buffer, @"\s", "_");
            buffer = Regex.Replace(buffer, @"__+", "_");
            buffer = buffer.ToLowerInvariant();

            return buffer;
        }
    }
}
