using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Spotter_Azure.Models.DBModels
{
    public partial class Spotify
    {
        public int SpotId { get; set; }
        public string SpotifyId { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? AuthExpires { get; set; }
        public int? SkipThreshold { get; set; }

        public virtual Models.Session Session { get; set; }
        public virtual ICollection<Models.Listen> Listens { get; set; }
        public virtual ICollection<Models.Skip> Skips { get; set; }
    }
}
