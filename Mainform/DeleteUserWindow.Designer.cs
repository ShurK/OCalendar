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
            this.listBox_Users.FormattingEnabled = true;
            this.listBox_Users.Location = new System.Drawing.Point(22, 12);
            this.listBox_Users.Name = "listBox_Users";
            this.listBox_Users.Size = new System.Drawing.Size(206, 121);
            this.listBox_Users.TabIndex = 3;
            // 
            // DeleteUser_Button
            // 
            this.DeleteUser_Button.Location = new System.Drawing.Point(22, 138);
            this.DeleteUser_Button.Name = "DeleteUser_Button";
            this.DeleteUser_Button.Size = new System.Drawing.Size(75, 23);
            this.DeleteUser_Button.TabIndex = 4;
            this.DeleteUser_Button.Text = "Удалить";
            this.DeleteUser_Button.UseVisualStyleBackColor = true;
            this.DeleteUser_Button.Click += new System.EventHandler(this.DeleteUser_Button_Click);
            // 
            // DeleteUserWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(253, 173);
            this.Controls.Add(this.DeleteUser_Button);
            this.Controls.Add(this.listBox_Users);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteUserWindow";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Удалить пользователя";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Users;
        private System.Windows.Forms.Button DeleteUser_Button;
    }
}