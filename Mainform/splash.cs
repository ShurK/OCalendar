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
    public partial class splash : Form
    {
        //public ProgressBar progressBar1;
        public splash()
        {
            InitializeComponent();
        }

        private void splash_Load(object sender, EventArgs e)
        {
            downloadLabel.BackColor = Color.Transparent;
            BigText.BackColor = Color.Transparent;
            progressBar1.Maximum = MainForm.Years+2;
            progressBar1.Value = 0;
        }
    }
}
