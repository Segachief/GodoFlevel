using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.FlevelRandomisation
{
    public class FieldScript
    {
        public static byte[] ChangeItems(byte[] data)
        {
            // This could be sped up by opening the kernel2 and grabbing all the strings from there.
            // Then I don't need to write them all out in hex.

            // Matching the string to the actual item pickup will be difficult; will need to seek out the window ID
            // and what text ID it's looking for. Will be fairly complex especially as they're not uniformly scripted.

            var search = new byte[] { 0x32, 0x45, 0x43, 0x45, 0x49, 0x56, 0x45, 0x44, 0x00, 0x02 };
            var current = new byte[10];
            var maxSearchRange = data.Length - 9;
            for (var o = 0; o < maxSearchRange; o++)
            {
                current[0] = data[o];
                current[1] = data[o + 1];
                current[2] = data[o + 2];
                current[3] = data[o + 3];
                current[4] = data[o + 4];
                current[5] = data[o + 5];
                current[6] = data[o + 6];
                current[7] = data[o + 7];
                current[8] = data[o + 8];
                current[9] = data[o + 9];

                // If a match is found, we can call a method to change the string
                if (search.SequenceEqual(current))
                {
                    // Call a method to select a new item here and write its string in using Index position

                    // Test
                    data[o] = 0x32; o++;
                    data[o] = 0x32; o++;
                    data[o] = 0x32; o++;
                    data[o] = 0x32; o++;
                    data[o] = 0x32; o++;
                    data[o] = 0x32; o++;
                    data[o] = 0x32; o++;
                    data[o] = 0x32; o++;
                    data[o] = 0x32; o++;
                    data[o] = 0x32; o++;
                }
            }
            return data;
        }
    }
}
