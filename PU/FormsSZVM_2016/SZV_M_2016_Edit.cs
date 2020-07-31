using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using PU.Models;
using PU.Classes;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;

namespace PU.FormsSZVM_2016
{
    public partial class SZV_M_2016_Edit : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public FormsSZV_M_2016 SZVM { get; set; }
        public long szvmID = 0;
        List<FormsSZV_M_2016_Staff> staffList = new List<FormsSZV_M_2016_Staff> { };

        public SZV_M_2016_Edit()
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

        private void SZV_M_2016_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            DateUnderwrite.Value = DateTime.Now.Date;

            foreach (var item in db.TypeInfo)
            {
                TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString() == "Корректирующая" ? "Дополняющая" : item.Name.ToString(), item.ID.ToString()));
            }

            switch (action)
            {
                case "add":
                    TypeInfo.Items.First().Selected = true;
                    Month.SelectedIndex = DateTime.Now.AddDays(-15).Month - 1;
                    Year.Value = DateTime.Now.Year;

                    SZVM = new FormsSZV_M_2016 { };
                    break;
                case "edit":
                    SZVM = db.FormsSZV_M_2016.First(x => x.ID == szvmID);
                    DateUnderwrite.Value = SZVM.DateFilling;
                    Year.Value = SZVM.YEAR;
                    Month.Items.Single(x => x.Tag.ToString() == SZVM.MONTH.ToString()).Selected = true;

                    TypeInfo.Items.Single(x => x.Value.ToString() == SZVM.TypeInfoID.ToString()).Selected = true;

                    staffList = SZVM.FormsSZV_M_2016_Staff.ToList();

                    break;
            }

        }

        private void DateUnderwrite_ValueChanged(object sender, EventArgs e)
        {
            if (DateUnderwrite.Value != DateUnderwrite.NullDate)
                DateUnderwriteMaskedEditBox.Text = DateUnderwrite.Value.ToShortDateString();
            else
                DateUnderwriteMaskedEditBox.Text = DateUnderwriteMaskedEditBox.NullText;
        }

        private void DateUnderwriteMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateUnderwriteMaskedEditBox.Text != DateUnderwriteMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateUnderwriteMaskedEditBox.Text, out date))
                {
                    DateUnderwrite.Value = date;
                }
                else
                {
                    DateUnderwrite.Value = DateUnderwrite.NullDate;
                }
            }
            else
            {
                DateUnderwrite.Value = DateUnderwrite.NullDate;
            }
        }

        private void staffGridView_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo != null && e.CellElement.ColumnInfo.Name == "Num" && string.IsNullOrEmpty(e.CellElement.Text))
            {
                e.CellElement.Text = (e.CellElement.RowIndex + 1).ToString();
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                savingForm();
            }
            else
            {
                return;
            }
        }

        private bool validation()
        {
            bool check = true;

            long InsID = Options.InsID;
            short y = (short)Year.Value;
            byte m = (byte)((Month.SelectedItem != null ? Month.SelectedIndex : 0) + 1);
            long t = 0;

            if (TypeInfo.SelectedItem != null)
            {
                t = long.Parse(TypeInfo.SelectedItem.Value.ToString());
            }
            else
            {
                RadMessageBox.Show("Необходимо выбрать Тип формы!");
                check = false;
            }

            switch (action)
            {
                case "add":
                    if (db.FormsSZV_M_2016.Any(x => x.InsurerID == InsID && x.YEAR == y && x.MONTH == m && t == 1 && x.TypeInfoID == t))
                    {
                        RadMessageBox.Show("Дублирование записи по ключу уникальности (страхователь, календарный год, месяц, тип формы)");
                        check = false;
                    }
                    break;
                case "edit":
                    if (db.FormsSZV_M_2016.Any(x => x.InsurerID == InsID && x.YEAR == y && x.MONTH == m && t == 1 && x.TypeInfoID == t && x.ID != SZVM.ID))
                    {
                        RadMessageBox.Show("Дублирование записи по ключу уникальности (страхователь, календарный год, месяц, тип формы)");
                        check = false;
                    }
                    break;
            }

            return check;

        }

        private void savingForm()
        {
            bool flag_ok = true;

            db = new pu6Entities();
            FormsSZV_M_2016 SZVM_t = action == "edit" ? db.FormsSZV_M_2016.FirstOrDefault(x => x.ID == SZVM.ID) : new FormsSZV_M_2016 { };

//            FormsSZV_M_2016 szvm_temp = db.FormsSZV_M_2016.First
            SZVM_t.InsurerID = Options.InsID;
            SZVM_t.YEAR = (short)Year.Value;
            SZVM_t.MONTH = (byte)((Month.SelectedItem != null ? Month.SelectedIndex : 0) + 1);
            SZVM_t.TypeInfoID = long.Parse(TypeInfo.SelectedItem.Value.ToString());
            SZVM_t.DateFilling = DateUnderwrite.Value;

            try
            {
                switch (action)
                {
                    case "add":
                        db.FormsSZV_M_2016.Add(SZVM_t);
                        break;
                    case "edit":
                        db.Entry(SZVM_t).State = EntityState.Modified;
                        break;
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                flag_ok = false;
                RadMessageBox.Show("Ошибка при сохранении Формы СЗВ-М! Код ошибки: " + ex.Message);
            }

            if (action == "edit")
            {
                var szvmStaffList_fromDB = db.FormsSZV_M_2016_Staff.Where(x => x.FormsSZV_M_2016_ID == SZVM.ID);

                var t = staffList.Select(c => c.StaffID);
                var list_for_del = szvmStaffList_fromDB.Where(x => !t.Contains(x.StaffID)).ToList();

                if (list_for_del.Count() > 0) // если есть записи на удаление из базы, т.е. в текущем списке этих сотрудников удалили. 
                {
                    string list = String.Join(",", list_for_del.Select(x => x.ID).ToArray());
                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsSZV_M_2016_Staff WHERE ([ID] IN ({0}))", list));

                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("Ошибка при обновлении списка сотрудников! Код ошибки: " + ex.Message);
                    }

                    t = list_for_del.Select(c => c.ID);
                    szvmStaffList_fromDB = szvmStaffList_fromDB.Where(x => !t.Contains(x.ID));
                }

                var b = szvmStaffList_fromDB.Select(c => c.StaffID).ToList();
                staffList = staffList.Where(x => !b.Contains(x.StaffID)).ToList();

            }

            foreach (var item in staffList)
            {
                db.FormsSZV_M_2016_Staff.Add(new FormsSZV_M_2016_Staff { StaffID = item.StaffID, FormsSZV_M_2016_ID = SZVM_t.ID });
            }

            try
            {
                if (staffList.Count > 0)
                    db.SaveChanges();
            }
            catch (Exception ex)
            {
                flag_ok = false;
                RadMessageBox.Show("Ошибка при сохранении Формы СЗВ-М! Код ошибки: " + ex.Message);
            }

            if (flag_ok)
                this.Close();

        }





    }
}
