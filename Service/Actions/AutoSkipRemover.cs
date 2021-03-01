using Model.Models;
using SpotifyAPI.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Actions
{
    public class AutoSkipRemover
    {
        #region Methods

        public static async Task<Skip> Skipped(Spotify user, FullTrack track, CurrentlyPlayingContext playing, SpotterAzure_dbContext dbContext)
        {
            if (user.Setting.SkipOn.Value&&
                !(user.Setting.SkipIgnorePlaylist.Value && playing.Context != null && playing.Context.Type != "playlist"))
            {
                List<bool> Exists = await user.spotify.Library.CheckTracks(new LibraryCheckTracksRequest(new List<string>() { track.Id }));
                if (Exists[0] || !user.Setting.SkipMustBeLiked.Value)
                {
                    int recent = user.RecentSkips(track.Id, user.Setting.SkipExpiryHours.Value, dbContext);

                    if (recent >= user.Setting.SkipTrigger - 1)
                    {
                        if (user.KickedTracks.Count(x => ((FullTrack)x.Track).Id == track.Id) == 0)
                        {
                            await user.spotify.Playlists.AddItems(user.KickedPlaylist.Id, new PlaylistAddItemsRequest(new List<string>() { track.Uri }));

                            Track t = new Track(track, user);
                            user.KickedTracks.Add(new PlaylistTrack<IPlayableItem>());
                            user.KickedTracks.Last().Track = track;
                        }

                        if (Exists[0]) await user.spotify.Library.RemoveTracks(new LibraryRemoveTracksRequest(new List<string>() { track.Id }));

                        if (playing.Context.Type == "playlist" && user.Setting.SkipRemoveFromPlaylist.Value)
                        {
                            PlaylistRemoveItemsRequest removeReq = new PlaylistRemoveItemsRequest();

                            PlaylistRemoveItemsRequest.Item removeSongReq = new PlaylistRemoveItemsRequest.Item();
                            removeSongReq.Uri = track.Uri;
                            removeReq.Tracks = new List<PlaylistRemoveItemsRequest.Item>() { removeSongReq };

                            await user.spotify.Playlists.RemoveItems(playing.Context.Href.Split('/').Last(), removeReq);
                        }
                    }
                    else
                    {
                    }
                }
            }
            return new Skip(track.Id, user);
        }

        #endregion Methods
    }
}
