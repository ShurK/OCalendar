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
    public partial class Users : Form
    {
        MainForm mainForm;
        public Users()
        {
            InitializeComponent();
            listBox_Users.DoubleClick += new EventHandler(listBox_Users_DoubleClick);
        }

        public Users(MainForm mf)
        {
            this.mainForm = mf;
            InitializeComponent();
            foreach (string user in mainForm.users){
                listBox_Users.Items.Add(user);
                if (user == Properties.Settings.Default.CurrentUser) listBox_Users.SetSelected(listBox_Users.Items.IndexOf(user), true);
            }
            listBox_Users.DoubleClick += new EventHandler(listBox_Users_DoubleClick);            
        }

       

        private void listBox_Users_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int index = this.listBox_Users.IndexFromPoint();
            //if (index != System.Windows.Forms.ListBox.NoMatches)
           // {
            //    MessageBox.Show(index.ToString());
           // }
        }

        private void listBox_Users_DoubleClick(object sender, EventArgs e)
        {           
                change_user();          
        }

        private void change_user() {
            if (listBox_Users.SelectedItem != null)
            {
                if (listBox_Users.SelectedItem.ToString()!=Properties.Settings.Default["CurrentUser"].ToString())
                {
                    Properties.Settings.Default.CurrentUser = listBox_Users.SelectedItem.ToString();
                    Properties.Settings.Default.Save();
                    this.Close();
                    MessageBox.Show("Пользователь изменен. Нужно перезапустить программу, \rчтобы изменения вступили в силу.", "Выполнено", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Application.Restart();
                }
                else
                {
                    this.Close();
                }
            }
        }
     

        private void ListBoxOK_Click(object sender, EventArgs e)
        {
            change_user();
        }

      
    }
}
