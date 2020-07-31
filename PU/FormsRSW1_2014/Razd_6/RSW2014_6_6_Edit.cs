using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using PU.Models;
using PU.FormsRSW2014;
using Telerik.WinControls.UI;
using Telerik.WinControls.Primitives;
using System.Reflection;

namespace PU.FormsRSW2014
{
    public partial class RSW2014_6_6_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool setNull = true;
        private List<RaschetPeriodContainer> RaschPer { get; set; }
        public long staffId = 0;
        public short y = 0;
        public byte q = 20;

        public FormsRSW2014_1_Razd_6_6 formData { get; set; }

        public RSW2014_6_6_Edit()
        {
            InitializeComponent();
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

        private void RSW2014_6_6_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

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


            this.Year.Items.Clear();

            foreach (var item in RaschPer.Select(x => x.Year).Distinct().ToList())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }


            switch (action)
            {
                case "add":
                    if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                        Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                    else
                        Year.Items.Last().Selected = true;
                    break;
                case "edit":
                    Year.Items.Single(x => x.Text.ToString() == formData.AccountPeriodYear.ToString()).Selected = true;
                    break;
            }


            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            Year_SelectedIndexChanged();



            switch (action)
            {
                case "add":
                    formData = new FormsRSW2014_1_Razd_6_6();
                    break;
                case "edit":
                    OPS_.EditValue = formData.SumFeePFR_D.HasValue ? formData.SumFeePFR_D.Value : (decimal)0;
                    Strah_.EditValue = formData.SumFeePFR_StrahD.HasValue ? formData.SumFeePFR_StrahD.Value : (decimal)0;
                    Nakop_.EditValue = formData.SumFeePFR_NakopD.HasValue ? formData.SumFeePFR_NakopD.Value : (decimal)0;
                    break;
            }


        }

        private void Year_SelectedIndexChanged()
        {
            if (short.Parse(Year.SelectedItem.Text) >= 2014)
            {
                OPS_.Enabled = true;
                Strah_.Enabled = false;
                Nakop_.Enabled = false;
            }
            else
            {
                OPS_.Enabled = false;
                Strah_.Enabled = true;
                Nakop_.Enabled = true;
            }

            // выпад список "Отчетный период"
            this.Period.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in RaschPer.Where(x => x.Year == y).ToList())
                {
                    Period.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                DateTime dt = DateTime.Now.Date;
                switch (action)
                {
                    case "add":
                        RaschetPeriodContainer rp = new RaschetPeriodContainer();

                        if (RaschPer.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                            rp = RaschPer.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                        else
                            rp = RaschPer.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                        if (rp != null)
                            Period.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                        else
                            Period.Items.OrderByDescending(x => x.Value).First().Selected = true;
                        break;
                    case "edit":
                        Period.Items.Single(x => x.Value.ToString() == formData.AccountPeriodQuarter.ToString()).Selected = true;
                        break;
                }

            }
        }

        private void RSW2014_6_6_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (setNull)
                formData = null;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            formData.SumFeePFR_D = Math.Round(decimal.Parse(OPS_.EditValue.ToString()), 2, MidpointRounding.AwayFromZero);
            formData.SumFeePFR_StrahD = Math.Round(decimal.Parse(Strah_.EditValue.ToString()), 2, MidpointRounding.AwayFromZero);
            formData.SumFeePFR_NakopD = Math.Round(decimal.Parse(Nakop_.EditValue.ToString()), 2, MidpointRounding.AwayFromZero);
            formData.AccountPeriodYear = short.Parse(Year.SelectedItem.Text);
            formData.AccountPeriodQuarter = byte.Parse(Period.SelectedItem.Value.ToString());
            setNull = false;
            this.Close();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            decimal s = 0;
            decimal n = 0;

            short yk = 0;
            if (short.TryParse(Year.SelectedItem.Text, out yk))
            {
                byte qk = 0;
                if (byte.TryParse(Period.SelectedItem.Value.ToString(), out qk))
                {
                    if (yk >= 2014)
                    {
                        if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID >= 2 && x.StaffID == staffId))
                        {
                            FormsRSW2014_1_Razd_6_1 is_korr = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID >= 2 && x.StaffID == staffId);
                            if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == is_korr.YearKorr.Value && x.Quarter == is_korr.QuarterKorr.Value && x.TypeInfoID == 1 && x.StaffID == staffId))
                            {
                                FormsRSW2014_1_Razd_6_1 is_ishod = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == is_korr.YearKorr.Value && x.Quarter == is_korr.QuarterKorr.Value && x.TypeInfoID == 1 && x.StaffID == staffId);

                                if (is_korr.SumFeePFR.HasValue && is_ishod.SumFeePFR.HasValue)
                                {
                                    OPS_.EditValue = is_korr.SumFeePFR.Value - is_ishod.SumFeePFR.Value;
                                }
                            }
                        }
                    }
                    else if (yk >= 2010 && yk <= 2012)
                    {
 /*                       if (db.FormsSZV_6.Any(x => x.Year == y && x.Quarter == q && x.TypeInfoID == 1 && x.StaffID == staffId) && db.FormsSZV_6.Any(x => x.YearKorr == y && x.QuarterKorr == q && x.TypeInfoID == 2 && x.StaffID == staffId))
                        {
                            FormsSZV_6 is_ishod = db.FormsSZV_6.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.TypeInfoID == 1 && x.StaffID == staffId);
                            FormsSZV_6 is_korr = db.FormsSZV_6.FirstOrDefault(x => x.YearKorr == y && x.QuarterKorr == q && x.TypeInfoID == 2 && x.StaffID == staffId);

                            if (is_korr.SumFeePFR_Strah.HasValue && is_ishod.SumFeePFR_Strah.HasValue)
                            {
                                Strah_.EditValue = is_korr.SumFeePFR_Strah.Value - is_ishod.SumFeePFR_Strah.Value;
                            }
                            if (is_korr.SumPayPFR_Nakop.HasValue && is_ishod.SumPayPFR_Nakop.HasValue)
                            {
                                Nakop_.EditValue = is_korr.SumPayPFR_Nakop.Value - is_ishod.SumPayPFR_Nakop.Value;
                            }
                        }*/

                        foreach (var item in db.FormsSZV_6.Where(x => x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID >= 2 && x.StaffID == staffId))
                        {
                            s = s + (item.SumFeePFR_Strah_D.HasValue ? item.SumFeePFR_Strah_D.Value : 0);
                            n = n + (item.SumFeePFR_Nakop_D.HasValue ? item.SumFeePFR_Nakop_D.Value : 0);
                        }
                        Strah_.EditValue = s;
                        Nakop_.EditValue = n;

                    }
                    else if (yk == 2013)
                    {
 /*                       if (db.FormsSZV_6_4.Any(x => x.Year == y && x.Quarter == q && x.TypeInfoID == 1 && x.StaffID == staffId) && db.FormsSZV_6_4.Any(x => x.YearKorr == y && x.QuarterKorr == q && x.TypeInfoID == 2 && x.StaffID == staffId))
                        {
                            FormsSZV_6_4 is_ishod = db.FormsSZV_6_4.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.TypeInfoID == 1 && x.StaffID == staffId);
                            FormsSZV_6_4 is_korr = db.FormsSZV_6_4.FirstOrDefault(x => x.YearKorr == y && x.QuarterKorr == q && x.TypeInfoID == 2 && x.StaffID == staffId);

                            if (is_korr.SumFeePFR_Strah.HasValue && is_ishod.SumFeePFR_Strah.HasValue)
                            {
                                Strah_.EditValue = is_korr.SumFeePFR_Strah.Value - is_ishod.SumFeePFR_Strah.Value;
                            }
                            if (is_korr.SumPayPFR_Nakop.HasValue && is_ishod.SumPayPFR_Nakop.HasValue)
                            {
                                Nakop_.EditValue = is_korr.SumPayPFR_Nakop.Value - is_ishod.SumPayPFR_Nakop.Value;
                            }
                        }*/
                        foreach (var item in db.FormsSZV_6_4.Where(x => x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID >= 2 && x.StaffID == staffId))
                        {
                            s = s + (item.SumFeePFR_Strah_D.HasValue ? item.SumFeePFR_Strah_D.Value : 0);
                            n = n + (item.SumFeePFR_Nakop_D.HasValue ? item.SumFeePFR_Nakop_D.Value : 0);
                        }
                        Strah_.EditValue = s;
                        Nakop_.EditValue = n;

                    }
                }

            }



        }
    }
}
