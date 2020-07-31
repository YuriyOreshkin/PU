using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using PU.Classes;
using Telerik.WinControls;

namespace PU.FormsDSW3
{
    public partial class DSW3_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public string action { get; set; }
        public long DepID = 0;// ID подразделения
        public FormsDSW_3 DSW3Data = new FormsDSW_3();
        public long DSW3ID = 0;
        private bool cleanData = true;
        private List<ErrList> errMessBox = new List<ErrList>();

        public DSW3_Edit()
        {
            InitializeComponent();
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    saveBtn.Enabled = false;
                    selectDepBtn.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    break;
            }
        }

        public static DSW3_Edit SelfRef
        {
            get;
            set;
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
                closeBtn_Click(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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

        private void cleanDepBtn_Click(object sender, EventArgs e)
        {
            DepID = 0;
            DepCode_.Text = "";
            DepName_.Text = "";
        }

        private void selectDepBtn_Click(object sender, EventArgs e)
        {
            PU.Models.DepartmentsFrm child = new PU.Models.DepartmentsFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.DepID = DepID;
            child.action = "selection";
            child.ShowDialog();
            DepID = child.DepID;
            if (DepID != 0)
            {
                Department dep = db.Department.FirstOrDefault(x => x.ID == DepID);

                DepCode_.Text = dep.Code;
                DepName_.Text = dep.Name;
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            DSW3Data = null;
            this.Close();
        }

        private void DSW3_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            checkAccessLevel();

            if (action == "edit")
            {
                if (db.FormsDSW_3.Any(x => x.ID == DSW3ID))
                {
                    DSW3Data = db.FormsDSW_3.FirstOrDefault(x => x.ID == DSW3ID);
                    Year.Value = DSW3Data.YEAR;
                    NUMBERPAYMENT.Text = DSW3Data.NUMBERPAYMENT;

                    if (DSW3Data.DATEPAYMENT != null)
                        DATEPAYMENT.Value = DSW3Data.DATEPAYMENT;
                    if (DSW3Data.DATEEXECUTPAYMENT != null)
                        DATEEXECUTPAYMENT.Value = DSW3Data.DATEEXECUTPAYMENT;
                    if (DSW3Data.DateFilling != null)
                        DateFilling.Value = DSW3Data.DateFilling;

                    if (DSW3Data.DepartmentID.HasValue)
                    {
                        DepID = DSW3Data.DepartmentID.Value;
                        Department dep = db.Department.FirstOrDefault(x => x.ID == DepID);

                        DepCode_.Text = dep.Code;
                        DepName_.Text = dep.Name;
                    }
                }
                else
                {
                    RadMessageBox.Show("Не удалось загрузить форму ДСВ-3 из базы данных!");
                }
            }
            else
            {
                DSW3Data = new FormsDSW_3();
                DateFilling.Value = DateTime.Now;
                Year.Value = DateTime.Now.Year;
            }

        }

        private void DSW3_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cleanData)
                DSW3Data = null;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                bool flag_ok = false;
                try
                {
                    getValues();
                    flag_ok = true;
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы. Код ошибки: " + ex.Message);
                }

                if (flag_ok)
                {
                    switch (action)
                    {
                        case "add":

                            try
                            {
                                db.FormsDSW_3.Add(DSW3Data);
                                db.SaveChanges();
                                cleanData = false;
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Формы ДСВ-3 произошла ошибка. Код ошибки: " + ex.InnerException);
                            }

                            break;
                        case "edit":
                            try
                            {
                                // сохраняем модифицированную запись обратно в бд
                                db.Entry(DSW3Data).State = EntityState.Modified;
                                db.SaveChanges();
                                cleanData = false;
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Формы ДСВ-3 произошла ошибка. Код ошибки: " + ex.Message);
                            }
                            break;
                    }
                }


                cleanData = false;
            }
            else
            {
                foreach (var item in errMessBox)
                { Methods.showAlert("Ошибка заполнения", item.name, this.ThemeName, 100); }
            }
        }

        private void getValues()
        {
            if (action == "add")
                DSW3Data.DateFilling = DateFilling.Value;

            DSW3Data.InsurerID = Options.InsID;
            DSW3Data.YEAR = (short)Year.Value;
            DSW3Data.NUMBERPAYMENT = NUMBERPAYMENT.Text.Trim();
            DSW3Data.DATEPAYMENT = DATEPAYMENT.Value;
            DSW3Data.DATEEXECUTPAYMENT = DATEEXECUTPAYMENT.Value;
            DSW3Data.DepartmentID = DepID != 0 ? (long?)DepID : null;


        }

        /// <summary>
        /// Проверка введенных данных
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            if (String.IsNullOrEmpty(NUMBERPAYMENT.Text.Trim()))
            {
                errMessBox.Add(new ErrList { name = "Необходимо указать номер поручения" });
            }

            if (DATEPAYMENT.Value == DATEPAYMENT.NullDate)
                errMessBox.Add(new ErrList { name = "Необходимо указать дату поручения" });
            if (DATEEXECUTPAYMENT.Value == DATEEXECUTPAYMENT.NullDate)
                errMessBox.Add(new ErrList { name = "Необходимо указать дату исполнения" });
            if (DateFilling.Value == DateFilling.NullDate)
                errMessBox.Add(new ErrList { name = "Необходимо указать дату заполнения Реестра" });

            if (errMessBox.Count <= 0)
                switch (action)
                {
                    case "add":
                        if (db.FormsDSW_3.Any(x => x.InsurerID == Options.InsID && x.NUMBERPAYMENT == NUMBERPAYMENT.Text.Trim() && x.DATEPAYMENT == DATEPAYMENT.Value && x.YEAR == Year.Value))
                        {
                            errMessBox.Add(new ErrList { name = "Ошибка! Нарушение уникальности записей." });
                        }

                        break;
                    case "edit":
                        if (db.FormsDSW_3.Any(x => x.InsurerID == Options.InsID && x.NUMBERPAYMENT == NUMBERPAYMENT.Text.Trim() && x.DATEPAYMENT == DATEPAYMENT.Value && x.YEAR == Year.Value && x.ID != DSW3Data.ID))
                        {
                            errMessBox.Add(new ErrList { name = "Ошибка! Нарушение уникальности записей." });
                        }
                        break;
                }


            if (errMessBox.Count > 0)
                check = false;
            return check;
        }
    }
}
