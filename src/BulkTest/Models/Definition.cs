using System.Collections.Generic;
using System.Linq;

namespace BulkTest.Models
{
    public class Definition
    {
        public Definition()
        {
            Propertise = new List<DefinitionProperty>();
        }

        public string Name { get; set; }
        public List<DefinitionProperty> Propertise { get; set; }

        public static List<Parameter> ToParameters(Definition definition, List<Definition> definitions, ParameterType parameterType)
        {
            List<Parameter> lst = new List<Parameter>();
            for (int i = 0; i < definition.Propertise.Count; i++)
            {
                DefinitionProperty property = definition.Propertise[i];

                if (string.IsNullOrEmpty(property.TypeName))
                {
                    lst.Add(new Parameter() { Name = property.Key, Type = property.Value, IsReadOnly = property.IsReadOnly, Required = property.IsRequired });
                }
                else
                {
                    Definition subDefinition = definitions.Where(x => x.Name == property.TypeName).FirstOrDefault();

                    Parameter parameter = new Parameter()
                    {
                        Name = property.Key,
                        Type = property.Value,
                        In = parameterType,
                        SubTypeParameters = subDefinition != null ? ToParameters(subDefinition, definitions, parameterType) : null,
                        Required = property.IsRequired
                    };

                    lst.Add(parameter);
                }
            }


            return lst;
        }

        //public static implicit operator List<Parameter>(Definition definition)
        //{
        //    return ToParameters(definition);
        //}
    }

    public class DefinitionProperty
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsReadOnly { get; set; }
        public string TypeName { get; set; }
        public bool IsRequired { get; set; }
    }
}
