using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using Spotter_Azure.Models;

namespace Spotter_Azure.Controllers
{
    public static class AuthFlow
    {
        //
        #region Fields

        private static string redirect = "https://spotter-azure20210215171759.azurewebsites.net/api/register";

        #endregion Fields

        #region Methods

        public static User FromCode(string scode)
        {
            WebRequest req = WebRequest.Create($"https://accounts.spotify.com/api/token");

            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";

            StreamWriter stream = new StreamWriter(req.GetRequestStream());
            stream.Write($"grant_type=authorization_code&code={scode}&redirect_uri={redirect}&client_id={SpotifyAuthKeys.client_id}&client_secret={SpotifyAuthKeys.client_secret}");
            stream.Flush();
            stream.Close();

            try
            {
                WebResponse res = req.GetResponse();

                StreamReader reader = new StreamReader(res.GetResponseStream());
                JToken data = JToken.Parse(reader.ReadToEnd());
                AuthFlowResponse flowResponse = data.ToObject<AuthFlowResponse>();

                return new User(flowResponse.access_token, flowResponse.refresh_token, DateTime.Now.AddSeconds(int.Parse(flowResponse.expires_in)).AddMinutes(-1));
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }

            return null;
        }

        public static void Refresh(User user)
        {
            WebRequest req = WebRequest.Create($"https://accounts.spotify.com/api/token");

            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";

            StreamWriter stream = new StreamWriter(req.GetRequestStream());
            stream.Write($"grant_type=refresh_token&refresh_token={user.refreshtoken}&client_id={SpotifyAuthKeys.client_id}&client_secret={SpotifyAuthKeys.client_secret}");
            stream.Flush();
            stream.Close();

            try
            {
                WebResponse res = req.GetResponse();

                StreamReader reader = new StreamReader(res.GetResponseStream());
                JToken data = JToken.Parse(reader.ReadToEnd());
                AuthFlowResponse flowResponse = data.ToObject<AuthFlowResponse>();

                user.authtoken = flowResponse.access_token;
                user.authExpires = DateTime.Now.AddSeconds(int.Parse(flowResponse.expires_in)).AddMinutes(-1);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        #endregion Methods

        #region Classes

        public class AuthFlowResponse
        {
            #region Fields

            public string access_token, token_type, scope, expires_in, refresh_token;

            #endregion Fields
        }

        #endregion Classes
    }
}
