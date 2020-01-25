using BulkTest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace UWPClient.Models
{
    public class ParameterToViewDto : INotifyPropertyChanged
    {
        public string Name { get; set; }

        private string _value;
        public string Value { get => _value;
            set { _value = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value))); if (Parameter != null) Parameter.Value = value; } }

        public Parameter Parameter { get; set; }

        public Guid Id { get; set; }

        public int Intent { get; set; }

        public ParameterToViewDto(Parameter parameter, int intent = 0)
        {
            Parameter = parameter;
            Name = parameter.Name;
            Value = parameter.Value;
            Intent = intent * 10;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<ParameterToViewDto> ToList(List<Parameter> parameters, int intent = 0)
        {
            List<ParameterToViewDto> lst = new List<ParameterToViewDto>();
            foreach (Parameter p in parameters)
            {
                if (p.SubTypeParameters?.Count > 0)
                {
                    lst.Add(new ParameterToViewDto(p, intent));
                    lst.AddRange(ToList(p.SubTypeParameters, intent + 1));
                }
                else
                    lst.Add(new ParameterToViewDto(p, intent));
            }

            return lst;
        }

    }
}
