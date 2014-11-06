namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_ARTICLE
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(19)]
        public string AR_Ref { get; set; }

        [StringLength(69)]
        public string AR_Design { get; set; }

        [MaxLength(70)]
        public byte[] cbAR_Design { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(11)]
        public string FA_CodeFamille { get; set; }

        [StringLength(19)]
        public string AR_Substitut { get; set; }

        [MaxLength(20)]
        public byte[] cbAR_Substitut { get; set; }

        [StringLength(7)]
        public string AR_Raccourci { get; set; }

        public short? AR_Garantie { get; set; }

        public short? AR_UnitePoids { get; set; }

        public decimal? AR_PoidsNet { get; set; }

        public decimal? AR_PoidsBrut { get; set; }

        public short? AR_UniteVen { get; set; }

        public decimal? AR_PrixAch { get; set; }

        public decimal? AR_Coef { get; set; }

        public decimal? AR_PrixVen { get; set; }

        public short? AR_PrixTTC { get; set; }

        public short? AR_Gamme1 { get; set; }

        public short? AR_Gamme2 { get; set; }

        public short? AR_SuiviStock { get; set; }

        public short? AR_Nomencl { get; set; }

        [StringLength(21)]
        public string AR_Stat01 { get; set; }

        [StringLength(21)]
        public string AR_Stat02 { get; set; }

        [StringLength(21)]
        public string AR_Stat03 { get; set; }

        [StringLength(21)]
        public string AR_Stat04 { get; set; }

        [StringLength(21)]
        public string AR_Stat05 { get; set; }

        public short? AR_Escompte { get; set; }

        public short? AR_Delai { get; set; }

        public short? AR_HorsStat { get; set; }

        public short? AR_VteDebit { get; set; }

        public short? AR_NotImp { get; set; }

        public short? AR_Sommeil { get; set; }

        [StringLength(69)]
        public string AR_Langue1 { get; set; }

        [StringLength(69)]
        public string AR_Langue2 { get; set; }

        [StringLength(35)]
        public string AR_CodeEdiED_Code1 { get; set; }

        [StringLength(3)]
        public string AR_CodeEdiED_Code2 { get; set; }

        [StringLength(3)]
        public string AR_CodeEdiED_Code3 { get; set; }

        [StringLength(3)]
        public string AR_CodeEdiED_Code4 { get; set; }

        [StringLength(19)]
        public string AR_CodeBarre { get; set; }

        [StringLength(25)]
        public string AR_CodeFiscal { get; set; }

        [StringLength(35)]
        public string AR_Pays { get; set; }

        [StringLength(21)]
        public string AR_Frais01FR_Denomination { get; set; }

        public decimal? AR_Frais01FR_Rem01REM_Valeur { get; set; }

        public short? AR_Frais01FR_Rem01REM_Type { get; set; }

        public decimal? AR_Frais01FR_Rem02REM_Valeur { get; set; }

        public short? AR_Frais01FR_Rem02REM_Type { get; set; }

        public decimal? AR_Frais01FR_Rem03REM_Valeur { get; set; }

        public short? AR_Frais01FR_Rem03REM_Type { get; set; }

        [StringLength(21)]
        public string AR_Frais02FR_Denomination { get; set; }

        public decimal? AR_Frais02FR_Rem01REM_Valeur { get; set; }

        public short? AR_Frais02FR_Rem01REM_Type { get; set; }

        public decimal? AR_Frais02FR_Rem02REM_Valeur { get; set; }

        public short? AR_Frais02FR_Rem02REM_Type { get; set; }

        public decimal? AR_Frais02FR_Rem03REM_Valeur { get; set; }

        public short? AR_Frais02FR_Rem03REM_Type { get; set; }

        [StringLength(21)]
        public string AR_Frais03FR_Denomination { get; set; }

        public decimal? AR_Frais03FR_Rem01REM_Valeur { get; set; }

        public short? AR_Frais03FR_Rem01REM_Type { get; set; }

        public decimal? AR_Frais03FR_Rem02REM_Valeur { get; set; }

        public short? AR_Frais03FR_Rem02REM_Type { get; set; }

        public decimal? AR_Frais03FR_Rem03REM_Valeur { get; set; }

        public short? AR_Frais03FR_Rem03REM_Type { get; set; }

        public short? AR_Condition { get; set; }

        public decimal? AR_PUNet { get; set; }

        public short? AR_Contremarque { get; set; }

        public short? AR_FactPoids { get; set; }

        public short? AR_FactForfait { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? AR_DateCreation { get; set; }

        public short? AR_SaisieVar { get; set; }

        public short? AR_Transfere { get; set; }

        public short? AR_Publie { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? AR_DateModif { get; set; }

        [StringLength(259)]
        public string AR_Photo { get; set; }

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

        [StringLength(1)]
        public string SERIALNUMBER { get; set; }

        [MaxLength(20)]
        public byte[] cbAR_Ref { get; set; }

        [MaxLength(12)]
        public byte[] cbFA_CodeFamille { get; set; }

        public decimal? AR_PrixAchNouv { get; set; }

        public decimal? AR_CoefNouv { get; set; }

        public decimal? AR_PrixVenNouv { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? AR_DateApplication { get; set; }

        public decimal? AR_CoutStd { get; set; }

        public decimal? AR_QteComp { get; set; }

        public decimal? AR_QteOperatoire { get; set; }

        public int? CO_No { get; set; }

        public int? cbCO_No { get; set; }

        public short? AR_Prevision { get; set; }

        public int? CL_No1 { get; set; }

        public int? cbCL_No1 { get; set; }

        public int? CL_No2 { get; set; }

        public int? cbCL_No2 { get; set; }

        public int? CL_No3 { get; set; }

        public int? cbCL_No3 { get; set; }

        public int? CL_No4 { get; set; }

        public int? cbCL_No4 { get; set; }

        public short? AR_Type { get; set; }

        [StringLength(11)]
        public string RP_CodeDefaut { get; set; }

        [MaxLength(8)]
        public byte[] cbAR_Raccourci { get; set; }

        [MaxLength(20)]
        public byte[] cbAR_CodeBarre { get; set; }
    }
}
