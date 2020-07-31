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
    public partial class PlatCategoryFrmEdit : Telerik.WinControls.UI.RadForm
    {
        public string action;
        public PlatCategory formData { get; set; }
        public PlatCategoryFrmEdit()
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
            PlatCategory pc = new PlatCategory
            {
                Code = radTextBox1.Text,
                Name = radTextBox2.Text,
                FullName = radTextBox3.Text
            };

            if (dateBegin.EditValue != null)
                pc.DateBegin = DateTime.Parse(dateBegin.EditValue.ToString()).Date;
            else
                pc.DateBegin = null;

            if (dateEnd.EditValue != null)
                pc.DateEnd = DateTime.Parse(dateEnd.EditValue.ToString()).Date;
            else
                pc.DateEnd = null;

            string result = "";
            switch (action)
            {
                case "add":
                    result = PlatCategoryFrm.SelfRef.add(pc);
                    break;
                case "edit":
                    result = PlatCategoryFrm.SelfRef.edit(pc);
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

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void PlatCategoryFrmEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            switch (action)
            {
                case "add":
                    formData = new PlatCategory();
                    radTextBox1.ReadOnly = false;
                    break;
                case "edit":
                    radTextBox1.Text = formData.Code;
                    radTextBox2.Text = formData.Name;
                    radTextBox3.Text = formData.FullName;
                    if (formData.DateBegin.HasValue)
                        dateBegin.EditValue = formData.DateBegin.Value;
                    if (formData.DateEnd.HasValue)
                        dateEnd.EditValue = formData.DateEnd.Value;

                    break;
            }
        }
    }
}
