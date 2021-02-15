using System.Collections.Generic;
using System.Threading;
using Spotter_Azure.Models;

namespace Spotter_Azure.Models
{
    public static class Memory
    {
        #region Fields

        public static List<User> Users = new List<User>();

        #endregion Fields

        #region Methods

        public static async void Add(User user)
        {
            while (user.userid == null) Thread.Sleep(100);

            int i = Users.FindIndex(x => x.userid == user.userid);
            if (i == -1) Users.Add(user);
            else
            {
                Users[i] = user;
            }
        }

        #endregion Methods
    }
}
