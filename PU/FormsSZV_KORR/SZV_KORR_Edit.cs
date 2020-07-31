﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using PU.Models;
using Telerik.WinControls.UI;
using PU.FormsRSW2014;
using System.Reflection;
using PU.Staj;

namespace PU.FormsSZV_KORR
{
    public partial class SZV_KORR_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public Staff staff { get; set; }
        public FormsODV_1_2017 ODV1 { get; set; }
        public int defaultPage = 0;
        List<RaschetPeriodContainer> avail_periods_all = new List<RaschetPeriodContainer>();
        List<RaschetPeriodContainer> avail_periods_all_2017 = new List<RaschetPeriodContainer>();
        List<FormsSZV_KORR_4_2017> FormsSZV_KORR_4_2017_List = new List<FormsSZV_KORR_4_2017>();
        List<FormsSZV_KORR_5_2017> FormsSZV_KORR_5_2017_List = new List<FormsSZV_KORR_5_2017>();
        public FormsSZV_KORR_2017 SZV_KORR { get; set; }
        public List<StajOsn> StajOsn_List = new List<StajOsn>();
        //    public List<StajLgot> StajLgot_List = new List<StajLgot>();
        private List<string> errMessBox = new List<string>();

        public byte period { get; set; }
        public byte periodKorr { get; set; }
        public byte CorrNum { get; set; }
        bool allowClose = false;

        public List<PU.FormsODV1.ODV1_List.MonthesDict> Monthes = new List<PU.FormsODV1.ODV1_List.MonthesDict>();

        public string parentName { get; set; }

        public SZV_KORR_Edit()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void DateFillingMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateFillingMaskedEditBox.Text != DateFillingMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateFillingMaskedEditBox.Text, out date))
                {
                    DateFilling.Value = date;
                }
                else
                {
                    DateFilling.Value = DateFilling.NullDate;
                }
            }
            else
            {
                DateFilling.Value = DateFilling.NullDate;
            }
        }

        private void DateFilling_ValueChanged(object sender, EventArgs e)
        {
            if (DateFilling.Value != DateFilling.NullDate)
                DateFillingMaskedEditBox.Text = DateFilling.Value.ToShortDateString();
            else
                DateFillingMaskedEditBox.Text = DateFillingMaskedEditBox.NullText;
        }

        private void ContractDateMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (ContractDateMaskedEditBox.Text != ContractDateMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(ContractDateMaskedEditBox.Text, out date))
                {
                    ContractDate.Value = date;
                }
                else
                {
                    ContractDate.Value = ContractDate.NullDate;
                }
            }
            else
            {
                ContractDate.Value = ContractDate.NullDate;
            }
        }

        private void ContractDate_ValueChanged(object sender, EventArgs e)
        {
            if (ContractDate.Value != ContractDate.NullDate)
                ContractDateMaskedEditBox.Text = ContractDate.Value.ToShortDateString();
            else
                ContractDateMaskedEditBox.Text = ContractDateMaskedEditBox.NullText;
        }

        private void selectStaffBtn_Click(object sender, EventArgs e)
        {
            Staff staffLoad = null;

            var ssn = SNILS.Value.ToString().Split(' ');

            if (!ssn[0].Contains("_"))
            {
                string snils = ssn[0].Replace("-", "");
                if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils))
                {
                    staffLoad = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils);
                }
            }

            StaffFrm child = new StaffFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.InsID = Options.InsID;
            child.action = "selection";
            child.StaffID = staffLoad == null ? 0 : staffLoad.ID; ;
            child.ShowDialog();
            long id = child.StaffID;
            if (db.Staff.Any(x => x.ID == id))
            {
                staff = db.Staff.FirstOrDefault(x => x.ID == id);
                updateStaffInfo();
            }
        }

        private void updateStaffInfo()
        {
            if (staff != null)
            {
                try
                {
                    LastName.Text = staff.LastName;
                    FirstName.Text = staff.FirstName;
                    MiddleName.Text = staff.MiddleName;
                    SNILS.Text = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber.Value) : "";
                    Tabel.Text = staff.TabelNumber != null ? staff.TabelNumber.Value.ToString() : "";
                }
                catch { }
            }
        }

        private void SZV_KORR_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            this.Cursor = Cursors.WaitCursor;
            this.radPageView1.SelectedPage = this.radPageView1.Pages[0];

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    this.Size = param.size;
                    this.Location = param.location;
                    this.WindowState = param.windowState == FormWindowState.Minimized ? FormWindowState.Maximized : param.windowState;
                }
                catch
                { }

            }

            Insurer ins = db.Insurer.First(x => x.ID == Options.InsID);

            RegNum.Text = ins.RegNum;
            INN.Text = ins.INN;
            KPP.Text = ins.KPP;
            NameShort.Text = ins.NameShort;
            int year = DateTime.Now.Year;

            this.PlatCategory.Items.Clear();

            this.PlatCategory.Items.Add(new RadListDataItem("", 0));

            foreach (var item in db.PlatCategory.ToList())
            {
                this.PlatCategory.Items.Add(new RadListDataItem(item.Code, item.ID.ToString()));
            }


            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year <= year).OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal2017.OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all_2017.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal2010_2013.OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal1996_2009.OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }

            avail_periods_all = avail_periods_all.OrderByDescending(x => x.Year).ToList();

            this.Year.Items.Clear();

            foreach (var item in avail_periods_all_2017.Where(x => x.Year >= 2017).Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }

            this.KorrYear.Items.Clear();

            foreach (var item in avail_periods_all.Select(x => x.Year).ToList().Distinct())
            {
                KorrYear.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }


            short y;

            try
            {

                switch (action)
                {
                    case "add":
                        SZV_KORR = new FormsSZV_KORR_2017();
                        TypeInfo.Items.Single(x => x.Tag.ToString() == "0").Selected = true;
                        DateFilling.Value = DateTime.Now.Date;
                        SZV_KORR.FormsODV_1_2017_ID = ODV1.ID;

                        // выпад список "календарный год"

                        if (Year.Items.Any(x => x.Text.ToString() == ODV1.Year.ToString()))
                        {
                            Year.Items.Single(x => x.Text.ToString() == ODV1.Year.ToString()).Selected = true;
                        }
                        else
                        {
                            if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                                Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                            else
                                Year.Items.OrderByDescending(x => x.Value).First().Selected = true;
                        }

                        // выпад список "Отчетный период"

                        this.Quarter.Items.Clear();

                        if (short.TryParse(Year.SelectedItem.Text, out y))
                        {
                            foreach (var item in avail_periods_all_2017.Where(x => x.Year == y).ToList()) //  && x.Kvartal != 0
                            {
                                Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                            }

                            if (Quarter.Items.Any(x => x.Value.ToString() == ODV1.Code.ToString()))
                            {
                                Quarter.Items.FirstOrDefault(x => x.Value.ToString() == ODV1.Code.ToString()).Selected = true;
                            }
                            else
                            {
                                DateTime dt = DateTime.Now.AddDays(-45);

                                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                                if (avail_periods_all_2017.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y))
                                    rp = avail_periods_all_2017.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y);
                                else
                                    rp = avail_periods_all_2017.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y);

                                if (rp != null)
                                    Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                                else
                                    Quarter.Items.OrderByDescending(x => x.Value).First().Selected = true;
                            }
                        }


                        // выпад список "корр календарный год"


                        if (KorrYear.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                            KorrYear.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                        else
                            KorrYear.Items.OrderByDescending(x => x.Value).First().Selected = true;

                        // выпад список "корр Отчетный период"

                        this.KorrQuarter.Items.Clear();

                        if (short.TryParse(KorrYear.SelectedItem.Text, out y))
                        {
                            foreach (var item in avail_periods_all.Where(x => x.Year == y).ToList()) //  && x.Kvartal != 0
                            {
                                KorrQuarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                            }
                            DateTime dt = DateTime.Now.AddDays(-45);

                            RaschetPeriodContainer rp = new RaschetPeriodContainer();

                            if (avail_periods_all.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y))
                                rp = avail_periods_all.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y);
                            else
                                rp = avail_periods_all.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y);

                            if (rp != null)
                                KorrQuarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                            else
                                KorrQuarter.Items.OrderByDescending(x => x.Value).First().Selected = true;
                        }

                        copyInsDataBtn_Click(null,null);

                        break;
                    case "edit":
                        #region Отчетный период

                        try
                        {
                            TypeInfo.Items.Single(x => x.Tag.ToString() == SZV_KORR.TypeInfo.ToString()).Selected = true;

                            if (SZV_KORR.PlatCategoryID.HasValue)
                            {
                                PlatCategory.Items.FirstOrDefault(x => x.Value.ToString() == SZV_KORR.PlatCategoryID.Value.ToString()).Selected = true;
                            }

                            // выпад список "календарный год"

                            if (Year.Items.Any(x => x.Text.ToString() == SZV_KORR.Year.ToString()))
                                Year.Items.Single(x => x.Text.ToString() == SZV_KORR.Year.ToString()).Selected = true;


                            // выпад список "Отчетный период"

                            this.Quarter.Items.Clear();

                            if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y))
                            {
                                foreach (var item in avail_periods_all_2017.Where(x => x.Year == y).ToList())
                                {
                                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                                }
                                if (Quarter.Items.Any(x => x.Value.ToString() == SZV_KORR.Code.ToString()))
                                    Quarter.Items.FirstOrDefault(x => x.Value.ToString() == SZV_KORR.Code.ToString()).Selected = true;

                            }
                        }
                        catch
                        { }


                        try
                        {
                            // выпад список "корр календарный год"

                            if (KorrYear.Items.Any(x => x.Text.ToString() == SZV_KORR.YearKorr.ToString()))
                                KorrYear.Items.Single(x => x.Text.ToString() == SZV_KORR.YearKorr.ToString()).Selected = true;


                            // выпад список "корр Отчетный период"

                            this.KorrQuarter.Items.Clear();

                            if (KorrYear.SelectedItem != null && short.TryParse(KorrYear.SelectedItem.Text, out y))
                            {
                                foreach (var item in avail_periods_all.Where(x => x.Year == y).ToList())
                                {
                                    KorrQuarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                                }
                                if (KorrQuarter.Items.Any(x => x.Value.ToString() == SZV_KORR.CodeKorr.ToString()))
                                    KorrQuarter.Items.FirstOrDefault(x => x.Value.ToString() == SZV_KORR.CodeKorr.ToString()).Selected = true;

                            }
                        }
                        catch
                        { }


                        #endregion

                        DateFilling.Value = SZV_KORR.DateFilling;
                        if (SZV_KORR.ContractType != null)
                            ContractType.Items.Single(x => x.Tag.ToString() == SZV_KORR.ContractType.ToString()).Selected = true;
                        if (SZV_KORR.ContractDate.HasValue)
                            ContractDate.Value = SZV_KORR.ContractDate.Value;
                        ContractNum.Text = SZV_KORR.ContractNum != null ? SZV_KORR.ContractNum : "";

                        if (SZV_KORR.DopTarCode != null)
                            DopTarCode.Items.Single(x => x.Text.ToString() == SZV_KORR.DopTarCode.ToString()).Selected = true;

                        KorrRegNum.Text = SZV_KORR.RegNumKorr;
                        KorrINN.Text = SZV_KORR.INNKorr;
                        KorrKPP.Text = SZV_KORR.KPPKorr;
                        KorrNameShort.Text = SZV_KORR.ShortNameKorr;

                        DismissedCheckBox.Checked = SZV_KORR.Dismissed.HasValue ? SZV_KORR.Dismissed.Value : false;


                        //Информация о сотруднике
                        staff = SZV_KORR.Staff;

                        SNILS.Enabled = false;
                        selectStaffBtn.Enabled = false;

                        FormsSZV_KORR_4_2017_List = SZV_KORR.FormsSZV_KORR_4_2017.ToList();
                        SZV_KORR_4_Grid_update();

                        FormsSZV_KORR_5_2017_List = SZV_KORR.FormsSZV_KORR_5_2017.ToList();
                        SZV_KORR_5_Grid_update();

                        break;
                }

                updateStaffInfo();

                this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();
                this.KorrQuarter.SelectedIndexChanged += (s, с) => KorrQuarter_SelectedIndexChanged();
                this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
                this.KorrYear.SelectedIndexChanged += (s, с) => KorrYear_SelectedIndexChanged();

                setPeriod();

                var stajOsn = SZV_KORR.StajOsn;

                if (StajOsn_List != null)
                    StajOsn_List.Clear();
                foreach (var item in stajOsn)
                {
                    StajOsn_List.Add(item);
                }

                gridUpdate_StajOsn();


                long id_old = 0;
                switch (action)
                {
                    case "add":

                        break;
                    case "edit":
                        id_old = SZV_KORR.ID;
                        SZV_KORR = new FormsSZV_KORR_2017 { ID = id_old };
                        break;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }


        }

        private void Year_SelectedIndexChanged()
        {
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods_all_2017.Where(x => x.Year == y))
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }

                if (Quarter.Items.Count() > 0)
                {
                    if (q != 20 && Quarter.Items.Any(x => x.Value.ToString() == q.ToString()))
                        Quarter.Items.FirstOrDefault(x => x.Value.ToString() == q.ToString()).Selected = true;
                    else
                        Quarter.Items.First().Selected = true;
                }
            }
        }

        private void KorrYear_SelectedIndexChanged()
        {
            byte q = 20;
            if (KorrQuarter.SelectedItem != null && byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out q)) { }

            KorrQuarter.Items.Clear();

            short y;
            if (short.TryParse(KorrYear.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods_all.Where(x => x.Year == y))
                {
                    KorrQuarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }

                if (KorrQuarter.Items.Count() > 0)
                {
                    if (q != 20 && KorrQuarter.Items.Any(x => x.Value.ToString() == q.ToString()))
                        KorrQuarter.Items.FirstOrDefault(x => x.Value.ToString() == q.ToString()).Selected = true;
                    else
                        KorrQuarter.Items.First().Selected = true;
                }
            }
        }

        private void SZV_KORR_4_Grid_update()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            SZV_KORR_4_Grid.Rows.Clear();

            if (FormsSZV_KORR_4_2017_List.Count() != 0)
            {
                foreach (var item in FormsSZV_KORR_4_2017_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.SZV_KORR_4_Grid.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Month"].Value = item.Month.HasValue ? (Monthes.Any(x => x.Code == item.Month.Value) ? Monthes.First(x => x.Code == item.Month.Value).Name : "") : "";
                    rowInfo.Cells["SumFeePFR"].Value = item.SumFeePFR.HasValue ? item.SumFeePFR.Value : 0;
                    rowInfo.Cells["BaseALL"].Value = item.BaseALL.HasValue ? item.BaseALL.Value : 0;
                    rowInfo.Cells["BaseGPD"].Value = item.BaseGPD.HasValue ? item.BaseGPD.Value : 0;
                    rowInfo.Cells["SumPrevBaseALL"].Value = item.SumPrevBaseALL.HasValue ? item.SumPrevBaseALL.Value : 0;
                    rowInfo.Cells["SumPrevBaseGPD"].Value = item.SumPrevBaseGPD.HasValue ? item.SumPrevBaseGPD.Value : 0;
                    rowInfo.Cells["SumFeeBefore2001Insurer"].Value = item.SumFeeBefore2001Insurer.HasValue ? item.SumFeeBefore2001Insurer.Value : 0;
                    rowInfo.Cells["SumFeeBefore2001Staff"].Value = item.SumFeeBefore2001Staff.HasValue ? item.SumFeeBefore2001Staff.Value : 0;
                    rowInfo.Cells["SumFeeAfter2001STRAH"].Value = item.SumFeeAfter2001STRAH.HasValue ? item.SumFeeAfter2001STRAH.Value : 0;
                    rowInfo.Cells["SumFeeAfter2001NAKOP"].Value = item.SumFeeAfter2001NAKOP.HasValue ? item.SumFeeAfter2001NAKOP.Value : 0;
                    rowInfo.Cells["SumPaySTRAH"].Value = item.SumPaySTRAH.HasValue ? item.SumPaySTRAH.Value : 0;
                    rowInfo.Cells["SumPayNAKOP"].Value = item.SumPayNAKOP.HasValue ? item.SumPayNAKOP.Value : 0;
                    rowInfo.Cells["SumFeeTarSV"].Value = item.SumFeeTarSV.HasValue ? item.SumFeeTarSV.Value : 0;

                    SZV_KORR_4_Grid.Rows.Add(rowInfo);
                }
            }

            SZV_KORR_4_Grid.Refresh();
        }

        private void SZV_KORR_5_Grid_update()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            SZV_KORR_5_Grid.Rows.Clear();

            if (FormsSZV_KORR_5_2017_List.Count() != 0)
            {
                foreach (var item in FormsSZV_KORR_5_2017_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.SZV_KORR_5_Grid.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["CodeOcenki"].Value = item.SpecOcenkaUslTrudaID.HasValue ? item.SpecOcenkaUslTruda.Code : "";
                    rowInfo.Cells["Month"].Value = item.Month.HasValue ? (Monthes.Any(x => x.Code == item.Month.Value) ? Monthes.First(x => x.Code == item.Month.Value).Name : "") : "";
                    rowInfo.Cells["s_0"].Value = item.s_0;
                    rowInfo.Cells["s_1"].Value = item.s_1;
                    SZV_KORR_5_Grid.Rows.Add(rowInfo);
                }
            }

            this.SZV_KORR_5_Grid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            SZV_KORR_5_Grid.Refresh();
        }

        private void Quarter_SelectedIndexChanged()
        {
            if (Quarter.SelectedItem != null)
            {
                setPeriod();
            }
        }

        private void setPeriod()
        {
            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                byte q;
                if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                {
                    period = q;
                }
            }

        }

        private void KorrQuarter_SelectedIndexChanged()
        {
            if (KorrQuarter.SelectedItem != null)
            {
                setKorrPeriod();
            }
        }

        private void setKorrPeriod()
        {
            short y;
            if (short.TryParse(KorrYear.SelectedItem.Text, out y))
            {
                byte q;
                if (byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out q))
                {
                    periodKorr = q;
                }
            }

        }

        
        #region Основной стаж

        /// <summary>
        /// обновление таблицы раздела 6.8 Основной стаж
        /// </summary>
        private void gridUpdate_StajOsn()
        {
            stajOsnGrid.Rows.Clear();

            StajOsn_List = StajOsn_List.OrderBy(x => x.Number.Value).ToList();

            if (StajOsn_List.Count() != 0)
            {
                foreach (var item in StajOsn_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.stajOsnGrid.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Number"].Value = item.Number;
                    rowInfo.Cells["DateBegin"].Value = item.DateBegin.Value.ToShortDateString();
                    rowInfo.Cells["DateEnd"].Value = item.DateEnd.Value.ToShortDateString();
                    rowInfo.Cells["codeBEZR"].Value = item.CodeBEZR.HasValue ? item.CodeBEZR.Value : false;
                    stajOsnGrid.Rows.Add(rowInfo);
                }
            }

            this.stajOsnGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            stajOsnGrid.Refresh();

            // Обновляем таблицу доп записей по стажу
            gridUpdate_StajLgot();
        }

        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton15_Click(object sender, EventArgs e)
        {
            StajOsnFrm child = new StajOsnFrm();
            child.Owner = this;
            child.action = "add";
            child.ParentFormName = "SZV_KORR_Edit";
            child.dateControl = dateControlCheckBox.Checked;
            child.formData = new StajOsn();
            child.rowindex = -1;
            var y = short.Parse(KorrYear.Text);
            var q = periodKorr;

            child.period = avail_periods_all.FirstOrDefault(x => x.Kvartal == q && x.Year == y);
            if (StajOsn_List.Count == 0)
            {
                child.StajBeginDate.Value = child.period.DateBegin;
            }
            else
            {
                var date = StajOsn_List.Max(x => x.DateEnd).Value;
                child.StajBeginDate.Value = date.AddDays(1);
                child.NumberSpin.Value = StajOsn_List.Max(x => x.Number).Value + 1;
            }

            child.StajEndDate.Value = child.period.DateEnd;

            if (child.StajBeginDate.Value > child.StajEndDate.Value)
            {
                child.StajEndDate.Value = child.StajBeginDate.Value.AddDays(1);
            }

            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
            if (child.formData != null)
            {
                StajOsn_List.Add(child.formData);
                gridUpdate_StajOsn();
            }


        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void radButton14_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int rowindex = stajOsnGrid.CurrentRow.Index;

                StajOsnFrm child = new StajOsnFrm();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ParentFormName = "SZV_KORR_Edit";
                child.dateControl = dateControlCheckBox.Checked;
                child.action = "edit";
                var y = short.Parse(Year.Text);
                var q = period;
                child.period = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == q && x.Year == y);
                child.formData = StajOsn_List.Skip(rowindex).Take(1).First();
                child.rowindex = rowindex;
                child.ShowDialog();


                if (child.formData != null)
                {
                    //    var rsw_ind = StajOsn_List.FindIndex(x => x.ID == child.formData.ID);
                    StajOsn_List.RemoveAt(rowindex);
                    StajOsn_List.Insert(rowindex, child.formData);

                    gridUpdate_StajOsn();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton13_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int rowindex = stajOsnGrid.CurrentRow.Index;
                StajOsn_List.RemoveAt(rowindex);
                gridUpdate_StajOsn();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void stajOsnGrid_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            radButton14_Click(null, null);
        }

        private void stajOsnGrid_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            gridUpdate_StajLgot();
        }


        #endregion

        #region Льготный стаж

        /// <summary>
        /// обновление таблицы раздела 6.8 Льготный стаж
        /// </summary>
        private void gridUpdate_StajLgot()
        {
            stajLgotGrid.Rows.Clear();

            if (StajOsn_List.Count > 0)
            {

                int rowindex = 0;

                if (stajOsnGrid.CurrentRow != null)
                {
                    rowindex = stajOsnGrid.CurrentRow.Index;
                }

                List<StajLgot> StajLgot_List = StajOsn_List.Skip(rowindex).Take(1).First().StajLgot.OrderBy(x => x.Number).ToList();

                if (StajLgot_List.Count() != 0)
                {
                    foreach (var item in StajLgot_List)
                    {
                        string str = item.IschislStrahStajDopID == null ? "" : item.IschislStrahStajDopID.HasValue ? db.IschislStrahStajDop.FirstOrDefault(x => x.ID == item.IschislStrahStajDopID).Code : "";
                        string s1 = item.Strah1Param.HasValue == true ? item.Strah1Param.Value.ToString() : "0";
                        string s2 = item.Strah2Param.HasValue == true ? item.Strah2Param.Value.ToString() : "0";

                        str = "[" + s1 + "][" + s2 + "][" + str + "]";

                        string s1_ = item.UslDosrNazn1Param.HasValue == true ? item.UslDosrNazn1Param.Value.ToString() : "0";
                        string s2_ = item.UslDosrNazn2Param.HasValue == true ? item.UslDosrNazn2Param.Value.ToString() : "0";
                        string s3_ = item.UslDosrNazn3Param.HasValue == true ? item.UslDosrNazn3Param.Value.ToString() : "0.00";

                        string str_ = "[" + s1_ + "][" + s2_ + "][" + s3_ + "]";

                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.stajLgotGrid.MasterView);
                        rowInfo.Cells["ID"].Value = item.ID;
                        rowInfo.Cells["Number"].Value = item.Number;
                        rowInfo.Cells["TerrUslCode"].Value = item.TerrUslID == null ? "" : item.TerrUslID.HasValue ? db.TerrUsl.FirstOrDefault(x => x.ID == item.TerrUslID.Value).Code : "";
                        rowInfo.Cells["TerrUslKoef"].Value = item.TerrUslKoef == null ? "" : item.TerrUslKoef.HasValue ? item.TerrUslKoef.Value.ToString() : "";
                        rowInfo.Cells["OsobUslCode"].Value = item.OsobUslTrudaID == null ? "" : item.OsobUslTrudaID.HasValue ? db.OsobUslTruda.FirstOrDefault(x => x.ID == item.OsobUslTrudaID).Code : "";
                        rowInfo.Cells["KodVredOsn"].Value = item.KodVred_OsnID == null ? "" : item.KodVred_OsnID.HasValue ? db.KodVred_2.FirstOrDefault(x => x.ID == item.KodVred_OsnID).Code : "";
                        rowInfo.Cells["IschislStrahOsn"].Value = item.IschislStrahStajOsnID == null ? "" : item.IschislStrahStajOsnID.HasValue ? db.IschislStrahStajOsn.FirstOrDefault(x => x.ID == item.IschislStrahStajOsnID).Code : "";
                        rowInfo.Cells["IschislStrahDop"].Value = str;
                        rowInfo.Cells["UslDosrNaznOsn"].Value = item.UslDosrNaznID == null ? "" : item.UslDosrNaznID.HasValue ? db.UslDosrNazn.FirstOrDefault(x => x.ID == item.UslDosrNaznID).Code : "";
                        rowInfo.Cells["UslDosrNaznDop"].Value = str_;
                        stajLgotGrid.Rows.Add(rowInfo);
                    }
                }

                //                this.stajLgotGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                stajLgotGrid.Refresh();
            }
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void radButton16_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount > 0)
            {
                StajLgotFrm child = new StajLgotFrm();
                child.Owner = this;
                child.action = "add";
                int rowindex = stajOsnGrid.CurrentRow.Index;
                child.StajOsnData = StajOsn_List.Skip(rowindex).Take(1).First();

                if (child.StajOsnData.StajLgot.Count != 0)
                {
                    child.NumberSpin.Value = child.StajOsnData.StajLgot.Max(x => x.Number).Value + 1;
                }

                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ShowDialog();
                if (child.formData != null)
                {
                    var item = child.formData;

                    StajLgot r = new StajLgot();

                    var fields = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    var names = Array.ConvertAll(fields, field => field.Name);

                    foreach (var itemName_ in names)
                    {
                        string itemName = itemName_.TrimStart('_');
                        var properties = item.GetType().GetProperty(itemName);
                        if (properties != null)
                        {
                            object value = properties.GetValue(item, null);
                            var data = value;

                            r.GetType().GetProperty(itemName).SetValue(r, data, null);
                        }

                    }
                    StajOsn_List.Skip(rowindex).Take(1).First().StajLgot.Add(r);

                    gridUpdate_StajLgot();
                }
            }

        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void radButton18_Click(object sender, EventArgs e)
        {
            if (stajLgotGrid.RowCount > 0)
            {
                int rowindex = stajLgotGrid.CurrentRow.Index;
                StajOsn st = StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First();

                StajLgotFrm child = new StajLgotFrm();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.StajOsnData = st;
                child.formData = st.StajLgot.Skip(rowindex).Take(1).First();
                child.rowindex = rowindex;
                child.ShowDialog();


                if (child.formData != null)
                {
                    //    var rsw_ind = StajOsn_List.FindIndex(x => x.ID == child.formData.ID);
                    //StajLgot_List.RemoveAt(rowindex);
                    //StajLgot_List.Insert(rowindex, child.formData);
                    List<StajLgot> stl = StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.ToList();
                    stl.RemoveAt(rowindex);
                    stl.Insert(rowindex, child.formData);

                    StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.Clear();

                    foreach (var item in stl.OrderByDescending(x => x.Number))
                    {

                        StajLgot r = new StajLgot();

                        var fields = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        var names = Array.ConvertAll(fields, field => field.Name);

                        foreach (var itemName_ in names)
                        {
                            string itemName = itemName_.TrimStart('_');
                            var properties = item.GetType().GetProperty(itemName);
                            if (properties != null)
                            {
                                object value = properties.GetValue(item, null);
                                var data = value;

                                r.GetType().GetProperty(itemName).SetValue(r, data, null);
                            }

                        }
                        StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.Add(r);
                    }


                    gridUpdate_StajLgot();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }
        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton17_Click(object sender, EventArgs e)
        {
            if (stajLgotGrid.RowCount > 0)
            {
                int rowindex = stajLgotGrid.CurrentRow.Index;
                List<StajLgot> stl = StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.ToList();
                stl.RemoveAt(rowindex);

                StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.Clear();

                foreach (var item in stl)
                {

                    StajLgot r = new StajLgot();

                    var fields = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    var names = Array.ConvertAll(fields, field => field.Name);

                    foreach (var itemName_ in names)
                    {
                        string itemName = itemName_.TrimStart('_');
                        var properties = item.GetType().GetProperty(itemName);
                        if (properties != null)
                        {
                            object value = properties.GetValue(item, null);
                            var data = value;

                            r.GetType().GetProperty(itemName).SetValue(r, data, null);
                        }

                    }
                    StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.Add(r);
                }

                gridUpdate_StajLgot();

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }


        private void stajLgotGrid_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            radButton18_Click(null, null);
        }

        private void stajLgotGrid_SizeChanged(object sender, EventArgs e)
        {
            if (stajLgotGrid.Width >= 753)
            {
                stajLgotGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                stajLgotGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                stajLgotGrid.Columns[1].Width = 24;
                stajLgotGrid.Columns[2].Width = 82;
                stajLgotGrid.Columns[3].Width = 46;
                stajLgotGrid.Columns[4].Width = 90;
                stajLgotGrid.Columns[5].Width = 94;
                stajLgotGrid.Columns[6].Width = 100;
                stajLgotGrid.Columns[7].Width = 112;
                stajLgotGrid.Columns[8].Width = 100;
                stajLgotGrid.Columns[9].Width = 112;
            }
        }

        #endregion

        private void moveStajUP_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int index = stajOsnGrid.CurrentRow.Index;
                if (index != 0)
                {
                    long numPrev = StajOsn_List.Skip(index - 1).Take(1).First().Number.Value;
                    long numCurr = StajOsn_List.Skip(index).Take(1).First().Number.Value;
                    StajOsn_List.Skip(index).Take(1).First().Number--; // уменьшаем на 1 (поднимаем вверх)
                    if ((numCurr - numPrev) == 1) // если последовательность один за одним например 2, 3
                    {
                        StajOsn_List.Skip(index - 1).Take(1).First().Number++; // увеличиваем номер на 1 (опускаем ниже)
                        index--;
                    }
                    else // если в последовательности есть промежутки например 2, 4
                    {

                    }

                    gridUpdate_StajOsn();
                    stajOsnGrid.Rows[index].IsCurrent = true;
                }

            }
        }

        private void moveStajDOWN_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int index = stajOsnGrid.CurrentRow.Index;
                if (index != (stajOsnGrid.RowCount - 1))
                {
                    long numNext = StajOsn_List.Skip(index + 1).Take(1).First().Number.Value;
                    long numCurr = StajOsn_List.Skip(index).Take(1).First().Number.Value;
                    StajOsn_List.Skip(index).Take(1).First().Number++; // уменьшаем на 1 (поднимаем вверх)
                    if ((numNext - numCurr) == 1) // если последовательность один за одним например 2, 3
                    {
                        StajOsn_List.Skip(index + 1).Take(1).First().Number--; // увеличиваем номер на 1 (опускаем ниже)
                        index++;
                    }
                    else // если в последовательности есть промежутки например 2, 4
                    {

                    }

                    gridUpdate_StajOsn();
                    stajOsnGrid.Rows[index].IsCurrent = true;
                }

            }
        }

        private void moveStajAUTO_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                StajOsn_List = StajOsn_List.OrderBy(x => x.DateBegin.Value).ToList();
                int num = 1;
                foreach (var item in StajOsn_List)
                {
                    item.Number = num;
                    num++;
                }

                gridUpdate_StajOsn();
            }
        }

        private void copyInsDataBtn_Click(object sender, EventArgs e)
        {
            KorrNameShort.Text = NameShort.Text;
            KorrINN.Text = INN.Text;
            KorrKPP.Text = KPP.Text;
            KorrRegNum.Text = RegNum.Text;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (Quarter.SelectedItem != null)
                setPeriod();
            if (validation())
            {
                bool flag_ok = false;
                SZV_KORR.Year = short.Parse(Year.Text);
                SZV_KORR.Code = period;
                SZV_KORR.InsurerID = Options.InsID;
                SZV_KORR.Staff = staff;
                SZV_KORR.DateFilling = DateFilling.Value.Date;
                SZV_KORR.ContractType = ContractType.SelectedItem != null ? byte.Parse(ContractType.SelectedItem.Tag.ToString()) : (byte)0;
                SZV_KORR.TypeInfo = byte.Parse(TypeInfo.SelectedItem.Tag.ToString());
                SZV_KORR.Dismissed = DismissedCheckBox.Checked;


                if (!String.IsNullOrEmpty(PlatCategory.Text))
                    SZV_KORR.PlatCategoryID = long.Parse(PlatCategory.SelectedItem.Value.ToString());
                else
                    SZV_KORR.PlatCategoryID = null;


                short yk;
                if (short.TryParse(KorrYear.SelectedItem.Text, out yk))
                {
                    SZV_KORR.YearKorr = yk;
                    byte qk;
                    if (byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out qk))
                    {
                        SZV_KORR.CodeKorr = qk;
                    }
                }

                if (ContractDate.Value != ContractDate.NullDate)
                {
                    SZV_KORR.ContractDate = ContractDate.Value;
                }

                SZV_KORR.ContractNum = ContractNum.Text;

                SZV_KORR.DopTarCode = DopTarCode.Text.Trim();

                string regN = "";

                var s = (KorrRegNum.Value.ToString()).Split('-');
                foreach (var item in s)
                {
                    if (!item.Contains("_"))
                        regN += item;
                    else
                    {
                        regN = "";
                        break;
                    }
                }

                SZV_KORR.RegNumKorr = regN;
                SZV_KORR.INNKorr = KorrINN.Text;
                SZV_KORR.KPPKorr = KorrKPP.Text;
                SZV_KORR.ShortNameKorr = KorrNameShort.Text;


                switch (action)
                {
                    case "add":
                        db.FormsSZV_KORR_2017.Add(SZV_KORR);
                        db.SaveChanges();
                        try
                        {
                            foreach (var item in FormsSZV_KORR_4_2017_List)
                            {
                                item.FormsSZV_KORR_2017_ID = SZV_KORR.ID;
                                FormsSZV_KORR_4_2017 r = new FormsSZV_KORR_4_2017();

                                var fields = typeof(FormsSZV_KORR_4_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = item.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(item, null);
                                        var data = value;

                                        r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                    }

                                }

                                db.FormsSZV_KORR_4_2017.Add(r);
                            }

                            foreach (var item in FormsSZV_KORR_5_2017_List)
                            {
                                item.FormsSZV_KORR_2017_ID = SZV_KORR.ID;
                                FormsSZV_KORR_5_2017 r = new FormsSZV_KORR_5_2017();

                                var fields = typeof(FormsSZV_KORR_5_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = item.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(item, null);
                                        var data = value;

                                        r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                    }

                                }

                                db.FormsSZV_KORR_5_2017.Add(r);
                            }

                            var fields_lgot = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names_lgot = Array.ConvertAll(fields_lgot, field => field.Name);


                            foreach (var item in StajOsn_List)
                            {
                                item.FormsSZV_KORR_2017_ID = SZV_KORR.ID;
                                StajOsn r = new StajOsn();

                                var fields = typeof(StajOsn).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = item.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(item, null);
                                        var data = value;

                                        r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                    }

                                }
                                db.StajOsn.Add(r);
                                try
                                {
                                    flag_ok = false;
                                    db.SaveChanges();
                                    flag_ok = true;
                                }
                                catch (Exception ex)
                                {
                                    RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                                }

                                //добавление дополнительных льготных записей
                                foreach (var item_lgot in item.StajLgot)
                                {
                                    // добавление записи в БД
                                    item_lgot.StajOsnID = r.ID;
                                    StajLgot r_ = new StajLgot();

                                    foreach (var itemName_ in names_lgot)
                                    {
                                        string itemName = itemName_.TrimStart('_');
                                        var properties = item_lgot.GetType().GetProperty(itemName);
                                        if (properties != null)
                                        {
                                            object value = properties.GetValue(item_lgot, null);
                                            var data = value;

                                            r_.GetType().GetProperty(itemName).SetValue(r_, data, null);
                                        }

                                    }

                                    db.StajLgot.Add(r_);
                                }

                            }

                            flag_ok = true;

                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);

                        }
                        if (flag_ok)
                        {
                            try
                            {
                                flag_ok = false;
                                db.SaveChanges();
                                allowClose = true;
                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                            }
                            if (flag_ok)
                                this.Close();
                        }
                        break;
                    case "edit":
                        // выбираем из базы исходную запись по идешнику
                        db = new pu6Entities();
                        FormsSZV_KORR_2017 r6 = db.FormsSZV_KORR_2017.FirstOrDefault(x => x.ID == SZV_KORR.ID);
                        try
                        {

                            r6.DateFilling = SZV_KORR.DateFilling;
                            r6.Year = SZV_KORR.Year;
                            r6.Code = SZV_KORR.Code;
                            r6.YearKorr = SZV_KORR.YearKorr;
                            r6.CodeKorr = SZV_KORR.CodeKorr;
                            r6.TypeInfo = SZV_KORR.TypeInfo;
                            r6.StaffID = SZV_KORR.StaffID;
                            r6.ContractType = SZV_KORR.ContractType;
                            if (SZV_KORR.ContractDate.HasValue)
                                r6.ContractDate = SZV_KORR.ContractDate;
                            r6.ContractNum = SZV_KORR.ContractNum;
                            r6.DopTarCode = SZV_KORR.DopTarCode;
                            if (!String.IsNullOrEmpty(PlatCategory.Text))
                                r6.PlatCategoryID = SZV_KORR.PlatCategoryID;
                            else
                                r6.PlatCategoryID = null;
                            r6.RegNumKorr = SZV_KORR.RegNumKorr;
                            r6.INNKorr = SZV_KORR.INNKorr;
                            r6.KPPKorr = SZV_KORR.KPPKorr;
                            r6.ShortNameKorr = SZV_KORR.ShortNameKorr;


                            // сохраняем модифицированную запись обратно в бд
                            db.Entry(r6).State = EntityState.Modified;
                            db.SaveChanges();
                            flag_ok = true;



                            {
                                flag_ok = false;
                                #region обрабатываем записи о выплатах из Раздела 4
                                try
                                {
                                    var FormsSZV_KORR_4_2017_List_from_db = db.FormsSZV_KORR_4_2017.Where(x => x.FormsSZV_KORR_2017_ID == r6.ID);

                                    // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                    var t = FormsSZV_KORR_4_2017_List.Select(x => x.ID);
                                    var list_for_del = FormsSZV_KORR_4_2017_List_from_db.Where(x => !t.Contains(x.ID));

                                    foreach (var item in list_for_del)
                                    {
                                        db.FormsSZV_KORR_4_2017.Remove(item);
                                    }

                                    if (list_for_del.Count() != 0)
                                    {
                                        //db.SaveChanges();
                                        FormsSZV_KORR_4_2017_List_from_db = db.FormsSZV_KORR_4_2017.Where(x => x.FormsSZV_KORR_2017_ID == r6.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                    }


                                    // проверка текущих записей Раздела 6.4 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                    var fields = typeof(FormsSZV_KORR_4_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    var names = Array.ConvertAll(fields, field => field.Name);


                                    foreach (var item in FormsSZV_KORR_4_2017_List)
                                    {
                                        bool flag_add_new = true;
                                        //если такая запись есть, надо проверять на отличия
                                        if (FormsSZV_KORR_4_2017_List_from_db.Any(x => x.ID == item.ID))
                                        {
                                            flag_add_new = false;
                                            bool flag_edited = false;
                                            FormsSZV_KORR_4_2017 szv4_temp = FormsSZV_KORR_4_2017_List_from_db.Single(x => x.ID == item.ID);


                                            foreach (var item_ in names)
                                            {
                                                string itemName = item_.TrimStart('_');
                                                if (item_.IndexOf("FormsSZV_KORR_2017_ID") < 0)
                                                {
                                                    string data_old = "";
                                                    string data_new = "";

                                                    var properties_old = szv4_temp.GetType().GetProperty(itemName);
                                                    object value_old = properties_old.GetValue(szv4_temp, null);
                                                    data_old = value_old != null ? value_old.ToString() : "";

                                                    var properties_new = item.GetType().GetProperty(itemName);
                                                    object value_new = properties_new.GetValue(item, null);
                                                    data_new = value_new != null ? value_new.ToString() : "";

                                                    if (data_old != data_new)
                                                    {
                                                        flag_edited = true;

                                                        szv4_temp.GetType().GetProperty(itemName).SetValue(szv4_temp, value_new, null);
                                                    }

                                                }
                                            }


                                            if (flag_edited) // если записи отличаются
                                            {
                                                db.Entry(szv4_temp).State = EntityState.Modified;

                                            }

                                        }
                                        if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                        {

                                            // добавление записи в БД
                                            item.FormsSZV_KORR_2017_ID = SZV_KORR.ID;
                                            FormsSZV_KORR_4_2017 r = new FormsSZV_KORR_4_2017();

                                            foreach (var itemName_ in names)
                                            {
                                                string itemName = itemName_.TrimStart('_');
                                                var properties = item.GetType().GetProperty(itemName);
                                                if (properties != null)
                                                {
                                                    object value = properties.GetValue(item, null);
                                                    var data = value;

                                                    r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                                }

                                            }

                                            db.FormsSZV_KORR_4_2017.Add(r);
                                        }


                                    }

                                    flag_ok = true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);

                                }
                                #endregion


                                #region обрабатываем записи о выплатах из Раздела 7
                                try
                                {
                                    var FormsSZV_KORR_5_2017_List_from_db = db.FormsSZV_KORR_5_2017.Where(x => x.FormsSZV_KORR_2017_ID == r6.ID);

                                    // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                    var t = FormsSZV_KORR_5_2017_List.Select(x => x.ID);
                                    var list_for_del = FormsSZV_KORR_5_2017_List_from_db.Where(x => !t.Contains(x.ID));

                                    foreach (var item in list_for_del)
                                    {
                                        db.FormsSZV_KORR_5_2017.Remove(item);
                                    }

                                    if (list_for_del.Count() != 0)
                                    {
                                        //db.SaveChanges();
                                        FormsSZV_KORR_5_2017_List_from_db = db.FormsSZV_KORR_5_2017.Where(x => x.FormsSZV_KORR_2017_ID == r6.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                    }


                                    // проверка текущих записей Раздела 7 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                    var fields = typeof(FormsSZV_KORR_5_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    var names = Array.ConvertAll(fields, field => field.Name);


                                    foreach (var item in FormsSZV_KORR_5_2017_List)
                                    {
                                        bool flag_add_new = true;
                                        //если такая запись есть, надо проверять на отличия
                                        if (FormsSZV_KORR_5_2017_List_from_db.Any(x => x.ID == item.ID))
                                        {
                                            flag_add_new = false;
                                            bool flag_edited = false;
                                            FormsSZV_KORR_5_2017 szv7 = FormsSZV_KORR_5_2017_List_from_db.Single(x => x.ID == item.ID);


                                            foreach (var item_ in names)
                                            {
                                                string itemName = item_.TrimStart('_');
                                                if (item_.IndexOf("FormsSZV_KORR_2017_ID") < 0)
                                                {
                                                    string data_old = "";
                                                    string data_new = "";

                                                    var properties_old = szv7.GetType().GetProperty(itemName);
                                                    object value_old = properties_old.GetValue(szv7, null);
                                                    data_old = value_old != null ? value_old.ToString() : "";

                                                    var properties_new = item.GetType().GetProperty(itemName);
                                                    object value_new = properties_new.GetValue(item, null);
                                                    data_new = value_new != null ? value_new.ToString() : "";

                                                    if (data_old != data_new)
                                                    {
                                                        flag_edited = true;

                                                        szv7.GetType().GetProperty(itemName).SetValue(szv7, value_new, null);
                                                    }

                                                }
                                            }


                                            if (flag_edited) // если записи отличаются
                                            {
                                                db.Entry(szv7).State = EntityState.Modified;

                                            }

                                        }
                                        if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                        {

                                            // добавление записи в БД
                                            item.FormsSZV_KORR_2017_ID = SZV_KORR.ID;
                                            FormsSZV_KORR_5_2017 r = new FormsSZV_KORR_5_2017();

                                            foreach (var itemName_ in names)
                                            {
                                                string itemName = itemName_.TrimStart('_');
                                                var properties = item.GetType().GetProperty(itemName);
                                                if (properties != null)
                                                {
                                                    object value = properties.GetValue(item, null);
                                                    var data = value;

                                                    r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                                }

                                            }

                                            db.FormsSZV_KORR_5_2017.Add(r);
                                        }


                                    }

                                    flag_ok = true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);

                                }

                                #endregion

                                #region обрабатываем записи о Стаже (из Раздела 8)
                                try
                                {
                                    var StajOsn_List_from_db = db.StajOsn.Where(x => x.FormsSZV_KORR_2017_ID == r6.ID);

                                    // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                    var t = StajOsn_List.Select(x => x.ID);
                                    var list_for_del = StajOsn_List_from_db.Where(x => !t.Contains(x.ID));

                                    foreach (var item in list_for_del)
                                    {
                                        if (item.StajLgot.Any())
                                        {
                                            List<long> l_id = item.StajLgot.Select(x => x.ID).ToList();
                                            foreach (var stl in l_id)
                                            {
                                                StajLgot l = db.StajLgot.FirstOrDefault(x => x.ID == stl);
                                                db.StajLgot.Remove(l);
                                            }
                                        }

                                        db.StajOsn.Remove(item);
                                    }

                                    if (list_for_del.Count() != 0)
                                    {
                                        //db.SaveChanges();
                                        StajOsn_List_from_db = db.StajOsn.Where(x => x.FormsSZV_KORR_2017_ID == r6.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                    }


                                    // проверка текущих записей Раздела 6.8 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                    var fields = typeof(StajOsn).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    var names = Array.ConvertAll(fields, field => field.Name);

                                    var fields_lgot = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    var names_lgot = Array.ConvertAll(fields_lgot, field => field.Name);


                                    bool flag_ok_lgot = true;
                                    foreach (var item in StajOsn_List)
                                    {
                                        bool flag_add_new = true;
                                        //если такая запись есть, надо проверять на отличия
                                        if (StajOsn_List_from_db.Any(x => x.ID == item.ID))
                                        {

                                            #region Проверка дополнительных льготных записей
                                            flag_ok_lgot = false;
                                            try
                                            {
                                                var StajLgot_List_from_db = db.StajLgot.Where(x => x.StajOsnID == item.ID);

                                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                                var StajLgot_List = item.StajLgot;
                                                var t_lgot = StajLgot_List.Select(x => x.ID);
                                                var list_for_del_lgot = StajLgot_List_from_db.Where(x => !t_lgot.Contains(x.ID));

                                                foreach (var item_lgot in list_for_del_lgot)
                                                {
                                                    db.StajLgot.Remove(item_lgot);
                                                }

                                                if (list_for_del_lgot.Count() != 0)
                                                {
                                                    //db.SaveChanges();
                                                    StajLgot_List_from_db = db.StajLgot.Where(x => x.StajOsnID == item.ID && !list_for_del_lgot.Select(y => y.ID).Contains(x.ID));
                                                }

                                                // Проверка дополнительных льготных записей на факт их редактирования, если записи новая то добавляем ее



                                                foreach (var item_lgot in StajLgot_List)
                                                {
                                                    bool flag_lgot_add_new = true;
                                                    //если такая запись есть, надо проверять на отличия
                                                    if (StajLgot_List_from_db.Any(x => x.ID == item_lgot.ID))
                                                    {
                                                        flag_lgot_add_new = false;
                                                        bool flag_lgot_edited = false;
                                                        StajLgot lgot_temp = StajLgot_List_from_db.Single(x => x.ID == item_lgot.ID);


                                                        foreach (var item_lgot_ in names_lgot)
                                                        {
                                                            string itemName = item_lgot_.TrimStart('_');
                                                            if (itemName != "StajOsnID")
                                                            {
                                                                string data_old = "";
                                                                string data_new = "";

                                                                var properties_old = lgot_temp.GetType().GetProperty(itemName);
                                                                object value_old = properties_old.GetValue(lgot_temp, null);
                                                                if (value_old != null)
                                                                    data_old = value_old.ToString();

                                                                var properties_new = item_lgot.GetType().GetProperty(itemName);
                                                                object value_new = properties_new.GetValue(item_lgot, null);
                                                                if (value_new != null)
                                                                    data_new = value_new.ToString();

                                                                if (data_old != data_new)
                                                                {
                                                                    flag_lgot_edited = true;

                                                                    lgot_temp.GetType().GetProperty(itemName).SetValue(lgot_temp, value_new, null);
                                                                }
                                                            }
                                                        }


                                                        if (flag_lgot_edited) // если записи отличаются
                                                        {

                                                            db.Entry(lgot_temp).State =  EntityState.Modified;

                                                        }


                                                    }
                                                    if (flag_lgot_add_new) // такой записи в базе нет, значит просто добавляем ее
                                                    {

                                                        // добавление записи в БД
                                                        item_lgot.StajOsnID = item.ID;
                                                        StajLgot r = new StajLgot();

                                                        foreach (var itemName_ in names_lgot)
                                                        {
                                                            string itemName = itemName_.TrimStart('_');
                                                            var properties = item_lgot.GetType().GetProperty(itemName);
                                                            if (properties != null)
                                                            {
                                                                object value = properties.GetValue(item_lgot, null);
                                                                var data = value;

                                                                r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                                            }

                                                        }

                                                        db.StajLgot.Add(r);
                                                    }



                                                }
                                                flag_ok_lgot = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                MessageBox.Show("При сохранение данных Дополнительныз записей о льготном стаже произошла ошибка. Код ошибки: " + ex.Message);

                                            }


                                            #endregion


                                            if (flag_ok_lgot)
                                            {
                                                flag_add_new = false;
                                                bool flag_edited = false;
                                                StajOsn rsw_temp = StajOsn_List_from_db.Single(x => x.ID == item.ID);


                                                foreach (var item_ in names)
                                                {
                                                    string itemName = item_.TrimStart('_');
                                                    if (item_.IndexOf("FormsSZV_KORR_2017_ID") < 0)
                                                    {
                                                        string data_old = "";
                                                        string data_new = "";

                                                        var properties_old = rsw_temp.GetType().GetProperty(itemName);
                                                        object value_old = properties_old.GetValue(rsw_temp, null);
                                                        data_old = value_old != null ? value_old.ToString() : "";

                                                        var properties_new = item.GetType().GetProperty(itemName);
                                                        object value_new = properties_new.GetValue(item, null);
                                                        data_new = value_new != null ? value_new.ToString() : "";

                                                        if (data_old != data_new)
                                                        {
                                                            flag_edited = true;

                                                            rsw_temp.GetType().GetProperty(itemName).SetValue(rsw_temp, value_new, null);
                                                        }
                                                    }
                                                }


                                                if (flag_edited) // если записи отличаются
                                                {

                                                    db.Entry(rsw_temp).State = EntityState.Modified;

                                                }
                                            }

                                        }
                                        if (flag_add_new && flag_ok_lgot) // такой записи в базе нет, значит просто добавляем ее
                                        {

                                            // добавление записи в БД
                                            item.FormsSZV_KORR_2017_ID = SZV_KORR.ID;
                                            StajOsn r = new StajOsn();

                                            foreach (var itemName_ in names)
                                            {
                                                string itemName = itemName_.TrimStart('_');
                                                var properties = item.GetType().GetProperty(itemName);
                                                if (properties != null)
                                                {
                                                    object value = properties.GetValue(item, null);
                                                    var data = value;

                                                    r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                                }

                                            }

                                            db.StajOsn.Add(r);

                                            try
                                            {
                                                flag_ok = false;
                                                db.SaveChanges();
                                                flag_ok = true;
                                            }
                                            catch (Exception ex)
                                            {
                                                RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                                            }


                                            //добавление дополнительных льготных записей
                                            foreach (var item_lgot in item.StajLgot)
                                            {
                                                // добавление записи в БД
                                                item_lgot.StajOsnID = r.ID;
                                                StajLgot r_ = new StajLgot();

                                                foreach (var itemName_ in names_lgot)
                                                {
                                                    string itemName = itemName_.TrimStart('_');
                                                    var properties = item_lgot.GetType().GetProperty(itemName);
                                                    if (properties != null)
                                                    {
                                                        object value = properties.GetValue(item_lgot, null);
                                                        var data = value;

                                                        r_.GetType().GetProperty(itemName).SetValue(r_, data, null);
                                                    }

                                                }

                                                db.StajLgot.Add(r_);
                                            }

                                        }


                                    }

                                    flag_ok = true;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);

                                }

                                #endregion

                                if (flag_ok)
                                {
                                    try
                                    {
                                        flag_ok = false;
                                        db.SaveChanges();
                                        allowClose = true;
                                        flag_ok = true;
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                                    }
                                    if (flag_ok)
                                        this.Close();
                                }
                            }




                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                        }

                        break;
                }
            }
            else
            {
                if (errMessBox.Count != 0)
                {
                    foreach (var item in errMessBox)
                    {
                        RadMessageBox.Show(this, item, "Внимание!");
                    }
                }

            }
        }

        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();
            short y = 0;
            if (Year.Text == "")
                errMessBox.Add("Календарный год должен быть заполнен!");

            if (Quarter.Text == "")
                errMessBox.Add("Отчетный период должен быть заполнен!");

            if (staff == null)
            {
                errMessBox.Add("Необходимо выбрать сотрудника");
                check = false;
                return check;
            }

            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        private void addBtn_4_Click(object sender, EventArgs e)
        {

            SZV_KORR_4_Edit child = new SZV_KORR_4_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.Monthes = Monthes;
            child.ShowDialog();
            if (child.formData != null)
            {
                FormsSZV_KORR_4_2017_List.Add(child.formData);
                SZV_KORR_4_Grid_update();
            }

        }

        private void editBtn_4_Click(object sender, EventArgs e)
        {
            if (SZV_KORR_4_Grid.RowCount != 0)
            {
                int rowindex = SZV_KORR_4_Grid.CurrentRow.Index;
                FormsSZV_KORR_4_2017 szv4_temp = FormsSZV_KORR_4_2017_List.Skip(rowindex).Take(1).First();


                SZV_KORR_4_Edit child = new SZV_KORR_4_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.formData = szv4_temp;
                child.Monthes = Monthes;
                child.ShowDialog();

                if (child.formData != null)
                {
                    FormsSZV_KORR_4_2017_List.RemoveAt(rowindex);
                    FormsSZV_KORR_4_2017_List.Insert(rowindex, child.formData);

                    SZV_KORR_4_Grid_update();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void delBtn_4_Click(object sender, EventArgs e)
        {
            if (SZV_KORR_4_Grid.RowCount != 0 && SZV_KORR_4_Grid.CurrentRow.Cells[0].Value != null)
            {
                int rowindex = SZV_KORR_4_Grid.CurrentRow.Index;
                if (rowindex >= 0)
                {
                    FormsSZV_KORR_4_2017_List.RemoveAt(rowindex);

                    SZV_KORR_4_Grid_update();
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void addBtn_5_Click(object sender, EventArgs e)
        {

            SZV_KORR_5_Edit child = new SZV_KORR_5_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.Monthes = Monthes;
            child.ShowDialog();
            if (child.formData != null)
            {
                FormsSZV_KORR_5_2017_List.Add(child.formData);
                SZV_KORR_5_Grid_update();
            }

        }

        private void editBtn_5_Click(object sender, EventArgs e)
        {
            if (SZV_KORR_5_Grid.RowCount != 0)
            {
                int rowindex = SZV_KORR_5_Grid.CurrentRow.Index;
                FormsSZV_KORR_5_2017 szv5_temp = FormsSZV_KORR_5_2017_List.Skip(rowindex).Take(1).First();


                SZV_KORR_5_Edit child = new SZV_KORR_5_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.formData = szv5_temp;
                child.Monthes = Monthes;
                child.ShowDialog();

                if (child.formData != null)
                {
                    FormsSZV_KORR_5_2017_List.RemoveAt(rowindex);
                    FormsSZV_KORR_5_2017_List.Insert(rowindex, child.formData);

                    SZV_KORR_5_Grid_update();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void delBtn_5_Click(object sender, EventArgs e)
        {
            if (SZV_KORR_5_Grid.RowCount != 0 && SZV_KORR_5_Grid.CurrentRow.Cells[0].Value != null)
            {
                int rowindex = SZV_KORR_5_Grid.CurrentRow.Index;
                FormsSZV_KORR_5_2017_List.RemoveAt(rowindex);

                SZV_KORR_5_Grid_update();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void TypeInfo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (TypeInfo.SelectedItem != null)
            {
                //radGroupBox9.Enabled = TypeInfo.SelectedItem.Tag != "1";
                radPageView1.Pages[1].Enabled = TypeInfo.SelectedItem.Tag != "1";
            }
        }

        private void SZV_KORR_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (allowClose)
            {
                SZV_KORR = null;
                db.Dispose();
            }
            else
            {
                DialogResult dialogResult = RadMessageBox.Show("Вы хотите сохранить изменения перед закрытием формы?", "Сохранение записи!", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button3);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        saveBtn_Click(null, null);
                        break;
                    case DialogResult.No:
                        SZV_KORR = null;
                        db.Dispose();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }

            }            

            Props props = new Props(); //экземпляр класса с настройками
            List<WindowData> windowData = new List<WindowData> { };

            props.setFormParams(this, windowData);
        }









    }
}
