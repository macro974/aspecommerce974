namespace MC3Shopper.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class F_COMPTET
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(17)]
        public string CT_Num { get; set; }

        [StringLength(35)]
        public string CT_Intitule { get; set; }

        public short? CT_Type { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(13)]
        public string CG_NumPrinc { get; set; }

        [StringLength(17)]
        public string CT_Qualite { get; set; }

        [StringLength(17)]
        public string CT_Classement { get; set; }

        [MaxLength(18)]
        public byte[] cbCT_Classement { get; set; }

        [StringLength(35)]
        public string CT_Contact { get; set; }

        [StringLength(35)]
        public string CT_Adresse { get; set; }

        [StringLength(35)]
        public string CT_Complement { get; set; }

        [StringLength(9)]
        public string CT_CodePostal { get; set; }

        [MaxLength(10)]
        public byte[] cbCT_CodePostal { get; set; }

        [StringLength(35)]
        public string CT_Ville { get; set; }

        [StringLength(25)]
        public string CT_CodeRegion { get; set; }

        [StringLength(35)]
        public string CT_Pays { get; set; }

        [StringLength(7)]
        public string CT_Raccourci { get; set; }

        [MaxLength(8)]
        public byte[] cbCT_Raccourci { get; set; }

        public short? BT_Num { get; set; }

        public short? N_Devise { get; set; }

        [StringLength(7)]
        public string CT_Ape { get; set; }

        [StringLength(25)]
        public string CT_Identifiant { get; set; }

        [StringLength(15)]
        public string CT_Siret { get; set; }

        [StringLength(21)]
        public string CT_Statistique01 { get; set; }

        [StringLength(21)]
        public string CT_Statistique02 { get; set; }

        [StringLength(21)]
        public string CT_Statistique03 { get; set; }

        [StringLength(21)]
        public string CT_Statistique04 { get; set; }

        [StringLength(21)]
        public string CT_Statistique05 { get; set; }

        [StringLength(21)]
        public string CT_Statistique06 { get; set; }

        [StringLength(21)]
        public string CT_Statistique07 { get; set; }

        [StringLength(21)]
        public string CT_Statistique08 { get; set; }

        [StringLength(21)]
        public string CT_Statistique09 { get; set; }

        [StringLength(21)]
        public string CT_Statistique10 { get; set; }

        [StringLength(35)]
        public string CT_Commentaire { get; set; }

        public decimal? CT_Encours { get; set; }

        public decimal? CT_Assurance { get; set; }

        [StringLength(17)]
        public string CT_NumPayeur { get; set; }

        [MaxLength(18)]
        public byte[] cbCT_NumPayeur { get; set; }

        public short? N_Risque { get; set; }

        public short? N_CatTarif { get; set; }

        public decimal? CT_Taux01 { get; set; }

        public decimal? CT_Taux02 { get; set; }

        public decimal? CT_Taux03 { get; set; }

        public decimal? CT_Taux04 { get; set; }

        public short? N_CatCompta { get; set; }

        public short? N_Period { get; set; }

        public short? CT_Facture { get; set; }

        public short? CT_BLFact { get; set; }

        public short? CT_Langue { get; set; }

        [StringLength(17)]
        public string CT_Edi1 { get; set; }

        [StringLength(3)]
        public string CT_Edi2 { get; set; }

        [StringLength(3)]
        public string CT_Edi3 { get; set; }

        public short? N_Expedition { get; set; }

        public short? N_Condition { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CT_DateCreate { get; set; }

        public short? CT_Saut { get; set; }

        public short? CT_Lettrage { get; set; }

        public short? CT_ValidEch { get; set; }

        public short? CT_Sommeil { get; set; }

        public int? DE_No { get; set; }

        public int? cbDE_No { get; set; }

        public short? CT_ControlEnc { get; set; }

        public short? CT_NotRappel { get; set; }

        public short? N_Analytique { get; set; }

        public short? cbN_Analytique { get; set; }

        [StringLength(13)]
        public string CA_Num { get; set; }

        [MaxLength(14)]
        public byte[] cbCA_Num { get; set; }

        [StringLength(21)]
        public string CT_Telephone { get; set; }

        [StringLength(21)]
        public string CT_Telecopie { get; set; }

        [StringLength(69)]
        public string CT_EMail { get; set; }

        [StringLength(69)]
        public string CT_Site { get; set; }

        [StringLength(25)]
        public string CT_Coface { get; set; }

        public short? CT_Surveillance { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CT_SvDateCreate { get; set; }

        [StringLength(33)]
        public string CT_SvFormeJuri { get; set; }

        [StringLength(11)]
        public string CT_SvEffectif { get; set; }

        public decimal? CT_SvCA { get; set; }

        public decimal? CT_SvResultat { get; set; }

        public short? CT_SvIncident { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CT_SvDateIncid { get; set; }

        public short? CT_SvPrivil { get; set; }

        [StringLength(3)]
        public string CT_SvRegul { get; set; }

        [StringLength(5)]
        public string CT_SvCotation { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CT_SvDateMaj { get; set; }

        [StringLength(61)]
        public string CT_SvObjetMaj { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CT_SvDateBilan { get; set; }

        public short? CT_SvNbMoisBilan { get; set; }

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
        public string TYPE_TIERS { get; set; }

        [StringLength(2)]
        public string COUNTRY_ISO { get; set; }

        [StringLength(1)]
        public string HP_SELLOUT { get; set; }

        [StringLength(1)]
        public string Affacturage { get; set; }

        public decimal? OBJECTIFS { get; set; }

        [StringLength(20)]
        public string HP_PartnerID { get; set; }

        [StringLength(20)]
        public string Banque_Coface { get; set; }

        [MaxLength(14)]
        public byte[] cbCG_NumPrinc { get; set; }

        public int? CO_No { get; set; }

        public int? cbCO_No { get; set; }

        public short? N_AnalytiqueIFRS { get; set; }

        public short? cbN_AnalytiqueIFRS { get; set; }

        [StringLength(13)]
        public string CA_NumIFRS { get; set; }

        public short? CT_PrioriteLivr { get; set; }

        public short? CT_LivrPartielle { get; set; }

        public int? MR_No { get; set; }

        public int? cbMR_No { get; set; }

        public short? CT_NotPenal { get; set; }

        public int? EB_No { get; set; }

        public int? cbEB_No { get; set; }

        [StringLength(17)]
        public string CT_NumCentrale { get; set; }

        [MaxLength(18)]
        public byte[] cbCT_NumCentrale { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CT_DateFermeDebut { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? CT_DateFermeFin { get; set; }

        public short? CT_FactureElec { get; set; }

        public short? CT_TypeNIF { get; set; }

        [StringLength(35)]
        public string CT_RepresentInt { get; set; }

        [StringLength(25)]
        public string CT_RepresentNIF { get; set; }

        [MaxLength(18)]
        public byte[] cbCT_Num { get; set; }
    }
}
