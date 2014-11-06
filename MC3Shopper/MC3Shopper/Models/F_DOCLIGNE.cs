namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_DOCLIGNE
    {
        public short? DO_Domaine { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short DO_Type { get; set; }

        [StringLength(17)]
        public string CT_Num { get; set; }

        [MaxLength(18)]
        public byte[] cbCT_Num { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(9)]
        public string DO_Piece { get; set; }

        [MaxLength(10)]
        public byte[] cbDO_Piece { get; set; }

        [StringLength(9)]
        public string DL_PieceBC { get; set; }

        [MaxLength(10)]
        public byte[] cbDL_PieceBC { get; set; }

        [StringLength(9)]
        public string DL_PieceBL { get; set; }

        [MaxLength(10)]
        public byte[] cbDL_PieceBL { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DO_Date { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DL_DateBC { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DL_DateBL { get; set; }

        public int? DL_Ligne { get; set; }

        [StringLength(17)]
        public string DO_Ref { get; set; }

        public short? DL_TNomencl { get; set; }

        public short? DL_TRemPied { get; set; }

        public short? DL_TRemExep { get; set; }

        [StringLength(19)]
        public string AR_Ref { get; set; }

        [MaxLength(20)]
        public byte[] cbAR_Ref { get; set; }

        [StringLength(69)]
        public string DL_Design { get; set; }

        public decimal? DL_Qte { get; set; }

        public decimal? DL_QteBC { get; set; }

        public decimal? DL_QteBL { get; set; }

        public decimal? DL_PoidsNet { get; set; }

        public decimal? DL_PoidsBrut { get; set; }

        public decimal? DL_Remise01REM_Valeur { get; set; }

        public short? DL_Remise01REM_Type { get; set; }

        public decimal? DL_Remise02REM_Valeur { get; set; }

        public short? DL_Remise02REM_Type { get; set; }

        public decimal? DL_Remise03REM_Valeur { get; set; }

        public short? DL_Remise03REM_Type { get; set; }

        public decimal? DL_PrixUnitaire { get; set; }

        public decimal? DL_PUBC { get; set; }

        public decimal? DL_Taxe1 { get; set; }

        public short? DL_TypeTaux1 { get; set; }

        public short? DL_TypeTaxe1 { get; set; }

        public decimal? DL_Taxe2 { get; set; }

        public short? DL_TypeTaux2 { get; set; }

        public short? DL_TypeTaxe2 { get; set; }

        public int? AG_No1 { get; set; }

        public int? AG_No2 { get; set; }

        public decimal? DL_PrixRU { get; set; }

        public decimal? DL_CMUP { get; set; }

        public short? DL_MvtStock { get; set; }

        [StringLength(19)]
        public string AF_RefFourniss { get; set; }

        [StringLength(21)]
        public string EU_Enumere { get; set; }

        public decimal? EU_Qte { get; set; }

        public short? DL_TTC { get; set; }

        public int? DE_No { get; set; }

        public int? cbDE_No { get; set; }

        public short? DL_NoRef { get; set; }

        public short? DL_TypePL { get; set; }

        public decimal? DL_PUDevise { get; set; }

        public decimal? DL_PUTTC { get; set; }

        public int? DL_No { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DO_DateLivr { get; set; }

        [StringLength(13)]
        public string CA_Num { get; set; }

        [MaxLength(14)]
        public byte[] cbCA_Num { get; set; }

        public decimal? DL_Taxe3 { get; set; }

        public short? DL_TypeTaux3 { get; set; }

        public short? DL_TypeTaxe3 { get; set; }

        public decimal? DL_Frais { get; set; }

        public short? DL_Valorise { get; set; }

        [StringLength(19)]
        public string AR_RefCompose { get; set; }

        public short? DL_NonLivre { get; set; }

        [StringLength(19)]
        public string AC_RefClient { get; set; }

        public decimal? DL_MontantHT { get; set; }

        public decimal? DL_MontantTTC { get; set; }

        public short? DL_FactPoids { get; set; }

        public short? DL_Escompte { get; set; }

        public short? cbProt { get; set; }

        [Key]
        [Column(Order = 2)]
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

        [StringLength(9)]
        public string DL_PiecePL { get; set; }

        [MaxLength(10)]
        public byte[] cbDL_PiecePL { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DL_DatePL { get; set; }

        public decimal? DL_QtePL { get; set; }

        [StringLength(19)]
        public string DL_NoColis { get; set; }

        public int? DL_NoLink { get; set; }

        public int? cbDL_NoLink { get; set; }

        [StringLength(11)]
        public string RP_Code { get; set; }

        [MaxLength(12)]
        public byte[] cbRP_Code { get; set; }

        public int? DL_QteRessource { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DL_DateAvancement { get; set; }
    }
}
