using BulkTest.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BulkTest.Models
{
    [JsonConverter(typeof(ParameterConverter))]
    public class Parameter
    {
        public string Name { get; set; }
        public ParameterType In { get; set; }
        public bool Required { get; set; }
        public string Type { get; set; }
        public string Format { get; set; }
        public string DefinitionType { get; set; }
        public string Value { get; set; }
        public bool IsReadOnly { get; set; }

        public List<Parameter> SubTypeParameters { get; set; }
    }
}
