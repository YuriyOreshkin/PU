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
    public partial class CopyStaj : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<long> staffList = new List<long>();
        private List<RaschetPeriodContainer> RaschPer { get; set; }
        List<FormsRSW2014_1_Razd_6_1> rsw61List;
        RSW2014_6_Copy_Wait child = new RSW2014_6_Copy_Wait();
        public BackgroundWorker bw = new BackgroundWorker();

        public CopyStaj()
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

        private void prevYearRadioButton_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (prevYearRadioButton.IsChecked)
            {
                primerLabel.Text = "Будет заменен календарный год в копируемых записях о Стаже. Например для Отчетного периода 1 квартал 2015 года: предыдущий период Стажа 1 квартал 2014 года (01.01.2014-31.03.2014 будет скопирован как 01.01.2015-31.03.2015)";
            }
            else if (prevPeriodRadioButton.IsChecked)
            {
                primerLabel.Text = "Будет заменен месяц в копируемых записях о Стаже. Например для Отчетного периода полугодие 2015 года: предыдущий период Стажа 1 квартал 2015 года (01.01.2015-31.03.2015 будет скопирован как 01.04.2015-30.06.2015)";
            }
            else if (korrPeriodRadioButton.IsChecked)
            {
                primerLabel.Text = "";
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CopyStaj_FormClosing(object sender, FormClosingEventArgs e)
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

        private void CopyStaj_Load(object sender, EventArgs e)
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
            this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();

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
            prevYearRadioButton_ToggleStateChanged(null, null);
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startBtn.Enabled = true;
            closeBtn.Enabled = true;

            this.child.Close();
            Methods.showAlert("Внимание", "Копирование Стажа завершено!", this.ThemeName);
            this.Close();
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            child.radProgressBar1.Value1 = e.ProgressPercentage;
            child.firstPartLabel.Text = e.UserState.ToString();
        }



        private void TypeInfo_SelectedIndexChanged()
        {
            if (TypeInfo.SelectedIndex == 0)
            {
                korrGroupBox.Enabled = false;
                if (korrPeriodRadioButton.IsChecked)
                {
                    korrPeriodRadioButton.IsChecked = false;
                    prevYearRadioButton.IsChecked = true;
                }
                korrPeriodRadioButton.Enabled = false;

            }
            else
            {
                korrGroupBox.Enabled = true;
                korrPeriodRadioButton.Enabled = true;
            }
        }

        private void Quarter_SelectedIndexChanged()
        {
            if (Quarter.Items.Count() > 0)
            {
                byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
                if (q == 3)
                {
                    if (prevPeriodRadioButton.IsChecked)
                    {
                        prevPeriodRadioButton.IsChecked = false;
                        prevYearRadioButton.IsChecked = true;
                    }
                    prevPeriodRadioButton.Enabled = false;
                }
                else
                {
                    prevPeriodRadioButton.Enabled = true;
                }
            }
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
            staffList = new List<long>();
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
                    rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 1 && x.StajOsn.Count() == 0).ToList();
                }
                else // корректирующие
                {
                    byte qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
                    short yk = short.Parse(YearKorr.Text);
                    rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.StajOsn.Count() == 0 && x.TypeInfoID == 2).ToList();
                }


                if (rsw61List.Count() > 0)
                {
                    bw.RunWorkerAsync();
                    startBtn.Enabled = false;
                    closeBtn.Enabled = false;



                    child = new RSW2014_6_Copy_Wait();
                    child.Owner = this;
                    child.ownerName = this.Name;
                    child.ThemeName = this.ThemeName;
                    child.titleLabel.Text = "Копирование информации о Стаже";
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


            foreach (var item in rsw61List)
            {
                if (bw.CancellationPending)
                {
                    rsw61List.Clear();
                    return;
                }
                k++;

                if (prevYearRadioButton.IsChecked) // если берем стаж за прошлый год
                {//item.TypeInfoID == 1 && 
                    if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.StaffID == item.StaffID && x.Year == (item.Year - 1) && x.Quarter == item.Quarter && x.TypeInfoID == 1 && x.StajOsn.Count() > 0))
                    {
                        FormsRSW2014_1_Razd_6_1 rsw61 = db.FormsRSW2014_1_Razd_6_1.First(x => x.StaffID == item.StaffID && x.Year == (item.Year - 1) && x.Quarter == item.Quarter && x.TypeInfoID == 1 && x.StajOsn.Count() > 0);
                        cloneStaj(rsw61, item.ID, true);
                    }
                    //if (item.TypeInfoID == 2 && db.FormsRSW2014_1_Razd_6_1.Any(x => x.StaffID == item.StaffID && x.Year == (item.Year - 1) && x.Quarter == item.Quarter && x.TypeInfoID == item.TypeInfoID && x.YearKorr == item.YearKorr && x.QuarterKorr == item.QuarterKorr && x.StajOsn.Count() > 0))
                    //{
                    //    FormsRSW2014_1_Razd_6_1 rsw61 = db.FormsRSW2014_1_Razd_6_1.First(x => x.StaffID == item.StaffID && x.Year == (item.Year - 1) && x.Quarter == item.Quarter && x.TypeInfoID == item.TypeInfoID && x.YearKorr == item.YearKorr && x.QuarterKorr == item.QuarterKorr && x.StajOsn.Count() > 0);
                    //    cloneStaj(rsw61, item.ID, true);
                    //}
                }
                else if (prevPeriodRadioButton.IsChecked)  // за прошлый период
                {
                    if (item.Quarter != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                    {
                        byte quarter = 20;
                        if (item.Quarter == 6)
                            quarter = 3;
                        else if (item.Quarter == 9)
                            quarter = 6;
                        else if (item.Quarter == 0)
                            quarter = 9;
                        //item.TypeInfoID == 1 &&            item.TypeInfoID 
                        if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.StaffID == item.StaffID && x.Year == item.Year && x.Quarter == quarter && x.TypeInfoID == 1 && x.StajOsn.Count() > 0))
                        {
                            FormsRSW2014_1_Razd_6_1 rsw61 = db.FormsRSW2014_1_Razd_6_1.First(x => x.StaffID == item.StaffID && x.Year == item.Year && x.Quarter == quarter && x.TypeInfoID == 1 && x.StajOsn.Count() > 0);
                            cloneStaj(rsw61, item.ID, false);
                        }
                        //if (item.TypeInfoID == 2 && db.FormsRSW2014_1_Razd_6_1.Any(x => x.StaffID == item.StaffID && x.Year == item.Year && x.Quarter == quarter && x.TypeInfoID == item.TypeInfoID && x.YearKorr == item.YearKorr && x.QuarterKorr == item.QuarterKorr && x.StajOsn.Count() > 0))
                        //{
                        //    FormsRSW2014_1_Razd_6_1 rsw61 = db.FormsRSW2014_1_Razd_6_1.First(x => x.StaffID == item.StaffID && x.Year == item.Year && x.Quarter == quarter && x.TypeInfoID == item.TypeInfoID && x.YearKorr == item.YearKorr && x.QuarterKorr == item.QuarterKorr && x.StajOsn.Count() > 0);
                        //    cloneStaj(rsw61, item.ID, false);
                        //}



                    }

                }
                else if (korrPeriodRadioButton.IsChecked && item.TypeInfoID == 2)  // за корректируемый период
                {
                    byte qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
                    short yk = short.Parse(YearKorr.Text);

                    if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.StaffID == item.StaffID && x.Year == yk && x.Quarter == qk && x.TypeInfoID == 1 && x.StajOsn.Count() > 0))
                    {
                        FormsRSW2014_1_Razd_6_1 rsw61 = db.FormsRSW2014_1_Razd_6_1.First(x => x.StaffID == item.StaffID && x.Year == yk && x.Quarter == qk && x.TypeInfoID == 1 && x.StajOsn.Count() > 0);
                        cloneStaj(rsw61, item.ID, false);
                    }
                }



                decimal temp = (decimal)k / (decimal)rsw61List.Count();
                int proc = (int)Math.Round((temp * 100), 0);
                bw.ReportProgress(proc, k.ToString());
            }
        }

        private void cloneStaj(FormsRSW2014_1_Razd_6_1 rsw61, long id, bool year)
        {
            foreach (var staj in rsw61.StajOsn)
            {
                try
                {
                    StajOsn sto = new StajOsn();
                    sto.Number = staj.Number;
                    sto.DateBegin = !korrPeriodRadioButton.IsChecked ? (year ? staj.DateBegin.Value.AddYears(1) : staj.DateBegin.Value.AddMonths(3)) : staj.DateBegin.Value;
                    sto.DateEnd = !korrPeriodRadioButton.IsChecked ? (year ? staj.DateEnd.Value.AddYears(1) : staj.DateEnd.Value.AddMonths(3)) : staj.DateEnd.Value;
                    sto.FormsRSW2014_1_Razd_6_1_ID = id;
                    db.StajOsn.Add(sto);
                    var stajlList = staj.StajLgot.ToList();
                    foreach (var stajl in stajlList)
                    {
                        StajLgot stl = stajl.Clone();
                        stl.StajOsn = null;
                        stl.StajOsnID = sto.ID;
                        db.StajLgot.Add(stl);
                    }
                    db.SaveChanges();
                }
                catch
                { }
            }

        }





    }
}
