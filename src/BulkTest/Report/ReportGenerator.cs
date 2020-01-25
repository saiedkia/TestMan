using BulkTest.Models;
using System.Collections.Generic;
using System.Text;

namespace BulkTest.Report
{
    public static class ReportGenerator
    {
        public static string Generate(Response response, Request request, long elapsedTime, string name, string rawBody)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (response == null || request?.Api == null) return "Invalid response/request!!";

            stringBuilder.AppendLine("Request: " + name);
            stringBuilder.AppendLine("Url: " + request.Api.URL);

            stringBuilder.AppendLine("Headers/Parameters: ");
            stringBuilder.AppendLine(GenerateParametersReportView(request.Parameters));

            if (!string.IsNullOrEmpty(rawBody))
            {
                stringBuilder.AppendLine("rawBody:");
                stringBuilder.AppendLine(rawBody);
            }

            stringBuilder.AppendLine();
            stringBuilder.AppendLine("Response:");
            stringBuilder.AppendLine($"Elapsed Time: {elapsedTime} ms");
            stringBuilder.AppendLine($"StatusCode: {response.StatusCode}");
            stringBuilder.AppendLine($"Body: {response.Body}");

            return stringBuilder.ToString();
        }

        private static string GenerateParametersReportView(List<Parameter> parameters, int depth = 0)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Parameter item in parameters)
            {
                string tmpValue = $"{new string(' ', depth)}in:{item.In} - {item.Name} : {item.Value}";
                builder.AppendLine(tmpValue);
                if (item.SubTypeParameters?.Count > 0)
                    builder.Append(GenerateParametersReportView(item.SubTypeParameters, tmpValue.Length + 3));
            }

            return builder.ToString();
        }
    }
}
