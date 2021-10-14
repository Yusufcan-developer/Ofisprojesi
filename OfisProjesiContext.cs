using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ofisprojesi
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
        public virtual DbSet<Ofi> Ofis { get; set; }
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

                entity.HasIndex(e => e.BagliOlduguOfis, "bagli_oldugu_ofis");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Ad)
                    .HasMaxLength(25)
                    .HasColumnName("ad");

                entity.Property(e => e.BagliOlduguOfis).HasColumnName("bagli_oldugu_ofis");

                entity.Property(e => e.Durum).HasColumnName("durum");

                entity.Property(e => e.Soyad)
                    .HasMaxLength(25)
                    .HasColumnName("soyad");

                entity.HasOne(d => d.BagliOlduguOfisNavigation)
                    .WithMany(p => p.Calisans)
                    .HasForeignKey(d => d.BagliOlduguOfis)
                    .HasConstraintName("calisan_bagli_oldugu_ofis_fkey");
            });

            modelBuilder.Entity<Demirba>(entity =>
            {
                entity.ToTable("demirbas");

                entity.HasIndex(e => e.BulunduguOfis, "fki_ofisiddd");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Ad)
                    .HasMaxLength(25)
                    .HasColumnName("ad");

                entity.Property(e => e.BulunduguOfis).HasColumnName("bulundugu_ofis");

                entity.Property(e => e.Durum).HasColumnName("durum");

                entity.HasOne(d => d.BulunduguOfisNavigation)
                    .WithMany(p => p.Demirbas)
                    .HasForeignKey(d => d.BulunduguOfis)
                    .HasConstraintName("demirbas_bulundugu_ofis_fkey");
            });

            modelBuilder.Entity<Ofi>(entity =>
            {
                entity.ToTable("ofis");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Ad)
                    .HasMaxLength(25)
                    .HasColumnName("ad");

                entity.Property(e => e.Durum).HasColumnName("durum");
            });

            modelBuilder.Entity<Zimmet>(entity =>
            {
                entity.ToTable("zimmet");

                entity.HasIndex(e => e.ZimmetlenmisDemirbas, "fki_demirbaszimmeting");

                entity.HasIndex(e => e.ZimmetlenenCalisanlar, "fki_zimmeting");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Durum).HasColumnName("durum");

                entity.Property(e => e.Tarih)
                    .HasColumnType("date")
                    .HasColumnName("tarih");

                entity.Property(e => e.ZimmetlenenCalisanlar).HasColumnName("zimmetlenen_calisanlar");

                entity.Property(e => e.ZimmetlenmisDemirbas).HasColumnName("zimmetlenmis_demirbas");

                entity.HasOne(d => d.ZimmetlenenCalisanlarNavigation)
                    .WithMany(p => p.Zimmets)
                    .HasForeignKey(d => d.ZimmetlenenCalisanlar)
                    .HasConstraintName("Zimmet_zimmetlenen_kisi_fkey");

                entity.HasOne(d => d.ZimmetlenmisDemirbasNavigation)
                    .WithMany(p => p.Zimmets)
                    .HasForeignKey(d => d.ZimmetlenmisDemirbas)
                    .HasConstraintName("demirbaszimmeting");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
