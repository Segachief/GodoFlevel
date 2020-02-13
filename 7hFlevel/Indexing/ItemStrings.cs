using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.Indexing
{
    public class ItemStrings
    {
        public static Dictionary<int, byte[]> GetItemStrings(string itemStringsFilePath)
        {
            // Need to make a collection of all item strings, assigned to an ID.
            // Hopefully they'll be organised by ID like the items themselves.

            FileStream fs = File.OpenRead(itemStringsFilePath);
            try
            {
                int r = 0; // Navigates byte[] currentItem
                int o = 0; // Iterates through headers
                int c = 0; // Item ID
                int k = 0; // Navigates byte[] data

                bool terminate = false;
                var itemNames = new Dictionary<int, byte[]>();

                byte[] stringHeader = new byte[2];
                byte[][] currentItem = new byte[127][];
                byte[] data = new byte[fs.Length];

                fs.Read(data, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                while (!terminate)
                {
                    stringHeader[0] = data[o];
                    stringHeader[1] = data[o + 1];
                    k = EndianMethods.GetLittleEndianIntTwofer(stringHeader, 0);

                    currentItem[c] = new byte[20];
                    // Read string until terminator FF is hit
                    while (data[k + r] != 0xFF)
                    {
                        currentItem[c][r] = data[k + r];
                        r++;
                    }
                    currentItem[c][r] = 0xFF;
                    // Bug with C#, the reference is trapped within the loop so it's overwriting previous Values. Maddening.
                    itemNames.Add(c, currentItem[c]);

                    // Check if we've hit end of file; if next byte is FF then we have
                    if (data[k + r + 1] == 0xFF)
                    {
                        terminate = true;
                    }
                    else
                    {
                        o += 2; // Next header
                        c++;    // Next item ID
                        r = 0;  // Reset for next item string reading
                    }
                }
                return itemNames;
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
