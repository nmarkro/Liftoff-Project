using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BCCWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BCCWebApp.ViewModel
{
    public class AddDeckViewModel
    {
        public List<SelectListItem> Operators = BCCData.Operators.Select(o => new SelectListItem { Text = o.Name, Value = o.Id.ToString() }).ToList();
        public List<SelectListItem> Chips = BCCData.Chips.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();
        public List<SelectListItem> NaviChips = BCCData.NaviChips.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() }).ToList();

        [Required(ErrorMessage = "Deck Name is required")]
        public string DeckName { get; set; }

        [Required(ErrorMessage = "Operator is required")]
        public int Operator { get; set; }

        public int ChipR { get; set; }
        [Required]
        public int ChipNavi { get; set; }
        public int ChipL { get; set; }
        public int Chip1a { get; set; }
        public int Chip1b { get; set; }
        public int Chip2a { get; set; }
        public int Chip2b { get; set; }
        public int Chip2c { get; set; }
        public int Chip3a { get; set; }
        public int Chip3b { get; set; }
        public int Chip3c { get; set; }
        public int Chip3d { get; set; }

        public AddDeckViewModel()
        {
        }
    }
}
