using System.Collections.Generic;
using Bing.Elasticsearch.Configs;
using Xunit.Abstractions;

namespace Bing.Elasticsearch.Tests
{
    public abstract class TestBase
    {
        protected IElasticsearchClient Client;

        protected ITestOutputHelper Output;

        protected TestBase(ITestOutputHelper output)
        {
            Output = output;
            Client = new ElasticsearchClient(new ElasticsearchConfigProvider(new ElasticsearchConfig()
            {
                Nodes = new List<ElasticsearchNode>()
                {
                    new ElasticsearchNode() {Host = "192.168.0.254", Port = 9200},
                    new ElasticsearchNode() {Host = "192.168.0.254", Port = 9201},
                }
            }));
        }        
    }
}
