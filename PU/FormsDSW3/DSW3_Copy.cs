using PU.Classes;
using PU.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;

namespace PU.FormsDSW3
{
    public partial class DSW3_Copy : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        private FormsDSW_3 DSW3source { get; set; }
        public long dsw3ID = 0;

        public DSW3_Copy()
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

        private void DSW3_Copy_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            DSW3source = db.FormsDSW_3.FirstOrDefault(x => x.ID == dsw3ID);

            Year.Value = DSW3source.YEAR;
            NUMBERPAYMENT.Text = DSW3source.NUMBERPAYMENT;

            if (DSW3source.DATEPAYMENT != null)
                DATEPAYMENT.Value = DSW3source.DATEPAYMENT;
            if (DSW3source.DATEEXECUTPAYMENT != null)
                DATEEXECUTPAYMENT.Value = DSW3source.DATEEXECUTPAYMENT;
            if (DSW3source.DateFilling != null)
                DateFilling.Value = DSW3source.DateFilling;

        }

        private void DATEPAYMENT_ValueChanged(object sender, EventArgs e)
        {
            if (DATEPAYMENT.Value != DATEPAYMENT.NullDate)
                DATEPAYMENTMaskedEditBox.Text = DATEPAYMENT.Value.ToShortDateString();
            else
                DATEPAYMENTMaskedEditBox.Text = DATEPAYMENTMaskedEditBox.NullText;
        }

        private void DATEPAYMENTMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DATEPAYMENTMaskedEditBox.Text != DATEPAYMENTMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DATEPAYMENTMaskedEditBox.Text, out date))
                {
                    DATEPAYMENT.Value = date;
                }
                else
                {
                    DATEPAYMENT.Value = DATEPAYMENT.NullDate;
                }
            }
            else
            {
                DATEPAYMENT.Value = DATEPAYMENT.NullDate;
            }
        }

        private void DATEEXECUTPAYMENT_ValueChanged(object sender, EventArgs e)
        {
            if (DATEEXECUTPAYMENT.Value != DATEEXECUTPAYMENT.NullDate)
                DATEEXECUTPAYMENTMaskedEditBox.Text = DATEEXECUTPAYMENT.Value.ToShortDateString();
            else
                DATEEXECUTPAYMENTMaskedEditBox.Text = DATEEXECUTPAYMENTMaskedEditBox.NullText;
        }

        private void DATEEXECUTPAYMENTMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DATEEXECUTPAYMENTMaskedEditBox.Text != DATEEXECUTPAYMENTMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DATEEXECUTPAYMENTMaskedEditBox.Text, out date))
                {
                    DATEEXECUTPAYMENT.Value = date;
                }
                else
                {
                    DATEEXECUTPAYMENT.Value = DATEEXECUTPAYMENT.NullDate;
                }
            }
            else
            {
                DATEEXECUTPAYMENT.Value = DATEEXECUTPAYMENT.NullDate;
            }
        }

        private void DateFilling_ValueChanged(object sender, EventArgs e)
        {
            if (DateFilling.Value != DateFilling.NullDate)
                DateFillingMaskedEditBox.Text = DateFilling.Value.ToShortDateString();
            else
                DateFillingMaskedEditBox.Text = DateFillingMaskedEditBox.NullText;
        }

        private void DateFillingMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateFillingMaskedEditBox.Text != DateFillingMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateFillingMaskedEditBox.Text, out date))
                {
                    DateFilling.Value = date;
                }
                else
                {
                    DateFilling.Value = DateFilling.NullDate;
                }
            }
            else
            {
                DateFilling.Value = DateFilling.NullDate;
            }
        }





        private void DATEPAYMENT_New_ValueChanged(object sender, EventArgs e)
        {
            if (DATEPAYMENT_New.Value != DATEPAYMENT.NullDate)
                DATEPAYMENTMaskedEditBox_New.Text = DATEPAYMENT_New.Value.ToShortDateString();
            else
                DATEPAYMENTMaskedEditBox_New.Text = DATEPAYMENTMaskedEditBox_New.NullText;
        }

        private void DATEPAYMENTMaskedEditBox_New_Leave(object sender, EventArgs e)
        {
            if (DATEPAYMENTMaskedEditBox_New.Text != DATEPAYMENTMaskedEditBox_New.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DATEPAYMENTMaskedEditBox_New.Text, out date))
                {
                    DATEPAYMENT_New.Value = date;
                }
                else
                {
                    DATEPAYMENT_New.Value = DATEPAYMENT_New.NullDate;
                }
            }
            else
            {
                DATEPAYMENT_New.Value = DATEPAYMENT_New.NullDate;
            }
        }

        private void DATEEXECUTPAYMENT_New_ValueChanged(object sender, EventArgs e)
        {
            if (DATEEXECUTPAYMENT_New.Value != DATEEXECUTPAYMENT_New.NullDate)
                DATEEXECUTPAYMENTMaskedEditBox_New.Text = DATEEXECUTPAYMENT_New.Value.ToShortDateString();
            else
                DATEEXECUTPAYMENTMaskedEditBox_New.Text = DATEEXECUTPAYMENTMaskedEditBox_New.NullText;
        }

        private void DATEEXECUTPAYMENTMaskedEditBox_New_Leave(object sender, EventArgs e)
        {
            if (DATEEXECUTPAYMENTMaskedEditBox_New.Text != DATEEXECUTPAYMENTMaskedEditBox_New.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DATEEXECUTPAYMENTMaskedEditBox_New.Text, out date))
                {
                    DATEEXECUTPAYMENT_New.Value = date;
                }
                else
                {
                    DATEEXECUTPAYMENT_New.Value = DATEEXECUTPAYMENT_New.NullDate;
                }
            }
            else
            {
                DATEEXECUTPAYMENT_New.Value = DATEEXECUTPAYMENT_New.NullDate;
            }
        }

        private void DateFilling_New_ValueChanged(object sender, EventArgs e)
        {
            if (DateFilling_New.Value != DateFilling_New.NullDate)
                DateFillingMaskedEditBox_New.Text = DateFilling_New.Value.ToShortDateString();
            else
                DateFillingMaskedEditBox_New.Text = DateFillingMaskedEditBox_New.NullText;
        }

        private void DateFillingMaskedEditBox_New_Leave(object sender, EventArgs e)
        {
            if (DateFillingMaskedEditBox_New.Text != DateFillingMaskedEditBox_New.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateFillingMaskedEditBox_New.Text, out date))
                {
                    DateFilling_New.Value = date;
                }
                else
                {
                    DateFilling_New.Value = DateFilling_New.NullDate;
                }
            }
            else
            {
                DateFilling_New.Value = DateFilling_New.NullDate;
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copyBtn_Click(object sender, EventArgs e)
        {


            if (validation())
            {
                FormsDSW_3 DSW3Data = new FormsDSW_3
                {
                    InsurerID = Options.InsID,
                    YEAR = (short)Year_New.Value,
                    NUMBERPAYMENT = NUMBERPAYMENT_New.Text.Trim(),
                    DATEPAYMENT = DATEPAYMENT_New.Value,
                    DATEEXECUTPAYMENT = DATEEXECUTPAYMENT_New.Value,
                    DepartmentID = DSW3source.DepartmentID != null ? DSW3source.DepartmentID : null,
                    DateFilling = DateFilling_New.Value
                };


                var staffDSW3list = DSW3source.FormsDSW_3_Staff.ToList();

                foreach (var item in staffDSW3list)
                {
                    DSW3Data.FormsDSW_3_Staff.Add(new FormsDSW_3_Staff { StaffID = item.StaffID, SUMFEEPFR_EMPLOYERS = item.SUMFEEPFR_EMPLOYERS, SUMFEEPFR_PAYER = item.SUMFEEPFR_PAYER});
                }

                db.FormsDSW_3.Add(DSW3Data);
                db.SaveChanges();
                Methods.showAlert("Успех", "Реестр ДСВ-3 успешно скопирован!", this.ThemeName);
                this.Close();
            }

        }

        private bool validation()
        {
            if (String.IsNullOrEmpty(NUMBERPAYMENT_New.Text.Trim()))
            {
                Methods.showAlert("Внимание", "Необходимо указать номер поручения", this.ThemeName);
                return false;
            }

            if (DATEPAYMENT_New.Value == DATEPAYMENT_New.NullDate)
            {
                Methods.showAlert("Внимание", "Необходимо указать дату поручения", this.ThemeName);
                return false;
            }
            if (DATEEXECUTPAYMENT_New.Value == DATEEXECUTPAYMENT_New.NullDate)
            {
                Methods.showAlert("Внимание", "Необходимо указать дату исполнения", this.ThemeName);
                return false;
            }
            if (DateFilling_New.Value == DateFilling_New.NullDate)
            {
                Methods.showAlert("Внимание", "Необходимо указать дату заполнения Реестра", this.ThemeName);
                return false;
            }


            if (Year.Value == Year_New.Value && NUMBERPAYMENT.Text.Trim() == NUMBERPAYMENT_New.Text.Trim() && DATEPAYMENT.Value == DATEPAYMENT_New.Value)
            {
                Methods.showAlert("Внимание", "Реестр назначения не может совпадать с исходный реестром!", this.ThemeName);
                return false;
            }
            if (db.FormsDSW_3.Any(x => x.InsurerID == Options.InsID && x.DepartmentID == DSW3source.DepartmentID && x.ID != DSW3source.ID && x.YEAR == Year_New.Value && x.NUMBERPAYMENT == NUMBERPAYMENT_New.Text.Trim() && x.DATEPAYMENT == DATEPAYMENT.Value))
            {
                Methods.showAlert("Внимание", "Дублирование по ключу уникальности! Выберите другие параметры для реестра назначения!", this.ThemeName);
                return false;
            }



            return true;

        }



    }
}
