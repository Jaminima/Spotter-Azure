using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Spotter_Azure.Models
{
    public partial class spotterdbContext : DbContext
    {
        public static spotterdbContext dbContext = new spotterdbContext();

        public spotterdbContext()
        {
        }

        public spotterdbContext(DbContextOptions<spotterdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Listen> Listens { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Skip> Skips { get; set; }
        public virtual DbSet<Spotify> Spotifies { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=spotter-sql.database.windows.net;Database=spotter-db;User Id=Jaminima; Password=William48");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Listen>(entity =>
            {
                entity.ToTable("Listen");

                entity.HasIndex(e => e.ListenId, "UQ__Listen__DF7610CC5146FD6F")
                    .IsUnique();

                entity.Property(e => e.ListenId).HasColumnName("listen_id");

                entity.Property(e => e.ListenAt)
                    .HasColumnType("datetime")
                    .HasColumnName("listen_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SpotId).HasColumnName("spot_id");

                entity.Property(e => e.TrackId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("track_id");

                entity.HasOne(d => d.Spot)
                    .WithMany(p => p.Listens)
                    .HasForeignKey(d => d.SpotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Listen__spot_id__63A3C44B");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Listens)
                    .HasPrincipalKey(p => p.TrackId)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK__Listen__track_id__62AFA012");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.SessId)
                    .HasName("PK__Sessions__2282B9DB6AF6D1D8");

                entity.HasIndex(e => e.SessId, "UQ__Sessions__2282B9DA369EFDDD")
                    .IsUnique();

                entity.HasIndex(e => e.SpotId, "UQ__Sessions__330AF0F7E31D6365")
                    .IsUnique();

                entity.Property(e => e.SessId).HasColumnName("sess_id");

                entity.Property(e => e.AuthToken)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("auth_token");

                entity.Property(e => e.SpotId).HasColumnName("spot_id");

                entity.HasOne(d => d.Spot)
                    .WithOne(p => p.Session)
                    .HasForeignKey<Session>(d => d.SpotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sessions__spot_i__6F1576F7");
            });

            modelBuilder.Entity<Skip>(entity =>
            {
                entity.ToTable("Skip");

                entity.HasIndex(e => e.SkipId, "UQ__Skip__931FA3A94DAE46D9")
                    .IsUnique();

                entity.Property(e => e.SkipId).HasColumnName("skip_id");

                entity.Property(e => e.SkipAt)
                    .HasColumnType("datetime")
                    .HasColumnName("skip_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SpotId).HasColumnName("spot_id");

                entity.Property(e => e.TrackId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("track_id");

                entity.HasOne(d => d.Spot)
                    .WithMany(p => p.Skips)
                    .HasForeignKey(d => d.SpotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Skip__spot_id__695C9DA1");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Skips)
                    .HasPrincipalKey(p => p.TrackId)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK__Skip__track_id__68687968");
            });

            modelBuilder.Entity<Spotify>(entity =>
            {
                entity.HasKey(e => e.SpotId)
                    .HasName("PK__Spotify__330AF0F6C3A6AFFA");

                entity.ToTable("Spotify");

                entity.HasIndex(e => e.SpotId, "UQ__Spotify__330AF0F7053DB40A")
                    .IsUnique();

                entity.HasIndex(e => e.SpotifyId, "UQ__Spotify__C253CFF19A8831EA")
                    .IsUnique();

                entity.Property(e => e.SpotId).HasColumnName("spot_id");

                entity.Property(e => e.AuthExpires)
                    .HasColumnType("datetime")
                    .HasColumnName("authExpires");

                entity.Property(e => e.AuthToken)
                    .HasColumnType("text")
                    .HasColumnName("authToken");

                entity.Property(e => e.RefreshToken)
                    .HasColumnType("text")
                    .HasColumnName("refreshToken");

                entity.Property(e => e.SkipThreshold).HasDefaultValueSql("((3))");

                entity.Property(e => e.SpotifyId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("spotify_id");
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasKey(e => new { e.TrkId, e.TrackId })
                    .HasName("PK__Tracks__0D07174BEF247E75");

                entity.HasIndex(e => e.TrackId, "UQ__Tracks__24ECC82F4C42670B")
                    .IsUnique();

                entity.HasIndex(e => e.TrkId, "UQ__Tracks__FF49DBC87E3FC466")
                    .IsUnique();

                entity.Property(e => e.TrkId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("trk_id");

                entity.Property(e => e.TrackId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("track_id");

                entity.Property(e => e.Features)
                    .HasColumnType("text")
                    .HasColumnName("features");

                entity.Property(e => e.Title)
                    .HasColumnType("text")
                    .HasColumnName("title");

                entity.Property(e => e.TrueAt)
                    .HasColumnType("datetime")
                    .HasColumnName("true_at")
                    .HasDefaultValueSql("(getdate())");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
