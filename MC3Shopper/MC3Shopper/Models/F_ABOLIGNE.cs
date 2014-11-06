namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_ABOLIGNE
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AB_No { get; set; }

        public int? AL_Ligne { get; set; }

        [StringLength(17)]
        public string AL_Ref { get; set; }

        public short? AL_TRemPied { get; set; }

        public short? AL_TRemExep { get; set; }

        [StringLength(19)]
        public string AR_Ref { get; set; }

        [MaxLength(20)]
        public byte[] cbAR_Ref { get; set; }

        [StringLength(69)]
        public string AL_Design { get; set; }

        public decimal? AL_Qte { get; set; }

        public decimal? AL_PoidsNet { get; set; }

        public decimal? AL_PoidsBrut { get; set; }

        public decimal? AL_Remise01REM_Valeur { get; set; }

        public short? AL_Remise01REM_Type { get; set; }

        public decimal? AL_Remise02REM_Valeur { get; set; }

        public short? AL_Remise02REM_Type { get; set; }

        public decimal? AL_Remise03REM_Valeur { get; set; }

        public short? AL_Remise03REM_Type { get; set; }

        public decimal? AL_PrixUnitaire { get; set; }

        public decimal? AL_Taxe1 { get; set; }

        public short? AL_TypeTaux1 { get; set; }

        public short? AL_TypeTaxe1 { get; set; }

        public decimal? AL_Taxe2 { get; set; }

        public short? AL_TypeTaux2 { get; set; }

        public short? AL_TypeTaxe2 { get; set; }

        public int? AG_No1 { get; set; }

        public int? AG_No2 { get; set; }

        [StringLength(19)]
        public string AF_RefFourniss { get; set; }

        [StringLength(21)]
        public string EU_Enumere { get; set; }

        public decimal? EU_Qte { get; set; }

        public short? AL_TTC { get; set; }

        public int? DE_No { get; set; }

        public int? cbDE_No { get; set; }

        public decimal? AL_PUDevise { get; set; }

        public decimal? AL_PUTTC { get; set; }

        [StringLength(13)]
        public string CA_Num { get; set; }

        [MaxLength(14)]
        public byte[] cbCA_Num { get; set; }

        public decimal? AL_Taxe3 { get; set; }

        public short? AL_TypeTaux3 { get; set; }

        public short? AL_TypeTaxe3 { get; set; }

        public short? AL_Period { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? AL_Debut { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? AL_Fin { get; set; }

        public short? AL_GestAnnee { get; set; }

        public short? AL_Prorata { get; set; }

        public short? AL_Reconduction { get; set; }

        public decimal? AL_PrixRU { get; set; }

        public decimal? AL_CMUP { get; set; }

        public short? AL_Valorise { get; set; }

        [StringLength(19)]
        public string AR_RefCompose { get; set; }

        [StringLength(19)]
        public string AC_RefClient { get; set; }

        public decimal? AL_MontantHT { get; set; }

        public decimal? AL_MontantTTC { get; set; }

        public short? AL_FactPoids { get; set; }

        public short? AL_Escompte { get; set; }

        public short? cbProt { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cbMarq { get; set; }

        [StringLength(4)]
        public string cbCreateur { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? cbModification { get; set; }

        public int? cbReplication { get; set; }

        public short? cbFlag { get; set; }

        [StringLength(40)]
        public string NO_SERIE { get; set; }

        public decimal? FICHE { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DELAIS { get; set; }

        [StringLength(15)]
        public string DATE_ARRIVEE { get; set; }

        [StringLength(69)]
        public string ACHATS { get; set; }

        public int? CO_No { get; set; }

        public int? cbCO_No { get; set; }

        public int? DT_No { get; set; }

        public int? cbDT_No { get; set; }

        [MaxLength(20)]
        public byte[] cbAF_RefFourniss { get; set; }

        [StringLength(11)]
        public string RP_Code { get; set; }

        [MaxLength(12)]
        public byte[] cbRP_Code { get; set; }

        public int? AL_QteRessource { get; set; }
    }
}
