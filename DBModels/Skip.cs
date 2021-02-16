using System;
using System.Collections.Generic;

#nullable disable

namespace Spotter_Azure.DBModels
{
    public partial class Skip
    {
        public int SkipId { get; set; }
        public string TrackId { get; set; }
        public int SpotId { get; set; }
        public DateTime? SkipAt { get; set; }

        public virtual Spotify Spot { get; set; }

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
    }
}
