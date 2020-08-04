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
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string Name { get; set; }
        [StringLength(4, MinimumLength = 1)]
        public string NaviName { get; set; }
        [StringLength(29, MinimumLength = 29)]
        public string NaviCode { get; set; }
        public int Wins { get; set; }
        public int Battles { get; set; }
    
        public Deck()
        {
        }
    }
}
