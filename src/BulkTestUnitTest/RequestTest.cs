using BulkTest.Models;
using BulkTest.Utils;
using FluentAssertions;
using Xunit;

namespace BulkTestUnitTest
{
    public class RequestTest
    {
        [Fact]
        public void Send_simple_get_request()
        {
            Request request = new Request();
            request.Api = Utils.ReadFile<API>("SingleApi.json");
            HttpGeneralRequest gr = new HttpGeneralRequest(request);

            Response response = gr.SendRequest("").Result;

            response.Body.Should().BeEmpty();
            response.StatusCode.Should().Be(401);
        }
    }
}
