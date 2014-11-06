using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using MC3Shopper.Models;
namespace MC3Shopper.Controllers
{
    public class ProduitController : Controller
    {
        private MC3Reunion db = new MC3Reunion();
        // GET: Produit
        public ActionResult Index()
        {
            return View();
        }

        // GET: Produit/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }



        public ActionResult Menu()
        {
            String[] exclude = {"TRANSPORT","","FINANCIER",""};

            var request = (from m in db.F_ARTICLE where !exclude.Contains(m.AR_Stat02) select m.AR_Stat02).Distinct() ;
            List<string> list = request.ToList<string>();
            ViewBag.request = list;
            return View();
        }
      

      

      
    }
}
