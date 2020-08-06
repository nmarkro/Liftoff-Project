using BCCWebApp.Data;
using BCCWebApp.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace BCCWebApp.ViewModel
{
    public class DeckViewerViewModel
    {
        public Chip ChipR { get; set; }
        public Chip ChipNavi { get; set; }
        public Chip ChipL { get; set; }
        public Chip Chip1a { get; set; }
        public Chip Chip1b { get; set; }
        public Chip Chip2a { get; set; }
        public Chip Chip2b { get; set; }
        public Chip Chip2c { get; set; }
        public Chip Chip3a { get; set; }
        public Chip Chip3b { get; set; }
        public Chip Chip3c { get; set; }
        public Chip Chip3d { get; set; }

        private Chip getChip(int chipID)
        {
            Chip chip = BCCData.Chips.Where(c => c.Id == chipID).FirstOrDefault();
            if (chip == null)
            {
                chip = BCCData.NaviChips.Where(c => c.Id == chipID).FirstOrDefault();
            }
            return chip;
        }

        public DeckViewerViewModel()
        {
            ChipNavi = getChip(200);
            Chip1a = getChip(0);
            Chip1b = getChip(0);
            Chip2a = getChip(0);
            Chip2b = getChip(0);
            Chip2c = getChip(0);
            Chip3a = getChip(0);
            Chip3b = getChip(0);
            Chip3c = getChip(0);
            Chip3d = getChip(0);
            ChipR = getChip(0);
            ChipL = getChip(0);
        }

        public DeckViewerViewModel(string naviName, string naviCode)
        {
            naviCode = Util.Normalize(naviCode);

            int[] data = Util.UnpackNaviCode(naviName, naviCode);

            ChipNavi =  getChip(data[1]);
            Chip1a =    getChip(data[2]);
            Chip1b =    getChip(data[3]);
            Chip2a =    getChip(data[4]);
            Chip2b =    getChip(data[5]);
            Chip2c =    getChip(data[6]);
            Chip3a =    getChip(data[7]);
            Chip3b =    getChip(data[8]);
            Chip3c =    getChip(data[9]);
            Chip3d =    getChip(data[10]);
            ChipR =     getChip(data[11]);
            ChipL =     getChip(data[12]);
        }               
    }
}
