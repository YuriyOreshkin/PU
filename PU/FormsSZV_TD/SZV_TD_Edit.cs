using PU.Classes;
using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;

namespace PU.FormsSZV_TD
{
    public partial class SZV_TD_Edit : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public FormsSZV_TD_2020 SZVTD { get; set; }
        public long szvtdID = 0;
        public List<long> szvtdStaffListIds { get; set; }

        public SZV_TD_Edit()
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

        private void SZV_TD_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            DateUnderwrite.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime;
            Annuled_DateTime.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime; 


            DateUnderwrite.Value = DateTime.Now.Date;
            Insurer ins = db.Insurer.First(x => x.ID == Options.InsID);


            //foreach (var item in db.TypeInfo)
            //{
            //    TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString() == "Корректирующая" ? "Дополняющая" : item.Name.ToString(), item.ID.ToString()));
            //}


            this.Height = 250;


            switch (action)
            {
                case "add":
//                    TypeInfo.Items.First().Selected = true;


                    if (szvtdStaffListIds.Count > 0)
                    {
                        copySzvtdGroupBox.Visible = true;

                        var id = szvtdStaffListIds[0];

                        var szvtd_source = db.FormsSZV_TD_2020_Staff.First(x => x.ID == id).FormsSZV_TD_2020;
                        fillData(szvtd_source);

                        this.Height = 330;



                    }
                    else
                    {
                        Month.SelectedIndex = DateTime.Now.AddDays(-15).Month - 1;
                        Year.Value = DateTime.Now.Year;
                        try
                        {
                            if (ins.TypePayer != 0)  // не организация
                            {
                                ConfirmLastName.Text = !String.IsNullOrEmpty(ins.LastName) ? ins.LastName : "";
                                ConfirmFirstName.Text = !String.IsNullOrEmpty(ins.FirstName) ? ins.FirstName : "";
                                ConfirmMiddleName.Text = !String.IsNullOrEmpty(ins.MiddleName) ? ins.MiddleName : "";
                            }
                            else
                            {
                                var FIO = ins.BossFIO.Split(' ');

                                ConfirmLastName.Text = FIO[0] != null ? (!String.IsNullOrEmpty(FIO[0]) ? FIO[0] : "") : "";
                                ConfirmFirstName.Text = FIO[1] != null ? (!String.IsNullOrEmpty(FIO[1]) ? FIO[1] : "") : "";

                                if (FIO.Length > 2)
                                    ConfirmMiddleName.Text = FIO[2] != null ? (!String.IsNullOrEmpty(FIO[2]) ? FIO[2] : "") : "";

                                ConfirmDolgn.Text = !String.IsNullOrEmpty(ins.BossDolgn) ? ins.BossDolgn : "";
                            }
                        }
                        catch { }
                    }
                    SZVTD = new FormsSZV_TD_2020 { };
                    break;
                case "edit":
                    SZVTD = db.FormsSZV_TD_2020.First(x => x.ID == szvtdID);
                    fillData(SZVTD);

 //                   TypeInfo.Items.Single(x => x.Value.ToString() == SZVM.TypeInfoID.ToString()).Selected = true;

//                    staffList = SZVTD.FormsSZV_M_2016_Staff.ToList();

                    break;
            }
        }

        private void fillData(FormsSZV_TD_2020 SZVTD)
        {
            DateUnderwrite.Value = SZVTD.DateFilling;
            Year.Value = SZVTD.Year;
            Month.Items.Single(x => x.Tag.ToString() == SZVTD.Month.ToString()).Selected = true;

            ConfirmDolgn.Text = SZVTD.ConfirmDolgn;
            ConfirmLastName.Text = SZVTD.ConfirmLastName;
            ConfirmFirstName.Text = SZVTD.ConfirmFirstName;
            ConfirmMiddleName.Text = SZVTD.ConfirmMiddleName;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
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

            if (DateUnderwrite.Value == DateUnderwrite.NullDate)
            {
                RadMessageBox.Show("Необходимо заполнить поле \"Дата заполнения\"!");
                check = false;
            }

            if (Annuled_CheckBox.Checked && Annuled_DateTime.Value == Annuled_DateTime.NullDate)
            {
                RadMessageBox.Show("Необходимо заполнить поле \"Дата отмены\"!");
                check = false;
            }

            //long InsID = Options.InsID;
            //short y = (short)Year.Value;
            //byte m = (byte)((Month.SelectedItem != null ? Month.SelectedIndex : 0) + 1);


            //switch (action)
            //{
            //    case "add":
            //        if (db.FormsSZV_TD_2020.Any(x => x.InsurerID == InsID && x.Year == y && x.Month == m))// && x.TypeInfoID == t  // && t == 1
            //        {
            //            RadMessageBox.Show("Дублирование записи по ключу уникальности (страхователь, календарный год, месяц)");//, тип формы
            //            check = false;
            //        }
            //        break;
            //    case "edit":
            //        if (db.FormsSZV_TD_2020.Any(x => x.InsurerID == InsID && x.Year == y && x.Month == m && x.ID != SZVTD.ID))  // && x.TypeInfoID == t  // && t == 1
            //        {
            //            RadMessageBox.Show("Дублирование записи по ключу уникальности (страхователь, календарный год, месяц)");//, тип формы
            //            check = false;
            //        }
            //        break;
            //}

            return check;

        }


        private void savingForm()
        {
            bool flag_ok = true;

            db = new pu6Entities();
            FormsSZV_TD_2020 SZVTD_t = action == "edit" ? db.FormsSZV_TD_2020.FirstOrDefault(x => x.ID == SZVTD.ID) : new FormsSZV_TD_2020 { };

            //            FormsSZV_M_2016 szvm_temp = db.FormsSZV_M_2016.First
            SZVTD_t.InsurerID = Options.InsID;
            SZVTD_t.Year = (short)Year.Value;
            SZVTD_t.Month = (byte)((Month.SelectedItem != null ? Month.SelectedIndex : 0) + 1);
            //SZVTD_t.TypeInfoID = long.Parse(TypeInfo.SelectedItem.Value.ToString());
            SZVTD_t.DateFilling = DateUnderwrite.Value;
            SZVTD_t.ConfirmLastName = ConfirmLastName.Text;
            SZVTD_t.ConfirmFirstName = ConfirmFirstName.Text;
            SZVTD_t.ConfirmMiddleName = ConfirmMiddleName.Text;
            SZVTD_t.ConfirmDolgn = ConfirmDolgn.Text;


            try
            {
                switch (action)
                {
                    case "add":

                        var szvtdStaffList = db.FormsSZV_TD_2020_Staff.Where(x => szvtdStaffListIds.Contains(x.ID)).ToList();

                        foreach (var item in szvtdStaffList)
                        {
                            FormsSZV_TD_2020_Staff newForm = item.Clone();

                            foreach (var ev in item.FormsSZV_TD_2020_Staff_Events.ToList())
                            {
                                FormsSZV_TD_2020_Staff_Events newEvent = ev.Clone();

                                if (Annuled_CheckBox.Checked && (newEvent.Annuled.HasValue && !newEvent.Annuled.Value))
                                {
                                    newEvent.Annuled = true;
                                    newEvent.AnnuleDate = Annuled_DateTime.Value;
                                }

                                newForm.FormsSZV_TD_2020_Staff_Events.Add(newEvent);
                            }

                            SZVTD_t.FormsSZV_TD_2020_Staff.Add(newForm);

                        }

                        db.FormsSZV_TD_2020.Add(SZVTD_t);

                        break;
                    case "edit":
                        db.Entry(SZVTD_t).State = EntityState.Modified;
                        break;
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                flag_ok = false;
                RadMessageBox.Show("Ошибка при сохранении Формы СЗВ-ТД! Код ошибки: " + ex.Message);
            }

            //if (action == "edit")
            //{
            //    var szvmStaffList_fromDB = db.FormsSZV_M_2016_Staff.Where(x => x.FormsSZV_M_2016_ID == SZVM.ID);

            //    var t = staffList.Select(c => c.StaffID);
            //    var list_for_del = szvmStaffList_fromDB.Where(x => !t.Contains(x.StaffID)).ToList();

            //    if (list_for_del.Count() > 0) // если есть записи на удаление из базы, т.е. в текущем списке этих сотрудников удалили. 
            //    {
            //        string list = String.Join(",", list_for_del.Select(x => x.ID).ToArray());
            //        try
            //        {
            //            db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsSZV_M_2016_Staff WHERE ([ID] IN ({0}))", list));

            //        }
            //        catch (Exception ex)
            //        {
            //            RadMessageBox.Show("Ошибка при обновлении списка сотрудников! Код ошибки: " + ex.Message);
            //        }

            //        t = list_for_del.Select(c => c.ID);
            //        szvmStaffList_fromDB = szvmStaffList_fromDB.Where(x => !t.Contains(x.ID));
            //    }

            //    var b = szvmStaffList_fromDB.Select(c => c.StaffID).ToList();
            //    staffList = staffList.Where(x => !b.Contains(x.StaffID)).ToList();

            //}

            //foreach (var item in staffList)
            //{
            //    db.AddToFormsSZV_M_2016_Staff(new FormsSZV_M_2016_Staff { StaffID = item.StaffID, FormsSZV_M_2016_ID = SZVTD_t.ID });
            //}

            //try
            //{
            //    if (staffList.Count > 0)
            //        db.SaveChanges();
            //}
            //catch (Exception ex)
            //{
            //    flag_ok = false;
            //    RadMessageBox.Show("Ошибка при сохранении Формы СЗВ-М! Код ошибки: " + ex.Message);
            //}

            if (flag_ok)
                this.Close();

        }

        private void Annuled_CheckBox_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            Annuled_DateTime.Enabled = Annuled_CheckBox.Checked;
        }

        private void DateUnderwrite_Enter(object sender, EventArgs e)
        {
            DateUnderwrite.Select();
        }

        private void Annuled_DateTime_Enter(object sender, EventArgs e)
        {
            Annuled_DateTime.Select();
        }

        //private void DateUnderwrite_ValueChanged(object sender, EventArgs e)
        //{
        //    if (DateUnderwrite.Value != DateUnderwrite.NullDate)
        //        DateUnderwriteMaskedEditBox.Text = DateUnderwrite.Value.ToShortDateString();
        //    else
        //        DateUnderwriteMaskedEditBox.Text = DateUnderwriteMaskedEditBox.NullText;
        //}

        //private void DateUnderwriteMaskedEditBox_Leave(object sender, EventArgs e)
        //{
        //    if (DateUnderwriteMaskedEditBox.Text != DateUnderwriteMaskedEditBox.NullText)
        //    {
        //        DateTime date;
        //        if (DateTime.TryParse(DateUnderwriteMaskedEditBox.Text, out date))
        //        {
        //            DateUnderwrite.Value = date;
        //        }
        //        else
        //        {
        //            DateUnderwrite.Value = DateUnderwrite.NullDate;
        //        }
        //    }
        //    else
        //    {
        //        DateUnderwrite.Value = DateUnderwrite.NullDate;
        //    }
        //}

    }
}
