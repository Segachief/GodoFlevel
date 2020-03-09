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
        public static byte[] ChangeItemsMateria(byte[] data, string name)
        {
            // Get the start of the text section offset
            byte[] textStart = new byte[2];

            textStart[0] = data[4];
            textStart[1] = data[5];
            int textOffset = EndianMethods.GetLittleEndianIntTwofer(textStart, 0);

            //if(textOffset < data.Length)
            //{
            //    textStart[0] = data[3];
            //    textStart[1] = data[4];
            //    textOffset = EndianMethods.GetBigEndianIntTwofer(textStart, 0);
            //}

            if (name == "qd")
            {
                // Breakpoint to analyse qd's allocation of items
            }

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
            var searchDialogue = new byte[] { 0x40 };
            List<int> usedTexts = new List<int>();
            for (int k = 0; k < maxSearchRangeDialogue; k++)
            {
                currentDialogue[0] = data[k];       // OpCode 0x40            

                // If a match is found, we can call a method to change the string
                if (searchDialogue.SequenceEqual(currentDialogue))
                {
                    // Check that next part of opcode is a valid value of 0-3
                    if (data[k + 1] < 4)
                    {
                        // Then check that the dialogue ID is a valid value
                        if (data[k + 2] <= textCount)
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


            Random rnd = new Random();
            int r = 0; // Iterates new item string
            int c = 0; // Iterates data when searching for item opcodes
            string itemFileLocation = "";
            string materiaFileLocation = Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin23";
            var materiaNames = MateriaStrings.GetMateriaStrings(materiaFileLocation);

            var terminator = new byte[1];
            int offset = 0;
            var currentString = new byte[10];
            var maxSearchRangeString = data.Length - 9;

            var searchItem = new byte[] { 0x58, 0x00, 0x01 };
            var searchFinalItem = new byte[] { 0x58, 0x00, 0x02 };
            var currentItem = new byte[3];
            var maxSearchRangeItem = data.Length - 5;
            bool matchFound = false;

            byte[] oldName = new byte[40];
            int oldMateriaID = 0;
            int oldItemID = 0;

            // Searches for string 'Received "' for items
            var searchString = new byte[] { 0x32, 0x45, 0x43, 0x45, 0x49, 0x56, 0x45, 0x44, 0x00, 0x02 };

            // Checks for '" Materia!' for Materia
            var appendSearchMateria = new byte[] { 0x02, 0x00, 0x2D, 0x41, 0x54, 0x45, 0x52, 0x49, 0x41, 0x01 };

            // Tracks the current Text ID based on how many terminators have been read; compared to List of Used Text IDs
            int textID = 0;

            // Checks for Materia first, then Items; Received will be overwritten by Materia check so no
            // overlap should occur.

            // If materia option is on
            for (var y = textOffset + (textCount * 2); y < maxSearchRangeString; y++)
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
                    textID++;
                    offset = y + 1;
                }

                if (appendSearchMateria.SequenceEqual(currentString))
                {
                    if (usedTexts.Contains(textID))
                    {
                        int newMateriaID = rnd.Next(91); // Selects new Item ID
                        while (newMateriaID > 104 && newMateriaID < 128)
                        {
                            newMateriaID = rnd.Next(91); // Selects new Item ID
                        }

                        int countCharacters = offset + 10;
                        int z = 0;
                        // Get old Materia name and match it to its ID
                        while(data[countCharacters] != 0x02)
                        {
                            oldName[z] = data[countCharacters];
                            z++; countCharacters++;
                        }

                        oldMateriaID = MateriaStrings.GetMateriaID(oldName, materiaFileLocation);

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
                        usedTexts.Remove(textID);
                    }
                }
            }
            r = 0;
            c = 0;
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
                }

                // Match found for the string in this chunk
                if (searchString.SequenceEqual(currentString))
                {
                    if (usedTexts.Contains(textID))
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

                        // Get a list of the item names (for this ID's group)
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

                            // MatchFound stops the process from continuing as this would hit the other item opcodes and incorrectly change them
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
                        usedTexts.Remove(textID);
                    }
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

                // If a match is found, we can call a method to change the string
                if (searchFinalItem.SequenceEqual(currentItem))
                {
                    c++;
                    c++;
                    c++;
                    c++;
                    data[c] = 0x01; c++; // Can change this if user specified it
                }
            }
            return data;
        }
    }
}
