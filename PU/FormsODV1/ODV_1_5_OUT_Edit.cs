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
    public partial class ODV_1_5_OUT_Edit : Telerik.WinControls.UI.RadForm
    {
        public FormsODV_1_5_2017_OUT formData { get; set; }
        private bool setNull = true;

        public ODV_1_5_OUT_Edit()
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

        private void OsobUslTrudaCodeCleanBtn_Click(object sender, EventArgs e)
        {
            OsobUslTrudaCode.Text = "";
        }

        private void CodePositionCleanBtn_Click(object sender, EventArgs e)
        {
            CodePosition.Text = "";
        }

        private void OsobUslTrudaCodeBtn_Click(object sender, EventArgs e)
        {
           /* PU.Dictionaries.BaseDictionaryFormList child = new PU.Dictionaries.BaseDictionaryFormList();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.DictName = "OsobUslTruda";
            child.radButtonSelect.Visible = true;
            child.ShowDialog();
            OsobUslTrudaCode.Text = child.Code;
            child = null;*/
        }

        private void UslDosrNaznCodeBtn_Click(object sender, EventArgs e)
        {
            /*PU.Dictionaries.BaseDictionaryFormList child = new PU.Dictionaries.BaseDictionaryFormList();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.DictName = "UslDosrNazn";
            child.radButtonSelect.Visible = true;
            child.ShowDialog();
            OsobUslTrudaCode.Text = child.Code;
            child = null;*/
        }

        private void CodePositionBtn_Click(object sender, EventArgs e)
        {
            PU.FormsRSW2014.KodVred child = new PU.FormsRSW2014.KodVred();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.ddl1_index = OsobUslTrudaCode.Text == "27-1" ? (byte)0 : (byte)1;
            child.btnSelection.Visible = true;
            child.ShowDialog();
            if (child.kv_osn != null)
            {
                CodePosition.Text = child.kv_osn.Code;
            }
            child = null;
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
        }

        private void ODV_1_5_OUT_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (setNull)
                formData = null;
        }

        private void ODV_1_5_OUT_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            if (formData == null)
            {
                formData = new FormsODV_1_5_2017_OUT();
            }
            else
            {
                OsobUslTrudaCode.Text = formData.OsobUslTrudaCode;
                CodePosition.Text = formData.CodePosition;
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(OsobUslTrudaCode.Text))
            {
                RadMessageBox.Show(this, "Следующие поля должны быть заполнены:\r\nКод особых условий труда/выслуги лет по Классификатору", "Внимание!");
                return;
            }

            formData.OsobUslTrudaCode = OsobUslTrudaCode.Text;
            formData.CodePosition = CodePosition.Text;

            setNull = false;
            this.Close();
        }
    }
}
