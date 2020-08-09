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

        private Chip GetChip(int chipID)
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

            int maxMB = GetChip(data[1]).MB;

            int curMB = 0;
            curMB += GetChip(data[2]).MB;
            curMB += GetChip(data[3]).MB;
            curMB += GetChip(data[4]).MB;
            curMB += GetChip(data[5]).MB;
            curMB += GetChip(data[6]).MB;
            curMB += GetChip(data[7]).MB;
            curMB += GetChip(data[8]).MB;
            curMB += GetChip(data[9]).MB;
            curMB += GetChip(data[10]).MB;

            if (curMB > maxMB)
            {
                yield return new ValidationResult("Invalid Deck: MB over limit");
            }
        }
    }
}
