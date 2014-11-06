namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_ARTSTOCK
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(19)]
        public string AR_Ref { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int DE_No { get; set; }

        public decimal? AS_QteMini { get; set; }

        public decimal? AS_QteMaxi { get; set; }

        public decimal? AS_MontSto { get; set; }

        public decimal? AS_QteSto { get; set; }

        public decimal? AS_QteRes { get; set; }

        public decimal? AS_QteCom { get; set; }

        public short? AS_Principal { get; set; }

        public decimal? AS_QteResCM { get; set; }

        public decimal? AS_QteComCM { get; set; }

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

        [MaxLength(20)]
        public byte[] cbAR_Ref { get; set; }

        public decimal? AS_QtePrepa { get; set; }

        public int? DP_NoPrincipal { get; set; }

        public int? cbDP_NoPrincipal { get; set; }

        public int? DP_NoControle { get; set; }

        public int? cbDP_NoControle { get; set; }

        public decimal? AS_QteAControler { get; set; }
    }
}
