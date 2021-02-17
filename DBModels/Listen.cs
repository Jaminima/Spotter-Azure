using System;

#nullable disable

namespace Spotter_Azure.DBModels
{
    public partial class Listen
    {
        #region Constructors

        public Listen()
        {
        }

        public Listen(string trackId, Spotify sp)
        {
            this.ListenId = 0;
            this.SpotId = sp.SpotId;
            this.TrackId = trackId;
            this.ListenAt = DateTime.Now;
        }

        #endregion Constructors

        #region Properties

        public DateTime? ListenAt { get; set; }
        public int ListenId { get; set; }
        public virtual Spotify Spot { get; set; }
        public int SpotId { get; set; }
        public string TrackId { get; set; }

        #endregion Properties
    }
}
