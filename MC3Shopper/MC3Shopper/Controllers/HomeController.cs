using MC3Shopper.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MC3Shopper.Controllers
{
    public class HomeController : Controller
    {
        private Database db;

        [Authorize]
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
        
        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}