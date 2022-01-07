using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Bing.Elasticsearch.Options;
using Bing.Elasticsearch.Provider;
using Bing.Elasticsearch.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Bing.Elasticsearch.WinformSample
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var serviceProvider = GetServiceProvider();
            using var scope=serviceProvider.CreateScope();
            Application.Run(scope.ServiceProvider.GetService<Form1>());
        }

        static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddOptions().Configure<ElasticsearchOptions>(x =>
            {
                x.Urls = new List<string>
                {
                    "http://10.186.132.138:9200"
                };
                x.DefaultIndex = "bing_es_sample";
            });
            services.AddSingleton<IElasticClientProvider, ElasticClientProvider>();
            services.AddScoped(typeof(IEsRepository<>), typeof(EsRepositoryBase<>));

            services.AddSingleton(typeof(Form1));
            return services.BuildServiceProvider();
        }
    }
}
