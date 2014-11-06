using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC3Shopper.Models
{
    public class entetedocument
    {
        public int DO_Domaine;
        public int DO_Type;
        public string DO_Piece { get; set; }
        public DateTime DO_Date { get; set; }
        public string DO_DateFormate
        {
            get
            {
                return DO_Date.ToString("dd/MM/yyyy");
            }
        }

        public string DO_Ref { get; set; }
        public string DO_Tiers;
        public int RE_No;
        public int DO_Period;
        public int DE_No;
        public string CT_NumPayeur;
        public int DO_Expedit;
        public int DO_transaction;
        public int DO_Regime;
        public int LI_No;
        public string DO_Coord01;
        public string DO_Coord02;
        public string DO_Coord03;
        public string DO_Coord04;
        public DateTime DO_DateLivr;

        public bool complete = false;
        public DateTime dateLivraison;


        public entetedocument()
        {

        }
    }
}