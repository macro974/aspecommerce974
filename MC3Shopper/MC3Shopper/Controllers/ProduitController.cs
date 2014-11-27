using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LINQtoCSV;
using MC3Shopper.Models;
using MC3Shopper.ViewModel;
using PagedList;

namespace MC3Shopper.Controllers
{
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
        [OutputCache(Duration = 60)]
        [HttpGet]
        public ActionResult _getListProduct(string Stat02 = "", string Famille = "", int page = 1)
        {
            var sys = new GestionSys(mb);

            IPagedList<Produit> liste_perso =
                sys.TousAllProductByCat(Stat02, Famille, page)
                    .Where(x => x.QteEnCommande + x.StockDisponible > 0 && x.Prix > 0)
                    .ToPagedList(page, 25);
            var user = Security.DeSerialize<Utilisateur>(Session["user"].ToString());
            sys.RemiseToListProduit(liste_perso.ToList(), user);
            ViewBag.Category = Stat02;

            ViewBag.famille = Famille;
            ViewBag.liste = liste_perso;

            //ViewBag.count = sys.CountGetAllProductByCat(Stat02, Famille);
            //ViewBag.current = page > 0 ? page : 1;
            return PartialView();
        }

        [Authorize]
        public FileResult DownloadFicheArticle(string Stat02 = "", string Famille = "")
        {
            var sys = new GestionSys(mb);
            var fiche_article = new List<ArticleSto>();
            List<Produit> produit = sys.getAllProduitByRefAndFamille(Stat02, Famille);
            foreach (Produit item in produit)
            {
                if (item.StockDisponible + item.QteEnCommande > 0)
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
            return File(chemin, "application/ms-excel", "MC3_Export_" + Stat02 + DateTime.Now + ".csv");
        }
    }
}