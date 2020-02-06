using _7hFlevel.FieldDictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _7hFlevel.Indexing
{
    public class FieldModels
    {
        public static string RandomSwap()
        {
            var typeIAnimIndex = TypeISkeleton.AssignAnims();
            var typeIIAnimIndex = TypeIISkeleton.AssignAnims();
            var typeOAnimIndex = TypeOSkeleton.AssignAnims();
            var uniqueAnimIndex = UniqueSkeleton.AssignAnims();

            // Convert array to a Lookup
            var lookup = typeIAnimIndex.ToLookup(k => k.Key, v => v.Value);

            // Retrieve random string from entry "AAAA"
            var entry = lookup["AAAA"];
            var rand = new Random();
            var max = entry.Count();
            var ans = entry.Skip(rand.Next(max)).First();        
            return ans;
        }

        public static string ConsistentRandomSwap()
        {
            // Swaps Model IDs and tries to make it consistent across all fields.
            // For instance, Cloud swaps to Air Buster on every field rather than just one field

            return "TEST";
        }

        public static string RandomAnimSwap()
        {
            // Randomly allocates an anim
            Random rnd = new Random();
            string[] nameArray = { "AAAA", "ABAA" };
            int x = rnd.Next(nameArray.Length);
            string pick = nameArray[x];
            return pick;
        }

        public static string MatchedAnimSwap()
        {
            // This intends to replace anims specific to the Model ID.
            // Will result in less jumbling.
            return "TEST";
        }
    }
}
