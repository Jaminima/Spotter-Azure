using System;
using System.Collections.Generic;

#nullable disable

namespace Model.Models.DBModels
{
    public partial class Track
    {
        #region Constructors

        public Track()
        {
            Listens = new HashSet<Models.Listen>();
            Skips = new HashSet<Models.Skip>();
        }

        #endregion Constructors

        #region Properties

        public string Features { get; set; }
        public virtual ICollection<Models.Listen> Listens { get; set; }
        public virtual ICollection<Models.Skip> Skips { get; set; }
        public string Title { get; set; }
        public string TrackId { get; set; }
        public int TrkId { get; set; }
        public DateTime? TrueAt { get; set; }

        #endregion Properties
    }
}
