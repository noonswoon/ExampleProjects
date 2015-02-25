namespace Noonswoon.LoggingToDbWorkerRole.SessionStorage
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
        IUnitOfWork Create(bool forceNew);
    }
}
