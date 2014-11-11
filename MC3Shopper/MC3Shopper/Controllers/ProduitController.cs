﻿using System;
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
        public ActionResult Category (string Stat02="",int page=1)
        {

            
            GestionSys sys = new GestionSys(mb);
            List<Produit> liste_perso = sys.GetAllProductByCAT(Stat02, page) ;
            ViewBag.Category = Stat02;
            ViewBag.liste = liste_perso;
            ViewBag.count =liste_perso.Count;
            ViewBag.current = page>0?page:1;
            return View();
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {

            GestionSys sys = new GestionSys(mb);
            List<string> menu =sys.FamillePourMenu();
            
            
            return PartialView(menu);
        }
      

        

      
    }
}
