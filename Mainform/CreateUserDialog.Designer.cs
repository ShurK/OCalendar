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
            this.label1 = new System.Windows.Forms.Label();
            this.NameForNewUser = new System.Windows.Forms.TextBox();
            this.NewUser_OK = new System.Windows.Forms.Button();
            this.NewUser_Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Введите имя пользователя";
            // 
            // NameForNewUser
            // 
            this.NameForNewUser.Location = new System.Drawing.Point(33, 46);
            this.NameForNewUser.Name = "NameForNewUser";
            this.NameForNewUser.Size = new System.Drawing.Size(143, 20);
            this.NameForNewUser.TabIndex = 1;
            // 
            // NewUser_OK
            // 
            this.NewUser_OK.Location = new System.Drawing.Point(12, 83);
            this.NewUser_OK.Name = "NewUser_OK";
            this.NewUser_OK.Size = new System.Drawing.Size(75, 23);
            this.NewUser_OK.TabIndex = 2;
            this.NewUser_OK.Text = "Ok";
            this.NewUser_OK.UseVisualStyleBackColor = true;
            this.NewUser_OK.Click += new System.EventHandler(this.NewUser_OK_Click);
            // 
            // NewUser_Cancel
            // 
            this.NewUser_Cancel.Location = new System.Drawing.Point(120, 83);
            this.NewUser_Cancel.Name = "NewUser_Cancel";
            this.NewUser_Cancel.Size = new System.Drawing.Size(75, 23);
            this.NewUser_Cancel.TabIndex = 3;
            this.NewUser_Cancel.Text = "Отмена";
            this.NewUser_Cancel.UseVisualStyleBackColor = true;
            this.NewUser_Cancel.Click += new System.EventHandler(this.NewUser_Cancel_Click);
            // 
            // CreateUsersWindow
            // 
            this.AcceptButton = this.NewUser_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.NewUser_Cancel;
            this.ClientSize = new System.Drawing.Size(207, 118);
            this.ControlBox = false;
            this.Controls.Add(this.NewUser_Cancel);
            this.Controls.Add(this.NewUser_OK);
            this.Controls.Add(this.NameForNewUser);
            this.Controls.Add(this.label1);
            this.Name = "CreateUsersWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Новый пользователь";
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