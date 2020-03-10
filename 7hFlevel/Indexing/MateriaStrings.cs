﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.Indexing
{
    public class MateriaStrings
    {
        public static Dictionary<int, byte[]> GetMateriaStrings(string materiaStringsFilePath)
        {
            FileStream fs = File.OpenRead(materiaStringsFilePath);
            try
            {
                int r = 0; // Navigates byte[] currentItem
                int o = 0; // Iterates through headers
                int c = 0; // Materia ID
                int k = 0; // Navigates byte[] data

                bool terminate = false;
                var materiaNames = new Dictionary<int, byte[]>();

                byte[] stringHeader = new byte[2];
                byte[][] currentMateria = new byte[96][];
                byte[] data = new byte[fs.Length];

                fs.Read(data, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                while (!terminate)
                {
                    stringHeader[0] = data[o];
                    stringHeader[1] = data[o + 1];
                    k = EndianMethods.GetLittleEndianIntTwofer(stringHeader, 0);

                    currentMateria[c] = new byte[40];
                    // Read string until terminator FF is hit
                    while (data[k + r] != 0xFF)
                    {
                        currentMateria[c][r] = data[k + r];
                        r++;
                    }
                    currentMateria[c][r] = 0xFF;
                    materiaNames.Add(c, currentMateria[c]);

                    // Check if we've hit end of file; if next byte is FF then we have
                    if (data[k + r + 1] == 0xFF)
                    {
                        terminate = true;
                    }
                    else
                    {
                        o += 2; // Next header
                        c++;    // Next Materia ID
                        r = 0;  // Reset for next Materia string reading
                    }
                }
                return materiaNames;
            }
            finally
            {
                fs.Close();
            }
        }

        public static int GetMateriaID(List<byte> oldMateriaName)
        {
            try
            {
                byte[] terminator = { 0xFF };
                int materiaID = 0;

                FileStream fs = File.OpenRead(Directory.GetCurrentDirectory() + "\\Kernel Strings\\kernel.bin23");
                byte[] data = new byte[fs.Length];
                fs.Read(data, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                for (int i = 0; i < data.Length; i++)
                {
                    if (data.Skip(i).Take(terminator.Length).SequenceEqual(terminator))
                    {
                        materiaID++;
                    }

                    if (data.Skip(i).Take(oldMateriaName.Count).SequenceEqual(oldMateriaName))
                    {
                        return materiaID;
                    }
                }
                Trace.WriteLine("No matches in getting old Materia ID - Used 0 as fallback");
                return 0;
            }
            catch
            {
                Trace.WriteLine("Error in getting old Materia ID");
                return 0;
            }
        }
    }
}