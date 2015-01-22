using MC3Shopper.Models;
using System.Collections.Generic;
using System.Linq;
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
            var masys = new GestionSys(db);
            List<Produit> liste_arrivage = masys.ProductByEvent(1).Where(x => x.QteEnCommande + x.StockDisponible > 0 && x.Prix > 0).ToList();
            List<Produit> liste_promotions = masys.ProductByEvent(2).Where(x => x.QteEnCommande + x.StockDisponible > 0 && x.Prix > 0).ToList();
            List<Produit> liste_destockage = masys.ProductByEvent(3).Where(x => x.QteEnCommande + x.StockDisponible > 0 && x.Prix > 0).ToList();

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

        [Authorize]
        public ActionResult CGV()
        {
            return View();
        }
    }
}