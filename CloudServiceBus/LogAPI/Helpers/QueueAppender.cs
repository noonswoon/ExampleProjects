using log4net.Appender;
using log4net.Core;
using LogModels.Dto;

namespace LogAPI.Helpers
{
    public class QueueAppender : AppenderSkeleton
    {
        private LogDto _logEvent;
        private readonly QueueService _service = new QueueService();

        //public override void ActivateOptions()
        //{
        //    _logEvent = new LogDto();
        //}

        protected override void Append(LoggingEvent loggingEvent)
        {
            _logEvent = new LogDto
            {
                LogTime = loggingEvent.TimeStamp,
                LogLevel = loggingEvent.Level.Name,
                LogDetail = this.RenderLoggingEvent(loggingEvent)
            };

            if (!_service.CreateQueue())
            {
                _service.SendMessage(_logEvent);
            }
        }
    }
}