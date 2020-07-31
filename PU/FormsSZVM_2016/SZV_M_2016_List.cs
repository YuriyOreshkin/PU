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
using PU.Classes;
using Telerik.WinControls.UI;
using System.Globalization;
using Telerik.WinControls.Data;

namespace PU.FormsSZVM_2016
{
    delegate void DelEvent();

    public partial class SZV_M_2016_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками

        public SZV_M_2016_List()
        {
            InitializeComponent();
        }

        private void SZV_M_2016_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            HeaderChange();
        }

        private void closeBtn_Click(object sender, EventArgs e)
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
        /// Смена заголовка при выборе страхователя
        /// </summary>
        public void HeaderChange()
        {
            insNamePanel.Text = Methods.HeaderChange();

            szvmGrid_upd();

        }

        public class SzvmItem
        {
            public long ID { get; set; }
            public short Year { get; set; }
            public string Month { get; set; }
            public string TypeInfo { get; set; }
            public int Count { get; set; }
        }


        DelEvent d;

        public void szvmGrid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (szvmGridView.RowCount > 0 && szvmGridView.CurrentRow != null)
            {
                rowindex = szvmGridView.CurrentRow.Index;
                currId = szvmGridView.CurrentRow.Cells[0].Value.ToString();
            }

            d = null;

            SortDescriptor descriptor = new SortDescriptor();

            if (szvmGridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = szvmGridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "YEAR";
                descriptor.Direction = ListSortDirection.Ascending;
            }

            var szvm = db.FormsSZV_M_2016.Where(x => x.InsurerID == Options.InsID).OrderBy(x => x.YEAR);

            List<SzvmItem> szvmList = new List<SzvmItem>();

            if (szvm.Count() != 0)
            {
                foreach (var item in szvm)
                {
                    string typeinfo = "";
                    switch (item.TypeInfoID)
                    {
                        case 1:
                            typeinfo = "ИСХД";
                            break;
                        case 2:
                            typeinfo = "ДОП";
                            break;
                        case 3:
                            typeinfo = "ОТМН";
                            break;
                    }

                    szvmList.Add(new SzvmItem()
                    {
                        ID = item.ID,
                        Year = item.YEAR,
                        Month = item.MONTH.ToString().PadLeft(2, '0') + " - " + DateTimeFormatInfo.CurrentInfo.GetMonthName(item.MONTH),
                        TypeInfo = typeinfo,
                        Count = db.FormsSZV_M_2016_Staff.Count(x => x.FormsSZV_M_2016_ID == item.ID) //item.FormsSZV_M_2016_Staff.Count
                    });
                }
            }

            this.szvmGridView.TableElement.BeginUpdate();

            szvmGridView.DataSource = szvmList.OrderBy(x => x.Year);
            if (descriptor != null)
            {
                this.szvmGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (szvmList.Count > 0)
            {
                szvmGridView.Columns["ID"].IsVisible = false;
                szvmGridView.Columns["Year"].Width = 100;
                szvmGridView.Columns["Year"].HeaderText = "Год";
                szvmGridView.Columns["Month"].Width = 200;
                szvmGridView.Columns["Month"].HeaderText = "Месяц";
                szvmGridView.Columns["TypeInfo"].Width = 150;
                szvmGridView.Columns["TypeInfo"].HeaderText = "Тип формы";
                szvmGridView.Columns["Count"].Width = 150;
                szvmGridView.Columns["Count"].HeaderText = "Количество сотрудников";


                for (var i = 0; i < szvmGridView.Columns.Count; i++)
                {
                    szvmGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    szvmGridView.Columns[i].ReadOnly = true;
                }

                this.szvmGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }

            foreach (var item in szvmGridView.Rows)
            {
                item.MinHeight = 22;
            }

            this.szvmGridView.TableElement.EndUpdate();

            d += staffGrid_upd;

            if (szvmGridView.ChildRows.Count > 0)
            {
                if (szvmGridView.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    szvmGridView.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= szvmGridView.RowCount)
                        rowindex = szvmGridView.RowCount - 1;
                    szvmGridView.Rows[rowindex].IsCurrent = true;
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
            long szvmID = 0;
            if (szvmGridView.RowCount > 0)
            {
                szvmID = Convert.ToInt64(szvmGridView.CurrentRow.Cells[0].Value);
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


            var szvmstaffList = db.FormsSZV_M_2016_Staff.Where(x => x.FormsSZV_M_2016_ID == szvmID).ToList();
            List<long> t = szvmstaffList.Select(x => x.StaffID).ToList();
            List<Staff> staffL = db.Staff.Where(x => t.Contains(x.ID)).ToList();

            this.staffGridView.TableElement.BeginUpdate();

            List<StaffObject> staffList = new List<StaffObject> { };

            //if (szvmstaffList.Count() != 0)
            //{
                foreach (var item in szvmstaffList)
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

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (Options.InsID != 0)
            {


                SZV_M_2016_Edit child = new SZV_M_2016_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.SZVM = new FormsSZV_M_2016();
                child.ShowDialog();
                child.Dispose();
                db.DetectChanges();
                db = new pu6Entities();
                szvmGrid_upd();
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (szvmGridView.RowCount != 0 && szvmGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(szvmGridView.CurrentRow.Cells[0].Value.ToString());

                SZV_M_2016_Edit child = new SZV_M_2016_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.szvmID = id;
                child.ShowDialog();
                child.Dispose();
                db.DetectChanges();
                db = new pu6Entities();
                szvmGrid_upd();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования", "");
            }
        }



        private void delBtn_Click(object sender, EventArgs e)
        {
            if (szvmGridView.RowCount != 0 && szvmGridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную Форму СЗВ-М?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    long id = long.Parse(szvmGridView.CurrentRow.Cells[0].Value.ToString());

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_M_2016 WHERE ([ID] = {0})", id));
                        db = new pu6Entities();
                        szvmGrid_upd();
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

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (szvmGridView.RowCount != 0 && szvmGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = 0;
                long.TryParse(szvmGridView.CurrentRow.Cells[0].Value.ToString(), out id);

                if (id != 0)
                    SZV_M_2016_Print.PrintSZVM(id, this.ThemeName);
                else
                    RadMessageBox.Show(this, "Не удалось сформировать печатную форму!");
            }
            else
                RadMessageBox.Show(this, "Нет данных для печати!");
        }

        private void exportToXMLBtn_Click(object sender, EventArgs e)
        {
            if (szvmGridView.RowCount != 0 && szvmGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(szvmGridView.CurrentRow.Cells[0].Value.ToString());

                CreateXmlPack_SZV_M_2016 child = new CreateXmlPack_SZV_M_2016();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.szvmID = id;
                child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для выгрузки", "");
            }
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            if (szvmGridView.RowCount != 0 && szvmGridView.CurrentRow.Cells[0].Value != null)
            {
                long szvmID = long.Parse(szvmGridView.CurrentRow.Cells[0].Value.ToString());

                SZV_M_2016_Copy child = new SZV_M_2016_Copy();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.szvmID = szvmID;
                child.ShowDialog();
                db.DetectChanges();
                db = new pu6Entities();
                szvmGrid_upd();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для копирования", "");
            }
        }

        private void szvmGridView_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            if (d != null)
                d();
        }

        private void staffGridView_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //if (e.CellElement.ColumnInfo.Name == "Num" && e.CellElement.RowIndex >= 0)
            //{
            //    e.CellElement.Value = e.CellElement.RowIndex + 1;
            //}
        }

        private void staffAddBtn_Click(object sender, EventArgs e)
        {
            if (szvmGridView.RowCount > 0 && szvmGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = Convert.ToInt64(szvmGridView.CurrentRow.Cells[0].Value);

                SZV_M_2016_EditStaff child = new SZV_M_2016_EditStaff();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.action = "add";
                child.SZVMData = db.FormsSZV_M_2016.FirstOrDefault(x => x.ID == id);
                child.ShowDialog();
                if (child.SZVMStaffData != null)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    szvmGrid_upd();
                }
            }
        }

        private void staffDelBtn_Click(object sender, EventArgs e)
        {
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

                        string result = String.Empty;

                        string list = String.Join(",", id.ToArray());
                        try
                        {
                            db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_M_2016_Staff WHERE ([ID] IN ({0}))", list));
                        }
                        catch (Exception ex)
                        {
                            result = ex.Message;
                        }

                        this.Cursor = Cursors.Default;


                        if (!String.IsNullOrEmpty(result))
                        {
                            Methods.showAlert("Внимание!", "При удалении данных произошла ошибка. Код исключения: " + result, this.ThemeName, 200);
                        }

                        db.DetectChanges();
                        db = new pu6Entities();
                        szvmGrid_upd();

                    }
                }
            }









        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (szvmGridView.RowCount > 0 && szvmGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(szvmGridView.CurrentRow.Cells[0].Value.ToString());

                SZV_M_2016_FillStaff child = new SZV_M_2016_FillStaff();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.SZVMData = db.FormsSZV_M_2016.FirstOrDefault(x => x.ID == id);
                child.ShowDialog();
                if (child.Updated)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    szvmGrid_upd();
                }

            }
        }

        private void staffGridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
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
                e.ContextMenu.Items.Insert(3, new RadMenuSeparatorItem());
                return;
            }

            RadMenuItem customMenuItem = new RadMenuItem();
            customMenuItem.Text = "Группирование вкл/откл";
            customMenuItem.Click += (s, с) => staffGridView_toggleGrouping();
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            e.ContextMenu.Items.Add(separator);
            e.ContextMenu.Items.Add(customMenuItem);
        }

        private void staffGridView_toggleGrouping()
        {
            staffGridView.EnableGrouping = !staffGridView.EnableGrouping;
        }

        private void szvmGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_Click(null, null);
        }

        private void staffEditBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                long id = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());
                long staffID = db.FormsSZV_M_2016_Staff.FirstOrDefault(x => x.ID == id).StaffID;

                StaffEdit child = new StaffEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.staffData = db.Staff.FirstOrDefault(x => x.ID == staffID);
                child.action = "edit";
                child.InsID = Options.InsID;
                child.ShowDialog();
                db = new pu6Entities();
                staffGrid_upd();
            }
        }

        private void szvmGridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(addBtn_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(editBtn_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(delBtn_Click);
                RadMenuItem menuItem4 = new RadMenuItem("Печать");
                menuItem4.Click += new EventHandler(printBtn_Click);
                RadMenuItem menuItem5 = new RadMenuItem("Запись в XML-файл");
                menuItem5.Click += new EventHandler(exportToXMLBtn_Click);
                e.ContextMenu.Items.Insert(0, menuItem1);
                e.ContextMenu.Items.Insert(1, menuItem2);
                e.ContextMenu.Items.Insert(2, menuItem3);
                e.ContextMenu.Items.Insert(3, new RadMenuSeparatorItem());
                e.ContextMenu.Items.Insert(4, menuItem4);
                e.ContextMenu.Items.Insert(5, menuItem5);
                e.ContextMenu.Items.Insert(6, new RadMenuSeparatorItem());
                return;
            }


        }

        private void staffGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                staffEditBtn_Click(null, null);
            }
            else if (e.KeyChar == 8)
            {
                if (this.staffGridView.MasterView.TableFilteringRow.Cells["FIO"].Value != null && !String.IsNullOrEmpty(this.staffGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString()))
                {
                    this.staffGridView.MasterView.TableFilteringRow.Cells["FIO"].Value = this.staffGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString().Remove(this.staffGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString().Length - 1);
                }
            }
            else
            {
                this.staffGridView.MasterView.TableFilteringRow.Cells["FIO"].Value = (this.staffGridView.MasterView.TableFilteringRow.Cells["FIO"].Value != null ? this.staffGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString() : "") + e.KeyChar;
            }
        }

        private void staffGridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if ((e.GridViewTemplate.MasterTemplate.CurrentRow == null || e.GridViewTemplate.MasterTemplate.CurrentRow.Index < 0) && e.GridViewTemplate.ChildRows.Count > 0 && !staffGridView.MasterView.TableFilteringRow.IsCurrent)
            {
                e.GridViewTemplate.ChildRows.First().IsCurrent = true;
            }
        }




    }
}
