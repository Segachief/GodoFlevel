namespace _7hFlevel
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bFLevel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.section9 = new System.Windows.Forms.CheckBox();
            this.section8 = new System.Windows.Forms.CheckBox();
            this.section7 = new System.Windows.Forms.CheckBox();
            this.section6 = new System.Windows.Forms.CheckBox();
            this.section5 = new System.Windows.Forms.CheckBox();
            this.section4 = new System.Windows.Forms.CheckBox();
            this.section3 = new System.Windows.Forms.CheckBox();
            this.section2 = new System.Windows.Forms.CheckBox();
            this.section1 = new System.Windows.Forms.CheckBox();
            this.pbChunk = new System.Windows.Forms.ProgressBar();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOutput = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnExtract = new System.Windows.Forms.Button();
            this.btnInput = new System.Windows.Forms.Button();
            this.txtInput = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnCompress = new System.Windows.Forms.Button();
            this.sectionAll = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(-102, 36);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Section #:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-102, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 23;
            this.label2.Text = "Output Folder:";
            // 
            // bFLevel
            // 
            this.bFLevel.Location = new System.Drawing.Point(382, -16);
            this.bFLevel.Margin = new System.Windows.Forms.Padding(2);
            this.bFLevel.Name = "bFLevel";
            this.bFLevel.Size = new System.Drawing.Size(26, 16);
            this.bFLevel.TabIndex = 21;
            this.bFLevel.Text = "...";
            this.bFLevel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-102, -15);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Select FLEVEL:";
            // 
            // section9
            // 
            this.section9.AutoSize = true;
            this.section9.Location = new System.Drawing.Point(325, 127);
            this.section9.Name = "section9";
            this.section9.Size = new System.Drawing.Size(132, 17);
            this.section9.TabIndex = 44;
            this.section9.Text = "Section 9 Background";
            this.section9.UseVisualStyleBackColor = true;
            this.section9.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // section8
            // 
            this.section8.AutoSize = true;
            this.section8.Location = new System.Drawing.Point(325, 104);
            this.section8.Name = "section8";
            this.section8.Size = new System.Drawing.Size(112, 17);
            this.section8.TabIndex = 43;
            this.section8.Text = "Section 8 Triggers";
            this.section8.UseVisualStyleBackColor = true;
            this.section8.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // section7
            // 
            this.section7.AutoSize = true;
            this.section7.Location = new System.Drawing.Point(325, 81);
            this.section7.Name = "section7";
            this.section7.Size = new System.Drawing.Size(123, 17);
            this.section7.TabIndex = 42;
            this.section7.Text = "Section 7 Encounter";
            this.section7.UseVisualStyleBackColor = true;
            this.section7.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // section6
            // 
            this.section6.AutoSize = true;
            this.section6.Location = new System.Drawing.Point(325, 58);
            this.section6.Name = "section6";
            this.section6.Size = new System.Drawing.Size(158, 17);
            this.section6.TabIndex = 41;
            this.section6.Text = "Section 6 TileMap (Unused)";
            this.section6.UseVisualStyleBackColor = true;
            this.section6.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // section5
            // 
            this.section5.AutoSize = true;
            this.section5.Location = new System.Drawing.Point(98, 150);
            this.section5.Name = "section5";
            this.section5.Size = new System.Drawing.Size(124, 17);
            this.section5.TabIndex = 40;
            this.section5.Text = "Section 5 Walkmesh";
            this.section5.UseVisualStyleBackColor = true;
            this.section5.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // section4
            // 
            this.section4.AutoSize = true;
            this.section4.Location = new System.Drawing.Point(98, 127);
            this.section4.Name = "section4";
            this.section4.Size = new System.Drawing.Size(107, 17);
            this.section4.TabIndex = 39;
            this.section4.Text = "Section 4 Palette";
            this.section4.UseVisualStyleBackColor = true;
            this.section4.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // section3
            // 
            this.section3.AutoSize = true;
            this.section3.Location = new System.Drawing.Point(98, 104);
            this.section3.Name = "section3";
            this.section3.Size = new System.Drawing.Size(139, 17);
            this.section3.TabIndex = 38;
            this.section3.Text = "Section 3 Model Loader";
            this.section3.UseVisualStyleBackColor = true;
            this.section3.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // section2
            // 
            this.section2.AutoSize = true;
            this.section2.Location = new System.Drawing.Point(98, 81);
            this.section2.Name = "section2";
            this.section2.Size = new System.Drawing.Size(141, 17);
            this.section2.TabIndex = 37;
            this.section2.Text = "Section 2 Camera Matrix";
            this.section2.UseVisualStyleBackColor = true;
            this.section2.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // section1
            // 
            this.section1.AutoSize = true;
            this.section1.Location = new System.Drawing.Point(98, 58);
            this.section1.Name = "section1";
            this.section1.Size = new System.Drawing.Size(162, 17);
            this.section1.TabIndex = 36;
            this.section1.Text = "Section 1 Field Script & Dialog";
            this.section1.UseVisualStyleBackColor = true;
            this.section1.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // pbChunk
            // 
            this.pbChunk.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbChunk.Location = new System.Drawing.Point(0, 256);
            this.pbChunk.Margin = new System.Windows.Forms.Padding(2);
            this.pbChunk.Name = "pbChunk";
            this.pbChunk.Size = new System.Drawing.Size(540, 12);
            this.pbChunk.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 58);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "Section #:";
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(498, 31);
            this.btnOutput.Margin = new System.Windows.Forms.Padding(2);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(26, 16);
            this.btnOutput.TabIndex = 33;
            this.btnOutput.Text = "...";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.BtnOutput_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(98, 30);
            this.txtOutput.Margin = new System.Windows.Forms.Padding(2);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(385, 20);
            this.txtOutput.TabIndex = 32;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 32);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "Output Folder:";
            // 
            // btnExtract
            // 
            this.btnExtract.Location = new System.Drawing.Point(98, 198);
            this.btnExtract.Margin = new System.Windows.Forms.Padding(2);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(138, 22);
            this.btnExtract.TabIndex = 30;
            this.btnExtract.Text = "Extract";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.BtnExtract_Click_1);
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(498, 6);
            this.btnInput.Margin = new System.Windows.Forms.Padding(2);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(26, 16);
            this.btnInput.TabIndex = 29;
            this.btnInput.Text = "...";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.BtnInput_Click);
            // 
            // txtInput
            // 
            this.txtInput.Location = new System.Drawing.Point(98, 5);
            this.txtInput.Margin = new System.Windows.Forms.Padding(2);
            this.txtInput.Name = "txtInput";
            this.txtInput.Size = new System.Drawing.Size(385, 20);
            this.txtInput.TabIndex = 28;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 7);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Select FLEVEL:";
            // 
            // btnCompress
            // 
            this.btnCompress.Location = new System.Drawing.Point(325, 198);
            this.btnCompress.Margin = new System.Windows.Forms.Padding(2);
            this.btnCompress.Name = "btnCompress";
            this.btnCompress.Size = new System.Drawing.Size(138, 22);
            this.btnCompress.TabIndex = 45;
            this.btnCompress.Text = "Compress";
            this.btnCompress.UseVisualStyleBackColor = true;
            this.btnCompress.Click += new System.EventHandler(this.BtnCompress_Click);
            // 
            // sectionAll
            // 
            this.sectionAll.AutoSize = true;
            this.sectionAll.Location = new System.Drawing.Point(325, 150);
            this.sectionAll.Name = "sectionAll";
            this.sectionAll.Size = new System.Drawing.Size(81, 17);
            this.sectionAll.TabIndex = 46;
            this.sectionAll.Text = "All Sections";
            this.sectionAll.UseVisualStyleBackColor = true;
            this.sectionAll.CheckedChanged += new System.EventHandler(this.cb_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 268);
            this.Controls.Add(this.sectionAll);
            this.Controls.Add(this.btnCompress);
            this.Controls.Add(this.section9);
            this.Controls.Add(this.section8);
            this.Controls.Add(this.section7);
            this.Controls.Add(this.section6);
            this.Controls.Add(this.section5);
            this.Controls.Add(this.section4);
            this.Controls.Add(this.section3);
            this.Controls.Add(this.section2);
            this.Controls.Add(this.section1);
            this.Controls.Add(this.pbChunk);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnOutput);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnExtract);
            this.Controls.Add(this.btnInput);
            this.Controls.Add(this.txtInput);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bFLevel);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bFLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox section9;
        private System.Windows.Forms.CheckBox section8;
        private System.Windows.Forms.CheckBox section7;
        private System.Windows.Forms.CheckBox section6;
        private System.Windows.Forms.CheckBox section5;
        private System.Windows.Forms.CheckBox section4;
        private System.Windows.Forms.CheckBox section3;
        private System.Windows.Forms.CheckBox section2;
        private System.Windows.Forms.CheckBox section1;
        private System.Windows.Forms.ProgressBar pbChunk;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.TextBox txtInput;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnCompress;
        private System.Windows.Forms.CheckBox sectionAll;
    }
}

