using System;
using Scrypt;
using System.Collections.Generic;

#nullable disable

namespace Spotter_Azure.Models
{
    public partial class Session:DBModels.Session
    {
        private static Scrypt.ScryptEncoder encoder = new ScryptEncoder();
        private static Random rnd = new Random();

        public Session()
        {

        }

        public Session(string AuthToken, Spotify user)
        {
            SpotId = user.SpotId;
            this.AuthToken = encoder.Encode(AuthToken);
        }

        public void SetAuthToken(string authToken)
        {
            this.AuthToken = encoder.Encode(authToken);
        }

        public bool AuthTokenMatches(string authToken)
        {
            return encoder.Compare(authToken, this.AuthToken);
        }

        public static string GetAuthToken(uint length = 32)
        {
            string s = "";
            for (; length > 0; length--) s += (char)rnd.Next(65, 122);
            return s.Replace("\\", "/");
        }
    }
}
