using BCCWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BCCWebApp.Scripts
{
    public class Util
    {
        // Deck encoding/decoding code modified and based on https://therockmanexezone.com/ncgen/ by Prof9 (https://github.com/Prof9)
        public static string PackNaviCode(string name, int[] data)
        {
            int[] nameBytes = GetNameBytes(name);

            data = Shift(data, nameBytes[0] + nameBytes[2]);
            data = Crypt(nameBytes, data);
            data = Unshift(data, nameBytes[1] + nameBytes[3]);

            data[14] = CalcChecksum(data);

            data = Crypt(nameBytes, data);

            string naviCode = Encode(data);

            return naviCode;
        }

        public static int[] GetNameBytes(string name)
        {
            int[] nameBytes = new int[] { 0, 0, 0, 0 };
            for (int i = 0; i < name.Length; i++)
            {
                nameBytes[i] = BCCData.CharTable[name[i].ToString()];
            }
            return nameBytes;
        }

        public static int[] Shift(int[] data, int bits)
        {
            int u = (bits >> 3) & 0xF;
            int l = bits & 0x7;

            int[] r = new int[15];
            for (int i = 0; i < 14; i++)
            {
                r[i] = (data[(i + u) % 14] << l) & 0xFF;
                r[i] |= (data[(i + u + 1) % 14] >> (8 - l)) & 0xFF;
            }
            return r;
        }

        public static int[] Crypt(int[] nameBytes, int[] data)
        {
            for (int i = 0; i < 14; i++)
            {
                data[i] ^= nameBytes[i & 3];
            }
            return data;
        }

        public static int[] Unshift(int[] data, int bits)
        {
            if (bits != 0)
            {
                int[] data2 = new int[15];
                for (int i = 0; i < 14; i++)
                {
                    data2[(i + 2) % 14] = data[i];
                }
                data = data2;
            }
            return Shift(data, -bits);
        }

        public static int CalcChecksum(int[] data)
        {
            int sum = 0;
            for (int i = 0; i < 14; i++)
            {
                sum += data[i];
            }
            return -sum & 0xFF;
        }

        public static string Encode(int[] data)
        {
            int[] passBytes = new int[24];
            for (int i = 0; i < 24; i++)
            {
                int b = 0;
                for (int j = 0; j < 15; j++)
                {
                    int v = data[j] | (b << 8);
                    data[j] = ~~(v / 36);
                    b = v % 36;
                }
                passBytes[i] = b;
            }

            string naviCode = "";
            for (int i = 0; i < 24; i++)
            {
                naviCode += BCCData.PassChars[passBytes[i]];
            }

            return naviCode;
        }
    }
}
