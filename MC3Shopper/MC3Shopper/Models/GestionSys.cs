﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.IO;
using System.Xml.Serialization;
namespace MC3Shopper.Models
{
    public class GestionSys
    {
        Database maDB;
        Database dbObject;
        Utilisateur Utilisateur;
        BackgroundWorker backGroundW;
        public int count { get; set; }
        public GestionSys(Database DB)
        {
            maDB = DB;
            dbObject = DB;
        }

        public GestionSys(Database DB, Utilisateur user)
        {
            maDB = DB;
            dbObject = DB;
            Utilisateur = user;
        }
       

        public GestionSys(Database DB, Utilisateur user, BackgroundWorker bkg)
        {
            maDB = DB;
            dbObject = DB;
            Utilisateur = user;
            backGroundW = bkg;
        }

        public List<Produit>ProductByEvent(int evenement)
        {
            List<Produit> maListe = new List<Produit>();
            string statement="";
            
            
            switch (evenement)
            {   case 1:// nouveaux produits
                    statement = "select top 10 AR_Ref,AR_Design,AR_PrixVen,AR_Stat01 from F_ARTICLE  where ar_publie=1 and ar_sommeil=0 and AR_PrixVen>0 order by AR_DateCreation DESC ";
                    break;
                case 2: // Promotions
                    statement = "select top 10 AR_Ref,AR_Design,AR_PrixVen,AR_Stat01 from F_ARTICLE where Lower(AR_Design) LIKE '%promo%' and ar_publie=1 and ar_sommeil=0 and AR_PrixVen>0 order by AR_DateModif DESC";
                    break;
                case 3: // destockage
                    statement = "select top 10 AR_Ref,AR_Design,AR_PrixVen,AR_Stat01 from F_ARTICLE where Lower(AR_Design) LIKE '%destockage%' AND Lower(AR_Design) NOT LIKE '%promo%'  and ar_publie=1 and ar_sommeil=0 and AR_PrixVen>0 order by AR_DateModif DESC";
                    break;
                default:
                    break;
            }
            
            SqlCommand myCommand = new SqlCommand(statement, dbObject.myConnection);
            dbObject.open();
            SqlDataReader myReader = myCommand.ExecuteReader();
            while(myReader.Read())
            {
                Produit monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(), myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));
                maListe.Add(monProduit);
            }
            myReader.Close();
            dbObject.close();
            return maListe;
        }
        public int CountGetAllProductByCat(string codestat,string famille)
        {
            maDB.open();
            int i=0;
            string statement = "SELECT DISTINCT count(F_ARTICLE.AR_Ref) AS NUMBER from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref " +
            "WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND AR_Stat02=@state  AND AR_Stat01 LIKE @famille";
            SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@state", System.Data.SqlDbType.NVarChar, 50);
            myCommand.Parameters["@state"].Value = codestat;
            myCommand.Parameters.Add("@famille", System.Data.SqlDbType.NVarChar).Value = "%" + famille + "%";
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                i = int.Parse(myReader["NUMBER"].ToString())/30;
            }
            maDB.close();
            return i;
        }
      
        public void GetQteCommandeProduit(List<Produit> p)
        {
             
            List<Produit> maListe = new List<Produit>();
            foreach (var item in p)
            {
                maDB.open();
                string statement = "SELECT DISTINCT DL_Qte,DO_DateLivr FROM F_DOCLIGNE WHERE DO_Type=12 AND DO_DateLivr >=(SELECT GETDATE()) AND AR_Ref=@ref";
                SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);
                myCommand.Parameters.Add("@ref", System.Data.SqlDbType.NVarChar, 50).Value = item.Reference;
                SqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                if (!myReader.HasRows)
                {
                    item.QteEnCommande = 0f;
                    item.Disponibilite = "";
                }
                else
                {
                    while (myReader.Read())
                    {
                        item.QteEnCommande = float.Parse(myReader["DL_Qte"].ToString());
                        item.Disponibilite = myReader["Do_DateLivr"].ToString();
                    }
                }
                myReader.Close();
                maDB.close();
            }
                 
            
        }
        public List<Produit> GetAllProductByCAT(string codestat,string famille,int NumberPage=1)
        {
            
            maDB.open();
            List<Produit> maListe = new List<Produit>();
            string statement = "SELECT * FROM (select DISTINCT ROW_NUMBER() OVER(ORDER BY F_Article.AR_Ref) AS NUMBER ,F_Article.AR_Ref,AR_Design,AR_PrixVen,AS_QteSto-AS_QteRes AS QTE,AS_MontSto " + 
                                "from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref "+
                                "WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND AR_Stat02=@state AND AR_Stat01 LIKE @famille) AS TBL " +
                                "WHERE NUMBER BETWEEN ((@PageNumber - 1) * 30 + 1) AND (@PageNumber * 30)";            
            SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@state", System.Data.SqlDbType.NVarChar, 50);
            myCommand.Parameters["@state"].Value = codestat;
            myCommand.Parameters.Add("@PageNumber", System.Data.SqlDbType.Int).Value = NumberPage;
            myCommand.Parameters.Add("@famille", System.Data.SqlDbType.NVarChar).Value = "%" + famille + "%";
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                Produit monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(), myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));
                monProduit.StockDispo_denis = float.Parse(myReader["QTE"].ToString()) < 0 ? 0 : float.Parse(myReader["QTE"].ToString());
                
                // get stock st pierre
                maListe.Add(monProduit);
            }
            maDB.close();
            foreach (Produit item in maListe)
            {
                maDB.open();
                string statement2 = "select Distinct AS_QteSto-AS_QteRes AS pierre from F_ARTSTOCK where DE_No=2 AND AR_Ref=@ref";
                SqlCommand myCommand2 = new SqlCommand(statement2, maDB.myConnection);
                myCommand2.Parameters.Add("@ref", System.Data.SqlDbType.NVarChar, 50);
                myCommand2.Parameters["@ref"].Value = item.Reference;

                SqlDataReader myReader2 = null;
                myReader2 = myCommand2.ExecuteReader();
                while (myReader2.Read())
                {
                    item.StockDispo_pierre = float.Parse(myReader2["pierre"].ToString()) < 0 ? 0 : float.Parse(myReader2["pierre"].ToString());
                    //item.StockDisponible = item.StockDispo_pierre + item.StockDispo_denis;
                }
                maDB.close();
            }
            GetQteCommandeProduit(maListe);
            return maListe;

        }
        public List<Produit> ProduitsParCodeStat(string codeStat)
        {
            List<Produit> maListe = new List<Produit>();
            int ColonneParPage=15;
            

            string statement = "select DISTINCT F_Article.AR_Ref,AR_Design,AR_PrixVen,AS_QteSto,AS_QteRes,AS_MontSto,F_Article.FA_CodeFamille from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND AR_Stat02=@state";
                                            
            SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);

            /** ##################### Ajout des parametres #############################**/

            myCommand.Parameters.Add("@state", System.Data.SqlDbType.NVarChar, 50);
            myCommand.Parameters["@state"].Value = codeStat;
          
            /** ########################## Fin #####################################*/
            maDB.open();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                Produit monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(), myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));
                float stock = float.Parse(myReader["AS_QteSto"].ToString());
                float stockRes = float.Parse(myReader["AS_QteRes"].ToString());
                monProduit.StockDispo_denis = stock - stockRes;
                monProduit.StockDisponible = monProduit.StockDispo_denis + monProduit.StockDispo_pierre;
                monProduit.StockRes = stockRes;
                decimal montSto = decimal.Parse(myReader["AS_MontSto"].ToString());
                if (stock > 0)
                {
                    monProduit.CMUP = montSto / decimal.Parse(stock.ToString());
                }
                monProduit.CodeFamille = myReader["FA_CodeFamille"].ToString();
                if (monProduit.StockDisponible <= 0)
                {
                    foreach (lignedocument item in backGroundW.Achats)
                    {
                        if (item.AR_Ref.Equals(monProduit.Reference))
                        {
                            monProduit.QteEnCommande += item.DL_Qte;
                            monProduit.Disponibilite = item.dateArrivee;
                        }
                    }
                }
                foreach (KeyValuePair<string, float> item in Utilisateur.Remises)
                {
                    if (item.Key.Equals(monProduit.CodeFamille))
                    {
                        monProduit.Remise = item.Value;
                    }
                }
                maListe.Add(monProduit);
            }


            maDB.close();

            foreach (Produit item in maListe)
            {
                dbObject.open();
                string statement2 = "select Distinct AS_QteSto-AS_QteRes AS pierre from F_ARTSTOCK where DE_No=2 AND AR_Ref=@ref";
                SqlCommand myCommand2 = new SqlCommand(statement2, maDB.myConnection);
                myCommand2.Parameters.Add("@ref", System.Data.SqlDbType.NVarChar, 50);
                myCommand2.Parameters["@ref"].Value = item.Reference;

                SqlDataReader myReader2 = null;
                myReader2 = myCommand2.ExecuteReader();
                while (myReader2.Read())
                {
                    item.StockDispo_pierre = float.Parse(myReader2["pierre"].ToString());
                    item.StockDispo_denis = item.StockDisponible;
                    item.StockDisponible = item.StockDispo_pierre + item.StockDispo_denis;
                }
                dbObject.close();
            }

            foreach (Produit item in maListe)
            {

                dbObject.open();
                //statement = "SELECT * FROM F_ARTCOMPTA INNER JOIN F_TAXE ON F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe1 WHERE     (F_ARTCOMPTA.AR_Ref = '" + item.Reference + "')";
                //statement = "SELECT * FROM F_ARTCOMPTA INNER JOIN F_TAXE ON F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe1 WHERE     (F_ARTCOMPTA.AR_Ref = '" + item.Reference + "')";
                statement = @"SELECT     *
                    FROM         F_ARTCOMPTA INNER JOIN
                      F_TAXE ON F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe1 OR F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe2 OR 
                      F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe3
            WHERE     (F_ARTCOMPTA.AR_Ref = '" + item.Reference + "') AND (F_ARTCOMPTA.ACP_Champ = 1) AND (F_ARTCOMPTA.ACP_Type = 0)";
                myCommand = new SqlCommand(statement, dbObject.myConnection);

                myReader = null;
                myReader = myCommand.ExecuteReader();
                //  
                while (myReader.Read())
                {
                   
                    item.Taxes[myReader["TA_Intitule"].ToString()] = float.Parse(myReader["TA_Taux"].ToString());
                    
                }
                dbObject.close();

                dbObject.open();
                statement = "SELECT * FROM F_ARTICLE INNER JOIN F_NOMENCLAT ON F_ARTICLE.AR_Ref = F_NOMENCLAT.NO_RefDet AND F_ARTICLE.AR_Ref = F_NOMENCLAT.NO_RefDet AND F_NOMENCLAT.AR_Ref = '" + item.Reference + "'";
                myCommand = new SqlCommand(statement, dbObject.myConnection);

                myReader = null;
                myReader = myCommand.ExecuteReader();
                //  
                while (myReader.Read())
                {
                    Produit monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(), myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));

                    item.ProduitAssocies.Add(monProduit);
                }
                dbObject.close();
            }
            
           
            return maListe;
        }
        public Utilisateur GetMyAccountInfos(Utilisateur monUser)
        {
            dbObject.myConnection.Open();
            string statement = "select * from F_COMPTET WHERE CT_Num = '" + monUser.CodeClient + "'";
            SqlCommand myCommand = new SqlCommand(statement, dbObject.myConnection);

            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            //  
            while (myReader.Read())
            {
                monUser.Intitule = myReader["CT_Intitule"].ToString();
                monUser.Adresse = myReader["CT_Adresse"].ToString();
                monUser.ComplementAdresse = myReader["CT_Complement"].ToString();
                monUser.CodePostal = myReader["CT_CodePostal"].ToString();
                monUser.Ville = myReader["CT_Ville"].ToString();
                monUser.Pays = myReader["CT_Pays"].ToString();
                monUser.CodeApe = myReader["CT_Ape"].ToString();
                monUser.SIRET = myReader["CT_Siret"].ToString();
                monUser.Telephone = myReader["CT_Telephone"].ToString();
                monUser.Telecopie = myReader["CT_Telecopie"].ToString();
                monUser.Email = myReader["CT_Email"].ToString();
                monUser.EncoursMax = myReader["CT_Encours"].ToString();
                monUser.EncoursAss = myReader["CT_Assurance"].ToString();
            }
            dbObject.myConnection.Close();

            monUser.Factures = this.recupererEnteteDocumentByType(6, monUser);



            return monUser;
        }
        public List<entetedocument> recupererEnteteDocumentByType(int DO_Type)
        {
            dbObject.open();
            SqlCommand myCommand = new SqlCommand("select * from F_DOCENTETE WHERE (DO_Type = '" + DO_Type + "') ORDER BY DO_Date", dbObject.myConnection);
            List<entetedocument> liste = new List<entetedocument>();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                entetedocument fiche = new entetedocument();
                fiche = remplirEntete(myReader);

                liste.Add(fiche);
            }
            dbObject.myConnection.Close();
            return liste;
        }
        public List<entetedocument> recupererEnteteDocumentByType(int DO_Type, Utilisateur client)
        {
            dbObject.open();
            SqlCommand myCommand = new SqlCommand("select * from F_DOCENTETE WHERE (DO_Type = '" + DO_Type + "') AND (DO_Tiers='" + client.CodeClient + "') ORDER BY DO_Date DESC", dbObject.myConnection);
            List<entetedocument> liste = new List<entetedocument>();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                entetedocument fiche = new entetedocument();
                fiche = remplirEntete(myReader);

                liste.Add(fiche);
            }
            dbObject.myConnection.Close();
            return liste;
        }
        public List<lignedocument> recupererLigneDocumentByListe(List<entetedocument> liste)
        {
            dbObject.myConnection.Open();
            string statement = "select * from F_DOCLIGNE WHERE DO_Piece = 'XX'";
            List<lignedocument> ligneDoc = new List<lignedocument>();
            foreach (entetedocument item in liste)
            {
                statement = statement + " OR DO_Piece = '" + item.DO_Piece + "'";
            }
            statement = statement + " ORDER BY DO_Date";
            SqlCommand myCommand = new SqlCommand(statement, dbObject.myConnection);

            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            //  
            while (myReader.Read())
            {
                lignedocument fiche = new lignedocument();
                fiche = remplirLigne(myReader);
                ligneDoc.Add(fiche);
            }
            dbObject.myConnection.Close();
            return ligneDoc;
        }

        public List<lignedocument> recupererLigneDocumentByType(int DO_Type)
        {
            List<lignedocument> maliste = new List<lignedocument>();
            maDB.open();
            SqlCommand myCommand = new SqlCommand("select * from F_DOCENTETE WHERE (DO_Type = '" + DO_Type + "') ORDER BY DO_Date", dbObject.myConnection);
            List<entetedocument> liste = new List<entetedocument>();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                entetedocument fiche = new entetedocument();
                fiche = remplirEntete(myReader);
                liste.Add(fiche);
            }
            maDB.myConnection.Close();

            foreach (entetedocument item in liste)
            {
                maDB.open();
                string statement = "select * from F_DOCLIGNE WHERE DO_Piece = '" + item.DO_Piece + "'";
                myCommand = new SqlCommand(statement, maDB.myConnection);

                myReader = null;
                myReader = myCommand.ExecuteReader();
                //  
                while (myReader.Read())
                {
                    lignedocument fiche = new lignedocument();
                    fiche = remplirLigne(myReader);
                    DateTime dateTemp = item.DO_DateLivr;
                    if (dateTemp.Year == 1900)
                    {
                    }
                    else
                    {
                        fiche.dateArrivee = dateTemp.ToString("dd/MM/yyyy");
                    }
                    maliste.Add(fiche);
                }
                maDB.close();
            }
            return maliste;
        }
        public lignedocument remplirLigne(SqlDataReader myReader)
        {
            lignedocument fiche = new lignedocument();
            fiche.AF_RefFourniss = myReader["AF_RefFourniss"].ToString();
            fiche.AR_Ref = myReader["AR_Ref"].ToString();
            fiche.cbCreateur = myReader["cbCreateur"].ToString();
            fiche.cbMarq = int.Parse(myReader["cbMarq"].ToString());
            fiche.CT_Num = myReader["CT_Num"].ToString();
            fiche.DE_No = int.Parse(myReader["DE_No"].ToString());
            fiche.DL_CMUP = double.Parse(myReader["DL_CMUP"].ToString());
            fiche.DL_DateBC = DateTime.Parse(myReader["DL_DateBC"].ToString());
            fiche.DL_DateBL = DateTime.Parse(myReader["DL_DateBL"].ToString());
            fiche.DL_Design = myReader["DL_Design"].ToString(); ;
            fiche.DL_Ligne = int.Parse(myReader["DL_Ligne"].ToString());
            fiche.DL_MontantHT = double.Parse(myReader["DL_MontantHT"].ToString());
            fiche.DL_MontantTTC = double.Parse(myReader["DL_MontantTTC"].ToString());
            fiche.DL_MvtStock = int.Parse(myReader["DL_MvtStock"].ToString());
            fiche.DL_No = int.Parse(myReader["DL_No"].ToString());
            fiche.DL_PieceBC = myReader["DL_PieceBC"].ToString();
            fiche.DL_PrixRU = double.Parse(myReader["DL_PrixRU"].ToString());
            fiche.DL_PrixUnitaire = double.Parse(myReader["DL_PrixUnitaire"].ToString()); ;
            fiche.DL_PUDevise = double.Parse(myReader["DL_PUDevise"].ToString());
            fiche.DL_PUTTC = double.Parse(myReader["DL_PUTTC"].ToString());
            fiche.DL_Qte = float.Parse(myReader["DL_Qte"].ToString());
            fiche.DL_QteBC = float.Parse(myReader["DL_QteBC"].ToString());
            fiche.DL_QteBL = float.Parse(myReader["DL_QteBL"].ToString());
            fiche.DL_TTC = int.Parse(myReader["DL_TTC"].ToString());
            fiche.DO_Date = DateTime.Parse(myReader["DO_Date"].ToString());
            fiche.DO_Piece = myReader["DO_Piece"].ToString();
            fiche.DO_Ref = myReader["DO_Ref"].ToString();
            fiche.DO_Type = int.Parse(myReader["DO_Type"].ToString());
            fiche.EU_Qte = float.Parse(myReader["EU_Qte"].ToString());
            fiche.dateArrivee = myReader["DATE_ARRIVEE"].ToString();
            fiche.DO_DateLivr = DateTime.Parse(myReader["DO_DateLivr"].ToString());
            return fiche;
        }
        public entetedocument remplirEntete(SqlDataReader myReader)
        {
            entetedocument fiche = new entetedocument();
            fiche.DO_Piece = myReader["DO_Piece"].ToString();
            fiche.CT_NumPayeur = myReader["CT_NumPayeur"].ToString();
            fiche.DE_No = int.Parse(myReader["DE_No"].ToString());
            fiche.DO_Date = DateTime.Parse(myReader["DO_Date"].ToString());
            fiche.DO_Type = int.Parse(myReader["DO_Type"].ToString());
            fiche.DO_Coord01 = myReader["DO_Coord01"].ToString();
            fiche.DO_Coord02 = myReader["DO_Coord02"].ToString();
            fiche.DO_Coord03 = myReader["DO_Coord03"].ToString();
            fiche.DO_Coord04 = myReader["DO_Coord04"].ToString();
            fiche.DO_DateLivr = DateTime.Parse(myReader["DO_DateLivr"].ToString());
            fiche.DO_Domaine = int.Parse(myReader["DO_Domaine"].ToString());
            fiche.DO_Expedit = int.Parse(myReader["DO_Expedit"].ToString());
            fiche.DO_Period = int.Parse(myReader["DO_Period"].ToString());
            fiche.DO_Ref = myReader["DO_Ref"].ToString();
            fiche.DO_Regime = int.Parse(myReader["DO_Regime"].ToString());
            fiche.DO_Tiers = myReader["DO_Tiers"].ToString();
            fiche.DO_transaction = int.Parse(myReader["DO_Transaction"].ToString());
            fiche.LI_No = int.Parse(myReader["LI_No"].ToString());
            //fiche.RE_No = int.Parse(myReader["RE_No"].ToString());
            return fiche;
        }
        public static float GetTVAOfProduit(string reference)
        {
            float taux = 0;
            Database dbObject = new Database();
            dbObject.open();
            string statement = "SELECT * FROM F_ARTCOMPTA INNER JOIN F_TAXE ON F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe1 WHERE     (F_ARTCOMPTA.AR_Ref = '" + reference + "')";
            SqlCommand myCommand = new SqlCommand(statement, dbObject.myConnection);

            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            //  
            while (myReader.Read())
            {
                //item.Taxes.Add(myReader["TA_Intitule"].ToString(), float.Parse(myReader["TA_Taux"].ToString()));
                taux = float.Parse(myReader["TA_Taux"].ToString());
                //item.Taxes[myReader["TA_Intitule"].ToString()] = float.Parse(myReader["TA_Taux"].ToString());
            }
            dbObject.close();

            return taux;
        }
        public List<String> FamillePourMenu()
        {
            
            String[] blacklist = { "", "FINANCIER", "INUTILE", "TRANSPORT" };
            List<string> maListe = new List<string>();
            string statement = "SELECT DISTINCT AR_Stat02 from F_ARTICLE WHERE AR_Stat02 NOT IN (' ','FINANCIER','TRANSPORT','INUTILE','MARKETING') ORDER BY AR_STAT02 ASC";
            SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {

                maListe.Add(myReader["AR_Stat02"].ToString().Trim());



            }
            maDB.close();
            // traitment des  inutile 

            return maListe;
        }
        public List<String> FamilleParCat(string Stat02)
        {
            List<string> maListe = new List<string>();
            string statement = "SELECT DISTINCT AR_Stat01 from F_ARTICLE WHERE AR_Stat02=@Stat02 ORDER BY AR_Stat01 ASC";
            SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);

            myCommand.Parameters.Add("@Stat02", System.Data.SqlDbType.NVarChar, 50);
            myCommand.Parameters["@Stat02"].Value = Stat02;
            maDB.open();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {

                maListe.Add(myReader["AR_Stat01"].ToString().Trim());



            }
            maDB.close();
            // traitment des  inutile 

            return maListe;
        }
        public static string GetNumeroBC()
        {

            //ALTER DATABASE "nomDeLaBase" SET ARITHABORT ON

            string numero = "";
            Database dbObject = new Database();
            dbObject.open();
            string statement = "SELECT TOP 1 DO_Piece, cbMarq FROM F_DOCENTETE WHERE (DO_Type = 1) ORDER BY cbMarq DESC";
            SqlCommand myCommand = new SqlCommand(statement, dbObject.myConnection);

            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            //  
            while (myReader.Read())
            {
                string intermediaire = myReader["DO_Piece"].ToString();
                int numBC = int.Parse(intermediaire.Split('W')[1]);
                int newNum = numBC + 1;
                numero = "W" + newNum;
            }
            dbObject.close();
            return numero;

        }
        static public string EncodeTo64(string toEncode)
        {

            byte[] toEncodeAsBytes

                  = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue

                  = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;

        }
        static public string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;

        }
    }
}