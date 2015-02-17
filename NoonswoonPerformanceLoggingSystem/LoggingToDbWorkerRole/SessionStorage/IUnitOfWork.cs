using System;

namespace Noonswoon.LoggingToDbWorkerRole.SessionStorage
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit(bool isWithTransactionRollback);
        void Commit();
        void Undo();
    }
}
