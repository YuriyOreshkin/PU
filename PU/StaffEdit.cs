using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using PU.Models;
using PU.Classes;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI;
using PU.FormsRSW2014;
using PU.Dictionaries;

namespace PU
{
    public partial class StaffEdit : Telerik.WinControls.UI.RadForm
    {
        public string action;
        public long InsID = 0;   // ID страхователя
        public long DepID = 0;// ID подразделения
        public pu6Entities db = new pu6Entities();

        public Staff staffData = new Staff();
        public Staff staff { get; set; }
        private List<ErrList> errMessBox = new List<ErrList>();
        private bool cleanData = true;

        public StaffEdit()
        {
            InitializeComponent();
            SelfRef = this;
        }

        public static StaffEdit SelfRef
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

        private void radButton2_Click(object sender, EventArgs e)
        {
            staff = null;
            staffData = null;
            this.Close();
        }

        private void GetData()
        {
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
            staff.InsurerID = InsID;
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

//            if (contrNum != 0)
//            {
                staff.ControlNumber = contrNum;
//            }
                if (DepID != 0)
                {
                    staff.DepartmentID = DepID;
                }
                else
                {
                    staff.DepartmentID = null;
                }


            if (!String.IsNullOrEmpty(DateBirth_.Text))
            {
                staff.DateBirth = DateBirth_.Value;
            }
            else
            {
                staff.DateBirth = null;
            }


            // Профессия
            if (DolgnTextBox.Text != "")
            {

                if (db.Dolgn.Any(x => x.Name == DolgnTextBox.Text.Trim())) // если такая профессия уже есть в справочнике, то берем ее ИД
                {
                    staff.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == DolgnTextBox.Text.Trim()).ID;
                }
                else // если такой профессии нет, то добавляем ее
                {
                    try
                    {
                        Dolgn newItem = new Dolgn
                        {
                            Name = DolgnTextBox.Text.Trim()
                        };
                        db.Dolgn.Add(newItem);
                        db.SaveChanges();

                        staff.DolgnID = newItem.ID;
                    }
                    catch(Exception ex)
                    {
                        RadMessageBox.Show("Не удалось сохранить должность сотрудника в справочник! Код ошибки - " + ex.Message);
                    }
                }
            }
            else
            {
                staff.DolgnID = null;
            }



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
            else {
                if (Snils_.Value.ToString() != "___-___-___ __")
                {
                    Snils_.Text = "___-___-___ __";
                }

            }

        }


        private void radButton3_Click(object sender, EventArgs e)
        {
            if (validation())
            {

                GetData();
                cleanData = false;

                switch (action)
                {
                    case "add":
                        try
                        {
                            if (db.Staff.Any(x => x.InsurerID == InsID && x.InsuranceNumber == staff.InsuranceNumber))
                            {
                                RadMessageBox.Show(this, "Дублирование записи.\r\nСотрудник с таким Страховым номером уже есть в базе данных!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
                            }

                            db.Staff.Add(staff);
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
                            if (db.Staff.Any(x => x.InsurerID == InsID && x.ID != staff.ID && x.InsuranceNumber == staff.InsuranceNumber))
                            {
                                RadMessageBox.Show(this, "Дублирование записи.\r\nСотрудник с таким Страховым номером уже есть в базе данных!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                                return;
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

                            db.Entry(staff).State = EntityState.Modified;
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


        private void StaffEdit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            radPageView1.SelectedPage = radPageView1.Pages[0];

            checkAccessLevel();

            if (action == "edit")
            {
                staff = db.Staff.FirstOrDefault(x => x.ID == staffData.ID);

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

                // Профессия
                if (staff.DolgnID.HasValue)
                {
                    DolgnTextBox.Text = db.Dolgn.First(x => x.ID == staff.DolgnID).Name;
                }
                else
                {
                    DolgnTextBox.ResetText();
                }

                dateWorkGridUpdate();
            }
            else
            {
                staff = new Staff();
                radPageView1.Pages[1].Enabled = false;
            }

        }

        private void dateWorkGridUpdate()
        {
            dateWorkGrid.Rows.Clear();
            dateWorkGrid.DataSource = staff.StaffDateWork;
//            dateWorkGrid.DataSource = db.StaffDateWork.Where(x => x.StaffID == staff.ID);

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

        private void radButton1_Click(object sender, EventArgs e)
        {
            DepartmentsFrm child = new DepartmentsFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.DepID = DepID;
            child.InsID = InsID;
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

        private void StaffEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (cleanData)
                staff = null;
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

        private void dateWorkGrid_CellEditorInitialized(object sender, GridViewCellEventArgs e)
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
            db.StaffDateWork.Remove((StaffDateWork)e.Rows[0].DataBoundItem);
        }

        private void findDolgnBtn_Click(object sender, EventArgs e)
        {
            Dictionaries.BaseDictionaryEvents.LookUp(this, DolgnTextBox, "Dolgn");
        }

        private void cleanDolgnBtn_Click(object sender, EventArgs e)
        {
            this.DolgnTextBox.Text = "";
        }


    }
}
