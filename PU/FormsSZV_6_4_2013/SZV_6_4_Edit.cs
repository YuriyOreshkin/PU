using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using Telerik.WinControls.UI;
using PU.Models;
using System.Reflection;
using PU.FormsRSW2014;

namespace PU.FormsSZV_6_4_2013
{
    public partial class SZV_6_4_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private List<ErrList> errMessBox = new List<ErrList>();
        public Staff staff { get; set; }
        public byte period { get; set; }
        public byte CorrNum { get; set; }
        public bool autoCalc { get; set; }
        public PlatCategory PlatCat { get; set; }
        public FormsSZV_6_4 SZV_6_4 { get; set; }
        public List<FormsSZV_6_4> formDataPrev { get; set; }
        private string textBoxContents = string.Empty;
        List<RaschetPeriodContainer> avail_periods_all = new List<RaschetPeriodContainer>();

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

        public SZV_6_4_Edit()
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            InitializeComponent();
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    radButton1.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    break;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SZV_6_4_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            this.radPageView1.SelectedPage = this.radPageView1.Pages[0];

            checkAccessLevel();

            dateBirthAlertLabel.Visible = !staff.DateBirth.HasValue;


            string contrNum = "";
            if (staff.ControlNumber != null)
            {
                contrNum = staff.ControlNumber.HasValue ? staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
            }
            string SNILS = !String.IsNullOrEmpty(staff.InsuranceNumber) ? staff.InsuranceNumber.Substring(0, 3) + "-" + staff.InsuranceNumber.Substring(3, 3) + "-" + staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : "";

            FIOLabel.Text = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + "   [" + SNILS + "]";

            foreach (var item in db.TypeInfo)
            {
                TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }

            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal2010_2013.Where(x => x.Year == 2013).OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }


            this.Year.Items.Clear();

            foreach (var item in avail_periods_all.Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }

            short y;

            switch (action)
            {
                case "add":
                    AutoCalcSwitch.IsOn = true;
                    SZV_6_4 = new FormsSZV_6_4();
                    DateFilling.EditValue = DateTime.Now;

                    #region Отчетный период
                    long staffid = staff.ID;

                    staff = new Staff();
                    staff = db.Staff.FirstOrDefault(x => x.ID == staffid);

                    // выпад список "календарный год"


                    if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                        Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                    else
                        Year.Items.OrderByDescending(x => x.Value).First().Selected = true;

                    // выпад список "Отчетный период"

                    this.Quarter.Items.Clear();

                    if (short.TryParse(Year.SelectedItem.Text, out y))
                    {
                        foreach (var item in avail_periods_all.Where(x => x.Year == y).ToList()) //  && x.Kvartal != 0
                        {
                            Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                        }
                        DateTime dt = DateTime.Now.AddDays(-45);

                        RaschetPeriodContainer rp = new RaschetPeriodContainer();

                        if (avail_periods_all.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y))
                            rp = avail_periods_all.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y);
                        else
                            rp = avail_periods_all.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y);

                        if (rp != null)
                            Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                        else
                            Quarter.Items.OrderByDescending(x => x.Value).First().Selected = true;
                    }
                    setPeriod();
                    #endregion


                    TypeInfo.Items.Single(x => x.Text == "Исходная").Selected = true;
                    break;
                case "edit":
                    DateFilling.EditValue = SZV_6_4.DateFilling;

                    #region Отчетный период

                    // выпад список "календарный год"

                    if (Year.Items.Any(x => x.Text.ToString() == SZV_6_4.Year.ToString()))
                        Year.Items.Single(x => x.Text.ToString() == SZV_6_4.Year.ToString()).Selected = true;
                 //   else
                 //       Year.Items.OrderByDescending(x => x.Value).First().Selected = true;


                    // выпад список "Отчетный период"

                    this.Quarter.Items.Clear();

                    if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y))
                    {
                        foreach (var item in avail_periods_all.Where(x => x.Year == y).ToList())
                        {
                            Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                        }
                        if (Quarter.Items.Any(x => x.Value.ToString() == SZV_6_4.Quarter.ToString()))
                            Quarter.Items.FirstOrDefault(x => x.Value.ToString() == SZV_6_4.Quarter.ToString()).Selected = true;
         
                    }
                    #endregion

                    TypeContract.Items.Single(x => x.Tag.ToString() == SZV_6_4.TypeContract.ToString()).Selected = true;
                    TypeInfo.Items.Single(x => x.Value.ToString() == SZV_6_4.TypeInfoID.ToString()).Selected = true;
                    change_TypeInfo();

                    if (SZV_6_4.QuarterKorr != null && KorrQuarter.Items.Any(x => x.Value.ToString() == SZV_6_4.QuarterKorr.ToString()))
                        KorrQuarter.Items.Single(x => x.Value.ToString() == SZV_6_4.QuarterKorr.ToString()).Selected = true;


                    PlatCat = SZV_6_4.PlatCategory;

                    if (PlatCat != null)
                    {
                        platCategoryPanel.Text = PlatCat.Code + "   " + PlatCat.Name;
                    }
                    else
                    {
                        platCategoryPanel.Text = "Категория плательщика - не определена... Нажмите для выбора";
                    }


                    //Информация о сотруднике
                    staff = SZV_6_4.Staff;

                    AutoCalcSwitch.IsOn = SZV_6_4.AutoCalc.HasValue ? SZV_6_4.AutoCalc.Value : true;
                    updateTextBoxes();


                    break;
            }

            getBase();
            getPrevData();

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            this.KorrYear.SelectedIndexChanged += (s, с) => KorrYear_SelectedIndexChanged();
            this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();


            long id_old = 0;
            switch (action)
            {
                case "add":

                    break;
                case "edit":
                    id_old = SZV_6_4.ID;
                    SZV_6_4 = new FormsSZV_6_4 { ID = id_old };
                    break;
            }

            this.s_1_0.Leave += (s, c) => CalcTextBoxes(s, c);
            this.s_1_1.Leave += (s, c) => CalcTextBoxes1(s, c);
            this.s_1_2.Leave += (s, c) => CalcTextBoxes2(s, c);
            this.s_2_0.Leave += (s, c) => CalcTextBoxes(s, c);
            this.s_2_1.Leave += (s, c) => CalcTextBoxes1(s, c);
            this.s_2_2.Leave += (s, c) => CalcTextBoxes2(s, c);
            this.s_3_0.Leave += (s, c) => CalcTextBoxes(s, c);
            this.s_3_1.Leave += (s, c) => CalcTextBoxes1(s, c);
            this.s_3_2.Leave += (s, c) => CalcTextBoxes2(s, c);

        }

        private void Quarter_SelectedIndexChanged()
        {
            if (Quarter.SelectedItem != null)
                setPeriod();
        }

        private void getPrevData()
        {
            short year;
            formDataPrev = new List<FormsSZV_6_4>();

            if (PlatCat != null && Year.Text != "" && TypeInfo.SelectedIndex == 0)
            {
                if (short.TryParse(Year.Text, out year))
                {
                    byte q = period;
                    if (q != 1) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                    {
                        for (byte i = 1; i < q; i++)
                        {
                            byte tc = byte.Parse(TypeContract.SelectedItem.Tag.ToString());
                            if (db.FormsSZV_6_4.Any(x => x.Year == year && x.Quarter == i && x.StaffID == staff.ID && x.PlatCategoryID == PlatCat.ID && x.TypeInfoID == 1 && x.TypeContract == tc))
                            {
                                formDataPrev.Add(db.FormsSZV_6_4.FirstOrDefault(x => x.Year == year && x.Quarter == i && x.StaffID == staff.ID && x.PlatCategoryID == PlatCat.ID && x.TypeInfoID == 1 && x.TypeContract == tc));
                            }
                        }
                    }
                }
            }
            calcFee();
        }

        private void updateTextBoxes()
        {
            var fields = typeof(FormsSZV_6_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_") || itemName.StartsWith("d_") || itemName.StartsWith("Sum"))
                {
              //      DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                    if (SZV_6_4 != null)
                    {
                        var properties = SZV_6_4.GetType().GetProperty(itemName);
                        object value = properties.GetValue(SZV_6_4, null);

                        string type = properties.PropertyType.FullName;
                        if (type.Contains("["))
                            type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                        type = type.Split(',')[0].Split('.')[1].ToLower();

                        if (value != null)
                        {
                            switch (type)
                            {
                                case "decimal":
                                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = decimal.Parse(value.ToString());
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "decimal":
                                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = (decimal)0;
                                    break;
                            }
                            
                        }
                    }
                }



            }

        }

        /// <summary>
        /// Обнуление 2 и 3 раздела и начисленных взносов
        /// </summary>
        private void updateTextBoxesSetZero()
        {
            var fields = typeof(FormsSZV_6_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_") || itemName.StartsWith("d_") || itemName.EndsWith("_Strah") || itemName.EndsWith("_Nakop"))
                {
                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = (decimal)0;
                }
            }
        }

        private void setPeriod()
        {
            short y;
            if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y))
            {
                byte q;
                if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                {
                    period = q;
                }
            }

            getPrevData();
        }

        private void Year_SelectedIndexChanged()
        {
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            this.Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                var avail_periods = avail_periods_all.Where(x => x.Year == y);
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
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

                setPeriod();
            }
            getBase();

        }

        private void change_TypeInfo()
        {
            if (TypeInfo.SelectedIndex == 0)
            {
                KorrYear.Items.Clear();
                KorrQuarter.Items.Clear();
                KorrRegNum.ResetText();
                KorrYear.Enabled = false;
                KorrQuarter.Enabled = false;
                KorrRegNum.Enabled = false;
                korrFeeGroup.Enabled = false;
                feeGroup.Enabled = true;
                radPageView1.Pages[2].Enabled = true;
                radPageView1.Pages[3].Enabled = true;

            }
            else
            {
                KorrYear.Enabled = true;
                KorrQuarter.Enabled = true;
                KorrRegNum.Enabled = true;
                korrFeeGroup.Enabled = true;

                if (TypeInfo.SelectedIndex == 1)
                {
                    radPageView1.Pages[2].Enabled = true;
                    radPageView1.Pages[3].Enabled = true;
                    feeGroup.Enabled = true;
                }
                else if (TypeInfo.SelectedIndex == 2)
                {
                    radPageView1.Pages[2].Enabled = false;
                    radPageView1.Pages[3].Enabled = false;
                    feeGroup.Enabled = false;
                }

                var avail_periods = avail_periods_all.Where(x => x.Year == 2013).OrderBy(x => x.Year);

                // выпад список "календарный год"

                this.KorrYear.Items.Clear();

                foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
                {
                    KorrYear.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                }


                switch (action)
                {
                    case "add":
                        if (KorrYear.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                            KorrYear.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                        break;
                    case "edit":
                        if (SZV_6_4.YearKorr != null)
                            KorrYear.Items.Single(x => x.Text.ToString() == SZV_6_4.YearKorr.ToString()).Selected = true;
                        else if (KorrYear.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                            KorrYear.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;


                        if (SZV_6_4.RegNumKorr != null)
                        {
                            KorrRegNum.Value = SZV_6_4.RegNumKorr.ToString();
                        }

                        break;
                }

                KorrYear_SelectedIndexChanged();

            }
        }

        private void KorrYear_SelectedIndexChanged()
        {
            // выпад список "Отчетный период"
            var avail_periods = avail_periods_all.Where(x => x.Year == 2013);

            byte q = 20;
            if (KorrQuarter.SelectedItem != null && byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out q)) { }
  
            this.KorrQuarter.Items.Clear();

            short y;
            if (KorrYear.SelectedItem != null && short.TryParse(KorrYear.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList()) //  && x.Kvartal != 0
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
            getBase();

        }


        private void getBase()
        {

            if (TypeInfo.SelectedIndex == 0 && !String.IsNullOrEmpty(Year.Text))
            {
                short y;

                if (short.TryParse(Year.Text, out y))
                {
                    if (db.MROT.Any(x => x.Year == y))
                        Options.mrot = db.MROT.First(x => x.Year == y);
                    else
                        Options.mrot = null;
                }
            }
            else if (TypeInfo.SelectedIndex > 0 && !String.IsNullOrEmpty(KorrYear.Text))
            {
                short y;

                if (short.TryParse(KorrYear.Text, out y))
                {
                    if (db.MROT.Any(x => x.Year == y))
                        Options.mrot = db.MROT.First(x => x.Year == y);
                    else
                        Options.mrot = null;
                }
            }
            else
            {
                Options.mrot = null;
            }
            if (Options.mrot != null)
                radLabel17.Text = "Максимальная база: " + Options.mrot.NalogBase.ToString() + " рублей в " + Options.mrot.Year.ToString() + " году";
            else
                radLabel17.Text = "Максимальная база - не определена...";
        }

        private void platCategoryPanel_MouseHover(object sender, EventArgs e)
        {
            platCategoryPanel.Font = new Font(platCategoryPanel.Font, platCategoryPanel.Font.Style | FontStyle.Underline);

        }

        private void platCategoryPanel_MouseLeave(object sender, EventArgs e)
        {
            platCategoryPanel.Font = new Font(platCategoryPanel.Font, FontStyle.Bold);
        }

        private void platCategoryPanel_MouseClick(object sender, MouseEventArgs e)
        {
            PlatCategoryFrm child = new PlatCategoryFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.PlatCat = PlatCat;
            child.radDropDownList1.Enabled = false;
            child.action = "selection";
            child.ShowDialog();
            PlatCat = child.PlatCat;

            if (PlatCat != null)
            {
                platCategoryPanel.Text = PlatCat.Code + "   " + PlatCat.Name;
            }
            else
            {
                platCategoryPanel.Text = "Категория плательщика - не определена... Нажмите для выбора";
            }
            getPrevData();

        }

        private void TypeInfo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            change_TypeInfo();
            getPrevData();

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (PlatCat == null)
            {
                platCategoryPanel_MouseClick(null, null);
                if (PlatCat == null)
                    return;
            }

            if (validation())
            {
                if (TypeInfo.SelectedIndex == 2)  // если выбрана отменяющая форма, то обнуляется 2 и 3 раздел и начисленные взносы
                    updateTextBoxesSetZero();
                savingForm();
            }
            else
            {
                if (errMessBox.Count != 0)
                {
                    if (errMessBox.Any(x => x.type == "error"))
                    {
                        ErrList err = errMessBox.FirstOrDefault(x => x.type == "error");
                        RadMessageBox.Show(this, err.name, "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        this.Controls.Find(err.control, true)[0].Select();

                    }
                    else
                    {
                        DialogResult ds = Telerik.WinControls.RadMessageBox.Show(this, errMessBox[0].name + "\r\nСохранить \"как есть\"?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        switch (ds)
                        {
                            case DialogResult.Yes:

                                savingForm();
                                break;
                            case DialogResult.No:
                                break;
                        }
                        
                    }
                }
            }
        }

        /// <summary>
        /// Сбор данных с основной экранной формы редактировния формы РСВ-1
        /// </summary>
        private void getValues()
        {
            var fields = typeof(FormsSZV_6_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_") || itemName.StartsWith("d_") || itemName.StartsWith("Sum"))
                {
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                         //   DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                            string type = SZV_6_4.GetType().GetProperty(itemName).PropertyType.FullName;
                            if (type.Contains("["))
                                type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                            type = type.Split(',')[0].Split('.')[1].ToLower();
                            if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text != "")
                            {
                                switch (type)
                                {
                                    case "decimal":
                                        SZV_6_4.GetType().GetProperty(itemName).SetValue(SZV_6_4, Math.Round(decimal.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text), 2, MidpointRounding.AwayFromZero), null);
                                        break;
                                }
                            }
                            else
                                SZV_6_4.GetType().GetProperty(itemName).SetValue(SZV_6_4, null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message + "    " + itemName);
                    }
                }

            }
        }


        private void savingForm()
        {
            bool flag_ok = false;
            try
            {
                getValues();
                flag_ok = true;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы СЗВ-6-4. Код ошибки: " + ex.Message);
            }

            if (flag_ok)
                try
                {
                    flag_ok = false;

                    SZV_6_4.InsurerID = Options.InsID;
                    SZV_6_4.StaffID = staff.ID;
                    SZV_6_4.Year = short.Parse(Year.Text);
                    SZV_6_4.Quarter = period;
                    SZV_6_4.DateFilling = DateFilling.DateTime.Date;
                    SZV_6_4.PlatCategoryID = PlatCat.ID;
                    SZV_6_4.TypeInfoID = long.Parse(TypeInfo.SelectedItem.Value.ToString());
                    SZV_6_4.TypeContract = byte.Parse(TypeContract.SelectedItem.Tag.ToString());
                    SZV_6_4.AutoCalc = AutoCalcSwitch.IsOn;

                    if (TypeInfo.SelectedIndex != 0)
                    {
                        string regN = "";
                        if (KorrRegNum.Value.ToString() != "___-___-______")
                        {
                            var s = (this.KorrRegNum.Value.ToString()).Split('-');
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
                        }

                        SZV_6_4.RegNumKorr = regN;

                        SZV_6_4.YearKorr = short.Parse(KorrYear.SelectedItem.Text);
                        SZV_6_4.QuarterKorr = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
                    }

                    flag_ok = true;
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка во время сохранения данных с главного окна редактирования формы СЗВ-6-4. Код ошибки: " + ex.Message);
                }


            if (flag_ok)
                switch (action)
                {
                    //новая записи формы РСВ-2
                    case "add":
                        try
                        {
                            flag_ok = false;
                            #region Сохранение новой записи
                            db.FormsSZV_6_4.Add(SZV_6_4);
                            db.SaveChanges();

                            flag_ok = true;
                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранении данных произошла ошибка. Код ошибки: " + ex.Message);
                        }
                        if (flag_ok)
                            this.Close();
                            #endregion

                        break;
                    //изменение записи формы РСВ-2
                    case "edit":
                        // выбираем из базы исходную запись по идешнику
                        FormsSZV_6_4 r1 = db.FormsSZV_6_4.FirstOrDefault(x => x.ID == SZV_6_4.ID);
                        try
                        {
                            var fields = typeof(FormsSZV_6_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            foreach (var itemName_ in names)
                            {
                                string itemName = itemName_.TrimStart('_');
                                var properties = SZV_6_4.GetType().GetProperty(itemName);
                                if (properties != null)
                                {
                                    object value = properties.GetValue(SZV_6_4, null);
                                    var data = value;

                                    r1.GetType().GetProperty(itemName).SetValue(r1, data, null);
                                }

                            }


                            // сохраняем модифицированную запись обратно в бд
                            db.Entry(r1).State = EntityState.Modified;
                            //                                db.SaveChanges();
                            flag_ok = true;

                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранении основных данных произошла ошибка. Код ошибки: " + ex.Message);
                        }

                        if (flag_ok)
                        {
                            try
                            {
                                db.SaveChanges();
                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                flag_ok = false;
                                RadMessageBox.Show("При сохранении данных произошла ошибка. Код ошибки: " + ex.Message);
                            }
                            if (flag_ok)
                                this.Close();
                        }

                        break;
                }

        }


        /// <summary>
        /// Проверка введенных данных формы СЗВ-6-4
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            if (Year.Text == "")
            {
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Year" , type = "error"});
                return false;
            }

            if (SumFeePFR_Strah.Text == "0.00")
            {
                errMessBox.Add(new ErrList { name = "Не внесены страховые взносы", control = "SumFeePFR_Strah", type = "warning" });
            }

            long InsID = Options.InsID;

            short y = short.Parse(Year.Text);
            byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
            byte tc = byte.Parse(TypeContract.SelectedItem.Tag.ToString());

            switch (action)
            {
                case "add":
                    if (TypeInfo.SelectedIndex == 0) // если исходная запись
                    {
                        if (db.FormsSZV_6_4.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCat.ID && x.TypeContract == tc))
                        {
                            errMessBox.Add(new ErrList { name = "Дублирование записи по ключу уникальности (сотрудник, отчетный период, категория плательщика, тип договора)", control = "Year", type = "error"});
                        }
                    }
                    else
                    {
                        if (KorrYear.Text == "")
                        {
                            errMessBox.Add(new ErrList { name = "Не выбран корректируемый отчетный период", control = "KorrYear" , type = "error"});
                            return false;
                        }
                        if (KorrQuarter.Text == "")
                        {
                            errMessBox.Add(new ErrList { name = "Не выбран корректируемый отчетный период", control = "KorrQuarter" , type = "error"});
                            return false;
                        }

                        short yk = short.Parse(KorrYear.Text);
                        byte qk = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());

                        if (db.FormsSZV_6_4.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCat.ID && x.TypeContract == tc && x.YearKorr == yk && x.QuarterKorr == qk))
                        {
                            errMessBox.Add(new ErrList { name = "Дублирование записи по ключу уникальности (сотрудник, отчетный период, корректируемый отчетный период, категория плательщика, тип договора)", control = "Year", type = "error" });
                        }

                    }

                        break;
                case "edit":
                        if (TypeInfo.SelectedIndex == 0) // если исходная запись
                        {
                            if (db.FormsSZV_6_4.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCat.ID && x.TypeContract == tc && x.ID != SZV_6_4.ID))
                            {
                                errMessBox.Add(new ErrList { name = "Дублирование записи по ключу уникальности (сотрудник, отчетный период, категория плательщика, тип договора)", control = "Year", type = "error" });
                            }
                        }
                        else
                        {
                            if (KorrYear.Text == "")
                            {
                                errMessBox.Add(new ErrList { name = "Не выбран корректируемый отчетный период", control = "KorrYear", type = "error" });
                                return false;
                            }
                            if (KorrQuarter.Text == "")
                            {
                                errMessBox.Add(new ErrList { name = "Не выбран корректируемый отчетный период", control = "KorrQuarter", type = "error" });
                                return false;
                            }

                            short yk = short.Parse(KorrYear.Text);
                            byte qk = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());

                            if (db.FormsSZV_6_4.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCat.ID && x.TypeContract == tc && x.YearKorr == yk && x.QuarterKorr == qk && x.ID != SZV_6_4.ID))
                            {
                                errMessBox.Add(new ErrList { name = "Дублирование записи по ключу уникальности (сотрудник, отчетный период, корректируемый отчетный период, категория плательщика, тип договора)", control = "Year", type = "error" });
                            }

                        }
                    break;
            }

            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        private void s_0_0_Enter(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)sender;

            textBoxContents = box.Text.ToString();
        }

        private void CalcTextBoxes(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit box_old = (DevExpress.XtraEditors.TextEdit)sender;
            if (textBoxContents == box_old.Text.ToString())
                return;
            else
            {
                decimal max = Options.mrot != null ? Options.mrot.NalogBase : 0;
                s_0_0.EditValue = (decimal)0;
                if (copyToOPSSum.Checked) // если выбрано копировать суммы на ОПС
                {
                    s_0_1.EditValue = (decimal)0;
                }

                if (copyToOPSAkt.Checked) // если выбрано копировать суммы на ОПС по гражданско-правовым актам
                {
                    s_0_2.EditValue = (decimal)0;
                }

                for (int i = 1; i <= 3; i++)
                {
                    List<String> name = new List<string>();
                    List<DevExpress.XtraEditors.TextEdit> box = new List<DevExpress.XtraEditors.TextEdit>();
                    for (var j = 0; j <= 2; j++)
                    {
                        name.Add("s_" + i + "_" + j);
                        box.Add((DevExpress.XtraEditors.TextEdit)this.Controls.Find(name[j], true)[0]);
                    }

                    s_0_0.EditValue = decimal.Parse(s_0_0.EditValue.ToString()) + decimal.Parse(box[0].EditValue.ToString());


                    if (copyToOPSSum.Checked) // если выбрано копировать суммы на ОПС
                    {
                        box[1].EditValue = decimal.Parse(box[0].EditValue.ToString());
                        s_0_1.EditValue = decimal.Parse(s_0_1.EditValue.ToString()) + decimal.Parse(box[1].EditValue.ToString());
                    }




                    if (copyToOPSAkt.Checked) // если выбрано копировать суммы на ОПС по гражданско-правовым актам
                    {
                        box[2].EditValue = box[1].EditValue;
                        s_0_2.EditValue = decimal.Parse(s_0_2.EditValue.ToString()) + decimal.Parse(box[2].EditValue.ToString());
                    }

                }

                calcFee();
            }

        }

        private void CalcTextBoxes1(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit box_old = (DevExpress.XtraEditors.TextEdit)sender;
            if (textBoxContents == box_old.Text.ToString())
                return;
            else
            {
                s_0_1.EditValue = (decimal)0;
                for (int i = 1; i <= 3; i++)
                {
                    List<String> name = new List<string>();
                    DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find("s_" + i + "_1", true)[0];

                    s_0_1.EditValue = decimal.Parse(s_0_1.EditValue.ToString()) + decimal.Parse(box.EditValue.ToString());


                }
                calcFee();

            }

        }

        private void CalcTextBoxes2(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit box_old = (DevExpress.XtraEditors.TextEdit)sender;
            if (textBoxContents == box_old.Text.ToString())
                return;
            else
            {
                s_0_2.EditValue = (decimal)0;
                for (int i = 1; i <= 3; i++)
                {
                    List<String> name = new List<string>();
                    DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find("s_" + i + "_2", true)[0];
                    
                    s_0_2.EditValue = decimal.Parse(s_0_2.EditValue.ToString()) + decimal.Parse(box.EditValue.ToString());


                }
            }

        }

        private void CalcTextBoxes_D(object sender, EventArgs e)
        {
                d_0_0.EditValue = decimal.Parse(d_1_0.EditValue.ToString()) + decimal.Parse(d_2_0.EditValue.ToString()) + decimal.Parse(d_3_0.EditValue.ToString());
                d_0_1.EditValue = decimal.Parse(d_1_1.EditValue.ToString()) + decimal.Parse(d_2_1.EditValue.ToString()) + decimal.Parse(d_3_1.EditValue.ToString());
        }

        private void calcFee()
        {
            if (!AutoCalcSwitch.IsOn)
            {
                decimal sumS = 0;
                decimal sumN = 0;

                if (PlatCat != null && Year.Text != "")
                {

                    short y = short.Parse(Year.Text);
                    if (TypeInfo.SelectedItem.Value.ToString() == "2" && KorrYear.SelectedItem != null)
                    {
                        short.TryParse(KorrYear.SelectedItem.Text, out y);
                    }

                    if ((PlatCat.TariffPlat.Any(x => x.Year == y)) && (PlatCat.TariffPlat.First(x => x.Year == y).StrahPercant1966 != null) && (PlatCat.TariffPlat.First(x => x.Year == y).StrahPercent1967 != null) && (PlatCat.TariffPlat.First(x => x.Year == y).NakopPercant != null))
                    {
                        decimal s1 = s_1_1 != null ? decimal.Parse(s_1_1.EditValue.ToString()) : 0;
                        decimal s2 = s_2_1 != null ? decimal.Parse(s_2_1.EditValue.ToString()) : 0;
                        decimal s3 = s_3_1 != null ? decimal.Parse(s_3_1.EditValue.ToString()) : 0;

                        decimal sumAll = s1 + s2 + s3;

                        decimal sumPrev = 0;
                        foreach (var item in formDataPrev)
                        {
                            sumPrev = sumPrev + item.s_0_0.Value;
                        }

                        if (Options.mrot != null)
                        {
                            if (sumPrev <= Options.mrot.NalogBase) // если за прошлые периоды не превышает макс базу
                            {
                                sumAll = (sumAll + sumPrev) <= Options.mrot.NalogBase ? sumAll : (Options.mrot.NalogBase - sumPrev);
                            }
                            else //если превышает
                            {
                                sumAll = 0;
                            }
                        }
                        else
                            sumAll = 0;

                        if (staff.DateBirth.HasValue) //если есть дата рождения, тогда проверяем год рождения
                        {
                            int YearB = staff.DateBirth.Value.Year;
                            if (YearB < 1967)
                            {
                                sumS = sumS + (sumAll * PlatCat.TariffPlat.First(x => x.Year == y).StrahPercant1966.Value / 100);
                            }
                            else
                            {
                                sumS = sumS + (sumAll * PlatCat.TariffPlat.First(x => x.Year == y).StrahPercent1967.Value / 100);
                                sumN = sumN + (sumAll * PlatCat.TariffPlat.First(x => x.Year == y).NakopPercant.Value / 100);
                            }
                        }
                        else
                        {
                            sumS = sumS + (sumAll * PlatCat.TariffPlat.First(x => x.Year == y).StrahPercant1966.Value / 100);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Категория плательщика: " + PlatCat.Code + "\r\nНе определен тариф Страховых взносов\r\nРасчет взносов по этой категории производиться не будет!");
                    }
                }

                SumFeePFR_Strah.EditValue = sumS;
                SumFeePFR_Nakop.EditValue = sumN;
            }
        }

        private void TypeContract_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            getPrevData();
        }

        private void reCalcFeeBtn_Click(object sender, EventArgs e)
        {
            AutoCalcSwitch.IsOn = true;
            calcFee();
            AutoCalcSwitch.IsOn = false;
        }

        private void AutoCalcSwitch_Toggled(object sender, EventArgs e)
        {
            calcFee();
        }

        private void calcFeeD_Click(object sender, EventArgs e)
        {
            SumFeePFR_Strah_D.EditValue = (decimal)0;
            SumFeePFR_Nakop_D.EditValue = (decimal)0;
            short y = 0;
            if (PlatCat != null && short.TryParse(KorrYear.SelectedItem.Text, out y))
            {
                byte q = 0;
                if (byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out q))
                {
                    if (db.FormsSZV_6_4.Any(x => x.Year == y && x.Quarter == q && x.TypeInfoID == 1 && x.StaffID == staff.ID && x.PlatCategoryID == PlatCat.ID))
                    {
                        FormsSZV_6_4 is_ishod = db.FormsSZV_6_4.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.TypeInfoID == 1 && x.StaffID == staff.ID && x.PlatCategoryID == PlatCat.ID);

                        if (is_ishod.SumFeePFR_Strah.HasValue)
                        {
                            SumFeePFR_Strah_D.EditValue = decimal.Parse(SumFeePFR_Strah.EditValue.ToString()) - is_ishod.SumFeePFR_Strah.Value;
                        }
                        if (is_ishod.SumFeePFR_Nakop.HasValue)
                        {
                            SumFeePFR_Nakop_D.EditValue = decimal.Parse(SumFeePFR_Nakop.EditValue.ToString()) - is_ishod.SumFeePFR_Nakop.Value;
                        }
                    }
                }
            }
        }
 
    }
}
