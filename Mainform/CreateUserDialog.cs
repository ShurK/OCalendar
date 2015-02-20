using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
                MessageBox.Show("Пользователь был создан и активирован. Нужно перезапустить \rпрограмму, чтобы изменения вступили в силу.", "Выполнено", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Restart();
            }
        }

        private void NewUser_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
      
    }
}
