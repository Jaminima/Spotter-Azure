using System;

#nullable disable

namespace Spotter_Azure.DBModels
{
    public partial class Skip
    {
        #region Constructors

        public Skip()
        {
        }

        public Skip(string trackId, Spotify sp)
        {
            this.SkipId = 0;
            this.SpotId = sp.SpotId;
            this.TrackId = trackId;
            this.SkipAt = DateTime.Now;
        }

        #endregion Constructors

        #region Properties

        public DateTime? SkipAt { get; set; }
        public int SkipId { get; set; }
        public virtual Spotify Spot { get; set; }
        public int SpotId { get; set; }
        public string TrackId { get; set; }

        #endregion Properties
    }
}
