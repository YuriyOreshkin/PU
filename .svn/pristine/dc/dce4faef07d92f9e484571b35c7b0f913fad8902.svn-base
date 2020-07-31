using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace PU.FormsSZV_6_2010
{
    public partial class SZV_6_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private List<ErrList> errMessBox = new List<ErrList>();
        public Staff staff { get; set; }
        public byte period { get; set; }
        public byte CorrNum { get; set; }
        public PlatCategory PlatCat { get; set; }
        public FormsSZV_6 SZV_6 { get; set; }
        public List<FormsSZV_6> formDataPrev { get; set; }
        private string textBoxContents = string.Empty;
        private List<int> months = new List<int>();
        List<RaschetPeriodContainer> avail_periods_all = new List<RaschetPeriodContainer>();



        public SZV_6_Edit()
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

        private void Quarter_SelectedIndexChanged()
        {
            if (Quarter.SelectedItem != null)
                setPeriod();
        }

        private void KorrQuarter_SelectedIndexChanged()
        {
            if (KorrQuarter.SelectedItem != null)
                setPeriod();
        }

        private void updateTextBoxes()
        {


            var fields = typeof(FormsSZV_6).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_") || itemName.ToUpper().StartsWith("SUM"))
                {
                    //       DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                    if (SZV_6 != null)
                    {
                        var properties = SZV_6.GetType().GetProperty(itemName);
                        object value = properties.GetValue(SZV_6, null);

                        string type = properties.PropertyType.FullName;
                        if (type.Contains("["))
                            type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                        type = type.Split(',')[0].Split('.')[1].ToLower();

                        if (value != null)
                        {
                            switch (type)
                            {
                                case "decimal":
                                    if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]) != null)
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
        /// Обнуление сумм
        /// </summary>
        private void updateTextBoxesSetZero(bool setZeroAll)
        {
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    string itemName = "s_" + i.ToString() + "_" + j.ToString();
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                            //   DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];
                            if (setZeroAll)
                            {
                                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = (decimal)0;
                                SUMTAXYEAR.EditValue = (decimal)0;
                                SumPayPFR_Strah.EditValue = (decimal)0;
                                SumPayPFR_Nakop.EditValue = (decimal)0;
                                SumFeePFR_Strah.EditValue = (decimal)0;
                                SumFeePFR_Nakop.EditValue = (decimal)0;

                            }
                            else if (!((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Enabled)
                            {
                                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = (decimal)0;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void setPeriod()
        {
            for (int i = 1; i <= 12; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    string itemName = "s_" + i.ToString() + "_" + j.ToString();
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                            //     DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];
                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Enabled = false;
                        }
                    }
                    catch
                    {
                    }
                }
            }


            short y;
            if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y))
            {
                byte q;
                if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                {
                    if (TypeInfo.SelectedIndex != 0)
                    {
                        y = 0;
                        if (KorrYear.SelectedItem != null && short.TryParse(KorrYear.SelectedItem.Text, out y))
                        {
                            q = 20;
                            if (KorrQuarter.SelectedItem != null && byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out q))
                            {
                            }
                            else
                            {
                                Methods.showAlert("Внимание!", "Не указан корректируемый период!", this.ThemeName);
                            }
                        }
                        else
                        {
                            Methods.showAlert("Внимание!", "Не указан корректируемый период!", this.ThemeName);
                        }

                    }

                    period = q;
                    RaschetPeriodContainer rp = avail_periods_all.FirstOrDefault(x => x.Year == y && x.Kvartal == q);
                    int m1 = rp.DateBegin.Month;
                    int m2 = rp.DateEnd.Month;

                    for (int i = m1; i <= m2; i++)
                    {
                        for (int j = 0; j <= 1; j++)
                        {
                            string itemName = "s_" + i.ToString() + "_" + j.ToString();
                            try
                            {
                                if (this.Controls.Find(itemName, true).Any())
                                {
                                    //     DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];
                                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Enabled = true;
                                }
                            }
                            catch
                            {
                            }
                        }
                    }


                }
            }
            calc();
            calc2();
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
                KorrYear.Enabled = false;
                KorrQuarter.Enabled = false;
                korrFeeGroup.Enabled = false;
                feeGroup.Enabled = true;
                radPageView1.Pages[2].Enabled = true;

            }
            else
            {
                KorrYear.Enabled = true;
                KorrQuarter.Enabled = true;
                korrFeeGroup.Enabled = true;

                if (TypeInfo.SelectedIndex == 1)
                {
                    radPageView1.Pages[2].Enabled = true;
                    feeGroup.Enabled = true;
                }
                else if (TypeInfo.SelectedIndex == 2)
                {
                    radPageView1.Pages[2].Enabled = false;
                    feeGroup.Enabled = false;
                }

                var avail_periods = avail_periods_all.Where(x => x.Year >= 2010 && x.Year <= 2012).OrderBy(x => x.Year);

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
                        if (SZV_6.YearKorr != null)
                            KorrYear.Items.Single(x => x.Text.ToString() == SZV_6.YearKorr.ToString()).Selected = true;
                        else if (KorrYear.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                            KorrYear.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;

                        break;
                }

                KorrYear_SelectedIndexChanged();



            }
        }

        private void KorrYear_SelectedIndexChanged()
        {
            // выпад список "Отчетный период"
            var avail_periods = avail_periods_all.Where(x => x.Year >= 2010 && x.Year <= 2012);

            byte q = 20;
            if (KorrQuarter.SelectedItem != null && byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out q)) { }

            this.KorrQuarter.Items.Clear();

            short y;
            if (KorrYear.SelectedItem != null && short.TryParse(KorrYear.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList()) // && x.Kvartal != 0
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
                setPeriod();
            }
            getBase();

        }


        private void getBase()
        {

            if (TypeInfo.SelectedIndex == 0 && !String.IsNullOrEmpty(Year.Text))
            {
                short y;

                if (Year.SelectedItem != null && short.TryParse(Year.Text, out y))
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

                if (KorrYear.SelectedItem != null && short.TryParse(KorrYear.Text, out y))
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
            child.action = "selection";
            child.radDropDownList1.Enabled = false;
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

            calc();
        }

        private void TypeInfo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            change_TypeInfo();

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
                    updateTextBoxesSetZero(true);
                else
                    updateTextBoxesSetZero(false);

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
            var fields = typeof(FormsSZV_6).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                            //    DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                            string type = SZV_6.GetType().GetProperty(itemName).PropertyType.FullName;
                            if (type.Contains("["))
                                type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                            type = type.Split(',')[0].Split('.')[1].ToLower();
                            if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text != "")
                            {
                                switch (type)
                                {
                                    case "decimal":
                                        SZV_6.GetType().GetProperty(itemName).SetValue(SZV_6, Math.Round(decimal.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text), 2, MidpointRounding.AwayFromZero), null);
                                        break;
                                }
                            }
                            else
                                SZV_6.GetType().GetProperty(itemName).SetValue(SZV_6, null, null);
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

                    SZV_6.InsurerID = Options.InsID;
                    SZV_6.StaffID = staff.ID;
                    SZV_6.Year = short.Parse(Year.Text);
                    SZV_6.Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString());
                    SZV_6.DateFilling = DateFilling.DateTime.Date;
                    SZV_6.PlatCategoryID = PlatCat.ID;
                    SZV_6.TypeInfoID = long.Parse(TypeInfo.SelectedItem.Value.ToString());
                    SZV_6.SUMTAXYEAR = decimal.Parse(SUMTAXYEAR.EditValue.ToString());
                    SZV_6.AutoCalc = AutoCalcSwitch.IsOn;

                    if (TypeInfo.SelectedIndex != 0)
                    {

                        SZV_6.YearKorr = short.Parse(KorrYear.SelectedItem.Text);
                        SZV_6.QuarterKorr = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
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
                            db.FormsSZV_6.AddObject(SZV_6);
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
                        FormsSZV_6 r1 = db.FormsSZV_6.FirstOrDefault(x => x.ID == SZV_6.ID);
                        try
                        {
                            var fields = typeof(FormsSZV_6).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            foreach (var itemName_ in names)
                            {
                                string itemName = itemName_.TrimStart('_');
                                var properties = SZV_6.GetType().GetProperty(itemName);
                                if (properties != null)
                                {
                                    object value = properties.GetValue(SZV_6, null);
                                    var data = value;

                                    r1.GetType().GetProperty(itemName).SetValue(r1, data, null);
                                }

                            }


                            // сохраняем модифицированную запись обратно в бд
                            db.ObjectStateManager.ChangeObjectState(r1, System.Data.EntityState.Modified);
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
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Year", type = "error" });
                return false;
            }

            if (SumFeePFR_Strah.Text == "0.00")
            {
                errMessBox.Add(new ErrList { name = "Не внесены страховые взносы", control = "SumFeePFR_Strah", type = "warning" });
            }

            long InsID = Options.InsID;

            short y = short.Parse(Year.Text);
            byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());

            switch (action)
            {
                case "add":
                    if (TypeInfo.SelectedIndex == 0) // если исходная запись
                    {
                        if (db.FormsSZV_6.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCat.ID))
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

                        if (db.FormsSZV_6.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCat.ID && x.YearKorr == yk && x.QuarterKorr == qk))
                        {
                            errMessBox.Add(new ErrList { name = "Дублирование записи по ключу уникальности (сотрудник, отчетный период, корректируемый отчетный период, категория плательщика, тип договора)", control = "Year", type = "error" });
                        }

                    }

                    break;
                case "edit":
                    if (TypeInfo.SelectedIndex == 0) // если исходная запись
                    {
                        if (db.FormsSZV_6.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCat.ID && x.ID != SZV_6.ID))
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

                        if (db.FormsSZV_6.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCat.ID && x.YearKorr == yk && x.QuarterKorr == qk && x.ID != SZV_6.ID))
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
                calc();
            }

        }

        private void calc()
        {
            if (!AutoCalcSwitch.IsOn)
            {

                decimal sum = 0;
                for (int i = 1; i <= 12; i++)
                {
                    string itemName = "s_" + i.ToString() + "_0";
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                            //   DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];
                            if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Enabled)
                            {
                                sum = sum + decimal.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                SUMTAXYEAR.EditValue = sum;
                OPSLabel_All.Text = sum != 0 ? sum.ToString() : "0.00";

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
                calc2();
            }

        }

        private void calc2()
        {
            if (!AutoCalcSwitch.IsOn)
            {

                decimal sum = 0;
                for (int i = 1; i <= 12; i++)
                {
                    string itemName = "s_" + i.ToString() + "_1";
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                            //      DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];
                            if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Enabled)
                            {
                                sum = sum + decimal.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString());
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                OPSLabel_MoreBase.Text = sum != 0 ? sum.ToString() : "0.00";
            }
        }

        private void calcFee()
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
                    decimal sumAll = decimal.Parse(SUMTAXYEAR.EditValue.ToString());

                    if (Options.mrot != null)
                    {
                        sumAll = sumAll <= Options.mrot.NalogBase ? sumAll : Options.mrot.NalogBase;
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


        private void reCalcFeeBtn_Click(object sender, EventArgs e)
        {
            calcFee();
        }


        private void SZV_6_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

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
            TypeInfo.Items[0].Selected = true;

            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal2010_2013.Where(x => x.Year <= 2012).OrderBy(x => x.Year);
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
                    SZV_6 = new FormsSZV_6();
                    DateFilling.EditValue = DateTime.Now;
                    AutoCalcSwitch.IsOn = true;
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
                        foreach (var item in avail_periods_all.Where(x => x.Year == y).ToList())  // && x.Kvartal != 0
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
                    DateFilling.EditValue = SZV_6.DateFilling;
                    AutoCalcSwitch.IsOn = SZV_6.AutoCalc.HasValue ? SZV_6.AutoCalc.Value : false;

                    #region Отчетный период

                    // выпад список "календарный год"

                    if (Year.Items.Any(x => x.Text.ToString() == SZV_6.Year.ToString()))
                        Year.Items.Single(x => x.Text.ToString() == SZV_6.Year.ToString()).Selected = true;
                    //           else
                    //              Year.Items.OrderByDescending(x => x.Value).First().Selected = true;


                    // выпад список "Отчетный период"

                    this.Quarter.Items.Clear();

                    if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y))
                    {
                        foreach (var item in avail_periods_all.Where(x => x.Year == y).ToList())
                        {
                            Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                        }
                        if (Quarter.Items.Any(x => x.Value.ToString() == SZV_6.Quarter.ToString()))
                            Quarter.Items.FirstOrDefault(x => x.Value.ToString() == SZV_6.Quarter.ToString()).Selected = true;

                    }
                    #endregion

                    TypeInfo.Items.Single(x => x.Value.ToString() == SZV_6.TypeInfoID.ToString()).Selected = true;
                    change_TypeInfo();

                    if (SZV_6.QuarterKorr != null && KorrQuarter.Items.Any(x => x.Value.ToString() == SZV_6.QuarterKorr.ToString()))
                        KorrQuarter.Items.Single(x => x.Value.ToString() == SZV_6.QuarterKorr.ToString()).Selected = true;

                    PlatCat = SZV_6.PlatCategory;

                    if (PlatCat != null)
                    {
                        platCategoryPanel.Text = PlatCat.Code + "   " + PlatCat.Name;
                    }
                    else
                    {
                        platCategoryPanel.Text = "Категория плательщика - не определена... Нажмите для выбора";
                    }


                    //Информация о сотруднике
                    staff = SZV_6.Staff;
                    setPeriod();

                    updateTextBoxes();


                    break;
            }

            getBase();

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            this.KorrYear.SelectedIndexChanged += (s, с) => KorrYear_SelectedIndexChanged();
            this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();
            this.KorrQuarter.SelectedIndexChanged += (s, с) => KorrQuarter_SelectedIndexChanged();

            //   calc();
            //     calc2();

            long id_old = 0;
            switch (action)
            {
                case "add":

                    break;
                case "edit":
                    id_old = SZV_6.ID;
                    SZV_6 = new FormsSZV_6 { ID = id_old };
                    break;
            }

            for (int i = 1; i <= 12; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    string itemName = "s_" + i.ToString() + "_" + j.ToString();
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                            //    DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];
                            if (j == 0)
                                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Leave += (s, c) => CalcTextBoxes(s, c);
                            else
                                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Leave += (s, c) => CalcTextBoxes2(s, c);
                        }
                    }
                    catch
                    {
                    }
                }
            }

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
                    if (db.FormsSZV_6.Any(x => x.Year == y && x.Quarter == q && x.TypeInfoID == 1 && x.StaffID == staff.ID && x.PlatCategoryID == PlatCat.ID))
                    {
                        FormsSZV_6 is_ishod = db.FormsSZV_6.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.TypeInfoID == 1 && x.StaffID == staff.ID && x.PlatCategoryID == PlatCat.ID);

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

        private void AutoCalcSwitch_Toggled(object sender, EventArgs e)
        {
            calc();
            calc2();
        }



    }
}
