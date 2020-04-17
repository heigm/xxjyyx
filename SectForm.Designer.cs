namespace xxjjyx
{
    partial class SectForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.processExt1 = new xxjjyx.UCControl.ProcessExt();
            this.resShow1 = new xxjjyx.UCControl.ResShow();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(318, 250);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "弟子";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "正邪：";
            // 
            // processExt1
            // 
            this.processExt1.BackColor = System.Drawing.Color.Transparent;
            this.processExt1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.processExt1.CornerRadius = 5;
            this.processExt1.FillColor = System.Drawing.Color.Transparent;
            this.processExt1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.processExt1.IsRadius = true;
            this.processExt1.IsShowRect = false;
            this.processExt1.Location = new System.Drawing.Point(191, 13);
            this.processExt1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.processExt1.MaxValue = 500;
            this.processExt1.MinValue = -500;
            this.processExt1.Name = "processExt1";
            this.processExt1.RectColor = System.Drawing.Color.Blue;
            this.processExt1.RectWidth = 1;
            this.processExt1.Size = new System.Drawing.Size(203, 19);
            this.processExt1.TabIndex = 3;
            this.processExt1.Value = 0;
            // 
            // resShow1
            // 
            this.resShow1.Location = new System.Drawing.Point(3, 161);
            this.resShow1.Name = "resShow1";
            this.resShow1.Size = new System.Drawing.Size(400, 87);
            this.resShow1.TabIndex = 5;
            // 
            // SectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 285);
            this.Controls.Add(this.resShow1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.processExt1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "SectForm";
            this.Text = "SectForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private UCControl.ProcessExt processExt1;
        private System.Windows.Forms.Label label2;
        private UCControl.ResShow resShow1;
    }
}