using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spotter_Azure.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        #region Methods

        [HttpGet]
        public String Default()
        {
            return "Hgmmm";
        }

        // POST api/<ValuesController>
        [HttpGet("register")]
        public async Task<IActionResult> Post([FromQuery] string code)
        {
            if (code != null)
            {
                Spotify u = AuthFlow.FromCode(code);
                if (u != null)
                {
                    while (u.SpotifyId == "" || u.SpotifyId == null) { }

                    string authToken = Session.GetAuthToken();

                    IQueryable<Spotify> spot = SpotterAzure_dbContext.dbContext.Spotifies.Where(x => x.SpotifyId == u.SpotifyId).Select(x => x);
                    if (spot.Any())
                    {
                        Spotify f = spot.First();
                        f.AuthExpires = u.AuthExpires;
                        f.AuthToken = u.AuthToken;
                        f.RefreshToken = u.RefreshToken;
                        SpotterAzure_dbContext.dbContext.Spotifies.Update(f);
                    }
                    else
                    {
                        await SpotterAzure_dbContext.dbContext.Spotifies.AddAsync(u);
                    }

                    await SpotterAzure_dbContext.dbContext.SaveChangesAsync();

                    IQueryable<Session> sess = SpotterAzure_dbContext.dbContext.Sessions.Where(x => x.SpotId == spot.First().SpotId);

                    if (!sess.Any())
                    {
                        Session s = new Session(authToken, spot.First());
                        await SpotterAzure_dbContext.dbContext.Sessions.AddAsync(s);
                    }
                    else
                    {
                        Session s = sess.First();
                        s.SetAuthToken(authToken);
                        SpotterAzure_dbContext.dbContext.Sessions.Update(s);
                    }

                    await SpotterAzure_dbContext.dbContext.SaveChangesAsync();

                    Actions.Log.Add("User Signed Up", Actions.LogError.Info);

                    HttpContext.Response.Cookies.Append("spotid", spot.First().SpotId.ToString());

                    HttpContext.Response.Cookies.Append("authToken", authToken);
                    return RedirectPermanent("/Insights");
                }
            }
            return BadRequest();
        }

        #endregion Methods
    }
}
