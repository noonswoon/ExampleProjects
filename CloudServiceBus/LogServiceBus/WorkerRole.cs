using System;
using System.Diagnostics;
using LogModels.Dto;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using log4net;
namespace LogServiceBus
{
    public class WorkerRole : RoleEntryPoint
    {
        const string QUEUE_NAME = "LogQueue";

        private readonly ILog _log = LogManager.GetLogger(typeof(WorkerRole));

        private string _connectionString;
        private QueueClient _client;
        public override bool OnStart()
        {
            _log.Debug("application started");
            _connectionString = CloudConfigurationManager.GetSetting("ServiceBus.ConnectionString");
            _log.DebugFormat("_connectionString [{0}]", _connectionString);

            return base.OnStart();
        }

        public override void Run()
        {
            _log.Debug("Run called");

            try
            {
                 _client = QueueClient.CreateFromConnectionString(_connectionString, QUEUE_NAME, ReceiveMode.ReceiveAndDelete);

                if (_client != null)
                {
                    while (true)
                    {
                        var messages = _client.Receive();
                        if (messages != null)
                        {
                            var log = messages.GetBody<LogDto>();
                            //Trace.WriteLine(log.ToString());
                            _log.Debug(log);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Trace.Write("Error Recieved Message :- " + exception.Message);
            }
        }


        public override void OnStop()
        {
            _client.Close();
            base.OnStop();
        }
    }
}
