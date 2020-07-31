using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using System.Linq;
using PU.Classes;

namespace PU.FormsRSW2014
{
    public partial class RSW2014_5_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool cleanData = true;
        private Staff staffInfo { get; set; }
        private List<ErrList> errMessBox = new List<ErrList>();


        public FormsRSW2014_1_Razd_5 formData { get; set; }
        public FormsRSW2014_1_Razd_5 formDataPrev { get; set; }

        public RSW2014_5_Edit()
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

        private void RSW2014_5_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            switch (action)
            {
                case "add":
                    NumOrdSpin.Value = formData.NumOrd.HasValue ? formData.NumOrd.Value : 1;
                    formDataPrev = null;
                    break;
                case "edit":
                    updateTextBoxes();
                    if (Options.inputTypeRSW1 != 0)
                    {
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

                            string insNum = Insurance.Text.Replace("-", "");

                            if (db.FormsRSW2014_1_Razd_5.Any(x => x.Year == year && x.Quarter == quarter && x.Insurance == insNum && x.InsurerID == Options.InsID))
                                formDataPrev = db.FormsRSW2014_1_Razd_5.Where(x => x.Year == year && x.Quarter == quarter && x.Insurance == insNum && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                        }
                        else
                        {
                            formDataPrev = null;
                        }
                    }
                    else
                        formDataPrev = null;

                    break;
            }

            SumPay.Enabled = (Options.inputTypeRSW1 == 0 || Options.inputTypeRSW1 == 2) ? true : false;

        }

        private void getValues()
        {
            formData.NumOrd = (long)NumOrdSpin.Value;
            formData.LastName = LastName.Text;
            formData.FirstName = FirstName.Text;
            formData.MiddleName = MiddleName.Text;
            formData.Insurance = Insurance.Value.ToString();
            formData.CorrectionNum = !String.IsNullOrEmpty(InsuranceCheck.Text) ? byte.Parse(InsuranceCheck.Text) : (byte)0;
            formData.NumSpravka = NumSpravka.Text;
            if (DateSpravka.EditValue != null)
                formData.DateSpravka = DateTime.Parse(DateSpravka.EditValue.ToString()).Date;
            else
                formData.DateSpravka = null;
            formData.NumSpravka1 = NumSpravka1.Text;
            if (DateSpravka1.EditValue != null)
                formData.DateSpravka1 = DateTime.Parse(DateSpravka1.EditValue.ToString()).Date;
            else
                formData.DateSpravka1 = null;
            formData.SumPay = decimal.Parse(SumPay.EditValue.ToString());
            formData.SumPay_0 = decimal.Parse(SumPay_0.EditValue.ToString());
            formData.SumPay_1 = decimal.Parse(SumPay_1.EditValue.ToString());
            formData.SumPay_2 = decimal.Parse(SumPay_2.EditValue.ToString());
        }

        private void updateTextBoxes()
        {
            NumOrdSpin.Value = formData.NumOrd.Value;
            LastName.Text = formData.LastName;
            FirstName.Text = formData.FirstName;
            MiddleName.Text = formData.MiddleName;
            Insurance.Value = formData.Insurance;
            InsuranceCheck.Text = formData.CorrectionNum.HasValue ? formData.CorrectionNum.Value.ToString() : "";
            SumPay.EditValue = formData.SumPay.Value;
            SumPay_0.EditValue = formData.SumPay_0.Value;
            SumPay_1.EditValue = formData.SumPay_1.Value;
            SumPay_2.EditValue = formData.SumPay_2.Value;
            if (formData.DateSpravka.HasValue)
                DateSpravka.EditValue = formData.DateSpravka.Value;
            NumSpravka.Text = formData.NumSpravka;
            if (formData.DateSpravka1.HasValue)
                DateSpravka1.EditValue = formData.DateSpravka1.Value;
            NumSpravka1.Text = formData.NumSpravka1;

        }

        private void RSW2014_5_Edit_FormClosed(object sender, FormClosedEventArgs e)
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
        /// Проверка введенных данных Раздела 5
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            #region Проверяем на уникальность по порядковому номеру
            RSW2014_Edit main = this.Owner as RSW2014_Edit;
            List<FormsRSW2014_1_Razd_5> RSW_5_List = main.RSW_5_List;

            switch (action)
            {
                case "add":
                    if (RSW_5_List.Any(x => x.NumOrd == (long)NumOrdSpin.Value))
                    {
                        errMessBox.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumOrdSpin" });
                    }
                    break;
                case "edit":
                    if (RSW_5_List.Any(x => x.NumOrd == (long)NumOrdSpin.Value && x.ID != formData.ID))
                    {
                        errMessBox.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumOrdSpin" });
                    }
                    break;
            }

            #endregion

            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        private void Calculation()
        {

            SumPay.EditValue = decimal.Parse(SumPay_0.EditValue.ToString()) + decimal.Parse(SumPay_1.EditValue.ToString()) + decimal.Parse(SumPay_2.EditValue.ToString());
        }

        private void SumPay_2_Leave(object sender, EventArgs e)
        {
            Calculation();
        }

        private void Insurance_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Insurance.Text))
            {
                string insNum = Insurance.Text.Replace("-", "").Replace("_", "");
                if (insNum.Length == 9)
                {
                    if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insNum))
                    {
                        staffInfo = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insNum);
                        FirstName.Text = staffInfo.FirstName;
                        LastName.Text = staffInfo.LastName;
                        MiddleName.Text = staffInfo.MiddleName;
                    }
                    InsuranceCheck.Text = Utils.GetControlSumSSN(insNum);

                    if (Options.inputTypeRSW1 != 0)
                    {
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

                            if (db.FormsRSW2014_1_Razd_5.Any(x => x.Year == year && x.Quarter == quarter && x.Insurance == insNum && x.InsurerID == Options.InsID))
                                formDataPrev = db.FormsRSW2014_1_Razd_5.Where(x => x.Year == year && x.Quarter == quarter && x.Insurance == insNum && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                        }
                        else
                        {
                            formDataPrev = null;
                        }
                    }
                    else
                        formDataPrev = null;

                }
                else
                {
                    Insurance.Text = Insurance.NullText;
                    formDataPrev = null;
                }

            }
            else
            {
                formDataPrev = null;
            }

            Calculation();


        }


    }
}
