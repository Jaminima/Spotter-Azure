using System;
using System.Collections.Generic;

#nullable disable

namespace Model.Models.DBModels
{
    public partial class Artist
    {
        #region Constructors

        public Artist()
        {
            Tracks = new HashSet<Models.Track>();
        }

        #endregion Constructors

        #region Properties

        public int ArtId { get; set; }
        public string ArtistId { get; set; }
        public string Details { get; set; }
        public virtual ICollection<Models.Track> Tracks { get; set; }
        public DateTime? TrueAt { get; set; }

        #endregion Properties
    }
}
