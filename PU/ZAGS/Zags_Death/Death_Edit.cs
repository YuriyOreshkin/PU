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

namespace PU.ZAGS.Zags_Death
{
    public partial class Death_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action;
        public ZAGS_Death formData { get; set; }
        private bool cleanData = true;


        public Death_Edit()
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
            if (DateBirth.Value != DateBirth.NullDate)
                DateBirth_MaskedEditBox.Text = DateBirth.Value.ToShortDateString();
            else
                DateBirth_MaskedEditBox.Text = DateBirth_MaskedEditBox.NullText;
        }

        private void cDateBirth_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateBirth_MaskedEditBox.Text != DateBirth_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateBirth_MaskedEditBox.Text, out date))
                {
                    DateBirth.Value = date;
                }
                else
                {
                    DateBirth.Value = DateBirth.NullDate;
                }
            }
            else
            {
                DateBirth.Value = DateBirth.NullDate;
            }
        }

        private void cAkt_Date_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (Akt_Date_MaskedEditBox.Text != Akt_Date_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(Akt_Date_MaskedEditBox.Text, out date))
                {
                    Akt_Date.Value = date;
                }
                else
                {
                    Akt_Date.Value = Akt_Date.NullDate;
                }
            }
            else
            {
                Akt_Date.Value = Akt_Date.NullDate;
            }
        }

        private void cAkt_Date_ValueChanged(object sender, EventArgs e)
        {
            if (Akt_Date.Value != Akt_Date.NullDate)
                Akt_Date_MaskedEditBox.Text = Akt_Date.Value.ToShortDateString();
            else
                Akt_Date_MaskedEditBox.Text = Akt_Date_MaskedEditBox.NullText;
        }


        private void fDateBirth_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateDeath_MaskedEditBox.Text != DateDeath_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateDeath_MaskedEditBox.Text, out date))
                {
                    DateDeath.Value = date;
                }
                else
                {
                    DateDeath.Value = DateDeath.NullDate;
                }
            }
            else
            {
                DateDeath.Value = DateDeath.NullDate;
            }
        }

        private void fDateBirth_ValueChanged(object sender, EventArgs e)
        {
            if (DateDeath.Value != DateDeath.NullDate)
                DateDeath_MaskedEditBox.Text = DateDeath.Value.ToShortDateString();
            else
                DateDeath_MaskedEditBox.Text = DateDeath_MaskedEditBox.NullText;
        }

        private void cDayOs_TextBox_Leave(object sender, EventArgs e)
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

        private void cDayOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void cDayOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            DayOs_TextBox.Text = DayOs_SpinEditor.Value == 0 ? "" : DayOs_SpinEditor.Value.ToString();
        }

        private void cMonthOs_TextBox_Leave(object sender, EventArgs e)
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

        private void cMonthOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void cMonthOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            MonthOs_TextBox.Text = MonthOs_SpinEditor.Value == 0 ? "" : MonthOs_SpinEditor.Value.ToString();
        }

        private void cYearOs_TextBox_Leave(object sender, EventArgs e)
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

        private void cYearOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void cYearOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            YearOs_TextBox.Text = YearOs_SpinEditor.Value == 0 ? "" : YearOs_SpinEditor.Value.ToString();
        }








        private void fDayOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(dDayOs_TextBox.Text))
            {
                try
                {
                    dDayOs_SpinEditor.Value = decimal.Parse(dDayOs_TextBox.Text);
                }
                catch
                {
                    dDayOs_TextBox.Text = "";
                }
            }
            else
                dDayOs_SpinEditor.Value = 0;
        }

        private void fDayOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void fDayOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            dDayOs_TextBox.Text = dDayOs_SpinEditor.Value == 0 ? "" : dDayOs_SpinEditor.Value.ToString();
        }

        private void fMonthOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(dMonthOs_TextBox.Text))
            {
                try
                {
                    dMonthOs_SpinEditor.Value = decimal.Parse(dMonthOs_TextBox.Text);
                }
                catch
                {
                    dMonthOs_TextBox.Text = "";
                }
            }
            else
                dMonthOs_SpinEditor.Value = 0;
        }

        private void fMonthOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void fMonthOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            dMonthOs_TextBox.Text = dMonthOs_SpinEditor.Value == 0 ? "" : dMonthOs_SpinEditor.Value.ToString();
        }

        private void fYearOs_TextBox_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(dYearOs_TextBox.Text))
            {
                try
                {
                    dYearOs_SpinEditor.Value = decimal.Parse(dYearOs_TextBox.Text);
                }
                catch
                {
                    dYearOs_TextBox.Text = "";
                }
            }
            else
                dYearOs_SpinEditor.Value = 0;
        }

        private void fYearOs_TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void fYearOs_SpinEditor_ValueChanged(object sender, EventArgs e)
        {
            dYearOs_TextBox.Text = dYearOs_SpinEditor.Value == 0 ? "" : dYearOs_SpinEditor.Value.ToString();
        }

        private void cDateOs_CheckBox_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            DayOs_SpinEditor.Enabled = DateOs_CheckBox.Checked;
            DayOs_TextBox.Enabled = DateOs_CheckBox.Checked;
            MonthOs_SpinEditor.Enabled = DateOs_CheckBox.Checked;
            MonthOs_TextBox.Enabled = DateOs_CheckBox.Checked;
            YearOs_SpinEditor.Enabled = DateOs_CheckBox.Checked;
            YearOs_TextBox.Enabled = DateOs_CheckBox.Checked;

            DateBirth.Enabled = !DateOs_CheckBox.Checked;
            DateBirth_MaskedEditBox.Enabled = !DateOs_CheckBox.Checked;
        }


        private void fDateOs_CheckBox_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            dDayOs_SpinEditor.Enabled = dDateOs_CheckBox.Checked;
            dDayOs_TextBox.Enabled = dDateOs_CheckBox.Checked;
            dMonthOs_SpinEditor.Enabled = dDateOs_CheckBox.Checked;
            dMonthOs_TextBox.Enabled = dDateOs_CheckBox.Checked;
            dYearOs_SpinEditor.Enabled = dDateOs_CheckBox.Checked;
            dYearOs_TextBox.Enabled = dDateOs_CheckBox.Checked;

            DateDeath.Enabled = !dDateOs_CheckBox.Checked;
            DateDeath_MaskedEditBox.Enabled = !dDateOs_CheckBox.Checked;
        }

        private void Death_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            radPageView1.SelectedPage = radPageView1.Pages[0];

            fillCombobox();

            if (action == "edit")
            {
                if (formData != null)
                {
                    formData = db.ZAGS_Death.First(x => x.ID == formData.ID);
                    fillData();
                }
            }
            else if (action == "add")
            {
                formData = new ZAGS_Death();
            }



        }

        private void fillData()
        {

            #region // заполняем Места рождения

            var fields = typeof(ZAGS_Death).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name).Where(x => x.Contains("Punkt") || x.Contains("Distr") || x.Contains("Region") || x.Contains("City") || x.Contains("Street") || x.Contains("Country"));

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
                    if (!String.IsNullOrEmpty(value))
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
	"LastName",
	"FirstName",
	"MiddleName",
	"Akt_Num",
	"Akt_OrgZags",
    "DomDeath",
    "DomDeath_sokr",
    "StroenDeath",
    "StroenDeath_sokr",
    "KorpDeath",
    "KorpDeath_sokr",
    "KvartDeath",
    "KvartDeath_sokr",
    "DomLast",
    "DomLast_sokr",
    "StroenLast",
    "StroenLast_sokr",
    "KorpLast",
    "KorpLast_sokr",
    "KvartLast",
    "KvartLast_sokr"};


            fields = typeof(ZAGS_Death).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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


            if (formData.Akt_Date != null && formData.Akt_Date.HasValue)
                Akt_Date.Value = formData.Akt_Date.Value;


            if (formData.Sex != null && formData.Sex.HasValue)
            {
                if (formData.Sex.Value == 0)
                    SexMRadioButton.IsChecked = true;
                else
                    SexFRadioButton.IsChecked = true;
            }

            if (formData.DateBirth != null && formData.DateBirth.HasValue)
                DateBirth.Value = formData.DateBirth.Value;

            if (formData.Type_DateBirth.HasValue && formData.Type_DateBirth.Value == 1)
            {
                DateOs_CheckBox.Checked = true;
                DayOs_SpinEditor.Value = formData.DateBirthDay_Os.HasValue ? formData.DateBirthDay_Os.Value : 0;
                MonthOs_SpinEditor.Value = formData.DateBirthMonth_Os.HasValue ? formData.DateBirthMonth_Os.Value : 0;
                YearOs_SpinEditor.Value = formData.DateBirthYear_Os.HasValue ? formData.DateBirthYear_Os.Value : 0;
            }




            if (formData.DateDeath != null && formData.DateDeath.HasValue)
                DateDeath.Value = formData.DateDeath.Value;

            //if (formData.Type_DateDeath.HasValue && formData.Type_DateDeath.Value == 1)
            //{
            //    dDateOs_CheckBox.Checked = true;
            //    dDayOs_SpinEditor.Value = formData.DateDeathDay_Os.HasValue ? formData.DateDeathDay_Os.Value : 0;
            //    dMonthOs_SpinEditor.Value = formData.DateDeathMonth_Os.HasValue ? formData.DateDeathMonth_Os.Value : 0;
            //    dYearOs_SpinEditor.Value = formData.DateDeathYear_Os.HasValue ? formData.DateDeathYear_Os.Value : 0;
            //}


            if (formData.Type_PlaceBirth.HasValue)
            {
                Type_PlaceBirth.Items.FirstOrDefault(x => x.Tag.ToString() == formData.Type_PlaceBirth.Value.ToString()).Selected = true;
            }




        }

        private void fillCombobox()
        {
            List<string> PunktList = new List<string>();
            List<string> CityList = new List<string>();
            List<string> StreetList = new List<string>();
            List<string> DistrList = new List<string>();
            List<string> RegionList = new List<string>();
            List<string> CountryList = new List<string>();


            List<string> PunktList_sokr = new List<string>();
            List<string> CityList_sokr = new List<string>();
            List<string> StreetList_sokr = new List<string>();
            List<string> DistrList_sokr = new List<string>();
            List<string> RegionList_sokr = new List<string>();


            List<ZAGS_Born> bornList = db.ZAGS_Born.ToList();
            List<ZAGS_Death> deathList = db.ZAGS_Death.ToList();

            RegionList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.cRegion)).Select(x => x.cRegion.Trim()).ToList());
            RegionList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.mRegion)).Select(x => x.mRegion.Trim()).Where(x => !RegionList.Contains(x)).ToList());
            RegionList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.fRegion)).Select(x => x.fRegion.Trim()).Where(x => !RegionList.Contains(x)).ToList());
            RegionList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.RegionBirth)).Select(x => x.RegionBirth.Trim()).Where(x => !RegionList.Contains(x)).ToList());
            RegionList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.RegionDeath)).Select(x => x.RegionDeath.Trim()).Where(x => !RegionList.Contains(x)).ToList());
            RegionList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.RegionLast)).Select(x => x.RegionLast.Trim()).Where(x => !RegionList.Contains(x)).ToList());

            RegionList = RegionList.Distinct().OrderBy(x => x).ToList();

            RegionBirth.DataSource = RegionList.ToList();
            RegionDeath.DataSource = RegionList.ToList();
            RegionLast.DataSource = RegionList.ToList();

            RegionBirth.SelectedIndex = -1;
            RegionDeath.SelectedIndex = -1;
            RegionLast.SelectedIndex = -1;


            DistrList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.cDistr)).Select(x => x.cDistr.Trim()).ToList());
            DistrList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.mDistr)).Select(x => x.mDistr.Trim()).Where(x => !DistrList.Contains(x)).ToList());
            DistrList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.fDistr)).Select(x => x.fDistr.Trim()).Where(x => !DistrList.Contains(x)).ToList());
            DistrList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.DistrBirth)).Select(x => x.DistrBirth.Trim()).Where(x => !DistrList.Contains(x)).ToList());
            DistrList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.DistrDeath)).Select(x => x.DistrDeath.Trim()).Where(x => !DistrList.Contains(x)).ToList());
            DistrList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.DistrLast)).Select(x => x.DistrLast.Trim()).Where(x => !DistrList.Contains(x)).ToList());

            DistrList = DistrList.Distinct().OrderBy(x => x).ToList();

            DistrBirth.DataSource = DistrList.ToList();
            DistrDeath.DataSource = DistrList.ToList();
            DistrLast.DataSource = DistrList.ToList();

            DistrBirth.SelectedIndex = -1;
            DistrDeath.SelectedIndex = -1;
            DistrLast.SelectedIndex = -1;


            PunktList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.cPunkt)).Select(x => x.cPunkt.Trim()).ToList());
            PunktList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.mPunkt)).Select(x => x.mPunkt.Trim()).Where(x => !PunktList.Contains(x)).ToList());
            PunktList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.fPunkt)).Select(x => x.fPunkt.Trim()).Where(x => !PunktList.Contains(x)).ToList());
            PunktList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.PunktBirth)).Select(x => x.PunktBirth.Trim()).Where(x => !PunktList.Contains(x)).ToList());
            PunktList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.PunktDeath)).Select(x => x.PunktDeath.Trim()).Where(x => !PunktList.Contains(x)).ToList());
            PunktList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.PunktLast)).Select(x => x.PunktLast.Trim()).Where(x => !PunktList.Contains(x)).ToList());

            PunktList = PunktList.Distinct().OrderBy(x => x).ToList();

            PunktBirth.DataSource = PunktList.ToList();
            PunktDeath.DataSource = PunktList.ToList();
            PunktLast.DataSource = PunktList.ToList();

            PunktBirth.SelectedIndex = -1;
            PunktDeath.SelectedIndex = -1;
            PunktLast.SelectedIndex = -1;


            CityList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.CityDeath)).Select(x => x.CityDeath.Trim()).ToList());
            CityList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.CityLast)).Select(x => x.CityLast.Trim()).Where(x => !CityList.Contains(x)).ToList());

            CityList = CityList.Distinct().OrderBy(x => x).ToList();

            CityDeath.DataSource = CityList.ToList();
            CityLast.DataSource = CityList.ToList();

            CityDeath.SelectedIndex = -1;
            CityLast.SelectedIndex = -1;

            StreetList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.StreetDeath)).Select(x => x.StreetDeath.Trim()).ToList());
            StreetList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.StreetLast)).Select(x => x.StreetLast.Trim()).Where(x => !StreetList.Contains(x)).ToList());

            StreetList = StreetList.Distinct().OrderBy(x => x).ToList();

            StreetDeath.DataSource = StreetList.ToList();
            StreetLast.DataSource = StreetList.ToList();

            StreetDeath.SelectedIndex = -1;
            StreetLast.SelectedIndex = -1;


            CountryList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.cCountry)).Select(x => x.cCountry.Trim()).ToList());
            CountryList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.mCountry)).Select(x => x.mCountry.Trim()).Where(x => !CountryList.Contains(x)).ToList());
            CountryList.AddRange(bornList.Where(x => !String.IsNullOrEmpty(x.fCountry)).Select(x => x.fCountry.Trim()).Where(x => !CountryList.Contains(x)).ToList());
            CountryList.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.CountryBirth)).Select(x => x.CountryBirth.Trim()).Where(x => !CountryList.Contains(x)).ToList());

            CountryList = CountryList.Distinct().OrderBy(x => x).ToList();

            CountryBirth.DataSource = CountryList.ToList();

            #region  Сокращения

            RegionList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.RegionDeath_sokr)).Select(x => x.RegionDeath_sokr.Trim()).ToList());
            RegionList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.RegionLast_sokr)).Select(x => x.RegionLast_sokr.Trim()).Where(x => !RegionList_sokr.Contains(x)).ToList());

            RegionList_sokr = RegionList_sokr.Distinct().OrderBy(x => x).ToList();

            RegionDeath_sokr.DataSource = RegionList_sokr.ToList();
            RegionLast_sokr.DataSource = RegionList_sokr.ToList();

            RegionDeath_sokr.SelectedIndex = -1;
            RegionLast_sokr.SelectedIndex = -1;


            DistrList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.DistrDeath_sokr)).Select(x => x.DistrDeath_sokr.Trim()).ToList());
            DistrList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.DistrLast_sokr)).Select(x => x.DistrLast_sokr.Trim()).Where(x => !DistrList_sokr.Contains(x)).ToList());

            DistrList_sokr = DistrList_sokr.Distinct().OrderBy(x => x).ToList();

            DistrDeath_sokr.DataSource = DistrList_sokr.ToList();
            DistrLast_sokr.DataSource = DistrList_sokr.ToList();

            DistrDeath_sokr.SelectedIndex = -1;
            DistrLast_sokr.SelectedIndex = -1;


            PunktList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.PunktDeath_sokr)).Select(x => x.PunktDeath_sokr.Trim()).ToList());
            PunktList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.PunktLast_sokr)).Select(x => x.PunktLast_sokr.Trim()).Where(x => !PunktList_sokr.Contains(x)).ToList());

            PunktList_sokr = PunktList_sokr.Distinct().OrderBy(x => x).ToList();

            PunktDeath_sokr.DataSource = PunktList_sokr.ToList();
            PunktLast_sokr.DataSource = PunktList_sokr.ToList();

            PunktDeath_sokr.SelectedIndex = -1;
            PunktLast_sokr.SelectedIndex = -1;


            CityList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.CityDeath_sokr)).Select(x => x.CityDeath_sokr.Trim()).ToList());
            CityList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.CityLast_sokr)).Select(x => x.CityLast_sokr.Trim()).Where(x => !CityList_sokr.Contains(x)).ToList());

            CityList_sokr = CityList_sokr.Distinct().OrderBy(x => x).ToList();

            CityDeath_sokr.DataSource = CityList_sokr.ToList();
            CityLast_sokr.DataSource = CityList_sokr.ToList();

            CityDeath_sokr.SelectedIndex = -1;
            CityLast_sokr.SelectedIndex = -1;

            StreetList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.StreetDeath_sokr)).Select(x => x.StreetDeath_sokr.Trim()).ToList());
            StreetList_sokr.AddRange(deathList.Where(x => !String.IsNullOrEmpty(x.StreetLast_sokr)).Select(x => x.StreetLast_sokr.Trim()).Where(x => !StreetList_sokr.Contains(x)).ToList());

            StreetList_sokr = StreetList_sokr.Distinct().OrderBy(x => x).ToList();

            StreetDeath_sokr.DataSource = StreetList_sokr.ToList();
            StreetLast_sokr.DataSource = StreetList_sokr.ToList();

            StreetDeath_sokr.SelectedIndex = -1;
            StreetLast_sokr.SelectedIndex = -1;


            #endregion




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

                        db.ZAGS_Death.Add(formData);
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

                        db.Entry(formData).State  = EntityState.Modified;
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

            var fields = typeof(ZAGS_Death).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name).Where(x => x.Contains("Punkt") || x.Contains("Distr") || x.Contains("Region") || x.Contains("City") || x.Contains("Street") || x.Contains("Country"));

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
	"LastName",
	"FirstName",
	"MiddleName",
	"Akt_Num",
	"Akt_OrgZags",
    "DomDeath",
    "DomDeath_sokr",
    "StroenDeath",
    "StroenDeath_sokr",
    "KorpDeath",
    "KorpDeath_sokr",
    "KvartDeath",
    "KvartDeath_sokr",
    "DomLast",
    "DomLast_sokr",
    "StroenLast",
    "StroenLast_sokr",
    "KorpLast",
    "KorpLast_sokr",
    "KvartLast",
    "KvartLast_sokr"};



            fields = typeof(ZAGS_Death).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

            if (Akt_Date.Text == "")
                formData.Akt_Date = null;
            else
                formData.Akt_Date = Akt_Date.Value.Date;


            if (SexMRadioButton.IsChecked || SexFRadioButton.IsChecked)
            {
                formData.Sex = SexMRadioButton.IsChecked ? (byte)0 : (byte)1;
            }

            if (!String.IsNullOrEmpty(DateBirth.Text) && !DateOs_CheckBox.Checked)
            {
                formData.DateBirth = DateBirth.Value;
            }
            else
            {
                formData.DateBirth = null;
            }

            formData.Type_DateBirth = DateOs_CheckBox.Checked ? (short)1 : (short)0;
            if (DateOs_CheckBox.Checked)
            {
                formData.DateBirthDay_Os = (short)DayOs_SpinEditor.Value;
                formData.DateBirthMonth_Os = (short)MonthOs_SpinEditor.Value;
                formData.DateBirthYear_Os = (short)YearOs_SpinEditor.Value;
            }


            if (!String.IsNullOrEmpty(DateDeath.Text) && !dDateOs_CheckBox.Checked)
            {
                formData.DateDeath = DateDeath.Value;
            }
            else
            {
                formData.DateDeath = null;
            }

            formData.Type_DateDeath = dDateOs_CheckBox.Checked ? (short)1 : (short)0;
            if (dDateOs_CheckBox.Checked)
            {
                formData.DateDeathDay_Os = (short)dDayOs_SpinEditor.Value;
                formData.DateDeathMonth_Os = (short)dMonthOs_SpinEditor.Value;
                formData.DateDeathYear_Os = (short)dYearOs_SpinEditor.Value;
            }


            formData.Type_PlaceBirth = short.Parse(Type_PlaceBirth.SelectedItem.Tag.ToString());



        }

        private void Death_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cleanData)
                formData = null;
        }

        private void DateDeath_MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateDeath_MaskedEditBox.Text != DateDeath_MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateDeath_MaskedEditBox.Text, out date))
                {
                    DateDeath.Value = date;
                }
                else
                {
                    DateDeath.Value = DateDeath.NullDate;
                }
            }
            else
            {
                DateDeath.Value = DateDeath.NullDate;
            }
        }

        private void DateDeath_ValueChanged(object sender, EventArgs e)
        {
            if (DateDeath.Value != DateDeath.NullDate)
                DateDeath_MaskedEditBox.Text = DateDeath.Value.ToShortDateString();
            else
                DateDeath_MaskedEditBox.Text = DateDeath_MaskedEditBox.NullText;
        }

        private void addAbbrDeath_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (addAbbrDeath.Checked)
            {
                DomDeath_sokr.Text = "Д.";
                StroenDeath_sokr.Text = "СТР.";
                KorpDeath_sokr.Text = "КОРП.";
                KvartDeath_sokr.Text = "КВ.";
            }

            DomDeath_sokr.Enabled = !addAbbrDeath.Checked;
            StroenDeath_sokr.Enabled = !addAbbrDeath.Checked;
            KorpDeath_sokr.Enabled = !addAbbrDeath.Checked;
            KvartDeath_sokr.Enabled = !addAbbrDeath.Checked;

        }

        private void addAbbrLast_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (addAbbrLast.Checked)
            {
                DomLast_sokr.Text = "Д.";
                StroenLast_sokr.Text = "СТР.";
                KorpLast_sokr.Text = "КОРП.";
                KvartLast_sokr.Text = "КВ.";
            }

            DomLast_sokr.Enabled = !addAbbrLast.Checked;
            StroenLast_sokr.Enabled = !addAbbrLast.Checked;
            KorpLast_sokr.Enabled = !addAbbrLast.Checked;
            KvartLast_sokr.Enabled = !addAbbrLast.Checked;
        }




    }
}
