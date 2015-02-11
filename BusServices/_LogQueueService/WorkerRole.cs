using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using LogQueueService.Model.Dto;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace LogQueueService
{
    public class WorkerRole : RoleEntryPoint
    {
        string _conn = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
        const string _queue = "LogQueue";
        private QueueClient _client;

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request
        //QueueClient Client;
        //ManualResetEvent CompletedEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.WriteLine("Starting processing of messages");

            // Initiates the message pump and callback is invoked for each message that is received, calling close on the client will stop the pump.
            //Client.OnMessage((receivedMessage) =>
            //    {
            //        try
            //        {
            //            // Process the message
            //            Trace.WriteLine("Processing Service Bus message: " + receivedMessage.SequenceNumber.ToString());
            //        }
            //        catch
            //        {
            //            // Handle any message processing specific exceptions here
            //        }
            //    });

            //if (Client != null)
            //{
            //    while (true)
            //    {
            //        var message = Client.Receive();
            //        if (message != null)
            //        {
            //            Trace.WriteLine(message.GetBody<LogDataDto>());
            //        }    
            //    }
                
            //}

            //CompletedEvent.WaitOne();


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
                            Trace.WriteLine(messages.GetBody<LogDataDto>());
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Trace.Write("Error Recieved Message :- " + exception.Message);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            //ServicePointManager.DefaultConnectionLimit = 12;

            //// Create the queue if it does not exist already
            
            //var namespaceManager = NamespaceManager.CreateFromConnectionString(_conn);
            //if (!namespaceManager.QueueExists(_queue))
            //{
            //    namespaceManager.CreateQueue(_queue);
            //}

            //// Initialize the connection to Service Bus Queue
            //Client = QueueClient.CreateFromConnectionString(_conn, _queue);

            //var manager = NamespaceManager.CreateFromConnectionString(_conn);
            //if (!manager.QueueExists(_queue))
            //{
            //    var desc = new QueueDescription(_queue)
            //    {
            //        MaxSizeInMegabytes = 5120,
            //        DefaultMessageTimeToLive = new TimeSpan(0, 20, 0),
            //        RequiresDuplicateDetection = false
            //    };

            //    manager.CreateQueue(desc);

            //    return true;
            //}

           _client = QueueClient.CreateFromConnectionString(_conn, _queue, ReceiveMode.ReceiveAndDelete);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            //Client.Close();
            //CompletedEvent.Set();
           
            _client.Close();
            base.OnStop();
        }
    }
}
