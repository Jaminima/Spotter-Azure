using System;
using System.Collections.Generic;

#nullable disable

namespace Spotter_Azure.Models
{
    public partial class Listen : DBModels.Listen
    {
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
    }
}
