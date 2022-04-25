using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bing.Elasticsearch.Repositories;
using Bing.Elasticsearch.Tests.Models;
using Nest;
using Xunit;

namespace Bing.Elasticsearch.Tests
{
    /// <summary>
    /// ES仓储测试
    /// </summary>
    public class EsRepositoryTest
    {
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

        /// <summary>
        /// student sample repo
        /// </summary>
        private readonly IEsRepository<StudentSample> _studentRepo;

        public EsRepositoryTest(IEsRepository<StudentSample> studentRepo)
        {
            _studentRepo = studentRepo;
        }

        [Fact]
        public async Task Test_InsertAsync()
        {
            var student = _students.First();
            await _studentRepo.InsertAsync(student);

            var result = await _studentRepo.FindByIdAsync(student.StudentId);
            Assert.Equal(student.StudentId, result.StudentId);
            await _studentRepo.DeleteAsync(student.StudentId);
        }

        [Fact]
        public async Task Test_InsertManyAsync()
        {
            await _studentRepo.InsertManyAsync(_students);

            var result = await _studentRepo.FindByIdsAsync(_students.Select(x => x.StudentId));
            Assert.Equal(_students.Count, result.Count());
            foreach (var student in _students)
                await _studentRepo.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_BulkAsync()
        {
            await _studentRepo.InsertManyAsync(_students);
            foreach (var student in _students)
                student.Name = "隔壁老王";
            await _studentRepo.BulkAsync(_students);

            var result = await _studentRepo.FindByIdsAsync(_students.Select(x => x.StudentId));
            Assert.Equal(_students.First().Name, result.First().Name);
            foreach (var student in _students)
                await _studentRepo.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_DeleteAsync_Id()
        {
            var student = _students.First();
            await _studentRepo.InsertAsync(student);
            await _studentRepo.DeleteAsync(student.StudentId);

            var result = await _studentRepo.FindByIdAsync(student.StudentId);
            Assert.Null(result);
        }

        [Fact]
        public async Task Test_DeleteAsync_Entity()
        {
            var student = _students.First();
            await _studentRepo.InsertAsync(student);
            await _studentRepo.DeleteAsync(student);

            var result = await _studentRepo.FindByIdAsync(student.StudentId);
            Assert.Null(result);
        }

        [Fact]
        public async Task Test_DeleteByQueryAsync()
        {
            var student = _students.First();
            await _studentRepo.InsertAsync(student);

            // 需要延迟一下
            await Task.Delay(1000);

            var descriptor = new DeleteByQueryDescriptor<StudentSample>();
            descriptor.Query(q => q
                .Term(r => r
                    .Field(f => f.StudentId)
                    .Value(student.StudentId)));
            await _studentRepo.DeleteByQueryAsync(descriptor);

            var result = await _studentRepo.FindByIdAsync(student.StudentId);
            Assert.Null(result);
        }

        [Fact]
        public async Task Test_UpdateAsync()
        {
            var student = _students.First();
            await _studentRepo.InsertAsync(student);
            student.Name = "隔壁老王666";
            await _studentRepo.UpdateAsync(student);

            var result = await _studentRepo.FindByIdAsync(student.StudentId);
            Assert.Equal(student.Name, result.Name);
            await _studentRepo.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_UpdateAsync_Id()
        {
            var student = _students.First();
            await _studentRepo.InsertAsync(student);
            student.Name = "隔壁老王666";
            await _studentRepo.UpdateAsync(student.StudentId, student);

            var result = await _studentRepo.FindByIdAsync(student.StudentId);
            Assert.Equal(student.Name, result.Name);
            await _studentRepo.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_UpdateByQueryAsync()
        {
            var student = _students.First();
            await _studentRepo.InsertAsync(student);

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
            await _studentRepo.UpdateByQueryAsync(descriptor);

            var result = await _studentRepo.FindByIdAsync(student.StudentId);
            Assert.Equal("隔壁老王八", result.Name);
            await _studentRepo.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_FindByIdAsync()
        {
            var student = _students.First();
            await _studentRepo.InsertAsync(student);
            var result = await _studentRepo.FindByIdAsync(student.StudentId);
            Assert.Equal(student.StudentId, result.StudentId);
            await _studentRepo.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_FindByIdsAsync_1()
        {
            await _studentRepo.InsertManyAsync(_students);

            var result = await _studentRepo.FindByIdsAsync(_students.Select(x => x.StudentId));
            Assert.Equal(_students.Count, result.Count());
            foreach (var student in _students)
                await _studentRepo.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_FindByIdsAsync_2()
        {
            await _studentRepo.InsertManyAsync(_students);

            var result = await _studentRepo.FindByIdsAsync(_students.Select(x => x.StudentId).ToArray());
            Assert.Equal(_students.Count, result.Count());
            foreach (var student in _students)
                await _studentRepo.DeleteAsync(student);
        }

        [Fact]
        public async Task Test_SearchAsync()
        {
            await _studentRepo.InsertManyAsync(_students);
            await Task.Delay(1000);
            var descriptor = new SearchDescriptor<StudentSample>();
            var result = await _studentRepo.SearchAsync(descriptor);
            Assert.Equal(_students.Count, result.TotalCount);
            foreach (var student in _students)
                await _studentRepo.DeleteAsync(student);
        }
    }
}
