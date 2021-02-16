using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using SpotifyAPI.Web;
using System.Threading.Tasks;

#nullable disable

namespace Spotter_Azure.DBModels
{
    public partial class Spotify
    {
        public Spotify()
        {
            Listens = new HashSet<Listen>();
            Skips = new HashSet<Skip>();
        }

        public int SpotId { get; set; }
        public string SpotifyId { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? AuthExpires { get; set; }
        public int? SkipThreshold { get; set; }

        public virtual ICollection<Listen> Listens { get; set; }
        public virtual ICollection<Skip> Skips { get; set; }

        private SpotifyClient _spotify;

        public SpotifyClient spotify
        {
            get { if (DateTime.Now > AuthExpires || _spotify == null) _spotify = new SpotifyClient(GetAuthToken()); return _spotify; }
        }

        public string GetAuthToken()
        {
            if (DateTime.Now > AuthExpires || AuthToken == null) Controllers.AuthFlow.Refresh(this); return AuthToken; 
        }

        public SimplePlaylist KickedPlaylist;

        public List<PlaylistTrack<IPlayableItem>> KickedTracks;

        public CurrentlyPlayingContext last = null;

        public FullTrack lastTrack = null;

        public Spotify(string authtoken, string refreshtoken, DateTime authExpires)
        {
            this.AuthToken = authtoken;
            this.RefreshToken = refreshtoken;
            this.AuthExpires = authExpires;
            _spotify = new SpotifyClient(this.AuthToken);
            SetUser();
            SetupKicked();
        }

        private async void SetupKicked()
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

        public async Task<PrivateUser> GetUser()
        {
            return await spotify.UserProfile.Current();
        }

        private async void SetUser()
        {
            this.SpotifyId = (await spotify.UserProfile.Current()).Id;
        }

        public int RecentSkips(string trackid)
        {
            DateTime After = DateTime.Now.AddDays(-7);
            return Skips.Count(x=>x.TrackId == trackid && x.SkipAt>After);
        }
    }
}
