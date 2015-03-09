using System;
using System.Data.Services.Common;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageDemo
{
    [DataServiceKey("PartitionKey","RowKey")]
    public class Movie : TableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTime Timestamp { get; set; }
        public int ReleaseYear { get; set; }
        public double Rating { get; set; }
        public string Language { get; set; }
        public bool Favorite { get; set; }
    }
}
