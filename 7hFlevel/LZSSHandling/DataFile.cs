/*
  This source is subject to the Microsoft Public License. See LICENSE.TXT for details.
  The original developer is Iros <irosff@outlook.com>
*/
using System.Collections.Generic;
using System.Linq;

namespace _7hFlevel
{
    public class DataFile
    {
        public List<DataItem> Items { get; set; }
        public string Filename { get; set; }

        public DataFile()
        {
            Items = new List<DataItem>();
        }

        public void Freeze()
        {
            Items = Items.OrderBy(i => i.Start).ToList();
        }

        public DataItem Get(int offset)
        {
            if (offset < Items[0].Start) return null;
            int min = 0, max = Items.Count - 1;
            while (min <= max)
            {
                int check = (min + max + 1) / 2;
                if (offset < Items[check].Start)
                    max = check - 1;
                else if (offset >= (Items[check].Start + Items[check].Length))
                    min = check + 1;
                else
                {
                    return Items[check];
                }
            }
            if (min > 0) System.Diagnostics.Debug.WriteLine("Mid read from " + Filename + " at offset " + offset);
            return null;
        }
    }
}
