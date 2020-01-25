using BulkTest.Models;
using System;
using System.Collections.Generic;

namespace UWPClient.Models
{
    public class ApiItem
    {
        public API Api { get; set; }
        public List<RequestParameterBindable> Parameters { get; set; }
        public string Response { get; set; }
        public string BaseUrl { get; set; }

        public ApiItem()
        {

        }

        public ApiItem(API api, List<RequestParameterBindable> parameters)
        {
            Api = api;
            Parameters = parameters ?? throw new NullReferenceException();
        }

        public ApiItem(API api)
        {
            Api = api;
            Parameters = new List<RequestParameterBindable>();
        }

        public ApiItem(API api, List<Parameter> parameters)
        {
            Api = api;
            Parameters = new List<RequestParameterBindable>();
            Parameters.Add(new RequestParameterBindable() { Parameters = parameters });
        }

        public void AddParameter(RequestParameterBindable parameters)
        {
            Parameters.Add(parameters);
        }

        public static List<ApiItem> Convert(Path path)
        {
            List<ApiItem> tmpList = new List<ApiItem>();
            foreach (API item in path.Paths)
                tmpList.Add(new ApiItem(item, item.Parameters));

            return tmpList;
        }
    }
}
