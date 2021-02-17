using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Spotter_Azure.DBModels
{
    public partial class spotterdbContext : DbContext
    {
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

                entity.HasIndex(e => e.ListenId, "UQ__Listen__DF7610CCCCA1B402")
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
                    .HasConstraintName("FK__Listen__spot_id__2BC97F7C");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.SessId)
                    .HasName("PK__Sessions__330AF0F64285117C");

                entity.HasIndex(e => e.SessId, "UQ__Sessions__330AF0F70FC61445")
                    .IsUnique();

                entity.Property(e => e.SessId).HasColumnName("sess_id");

                entity.Property(e => e.SpotId).HasColumnName("spot_id");

                entity.Property(e => e.SpotId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("spot_id");

                entity.Property(e => e.AuthToken)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("auth_token");

                entity.HasOne(d => d.Spot)
                    .WithOne(p => p.Session)
                    .HasForeignKey<Session>(d => d.SpotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Sessions__spot_i__42ACE4D4");
            });

            modelBuilder.Entity<Skip>(entity =>
            {
                entity.ToTable("Skip");

                entity.HasIndex(e => e.SkipId, "UQ__Skip__931FA3A993A8C6A2")
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
                    .HasConstraintName("FK__Skip__spot_id__308E3499");
            });

            modelBuilder.Entity<Spotify>(entity =>
            {
                entity.Ignore("KickedPlaylist");
                entity.Ignore("KickedTracks");
                entity.Ignore("last");
                entity.Ignore("_spotify");

                entity.HasKey(e => e.SpotId)
                    .HasName("PK__Spotify__330AF0F6CA517DD1");

                entity.ToTable("Spotify");

                entity.HasIndex(e => e.SpotId, "UQ__Spotify__330AF0F75C428C02")
                    .IsUnique();

                entity.HasIndex(e => e.SpotifyId, "UQ__Spotify__C253CFF155A347EB")
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

            OnModelCreatingPartial(modelBuilder);
        }

        public static spotterdbContext dbContext = new spotterdbContext();
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
