﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Model.Models.DBModels
{
    public partial class Spotify
    {
        public Spotify()
        {
            Listens = new HashSet<Models.Listen>();
            Skips = new HashSet<Models.Skip>();
        }

        public int SpotId { get; set; }
        public string SpotifyId { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? AuthExpires { get; set; }

        public virtual Models.Session Session { get; set; }
        public virtual Models.Setting Setting { get; set; }
        public virtual ICollection<Models.Listen> Listens { get; set; }
        public virtual ICollection<Models.Skip> Skips { get; set; }
    }
}
