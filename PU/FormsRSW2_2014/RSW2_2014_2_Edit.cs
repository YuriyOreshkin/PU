using PU.Models;
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
using Telerik.WinControls.UI;

namespace PU.FormsRSW2_2014
{
    public partial class RSW2_2014_2_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool setNull = true;
        public bool autoCalc { get; set; }
        public List<long> numRecList = new List<long> { };
        public short CalcYear {get; set;}


        public FormsRSW2014_2_2 formData { get; set; }

        public RSW2_2014_2_Edit()
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

        private void radButton1_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
        }

        private void RSW2_2014_2_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (setNull)
                formData = null;
        }

        private void RSW2_2014_2_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            OPS.Enabled = !autoCalc;
            OMS.Enabled = !autoCalc;

            switch (action)
            {
                case "add":
                    formData = new FormsRSW2014_2_2();
                    dateBegin.Value = DateTime.Parse("01.01." + CalcYear.ToString());
                    dateEnd.Value = DateTime.Parse("31.12." + CalcYear.ToString());
                    CalcFee();
                    break;
                case "edit":
                    NumberSpin.Value = formData.NumRec.Value;
                    Snils.Text = formData.ControlNumber != null ? formData.InsuranceNumber + formData.ControlNumber.Value.ToString().PadLeft(2, '0') : formData.InsuranceNumber;
                    LastName.Text = formData.LastName;
                    FirstName.Text = formData.FirstName;
                    MiddleName.Text = formData.MiddleName;
                    Year.Value = formData.Year;
                    if (formData.DateBegin.HasValue)
                        dateBegin.Value = formData.DateBegin.Value;
                    if (formData.DateEnd.HasValue)
                        dateEnd.Value = formData.DateEnd.Value;

                    OPS.EditValue = formData.SumOPS.Value;
                    OMS.EditValue = formData.SumOMS.Value;
                    break;
            }

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            int num = int.Parse(NumberSpin.Value.ToString());
            if (dateBegin.Value != dateBegin.NullDate && dateEnd.Value != dateEnd.NullDate)
            {
                if ((action == "add" && !numRecList.Contains(num)) || (action == "edit" && (num == formData.NumRec || (num != formData.NumRec && !numRecList.Contains(num)))))
                {
                    string snils = "";
                    byte contrNum = 0;

                    if (!Snils.Value.ToString().Contains("_"))
                    {
                        if (Utils.ValidateSSN(Snils.Value.ToString()))
                        {
                            var s2 = (this.Snils.Value.ToString()).Split(' ');

                            if (byte.TryParse(s2[1], out contrNum))
                            {
                                var s3 = s2[0].Split('-');
                                foreach (var item in s3)
                                {
                                    if (!item.Contains("_"))
                                        snils += item;
                                    else
                                    {
                                        snils = "";
                                        contrNum = 0;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    try
                    {
                        if (saveNewStaffCheckBox.Checked && !db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils)) // если отмечено добавить и такого сотрудника еще нет то добавляем его
                        {
                            Staff newStaff = new Staff
                            {
                                InsurerID = Options.InsID,
                                InsuranceNumber = snils,
                                ControlNumber = contrNum,
                                LastName = LastName.Text,
                                FirstName = FirstName.Text,
                                MiddleName = MiddleName.Text,
                                DateBirth = new DateTime((int)Year.Value,1,1)
                            };

                            db.Staff.Add(newStaff);
                            db.SaveChanges();

                        }
                    }
                    catch (Exception ex)
                    {
                        Methods.showAlert("Внимание!", "При сохранение данных о сотруднике произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
                    }

                    formData.NumRec = num;
                    formData.InsuranceNumber = snils;
                    formData.LastName = LastName.Text;
                    formData.FirstName = FirstName.Text;
                    formData.MiddleName = MiddleName.Text;
                    formData.ControlNumber = contrNum;
                    formData.Year = short.Parse(Year.Value.ToString());
                    formData.DateBegin = dateBegin.Value;
                    formData.DateEnd = dateEnd.Value;
                    formData.SumOPS = decimal.Parse(OPS.Text);
                    formData.SumOMS = decimal.Parse(OMS.Text);
                    setNull = false;
                    this.Close();

                }
                else
                {
                    RadMessageBox.Show(this, "Запись с порядковым номером \"" + NumberSpin.Value.ToString() + "\" уже существует!", "Внимание!");
                }
            }
            else
            {
                RadMessageBox.Show(this, "Даты периодов в КФХ должны быть заполнены!", "Внимание!");
            }


        }

        private void Snils_Leave(object sender, EventArgs e)
        {

            var ssn = Snils.Value.ToString().Split(' ');

            if (!ssn[0].Contains("_"))
            {
                string contrNum = Utils.GetControlSumSSN(ssn[0]);

                Snils.Value = ssn[0] + " " + contrNum;

                string snils = ssn[0].Replace("-", "");

                if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils))
                {
                    Staff staff = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils);
                    FirstName.Text = staff.FirstName;
                    LastName.Text = staff.LastName;
                    MiddleName.Text = staff.MiddleName;
                    if (staff.DateBirth.HasValue)
                        Year.Value = staff.DateBirth.Value.Year;

                    saveNewStaffCheckBox.Visible = false;
                    saveNewStaffCheckBox.Checked = false;

                }
                else
                {
                    saveNewStaffCheckBox.Visible = true;
                    saveNewStaffCheckBox.Checked = true;
                }
            }
            else
            {
                if (Snils.Value.ToString() != "___-___-___ __")
                {
                    Snils.Text = "___-___-___ __";
                }
                saveNewStaffCheckBox.Visible = false;
                saveNewStaffCheckBox.Checked = false;

            }

        }

        private void CalcFee()
        {
            if (autoCalc)
            if (dateBegin.Value != dateBegin.NullDate && dateEnd.Value != dateEnd.NullDate)
            {

                DateTime regDate = dateBegin.Value;
                DateTime expDate = dateEnd.Value;

                if (regDate.Year != CalcYear && expDate.Year == CalcYear)
                    regDate = new DateTime(CalcYear, 1, 1);

                if (regDate.Year == CalcYear && expDate.Year > CalcYear)
                    expDate = new DateTime(CalcYear, 12, 31);

                MROT mrot = db.MROT.FirstOrDefault(x => x.Year == CalcYear);
                TariffPlat tariffPlat = db.PlatCategory.FirstOrDefault(x => x.Code == "ФЛ" && x.PlatCategoryRaschPerID == 4).TariffPlat.FirstOrDefault(x => x.Year == CalcYear);

                //            OMSOPSTariff oMSOPSTariff = dataManager.OPSOMSTariffRepository.GetOMSOPSTariffByYear(CalcYear);
                //            OPSAgeTariff oPSAgeTariff = dataManager.OPSAgeTariffRepository.GetOPSAgeTariffByYear(CalcYear);

                decimal svTFOMS = 0;
                decimal svFFOMS = 0;
                decimal svOPSAgeStrah = 0;
                decimal svOPSAgeNakop = 0;

                decimal ssgTFOMS = 0;
                decimal ssgFFOMS = 0;
                decimal ssgOPSAgeStrah = 0;
                decimal ssgOPSAgeNakop = 0;

                //if (mrot != null && tariffPlat != null)
                //{
                //    ssgTFOMS = mrot.Mrot1 * 12 * (tariffPlat.TFOMS_Percent.HasValue ? (tariffPlat.TFOMS_Percent.Value / 100) : 0);
                //    ssgFFOMS = mrot.Mrot1 * 12 * (tariffPlat.FFOMS_Percent.HasValue ? (tariffPlat.FFOMS_Percent.Value / 100) : 0);

                //    if (Year.Value <= 1966)
                //    {
                //        ssgOPSAgeStrah = mrot.Mrot1 * 12 * (tariffPlat.StrahPercant1966.HasValue ? (tariffPlat.StrahPercant1966.Value / 100) : 0);

                //        ssgOPSAgeNakop = 0;
                //    }
                //    if (Year.Value > 1966)
                //    {
                //        ssgOPSAgeNakop = mrot.Mrot1 * 12 * (tariffPlat.NakopPercant.HasValue ? (tariffPlat.NakopPercant.Value / 100) : 0);
                //        ssgOPSAgeStrah = mrot.Mrot1 * 12 * (tariffPlat.StrahPercent1967.HasValue ? (tariffPlat.StrahPercent1967.Value / 100) : 0);
                //    }
                //}
                //ssgOPSAgeStrah = ssgOPSAgeStrah + ssgOPSAgeNakop;


                //int fullMonthCount = 0;
                ////кол-во полных месяцев
                //int notFullRegDayCount = 0;
                ////кол-во дней в нач. дате неполного месяца
                //int notFullExpDayCount = 0;
                ////кол-во дней в кон. дате неполного месяца
                ////если однаковые месяцы
                //if (regDate.Month == expDate.Month)
                //{
                //    //если один полный месяц
                //    if (regDate.Day == 1 && expDate.Day == Utils.GetDaysInMonth(expDate.Month, expDate.Year))
                //    {
                //        svTFOMS = ssgTFOMS / 12;
                //        svFFOMS = ssgFFOMS / 12;
                //        svOPSAgeNakop = ssgOPSAgeNakop / 12;
                //        svOPSAgeStrah = ssgOPSAgeStrah / 12;
                //        //если в середине месяца
                //    }
                //    else
                //    {
                //        svTFOMS = (ssgTFOMS * (expDate.Day - regDate.Day + 1)) / (12 * Utils.GetDaysInMonth(regDate.Month, regDate.Year));
                //        svFFOMS = (ssgFFOMS * (expDate.Day - regDate.Day + 1)) / (12 * Utils.GetDaysInMonth(regDate.Month, regDate.Year));
                //        svOPSAgeStrah = (ssgOPSAgeStrah * (expDate.Day - regDate.Day + 1)) / (12 * Utils.GetDaysInMonth(regDate.Month, regDate.Year));
                //        svOPSAgeNakop = (ssgOPSAgeNakop * (expDate.Day - regDate.Day + 1)) / (12 * Utils.GetDaysInMonth(regDate.Month, regDate.Year));
                //    }
                //    //разные месяцы
                //}
                //else
                //{
                //    //несколько полных месяцев
                //    if (regDate.Day == 1 && expDate.Day == Utils.GetDaysInMonth(expDate.Month, expDate.Year))
                //    {
                //        svTFOMS = (ssgTFOMS / 12) * (expDate.Month - regDate.Month + 1);
                //        svFFOMS = (ssgFFOMS / 12) * (expDate.Month - regDate.Month + 1);
                //        svOPSAgeStrah = (ssgOPSAgeStrah / 12) * (expDate.Month - regDate.Month + 1);
                //        svOPSAgeNakop = (ssgOPSAgeNakop / 12) * (expDate.Month - regDate.Month + 1);
                //    }
                //    else
                //    {
                //        notFullRegDayCount = Utils.GetDaysInMonth(regDate.Month, regDate.Year) - regDate.Day + 1;
                //        notFullExpDayCount = expDate.Day;
                //        fullMonthCount = expDate.Month - regDate.Month - 1;

                //        svFFOMS = (ssgFFOMS * notFullRegDayCount) / (12 * Utils.GetDaysInMonth(regDate.Month, regDate.Year)) + (fullMonthCount * ssgFFOMS / 12) + (ssgFFOMS * notFullExpDayCount) / (12 * Utils.GetDaysInMonth(expDate.Month, expDate.Year));


                //        svTFOMS = (ssgTFOMS * notFullRegDayCount) / (12 * Utils.GetDaysInMonth(regDate.Month, regDate.Year)) + (fullMonthCount * ssgTFOMS / 12) + (ssgTFOMS * notFullExpDayCount) / (12 * Utils.GetDaysInMonth(expDate.Month, expDate.Year));
                //        svOPSAgeStrah = (ssgOPSAgeStrah * notFullRegDayCount) / (12 * Utils.GetDaysInMonth(regDate.Month, regDate.Year)) + (fullMonthCount * ssgOPSAgeStrah / 12) + (ssgOPSAgeStrah * notFullExpDayCount) / (12 * Utils.GetDaysInMonth(expDate.Month, expDate.Year));

                //        svOPSAgeNakop = (ssgOPSAgeNakop * notFullRegDayCount) / (12 * Utils.GetDaysInMonth(regDate.Month, regDate.Year)) + (fullMonthCount * ssgOPSAgeNakop / 12) + (ssgOPSAgeNakop * notFullExpDayCount) / (12 * Utils.GetDaysInMonth(expDate.Month, expDate.Year));
                //    }
                //}


                if (mrot != null && tariffPlat != null)
                {
                    ssgTFOMS = mrot.Mrot1 * (tariffPlat.TFOMS_Percent.HasValue ? (tariffPlat.TFOMS_Percent.Value / 100) : 0);
                    ssgFFOMS = mrot.Mrot1 * (tariffPlat.FFOMS_Percent.HasValue ? (tariffPlat.FFOMS_Percent.Value / 100) : 0);

                    if (Year.Value <= 1966)
                    {
                        ssgOPSAgeStrah = mrot.Mrot1 * (tariffPlat.StrahPercant1966.HasValue ? (tariffPlat.StrahPercant1966.Value / 100) : 0);

                        ssgOPSAgeNakop = 0;
                    }
                    if (Year.Value > 1966)
                    {
                        ssgOPSAgeNakop = mrot.Mrot1 * (tariffPlat.NakopPercant.HasValue ? (tariffPlat.NakopPercant.Value / 100) : 0);
                        ssgOPSAgeStrah = mrot.Mrot1 * (tariffPlat.StrahPercent1967.HasValue ? (tariffPlat.StrahPercent1967.Value / 100) : 0);
                    }
                }
                ssgOPSAgeStrah = ssgOPSAgeStrah + ssgOPSAgeNakop;


                int fullMonthCount = 0;
                //кол-во полных месяцев
                int notFullRegDayCount = 0;
                //кол-во дней в нач. дате неполного месяца
                int notFullExpDayCount = 0;
                //кол-во дней в кон. дате неполного месяца
                //если однаковые месяцы
                if (regDate.Month == expDate.Month)
                {
                    //если один полный месяц
                    if (regDate.Day == 1 && expDate.Day == Utils.GetDaysInMonth(expDate.Month, expDate.Year))
                    {
                        svTFOMS = ssgTFOMS;
                        svFFOMS = ssgFFOMS;
                        svOPSAgeNakop = ssgOPSAgeNakop;
                        svOPSAgeStrah = ssgOPSAgeStrah;
                        //если в середине месяца
                    }
                    else
                    {
                        svTFOMS = (ssgTFOMS * (expDate.Day - regDate.Day + 1)) / (Utils.GetDaysInMonth(regDate.Month, regDate.Year));
                        svFFOMS = (ssgFFOMS * (expDate.Day - regDate.Day + 1)) / (Utils.GetDaysInMonth(regDate.Month, regDate.Year));
                        svOPSAgeStrah = (ssgOPSAgeStrah * (expDate.Day - regDate.Day + 1)) / (Utils.GetDaysInMonth(regDate.Month, regDate.Year));
                        svOPSAgeNakop = (ssgOPSAgeNakop * (expDate.Day - regDate.Day + 1)) / (Utils.GetDaysInMonth(regDate.Month, regDate.Year));
                    }
                    //разные месяцы
                }
                else
                {
                    //несколько полных месяцев
                    if (regDate.Day == 1 && expDate.Day == Utils.GetDaysInMonth(expDate.Month, expDate.Year))
                    {
                        svTFOMS = (ssgTFOMS) * (expDate.Month - regDate.Month + 1);
                        svFFOMS = (ssgFFOMS) * (expDate.Month - regDate.Month + 1);
                        svOPSAgeStrah = (ssgOPSAgeStrah) * (expDate.Month - regDate.Month + 1);
                        svOPSAgeNakop = (ssgOPSAgeNakop) * (expDate.Month - regDate.Month + 1);
                    }
                    else
                    {
                        notFullRegDayCount = Utils.GetDaysInMonth(regDate.Month, regDate.Year) - regDate.Day + 1;
                        notFullExpDayCount = expDate.Day;
                        fullMonthCount = expDate.Month - regDate.Month - 1;

                        svFFOMS = (ssgFFOMS * notFullRegDayCount) / (Utils.GetDaysInMonth(regDate.Month, regDate.Year)) + (fullMonthCount * ssgFFOMS) + (ssgFFOMS * notFullExpDayCount) / (Utils.GetDaysInMonth(expDate.Month, expDate.Year));

                        svTFOMS = (ssgTFOMS * notFullRegDayCount) / (Utils.GetDaysInMonth(regDate.Month, regDate.Year)) + (fullMonthCount * ssgTFOMS) + (ssgTFOMS * notFullExpDayCount) / (Utils.GetDaysInMonth(expDate.Month, expDate.Year));
                        svOPSAgeStrah = (ssgOPSAgeStrah * notFullRegDayCount) / (Utils.GetDaysInMonth(regDate.Month, regDate.Year)) + (fullMonthCount * ssgOPSAgeStrah) + (ssgOPSAgeStrah * notFullExpDayCount) / (Utils.GetDaysInMonth(expDate.Month, expDate.Year));

                        svOPSAgeNakop = (ssgOPSAgeNakop * notFullRegDayCount) / (Utils.GetDaysInMonth(regDate.Month, regDate.Year)) + (fullMonthCount * ssgOPSAgeNakop) + (ssgOPSAgeNakop * notFullExpDayCount) / (Utils.GetDaysInMonth(expDate.Month, expDate.Year));
                    }
                }


                OPS.Text = Math.Round(svOPSAgeStrah, 2, MidpointRounding.AwayFromZero).ToString();
                OMS.Text = Math.Round(svFFOMS, 2, MidpointRounding.AwayFromZero).ToString();

            }
        }


        private void Year_ValueChanged(object sender, EventArgs e)
        {
            CalcFee();
        }

        private void dateBeginMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (dateBeginMaskedEditBox.Text != dateBeginMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(dateBeginMaskedEditBox.Text, out date))
                {
                    dateBegin.Value = date;
                }
                else
                {
                    dateBegin.Value = dateBegin.NullDate;
                }
            }
            else
            {
                dateBegin.Value = dateBegin.NullDate;
            }
            CalcFee();

        }

        private void dateBegin_ValueChanged(object sender, EventArgs e)
        {
            if (dateBegin.Value != dateBegin.NullDate)
                dateBeginMaskedEditBox.Text = dateBegin.Value.ToShortDateString();
            else
                dateBeginMaskedEditBox.Text = dateBeginMaskedEditBox.NullText;

            CalcFee();
        }

        private void dateEndMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (dateEndMaskedEditBox.Text != dateEndMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(dateEndMaskedEditBox.Text, out date))
                {
                    dateEnd.Value = date;
                }
                else
                {
                    dateEnd.Value = dateEnd.NullDate;
                }
            }
            else
            {
                dateEnd.Value = dateEnd.NullDate;
            }
            CalcFee();

        }

        private void dateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (dateEnd.Value != dateEnd.NullDate)
                dateEndMaskedEditBox.Text = dateEnd.Value.ToShortDateString();
            else
                dateEndMaskedEditBox.Text = dateEndMaskedEditBox.NullText;

            CalcFee();
        }

        private void Snils_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                LastName.Focus();
            }
        }

        private void selectStaffBtn_Click(object sender, EventArgs e)
        {
            Staff staff = null;

            var ssn = Snils.Value.ToString().Split(' ');

            if (!ssn[0].Contains("_"))
            {
                string snils = ssn[0].Replace("-", "");

                staff = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils);
            }

            StaffFrm child = new StaffFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.InsID = Options.InsID;
            child.action = "selection";
            child.StaffID = staff == null ? 0 : staff.ID; ;
            child.db = db;
            child.ShowDialog();
            long id = child.StaffID;
            if (db.Staff.Any(x => x.ID == id))
            {
                staff = db.Staff.FirstOrDefault(x => x.ID == id);
                LastName.Text = staff.LastName;
                FirstName.Text = staff.FirstName;
                MiddleName.Text = staff.MiddleName;
                Snils.Text = staff.ControlNumber != null ? (staff.InsuranceNumber + staff.ControlNumber.Value.ToString().PadLeft(2, '0')) : staff.InsuranceNumber;

                if (staff.DateBirth.HasValue)
                {
                    Year.Value = staff.DateBirth.Value.Year;
                }

                saveNewStaffCheckBox.Checked = false;
                saveNewStaffCheckBox.Visible = false;
            }

        }


    }
}
