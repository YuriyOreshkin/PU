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
using Telerik.WinControls.Data;
using PU.Reports;

namespace PU.FormsDSW3
{
    delegate void DelEvent();

    public partial class DSW3_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        DSW3_Print ReportViewerDSW3;

        public DSW3_List()
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
                    DSW3AddBtn.Enabled = false;
                    DSW3DelBtn.Enabled = false;
                    createXMLBtn.Enabled = false;
                    radMenuItem1.Enabled = false;
                    staffAddBtn.Enabled = false;
                    staffEditBtn.Enabled = false;
                    staffDelBtn.Enabled = false;
                    radButton2.Enabled = false;
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
                    radButton2.Enabled = false;
                    break;
                case 3:
                    staffAddBtn.Enabled = false;
                    staffEditBtn.Enabled = false;
                    staffDelBtn.Enabled = false;
                    radButton2.Enabled = false;
                    break;
            }

        }

        public static DSW3_List SelfRef
        {
            get;
            set;
        }

        private void DSW3_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            checkAccessLevel();

            HeaderChange();

            this.dsw3GridView.CurrentRowChanged += (s, с) => dsw3GridView_CurrentRowChanged();

        }

        /// <summary>
        /// Смена заголовка при выборе страхователя
        /// </summary>
        public void HeaderChange()
        {
            insNamePanel.Text = Methods.HeaderChange();
            dsw3Grid_upd();

        }

        private void dsw3GridView_CurrentRowChanged()
        {
            if (d != null)
                d();
        }

        DelEvent d;

        public void dsw3Grid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (dsw3GridView.RowCount > 0 && dsw3GridView.CurrentRow != null)
            {
                rowindex = dsw3GridView.CurrentRow.Index;
                currId = dsw3GridView.CurrentRow.Cells[0].Value.ToString();
            }

            d = null;

            SortDescriptor descriptor = new SortDescriptor();

            if (dsw3GridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = dsw3GridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "YEAR";
                descriptor.Direction = ListSortDirection.Ascending;
            }


            var dsw3List = db.FormsDSW_3.Where(x => x.InsurerID == Options.InsID).ToList();


            this.dsw3GridView.TableElement.BeginUpdate();

            dsw3GridView.DataSource = dsw3List.OrderBy(x => x.YEAR);
            if (descriptor != null)
            {
                this.dsw3GridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (dsw3List.Count > 0)
            {
                dsw3GridView.Columns["ID"].IsVisible = false;
                dsw3GridView.Columns["DepartmentID"].IsVisible = false;
                dsw3GridView.Columns["Department"].IsVisible = false;
                dsw3GridView.Columns["DISPATCHPFR"].Width = 26;
                dsw3GridView.Columns["DISPATCHPFR"].IsVisible = false;
                dsw3GridView.Columns["NUMBERPAYMENT"].Width = 200;
                dsw3GridView.Columns["NUMBERPAYMENT"].HeaderText = "Номер поручения";
                dsw3GridView.Columns["DATEPAYMENT"].Width = 100;
                dsw3GridView.Columns["DATEPAYMENT"].HeaderText = "Дата поручения";
                dsw3GridView.Columns["DATEPAYMENT"].FormatString = "{0:dd.MM.yyyy}";
                dsw3GridView.Columns["DATEEXECUTPAYMENT"].Width = 100;
                dsw3GridView.Columns["DATEEXECUTPAYMENT"].HeaderText = "Дата исполнения";
                dsw3GridView.Columns["DATEEXECUTPAYMENT"].FormatString = "{0:dd.MM.yyyy}";
                dsw3GridView.Columns["YEAR"].Width = 70;
                dsw3GridView.Columns["YEAR"].HeaderText = "Год";
                dsw3GridView.Columns["DateFilling"].Width = 80;
                dsw3GridView.Columns["DateFilling"].HeaderText = "Дата заполнения";
                dsw3GridView.Columns["DateFilling"].FormatString = "{0:dd.MM.yyyy}";
                dsw3GridView.Columns["Insurer"].IsVisible = false;
                dsw3GridView.Columns["InsurerID"].IsVisible = false;
                dsw3GridView.Columns["FormsDSW_3_Staff"].IsVisible = false;



                for (var i = 0; i < dsw3GridView.Columns.Count; i++)
                {
                    dsw3GridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    dsw3GridView.Columns[i].ReadOnly = true;
                }
                dsw3GridView.Columns["NUMBERPAYMENT"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;

                this.dsw3GridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }

            foreach (var item in dsw3GridView.Rows)
            {
                item.MinHeight = 22;
            }

            //    this.dsw3GridView.AutoSizeRows = true;

            this.dsw3GridView.TableElement.EndUpdate();

            //            dsw3GridView.Refresh();

            d += staffGrid_upd;

            if (dsw3GridView.ChildRows.Count > 0)
            {
                if (dsw3GridView.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    dsw3GridView.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= dsw3GridView.ChildRows.Count)
                        rowindex = dsw3GridView.ChildRows.Count - 1;
                    rowindex = rowindex >= 0 ? rowindex : 0;
                    dsw3GridView.ChildRows[rowindex].IsCurrent = true;
                }

                staffGrid_upd();
            }
            else
            {
                staffGridView.DataSource = null;
            }
        }

        public class StaffDSW3
        {
            public long ID { get; set; }
            public string FIO { get; set; }
            public string SNILS { get; set; }
            public string INN { get; set; }
            public string TabelNumber { get; set; }
            public decimal SUMFEEPFR_EMPLOYERS { get; set; }
            public decimal SUMFEEPFR_PAYER { get; set; }
        }

        public void staffGrid_upd()
        {
            long dsw3ID = 0;
            if (dsw3GridView.RowCount > 0 && dsw3GridView.CurrentRow.Cells[0].Value != null)
            {
                dsw3ID = Convert.ToInt64(dsw3GridView.CurrentRow.Cells[0].Value);
            }

            int rowindex = 0;
            string currId = "";
            if (staffGridView.RowCount > 0 && staffGridView.CurrentRow.Cells[0].Value != null)
            {
                rowindex = staffGridView.CurrentRow.Index;
                currId = staffGridView.CurrentRow.Cells[0].Value.ToString();
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


            var dsw3staffList = db.FormsDSW_3_Staff.Where(x => x.FormsDSW_3_ID == dsw3ID).ToList();
            var t = dsw3staffList.Select(x => x.StaffID).ToList();
            List<Staff> staffL = db.Staff.Where(x => t.Contains(x.ID)).ToList();

            this.staffGridView.TableElement.BeginUpdate();

            List<StaffDSW3> staffList = new List<StaffDSW3> { };

            if (dsw3staffList.Count() != 0)
            {
                int i = 0;
                foreach (var item in dsw3staffList)
                {
                    i++;

                    var staff = staffL.FirstOrDefault(x => x.ID == item.StaffID);

                    //string contrNum = "";
                    //if (staff.ControlNumber != null)
                    //{
                    //    contrNum = staff.ControlNumber.HasValue ? staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                    //}

                    staffList.Add(new StaffDSW3()
                    {
                        ID = item.ID,
                        FIO = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName,
                        SNILS = Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber),
                        //                        SNILS = !String.IsNullOrEmpty(staff.InsuranceNumber) ? staff.InsuranceNumber.Substring(0, 3) + "-" + staff.InsuranceNumber.Substring(3, 3) + "-" + staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : "",
                        INN = !String.IsNullOrEmpty(staff.INN) ? staff.INN.PadLeft(12, '0') : "",
                        TabelNumber = staff.TabelNumber != null ? staff.TabelNumber.Value.ToString() : "",
                        SUMFEEPFR_EMPLOYERS = item.SUMFEEPFR_EMPLOYERS.HasValue ? item.SUMFEEPFR_EMPLOYERS.Value : 0,
                        SUMFEEPFR_PAYER = item.SUMFEEPFR_PAYER.HasValue ? item.SUMFEEPFR_PAYER.Value : 0,
                    });

                }
            }
            staffGridView.DataSource = staffList.OrderBy(x => x.FIO);
            if (descriptor != null)
            {
                this.staffGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (staffList.Count > 0)
            {
                staffGridView.Columns["ID"].IsVisible = false;
                staffGridView.Columns["FIO"].Width = 230;
                staffGridView.Columns["FIO"].ReadOnly = true;
                staffGridView.Columns["FIO"].WrapText = true;
                staffGridView.Columns["FIO"].HeaderText = "Фамилия Имя Отчество";
                staffGridView.Columns["SNILS"].Width = 90;
                staffGridView.Columns["SNILS"].HeaderText = "СНИЛС";
                staffGridView.Columns["INN"].Width = 70;
                staffGridView.Columns["INN"].HeaderText = "ИНН";
                staffGridView.Columns["TabelNumber"].HeaderText = "Табел.№";
                staffGridView.Columns["TabelNumber"].Width = 70;
                staffGridView.Columns["SUMFEEPFR_EMPLOYERS"].HeaderText = "Перечислено сотрудником";
                staffGridView.Columns["SUMFEEPFR_EMPLOYERS"].Width = 70;
                staffGridView.Columns["SUMFEEPFR_EMPLOYERS"].FormatString = "{0:N2}";
                staffGridView.Columns["SUMFEEPFR_PAYER"].HeaderText = "Перечислено страхователем";
                staffGridView.Columns["SUMFEEPFR_PAYER"].Width = 70;
                staffGridView.Columns["SUMFEEPFR_PAYER"].FormatString = "{0:N2}";

                for (var i = 2; i < (staffGridView.Columns.Count - 2); i++)
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

            if (staffGridView.RowCount > 0)
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


        private void DSW3AddBtn_Click(object sender, EventArgs e)
        {
            if (!staffAddBtn.Enabled)
                return;
            
            if (Options.InsID != 0)
            {
                DSW3_Edit child = new DSW3_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.ShowDialog();
                if (child.DSW3Data != null)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    dsw3Grid_upd();
                    string num = child.DSW3Data.NUMBERPAYMENT;
                    dsw3GridView.Rows.First(x => x.Cells["NUMBERPAYMENT"].Value.ToString() == num).IsCurrent = true;
                }
            }
        }

        private void DSW3EditBtn_Click(object sender, EventArgs e)
        {
            if (dsw3GridView.RowCount > 0 && dsw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(dsw3GridView.CurrentRow.Cells[0].Value.ToString());

                DSW3_Edit child = new DSW3_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.DSW3ID = id;
                child.action = "edit";
                child.ShowDialog();
                if (child.DSW3Data != null)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    dsw3Grid_upd();
                }

            }
        }

        private void DSW3DelBtn_Click(object sender, EventArgs e)
        {
            if (!staffDelBtn.Enabled)
                return;

            if (dsw3GridView.RowCount > 0 && dsw3GridView.CurrentRow.Cells[0].Value != null)
            {
                if (RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    string id = dsw3GridView.CurrentRow.Cells[0].Value.ToString();
                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsDSW_3 WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    dsw3Grid_upd();
                    this.Cursor = Cursors.Default;
                }
            }

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (!radButton2.Enabled)
                return;
            
            if (dsw3GridView.RowCount > 0 && dsw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(dsw3GridView.CurrentRow.Cells[0].Value.ToString());

                DSW3_FillStaff child = new DSW3_FillStaff();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.dsw3Data = db.FormsDSW_3.FirstOrDefault(x => x.ID == id);
                child.ShowDialog();
                if (child.Updated)
                {
                    dsw3Grid_upd();
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
                MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
                methodsNonStatic.writeSetting();
            }

            Methods.HeaderChangeAllTabs();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void staffEditBtn_Click(object sender, EventArgs e)
        {
            if (!staffEditBtn.Enabled)
                return;
            
            if (staffGridView.RowCount > 0 && staffGridView.CurrentRow.Cells[0].Value != null && dsw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(staffGridView.CurrentRow.Cells[0].Value.ToString());
                long idDSW3 = Convert.ToInt64(dsw3GridView.CurrentRow.Cells[0].Value);

                DSW3_EditStaff child = new DSW3_EditStaff();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.DSW3StaffID = id;
                child.DSW3Data = db.FormsDSW_3.FirstOrDefault(x => x.ID == idDSW3);
                child.action = "edit";
                child.ShowDialog();
                if (child.DSW3StaffData != null)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    staffGrid_upd();
                }

            }
        }

        private void staffAddBtn_Click(object sender, EventArgs e)
        {
            if (!staffAddBtn.Enabled)
                return;
            
            if (dsw3GridView.RowCount > 0 && dsw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = Convert.ToInt64(dsw3GridView.CurrentRow.Cells[0].Value);

                DSW3_EditStaff child = new DSW3_EditStaff();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.action = "add";
                child.DSW3Data = db.FormsDSW_3.FirstOrDefault(x => x.ID == id);
                child.ShowDialog();
                if (child.DSW3StaffData != null)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    staffGrid_upd();
                }
            }
        }

        private void staffDelBtn_Click(object sender, EventArgs e)
        {
            if (!staffDelBtn.Enabled)
                return;
            
            if (staffGridView.RowCount > 0 && staffGridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    string id = staffGridView.CurrentRow.Cells[0].Value.ToString();

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsDSW_3_Staff WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    staffGrid_upd();

                }
            }
        }

        private void createXMLBtn_Click(object sender, EventArgs e)
        {
            if (!createXMLBtn.Enabled)
                return;

            if (dsw3GridView.RowCount > 0 && dsw3GridView.CurrentRow.Cells[0].Value != null)
            {
                DSW3_CreateXML child = new DSW3_CreateXML();

                long id = long.Parse(dsw3GridView.CurrentRow.Cells[0].Value.ToString());
                child.dsw3Data = db.FormsDSW_3.FirstOrDefault(x => x.ID == id);
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowDialog();
            }

        }

        private void printDSW3Btn_Click(object sender, EventArgs e)
        {
            if (dsw3GridView.RowCount > 0 && dsw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(dsw3GridView.CurrentRow.Cells[0].Value.ToString());
                printReportDSW3(id);

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void printReportDSW3(long id)
        {
            if (db.FormsDSW_3.Any(x => x.ID == id))
            {

                ReportViewerDSW3 = new DSW3_Print();
                ReportViewerDSW3.DSW3data = db.FormsDSW_3.FirstOrDefault(x => x.ID == id);
                ReportViewerDSW3.Owner = this;
                ReportViewerDSW3.ThemeName = this.ThemeName;
                ReportViewerDSW3.ShowInTaskbar = false;

                this.Cursor = Cursors.WaitCursor;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerDSW3.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedDSW3);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Не удалось загрузить данные из базы данных для печати формы", "");
            }
        }

        private void bw_RunWorkerCompletedDSW3(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

            ReportViewerDSW3.ShowDialog();
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            if (!radMenuItem1.Enabled)
                return;

            if (dsw3GridView.RowCount != 0 && dsw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long dsw3ID = long.Parse(dsw3GridView.CurrentRow.Cells[0].Value.ToString());

                DSW3_Copy child = new DSW3_Copy();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.dsw3ID = dsw3ID;
                child.ShowDialog();
                db.DetectChanges();
                db = new pu6Entities();
                dsw3Grid_upd();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для копирования", "");
            }
        }

        private void dsw3GridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            DSW3EditBtn_Click(null, null);
        }

        private void dsw3GridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(DSW3AddBtn_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(DSW3EditBtn_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(DSW3DelBtn_Click);
                RadMenuItem menuItem4 = new RadMenuItem("Печать");
                menuItem4.Click += new EventHandler(printDSW3Btn_Click);
                RadMenuItem menuItem5 = new RadMenuItem("Формировать пачки");
                menuItem5.Click += new EventHandler(createXMLBtn_Click);
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
                e.ContextMenu.Items.Insert(4, new RadMenuSeparatorItem());
                return;
            }
        }

    }
}
