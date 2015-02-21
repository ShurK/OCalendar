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
            resources.ApplyResources(this.downloadLabel, "downloadLabel");
            this.downloadLabel.BackColor = System.Drawing.SystemColors.HotTrack;
            this.downloadLabel.ForeColor = System.Drawing.SystemColors.Info;
            this.downloadLabel.Name = "downloadLabel";
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // BigText
            // 
            resources.ApplyResources(this.BigText, "BigText");
            this.BigText.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.BigText.Name = "BigText";
            // 
            // splash
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Desktop;
            this.BackgroundImage = global::OvulationCalendar.Properties.Resources.splash;
            this.ControlBox = false;
            this.Controls.Add(this.BigText);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.downloadLabel);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "splash";
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