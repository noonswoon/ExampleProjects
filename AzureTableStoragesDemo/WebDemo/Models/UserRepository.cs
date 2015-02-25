using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using log4net;

namespace WebDemo.Models
{
    public class UserRepository
    {
        private ILog _log = LogManager.GetLogger(typeof(UserRepository));

        private CloudStorageAccount _storage;
        private CloudTableClient _client;
        private CloudTable _table;

        //Call First Method For Check AllReady Storage
        public bool CheckStorageDataExists()
        {
            try
            {
                var connectionString = CloudConfigurationManager.GetSetting("AzureTable.ConnectionString");
                _log.DebugFormat("connectionString [{0}]", connectionString);

                _storage = CloudStorageAccount.Parse(connectionString);

                _client = _storage.CreateCloudTableClient();
                _table = _client.GetTableReference("Users");

                if (_table.Exists())
                {
                    return true;
                }

                _table.Create();
               
                return true;
            }
            catch (Exception ex)
            {
                _log.Error("CheckStorage Exists Error", ex);
                return false;
            }

        }

        public void SaveUser(UsersEntity user)
        {
            try
            {
                if (CheckStorageDataExists())
                {
                    var operation = TableOperation.Insert(user);

                    _table.Execute(operation);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
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

                var users = _table.ExecuteQuery(query).ToList();
                return users;
            }
            else
            {
                return null;
            }
        }
    }
}