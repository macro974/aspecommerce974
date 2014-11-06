namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_FAMILLE
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(11)]
        public string FA_CodeFamille { get; set; }

        [MaxLength(13)]
        public byte[] cbFA_CodeFamille { get; set; }

        public short? FA_Type { get; set; }

        [StringLength(35)]
        public string FA_Intitule { get; set; }

        [MaxLength(36)]
        public byte[] cbFA_Intitule { get; set; }

        public short? FA_UniteVen { get; set; }

        public decimal? FA_Coef { get; set; }

        public short? FA_SuiviStock { get; set; }

        public short? FA_Garantie { get; set; }

        [StringLength(11)]
        public string FA_Central { get; set; }

        [MaxLength(12)]
        public byte[] cbFA_Central { get; set; }

        [StringLength(21)]
        public string FA_Stat01 { get; set; }

        [StringLength(21)]
        public string FA_Stat02 { get; set; }

        [StringLength(21)]
        public string FA_Stat03 { get; set; }

        [StringLength(21)]
        public string FA_Stat04 { get; set; }

        [StringLength(21)]
        public string FA_Stat05 { get; set; }

        [StringLength(25)]
        public string FA_CodeFiscal { get; set; }

        [StringLength(35)]
        public string FA_Pays { get; set; }

        public short? FA_UnitePoids { get; set; }

        public short? FA_Escompte { get; set; }

        public short? FA_Delai { get; set; }

        public short? FA_HorsStat { get; set; }

        public short? FA_VteDebit { get; set; }

        public short? FA_NotImp { get; set; }

        [StringLength(21)]
        public string FA_Frais01FR_Denomination { get; set; }

        public decimal? FA_Frais01FR_Rem01REM_Valeur { get; set; }

        public short? FA_Frais01FR_Rem01REM_Type { get; set; }

        public decimal? FA_Frais01FR_Rem02REM_Valeur { get; set; }

        public short? FA_Frais01FR_Rem02REM_Type { get; set; }

        public decimal? FA_Frais01FR_Rem03REM_Valeur { get; set; }

        public short? FA_Frais01FR_Rem03REM_Type { get; set; }

        [StringLength(21)]
        public string FA_Frais02FR_Denomination { get; set; }

        public decimal? FA_Frais02FR_Rem01REM_Valeur { get; set; }

        public short? FA_Frais02FR_Rem01REM_Type { get; set; }

        public decimal? FA_Frais02FR_Rem02REM_Valeur { get; set; }

        public short? FA_Frais02FR_Rem02REM_Type { get; set; }

        public decimal? FA_Frais02FR_Rem03REM_Valeur { get; set; }

        public short? FA_Frais02FR_Rem03REM_Type { get; set; }

        [StringLength(21)]
        public string FA_Frais03FR_Denomination { get; set; }

        public decimal? FA_Frais03FR_Rem01REM_Valeur { get; set; }

        public short? FA_Frais03FR_Rem01REM_Type { get; set; }

        public decimal? FA_Frais03FR_Rem02REM_Valeur { get; set; }

        public short? FA_Frais03FR_Rem02REM_Type { get; set; }

        public decimal? FA_Frais03FR_Rem03REM_Valeur { get; set; }

        public short? FA_Frais03FR_Rem03REM_Type { get; set; }

        public short? FA_Contremarque { get; set; }

        public short? FA_FactPoids { get; set; }

        public short? FA_FactForfait { get; set; }

        public short? FA_Publie { get; set; }

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

        [StringLength(19)]
        public string FA_RacineRef { get; set; }

        [StringLength(19)]
        public string FA_RacineCB { get; set; }

        public int? CL_No1 { get; set; }

        public int? cbCL_No1 { get; set; }

        public int? CL_No2 { get; set; }

        public int? cbCL_No2 { get; set; }

        public int? CL_No3 { get; set; }

        public int? cbCL_No3 { get; set; }

        public int? CL_No4 { get; set; }

        public int? cbCL_No4 { get; set; }
    }
}
