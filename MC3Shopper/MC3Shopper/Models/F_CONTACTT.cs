namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_CONTACTT
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(17)]
        public string CT_Num { get; set; }

        [StringLength(35)]
        public string CT_Nom { get; set; }

        [MaxLength(36)]
        public byte[] cbCT_Nom { get; set; }

        [StringLength(35)]
        public string CT_Prenom { get; set; }

        public short? N_Service { get; set; }

        [StringLength(35)]
        public string CT_Fonction { get; set; }

        [StringLength(21)]
        public string CT_Telephone { get; set; }

        [StringLength(21)]
        public string CT_TelPortable { get; set; }

        [StringLength(21)]
        public string CT_Telecopie { get; set; }

        [StringLength(69)]
        public string CT_EMail { get; set; }

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

        [MaxLength(18)]
        public byte[] cbCT_Num { get; set; }

        [MaxLength(36)]
        public byte[] cbCT_Prenom { get; set; }

        public short? CT_Civilite { get; set; }

        public short? N_Contact { get; set; }

        public int? CT_No { get; set; }
    }
}
