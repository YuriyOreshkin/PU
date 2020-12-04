using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using PU.Models;
using PU.Classes;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;
using PU.Staj;
using Telerik.WinControls.Data;
using PU.Reports;

namespace PU.FormsSZV_6_2010
{
    delegate void DelEvent();

    public partial class SZV_6_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        List<StaffObject> staffList = new List<StaffObject> { };
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
        PU.FormsRSW2014.RSW2014_Print ReportViewerRSW2014;

        public SZV_6_List()
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            InitializeComponent();
            SelfRef = this;

        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    indSvedAddBtn.Enabled = false;
                    indSvedDelBtn.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    break;
            }

            level = Methods.checkUserAccessLevel("StaffFrm");

            switch (level)
            {
                case 2:
                    staffAddBtn.Enabled = false;
                    staffDelBtn.Enabled = false;
                    break;
                case 3:
                    staffAddBtn.Enabled = false;
                    staffEditBtn.Enabled = false;
                    staffDelBtn.Enabled = false;
                    printStaffListBtn.Enabled = false;
                    break;
            }

        }

        public static SZV_6_List SelfRef
        {
            get;
            set;
        }

        private void radButton5_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Смена заголовка при выборе страхователя
        /// </summary>
        public void HeaderChange()
        {
            radPanel1.Text = Methods.HeaderChange();
            staffGrid_upd();

        }

        private void staffGridView_CurrentRowChanged()
        {
            if (d != null)
                d();
        }

        DelEvent d;

        public void staffGrid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (staffGridView.RowCount > 0)
            {
                rowindex = staffGridView.CurrentRow.Index;
                currId = staffGridView.CurrentRow.Cells[1].Value.ToString();
            }

            d = null;

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

            var staff = db.Staff.Where(x => x.InsurerID == Options.InsID).OrderBy(x => x.LastName).ToList();

            List<string> checkedItems = new List<string>();
            foreach (var row in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
            {
                checkedItems.Add(row.Cells[1].Value.ToString());
            }


            this.staffGridView.TableElement.BeginUpdate();

            staffList.Clear();
            if (staff.Count() != 0)
            {
                foreach (var item in staff)
                {
                    string dateb = "";
                    if (item.DateBirth != null)
                    {
                        dateb = item.DateBirth.HasValue ? item.DateBirth.Value.ToShortDateString() : "";
                    }

                    staffList.Add(new StaffObject()
                    {
                        ID = item.ID,
                        FIO = item.LastName + " " + item.FirstName + " " + item.MiddleName,
                        SNILS = Utils.ParseSNILS(item.InsuranceNumber, item.ControlNumber),
                        INN = !String.IsNullOrEmpty(item.INN) ? item.INN.PadLeft(12, '0') : " ",
                        TabelNumber = item.TabelNumber,// != null ? item.TabelNumber.Value.ToString() : ""
                        Sex = item.Sex.HasValue ? (item.Sex.Value == 0 ? "М" : "Ж") : "",
                        Dismissed = item.Dismissed.HasValue ? (item.Dismissed.Value == 1 ? "У" : " ") : " ",
                        DateBirth = dateb,
                        DepName = item.DepartmentID.HasValue ? (item.Department.Code + " " + item.Department.Name) : " "
                    });

                }
            }

            staffGridView.DataSource = staffList.OrderBy(x => x.FIO);

            if (descriptor != null)
            {
                this.staffGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (staffList.Count() > 0)
            {
                staffGridView.Columns[0].Width = 26;
                staffGridView.Columns["ID"].IsVisible = false;
                staffGridView.Columns["ID"].VisibleInColumnChooser = false;
                staffGridView.Columns["Num"].Width = 26;
                staffGridView.Columns["Num"].IsVisible = false;
                staffGridView.Columns["Num"].ReadOnly = true;
                staffGridView.Columns["Num"].HeaderText = "Номер";
                staffGridView.Columns["Num"].VisibleInColumnChooser = false;
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

                for (var i = 4; i < staffGridView.Columns.Count; i++)
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

            this.staffGridView.AutoSizeRows = true;

            this.staffGridView.TableElement.EndUpdate();

            d += indSvedGrid_upd;

            if (staffGridView.ChildRows.Count > 0)
            {
                foreach (var row in staffGridView.Rows.Where(x => checkedItems.Contains(x.Cells[1].Value.ToString())))
                {
                    row.Cells[0].Value = true;
                }

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

                indSvedGrid_upd();
                //                staffGridView.Rows.First().IsCurrent = true;
            }
            else
            {
                indSvedGridView.Rows.Clear();
            }

        }

        public void indSvedGrid_upd()
        {
            long staffID = 0;
            if (staffGridView.RowCount > 0)
            {
                if (staffGridView.CurrentRow == null)
                {
                    staffGridView.Rows.First().IsCurrent = true;
                }
                staffID = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);

            }
            this.indSvedGridView.TableElement.BeginUpdate();
            indSvedGridView.Rows.Clear();

            List<RaschetPeriodContainer> avail_periods_all = new List<RaschetPeriodContainer>();
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal2010_2013.Where(x => x.Year <= 2012).OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }


            if (staffID != 0)
            {
                var indSved = db.FormsSZV_6.Where(x => x.StaffID == staffID);

                if (indSved.Count() != 0)
                {
                    foreach (var item in indSved)
                    {
                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.indSvedGridView.MasterView);
                        rowInfo.Cells["ID"].Value = item.ID;
                        if (item.DispatchPFR.HasValue)
                            if (item.DispatchPFR.Value > 0)
                                rowInfo.Cells["PF"].Value = "";

                        rowInfo.Cells["PlatCategory"].Value = item.PlatCategory != null ? item.PlatCategory.Code : "Ошибка данных: Не указана категория";
                        rowInfo.Cells["Period"].Value = avail_periods_all.Any(x => x.Year == item.Year && x.Kvartal == item.Quarter) ? (item.Quarter.ToString() + " - " + avail_periods_all.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Quarter).Name) : "Период не определен";
                        string q = item.QuarterKorr.HasValue ? item.QuarterKorr.Value.ToString() : "";
                        string y = item.YearKorr.HasValue && item.YearKorr.Value > 0 ? item.YearKorr.Value.ToString() : "";
                        rowInfo.Cells["KorrPeriod"].Value = (q == "" || y == "") ? "" : Options.RaschetPeriodInternal2010_2013.Any(x => x.Year == item.YearKorr.Value && x.Kvartal == item.QuarterKorr.Value) ? (q + " - " + Options.RaschetPeriodInternal2010_2013.FirstOrDefault(x => x.Year == item.YearKorr.Value && x.Kvartal == item.QuarterKorr.Value).Name) : "Период не определен";
                        rowInfo.Cells["DateFilling"].Value = item.DateFilling.ToShortDateString();
                        indSvedGridView.Rows.Add(rowInfo);
                    }
                }
                for (var i = 0; i < indSvedGridView.Columns.Count; i++)
                {
                    indSvedGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    indSvedGridView.Columns[i].ReadOnly = true;
                }

            }

            this.indSvedGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.indSvedGridView.TableElement.EndUpdate();
            //       indSvedGridView.Refresh();


        }

        private void staffAddBtn_Click(object sender, EventArgs e)
        {
            if (!staffAddBtn.Enabled)
                return;

            if (Options.InsID != 0)
            {
                StaffEdit child = new StaffEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.InsID = Options.InsID;
                child.ShowDialog();
                if (child.staffData != null)
                {
                    staffGrid_upd();

                    string fio = child.staffData.LastName + " " + child.staffData.FirstName + " " + child.staffData.MiddleName;
                    staffGridView.Rows.First(x => x.Cells["FIO"].Value.ToString() == fio).IsCurrent = true;
                }
                child.Dispose();

            }
        }

        private void staffEditBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                long id = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());

                StaffEdit child = new StaffEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.staffData = db.Staff.FirstOrDefault(x => x.ID == id);
                child.action = "edit";
                child.InsID = Options.InsID;
                child.ShowDialog();
                child.Dispose();
                db = new pu6Entities();
                staffGrid_upd();
            }
        }

        private void staffDelBtn_Click(object sender, EventArgs e)
        {
            if (!staffDelBtn.Enabled)
                return;
            
            if (staffGridView.RowCount > 0)
            {
                DialogResult dialogResult;
                if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true)) // если выделено несколько записей на удаление
                {
                    int cnt = staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true).Count();
                    dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить " + cnt.ToString() + " запись(ей)", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                }
                else
                {
                    dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                }

                if (dialogResult == DialogResult.Yes)
                {
                    if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
                    {
                        int rowindex = staffGridView.CurrentRow.Index;
                        List<long> id = new List<long>();

                        if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                        {
                            foreach (var item in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                            {
                                id.Add(long.Parse(item.Cells[1].Value.ToString()));
                            }
                        }
                        else
                        {
                            id.Add(long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString()));

                        }

                        this.Cursor = Cursors.WaitCursor;
                        string result = Methods.DeleteStaff(id);
                        this.Cursor = Cursors.Default;


                        if (!String.IsNullOrEmpty(result))
                        {
                            Messenger.showAlert(AlertType.Error, "Внимание!", "При удалении данных произошла ошибка. Код исключения: " + result, this.ThemeName, 200);
                        }

                        staffGrid_upd();
                        if (rowindex < staffGridView.Rows.Count())
                        {
                            staffGridView.Rows[rowindex].IsCurrent = true;
                        }
                        else if (staffGridView.Rows.Count() > 0)
                            staffGridView.Rows.Last().IsCurrent = true;

                    }
                }
            }
        }

        private void indSvedAddBtn_Click(object sender, EventArgs e)
        {
            if (!indSvedAddBtn.Enabled)
                return;

            if (staffGridView.Rows.Count() != 0)
            {
                long id = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);

                SZV_6_Edit child = new SZV_6_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.staff = db.Staff.FirstOrDefault(x => x.ID == id);
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();

                indSvedGrid_upd();

            }
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            staffGrid_upd();
        }

        private void FilterBy_SelectedIndexChanged()
        {
            staffGrid_upd();
        }



        private void staffGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            staffEditBtn_Click(null, null);
        }

        private void indSvedEditBtn_Click(object sender, EventArgs e)
        {
            if (indSvedGridView.RowCount > 0 && indSvedGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);
                long id_szv = Convert.ToInt64(indSvedGridView.CurrentRow.Cells[0].Value);
                int rowindex = indSvedGridView.CurrentRow.Index;
                var szv_6 = db.FormsSZV_6.FirstOrDefault(x => x.ID == id_szv);
                SZV_6_Edit child = new SZV_6_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.staff = db.Staff.FirstOrDefault(x => x.ID == id);
                child.SZV_6 = szv_6;
                child.period = szv_6.Quarter;
                child.action = "edit";
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();

                indSvedGrid_upd();
                if (rowindex >= 0)
                    indSvedGridView.Rows[rowindex].IsCurrent = true;

            }
        }

        private void indSvedDelBtn_Click(object sender, EventArgs e)
        {
            if (!indSvedDelBtn.Enabled)
                return;

            if (indSvedGridView.RowCount != 0 && indSvedGridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись Индивидуальных сведений?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    long id = long.Parse(indSvedGridView.CurrentRow.Cells[0].Value.ToString());

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsSZV_6 WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }

                    indSvedGrid_upd();

                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void indSvedGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            indSvedEditBtn_Click(null, null);
        }

        private void stajBtn_Click(object sender, EventArgs e)
        {
            if (indSvedGridView.RowCount > 0 && indSvedGridView.CurrentRow.Cells[0].Value != null)
            {
                long id_szv = Convert.ToInt64(indSvedGridView.CurrentRow.Cells[0].Value);

                StajList child = new StajList();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.szv_6 = db.FormsSZV_6.FirstOrDefault(x => x.ID == id_szv);
                child.parentName = "SZV_6";
                child.ShowDialog();
            }
        }

        private void SZV_6_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            checkAccessLevel();

            HeaderChange();

            this.staffGridView.CurrentRowChanged += (s, с) => staffGridView_CurrentRowChanged();
        }

        private void SZV_6_List_Shown(object sender, EventArgs e)
        {
            if (!Options.fixCurrentInsurer)
            {
                radButton5_Click(null, null);
            }
        }

        private void printStaffListBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.RowCount > 0)
            {
                ReportMethods.PrintStaff(staffList, this.ThemeName);
            }
        }

        private void staffGridView_CreateRow(object sender, GridViewCreateRowEventArgs e)
        {
            if (e.RowInfo.Index == -1)
            {
                e.RowInfo.MinHeight = 24;
            }
        }

        private void printIndSvedbtn_Click(object sender, EventArgs e)
        {
            if (indSvedGridView.RowCount != 0 && staffGridView.CurrentRow.Cells[1].Value != null)
            {
                db = new pu6Entities();
                long id_szv = Convert.ToInt64(indSvedGridView.CurrentRow.Cells[0].Value);
                var szv_6 = db.FormsSZV_6.FirstOrDefault(x => x.ID == id_szv);

                // если форма относится к СЗВ-6-2
                bool flagSZV62 = szv_6.StajOsn == null || (szv_6.StajOsn != null && szv_6.StajOsn.Count() == 0) || (szv_6.StajOsn != null && szv_6.StajOsn.Count() == 1 && (!szv_6.StajOsn.Any(c => c.StajLgot.Any())));

  //              FormsRSW2014_1_1 rsw_data = new FormsRSW2014_1_1();


                ReportViewerRSW2014 = new PU.FormsRSW2014.RSW2014_Print();
 //               ReportViewerRSW2014.RSWdata = rsw_data;
                ReportViewerRSW2014.SZV_6_data = szv_6;
                if (flagSZV62)
                    ReportViewerRSW2014.SZV_6_2_List = new List<List<FormsSZV_6>> { new List<FormsSZV_6> { szv_6 } };
                else
                    ReportViewerRSW2014.SZV_6_1_List = new List<FormsSZV_6> { szv_6 };
                ReportViewerRSW2014.Owner = this;
                ReportViewerRSW2014.ThemeName = this.ThemeName;
                ReportViewerRSW2014.ShowInTaskbar = false;
                ReportViewerRSW2014.wp = 1;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerRSW2014.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                bw.RunWorkerAsync();

                //                ReportViewerRSW2014.createReport();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReportViewerRSW2014.ShowDialog();

        }

        private void staffGridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(staffAddBtn_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(staffEditBtn_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(staffDelBtn_Click);
                e.ContextMenu.Items.Insert(0, menuItem1);
                e.ContextMenu.Items.Insert(1, menuItem2);
                e.ContextMenu.Items.Insert(2, menuItem3);
                e.ContextMenu.Items.Insert(3, separator);

                return;
            }
            RadMenuItem customMenuItem = new RadMenuItem();
            customMenuItem.Text = "Группирование вкл/откл";
            customMenuItem.Click += (s, с) => staffGridView_toggleGrouping();
            e.ContextMenu.Items.Add(separator);
            e.ContextMenu.Items.Add(customMenuItem);
        }

        private void staffGridView_toggleGrouping()
        {
            staffGridView.EnableGrouping = !staffGridView.EnableGrouping;
        }

        private void staffGridView_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            //if (e.CellElement.ColumnInfo != null && e.CellElement.ColumnInfo.Name == "Num" && e.CellElement.RowIndex >= 0)
            //{
            //    e.CellElement.Value = e.CellElement.RowIndex + 1;
            //}
        }

        private void indSvedGridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(indSvedAddBtn_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(indSvedEditBtn_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(indSvedDelBtn_Click);
                RadMenuItem menuItem4 = new RadMenuItem("Печать");
                menuItem4.Click += new EventHandler(printIndSvedbtn_Click);
                RadMenuItem menuItem5 = new RadMenuItem("Стаж");
                menuItem5.Click += new EventHandler(stajBtn_Click);
                e.ContextMenu.Items.Insert(0, menuItem1);
                e.ContextMenu.Items.Insert(1, menuItem2);
                e.ContextMenu.Items.Insert(2, menuItem3);
                e.ContextMenu.Items.Insert(3, new RadMenuSeparatorItem());
                e.ContextMenu.Items.Insert(4, menuItem5);
                e.ContextMenu.Items.Insert(5, new RadMenuSeparatorItem());
                e.ContextMenu.Items.Insert(6, menuItem4);
                e.ContextMenu.Items.Insert(7, new RadMenuSeparatorItem());
                return;
            }
        }
    }
}
