using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models.DBModels
{
    public partial class Session
    {
        public int SessId { get; set; }
        public int SpotId { get; set; }
        public string AuthToken { get; set; }

        public virtual Models.Spotify Spot { get; set; }
    }
}
