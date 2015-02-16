namespace BeginningNHibernate.SessionStorage
{
    public class NHUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return Create(false);
        }

        public IUnitOfWork Create(bool forceNew)
        {
            return new NHUnitOfWork(forceNew);
        }
    }
}