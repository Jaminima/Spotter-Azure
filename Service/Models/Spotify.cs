using SpotifyAPI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable disable

namespace Service.Models
{
    public partial class Spotify : DBModels.Spotify
    {
        private SpotifyClient _spotify;

        #region Methods

        private async void SetUser()
        {
            PrivateUser u = await spotify.UserProfile.Current();
            this.SpotifyId = u.Id;
        }

        #endregion Methods

        public SimplePlaylist KickedPlaylist;

        public List<PlaylistTrack<IPlayableItem>> KickedTracks;

        public CurrentlyPlayingContext last = null;

        public FullTrack lastTrack = null;
        public Spotify() { 
        }

        public Spotify(string authtoken, string refreshtoken, DateTime authExpires)
        {
            this.AuthToken = authtoken;
            this.RefreshToken = refreshtoken;
            this.AuthExpires = authExpires;
            _spotify = new SpotifyClient(this.AuthToken);
            SetUser();
        }

        public SpotifyClient spotify
        {
            get { if (DateTime.Now > AuthExpires || _spotify == null) _spotify = new SpotifyClient(GetAuthToken()); return _spotify; }
        }

        public string GetAuthToken()
        {
            if (DateTime.Now > AuthExpires || AuthToken == null)
            {
                Controllers.AuthFlow.Refresh(this);
            }
            return AuthToken;
        }

        public async Task<PrivateUser> GetUser()
        {
            return await spotify.UserProfile.Current();
        }

        public async Task<bool> IsAlive()
        {
            try
            {
                SpotifyClient s = spotify;
                FullTrack t = await s.Tracks.Get("3Hvu1pq89D4R0lyPBoujSv");
                return t != null;
            }
            catch
            {
                return false;
            }
        }

        public int RecentSkips(string trackid)
        {
            DateTime After = DateTime.Now.AddDays(-7);
            return spotterdbContext.dbContext.Skips.Count(x => x.TrackId == trackid && x.SpotId == SpotId && x.SkipAt > After);
        }

        public async void SetupKicked()
        {
            string KickedName = $"Kicked Out {DateTime.Now.Year}";

            Paging<SimplePlaylist> playlists = await spotify.Playlists.CurrentUsers();

            KickedPlaylist = playlists.Items.Find(x => x.Name == KickedName);

            if (KickedPlaylist == null)
            {
                FullPlaylist playlist = await spotify.Playlists.Create((await spotify.UserProfile.Current()).Id, new PlaylistCreateRequest(KickedName));
                SetupKicked();
            }
            else KickedTracks = (await spotify.Playlists.GetItems(KickedPlaylist.Id)).Items;
        }
    }
}
