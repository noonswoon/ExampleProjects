using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noonswoon.LoggingToDbWorkerRole.NhSessionFactory;

namespace Noonswoon.LoggingToDbWorkerRole.SessionStorage
{
    public class NHUnitOfWork : IUnitOfWork
    {

        public NHUnitOfWork(bool forceNewContext)
        {
            if (forceNewContext)
            {
                SessionFactory.ClearCurrentSession();
            }
        }

        public void Commit(bool isWithTransactionRollback)
        {
            //without transaction
            if (!isWithTransactionRollback)
            {
                SessionFactory.GetCurrentSession().Flush();
                return;
            }

            using (var tx = SessionFactory.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    tx.Commit();
                }
                catch
                {
                    if (tx.IsActive) tx.Rollback();
                    throw;
                }
            }
        }

        public void Commit()
        {
            Commit(false);
        }

        public void Undo()
        {
            SessionFactory.ClearCurrentSession();
        }

        public void Dispose()
        {
            SessionFactory.ClearCurrentSession();
        }

    }
}
