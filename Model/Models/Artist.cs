using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using System;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Artist : DBModels.Artist
    {
        #region Methods

        private async void SetArtist(Spotify sp)
        {
            if (DateTime.Now.AddDays(7) > this.TrueAt || this.Details == null)
                this.Details = JObject.FromObject(await sp.spotify.Artists.Get(this.ArtistId)).ToString();
        }

        #endregion Methods

        #region Constructors

        public Artist()
        {
        }

        public Artist(Track track, Spotify sp)
        {
            this.ArtistId = track.ArtistId;
            SetArtist(sp);
            this.TrueAt = DateTime.Now;
        }

        #endregion Constructors

        #region Properties

        public ArtistDetails _artistDetails
        {
            get { return JObject.Parse(this.Details).ToObject<ArtistDetails>(); }
        }

        #endregion Properties

        public async Task<ArtistDetails> GetArtist(Spotify sp)
        {
            if (DateTime.Now.AddDays(-7) > this.TrueAt.Value || this.Details == null)
            {
                try
                {
                    FullArtist features = await sp.spotify.Artists.Get(this.ArtistId);
                    this.Details = JObject.FromObject(features).ToString();
                    SpotterAzure_dbContext.dbContext.Update(this);
                }
                catch
                {
                    return null;
                }
            }

            return JObject.Parse(this.Details).ToObject<ArtistDetails>();
        }
    }

    public class ArtistDetails
    {
        #region Fields

        public string[] genres;
        public string name;
        public int popularity;

        #endregion Fields
    }
}
