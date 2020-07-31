using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using PU.Models;
using PU.Classes;
using Telerik.WinControls.UI;

namespace PU.FormsRSW2014
{
    public partial class ProsmotrStaj : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        private List<RaschetPeriodContainer> RaschPer { get; set; }
        RSW2014_6_Copy_Wait child = new RSW2014_6_Copy_Wait();
        public BackgroundWorker bw = new BackgroundWorker();
        public List<long> staffList = new List<long>();
        List<FormsRSW2014_1_Razd_6_1> rsw61List;
        private bool abort;

        public ProsmotrStaj()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);

        }

        /// <summary>
        /// Перехват нажатия на ESC для закрытия формы
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ProsmotrStaj_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            foreach (var item in db.TypeInfo.Where(x => x.ID != 3))
            {
                TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }

            TypeInfo.SelectedIndex = 0;
            this.TypeInfo.SelectedIndexChanged += (s, с) => TypeInfo_SelectedIndexChanged();

            // выпад список "календарный год"
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);

            foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }

            if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                Year.Items.OrderByDescending(x => x.Value).First().Selected = true;

            // выпад список "Отчетный период"

            Year_SelectedIndexChanged();

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();


            // выпад список "календарный год"
            RaschPer = new List<RaschetPeriodContainer> { };
            foreach (var item in Options.RaschetPeriodInternal2010_2013)
            {
                RaschPer.Add(item);
            }
            foreach (var item in Options.RaschetPeriodInternal)
            {
                RaschPer.Add(item);
            }

            this.YearKorr.Items.Clear();

            foreach (var item in RaschPer.Select(x => x.Year).Distinct().ToList())
            {
                YearKorr.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }

            if (YearKorr.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                YearKorr.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                YearKorr.Items.Last().Selected = true;

            this.YearKorr.SelectedIndexChanged += (s, с) => YearKorr_SelectedIndexChanged();
            YearKorr_SelectedIndexChanged();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startBtn.Enabled = true;
            closeBtn.Enabled = true;

            this.child.Close();
            if (!abort)
            {
                string reg = "";
                if (Options.InsID != 0 && db.Insurer.Any(x => x.ID == Options.InsID))
                {
                    Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

                    reg = Utils.ParseRegNum(ins.RegNum);
                }

                string title = "   -   " + (TypeInfo.SelectedIndex == 0 ? Quarter.Text : QuarterKorr.Text) + " [" + TypeInfo.Text + "]    Рег.№: " + reg;

                title = this.Text + title;

                ProsmotrStajGrid child = new ProsmotrStajGrid();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.rsw61List = rsw61List;
                child.Text = title;
                child.ShowDialog();
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            child.radProgressBar1.Value1 = e.ProgressPercentage;
            child.firstPartLabel.Text = e.UserState.ToString();
        }

        private void Year_SelectedIndexChanged()
        {
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);

            // выпад список "Отчетный период"

            this.Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                DateTime dt = DateTime.Now.Date;

                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                else
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                if (rp != null)
                    Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                else
                    Quarter.Items.OrderByDescending(x => x.Value).First().Selected = true;
            }
        }

        private void YearKorr_SelectedIndexChanged()
        {
            // выпад список "Отчетный период"
            this.QuarterKorr.Items.Clear();

            short y;
            if (short.TryParse(YearKorr.SelectedItem.Text, out y))
            {
                foreach (var item in RaschPer.Where(x => x.Year == y).ToList())
                {
                    QuarterKorr.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                DateTime dt = DateTime.Now.Date;

                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                if (RaschPer.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                    rp = RaschPer.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                else
                    rp = RaschPer.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                if (rp != null)
                    QuarterKorr.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                else
                    QuarterKorr.Items.OrderByDescending(x => x.Value).First().Selected = true;

            }
        }


        private void TypeInfo_SelectedIndexChanged()
        {
            if (TypeInfo.SelectedIndex == 0)
            {
                korrGroupBox.Enabled = false;
            }
            else
            {
                korrGroupBox.Enabled = true;
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProsmotrStaj_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bw != null && bw.IsBusy)
            {
                bw.CancelAsync();
            }
            if (bw != null)
            {
                bw.Dispose();
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            RSW2014_List main = this.Owner as RSW2014_List;
            rsw61List = new List<FormsRSW2014_1_Razd_6_1>();
            staffList = new List<long>();

            if (main != null)
            {
                if (main.staffGridView.RowCount > 0)
                {
                    switch (copyRangeDDL.SelectedIndex)
                    {
                        case 0:
                            staffList.Add(Convert.ToInt64(main.staffGridView.CurrentRow.Cells[1].Value));
                            break;
                        case 1:
                            int rowindex = main.staffGridView.CurrentRow.Index;
                            if (main.staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                            {
                                foreach (var item in main.staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                {
                                    staffList.Add(long.Parse(item.Cells[1].Value.ToString()));
                                }
                            }
                            else
                            {
                                staffList.Add(long.Parse(main.staffGridView.Rows[rowindex].Cells[1].Value.ToString()));
                            }
                            break;
                        case 2:
                            foreach (var row in main.staffGridView.Rows)
                            {
                                staffList.Add(Convert.ToInt64(row.Cells[1].Value));
                            }
                            break;
                    }
                }

                byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
                short y = short.Parse(Year.Text);

                if (TypeInfo.SelectedIndex == 0) // исходные данные
                {
                    rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == (TypeInfo.SelectedIndex + 1)).ToList();
                }
                else // корректирующие и отменяющие
                {
                    byte qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
                    short yk = short.Parse(YearKorr.Text);
                    rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID == (TypeInfo.SelectedIndex + 1)).ToList();
                }


                if (rsw61List.Count() > 0)
                {
                    string title = "Поиск данных о Стаже";
                    bw.DoWork += new System.ComponentModel.DoWorkEventHandler(calculation);
                    bw.RunWorkerAsync();
                    startBtn.Enabled = false;
                    closeBtn.Enabled = false;



                    child = new RSW2014_6_Copy_Wait();
                    child.Owner = this;
                    child.ownerName = this.Name;
                    child.ThemeName = this.ThemeName;
                    child.titleLabel.Text = title;
                    child.secondPartLabel.Text = rsw61List.Count().ToString();
                    child.Show();
                }
                else
                {
                    RadMessageBox.Show(this, "Нет данных для обработки. Не удалось получить список сотрудников!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }
        }

        private void calculation(object sender, DoWorkEventArgs e)
        {
            int k = 0;
            abort = false;

            foreach (var item in rsw61List)
            {
                if (bw.CancellationPending)
                {
                    rsw61List.Clear();
                    abort = true;
                    return;
                }

                k++;

                decimal temp = (decimal)k / (decimal)rsw61List.Count();
                int proc = (int)Math.Round((temp * 100), 0);
                bw.ReportProgress(proc, k.ToString());
            }
        }

    }
}
