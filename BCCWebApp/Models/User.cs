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
        [Key]
        [ForeignKey("TwitchUser")]
        public string TwitchUserId { get; set; }
        public TwitchUser TwitchUser { get; set; }
        public int TotalWins { get; set; }
        public int TotalBattles { get; set; }
        public int? CurrentDeckId { get; set; }
        public virtual Deck CurrentDeck { get; set; }
        public bool TorunamentRegistered { get; set; }

        public User()
        {
        }

        public User(TwitchUser twitchUser)
        {
            this.TwitchUser = twitchUser;
            TwitchUserId = twitchUser.Id;
            TotalWins = 0;
            TotalBattles = 0;
            TorunamentRegistered = false;
        }
    }
}
