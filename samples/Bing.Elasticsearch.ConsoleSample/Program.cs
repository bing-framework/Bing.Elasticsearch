using System;
using System.Threading.Tasks;
using Bing.Elasticsearch.Configs;
using Bing.Elasticsearch.ConsoleSample.Samples;
using FreeSql;

namespace Bing.Elasticsearch.ConsoleSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Write("启动客户端");
            var context = new SampleContext();
            context.Orm =
                GetFreeSql(
                    "server=192.168.239.50;port=3306;database=;uid=report;pwd=;charset='utf8mb4';Allow User Variables=true;Connection Timeout=300;default command timeout=300;SslMode=none");
            context.ESClient = GetESClient();

            await SaveInOutStockProductReportAct.ExecuteAsync(context);
            //await QueryInOutStockProductReportAct.ExecuteAsync(context);
            Log.Write("执行完毕");
            Console.ReadLine();
        }

        /// <summary>
        /// 获取FreeSQL
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        private static IFreeSql GetFreeSql(string connectionString)
        {
            var fsql = new FreeSql.FreeSqlBuilder()
                .UseAutoSyncStructure(false)
                .UseNoneCommandParameter(true)
                .UseConnectionString(DataType.MySql, connectionString)
                //.UseMonitorCommand(cmd => Console.WriteLine(cmd.CommandText))
                .Build();
            return fsql;
        }

        /// <summary>
        /// 获取ES客户端
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static IElasticsearchClient GetESClient()
        {
            return new ElasticsearchClient(new ElasticsearchConfigProvider(new ElasticsearchConfig()
            {
                Urls = "http://10.186.132.138:9200",
            }));
        }
    }
}
