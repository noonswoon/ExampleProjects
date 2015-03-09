using System;
using System.Linq;
using BeginningNHibernate.NhSessionFactory;
using BeginningNHibernate.SessionStorage;
using NHibernate.Linq;
using Noonswoon.Appender;

namespace BeginningNHibernate
{
    class Program
    {
        public static void Main()
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

            using (unitOfWorkFactory.Create())
            {

                var session = SessionFactory.GetCurrentSession();
                var logCount = session.Query<MessageQueueLoggingEvent>().Count();

                Console.WriteLine("logCount [{0}]", logCount);
            }
        }
    }
}
