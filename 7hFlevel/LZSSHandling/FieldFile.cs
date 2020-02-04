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
    public static class FieldFile
    {
        public static List<byte[]> Unchunk(byte[] input)
        {
            var ms = new MemoryStream();
            var min = new MemoryStream(input);
            min.Position = 4;
            Lzs.Decode(min, ms);
            System.Diagnostics.Debug.WriteLine("FF:Unchunk:LZS expanded {0} bytes to {1} bytes", input.Length, ms.Length);
            byte[] scratch = new byte[4];
            ms.Position = 2;
            ms.Read(scratch, 0, 4);
            int numsection = BitConverter.ToInt32(scratch, 0);
            System.Diagnostics.Debug.WriteLine("FF:Unchunk:{0} sections", numsection, 0);
            List<byte[]> sections = new List<byte[]>();
            foreach (int i in Enumerable.Range(0, numsection))
            {
                if (i == 8) // Actually section9, as we started counting from 0. Lovely.
                {
                    ms.Position = 6 + i * 4;
                    ms.Read(scratch, 0, 4);
                    ms.Position = BitConverter.ToInt32(scratch, 0);
                    ms.Read(scratch, 0, 4);
                    int len = BitConverter.ToInt32(scratch, 0);

                    // Section 9
                    byte[] s = new byte[len];
                    ms.Read(s, 0, len);
                    sections.Add(s);

                    // Section '10' (terminator)
                    int finalLen = 14;
                    byte[] x = new byte[finalLen];
                    ms.Read(x, 0, finalLen);
                    sections.Add(x);
                }
                else
                {
                    ms.Position = 6 + i * 4;
                    ms.Read(scratch, 0, 4);
                    ms.Position = BitConverter.ToInt32(scratch, 0);
                    ms.Read(scratch, 0, 4);
                    int len = BitConverter.ToInt32(scratch, 0);

                    byte[] s = new byte[len];
                    ms.Read(s, 0, len);
                    sections.Add(s);
                }
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
                if (count == 9) // Section '10', is the terminating string
                {
                    offset += 4;
                    offset += s.Length;
                }
                else
                {
                    EndianMethods.WriteInt(ms, offset);
                    offset += 4;
                    offset += s.Length;
                }
                count++;
            }
            count = 0;

            // Writes the section contents
            foreach (var s in input)
            {
                if (count == 9)
                {
                    ms.Write(s, 0, s.Length);
                }
                else
                {
                    EndianMethods.WriteInt(ms, s.Length);
                    ms.Write(s, 0, s.Length);
                }
                count++;
            }

            // Re-encodes the data as LZSS format
            ms.Position = 0;
            var compress = new MemoryStream();
            Lzs.Encode(ms, compress);

            // Container for our compressed data, will pass this back to be appended to flevel
            byte[] data = new byte[compress.Length + 28]; // was 44

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
