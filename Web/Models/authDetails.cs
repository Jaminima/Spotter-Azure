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

        public async Task<bool> IsValid(Model.Models.SpotterAzure_dbContext dbContext)
        {
            if (authToken == null || spotid == null) return false;

            if (dbContext.Sessions.Any())
            {
                IQueryable<Model.Models.Session> sess = dbContext.Sessions.Where(x => x.SpotId.ToString() == spotid);
                if (sess.Any())
                {
                    return sess.First().AuthTokenMatches(authToken)/* && await sess.First().Spot.IsAlive()*/;
                }
            }
            return false;
        }

        #endregion Methods
    }
}
