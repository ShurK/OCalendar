namespace OvulationCalendar
{
    partial class splash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(splash));
            this.downloadLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.BigText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // downloadLabel
            // 
            this.downloadLabel.AutoSize = true;
            this.downloadLabel.BackColor = System.Drawing.SystemColors.HotTrack;
            this.downloadLabel.ForeColor = System.Drawing.SystemColors.Info;
            this.downloadLabel.Location = new System.Drawing.Point(10, 261);
            this.downloadLabel.Name = "downloadLabel";
            this.downloadLabel.Size = new System.Drawing.Size(57, 13);
            this.downloadLabel.TabIndex = 0;
            this.downloadLabel.Text = "Загрузка:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 278);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(476, 4);
            this.progressBar1.TabIndex = 1;
            // 
            // BigText
            // 
            this.BigText.AutoSize = true;
            this.BigText.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BigText.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.BigText.Location = new System.Drawing.Point(27, 59);
            this.BigText.Name = "BigText";
            this.BigText.Size = new System.Drawing.Size(451, 39);
            this.BigText.TabIndex = 2;
            this.BigText.Text = "Календарь женского цикла";
            this.BigText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.BackgroundImage = global::OvulationCalendar.Properties.Resources.splash;
            this.ClientSize = new System.Drawing.Size(500, 300);
            this.ControlBox = false;
            this.Controls.Add(this.BigText);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.downloadLabel);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "splash";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "splash";
            this.Load += new System.EventHandler(this.splash_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label downloadLabel;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label BigText;

    }
}