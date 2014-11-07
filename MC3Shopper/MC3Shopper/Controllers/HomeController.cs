using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MC3Shopper.Models;

namespace MC3Shopper.Controllers
{
    public class HomeController : Controller
    {
        Database db;
        public ActionResult Index()
        {
            db = new Database();
            Session.Add("maDB", db);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "walter test ";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}