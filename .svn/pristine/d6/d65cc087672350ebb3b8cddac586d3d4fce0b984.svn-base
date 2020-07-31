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

namespace PU.FormsRW_3_2015
{
    public partial class RW3_2015_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool editLoad { get; set; }
        private bool autoCalc { get; set; }
        bool allowClose = false;

        public FormsRW3_2015 RWdata { get; set; }
        public FormsRW3_2015 RWdataPrev { get; set; }
        public List<FormsRW3_2015_Razd_3> RW_3_3_List = new List<FormsRW3_2015_Razd_3>();
        private List<ErrList> errMessBox = new List<ErrList>();

        public short year_ { get; set; }
        public byte period_ { get; set; }
        public byte CorrNum { get; set; }
        private CodeBaseRW3_2015 CodeBase { get; set; }



        public RW3_2015_Edit()
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

        private void RW3_2015_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            checkAccessLevel();

            this.radPageView1.SelectedPage = this.radPageView1.Pages[0];

            DateUnderwrite.Value = DateTime.Now.Date;

            //var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2015).OrderBy(x => x.Year);


            BindingSource b = new BindingSource();
            b.DataSource = db.DocumentTypes;

            this.ConfirmDocType.DataSource = null;
            this.ConfirmDocType.Items.Clear();
            this.ConfirmDocType.DisplayMember = "Code";
            this.ConfirmDocType.ValueMember = "ID";
            this.ConfirmDocType.ShowItemToolTips = true;
            this.ConfirmDocType.DataSource = b.DataSource;
            this.ConfirmDocType.SelectedIndex = -1;
            this.ConfirmDocType.ResetText();

            // выпад список "календарный год"

            this.Year.Items.Clear();
            foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }

            switch (action)
            {
                case "add":
                    if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                        Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                    else
                        Year.Items.OrderByDescending(x => x.Value).First().Selected = true;
                    Year.Enabled = true;
                    Quarter.Enabled = true;

                    RWdata = new FormsRW3_2015 { AutoCalc = true };

                    break;
                case "edit":
                    if (Year.Items.Any(x => x.Text.ToString() == RWdata.Year.ToString()))
                        Year.Items.Single(x => x.Text.ToString() == RWdata.Year.ToString()).Selected = true;
                    break;
            }



            // выпад список "Отчетный период"
            this.Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                switch (action)
                {
                    case "add":
                        if (Quarter.Items.Count() > 0)
                        {
                            DateTime dt = DateTime.Now;
                            RaschetPeriodContainer rp = new RaschetPeriodContainer();

                            if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                                rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                            else
                                rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                            if (rp != null && Quarter.Items.Any(x => x.Value.ToString() == rp.Kvartal.ToString()))
                            {
                                Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                            }
                            else
                            {
                                Quarter.Items.First().Selected = true;
                            }

                        }
                        break;
                    case "edit":
                        if (Quarter.Items.Any(x => x.Value.ToString() == RWdata.Quarter.ToString()))
                            Quarter.Items.Single(x => x.Value.ToString() == RWdata.Quarter.ToString()).Selected = true;
                        break;
                }
                try
                {
                    getTariff();
                    getPrevData();
                }
                catch
                { }
            }

            editLoad = true;
            AutoCalcSwitch.IsOn = RWdata.AutoCalc.Value;
            editLoad = false;
            autoCalc = !AutoCalcSwitch.IsOn;
            s_100_0.Enabled = AutoCalcSwitch.IsOn;


            switch (action)
            {
                case "add":

                    RWdata.InsurerID = Options.InsID;
                    RWdata.CorrectionNum = byte.Parse(CorrectionNum.Value.ToString());

                    #region загрузка информации о представителе для инд. предпринимателей
                    if (Options.getInsurerFIOtoNewForm)
                    {
                        Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);
                        if (ins.TypePayer == 1)
                        {
                            ConfirmLastName.Text = ins.LastName;
                            ConfirmFirstName.Text = ins.FirstName;
                            ConfirmMiddleName.Text = ins.MiddleName;
                        }
                    }
                    #endregion

                    break;
                case "edit":
                    editLoad = true;

                    CorrectionNum.Value = RWdata.CorrectionNum;



                    WorkStop.Checked = RWdata.WorkStop == 0 ? false : true;
                    CountConfirmDoc.Value = RWdata.CountConfirmDoc.HasValue ? RWdata.CountConfirmDoc.Value : 0;
                    DateUnderwrite.Value = RWdata.DateUnderwrite.HasValue ? RWdata.DateUnderwrite.Value : DateTime.Now;

                    ConfirmType.SelectedIndex = RWdata.ConfirmType - 1;
                    ConfirmLastName.Text = RWdata.ConfirmLastName;
                    ConfirmFirstName.Text = RWdata.ConfirmFirstName;
                    ConfirmMiddleName.Text = RWdata.ConfirmMiddleName;
                    ConfirmOrgName.Text = RWdata.ConfirmOrgName;

                    CodeTarDDL.Items.FirstOrDefault(x => x.Tag.ToString() == RWdata.CodeTar.ToString()).Selected = true;

                    if (ConfirmType.SelectedIndex > 0 && RWdata.ConfirmDocType_ID.HasValue)
                    {
                        ConfirmDocType.Items.FirstOrDefault(x => x.Value.ToString() == RWdata.ConfirmDocType_ID.Value.ToString()).Selected = true;
                        ConfirmDocName.Text = RWdata.ConfirmDocName;
                        ConfirmDocSerLat.Text = RWdata.ConfirmDocSerLat;
                        ConfirmDocNum.Text = RWdata.ConfirmDocNum.HasValue ? RWdata.ConfirmDocNum.Value.ToString() : "";
                        if (RWdata.ConfirmDocDate.HasValue)
                            ConfirmDocDate.Value = RWdata.ConfirmDocDate.Value;
                        ConfirmDocKemVyd.Text = RWdata.ConfirmDocKemVyd;

                        if (RWdata.ConfirmDocDateBegin.HasValue)
                            ConfirmDocDateBegin.Value = RWdata.ConfirmDocDateBegin.Value;
                        if (RWdata.ConfirmDocDateEnd.HasValue)
                            ConfirmDocDateEnd.Value = RWdata.ConfirmDocDateEnd.Value;
                    }

                    updateValues();
                    editLoad = false;

                    break;
            }

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();
            ConfirmType_SelectedIndexChanged(null, null);


        }

        private void getTariff()
        {
            string year_ = Year.Text;
            if (!String.IsNullOrEmpty(year_))
            {
                short y;

                if (short.TryParse(year_, out y))
                {
                    if (db.CodeBaseRW3_2015.Any(x => x.Year == y))
                        CodeBase = db.CodeBaseRW3_2015.First(x => x.Year == y);
                    else
                        CodeBase = null;
                }
            }
            else
            {
                CodeBase = null;
            }
            if (CodeBase != null)
                tariffLabel.Text = "Ставка: " + (CodeTarDDL.SelectedIndex == 0 ? Utils.decToStr(CodeBase.Tar21) : Utils.decToStr(CodeBase.Tar22));
            else
                tariffLabel.Text = "Ставка не определена...";

        }

        private void Year_SelectedIndexChanged()
        {
            getTariff();
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            this.Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);
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

                    getPrevData();
                }
            }

        }


        private void Quarter_SelectedIndexChanged()
        {
            if (Quarter.SelectedItem != null)
                getPrevData();
        }


        private void getPrevData()
        {
            RWdataPrev = null;

            //            if (autoCalc)
            //            {
            short y = 0;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                byte q;
                if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                {
                    byte codeTar = 0;

                    byte.TryParse(CodeTarDDL.SelectedItem.Tag.ToString(), out codeTar);

                    if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                    {
                        byte quarter = 20;
                        if (q == 6)
                            quarter = 3;
                        else if (q == 9)
                            quarter = 6;
                        else if (q == 0)
                            quarter = 9;


                        if (db.FormsRW3_2015.Any(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID && x.CodeTar == codeTar))
                            RWdataPrev = db.FormsRW3_2015.Where(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID && x.CodeTar == codeTar).OrderByDescending(x => x.CorrectionNum).First();
                    }
                    else
                    {
                        byte quarter = 0;
                        y--;

                        if (db.FormsRW3_2015.Any(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID && x.CodeTar == codeTar))
                            RWdataPrev = db.FormsRW3_2015.Where(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID && x.CodeTar == codeTar).OrderByDescending(x => x.CorrectionNum).First();

                    }

                }
            }


            //            }

            if (autoCalc)
            {
                s_100_0.EditValue = RWdataPrev.s_150_0.HasValue ? RWdataPrev.s_150_0.Value : (decimal)0;
            }

        }

        /// <summary>
        /// Обновление полей формы РВ-3 и таблиц
        /// </summary>
        private void updateValues()
        {
            var fields = typeof(FormsRW3_2015).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    try
                    {

                        if (this.Controls.Find(itemName, true).Any())
                        {
                            //      DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                            if (RWdata != null)
                            {
                                var properties = RWdata.GetType().GetProperty(itemName);
                                object value = properties.GetValue(RWdata, null);
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
                                        case "integer":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = int.Parse(value.ToString());
                                            break;
                                        case "int64":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = long.Parse(value.ToString());
                                            break;
                                        case "datetime":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = DateTime.Parse(value.ToString()).Date;
                                            break;
                                        case "string":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = value.ToString();
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
                                        case "integer":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = 0;
                                            break;
                                        case "int64":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = 0;
                                            break;
                                        case "datetime":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text = "";
                                            break;
                                        case "string":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = "";
                                            break;
                                    }
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message);
                    }

                }

            }



            gridUpdate_3();
        }


        #region Раздел 3

        /// <summary>
        /// обновление таблицы раздела 3
        /// </summary>
        private void gridUpdate_3()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            grid_3.Rows.Clear();

            if (RW_3_3_List.Count() != 0)
            {
                int num = 0;
                foreach (var item in RW_3_3_List)
                {
                    num++;

                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.grid_3.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Num"].Value = num.ToString();
                    rowInfo.Cells["CodeBase"].Value = item.CodeBase.HasValue ? item.CodeBase.Value.ToString() : "";
                    rowInfo.Cells["Year"].Value = item.Year.HasValue ? item.Year.Value.ToString() : "";
                    rowInfo.Cells["Month"].Value = item.Month.HasValue ? item.Month.Value.ToString() : ""; ;
                    rowInfo.Cells["Sum"].Value = item.SumFee.HasValue ? Utils.decToStr(item.SumFee.Value) : Utils.decToStr(0);
                    grid_3.Rows.Add(rowInfo);
                }
            }

            grid_3.Refresh();
            updateTextBoxes_Razd_3();
        }

        /// <summary>
        /// Функция обновления результирующих значений в полях под таблицей Раздела 2
        /// </summary>
        /// <param name="rsw"></param>
        private void updateTextBoxes_Razd_3()
        {
            sumLabel.Text = RW_3_3_List.Count != 0 ? Utils.decToStr(RW_3_3_List.Where(x => x.SumFee.HasValue).Sum(x => x.SumFee.Value)) : Utils.decToStr(0);

            if (!editLoad)
                s_120_0.EditValue = RW_3_3_List.Count != 0 ? RW_3_3_List.Where(x => x.SumFee.HasValue).Sum(x => x.SumFee.Value) : (decimal)0;

        }


        #endregion

        private void radButton2_Click(object sender, EventArgs e)
        {
        //    RWdata = null;
            this.Close();
        }

        /// <summary>
        /// Сохранение данных формы РВ-3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton1_Click(object sender, EventArgs e)
        {
            if (validation())
            {

                savingForm();
            }
            else
            {
                //                if (errMessBox.Count != 0)
                //                {
                //                    foreach (var item in errMessBox)
                //                    {
                DialogResult ds = Telerik.WinControls.RadMessageBox.Show(this, errMessBox[0].name + "\r\nСохранить \"как есть\"?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                switch (ds)
                {
                    case DialogResult.Yes:

                        savingForm();
                        break;
                    case DialogResult.No:
                        break;
                }
                //                    }
                //                }

            }
        }
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            if (Year.Text == "")
            {
                errMessBox.Add(new ErrList { name = "Не выбран год", control = "Year" });
                return false;
            }

            if (Quarter.Text == "")
            {
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Quarter" });
                return false;
            }

            if (ConfirmType.SelectedItem == null)
                errMessBox.Add(new ErrList { name = "Не выбрано подтверждающее лицо", control = "ConfirmType" });


            int ccd = int.Parse(CountConfirmDoc.Value.ToString());
            if (ccd > 255)
                errMessBox.Add(new ErrList { name = "Количество подтверждающих документов не может быть больше 255", control = "CountConfirmDoc" });



            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        private void savingForm()
        {

            short y_;
            if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y_))
            {
                byte q_;
                if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q_))
                {
                    long InsID = Options.InsID;
                    byte corrNum = byte.Parse(CorrectionNum.Value.ToString());
                    byte codeTar = byte.Parse(CodeTarDDL.Text);

                    switch (action)
                    {
                        case "add":
                            if (db.FormsRW3_2015.Any(x => x.InsurerID == InsID && x.Year == y_ && x.Quarter == q_ && x.CorrectionNum == corrNum && x.CodeTar == codeTar))
                            {
                                RadMessageBox.Show("Дублирование записи по ключу уникальности (страхователь, календарный год, отчетный период, номер корректировки, код тарифа)");
                                return;
                            }
                            break;
                        case "edit":
                            if (db.FormsRW3_2015.Any(x => x.InsurerID == InsID && x.Year == y_ && x.Quarter == q_ && x.CorrectionNum == corrNum && x.CodeTar == codeTar && x.ID != RWdata.ID))
                            {
                                RadMessageBox.Show("Дублирование записи по ключу уникальности (страхователь, календарный год, отчетный период, номер корректировки, код тарифа)");
                                return;
                            }
                            break;
                    }
                }
            }



            bool flag_ok = false;

            try
            {
                getValues();
                flag_ok = true;
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы РСВ-2. Код ошибки: " + ex.Message);
            }

            short y = 0;
            byte q = 0;

            if (!short.TryParse(Year.Text, out y))
            {
                flag_ok = false;
            }

            if (Quarter.SelectedItem == null || (Quarter.SelectedItem != null && !byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)))
            {
                flag_ok = false;
            }

            if (flag_ok)
                try
                {
                    flag_ok = false;

                    RWdata.InsurerID = Options.InsID;
                    RWdata.CorrectionNum = byte.Parse(CorrectionNum.Value.ToString());

                    RWdata.Year = y;
                    RWdata.Quarter = q;
                    RWdata.CodeTar = byte.Parse(CodeTarDDL.Text);
                    RWdata.WorkStop = WorkStop.Checked ? (byte)1 : (byte)0;
                    if (int.Parse(CountConfirmDoc.Value.ToString()) <= 255)
                        RWdata.CountConfirmDoc = byte.Parse(CountConfirmDoc.Value.ToString());
                    else
                        RWdata.CountConfirmDoc = (byte)255;
                    RWdata.DateUnderwrite = DateUnderwrite.Value.Date;
                    RWdata.ConfirmType = byte.Parse(ConfirmType.SelectedItem.Tag.ToString());
                    RWdata.ConfirmFirstName = ConfirmFirstName.Text;
                    RWdata.ConfirmLastName = ConfirmLastName.Text;
                    RWdata.ConfirmMiddleName = ConfirmMiddleName.Text;
                    RWdata.ConfirmOrgName = ConfirmOrgName.Text;
                    if (ConfirmDocType.Text == "")
                        RWdata.ConfirmDocType_ID = null;
                    else
                        RWdata.ConfirmDocType_ID = long.Parse(ConfirmDocType.SelectedItem.Value.ToString());
                    RWdata.ConfirmDocName = ConfirmDocName.Text;
                    RWdata.ConfirmDocSerLat = ConfirmDocSerLat.Text;
                    if (ConfirmDocNum.Text == "")
                        RWdata.ConfirmDocNum = null;
                    else
                        RWdata.ConfirmDocNum = int.Parse(ConfirmDocNum.Text);
                    if (ConfirmDocDate.Text == "")
                        RWdata.ConfirmDocDate = null;
                    else
                        RWdata.ConfirmDocDate = ConfirmDocDate.Value.Date;
                    RWdata.ConfirmDocKemVyd = ConfirmDocKemVyd.Text;
                    if (ConfirmDocDateBegin.Text == "")
                        RWdata.ConfirmDocDateBegin = null;
                    else
                        RWdata.ConfirmDocDateBegin = ConfirmDocDateBegin.Value.Date;
                    if (ConfirmDocDateEnd.Text == "")
                        RWdata.ConfirmDocDateEnd = null;
                    else
                        RWdata.ConfirmDocDateEnd = ConfirmDocDateEnd.Value.Date;
                    RWdata.AutoCalc = AutoCalcSwitch.IsOn;

                    flag_ok = true;



                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка во время сохранения данных с главного окна редактирования формы РВ-3. Код ошибки: " + ex.Message);
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
                            db.AddToFormsRW3_2015(RWdata);
                            db.SaveChanges();

                            foreach (var item in RW_3_3_List)
                            {
                                item.FormsRW3_2015_ID = RWdata.ID;
                                FormsRW3_2015_Razd_3 r = new FormsRW3_2015_Razd_3();

                                var fields = typeof(FormsRW3_2015_Razd_3).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = item.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(item, null);
                                        var data = value;

                                        r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                    }

                                }

                                db.AddToFormsRW3_2015_Razd_3(r);
                            }


                            flag_ok = true;



                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранении данных произошла ошибка. Код ошибки: " + ex.Message);
                        }
                        if (flag_ok)
                        {
                            try
                            {
                                db.SaveChanges();
                                allowClose = true;
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
                            #endregion

                        break;
                    //изменение записи формы РВ-3
                    case "edit":
                        // выбираем из базы исходную запись по идешнику
                        FormsRW3_2015 r1 = db.FormsRW3_2015.FirstOrDefault(x => x.ID == RWdata.ID);
                        try
                        {
                            var fields = typeof(FormsRW3_2015).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            foreach (var itemName_ in names)
                            {
                                string itemName = itemName_.TrimStart('_');
                                var properties = RWdata.GetType().GetProperty(itemName);
                                if (properties != null)
                                {
                                    object value = properties.GetValue(RWdata, null);
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
                            flag_ok = false;

                            #region обрабатываем записи о выплатах из Раздела 3
                            try
                            {
                                var RW_3_3_List_from_db = db.FormsRW3_2015_Razd_3.Where(x => x.FormsRW3_2015_ID == RWdata.ID);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RW_3_3_List.Select(x => x.ID);
                                var list_for_del = RW_3_3_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRW3_2015_Razd_3.DeleteObject(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RW_3_3_List_from_db = db.FormsRW3_2015_Razd_3.Where(x => x.FormsRW3_2015_ID == RWdata.ID && !list_for_del.Select(c => c.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 3 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRW3_2015_Razd_3).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RW_3_3_List)
                                {
                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RW_3_3_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRW3_2015_Razd_3 rsw_temp = RW_3_3_List_from_db.Single(x => x.ID == item.ID);

                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
                                            if (item_.IndexOf("FormsRW3_2015_ID") < 0)
                                            {

                                                string data_old = "";
                                                string data_new = "";

                                                var properties_old = rsw_temp.GetType().GetProperty(itemName);
                                                object value_old = properties_old.GetValue(rsw_temp, null);
                                                data_old = value_old != null ? value_old.ToString() : "";


                                                var properties_new = item.GetType().GetProperty(itemName);
                                                object value_new = properties_new.GetValue(item, null);
                                                data_new = value_new != null ? value_new.ToString() : "";

                                                if (data_old != data_new)
                                                {
                                                    flag_edited = true;

                                                    rsw_temp.GetType().GetProperty(itemName).SetValue(rsw_temp, value_new, null);
                                                }
                                            }
                                        }


                                        if (flag_edited) // если записи отличаются
                                        {

                                            db.ObjectStateManager.ChangeObjectState(rsw_temp, System.Data.EntityState.Modified);

                                        }

                                    }
                                    if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        item.FormsRW3_2015_ID = RWdata.ID;
                                        FormsRW3_2015_Razd_3 r = new FormsRW3_2015_Razd_3();

                                        foreach (var itemName_ in names)
                                        {
                                            string itemName = itemName_.TrimStart('_');
                                            var properties = item.GetType().GetProperty(itemName);
                                            if (properties != null)
                                            {
                                                object value = properties.GetValue(item, null);
                                                var data = value;

                                                r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                            }

                                        }

                                        db.AddToFormsRW3_2015_Razd_3(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Раздела 2 произошла ошибка. Код ошибки: " + ex.Message);

                            }
                            #endregion



                            flag_ok = true;
                        }

                        if (flag_ok)
                        {
                            try
                            {
                                db.SaveChanges();
                                allowClose = true;
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


            //    this.Close();
        }

        /// <summary>
        /// Сбор данных с основной экранной формы редактировния формы РСВ-1
        /// </summary>
        private void getValues()
        {
            var fields = typeof(FormsRW3_2015).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                            //  DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                            string type = RWdata.GetType().GetProperty(itemName).PropertyType.FullName;
                            if (type.Contains("["))
                                type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                            type = type.Split(',')[0].Split('.')[1].ToLower();
                            if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text != "")
                            {
                                switch (type)
                                {
                                    case "decimal":
                                        RWdata.GetType().GetProperty(itemName).SetValue(RWdata, Math.Round(decimal.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text), 2, MidpointRounding.AwayFromZero), null);
                                        break;
                                    case "integer":
                                        RWdata.GetType().GetProperty(itemName).SetValue(RWdata, int.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString()), null);
                                        break;
                                    case "int64":
                                        RWdata.GetType().GetProperty(itemName).SetValue(RWdata, long.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString()), null);
                                        break;
                                    case "datetime":
                                        RWdata.GetType().GetProperty(itemName).SetValue(RWdata, DateTime.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text), null);
                                        break;
                                    case "string":
                                        RWdata.GetType().GetProperty(itemName).SetValue(RWdata, ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text, null);
                                        break;
                                }
                            }
                            else
                                RWdata.GetType().GetProperty(itemName).SetValue(RWdata, null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message + "    " + itemName);
                    }
                }

            }
        }

        private void ConfirmType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (ConfirmType.SelectedIndex > 0)
            {
                ConfirmDocType.Items.FirstOrDefault(x => x.DisplayValue.ToString() == "ПРОЧЕЕ").Selected = true;
                ConfirmDocName.Text = "ДОВЕРЕННОСТЬ";
                ConfirmOrgName.Enabled = true;
                ConfirmDocSerLat.Enabled = true;
                ConfirmDocNum.Enabled = true;
                ConfirmDocKemVyd.Enabled = true;
                ConfirmDocDate.Enabled = true;
                ConfirmDocDateBegin.Enabled = true;
                ConfirmDocDateEnd.Enabled = true;
                ConfirmDocDateMaskedEditBox.Enabled = true;
                ConfirmDocDateBeginMaskedEditBox.Enabled = true;
                ConfirmDocDateEndMaskedEditBox.Enabled = true;
            }
            else
            {
                ConfirmDocType.SelectedIndex = -1;
                ConfirmDocName.Text = "";
                ConfirmOrgName.Enabled = false;
                ConfirmDocType.Enabled = false;
                ConfirmDocSerLat.Enabled = false;
                ConfirmDocNum.Enabled = false;
                ConfirmDocKemVyd.Enabled = false;
                ConfirmDocDate.Enabled = false;
                ConfirmDocDateBegin.Enabled = false;
                ConfirmDocDateEnd.Enabled = false;
                ConfirmDocDateMaskedEditBox.Enabled = false;
                ConfirmDocDateBeginMaskedEditBox.Enabled = false;
                ConfirmDocDateEndMaskedEditBox.Enabled = false;

            }
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

        private void ConfirmDocDateBegin_ValueChanged(object sender, EventArgs e)
        {
            if (ConfirmDocDateBegin.Value != ConfirmDocDateBegin.NullDate)
                ConfirmDocDateBeginMaskedEditBox.Text = ConfirmDocDateBegin.Value.ToShortDateString();
            else
                ConfirmDocDateBeginMaskedEditBox.Text = ConfirmDocDateBeginMaskedEditBox.NullText;

        }

        private void ConfirmDocDateBeginMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (ConfirmDocDateBeginMaskedEditBox.Text != ConfirmDocDateBeginMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(ConfirmDocDateBeginMaskedEditBox.Text, out date))
                {
                    ConfirmDocDateBegin.Value = date;
                }
                else
                {
                    ConfirmDocDateBegin.Value = ConfirmDocDateBegin.NullDate;
                }
            }
            else
            {
                ConfirmDocDateBegin.Value = ConfirmDocDateBegin.NullDate;
            }
        }

        private void ConfirmDocDateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (ConfirmDocDateEnd.Value != ConfirmDocDateEnd.NullDate)
                ConfirmDocDateEndMaskedEditBox.Text = ConfirmDocDateEnd.Value.ToShortDateString();
            else
                ConfirmDocDateEndMaskedEditBox.Text = ConfirmDocDateEndMaskedEditBox.NullText;

        }

        private void ConfirmDocDateEndMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (ConfirmDocDateEndMaskedEditBox.Text != ConfirmDocDateEndMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(ConfirmDocDateEndMaskedEditBox.Text, out date))
                {
                    ConfirmDocDateEnd.Value = date;
                }
                else
                {
                    ConfirmDocDateEnd.Value = ConfirmDocDateEnd.NullDate;
                }
            }
            else
            {
                ConfirmDocDateEnd.Value = ConfirmDocDateEnd.NullDate;
            }
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


        private void calcFields_200_210(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                s_220_0.EditValue = decimal.Parse(s_200_0.EditValue.ToString()) - decimal.Parse(s_210_0.EditValue.ToString());
                s_220_1.EditValue = decimal.Parse(s_200_1.EditValue.ToString()) - decimal.Parse(s_210_1.EditValue.ToString());
                s_220_2.EditValue = decimal.Parse(s_200_2.EditValue.ToString()) - decimal.Parse(s_210_2.EditValue.ToString());
                s_220_3.EditValue = decimal.Parse(s_200_3.EditValue.ToString()) - decimal.Parse(s_210_3.EditValue.ToString());
            }
        }

        private void calcFields_220(object sender, EventArgs e)
        {
            if (autoCalc)
            {
                if (!editLoad)
                {
                    decimal koef = CodeBase != null ? (CodeTarDDL.SelectedIndex == 0 ? CodeBase.Tar21 : CodeBase.Tar22) : 0;

                    s_110_0.EditValue = decimal.Parse(s_220_0.EditValue.ToString()) * koef / 100;
                    s_111_0.EditValue = decimal.Parse(s_220_1.EditValue.ToString()) * koef / 100;
                    s_112_0.EditValue = decimal.Parse(s_220_2.EditValue.ToString()) * koef / 100;
                    s_113_0.EditValue = decimal.Parse(s_220_3.EditValue.ToString()) * koef / 100;
                }
            }
        }


        private void calcFields_111_112_113(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                s_114_0.EditValue = decimal.Parse(s_111_0.EditValue.ToString()) + decimal.Parse(s_112_0.EditValue.ToString()) + decimal.Parse(s_113_0.EditValue.ToString());
            }
        }

        private void calcFields_141_142_143(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                s_144_0.EditValue = decimal.Parse(s_141_0.EditValue.ToString()) + decimal.Parse(s_142_0.EditValue.ToString()) + decimal.Parse(s_143_0.EditValue.ToString());
            }
        }

        private void calcFields_100_110_120(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                s_130_0.EditValue = decimal.Parse(s_100_0.EditValue.ToString()) + decimal.Parse(s_110_0.EditValue.ToString()) + decimal.Parse(s_120_0.EditValue.ToString());
            }
        }

        private void calcFields_130_140(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                s_150_0.EditValue = decimal.Parse(s_130_0.EditValue.ToString()) - decimal.Parse(s_140_0.EditValue.ToString());
            }
        }

        private void addBtn_3_Click(object sender, EventArgs e)
        {
            RW3_2015_3_Edit child = new RW3_2015_3_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.action = "add";
            child.ShowDialog();
            if (child.formData != null)
            {
                RW_3_3_List.Add(child.formData);
                gridUpdate_3();
                updateTextBoxes_Razd_3();
            }
        }

        private void editBtn_3_Click(object sender, EventArgs e)
        {
            if (grid_3.RowCount != 0)
            {
                int rowindex = grid_3.CurrentRow.Index;

                RW3_2015_3_Edit child = new RW3_2015_3_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.formData = RW_3_3_List.Skip(rowindex).Take(1).First();
                child.ShowDialog();

                if (child.formData != null)
                {
                    RW_3_3_List.RemoveAt(rowindex);
                    RW_3_3_List.Insert(rowindex, child.formData);

                    gridUpdate_3();
                    updateTextBoxes_Razd_3();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void delBtn_3_Click(object sender, EventArgs e)
        {
            if (grid_3.RowCount != 0)
            {
                int rowindex = grid_3.CurrentRow.Index;

                RW_3_3_List.RemoveAt(rowindex);

                gridUpdate_3();
                updateTextBoxes_Razd_3();

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void grid_3_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_3_Click(null, null);
        }

        private void AutoCalcSwitch_Toggled(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                autoCalc = !AutoCalcSwitch.IsOn;
                s_100_0.Enabled = AutoCalcSwitch.IsOn;


                if (!AutoCalcSwitch.IsOn)
                {
                    DialogResult dialogResult = RadMessageBox.Show(this, "Вы выбрали режим автоматического расчета страховых взносов.\r\nПроизвести перерасчет взносов?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    if (dialogResult == DialogResult.Yes)
                    {
                        if (autoCalc)
                        {
                            s_100_0.EditValue = RWdataPrev != null ? (RWdataPrev.s_150_0.HasValue ? RWdataPrev.s_150_0.Value : (decimal)0) : (decimal)0;

                        }
                        calcFields_220(null, null);
                        s_120_0.EditValue = RW_3_3_List.Count != 0 ? RW_3_3_List.Where(x => x.SumFee.HasValue).Sum(x => x.SumFee.Value) : (decimal)0;

//                        calcFields_Razd_2();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                    }
                }


            }
        }

        private void CodeTarDDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (CodeBase != null)
                tariffLabel.Text = "Ставка: " + (CodeTarDDL.SelectedIndex == 0 ? Utils.decToStr(CodeBase.Tar21) : Utils.decToStr(CodeBase.Tar22));
            else
                tariffLabel.Text = "Ставка не определена...";
            calcFields_220(null, null);
        }

        private void RW3_2015_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (allowClose)
            {

                RWdata = null;
                db.Dispose();
            }
            else
            {
                DialogResult dialogResult = RadMessageBox.Show("Вы хотите сохранить изменения перед закрытием формы?", "Сохранение записи!", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button3);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        radButton1_Click(null, null);
                        break;
                    case DialogResult.No:
                        RWdata = null;
                        db.Dispose();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }

            }
        }

    }
}
