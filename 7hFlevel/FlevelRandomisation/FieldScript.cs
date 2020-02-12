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
            // Change Item ID
            // Current issue; we can't search statically, and itemID can use 2-byte so effectively we can only search
            // for 58 00, which is too short to avoid false positives. Is there a way to wild card it? Or resrict search
            // to the script section only and not text?

            // We can make the following assumptions:
            // 1. The quantity is always 1.
            // Therefore, we can skip comparing the ID/bank and instead just compare the opening
            var searchItem = new byte[] { 0x58, 0x00, 0x01 };
            var currentItem = new byte[3];
            var maxSearchRangeItem = data.Length - 5;
            for (var o = 0; o < maxSearchRangeItem; o++)
            {
                currentItem[0] = data[o];       // Always 0x58
                currentItem[1] = data[o + 1];   // Always 0x00
                // Two bytes are skipped as they can vary (Item ID)
                currentItem[2] = data[o + 4];   // Always 0x01, but may be rare cases where it is higher number like Mt. Corel

                // If a match is found, we can call a method to change the string
                if (searchItem.SequenceEqual(currentItem))
                {
                    // Call a method to select a new item here and write its data in using Index position
                    // Test
                    data[o] = 0x58; o++;
                    data[o] = 0x00; o++;
                    data[o] = 0x32; o++; // Item ID 1st byte
                    data[o] = 0x00; o++; // Item ID 2nd byte
                    data[o] = 0x09; o++; // Quantity here
                }
            }

            // Change Item String
            // This could be sped up by opening the kernel2 and grabbing all the strings from there.
            // Then I don't need to write them all out in hex.

            // Matching the string to the actual item pickup will be difficult; will need to seek out the window ID
            // and what text ID it's looking for. Will be fairly complex especially as they're not uniformly scripted.

            var searchString = new byte[] { 0x32, 0x45, 0x43, 0x45, 0x49, 0x56, 0x45, 0x44, 0x00, 0x02 };
            var currentString = new byte[10];
            var maxSearchRangeString = data.Length - 9;
            for (var o = 0; o < maxSearchRangeString; o++)
            {
                currentString[0] = data[o];
                currentString[1] = data[o + 1];
                currentString[2] = data[o + 2];
                currentString[3] = data[o + 3];
                currentString[4] = data[o + 4];
                currentString[5] = data[o + 5];
                currentString[6] = data[o + 6];
                currentString[7] = data[o + 7];
                currentString[8] = data[o + 8];
                currentString[9] = data[o + 9];

                // If a match is found, we can call a method to change the string
                // if doesn't work swap search and current
                if (searchString.SequenceEqual(currentString))
                {
                    // Call a method to select a new item here and write its string in using Index position

                    // Test - Success (Replaced Received with RRRRRR)
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
