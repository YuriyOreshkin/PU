using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PU.Models;
using Telerik.WinControls;

namespace PU.Dictionaries
{
    public partial class TariffCodeRW3Edit : Telerik.WinControls.UI.RadForm
    {
        public string action;
        public CodeBaseRW3_2015 formData { get; set; }
        private bool setNull = true;

        public TariffCodeRW3Edit()
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

        private void TariffCodeRW3Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            switch (action)
            {
                case "add":
                    formData = new CodeBaseRW3_2015();
                    YearSpinEditor.Value = DateTime.Now.Year;
                    break;
                case "edit":
                    YearSpinEditor.Value = formData.Year;
                    Tar21MaskedEditBox.Value = formData.Tar21;
                    Tar22MaskedEditBox.Value = formData.Tar22;
                    break;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            formData.Year = (short)YearSpinEditor.Value;
            formData.Tar21 = decimal.Parse(Tar21MaskedEditBox.Value.ToString());
            formData.Tar22 = decimal.Parse(Tar22MaskedEditBox.Value.ToString());

            setNull = false;
            this.Close();
        }

        private void TariffCodeRW3Edit_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (setNull)
                formData = null;
        }



    }
}
