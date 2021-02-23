using System;

#nullable disable

namespace Model.Models.DBModels
{
    public partial class Skip
    {
        #region Properties

        public DateTime? SkipAt { get; set; }
        public int SkipId { get; set; }
        public int SpotId { get; set; }
        public string TrackId { get; set; }
        public virtual Models.Spotify Spot { get; set; }
        public virtual Models.Track Track { get; set; }

        #endregion Properties
    }
}
