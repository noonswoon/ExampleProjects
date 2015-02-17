using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Noonswoon.LoggingToDbWorkerRole.SessionStorage
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
