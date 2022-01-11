using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bing.Elasticsearch.ConsoleSample.Samples;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;

namespace Bing.Elasticsearch.ConsoleSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Write("启动客户端");

            var serviceProvider = GetServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<SampleContext>();
            //await SaveInOutStockProductReportAct.ExecuteAsync(context);
            await QueryInOutStockProductReportAct.ExecuteAsync(context);

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

        static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddElasticsearch(o =>
            {
                o.Urls = new List<string>
                {
                    "http://10.186.132.138:9200"
                };
            });
            services.AddSingleton(GetFreeSql(
                "server=192.168.239.50;port=3306;database=;uid=report;pwd=;charset='utf8mb4';Allow User Variables=true;Connection Timeout=300;default command timeout=300;SslMode=none"));

            services.AddSingleton<SampleContext>();
            return services.BuildServiceProvider();
        }
    }
}
