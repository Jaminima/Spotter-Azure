using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Spotter_Azure.DBModels;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Spotter_Azure.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        public String Default()
        {
            return "Hgmmm";
        }

        // POST api/<ValuesController>
        [HttpGet("register")]
        public void Post([FromQuery] string code)
        {
            if (code != null)
            {
                Spotify u = AuthFlow.FromCode(code);
                if (u != null)
                {
                    while (u.SpotifyId == null) { }
                    IQueryable<Spotify> spot = spotterdbContext.dbContext.Spotifies.Where(x => x.SpotifyId == u.SpotifyId).Select(x=>x);
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
                        spotterdbContext.dbContext.Spotifies.Add(u);
                    }

                    spotterdbContext.dbContext.SaveChanges();

                    Response.StatusCode = 200;
                    Response.Redirect("/");
                    return;
                }
            }
            Response.StatusCode = 405;
            return;
        }
    }
}
