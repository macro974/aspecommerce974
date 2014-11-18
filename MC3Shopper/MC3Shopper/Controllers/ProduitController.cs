﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using MC3Shopper.Models;
using MC3Shopper.ViewModel;
using System.Diagnostics;
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

        [Authorize]
        public ActionResult Details(string id)
        {
            string arref = HttpUtility.UrlDecode(id.Replace('!', '%'));
            if(arref.Equals(""))
            {
                return PartialView("~/Views/Shared/_blank.cshtml");
            }
            else
            {
                GestionSys sys = new GestionSys(mb);
                Produit p = sys.ProductParRef(arref);
                return PartialView("_Details",p);
            }
           
        }
        [Authorize]
        [HttpGet]
        //[Route ("Category/{State02}/{page:int:min(1)}")]
        public ActionResult Category(string Stat02 = "")
        {

            
           
            ViewBag.Category = Stat02;
            return View();
        }
        [Authorize]
        [ChildActionOnly]
        public ActionResult Menu()
        {

            GestionSys sys = new GestionSys(mb);
            List<string> menu =sys.FamillePourMenu();
            
            
            return PartialView(menu);
        }
        [Authorize]
        [ChildActionOnly]
        public ActionResult _fam(String Stat02="")
        {

            GestionSys sys = new GestionSys(mb);
            List<string> fam = sys.FamilleParCat(Stat02);
            ViewBag.fam = fam;
            ViewBag.Stat02 = Stat02;
            return PartialView();
        }
        [Authorize]
        [OutputCache(Duration = 60)]
        [HttpGet]
        public ActionResult _getListProduct(string Stat02="",string Famille="" ,int page=1)
        {
            GestionSys sys = new GestionSys(mb);
            Stopwatch sw = new Stopwatch();
            sw.Start(); 
            List<Produit> liste_perso = sys.GetAllProductByCAT(Stat02, Famille, page).Where(x => x.QteEnCommande + x.StockDisponible > 0).ToList();
            sw.Stop();
            ViewBag.Category = Stat02;
            
            ViewBag.famille = Famille;
            ViewBag.liste = liste_perso;
            sw.Restart();

            ViewBag.count = sys.CountGetAllProductByCat(Stat02, Famille);
            sw.Stop();
            Debug.WriteLine(" temps fonction count est de :{0}", sw.Elapsed);
            ViewBag.current = page > 0 ? page : 1;
            return PartialView();
        }

      
    }
}
