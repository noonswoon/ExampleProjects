using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;

namespace StorageDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var _conn = ConfigurationManager.AppSettings["AzureStorage.ConnectionString"];
            var _tbname = "Movies";

            CloudStorageAccount _storage;
            CloudTableClient _client;
            CloudTable _table;

            _storage = CloudStorageAccount.Parse(_conn);

            _client = _storage.CreateCloudTableClient();

            _table = _client.GetTableReference(_tbname);

            if (!_table.Exists())
            {
                _table.Create();
            }

            var movie = new Movie
            {
                PartitionKey = "Action",
                RowKey = "White Water Rapids Survival"
            };

            var _operation = TableOperation.Insert(movie);
        }
    }
}
