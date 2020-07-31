using PU.Models;
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
using Telerik.WinControls.UI;

namespace PU.FormsRW_3_2015
{
    public partial class RW3_2015_3_Edit : Telerik.WinControls.UI.RadForm
    {
        public string action { get; set; }
        private bool setNull = true;

        public FormsRW3_2015_Razd_3 formData { get; set; }

        public RW3_2015_3_Edit()
        {
            InitializeComponent();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
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

        private void RW3_2015_3_Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (setNull)
                formData = null;
        }

        private void RW3_2015_3_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);


            switch (action)
            {
                case "add":
                    formData = new FormsRW3_2015_Razd_3();
                    break;
                case "edit":
                    YearSpin.Value = formData.Year.Value;
                    MonthSpin.Value = formData.Month.Value;
                    sumFee.EditValue = formData.SumFee.Value;
                    CodeBaseDDL.Items[formData.CodeBase.Value - 1].Selected = true;
                    break;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            formData.CodeBase = (byte)(CodeBaseDDL.SelectedIndex + 1);
            formData.Year = (short)YearSpin.Value;
            formData.Month = (byte)MonthSpin.Value;
            formData.SumFee = decimal.Parse(sumFee.EditValue.ToString());

            setNull = false;
            this.Close();
        }



    }
}
