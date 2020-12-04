using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using PU.Models;
using PU.Classes;
using Telerik.WinControls.UI;

namespace PU.FormsSZV_6_4_2013.DopFunc
{
    public partial class RaschDonachSumSZV64 : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<long> staffList = new List<long>();
        private List<RaschetPeriodContainer> RaschPer { get; set; }
        List<FormsSZV_6_4> szv64List;
        PU.FormsRSW2014.RSW2014_6_Copy_Wait child = new PU.FormsRSW2014.RSW2014_6_Copy_Wait();
        public BackgroundWorker bw = new BackgroundWorker();
        //        public short y = 0;
        //        public byte q = 20;

        public RaschDonachSumSZV64()
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
            SZV_6_4_List main = this.Owner as SZV_6_4_List;
            szv64List = new List<FormsSZV_6_4>();
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
                byte qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
                short yk = short.Parse(YearKorr.Text);

                szv64List = db.FormsSZV_6_4.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID == 2).ToList();

                if (szv64List.Count > 0)
                {

                    bw.RunWorkerAsync();
                    startBtn.Enabled = false;
                    closeBtn.Enabled = false;



                    child = new PU.FormsRSW2014.RSW2014_6_Copy_Wait();
                    child.Owner = this;
                    child.ownerName = this.Name;
                    child.ThemeName = this.ThemeName;
                    child.titleLabel.Text = "Расчет доначисленных взносов";
                    child.secondPartLabel.Text = szv64List.Count().ToString();
                    child.Show();
                }
                else
                {
                    RadMessageBox.Show(this, "Нет данных для обработки. Не найдены корректирующие формы за выбранный Отчетный период!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }
        }

        private void calculation(object sender, DoWorkEventArgs e)
        {
            int k = 0;

            pu6Entities db_temp = new pu6Entities();

            foreach (var item in szv64List)
            {
                if (bw.CancellationPending)
                {
                    szv64List.Clear();
                    return;
                }
                k++;

                if (item.PlatCategoryID == null || item.PlatCategoryID == 0) // если не заполнена категория в корр сведениях
                {
                    continue;
                }
                else
                {
                    long platCat = item.PlatCategoryID;
                    if (db_temp.FormsSZV_6_4.Any(x => x.PlatCategoryID == platCat && x.StaffID == item.StaffID && x.Year == item.YearKorr && x.Quarter == item.QuarterKorr && x.TypeInfoID == 1))
                    {
                        FormsSZV_6_4 szv64i = db_temp.FormsSZV_6_4.First(x => x.PlatCategoryID == platCat && x.StaffID == item.StaffID && x.Year == item.YearKorr && x.Quarter == item.QuarterKorr && x.TypeInfoID == 1);
                        bool flag = false;

                        if (szv64i.SumFeePFR_Strah.HasValue)
                        {
                            if (!item.SumFeePFR_Strah_D.HasValue || (item.SumFeePFR_Strah_D.HasValue && item.SumFeePFR_Strah_D.Value == 0) || (item.SumFeePFR_Strah_D.HasValue && item.SumFeePFR_Strah_D.Value != 0 && !IfExist_ChkBox.Checked))
                            {
                                item.SumFeePFR_Strah_D = (item.SumFeePFR_Strah.HasValue ? item.SumFeePFR_Strah.Value : 0) - szv64i.SumFeePFR_Strah.Value;
                                flag = true;
                            }
                        }
                        if (szv64i.SumFeePFR_Nakop.HasValue)
                        {
                            if (!item.SumFeePFR_Nakop_D.HasValue || (item.SumFeePFR_Nakop_D.HasValue && item.SumFeePFR_Nakop_D.Value == 0) || (item.SumFeePFR_Nakop_D.HasValue && item.SumFeePFR_Nakop_D.Value != 0 && !IfExist_ChkBox.Checked))
                            {
                                item.SumFeePFR_Nakop_D = (item.SumFeePFR_Nakop.HasValue ? item.SumFeePFR_Nakop.Value : 0) - szv64i.SumFeePFR_Nakop.Value;
                                flag = true;
                            }
                        }

                        if (szv64i.SumPayPFR_Strah.HasValue)
                        {
                            if (!item.SumPayPFR_Strah_D.HasValue || (item.SumPayPFR_Strah_D.HasValue && item.SumPayPFR_Strah_D.Value == 0) || (item.SumPayPFR_Strah_D.HasValue && item.SumPayPFR_Strah_D.Value != 0 && !IfExist_ChkBox.Checked))
                            {
                                item.SumPayPFR_Strah_D = (item.SumPayPFR_Strah.HasValue ? item.SumPayPFR_Strah.Value : 0) - szv64i.SumPayPFR_Strah.Value;
                                flag = true;
                            }
                        }
                        if (szv64i.SumPayPFR_Nakop.HasValue)
                        {
                            if (!item.SumPayPFR_Nakop_D.HasValue || (item.SumPayPFR_Nakop_D.HasValue && item.SumPayPFR_Nakop_D.Value == 0) || (item.SumPayPFR_Nakop_D.HasValue && item.SumPayPFR_Nakop_D.Value != 0 && !IfExist_ChkBox.Checked))
                            {
                                item.SumPayPFR_Nakop_D = (item.SumPayPFR_Nakop.HasValue ? item.SumPayPFR_Nakop.Value : 0) - szv64i.SumPayPFR_Nakop.Value;
                                flag = true;
                            }
                        }

                        if (flag)
                            db.Entry(item).State = EntityState.Modified;

                    }


                }

                if (k % 500 == 0)
                {
                    db.SaveChanges();
                }

                decimal temp = (decimal)k / (decimal)staffList.Count();
                int proc = (int)Math.Round((temp * 100), 0);
                bw.ReportProgress(proc, k.ToString());
            }

            db.SaveChanges();

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
            Messenger.showAlert(AlertType.Info, "Внимание", "Рассчет доначисленных взносов произведен!", this.ThemeName);
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
