using Microsoft.AspNetCore.Mvc;
using Spotter_Azure.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

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

                    IQueryable<Spotify> spot = spotterdbContext.dbContext.Spotifies.Where(x => x.SpotifyId == u.SpotifyId).Select(x => x);
                    if (spot.Any())
                    {
                        Spotify f = spot.First();
                        f.AuthExpires = u.AuthExpires;
                        f.AuthToken = u.AuthToken;
                        f.RefreshToken = u.RefreshToken;
                        spotterdbContext.dbContext.Spotifies.Update(f);
                    }
                    else
                    {
                        await spotterdbContext.dbContext.Spotifies.AddAsync(u);
                    }

                    await spotterdbContext.dbContext.SaveChangesAsync();

                    if (!spot.Any() || !spot.First().Sessions.Any())
                    {
                        Session s = new Session(authToken, spot.First());
                        await spotterdbContext.dbContext.Sessions.AddAsync(s);
                    }
                    else
                    {
                        Session s = spot.First().Sessions.First();
                        s.AuthToken = authToken;
                        spotterdbContext.dbContext.Sessions.Update(s);
                    }

                    await spotterdbContext.dbContext.SaveChangesAsync();

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
