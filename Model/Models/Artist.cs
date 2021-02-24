using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Artist : DBModels.Artist
    {
        public Artist()
        {
        }

        public Artist(FullTrack track, Spotify sp)
        {
            SimpleArtist artist = track.Artists.First();

            this.ArtistId = artist.Id;
            this.ArtistName = artist.Name;
            SetArtist(sp);

            this.TrueAt = DateTime.Now;
        }

        public async Task<Features> GetArtist(Spotify sp)
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

            return JObject.Parse(this.Details).ToObject<Features>();
        }

        private async void SetArtist(Spotify sp)
        {
            if (DateTime.Now.AddDays(7) > this.TrueAt || this.Details == null)
                this.Details = JObject.FromObject(await sp.spotify.Artists.Get(this.ArtistId)).ToString();
        }
    }
}
