using System;
using System.Runtime.Serialization;

namespace Noonswoon.Appender
{
    [DataContract]
    public class MessageQueueLoggingEvent
    {
        public virtual Guid Id { get; set; }

        [DataMember]
        public virtual DateTime Time { get; set; }

        [DataMember]
        public virtual DateTime Date { get; set; }

        [DataMember]
        public virtual string Level { get; set; }

        [DataMember]
        public virtual string Message { get; set; }

        [DataMember]
        public virtual string Logger { get; set; }

        public  override string ToString()
        {
            return string.Format(
                "LogTime [{0}], LogLevel [{1}], LogDetail [{2}]",
                Time, Level, Message);
        }

    }
}