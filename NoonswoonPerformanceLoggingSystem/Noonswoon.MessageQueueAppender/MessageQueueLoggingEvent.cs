using System;
using System.Runtime.Serialization;

namespace Noonswoon.Appender
{
    [DataContract]
    public class MessageQueueLoggingEvent
    {
        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Level { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string Logger { get; set; }

        public override string ToString()
        {
            return string.Format(
                "LogTime [{0}], LogLevel [{1}], LogDetail [{2}]",
                Time, Level, Message);
        }

    }
}