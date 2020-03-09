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
        // TODO
        // Uniques should have their own Anim sets made, so if Consistent Model Swap is used then they can be given proper anims.
        // Probably do this for Objects too.

        public static string RandomModelSwap(Random rnd)
        {
            // Randomly allocates an .HRC
            string newHRC = AllSkeletons.AssignModel(rnd);
            return newHRC;
        }

        public static string RandomAnimSwap(Random rnd)
        {
            // Randomly allocates an Anim
            string newAnim = AllAnims.AssignAnim(rnd);
            return newAnim;
        }

        public static string ConsistentModelSwap()
        {
            // Swaps Model IDs persistently across all fields

            // In ModelLoader, a Dictionary should be built and passed to this method
            // It should be built using two Lists; a starting list of all models and a shuffled list of all models.
            // Starting List becomes the Key, shuffled List becomes the Value; this way, all Models are also unique (no duplicates)

            // This should create a Dictionary of Key<Old HRC> and Value<New HRC> they were assigned to.
            // If HRC matches the one fed in, then take its New HRC.
            // Otherwise, allocate a new Key and Value.

            return "TEST";
        }

        public static string MatchedModelSwap(string oldHRC, string HRC)
        {
            // Swaps Models within their own HRC skeleton group.
            // Uniques and Objects can be toggled on, but will be jumbled
            return "TEST";
        }

        public static string MatchedAnimSwap(string oldAnim, string HRC)
        {
            // Swaps Anims from within the source HRC's skeleton group
            // Uniques and Objects can be toggled on, but will be jumbled
            var rnd = new Random();
            string newAnim = oldAnim; // If no new string assigned, then old one goes back
            var typeIAnimIndex = TypeISkeletonAnims.AssignAnims();       // Cloud and most NPCs use this skeleton
            var typeIIAnimIndex = TypeIISkeletonAnims.AssignAnims();     // Characters with an extra part for long hair use this one
            var uniqueAnimIndex = UniqueSkeletonAnims.AssignAnims();     // Characters with unique skeletons go in here
            var objectAnimIndex = UniqueSkeletonAnims.AssignAnims();     // This is for doors, chests, etc.


            // Have an option check here that segways for two types of Matched behaviour
            // A) Matched to new HRC
            // B) Matched to skeleton (any from within; exclude uniques/objects from this)

            // Convert array to a Lookup and check for HRC key matches
            var lookupI = typeIAnimIndex.ToLookup(k => k.Key, v => v.Value);
            var matchesI = lookupI[HRC].ToList();
            if (matchesI.Count != 0)
            {
                // Go to a random index within the counted entries that match the HRC and assign it
                newAnim = matchesI.Skip(rnd.Next(matchesI.Count)).First();
            }
            else
            {
                // No matches here
            }

            var lookupII = typeIIAnimIndex.ToLookup(k => k.Key, v => v.Value);
            var matchesII = lookupII[HRC].ToList();
            if (matchesII.Count != 0)
            {
                newAnim = matchesII.Skip(rnd.Next(matchesII.Count)).First();
            }
            else
            {
                // No matches here
            }

            // Unique can't avoid jumbling, perhaps remove this for Matched Anim Swap
            var lookupUnq = uniqueAnimIndex.ToLookup(k => k.Key, v => v.Value);
            var matchesUnq = lookupUnq[HRC].ToList();
            if (matchesUnq.Count != 0)
            {
                newAnim = matchesUnq.Skip(rnd.Next(matchesUnq.Count)).First();
            }
            else
            {
                // No matches here
            }

            // Object can't avoid jumbling, perhaps remove this for Matched Anim Swap
            var lookupObj = objectAnimIndex.ToLookup(k => k.Key, v => v.Value);
            var matchesObj = lookupObj[HRC].ToList();
            if (matchesObj.Count != 0)
            {
                newAnim = matchesObj.Skip(rnd.Next(matchesObj.Count)).First();
            }
            else
            {
                // No matches here   
            }

            // If no match was found, newAnim still has value of oldAnim and returns it without change
            return newAnim;
        }
    }
}
