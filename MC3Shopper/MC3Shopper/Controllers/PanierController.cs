using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MC3Shopper.Models;

namespace MC3Shopper.Controllers
{
    public class PanierController : Controller
    {
        // GET: Panier
        public ActionResult GetPanier()
        {
            if (Session["Panier"] != null)
            {
                Panier panier = Security.DeSerialize<Panier>(Session["panier"].ToString());
                ViewBag.panier = panier;
            }
            else
            {
                ViewBag.panier = null;
            }
            return PartialView();
        }

    }
}