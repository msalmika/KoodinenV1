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
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
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
                    .HasConstraintName("FK__Kurssi__kayttaja__5EBF139D");
            });

            modelBuilder.Entity<KurssiSuoritu>(entity =>
            {
                entity.HasKey(e => e.KurssiSuoritusId)
                    .HasName("PK__KurssiSu__2B6AE3F78E7E4830");

                entity.Property(e => e.KurssiSuoritusId).HasColumnName("kurssiSuoritus_id");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.KurssiId).HasColumnName("kurssi_id");

                entity.Property(e => e.Kesken).HasColumnName("kesken");

                entity.Property(e => e.SuoritusPvm)
                    .HasColumnType("date")
                    .HasColumnName("suoritusPvm");

                entity.HasOne(d => d.Kayttaja)
                    .WithMany(p => p.KurssiSuoritus)
                    .HasForeignKey(d => d.KayttajaId)
                    .HasConstraintName("FK__KurssiSuo__kaytt__619B8048");

                entity.HasOne(d => d.Kurssi)
                    .WithMany(p => p.KurssiSuoritus)
                    .HasForeignKey(d => d.KurssiId)
                    .HasConstraintName("FK__KurssiSuo__kurss__628FA481");
            });

            modelBuilder.Entity<Ohjeistu>(entity =>
            {
                entity.HasKey(e => e.OhjeistusId)
                    .HasName("PK__Ohjeistu__6E092E37C7E604C0");

                entity.Property(e => e.OhjeistusId).HasColumnName("Ohjeistus_id");

                entity.Property(e => e.OppituntiId).HasColumnName("Oppitunti_id");

                entity.Property(e => e.TekstiKentta).HasMaxLength(2000);

                entity.HasOne(d => d.Oppitunti)
                    .WithMany(p => p.Ohjeistus)
                    .HasForeignKey(d => d.OppituntiId)
                    .HasConstraintName("FK__Ohjeistus__Oppit__787EE5A0");
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
                    .HasConstraintName("FK__Oppitunti__kurss__656C112C");
            });

            modelBuilder.Entity<OppituntiSuoritu>(entity =>
            {
                entity.HasKey(e => e.OppituntiSuoritusId)
                    .HasName("PK__Oppitunt__813DD82B71A5DAAC");

                entity.Property(e => e.OppituntiSuoritusId).HasColumnName("oppituntiSuoritus_id");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.Kesken).HasColumnName("kesken");

                entity.Property(e => e.OppituntiId).HasColumnName("oppitunti_id");

                entity.Property(e => e.SuoritusPvm)
                    .HasColumnType("date")
                    .HasColumnName("suoritusPvm");

                entity.HasOne(d => d.Kayttaja)
                    .WithMany(p => p.OppituntiSuoritus)
                    .HasForeignKey(d => d.KayttajaId)
                    .HasConstraintName("FK__Oppitunti__kaytt__68487DD7");

                entity.HasOne(d => d.Oppitunti)
                    .WithMany(p => p.OppituntiSuoritus)
                    .HasForeignKey(d => d.OppituntiId)
                    .HasConstraintName("FK__Oppitunti__oppit__693CA210");
            });

            modelBuilder.Entity<Palaute>(entity =>
            {
                entity.ToTable("Palaute");

                entity.Property(e => e.PalauteId).HasColumnName("Palaute_id");

                entity.Property(e => e.Lahettaja).HasMaxLength(50);

                entity.Property(e => e.Teksti).HasMaxLength(2000);
            });

            modelBuilder.Entity<SahkopostiListum>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");
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
                    .HasConstraintName("FK__Tehtava__oppitun__6C190EBB");
            });

            modelBuilder.Entity<TehtavaEpaonnistunut>(entity =>
            {
                entity.HasKey(e => e.EpaonnistunutId)
                    .HasName("PK__TehtavaE__DF833DBA3D9BE282");

                entity.ToTable("TehtavaEpaonnistunut");

                entity.Property(e => e.EpaonnistunutId).HasColumnName("Epaonnistunut_id");

                entity.Property(e => e.TehtavaId).HasColumnName("Tehtava_id");

                entity.Property(e => e.TehtavanNimi).HasMaxLength(50);

                entity.HasOne(d => d.Tehtava)
                    .WithMany(p => p.TehtavaEpaonnistunuts)
                    .HasForeignKey(d => d.TehtavaId)
                    .HasConstraintName("FK__TehtavaEp__Tehta__02FC7413");
            });

            modelBuilder.Entity<TehtavaSuoritu>(entity =>
            {
                entity.HasKey(e => e.TehtavaSuoritusId)
                    .HasName("PK__TehtavaS__127955F92E159BB6");

                entity.Property(e => e.TehtavaSuoritusId).HasColumnName("TehtavaSuoritus_id");

                entity.Property(e => e.KayttajaId).HasColumnName("kayttaja_id");

                entity.Property(e => e.SuoritusPvm)
                    .HasColumnType("date")
                    .HasColumnName("suoritusPvm");

                entity.Property(e => e.TehtavaId).HasColumnName("tehtava_id");

                entity.HasOne(d => d.Kayttaja)
                    .WithMany(p => p.TehtavaSuoritus)
                    .HasForeignKey(d => d.KayttajaId)
                    .HasConstraintName("FK__TehtavaSu__kaytt__6EF57B66");

                entity.HasOne(d => d.Tehtava)
                    .WithMany(p => p.TehtavaSuoritus)
                    .HasForeignKey(d => d.TehtavaId)
                    .HasConstraintName("FK__TehtavaSu__tehta__6FE99F9F");
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
                    .HasConstraintName("FK__Vihje__Tehtava_i__75A278F5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
