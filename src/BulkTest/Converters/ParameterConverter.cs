using BulkTest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace BulkTest.Converters
{
    public class ParameterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            Parameter parameter = new Parameter
            {
                In = jObject["in"].ToObject<ParameterType>(),
                Name = jObject["name"].ToString(),
                Required = jObject["required"].ToObject<bool>(),
                Type = jObject["type"]?.ToString(),

                DefinitionType = (jObject["schema"]?.First as JProperty)?.Value.ToString().Split('/')?.Last()
            };

            return parameter;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Parameter parameter = (Parameter)value;
            JObject jParameter = new JObject();
            JObject jParameterSchema = new JObject
            {
                ["v1"] = parameter.DefinitionType
            };

            jParameter["in"] = (int)parameter.In;
            jParameter["name"] = parameter.Name;
            jParameter["required"] = parameter.Required;
            jParameter["type"] = parameter.Type;
            jParameter["schema"] = jParameterSchema;

            jParameter.WriteTo(writer);
            writer.Flush();
        }
    }
}
