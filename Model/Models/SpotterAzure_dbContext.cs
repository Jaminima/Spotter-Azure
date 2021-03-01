using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Model.Models
{
    public partial class SpotterAzure_dbContext : DbContext
    {
        public SpotterAzure_dbContext()
        {
        }

        //public SpotterAzure_dbContext(DbContextOptions<SpotterAzure_dbContext> options)
        //    : base(options)
        //{
        //}

        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Listen> Listens { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Skip> Skips { get; set; }
        public virtual DbSet<Spotify> Spotifies { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=spotter-azuredbserver.database.windows.net;Database=Spotter-Azure_db;User Id=Jaminima; Password=William48");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Artist>(entity =>
            {
                entity.HasKey(e => new { e.ArtId, e.ArtistId })
                    .HasName("PK__Artists__C2B54B0002FED1C9");

                entity.HasIndex(e => e.ArtistId, "UQ__Artists__6CD04000F6E2F214")
                    .IsUnique();

                entity.HasIndex(e => e.ArtId, "UQ__Artists__C4784F013FC5D1B6")
                    .IsUnique();

                entity.Property(e => e.ArtId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("art_id");

                entity.Property(e => e.ArtistId)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("artist_id");

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

                entity.HasIndex(e => e.ListenId, "UQ__Listen__DF7610CC3B2E6DB2")
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
                    .HasConstraintName("FK__Listen__spot_id__314D4EA8");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Listens)
                    .HasPrincipalKey(p => p.TrackId)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK__Listen__track_id__30592A6F");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.SessId)
                    .HasName("PK__Sessions__2282B9DBDDACAD0E");

                entity.HasIndex(e => e.SessId, "UQ__Sessions__2282B9DA28A72EC9")
                    .IsUnique();

                entity.HasIndex(e => e.SpotId, "UQ__Sessions__330AF0F73D151DD1")
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
                    .HasConstraintName("FK__Sessions__spot_i__4A18FC72");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => e.SettId)
                    .HasName("PK__Settings__600313CE7A436103");

                entity.HasIndex(e => e.SpotId, "UQ__Settings__330AF0F7CA03ABAC")
                    .IsUnique();

                entity.HasIndex(e => e.SettId, "UQ__Settings__600313CF8A6A11D1")
                    .IsUnique();

                entity.Property(e => e.SettId).HasColumnName("sett_id");

                entity.Property(e => e.ShuffleAlbums)
                    .HasColumnName("shuffle_albums")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ShuffleOn)
                    .HasColumnName("shuffle_on")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ShufflePlaylists)
                    .HasColumnName("shuffle_playlists")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SkipExpiryHours)
                    .HasColumnName("skip_expiry_hours")
                    .HasDefaultValueSql("((168))");

                entity.Property(e => e.SkipIgnorePlaylist)
                    .HasColumnName("skip_ignore_playlist")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SkipMustBeLiked)
                    .HasColumnName("skip_must_be_liked")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.SkipOn)
                    .HasColumnName("skip_on")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SkipRemoveFromPlaylist)
                    .HasColumnName("skip_remove_from_playlist")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SkipTrigger)
                    .HasColumnName("skip_trigger")
                    .HasDefaultValueSql("((3))");

                entity.Property(e => e.SpotId).HasColumnName("spot_id");

                entity.HasOne(d => d.Spot)
                    .WithOne(p => p.Setting)
                    .HasForeignKey<Setting>(d => d.SpotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Settings__spot_i__3CBF0154");
            });

            modelBuilder.Entity<Skip>(entity =>
            {
                entity.ToTable("Skip");

                entity.HasIndex(e => e.SkipId, "UQ__Skip__931FA3A987AD13C6")
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
                    .HasConstraintName("FK__Skip__spot_id__370627FE");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Skips)
                    .HasPrincipalKey(p => p.TrackId)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK__Skip__track_id__361203C5");
            });

            modelBuilder.Entity<Spotify>(entity =>
            {
                entity.HasKey(e => e.SpotId)
                    .HasName("PK__Spotify__330AF0F6406864E2");

                entity.ToTable("Spotify");

                entity.HasIndex(e => e.SpotId, "UQ__Spotify__330AF0F7ADBF99F3")
                    .IsUnique();

                entity.HasIndex(e => e.SpotifyId, "UQ__Spotify__C253CFF1C0E0E73F")
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

                entity.Property(e => e.SpotifyId)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("spotify_id");
            });

            modelBuilder.Entity<Track>(entity =>
            {
                entity.HasKey(e => new { e.TrkId, e.TrackId })
                    .HasName("PK__Tracks__0D07174B6ADD9F7C");

                entity.HasIndex(e => e.TrackId, "UQ__Tracks__24ECC82FE620B9A0")
                    .IsUnique();

                entity.HasIndex(e => e.TrkId, "UQ__Tracks__FF49DBC826114479")
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
                    .HasConstraintName("FK__Tracks__artist_i__2B947552");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
