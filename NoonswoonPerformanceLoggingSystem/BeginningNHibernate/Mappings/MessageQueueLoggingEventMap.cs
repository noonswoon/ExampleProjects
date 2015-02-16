using FluentNHibernate.Mapping;
using Noonswoon.Appender;

namespace BeginningNHibernate.Mappings
{
    public class MessageQueueLoggingEventMap : ClassMap<MessageQueueLoggingEvent>
    {
        public MessageQueueLoggingEventMap()
        {
            Table("LoggingEvents");

            Id(x => x.Id).GeneratedBy.GuidComb().Index("PK_LoggingEvents_Id");
            Map(x => x.Level).Not.Nullable();
            Map(x => x.Logger).Not.Nullable();
            Map(x => x.Message).Not.Nullable();
            Map(x => x.Time).Not.Nullable();
            Map(x => x.Date).Not.Nullable();

        }
    }
}