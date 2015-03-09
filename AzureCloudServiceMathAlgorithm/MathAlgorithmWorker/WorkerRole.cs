using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MathAlgorithm.BL;
using MathAlgorithm.Core;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace MathAlgorithmWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(WorkerRole));

        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent _runCompleteEvent = new ManualResetEvent(false);

        readonly Queue<MathAlgorithmMessage> _messages = new Queue<MathAlgorithmMessage>();

        public override void Run()
        {
            Trace.TraceInformation("MathAlgorithmWorker is running");
            _log.Info("MathAlgorithmWorker is running");

            try
            {
                var task = this.RunAsync(this._cancellationTokenSource.Token);
                task.Wait();
            }
            catch (Exception exception)
            {
                _log.ErrorFormat("Error : {0}", exception);
            }
            finally
            {
                this._runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            if (_messages.Count == 0)
            {

                _log.Info("Create Message");
                var items = new List<MathAlgorithmMessage>
                    {
                        new MathAlgorithmMessage
                        {
                            MessageId = 1,
                            Type = MathAlgorithmMessage.QueueType.Calculate
                        },
                        new MathAlgorithmMessage
                        {
                            MessageId = 2,
                            Type = MathAlgorithmMessage.QueueType.Save
                        },
                        new MathAlgorithmMessage
                        {
                            MessageId = 3,
                            Type = MathAlgorithmMessage.QueueType.Calculate
                        },
                        new MathAlgorithmMessage
                        {
                            MessageId = 4,
                            Type = MathAlgorithmMessage.QueueType.Save
                        }
                    };

                foreach (var item in items)
                {
                    _messages.Enqueue(item);
                }

                _log.Info("MathAlgorithm Queue add");
            }

            var result = base.OnStart();

            Trace.TraceInformation("MathAlgorithmWorker has been started");
            _log.Info("MathAlgorithmWorker has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("MathAlgorithmWorker is stopping");
            _log.Info("MathAlgorithmWorker is stopping");

            this._cancellationTokenSource.Cancel();
            this._runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("MathAlgorithmWorker has stopped");
            _log.Info("MathAlgorithmWorker has stopped");
        }

        private Task RunAsync(CancellationToken cancellationToken)
        {
            var task = Task.Factory.StartNew(() =>
            {
                var tmp = new MathAlgorithmWorkFlow();
                var items = tmp.GetMessageFromStorage();
                if (items != null)
                {
                   _log.Info("Found Data in Storage.");
                    foreach (var item in items)
                    {
                        _log.InfoFormat("Message from Storage ID : {0}, Type : {1}, Time {2}", item.PartitionKey, item.RowKey, item.Timestamp);
                    }
                }

                // TODO: Replace the following with your own logic.
                while (!cancellationToken.IsCancellationRequested)
                {
                    Trace.TraceInformation("Working");
                    _log.Info("Working");

                    _log.Info("Check Message Queue is Empty");

                    if (_messages.Count != 0)
                    {

                        var message = _messages.Dequeue();

                        var flow = new MathAlgorithmWorkFlow();
                        flow.WorkerCase(message);

                        _log.Info("MathAlgorithm Queue Procress");
                    }
                    Thread.Sleep(1000);
                }
            });
            return task;
        }
    }
}
