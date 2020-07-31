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
using Telerik.WinControls.UI;
using PU.Classes;

namespace PU.FormsRSW2014
{
    public partial class RSW2014_Copy : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public FormsRSW2014_1_1 RSWdata { get; set; }
        private List<ErrList> errMessBox = new List<ErrList>();
        private short yearOld = 0;
        private byte quarterOld = 0;

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

        public RSW2014_Copy()
        {
            InitializeComponent();
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(calculation);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);

        }

        private void radButton9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            radProgressBar1.Value1 = e.ProgressPercentage;
        }

        private void RSW2014_Copy_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            // выпад список "календарный год"
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014 && x.Year < 2017).OrderBy(x => x.Year);

            this.Year.Items.Clear();

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

            yearOld = RSWdata.Year;
            quarterOld = RSWdata.Quarter;

            yearLabel.Text = RSWdata.Year.ToString();
            corrNumLabel.Text = RSWdata.CorrectionNum.ToString();
            if (RSWdata.CorrectionType.HasValue)
            {
                corrTypeLabel.Text = CorrectionType.Items.FirstOrDefault(x => x.Tag.ToString() == RSWdata.CorrectionType.Value.ToString()).Text;
            }
            else
                corrTypeLabel.ResetText();

            if (Options.RaschetPeriodInternal.Any(x => x.Year == RSWdata.Year && x.Kvartal == RSWdata.Quarter))
            {
                var rp_v = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == RSWdata.Year && x.Kvartal == RSWdata.Quarter);
                quarterLabel.Text = rp_v.Kvartal + " - " + rp_v.Name;
            }

        }

        private void CorrectionNum_ValueChanged(object sender, EventArgs e)
        {
            if (CorrectionNum.Value == 0)
            {
                CorrectionType.Enabled = false;
                CorrectionType.Text = "";
            }
            else
            {
                CorrectionType.Enabled = true;
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


        private void radButton8_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                radProgressBar1.Visible = true;
                backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, errMessBox[0].name, "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation,MessageBoxDefaultButton.Button1);
            }


        }

        private void calculation(object sender, DoWorkEventArgs e)
        {
            FormsRSW2014_1_1 old_rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == RSWdata.ID);
            IQueryable<FormsRSW2014_1_Razd_2_1> old_rsw_2_1 = db.FormsRSW2014_1_Razd_2_1.Where(x => x.Year == old_rsw.Year && x.Quarter == old_rsw.Quarter && x.InsurerID == old_rsw.InsurerID && x.CorrectionNum == old_rsw.CorrectionNum);
            IQueryable<FormsRSW2014_1_Razd_2_4> old_rsw_2_4 = db.FormsRSW2014_1_Razd_2_4.Where(x => x.Year == old_rsw.Year && x.Quarter == old_rsw.Quarter && x.InsurerID == old_rsw.InsurerID && x.CorrectionNum == old_rsw.CorrectionNum);
            IQueryable<FormsRSW2014_1_Razd_3_4> old_rsw_3_4 = db.FormsRSW2014_1_Razd_3_4.Where(x => x.Year == old_rsw.Year && x.Quarter == old_rsw.Quarter && x.InsurerID == old_rsw.InsurerID && x.CorrectionNum == old_rsw.CorrectionNum);
            IQueryable<FormsRSW2014_1_Razd_4> old_rsw_4 = db.FormsRSW2014_1_Razd_4.Where(x => x.Year == old_rsw.Year && x.Quarter == old_rsw.Quarter && x.InsurerID == old_rsw.InsurerID && x.CorrectionNum == old_rsw.CorrectionNum);
            IQueryable<FormsRSW2014_1_Razd_5> old_rsw_5 = db.FormsRSW2014_1_Razd_5.Where(x => x.Year == old_rsw.Year && x.Quarter == old_rsw.Quarter && x.InsurerID == old_rsw.InsurerID && x.CorrectionNum == old_rsw.CorrectionNum);


            FormsRSW2014_1_1 new_rsw = old_rsw.Clone();
            db.Entry(old_rsw).State = EntityState.Detached;

            //           new_rsw.EntityKey = null;

            new_rsw.Insurer = null;
            //new_rsw.InsurerReference = null;
            new_rsw.InsurerID = RSWdata.InsurerID;

            // make any other changes you want on your clone here
            byte corrNew = byte.Parse(CorrectionNum.Value.ToString());
            new_rsw.CorrectionNum = corrNew;

            byte quarterNew = byte.Parse(Quarter.SelectedItem.Value.ToString());
            new_rsw.Quarter = quarterNew;

            short yearNew = Int16.Parse(Year.Text);
            new_rsw.Year = yearNew;

            new_rsw.DateUnderwrite = DateTime.Now;

            if (CorrectionNum.Value == 0)
            {
                new_rsw.CorrectionType = null;
            }
            else if (CorrectionType.SelectedItem != null)
            {
                new_rsw.CorrectionType = byte.Parse(CorrectionType.SelectedItem.Tag.ToString());
            }
            else
            {
                new_rsw.CorrectionType = null;
            }

            db.FormsRSW2014_1_1.Add(new_rsw);
            backgroundWorker1.ReportProgress(1);

            foreach (var item in old_rsw_2_1)
            {
                FormsRSW2014_1_Razd_2_1 new_rsw_2_1 = item.Clone();
                //                   new_rsw_2_1.EntityKey = null;
                new_rsw_2_1.CorrectionNum = corrNew;
                new_rsw_2_1.Quarter = quarterNew;
                new_rsw_2_1.Year = yearNew;
                short yearTypeOld = ((yearOld == (short)2014) || (yearOld == (short)2015 && quarterOld == 3)) ? (short)2014 : (short)2015;
                short yearTypeNew = ((yearNew == (short)2014) || (yearNew == (short)2015 && quarterNew == 3)) ? (short)2014 : (short)2015;

                if (yearTypeOld == 2014 && yearTypeNew == 2015)
                {
                    new_rsw_2_1.s_213_0 = item.s_214_0.HasValue ? item.s_214_0.Value : 0;
                    new_rsw_2_1.s_213_1 = item.s_214_1.HasValue ? item.s_214_1.Value : 0;
                    new_rsw_2_1.s_213_2 = item.s_214_2.HasValue ? item.s_214_2.Value : 0;
                    new_rsw_2_1.s_213_3 = item.s_214_3.HasValue ? item.s_214_3.Value : 0;

                    new_rsw_2_1.s_214_0 = item.s_215_0.HasValue ? item.s_215_0.Value : 0;
                    new_rsw_2_1.s_214_1 = item.s_215_1.HasValue ? item.s_215_1.Value : 0;
                    new_rsw_2_1.s_214_2 = item.s_215_2.HasValue ? item.s_215_2.Value : 0;
                    new_rsw_2_1.s_214_3 = item.s_215_3.HasValue ? item.s_215_3.Value : 0;
                }
                else if (yearTypeOld == 2015 && yearTypeNew == 2014)
                {
                    new_rsw_2_1.s_215_0 = item.s_214_0.HasValue ? item.s_214_0.Value : 0;
                    new_rsw_2_1.s_215_1 = item.s_214_1.HasValue ? item.s_214_1.Value : 0;
                    new_rsw_2_1.s_215_2 = item.s_214_2.HasValue ? item.s_214_2.Value : 0;
                    new_rsw_2_1.s_215_3 = item.s_214_3.HasValue ? item.s_214_3.Value : 0;

                    new_rsw_2_1.s_214_0 = item.s_213_0.HasValue ? item.s_213_0.Value : 0;
                    new_rsw_2_1.s_214_1 = item.s_213_1.HasValue ? item.s_213_1.Value : 0;
                    new_rsw_2_1.s_214_2 = item.s_213_2.HasValue ? item.s_213_2.Value : 0;
                    new_rsw_2_1.s_214_3 = item.s_213_3.HasValue ? item.s_213_3.Value : 0;

                    new_rsw_2_1.s_213_0 = 0;
                    new_rsw_2_1.s_213_1 = 0;
                    new_rsw_2_1.s_213_2 = 0;
                    new_rsw_2_1.s_213_3 = 0;
                }

                db.Entry(item).State = EntityState.Detached;

                db.FormsRSW2014_1_Razd_2_1.Add(new_rsw_2_1);
            }
            backgroundWorker1.ReportProgress(2);
            foreach (var item in old_rsw_2_4)
            {
                FormsRSW2014_1_Razd_2_4 new_rsw_2_4 = item.Clone();
                db.Entry(item).State = EntityState.Detached;
                //                    new_rsw_2_4.EntityKey = null;
                new_rsw_2_4.CorrectionNum = corrNew;
                new_rsw_2_4.Quarter = quarterNew;
                new_rsw_2_4.Year = yearNew;
                db.FormsRSW2014_1_Razd_2_4.Add(new_rsw_2_4);
            }
            backgroundWorker1.ReportProgress(3);

            foreach (var item in old_rsw_3_4)
            {
                FormsRSW2014_1_Razd_3_4 new_rsw_3_4 = item.Clone();
                db.Entry(item).State = EntityState.Detached;
                //                    new_rsw_3_4.EntityKey = null;
                new_rsw_3_4.CorrectionNum = corrNew;
                new_rsw_3_4.Quarter = quarterNew;
                new_rsw_3_4.Year = yearNew;
                db.FormsRSW2014_1_Razd_3_4.Add(new_rsw_3_4);
            }
            backgroundWorker1.ReportProgress(4);
            foreach (var item in old_rsw_4)
            {
                FormsRSW2014_1_Razd_4 new_rsw_4 = item.Clone();
                db.Entry(item).State = EntityState.Detached;
                //                    new_rsw_4.EntityKey = null;
                new_rsw_4.CorrectionNum = corrNew;
                new_rsw_4.Quarter = quarterNew;
                new_rsw_4.Year = yearNew;
                db.FormsRSW2014_1_Razd_4.Add(new_rsw_4);
            }
            backgroundWorker1.ReportProgress(5);
            foreach (var item in old_rsw_5)
            {
                FormsRSW2014_1_Razd_5 new_rsw_5 = item.Clone();
                db.Entry(item).State = EntityState.Detached;
                //                    new_rsw_5.EntityKey = null;
                new_rsw_5.CorrectionNum = corrNew;
                new_rsw_5.Quarter = quarterNew;
                new_rsw_5.Year = yearNew;
                db.FormsRSW2014_1_Razd_5.Add(new_rsw_5);
            }


            db.SaveChanges();
            backgroundWorker1.ReportProgress(6);


        }

        private bool validation()
        {
            errMessBox.Clear();
            bool result = true;
            if (Year.Text == "")
            {
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Year" });
                result = false;
            }
            if (Quarter.Text == "")
            {
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Quarter" });
                result = false;
            }

            if (CorrectionNum.Value != 0)
                if (CorrectionType.Text == "")
                {
                    errMessBox.Add(new ErrList { name = "Не выбран тип корректировки", control = "CorrectionType" });
                    result = false;
                }


            if (result)
            {
                long InsId_New = Options.InsID;
                byte CorrectionNum_New = byte.Parse(CorrectionNum.Value.ToString());
                //byte? CorrectionType_New = null;
                byte Quarter_New = byte.Parse(Quarter.SelectedItem.Value.ToString());
                Int16 Year_New = Int16.Parse(Year.Text);

                //if (CorrectionNum.Value == 0)
                //{
                    if (RSWdata.Year == Year_New && RSWdata.Quarter == Quarter_New && RSWdata.CorrectionNum == CorrectionNum_New)
                    {
                        errMessBox.Add(new ErrList { name = "Период и номер корректировки новой формы не может совпадать с текущей формой", control = "Quarter" });
                        return false;
                    }
                //}
                //else if (CorrectionType.SelectedItem != null)
                //{
                //    if (RSWdata.Year == Year_New && RSWdata.Quarter == Quarter_New && RSWdata.CorrectionNum == CorrectionNum_New)
                //    {
                //        errMessBox.Add(new ErrList { name = "Период и номер корректировки новой формы не может совпадать с текущей формой", control = "Quarter" });
                //        return false;
                //    }
                //}


                //if (result)
                //{
                    //if (CorrectionNum.Value == 0)
                    //{
                        if (db.FormsRSW2014_1_1.Any(x => x.InsurerID == Options.InsID && x.Year == Year_New && x.Quarter == Quarter_New && x.CorrectionNum == CorrectionNum_New))
                        {
                            errMessBox.Add(new ErrList { name = "Невозможно выполнить копирование формы, т.к. создается дублирование форм", control = "Quarter" });
                            return false;
                        }
                    //}
                    //else 
                    //{
                    //    if (db.FormsRSW2014_1_1.Any(x => x.InsurerID == Options.InsID && x.Year == Year_New && x.Quarter == Quarter_New && x.CorrectionNum == CorrectionNum_New))
                    //    {
                    //        errMessBox.Add(new ErrList { name = "Невозможно выполнить копирование формы, т.к. создается дублирование форм", control = "Quarter" });
                    //        return false;
                    //    }
                    //}

                //}

            }



            return result;
        }

    }
}
