using System.Collections.Generic;
using System;
namespace MC3Shopper.Models
{

   [Serializable()]	
    public class Utilisateur 
    {
        public string CodeClient { get; set; }

        public string NomPre { get; set; }

        public string Email { get; set; }

        public string Password;

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

        private string _EncoursMax = "";

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
            set
            {
                _EncoursMax = value;
            }
        }

        private string _EncoursAss = "";

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
            set
            {
                _EncoursAss = value;
            }
        }

        private string _Solde = "";

        public string Solde
        {
            get
            {
                // _Solde = Statisitques["TotauxDebits"].ToString();
                if (this.Statisitques.ContainsKey("TotauxDebits"))
                {
                    _Solde = this.Statisitques["TotauxDebits"].ToString("#,#0.00");
                    if (this.Statisitques.ContainsKey("TotauxCredits"))
                    {
                        decimal toRet = this.Statisitques["TotauxDebits"] - this.Statisitques["TotauxCredits"];
                        _Solde = toRet.ToString("#,#0.00");
                    }
                }
                if (_Solde == "")
                {
                    _Solde = "0,00";
                }
                return _Solde + "€";
            }
            set
            {
                _Solde = value;
            }
        }

        private string _Echus1mois = "";

        public string Echus1mois
        {
            get
            {
                if (this.Statisitques.ContainsKey("Debit1mois"))
                {
                    _Echus1mois = this.Statisitques["Debit1mois"].ToString("#,#0.00");
                    //if (this.Statisitques.ContainsKey("Credit1mois"))
                    //{
                    //    decimal toReturn = this.Statisitques["Debit1mois"] + this.Statisitques["Credit1mois"];
                    //    _Echus1mois = toReturn.ToString("0.00");
                    //}
                }
                else if (this.Statisitques.ContainsKey("Credit1mois"))
                {
                    _Echus1mois = this.Statisitques["Credit1mois"].ToString("#,#0.00");
                }
                if (_Echus1mois == "")
                {
                    _Echus1mois = "0,00";
                }
                return _Echus1mois + "€";
            }
            set
            {
                _Echus1mois = value;
            }
        }

        private string _Echus2mois = "";

        public string Echus2mois
        {
            get
            {
                if (this.Statisitques.ContainsKey("Debit2mois"))
                {
                    _Echus2mois = this.Statisitques["Debit2mois"].ToString("#,#0.00");
                    //if (this.Statisitques.ContainsKey("Credit2mois"))
                    //{
                    //    decimal toReturn = this.Statisitques["Debit2mois"] + this.Statisitques["Credit2mois"];
                    //    _Echus2mois = toReturn.ToString("0.00");
                    //}
                }
                else if (this.Statisitques.ContainsKey("Credit2mois"))
                {
                    _Echus2mois = this.Statisitques["Credit2mois"].ToString("#,#0.00");
                }
                if (_Echus2mois == "")
                {
                    _Echus2mois = "0,00";
                }
                return _Echus2mois + "€";
            }
            set
            {
                _Echus2mois = value;
            }
        }

        private string _Echus3mois = "";

        public string Echus3mois
        {
            get
            {
                if (this.Statisitques.ContainsKey("Debit3mois"))
                {
                    decimal toReturn = this.Statisitques["Debit3mois"];
                    _Echus3mois = this.Statisitques["Debit3mois"].ToString("#,#0.00");
                    if (this.Statisitques.ContainsKey("Credit3mois"))
                    {
                        //decimal toReturn = this.Statisitques["Debit3mois"] + this.Statisitques["Credit3mois"];
                        toReturn -= this.Statisitques["Credit3mois"];
                        _Echus3mois = toReturn.ToString("#,#0.00");
                    }
                    if (this.Statisitques.ContainsKey("Credit2mois"))
                    {
                        //decimal toReturn = this.Statisitques["Debit3mois"] + this.Statisitques["Credit3mois"];
                        toReturn -= this.Statisitques["Credit2mois"];
                        _Echus3mois = toReturn.ToString("#,#0.00");
                    }
                    if (this.Statisitques.ContainsKey("Credit1mois"))
                    {
                        //decimal toReturn = this.Statisitques["Debit3mois"] + this.Statisitques["Credit3mois"];
                        toReturn -= this.Statisitques["Credit1mois"];
                        _Echus3mois = toReturn.ToString("#,#0.00");
                    }
                }
                else if (this.Statisitques.ContainsKey("Credit3mois"))
                {
                    _Echus3mois = this.Statisitques["Credit3mois"].ToString("#,#0.00");
                }

                if (_Echus3mois == "")
                {
                    _Echus3mois = "0,00";
                }
                return _Echus3mois + "€";
            }
            set
            {
                _Echus3mois = value;
            }
        }

        private string _NonEchus = "";

        public string NonEchus
        {
            get
            {
                if (this.Statisitques.ContainsKey("DebitNonEchus"))
                {
                    decimal toReturn = this.Statisitques["DebitNonEchus"];
                    _NonEchus = this.Statisitques["DebitNonEchus"].ToString("#,#0.00");
                    if (this.Statisitques.ContainsKey("CreditNonEchus"))
                    {
                        //decimal toReturn = this.Statisitques["Debit3mois"] + this.Statisitques["Credit3mois"];
                        toReturn -= this.Statisitques["CreditEchus"];
                        _NonEchus = toReturn.ToString("#,#0.00");
                    }
                }
                else if (this.Statisitques.ContainsKey("CreditNonEchus"))
                {
                    _NonEchus = this.Statisitques["CreditNonEchus"].ToString("#,#0.00");
                }
                if (_NonEchus == "")
                {
                    _NonEchus = "0,00";
                }
                return _NonEchus + "€";
            }
            set
            {
                _NonEchus = value;
            }
        }

        public List<Utilisateur> Contact { get; set; }

        public List<entetedocument> Factures { get; set; }//DO_TYpe=6

        public List<Panier> FacturesAvoirs { get; set; }

        public Dictionary<string, float> Remises { get; set; }

        public Dictionary<string, decimal> Statisitques { get; set; }

        public Utilisateur(string codeClient)
        {
            this.CodeClient = codeClient;
            Remises = new Dictionary<string, float>();
        }
    }
}