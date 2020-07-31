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
    public partial class MROTEdit : Telerik.WinControls.UI.RadForm
    {
        public string action;
        MROT formData { get; set; }

        public MROTEdit()
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
            MROT mrot = new MROT()
            {
                Year = Convert.ToInt16(radSpinEditor1.Value),
                Mrot1 = decimal.Parse(radMaskedEditBox2.Text),
                NalogBase = decimal.Parse(radMaskedEditBox1.Text)

            };
            string result = "";
            switch (action)
            {
                case "add":
                    result = MROTFrm.SelfRef.add(mrot);
                    break;
                case "edit":
                    result = MROTFrm.SelfRef.edit(mrot);
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

        private void MROTEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            formData = null;
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MROTEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

        }
    }
}
