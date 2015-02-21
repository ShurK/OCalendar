using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Threading;

namespace OvulationCalendar
{
    public partial class CreateUsersWindow : Form
    {
        MainForm mainForm;
        public CreateUsersWindow()
        {
            InitializeComponent();
        }

        public CreateUsersWindow(MainForm mf)
        {
            this.mainForm = mf;
            CultureInfo myCultureInfo = new CultureInfo("en-US");           
            Thread.CurrentThread.CurrentCulture = myCultureInfo;
            Thread.CurrentThread.CurrentUICulture = myCultureInfo;
            InitializeComponent();
        }

        private void NewUser_OK_Click(object sender, EventArgs e)
        {
            if (this.NameForNewUser.Text != null)
            {
                this.mainForm.createUser(this.NameForNewUser.Text);
                Properties.Settings.Default.CurrentUser = this.NameForNewUser.Text.ToString();
                Properties.Settings.Default.Save();
                this.Close();
                MessageBox.Show(Strings.NewUserCreateSuccess + " \r"+ Strings.AppRestartingRequired , Strings.Done, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Restart();
            }
        }

        private void NewUser_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
    }
}
