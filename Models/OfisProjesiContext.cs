using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ofisprojesi.Models
{
    public partial class OfisProjesiContext : DbContext
    {
        public OfisProjesiContext()
        {
        }

        public OfisProjesiContext(DbContextOptions<OfisProjesiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Calisan> Calisans { get; set; }
        public virtual DbSet<Demirba> Demirbas { get; set; }
        public virtual DbSet<ofi> ofis { get; set; }
        public virtual DbSet<Zimmet> Zimmets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseNpgsql("Host=localhost;Database=OfisProjesi;Username=yusuf;Password=yusufcan123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "tr_TR.UTF-8");

            modelBuilder.Entity<Calisan>(entity =>
            {
                entity.ToTable("calisan");

                entity.Property(e => e.Calisanid)
                    .HasColumnName("calisanid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.CalisanAdi)
                    .HasColumnType("character varying")
                    .HasColumnName("calisan_adi");

                entity.Property(e => e.CalisanSoyadi)
                    .HasColumnType("character varying")
                    .HasColumnName("calisan_soyadi");
            });

            modelBuilder.Entity<Demirba>(entity =>
            {
                entity.HasKey(e => e.Demirbasid)
                    .HasName("Demirbas_pkey");

                entity.ToTable("demirbas");

                entity.Property(e => e.Demirbasid).ValueGeneratedNever();

                entity.Property(e => e.DemirbasAdi).HasColumnType("character varying");
            });

            modelBuilder.Entity<ofi>(entity =>
            {
                entity.HasKey(e => e.Ofisid)
                    .HasName("Ofis_pkey");

                entity.ToTable("ofis");

                entity.Property(e => e.Ofisid)
                    .ValueGeneratedNever()
                    .HasColumnName("ofisid");

                entity.Property(e => e.OfisIsim)
                    .HasColumnType("character varying")
                    .HasColumnName("ofis_isim");
            });

            modelBuilder.Entity<Zimmet>(entity =>
            {
                entity.ToTable("zimmet");

                entity.Property(e => e.Zimmetid)
                    .ValueGeneratedNever()
                    .HasColumnName("zimmetid");

                entity.Property(e => e.Demirbas).HasColumnName("demirbas");

                entity.Property(e => e.Kisi).HasColumnName("kisi");

                entity.Property(e => e.Tarih)
                    .HasColumnType("date")
                    .HasColumnName("tarih");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
