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
    public partial class DopTariffFrmEdit : Telerik.WinControls.UI.RadForm
    {
        public string action;
        public DopTariffFrmEdit()
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
            DopTariff dt = new DopTariff()
            {
                Year = Convert.ToInt16(radSpinEditor1.Value),
                Tariff1 = decimal.Parse(radMaskedEditBox1.Text),
                Tariff2 = decimal.Parse(radMaskedEditBox2.Text)

            };
            string result = "";
            switch (action)
            {
                case "add":
                    result = DopTariffFrm.SelfRef.add(dt);
                    break;
                case "edit":
                    result = DopTariffFrm.SelfRef.edit(dt);
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

        private void DopTariffFrmEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            radSpinEditor1.ResetText();
            radMaskedEditBox1.ResetText();
            radMaskedEditBox2.ResetText();
        }

        private void DopTariffFrmEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

        }
    }
}
