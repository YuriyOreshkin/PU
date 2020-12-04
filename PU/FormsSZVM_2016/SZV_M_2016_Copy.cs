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

namespace PU.FormsSZVM_2016
{
    public partial class SZV_M_2016_Copy : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        private FormsSZV_M_2016 SZVMsource { get; set; }
        public long szvmID = 0;

        public SZV_M_2016_Copy()
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

        private void SZV_M_2016_Copy_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            SZVMsource = db.FormsSZV_M_2016.FirstOrDefault(x => x.ID == szvmID);

            foreach (var item in db.TypeInfo)
            {
                TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString() == "Корректирующая" ? "Дополняющая" : item.Name.ToString(), item.ID.ToString()));
            }
            TypeInfo.Items[0].Selected = true;
            Month.Items.Single(x => x.Tag.ToString() == DateTime.Now.Month.ToString()).Selected = true;

            yearLabel.Text = SZVMsource.YEAR.ToString();
            monthLabel.Text = Month.Items.Single(x => x.Tag.ToString() == SZVMsource.MONTH.ToString()).Text;
            typeInfoLabel.Text = TypeInfo.Items.Single(x => x.Value.ToString() == SZVMsource.TypeInfoID.ToString()).Text;
            Year.Value = DateTime.Now.Year;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copyBtn_Click(object sender, EventArgs e)
        {
            short year = (short)Year.Value;
            byte month = byte.Parse(Month.SelectedItem.Tag.ToString());
            long typeinfo = long.Parse(TypeInfo.SelectedItem.Value.ToString());

            if (SZVMsource.YEAR == year && SZVMsource.MONTH == month && SZVMsource.TypeInfoID == typeinfo && typeinfo == 1)
            {
                Messenger.showAlert(AlertType.Info, "Внимание", "Форма назначения не может совпадать с исходной формой!", this.ThemeName);
                return;
            }
            if (db.FormsSZV_M_2016.Any(x => x.InsurerID == Options.InsID && x.ID != SZVMsource.ID && x.YEAR == year && x.MONTH == month && typeinfo == 1 && x.TypeInfoID == typeinfo))
            {
                Messenger.showAlert(AlertType.Info, "Внимание", "Дублирование по ключу уникальности! Выберите другие параметры для формы назначения!", this.ThemeName);
                return;
            }


            try
            {
                FormsSZV_M_2016 SZVMnew = new FormsSZV_M_2016
                {
                    InsurerID = Options.InsID,
                    YEAR = year,
                    MONTH = month,
                    TypeInfoID = typeinfo,
                    DateFilling = DateTime.Now
                };

                List<long> staffIDlist = SZVMsource.FormsSZV_M_2016_Staff.Select(x => x.StaffID).ToList();

                foreach (long staffID in staffIDlist)
                {
                    SZVMnew.FormsSZV_M_2016_Staff.Add(new FormsSZV_M_2016_Staff { StaffID = staffID });
                }

                db.FormsSZV_M_2016.Add(SZVMnew);
                db.SaveChanges();
                Messenger.showAlert(AlertType.Success, "Успех", "Форма СЗВ-М успешно скопирована!", this.ThemeName);
                this.Close();
            }
            catch (Exception ex)
            {
                Messenger.showAlert(AlertType.Error, "Ошибка", "Во время копирования формы СЗВ-М произошла ошибка. Код ошибки - " + ex.Message, this.ThemeName);
            }


        }


    }
}
