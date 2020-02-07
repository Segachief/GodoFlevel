/*
  This source is subject to the Microsoft Public License. See LICENSE.TXT for details.
  The original developer is Iros <irosff@outlook.com>
*/
using _7hFlevel.FlevelRandomisation;
using _7hFlevel.Indexing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace _7hFlevel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Stores the selected Chunk options
        private List<int> SectionsList = new List<int>();

        // Strings for directory locations (where flevel is: input, where to put chunks: output)
        // Array of int[] for selected chunks; the UI of this seems different to what I have, more advanced.
        private class ExtractArgs
        {
            public string Input, Output;
            public int[] Chunks;
        }

        // Method that chunks an flevel
        void bw_Decompress(object sender, DoWorkEventArgs e)
        {
            ExtractArgs ea = (ExtractArgs)e.Argument;
            using (var fs = new FileStream(ea.Input, System.IO.FileMode.Open))
            {
                var df = FF7Files.LoadLGP(fs, ea.Input);
                int file = 0;
                foreach (var item in df.Items)
                {
                    (sender as BackgroundWorker).ReportProgress(100 * file++ / df.Items.Count);
                    if (Path.GetExtension(item.Name).Length == 0)
                    {
                        byte[] ff = new byte[item.Length - 24];
                        fs.Position = item.Start + 24;
                        fs.Read(ff, 0, ff.Length);
                        var chunks = FieldFile.Unchunk(ff);
                        if (chunks.Count > 0)
                            foreach (int i in ea.Chunks)
                            {
                                string fn = System.IO.Path.Combine(ea.Output, Path.GetFileNameWithoutExtension(item.Name) + ".chunk." + i);
                                File.WriteAllBytes(fn, chunks[i - 1]);
                            }
                    }
                }
            }
        }

        // Method that chunks an flevel then re-assembles it
        void bw_Compress(object sender, DoWorkEventArgs e)
        {
            ExtractArgs ea = (ExtractArgs)e.Argument;
            using (var fs = new FileStream(ea.Input, FileMode.Open))
            {
                var df = FF7Files.LoadLGP(fs, ea.Input);
                int file = 0;
                List<byte[]> chunks = new List<byte[]>();
                string flev = Path.Combine(ea.Output, Path.GetFileName("newflevel.lgp"));

                // Apply ToC and CRC to the new flevel
                byte[] addStart = new byte[23301];
                fs.Position = 0;
                fs.Read(addStart, 0, addStart.Length);
                using (var appendStart = new FileStream(flev, FileMode.Append))
                {
                    appendStart.Write(addStart, 0, addStart.Length);
                    appendStart.Close();
                }

                int tocPointer = 36; // Holds pointer location for where to write new offset for ToC
                ulong fieldOffset = 0x5B05; // Holds offset value to write into tocPointer
                int fieldCount = 0; // Counts fields
                foreach (var item in df.Items)
                {
                    (sender as BackgroundWorker).ReportProgress(100 * file++ / df.Items.Count);
                    // This route is for field files
                    if (Path.GetExtension(item.Name).Length == 0
                        && item.Name != "maplist"
                        // These files 'shrink' when included; seem functional but excluded for now
                        && item.Name != "blackbgb"
                        && item.Name != "frcyo"
                        && item.Name != "fship_4"
                        && item.Name != "las0_8"
                        && item.Name != "las2_1"
                        && item.Name != "uutai1"
                        )
                    {
                        byte[] ff = new byte[item.Length - 24];
                        fs.Position = item.Start + 24;
                        fs.Read(ff, 0, ff.Length);
                        chunks = FieldFile.Unchunk(ff);

                        // >>>>
                        // Can adjust the uncompressed chunks here if need be before they go to recompression
                        // Randomisation tasks should be called here, triggered based on section #
                        // >>>>

                        // If using consistent allocation of models, then that logic should be done somewhere here so
                        // that all fields passing through have access to the newly assigned HRC strings and can pass
                        // them through to the rando logic.

                        // Sends Field Script chunk of field to be randomised
                        chunks[0] = FieldScript.ChangeItems(chunks[0]);

                        // Sends Model Loader chunk of field to be randomised
                        chunks[2] = ModelLoader.SwapFieldModels(chunks[2]);

                        // Recompresses the chunks into a field
                        var field = FieldFile.Chunk(chunks, item.Name);

                        // Skip the first ToC offset as this won't change
                        if (fieldCount != 0)
                        {
                            // Adds field length to pointer so we know where next section starts
                            tocPointer += 27;
                            byte[] byteOffset = EndianMethods.GetLittleEndianConvert(fieldOffset);
                            using (Stream stream = File.Open(flev, FileMode.Open))
                            {
                                stream.Position = tocPointer;
                                stream.Write(byteOffset, 0, 4);
                            }
                        }

                        // Takes the size of the chunked field; used to determine next offset for ToC
                        fieldOffset += (ulong)field.Length;
                        fieldCount++;

                        // Writes it into the new flevel
                        using (var stream = new FileStream(flev, FileMode.Append))
                        {
                            stream.Write(field, 0, field.Length);
                        }
                    }
                    // This route is for non-field files
                    else
                    {
                        byte[] field = new byte[item.Length];
                        fs.Position = item.Start;
                        fs.Read(field, 0, field.Length);

                        // Adds field length to pointer so we know where next section starts
                        tocPointer += 27;
                        byte[] byteOffset = EndianMethods.GetLittleEndianConvert(fieldOffset);
                        using (Stream stream = File.Open(flev, FileMode.Open))
                        {
                            stream.Position = tocPointer;
                            stream.Write(byteOffset, 0, 4);
                        }
                        // Takes the size of the misc file
                        fieldOffset += (ulong)field.Length;
                        fieldCount++;

                        // Writes it into the new flevel
                        using (var stream = new FileStream(flev, FileMode.Append))
                        {
                            stream.Write(field, 0, field.Length);
                        }
                    }
                }
                // Adds the final terminating string to the flevel
                byte[] terminate = new byte[] { 0x46, 0x49, 0x4E, 0x41, 0x4C, 0x20, 0x46, 0x41, 0x4E, 0x54, 0x41, 0x53, 0x59, 0x37 };
                using (var finalStream = new FileStream(flev, FileMode.Append))
                {
                    finalStream.Write(terminate, 0, terminate.Length);
                }
            }
        }

        // Tracks progress, green gauge
        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // pbChunk is the progress bar on Form
            pbChunk.Value = e.ProgressPercentage;
        }

        // Confirms operation is finished
        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Complete!");
        }

        // Chunks an flevel
        private void BtnExtract_Click_1(object sender, EventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker() { WorkerReportsProgress = true };
            bw.DoWork += bw_Decompress;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync(new ExtractArgs()
            {
                Input = txtInput.Text,  // Form property; enter Flevel's directory location
                Output = txtOutput.Text, // Form property; where Chunks will be spat out
                Chunks = SectionsList.ToArray() // An array of selected options on the form
            });
        }

        // Chunks an flevel and then re-assembles it.
        private void BtnCompress_Click(object sender, EventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker() { WorkerReportsProgress = true };
            bw.DoWork += bw_Compress;
            bw.ProgressChanged += bw_ProgressChanged;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            bw.RunWorkerAsync(new ExtractArgs()
            {
                Input = txtInput.Text,  // Form property; enter Flevel's directory location
                Output = txtOutput.Text, // Form property; where Chunks will be spat out
                Chunks = SectionsList.ToArray() // An array of selected options on the form
            });
        }

        private void BtnInput_Click(object sender, EventArgs e)
        {
            txtInput.Text = "C:\\Users\\stewart.melville\\Downloads\\Default Files\\Default Files\\data\\field\\flevel.lgp";
        }

        private void BtnOutput_Click(object sender, EventArgs e)
        {
            txtOutput.Text = "C:\\Users\\stewart.melville\\Downloads\\Default Files\\Default Files\\data\\field\\Chunks";
        }

        // Handles checkbox options for sections
        void cb_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            if (c.Name.ToString() == "section1")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(1);
                }
                else
                {
                    SectionsList.Remove(1);
                }
            }
            else if (c.Name.ToString() == "section2")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(2);
                }
                else
                {
                    SectionsList.Remove(2);
                }
            }
            else if (c.Name.ToString() == "section3")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(3);
                }
                else
                {
                    SectionsList.Remove(3);
                }
            }
            else if (c.Name.ToString() == "section4")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(4);
                }
                else
                {
                    SectionsList.Remove(4);
                }
            }
            else if (c.Name.ToString() == "section5")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(5);
                }
                else
                {
                    SectionsList.Remove(5);
                }
            }
            else if (c.Name.ToString() == "section6")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(6);
                }
                else
                {
                    SectionsList.Remove(6);
                }
            }
            else if (c.Name.ToString() == "section7")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(7);
                }
                else
                {
                    SectionsList.Remove(7);
                }
            }
            else if (c.Name.ToString() == "section8")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(8);
                }
                else
                {
                    SectionsList.Remove(8);
                }
            }
            else if (c.Name.ToString() == "section9")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(9);
                }
                else
                {
                    SectionsList.Remove(9);
                }
            }
            else if (c.Name.ToString() == "sectionAll")
            {
                bool result = c.Checked;
                if (result)
                {
                    SectionsList.Add(1);
                    SectionsList.Add(2);
                    SectionsList.Add(3);
                    SectionsList.Add(4);
                    SectionsList.Add(5);
                    SectionsList.Add(6);
                    SectionsList.Add(7);
                    SectionsList.Add(8);
                    SectionsList.Add(9);
                }
                else
                {
                    SectionsList.Remove(1);
                    SectionsList.Remove(2);
                    SectionsList.Remove(3);
                    SectionsList.Remove(4);
                    SectionsList.Remove(5);
                    SectionsList.Remove(6);
                    SectionsList.Remove(7);
                    SectionsList.Remove(8);
                    SectionsList.Remove(9);
                }
            }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            //string test = FieldModels.RandomSwap();
            //MessageBox.Show(test);
        }
    }
}
