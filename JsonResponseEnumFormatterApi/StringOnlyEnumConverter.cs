using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonResponseEnumFormatterApi
{
    public class StringOnlyEnumConverter : StringEnumConverter
    {
        public StringOnlyEnumConverter()
        {
            AllowIntegerValues = false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (AllowIntegerValues || reader.TokenType != JsonToken.String)
                return base.ReadJson(reader, objectType, existingValue, serializer);

            var s = reader.Value.ToString().Trim();

            if (string.IsNullOrEmpty(s)) return base.ReadJson(reader, objectType, existingValue, serializer);

            if (!char.IsDigit(s[0]) && s[0] != '-' && s[0] != '+')
                return base.ReadJson(reader, objectType, existingValue, serializer);

            throw new JsonSerializationException($"Value '{s}' is not allowed for enum '{objectType.Name}'.");
        }
    }
}
