using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Noonswoon.Appender;
using log4net;
using Noonswoon.LoggingToDbWorkerRole.NhSessionFactory;
using Noonswoon.LoggingToDbWorkerRole.SessionStorage;

namespace Noonswoon.LoggingToDbWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(WorkerRole));

        private string _connectionString;
        private QueueClient _client;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);



        public override void Run()
        {
            _log.Debug("Background Worker Role Run");

            Trace.TraceInformation("LoggingToDbWorkerRole is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            _log.Debug("application started");
            _connectionString = CloudConfigurationManager.GetSetting("ServiceBus.ConnectionString");
            _log.DebugFormat("_connectionString [{0}]", _connectionString);


            bool result = base.OnStart();

            _log.Debug("LoggingToDbWorkerRole has been started");



            return result;
        }

        public override void OnStop()
        {
            _log.Debug("LoggingToDbWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            _log.Debug("LoggingToDbWorkerRole has stopped");
        }

        private Task RunAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => GetMessage(cancellationToken));
        }

        private void GetMessage(CancellationToken cancellationToken)
        {
            try
            {
                SessionFactory.Init();

                _client = QueueClient.CreateFromConnectionString(
                    _connectionString,
                    QueueService.QUEUE_NAME,
                    ReceiveMode.ReceiveAndDelete);

                if (_client != null)
                {
                    while (!cancellationToken.IsCancellationRequested)//false
                    {
                        var messages = _client.Receive();
                        if (messages != null)
                        {
                            IUnitOfWorkFactory unitOfWorkFactory = new NHUnitOfWorkFactory(); 
                            var log = messages.GetBody<MessageQueueLoggingEvent[]>();
                            foreach (var l in log)
                            {
                                using (var unitOfWork = unitOfWorkFactory.Create())
                                {
                                    var session = SessionFactory.GetCurrentSession();
                                    var now = DateTime.UtcNow;
                                    var tmp = new MessageQueueLoggingEvent
                                    {
                                        Date = l.Date,
                                        Level = l.Level,
                                        Logger = l.Logger,
                                        Message = l.Message,
                                        Time = l.Time
                                    };

                                    session.Save(log);
                                    unitOfWork.Commit();
                                }

                            }
                            //Trace.WriteLine(log.ToString());
                           // _log.Debug(log);
                        }
                        else
                        { 
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }
    }
}
