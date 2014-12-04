using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;
using MC3Shopper.Models;

namespace MC3Shopper.Controllers
{
    public class CompteController : Controller
    {
        // GET: Compte

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View();
        }
        [Authorize()]
        public ActionResult MonCompte()
        {
            var monUser = Security.DeSerialize<Utilisateur>(Session["user"].ToString()); ;
            Database maDB = Session["maDB"] as Database;
            GestionSys maSys = new GestionSys(maDB, monUser);
            if (monUser != null && monUser.Factures == null)
            {
                
                monUser = maSys.GetMyAccountInfos(monUser);
                monUser.Statisitques = maSys.GetStatAtToday(monUser);
            }
            List<lignedocument> dernier_commande=maSys.RecupererLignedocumentsByTypeAndNum(1,monUser.CodeClient);
            List<lignedocument> factures=maSys.RecupererLignedocumentsByTypeAndNum(7,monUser.CodeClient);
            ViewBag.monUser = monUser;
            ViewBag.historique = dernier_commande;
            ViewBag.factures = factures;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult SignIn(UserViewModel user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Utilisateur monUser = null;
                ;
                var maDB = new Database();
                var sys = new GestionSys(maDB);
                string username = user.CodeClient;
                string password = user.Password;
                const string statement = "SELECT * FROM F_CONTACTT WHERE CT_Num = @user AND CT_Prenom = @password AND (CT_Fonction = 'ACCES WEB2' OR CT_Fonction = 'ACCES WEB RESTREINT2' OR CT_Fonction='ACCES WEB')";
                var myCommand = new SqlCommand(statement, maDB.myConnection);
                myCommand.Parameters.Add("@user", SqlDbType.NVarChar).Value = username;
                myCommand.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
                maDB.open();
                SqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                if (myReader.HasRows)
                {
                    FormsAuthentication.SetAuthCookie(username, user.RememberMe);
                    while (myReader.Read())
                    {
                        monUser = new Utilisateur(username);
                        monUser.Email = myReader["CT_Email"].ToString();
                        monUser.Password = password;
                        monUser.NomPre = myReader["CT_Nom"].ToString();
                    }
                    myReader.Close();
                    sys.RecupererListeFamilleRemise(monUser);
                    Session.Add("user", Security.Serialize(monUser));
                    //Session["user"] = monUser;
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Le nom d'utilisateur et/ou le mot de passe ne correspond pas");

                maDB.close();
            }
            return View(user);
        }

        // POST: /Account/LogOff
        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
    }
}