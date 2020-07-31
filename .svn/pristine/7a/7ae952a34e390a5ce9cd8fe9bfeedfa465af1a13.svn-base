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
    public partial class RSW2_2014_3_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool setNull = true;
        public bool autoCalc { get; set; }
        public List<long> numRecList = new List<long> { };
        public short CalcYear { get; set; }
        public short YearT { get; set; }


        public FormsRSW2014_2_3 formData { get; set; }

        public RSW2_2014_3_Edit()
        {
            InitializeComponent();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            setNull = true;
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
                                DateBirth = new DateTime((int)Year.Value, 1, 1)
                            };

                            db.AddToStaff(newStaff);
                            db.SaveChanges();

                        }
                    }
                    catch (Exception ex)
                    {
                        Methods.showAlert("Внимание!", "При сохранение данных о сотруднике произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
                    }

                    formData.NumRec = num;
                    formData.CodeBase = (byte)CodeBaseSpinEditor.Value;
                    formData.InsuranceNumber = snils;
                    formData.LastName = LastName.Text;
                    formData.FirstName = FirstName.Text;
                    formData.MiddleName = MiddleName.Text;
                    formData.ControlNumber = contrNum;
                    formData.Year = short.Parse(Year.Value.ToString());
                    formData.DateBegin = dateBegin.Value;
                    formData.DateEnd = dateEnd.Value;
                    formData.SumOPS_D = decimal.Parse(OPS_D.Text);
                    formData.SumStrah_D = decimal.Parse(Strah_D.Text);
                    formData.SumNakop_D = decimal.Parse(Nakop_D.Text);
                    formData.SumOMS_D = decimal.Parse(OMS_D.Text);
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

            }

        }


        private void DateBegin_Leave(object sender, EventArgs e)
        {
            if (dateBegin.Value.Year != CalcYear)
            {
                dateBegin.Value = new DateTime(CalcYear, dateBegin.Value.Month, dateBegin.Value.Day);
            }
            if (dateEnd.Value.Year != CalcYear)
            {
                dateEnd.Value = new DateTime(CalcYear, dateEnd.Value.Month, dateEnd.Value.Day);
            }

        }

        private void DateEnd_Leave(object sender, EventArgs e)
        {
            if (dateBegin.Value.Year != CalcYear)
            {
                dateBegin.Value = new DateTime(CalcYear, dateBegin.Value.Month, dateBegin.Value.Day);
            }
            if (dateEnd.Value.Year != CalcYear)
            {
                dateEnd.Value = new DateTime(CalcYear, dateEnd.Value.Month, dateEnd.Value.Day);
            }
        }


        private void RSW2_2014_3_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (setNull)
                formData = null;
        }

        private void RSW2_2014_3_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            if (YearT == 2015)
            {
                CodeBaseLabel.Visible = true;
                CodeBaseSpinEditor.Visible = true;
                CodeBaseAbout.Visible = true;
            }


            switch (action)
            {
                case "add":
                    formData = new FormsRSW2014_2_3();
                    dateBegin.Value = DateTime.Parse("01.01." + CalcYear.ToString());
                    dateEnd.Value = DateTime.Parse("31.12." + CalcYear.ToString());
                    break;
                case "edit":
                    NumberSpin.Value = formData.NumRec.Value;
                    Snils.Text = formData.ControlNumber != null ? formData.InsuranceNumber + formData.ControlNumber.Value.ToString().PadLeft(2, '0') : formData.InsuranceNumber;
                    LastName.Text = formData.LastName;
                    FirstName.Text = formData.FirstName;
                    MiddleName.Text = formData.MiddleName;
                    Year.Value = formData.Year;
                    CodeBaseSpinEditor.Value = formData.CodeBase.HasValue ? formData.CodeBase.Value : 1;
                    if (formData.DateBegin.HasValue)
                        dateBegin.Value = formData.DateBegin.Value;
                    if (formData.DateEnd.HasValue)
                        dateEnd.Value = formData.DateEnd.Value;

                    OPS_D.EditValue = formData.SumOPS_D.Value;
                    OMS_D.EditValue = formData.SumOMS_D.Value;
                    Strah_D.EditValue = formData.SumStrah_D.Value;
                    Nakop_D.EditValue = formData.SumNakop_D.Value;
                    break;
            }
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
        }

        private void dateBegin_ValueChanged(object sender, EventArgs e)
        {
            if (dateBegin.Value != dateBegin.NullDate)
                dateBeginMaskedEditBox.Text = dateBegin.Value.ToShortDateString();
            else
                dateBeginMaskedEditBox.Text = dateBeginMaskedEditBox.NullText;

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
        }

        private void dateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (dateEnd.Value != dateEnd.NullDate)
                dateEndMaskedEditBox.Text = dateEnd.Value.ToShortDateString();
            else
                dateEndMaskedEditBox.Text = dateEndMaskedEditBox.NullText;
        }

        private void CodeBaseSpinEditor_ValueChanged(object sender, EventArgs e)
        {
            switch (CodeBaseSpinEditor.Value.ToString())
            {
                case "1":
                    CodeBaseAbout.Text = "в случае доначисления по актам камеральных проверок";
                    break;
                case "2":
                    CodeBaseAbout.Text = "в случае самостоятельного доначисления страховых взносов";
                    break;
                case "3":
                    CodeBaseAbout.Text = "в случае корректировки базы для исчисления страховых взносов";
                    break;
            }
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
