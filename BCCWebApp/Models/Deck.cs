using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BCCWebApp.Models
{
    public class Deck
    {
        public int Id { get; set; }
        [ForeignKey("TwitchUser")]
        public string TwitchUserId { get; set; }
        public TwitchUser TwitchUser { get; set; }
        [StringLength(4, MinimumLength = 1)]
        public string NaviName { get; set; }
        [StringLength(24, MinimumLength = 24)]
        public string NaviCode { get; set; }
        public int Wins { get; set; }
        public int Battles { get; set; }
    
        public Deck()
        {
        }
    }
}
