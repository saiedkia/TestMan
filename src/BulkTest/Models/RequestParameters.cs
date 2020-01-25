using System.Collections.Generic;

namespace BulkTest.Models
{
    public class RequestParameters
    {
        public List<Parameter> Parameters { get; set; }
        public List<Parameter> ExtraParameters { get; set; }

        public int? ResponseExpectedStatusCode { get; set; } = null;
        public virtual TestResult IsPassed { get; set; }

        Response _response;
        public Response Response
        {
            get => _response;
            set
            {
                _response = value;
                if (ResponseExpectedStatusCode == null || _response.StatusCode == 0)
                    IsPassed = TestResult.None;
                else if (_response.StatusCode == ResponseExpectedStatusCode)
                    IsPassed = TestResult.Passed;
                else
                    IsPassed = TestResult.Failed;
            }
        }

        public virtual string RawValue { get; set; }
        public virtual long ResponseTime { get; set; }

        public RequestParameters()
        {
            Parameters = new List<Parameter>();
            ExtraParameters = new List<Parameter>();
        }

        public void AddParameter(Parameter parameter)
        {
            if (Parameters == null) Parameters = new List<Parameter>();
            Parameters.Add(parameter);
        }

        public void AddParameter(string key, string value, ParameterType parameterType = ParameterType.Body)
        {
            Parameters.Add(new Parameter()
            {
                Name = key,
                Value = value,
                In = parameterType
            });
        }

        public void AddExtraParameter(string key, string value, ParameterType parameterType = ParameterType.Body)
        {
            if (ExtraParameters == null) ExtraParameters = new List<Parameter>();
            ExtraParameters.Add(new Parameter()
            {
                Name = key,
                Value = value,
                In = parameterType
            });
        }
    }
}
