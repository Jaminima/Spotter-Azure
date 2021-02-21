﻿using SpotifyAPI.Web;
using Model.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Actions
{
    public class AutoSkipRemover
    {
        #region Methods

        public static async Task<Skip> Skipped(Spotify user, FullTrack track)
        {
            int recent = user.RecentSkips(track.Id);

            if (recent >= user.SkipThreshold - 1)
            {
                if (user.KickedTracks.Count(x => ((FullTrack)x.Track).Id == track.Id) == 0)
                {
                    await user.spotify.Playlists.AddItems(user.KickedPlaylist.Id, new PlaylistAddItemsRequest(new List<string>() { track.Uri }));

                    Track t = new Track(track, user);
                    user.KickedTracks.Add(new PlaylistTrack<IPlayableItem>());
                    user.KickedTracks.Last().Track = track;
                }

                List<bool> Exists = await user.spotify.Library.CheckTracks(new LibraryCheckTracksRequest(new List<string>() { track.Id }));
                if (Exists[0])
                {
                    await user.spotify.Library.RemoveTracks(new LibraryRemoveTracksRequest(new List<string>() { track.Id }));
                }
            }
            else
            {
            }
            return new Skip(track.Id, user);
        }

        #endregion Methods
    }
}
