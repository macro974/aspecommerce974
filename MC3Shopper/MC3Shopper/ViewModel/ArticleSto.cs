using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MC3Shopper.ViewModel
{
     [Serializable]
    public class ArticleSto
    {
         public MC3Shopper.Models.F_ARTICLE f_article { get; set; }
         public MC3Shopper.Models.F_ARTSTOCK f_artstock { get; set; }
         public int Qte { get; set; }

         public int Qte_denis { get; set; }

         public int Qte_pierre { get; set; }

         public decimal? Qte_maximal_commande { get; set; }

         public MC3Shopper.Models.F_DOCLIGNE f_doc { get; set; }

         public void set_Qte()
         {
             this.Qte = (this.Qte_denis + this.Qte_pierre) > 0 ? (this.Qte_denis + this.Qte_pierre) : 0;
         }
    }
}