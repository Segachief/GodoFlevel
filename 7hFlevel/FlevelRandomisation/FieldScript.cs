using _7hFlevel.Indexing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.FlevelRandomisation
{
    public class FieldScript
    {
        public static byte[] ChangeItemsMateria(byte[] data, string name)
        {
            // Get the start of the text section offset
            byte[] textStart = new byte[2];

            textStart[0] = data[4];
            textStart[1] = data[5];
            int textOffset = EndianMethods.GetLittleEndianIntTwofer(textStart, 0);

            if (name == "qd")
            {
                // Breakpoint sink to analyse qd's allocation of items
            }

            // Prevents an out of bounds exception and returns data unaltered
            if (textOffset >= data.Length)
            {
                return data;
            }
            int textCount = data[textOffset];

            // Now iterate through the event script to find references to used dialog
            // We have very little security available for a 3-byte opcode in which 2
            // of the bytes are unknown, so anything available has been added to help
            // reduce possibility of false positives.
            var maxSearchRangeDialogue = data.Length - 3;
            var currentDialogue = new byte[1];
            var searchDialogue = new byte[] { 0x40 }; // This is the opcode, the only 1 of the 3 bytes that is static
            List<int> usedTexts = new List<int>();
            for (int k = 0; k < maxSearchRangeDialogue; k++)
            {
                // OpCode 0x40
                currentDialogue[0] = data[k];

                // If a match is found, we can call a method to change the string
                if (searchDialogue.SequenceEqual(currentDialogue))
                {
                    // Check that next part of opcode is a valid value of 0-3
                    if (data[k + 1] < 4)
                    {
                        // Then check that the dialogue ID is a valid value
                        if (data[k + 2] < textCount)
                        {
                            // Add the text ID set to this opcode to our list of used texts
                            // Exclude duplicates
                            if (!usedTexts.Contains(data[k + 2]))
                            {
                                usedTexts.Add(data[k + 2]);
                            }
                        }
                    }
                }
            }

            // Exceptions List
            // A number of texts aren't uniformly formatted; some have extra spaces, some omit the Key Item part, etc.
            // This removes them from the pool so that their strings aren't swapped.
            if (name == "blin62_1")
            {
                // Removes Keycard 65 reference from the pool
                usedTexts.Remove(19);
            }
            if (name == "bugin1b")
            {
                // Master Magic/Command/Summon texts have false positives, prune them
                usedTexts.Remove(14);
                usedTexts.Remove(16);
                usedTexts.Remove(17);
            }
            if (name == "eals_1")
            {
                // Received texts on this field start with 2 spaces, throwing it off
                // TODO: Add logic to handle this field
                return data;
            }
            if (name == "mkt_w")
            {
                // Removes Batteries reference from the pool
                usedTexts.Remove(13);
            }
            if (name == "zz1")
            {
                // Removes Mythril reference from pool
                usedTexts.Remove(5);
            }
            if (name == "zz6")
            {
                // Has no 'Materia!' at the end of its Materia string, gets flagged as an item instead.
                // TODO: Add logic to handle this case.
                return data;
            }
            if (name == "zz8")
            {
                // Name of Materia differs to Kernel-stored name (KOTR)
                // TODO: Add logic to handle this case.
                return data;
            }


            Random rnd = new Random();
            int r = 0; // Iterates new item string
            string itemFileLocation = "";
            //var itemNames = MateriaStrings.GetMateriaStrings(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin19");
            //var weaponNames = MateriaStrings.GetMateriaStrings(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin20");
            //var armourNames = MateriaStrings.GetMateriaStrings(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin21");
            //var accessoryNames = MateriaStrings.GetMateriaStrings(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin22");
            var materiaNames = MateriaStrings.GetMateriaStrings(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin23");

            var terminator = new byte[1];
            int offset = 0;
            var currentString = new byte[10];
            var maxSearchRangeString = data.Length - 9;

            var maxSearchRangeItem = data.Length - 5;

            var currentMateria = new byte[7];
            var currentItem = new byte[5];
            var finalItem = new byte[3];

            List<byte> oldName = new List<byte>();
            int oldMateriaID = 0;
            int oldItemID = 0;

            // Searches for final items to update them back to 01 quantity
            var searchFinalItem = new byte[] { 0x58, 0x00, 0xFE };

            // Searches for string 'Received "' for items
            var searchString = new byte[] { 0x32, 0x45, 0x43, 0x45, 0x49, 0x56, 0x45, 0x44, 0x00, 0x02 };

            // Checks for '" Materia!' for Materia
            var appendSearchMateria = new byte[] { 0x02, 0x00, 0x2D, 0x41, 0x54, 0x45, 0x52, 0x49, 0x41, 0x01 };

            // Tracks the current Text ID based on how many terminators have been read; compared to List of Used Text IDs
            int textID = 0;

            // Checks for Materia first, then Items; Received will be overwritten by Materia check so no overlap should occur.

            // If materia option is on
            for (var y = textOffset + (textCount * 2); y < maxSearchRangeString; y++)
            {
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
                    textID++;
                    offset = y + 1;
                }

                if (appendSearchMateria.SequenceEqual(currentString))
                {
                    if (usedTexts.Contains(textID))
                    {
                        // Selects new Item ID
                        int newMateriaID = rnd.Next(91);

                        // Re-rolls until ID is valid
                        while (newMateriaID == 22 ||
                            newMateriaID == 38 ||
                            newMateriaID == 45 ||
                            newMateriaID == 46 ||
                            newMateriaID == 47 ||
                            newMateriaID == 63 ||
                            newMateriaID == 66 ||
                            newMateriaID == 67)
                        {
                            newMateriaID = rnd.Next(91);
                            if (newMateriaID == 91)
                            {
                                // break
                            }
                        }

                        // Skips past the Receieved " part of the string to the Materia Name
                        int countCharacters = offset + 10;

                        // Get old Materia name and match it to its ID; 0x02 is the terminator (")
                        while (data[countCharacters] != 0x02)
                        {
                            oldName.Add(data[countCharacters]);
                            countCharacters++;
                        }

                        // Figure out the Materia ID by matching the name in the kernel strings
                        oldMateriaID = MateriaStrings.GetMateriaID(oldName);
                        oldName.Clear();

                        // Replace string with new Materia name
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
                        Trace.WriteLine("Rewrote Materia String");
                        y++;
                        r = 0;

                        // Now to find the Materia opcode; assuming they are in same order as text.
                        // When a string is found, a search is conducted to find the next Materia opcode.
                        var searchMateria = new byte[] { 0x5B, 0x00, 0x00, (byte)oldMateriaID, 0x00, 0x00, 0x00 };
                        maxSearchRangeItem = data.Length - 6;

                        // You know you've hit rock bottom when you have to start assigning the value of a variable to itself.
                        for (int c = 0; c < maxSearchRangeItem; c++)
                        {
                            currentMateria[0] = data[c];       // Always 0x5B
                            currentMateria[1] = data[c + 1];   // Always 0x00
                            currentMateria[2] = data[c + 2];   // Always 0x00
                            currentMateria[3] = data[c + 3];   // Old Materia ID
                            currentMateria[4] = data[c + 4];   // Always 0x00
                            currentMateria[5] = data[c + 5];   // Always 0x00
                            currentMateria[6] = data[c + 6];   // Always 0x00

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
                                Trace.WriteLine("A Materia was rewritten successfully");
                            }
                        }
                        usedTexts.Remove(textID);
                    }
                }
            }
            offset = 0;
            r = 0;
            textID = 0;

            // If item option is on
            for (var o = textOffset + (textCount * 2); o < maxSearchRangeString; o++)
            {
                terminator[0] = data[o];

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

                // Tracks location of the terminator, and moves up 1 to start of next string
                if (terminator[0] == 0xFF)
                {
                    textID++;
                    offset = o + 1;
                }

                if(textID == 10)
                {
                    // break
                }

                // Match found for the string in this chunk
                if (searchString.SequenceEqual(currentString))
                {
                    if (usedTexts.Contains(textID))
                    {
                        // Rolls a new Item ID; will re-roll if it picks an empty item ID
                        int newItemID = rnd.Next(0, 319);
                        while (newItemID > 104 && newItemID < 128)
                        {
                            newItemID = rnd.Next(0, 319); // Selects new Item ID
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

                        // Skips past the Receieved " part of the string to the Item Name
                        int countItemCharacters = offset + 10;

                        // Get old Item name and match it to its ID; 0x02 is the terminator (")
                        while (data[countItemCharacters] != 0x02)
                        {
                            oldName.Add(data[countItemCharacters]);
                            countItemCharacters++;
                        }

                        // Figure out the Item ID by matching the name in the kernel strings
                        oldItemID = ItemStrings.GetItemID(oldName);
                        oldName.Clear();

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
                        Trace.WriteLine("Rewrote Item String");
                        r = 0;

                        // Now to find the item opcode; assuming they are in same order as text.
                        // When a string is found, a search is conducted to find the next item Opcode.

                        ulong convertOldItemID = (ulong)oldItemID;
                        byte[] oldItemIDByte = EndianMethods.GetLittleEndianConvert(convertOldItemID);

                        var searchItem = new byte[] { 0x58, 0x00, oldItemIDByte[0], oldItemIDByte[1], 0x01 };

                        // You know you've hit rock bottom when you have to start assigning the value of a variable to itself.
                        for (int c = 0; c < maxSearchRangeItem; c++)
                        {
                            currentItem[0] = data[c];       // Always 0x58
                            currentItem[1] = data[c + 1];   // Always 0x00
                            currentItem[2] = data[c + 2];   // Old Item ID - 2 Bytes
                            currentItem[3] = data[c + 3];
                            currentItem[4] = data[c + 4];   // Always 0x01, but may be rare cases where it is higher number like Mt. Corel

                            // If a match is found, we can call a method to change the string
                            if (searchItem.SequenceEqual(currentItem))
                            {
                                // Convert item ID into a 2 byte endian value
                                ulong convertItemID = (ulong)newItemID;
                                byte[] convertedItemID = EndianMethods.GetLittleEndianConvert(convertItemID);

                                data[c] = 0x58;
                                data[c + 1] = 0x00;
                                data[c + 2] = convertedItemID[0]; // Item ID 1st byte
                                data[c + 3] = convertedItemID[1]; // Item ID 2nd byte
                                data[c + 4] = 0xFE; // Quantity
                                Trace.WriteLine("An item ID was rewritten");
                            }
                        }
                        usedTexts.Remove(textID);
                    }
                }
            }

            // Final Pass to revert items back to 01 quantity (or vary it)
            for (int f = 0; f < maxSearchRangeItem; f++)
            {
                finalItem[0] = data[f];       // Always 0x58
                finalItem[1] = data[f + 1];   // Always 0x00
                                                // Two bytes are skipped as they can vary (Item ID)
                finalItem[2] = data[f + 4];   // Always 0x01, but may be rare cases where it is higher number like Mt. Corel

                // If a match is found, we can call a method to change the string
                if (searchFinalItem.SequenceEqual(finalItem))
                {
                    data[f + 4] = 0x01; // Can change this if user specified it
                }
            }
            return data;
        }
    }
}
