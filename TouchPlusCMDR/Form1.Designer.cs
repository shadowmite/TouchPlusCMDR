namespace TouchPlusCMDR
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.IRLB = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.FilterLB = new System.Windows.Forms.Label();
            this.PictureBN = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button6 = new System.Windows.Forms.Button();
            this.FiltersBN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 20);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect Touch+";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 180);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(109, 20);
            this.button2.TabIndex = 1;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(138, 25);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(460, 264);
            this.listBox1.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 60);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(109, 20);
            this.button3.TabIndex = 3;
            this.button3.Text = "Disconnect";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Console Information:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 84);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(109, 20);
            this.button4.TabIndex = 5;
            this.button4.Text = "Toggle IR Lights";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // IRLB
            // 
            this.IRLB.AutoSize = true;
            this.IRLB.Location = new System.Drawing.Point(12, 283);
            this.IRLB.Name = "IRLB";
            this.IRLB.Size = new System.Drawing.Size(40, 13);
            this.IRLB.TabIndex = 6;
            this.IRLB.Text = "IR: ON";
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 108);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(109, 20);
            this.button5.TabIndex = 7;
            this.button5.Text = "Get Serial";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // FilterLB
            // 
            this.FilterLB.AutoSize = true;
            this.FilterLB.Location = new System.Drawing.Point(66, 283);
            this.FilterLB.Name = "FilterLB";
            this.FilterLB.Size = new System.Drawing.Size(66, 13);
            this.FilterLB.TabIndex = 8;
            this.FilterLB.Text = "FILTER: ON";
            // 
            // PictureBN
            // 
            this.PictureBN.Location = new System.Drawing.Point(12, 156);
            this.PictureBN.Name = "PictureBN";
            this.PictureBN.Size = new System.Drawing.Size(109, 20);
            this.PictureBN.TabIndex = 9;
            this.PictureBN.Text = "Take 3D Picture";
            this.PictureBN.UseVisualStyleBackColor = true;
            this.PictureBN.Click += new System.EventHandler(this.PictureBN_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 36);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(109, 20);
            this.button6.TabIndex = 10;
            this.button6.Text = "Unlock Only";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.UnlockOnly_Click);
            // 
            // FiltersBN
            // 
            this.FiltersBN.Location = new System.Drawing.Point(12, 132);
            this.FiltersBN.Name = "FiltersBN";
            this.FiltersBN.Size = new System.Drawing.Size(109, 20);
            this.FiltersBN.TabIndex = 11;
            this.FiltersBN.Text = "Filters";
            this.FiltersBN.UseVisualStyleBackColor = true;
            this.FiltersBN.Click += new System.EventHandler(this.FiltersBN_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 305);
            this.Controls.Add(this.FiltersBN);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.PictureBN);
            this.Controls.Add(this.FilterLB);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.IRLB);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Touch+ Commander";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label IRLB;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label FilterLB;
        private System.Windows.Forms.Button PictureBN;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button FiltersBN;
    }
}

