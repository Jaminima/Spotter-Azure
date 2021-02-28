using System;

#nullable disable

namespace Model.Models.DBModels
{
    public partial class Listen
    {
        #region Properties

        public DateTime? ListenAt { get; set; }
        public int ListenId { get; set; }
        public int SpotId { get; set; }
        public string TrackId { get; set; }
        public virtual Models.Spotify Spot { get; set; }
        public virtual Models.Track Track { get; set; }

        #endregion Properties
    }
}
