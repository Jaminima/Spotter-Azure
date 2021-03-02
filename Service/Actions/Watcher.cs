using Model.Models;
using SpotifyAPI.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Service.Actions
{
    public static class Watcher
    {
        #region Fields

        private const float IsntSkip = 0.9f;
        private static SpotterAzure_dbContext dbContext;

        private static Dictionary<int, CurrentlyPlayingContext> lastTracks = new Dictionary<int, CurrentlyPlayingContext>();

        #endregion Fields

        #region Methods

        private static async void CheckUserEvent(Spotify user, SpotterAzure_dbContext dbContext)
        {
            CurrentlyPlayingContext playing = await user.spotify.Player.GetCurrentPlayback();

            if (playing != null)
            {
                if (!lastTracks.ContainsKey(user.SpotId))
                {
                    lastTracks.Add(user.SpotId, playing);
                    user.last = playing;
                    user.lastTrack = (FullTrack)user.last.Item;
                }
                else if (playing.Item != null)
                {
                    FullTrack track = (FullTrack)playing.Item;

                    user.last = lastTracks[user.SpotId];
                    user.lastTrack = (FullTrack)user.last.Item;

                    lastTracks[user.SpotId] = playing;

                    //Check if skipped
                    if (track.Id != user.lastTrack.Id)
                    {
                        Track t = new Track(track, user);
                        Artist a = await t.GetArtist(user, dbContext);
                        t.ArtistId = a.ArtistId;

                        t.Artist = a;
                        await t.GetFeatures(user,dbContext);

                        if (!dbContext.Artists.Any(x => x.ArtistId == t.ArtistId))
                        {
                            (await dbContext.Artists.AddAsync(a)).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }
                        else if (!dbContext.Tracks.Any(x => x.TrackId == t.TrackId))
                        {
                            (await dbContext.Tracks.AddAsync(t)).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }

                        if (dbContext.Tracks.Any(x => x.TrackId == track.Id))
                        {
                            t = dbContext.Tracks.Where(x => x.TrackId == track.Id).First();

                            if (t.Features == null) await t.GetFeatures(user,dbContext);
                            if (t.Artist == null) { t.ArtistId = a.ArtistId; await t.GetArtist(user, dbContext); }

                            dbContext.Tracks.Update(t);
                        }

                        if (user.last.ProgressMs < user.lastTrack.DurationMs * IsntSkip)
                        {
                            if (OnSkip != null)
                            {
                                Skip s = await OnSkip(user, user.lastTrack, playing, dbContext);
                                if (s != null)
                                {
                                    s.Track = t;
                                    await dbContext.Skips.AddAsync(s);
                                }
                            }
                        }
                        dbContext.Listens.Add(new Listen(track, user));
                        if (OnNextSong != null) OnNextSong(user, playing);
                    }


                    //Check if play state changed
                    if (playing.IsPlaying != user.last.IsPlaying)
                    {
                        if (playing.IsPlaying) 
                            if (OnResume != null) OnResume(user, playing);
                            else if (OnPause != null) OnPause(user, playing);
                    }
                }
            }

            await dbContext.SaveChangesAsync();
        }

        #endregion Methods

        public static Action<Spotify, CurrentlyPlayingContext> OnNextSong;

        public static EventHandler<CurrentlyPlayingContext> OnResume, OnPause;

        public static Func<Spotify, FullTrack, CurrentlyPlayingContext, SpotterAzure_dbContext, Task<Skip>> OnSkip;

        public static async void CheckEvents(SpotterAzure_dbContext dbContext)
        {
            foreach (Spotify s in dbContext.Spotifies.ToArray())
            {
                if (await s.IsAlive())
                {
                    s.SetupKicked();

                    Setting _sett = dbContext.Settings.First(x => x.SpotId == s.SpotId);
                    dbContext.Entry(_sett).Reload();
                    s.Setting = _sett;

                    CheckUserEvent(s,dbContext);
                }
            }
        }
    }
}
