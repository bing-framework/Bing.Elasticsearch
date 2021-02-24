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
            Client = new ElasticsearchClient(new ElasticsearchConfigProvider(new ElasticsearchConfig
            {
                Urls = "http://10.186.132.138:9200"
            }));
        }        
    }
}
