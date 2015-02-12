using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.WindowsAzure;
using log4net;

namespace LogAPI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            _log.Debug("Application_Start");

            var connectionString = CloudConfigurationManager.GetSetting("ServiceBus.ConnectionString");
            _log.DebugFormat("connectionString [{0}]", connectionString);

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

        }
    }
}