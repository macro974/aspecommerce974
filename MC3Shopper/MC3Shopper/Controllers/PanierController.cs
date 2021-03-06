﻿using MC3Shopper.Models;
using System;
using System.Web.Mvc;

namespace MC3Shopper.Controllers
{
    public class PanierController : Controller
    {
        private readonly Database mb = new Database();

        // GET: Panier

        public ActionResult _GetPanier()
        {
            if (Session["Panier"] != null)
            {
                Panier panier = Security.DeSerialize<Panier>(Session["panier"].ToString());
                ViewBag.count = panier.monPanier.Count;
                ViewBag.panier = panier;
            }
            else
            {
                ViewBag.panier = null;
            }
            return PartialView();
        }

        [Route("Resume/")]
        public ActionResult _ResumePanier()
        {
            if (Session["Panier"] != null)
            {
                Panier panier = Security.DeSerialize<Panier>(Session["panier"].ToString());
                ViewBag.count = panier.monPanier.Count;
                ViewBag.panier = panier;
            }
            else
            {
                ViewBag.panier = null;
            }
            return PartialView();
        }

        public JsonResult CountPanier()
        {
            int count = 0;
            if (Session["Panier"] != null)
            {
                Panier panier = Security.DeSerialize<Panier>(Session["panier"].ToString());
                count = panier.monPanier.Count == null ? 0 : panier.monPanier.Count;

                return Json(count, JsonRequestBehavior.AllowGet);
            }

            return Json(count, JsonRequestBehavior.AllowGet);
        }

        //[Route("Commander")]
        public ActionResult CreerCommande()
        {
            var user = Security.DeSerialize<Utilisateur>(Session["user"].ToString());
            GestionSys sys = new GestionSys(mb, user);
            Panier panier = Security.DeSerialize<Panier>(Session["panier"].ToString());
            string a = sys.CreeCommande(panier, user, 1);
            ViewBag.a = a;
            return View();
        }

        [Route("Commander/")]
        public ActionResult ProcessStep()
        {
            return View();
        }

        public JsonResult Remove(string AR_Ref)
        {
            try
            {
                var panier = Security.DeSerialize<Panier>(Session["Panier"].ToString());
                panier.Suprrimer(AR_Ref);
                Session["Panier"] = Security.Serialize(panier);
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EditQuantity(string AR_ref, int Qte)
        {
            try
            {
                var panier = Security.DeSerialize<Panier>(Session["Panier"].ToString());

                panier.ChangerQuantite(AR_ref, Qte);
                Session["Panier"] = Security.Serialize(panier);
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }
    }
}