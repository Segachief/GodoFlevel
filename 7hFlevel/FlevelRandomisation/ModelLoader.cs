using _7hFlevel.Helper;
using _7hFlevel.Indexing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _7hFlevel.FlevelRandomisation
{
    public class ModelLoader
    {
        public static byte[] SwapFieldModels(byte[] data)
        {

            int r = 0; // Iteration through model entries
            int o = 0; // Array index
            int c = 0; // Iteration through model's anims

            // Model Loader header
            // 0x00: Always 0
            // 0x02: Model Count
            // 0x04: Model Scale (unused)
            // 0x06: Model Loader Data starts

            try
            {
                // Get the number of models in this field
                byte[] modelCountByte = new byte[2];
                modelCountByte[0] = data[o + 2];
                modelCountByte[1] = data[o + 3];
                int modelCount = EndianMethods.GetLittleEndianIntTwofer(modelCountByte, 0);
                o += 6; // Skip data position past the header

                while (r < modelCount)
                {
                    // Take size of model name and convert it into an int for array index
                    byte[] modelNameSizeByte = new byte[2];
                    modelNameSizeByte[0] = data[o];
                    modelNameSizeByte[1] = data[o + 1];
                    int modelNameSize = EndianMethods.GetLittleEndianIntTwofer(modelNameSizeByte, 0);
                    o += 2;

                    // Skip 2-byte unknown value
                    o += 2;

                    // Jump past model name string to HRC location
                    o += modelNameSize;

                    // Read the current .HRC ID
                    byte[] currentHRCBytes = new byte[4];
                    currentHRCBytes[0] = data[o];
                    currentHRCBytes[1] = data[o + 1];
                    currentHRCBytes[2] = data[o + 2];
                    currentHRCBytes[3] = data[o + 3];

                    // ASCII conversion - string from .HRC bytes - Output: "AAAA"
                    string currentHRC = @"" + Encoding.ASCII.GetString(currentHRCBytes, 0, currentHRCBytes.Length) + @"";

                    // If no options on, the current HRC will be used
                    string newHRC = currentHRC;

                    // temp var for testing, will be fed through to method from Form later
                    bool rndModelSwap = false;

                    // Complete Random Swap
                    if (rndModelSwap)
                    {
                        newHRC = FieldModels.RandomModelSwap();
                    }

                    // Converts the returned string into bytes
                    byte[] newHRCBytes = ConvertString.GetNameBytes(newHRC);

                    // Writes the new bytes to the .HRC
                    data[o] = newHRCBytes[0];
                    data[o + 1] = newHRCBytes[1];
                    data[o + 2] = newHRCBytes[2];
                    data[o + 3] = newHRCBytes[3];
                    o += 8; // Skip the .HRC part of the string


                    // Model Scale - Definitely have this as an option
                    // This is actually a string; '512' but written in ascii. Bear that in mind.
                    o += 4;

                    // Count the number of anims for this model
                    byte[] animCountByte = new byte[2];
                    animCountByte[0] = data[o];
                    animCountByte[1] = data[o + 1];
                    int animCount = EndianMethods.GetLittleEndianIntTwofer(animCountByte, 0);
                    o += 2;

                    // Light/Shading data - Will probably not be modifying these values
                    o += 30;

                    // Anims - Each anim has the following:
                    // 0x00: Size of anim name string
                    // 0x02: Anim name string
                    // 0x02 + Size: Unknown, 2-byte value
                    while(c < animCount)
                    {
                        // Take size of anim name and convert it into an int for array index
                        byte[] animNameSizeByte = new byte[2];
                        animNameSizeByte[0] = data[o];
                        animNameSizeByte[1] = data[o + 1];
                        int animNameSize = EndianMethods.GetLittleEndianIntTwofer(animNameSizeByte, 0);
                        o += 2;

                        // Read the current .HRC ID
                        byte[] currentAnimBytes = new byte[4];
                        currentAnimBytes[0] = data[o];
                        currentAnimBytes[1] = data[o + 1];
                        currentAnimBytes[2] = data[o + 2];
                        currentAnimBytes[3] = data[o + 3];

                        // ASCII conversion - string from .HRC bytes - Output: "AAAA"
                        string currentAnim = @"" + Encoding.ASCII.GetString(currentAnimBytes, 0, currentAnimBytes.Length) + @"";

                        // If no options on, the current Anim will be used
                        string newAnim = currentAnim;

                        bool rndAnimSwap = false;
                        // Changes the Anim string - Matches it to current HRC's anim pool
                        if (rndAnimSwap == true)
                        {
                            newAnim = FieldModels.MatchedAnimSwap(currentAnim, newHRC);
                        }

                        bool animTest = false;
                        // Complete Random
                        if (animTest == true)
                        {
                            newAnim = FieldModels.RandomAnimSwap();
                        }

                        // Converts the string into bytes
                        byte[] newAnimBytes = ConvertString.GetNameBytes(newAnim);

                        // Writes the new bytes
                        data[o] = newAnimBytes[0];
                        data[o + 1] = newAnimBytes[1];
                        data[o + 2] = newAnimBytes[2];
                        data[o + 3] = newAnimBytes[3];
                        o += animNameSize; // Move past the anim name

                        // Unknown 2 bytes, skipped
                        o += 2;

                        c++;
                    }
                    c = 0;
                    r++;
                }
            }
            catch
            {
                MessageBox.Show("Flevel Chunk #3 (Model Loader) has encountered an issue; skipping current field");
            }
            return data;
        }
    }
}