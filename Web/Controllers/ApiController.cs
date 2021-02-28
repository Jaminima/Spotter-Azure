﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Spotter_Azure.Models;

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

        [HttpPost("update")]
        public IActionResult UpdateUser([FromForm]IFormCollection formDetails)
        {
            SpotterAzure_dbContext dbContext = new SpotterAzure_dbContext();

            Spotify sp = authDetails.CheckAuth(Request, dbContext);

            if (sp!=null)
            {
                Setting setting = dbContext.Settings.First(x => x.SpotId == sp.SpotId);

                if (formDetails.ContainsKey("SkipOn")) setting.SkipOn = true;
                else setting.SkipOn = false;

                if (formDetails.ContainsKey("SkipIgnorePlaylist")) setting.SkipIgnorePlaylist = true;
                else setting.SkipIgnorePlaylist = false;

                if (formDetails.ContainsKey("SkipRemoveFromPlaylist")) setting.SkipRemoveFromPlaylist = true;
                else setting.SkipRemoveFromPlaylist = false;

                if (formDetails.ContainsKey("ShuffleOn")) setting.ShuffleOn = true;
                else setting.ShuffleOn = false;

                if (formDetails.ContainsKey("ShuffleAlbums")) setting.ShuffleAlbums = true;
                else setting.ShuffleAlbums = false;

                if (formDetails.ContainsKey("ShufflePlaylists")) setting.ShufflePlaylists = true;
                else setting.ShufflePlaylists = false;

                setting.SkipExpiryHours = int.Parse(formDetails["SkipExpiryHours"].First()) * 24;
                setting.SkipTrigger = int.Parse(formDetails["SkipTrigger"].First());

                dbContext.Update(setting);
                dbContext.SaveChangesAsync();
            }

            return RedirectPermanent("/settings");
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

                    CookieOptions options = new CookieOptions();
                    options.Expires = DateTimeOffset.Now.AddDays(1);

                    Actions.Log.Add("User Signed Up", Actions.LogError.Info);

                    HttpContext.Response.Cookies.Append("spotid", spot.First().SpotId.ToString(), options);

                    HttpContext.Response.Cookies.Append("authToken", authToken, options);
                    return RedirectPermanent("/Insights");
                }
            }
            return BadRequest();
        }

        #endregion Methods
    }
}
