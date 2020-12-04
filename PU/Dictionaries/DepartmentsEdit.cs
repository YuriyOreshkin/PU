using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using PU.Classes;

namespace PU.Dictionaries
{
    public partial class DepartmentsEdit : Telerik.WinControls.UI.RadForm
    {
        public string action;
        public long ParId = 0;
        public long InsID = 0;   // ID страхователя
        public DepartmentsEdit()
        {
            InitializeComponent();
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    radButton1.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    //this.Dispose();
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


        private void DepartmentsEdit_FormClosing(object sender, FormClosingEventArgs e)
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
            if (!String.IsNullOrEmpty(radTextBox2.Text))
            {
                Department dep = new Department()
                {
                    Code = radTextBox1.Text,
                    Name = radTextBox2.Text,
                    ParentID = ParId == 0 ? (long?)null : ParId,
                    InsurerID = InsID
                };


                string result = "";
                switch (action)
                {
                    case "add":
                        result = DepartmentsFrm.SelfRef.add(dep);
                        break;
                    case "edit":
                        result = DepartmentsFrm.SelfRef.edit(dep);
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
            else
            {
                Messenger.showAlert(AlertType.Info, "Внимание!", "Укажите наименование подразделения!", this.ThemeName);
                radTextBox2.Focus();
            }
        }

        private void DepartmentsEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            checkAccessLevel();
        }

 /*       private void radTextBox1_Validating(object sender, CancelEventArgs e)
        {
            string error = null;
            if (radTextBox1.Text.Length == 0)
            {
                error = "Please enter a name";
                e.Cancel = true;
            }
            errorProvider1.SetError((Control)sender, error);
        }
  */
    }
}
