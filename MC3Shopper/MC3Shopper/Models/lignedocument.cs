using System;

namespace MC3Shopper.Models
{
    public class lignedocument
    {
        public int DO_Type;
        public string CT_Num;
        public string DO_Piece;
        public string DL_PieceBC;
        public DateTime DO_Date;
        public DateTime DL_DateBC;
        public DateTime DL_DateBL;
        public int DL_Ligne;
        public string DO_Ref;
        public string AR_Ref;
        public string DL_Design;
        public float DL_Qte;
        public float DL_QteBC;
        public float DL_QteBL;
        public double DL_PrixUnitaire;
        public double DL_PrixRU;
        public double DL_CMUP;
        public int DL_MvtStock;
        public string AF_RefFourniss;
        public float EU_Qte;
        public int DL_TTC;
        public int DE_No;
        public double DL_PUDevise;
        public double DL_PUTTC;
        public int DL_No;
        public double DL_MontantHT;
        public double DL_MontantTTC;
        public int cbMarq;
        public string cbCreateur;
        public string dateArrivee;
        public DateTime DO_DateLivr;

        public string infos = "";
        public DateTime dateLivraison;
        public string refFournisseurSiNonStock = "";
        public bool enStock = false;
        public string DoTiersF = "";

        public lignedocument()
        { }
    }
}