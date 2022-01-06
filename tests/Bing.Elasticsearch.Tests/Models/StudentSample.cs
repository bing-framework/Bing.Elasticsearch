using System;
using Nest;

namespace Bing.Elasticsearch.Tests.Models
{
    [ElasticsearchType(RelationName = "test_student", IdProperty = "StudentId")]
    public class StudentSample
    {
        public long StudentId { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public DateTime BirthDay { get; set; }

        public bool IsValid { get; set; }
    }
}
