using MC3Shopper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MC3Shopper.Controllers
{
    public class ProduitController : Controller
    {
        private Database mb = new Database();

        // GET: Produit
        public ActionResult Index()
        {
            return View();
        }

        // GET: Produit/Details/5
        public ActionResult Details(string id)
        {
            return View();
        }

        [HttpGet]
        //[Route ("Category/{State02}/{page:int:min(1)}")]
        public ActionResult Category(string Stat02 = "", string Famille = "", int page = 1)
        {
            GestionSys sys = new GestionSys(mb);
            List<Produit> liste_perso = sys.GetAllProductByCAT(Stat02, Famille, page).Where(x => x.QteEnCommande + x.StockDisponible > 0).ToList();
            ViewBag.Category = Stat02;

            ViewBag.famille = Famille;
            ViewBag.liste = liste_perso;
            ViewBag.count = sys.CountGetAllProductByCat(Stat02, Famille);
            ViewBag.current = page > 0 ? page : 1;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            GestionSys sys = new GestionSys(mb);
            List<string> menu = sys.FamillePourMenu();

            return PartialView(menu);
        }

        [ChildActionOnly]
        public ActionResult _fam(String Stat02 = "")
        {
            GestionSys sys = new GestionSys(mb);
            List<string> fam = sys.FamilleParCat(Stat02);
            ViewBag.fam = fam;
            ViewBag.Stat02 = Stat02;
            return PartialView(fam);
        }
    }
}