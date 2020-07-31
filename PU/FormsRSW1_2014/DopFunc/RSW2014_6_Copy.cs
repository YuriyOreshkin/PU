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
using Telerik.WinControls.UI;
using PU.Classes;

namespace PU.FormsRSW2014
{
    public partial class RSW2014_6_Copy : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        private List<ErrList> errMessBox = new List<ErrList>();
        RSW2014_6_Copy_Wait child = new RSW2014_6_Copy_Wait();
        public List<long> staffList = new List<long>();


        public RSW2014_6_Copy()
        {
            InitializeComponent();
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(calculation);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);

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

        private void radButton9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.child.Close();
            this.Close();
        }

        private void RSW2014_Copy_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            foreach (var item in db.TypeInfo)
            {
                TypeInfo_old.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
                TypeInfo_new.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }

            TypeInfo_old.SelectedIndex = 0;
            TypeInfo_new.SelectedIndex = 0;
            DateUnderwrite.DateTime = DateTime.Now;

            // выпад список "календарный год"
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);

            this.Year_old.Items.Clear();
            this.Year_new.Items.Clear();
            this.YearKorr_old.Items.Clear();
            this.YearKorr_new.Items.Clear();

            foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
            {
                Year_old.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                Year_new.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                YearKorr_old.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                YearKorr_new.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }


            if (Year_old.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                Year_old.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                Year_old.Items.OrderByDescending(x => x.Value).First().Selected = true;

            if (Year_new.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                Year_new.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                Year_new.Items.OrderByDescending(x => x.Value).First().Selected = true;

            if (YearKorr_old.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                YearKorr_old.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                YearKorr_old.Items.OrderByDescending(x => x.Value).First().Selected = true;

            if (YearKorr_new.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                YearKorr_new.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                YearKorr_new.Items.OrderByDescending(x => x.Value).First().Selected = true;


            // выпад список "Отчетный период"

            Year_old_SelectedIndexChanged();
            Year_new_SelectedIndexChanged();
            YearKorr_old_SelectedIndexChanged();
            YearKorr_new_SelectedIndexChanged();

            this.Year_old.SelectedIndexChanged += (s, с) => Year_old_SelectedIndexChanged();
            this.Year_new.SelectedIndexChanged += (s, с) => Year_new_SelectedIndexChanged();
            this.YearKorr_old.SelectedIndexChanged += (s, с) => YearKorr_old_SelectedIndexChanged();
            this.YearKorr_new.SelectedIndexChanged += (s, с) => YearKorr_new_SelectedIndexChanged();
            this.TypeInfo_old.SelectedIndexChanged += (s, с) => TypeInfo_old_SelectedIndexChanged();
            this.TypeInfo_new.SelectedIndexChanged += (s, с) => TypeInfo_new_SelectedIndexChanged();


        }


        private void Year_old_SelectedIndexChanged()
        {
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);

            // выпад список "Отчетный период"

            this.Quarter_old.Items.Clear();

            short y;
            if (short.TryParse(Year_old.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter_old.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                DateTime dt = DateTime.Now.Date;

                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                else
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                if (rp != null)
                    Quarter_old.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                else
                    Quarter_old.Items.OrderByDescending(x => x.Value).First().Selected = true;


            }
        }
        private void Year_new_SelectedIndexChanged()
        {
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);

            // выпад список "Отчетный период"

            this.Quarter_new.Items.Clear();

            short y;
            if (short.TryParse(Year_new.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter_new.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                DateTime dt = DateTime.Now.Date;

                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                else
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                if (rp != null)
                    Quarter_new.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                else
                    Quarter_new.Items.OrderByDescending(x => x.Value).First().Selected = true;


            }
        }
        private void YearKorr_old_SelectedIndexChanged()
        {
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);

            // выпад список "Отчетный период"

            this.QuarterKorr_old.Items.Clear();

            short y;
            if (short.TryParse(YearKorr_old.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    QuarterKorr_old.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                DateTime dt = DateTime.Now.Date;

                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                else
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                if (rp != null)
                    QuarterKorr_old.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                else
                    QuarterKorr_old.Items.OrderByDescending(x => x.Value).First().Selected = true;

            }
        }
        private void YearKorr_new_SelectedIndexChanged()
        {
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);

            // выпад список "Отчетный период"

            this.QuarterKorr_new.Items.Clear();

            short y;
            if (short.TryParse(YearKorr_new.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    QuarterKorr_new.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                DateTime dt = DateTime.Now.Date;

                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                else
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                if (rp != null)
                    QuarterKorr_new.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                else
                    QuarterKorr_new.Items.OrderByDescending(x => x.Value).First().Selected = true;

            }
        }


        private void radButton8_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                RSW2014_List main = this.Owner as RSW2014_List;
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
                }


                backgroundWorker1.RunWorkerAsync();

                child = new RSW2014_6_Copy_Wait();
                child.Owner = this;
                child.ownerName = this.Name;
                child.ThemeName = this.ThemeName;
                child.titleLabel.Text = "Копирование Индивидуальных сведений";
                child.secondPartLabel.Text = staffList.Count().ToString();
                child.Show();

            }
            else
            {
                RadMessageBox.Show(this, errMessBox[0].name, "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }


        }

        private void calculation(object sender, DoWorkEventArgs e)
        {
            FormsRSW2014_1_Razd_6_1 old_rsw = new FormsRSW2014_1_Razd_6_1();
            int k = 0;
            foreach (var item in staffList)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    staffList.Clear();
                    return;
                }
                k++;

                long typeInfoId = long.Parse(TypeInfo_old.SelectedItem.Value.ToString());
                byte q = byte.Parse(Quarter_old.SelectedItem.Value.ToString());
                short y = short.Parse(Year_old.Text);
                byte qk = byte.Parse(QuarterKorr_old.SelectedItem.Value.ToString());
                short yk = short.Parse(YearKorr_old.Text);

                long typeInfoId_n = long.Parse(TypeInfo_new.SelectedItem.Value.ToString());
                byte q_n = byte.Parse(Quarter_new.SelectedItem.Value.ToString());
                short y_n = short.Parse(Year_new.Text);
                byte qk_n = byte.Parse(QuarterKorr_new.SelectedItem.Value.ToString());
                short yk_n = short.Parse(YearKorr_new.Text);


                if (TypeInfo_old.SelectedIndex == 0)
                    old_rsw = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.StaffID == item && x.TypeInfoID == typeInfoId && x.Year == y && x.Quarter == q);
                else
                {
                    old_rsw = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.StaffID == item && x.TypeInfoID == typeInfoId && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk);
                }

                if (old_rsw != null)
                {
                    List<long> id_del = new List<long>();
                    if (TypeInfo_new.SelectedIndex == 0)
                    {
                        if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.StaffID == item && x.TypeInfoID == typeInfoId_n && x.Year == y_n && x.Quarter == q_n))
                        {
                            id_del = db.FormsRSW2014_1_Razd_6_1.Where(x => x.StaffID == item && x.TypeInfoID == typeInfoId_n && x.Year == y_n && x.Quarter == q_n).Select(x => x.ID).ToList();
                        }
                    }
                    else
                    {
                        if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.StaffID == item && x.TypeInfoID == typeInfoId_n && x.Year == y_n && x.Quarter == q_n && x.YearKorr == yk_n && x.QuarterKorr == qk_n))
                        {
                            id_del = db.FormsRSW2014_1_Razd_6_1.Where(x => x.StaffID == item && x.TypeInfoID == typeInfoId_n && x.Year == y_n && x.Quarter == q_n && x.YearKorr == yk_n && x.QuarterKorr == qk_n).Select(x => x.ID).ToList();
                        }
                    }
                    if (id_del.Count() > 0)
                    {
                        string list = String.Join(",", id_del.ToArray());
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_1 WHERE ([ID] IN ({0}))", list));
                    }

                    old_rsw.FormsRSW2014_1_Razd_6_4 = null;
                    old_rsw.FormsRSW2014_1_Razd_6_6 = null;
                    old_rsw.FormsRSW2014_1_Razd_6_7 = null;

                    FormsRSW2014_1_Razd_6_1 new_rsw = old_rsw.Clone();
                    db.Detach(old_rsw);
                    IQueryable<FormsRSW2014_1_Razd_6_4> old_rsw_6_4 = db.FormsRSW2014_1_Razd_6_4.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == old_rsw.ID);
                    IQueryable<FormsRSW2014_1_Razd_6_7> old_rsw_6_7 = db.FormsRSW2014_1_Razd_6_7.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == old_rsw.ID);


                    new_rsw.TypeInfoID = long.Parse(TypeInfo_new.SelectedItem.Value.ToString());
                    new_rsw.Year = short.Parse(Year_new.Text);
                    new_rsw.Quarter = byte.Parse(Quarter_new.SelectedItem.Value.ToString());
                    if (TypeInfo_new.SelectedIndex != 0)
                    {
                        if (RegNumKorr.Value.ToString() != "___-___-______")
                            new_rsw.RegNumKorr = RegNumKorr.Value.ToString();
                        else
                            new_rsw.RegNumKorr = null;

                        new_rsw.YearKorr = short.Parse(YearKorr_new.Text);
                        new_rsw.QuarterKorr = byte.Parse(QuarterKorr_new.SelectedItem.Value.ToString());
                    }
                    else
                    {
                        new_rsw.RegNumKorr = null;
                        new_rsw.YearKorr = null;
                        new_rsw.QuarterKorr = null;
                    }
                    new_rsw.DateFilling = DateUnderwrite.DateTime.Date;
                    db.SaveChanges();

                    foreach (var item_ in old_rsw_6_4)
                    {
                        FormsRSW2014_1_Razd_6_4 new_rsw_6_4 = item_.Clone();
                        /*                    new_rsw_6_4.PlatCategory = null;
                                            new_rsw_6_4.PlatCategoryReference = null;
                                            new_rsw_6_4.PlatCategoryID = item_.PlatCategoryID;                    
                    */
                        new_rsw_6_4.FormsRSW2014_1_Razd_6_1 = null;
                        new_rsw_6_4.FormsRSW2014_1_Razd_6_1Reference = null;
                        new_rsw_6_4.FormsRSW2014_1_Razd_6_1_ID = new_rsw.ID;

                        db.AddToFormsRSW2014_1_Razd_6_4(new_rsw_6_4);
                    }

                    foreach (var item_ in old_rsw_6_7)
                    {
                        FormsRSW2014_1_Razd_6_7 new_rsw_6_7 = item_.Clone();
                        /*                    new_rsw_6_7.SpecOcenkaUslTruda = null;
                                            new_rsw_6_7.SpecOcenkaUslTrudaReference = null;
                                            new_rsw_6_7.SpecOcenkaUslTrudaID = item_.SpecOcenkaUslTrudaID;                    
                    */
                        new_rsw_6_7.FormsRSW2014_1_Razd_6_1 = null;
                        new_rsw_6_7.FormsRSW2014_1_Razd_6_1Reference = null;
                        new_rsw_6_7.FormsRSW2014_1_Razd_6_1_ID = new_rsw.ID;

                        db.AddToFormsRSW2014_1_Razd_6_7(new_rsw_6_7);
                    }


                    if (CopyRazd_6_6_ChkBox.Checked)  // если выбрано копировать раздел 6.6
                    {
                        IQueryable<FormsRSW2014_1_Razd_6_6> old_rsw_6_6 = db.FormsRSW2014_1_Razd_6_6.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == old_rsw.ID);

                        foreach (var item_ in old_rsw_6_6)
                        {
                            FormsRSW2014_1_Razd_6_6 new_rsw_6_6 = item_.Clone();
                            new_rsw_6_6.FormsRSW2014_1_Razd_6_1 = null;
                            new_rsw_6_6.FormsRSW2014_1_Razd_6_1Reference = null;
                            new_rsw_6_6.FormsRSW2014_1_Razd_6_1_ID = new_rsw.ID;

                            db.AddToFormsRSW2014_1_Razd_6_6(new_rsw_6_6);
                        }
                    }
                    db.SaveChanges();
                }



                decimal temp = (decimal)k / (decimal)staffList.Count();
                int proc = (int)Math.Round((temp * 100), 0);
                backgroundWorker1.ReportProgress(proc, k.ToString());

            }


            /*            new_rsw.Insurer = null;
                        new_rsw.InsurerReference = null;
                        new_rsw.InsurerID = RSWdata.InsurerID;
                        */


        }

        private bool validation()
        {
            bool result = true;
            if (Year_old.Text == "")
            {
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Year_old" });
                result = false;
            }
            if (Quarter_old.Text == "")
            {
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Quarter_old" });
                result = false;
            }
            if (Year_new.Text == "")
            {
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Year_new" });
                result = false;
            }
            if (Quarter_new.Text == "")
            {
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Quarter_new" });
                result = false;
            }

            if (result && TypeInfo_old.SelectedIndex > 0)
            {
                short yo = short.Parse(Year_old.Text);
                short yko = short.Parse(YearKorr_old.Text);
                if ((yo < yko) || ((yo == yko) && (QuarterKorr_old.SelectedIndex >= Quarter_old.SelectedIndex)))
                {
                    errMessBox.Add(new ErrList { name = "Копируемая форма Индивидуальных сведений.\n\rКорректируемый отчетный период не может превышать или быть равным Отчетному периоду.", control = "Quarter_old" });
                    result = false;
                }
            }

            if (result && TypeInfo_new.SelectedIndex > 0)
            {
                short yn = short.Parse(Year_new.Text);
                short ykn = short.Parse(YearKorr_new.Text);
                if ((yn < ykn) || ((yn == ykn) && (QuarterKorr_new.SelectedIndex >= Quarter_new.SelectedIndex)))
                {
                    errMessBox.Add(new ErrList { name = "Новая форма Индивидуальных сведений.\n\rКорректируемый отчетный период не может превышать или быть равным Отчетному периоду.", control = "Quarter_new" });
                    result = false;
                }
            }

            if (result)
            {
                short yo = short.Parse(Year_old.Text);
                short yko = short.Parse(YearKorr_old.Text);
                short yn = short.Parse(Year_new.Text);
                short ykn = short.Parse(YearKorr_new.Text);

                if (TypeInfo_old.SelectedIndex > 0 && TypeInfo_old.SelectedIndex == TypeInfo_new.SelectedIndex)
                {
                    if ((yo == yn) && (yko == ykn) && (QuarterKorr_new.SelectedIndex == QuarterKorr_old.SelectedIndex) && (Quarter_new.SelectedIndex == Quarter_old.SelectedIndex))
                    {
                        errMessBox.Add(new ErrList { name = "Нельзя копировать форму саму в себя.", control = "Quarter_new" });
                        result = false;
                    }
                }
                else if (TypeInfo_old.SelectedIndex == 0 && TypeInfo_old.SelectedIndex == TypeInfo_new.SelectedIndex)
                {
                    if ((yo == yn) && (Quarter_new.SelectedIndex == Quarter_old.SelectedIndex))
                    {
                        errMessBox.Add(new ErrList { name = "Нельзя копировать форму саму в себя.", control = "Quarter_new" });
                        result = false;
                    }
                }
            }

            return result;
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            child.radProgressBar1.Value1 = e.ProgressPercentage;
            child.firstPartLabel.Text = e.UserState.ToString();
        }



        private void TypeInfo_old_SelectedIndexChanged()
        {
            if (TypeInfo_old.SelectedIndex == 0)
            {
                korrOldGroupBox.Enabled = false;
            }
            else
            {
                korrOldGroupBox.Enabled = true;
            }
        }

        private void TypeInfo_new_SelectedIndexChanged()
        {
            if (TypeInfo_new.SelectedIndex == 0)
            {
                korrNewGroupBox.Enabled = false;
                RegNumKorr.Enabled = false;
            }
            else
            {
                korrNewGroupBox.Enabled = true;
                RegNumKorr.Enabled = true;
            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            YearKorr_old.Enabled = true;
        }

    }
}
