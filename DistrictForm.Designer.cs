namespace xxjjyx
{
    partial class DistrictForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.humanTalent1 = new xxjjyx.UCControl.ProgressBar();
            this.humanTalent5 = new xxjjyx.UCControl.ProgressBar();
            this.humanTalent2 = new xxjjyx.UCControl.ProgressBar();
            this.humanTalent4 = new xxjjyx.UCControl.ProgressBar();
            this.humanTalent3 = new xxjjyx.UCControl.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.fiveGraph1 = new xxjjyx.UCControl.FiveGraph();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 300);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(319, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.humanTalent1);
            this.groupBox1.Controls.Add(this.humanTalent5);
            this.groupBox1.Controls.Add(this.humanTalent2);
            this.groupBox1.Controls.Add(this.humanTalent4);
            this.groupBox1.Controls.Add(this.humanTalent3);
            this.groupBox1.Location = new System.Drawing.Point(321, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 218);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "灵气分布";
            // 
            // humanTalent1
            // 
            this.humanTalent1.Color = System.Drawing.Color.Yellow;
            this.humanTalent1.Location = new System.Drawing.Point(6, 20);
            this.humanTalent1.MaxValue = 100;
            this.humanTalent1.Name = "humanTalent1";
            this.humanTalent1.Size = new System.Drawing.Size(26, 190);
            this.humanTalent1.TabIndex = 10;
            this.humanTalent1.TextTag = "金";
            this.humanTalent1.Value = 66;
            // 
            // humanTalent5
            // 
            this.humanTalent5.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.humanTalent5.Location = new System.Drawing.Point(134, 20);
            this.humanTalent5.MaxValue = 100;
            this.humanTalent5.Name = "humanTalent5";
            this.humanTalent5.Size = new System.Drawing.Size(26, 190);
            this.humanTalent5.TabIndex = 14;
            this.humanTalent5.TextTag = "土";
            this.humanTalent5.Value = 56;
            // 
            // humanTalent2
            // 
            this.humanTalent2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.humanTalent2.Location = new System.Drawing.Point(38, 20);
            this.humanTalent2.MaxValue = 100;
            this.humanTalent2.Name = "humanTalent2";
            this.humanTalent2.Size = new System.Drawing.Size(26, 190);
            this.humanTalent2.TabIndex = 11;
            this.humanTalent2.TextTag = "木";
            this.humanTalent2.Value = 33;
            // 
            // humanTalent4
            // 
            this.humanTalent4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.humanTalent4.Location = new System.Drawing.Point(102, 20);
            this.humanTalent4.MaxValue = 100;
            this.humanTalent4.Name = "humanTalent4";
            this.humanTalent4.Size = new System.Drawing.Size(26, 190);
            this.humanTalent4.TabIndex = 13;
            this.humanTalent4.TextTag = "火";
            this.humanTalent4.Value = 84;
            // 
            // humanTalent3
            // 
            this.humanTalent3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.humanTalent3.Location = new System.Drawing.Point(70, 20);
            this.humanTalent3.MaxValue = 100;
            this.humanTalent3.Name = "humanTalent3";
            this.humanTalent3.Size = new System.Drawing.Size(26, 190);
            this.humanTalent3.TabIndex = 12;
            this.humanTalent3.TextTag = "水";
            this.humanTalent3.Value = 18;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(407, 312);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "人员";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "label2";
            // 
            // fiveGraph1
            // 
            this.fiveGraph1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fiveGraph1.Location = new System.Drawing.Point(319, 277);
            this.fiveGraph1.Name = "fiveGraph1";
            this.fiveGraph1.Size = new System.Drawing.Size(168, 18);
            this.fiveGraph1.TabIndex = 18;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(327, 312);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 19;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // DistrictForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 347);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.fiveGraph1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "DistrictForm";
            this.Text = "District";
            this.Load += new System.EventHandler(this.District_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private UCControl.ProgressBar humanTalent5;
        private UCControl.ProgressBar humanTalent4;
        private UCControl.ProgressBar humanTalent3;
        private UCControl.ProgressBar humanTalent2;
        private UCControl.ProgressBar humanTalent1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private UCControl.FiveGraph fiveGraph1;
        private System.Windows.Forms.Button button2;
    }
}