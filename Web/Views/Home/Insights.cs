using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spotter_Azure.Views.Home
{
    public static class Insights
    {
        #region Methods

        public static async Task<InsightData[]> GetInsightDataAsync(Spotify sp)
        {
            IQueryable<Listen> listens = SpotterAzure_dbContext.dbContext.Listens.Where(x => x.SpotId == sp.SpotId).OrderByDescending(x => x.ListenAt).Take(100);
            Listen[] _listens = listens.ToArray();
            Track[] _tracks = listens.Select(x => x.Track).ToArray();

            Dictionary<int, InsightData> data = new Dictionary<int, InsightData>();

            for (int i = 0; i < _tracks.Length; i++)
            {
                if (data.Keys.Contains(_tracks[i].TrkId))
                {
                    if (_listens[i].ListenAt > data[_tracks[i].TrkId].listen.ListenAt)
                    {
                        data[_tracks[i].TrkId].listen.ListenAt = _listens[i].ListenAt;
                    }
                    data[_tracks[i].TrkId].count++;
                }
                else
                    data.Add(_tracks[i].TrkId, new InsightData(_listens[i], _tracks[i], sp));
            }

            return data.Values.ToArray();
        }

        #endregion Methods

        #region Classes

        public class InsightData
        {
            #region Fields

            public Artist artist;
            public int count = 1;
            public Features features;
            public Listen listen;
            public Track track;
            public string[] genres;

            #endregion Fields

            #region Constructors

            public InsightData(Listen listen, Track track, Spotify sp)
            {
                this.listen = listen;
                this.track = track;

                Task<Features> f = this.track.GetFeatures(sp);
                Task<Artist> a = this.track.GetArtist(sp, SpotterAzure_dbContext.dbContext);

                if (!f.IsCompleted) { f.Start(); f.Wait(); }
                if (!a.IsCompleted) { a.Start(); a.Wait(); }

                this.features = f.Result;
                this.artist = a.Result;

                this.genres = this.artist._artistDetails.genres.SelectMany(x=>x.Split(' ')).Distinct().ToArray();
            }

            #endregion Constructors
        }

        #endregion Classes
    }
}
