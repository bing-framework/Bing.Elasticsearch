using System;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// Nginx日志 测试
    /// </summary>
    public class NginxLogTest:TestBase
    {
        public NginxLogTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Test_PageQueryAsync()
        {
            var result = await Client.PageQueryAsync<NginxLogModel>(null, "logstash-filebeat-*");
            Output.WriteLine(JsonConvert.SerializeObject(result));
        }
    }

    [ElasticsearchType(IdProperty = nameof(Id))]
    public class NginxLogModel
    {
        [Text(Name = "_id")]
        public string Id { get; set; }

        [Date(Name = "@timestamp")]
        public DateTime TimeStamp { get; set; }

        [Text(Name = "eshop_order_no")]
        public string EShopOrderNo { get; set; }

        [Text(Name = "uri")]
        public string Uri { get; set; }

        [Text(Name = "request_method")]
        public string RequestMethod { get; set; }

        [Number(Name = "request_time")]
        public decimal RequestTime { get; set; }

        [Text(Name = "status")]
        public string Status { get; set; }

        [Text(Name = "client_ip")]
        public string ClientIp { get; set; }

        [Text(Name = "request_body")]
        public string RequestBody { get; set; }

        [Object(Store = false)]
        public AgentModel Agent { get; set; }

        [ElasticsearchType(RelationName = "agent")]
        public class AgentModel
        {
            [Text(Name = "id")]
            public Guid Id { get; set; }

            [Text(Name = "name")]
            public string Name { get; set; }

            [Text(Name = "type")]
            public string Type { get; set; }

            [Text(Name = "version")]
            public string Version { get; set; }
        }
    }
}
