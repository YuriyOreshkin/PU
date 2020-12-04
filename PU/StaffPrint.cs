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
using PU.FormsRSW2014;
using PU.Classes;
using Telerik.WinControls.UI;
using PU.Reports;

namespace PU
{
    public partial class StaffPrint : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public string formParent { get; set; }
        private List<RaschetPeriodContainer> RaschPer { get; set; }

        public List<long> staffList_current { get; set; }
        public List<long> staffList_checked { get; set; }
        public long currentStaffId = 0;

        public StaffPrint()
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

        private void printCurrentStaffListCheckBox_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            radPanel1.Enabled = !printCurrentStaffListCheckBox.Checked;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StaffPrint_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            if (action == "staff")
            {
                this.Text = "Печать списка СОТРУДНИКОВ";
            }
            else if (action == "lgot")
            {
                this.Text = "Печать списка ЛЬГОТНИКОВ";
                printCurrentStaffListCheckBox.Checked = false;
                printCurrentStaffListCheckBox.Enabled = false;
            }

            foreach (var item in db.TypeInfo)
            {
                TypeInfoDDL.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }

            TypeInfoDDL.SelectedIndex = 0;

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


            Year_SelectedIndexChanged();

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();


            // выпад список "календарный год"
            RaschPer = new List<RaschetPeriodContainer> { };
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

            this.TypeInfoDDL.SelectedIndexChanged += (s, с) => TypeInfo_SelectedIndexChanged();

        }

        private void TypeInfo_SelectedIndexChanged()
        {
            if (TypeInfoDDL.SelectedIndex == 0)
            {
                korrGroupBox.Enabled = false;
            }
            else
            {
                korrGroupBox.Enabled = true;
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
            if (!printCurrentStaffListCheckBox.Checked)
                if ((printRangeDDL.SelectedIndex == 2 && (staffList_checked == null || (staffList_checked != null && staffList_checked.Count <= 0))) || (printRangeDDL.SelectedIndex == 1 && currentStaffId == 0))
                {
                    RadMessageBox.Show("Пустой список сотрудников для печати! Необходимо выбрать сотрудников!", "Внимание");
                    return;
                }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<long> razd61ID = new List<long>();
                List<long> staffTempIDList = new List<long>();
                if (printCurrentStaffListCheckBox.Checked)  // если печатаем текущий список сотрудников
                {
                    staffTempIDList = staffList_current;
                }
                else
                {


                    switch (printRangeDDL.SelectedIndex)  // Границы печати
                    {
                        case 0: //все записи
                            staffTempIDList = db.Staff.Where(x => x.InsurerID == Options.InsID).Select(x => x.ID).ToList();
                            break;
                        case 1: //текущая запись
                            staffTempIDList.Add(currentStaffId);
                            break;
                        case 2: //по выделенным записям
                            staffTempIDList = staffList_checked;
                            break;
                    }

                    byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
                    short y = short.Parse(Year.Text);

                    byte qk = 0;
                    short yk = 0;

                    if (TypeInfoDDL.SelectedIndex > 0) // корр\отмн данные
                    {
                        qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
                        yk = short.Parse(YearKorr.Text);
                    }

                    switch (formParent)
                    {
                        case "rsw1":

                            switch (TypeInfoDDL.SelectedIndex)  // тип сведений
                            {
                                case 0:  // исходные сведения
                                    var temp0 = db.FormsRSW2014_1_Razd_6_1.Where(x => staffTempIDList.Contains(x.StaffID) && x.TypeInfoID == 1 && x.Year == y && x.Quarter == q);
                                    staffTempIDList = temp0.Select(x => x.StaffID).ToList();
                                    razd61ID = temp0.Select(x => x.ID).ToList();
                                    break;
                                case 1: // корректирующие
                                    var temp1 = db.FormsRSW2014_1_Razd_6_1.Where(x => staffTempIDList.Contains(x.StaffID) && x.TypeInfoID == 2 && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk);
                                    staffTempIDList = temp1.Select(x => x.StaffID).ToList();
                                    razd61ID = temp1.Select(x => x.ID).ToList();
                                    break;
                                case 2: // отменяющие
                                    var temp2 = db.FormsRSW2014_1_Razd_6_1.Where(x => staffTempIDList.Contains(x.StaffID) && x.TypeInfoID == 3 && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk);
                                    staffTempIDList = temp2.Select(x => x.StaffID).ToList();
                                    razd61ID = temp2.Select(x => x.ID).ToList();
                                    break;
                            }
                            break;
                        case "spw2":

                            switch (TypeInfoDDL.SelectedIndex)  // тип сведений
                            {
                                case 0:  // исходные сведения
                                    staffTempIDList = db.FormsSPW2.Where(x => staffTempIDList.Contains(x.StaffID) && x.TypeInfoID == 1 && x.Year == y && x.Quarter == q).Select(x => x.StaffID).ToList();
                                    break;
                                case 1: // корректирующие
                                    staffTempIDList = db.FormsSPW2.Where(x => staffTempIDList.Contains(x.StaffID) && x.TypeInfoID == 2 && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk).Select(x => x.StaffID).ToList();
                                    break;
                                case 2: // отменяющие
                                    staffTempIDList = db.FormsSPW2.Where(x => staffTempIDList.Contains(x.StaffID) && x.TypeInfoID == 3 && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk).Select(x => x.StaffID).ToList();
                                    break;
                            }
                            break;
                    }

                }

                List<long> stajOsnID = new List<long>();
                if (action == "lgot")
                {
                    //var razd61 = db.FormsRSW2014_1_Razd_6_1.Where(x => staffTempIDList.Contains(x.StaffID));
//                    List<long> razd61ID = new List<long>();


                    var stajOsn = db.StajOsn.Where(x => razd61ID.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).ToList();
                    //     stajOsn = stajOsn.Where(x => x.StajLgot.Any(v => v.OsobUslTrudaID != null || v.UslDosrNaznID != null || (v.IschislStrahStajOsnID != null && (v.IschislStrahStajOsnID == 1 || v.IschislStrahStajOsnID == 2 || v.IschislStrahStajOsnID == 3 || v.IschislStrahStajOsnID == 5)))).ToList();
                    stajOsnID = stajOsn.Select(x => x.ID).ToList();
                    var stajLgot = db.StajLgot.Where(x => stajOsnID.Contains(x.StajOsnID) && (x.OsobUslTrudaID != null || x.UslDosrNaznID != null || (x.IschislStrahStajOsnID != null && (x.IschislStrahStajOsnID == 1 || x.IschislStrahStajOsnID == 2 || x.IschislStrahStajOsnID == 3 || x.IschislStrahStajOsnID == 5)))).Select(x => x.StajOsnID).ToList();
                    stajOsn = stajOsn.Where(x => stajLgot.Contains(x.ID)).ToList();
                    razd61ID = stajOsn.Select(x => x.FormsRSW2014_1_Razd_6_1_ID.Value).ToList();
                    stajOsnID = stajOsn.Select(x => x.ID).ToList();

                    var staffID_T = db.FormsRSW2014_1_Razd_6_1.Where(x => razd61ID.Contains(x.ID)).Select(x => x.StaffID).ToList();

                    staffTempIDList = staffID_T;
                }

                List<Staff> staff = new List<Staff>();
                switch (sortingDDL.SelectedIndex)
                {
                    case 0: //сортировка по ФИО
                        staff = db.Staff.Where(x => staffTempIDList.Contains(x.ID)).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.MiddleName).ToList();
                        break;
                    case 1: //по страх. номеру
                        staff = db.Staff.Where(x => staffTempIDList.Contains(x.ID)).OrderBy(x => x.InsuranceNumber).ToList();
                        break;
                    case 2: //по табелю
                        staff = db.Staff.Where(x => staffTempIDList.Contains(x.ID)).OrderBy(x => x.TabelNumber.Value).ToList();
                        break;
                }

                if (staff.Count() != 0)
                {
                    int i = 0;

                    if (action == "staff")
                    {
                        List<StaffObject> staffList = new List<StaffObject> { };
                        foreach (var item in staff)
                        {
                            i++;
                            string dateb = "";
                            if (item.DateBirth != null)
                            {
                                dateb = item.DateBirth.HasValue ? item.DateBirth.Value.ToShortDateString() : "";
                            }

                            staffList.Add(new StaffObject()
                            {
                                ID = item.ID,
                                Num = i,
                                FIO = item.LastName + " " + item.FirstName + " " + item.MiddleName,
                                SNILS = Utils.ParseSNILS(item.InsuranceNumber, item.ControlNumber),
                                INN = !String.IsNullOrEmpty(item.INN) ? item.INN.PadLeft(12, '0') : " ",
                                TabelNumber = item.TabelNumber,
                                Sex = item.Sex.HasValue ? (item.Sex.Value == 0 ? "М" : "Ж") : "",
                                Dismissed = item.Dismissed.HasValue ? (item.Dismissed.Value == 1 ? "У" : " ") : " ",
                                DateBirth = dateb
                            });

                        }

                        if (staffList.Count() > 0)
                        {
                            ReportMethods.PrintStaff(staffList, this.ThemeName);
                        }
                        else
                        {
                            Messenger.showAlert(AlertType.Info, "Внимание!", "Нет данных для печати!", this.ThemeName);
                        }
                    }
                    else if (action == "lgot")
                    {
                        List<StaffLgotObject> staffLgotList = new List<StaffLgotObject> { };
                        var stajOsn = db.StajOsn.Where(x => stajOsnID.Contains(x.ID)).ToList();
                        var razd61ID_ = stajOsn.Select(x => x.FormsRSW2014_1_Razd_6_1_ID.Value).ToList();
                        var razd61List = db.FormsRSW2014_1_Razd_6_1.Where(x => razd61ID_.Contains(x.ID)).ToList();

                        foreach (var item in staff)
                        {
                            i++;
                            var razd61 = razd61List.First(x => x.StaffID == item.ID);

                            List<StajOsn_Lgot> stajOsnL = new List<StajOsn_Lgot>();
                            var t = stajOsn.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == razd61.ID).ToList();
                            foreach (var item_ in t)
                            {
                                StajLgot sl = item_.StajLgot.First(x => x.OsobUslTrudaID != null || x.UslDosrNaznID != null || (x.IschislStrahStajOsnID != null && (x.IschislStrahStajOsnID == 1 || x.IschislStrahStajOsnID == 2 || x.IschislStrahStajOsnID == 3 || x.IschislStrahStajOsnID == 5)));

                                stajOsnL.Add(new StajOsn_Lgot
                                {
                                    DateBegin = item_.DateBegin.HasValue ? item_.DateBegin.Value : DateTime.Now,
                                    DateEnd = item_.DateEnd.HasValue ? item_.DateEnd.Value : DateTime.Now,
                                    Lgot = sl != null ? String.Format("{0},{1},[{8}],{2},[{3}][{4}][{5}] [ {6} ] {7}", sl.OsobUslTrudaID.HasValue ? sl.OsobUslTruda.Code : "", sl.IschislStrahStajOsnID.HasValue ? sl.IschislStrahStajOsn.Code : "", sl.UslDosrNaznID.HasValue ? sl.UslDosrNazn.Code : "", sl.UslDosrNazn1Param.HasValue ? sl.UslDosrNazn1Param.Value : 0, sl.UslDosrNazn2Param.HasValue ? sl.UslDosrNazn2Param.Value : 0, sl.UslDosrNazn3Param.HasValue ? sl.UslDosrNazn3Param.Value : 0, sl.KodVred_2 != null ? sl.KodVred_2.Code : "", sl.DolgnID.HasValue ? sl.Dolgn.Name : "", sl.IschislStrahStajDopID.HasValue ? sl.IschislStrahStajDop.Code : "") : ""
                                });
                            }

                            //         stajOsn = stajOsn.Where(x => x.StajLgot.Any(v => v.OsobUslTrudaID != null || v.UslDosrNaznID != null || (v.IschislStrahStajOsnID != null && (v.IschislStrahStajOsnID == 1 || v.IschislStrahStajOsnID == 2 || v.IschislStrahStajOsnID == 3 || v.IschislStrahStajOsnID == 5)))).ToList();


                            staffLgotList.Add(new StaffLgotObject()
                            {
                                Num = i,
                                FIO = item.LastName + " " + item.FirstName + " " + item.MiddleName,
                                SNILS = Utils.ParseSNILS(item.InsuranceNumber, item.ControlNumber),
                                stajOsn = stajOsnL,
                                razd67 = razd61.FormsRSW2014_1_Razd_6_7.Any() ? razd61.FormsRSW2014_1_Razd_6_7.Select(x => new FormsRSW2014_1_Razd_6_7_Lgot
                                {
                                    Code = x.SpecOcenkaUslTrudaID != null ? x.SpecOcenkaUslTruda.Code : "",
                                    s_0_0 = x.s_0_0.HasValue ? x.s_0_0.Value : 0,
                                    s_0_1 = x.s_0_1.HasValue ? x.s_0_1.Value : 0,
                                    s_1_0 = x.s_1_0.HasValue ? x.s_1_0.Value : 0,
                                    s_1_1 = x.s_1_1.HasValue ? x.s_1_1.Value : 0,
                                    s_2_0 = x.s_2_0.HasValue ? x.s_2_0.Value : 0,
                                    s_2_1 = x.s_2_1.HasValue ? x.s_2_1.Value : 0,
                                    s_3_0 = x.s_3_0.HasValue ? x.s_3_0.Value : 0,
                                    s_3_1 = x.s_3_1.HasValue ? x.s_3_1.Value : 0
                                }).ToList() : null
                            });
                        }

                        if (staffLgotList.Count() > 0)
                        {
                            ReportMethods.PrintStaffLgot(staffLgotList, this.ThemeName);
                        }
                        else
                        {
                            Messenger.showAlert(AlertType.Info, "Внимание!", "Нет данных для печати!", this.ThemeName);
                        }

                    }
                }
                else
                {
                    Messenger.showAlert(AlertType.Info, "Внимание!", "Нет данных для печати!", this.ThemeName);
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }

        }

        private void StaffPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            db.Dispose();
        }


    }
}
