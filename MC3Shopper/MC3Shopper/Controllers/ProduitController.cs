using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using MC3Shopper.Models;
using MC3Shopper.ViewModel;
using PagedList;
namespace MC3Shopper.Controllers
{
    public class ProduitController : Controller
    {
       
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
        public ActionResult Category (string Stat02="",int page=1)
        {
            Database mb = new Database();
            GestionSys sys = new GestionSys(mb);
            List<Produit> liste_perso = sys.GetAllProduct(Stat02);
            var onePageOfProducts = liste_perso.ToPagedList(page, 25);
            ViewBag.Category = Stat02;
            ViewBag.onePageOfProducts = onePageOfProducts;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {

            GestionSys sys = new GestionSys(Session["maDB"] as Database);
            List<string> menu =sys.FamillePourMenu();
            
            
            return PartialView(menu);
        }
      

        

      
    }
}
