using System;
using System.Collections.Generic;
using System.IO;

namespace MC3Shopper.Models
{
    [Serializable()]
    public class Produit
    {
        public lignedocument LigneAssociee { get; set; }

        public string ImgPath { get; set; }

        public string Reference { get; set; }

        public bool isOK { get; set; }

        public string Informations { get; set; }

        private string _designationOriginal;
        private string _designation;

        public string Designation
        {
            get
            {
                return _designation;
            }
            set
            {
                _designation = value;
                _designationOriginal = value;
            }
        }

        public string DesignationFac
        {
            get
            {
                foreach (KeyValuePair<string, float> item in this.Taxes)
                {
                    if (item.Key.ToLower().Contains("eco") || item.Key.ToLower().Contains("ecoparticipation"))
                    {
                        _designation = _designationOriginal + "\r Eco taxe : " + item.Value.ToString();
                    }
                }
                return _designation;
            }
        }

        public string PrixFormate { get; set; }

        private float _remise = 0;

        public float Remise
        {
            get
            {
                return _remise;
            }
            set
            {
                _remise = value;
            }
        }

        public string CodeFamille { get; set; }

        public float StockRes { get; set; }

        private float tva = 0;

        public float TVA
        {
            get
            {
                //return GestionSys.GetTVAOfProduit(Reference);
                foreach (KeyValuePair<string, float> item in this.Taxes)
                {
                    if (item.Key.ToLower().Contains("tva"))
                    {
                        tva = item.Value;
                        PUTTC = Prix * decimal.Parse(tva.ToString());
                    }
                }
                return tva;
            }
            set
            {
                tva = value;
                PUTTC = Prix * decimal.Parse(value.ToString());
            }
        }

        private float _Taxe2 = 0;

        public float Taxe2
        {
            get
            {
                return _Taxe2;
            }
            set
            {
                _Taxe2 = value;
            }
        }

        private float _Taxe3 = 0;

        public float Taxe3
        {
            get
            {
                return _Taxe3;
            }
            set
            {
                _Taxe3 = value;
            }
        }

        public decimal PrixOriginal;
        private decimal _prix;

        public decimal Prix
        {
            get
            {
                decimal lol = decimal.Round(_prix, 2);
                decimal ok = lol - ((lol / 100) * decimal.Parse(_remise.ToString()));
                decimal okk = decimal.Round(ok, 6);
                return okk;
                //return _prix - ((_prix / 100) * decimal.Parse(_remise.ToString()));
            }
            set
            {
                _prix = value;
                PrixOriginal = value;
            }
        }

        public string PrixTotalFormate { get; set; }

        public List<Produit> ProduitAssocies { get; set; }

        private Dictionary<string, float> _taxes;

        public Dictionary<string, float> Taxes
        {
            get
            {
                return _taxes;
            }
            set
            {
                _taxes = value;
            }
        }

        public decimal PUTTC { get; set; }

        public decimal CMUP { get; set; }

        private decimal prixTotal;

        public decimal PrixTotal
        {
            get
            {
                return decimal.Round(prixTotal, 2);
                //return prixTotal;
            }
            set
            {
                prixTotal = value;

                PrixTotalFormate = value.ToString("0.00") + "€";
            }
        }

        public decimal PrixTTC
        {
            get
            {
                decimal lol = decimal.Round(_prix, 2);
                decimal ok = lol - ((lol / 100) * decimal.Parse(_remise.ToString()));
                decimal okk = decimal.Round(ok, 6);
                decimal test = PrixTotal * decimal.Parse(TVA.ToString()) / 100 + PrixTotal;
                return decimal.Round(test, 2);
            }
        }

        public float StockDisponible
        {
            get
            {
                return this.StockDispo_denis + StockDispo_pierre;
            }
            set
            {
            }
        }

        public float StockDispo_pierre { get; set; }

        public float StockDispo_denis { get; set; }

        public float QteEnCommande { get; set; }

        public string Disponibilite { get; set; }

        public DateTime? QteLivraison { get; set; }

        public float QteDemande { get; set; }

        public Produit()
        { }

        public Produit(string Img, string Ref, string Design, decimal Price)
        {
            //this.ImgPath = Img;
            this.Reference = Ref;
            this.Designation = Design;
            this.Prix = Price;
            this.QteDemande = 0;

            if (File.Exists("~/Imgs/articles/" + Img))
            {
                this.ImgPath = "~/Imgs/articles/" + Img;
            }
            else
            {
                this.ImgPath = "~/Imgs/no_picture2.png";
            }
            ProduitAssocies = new List<Produit>();
            Taxes = new Dictionary<string, float>();
        }

        public Produit(bool livraisonFree)
        {
            this.Reference = "FRAIS_DE_TRANSPORT";
            this.Designation = "FRAIS DE TRANSPORT SUR VENTE";
            if (livraisonFree)
            {
                this.Prix = 0;
            }
            else
            {
                this.Prix = 35;
            }
            this.QteDemande = 1;
            this.PrixTotal = this.Prix * int.Parse(this.QteDemande.ToString());
            ProduitAssocies = new List<Produit>();
            Taxes = new Dictionary<string, float>();
            Taxes["tva"] = 8.5f;
            this.isOK = true;
        }
    }
}