using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebDemo.Models
{
    public class UserRepository
    {
        private CloudStorageAccount _storage;
        private CloudTableClient _client;
        private CloudTable _table;

        //Call First Method For Check AllReady Storage
        public bool CheckStorageDataExists()
        {
            _storage = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"));

            _client = _storage.CreateCloudTableClient();

            _table = _client.GetTableReference("Users");

            if (!_table.CreateIfNotExists())
            {
                return true;
            }

            return false;
        }

        public void SaveUser(UsersEntity user)
        {
            if (CheckStorageDataExists())
            {
                var operation = TableOperation.Insert(user);

                _table.Execute(operation);
            }
        }

        public List<UsersEntity> GetUsers(string firstname, string lastname)
        {
            if (CheckStorageDataExists())
            {
                var query =
                    new TableQuery<UsersEntity>().Where(
                        TableQuery.CombineFilters(
                            TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, lastname),
                            TableOperators.And,
                            TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, firstname)));

                var users = _table.ExecuteQuery(query).Select(entity => new UsersEntity(entity.PartitionKey, entity.RowKey)
                {
                    Email = entity.Email, Phone = entity.Phone
                }).ToList();

                return users.ToList();
            }
            else
            {
                return null;
            }
        }
    }
}