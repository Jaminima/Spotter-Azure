using System;
using Scrypt;
using System.Collections.Generic;

#nullable disable

namespace Spotter_Azure.Models.DBModels
{
    public partial class Session
    {
        public int SpotId { get; set; }
        public string AuthToken { get; set; }
        public int SessId { get; set; }
        public virtual Models.Spotify Spot { get; set; }
    }
}
