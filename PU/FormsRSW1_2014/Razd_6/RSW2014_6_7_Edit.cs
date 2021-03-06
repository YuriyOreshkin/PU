﻿using System;
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
    public partial class RSW2014_6_7_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool cleanData = true;
        private decimal startMaxSum = 0;
        public bool autoCalc { get; set; }

        public byte periodQ = 14;
        public short periodY = 0;
        public long staffID = 0;

        public SpecOcenkaUslTruda ocenka = new SpecOcenkaUslTruda { };
        public FormsRSW2014_1_Razd_6_7 formData { get; set; }
        public FormsRSW2014_1_Razd_6_7 formDataPrev { get; set; }

        public RSW2014_6_7_Edit()
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
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RSW2014_6_7_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (cleanData)
                formData = null;
        }

        private void radPanel1_MouseHover(object sender, EventArgs e)
        {
            radPanel1.Font = new Font("Segoe UI", 9, FontStyle.Underline);
        }

        private void radPanel1_MouseLeave(object sender, EventArgs e)
        {
            radPanel1.Font = new Font("Segoe UI", 9);
        }

        private void radPanel1_Click(object sender, EventArgs e)
        {
            /*PU.Dictionaries.BaseDictionaryFormList child = new PU.Dictionaries.BaseDictionaryFormList();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ocenka = ocenka;
            child.action = "selection";
            child.DictName = "SpecOcenkaUslTruda";
            child.radButtonSelect.Visible = true;
            child.ShowDialog();
            formData = null;
            formData = new FormsRSW2014_1_Razd_6_7();
            ocenka = child.ocenka;
//            formData.SpecOcenkaUslTruda = null;
//            formData.SpecOcenkaUslTrudaReference = null;
            formData.SpecOcenkaUslTruda = child.ocenka == null ? null : child.ocenka;

            if (ocenka != null)
            {
                radPanel1.Text = ocenka.Code + "   " + ocenka.Name;
            }
            else
            {
                radPanel1.Text = "Код специальной оценки труда - не определен... Нажмите для выбора";
            }
            if (!autoCalc)
                getPrevData();
            CalcTextBoxes(null, null);*/

        }

        private void updateTextBoxes()
        {
            var fields = typeof(FormsRSW2014_1_Razd_6_7).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
             //       DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                    if (formData != null)
                    {
                        var properties = formData.GetType().GetProperty(itemName);
                        object value = properties.GetValue(formData, null);

                        string type = properties.PropertyType.FullName;
                        if (type.Contains("["))
                            type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                        type = type.Split(',')[0].Split('.')[1].ToLower();

                        //if ()
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

        private void getPrevData()
        {
            short year = periodY;
            byte q = periodQ;
            if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
            {
                byte quarter = 20;
                if (q == 6)
                    quarter = 3;
                else if (q == 9)
                    quarter = 6;
                else if (q == 0)
                    quarter = 9;

                FormsRSW2014_1_Razd_6_1 RSW_6 = null;
                if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == year && x.Quarter == q && x.YearKorr == year && x.QuarterKorr == quarter && x.StaffID == staffID && x.TypeInfoID == 2))
                {
                    RSW_6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == year && x.Quarter == q && x.YearKorr == year && x.QuarterKorr == quarter && x.StaffID == staffID && x.TypeInfoID == 2);
                }
                else if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == year && x.Quarter == quarter && x.StaffID == staffID && x.TypeInfoID == 1))
                {
                    RSW_6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == year && x.Quarter == quarter && x.StaffID == staffID && x.TypeInfoID == 1);
                }

                if (RSW_6 != null)
                {
                    if (ocenka.ID != 0)
                    {
                        if (RSW_6.FormsRSW2014_1_Razd_6_7.Any(x => x.SpecOcenkaUslTrudaID == ocenka.ID))
                        {
                            formDataPrev = RSW_6.FormsRSW2014_1_Razd_6_7.FirstOrDefault(x => x.SpecOcenkaUslTrudaID == ocenka.ID);
                        }
                        else
                            formDataPrev = null;
                    }
                    else
                    {
                        if (RSW_6.FormsRSW2014_1_Razd_6_7.Any(x => x.SpecOcenkaUslTrudaID == null))
                        {
                            formDataPrev = RSW_6.FormsRSW2014_1_Razd_6_7.FirstOrDefault(x => x.SpecOcenkaUslTrudaID == null);
                        }
                        else
                            formDataPrev = null;
                    }
                }

            }
            else
            {
                formDataPrev = null;
            }

            CalcTextBoxes(null, null);
        }

        private void RSW2014_6_7_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            s_0_0.Enabled = autoCalc;
            s_0_1.Enabled = autoCalc;

            startMaxSum = decimal.Parse(s_0_0.EditValue.ToString());
            formDataPrev = new FormsRSW2014_1_Razd_6_7();

            switch (action)
            {
                case "add":
                    formData = new FormsRSW2014_1_Razd_6_7();
                    if (!autoCalc)
                        getPrevData();
                    break;
                case "edit":
                    updateTextBoxes();
                    ocenka = formData.SpecOcenkaUslTruda == null ? new SpecOcenkaUslTruda() : formData.SpecOcenkaUslTruda;
                    if (ocenka.ID != 0)
                    {
                        radPanel1.Text = ocenka.Code + "   " + ocenka.Name;
                    }
                    else
                    {
                        radPanel1.Text = "Код специальной оценки труда - не определен... Нажмите для выбора";
                    }
                    if (!autoCalc)
                        getPrevData();

                    break;
            }

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (ocenka != null)
            {
                getValues();
                cleanData = false;
                this.Close();
            }
            else
            {
                MessageBox.Show("Необходимо выбрать код специальной оценки труда");
                radPanel1_Click(null, null);
                radButton2_Click(null, null);
//                getValues();
//                this.Close();
            }
        }

        private void getValues()
        {
            var fields = typeof(FormsRSW2014_1_Razd_6_7).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                   //         DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

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


        private void CalcTextBoxes(object sender, EventArgs e)
        {
            if (!autoCalc)
                if (formDataPrev != null)
                {
                    s_0_0.EditValue = formDataPrev.s_0_0.HasValue ? formDataPrev.s_0_0.Value + decimal.Parse(s_1_0.EditValue.ToString()) + decimal.Parse(s_2_0.EditValue.ToString()) + decimal.Parse(s_3_0.EditValue.ToString()) : decimal.Parse(s_1_0.EditValue.ToString()) + decimal.Parse(s_2_0.EditValue.ToString()) + decimal.Parse(s_3_0.EditValue.ToString());
                    s_0_1.EditValue = formDataPrev.s_0_1.HasValue ? formDataPrev.s_0_1.Value + decimal.Parse(s_1_1.EditValue.ToString()) + decimal.Parse(s_2_1.EditValue.ToString()) + decimal.Parse(s_3_1.EditValue.ToString()) : decimal.Parse(s_1_1.EditValue.ToString()) + decimal.Parse(s_2_1.EditValue.ToString()) + decimal.Parse(s_3_1.EditValue.ToString());
                }
                else
                {
                    s_0_0.EditValue = decimal.Parse(s_1_0.EditValue.ToString()) + decimal.Parse(s_2_0.EditValue.ToString()) + decimal.Parse(s_3_0.EditValue.ToString());
                    s_0_1.EditValue = decimal.Parse(s_1_1.EditValue.ToString()) + decimal.Parse(s_2_1.EditValue.ToString()) + decimal.Parse(s_3_1.EditValue.ToString());
                }

        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            ocenka = new SpecOcenkaUslTruda { };
            radPanel1.Text = "Код специальной оценки труда - не определен... Нажмите для выбора";
            formData.SpecOcenkaUslTruda = null;

            if (!autoCalc)
                getPrevData();
            CalcTextBoxes(null, null);

        }



    }
}
