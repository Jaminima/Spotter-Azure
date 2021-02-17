using System;
using System.Threading.Tasks;
using Scrypt;

#nullable disable

namespace Spotter_Azure.DBModels
{
    public partial class Session
    {
        private static Scrypt.ScryptEncoder encoder = new ScryptEncoder();
        private static Random rnd = new Random();

        public int SpotId { get; set; }
        public virtual Spotify Spot { get; set; }
        public string AuthToken { get; set; }

        public Session()
        {

        }

        public Session(string AuthToken, Spotify user)
        {
            SpotId = user.SpotId;
            AuthToken = encoder.Encode(AuthToken);
        }

        public static string GetAuthToken(uint length = 32)
        {
            string s = "";
            for (; length > 0; length--) s += (char)rnd.Next(65, 122);
            return s.Replace("\\", "/");
        }
    }
}
