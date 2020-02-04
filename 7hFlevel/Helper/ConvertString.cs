using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.Helper
{
    public class ConvertString
    {
        public static byte[] GetNameBytes(string newHRC)
        {
            byte[] nameBytes = Encoding.ASCII.GetBytes(newHRC); // Encodes the string into ASCII byte values

            return nameBytes;
        }
    }
}
