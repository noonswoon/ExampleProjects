using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using WebDemo.Models;

namespace WebDemo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/



        public ActionResult Index()
        {
            var repository = new UserRepository();

            //row , partition
            var rowKey = "Vatthanachai" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var partitionKey = "Wongprasert";
            var user = new UsersEntity(rowKey,partitionKey)
            {
                RowKey = rowKey,
                PartitionKey = partitionKey,
                Email = "Vatthanachai.w@gmail.com",
                Phone = "087-659-5578"
            };

            repository.SaveUser(user);

            var tmp = repository.GetUsers(rowKey, partitionKey);


            if (tmp.Any())
            {
                foreach (var t in tmp)
                {
                    return Content(string.Format("Firstname :- {0}, Lastname :- {1}, Email :- {2}, Phone :- {3}",
                        t.PartitionKey, t.RowKey, t.Email, t.Phone));
                }
            }

            return Content("Notting.");
        }

    }
}
