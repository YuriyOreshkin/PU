using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using PU.FormsRSW2014;
using PU.Models;

namespace PU.FormsRSW2014
{
    public partial class DocTypesEdit : Telerik.WinControls.UI.RadForm
    {
        public string action = "";
        public DocTypesEdit()
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

        private void DocTypesEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            radTextBox1.Text = "";
            radTextBox2.Text = "";
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DocumentTypes dt = new DocumentTypes()
            {
                Code = radTextBox1.Text,
                Name = radTextBox2.Text
            };
            string result = DocTypes.SelfRef.add(dt, action);

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
                RadMessageBox.Show(this, "При сохранении данных произошла ошибка!", "Ошибка");
            };
        }

        private void DocTypesEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

        }
    }
}
