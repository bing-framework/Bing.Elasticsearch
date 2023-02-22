﻿using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.DependencyInjection;
using Xunit.DependencyInjection.Logging;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// 启动配置
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置主机
        /// </summary>
        public void ConfigureHost(IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((context, builder) =>
            {
            });
        }

        /// <summary>
        /// 配置服务
        /// </summary>
        public void ConfigureServices(IServiceCollection services)
        {
            // 注入日志，并配置日志级别
            services.AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace));
            services.AddElasticsearch(o =>
            {
                o.Urls = new List<string>
                {
                    //"http://10.186.103.244:9201",
                    //"http://10.186.103.244:9202",
                    //"http://10.186.103.244:9203",
                    "http://10.186.103.244:9204",
                    "http://10.186.103.244:9205",
                    "http://10.186.103.244:9206",
                    //"http://10.186.135.127:9200",
                    //"http://10.186.135.143:9200",
                    //"http://10.186.135.113:9200"
                };
                //o.DefaultIndex = "bing_es_sample";
                //o.Prefix = "bing_sample";
                o.UserName = "elastic";
                o.Password = "elastic@2022";
                o.NumberOfShards = 3;
                o.NumberOfReplicas = 1;
                o.ThrowExceptions = false;
            });
        }

        /// <summary>
        /// 配置日志提供程序
        /// </summary>
        public void Configure(ILoggerFactory loggerFactory, ITestOutputHelperAccessor accessor)
        {
            // 添加单元测试日志提供程序，并配置日志过滤
            loggerFactory.AddProvider(new XunitTestOutputLoggerProvider(accessor, (s, logLevel) => logLevel >= LogLevel.Trace));
        }
    }
}