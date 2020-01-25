using System.Collections.Generic;
using BulkTest.Converters;
using Newtonsoft.Json;

namespace BulkTest.Models
{
    [JsonConverter(typeof(PathConverter))]
    public class Path
    {
        public Path()
        {
            Paths = new List<API>();
        }

        public List<API> Paths { get; set; }
        public string Name { get; set; }
    }
}
