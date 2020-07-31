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
    public partial class RSW2014_2_4_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool cleanData = true;

        private List<ErrList> errMessBox = new List<ErrList>();

        public FormsRSW2014_1_Razd_2_4 formData { get; set; }
        public FormsRSW2014_1_Razd_2_4 formDataPrev { get; set; }
        public IEnumerable<SpecOcenkaUslTruda> SpecOcenka { get; set; }

        public RSW2014_2_4_Edit()
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

        private void RSW2014_2_4_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            SpecOcenka = db.SpecOcenkaUslTruda;
            FilledBaseDDL.DataSource = null;
            FilledBaseDDL.Items.Clear();
            foreach (var item in Options.FilledBaseArr)
            {
                this.FilledBaseDDL.Items.Add(new RadListDataItem { Text = item.name, Value = item.id });
            }

            this.FilledBaseDDL.ShowItemToolTips = true;
            this.FilledBaseDDL.SelectedIndex = 0;
            this.FilledBaseDDL.SelectedIndexChanged += (s, с) => FilledBaseDDL_SelectedIndexChanged();

            getPrevData();

            List<string> list = new List<string> { };
            list.Add("s_240_0");
            list.Add("s_241_0");
            list.Add("s_246_0");
            list.Add("s_247_0");
            list.Add("s_252_0");
            list.Add("s_253_0");
            list.Add("s_258_0");
            list.Add("s_259_0");
            list.Add("s_264_0");
            list.Add("s_265_0");

            foreach (var item in list)
            {
            //    DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0];
                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0]).Enabled = (Options.inputTypeRSW1 == 0 || Options.inputTypeRSW1 == 2) ? true : false;
            }

            AutoCalcSwitch.IsOn = formData.AutoCalc.HasValue ? formData.AutoCalc.Value : false;
            switch (action)
            {
                case "add":



                    FilledBaseDDL_SelectedIndexChanged();
                    break;
                case "edit":
                    updateTextBoxes();
                    break;
            }


            this.CodeBaseSpin.ValueChanged += (s, с) => CodeBaseSpin_ValueChanged();

            this.panel1.Select();
        }

        private void CodeBaseSpin_ValueChanged(object sender, EventArgs e)
        {
            getPrevData();
            Calculation();
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
                    short year = formData.Year;
                    byte quarter = 20;
                    if (q == 6)
                        quarter = 3;
                    else if (q == 9)
                        quarter = 6;
                    else if (q == 0)
                        quarter = 9;

                    byte codeBase = (byte)CodeBaseSpin.Value;
                    byte filledBase = byte.Parse(FilledBaseDDL.SelectedItem.Value.ToString());

                    if (db.FormsRSW2014_1_Razd_2_4.Any(x => x.Year == year && x.Quarter == quarter && x.CodeBase == codeBase && x.FilledBase == filledBase && x.InsurerID == Options.InsID))
                        formDataPrev = db.FormsRSW2014_1_Razd_2_4.Where(x => x.Year == year && x.Quarter == quarter && x.CodeBase == codeBase && x.FilledBase == filledBase && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                }
                else
                {
                    formDataPrev = null;
                }
            }
            else
            {
                formDataPrev = null;
            }


        }

        private void FilledBaseDDL_SelectedIndexChanged()
        {
            if (this.FilledBaseDDL.SelectedItem != null)
            {
                //long id = long.Parse(this.FilledBaseDDL.SelectedItem.Value.ToString());
                getPrevData();
                Calculation();
            }
            else
            {

            }

        }

        private void getValues()
        {
            formData.CodeBase = (byte)CodeBaseSpin.Value;
            formData.FilledBase = byte.Parse(FilledBaseDDL.SelectedItem.Value.ToString());
            formData.AutoCalc = AutoCalcSwitch.IsOn;

            var fields = typeof(FormsRSW2014_1_Razd_2_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
            CodeBaseSpin.Value = formData.CodeBase.Value;
            FilledBaseDDL.Items.FirstOrDefault(x => byte.Parse(x.Value.ToString()) == formData.FilledBase.Value).Selected = true;

            var fields = typeof(FormsRSW2014_1_Razd_2_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
            //        DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

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

        private void RSW2014_2_4_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (cleanData)
                formData = null;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            this.Close();
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

        /// <summary>
        /// Подсчет значений полей
        /// </summary>
        /// 
        private void Calculation()
        {
            CalcTextBoxesO4(null, null);
            CalcTextBoxesB34(null, null);
            CalcTextBoxesB33(null, null);
            CalcTextBoxesB32(null, null);
            CalcTextBoxesB31(null, null);
        }

        private void CalcTextBoxesO4(object sender, EventArgs e)
        {
            if (Options.inputTypeRSW1 != 0)
            {
                if (formDataPrev != null)
                {
                    s_240_0.EditValue = formDataPrev.s_240_0.HasValue ? formDataPrev.s_240_0.Value + decimal.Parse(s_240_1.EditValue.ToString()) + decimal.Parse(s_240_2.EditValue.ToString()) + decimal.Parse(s_240_3.EditValue.ToString()) : decimal.Parse(s_240_1.EditValue.ToString()) + decimal.Parse(s_240_2.EditValue.ToString()) + decimal.Parse(s_240_3.EditValue.ToString());
                    s_241_0.EditValue = formDataPrev.s_241_0.HasValue ? formDataPrev.s_241_0.Value + decimal.Parse(s_241_1.EditValue.ToString()) + decimal.Parse(s_241_2.EditValue.ToString()) + decimal.Parse(s_241_3.EditValue.ToString()) : decimal.Parse(s_241_1.EditValue.ToString()) + decimal.Parse(s_241_2.EditValue.ToString()) + decimal.Parse(s_241_3.EditValue.ToString());
                }
                else
                {
                    s_240_0.EditValue = decimal.Parse(s_240_1.EditValue.ToString()) + decimal.Parse(s_240_2.EditValue.ToString()) + decimal.Parse(s_240_3.EditValue.ToString());
                    s_241_0.EditValue = decimal.Parse(s_241_1.EditValue.ToString()) + decimal.Parse(s_241_2.EditValue.ToString()) + decimal.Parse(s_241_3.EditValue.ToString());
                }
            }

            s_243_0.EditValue = decimal.Parse(s_240_0.EditValue.ToString()) - decimal.Parse(s_241_0.EditValue.ToString());
            s_243_1.EditValue = decimal.Parse(s_240_1.EditValue.ToString()) - decimal.Parse(s_241_1.EditValue.ToString());
            s_243_2.EditValue = decimal.Parse(s_240_2.EditValue.ToString()) - decimal.Parse(s_241_2.EditValue.ToString());
            s_243_3.EditValue = decimal.Parse(s_240_3.EditValue.ToString()) - decimal.Parse(s_241_3.EditValue.ToString());

            if (!AutoCalcSwitch.IsOn)
            {
                byte Type_ = FilledBaseDDL.SelectedIndex == 0 ? (byte)0 : (byte)1;
                string Code = "О4";
                DateTime Date = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == formData.Year && x.Kvartal == formData.Quarter).DateBegin.Date;

                decimal tariff = SpecOcenka.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;

                s_244_0.EditValue = decimal.Parse(s_243_0.EditValue.ToString()) * tariff / 100;
                s_244_1.EditValue = ((decimal.Parse(s_243_0.EditValue.ToString()) - decimal.Parse(s_243_2.EditValue.ToString()) - decimal.Parse(s_243_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_243_0.EditValue.ToString()) - decimal.Parse(s_243_1.EditValue.ToString()) - decimal.Parse(s_243_2.EditValue.ToString()) - decimal.Parse(s_243_3.EditValue.ToString())) * tariff / 100);
                s_244_2.EditValue = (((decimal.Parse(s_243_0.EditValue.ToString()) - decimal.Parse(s_243_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_243_0.EditValue.ToString()) - decimal.Parse(s_243_1.EditValue.ToString()) - decimal.Parse(s_243_2.EditValue.ToString()) - decimal.Parse(s_243_3.EditValue.ToString())) * tariff / 100)) - decimal.Parse(s_244_1.EditValue.ToString());
                s_244_3.EditValue = (((decimal.Parse(s_243_0.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_243_0.EditValue.ToString()) - decimal.Parse(s_243_1.EditValue.ToString()) - decimal.Parse(s_243_2.EditValue.ToString()) - decimal.Parse(s_243_3.EditValue.ToString())) * tariff / 100)) - (decimal.Parse(s_244_1.EditValue.ToString()) + decimal.Parse(s_244_2.EditValue.ToString()));
            }

        }

        private void CalcTextBoxesB34(object sender, EventArgs e)
        {
            if (Options.inputTypeRSW1 != 0)
            {
                if (formDataPrev != null)
                {
                    s_246_0.EditValue = formDataPrev.s_246_0.HasValue ? formDataPrev.s_246_0.Value + decimal.Parse(s_246_1.EditValue.ToString()) + decimal.Parse(s_246_2.EditValue.ToString()) + decimal.Parse(s_246_3.EditValue.ToString()) : decimal.Parse(s_246_1.EditValue.ToString()) + decimal.Parse(s_246_2.EditValue.ToString()) + decimal.Parse(s_246_3.EditValue.ToString());
                    s_247_0.EditValue = formDataPrev.s_247_0.HasValue ? formDataPrev.s_247_0.Value + decimal.Parse(s_247_1.EditValue.ToString()) + decimal.Parse(s_247_2.EditValue.ToString()) + decimal.Parse(s_247_3.EditValue.ToString()) : decimal.Parse(s_247_1.EditValue.ToString()) + decimal.Parse(s_247_2.EditValue.ToString()) + decimal.Parse(s_247_3.EditValue.ToString());
                }
                else
                {
                    s_246_0.EditValue = decimal.Parse(s_246_1.EditValue.ToString()) + decimal.Parse(s_246_2.EditValue.ToString()) + decimal.Parse(s_246_3.EditValue.ToString());
                    s_247_0.EditValue = decimal.Parse(s_247_1.EditValue.ToString()) + decimal.Parse(s_247_2.EditValue.ToString()) + decimal.Parse(s_247_3.EditValue.ToString());
                }
            }


            s_249_0.EditValue = decimal.Parse(s_246_0.EditValue.ToString()) - decimal.Parse(s_247_0.EditValue.ToString());
            s_249_1.EditValue = decimal.Parse(s_246_1.EditValue.ToString()) - decimal.Parse(s_247_1.EditValue.ToString());
            s_249_2.EditValue = decimal.Parse(s_246_2.EditValue.ToString()) - decimal.Parse(s_247_2.EditValue.ToString());
            s_249_3.EditValue = decimal.Parse(s_246_3.EditValue.ToString()) - decimal.Parse(s_247_3.EditValue.ToString());

            if (!AutoCalcSwitch.IsOn)
            {
                byte Type_ = FilledBaseDDL.SelectedIndex == 0 ? (byte)0 : (byte)1;
                string Code = "В3.4";
                DateTime Date = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == formData.Year && x.Kvartal == formData.Quarter).DateBegin.Date;

                decimal tariff = SpecOcenka.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;


                s_250_0.EditValue = decimal.Parse(s_249_0.EditValue.ToString()) * tariff / 100;
                s_250_1.EditValue = ((decimal.Parse(s_249_0.EditValue.ToString()) - decimal.Parse(s_249_2.EditValue.ToString()) - decimal.Parse(s_249_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_249_0.EditValue.ToString()) - decimal.Parse(s_249_1.EditValue.ToString()) - decimal.Parse(s_249_2.EditValue.ToString()) - decimal.Parse(s_249_3.EditValue.ToString())) * tariff / 100);
                s_250_2.EditValue = (((decimal.Parse(s_249_0.EditValue.ToString()) - decimal.Parse(s_249_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_249_0.EditValue.ToString()) - decimal.Parse(s_249_1.EditValue.ToString()) - decimal.Parse(s_249_2.EditValue.ToString()) - decimal.Parse(s_249_3.EditValue.ToString())) * tariff / 100)) - decimal.Parse(s_250_1.EditValue.ToString());
                s_250_3.EditValue = (((decimal.Parse(s_249_0.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_249_0.EditValue.ToString()) - decimal.Parse(s_249_1.EditValue.ToString()) - decimal.Parse(s_249_2.EditValue.ToString()) - decimal.Parse(s_249_3.EditValue.ToString())) * tariff / 100)) - (decimal.Parse(s_250_1.EditValue.ToString()) + decimal.Parse(s_250_2.EditValue.ToString()));
            }

        }

        private void CalcTextBoxesB33(object sender, EventArgs e)
        {
            if (Options.inputTypeRSW1 != 0)
            {
                if (formDataPrev != null)
                {
                    s_252_0.EditValue = formDataPrev.s_252_0.HasValue ? formDataPrev.s_252_0.Value + decimal.Parse(s_252_1.EditValue.ToString()) + decimal.Parse(s_252_2.EditValue.ToString()) + decimal.Parse(s_252_3.EditValue.ToString()) : decimal.Parse(s_252_1.EditValue.ToString()) + decimal.Parse(s_252_2.EditValue.ToString()) + decimal.Parse(s_252_3.EditValue.ToString());
                    s_253_0.EditValue = formDataPrev.s_253_0.HasValue ? formDataPrev.s_253_0.Value + decimal.Parse(s_253_1.EditValue.ToString()) + decimal.Parse(s_253_2.EditValue.ToString()) + decimal.Parse(s_253_3.EditValue.ToString()) : decimal.Parse(s_253_1.EditValue.ToString()) + decimal.Parse(s_253_2.EditValue.ToString()) + decimal.Parse(s_253_3.EditValue.ToString());
                }
                else
                {
                    s_252_0.EditValue = decimal.Parse(s_252_1.EditValue.ToString()) + decimal.Parse(s_252_2.EditValue.ToString()) + decimal.Parse(s_252_3.EditValue.ToString());
                    s_253_0.EditValue = decimal.Parse(s_253_1.EditValue.ToString()) + decimal.Parse(s_253_2.EditValue.ToString()) + decimal.Parse(s_253_3.EditValue.ToString());
                }
            }


            s_255_0.EditValue = decimal.Parse(s_252_0.EditValue.ToString()) - decimal.Parse(s_253_0.EditValue.ToString());
            s_255_1.EditValue = decimal.Parse(s_252_1.EditValue.ToString()) - decimal.Parse(s_253_1.EditValue.ToString());
            s_255_2.EditValue = decimal.Parse(s_252_2.EditValue.ToString()) - decimal.Parse(s_253_2.EditValue.ToString());
            s_255_3.EditValue = decimal.Parse(s_252_3.EditValue.ToString()) - decimal.Parse(s_253_3.EditValue.ToString());

            if (!AutoCalcSwitch.IsOn)
            {
                byte Type_ = FilledBaseDDL.SelectedIndex == 0 ? (byte)0 : (byte)1;
                string Code = "В3.3";
                DateTime Date = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == formData.Year && x.Kvartal == formData.Quarter).DateBegin.Date;

                decimal tariff = SpecOcenka.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;

                s_256_0.EditValue = decimal.Parse(s_255_0.EditValue.ToString()) * tariff / 100;
                s_256_1.EditValue = ((decimal.Parse(s_255_0.EditValue.ToString()) - decimal.Parse(s_255_2.EditValue.ToString()) - decimal.Parse(s_255_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_255_0.EditValue.ToString()) - decimal.Parse(s_255_1.EditValue.ToString()) - decimal.Parse(s_255_2.EditValue.ToString()) - decimal.Parse(s_255_3.EditValue.ToString())) * tariff / 100);
                s_256_2.EditValue = (((decimal.Parse(s_255_0.EditValue.ToString()) - decimal.Parse(s_255_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_255_0.EditValue.ToString()) - decimal.Parse(s_255_1.EditValue.ToString()) - decimal.Parse(s_255_2.EditValue.ToString()) - decimal.Parse(s_255_3.EditValue.ToString())) * tariff / 100)) - decimal.Parse(s_256_1.EditValue.ToString());
                s_256_3.EditValue = (((decimal.Parse(s_255_0.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_255_0.EditValue.ToString()) - decimal.Parse(s_255_1.EditValue.ToString()) - decimal.Parse(s_255_2.EditValue.ToString()) - decimal.Parse(s_255_3.EditValue.ToString())) * tariff / 100)) - (decimal.Parse(s_256_1.EditValue.ToString()) + decimal.Parse(s_256_2.EditValue.ToString()));
            }

        }

        private void CalcTextBoxesB32(object sender, EventArgs e)
        {
            if (Options.inputTypeRSW1 != 0)
            {
                if (formDataPrev != null)
                {
                    s_258_0.EditValue = formDataPrev.s_258_0.HasValue ? formDataPrev.s_258_0.Value + decimal.Parse(s_258_1.EditValue.ToString()) + decimal.Parse(s_258_2.EditValue.ToString()) + decimal.Parse(s_258_3.EditValue.ToString()) : decimal.Parse(s_258_1.EditValue.ToString()) + decimal.Parse(s_258_2.EditValue.ToString()) + decimal.Parse(s_258_3.EditValue.ToString());
                    s_259_0.EditValue = formDataPrev.s_259_0.HasValue ? formDataPrev.s_259_0.Value + decimal.Parse(s_259_1.EditValue.ToString()) + decimal.Parse(s_259_2.EditValue.ToString()) + decimal.Parse(s_259_3.EditValue.ToString()) : decimal.Parse(s_259_1.EditValue.ToString()) + decimal.Parse(s_259_2.EditValue.ToString()) + decimal.Parse(s_259_3.EditValue.ToString());
                }
                else
                {
                    s_258_0.EditValue = decimal.Parse(s_258_1.EditValue.ToString()) + decimal.Parse(s_258_2.EditValue.ToString()) + decimal.Parse(s_258_3.EditValue.ToString());
                    s_259_0.EditValue = decimal.Parse(s_259_1.EditValue.ToString()) + decimal.Parse(s_259_2.EditValue.ToString()) + decimal.Parse(s_259_3.EditValue.ToString());
                }
            }

            s_261_0.EditValue = decimal.Parse(s_258_0.EditValue.ToString()) - decimal.Parse(s_259_0.EditValue.ToString());
            s_261_1.EditValue = decimal.Parse(s_258_1.EditValue.ToString()) - decimal.Parse(s_259_1.EditValue.ToString());
            s_261_2.EditValue = decimal.Parse(s_258_2.EditValue.ToString()) - decimal.Parse(s_259_2.EditValue.ToString());
            s_261_3.EditValue = decimal.Parse(s_258_3.EditValue.ToString()) - decimal.Parse(s_259_3.EditValue.ToString());

            if (!AutoCalcSwitch.IsOn)
            {
                byte Type_ = FilledBaseDDL.SelectedIndex == 0 ? (byte)0 : (byte)1;
                string Code = "В3.2";
                DateTime Date = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == formData.Year && x.Kvartal == formData.Quarter).DateBegin.Date;

                decimal tariff = SpecOcenka.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;

                s_262_0.EditValue = decimal.Parse(s_261_0.EditValue.ToString()) * tariff / 100;
                s_262_1.EditValue = ((decimal.Parse(s_261_0.EditValue.ToString()) - decimal.Parse(s_261_2.EditValue.ToString()) - decimal.Parse(s_261_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_261_0.EditValue.ToString()) - decimal.Parse(s_261_1.EditValue.ToString()) - decimal.Parse(s_261_2.EditValue.ToString()) - decimal.Parse(s_261_3.EditValue.ToString())) * tariff / 100);
                s_262_2.EditValue = (((decimal.Parse(s_261_0.EditValue.ToString()) - decimal.Parse(s_261_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_261_0.EditValue.ToString()) - decimal.Parse(s_261_1.EditValue.ToString()) - decimal.Parse(s_261_2.EditValue.ToString()) - decimal.Parse(s_261_3.EditValue.ToString())) * tariff / 100)) - decimal.Parse(s_262_1.EditValue.ToString());
                s_262_3.EditValue = (((decimal.Parse(s_261_0.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_261_0.EditValue.ToString()) - decimal.Parse(s_261_1.EditValue.ToString()) - decimal.Parse(s_261_2.EditValue.ToString()) - decimal.Parse(s_261_3.EditValue.ToString())) * tariff / 100)) - (decimal.Parse(s_262_1.EditValue.ToString()) + decimal.Parse(s_262_2.EditValue.ToString()));

            }

        }

        private void CalcTextBoxesB31(object sender, EventArgs e)
        {
            if (Options.inputTypeRSW1 != 0)
            {
                if (formDataPrev != null)
                {
                    s_264_0.EditValue = formDataPrev.s_264_0.HasValue ? formDataPrev.s_264_0.Value + decimal.Parse(s_264_1.EditValue.ToString()) + decimal.Parse(s_264_2.EditValue.ToString()) + decimal.Parse(s_264_3.EditValue.ToString()) : decimal.Parse(s_264_1.EditValue.ToString()) + decimal.Parse(s_264_2.EditValue.ToString()) + decimal.Parse(s_264_3.EditValue.ToString());
                    s_265_0.EditValue = formDataPrev.s_265_0.HasValue ? formDataPrev.s_265_0.Value + decimal.Parse(s_265_1.EditValue.ToString()) + decimal.Parse(s_265_2.EditValue.ToString()) + decimal.Parse(s_265_3.EditValue.ToString()) : decimal.Parse(s_265_1.EditValue.ToString()) + decimal.Parse(s_265_2.EditValue.ToString()) + decimal.Parse(s_265_3.EditValue.ToString());
                }
                else
                {
                    s_264_0.EditValue = decimal.Parse(s_264_1.EditValue.ToString()) + decimal.Parse(s_264_2.EditValue.ToString()) + decimal.Parse(s_264_3.EditValue.ToString());
                    s_265_0.EditValue = decimal.Parse(s_265_1.EditValue.ToString()) + decimal.Parse(s_265_2.EditValue.ToString()) + decimal.Parse(s_265_3.EditValue.ToString());
                }
            }

            s_267_0.EditValue = decimal.Parse(s_264_0.EditValue.ToString()) - decimal.Parse(s_265_0.EditValue.ToString());
            s_267_1.EditValue = decimal.Parse(s_264_1.EditValue.ToString()) - decimal.Parse(s_265_1.EditValue.ToString());
            s_267_2.EditValue = decimal.Parse(s_264_2.EditValue.ToString()) - decimal.Parse(s_265_2.EditValue.ToString());
            s_267_3.EditValue = decimal.Parse(s_264_3.EditValue.ToString()) - decimal.Parse(s_265_3.EditValue.ToString());

            if (!AutoCalcSwitch.IsOn)
            {
                byte Type_ = FilledBaseDDL.SelectedIndex == 0 ? (byte)0 : (byte)1;
                string Code = "В3.1";
                DateTime Date = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == formData.Year && x.Kvartal == formData.Quarter).DateBegin.Date;

                decimal tariff = SpecOcenka.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;

                s_268_0.EditValue = decimal.Parse(s_267_0.EditValue.ToString()) * tariff / 100;
                s_268_1.EditValue = ((decimal.Parse(s_267_0.EditValue.ToString()) - decimal.Parse(s_267_2.EditValue.ToString()) - decimal.Parse(s_267_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_267_0.EditValue.ToString()) - decimal.Parse(s_267_1.EditValue.ToString()) - decimal.Parse(s_267_2.EditValue.ToString()) - decimal.Parse(s_267_3.EditValue.ToString())) * tariff / 100);
                s_268_2.EditValue = (((decimal.Parse(s_267_0.EditValue.ToString()) - decimal.Parse(s_267_3.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_267_0.EditValue.ToString()) - decimal.Parse(s_267_1.EditValue.ToString()) - decimal.Parse(s_267_2.EditValue.ToString()) - decimal.Parse(s_267_3.EditValue.ToString())) * tariff / 100)) - decimal.Parse(s_268_1.EditValue.ToString());
                s_268_3.EditValue = (((decimal.Parse(s_267_0.EditValue.ToString())) * tariff / 100) - ((decimal.Parse(s_267_0.EditValue.ToString()) - decimal.Parse(s_267_1.EditValue.ToString()) - decimal.Parse(s_267_2.EditValue.ToString()) - decimal.Parse(s_267_3.EditValue.ToString())) * tariff / 100)) - (decimal.Parse(s_268_1.EditValue.ToString()) + decimal.Parse(s_268_2.EditValue.ToString()));
            }

        }

        private void AutoCalcSwitch_Toggled(object sender, EventArgs e)
        {
            if (!AutoCalcSwitch.IsOn)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы выбрали режим автоматического расчета страховых взносов.\r\nПроизвести перерасчет взносов?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.Yes)
                {
                    getPrevData();
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
                list.Add("s_244_" + i.ToString());
                list.Add("s_250_" + i.ToString());
                list.Add("s_256_" + i.ToString());
                list.Add("s_262_" + i.ToString());
                list.Add("s_268_" + i.ToString());
            }

            foreach (var item in list)
            {
                //DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0];
                ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(item, true)[0]).Enabled = AutoCalcSwitch.IsOn;
            }
                
        }


        /// <summary>
        /// Проверка введенных данных Раздела 2.4
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            #region Проверяем на уникальность по коду тарифа
/*            RSW2014_Edit main = this.Owner as RSW2014_Edit;
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
                    if (RSW_2_1_List.Any(x => x.TariffCodeID == formData.TariffCodeID && x.ID != formData.ID))
                    {
                        errMessBox.Add(new ErrList { name = "на двух подразделах раздела 2.1 не может быть указан  одинаковый код тарифа", control = "TariffCodeDDL" });
                    }
                    break;
            }
*/
            #endregion

            #region Раздел 2.4   13
            if (decimal.Parse(s_244_0.EditValue.ToString()) > 0)
                if (long.Parse(s_245_0.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.3 стр.245 должна иметь ненулевое значение при ненулевой сумме значений гр.3 стр.244", control = "s_245_0" });
            #endregion
            #region Раздел 2.4   13
            if (decimal.Parse(s_244_1.EditValue.ToString()) > 0)
                if (long.Parse(s_245_1.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.4 стр.245 должна иметь ненулевое значение при ненулевой сумме значений гр.4 стр.244", control = "s_245_1" });
            #endregion
            #region Раздел 2.4   13
            if (decimal.Parse(s_244_2.EditValue.ToString()) > 0)
                if (long.Parse(s_245_2.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.5 стр.245 должна иметь ненулевое значение при ненулевой сумме значений гр.5 стр.244", control = "s_245_2" });
            #endregion
            #region Раздел 2.4   13
            if (decimal.Parse(s_244_3.EditValue.ToString()) > 0)
                if (long.Parse(s_245_3.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.6 стр.245 должна иметь ненулевое значение при ненулевой сумме значений гр.6 стр.244", control = "s_245_3" });
            #endregion



            #region Раздел 2.4   24
            if (decimal.Parse(s_250_0.EditValue.ToString()) > 0)
                if (long.Parse(s_251_0.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.3 стр.251 должна иметь ненулевое значение при ненулевой сумме значений гр.3 стр.250", control = "s_251_0" });
            #endregion
            #region Раздел 2.4   24
            if (decimal.Parse(s_250_1.EditValue.ToString()) > 0)
                if (long.Parse(s_251_1.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.4 стр.251 должна иметь ненулевое значение при ненулевой сумме значений гр.4 стр.250", control = "s_251_1" });
            #endregion
            #region Раздел 2.4   24
            if (decimal.Parse(s_250_2.EditValue.ToString()) > 0)
                if (long.Parse(s_251_2.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.5 стр.251 должна иметь ненулевое значение при ненулевой сумме значений гр.5 стр.250", control = "s_251_2" });
            #endregion
            #region Раздел 2.4   24
            if (decimal.Parse(s_250_3.EditValue.ToString()) > 0)
                if (long.Parse(s_251_3.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.6 стр.251 должна иметь ненулевое значение при ненулевой сумме значений гр.6 стр.250", control = "s_251_3" });
            #endregion



            #region Раздел 2.4   35
            if (decimal.Parse(s_256_0.EditValue.ToString()) > 0)
                if (long.Parse(s_257_0.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.3 стр.257 должна иметь ненулевое значение при ненулевой сумме значений гр.3 стр.256", control = "s_257_0" });
            #endregion
            #region Раздел 2.4   35
            if (decimal.Parse(s_256_1.EditValue.ToString()) > 0)
                if (long.Parse(s_257_1.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.4 стр.257 должна иметь ненулевое значение при ненулевой сумме значений гр.4 стр.256", control = "s_257_1" });
            #endregion
            #region Раздел 2.4   35
            if (decimal.Parse(s_256_2.EditValue.ToString()) > 0)
                if (long.Parse(s_257_2.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.5 стр.257 должна иметь ненулевое значение при ненулевой сумме значений гр.5 стр.256", control = "s_257_2" });
            #endregion
            #region Раздел 2.4   35
            if (decimal.Parse(s_256_3.EditValue.ToString()) > 0)
                if (long.Parse(s_257_3.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.6 стр.257 должна иметь ненулевое значение при ненулевой сумме значений гр.6 стр.256", control = "s_257_3" });
            #endregion



            #region Раздел 2.4   46
            if (decimal.Parse(s_262_0.EditValue.ToString()) > 0)
                if (long.Parse(s_263_0.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.3 стр.263 должна иметь ненулевое значение при ненулевой сумме значений гр.3 стр.262", control = "s_263_0" });
            #endregion
            #region Раздел 2.4   46
            if (decimal.Parse(s_262_1.EditValue.ToString()) > 0)
                if (long.Parse(s_263_1.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.4 стр.263 должна иметь ненулевое значение при ненулевой сумме значений гр.4 стр.262", control = "s_263_1" });
            #endregion
            #region Раздел 2.4   46
            if (decimal.Parse(s_262_2.EditValue.ToString()) > 0)
                if (long.Parse(s_263_2.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.5 стр.263 должна иметь ненулевое значение при ненулевой сумме значений гр.5 стр.262", control = "s_263_2" });
            #endregion
            #region Раздел 2.4   46
            if (decimal.Parse(s_262_3.EditValue.ToString()) > 0)
                if (long.Parse(s_263_3.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.6 стр.263 должна иметь ненулевое значение при ненулевой сумме значений гр.6 стр.262", control = "s_263_3" });
            #endregion


            #region Раздел 2.4   57
            if (decimal.Parse(s_268_0.EditValue.ToString()) > 0)
                if (long.Parse(s_269_0.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.3 стр.269 должна иметь ненулевое значение при ненулевой сумме значений гр.3 стр.268", control = "s_269_0" });
            #endregion
            #region Раздел 2.4   57
            if (decimal.Parse(s_268_1.EditValue.ToString()) > 0)
                if (long.Parse(s_269_1.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.4 стр.269 должна иметь ненулевое значение при ненулевой сумме значений гр.4 стр.268", control = "s_269_1" });
            #endregion
            #region Раздел 2.4   57
            if (decimal.Parse(s_268_2.EditValue.ToString()) > 0)
                if (long.Parse(s_269_2.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.5 стр.269 должна иметь ненулевое значение при ненулевой сумме значений гр.5 стр.268", control = "s_269_2" });
            #endregion
            #region Раздел 2.4   57
            if (decimal.Parse(s_268_3.EditValue.ToString()) > 0)
                if (long.Parse(s_269_3.EditValue.ToString()) <= 0)
                    errMessBox.Add(new ErrList { name = "Гр.6 стр.269 должна иметь ненулевое значение при ненулевой сумме значений гр.6 стр.268", control = "s_269_3" });
            #endregion

            if (errMessBox.Count > 0)
                check = false; 
            return check;
        }

        private void CodeBaseSpin_ValueChanged()
        {

        }

    }
}
