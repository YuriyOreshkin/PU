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
    public partial class RSW2014_6_4_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private decimal startMaxSum = 0;
        private bool editLoad { get; set; }
        private string textBoxContents = string.Empty;
        private decimal s_0_3_tmp = 0;
        private List<string> errMessBox = new List<string>();

        private bool cleanData = true;
        public bool autoCalc { get; set; }
        public List<long> platCatList { get; set; }

        public byte periodQ = 14;
        public short periodY = 0;
        public long staffID = 0;

        public PlatCategory PlatCat { get; set; }
        public FormsRSW2014_1_Razd_6_4 formData { get; set; }
        public FormsRSW2014_1_Razd_6_4 formDataPrev { get; set; }


        public RSW2014_6_4_Edit()
        {
            InitializeComponent();
        }

        private void RSW2014_6_4_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (cleanData)
                formData = null;
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

        private void radPanel1_MouseHover(object sender, EventArgs e)
        {
            radPanel1.Font = new Font("Segoe UI", 9, FontStyle.Underline | FontStyle.Bold);
        }

        private void radPanel1_MouseLeave(object sender, EventArgs e)
        {
            radPanel1.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        private void radPanel1_MouseClick(object sender, MouseEventArgs e)
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
            long id = formData.ID;
            formData = new FormsRSW2014_1_Razd_6_4();
            formData.ID = id;
            formData.PlatCategory = child.PlatCat == null ? null : child.PlatCat;
            changePlatCatHeader();
        }

        private void changePlatCatHeader()
        {
            if (PlatCat != null)
            {
                if (checkPlatCat(PlatCat))
                {
                    if ((PlatCat.Code == "ФЛ" || PlatCat.Code == "СДП" || PlatCat.Code == "ДП") && periodQ != 0)
                    {
                        InfoLabel.Visible = true;
                        radPanel1.Text = "Категория плательщика - не определена... Нажмите для выбора";
                        RadMessageBox.Show(this, "Для категории ФЛ/СДП/ДП допустим отчетный период только в целом за год.", "Внимание");
                        PlatCat = null;
                        return;
                    }

                    radPanel1.Text = PlatCat.Code + "   " + PlatCat.Name;
                    InfoLabel.Visible = false;
                    if (!autoCalc)
                        getPrevData();
                }
                else
                {
                    InfoLabel.Visible = true;
                    radPanel1.Text = "Категория плательщика - не определена... Нажмите для выбора";
                    RadMessageBox.Show(this, "Срок действия Категории Плательщика: [" + PlatCat.Code + "] не совпадает с выбранным Отчетным периодом. Необходимо выбрать другую категорию!", "Внимание");
                    PlatCat = null;
                }
            }
            else
            {
                InfoLabel.Visible = true;
                radPanel1.Text = "Категория плательщика - не определена... Нажмите для выбора";
            }
        }

        /// <summary>
        /// Проверяем действует ли категория плательщика в выбранном расчетном периоде
        /// </summary>
        /// <param name="platCat"></param>
        /// <returns></returns>
        private bool checkPlatCat(PlatCategory platCat)
        {
            bool result = false;
            if (Options.RaschetPeriodInternal.Any(x => x.Year == periodY && x.Kvartal == periodQ))
            {
                DateTime date = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == periodY && x.Kvartal == periodQ).DateBegin;

                if ((platCat.DateEnd.HasValue && (platCat.DateBegin.Value <= date && platCat.DateEnd.Value >= date)) || (!platCat.DateEnd.HasValue && platCat.DateBegin <= date))
                {
                    result = true;
                }
            }

            return result;
        }


        private void updateTextBoxes()
        {
            var fields = typeof(FormsRSW2014_1_Razd_6_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    //    DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

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

                if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == year && x.Quarter == q && x.YearKorr == year && x.QuarterKorr == quarter && x.StaffID == staffID && x.TypeInfoID == 2))
                {
                    FormsRSW2014_1_Razd_6_1 RSW_6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == year && x.Quarter == q && x.YearKorr == year && x.QuarterKorr == quarter && x.StaffID == staffID && x.TypeInfoID == 2);
                    formDataPrev = RSW_6.FormsRSW2014_1_Razd_6_4.FirstOrDefault(x => x.PlatCategoryID == PlatCat.ID);
                }
                else if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == year && x.Quarter == quarter && x.StaffID == staffID && x.TypeInfoID == 1))
                {
                    FormsRSW2014_1_Razd_6_1 RSW_6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == year && x.Quarter == quarter && x.StaffID == staffID && x.TypeInfoID == 1);
                    formDataPrev = RSW_6.FormsRSW2014_1_Razd_6_4.FirstOrDefault(x => x.PlatCategoryID == PlatCat.ID);
                }
                //           formDataPrev = db.FormsRSW2014_1_Razd_6_4.Where(x => x.Year == year && x.Quarter == quarter && x.).OrderByDescending(x => x.CorrectionNum).First();
            }
            else
            {
                formDataPrev = null;
            }
            textBoxContents = "";
            DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find("s_1_0", true)[0];
            CalcTextBoxes(box, null);
        }

        private void RSW2014_6_4_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            if (Options.mrot != null)
                predBase.Text = "Предельная база: " + Options.mrot.NalogBase.ToString() + "рублей в " + Options.mrot.Year.ToString() + " финансовом году";
            else
                predBase.Text = "Предельная база: Ошибка при получении данных";


            s_0_0.Enabled = autoCalc;
            s_0_1.Enabled = autoCalc;
            s_0_2.Enabled = autoCalc;

            PrevPredSumFlag_ToggleStateChanged(null, null);
            formDataPrev = new FormsRSW2014_1_Razd_6_4();

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    foreach (var item in param.windowData)
                    {
                        int i = 0;

                        switch (item.control)
                        {
                            case "fixPlatCatCheckBox":
                                fixPlatCatCheckBox.Checked = item.value == "true" ? true : false;
                                break;
                            case "PlatCat":
                                if (fixPlatCatCheckBox.Checked && action == "add" && !String.IsNullOrEmpty(item.value))
                                { 
                                    long id = 0;

                                    long.TryParse(item.value, out id);
                                    if (db.PlatCategory.Any(x => x.ID == id))
                                    {
                                        PlatCat = db.PlatCategory.FirstOrDefault(x => x.ID == id);

                                        if (action == "add")
                                        {
                                            formData = new FormsRSW2014_1_Razd_6_4();
                                        }

                                        formData.PlatCategory = PlatCat == null ? null : PlatCat;
                                        changePlatCatHeader();
                                    }
                                }
                                break;
                            case "PrevPredSumFlag":
                                PrevPredSumFlag.Checked = item.value == "true" ? true : false;
                                break;
                            case "ControlPrevPredSumFlag":
                                ControlPrevPredSumFlag.Checked = item.value == "true" ? true : false;
                                break;
                            case "copyToOPSSum":
                                copyToOPSSum.Checked = item.value == "true" ? true : false;
                                break;
                            case "copyToOPSAkt":
                                copyToOPSAkt.Checked = item.value == "true" ? true : false;
                                break;
                        }
                    }

                }
                catch
                { }

            }

            switch (action)
            {
                case "add":
                    if (formData == null)
                        formData = new FormsRSW2014_1_Razd_6_4();
                    //InfoLabel.Visible = true;
                    break;
                case "edit":
                    editLoad = true;
                    updateTextBoxes();
                    PlatCat = formData.PlatCategory;
                    if (PlatCat != null)
                    {
                        if (checkPlatCat(PlatCat))
                        {
                            radPanel1.Text = PlatCat.Code + "   " + PlatCat.Name;
                            InfoLabel.Visible = false;
                            if (!autoCalc)
                                getPrevData();
                        }
                        else
                        {
                            InfoLabel.Visible = true;
                            radPanel1.Text = "Категория плательщика - не определена... Нажмите для выбора";
                            RadMessageBox.Show(this, "Срок действия Категории Плательщика: [" + PlatCat.Code + "] закончился " + PlatCat.DateEnd.Value.ToShortDateString() + " Необходимо выбрать другую категорию!", "Внимание");
                            PlatCat = null;
                        }

                    }
                    editLoad = false;
                    break;
            }
            if (formDataPrev != null && formDataPrev.s_0_0.HasValue)
                startMaxSum = formDataPrev.s_0_0.Value;
            //                    decimal.Parse(s_0_0.Text.ToString());

            this.s_1_0.Leave += (s, c) => CalcTextBoxes(s, c);
            this.s_1_1.Leave += (s, c) => CalcTextBoxes2(s, c);
            this.s_1_2.Leave += (s, c) => CalcTextBoxes2(s, c);
            this.s_1_3.Leave += (s, c) => CalcTextBoxes3(s, c);
            this.s_2_0.Leave += (s, c) => CalcTextBoxes(s, c);
            this.s_2_1.Leave += (s, c) => CalcTextBoxes2(s, c);
            this.s_2_2.Leave += (s, c) => CalcTextBoxes2(s, c);
            this.s_2_3.Leave += (s, c) => CalcTextBoxes3(s, c);
            this.s_3_0.Leave += (s, c) => CalcTextBoxes(s, c);
            this.s_3_1.Leave += (s, c) => CalcTextBoxes2(s, c);
            this.s_3_2.Leave += (s, c) => CalcTextBoxes2(s, c);
            this.s_3_3.Leave += (s, c) => CalcTextBoxes3(s, c);

            // ПРОВЕРИТЬ!!!

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void validation()
        {
            errMessBox = new List<string>();
            if (platCatList.Contains(PlatCat.ID))
            {
                errMessBox.Add("Дублирование записи по ключу уникальности");
            }


        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (PlatCat != null)
            {
                getValues();
                validation();

                if (errMessBox.Count == 0)
                {
                    cleanData = false;
                    this.Close();
                }
                else
                {
                    foreach (var item in errMessBox)
                    {
                        RadMessageBox.Show(this, item, "Внимание!");
                    }
                }

            }
            else
            {
                RadMessageBox.Show("Необходимо выбрать категорию плательщика");
                radPanel1_MouseClick(null, null);
                if (PlatCat != null)
                {
                    getValues();
                    validation();

                    if (errMessBox.Count == 0)
                    {
                        cleanData = false;
                        this.Close();
                    }
                    else
                        foreach (var item in errMessBox)
                        {
                            RadMessageBox.Show(this, item, "Внимание!");
                        }
                }
            }
        }

        private void getValues()
        {
            var fields = typeof(FormsRSW2014_1_Razd_6_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                            //    DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

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

        private void checkPrevPredSum()
        {
            decimal max = Options.mrot != null ? Options.mrot.NalogBase : 0;
            decimal tmp = 0;
            //            if (decimal.Parse(s_0_0.EditValue.ToString()) > max)
            //            {
            //                s_0_3.EditValue = decimal.Parse(s_0_0.EditValue.ToString()) - max;
            //            }
            decimal s_0_1_ = 0;
            if (formDataPrev != null)
            {
                s_0_1_ = formDataPrev.s_0_1.HasValue ? formDataPrev.s_0_1.Value : 0;
            }

            s_0_3.EditValue = decimal.Parse(s_0_0.EditValue.ToString()) - s_0_1_ - (decimal.Parse(s_1_1.EditValue.ToString()) + decimal.Parse(s_2_1.EditValue.ToString()) + decimal.Parse(s_3_1.EditValue.ToString()));

            if (decimal.Parse(s_1_0.EditValue.ToString()) != 0)
            {
                tmp = startMaxSum + decimal.Parse(s_1_0.EditValue.ToString());
                if (tmp > max)
                {
                    s_1_3.EditValue = tmp - max;
                }
                else
                    s_1_3.ResetText();
            }

            if (decimal.Parse(s_2_0.EditValue.ToString()) != 0)
            {
                tmp = startMaxSum + decimal.Parse(s_1_0.EditValue.ToString()) + decimal.Parse(s_2_0.EditValue.ToString());
                if (tmp > max)
                {
                    s_2_3.EditValue = tmp - max;
                }
                else
                    s_2_3.ResetText();

            }
            if (decimal.Parse(s_3_0.EditValue.ToString()) != 0)
            {
                tmp = startMaxSum + decimal.Parse(s_1_0.EditValue.ToString()) + decimal.Parse(s_2_0.EditValue.ToString()) + decimal.Parse(s_3_0.EditValue.ToString());
                if (tmp > max)
                {
                    s_3_3.EditValue = tmp - max;
                }
                else
                    s_3_3.ResetText();
            }



        }

        private void PrevPredSumFlag_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (PrevPredSumFlag.Checked)
            {
                s_0_3.Enabled = true;
                s_1_3.Enabled = true;
                s_2_3.Enabled = true;
                s_3_3.Enabled = true;

            }
            else
            {
                s_0_3.Enabled = false;
                s_1_3.Enabled = false;
                s_2_3.Enabled = false;
                s_3_3.Enabled = false;


                checkPrevPredSum();
            }
        }

        private void s_0_0_Leave(object sender, EventArgs e)
        {
            checkPrevPredSum();
        }


        private void s_0_0_TextChanged(object sender, EventArgs e)
        {
            startMaxSum = decimal.Parse(s_0_0.EditValue.ToString());

        }

        private void CalcTextBoxes(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                DevExpress.XtraEditors.TextEdit box_old = (DevExpress.XtraEditors.TextEdit)sender;
                if (textBoxContents == box_old.Text.ToString())
                    return;
                else if (decimal.Parse(box_old.EditValue.ToString()) < 0)
                {
                    box_old.Text = textBoxContents;
                    return;
                }
                else
                {
                    var row = int.Parse(box_old.Name.Substring(2, 1));
                    decimal max = Options.mrot != null ? Options.mrot.NalogBase : 0;

                    //               if (!autoCalc)
                    if (formDataPrev != null)
                    {
                        s_0_0.EditValue = formDataPrev.s_0_0.HasValue ? formDataPrev.s_0_0.Value : (decimal)0;
                        s_0_1.EditValue = formDataPrev.s_0_1.HasValue ? formDataPrev.s_0_1.Value : (decimal)0;
                        s_0_2.EditValue = formDataPrev.s_0_2.HasValue ? formDataPrev.s_0_2.Value : (decimal)0;
                    }
                    else
                    {
                        s_0_0.EditValue = (decimal)0;
                        s_0_1.EditValue = (decimal)0;
                        s_0_2.EditValue = (decimal)0;
                    }


                    for (int i = 1; i <= 3; i++)
                    {
                        List<String> name = new List<string>();
                        List<DevExpress.XtraEditors.TextEdit> box = new List<DevExpress.XtraEditors.TextEdit>();
                        for (var j = 0; j <= 3; j++)
                        {
                            name.Add("s_" + i + "_" + j);
                            box.Add((DevExpress.XtraEditors.TextEdit)this.Controls.Find(name[j], true)[0]);
                        }

                            s_0_0.EditValue = decimal.Parse(s_0_0.EditValue.ToString()) + decimal.Parse(box[0].EditValue.ToString());
//                            decimal s_0_1_t = decimal.Parse(s_0_1.EditValue.ToString()) + decimal.Parse(box[1].EditValue.ToString());
                            decimal s_0_1_t = decimal.Parse(s_0_1.EditValue.ToString()) + (copyToOPSSum.Checked ? decimal.Parse(box[0].EditValue.ToString()) : decimal.Parse(box[1].EditValue.ToString()));

                            //s_0_2.EditValue = decimal.Parse(s_0_2.EditValue.ToString()) + decimal.Parse(box[2].EditValue.ToString());

                            if (s_0_1_t > max)//
                        {
                            if (ControlPrevPredSumFlag.Checked)
                            {
                                s_0_3_tmp = s_0_1_t - max;//
                            }
                            else
                                s_0_3_tmp = 0;
                        }
                        else
                        {
                            s_0_3_tmp = 0;
                        }

                        //if (copyToOPSSum.Checked) // если выбрано копировать суммы на ОПС
                        //{
                            if (i >= row)
                            {
                                if (ControlPrevPredSumFlag.Checked) // Запрет на ввод Базы для начисления взносов на ОПС при превышении предельной базы
                                {
                                    if (s_0_3_tmp > decimal.Parse(box[0].EditValue.ToString()))
                                    {
                                        if (copyToOPSSum.Checked) // если выбрано копировать суммы на ОПС
                                            box[1].EditValue = 0;

                                        if (!PrevPredSumFlag.Checked) // Ручной ввод сумм выплат, превышающих предельную базу
                                            box[3].EditValue = box[0].EditValue;
                                    }
                                    else
                                    {
                                        if (copyToOPSSum.Checked) // если выбрано копировать суммы на ОПС
                                            box[1].EditValue = decimal.Parse(box[0].EditValue.ToString()) - s_0_3_tmp;

                                        if (!PrevPredSumFlag.Checked) // Ручной ввод сумм выплат, превышающих предельную базу
                                            box[3].EditValue = s_0_3_tmp;
                                    }
                                }
                                else
                                {
                                    if (copyToOPSSum.Checked) // если выбрано копировать суммы на ОПС
                                        box[1].EditValue = decimal.Parse(box[0].EditValue.ToString());

                                    //if (!PrevPredSumFlag.Checked) // Ручной ввод сумм выплат, превышающих предельную базу
                                    //    box[3].EditValue = decimal.Parse(box[0].EditValue.ToString()) - decimal.Parse(box[1].EditValue.ToString());

                                    //s_0_3_tmp = s_0_3_tmp + decimal.Parse(box[3].EditValue.ToString());
                                }

                                if (copyToOPSAkt.Checked) // если выбрано копировать суммы на ОПС по гражданско-правовым актам
                                {
                                    box[2].EditValue = box[1].EditValue;
                                }

                            }
                        //}
                        //else // если НЕ выбрано копировать суммы на ОПС
                        //{
                        //    if (ControlPrevPredSumFlag.Checked) // Запрет на ввод Базы для начисления взносов на ОПС при превышении предельной базы
                        //    {
                        //        if (s_0_3_tmp > decimal.Parse(box[0].EditValue.ToString()))
                        //        {
                        //            if (!PrevPredSumFlag.Checked) // Ручной ввод сумм выплат, превышающих предельную базу
                        //                box[3].EditValue = box[0].EditValue;
                        //        }
                        //        else
                        //        {
                        //            if (!PrevPredSumFlag.Checked) // Ручной ввод сумм выплат, превышающих предельную базу
                        //                box[3].EditValue = s_0_3_tmp;
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (!PrevPredSumFlag.Checked) // Ручной ввод сумм выплат, превышающих предельную базу
                        //            box[3].EditValue = decimal.Parse(box[0].EditValue.ToString()) - decimal.Parse(box[1].EditValue.ToString());

                        //        s_0_3_tmp = s_0_3_tmp + decimal.Parse(box[3].EditValue.ToString());
                        //    }
                        //}

                        if (((decimal.Parse(s_0_1.EditValue.ToString()) + decimal.Parse(box[1].EditValue.ToString())) > max) && ControlPrevPredSumFlag.Checked)
                        {
                                s_0_1.EditValue = max;
                        }
                        else
                        {
                                s_0_1.EditValue = decimal.Parse(s_0_1.EditValue.ToString()) + decimal.Parse(box[1].EditValue.ToString());
                        }


                        if (((decimal.Parse(s_0_2.EditValue.ToString()) + decimal.Parse(box[2].EditValue.ToString())) > max) && ControlPrevPredSumFlag.Checked)
                        {
                                s_0_2.EditValue = max;
                        }
                        else
                        {
                                s_0_2.EditValue = decimal.Parse(s_0_2.EditValue.ToString()) + decimal.Parse(box[2].EditValue.ToString());
                        }


                    }

                    //       if (copyToOPSSum.Checked)
                    //      s_0_3.EditValue = s_0_3_tmp;
                    //s_0_3.EditValue = decimal.Parse(s_0_3.EditValue.ToString()) + s_0_3_tmp;
                    if (!PrevPredSumFlag.Checked)
                        s_0_3.EditValue = decimal.Parse(s_1_3.EditValue.ToString()) + decimal.Parse(s_2_3.EditValue.ToString()) + decimal.Parse(s_3_3.EditValue.ToString());
                    



                }
            }

        }

        private void CalcTextBoxes2(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                DevExpress.XtraEditors.TextEdit box_old = (DevExpress.XtraEditors.TextEdit)sender;
                if (textBoxContents == box_old.Text.ToString())
                    return;
                else if (decimal.Parse(box_old.EditValue.ToString()) < 0)
                {
                    box_old.Text = textBoxContents;
                    return;
                }
                else
                {
                    var row = int.Parse(box_old.Name.Substring(2, 1));
                    decimal max = Options.mrot != null ? Options.mrot.NalogBase : 0;

                    if (!autoCalc)
                        if (formDataPrev != null)
                        {
                            s_0_0.EditValue = formDataPrev.s_0_0.HasValue ? formDataPrev.s_0_0.Value : (decimal)0;
                            s_0_1.EditValue = formDataPrev.s_0_1.HasValue ? formDataPrev.s_0_1.Value : (decimal)0;
                            s_0_2.EditValue = formDataPrev.s_0_2.HasValue ? formDataPrev.s_0_2.Value : (decimal)0;
                            s_0_3_tmp = formDataPrev.s_0_3.HasValue ? formDataPrev.s_0_3.Value : 0;
                        }
                        else
                        {
                            s_0_0.EditValue = (decimal)0;
                            s_0_1.EditValue = (decimal)0;
                            s_0_2.EditValue = (decimal)0;
                            s_0_3_tmp = 0;
                        }

                    for (int i = 1; i <= 3; i++)
                    {
                        List<String> name = new List<string>();
                        List<DevExpress.XtraEditors.TextEdit> box = new List<DevExpress.XtraEditors.TextEdit>();
                        for (var j = 0; j <= 3; j++)
                        {
                            name.Add("s_" + i + "_" + j);
                            box.Add((DevExpress.XtraEditors.TextEdit)this.Controls.Find(name[j], true)[0]);
                        }

                            s_0_0.EditValue = decimal.Parse(s_0_0.EditValue.ToString()) + decimal.Parse(box[0].EditValue.ToString());

                            if (i >= row)
                            {
                                if(copyToOPSSum.Checked)
                                {
                                    box[1].EditValue = decimal.Parse(box[0].EditValue.ToString());
                                }

                                decimal s_0_1_t = decimal.Parse(s_0_1.EditValue.ToString()) + decimal.Parse(box[1].EditValue.ToString());

                                if (ControlPrevPredSumFlag.Checked)
                                {
                                    if (s_0_1_t > max)//
                                    {
                                        if (ControlPrevPredSumFlag.Checked)
                                        {
                                            s_0_3_tmp = s_0_1_t - max;//
                                        }
                                        else
                                            s_0_3_tmp = 0;
                                    }
                                    //else
                                    //{
                                    //    s_0_3_tmp = 0;
                                    //}

                                    if (s_0_3_tmp > decimal.Parse(box[1].EditValue.ToString()))//
                                    {
                                        if (!PrevPredSumFlag.Checked)
                                            box[3].EditValue = box[1].EditValue;//

                                        box[1].EditValue = 0;

                                    }
                                    else
                                    {
                                        if (copyToOPSSum.Checked) // если выбрано копировать суммы на ОПС
                                        {
                                            box[1].EditValue = decimal.Parse(box[0].EditValue.ToString()) - s_0_3_tmp;
                                        }
                                        else
                                        {
                                            box[1].EditValue = decimal.Parse(box[1].EditValue.ToString()) - s_0_3_tmp;
                                        }
                                        if (!PrevPredSumFlag.Checked)
                                            box[3].EditValue = s_0_3_tmp;
                                    }
                                }
                                else
                                {
                                    //                    box[1].EditValue = decimal.Parse(box[0].EditValue.ToString());
                                    //Зачем это надо ???                            box[3].EditValue = decimal.Parse(box[0].EditValue.ToString()) - decimal.Parse(box[1].EditValue.ToString());
                                    if ((box[3].EditValue == null) || (box[3].EditValue != null && String.IsNullOrEmpty(box[3].EditValue.ToString())))
                                        box[3].EditValue = 0;
                                    //                    s_0_3_tmp = s_0_3_tmp - decimal.Parse(box[1].EditValue.ToString());
                                    s_0_3_tmp = s_0_3_tmp + decimal.Parse(box[3].EditValue.ToString());

                                }
                            }

                        if (((decimal.Parse(s_0_1.EditValue.ToString()) + decimal.Parse(box[1].EditValue.ToString())) > max) && ControlPrevPredSumFlag.Checked)
                        {
                                s_0_1.EditValue = max;
                        }
                        else
                        {
                                s_0_1.EditValue = decimal.Parse(s_0_1.EditValue.ToString()) + decimal.Parse(box[1].EditValue.ToString());
                        }


                        //                    box[2].EditValue = box[1].EditValue;
                        if (((decimal.Parse(s_0_2.EditValue.ToString()) + decimal.Parse(box[2].EditValue.ToString())) > max) && ControlPrevPredSumFlag.Checked)
                        {
                                s_0_2.EditValue = max;
                        }
                        else
                        {
                                s_0_2.EditValue = decimal.Parse(s_0_2.EditValue.ToString()) + decimal.Parse(box[2].EditValue.ToString());
                        }



                    }

                    //            if (copyToOPSSum.Checked)
                    if (!PrevPredSumFlag.Checked)
                        s_0_3.EditValue = decimal.Parse(s_1_3.EditValue.ToString()) + decimal.Parse(s_2_3.EditValue.ToString()) + decimal.Parse(s_3_3.EditValue.ToString());
                        //s_0_3.EditValue = s_0_3_tmp;
                }
            }
        }

        private void CalcTextBoxes3(object sender, EventArgs e)
        {
            if (!editLoad)
            {
                if (PrevPredSumFlag.Checked)
                {
                    DevExpress.XtraEditors.TextEdit box_old = (DevExpress.XtraEditors.TextEdit)sender;
                    if (textBoxContents == box_old.Text.ToString())
                        return;
                    else
                    {
                        s_0_3.EditValue = (decimal)0;
                        for (int i = 1; i <= 3; i++)
                        {
                            List<String> name = new List<string>();
                            DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find("s_" + i + "_3", true)[0];

                            s_0_3.EditValue = decimal.Parse(s_0_3.EditValue.ToString()) + decimal.Parse(box.EditValue.ToString());


                        }


                    }
                }
            }
        }

        private void copyToOPSAkt_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (copyToOPSAkt.Checked)
            {
                copyToOPSSum.Checked = true;
                copyToOPSSum.Enabled = false;
                DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find("s_1_0", true)[0];
                textBoxContents = (decimal.Parse(box.EditValue.ToString()) - 10).ToString();
                CalcTextBoxes(box, null);
            }
            else
                copyToOPSSum.Enabled = true;


        }

        private void s_0_0_Enter(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)sender;

            textBoxContents = box.Text.ToString();
        }

        private void ControlPrevPredSumFlag_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            //if (ControlPrevPredSumFlag.Checked)
            //{
                DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find("s_1_0", true)[0];
                textBoxContents = (decimal.Parse(box.EditValue.ToString()) - 10).ToString();
                CalcTextBoxes(box, null);
                CalcTextBoxes2(box, null);
            //}
        }

        private void copyToOPSSum_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            //if (copyToOPSSum.Checked)
            //{
                DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find("s_1_0", true)[0];
                textBoxContents = (decimal.Parse(box.EditValue.ToString()) - 10).ToString();
                CalcTextBoxes(box, null);
            //}
        }

        private void RSW2014_6_4_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками

            List<WindowData> windowData = new List<WindowData> { };

            windowData.Add(new WindowData
            {
                control = "fixPlatCatCheckBox",
                value = fixPlatCatCheckBox.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "PlatCat",
                value = PlatCat == null ? "" : PlatCat.ID.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "PrevPredSumFlag",
                value = PrevPredSumFlag.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "ControlPrevPredSumFlag",
                value = ControlPrevPredSumFlag.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "copyToOPSSum",
                value = copyToOPSSum.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "copyToOPSAkt",
                value = copyToOPSAkt.Checked ? "true" : "false"
            });


            props.setFormParams(this, windowData);
        }













    }
}
