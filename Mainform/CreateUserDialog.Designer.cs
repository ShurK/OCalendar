namespace OvulationCalendar
{
    partial class CreateUsersWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateUsersWindow));
            this.label1 = new System.Windows.Forms.Label();
            this.NameForNewUser = new System.Windows.Forms.TextBox();
            this.NewUser_OK = new System.Windows.Forms.Button();
            this.NewUser_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // NameForNewUser
            // 
            resources.ApplyResources(this.NameForNewUser, "NameForNewUser");
            this.NameForNewUser.Name = "NameForNewUser";
            // 
            // NewUser_OK
            // 
            resources.ApplyResources(this.NewUser_OK, "NewUser_OK");
            this.NewUser_OK.Name = "NewUser_OK";
            this.NewUser_OK.UseVisualStyleBackColor = true;
            this.NewUser_OK.Click += new System.EventHandler(this.NewUser_OK_Click);
            // 
            // NewUser_Cancel
            // 
            resources.ApplyResources(this.NewUser_Cancel, "NewUser_Cancel");
            this.NewUser_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.NewUser_Cancel.Name = "NewUser_Cancel";
            this.NewUser_Cancel.UseVisualStyleBackColor = true;
            this.NewUser_Cancel.Click += new System.EventHandler(this.NewUser_Cancel_Click);
            // 
            // CreateUsersWindow
            // 
            this.AcceptButton = this.NewUser_OK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.NewUser_Cancel;
            this.ControlBox = false;
            this.Controls.Add(this.NewUser_Cancel);
            this.Controls.Add(this.NewUser_OK);
            this.Controls.Add(this.NameForNewUser);
            this.Controls.Add(this.label1);
            this.Name = "CreateUsersWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameForNewUser;
        private System.Windows.Forms.Button NewUser_OK;
        private System.Windows.Forms.Button NewUser_Cancel;
    }
}