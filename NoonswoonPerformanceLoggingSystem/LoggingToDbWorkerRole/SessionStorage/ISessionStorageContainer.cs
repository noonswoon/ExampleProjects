using NHibernate;

namespace Noonswoon.LoggingToDbWorkerRole.SessionStorage
{
    public interface ISessionStorageContainer
    {
        ISession GetCurrentSession();
        void Store(ISession session);
        void Clear();
    }
}
