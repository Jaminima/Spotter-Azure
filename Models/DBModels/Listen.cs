using System;
using System.Collections.Generic;

#nullable disable

namespace Spotter_Azure.Models.DBModels
{
    public partial class Listen
    {
        public int ListenId { get; set; }
        public string TrackId { get; set; }
        public int SpotId { get; set; }
        public DateTime? ListenAt { get; set; }

        public virtual Models.Spotify Spot { get; set; }
    }
}
