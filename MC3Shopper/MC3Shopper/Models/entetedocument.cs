using System;

namespace MC3Shopper.Models
{
    [Serializable]
    public class entetedocument
    {
        public string CT_NumPayeur;
        public int DE_No;
        public string DO_Coord01;
        public string DO_Coord02;
        public string DO_Coord03;
        public string DO_Coord04;
        public DateTime DO_DateLivr;
        public int DO_Domaine;
        public int DO_Expedit;
        public int DO_Period;
        public int DO_Regime;
        public string DO_Tiers;
        public int DO_Type;
        public int DO_transaction;
        public int LI_No;
        public int RE_No;

        public bool complete = false;
        public DateTime dateLivraison;
        public string DO_Piece { get; set; }

        public DateTime DO_Date { get; set; }

        public string DO_DateFormate
        {
            get { return DO_Date.ToString("dd/MM/yyyy"); }
        }

        public string DO_Ref { get; set; }
    }
}