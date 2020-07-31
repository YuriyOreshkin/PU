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

namespace PU.FormsRSW2014
{
    public partial class TariffPlatFrmEdit : Telerik.WinControls.UI.RadForm
    {
        public string action;

        public TariffPlatFrmEdit()
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

        private void radButton1_Click(object sender, EventArgs e)
        {
            TariffPlat dt = new TariffPlat()
            {
                Year = Convert.ToInt16(radSpinEditor1.Value),
                StrahPercant1966 = decimal.Parse(radMaskedEditBox1.Text),
                StrahPercent1967 = decimal.Parse(radMaskedEditBox2.Text),
                NakopPercant = decimal.Parse(radMaskedEditBox3.Text),
                FFOMS_Percent = decimal.Parse(radMaskedEditBox4.Text),
                TFOMS_Percent = decimal.Parse(radMaskedEditBox5.Text)

            };
            string result = "";
            switch (action)
            {
                case "add":
                    result = TariffPlatFrm.SelfRef.add(dt);
                    break;
                case "edit":
                    result = TariffPlatFrm.SelfRef.edit(dt);
                    break;
            }


            if (result == "")
            {
                /*       DocTypes main = this.Owner as DocTypes;
                       if (main != null)
                       {
                           main.label1.Text = "Test";
                    
                       }
                 * */
                this.Close();
            }
            else
            {
                RadMessageBox.Show(this, "При сохранении данных произошла ошибка! " + result, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            };
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TariffPlatFrmEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            radSpinEditor1.ResetText();
            radMaskedEditBox1.ResetText();
            radMaskedEditBox2.ResetText();
            radMaskedEditBox3.ResetText();
            radMaskedEditBox4.ResetText();
            radMaskedEditBox5.ResetText();
        }

        private void radMaskedEditBox4_Click(object sender, EventArgs e)
        {

        }

        private void TariffPlatFrmEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

        }
    }
}
