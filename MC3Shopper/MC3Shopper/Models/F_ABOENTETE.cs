namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_ABOENTETE
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AB_No { get; set; }

        [StringLength(17)]
        public string AE_Ref { get; set; }

        public short? AE_Period { get; set; }

        public short? AE_Devise { get; set; }

        public decimal? AE_Cours { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DE_No { get; set; }

        public int? cbDE_No { get; set; }

        public int? LI_No { get; set; }

        public int? cbLI_No { get; set; }

        [StringLength(17)]
        public string CT_NumPayeur { get; set; }

        [MaxLength(18)]
        public byte[] cbCT_NumPayeur { get; set; }

        public short? AE_Expedit { get; set; }

        public short? AE_NbFacture { get; set; }

        public short? AE_BLFact { get; set; }

        public decimal? AE_TxEscompte { get; set; }

        [StringLength(13)]
        public string CA_Num { get; set; }

        [MaxLength(14)]
        public byte[] cbCA_Num { get; set; }

        [StringLength(25)]
        public string AE_Coord01 { get; set; }

        [StringLength(25)]
        public string AE_Coord02 { get; set; }

        [StringLength(25)]
        public string AE_Coord03 { get; set; }

        [StringLength(25)]
        public string AE_Coord04 { get; set; }

        public short? AE_Condition { get; set; }

        public short? AE_Tarif { get; set; }

        public short? AE_Colisage { get; set; }

        public short? AE_TypeColis { get; set; }

        public short? AE_Transaction { get; set; }

        public short? AE_Langue { get; set; }

        public short? AE_Regime { get; set; }

        public short? N_CatCompta { get; set; }

        public short? AE_BaseCalcul { get; set; }

        public short? AE_Genere { get; set; }

        [StringLength(13)]
        public string CG_Num { get; set; }

        [MaxLength(14)]
        public byte[] cbCG_Num { get; set; }

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

        public int? CO_No { get; set; }

        public int? cbCO_No { get; set; }

        [StringLength(13)]
        public string CA_NumIFRS { get; set; }

        [StringLength(17)]
        public string CT_NumCentrale { get; set; }

        [MaxLength(18)]
        public byte[] cbCT_NumCentrale { get; set; }

        [StringLength(35)]
        public string AE_Contact { get; set; }

        [StringLength(20)]
        public string FournAffaireSpeciale { get; set; }

        [StringLength(10)]
        public string TypeAffaireSpeciale { get; set; }

        [StringLength(20)]
        public string NumeroAffaireSpeciale { get; set; }

        [StringLength(35)]
        public string ClientFinalAffaireSpeciale { get; set; }

        [StringLength(2)]
        public string PaysDestination { get; set; }
    }
}
