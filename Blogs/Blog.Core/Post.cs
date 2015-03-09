using System;
using WindowsAzure.Table.Attributes;

namespace Blog.Core
{
    public class Post
    {
        [PartitionKey]
        public string PostTitle { get; set; }
        public string PostDetail { get; set; }
        [Timestamp]
        public DateTime PostDateTime { get; set; }
    }
}
