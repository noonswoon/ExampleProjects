using System;

namespace Noonswoon.LoggingToDbWorkerRole.NhSessionFactory
{
    interface ILastUpdate
    {
        DateTime LastUpdate { get; set; }
    }
}
