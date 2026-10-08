using System.Text.Json;
using System.Text.Json.Serialization;

namespace API.Common;

public static class ApiJson
{
    public static void Configure(JsonSerializerOptions options)
    {
        options.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }
}