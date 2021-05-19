using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// ES客户端测试
    /// 参考：https://www.cnblogs.com/huhangfei/p/7524886.html
    /// ES搭建参考：https://juejin.im/post/5ba4c8976fb9a05cec4da9f5
    /// </summary>
    public class ElasticsearchClientTest:TestBase
    {
        public ElasticsearchClientTest(ITestOutputHelper output) : base(output)
        {
        }

        /// <summary>
        /// 测试是否存在指定索引
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task Test_ExistsAsync()
        {
            var result = await Client.ExistsAsync("Test_CreateIndex_NotMap".ToLower());
            Assert.True(result);
        }

        /// <summary>
        /// 测试创建索引 - 不映射
        /// </summary>
        [Fact]
        public async Task Test_CreateIndexAsync_NotMap()
        {
            await Client.AddAsync("Test_CreateIndex_NotMap".ToLower());
        }

        /// <summary>
        /// 测试创建索引 - 映射对象
        /// </summary>
        [Fact]
        public async Task Test_CreateIndexAsync_Map()
        {
            await Client.AddAsync<TestModel5>("Test_CreateIndex_Map".ToLower());
        }

        /// <summary>
        /// 测试添加索引
        /// </summary>
        [Fact]
        public async Task Test_AddAsync()
        {
            var model = new TestModel5()
            {
                Id = DateTime.Now.Ticks,
                CreateTime = DateTime.Now,
                Dic = "测试,装逼,蓝瘦,凉凉",
                Name = "测试数据",
                Fvalue = 100,
                Dvalue = 200,
                State = 10
            };
            await Client.AddAsync("Test_Add".ToLower(), model);
        }

        /// <summary>
        /// 测试更新索引
        /// </summary>
        [Fact]
        public async Task Test_UpdateAsync()
        {
            var model = new TestModel5()
            {
                Id = DateTime.Now.Ticks,
                CreateTime = DateTime.Now,
                Dic = "测试,装逼,蓝瘦,凉凉,GG",
                Name = "测试数据111",
                Fvalue = 100+1,
                Dvalue = 200+3,
                State = 10+4
            };
            await Client.AddAsync("Test_Add".ToLower(), model);
        }

        /// <summary>
        /// 测试删除指定索引
        /// </summary>
        [Fact]
        public async Task Test_DeleteAsync_All()
        {
            await Client.DeleteAsync("Test_CreateIndex_NotMap".ToLower());
        }

        /// <summary>
        /// 测试删除指定索引 - 根据对象
        /// </summary>
        [Fact]
        public async Task Test_DeleteAsync_Entity()
        {
            var model = new TestModel5()
            {
                Id = 636881685170663484,
                CreateTime = DateTime.Now,
                Dic = "测试,装逼,蓝瘦,凉凉",
                Name = "测试数据",
                Fvalue = 100,
                Dvalue = 200,
                State = 10
            };
            await Client.DeleteAsync("Test_Add".ToLower(), model);
        }

        /// <summary>
        /// 测试删除指定索引 - 根据ID
        /// </summary>
        [Fact]
        public async Task Test_DeleteAsync_Id()
        {
            await Client.DeleteAsync<TestModel5>("Test_Add".ToLower(), 636881683884979391L);
        }

        /// <summary>
        /// 测试查找实体
        /// </summary>
        [Fact]
        public async Task Test_FindAsync()
        {
            var result = await Client.FindAsync<TestModel5>("Test_Add".ToLower(), 636881689933894494);
            Output.WriteLine(JsonConvert.SerializeObject(result));
            Assert.NotNull(result);
        }

        /// <summary>
        /// 测试查询
        /// </summary>
        [Fact]
        public async Task Test_QueryAsync()
        {
            var result = await Client.QueryAsync<TestModel5>("Test_Add".ToLower(), "Name", "测试数据");
            Output.WriteLine(JsonConvert.SerializeObject(result));
            Assert.NotNull(result);
        }

        /// <summary>
        /// 测试查找实体列表
        /// </summary>
        [Fact]
        public async Task Test_FindByIdsAsync()
        {
            var ids = new[] {636881689933894494, 636881689966209798};
            var result = await Client.FindByIdsAsync<TestModel5>("Test_Add".ToLower(), ids);
            Output.WriteLine(JsonConvert.SerializeObject(result));
            Assert.NotNull(result);
        }

        /// <summary>
        /// 测试分页查询
        /// </summary>
        [Fact]
        public async Task Test_PageQueryAsync()
        {
            var result = await Client.PageQueryAsync<TestModel5>(null, "Test_Add".ToLower());
            Output.WriteLine(JsonConvert.SerializeObject(result));
            Assert.NotNull(result);
        }

        /// <summary>
        /// 测试批量保存
        /// </summary>
        [Fact]
        public async Task Test_BulkSaveAsync()
        {
            var list = new List<TestModel5>()
            {
                new TestModel5()
                {
                    Id = 1,
                    CreateTime = DateTime.Now,
                    Dic = "测试,装逼,蓝瘦,凉凉,老王",
                    Name = "测试数据",
                    Fvalue = 1,
                    Dvalue = 2,
                    State = 10
                },
                new TestModel5()
                {
                    Id = 2,
                    CreateTime = DateTime.Now,
                    Dic = "测试,装逼,蓝瘦,凉凉,老张",
                    Name = "测试数据",
                    Fvalue = 2,
                    Dvalue = 3,
                    State = 14
                },
                new TestModel5()
                {
                    Id = 3,
                    CreateTime = DateTime.Now,
                    Dic = "测试,装逼,蓝瘦,凉凉,老张666",
                    Name = "测试数据",
                    Fvalue = 4,
                    Dvalue = 5,
                    State = 16
                }
            };
            await Client.BulkSaveAsync("Test_BulkSaveAsync".ToLower(), list);
        }
    }
}
