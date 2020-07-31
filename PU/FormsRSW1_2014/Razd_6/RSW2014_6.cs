using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace PU.FormsRSW2014
{
    public partial class RSW2014_6 : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public Staff staff { get; set; }
        public int defaultPage = 0;
        public FormsRSW2014_1_Razd_6_1 RSW_6 { get; set; }
        public List<FormsRSW2014_1_Razd_6_4> RSW_6_4_List = new List<FormsRSW2014_1_Razd_6_4>();
        public List<FormsRSW2014_1_Razd_6_6> RSW_6_6_List = new List<FormsRSW2014_1_Razd_6_6>();
        public List<FormsRSW2014_1_Razd_6_7> RSW_6_7_List = new List<FormsRSW2014_1_Razd_6_7>();
        public List<StajOsn> StajOsn_List = new List<StajOsn>();
        //    public List<StajLgot> StajLgot_List = new List<StajLgot>();
        private List<string> errMessBox = new List<string>();

        public byte period { get; set; }
        public byte CorrNum { get; set; }
        bool allowClose = false;

        public string parentName { get; set; }

        public RSW2014_6()
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
        /// Сохранение записи из раздела 6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton1_Click(object sender, EventArgs e)
        {
            if (Quarter.SelectedItem != null)
                setPeriod();
            if (validation())
            {
                bool flag_ok = false;

                RSW_6.DateFilling = DateTime.Now;
                RSW_6.Year = short.Parse(Year.Text);
                RSW_6.Quarter = period;
                //      RSW_6.CorrectionNum = CorrNum;
                RSW_6.InsurerID = Options.InsID;
                RSW_6.Staff = staff;
                RSW_6.TypeInfoID = long.Parse(TypeInfo.SelectedItem.Value.ToString());
                RSW_6.SumFeePFR = Math.Round(decimal.Parse(SumFeePFR.Value.ToString()), 2, MidpointRounding.AwayFromZero);
                RSW_6.AutoCalc = AutoCalcSwitch.IsOn;
                RSW_6.DateFilling = DateFilling.Value.Date;

                if (TypeInfo.SelectedIndex != 0)
                {
                    string regN = "";
                    if (KorrRegNum.Value.ToString() != "___-___-______")
                    {
                        var s = (this.KorrRegNum.Value.ToString()).Split('-');
                        foreach (var item in s)
                        {
                            if (!item.Contains("_"))
                                regN += item;
                            else
                            {
                                regN = "";
                                break;
                            }
                        }
                    }

                    RSW_6.RegNumKorr = regN;

                    RSW_6.YearKorr = short.Parse(KorrYear.SelectedItem.Text);
                    RSW_6.QuarterKorr = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
                }

                switch (action)
                {
                    case "add":
                        try
                        {
                            db.AddToFormsRSW2014_1_Razd_6_1(RSW_6);
                            db.SaveChanges();

                            foreach (var item in RSW_6_4_List)
                            {
                                item.FormsRSW2014_1_Razd_6_1_ID = RSW_6.ID;
                                FormsRSW2014_1_Razd_6_4 r = new FormsRSW2014_1_Razd_6_4();

                                var fields = typeof(FormsRSW2014_1_Razd_6_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                                db.AddToFormsRSW2014_1_Razd_6_4(r);
                            }
                            foreach (var item in RSW_6_6_List)
                            {
                                item.FormsRSW2014_1_Razd_6_1_ID = RSW_6.ID;

                                FormsRSW2014_1_Razd_6_6 r = new FormsRSW2014_1_Razd_6_6();

                                var fields = typeof(FormsRSW2014_1_Razd_6_6).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                                db.FormsRSW2014_1_Razd_6_6.AddObject(r);
                            }
                            foreach (var item in RSW_6_7_List)
                            {
                                item.FormsRSW2014_1_Razd_6_1_ID = RSW_6.ID;
                                FormsRSW2014_1_Razd_6_7 r = new FormsRSW2014_1_Razd_6_7();

                                var fields = typeof(FormsRSW2014_1_Razd_6_7).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                                db.FormsRSW2014_1_Razd_6_7.AddObject(r);
                            }


                            var fields_lgot = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names_lgot = Array.ConvertAll(fields_lgot, field => field.Name);

                            if (TypeInfo.SelectedIndex != 2) // если отменяющая то стаж не сохраняем
                            {

                                foreach (var item in StajOsn_List)
                                {
                                    item.FormsRSW2014_1_Razd_6_1_ID = RSW_6.ID;
                                    StajOsn r = new StajOsn();

                                    var fields = typeof(StajOsn).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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
                                    db.StajOsn.AddObject(r);
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

                                        db.AddToStajLgot(r_);
                                    }

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
                            if (flag_ok)
                                this.Close();
                        }


                        break;
                    case "edit":   // режим редактирования записи


                        // выбираем из базы исходную запись по идешнику
                        db = new pu6Entities();
                        FormsRSW2014_1_Razd_6_1 r6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.ID == RSW_6.ID);
                        try
                        {

                            r6.DateFilling = RSW_6.DateFilling;
                            r6.Year = RSW_6.Year;
                            r6.Quarter = RSW_6.Quarter;
                            r6.StaffID = RSW_6.StaffID;
                            r6.TypeInfoID = RSW_6.TypeInfoID;
                            r6.SumFeePFR = RSW_6.SumFeePFR;
                            r6.RegNumKorr = RSW_6.RegNumKorr;
                            r6.YearKorr = RSW_6.YearKorr;
                            r6.QuarterKorr = RSW_6.QuarterKorr;


                            // сохраняем модифицированную запись обратно в бд
                            db.ObjectStateManager.ChangeObjectState(r6, EntityState.Modified);
                            db.SaveChanges();
                            flag_ok = true;

                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранение данных Раздела 6.1 произошла ошибка. Код ошибки: " + ex.Message);
                        }
                        if (flag_ok)
                        {
                            flag_ok = false;
                            //выбираем данные о выплатах и прочее (разделы 6.4,6.6,6.7) и сравниваем что у нас есть в текущей версии после редактирования, в зависимости - пропускаем, удаляем, редактируем


                            #region обрабатываем записи о выплатах из Раздела 6.4
                            try
                            {
                                var RSW_6_4_List_from_db = db.FormsRSW2014_1_Razd_6_4.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == r6.ID);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_6_4_List.Select(x => x.ID);
                                var list_for_del = RSW_6_4_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_1_Razd_6_4.DeleteObject(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_6_4_List_from_db = db.FormsRSW2014_1_Razd_6_4.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == r6.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 6.4 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_1_Razd_6_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_6_4_List)
                                {
                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_6_4_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_1_Razd_6_4 rsw_temp = RSW_6_4_List_from_db.Single(x => x.ID == item.ID);


                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
                                            if (item_.IndexOf("FormsRSW2014_1_Razd_6_1_ID") < 0)
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

                                            db.ObjectStateManager.ChangeObjectState(rsw_temp, EntityState.Modified);

                                        }

                                    }
                                    if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        item.FormsRSW2014_1_Razd_6_1_ID = RSW_6.ID;
                                        FormsRSW2014_1_Razd_6_4 r = new FormsRSW2014_1_Razd_6_4();

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

                                        db.AddToFormsRSW2014_1_Razd_6_4(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);

                            }
                            #endregion

                            #region обрабатываем записи о выплатах из Раздела 6.6
                            try
                            {
                                var RSW_6_6_List_from_db = db.FormsRSW2014_1_Razd_6_6.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == r6.ID);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_6_6_List.Select(x => x.ID);
                                var list_for_del = RSW_6_6_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_1_Razd_6_6.DeleteObject(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_6_6_List_from_db = db.FormsRSW2014_1_Razd_6_6.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == r6.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 6.6 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_1_Razd_6_6).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_6_6_List)
                                {
                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_6_6_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_1_Razd_6_6 rsw_temp = RSW_6_6_List_from_db.Single(x => x.ID == item.ID);


                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
                                            if (item_.IndexOf("FormsRSW2014_1_Razd_6_1_ID") < 0)
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

                                            db.ObjectStateManager.ChangeObjectState(rsw_temp, EntityState.Modified);

                                        }

                                    }
                                    if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        item.FormsRSW2014_1_Razd_6_1_ID = RSW_6.ID;
                                        FormsRSW2014_1_Razd_6_6 r = new FormsRSW2014_1_Razd_6_6();

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

                                        db.AddToFormsRSW2014_1_Razd_6_6(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);

                            }
                            #endregion

                            #region обрабатываем записи о выплатах из Раздела 6.7
                            try
                            {
                                var RSW_6_7_List_from_db = db.FormsRSW2014_1_Razd_6_7.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == r6.ID);

                                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                var t = RSW_6_7_List.Select(x => x.ID);
                                var list_for_del = RSW_6_7_List_from_db.Where(x => !t.Contains(x.ID));

                                foreach (var item in list_for_del)
                                {
                                    db.FormsRSW2014_1_Razd_6_7.DeleteObject(item);
                                }

                                if (list_for_del.Count() != 0)
                                {
                                    //db.SaveChanges();
                                    RSW_6_7_List_from_db = db.FormsRSW2014_1_Razd_6_7.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == r6.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                }


                                // проверка текущих записей Раздела 6.7 на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то удаляем ее и добавляем заново) или добавления новых (необходимо добавить в БД)

                                var fields = typeof(FormsRSW2014_1_Razd_6_7).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                var names = Array.ConvertAll(fields, field => field.Name);


                                foreach (var item in RSW_6_7_List)
                                {
                                    bool flag_add_new = true;
                                    //если такая запись есть, надо проверять на отличия
                                    if (RSW_6_7_List_from_db.Any(x => x.ID == item.ID))
                                    {
                                        flag_add_new = false;
                                        bool flag_edited = false;
                                        FormsRSW2014_1_Razd_6_7 rsw_temp = RSW_6_7_List_from_db.Single(x => x.ID == item.ID);


                                        foreach (var item_ in names)
                                        {
                                            string itemName = item_.TrimStart('_');
                                            if (item_.IndexOf("FormsRSW2014_1_Razd_6_1_ID") < 0)
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

                                            db.ObjectStateManager.ChangeObjectState(rsw_temp, EntityState.Modified);

                                        }

                                    }
                                    if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                    {

                                        // добавление записи в БД
                                        item.FormsRSW2014_1_Razd_6_1_ID = RSW_6.ID;
                                        FormsRSW2014_1_Razd_6_7 r = new FormsRSW2014_1_Razd_6_7();

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

                                        db.AddToFormsRSW2014_1_Razd_6_7(r);
                                    }


                                }

                                flag_ok = true;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);

                            }

                            #endregion

                            #region обрабатываем записи о Стаже (из Раздела 6.8)
                            try
                            {
                                if (TypeInfo.SelectedIndex == 2) // если отменяющая то стаж удаляем
                                {
                                    var list_for_del = db.StajOsn.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == r6.ID);
                                    foreach (var item in list_for_del)
                                    {
                                        if (item.StajLgot.Any())
                                        {
                                            List<long> l_id = item.StajLgot.Select(x => x.ID).ToList();
                                            foreach (var stl in l_id)
                                            {
                                                StajLgot l = db.StajLgot.FirstOrDefault(x => x.ID == stl);
                                                db.StajLgot.DeleteObject(l);
                                            }
                                        }

                                        db.StajOsn.DeleteObject(item);
                                    }
                                }
                                else
                                {
                                    var StajOsn_List_from_db = db.StajOsn.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == r6.ID);

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
                                                db.StajLgot.DeleteObject(l);
                                            }
                                        }

                                        db.StajOsn.DeleteObject(item);
                                    }

                                    if (list_for_del.Count() != 0)
                                    {
                                        //db.SaveChanges();
                                        StajOsn_List_from_db = db.StajOsn.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == r6.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
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
                                                    db.StajLgot.DeleteObject(item_lgot);
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

                                                            db.ObjectStateManager.ChangeObjectState(lgot_temp, EntityState.Modified);

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

                                                        db.AddToStajLgot(r);
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
                                                    if (item_.IndexOf("FormsRSW2014_1_Razd_6_1_ID") < 0)
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

                                                    db.ObjectStateManager.ChangeObjectState(rsw_temp, EntityState.Modified);

                                                }
                                            }

                                        }
                                        if (flag_add_new && flag_ok_lgot) // такой записи в базе нет, значит просто добавляем ее
                                        {

                                            // добавление записи в БД
                                            item.FormsRSW2014_1_Razd_6_1_ID = RSW_6.ID;
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

                                            db.AddToStajOsn(r);
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

                                                db.AddToStajLgot(r_);
                                            }

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
                                if (flag_ok)
                                    this.Close();
                            }
                        }

                        break;

                }


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

            if (Quarter.Text == "")
                errMessBox.Add("Отчетный период должен быть заполнен!");

            if (staff == null)
            {
                errMessBox.Add("Необходимо выбрать сотрудника");
                check = false;
                return check;
            }

            short.TryParse(Year.Text, out y);

            long typeInfoID = long.Parse(TypeInfo.SelectedItem.Value.ToString());

            if (TypeInfo.SelectedIndex != 0)
            {
                if (KorrYear.Text == "")
                    errMessBox.Add("Корр. календарный год должен быть заполнен!");

                if (KorrQuarter.Text == "")
                    errMessBox.Add("Корр. отчетный период должен быть заполнен!");

                short yearKorr = 0;
                short.TryParse(KorrYear.Text, out yearKorr);

                byte quarterKorr = 0;
                if (KorrQuarter.SelectedItem != null)
                {
                    byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out quarterKorr);
                }

                switch (action)
                {
                    case "add":
                        if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == period && x.Year == yearKorr && x.Quarter == quarterKorr && x.TypeInfoID == typeInfoID && x.InsurerID == Options.InsID && x.StaffID == staff.ID))
                            errMessBox.Add("Дублирование записи по ключу уникальности");
                        break;
                    case "edit":
                        if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == period && x.Year == yearKorr && x.Quarter == quarterKorr && x.TypeInfoID == typeInfoID && x.InsurerID == Options.InsID && x.StaffID == staff.ID && x.ID != RSW_6.ID))
                            errMessBox.Add("Дублирование записи по ключу уникальности");
                        break;
                }

            }
            else  // Без корректируемого периода
            {
                switch (action)
                {
                    case "add":
                        if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == period && x.TypeInfoID == typeInfoID && x.InsurerID == Options.InsID && x.StaffID == staff.ID))
                            errMessBox.Add("Дублирование записи по ключу уникальности");
                        break;
                    case "edit":
                        if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == period && x.TypeInfoID == typeInfoID && x.InsurerID == Options.InsID && x.StaffID == staff.ID && x.ID != RSW_6.ID))
                            errMessBox.Add("Дублирование записи по ключу уникальности");
                        break;
                }

            }

            if (TypeInfo.SelectedIndex != 0)
            {
                RSW_6.RegNumKorr = KorrRegNum.Value.ToString();

                short yearKorr = 0;
                short.TryParse(KorrYear.Text, out yearKorr);

                byte quarterKorr = 0;
                if (KorrQuarter.SelectedItem != null)
                {
                    byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out quarterKorr);
                }

                RSW_6.YearKorr = yearKorr;
                RSW_6.QuarterKorr = quarterKorr;
            }



            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            long id = staff == null ? 0 : staff.ID;

            StaffFrm child = new StaffFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.InsID = Options.InsID;
            child.action = "selection";
            child.StaffID = id;
            child.ShowDialog();
            id = (child.StaffID == 0) ? 0 : child.StaffID;

            staff = db.Staff.FirstOrDefault(x => x.ID == id);
            if (staff != null)
            {
                LastName.Text = staff.LastName;
                FirstName.Text = staff.FirstName;
                MiddleName.Text = staff.MiddleName;
                SNILS.Text = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber.Value) : "";
                Tabel.Text = staff.TabelNumber != null ? staff.TabelNumber.Value.ToString() : "";
            }
            else
            {
                LastName.ResetText();
                FirstName.ResetText();
                MiddleName.ResetText();
                SNILS.ResetText();
                Tabel.ResetText();
            }





        }


        /// <summary>
        /// Загрузка формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RSW2014_6_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            this.Cursor = Cursors.WaitCursor;

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

            this.radPageView1.SelectedPage = this.radPageView1.Pages[defaultPage];
            DateFilling.Value = DateTime.Now.Date;
            try
            {
                foreach (var item in db.TypeInfo)
                {
                    TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
                }

                var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014 && x.Year <= 2018).OrderBy(x => x.Year);
                short y;

                switch (action)
                {
                    case "add":
                        RSW_6 = new FormsRSW2014_1_Razd_6_1();
                        TypeInfo.Items.Single(x => x.Text == "Исходная").Selected = true;

                        switch (parentName)
                        {
                            case "RSW2014_List":
                                #region Отчетный период
                                this.Year.Items.Clear();
                                y = 0;
                                foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
                                {
                                    Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                                }

                                long staffid = staff.ID;

                                staff = new Staff();
                                staff = db.Staff.FirstOrDefault(x => x.ID == staffid);

                                Year.Enabled = true;
                                Quarter.Enabled = true;
                                radButton3.Enabled = false;
                                LastName.Text = staff.LastName;
                                FirstName.Text = staff.FirstName;
                                MiddleName.Text = staff.MiddleName;
                                SNILS.Text = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber.Value) : "";
                                Tabel.Text = staff.TabelNumber != null ? staff.TabelNumber.Value.ToString() : "";


                                // выпад список "календарный год"

                                if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                                    Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                                else
                                    Year.Items.OrderByDescending(x => x.Value).First().Selected = true;

                                // выпад список "Отчетный период"

                                this.Quarter.Items.Clear();

                                if (short.TryParse(Year.SelectedItem.Text, out y))
                                {
                                    foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                                    {
                                        Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                                    }
                                    DateTime dt = DateTime.Now.AddDays(-45);

                                    RaschetPeriodContainer rp = new RaschetPeriodContainer();

                                    if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                                    else
                                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                                    if (rp != null)
                                        Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                                    else
                                        Quarter.Items.OrderByDescending(x => x.Value).First().Selected = true;
                                }
                                getBase();
                                setPeriod();

                                #endregion

                                break;
                            case "RSW2014_Edit":
                                Year.Enabled = false;
                                Quarter.Enabled = false;
                                RSW_6.Year = short.Parse(Year.Text);
                                RSW_6.Quarter = period;
                                //   RSW_6.CorrectionNum = CorrNum;
                                break;
                        }

                        break;
                    case "edit":
                        #region Отчетный период

                        // выпад список "календарный год"
                        this.Year.Items.Clear();
                        foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
                        {
                            Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                        }
                        y = 0;


                        if (Year.Items.Any(x => x.Text.ToString() == RSW_6.Year.ToString()))
                        {
                            Year.Items.Single(x => x.Text.ToString() == RSW_6.Year.ToString()).Selected = true;
                            // выпад список "Отчетный период"

                            this.Quarter.Items.Clear();

                            if (short.TryParse(Year.Text, out y))
                            {
                                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                                {
                                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                                }
                            }

                            if (Quarter.Items.Any(x => x.Value.ToString() == RSW_6.Quarter.ToString()))
                                Quarter.Items.FirstOrDefault(x => x.Value.ToString() == RSW_6.Quarter.ToString()).Selected = true;
                        }


                        #endregion

                        DateFilling.Value = RSW_6.DateFilling;

                        AutoCalcSwitch.IsOn = RSW_6.AutoCalc.HasValue ? RSW_6.AutoCalc.Value : false;

                        TypeInfo.Items.Single(x => x.Value.ToString() == RSW_6.TypeInfoID.ToString()).Selected = true;
                        change_TypeInfo();

                        if (RSW_6.QuarterKorr != null && KorrQuarter.Items.Any(x => x.Value.ToString() == RSW_6.QuarterKorr.ToString()))
                            KorrQuarter.Items.Single(x => x.Value.ToString() == RSW_6.QuarterKorr.ToString()).Selected = true;



                        //Информация о сотруднике
                        staff = RSW_6.Staff;

                        LastName.Text = staff.LastName;
                        FirstName.Text = staff.FirstName;
                        MiddleName.Text = staff.MiddleName;
                        if (!String.IsNullOrEmpty(staff.InsuranceNumber) && staff.ControlNumber != null)
                            SNILS.Text = Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber.Value);
                        else if (!String.IsNullOrEmpty(staff.InsuranceNumber))
                            SNILS.Text = staff.InsuranceNumber;

                        Tabel.Text = staff.TabelNumber != null ? staff.TabelNumber.Value.ToString() : "";


                        switch (parentName)
                        {
                            case "RSW2014_List":
                                Year.Enabled = true;
                                Quarter.Enabled = true;
                                radButton3.Enabled = false;
                                getBase();
                                break;
                            case "RSW2014_Edit":
                                Year.Enabled = false;
                                Quarter.Enabled = false;
                                break;
                        }

                        break;
                }

                if (Quarter.SelectedItem != null)
                {
                    this.Text = "Раздел 6. Сведения о сумме выплат и иных вознаграждений и страховом стаже  - [ " + Quarter.SelectedItem.Text + " ]";
                }


                this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
                this.KorrYear.SelectedIndexChanged += (s, с) => KorrYear_SelectedIndexChanged();
                this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();


                var rsw_6_4 = RSW_6.FormsRSW2014_1_Razd_6_4;

                if (RSW_6_4_List != null)
                    RSW_6_4_List.Clear();
                foreach (var item in rsw_6_4)
                {
                    RSW_6_4_List.Add(item);
                }

                gridUpdate_6_4();

                SumFeePFR.Value = RSW_6.SumFeePFR == null ? 0 : RSW_6.SumFeePFR.Value;

                var rsw_6_6 = RSW_6.FormsRSW2014_1_Razd_6_6;

                if (RSW_6_6_List != null)
                    RSW_6_6_List.Clear();
                foreach (var item in rsw_6_6)
                {
                    RSW_6_6_List.Add(item);
                }

                gridUpdate_6_6();

                var rsw_6_7 = RSW_6.FormsRSW2014_1_Razd_6_7;

                if (RSW_6_7_List != null)
                    RSW_6_7_List.Clear();
                foreach (var item in rsw_6_7)
                {
                    RSW_6_7_List.Add(item);
                }

                gridUpdate_6_7();

                var stajOsn = RSW_6.StajOsn;

                if (StajOsn_List != null)
                    StajOsn_List.Clear();
                foreach (var item in stajOsn)
                {
                    StajOsn_List.Add(item);
                }

                gridUpdate_StajOsn();




                /*         if (StajLgot_List != null)
                             StajLgot_List.Clear();

                         if (StajOsn_List.Count > 0)
                         {
                             long index = 0;
                    
                             if (long.TryParse(radGridView4.CurrentRow.Cells[0].Value.ToString(), out index))
                             {
                                 var stajLgot = db.StajOsn.FirstOrDefault(x => x.ID == index).StajLgot;
                                 foreach (var item in stajLgot)
                                 {
                                     StajLgot_List.Add(item);
                                 }
                             }

                         }
                 * */
                //            gridUpdate_StajLgot();

                long id_old = 0;
                switch (action)
                {
                    case "add":

                        break;
                    case "edit":
                        id_old = RSW_6.ID;
                        RSW_6 = new FormsRSW2014_1_Razd_6_1 { ID = id_old };
                        break;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void getBase()
        {

            if (TypeInfo.SelectedIndex == 0 && !String.IsNullOrEmpty(Year.Text))
            {
                short y;

                if (short.TryParse(Year.Text, out y))
                {
                    if (db.MROT.Any(x => x.Year == y))
                        Options.mrot = db.MROT.First(x => x.Year == y);
                    else
                        Options.mrot = null;
                }
            }
            else if (TypeInfo.SelectedIndex > 0 && !String.IsNullOrEmpty(KorrYear.Text))
            {
                short y;

                if (short.TryParse(KorrYear.Text, out y))
                {
                    if (db.MROT.Any(x => x.Year == y))
                        Options.mrot = db.MROT.First(x => x.Year == y);
                    else
                        Options.mrot = null;
                }
            }
            else
            {
                Options.mrot = null;
            }
        }


        private void Year_SelectedIndexChanged()
        {
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            this.Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }

                if (Quarter.Items.Count() > 0)
                {
                    if (q != 20 && Quarter.Items.Any(x => x.Value.ToString() == q.ToString()))
                        Quarter.Items.FirstOrDefault(x => x.Value.ToString() == q.ToString()).Selected = true;
                    else
                        Quarter.Items.First().Selected = true;
                }
            }
            getBase();

        }

        private void KorrYear_SelectedIndexChanged()
        {
            // выпад список "Отчетный период"
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year <= DateTime.Now.Year);

            byte q = 20;
            if (KorrQuarter.SelectedItem != null && byte.TryParse(KorrQuarter.SelectedItem.Value.ToString(), out q)) { }

            this.KorrQuarter.Items.Clear();

            short y;
            if (KorrYear.SelectedItem != null && short.TryParse(KorrYear.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    KorrQuarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                if (KorrQuarter.Items.Count() > 0)
                {
                    if (q != 20 && KorrQuarter.Items.Any(x => x.Value.ToString() == q.ToString()))
                        KorrQuarter.Items.FirstOrDefault(x => x.Value.ToString() == q.ToString()).Selected = true;
                    else
                        KorrQuarter.Items.First().Selected = true;
                }

                /*      DateTime dt = DateTime.Now.Date;
                      switch (action)
                      {
                          case "add":

                              RaschetPeriodContainer rp = new RaschetPeriodContainer();

                              if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                                  rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                              else
                                  rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                              if (rp != null)
                                  KorrQuarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                              else
                                  KorrQuarter.Items.OrderByDescending(x => x.Value).First().Selected = true;
                              break;
                          case "edit":
                              if (RSW_6.QuarterKorr != null && KorrQuarter.Items.Any(x => x.Value.ToString() == RSW_6.QuarterKorr.ToString()))
                                  KorrQuarter.Items.Single(x => x.Value.ToString() == RSW_6.QuarterKorr.ToString()).Selected = true;
                              else
                              {
                                  rp = new RaschetPeriodContainer();

                                  if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                                      rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                                  else
                                      rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                                  if (rp != null)
                                      KorrQuarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                                  else
                                      KorrQuarter.Items.OrderByDescending(x => x.Value).First().Selected = true;

                              }
                              break;
                      }*/

            }
            getBase();

        }

        private void Quarter_SelectedIndexChanged()
        {
            if (Quarter.SelectedItem != null)
            {
                setPeriod();
                this.Text = "Раздел 6. Сведения о сумме выплат и иных вознаграждений и страховом стаже  - [ " + Quarter.SelectedItem.Text + " ]";

            }
        }

        private void setPeriod()
        {
            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                byte q;
                if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                {
                    period = q;
                }
            }

        }

        private void change_TypeInfo()
        {
            if (TypeInfo.SelectedIndex == 0)
            {
                KorrYear.Items.Clear();
                KorrQuarter.Items.Clear();
                KorrRegNum.ResetText();
                KorrYear.Enabled = false;
                KorrQuarter.Enabled = false;
                KorrRegNum.Enabled = false;
                radGroupBox5.Enabled = true;
                radPageView1.Pages[2].Enabled = true;
                radPageView1.Pages[3].Enabled = true;

            }
            else
            {
                KorrYear.Enabled = true;
                KorrQuarter.Enabled = true;
                KorrRegNum.Enabled = true;


                radPageView1.Pages[2].Enabled = false;

                if (TypeInfo.SelectedIndex == 1)
                {
                    radPageView1.Pages[3].Enabled = true;
                    radGroupBox5.Enabled = true;
                }
                else if (TypeInfo.SelectedIndex == 2)
                {
                    radPageView1.Pages[3].Enabled = false;
                    radGroupBox5.Enabled = false;
                }

                short y = 2014;
                try
                {
                    y = short.Parse(Year.SelectedItem.Text);
                }
                catch
                {
                }

                var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014 && x.Year <= y).OrderBy(x => x.Year);

                // выпад список "календарный год"

                this.KorrYear.Items.Clear();

                foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
                {
                    KorrYear.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                }


                switch (action)
                {
                    case "add":
                        if (KorrYear.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                            KorrYear.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                        break;
                    case "edit":
                        if (RSW_6.YearKorr != null)
                            KorrYear.Items.Single(x => x.Text.ToString() == RSW_6.YearKorr.ToString()).Selected = true;
                        else if (KorrYear.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                            KorrYear.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;


                        if (RSW_6.RegNumKorr != null)
                        {
                            KorrRegNum.Value = RSW_6.RegNumKorr.ToString();
                        }

                        break;
                }

                KorrYear_SelectedIndexChanged();



            }


        }

        private void TypeInfo_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            change_TypeInfo();
            getBase();
        }



        #region Раздел 6.4-6.5


        /// <summary>
        /// Добавление новой записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton4_Click(object sender, EventArgs e)
        {
            if (staff != null)
            {
                RSW2014_6_4_Edit child = new RSW2014_6_4_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.staffID = staff.ID;
                short y = Year.Text != "" ? short.Parse(Year.Text) : (short)0;
                byte q = period;
                if (TypeInfo.SelectedIndex > 0 && (KorrYear.Text != "" && KorrQuarter.Text != ""))
                {
                    y = short.Parse(KorrYear.Text);
                    q = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
                }
                child.periodQ = q;
                child.periodY = y;

                child.autoCalc = AutoCalcSwitch.IsOn;
                child.platCatList = new List<long>();
                foreach (var item in RSW_6_4_List)
                {
                    child.platCatList.Add(item.PlatCategoryID);
                }
                child.ShowDialog();
                if (child.formData != null)
                {
                    if (!RSW_6_4_List.Any(x => x.PlatCategoryID == child.formData.PlatCategoryID))
                    {
                        RSW_6_4_List.Add(child.formData);
                        gridUpdate_6_4();
                    }
                    else
                    {
                        RadMessageBox.Show(this, "Дублирование записи по ключу уникальности", "Внимание!");
                    }
                }
            }
            else
            {
                RadMessageBox.Show("Необходимо сначала выбрать сотрудника!");
            }
        }

        /// <summary>
        /// обновление таблицы раздела 6.4
        /// </summary>
        private void gridUpdate_6_4()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            radGridView1.Rows.Clear(); // If dgv is bound to datatable
            //            radGridView1.DataSource = null;

            /*            int width = 0;
                        for (int i = 0; i < radGridView1.ColumnCount - 1; i++)
                        {
                            if (radGridView1.Columns[i].IsVisible)
                                width = width + radGridView1.Columns[i].Width;
                        }


                        radGridView1.Columns["s_0_3"].Width = radGridView1.Width - width;
                        */
            if (RSW_6_4_List.Count() != 0)
            {
                foreach (var item in RSW_6_4_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView1.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Category"].Value = item.PlatCategory.Code;
                    rowInfo.Cells["s_0_0"].Value = item.s_0_0.HasValue ? item.s_0_0.Value : 0;
                    rowInfo.Cells["s_0_1"].Value = item.s_0_1.HasValue ? item.s_0_1.Value : 0;
                    rowInfo.Cells["s_0_2"].Value = item.s_0_2.HasValue ? item.s_0_2.Value : 0;
                    rowInfo.Cells["s_0_3"].Value = item.s_0_3.HasValue ? item.s_0_3.Value : 0;
                    radGridView1.Rows.Add(rowInfo);
                }


                int rowindex = radGridView1.CurrentRow.Index;
                FormsRSW2014_1_Razd_6_4 rsw = RSW_6_4_List.Skip(rowindex).Take(1).First();
                updateTextBoxes_Razd_6_4(rsw);
            }
            else
            {
                updateTextBoxes_Razd_6_4(new FormsRSW2014_1_Razd_6_4());
            }

            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            radGridView1.Refresh();


            short y = short.Parse(Year.Text);
            if (TypeInfo.SelectedItem.Value.ToString() == "2" && KorrYear.SelectedItem != null)
            {
                short.TryParse(KorrYear.SelectedItem.Text, out y);
            }

            MethodsNonStatic methods = new MethodsNonStatic();
            SumFeePFR.Value = methods.UpdateSumFeePFR(false, RSW_6_4_List, y);
        }


        /// <summary>
        /// Обновление поля Начислено страховых взносов на ОПС
        /// </summary>
        //public decimal UpdateSumFeePFR(bool manual, List<FormsRSW2014_1_Razd_6_4> RSW64List, short y)
        //{
        //    decimal sum = 0;
        //    if ((RSW64List != null) && (RSW64List.Count != 0))
        //    {
        //        foreach (var item in RSW64List)
        //        {
        //            if ((item.PlatCategory.TariffPlat.Any(x => x.Year == y)) && (item.PlatCategory.TariffPlat.First(x => x.Year == y).StrahPercant1966 != null))
        //            {
        //                decimal s1 = item.s_1_1 != null ? item.s_1_1.Value : 0;
        //                decimal s2 = item.s_2_1 != null ? item.s_2_1.Value : 0;
        //                decimal s3 = item.s_3_1 != null ? item.s_3_1.Value : 0;

        //                decimal sumAll = s1 + s2 + s3;

        //                if (Options.mrot != null)
        //                {
        //                    sumAll = sumAll <= Options.mrot.NalogBase ? sumAll : Options.mrot.NalogBase;
        //                }

        //                sum = sum + (sumAll * item.PlatCategory.TariffPlat.First(x => x.Year == y).StrahPercant1966.Value / 100);
        //            }
        //            else
        //            {
        //                if (manual == true)
        //                    MessageBox.Show("Категория плательщика: " + item.PlatCategory.Code + "\r\nНе определен тариф Страховых взносов\r\nРасчет взносов по этой категории производиться не будет!");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        sum = 0;
        //    }

        //    return sum;
        //}

        /// <summary>
        /// Обновление значений в полях быстрого просмотра под таблицей при изменении текущей строки Раздела 6.4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridView1_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            int rowindex = radGridView1.CurrentRow != null ? radGridView1.CurrentRow.Index : 0;

            //      long id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());

            if (radGridView1.RowCount > 0)
            {
                FormsRSW2014_1_Razd_6_4 rsw = RSW_6_4_List.Skip(rowindex).Take(1).First();
                updateTextBoxes_Razd_6_4(rsw);
            }



        }

        /// <summary>
        /// Функция обновления значений в полях быстрого просмотра под таблицей Раздела 6.4
        /// </summary>
        /// <param name="rsw"></param>
        private void updateTextBoxes_Razd_6_4(FormsRSW2014_1_Razd_6_4 rsw)
        {
            var fields = typeof(FormsRSW2014_1_Razd_6_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_1") || itemName.StartsWith("s_2") || itemName.StartsWith("s_3"))
                {
                    RadLabel label = (RadLabel)this.Controls.Find("radPageViewPage2", true)[0].Controls.Find(itemName, true)[0];

                    string data = "0.00";
                    if (rsw != null)
                    {
                        var properties = rsw.GetType().GetProperty(itemName);
                        object value = properties.GetValue(rsw, null);
                        if (value != null)
                        {
                            data = Utils.decToStr(decimal.Parse(value.ToString()));
                        }
                    }

                    label.Text = data;
                }

            }

        }


        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton5_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                //long id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
                FormsRSW2014_1_Razd_6_4 rsw_temp = RSW_6_4_List.Skip(rowindex).Take(1).First();


                RSW2014_6_4_Edit child = new RSW2014_6_4_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.autoCalc = AutoCalcSwitch.IsOn;
                child.staffID = staff.ID;
                short y = Year.Text != "" ? short.Parse(Year.Text) : (short)0;
                byte q = period;
                if (TypeInfo.SelectedIndex > 0 && (KorrYear.Text != "" && KorrQuarter.Text != ""))
                {
                    y = short.Parse(KorrYear.Text);
                    q = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
                }
                child.periodQ = q;
                child.periodY = y;
                child.platCatList = new List<long>();
                foreach (var item in RSW_6_4_List.Where(x => x.PlatCategoryID != rsw_temp.PlatCategoryID))
                {
                    child.platCatList.Add(item.PlatCategoryID);
                }
                child.formData = rsw_temp;
                child.ShowDialog();

                if (child.formData != null)
                {
                    RSW_6_4_List.RemoveAt(rowindex);
                    RSW_6_4_List.Insert(rowindex, child.formData);

                    gridUpdate_6_4();
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
        private void radButton6_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0 && radGridView1.CurrentRow.Cells[0].Value != null)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                RSW_6_4_List.RemoveAt(rowindex);

                gridUpdate_6_4();

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            radButton5_Click(null, null);
        }


        /// <summary>
        /// Кнопка для обновление суммы страховых взносов на ОПС
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton19_Click(object sender, EventArgs e)
        {
            short y = short.Parse(Year.Text);
            if (TypeInfo.SelectedItem.Value.ToString() == "2" && KorrYear.SelectedItem != null)
            {
                short.TryParse(KorrYear.SelectedItem.Text, out y);
            }

            MethodsNonStatic methods = new MethodsNonStatic();
            SumFeePFR.Value = methods.UpdateSumFeePFR(true, RSW_6_4_List, y);
        }

        #endregion


        #region Раздел 6.6

        /// <summary>
        /// обновление таблицы раздела 6.6
        /// </summary>
        private void gridUpdate_6_6()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            radGridView3.Rows.Clear(); // If dgv is bound to datatable


            List<RaschetPeriodContainer> RaschPer = new List<RaschetPeriodContainer> { };
            foreach (var item in Options.RaschetPeriodInternal2010_2013)
            {
                RaschPer.Add(item);
            }
            foreach (var item in Options.RaschetPeriodInternal)
            {
                RaschPer.Add(item);
            }


            if (RSW_6_6_List.Count() != 0)
            {
                foreach (var item in RSW_6_6_List)
                {
                    RaschetPeriodContainer rp = RaschPer.FirstOrDefault(x => x.Year == item.AccountPeriodYear && x.Kvartal == item.AccountPeriodQuarter);
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView3.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Year"].Value = item.AccountPeriodYear.Value;
                    rowInfo.Cells["Period"].Value = rp != null ? (rp.Kvartal + " - " + rp.Name) : "Период не определен";
                    rowInfo.Cells["OPS"].Value = item.SumFeePFR_D.HasValue ? item.SumFeePFR_D.Value : 0;
                    rowInfo.Cells["Strah"].Value = item.SumFeePFR_StrahD.HasValue ? item.SumFeePFR_StrahD.Value : 0;
                    rowInfo.Cells["Nakop"].Value = item.SumFeePFR_NakopD.HasValue ? item.SumFeePFR_NakopD.Value : 0;
                    radGridView3.Rows.Add(rowInfo);
                }
            }

            this.radGridView3.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            radGridView3.Refresh();
            updateTextBoxes_Razd_6_6();
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton12_Click(object sender, EventArgs e)
        {
            if (staff != null)
            {
                short y = 0;
                byte q = 20;
                if (short.TryParse(Year.SelectedItem.Text, out y))
                {
                    if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                    {
                    }
                }
                RSW2014_6_6_Edit child = new RSW2014_6_6_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.y = y;
                child.q = q;
                child.staffId = staff != null ? staff.ID : 0;
                child.ShowDialog();
                if (child.formData != null)
                {
                    RSW_6_6_List.Add(child.formData);
                    gridUpdate_6_6();
                }
            }
            else
            {
                RadMessageBox.Show("Необходимо сначала выбрать сотрудника!");
            }
        }

        /// <summary>
        /// Функция обновления значений в полях быстрого просмотра под таблицей Раздела 6.6
        /// </summary>
        /// <param name="rsw"></param>
        private void updateTextBoxes_Razd_6_6()
        {
            if (RSW_6_6_List.Count != 0)
            {
                decimal[] sum = new decimal[3] { 0, 0, 0 };

                foreach (var item in RSW_6_6_List)
                {
                    sum[0] = sum[0] + item.SumFeePFR_D.Value;
                    sum[1] = sum[1] + item.SumFeePFR_StrahD.Value;
                    sum[2] = sum[2] + item.SumFeePFR_NakopD.Value;
                }

                r_6_6_OPS.Text = Utils.decToStr(sum[0]);
                r_6_6_Strah.Text = Utils.decToStr(sum[1]);
                r_6_6_Nakop.Text = Utils.decToStr(sum[2]);

            }
            else
            {
                r_6_6_OPS.Text = "0.00";
                r_6_6_Strah.Text = "0.00";
                r_6_6_Nakop.Text = "0.00";
            }


        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton11_Click(object sender, EventArgs e)
        {
            if (radGridView3.RowCount != 0)
            {
                short y = 0;
                byte q = 20;
                if (short.TryParse(Year.SelectedItem.Text, out y))
                {
                    if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                    {
                    }
                }

                int rowindex = radGridView3.CurrentRow.Index;

                RSW2014_6_6_Edit child = new RSW2014_6_6_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.y = y;
                child.q = q;
                child.formData = RSW_6_6_List.Skip(rowindex).Take(1).First();
                child.staffId = staff != null ? staff.ID : 0;
                child.ShowDialog();

                if (child.formData != null)
                {
                    RSW_6_6_List.RemoveAt(rowindex);
                    RSW_6_6_List.Insert(rowindex, child.formData);

                    gridUpdate_6_6();
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
        private void radButton10_Click(object sender, EventArgs e)
        {
            if (radGridView3.RowCount != 0)
            {
                int rowindex = radGridView3.CurrentRow.Index;
                RSW_6_6_List.RemoveAt(rowindex);

                gridUpdate_6_6();


            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }


        private void radGridView3_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            radButton11_Click(null, null);
        }

        #endregion


        #region Раздел 6.7

        /// <summary>
        /// обновление таблицы раздела 6.7
        /// </summary>
        private void gridUpdate_6_7()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            radGridView2.Rows.Clear(); // If dgv is bound to datatable
            //            radGridView1.DataSource = null;

            /*           int width = 0;
                       for (int i = 0; i < radGridView2.ColumnCount - 1; i++)
                       {
                           if (radGridView2.Columns[i].IsVisible)
                               width = width + radGridView2.Columns[i].Width;
                       }


                       radGridView2.Columns["s_0_3"].Width = radGridView2.Width - width;
           */
            if (RSW_6_7_List.Count() != 0)
            {
                foreach (var item in RSW_6_7_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView2.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["CodeOcenki"].Value = item.SpecOcenkaUslTrudaID.HasValue ? item.SpecOcenkaUslTruda.Code : "";
                    rowInfo.Cells["s_0_0"].Value = item.s_0_0;
                    rowInfo.Cells["s_0_1"].Value = item.s_0_1;
                    radGridView2.Rows.Add(rowInfo);
                }
            }

            this.radGridView2.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            radGridView2.Refresh();
        }


        /// <summary>
        /// Добавление записи о доп. тарифах по спец оценке условий труда
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton9_Click(object sender, EventArgs e)
        {
            if (staff != null)
            {
                RSW2014_6_7_Edit child = new RSW2014_6_7_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.staffID = staff.ID;
                short y = Year.Text != "" ? short.Parse(Year.Text) : (short)0;
                byte q = period;
                if (TypeInfo.SelectedIndex > 0 && (KorrYear.Text != "" && KorrQuarter.Text != ""))
                {
                    y = short.Parse(KorrYear.Text);
                    q = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
                }
                child.periodQ = q;
                child.periodY = y;
                child.autoCalc = AutoCalcSwitch.IsOn;
                child.ShowDialog();
                if (child.formData != null)
                {
                    RSW_6_7_List.Add(child.formData);
                    gridUpdate_6_7();
                }
            }
            else
            {
                RadMessageBox.Show("Необходимо сначала выбрать сотрудника!");
            }
        }

        /// <summary>
        /// Функция обновления значений в полях быстрого просмотра под таблицей Раздела 6.7
        /// </summary>
        /// <param name="rsw"></param>
        private void updateTextBoxes_Razd_6_7(FormsRSW2014_1_Razd_6_7 rsw)
        {
            var fields = typeof(FormsRSW2014_1_Razd_6_7).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                string ControlItemName = "r_6_7_" + itemName;
                if (ControlItemName.StartsWith("r_6_7_s_1") || ControlItemName.StartsWith("r_6_7_s_2") || ControlItemName.StartsWith("r_6_7_s_3"))
                {
                    RadLabel label = (RadLabel)this.Controls.Find(ControlItemName, true)[0];

                    string data = "0.00";
                    if (rsw != null)
                    {
                        var properties = rsw.GetType().GetProperty(itemName);
                        object value = properties.GetValue(rsw, null);
                        if (value != null)
                            data = Utils.decToStr(decimal.Parse(value.ToString()));

                    }

                    label.Text = data;
                }

            }

        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton8_Click(object sender, EventArgs e)
        {
            if (radGridView2.RowCount != 0)
            {
                int rowindex = radGridView2.CurrentRow.Index;
                FormsRSW2014_1_Razd_6_7 rsw_temp = RSW_6_7_List.Skip(rowindex).Take(1).First();

                RSW2014_6_7_Edit child = new RSW2014_6_7_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.autoCalc = AutoCalcSwitch.IsOn;
                child.staffID = staff.ID;
                short y = Year.Text != "" ? short.Parse(Year.Text) : (short)0;
                byte q = period;
                if (TypeInfo.SelectedIndex > 0 && (KorrYear.Text != "" && KorrQuarter.Text != ""))
                {
                    y = short.Parse(KorrYear.Text);
                    q = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
                }
                child.periodQ = q;
                child.periodY = y;
                child.formData = rsw_temp;
                child.ShowDialog();

                if (child.formData != null)
                {
                    RSW_6_7_List.RemoveAt(rowindex);
                    RSW_6_7_List.Insert(rowindex, child.formData);

                    gridUpdate_6_7();
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
        private void radButton7_Click(object sender, EventArgs e)
        {
            if (radGridView2.RowCount != 0)
            {
                int rowindex = radGridView2.CurrentRow.Index;
                RSW_6_7_List.RemoveAt(rowindex);

                gridUpdate_6_7();

                if (RSW_6_7_List.Count == 0)
                {
                    FormsRSW2014_1_Razd_6_7 rsw = new FormsRSW2014_1_Razd_6_7();
                    updateTextBoxes_Razd_6_7(rsw);
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void radGridView2_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            radButton8_Click(null, null);
        }

        /// <summary>
        /// Обновление значений в полях быстрого просмотра под таблицей при изменении текущей строки Раздела 6.6
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridView2_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            if (radGridView2.CurrentRow == null)
            {
                if (radGridView2.RowCount > 0)
                {
                    radGridView2.GridNavigator.SelectFirstRow();
                    radGridView2.CurrentRow = radGridView2.Rows[0];
                }
                else
                    return;
            }

            if (radGridView2.CurrentRow != null)
            {
                int rowindex = radGridView2.CurrentRow.Index;
                //long id = long.Parse(radGridView2.Rows[rowindex].Cells[0].Value.ToString());

                FormsRSW2014_1_Razd_6_7 rsw = RSW_6_7_List.Skip(rowindex).Take(1).First();
                updateTextBoxes_Razd_6_7(rsw);
            }
        }

        #endregion


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
                    stajOsnGrid.Rows.Add(rowInfo);
                }
            }

            this.stajOsnGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            stajOsnGrid.Refresh();

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
            child.ParentFormName = "RSW2014_6";
            child.dateControl = dateControlCheckBox.Checked;
            child.formData = new StajOsn();
            child.rowindex = -1;
            var y = short.Parse(Year.Text);
            var q = period;
            if (TypeInfo.SelectedIndex > 0 && (KorrYear.Text != "" && KorrQuarter.Text != ""))
            {
                y = short.Parse(KorrYear.Text);
                q = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
            }
            child.period = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == q && x.Year == y);
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
                child.ParentFormName = "RSW2014_6";
                child.dateControl = dateControlCheckBox.Checked;
                child.action = "edit";
                var y = short.Parse(Year.Text);
                var q = period;
                if (TypeInfo.SelectedIndex > 0 && (KorrYear.Text != "" && KorrQuarter.Text != ""))
                {
                    y = short.Parse(KorrYear.Text);
                    q = byte.Parse(KorrQuarter.SelectedItem.Value.ToString());
                }
                child.period = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == q && x.Year == y);
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

                stajLgotGrid.Refresh();
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

        private void radGridView1_SizeChanged(object sender, EventArgs e)
        {
            tableLayoutPanel1.Width = radGridView1.Width;
            for (int i = 1; i < radGridView1.ColumnCount; i++)
            {
                if (i != (radGridView1.ColumnCount - 1))
                    tableLayoutPanel1.ColumnStyles[i - 1].Width = radGridView1.Columns[i].Width;
                else
                    tableLayoutPanel1.ColumnStyles[i - 1].Width = radGridView1.Columns[i].Width - 10;
            }
        }

        private void radGridView3_SizeChanged(object sender, EventArgs e)
        {
            tableLayoutPanel3.Width = radGridView3.Width;
            tableLayoutPanel3.ColumnStyles[0].Width = radGridView3.Columns[1].Width + radGridView3.Columns[2].Width;
            for (int i = 3; i < radGridView3.ColumnCount; i++)
            {
                if (i != (radGridView3.ColumnCount - 1))
                    tableLayoutPanel3.ColumnStyles[i - 2].Width = radGridView3.Columns[i].Width;
                else
                    tableLayoutPanel3.ColumnStyles[i - 2].Width = radGridView3.Columns[i].Width - 3;
            }
        }

        private void radGridView2_SizeChanged(object sender, EventArgs e)
        {
            tableLayoutPanel2.Width = radGridView2.Width;
            for (int i = 1; i < radGridView2.ColumnCount; i++)
            {
                if (i != (radGridView2.ColumnCount - 1))
                    tableLayoutPanel2.ColumnStyles[i - 1].Width = radGridView2.Columns[i].Width;
                else
                    tableLayoutPanel2.ColumnStyles[i - 1].Width = radGridView2.Columns[i].Width - 10;
            }
        }

        private void radPageView1_SelectedPageChanged(object sender, EventArgs e)
        {
            radGridView1_SizeChanged(null, null);
            radGridView2_SizeChanged(null, null);
            radGridView3_SizeChanged(null, null);
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

                RSW_6 = null;
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
                        RSW_6 = null;
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








































    }
}
