using SpotifyAPI.Web;
using Spotter_Azure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Spotter_Azure.Actions
{
    public static class BetterShuffle
    {
        static Random rnd = new Random();

        public static async void OnNextSong(Spotify user, CurrentlyPlayingContext playing)
        {
            if (playing.Context.Type == "playlist")
            {
                FullPlaylist playlist = await user.spotify.Playlists.Get(playing.Context.Href.Split('/').Last());
                int i = rnd.Next(0, playlist.Tracks.Total.Value);
                PlaylistTrack<IPlayableItem> t = playlist.Tracks.Items[i];
                await user.spotify.Player.AddToQueue(new PlayerAddToQueueRequest(((FullTrack)t.Track).Uri));
            }
        }
    }
}
