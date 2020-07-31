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
    public partial class RSW2014_4_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool cleanData = true;
        private List<ErrList> errMessBox = new List<ErrList>();
        public short yearType = 2014;

        public FormsRSW2014_1_Razd_4 formData { get; set; }

        public RSW2014_4_Edit()
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

        private void RSW2014_4_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            changeFormByYear();

            switch (action)
            {
                case "add":
                    NumOrdSpin.Value = formData.NumOrd.HasValue ? formData.NumOrd.Value : 1;
                    break;
                case "edit":
                    updateTextBoxes();
                    break;
            }

            YearPerSpin_ValueChanged(null, null);
            CodeBaseDDL_SelectedIndexChanged(null, null);
        }


        public void changeFormByYear()
        {
            BaseRadio4.Visible = yearType == 2015 ? true : false;
            radGroupBox1.Text = yearType == 2015 ? "Основание для перерасчета страховых взносов" : "Основание для доначисления страховых взносов";
            radGroupBox2.Text = yearType == 2015 ? "Период, за который производится перерасчет страховых взносов" : "Период, за который доначислены страховые взносы";

        }

        private void getValues()
        {
            formData.NumOrd = (long)NumOrdSpin.Value;
            if (BaseRadio1.IsChecked)
                formData.Base = 1;
            else if (BaseRadio2.IsChecked)
                formData.Base = 2;
            else if (BaseRadio3.IsChecked)
                formData.Base = 3;
            else if (BaseRadio4.IsChecked)
                formData.Base = 4;

            formData.YearPer = (short)YearPerSpin.Value;
            formData.MonthPer = byte.Parse(MonthPerDDL.SelectedItem.Tag.ToString());
            formData.CodeBase = byte.Parse(CodeBaseDDL.SelectedItem.Tag.ToString());
            formData.Strah2014 = !String.IsNullOrEmpty(Strah2014.Text) ? decimal.Parse(Strah2014.Text) : 0;
            formData.StrahMoreBase2014 = !String.IsNullOrEmpty(StrahMoreBase2014.Text) ? decimal.Parse(StrahMoreBase2014.Text) : 0;
            formData.Strah2013 = !String.IsNullOrEmpty(Strah2013.Text) ? decimal.Parse(Strah2013.Text) : 0;
            formData.StrahMoreBase2013 = !String.IsNullOrEmpty(StrahMoreBase2013.Text) ? decimal.Parse(StrahMoreBase2013.Text) : 0;
            formData.Nakop2013 = !String.IsNullOrEmpty(Nakop2013.Text) ? decimal.Parse(Nakop2013.Text) : 0;
            formData.Dop1 = !String.IsNullOrEmpty(Dop1.Text) ? decimal.Parse(Dop1.Text) : 0;
            formData.Dop2 = !String.IsNullOrEmpty(Dop2.Text) ? decimal.Parse(Dop2.Text) : 0;
            formData.Dop21 = !String.IsNullOrEmpty(Dop21.Text) ? decimal.Parse(Dop21.Text) : 0;
            formData.OMS = !String.IsNullOrEmpty(OMS.Text) ? decimal.Parse(OMS.Text) : 0;
        }

        private void updateTextBoxes()
        {
            NumOrdSpin.Value = formData.NumOrd.Value;
            switch (formData.Base)
            {
                case 1:
                    BaseRadio1.IsChecked = true;
                    break;
                case 2:
                    BaseRadio2.IsChecked = true;
                    break;
                case 3:
                    BaseRadio3.IsChecked = true;
                    break;
                case 4:
                    BaseRadio4.IsChecked = true;
                    break;
            }
            YearPerSpin.Value = formData.YearPer.Value;
            MonthPerDDL.SelectedItem = MonthPerDDL.Items.FirstOrDefault(x => x.Tag.ToString() == formData.MonthPer.Value.ToString());
            CodeBaseDDL.SelectedItem = CodeBaseDDL.Items.FirstOrDefault(x => x.Tag.ToString() == formData.CodeBase.Value.ToString());
            Strah2014.EditValue = formData.Strah2014.Value;
            StrahMoreBase2014.EditValue = formData.StrahMoreBase2014.Value;
            Strah2013.EditValue = formData.Strah2013.Value;
            StrahMoreBase2013.EditValue = formData.StrahMoreBase2013.Value;
            Nakop2013.EditValue = formData.Nakop2013.Value;
            Dop1.EditValue = formData.Dop1.Value;
            Dop2.EditValue = formData.Dop2.Value;
            Dop21.EditValue = formData.Dop21.Value;
            OMS.EditValue = formData.OMS.Value;

        }

        private void RSW2014_4_Edit_FormClosed(object sender, FormClosedEventArgs e)
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

        private void YearPerSpin_ValueChanged(object sender, EventArgs e)
        {
            bool flag = true;
            if (YearPerSpin.Value < 2014)
            {
                flag = false;
                Strah2014.ResetText();
                StrahMoreBase2014.ResetText();
                Dop21.ResetText();
                Dop21.Enabled = flag;
                Dop21Lab.Enabled = flag;
            }
            else
            {
                Strah2013.ResetText();
                StrahMoreBase2013.ResetText();
                Nakop2013.ResetText();

                if (CodeBaseDDL.SelectedItem.Tag.ToString() != "0")
                {
                    Dop21.Enabled = flag;
                    Dop21Lab.Enabled = flag;
                }
            }

            Strah2014.Enabled = flag;
            Strah2014Lab.Enabled = flag;
            StrahMoreBase2014Lab.Enabled = flag;
            StrahMoreBase2014.Enabled = flag;





            Strah2013.Enabled = !flag;
            Strah2013Lab.Enabled = !flag;
            StrahMoreBase2013.Enabled = !flag;
            StrahMoreBase2013Lab.Enabled = !flag;
            Nakop2013.Enabled = !flag;
            Nakop2013Lab.Enabled = !flag;


        }

        /// <summary>
        /// Проверка введенных данных Раздела 4
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            #region Проверяем на уникальность по коду тарифа
            RSW2014_Edit main = this.Owner as RSW2014_Edit;
            List<FormsRSW2014_1_Razd_4> RSW_4_List = main.RSW_4_List;

            switch (action)
            {
                case "add":
                    if (RSW_4_List.Any(x => x.NumOrd == (long)NumOrdSpin.Value))
                    {
                        errMessBox.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumOrdSpin" });
                    }
                    break;
                case "edit":
                    if (RSW_4_List.Any(x => x.NumOrd == (long)NumOrdSpin.Value && x.ID != formData.ID))
                    {
                        errMessBox.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumOrdSpin" });
                    }
                    break;
            }

            #endregion

    /*        #region Раздел 4   2
            if ((decimal.Parse(Strah2014.EditValue.ToString()) < decimal.Parse(StrahMoreBase2014.EditValue.ToString())))
                errMessBox.Add(new ErrList { name = "Сумма страховых взносов за 2014 год должна быть больше либо равна сумме превышающей базу", control = "Strah2014" });
            #endregion
            #region Раздел 4   3
            if ((decimal.Parse(Strah2013.EditValue.ToString()) < decimal.Parse(StrahMoreBase2013.EditValue.ToString())))
                errMessBox.Add(new ErrList { name = "Сумма страховых взносов за 2010-2013 гг. должна быть больше либо равна сумме превышающей базу", control = "Strah2013" });
            #endregion
            */
            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        private void CodeBaseDDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (CodeBaseDDL.SelectedItem.Tag.ToString() == "0")
            {
                Dop21.ResetText();
                Dop21.Enabled = false;
                Dop21Lab.Enabled = false;
            }
            else
            {
                if (YearPerSpin.Value >= 2014)
                {
                    Dop21.Enabled = true;
                    Dop21Lab.Enabled = true;
                }
            }
        }


    }
}
