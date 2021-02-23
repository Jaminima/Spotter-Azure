using SpotifyAPI.Web;
using System;

#nullable disable

namespace Model.Models
{
    public partial class Listen : DBModels.Listen
    {
        #region Constructors

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

        #endregion Constructors
    }
}
