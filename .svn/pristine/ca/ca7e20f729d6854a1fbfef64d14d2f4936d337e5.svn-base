using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;

namespace PU.FormsODV1
{
    public partial class ODV_1_4_Edit : Telerik.WinControls.UI.RadForm
    {
        public FormsODV_1_4_2017 formData { get; set; }
        private bool setNull = true;
        public int cnt = 0;

        public ODV_1_4_Edit()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
        }

        private void ODV_1_4_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (setNull)
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

        private void ODV_1_4_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            if (formData == null)
            {
                formData = new FormsODV_1_4_2017();
                Year.Value = DateTime.Now.Year - 1;
            }
            else
            {
                Year.Value = (decimal)formData.Year;
                OPS.EditValue = formData.OPS.HasValue ? formData.OPS.Value : (decimal)0;
                NAKOP.EditValue = formData.NAKOP.HasValue ? formData.NAKOP.Value : (decimal)0;
                DopTar.EditValue = formData.DopTar.HasValue ? formData.DopTar.Value : (decimal)0;

            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            formData.Year = (short)Year.Value;
            formData.OPS = Math.Round(decimal.Parse(OPS.Text), 2, MidpointRounding.AwayFromZero);
            formData.NAKOP = Math.Round(decimal.Parse(NAKOP.Text), 2, MidpointRounding.AwayFromZero);
            formData.DopTar = Math.Round(decimal.Parse(DopTar.Text), 2, MidpointRounding.AwayFromZero);

            setNull = false;
            this.Close();
        }


    }
}
