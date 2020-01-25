using BulkTest.Models;
using System.Collections.Generic;

namespace UWPClient
{
    public static class Extensions
    {
        public static List<T> Append<T>(this List<T> list, List<T> items)
        {
            if (items == null) return list;

            List<T> tmpList = new List<T>();
            tmpList.AddRange(list);
            tmpList.AddRange(items);
            return tmpList;
        }

        public static List<Parameter> Clone(this List<Parameter> list)
        {
            List<Parameter> tmpParameters = new List<Parameter>();
            foreach (Parameter item in list)
            {
                Parameter parameter = new Parameter();
                parameter.Format = item.Format;
                parameter.In = item.In;
                parameter.Name = item.Name;
                parameter.DefinitionType = item.DefinitionType;
                parameter.SubTypeParameters = item.SubTypeParameters.Clone();
                parameter.Type = item.Type;
                parameter.Value = null;

                tmpParameters.Add(parameter);
            }

            return tmpParameters;
        }
    }
}
