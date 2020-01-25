using BulkTest.Models;
using BulkTest.Utils;
using FluentAssertions;
using Xunit;

namespace BulkTestUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Deserialize_single_api()
        {
            string json = Utils.ReadFile("SingleApi.json");
            API api = Newtonsoft.Json.JsonConvert.DeserializeObject<API>(json);
            api.Should().NotBeNull();
        }


        [Fact]
        public void Deserialize_whole_json()
        {
            string json = Utils.ReadFile("SwaggerJson.json");
            Path path = Newtonsoft.Json.JsonConvert.DeserializeObject<Path>(json);
            path.Should().NotBeNull();
            path.Paths.Count.Should().BeGreaterThan(10);
        }
    }
}
