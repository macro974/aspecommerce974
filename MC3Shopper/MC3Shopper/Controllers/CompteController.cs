using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MC3Shopper.Models;
using System.Data.SqlClient;
using System.Web.Security;
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
        [AllowAnonymous]
        [HttpPost]
        public ActionResult SignIn (UserViewModel user , string returnUrl)
        {
            if(ModelState.IsValid)
            {
                Utilisateur monUser=null;;
                Database maDB = new Database();
                GestionSys sys = new GestionSys(maDB);
                string username = user.CodeClient;
                string password = user.Password;
                string statement = "SELECT * FROM F_CONTACTT WHERE CT_Num = @user AND CT_Prenom = @password AND (CT_Fonction = 'ACCES WEB2' OR CT_Fonction = 'ACCES WEB RESTREINT2')";
                SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);
                myCommand.Parameters.Add("@user", System.Data.SqlDbType.NVarChar).Value = username;
                myCommand.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = password;
                maDB.open();
                SqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                if(myReader.HasRows)
                {
                    FormsAuthentication.SetAuthCookie(username,false);
                    while(myReader.Read())
                    {
                        monUser= new Utilisateur(username);
                        monUser.Email= myReader["CT_Email"].ToString();
                        monUser.Password=password;
                        monUser.NomPre=myReader["CT_Nom"].ToString();
                    }
                    myReader.Close();
                    sys.RecupererListeFamilleRemise(monUser);
                    Session.Add("user",Security.Serialize(monUser));
                   
                    return RedirectToAction("Index", "Home");
                      
                }
                else {
                    ModelState.AddModelError("", "Le nom d'utilisateur et/ou le mot de passe ne correspond pas");
                }

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