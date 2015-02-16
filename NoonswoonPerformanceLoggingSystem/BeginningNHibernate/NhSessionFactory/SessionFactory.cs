using System;
using System.Configuration;
using System.Reflection;
using BeginningNHibernate.Mappings;
using BeginningNHibernate.SessionStorage;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Event;
using log4net;

namespace BeginningNHibernate.NhSessionFactory
{
    public class SessionFactory
    {

        private static ISessionFactory _sessionFactory;
        private static NHibernate.Cfg.Configuration _config;
        private static ILog _log = LogManager.GetLogger(typeof(SessionFactory));

        public static NHibernate.Cfg.Configuration Config
        {
            get { return _config; }
        }

        public static void Init()
        {
            try
            {
                _config = Fluently.Configure()
                    .Database(GetDatabaseConfig())
                    .Mappings(m =>
                    {
                        m.FluentMappings.AddFromAssemblyOf<MessageQueueLoggingEventMap>();
                        //to use name query in xml file
                        m.HbmMappings.AddFromAssemblyOf<MessageQueueLoggingEventMap>();
                    })
                    .ExposeConfiguration(TreatConfiguration)
                    .BuildConfiguration();

                _sessionFactory = _config.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                LogExceptionRecursively(ex);
            }
        }

        private static void LogExceptionRecursively(Exception ex)
        {
            if (ex == null)
            {
                _log.Fatal("SessionFactory init exception is null and return exit method");
            }
            else
            {
                if (ex is ReflectionTypeLoadException)
                {
                    var typeLoadException = ex as ReflectionTypeLoadException;
                    var loaderExceptions = typeLoadException.LoaderExceptions;
                    foreach (var loaderException in loaderExceptions)
                    {
                        _log.Fatal(loaderException);
                    }
                }
                else
                {
                    _log.Fatal(ex);
                }



                LogExceptionRecursively(ex.InnerException);
            }
        }

        public static void Init(string connectionString)
        {
            _config = Fluently.Configure()
                    .Database(GetDatabaseConfig(connectionString))
                    .Mappings(m =>
                    {
                        m.FluentMappings.AddFromAssemblyOf<MessageQueueLoggingEventMap>();
                        //to use name query in xml file
                        m.HbmMappings.AddFromAssemblyOf<MessageQueueLoggingEventMap>();
                    })
                    .ExposeConfiguration(TreatConfiguration)
                    .BuildConfiguration();

            _sessionFactory = _config.BuildSessionFactory();
        }

        private static void TreatConfiguration(NHibernate.Cfg.Configuration cfg)
        {
            cfg.EventListeners.PreInsertEventListeners
            = new IPreInsertEventListener[]
                                                 {
                                                    new AuditEventListener()
                                                 };

            cfg.EventListeners.PreUpdateEventListeners

                 = new IPreUpdateEventListener[]
                                                 {
                                                    new AuditEventListener()
                                                 };

            //for unique constrain exception
            cfg.SetProperty("sql_exception_converter",
                typeof(SqlServerExceptionConverter).AssemblyQualifiedName);
        }

        private static IPersistenceConfigurer GetDatabaseConfig()
        {
            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            return MsSqlConfiguration.MsSql2008.ConnectionString(connectionString)
                //.ShowSql()
                //.FormatSql()
                .AdoNetBatchSize(100);
        }

        private static IPersistenceConfigurer GetDatabaseConfig(string connectionString)
        {
            return MsSqlConfiguration.MsSql2008.ConnectionString(connectionString)
                //.ShowSql()
                //.FormatSql()
                .AdoNetBatchSize(100);
        }
        public static ISessionFactory GetNHSessionFactory()
        {
            if (_sessionFactory == null)
                Init();//we can move this to Global.ascx
            return _sessionFactory;
        }

        private static ISession GetNewSession()
        {
            return GetNHSessionFactory().OpenSession();
        }

        public static ISession GetCurrentSession()
        {
            var sessionStorageContainer = SessionStorageFactory.GetStorageContainer();

            var currentSession = sessionStorageContainer.GetCurrentSession();
            if (currentSession == null)
            {
                currentSession = GetNewSession();
                sessionStorageContainer.Store(currentSession);
            }
            return currentSession;
        }

        public static void ClearCurrentSession()
        {
            var sessionStorageContainer =
                SessionStorageFactory.GetStorageContainer();
            sessionStorageContainer.Clear();

        }


    }
}
