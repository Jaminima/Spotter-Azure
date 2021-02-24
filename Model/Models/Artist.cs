using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Models
{
    public class ArtistDetails
    {
        public string[] genres;
        public int popularity;
        public string name;
    }

    public class Artist : DBModels.Artist
    {
        public Artist()
        {
        }

        public Artist(Track track, Spotify sp)
        {
            this.ArtistId = track.ArtistId;
            SetArtist(sp);
            this.TrueAt = DateTime.Now;
        }

        public ArtistDetails _artistDetails
        {
            get { return JObject.Parse(this.Details).ToObject<ArtistDetails>(); }
        }

        public async Task<ArtistDetails> GetArtist(Spotify sp)
        {
            if (DateTime.Now.AddDays(-7) > this.TrueAt.Value || this.Details == null)
            {
                try
                {
                    FullArtist features = await sp.spotify.Artists.Get(this.ArtistId);
                    this.Details = JObject.FromObject(features).ToString();
                }
                catch
                {
                    return null;
                }
                finally
                {
                    SpotterAzure_dbContext.dbContext.Artists.Update(this);
                }
            }

            return JObject.Parse(this.Details).ToObject<ArtistDetails>();
        }

        private async void SetArtist(Spotify sp)
        {
            if (DateTime.Now.AddDays(7) > this.TrueAt || this.Details == null)
                this.Details = JObject.FromObject(await sp.spotify.Artists.Get(this.ArtistId)).ToString();
        }
    }
}
