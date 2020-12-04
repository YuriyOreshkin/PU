using PU.FormsRSW2014;
using System;
using System.Data.Entity;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using PU.Models;
using Telerik.WinControls.UI;

namespace PU.FormsADW3
{
    public partial class FormsADW3_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action;
        public FormsADW_3 adw_3 { get; set; }
        public long StaffID { get; set; }
        private bool cleanData = true;

        public FormsADW3_Edit()
        {
            InitializeComponent();
        }

        public static FormsADW3_Edit SelfRef
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
            Dictionaries.BaseDictionaryEvents.LookUp(this, ConfirmDocType, "DocumentTypes");
           
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
            adw_3 = null;
            this.Close();
        }


        private void StaffAnketaEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cleanData)
                adw_3 = null;
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
            this.ConfirmDocType.Items.Add(new RadListDataItem { Text = "", Value = 0});

            this.ConfirmDocType.Items.Last.Selected = true;
            //this.ConfirmDocType.ResetText();

            #endregion

            this.ConfirmDocType.SelectedIndexChanged += (s, с) => ConfirmDocTypeDDL_SelectedIndexChanged();
            ConfirmDocTypeDDL_SelectedIndexChanged();

            if (action == "edit")
            {
                adw_3 = db.FormsADW_3.FirstOrDefault(x => x.ID == adw_3.ID);

                LastName_.Text = adw_3.LastName;
                FirstName_.Text = adw_3.FirstName;
                MiddleName_.Text = adw_3.MiddleName;
                numSpinEditor.Value = adw_3.Num.HasValue ? adw_3.Num.Value : 1;
                MiddleNameCancel.Checked = !String.IsNullOrEmpty(adw_3.MiddleNameCancel);
                PlaceBirthCancel.Checked = !String.IsNullOrEmpty(adw_3.PlaceBirthCancel);

                if (adw_3.DateBirth != null && adw_3.DateBirth.HasValue)
                    DateBirth_.Value = adw_3.DateBirth.Value;

                if (adw_3.Sex != null && adw_3.Sex.HasValue)
                {
                    if (adw_3.Sex.Value == 0)
                        SexMRadioButton.IsChecked = true;
                    else
                        SexFRadioButton.IsChecked = true;
                }

                if (!String.IsNullOrEmpty(adw_3.OtmetkaOPredSved.Trim()))
                {
                    Otmetka.Items.FirstOrDefault(x => x.Text.ToString() == adw_3.OtmetkaOPredSved).Selected = true;
                }


                if (adw_3.Doc_Type_ID.HasValue)
                {
                    ConfirmDocType.Items.FirstOrDefault(x => x.Value.ToString() == adw_3.Doc_Type_ID.Value.ToString()).Selected = true;
                    ConfirmDocName.Text = adw_3.Doc_Name;
                    ConfirmDocSerLat.Text = adw_3.Ser_Lat;
                    ConfirmDocSerRus.Text = adw_3.Ser_Rus;
                    ConfirmDocNum.Text = adw_3.Doc_Num;
                    if (adw_3.Doc_Date.HasValue)
                        ConfirmDocDate.Value = adw_3.Doc_Date.Value;
                    ConfirmDocKemVyd.Text = adw_3.Doc_Kem_Vyd;
                }

                if (adw_3.Type_DateBirth == 1)
                {
                    DateOs_CheckBox.Checked = true;
                    DayOs_SpinEditor.Value = adw_3.DateBirthDay_Os.HasValue ? adw_3.DateBirthDay_Os.Value : 0;
                    MonthOs_SpinEditor.Value = adw_3.DateBirthMonth_Os.HasValue ? adw_3.DateBirthMonth_Os.Value : 0;
                    YearOs_SpinEditor.Value = adw_3.DateBirthYear_Os.HasValue ? adw_3.DateBirthYear_Os.Value : 0;
                }

                Phone.Text = adw_3.Phone;
                if (adw_3.DateFilling.HasValue)
                    DateUnderwrite.Value = adw_3.DateFilling.Value;
                else
                    DateUnderwrite.Value = DateTime.Now;


                if (adw_3.Type_PlaceBirth.HasValue)
                {
                    Type_PlaceBirth.Items.FirstOrDefault(x => x.Tag.ToString() == adw_3.Type_PlaceBirth.Value.ToString()).Selected = true;
                }

                Punkt.Text = adw_3.Punkt;
                Distr.Text = adw_3.Distr;
                Region.Text = adw_3.Region;
                Country.Text = adw_3.Country;
                Citizenship.Text = adw_3.Citizenship;

                Reg_Addr.Text = adw_3.Reg_Addr;
                Fakt_Addr.Text = adw_3.Fakt_Addr;

            }
            else
            {
                adw_3 = new FormsADW_3();
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
            GetData();
            cleanData = false;

            switch (action)
            {
                case "add":
                    try
                    {
                        if (db.FormsADW_3.Any(x => x.StaffID == StaffID && x.Num == adw_3.Num))
                        {
                            RadMessageBox.Show(this, "Дублирование записи.\r\nФорма АДВ-3 с таким номером уже есть в базе данных!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }

                        db.FormsADW_3.Add(adw_3);
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
                        if (db.FormsADW_3.Any(x => x.StaffID == StaffID && x.ID != adw_3.ID && x.Num == adw_3.Num))
                        {
                            RadMessageBox.Show(this, "Дублирование записи.\r\nФорма АДВ-3 с таким номером уже есть в базе данных!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                            return;
                        }


                        db.Entry(adw_3).State = EntityState.Modified;
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


        private void GetData()
        {
            adw_3.FirstName = FirstName_.Text;
            adw_3.StaffID = StaffID;
            adw_3.LastName = LastName_.Text;
            adw_3.MiddleName = MiddleName_.Text;
            adw_3.MiddleNameCancel = MiddleNameCancel.Checked ? "ОТМН" : "";
            adw_3.Num = (short)numSpinEditor.Value;

            if (SexMRadioButton.IsChecked || SexFRadioButton.IsChecked)
            {
                adw_3.Sex = SexMRadioButton.IsChecked ? (byte)0 : (byte)1;
            }


            if (!String.IsNullOrEmpty(DateBirth_.Text) && !DateOs_CheckBox.Checked)
            {
                adw_3.DateBirth = DateBirth_.Value;
            }
            else
            {
                adw_3.DateBirth = null;
            }

            adw_3.OtmetkaOPredSved = Otmetka.Text;

            adw_3.Type_DateBirth = DateOs_CheckBox.Checked ? (short)1 : (short)0;
            if (DateOs_CheckBox.Checked)
            {
                adw_3.DateBirthDay_Os = (short)DayOs_SpinEditor.Value;
                adw_3.DateBirthMonth_Os = (short)MonthOs_SpinEditor.Value;
                adw_3.DateBirthYear_Os = (short)YearOs_SpinEditor.Value;
            }

            adw_3.Phone = Phone.Text;
            adw_3.DateFilling = DateUnderwrite.Value;
            adw_3.DateUpdate = DateTime.Now;

            adw_3.Type_PlaceBirth = short.Parse(Type_PlaceBirth.SelectedItem.Tag.ToString());
            adw_3.Punkt = Punkt.Text;
            adw_3.Distr = Distr.Text;
            adw_3.Region = Region.Text;
            adw_3.Country = Country.Text;
            adw_3.Citizenship = Citizenship.Text;

            adw_3.PlaceBirthCancel = PlaceBirthCancel.Checked ? "ОТМН" : "";

            adw_3.Fakt_Addr = Fakt_Addr.Text;
            adw_3.Reg_Addr = Reg_Addr.Text;

            if (ConfirmDocType.Text == "")
                adw_3.Doc_Type_ID = null;
            else
            {
                adw_3.Doc_Type_ID = long.Parse(ConfirmDocType.SelectedItem.Value.ToString());
            }
            adw_3.Doc_Name = ConfirmDocName.Text;
            adw_3.Ser_Lat = ConfirmDocSerLat.Text;
            adw_3.Ser_Rus = ConfirmDocSerRus.Text;
            adw_3.Doc_Num = ConfirmDocNum.Text;

            if (ConfirmDocDate.Text == "")
                adw_3.Doc_Date = null;
            else
                adw_3.Doc_Date = ConfirmDocDate.Value.Date;
            adw_3.Doc_Kem_Vyd = ConfirmDocKemVyd.Text;

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

        private void PlaceBirthCancel_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            Type_PlaceBirth.Items.First.Selected = true;
            Punkt.Text = "";
            Distr.Text = "";
            Region.Text = "";
            Country.Text = "";

            Type_PlaceBirth.Enabled = !PlaceBirthCancel.Checked;
            Punkt.Enabled = !PlaceBirthCancel.Checked;
            Distr.Enabled = !PlaceBirthCancel.Checked;
            Region.Enabled = !PlaceBirthCancel.Checked;
            Country.Enabled = !PlaceBirthCancel.Checked;
        }

        private void MiddleNameCancel_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            MiddleName_.Text = "";
            MiddleName_.Enabled = !MiddleNameCancel.Checked;
        }


    }
}
