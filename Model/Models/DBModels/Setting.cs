#nullable disable

namespace Model.Models.DBModels
{
    public partial class Setting
    {
        #region Properties

        public int SettId { get; set; }
        public bool? ShuffleAlbums { get; set; }
        public bool? ShuffleOn { get; set; }
        public bool? ShufflePlaylists { get; set; }
        public int? SkipExpiryHours { get; set; }
        public bool? SkipIgnorePlaylist { get; set; }
        public bool? SkipOn { get; set; }
        public bool? SkipRemoveFromPlaylist { get; set; }
        public int? SkipTrigger { get; set; }
        public int SpotId { get; set; }
        public virtual Models.Spotify Spot { get; set; }

        #endregion Properties
    }
}
