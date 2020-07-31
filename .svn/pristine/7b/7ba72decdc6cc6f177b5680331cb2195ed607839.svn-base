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

namespace PU.FormsRSW2014
{
    public partial class RSW2014_3_4_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool cleanData = true;

        public FormsRSW2014_1_Razd_3_4 formData { get; set; }

        public RSW2014_3_4_Edit()
        {
            InitializeComponent();
        }

        private void RSW2014_3_4_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            switch (action)
            {
                case "add":
                    break;
                case "edit":
                    updateTextBoxes();

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

        private void getValues()
        {
            formData.NumOrd = (long)NumOrdSpin.Value;
            formData.OKWED = OKWEDTextBox.Text;
            formData.NameOKWED = NameOKWEDTextBox.Text;
            formData.Income = decimal.Parse(IncomeMaskedEditBox.Value.ToString());
            formData.RateIncome = decimal.Parse(RateIncomeMaskedEditBox.Value.ToString());
        }

        private void updateTextBoxes()
        {
            OKWEDTextBox.Text = formData.OKWED;
            NameOKWEDTextBox.Text = formData.NameOKWED;
            NumOrdSpin.Value = formData.NumOrd.Value;
            IncomeMaskedEditBox.Value = formData.Income.Value;
            RateIncomeMaskedEditBox.Value = formData.RateIncome.Value;
        }

        private void RSW2014_3_4_Edit_FormClosed(object sender, FormClosedEventArgs e)
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
            if (String.IsNullOrEmpty(OKWEDTextBox.Text))
            {
                OKWEDTextBox.Focus();
            }
            else
            {
                if (String.IsNullOrEmpty(NameOKWEDTextBox.Text))
                {
                    NameOKWEDTextBox.Focus();
                }
                else
                {
                    List<ErrList> errMessBox = new List<ErrList>();
                    RSW2014_Edit main = this.Owner as RSW2014_Edit;
                    List<FormsRSW2014_1_Razd_3_4> RSW_3_4_List = main.RSW_3_4_List;

                    switch (action)
                    {
                        case "add":
                            if (RSW_3_4_List.Any(x => x.NumOrd == (long)NumOrdSpin.Value))
                            {
                                errMessBox.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumOrdSpin" });
                            }
                            break;
                        case "edit":
                            if (RSW_3_4_List.Any(x => x.NumOrd == (long)NumOrdSpin.Value && x.ID != formData.ID))
                            {
                                errMessBox.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumOrdSpin" });
                            }
                            break;
                    }

                    if (errMessBox.Count != 0)
                    {
                        RadMessageBox.Show(this, errMessBox[0].name);
                    }
                    else
                    {
                        getValues();
                        cleanData = false;
                        this.Close();
                    }

                }
            }
        }




    }
}
