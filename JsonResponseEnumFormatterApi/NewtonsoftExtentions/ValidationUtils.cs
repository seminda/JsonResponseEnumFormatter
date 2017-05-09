using System;

namespace JsonResponseEnumFormatterApi.NewtonsoftExtentions
{
    internal static class ValidationUtils
    {
        public static void ArgumentNotNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
