using System;
using JsonResponseEnumFormatterApi.NewtonsoftExtentions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonResponseEnumFormatterApi
{
    public class StringCapitalizationEnumConverter : StringEnumConverter
    {
        private readonly CapitalizationStyles _caseStyle;
        public StringCapitalizationEnumConverter(CapitalizationStyles caseStyle)
        {
            _caseStyle = caseStyle;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var e = (Enum)value;

            var enumName = e.ToString("G");
            var enumType = e.GetType();
            string finalName;
            switch (_caseStyle)
            {
                case CapitalizationStyles.CamelCase:
                    CamelCaseText = true;
                    base.WriteJson(writer, value, serializer);
                    break;
                case CapitalizationStyles.Number:
                    AllowIntegerValues = true;
                    base.WriteJson(writer, value, serializer);
                    break;
                case CapitalizationStyles.None:
                    CamelCaseText = false;
                    base.WriteJson(writer, value, serializer);
                    break;
                case CapitalizationStyles.UpperCase:
                    finalName = EnumUtils.ToEnumName(enumType, enumName, CapitalizationStyles.UpperCase);
                    writer.WriteValue(finalName);
                    break;
                case CapitalizationStyles.LowerCase:
                    finalName = EnumUtils.ToEnumName(enumType, enumName, CapitalizationStyles.LowerCase);
                    writer.WriteValue(finalName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
