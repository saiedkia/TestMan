using BulkTest.Converters;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BulkTest.Models
{
    [JsonConverter(typeof(ApiConverter))]
    public class API
    {
        public string URL { get; set; }
        public List<Parameter> Parameters { get; set; }
        public string OperationId { get; set; }
        public RequestType RequestType { get; set; }
        public string Produces { get; set; }
        public string Category { get; set; }


        public string Name
        {
            get
            {
                return OperationId;
            }
        }
    }
}
