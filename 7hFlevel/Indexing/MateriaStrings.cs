using System;
using System.Collections.Generic;
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

        public static Dictionary<int> GetMateriaID(byte[] oldMateriaName, string materiaStringsFilePath)
        {
            FileStream fs = File.OpenRead(materiaStringsFilePath);
            try
            {
                int r = 0; // Navigates byte[] currentItem
                int o = 0; // Iterates through headers
                int c = 0; // Materia ID
                int k = 0; // Navigates byte[] data

                byte[] terminator = new byte[1];
                int materiaID = 0;

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
                //return oldMateriaID;

                for (var y = k; y < data.Length; y++)
                {
                    // What could do is, have a var that looks for FF while we search and records its offset.
                    // That way can jump to start of the string that has matched.

                    terminator[0] = data[y];

                    oldMateriaName[0] = data[y];
                    oldMateriaName[1] = data[y + 1];
                    oldMateriaName[2] = data[y + 2];

                    // Tracks location of the terminator, and moves up 1 to start of next string
                    if (terminator[0] == 0xFF)
                    {
                        materiaID++;
                    }

                    if (appendSearchMateria.SequenceEqual(oldMateriaName))
                    {


                    }
            finally
            {
                fs.Close();
            }
        }
    }
}
