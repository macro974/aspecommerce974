using MC3Shopper.Models;
using System.Web.Mvc;

namespace MC3Shopper.Controllers
{
    public class PanierController : Controller
    {
        private readonly Database mb = new Database();

        // GET: Panier
        public ActionResult GetPanier()
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
            Panier panier = Security.DeSerialize<Panier>(Session["panier"].ToString());
            string a = GestionSys.CreeCommande(panier, user, 1);
            ViewBag.a = a;
            return View();
        }

        [Route("Commander")]
        public ActionResult ProcessStep()
        {
            return View();
        }

        public ActionResult _ProcessStep1()
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
            return PartialView("~/Views/Panier/ProcessStep1.cshtml");
        }

        public ActionResult ProcessStep2()
        {
            return PartialView();
        }

        public ActionResult ProcessStep3()
        {
            return PartialView();
        }
    }
}