using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;

namespace PU.FormsRSW2014
{
    public partial class DictionEdit : Telerik.WinControls.UI.RadForm
    {
        public string action;
        public DictionEdit()
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

        private void DictionEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            radTextBox1.Text = "";
            radTextBox2.Text = "";
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DictionContainer dc = new DictionContainer()
            {
                Code = radTextBox1.Text,
                Name = radTextBox2.Text,
            };

            if (dateBegin.EditValue != null)
                dc.DateBegin = DateTime.Parse(dateBegin.EditValue.ToString()).Date;
            else
                dc.DateBegin = null;

            if (dateEnd.EditValue != null)
                dc.DateEnd = DateTime.Parse(dateEnd.EditValue.ToString()).Date;
            else
                dc.DateEnd = null;

            if (radDropDownList1.Visible)
            {
                Diction.SelfRef.id_izmer_ed = long.Parse(radDropDownList1.SelectedItem.Value.ToString());
                radLabel4.Visible = false;
                radDropDownList1.Visible = false;
            }
            string result = "";
            switch (action)
            {
                case "add":
                    result = Diction.SelfRef.add(dc);
                    break;
                case "edit":
                    result = Diction.SelfRef.edit(dc);
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

                RadMessageBox.Show("При сохранении данных произошла ошибка! " + result, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
            };
        }

        private void DictionEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

        }
    }
}
