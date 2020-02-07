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
        public static string RandomModelSwap()
        {
            // Randomly allocates an .HRC - Build a full HRC list
            return "TEST";
        }

        public static string MatchedAnimSwap(string oldAnim, string HRC)
        {
            // Swaps indiscriminately
            var rnd = new Random();
            string newAnim = oldAnim; // If no new string assigned, then old one goes back
            var typeIAnimIndex = TypeISkeleton.AssignAnims();       // Cloud and most NPCs use this skeleton
            var typeIIAnimIndex = TypeIISkeleton.AssignAnims();     // Characters with an extra part for long hair use this one
            var typeIIIAnimIndex = TypeIIISkeleton.AssignAnims();   // Older version of the Type 1, still used by Barret and Cid
            var uniqueAnimIndex = UniqueSkeleton.AssignAnims();     // Characters with unique skeletons go in here
            //var objectAnimIndex;                                  // This is for doors, chests, etc.


            // Have an option check here that segways for two types of Matched behaviour
            // A) Matched to new HRC
            // B) Matched to skeleton (any from within; exclude uniques/objects from this)

            // Convert array to a Lookup and check for HRC key matches
            var lookupI = typeIAnimIndex.ToLookup(k => k.Key, v => v.Value);
            var matchesI = lookupI[HRC].ToList();
            if(matchesI.Count == 0)
            {
                // No matches here
            }
            else
            {
                // Go to a random index within the counted entries that match the HRC and assign it
                newAnim = matchesI.Skip(rnd.Next(matchesI.Count)).First();
            }

            var lookupII = typeIIAnimIndex.ToLookup(k => k.Key, v => v.Value);
            var matchesII = lookupII[HRC].ToList();
            if (matchesII.Count == 0)
            {
                // No matches here
            }
            else
            {
                newAnim = matchesII.Skip(rnd.Next(matchesII.Count)).First();
            }

            var lookupIII = typeIIIAnimIndex.ToLookup(k => k.Key, v => v.Value);
            var matchesIII = lookupIII[HRC].ToList();
            if (matchesIII.Count == 0)
            {
                // No matches here
            }
            else
            {
                newAnim = matchesIII.Skip(rnd.Next(matchesIII.Count)).First();
            }

            var lookupUnq = uniqueAnimIndex.ToLookup(k => k.Key, v => v.Value);
            var matchesUnq = lookupUnq[HRC].ToList();
            if (matchesUnq.Count == 0)
            {
                // No matches here
            }
            else
            {
                newAnim = matchesUnq.Skip(rnd.Next(matchesUnq.Count)).First();
            }

            //var lookupObj = objectAnimIndex.ToLookup(k => k.Key, v => v.Value);
            //var matchesObj = lookupObj[oldHRC].ToList();
            //if (matchesObj.Count == 0)
            //{
            //    
            //}
            //else
            //{
                //newHRC = matchesObj.Skip(rnd.Next(matchesObj.Count)).First();
            //}

            // Retrieve random string to replace old one
            var entry = lookupI[HRC]; // Look up this key
            var max = entry.Count(); // How many entries had the specified key
            var ans = entry.Skip(rnd.Next(max)).First(); // Go to a random index within the counted entries
            return ans;
        }

        public static string ConsistentModelSwap()
        {
            // Swaps Model IDs and tries to make it consistent across all fields.
            // For instance, Cloud swaps to Air Buster on every field rather than just one field

            // This should create a Dictionary of Key<Old HRC> and Value<New HRC> they were assigned to.
            // If HRC matches the one fed in, then take its New HRC.
            // Otherwise, allocate a new Key and Value.

            return "TEST";
        }

        public static string RandomAnimSwap()
        {
            // Randomly allocates an anim - Build a full anim list
            Random rnd = new Random();
            string[] nameArray = { "AAAA", "ABAA" };
            int x = rnd.Next(nameArray.Length);
            string pick = nameArray[x];
            return pick;
        }

        
    }
}
