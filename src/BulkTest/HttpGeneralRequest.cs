using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BulkTest.Models
{
    public class HttpGeneralRequest
    {
        public Request Request { private set; get; }
        HttpClient client;
        string query = "";
        public HttpGeneralRequest(Request request)
        {
            this.Request = request;
        }

        public async Task<Response> SendRequest(string baseUrl)
        {
            query = "";
            client = new HttpClient();
            string url = baseUrl + Request.Api.URL;
            HttpResponseMessage response = null;

            JObject body;
            if (Request.RawValue != null)
            {
                AddParameters(Request.Parameters, false);
                body = JObject.Parse(Request.RawValue);
            }
            else
                body = AddParameters(Request.Parameters, true);

            switch (Request.Api.RequestType)
            {
                case RequestType.Get:
                    {
                        response = await client.GetAsync(url + (query.Length > 2 ? ("?" + query) : ""));
                        break;
                    }
                case RequestType.Post:
                    {
                        StringContent content = new StringContent(Utils.Utils.Serialize(body), Encoding.UTF8, ContentType.ApplicationJson);
                        response = await client.PostAsync(url, content);
                        break;
                    }

                case RequestType.Put:
                    {
                        StringContent content = new StringContent(Utils.Utils.Serialize(body), Encoding.UTF8, ContentType.ApplicationJson);
                        response = await client.PutAsync(url, content);
                        break;
                    }
                case RequestType.Delete:
                    {
                        response = await client.DeleteAsync(url);
                        break;
                    }
            }

            if (response == null) return new Response() { Body = "Invalid request, 'internal app error!!!!'", StatusCode = -100 };
            return new Response() { Body = await response?.Content.ReadAsStringAsync(), StatusCode = (int)(response?.StatusCode ?? 0) };
        }

        public static async Task<Response> GetJson(string url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);

            return new Response() { Body = await response.Content.ReadAsStringAsync(), StatusCode = (int)response.StatusCode };
        }

        private JObject AddParameters(List<Parameter> parameters, bool parseBody)
        {
            JObject body = new JObject();
            StringBuilder query = new StringBuilder();

            foreach (Parameter item in parameters)
            {
                if (item.Name == "model")
                {
                    if (item.SubTypeParameters.Count > 0)
                        body = new JObject(AddParameters(item.SubTypeParameters, parseBody));
                    else
                        body = new JObject(item.Value);

                    continue;
                }

                switch (item.In)
                {
                    case ParameterType.Body:
                        {
                            if (!parseBody) continue;


                            if (item.SubTypeParameters?.Where(x => !x.IsReadOnly).Count() > 0)
                                if (item.Type == "array")
                                    body.Add(new JProperty(item.Name, new JArray(AddParameters(item.SubTypeParameters, parseBody))));
                                else
                                    body.Add(item.Name, AddParameters(item.SubTypeParameters, parseBody));
                            else
                            {
                                if (item.Type == "array")
                                    body.Add(new JProperty(item.Name, new JArray(item.Value?.Split(','))));
                                else
                                    body.Add(item.Name, item.Value);
                            }
                            break;
                        }
                    case ParameterType.Header:
                        {
                            if (item.Value != null)
                                client.DefaultRequestHeaders.Add(item.Name, item.Value);
                            break;
                        }
                    case ParameterType.Query:
                        {
                            query.Append(item.Name + "=" + (item.Value ?? ""));
                            break;
                        }
                    case ParameterType.FormData:
                        {
                            break;
                        }
                }
            }

            this.query = query.ToString();
            return body;
        }

        private JObject ToObject(List<Parameter> parameters)
        {
            JObject body = new JObject();

            foreach (Parameter item in parameters)
                if (item.Type == "array")
                {
                    JArray jArray = new JArray();
                    foreach (string str in item.Value.Split(','))
                        jArray.Add(str);

                    body[item.Name] = jArray;
                }
                else
                    body[item.Name] = item.Value;


            return body;
        }
    }
}
