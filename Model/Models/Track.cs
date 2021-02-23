using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;

namespace Model.Models
{
    public class Features
    {
        public float danceability, energy, loudness, speechiness, acousticness, instrumentalness, liveness, valence, tempo;

        public float getTotal()
        {
            return danceability + energy + speechiness + acousticness + instrumentalness + liveness + valence;
        }

        public int key, mode, duration_ms, time_signature;
        public string type, id, uri, track_href, analysis_url;

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
    }

    public class Track : DBModels.Track
    {
        public Track()
        {
        }

        public Track(FullTrack track, Spotify sp)
        {
            this.TrackId = track.Id;
            this.TrueAt = DateTime.Now;
            this.Title = track.Name;

            SetFeatures(sp);
        }

        public Features _features
        {
            get { return JObject.Parse(this.Features).ToObject<Features>(); }
        }

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
                    spotterdbContext.dbContext.Tracks.Update(this);
                }
            }

            return JObject.Parse(this.Features).ToObject<Features>();
        }

        public async void SetFeatures(Spotify sp)
        {
            if (DateTime.Now.AddDays(7) > this.TrueAt || this.Features == null) 
                this.Features = JObject.FromObject(await sp.spotify.Tracks.GetAudioFeatures(TrackId)).ToString();
        }
    }
}
