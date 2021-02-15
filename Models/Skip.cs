using System;

namespace Spotter_Azure.Models
{
    public class Skip
    {
        #region Fields

        public string trackId;
        public DateTime when;

        #endregion Fields

        #region Constructors

        public Skip(string trackId)
        {
            this.trackId = trackId;
            this.when = DateTime.Now;
        }

        #endregion Constructors
    }
}
