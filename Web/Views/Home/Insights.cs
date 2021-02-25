using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spotter_Azure;
using Model.Models;
using Spotter_Azure.Models;
using Spotter_Azure.Actions;
using System.Linq;
using Spotter_Azure.Controllers;
using System;

namespace Spotter_Azure.Views.Home
{
    public static class Insights
    {
        public class InsightData
        {
            public Listen listen;
            public Track track;
            public Features features;
            public Artist artist;
            public int count = 1;

            public InsightData(Listen listen, Track track,Spotify sp) {
                this.listen = listen;
                this.track = track;
                
                Task<Features> f = this.track.GetFeatures(sp);
                Task<Artist> a = this.track.GetArtist(sp, SpotterAzure_dbContext.dbContext);

                if (!f.IsCompleted) { f.Start(); f.Wait(); }
                if (!a.IsCompleted) { a.Start(); a.Wait(); }

                this.features = f.Result;
                this.artist = a.Result;
            }
        }

        public static async Task<InsightData[]> GetInsightDataAsync(Spotify sp)
        {
            IQueryable<Listen> listens = SpotterAzure_dbContext.dbContext.Listens.Where(x => x.SpotId == sp.SpotId);
            Listen[] _listens = listens.ToArray();
            Track[] _tracks = listens.Select(x => x.Track).ToArray();

            Dictionary<int,InsightData> data = new Dictionary<int, InsightData>();

            for (int i=0;i< _tracks.Length;i++)
            {
                if (data.Keys.Contains(_tracks[i].TrkId))
                    data[_tracks[i].TrkId].count++;
                else
                    data.Add(_tracks[i].TrkId, new InsightData(_listens[i], _tracks[i], sp));
            }

            return data.Values.ToArray();
        }
    }
}
