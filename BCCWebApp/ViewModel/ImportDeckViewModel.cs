using BCCWebApp.Data;
using BCCWebApp.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BCCWebApp.ViewModel
{
    public class ImportDeckViewModel : IValidatableObject
    {
        [Required]
        public string DeckName { get; set; }
        [Required]
        [StringLength(4, MinimumLength = 1)]
        public string NaviName { get; set; }
        [Required]
        [StringLength(29, MinimumLength = 24)]
        public string NaviCode { get; set; }

        private Chip getChip(int chipID)
        {
            Chip chip = BCCData.Chips.Where(c => c.Id == chipID).FirstOrDefault();
            if (chip == null)
            {
                chip = BCCData.NaviChips.Where(c => c.Id == chipID).FirstOrDefault();
            }
            return chip;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            int[] data = Util.UnpackNaviCode(NaviName, NaviCode);
            if (data == null)
            {
                yield return new ValidationResult("Invalid Code");
            }

            int maxMB = getChip(data[1]).MB;

            int curMB = 0;
            curMB += getChip(data[2]).MB;
            curMB += getChip(data[3]).MB;
            curMB += getChip(data[4]).MB;
            curMB += getChip(data[5]).MB;
            curMB += getChip(data[6]).MB;
            curMB += getChip(data[7]).MB;
            curMB += getChip(data[8]).MB;
            curMB += getChip(data[9]).MB;
            curMB += getChip(data[10]).MB;

            if (curMB > maxMB)
            {
                yield return new ValidationResult("Invalid Deck: MB over limit");
            }
        }
    }
}
