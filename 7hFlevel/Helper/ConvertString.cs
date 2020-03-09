using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.Helper
{
    public class ConvertString
    {
        public static byte[] GetNameBytes(string newFile)
        {
            byte[] nameBytes = Encoding.ASCII.GetBytes(newFile); // Encodes the string into ASCII byte values

            return nameBytes;
        }
    }
}
