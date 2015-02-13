using System;
using System.Diagnostics;
using log4net.Appender;
using log4net.Core;
using System.Linq;

namespace Noonswoon.Appender
{
    public class MessageQueueAppender : BufferingAppenderSkeleton
    {

        protected override void SendBuffer(LoggingEvent[] events)
        {
            try
            {
                var queueService = new QueueService();
                var logs = events.Select(e =>
                                         new MessageQueueLoggingEvent()
                                             {
                                                 Date = e.TimeStamp.Date,
                                                 Time = e.TimeStamp,
                                                 Level = e.Level.Name,
                                                 Logger = e.LoggerName,
                                                 Message = e.MessageObject.ToString(),
                                             }).ToArray();
                queueService.SendMessage(logs);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message); 
            }
        }
    }
}