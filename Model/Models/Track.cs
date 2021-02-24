using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Models
{
    public class Features
    {
        #region Fields

        public float danceability, energy, loudness, speechiness, acousticness, instrumentalness, liveness, valence, tempo;

        public int key, mode, duration_ms, time_signature;

        public string type, id, uri, track_href, analysis_url;

        #endregion Fields

        #region Methods

        public static Features operator +(Features a, Features b)
        {
            Features c = new Features();

            c.danceability = a.danceability + b.danceability;
            c.energy = a.energy + b.energy;
            c.loudness = a.loudness + b.loudness;
            c.speechiness = a.speechiness + b.speechiness;
            c.acousticness = a.acousticness + b.acousticness;
            c.instrumentalness = a.instrumentalness + b.instrumentalness;
            c.liveness = a.liveness + b.liveness;
            c.valence = a.valence + b.valence;
            c.tempo = a.tempo + b.tempo;

            c.duration_ms = a.duration_ms + b.duration_ms;

            return c;
        }
        
        public static Features operator *(Features a, float b)
        {
            Features c = new Features();

            c.danceability = a.danceability * b;
            c.energy = a.energy * b;
            c.loudness = a.loudness * b;
            c.speechiness = a.speechiness * b;
            c.acousticness = a.acousticness * b;
            c.instrumentalness = a.instrumentalness * b;
            c.liveness = a.liveness * b;
            c.valence = a.valence * b;
            c.tempo = a.tempo * b;

            return c;
        }

        public static Features operator /(Features a, float b)
        {
            Features c = new Features();

            c.danceability = a.danceability / b;
            c.energy = a.energy / b;
            c.loudness = a.loudness / b;
            c.speechiness = a.speechiness / b;
            c.acousticness = a.acousticness / b;
            c.instrumentalness = a.instrumentalness / b;
            c.liveness = a.liveness / b;
            c.valence = a.valence / b;
            c.tempo = a.tempo / b;

            return c;
        }

        public float getTotal()
        {
            return danceability + energy + speechiness + acousticness + instrumentalness + liveness + valence;
        }

        #endregion Methods
    }

    public class Track : DBModels.Track
    {
        #region Constructors

        public Track()
        {
        }

        public Track(FullTrack track, Spotify sp)
        {
            this.TrackId = track.Id;
            this.TrueAt = DateTime.Now;
            this.Title = track.Name;
            this.ArtistId = track.Artists.First().Id;

            SetFeatures(sp);

            IQueryable<Artist> artists = SpotterAzure_dbContext.dbContext.Artists.Where(x => x.ArtistId == this.ArtistId);
            if (artists.Any())
            {

            }
            else
            {
                Artist art = new Artist(track, sp);
                SpotterAzure_dbContext.dbContext.Artists.AddAsync(art);
                SpotterAzure_dbContext.dbContext.SaveChanges();
            }
        }

        #endregion Constructors

        #region Properties

        public Features _features
        {
            get { return JObject.Parse(this.Features).ToObject<Features>(); }
        }

        #endregion Properties

        #region Methods

        public async Task<Features> GetFeatures(Spotify sp)
        {
            if (DateTime.Now.AddDays(-7) > this.TrueAt.Value || this.Features == null)
            {
                try
                {
                    TrackAudioFeatures features = await sp.spotify.Tracks.GetAudioFeatures(TrackId);
                    this.Features = JObject.FromObject(features).ToString();
                }
                catch
                {
                    return null;
                }
                finally
                {
                    SpotterAzure_dbContext.dbContext.Tracks.Update(this);
                }
            }

            return JObject.Parse(this.Features).ToObject<Features>();
        }

        public async void SetFeatures(Spotify sp)
        {
            if (DateTime.Now.AddDays(7) > this.TrueAt || this.Features == null)
                this.Features = JObject.FromObject(await sp.spotify.Tracks.GetAudioFeatures(TrackId)).ToString();
        }

        #endregion Methods
    }
}
