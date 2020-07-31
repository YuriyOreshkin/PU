using PU.FormsRSW2014;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using PU.Models;
using PU.Classes;
using Telerik.WinControls.UI;

namespace PU
{
    public partial class StaffAnketaEdit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action;
        public Staff staff { get; set; }
        public long DepID = 0;// ID подразделения
        private bool cleanData = true;
        private List<ErrList> errMessBox = new List<ErrList>();

        public StaffAnketaEdit()
        {
            InitializeComponent();
        }

        public static StaffAnketaEdit SelfRef
        {
            get;
            set;
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    radButton3.Enabled = false;
                    radButton1.Enabled = false;
                    dateWorkGrid.ReadOnly = true;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    break;
            }
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
                radButton2_Click(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void DateOs_CheckBox_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            DayOs_SpinEditor.Enabled = DateOs_CheckBox.Checked;
            DayOs_TextBox.Enabled = DateOs_CheckBox.Checked;
            MonthOs_SpinEditor.Enabled = DateOs_CheckBox.Checked;
            MonthOs_TextBox.Enabled = DateOs_CheckBox.Checked;
            YearOs_SpinEditor.Enabled = DateOs_CheckBox.Checked;
            YearOs_TextBox.Enabled = DateOs_CheckBox.Checked;

            DateBirth_.Enabled = !DateOs_CheckBox.Checked;
            DateBirth_MaskedEditBox.Enabled = !DateOs_CheckBox.Checked;
        }

        private void DateUnderwrite_ValueChanged(object sender, EventArgs e)
        {
            if (DateUnderwrite.Value != DateUnderwrite.NullDate)
                DateUnderwriteMaskedEditBox.Text = DateUnderwrite.Value.ToShortDateString();
            else
                DateUnderwriteMaskedEditBox.Text = DateUnderwriteMaskedEditBox.NullText;
        }

        private void DateUnderwriteMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateUnderwriteMaskedEditBox.Text != DateUnderwriteMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateUnderwriteMaskedEditBox.Text, out date))
                {
                    DateUnderwrite.Value = date;
                }
                else
                {
                    DateUnderwrite.Value = DateUnderwrite.NullDate;
                }
            }
            else
            {
                DateUnderwrite.Value = DateUnderwrite.NullDate;
            }
        }

        private void Reg_Addr_Clear_btn_Click(object sender, EventArgs e)
        {
            Reg_Addr.Clear();
        }

        private void Fakt_Addr_Clear_btn_Click(object sender, EventArgs e)
        {
            Fakt_Addr.Clear();
        }

        private void Reg_Addr_Copy_to_Fakt_btn_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Reg_Addr.Text))
                Fakt_Addr.Text = Reg_Addr.Text;
        }

        private void Fakt_Addr_Copy_to_Reg_btn_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Fakt_Addr.Text))
                Reg_Addr.Text = Fakt_Addr.Text;
        }

        private void ConfirmDocTypeBtnClear_Click(object sender, EventArgs e)
        {
            this.ConfirmDocType.ResetText();
        }

        private void ConfirmDocTypeBtn_Click(object sender, EventArgs e)
        {
            DocTypes child = new DocTypes();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            if (ConfirmDocType.SelectedIndex > 0)
            {
                long id = long.Parse(ConfirmDocType.SelectedItem.Value.ToString());
                child.DocType = db.DocumentTypes.FirstOrDefault(x => x.ID == id);
            }
            child.ShowDialog();
            if (child.DocType != null)
            {
                ConfirmDocType.Items.FirstOrDefault(x => x.Value.ToString() == child.DocType.ID.ToString()).Selected = true;
            }
            child = null;
        }

        private void ConfirmDocDate_ValueChanged(object sender, EventArgs e)
        {
            if (ConfirmDocDate.Value != ConfirmDocDate.NullDate)
                ConfirmDocDateMaskedEditBox.Text = ConfirmDocDate.Value.ToShortDateString();
            else
                ConfirmDocDateMaskedEditBox.Text = ConfirmDocDateMaskedEditBox.NullText;
        }

        private void ConfirmDocDateMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (ConfirmDocDateMaskedEditBox.Text != ConfirmDocDateMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(ConfirmDocDateMaskedEditBox.Text, out date))
                {
                    ConfirmDocDate.Value = date;
                }
                else
                {
                    ConfirmDocDate.Value = ConfirmDocDate.NullDate;
                }
            }
            else
            {
                ConfirmDocDate.Value = ConfirmDocDate.NullDate;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            staff = null;
            this.Close();
        }

        private void Snils__Leave(object sender, EventArgs e)
        {
            var ssn = Snils_.Value.ToString().Split(' ');

            if (!ssn[0].Contains("_"))
            {
                string contrNum = Utils.GetControlSumSSN(ssn[0]);

                Snils_.Value = ssn[0] + " " + contrNum;
                //if (!Utils.ValidateSSN(Snils_.Value.ToString()))
                //{
                //    RadMessageBox.Show("Ошибка при заполнении Страхового номера");
                //    Snils_.Focus();
                //}
            }
            else
            {
                if (Snils_.Value.ToString() != "___-___-___ __")
                {
                    Snils_.Text = "___-___-___ __";
                }

            }
        }

        private void dateWorkGridUpdate()
        {
            dateWorkGrid.Rows.Clear();
            dateWorkGrid.DataSource = staff.StaffDateWork;
            dateWorkGrid.Columns["ID"].IsVisible = false;
            dateWorkGrid.Columns["StaffID"].IsVisible = false;
            dateWorkGrid.Columns["Staff"].IsVisible = false;
            dateWorkGrid.Columns["DateBeginWork"].HeaderText = "Дата приёма";
            dateWorkGrid.Columns["DateBeginWork"].Width = 180;
            dateWorkGrid.Columns["DateBeginWork"].FormatString = "{0:dd.MM.yyyy}";
            dateWorkGrid.Columns["DateEndWork"].HeaderText = "Дата увольнения";
            dateWorkGrid.Columns["DateEndWork"].Width = 180;
            dateWorkGrid.Columns["DateEndWork"].FormatString = "{0:dd.MM.yyyy}";


        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DepartmentsFrm child = new DepartmentsFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.DepID = DepID;
            child.action = "selection";
            child.ShowDialog();
            DepID = child.DepID;
            if (DepID != 0)
            {
                Department dep = db.Department.FirstOrDefault(x => x.ID == DepID);

                DepCode_.Text = dep.Code;
                DepName_.Text = dep.Name;
            }
        }

        private void StaffAnketaEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cleanData)
                staff = null;
        }

        private void DateBirth__ValueChanged(object sender, EventArgs e)
        {
            if (DateBirth_.Value != DateBirth_.NullDate)
                DateBirth_MaskedEditBox.Text = DateBirth_.Value.ToShortDateString();
            else
                DateBirth_MaskedEditBox.Text = DateBirth_MaskedEditBox.NullText;
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

        private void Snils__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                TabelNumber_.Focus();
            }
        }

        private void INN__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void INN__Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(INN_.Text))
                INN_.Text = INN_.Text.PadLeft(12, '0');
        }

        private void cleanDepBtn_Click(object sender, EventArgs e)
        {
            DepID = 0;
            DepCode_.Text = "";
            DepName_.Text = "";
        }

        private void dateWorkGrid_CellEditorInitialized(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            RadDateTimeEditor editor = this.dateWorkGrid.ActiveEditor as RadDateTimeEditor;
            if (editor != null)
            {
                //Pick up one of the default formats
                ((RadDateTimeEditorElement)((RadDateTimeEditor)this.dateWorkGrid.ActiveEditor).EditorElement).Format = DateTimePickerFormat.Short;

                //Or set a custom date format
                ((RadDateTimeEditorElement)((RadDateTimeEditor)this.dateWorkGrid.ActiveEditor).EditorElement).CustomFormat = "t";
            }
        }

        private void dateWorkGrid_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            db.DeleteObject((StaffDateWork)e.Rows[0].DataBoundItem);
        }

        private void StaffAnketaEdit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            radPageView1.SelectedPage = radPageView1.Pages[0];

            checkAccessLevel();

            #region Тип документов DDL
            BindingSource b = new BindingSource();
            b.DataSource = db.DocumentTypes.ToList();

            this.ConfirmDocType.DataSource = null;
            this.ConfirmDocType.Items.Clear();
            this.ConfirmDocType.DisplayMember = "Code";
            this.ConfirmDocType.ValueMember = "ID";
            this.ConfirmDocType.ShowItemToolTips = true;
            this.ConfirmDocType.DataSource = b.DataSource;

            this.ConfirmDocType.SelectedIndex = ConfirmDocType.Items.Any(x => x.Text.ToString() == "ПАСПОРТ РОССИИ") ? ConfirmDocType.Items.First(x => x.Text.ToString() == "ПАСПОРТ РОССИИ").Index : -1;
            //this.ConfirmDocType.ResetText();

            #endregion

            this.ConfirmDocType.SelectedIndexChanged += (s, с) => ConfirmDocTypeDDL_SelectedIndexChanged();
            ConfirmDocTypeDDL_SelectedIndexChanged();

            if (action == "edit")
            {
                staff = db.Staff.FirstOrDefault(x => x.ID == staff.ID);

                Snils_.Text = staff.ControlNumber != null ? staff.InsuranceNumber + staff.ControlNumber.Value.ToString().PadLeft(2, '0') : staff.InsuranceNumber;
                TabelNumber_.Text = staff.TabelNumber != null ? staff.TabelNumber.Value.ToString() : "";
                INN_.Text = (!String.IsNullOrEmpty(staff.INN) && staff.INN != "0") ? staff.INN.PadLeft(12, '0') : "";
                Dismissed_.Checked = staff.Dismissed == 0 ? false : true;
                LastName_.Text = staff.LastName;
                FirstName_.Text = staff.FirstName;
                MiddleName_.Text = staff.MiddleName;

                if (staff.DateBirth != null && staff.DateBirth.HasValue)
                    DateBirth_.Value = staff.DateBirth.Value;

                if (staff.Sex != null && staff.Sex.HasValue)
                {
                    if (staff.Sex.Value == 0)
                        SexMRadioButton.IsChecked = true;
                    else
                        SexFRadioButton.IsChecked = true;
                }

                if (staff.DepartmentID != null)
                {
                    DepID = staff.DepartmentID.Value;
                    Department dep = db.Department.FirstOrDefault(x => x.ID == DepID);

                    DepCode_.Text = dep.Code;
                    DepName_.Text = dep.Name;
                }

                dateWorkGridUpdate();

                fillAnketa();

            }
            else
            {
                staff = new Staff();
                DateUnderwrite.Value = DateTime.Now;
                radPageView1.Pages["radPageViewPage2"].Enabled = false;
            }


        }

        private void fillAnketa()
        {
            if (db.FormsADW_1.Any(x => x.StaffID == staff.ID))   // Если есть заполненная анкета на этого сотрудника
            {
                FormsADW_1 adw1 = db.FormsADW_1.First(x => x.StaffID == staff.ID);

                if (adw1.Doc_Type_ID.HasValue)
                {
                    ConfirmDocType.Items.FirstOrDefault(x => x.Value.ToString() == adw1.Doc_Type_ID.Value.ToString()).Selected = true;
                    ConfirmDocName.Text = adw1.Doc_Name;
                    ConfirmDocSerLat.Text = adw1.Ser_Lat;
                    ConfirmDocSerRus.Text = adw1.Ser_Rus;
                    ConfirmDocNum.Text = adw1.Doc_Num;
                    if (adw1.Doc_Date.HasValue)
                        ConfirmDocDate.Value = adw1.Doc_Date.Value;
                    ConfirmDocKemVyd.Text = adw1.Doc_Kem_Vyd;
                }

                if (adw1.Type_DateBirth.HasValue && adw1.Type_DateBirth.Value == 1)
                {
                    DateOs_CheckBox.Checked = true;
                    DayOs_SpinEditor.Value = adw1.DateBirthDay_Os.HasValue ? adw1.DateBirthDay_Os.Value : 0;
                    MonthOs_SpinEditor.Value = adw1.DateBirthMonth_Os.HasValue ? adw1.DateBirthMonth_Os.Value : 0;
                    YearOs_SpinEditor.Value = adw1.DateBirthYear_Os.HasValue ? adw1.DateBirthYear_Os.Value : 0;
                }

                Phone.Text = adw1.Phone;
                if (adw1.DateFilling.HasValue)
                    DateUnderwrite.Value = adw1.DateFilling.Value;
                else
                    DateUnderwrite.Value = DateTime.Now;


                if (adw1.Type_PlaceBirth.HasValue)
                {
                    Type_PlaceBirth.Items.FirstOrDefault(x => x.Tag.ToString() == adw1.Type_PlaceBirth.Value.ToString()).Selected = true;
                }

                Punkt.Text = adw1.Punkt;
                Distr.Text = adw1.Distr;
                Region.Text = adw1.Region;
                Country.Text = adw1.Country;
                Citizenship.Text = adw1.Citizenship;

                Reg_Addr.Text = adw1.Reg_Addr;
                Fakt_Addr.Text = adw1.Fakt_Addr;

            }
            else
            {
                DateUnderwrite.Value = DateTime.Now;

            }

        }

        private void ConfirmDocTypeDDL_SelectedIndexChanged()
        {
            if (this.ConfirmDocType.SelectedItem != null && this.ConfirmDocType.SelectedItem.Text.ToLower() == "прочее")
            {
                ConfirmDocName.Enabled = true;
            }
            else
            {
                ConfirmDocName.Enabled = false;
            }

        }

        private void radButton3_Click(object sender, EventArgs e)
        {

            if (validation())
            {

                FormsADW_1 adw1 = GetData();
                cleanData = false;

                switch (action)
                {
                    case "add":
                        try
                        {
                            if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == staff.InsuranceNumber))
                            {
                                RadMessageBox.Show(this, "Дублирование записи.\r\nСотрудник с таким Страховым номером уже есть в базе данных!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }

                            staff.FormsADW_1.Add(adw1);

                            db.AddToStaff(staff);
                            db.SaveChanges();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранении данных произошла ошибка! " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                        }

                        break;
                    case "edit":
                        try
                        {
                            if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.ID != staff.ID && x.InsuranceNumber == staff.InsuranceNumber))
                            {
                                RadMessageBox.Show(this, "Дублирование записи.\r\nСотрудник с таким Страховым номером уже есть в базе данных!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }

                            if (!staff.FormsADW_1.Any())
                            {
                                staff.FormsADW_1.Add(adw1);
                            }
                            else
                            {
                                db.ObjectStateManager.ChangeObjectState(adw1, EntityState.Modified);
                            }

                            //staff.DepartmentID = staffData.DepartmentID;
                            //staff.ControlNumber = staffData.ControlNumber;
                            //staff.DateBirth = staffData.DateBirth;
                            //staff.Dismissed = staffData.Dismissed;
                            //staff.FirstName = staffData.FirstName;
                            //staff.INN = staffData.INN;
                            //staff.InsuranceNumber = staffData.InsuranceNumber;
                            //staff.LastName = staffData.LastName;
                            //staff.MiddleName = staffData.MiddleName;
                            //staff.Sex = staffData.Sex;
                            //staff.TabelNumber = staffData.TabelNumber;

                            db.ObjectStateManager.ChangeObjectState(staff, EntityState.Modified);
                            db.SaveChanges();
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранении данных произошла ошибка! " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                        }
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

            if (Snils_.Value.ToString() == "___-___-___ __")
                errMessBox.Add(new ErrList { name = "Поле СНИЛС обязательно к заполнению!", control = "Snils_" });
            if (String.IsNullOrEmpty(LastName_.Text))
                errMessBox.Add(new ErrList { name = "Поле Фамилия обязательно к заполнению!", control = "LastName_" });
            if (String.IsNullOrEmpty(FirstName_.Text))
                errMessBox.Add(new ErrList { name = "Поле Имя обязательно к заполнению!", control = "FirstName_" });
            //            if (String.IsNullOrEmpty(SexDropDown_.Text))
            //                errMessBox.Add(new ErrList { name = "Поле Пол обязательно к заполнению!", control = "SexDropDown_" });
            //            if (String.IsNullOrEmpty(DateBirthDateTimePicker_.Text))
            //                errMessBox.Add(new ErrList { name = "Поле Дата рождения обязательно к заполнению!", control = "DateBirthDateTimePicker_" });


            if (errMessBox.Count > 0)
                result = false;

            return result;
        }

        private FormsADW_1 GetData()
        {
            FormsADW_1 adw1 = staff.FormsADW_1.Any() ? staff.FormsADW_1.First() : new FormsADW_1();

            string snils = "";
            byte contrNum = 0;

            if (Utils.ValidateSSN(Snils_.Value.ToString()))
            {
                var s2 = (this.Snils_.Value.ToString()).Split(' ');

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


            staff.Dismissed = Dismissed_.Checked ? (byte)1 : (byte)0;
            staff.FirstName = FirstName_.Text;
            staff.INN = INN_.Text;
            staff.InsuranceNumber = snils;
            staff.InsurerID = Options.InsID;
            staff.LastName = LastName_.Text;
            staff.MiddleName = MiddleName_.Text;
            if (SexMRadioButton.IsChecked || SexFRadioButton.IsChecked)
            {
                staff.Sex = SexMRadioButton.IsChecked ? (byte)0 : (byte)1;
            }

            if (!String.IsNullOrEmpty(TabelNumber_.Text) && long.Parse(TabelNumber_.Text) != 0)
                staff.TabelNumber = long.Parse(TabelNumber_.Text);
            else
                staff.TabelNumber = null;

            staff.ControlNumber = contrNum;

            if (DepID != 0)
            {
                staff.DepartmentID = DepID;
            }
            else
            {
                staff.DepartmentID = null;
            }

            if (!String.IsNullOrEmpty(DateBirth_.Text) && !DateOs_CheckBox.Checked)
            {
                staff.DateBirth = DateBirth_.Value;
            }
            else
            {
                staff.DateBirth = null;
            }

            adw1.Type_DateBirth = DateOs_CheckBox.Checked ? (short)1 : (short)0;
            if (DateOs_CheckBox.Checked)
            {
                adw1.DateBirthDay_Os = (short)DayOs_SpinEditor.Value;
                adw1.DateBirthMonth_Os = (short)MonthOs_SpinEditor.Value;
                adw1.DateBirthYear_Os = (short)YearOs_SpinEditor.Value;
            }

            adw1.Phone = Phone.Text;
            adw1.DateFilling = DateUnderwrite.Value;

            adw1.Type_PlaceBirth = short.Parse(Type_PlaceBirth.SelectedItem.Tag.ToString());
            adw1.Punkt = Punkt.Text;
            adw1.Distr = Distr.Text;
            adw1.Region = Region.Text;
            adw1.Country = Country.Text;
            adw1.Citizenship = Citizenship.Text;

            adw1.Fakt_Addr = Fakt_Addr.Text;
            adw1.Reg_Addr = Reg_Addr.Text;

            if (ConfirmDocType.Text == "")
                adw1.Doc_Type_ID = null;
            else
                adw1.Doc_Type_ID = long.Parse(ConfirmDocType.SelectedItem.Value.ToString());
            adw1.Doc_Name = ConfirmDocName.Text;
            adw1.Ser_Lat = ConfirmDocSerLat.Text;
            adw1.Ser_Rus = ConfirmDocSerRus.Text;
            adw1.Doc_Num = ConfirmDocNum.Text;

            if (ConfirmDocDate.Text == "")
                adw1.Doc_Date = null;
            else
                adw1.Doc_Date = ConfirmDocDate.Value.Date;
            adw1.Doc_Kem_Vyd = ConfirmDocKemVyd.Text;

            return adw1;
        }


        private void DayOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            DayOs_TextBox.Text = DayOs_SpinEditor.Value == 0 ? "" : DayOs_SpinEditor.Value.ToString();
        }

        private void MonthOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            MonthOs_TextBox.Text = MonthOs_SpinEditor.Value == 0 ? "" : MonthOs_SpinEditor.Value.ToString();
        }

        private void YearOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            YearOs_TextBox.Text = YearOs_SpinEditor.Value == 0 ? "" : YearOs_SpinEditor.Value.ToString();
        }

        private void DayOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void MonthOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void YearOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void DayOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(DayOs_TextBox.Text))
            {
                try
                {
                    DayOs_SpinEditor.Value = decimal.Parse(DayOs_TextBox.Text);
                }
                catch
                {
                    DayOs_TextBox.Text = "";
                }
            }
            else
                DayOs_SpinEditor.Value = 0;
        }

        private void MonthOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(MonthOs_TextBox.Text))
            {
                try
                {
                    MonthOs_SpinEditor.Value = decimal.Parse(MonthOs_TextBox.Text);
                }
                catch
                {
                    MonthOs_TextBox.Text = "";
                }
            }
            else
                MonthOs_SpinEditor.Value = 0;

        }

        private void YearOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(YearOs_TextBox.Text))
            {
                try
                {
                    YearOs_SpinEditor.Value = decimal.Parse(YearOs_TextBox.Text);
                }
                catch
                {
                    YearOs_TextBox.Text = "";
                }
            }
            else
                YearOs_SpinEditor.Value = 0;
        }

        private void Reg_Addr_Edit_btn_Click(object sender, EventArgs e)
        {
            FormsADW1.AddressEditor child = new FormsADW1.AddressEditor();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.address = Reg_Addr.Text;
            child.ShowDialog();
            Reg_Addr.Text = child.address;
        }

        private void Fakt_Addr_Edit_btn_Click(object sender, EventArgs e)
        {
            FormsADW1.AddressEditor child = new FormsADW1.AddressEditor();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.address = Fakt_Addr.Text;
            child.ShowDialog();
            Fakt_Addr.Text = child.address;
        }


    }
}
