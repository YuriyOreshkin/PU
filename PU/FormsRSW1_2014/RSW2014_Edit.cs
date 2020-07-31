using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using PU.Models;
using Telerik.WinControls.UI;
using System.Data.Objects;
using System.Data.Entity;
using System.Reflection;

namespace PU.FormsRSW2014
{

    public partial class RSW2014_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool editLoad { get; set; }
        public short yearType { get; set; }
        BackgroundWorker bw;
        Font totalRowsFont;

        public FormsRSW2014_1_1 RSWdata { get; set; }
        public FormsRSW2014_1_1 RSWdataPrev { get; set; }

        public List<FormsRSW2014_1_Razd_2_1> RSW_2_1_List = new List<FormsRSW2014_1_Razd_2_1>();
        public List<FormsRSW2014_1_Razd_2_4> RSW_2_4_List = new List<FormsRSW2014_1_Razd_2_4>();
        public List<FormsRSW2014_1_Razd_3_4> RSW_3_4_List = new List<FormsRSW2014_1_Razd_3_4>();
        public List<FormsRSW2014_1_Razd_4> RSW_4_List = new List<FormsRSW2014_1_Razd_4>();
        public List<FormsRSW2014_1_Razd_5> RSW_5_List = new List<FormsRSW2014_1_Razd_5>();
        public List<FormsRSW2014_1_Razd_6_1> RSW_6_1_List = new List<FormsRSW2014_1_Razd_6_1>();
        int grid_6_1_rowindex = 0;
        bool allowClose = false;
        byte corrNumOld = 0;

        public List<TimerList> timer = new List<TimerList>();

        private List<ErrList> errMessBox = new List<ErrList>();

        public RSW2014_Edit()
        {
            InitializeComponent();
            totalRowsFont = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            this.MouseWheel += new MouseEventHandler(Panel1_MouseWheel);
        }


        private void CorrectionNum_ValueChanged(object sender, EventArgs e)
        {
            if (CorrectionNum.Value == 0)
            {
                CorrectionType.Enabled = false;
                CorrectionType.Text = "";
                fillRazd21Btn.Enabled = true;
            }
            else
            {
                CorrectionType.Enabled = true;
                fillRazd21Btn.Enabled = false;
            }

            changeUniqIdentifier(); // Изменение набора уникальных идентификаторов в списках формы
            gridUpdate_6_1_();
        }

        public void changeFormByYear()
        {
            if (yearType == 2015)
            {
                //                reducedRateLabel.Visible = true;
                //                reducedRateDDL.Visible = true;

                Razd1tableLayoutPanel.RowStyles[11].Height = 0;
                Razd1tableLayoutPanel.RowStyles[12].Height = 0;
                s_122_0.Visible = false;
                s_122_1.Visible = false;
                s_122_2.Visible = false;
                s_122_3.Visible = false;
                s_122_4.Visible = false;
                s_122_5.Visible = false;
                s_123_0.Visible = false;
                s_123_1.Visible = false;
                label155.Visible = false;
                label156.Visible = false;
                label158.Visible = false;
                label159.Visible = false;
                panel154.Visible = false;

                this.radPageView1.Pages[5].Item.Visibility = ElementVisibility.Collapsed;
                this.radPageView1.Pages[6].Item.Visibility = ElementVisibility.Collapsed;
                this.radPageView1.Pages[8].Item.Visibility = ElementVisibility.Collapsed;

                this.radPageView1.Pages[7].Text = "Раздел 3.1";
                this.radPageView1.Pages[9].Text = "Раздел 3.2";
                this.radPageView1.Pages[10].Text = "Раздел 3.3";

                label138.Text = "3.1." + label138.Text.Substring(4);
                label117.Text = "3.2." + label117.Text.Substring(4);
                label147.Text = "3.3." + label147.Text.Substring(4);

                label35.Text = "Сумма перерасчета страховых взносов за предыдущие отчетные (расчетные) периоды с начала расчетного периода";
                label36.Text = "в т.ч., с сумм, превышаюших предельную величину базы для начисления страховых взносов";
                label57.Text = "на финансирование страховой пенсии";
                label58.Text = "на финансирование накопительной пенсии";

                radLabel2.Text = "Номер уточнения";
                radLabel3.Text = "Причина уточнения";

                radLabel34.Text = "Раздел 4. Суммы перерасчета страховых взносов с начала расчетного периода";

            }
            else if (yearType == 2014 || yearType == 2012)
            {
                Razd1tableLayoutPanel.RowStyles[11].Height = 0;
                Razd1tableLayoutPanel.RowStyles[12].Height = 0;
                s_122_0.Visible = false;
                s_122_1.Visible = false;
                s_122_2.Visible = false;
                s_122_3.Visible = false;
                s_122_4.Visible = false;
                s_122_5.Visible = false;
                s_123_0.Visible = false;
                s_123_1.Visible = false;
                label155.Visible = false;
                label156.Visible = false;
                label158.Visible = false;
                label159.Visible = false;
                panel154.Visible = false;

                label35.Text = "Доначислено страховых взносов с начала расчетного периода, всего";
                label36.Text = "в т.ч. с сумм, превышающих предельную величину базы";
                label57.Text = "страховая часть";
                label58.Text = "накопительная часть";

                radLabel2.Text = "Номер корректировки";
                radLabel3.Text = "Тип корректировки";

                radLabel34.Text = "Раздел 4. Суммы доначисленных страховых взносов с начала расчетного периода";

            }

        }

        private void RSW2014_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn { Name = "Num_", HeaderText = "#", IsPinned = true, VisibleInColumnChooser = false, ReadOnly = true, Width = 40, AllowSort = false, AllowFiltering = false, DataType = typeof(int) });

            ///Титульная страница
            this.radPageView1.SelectedPage = this.radPageView1.Pages[0];

            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);
            if (yearType == 2014)
            {
                avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year == 2014 || (x.Year == 2015 && x.Kvartal == 3)).OrderBy(x => x.Year);
            }
            if (yearType == 2015)
            {
                avail_periods = Options.RaschetPeriodInternal.Where(x => (x.Year == 2015 && (x.Kvartal == 6 || x.Kvartal == 9 || x.Kvartal == 0)) || (x.Year >= 2016)).OrderBy(x => x.Year);
            }
            DateUnderwrite.Value = DateTime.Now.Date;
            AutoCalcSwitch.IsOn = RSWdata.AutoCalc.HasValue ? RSWdata.AutoCalc.Value : false;


            #region Тип документов DDL
            BindingSource b = new BindingSource();
            b.DataSource = db.DocumentTypes;

            this.ConfirmDocType.DataSource = null;
            this.ConfirmDocType.Items.Clear();
            this.ConfirmDocType.DisplayMember = "Code";
            this.ConfirmDocType.ValueMember = "ID";
            this.ConfirmDocType.ShowItemToolTips = true;
            this.ConfirmDocType.DataSource = b.DataSource;
            this.ConfirmDocType.SelectedIndex = -1;
            //this.ConfirmDocType.ResetText();

            #endregion


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
                    break;
                case "edit":
                    if (Year.Items.Any(x => x.Text.ToString() == RSWdata.Year.ToString()))
                        Year.Items.Single(x => x.Text.ToString() == RSWdata.Year.ToString()).Selected = true;
                    //					Year.Items.Single(x => x.Text.ToString() == RSWdata.Year.ToString()).Selected = true;
                    break;
            }



            // выпад список "Отчетный период"
            this.Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                getBase(Year.SelectedItem.Text);

                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                switch (action)
                {
                    case "add":
                        if (Quarter.Items.Count() > 0)
                        {
                            DateTime dt = DateTime.Now.AddDays(-45);
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
                        if (Quarter.Items.Any(x => x.Value.ToString() == RSWdata.Quarter.ToString()))
                            Quarter.Items.Single(x => x.Value.ToString() == RSWdata.Quarter.ToString()).Selected = true;
                        break;
                }
                try
                {
                    getPrevData();
                }
                catch
                { }
            }



            switch (action)
            {
                case "add":

                    RSWdata.InsurerID = Options.InsID;

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



                    RSWdata.CorrectionNum = byte.Parse(CorrectionNum.Value.ToString());
                    changeUniqIdentifier(); // Изменение набора уникальных идентификаторов в списках формы

                    updateComplete(null, null);
                    break;
                case "edit":
                    editLoad = true;

                    CorrectionNum.Value = RSWdata.CorrectionNum;
                    if (RSWdata.CorrectionNum > 0 && RSWdata.CorrectionType.HasValue)
                    {
                        CorrectionType.SelectedIndex = RSWdata.CorrectionType.Value - 1;
                    }

                    if (yearType == 2015)
                    {
                        if (!String.IsNullOrEmpty(RSWdata.ReducedRate))
                            reducedRateDDL.Items.FirstOrDefault(x => x.Tag.ToString() == RSWdata.ReducedRate.ToString()).Selected = true;
                    }


                    WorkStop.Checked = RSWdata.WorkStop == 0 ? false : true;
                    CountEmployers.Value = RSWdata.CountEmployers.HasValue ? RSWdata.CountEmployers.Value : 0;
                    CountAverageEmployers.Value = RSWdata.CountAverageEmployers.HasValue ? RSWdata.CountAverageEmployers.Value : 0;
                    CountConfirmDoc.Value = RSWdata.CountConfirmDoc.HasValue ? RSWdata.CountConfirmDoc.Value : 0;
                    DateUnderwrite.Value = RSWdata.DateUnderwrite.HasValue ? RSWdata.DateUnderwrite.Value : DateTime.Now;

                    ConfirmType.SelectedIndex = RSWdata.ConfirmType - 1;
                    ConfirmLastName.Text = RSWdata.ConfirmLastName;
                    ConfirmFirstName.Text = RSWdata.ConfirmFirstName;
                    ConfirmMiddleName.Text = RSWdata.ConfirmMiddleName;
                    ConfirmOrgName.Text = RSWdata.ConfirmOrgName;

                    if (RSWdata.ConfirmDocType_ID.HasValue)
                    {
                        ConfirmDocType.Items.FirstOrDefault(x => x.Value.ToString() == RSWdata.ConfirmDocType_ID.Value.ToString()).Selected = true;
                        ConfirmDocName.Text = RSWdata.ConfirmDocName;
                        ConfirmDocSerLat.Text = RSWdata.ConfirmDocSerLat;
                        ConfirmDocSerRus.Text = RSWdata.ConfirmDocSerRus;
                        ConfirmDocNum.Text = RSWdata.ConfirmDocNum.HasValue ? RSWdata.ConfirmDocNum.Value.ToString() : "";
                        if (RSWdata.ConfirmDocDate.HasValue)
                            ConfirmDocDate.Value = RSWdata.ConfirmDocDate.Value;
                        ConfirmDocKemVyd.Text = RSWdata.ConfirmDocKemVyd;

                    }

                    //      updateValues(null,null);
                    editLoad = false;

                    break;
            }

            corrNumOld = RSWdata.CorrectionNum;

            /*		List<string> list = new List<string> { };
                    for (int i = 0; i <= 5; i++)
                    {
                        list.Add("s_140_" + i.ToString());
                    }
                    list.Add("s_220_0");
                    list.Add("s_221_0");
                    list.Add("s_230_0");
                    list.Add("s_231_0");
                    list.Add("s_334_0");
                    list.Add("s_335_0");

                    foreach (var item in list)
                    {
                        DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0];
                        box.Enabled = Options.inputTypeRSW1 == 0 ? true : false;
                    }

                    this.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                    gridUpdate_6_1(null,null);

                    ConfirmType_SelectedIndexChanged(null,null);

                    this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
                    this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();
                    this.SearchBy.SelectedIndexChanged += (s, с) => SearchBy_SelectedIndexChanged();
                    this.SearchTextBox.TextChanged += (s, с) => SearchTextBox_TextChanged();
                    this.ConfirmDocType.SelectedIndexChanged += (s, с) => ConfirmDocTypeDDL_SelectedIndexChanged();
            
                    */
            //            timer.Add(new TimerList { stamp = DateTime.Now, name = "Конец RSW2014_Edit_Load" });

            // Таймер тестирования времени отрисовки формы, чисто для теста

            /*			string s2 = "";
                        int k = 0;
                        foreach (var item in timer)
                        {
                            if (k == 0)
                                s2 = item.stamp.ToLongTimeString() + ":" + item.stamp.Millisecond + "    -   " + item.name + "\r\n";
                            else
                                s2 = s2 + item.stamp.ToLongTimeString() + ":" + item.stamp.Millisecond + "      " + (item.stamp - timer[k - 1].stamp).Seconds + ":" + (item.stamp - timer[k - 1].stamp).Milliseconds + "      " + (item.stamp - timer[0].stamp).Seconds + ":" + (item.stamp - timer[0].stamp).Milliseconds + "    -   " + item.name + "\r\n";

                            k++;
                        }
                        */
            //            RadMessageBox.Show(this, s2);
        }



        /// <summary>
        /// Обновление полей формы РСВ-1 и таблиц
        /// </summary>
        private void updateValues(object sender, DoWorkEventArgs e)
        {
            var fields = typeof(FormsRSW2014_1_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            List<string> radNames = new List<string> { "s_345_0", "s_351_0", "s_501_0_0", "s_501_0_1", "s_501_0_2", "s_501_0_3" };

            foreach (var item in names)
            {
                if (bw.CancellationPending)
                {
                    return;
                }
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                            if (Options.inputTypeRSW1 == 2)
                            {
                                try
                                {
                                    if (!radNames.Contains(itemName))
                                    {
                                        ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Enabled = true; }));
                                    }
                                    else
                                    {
                                        ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Enabled = true; }));
                                    }

                                }
                                catch (Exception ex)
                                {
                                    RadMessageBox.Show(ex.Message);
                                }
                            }

                            if (RSWdata != null)
                            {
                                var properties = RSWdata.GetType().GetProperty(itemName);
                                object value = properties.GetValue(RSWdata, null);
                                string type = properties.PropertyType.FullName;
                                if (type.Contains("["))
                                    type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                                type = type.Split(',')[0].Split('.')[1].ToLower();

                                //DevExpress.XtraEditors.TextEdit box = new DevExpress.XtraEditors.TextEdit();
                                //RadDateTimePicker box2 = new RadDateTimePicker();

                                /*          if (type != "datetime")
                                          {
                                              box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];
                                          }
                                          else
                                          {
                                              box2 = (RadDateTimePicker)this.Controls.Find(itemName, true)[0];
                                          }
                                             */



                                if (value != null)
                                {
                                    switch (type)
                                    {
                                        case "decimal":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = decimal.Parse(value.ToString()); }));
                                            //          ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = decimal.Parse(value.ToString());
                                            break;
                                        case "integer":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = int.Parse(value.ToString()); }));
                                            //            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = int.Parse(value.ToString());
                                            break;
                                        case "int64":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = long.Parse(value.ToString()); }));
                                            //              ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = long.Parse(value.ToString());
                                            break;
                                        case "datetime":
                                            ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Value = DateTime.Parse(value.ToString()).Date; }));
                                            //                ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Value = DateTime.Parse(value.ToString()).Date;
                                            break;
                                        case "string":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = value.ToString(); }));
                                            //                  ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = value.ToString();
                                            break;

                                    }
                                }
                                else
                                {
                                    switch (type)
                                    {
                                        case "decimal":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = (decimal)0; }));
                                            //                                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = 0;
                                            break;
                                        case "integer":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = 0; }));
                                            //                                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = 0;
                                            break;
                                        case "int64":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = 0; }));
                                            //                                  ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = 0;
                                            break;
                                        case "datetime":
                                            ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Value = ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).NullDate; }));
                                            //                                    ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Value = ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).NullDate;
                                            break;
                                        case "string":
                                            ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Invoke(new Action(() => { ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = ""; }));
                                            //                                      ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = "";
                                            break;
                                        /*											  case "decimal":
                                                                                          box.EditValue = 0;
                                                                                          break;
                                                                                      case "integer":
                                                                                          box.EditValue = 0;
                                                                                          break;
                                                                                      case "int64":
                                                                                          box.EditValue = 0;
                                                                                          break;
                                                                                      case "datetime":
                                                                                          box2.Value = box2.NullDate;
                                                                                          break;
                                                                                      case "string":
                                                                                          box.EditValue = "";
                                                                                          break;*/
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


        }



        private void getPrevData()
        {
            RSWdataPrev = null;

            if (Options.inputTypeRSW1 == 1)
            {
                short y = 0;
                if (short.TryParse(Year.SelectedItem.Text, out y))
                {
                    byte q;
                    if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                    {
                        if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                        {
                            byte quarter = 20;
                            if (q == 6)
                                quarter = 3;
                            else if (q == 9)
                                quarter = 6;
                            else if (q == 0)
                                quarter = 9;

                            if (db.FormsRSW2014_1_1.Any(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID))
                                RSWdataPrev = db.FormsRSW2014_1_1.Where(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                        }
                        else
                        {
                            byte quarter = 0;
                            y--;

                            if (db.FormsRSW2014_1_1.Any(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID))
                                RSWdataPrev = db.FormsRSW2014_1_1.Where(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                        }
                    }
                }


            }

            if (Quarter.Items.Count > 0 && Options.inputTypeRSW1 != 2)
            {
                int n = grid_2_1.Rows.Count();
                for (int rowindex = 0; rowindex < n; rowindex++)
                {
                    //int rowindex = row.Index;

                    RSW2014_2_1_Edit child = new RSW2014_2_1_Edit();
                    child.Hide();
                    child.Owner = this;
                    child.ThemeName = this.ThemeName;
                    child.ShowInTaskbar = false;
                    child.action = "edit";
                    child.yearType = yearType == 2012 ? (short)2014 : yearType;
                    child.changeFormByYear();
                    child.formData = RSW_2_1_List.Skip(rowindex).Take(1).First();
                    child.RSW2014_2_1_Edit_Load(null, null);
                    //child.Show();
                    child.getValues();
                    child.cleanData = false;
                    child.Close();

                    if (child.formData != null)
                    {
                        RSW_2_1_List.RemoveAt(rowindex);
                        RSW_2_1_List.Insert(rowindex, child.formData);

                        gridUpdate_2_1();
                        calcFields_Razd_2_1(null, null);
                    }
                }

                if (grid_2_1.Rows.Count <= 0)
                    calcFields_Razd_2_1(null, null);
                calcFields_Razd_2_2(null, null);
                calcFields_Razd_2_3(null, null);
                calcFields_Razd_3_2(null, null);
                calcFields_144(null, null);
            }

        }

        #region Титульный лист
        private void Year_SelectedIndexChanged()
        {
            getBase(Year.SelectedItem.Text);
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            this.Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);
                if (yearType == 2014)
                {
                    avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year == 2014 || (x.Year == 2015 && x.Kvartal == 3)).OrderBy(x => x.Year);
                }
                if (yearType == 2015)
                {
                    avail_periods = Options.RaschetPeriodInternal.Where(x => (x.Year == 2015 && (x.Kvartal == 6 || x.Kvartal == 9 || x.Kvartal == 0)) || (x.Year == 2016)).OrderBy(x => x.Year);
                }

                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                switch (action)
                {
                    case "add":
                        if (Quarter.Items.Count() > 0)
                        {
                            if (q != 20 && Quarter.Items.Any(x => x.Value.ToString() == q.ToString()))
                                Quarter.Items.FirstOrDefault(x => x.Value.ToString() == q.ToString()).Selected = true;
                            else
                                Quarter.Items.First().Selected = true;
                        }
                        break;
                }
            }
            Quarter_SelectedIndexChanged();

            changeUniqIdentifier(); // Изменение набора уникальных идентификаторов в списках формы
        }

        private void Quarter_SelectedIndexChanged()
        {
            if (Quarter.SelectedItem != null)
            {

                changeUniqIdentifier(); // Изменение набора уникальных идентификаторов в списках формы
                gridUpdate_6_1(null, null);
                this.Text = "Форма расчета страховых взносов РСВ-1  - [ " + Quarter.SelectedItem.Text + " ]";

            }
            getPrevData();
            calcFields_100(null, null);

        }

        #endregion


        #region Раздел 6
        delegate void UpdateGridHandler();
        delegate void UpdateGridThreadHandler();

        /// <summary>
        /// Заполнение из базы списка раздела 6, для дальнейшего заполнения таблицы Раздела 6 
        /// </summary>

        public void gridUpdate_6_1(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateGridHandler ug = gridUpdate_6_1_first;
            ug.BeginInvoke(cb, null);
        }

        private void cb(IAsyncResult res)
        {

        }

        public void gridUpdate_6_1_first()
        {
            if (radGridView1.InvokeRequired)
            {
                UpdateGridThreadHandler handler = gridUpdate_6_1_;
                radGridView1.BeginInvoke(handler, null);
            }
            else
            {
                //radGridView1.DataSource = table;
            }
        }

        public void gridUpdate_6_1_()
        {
            if (Year.SelectedItem != null && Quarter.SelectedItem != null)
            {
                //      if (Year.SelectedItem != null)
                //          Year.SelectedIndex = 0;

                short y = (short)0;

                try
                {
                    y = short.Parse(Year.SelectedItem.Value.ToString());
                }
                catch
                { }



                //    if (Quarter.SelectedItem != null)
                //        Quarter.SelectedIndex = 0;

                byte q = (byte)0;

                try
                {
                    q = byte.Parse(Quarter.SelectedItem.Value.ToString());
                }
                catch
                { }
                byte c = byte.Parse(CorrectionNum.Value.ToString());

                RSW_6_1_List = db.FormsRSW2014_1_Razd_6_1.Where(x => x.InsurerID == Options.InsID && x.Quarter == q && x.Year == y).OrderBy(x => x.ID).ToList();

                long RSWid = RSWdata == null ? 0 : RSWdata.ID;


                List<FormsRSW2014_1_Razd_6_1> rsw = new List<FormsRSW2014_1_Razd_6_1>();

                /*                else
                                {
                                    rsw = RSW_6_1_List.ToList();
                                }
                                */
                //             this.radGridView1.TableElement.BeginUpdate();
                //       radGridView1.Rows.Clear();
                //            radGridView1.DataSource = null;
                List<StaffObject> staffList = new List<StaffObject> { };


                if (RSW_6_1_List.Count() != 0)
                {
                    int i = 0;
                    List<long> listId = RSW_6_1_List.Select(a => a.StaffID).ToList();
                    List<Staff> staff = db.Staff.Where(x => listId.Contains(x.ID)).ToList();
                    foreach (var item in RSW_6_1_List)
                    {
                        i++;
                        var st = staff.First(x => x.ID == item.StaffID);


                        string dateb = "";
                        if (st.DateBirth != null)
                        {
                            dateb = st.DateBirth.HasValue ? st.DateBirth.Value.ToShortDateString() : "";
                        }
                        string contrNum = "";
                        if (st.ControlNumber != null)
                        {
                            contrNum = st.ControlNumber.HasValue ? st.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                        }

                        staffList.Add(new StaffObject()
                        {
                            ID = item.ID,
                            //                            Num = i,
                            FIO = st.LastName + " " + st.FirstName + " " + st.MiddleName,
                            SNILS = !String.IsNullOrEmpty(st.InsuranceNumber) ? st.InsuranceNumber.Substring(0, 3) + "-" + st.InsuranceNumber.Substring(3, 3) + "-" + st.InsuranceNumber.Substring(6, 3) + " " + contrNum : "",
                            TabelNumber = st.TabelNumber,// != null ? st.TabelNumber.Value.ToString() : ""
                            Sex = st.Sex.HasValue ? (st.Sex.Value == 0 ? "М" : "Ж") : "",
                            Dismissed = st.Dismissed.HasValue ? (st.Dismissed.Value == 1 ? "У" : " ") : " ",
                            DateBirth = dateb,
                            TypeInfo = item.TypeInfo.Name,
                            Period = item.Quarter.ToString() + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Quarter).Name
                        });

                        string q_ = item.QuarterKorr.HasValue ? item.QuarterKorr.Value.ToString() : "";
                        string y_ = item.YearKorr.HasValue ? item.YearKorr.Value.ToString() : "";
                        staffList.Last().KorrPeriod = q_ == "" || y_ == "0" ? "" : q_ + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == item.YearKorr.Value && x.Kvartal == item.QuarterKorr.Value).Name;


                    }
                }


                //            staffGridView.Rows.Clear();
                //                if (CorrectionNum.Value == 0)
                //                {
                radGridView1.DataSource = staffList.OrderBy(x => x.FIO);

                if (staffList.Count > 0)
                {
                    radGridView1.Columns["ID"].IsVisible = false;
                    radGridView1.Columns["ID"].IsPinned = true;
                    radGridView1.Columns["Num_"].Width = 40;
                    radGridView1.Columns["Num"].IsVisible = false;
                    radGridView1.Columns["FIO"].Width = 210;
                    radGridView1.Columns["FIO"].IsPinned = true;
                    radGridView1.Columns["FIO"].ReadOnly = true;
                    radGridView1.Columns["FIO"].HeaderText = "Фамилия Имя Отчество";
                    radGridView1.Columns["SNILS"].Width = 100;
                    radGridView1.Columns["SNILS"].IsPinned = true;
                    radGridView1.Columns["SNILS"].HeaderText = "СНИЛС";
                    radGridView1.Columns["TabelNumber"].HeaderText = "Табел.№";
                    radGridView1.Columns["TabelNumber"].Width = 70;
                    radGridView1.Columns["Sex"].HeaderText = "Пол";
                    radGridView1.Columns["Sex"].Width = 40;
                    radGridView1.Columns["Dismissed"].HeaderText = "Уволен";
                    radGridView1.Columns["Dismissed"].Width = 50;
                    radGridView1.Columns["DateBirth"].HeaderText = "Дата рожд.";
                    radGridView1.Columns["DateBirth"].Width = 80;
                    radGridView1.Columns["DepName"].HeaderText = "Подразделение";
                    radGridView1.Columns["DepName"].IsVisible = false;
                    radGridView1.Columns["Period"].HeaderText = "Отчет.пер.";
                    radGridView1.Columns["Period"].Width = 130;
                    radGridView1.Columns["Period"].IsVisible = false;
                    radGridView1.Columns["TypeInfo"].HeaderText = "Тип инфо.";
                    radGridView1.Columns["TypeInfo"].Width = 100;
                    radGridView1.Columns["TypeInfo"].IsVisible = true;
                    radGridView1.Columns["KorrPeriod"].HeaderText = "Корр.пер.";
                    radGridView1.Columns["KorrPeriod"].Width = 130;
                    radGridView1.Columns["KorrPeriod"].IsVisible = true;
                    radGridView1.Columns["InsReg"].IsVisible = false;
                    radGridView1.Columns["InsName"].IsVisible = false;

                    for (var i = 4; i < radGridView1.Columns.Count; i++)
                    {
                        radGridView1.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                        radGridView1.Columns[i].ReadOnly = true;
                    }
                }
                //                }

                //  this.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                //              this.radGridView1.TableElement.EndUpdate();


            }
            //            radGridView1.Refresh();   // мерцание таблицы при обновлении
            //            RadMessageBox.Show(radGridView1.RowCount.ToString());
            if (radGridView1.RowCount > 0)
                radGridView1.Rows[grid_6_1_rowindex].IsCurrent = true;


        }


        private void radButton4_Click(object sender, EventArgs e)
        {
            if (Year.SelectedItem == null || Quarter.SelectedItem == null || Year.SelectedItem.Text == "" || Quarter.SelectedItem.Text == "")
            {
                RadMessageBox.Show(this, "Невыбран отченый период", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);

            }
            else
            {
                RSW2014_6 child = new RSW2014_6();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.parentName = this.Name;
                child.Year.Text = Year.Text;
                child.Quarter.Text = Quarter.SelectedItem.Text;
                child.period = byte.Parse(Quarter.SelectedItem.Value.ToString());
                //            child.CorrNum = (byte)0;
                child.action = "add";
                child.ShowDialog();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                gridUpdate_6_1(null, null);
            }
        }

        #endregion



        private void getBase(string Year)
        {
            if (!String.IsNullOrEmpty(Year))
            {
                short y;

                if (short.TryParse(Year, out y))
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
                radLabel17.Text = "Максимальная база: " + Options.mrot.NalogBase.ToString() + " рублей в " + Options.mrot.Year.ToString() + " финансовом году";
            else
                radLabel17.Text = "Максимальная база для начисления взносов - не определена...";

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                long id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());

                db = new pu6Entities();
                FormsRSW2014_1_Razd_6_1 rsw_6 = db.FormsRSW2014_1_Razd_6_1.Include(x => x.FormsRSW2014_1_Razd_6_4).FirstOrDefault(x => x.ID == id);

                RSW2014_6 child = new RSW2014_6();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                //                child.Year.Text = rsw_6.Year.ToString();
                //                child.Quarter.Text = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == rsw_6.Quarter && x.Year == rsw_6.Year).Name;
                child.period = rsw_6.Quarter;
                //             child.CorrNum = rsw_6.CorrectionNum.Value;
                child.action = "edit";
                child.parentName = this.Name;
                child.RSW_6 = rsw_6;
                child.RSW_6_4_List = rsw_6.FormsRSW2014_1_Razd_6_4.OrderBy(x => x.ID).ToList();
                child.RSW_6_6_List = rsw_6.FormsRSW2014_1_Razd_6_6.OrderBy(x => x.ID).ToList();
                child.RSW_6_7_List = rsw_6.FormsRSW2014_1_Razd_6_7.OrderBy(x => x.ID).ToList();
                child.staff = rsw_6.Staff;
                child.ShowDialog();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();

                gridUpdate_6_1(null, null);
                if (rowindex < radGridView1.RowCount)
                {
                    grid_6_1_rowindex = rowindex;
                }
                else
                {
                    grid_6_1_rowindex = radGridView1.Rows.Last().Index;
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования", "");
            }
        }

        private void tableLayoutPanel1_Scroll(object sender, ScrollEventArgs e)
        {
            //            this.radScrollablePanel1.VerticalScrollbar.PerformSmallIncrement(1);
            //      this.radScrollablePanel1.VerticalScrollbar.PerformLargeIncrement(1);
        }


        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            panel1.Focus();
        }

        private void radButton9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Сохранение, проверка данных формы РСВ-1

        /// <summary>
        /// Проверка введенных данных формы РСВ-1
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            if (Year.Text == "")
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Year" });
            if (Quarter.Text == "")
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Quarter" });

            if (CorrectionNum.Value != 0)
                if (CorrectionType.Text == "")
                    errMessBox.Add(new ErrList { name = "Не выбран тип корректировки", control = "CorrectionType" });

            if (ConfirmType.SelectedItem == null)
                errMessBox.Add(new ErrList { name = "Не выбрано подтверждающее лицо", control = "ConfirmType" });


            int ccd = int.Parse(CountConfirmDoc.Value.ToString());
            if (ccd > 255)
                errMessBox.Add(new ErrList { name = "Количество подтверждающих документов не может быть больше 255", control = "CountConfirmDoc" });


            // Проверка на заполненность разделов

            RSWdata.ExistPart_2_4 = RSW_2_4_List.Any() ? (byte)1 : (byte)0;
            RSWdata.ExistPart_3_4 = RSW_3_4_List.Any() ? (byte)1 : (byte)0;
            RSWdata.ExistPart_4 = RSW_4_List.Any() ? (byte)1 : (byte)0;
            if (RSW_5_List.Any() || s_501_0_0.Value != s_501_0_0.NullDate)
                RSWdata.ExistPart_5 = 1;
            else
                RSWdata.ExistPart_5 = 0;
            RSWdata.ExistPart_2_2 = checkExists(220, 225, (byte)4) ? (byte)1 : (byte)0;
            RSWdata.ExistPart_2_3 = checkExists(230, 235, (byte)4) ? (byte)1 : (byte)0;
            RSWdata.ExistPart_3_1 = checkExists(321, 323, (byte)4) ? (byte)1 : (byte)0;
            RSWdata.ExistPart_3_2 = checkExists(331, 336, (byte)4) ? (byte)1 : (byte)0;
            RSWdata.ExistPart_3_3 = checkExists(341, 345, (byte)2) ? (byte)1 : (byte)0;
            RSWdata.ExistPart_3_5 = checkExists(361, 363, (byte)1) ? (byte)1 : (byte)0;
            RSWdata.ExistPart_3_6 = checkExists(371, 375, (byte)2) ? (byte)1 : (byte)0;


            /*            if (decimal.Parse(s_110_0.EditValue.ToString()) > 0) //Если значение гр.3 стр.110 больше 0, то средсписочная численность не равна 0
                        {
                            if (long.Parse(CountAverageEmployers.Value.ToString()) <= 0)
                                errMessBox.Add(new ErrList { name = "Если значение гр.3 стр.110 больше 0, то средсписочная численность не равна 0" });
                        }
                        */
            /*            if (staff == null)
                        {
                            check = false;
                            errMessBox.Add("Необходимо выбрать сотрудника");
                        }
                        */

            #region Раздел 2.2   10
            if (decimal.Parse(s_224_0.EditValue.ToString()) > 0)
                if (long.Parse(s_225_0.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Стр.225 должна иметь ненулевое значение при ненулевом значении стр.224", control = "s_225_0" });
            if (decimal.Parse(s_224_1.EditValue.ToString()) > 0)
                if (long.Parse(s_225_1.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Стр.225 должна иметь ненулевое значение при ненулевом значении стр.224", control = "s_225_1" });
            if (decimal.Parse(s_224_2.EditValue.ToString()) > 0)
                if (long.Parse(s_225_2.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Стр.225 должна иметь ненулевое значение при ненулевом значении стр.224", control = "s_225_2" });
            if (decimal.Parse(s_224_3.EditValue.ToString()) > 0)
                if (long.Parse(s_225_3.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Стр.225 должна иметь ненулевое значение при ненулевом значении стр.224", control = "s_225_3" });
            #endregion
            #region Раздел 2.3   10
            if (decimal.Parse(s_234_0.EditValue.ToString()) > 0)
                if (long.Parse(s_235_0.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Стр.235 должна иметь ненулевое значение при ненулевом значении стр.234", control = "s_235_0" });
            if (decimal.Parse(s_234_1.EditValue.ToString()) > 0)
                if (long.Parse(s_235_1.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Стр.235 должна иметь ненулевое значение при ненулевом значении стр.234", control = "s_235_1" });
            if (decimal.Parse(s_234_2.EditValue.ToString()) > 0)
                if (long.Parse(s_235_2.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Стр.235 должна иметь ненулевое значение при ненулевом значении стр.234", control = "s_235_2" });
            if (decimal.Parse(s_234_3.EditValue.ToString()) > 0)
                if (long.Parse(s_235_3.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Стр.235 должна иметь ненулевое значение при ненулевом значении стр.234", control = "s_235_3" });
            #endregion

            var tarCodesList = db.TariffCode.ToList();

            #region Раздел 3.1
            if (decimal.Parse(s_321_0.EditValue.ToString()) < decimal.Parse(s_322_0.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.321 должна быть больше или равна стр.322", control = "s_321_0" });
            if (decimal.Parse(s_321_1.EditValue.ToString()) < decimal.Parse(s_322_1.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.321 должна быть больше или равна стр.322", control = "s_321_1" });
            if (decimal.Parse(s_321_2.EditValue.ToString()) < decimal.Parse(s_322_2.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.321 должна быть больше или равна стр.322", control = "s_321_2" });
            if (decimal.Parse(s_321_3.EditValue.ToString()) < decimal.Parse(s_322_3.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.321 должна быть больше или равна стр.322", control = "s_321_3" });

            if (RSWdata.ExistPart_3_1.HasValue && RSWdata.ExistPart_3_1.Value == 1)
            {
                long id = tarCodesList.Any(x => x.Code == "03") ? tarCodesList.FirstOrDefault(x => x.Code == "03").ID : 0;
                if (!RSW_2_1_List.Any(x => x.TariffCodeID == id))
                {
                    errMessBox.Add(new ErrList { name = "Если раздел 3.1 заполнен, то в расчете должен быть подраздел 2.1 раздела 2 с кодом тарифа 03", control = "grid_2_1" });
                }
            }
            #endregion

            #region Раздел 3.2
            if (decimal.Parse(s_331_0.EditValue.ToString()) < decimal.Parse(s_332_0.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.331 должна быть больше или равна стр.332", control = "s_331_0" });
            if (decimal.Parse(s_331_1.EditValue.ToString()) < decimal.Parse(s_332_1.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.331 должна быть больше или равна стр.332", control = "s_331_1" });
            if (decimal.Parse(s_331_2.EditValue.ToString()) < decimal.Parse(s_332_2.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.331 должна быть больше или равна стр.332", control = "s_331_2" });
            if (decimal.Parse(s_331_3.EditValue.ToString()) < decimal.Parse(s_332_3.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.331 должна быть больше или равна стр.332", control = "s_331_3" });

            /*            if (decimal.Parse(s_331_0.EditValue.ToString()) != 0 || decimal.Parse(s_331_1.EditValue.ToString()) != 0 || decimal.Parse(s_331_2.EditValue.ToString()) != 0 || decimal.Parse(s_331_3.EditValue.ToString()) != 0
                             || decimal.Parse(s_332_0.EditValue.ToString()) != 0 || decimal.Parse(s_332_1.EditValue.ToString()) != 0 || decimal.Parse(s_332_2.EditValue.ToString()) != 0 || decimal.Parse(s_332_3.EditValue.ToString()) != 0
                             || decimal.Parse(s_334_0.EditValue.ToString()) != 0 || decimal.Parse(s_334_1.EditValue.ToString()) != 0 || decimal.Parse(s_334_2.EditValue.ToString()) != 0 || decimal.Parse(s_334_3.EditValue.ToString()) != 0
                             || decimal.Parse(s_335_0.EditValue.ToString()) != 0 || decimal.Parse(s_335_1.EditValue.ToString()) != 0 || decimal.Parse(s_335_2.EditValue.ToString()) != 0 || decimal.Parse(s_335_3.EditValue.ToString()) != 0)
                        */
            if (RSWdata.ExistPart_3_2.HasValue && RSWdata.ExistPart_3_2.Value == 1)
            {
                long id = tarCodesList.Any(x => x.Code == "03") ? tarCodesList.FirstOrDefault(x => x.Code == "03").ID : 0;

                if (RSW_2_1_List.Count() <= 0)
                {
                    errMessBox.Add(new ErrList { name = "Если раздел 3.2 заполнен, то в расчете должен быть подраздел 2.1 раздела 2 с кодом тарифа 03", control = "grid_2_1" });
                }
                else if (!RSW_2_1_List.Any(x => x.TariffCodeID == id))
                {
                    errMessBox.Add(new ErrList { name = "Если раздел 3.2 заполнен, то в расчете должен быть подраздел 2.1 раздела 2 с кодом тарифа 03", control = "grid_2_1" });
                }
            }

            /*            if (decimal.Parse(s_331_0.EditValue.ToString()) <= 0 && decimal.Parse(s_331_1.EditValue.ToString()) <= 0 && decimal.Parse(s_331_2.EditValue.ToString()) <= 0 && decimal.Parse(s_331_3.EditValue.ToString()) <= 0)
                            errMessBox.Add(new ErrList { name = "Стр.331 значение должно быть больше 0", control = "s_331_0" });

                        if (decimal.Parse(s_334_0.EditValue.ToString()) <= 0 && decimal.Parse(s_334_1.EditValue.ToString()) <= 0 && decimal.Parse(s_334_2.EditValue.ToString()) <= 0 && decimal.Parse(s_334_3.EditValue.ToString()) <= 0)
                            errMessBox.Add(new ErrList { name = "Стр.334 значение должно быть больше 0", control = "s_334_0" });
            */
            if (RSW_2_1_List.Count() > 0 && RSW_2_1_List.Any(x => db.TariffCode.Any(y => y.ID == x.TariffCodeID && y.Code == "03")))
            {
                FormsRSW2014_1_Razd_2_1 razd_2_1 = RSW_2_1_List.FirstOrDefault(x => x.TariffCodeID == db.TariffCode.FirstOrDefault(y => y.Code == "03").ID);
                if (decimal.Parse(s_334_0.EditValue.ToString()) > razd_2_1.s_200_0)
                    errMessBox.Add(new ErrList { name = "Стр.334 гр.3 Значение не может быть больше значения соответствующей графы в строке 200 подраздела 2.1 с тарифом 03", control = "s_334_0" });
                if (decimal.Parse(s_334_1.EditValue.ToString()) > razd_2_1.s_200_1)
                    errMessBox.Add(new ErrList { name = "Стр.334 гр.4 Значение не может быть больше значения соответствующей графы в строке 200 подраздела 2.1 с тарифом 03", control = "s_334_1" });
                if (decimal.Parse(s_334_2.EditValue.ToString()) > razd_2_1.s_200_2)
                    errMessBox.Add(new ErrList { name = "Стр.334 гр.5 Значение не может быть больше значения соответствующей графы в строке 200 подраздела 2.1 с тарифом 03", control = "s_334_2" });
                if (decimal.Parse(s_334_3.EditValue.ToString()) > razd_2_1.s_200_3)
                    errMessBox.Add(new ErrList { name = "Стр.334 гр.6 Значение не может быть больше значения соответствующей графы в строке 200 подраздела 2.1 с тарифом 03", control = "s_334_3" });
            }


            #endregion


            #region Раздел 3.3
            if (decimal.Parse(s_341_0.EditValue.ToString()) < decimal.Parse(s_342_0.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.341 должна быть больше или равна стр.342", control = "s_341_0" });
            if (decimal.Parse(s_341_1.EditValue.ToString()) < decimal.Parse(s_342_1.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.341 должна быть больше или равна стр.342", control = "s_341_1" });

            /*            if (decimal.Parse(s_342_0.EditValue.ToString()) <= 0)
                            errMessBox.Add(new ErrList { name = "Стр.342 значение показателя должно быть больше 0", control = "s_342_0" });
                        if (decimal.Parse(s_342_1.EditValue.ToString()) <= 0)
                            errMessBox.Add(new ErrList { name = "Стр.342 значение показателя должно быть больше 0", control = "s_342_1" });
                        */

            if (RSWdata.ExistPart_3_3.HasValue && RSWdata.ExistPart_3_3.Value == 1)
            {
                long id = tarCodesList.Any(x => x.Code == "06") ? tarCodesList.FirstOrDefault(x => x.Code == "06").ID : 0;

                if (!RSW_2_1_List.Any(x => x.TariffCodeID == id))
                {
                    errMessBox.Add(new ErrList { name = "Если раздел 3.3 заполнен, то в расчете должен быть подраздел 2.1 раздела 2 с кодом тарифа 06", control = "grid_2_1" });
                }
            }

            if (decimal.Parse(s_343_0.EditValue.ToString()) != 0)
                if (decimal.Parse(s_343_0.EditValue.ToString()) < 90)
                    errMessBox.Add(new ErrList { name = "Значение в строке 343 должно быть больше либо равно 90%", control = "s_343_0" });
            if (decimal.Parse(s_343_1.EditValue.ToString()) != 0)
                if (decimal.Parse(s_343_1.EditValue.ToString()) < 90)
                    errMessBox.Add(new ErrList { name = "Значение в строке 343 должно быть больше либо равно 90%", control = "s_343_1" });

            /*            if (long.Parse(s_344_0.EditValue.ToString()) < 7)
                            errMessBox.Add(new ErrList { name = "Значение в строке 344 должно быть больше либо равно 7", control = "s_344_0" });
                        if (long.Parse(s_344_1.EditValue.ToString()) < 7)
                            errMessBox.Add(new ErrList { name = "Значение в строке 344 должно быть больше либо равно 7", control = "s_344_1" });
                        */
            //			if (s_345_0.Text == "" && s_345_1.Text == "")
            //				errMessBox.Add(new ErrList { name = "Строка 345 должна быть заполнена", control = "s_345_0" });


            #endregion

            #region Раздел 3.4
            if (RSW_3_4_List.Count > 0)
            {
                long id = tarCodesList.Any(x => x.Code == "09") ? tarCodesList.FirstOrDefault(x => x.Code == "09").ID : 0;

                if (!RSW_2_1_List.Any(x => x.TariffCodeID == id))
                {
                    errMessBox.Add(new ErrList { name = "Если раздел 3.4 заполнен, то в расчете должен быть подраздел 2.1 раздела 2 с кодом тарифа 09", control = "grid_2_1" });
                }

                //                if (s_351_0.Text == "" && s_351_1.Text == "")
                //                    errMessBox.Add(new ErrList { name = "Строка 351 должна быть заполнена", control = "s_351_0" });
            }
            #endregion

            #region Раздел 3.5
            if (decimal.Parse(s_361_0.EditValue.ToString()) < decimal.Parse(s_362_0.EditValue.ToString()))
                errMessBox.Add(new ErrList { name = "Стр.361 должна быть больше или равна стр.362", control = "s_361_0" });

            //            if (decimal.Parse(s_361_0.EditValue.ToString()) != 0 || decimal.Parse(s_362_0.EditValue.ToString()) != 0 || decimal.Parse(s_321_2.EditValue.ToString()) != 0 || decimal.Parse(s_321_3.EditValue.ToString()) != 0)
            if (RSWdata.ExistPart_3_5.HasValue && RSWdata.ExistPart_3_5.Value == 1)
            {
                long id = tarCodesList.Any(x => x.Code == "07") ? tarCodesList.FirstOrDefault(x => x.Code == "07").ID : 0;

                if (!RSW_2_1_List.Any(x => x.TariffCodeID == id))
                {
                    errMessBox.Add(new ErrList { name = "Если раздел 3.5 заполнен, то в расчете должен быть подраздел 2.1 раздела 2 с кодом тарифа 07", control = "grid_2_1", type = "warning" });
                }
            }

            if (RSW_2_1_List.Any(x => db.TariffCode.Any(c => c.Code == "07" && c.ID == x.TariffCodeID)) && (!RSWdata.ExistPart_3_5.HasValue || (RSWdata.ExistPart_3_5.HasValue && RSWdata.ExistPart_3_5.Value == 0)))
            {
                errMessBox.Add(new ErrList { name = "При наличии в Разделе 2.1 сведений по коду категории  - 07, необходимо заполнить раздел 3." + (yearType < 2015 ? "5" : "2"), control = "s_361_0", type = "warning" });
            }

            #endregion

            #region Раздел 3.6
            if (decimal.Parse(s_371_0.EditValue.ToString()) < (decimal.Parse(s_372_0.EditValue.ToString()) + decimal.Parse(s_373_0.EditValue.ToString()) + decimal.Parse(s_374_0.EditValue.ToString())))
                errMessBox.Add(new ErrList { name = "Стр.371 должна быть больше или равна сумме стр.372,373,374", control = "s_371_0" });
            if (decimal.Parse(s_371_1.EditValue.ToString()) < (decimal.Parse(s_372_1.EditValue.ToString()) + decimal.Parse(s_373_1.EditValue.ToString()) + decimal.Parse(s_374_1.EditValue.ToString())))
                errMessBox.Add(new ErrList { name = "Стр.371 должна быть больше или равна сумме стр.372,373,374", control = "s_371_1" });

            /*            if (decimal.Parse(s_371_0.EditValue.ToString()) != 0 || decimal.Parse(s_371_1.EditValue.ToString()) != 0
                             || decimal.Parse(s_372_0.EditValue.ToString()) != 0 || decimal.Parse(s_372_1.EditValue.ToString()) != 0
                             || decimal.Parse(s_373_0.EditValue.ToString()) != 0 || decimal.Parse(s_373_1.EditValue.ToString()) != 0
                             || decimal.Parse(s_374_0.EditValue.ToString()) != 0 || decimal.Parse(s_374_1.EditValue.ToString()) != 0)
            */
            if (RSWdata.ExistPart_3_6.HasValue && RSWdata.ExistPart_3_6.Value == 1)
            {
                long id = tarCodesList.Any(x => x.Code == "12") ? tarCodesList.FirstOrDefault(x => x.Code == "12").ID : 0;

                if (!RSW_2_1_List.Any(x => x.TariffCodeID == id))
                {
                    errMessBox.Add(new ErrList { name = "Если раздел 3.6 заполнен, то в расчете должен быть подраздел 2.1 раздела 2 с кодом тарифа 12", control = "grid_2_1" });
                }
            }
            #endregion

            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        /// <summary>
        /// Обработка события когда меняется один из параметров уникального идентификатора 
        /// (год, период, номер корректировки, 'страхователь')
        /// </summary>
        private void changeUniqIdentifier()
        {
            //            RSWdata.InsurerID = Options.InsID; //нельза поменять из окна редактирования формы
            RSWdata.CorrectionNum = byte.Parse(CorrectionNum.Value.ToString());
            RSWdata.Quarter = Quarter.SelectedItem != null ? byte.Parse(Quarter.SelectedItem.Value.ToString()) : (byte)20;
            RSWdata.Year = Year.SelectedItem != null ? Int16.Parse(Year.Text) : (short)0;

            foreach (var item in RSW_2_1_List)
            {
                item.CorrectionNum = RSWdata.CorrectionNum;
                item.Quarter = RSWdata.Quarter;
                item.Year = RSWdata.Year;
            }

            foreach (var item in RSW_2_4_List)
            {
                item.CorrectionNum = RSWdata.CorrectionNum;
                item.Quarter = RSWdata.Quarter;
                item.Year = RSWdata.Year;
            }
            foreach (var item in RSW_3_4_List)
            {
                item.CorrectionNum = RSWdata.CorrectionNum;
                item.Quarter = RSWdata.Quarter;
                item.Year = RSWdata.Year;
            }
            foreach (var item in RSW_4_List)
            {
                item.CorrectionNum = RSWdata.CorrectionNum;
                item.Quarter = RSWdata.Quarter;
                item.Year = RSWdata.Year;
            }
            foreach (var item in RSW_5_List)
            {
                item.CorrectionNum = RSWdata.CorrectionNum;
                item.Quarter = RSWdata.Quarter;
                item.Year = RSWdata.Year;
            }



        }

        /// <summary>
        /// Сбор данных с основной экранной формы редактировния формы РСВ-1
        /// </summary>
        private void getValues()
        {
            var fields = typeof(FormsRSW2014_1_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                            string type = RSWdata.GetType().GetProperty(itemName).PropertyType.FullName;
                            if (type.Contains("["))
                                type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                            type = type.Split(',')[0].Split('.')[1].ToLower();

                            /*        DevExpress.XtraEditors.TextEdit box = new DevExpress.XtraEditors.TextEdit();
                                    Telerik.WinControls.UI.RadDateTimePicker box2 = new RadDateTimePicker();

                                    if (type != "datetime")
                                    {
                                        box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];
                                    }
                                    else
                                    {
                                        box2 = (RadDateTimePicker)this.Controls.Find(itemName, true)[0];
                                    }
        */

                            if (type != "datetime")
                            {
                                if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text != "")
                                {
                                    switch (type)
                                    {
                                        case "decimal":
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, Math.Round(decimal.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text), 2, MidpointRounding.AwayFromZero), null);
                                            break;
                                        case "integer":
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, int.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString()), null);
                                            break;
                                        case "int64":
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, long.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString()), null);
                                            break;
                                        //                                        case "datetime":
                                        //                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Value, null);
                                        //                                            break;
                                        case "string":
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text, null);
                                            break;
                                    }
                                }
                                else
                                    RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, null, null);
                            }
                            else
                            {
                                if (((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Value != ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).NullDate)
                                {
                                    RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, ((RadDateTimePicker)this.Controls.Find(itemName, true)[0]).Value, null);
                                }
                                else
                                    RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, null, null);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message + "    " + itemName);
                    }
                }

            }
        }
        /// <summary>
        /// Проверка на заполненность разделов
        /// </summary>
        private bool checkExists(int start, int end, byte cols)
        {
            bool result = false;
            for (int i = start; i <= end; i++)
            {
                if (i != 323 && i != 333 && i != 336 && i != 343 && i != 363 && i != 375)
                    if (!result)
                        for (byte j = 0; j < cols; j++)
                        {
                            string itemName = "s_" + i.ToString() + "_" + j.ToString();
                            if (this.Controls.Find(itemName, true).Any())
                            {
                                string type = RSWdata.GetType().GetProperty(itemName).PropertyType.FullName;
                                type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                                type = type.Split(',')[0].Split('.')[1].ToLower();

                                if (type != "datetime")
                                {
                                    //        DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];
                                    if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text != "")
                                    {
                                        switch (type)
                                        {
                                            case "decimal":
                                                if (decimal.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text) != 0)
                                                    result = true;
                                                break;
                                            case "integer":
                                                if (int.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString()) != 0)
                                                    result = true;
                                                break;
                                            case "int64":
                                                if (long.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString()) != 0)
                                                    result = true;
                                                break;
                                            case "string":
                                                if (!String.IsNullOrEmpty(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text))
                                                    result = true;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
            }


            return result;
        }


        /// <summary>
        /// Сохранение данных формы РСВ-1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton8_Click(object sender, EventArgs e)
        {

            if (validation())
            {
                savingForm();
            }
            else
            {
                if (preSavingWithErrors())
                {
                    savingForm();
                }



            }

        }

        private bool preSavingWithErrors()
        {
            bool result = true;
            if (errMessBox.Count != 0)
            {
                foreach (var item in errMessBox)
                {
                    DialogResult ds = Telerik.WinControls.RadMessageBox.Show(this, item.name + "\r\nСохранить \"как есть\"?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                    switch (ds)
                    {
                        case DialogResult.Yes:
                            break;
                        case DialogResult.No:
                            return false;
                        //  break;
                    }

                }
            }
            return result;
        }

        private void savingForm()
        {
            short y_;
            if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y_))
            {
                byte q_;
                if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q_))
                {
                    long insID_ = Options.InsID;
                    byte corrNum_ = byte.Parse(CorrectionNum.Value.ToString());
                    if (action == "add" && db.FormsRSW2014_1_1.Any(x => x.Year == y_ && x.Quarter == q_ && x.InsurerID == insID_ && x.CorrectionNum == corrNum_))
                    {
                        RadMessageBox.Show(this, "При сохранении записи произошло дублирование по первичному ключу. Необходимо выбрать другой Отчетный период или номер корректировки!", "Внимание! Дублирование по ключу");
                        return;
                    }
                    if (action == "edit" && db.FormsRSW2014_1_1.Any(x => x.Year == y_ && x.Quarter == q_ && x.InsurerID == insID_ && x.CorrectionNum == corrNum_ && x.ID != RSWdata.ID))
                    {
                        RadMessageBox.Show(this, "При сохранении записи произошло дублирование по первичному ключу. Необходимо выбрать другой Отчетный период или номер корректировки!", "Внимание! Дублирование по ключу");
                        return;
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
                RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы РСВ-1. Код ошибки: " + ex.Message);
            }

            if (flag_ok)
                try
                {
                    flag_ok = false;

                    RSWdata.InsurerID = Options.InsID;
                    RSWdata.CorrectionNum = byte.Parse(CorrectionNum.Value.ToString());
                    if (RSWdata.CorrectionNum == 0)
                        RSWdata.CorrectionType = null;
                    else if (CorrectionType.SelectedItem != null)
                        RSWdata.CorrectionType = byte.Parse(CorrectionType.SelectedItem.Tag.ToString());
                    RSWdata.AutoCalc = AutoCalcSwitch.IsOn;

                    if (yearType == 2015)
                    {
                        if (reducedRateDDL.SelectedItem == null)
                        {
                            RSWdata.ReducedRate = null;
                        }
                        else
                        {
                            RSWdata.ReducedRate = reducedRateDDL.SelectedItem.Tag.ToString();
                        }
                    }

                    RSWdata.Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString());
                    RSWdata.Year = Int16.Parse(Year.Text);
                    RSWdata.WorkStop = WorkStop.Checked ? (byte)1 : (byte)0;
                    RSWdata.CountEmployers = int.Parse(CountEmployers.Value.ToString());
                    RSWdata.CountAverageEmployers = int.Parse(CountAverageEmployers.Value.ToString());
                    if (int.Parse(CountConfirmDoc.Value.ToString()) <= 255)
                        RSWdata.CountConfirmDoc = byte.Parse(CountConfirmDoc.Value.ToString());
                    else
                        RSWdata.CountConfirmDoc = (byte)255;
                    RSWdata.DateUnderwrite = DateUnderwrite.Value.Date;
                    RSWdata.ConfirmType = byte.Parse(ConfirmType.SelectedItem.Tag.ToString());
                    RSWdata.ConfirmFirstName = ConfirmFirstName.Text;
                    RSWdata.ConfirmLastName = ConfirmLastName.Text;
                    RSWdata.ConfirmMiddleName = ConfirmMiddleName.Text;
                    RSWdata.ConfirmOrgName = ConfirmOrgName.Text;
                    if (ConfirmDocType.Text == "")
                        RSWdata.ConfirmDocType_ID = null;
                    else
                        RSWdata.ConfirmDocType_ID = long.Parse(ConfirmDocType.SelectedItem.Value.ToString());
                    RSWdata.ConfirmDocName = ConfirmDocName.Text;
                    RSWdata.ConfirmDocSerLat = ConfirmDocSerLat.Text;
                    RSWdata.ConfirmDocSerRus = ConfirmDocSerRus.Text;
                    if (ConfirmDocNum.Text == "")
                        RSWdata.ConfirmDocNum = null;
                    else
                        RSWdata.ConfirmDocNum = int.Parse(ConfirmDocNum.Text);
                    if (ConfirmDocDate.Text == "")
                        RSWdata.ConfirmDocDate = null;
                    else
                        RSWdata.ConfirmDocDate = ConfirmDocDate.Value.Date;
                    RSWdata.ConfirmDocKemVyd = ConfirmDocKemVyd.Text;


                    // проверка раздела 6 на соответствие уникальным параметрам, год, период, страхователь, номер корректировки
                    List<long> list_id = RSW_6_1_List.Select(x => x.ID).ToList();
                    var RSW_6_1_List_from_db = db.FormsRSW2014_1_Razd_6_1.Where(x => list_id.Contains(x.ID));

                    foreach (var item in RSW_6_1_List_from_db)
                    {
                        bool flag_edited = false;

                        // проверка текущих записей Раздела 6.1 на факт их редактирования (отличия от имеющихся в БД)

                        if (item.CorrectionNum != RSWdata.CorrectionNum || item.Year != RSWdata.Year || item.Quarter != RSWdata.Quarter || item.InsurerID != RSWdata.InsurerID)
                        {
                            flag_edited = true;
                        }

                        if (flag_edited) // если записи отличаются
                        {
                            item.CorrectionNum = RSWdata.CorrectionNum;
                            item.Year = RSWdata.Year;
                            item.Quarter = RSWdata.Quarter;
                            item.InsurerID = RSWdata.InsurerID;

                            db.Entry(item).State = EntityState.Modified;
                        }


                    }

                    flag_ok = true;



                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка во время сохранения данных с главного окна редактирования формы РСВ-1. Код ошибки: " + ex.Message);
                }


            if (flag_ok)
                switch (action)
                {
                    //новая записи формы РСВ-1
                    case "add":
                        try
                        {
                            flag_ok = false;
                            #region Сохранение новой записи

                            db.FormsRSW2014_1_1.Add(RSWdata);
                            foreach (var item in RSW_2_1_List)
                            {
                                /*          item.CorrectionNum = RSWdata.CorrectionNum;
                                          item.Year = RSWdata.Year;
                                          item.Quarter = RSWdata.Quarter;
                                          item.InsurerID = RSWdata.InsurerID;
                                          */
                                FormsRSW2014_1_Razd_2_1 r = new FormsRSW2014_1_Razd_2_1();

                                var fields = typeof(FormsRSW2014_1_Razd_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                                db.FormsRSW2014_1_Razd_2_1.Add(r);
                            }

                            foreach (var item in RSW_2_4_List)
                            {
                                FormsRSW2014_1_Razd_2_4 r = new FormsRSW2014_1_Razd_2_4();

                                var fields = typeof(FormsRSW2014_1_Razd_2_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                                db.FormsRSW2014_1_Razd_2_4.Add(r);
                            }

                            foreach (var item in RSW_3_4_List)
                            {
                                FormsRSW2014_1_Razd_3_4 r = new FormsRSW2014_1_Razd_3_4();

                                var fields = typeof(FormsRSW2014_1_Razd_3_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                                db.FormsRSW2014_1_Razd_3_4.Add(r);
                            }

                            foreach (var item in RSW_4_List)
                            {
                                FormsRSW2014_1_Razd_4 r = new FormsRSW2014_1_Razd_4();

                                var fields = typeof(FormsRSW2014_1_Razd_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                                db.FormsRSW2014_1_Razd_4.Add(r);
                            }

                            foreach (var item in RSW_5_List)
                            {
                                FormsRSW2014_1_Razd_5 r = new FormsRSW2014_1_Razd_5();

                                var fields = typeof(FormsRSW2014_1_Razd_5).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                                db.FormsRSW2014_1_Razd_5.Add(r);
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
                    //изменение записи формы РСВ-1
                    case "edit":
                        // выбираем из базы исходную запись по идешнику
                        FormsRSW2014_1_1 r1 = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == RSWdata.ID);
                        try
                        {
                            var fields = typeof(FormsRSW2014_1_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            foreach (var itemName_ in names)
                            {
                                string itemName = itemName_.TrimStart('_');
                                var properties = RSWdata.GetType().GetProperty(itemName);
                                if (properties != null)
                                {
                                    object value = properties.GetValue(RSWdata, null);
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
                            flag_ok = false;

                            #region обрабатываем записи о выплатах из Раздела 2.1
                            try
                            {
                                var RSW_2_1_List_from_db = db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_2_1_List.Select(x => x.ID);
                                var list_for_del = RSW_2_1_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_1_Razd_2_1.Remove(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_2_1_List_from_db = db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 2.1 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_1_Razd_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_2_1_List)
                                {
                                    item.CorrectionNum = RSWdata.CorrectionNum;
                                    item.Year = RSWdata.Year;
                                    item.Quarter = RSWdata.Quarter;
                                    item.InsurerID = RSWdata.InsurerID;

                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_2_1_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_1_Razd_2_1 rsw_temp = RSW_2_1_List_from_db.Single(x => x.ID == item.ID);

                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
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


                                        if (flag_edited) // если записи отличаются
                                        {

                                            db.Entry(rsw_temp).State = EntityState.Modified;

                                        }

                                    }
                                    if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        FormsRSW2014_1_Razd_2_1 r = new FormsRSW2014_1_Razd_2_1();

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

                                        db.FormsRSW2014_1_Razd_2_1.Add(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Раздела 2.1 произошла ошибка. Код ошибки: " + ex.Message);

                            }
                            #endregion

                            #region обрабатываем записи о выплатах из Раздела 2.4
                            try
                            {
                                var RSW_2_4_List_from_db = db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_2_4_List.Select(x => x.ID);
                                var list_for_del = RSW_2_4_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_1_Razd_2_4.Remove(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_2_4_List_from_db = db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 2.4 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_1_Razd_2_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_2_4_List)
                                {
                                    item.CorrectionNum = RSWdata.CorrectionNum;
                                    item.Year = RSWdata.Year;
                                    item.Quarter = RSWdata.Quarter;
                                    item.InsurerID = RSWdata.InsurerID;

                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_2_4_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_1_Razd_2_4 rsw_temp = RSW_2_4_List_from_db.Single(x => x.ID == item.ID);


                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
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


                                        if (flag_edited) // если записи отличаются
                                        {

                                            db.Entry(rsw_temp).State =EntityState.Modified;

                                        }

                                    }
                                    if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        FormsRSW2014_1_Razd_2_4 r = new FormsRSW2014_1_Razd_2_4();

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

                                        db.FormsRSW2014_1_Razd_2_4.Add(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Раздела 2.4 произошла ошибка. Код ошибки: " + ex.Message);

                            }
                            #endregion

                            #region обрабатываем записи о выплатах из Раздела 2.5.1 при смене номера корректировки
                            if (RSWdata.CorrectionNum != corrNumOld)
                            {
                                try
                                {
                                    var RSW_2_5_1_List_from_db = db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld);

                                    foreach (var item in RSW_2_5_1_List_from_db)
                                    {
                                        item.CorrectionNum = RSWdata.CorrectionNum;
                                        db.Entry(item).State = EntityState.Modified;
                                    }

                                    flag_ok = true;
                                }
                                catch (Exception ex)
                                {
                                    RadMessageBox.Show("При сохранении данных Раздела 2.5.1 произошла ошибка. Код ошибки: " + ex.Message);
                                }
                            }
                            #endregion

                            #region обрабатываем записи о выплатах из Раздела 2.5.2 при смене номера корректировки
                            if (RSWdata.CorrectionNum != corrNumOld)
                            {
                                try
                                {
                                    var RSW_2_5_2_List_from_db = db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld);

                                    foreach (var item in RSW_2_5_2_List_from_db)
                                    {
                                        item.CorrectionNum = RSWdata.CorrectionNum;
                                        db.Entry(item).State  = EntityState.Modified;
                                    }

                                    flag_ok = true;
                                }
                                catch (Exception ex)
                                {
                                    RadMessageBox.Show("При сохранении данных Раздела 2.5.2 произошла ошибка. Код ошибки: " + ex.Message);
                                }
                            }
                            #endregion

                            #region обрабатываем записи о выплатах из Раздела 3.4
                            try
                            {
                                var RSW_3_4_List_from_db = db.FormsRSW2014_1_Razd_3_4.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_3_4_List.Select(x => x.ID);
                                var list_for_del = RSW_3_4_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_1_Razd_3_4.Remove(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_3_4_List_from_db = db.FormsRSW2014_1_Razd_3_4.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 3.4 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_1_Razd_3_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_3_4_List)
                                {
                                    item.CorrectionNum = RSWdata.CorrectionNum;
                                    item.Year = RSWdata.Year;
                                    item.Quarter = RSWdata.Quarter;
                                    item.InsurerID = RSWdata.InsurerID;

                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_3_4_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_1_Razd_3_4 rsw_temp = RSW_3_4_List_from_db.Single(x => x.ID == item.ID);


                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
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


                                        if (flag_edited) // если записи отличаются
                                        {

                                            db.Entry(rsw_temp).State = EntityState.Modified;

                                        }

                                    }
                                    if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        FormsRSW2014_1_Razd_3_4 r = new FormsRSW2014_1_Razd_3_4();

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

                                        db.FormsRSW2014_1_Razd_3_4.Add(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Раздела 3.4 произошла ошибка. Код ошибки: " + ex.Message);

                            }
                            #endregion

                            #region обрабатываем записи о выплатах из Раздела 4
                            try
                            {
                                var RSW_4_List_from_db = db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_4_List.Select(x => x.ID);
                                var list_for_del = RSW_4_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_1_Razd_4.Remove(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_4_List_from_db = db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 4 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_1_Razd_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_4_List)
                                {
                                    item.CorrectionNum = RSWdata.CorrectionNum;
                                    item.Year = RSWdata.Year;
                                    item.Quarter = RSWdata.Quarter;
                                    item.InsurerID = RSWdata.InsurerID;

                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_4_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_1_Razd_4 rsw_temp = RSW_4_List_from_db.Single(x => x.ID == item.ID);


                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
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


                                        if (flag_edited) // если записи отличаются
                                        {

                                            db.Entry(rsw_temp).State = EntityState.Modified;

                                        }

                                    }
                                    if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        FormsRSW2014_1_Razd_4 r = new FormsRSW2014_1_Razd_4();

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

                                        db.FormsRSW2014_1_Razd_4.Add(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Раздела 4 произошла ошибка. Код ошибки: " + ex.Message);

                            }
                            #endregion

                            #region обрабатываем записи о выплатах из Раздела 5
                            try
                            {

                                var RSW_5_List_from_db = db.FormsRSW2014_1_Razd_5.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_5_List.Select(x => x.ID);
                                var list_for_del = RSW_5_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_1_Razd_5.Remove(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_5_List_from_db = db.FormsRSW2014_1_Razd_5.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == corrNumOld && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 5 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_1_Razd_5).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_5_List)
                                {
                                    item.CorrectionNum = RSWdata.CorrectionNum;
                                    item.Year = RSWdata.Year;
                                    item.Quarter = RSWdata.Quarter;
                                    item.InsurerID = RSWdata.InsurerID;

                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_5_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_1_Razd_5 rsw_temp = RSW_5_List_from_db.Single(x => x.ID == item.ID);


                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
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


                                        if (flag_edited) // если записи отличаются
                                        {

                                            db.Entry(rsw_temp).State = EntityState.Modified;

                                        }

                                    }
                                    if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        FormsRSW2014_1_Razd_5 r = new FormsRSW2014_1_Razd_5();

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

                                        db.FormsRSW2014_1_Razd_5.Add(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Раздела 5 произошла ошибка. Код ошибки: " + ex.Message);

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


        #endregion


        private void ConfirmType_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            /// Подтверждающее лицо
            if (ConfirmType.SelectedIndex > 0)
            {
                ConfirmDocTypeDDL_SelectedIndexChanged();

                ConfirmOrgName.Enabled = true;
                ConfirmDocType.Enabled = true;
                ConfirmDocTypeBtn.Enabled = true;
                ConfirmDocSerRus.Enabled = true;
                ConfirmDocSerLat.Enabled = true;
                ConfirmDocNum.Enabled = true;
                ConfirmDocKemVyd.Enabled = true;
                ConfirmDocDate.Enabled = true;
                ConfirmDocDateMaskedEditBox.Enabled = true;
            }
            else
            {
                ConfirmOrgName.Enabled = false;
                ConfirmDocType.Enabled = false;
                ConfirmDocTypeBtn.Enabled = false;
                ConfirmDocSerRus.Enabled = false;
                ConfirmDocSerLat.Enabled = false;
                ConfirmDocNum.Enabled = false;
                ConfirmDocKemVyd.Enabled = false;
                ConfirmDocDate.Enabled = false;
                ConfirmDocDateMaskedEditBox.Enabled = false;

            }
            //          ConfirmDocTypeDDL_SelectedIndexChanged();
        }


        #region Тип документа

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
        #endregion



        #region Раздел 2.1

        /// <summary>
        /// обновление таблицы раздела 2.1
        /// </summary>
        private void gridUpdate_2_1()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            grid_2_1.Rows.Clear();

            if (RSW_2_1_List.Count() != 0)
            {
                foreach (var item in RSW_2_1_List)
                {
                    TariffCode tc = db.TariffCode.FirstOrDefault(x => x.ID == item.TariffCodeID);
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.grid_2_1.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["TariffCode"].Value = tc.Code != null ? tc.Code : "";
                    rowInfo.Cells["OPSLessBase"].Value = item.s_205_0.HasValue ? item.s_205_0.Value : 0;
                    rowInfo.Cells["OPSMoreBase"].Value = item.s_206_0.HasValue ? item.s_206_0.Value : 0;
                    rowInfo.Cells["OMS"].Value = yearType != 2015 ? (item.s_215_0.HasValue ? item.s_215_0.Value : 0) : (item.s_214_0.HasValue ? item.s_214_0.Value : 0);
                    grid_2_1.Rows.Add(rowInfo);
                }
            }

            this.grid_2_1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            grid_2_1.Refresh();

        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_2_1_Click(object sender, EventArgs e)
        {
            RSW2014_2_1_Edit child = new RSW2014_2_1_Edit();
            //       child.db = db;
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.yearType = yearType == 2012 ? (short)2014 : yearType;
            child.changeFormByYear();
            child.formData = new FormsRSW2014_1_Razd_2_1()
            {
                InsurerID = Options.InsID,
                CorrectionNum = byte.Parse(CorrectionNum.Value.ToString()),
                Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString()),
                Year = short.Parse(Year.SelectedItem.Value.ToString()),
                AutoCalc = false
            };
            child.ShowDialog();
            if (child.formData != null)
            {
                RSW_2_1_List.Add(child.formData);
                gridUpdate_2_1();
                calcFields_Razd_2_1(null, null);

            }
        }


        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editBtn_2_1_Click(object sender, EventArgs e)
        {
            if (grid_2_1.RowCount != 0)
            {
                int rowindex = grid_2_1.CurrentRow.Index;
                //long id = long.Parse(radGridView3.Rows[rowindex].Cells[0].Value.ToString());

                if (rowindex >= 0)
                {
                    RSW2014_2_1_Edit child = new RSW2014_2_1_Edit();
                    //           child.db = db;
                    child.Owner = this;
                    child.ThemeName = this.ThemeName;
                    child.ShowInTaskbar = false;
                    child.action = "edit";
                    child.yearType = yearType == 2012 ? (short)2014 : yearType;
                    child.changeFormByYear();
                    child.formData = RSW_2_1_List.Skip(rowindex).Take(1).First();

                    /*			FormsRSW2014_1_Razd_2_1 formDataPrev = new FormsRSW2014_1_Razd_2_1();
                                byte q = child.formData.Quarter;
                                if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                                {
                                    short year = child.formData.Year;
                                    byte quarter = 20;
                                    if (q == 6)
                                        quarter = 3;
                                    else if (q == 9)
                                        quarter = 6;
                                    else if (q == 0)
                                        quarter = 9;

                                    if (db.FormsRSW2014_1_Razd_2_1.Any(x => x.Year == year && x.Quarter == quarter && x.TariffCodeID == child.formData.TariffCodeID && x.InsurerID == Options.InsID))
                                        formDataPrev = db.FormsRSW2014_1_Razd_2_1.Where(x => x.Year == year && x.Quarter == quarter && x.TariffCodeID == child.formData.TariffCodeID && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                                }
                                else
                                {
                                    child.formDataPrev = null;
                                }

                                child.formDataPrev = formDataPrev;
                                */
                    child.ShowDialog();

                    if (child.formData != null)
                    {
                        //  var rsw_ind = RSW_6_6_List.FindIndex(x => x.ID == child.formData.ID);
                        RSW_2_1_List.RemoveAt(rowindex);
                        RSW_2_1_List.Insert(rowindex, child.formData);

                        gridUpdate_2_1();
                        calcFields_Razd_2_1(null, null);
                    }
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }




        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delBtn_2_1_Click(object sender, EventArgs e)
        {
            if (grid_2_1.RowCount != 0)
            {
                int rowindex = grid_2_1.CurrentRow.Index;
                if (rowindex >= 0 && grid_2_1.CurrentRow.Cells[0].Value != null)
                {
                    DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //long id = long.Parse(grid_2_1.Rows[rowindex].Cells[0].Value.ToString());

                        //FormsRSW2014_1_Razd_2_1 rsw = RSW_2_1_List.First(x => x.ID == id);
                        //RSW_2_1_List.Remove(rsw);

                        RSW_2_1_List.RemoveAt(rowindex);
                        calcFields_Razd_2_1(null, null);

                        gridUpdate_2_1();

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }


        private void grid_2_1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_2_1_Click(null, null);
        }

        #endregion

        #region Раздел 2.4

        /// <summary>
        /// обновление таблицы раздела 2.4
        /// </summary>
        private void gridUpdate_2_4()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            grid_2_4.Rows.Clear();

            if (RSW_2_4_List.Count() != 0)
            {
                foreach (var item in RSW_2_4_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.grid_2_4.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["CodeBase"].Value = item.CodeBase;
                    rowInfo.Cells["FilledBase"].Value = item.FilledBase.HasValue ? Options.FilledBaseArr.FirstOrDefault(x => x.id == item.FilledBase.Value.ToString()).name : "";
                    rowInfo.Cells["O4"].Value = item.s_244_0.HasValue ? item.s_244_0.Value : 0;
                    rowInfo.Cells["B3.4"].Value = item.s_250_0.HasValue ? item.s_250_0.Value : 0;
                    rowInfo.Cells["B3.3"].Value = item.s_256_0.HasValue ? item.s_256_0.Value : 0;
                    rowInfo.Cells["B3.2"].Value = item.s_262_0.HasValue ? item.s_262_0.Value : 0;
                    rowInfo.Cells["B3.1"].Value = item.s_268_0.HasValue ? item.s_268_0.Value : 0;
                    grid_2_4.Rows.Add(rowInfo);
                }
            }

            this.grid_2_4.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            grid_2_4.Refresh();
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_2_4_Click(object sender, EventArgs e)
        {
            RSW2014_2_4_Edit child = new RSW2014_2_4_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.formData = new FormsRSW2014_1_Razd_2_4()
            {
                InsurerID = Options.InsID,
                CorrectionNum = byte.Parse(CorrectionNum.Value.ToString()),
                Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString()),
                Year = short.Parse(Year.SelectedItem.Value.ToString()),
                AutoCalc = false
            };
            child.ShowDialog();
            if (child.formData != null)
            {
                RSW_2_4_List.Add(child.formData);
                calcFields_Razd_2_4(null, null);
                gridUpdate_2_4();
            }
        }


        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editBtn_2_4_Click(object sender, EventArgs e)
        {
            if (grid_2_4.RowCount != 0)
            {
                int rowindex = grid_2_4.CurrentRow.Index;

                RSW2014_2_4_Edit child = new RSW2014_2_4_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.formData = RSW_2_4_List.Skip(rowindex).Take(1).First();

                FormsRSW2014_1_Razd_2_4 formDataPrev = new FormsRSW2014_1_Razd_2_4();
                byte q = child.formData.Quarter;
                if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                {
                    short year = child.formData.Year;
                    byte quarter = 20;
                    if (q == 6)
                        quarter = 3;
                    else if (q == 9)
                        quarter = 6;
                    else if (q == 0)
                        quarter = 9;

                    if (db.FormsRSW2014_1_Razd_2_4.Any(x => x.Year == year && x.Quarter == quarter && x.CodeBase == child.formData.CodeBase && x.FilledBase == child.formData.FilledBase))
                        formDataPrev = db.FormsRSW2014_1_Razd_2_4.Where(x => x.Year == year && x.Quarter == quarter && x.CodeBase == child.formData.CodeBase && x.FilledBase == child.formData.FilledBase).OrderByDescending(x => x.CorrectionNum).First();
                }
                else
                {
                    child.formDataPrev = null;
                }

                child.formDataPrev = formDataPrev;

                child.ShowDialog();

                if (child.formData != null)
                {
                    //  var rsw_ind = RSW_6_6_List.FindIndex(x => x.ID == child.formData.ID);
                    RSW_2_4_List.RemoveAt(rowindex);
                    RSW_2_4_List.Insert(rowindex, child.formData);
                    calcFields_Razd_2_4(null, null);
                    gridUpdate_2_4();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }




        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delBtn_2_4_Click(object sender, EventArgs e)
        {
            if (grid_2_4.RowCount != 0 && grid_2_4.CurrentRow.Cells[0].Value != null)
            {
                int rowindex = grid_2_4.CurrentRow.Index;
                RSW_2_4_List.RemoveAt(rowindex);
                calcFields_Razd_2_4(null, null);

                gridUpdate_2_4();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void grid_2_4_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_2_4_Click(null, null);
        }

        #endregion

        #region Раздел 3.4

        /// <summary>
        /// обновление таблицы раздела 3.4
        /// </summary>
        private void gridUpdate_3_4()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            grid_3_4.Rows.Clear();

            if (RSW_3_4_List.Count() != 0)
            {
                foreach (var item in RSW_3_4_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.grid_3_4.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["NumOrd"].Value = item.NumOrd;
                    rowInfo.Cells["OKWED"].Value = item.OKWED;
                    rowInfo.Cells["NameOKWED"].Value = item.NameOKWED;
                    rowInfo.Cells["Income"].Value = item.Income.HasValue ? item.Income.Value : 0;
                    rowInfo.Cells["RateIncome"].Value = item.RateIncome.HasValue ? item.RateIncome.Value : 0;
                    grid_3_4.Rows.Add(rowInfo);
                }
            }

            this.grid_3_4.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            grid_3_4.Refresh();
            updateTextBoxes_Razd_3_4();
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_3_4_Click(object sender, EventArgs e)
        {
            RSW2014_3_4_Edit child = new RSW2014_3_4_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.formData = new FormsRSW2014_1_Razd_3_4()
            {
                InsurerID = Options.InsID,
                CorrectionNum = byte.Parse(CorrectionNum.Value.ToString()),
                Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString()),
                Year = short.Parse(Year.SelectedItem.Value.ToString()),
                NumOrd = RSW_3_4_List.Any() ? RSW_3_4_List.Select(x => x.NumOrd).Max().Value + 1 : 1
            };
            child.ShowDialog();
            if (child.formData != null)
            {
                RSW_3_4_List.Add(child.formData);
                gridUpdate_3_4();
            }

        }



        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editBtn_3_4_Click(object sender, EventArgs e)
        {
            if (grid_3_4.RowCount != 0)
            {
                int rowindex = grid_3_4.CurrentRow.Index;
                //long id = long.Parse(radGridView3.Rows[rowindex].Cells[0].Value.ToString());


                RSW2014_3_4_Edit child = new RSW2014_3_4_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.formData = RSW_3_4_List.Skip(rowindex).Take(1).First();
                child.ShowDialog();

                if (child.formData != null)
                {
                    //  var rsw_ind = RSW_6_6_List.FindIndex(x => x.ID == child.formData.ID);
                    RSW_3_4_List.RemoveAt(rowindex);
                    RSW_3_4_List.Insert(rowindex, child.formData);

                    gridUpdate_3_4();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }




        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delBtn_3_4_Click(object sender, EventArgs e)
        {
            if (grid_3_4.RowCount != 0 && grid_3_4.CurrentRow.Cells[0].Value != null)
            {
                int rowindex = grid_3_4.CurrentRow.Index;
                RSW_3_4_List.RemoveAt(rowindex);

                gridUpdate_3_4();

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        /// <summary>
        /// Функция обновления значений в полях быстрого просмотра под таблицей Раздела 2.1
        /// </summary>
        /// <param name="rsw"></param>
        private void updateTextBoxes_Razd_3_4()
        {
            if (RSW_3_4_List.Count != 0)
            {
                decimal[] sum = new decimal[2] { 0, 0 };

                /*                foreach (var item in RSW_3_4_List)
                                {
                                    sum[0] = sum[0] + item.Income.Value;
                                    sum[1] = sum[1] + item.RateIncome.Value;
                                }
                                */

                sum[0] = Math.Round(RSW_3_4_List.Sum(x => x.Income.Value), 2, MidpointRounding.AwayFromZero);
                sum[1] = Math.Round(RSW_3_4_List.Sum(x => x.RateIncome.Value), 5, MidpointRounding.AwayFromZero);


                IncomeLabel_3_4.Text = Utils.decToStr(sum[0]);
                RateIncomeLabel_3_4.Text = sum[1].ToString();

            }
            else
            {
                IncomeLabel_3_4.Text = "0.00";
                RateIncomeLabel_3_4.Text = "0.00000";
            }


        }

        private void grid_3_4_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_3_4_Click(null, null);
        }
        #endregion

        #region Раздел 4

        /// <summary>
        /// обновление таблицы раздела 4
        /// </summary>
        private void gridUpdate_4()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            grid_4.Rows.Clear();

            if (RSW_4_List.Count() != 0)
            {
                foreach (var item in RSW_4_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.grid_4.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["NumOrd"].Value = item.NumOrd;
                    rowInfo.Cells["YearPer"].Value = item.YearPer;
                    rowInfo.Cells["MonthPer"].Value = item.MonthPer;
                    rowInfo.Cells["Strah2014"].Value = item.Strah2014.HasValue ? item.Strah2014.Value : 0;
                    rowInfo.Cells["Strah2013"].Value = item.Strah2013.HasValue ? item.Strah2013.Value : 0;
                    rowInfo.Cells["Nakop2013"].Value = item.Nakop2013.HasValue ? item.Nakop2013.Value : 0;
                    rowInfo.Cells["Dop1"].Value = item.Dop1.HasValue ? item.Dop1.Value : 0;
                    rowInfo.Cells["Dop2"].Value = item.Dop2.HasValue ? item.Dop2.Value : 0;
                    rowInfo.Cells["Dop21"].Value = item.Dop21.HasValue ? item.Dop21.Value : 0;
                    rowInfo.Cells["OMS"].Value = item.OMS.HasValue ? item.OMS.Value : 0;
                    grid_4.Rows.Add(rowInfo);
                }
            }

            this.grid_4.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            grid_4.Refresh();
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_4_Click(object sender, EventArgs e)
        {
            RSW2014_4_Edit child = new RSW2014_4_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.yearType = yearType;
            child.formData = new FormsRSW2014_1_Razd_4()
            {
                InsurerID = Options.InsID,
                CorrectionNum = byte.Parse(CorrectionNum.Value.ToString()),
                Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString()),
                Year = short.Parse(Year.SelectedItem.Value.ToString()),
                NumOrd = RSW_4_List.Any() ? RSW_4_List.Select(x => x.NumOrd).Max().Value + 1 : 1
            };
            child.ShowDialog();
            if (child.formData != null)
            {
                RSW_4_List.Add(child.formData);
                calcFields_Razd_4(null, null);
                gridUpdate_4();
            }
        }



        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editBtn_4_Click(object sender, EventArgs e)
        {
            if (grid_4.RowCount != 0)
            {
                int rowindex = grid_4.CurrentRow.Index;

                RSW2014_4_Edit child = new RSW2014_4_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.yearType = yearType;
                child.formData = RSW_4_List.Skip(rowindex).Take(1).First();
                child.ShowDialog();

                if (child.formData != null)
                {
                    //  var rsw_ind = RSW_6_6_List.FindIndex(x => x.ID == child.formData.ID);
                    RSW_4_List.RemoveAt(rowindex);
                    RSW_4_List.Insert(rowindex, child.formData);
                    calcFields_Razd_4(null, null);
                    gridUpdate_4();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }



        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delBtn_4_Click(object sender, EventArgs e)
        {
            if (grid_4.RowCount != 0 && grid_4.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    int rowindex = grid_4.CurrentRow.Index;
                    //long id = long.Parse(grid_4.Rows[rowindex].Cells[0].Value.ToString());

                    //FormsRSW2014_1_Razd_4 rsw = RSW_4_List.First(x => x.ID == id);
                    //RSW_4_List.Remove(rsw);
                    RSW_4_List.RemoveAt(rowindex);
                    calcFields_Razd_4(null, null);
                    gridUpdate_4();
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void grid_4_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_4_Click(null, null);
        }

        #endregion

        #region Раздел 5

        /// <summary>
        /// обновление таблицы раздела 5
        /// </summary>
        private void gridUpdate_5()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            grid_5.Rows.Clear();

            if (RSW_5_List.Count() != 0)
            {
                foreach (var item in RSW_5_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.grid_5.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["NumOrd"].Value = item.NumOrd;
                    rowInfo.Cells["FIO"].Value = item.LastName + " " + item.FirstName + " " + item.MiddleName;
                    rowInfo.Cells["Sum"].Value = item.SumPay.HasValue ? item.SumPay.Value : 0;
                    rowInfo.Cells["Sum_1"].Value = item.SumPay_0.HasValue ? item.SumPay_0.Value : 0;
                    rowInfo.Cells["Sum_2"].Value = item.SumPay_1.HasValue ? item.SumPay_1.Value : 0;
                    rowInfo.Cells["Sum_3"].Value = item.SumPay_2.HasValue ? item.SumPay_2.Value : 0;
                    grid_5.Rows.Add(rowInfo);
                }
            }

            this.grid_5.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            grid_5.Refresh();
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_5_Click(object sender, EventArgs e)
        {
            RSW2014_5_Edit child = new RSW2014_5_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.formData = new FormsRSW2014_1_Razd_5()
            {
                InsurerID = Options.InsID,
                CorrectionNum = byte.Parse(CorrectionNum.Value.ToString()),
                Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString()),
                Year = short.Parse(Year.SelectedItem.Value.ToString()),
                NumOrd = RSW_5_List.Any() ? RSW_5_List.Select(x => x.NumOrd).Max().Value + 1 : 1
            };

            child.ShowDialog();
            if (child.formData != null)
            {
                RSW_5_List.Add(child.formData);
                gridUpdate_5();
            }
        }


        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editBtn_5_Click(object sender, EventArgs e)
        {
            if (grid_5.RowCount != 0)
            {
                int rowindex = grid_5.CurrentRow.Index;

                RSW2014_5_Edit child = new RSW2014_5_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.formData = RSW_5_List.Skip(rowindex).Take(1).First();
                child.ShowDialog();

                if (child.formData != null)
                {
                    //  var rsw_ind = RSW_6_6_List.FindIndex(x => x.ID == child.formData.ID);
                    RSW_5_List.RemoveAt(rowindex);
                    RSW_5_List.Insert(rowindex, child.formData);

                    gridUpdate_5();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }



        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delBtn_5_Click(object sender, EventArgs e)
        {
            if (grid_5.RowCount != 0 && grid_5.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    int rowindex = grid_5.CurrentRow.Index;

                    RSW_5_List.RemoveAt(rowindex);

                    gridUpdate_5();
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void grid_5_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_5_Click(null, null);
        }
        #endregion

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_1 WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }

                    gridUpdate_6_1(null, null);
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }


        #region Обновление полей при изменении значений
        private void calcFields_100(object sender, EventArgs e)
        {
            if (Options.inputTypeRSW1 != 2)
            {
                short y = 0;
                if (short.TryParse(Year.SelectedItem.Text, out y))
                {
                    byte q;
                    if (y >= 2015 && Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                    {
                        if (q == 3) // Если первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                        {
                            if (db.FormsRSW2014_1_1.Any(x => x.Year == (y - 1) && x.Quarter == 0 && x.InsurerID == Options.InsID))
                            {
                                var prev_d = db.FormsRSW2014_1_1.Where(x => x.Year == (y - 1) && x.Quarter == 0 && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                                s_100_0.EditValue = prev_d.s_150_0.HasValue ? prev_d.s_150_0.Value : 0;
                                s_100_1.EditValue = prev_d.s_150_1.HasValue ? prev_d.s_150_1.Value : 0;
                                s_100_2.EditValue = prev_d.s_150_2.HasValue ? prev_d.s_150_2.Value : 0;
                                s_100_3.EditValue = prev_d.s_150_3.HasValue ? prev_d.s_150_3.Value : 0;
                                s_100_4.EditValue = prev_d.s_150_4.HasValue ? prev_d.s_150_4.Value : 0;
                                s_100_5.EditValue = prev_d.s_150_5.HasValue ? prev_d.s_150_5.Value : 0;
                            }
                        }
                        else
                        {
                            if (!editLoad)
                            {
                                if (RSWdataPrev != null)
                                {
                                    s_100_0.EditValue = RSWdataPrev.s_100_0.HasValue ? RSWdataPrev.s_100_0.Value : s_100_0.EditValue;
                                    s_100_1.EditValue = RSWdataPrev.s_100_1.HasValue ? RSWdataPrev.s_100_1.Value : s_100_1.EditValue;
                                    s_100_2.EditValue = RSWdataPrev.s_100_2.HasValue ? RSWdataPrev.s_100_2.Value : s_100_2.EditValue;
                                    s_100_3.EditValue = RSWdataPrev.s_100_3.HasValue ? RSWdataPrev.s_100_3.Value : s_100_3.EditValue;
                                    s_100_4.EditValue = RSWdataPrev.s_100_4.HasValue ? RSWdataPrev.s_100_4.Value : s_100_4.EditValue;
                                    s_100_5.EditValue = RSWdataPrev.s_100_5.HasValue ? RSWdataPrev.s_100_5.Value : s_100_5.EditValue;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void calcFields_111_112_113(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                s_114_0.EditValue = decimal.Parse(s_111_0.EditValue.ToString()) + decimal.Parse(s_112_0.EditValue.ToString()) + decimal.Parse(s_113_0.EditValue.ToString());
                s_114_3.EditValue = decimal.Parse(s_111_3.EditValue.ToString()) + decimal.Parse(s_112_3.EditValue.ToString()) + decimal.Parse(s_113_3.EditValue.ToString());
                s_114_4.EditValue = decimal.Parse(s_111_4.EditValue.ToString()) + decimal.Parse(s_112_4.EditValue.ToString()) + decimal.Parse(s_113_4.EditValue.ToString());
                s_114_5.EditValue = decimal.Parse(s_111_5.EditValue.ToString()) + decimal.Parse(s_112_5.EditValue.ToString()) + decimal.Parse(s_113_5.EditValue.ToString());
            }
        }

        private void calcFields_100_110_120(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                s_130_0.EditValue = decimal.Parse(s_100_0.EditValue.ToString()) + decimal.Parse(s_110_0.EditValue.ToString()) + decimal.Parse(s_120_0.EditValue.ToString());
                s_130_1.EditValue = decimal.Parse(s_100_1.EditValue.ToString()) + decimal.Parse(s_120_1.EditValue.ToString());
                s_130_2.EditValue = decimal.Parse(s_100_2.EditValue.ToString()) + decimal.Parse(s_120_2.EditValue.ToString());
                s_130_3.EditValue = decimal.Parse(s_100_3.EditValue.ToString()) + decimal.Parse(s_110_3.EditValue.ToString()) + decimal.Parse(s_120_3.EditValue.ToString());
                s_130_4.EditValue = decimal.Parse(s_100_4.EditValue.ToString()) + decimal.Parse(s_110_4.EditValue.ToString()) + decimal.Parse(s_120_4.EditValue.ToString());
                s_130_5.EditValue = decimal.Parse(s_100_5.EditValue.ToString()) + decimal.Parse(s_110_5.EditValue.ToString()) + decimal.Parse(s_120_5.EditValue.ToString());
            }
        }

        private void calcFields_141_142_143(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                s_144_0.EditValue = decimal.Parse(s_141_0.EditValue.ToString()) + decimal.Parse(s_142_0.EditValue.ToString()) + decimal.Parse(s_143_0.EditValue.ToString());
                s_144_1.EditValue = decimal.Parse(s_141_1.EditValue.ToString()) + decimal.Parse(s_142_1.EditValue.ToString()) + decimal.Parse(s_143_1.EditValue.ToString());
                s_144_2.EditValue = decimal.Parse(s_141_2.EditValue.ToString()) + decimal.Parse(s_142_2.EditValue.ToString()) + decimal.Parse(s_143_2.EditValue.ToString());
                s_144_3.EditValue = decimal.Parse(s_141_3.EditValue.ToString()) + decimal.Parse(s_142_3.EditValue.ToString()) + decimal.Parse(s_143_3.EditValue.ToString());
                s_144_4.EditValue = decimal.Parse(s_141_4.EditValue.ToString()) + decimal.Parse(s_142_4.EditValue.ToString()) + decimal.Parse(s_143_4.EditValue.ToString());
                s_144_5.EditValue = decimal.Parse(s_141_5.EditValue.ToString()) + decimal.Parse(s_142_5.EditValue.ToString()) + decimal.Parse(s_143_5.EditValue.ToString());
            }
        }

        private void calcFields_130_140(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                s_150_0.EditValue = decimal.Parse(s_130_0.EditValue.ToString()) - decimal.Parse(s_140_0.EditValue.ToString());
                s_150_1.EditValue = decimal.Parse(s_130_1.EditValue.ToString()) - decimal.Parse(s_140_1.EditValue.ToString());
                s_150_2.EditValue = decimal.Parse(s_130_2.EditValue.ToString()) - decimal.Parse(s_140_2.EditValue.ToString());
                s_150_3.EditValue = decimal.Parse(s_130_3.EditValue.ToString()) - decimal.Parse(s_140_3.EditValue.ToString());
                s_150_4.EditValue = decimal.Parse(s_130_4.EditValue.ToString()) - decimal.Parse(s_140_4.EditValue.ToString());
                s_150_5.EditValue = decimal.Parse(s_130_5.EditValue.ToString()) - decimal.Parse(s_140_5.EditValue.ToString());
            }
        }

        private void calcFields_144(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                byte q = 10;
                if (Quarter.SelectedItem != null)
                {
                    byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q);
                }
                if (RSWdataPrev != null && q != 3)  // Если не первый отчетный период в году
                {
                    s_140_0.EditValue = RSWdataPrev.s_140_0.HasValue ? RSWdataPrev.s_140_0.Value + decimal.Parse(s_144_0.EditValue.ToString()) : decimal.Parse(s_144_0.EditValue.ToString());
                    s_140_1.EditValue = RSWdataPrev.s_140_1.HasValue ? RSWdataPrev.s_140_1.Value + decimal.Parse(s_144_1.EditValue.ToString()) : decimal.Parse(s_144_1.EditValue.ToString());
                    s_140_2.EditValue = RSWdataPrev.s_140_2.HasValue ? RSWdataPrev.s_140_2.Value + decimal.Parse(s_144_2.EditValue.ToString()) : decimal.Parse(s_144_2.EditValue.ToString());
                    s_140_3.EditValue = RSWdataPrev.s_140_3.HasValue ? RSWdataPrev.s_140_3.Value + decimal.Parse(s_144_3.EditValue.ToString()) : decimal.Parse(s_144_3.EditValue.ToString());
                    s_140_4.EditValue = RSWdataPrev.s_140_4.HasValue ? RSWdataPrev.s_140_4.Value + decimal.Parse(s_144_4.EditValue.ToString()) : decimal.Parse(s_144_4.EditValue.ToString());
                    s_140_5.EditValue = RSWdataPrev.s_140_5.HasValue ? RSWdataPrev.s_140_5.Value + decimal.Parse(s_144_5.EditValue.ToString()) : decimal.Parse(s_144_5.EditValue.ToString());
                }
                else
                {
                    if (Options.inputTypeRSW1 == 1)
                    {
                        s_140_0.EditValue = decimal.Parse(s_144_0.EditValue.ToString());
                        s_140_1.EditValue = decimal.Parse(s_144_1.EditValue.ToString());
                        s_140_2.EditValue = decimal.Parse(s_144_2.EditValue.ToString());
                        s_140_3.EditValue = decimal.Parse(s_144_3.EditValue.ToString());
                        s_140_4.EditValue = decimal.Parse(s_144_4.EditValue.ToString());
                        s_140_5.EditValue = decimal.Parse(s_144_5.EditValue.ToString());
                    }

                }
            }
        }



        /// <summary>
        /// Обновление связаных полей раздела 2.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_2_1(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                s_110_0.EditValue = (decimal)0;
                s_111_0.EditValue = (decimal)0;
                s_112_0.EditValue = (decimal)0;
                s_113_0.EditValue = (decimal)0;

                s_110_5.EditValue = (decimal)0;
                s_111_5.EditValue = (decimal)0;
                s_112_5.EditValue = (decimal)0;
                s_113_5.EditValue = (decimal)0;

                foreach (var item in RSW_2_1_List)
                {
                    s_110_0.EditValue = decimal.Parse(s_110_0.EditValue.ToString()) + item.s_205_0 + item.s_206_0;
                    s_111_0.EditValue = decimal.Parse(s_111_0.EditValue.ToString()) + item.s_205_1 + item.s_206_1;
                    s_112_0.EditValue = decimal.Parse(s_112_0.EditValue.ToString()) + item.s_205_2 + item.s_206_2;
                    s_113_0.EditValue = decimal.Parse(s_113_0.EditValue.ToString()) + item.s_205_3 + item.s_206_3;

                    if (yearType != 2015)
                    {
                        s_110_5.EditValue = decimal.Parse(s_110_5.EditValue.ToString()) + item.s_215_0;
                        s_111_5.EditValue = decimal.Parse(s_111_5.EditValue.ToString()) + item.s_215_1;
                        s_112_5.EditValue = decimal.Parse(s_112_5.EditValue.ToString()) + item.s_215_2;
                        s_113_5.EditValue = decimal.Parse(s_113_5.EditValue.ToString()) + item.s_215_3;
                    }
                    else
                    {
                        s_110_5.EditValue = decimal.Parse(s_110_5.EditValue.ToString()) + item.s_214_0;
                        s_111_5.EditValue = decimal.Parse(s_111_5.EditValue.ToString()) + item.s_214_1;
                        s_112_5.EditValue = decimal.Parse(s_112_5.EditValue.ToString()) + item.s_214_2;
                        s_113_5.EditValue = decimal.Parse(s_113_5.EditValue.ToString()) + item.s_214_3;
                    }
                }

                //if (RSWdataPrev != null)
                //{
                //    s_110_0.EditValue = RSWdataPrev.s_110_0.HasValue ? (RSWdataPrev.s_110_0.Value + decimal.Parse(s_110_0.EditValue.ToString())) : s_110_0.EditValue;
                //    s_110_5.EditValue = RSWdataPrev.s_110_5.HasValue ? (RSWdataPrev.s_110_5.Value + decimal.Parse(s_110_5.EditValue.ToString())) : s_110_5.EditValue;
                //}

                //s_110_0.EditValue = decimal.Parse(s_110_0.EditValue.ToString()) + decimal.Parse(s_111_0.EditValue.ToString()) + decimal.Parse(s_112_0.EditValue.ToString()) + decimal.Parse(s_113_0.EditValue.ToString());
                //s_110_5.EditValue = decimal.Parse(s_110_5.EditValue.ToString()) + decimal.Parse(s_111_5.EditValue.ToString()) + decimal.Parse(s_112_5.EditValue.ToString()) + decimal.Parse(s_113_5.EditValue.ToString());
            }

        }


        /// <summary>
        /// Обновление связаных полей раздела 2.2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_2_2(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                if (Options.inputTypeRSW1 == 1)
                {
                    if (RSWdataPrev != null)
                    {
                        s_220_0.EditValue = RSWdataPrev.s_220_0.HasValue ? RSWdataPrev.s_220_0.Value + decimal.Parse(s_220_1.EditValue.ToString()) + decimal.Parse(s_220_2.EditValue.ToString()) + decimal.Parse(s_220_3.EditValue.ToString()) : decimal.Parse(s_220_1.EditValue.ToString()) + decimal.Parse(s_220_2.EditValue.ToString()) + decimal.Parse(s_220_3.EditValue.ToString());
                        s_221_0.EditValue = RSWdataPrev.s_221_0.HasValue ? RSWdataPrev.s_221_0.Value + decimal.Parse(s_221_1.EditValue.ToString()) + decimal.Parse(s_221_2.EditValue.ToString()) + decimal.Parse(s_221_3.EditValue.ToString()) : decimal.Parse(s_221_1.EditValue.ToString()) + decimal.Parse(s_221_2.EditValue.ToString()) + decimal.Parse(s_221_3.EditValue.ToString());
                    }
                    else
                    {
                        s_220_0.EditValue = decimal.Parse(s_220_1.EditValue.ToString()) + decimal.Parse(s_220_2.EditValue.ToString()) + decimal.Parse(s_220_3.EditValue.ToString());
                        s_221_0.EditValue = decimal.Parse(s_221_1.EditValue.ToString()) + decimal.Parse(s_221_2.EditValue.ToString()) + decimal.Parse(s_221_3.EditValue.ToString());
                    }
                }
                s_223_0.EditValue = decimal.Parse(s_220_0.EditValue.ToString()) - decimal.Parse(s_221_0.EditValue.ToString());
                s_223_1.EditValue = decimal.Parse(s_220_1.EditValue.ToString()) - decimal.Parse(s_221_1.EditValue.ToString());
                s_223_2.EditValue = decimal.Parse(s_220_2.EditValue.ToString()) - decimal.Parse(s_221_2.EditValue.ToString());
                s_223_3.EditValue = decimal.Parse(s_220_3.EditValue.ToString()) - decimal.Parse(s_221_3.EditValue.ToString());

                //if (RSWdataPrev != null)
                //{
                //    s_223_0.EditValue = RSWdataPrev.s_223_0.HasValue ? RSWdataPrev.s_223_0.Value + decimal.Parse(s_220_1.EditValue.ToString()) + decimal.Parse(s_220_2.EditValue.ToString()) + decimal.Parse(s_223_3.EditValue.ToString()) : decimal.Parse(s_223_1.EditValue.ToString()) + decimal.Parse(s_223_2.EditValue.ToString()) + decimal.Parse(s_223_3.EditValue.ToString());
                //}
                //else
                //{
                //    s_223_0.EditValue = decimal.Parse(s_223_1.EditValue.ToString()) + decimal.Parse(s_223_2.EditValue.ToString()) + decimal.Parse(s_220_3.EditValue.ToString());
                //}

            }

            if (!AutoCalcSwitch.IsOn && Options.inputTypeRSW1 != 2)
            {
                decimal tariff = 0;
                if (db.DopTariff.Any(x => x.Year == RSWdata.Year))
                {
                    tariff = db.DopTariff.FirstOrDefault(x => x.Year == RSWdata.Year).Tariff1.HasValue ? db.DopTariff.FirstOrDefault(x => x.Year == RSWdata.Year).Tariff1.Value : 0;
                }

                s_224_0.EditValue = decimal.Parse(s_223_0.EditValue.ToString()) * tariff / 100;
                s_224_1.EditValue = ((decimal.Parse(s_223_0.EditValue.ToString()) - decimal.Parse(s_223_2.EditValue.ToString()) - decimal.Parse(s_223_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_223_0.EditValue.ToString()) - decimal.Parse(s_223_1.EditValue.ToString()) - decimal.Parse(s_223_2.EditValue.ToString()) - decimal.Parse(s_223_3.EditValue.ToString())) * tariff / 100);
                s_224_2.EditValue = (((decimal.Parse(s_223_0.EditValue.ToString()) - decimal.Parse(s_223_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_223_0.EditValue.ToString()) - decimal.Parse(s_223_1.EditValue.ToString()) - decimal.Parse(s_223_2.EditValue.ToString()) - decimal.Parse(s_223_3.EditValue.ToString())) * tariff / 100)) - decimal.Parse(s_224_1.EditValue.ToString());
                s_224_3.EditValue = (((decimal.Parse(s_223_0.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_223_0.EditValue.ToString()) - decimal.Parse(s_223_1.EditValue.ToString()) - decimal.Parse(s_223_2.EditValue.ToString()) - decimal.Parse(s_223_3.EditValue.ToString())) * tariff / 100)) - (decimal.Parse(s_224_1.EditValue.ToString()) + decimal.Parse(s_224_2.EditValue.ToString()));
            }

            calcFields_Razd_2_4(null, null);

        }

        /// <summary>
        /// Обновление связаных полей раздела 2.3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_2_3(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                if (Options.inputTypeRSW1 == 1)
                {
                    if (RSWdataPrev != null)
                    {
                        s_230_0.EditValue = RSWdataPrev.s_230_0.HasValue ? RSWdataPrev.s_230_0.Value + decimal.Parse(s_230_1.EditValue.ToString()) + decimal.Parse(s_230_2.EditValue.ToString()) + decimal.Parse(s_230_3.EditValue.ToString()) : decimal.Parse(s_230_1.EditValue.ToString()) + decimal.Parse(s_230_2.EditValue.ToString()) + decimal.Parse(s_230_3.EditValue.ToString());
                        s_231_0.EditValue = RSWdataPrev.s_231_0.HasValue ? RSWdataPrev.s_231_0.Value + decimal.Parse(s_231_1.EditValue.ToString()) + decimal.Parse(s_231_2.EditValue.ToString()) + decimal.Parse(s_231_3.EditValue.ToString()) : decimal.Parse(s_231_1.EditValue.ToString()) + decimal.Parse(s_231_2.EditValue.ToString()) + decimal.Parse(s_231_3.EditValue.ToString());
                    }
                    else
                    {
                        s_230_0.EditValue = decimal.Parse(s_230_1.EditValue.ToString()) + decimal.Parse(s_230_2.EditValue.ToString()) + decimal.Parse(s_230_3.EditValue.ToString());
                        s_231_0.EditValue = decimal.Parse(s_231_1.EditValue.ToString()) + decimal.Parse(s_231_2.EditValue.ToString()) + decimal.Parse(s_231_3.EditValue.ToString());
                    }
                }

                s_233_0.EditValue = decimal.Parse(s_230_0.EditValue.ToString()) - decimal.Parse(s_231_0.EditValue.ToString());
                s_233_1.EditValue = decimal.Parse(s_230_1.EditValue.ToString()) - decimal.Parse(s_231_1.EditValue.ToString());
                s_233_2.EditValue = decimal.Parse(s_230_2.EditValue.ToString()) - decimal.Parse(s_231_2.EditValue.ToString());
                s_233_3.EditValue = decimal.Parse(s_230_3.EditValue.ToString()) - decimal.Parse(s_231_3.EditValue.ToString());

                //if (RSWdataPrev != null)
                //{
                //    s_233_0.EditValue = RSWdataPrev.s_233_0.HasValue ? RSWdataPrev.s_233_0.Value + decimal.Parse(s_230_1.EditValue.ToString()) + decimal.Parse(s_230_2.EditValue.ToString()) + decimal.Parse(s_233_3.EditValue.ToString()) : decimal.Parse(s_233_1.EditValue.ToString()) + decimal.Parse(s_233_2.EditValue.ToString()) + decimal.Parse(s_233_3.EditValue.ToString());
                //}
                //else
                //{
                //    s_233_0.EditValue = decimal.Parse(s_233_1.EditValue.ToString()) + decimal.Parse(s_233_2.EditValue.ToString()) + decimal.Parse(s_230_3.EditValue.ToString());
                //}

            }

            if (!AutoCalcSwitch.IsOn && Options.inputTypeRSW1 != 2)
            {
                decimal tariff = 0;
                if (db.DopTariff.Any(x => x.Year == RSWdata.Year))
                {
                    tariff = db.DopTariff.FirstOrDefault(x => x.Year == RSWdata.Year).Tariff2.HasValue ? db.DopTariff.FirstOrDefault(x => x.Year == RSWdata.Year).Tariff2.Value : 0;
                }

                s_234_0.EditValue = decimal.Parse(s_233_0.EditValue.ToString()) * tariff / 100;
                s_234_1.EditValue = ((decimal.Parse(s_233_0.EditValue.ToString()) - decimal.Parse(s_233_2.EditValue.ToString()) - decimal.Parse(s_233_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_233_0.EditValue.ToString()) - decimal.Parse(s_233_1.EditValue.ToString()) - decimal.Parse(s_233_2.EditValue.ToString()) - decimal.Parse(s_233_3.EditValue.ToString())) * tariff / 100);
                s_234_2.EditValue = (((decimal.Parse(s_233_0.EditValue.ToString()) - decimal.Parse(s_233_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_233_0.EditValue.ToString()) - decimal.Parse(s_233_1.EditValue.ToString()) - decimal.Parse(s_233_2.EditValue.ToString()) - decimal.Parse(s_233_3.EditValue.ToString())) * tariff / 100)) - decimal.Parse(s_234_1.EditValue.ToString());
                s_234_3.EditValue = (((decimal.Parse(s_233_0.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_233_0.EditValue.ToString()) - decimal.Parse(s_233_1.EditValue.ToString()) - decimal.Parse(s_233_2.EditValue.ToString()) - decimal.Parse(s_233_3.EditValue.ToString())) * tariff / 100)) - (decimal.Parse(s_234_1.EditValue.ToString()) + decimal.Parse(s_234_2.EditValue.ToString()));
            }

            calcFields_Razd_2_4(null, null);
        }

        /// <summary>
        /// Обновление строк раздела 1 при изменении Начисленных взносов в разделах 2.2 и 2.3 в ручном режиме
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_2_2__2_3(object sender, EventArgs e)
        {
            if (AutoCalcSwitch.IsOn && Options.inputTypeRSW1 != 2)
            {
                calcFields_Razd_2_4(null, null);
            }
        }

        /// <summary>
        /// Обновление связаных полей раздела 2.4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_2_4(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {

                //Строка 110,111,112,113 графа 6
                s_110_3.EditValue = decimal.Parse(s_224_0.EditValue.ToString());
                s_111_3.EditValue = decimal.Parse(s_224_1.EditValue.ToString());
                s_112_3.EditValue = decimal.Parse(s_224_2.EditValue.ToString());
                s_113_3.EditValue = decimal.Parse(s_224_3.EditValue.ToString());

                foreach (var item in RSW_2_4_List.Where(x => x.CodeBase == 1))
                {
                    s_110_3.EditValue = decimal.Parse(s_110_3.EditValue.ToString()) + item.s_244_0 + item.s_250_0 + item.s_256_0 + item.s_262_0 + item.s_268_0;
                    s_111_3.EditValue = decimal.Parse(s_111_3.EditValue.ToString()) + item.s_244_1 + item.s_250_1 + item.s_256_1 + item.s_262_1 + item.s_268_1;
                    s_112_3.EditValue = decimal.Parse(s_112_3.EditValue.ToString()) + item.s_244_2 + item.s_250_2 + item.s_256_2 + item.s_262_2 + item.s_268_2;
                    s_113_3.EditValue = decimal.Parse(s_113_3.EditValue.ToString()) + item.s_244_3 + item.s_250_3 + item.s_256_3 + item.s_262_3 + item.s_268_3;
                }

                //Строка 110,111,112,113 графа 7
                s_110_4.EditValue = decimal.Parse(s_234_0.EditValue.ToString());
                s_111_4.EditValue = decimal.Parse(s_234_1.EditValue.ToString());
                s_112_4.EditValue = decimal.Parse(s_234_2.EditValue.ToString());
                s_113_4.EditValue = decimal.Parse(s_234_3.EditValue.ToString());

                foreach (var item in RSW_2_4_List.Where(x => x.CodeBase == 2))
                {
                    s_110_4.EditValue = decimal.Parse(s_110_4.EditValue.ToString()) + item.s_244_0 + item.s_250_0 + item.s_256_0 + item.s_262_0 + item.s_268_0;
                    s_111_4.EditValue = decimal.Parse(s_111_4.EditValue.ToString()) + item.s_244_1 + item.s_250_1 + item.s_256_1 + item.s_262_1 + item.s_268_1;
                    s_112_4.EditValue = decimal.Parse(s_112_4.EditValue.ToString()) + item.s_244_2 + item.s_250_2 + item.s_256_2 + item.s_262_2 + item.s_268_2;
                    s_113_4.EditValue = decimal.Parse(s_113_4.EditValue.ToString()) + item.s_244_3 + item.s_250_3 + item.s_256_3 + item.s_262_3 + item.s_268_3;
                }



            }

        }

        /// <summary>
        /// Обновление связаных полей раздела 3.1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_3_1(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                if (decimal.Parse(s_322_0.EditValue.ToString()) != 0 && decimal.Parse(s_321_0.EditValue.ToString()) != 0)
                    s_323_0.EditValue = (decimal.Parse(s_322_0.EditValue.ToString()) / decimal.Parse(s_321_0.EditValue.ToString())) * 100;
                if (decimal.Parse(s_322_1.EditValue.ToString()) != 0 && decimal.Parse(s_321_1.EditValue.ToString()) != 0)
                    s_323_1.EditValue = (decimal.Parse(s_322_1.EditValue.ToString()) / decimal.Parse(s_321_1.EditValue.ToString())) * 100;
                if (decimal.Parse(s_322_2.EditValue.ToString()) != 0 && decimal.Parse(s_321_2.EditValue.ToString()) != 0)
                    s_323_2.EditValue = (decimal.Parse(s_322_2.EditValue.ToString()) / decimal.Parse(s_321_2.EditValue.ToString())) * 100;
                if (decimal.Parse(s_322_3.EditValue.ToString()) != 0 && decimal.Parse(s_321_3.EditValue.ToString()) != 0)
                    s_323_3.EditValue = (decimal.Parse(s_322_3.EditValue.ToString()) / decimal.Parse(s_321_3.EditValue.ToString())) * 100;
            }

        }




        /// <summary>
        /// Обновление связаных полей раздела 3.2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_3_2(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                if (decimal.Parse(s_332_0.EditValue.ToString()) != 0 && decimal.Parse(s_331_0.EditValue.ToString()) != 0)
                    s_333_0.EditValue = (decimal.Parse(s_332_0.EditValue.ToString()) / decimal.Parse(s_331_0.EditValue.ToString())) * 100;
                if (decimal.Parse(s_332_1.EditValue.ToString()) != 0 && decimal.Parse(s_331_1.EditValue.ToString()) != 0)
                    s_333_1.EditValue = (decimal.Parse(s_332_1.EditValue.ToString()) / decimal.Parse(s_331_1.EditValue.ToString())) * 100;
                if (decimal.Parse(s_332_2.EditValue.ToString()) != 0 && decimal.Parse(s_331_2.EditValue.ToString()) != 0)
                    s_333_2.EditValue = (decimal.Parse(s_332_2.EditValue.ToString()) / decimal.Parse(s_331_2.EditValue.ToString())) * 100;
                if (decimal.Parse(s_332_3.EditValue.ToString()) != 0 && decimal.Parse(s_331_3.EditValue.ToString()) != 0)
                    s_333_3.EditValue = (decimal.Parse(s_332_3.EditValue.ToString()) / decimal.Parse(s_331_3.EditValue.ToString())) * 100;


                if (decimal.Parse(s_335_0.EditValue.ToString()) != 0 && decimal.Parse(s_334_0.EditValue.ToString()) != 0)
                    s_336_0.EditValue = (decimal.Parse(s_335_0.EditValue.ToString()) / decimal.Parse(s_334_0.EditValue.ToString())) * 100;
                if (decimal.Parse(s_335_1.EditValue.ToString()) != 0 && decimal.Parse(s_334_1.EditValue.ToString()) != 0)
                    s_336_1.EditValue = (decimal.Parse(s_335_1.EditValue.ToString()) / decimal.Parse(s_334_1.EditValue.ToString())) * 100;
                if (decimal.Parse(s_335_2.EditValue.ToString()) != 0 && decimal.Parse(s_334_2.EditValue.ToString()) != 0)
                    s_336_2.EditValue = (decimal.Parse(s_335_2.EditValue.ToString()) / decimal.Parse(s_334_2.EditValue.ToString())) * 100;
                if (decimal.Parse(s_335_3.EditValue.ToString()) != 0 && decimal.Parse(s_334_3.EditValue.ToString()) != 0)
                    s_336_3.EditValue = (decimal.Parse(s_335_3.EditValue.ToString()) / decimal.Parse(s_334_3.EditValue.ToString())) * 100;

                if (RSWdataPrev != null)
                {
                    s_334_0.EditValue = RSWdataPrev.s_334_0.HasValue ? RSWdataPrev.s_334_0.Value + decimal.Parse(s_334_1.EditValue.ToString()) + decimal.Parse(s_334_2.EditValue.ToString()) + decimal.Parse(s_334_3.EditValue.ToString()) : decimal.Parse(s_334_1.EditValue.ToString()) + decimal.Parse(s_334_2.EditValue.ToString()) + decimal.Parse(s_334_3.EditValue.ToString());
                    s_335_0.EditValue = RSWdataPrev.s_335_0.HasValue ? RSWdataPrev.s_335_0.Value + decimal.Parse(s_335_1.EditValue.ToString()) + decimal.Parse(s_335_2.EditValue.ToString()) + decimal.Parse(s_335_3.EditValue.ToString()) : decimal.Parse(s_335_1.EditValue.ToString()) + decimal.Parse(s_335_2.EditValue.ToString()) + decimal.Parse(s_335_3.EditValue.ToString());
                }

            }

        }

        /// <summary>
        /// Обновление связаных полей раздела 3.3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_3_3(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                if (decimal.Parse(s_342_0.EditValue.ToString()) != 0 && decimal.Parse(s_341_0.EditValue.ToString()) != 0)
                {
                    s_343_0.EditValue = (decimal.Parse(s_342_0.EditValue.ToString()) / decimal.Parse(s_341_0.EditValue.ToString())) * 100;
                }
                if (decimal.Parse(s_342_1.EditValue.ToString()) != 0 && decimal.Parse(s_341_1.EditValue.ToString()) != 0)
                    s_343_1.EditValue = (decimal.Parse(s_342_1.EditValue.ToString()) / decimal.Parse(s_341_1.EditValue.ToString())) * 100;

            }

        }


        /// <summary>
        /// Обновление связаных полей раздела 3.5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_3_5(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                if (decimal.Parse(s_361_0.EditValue.ToString()) != 0)
                    s_363_0.EditValue = (decimal.Parse(s_362_0.EditValue.ToString()) / decimal.Parse(s_361_0.EditValue.ToString())) * 100;
                else
                    s_363_0.EditValue = (decimal)0;
            }

        }

        /// <summary>
        /// Обновление связаных полей раздела 3.6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_3_6(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                if (decimal.Parse(s_371_0.EditValue.ToString()) != 0)
                    s_375_0.EditValue = ((decimal.Parse(s_372_0.EditValue.ToString()) + decimal.Parse(s_373_0.EditValue.ToString()) + decimal.Parse(s_374_0.EditValue.ToString())) / decimal.Parse(s_371_0.EditValue.ToString())) * 100;
                if (decimal.Parse(s_371_1.EditValue.ToString()) != 0)
                    s_375_1.EditValue = ((decimal.Parse(s_372_1.EditValue.ToString()) + decimal.Parse(s_373_1.EditValue.ToString()) + decimal.Parse(s_374_1.EditValue.ToString())) / decimal.Parse(s_371_1.EditValue.ToString())) * 100;
            }

        }

        /// <summary>
        /// Обновление связаных полей раздела 4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_4(object sender, EventArgs e)
        {
            if (!editLoad && Options.inputTypeRSW1 != 2)
            {
                s_120_0.EditValue = (decimal)0;
                s_120_1.EditValue = (decimal)0;
                s_120_2.EditValue = (decimal)0;
                s_120_3.EditValue = (decimal)0;
                s_120_4.EditValue = (decimal)0;
                s_120_5.EditValue = (decimal)0;

                s_121_0.EditValue = (decimal)0;
                s_121_1.EditValue = (decimal)0;

                foreach (var item in RSW_4_List)
                {
                    s_120_0.EditValue = decimal.Parse(s_120_0.EditValue.ToString()) + item.Strah2014.Value;
                    s_120_1.EditValue = decimal.Parse(s_120_1.EditValue.ToString()) + item.Strah2013.Value;
                    s_120_2.EditValue = decimal.Parse(s_120_2.EditValue.ToString()) + item.Nakop2013.Value;
                    decimal temp_sum = item.CodeBase == 1 ? item.Dop21.Value : 0;
                    s_120_3.EditValue = decimal.Parse(s_120_3.EditValue.ToString()) + item.Dop1.Value + temp_sum;
                    temp_sum = item.CodeBase == 2 ? item.Dop21.Value : 0;
                    s_120_4.EditValue = decimal.Parse(s_120_4.EditValue.ToString()) + item.Dop2.Value + temp_sum;
                    s_120_5.EditValue = decimal.Parse(s_120_5.EditValue.ToString()) + item.OMS.Value;

                    s_121_0.EditValue = decimal.Parse(s_121_0.EditValue.ToString()) + item.StrahMoreBase2014.Value;
                    s_121_1.EditValue = decimal.Parse(s_121_1.EditValue.ToString()) + item.StrahMoreBase2013.Value;
                }
            }
        }

        #endregion


        /// <summary>
        /// Заполнить Раздел 2.1 на основе данных Раздела 6.5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fillRazd21Btn_Click(object sender, EventArgs e)
        {
            RSW2014_2_1_Filling child = new RSW2014_2_1_Filling();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.identifier = new Identifier
            {
                InsurerID = RSWdata.InsurerID,
                CorrectionNum = RSWdata.CorrectionNum,
                Quarter = RSWdata.Quarter,
                Year = RSWdata.Year
            };
            child.yearType = yearType;
            child.ShowDialog();
            if (child.RSW_2_1_List != null)
            {
                RSW_2_1_List.Clear();
                foreach (var item in child.RSW_2_1_List)
                {
                    var fields = typeof(FormsRSW2014_1_Razd_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    var names = Array.ConvertAll(fields, field => field.Name);

                    foreach (var item_ in names)
                    {
                        string itemName = item_.TrimStart('_');
                        if (itemName.StartsWith("s_"))
                        {
                            if (item != null)
                            {
                                var properties = item.GetType().GetProperty(itemName);
                                object value = properties.GetValue(item, null);

                                string type = properties.PropertyType.FullName;
                                if (type.Contains("["))
                                    type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                                type = type.Split(',')[0].Split('.')[1].ToLower();

                                switch (type)
                                {
                                    case "decimal":
                                        properties.SetValue(item, value != null ? Math.Round((decimal)value, 2, MidpointRounding.AwayFromZero) : (decimal)0, null);
                                        break;
                                }
                            }

                        }

                    }

                    RSW_2_1_List.Add(item);
                }
                gridUpdate_2_1();
                calcFields_Razd_2_1(null, null);
            }
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            //           radButton2_Click(null, null);
        }

        private void CopyPrevPer_4_Click(object sender, EventArgs e)
        {
            byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
            short y = short.Parse(Year.SelectedItem.Value.ToString());

            byte quarter = 20;
            if (q == 6)
                quarter = 3;
            else if (q == 9)
                quarter = 6;
            else if (q == 0)
                quarter = 9;

            if (db.FormsRSW2014_1_Razd_4.Any(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == quarter))
            {
             try{
                var corrNum = db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == quarter).Max(x => x.CorrectionNum.Value);
                var razd4PrevList = db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == quarter && x.CorrectionNum == corrNum);

                RSW_4_List.Clear();
                foreach (var item in razd4PrevList)
                {
                    var newRazd4 = item.Clone();
                    newRazd4.Quarter = q;
                    newRazd4.CorrectionNum = byte.Parse(CorrectionNum.Value.ToString());

                    RSW_4_List.Add(newRazd4);
                }
             }
                catch{
                    return;
                }


             calcFields_Razd_4(null, null);
             gridUpdate_4();
            }
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

        private void DateUnderwrite_ValueChanged(object sender, EventArgs e)
        {
            if (DateUnderwrite.Value != DateUnderwrite.NullDate)
                DateUnderwriteMaskedEditBox.Text = DateUnderwrite.Value.ToShortDateString();
            else
                DateUnderwriteMaskedEditBox.Text = DateUnderwriteMaskedEditBox.NullText;

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

        private void ConfirmDocDate_ValueChanged(object sender, EventArgs e)
        {
            if (ConfirmDocDate.Value != ConfirmDocDate.NullDate)
                ConfirmDocDateMaskedEditBox.Text = ConfirmDocDate.Value.ToShortDateString();
            else
                ConfirmDocDateMaskedEditBox.Text = ConfirmDocDateMaskedEditBox.NullText;
        }

        private void s_345_0MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (s_345_0MaskedEditBox.Text != s_345_0MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(s_345_0MaskedEditBox.Text, out date))
                {
                    s_345_0.Value = date;
                }
                else
                {
                    s_345_0.Value = s_345_0.NullDate;
                }
            }
            else
            {
                s_345_0.Value = s_345_0.NullDate;
            }
        }

        private void s_345_0_ValueChanged(object sender, EventArgs e)
        {
            if (s_345_0.Value != s_345_0.NullDate)
                s_345_0MaskedEditBox.Text = s_345_0.Value.ToShortDateString();
            else
                s_345_0MaskedEditBox.Text = s_345_0MaskedEditBox.NullText;
        }

        private void s_351_0MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (s_351_0MaskedEditBox.Text != s_351_0MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(s_351_0MaskedEditBox.Text, out date))
                {
                    s_351_0.Value = date;
                }
                else
                {
                    s_351_0.Value = s_351_0.NullDate;
                }
            }
            else
            {
                s_351_0.Value = s_351_0.NullDate;
            }
        }

        private void s_351_0_ValueChanged(object sender, EventArgs e)
        {
            if (s_351_0.Value != s_351_0.NullDate)
                s_351_0MaskedEditBox.Text = s_351_0.Value.ToShortDateString();
            else
                s_351_0MaskedEditBox.Text = s_351_0MaskedEditBox.NullText;
        }

        private void s_501_0_0MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (s_501_0_0MaskedEditBox.Text != s_501_0_0MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(s_501_0_0MaskedEditBox.Text, out date))
                {
                    s_501_0_0.Value = date;
                }
                else
                {
                    s_501_0_0.Value = s_501_0_0.NullDate;
                }
            }
            else
            {
                s_501_0_0.Value = s_501_0_0.NullDate;
            }
        }

        private void s_501_0_0_ValueChanged(object sender, EventArgs e)
        {
            if (s_501_0_0.Value != s_501_0_0.NullDate)
                s_501_0_0MaskedEditBox.Text = s_501_0_0.Value.ToShortDateString();
            else
                s_501_0_0MaskedEditBox.Text = s_501_0_0MaskedEditBox.NullText;
        }

        private void s_501_0_1MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (s_501_0_1MaskedEditBox.Text != s_501_0_1MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(s_501_0_1MaskedEditBox.Text, out date))
                {
                    s_501_0_1.Value = date;
                }
                else
                {
                    s_501_0_1.Value = s_501_0_1.NullDate;
                }
            }
            else
            {
                s_501_0_1.Value = s_501_0_1.NullDate;
            }
        }

        private void s_501_0_1_ValueChanged(object sender, EventArgs e)
        {
            if (s_501_0_1.Value != s_501_0_1.NullDate)
                s_501_0_1MaskedEditBox.Text = s_501_0_1.Value.ToShortDateString();
            else
                s_501_0_1MaskedEditBox.Text = s_501_0_1MaskedEditBox.NullText;
        }

        private void s_501_0_2MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (s_501_0_2MaskedEditBox.Text != s_501_0_2MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(s_501_0_2MaskedEditBox.Text, out date))
                {
                    s_501_0_2.Value = date;
                }
                else
                {
                    s_501_0_2.Value = s_501_0_2.NullDate;
                }
            }
            else
            {
                s_501_0_2.Value = s_501_0_2.NullDate;
            }
        }

        private void s_501_0_2_ValueChanged(object sender, EventArgs e)
        {
            if (s_501_0_2.Value != s_501_0_2.NullDate)
                s_501_0_2MaskedEditBox.Text = s_501_0_2.Value.ToShortDateString();
            else
                s_501_0_2MaskedEditBox.Text = s_501_0_2MaskedEditBox.NullText;
        }

        private void s_501_0_3MaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (s_501_0_3MaskedEditBox.Text != s_501_0_3MaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(s_501_0_3MaskedEditBox.Text, out date))
                {
                    s_501_0_3.Value = date;
                }
                else
                {
                    s_501_0_3.Value = s_501_0_3.NullDate;
                }
            }
            else
            {
                s_501_0_3.Value = s_501_0_3.NullDate;
            }
        }

        private void s_501_0_3_ValueChanged(object sender, EventArgs e)
        {
            if (s_501_0_3.Value != s_501_0_3.NullDate)
                s_501_0_3MaskedEditBox.Text = s_501_0_3.Value.ToShortDateString();
            else
                s_501_0_3MaskedEditBox.Text = s_501_0_3MaskedEditBox.NullText;
        }

        private void updateComplete(object sender, EventArgs e)
        {
            editLoad = false;

            calcFields_100(null, null);

            calcFields_Razd_3_1(null, null);
            calcFields_Razd_3_2(null, null);
            calcFields_Razd_3_3(null, null);
            calcFields_Razd_3_5(null, null);
            calcFields_Razd_3_6(null, null);

            calcFields_141_142_143(null, null);
            calcFields_144(null, null);
            calcFields_130_140(null, null);

            if (action == "edit")
            {
                RSW_2_1_List = db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(x => x.ID).ToList();
                RSW_2_4_List = db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(x => x.ID).ToList();
                RSW_3_4_List = db.FormsRSW2014_1_Razd_3_4.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(x => x.ID).ToList();
                RSW_4_List = db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(x => x.ID).ToList();
                RSW_5_List = db.FormsRSW2014_1_Razd_5.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(x => x.ID).ToList();
                RSW_6_1_List = db.FormsRSW2014_1_Razd_6_1.Where(x => x.InsurerID == RSWdata.InsurerID && x.Quarter == RSWdata.Quarter && x.Year == RSWdata.Year && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(x => x.ID).ToList();
            }

            gridUpdate_2_1();
            gridUpdate_2_4();
            gridUpdate_3_4();
            gridUpdate_4();
            gridUpdate_5();

            List<string> list = new List<string> { };
            for (int i = 0; i <= 5; i++)
            {
                list.Add("s_140_" + i.ToString());
                if (i != 1 && i != 2)
                    list.Add("s_110_" + i.ToString());
            }
            list.Add("s_220_0");
            list.Add("s_221_0");
            list.Add("s_230_0");
            list.Add("s_231_0");
            list.Add("s_334_0");
            list.Add("s_335_0");

            foreach (var item in list)
            {
                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0]).Enabled = (Options.inputTypeRSW1 == 0 || Options.inputTypeRSW1 == 2) ? true : false;
            }

            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            //            gridUpdate_6_1(null, null);

            ConfirmType_SelectedIndexChanged(null, null);

            if (Quarter.SelectedItem != null)
            {
                this.Text = "Форма расчета страховых взносов РСВ-1  - [ " + Quarter.SelectedItem.Text + " ]";
            }

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();
            this.ConfirmDocType.SelectedIndexChanged += (s, с) => ConfirmDocTypeDDL_SelectedIndexChanged();

        }

        private void RSW2014_Edit_Shown(object sender, EventArgs e)
        {
            switch (action)
            {
                case "add":
                    break;
                case "edit":
                    editLoad = true;
                    bw = new BackgroundWorker();
                    bw.DoWork += new System.ComponentModel.DoWorkEventHandler(updateValues);
                    bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(updateComplete);
                    bw.WorkerSupportsCancellation = true;

                    bw.RunWorkerAsync();
                    //updateValues();

                    break;
            }



        }

        private void RSW2014_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (allowClose)
            {

                if (bw != null && bw.IsBusy)
                {
                    bw.CancelAsync();
                }
                if (bw != null)
                {
                    bw.Dispose();
                }

                RSWdata = null;
                RSWdataPrev = null;

                RSW_2_1_List = null;
                RSW_2_4_List = null;
                RSW_3_4_List = null;
                RSW_4_List = null;
                RSW_5_List = null;
                RSW_6_1_List = null;
                db.Dispose();
            }
            else
            {
                DialogResult dialogResult = RadMessageBox.Show("Вы хотите сохранить изменения перед закрытием формы?", "Сохранение записи!", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button3);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        radButton8_Click(null, null);
                        break;
                    case DialogResult.No:
                        if (bw != null && bw.IsBusy)
                        {
                            bw.CancelAsync();
                        }
                        if (bw != null)
                        {
                            bw.Dispose();
                        }

                        RSWdata = null;
                        RSWdataPrev = null;

                        RSW_2_1_List = null;
                        RSW_2_4_List = null;
                        RSW_3_4_List = null;
                        RSW_4_List = null;
                        RSW_5_List = null;
                        RSW_6_1_List = null;
                        db.Dispose();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                        break;
                }

            }

        }

        /// <summary>
        /// Подсчет значений полей Раздела 2.1 и 2.3
        /// </summary>
        /// 
        private void Calculation()
        {
            calcFields_Razd_2_3(null, null);
            calcFields_Razd_2_4(null, null);
        }

        private void AutoCalcSwitch_Toggled(object sender, EventArgs e)
        {
            if (!AutoCalcSwitch.IsOn)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы выбрали режим автоматического расчета страховых взносов.\r\nПроизвести перерасчет взносов?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.Yes)
                {
                    Calculation();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }

            List<string> list = new List<string> { };
            for (int i = 0; i <= 3; i++)
            {
                list.Add("s_224_" + i.ToString());
                list.Add("s_234_" + i.ToString());
            }

            foreach (var item in list)
            {
                //DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0];
                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0]).Enabled = AutoCalcSwitch.IsOn;
            }
        }

        private void grid_2_1_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.RowInfo.Group == null && e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.TextAlignment = ContentAlignment.BottomRight;
                e.CellElement.Font = totalRowsFont;
            }
        }

        private void radGridView1_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo != null && e.CellElement.ColumnInfo.Name == "Num_" && string.IsNullOrEmpty(e.CellElement.Text))
            {
                e.CellElement.Text = (e.CellElement.RowIndex + 1).ToString();
            }
        }


































    }
}
