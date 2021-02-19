using System;
using System.Collections.Generic;

#nullable disable

namespace Spotter_Azure.Models.DBModels
{
    public partial class Spotify
    {
        public Spotify()
        {
            Listens = new HashSet<Models.Listen>();
            Sessions = new HashSet<Models.Session>();
            Skips = new HashSet<Models.Skip>();
        }

        public int SpotId { get; set; }
        public string SpotifyId { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? AuthExpires { get; set; }
        public int? SkipThreshold { get; set; }

        public virtual ICollection<Models.Listen> Listens { get; set; }
        public virtual ICollection<Models.Session> Sessions { get; set; }
        public virtual ICollection<Models.Skip> Skips { get; set; }
    }
}
