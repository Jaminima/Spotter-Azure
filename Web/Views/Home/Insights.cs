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

        private static string[] genres = new SpotterAzure_dbContext().Artists.Where(x=>x.Details!=null).ToArray().SelectMany(x=>x._artistDetails.genres.SelectMany(y=>y.Split(' ', StringSplitOptions.None))).Distinct().ToArray();

        public static string GetColor(string genre)
        {
            int i = 0;
            for (; i < genres.Length; i++) if (genres[i].Contains(genre)) break;

            float p = (float)i / genres.Length;

            int r = 0, g = 0, b=0;

            if (p < 1 / 3.0f) r = (int) ( 255 * p * 2);
            else if (p < 2 / 3.0f) g = (int)(255 * (p-0.33f) * 2);
            else b = (int)(255 * (p-0.66f) * 2);

            r += 85;
            g += 85;
            b += 85;

            r %= 255;
            g %= 255;
            b %= 255;

            return String.Format("{0:X2}{1:X2}{2:X2}",r,g,b);
        }

        public static async Task<InsightData[]> GetInsightDataAsync(Spotify sp, SpotterAzure_dbContext dbContext)
        {
            IQueryable<Listen> listens = dbContext.Listens.Where(x => x.SpotId == sp.SpotId).OrderByDescending(x => x.ListenAt).Take(100);
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
                    data.Add(_tracks[i].TrkId, new InsightData(_listens[i], _tracks[i], sp, dbContext));
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

            public InsightData(Listen listen, Track track, Spotify sp, SpotterAzure_dbContext dbContext)
            {
                this.listen = listen;
                this.track = track;

                Task<Features> f = this.track.GetFeatures(sp,dbContext);
                Task<Artist> a = this.track.GetArtist(sp, dbContext);

                if (!f.IsCompleted) { f.Start(); f.Wait(); }
                if (!a.IsCompleted) { a.Start(); a.Wait(); }

                this.features = f.Result;
                this.artist = a.Result;

                if (this.artist.Details!=null)
                    this.genres = this.artist._artistDetails.genres.SelectMany(x=>x.Split(' ')).Distinct().ToArray();
            }

            #endregion Constructors
        }

        #endregion Classes
    }
}
