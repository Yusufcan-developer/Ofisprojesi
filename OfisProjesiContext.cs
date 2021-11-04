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

        public virtual DbSet<Debit> Debits { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Fixture> Fixtures { get; set; }
        public virtual DbSet<Office> Offices { get; set; }

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

            modelBuilder.Entity<Debit>(entity =>
            {
                entity.ToTable("debit");

                entity.HasIndex(e => e.FixtureId, "fki_demirbaszimmeting");

                entity.HasIndex(e => e.EmployeeId, "fki_zimmeting");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.FixtureId).HasColumnName("fixture_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Debits)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("Zimmet_zimmetlenen_kisi_fkey");

                entity.HasOne(d => d.Fixture)
                    .WithMany(p => p.Debits)
                    .HasForeignKey(d => d.FixtureId)
                    .HasConstraintName("demirbaszimmeting");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.HasIndex(e => e.OfficeId, "asd");

                entity.HasIndex(e => e.OfficeId, "bagli_oldugu_ofis");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(25)
                    .HasColumnName("lastname");

                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .HasColumnName("name");

                entity.Property(e => e.OfficeId).HasColumnName("office_id");

                entity.Property(e => e.RecordDate)
                    .HasColumnType("date")
                    .HasColumnName("record_date");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("update_date");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("calisan_bagli_oldugu_ofis_fkey");
            });
            modelBuilder.Entity<Fixture>(entity =>
            {
                entity.ToTable("fixture");

                entity.HasIndex(e => e.OfficeId, "demirbass");

                entity.HasIndex(e => e.OfficeId, "officeidd");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .HasColumnName("name");

                entity.Property(e => e.OfficeId).HasColumnName("office_id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Fixtures)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("fixture_office_id_fkey");
            });

            modelBuilder.Entity<Office>(entity =>
            {
                entity.ToTable("office");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Name)
                    .HasMaxLength(25)
                    .HasColumnName("name");

                entity.Property(e => e.Status).HasColumnName("status");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
