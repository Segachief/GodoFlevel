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
            // Change over to List<string>, then I can apply randomisation.
            // Dynamic is more for if you don't know what the data is going to be coming in

            // Dyyyyyyyyyyynamic dictionaryyyyyyyy
            // We can set anim types to have a certain value as well.
            // 'i' = Idle, 'w' = Walk, 'r' = Run, 'a' = Animate and so on
            // d is reserved for dynamic object naming

            dynamic d1 = new System.Dynamic.ExpandoObject(); // Dynamic object
            dynamic d2 = new System.Dynamic.ExpandoObject(); // Dynamic object
            dynamic d3 = new System.Dynamic.ExpandoObject(); // Dynamic object

            var dict = new Dictionary<string, dynamic>();
            dict["AAAA"] = d1; // Assign a dynamic object to the Dictionary Key
            dict["AAGB"] = d2; // Assign a dynamic object to the Dictionary Key
            dict["ABDA"] = d3; // Assign a dynamic object to the Dictionary Key

            dict["AAAA"].FooBar = new { i1 = "ACFE", i2 = "AAFE", i3 = "CAED", i4 = "HSGB" }; // Populate the dynamic object with the value(s)
            dict["AAGB"].FooBar = new { i1 = "ZZZZ", i2 = "XXXX", i3 = "CAED", i4 = "HSGB" }; // Populate the dynamic object with the value(s)
            dict["ABDA"].FooBar = new { i1 = "YYYY", i2 = "WWWW", i3 = "CAED", i4 = "HSGB" }; // Populate the dynamic object with the value(s)

            // Can retrieve like this: dict["AAAA"].FooBar.i1;
            //string newName = dict["ABDA"].FooBar.i1;

            string[] random = new string[] { "i1", "i2", "i3", "i4"};
            Random rnd = new Random();

            string newName = dict["ABDA"].FooBar;
            return newName;

            // Randomly allocates a Model ID
            //Random rnd = new Random();
            string[] nameArray = { "AAAA", "AAGB", "ABDA", "ABJB", "ACGD", "ADDA", "AEBC", "AEHD", "AFEC", "AFIE", "AGGB", "AHDF", "AIBA", "AIHB", "AJIF", };
            int x = rnd.Next(nameArray.Length);
            string pick = nameArray[x];
            return pick;
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
