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
using PU.Classes;
using PU.Models;
using Telerik.WinControls.UI;
using PU.FormsRSW2014;
using System.Reflection;
using PU.Staj;

namespace PU.FormsSZV_STAJ
{
    public partial class SZV_STAJ_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public Staff staff { get; set; }
        public long ODV1_ID { get; set; }
        public int defaultPage = 0;
        List<FormsSZV_STAJ_4_2017> FormsSZV_STAJ_4_2017_List = new List<FormsSZV_STAJ_4_2017>();
        public FormsSZV_STAJ_2017 SZV_STAJ { get; set; }
        public List<StajOsn> StajOsn_List = new List<StajOsn>();
        //    public List<StajLgot> StajLgot_List = new List<StajLgot>();
        private List<string> errMessBox = new List<string>();

        public byte period { get; set; }
        public byte CorrNum { get; set; }
        bool allowClose = false;
        bool editAllRecords = false;

        public string parentName { get; set; }

        public SZV_STAJ_Edit()
        {
            InitializeComponent();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
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
        /// <summary>
        /// Сохранение записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton1_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                bool flag_ok = false;

                SZV_STAJ.Year = short.Parse(Year.Text);
                SZV_STAJ.InsurerID = Options.InsID;
                SZV_STAJ.Staff = staff;
                SZV_STAJ.TypeInfo = byte.Parse(TypeInfo.SelectedItem.Tag.ToString());
                SZV_STAJ.DateFilling = DateFilling.Value.Date;
                SZV_STAJ.ConfirmFIO = ConfirmFIO.Text;
                SZV_STAJ.ConfirmDolgn = ConfirmDolgn.Text;
                SZV_STAJ.Dismissed = DismissedCheckBox.Checked;

                if (TypeInfo.SelectedIndex == 2)
                {
                    SZV_STAJ.OPSFeeNach = OPSFeeNachCheckBox.Checked ? (byte)1 : (byte)0;
                    SZV_STAJ.DopTarFeeNach = DopTarFeeNachCheckBox.Checked ? (byte)1 : (byte)0;

                }
                else
                {
                    SZV_STAJ.OPSFeeNach = (byte)0;
                    SZV_STAJ.DopTarFeeNach = (byte)0;


                }

                long szvID = 0;

                switch (action)
                {
                    case "add":
                        try
                        {
                            var fields = typeof(FormsSZV_STAJ_4_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            //таблица 5
                            foreach (var item in FormsSZV_STAJ_4_2017_List)
                            {
                                //item.FormsODV_1_2017_ID = ODV1.ID;
                                FormsSZV_STAJ_4_2017 r = new FormsSZV_STAJ_4_2017();

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = item.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(item, null);
                                        var data = value;

                                        r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                    }

                                }

                                SZV_STAJ.FormsSZV_STAJ_4_2017.Add(r);

                            }

                            db.FormsSZV_STAJ_2017.Add(SZV_STAJ);
                            db.SaveChanges();
                            szvID = SZV_STAJ.ID;



                            var fields_lgot = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names_lgot = Array.ConvertAll(fields_lgot, field => field.Name);


                            foreach (var item in StajOsn_List)
                            {
                                item.FormsSZV_STAJ_2017_ID = SZV_STAJ.ID;
                                StajOsn r = new StajOsn();

                                fields = typeof(StajOsn).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                names = Array.ConvertAll(fields, field => field.Name);

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = item.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(item, null);
                                        var data = value;

                                        r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                    }

                                }
                                db.StajOsn.Add(r);
                                try
                                {
                                    flag_ok = false;
                                    db.SaveChanges();
                                    flag_ok = true;
                                }
                                catch (Exception ex)
                                {
                                    RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                                }

                                //добавление дополнительных льготных записей
                                foreach (var item_lgot in item.StajLgot)
                                {
                                    // добавление записи в БД
                                    item_lgot.StajOsnID = r.ID;
                                    StajLgot r_ = new StajLgot();

                                    foreach (var itemName_ in names_lgot)
                                    {
                                        string itemName = itemName_.TrimStart('_');
                                        var properties = item_lgot.GetType().GetProperty(itemName);
                                        if (properties != null)
                                        {
                                            object value = properties.GetValue(item_lgot, null);
                                            var data = value;

                                            r_.GetType().GetProperty(itemName).SetValue(r_, data, null);
                                        }

                                    }

                                    db.StajLgot.Add(r_);
                                }

                            }
                            flag_ok = true;
                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);

                        }
                        if (flag_ok)
                        {
                            try
                            {
                                flag_ok = false;
                                db.SaveChanges();
                                allowClose = true;
                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                            }

                        }


                        break;
                    case "edit":   // режим редактирования записи


                        // выбираем из базы исходную запись по идешнику
                        db = new pu6Entities();
                        FormsSZV_STAJ_2017 szvstaj = db.FormsSZV_STAJ_2017.FirstOrDefault(x => x.ID == SZV_STAJ.ID);
                        try
                        {

                            szvstaj.DateFilling = SZV_STAJ.DateFilling;
                            szvstaj.Year = SZV_STAJ.Year;
                            szvstaj.StaffID = SZV_STAJ.StaffID;
                            szvstaj.TypeInfo = SZV_STAJ.TypeInfo;
                            szvstaj.OPSFeeNach = SZV_STAJ.OPSFeeNach;
                            szvstaj.DopTarFeeNach = SZV_STAJ.DopTarFeeNach;
                            szvstaj.ConfirmDolgn = SZV_STAJ.ConfirmDolgn;
                            szvstaj.ConfirmFIO = SZV_STAJ.ConfirmFIO;
                            szvstaj.Dismissed = SZV_STAJ.Dismissed;


                            // сохраняем модифицированную запись обратно в бд
                            db.Entry(szvstaj).State = EntityState.Modified;
                            db.SaveChanges();

                            szvID = szvstaj.ID;
                            flag_ok = true;

                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранение данных Формы СЗВ-СТАЖ произошла ошибка. Код ошибки: " + ex.Message);
                        }

                        try
                        {
                            var fields = typeof(FormsSZV_STAJ_4_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            var FormsSZV_STAJ_4_2017_List_from_db = db.FormsSZV_STAJ_4_2017.Where(x => x.FormsSZV_STAJ_2017_ID == szvstaj.ID);

                            // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                            var t = FormsSZV_STAJ_4_2017_List.Select(x => x.ID);
                            var list_for_del = FormsSZV_STAJ_4_2017_List_from_db.Where(x => !t.Contains(x.ID));

                            foreach (var item in list_for_del)
                            {
                                db.FormsSZV_STAJ_4_2017.Remove(item);
                            }

                            if (list_for_del.Count() != 0)
                            {
                                //db.SaveChanges();
                                FormsSZV_STAJ_4_2017_List_from_db = db.FormsSZV_STAJ_4_2017.Where(x => x.FormsSZV_STAJ_2017_ID == szvstaj.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                            }


                            foreach (var item in FormsSZV_STAJ_4_2017_List)
                            {
                                item.FormsSZV_STAJ_2017_ID = szvstaj.ID;
                                FormsSZV_STAJ_4_2017 r = new FormsSZV_STAJ_4_2017();
                                bool exist = false;

                                if (item.ID != 0)
                                {
                                    r = FormsSZV_STAJ_4_2017_List_from_db.First(x => x.ID == item.ID);
                                    exist = true;
                                }

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = item.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(item, null);
                                        var data = value;

                                        r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                    }

                                }

                                if (!exist)
                                    db.FormsSZV_STAJ_4_2017.Add(r);
                                else
                                    db.Entry(r).State = EntityState.Modified;

                            }

                            try
                            {
                                db.SaveChanges();
                            }
                            catch { }

                        }
                        catch { }

                        if (flag_ok)
                        {
                            flag_ok = false;

                            #region обрабатываем записи о Стаже
                            try
                            {
                                var StajOsn_List_from_db = db.StajOsn.Where(x => x.FormsSZV_STAJ_2017_ID == szvstaj.ID);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = StajOsn_List.Select(x => x.ID);
                                var list_for_del = StajOsn_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    if (item.StajLgot.Any())
                                    {
                                        List<long> l_id = item.StajLgot.Select(x => x.ID).ToList();
                                        foreach (var stl in l_id)
                                        {
                                            StajLgot l = db.StajLgot.FirstOrDefault(x => x.ID == stl);
                                            db.StajLgot.Remove(l);
                                        }
                                    }

                                    db.StajOsn.Remove(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    StajOsn_List_from_db = db.StajOsn.Where(x => x.FormsSZV_STAJ_2017_ID == szvstaj.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 6.8 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(StajOsn).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);

                                var fields_lgot = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names_lgot = Array.ConvertAll(fields_lgot, field => field.Name);


                                bool flag_ok_lgot = true;
                                foreach (var item in StajOsn_List)
                                {
                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (StajOsn_List_from_db.Any(x => x.ID == item.ID))
                                    {

                                        #region Проверка дополнительных льготных записей
                                        flag_ok_lgot = false;
                                        try
                                        {
                                            var StajLgot_List_from_db = db.StajLgot.Where(x => x.StajOsnID == item.ID);

                                            // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                            var StajLgot_List = item.StajLgot;
                                            var t_lgot = StajLgot_List.Select(x => x.ID);
                                            var list_for_del_lgot = StajLgot_List_from_db.Where(x => !t_lgot.Contains(x.ID));

                                            foreach (var item_lgot in list_for_del_lgot)
                                            {
                                                db.StajLgot.Remove(item_lgot);
                                            }

                                            if (list_for_del_lgot.Count() != 0)
                                            {
                                                //db.SaveChanges();
                                                StajLgot_List_from_db = db.StajLgot.Where(x => x.StajOsnID == item.ID && !list_for_del_lgot.Select(y => y.ID).Contains(x.ID));
                                            }

                                            // Проверка дополнительных льготных записей на факт их редактирования, если записи новая то добавляем ее



                                            foreach (var item_lgot in StajLgot_List)
                                            {
                                                bool flag_lgot_add_new = true;
                                                //если такая запись есть, надо проверять на отличия
                                                if (StajLgot_List_from_db.Any(x => x.ID == item_lgot.ID))
                                                {
                                                    flag_lgot_add_new = false;
                                                    bool flag_lgot_edited = false;
                                                    StajLgot lgot_temp = StajLgot_List_from_db.Single(x => x.ID == item_lgot.ID);


                                                    foreach (var item_lgot_ in names_lgot)
                                                    {
                                                        string itemName = item_lgot_.TrimStart('_');
                                                        if (itemName != "StajOsnID")
                                                        {
                                                            string data_old = "";
                                                            string data_new = "";

                                                            var properties_old = lgot_temp.GetType().GetProperty(itemName);
                                                            object value_old = properties_old.GetValue(lgot_temp, null);
                                                            if (value_old != null)
                                                                data_old = value_old.ToString();

                                                            var properties_new = item_lgot.GetType().GetProperty(itemName);
                                                            object value_new = properties_new.GetValue(item_lgot, null);
                                                            if (value_new != null)
                                                                data_new = value_new.ToString();

                                                            if (data_old != data_new)
                                                            {
                                                                flag_lgot_edited = true;

                                                                lgot_temp.GetType().GetProperty(itemName).SetValue(lgot_temp, value_new, null);
                                                            }
                                                        }
                                                    }


                                                    if (flag_lgot_edited) // если записи отличаются
                                                    {

                                                        db.Entry(lgot_temp).State = EntityState.Modified;

                                                    }


                                                }
                                                if (flag_lgot_add_new) // такой записи в базе нет, значит просто добавляем ее
                                                {

                                                    // добавление записи в БД
                                                    item_lgot.StajOsnID = item.ID;
                                                    StajLgot r = new StajLgot();

                                                    foreach (var itemName_ in names_lgot)
                                                    {
                                                        string itemName = itemName_.TrimStart('_');
                                                        var properties = item_lgot.GetType().GetProperty(itemName);
                                                        if (properties != null)
                                                        {
                                                            object value = properties.GetValue(item_lgot, null);
                                                            var data = value;

                                                            r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                                        }

                                                    }

                                                    db.StajLgot.Add(r);
                                                }



                                            }
                                            flag_ok_lgot = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("При сохранение данных Дополнительныз записей о льготном стаже произошла ошибка. Код ошибки: " + ex.Message);

                                        }


                                        #endregion


                                        if (flag_ok_lgot)
                                        {
                                            flag_add_new = false;
                                            bool flag_edited = false;
                                            StajOsn rsw_temp = StajOsn_List_from_db.Single(x => x.ID == item.ID);


                                            foreach (var item_ in names)
                                            {
                                                string itemName = item_.TrimStart('_');
                                                if (item_.IndexOf("FormsSZV_STAJ_2017_ID") < 0)
                                                {
                                                    string data_old = "";
                                                    string data_new = "";

                                                    var properties_old = rsw_temp.GetType().GetProperty(itemName);
                                                    object value_old = properties_old.GetValue(rsw_temp, null);
                                                    data_old = value_old != null ? value_old.ToString() : "";

                                                    var properties_new = item.GetType().GetProperty(itemName);
                                                    object value_new = properties_new.GetValue(item, null);
                                                    data_new = value_new != null ? value_new.ToString() : "";

                                                    if (data_old != data_new)
                                                    {
                                                        flag_edited = true;

                                                        rsw_temp.GetType().GetProperty(itemName).SetValue(rsw_temp, value_new, null);
                                                    }
                                                }
                                            }


                                            if (flag_edited) // если записи отличаются
                                            {

                                                db.Entry(rsw_temp).State = EntityState.Modified;

                                            }
                                        }

                                    }
                                    if (flag_add_new && flag_ok_lgot) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        item.FormsSZV_STAJ_2017_ID = SZV_STAJ.ID;
                                        StajOsn r = new StajOsn();

                                        foreach (var itemName_ in names)
                                        {
                                            string itemName = itemName_.TrimStart('_');
                                            var properties = item.GetType().GetProperty(itemName);
                                            if (properties != null)
                                            {
                                                object value = properties.GetValue(item, null);
                                                var data = value;

                                                r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                            }

                                        }

                                        db.StajOsn.Add(r);
                                        try
                                        {
                                            flag_ok = false;
                                            db.SaveChanges();
                                            flag_ok = true;
                                        }
                                        catch (Exception ex)
                                        {
                                            RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                                        }


                                        //добавление дополнительных льготных записей
                                        foreach (var item_lgot in item.StajLgot)
                                        {
                                            // добавление записи в БД
                                            item_lgot.StajOsnID = r.ID;
                                            StajLgot r_ = new StajLgot();

                                            foreach (var itemName_ in names_lgot)
                                            {
                                                string itemName = itemName_.TrimStart('_');
                                                var properties = item_lgot.GetType().GetProperty(itemName);
                                                if (properties != null)
                                                {
                                                    object value = properties.GetValue(item_lgot, null);
                                                    var data = value;

                                                    r_.GetType().GetProperty(itemName).SetValue(r_, data, null);
                                                }

                                            }

                                            db.StajLgot.Add(r_);
                                        }

                                    }


                                }
                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);

                            }

                            #endregion

                            if (flag_ok)
                            {
                                try
                                {
                                    flag_ok = false;
                                    db.SaveChanges();
                                    allowClose = true;
                                    flag_ok = true;
                                }
                                catch (Exception ex)
                                {
                                    RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                                }

                            }
                        }

                        break;

                }


                if (editAllRecords)  //  Меняем основные параметры для всех записей, если было выбрано их редактирование
                {
                    try
                    {
                        pu6Entities db2 = new pu6Entities();
                        var szvListForEdit = db2.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == ODV1_ID && x.ID != szvID).ToList();

                        int k = 0;
                        foreach (var item in szvListForEdit)
                        {
                            item.TypeInfo = SZV_STAJ.TypeInfo;
                            item.Year = SZV_STAJ.Year;
                            item.OPSFeeNach = SZV_STAJ.OPSFeeNach;
                            item.DopTarFeeNach = SZV_STAJ.DopTarFeeNach;
                            item.DateFilling = SZV_STAJ.DateFilling;
                            item.ConfirmFIO = SZV_STAJ.ConfirmFIO;
                            item.ConfirmDolgn = SZV_STAJ.ConfirmDolgn;

                            foreach (var t in item.FormsSZV_STAJ_4_2017.ToList())
                            {
                                db2.FormsSZV_STAJ_4_2017.Remove(t);
                            }


                            foreach (var item4 in FormsSZV_STAJ_4_2017_List)
                            {
                                item.FormsSZV_STAJ_4_2017.Add(new FormsSZV_STAJ_4_2017 { DNPO_DateFrom = item4.DNPO_DateFrom, DNPO_DateTo = item4.DNPO_DateTo, DNPO_Fee = item4.DNPO_Fee});
                            }

                            k++;

                            if (k % 200 == 0)
                                db2.SaveChanges();
                        }


                        db2.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "В процессе обновления данных всех записей СЗВ-СТАЖ произошла ошибка! Код ошибки - " + ex.Message, "Внимание!");
                    }


                }

                if (flag_ok)
                    this.Close();

            }
            else
            {
                if (errMessBox.Count != 0)
                {
                    foreach (var item in errMessBox)
                    {
                        RadMessageBox.Show(this, item, "Внимание!");
                    }
                }

            }


        }


        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();
            short y = 0;
            if (Year.Text == "")
                errMessBox.Add("Календарный год должен быть заполнен!");

            if (staff == null)
            {
                errMessBox.Add("Необходимо выбрать сотрудника");
                check = false;
                return check;
            }

            short.TryParse(Year.Text, out y);

            byte typeInfo = byte.Parse(TypeInfo.SelectedItem.Tag.ToString());

            if (typeInfo != 1)
            {
                switch (action)
                {
                    case "add":
                        if (db.FormsSZV_STAJ_2017.Any(x => x.Year == y && x.TypeInfo == typeInfo && x.InsurerID == Options.InsID && x.StaffID == staff.ID && x.FormsODV_1_2017_ID == SZV_STAJ.FormsODV_1_2017_ID))
                            errMessBox.Add("Дублирование записи по ключу уникальности");
                        break;
                    case "edit":
                        if (db.FormsSZV_STAJ_2017.Any(x => x.Year == y && x.TypeInfo == typeInfo && x.InsurerID == Options.InsID && x.StaffID == staff.ID && x.ID != SZV_STAJ.ID && x.FormsODV_1_2017_ID == SZV_STAJ.FormsODV_1_2017_ID))
                            errMessBox.Add("Дублирование записи по ключу уникальности");
                        break;
                }
            }

            if (errMessBox.Count > 0)
                check = false;
            return check;
        }


        /// <summary>
        /// Загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SZV_STAJ_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            loadRukBtn.ButtonElement.ToolTipText = "Загрузить данные о руководителе из справочника";
            this.Cursor = Cursors.WaitCursor;
            this.radPageView1.SelectedPage = this.radPageView1.Pages[0];

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    this.Size = param.size;
                    this.Location = param.location;
                    this.WindowState = param.windowState == FormWindowState.Minimized ? FormWindowState.Maximized : param.windowState;
                }
                catch
                { }

            }

            try
            {

                bool firstRecord = false;




                switch (action)
                {
                    case "add":

                        firstRecord = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == ODV1_ID).Count() <= 0;

                        SZV_STAJ = new FormsSZV_STAJ_2017();
                        TypeInfo.Items.Single(x => x.Tag.ToString() == "2").Selected = true;
                        DateFilling.Value = DateTime.Now.Date;
                        SZV_STAJ.FormsODV_1_2017_ID = ODV1_ID;
                        Year.Value = DateTime.Now.AddDays(-30).Year;

                        Insurer ins = db.Insurer.First(x => x.ID == Options.InsID);

                        if (!String.IsNullOrEmpty(ins.BossDolgn))
                        {
                            ConfirmDolgn.Text = ins.BossDolgn;
                        }
                        if (!String.IsNullOrEmpty(ins.BossFIO))
                        {
                            ConfirmFIO.Text = ins.BossFIO;
                        }

                        if (!firstRecord)
                        {
                            var szvSfirst = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == ODV1_ID).OrderBy(x => x.ID).First();

                            TypeInfo.Items.Single(x => x.Tag.ToString() == szvSfirst.TypeInfo.ToString()).Selected = true;
                            change_TypeInfo();
                            TypeInfo.Enabled = false;

                            Year.Value = szvSfirst.Year;
                            Year.Enabled = false;

                            OPSFeeNachCheckBox.Checked = (szvSfirst.OPSFeeNach.HasValue && szvSfirst.OPSFeeNach.Value == 1);
                            OPSFeeNachCheckBox.Enabled = false;
                            DopTarFeeNachCheckBox.Checked = (szvSfirst.DopTarFeeNach.HasValue && szvSfirst.DopTarFeeNach.Value == 1);
                            DopTarFeeNachCheckBox.Enabled = false;

                            ConfirmDolgn.Text = szvSfirst.ConfirmDolgn;
                            ConfirmDolgn.Enabled = false;
                            ConfirmFIO.Text = szvSfirst.ConfirmFIO;
                            ConfirmFIO.Enabled = false;

                            FormsSZV_STAJ_4_2017_List = szvSfirst.FormsSZV_STAJ_4_2017.ToList();
                            SZV_STAJ_4_Grid_update();

                            razd5_addBtn.Enabled = false;
                            razd5_delBtn.Enabled = false;
                            razd5_editBtn.Enabled = false;

                            loadRukBtn.Enabled = false;
                            DateFilling.Enabled = false;
                            DateFillingMaskedEditBox.Enabled = false;
                        }
                        else
                        {
                            radLabel7.Visible = false;
                        }

                        break;
                    case "edit":
                        #region Отчетный период

                        try
                        {
                            Year.Value = SZV_STAJ.Year;
                        }
                        catch
                        { }


                        #endregion

                        DateFilling.Value = SZV_STAJ.DateFilling;

                        firstRecord = firstRecord = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == ODV1_ID).Count() == 1;

                        TypeInfo.Items.Single(x => x.Tag.ToString() == SZV_STAJ.TypeInfo.ToString()).Selected = true;
                        change_TypeInfo();

                        OPSFeeNachCheckBox.Checked = (SZV_STAJ.OPSFeeNach.HasValue && SZV_STAJ.OPSFeeNach.Value == 1);
                        DopTarFeeNachCheckBox.Checked = (SZV_STAJ.DopTarFeeNach.HasValue && SZV_STAJ.DopTarFeeNach.Value == 1);

                        ConfirmDolgn.Text = SZV_STAJ.ConfirmDolgn;
                        ConfirmFIO.Text = SZV_STAJ.ConfirmFIO;

                        DismissedCheckBox.Checked = SZV_STAJ.Dismissed.HasValue ? SZV_STAJ.Dismissed.Value : false;

                        //Информация о сотруднике
                        staff = SZV_STAJ.Staff;

                        SNILS.Enabled = false;
                        selectStaffBtn.Enabled = false;

                        if (!firstRecord)
                        {
                            TypeInfo.Enabled = false;
                            OPSFeeNachCheckBox.Enabled = false;
                            DopTarFeeNachCheckBox.Enabled = false;
                            Year.Enabled = false;

                            ConfirmDolgn.Enabled = false;
                            ConfirmFIO.Enabled = false;

                            razd5_addBtn.Enabled = false;
                            razd5_delBtn.Enabled = false;
                            razd5_editBtn.Enabled = false;

                            loadRukBtn.Enabled = false;
                            DateFilling.Enabled = false;
                            DateFillingMaskedEditBox.Enabled = false;

                        }
                        else
                        {
                            radLabel7.Visible = false;
                        }


                        FormsSZV_STAJ_4_2017_List = SZV_STAJ.FormsSZV_STAJ_4_2017.ToList();
                        SZV_STAJ_4_Grid_update();

                        break;
                }

                updateStaffInfo();

                this.TypeInfo.SelectedIndexChanged += (s, с) => change_TypeInfo();


                var stajOsn = SZV_STAJ.StajOsn;

                if (StajOsn_List != null)
                    StajOsn_List.Clear();
                foreach (var item in stajOsn)
                {
                    StajOsn_List.Add(item);
                }

                gridUpdate_StajOsn();


                long id_old = 0;
                switch (action)
                {
                    case "add":

                        break;
                    case "edit":
                        id_old = SZV_STAJ.ID;
                        SZV_STAJ = new FormsSZV_STAJ_2017 { ID = id_old };
                        break;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void SZV_STAJ_4_Grid_update()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            SZV_STAJ_4_Grid.Rows.Clear();

            if (FormsSZV_STAJ_4_2017_List.Count() != 0)
            {
                foreach (var item in FormsSZV_STAJ_4_2017_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.SZV_STAJ_4_Grid.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    if (item.DNPO_DateFrom.HasValue)
                        rowInfo.Cells["DNPO_DateFrom"].Value = item.DNPO_DateFrom.Value;
                    if (item.DNPO_DateTo.HasValue)
                        rowInfo.Cells["DNPO_DateTo"].Value = item.DNPO_DateTo.Value;
                    rowInfo.Cells["DNPO_Fee"].Value = item.DNPO_Fee.HasValue ? item.DNPO_Fee.Value : false;
                    SZV_STAJ_4_Grid.Rows.Add(rowInfo);
                }
            }

            this.SZV_STAJ_4_Grid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            SZV_STAJ_4_Grid.Refresh();
        }

        private void updateStaffInfo()
        {
            if (staff != null)
            {
                try
                {
                    LastName.Text = staff.LastName;
                    FirstName.Text = staff.FirstName;
                    MiddleName.Text = staff.MiddleName;
                    SNILS.Text = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber.Value) : "";
                    Tabel.Text = staff.TabelNumber != null ? staff.TabelNumber.Value.ToString() : "";
                }
                catch { }
            }
        }

        private void change_TypeInfo()
        {
            if (TypeInfo.SelectedIndex == 2)
            {
                radGroupBox4.Enabled = true;
                radGroupBox2.Enabled = true;
            }
            else
            {
                radGroupBox4.Enabled = false;
                radGroupBox2.Enabled = false;
            }


        }

        //private void TypeInfo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        //{
        //    change_TypeInfo();
        //}




        #region Основной стаж

        /// <summary>
        /// обновление таблицы раздела 6.8 Основной стаж
        /// </summary>
        private void gridUpdate_StajOsn()
        {
            stajOsnGrid.Rows.Clear();

            StajOsn_List = StajOsn_List.OrderBy(x => x.Number.Value).ToList();

            if (StajOsn_List.Count() != 0)
            {
                foreach (var item in StajOsn_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.stajOsnGrid.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Number"].Value = item.Number;
                    rowInfo.Cells["DateBegin"].Value = item.DateBegin.Value.ToShortDateString();
                    rowInfo.Cells["DateEnd"].Value = item.DateEnd.Value.ToShortDateString();
                    rowInfo.Cells["codeBEZR"].Value = item.CodeBEZR.HasValue ? item.CodeBEZR.Value : false;
                    stajOsnGrid.Rows.Add(rowInfo);
                }
            }

            this.stajOsnGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            //     stajOsnGrid.Refresh();

            // Обновляем таблицу доп записей по стажу
            gridUpdate_StajLgot();
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton15_Click(object sender, EventArgs e)
        {
            StajOsnFrm child = new StajOsnFrm();
            child.Owner = this;
            child.action = "add";
            child.ParentFormName = "SZV_STAJ_Edit";
            child.dateControl = dateControlCheckBox.Checked;
            child.formData = new StajOsn();
            child.rowindex = -1;
            child.period = new RaschetPeriodContainer { Year = (short)Year.Value, Kvartal = 0, Name = "", DateBegin = new DateTime((int)Year.Value, 1, 1), DateEnd = new DateTime((int)Year.Value, 12, 31) };
            if (StajOsn_List.Count == 0)
            {
                child.StajBeginDate.Value = child.period.DateBegin;
            }
            else
            {
                var date = StajOsn_List.Max(x => x.DateEnd).Value;
                child.StajBeginDate.Value = date.AddDays(1);
                child.NumberSpin.Value = StajOsn_List.Max(x => x.Number).Value + 1;
            }

            child.StajEndDate.Value = child.period.DateEnd;

            if (child.StajBeginDate.Value > child.StajEndDate.Value)
            {
                child.StajEndDate.Value = child.StajBeginDate.Value.AddDays(1);
            }

            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
            if (child.formData != null)
            {
                StajOsn_List.Add(child.formData);
                gridUpdate_StajOsn();
            }


        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void radButton14_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int rowindex = stajOsnGrid.CurrentRow.Index;

                StajOsnFrm child = new StajOsnFrm();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ParentFormName = "SZV_STAJ_Edit";
                child.dateControl = dateControlCheckBox.Checked;
                child.action = "edit";
                child.period = new RaschetPeriodContainer { Year = (short)Year.Value, Kvartal = 0, Name = "", DateBegin = new DateTime((int)Year.Value, 1, 1), DateEnd = new DateTime((int)Year.Value, 12, 31) };
                child.formData = StajOsn_List.Skip(rowindex).Take(1).First();
                child.rowindex = rowindex;
                child.ShowDialog();


                if (child.formData != null)
                {
                    //    var rsw_ind = StajOsn_List.FindIndex(x => x.ID == child.formData.ID);
                    StajOsn_List.RemoveAt(rowindex);
                    StajOsn_List.Insert(rowindex, child.formData);

                    gridUpdate_StajOsn();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton13_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int rowindex = stajOsnGrid.CurrentRow.Index;
                StajOsn_List.RemoveAt(rowindex);
                gridUpdate_StajOsn();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void radGridView4_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            radButton14_Click(null, null);
        }

        private void radGridView4_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {


            gridUpdate_StajLgot();
        }

        #endregion

        #region Льготный стаж

        /// <summary>
        /// обновление таблицы раздела 6.8 Льготный стаж
        /// </summary>
        private void gridUpdate_StajLgot()
        {
            stajLgotGrid.Rows.Clear();

            if (StajOsn_List.Count > 0)
            {

                int rowindex = 0;

                if (stajOsnGrid.CurrentRow != null)
                {
                    rowindex = stajOsnGrid.CurrentRow.Index;
                }

                List<StajLgot> StajLgot_List = StajOsn_List.Skip(rowindex).Take(1).First().StajLgot.OrderBy(x => x.Number).ToList();

                if (StajLgot_List.Count() != 0)
                {
                    foreach (var item in StajLgot_List)
                    {
                        string str = item.IschislStrahStajDopID == null ? "" : item.IschislStrahStajDopID.HasValue ? db.IschislStrahStajDop.FirstOrDefault(x => x.ID == item.IschislStrahStajDopID).Code : "";
                        string s1 = item.Strah1Param.HasValue == true ? item.Strah1Param.Value.ToString() : "0";
                        string s2 = item.Strah2Param.HasValue == true ? item.Strah2Param.Value.ToString() : "0";

                        str = "[" + s1 + "][" + s2 + "][" + str + "]";

                        string s1_ = item.UslDosrNazn1Param.HasValue == true ? item.UslDosrNazn1Param.Value.ToString() : "0";
                        string s2_ = item.UslDosrNazn2Param.HasValue == true ? item.UslDosrNazn2Param.Value.ToString() : "0";
                        string s3_ = item.UslDosrNazn3Param.HasValue == true ? item.UslDosrNazn3Param.Value.ToString() : "0.00";

                        string str_ = "[" + s1_ + "][" + s2_ + "][" + s3_ + "]";

                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.stajLgotGrid.MasterView);
                        rowInfo.Cells["ID"].Value = item.ID;
                        rowInfo.Cells["Number"].Value = item.Number;
                        rowInfo.Cells["TerrUslCode"].Value = item.TerrUslID == null ? "" : item.TerrUslID.HasValue ? db.TerrUsl.FirstOrDefault(x => x.ID == item.TerrUslID.Value).Code : "";
                        rowInfo.Cells["TerrUslKoef"].Value = item.TerrUslKoef == null ? "" : item.TerrUslKoef.HasValue ? item.TerrUslKoef.Value.ToString() : "";
                        rowInfo.Cells["OsobUslCode"].Value = item.OsobUslTrudaID == null ? "" : item.OsobUslTrudaID.HasValue ? db.OsobUslTruda.FirstOrDefault(x => x.ID == item.OsobUslTrudaID).Code : "";
                        rowInfo.Cells["KodVredOsn"].Value = item.KodVred_OsnID == null ? "" : item.KodVred_OsnID.HasValue ? db.KodVred_2.FirstOrDefault(x => x.ID == item.KodVred_OsnID).Code : "";
                        rowInfo.Cells["IschislStrahOsn"].Value = item.IschislStrahStajOsnID == null ? "" : item.IschislStrahStajOsnID.HasValue ? db.IschislStrahStajOsn.FirstOrDefault(x => x.ID == item.IschislStrahStajOsnID).Code : "";
                        rowInfo.Cells["IschislStrahDop"].Value = str;
                        rowInfo.Cells["UslDosrNaznOsn"].Value = item.UslDosrNaznID == null ? "" : item.UslDosrNaznID.HasValue ? db.UslDosrNazn.FirstOrDefault(x => x.ID == item.UslDosrNaznID).Code : "";
                        rowInfo.Cells["UslDosrNaznDop"].Value = str_;
                        stajLgotGrid.Rows.Add(rowInfo);
                    }
                }

                //                this.stajLgotGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                //   stajLgotGrid.Refresh();
            }
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void radButton16_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount > 0)
            {
                StajLgotFrm child = new StajLgotFrm();
                child.Owner = this;
                child.action = "add";
                int rowindex = stajOsnGrid.CurrentRow.Index;
                child.StajOsnData = StajOsn_List.Skip(rowindex).Take(1).First();

                if (child.StajOsnData.StajLgot.Count != 0)
                {
                    child.NumberSpin.Value = child.StajOsnData.StajLgot.Max(x => x.Number).Value + 1;
                }

                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ShowDialog();
                if (child.formData != null)
                {
                    var item = child.formData;

                    StajLgot r = new StajLgot();

                    var fields = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    var names = Array.ConvertAll(fields, field => field.Name);

                    foreach (var itemName_ in names)
                    {
                        string itemName = itemName_.TrimStart('_');
                        var properties = item.GetType().GetProperty(itemName);
                        if (properties != null)
                        {
                            object value = properties.GetValue(item, null);
                            var data = value;

                            r.GetType().GetProperty(itemName).SetValue(r, data, null);
                        }

                    }
                    StajOsn_List.Skip(rowindex).Take(1).First().StajLgot.Add(r);

                    gridUpdate_StajLgot();
                }
            }

        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void radButton18_Click(object sender, EventArgs e)
        {
            if (stajLgotGrid.RowCount > 0)
            {
                int rowindex = stajLgotGrid.CurrentRow.Index;
                StajOsn st = StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First();

                StajLgotFrm child = new StajLgotFrm();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.StajOsnData = st;
                child.formData = st.StajLgot.Skip(rowindex).Take(1).First();
                child.rowindex = rowindex;
                child.ShowDialog();


                if (child.formData != null)
                {
                    //    var rsw_ind = StajOsn_List.FindIndex(x => x.ID == child.formData.ID);
                    //StajLgot_List.RemoveAt(rowindex);
                    //StajLgot_List.Insert(rowindex, child.formData);
                    List<StajLgot> stl = StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.ToList();
                    stl.RemoveAt(rowindex);
                    stl.Insert(rowindex, child.formData);

                    StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.Clear();

                    foreach (var item in stl.OrderByDescending(x => x.Number))
                    {

                        StajLgot r = new StajLgot();

                        var fields = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        var names = Array.ConvertAll(fields, field => field.Name);

                        foreach (var itemName_ in names)
                        {
                            string itemName = itemName_.TrimStart('_');
                            var properties = item.GetType().GetProperty(itemName);
                            if (properties != null)
                            {
                                object value = properties.GetValue(item, null);
                                var data = value;

                                r.GetType().GetProperty(itemName).SetValue(r, data, null);
                            }

                        }
                        StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.Add(r);
                    }


                    gridUpdate_StajLgot();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }
        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton17_Click(object sender, EventArgs e)
        {
            if (stajLgotGrid.RowCount > 0)
            {
                int rowindex = stajLgotGrid.CurrentRow.Index;
                List<StajLgot> stl = StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.ToList();
                stl.RemoveAt(rowindex);

                StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.Clear();

                foreach (var item in stl)
                {

                    StajLgot r = new StajLgot();

                    var fields = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    var names = Array.ConvertAll(fields, field => field.Name);

                    foreach (var itemName_ in names)
                    {
                        string itemName = itemName_.TrimStart('_');
                        var properties = item.GetType().GetProperty(itemName);
                        if (properties != null)
                        {
                            object value = properties.GetValue(item, null);
                            var data = value;

                            r.GetType().GetProperty(itemName).SetValue(r, data, null);
                        }

                    }
                    StajOsn_List.Skip(stajOsnGrid.CurrentRow.Index).Take(1).First().StajLgot.Add(r);
                }

                gridUpdate_StajLgot();

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }



        #endregion

        private void stajLgotGrid_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            radButton18_Click(null, null);
        }


        private void stajLgotGrid_SizeChanged(object sender, EventArgs e)
        {
            if (stajLgotGrid.Width >= 753)
            {
                stajLgotGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }
            else
            {
                stajLgotGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                stajLgotGrid.Columns[1].Width = 24;
                stajLgotGrid.Columns[2].Width = 82;
                stajLgotGrid.Columns[3].Width = 46;
                stajLgotGrid.Columns[4].Width = 90;
                stajLgotGrid.Columns[5].Width = 94;
                stajLgotGrid.Columns[6].Width = 100;
                stajLgotGrid.Columns[7].Width = 112;
                stajLgotGrid.Columns[8].Width = 100;
                stajLgotGrid.Columns[9].Width = 112;
            }
        }

        private void moveStajUP_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int index = stajOsnGrid.CurrentRow.Index;
                if (index != 0)
                {
                    long numPrev = StajOsn_List.Skip(index - 1).Take(1).First().Number.Value;
                    long numCurr = StajOsn_List.Skip(index).Take(1).First().Number.Value;
                    StajOsn_List.Skip(index).Take(1).First().Number--; // уменьшаем на 1 (поднимаем вверх)
                    if ((numCurr - numPrev) == 1) // если последовательность один за одним например 2, 3
                    {
                        StajOsn_List.Skip(index - 1).Take(1).First().Number++; // увеличиваем номер на 1 (опускаем ниже)
                        index--;
                    }
                    else // если в последовательности есть промежутки например 2, 4
                    {

                    }

                    gridUpdate_StajOsn();
                    stajOsnGrid.Rows[index].IsCurrent = true;
                }

            }
        }

        private void moveStajDOWN_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int index = stajOsnGrid.CurrentRow.Index;
                if (index != (stajOsnGrid.RowCount - 1))
                {
                    long numNext = StajOsn_List.Skip(index + 1).Take(1).First().Number.Value;
                    long numCurr = StajOsn_List.Skip(index).Take(1).First().Number.Value;
                    StajOsn_List.Skip(index).Take(1).First().Number++; // уменьшаем на 1 (поднимаем вверх)
                    if ((numNext - numCurr) == 1) // если последовательность один за одним например 2, 3
                    {
                        StajOsn_List.Skip(index + 1).Take(1).First().Number--; // увеличиваем номер на 1 (опускаем ниже)
                        index++;
                    }
                    else // если в последовательности есть промежутки например 2, 4
                    {

                    }

                    gridUpdate_StajOsn();
                    stajOsnGrid.Rows[index].IsCurrent = true;
                }

            }
        }

        private void moveStajAUTO_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                StajOsn_List = StajOsn_List.OrderBy(x => x.DateBegin.Value).ToList();
                int num = 1;
                foreach (var item in StajOsn_List)
                {
                    item.Number = num;
                    num++;
                }

                gridUpdate_StajOsn();
            }
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

        private void DateFilling_ValueChanged(object sender, EventArgs e)
        {
            if (DateFilling.Value != DateFilling.NullDate)
                DateFillingMaskedEditBox.Text = DateFilling.Value.ToShortDateString();
            else
                DateFillingMaskedEditBox.Text = DateFillingMaskedEditBox.NullText;
        }

        private void RSW2014_6_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (allowClose)
            {

                SZV_STAJ = null;
                db.Dispose();
            }
            else
            {
                DialogResult dialogResult = RadMessageBox.Show("Вы хотите сохранить изменения перед закрытием формы?", "Сохранение записи!", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button3);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        radButton1_Click(null, null);
                        break;
                    case DialogResult.No:
                        SZV_STAJ = null;
                        db.Dispose();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }

            }

            Props props = new Props(); //экземпляр класса с настройками
            List<WindowData> windowData = new List<WindowData> { };

            props.setFormParams(this, windowData);
        }

        private void OPSFeeNachCheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            OPSFeeNachCheckBox.Text = OPSFeeNachCheckBox.Checked ? "Да" : "Нет";
        }

        private void DopTarFeeNachCheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            DopTarFeeNachCheckBox.Text = DopTarFeeNachCheckBox.Checked ? "Да" : "Нет";

        }


        private void selectStaffBtn_Click(object sender, EventArgs e)
        {
            Staff staffLoad = null;

            var ssn = SNILS.Value.ToString().Split(' ');

            if (!ssn[0].Contains("_"))
            {
                string snils = ssn[0].Replace("-", "");
                if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils))
                {
                    staffLoad = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils);
                }
            }

            StaffFrm child = new StaffFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.InsID = Options.InsID;
            child.action = "selection";
            child.StaffID = staffLoad == null ? 0 : staffLoad.ID; ;
            child.ShowDialog();
            long id = child.StaffID;
            if (db.Staff.Any(x => x.ID == id))
            {
                staff = db.Staff.FirstOrDefault(x => x.ID == id);
                updateStaffInfo();
            }
        }

        private void razd5_addBtn_Click(object sender, EventArgs e)
        {
            SZV_STAJ_5_Edit child = new SZV_STAJ_5_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
            if (child.formData != null)
            {
                FormsSZV_STAJ_4_2017_List.Add(child.formData);
                SZV_STAJ_4_Grid_update();
            }
        }

        private void razd5_editBtn_Click(object sender, EventArgs e)
        {
            if (SZV_STAJ_4_Grid.RowCount != 0)
            {
                int rowindex = SZV_STAJ_4_Grid.CurrentRow.Index;

                SZV_STAJ_5_Edit child = new SZV_STAJ_5_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.formData = FormsSZV_STAJ_4_2017_List.Skip(rowindex).Take(1).First();
                child.ShowDialog();

                if (child.formData != null)
                {
                    FormsSZV_STAJ_4_2017_List.RemoveAt(rowindex);
                    FormsSZV_STAJ_4_2017_List.Insert(rowindex, child.formData);

                    SZV_STAJ_4_Grid_update();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void razd5_delBtn_Click(object sender, EventArgs e)
        {
            if (SZV_STAJ_4_Grid.RowCount != 0)
            {
                int rowindex = SZV_STAJ_4_Grid.CurrentRow.Index;
                FormsSZV_STAJ_4_2017_List.RemoveAt(rowindex);

                SZV_STAJ_4_Grid_update();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void loadRukBtn_Click(object sender, EventArgs e)
        {
            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            if (!String.IsNullOrEmpty(ins.BossDolgn))
            {
                ConfirmDolgn.Text = ins.BossDolgn;
            }
            if (!String.IsNullOrEmpty(ins.BossFIO))
            {
                ConfirmFIO.Text = ins.BossFIO;
            }
        }


        private void radLabel7_Click(object sender, EventArgs e)
        {
            if (RadMessageBox.Show("Все изменения общих данных, будут применены ко всем Формам СЗВ-СТАЖ привязанным к одной Форме ОДВ-1!", "Внимание", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                editAllRecords = true;

                TypeInfo.Enabled = true;
                OPSFeeNachCheckBox.Enabled = true;
                DopTarFeeNachCheckBox.Enabled = true;
                Year.Enabled = true;

                ConfirmDolgn.Enabled = true;
                ConfirmFIO.Enabled = true;

                razd5_addBtn.Enabled = true;
                razd5_delBtn.Enabled = true;
                razd5_editBtn.Enabled = true;

                loadRukBtn.Enabled = true;
                DateFilling.Enabled = true;
                DateFillingMaskedEditBox.Enabled = true;

                change_TypeInfo();
            }
        }

        private void radLabel7_MouseHover(object sender, EventArgs e)
        {
            radLabel7.Font = new Font(radLabel7.Font, FontStyle.Underline | FontStyle.Bold);
        }

        private void radLabel7_MouseLeave(object sender, EventArgs e)
        {
            radLabel7.Font = new Font(radLabel7.Font, FontStyle.Bold);
        }











































    }
}
