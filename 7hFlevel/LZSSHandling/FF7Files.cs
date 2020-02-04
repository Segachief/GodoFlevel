/*
  This source is subject to the Microsoft Public License. See LICENSE.TXT for details.
  The original developer is Iros <irosff@outlook.com>
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _7hFlevel
{
    public static class FF7Files
    {
        // Converts int to 4-byte value
        public static int ReadInt(this System.IO.Stream s)
        {
            byte[] data = new byte[4];
            s.Read(data, 0, 4);
            return BitConverter.ToInt32(data, 0);
        }

        // Converts unsigned Int to 4-byte value
        public static uint ReadUInt(this System.IO.Stream s)
        {
            byte[] data = new byte[4];
            s.Read(data, 0, 4);
            return BitConverter.ToUInt32(data, 0);
        }

        // Converts unsigned Short to 2-byte value
        public static ushort ReadUShort(this System.IO.Stream s)
        {
            byte[] data = new byte[2];
            s.Read(data, 0, 2);
            return BitConverter.ToUInt16(data, 0);
        }

        // Manages Audio, not sure for what though
        public static DataFile LoadSounds(string ff7folder)
        {
            int count = 0;
            DataFile df = new DataFile() { Filename = System.IO.Path.Combine(ff7folder, "sound\\audio.dat") };
            using (var fmtFile = new System.IO.FileStream(System.IO.Path.Combine(ff7folder, "sound\\audio.fmt"), System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                while (fmtFile.Position < fmtFile.Length)
                {
                    int length = fmtFile.ReadInt();
                    if (length == 0)
                    {
                        fmtFile.Seek(38, System.IO.SeekOrigin.Current);
                        continue;
                    }
                    df.Items.Add(new DataItem() { Name = "SOUND" + (count++), Length = length, Start = fmtFile.ReadInt() });
                    fmtFile.Seek(66, System.IO.SeekOrigin.Current);
                }
            }
            return df;
        }

        // Loads an LGP archive into a stream
        public static DataFile LoadLGP(System.IO.Stream fs, string file)
        {
            DataFile df = new DataFile() { Filename = file };

            // Checks that LGP starts with 00 then that SQUARESOFT is stated
            if (fs.ReadUShort() != 0)
                throw new Exception(file + " - not a valid LGP archive");
            byte[] header = new byte[10];
            fs.Read(header, 0, 10);
            if (!Encoding.ASCII.GetString(header).Equals("SQUARESOFT"))
                throw new Exception(file + " - not a valid LGP archive");

            // Takes the field name (20 bytes)
            // The offset it points to (4 byte integer)
            // Check code (1 byte)
            // Appended if file name is duplicate (2 byte short)
            int count = fs.ReadInt();
            byte[] fname = new byte[20];
            List<Tuple<string, uint, ushort, int>> files = new List<Tuple<string, uint, ushort, int>>();
            for (int i = 0; i < count; i++)
            {
                fs.Read(fname, 0, 20);          // Field Name
                uint offset = fs.ReadUInt();    // Offset
                fs.ReadByte();                  // Check Code
                ushort dupe = fs.ReadUShort();  // Dupe Name ID
                string lgpFile = Encoding.ASCII.GetString(fname);
                int nPos = lgpFile.IndexOf('\0');
                if (nPos >= 0) lgpFile = lgpFile.Substring(0, nPos);
                files.Add(new Tuple<string, uint, ushort, int>(lgpFile, offset, dupe, i));
            }

            if (files.Any(t => t.Item3 != 0))
            {
                // Skip lookup table
                fs.Seek(3600, System.IO.SeekOrigin.Current);
                ushort entries = fs.ReadUShort();
                byte[] pname = new byte[128];
                foreach (int i in Enumerable.Range(0, entries))
                {
                    foreach (int j in Enumerable.Range(0, fs.ReadUShort()))
                    {
                        fs.Read(pname, 0, 128);
                        ushort toc = fs.ReadUShort();
                        string ppname = Encoding.ASCII.GetString(pname);
                        ppname = ppname.Substring(0, ppname.IndexOf('\0'));
                        files[toc] = new Tuple<string, uint, ushort, int>(
                            ppname.Replace("/", "\\") + "\\" + files[toc].Item1,
                            files[toc].Item2,
                            files[toc].Item3,
                            files[toc].Item4
                        );
                    }
                }
            }

            // Derives start position and length for each field file
            df.Items = files.Select(t => new DataItem() { Name = t.Item1, Start = t.Item2, Index = t.Item4 }).ToList();
            foreach (var item in df.Items)
            {
                fs.Position = item.Start + 20; // Skips the name of the file on the field
                item.Length = fs.ReadInt() + 24; // Takes the file length header on the field
            }
            df.Freeze();
            return df;
        }

        // Appears to be unreferenced/unused
        // Opens the file based on passed directory name
        public static DataFile LoadLGP(string file)
        {
            using (var fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                return LoadLGP(fs, file);
            }
        }
    }
}
