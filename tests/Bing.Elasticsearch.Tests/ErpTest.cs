using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Bing.Elasticsearch.Tests
{
    public class ErpClientTest : TestBase
    {
        public ErpClientTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Test_QueryAsync()
        {
            var result = await Client.QueryAsync<ErpOpenApiModel>("logstash-filebeat-*", "eshop_order_no",
                "");
            Output.WriteLine(JsonConvert.SerializeObject(result));
        }
    }

    [ElasticsearchType]
    public class ErpOpenApiModel
    {
        public string Id { get; set; }

        [Text(Name = "uri")]
        public string Uri { get; set; }

        [Text(Name = "request_body")]
        public string RequestBody { get; set; }


        [Text(Name = "eshop_order_no")]
        public string EShopOrderNo { get; set; }
    }
}
