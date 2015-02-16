using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeginningNHibernate.NhSessionFactory;
using BeginningNHibernate.SessionStorage;
using Noonswoon.Appender;

namespace BeginningNHibernate
{
    class Program
    {
      public  static void Main()
        {
            SessionFactory.Init();

            IUnitOfWorkFactory unitOfWorkFactory = new NHUnitOfWorkFactory();
            using (var unitOfWork = unitOfWorkFactory.Create())
            {

                var session = SessionFactory.GetCurrentSession();
                var now = DateTime.UtcNow;
                var log = new MessageQueueLoggingEvent()
                {
                    Date = now.Date,
                    Time = now,
                    Message = "error",
                    Logger = typeof(Program).ToString(),
                    Level = "DEBUG"
                };
                session.Save(log);


                unitOfWork.Commit();
            }

        }
    }
}
