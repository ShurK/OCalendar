using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OvulationCalendar
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            MainForm MForm = new MainForm();
            
            Application.Run(MForm);
            
        }
    }
}
