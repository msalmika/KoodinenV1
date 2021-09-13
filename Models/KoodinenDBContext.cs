using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace KoodinenV1.Models
{
    public partial class KoodinenDBContext : DbContext
    {
        public KoodinenDBContext()
        {
        }

        public KoodinenDBContext(DbContextOptions<KoodinenDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Kayttaja> Kayttajas { get; set; }
        public virtual DbSet<Kurssi> Kurssis { get; set; }
        public virtual DbSet<KurssiSuoritu> KurssiSuoritus { get; set; }
        public virtual DbSet<Ohjeistu> Ohjeistus { get; set; }
        public virtual DbSet<Oppitunti> Oppituntis { get; set; }
        public virtual DbSet<OppituntiSuoritu> OppituntiSuoritus { get; set; }
        public virtual DbSet<Palaute> Palautes { get; set; }
        public virtual DbSet<SahkopostiListum> SahkopostiLista { get; set; }
        public virtual DbSet<Tehtava> Tehtavas { get; set; }
        public virtual DbSet<TehtavaEpaonnistunut> TehtavaEpaonnistunuts { get; set; }
        public virtual DbSet<TehtavaSuoritu> TehtavaSuoritus { get; set; }
        public virtual DbSet<Vihje> Vihjes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Kayttaja>(entity =>
            {
                entity.ToTable("Kayttaja");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("email");

                entity.Property(e => e.Nimi)
                    .HasMaxLength(100)
                    .HasColumnName("nimi");

                entity.Property(e => e.OnAdmin).HasColumnName("onAdmin");

                entity.Property(e => e.Salasana)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("salasana");
            });

            modelBuilder.Entity<Kurssi>(entity =>
            {
                entity.ToTable("Kurssi");

                entity.Property(e => e.KurssiId).HasColumnName("kurssi_id");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.Kuvaus)
                    .HasMaxLength(400)
                    .HasColumnName("kuvaus");

                entity.Property(e => e.Nimi)
                    .HasMaxLength(100)
                    .HasColumnName("nimi");

                entity.HasOne(d => d.Kayttaja)
                    .WithMany(p => p.Kurssis)
                    .HasForeignKey(d => d.KayttajaId)
                    .HasConstraintName("FK__Kurssi__kayttaja__267ABA7A");
            });

            modelBuilder.Entity<KurssiSuoritu>(entity =>
            {
                entity.HasKey(e => e.KurssiSuoritusId)
                    .HasName("PK__KurssiSu__2B6AE3F76B85AB21");

                entity.Property(e => e.KurssiSuoritusId).HasColumnName("kurssiSuoritus_id");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.KurssiId).HasColumnName("kurssi_id");

                entity.Property(e => e.SuoritusPvm)
                    .HasColumnType("date")
                    .HasColumnName("suoritusPvm");

                entity.HasOne(d => d.Kayttaja)
                    .WithMany(p => p.KurssiSuoritus)
                    .HasForeignKey(d => d.KayttajaId)
                    .HasConstraintName("FK__KurssiSuo__kaytt__29572725");

                entity.HasOne(d => d.Kurssi)
                    .WithMany(p => p.KurssiSuoritus)
                    .HasForeignKey(d => d.KurssiId)
                    .HasConstraintName("FK__KurssiSuo__kurss__2A4B4B5E");
            });

            modelBuilder.Entity<Ohjeistu>(entity =>
            {
                entity.HasKey(e => e.OhjeistusId)
                    .HasName("PK__Ohjeistu__6E092E37EA83699B");

                entity.Property(e => e.OhjeistusId).HasColumnName("Ohjeistus_id");

                entity.Property(e => e.OppituntiId).HasColumnName("Oppitunti_id");

                entity.Property(e => e.TekstiKentta).HasMaxLength(2000);

                entity.HasOne(d => d.Oppitunti)
                    .WithMany(p => p.Ohjeistus)
                    .HasForeignKey(d => d.OppituntiId)
                    .HasConstraintName("FK__Ohjeistus__Oppit__403A8C7D");
            });

            modelBuilder.Entity<Oppitunti>(entity =>
            {
                entity.ToTable("Oppitunti");

                entity.Property(e => e.OppituntiId).HasColumnName("oppitunti_id");

                entity.Property(e => e.KurssiId).HasColumnName("kurssi_id");

                entity.Property(e => e.Kuvaus)
                    .HasMaxLength(400)
                    .HasColumnName("kuvaus");

                entity.Property(e => e.Nimi)
                    .HasMaxLength(100)
                    .HasColumnName("nimi");

                entity.HasOne(d => d.Kurssi)
                    .WithMany(p => p.Oppituntis)
                    .HasForeignKey(d => d.KurssiId)
                    .HasConstraintName("FK__Oppitunti__kurss__2D27B809");
            });

            modelBuilder.Entity<OppituntiSuoritu>(entity =>
            {
                entity.HasKey(e => e.OppituntiSuoritusId)
                    .HasName("PK__Oppitunt__813DD82BEC02A03D");

                entity.Property(e => e.OppituntiSuoritusId).HasColumnName("oppituntiSuoritus_id");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.OppituntiId).HasColumnName("oppitunti_id");

                entity.Property(e => e.SuoritusPvm)
                    .HasColumnType("date")
                    .HasColumnName("suoritusPvm");

                entity.HasOne(d => d.Kayttaja)
                    .WithMany(p => p.OppituntiSuoritus)
                    .HasForeignKey(d => d.KayttajaId)
                    .HasConstraintName("FK__Oppitunti__kaytt__300424B4");

                entity.HasOne(d => d.Oppitunti)
                    .WithMany(p => p.OppituntiSuoritus)
                    .HasForeignKey(d => d.OppituntiId)
                    .HasConstraintName("FK__Oppitunti__oppit__30F848ED");
            });

            modelBuilder.Entity<Tehtava>(entity =>
            {
                entity.ToTable("Tehtava");

                entity.Property(e => e.TehtavaId).HasColumnName("tehtava_id");

                entity.Property(e => e.Kuvaus)
                    .HasMaxLength(400)
                    .HasColumnName("kuvaus");

                entity.Property(e => e.Nimi)
                    .HasMaxLength(200)
                    .HasColumnName("nimi");

                entity.Property(e => e.OppituntiId).HasColumnName("oppitunti_id");

                entity.Property(e => e.TehtavaUrl)
                    .HasMaxLength(200)
                    .HasColumnName("tehtavaUrl");

                entity.HasOne(d => d.Oppitunti)
                    .WithMany(p => p.Tehtavas)
                    .HasForeignKey(d => d.OppituntiId)
                    .HasConstraintName("FK__Tehtava__oppitun__33D4B598");
            });

            modelBuilder.Entity<TehtavaSuoritu>(entity =>
            {
                entity.HasKey(e => e.TehtavaSuoritusId)
                    .HasName("PK__TehtavaS__127955F9A1A708CC");

                entity.Property(e => e.TehtavaSuoritusId).HasColumnName("TehtavaSuoritus_id");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.SuoritusPvm)
                    .HasColumnType("date")
                    .HasColumnName("suoritusPvm");

                entity.Property(e => e.TehtavaId).HasColumnName("tehtava_id");

                entity.HasOne(d => d.Kayttaja)
                    .WithMany(p => p.TehtavaSuoritus)
                    .HasForeignKey(d => d.KayttajaId)
                    .HasConstraintName("FK__TehtavaSu__kaytt__36B12243");

                entity.HasOne(d => d.Tehtava)
                    .WithMany(p => p.TehtavaSuoritus)
                    .HasForeignKey(d => d.TehtavaId)
                    .HasConstraintName("FK__TehtavaSu__tehta__37A5467C");
            });

            modelBuilder.Entity<Vihje>(entity =>
            {
                entity.ToTable("Vihje");

                entity.Property(e => e.VihjeId).HasColumnName("Vihje_id");

                entity.Property(e => e.TehtavaId).HasColumnName("Tehtava_id");

                entity.Property(e => e.Vihje1).HasMaxLength(500);

                entity.Property(e => e.Vihje2).HasMaxLength(500);

                entity.Property(e => e.Vihje3).HasMaxLength(500);

                entity.HasOne(d => d.Tehtava)
                    .WithMany(p => p.Vihjes)
                    .HasForeignKey(d => d.TehtavaId)
                    .HasConstraintName("FK__Vihje__Tehtava_i__3D5E1FD2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
