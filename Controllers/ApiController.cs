using Microsoft.AspNetCore.Mvc;
using Spotter_Azure.DBModels;
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
        public async void Post([FromQuery] string code)
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

                        Session s = f.Sessions.First();
                        s.AuthToken = authToken;
                        spotterdbContext.dbContext.Sessions.Update(s);
                    }
                    else
                    {
                        await spotterdbContext.dbContext.Spotifies.AddAsync(u);
                        Session s = new Session(authToken, u);
                        spotterdbContext.dbContext.Sessions.Add(s);
                    }

                    await spotterdbContext.dbContext.SaveChangesAsync();

                    Actions.Log.Add("User Signed Up", Actions.LogError.Info);

                    Response.Cookies.Append("authToken", authToken);

                    Response.StatusCode = 200;
                    Response.Redirect("/Insights",true);
                    return;
                }
            }
            Response.StatusCode = 405;
            return;
        }

        #endregion Methods
    }
}
