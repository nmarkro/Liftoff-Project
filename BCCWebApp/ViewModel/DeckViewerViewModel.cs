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

        private Chip GetChip(int chipID)
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
            ChipNavi = GetChip(200);
            Chip1a = GetChip(0);
            Chip1b = GetChip(0);
            Chip2a = GetChip(0);
            Chip2b = GetChip(0);
            Chip2c = GetChip(0);
            Chip3a = GetChip(0);
            Chip3b = GetChip(0);
            Chip3c = GetChip(0);
            Chip3d = GetChip(0);
            ChipR = GetChip(0);
            ChipL = GetChip(0);
        }

        public DeckViewerViewModel(string naviName, string naviCode)
        {
            naviCode = Util.Normalize(naviCode);

            int[] data = Util.UnpackNaviCode(naviName, naviCode);

            ChipNavi =  GetChip(data[1]);
            Chip1a =    GetChip(data[2]);
            Chip1b =    GetChip(data[3]);
            Chip2a =    GetChip(data[4]);
            Chip2b =    GetChip(data[5]);
            Chip2c =    GetChip(data[6]);
            Chip3a =    GetChip(data[7]);
            Chip3b =    GetChip(data[8]);
            Chip3c =    GetChip(data[9]);
            Chip3d =    GetChip(data[10]);
            ChipR =     GetChip(data[11]);
            ChipL =     GetChip(data[12]);
        }               
    }
}
