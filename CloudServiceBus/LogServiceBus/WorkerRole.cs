using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using LogModels.Dto;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace LogServiceBus
{
    public class WorkerRole : RoleEntryPoint
    {
        string _conn = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
        const string _queue = "LogQueue";
        private QueueClient _client;

        public override void Run()
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
                            var log = messages.GetBody<LogDto>();

                            //Trace.WriteLine(log.ToString());
                            Trace.WriteLine("Message Id :- " + messages.MessageId + ", log time :- " + log.LogTime + ", log level :-" + log.LogLevel + ", log detail :- " + log.LogDetail);
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
            _client = QueueClient.CreateFromConnectionString(_conn, _queue, ReceiveMode.ReceiveAndDelete);
            return base.OnStart();
        }

        public override void OnStop()
        {
            _client.Close();
            base.OnStop();
        }
    }
}
