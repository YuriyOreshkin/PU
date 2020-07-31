using PU.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace PU.Service.CheckFiles
{
    public partial class CheckFilesQuestion : Telerik.WinControls.UI.RadForm
    {
        public List<string> FileInfoList = new List<string>();

        public CheckFilesQuestion()
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

        private void noBtn_Click(object sender, EventArgs e)
        {
            if (radCheckBox1.Checked)
                Options.checkFilesAfterSaving = false;

            this.Close();
        }


        private void yesBtn_Click(object sender, EventArgs e)
        {
            if (radCheckBox1.Checked)
                Options.checkFilesAfterSaving = true;

            PU.FormsRSW2014.rsw2014packs main = this.Owner as PU.FormsRSW2014.rsw2014packs;

            CheckFilesList child = new CheckFilesList();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.FileInfoList = FileInfoList;
            this.Hide();
            child.ShowDialog();
            this.Close();
        }

        private void CheckFilesQuestion_FormClosing(object sender, FormClosingEventArgs e)
        {
            Options.hideDialogCheckFiles = radCheckBox1.Checked;
        }

        private void CheckFilesQuestion_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            if (Options.checkFilesAfterSaving)
            {
                yesBtn.Select();
            }
            else
            {
                noBtn.Select();
            }
        }
    }
}
