using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.Indexing
{
    public class ItemStrings
    {
        public static Dictionary<int, byte[]> GetItemStrings(string itemStringsFilePath, int c)
        {
            FileStream fs = File.OpenRead(itemStringsFilePath);
            try
            {
                int r = 0; // Navigates byte[] currentItem
                int o = 0; // Iterates through headers
                int k = 0; // Navigates byte[] data

                if (c < 128)
                {
                    c = 0;
                }
                else if (c < 256)
                {
                    c = 128;
                }
                else if (c < 288)
                {
                    c = 256;
                }
                else if (c < 320)
                {
                    c = 288;
                }

                bool terminate = false;
                var itemNames = new Dictionary<int, byte[]>();

                byte[] stringHeader = new byte[2];
                byte[][] currentItem = new byte[320][];
                byte[] data = new byte[fs.Length];

                fs.Read(data, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                while (!terminate)
                {
                    stringHeader[0] = data[o];
                    stringHeader[1] = data[o + 1];
                    k = EndianMethods.GetLittleEndianIntTwofer(stringHeader, 0);

                    currentItem[c] = new byte[40];
                    // Read string until terminator FF is hit
                    while (data[k + r] != 0xFF)
                    {
                        currentItem[c][r] = data[k + r];
                        r++;
                    }
                    currentItem[c][r] = 0xFF;
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


        public static int GetItemID(List<byte> oldItemName)
        {
            byte[] terminator = { 0xFF };
            int itemID = 0;

            FileStream item = File.OpenRead(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin19");
            byte[] dataItem = new byte[item.Length];
            item.Read(dataItem, 0, Convert.ToInt32(item.Length));
            item.Close();

            FileStream weapon = File.OpenRead(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin20");
            byte[] dataWeapon = new byte[weapon.Length];
            weapon.Read(dataWeapon, 0, Convert.ToInt32(weapon.Length));
            weapon.Close();

            FileStream armour = File.OpenRead(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin21");
            byte[] dataArmour = new byte[armour.Length];
            armour.Read(dataArmour, 0, Convert.ToInt32(armour.Length));
            armour.Close();

            FileStream accessory = File.OpenRead(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin22");
            byte[] dataAccessory = new byte[accessory.Length];
            accessory.Read(dataAccessory, 0, Convert.ToInt32(accessory.Length));
            accessory.Close();

            try
            {
                for (int r = 0; r < dataItem.Length; r++)
                {
                    if (dataItem.Skip(r).Take(terminator.Length).SequenceEqual(terminator))
                    {
                        itemID++;
                    }

                    if (dataItem.Skip(r).Take(oldItemName.Count).SequenceEqual(oldItemName))
                    {
                        return itemID;
                    }
                }

                itemID = 128;
                for (int o = 0; o < dataWeapon.Length; o++)
                {
                    if (dataWeapon.Skip(o).Take(terminator.Length).SequenceEqual(terminator))
                    {
                        itemID++;
                    }

                    if (dataWeapon.Skip(o).Take(oldItemName.Count).SequenceEqual(oldItemName))
                    {
                        return itemID;
                    }
                }

                itemID = 256;
                for (int c = 0; c < dataArmour.Length; c++)
                {
                    if (dataArmour.Skip(c).Take(terminator.Length).SequenceEqual(terminator))
                    {
                        itemID++;
                    }

                    if (dataArmour.Skip(c).Take(oldItemName.Count).SequenceEqual(oldItemName))
                    {
                        return itemID;
                    }
                }

                itemID = 288;
                for (int k = 0; k < dataAccessory.Length; k++)
                {
                    if (dataAccessory.Skip(k).Take(terminator.Length).SequenceEqual(terminator))
                    {
                        itemID++;
                    }

                    if (dataAccessory.Skip(k).Take(oldItemName.Count).SequenceEqual(oldItemName))
                    {
                        return itemID;
                    }
                }
                Trace.WriteLine("No matches in getting old Item ID - Used 0 as fallback");
                return 0;
            }
            catch
            {
                Trace.WriteLine("Error in getting old Item ID");
                return 0;
            }
        }

    }
}
