using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NHibernate;

namespace Noonswoon.LoggingToDbWorkerRole.SessionStorage
{
    public class HttpSessionContainer : ISessionStorageContainer
    {
        private const string SESSION_KEY = "PerformanceLogSession";

        public ISession GetCurrentSession()
        {
            ISession nhSession = null;

            if (HttpContext.Current.Items.Contains(SESSION_KEY))
                nhSession = (ISession)HttpContext.Current.Items[SESSION_KEY];

            return nhSession;
        }

        public void Store(ISession session)
        {
            if (HttpContext.Current.Items.Contains(SESSION_KEY))
                HttpContext.Current.Items[SESSION_KEY] = session;
            else
                HttpContext.Current.Items.Add(SESSION_KEY, session);
        }

        public void Clear()
        {
            var session = GetCurrentSession();
            if (session != null)
            {
                HttpContext.Current.Items[SESSION_KEY] = null;//set to null
                session.Dispose(); //
            }
        }
    }
}
