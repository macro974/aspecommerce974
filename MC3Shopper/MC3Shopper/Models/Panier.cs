using System;
using System.Collections.Generic;

namespace MC3Shopper.Models
{
    [Serializable]
    public class Panier
    {
        public List<Produit> monPanier;

        private decimal totalPanier;

        public Panier()
        {
            monPanier = new List<Produit>();
            TotalPanier = 0;
        }

        public Panier(List<Produit> panier)
        {
            monPanier = panier;
        }

        public decimal TotalPanier
        {
            get { return totalPanier; }
            set
            {
                totalPanier = value;
                TotalPanierFormate = value.ToString("0.00") + "€";
            }
        }

        public string TotalPanierFormate { get; set; }

        public decimal TVAPanier
        {
            get { return decimal.Round(GetTVADuPanier(), 2); }
        }

        public void Add(string Reference)
        {
        }

        public decimal GetEcoTaxePanier()
        {
            decimal totalEcoTax = 0;
            foreach (Produit item in monPanier)
            {
                foreach (var taxx in item.Taxes)
                {
                    if (taxx.Key.ToLower().Contains("ecoparticipation"))
                    {
                        totalEcoTax += decimal.Parse(taxx.Value.ToString()) * item.Prix / 100 *
                                       int.Parse(item.QteDemande.ToString());
                        item.Taxe2 = taxx.Value;
                    }
                }
            }
            return totalEcoTax;
        }

        public decimal GetTVADuPanier()
        {
            decimal totalTVA = 0;
            decimal totalHT = 0;
            foreach (Produit item in monPanier)
            {
                foreach (var taxx in item.Taxes)
                {
                    if (taxx.Key.ToLower().Contains("tva"))
                    {
                        totalTVA += decimal.Parse(taxx.Value.ToString()) * item.Prix / 100 *
                                    int.Parse(item.QteDemande.ToString());
                        totalHT += item.Prix * int.Parse(item.QteDemande.ToString());
                        item.TVA = taxx.Value;
                    }
                }
            }
            //totalTVA = decimal.Parse(taxx.Value.ToString()) * item.Prix / 100
            return totalTVA;
        }

        public void Plus(string Reference)
        {
            var monProduit = new Produit();
            foreach (Produit item in monPanier)
            {
                if (item.Reference.Equals(Reference))
                {
                    //if (item.QteDemande < item.StockDisponible + item.QteEnCommande)
                    //{
                    item.QteDemande += 1;
                    monProduit = item;
                    Add(monProduit);
                    //}
                }
            }

            //Add(monProduit);
        }

        public void Moins(string Reference)
        {
            var monProduit = new Produit();
            foreach (Produit item in monPanier)
            {
                if (item.Reference.Equals(Reference))
                {
                    item.QteDemande -= 1;
                    monProduit = item;
                }
            }
            Add(monProduit);
        }

        public void Suprrimer(string Reference)
        {
            var monProduit = new Produit();
            foreach (Produit item in monPanier)
            {
                if (item.Reference.Equals(Reference))
                {
                    item.QteDemande = 0;
                    monProduit = item;
                }
            }
            Add(monProduit);
        }

        public void Add(Produit Product)
        {
            bool isExist = false;
            int cont = 0;
            int IndexToRemove = 0;
            TotalPanier = 0;
            foreach (Produit item in monPanier)
            {
                if (item.Reference.Equals(Product.Reference))
                {
                    if (Product.QteDemande > 0)
                    {
                        item.QteDemande = Product.QteDemande;
                        item.PrixTotal = item.Prix * int.Parse(item.QteDemande.ToString());
                    }
                    else
                    {
                        item.QteDemande = 0;
                        IndexToRemove = cont;
                    }
                    isExist = true;
                }
                TotalPanier += item.Prix * int.Parse(item.QteDemande.ToString());
                cont++;
            }

            if (!isExist & Product.QteDemande > 0)
            {
                Product.PrixTotal = Product.Prix * int.Parse(Product.QteDemande.ToString());
                monPanier.Add(Product);

                TotalPanier += Product.Prix * int.Parse(Product.QteDemande.ToString());
                if (Product.ProduitAssocies.Count > 0)
                {
                    foreach (Produit ssItem in Product.ProduitAssocies)
                    {
                        ssItem.QteDemande = Product.QteDemande;
                        ssItem.StockDisponible = Product.QteDemande;
                        ssItem.PrixTotal = ssItem.Prix * int.Parse(Product.QteDemande.ToString());
                        TotalPanier += ssItem.Prix * int.Parse(ssItem.QteDemande.ToString());
                        monPanier.Add(ssItem);
                    }
                }
            }

            if (Product.QteDemande <= 0)
            {
                try
                {
                    monPanier.RemoveAt(IndexToRemove);
                }
                catch (Exception)
                {
                }
            }
        }

        public void AddToPanier(Produit Produit)
        {
            Produit item = this.monPanier.Find(x => x.Reference == Produit.Reference);
            if (item != null)
            {
                // check si les 2 quantités ajouté ne dépasse pas le stock prévu
                if ((item.QteDemande + Produit.QteDemande) <= item.QteEnCommande + item.StockDisponible)
                {
                    this.monPanier.Remove(item);
                    item.QteDemande += Produit.QteDemande;
                    item.PrixTotal = item.Prix * int.Parse(item.QteDemande.ToString());
                    TotalPanier += item.Prix * int.Parse(Produit.QteDemande.ToString());
                    this.monPanier.Add(item);
                }
            }
            else
            {
                Produit.PrixTotal = Produit.Prix * int.Parse(Produit.QteDemande.ToString());
                TotalPanier += Produit.PrixTotal;
                this.monPanier.Add(Produit);
            }
        }
    }
}