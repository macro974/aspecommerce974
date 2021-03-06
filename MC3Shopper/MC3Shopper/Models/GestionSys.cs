﻿using System;
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
                        "select top 10 F_Article.AR_Ref,AR_Design,AR_PrixVen,AR_Stat01,AS_QteSto-AS_QteRes AS QTE from F_ARTICLE INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE F_ARTSTOCK.DE_No = 1  and ar_publie=1 and ar_sommeil=0 and AR_PrixVen>0 order by AR_DateCreation DESC ";
                    break;

                case 2: // Promotions
                    statement =
                        "select top 10 F_Article.AR_Ref,AR_Design,AR_PrixVen,AR_Stat01,AS_QteSto-AS_QteRes AS QTE from F_ARTICLE INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE F_ARTSTOCK.DE_No = 1 and Lower(AR_Design) LIKE '%promo%' and ar_publie=1 and ar_sommeil=0 and AR_PrixVen>0 order by AR_DateModif DESC";
                    break;

                case 3: // destockage
                    statement =
                        "select top 10 F_Article.AR_Ref,AR_Design,AR_PrixVen,AR_Stat01,AS_QteSto-AS_QteRes AS QTE from F_ARTICLE INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE F_ARTSTOCK.DE_No = 1 and Lower(AR_Design) LIKE '%destockage%' AND Lower(AR_Design) NOT LIKE '%promo%'  and ar_publie=1 and ar_sommeil=0 and AR_PrixVen>0 order by AR_DateModif DESC";
                    break;

                default:
                    break;
            }
            dbObject.open();
            var myCommand = new SqlCommand(statement, dbObject.myConnection);

            SqlDataReader myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var monProduit = new Produit(myReader["AR_Ref"].ToString(), myReader["AR_Ref"].ToString(),
                    myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()))
                 {
                     StockDispo_denis = float.Parse(myReader["QTE"].ToString()) < 0
                         ? 0
                         : float.Parse(myReader["QTE"].ToString())
                 };
                maListe.Add(monProduit);
            }
            maDB.close();
            ListArticleStock(maListe);
            myReader.Close();
            dbObject.close();
            return maListe;
        }

        public void ListArticleStock(List<Produit> P)
        {
            var p_copie = new List<Produit>();
            string statement2 =
                "select Distinct AR_Ref,AS_QteRes,AS_QteSto-AS_QteRes AS pierre from F_ARTSTOCK where DE_No=2 AND AR_Ref IN(";
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
                    p.Reference = myReader2["AR_Ref"].ToString();
                    p.QteRes_pierre = float.Parse(myReader2["AS_QteRes"].ToString());
                    p.StockDispo_pierre = float.Parse(myReader2["pierre"].ToString()) < 0
                        ? 0
                        : float.Parse(myReader2["pierre"].ToString());
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

        public void RemiseToProduit(Produit p, Utilisateur user)
        {
            foreach (var item in user.Remises)
            {
                if (item.Key.Equals(p.CodeFamille))
                {
                    p.Remise = item.Value;
                }
            }
            if (p.Remise > 0)
            {
                p.PrixFormate = p.Prix.ToString("0.00") + "€ (PP:" + p.PrixOriginal.ToString("0.00") + "€)";
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
                new SqlCommand("select * from F_DOCENTETE WHERE (DO_Type = '" + DO_Type + "' AND DO_Domaine=0 AND DO_Tiers='" + CT_NUM + "') ORDER BY DO_Date DESC",
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
            // fiche.DL_No = int.Parse(myReader["DL_No"].ToString());
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
            String[] blacklist = { "", "FINANCIER", "INUTILE", "TRANSPORT" };
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
            string statement = "SELECT TOP 1 DO_Piece FROM F_DOCENTETE WHERE (DO_Type = 1) ORDER BY cbMarq DESC";
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
                "select DISTINCT F_Article.AR_Ref,F_Article.AR_UnitePoids,F_Article.AR_PoidsNet,F_Article.AR_PoidsBrut,AR_Design,AR_PrixVen,AS_QteSto,AS_QteRes,AS_MontSto,F_Article.FA_CodeFamille from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND F_Article.AR_Ref=@ref";
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
                monProduit.StockDispo_denis = stock - stockRes <= 0 ? 0 : stock - stockRes;
                monProduit.AR_PoidsNet = double.Parse(myReader["AR_PoidsNet"].ToString());
                monProduit.AR_PoidsBrut = double.Parse(myReader["AR_PoidsBrut"].ToString());
                monProduit.AR_UnitePoids = int.Parse(myReader["AR_UnitePoids"].ToString());
            }
            myReader.Close();
            maDB.close();
            monProduit.StockDispo_pierre = ArticleParStock(monProduit.Reference, 2);
            monProduit.StockDisponible = monProduit.StockDispo_denis + monProduit.StockDispo_pierre;
            getProduitAssocie(monProduit);
            getQteCommandeProduitByRef(monProduit);
            maDB.open();
            //statement = "SELECT * FROM F_ARTCOMPTA INNER JOIN F_TAXE ON F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe1 WHERE     (F_ARTCOMPTA.AR_Ref = '" + item.Reference + "')";
            //statement = "SELECT * FROM F_ARTCOMPTA INNER JOIN F_TAXE ON F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe1 WHERE     (F_ARTCOMPTA.AR_Ref = '" + item.Reference + "')";
            string statement1 = @"SELECT     *
                    FROM         F_ARTCOMPTA INNER JOIN
                      F_TAXE ON F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe1 OR F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe2 OR
                      F_TAXE.TA_Code = F_ARTCOMPTA.ACP_ComptaCPT_Taxe3
            WHERE     (F_ARTCOMPTA.AR_Ref = '" + monProduit.Reference + "') AND (F_ARTCOMPTA.ACP_Champ = 1) AND (F_ARTCOMPTA.ACP_Type = 0)";
            myCommand = new SqlCommand(statement1, dbObject.myConnection);

            myReader = null;
            myReader = myCommand.ExecuteReader();
            //
            while (myReader.Read())
            {
                //item.Taxes.Add(myReader["TA_Intitule"].ToString(), float.Parse(myReader["TA_Taux"].ToString()));
                //item.TVA = float.Parse(myReader["TA_Taux"].ToString());
                //item.Taxe2 = float.Parse(myReader["ACP_ComptaCPT_Taxe2"].ToString());
                //item.Taxe3 = float.Parse(myReader["ACP_ComptaCPT_Taxe3"].ToString());
                monProduit.Taxes[myReader["TA_Intitule"].ToString()] = float.Parse(myReader["TA_Taux"].ToString());
                //item.Taxes[myReader["TA_Intitule"].ToString()] = float.Parse(myReader["TA_Taux"].ToString());
            }
            maDB.close();

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
                "WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND AR_Stat02=@state AND (AR_Stat01 LIKE @famille OR AR_Stat01 LIKE '%')";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@state", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@state"].Value = codestat;
            myCommand.Parameters.Add("@famille", SqlDbType.NVarChar, 50).Value = "%" + famille + "%";
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
                "select DISTINCT F_Article.AR_Ref,AR_Photo,AR_Design,AR_PrixVen,AS_QteRes,F_Article.FA_CodeFamille,AS_QteSto-AS_QteRes AS QTE,AS_MontSto from F_Article INNER JOIN F_ARTSTOCK ON F_ARTICLE.AR_Ref = F_ARTSTOCK.AR_Ref WHERE F_ARTSTOCK.DE_No = 1 AND AR_Sommeil = 0 AND AR_Publie = 1 AND AR_Stat02=@state AND AR_Stat01 LIKE @famille";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@state", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@state"].Value = codestat;
            //myCommand.Parameters.Add("@PageNumber", System.Data.SqlDbType.Int).Value = NumberPage;
            myCommand.Parameters.Add("@famille", SqlDbType.NVarChar).Value = "%" + famille + "%";
            SqlDataReader myReader = null;
            myReader = myCommand.ExecuteReader();
            while (myReader.Read())
            {
                var monProduit = new Produit(myReader["AR_Photo"].ToString(), myReader["AR_Ref"].ToString(),
                    myReader["AR_Design"].ToString(), decimal.Parse(myReader["AR_PrixVen"].ToString()))
                {
                    CodeFamille = myReader["FA_CodeFamille"].ToString(),
                    StockDispo_denis = float.Parse(myReader["QTE"].ToString()) < 0
                        ? 0
                        : float.Parse(myReader["QTE"].ToString()),
                    QteRes_denis = float.Parse(myReader["AS_QteRes"].ToString()),
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
            myCommand.Parameters["@state"].Value = '%' + search + '%';
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

        public Produit FindAndCheckProduitByRef(string AR_Ref, int qte)
        {
            Produit p = ProductParRef(AR_Ref);
            if (p != null)
            {
                if (qte <= (p.StockDisponible + p.QteEnCommande))
                {
                    p.QteDemande = qte;
                    return p;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public float getQTEResFromArticle(int DE_no, string Ar_ref)
        {
            maDB.open();
            float qte = 0f;
            string statement = "select AS_QteRes AS Qte from F_ARTSTOCK where AR_Ref=@ref and DE_No=@depot ";
            var myCommand = new SqlCommand(statement, maDB.myConnection);
            myCommand.Parameters.Add("@ref", SqlDbType.NVarChar, 50);
            myCommand.Parameters["@ref"].Value = Ar_ref;
            myCommand.Parameters.Add("@depot", SqlDbType.Int).Value = DE_no;
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

        public string CreeCommande(Panier monPanier, Utilisateur monUser, int depot)
        {
            //ALTER DATABASE "nomDeLaBase" SET ARITHABORT ON
            List<Produit> ListTransfert = new List<Produit>(); // Liste qui va servir pour le mouvement de transfert
            String getdate = null;
            string numeroBC = GetNumeroBC();
            Database dbObject = new Database();
            int cbmarq = 0;

            dbObject.open();
            /**
            string statement1 = @"ALTER DATABASE MC3REUNION SET ARITHABORT ON";**/
            string statement1 = "select MAX(cbMarq)+1 from F_DOCENTETE";
            SqlCommand myCommand1 = new SqlCommand(statement1, dbObject.myConnection);
            /**
            myCommand1.ExecuteScalar();**/
            SqlDataReader reader = myCommand1.ExecuteReader();
            while (reader.Read())
            {
                cbmarq = int.Parse(reader[0].ToString());
            }
            dbObject.close();
            // get date
            dbObject.open();
            /**
            string statement1 = @"ALTER DATABASE MC3REUNION SET ARITHABORT ON";**/
            statement1 = "select GETDATE()";
            myCommand1 = new SqlCommand(statement1, dbObject.myConnection);
            /**
            myCommand1.ExecuteScalar();**/
            reader = myCommand1.ExecuteReader();
            while (reader.Read())
            {
                getdate = reader[0].ToString();
            }
            dbObject.close();
            dbObject.open();
            string minute = "";
            string heure = "";
            string seconde = "";
            if (DateTime.Now.Minute < 10)
            {
                minute = 0 + DateTime.Now.Minute.ToString();
            }
            else
            {
                minute = DateTime.Now.Minute.ToString();
            }
            if (DateTime.Now.Hour < 10)
            {
                heure = 0 + DateTime.Now.Hour.ToString();
            }
            else
            {
                heure = DateTime.Now.Hour.ToString();
            }
            if (DateTime.Now.Second < 10)
            {
                seconde = 0 + DateTime.Now.Second.ToString();
            }
            else
            {
                seconde = DateTime.Now.Second.ToString();
            }

            string Intt = "Web : Commande";

            foreach (Produit item in monPanier.monPanier)
            {
                if (item.Reference.Equals("FRAIS_DE_TRANSPORT"))
                {
                    Intt = "Web : A livrer";
                }
            }

            string statement = @"INSERT INTO F_DOCENTETE
                      (DO_Piece,AB_No, CA_No, CA_Num, CG_Num, CT_NumPayeur, DE_No, DO_Attente, DO_BLFact, DO_Cloture, DO_Colisage, DO_Condition, DO_Coord01,
                      DO_Coord02, DO_Coord03, DO_Coord04, DO_Cours, DO_Date, DO_DateLivr, DO_DebutAbo, DO_DebutPeriod, DO_Devise, DO_Domaine, DO_Ecart,
                      DO_Expedit, DO_FinAbo, DO_FinPeriod, DO_Heure, DO_Imprim, DO_Langue, DO_NbFacture, DO_NoWeb, DO_Period, DO_Ref, DO_Regime,
                      DO_Reliquat, DO_Souche, DO_Statut, DO_Tarif, DO_Tiers, DO_Transaction, DO_Transfere, DO_TxEscompte, DO_Type, DO_TypeColis, DO_Ventile,
                      LI_No, N_CatCompta,cbMarq)
    VALUES     ('" + numeroBC + @"',0, 0, '', '4111000000', '" + monUser.CodeClient + @"', " + depot + ", 0, 0, 0, 1, 1, '', '', '', '', 0, '" + getdate + @"', '1900-01-01 00:00:00', '1900-01-01 00:00:00', '1900-01-01 00:00:00', 0, 0, 0, 1, '1900-01-01 00:00:00', '1900-01-01 00:00:00',
                       '" + heure + minute + seconde + @"', 0, 0, 1, '', 1, '" + Intt + "', 21, 0, 0, 0, 1, '" + monUser.CodeClient + @"', 11, 0, 0, 1, 1, 0, 84, 1," + cbmarq + ")";
            SqlCommand myCommand = new SqlCommand(statement, dbObject.myConnection);

            myCommand.ExecuteScalar();
            dbObject.close();

            foreach (Produit item in monPanier.monPanier)
            {
                dbObject.open();
                string kaboom = item.Designation.Replace("'", "''");
                string statemnt = @"INSERT INTO F_DOCLIGNE
                      (AF_RefFourniss, AG_No1, AG_No2, AR_Ref, AR_RefCompose, CA_Num, CT_Num, DE_No, DL_CMUP, DL_DateBC, DL_DateBL, DL_Design, DL_Frais,
                      DL_Ligne, DL_MvtStock, DL_NonLivre, DL_NoRef, DL_PieceBC, DL_PieceBL, DL_PoidsBrut, DL_PoidsNet, DL_PrixRU, DL_PrixUnitaire,
                      DL_PUBC, DL_PUDevise, DL_PUTTC, DL_Qte, DL_QteBC, DL_QteBL, DL_Remise01REM_Type, DL_Remise01REM_Valeur, DL_Remise02REM_Type,
                      DL_Remise02REM_Valeur, DL_Remise03REM_Type, DL_Remise03REM_Valeur, DL_Taxe1, DL_Taxe2, DL_Taxe3, DL_TNomencl, DL_TRemExep,
                      DL_TRemPied, DL_TTC, DL_TypePL, DL_TypeTaux1, DL_TypeTaux2, DL_TypeTaux3, DL_TypeTaxe1, DL_TypeTaxe2, DL_TypeTaxe3, DL_Valorise,
                      DO_Date, DO_DateLivr, DO_Domaine, DO_Piece, DO_Ref, DO_Type, EU_Enumere, EU_Qte, DL_MontantHT, DL_MontantTTC,
                      DL_FactPoids, DL_Escompte,cbProt,DL_DateAvancement,cbMarq)
VALUES     ('" + item.Reference + @"', 0, 0, '" + item.Reference + @"', '', '', '" + monUser.CodeClient + @"', " + depot + ", '" + item.CMUP.ToString("0.00").Replace(',', '.') + @"', '" + getdate + @"','1900-01-01 00:00:00', '" + item.Designation.Replace("'", "''") + @"', 0, 10000, 0, 0, 0, NULL, NULL, 1, 0, 0,'" + item.PrixOriginal.ToString("0.00").Replace(',', '.') + @"', 0, 0, " + item.PUTTC.ToString("0.00").Replace(',', '.') + @", '" + item.QteDemande.ToString("0.00").Replace(',', '.') + @"',
                      '" + item.QteDemande.ToString("0.00").Replace(',', '.') + "', '" + item.QteDemande.ToString("0.00").Replace(',', '.') + "',1, " + item.Remise.ToString("0.00").Replace(',', '.') + ", 0, 0, 0, 0, '" + item.TVA.ToString("0.00").Replace(',', '.') + @"', '" + item.Taxe2.ToString("0.00").Replace(',', '.') + @"', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, '" + getdate + @"', '1900-01-01 00:00:00', 0, '" + numeroBC + "', NULL, 1, 0, '" + item.QteDemande.ToString("0.00").Replace(',', '.') + "','" + item.PrixTotal.ToString("0.00").Replace(',', '.') + @"', '" + item.PrixTTC.ToString("0.00").Replace(',', '.') + "', 0, 0,0,'1900-01-01 00:00:00'," + cbmarq + ") ";
                myCommand = new SqlCommand(statemnt, dbObject.myConnection);

                myCommand.ExecuteScalar();
                dbObject.close();

                float stockRes = 0;
                Trace.WriteLine("test", "stockres :" + stockRes + "");
                if (depot == 1)
                {
                    stockRes = getQTEResFromArticle(depot, item.Reference) + item.QteDemande;
                    if ((item.StockDispo_denis - item.QteDemande) < 0)
                    {
                        ListTransfert.Add(item);
                    }
                }
                else
                {
                    stockRes = item.QteRes_pierre + item.QteDemande;
                    if ((item.StockDispo_pierre - item.QteDemande) < 0)
                    {
                        ListTransfert.Add(item);
                    }
                }
                dbObject.open();
                statemnt = @"UPDATE F_ARTSTOCK SET AS_QteRes='" + stockRes.ToString().Replace(',', '.') + "' WHERE AR_Ref='" + item.Reference + "' AND DE_No=" + depot + "";
                myCommand = new SqlCommand(statemnt, dbObject.myConnection);

                myCommand.ExecuteScalar();
                dbObject.close();
            }

            return numeroBC;
        }

        public void MouvementTransfert(List<Produit> lp, Utilisateur user, int Depot)
        {
            // selection de la docentete
            entetedocument doc_entete = new entetedocument();
            maDB.open();
            string statement = "select top 1 * from F_DOCENTETE where DO_Domaine=2 and DO_Type=23   and DO_Ref LIKE '[^**]%[NS]>_' and DO_Tiers=" + Depot + " order by DO_Date asc";
            SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);
            SqlDataReader reader = myCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    doc_entete = remplirEntete(reader);
                }
                reader.Close();
                maDB.close();
                // creation nouveau f_docligne
                foreach (var item in lp)
                {
                    // instructions de pour le choix de du poids suivant l'objet
                    int MvtStock = 0;
                    for (int i = 0; i == 1; i++)
                    {
                        MvtStock = i > 0 ? 1 : 3;

                        statement = @"INSERT INTO F_DOCLIGNE
                      (DO_Domaine,DO_Type,CT_Num ,DO_Piece,DL_PieceBC,DL_PieceBL,DO_Date,DL_DateBC,DL_DateBL,DL_Ligne,DO_Ref,DL_TNomencl,DL_TRemPied,DL_TRemExep,AR_Ref,DL_Design,DL_Qte,DL_QteBC,DL_QteBL,DL_PoidsNet,DL_PoidsBrut,
                        DL_Remise01REM_Valeur,DL_Remise01REM_Type,DL_Remise02REM_Valeur,DL_Remise02REM_Type,DL_Remise03REM_Valeur,
                        DL_Remise03REM_Type,DL_PrixUnitaire,DL_PUBC,DL_Taxe1,DL_TypeTaux1,DL_TypeTaxe1,DL_Taxe2,DL_TypeTaux2,DL_TypeTaxe2,AG_No1,AG_No2,DL_PrixRU,DL_CMUP,
                        DL_MvtStock,AF_RefFourniss,EU_Enumere,EU_Qte,DL_TTC,DE_No,DL_NoRef,DL_TypePL,DL_PUDevise,DL_PUTTC,
                        DL_No,DO_DateLivr,CA_Num,DL_Taxe3,DL_TypeTaux3,DL_TypeTaxe3,
                        DL_Frais,DL_Valorise,AR_RefCompose,DL_NonLivre,AC_RefClient,DL_MontantHT,DL_MontantTTC,DL_FactPoids,DL_Escompte,
                        cbProt,cbMarq,cbCreateur,cbModification,cbReplication,cbFlag,CO_No,DT_No,DL_PiecePL,DL_DatePL,DL_QtePL,DL_NoColis,DL_NoLink,RP_Code,DL_QteRessource,DL_DateAvancement)
                        VALUES(@DO_Domaine,@DO_Type,@CT_Num ,@DO_Piece,@DL_PieceBC,@DL_PieceBL,@DO_Date,@DL_DateBC,@DL_DateBL,@DL_Ligne,@DO_Ref,@DL_TNomencl,@DL_TRemPied,@DL_TRemExep,@AR_Ref,@DL_Design,@DL_Qte,@DL_QteBC,@DL_QteBL,@DL_PoidsNet,@DL_PoidsBrut,
@DL_Remise01REM_Valeur,@DL_Remise01REM_Type,DL_Remise02REM_Valeur,@DL_Remise02REM_Type,DL_Remise03REM_Valeur,
@DL_Remise03REM_Type,@DL_PrixUnitaire,@DL_PUBC,@DL_Taxe1,@DL_TypeTaux1,@DL_TypeTaxe1,@DL_Taxe2,@DL_TypeTaux2,@DL_TypeTaxe2,@AG_No1,@AG_No2,@DL_PrixRU,@DL_CMUP,
@DL_MvtStock,@AF_RefFourniss,@EU_Enumere,@EU_Qte,@DL_TTC,@DE_No,@DL_NoRef,@DL_TypePL,@DL_PUDevise,@DL_PUTTC,
@DL_No,@DO_DateLivr,@CA_Num,@DL_Taxe3,@DL_TypeTaux3,@DL_TypeTaxe3,
@DL_Frais,@DL_Valorise,@AR_RefCompose,@DL_NonLivre,@AC_RefClient,@DL_MontantHT,@DL_MontantTTC,@DL_FactPoids,@DL_Escompte,
@cbProt,@cbMarq,@cbCreateur,@cbModification,@cbReplication,@cbFlag,@CO_No,@DT_No,@DL_PiecePL,@DL_DatePL,@DL_QtePL,@DL_NoColis,@DL_NoLink,@RP_Code,@DL_QteRessource,@DL_DateAvancement)";
                        myCommand = new SqlCommand(statement, maDB.myConnection);
                        // Méthode sql prépare hyper longue 
                        myCommand.Parameters.Add("@DO_Domaine", SqlDbType.Int).Value = 2;
                        myCommand.Parameters.Add("@Do_Type", SqlDbType.Int).Value = 23;
                        myCommand.Parameters.Add("@CT_Num", SqlDbType.VarChar, 50).Value = doc_entete.DO_Tiers;
                        myCommand.Parameters.Add("@DO_Piece", SqlDbType.NVarChar, 50).Value = doc_entete.DO_Piece;
                        myCommand.Parameters.Add("@DL_PieceBC", SqlDbType.NVarChar, 50).Value = "";
                        myCommand.Parameters.Add("@DL_PieceBL", SqlDbType.NVarChar, 50).Value = "";
                        myCommand.Parameters.Add("@DO_Date", SqlDbType.SmallDateTime).Value = doc_entete.DO_Date;
                        myCommand.Parameters.Add("@DL_DateBC", SqlDbType.SmallDateTime).Value = "1900-01-01 00:00:00";
                        myCommand.Parameters.Add("@DO_DateBL", SqlDbType.SmallDateTime).Value = doc_entete.DO_Date;
                        myCommand.Parameters.Add("@DL_Ligne", SqlDbType.Int).Value = getMaxDLligne(doc_entete.DO_Piece);
                        myCommand.Parameters.Add("@DO_Ref", SqlDbType.NVarChar, 50).Value = doc_entete.DO_Ref;
                        myCommand.Parameters.Add("@DL_TNomencl", SqlDbType.Int).Value = 0;
                        myCommand.Parameters.Add("@DL_TRemPied", SqlDbType.Int).Value = 0;
                        myCommand.Parameters.Add("@DL_TRemExep", SqlDbType.Int).Value = 0;
                        myCommand.Parameters.Add("@AR_Ref", SqlDbType.NVarChar, 50).Value = item.Reference;
                        myCommand.Parameters.Add("@DL_Design", SqlDbType.NVarChar, 50).Value = item.Designation;
                        myCommand.Parameters.Add("@DL_Qte", SqlDbType.Float).Value = item.QteDemande;
                        myCommand.Parameters.Add("@DL_QteBC", SqlDbType.Float).Value = item.QteDemande;
                        myCommand.Parameters.Add("@DL_QteBL", SqlDbType.Float).Value = 0.000000;
                        myCommand.Parameters.Add("@DL_QteBL", SqlDbType.Float).Value = 0.000000;
                        myCommand.Parameters.Add("@DL_PoidsNet", SqlDbType.Float).Value = item.DL_PoidsNet;
                        myCommand.Parameters.Add("@DL_PoidsBrut", SqlDbType.Float).Value = item.DL_PoidsBrut;
                        // 2eme ligne 
                        myCommand.Parameters.Add("@DL_Remise01REM_Valeur", SqlDbType.Float).Value = 0.000000;
                        myCommand.Parameters.Add("@DL_Remise01REM_Type", SqlDbType.Int).Value = 0;
                        myCommand.Parameters.Add("@DL_Remise02REM_Valeur", SqlDbType.Float).Value = 0.000000;
                        myCommand.Parameters.Add("@DL_Remise02REM_Type", SqlDbType.Int).Value = 0;
                        myCommand.Parameters.Add("@DL_Remise03REM_Valeur", SqlDbType.Float).Value = 0.000000;
                        myCommand.Parameters.Add("@DL_Remise03REM_Type", SqlDbType.Int).Value = 0;
                        // 3eme ligne 
                        myCommand.Parameters.Add("@DL_PrixUnitaire", SqlDbType.Float).Value = item.Prix;
                        myCommand.Parameters.Add("@DL_PUBC", SqlDbType.Float).Value = 0f;

                    }
                }
            }
        }

        public int getMaxDLligne(string DO_Piece)
        {
            maDB.open();
            int Dl_Ligne = 10000;
            string statement = "select MAX(DL_Ligne) from F_DOCLIGNE where DO_Piece='" + DO_Piece + "' ";
            SqlCommand myCommand = new SqlCommand(statement, maDB.myConnection);
            SqlDataReader reader = myCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Dl_Ligne += int.Parse(reader[0].ToString());
                }
                reader.Close();
                maDB.close();
            }
            return Dl_Ligne;

        }
    }
}