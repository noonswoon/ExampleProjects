using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using LogAPI.Models.Dto;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;

namespace LogAPI.Models.Utils
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

        public void SendMessage(LogDataDto log)
        {
            try
            {
                var client = QueueClient.CreateFromConnectionString(_conn, _queue, ReceiveMode.PeekLock);
                if (client != null)
                {
                    var message = new BrokeredMessage(log);
                   
                    //string tmpTime = log.LogTime.ToString("yyyy-MM-dd HH:mm");

                    //message.Properties["LogTime"] = tmpTime;
                    //message.Properties["LogLevel"] = log.LogLevel;
                    //message.Properties["LogDetail"] = log.LogDetail;
                   
                    client.Send(message);
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine("Error Sending Message :- " + exception.Message);
            }
        }

        public LogDataDto GetLogMessage()
        {
            try
            {
                var client = QueueClient.CreateFromConnectionString(_conn, _queue, ReceiveMode.ReceiveAndDelete);

                if (client != null)
                {
                    while (true)
                    {
                        var messages = client.Receive();
                        if (messages != null)
                        {
                            return messages.GetBody<LogDataDto>();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.Write("Error Recieved Message :- " + exception.Message);
            }

            return null;
        }
    }
}