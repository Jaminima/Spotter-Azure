using SpotifyAPI.Web;
using Model.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Service.Actions
{
    public static class Watcher
    {
        #region Fields

        private const float IsntSkip = 0.9f;
        private static spotterdbContext dbContext = new spotterdbContext();

        #endregion Fields

        #region Methods

        public static async void CheckEvents()
        {
            foreach (Spotify s in dbContext.Spotifies.ToArray())
            {
                if (await s.IsAlive())
                {
                    s.SetupKicked();
                    CheckUserEvent(s);
                }
            }
            
        }

        private static async void CheckUserEvent(Spotify user)
        {
            spotterdbContext dbContext = new spotterdbContext();
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
                    if (track.Id != user.lastTrack.Id)
                    {
                        Track t = new Track(track, user);
                        if (dbContext.Tracks.Count(x => x.TrackId == track.Id) == 0)
                        {
                            await dbContext.Tracks.AddAsync(t, CancellationToken.None);
                            await dbContext.SaveChangesAsync();
                        }
                        else
                        {
                            t = dbContext.Tracks.Where(x => x.TrackId == track.Id).First();
                        }

                        if (user.last.ProgressMs < user.lastTrack.DurationMs * IsntSkip)
                        {
                            if (OnSkip != null)
                            {
                                Skip s = await OnSkip(user, user.lastTrack);
                                s.Track = t;
                                await dbContext.Skips.AddAsync(s);
                            }
                        }
                        dbContext.Listens.Add(new Listen(track, user));
                        if (OnNextSong != null) OnNextSong(user,playing);
                    }

                    user.lastTrack = track;
                }

                //Check if play state changed
                if (playing.IsPlaying != user.last.IsPlaying)
                {
                    if (playing.IsPlaying) if (OnResume != null) OnResume(user, playing);
                        else if (OnPause != null) OnPause(user, playing);
                }

                user.last = playing;
            }

            await dbContext.SaveChangesAsync();
        }

        #endregion Methods

        public static EventHandler<CurrentlyPlayingContext> OnResume, OnPause;
        public static Func<Spotify, FullTrack, Task<Skip>> OnSkip;
        public static Action<Spotify, CurrentlyPlayingContext> OnNextSong;
    }
}
