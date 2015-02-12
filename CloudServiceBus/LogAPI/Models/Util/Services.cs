using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using LogModels.Dto;
using Microsoft.WindowsAzure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace LogAPI.Models.Util
{
    public class Services
    {
        private string _conn = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
        private string _queue = "LogQueue";

        public bool CreateQueue()
        {
            try
            {
                var manager = NamespaceManager.CreateFromConnectionString(_conn);
                if (!manager.QueueExists(_queue))
                {
                    var desc = new QueueDescription(_queue)
                    {
                        MaxSizeInMegabytes = 5120,
                        DefaultMessageTimeToLive = new TimeSpan(0, 20, 0),
                        RequiresDuplicateDetection = false
                    };

                    manager.CreateQueue(desc);

                    return true;
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("Can not create queue :- " + exception.Message);
            }
            return false;
        }

        public void SendMessage(LogDto log)
        {
            try
            {
                var client = QueueClient.CreateFromConnectionString(_conn, _queue, ReceiveMode.PeekLock);
                if (client != null)
                {
                    var message = new BrokeredMessage(log);


                    client.Send(message);
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("Error Sending Message :- " + exception.Message);
            }
        }


    }
}