using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.FieldDictionaries
{
    public class AllAnims
    {
        public static string AssignAnim()
        {
            List<string> index = new List<string> {
                "AAFE",
                "AAGA"
            };
            Random rnd = new Random();
            int picker = rnd.Next(index.Count);
            return index[picker];
        }
    }
}
