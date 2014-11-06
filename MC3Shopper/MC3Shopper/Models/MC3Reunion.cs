namespace MC3Shopper.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MC3Reunion : DbContext
    {
        public MC3Reunion()
            : base("name=MC3Reunion")
        {
        }

        public virtual DbSet<F_ABOENTETE> F_ABOENTETE { get; set; }
        public virtual DbSet<F_ABOLIGNE> F_ABOLIGNE { get; set; }
        public virtual DbSet<F_ARTICLE> F_ARTICLE { get; set; }
        public virtual DbSet<F_ARTSTOCK> F_ARTSTOCK { get; set; }
        public virtual DbSet<F_COMPTEG> F_COMPTEG { get; set; }
        public virtual DbSet<F_COMPTET> F_COMPTET { get; set; }
        public virtual DbSet<F_COMPTETG> F_COMPTETG { get; set; }
        public virtual DbSet<F_CONTACTT> F_CONTACTT { get; set; }
        public virtual DbSet<F_DEPOT> F_DEPOT { get; set; }
        public virtual DbSet<F_DOCENTETE> F_DOCENTETE { get; set; }
        public virtual DbSet<F_DOCLIGNE> F_DOCLIGNE { get; set; }
        public virtual DbSet<F_FAMILLE> F_FAMILLE { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<F_ABOENTETE>()
                .Property(e => e.AE_Cours)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOENTETE>()
                .Property(e => e.AE_TxEscompte)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOENTETE>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_Qte)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_PoidsNet)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_PoidsBrut)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_Remise01REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_Remise02REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_Remise03REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_PrixUnitaire)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_Taxe1)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_Taxe2)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.EU_Qte)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_PUDevise)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_PUTTC)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_Taxe3)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_PrixRU)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_CMUP)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_MontantHT)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.AL_MontantTTC)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_ABOLIGNE>()
                .Property(e => e.FICHE)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_PoidsNet)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_PoidsBrut)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_PrixAch)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Coef)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_PrixVen)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Frais01FR_Rem01REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Frais01FR_Rem02REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Frais01FR_Rem03REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Frais02FR_Rem01REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Frais02FR_Rem02REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Frais02FR_Rem03REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Frais03FR_Rem01REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Frais03FR_Rem02REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_Frais03FR_Rem03REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_PUNet)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_PrixAchNouv)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_CoefNouv)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_PrixVenNouv)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_CoutStd)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_QteComp)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTICLE>()
                .Property(e => e.AR_QteOperatoire)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_QteMini)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_QteMaxi)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_MontSto)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_QteSto)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_QteRes)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_QteCom)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_QteResCM)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_QteComCM)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_QtePrepa)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_ARTSTOCK>()
                .Property(e => e.AS_QteAControler)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTEG>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.CT_Encours)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.CT_Assurance)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.CT_Taux01)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.CT_Taux02)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.CT_Taux03)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.CT_Taux04)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.CT_SvCA)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.CT_SvResultat)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_COMPTET>()
                .Property(e => e.OBJECTIFS)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_COMPTETG>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_CONTACTT>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_DEPOT>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.DO_Cours)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.DO_TxEscompte)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.DO_Ecart)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.DO_Heure)
                .IsFixedLength();

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.DO_ValFrais)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.DO_ValFranco)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.DO_Taxe1)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.DO_Taxe2)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCENTETE>()
                .Property(e => e.DO_Taxe3)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_Qte)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_QteBC)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_QteBL)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_PoidsNet)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_PoidsBrut)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_Remise01REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_Remise02REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_Remise03REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_PrixUnitaire)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_PUBC)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_Taxe1)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_Taxe2)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_PrixRU)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_CMUP)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.EU_Qte)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_PUDevise)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_PUTTC)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_Taxe3)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_Frais)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_MontantHT)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_MontantTTC)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.FICHE)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_DOCLIGNE>()
                .Property(e => e.DL_QtePL)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Coef)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Frais01FR_Rem01REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Frais01FR_Rem02REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Frais01FR_Rem03REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Frais02FR_Rem01REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Frais02FR_Rem02REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Frais02FR_Rem03REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Frais03FR_Rem01REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Frais03FR_Rem02REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.FA_Frais03FR_Rem03REM_Valeur)
                .HasPrecision(24, 6);

            modelBuilder.Entity<F_FAMILLE>()
                .Property(e => e.cbCreateur)
                .IsFixedLength();
        }
    }
}
