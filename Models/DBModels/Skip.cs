using System;
using System.Collections.Generic;

#nullable disable

namespace Spotter_Azure.Models.DBModels
{
    public partial class Skip
    {
        public int SkipId { get; set; }
        public string TrackId { get; set; }
        public int SpotId { get; set; }
        public DateTime? SkipAt { get; set; }

        public virtual Models.Spotify Spot { get; set; }
    }
}
