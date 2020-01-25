using BulkTest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace BulkTest.Converters
{
    public class ApiConverter : JsonConverter
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObj = JObject.Load(reader);
            API api = jObj["extraProp"] != null ? DeserializeFromJsonFileFormat(jObj) : DeserializeFromWebResponseFormat(jObj);
            //if (jObj["extraProp"] != null) api = DeserializeFromJsonFileFormat(jObj);
            //else api = DeserializeFromWebResponseFormat(jObj);

            return api;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            API api = (API)value;
            JObject jo = new JObject
            {
                ["url"] = api.URL,
                ["operationId"] = api.OperationId,
                ["parameters"] = JArray.FromObject(api.Parameters),
                ["produces"] = api.Produces,
                ["extraProp"] = "not set"
            };

            jo.WriteTo(writer);
            writer.Flush();
        }


        private API DeserializeFromJsonFileFormat(JObject jObj)
        {
            API api = new API
            {
                URL = jObj["url"].ToString(),
                OperationId = jObj["operationId"].ToString(),
                Parameters = JsonConvert.DeserializeObject<List<Parameter>>(jObj["parameters"].ToString()),
                Produces = jObj["produces"].ToString(),
                Category = jObj["tags"]?.First?.ToString()
            };

            return api;
        }

        private API DeserializeFromWebResponseFormat(JObject jObj)
        {
            JProperty jApi = (JProperty)jObj.First;
            API api = new API
            {
                URL = jApi.Name
            };

            JObject jBody = (JObject)jApi.First;

            api.RequestType = Utils.Utils.ParseEnum<RequestType>(((JProperty)jBody.First).Name);

            JToken firstElement = jBody.First.First;
            api.OperationId = firstElement["operationId"].ToString();
            api.Parameters = JsonConvert.DeserializeObject<List<Parameter>>(firstElement["parameters"].ToString());
            api.Produces = firstElement["produces"]?.First?.ToString();
            api.Category = firstElement["tags"]?.First?.ToString();

            return api;
        }
    }
}
