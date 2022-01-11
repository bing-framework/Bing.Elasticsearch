using System;
using System.Threading.Tasks;
using Bing.Elasticsearch.ConsoleSample.ESModel;
using Bing.Elasticsearch.ConsoleSample.Model.Reports;
using Bing.Extensions;
using Bing.Utils.Json;

namespace Bing.Elasticsearch.ConsoleSample.Samples
{
    /// <summary>
    /// 查询 - 出入库产品报表
    /// </summary>
    public static class QueryInOutStockProductReportAct
    {
        /// <summary>
        /// 执行
        /// </summary>
        public static async Task ExecuteAsync(SampleContext context)
        {
            var beginTime = DateTime.Parse("2021-11-01 00:00:00");
            //var indexName = $"test_{nameof(InOutStockProductReport).ToSnakeCase()}";
            //var indexName = $"test_{nameof(InOutStockProductReport).ToSnakeCase()}_{beginTime:yyyy.MM.dd}";
            var indexName = $"test_{nameof(InOutStockProductReport).ToSnakeCase()}_*";

            var result =
                await context.ESContext.GetAllAsync<InOutStockProductReportEo>(indexName);

            foreach (var item in result) Log.Write(JsonHelper.ToJson(item));
        }
    }
}
