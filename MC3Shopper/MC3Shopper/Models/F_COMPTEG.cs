namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_COMPTEG
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(13)]
        public string CG_Num { get; set; }

        public short? CG_Type { get; set; }

        [StringLength(35)]
        public string CG_Intitule { get; set; }

        [StringLength(17)]
        public string CG_Classement { get; set; }

        [MaxLength(18)]
        public byte[] cbCG_Classement { get; set; }

        public short? N_Nature { get; set; }

        public short? CG_Report { get; set; }

        [StringLength(13)]
        public string CR_Num { get; set; }

        [MaxLength(14)]
        public byte[] cbCR_Num { get; set; }

        [StringLength(7)]
        public string CG_Raccourci { get; set; }

        [MaxLength(8)]
        public byte[] cbCG_Raccourci { get; set; }

        public short? CG_Saut { get; set; }

        public short? CG_Regroup { get; set; }

        public short? CG_Analytique { get; set; }

        public short? CG_Echeance { get; set; }

        public short? CG_Quantite { get; set; }

        public short? CG_Lettrage { get; set; }

        public short? CG_Tiers { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CG_DateCreate { get; set; }

        public short? CG_Devise { get; set; }

        public short? N_Devise { get; set; }

        [StringLength(5)]
        public string TA_Code { get; set; }

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

        public short? CG_Sommeil { get; set; }

        [MaxLength(15)]
        public byte[] cbCG_Num { get; set; }

        [MaxLength(6)]
        public byte[] cbTA_Code { get; set; }
    }
}
