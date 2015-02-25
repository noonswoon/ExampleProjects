using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace Noonswoon.Test.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private readonly ILog _log = LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
           
            for (int i = 0; i < 10; i++)
            {
                _log.Debug("Debug from Web - Test");    
            }
            
          
            return Content("check speed in log file");
        }

    }
}
