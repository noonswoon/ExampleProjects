using System;
using System.Diagnostics;
using LogModels.Dto;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;

namespace LogAPI.Helpers
{
    public class QueueService
    {
        private readonly string _connectionString = CloudConfigurationManager.GetSetting("ServiceBus.ConnectionString");
        private string QUEUE_NAME = "LogQueue";

        public bool CreateQueue()
        {
            Trace.TraceInformation("_connectionString [{0}]", _connectionString);
            try
            {
                var manager = NamespaceManager.CreateFromConnectionString(_connectionString);
                if (!manager.QueueExists(QUEUE_NAME))
                {
                    var desc = new QueueDescription(QUEUE_NAME)
                    {
                        MaxSizeInMegabytes = 1024,//size of queue 5 GB
                        DefaultMessageTimeToLive = new TimeSpan(6, 0, 0),
                        RequiresDuplicateDetection = false//?
                    };

                    manager.CreateQueue(desc);
                    return true;
                }
            }
            catch (Exception exception)
            {
                Trace.TraceError("Can not create queue [{0}]", exception.Message);
            }
            return false;
        }

        public void SendMessage(LogDto log)
        {
            try
            {
                var client = QueueClient.CreateFromConnectionString(_connectionString, QUEUE_NAME);
                if (client != null)
                {
                    var message = new BrokeredMessage(log);//send to body
                    client.Send(message);
                }
            }
            catch (Exception exception)
            {
                Trace.TraceError("Error Sending Message [{0}]",exception.Message);
            }
        }


    }

}