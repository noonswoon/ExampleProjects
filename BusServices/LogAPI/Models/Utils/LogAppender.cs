using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net.Appender;
using log4net.Core;
using LogAPI.Models.Dto;

namespace LogAPI.Models.Utils
{
    public class LogAppender:AppenderSkeleton
    {

        private LogDataDto _log;

        public override void ActivateOptions()
        {
            _log = new LogDataDto();
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            _log = new LogDataDto
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