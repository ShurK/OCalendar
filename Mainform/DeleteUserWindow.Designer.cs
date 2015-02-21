namespace OvulationCalendar
{
    partial class DeleteUserWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeleteUserWindow));
            this.listBox_Users = new System.Windows.Forms.ListBox();
            this.DeleteUser_Button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox_Users
            // 
            resources.ApplyResources(this.listBox_Users, "listBox_Users");
            this.listBox_Users.FormattingEnabled = true;
            this.listBox_Users.Name = "listBox_Users";
            // 
            // DeleteUser_Button
            // 
            resources.ApplyResources(this.DeleteUser_Button, "DeleteUser_Button");
            this.DeleteUser_Button.Name = "DeleteUser_Button";
            this.DeleteUser_Button.UseVisualStyleBackColor = true;
            this.DeleteUser_Button.Click += new System.EventHandler(this.DeleteUser_Button_Click);
            // 
            // DeleteUserWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DeleteUser_Button);
            this.Controls.Add(this.listBox_Users);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteUserWindow";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Users;
        private System.Windows.Forms.Button DeleteUser_Button;
    }
}