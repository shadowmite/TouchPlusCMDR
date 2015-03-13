namespace TouchPlusCMDR
{
    partial class Viewer
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.XYZLB = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.minHeightUD = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.maxWidthUD = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minHeightUD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWidthUD)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1280, 480);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(488, 503);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Reset Background";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(760, 506);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(697, 508);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Threshold:";
            // 
            // XYZLB
            // 
            this.XYZLB.AutoSize = true;
            this.XYZLB.Location = new System.Drawing.Point(24, 509);
            this.XYZLB.Name = "XYZLB";
            this.XYZLB.Size = new System.Drawing.Size(34, 13);
            this.XYZLB.TabIndex = 5;
            this.XYZLB.Text = "X,Y,Z";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(902, 509);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Min Height:";
            // 
            // minHeightUD
            // 
            this.minHeightUD.Location = new System.Drawing.Point(967, 506);
            this.minHeightUD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.minHeightUD.Name = "minHeightUD";
            this.minHeightUD.Size = new System.Drawing.Size(120, 20);
            this.minHeightUD.TabIndex = 6;
            this.minHeightUD.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.minHeightUD.ValueChanged += new System.EventHandler(this.minHeightUD_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1096, 509);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Max Width:";
            // 
            // maxWidthUD
            // 
            this.maxWidthUD.Location = new System.Drawing.Point(1161, 506);
            this.maxWidthUD.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.maxWidthUD.Name = "maxWidthUD";
            this.maxWidthUD.Size = new System.Drawing.Size(120, 20);
            this.maxWidthUD.TabIndex = 8;
            this.maxWidthUD.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.maxWidthUD.ValueChanged += new System.EventHandler(this.maxWidthUD_ValueChanged);
            // 
            // Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1307, 534);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.maxWidthUD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.minHeightUD);
            this.Controls.Add(this.XYZLB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Viewer";
            this.Text = "Viewer";
            this.Load += new System.EventHandler(this.Viewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minHeightUD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxWidthUD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label XYZLB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown minHeightUD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown maxWidthUD;
    }
}