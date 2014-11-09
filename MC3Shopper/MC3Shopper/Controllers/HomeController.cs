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
            GestionSys masys = new GestionSys(db);
            List<Produit> liste_arrivage = masys.ProductByEvent(1);
            List<Produit> liste_promotions = masys.ProductByEvent(2);
            List<Produit> liste_destockage = masys.ProductByEvent(3);

            ViewBag.arrivage = liste_arrivage;
            ViewBag.promo = liste_promotions;
            ViewBag.destock = liste_destockage;
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