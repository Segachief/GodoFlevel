using _7hFlevel.Indexing;
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
            Random rnd = new Random();
            int r = 0; // Iterates new item string
            int c = 0; // Iterates data when searching for item opcodes
            int newItemID = rnd.Next(10); // Selects new Item ID

            string fileLocation = "C:\\Users\\stewart.melville\\Downloads\\Default Files\\Default Files\\data\\field\\Chunks\\kernel.bin19";
            var itemNames = ItemStrings.GetItemStrings(fileLocation); //  Retrieves item names from kernel2 in order of Item ID

            // Searches for string 'Received "'
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

                // Match found for the string in this chunk
                if (searchString.SequenceEqual(currentString))
                {
                    // Replace string with new item name
                    while (itemNames[newItemID][r] != 0xFF)
                    {
                        data[o] = itemNames[newItemID][r]; o++; r++;
                    }
                    // Blank the rest of the string with 00s until the terminator FF
                    while(data[o] != 0xFF)
                    {
                        data[o] = 0x00; o++;
                    }
                    r = 0;

                    // Now to find the item opcode; assuming they are in same order as text.
                    // When a string is found, a search is conducted to find the next item Opcode.
                    var searchItem = new byte[] { 0x58, 0x00, 0x01 };
                    var currentItem = new byte[3];
                    var maxSearchRangeItem = data.Length - 5;

                    // You know you've hit rock bottom when you have to start assigning the value of a variable to itself.
                    for (c = c; c < maxSearchRangeItem; c++)
                    {
                        currentItem[0] = data[c];       // Always 0x58
                        currentItem[1] = data[c + 1];   // Always 0x00
                                                        // Two bytes are skipped as they can vary (Item ID)
                        currentItem[2] = data[c + 4];   // Always 0x01, but may be rare cases where it is higher number like Mt. Corel

                        // If a match is found, we can call a method to change the string
                        if (searchItem.SequenceEqual(currentItem))
                        {
                            // Call a method to select a new item here and write its data in using Index position
                            // Test
                            data[c] = 0x58; c++;
                            data[c] = 0x00; c++;
                            data[c] = (byte)newItemID; c++; // Item ID 1st byte
                            data[c] = 0x00; c++; // Item ID 2nd byte
                            data[c] = 0x09; c++; // Quantity here
                        }
                    }
                }
            }
            return data;
        }
    }
}
