using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BCCWebApp.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageUrl { get; set; }
        public int TotalWins { get; set; }
        public int TotalBattles { get; set; }
        public int? CurrentDeckId { get; set; }
        public bool TorunamentRegistered { get; set; }

        public User()
        {
        }

        public User(string id, string login, string displayName, string profileImageUrl)
        {
            Id = id;
            Login = login;
            DisplayName = displayName;
            ProfileImageUrl = profileImageUrl;
            TotalWins = 0;
            TotalBattles = 0;
            TorunamentRegistered = false;
        }
    }
}
