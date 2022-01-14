using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bing.Elasticsearch.Repositories;
using Bing.Elasticsearch.Tests.Models;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using Xunit;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// ES仓储测试
    /// </summary>
    public class EsRepositoryTest
    {
        /// <summary>
        /// 服务提供程序
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        private readonly List<StudentSample> _students = new List<StudentSample>
        {
            new StudentSample
            {
                StudentId = 10,
                Name = "张三",
                Age = 21,
                Address = "北京市",
                BirthDay = new DateTime(2000, 1, 5),
                IsValid = true
            },
            new StudentSample
            {
                StudentId = 20,
                Name = "李四",
                Age = 22,
                Address = "杭州市",
                BirthDay = new DateTime(2000, 2, 5),
                IsValid = false
            },
            new StudentSample
            {
                StudentId = 30,
                Name = "王五",
                Age = 23,
                Address = "太原市",
                BirthDay = new DateTime(2000, 3, 5),
                IsValid = true
            },
            new StudentSample
            {
                StudentId = 40,
                Name = "王六一",
                Age = 24,
                Address = "太原市",
                BirthDay = new DateTime(2000, 4, 5),
                IsValid = false
            }
        };

        public EsRepositoryTest()
        {
            ServiceProvider = GetServiceProvider();
        }

        protected IServiceProvider GetServiceProvider()
        {
            var services = new ServiceCollection();
            services.AddElasticsearch(o =>
            {
                o.Urls = new List<string>
                {
                    "http://10.186.135.120:9200",
                    "http://10.186.135.125:9200",
                    "http://10.186.135.135:9200",
                };
                o.DefaultIndex = "bing_es_sample";
                o.Prefix = "bing_sample";
            });
            return services.BuildServiceProvider();
        }

        [Fact]
        public void Test_GetEsRepository()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            Assert.NotNull(resp);

            Assert.IsType<EsRepository<StudentSample>>(resp);
        }

        [Fact]
        public async Task Test_InsertAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            var student = _students.First();
            await resp.InsertAsync(student);

            var result = await resp.FindByIdAsync(student.StudentId);
            Assert.Equal(student.StudentId, result.StudentId);
            await resp.DeleteAsync(student.StudentId);
        }

        [Fact]
        public async Task Test_InsertManyAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            await resp.InsertManyAsync(_students);

            var result = await resp.FindByIdsAsync(_students.Select(x => x.StudentId));
            Assert.Equal(_students.Count, result.Count());
            foreach (var student in _students)
                await resp.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_BulkAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            await resp.InsertManyAsync(_students);
            foreach (var student in _students) 
                student.Name = "隔壁老王";
            await resp.BulkAsync(_students);

            var result = await resp.FindByIdsAsync(_students.Select(x => x.StudentId));
            Assert.Equal(_students.First().Name, result.First().Name);
            foreach (var student in _students)
                await resp.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_DeleteAsync_Id()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            var student = _students.First();
            await resp.InsertAsync(student);
            await resp.DeleteAsync(student.StudentId);

            var result = await resp.FindByIdAsync(student.StudentId);
            Assert.Null(result);
        }

        [Fact]
        public async Task Test_DeleteAsync_Entity()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            var student = _students.First();
            await resp.InsertAsync(student);
            await resp.DeleteAsync(student);

            var result = await resp.FindByIdAsync(student.StudentId);
            Assert.Null(result);
        }

        [Fact]
        public async Task Test_DeleteByQueryAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            var student = _students.First();
            await resp.InsertAsync(student);

            // 需要延迟一下
            await Task.Delay(1000);

            var descriptor = new DeleteByQueryDescriptor<StudentSample>();
            descriptor.Query(q => q
                .Term(r => r
                    .Field(f => f.StudentId)
                    .Value(student.StudentId)));
            await resp.DeleteByQueryAsync(descriptor);

            var result = await resp.FindByIdAsync(student.StudentId);
            Assert.Null(result);
        }

        [Fact]
        public async Task Test_UpdateAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            var student = _students.First();
            await resp.InsertAsync(student);
            student.Name = "隔壁老王666";
            await resp.UpdateAsync(student);

            var result = await resp.FindByIdAsync(student.StudentId);
            Assert.Equal(student.Name, result.Name);
            await resp.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_UpdateAsync_Id()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            var student = _students.First();
            await resp.InsertAsync(student);
            student.Name = "隔壁老王666";
            await resp.UpdateAsync(student.StudentId, student);

            var result = await resp.FindByIdAsync(student.StudentId);
            Assert.Equal(student.Name, result.Name);
            await resp.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_UpdateByQueryAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            var student = _students.First();
            await resp.InsertAsync(student);

            // 需要延迟一下
            await Task.Delay(1000);

            var descriptor = new UpdateByQueryDescriptor<StudentSample>();
            descriptor
                .Script(s => s
                    .Source("ctx._source.name='隔壁老王八'")
                    .Lang("painless"))
                .Query(q => q
                    .Term(r => r
                        .Field(f => f.StudentId)
                        .Value(student.StudentId)));
            await resp.UpdateByQueryAsync(descriptor);

            var result = await resp.FindByIdAsync(student.StudentId);
            Assert.Equal("隔壁老王八", result.Name);
            await resp.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_FindByIdAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            var student = _students.First();
            await resp.InsertAsync(student);
            var result = await resp.FindByIdAsync(student.StudentId);
            Assert.Equal(student.StudentId, result.StudentId);
            await resp.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_FindByIdsAsync_1()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            await resp.InsertManyAsync(_students);

            var result = await resp.FindByIdsAsync(_students.Select(x => x.StudentId));
            Assert.Equal(_students.Count, result.Count());
            foreach (var student in _students)
                await resp.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_FindByIdsAsync_2()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            await resp.InsertManyAsync(_students);

            var result = await resp.FindByIdsAsync(_students.Select(x => x.StudentId).ToArray());
            Assert.Equal(_students.Count, result.Count());
            foreach (var student in _students)
                await resp.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_SearchAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var resp = scope.ServiceProvider.GetService<IEsRepository<StudentSample>>();
            await resp.InsertManyAsync(_students);
            await Task.Delay(1000);
            var context = scope.ServiceProvider.GetService<IElasticsearchContext>();
            var descriptor = new SearchDescriptor<StudentSample>();
            var result = await resp.SearchAsync(descriptor);
            Assert.Equal(_students.Count, result.TotalCount);
            foreach (var student in _students)
                await resp.DeleteAsync(student);
        }
    }
}
