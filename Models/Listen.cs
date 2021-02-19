using System;
using System.Collections.Generic;
using SpotifyAPI.Web;
using Newtonsoft.Json.Linq;

#nullable disable

namespace Spotter_Azure.Models
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

    public partial class Listen : DBModels.Listen
    {
        public Features _features
        {
            get { return JObject.Parse(this.Features).ToObject<Features>(); }
        }

        public Listen()
        {
        }

        public Listen(FullTrack track, Spotify sp)
        {
            this.ListenId = 0;
            this.SpotId = sp.SpotId;
            this.TrackId = track.Id;
            this.ListenAt = DateTime.Now;
            SetFeatures(sp);
        }

        public async void SetFeatures(Spotify sp)
        {
            this.Features = JObject.FromObject(await sp.spotify.Tracks.GetAudioFeatures(TrackId)).ToString();
        }
    }
}
