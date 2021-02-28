using Model.Models;
using SpotifyAPI.Web;
using System;
using System.Linq;

namespace Service.Actions
{
    public static class BetterShuffle
    {
        #region Fields

        private static Random rnd = new Random();

        #endregion Fields

        #region Methods

        public static async void OnNextSong(Spotify user, CurrentlyPlayingContext playing)
        {
            if (!user.Setting.ShuffleOn.Value) return;

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
            else if (playing.Context.Type == "playlist" && user.Setting.ShufflePlaylists.Value)
            {
                string id = playing.Context.Href.Split('/').Last();
                FullPlaylist playlist = await user.spotify.Playlists.Get(id);
                int i = rnd.Next(0, playlist.Tracks.Total.Value);

                PlaylistGetItemsRequest playlistGet = new PlaylistGetItemsRequest();
                playlistGet.Offset = i;

                Paging<PlaylistTrack<IPlayableItem>> targetTrack = await user.spotify.Playlists.GetItems(id, playlistGet);

                PlaylistTrack<IPlayableItem> t = targetTrack.Items[0];
                await user.spotify.Player.AddToQueue(new PlayerAddToQueueRequest(((FullTrack)t.Track).Uri));
            }
            else if (playing.Context.Type == "album" && user.Setting.ShuffleAlbums.Value)
            {
#warning Not Implemented

            }
        }

        #endregion Methods
    }
}
