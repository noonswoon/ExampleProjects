using System;

namespace BeginningNHibernate.SessionStorage
{

        public interface IUnitOfWork : IDisposable
        {
            void Commit(bool isWithTransactionRollback);
            void Commit();
            void Undo();
        }


    //end class


}