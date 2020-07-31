using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using PU.Models;
using PU.Classes;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using System.Globalization;

namespace PU.FormsPredPens
{
    public partial class PredPensZapros_EditStaff : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action;
        private List<ErrList> errMessBox = new List<ErrList>();
        private bool cleanData = true;
        public FormsPredPens_Zapros_Staff ZaprosStaffData = new FormsPredPens_Zapros_Staff();
        public FormsPredPens_Zapros ZaprosData { get; set; }
        public long ZaprosStaffID = 0;

        public PredPensZapros_EditStaff()
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
                closeBtn_Click(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void PredPensZapros_EditStaff_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            if (ZaprosData != null)
            {
                szvmAttr.Text = "от " + ZaprosData.Date.ToShortDateString() + " № " + ZaprosData.Number.Trim();
            }

            ZaprosStaffData = new FormsPredPens_Zapros_Staff();

        }

        private void setReadonly(bool flag)
        {
            TabelNumber.ReadOnly = flag;
            INN.ReadOnly = flag;
            LastName.ReadOnly = flag;
            FirstName.ReadOnly = flag;
            MiddleName.ReadOnly = flag;
            DateBirth_MaskedEditBox.ReadOnly = flag;
            DateBirth_.Enabled = !flag;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            ZaprosStaffData = null;
            this.Close();
        }

        private void SZV_M_2016_EditStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cleanData)
                ZaprosStaffData = null;
        }

        private void INN_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(INN.Text))
                INN.Text = INN.Text.PadLeft(12, '0');
        }

        private void INN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void SNILS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                TabelNumber.Focus();
            }
        }

        private void SNILS_Leave(object sender, EventArgs e)
        {
            var ssn = SNILS.Value.ToString().Split(' ');

            if (!ssn[0].Contains("_"))
            {
                string contrNum = Utils.GetControlSumSSN(ssn[0]);

                SNILS.Value = ssn[0] + " " + contrNum;

                string snils = ssn[0].Replace("-", "");

                if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils))
                {
                    Staff staff = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils);
                    FirstName.Text = staff.FirstName;
                    LastName.Text = staff.LastName;
                    MiddleName.Text = staff.MiddleName;

                    if (staff.DateBirth != null && staff.DateBirth.HasValue)
                        DateBirth_.Value = staff.DateBirth.Value;

                    newStaffLabel.Visible = false;
                    setReadonly(true);
                }
                else
                {
                    newStaffLabel.Visible = true;
                    setReadonly(false);
                }
            }
            else
            {
                newStaffLabel.Visible = false;
                setReadonly(false);
                if (SNILS.Value.ToString() != "___-___-___ __")
                {
                    SNILS.Text = "___-___-___ __";
                }

            }
        }

        private void selectStaffBtn_Click(object sender, EventArgs e)
        {
            Staff staff = null;

            var ssn = SNILS.Value.ToString().Split(' ');

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
                setReadonly(false);
                staff = db.Staff.FirstOrDefault(x => x.ID == id);
                LastName.Text = staff.LastName;
                FirstName.Text = staff.FirstName;
                MiddleName.Text = staff.MiddleName;
                SNILS.Text = staff.ControlNumber != null ? (staff.InsuranceNumber + staff.ControlNumber.Value.ToString().PadLeft(2, '0')) : staff.InsuranceNumber;

                if (staff.DateBirth != null && staff.DateBirth.HasValue)
                    DateBirth_.Value = staff.DateBirth.Value;

                newStaffLabel.Visible = false;
                setReadonly(true);
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                if (action == "add")
                {
                    string snils = "";
                    byte contrNum = 0;

                    if (!SNILS.Value.ToString().Contains("_"))
                    {
                        if (Utils.ValidateSSN(SNILS.Value.ToString()))
                        {
                            var ssn = Utils.ParseSNILS_XML(SNILS.Text, true);

                            snils = ssn.num;
                            if (ssn.contrNum.HasValue)
                                contrNum = ssn.contrNum.Value;
                        }
                    }

                    try
                    {
                        if (newStaffLabel.Visible && !db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils)) // если отмечено добавить и такого сотрудника еще нет то добавляем его
                        {
                            Staff newStaff = new Staff
                            {
                                InsurerID = Options.InsID,
                                InsuranceNumber = snils,
                                ControlNumber = contrNum,
                                LastName = LastName.Text,
                                FirstName = FirstName.Text,
                                MiddleName = MiddleName.Text,
                                INN = INN.Text
                            };

                            if (!String.IsNullOrEmpty(TabelNumber.Text) && long.Parse(TabelNumber.Text) != 0)
                                newStaff.TabelNumber = long.Parse(TabelNumber.Text);
                            else
                                newStaff.TabelNumber = null;

                            if (!String.IsNullOrEmpty(DateBirth_.Text))
                            {
                                newStaff.DateBirth = DateBirth_.Value;
                            }
                            else
                            {
                                newStaff.DateBirth = null;
                            }

                            db.Staff.Add(newStaff);
                            db.SaveChanges();

                        }
                    }
                    catch (Exception ex)
                    {
                        Methods.showAlert("Внимание!", "При сохранение данных о сотруднике произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
                    }
                }


                switch (action)
                {
                    case "add":
                        var ssn = Utils.ParseSNILS_XML(SNILS.Text, false);

                        var snils = ssn.num;

                        if (db.Staff.Any(x => x.InsuranceNumber == snils && x.InsurerID == Options.InsID))
                        {
                            var staffID = db.Staff.FirstOrDefault(x => x.InsuranceNumber == snils && x.InsurerID == Options.InsID).ID;

                            FormsPredPens_Zapros_Staff zaprostaffNew = new FormsPredPens_Zapros_Staff
                            {
                                FormsPredPens_ZaprosID = ZaprosData.ID,
                                StaffID = staffID,
                            };

                            db.FormsPredPens_Zapros_Staff.Add(zaprostaffNew);
                            try
                            {
                                db.SaveChanges();
                                cleanData = false;
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных в базу данных произошла ошибка! Код ошибки: " + ex.InnerException.Message);
                            }
                        }
                        else
                            RadMessageBox.Show(this, "Не удалось загрузить данные по сотруднику из базы данных!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);

                        break;
                }

            }
            else
            {
                RadMessageBox.Show(this, errMessBox[0].name, "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private bool validation()
        {
            bool result = true;
            errMessBox.Clear();

            if (SNILS.Value.ToString() == "___-___-___ __")
                errMessBox.Add(new ErrList { name = "Поле СНИЛС обязательно к заполнению!", control = "SNILS" });
            if (String.IsNullOrEmpty(LastName.Text))
                errMessBox.Add(new ErrList { name = "Поле Фамилия обязательно к заполнению!", control = "LastName" });
            if (String.IsNullOrEmpty(FirstName.Text))
                errMessBox.Add(new ErrList { name = "Поле Имя обязательно к заполнению!", control = "FirstName" });
            if (String.IsNullOrEmpty(DateBirth_.Text))
                errMessBox.Add(new ErrList { name = "Поле Дата Рождения обязательно к заполнению!", control = "DateBirth_MaskedEditBox" });


            if (action == "add")
            {
                if (!SNILS.Value.ToString().Contains("_"))
                {
                    if (Utils.ValidateSSN(SNILS.Value.ToString()))
                    {
                        var ssn = Utils.ParseSNILS_XML(SNILS.Text, false);

                        var snils = ssn.num;

                        if (db.Staff.Any(x => x.InsuranceNumber == snils && x.InsurerID == Options.InsID))
                        {
                            var staffID = db.Staff.FirstOrDefault(x => x.InsuranceNumber == snils && x.InsurerID == Options.InsID).ID;
                            if (db.FormsPredPens_Zapros_Staff.Any(x => x.FormsPredPens_ZaprosID == ZaprosData.ID && x.StaffID == staffID))
                            {
                                errMessBox.Add(new ErrList { name = "Данные по этому сотруднику уже добавлены в этот Запрос", control = "SNILS" });
                            }

                        }
                    }
                    else
                    {
                        errMessBox.Add(new ErrList { name = "СНИЛС не прошел проверку!", control = "SNILS" });
                    }
                }
            }


            if (errMessBox.Count > 0)
                result = false;

            return result;
        }

        private void DateBirth_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateBirth_MaskedEditBox.Text != DateBirth_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateBirth_MaskedEditBox.Text, out date))
                {
                    DateBirth_.Value = date;
                }
                else
                {
                    DateBirth_.Value = DateBirth_.NullDate;
                }
            }
            else
            {
                DateBirth_.Value = DateBirth_.NullDate;
            }
        }

        private void DateBirth__ValueChanged(object sender, EventArgs e)
        {
            if (DateBirth_.Value != DateBirth_.NullDate)
                DateBirth_MaskedEditBox.Text = DateBirth_.Value.ToShortDateString();
            else
                DateBirth_MaskedEditBox.Text = DateBirth_MaskedEditBox.NullText;
        }


    }
}
