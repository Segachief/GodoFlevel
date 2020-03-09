/*
  This source is subject to the Microsoft Public License. See LICENSE.TXT for details.
  The original developer is Iros <irosff@outlook.com>
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel
{
    static class EndianMethods
    {
        public static void WriteInt(byte[] data, int offset, int value)
        {
            data[offset + 0] = (byte)(value & 0xff);
            data[offset + 1] = (byte)((value >> 8) & 0xff);
            data[offset + 2] = (byte)((value >> 16) & 0xff);
            data[offset + 3] = (byte)((value >> 24) & 0xff);
        }
        public static void WriteUInt(byte[] data, int offset, uint value)
        {
            data[offset + 0] = (byte)(value & 0xff);
            data[offset + 1] = (byte)((value >> 8) & 0xff);
            data[offset + 2] = (byte)((value >> 16) & 0xff);
            data[offset + 3] = (byte)((value >> 24) & 0xff);
        }
        public static void WriteUShort(byte[] data, int offset, ushort value)
        {
            data[offset + 0] = (byte)(value & 0xff);
            data[offset + 1] = (byte)((value >> 8) & 0xff);
        }
        public static long ReadLong(this System.IO.Stream s)
        {
            byte[] data = new byte[8];
            s.Read(data, 0, 8);
            return BitConverter.ToInt64(data, 0);
        }
        public static void WriteInt(this System.IO.Stream s, int i)
        {
            var data = BitConverter.GetBytes(i);
            s.Write(data, 0, 4);
        }
        public static void WriteIntFinalSection(this System.IO.Stream s, int i)
        {
            var data = BitConverter.GetBytes(i);
            s.Write(data, 0, 4);
        }
        public static void WriteLong(this System.IO.Stream s, long L)
        {
            var data = BitConverter.GetBytes(L);
            s.Write(data, 0, 8);
        }
        public static void WriteUInt(this System.IO.Stream s, uint i)
        {
            var data = BitConverter.GetBytes(i);
            s.Write(data, 0, 4);
        }
        public static void WriteUShort(this System.IO.Stream s, ushort us)
        {
            var data = BitConverter.GetBytes(us);
            s.Write(data, 0, 2);
        }

        // Converts a ulong into a reversed 2-byte value
        public static byte[] GetLittleEndianConvert(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);

            // If it was big endian, reverse it
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }

        // Converts a byte[2] into an int
        public static int GetLittleEndianIntTwofer(byte[] data, int startIndex)
        {
            return (data[startIndex + 1] << 8)
                 | data[startIndex];
        }
    }
}