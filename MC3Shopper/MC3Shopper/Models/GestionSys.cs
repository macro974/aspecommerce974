using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;

namespace MC3Shopper.Models
{
    public class GestionSys
    {
        private readonly Utilisateur Utilisateur;
        private readonly BackgroundWorker backGroundW;
        private readonly Database dbObject;
        private readonly Database maDB;


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

        public List<Produit> ProductByEvent(int evenement)
        {
            var maListe = new List<Produit>();
            string statement = "";

            switch (evenement)
            {
                case 1: // nouveaux produits
                    statement =
                        "select top 10 AR_Ref,AR_Design,AR_PrixVen,AR_Stat01 from F_ARTICLE  where ar_publie=1 and ar_sommeil=0 and AR_PrixVen>0 order by AR_DateCreation DESC ";
                    break;

                case 2: // Promotions
                    statement =
                        "select top 10 AR_Ref,AR_Design,AR_PrixVen,AR_Stat01 from F_ARTICLE where Lower(AR_Design) LIKE '%promo%' and ar_publie=1 and ar_sommeil=0 and AR_PrixVen>0 order by AR_DateModif DESC";
                    break;

                case 3: // destockage
                    statement =
                        "select top 10 AR_Ref,AR_Design,AR_PrixVen,AR_Stat01 from F_ARTICLE where Lower(AR_Design) LIKE '%destockage%' AND Lower(AR_Design) NOT LIKE '%promo%'  and ar_publie=1 and ar_sommeil=0 and AR_PrixVen>0 order by AR_DateModif DESC";
                    break;

                default:
                    break;
            }

            var myCommand = new SqlCommand(statement, dbObject.myConnection);
            dbObject.open();
            SqlDataReader myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(),
                    myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));
                maListe.Add(monProduit);
            }
            myReader.Close();
            dbObject.close();
            return maListe;
        }

       

        public void ListArticleStock(List<Produit> P)
        {
            var p_copie = new List<Produit>();
            string statement2 =
                "select Distinct AR_Ref,AS_QteSto-AS_QteRes AS pierre from F_ARTSTOCK where DE_No=2 AND AR_Ref IN(";
            if (P.Count <= 0)
            {
            }
            else
            {
                for (int i = 0; i < P.Count; i++)
                {
                    if (i + 1 == P.Count)
                    {
                        statement2 += "'" + P[i].Reference + "')";
                    }
                    else
                    {
                        statement2 += "'" + P[i].Reference + "',";
                    }
                }
                maDB.open();
                var myCommand2 = new SqlCommand(statement2, maDB.myConnection);


                SqlDataReader myReader2 = null;
                myReader2 = myCommand2.ExecuteReader();
                while (myReader2.Read())
                {
                    var p = new Produit();
                    p.Reference = myReader2[0].ToString();
                    p.StockDispo_pierre = float.Parse(myReader2[1].ToString()) < 0
                        ? 0
                        : float.Parse(myReader2[1].ToString());
                    p_copie.Add(p);
                }
                myReader2.Close();
                maDB.close();
                foreach (Produit item in p_copie)
                {
                    foreach (Produit prod in P)
                    {
                        if (item.Reference.Equals(prod.Reference))
                        {
                            prod.StockDispo_pierre = item.StockDispo_pierre;
                        }
                    }
                }
            }
        }

        public void RemiseToListProduit(List<Produit> P, Utilisateur user)
        {
            foreach (Produit monProduit in P)
            {
                foreach (var item in user.Remises)
                {
                    if (item.Key.Equals(monProduit.CodeFamille))
                    {
                        monProduit.Remise = item.Value;
                    }
                }
            }
            foreach (Produit item in P)
            {
                if (item.Remise > 0)
                {
                    item.PrixFormate = item.Prix.ToString("0.00") + "€ (PP:" + item.PrixOriginal.ToString("0.00") + "€)";
                }
                else
                {
                    item.PrixFormate = item.Prix.ToString("0.00") + "€";
                }
            }
        }


        public void GEtqteCommandeProduit(List<Produit> p)
        {
            var p_copie = new List<Produit>();
            maDB.open();
            string statement =
                "SELECT DISTINCT AR_Ref,DL_Qte,DO_DateLivr FROM F_DOCLIGNE WHERE DO_Type=12 AND DO_DateLivr >=(SELECT GETDATE()) AND AR_Ref IN(";
            if (p.Count <= 0)
            {
            }
            else
            {
                for (int i = 0; i < p.Count; i++)
                {
                    if (i + 1 == p.Count)
                    {
                        statement += "'" + p[i].Reference + "')";
                    }
                    else
                    {
                        statement += "'" + p[i].Reference + "',";
                    }
                }
                var myCommand = new SqlCommand(statement, maDB.myConnection);
                //myCommand.Parameters.Add("@ref", System.Data.SqlDbType.NVarChar, 50).Value = item.Reference;
                SqlDataReader myReader = null;
                myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    var pr = new Produit();
                    pr.Reference = myReader[0].ToString();
                    pr.QteEnCommande = float.Parse(myReader[1].ToString());
                    pr.Disponibilite = myReader[2].ToString();
                    p_copie.Add(pr);
                }

                myReader.Close();
                maDB.close();
                foreach (Produit item in p_copie)
                {
                    foreach (Produit prod in p)
                    {
                        if (item.Reference.Equals(prod.Reference))
                        {
                            prod.QteEnCommande = item.QteEnCommande;
                            prod.Disponibilite = item.Disponibilite;
                        }
                    }
                }
            }
        }

        public void getQteCommandeProduitByRef(Produit P)
        {
            maDB.open();
            string statement =
                "SELECT DISTINCT DL_Qte,DO_DateLivr FROM F_DOCLIGNE WHERE DO_Type=12 AND DO_DateLivr >=(SELECT GETDATE()) AND AR_Ref=@ref";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@ref", SqlDbType.NVarChar, 50).Value = P.Reference;
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            if (!myReader.HasRows)
            {
                P.QteEnCommande = 0f;
                P.Disponibilite = "";
            }
            else
            {
                while (myReader.Read())
                {
                    P.QteEnCommande = float.Parse(myReader["DL_Qte"].ToString());
                    P.Disponibilite = myReader["Do_DateLivr"].ToString();
                }
            }
            myReader.Close();
            maDB.close();
        }

        public void getProduitAssocie(Produit P)
        {
            maDB.open();
            string statement =
                "SELECT * FROM F_ARTICLE INNER JOIN F_NOMENCLAT ON F_ARTICLE.AR_Ref = F_NOMENCLAT.NO_RefDet AND F_ARTICLE.AR_Ref = F_NOMENCLAT.NO_RefDet AND F_NOMENCLAT.AR_Ref = @ref";

            var myCommand = new SqlCommand(statement, dbObject.myConnection);
            myCommand.Parameters.Add("@ref", SqlDbType.NVarChar, 50).Value = P.Reference;
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            //
            while (myReader.Read())
            {
                var monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(),
                    myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));

                P.ProduitAssocies.Add(monProduit);
            }
            myReader.Close();
            maDB.close();
        }


        public List<Produit> ProduitsParCodeStat(string codeStat)
        {
            var maListe = new List<Produit>();
            int ColonneParPage = 15;

            string statement =
                "select DISTINCT F_Article.AR_Ref,AR_Design,AR_PrixVen,AS_QteSto,AS_QteRes,AS_MontSto,F_Article.FA_CodeFamille from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND AR_Stat02=@state";

            var myCommand = new SqlCommand(statement, maDB.myConnection);

            /** ##################### Ajout des parametres #############################**/

            myCommand.Parameters.Add("@state", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@state"].Value = codeStat;

            /** ########################## Fin #####################################*/
            maDB.open();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(),
                    myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));
                float stock = float.Parse(myReader["AS_QteSto"].ToString());
                float stockRes = float.Parse(myReader["AS_QteRes"].ToString());
                monProduit.StockDispo_denis = stock - stockRes;
                monProduit.StockDisponible = monProduit.StockDispo_denis + monProduit.StockDispo_pierre;
                monProduit.StockRes = stockRes;
                decimal montSto = decimal.Parse(myReader["AS_MontSto"].ToString());
                if (stock > 0)
                {
                    monProduit.CMUP = montSto/decimal.Parse(stock.ToString());
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
                foreach (var item in Utilisateur.Remises)
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
                string statement2 =
                    "select Distinct AS_QteSto-AS_QteRes AS pierre from F_ARTSTOCK where DE_No=2 AND AR_Ref=@ref";
                var myCommand2 = new SqlCommand(statement2, maDB.myConnection);
                myCommand2.Parameters.Add("@ref", SqlDbType.NVarChar, 50);
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
            WHERE     (F_ARTCOMPTA.AR_Ref = '" + item.Reference +
                            "') AND (F_ARTCOMPTA.ACP_Champ = 1) AND (F_ARTCOMPTA.ACP_Type = 0)";
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
                statement =
                    "SELECT * FROM F_ARTICLE INNER JOIN F_NOMENCLAT ON F_ARTICLE.AR_Ref = F_NOMENCLAT.NO_RefDet AND F_ARTICLE.AR_Ref = F_NOMENCLAT.NO_RefDet AND F_NOMENCLAT.AR_Ref = '" +
                    item.Reference + "'";
                myCommand = new SqlCommand(statement, dbObject.myConnection);

                myReader = null;
                myReader = myCommand.ExecuteReader();
                //
                while (myReader.Read())
                {
                    var monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(),
                        myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));

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
            var myCommand = new SqlCommand(statement, dbObject.myConnection);

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

            monUser.Factures = recupererEnteteDocumentByType(6, monUser);

            return monUser;
        }

        public List<entetedocument> recupererEnteteDocumentByType(int DO_Type)
        {
            dbObject.open();
            var myCommand =
                new SqlCommand("select * from F_DOCENTETE WHERE (DO_Type = '" + DO_Type + "') ORDER BY DO_Date",
                    dbObject.myConnection);
            var liste = new List<entetedocument>();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var fiche = new entetedocument();
                fiche = remplirEntete(myReader);

                liste.Add(fiche);
            }
            dbObject.myConnection.Close();
            return liste;
        }

        public List<entetedocument> recupererEnteteDocumentByType(int DO_Type, Utilisateur client)
        {
            dbObject.open();
            var myCommand =
                new SqlCommand(
                    "select * from F_DOCENTETE WHERE (DO_Type = '" + DO_Type + "') AND (DO_Tiers='" + client.CodeClient +
                    "') ORDER BY DO_Date DESC", dbObject.myConnection);
            var liste = new List<entetedocument>();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var fiche = new entetedocument();
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
            var ligneDoc = new List<lignedocument>();
            foreach (entetedocument item in liste)
            {
                statement = statement + " OR DO_Piece = '" + item.DO_Piece + "'";
            }
            statement = statement + " ORDER BY DO_Date";
            var myCommand = new SqlCommand(statement, dbObject.myConnection);

            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            //
            while (myReader.Read())
            {
                var fiche = new lignedocument();
                fiche = remplirLigne(myReader);
                ligneDoc.Add(fiche);
            }
            dbObject.myConnection.Close();
            return ligneDoc;
        }

        public List<lignedocument> recupererLigneDocumentByType(int DO_Type)
        {
            var maliste = new List<lignedocument>();
            maDB.open();
            var myCommand =
                new SqlCommand("select * from F_DOCENTETE WHERE (DO_Type = '" + DO_Type + "') ORDER BY DO_Date",
                    dbObject.myConnection);
            var liste = new List<entetedocument>();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var fiche = new entetedocument();
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
                    var fiche = new lignedocument();
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

        public List<lignedocument> RecupererLignedocumentsByTypeAndNum(int DO_Type, string CT_NUM)
        {
            List<lignedocument> maList = new List<lignedocument>();
            maDB.open();
            var myCommand =
                new SqlCommand("select * from F_DOCENTETE WHERE (DO_Type = '" + DO_Type + "' AND DO_Domaine=0 AND DO_Tiers='"+CT_NUM+"') ORDER BY DO_Date DESC",
                    maDB.myConnection);
            var liste = new List<entetedocument>();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    var fiche = new entetedocument();
                    fiche = remplirEntete(myReader);
                    liste.Add(fiche);
                }
            }
            myReader.Close();
                maDB.myConnection.Close();
                if (liste.Count > 0)
                {
                    string statement = "select * from F_DOCLIGNE WHERE DO_Piece IN (";
                    for (int i = 0; i < liste.Count; i++)
                    {
                        if (i + 1 == liste.Count)
                        {
                            statement += "'" + liste[i].DO_Piece + "') ORDER BY DO_DATE DESC";
                        }
                        else
                        {
                            statement += "'" + liste[i].DO_Piece + "',";
                        }
                    }
                    maDB.open();
                    myCommand = new SqlCommand(statement, maDB.myConnection);

                    myReader = myCommand.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            var doc = new lignedocument();
                            doc = remplirLigne(myReader);
                            maList.Add(doc);
                        }
                       myReader.Close();
                    }
                    else
                    {
                        maList = null;
                    }
                    maDB.close();
                }
            
            
           
            return maList;

        }
        public lignedocument remplirLigne(SqlDataReader myReader)
        {
            var fiche = new lignedocument();
            fiche.AF_RefFourniss = myReader["AF_RefFourniss"].ToString();
            fiche.AR_Ref = myReader["AR_Ref"].ToString();
            fiche.cbCreateur = myReader["cbCreateur"].ToString();
            fiche.cbMarq = int.Parse(myReader["cbMarq"].ToString());
            fiche.CT_Num = myReader["CT_Num"].ToString();
            fiche.DE_No = int.Parse(myReader["DE_No"].ToString());
            fiche.DL_CMUP = double.Parse(myReader["DL_CMUP"].ToString());
            fiche.DL_DateBC = DateTime.Parse(myReader["DL_DateBC"].ToString());
            fiche.DL_DateBL = DateTime.Parse(myReader["DL_DateBL"].ToString());
            fiche.DL_Design = myReader["DL_Design"].ToString();
            ;
            fiche.DL_Ligne = int.Parse(myReader["DL_Ligne"].ToString());
            fiche.DL_MontantHT = double.Parse(myReader["DL_MontantHT"].ToString());
            fiche.DL_MontantTTC = double.Parse(myReader["DL_MontantTTC"].ToString());
            fiche.DL_MvtStock = int.Parse(myReader["DL_MvtStock"].ToString());
            fiche.DL_No = int.Parse(myReader["DL_No"].ToString());
            fiche.DL_PieceBC = myReader["DL_PieceBC"].ToString();
            fiche.DL_PrixRU = double.Parse(myReader["DL_PrixRU"].ToString());
            fiche.DL_PrixUnitaire = double.Parse(myReader["DL_PrixUnitaire"].ToString());
            ;
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
            var fiche = new entetedocument();
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
            var dbObject = new Database();
            dbObject.open();
            string statement =
                "SELECT * FROM F_ARTCOMPTA INNER JOIN F_TAXE ON F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe1 WHERE     (F_ARTCOMPTA.AR_Ref = '" +
                reference + "')";
            var myCommand = new SqlCommand(statement, dbObject.myConnection);

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
            String[] blacklist = {"", "FINANCIER", "INUTILE", "TRANSPORT"};
            var maListe = new List<string>();
            string statement =
                "SELECT DISTINCT AR_Stat02 from F_ARTICLE WHERE AR_Stat02 NOT IN (' ','FINANCIER','TRANSPORT','INUTILE','MARKETING') ORDER BY AR_STAT02 ASC";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
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
            var maListe = new List<string>();
            string statement = "SELECT DISTINCT AR_Stat01 from F_ARTICLE WHERE AR_Stat02=@Stat02 ORDER BY AR_Stat01 ASC";
            var myCommand = new SqlCommand(statement, maDB.myConnection);

            myCommand.Parameters.Add("@Stat02", SqlDbType.NVarChar, 50);
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
            var dbObject = new Database();
            dbObject.open();
            string statement = "SELECT TOP 1 DO_Piece, cbMarq FROM F_DOCENTETE WHERE (DO_Type = 1) ORDER BY cbMarq DESC";
            var myCommand = new SqlCommand(statement, dbObject.myConnection);

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

        public static string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes
                = Encoding.ASCII.GetBytes(toEncode);

            string returnValue
                = Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        public static string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes
                = Convert.FromBase64String(encodedData);

            string returnValue =
                Encoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
        }

        public Produit ProductParRef(string AR_Ref)
        {
            Produit monProduit = null;
            ;
            string statement =
                "select DISTINCT F_Article.AR_Ref,AR_Design,AR_PrixVen,AS_QteSto,AS_QteRes,AS_MontSto,F_Article.FA_CodeFamille from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND F_Article.AR_Ref=@ref";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@ref", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@ref"].Value = AR_Ref;

            /** ########################## Fin #####################################*/
            maDB.open();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(),
                    myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));
                float stock = float.Parse(myReader["AS_QteSto"].ToString());
                float stockRes = float.Parse(myReader["AS_QteRes"].ToString());
                monProduit.StockDispo_denis = stock - stockRes;
            }
            myReader.Close();
            maDB.close();
            monProduit.StockDispo_pierre = ArticleParStock(monProduit.Reference, 2);
            getProduitAssocie(monProduit);
            getQteCommandeProduitByRef(monProduit);
            return monProduit;
        }

        public float ArticleParStock(string AR_Ref, int DE_No)
        {
            maDB.open();
            float qte = 0f;
            string statement = "select AS_QteSto-AS_QteRes AS Qte from F_ARTSTOCK where AR_Ref=@ref and DE_No=@depot ";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@ref", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@ref"].Value = AR_Ref;
            myCommand.Parameters.Add("@depot", SqlDbType.Int).Value = DE_No;
            /** ########################## Fin #####################################*/

            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                float f = float.Parse(myReader["Qte"].ToString());
                if (f > 0)
                {
                    qte = f;
                }
            }
            myReader.Close();
            maDB.close();
            return qte;
        }

        public void RecupererListeFamilleRemise(Utilisateur monUser)
        {
            string statement = "SELECT * FROM F_FAMCLIENT WHERE CT_Num = '" + monUser.CodeClient + "'";
            //RECUPERER LES INFOS UTILISATEUR


            var myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                monUser.Remises[myReader["FA_CodeFamille"].ToString()] = float.Parse(myReader["FC_Remise"].ToString());
            }
            maDB.close();
        }

        public List<Produit> getAllProduitByRefAndFamille(string codestat, string famille = "")
        {
            maDB.open();
            var sw = new Stopwatch();
            sw.Start();
            var maListe = new List<Produit>();
            string statement =
                "select DISTINCT F_Article.AR_Ref,AR_Design,AR_PrixVen,AS_QteSto-AS_QteRes AS QTE,AS_MontSto " +
                "from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref " +
                "WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND AR_Stat02=@state AND AR_Stat01 LIKE @famille";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@state", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@state"].Value = codestat;
            myCommand.Parameters.Add("@famille", SqlDbType.NVarChar).Value = "%" + famille + "%";
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(),
                    myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()));
                monProduit.StockDispo_denis = float.Parse(myReader["QTE"].ToString()) < 0
                    ? 0
                    : float.Parse(myReader["QTE"].ToString());

                // get stock st pierre
                maListe.Add(monProduit);
            }
            maDB.close();


            ListArticleStock(maListe);

            GEtqteCommandeProduit(maListe);
            sw.Stop();
            Debug.WriteLine(" temps fonction recup all article est de :{0}", sw.Elapsed);

            return maListe;
        }

        public List<Produit> TousAllProductByCat(string codestat = "", string famille = "", int NumberPage = 1)
        {
            maDB.open();
            var sw = new Stopwatch();
            sw.Start();
            // execution time

            var blah = new SqlCommand("SET ARITHABORT ON", maDB.myConnection);
            blah.ExecuteNonQuery();
            var maListe = new List<Produit>();
            const string statement =
                "select DISTINCT F_Article.AR_Ref,AR_Design,AR_PrixVen,AS_QteSto-AS_QteRes AS QTE,AS_MontSto from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND AR_Stat02=@state AND AR_Stat01 LIKE @famille";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@state", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@state"].Value = codestat;
            //myCommand.Parameters.Add("@PageNumber", System.Data.SqlDbType.Int).Value = NumberPage;
            myCommand.Parameters.Add("@famille", SqlDbType.NVarChar).Value = "%" + famille + "%";
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(),
                    myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()))
                {
                    StockDispo_denis = float.Parse(myReader["QTE"].ToString()) < 0
                        ? 0
                        : float.Parse(myReader["QTE"].ToString())
                };

                // get stock st pierre
                maListe.Add(monProduit);
            }
            maDB.close();
            sw.Stop();
            Debug.WriteLine(" temps fonction recup produit est de :{0}", sw.Elapsed);

            sw.Restart();
            ListArticleStock(maListe);

            sw.Stop();
            Debug.WriteLine(" temps fonction recup stock est de :{0}", sw.Elapsed);
            sw.Restart();
            GEtqteCommandeProduit(maListe);
            sw.Stop();
            Debug.WriteLine(" temps fonction doc_ligne est de :{0}", sw.Elapsed);

            return maListe;
        }

        public Dictionary<string, decimal> GetStatAtToday(Utilisateur monUser)
        {

            TimeSpan time60 = new TimeSpan(60, 0, 0, 0, 0);
            TimeSpan time30 = new TimeSpan(31, 0, 0, 0, 0);
            TimeSpan time90 = new TimeSpan(90, 0, 0, 0, 0);

            DateTime today = DateTime.Now;
            TimeSpan Ttoday = new TimeSpan(today.Day, today.Hour, today.Second);
            DateTime echus1mois = today.Subtract(time30);
            DateTime echus2mois = echus1mois.Subtract(time30);
            DateTime echus3mois = echus2mois.Subtract(time30);


            int echus1Month = today.Month - 1;
            int echus2Month = today.Month - 2;
            int echus3Month = today.Month - 4;

            int echus1Year = today.Year;
            int echus2Year = today.Year;
            int echus3Year = today.Year;

            if (echus1Month <= 0)
            {
                echus1Year = today.Year - 1;
                echus1Month = echus1Month + 12;
            }
            if (echus2Month <= 0)
            {
                echus2Year = today.Year - 1;
                echus2Month = echus2Month + 12;
            }
            if (echus3Month <= 0)
            {
                echus3Year = today.Year - 1;
                echus3Month = echus3Month + 12;
            }


            echus1mois = new DateTime(echus1Year, echus1Month, today.Day);
            echus2mois = new DateTime(echus2Year, echus2Month, today.Day);
            echus3mois = new DateTime(echus3Year, echus3Month, echus3mois.Day);


            Dictionary<string, decimal> monDic = new Dictionary<string, decimal>();
            string statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "')  AND (EC_Lettrage = '') AND (EC_Sens = 0) AND (EC_Echeance < CONVERT(DATETIME, '" + today.Year + "-12-12 00:00:00', 102))  AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {
                    monDic["TotauxDebits"] = decimal.Parse(test);
                }
            }
            maDB.close();
            //AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))
            statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "')  AND (EC_Lettrage = '') AND (EC_Sens = 1) AND (EC_Echeance < CONVERT(DATETIME, '" + today.Year + "-12-12 00:00:00', 102)) AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {
                    monDic["TotauxCredits"] = decimal.Parse(test);
                }
            }
            myReader.Close();
            maDB.close();

            statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "') AND (EC_Echeance > CONVERT(DATETIME, '" + echus1mois.Year + "-" + echus1mois.Month + "-" + echus1mois.Day + " 00:00:00', 102)) AND (EC_Lettrage = '') AND (EC_Sens = 0) AND (EC_Echeance < CONVERT(DATETIME, '" + today.Year + "-" + today.Month + "-" + today.Day + " 00:00:00', 102)) AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {
                    monDic["Debit1mois"] = decimal.Parse(test);
                }
            }
            myReader.Close();
            maDB.close();

            statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "') AND (EC_Echeance > CONVERT(DATETIME, '" + echus2mois.Year + "-" + echus2mois.Month + "-" + echus2mois.Day + " 00:00:00', 102)) AND (EC_Lettrage = '') AND (EC_Sens = 0) AND (EC_Echeance < CONVERT(DATETIME, '" + echus1mois.Year + "-" + echus1mois.Month + "-" + echus1mois.Day + " 00:00:00', 102)) AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {
                    monDic["Debit2mois"] = decimal.Parse(test);
                }
            }
            myReader.Close();
            maDB.close();

            statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "') AND (EC_Echeance > CONVERT(DATETIME, '" + echus3mois.Year + "-" + echus3mois.Month + "-" + echus3mois.Day + " 00:00:00', 102)) AND (EC_Lettrage = '') AND (EC_Sens = 0) AND (EC_Echeance < CONVERT(DATETIME, '" + echus2mois.Year + "-" + echus2mois.Month + "-" + echus2mois.Day + " 00:00:00', 102)) AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {
                    monDic["Debit3mois"] = decimal.Parse(test);
                }
            }
            myReader.Close();
            maDB.close();


            statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "') AND (EC_Echeance > CONVERT(DATETIME, '" + echus1mois.Year + "-" + echus1mois.Month + "-" + echus1mois.Day + " 00:00:00', 102)) AND (EC_Lettrage = '') AND (EC_Sens = 1) AND (EC_Echeance < CONVERT(DATETIME, '" + today.Year + "-" + today.Month + "-" + today.Day + " 00:00:00', 102)) AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {
                    monDic["Credit1mois"] = decimal.Parse(test);
                }
            }
            myReader.Close();
            maDB.close();

            statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "') AND (EC_Echeance > CONVERT(DATETIME, '" + echus2mois.Year + "-" + echus2mois.Month + "-" + echus2mois.Day + " 00:00:00', 102)) AND (EC_Lettrage = '') AND (EC_Sens = 1) AND (EC_Echeance < CONVERT(DATETIME, '" + echus1mois.Year + "-" + echus1mois.Month + "-" + echus1mois.Day + " 00:00:00', 102)) AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {
                    monDic["Credit2mois"] = decimal.Parse(test);
                }
            }
            myReader.Close();
            maDB.close();

            statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "') AND (EC_Echeance > CONVERT(DATETIME, '" + echus3mois.Year + "-" + echus3mois.Month + "-" + echus3mois.Day + " 00:00:00', 102)) AND (EC_Lettrage = '') AND (EC_Sens = 1) AND (EC_Echeance < CONVERT(DATETIME, '" + echus2mois.Year + "-" + echus2mois.Month + "-" + echus2mois.Day + " 00:00:00', 102)) AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {

                    monDic["Credit3mois"] = decimal.Parse(test);
                }
            }
            myReader.Close();
            maDB.close();


            statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "') AND (EC_Echeance > CONVERT(DATETIME, '" + today.Year + "-" + today.Month + "-" + today.Day + " 00:00:00', 102)) AND (EC_Lettrage = '') AND (EC_Sens = 0) AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {

                    monDic["DebitNonEchus"] = decimal.Parse(test);
                }
            }
            myReader.Close();
            maDB.close();

            statement = "SELECT SUM(EC_Montant) AS Expr1 FROM F_ECRITUREC WHERE (CT_Num = '" + monUser.CodeClient + "') AND (EC_Echeance > CONVERT(DATETIME, '" + today.Year + "-" + today.Month + "-" + today.Day + " 00:00:00', 102)) AND (EC_Lettrage = '') AND (EC_Sens = 1) AND (EC_Date > CONVERT(DATETIME, '" + today.Year + "-01-01 00:00:00', 102))";

            myCommand = new SqlCommand(statement, maDB.myConnection);
            maDB.open();
            myReader = null;
            myReader = myCommand.ExecuteReader();

            while (myReader.Read())
            {
                string test = myReader["Expr1"].ToString();
                if (test != "")
                {

                    monDic["CreditNonEchus"] = decimal.Parse(test);
                }
            }
            myReader.Close();
            maDB.close();


            return monDic;
        }

        public List<Produit> SearchProduitsByUser(string search)
        {
            maDB.open();
            var sw = new Stopwatch();
            sw.Start();
            var maListe = new List<Produit>();
            const string statement =
                "select DISTINCT F_Article.AR_Ref,AR_Design,AR_PrixVen,AS_QteSto-AS_QteRes AS QTE,AS_MontSto from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE (F_Article.AR_Ref LIKE @state or F_Article.AR_Design LIKE @state or AR_Stat02 like @state or AR_Stat01 LIKE @state) AND F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@state", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@state"].Value = '%'+search+'%';
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(),
                    myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()))
                {
                    StockDispo_denis = float.Parse(myReader["QTE"].ToString()) < 0
                        ? 0
                        : float.Parse(myReader["QTE"].ToString())
                };

                // get stock st pierre
                maListe.Add(monProduit);
            }
            maDB.close();
            sw.Stop();
            Debug.WriteLine(" temps fonction  search recup produit est de :{0}", sw.Elapsed);

            sw.Restart();
            ListArticleStock(maListe);

            sw.Stop();
            Debug.WriteLine(" temps fonction search recup stock est de :{0}", sw.Elapsed);
            sw.Restart();
            GEtqteCommandeProduit(maListe);
            sw.Stop();
            Debug.WriteLine(" temps fonction search doc_ligne est de :{0}", sw.Elapsed);

            return maListe;
        }
    }
}