using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCCWebApp.Models
{
    public class TwitchUser
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public TwitchUser()
        {

        }
    }
}
