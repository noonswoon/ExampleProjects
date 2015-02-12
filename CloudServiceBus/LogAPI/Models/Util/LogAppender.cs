using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net.Appender;
using log4net.Core;
using LogModels.Dto;

namespace LogAPI.Models.Util
{
    public class LogAppender : AppenderSkeleton
    {
        private LogDto _log;

        public override void ActivateOptions()
        {
            _log = new LogDto();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            _log = new LogDto
            {
                LogTime = loggingEvent.TimeStamp,
                LogLevel = loggingEvent.Level.Name,
                LogDetail = this.RenderLoggingEvent(loggingEvent)
            };

            var service = new Services();
            if (!service.CreateQueue())
            {
                service.SendMessage(_log);
            }
        }
    }
}