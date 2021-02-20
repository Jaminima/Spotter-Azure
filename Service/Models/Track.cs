using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;

namespace Service.Models
{
    public struct Features
    {
        public float danceability, energy, loudness, speechiness, acousticness, instrumentalness, liveness, valence, tempo;

        public float getTotal()
        {
            return danceability + energy + speechiness + acousticness + instrumentalness + liveness + valence;
        }

        public int key, mode, duration_ms, time_signature;
        public string type, id, uri, track_href, analysis_url;
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

        public async void SetFeatures(Spotify sp)
        {
            if (DateTime.Now.AddDays(7) > this.TrueAt) 
                this.Features = JObject.FromObject(await sp.spotify.Tracks.GetAudioFeatures(TrackId)).ToString();
        }
    }
}
