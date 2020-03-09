/*
  This source is subject to the Microsoft Public License. See LICENSE.TXT for details.
  The original developer is Iros <irosff@outlook.com>
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _7hFlevel
{
    public static class PSXFieldFile
    {
        public static List<byte[]> Unchunk(byte[] input, string dat)
        {
            var ms = new MemoryStream();
            var min = new MemoryStream(input);
            min.Position = 4;
            Lzs.Decode(min, ms);
            System.Diagnostics.Debug.WriteLine("FF:Unchunk:LZS expanded {0} bytes to {1} bytes", input.Length, ms.Length);
            byte[] scratch = new byte[4];
            byte[] all = new byte[ms.Length];
            int numsection = 6; // 7, but starts from 0
            System.Diagnostics.Debug.WriteLine("FF:Unchunk:{0} sections", numsection, 0);
            List<byte[]> sections = new List<byte[]>();
            foreach (int i in Enumerable.Range(0, numsection))
            {
                ms.Position = 0 + i * 4; // Jumps to next header offset (4-byte)
                ms.Read(scratch, 0, 4); // Reads the header offset for section start
                ms.Read(all, 0, (int)ms.Length); // Reads the header offset for section start
                using (var stream = new FileStream(dat, FileMode.Append))
                {
                    stream.Write(all, 0, all.Length);
                }
                ms.Position = BitConverter.ToInt32(scratch, 0); // Sets position to header offset
                ms.Read(scratch, 0, 4); // Reads the section header for section length
                int len = BitConverter.ToInt32(scratch, 0); // Converts section length into integer

                byte[] s = new byte[len]; // Creates a buffer equal to length
                ms.Read(s, 0, len); // Writes data to the buffer
                sections.Add(s); // Adds the section to a collection which will be returned at the end
            }
            return sections;
        }

        public static byte[] Chunk(List<byte[]> input, string name)
        {
            var ms = new MemoryStream();
            byte[] scratch = new byte[4];
            ms.Write(scratch, 0, 2);
            ms.Write(BitConverter.GetBytes(input.Count - 1), 0, 4);
            int offset = 0x2A;
            int count = 0;

            // Writes the section headers
            foreach (var s in input)
            {
                EndianMethods.WriteInt(ms, offset);
                offset += 4;
                offset += s.Length;
                count++;
            }
            count = 0;

            // Writes the section contents
            foreach (var s in input)
            {
                EndianMethods.WriteInt(ms, s.Length);
                ms.Write(s, 0, s.Length);
                count++;
            }

            // Re-encodes the data as LZSS format
            ms.Position = 0;
            var compress = new MemoryStream();
            Lzs.Encode(ms, compress);

            // Container for our compressed data, will pass this back to be appended to flevel
            byte[] data = new byte[compress.Length + 28];

            // Adds field name 20bytes as a header
            byte[] nameBytes = Encoding.ASCII.GetBytes(name);
            Array.Resize(ref nameBytes, 24);
            int z = 0;
            while (z < 24)
            {
                data[z] = nameBytes[z];
                z++;
            }

            // Writes in encoded data - Offset to avoid losing the field title header
            compress.Position = 0;
            compress.Read(data, 28, (int)compress.Length);

            // Updates the field header for file length; jumps from byte after header to start of next field's name
            EndianMethods.WriteInt(data, 20, (int)compress.Length + 4);
            EndianMethods.WriteInt(data, 24, (int)compress.Length + 0);

            return data;
        }
    }
}