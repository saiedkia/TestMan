using System.Collections.Generic;

namespace BulkTest.Models
{
    public class Request
    {
        public Request()
        {
        }

        public API Api { get; set; }
        public List<Parameter> Parameters { get; set; }
        public string ContentType { get; set; }
        public string Response { get; set; }
        public string RawValue { get; set; }
    }
}
