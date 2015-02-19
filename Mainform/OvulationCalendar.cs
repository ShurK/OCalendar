using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Xml.Serialization;



namespace OvulationCalendar
{
    public partial class MainForm : Form
    {
        //SortedList al = new SortedList();
        
        public static int LuteinPhase_DaysBeforeMenstruation_Max = 16;
        public static int LuteinPhase_DaysBeforeMenstruation_Min = 12;
        public static int Years = 6;
        public static float LuteinPhase_DaysBeforeMenstruation_Average = (float)(LuteinPhase_DaysBeforeMenstruation_Max + LuteinPhase_DaysBeforeMenstruation_Min)/2;
        public static Color MentruationForeColor= System.Drawing.Color.FromArgb(0xf0, 0xf0, 0xf0);
        public static Color BackGroundCycleColor = System.Drawing.Color.FromArgb(0xE0, 0x10, 0x10);
        public static Color BackGroundPredictionColor = System.Drawing.Color.FromArgb(0xE0, 0x10, 0xe0);
        public static Color foreOfnoEnd = System.Drawing.Color.Yellow;
        public static Color foreOfPrediction = System.Drawing.Color.Yellow;
        public static Color BackGroundOvulationColor = System.Drawing.Color.FromArgb(0x10, 0xE0, 0x10);
        public Calendar CL ;
        public int yearsBeforeCurYear=5, CurYearIndex = 5;
        public int yearsAfterCurYear = 1;        
        public int longestCycle = 20;
        public string FileWithUsersNames;
        public string FileWithSerializedCycles;
        public List<string> users = new List<string>();
        public static System.Drawing.Size MonthPanelSize = new System.Drawing.Size(200, 175);
        public static int YearBordersPadding = 5;
        public int monthsForPrediction = 6;
        public splash Splash = new splash();
        
        
        public MainForm()
        {

            
            Splash.Show();
            Application.DoEvents();

            //Splash.Show();
            
            InitializeComponent();

            //int a = 0;


            

            //MessageBox.Show(CL.AllYears[5].NumYear.ToString(), "Год");

        }


        private void LoadSettings()
        {
            string AppName = "OvCalendar";
            
            //string output="";
            IDictionary Envs= System.Environment.GetEnvironmentVariables();
            
            //foreach (DictionaryEntry Var in Envs)
            //{
            //    output =  output + Var.Key.ToString()+ "=" + Var.Value+"\n";
                
            //}
            //MessageBox.Show(output, "fdfd");
            string CurUserName = Envs["USERNAME"].ToString();
            string pth = "";
            string FileWithUsersNames="Users";



            if (!File.Exists(FileWithUsersNames))
            {
                users.Add(CurUserName);
                //XmlSerializer serializer = new XmlSerializer(typeof(List<string>));
                try
                {


                    TextWriter writer = new StreamWriter(FileWithUsersNames);
                    writer.WriteLine(CurUserName);
                    writer.Close();
                    this.FileWithUsersNames = FileWithUsersNames;
                    this.FileWithSerializedCycles = Path.GetDirectoryName(FileWithUsersNames) + users[0] ;


                }
                catch
                {
                    try
                    {
                        pth = Envs["APPDATA"].ToString();
                        pth = pth + Path.DirectorySeparatorChar + AppName;
                        if (!Directory.Exists(pth))
                        {
                            Directory.CreateDirectory(pth);
                        }

                        pth = pth + Path.DirectorySeparatorChar + FileWithUsersNames;

                        if (!File.Exists(pth))
                        {
                            TextWriter writer = new StreamWriter(pth);
                            writer.WriteLine(CurUserName);
                            writer.Close();
                            this.FileWithUsersNames = FileWithUsersNames;
                            this.FileWithSerializedCycles = Path.GetDirectoryName(FileWithUsersNames) + users[0];
                            //MessageBox.Show(this.FileWithSerializedCycles, "");
                        }
                        else
                        {
                            ReadProgramData(pth);
  
                        }

                        //UserData

                    }
                    catch
                    {

                        MessageBox.Show("Программа не может создать файл настроек. \rВидимо отсутствует доступ к файловой системе \r", "Ошибка");
                        this.Close();
                    }


                }


            }

            else //файл юзеров существует
            {
                this.FileWithUsersNames = FileWithUsersNames;
                
                ReadProgramData(FileWithUsersNames);
  
            }

        }

        private void ReadProgramData(string FileWithUsersNames)
        {
            StreamReader sr;
            try
            {
                 sr = new StreamReader(FileWithUsersNames);
            
                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    users.Add(line);
                }

                sr.Close();
            }
            catch
            {
                MessageBox.Show("Программа не может открыть файл с именами пользователей (Users). \r", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
            };

                if (this.FileWithSerializedCycles == null)
                    this.FileWithSerializedCycles = Path.GetDirectoryName(FileWithUsersNames) + users[0];

                if (File.Exists(this.FileWithSerializedCycles))
                {
                    //serializer = new XmlSerializer(typeof(List<Calendar.Cycle>));
                    StreamReader Cycles = new StreamReader(this.FileWithSerializedCycles);
                    //this.CL.CyclesList = (List<Calendar.Cycle>)serializer.Deserialize(Cycles);
                    String line;
                    while ((line = Cycles.ReadLine()) != null)
                    {
                        string line2 = Cycles.ReadLine();
                        if (line2 != null)
                        {
                            string line3 = Cycles.ReadLine();
                            DateTime Begin;
                            DateTime End;
                            bool noEnd;
                            if (line3 != null)
                                if (DateTime.TryParse(line, out Begin) && DateTime.TryParse(line2, out End) &&
                                        bool.TryParse(line3, out noEnd))
                                {
                                    this.CL.CyclesList.Add(new Calendar.Cycle(Begin, End, noEnd));
                                };
                        }

                    }
                    Cycles.Close();

                    foreach (Calendar.Cycle Cle in this.CL.CyclesList)
                    {
                        if (Cle.HasNoEnd == true) { Calendar.Cycle.IndexOfNoEndElement = this.CL.CyclesList.IndexOf(Cle); break; }
                    }

                    foreach (Calendar.Cycle Cle in this.CL.CyclesList)
                    {
                        ShowCycle(Cle);
                    }
                    this.CL.updateNoEndCycle();
                }

          
            CL.CalcPredictedMenstruation();
            
        }

        private void ShowCycle(Calendar.Cycle Cle)
        {
            if (!Cle.HasNoEnd)
            {
                List<DateLabel> curLabels = this.CL.getLabels(Cle);
                foreach (DateLabel CurLabel in curLabels)
                {
                    CurLabel.BackColor = MainForm.BackGroundCycleColor;
                    CurLabel.MestruationColor = MainForm.BackGroundCycleColor;
                    CurLabel.ForeColor = MainForm.MentruationForeColor; 
                    CurLabel.ContextMenu.MenuItems[2].Enabled = true;
                    CurLabel.ContextMenu.MenuItems[0].Enabled = false;
                    CurLabel.ContextMenu.MenuItems[1].Enabled = true;
                }
            
            }
            else
            {

                
                //OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement = this.CL.CyclesList.IndexOf(Cle);
                
                //this.CL.updateNoEndCycle();
                

            }

        }
 

        private void DecreaseYear_Click(object sender, EventArgs e)
        {
            if (Year.CurrentYearArrayIndex > 0) {
                MainGroupBox.SuspendLayout();
                for (int i = 0; i <= 11; i++) MainGroupBox.Controls.Remove(CL.AllYears[Year.CurrentYearArrayIndex].MonthsList[i].MonthPanel);
                Year.Current--; Year.CurrentYearArrayIndex--;
                YearLabel.Text = Year.Current.ToString();
            } else return;

            for (int i = 0; i <= 11; i++) MainGroupBox.Controls.Add(CL.AllYears[Year.CurrentYearArrayIndex].MonthsList[i].MonthPanel);
            MainGroupBox.ResumeLayout();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Year.CurrentYearArrayIndex < yearsBeforeCurYear + yearsAfterCurYear)
            {
                MainGroupBox.SuspendLayout();
                for (int i = 0; i <= 11; i++) 
                    MainGroupBox.Controls.Remove(CL.AllYears[Year.CurrentYearArrayIndex].MonthsList[i].MonthPanel);
                    
                Year.Current++; Year.CurrentYearArrayIndex++;
                this.YearLabel.Text = Year.Current.ToString();
            }
            else return;

            for (int i = 0; i <= 11; i++)
            
                MainGroupBox.Controls.Add(CL.AllYears[Year.CurrentYearArrayIndex].MonthsList[i].MonthPanel);
               
             MainGroupBox.ResumeLayout();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
            
            System.Reflection.Assembly thisExe;
            thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            //string[] resurces = thisExe.GetManifestResourceNames();
            //foreach (string res in resurces) { if (res != null) MessageBox.Show(res, "dfd"); }
            try
            {
                System.IO.Stream file =
                thisExe.GetManifestResourceStream("OvulationCalendar.icon2.ico");
                Icon = new Icon(file);
            }
            catch  { }
            
            
            CL = new Calendar(this);
            foreach (Year year in CL.AllYears)
            {
                this.Splash.progressBar1.Value++;//обновление прогрессбара
                for (int i = 0; i <= 11; i++)
                {
                   //кеширование
                    MainGroupBox.Controls.Add(year.MonthsList[i].MonthPanel);                    
                    MainGroupBox.Controls.Remove(year.MonthsList[i].MonthPanel);
                    
                }
            }
            this.Splash.progressBar1.Value++;
            for (int i = 0; i <= 11; i++) this.MainGroupBox.Controls.Add(CL.AllYears[CurYearIndex].MonthsList[i].MonthPanel);
            
            YearLabel.Text = CL.AllYears[CurYearIndex].NumYear.ToString();

            Year.Current = CL.AllYears[CurYearIndex].NumYear;
            Year.CurrentYearArrayIndex = CurYearIndex;

            this.MainGroupBox.Padding = new Padding(MainForm.YearBordersPadding-1);
            Size MainGrSize = MainGroupBox.GetPreferredSize(MainGroupBox.Size);
            StatsPanel.Location = new Point(MainGroupBox.Location.X + MainGrSize.Width +MainGroupBox.Padding.Right, MainGroupBox.Location.Y);

            this.YearLabel.Location = new Point(
                (CL.AllYears[CurYearIndex].MonthsList[11].MonthPanel.Location.X + 
                    CL.AllYears[CurYearIndex].MonthsList[11].MonthPanel.Width
                    - CL.AllYears[CurYearIndex].MonthsList[0].MonthPanel.Location.X)/2 -
                    this.YearLabel.Width/2 + MainForm.YearBordersPadding,
                (CL.AllYears[CurYearIndex].MonthsList[0].MonthPanel.Location.Y-30)
                );
            this.DecreaseYear.Location = new Point(
                this.YearLabel.Location.X - this.DecreaseYear.Width,
                this.YearLabel.Location.Y + this.YearLabel.Height / 2 - this.DecreaseYear.Height/2
                );
            this.IncreaseYear.Location = new Point(
                this.YearLabel.Location.X + this.YearLabel.Width ,
                this.YearLabel.Location.Y + this.YearLabel.Height / 2 - this.IncreaseYear.Height / 2
            );
            this.calcPeriod.SelectedText = "6";

            LoadSettings();
            Splash.Close();
            ResumeLayout();
            
            ShowStats();
            

        }

        private void calcPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            int Period;
            if (int.TryParse(calcPeriod.SelectedItem.ToString(), out Period) && Period > 2 && Period < 24) monthsForPrediction = Period;
            else monthsForPrediction = 48;
            //if (CL.RaiseCyclesListChange != null) CL.invokeCyclesListChangeEvent();
            //updateNoEndCycle();
            
            
            CL.updateNoEndCycle();
            CL.CalcPredictedMenstruation();
            ShowStats();
            //this.ShowStats();

        }

        public void ShowStats()
        {
            int ShortestPeriod = CL.findShortestPeriod();
            int LongestPeriod = CL.findLongestPeriod();
            float AverageDuration = CL.findAverageCycleDuration();

            if (ShortestPeriod < 40) val1.Text = ShortestPeriod.ToString(); else val1.Text = "н/д";
            if (LongestPeriod > 1) val2.Text = string.Format("{0:F0}",LongestPeriod); else val2.Text = "н/д";
            if (AverageDuration > 0) val3.Text = string.Format("{0:F1}", AverageDuration); else val3.Text = "н/д";

            float AverageCycleDuration=Calendar.statistics.getAverageCycle(this);
            if (AverageCycleDuration > -1) val4.Text = string.Format("{0:F1}", AverageCycleDuration); else val4.Text = "н/д";

            int MaxCycleDuration = Calendar.statistics.getMaxCycle(this);
            if (MaxCycleDuration > -1) val6.Text = MaxCycleDuration.ToString(); else val6.Text = "н/д";

            int MinCycleDuration = Calendar.statistics.getMinCycle(this);
            if (MinCycleDuration > -1) val5.Text = MinCycleDuration.ToString(); else val5.Text = "н/д";

            val7.Text = CL.CyclesList.Count().ToString();

            DateTime begin = Calendar.statistics.getBeginDate(this);
            if (begin.Year > 1800) val8.Text = begin.ToShortDateString(); else val8.Text = "н/д";

            DateTime end = Calendar.statistics.getEndDate(this);
            if (end.Year > 1800) val9.Text = end.ToShortDateString(); else val9.Text = "н/д";

            


        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
           About aboutform = new About();
           aboutform.Show();
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }



   

    }


    public class Calendar
    {
        MainForm Mf;
        public List<Cycle> CyclesList = new List<Cycle>();
        public Cycle MenstruationPrediction = new Cycle();
        public Cycle OvulationPrediction = new Cycle();

        public static List<string> MonthsArray = new List<string> { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
        public static List<string> DaysOfWeek = new List<string> { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Вс" };
        static readonly DateTime today = DateTime.Now;
        static readonly TimeSpan tm = new TimeSpan(today.DayOfYear - 1, 0, 0, 0);
        static readonly DateTime FirstJanuaryCurrent = today - tm;  // получаем 1 января текущего года
        public List<Year> AllYears;
        
        public struct Cycle : IComparable, IEquatable<Cycle>
        {
            
            

            
            public static int IndexOfNoEndElement=-1; //индекс цикла (если есть), который не закончен
            public Boolean HasNoEnd ; // призднка того, что цикл не закончен

            public DateTime Start; // начало цикла
            private DateTime en; //конец цикла

            public Cycle(DateTime dt)
            {
                HasNoEnd=true;
                Start=dt;
                en = new DateTime();
            }

            public Cycle(DateTime beginOfCycle, DateTime endOfCycle, bool noendflag)
            {
                HasNoEnd = noendflag;
                Start = beginOfCycle;
                en = endOfCycle;
            }

            public DateTime End
            {
                get {

                    if (en>Start) return en;
                    else return Start;
                }

                set {
                    en = value; //HasNoEnd = false;
                }
            }

            public TimeSpan Days

            {
                get
                {
                    return End - Start;
                }

                set
                {
                    End = Start + value;

                }

            }

            public int CompareTo(Object scle)
            {
                if (scle is Cycle)
                {
                    Cycle c = (Cycle)scle;
                    return Start.CompareTo(c.Start);
                }
                else { throw new ArgumentException("Object is not a Cycle"); }

            }

            public bool Equals(Cycle other)
            {
                if (this.Start > other.End || this.End < other.Start)
                    return false;
                else
                    return true;
            }

            public override bool Equals(Object obj)
            {
                if (obj == null) return base.Equals(obj);

                if (!(obj is Cycle))
                    throw new InvalidCastException("The 'obj' argument is not a Cycle object.");
                else
                    return Equals((Cycle)obj);
            }

            public override int GetHashCode()
            {
                return this.Start.GetHashCode();
            }

            public static bool operator ==(Cycle Cycle1, Cycle Cycle2)
            {
                return Cycle1.Equals(Cycle2);
            }

            public static bool operator !=(Cycle Cycle1, Cycle Cycle2)
            {
                return (!Cycle1.Equals(Cycle2));
            }

            public static int operator -(Cycle Cycle1, Cycle Cycle2)
            {
                if (Cycle1 == Cycle2) return 0;
                if (Cycle1.Start > Cycle2.Start) return (Cycle1.Start - Cycle2.End).Days;
                else return (Cycle1.End - Cycle2.Start).Days ;
            }

            public static bool operator >(Cycle Cycle1, Cycle Cycle2)
            {
                if (Cycle1 == Cycle2) return false;
                if (Cycle1.Start > Cycle2.End) return true;
                else return false;
            }

            public static bool operator <(Cycle Cycle1, Cycle Cycle2)
            {
                if (Cycle1 == Cycle2) return false;
                if (Cycle1.End < Cycle2.Start) return true;
                else return false;
            }

            public static bool operator >=(Cycle Cycle1, Cycle Cycle2)
            {
                if (Cycle1 == Cycle2) return true;
                if (Cycle1.Start > Cycle2.End) return true;
                else return false;
            }

            public static bool operator <=(Cycle Cycle1, Cycle Cycle2)
            {
                if (Cycle1 == Cycle2) return true;
                if (Cycle1.End < Cycle2.Start) return true;
                else return false;
            }



        }

        public List<DateLabel> getLabels(Cycle Cycle)
        {
            List<DateLabel> LabelsList = new List<DateLabel>();
            DateTime start = Cycle.Start;
            DateTime end = Cycle.End;
            DateTime dt = start;
            while ( dt <= end )
            {
                if (dt.Year - DateTime.Now.Year + Mf.yearsBeforeCurYear >= 0)
                {
                    DateLabel CurLabel = AllYears[dt.Year - DateTime.Now.Year + Mf.yearsBeforeCurYear].MonthsList[dt.Month - Mf.yearsAfterCurYear].DateLabels[dt.Day - Mf.yearsAfterCurYear];
                    LabelsList.Add(CurLabel);
                    dt = dt.AddDays(1);
                }
                else
                {
                    dt = dt.AddYears(1);
                }
            }   
            return LabelsList;
        }

        public List<DateLabel> getLabels(DateTime date, int durationInDays )
        {
            List<DateLabel> LabelsList = new List<DateLabel>();
            DateTime start = date;
            DateTime end = date.AddDays(durationInDays);
            for (DateTime dt = start; dt <= end; dt=dt.AddDays(1))
            {
                DateLabel CurLabel = AllYears[dt.Year - DateTime.Now.Year + Mf.yearsBeforeCurYear].MonthsList[dt.Month - Mf.yearsAfterCurYear].DateLabels[dt.Day - Mf.yearsAfterCurYear];
                LabelsList.Add(CurLabel);
            }
            return LabelsList;
        }

       
        public void SaveCyclesList()
        {
            try
            {
                //XmlSerializer serializer = new XmlSerializer(typeof(List<Cycle>));
                TextWriter writer = new StreamWriter(Mf.FileWithSerializedCycles);
                //serializer.Serialize(writer, CyclesList);
                foreach (Cycle Cle in CyclesList)
                {
                    writer.WriteLine(Cle.Start.ToString());
                    writer.WriteLine(Cle.End.ToString());
                    writer.WriteLine(Cle.HasNoEnd.ToString());
                }
                writer.Close();
            }
            catch
            {
                MessageBox.Show("Не удалось сохранить настройки в \rфайл " + Mf.FileWithSerializedCycles.ToString());
            }
            
        }

        public float findAverageCycleDuration(DateTime begin, DateTime end)
        {
            int Cycles = getCyclesPerPeriod(begin, end);
            if (Cycle.IndexOfNoEndElement>=0 && CyclesList[Cycle.IndexOfNoEndElement] > new Cycle(begin) && CyclesList[Cycle.IndexOfNoEndElement] < new Cycle(end))
            { Cycles--; }
            int sum=0;
            
            foreach (Cycle curCycle in CyclesList)
            {
                if (curCycle >= new Cycle(begin) && curCycle <= new Cycle(end) && (Cycle.IndexOfNoEndElement<0 || curCycle != CyclesList[Cycle.IndexOfNoEndElement])) sum += curCycle.Days.Days;
            }
            if (Cycles > 0) return (float)sum / Cycles+1;
                else return 0;
        }

        public float findAverageCycleDuration()
        {
            List<Cycle> Cycles = Mf.CL.CyclesList;
            Cycles.Sort();
            if (Cycles.Count > Mf.monthsForPrediction) Cycles = CyclesList.GetRange(Cycles.Count - Mf.monthsForPrediction, Mf.monthsForPrediction);
            if (Cycles.Count > 0) return findAverageCycleDuration(Cycles[0].Start, Cycles[Cycles.Count - 1].End);
            else return 0;
        }

       
        public int findShortestPeriod()
        {
            List<Cycle> Cycles = Mf.CL.CyclesList;
            Cycles.Sort();
            if (Cycles.Count > Mf.monthsForPrediction) Cycles = CyclesList.GetRange(Cycles.Count - Mf.monthsForPrediction, Mf.monthsForPrediction);

            int CurDuration = 50;
            foreach (Cycle cle in Cycles)
            {

                if (cle.Days.Days < CurDuration && (Cycle.IndexOfNoEndElement < 0 || cle != CyclesList[Cycle.IndexOfNoEndElement])) CurDuration = cle.Days.Days;   
            }
            return CurDuration+1;
        }

        public int findLongestPeriod()
        {
            List<Cycle> Cycles = Mf.CL.CyclesList;
            Cycles.Sort();
            if (Cycles.Count > Mf.monthsForPrediction) Cycles = CyclesList.GetRange(Cycles.Count - Mf.monthsForPrediction, Mf.monthsForPrediction);


            int CurDuration = 0;
            foreach (Cycle cle in Cycles)
            {

                if (cle.Days.Days > CurDuration && (Cycle.IndexOfNoEndElement < 0 || cle != CyclesList[Cycle.IndexOfNoEndElement])) CurDuration = cle.Days.Days; 
            }
            return CurDuration+1;
        }

        public int getCyclesPerPeriod(DateTime begin, DateTime end)
        {
            int sum = 0;
            Cycle BeginGeneric = new Cycle(begin);
            Cycle EndGeneric = new Cycle(end);

            foreach (Cycle curCycle in CyclesList)
            {
                if (curCycle >= BeginGeneric && curCycle <= EndGeneric) { sum++; }
            }
            return sum;
        }

        public int findNearestCycleDistance(Cycle c) {
            
            Cycle GenericCycle=c;
            //GenericCycle.St

            int minDistance = 0xfffffff;
            foreach (Cycle cl in CyclesList)
            {
                if (cl != GenericCycle)
                {
                    int curDistance = Math.Abs(GenericCycle - cl);
                    if (curDistance < minDistance) { minDistance = curDistance; }

                }
            }

            return minDistance;

        }

        public float getPossibilityOfCycleEndAfter(int DaysAfterBegin)
        {
            int counter1 = 0;
            int daysCounted = 0;
            List<Cycle> Cycles = Mf.CL.CyclesList;
            Cycles.Sort();
            if (Cycles.Count > Mf.monthsForPrediction) Cycles = CyclesList.GetRange(Cycles.Count - Mf.monthsForPrediction, Mf.monthsForPrediction);


            foreach (Cycle curCycle in Cycles)
            {
                if ((curCycle.End - curCycle.Start).Days < DaysAfterBegin)
                {
                    counter1 += DaysAfterBegin - (curCycle.End - curCycle.Start).Days;
                    daysCounted += DaysAfterBegin - (curCycle.End - curCycle.Start).Days;
                }
                else { daysCounted += (curCycle.End - curCycle.Start).Days - DaysAfterBegin +1; }
            }

            if (Cycles.Count < 2)
            {

                return ((float)(6 - DaysAfterBegin) / 6);
            }

            else return (1f-(float)counter1 / daysCounted);

            
        }

        

        public void CalcPredictedMenstruation()
        {
            //очищаем предидущие метки
            
            
            if (MenstruationPrediction.Start.Year > 1800) 
                {
                   List<DateLabel> labels= getLabels(MenstruationPrediction);
                   foreach (DateLabel label in labels)
                   {
                       
                       label.ForeColor = Mf.ForeColor;
                       label.MentruationPredictionColor = Mf.BackColor;
                       label.blendColors();
                       if (label.ttip.GetToolTip(label) != null) label.ttip.SetToolTip(label,null); //label.
                   }
                   MenstruationPrediction.Start = new DateTime();  
                }
            if (OvulationPrediction.Start.Year > 1800)
            {
                List<DateLabel> labels = getLabels(OvulationPrediction);
                foreach (DateLabel label in labels)
                {


                    //if (new Cycle(label.chislo) != CyclesList[CyclesList.Count - 1])
                    //{ label.ForeColor = Mf.ForeColor;  }
                    //else label.ForeColor = MainForm.MentruationForeColor; 
                    
                    //label.BackColor = label.MestruationColor;
                        
                         //label.
                    label.OvulationColor = Mf.BackColor;
                    label.blendColors();
                    //MessageBox.Show(label.ttip.GetToolTip(label).ToString());
                    if (label.ttip.GetToolTip(label) != null && new Cycle(label.chislo) != CyclesList[CyclesList.Count - 1]
                        && label.ttip.GetToolTip(label).IndexOf("прекра")==-1 
                        )
                        label.ttip.SetToolTip(label,null);
                    else if (label.ttip.GetToolTip(label) != null && label.ttip.GetToolTip(label).IndexOf("прекра") > -1
                        && label.ttip.GetToolTip(label).IndexOf("овул") > -1
                        ) 
                    {
                        string initialText = label.ttip.GetToolTip(label);
                        int beginDeleteIndex = initialText.IndexOf("\n");
                        label.ttip.SetToolTip(label, initialText.Substring(0,beginDeleteIndex-1));
                    }

                    //if (label.ttip != null && new Cycle(label.chislo) == CyclesList[CyclesList.Count - 1])
                    //    label.ttip.GetToolTip(label).ToString();
                    
                }
                OvulationPrediction.Start = new DateTime();
            }
            
            //int NoEnd=getNoEndElement(CyclesList);
            if ( CyclesList.Count > 1) 
            {
                List<Cycle> Cycles = CyclesList;
                Cycles.Sort();
                if (Cycles.Count > Mf.monthsForPrediction) Cycles = Cycles.GetRange(Cycles.Count - Mf.monthsForPrediction, Mf.monthsForPrediction);
                int Count=Cycles.Count;
                int MinCycle = statistics.getMinCyclePerPeriod(Mf);
                int MaxCycle = statistics.getMaxCyclePerPeriod(Mf);
                MenstruationPrediction.Start = Cycles[Count - 1].Start + new TimeSpan(MinCycle, 0, 0, 0) - new TimeSpan(5, 0, 0, 0); ;
                MenstruationPrediction.End = Cycles[Count - 1].Start + new TimeSpan(MaxCycle+1, 0, 0, 0);
                float[] percents= new float[MaxCycle - MinCycle+7]; 


                                    
                    int n = 0;
                       
                            int alldays = 0;
                            foreach (Cycle cycle in Cycles)
                            {
                                if (Cycles.IndexOf(cycle) == Count - 1) break;
                            
                            
                            int  diff1=((cycle.Start - Cycles[Cycles.IndexOf(cycle)+1].Start)).Days;
                            

                            alldays += diff1;
                            
                            }
                            int avarage_menstruation = -(int)((float)(alldays / ((float)Count - 1))+0.5);
                            
                            n = 1;
                            
                            foreach (float curPercent in percents)
                            {
                                int curdif = (MenstruationPrediction.Start.AddDays(n) - Cycles[Cycles.Count - 1].Start).Days;
                                
                                    
                                    percents[n - 1] = 1 - (Math.Abs((float)curdif - avarage_menstruation) / avarage_menstruation);
                                   
                               
                                n++;
                            }
    
                n=0;
                foreach (float curPercent in percents)
                {
                    percents[n] = 1 - curPercent;
                    n++;
                }
                n = 0;
                float percMax = percents.Max();
               
                
                    foreach (float curPercent in percents)
                    {
                        if (percMax > 0) percents[n] = curPercent / percMax;
                        else percents[n] = 1;

                                                    
                        n++;
                    }

                    n = 0;
                    foreach (float curPercent in percents)
                    {
                        percents[n] = (1 - curPercent)/ (float)1.5;
                        n++;
                    }
                    
                //}
                List<DateLabel> labels = getLabels(MenstruationPrediction);
                OvulationPrediction.Start=MenstruationPrediction.Start-new TimeSpan(MainForm.LuteinPhase_DaysBeforeMenstruation_Max-2,0,0,0);
                OvulationPrediction.End = OvulationPrediction.Start + MenstruationPrediction.Days;
                List<DateLabel> ovulation_labels = getLabels(OvulationPrediction);
                int k=0;
                Color bgColor = Mf.BackColor;
                foreach (DateLabel label in labels)
                {

                    int curR = (int)(bgColor.R + (MainForm.BackGroundPredictionColor.R - bgColor.R) * (1 - (1 - percents[k]) * 0.8 ));
                    int curG = (int)(bgColor.G + (MainForm.BackGroundPredictionColor.G - bgColor.G) * (1 - (1 - percents[k]) * 0.8 ));
                    int curB = (int)(bgColor.B + (MainForm.BackGroundPredictionColor.B - bgColor.B) * (1 - (1 - percents[k]) * 0.8 ));
                    int curOvR = (int)(bgColor.R + (MainForm.BackGroundOvulationColor.R - bgColor.R) * (1 - (1 - percents[k]) * 0.8));
                    int curOvG = (int)(bgColor.G + (MainForm.BackGroundOvulationColor.G - bgColor.G) * (1 - (1 - percents[k]) * 0.8));
                    int curOvB = (int)(bgColor.B + (MainForm.BackGroundOvulationColor.B - bgColor.B) * (1 - (1 - percents[k]) * 0.8));


                    Color newColor, newOvColor = new Color();
                    newColor = System.Drawing.Color.FromArgb(curR, curG, curB);
                    newOvColor = System.Drawing.Color.FromArgb(curOvR, curOvG, curOvB);

                    //label.BackColor = newColor;
                    label.MentruationPredictionColor = newColor;
                    label.blendColors();

                    label.ForeColor = MainForm.foreOfPrediction;                    
                    float curpersents = (int)(percents[k] * 100);
                    if (curpersents == 100) { curpersents = 99; }
                    if (curpersents == 0) { curpersents = 1; }
                    label.ttip.SetToolTip(label, string.Format("Вероятность начала менструации {0}%", curpersents));
                    label.ttip.InitialDelay = 100;


                    if ((ovulation_labels[k].ttip.GetToolTip(ovulation_labels[k]).IndexOf("менстр")>-1
                        || ovulation_labels[k].ttip.GetToolTip(ovulation_labels[k]).IndexOf("прекр")>-1)
                        && ovulation_labels[k].ttip.GetToolTip(ovulation_labels[k]).IndexOf("овул")==-1
                        ) // && ovulation_labels[k].ttip.GetToolTip(ovulation_labels[k]).Length > 2)
                    {
                        ovulation_labels[k].ttip.SetToolTip(ovulation_labels[k], ovulation_labels[k].ttip.GetToolTip(ovulation_labels[k]) + "\n" + string.Format("Вероятность начала овуляции {0}%", curpersents));
                        //MessageBox.Show("C Переводом");
                    }
                    else
                    {
                        if (ovulation_labels[k].ttip.GetToolTip(ovulation_labels[k]) != null) ovulation_labels[k].ttip.SetToolTip(ovulation_labels[k],null);
                        
                        
                        ovulation_labels[k].ttip.SetToolTip(ovulation_labels[k], string.Format("Вероятность начала овуляции {0}%", curpersents));
                        ovulation_labels[k].ttip.InitialDelay = 100;
                        //MessageBox.Show("Без");
                        
                    }
                        //for (int i = 0; i < 30; i++)
                        //{
                        //    System.Threading.Thread.Sleep(100);
                        //   Application.DoEvents();
                        //} 
                        //MessageBox.Show("dfdsf");
                        ovulation_labels[k].OvulationColor = newOvColor;
                        ovulation_labels[k].blendColors();
                        //ovulation_labels[k].ForeColor = MainForm.foreOfPrediction;
                        //if (ovulation_labels[k].BackColor != Mf.BackColor) 
                        //{
                        //    //MessageBox.Show(ovulation_labels[k].ttip.GetToolTip(ovulation_labels[k]).ToString());
                        //    Color tColor = ovulation_labels[k].MestruationColor;
                        //    if (new Cycle(ovulation_labels[k].chislo) != CyclesList[CyclesList.Count - 1])
                        //    ovulation_labels[k].BackColor = Color.FromArgb( //смешиваем цвета
                        //        (ovulation_labels[k].BackColor.R + newOvColor.R) / 2,
                        //        (ovulation_labels[k].BackColor.G + newOvColor.G) / 2,
                        //        (ovulation_labels[k].BackColor.B + newOvColor.B) / 2
                        //        );
                        //    else ovulation_labels[k].BackColor = Color.FromArgb( //смешиваем цвета
                        //        (tColor.R + newOvColor.R) / 2,
                        //        (tColor.G + newOvColor.G) / 2,
                        //        (tColor.B + newOvColor.B) / 2
                        //        );
                        //}
                        //else
                        //{
                        //    ovulation_labels[k].BackColor = newOvColor;
                        //    ovulation_labels[k].ForeColor = MainForm.foreOfPrediction;
                            

                            
                        //}
                        //MessageBox.Show("dfdsf");
                    k++;
                }
                Cycles = CyclesList;
            }
            
        }

        public int getNoEndElement(List<Cycle> Cycles)
        {
            foreach (Cycle Cycle in Cycles)
            {
                if (Cycle.HasNoEnd == true) return CyclesList.IndexOf(Cycle);
            }
            return -1;
        }

        public class CyclesChangeEventArgs : EventArgs
        {
            public CyclesChangeEventArgs()
            {
                
            }

            public CyclesChangeEventArgs(Cycle s)
            {
              Cle = s;
            }
             private Cycle Cle;
             public Cycle s
            {
             get { return Cle; }
            } 
        }

        public delegate void CyclesListChangeEventHandler(object sender, CyclesChangeEventArgs e);

        public event CyclesListChangeEventHandler RaiseCyclesListChange;
         

        public void  delete_Cycle(Cycle Cycl)
        {
            //MessageBox.Show("Deleting");
            Cycle GenericCycle = new Cycle();
            GenericCycle = Cycl;
            int idx = Mf.CL.CyclesList.IndexOf(GenericCycle);
            //MessageBox.Show(idx.ToString());
            if (idx < 0) return;
            
            GenericCycle = Mf.CL.CyclesList[idx];
            if (GenericCycle.HasNoEnd)
            {
                GenericCycle = new Cycle(GenericCycle.Start, GenericCycle.Start.AddDays(GenericCycle.Days.Days+1), true);
            }
            //DateTime EndOfCycle = GenericCycle.End;
            List<DateLabel> curLabels = getLabels(GenericCycle);
            foreach (DateLabel CurLabel in curLabels)
            //while (GenericCycle.Start <= EndOfCycle)
            {
                //DateLabel CurLabel = Mf.CL.AllYears[GenericCycle.Start.Year - DateTime.Now.Year + 6].MonthsList[GenericCycle.Start.Month - 1].DateLabels[GenericCycle.Start.Day - 1];
                //CurLabel.BackColor = Mf.BackColor;
                //CurLabel.ForeColor = Mf.ForeColor;
                CurLabel.MestruationColor = Mf.BackColor;
                CurLabel.BackColor = Mf.BackColor;
                CurLabel.ForeColor = Mf.ForeColor;
                CurLabel.previousBackColor = Mf.BackColor;
                GenericCycle.Start = GenericCycle.Start.AddDays(1);
                CurLabel.ContextMenu.MenuItems[2].Enabled = false;
                CurLabel.ContextMenu.MenuItems[1].Enabled = false;
                CurLabel.ContextMenu.MenuItems[0].Enabled = true;
                CurLabel.ttip.SetToolTip(CurLabel,null);
                //MessageBox.Show("sdfsdfsdf");
            }
            if (CyclesList[idx].HasNoEnd == true) OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement = -1;
            if (OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement >= 0 && idx < OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement) 
            {
                OvulationCalendar.Calendar.Cycle.IndexOfNoEndElement--;
            }
            CyclesList.RemoveAt(idx);
            //CalcPredictedMenstruation();
            //if (RaiseCyclesListChange != null) invokeCyclesListChangeEvent();                    
            

        }
                
        public void updateNoEndCycle()
        {
            //CalcPredictedMenstruation();
            if (Cycle.IndexOfNoEndElement < 0) return;
            
            Cycle Cle = CyclesList[Cycle.IndexOfNoEndElement];
            delete_Cycle(Cle);
            //CalcPredictedMenstruation();
            List<DateLabel> curLabels = getLabels(Cle.Start, Calendar.statistics.getMaxMenstrPerPeriod(Mf)+1);
            Color bgColor = Mf.BackColor;
            Cle.HasNoEnd = true;
            CyclesList.Add(Cle);
            Cycle.IndexOfNoEndElement = CyclesList.IndexOf(Cle);
            //CalcPredictedMenstruation();
            int DaysAfterBegin=0;
            foreach (DateLabel CurLabel in curLabels)
            {
                
                DaysAfterBegin = (CurLabel.chislo - curLabels[0].chislo).Days;
                float percent = getPossibilityOfCycleEndAfter(DaysAfterBegin);
                
                int curR = (int)(bgColor.R + (MainForm.BackGroundCycleColor.R - bgColor.R) * (1-(1-percent)*0.8));
                int curG = (int)(bgColor.G + (MainForm.BackGroundCycleColor.G - bgColor.G) * (1 - (1 - percent) * 0.8));
                int curB = (int)(bgColor.B + (MainForm.BackGroundCycleColor.B - bgColor.B) * (1 - (1 - percent) * 0.8));
                Color newColor = new Color();
                newColor = System.Drawing.Color.FromArgb(curR, curG, curB);
                //CurLabel.BackColor = newColor; //цвет фона в данный момент
                CurLabel.MestruationColor = newColor; //нужен для расчета смешвания цветов
                CurLabel.blendColors();
   
                //Yellow;
                
                CurLabel.ForeColor = MainForm.foreOfnoEnd; 
                
                int ProbabilityToPrint = (int)((1.0 - percent) * 100);
                if (ProbabilityToPrint==100) {ProbabilityToPrint=99;}
                if (ProbabilityToPrint==0) {ProbabilityToPrint=1;}
                if (CyclesList.Count < 2)
                {
                    CurLabel.ttip.SetToolTip(CurLabel, string.Format("Вероятность прекращения {0}% \r(Нужно ввести по меньшей мере 2 цикла, чтобы \rотобразилась более точная статистика)", ProbabilityToPrint));
                }
                else 
                {
                    CurLabel.ttip.SetToolTip(CurLabel, string.Format("Вероятность прекращения {0}%", ProbabilityToPrint));
                }
                CurLabel.ttip.InitialDelay = 100;
                //CurLabel.
                CurLabel.ContextMenu.MenuItems[2].Enabled = true;
                CurLabel.ContextMenu.MenuItems[0].Enabled = false;
                CurLabel.ContextMenu.MenuItems[1].Enabled = true;
                Cle.End = CurLabel.chislo;
                if (percent <= 0) break;
                //MessageBox.Show("dfsdfsf");
                //for (int i = 0; i < 30; i++)
                //{
                //    System.Threading.Thread.Sleep(100);
                //    Application.DoEvents();
                //} 
                
            }
            CyclesList[Cycle.IndexOfNoEndElement] = new Cycle(Cle.Start, Cle.Start.AddDays(Calendar.statistics.getMaxMenstrPerPeriod(Mf)), true);
            //CalcPredictedMenstruation();

           
        }

        public void onCyclesListChanged(object sender, CyclesChangeEventArgs e)
        {
            SaveCyclesList();
            updateNoEndCycle();
            CalcPredictedMenstruation();            
            Mf.ShowStats();
        }

        public void invokeCyclesListChangeEvent() // возможность вызова события для внешних классов
        {
            CyclesListChangeEventHandler handler = RaiseCyclesListChange;
            if (handler != null) handler(this, new CyclesChangeEventArgs());
        }

        public struct Dimensions
        {
            static int CalendarXSize = 4;
            static int CalendarYSize = 3;
            public static int x
            {
                get { return CalendarXSize; }

                set { CalendarXSize = value; }
            }

            public static int y
            {
                get { return CalendarYSize; }

                set { CalendarYSize = value; }
            }


        }
 
        public Calendar(MainForm Mainf)
        {
            Mf = Mainf;
            AllYears = new List<Year>();
            for (int i = today.Year - Mf.yearsBeforeCurYear; i <= today.Year + Mf.yearsAfterCurYear; i++)
            {
                AllYears.Add(new Year(i, Mf));

            };
            RaiseCyclesListChange += new CyclesListChangeEventHandler(onCyclesListChanged);
            
        }

        public class statistics
        {
            

            public static float getAverageCycle(MainForm Mainf) //имеем в виду длительность месячного цикла
            {
                List<Cycle> Cycles = Mainf.CL.CyclesList;
                Cycles.Sort();
                if (Cycles.Count > Mainf.monthsForPrediction) Cycles = Cycles.GetRange(Cycles.Count - Mainf.monthsForPrediction, Mainf.monthsForPrediction);

                int DaysCounted = 0;
                int i=0;
                if (Cycles.Count > 1)
                {
                    for (i = 1; i <= Cycles.Count - 1; i++)
                    {
                        DaysCounted += (Cycles[i].Start - Cycles[i - 1].Start).Days;
                    }

                    return (float)DaysCounted / (Cycles.Count-1);
                }
                else
                {
                    return -1;
                }
            }

            public static int getMinCyclePerPeriod(MainForm Mainf) //имеем в виду длительность месячного цикла
            {
                List<Cycle> Cycles = Mainf.CL.CyclesList;
                Cycles.Sort();
                if (Cycles.Count > Mainf.monthsForPrediction) Cycles = Cycles.GetRange(Cycles.Count - Mainf.monthsForPrediction, Mainf.monthsForPrediction);
              
                int minDur = 32767;
                int curDur;
                int i = 0;
                if (Cycles.Count > 1)
                {
                    for (i = 1; i <= Cycles.Count - 1; i++)
                    {
                        curDur = (Cycles[i].Start - Cycles[i - 1].Start).Days;
                        if (curDur < minDur) minDur = curDur;
                    }

                    return minDur;
                }
                else
                {
                    return -1;
                }
            }

            public static int getMaxMenstrPerPeriod(MainForm Mainf) {
                List<Cycle> Cycles = Mainf.CL.CyclesList;
                Cycles.Sort();
                TimeSpan _MaxMenst= new TimeSpan(4,0,0,0);
                if (Cycles.Count > Mainf.monthsForPrediction) Cycles = Cycles.GetRange(Cycles.Count - Mainf.monthsForPrediction, Mainf.monthsForPrediction);
                if (Cycles.Count > 0)
                {
                    for (int i = 0; i <= Cycles.Count - 1; i++)
                    {
                        if (Cycles[i].Days > _MaxMenst && Cycles[i].HasNoEnd != true) _MaxMenst = Cycles[i].Days;
                        
                    }
                }
                return _MaxMenst.Days;
            }

            public static int getMaxCyclePerPeriod(MainForm Mainf) //имеем в виду длительность месячного цикла
            {
                List<Cycle> Cycles = Mainf.CL.CyclesList;
                Cycles.Sort();

                if (Cycles.Count > Mainf.monthsForPrediction) Cycles = Cycles.GetRange(Cycles.Count - Mainf.monthsForPrediction, Mainf.monthsForPrediction);
                int maxDur = 0;
                int curDur;
                int i = 0;
                if (Cycles.Count > 1)
                {
                    for (i = 1; i <= Cycles.Count - 1; i++)
                    {
                        curDur = (Cycles[i].Start - Cycles[i - 1].Start).Days;
                        if (curDur > maxDur) maxDur = curDur;
                    }

                    return maxDur;
                }
                else
                {
                    return -1;
                }
            }


            public static int getMaxCycle(MainForm Mainf) //имеем в виду длительность месячного цикла
            {
                List<Cycle> Cycles = Mainf.CL.CyclesList;
                Cycles.Sort();
                if (Cycles.Count > Mainf.monthsForPrediction) Cycles = Cycles.GetRange(Cycles.Count - Mainf.monthsForPrediction, Mainf.monthsForPrediction);

                //if (Cycles.Count > Mainf.monthsForPrediction) Cycles = Cycles.GetRange(Cycles.Count - Mainf.monthsForPrediction, Cycles.Count - 1);
                int maxDur = 0;
                int curDur;
                int i = 0;
                if (Cycles.Count > 1)
                {
                    for (i = 1; i <= Cycles.Count - 1; i++)
                    {
                        curDur=(Cycles[i].Start - Cycles[i - 1].Start).Days;
                        if (curDur > maxDur) maxDur = curDur;
                    }

                    return maxDur;
                }
                else
                {
                    return -1;
                }
            }

            public static int getMinCycle(MainForm Mainf) //имеем в виду длительность месячного цикла
            {
                List<Cycle> Cycles = Mainf.CL.CyclesList;
                Cycles.Sort();
                if (Cycles.Count > Mainf.monthsForPrediction) Cycles = Cycles.GetRange(Cycles.Count - Mainf.monthsForPrediction, Mainf.monthsForPrediction);

                int minDur = 32767;
                int curDur;
                int i = 0;
                if (Cycles.Count > 1)
                {
                    for (i = 1; i <= Cycles.Count - 1; i++)
                    {
                        curDur = (Cycles[i].Start - Cycles[i - 1].Start).Days;
                        if (curDur < minDur) minDur = curDur;
                    }

                    return minDur;
                }
                else
                {
                    return -1;
                }
            }


            public static DateTime getBeginDate(MainForm Mainf) //имеем в виду длительность месячного цикла
            {
                List<Cycle> Cycles = Mainf.CL.CyclesList;
                Cycles.Sort();
                if (Cycles.Count > 0)
                {
                    return Cycles[0].Start;
                }
                else return new DateTime(1800,1,1);
            }

            public static DateTime getEndDate(MainForm Mainf) //имеем в виду длительность месячного цикла
            {
                List<Cycle> Cycles = Mainf.CL.CyclesList;
                Cycles.Sort();
                if (Cycles.Count > 0)
                {
                    if (Cycles[Cycles.Count - 1].HasNoEnd == true) return Cycles[Cycles.Count - 1].Start;
                    else return Cycles[Cycles.Count - 1].End;
                }
                else return new DateTime(1800, 1, 1);
            }

        }

    }

    public class DateLabel : Label , IComparable
    {


        public DateTime chislo;
        //public byte chanceOfEndOfCycle;
        public ToolTip ttip;
        public Color previousBackColor;
        public Color MestruationColor;
        public Color MentruationPredictionColor;
        public Color OvulationColor;
        MainForm Mf;
        

        public DateLabel(DateTime ch, MainForm MainF)
        {
            Mf = MainF;
            TextAlign = ContentAlignment.MiddleCenter;
            chislo = ch;
            MestruationColor = Mf.BackColor;
            MentruationPredictionColor = Mf.BackColor;
            OvulationColor = Mf.BackColor;
            Text = ch.Day.ToString();
            ttip = new ToolTip();

            Anchor = System.Windows.Forms.AnchorStyles.Bottom;

            AutoSize = true;

            Location = new System.Drawing.Point(0, 0);

            //DoubleClick += new EventHandler(lbl_Click);
            MouseClick += new MouseEventHandler(lbl_MouseClick);
            
            
            MouseEnter+= new EventHandler(lbl_onmouseOver);
            MouseLeave += new EventHandler(lbl_onmouseOut);

            System.Windows.Forms.ContextMenu contextMenu1;
            contextMenu1 = new System.Windows.Forms.ContextMenu();
            System.Windows.Forms.MenuItem menuItem1;
            menuItem1 = new System.Windows.Forms.MenuItem();
            System.Windows.Forms.MenuItem menuItem2;
            menuItem2 = new System.Windows.Forms.MenuItem();
            System.Windows.Forms.MenuItem menuItem3;
            menuItem3 = new System.Windows.Forms.MenuItem();
            contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] { menuItem1, menuItem2, menuItem3 });
            menuItem1.Index = 0;
            menuItem1.Text = "Добавить месячные";
            menuItem2.Index = 1;
            menuItem2.Text = "Изменить месячные";
            menuItem2.Enabled = false;
            menuItem3.Index = 2;
            menuItem3.Text = "Удалить месячные";
            menuItem3.Enabled = false;

            ContextMenu = contextMenu1;
            

            //MessageBox.Show(ContextMenu.Handle.ToString());
            //ContextMenu.SourceControl.Enabled = false;
            ContextMenu.MenuItems[0].Click += new EventHandler(lbl_AddCycleSelected);
            ContextMenu.MenuItems[1].Click += new EventHandler(lbl_AddCycleSelected);
            ContextMenu.MenuItems[2].Click += new EventHandler(lbl_DelCycleSelected);

            


        }
        public void blendColors()
        {
            if (MestruationColor != Mf.BackColor && OvulationColor != Mf.BackColor && MentruationPredictionColor == Mf.BackColor)
            {
                BackColor = Color.FromArgb( //смешиваем цвета
                                (MestruationColor.R + OvulationColor.R) / 2,
                                (MestruationColor.G + OvulationColor.G) / 2,
                                (MestruationColor.B + OvulationColor.B) / 2
                                );
                ForeColor = MainForm.foreOfPrediction;
                previousBackColor = BackColor;
                return;
            }
            if (MestruationColor == Mf.BackColor && OvulationColor != Mf.BackColor && MentruationPredictionColor != Mf.BackColor)
            {
                BackColor = Color.FromArgb( //смешиваем цвета
                                (MentruationPredictionColor.R + OvulationColor.R) / 2,
                                (MentruationPredictionColor.G + OvulationColor.G) / 2,
                                (MentruationPredictionColor.B + OvulationColor.B) / 2
                                );
                ForeColor = MainForm.foreOfPrediction;
                previousBackColor = BackColor;
                return;
            }
            if (MestruationColor == Mf.BackColor && OvulationColor == Mf.BackColor && MentruationPredictionColor != Mf.BackColor)
            {
                BackColor = MentruationPredictionColor;
                previousBackColor = BackColor;
                return;
            }
            if (MestruationColor == Mf.BackColor && OvulationColor != Mf.BackColor && MentruationPredictionColor == Mf.BackColor)
            {
                BackColor = OvulationColor;
                ForeColor = MainForm.foreOfPrediction;
                previousBackColor = BackColor;
                return;
            }
            if (MestruationColor != Mf.BackColor && OvulationColor == Mf.BackColor && MentruationPredictionColor == Mf.BackColor)
            {
                BackColor = MestruationColor;
                ForeColor = MainForm.MentruationForeColor;
                previousBackColor = BackColor;
                return;
            }
            if (MestruationColor == Mf.BackColor && OvulationColor == Mf.BackColor && MentruationPredictionColor == Mf.BackColor)
            {
                BackColor = Mf.BackColor;
                ForeColor = Mf.ForeColor;
                previousBackColor = BackColor;
                return;
            }
        }

        public int CompareTo(Object obj)
        {
            if (obj is DateLabel)
            {
                DateLabel dlbl = (DateLabel)obj;
                return chislo.CompareTo(dlbl.chislo);
            }
            else
             throw new ArgumentException("Object is not a DateLabel"); 

        }

        private void lbl_onmouseOver(object sender, EventArgs e)
        {
            DateLabel dtlbl = (DateLabel)sender;
            dtlbl.previousBackColor = dtlbl.BackColor;
            dtlbl.BackColor = Color.FromArgb(99,234,244);
            
            

            
        }

        private void lbl_onmouseOut(object sender, EventArgs e)
        {
            DateLabel dtlbl = (DateLabel)sender;
            dtlbl.BackColor = dtlbl.previousBackColor;
            
        }

        private void lbl_MouseClick(object sender, MouseEventArgs e)
        {
            DateLabel dtlbl = (DateLabel)sender;
            if (e.Button == MouseButtons.Left) {
                lbl_Click(sender, e);
            }
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu.Show(dtlbl, new Point(0, dtlbl.Height));
            }
        }

        private void lbl_Click(object sender, EventArgs e)
        {
            DateLabel dtlbl = (DateLabel)sender;
            //MessageBox.Show(dtlbl.chislo.ToShortDateString());
            NewCycleControlDialog subForm;
            Calendar.Cycle GCycle = new Calendar.Cycle();
            GCycle.Start =  dtlbl.chislo;
            GCycle.End = dtlbl.chislo;
            int idx=Mf.CL.CyclesList.IndexOf(GCycle);
            Calendar.Cycle ClickedCycle;
            if (idx >= 0)
            {
                ClickedCycle = Mf.CL.CyclesList[idx];
                subForm = new NewCycleControlDialog(dtlbl.chislo, dtlbl.Mf, ClickedCycle);
            }
            else
            {
                subForm = new NewCycleControlDialog(dtlbl.chislo, dtlbl.Mf);
            }
            subForm.Show();
        }

        private void lbl_AddCycleSelected(object sender, EventArgs e)
        {
            MenuItem it = (MenuItem)sender;
            ContextMenu ct = (ContextMenu)it.Parent;
            
            lbl_Click(ct.SourceControl, e);
            
        }



        private void lbl_DelCycleSelected(object sender, EventArgs e)
        {
            MenuItem it = (MenuItem)sender;
            ContextMenu ct = (ContextMenu)it.Parent;
            DateLabel lbl = (DateLabel) ct.SourceControl;
            Calendar.Cycle GenericCycle = new Calendar.Cycle(lbl.chislo);
            this.Mf.CL.delete_Cycle(GenericCycle);
            this.Mf.CL.SaveCyclesList();
            this.Mf.CL.CalcPredictedMenstruation();
            this.Mf.ShowStats();
            
            SendKeys.Send("{ESC}");
        }




    }

    public class Year
    {

        public static int Current;
        public static int CurrentYearArrayIndex;
        private int NYear;
        public List<Month> MonthsList;

        public int NumYear
        {
            get
            {
                return NYear;
            }

        }
        MainForm Mf;
        public Year(int yr, MainForm MainF)
        {
            Mf = MainF;
            NYear = yr;
            MonthsList = new List<Month>();
            for (int i = 1; i <= 12; i++)
            {
                MonthsList.Add(new Month(i, yr, Mf));
            }


        }


    }

    public class Month
    {

        public List<DateLabel> DateLabels;

        private System.Windows.Forms.Label Mon;
        private System.Windows.Forms.Label Tue;
        private System.Windows.Forms.Label Wed;
        private System.Windows.Forms.Label Thu;
        private System.Windows.Forms.Label Fri;
        private System.Windows.Forms.Label Sat;
        private System.Windows.Forms.Label Sun;
        public System.Windows.Forms.Panel MonthPanel;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel DaydOfWeekTableLayoutPanel;
        private System.Windows.Forms.Label MonthTitleLabel;
        private System.Windows.Forms.Label UnderLineStringLabel;


        MainForm Mf;
        public Month(int mth, int yr, MainForm MainF)
        {
            Mf = MainF;
            int currentXMuliplier = (mth - 1) % Calendar.Dimensions.x;
            int currentYMuliplier = (int)(mth - 1) / Calendar.Dimensions.x;
            string mthStringNumber = mth.ToString();


            Mon = new System.Windows.Forms.Label();
            Tue = new System.Windows.Forms.Label();
            Wed = new System.Windows.Forms.Label();
            Thu = new System.Windows.Forms.Label();
            Fri = new System.Windows.Forms.Label();
            Sat = new System.Windows.Forms.Label();
            Sun = new System.Windows.Forms.Label();
            MonthPanel = new System.Windows.Forms.Panel();
            MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            DaydOfWeekTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            MonthTitleLabel = new System.Windows.Forms.Label();
            UnderLineStringLabel = new System.Windows.Forms.Label();

            // 
            // Mon
            // 
            Mon.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            Mon.AutoSize = true;
            Mon.Location = new System.Drawing.Point(2, 8);
            Mon.Name = "Mon" + mthStringNumber;
            Mon.Size = new System.Drawing.Size(21, 13);
            Mon.TabIndex = 0;
            Mon.Text = Calendar.DaysOfWeek[0];
            // 
            // Tue
            // 
            Tue.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            Tue.AutoSize = true;
            Tue.Location = new System.Drawing.Point(31, 8);
            Tue.Name = "Tue" + mthStringNumber;
            Tue.Size = new System.Drawing.Size(22, 13);
            Tue.TabIndex = 1;
            Tue.Text = Calendar.DaysOfWeek[1];
            // 
            // Wed
            // 
            Wed.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            Wed.AutoSize = true;
            Wed.Location = new System.Drawing.Point(59, 8);
            Wed.Name = "Wed" + mthStringNumber;
            Wed.Size = new System.Drawing.Size(21, 13);
            Wed.TabIndex = 2;
            Wed.Text = Calendar.DaysOfWeek[2];
            // 
            // Thu
            // 
            Thu.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            Thu.AutoSize = true;
            Thu.Location = new System.Drawing.Point(87, 8);
            Thu.Name = "Thu" + mthStringNumber;
            Thu.Size = new System.Drawing.Size(22, 13);
            Thu.TabIndex = 3;
            Thu.Text = Calendar.DaysOfWeek[3];            
            // 
            // Fri
            // 
            Fri.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            Fri.AutoSize = true;
            Fri.Location = new System.Drawing.Point(118, 8);
            Fri.Name = "Fri" + mthStringNumber;
            Fri.Size = new System.Drawing.Size(15, 13);
            Fri.TabIndex = 4;
            Fri.Text = Calendar.DaysOfWeek[4];
            // 
            // Sat
            // 
            Sat.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            Sat.AutoSize = true;
            Sat.Location = new System.Drawing.Point(143, 8);
            Sat.Name = "Sat" + mthStringNumber;
            Sat.Size = new System.Drawing.Size(21, 13);
            Sat.TabIndex = 5;
            Sat.Text = Calendar.DaysOfWeek[5];
            // 
            // Sun
            // 
            Sun.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            Sun.AutoSize = true;
            Sun.Location = new System.Drawing.Point(172, 8);
            Sun.Name = "Sun" + mthStringNumber;
            Sun.Size = new System.Drawing.Size(23, 13);
            Sun.TabIndex = 6;
            Sun.Text = Calendar.DaysOfWeek[6];


            // 
            // label1
            // 
            MonthTitleLabel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            MonthTitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            MonthTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            MonthTitleLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            MonthTitleLabel.Location = new System.Drawing.Point(0, 0);
            MonthTitleLabel.Name = "MonthTitleLabel" + mthStringNumber; ;
            MonthTitleLabel.Size = new System.Drawing.Size(200, 28);
            MonthTitleLabel.TabIndex = 0;
            MonthTitleLabel.Text = Calendar.MonthsArray[mth - 1];
            MonthTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;


            // 
            // label3
            // 
            UnderLineStringLabel.Location = new System.Drawing.Point(0, 35);
            UnderLineStringLabel.Name = "UnderLineStringLabel" + mthStringNumber; ;
            UnderLineStringLabel.Size = new System.Drawing.Size(200, 13);
            UnderLineStringLabel.TabIndex = 3;
            UnderLineStringLabel.Text = "_________________________________";
            UnderLineStringLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            // 
            // DaydOfWeekTableLayoutPanel
            // 
            DaydOfWeekTableLayoutPanel.ColumnCount = 7;
            DaydOfWeekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            DaydOfWeekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            DaydOfWeekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            DaydOfWeekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            DaydOfWeekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            DaydOfWeekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            DaydOfWeekTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            DaydOfWeekTableLayoutPanel.Controls.Add(Mon, 0, 0);
            DaydOfWeekTableLayoutPanel.Controls.Add(Tue, 1, 0);
            DaydOfWeekTableLayoutPanel.Controls.Add(Wed, 2, 0);
            DaydOfWeekTableLayoutPanel.Controls.Add(Thu, 3, 0);
            DaydOfWeekTableLayoutPanel.Controls.Add(Fri, 4, 0);
            DaydOfWeekTableLayoutPanel.Controls.Add(Sat, 5, 0);
            DaydOfWeekTableLayoutPanel.Controls.Add(Sun, 6, 0);
            DaydOfWeekTableLayoutPanel.Location = new System.Drawing.Point(0, 25);
            DaydOfWeekTableLayoutPanel.Name = "DaydOfWeekTableLayoutPanel" + mthStringNumber; ;
            DaydOfWeekTableLayoutPanel.RowCount = 1;
            DaydOfWeekTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            DaydOfWeekTableLayoutPanel.Size = new System.Drawing.Size(200, 21);
            DaydOfWeekTableLayoutPanel.TabIndex = 2;


            // 
            // MainTableLayoutPanel
            // 
            MainTableLayoutPanel.ColumnCount = 7;
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28143F));
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28643F));
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28643F));
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28643F));
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28643F));
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28643F));
            MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28643F));
            MainTableLayoutPanel.Location = new System.Drawing.Point(0, 50);
            MainTableLayoutPanel.Name = "MainTableLayoutPanel" + mthStringNumber; ;
            MainTableLayoutPanel.RowCount = 6;
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.2835F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.2835F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.2835F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.2835F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28779F));
            MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28919F));
            MainTableLayoutPanel.Size = new System.Drawing.Size(200, 125);
            MainTableLayoutPanel.TabIndex = 1;



            MainTableLayoutPanel.SuspendLayout();

            DateTime dtm = new DateTime(yr, mth, 1);
            int firstDay = (int)dtm.DayOfWeek - 1;
            if (firstDay < 0) firstDay = 6;
            int mx = firstDay;
            int my = 0;
            DateLabels = new List<DateLabel>();
            while (dtm.Month == mth)
            {
                DateLabel Lbl = new DateLabel(dtm, Mf);

                MainTableLayoutPanel.Controls.Add(Lbl, mx, my);
                if (mx < 6) mx++; else { mx = 0; my++; }
                DateLabels.Add(Lbl);
                dtm = dtm.AddDays(1);
            }

            MainTableLayoutPanel.ResumeLayout(false);


            // 
            // MonthPanel
            // 
            MonthPanel.SuspendLayout();

            MonthPanel.Controls.Add(MainTableLayoutPanel);
            MonthPanel.Controls.Add(MonthTitleLabel);
            MonthPanel.Controls.Add(DaydOfWeekTableLayoutPanel);
            MonthPanel.Controls.Add(UnderLineStringLabel);

            MonthPanel.Name = "MonthPanel" + mthStringNumber;
            MonthPanel.Size = MainForm.MonthPanelSize;
            //MonthPanel.Margin = new Padding(8);
            MonthPanel.Location = new System.Drawing.Point((int)(MainForm.MonthPanelSize.Width * currentXMuliplier * 1.08) + MainForm.YearBordersPadding, (int)(MainForm.MonthPanelSize.Height * currentYMuliplier * 1.03) + 50);
            MonthPanel.TabIndex = 3;

            MonthPanel.ResumeLayout(false);


        }



    }



}



