using BCCWebApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCCWebApp.Models;
using BCCWebApp.Scripts;

namespace BCCWebApp.ViewModel
{
    public class AddOrEditDeckViewModel : IValidatableObject
    {
        public int DeckId { get; set; }
        
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

        private Chip getChip(int chipID)
        {
            Chip chip = BCCData.Chips.Where(c => c.Id == chipID).FirstOrDefault();
            if (chip == null)
            {
                chip = BCCData.NaviChips.Where(c => c.Id == chipID).FirstOrDefault();
            }
            return chip;
        }
        
        public AddOrEditDeckViewModel()
        {
        }

        public AddOrEditDeckViewModel(Deck deck)
        {
            DeckId = deck.Id;
            DeckName = deck.Name;

            int[] data = Util.UnpackNaviCode(deck.NaviName, Util.Normalize(deck.NaviCode));

            Operator =  data[0];
            ChipNavi =  data[1];
            Chip1a =    data[2];
            Chip1b =    data[3];
            Chip2a =    data[4];
            Chip2b =    data[5];
            Chip2c =    data[6];
            Chip3a =    data[7];
            Chip3b =    data[8];
            Chip3c =    data[9];
            Chip3d =    data[10];
            ChipR =     data[11];
            ChipL =     data[12];
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int maxMB = getChip(ChipNavi).MB;

            int curMB = 0;
            curMB += getChip(Chip1a).MB;
            curMB += getChip(Chip1b).MB;
            curMB += getChip(Chip2a).MB;
            curMB += getChip(Chip2b).MB;
            curMB += getChip(Chip2c).MB;
            curMB += getChip(Chip3a).MB;
            curMB += getChip(Chip3b).MB;
            curMB += getChip(Chip3c).MB;
            curMB += getChip(Chip3d).MB;

            if (curMB > maxMB)
            {
                yield return new ValidationResult("Invalid Deck: MB over limit");
            }
        }
    }
}
