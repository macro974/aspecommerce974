using LINQtoCSV;
using MC3Shopper.Models;
using MC3Shopper.ViewModel;
using Microsoft.Security.Application;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MC3Shopper.Controllers
{
    [RoutePrefix("Catalogue")]
    public class ProduitController : Controller
    {
        private readonly Database mb = new Database();

        // GET: Produit
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details(string id)
        {
            string arref = HttpUtility.UrlDecode(id.Replace('!', '%'));
            if (arref.Equals(""))
            {
                return PartialView("~/Views/Shared/_blank.cshtml");
            }
            var sys = new GestionSys(mb);
            Produit p = sys.ProductParRef(arref);
            return PartialView("_Details", p);
        }

        [ValidateInput(false)]
        [Route("{Stat02?}")]
        [Authorize]
        [HttpGet]
        //[Route ("Category/{State02}/{page:int:min(1)}")]
        public ActionResult Category(string Stat02 = null)
        {
            ViewBag.Category = Stat02;
            return View();
        }

        [Authorize]
        [ChildActionOnly]
        public ActionResult Menu()
        {
            var sys = new GestionSys(mb);
            List<string> menu = sys.FamillePourMenu();

            return PartialView(menu);
        }

        [Authorize]
        [ChildActionOnly]
        public ActionResult _fam(String Stat02 = "")
        {
            var sys = new GestionSys(mb);
            List<string> fam = sys.FamilleParCat(Stat02);
            ViewBag.fam = fam;
            ViewBag.Stat02 = Stat02;
            return PartialView();
        }

        [Authorize]
        //[OutputCache(Duration = 60)]
        [HttpGet]
        public ActionResult _getListProduct(string Stat02 = "", string Famille = "", int page = 1)
        {
            string arref = Stat02.Replace('_', '/');
            var sys = new GestionSys(mb);

            IPagedList<Produit> liste_perso =
                sys.TousAllProductByCat(arref, Famille, page)
                    .Where(x => x.QteEnCommande + x.StockDisponible > 0 && x.Prix > 0)
                    .ToPagedList(page, 25);
            var user = Security.DeSerialize<Utilisateur>(Session["user"].ToString());
            if(user ==null)
            {
                return RedirectToAction("Index", "Home");
            }
            sys.RemiseToListProduit(liste_perso.ToList(), user);
            ViewBag.Category = arref;

            ViewBag.famille = Famille;
            ViewBag.liste = liste_perso;

            //ViewBag.count = sys.CountGetAllProductByCat(Stat02, Famille);
            //ViewBag.current = page > 0 ? page : 1;
            return PartialView();
        }

        [Authorize]
        [Route("Download/{Stat02=''}/{Famille=''}")]
        public FileResult DownloadFicheArticle(string Stat02 = "", string Famille = "")
        {
            var sys = new GestionSys(mb);
            var fiche_article = new List<ArticleSto>();
            List<Produit> produit = sys.getAllProduitByRefAndFamille(Stat02, Famille);
            foreach (Produit item in produit)
            {
                if (item.StockDisponible + item.QteEnCommande > 0 && item.Prix > 0)
                {
                    var At = new ArticleSto();
                    At.Reference = item.Reference;
                    At.Description = item.Designation;
                    At.Qte_denis = item.StockDispo_denis > 0 ? item.StockDispo_denis : 0;
                    At.Qte_pierre = item.StockDispo_pierre > 0 ? item.StockDispo_pierre : 0;
                    At.Stockdisponible = item.StockDisponible;
                    At.prix = item.Prix;
                    if (item.QteEnCommande > 0)
                    {
                        At.arrivage = item.QteEnCommande + " le " + item.Disponibilite;
                    }
                    else
                    {
                        At.arrivage = "";
                    }
                    fiche_article.Add(At);
                }
            }
            var outputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ';', // tab delimited
                FirstLineHasColumnNames = true, // no column names in first record
                FileCultureName = "fr-FR" // use formats used in The Netherlands
            };
            var cc = new CsvContext();
            string chemin = Server.MapPath("~/CSVFileTemp/temp_csv.csv");
            cc.Write(
                fiche_article,
                chemin,
                outputFileDescription);
            return File(chemin, "text/csv", "MC3_Export_" + Stat02 + DateTime.Now + ".csv");
        }

        [HttpGet]
        public ActionResult Search(string Search, int page = 1)
        {
            String search_propre = Sanitizer.GetSafeHtmlFragment(Search);
            var sys = new GestionSys(mb);
            IPagedList<Produit> list_search = sys.SearchProduitsByUser(search_propre).Where(x => x.QteEnCommande + x.StockDisponible > 0 && x.Prix > 0)
                    .ToPagedList(page, 25);
            var user = Security.DeSerialize<Utilisateur>(Session["user"].ToString());
            sys.RemiseToListProduit(list_search.ToList(), user);
            ViewBag.search = search_propre;
            ViewBag.liste = list_search;

            return View();
        }

        [Route("Add")]
        [HttpPost]
        public JsonResult AddPanier(string AR_Ref, int Qte)
        {
            //string reference=Sanitizer.GetSafeHtml(AR_Ref);
            var sys = new GestionSys(mb);
            Produit p = sys.FindAndCheckProduitByRef(AR_Ref, Qte);
            var panier = Security.DeSerialize<Panier>(Session["Panier"].ToString());
            if (Qte == 0)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
            if (p != null)
            {
                try
                {
                    panier.AddToPanier(p);
                    Session["Panier"] = Security.Serialize(panier);

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }

            return Json("Fail", JsonRequestBehavior.AllowGet);
        }
    }
}