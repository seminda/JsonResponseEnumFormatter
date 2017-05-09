using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace JsonResponseEnumFormatterApi
{
    public class JsonResponseEnumFormatterAttribute : Attribute, IControllerConfiguration
    {
        private readonly CapitalizationStyles _caseStyle;

        public JsonResponseEnumFormatterAttribute(CapitalizationStyles caseStyle)
        {
            _caseStyle = caseStyle;
        }
        public void Initialize(HttpControllerSettings controllerSettings, HttpControllerDescriptor controllerDescriptor)
        {
            var jsonMediaTypeFormatter = controllerSettings.Formatters.OfType<JsonMediaTypeFormatter>().Single();
            controllerSettings.Formatters.Remove(jsonMediaTypeFormatter);

            jsonMediaTypeFormatter = new JsonMediaTypeFormatter
            {
                SerializerSettings =
               {
                   ContractResolver = new CamelCasePropertyNamesContractResolver(),
                  Converters = new List<JsonConverter> { new StringCapitalizationEnumConverter(_caseStyle) }
               }
            };
            controllerSettings.Formatters.Add(jsonMediaTypeFormatter);
        }
    }
}
