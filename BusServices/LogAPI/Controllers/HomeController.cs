using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LogAPI.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var log = log4net.LogManager.GetLogger(typeof(HomeController));

            for (int i = 0; i < 10; i++)
            {
                log.Debug("Test Log");
            }

            return View();
        }

    }
}
