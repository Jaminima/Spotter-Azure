using SpotifyAPI.Web;
using Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Service.Actions
{
    public static class BetterShuffle
    {
        static Random rnd = new Random();

        public static async void OnNextSong(Spotify user, CurrentlyPlayingContext playing)
        {
            if (playing.Context == null)
            {
                Paging<SavedTrack> tracks = await user.spotify.Library.GetTracks();

                int i = rnd.Next(0, tracks.Total.Value);
                LibraryTracksRequest trackReq = new LibraryTracksRequest();
                trackReq.Offset = i;

                Paging<SavedTrack> targetTrack = await user.spotify.Library.GetTracks(trackReq);

                SavedTrack t = targetTrack.Items[0];
                await user.spotify.Player.AddToQueue(new PlayerAddToQueueRequest(t.Track.Uri));
            }
            else if (playing.Context.Type == "playlist")
            {
                FullPlaylist playlist = await user.spotify.Playlists.Get(playing.Context.Href.Split('/').Last());
                int i = rnd.Next(0, playlist.Tracks.Total.Value);

#warning not proper paging
                PlaylistTrack<IPlayableItem> t = playlist.Tracks.Items[i%playlist.Tracks.Limit.Value];
                await user.spotify.Player.AddToQueue(new PlayerAddToQueueRequest(((FullTrack)t.Track).Uri));
            }
        }
    }
}
