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

            public InsightData(Listen listen, Track track,Spotify sp) {
                this.listen = listen;
                this.track = track;
                this.track.GetFeatures(sp);
            }
        }

        public static async Task<InsightData[]> GetInsightDataAsync(Spotify sp)
        {
            IQueryable<Listen> listens = spotterdbContext.dbContext.Listens.Where(x => x.SpotId == sp.SpotId);
            Listen[] _listens = listens.ToArray();
            Track[] _tracks = listens.Select(x => x.Track).ToArray();

            List<InsightData> data = new List<InsightData>();

            for (int i=0;i<_listens.Length;i++)
            {
                data.Add(new InsightData(_listens[i], _tracks[i], sp));
            }

            return data.ToArray();
        }
    }
}
