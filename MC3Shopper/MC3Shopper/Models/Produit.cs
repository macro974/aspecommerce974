using System;
using System.Collections.Generic;
using System.IO;

namespace MC3Shopper.Models
{
    [Serializable]
    public class Produit
    {
        public decimal PrixOriginal;
        private float _Taxe2;
        private float _Taxe3;
        private string _designation;
        private string _designationOriginal;
        private decimal _prix;
        private float _remise;
        private Dictionary<string, float> _taxes;
        private decimal prixTotal;
        private float tva;
        private int _AR_UnitePoids;
        public Produit()
        {
        }

        public Produit(string Img, string Ref, string Design, decimal Price)
        {
            //this.ImgPath = Img;
            Reference = Ref;
            Designation = Design;
            Prix = Price;
            QteDemande = 0;
            string path = "S:\\Raccourci Gesco\\v16\\Multimedia\\";
            //String[] ch_img = Img.Split()
            if (File.Exists("~/images/articles/" + Img))
            {
                ImgPath = "~/images/articles/" + Img;
            }
            else
            {
                ImgPath = "~/Content/img/no-image.png";
            }
            ProduitAssocies = new List<Produit>();
            Taxes = new Dictionary<string, float>();
        }

        public Produit(bool livraisonFree)
        {
            Reference = "FRAIS_DE_TRANSPORT";
            Designation = "FRAIS DE TRANSPORT SUR VENTE";
            if (livraisonFree)
            {
                Prix = 0;
            }
            else
            {
                Prix = 35;
            }
            QteDemande = 1;
            PrixTotal = Prix * int.Parse(QteDemande.ToString());
            ProduitAssocies = new List<Produit>();
            Taxes = new Dictionary<string, float>();
            Taxes["tva"] = 8.5f;
            isOK = true;
        }

        public lignedocument LigneAssociee { get; set; }

        public string ImgPath { get; set; }

        public string Reference { get; set; }

        public bool isOK { get; set; }

        public string Informations { get; set; }

        public string Designation
        {
            get { return _designation; }
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
                foreach (var item in Taxes)
                {
                    if (item.Key.ToLower().Contains("eco") || item.Key.ToLower().Contains("ecoparticipation"))
                    {
                        _designation = _designationOriginal + "\r Eco taxe : " + item.Value;
                    }
                }
                return _designation;
            }
        }

        public string PrixFormate { get; set; }

        public float Remise
        {
            get { return _remise; }
            set { _remise = value; }
        }

        public string CodeFamille { get; set; }

        public float StockRes { get; set; }

        public float TVA
        {
            get
            {
                //return GestionSys.GetTVAOfProduit(Reference);
                foreach (var item in Taxes)
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

        public float Taxe2
        {
            get { return _Taxe2; }
            set { _Taxe2 = value; }
        }

        public float Taxe3
        {
            get { return _Taxe3; }
            set { _Taxe3 = value; }
        }

        public decimal Prix
        {
            get
            {
                decimal lol = decimal.Round(_prix, 2);
                decimal ok = lol - ((lol / 100) * decimal.Parse(_remise.ToString()));
                decimal okk = decimal.Round(ok, 2);
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

        public Dictionary<string, float> Taxes
        {
            get { return _taxes; }
            set { _taxes = value; }
        }

        public decimal PUTTC { get; set; }

        public decimal CMUP { get; set; }

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
            get { return StockDispo_denis + StockDispo_pierre; }
            set { }
        }

        public float StockDispo_pierre { get; set; }

        public float QteRes_denis { get; set; }

        public float QteRes_pierre { get; set; }

        public float StockDispo_denis { get; set; }

        public float QteEnCommande { get; set; }

        public string Disponibilite { get; set; }

        public DateTime? QteLivraison { get; set; }

        public float QteDemande { get; set; }

        public double AR_PoidsNet { get; set; }

        public double AR_PoidsBrut { get; set; }

        public double DL_PoidsBrut {get;set;}
        

        public double DL_PoidsNet { get; set; }

        public int AR_UnitePoids
        {
            get
            {
                return AR_UnitePoids;
            }

            set
            {
                _AR_UnitePoids = value;
                switch (_AR_UnitePoids)
                {
                    case 0:
                        DL_PoidsBrut = AR_PoidsBrut * 1000000;
                        DL_PoidsNet = AR_PoidsNet * 1000000;
                        break;

                    case 1:
                        DL_PoidsBrut = AR_PoidsBrut * 100000;
                        DL_PoidsNet = AR_PoidsNet * 100000;
                        break;

                    case 2:
                        DL_PoidsBrut = AR_PoidsBrut * 1000;
                        DL_PoidsNet = AR_PoidsNet * 1000;
                        break;

                    case 3:
                        DL_PoidsBrut = AR_PoidsBrut * 1;
                        DL_PoidsNet = AR_PoidsNet * 1;
                        break;

                    case 5:
                        DL_PoidsBrut = AR_PoidsBrut * 0.001;
                        DL_PoidsNet = AR_PoidsNet * 0.001;
                        break;

                    default:
                        break;
                }
            }
        }
    }
}