using System;
using System.Globalization;
using System.Reflection;

namespace JsonResponseEnumFormatterApi.NewtonsoftExtentions
{
    class EnumUtils
    {
        private static readonly ThreadSafeStore<Type, BidirectionalDictionary<string, string>> EnumMemberNamesPerType = new ThreadSafeStore<Type, BidirectionalDictionary<string, string>>(InitializeEnumType);

        private static BidirectionalDictionary<string, string> InitializeEnumType(Type type)
        {
            BidirectionalDictionary<string, string> map = new BidirectionalDictionary<string, string>(
                StringComparer.Ordinal,
                StringComparer.Ordinal);

            foreach (FieldInfo f in type.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                string n1 = f.Name;
                string n2;

#if HAVE_DATA_CONTRACTS
                n2 = f.GetCustomAttributes(typeof(EnumMemberAttribute), true)
                    .Cast<EnumMemberAttribute>()
                    .Select(a => a.Value)
                    .SingleOrDefault() ?? f.Name;
#else
                n2 = f.Name;
#endif

                string s;
                if (map.TryGetBySecond(n2, out s))
                {
                    throw new InvalidOperationException(
                        "Enum name '{0}' already exists on enum '{1}'.".FormatWith(CultureInfo.InvariantCulture, n2,
                            type.Name));
                }

                map.Set(n1, n2);
            }

            return map;
        }

        public static string ToEnumName(Type enumType, string enumText, CapitalizationStyles caseStyle)
        {
            BidirectionalDictionary<string, string> map = EnumMemberNamesPerType.Get(enumType);
            string[] names = enumText.Split(',');
            for (var i = 0; i < names.Length; i++)
            {
                string name = names[i].Trim();
                string resolvedEnumName;
                map.TryGetByFirst(name, out resolvedEnumName);
                resolvedEnumName = resolvedEnumName ?? name;
                switch (caseStyle)
                {
                    case CapitalizationStyles.UpperCase:
                        resolvedEnumName = resolvedEnumName.ToUpper(CultureInfo.InvariantCulture);
                        break;
                    case CapitalizationStyles.LowerCase:
                        resolvedEnumName = resolvedEnumName.ToLower(CultureInfo.InvariantCulture);
                        break;
                    case CapitalizationStyles.CamelCase:
                        break;
                    case CapitalizationStyles.Number:
                        break;
                    case CapitalizationStyles.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(caseStyle), caseStyle, null);
                }
                names[i] = resolvedEnumName;
            }
            string finalName = string.Join(", ", names);
            return finalName;
        }
    }
}
