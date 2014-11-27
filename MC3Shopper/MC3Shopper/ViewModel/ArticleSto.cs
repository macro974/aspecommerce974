using System;
using LINQtoCSV;

namespace MC3Shopper.ViewModel
{
    [Serializable]
    public class ArticleSto
    {
        [CsvColumn(Name = "Famille", FieldIndex = 1)]
        public string code_Famille { get; set; }

        [CsvColumn(Name = "Référence", FieldIndex = 2)]
        public string Reference { get; set; }

        [CsvColumn(Name = "Désignation", FieldIndex = 3)]
        public string Description { get; set; }

        [CsvColumn(Name = "Stock sur Saint-Denis", FieldIndex = 4, CanBeNull = false, OutputFormat = "F")]
        public float Qte_denis { get; set; }

        [CsvColumn(Name = "Stock sur Saint-Pierre", FieldIndex = 5, CanBeNull = false, OutputFormat = "F")]
        public float Qte_pierre { get; set; }

        [CsvColumn(Name = "Stock Disponible", FieldIndex = 6, CanBeNull = false, OutputFormat = "F")]
        public float Stockdisponible { get; set; }

        [CsvColumn(Name = "Prochain arrivage ", FieldIndex = 7)]
        public string arrivage { get; set; }


        [CsvColumn(Name = "Prix", FieldIndex = 8, OutputFormat = "C3")]
        public decimal prix { get; set; }
    }
}