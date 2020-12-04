using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using PU.Models;
using PU.Classes;
using System.Windows.Forms;
using Telerik.WinControls;
//using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;
using Telerik.WinControls.Data;

namespace PU
{
    public partial class StaffFrm : Telerik.WinControls.UI.RadForm
    {
        public pu6Entities db = new pu6Entities();
        public long InsID = 0;   // ID страхователя
        public long StaffID { get; set; }
        public string action { get; set; }
        List<StaffObject> staffList = new List<StaffObject> { };
        public bool showAllStaff { get; set; }
        public List<long> selectedStaff = new List<long>();


        public StaffFrm()
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
                    radButton3.Enabled = false;
                    radButton1.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    //this.Dispose();
                    break;
            }

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

        public static StaffFrm SelfRef
        {
            get;
            set;
        }

        public void dataGrid_upd()
        {

            //          RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

//            this.staffGridView.MasterTemplate.AutoGenerateColumns = true;

            int rowindex = 0;
            string currId = "";
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                rowindex = staffGridView.CurrentRow.Index;
                currId = staffGridView.CurrentRow.Cells[1].Value.ToString();
            }


            var ins = db.Staff.Where(x => x.InsurerID == InsID).ToList();

            if (showAllStaff)
            {
                ins = db.Staff.ToList();
                staffGridView.EnablePaging = true;
                staffGridView.EnableGrouping = true;
                staffGridView.PageSize = 50;
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

            List<string> checkedItems = new List<string>();
            foreach (var row in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
            {
                checkedItems.Add(row.Cells[1].Value.ToString());
            }

//            staffGridView.Rows.Clear(); 

            this.staffGridView.TableElement.BeginUpdate();
          
            staffList = new List<StaffObject>();
            if (ins.Count() != 0)
            {
                foreach (var item in ins)
                {
                    string dateb = "";
                    if (item.DateBirth != null)
                    {
                        dateb = item.DateBirth.HasValue ? item.DateBirth.Value.ToShortDateString() : "";
                    }

                    string InsName = showAllStaff ? (item.Insurer.TypePayer == 0 ? (item.Insurer.NameShort != null ? item.Insurer.NameShort : (item.Insurer.Name != null ? item.Insurer.Name : "")) : (item.Insurer.LastName + " " + item.Insurer.FirstName + " " + (item.Insurer.MiddleName != null ? item.Insurer.MiddleName : ""))) : "";
                    string InsReg = showAllStaff ? (!String.IsNullOrEmpty(item.Insurer.RegNum) ? Utils.ParseRegNum(item.Insurer.RegNum) : "") : "";

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
                        DepName = item.DepartmentID.HasValue ? (item.Department.Code + " " + item.Department.Name) : " ",
                        InsName = InsName,
                        InsReg = InsReg
                    });

                }
            }
            //staffGridView.DataSource = null;
            //            staffGridView.Rows.Clear();
            staffGridView.DataSource = staffList.OrderBy(x => x.FIO);
            if (descriptor != null)
            {
                this.staffGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }


            if (staffList.Count() > 0)
            {


                staffGridView.Columns[0].Width = 26;
                staffGridView.Columns[0].IsPinned = true;
                staffGridView.Columns[0].PinPosition = PinnedColumnPosition.Left;
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
                staffGridView.Columns["InsReg"].IsVisible = showAllStaff;
                staffGridView.Columns["InsReg"].VisibleInColumnChooser = showAllStaff;
                staffGridView.Columns["InsReg"].HeaderText = "Рег.номер";
                staffGridView.Columns["InsName"].IsVisible = showAllStaff;
                staffGridView.Columns["InsName"].VisibleInColumnChooser = showAllStaff;
                staffGridView.Columns["InsName"].HeaderText = "Страхователь";

            }

            for (var i = 4; i < staffGridView.Columns.Count; i++)
            {
                staffGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                staffGridView.Columns[i].ReadOnly = true;
            }

            this.staffGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            this.staffGridView.TableElement.EndUpdate();

            if (staffGridView.RowCount > 0)
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

            }

            //if (staffGridView.RowCount > 0)
            //{
            //    foreach (var row in staffGridView.Rows.Where(x => selectedStaff.Select(c => c.ToString()).Contains(x.Cells[1].Value.ToString())))
            //    {
            //        row.Cells[0].Value = true;
            //    }
            //    foreach (var row in staffGridView.Rows.Where(x => checkedItems.Contains(x.Cells[1].Value.ToString())))
            //    {
            //        row.Cells[0].Value = true;
            //    }
            //}

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (!radButton1.Enabled)
                return;

            StaffEdit child = new StaffEdit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.InsID = InsID;
            child.db = db;
            child.ShowDialog();
            if (child.staff != null)
            {

                dataGrid_upd();

                string fio = child.staff.LastName + " " + child.staff.FirstName + " " + child.staff.MiddleName;
                staffGridView.Rows.First(x => x.Cells["FIO"].Value.ToString() == fio).IsCurrent = true;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                int rowindex = staffGridView.CurrentRow.Index;

                long id = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());

                StaffEdit child = new StaffEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.staffData = db.Staff.FirstOrDefault(x => x.ID == id);
                child.action = "edit";
                child.InsID = InsID;
                child.db = db;
                child.ShowDialog();
                if (child.staff != null)
                {
                //    db = new pu6Entities();

                    dataGrid_upd();
                    staffGridView.Rows[rowindex].IsCurrent = true;
                }
            }
        }


        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StaffFrm_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            checkAccessLevel();

            if (action == "selection")
            {
                radButton5.Visible = true;
            }

            this.Cursor = Cursors.WaitCursor;
            dataGrid_upd();
            this.Cursor = Cursors.Default;

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    this.Size = param.size;
                    this.Location = param.location;
                    this.WindowState = param.windowState;
                }
                catch
                { }
            }

            if (StaffID != 0)
            {
                string id = StaffID.ToString();

                if (staffGridView.RowCount != 0)
                {
                    if (staffGridView.Rows.Any(x => x.Cells[1].Value.ToString() == id))
                        staffGridView.Rows.FirstOrDefault(x => x.Cells[1].Value.ToString() == id).IsCurrent = true;
                    else
                    {

                    }
                }
                else
                {

                }


            }

                    PU.FormsSZVM_2016.SZV_M_2016_Edit main = this.Owner as PU.FormsSZVM_2016.SZV_M_2016_Edit;
                    if (main != null)
                    {
                        radButton3.Enabled = false;
                    }


        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            switch (action)
            {
                case "selection":
                    radButton5_Click(null, null);
                    break;
                default:
                    radButton2_Click(null, null);
                    break;
            }
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            if (action == "selection")
            {
                selectedStaff = new List<long>();


                if (staffGridView.RowCount > 0)
                {
                    if (staffGridView.CurrentRow.Cells[1].Value == null)
                    {
                        staffGridView.Rows[0].IsCurrent = true;
                    }

                    int rowindex = staffGridView.CurrentRow.Index;

                    StaffID = long.Parse(staffGridView.CurrentRow.Cells[1].Value.ToString());


                    PU.FormsSZVM_2016.SZV_M_2016_Edit main = this.Owner as PU.FormsSZVM_2016.SZV_M_2016_Edit;
                    if (main != null)
                    {
                        if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
                        {
                            if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                            {
                                foreach (var item in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                {
                                    // выбираем только тех кто отмечен галочкой
                                    selectedStaff.Add(long.Parse(item.Cells[1].Value.ToString()));
                                }
                            }
                        }

                    }

                }
                else
                {
                    StaffID = 0;
                }
                this.Close();
            }
        }

        private void StaffFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (action == "selection")
            {
                if (staffGridView.RowCount == 0)
                {
                    StaffID = 0;
                }
            }

            radButton5.Visible = false;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (!radButton3.Enabled)
                return;

            DialogResult dialogResult;
            if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true)) // если выделено несколько записей на удаление
            {
                int cnt = staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true).Count();
                dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить " + cnt.ToString() + " записи(ей)", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
            }
            else
            {
                dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
            }

            if (dialogResult == DialogResult.Yes)
            {
                if (staffGridView.RowCount > 0)
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
                        id.Add(long.Parse(staffGridView.Rows[rowindex].Cells[1].Value.ToString()));

                    }

                    this.Cursor = Cursors.WaitCursor;
                    string result = Methods.DeleteStaff(id);
                    this.Cursor = Cursors.Default;


                    if (!String.IsNullOrEmpty(result))
                    {
                        Messenger.showAlert(AlertType.Error, "Внимание!", "При удалении данных произошла ошибка. Код исключения: " + result, this.ThemeName, 200);
                    }

                    dataGrid_upd();
                    if (rowindex < staffGridView.Rows.Count() && rowindex >= 0)
                    {
                        staffGridView.Rows[rowindex].IsCurrent = true;
                    }
                    else if (staffGridView.Rows.Count() > 0)
                        staffGridView.Rows.Last().IsCurrent = true;

                }
            }
        }

        private void radGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            //MessageBox.Show(radGridView1.CurrentRow.Cells[0].Value.ToString());
        }

        private void StaffFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками
            List<WindowData> windowData = new List<WindowData> { };

            props.setFormParams(this, windowData);
        }

        private void staffGridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(radButton1_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(radButton2_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(radButton3_Click);
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


        private void staffGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                switch (action)
                {
                    case "selection":
                        radButton5_Click(null, null);
                        break;
                    default:
                        radButton2_Click(null, null);
                        break;
                }
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
