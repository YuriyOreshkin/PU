using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using PU.Models;
using PU.FormsRSW2014;
using Telerik.WinControls.UI;
using Telerik.WinControls.Primitives;
using System.Reflection;

namespace PU.FormsRSW2014
{
    public partial class RSW2014_2_1_Edit : Telerik.WinControls.UI.RadForm
    {
        public pu6Entities db = new pu6Entities();
//        public pu6Entities db { get; set; }
        public string action { get; set; }
        public short yearType { get; set; }
        public bool cleanData = true;
        private decimal tariffStrah = 0;
        private decimal tariffOMS = 0;
        private decimal tariffMore = 0;

        private List<ErrList> errMessBox = new List<ErrList>();

        public TariffCode tariffCode { get; set; }
        public FormsRSW2014_1_Razd_2_1 formData { get; set; }
        public FormsRSW2014_1_Razd_2_1 formDataPrev { get; set; }


        public RSW2014_2_1_Edit()
        {
            InitializeComponent();
            this.MouseWheel += new MouseEventHandler(Panel1_MouseWheel);
        }

        private void Panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            panel1.Focus();
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

        public void changeFormByYear()
        {
            if (yearType == 2015)
            {
                tableLayoutPanel1.RowStyles[18].Height = 0;
                panel215n.Visible = false;
                panel215.Visible = false;
                s_215_0.Visible = false;
                s_215_1.Visible = false;
                s_215_2.Visible = false;
                s_215_3.Visible = false;

                label213n.Text = "База для начисления страховых взносов на обязательное медицинское страхование (210-211-212)";
                label214n.Text = "Начислено страховых взносов на обязательное медицинское страхование";


            }
            else if (yearType == 2014 || yearType == 2012)
            {
                tableLayoutPanel1.RowStyles[19].Height = 0;
                panel215ni.Visible = false;
                panel215i.Visible = false;
                s_215i_0.Visible = false;
                s_215i_1.Visible = false;
                s_215i_2.Visible = false;
                s_215i_3.Visible = false;

                //label214n.Text = "База для начисления страховых взносов на обязательное медицинское страхование (210-211-212-213)";
            }

        }

        public void RSW2014_2_1_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            #region Код тарифа DDL
            //            BindingSource b = new BindingSource();
            //            b.DataSource = db.TariffCode;

            this.TariffCodeDDL.DataSource = null;
            this.TariffCodeDDL.Items.Clear();

            if (Options.RaschetPeriodInternal.Any(x => x.Year == formData.Year && x.Kvartal == formData.Quarter))
            {
                DateTime date = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == formData.Year && x.Kvartal == formData.Quarter).DateBegin;

                foreach (var item in db.TariffCode.Where(x => ((x.DateEnd.HasValue && (x.DateBegin.Value <= date && x.DateEnd.Value >= date)) || (!x.DateEnd.HasValue && x.DateBegin <= date))))
                {
                    var name = item.Code + " - " + String.Join(", ", item.TariffCodePlatCat.Select(x => x.PlatCategory.Code).ToArray());
                    this.TariffCodeDDL.Items.Add(new RadListDataItem { Text = name, Value = item.ID });
                }

                this.TariffCodeDDL.ShowItemToolTips = true;
                if (formData.TariffCodeID == null)
                    this.TariffCodeDDL.SelectedIndex = 0;
                else
                {
                    //                long id_ = formData.TariffCodeID.Value;
                    if (TariffCodeDDL.Items.Count() > 0)
                        if (TariffCodeDDL.Items.Any(x => x.Value.ToString() == formData.TariffCodeID.Value.ToString()))
                            this.TariffCodeDDL.Items.FirstOrDefault(x => x.Value.ToString() == formData.TariffCodeID.Value.ToString()).Selected = true;
                }

                this.TariffCodeDDL.SelectedIndexChanged += (s, с) => TariffCodeDDL_SelectedIndexChanged();
            }

            
            #endregion

            getPrevData();  // получаем данные за прошлый период

            List<string> list = new List<string> { };
            for (int i = 0; i <= 3; i++)
            {
                list.Add("s_20" + i.ToString() + "_0");
                list.Add("s_21" + i.ToString() + "_0");
            }

            foreach (var item in list)
            {
     //           ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0]).Enabled = Options.inputTypeRSW1 == 0 ? true : false;
                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0]).Enabled = (Options.inputTypeRSW1 == 0 || Options.inputTypeRSW1 == 2) ? true : false;

            }


            AutoCalcSwitch.IsOn = formData.AutoCalc.HasValue ? formData.AutoCalc.Value : false;


            list = new List<string> { };
            for (int i = 0; i <= 3; i++)
            {
                if (yearType != 2015)
                    list.Add("s_214_" + i.ToString());
                else
                    list.Add("s_213_" + i.ToString());

            }
            foreach (var item in list)
            {
                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0]).Enabled = false;
            }

            list = new List<string> { };
            for (int i = 0; i <= 3; i++)
            {
                if (yearType != 2015)
                    list.Add("s_215_" + i.ToString());
                else
                    list.Add("s_214_" + i.ToString());
            }

            foreach (var item in list)
            {
                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0]).Enabled = AutoCalcSwitch.IsOn;
            }

            switch (action)
            {
                case "add":
                 //   TariffCodeDDL_SelectedIndexChanged();
                    break;
                case "edit":

//                    formData = db.FormsRSW2014_1_Razd_2_1.FirstOrDefault(x => x.ID == formData.ID);

                    updateTextBoxes();
                    
//                    if (db.TariffCode.Any(x => x.ID == formData.TariffCodeID))
//                    {
                        tariffCode = db.TariffCode.FirstOrDefault(x => x.ID == formData.TariffCodeID);
                 //       TariffCodeDDL.Items.First(x => x.Value.ToString() == tariffCode.ID.ToString());
//                    }
                    break;
            }

/*            if (tariffCode != null && tariffCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.Any(x => x.Year == Options.mrot.Year))
            {
                tariffStrah = tariffCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.FirstOrDefault(x => x.Year == Options.mrot.Year).StrahPercant1966.Value;
                tariffOMS = tariffCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.FirstOrDefault(x => x.Year == Options.mrot.Year).FFOMS_Percent.Value;
            }

            Calculation();
 */
            TariffCodeDDL_SelectedIndexChanged();
            this.panel1.Select();
        }


        /// <summary>
        /// Получение данных за прошлый период
        /// </summary>
        private void getPrevData()
        {
            if (Options.inputTypeRSW1 != 0)
            {
                formDataPrev = null;
                byte q = formData.Quarter;
                if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                {
                    long id = 0;
                    if (long.TryParse(this.TariffCodeDDL.SelectedItem.Value.ToString(), out id))
                    {
                        short year = formData.Year;
                        byte quarter = 20;
                        if (q == 6)
                            quarter = 3;
                        else if (q == 9)
                            quarter = 6;
                        else if (q == 0)
                            quarter = 9;

                        if (db.FormsRSW2014_1_Razd_2_1.Any(x => x.Year == year && x.Quarter == quarter && x.TariffCodeID == id && x.InsurerID == Options.InsID))
                            formDataPrev = db.FormsRSW2014_1_Razd_2_1.Where(x => x.Year == year && x.Quarter == quarter && x.TariffCodeID == id && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                    }
                }
                else
                {
                    formDataPrev = null;
                }
            }
            else
                formDataPrev = null;


        }

        public void getValues()
        {
            long id = long.Parse(this.TariffCodeDDL.SelectedItem.Value.ToString());

//            formData.TariffCode = db.TariffCode.FirstOrDefault(x => x.ID == id);
            formData.TariffCodeID = id;
            
            var fields = typeof(FormsRSW2014_1_Razd_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);
            formData.AutoCalc = AutoCalcSwitch.IsOn;

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                       //     DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];



                            string type = formData.GetType().GetProperty(itemName).PropertyType.FullName;
                            type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                            type = type.Split(',')[0].Split('.')[1].ToLower();
                            if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text != "")
                            {
                                switch (type)
                                {
                                    case "decimal":
                                        formData.GetType().GetProperty(itemName).SetValue(formData, Math.Round(decimal.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text), 2, MidpointRounding.AwayFromZero), null);
                                        break;
                                    case "integer":
                                        formData.GetType().GetProperty(itemName).SetValue(formData, int.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString()), null);
                                        break;
                                    case "int64":
                                        formData.GetType().GetProperty(itemName).SetValue(formData, long.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue.ToString()), null);
                                        break;
                                    case "datetime":
                                        formData.GetType().GetProperty(itemName).SetValue(formData, DateTime.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text), null);
                                        break;
                                    case "string":
                                        formData.GetType().GetProperty(itemName).SetValue(formData, ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text, null);
                                        break;
                                }
                            }
                            else
                                formData.GetType().GetProperty(itemName).SetValue(formData, null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message);
                    }
                }

            }

        }

        private void updateTextBoxes()
        {
            var fields = typeof(FormsRSW2014_1_Razd_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
    //                DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                    if (formData != null)
                    {
                        var properties = formData.GetType().GetProperty(itemName);
                        object value = properties.GetValue(formData, null);

                        string type = properties.PropertyType.FullName;
                        if (type.Contains("["))
                            type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                        type = type.Split(',')[0].Split('.')[1].ToLower();
                        
                        //if (value != null)
                        //{
                            switch (type)
                            {
                                case "decimal":
                                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = value != null ? decimal.Parse(value.ToString()) : (decimal)0;
                                    break;
                                case "integer":
                                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = value != null ? int.Parse(value.ToString()) : 0;
                                    break;
                                case "int64":
                                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = value != null ? long.Parse(value.ToString()) : 0;
                                    break;
                                case "datetime":
                                    if (value != null)
                                    {
                                        ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = DateTime.Parse(value.ToString()).Date;
                                    }
                                    break;
                                case "string":
                                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = value != null ? value.ToString() : "";
                                    break;
                            }
                        //}
                        //else
                        //{
                        //    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text = "";
                        //}
                        
                    }

                }

            }

        }


        private void TariffCodeDDL_SelectedIndexChanged()
        {
            if (this.TariffCodeDDL.SelectedItem != null)
            {
                long id = long.Parse(this.TariffCodeDDL.SelectedItem.Value.ToString());
                tariffCode = db.TariffCode.FirstOrDefault(x => x.ID == id);
                formData.TariffCodeID = tariffCode.ID;

                getPrevData();

                if (tariffCode != null && tariffCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.Any(x => x.Year == Options.mrot.Year))
                {
                    tariffStrah = tariffCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.FirstOrDefault(x => x.Year == Options.mrot.Year).StrahPercant1966.Value;
                    tariffOMS = tariffCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.FirstOrDefault(x => x.Year == Options.mrot.Year).FFOMS_Percent.Value;
                }
                else
                {
                    tariffStrah = 0;
                    tariffOMS = 0;
                }
                tariffMore = tariffStrah == 0 ? 0 : ((tariffCode.Code == "01" || tariffCode.Code == "52" || tariffCode.Code == "53") ? 10 : 0);

                Calculation();

//                CalcTextBoxes(null, null);
//                CalcTextBoxesOMS(null, null);

            }
            else
            {
                tariffCode = null;
                formData.TariffCodeID = null;
            }

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                getValues();
                cleanData = false;
                this.Close();
            }
            else
            {
                if (errMessBox.Count != 0)
                {
//                    foreach (var item in errMessBox)
//                    {
                    RadMessageBox.Show(this, errMessBox[0].name);
//                    }
                }

            }
        }

        private void RSW2014_2_1_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (cleanData)
                formData = null;

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Подсчет значений полей
        /// </summary>
        /// 
        public void Calculation()
        {
            CalcTextBoxes(null, null);
            CalcTextBoxesOMS(null, null);
        }

        private void CalcTextBoxes(object sender, EventArgs e)
        {

            if (Options.inputTypeRSW1 != 0)
            {
                if (formDataPrev != null)
                {
                    s_200_0.EditValue = formDataPrev.s_200_0.HasValue ? formDataPrev.s_200_0.Value + decimal.Parse(s_200_1.EditValue.ToString()) + decimal.Parse(s_200_2.EditValue.ToString()) + decimal.Parse(s_200_3.EditValue.ToString()) : decimal.Parse(s_200_1.EditValue.ToString()) + decimal.Parse(s_200_2.EditValue.ToString()) + decimal.Parse(s_200_3.EditValue.ToString());
                    s_201_0.EditValue = formDataPrev.s_201_0.HasValue ? formDataPrev.s_201_0.Value + decimal.Parse(s_201_1.EditValue.ToString()) + decimal.Parse(s_201_2.EditValue.ToString()) + decimal.Parse(s_201_3.EditValue.ToString()) : decimal.Parse(s_201_1.EditValue.ToString()) + decimal.Parse(s_201_2.EditValue.ToString()) + decimal.Parse(s_201_3.EditValue.ToString());
                    s_202_0.EditValue = formDataPrev.s_202_0.HasValue ? formDataPrev.s_202_0.Value + decimal.Parse(s_202_1.EditValue.ToString()) + decimal.Parse(s_202_2.EditValue.ToString()) + decimal.Parse(s_202_3.EditValue.ToString()) : decimal.Parse(s_202_1.EditValue.ToString()) + decimal.Parse(s_202_2.EditValue.ToString()) + decimal.Parse(s_202_3.EditValue.ToString());
                    s_203_0.EditValue = formDataPrev.s_203_0.HasValue ? formDataPrev.s_203_0.Value + decimal.Parse(s_203_1.EditValue.ToString()) + decimal.Parse(s_203_2.EditValue.ToString()) + decimal.Parse(s_203_3.EditValue.ToString()) : decimal.Parse(s_203_1.EditValue.ToString()) + decimal.Parse(s_203_2.EditValue.ToString()) + decimal.Parse(s_203_3.EditValue.ToString());
                }
                else
                {
                    s_200_0.EditValue = decimal.Parse(s_200_1.EditValue.ToString()) + decimal.Parse(s_200_2.EditValue.ToString()) + decimal.Parse(s_200_3.EditValue.ToString());
                    s_201_0.EditValue = decimal.Parse(s_201_1.EditValue.ToString()) + decimal.Parse(s_201_2.EditValue.ToString()) + decimal.Parse(s_201_3.EditValue.ToString());
                    s_202_0.EditValue = decimal.Parse(s_202_1.EditValue.ToString()) + decimal.Parse(s_202_2.EditValue.ToString()) + decimal.Parse(s_202_3.EditValue.ToString());
                    s_203_0.EditValue = decimal.Parse(s_203_1.EditValue.ToString()) + decimal.Parse(s_203_2.EditValue.ToString()) + decimal.Parse(s_203_3.EditValue.ToString());
                }
            }

            s_204_0.EditValue = decimal.Parse(s_200_0.EditValue.ToString()) - decimal.Parse(s_201_0.EditValue.ToString()) - decimal.Parse(s_202_0.EditValue.ToString()) - decimal.Parse(s_203_0.EditValue.ToString());
            s_204_1.EditValue = decimal.Parse(s_200_1.EditValue.ToString()) - decimal.Parse(s_201_1.EditValue.ToString()) - decimal.Parse(s_202_1.EditValue.ToString()) - decimal.Parse(s_203_1.EditValue.ToString());
            s_204_2.EditValue = decimal.Parse(s_200_2.EditValue.ToString()) - decimal.Parse(s_201_2.EditValue.ToString()) - decimal.Parse(s_202_2.EditValue.ToString()) - decimal.Parse(s_203_2.EditValue.ToString());
            s_204_3.EditValue = decimal.Parse(s_200_3.EditValue.ToString()) - decimal.Parse(s_201_3.EditValue.ToString()) - decimal.Parse(s_202_3.EditValue.ToString()) - decimal.Parse(s_203_3.EditValue.ToString());

            if (!AutoCalcSwitch.IsOn)
            {

                s_205_0.EditValue = decimal.Parse(s_204_0.EditValue.ToString()) * tariffStrah / 100;
                s_205_1.EditValue = ((decimal.Parse(s_204_0.EditValue.ToString()) - decimal.Parse(s_204_2.EditValue.ToString()) - decimal.Parse(s_204_3.EditValue.ToString())) * tariffStrah / 100) - ((decimal.Parse(s_204_0.EditValue.ToString()) - decimal.Parse(s_204_1.EditValue.ToString()) - decimal.Parse(s_204_2.EditValue.ToString()) - decimal.Parse(s_204_3.EditValue.ToString())) * tariffStrah / 100);
                s_205_2.EditValue = (((decimal.Parse(s_204_0.EditValue.ToString()) - decimal.Parse(s_204_3.EditValue.ToString())) * tariffStrah / 100) - ((decimal.Parse(s_204_0.EditValue.ToString()) - decimal.Parse(s_204_1.EditValue.ToString()) - decimal.Parse(s_204_2.EditValue.ToString()) - decimal.Parse(s_204_3.EditValue.ToString())) * tariffStrah / 100)) - decimal.Parse(s_205_1.EditValue.ToString());
                s_205_3.EditValue = (((decimal.Parse(s_204_0.EditValue.ToString())) * tariffStrah / 100) - ((decimal.Parse(s_204_0.EditValue.ToString()) - decimal.Parse(s_204_1.EditValue.ToString()) - decimal.Parse(s_204_2.EditValue.ToString()) - decimal.Parse(s_204_3.EditValue.ToString())) * tariffStrah / 100)) - (decimal.Parse(s_205_1.EditValue.ToString()) + decimal.Parse(s_205_2.EditValue.ToString()));

                //tariffMore = tariffStrah != 0 ? 10 : 0;

                s_206_0.EditValue = decimal.Parse(s_203_0.EditValue.ToString()) * tariffMore / 100;
                s_206_1.EditValue = ((decimal.Parse(s_203_0.EditValue.ToString()) - decimal.Parse(s_203_2.EditValue.ToString()) - decimal.Parse(s_203_3.EditValue.ToString())) * tariffMore / 100) - ((decimal.Parse(s_203_0.EditValue.ToString()) - decimal.Parse(s_203_1.EditValue.ToString()) - decimal.Parse(s_203_2.EditValue.ToString()) - decimal.Parse(s_203_3.EditValue.ToString())) * tariffMore / 100);
                s_206_2.EditValue = (((decimal.Parse(s_203_0.EditValue.ToString()) - decimal.Parse(s_203_3.EditValue.ToString())) * tariffMore / 100) - ((decimal.Parse(s_203_0.EditValue.ToString()) - decimal.Parse(s_203_1.EditValue.ToString()) - decimal.Parse(s_203_2.EditValue.ToString()) - decimal.Parse(s_203_3.EditValue.ToString())) * tariffMore / 100)) - decimal.Parse(s_206_1.EditValue.ToString());
                s_206_3.EditValue = (((decimal.Parse(s_203_0.EditValue.ToString())) * tariffMore / 100) - ((decimal.Parse(s_203_0.EditValue.ToString()) - decimal.Parse(s_203_1.EditValue.ToString()) - decimal.Parse(s_203_2.EditValue.ToString()) - decimal.Parse(s_203_3.EditValue.ToString())) * tariffMore / 100)) - (decimal.Parse(s_206_1.EditValue.ToString()) + decimal.Parse(s_206_2.EditValue.ToString()));
            }

        }

        private void CalcTextBoxesOMS(object sender, EventArgs e)
        {
            if (yearType != 2015)
            {
                if (Options.inputTypeRSW1 != 0)
                {
                    if (formDataPrev != null)
                    {
                        s_210_0.EditValue = formDataPrev.s_210_0.HasValue ? formDataPrev.s_210_0.Value + decimal.Parse(s_210_1.EditValue.ToString()) + decimal.Parse(s_210_2.EditValue.ToString()) + decimal.Parse(s_210_3.EditValue.ToString()) : decimal.Parse(s_210_1.EditValue.ToString()) + decimal.Parse(s_210_2.EditValue.ToString()) + decimal.Parse(s_210_3.EditValue.ToString());
                        s_211_0.EditValue = formDataPrev.s_211_0.HasValue ? formDataPrev.s_211_0.Value + decimal.Parse(s_211_1.EditValue.ToString()) + decimal.Parse(s_211_2.EditValue.ToString()) + decimal.Parse(s_211_3.EditValue.ToString()) : decimal.Parse(s_211_1.EditValue.ToString()) + decimal.Parse(s_211_2.EditValue.ToString()) + decimal.Parse(s_211_3.EditValue.ToString());
                        s_212_0.EditValue = formDataPrev.s_212_0.HasValue ? formDataPrev.s_212_0.Value + decimal.Parse(s_212_1.EditValue.ToString()) + decimal.Parse(s_212_2.EditValue.ToString()) + decimal.Parse(s_212_3.EditValue.ToString()) : decimal.Parse(s_212_1.EditValue.ToString()) + decimal.Parse(s_212_2.EditValue.ToString()) + decimal.Parse(s_212_3.EditValue.ToString());
                        s_213_0.EditValue = formDataPrev.s_213_0.HasValue ? formDataPrev.s_213_0.Value + decimal.Parse(s_213_1.EditValue.ToString()) + decimal.Parse(s_213_2.EditValue.ToString()) + decimal.Parse(s_213_3.EditValue.ToString()) : decimal.Parse(s_213_1.EditValue.ToString()) + decimal.Parse(s_213_2.EditValue.ToString()) + decimal.Parse(s_213_3.EditValue.ToString());
                    }
                    else
                    {
                        s_210_0.EditValue = decimal.Parse(s_210_1.EditValue.ToString()) + decimal.Parse(s_210_2.EditValue.ToString()) + decimal.Parse(s_210_3.EditValue.ToString());
                        s_211_0.EditValue = decimal.Parse(s_211_1.EditValue.ToString()) + decimal.Parse(s_211_2.EditValue.ToString()) + decimal.Parse(s_211_3.EditValue.ToString());
                        s_212_0.EditValue = decimal.Parse(s_212_1.EditValue.ToString()) + decimal.Parse(s_212_2.EditValue.ToString()) + decimal.Parse(s_212_3.EditValue.ToString());
                        s_213_0.EditValue = decimal.Parse(s_213_1.EditValue.ToString()) + decimal.Parse(s_213_2.EditValue.ToString()) + decimal.Parse(s_213_3.EditValue.ToString());
                    }
                }


                s_214_0.EditValue = decimal.Parse(s_210_0.EditValue.ToString()) - decimal.Parse(s_211_0.EditValue.ToString()) - decimal.Parse(s_212_0.EditValue.ToString()) - decimal.Parse(s_213_0.EditValue.ToString());
                s_214_1.EditValue = decimal.Parse(s_210_1.EditValue.ToString()) - decimal.Parse(s_211_1.EditValue.ToString()) - decimal.Parse(s_212_1.EditValue.ToString()) - decimal.Parse(s_213_1.EditValue.ToString());
                s_214_2.EditValue = decimal.Parse(s_210_2.EditValue.ToString()) - decimal.Parse(s_211_2.EditValue.ToString()) - decimal.Parse(s_212_2.EditValue.ToString()) - decimal.Parse(s_213_2.EditValue.ToString());
                s_214_3.EditValue = decimal.Parse(s_210_3.EditValue.ToString()) - decimal.Parse(s_211_3.EditValue.ToString()) - decimal.Parse(s_212_3.EditValue.ToString()) - decimal.Parse(s_213_3.EditValue.ToString());

                if (!AutoCalcSwitch.IsOn)
                {
                    s_215_0.EditValue = decimal.Parse(s_214_0.EditValue.ToString()) * tariffOMS / 100;
                    s_215_1.EditValue = ((decimal.Parse(s_214_0.EditValue.ToString()) - decimal.Parse(s_214_2.EditValue.ToString()) - decimal.Parse(s_214_3.EditValue.ToString())) * tariffOMS / 100) - ((decimal.Parse(s_214_0.EditValue.ToString()) - decimal.Parse(s_214_1.EditValue.ToString()) - decimal.Parse(s_214_2.EditValue.ToString()) - decimal.Parse(s_214_3.EditValue.ToString())) * tariffOMS / 100);
                    s_215_2.EditValue = (((decimal.Parse(s_214_0.EditValue.ToString()) - decimal.Parse(s_214_3.EditValue.ToString())) * tariffOMS / 100) - ((decimal.Parse(s_214_0.EditValue.ToString()) - decimal.Parse(s_214_1.EditValue.ToString()) - decimal.Parse(s_214_2.EditValue.ToString()) - decimal.Parse(s_214_3.EditValue.ToString())) * tariffOMS / 100)) - decimal.Parse(s_215_1.EditValue.ToString());
                    s_215_3.EditValue = (((decimal.Parse(s_214_0.EditValue.ToString())) * tariffOMS / 100) - ((decimal.Parse(s_214_0.EditValue.ToString()) - decimal.Parse(s_214_1.EditValue.ToString()) - decimal.Parse(s_214_2.EditValue.ToString()) - decimal.Parse(s_214_3.EditValue.ToString())) * tariffOMS / 100)) - (decimal.Parse(s_215_1.EditValue.ToString()) + decimal.Parse(s_215_2.EditValue.ToString()));

                    //s_215_0.EditValue = decimal.Parse(s_215_1.EditValue.ToString()) + decimal.Parse(s_215_2.EditValue.ToString()) + decimal.Parse(s_215_3.EditValue.ToString());
                    /*                s_215_0.EditValue = decimal.Parse(s_214_0.EditValue.ToString()) * tariffOMS / 100;
                                    s_215_1.EditValue = decimal.Parse(s_214_1.EditValue.ToString()) * tariffOMS / 100;
                                    s_215_2.EditValue = decimal.Parse(s_214_2.EditValue.ToString()) * tariffOMS / 100;
                                    s_215_3.EditValue = decimal.Parse(s_214_3.EditValue.ToString()) * tariffOMS / 100;
                      */
                }
            }
            else // для 2015 года
            {
                if (Options.inputTypeRSW1 != 0)
                {
                    if (formDataPrev != null)
                    {
                        s_210_0.EditValue = formDataPrev.s_210_0.HasValue ? formDataPrev.s_210_0.Value + decimal.Parse(s_210_1.EditValue.ToString()) + decimal.Parse(s_210_2.EditValue.ToString()) + decimal.Parse(s_210_3.EditValue.ToString()) : decimal.Parse(s_210_1.EditValue.ToString()) + decimal.Parse(s_210_2.EditValue.ToString()) + decimal.Parse(s_210_3.EditValue.ToString());
                        s_211_0.EditValue = formDataPrev.s_211_0.HasValue ? formDataPrev.s_211_0.Value + decimal.Parse(s_211_1.EditValue.ToString()) + decimal.Parse(s_211_2.EditValue.ToString()) + decimal.Parse(s_211_3.EditValue.ToString()) : decimal.Parse(s_211_1.EditValue.ToString()) + decimal.Parse(s_211_2.EditValue.ToString()) + decimal.Parse(s_211_3.EditValue.ToString());
                        s_212_0.EditValue = formDataPrev.s_212_0.HasValue ? formDataPrev.s_212_0.Value + decimal.Parse(s_212_1.EditValue.ToString()) + decimal.Parse(s_212_2.EditValue.ToString()) + decimal.Parse(s_212_3.EditValue.ToString()) : decimal.Parse(s_212_1.EditValue.ToString()) + decimal.Parse(s_212_2.EditValue.ToString()) + decimal.Parse(s_212_3.EditValue.ToString());
                    }
                    else
                    {
                        s_210_0.EditValue = decimal.Parse(s_210_1.EditValue.ToString()) + decimal.Parse(s_210_2.EditValue.ToString()) + decimal.Parse(s_210_3.EditValue.ToString());
                        s_211_0.EditValue = decimal.Parse(s_211_1.EditValue.ToString()) + decimal.Parse(s_211_2.EditValue.ToString()) + decimal.Parse(s_211_3.EditValue.ToString());
                        s_212_0.EditValue = decimal.Parse(s_212_1.EditValue.ToString()) + decimal.Parse(s_212_2.EditValue.ToString()) + decimal.Parse(s_212_3.EditValue.ToString());
                    }
                }


                s_213_0.EditValue = decimal.Parse(s_210_0.EditValue.ToString()) - decimal.Parse(s_211_0.EditValue.ToString()) - decimal.Parse(s_212_0.EditValue.ToString());
                s_213_1.EditValue = decimal.Parse(s_210_1.EditValue.ToString()) - decimal.Parse(s_211_1.EditValue.ToString()) - decimal.Parse(s_212_1.EditValue.ToString());
                s_213_2.EditValue = decimal.Parse(s_210_2.EditValue.ToString()) - decimal.Parse(s_211_2.EditValue.ToString()) - decimal.Parse(s_212_2.EditValue.ToString());
                s_213_3.EditValue = decimal.Parse(s_210_3.EditValue.ToString()) - decimal.Parse(s_211_3.EditValue.ToString()) - decimal.Parse(s_212_3.EditValue.ToString());

                if (!AutoCalcSwitch.IsOn)
                {
                    s_214_0.EditValue = decimal.Parse(s_213_0.EditValue.ToString()) * tariffOMS / 100;
                    s_214_1.EditValue = ((decimal.Parse(s_213_0.EditValue.ToString()) - decimal.Parse(s_213_2.EditValue.ToString()) - decimal.Parse(s_213_3.EditValue.ToString())) * tariffOMS / 100) - ((decimal.Parse(s_213_0.EditValue.ToString()) - decimal.Parse(s_213_1.EditValue.ToString()) - decimal.Parse(s_213_2.EditValue.ToString()) - decimal.Parse(s_213_3.EditValue.ToString())) * tariffOMS / 100);
                    s_214_2.EditValue = (((decimal.Parse(s_213_0.EditValue.ToString()) - decimal.Parse(s_213_3.EditValue.ToString())) * tariffOMS / 100) - ((decimal.Parse(s_213_0.EditValue.ToString()) - decimal.Parse(s_213_1.EditValue.ToString()) - decimal.Parse(s_213_2.EditValue.ToString()) - decimal.Parse(s_213_3.EditValue.ToString())) * tariffOMS / 100)) - decimal.Parse(s_214_1.EditValue.ToString());
                    s_214_3.EditValue = (((decimal.Parse(s_213_0.EditValue.ToString())) * tariffOMS / 100) - ((decimal.Parse(s_213_0.EditValue.ToString()) - decimal.Parse(s_213_1.EditValue.ToString()) - decimal.Parse(s_213_2.EditValue.ToString()) - decimal.Parse(s_213_3.EditValue.ToString())) * tariffOMS / 100)) - (decimal.Parse(s_214_1.EditValue.ToString()) + decimal.Parse(s_214_2.EditValue.ToString()));


                    //s_214_0.EditValue = decimal.Parse(s_214_1.EditValue.ToString()) + decimal.Parse(s_214_2.EditValue.ToString()) + decimal.Parse(s_214_3.EditValue.ToString());

                }
            }


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
      //              return;
                    //do something else
                }
            }
            

            List<string> list = new List<string> { };
            for (int i = 0; i <= 3; i++)
            {
                list.Add("s_205_" + i.ToString());
                list.Add("s_206_" + i.ToString());
                if (yearType != 2015)
                    list.Add("s_215_" + i.ToString());
                else
                    list.Add("s_214_" + i.ToString());

            }

            foreach (var item in list)
            {
           //     DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0];
                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0]).Enabled = AutoCalcSwitch.IsOn;
            }
        }


        /// <summary>
        /// Проверка введенных данных Раздела 2.1
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();
            if (tariffCode == null)
            {
                errMessBox.Add(new ErrList { name = "Необходимо выбрать Код тарифа", control = "TariffCodeDDL" });
            }

            #region Проверяем на уникальность по коду тарифа
            RSW2014_Edit main = this.Owner as RSW2014_Edit;
            List<FormsRSW2014_1_Razd_2_1> RSW_2_1_List = main.RSW_2_1_List;

            switch (action)
            {
                case "add":
                    if (RSW_2_1_List.Any(x => x.TariffCodeID == formData.TariffCodeID))
                    {
                        errMessBox.Add(new ErrList { name = "на двух подразделах раздела 2.1 не может быть указан  одинаковый код тарифа", control = "TariffCodeDDL" });
                    }
                    break;
                case "edit":
                    if (RSW_2_1_List.Count(x => x.TariffCodeID == formData.TariffCodeID) > 1)
                    {
                        errMessBox.Add(new ErrList { name = "на двух подразделах раздела 2.1 не может быть указан  одинаковый код тарифа", control = "TariffCodeDDL" });
                    }
                    break;
            }

            #endregion

            #region Раздел 2.1   30
            if ((decimal.Parse(s_205_0.EditValue.ToString()) + decimal.Parse(s_206_0.EditValue.ToString())) > 0)
                if (long.Parse(s_207_0.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.3 стр.207 должна иметь ненулевое значение при ненулевой сумме значений гр.3 стр.205-206", control = "s_207_0" });
            #endregion
            #region Раздел 2.1   31
            if ((decimal.Parse(s_205_1.EditValue.ToString()) + decimal.Parse(s_206_1.EditValue.ToString())) > 0)
                if (long.Parse(s_207_1.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.4 стр.207 должна иметь ненулевое значение при ненулевой сумме значений гр.4 стр.205-206", control = "s_207_1" });
            #endregion
            #region Раздел 2.1   32
            if ((decimal.Parse(s_205_2.EditValue.ToString()) + decimal.Parse(s_206_2.EditValue.ToString())) > 0)
                if (long.Parse(s_207_2.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.5 стр.207 должна иметь ненулевое значение при ненулевой сумме значений гр.5 стр.205-206", control = "s_207_2" });
            #endregion
            #region Раздел 2.1   33
            if ((decimal.Parse(s_205_3.EditValue.ToString()) + decimal.Parse(s_206_3.EditValue.ToString())) > 0)
                if (long.Parse(s_207_3.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.6 стр.207 должна иметь ненулевое значение при ненулевой сумме значений гр.6 стр.205-206", control = "s_207_3" });
            #endregion

            #region Раздел 2.1   35
            if (decimal.Parse(s_203_0.EditValue.ToString()) > 0)
                if (long.Parse(s_208_0.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.3 стр.208 должна иметь ненулевое значение при ненулевой сумме значений гр.3 стр.203", control = "s_208_0" });
            #endregion
            #region Раздел 2.1   36
            if (decimal.Parse(s_203_1.EditValue.ToString()) > 0)
                if (long.Parse(s_208_1.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.4 стр.208 должна иметь ненулевое значение при ненулевой сумме значений гр.4 стр.203", control = "s_208_1" });
            #endregion
            #region Раздел 2.1   37
            if (decimal.Parse(s_203_2.EditValue.ToString()) > 0)
                if (long.Parse(s_208_2.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.5 стр.208 должна иметь ненулевое значение при ненулевой сумме значений гр.5 стр.203", control = "s_208_2" });
            #endregion
            #region Раздел 2.1   38
            if (decimal.Parse(s_203_3.EditValue.ToString()) > 0)
                if (long.Parse(s_208_3.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.6 стр.208 должна иметь ненулевое значение при ненулевой сумме значений гр.6 стр.203", control = "s_208_3" });
            #endregion

            #region Раздел 2.1   39
            if ((long.Parse(s_208_0.EditValue.ToString()) < (long.Parse(s_208_1.EditValue.ToString()))) && (long.Parse(s_208_0.EditValue.ToString()) < (long.Parse(s_208_2.EditValue.ToString()))) && (long.Parse(s_208_0.EditValue.ToString()) < (long.Parse(s_208_3.EditValue.ToString()))))
                errMessBox.Add(new ErrList { name = "Значение гр.3 стр.208 должно быть не меньше значений граф 4, или 5, или 6", control = "s_208_0" });
            #endregion

            #region Раздел 2.1   40
            if (formDataPrev != null)
                if (formDataPrev.s_208_0.HasValue)
                    if (long.Parse(s_208_0.EditValue.ToString()) < formDataPrev.s_208_0.Value)
                    errMessBox.Add(new ErrList { name = "Значение гр.3 стр.208 должно быть не меньше значения за предыдущий период по тому же тарифу расчета", control = "s_208_0" });
            #endregion
            #region Раздел 2.1   41
            if ((long.Parse(s_208_0.EditValue.ToString()) > (long.Parse(s_207_0.EditValue.ToString()))) || (long.Parse(s_208_1.EditValue.ToString()) > (long.Parse(s_207_1.EditValue.ToString()))) || (long.Parse(s_208_2.EditValue.ToString()) > (long.Parse(s_207_2.EditValue.ToString()))) || (long.Parse(s_208_3.EditValue.ToString()) > (long.Parse(s_207_3.EditValue.ToString()))))
                errMessBox.Add(new ErrList { name = "Значения в строке 208 меньше либо равны соответствующим значениям строки 207", control = "s_208_0" });
            #endregion

            //#region Раздел 2.1   13
            //if ((decimal.Parse(s_210_0.EditValue.ToString()) > (decimal.Parse(s_200_0.EditValue.ToString()))) || (decimal.Parse(s_210_1.EditValue.ToString()) > (decimal.Parse(s_200_1.EditValue.ToString()))) || (decimal.Parse(s_210_2.EditValue.ToString()) > (decimal.Parse(s_200_2.EditValue.ToString()))) || (decimal.Parse(s_210_3.EditValue.ToString()) > (decimal.Parse(s_200_3.EditValue.ToString()))))
            //    errMessBox.Add(new ErrList { name = "Значения в строке 200 должны быть меньше либо равны соответствующим значениям строки 210", control = "s_208_0" });
            //#endregion
            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            s_210_1.EditValue = s_200_1.EditValue;
            s_210_2.EditValue = s_200_2.EditValue;
            s_210_3.EditValue = s_200_3.EditValue;
            CalcTextBoxesOMS(null,null);
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            s_211_1.EditValue = s_201_1.EditValue;
            s_211_2.EditValue = s_201_2.EditValue;
            s_211_3.EditValue = s_201_3.EditValue;
            CalcTextBoxesOMS(null, null);
        }



    }
}
