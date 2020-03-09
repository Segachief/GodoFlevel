using _7hFlevel.Indexing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.FlevelRandomisation
{
    public class FieldScript
    {
        public static byte[] ChangeItemsMateria(byte[] data)
        {
            Random rnd = new Random();
            int r = 0; // Iterates new item string
            int c = 0; // Iterates data when searching for item opcodes
            string itemFileLocation = "";
            string materiaFileLocation = Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin23";

            var terminator = new byte[1];
            int offset = 0;
            var currentString = new byte[10];
            var maxSearchRangeString = data.Length - 9;

            var searchItem = new byte[] { 0x58, 0x00, 0x01 };
            var searchFinalItem = new byte[] { 0x58, 0x00, 0x02 };
            var currentItem = new byte[3];
            var maxSearchRangeItem = data.Length - 5;
            bool matchFound = false;

            // Searches for string 'Received "' for items
            var searchString = new byte[] { 0x32, 0x45, 0x43, 0x45, 0x49, 0x56, 0x45, 0x44, 0x00, 0x02 };

            // Checks for '" Materia!' for Materia
            var appendSearchMateria = new byte[] { 0x02, 0x00, 0x2D, 0x41, 0x54, 0x45, 0x52, 0x49, 0x41, 0x01 };

            // Checks for Materia first, then Items; Recieved will be overwritten by Materia check so no
            // overlap should occur.

            // If materia option is on
            for (var y = 0; y < maxSearchRangeString; y++)
            {

                // What could do is, have a var that looks for FF while we search and records its offset.
                // That way can jump to start of the string that has matched.

                terminator[0] = data[y];

                currentString[0] = data[y];
                currentString[1] = data[y + 1];
                currentString[2] = data[y + 2];
                currentString[3] = data[y + 3];
                currentString[4] = data[y + 4];
                currentString[5] = data[y + 5];
                currentString[6] = data[y + 6];
                currentString[7] = data[y + 7];
                currentString[8] = data[y + 8];
                currentString[9] = data[y + 9];

                // Tracks location of the terminator, and moves up 1 to start of next string
                if (terminator[0] == 0xFF)
                {
                    offset = y + 1;
                }

                if (appendSearchMateria.SequenceEqual(currentString))
                {
                    int newMateriaID = rnd.Next(91); // Selects new Item ID
                    while (newMateriaID > 104 && newMateriaID < 128)
                    {
                        newMateriaID = rnd.Next(91); // Selects new Item ID
                    }
                    var materiaNames = MateriaStrings.GetMateriaStrings(materiaFileLocation);
                    // Replace string with new Materia name
                    //y = offset;
                    while (materiaNames[newMateriaID][r] != 0xFF)
                    {
                        // Sets position to start of string
                        data[offset] = materiaNames[newMateriaID][r]; offset++; r++;
                    }
                    // Blank the rest of the string with 00s until the terminator FF
                    while (data[offset] != 0xFF)
                    {
                        data[offset] = 0x00; offset++;
                    }
                    y++;
                    r = 0;

                    // Now to find the Materia opcode; assuming they are in same order as text.
                    // When a string is found, a search is conducted to find the next Materia opcode.
                    var searchMateria = new byte[] { 0x5B, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    var currentMateria = new byte[6];
                    maxSearchRangeItem = data.Length - 6;

                    // You know you've hit rock bottom when you have to start assigning the value of a variable to itself.
                    for (c = c; c < maxSearchRangeItem; c++)
                    {
                        currentMateria[0] = data[c];       // Always 0x5B
                        currentMateria[1] = data[c + 1];   // Always 0x00
                        currentMateria[2] = data[c + 2];   // Always 0x00
                                                           // Skipped as this is the ID and varies
                        currentMateria[3] = data[c + 4];   // Always 0x00
                        currentMateria[4] = data[c + 5];   // Always 0x00
                        currentMateria[5] = data[c + 6];   // Always 0x00

                        // If a match is found, we can call a method to change the string
                        if (searchMateria.SequenceEqual(currentMateria))
                        {
                            data[c] = 0x5B; c++;
                            data[c] = 0x00; c++;
                            data[c] = 0x00; c++;
                            data[c] = (byte)newMateriaID; c++; // Materia ID
                            data[c] = 0x00; c++;
                            data[c] = 0x00; c++;
                            data[c] = 0x00; c++;
                        }
                    }
                }
            }
            r = 0;
            c = 0;

            // If item option is on
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
                    int newItemID = rnd.Next(256, 319); // Selects new Item ID
                    while (newItemID > 104 && newItemID < 128)
                    {
                        newItemID = rnd.Next(256, 319); // Selects new Item ID
                    }

                    if (newItemID < 128)
                    {
                        itemFileLocation = Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin19";
                    }
                    else if (newItemID < 256)
                    {
                        itemFileLocation = Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin20";
                    }
                    else if (newItemID < 288)
                    {
                        itemFileLocation = Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin21";
                    }
                    else if (newItemID < 320)
                    {
                        itemFileLocation = Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin22";
                    }

                    var itemNames = ItemStrings.GetItemStrings(itemFileLocation, newItemID);

                    // Replace string with new item name
                    while (itemNames[newItemID][r] != 0xFF)
                    {
                        data[o] = itemNames[newItemID][r]; o++; r++;
                    }
                    // Blank the rest of the string with 00s until the terminator FF
                    while (data[o] != 0xFF)
                    {
                        data[o] = 0x00; o++;
                    }
                    r = 0;

                    // Now to find the item opcode; assuming they are in same order as text.
                    // When a string is found, a search is conducted to find the next item Opcode.
                    //searchItem = new byte[] { 0x58, 0x00, 0x01 };
                    //currentItem = new byte[3];
                    //maxSearchRangeItem = data.Length - 5;
                    //matchFound = false;

                    // You know you've hit rock bottom when you have to start assigning the value of a variable to itself.
                    for (c = c; c < maxSearchRangeItem; c++)
                    {
                        currentItem[0] = data[c];       // Always 0x58
                        currentItem[1] = data[c + 1];   // Always 0x00
                                                        // Two bytes are skipped as they can vary (Item ID)
                        currentItem[2] = data[c + 4];   // Always 0x01, but may be rare cases where it is higher number like Mt. Corel

                        if (matchFound == false)
                        {
                            // If a match is found, we can call a method to change the string
                            if (searchItem.SequenceEqual(currentItem))
                            {
                                // Convert item ID into a 2 byte endian value
                                ulong convertItemID = (ulong)newItemID;
                                byte[] convertedItemID = EndianMethods.GetLittleEndianConvert(convertItemID);

                                data[c] = 0x58; c++;
                                data[c] = 0x00; c++;
                                data[c] = convertedItemID[0]; c++; // Item ID 1st byte
                                data[c] = convertedItemID[1]; c++; // Item ID 2nd byte
                                data[c] = 0x02; c++; // Quantity here - Setting 02 prevents it from being changed again
                                matchFound = true;
                            }
                        }
                    }
                    c = 0;
                    matchFound = false;
                }
            }
            c = 0;
            // Final Pass to revert items back to 01 quantity (or vary it)
            for (c = c; c < maxSearchRangeItem; c++)
            {
                currentItem[0] = data[c];       // Always 0x58
                currentItem[1] = data[c + 1];   // Always 0x00
                                                // Two bytes are skipped as they can vary (Item ID)
                currentItem[2] = data[c + 4];   // Always 0x01, but may be rare cases where it is higher number like Mt. Corel

                if (matchFound == false)
                {
                    // If a match is found, we can call a method to change the string
                    if (searchFinalItem.SequenceEqual(currentItem))
                    {
                        c++;
                        c++;
                        c++;
                        c++;
                        data[c] = 0x01; c++; // Can change this if user specified it
                        matchFound = true;
                    }
                }
            }
            return data;
        }
    }
}
