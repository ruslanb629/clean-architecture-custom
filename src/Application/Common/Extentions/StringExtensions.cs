using Application.Common.Resolver;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.Json;

namespace Application.Common.Extentions;

public static class StringExtensions
{
    public static T ToLogJson<T>(this T obj)
    {
        var serializedString = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
        {
            ContractResolver = new JsonIgnoreResolver(),
            NullValueHandling = NullValueHandling.Ignore
        });

        return JsonConvert.DeserializeObject<T>(serializedString);
    }

    public static string ToJson<T>(this T obj)
    {
        return System.Text.Json.JsonSerializer.Serialize(obj,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
    }

    public static DateTime? ToDateTime(this string date, string format = "yyyy-MM-ddTHH:mm:ss.fffZ")
    {
        if (string.IsNullOrEmpty(date))
            return null;

        if (date.Length == 10)
            return DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        return DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
    }
}