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
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;
using Telerik.WinControls.Data;
using System.Globalization;

namespace PU.FormsSZVM_2016
{
    public partial class SZV_M_2016_FillStaff : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public FormsSZV_M_2016 SZVMData { get; set; }
        public bool Updated = false;
        List<long> staffSelectedIDList = new List<long>();

        public SZV_M_2016_FillStaff()
        {
            InitializeComponent();
        }

        public class StaffItemSelected
        {
            public long ID { get; set; }
            public string FIO { get; set; }
            public string SNILS { get; set; }
            public string INN { get; set; }
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

        private void SZV_M_2016_FillStaff_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            radLabel6.ThemeName = "";
            radLabel7.ThemeName = "";

            if (SZVMData != null)
            {
                SZVMData = db.FormsSZV_M_2016.FirstOrDefault(x => x.ID == SZVMData.ID);
                string typeinfo = "";
                switch (SZVMData.TypeInfoID)
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

                szvmAttr.Text = SZVMData.MONTH.ToString().PadLeft(2, '0') + " - " + DateTimeFormatInfo.CurrentInfo.GetMonthName(SZVMData.MONTH) + "   " + SZVMData.YEAR + "   [" + typeinfo + "]";


                if (SZVMData.FormsSZV_M_2016_Staff.Any())
                {
                    staffSelectedIDList = SZVMData.FormsSZV_M_2016_Staff.Select(x => x.StaffID).ToList();
                }
            }




            staffSELECTEDGrid_upd();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void staffSELECTEDGrid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (staffSELECTEDGridView.ChildRows.Count > 0 && staffSELECTEDGridView.CurrentRow.Cells[1].Value != null)
            {
                rowindex = staffSELECTEDGridView.CurrentRow.Index;
                currId = staffSELECTEDGridView.CurrentRow.Cells[1].Value.ToString();
            }

            SortDescriptor descriptor = new SortDescriptor();

            if (staffSELECTEDGridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = staffSELECTEDGridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "FIO";
                descriptor.Direction = ListSortDirection.Ascending;
            }

            List<string> checkedItems = new List<string>();
            foreach (var row in staffSELECTEDGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
            {
                checkedItems.Add(row.Cells[1].Value.ToString());
            }

            List<Staff> staffL = db.Staff.Where(x => staffSelectedIDList.Contains(x.ID)).ToList();

            this.staffSELECTEDGridView.TableElement.BeginUpdate();

            List<StaffObject> staffList = new List<StaffObject> { };

            int n = 0;
            foreach (var item in staffL)
            {
                n++;
                string dateb = "";
                if (item.DateBirth != null)
                {
                    dateb = item.DateBirth.HasValue ? item.DateBirth.Value.ToShortDateString() : "";
                }

                staffList.Add(new StaffObject()
                {
                    ID = item.ID,
                    Num = n,
                    FIO = item.LastName + " " + item.FirstName + " " + item.MiddleName,
                    SNILS = Utils.ParseSNILS(item.InsuranceNumber, item.ControlNumber),
                    INN = !String.IsNullOrEmpty(item.INN) ? item.INN.PadLeft(12, '0') : " ",
                    TabelNumber = item.TabelNumber,
                    Sex = item.Sex.HasValue ? (item.Sex.Value == 0 ? "М" : "Ж") : "",
                    Dismissed = item.Dismissed.HasValue ? (item.Dismissed.Value == 1 ? "У" : " ") : " ",
                    DateBirth = dateb
                });

            }

            staffSELECTEDGridView.DataSource = staffList.OrderBy(x => x.FIO);
            if (descriptor != null)
            {
                this.staffSELECTEDGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (staffList.Count > 0)
            {
                staffSELECTEDGridView.Columns[0].Width = 26;
                staffSELECTEDGridView.Columns["ID"].IsVisible = false;
                staffSELECTEDGridView.Columns["Num"].Width = 26;
                staffSELECTEDGridView.Columns["Num"].IsVisible = false;
                staffSELECTEDGridView.Columns["Num"].ReadOnly = true;
                staffSELECTEDGridView.Columns["Num"].HeaderText = "#";
                staffSELECTEDGridView.Columns["FIO"].Width = 250;
                staffSELECTEDGridView.Columns["FIO"].ReadOnly = true;
                staffSELECTEDGridView.Columns["FIO"].WrapText = true;
                staffSELECTEDGridView.Columns["FIO"].HeaderText = "Фамилия Имя Отчество";
                staffSELECTEDGridView.Columns["SNILS"].Width = 100;
                staffSELECTEDGridView.Columns["SNILS"].HeaderText = "СНИЛС";
                staffSELECTEDGridView.Columns["INN"].Width = 80;
                staffSELECTEDGridView.Columns["INN"].HeaderText = "ИНН";
                staffSELECTEDGridView.Columns["TabelNumber"].IsVisible = false;
                staffSELECTEDGridView.Columns["Sex"].IsVisible = false;
                staffSELECTEDGridView.Columns["Dismissed"].IsVisible = false;
                staffSELECTEDGridView.Columns["DateBirth"].IsVisible = false;
                staffSELECTEDGridView.Columns["DepName"].HeaderText = "Подразделение";
                staffSELECTEDGridView.Columns["DepName"].IsVisible = false;
                staffSELECTEDGridView.Columns["Period"].VisibleInColumnChooser = false;
                staffSELECTEDGridView.Columns["Period"].IsVisible = false;
                staffSELECTEDGridView.Columns["TypeInfo"].VisibleInColumnChooser = false;
                staffSELECTEDGridView.Columns["TypeInfo"].IsVisible = false;
                staffSELECTEDGridView.Columns["KorrPeriod"].VisibleInColumnChooser = false;
                staffSELECTEDGridView.Columns["KorrPeriod"].IsVisible = false;
                staffSELECTEDGridView.Columns["InsReg"].VisibleInColumnChooser = false;
                staffSELECTEDGridView.Columns["InsReg"].IsVisible = false;
                staffSELECTEDGridView.Columns["InsName"].VisibleInColumnChooser = false;
                staffSELECTEDGridView.Columns["InsName"].IsVisible = false;

                for (var i = 4; i < (staffSELECTEDGridView.Columns.Count - 2); i++)
                {
                    staffSELECTEDGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    staffSELECTEDGridView.Columns[i].ReadOnly = true;
                }

                staffSELECTEDcnt.Text = staffList.Count.ToString();

            }
            else
            {
                staffSELECTEDcnt.Text = "0";

            }

            foreach (var item in staffSELECTEDGridView.Rows)
            {
                item.MinHeight = 22;
            }
            staffSELECTEDGridView.MasterView.TableHeaderRow.MinHeight = 26;
            staffSELECTEDGridView.MasterView.TableFilteringRow.MinHeight = 26;

            this.staffSELECTEDGridView.TableElement.EndUpdate();

            if (staffSELECTEDGridView.ChildRows.Count > 0)
            {
                foreach (var row in staffSELECTEDGridView.Rows.Where(x => checkedItems.Contains(x.Cells[1].Value.ToString())))
                {
                    row.Cells[0].Value = true;
                }

                if (staffSELECTEDGridView.Rows.Any(x => x.Cells[1].Value.ToString() == currId))
                {
                    staffSELECTEDGridView.Rows.First(x => x.Cells[1].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= staffSELECTEDGridView.ChildRows.Count)
                        rowindex = staffSELECTEDGridView.ChildRows.Count - 1;
                    if (rowindex < 0)
                        rowindex = 0;

                        staffSELECTEDGridView.Rows[rowindex].IsCurrent = true;
                }


            }

            staffALLGrid_upd();

        }

        public void staffALLGrid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (staffALLGridView.ChildRows.Count > 0 && staffALLGridView.CurrentRow.Cells[1].Value != null)
            {
                rowindex = staffALLGridView.CurrentRow.Index;
                currId = staffALLGridView.CurrentRow.Cells[1].Value.ToString();
            }

            SortDescriptor descriptor = new SortDescriptor();

            if (staffALLGridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = staffALLGridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "FIO";
                descriptor.Direction = ListSortDirection.Ascending;
            }

            List<string> checkedItems = new List<string>();
            foreach (var row in staffALLGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
            {
                checkedItems.Add(row.Cells[1].Value.ToString());
            }

            List<Staff> staffL = db.Staff.Where(x => x.InsurerID == Options.InsID && !staffSelectedIDList.Contains(x.ID)).ToList();

            this.staffALLGridView.TableElement.BeginUpdate();

            List<StaffObject> staffList = new List<StaffObject> { };

            int n = 0;
            foreach (var item in staffL)
            {
                n++;
                string dateb = "";
                if (item.DateBirth != null)
                {
                    dateb = item.DateBirth.HasValue ? item.DateBirth.Value.ToShortDateString() : "";
                }

                staffList.Add(new StaffObject()
                {
                    ID = item.ID,
                    Num = n,
                    FIO = item.LastName + " " + item.FirstName + " " + item.MiddleName,
                    SNILS = Utils.ParseSNILS(item.InsuranceNumber, item.ControlNumber),
                    INN = !String.IsNullOrEmpty(item.INN) ? item.INN.PadLeft(12, '0') : " ",
                    TabelNumber = item.TabelNumber,
                    Sex = item.Sex.HasValue ? (item.Sex.Value == 0 ? "М" : "Ж") : "",
                    Dismissed = item.Dismissed.HasValue ? (item.Dismissed.Value == 1 ? "У" : " ") : " ",
                    DateBirth = dateb
                });

            }

            staffALLGridView.DataSource = staffList.OrderBy(x => x.FIO);
            if (descriptor != null)
            {
                this.staffALLGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (staffList.Count > 0)
            {
                staffALLGridView.Columns[0].Width = 26;
                staffALLGridView.Columns["ID"].IsVisible = false;
                staffALLGridView.Columns["Num"].Width = 26;
                staffALLGridView.Columns["Num"].IsVisible = false;
                staffALLGridView.Columns["Num"].ReadOnly = true;
                staffALLGridView.Columns["Num"].HeaderText = "#";
                staffALLGridView.Columns["FIO"].Width = 250;
                staffALLGridView.Columns["FIO"].ReadOnly = true;
                staffALLGridView.Columns["FIO"].WrapText = true;
                staffALLGridView.Columns["FIO"].HeaderText = "Фамилия Имя Отчество";
                staffALLGridView.Columns["SNILS"].Width = 100;
                staffALLGridView.Columns["SNILS"].HeaderText = "СНИЛС";
                staffALLGridView.Columns["INN"].Width = 80;
                staffALLGridView.Columns["INN"].HeaderText = "ИНН";
                staffALLGridView.Columns["TabelNumber"].HeaderText = "Табел.№";
                staffALLGridView.Columns["TabelNumber"].Width = 80;
                staffALLGridView.Columns["Sex"].HeaderText = "Пол";
                staffALLGridView.Columns["Sex"].IsVisible = false;
                staffALLGridView.Columns["Dismissed"].HeaderText = "Уволен";
                staffALLGridView.Columns["Dismissed"].Width = 50;
                staffALLGridView.Columns["DateBirth"].HeaderText = "Дата рождения";
                staffALLGridView.Columns["DateBirth"].IsVisible = false;
                staffALLGridView.Columns["DepName"].HeaderText = "Подразделение";
                staffALLGridView.Columns["DepName"].IsVisible = false;
                staffALLGridView.Columns["Period"].VisibleInColumnChooser = false;
                staffALLGridView.Columns["Period"].IsVisible = false;
                staffALLGridView.Columns["TypeInfo"].VisibleInColumnChooser = false;
                staffALLGridView.Columns["TypeInfo"].IsVisible = false;
                staffALLGridView.Columns["KorrPeriod"].VisibleInColumnChooser = false;
                staffALLGridView.Columns["KorrPeriod"].IsVisible = false;
                staffALLGridView.Columns["InsReg"].VisibleInColumnChooser = false;
                staffALLGridView.Columns["InsReg"].IsVisible = false;
                staffALLGridView.Columns["InsName"].VisibleInColumnChooser = false;
                staffALLGridView.Columns["InsName"].IsVisible = false;

                for (var i = 4; i < (staffALLGridView.Columns.Count - 2); i++)
                {
                    staffALLGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    staffALLGridView.Columns[i].ReadOnly = true;
                }

                staffALLcnt.Text = staffList.Count.ToString();

            }
            else
            {
                staffALLcnt.Text = "0";

            }

            foreach (var item in staffALLGridView.Rows)
            {
                item.MinHeight = 22;
            }
            staffALLGridView.MasterView.TableHeaderRow.MinHeight = 26;
            staffALLGridView.MasterView.TableFilteringRow.MinHeight = 26;

            this.staffALLGridView.TableElement.EndUpdate();

            if (staffALLGridView.ChildRows.Count > 0)
            {
                foreach (var row in staffALLGridView.Rows.Where(x => checkedItems.Contains(x.Cells[1].Value.ToString())))
                {
                    row.Cells[0].Value = true;
                }

                if (staffALLGridView.Rows.Any(x => x.Cells[1].Value.ToString() == currId))
                {
                    staffALLGridView.Rows.First(x => x.Cells[1].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= staffALLGridView.ChildRows.Count)
                        rowindex = staffALLGridView.ChildRows.Count - 1;
                    if (rowindex < 0)
                        rowindex = 0;

                    staffALLGridView.ChildRows[rowindex].IsCurrent = true;
                }


            }


        }

        private void staffALLGridView_SizeChanged(object sender, EventArgs e)
        {
            if (staffALLGridView.Width > 590)
                this.staffALLGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            else
            {
                this.staffALLGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                staffALLGridView.Columns[0].Width = 26;
                if (staffALLGridView.ColumnCount > 1)
                {
                    staffALLGridView.Columns["FIO"].Width = 250;
                    staffALLGridView.Columns["SNILS"].Width = 100;
                    staffALLGridView.Columns["INN"].Width = 80;
                    staffALLGridView.Columns["TabelNumber"].Width = 80;
                    staffALLGridView.Columns["Dismissed"].Width = 50;
                }
            }
        }

        private void staffSELECTEDGridView_SizeChanged(object sender, EventArgs e)
        {
            if (staffSELECTEDGridView.Width > 460)
                this.staffSELECTEDGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            else
            {
                this.staffSELECTEDGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                staffSELECTEDGridView.Columns[0].Width = 26;
                if (staffSELECTEDGridView.ColumnCount > 1)
                {
                    staffSELECTEDGridView.Columns["FIO"].Width = 250;
                    staffSELECTEDGridView.Columns["SNILS"].Width = 100;
                    staffSELECTEDGridView.Columns["INN"].Width = 80;
                }
            }
        }


        private void staffSELECTEDGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (staffSELECTEDGridView.ChildRows.Count > 0 && staffSELECTEDGridView.CurrentRow.Cells[1].Value != null)
            {
                long id = (long)staffSELECTEDGridView.CurrentRow.Cells[1].Value;

                if (staffSelectedIDList.Contains(id))
                {
                    staffSelectedIDList.Remove(id);
                    staffSELECTEDGrid_upd();
                }
            }
        }

        private void staffALLGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (staffALLGridView.ChildRows.Count > 0 && staffALLGridView.CurrentRow.Cells[1].Value != null)
            {
                long id = (long)staffALLGridView.CurrentRow.Cells[1].Value;

                if (!staffSelectedIDList.Contains(id))
                {
                    staffSelectedIDList.Add(id);
                    staffSELECTEDGrid_upd();
                }
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (staffALLGridView.ChildRows.Count > 0)
            {
                List<long> id = new List<long>();
                if (staffALLGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {
                    foreach (var item in staffALLGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                    {
                        id.Add(long.Parse(item.Cells[1].Value.ToString()));
                    }
                }
                else if (staffALLGridView.CurrentRow.Cells[1].Value != null)
                {
                    id.Add(long.Parse(staffALLGridView.CurrentRow.Cells[1].Value.ToString()));
                }

                if (id.Count > 0)
                {
                    staffSelectedIDList = staffSelectedIDList.Concat(id).ToList();
                    staffSelectedIDList = staffSelectedIDList.Distinct().ToList();
                    staffSELECTEDGrid_upd();
                }
            }

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (staffSELECTEDGridView.ChildRows.Count > 0)
            {
                List<long> id = new List<long>();
                if (staffSELECTEDGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {
                    foreach (var item in staffSELECTEDGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                    {
                        id.Add(long.Parse(item.Cells[1].Value.ToString()));
                    }
                }
                else if (staffSELECTEDGridView.CurrentRow.Cells[1].Value != null)
                {
                    id.Add(long.Parse(staffSELECTEDGridView.CurrentRow.Cells[1].Value.ToString()));
                }

                if (id.Count > 0)
                {
                    staffSelectedIDList = staffSelectedIDList.Except(id).ToList();
                    staffSELECTEDGrid_upd();
                }
            }
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            if (staffALLGridView.ChildRows.Count > 0)
            {
                List<long> id = staffALLGridView.ChildRows.Where(x => x.Cells[1] != null).Select(x => (long)x.Cells[1].Value).ToList();

                if (id.Count > 0)
                {
                    staffSelectedIDList = staffSelectedIDList.Concat(id).ToList();
                    staffSelectedIDList = staffSelectedIDList.Distinct().ToList();
                    staffSELECTEDGrid_upd();
                }
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (staffSELECTEDGridView.ChildRows.Count > 0)
            {
                List<long> id = staffSELECTEDGridView.ChildRows.Where(x => x.Cells[1] != null).Select(x => (long)x.Cells[1].Value).ToList();

                if (id.Count > 0)
                {
                    staffSelectedIDList = staffSelectedIDList.Except(id).ToList();
                    staffSelectedIDList = staffSelectedIDList.Distinct().ToList();
                    staffSELECTEDGrid_upd();
                }
            }

        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            foreach (var item in staffALLGridView.Rows.Where(x => x.Cells[1].Value != null))
            {
                if (item.Cells[0].Value != null)
                    item.Cells[0].Value = !(bool)item.Cells[0].Value;
                else
                    item.Cells[0].Value = true;
            }
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            foreach (var item in staffSELECTEDGridView.Rows.Where(x => x.Cells[1].Value != null))
            {
                if (item.Cells[0].Value != null)
                    item.Cells[0].Value = !(bool)item.Cells[0].Value;
                else
                    item.Cells[0].Value = true;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            Updated = false;
            if (staffSelectedIDList.Count > 0 && SZVMData != null)
            {
                var szvmStaffList_fromDB = db.FormsSZV_M_2016_Staff.Where(x => x.FormsSZV_M_2016_ID == SZVMData.ID);

                var list_for_del = szvmStaffList_fromDB.Where(x => !staffSelectedIDList.Contains(x.StaffID)).ToList();

                if (list_for_del.Count() > 0) // если есть записи на удаление из базы, т.е. в текущем списке этих сотрудников удалили. 
                {
                    string list = String.Join(",", list_for_del.Select(x => x.ID).ToArray());
                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_M_2016_Staff WHERE ([ID] IN ({0}))", list));
                        Updated = true;

                    }
                    catch (Exception ex)
                    {
                        Methods.showAlert("Внимание!", "Ошибка при обновлении списка сотрудников Формы СЗВ-М! Код ошибки: " + ex.Message, this.ThemeName);
                        Updated = false;
                        return;
                    }

                    var t = list_for_del.Select(c => c.ID);
                    szvmStaffList_fromDB = szvmStaffList_fromDB.Where(x => !t.Contains(x.ID));
                }

                var b = szvmStaffList_fromDB.Select(c => c.StaffID).ToList();
                staffSelectedIDList = staffSelectedIDList.Except(b).ToList();

                foreach (var item in staffSelectedIDList)
                {
                    db.AddToFormsSZV_M_2016_Staff(new FormsSZV_M_2016_Staff { StaffID = item, FormsSZV_M_2016_ID = SZVMData.ID });
                }

                try
                {
                    if (staffSelectedIDList.Count > 0)
                    {
                        db.SaveChanges();
                        Updated = true;
                    }

                }
                catch (Exception ex)
                {
                    Methods.showAlert("Внимание!", "Ошибка при сохранении Формы СЗВ-М! Код ошибки: " + ex.Message, this.ThemeName);
                    Updated = false;
                    return;
                }
            }
            else if (SZVMData.FormsSZV_M_2016_Staff.Any())
            {
                try
                {
                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_M_2016_Staff WHERE ([FormsSZV_M_2016_ID] = {0})", SZVMData.ID));
                    Updated = true;
                }
                catch (Exception ex)
                {
                    Methods.showAlert("Внимание!", "Во время удаления данных из формы СЗВ-М произошла ошибка! Код ошибки: " + ex.Message, this.ThemeName);
                    Updated = false;
                    return;
                }

            }

            this.Close();
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            if (SZVMData != null && staffALLGridView.ChildRows.Count > 0)
            {
                List<Staff> staffL = db.Staff.Where(x => x.InsurerID == Options.InsID && !staffSelectedIDList.Contains(x.ID)).ToList();
                                var ids = staffL.Select(x => x.ID).ToList();
                int daysInMonth = DateTime.DaysInMonth(SZVMData.YEAR, SZVMData.MONTH);

                DateTime dateBegin = new DateTime(SZVMData.YEAR, SZVMData.MONTH, 1);
                DateTime dateEnd = new DateTime(SZVMData.YEAR, SZVMData.MONTH, daysInMonth);

                //var workStaff = db.StaffDateWork.Where(x => ids.Contains(x.StaffID)).ToList();

                //workStaff = workStaff.Where(c => (c.DateBeginWork.HasValue && !c.DateEndWork.HasValue && c.DateBeginWork.Value <= dateEnd) || (c.DateBeginWork.HasValue && c.DateEndWork.HasValue && c.DateBeginWork.Value <= dateEnd && c.DateEndWork.Value >= dateBegin)).ToList();


                staffL = staffL.Where(x => x.StaffDateWork.Any(c => (c.DateBeginWork.HasValue && !c.DateEndWork.HasValue && c.DateBeginWork.Value <= dateEnd) || (c.DateBeginWork.HasValue && c.DateEndWork.HasValue && c.DateBeginWork.Value <= dateEnd && c.DateEndWork.Value >= dateBegin))).ToList();

                ids = staffL.Select(x => x.ID).ToList();

                foreach (var item in staffALLGridView.Rows.Where(x => x.Cells[1].Value != null))
                {
                    item.Cells[0].Value = false;
                }

                foreach (var item in staffALLGridView.Rows.Where(x => x.Cells[1].Value != null && ids.Contains((long)x.Cells[1].Value)))
                {
                     item.Cells[0].Value = true;
                }


            }



        }

        private void staffALLGridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            RadMenuItem menuItem = new RadMenuItem("Добавить сотрудника в форму СЗВ-М");
            menuItem.Click += new EventHandler(radButton2_Click);
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            e.ContextMenu.Items.Insert(0, menuItem);
            e.ContextMenu.Items.Insert(1, separator);
        }

        private void staffSELECTEDGridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            RadMenuItem menuItem = new RadMenuItem("Убрать сотрудника из формы СЗВ-М");
            menuItem.Click += new EventHandler(radButton1_Click);
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            e.ContextMenu.Items.Insert(0, menuItem);
            e.ContextMenu.Items.Insert(1, separator);

        }

        private void staffALLGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                radButton2_Click(null, null);
            }
            else if (e.KeyChar == 8)
            {
                if (this.staffALLGridView.MasterView.TableFilteringRow.Cells["FIO"].Value != null && !String.IsNullOrEmpty(this.staffALLGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString()))
                {
                    this.staffALLGridView.MasterView.TableFilteringRow.Cells["FIO"].Value = this.staffALLGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString().Remove(this.staffALLGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString().Length - 1);
                }
            }
            else
            {
                this.staffALLGridView.MasterView.TableFilteringRow.Cells["FIO"].Value = (this.staffALLGridView.MasterView.TableFilteringRow.Cells["FIO"].Value != null ? this.staffALLGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString() : "") + e.KeyChar;
            }
        }

        private void staffALLGridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if ((e.GridViewTemplate.MasterTemplate.CurrentRow == null || e.GridViewTemplate.MasterTemplate.CurrentRow.Index < 0) && e.GridViewTemplate.ChildRows.Count > 0 && !staffALLGridView.MasterView.TableFilteringRow.IsCurrent)
            {
                e.GridViewTemplate.ChildRows.First().IsCurrent = true;
            }
        }

        private void staffSELECTEDGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                radButton1_Click(null, null);
            }
            else if (e.KeyChar == 8)
            {
                if (this.staffSELECTEDGridView.MasterView.TableFilteringRow.Cells["FIO"].Value != null && !String.IsNullOrEmpty(this.staffSELECTEDGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString()))
                {
                    this.staffSELECTEDGridView.MasterView.TableFilteringRow.Cells["FIO"].Value = this.staffSELECTEDGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString().Remove(this.staffSELECTEDGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString().Length - 1);
                }
            }
            else
            {
                this.staffSELECTEDGridView.MasterView.TableFilteringRow.Cells["FIO"].Value = (this.staffSELECTEDGridView.MasterView.TableFilteringRow.Cells["FIO"].Value != null ? this.staffSELECTEDGridView.MasterView.TableFilteringRow.Cells["FIO"].Value.ToString() : "") + e.KeyChar;
            }
        }


    }
}
