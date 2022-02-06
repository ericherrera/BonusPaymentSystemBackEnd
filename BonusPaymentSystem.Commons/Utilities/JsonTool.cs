using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BonusPaymentSystem.Commons.Utilities
{
    public static class JsonTool
    {
        public static string ObjectToJsonString<T>(T tmpObject)
        {
            var options = new JsonSerializerOptions
            {
                AllowTrailingCommas = true
            };

            return JsonSerializer.Serialize(tmpObject, options);
        }

        public static T StringJsonDeserializer<T>(string content)
        {
            return JsonSerializer.Deserialize<T>(content);
        }
    }
}
