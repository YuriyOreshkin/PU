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
    public partial class RaschetPeriodFrmEdit : Telerik.WinControls.UI.RadForm
    {
        public string action;
        public RaschetPeriodFrmEdit()
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

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            RaschetPeriod rp = new RaschetPeriod
            {
                Year = Convert.ToInt16(radSpinEditor1.Value),
                Kvartal = Convert.ToByte(radSpinEditor2.Value),
                Name = radTextBox1.Text
            };

            if (dateBegin.EditValue != null)
                rp.DateBegin = DateTime.Parse(dateBegin.EditValue.ToString()).Date;
            else
                rp.DateBegin = null;

            if (dateEnd.EditValue != null)
                rp.DateEnd = DateTime.Parse(dateEnd.EditValue.ToString()).Date;
            else
                rp.DateEnd = null;


            string result = "";
            switch (action)
            {
                case "add":
                    result = RaschetPeriodFrm.SelfRef.add(rp);
                    break;
                case "edit":
                    result = RaschetPeriodFrm.SelfRef.edit(rp);
                    break;
            }


            if (result == "")
            {
                this.Close();
            }
            else
            {
                RadMessageBox.Show("При сохранении данных произошла ошибка! " + result, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            };
        }

        private void RaschetPeriodFrmEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

        }
    }
}
