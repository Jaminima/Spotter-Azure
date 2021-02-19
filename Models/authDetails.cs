using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Spotter_Azure.Models
{
    public class authDetails
    {
        public string authToken, spotid;

        public authDetails(HttpRequest request)
        {
            authToken = request.Cookies["authToken"];
            spotid = request.Cookies["spotid"];
        }

        public async Task<bool> IsValid(spotterdbContext dbContext)
        {
            if (authToken == null || spotid == null) return false;

            if (dbContext.Sessions.Any())
            {
                IQueryable<Session> sess = dbContext.Sessions.Where(x => x.SpotId.ToString() == spotid);
                if (sess.Any())
                {
                    return sess.First().AuthTokenMatches(authToken)/* && await sess.First().Spot.IsAlive()*/;
                }
            }
            return false;
        }
    }
}
