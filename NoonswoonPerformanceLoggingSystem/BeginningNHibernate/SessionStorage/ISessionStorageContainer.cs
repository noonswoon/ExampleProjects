using NHibernate;

namespace BeginningNHibernate.SessionStorage
{
    public interface ISessionStorageContainer
    {
        ISession GetCurrentSession();
        void Store(ISession session);
        void Clear();
    }
}