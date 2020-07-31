using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;

namespace PU.FormsSZV_STAJ
{
    public partial class SZV_STAJ_5_Edit : Telerik.WinControls.UI.RadForm
    {
        public FormsSZV_STAJ_4_2017 formData { get; set; }
        private bool setNull = true;
        public int cnt = 0;

        public SZV_STAJ_5_Edit()
        {
            InitializeComponent();
        }

        private void DNPO_1_DateFrom_ValueChanged(object sender, EventArgs e)
        {
            if (DNPO_DateFrom.Value != DNPO_DateFrom.NullDate)
                DNPO_DateFromMaskedEditBox.Text = DNPO_DateFrom.Value.ToShortDateString();
            else
                DNPO_DateFromMaskedEditBox.Text = DNPO_DateFromMaskedEditBox.NullText;
        }

        private void DNPO_1_DateFromMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DNPO_DateFromMaskedEditBox.Text != DNPO_DateFromMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DNPO_DateFromMaskedEditBox.Text, out date))
                {
                    DNPO_DateFrom.Value = date;
                }
                else
                {
                    DNPO_DateFrom.Value = DNPO_DateFrom.NullDate;
                }
            }
            else
            {
                DNPO_DateFrom.Value = DNPO_DateFrom.NullDate;
            }
        }

        private void DNPO_1_DateTo_ValueChanged(object sender, EventArgs e)
        {
            if (DNPO_DateTo.Value != DNPO_DateTo.NullDate)
                DNPO_DateToMaskedEditBox.Text = DNPO_DateTo.Value.ToShortDateString();
            else
                DNPO_DateToMaskedEditBox.Text = DNPO_DateToMaskedEditBox.NullText;
        }

        private void DNPO_1_DateToMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DNPO_DateToMaskedEditBox.Text != DNPO_DateToMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DNPO_DateToMaskedEditBox.Text, out date))
                {
                    DNPO_DateTo.Value = date;
                }
                else
                {
                    DNPO_DateTo.Value = DNPO_DateTo.NullDate;
                }
            }
            else
            {
                DNPO_DateTo.Value = DNPO_DateTo.NullDate;
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
        }

        private void SZV_STAJ_5_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            if (formData == null)
            {
                formData = new FormsSZV_STAJ_4_2017();
            }
            else
            {
                if (formData.DNPO_DateFrom.HasValue)
                    DNPO_DateFrom.Value = formData.DNPO_DateFrom.Value;

                if (formData.DNPO_DateTo.HasValue)
                    DNPO_DateTo.Value = formData.DNPO_DateTo.Value;
                DNPO_1_Fee.Checked = formData.DNPO_Fee.HasValue ? formData.DNPO_Fee.Value : false;

            }
        }

        private void SZV_STAJ_5_Edit_FormClosing(object sender, FormClosingEventArgs e)
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

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (!DNPO_DateFromMaskedEditBox.Text.Contains("_"))
                formData.DNPO_DateFrom = DNPO_DateFrom.Value;

            if (!DNPO_DateToMaskedEditBox.Text.Contains("_"))
                formData.DNPO_DateTo = DNPO_DateTo.Value;

            formData.DNPO_Fee = DNPO_1_Fee.Checked;

            setNull = false;
            this.Close();
        }

        private void DNPO_1_Fee_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            DNPO_1_Fee.Text = DNPO_1_Fee.Checked ? "Да" : "Нет";
        }


    }
}
