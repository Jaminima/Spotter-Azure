using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Model.Models
{
    public partial class SpotterAzure_dbContext : DbContext
    {
        public static SpotterAzure_dbContext dbContext = new SpotterAzure_dbContext();

        public SpotterAzure_dbContext()
        {
        }

        public SpotterAzure_dbContext(DbContextOptions<SpotterAzure_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Artist> Artists { get; set; }
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
                optionsBuilder.UseSqlServer("Server=spotter-azuredbserver.database.windows.net;Database=Spotter-Azure_db;User Id=jaminima; Password=William48");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasKey(e => new { e.ArtId, e.ArtistId })
                    .HasName("PK__Artists__C2B54B003E8B56E9");

                entity.HasIndex(e => e.ArtistId, "UQ__Artists__6CD04000CC6A2CBF")
                    .IsUnique();

                entity.HasIndex(e => e.ArtId, "UQ__Artists__C4784F017FFA594B")
                    .IsUnique();

                entity.Property(e => e.ArtId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("art_id");

                entity.Property(e => e.ArtistId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("artist_id");

                entity.Property(e => e.ArtistName)
                    .HasColumnType("text")
                    .HasColumnName("artist_name");

                entity.Property(e => e.Details)
                    .HasColumnType("text")
                    .HasColumnName("details");

                entity.Property(e => e.TrueAt)
                    .HasColumnType("datetime")
                    .HasColumnName("true_at")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Listen>(entity =>
            {
                entity.ToTable("Listen");

                entity.HasIndex(e => e.ListenId, "UQ__Listen__DF7610CCA15B6DA2")
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
                    .HasConstraintName("FK__Listen__spot_id__5AB9788F");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Listens)
                    .HasPrincipalKey(p => p.TrackId)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK__Listen__track_id__59C55456");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.SessId)
                    .HasName("PK__Sessions__2282B9DB2947E624");

                entity.HasIndex(e => e.SessId, "UQ__Sessions__2282B9DA7FF8D8A6")
                    .IsUnique();

                entity.HasIndex(e => e.SpotId, "UQ__Sessions__330AF0F7572B2848")
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
                    .HasConstraintName("FK__Sessions__spot_i__662B2B3B");
            });

            modelBuilder.Entity<Skip>(entity =>
            {
                entity.ToTable("Skip");

                entity.HasIndex(e => e.SkipId, "UQ__Skip__931FA3A9AF6089C5")
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
                    .HasConstraintName("FK__Skip__spot_id__607251E5");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Skips)
                    .HasPrincipalKey(p => p.TrackId)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK__Skip__track_id__5F7E2DAC");
            });

            modelBuilder.Entity<Spotify>(entity =>
            {
                entity.HasKey(e => e.SpotId)
                    .HasName("PK__Spotify__330AF0F685006D47");

                entity.ToTable("Spotify");

                entity.HasIndex(e => e.SpotId, "UQ__Spotify__330AF0F7747BEF91")
                    .IsUnique();

                entity.HasIndex(e => e.SpotifyId, "UQ__Spotify__C253CFF1AA518F78")
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
                    .HasName("PK__Tracks__0D07174B7028C8B7");

                entity.HasIndex(e => e.TrackId, "UQ__Tracks__24ECC82F048563BA")
                    .IsUnique();

                entity.HasIndex(e => e.TrkId, "UQ__Tracks__FF49DBC830266459")
                    .IsUnique();

                entity.Property(e => e.TrkId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("trk_id");

                entity.Property(e => e.TrackId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("track_id");

                entity.Property(e => e.ArtistId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("artist_id");

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

                entity.HasOne(d => d.Artist)
                    .WithMany(p => p.Tracks)
                    .HasPrincipalKey(p => p.ArtistId)
                    .HasForeignKey(d => d.ArtistId)
                    .HasConstraintName("FK__Tracks__artist_i__55009F39");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
