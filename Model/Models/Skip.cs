using System;

#nullable disable

namespace Model.Models
{
    public partial class Skip : DBModels.Skip
    {
        #region Constructors

        public Skip()
        {
        }

        public Skip(string trackId, Spotify sp)
        {
            this.SkipId = 0;
            this.SpotId = sp.SpotId;
            this.TrackId = trackId;
            this.SkipAt = DateTime.Now;
        }

        #endregion Constructors
    }
}
