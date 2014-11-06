namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_COMPTETG
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(17)]
        public string CT_Num { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(13)]
        public string CG_Num { get; set; }

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

        [MaxLength(18)]
        public byte[] cbCT_Num { get; set; }

        [MaxLength(14)]
        public byte[] cbCG_Num { get; set; }
    }
}
