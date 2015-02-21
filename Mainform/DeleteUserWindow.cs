using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace OvulationCalendar
{
    public partial class DeleteUserWindow : Form
    {
        MainForm mainForm;
        public DeleteUserWindow()
        {
            InitializeComponent();
        }

        public DeleteUserWindow(MainForm mf)
        {
            this.mainForm = mf;
            InitializeComponent();
            foreach (string user in mainForm.users)
            {
                listBox_Users.Items.Add(user);
                if (user == Properties.Settings.Default["CurrentUser"].ToString()) listBox_Users.SetSelected(listBox_Users.Items.IndexOf(user), true);
            }
            
        }

        private void DeleteUser_Button_Click(object sender, EventArgs e)
        {

            if (listBox_Users.SelectedItem != null)
            {
                if (mainForm.users.Count < 2)
                {
                    MessageBox.Show(Strings.WarnLastUserDelete, Strings.Failed , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return; 
                }
                string prev_user= mainForm.users[mainForm.users.Count - 2].ToString();
                Properties.Settings.Default.CurrentUser = prev_user;
                Properties.Settings.Default.Save();
                this.mainForm.CurrentUserName = prev_user;
                string FullPathToDataBase = Properties.Settings.Default.SettingsFolder + Path.DirectorySeparatorChar;
                string UserDataFile= FullPathToDataBase + listBox_Users.SelectedItem.ToString();
                string FileWithUserNames = FullPathToDataBase + MainForm.FileWithUsersNames;
                if (File.Exists(UserDataFile)) // MainForm.FileWithUsersNames
                {  
                    try {                          
                            File.Delete(UserDataFile);                            
                        }
                    catch
                    {

                        MessageBox.Show(Strings.CannotDelDatabaseFile + " \r" + Strings.NoAccessToFile+" \r", Strings.Error );
                        this.Close();
                    }
                }

                if (File.Exists(FileWithUserNames)) // MainForm.FileWithUsersNames
                {
                    try
                    {
                        var tempFile = Path.GetTempFileName();
                        var linesToKeep = File.ReadLines(FileWithUserNames).Where(l => l != listBox_Users.SelectedItem.ToString());

                        File.WriteAllLines(tempFile, linesToKeep);

                        File.Delete(FileWithUserNames);
                        File.Move(tempFile, FileWithUserNames);                       

                    }
                    catch
                    {

                        MessageBox.Show(Strings.CannotDelDatabaseFile + " \r" + Strings.NoAccessToFile + " \r", Strings.Error);
                        this.Close();
                    }
                }

                
                this.Close();
                MessageBox.Show(Strings.UserHasBeenDeleted + " \r" + Strings.AppRestartingRequired +" \r", Strings.Done , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Restart();
               
            }
        }

    }
}
