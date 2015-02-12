using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace LogAPI.Controllers
{
    public class HomeController : Controller
    {
            private readonly ILog _log = LogManager.GetLogger(typeof(HomeController));
        // GET: /Home/

        public ActionResult Index()
        {
            for (int i = 0; i < 5; i++)
            {
                _log.Debug("Test Log");
            }
            return Content("message sent");
        }

    }
}
