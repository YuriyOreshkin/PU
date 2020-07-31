using PU.Classes;
using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;
using System.Reflection;

namespace PU.FormsSZV_TD
{
    public partial class SZV_TD_EditStaff : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public Staff staff { get; set; }
        public long SZVTD_ID { get; set; }
        bool allowClose = false;
        List<FormsSZV_TD_2020_Staff_Events> FormsSZV_TD_2020_Staff_Events_List = new List<FormsSZV_TD_2020_Staff_Events>();
        public FormsSZV_TD_2020_Staff SZVTD_Staff { get; set; }

        public SZV_TD_EditStaff()
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

            db.ChangeTracker.DetectChanges();

            if (db.Staff.Any(x => x.ID == id && x.InsurerID == Options.InsID))
            {
                if (db.FormsSZV_TD_2020_Staff.Any(x => x.StaffID == id && x.FormsSZV_TD_2020_ID == SZVTD_ID))
                {
                    RadMessageBox.Show(this, "Внимание!\r\nДля данного сотрудника уже существует запись в текущей Форме СЗВ-ТД!\r\nВыберите другого сотрудника!", "Внимание!");
                }
                else
                {
                    staff = db.Staff.FirstOrDefault(x => x.ID == id && x.InsurerID == Options.InsID);
                    updateStaffInfo();
                }
            }
        }


        private void SZV_TD_EditStaff_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            ZayavOProdoljDate.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime;
            ZayavOProdoljDate.Value = ZayavOProdoljDate.NullDate;

            ZayavOPredostDate.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime;
            ZayavOPredostDate.Value = ZayavOPredostDate.NullDate;


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

            try
            {


                switch (action)
                {
                    case "add":
                        SZVTD_Staff = new FormsSZV_TD_2020_Staff();
                        SZVTD_Staff.FormsSZV_TD_2020_ID = SZVTD_ID;




                        break;
                    case "edit":

                        //Информация о сотруднике
                        staff = SZVTD_Staff.Staff;

                        SNILS.Enabled = false;
                        selectStaffBtn.Enabled = false;

                        if (SZVTD_Staff.ZayavOProdoljDate.HasValue)
                        {
                            ZayavOProdoljDate.Value = SZVTD_Staff.ZayavOProdoljDate.Value;
                        }

                        if (SZVTD_Staff.ZayavOProdoljState.HasValue)
                        {
                            ZayavOProdoljState.Items.Single(x => x.Tag.ToString() == SZVTD_Staff.ZayavOProdoljState.Value.ToString()).Selected = true;
                        }

                        
                        if (SZVTD_Staff.ZayavOPredostDate.HasValue)
                        {
                            ZayavOPredostDate.Value = SZVTD_Staff.ZayavOPredostDate.Value;
                        }

                        if (SZVTD_Staff.ZayavOPredostState.HasValue)
                        {
                            ZayavOPredostState.Items.Single(x => x.Tag.ToString() == SZVTD_Staff.ZayavOPredostState.Value.ToString()).Selected = true;
                        }

  
                        FormsSZV_TD_2020_Staff_Events_List = SZVTD_Staff.FormsSZV_TD_2020_Staff_Events.ToList();
                        SZVTD_STAFF_EVENTS_Grid_update();

                        break;
                }

                updateStaffInfo();




                long id_old = 0;
                switch (action)
                {
                    case "add":

                        break;
                    case "edit":
                        id_old = SZVTD_Staff.ID;
                        SZVTD_Staff = new FormsSZV_TD_2020_Staff { ID = id_old };
                        break;
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        private void SZVTD_STAFF_EVENTS_Grid_update()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            SZVTD_STAFF_EVENTS_Grid.Rows.Clear();

            if (FormsSZV_TD_2020_Staff_Events_List.Count() != 0)
            {
                int n = 1;
                foreach (var item in FormsSZV_TD_2020_Staff_Events_List)
                {
                    bool StatPunkt = !String.IsNullOrEmpty(item.Statya) || !String.IsNullOrEmpty(item.Punkt);

                    string uvol = "";

                    if (StatPunkt)
                    {
                        uvol = (!String.IsNullOrEmpty(item.Statya) ? "\r\nСт. " + item.Statya.Trim() : "") + (!String.IsNullOrEmpty(item.Punkt) ? "\r\nП. " + item.Punkt.Trim() : "");
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(item.OsnUvolName))
                        {
                            uvol += item.OsnUvolName.Trim() + "\r\n";
                        }
                        if (!String.IsNullOrEmpty(item.OsnUvolStartya))
                        {
                            uvol += "Ст. " + item.OsnUvolStartya.Trim() + "\r\n";
                        }
                        if (!String.IsNullOrEmpty(item.OsnUvolChyast))
                        {
                            uvol += "Ч. " + item.OsnUvolChyast.Trim() + "\r\n";
                        }
                        if (!String.IsNullOrEmpty(item.OsnUvolPunkt))
                        {
                            uvol += "П. " + item.OsnUvolPunkt.Trim() + "\r\n";
                        }
                        if (!String.IsNullOrEmpty(item.OsnUvolPodPunkt))
                        {
                            uvol += "П.п. " + item.OsnUvolPodPunkt.Trim() + "\r\n";
                        }

                        if (!String.IsNullOrEmpty(uvol))
                        {
                            uvol = "\r\n" + uvol;
                        }
                    }


                    string osn1 = item.OsnName1 + (item.OsnDate1.HasValue ? ("  От " +  item.OsnDate1.Value.ToShortDateString()) : "") + (!String.IsNullOrEmpty(item.OsnNum1) ? ("  № " + item.OsnNum1.Trim()) : "") + (!String.IsNullOrEmpty(item.OsnSer1) ? (" Серия: " + item.OsnSer1.Trim()) : "");
                    string osn2 = !String.IsNullOrEmpty(item.OsnName2) ? ("\r\n\r\n" + item.OsnName2 + (item.OsnDate2.HasValue ? ("  От " + item.OsnDate2.Value.ToShortDateString()) : "") + (!String.IsNullOrEmpty(item.OsnNum2) ? ("  № " + item.OsnNum2.Trim()) : "") + (!String.IsNullOrEmpty(item.OsnSer2) ? (" Серия: " + item.OsnSer2.Trim()) : "")) : "";

                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.SZVTD_STAFF_EVENTS_Grid.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Number"].Value = n;
                    rowInfo.Cells["DateOfEvent"].Value = item.DateOfEvent;
                    rowInfo.Cells["Svedenia"].Value = item.FormsSZV_TD_2020_TypesOfEvents.Name;
                    rowInfo.Cells["TrudFunction"].Value = String.Format("{0}{1}{2}{3}\r\nСовместитель: {4}{5}{6}", item.Dolgn, !String.IsNullOrEmpty(item.VydPoruchRaboty) ? ("\r\n" + item.VydPoruchRaboty) : "", !String.IsNullOrEmpty(item.Department) ? ("\r\n" + item.Department) : "", !String.IsNullOrEmpty(item.Svedenia) ? ("\r\n" + item.Svedenia) : "", item.Sovmestitel == 0 ? "НЕТ" : "ДА", item.DateFrom.HasValue ? ("\r\nС - " + item.DateFrom.Value.ToShortDateString()) : "", item.DateTo.HasValue ? ("\r\nПО - " + item.DateTo.Value.ToShortDateString()) : "");
                    rowInfo.Cells["KodVypFunction"].Value = item.KodVypFunc;
                    rowInfo.Cells["PrichUvoln"].Value = String.Format("{0}{1}", !String.IsNullOrEmpty(item.Prichina) ? ("Причина: " + item.Prichina) : "", uvol);
                    rowInfo.Cells["OsnName"].Value = osn1 + osn2;
                    rowInfo.Cells["PriznakOtmeni"].Value = (item.Annuled.HasValue && item.Annuled.Value) ? "ОТМЕНА" + (item.AnnuleDate.HasValue ? "\r\n" + item.AnnuleDate.Value.ToShortDateString() : "") : "";


                    SZVTD_STAFF_EVENTS_Grid.Rows.Add(rowInfo);

                    n++;
                }
            }

            this.SZVTD_STAFF_EVENTS_Grid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            SZVTD_STAFF_EVENTS_Grid.Refresh();
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
                    DateBirth_MaskedEditBox.Text = staff.DateBirth.HasValue ? staff.DateBirth.Value.ToShortDateString() : DateBirth_MaskedEditBox.NullText;

                    SNILS.Enabled = false;
                }
                catch { }
            }
        }

        private void SNILS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SNILS_Leave(sender, null);

            }
        }

        private void SNILS_Leave(object sender, EventArgs e)
        {
            var ssn = SNILS.Value.ToString().Split(' ');

            if (!ssn[0].Contains("_"))
            {
                string contrNum = Utils.GetControlSumSSN(ssn[0]);

                SNILS.Value = ssn[0] + " " + contrNum;

                string snils = ssn[0].Replace("-", "");

                if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils))
                {
                    staff = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils);
                    updateStaffInfo();
                }

            }
            else
            {
                if (SNILS.Value.ToString() != "___-___-___ __")
                {
                    SNILS.Text = "___-___-___ __";
                }

            }
        }



        private void addEventBtn_Click(object sender, EventArgs e)
        {
            if (staff == null)
            {
                RadMessageBox.Show("Необходимо выбрать Сотрудника!");
            }
            else
            {

                SZV_TD_EditStaff_Event child = new SZV_TD_EditStaff_Event();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.staff = staff;
                child.action = "add";

                if (FormsSZV_TD_2020_Staff_Events_List.Count > 0)
                {
                    child.firstRecordIsAnnuled = FormsSZV_TD_2020_Staff_Events_List.First().Annuled;
                }

                child.ShowDialog();
                if (child.formData != null)
                {
                    FormsSZV_TD_2020_Staff_Events_List.Add(child.formData);
                    SZVTD_STAFF_EVENTS_Grid_update();
                }
            }
        }

        private void editEventBtn_Click(object sender, EventArgs e)
        {
            if (SZVTD_STAFF_EVENTS_Grid.RowCount != 0)
            {
                int rowindex = SZVTD_STAFF_EVENTS_Grid.CurrentRow.Index;
                FormsSZV_TD_2020_Staff_Events szvtd_event_temp = FormsSZV_TD_2020_Staff_Events_List.Skip(rowindex).Take(1).First();


                SZV_TD_EditStaff_Event child = new SZV_TD_EditStaff_Event();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.formData = szvtd_event_temp;
                child.ShowDialog();

                if (child.formData != null)
                {
                    FormsSZV_TD_2020_Staff_Events_List.RemoveAt(rowindex);
                    FormsSZV_TD_2020_Staff_Events_List.Insert(rowindex, child.formData);

                    SZVTD_STAFF_EVENTS_Grid_update();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void delEventBtn_Click(object sender, EventArgs e)
        {
            if (SZVTD_STAFF_EVENTS_Grid.RowCount != 0 && SZVTD_STAFF_EVENTS_Grid.CurrentRow.Cells[0].Value != null)
            {
                int rowindex = SZVTD_STAFF_EVENTS_Grid.CurrentRow.Index;
                if (rowindex >= 0)
                {
                    FormsSZV_TD_2020_Staff_Events_List.RemoveAt(rowindex);

                    SZVTD_STAFF_EVENTS_Grid_update();
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            List<string> errList = validation();

            if (errList.Count > 0)
            {
                foreach (var item in errList)
                {
                    RadMessageBox.Show(this, item, "Внимание!");
                }

            }
            else
            {
                bool flag_ok = false;

                SZVTD_Staff.Staff = staff;

                if (ZayavOPredostDate.Value != ZayavOPredostDate.NullDate)
                    SZVTD_Staff.ZayavOPredostDate = ZayavOPredostDate.Value;

                SZVTD_Staff.ZayavOPredostState = ZayavOPredostState.SelectedItem != null ? byte.Parse(ZayavOPredostState.SelectedItem.Tag.ToString()) : (byte)0;


                if (ZayavOProdoljDate.Value != ZayavOProdoljDate.NullDate)
                    SZVTD_Staff.ZayavOProdoljDate = ZayavOProdoljDate.Value;


                SZVTD_Staff.ZayavOProdoljState = ZayavOProdoljState.SelectedItem != null ? byte.Parse(ZayavOProdoljState.SelectedItem.Tag.ToString()) : (byte)0;


                switch (action)
                {
                    case "add":
                        db.FormsSZV_TD_2020_Staff.Add(SZVTD_Staff);
                        db.SaveChanges();

                                                try
                        {
                        foreach (var item in FormsSZV_TD_2020_Staff_Events_List)
                        {
                            item.FormsSZV_TD_2020_Staff_ID = SZVTD_Staff.ID;
                            FormsSZV_TD_2020_Staff_Events r = new FormsSZV_TD_2020_Staff_Events();

                            var fields = typeof(FormsSZV_TD_2020_Staff_Events).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
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

                            db.FormsSZV_TD_2020_Staff_Events.Add(r);
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
                    case "edit":
                        db = new pu6Entities();
                        FormsSZV_TD_2020_Staff r_edit = db.FormsSZV_TD_2020_Staff.FirstOrDefault(x => x.ID == SZVTD_Staff.ID);

                        try {

                            r_edit.ZayavOPredostDate = SZVTD_Staff.ZayavOPredostDate;
                            r_edit.ZayavOPredostState = SZVTD_Staff.ZayavOPredostState;
                            r_edit.ZayavOProdoljDate = SZVTD_Staff.ZayavOProdoljDate;
                            r_edit.ZayavOProdoljState = SZVTD_Staff.ZayavOProdoljState;

                            // сохраняем модифицированную запись обратно в бд
                            db.Entry(r_edit).State = EntityState.Modified;
                            db.SaveChanges();
                            flag_ok = true;

                            if (flag_ok)
                            {
                                flag_ok = false;


                                #region обрабатываем записи о Мероприятиях
                                try
                                {
                                    var FormsSZV_TD_2020_Staff_Events_List_from_db = db.FormsSZV_TD_2020_Staff_Events.Where(x => x.FormsSZV_TD_2020_Staff_ID == r_edit.ID);

                                    // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                    var t = FormsSZV_TD_2020_Staff_Events_List.Select(x => x.ID);
                                    var list_for_del = FormsSZV_TD_2020_Staff_Events_List_from_db.Where(x => !t.Contains(x.ID));

                                    foreach (var item in list_for_del)
                                    {
                                        db.FormsSZV_TD_2020_Staff_Events.Remove(item);
                                    }

                                    if (list_for_del.Count() != 0)
                                    {
                                        //db.SaveChanges();
                                        FormsSZV_TD_2020_Staff_Events_List_from_db = db.FormsSZV_TD_2020_Staff_Events.Where(x => x.FormsSZV_TD_2020_Staff_ID == r_edit.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                                    }


                                    // проверка текущих записей на факт их редактирования (отличия от имеющихся в БД) (если запись изменена, то вносим изменения) или добавления новых (необходимо добавить в БД)

                                    var fields = typeof(FormsSZV_TD_2020_Staff_Events).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    var names = Array.ConvertAll(fields, field => field.Name);


                                    foreach (var item in FormsSZV_TD_2020_Staff_Events_List)
                                    {
                                        bool flag_add_new = true;
                                        //если такая запись есть, надо проверять на отличия
                                        if (FormsSZV_TD_2020_Staff_Events_List_from_db.Any(x => x.ID == item.ID))
                                        {
                                            flag_add_new = false;
                                            bool flag_edited = false;
                                            FormsSZV_TD_2020_Staff_Events szvtd_events_temp = FormsSZV_TD_2020_Staff_Events_List_from_db.Single(x => x.ID == item.ID);


                                            foreach (var item_ in names)
                                            {
                                                string itemName = item_.TrimStart('_');
                                                if (item_.IndexOf("FormsSZV_TD_2020_Staff_ID") < 0)
                                                {
                                                    string data_old = "";
                                                    string data_new = "";

                                                    var properties_old = szvtd_events_temp.GetType().GetProperty(itemName);
                                                    object value_old = properties_old.GetValue(szvtd_events_temp, null);
                                                    data_old = value_old != null ? value_old.ToString() : "";

                                                    var properties_new = item.GetType().GetProperty(itemName);
                                                    object value_new = properties_new.GetValue(item, null);
                                                    data_new = value_new != null ? value_new.ToString() : "";

                                                    if (data_old != data_new)
                                                    {
                                                        flag_edited = true;

                                                        szvtd_events_temp.GetType().GetProperty(itemName).SetValue(szvtd_events_temp, value_new, null);
                                                    }

                                                }
                                            }


                                            if (flag_edited) // если записи отличаются
                                            {

                                                db.Entry(szvtd_events_temp).State = EntityState.Modified;

                                            }

                                        }
                                        if (flag_add_new) // такой записи в базе нет, значит просто добавляем ее
                                        {

                                            // добавление записи в БД
                                            item.FormsSZV_TD_2020_Staff_ID = SZVTD_Staff.ID;
                                            FormsSZV_TD_2020_Staff_Events r = new FormsSZV_TD_2020_Staff_Events();

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

                                            db.FormsSZV_TD_2020_Staff_Events.Add(r);
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


                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранение данных произошла ошибка. Код ошибки: " + ex.Message);
                        }

                        break;

                }


            }

        }


        private List<string> validation()
        {
            List<string> errMessBox = new List<string>();

            if (staff == null)
            {
                errMessBox.Add("Необходимо выбрать Сотрудника!");
            }
            
            
            return errMessBox;
        }



        private void SZV_TD_EditStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (allowClose)
            {
                SZVTD_Staff = null;
                db.Dispose();
            }
            else
            {
                DialogResult dialogResult = RadMessageBox.Show("Вы хотите сохранить изменения перед закрытием формы?", "Сохранение записи!", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button3);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        saveBtn_Click(null, null);
                        break;
                    case DialogResult.No:
                        SZVTD_Staff = null;
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

        private void SZVTD_STAFF_EVENTS_Grid_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editEventBtn_Click(null, null);
        }

        private void ZayavOProdoljDate_Enter(object sender, EventArgs e)
        {
            ZayavOProdoljDate.Select();
        }

        private void ZayavOPredostDate_Enter(object sender, EventArgs e)
        {
            ZayavOPredostDate.Select();
        }
    }
}
