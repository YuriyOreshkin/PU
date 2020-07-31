using PU.Classes;
using PU.Models;
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
using Telerik.WinControls.Data;
using System.Globalization;
using Telerik.WinControls.UI;

namespace PU.FormsSZV_TD
{
    delegate void DelEvent();

    public partial class SZV_TD_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        SZV_TD_Print ReportViewerSZV_TD;
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками

        public SZV_TD_List()
        {
            InitializeComponent();
            SelfRef = this;
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    addBtn.Enabled = false;
                    editBtn.Enabled = false;
                    delBtn.Enabled = false;
                    exportToXMLBtn.Enabled = false;
                    radMenuItem1.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.BeginInvoke(new MethodInvoker(this.Close));
                    return;
            }

        }

        public static SZV_TD_List SelfRef
        {
            get;
            set;
        }

        private void SZV_TD_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            checkAccessLevel();

            HeaderChange();


            this.szvTDGridView.CurrentRowChanged += (s, с) => szvTDGridView_CurrentRowChanged();
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
        /// Смена заголовка при выборе страхователя
        /// </summary>
        public void HeaderChange()
        {
            insNamePanel.Text = Methods.HeaderChange();

            szvTDGrid_upd();
        }

        private void insurerBtn_Click(object sender, EventArgs e)
        {
            InsurerFrm child = new InsurerFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.InsID = Options.InsID;
            child.action = "selection";
            child.ShowDialog();
            if (Options.InsID != child.InsID)
            {
                Options.InsID = child.InsID;
                methodsNonStatic.writeSetting();
            }

            Methods.HeaderChangeAllTabs();
        }


        private void szvTDGridView_CurrentRowChanged()
        {
            if (d != null)
                d();
        }

        DelEvent d;

        public class SzvTDItem
        {
            public long ID { get; set; }
            public short Year { get; set; }
            public string Month { get; set; }
            public string TypeInfo { get; set; }
            public int Count { get; set; }
            public DateTime DateFilling { get; set; }
        }


        public void szvTDGrid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (szvTDGridView.RowCount > 0 && szvTDGridView.CurrentRow != null)
            {
                rowindex = szvTDGridView.CurrentRow.Index;
                currId = szvTDGridView.CurrentRow.Cells[0].Value.ToString();
            }

            d = null;

            SortDescriptor descriptor = new SortDescriptor();

            if (szvTDGridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = szvTDGridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "Year";
                descriptor.Direction = ListSortDirection.Ascending;
            }

            var szvtd = db.FormsSZV_TD_2020.Where(x => x.InsurerID == Options.InsID).OrderBy(x => x.Year);

            List<SzvTDItem> szvtdList = new List<SzvTDItem>();

            if (szvtd.Count() != 0)
            {
                foreach (var item in szvtd)
                {

                    szvtdList.Add(new SzvTDItem()
                    {
                        ID = item.ID,
                        Year = item.Year,
                        DateFilling = item.DateFilling,
                        Month = item.Month.ToString().PadLeft(2, '0') + " - " + DateTimeFormatInfo.CurrentInfo.GetMonthName(item.Month),
                        Count = db.FormsSZV_TD_2020_Staff.Count(x => x.FormsSZV_TD_2020_ID == item.ID) //item.FormsSZV_M_2016_Staff.Count
                    });
                }
            }

            this.szvTDGridView.TableElement.BeginUpdate();

            szvTDGridView.DataSource = szvtdList.OrderBy(x => x.Year);
            if (descriptor != null)
            {
                this.szvTDGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (szvtdList.Count > 0)
            {
                szvTDGridView.Columns["ID"].IsVisible = false;
                szvTDGridView.Columns["TypeInfo"].IsVisible = false;
                szvTDGridView.Columns["Year"].Width = 100;
                szvTDGridView.Columns["Year"].HeaderText = "Год";
                szvTDGridView.Columns["Month"].Width = 200;
                szvTDGridView.Columns["Month"].HeaderText = "Месяц";
                szvTDGridView.Columns["DateFilling"].Width = 150;
                szvTDGridView.Columns["DateFilling"].HeaderText = "Дата";
                szvTDGridView.Columns["DateFilling"].FormatString = "{0:dd.MM.yyyy}";
                szvTDGridView.Columns["Count"].Width = 150;
                szvTDGridView.Columns["Count"].HeaderText = "Количество сотрудников";


                for (var i = 0; i < szvTDGridView.Columns.Count; i++)
                {
                    szvTDGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    szvTDGridView.Columns[i].ReadOnly = true;
                }

                this.szvTDGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }

            foreach (var item in szvTDGridView.Rows)
            {
                item.MinHeight = 22;
            }

            this.szvTDGridView.TableElement.EndUpdate();

            d += staffGrid_upd;

            if (szvTDGridView.ChildRows.Count > 0)
            {
                if (szvTDGridView.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    szvTDGridView.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= szvTDGridView.RowCount)
                        rowindex = szvTDGridView.RowCount - 1;
                    szvTDGridView.Rows[rowindex].IsCurrent = true;
                }

                staffGrid_upd();
            }
            else
            {
                staffGridView.DataSource = null;
            }

        }

        public void staffGrid_upd()
        {
            long szvtdID = 0;
            if (szvTDGridView.RowCount > 0)
            {
                szvtdID = Convert.ToInt64(szvTDGridView.CurrentRow.Cells[0].Value);
            }

            int rowindex = 0;
            string currId = "";
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                rowindex = staffGridView.CurrentRow.Index;
                currId = staffGridView.CurrentRow.Cells[1].Value.ToString();
            }


            SortDescriptor descriptor = new SortDescriptor();

            if (staffGridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = staffGridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "FIO";
                descriptor.Direction = ListSortDirection.Ascending;
            }


            var szvtdstaffList = db.FormsSZV_TD_2020_Staff.Where(x => x.FormsSZV_TD_2020_ID == szvtdID).ToList();
          //  List<long> t = szvtdstaffList.Select(x => x.StaffID).ToList();
           // List<Staff> staffL = db.Staff.Where(x => t.Contains(x.ID)).ToList();

            this.staffGridView.TableElement.BeginUpdate();

            List<StaffObject> staffList = new List<StaffObject> { };

            //if (szvmstaffList.Count() != 0)
            //{
            foreach (var item in szvtdstaffList)
            {
                //         var staff = staffL.First(x => x.ID == item.StaffID);
                string dateb = "";
                if (item.Staff.DateBirth != null)
                {
                    dateb = item.Staff.DateBirth.HasValue ? item.Staff.DateBirth.Value.ToShortDateString() : "";
                }

                staffList.Add(new StaffObject()
                {
                    ID = item.ID,
                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                    SNILS = Utils.ParseSNILS(item.Staff.InsuranceNumber, item.Staff.ControlNumber),
                    INN = !String.IsNullOrEmpty(item.Staff.INN) ? item.Staff.INN.PadLeft(12, '0') : " ",
                    TabelNumber = item.Staff.TabelNumber,
                    Sex = item.Staff.Sex.HasValue ? (item.Staff.Sex.Value == 0 ? "М" : "Ж") : "",
                    Dismissed = item.Staff.Dismissed.HasValue ? (item.Staff.Dismissed.Value == 1 ? "У" : " ") : " ",
                    DateBirth = dateb,
                    DepName = item.Staff.DepartmentID.HasValue ? (item.Staff.Department.Code + " " + item.Staff.Department.Name) : " "
                });



            }
            //}
            staffGridView.DataSource = staffList.OrderBy(x => x.FIO);
            if (descriptor != null)
            {
                this.staffGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (staffList.Count > 0)
            {
                staffGridView.Columns[0].Width = 26;
                staffGridView.Columns["ID"].IsVisible = false;
                staffGridView.Columns["ID"].VisibleInColumnChooser = false;
                staffGridView.Columns["Num"].Width = 26;
                staffGridView.Columns["Num"].IsVisible = false;
                staffGridView.Columns["Num"].VisibleInColumnChooser = false;
                staffGridView.Columns["Num"].ReadOnly = true;
                staffGridView.Columns["Num"].HeaderText = "Номер";
                staffGridView.Columns["FIO"].Width = 230;
                staffGridView.Columns["FIO"].ReadOnly = true;
                staffGridView.Columns["FIO"].WrapText = true;
                staffGridView.Columns["FIO"].HeaderText = "Фамилия Имя Отчество";
                staffGridView.Columns["SNILS"].Width = 100;
                staffGridView.Columns["SNILS"].HeaderText = "СНИЛС";
                staffGridView.Columns["INN"].Width = 80;
                staffGridView.Columns["INN"].HeaderText = "ИНН";
                staffGridView.Columns["TabelNumber"].HeaderText = "Табел.№";
                staffGridView.Columns["TabelNumber"].Width = 80;
                staffGridView.Columns["Sex"].HeaderText = "Пол";
                staffGridView.Columns["Sex"].Width = 50;
                staffGridView.Columns["Dismissed"].HeaderText = "Уволен";
                staffGridView.Columns["Dismissed"].Width = 50;
                staffGridView.Columns["DateBirth"].HeaderText = "Дата рождения";
                staffGridView.Columns["DateBirth"].Width = 110;
                staffGridView.Columns["DepName"].HeaderText = "Подразделение";
                staffGridView.Columns["DepName"].IsVisible = false;
                staffGridView.Columns["Period"].VisibleInColumnChooser = false;
                staffGridView.Columns["Period"].IsVisible = false;
                staffGridView.Columns["TypeInfo"].VisibleInColumnChooser = false;
                staffGridView.Columns["TypeInfo"].IsVisible = false;
                staffGridView.Columns["KorrPeriod"].VisibleInColumnChooser = false;
                staffGridView.Columns["KorrPeriod"].IsVisible = false;
                staffGridView.Columns["InsReg"].VisibleInColumnChooser = false;
                staffGridView.Columns["InsReg"].IsVisible = false;
                staffGridView.Columns["InsName"].VisibleInColumnChooser = false;
                staffGridView.Columns["InsName"].IsVisible = false;

                for (var i = 4; i < (staffGridView.Columns.Count - 2); i++)
                {
                    staffGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    staffGridView.Columns[i].ReadOnly = true;
                }

                this.staffGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }

            foreach (var item in staffGridView.Rows)
            {
                item.MinHeight = 22;
            }

            this.staffGridView.TableElement.EndUpdate();

            if (staffGridView.ChildRows.Count > 0)
            {
                if (staffGridView.Rows.Any(x => x.Cells[1].Value.ToString() == currId))
                {
                    staffGridView.Rows.First(x => x.Cells[1].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= staffGridView.ChildRows.Count)
                        rowindex = staffGridView.ChildRows.Count - 1;
                    rowindex = rowindex >= 0 ? rowindex : 0;
                    staffGridView.ChildRows[rowindex].IsCurrent = true;
                }

            }

        }

        private void addBtn_Click(object sender, EventArgs e )
        {
            addNewSzvtd(new List<long>());

        }

        private void addNewSzvtd(List<long> ids)
        {
            if (Options.InsID != 0)
            {
                SZV_TD_Edit child = new SZV_TD_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.szvtdStaffListIds = ids;
                child.action = "add";
                child.SZVTD = new FormsSZV_TD_2020();
                child.ShowDialog();
                child.Dispose();
                if (child.SZVTD != null)
                {
                    db.ChangeTracker.DetectChanges();
                    db = new pu6Entities();
                    szvTDGrid_upd();
                    string id = child.SZVTD.ID.ToString();
                    if (szvTDGridView.Rows.Any(x => x.Cells["ID"].Value.ToString() == id))
                        szvTDGridView.Rows.First(x => x.Cells["ID"].Value.ToString() == id).IsCurrent = true;
                }
                child.Dispose();
            }
            else
            {
                RadMessageBox.Show(this, "Необходимо выбрать Страхователя!", "Внимание!");
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (szvTDGridView.RowCount != 0 && szvTDGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(szvTDGridView.CurrentRow.Cells[0].Value.ToString());

                SZV_TD_Edit child = new SZV_TD_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.szvtdID = id;
                child.ShowDialog();
                child.Dispose();
                if (child.SZVTD != null)
                {
                    db.ChangeTracker.DetectChanges();
                    db = new pu6Entities();
                    szvTDGrid_upd();
                }
                child.Dispose();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования", "");
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (szvTDGridView.RowCount != 0 && szvTDGridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную Форму СЗВ-ТД?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    long id = long.Parse(szvTDGridView.CurrentRow.Cells[0].Value.ToString());

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsSZV_TD_2020 WHERE ([ID] = {0})", id));
                        db = new pu6Entities();
                        szvTDGrid_upd();
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("При удалении данных произошла ошибка. Код ошибки: " + ex.Message);
                    }

                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void szvTDGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_Click(null, null);
        }

        private void staffAddBtn_Click(object sender, EventArgs e)
        {
            if (szvTDGridView.ChildRows.Count > 0 && szvTDGridView.CurrentRow.Cells["ID"].Value != null && szvTDGridView.CurrentRow.Index >= 0)
            {
                long id = Convert.ToInt64(szvTDGridView.CurrentRow.Cells["ID"].Value);

                PU.FormsSZV_TD.SZV_TD_EditStaff child = new PU.FormsSZV_TD.SZV_TD_EditStaff();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
              //  child.parentName = this.Name;
                //                child.staff = db.Staff.FirstOrDefault(x => x.ID == id);
                child.SZVTD_ID = id;
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();

                staffGrid_upd();

            }
        }

        private void staffEditBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.RowCount > 0 && staffGridView.CurrentRow.Cells[1].Value != null)
            {
                long id = Convert.ToInt64(szvTDGridView.CurrentRow.Cells["ID"].Value);
                long id_szvtd_staff = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);
                int rowindex = staffGridView.CurrentRow.Index;

                PU.FormsSZV_TD.SZV_TD_EditStaff child = new PU.FormsSZV_TD.SZV_TD_EditStaff();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.SZVTD_Staff = db.FormsSZV_TD_2020_Staff.FirstOrDefault(x => x.ID == id_szvtd_staff);
                child.action = "edit";
                child.SZVTD_ID = id;
                //child.parentName = this.Name;
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                staffGrid_upd();

                if (rowindex >= 0)
                    staffGridView.Rows[rowindex].IsCurrent = true;

            }
        }

        private void staffDelBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.RowCount > 0 && staffGridView.CurrentRow.Cells[1].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    string id = staffGridView.CurrentRow.Cells[1].Value.ToString();

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsSZV_TD_2020_Staff WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    staffGrid_upd();

                }
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (szvTDGridView.RowCount > 0 && szvTDGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(szvTDGridView.CurrentRow.Cells[0].Value.ToString());
                if (db.FormsSZV_TD_2020.Any(x => x.ID == id))
                {

                    ReportViewerSZV_TD = new SZV_TD_Print();
                    ReportViewerSZV_TD.SZVTD_Data = db.FormsSZV_TD_2020.FirstOrDefault(x => x.ID == id);
                    ReportViewerSZV_TD.Owner = this;
                    ReportViewerSZV_TD.ThemeName = this.ThemeName;
                    ReportViewerSZV_TD.ShowInTaskbar = false;

                    this.Cursor = Cursors.WaitCursor;

                    radWaitingBar1.Visible = true;
                    radWaitingBar1.StartWaiting();

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerSZV_TD.createReport);
                    bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedSZV_TD);

                    bw.RunWorkerAsync();
                }
                else
                {
                    RadMessageBox.Show(this, "Не удалось загрузить данные из базы данных для печати формы", "");
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void bw_RunWorkerCompletedSZV_TD(object sender, RunWorkerCompletedEventArgs e)
        {
            radWaitingBar1.Invoke(new Action(() => { radWaitingBar1.StopWaiting(); radWaitingBar1.Visible = false; }));
            this.Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

            ReportViewerSZV_TD.ShowDialog();
        }

        private void printStaffBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.RowCount > 0 && staffGridView.CurrentRow.Cells[1].Value != null)
            {
                long id = long.Parse(szvTDGridView.CurrentRow.Cells[0].Value.ToString());
                long id_szvtd_staff = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);

                if (db.FormsSZV_TD_2020.Any(x => x.ID == id))
                {

                    ReportViewerSZV_TD = new SZV_TD_Print();
                    ReportViewerSZV_TD.SZVTD_Data = db.FormsSZV_TD_2020.FirstOrDefault(x => x.ID == id);
                    ReportViewerSZV_TD.szvtd_staff_ids_list = new List<long>() { id_szvtd_staff };
                    ReportViewerSZV_TD.Owner = this;
                    ReportViewerSZV_TD.ThemeName = this.ThemeName;
                    ReportViewerSZV_TD.ShowInTaskbar = false;

                    this.Cursor = Cursors.WaitCursor;

                    radWaitingBar1.Visible = true;
                    radWaitingBar1.StartWaiting();

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerSZV_TD.createReport);
                    bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedSZV_TD);

                    bw.RunWorkerAsync();
                }
                else
                {
                    RadMessageBox.Show(this, "Не удалось загрузить данные из базы данных для печати формы", "");
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void exportToXMLBtn_Click(object sender, EventArgs e)
        {
            if (szvTDGridView.RowCount != 0 && szvTDGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(szvTDGridView.CurrentRow.Cells[0].Value.ToString());

                SZV_TD_CreateXmlPack child = new SZV_TD_CreateXmlPack();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.szvtdID = id;
                child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для выгрузки", "");
            }
        }

        private void staffGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            staffEditBtn_Click(null, null);
        }

        private void copyStaffItemsMenuItem_Click(object sender, EventArgs e)
        {
            if (staffGridView.RowCount > 0 && staffGridView.CurrentRow.Cells[1].Value != null)
            {
                List<long> ids = new List<long>();

                if (staffGridView.CurrentRow.Cells[1].Value != null)
                {
                    ids.Add(long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString()));
                }

                if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {
                    ids = new List<long>();


                    foreach (var item in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                    {
                        ids.Add(long.Parse(item.Cells[1].Value.ToString()));
                    }

                }

                addNewSzvtd(ids);

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для копирования", "");
            }
        }




    }
}
