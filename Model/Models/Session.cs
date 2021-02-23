using Scrypt;
using System;

#nullable disable

namespace Model.Models
{
    public partial class Session : DBModels.Session
    {
        #region Fields

        private static Scrypt.ScryptEncoder encoder = new ScryptEncoder();
        private static Random rnd = new Random();

        #endregion Fields

        #region Constructors

        public Session()
        {
        }

        public Session(string AuthToken, Spotify user)
        {
            SpotId = user.SpotId;
            this.AuthToken = encoder.Encode(AuthToken);
        }

        #endregion Constructors

        #region Methods

        public static string GetAuthToken(uint length = 32)
        {
            string s = "";
            for (; length > 0; length--) s += (char)rnd.Next(65, 122);
            return s.Replace("\\", "/");
        }

        public bool AuthTokenMatches(string authToken)
        {
            return encoder.Compare(authToken, this.AuthToken);
        }

        public void SetAuthToken(string authToken)
        {
            this.AuthToken = encoder.Encode(authToken);
        }

        #endregion Methods
    }
}
