using System;
using System.Collections;
using System.Threading;
using log4net;
using NHibernate;

namespace Noonswoon.LoggingToDbWorkerRole.SessionStorage
{
    public class ThreadSessionStorageContainer : ISessionStorageContainer
    {
        private static readonly Hashtable Sessions = new Hashtable();
        private ILog _log = LogManager.GetLogger(typeof(ThreadSessionStorageContainer));

        public ISession GetCurrentSession()
        {
            ISession nhSession = null;
            var threadName = GetThreadName();
            //_log.DebugFormat("getting current session from thread name: {0}", threadName);
            if (Sessions.Contains(threadName))
            {
                nhSession = (ISession)Sessions[threadName];
            }
            return nhSession;
        }

        public void Store(ISession session)
        {
            if (Sessions.Contains(GetThreadName()))
            {
                Sessions[GetThreadName()] = session;
            }
            else
            {
                Sessions.Add(GetThreadName(), session);
            }
        }

        private static string GetThreadName()
        {
            if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
            {
                Thread.CurrentThread.Name = Guid.NewGuid().ToString();
            }
            return Thread.CurrentThread.Name;
        }

        public void Clear()
        {
            var session = GetCurrentSession();
            if (session != null)
            {
                Sessions[GetThreadName()] = null;
                session.Dispose();
            }
        }
    }
}
