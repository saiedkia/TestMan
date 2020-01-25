using BulkTest.Models;
using BulkTest.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BulkTest.Converters
{
    public class PathConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            List<Definition> definitions = new List<Definition>();
            DeserializeDefinitions(jObject["definitions"], definitions);


            JToken jPaths = jObject["paths"];
            Path path = new Path
            {
                Name = jObject["info"]?["title"]?.ToString() ?? Guid.NewGuid().ToString()
            };

            foreach (JProperty prop in jPaths.Children())
            {
                API api = JsonConvert.DeserializeObject<API>("{" + prop.ToString() + "}");
                MapParametersToDefinitions(api.Parameters, definitions);

                path.Paths.Add(api);
            }

            return path;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        private void DeserializeDefinitions(JToken jObject, in List<Definition> definitions)
        {
            foreach (JProperty jDefinition in jObject.Children())
            {
                Definition definition = new Definition
                {
                    Name = jDefinition.Name
                };

                IEnumerable<JToken> propertise = jDefinition.First["properties"].Children();
                IEnumerable<JToken> requiredFields = jDefinition.First["required"]?.Children();

                foreach (JProperty propertie in propertise)
                {
                    JToken firstElement = propertie.First;

                    string typeName = (firstElement["items"]?.First as JProperty)?.Value.GetLastString('/') ?? propertie.First["$ref"]?.GetLastString('/') ?? firstElement["type"]?.ToString();

                    definition.Propertise.Add(new DefinitionProperty() { Key = propertie.Name, Value = firstElement["type"]?.ToString(), IsReadOnly = ((bool?)firstElement["readOnly"] ?? false), TypeName = typeName, IsRequired = IsRequired(requiredFields, propertie.Name) });
                }

                definitions.Add(definition);
            }
        }

        private bool IsRequired(IEnumerable<JToken> requireds, string name)
        {
            return requireds?.Where(x => x.ToString() == name).Count() > 0;
        }

        private void MapParametersToDefinitions(List<Parameter> parameters, List<Definition> definitions)
        {
            foreach (Parameter item in parameters)
            {
                if (item.DefinitionType != null)
                {
                    Definition definition = definitions.Where(x => x.Name == item.DefinitionType).FirstOrDefault();
                    if (definition != null)
                    {
                        if (item.SubTypeParameters == null) item.SubTypeParameters = new List<Parameter>();

                        item.SubTypeParameters.AddRange(Definition.ToParameters(definition, definitions, item.In));
                        MapParametersToDefinitions(item.SubTypeParameters, definitions);
                    }
                }
                else if (item.SubTypeParameters?.Count > 0) MapParametersToDefinitions(item.SubTypeParameters, definitions);
            }
        }
    }
}
