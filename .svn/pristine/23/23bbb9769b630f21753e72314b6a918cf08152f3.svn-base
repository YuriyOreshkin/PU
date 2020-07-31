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

namespace PU.FormsRSW2_2014
{
    public partial class RSW2_2014_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool editLoad { get; set; }
        private bool autoCalc { get; set; }
        public short YearT { get; set; }

        public FormsRSW2014_2_1 RSWdata { get; set; }
        public List<FormsRSW2014_2_2> RSW_2_2_List = new List<FormsRSW2014_2_2>();
        public List<FormsRSW2014_2_3> RSW_2_3_List = new List<FormsRSW2014_2_3>();
        private List<ErrList> errMessBox = new List<ErrList>();

        public short year_ { get; set; }
        public byte CorrNum { get; set; }
        bool allowClose = false;

        public RSW2_2014_Edit()
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

        private void RSW2_2014_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            checkAccessLevel();

            this.radPageView1.SelectedPage = this.radPageView1.Pages[0];

            DateUnderwrite.Value = DateTime.Now.Date;

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
            this.ConfirmDocType.ResetText();

            #endregion

            switch (action)
            {
                case "add":
                    #region Отчетный период
                    Year.Enabled = true;
                    RSWdata = new FormsRSW2014_2_1 { AutoCalc = true};

                    // выпад список "календарный год"

                    Year.Items.Single(x => x.Text.ToString() == YearT.ToString()).Selected = true;


                    //if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                    //    Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                    //else
                    //    Year.Items.OrderByDescending(x => x.Value).First().Selected = true;
                  
                    #endregion  

                    break;
                case "edit":
                    Year.Items.Single(x => x.Text.ToString() == RSWdata.Year.ToString()).Selected = true;

                    break;
            }
            Year.Enabled = false;

            editLoad = true;
            AutoCalcSwitch.IsOn = RSWdata.AutoCalc.Value;
            editLoad = false;
            autoCalc = !AutoCalcSwitch.IsOn;
            s_110_0.Enabled = AutoCalcSwitch.IsOn;
            s_110_3.Enabled = AutoCalcSwitch.IsOn;

            if (Year.Text != "")
            {
                if (Year.Text == "2014")
                {
                    s_100_0.Enabled = true;
                    s_100_1.Enabled = true;
                    s_100_2.Enabled = true;
                    s_100_3.Enabled = true;
                }
                else if (Year.Text == "2015" || Year.Text == "2016") 
                {
                    radLabel2.Text = "Номер уточнения";
                    radLabel1.Text = "(0 - исходная, 1-999 - номер уточнения)";
                }

                try
                {
                    getBase(Year.SelectedItem.Text);

                    getPrevData();
                }
                catch
                { }


                switch (action)
                {
                    case "add":

                        RSWdata.InsurerID = Options.InsID;
                        RSWdata.CorrectionNum = byte.Parse(CorrectionNum.Value.ToString());
                        RSWdata.Year = Int16.Parse(Year.Text);

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
                        CorrectionNum.Value = RSWdata.CorrectionNum;

                        WorkStop.Checked = RSWdata.WorkStop == 0 ? false : true;
                        CountEmployers.Value = RSWdata.CountEmployers.HasValue ? RSWdata.CountEmployers.Value : 0;
                        CountConfirmDoc.Value = RSWdata.CountConfirmDoc.HasValue ? RSWdata.CountConfirmDoc.Value : 0;
                        DateUnderwrite.Value = RSWdata.DateUnderwrite.HasValue ? RSWdata.DateUnderwrite.Value : DateTime.Now;

                        ConfirmType.SelectedIndex = RSWdata.ConfirmType - 1;
                        ConfirmLastName.Text = RSWdata.ConfirmLastName;
                        ConfirmFirstName.Text = RSWdata.ConfirmFirstName;
                        ConfirmMiddleName.Text = RSWdata.ConfirmMiddleName;
                        ConfirmOrgName.Text = RSWdata.ConfirmOrgName;

                        if (ConfirmType.SelectedIndex > 0 && RSWdata.ConfirmDocType_ID.HasValue)
                        {
                            ConfirmDocType.Items.FirstOrDefault(x => x.Value.ToString() == RSWdata.ConfirmDocType_ID.Value.ToString()).Selected = true;
                            ConfirmDocName.Text = RSWdata.ConfirmDocName;
                            ConfirmDocSerLat.Text = RSWdata.ConfirmDocSerLat;
                            ConfirmDocNum.Text = RSWdata.ConfirmDocNum.HasValue ? RSWdata.ConfirmDocNum.Value.ToString() : "";
                            if (RSWdata.ConfirmDocDate.HasValue)
                                ConfirmDocDate.Value = RSWdata.ConfirmDocDate.Value;
                            ConfirmDocKemVyd.Text = RSWdata.ConfirmDocKemVyd;

                            if (RSWdata.ConfirmDocDateBegin.HasValue)
                                ConfirmDocDateBegin.Value = RSWdata.ConfirmDocDateBegin.Value;
                            if (RSWdata.ConfirmDocDateEnd.HasValue)
                                ConfirmDocDateEnd.Value = RSWdata.ConfirmDocDateEnd.Value;

                        }

                        updateValues();
                        editLoad = false;
                        break;
                }
            }
            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            ConfirmType_SelectedIndexChanged(null, null);



        }


        private void Year_SelectedIndexChanged()
        {
            getBase(Year.SelectedItem.Text);

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                getPrevData();
            }
        }

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
                radLabel17.Text = "Максимальная база: " + Options.mrot.NalogBase.ToString() + " рублей в " + Options.mrot.Year.ToString() + " году";
            else
                radLabel17.Text = "Максимальная база - не определена...";

        }

        private void getPrevData()
        {
            if (autoCalc)
            {
                short y = short.Parse(Year.SelectedItem.Text);

                s_100_0.EditValue = (decimal)0;
                s_100_1.EditValue = (decimal)0;
                s_100_2.EditValue = (decimal)0;
                s_100_3.EditValue = (decimal)0;


                if (y >= 2015)
                {
                    y--;

                    if (db.FormsRSW2014_2_1.Any(x => x.Year == y && x.InsurerID == Options.InsID))
                    {
                        FormsRSW2014_2_1 RSWdataPrev = db.FormsRSW2014_2_1.Where(x => x.Year == y && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                        s_100_0.EditValue = RSWdataPrev.s_150_0.HasValue ? RSWdataPrev.s_150_0.Value : (decimal)0;
                        s_100_1.EditValue = RSWdataPrev.s_150_1.HasValue ? RSWdataPrev.s_150_1.Value : (decimal)0;
                        s_100_2.EditValue = RSWdataPrev.s_150_2.HasValue ? RSWdataPrev.s_150_2.Value : (decimal)0;
                        s_100_3.EditValue = RSWdataPrev.s_150_3.HasValue ? RSWdataPrev.s_150_3.Value : (decimal)0;

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

        /// <summary>
        /// Обновление полей формы РСВ-2 и таблиц
        /// </summary>
        private void updateValues()
        {
            var fields = typeof(FormsRSW2014_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                            if (RSWdata != null)
                            {
                                var properties = RSWdata.GetType().GetProperty(itemName);
                                object value = properties.GetValue(RSWdata, null);
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



            gridUpdate_2();
            gridUpdate_3();
        }


        #region Раздел 2

        /// <summary>
        /// обновление таблицы раздела 2
        /// </summary>
        private void gridUpdate_2()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            grid_2.Rows.Clear();

            if (RSW_2_2_List.Count() != 0)
            {
                foreach (var item in RSW_2_2_List)
                {
                    string contrNum = "";
                    string insNum = "";
                    if (item.ControlNumber != null)
                    {
                        contrNum = item.ControlNumber.HasValue ? item.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                    }
                    if (!String.IsNullOrEmpty(item.InsuranceNumber))
                    {
                        insNum = !String.IsNullOrEmpty(item.InsuranceNumber) ? item.InsuranceNumber.Substring(0, 3) + "-" + item.InsuranceNumber.Substring(3, 3) + "-" + item.InsuranceNumber.Substring(6, 3) : "";
                    }

                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.grid_2.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["NumRec"].Value = item.NumRec.Value.ToString();
                    rowInfo.Cells["FIO"].Value = item.LastName + " " + item.FirstName + " " + item.MiddleName;
                    rowInfo.Cells["SNILS"].Value = insNum + " " + contrNum;
                    rowInfo.Cells["Year"].Value = item.Year;
                    rowInfo.Cells["DateBegin"].Value = item.DateBegin.HasValue ? item.DateBegin.Value.ToShortDateString() : "";
                    rowInfo.Cells["DateEnd"].Value = item.DateEnd.HasValue ? item.DateEnd.Value.ToShortDateString() : "";
                    rowInfo.Cells["OPS"].Value = item.SumOPS.HasValue ? Math.Round(item.SumOPS.Value, 2, MidpointRounding.AwayFromZero).ToString() : "0.00";
                    rowInfo.Cells["OMS"].Value = item.SumOMS.HasValue ? Math.Round(item.SumOMS.Value, 2, MidpointRounding.AwayFromZero).ToString() : "0.00";
                    grid_2.Rows.Add(rowInfo);
                }
            }

            grid_2.Refresh();
            updateTextBoxes_Razd_2();
        }

        /// <summary>
        /// Функция обновления результирующих значений в полях под таблицей Раздела 2
        /// </summary>
        /// <param name="rsw"></param>
        private void updateTextBoxes_Razd_2()
        {
            if (RSW_2_2_List.Count != 0)
            {
                decimal[] sum = new decimal[2] { 0, 0};

                sum[0] = Math.Round(RSW_2_2_List.Where(x => x.SumOPS.HasValue).Sum(x => x.SumOPS.Value), 2, MidpointRounding.AwayFromZero);
                sum[1] = Math.Round(RSW_2_2_List.Where(x => x.SumOMS.HasValue).Sum(x => x.SumOMS.Value), 2, MidpointRounding.AwayFromZero);

                OPSLabel_2.Text = sum[0] != 0 ? sum[0].ToString() : "0.00";
                OMSLabel_2.Text = sum[1] != 0 ? sum[1].ToString() : "0.00";

            }
            else
            {
                OPSLabel_2.Text = "0.00";
                OMSLabel_2.Text = "0.00";
            }


        }


        #endregion

        #region Раздел 3

        /// <summary>
        /// обновление таблицы раздела 3
        /// </summary>
        private void gridUpdate_3()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            grid_3.Rows.Clear();

            if (RSW_2_3_List.Count() != 0)
            {
                foreach (var item in RSW_2_3_List)
                {
                    string contrNum = "";
                    string insNum = "";
                    if (item.ControlNumber != null)
                    {
                        contrNum = item.ControlNumber.HasValue ? item.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                    }
                    if (!String.IsNullOrEmpty(item.InsuranceNumber))
                    {
                        insNum = !String.IsNullOrEmpty(item.InsuranceNumber) ? item.InsuranceNumber.Substring(0, 3) + "-" + item.InsuranceNumber.Substring(3, 3) + "-" + item.InsuranceNumber.Substring(6, 3) : "";
                    }


                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.grid_3.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["NumRec"].Value = item.NumRec.Value.ToString();
                    rowInfo.Cells["FIO"].Value = item.LastName + " " + item.FirstName + " " + item.MiddleName;
                    rowInfo.Cells["SNILS"].Value = insNum + " " + contrNum;
                    rowInfo.Cells["Year"].Value = item.Year;
                    rowInfo.Cells["DateBegin"].Value = item.DateBegin.HasValue ? item.DateBegin.Value.ToShortDateString() : "";
                    rowInfo.Cells["DateEnd"].Value = item.DateEnd.HasValue ? item.DateEnd.Value.ToShortDateString() : "";
                    rowInfo.Cells["OPS"].Value = item.SumOPS_D.HasValue ? Utils.decToStr(item.SumOPS_D.Value) : "0.00";
                    rowInfo.Cells["Strah"].Value = item.SumStrah_D.HasValue ? Utils.decToStr(item.SumStrah_D.Value) : "0.00";
                    rowInfo.Cells["Nakop"].Value = item.SumNakop_D.HasValue ? Utils.decToStr(item.SumNakop_D.Value) : "0.00";
                    rowInfo.Cells["OMS"].Value = item.SumOMS_D.HasValue ? Utils.decToStr(item.SumOMS_D.Value) : "0.00";
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
            if (RSW_2_3_List.Count != 0)
            {
                decimal[] sum = new decimal[4] { 0, 0, 0, 0 };

                sum[0] = Math.Round(RSW_2_3_List.Sum(x => x.SumOPS_D.Value), 2, MidpointRounding.AwayFromZero);
                sum[1] = Math.Round(RSW_2_3_List.Sum(x => x.SumStrah_D.Value), 2, MidpointRounding.AwayFromZero);
                sum[2] = Math.Round(RSW_2_3_List.Sum(x => x.SumNakop_D.Value), 2, MidpointRounding.AwayFromZero);
                sum[3] = Math.Round(RSW_2_3_List.Sum(x => x.SumOMS_D.Value), 2, MidpointRounding.AwayFromZero);

                OPSLabel_3.Text = sum[0].ToString();
                strahLabel_3.Text = sum[1].ToString();
                nakopLabel_3.Text = sum[2].ToString();
                OMSLabel_3.Text = sum[3].ToString();

            }
            else
            {
                OPSLabel_3.Text = "0.00";
                strahLabel_3.Text = "0.00";
                nakopLabel_3.Text = "0.00";
                OMSLabel_3.Text = "0.00";
            }


        }


        #endregion

        private void AutoCalcSwitch_Toggled(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                autoCalc = !AutoCalcSwitch.IsOn;
                s_110_0.Enabled = AutoCalcSwitch.IsOn;
                s_110_3.Enabled = AutoCalcSwitch.IsOn;

                if (short.Parse(Year.Text) >= 2015)
                {
                    s_100_0.Enabled = AutoCalcSwitch.IsOn;
                    s_100_1.Enabled = AutoCalcSwitch.IsOn;
                    s_100_2.Enabled = AutoCalcSwitch.IsOn;
                    s_100_3.Enabled = AutoCalcSwitch.IsOn;

                    if (!AutoCalcSwitch.IsOn)
                    {
                        getPrevData();
                    }
                }
                else if (short.Parse(Year.Text) == 2014)
                {
                    s_100_0.Enabled = true;
                    s_100_1.Enabled = true;
                    s_100_2.Enabled = true;
                    s_100_3.Enabled = true;
                }

                if (!AutoCalcSwitch.IsOn)
                {
                    DialogResult dialogResult = RadMessageBox.Show(this, "Вы выбрали режим автоматического расчета страховых взносов.\r\nПроизвести перерасчет взносов?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                    if (dialogResult == DialogResult.Yes)
                    {
                        calcFields_Razd_2();
                        calcFields_Razd_3();
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                    }
                }


            }
        }



        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Сохранение, проверка данных формы РСВ-2

        /// <summary>
        /// Проверка введенных данных формы РСВ-2
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            if (Year.Text == "")
                errMessBox.Add(new ErrList { name = "Не выбран отчетный период", control = "Year" });

            if (ConfirmType.SelectedItem == null)
                errMessBox.Add(new ErrList { name = "Не выбрано подтверждающее лицо", control = "ConfirmType" });


            int ccd = int.Parse(CountConfirmDoc.Value.ToString());
            if (ccd > 255)
                errMessBox.Add(new ErrList { name = "Количество подтверждающих документов не может быть больше 255", control = "CountConfirmDoc" });



            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        /// <summary>
        /// Сохранение данных формы РСВ-2
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

        /// <summary>
        /// Сбор данных с основной экранной формы редактировния формы РСВ-1
        /// </summary>
        private void getValues()
        {
            var fields = typeof(FormsRSW2014_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                            string type = RSWdata.GetType().GetProperty(itemName).PropertyType.FullName;
                            if (type.Contains("["))
                                type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                            type = type.Split(',')[0].Split('.')[1].ToLower();
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
                                    case "datetime":
                                        RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, DateTime.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text), null);
                                        break;
                                    case "string":
                                        RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text, null);
                                        break;
                                }
                            }
                            else
                                RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, null, null);
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
            long InsID = Options.InsID;
            byte corrNum = byte.Parse(CorrectionNum.Value.ToString());
            short y_ = Int16.Parse(Year.Text);

            switch (action)
            {
                case "add":
                    if (db.FormsRSW2014_2_1.Any(x => x.InsurerID == InsID && x.Year == y_ && x.CorrectionNum == corrNum))
                    {
                        RadMessageBox.Show("Дублирование записи по ключу уникальности (страхователь, календарный год, номер корректировки)");
                        return;
                    }
                    break;
                case "edit":
                    if (db.FormsRSW2014_2_1.Any(x => x.InsurerID == InsID && x.Year == y_ && x.CorrectionNum == corrNum && x.ID != RSWdata.ID))
                    {
                        RadMessageBox.Show("Дублирование записи по ключу уникальности (страхователь, календарный год, номер корректировки)");
                        return;
                    }
                    break;
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

            if (flag_ok)
                try
                {
                    flag_ok = false;

                    RSWdata.InsurerID = Options.InsID;
                    RSWdata.CorrectionNum = byte.Parse(CorrectionNum.Value.ToString());

                    RSWdata.Year = Int16.Parse(Year.Text);
                    RSWdata.WorkStop = WorkStop.Checked ? (byte)1 : (byte)0;
                    RSWdata.CountEmployers = int.Parse(CountEmployers.Value.ToString());
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
                    if (ConfirmDocNum.Text == "")
                        RSWdata.ConfirmDocNum = null;
                    else
                        RSWdata.ConfirmDocNum = int.Parse(ConfirmDocNum.Text);
                    if (ConfirmDocDate.Text == "")
                        RSWdata.ConfirmDocDate = null;
                    else
                        RSWdata.ConfirmDocDate = ConfirmDocDate.Value.Date;
                    RSWdata.ConfirmDocKemVyd = ConfirmDocKemVyd.Text;
                    if (ConfirmDocDateBegin.Text == "")
                        RSWdata.ConfirmDocDateBegin = null;
                    else
                        RSWdata.ConfirmDocDateBegin = ConfirmDocDateBegin.Value.Date;
                    if (ConfirmDocDateEnd.Text == "")
                        RSWdata.ConfirmDocDateEnd = null;
                    else
                        RSWdata.ConfirmDocDateEnd = ConfirmDocDateEnd.Value.Date;
                    RSWdata.AutoCalc = AutoCalcSwitch.IsOn;

                    flag_ok = true;



                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка во время сохранения данных с главного окна редактирования формы РСВ-2. Код ошибки: " + ex.Message);
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
                            db.AddToFormsRSW2014_2_1(RSWdata);
                            db.SaveChanges();

                            foreach (var item in RSW_2_2_List)
                            {
                                item.FormsRSW2014_2_1D = RSWdata.ID;
                                FormsRSW2014_2_2 r = new FormsRSW2014_2_2();

                                var fields = typeof(FormsRSW2014_2_2).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                                db.AddToFormsRSW2014_2_2(r);
                            }

                            foreach (var item in RSW_2_3_List)
                            {
                                item.FormsRSW2014_2_1D = RSWdata.ID;
                                FormsRSW2014_2_3 r = new FormsRSW2014_2_3();

                                var fields = typeof(FormsRSW2014_2_3).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                                db.AddToFormsRSW2014_2_3(r);
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
                    //изменение записи формы РСВ-2
                    case "edit":
                        // выбираем из базы исходную запись по идешнику
                        FormsRSW2014_2_1 r1 = db.FormsRSW2014_2_1.FirstOrDefault(x => x.ID == RSWdata.ID);
                        try
                        {
                            var fields = typeof(FormsRSW2014_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                            #region обрабатываем записи о выплатах из Раздела 2
                            try
                            {
                                var RSW_2_2_List_from_db = db.FormsRSW2014_2_2.Where(x => x.FormsRSW2014_2_1D == RSWdata.ID);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_2_2_List.Select(x => x.ID);
                                var list_for_del = RSW_2_2_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_2_2.DeleteObject(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_2_2_List_from_db = db.FormsRSW2014_2_2.Where(x => x.FormsRSW2014_2_1D == RSWdata.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 2.1 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_2_2).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_2_2_List)
                                {
                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_2_2_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_2_2 rsw_temp = RSW_2_2_List_from_db.Single(x => x.ID == item.ID);

                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
                                            if (item_.IndexOf("FormsRSW2014_2_1D") < 0)
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
                                        item.FormsRSW2014_2_1D = RSWdata.ID;
                                        FormsRSW2014_2_2 r = new FormsRSW2014_2_2();

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

                                        db.AddToFormsRSW2014_2_2(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Раздела 2 произошла ошибка. Код ошибки: " + ex.Message);

                            }
                            #endregion

                            #region обрабатываем записи о выплатах из Раздела 3
                            try
                            {
                                var RSW_2_3_List_from_db = db.FormsRSW2014_2_3.Where(x => x.FormsRSW2014_2_1D == RSWdata.ID);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_2_3_List.Select(x => x.ID);
                                var list_for_del = RSW_2_3_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_2_3.DeleteObject(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_2_3_List_from_db = db.FormsRSW2014_2_3.Where(x => x.FormsRSW2014_2_1D == RSWdata.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 2.1 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_2_3).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_2_3_List)
                                {
                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_2_3_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_2_3 rsw_temp = RSW_2_3_List_from_db.Single(x => x.ID == item.ID);

                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
                                            if (item_.IndexOf("FormsRSW2014_2_1D") < 0)
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
                                        item.FormsRSW2014_2_1D = RSWdata.ID;
                                        FormsRSW2014_2_3 r = new FormsRSW2014_2_3();

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

                                        db.AddToFormsRSW2014_2_3(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Раздела 3 произошла ошибка. Код ошибки: " + ex.Message);

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

        private void addBtn_2_Click(object sender, EventArgs e)
        {
            RSW2_2014_2_Edit child = new RSW2_2014_2_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.NumberSpin.Value = RSW_2_2_List.Count() > 0 ? (RSW_2_2_List.Max(x => x.NumRec).Value == 1000 ? 1000 : (RSW_2_2_List.Max(x => x.NumRec).Value + 1)) : 1;
            child.CalcYear = short.Parse(Year.Text);
            child.autoCalc = !AutoCalcSwitch.IsOn;
            child.numRecList = RSW_2_2_List.Select(x => x.NumRec.Value).ToList();
            child.ShowDialog();
            if (child.formData != null)
            {
                RSW_2_2_List.Add(child.formData);
                gridUpdate_2();
                calcFields_Razd_2();
            }
        }

        private void editBtn_2_Click(object sender, EventArgs e)
        {
            if (grid_2.RowCount != 0)
            {
                int rowindex = grid_2.CurrentRow.Index;

                RSW2_2014_2_Edit child = new RSW2_2014_2_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.autoCalc = !AutoCalcSwitch.IsOn;
                child.CalcYear = short.Parse(Year.Text);
                child.numRecList = RSW_2_2_List.Select(x => x.NumRec.Value).ToList();
                child.formData = RSW_2_2_List.Skip(rowindex).Take(1).First();
                child.ShowDialog();

                if (child.formData != null)
                {
                    RSW_2_2_List.RemoveAt(rowindex);
                    RSW_2_2_List.Insert(rowindex, child.formData);

                    gridUpdate_2();
                    calcFields_Razd_2();
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
        private void delBtn_2_Click(object sender, EventArgs e)
        {
            if (grid_2.RowCount != 0)
            {
                int rowindex = grid_2.CurrentRow.Index;

                RSW_2_2_List.RemoveAt(rowindex);

                gridUpdate_2();
                calcFields_Razd_2();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void grid_2_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_2_Click(null, null);
        }

        private void addBtn_3_Click(object sender, EventArgs e)
        {
            RSW2_2014_3_Edit child = new RSW2_2014_3_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.NumberSpin.Value = RSW_2_3_List.Count() > 0 ? (RSW_2_3_List.Max(x => x.NumRec).Value == 1000 ? 1000 : (RSW_2_3_List.Max(x => x.NumRec).Value + 1)) : 1;
            child.autoCalc = !AutoCalcSwitch.IsOn;
            child.CalcYear = short.Parse(Year.Text);
            child.YearT = YearT;
            child.numRecList = RSW_2_3_List.Select(x => x.NumRec.Value).ToList();
            child.ShowDialog();
            if (child.formData != null)
            {
                RSW_2_3_List.Add(child.formData);
                gridUpdate_3();
                calcFields_Razd_3();
            }
        }

        private void editBtn_3_Click(object sender, EventArgs e)
        {
            if (grid_3.RowCount != 0)
            {
                int rowindex = grid_3.CurrentRow.Index;

                RSW2_2014_3_Edit child = new RSW2_2014_3_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.autoCalc = !AutoCalcSwitch.IsOn;
                child.CalcYear = short.Parse(Year.Text);
                child.YearT = YearT;
                child.numRecList = RSW_2_3_List.Select(x => x.NumRec.Value).ToList();
                child.formData = RSW_2_3_List.Skip(rowindex).Take(1).First();
                child.ShowDialog();

                if (child.formData != null)
                {
                    RSW_2_3_List.RemoveAt(rowindex);
                    RSW_2_3_List.Insert(rowindex, child.formData);

                    gridUpdate_3();
                    calcFields_Razd_3();
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

                RSW_2_3_List.RemoveAt(rowindex);

                gridUpdate_3();
                calcFields_Razd_3();

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

        /// <summary>
        /// Обновление связаных полей раздела 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_2()
        {
            if (autoCalc)
            {
                if (!editLoad)
                {
                    //s_110_0.EditValue = 0;
                    //s_110_3.EditValue = 0;

                    //foreach (var item in RSW_2_2_List)
                    //{
                    //    s_110_0.EditValue = decimal.Parse(s_110_0.EditValue.ToString()) + item.SumOPS;
                    //    s_110_3.EditValue = decimal.Parse(s_110_3.EditValue.ToString()) + item.SumOMS;
                    //}

                    s_110_0.EditValue = RSW_2_2_List.Sum(x => x.SumOPS.Value);
                    s_110_3.EditValue = RSW_2_2_List.Sum(x => x.SumOMS.Value);

                }
            }

        }

        /// <summary>
        /// Обновление связаных полей раздела 3
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void calcFields_Razd_3()
        {
            if (!editLoad)
            {
                //s_120_0.EditValue = 0;
                //s_120_1.EditValue = 0;
                //s_120_2.EditValue = 0;
                //s_120_3.EditValue = 0;

//                foreach (var item in RSW_2_3_List)
//                {
                    //s_120_0.EditValue = decimal.Parse(s_120_0.EditValue.ToString()) + item.SumOPS_D.Value;
                    //s_120_1.EditValue = decimal.Parse(s_120_1.EditValue.ToString()) + item.SumStrah_D;
                    //s_120_2.EditValue = decimal.Parse(s_120_2.EditValue.ToString()) + item.SumNakop_D;
                    //s_120_3.EditValue = decimal.Parse(s_120_3.EditValue.ToString()) + item.SumOMS_D;
//                }

                s_120_0.EditValue = RSW_2_3_List.Sum(x => x.SumOPS_D.Value);
                s_120_1.EditValue = RSW_2_3_List.Sum(x => x.SumStrah_D.Value);
                s_120_2.EditValue = RSW_2_3_List.Sum(x => x.SumNakop_D.Value);
                s_120_3.EditValue = RSW_2_3_List.Sum(x => x.SumOMS_D.Value);

            }
        }

        private void calcFields_100_110_120(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                s_130_0.EditValue = decimal.Parse(s_100_0.EditValue.ToString()) + decimal.Parse(s_110_0.EditValue.ToString()) + decimal.Parse(s_120_0.EditValue.ToString());
                s_130_1.EditValue = decimal.Parse(s_100_1.EditValue.ToString()) + decimal.Parse(s_120_1.EditValue.ToString());
                s_130_2.EditValue = decimal.Parse(s_100_2.EditValue.ToString()) + decimal.Parse(s_120_2.EditValue.ToString());
                s_130_3.EditValue = decimal.Parse(s_100_3.EditValue.ToString()) + decimal.Parse(s_110_3.EditValue.ToString()) + decimal.Parse(s_120_3.EditValue.ToString());
            }
        }

        private void calcFields_130_140(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                s_150_0.EditValue = decimal.Parse(s_130_0.EditValue.ToString()) - decimal.Parse(s_140_0.EditValue.ToString());
                s_150_1.EditValue = decimal.Parse(s_130_1.EditValue.ToString()) - decimal.Parse(s_140_1.EditValue.ToString());
                s_150_2.EditValue = decimal.Parse(s_130_2.EditValue.ToString()) - decimal.Parse(s_140_2.EditValue.ToString());
                s_150_3.EditValue = decimal.Parse(s_130_3.EditValue.ToString()) - decimal.Parse(s_140_3.EditValue.ToString());
            }
        }

        private void copy130To140Btn_Click(object sender, EventArgs e)
        {
            s_140_0.EditValue = s_130_0.EditValue;
            s_140_1.EditValue = s_130_1.EditValue;
            s_140_2.EditValue = s_130_2.EditValue;
            s_140_3.EditValue = s_130_3.EditValue;
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

        private void ConfirmDocDateBegin_ValueChanged(object sender, EventArgs e)
        {
            if (ConfirmDocDateBegin.Value != ConfirmDocDateBegin.NullDate)
                ConfirmDocDateBeginMaskedEditBox.Text = ConfirmDocDateBegin.Value.ToShortDateString();
            else
                ConfirmDocDateBeginMaskedEditBox.Text = ConfirmDocDateBeginMaskedEditBox.NullText;
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

        private void ConfirmDocDateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (ConfirmDocDateEnd.Value != ConfirmDocDateEnd.NullDate)
                ConfirmDocDateEndMaskedEditBox.Text = ConfirmDocDateEnd.Value.ToShortDateString();
            else
                ConfirmDocDateEndMaskedEditBox.Text = ConfirmDocDateEndMaskedEditBox.NullText;
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

        private void RSW2_2014_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (allowClose)
            {

                RSWdata = null;
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
                        RSWdata = null;
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
