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

namespace PU.FormsSPW2_2014
{
    delegate void DelEvent();

    public partial class SPW2_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public long InsID = 0;   // ID страхователя
        public long StaffID { get; set; }
        List<StaffObject> staffList = new List<StaffObject> { };
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
        SPW2_Print ReportViewerSPW2;

        public SPW2_List()
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
                    spwAddBtn.Enabled = false;
                    spwDelBtn.Enabled = false;
                    radButton1.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.BeginInvoke(new MethodInvoker(this.Close));
                    return;
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

        public static SPW2_List SelfRef
        {
            get;
            set;
        }

        private void SPW2_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            checkAccessLevel();

            InsID = Options.InsID;
            HeaderChange();
            this.staffGridView.CurrentRowChanged += (s, с) => staffGridView_CurrentRowChanged();
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


            var staff = db.Staff.Where(x => x.InsurerID == Options.InsID).ToList();

            List<string> checkedItems = new List<string>();
            foreach (var row in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
            {
                checkedItems.Add(row.Cells[1].Value.ToString());
            }

            this.staffGridView.TableElement.BeginUpdate();

            staffList = new List<StaffObject> { };
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
                        TabelNumber = item.TabelNumber, // != null ? item.TabelNumber.Value.ToString() : ""
                        Sex = item.Sex.HasValue ? (item.Sex.Value == 0 ? "М" : "Ж") : "",
                        Dismissed = item.Dismissed.HasValue ? (item.Dismissed.Value == 1 ? "У" : " ") : " ",
                        DateBirth = dateb,
                        DepName = item.DepartmentID.HasValue ? (item.Department.Code + " " + item.Department.Name) : " "
                    });

                }

                staffGridView.DataSource = staffList;//.OrderBy(x => x.FIO)
            }
            else
            {
                staffGridView.DataSource = null;
            }


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

            d += spw2Grid_upd;

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
                spw2Grid_upd();
            }
            else
            {
                spw2GridView.Rows.Clear();
            }
        }



        public void spw2Grid_upd()
        {
            long staffID = 0;
            if (staffGridView.RowCount > 0)
            {
                staffID = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);
            }
            this.spw2GridView.TableElement.BeginUpdate();
            spw2GridView.Rows.Clear();

            if (staffID != 0)
            {
                var spw2 = db.FormsSPW2.Where(x => x.StaffID == staffID);

                if (spw2.Count() != 0)
                {
                    foreach (var item in spw2)
                    {
                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.spw2GridView.MasterView);
                        rowInfo.Cells["ID"].Value = item.ID;
                        if (item.DispatchPFR.HasValue)
                            if (item.DispatchPFR.Value > 0)
                                rowInfo.Cells["PF"].Value = "";

                        rowInfo.Cells["PlatCategory"].Value = item.PlatCategory != null ? item.PlatCategory.Code : "Ошибка данных: Не найдена категория";
                        rowInfo.Cells["TypeInfo"].Value = item.TypeInfo != null ? item.TypeInfo.Name : "Ошибка данных: Не найден тип сведений";
                        rowInfo.Cells["Period"].Value = Options.RaschetPeriodInternal.Any(x => x.Year == item.Year && x.Kvartal == item.Quarter) ? (item.Quarter.ToString() + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Quarter).Name) : "Период не определен";

                        string q = item.QuarterKorr.HasValue ? item.QuarterKorr.Value.ToString() : "";
                        string y = item.YearKorr.HasValue && item.YearKorr.Value > 0 ? item.YearKorr.Value.ToString() : "";
                        rowInfo.Cells["KorrPeriod"].Value = q == "" || y == "" ? "" : q + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == item.YearKorr.Value && x.Kvartal == item.QuarterKorr.Value).Name;
                        rowInfo.Cells["DateFilling"].Value = item.DateFilling.ToShortDateString();
                        spw2GridView.Rows.Add(rowInfo);
                    }
                }

            }

            this.spw2GridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.spw2GridView.TableElement.EndUpdate();
            spw2GridView.Refresh();

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
        /// <summary>
        /// Смена заголовка при выборе страхователя
        /// </summary>
        public void HeaderChange()
        {
            insNamePanel.Text = Methods.HeaderChange();
            InsID = Options.InsID;
            staffGrid_upd();

        }


        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
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
                child.InsID = InsID;
                child.ShowDialog();
                if (child.staff != null)
                {
                    staffGrid_upd();
                    string contrNum = "";
                    if (child.staff.ControlNumber != null)
                    {
                        contrNum = child.staff.ControlNumber.HasValue ? child.staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                    }

                    string snils = !String.IsNullOrEmpty(child.staff.InsuranceNumber) ? child.staff.InsuranceNumber.Substring(0, 3) + "-" + child.staff.InsuranceNumber.Substring(3, 3) + "-" + child.staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : "";
                    staffGridView.Rows.First(x => x.Cells["SNILS"].Value.ToString() == snils).IsCurrent = true;
                }
                child.Dispose();
            }

        }

        private void staffEditBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                var rowindex = staffGridView.CurrentRow.Index;
                long id = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);

                StaffEdit child = new StaffEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.staffData = db.Staff.FirstOrDefault(x => x.ID == id);
                child.action = "edit";
                child.InsID = InsID;
                child.ShowDialog();
                if (child.staffData != null)
                {
                    db.ChangeTracker.DetectChanges();
                    db = new pu6Entities();
                    staffGrid_upd();
//                    staffGridView.Rows[rowindex].IsCurrent = true;
                }
                child.Dispose();

            }

        }

        private void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            staffGrid_upd();
        }

        /// <summary>
        /// Добавление новой записи СПВ2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spwAddBtn_Click(object sender, EventArgs e)
        {
            if (!spwAddBtn.Enabled)
                return;

            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                int rowindex = spw2GridView.RowCount > 0 ? spw2GridView.CurrentRow.Index : -1;
                //        int rowindex = staffGridView.CurrentRow.Index;
                long id = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);

                SPW2_Edit child = new SPW2_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.staff = db.Staff.FirstOrDefault(x => x.ID == id);
                child.action = "add";
                child.ShowDialog();
                child.Dispose();
                spw2Grid_upd();
                if (rowindex >= 0)
                    spw2GridView.Rows[rowindex].IsCurrent = true;
            }
        }


        /// <summary>
        /// Редактирование записи СПВ2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spwEditBtn_Click(object sender, EventArgs e)
        {
            if (spw2GridView.RowCount > 0 && spw2GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);
                long id_spw2 = Convert.ToInt64(spw2GridView.CurrentRow.Cells[0].Value);
                int rowindex = spw2GridView.CurrentRow.Index;

                SPW2_Edit child = new SPW2_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.staff = db.Staff.FirstOrDefault(x => x.ID == id);
                child.formData = db.FormsSPW2.FirstOrDefault(x => x.ID == id_spw2);
                child.action = "edit";
                child.ShowDialog();
                child.Dispose();
                spw2Grid_upd();
                spw2GridView.Rows[rowindex].IsCurrent = true;

            }


        }

        private void spwStajBtn_Click(object sender, EventArgs e)
        {
            if (spw2GridView.RowCount > 0 && spw2GridView.CurrentRow.Cells[0].Value != null)
            {
                long id_spw2 = Convert.ToInt64(spw2GridView.CurrentRow.Cells[0].Value);

                StajList child = new StajList();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.spw2 = db.FormsSPW2.FirstOrDefault(x => x.ID == id_spw2);
                child.parentName = "SPW2";
                child.ShowDialog();
            }
        }


        private void staffGridView_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            spw2Grid_upd();
        }

        private void staffDelBtn_Click(object sender, EventArgs e)
        {
            if (!staffDelBtn.Enabled)
                return;

            if (staffGridView.Rows.Count() > 0)
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

        private void spwDelBtn_Click(object sender, EventArgs e)
        {
            if (!spwAddBtn.Enabled)
                return;

            if (spw2GridView.RowCount > 0 && spw2GridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    string id = spw2GridView.CurrentRow.Cells[0].Value.ToString();

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsSPW2 WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    spw2Grid_upd();

                }
            }
        }

        private void staffGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            staffEditBtn_Click(null, null);
        }

        private void spw2GridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            spwEditBtn_Click(null, null);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (!radButton1.Enabled)
                return;

            CreateXmlPack_SPW2_2014 child = new CreateXmlPack_SPW2_2014();

            if (staffGridView.RowCount > 0)
            {
                int rowindex = staffGridView.CurrentRow.Index;
                child.currentStaffId = long.Parse(staffGridView.Rows[rowindex].Cells[1].Value.ToString());

                List<long> id = new List<long>();

                if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {
                    foreach (var item in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                    {
                        id.Add(long.Parse(item.Cells[1].Value.ToString()));
                    }

                    child.staffList = id;
                }
            }

            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }


        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReportViewerSPW2.ShowDialog();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (spw2GridView.RowCount != 0 && spw2GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(spw2GridView.CurrentRow.Cells[0].Value.ToString());

                FormsSPW2 spw_data = db.FormsSPW2.FirstOrDefault(x => x.ID == id);

                ReportViewerSPW2 = new SPW2_Print();
                ReportViewerSPW2.SPW_2_List = new List<FormsSPW2>();
                ReportViewerSPW2.SPW_2_List.Add(spw_data);
                ReportViewerSPW2.Owner = this;
                ReportViewerSPW2.ThemeName = this.ThemeName;
                ReportViewerSPW2.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerSPW2.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void SPW2_List_Shown(object sender, EventArgs e)
        {
            if (!Options.fixCurrentInsurer)
            {
                insurerBtn_Click(null, null);
            }
        }

        private void staffGridView_CreateRow(object sender, GridViewCreateRowEventArgs e)
        {
            if (e.RowInfo.Index == -1)
            {
                e.RowInfo.MinHeight = 24;
            }
        }

        private void printStaffListBtn_Click(object sender, EventArgs e)
        {
            //if (staffGridView.RowCount > 0)
            //{
            //    ReportMethods.PrintStaff(staffList, this.ThemeName);
            //}
            if (staffGridView.RowCount > 0)
            {
                StaffPrint child = new StaffPrint();

                if (staffGridView.CurrentRow.Cells[1].Value != null)
                {
                    child.currentStaffId = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());
                }

                List<long> id = new List<long>();

                if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {
                    //foreach (var item in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                    //{
                    //    id.Add(long.Parse(item.Cells[1].Value.ToString()));
                    //}

                    child.staffList_checked = staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true && x.Cells[1].Value != null).Select(x => (long)x.Cells[1].Value).ToList();
                }

                child.staffList_current = staffGridView.Rows.Where(x => x.Cells[1].Value != null).Select(x => (long)x.Cells[1].Value).ToList();


                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.action = "staff";
                child.formParent = "spw2";
                child.ShowDialog();

            }
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

        private void spw2GridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(spwAddBtn_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(spwEditBtn_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(spwDelBtn_Click);
                RadMenuItem menuItem4 = new RadMenuItem("Печать");
                menuItem4.Click += new EventHandler(printBtn_Click);
                RadMenuItem menuItem6 = new RadMenuItem("Формировать пачки");
                menuItem6.Click += new EventHandler(radButton1_Click);
                RadMenuItem menuItem5 = new RadMenuItem("Стаж");
                menuItem5.Click += new EventHandler(spwStajBtn_Click);
                e.ContextMenu.Items.Insert(0, menuItem1);
                e.ContextMenu.Items.Insert(1, menuItem2);
                e.ContextMenu.Items.Insert(2, menuItem3);
                e.ContextMenu.Items.Insert(3, new RadMenuSeparatorItem());
                e.ContextMenu.Items.Insert(4, menuItem5);
                e.ContextMenu.Items.Insert(5, new RadMenuSeparatorItem());
                e.ContextMenu.Items.Insert(6, menuItem4);
                e.ContextMenu.Items.Insert(7, menuItem6);
                e.ContextMenu.Items.Insert(8, new RadMenuSeparatorItem());
                return;
            }
        }
    }
}
