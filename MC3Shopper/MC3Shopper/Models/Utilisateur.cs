using System;
using System.Collections.Generic;

namespace MC3Shopper.Models
{
    [Serializable]
    public class Utilisateur
    {
        public string Password;
        private string _Echus1mois = "";
        private string _Echus2mois = "";
        private string _Echus3mois = "";
        private string _EncoursAss = "";
        private string _EncoursMax = "";
        private string _NonEchus = "";
        private string _Solde = "";

        public Utilisateur(string codeClient)
        {
            CodeClient = codeClient;
            Remises = new Dictionary<string, float>();
        }

        public string CodeClient { get; set; }

        public string NomPre { get; set; }

        public string Email { get; set; }

        public bool AncienUser { get; set; }

        public string Intitule { get; set; }

        public string Adresse { get; set; }

        public string ComplementAdresse { get; set; }

        public string CodePostal { get; set; }

        public string Ville { get; set; }

        public string Pays { get; set; }

        public string CodeApe { get; set; }

        public string SIRET { get; set; }

        public string Telephone { get; set; }

        public string Telecopie { get; set; }

        public string EncoursMax
        {
            get
            {
                if (_EncoursMax != "")
                {
                    decimal toRet = decimal.Parse(_EncoursMax);
                    _EncoursMax = toRet.ToString("#,#0.00");
                }
                else
                {
                    _EncoursMax = "0,00";
                }
                return _EncoursMax + "€";
            }
            set { _EncoursMax = value; }
        }

        public string EncoursAss
        {
            get
            {
                if (_EncoursAss != "")
                {
                    decimal toRet = decimal.Parse(_EncoursAss);
                    _EncoursAss = toRet.ToString("#,#0.00");
                }
                else
                {
                    _EncoursAss = "0,00";
                }
                return _EncoursAss + "€";
            }
            set { _EncoursAss = value; }
        }

        public string Solde
        {
            get
            {
                // _Solde = Statisitques["TotauxDebits"].ToString();
                if (Statisitques.ContainsKey("TotauxDebits"))
                {
                    _Solde = Statisitques["TotauxDebits"].ToString("#,#0.00");
                    if (Statisitques.ContainsKey("TotauxCredits"))
                    {
                        decimal toRet = Statisitques["TotauxDebits"] - Statisitques["TotauxCredits"];
                        _Solde = toRet.ToString("#,#0.00");
                    }
                }
                if (_Solde == "")
                {
                    _Solde = "0,00";
                }
                return _Solde + "€";
            }
            set { _Solde = value; }
        }

        public string Echus1mois
        {
            get
            {
                if (Statisitques.ContainsKey("Debit1mois"))
                {
                    _Echus1mois = Statisitques["Debit1mois"].ToString("#,#0.00");
                    //if (this.Statisitques.ContainsKey("Credit1mois"))
                    //{
                    //    decimal toReturn = this.Statisitques["Debit1mois"] + this.Statisitques["Credit1mois"];
                    //    _Echus1mois = toReturn.ToString("0.00");
                    //}
                }
                else if (Statisitques.ContainsKey("Credit1mois"))
                {
                    _Echus1mois = Statisitques["Credit1mois"].ToString("#,#0.00");
                }
                if (_Echus1mois == "")
                {
                    _Echus1mois = "0,00";
                }
                return _Echus1mois + "€";
            }
            set { _Echus1mois = value; }
        }

        public string Echus2mois
        {
            get
            {
                if (Statisitques.ContainsKey("Debit2mois"))
                {
                    _Echus2mois = Statisitques["Debit2mois"].ToString("#,#0.00");
                    //if (this.Statisitques.ContainsKey("Credit2mois"))
                    //{
                    //    decimal toReturn = this.Statisitques["Debit2mois"] + this.Statisitques["Credit2mois"];
                    //    _Echus2mois = toReturn.ToString("0.00");
                    //}
                }
                else if (Statisitques.ContainsKey("Credit2mois"))
                {
                    _Echus2mois = Statisitques["Credit2mois"].ToString("#,#0.00");
                }
                if (_Echus2mois == "")
                {
                    _Echus2mois = "0,00";
                }
                return _Echus2mois + "€";
            }
            set { _Echus2mois = value; }
        }

        public string Echus3mois
        {
            get
            {
                if (Statisitques.ContainsKey("Debit3mois"))
                {
                    decimal toReturn = Statisitques["Debit3mois"];
                    _Echus3mois = Statisitques["Debit3mois"].ToString("#,#0.00");
                    if (Statisitques.ContainsKey("Credit3mois"))
                    {
                        //decimal toReturn = this.Statisitques["Debit3mois"] + this.Statisitques["Credit3mois"];
                        toReturn -= Statisitques["Credit3mois"];
                        _Echus3mois = toReturn.ToString("#,#0.00");
                    }
                    if (Statisitques.ContainsKey("Credit2mois"))
                    {
                        //decimal toReturn = this.Statisitques["Debit3mois"] + this.Statisitques["Credit3mois"];
                        toReturn -= Statisitques["Credit2mois"];
                        _Echus3mois = toReturn.ToString("#,#0.00");
                    }
                    if (Statisitques.ContainsKey("Credit1mois"))
                    {
                        //decimal toReturn = this.Statisitques["Debit3mois"] + this.Statisitques["Credit3mois"];
                        toReturn -= Statisitques["Credit1mois"];
                        _Echus3mois = toReturn.ToString("#,#0.00");
                    }
                }
                else if (Statisitques.ContainsKey("Credit3mois"))
                {
                    _Echus3mois = Statisitques["Credit3mois"].ToString("#,#0.00");
                }

                if (_Echus3mois == "")
                {
                    _Echus3mois = "0,00";
                }
                return _Echus3mois + "€";
            }
            set { _Echus3mois = value; }
        }

        public string NonEchus
        {
            get
            {
                if (Statisitques.ContainsKey("DebitNonEchus"))
                {
                    decimal toReturn = Statisitques["DebitNonEchus"];
                    _NonEchus = Statisitques["DebitNonEchus"].ToString("#,#0.00");
                    if (Statisitques.ContainsKey("CreditNonEchus"))
                    {
                        //decimal toReturn = this.Statisitques["Debit3mois"] + this.Statisitques["Credit3mois"];
                        toReturn -= Statisitques["CreditEchus"];
                        _NonEchus = toReturn.ToString("#,#0.00");
                    }
                }
                else if (Statisitques.ContainsKey("CreditNonEchus"))
                {
                    _NonEchus = Statisitques["CreditNonEchus"].ToString("#,#0.00");
                }
                if (_NonEchus == "")
                {
                    _NonEchus = "0,00";
                }
                return _NonEchus + "€";
            }
            set { _NonEchus = value; }
        }

        public List<Utilisateur> Contact { get; set; }

        public List<entetedocument> Factures { get; set; } //DO_TYpe=6

        public List<Panier> FacturesAvoirs { get; set; }

        public Dictionary<string, float> Remises { get; set; }

        public Dictionary<string, decimal> Statisitques { get; set; }
    }
}