#nullable disable

namespace Model.Models.DBModels
{
    public partial class Session
    {
        #region Properties

        public string AuthToken { get; set; }
        public int SessId { get; set; }
        public int SpotId { get; set; }
        public virtual Models.Spotify Spot { get; set; }

        #endregion Properties
    }
}
