using BulkTest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace BulkTest.Utils
{
    public static class Utils
    {
        public static string ReadFile(string path, string fileName)
        {
            return File.ReadAllText(path);
        }

        public static string ReadFile(string fileName)
        {
            return File.ReadAllText(Directory.GetCurrentDirectory() + "/json/" + fileName);
        }

        public static T ReadFile<T>(string fileName)
        {
            string content = File.ReadAllText(Directory.GetCurrentDirectory() + "/json/" + fileName);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static string ReadFileRelativePath(string fileName)
        {
            string content = File.ReadAllText(fileName);
            return content;
        }


        public static string Serialize(object obj)
        {
            JsonSerializerSettings js = new JsonSerializerSettings();
            js.Converters.Clear();
            return JsonConvert.SerializeObject(obj, js);
        }

        public static T Deserialize<T>(string json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }



        public static T ConvertXmlStringtoObject<T>(string xmlString)
        {
            T classObject;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (StringReader stringReader = new StringReader(xmlString))
            {
                classObject = (T)xmlSerializer.Deserialize(stringReader);
            }
            return classObject;
        }

        public static string ConvertObjectToXMLString(object classObject)
        {
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, classObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }


        public static string Beautity(string json)
        {
            object obj = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static T ParseEnum<T>(string value) where T : Enum
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value.UppercaseFirst());
            }
            catch { }

            return (T)Enum.GetValues(typeof(T)).GetValue(0);
        }

        public static bool JsonIsValid(string json)
        {
            try
            {
                JObject.Parse(json);
                return true;
            }
            catch { }
            return false;
        }
    }


    public static class UtilsExtension
    {
        public static string UppercaseFirst(this string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }
        
        public static string GetLastString(this JToken token, char separator)
        {
            return token?.ToString().Split(separator).Last();
        }

        public static string GetlastItem(this string str, char separator)
        {
            return null;
        }

        public static string TrimLineFeed(this string str)
        {
            return str.Trim(new char[] { '\r', '\n' }).Replace("\r", "").Replace("\n", "");
        }
    }
}
