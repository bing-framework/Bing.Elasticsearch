using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
            using var scope = serviceProvider.CreateScope();
            Application.Run(scope.ServiceProvider.GetService<Form1>());
        }

        static IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddElasticsearch(o =>
            {
                //o.Urls = new List<string>
                //{
                //    "http://10.186.132.138:9200"
                //};
                //o.DefaultIndex = "bing_es_sample";
                o.Urls = new List<string>
                {
                    "http://10.186.135.120:9200",
                    "http://10.186.135.125:9200",
                    "http://10.186.135.135:9200",
                };
            });

            services.AddSingleton(typeof(Form1));
            return services.BuildServiceProvider();
        }
    }
}
