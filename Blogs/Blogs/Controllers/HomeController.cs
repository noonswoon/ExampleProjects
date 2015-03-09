using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WindowsAzure.Table;
using Blog.Bl;
using Blog.Core;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Blogs.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        private ILog _log = LogManager.GetLogger(typeof(HomeController));

        //private static readonly CloudStorageAccount Storage = CloudStorageAccount.DevelopmentStorageAccount;
        //private static readonly CloudTableClient Client = Storage.CreateCloudTableClient();

        private TableSet<Post> _post;

        public void CreateTable()
        {
            var storage = CloudStorageAccount.DevelopmentStorageAccount;
            var client = storage.CreateCloudTableClient();
            _post = new TableSet<Post>(client);
        }

        public ActionResult Index()
        {
            CreateTable();

            var tmp = _post.ToList();
            
            return View(tmp);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Post post)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CreateTable();

                    post.PostDateTime = DateTime.Now;

                    _post.Add(post);

                  return RedirectToAction("Index");
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }

            return View();
        }

        public ActionResult Detail(string title)
        {
            CreateTable();

            var tmp = _post.Where(p => p.PostTitle.Contains(title));

            return View(tmp);
        }

        //[HttpPost]
        //public ActionResult Detail(string title)
        //{
        //    var tmp = _post.SingleOrDefault(p => p.PostTitle == title);
        //    if (tmp != null)
        //    {
        //        return View(tmp);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
    }
}
