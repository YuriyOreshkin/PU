using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using PU.Models;
using Telerik.WinControls.UI;
using PU.Classes;
using PU.FormsRSW2014;
using System.Reflection;
using Telerik.WinControls.Enumerations;

namespace PU.FormsSPW2_2014
{
    public partial class SPW2_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public FormsSPW2 formData { get; set; }
        public Staff staff { get; set; }
        public PlatCategory PlatCat { get; set; }
        private bool cleanData = true;
        private List<ErrList> errMessBox = new List<ErrList>();


        public SPW2_Edit()
        {
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

        /// <summary>
        /// Загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SPW2_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            this.radPageView1.SelectedPage = this.radPageView1.Pages[0];

            checkAccessLevel();

            foreach (var item in db.TypeInfo)
            {
                TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }

            this.Year.Items.Clear();
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014 && x.Year <= 2018).OrderBy(x => x.Year);
            foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }
            short y;

            switch (action)
            {
                case "add":
                    if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                        Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                    else
						Year.Items.OrderByDescending(x => x.Value).First().Selected = true;

                    // выпад список "Отчетный период"

                    this.Quarter.Items.Clear();

                    if (short.TryParse(Year.SelectedItem.Text, out y))
                    {
                        foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                        {
                            Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                        }
                        DateTime dt = DateTime.Now.AddDays(-45);

                        RaschetPeriodContainer rp = new RaschetPeriodContainer();

                        if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                            rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                        else
                            rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                        if (rp != null)
                            Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                        else
                            Quarter.Items.OrderByDescending(x => x.Value).First().Selected = true;
                    }

                        formData = new FormsSPW2();

                        dateFilling.Value = DateTime.Now;  // дата заполнения

                        TypeInfo.Items.Single(x => x.Text == "Исходная").Selected = true;

                    break;
                case "edit":
                        if (formData != null)
                        {
                            if (Year.Items.Any(x => x.Text == formData.Year.ToString()))
                            {
                                Year.Items.First(x => x.Text == formData.Year.ToString()).Selected = true;

                                // выпад список "Отчетный период"

                                this.Quarter.Items.Clear();

                                if (short.TryParse(Year.Text, out y))
                                {
                                    foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                                    {
                                        Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                                    }
                                }
                                if (Quarter.Items.Any(x => x.Value.ToString() == formData.Quarter.ToString()))
                                    Quarter.Items.First(x => x.Value.ToString() == formData.Quarter.ToString()).Selected = true;

                            }

                            TypeInfo.Items.Single(x => x.Value.ToString() == formData.TypeInfoID.ToString()).Selected = true;
                            change_TypeInfo();



                            if (formData.TypeInfoID > 1)
                            {
                                if (KorrQuarter.Items.Any(x => x.Value.ToString() == formData.QuarterKorr.ToString()))
                                    KorrQuarter.Items.First(x => x.Value.ToString() == formData.QuarterKorr.ToString()).Selected = true;
                                else
                                {
                                    KorrQuarter.Text = "";
                                }
                            }

                            dateFilling.Value = formData.DateFilling;
                            DateComposite.Value = formData.DateComposit;

                            PlatCat = formData.PlatCategory;
                            if (PlatCat != null)
                            {
                                platCatPanel.Text = PlatCat.Code + "   " + PlatCat.FullName;
                            }

                            existsInsurCheckBox.Checked = formData.ExistsInsurOPS.Value == 0 ? false : true;
                            existsInsurDopCheckBox.Checked = formData.ExistsInsurDop.Value == 0 ? false : true;

                        }
                    break;
            }


            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();

            staffFIOLabel.Text = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName;

        }

        private void Year_SelectedIndexChanged()
        {
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }
            
            Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year == y))
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
            }
        }

        private void change_TypeInfo()
        {
            if (TypeInfo.SelectedIndex == 0)
            {
                KorrYear.Items.Clear();
                KorrQuarter.Items.Clear();
                KorrYear.Enabled = false;
                KorrQuarter.Enabled = false;
                this.KorrYear.SelectedIndexChanged += null;

            }
            else
            {
                KorrYear.Enabled = true;
                KorrQuarter.Enabled = true;

                var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year <= DateTime.Now.Year).OrderBy(x => x.Year);

                // выпад список "календарный год"

                this.KorrYear.Items.Clear();

                foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
                {
                    KorrYear.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                }

                switch (action)
                {
                    case "add":

                        break;
                    case "edit":
                        if (formData.YearKorr != null)
                            KorrYear.Items.Single(x => x.Text.ToString() == formData.YearKorr.ToString()).Selected = true;
                        break;
                }

                this.KorrYear.SelectedIndexChanged += (s, с) => KorrYear_SelectedIndexChanged();

                KorrYear_SelectedIndexChanged();




            }
        }

        private void KorrYear_SelectedIndexChanged()
        {
            // выпад список "Отчетный период"
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year <= DateTime.Now.Year);

            byte q = 20;
            if (KorrQuarter.SelectedItem != null && byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out q)) { }
   
            this.KorrQuarter.Items.Clear();

            short y;
            if (short.TryParse(KorrYear.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
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
        }

        private void SPW2_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (cleanData)
                formData = null;
        }

        private void radPanel1_MouseHover(object sender, EventArgs e)
        {
            platCatPanel.Font = new Font(platCatPanel.Font, platCatPanel.Font.Style | FontStyle.Underline);
        }

        private void radPanel1_MouseLeave(object sender, EventArgs e)
        {
            platCatPanel.Font = new Font(platCatPanel.Font, FontStyle.Bold);
        }

        private void radPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            PlatCategoryFrm child = new PlatCategoryFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.PlatCat = PlatCat;
            child.action = "selection";
            child.ShowDialog();
            PlatCat = child.PlatCat;
            //formData = new FormsSPW2();
            formData.PlatCategoryID = child.PlatCat == null ? 0 : child.PlatCat.ID;

            if (PlatCat != null)
            {
                platCatPanel.Text = PlatCat.Code + "   " + PlatCat.Name;
            }


        }

        private void TypeInfo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            change_TypeInfo();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                bool flag_ok = false;
                try
                {
                    getValues();
                    flag_ok = true;
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы. Код ошибки: " + ex.Message);
                }

                if (flag_ok)
                {
                    switch (action)
                    { 
                        case "add":

                            try
                            {
                                db.AddToFormsSPW2(formData);
                                db.SaveChanges();
                                cleanData = false;
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Формы СПВ-2 произошла ошибка. Код ошибки: " + ex.InnerException);
                            }

                            break;
                        case "edit":
                            // выбираем из базы исходную запись по идешнику
                            FormsSPW2 r1 = db.FormsSPW2.FirstOrDefault(x => x.ID == formData.ID);
                            try
                            {
                                var fields = typeof(FormsSPW2).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = formData.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(formData, null);
                                        var data = value;

                                        r1.GetType().GetProperty(itemName).SetValue(r1, data, null);
                                    }

                                }


                                // сохраняем модифицированную запись обратно в бд
                                db.ObjectStateManager.ChangeObjectState(r1, System.Data.EntityState.Modified);
                                db.SaveChanges();
                                cleanData = false;
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Формы СПВ-2 произошла ошибка. Код ошибки: " + ex.Message);
                            }
                            break;
                    }
                }

            }
            else
            {
                foreach (var item in errMessBox)
                { Methods.showAlert("Ошибка заполнения", item.name, this.ThemeName, 100); }

            }

        }

                /// <summary>
        /// Сбор данных для сохранения
        /// </summary>
        private void getValues()
        {
            if (dateFilling.Value != dateFilling.NullDate)
                formData.DateFilling = dateFilling.Value;

            if (DateComposite.Value != DateComposite.NullDate)
                formData.DateComposit = DateComposite.Value;

            formData.TypeInfoID = long.Parse(TypeInfo.SelectedItem.Value.ToString());
            formData.Year = short.Parse(Year.Text);

            if (Quarter.Text != "")
                formData.Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString());

            formData.PlatCategoryID = PlatCat == null ? 0 : PlatCat.ID;

            if (formData.TypeInfoID > 1)
            {
                if (KorrYear.Text != "")
                    formData.YearKorr = short.Parse(KorrYear.Text);

                if (Quarter.Text != "")
                    formData.QuarterKorr = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
            }
            else
            {
                formData.YearKorr = null;
                formData.QuarterKorr = null;
            }

            formData.ExistsInsurOPS = existsInsurCheckBox.Checked ? (byte)1 : (byte)0;
            formData.ExistsInsurDop = existsInsurDopCheckBox.Checked ? (byte)1 : (byte)0;
            formData.StaffID = staff.ID;
            formData.InsurerID = Options.InsID;
        }

        /// <summary>
        /// Проверка введенных данных
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            if (DateComposite.Value == DateComposite.NullDate)
                errMessBox.Add(new ErrList { name = "Необходимо указать предполагаемую дату установления трудовой пенсии" });

            if (dateFilling.Value == dateFilling.NullDate)
                errMessBox.Add(new ErrList { name = "Необходимо указать дату заполнения" });

            if (PlatCat == null)
                errMessBox.Add(new ErrList { name = "Необходимо выбрать Ккатегорию плательщика" });

            long TypeInfoID = long.Parse(TypeInfo.SelectedItem.Value.ToString());
            short y = 0;
            short yk = 0;
            byte q = 20;
            byte qk = 20;
            if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y))
            {
                if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                {

                }
                else
                {
                    errMessBox.Add(new ErrList { name = "Необходимо выбрать Отчетный период" });
                    return false;
                }
            }
            else
            {
                errMessBox.Add(new ErrList { name = "Необходимо выбрать Год" });
                return false;
            }

            if (TypeInfoID > 1)
            {
                if (KorrYear.SelectedItem != null && short.TryParse(KorrYear.SelectedItem.Text, out yk))
                {
                    if (KorrQuarter.SelectedItem != null && byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out qk))
                    {

                    }
                    else
                    {
                        errMessBox.Add(new ErrList { name = "Необходимо выбрать Корректируемый Отчетный период" });
                        return false;
                    }
                }
                else
                {
                    errMessBox.Add(new ErrList { name = "Необходимо выбрать Корректируемый Год" });
                    return false;
                }
            }

            long PlatCategoryID = PlatCat == null ? 0 : PlatCat.ID;


            switch (action)
            {
                case "add":
                    if (TypeInfoID == 1)
                    {
                        if (db.FormsSPW2.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCategoryID && x.TypeInfoID == TypeInfoID))
                        {
                            errMessBox.Add(new ErrList { name = "Ошибка! Нарушение уникальности записей. Для данного сотрудника уже есть запись Формы СПВ-2 с указанными параметрами Календарного года, Отчетного периода и Категории плательщика." });
                        }
                    }
                    else
                    {
                        if (db.FormsSPW2.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.PlatCategoryID == PlatCategoryID && x.TypeInfoID == TypeInfoID))
                        {
                            errMessBox.Add(new ErrList { name = "Ошибка! Нарушение уникальности записей. Для данного сотрудника уже есть запись Формы СПВ-2 с указанными параметрами Календарного года, Отчетного периода и Категории плательщика." });
                        }
                    }

                    break;
                case "edit":
                    if (TypeInfoID == 1)
                    {
                        if (db.FormsSPW2.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.PlatCategoryID == PlatCategoryID && x.TypeInfoID == TypeInfoID && x.ID != formData.ID))
                        {
                            errMessBox.Add(new ErrList { name = "Ошибка! Нарушение уникальности записей. Для данного сотрудника уже есть запись Формы СПВ-2 с указанными параметрами Календарного года, Отчетного периода и Категории плательщика." });
                        }
                    }
                    else
                    {
                        if (db.FormsSPW2.Any(x => x.StaffID == staff.ID && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.PlatCategoryID == PlatCategoryID && x.TypeInfoID == TypeInfoID && x.ID != formData.ID))
                        {
                            errMessBox.Add(new ErrList { name = "Ошибка! Нарушение уникальности записей. Для данного сотрудника уже есть запись Формы СПВ-2 с указанными параметрами Календарного года, Отчетного периода и Категории плательщика." });
                        }
                    }


                    break;
            }





            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DateCompositeMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateCompositeMaskedEditBox.Text != DateCompositeMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateCompositeMaskedEditBox.Text, out date))
                {
                    DateComposite.Value = date;
                }
                else
                {
                    DateComposite.Value = DateComposite.NullDate;
                }
            }
            else
            {
                DateComposite.Value = DateComposite.NullDate;
            }
        }

        private void DateComposite_ValueChanged(object sender, EventArgs e)
        {
            if (DateComposite.Value != DateComposite.NullDate)
                DateCompositeMaskedEditBox.Text = DateComposite.Value.ToShortDateString();
            else
                DateCompositeMaskedEditBox.Text = DateCompositeMaskedEditBox.NullText;
        }

        private void dateFillingMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (dateFillingMaskedEditBox.Text != dateFillingMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(dateFillingMaskedEditBox.Text, out date))
                {
                    dateFilling.Value = date;
                }
                else
                {
                    dateFilling.Value = dateFilling.NullDate;
                }
            }
            else
            {
                dateFilling.Value = dateFilling.NullDate;
            }
        }

        private void dateFilling_ValueChanged(object sender, EventArgs e)
        {
            if (dateFilling.Value != dateFilling.NullDate)
                dateFillingMaskedEditBox.Text = dateFilling.Value.ToShortDateString();
            else
                dateFillingMaskedEditBox.Text = dateFillingMaskedEditBox.NullText;
        }

        private void existsInsurCheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            existsInsurCheckBox.Text = existsInsurCheckBox.Checked ? "  Да" : "  Нет";
        }

        private void existsInsurDopCheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            existsInsurDopCheckBox.Text = existsInsurDopCheckBox.Checked ? "  Да" : "  Нет";
        }





    }
}
