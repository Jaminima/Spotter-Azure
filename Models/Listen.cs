using System;
using System.Collections.Generic;
using SpotifyAPI.Web;
using Newtonsoft.Json.Linq;

#nullable disable

namespace Spotter_Azure.Models
{
    

    public partial class Listen : DBModels.Listen
    {
        public Listen()
        {
        }

        public Listen(FullTrack track, Spotify sp)
        {
            this.ListenId = 0;
            this.SpotId = sp.SpotId;
            this.TrackId = track.Id;
            this.ListenAt = DateTime.Now;
        }

    }
}
