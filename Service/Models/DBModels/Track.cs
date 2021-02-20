using System;
using System.Collections.Generic;

#nullable disable

namespace Service.Models.DBModels
{
    public partial class Track
    {
        public Track()
        {
            Listens = new HashSet<Models.Listen>();
            Skips = new HashSet<Models.Skip>();
        }

        public int TrkId { get; set; }
        public string TrackId { get; set; }
        public string Title { get; set; }
        public string Features { get; set; }
        public DateTime? TrueAt { get; set; }

        public virtual ICollection<Models.Listen> Listens { get; set; }
        public virtual ICollection<Models.Skip> Skips { get; set; }
    }
}
