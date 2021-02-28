using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Spotter_Azure.Models
{
    public class authDetails
    {
        #region Fields

        public string authToken, spotid;

        #endregion Fields

        #region Constructors

        public authDetails(HttpRequest request)
        {
            authToken = request.Cookies["authToken"];
            spotid = request.Cookies["spotid"];
        }

        #endregion Constructors

        #region Methods

        public static Model.Models.Spotify CheckAuth(HttpRequest request, Model.Models.SpotterAzure_dbContext dbContext)
        {
            authDetails details = new authDetails(request);

            if (details.authToken == null || details.spotid == null) return null;

            IQueryable<Model.Models.Session> sess = dbContext.Sessions.Where(x => x.SpotId.ToString() == details.spotid);

            if (sess.Any() && sess.First().AuthTokenMatches(details.authToken))
            {
                return dbContext.Spotifies.First(x => x.SpotId == sess.First().SpotId);
            }
            return null;
        }

        public async Task<bool> IsValid(Model.Models.SpotterAzure_dbContext dbContext)
        {
            if (authToken == null || spotid == null) return false;

            IQueryable<Model.Models.Session> sess = dbContext.Sessions.Where(x => x.SpotId.ToString() == spotid);

            if (sess.Any())
            {
                return sess.First().AuthTokenMatches(authToken)/* && await sess.First().Spot.IsAlive()*/;
            }
            return false;
        }

        #endregion Methods
    }
}
