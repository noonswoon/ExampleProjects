using System;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using log4net;

namespace BeginningNHibernate.NhSessionFactory
{
    public class AuditEventListener : IPreUpdateEventListener, IPreInsertEventListener
    {
        private ILog _log = LogManager.GetLogger(typeof(AuditEventListener));

        private TimeZoneInfo GetCurrentTimeZoneInfo()
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return timeZoneInfo;
        }


        public bool OnPreInsert(PreInsertEvent @event)
        {
            var audit = @event.Entity as ICreateDate;
            if (audit == null)
                return false;

            var adminTime = DateTime.UtcNow;
            //log.DebugFormat("localNow = {0}, audit type {1}", localNow, audit.GetType());            

            Set(@event.Persister, @event.State, "CreateDate", adminTime);
            audit.CreateDate = adminTime;
            return false;
        }

        public bool OnPreUpdate(PreUpdateEvent @event)
        {
            var audit = @event.Entity as ILastUpdate;
            if (audit == null)
                return false;

            var adminTime = DateTime.UtcNow;
            //log.DebugFormat("localNow = {0}, audit type {1}", localNow, audit.GetType());            

            Set(@event.Persister, @event.State, "LastUpdate", adminTime);
            audit.LastUpdate = adminTime;
            return false;
        }


        private void Set(IEntityPersister persister, object[] state, string propertyName, object value)
        {
            var index = Array.IndexOf(persister.PropertyNames, propertyName);
            if (index == -1)
                return;
            state[index] = value;
        }

    }
}