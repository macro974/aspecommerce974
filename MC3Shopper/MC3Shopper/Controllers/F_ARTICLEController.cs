using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MC3Shopper.Models;

namespace MC3Shopper.Controllers
{
    public class F_ARTICLEController : Controller
    {
        private MC3Reunion db = new MC3Reunion();

        // GET: F_ARTICLE
        public ActionResult Index()
        {
            return View(db.F_ARTICLE.ToList());
        }

        // GET: F_ARTICLE/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            F_ARTICLE f_ARTICLE = db.F_ARTICLE.Find(id);
            if (f_ARTICLE == null)
            {
                return HttpNotFound();
            }
            return View(f_ARTICLE);
        }

        // GET: F_ARTICLE/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: F_ARTICLE/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AR_Ref,FA_CodeFamille,cbMarq,AR_Design,cbAR_Design,AR_Substitut,cbAR_Substitut,AR_Raccourci,AR_Garantie,AR_UnitePoids,AR_PoidsNet,AR_PoidsBrut,AR_UniteVen,AR_PrixAch,AR_Coef,AR_PrixVen,AR_PrixTTC,AR_Gamme1,AR_Gamme2,AR_SuiviStock,AR_Nomencl,AR_Stat01,AR_Stat02,AR_Stat03,AR_Stat04,AR_Stat05,AR_Escompte,AR_Delai,AR_HorsStat,AR_VteDebit,AR_NotImp,AR_Sommeil,AR_Langue1,AR_Langue2,AR_CodeEdiED_Code1,AR_CodeEdiED_Code2,AR_CodeEdiED_Code3,AR_CodeEdiED_Code4,AR_CodeBarre,AR_CodeFiscal,AR_Pays,AR_Frais01FR_Denomination,AR_Frais01FR_Rem01REM_Valeur,AR_Frais01FR_Rem01REM_Type,AR_Frais01FR_Rem02REM_Valeur,AR_Frais01FR_Rem02REM_Type,AR_Frais01FR_Rem03REM_Valeur,AR_Frais01FR_Rem03REM_Type,AR_Frais02FR_Denomination,AR_Frais02FR_Rem01REM_Valeur,AR_Frais02FR_Rem01REM_Type,AR_Frais02FR_Rem02REM_Valeur,AR_Frais02FR_Rem02REM_Type,AR_Frais02FR_Rem03REM_Valeur,AR_Frais02FR_Rem03REM_Type,AR_Frais03FR_Denomination,AR_Frais03FR_Rem01REM_Valeur,AR_Frais03FR_Rem01REM_Type,AR_Frais03FR_Rem02REM_Valeur,AR_Frais03FR_Rem02REM_Type,AR_Frais03FR_Rem03REM_Valeur,AR_Frais03FR_Rem03REM_Type,AR_Condition,AR_PUNet,AR_Contremarque,AR_FactPoids,AR_FactForfait,AR_DateCreation,AR_SaisieVar,AR_Transfere,AR_Publie,AR_DateModif,AR_Photo,cbProt,cbCreateur,cbModification,cbReplication,cbFlag,SERIALNUMBER,cbAR_Ref,cbFA_CodeFamille,AR_PrixAchNouv,AR_CoefNouv,AR_PrixVenNouv,AR_DateApplication,AR_CoutStd,AR_QteComp,AR_QteOperatoire,CO_No,cbCO_No,AR_Prevision,CL_No1,cbCL_No1,CL_No2,cbCL_No2,CL_No3,cbCL_No3,CL_No4,cbCL_No4,AR_Type,RP_CodeDefaut,cbAR_Raccourci,cbAR_CodeBarre")] F_ARTICLE f_ARTICLE)
        {
            if (ModelState.IsValid)
            {
                db.F_ARTICLE.Add(f_ARTICLE);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(f_ARTICLE);
        }

        // GET: F_ARTICLE/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            F_ARTICLE f_ARTICLE = db.F_ARTICLE.Find(id);
            if (f_ARTICLE == null)
            {
                return HttpNotFound();
            }
            return View(f_ARTICLE);
        }

        // POST: F_ARTICLE/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AR_Ref,FA_CodeFamille,cbMarq,AR_Design,cbAR_Design,AR_Substitut,cbAR_Substitut,AR_Raccourci,AR_Garantie,AR_UnitePoids,AR_PoidsNet,AR_PoidsBrut,AR_UniteVen,AR_PrixAch,AR_Coef,AR_PrixVen,AR_PrixTTC,AR_Gamme1,AR_Gamme2,AR_SuiviStock,AR_Nomencl,AR_Stat01,AR_Stat02,AR_Stat03,AR_Stat04,AR_Stat05,AR_Escompte,AR_Delai,AR_HorsStat,AR_VteDebit,AR_NotImp,AR_Sommeil,AR_Langue1,AR_Langue2,AR_CodeEdiED_Code1,AR_CodeEdiED_Code2,AR_CodeEdiED_Code3,AR_CodeEdiED_Code4,AR_CodeBarre,AR_CodeFiscal,AR_Pays,AR_Frais01FR_Denomination,AR_Frais01FR_Rem01REM_Valeur,AR_Frais01FR_Rem01REM_Type,AR_Frais01FR_Rem02REM_Valeur,AR_Frais01FR_Rem02REM_Type,AR_Frais01FR_Rem03REM_Valeur,AR_Frais01FR_Rem03REM_Type,AR_Frais02FR_Denomination,AR_Frais02FR_Rem01REM_Valeur,AR_Frais02FR_Rem01REM_Type,AR_Frais02FR_Rem02REM_Valeur,AR_Frais02FR_Rem02REM_Type,AR_Frais02FR_Rem03REM_Valeur,AR_Frais02FR_Rem03REM_Type,AR_Frais03FR_Denomination,AR_Frais03FR_Rem01REM_Valeur,AR_Frais03FR_Rem01REM_Type,AR_Frais03FR_Rem02REM_Valeur,AR_Frais03FR_Rem02REM_Type,AR_Frais03FR_Rem03REM_Valeur,AR_Frais03FR_Rem03REM_Type,AR_Condition,AR_PUNet,AR_Contremarque,AR_FactPoids,AR_FactForfait,AR_DateCreation,AR_SaisieVar,AR_Transfere,AR_Publie,AR_DateModif,AR_Photo,cbProt,cbCreateur,cbModification,cbReplication,cbFlag,SERIALNUMBER,cbAR_Ref,cbFA_CodeFamille,AR_PrixAchNouv,AR_CoefNouv,AR_PrixVenNouv,AR_DateApplication,AR_CoutStd,AR_QteComp,AR_QteOperatoire,CO_No,cbCO_No,AR_Prevision,CL_No1,cbCL_No1,CL_No2,cbCL_No2,CL_No3,cbCL_No3,CL_No4,cbCL_No4,AR_Type,RP_CodeDefaut,cbAR_Raccourci,cbAR_CodeBarre")] F_ARTICLE f_ARTICLE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(f_ARTICLE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(f_ARTICLE);
        }

        // GET: F_ARTICLE/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            F_ARTICLE f_ARTICLE = db.F_ARTICLE.Find(id);
            if (f_ARTICLE == null)
            {
                return HttpNotFound();
            }
            return View(f_ARTICLE);
        }

        // POST: F_ARTICLE/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            F_ARTICLE f_ARTICLE = db.F_ARTICLE.Find(id);
            db.F_ARTICLE.Remove(f_ARTICLE);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
