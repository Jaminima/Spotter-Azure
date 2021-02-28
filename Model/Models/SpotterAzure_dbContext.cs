using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Model.Models
{
    public partial class SpotterAzure_dbContext : DbContext
    {
        #region Methods

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

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
                    .HasName("PK__Artists__C2B54B003A108C92");

                entity.HasIndex(e => e.ArtistId, "UQ__Artists__6CD04000FA39A85B")
                    .IsUnique();

                entity.HasIndex(e => e.ArtId, "UQ__Artists__C4784F01A4765DCF")
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

                entity.HasIndex(e => e.ListenId, "UQ__Listen__DF7610CC7B7E6425")
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
                    .HasConstraintName("FK__Listen__spot_id__589C25F3");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Listens)
                    .HasPrincipalKey(p => p.TrackId)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK__Listen__track_id__57A801BA");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.SessId)
                    .HasName("PK__Sessions__2282B9DB16EAE594");

                entity.HasIndex(e => e.SessId, "UQ__Sessions__2282B9DA7255E164")
                    .IsUnique();

                entity.HasIndex(e => e.SpotId, "UQ__Sessions__330AF0F79EB22EF4")
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
                    .HasConstraintName("FK__Sessions__spot_i__640DD89F");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.HasKey(e => e.SettId)
                    .HasName("PK__Settings__600313CE84C45C51");

                entity.HasIndex(e => e.SpotId, "UQ__Settings__330AF0F784B853F9")
                    .IsUnique();

                entity.HasIndex(e => e.SettId, "UQ__Settings__600313CF19FD1710")
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
                    .HasConstraintName("FK__Settings__spot_i__69C6B1F5");
            });

            modelBuilder.Entity<Skip>(entity =>
            {
                entity.ToTable("Skip");

                entity.HasIndex(e => e.SkipId, "UQ__Skip__931FA3A97910A8D3")
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
                    .HasConstraintName("FK__Skip__spot_id__5E54FF49");

                entity.HasOne(d => d.Track)
                    .WithMany(p => p.Skips)
                    .HasPrincipalKey(p => p.TrackId)
                    .HasForeignKey(d => d.TrackId)
                    .HasConstraintName("FK__Skip__track_id__5D60DB10");
            });

            modelBuilder.Entity<Spotify>(entity =>
            {
                entity.HasKey(e => e.SpotId)
                    .HasName("PK__Spotify__330AF0F6740A3E93");

                entity.ToTable("Spotify");

                entity.HasIndex(e => e.SpotId, "UQ__Spotify__330AF0F736CD0E7F")
                    .IsUnique();

                entity.HasIndex(e => e.SpotifyId, "UQ__Spotify__C253CFF105DA47E7")
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
                    .HasName("PK__Tracks__0D07174BF863E4B4");

                entity.HasIndex(e => e.TrackId, "UQ__Tracks__24ECC82F691DC9E9")
                    .IsUnique();

                entity.HasIndex(e => e.TrkId, "UQ__Tracks__FF49DBC88B389E4B")
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
                    .HasConstraintName("FK__Tracks__artist_i__52E34C9D");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        #endregion Methods

        #region Fields

        public static SpotterAzure_dbContext dbContext = new SpotterAzure_dbContext();

        #endregion Fields

        #region Constructors

        public SpotterAzure_dbContext()
        {
        }

        public SpotterAzure_dbContext(DbContextOptions<SpotterAzure_dbContext> options)
            : base(options)
        {
        }

        #endregion Constructors

        #region Properties

        public virtual DbSet<Artist> Artists { get; set; }
        public virtual DbSet<Listen> Listens { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Skip> Skips { get; set; }
        public virtual DbSet<Spotify> Spotifies { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }

        #endregion Properties
    }
}
