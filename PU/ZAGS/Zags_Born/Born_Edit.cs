using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using System.Reflection;

namespace PU.ZAGS.Zags_Born
{
    public partial class Born_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action;
        public ZAGS_Born formData { get; set; }
        private bool cleanData = true;


        public Born_Edit()
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

        private void cDateBirth_ValueChanged(object sender, EventArgs e)
        {
            if (cDateBirth.Value != cDateBirth.NullDate)
                cDateBirth_MaskedEditBox.Text = cDateBirth.Value.ToShortDateString();
            else
                cDateBirth_MaskedEditBox.Text = cDateBirth_MaskedEditBox.NullText;
        }

        private void cDateBirth_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (cDateBirth_MaskedEditBox.Text != cDateBirth_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(cDateBirth_MaskedEditBox.Text, out date))
                {
                    cDateBirth.Value = date;
                }
                else
                {
                    cDateBirth.Value = cDateBirth.NullDate;
                }
            }
            else
            {
                cDateBirth.Value = cDateBirth.NullDate;
            }
        }

        private void cAkt_Date_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (cAkt_Date_MaskedEditBox.Text != cAkt_Date_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(cAkt_Date_MaskedEditBox.Text, out date))
                {
                    cAkt_Date.Value = date;
                }
                else
                {
                    cAkt_Date.Value = cAkt_Date.NullDate;
                }
            }
            else
            {
                cAkt_Date.Value = cAkt_Date.NullDate;
            }
        }

        private void cAkt_Date_ValueChanged(object sender, EventArgs e)
        {
            if (cAkt_Date.Value != cAkt_Date.NullDate)
                cAkt_Date_MaskedEditBox.Text = cAkt_Date.Value.ToShortDateString();
            else
                cAkt_Date_MaskedEditBox.Text = cAkt_Date_MaskedEditBox.NullText;
        }

        private void cSvid_Date_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (cSvid_Date_MaskedEditBox.Text != cSvid_Date_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(cSvid_Date_MaskedEditBox.Text, out date))
                {
                    cSvid_Date.Value = date;
                }
                else
                {
                    cSvid_Date.Value = cSvid_Date.NullDate;
                }
            }
            else
            {
                cSvid_Date.Value = cSvid_Date.NullDate;
            }
        }

        private void cSvid_Date_VisibleChanged(object sender, EventArgs e)
        {
            if (cSvid_Date.Value != cSvid_Date.NullDate)
                cSvid_Date_MaskedEditBox.Text = cSvid_Date.Value.ToShortDateString();
            else
                cSvid_Date_MaskedEditBox.Text = cSvid_Date_MaskedEditBox.NullText;
        }

        private void mDateBirth_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (mDateBirth_MaskedEditBox.Text != mDateBirth_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(mDateBirth_MaskedEditBox.Text, out date))
                {
                    mDateBirth.Value = date;
                }
                else
                {
                    mDateBirth.Value = mDateBirth.NullDate;
                }
            }
            else
            {
                mDateBirth.Value = mDateBirth.NullDate;
            }
        }

        private void mDateBirth_ValueChanged(object sender, EventArgs e)
        {
            if (mDateBirth.Value != mDateBirth.NullDate)
                mDateBirth_MaskedEditBox.Text = mDateBirth.Value.ToShortDateString();
            else
                mDateBirth_MaskedEditBox.Text = mDateBirth_MaskedEditBox.NullText;
        }

        private void fDateBirth_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (fDateBirth_MaskedEditBox.Text != fDateBirth_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(fDateBirth_MaskedEditBox.Text, out date))
                {
                    fDateBirth.Value = date;
                }
                else
                {
                    fDateBirth.Value = fDateBirth.NullDate;
                }
            }
            else
            {
                fDateBirth.Value = fDateBirth.NullDate;
            }
        }

        private void fDateBirth_ValueChanged(object sender, EventArgs e)
        {
            if (fDateBirth.Value != fDateBirth.NullDate)
                fDateBirth_MaskedEditBox.Text = fDateBirth.Value.ToShortDateString();
            else
                fDateBirth_MaskedEditBox.Text = fDateBirth_MaskedEditBox.NullText;
        }

        private void cDayOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cDayOs_TextBox.Text))
            {
                try
                {
                    cDayOs_SpinEditor.Value = decimal.Parse(cDayOs_TextBox.Text);
                }
                catch
                {
                    cDayOs_TextBox.Text = "";
                }
            }
            else
                cDayOs_SpinEditor.Value = 0;
        }

        private void cDayOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void cDayOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            cDayOs_TextBox.Text = cDayOs_SpinEditor.Value == 0 ? "" : cDayOs_SpinEditor.Value.ToString();
        }

        private void cMonthOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cMonthOs_TextBox.Text))
            {
                try
                {
                    cMonthOs_SpinEditor.Value = decimal.Parse(cMonthOs_TextBox.Text);
                }
                catch
                {
                    cMonthOs_TextBox.Text = "";
                }
            }
            else
                cMonthOs_SpinEditor.Value = 0;
        }

        private void cMonthOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void cMonthOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            cMonthOs_TextBox.Text = cMonthOs_SpinEditor.Value == 0 ? "" : cMonthOs_SpinEditor.Value.ToString();
        }

        private void cYearOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(cYearOs_TextBox.Text))
            {
                try
                {
                    cYearOs_SpinEditor.Value = decimal.Parse(cYearOs_TextBox.Text);
                }
                catch
                {
                    cYearOs_TextBox.Text = "";
                }
            }
            else
                cYearOs_SpinEditor.Value = 0;
        }

        private void cYearOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void cYearOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            cYearOs_TextBox.Text = cYearOs_SpinEditor.Value == 0 ? "" : cYearOs_SpinEditor.Value.ToString();
        }








        private void mDayOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(mDayOs_TextBox.Text))
            {
                try
                {
                    mDayOs_SpinEditor.Value = decimal.Parse(mDayOs_TextBox.Text);
                }
                catch
                {
                    mDayOs_TextBox.Text = "";
                }
            }
            else
                mDayOs_SpinEditor.Value = 0;
        }

        private void mDayOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void mDayOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            mDayOs_TextBox.Text = mDayOs_SpinEditor.Value == 0 ? "" : mDayOs_SpinEditor.Value.ToString();
        }

        private void mMonthOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(mMonthOs_TextBox.Text))
            {
                try
                {
                    mMonthOs_SpinEditor.Value = decimal.Parse(mMonthOs_TextBox.Text);
                }
                catch
                {
                    mMonthOs_TextBox.Text = "";
                }
            }
            else
                mMonthOs_SpinEditor.Value = 0;
        }

        private void mMonthOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void mMonthOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            mMonthOs_TextBox.Text = mMonthOs_SpinEditor.Value == 0 ? "" : mMonthOs_SpinEditor.Value.ToString();
        }

        private void mYearOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(mYearOs_TextBox.Text))
            {
                try
                {
                    mYearOs_SpinEditor.Value = decimal.Parse(mYearOs_TextBox.Text);
                }
                catch
                {
                    mYearOs_TextBox.Text = "";
                }
            }
            else
                mYearOs_SpinEditor.Value = 0;
        }

        private void mYearOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void mYearOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            mYearOs_TextBox.Text = mYearOs_SpinEditor.Value == 0 ? "" : mYearOs_SpinEditor.Value.ToString();
        }











        private void fDayOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(fDayOs_TextBox.Text))
            {
                try
                {
                    fDayOs_SpinEditor.Value = decimal.Parse(fDayOs_TextBox.Text);
                }
                catch
                {
                    fDayOs_TextBox.Text = "";
                }
            }
            else
                fDayOs_SpinEditor.Value = 0;
        }

        private void fDayOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void fDayOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            fDayOs_TextBox.Text = fDayOs_SpinEditor.Value == 0 ? "" : fDayOs_SpinEditor.Value.ToString();
        }

        private void fMonthOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(fMonthOs_TextBox.Text))
            {
                try
                {
                    fMonthOs_SpinEditor.Value = decimal.Parse(fMonthOs_TextBox.Text);
                }
                catch
                {
                    fMonthOs_TextBox.Text = "";
                }
            }
            else
                fMonthOs_SpinEditor.Value = 0;
        }

        private void fMonthOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void fMonthOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            fMonthOs_TextBox.Text = fMonthOs_SpinEditor.Value == 0 ? "" : fMonthOs_SpinEditor.Value.ToString();
        }

        private void fYearOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(fYearOs_TextBox.Text))
            {
                try
                {
                    fYearOs_SpinEditor.Value = decimal.Parse(fYearOs_TextBox.Text);
                }
                catch
                {
                    fYearOs_TextBox.Text = "";
                }
            }
            else
                fYearOs_SpinEditor.Value = 0;
        }

        private void fYearOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void fYearOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            fYearOs_TextBox.Text = fYearOs_SpinEditor.Value == 0 ? "" : fYearOs_SpinEditor.Value.ToString();
        }

        private void cDateOs_CheckBox_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            cDayOs_SpinEditor.Enabled = cDateOs_CheckBox.Checked;
            cDayOs_TextBox.Enabled = cDateOs_CheckBox.Checked;
            cMonthOs_SpinEditor.Enabled = cDateOs_CheckBox.Checked;
            cMonthOs_TextBox.Enabled = cDateOs_CheckBox.Checked;
            cYearOs_SpinEditor.Enabled = cDateOs_CheckBox.Checked;
            cYearOs_TextBox.Enabled = cDateOs_CheckBox.Checked;

            cDateBirth.Enabled = !cDateOs_CheckBox.Checked;
            cDateBirth_MaskedEditBox.Enabled = !cDateOs_CheckBox.Checked;
        }

        private void mDateOs_CheckBox_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            mDayOs_SpinEditor.Enabled = mDateOs_CheckBox.Checked;
            mDayOs_TextBox.Enabled = mDateOs_CheckBox.Checked;
            mMonthOs_SpinEditor.Enabled = mDateOs_CheckBox.Checked;
            mMonthOs_TextBox.Enabled = mDateOs_CheckBox.Checked;
            mYearOs_SpinEditor.Enabled = mDateOs_CheckBox.Checked;
            mYearOs_TextBox.Enabled = mDateOs_CheckBox.Checked;

            mDateBirth.Enabled = !mDateOs_CheckBox.Checked;
            mDateBirth_MaskedEditBox.Enabled = !mDateOs_CheckBox.Checked;
        }

        private void fDateOs_CheckBox_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            fDayOs_SpinEditor.Enabled = fDateOs_CheckBox.Checked;
            fDayOs_TextBox.Enabled = fDateOs_CheckBox.Checked;
            fMonthOs_SpinEditor.Enabled = fDateOs_CheckBox.Checked;
            fMonthOs_TextBox.Enabled = fDateOs_CheckBox.Checked;
            fYearOs_SpinEditor.Enabled = fDateOs_CheckBox.Checked;
            fYearOs_TextBox.Enabled = fDateOs_CheckBox.Checked;

            fDateBirth.Enabled = !fDateOs_CheckBox.Checked;
            fDateBirth_MaskedEditBox.Enabled = !fDateOs_CheckBox.Checked;
        }

        private void Born_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            radPageView1.SelectedPage = radPageView1.Pages[0];

            fillCombobox();

            if (action == "edit")
            {
                if (formData != null)
                {
                    formData = db.ZAGS_Born.First(x => x.ID == formData.ID);
                    fillData();
                }
            }
            else if (action == "add")
            {
                formData = new ZAGS_Born();
            }



        }

        private void fillData()
        {

            #region // заполняем Места рождения

            var fields = typeof(ZAGS_Born).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name).Where(x => x.Contains("Punkt") || x.Contains("Distr") || x.Contains("Region") || x.Contains("Country") || x.Contains("Citizenship"));

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');

                if (this.Controls.Find(itemName, true).Any())
                {

                    var properties = formData.GetType().GetProperty(itemName);
                    if (properties.GetValue(formData, null) == null)
                        continue;

                    string value = properties.GetValue(formData, null).ToString();


                    Telerik.WinControls.UI.RadDropDownList itemForm = ((Telerik.WinControls.UI.RadDropDownList)this.Controls.Find(itemName, true)[0]);

                    //if (((Telerik.WinControls.UI.RadDropDownList)this.Controls.Find(itemName, true)[0]).Items.Any(x => x.Value.ToString() == value.Trim()))
                    //{
                    //    ((Telerik.WinControls.UI.RadDropDownList)this.Controls.Find(itemName, true)[0]).Items.First(x => x.Value.ToString() == value.Trim()).Selected = true;
                    //}
                    //else
                    //{
                    //    ((Telerik.WinControls.UI.RadDropDownList)this.Controls.Find(itemName, true)[0]).Text = value.Trim();
                    //}


                    //Telerik.WinControls.UI.RadDropDownList itemForm = ((Telerik.WinControls.UI.RadDropDownList)this.Controls.Find(itemName, true)[0]);
                    if (value != null)
                    {
                        if (itemForm.Items.Any(x => x.Value.ToString() == value.Trim()))
                        {
                            itemForm.Items.First(x => x.Value.ToString() == value.Trim()).Selected = true;
                        }
                        else
                        {
                            itemForm.Text = value.Trim();
                        }
                    }
                }
            }

            #endregion


            #region // заполняем Текстовые поля
                List<string> textFields = new List<string>{
	"cLastName",
	"cFirstName",
	"cMiddleName",
	"cAkt_Num",
	"cAkt_OrgZags",
	"cSvid_Ser",
	"cSvid_Num",
	"cSvid_OrgZags",
	"mLastName",
	"mFirstName",
	"mMiddleName",
	"fLastName",
	"fFirstName",
	"fMiddleName"};



            fields = typeof(ZAGS_Born).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            names = Array.ConvertAll(fields, field => field.Name);

            foreach (var tf in textFields)
            {
                if (!names.Any(x => x.Contains(tf)))
                    continue;

                var item = names.First(x => x.Contains(tf));

                string itemName = item.TrimStart('_');

                if (this.Controls.Find(itemName, true).Any())
                {
                    var properties = formData.GetType().GetProperty(itemName);
                    if (properties.GetValue(formData, null) == null)
                        continue;

                    string value = properties.GetValue(formData, null).ToString();

                    ((Telerik.WinControls.UI.RadTextBox)this.Controls.Find(itemName, true)[0]).Text = value.Trim();
                }
            }

            #endregion


            if (formData.cAkt_Date != null && formData.cAkt_Date.HasValue)
                cAkt_Date.Value = formData.cAkt_Date.Value;

            if (formData.cSvid_Date != null && formData.cSvid_Date.HasValue)
                cSvid_Date.Value = formData.cSvid_Date.Value;



            if (formData.cSex != null && formData.cSex.HasValue)
            {
                if (formData.cSex.Value == 0)
                    SexMRadioButton.IsChecked = true;
                else
                    SexFRadioButton.IsChecked = true;
            }

            if (formData.cDateBirth != null && formData.cDateBirth.HasValue)
                cDateBirth.Value = formData.cDateBirth.Value;

            if (formData.cType_DateBirth.HasValue && formData.cType_DateBirth.Value == 1)
            {
                cDateOs_CheckBox.Checked = true;
                cDayOs_SpinEditor.Value = formData.cDateBirthDay_Os.HasValue ? formData.cDateBirthDay_Os.Value : 0;
                cMonthOs_SpinEditor.Value = formData.cDateBirthMonth_Os.HasValue ? formData.cDateBirthMonth_Os.Value : 0;
                cYearOs_SpinEditor.Value = formData.cDateBirthYear_Os.HasValue ? formData.cDateBirthYear_Os.Value : 0;
            }


            if (formData.mDateBirth != null && formData.mDateBirth.HasValue)
                mDateBirth.Value = formData.mDateBirth.Value;

            if (formData.mType_DateBirth.HasValue && formData.mType_DateBirth.Value == 1)
            {
                mDateOs_CheckBox.Checked = true;
                mDayOs_SpinEditor.Value = formData.mDateBirthDay_Os.HasValue ? formData.mDateBirthDay_Os.Value : 0;
                mMonthOs_SpinEditor.Value = formData.mDateBirthMonth_Os.HasValue ? formData.mDateBirthMonth_Os.Value : 0;
                mYearOs_SpinEditor.Value = formData.mDateBirthYear_Os.HasValue ? formData.mDateBirthYear_Os.Value : 0;
            }


            if (formData.fDateBirth != null && formData.fDateBirth.HasValue)
                fDateBirth.Value = formData.fDateBirth.Value;

            if (formData.fType_DateBirth.HasValue && formData.fType_DateBirth.Value == 1)
            {
                fDateOs_CheckBox.Checked = true;
                fDayOs_SpinEditor.Value = formData.fDateBirthDay_Os.HasValue ? formData.fDateBirthDay_Os.Value : 0;
                fMonthOs_SpinEditor.Value = formData.fDateBirthMonth_Os.HasValue ? formData.fDateBirthMonth_Os.Value : 0;
                fYearOs_SpinEditor.Value = formData.fDateBirthYear_Os.HasValue ? formData.fDateBirthYear_Os.Value : 0;
            }


            if (formData.cType_PlaceBirth.HasValue)
            {
                cType_PlaceBirth.Items.FirstOrDefault(x => x.Tag.ToString() == formData.cType_PlaceBirth.Value.ToString()).Selected = true;
            }

            if (formData.mType_PlaceBirth.HasValue)
            {
                mType_PlaceBirth.Items.FirstOrDefault(x => x.Tag.ToString() == formData.mType_PlaceBirth.Value.ToString()).Selected = true;
            }

            if (formData.fType_PlaceBirth.HasValue)
            {
                fType_PlaceBirth.Items.FirstOrDefault(x => x.Tag.ToString() == formData.fType_PlaceBirth.Value.ToString()).Selected = true;
            }


        }

        private void fillCombobox()
        {
            List<string> PunktList = new List<string>();
            List<string> DistrList = new List<string>();
            List<string> RegionList = new List<string>();
            List<string> CountryList = new List<string>();
            List<string> CitizenshipList = new List<string>();

            List<ZAGS_Born> bornList = db.ZAGS_Born.ToList();
            List<ZAGS_Death> deathList = db.ZAGS_Death.ToList();

            PunktList.AddRange(bornList.Select(x => x.cPunkt.Trim()).ToList());
            PunktList.AddRange(bornList.Select(x => x.mPunkt.Trim()).Where(x => !PunktList.Contains(x)).ToList());
            PunktList.AddRange(bornList.Select(x => x.fPunkt.Trim()).Where(x => !PunktList.Contains(x)).ToList());
            PunktList.AddRange(deathList.Select(x => x.PunktBirth.Trim()).Where(x => !PunktList.Contains(x)).ToList());
            PunktList.AddRange(deathList.Select(x => x.PunktLast.Trim()).Where(x => !PunktList.Contains(x)).ToList());

            PunktList = PunktList.Distinct().OrderBy(x => x).ToList();

            cPunkt.DataSource = PunktList.ToList();
            mPunkt.DataSource = PunktList.ToList();
            fPunkt.DataSource = PunktList.ToList();

            cPunkt.SelectedIndex = -1;
            mPunkt.SelectedIndex = -1;
            fPunkt.SelectedIndex = -1;


            DistrList.AddRange(bornList.Select(x => x.cDistr.Trim()).ToList());
            DistrList.AddRange(bornList.Select(x => x.mDistr.Trim()).Where(x => !DistrList.Contains(x)).ToList());
            DistrList.AddRange(bornList.Select(x => x.fDistr.Trim()).Where(x => !DistrList.Contains(x)).ToList());
            DistrList.AddRange(deathList.Select(x => x.DistrBirth.Trim()).Where(x => !DistrList.Contains(x)).ToList());
            DistrList.AddRange(deathList.Select(x => x.DistrLast.Trim()).Where(x => !DistrList.Contains(x)).ToList());

            DistrList = DistrList.Distinct().OrderBy(x => x).ToList();

            cDistr.DataSource = DistrList.ToList();
            mDistr.DataSource = DistrList.ToList();
            fDistr.DataSource = DistrList.ToList();

            cDistr.SelectedIndex = -1;
            mDistr.SelectedIndex = -1;
            fDistr.SelectedIndex = -1;

            RegionList.AddRange(bornList.Select(x => x.cRegion.Trim()).ToList());
            RegionList.AddRange(bornList.Select(x => x.mRegion.Trim()).Where(x => !RegionList.Contains(x)).ToList());
            RegionList.AddRange(bornList.Select(x => x.fRegion.Trim()).Where(x => !RegionList.Contains(x)).ToList());
            RegionList.AddRange(deathList.Select(x => x.RegionBirth.Trim()).Where(x => !RegionList.Contains(x)).ToList());
            RegionList.AddRange(deathList.Select(x => x.RegionLast.Trim()).Where(x => !RegionList.Contains(x)).ToList());

            RegionList = RegionList.Distinct().OrderBy(x => x).ToList();

            cRegion.DataSource = RegionList.ToList();
            mRegion.DataSource = RegionList.ToList();
            fRegion.DataSource = RegionList.ToList();

            cRegion.SelectedIndex = -1;
            mRegion.SelectedIndex = -1;
            fRegion.SelectedIndex = -1;

            CountryList.AddRange(bornList.Select(x => x.cCountry.Trim()).ToList());
            CountryList.AddRange(bornList.Select(x => x.mCountry.Trim()).Where(x => !CountryList.Contains(x)).ToList());
            CountryList.AddRange(bornList.Select(x => x.fCountry.Trim()).Where(x => !CountryList.Contains(x)).ToList());
            CountryList.AddRange(deathList.Select(x => x.CountryBirth.Trim()).Where(x => !CountryList.Contains(x)).ToList());

            CountryList = CountryList.Distinct().OrderBy(x => x).ToList();

            cCountry.DataSource = CountryList.ToList();
            mCountry.DataSource = CountryList.ToList();
            fCountry.DataSource = CountryList.ToList();


            CitizenshipList.AddRange(bornList.Select(x => x.mCitizenship.Trim()).ToList());
            CitizenshipList.AddRange(bornList.Select(x => x.fCitizenship.Trim()).Where(x => !CitizenshipList.Contains(x)).ToList());

            CitizenshipList = CitizenshipList.Distinct().OrderBy(x => x).ToList();

            mCitizenship.DataSource = CitizenshipList.ToList();
            fCitizenship.DataSource = CitizenshipList.ToList();

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            formData = null;
            this.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            GetData();
 //           cleanData = false;

            switch (action)
            {
                case "add":
                    try
                    {

                        db.ZAGS_Born.Add(formData);
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

                        db.Entry(formData).State = EntityState.Modified;
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


            #region // собираем Места рождения

            var fields = typeof(ZAGS_Born).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name).Where(x => x.Contains("Punkt") || x.Contains("Distr") || x.Contains("Region") || x.Contains("Country") || x.Contains("Citizenship"));

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');

                if (this.Controls.Find(itemName, true).Any())
                {
                    formData.GetType().GetProperty(itemName).SetValue(formData, ((Telerik.WinControls.UI.RadDropDownList)this.Controls.Find(itemName, true)[0]).Text.Trim(), null);
                }
            }

            #endregion


            #region // заполняем Текстовые поля
            List<string> textFields = new List<string>{
	"cLastName",
	"cFirstName",
	"cMiddleName",
	"cAkt_Num",
	"cAkt_OrgZags",
	"cSvid_Ser",
	"cSvid_Num",
	"cSvid_OrgZags",
	"mLastName",
	"mFirstName",
	"mMiddleName",
	"fLastName",
	"fFirstName",
	"fMiddleName"};



            fields = typeof(ZAGS_Born).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            names = Array.ConvertAll(fields, field => field.Name);

            foreach (var tf in textFields)
            {
                if (!names.Any(x => x.Contains(tf)))
                    continue;

                var item = names.First(x => x.Contains(tf));

                string itemName = item.TrimStart('_');

                if (this.Controls.Find(itemName, true).Any())
                {
                    formData.GetType().GetProperty(itemName).SetValue(formData, ((Telerik.WinControls.UI.RadTextBox)this.Controls.Find(itemName, true)[0]).Text.Trim(), null);
                }
            }

            #endregion

            if (cAkt_Date.Text == "")
                formData.cAkt_Date = null;
            else
                formData.cAkt_Date = cAkt_Date.Value.Date;

            if (cSvid_Date.Text == "")
                formData.cSvid_Date = null;
            else
                formData.cSvid_Date = cSvid_Date.Value.Date;

            if (SexMRadioButton.IsChecked || SexFRadioButton.IsChecked)
            {
                formData.cSex = SexMRadioButton.IsChecked ? (byte)0 : (byte)1;
            }

            if (!String.IsNullOrEmpty(cDateBirth.Text) && !cDateOs_CheckBox.Checked)
            {
                formData.cDateBirth = cDateBirth.Value;
            }
            else
            {
                formData.cDateBirth = null;
            }

            formData.cType_DateBirth = cDateOs_CheckBox.Checked ? (short)1 : (short)0;
            if (cDateOs_CheckBox.Checked)
            {
                formData.cDateBirthDay_Os = (short)cDayOs_SpinEditor.Value;
                formData.cDateBirthMonth_Os = (short)cMonthOs_SpinEditor.Value;
                formData.cDateBirthYear_Os = (short)cYearOs_SpinEditor.Value;
            }


            if (!String.IsNullOrEmpty(mDateBirth.Text) && !mDateOs_CheckBox.Checked)
            {
                formData.mDateBirth = mDateBirth.Value;
            }
            else
            {
                formData.mDateBirth = null;
            }

            formData.mType_DateBirth = mDateOs_CheckBox.Checked ? (short)1 : (short)0;
            if (mDateOs_CheckBox.Checked)
            {
                formData.mDateBirthDay_Os = (short)mDayOs_SpinEditor.Value;
                formData.mDateBirthMonth_Os = (short)mMonthOs_SpinEditor.Value;
                formData.mDateBirthYear_Os = (short)mYearOs_SpinEditor.Value;
            }


            if (!String.IsNullOrEmpty(fDateBirth.Text) && !fDateOs_CheckBox.Checked)
            {
                formData.fDateBirth = fDateBirth.Value;
            }
            else
            {
                formData.fDateBirth = null;
            }

            formData.fType_DateBirth = fDateOs_CheckBox.Checked ? (short)1 : (short)0;
            if (fDateOs_CheckBox.Checked)
            {
                formData.fDateBirthDay_Os = (short)fDayOs_SpinEditor.Value;
                formData.fDateBirthMonth_Os = (short)fMonthOs_SpinEditor.Value;
                formData.fDateBirthYear_Os = (short)fYearOs_SpinEditor.Value;
            }


            formData.cType_PlaceBirth = short.Parse(cType_PlaceBirth.SelectedItem.Tag.ToString());
            formData.mType_PlaceBirth = short.Parse(mType_PlaceBirth.SelectedItem.Tag.ToString());
            formData.fType_PlaceBirth = short.Parse(fType_PlaceBirth.SelectedItem.Tag.ToString());


        }

        private void Born_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cleanData)
                formData = null;
        }

    }
}
