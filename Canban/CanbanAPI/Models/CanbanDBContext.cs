using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CanbanAPI.Models
{
    public partial class CanbanDBContext : DbContext
    {
        public CanbanDBContext()
        {
        }

        public CanbanDBContext(DbContextOptions<CanbanDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ListyZadan> ListyZadans { get; set; } = null!;
        public virtual DbSet<PrzestrzenRoboczaUzytkownika> PrzestrzenRoboczaUzytkownikas { get; set; } = null!;
        public virtual DbSet<PrzestrzenieRobocze> PrzestrzenieRoboczes { get; set; } = null!;
        public virtual DbSet<TabliceZadan> TabliceZadans { get; set; } = null!;
        public virtual DbSet<Uzytkownicy> Uzytkownicies { get; set; } = null!;
        public virtual DbSet<ZadaniaUzytkownika> ZadaniaUzytkownikas { get; set; } = null!;
        public virtual DbSet<Zadanium> Zadania { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-9V8N67S\\SQLEXPRESS;Database=CanbanDB;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListyZadan>(entity =>
            {
                entity.HasKey(e => e.IdListaZadan);

                entity.ToTable("ListyZadan");

                entity.Property(e => e.IdListaZadan).HasColumnName("idListaZadan");

                entity.Property(e => e.DataUtworzeniaListaZadan)
                    .HasColumnType("datetime")
                    .HasColumnName("dataUtworzeniaListaZadan");

                entity.Property(e => e.IdTablicaZadan).HasColumnName("idTablicaZadan");

                entity.Property(e => e.NazwaListaZadan)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nazwaListaZadan");

                entity.HasOne(d => d.IdTablicaZadanNavigation)
                    .WithMany(p => p.ListyZadans)
                    .HasForeignKey(d => d.IdTablicaZadan)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ListyZadan_TabliceZadan");
            });

            modelBuilder.Entity<PrzestrzenRoboczaUzytkownika>(entity =>
            {
                entity.HasKey(e => e.IdPrzestrzenRoboczaUzytkownika);

                entity.ToTable("PrzestrzenRoboczaUzytkownika");

                entity.Property(e => e.IdPrzestrzenRoboczaUzytkownika).HasColumnName("idPrzestrzenRoboczaUzytkownika");

                entity.Property(e => e.IdPrzestrzenRobocza).HasColumnName("idPrzestrzenRobocza");

                entity.Property(e => e.IdUzytkownik).HasColumnName("idUzytkownik");

                entity.HasOne(d => d.IdPrzestrzenRoboczaNavigation)
                    .WithMany(p => p.PrzestrzenRoboczaUzytkownikas)
                    .HasForeignKey(d => d.IdPrzestrzenRobocza)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrzestrzenRoboczaUzytkownika_PrzestrzenieRobocze");

                entity.HasOne(d => d.IdUzytkownikNavigation)
                    .WithMany(p => p.PrzestrzenRoboczaUzytkownikas)
                    .HasForeignKey(d => d.IdUzytkownik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrzestrzenRoboczaUzytkownika_Uzytkownicy");
            });

            modelBuilder.Entity<PrzestrzenieRobocze>(entity =>
            {
                entity.HasKey(e => e.IdPrzestrzenRobocza);

                entity.ToTable("PrzestrzenieRobocze");

                entity.Property(e => e.IdPrzestrzenRobocza).HasColumnName("idPrzestrzenRobocza");

                entity.Property(e => e.DataUtworzeniaPrzestrzenRobocza)
                    .HasColumnType("datetime")
                    .HasColumnName("dataUtworzeniaPrzestrzenRobocza");

                entity.Property(e => e.NazwaPrzestrzenRobocza)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nazwaPrzestrzenRobocza");
            });

            modelBuilder.Entity<TabliceZadan>(entity =>
            {
                entity.HasKey(e => e.IdTablicaZadan);

                entity.ToTable("TabliceZadan");

                entity.Property(e => e.IdTablicaZadan).HasColumnName("idTablicaZadan");

                entity.Property(e => e.DataUtworzeniaTablicaZadan)
                    .HasColumnType("datetime")
                    .HasColumnName("dataUtworzeniaTablicaZadan");

                entity.Property(e => e.IdPrzestrzenRobocza).HasColumnName("idPrzestrzenRobocza");

                entity.Property(e => e.NazwaTablicaZadan)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("nazwaTablicaZadan");

                entity.HasOne(d => d.IdPrzestrzenRoboczaNavigation)
                    .WithMany(p => p.TabliceZadans)
                    .HasForeignKey(d => d.IdPrzestrzenRobocza)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_TabliceZadan_PrzestrzenieRobocze");
            });

            modelBuilder.Entity<Uzytkownicy>(entity =>
            {
                entity.HasKey(e => e.IdUzytkownika);

                entity.ToTable("Uzytkownicy");

                entity.Property(e => e.IdUzytkownika).HasColumnName("idUzytkownika");

                entity.Property(e => e.DataRejestracjaUzytkownik)
                    .HasColumnType("datetime")
                    .HasColumnName("dataRejestracjaUzytkownik");

                entity.Property(e => e.DataUrodzeniaUzytkownik)
                    .HasColumnType("date")
                    .HasColumnName("dataUrodzeniaUzytkownik");

                entity.Property(e => e.EmailUzytkownik)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("emailUzytkownik");

                entity.Property(e => e.HasloHashUzytkownik)
                    .HasMaxLength(64)
                    .HasColumnName("hasloHashUzytkownik")
                    .IsFixedLength();

                entity.Property(e => e.NazwaUzytkownik)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("nazwaUzytkownik");

                entity.Property(e => e.SaltHasloUzytkownik)
                    .HasMaxLength(128)
                    .HasColumnName("saltHasloUzytkownik")
                    .IsFixedLength();
            });

            modelBuilder.Entity<ZadaniaUzytkownika>(entity =>
            {
                entity.HasKey(e => e.IdZadanieUzytkownik);

                entity.ToTable("ZadaniaUzytkownika");

                entity.Property(e => e.IdZadanieUzytkownik).HasColumnName("idZadanieUzytkownik");

                entity.Property(e => e.DataPrzydzielenieZadanie)
                    .HasColumnType("datetime")
                    .HasColumnName("dataPrzydzielenieZadanie");

                entity.Property(e => e.IdUzytkownik).HasColumnName("idUzytkownik");

                entity.Property(e => e.IdZadanie).HasColumnName("idZadanie");

                entity.HasOne(d => d.IdUzytkownikNavigation)
                    .WithMany(p => p.ZadaniaUzytkownikas)
                    .HasForeignKey(d => d.IdUzytkownik)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZadaniaUzytkownika_Uzytkownicy");

                entity.HasOne(d => d.IdZadanieNavigation)
                    .WithMany(p => p.ZadaniaUzytkownikas)
                    .HasForeignKey(d => d.IdZadanie)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ZadaniaUzytkownika_Zadania");
            });

            modelBuilder.Entity<Zadanium>(entity =>
            {
                entity.HasKey(e => e.IdZadanie);

                entity.Property(e => e.IdZadanie).HasColumnName("idZadanie");

                entity.Property(e => e.CzyZadanieZakonczone).HasColumnName("czyZadanieZakonczone");

                entity.Property(e => e.DataPrognozowaniaZakoczenia)
                    .HasColumnType("datetime")
                    .HasColumnName("dataPrognozowaniaZakoczenia");

                entity.Property(e => e.DataUtworzenia)
                    .HasColumnType("datetime")
                    .HasColumnName("dataUtworzenia");

                entity.Property(e => e.DataZakonczenia)
                    .HasColumnType("datetime")
                    .HasColumnName("dataZakonczenia");

                entity.Property(e => e.IdLista).HasColumnName("idLista");

                entity.Property(e => e.NazwaZadanie)
                    .HasMaxLength(10)
                    .HasColumnName("nazwaZadanie")
                    .IsFixedLength();

                entity.HasOne(d => d.IdListaNavigation)
                    .WithMany(p => p.Zadania)
                    .HasForeignKey(d => d.IdLista)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Zadania_ListyZadan");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
