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
using Telerik.WinControls.Data;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;

namespace PU.FormsADW3
{
    delegate void DelEvent();

    public partial class FormsADW3_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками


        public FormsADW3_List()
        {
            InitializeComponent();

            SelfRef = this;
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

        public static FormsADW3_List SelfRef
        {
            get;
            set;
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void FormsADW3_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            exportToXMLBtn.ButtonElement.ToolTipText = "Формирование и просмотр пачек";

            HeaderChange();
            this.staffGridView.CurrentRowChanged += (s, с) => staffGridView_CurrentRowChanged();

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
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
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

            List<StaffObject> staffList = new List<StaffObject> { };

            //           staffList.Clear();
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

            if (staffList.Count() > 0)
            {
                staffGridView.Columns[0].Width = 26;
                staffGridView.Columns["ID"].IsVisible = false;
                staffGridView.Columns["ID"].VisibleInColumnChooser = false;
                staffGridView.Columns["Num"].IsVisible = false;
                staffGridView.Columns["Num"].ReadOnly = true;
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

            d += adw3Grid_upd;

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
                adw3Grid_upd();

            }
            else
            {
                adw3GridView.Rows.Clear();
            }

        }

        public void adw3Grid_upd()
        {
            long staffID = 0;
            if (staffGridView.RowCount > 0 && staffGridView.CurrentRow != null)
            {
                if (staffGridView.CurrentRow == null)
                {
                    staffGridView.Rows.First().IsCurrent = true;
                    //                    this.staffGridView.GridNavigator.SelectFirstRow();
                }
                staffID = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);

            }
            this.adw3GridView.TableElement.BeginUpdate();
            adw3GridView.Rows.Clear();

            if (staffID != 0)
            {
                var indSved = db.FormsADW_3.Where(x => x.StaffID == staffID);

                if (indSved.Count() != 0)
                {
                    foreach (var item in indSved)
                    {
                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.adw3GridView.MasterView);
                        rowInfo.Cells["ID"].Value = item.ID;
                        if (item.DispatchPFR.HasValue)
                            if (item.DispatchPFR.Value > 0)
                                rowInfo.Cells["PF"].Value = "";

                        rowInfo.Cells["Num"].Value = item.Num;
                        rowInfo.Cells["DateFilling"].Value = item.DateFilling.HasValue ? item.DateFilling.Value.ToShortDateString() : "";
                        rowInfo.Cells["DateUpdate"].Value = item.DateUpdate.HasValue ? item.DateUpdate.Value.ToShortDateString() : "";
                        adw3GridView.Rows.Add(rowInfo);
                    }
                }
                for (var i = 0; i < adw3GridView.Columns.Count; i++)
                {
                    adw3GridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    adw3GridView.Columns[i].ReadOnly = true;
                }

            }

            this.adw3GridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.adw3GridView.TableElement.EndUpdate();
            //       indSvedGridView.Refresh();


        }


        private void staffEditBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                long id = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());

                StaffAnketaEdit child = new StaffAnketaEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.staff = db.Staff.FirstOrDefault(x => x.ID == id);
                child.action = "edit";
                child.ShowDialog();
                db = new pu6Entities();
                staffGrid_upd();
            }
        }

        private void staffGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            staffEditBtn_Click(null, null);
        }

        private void staffAddBtn_Click(object sender, EventArgs e)
        {
            if (Options.InsID != 0)
            {
                StaffAnketaEdit child = new StaffAnketaEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.ShowDialog();
                if (child.staff != null)
                {
                    string fio = child.staff.LastName + " " + child.staff.FirstName + " " + child.staff.MiddleName;
                    db = new pu6Entities();

                    staffGrid_upd();
                    staffGridView.Rows.First(x => x.Cells["FIO"].Value.ToString() == fio).IsCurrent = true;
                }

            }
        }

        private void DelADW3Btn_Click(object sender, EventArgs e)
        {
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую Форму АДВ-1?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    //        int rowindex = indSvedGridView.CurrentRow.Index;
                    long id = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsADW_3 WHERE ([StaffID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }

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

        private void DelStaffBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
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
                            Methods.showAlert("Внимание!", "При удалении данных произошла ошибка. Код исключения: " + result, this.ThemeName, 200);
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

        private void exportToXMLBtn_Click(object sender, EventArgs e)
        {
            CreateXmlPack_ADW3 child = new CreateXmlPack_ADW3();

            if (staffGridView.RowCount > 0)
            {
                if (staffGridView.CurrentRow.Cells[1].Value != null)
                {
                    child.currentStaffId = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());
                }

                List<long> id = new List<long>();

                if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {
                    foreach (var item in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                    {
                        id.Add(long.Parse(item.Cells[1].Value.ToString()));
                    }

                    child.staffList_temp = id;
                }
            }

            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }
        FormsADW3_Print ReportViewerADW3;
        private void printStaffListBtn_Click(object sender, EventArgs e)
        {
            if (adw3GridView.RowCount != 0 && adw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(adw3GridView.CurrentRow.Cells[0].Value.ToString());

                FormsADW_3 adw3 = db.FormsADW_3.FirstOrDefault(x => x.ID == id);

                ReportViewerADW3 = new FormsADW3_Print();
                ReportViewerADW3.ADW3_List = new List<FormsADW_3>();
                ReportViewerADW3.ADW3_List.Add(adw3);
                ReportViewerADW3.Owner = this;
                ReportViewerADW3.ThemeName = this.ThemeName;
                ReportViewerADW3.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerADW3.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReportViewerADW3.ShowDialog();
        }

        private void staffDelBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
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
                            Methods.showAlert("Внимание!", "При удалении данных произошла ошибка. Код исключения: " + result, this.ThemeName, 200);
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

        private void staffGridView_CurrentViewChanged(object sender, GridViewCurrentViewChangedEventArgs e)
        {
            if (d != null)
                d();
        }

        private void addADW3Btn_Click(object sender, EventArgs e)
        {
            if (!staffAddBtn.Enabled)
                return;

            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                long id = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());

                short num = 1;
                if (adw3GridView.ChildRows.Count > 0)
                {
                    string numS = adw3GridView.ChildRows.Max(x => x.Cells["Num"].Value).ToString();

                    short.TryParse(numS, out num);
                    num++;
                }

                FormsADW3_Edit child = new FormsADW3_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.action = "add";
                child.StaffID = id;
                child.numSpinEditor.Value = num;
                child.ShowDialog();
                if (child.adw_3 != null)
                {
                    db.ChangeTracker.DetectChanges();
                    db = new pu6Entities();
                    adw3Grid_upd();
                }
            }
        }

        private void editADW3Btn_Click(object sender, EventArgs e)
        {
            if (!staffEditBtn.Enabled)
                return;

            if (adw3GridView.RowCount > 0 && adw3GridView.CurrentRow.Cells[0].Value != null && staffGridView.CurrentRow.Cells[1].Value != null)
            {
                long idSTAFF = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());
                long id = long.Parse(adw3GridView.CurrentRow.Cells[0].Value.ToString());

                FormsADW3_Edit child = new FormsADW3_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.StaffID = idSTAFF;
                child.adw_3 = db.FormsADW_3.FirstOrDefault(x => x.ID == id);
                child.action = "edit";
                child.ShowDialog();
                if (child.adw_3 != null)
                {
                    db.ChangeTracker.DetectChanges();
                    db = new pu6Entities();
                    adw3Grid_upd();
                }

            }
        }

        private void delADW3Btn_Click_1(object sender, EventArgs e)
        {
            if (!staffDelBtn.Enabled)
                return;

            if (adw3GridView.RowCount > 0 && adw3GridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    string id = adw3GridView.CurrentRow.Cells[0].Value.ToString();

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsADW_3 WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    adw3Grid_upd();

                }
            }
        }

        private void adw3GridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editADW3Btn_Click(null, null);
        }

    }
}
