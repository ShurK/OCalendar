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
    
    public partial class NewCycleControlDialog : Form
    {
        public NewCycleControlDialog()
        {
            InitializeComponent();
            
        }
        
        
          MainForm mainForm;
  
          OvulationCalendar.Calendar.Cycle CY;

          public NewCycleControlDialog(DateTime dt, MainForm mf, OvulationCalendar.Calendar.Cycle Cycle) //окно редактирования цикла
          {
              this.mainForm = mf;
              InitializeComponent();
              BeginDate.Value = Cycle.Start;
              if (Cycle.End > Cycle.Start) EndDate.Value = Cycle.End;
                else EndDate.Value = Cycle.Start.AddDays(4);
              BeginDateOnly.Value = Cycle.Start;
              Text = Strings.CycleChanging ;
              CY = Cycle;

          }

        public NewCycleControlDialog(DateTime dt, MainForm mf)
        {
            this.mainForm = mf;
            InitializeComponent();
            BeginDate.Value = dt;
            EndDate.Value = dt.AddDays(4);
            BeginDateOnly.Value = dt;
            CY = new Calendar.Cycle();

        }




        private void NewCycleControlDialog_Load(object sender, EventArgs e)
        {
            this.Icon = this.mainForm.Icon;
            Screen curScreen = System.Windows.Forms.Screen.FromControl(this);
            Point loc = new Point(MousePosition.X - this.Width / 2, MousePosition.Y - this.Height / 2);
            if (loc.X < curScreen.WorkingArea.Left)
            { loc.X = curScreen.WorkingArea.Left; }
            if (loc.X + this.Width > curScreen.WorkingArea.Right)
            {
                loc.X = curScreen.WorkingArea.Right - this.Width;
            }
            if (loc.Y < curScreen.WorkingArea.Top)
            { loc.Y = curScreen.WorkingArea.Top; }

            if (loc.Y + this.Height > curScreen.WorkingArea.Bottom)
            { loc.Y = curScreen.WorkingArea.Bottom - this.Height; }

            this.Location = loc;
            System.Windows.Forms.RadioButton[] radioButtons =
            new System.Windows.Forms.RadioButton[2];
            radioButtons[0] = radioButton1;
            radioButtons[1] = radioButton2;


        }

      

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            BeginDate.Enabled = EndDate.Enabled = false;
            BeginDateOnly.Enabled = true;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            BeginDate.Enabled= true;
            EndDate.Enabled = true;
            BeginDateOnly.Enabled = false;

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {           
            
            float AverageCycleDuration=this.mainForm.CL.findAverageCycleDuration();

            if (radioButton1.Checked && BeginDate.Value > EndDate.Value) { //Begining is later the ending
                MessageBox.Show(this, Strings.Err_EndIsEarlierThanBegin, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return;
            }

            if ((BeginDate.Value-DateTime.Now).Days> this.mainForm.yearsAfterCurYear*365-30) {  //Trying to set the cycle too far from current date
                MessageBox.Show(this, Strings.Err_TheCycleIsTooFar, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return; 
            }

            if (radioButton1.Checked && (EndDate.Value - BeginDate.Value).Days < 1)  //Too short cycle
            {
                MessageBox.Show(this, Strings.Err_CycleShuldBeLongerThanOneDay, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return;
            }

            if (radioButton1.Checked && (EndDate.Value - BeginDate.Value).Days > this.mainForm.longestCycle) //Too large Cycle 
            {
                MessageBox.Show(this, Strings.Err_CycleIsTooLong, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return;
            }

            
            if (radioButton1.Checked && BeginDate.Value.Year < DateTime.Now.Year - this.mainForm.yearsBeforeCurYear ||
                    radioButton1.Checked && BeginDate.Value.Year > DateTime.Now.Year + this.mainForm.yearsAfterCurYear ||
                    radioButton1.Checked && EndDate.Value.Year > DateTime.Now.Year + this.mainForm.yearsAfterCurYear ||
                    radioButton1.Checked && BeginDateOnly.Value.Year < DateTime.Now.Year - this.mainForm.yearsBeforeCurYear ||
                    radioButton1.Checked && BeginDateOnly.Value.Year > DateTime.Now.Year + this.mainForm.yearsAfterCurYear
                )
            {
                MessageBox.Show(this, Strings.Err_YearShouldBeInRange + " " + (DateTime.Now.Year - this.mainForm.yearsBeforeCurYear).ToString() + " - " + (DateTime.Now.Year + this.mainForm.yearsAfterCurYear).ToString(),
                                Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return;
            }

           // else if ()

            OvulationCalendar.Calendar.Cycle Cle = new OvulationCalendar.Calendar.Cycle();

            if (BeginDate.Enabled) { Cle.Start = BeginDate.Value; Cle.End = EndDate.Value; } // создаем цикл из параметров ввода
            else { Cle.Start = BeginDateOnly.Value; Cle.HasNoEnd = true; } //чтобы легче было сравнивать с существующими циклами

            if (this.mainForm.CL.CyclesList.Contains(Cle) && this.CY.Start.Year<1800) //Overlapped Cycles
            { MessageBox.Show(this, Strings.Err_CycleIsOverlappedWithExisting,
                    Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return; }

            if (Cle.HasNoEnd && (DateTime.Now - BeginDateOnly.Value).Days >17 ) { //Too old date for not ended Cycle. User should eneter the complete cycle instead
                MessageBox.Show(this, Strings.Err_TooOldDateForNotCompleteCycle,
                        Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return; 
                }        
            

            if (this.CY.Start.Year<1800 && OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement > -1 && Cle.HasNoEnd)
            { MessageBox.Show(this, Strings.Err_OneNotEndedCycleAlreadyExist,
                Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return; }

            int CleIdx=this.mainForm.CL.CyclesList.IndexOf(Cle);
            if (this.CY.Start.Year>1800 && CleIdx >= 0 && CleIdx != this.mainForm.CL.CyclesList.IndexOf(CY)) 
            {
                MessageBox.Show(this, Strings.Err_NewRangeIsOverlappingWithOtherCycle,
                    Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return;
            }

            if (OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement > -1)
            {
                if (Cle > this.mainForm.CL.CyclesList[OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement])
                {
                    MessageBox.Show(this, Strings.Err_CannotCreateNewCycleWhileOldOneIsNotEnded,
                        Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return;
                }
            }

            int nearestCycle = this.mainForm.CL.findNearestCycleDistance(Cle);
            //MessageBox.Show(nearestCycle.ToString(), "Овавашибка"); 
            if (nearestCycle != 0xfffffff && nearestCycle < 10)
            {
                MessageBox.Show(this, Strings.Err_ShouldBeMoreThan10daysBetweenCycles,
                        Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1); return;
            }

            if (this.CY.Start.Year > 1800)
            {                
                this.mainForm.CL.delete_Cycle(this.CY);
            }
             
            


            
            if (!Cle.HasNoEnd)
            {
                List<DateLabel> curLabels = this.mainForm.CL.getLabels(Cle);
                this.mainForm.CL.CyclesList.Add(Cle);
                //this.mainForm.CL.CalcPredictedMenstruation();
                
                foreach (DateLabel CurLabel in curLabels)
                {
                    CurLabel.MestruationColor = MainForm.BackGroundCycleColor;
                    CurLabel.blendColors();
                    CurLabel.ContextMenu.MenuItems[2].Enabled = true;
                    CurLabel.ContextMenu.MenuItems[0].Enabled = false;
                    CurLabel.ContextMenu.MenuItems[1].Enabled = true;
                    CurLabel.ttip.SetToolTip(CurLabel, null);                    
                }
 
                                
                this.mainForm.CL.SaveCyclesList();
                this.mainForm.CL.CalcPredictedMenstruation();
                this.mainForm.ShowStats();

            }
            else
            {
                
 
                this.mainForm.CL.CyclesList.Add(Cle);
               
                    OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement = this.mainForm.CL.CyclesList.IndexOf(Cle);
                    Cle.End = Cle.Start.AddDays(AverageCycleDuration);
                    this.mainForm.CL.CyclesList[OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement] = Cle;
                      
                this.mainForm.CL.updateNoEndCycle(); 
                    this.mainForm.CL.SaveCyclesList();

                    this.mainForm.CL.CalcPredictedMenstruation();
                        this.mainForm.CL.updateNoEndCycle();
                        this.mainForm.CL.CalcPredictedMenstruation();
                    this.mainForm.ShowStats();
            }

            this.Close();
        }

        private void BeginDate_ValueChanged(object sender, EventArgs e)
        {

        }



    }
}
