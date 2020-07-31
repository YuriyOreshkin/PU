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
    public partial class PereschetNachVznos : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        RSW2014_6_Copy_Wait child = new RSW2014_6_Copy_Wait();
        public BackgroundWorker bw = new BackgroundWorker();
        List<Staff> staffList = new List<Staff>();
        private List<RaschetPeriodContainer> RaschPer { get; set; }


        public PereschetNachVznos()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(calculation);
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

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startBtn.Enabled = true;
            closeBtn.Enabled = true;

            this.child.Close();
            Methods.showAlert("Внимание", "Перерасчет начисленных взносов в ПФР произведен!", this.ThemeName);
            this.Close();
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

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PereschetNachVznos_FormClosing(object sender, FormClosingEventArgs e)
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

        private void calculation(object sender, DoWorkEventArgs e)
        {
            int k = 0;
            byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
            short y = short.Parse(Year.Text);
            byte qk = (byte)0;
            short yk = (short)0;
            MethodsNonStatic methods = new MethodsNonStatic();

            short year = y;

            if (TypeInfo.SelectedIndex == 1) // корр данные
            {
                qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
                yk = short.Parse(YearKorr.Text);
                year = yk;
            }

            foreach (var staff in staffList)
            {
                if (bw.CancellationPending)
                {
                    staffList.Clear();
                    return;
                }
                k++;
                FormsRSW2014_1_Razd_6_1 rsw6 = new FormsRSW2014_1_Razd_6_1();


                if (TypeInfo.SelectedIndex == 0) // исходные данные
                {
                    rsw6 = staff.FormsRSW2014_1_Razd_6_1.FirstOrDefault(c => c.TypeInfoID == 1 && c.Year == y && c.Quarter == q);
                }
                else // корректирующие
                {
                    rsw6 = staff.FormsRSW2014_1_Razd_6_1.FirstOrDefault(c => c.TypeInfoID == 2 && c.Year == y && c.Quarter == q && c.YearKorr == yk && c.QuarterKorr == qk);
                }

                if (rsw6 != null && rsw6.FormsRSW2014_1_Razd_6_4.Count > 0)
                {
                    rsw6.SumFeePFR = methods.UpdateSumFeePFR(false, rsw6.FormsRSW2014_1_Razd_6_4.ToList(), year);
                    db.ObjectStateManager.ChangeObjectState(rsw6, EntityState.Modified);
                }
                decimal temp = (decimal)k / (decimal)staffList.Count();
                int proc = (int)Math.Round((temp * 100), 0);
                bw.ReportProgress(proc, k.ToString());

                if (k % 50 == 0)
                {
                    db.SaveChanges();
                }

            }

            db.SaveChanges();

        }

        private void PereschetNachVznos_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            foreach (var item in db.TypeInfo.Where(x => x.ID <= 2))
            {
                TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }

            TypeInfo.SelectedIndex = 0;

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

            this.TypeInfo.SelectedIndexChanged += (s, с) => TypeInfo_SelectedIndexChanged();

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

        private void startBtn_Click(object sender, EventArgs e)
        {
            RSW2014_List main = this.Owner as RSW2014_List;

            if (main != null)
            {
                if (main.staffGridView.RowCount > 0 && ((TypeInfo.SelectedIndex == 0 && Year.Text != "" && Quarter.Text != "") || (TypeInfo.SelectedIndex == 1 && Year.Text != "" && Quarter.Text != "" && YearKorr.Text != "" && QuarterKorr.Text != "")))
                {
                    List<long> staffIDList = new List<long>();

                    switch (copyRangeDDL.SelectedIndex)
                    {
                        case 0:
                            staffIDList.Add(Convert.ToInt64(main.staffGridView.CurrentRow.Cells[1].Value));
                            break;
                        case 1:
                            int rowindex = main.staffGridView.CurrentRow.Index;
                            if (main.staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                            {
                                foreach (var item in main.staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                {
                                    staffIDList.Add(long.Parse(item.Cells[1].Value.ToString()));
                                }
                            }
                            else
                            {
                                staffIDList.Add(long.Parse(main.staffGridView.Rows[rowindex].Cells[1].Value.ToString()));
                            }

                            break;
                        case 2:
                            foreach (var row in main.staffGridView.Rows)
                            {
                                staffIDList.Add(Convert.ToInt64(row.Cells[1].Value));
                            }
                            break;
                    }

                    byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
                    short y = short.Parse(Year.Text);

                    if (TypeInfo.SelectedIndex == 0) // исходные данные
                    {
                        staffList = db.Staff.Where(x => staffIDList.Contains(x.ID) && x.FormsRSW2014_1_Razd_6_1.Any(c => c.TypeInfoID == 1 && c.Year == y && c.Quarter == q)).ToList();
                    }
                    else // корректирующие и отменяющие
                    {
                        byte qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
                        short yk = short.Parse(YearKorr.Text);
                        staffList = db.Staff.Where(x => staffIDList.Contains(x.ID) && x.FormsRSW2014_1_Razd_6_1.Any(c => c.TypeInfoID == 2 && c.Year == y && c.Quarter == q && c.YearKorr == yk && c.QuarterKorr == qk)).ToList();
                    }



                    bw.RunWorkerAsync();
                    startBtn.Enabled = false;
                    closeBtn.Enabled = false;


                    child = new RSW2014_6_Copy_Wait();
                    child.Owner = this;
                    child.ownerName = this.Name;
                    child.ThemeName = this.ThemeName;
                    child.titleLabel.Text = "Перерасчет начисленных взносов в ПФР";
                    child.secondPartLabel.Text = staffList.Count().ToString();
                    child.Show();
                }
                else
                {
                    RadMessageBox.Show(this, "Нет данных для обработки. Не удалось получить список сотрудников!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }

        }

    }
}
