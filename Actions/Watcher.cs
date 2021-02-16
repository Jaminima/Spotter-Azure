using SpotifyAPI.Web;
using System;
using System.Linq;
using System.Threading;
using Spotter_Azure.DBModels;

namespace Spotter_Azure.Actions
{
    public static class Watcher
    {
        #region Fields

        private const float IsntSkip = 0.9f;

        #endregion Fields

        #region Methods

        private static void CheckEvents()
        {
            while (true)
            {
                foreach (Spotify s in spotterdbContext.dbContext.Spotifies)
                {
                    CheckUserEvent(s);
                }
                Thread.Sleep(1000);
            }
        }

        private static async void CheckUserEvent(Spotify user)
        {
            CurrentlyPlayingContext playing = await user.spotify.Player.GetCurrentPlayback();

            if (playing != null)
            {
                if (user.lastTrack == null)
                {
                    user.last = playing;
                    user.lastTrack = (FullTrack)playing.Item;
                }
                else if (playing.Item != null)
                {
                    FullTrack track = (FullTrack)playing.Item;

                    //Check if skipped
                    if (track.Id != user.lastTrack.Id && user.last.ProgressMs < user.lastTrack.DurationMs * IsntSkip)
                    {
                        if (OnSkip!=null) OnSkip(user, user.lastTrack);
                    }

                    user.lastTrack = track;
                }

                //Check if play state changed
                if (playing.IsPlaying != user.last.IsPlaying)
                {
                    if (playing.IsPlaying) if (OnResume!=null) OnResume(user, playing);
                    else if (OnPause!=null) OnPause(user, playing);
                }

                user.last = playing;
            }
        }

        #endregion Methods

        public static EventHandler<CurrentlyPlayingContext> OnResume, OnPause;
        public static EventHandler<FullTrack> OnSkip;

        public static void Start()
        {
            new Thread(() => CheckEvents()).Start();
            Console.WriteLine("Running Spotify Watcher");
        }
    }
}