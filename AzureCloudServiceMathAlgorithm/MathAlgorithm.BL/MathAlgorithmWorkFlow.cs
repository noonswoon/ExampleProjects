using System;
using System.Collections.Generic;
using System.Linq;
using MathAlgorithm.Core;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace MathAlgorithm.BL
{
    public class MathAlgorithmWorkFlow
    {
        private CloudStorageAccount _storage;
        private CloudTableClient _client;
        private CloudTable _table;

        public void CheckStorage()
        {
            _storage = CloudStorageAccount.DevelopmentStorageAccount;
            _client = _storage.CreateCloudTableClient();
            _table = _client.GetTableReference("MatchAlgorithmDataModel");

            _table.CreateIfNotExists();
        }

        public void WorkerCase(MathAlgorithmMessage message)
        {
            if (message != null)
            {
                //switch (message.MessageId)
                //{
                //    case 1:
                //        CheckWorkerType(message.Type, message);
                //        break;
                //    case 2:
                //        CheckWorkerType(message.Type, message);
                //        break;
                //    case 3:
                //        CheckWorkerType(message.Type, message);
                //        break;
                //    case 4:
                //        CheckWorkerType(message.Type, message);
                //        break;
                //    case 5:
                //        CheckWorkerType(message.Type, message);
                //        break;
                //    case 6:
                //        CheckWorkerType(message.Type, message);
                //        break;
                //    case 7:
                //        CheckWorkerType(message.Type, message);
                //        break;
                //}

                CheckWorkerType(message.Type, message);
            }
        }

        private void CheckWorkerType(MathAlgorithmMessage.QueueType type, MathAlgorithmMessage messageItem)
        {
            CheckStorage();

            //if (type == MathAlgorithmMessage.QueueType.Calculate)
            //{

            //}

            if (type == MathAlgorithmMessage.QueueType.Save)
            {
                var msg = new MatchAlgorithmDataModel
                {
                    PartitionKey = "Step_" + messageItem.MessageId + "_" + DateTime.Now.ToString("s"),
                    RowKey = messageItem.Type.ToString()
                };

                var operation = new TableBatchOperation();

                operation.Insert(msg);
                _table.ExecuteBatch(operation);
            }
        }

        public List<MatchAlgorithmDataModel> GetMessageFromStorage()
        {
            CheckStorage();

            if (_table.Exists())
            {
                var query = new TableQuery<MatchAlgorithmDataModel>();

                var tmp = _table.ExecuteQuery(query);

                //_table.DeleteIfExists();
                
                return tmp.Any() ? tmp.ToList() : null;
            }
            else
            {
                return null;
            }
        }
    }
}
