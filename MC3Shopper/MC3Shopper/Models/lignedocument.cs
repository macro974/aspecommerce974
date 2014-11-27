using System;

namespace MC3Shopper.Models
{
    [Serializable]
    public class lignedocument
    {
        public string AF_RefFourniss;
        public string AR_Ref;
        public string CT_Num;
        public int DE_No;
        public double DL_CMUP;
        public DateTime DL_DateBC;
        public DateTime DL_DateBL;
        public string DL_Design;
        public int DL_Ligne;
        public double DL_MontantHT;
        public double DL_MontantTTC;
        public int DL_MvtStock;
        public int DL_No;
        public double DL_PUDevise;
        public double DL_PUTTC;
        public string DL_PieceBC;
        public double DL_PrixRU;
        public double DL_PrixUnitaire;
        public float DL_Qte;
        public float DL_QteBC;
        public float DL_QteBL;
        public int DL_TTC;
        public DateTime DO_Date;
        public DateTime DO_DateLivr;
        public string DO_Piece;
        public string DO_Ref;
        public int DO_Type;
        public string DoTiersF = "";
        public float EU_Qte;
        public string cbCreateur;
        public int cbMarq;
        public string dateArrivee;
        public DateTime dateLivraison;
        public bool enStock = false;
        public string infos = "";
        public string refFournisseurSiNonStock = "";
    }
}