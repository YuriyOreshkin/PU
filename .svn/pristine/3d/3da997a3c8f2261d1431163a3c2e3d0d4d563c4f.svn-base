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
    public partial class RaschDonachSum : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<long> staffList = new List<long>();
        private List<RaschetPeriodContainer> RaschPer { get; set; }
        List<FormsRSW2014_1_Razd_6_1> rsw61List;
        RSW2014_6_Copy_Wait child = new RSW2014_6_Copy_Wait();
        public BackgroundWorker bw = new BackgroundWorker();
        //        public short y = 0;
        //        public byte q = 20;

        public RaschDonachSum()
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

                rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 1).ToList();

                if (rsw61List.Count > 0)
                {

                    bw.RunWorkerAsync();
                    startBtn.Enabled = false;
                    closeBtn.Enabled = false;



                    child = new RSW2014_6_Copy_Wait();
                    child.Owner = this;
                    child.ownerName = this.Name;
                    child.ThemeName = this.ThemeName;
                    child.titleLabel.Text = "Расчет доначисленных взносов";
                    child.secondPartLabel.Text = rsw61List.Count().ToString();
                    child.Show();
                }
                else
                {
                    RadMessageBox.Show(this, "Нет данных для обработки. Не найдены исходные данные Индивидуальных сведений за выбранный Отчетный период!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }
        }

        private void calculation(object sender, DoWorkEventArgs e)
        {
            int k = 0;
            byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
            short y = short.Parse(Year.Text);
            byte qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
            short yk = short.Parse(YearKorr.Text);

            foreach (var item in rsw61List)
            {
                if (bw.CancellationPending)
                {
                    rsw61List.Clear();
                    return;
                }
                k++;
                decimal s = 0;
                decimal n = 0;
                //                FormsRSW2014_1_Razd_6_6 rsw66_old = db.FormsRSW2014_1_Razd_6_6.FirstOrDefault(x => x.FormsRSW2014_1_Razd_6_1_ID == item.ID && x.AccountPeriodYear == yk && x.AccountPeriodQuarter == qk);

                FormsRSW2014_1_Razd_6_6 rsw66_new = new FormsRSW2014_1_Razd_6_6 { FormsRSW2014_1_Razd_6_1_ID = item.ID, AccountPeriodYear = yk, AccountPeriodQuarter = qk, SumFeePFR_D = 0, SumFeePFR_NakopD = 0, SumFeePFR_StrahD = 0 };


                if (yk >= 2014)
                {
                    if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID >= 2 && x.StaffID == item.StaffID))
                    {
                        FormsRSW2014_1_Razd_6_1 is_korr = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID >= 2 && x.StaffID == item.StaffID);
                        if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == is_korr.YearKorr.Value && x.Quarter == is_korr.QuarterKorr.Value && x.TypeInfoID == 1 && x.StaffID == item.StaffID))
                        {
                            FormsRSW2014_1_Razd_6_1 is_ishod = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == is_korr.YearKorr.Value && x.Quarter == is_korr.QuarterKorr.Value && x.TypeInfoID == 1 && x.StaffID == item.StaffID);

                            if (is_korr.SumFeePFR.HasValue && is_ishod.SumFeePFR.HasValue)
                            {
                                rsw66_new.SumFeePFR_D = is_korr.SumFeePFR.Value - is_ishod.SumFeePFR.Value;
                            }
                        }
                    }
                }
                else if (yk >= 2010 && yk <= 2012)
                {
                    foreach (var item_ in db.FormsSZV_6.Where(x => x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID >= 2 && x.StaffID == item.StaffID))
                    {
                        s = s + (item_.SumFeePFR_Strah_D.HasValue ? item_.SumFeePFR_Strah_D.Value : 0);
                        n = n + (item_.SumFeePFR_Nakop_D.HasValue ? item_.SumFeePFR_Nakop_D.Value : 0);
                    }
                    rsw66_new.SumFeePFR_StrahD = s;
                    rsw66_new.SumFeePFR_NakopD = n;

                }
                else if (yk == 2013)
                {
                    foreach (var item_ in db.FormsSZV_6_4.Where(x => x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID >= 2 && x.StaffID == item.StaffID))
                    {
                        s = s + (item_.SumFeePFR_Strah_D.HasValue ? item_.SumFeePFR_Strah_D.Value : 0);
                        n = n + (item_.SumFeePFR_Nakop_D.HasValue ? item_.SumFeePFR_Nakop_D.Value : 0);
                    }
                    rsw66_new.SumFeePFR_StrahD = s;
                    rsw66_new.SumFeePFR_NakopD = n;

                }

                if (Razd_6_6_ChkBox.Checked && (rsw66_new.SumFeePFR_D != 0 || rsw66_new.SumFeePFR_StrahD != 0 || rsw66_new.SumFeePFR_NakopD != 0))
                {
                    if (db.FormsRSW2014_1_Razd_6_6.Any(x => x.FormsRSW2014_1_Razd_6_1_ID == item.ID && x.AccountPeriodYear == yk && x.AccountPeriodQuarter == qk))
                    {
                        FormsRSW2014_1_Razd_6_6 rsw66_old = db.FormsRSW2014_1_Razd_6_6.FirstOrDefault(x => x.FormsRSW2014_1_Razd_6_1_ID == item.ID && x.AccountPeriodYear == yk && x.AccountPeriodQuarter == qk);
                        rsw66_old.SumFeePFR_D = Math.Round(rsw66_new.SumFeePFR_D.Value, 2, MidpointRounding.AwayFromZero);
                        rsw66_old.SumFeePFR_NakopD = Math.Round(rsw66_new.SumFeePFR_NakopD.Value, 2, MidpointRounding.AwayFromZero);
                        rsw66_old.SumFeePFR_StrahD = Math.Round(rsw66_new.SumFeePFR_StrahD.Value, 2, MidpointRounding.AwayFromZero);
                        db.ObjectStateManager.ChangeObjectState(rsw66_old, EntityState.Modified);
                        db.SaveChanges();

                    }
                    else
                    {
                        db.FormsRSW2014_1_Razd_6_6.AddObject(rsw66_new);
                        db.SaveChanges();
                    }
                }

                decimal temp = (decimal)k / (decimal)staffList.Count();
                int proc = (int)Math.Round((temp * 100), 0);
                bw.ReportProgress(proc, k.ToString());
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startBtn.Enabled = true;
            closeBtn.Enabled = true;

            this.child.Close();
            Methods.showAlert("Внимание", "Рассчет доначисленных взносов произведен!", this.ThemeName);
            //            this.Close();
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            child.radProgressBar1.Value1 = e.ProgressPercentage;
            child.firstPartLabel.Text = e.UserState.ToString();
        }

        private void RaschDonachSum_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

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

        private void RaschDonachSum_FormClosing(object sender, FormClosingEventArgs e)
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


    }
}
