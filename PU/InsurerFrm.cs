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
using PU.Reports;
using Telerik.WinControls.Data;
using System.Threading.Tasks;
using PU.Dictionaries;

namespace PU
{
    public partial class InsurerFrm : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public long InsID { get; set; }
        public string action { get; set; }
        string currentReg { get; set; }

        public InsurerFrm()
        {
            InitializeComponent();

            //            db.Connection.ConnectionString = @"metadata=res://*/Models.Model.csdl|res://*/Models.Model.ssdl|res://*/Models.Model.msl;provider=System.Data.SQLite;provider connection string='C:\Documents and Settings\990812\Мои документы\Visual Studio 2010\Projects\PU\PU\App_Data\_pu6.db3;'";

            //       StringBuilder Sb = new StringBuilder();
            //       Sb.Append(@"metadata=res://*/Models.Model.csdl|res://*/Models.Model.ssdl|res://*/Models.Model.msl;provider=System.Data.SQLite;");
            //       Sb.Append("provider connection string='data source=C:\\pu6__.db3;'");
            //       db.Connection.ConnectionString = Sb.ToString();

            SelfRef = this;
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    addBtn.Enabled = false;
                    delBtn.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    //this.Dispose();
                    break;
            }

        }

        public static InsurerFrm SelfRef
        {
            get;
            set;
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

        public class insListTemp
        {
            public long ID { get; set; }
            public bool CheckBox { get; set; }
            public string Name { get; set; }
            public string RegNum { get; set; }
            public string INN { get; set; }
            public string DolgnFIO { get; set; }
            public string Dolgn { get; set; }
        }

        public void dataGrid_upd()
        {

            long insID = 0;

            if (insurerGrid.RowCount > 0)
            {
                insID = Convert.ToInt64(insurerGrid.CurrentRow.Cells[0].Value);
            }


            int rowindex = 0;
            string currId = "";
            if (insurerGrid.ChildRows.Count > 0 && insurerGrid.CurrentRow.Cells[0].Value != null && insurerGrid.CurrentRow.Index >= 0)
            {
                rowindex = insurerGrid.CurrentRow.Index;
                currId = insurerGrid.CurrentRow.Cells[0].Value.ToString();
            }


            SortDescriptor descriptor = new SortDescriptor();

            if (insurerGrid.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = insurerGrid.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "Name";
                descriptor.Direction = ListSortDirection.Ascending;
            }



            var insList = db.Insurer.ToList();
            this.insurerGrid.TableElement.BeginUpdate();


            List<insListTemp> insL = new List<insListTemp>();


            foreach (var item in insList)
            {
                insL.Add(new insListTemp
                {
                    ID = item.ID,
                    CheckBox = false,
                    Name = item.TypePayer == 0 ? (item.NameShort != null ? item.NameShort : (item.Name != null ? item.Name : "")) : (item.LastName + " " + item.FirstName + " " + (item.MiddleName != null ? item.MiddleName : "")),
                    RegNum = !String.IsNullOrEmpty(item.RegNum) ? Utils.ParseRegNum(item.RegNum) : "",
                    INN = item.INN != null ? item.INN : "",
                    Dolgn = item.BossDolgn != null ? item.BossDolgn : "",
                    DolgnFIO = item.BossFIO != null ? item.BossFIO : ""
                });

            }


            insurerGrid.DataSource = insL.OrderBy(x => x.Name);

            if (descriptor != null)
            {
                this.insurerGrid.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (insL.Count > 0)
            {
                insurerGrid.Columns["ID"].IsVisible = false;
                insurerGrid.Columns["CheckBox"].HeaderText = "";
                insurerGrid.Columns["CheckBox"].TextAlignment = ContentAlignment.MiddleCenter;
                (insurerGrid.Columns["CheckBox"] as GridViewCheckBoxColumn).EnableHeaderCheckBox = true;
                (insurerGrid.Columns["CheckBox"] as GridViewCheckBoxColumn).Checked = Telerik.WinControls.Enumerations.ToggleState.Indeterminate;
                insurerGrid.Columns["Name"].HeaderText = "Наименование";
                insurerGrid.Columns["RegNum"].HeaderText = "Рег.№ в ПФР";
                insurerGrid.Columns["RegNum"].TextAlignment = ContentAlignment.MiddleCenter;
                insurerGrid.Columns["INN"].HeaderText = "ИНН";
                insurerGrid.Columns["Dolgn"].HeaderText = "Должность руководителя";
                insurerGrid.Columns["DolgnFIO"].HeaderText = "ФИО руководителя";

                foreach (GridViewDataColumn column in this.insurerGrid.Columns)
                {
                    column.ReadOnly = (column.Name != "CheckBox");
                }
            }


            foreach (var item in insurerGrid.Rows)
            {
                item.MinHeight = 22;
            }

            this.insurerGrid.TableElement.EndUpdate();

            if (insurerGrid.ChildRows.Count > 0)
            {
                if (insurerGrid.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    insurerGrid.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= insurerGrid.ChildRows.Count)
                        rowindex = insurerGrid.ChildRows.Count - 1;
                    rowindex = rowindex >= 0 ? rowindex : 0;
                    insurerGrid.ChildRows[rowindex].IsCurrent = true;
                }

            }

            //insurerGrid.Refresh();

            //if (insurerGrid.RowCount > 0)
            //{
            //    if (!String.IsNullOrEmpty(currentReg) && (insurerGrid.Rows.Any(x => x.Cells["RegNum"].Value.ToString() == currentReg)))
            //    {
            //        insurerGrid.TableElement.ScrollToRow(insurerGrid.Rows.First(x => x.Cells["RegNum"].Value.ToString() == currentReg));
            //        insurerGrid.Rows.First(x => x.Cells["RegNum"].Value.ToString() == currentReg).IsCurrent = true;
            //    }
            //    else
            //    {
            //        insurerGrid.TableElement.ScrollToRow(insurerGrid.Rows[0]);
            //        this.insurerGrid.GridNavigator.SelectFirstRow();
            //    }

            //}


        }

        private void Insurer_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            checkAccessLevel();


            if (action == "selection")
            {
                selectBtn.Visible = true;
            }


            foreach (GridViewDataColumn column in this.insurerGrid.Columns)
            {
                column.ReadOnly = (column.Name != "CheckBox");
            }

            dataGrid_upd();
            insurerGrid_SizeChanged(null, null);


            string id = Options.InsID.ToString();
            if (insurerGrid.Rows.Any(x => x.Cells["ID"].Value.ToString() == id))
                insurerGrid.Rows.First(x => x.Cells["ID"].Value.ToString() == id).IsCurrent = true;


            this.insurerGrid.Select();
        }



        //public string add(Insurer insData)
        //{
        //    string result = "";

        //    int rowindex = insurerGrid.RowCount == 0 ? 0 : insurerGrid.CurrentRow.Index;



        //    return result;
        //}
        //
        //public string edit(Insurer insData)
        //{
        //    string result = "";

        //    int rowindex = insurerGrid.CurrentRow.Index;
        //    long id = long.Parse(insurerGrid.CurrentRow.Cells[0].Value.ToString());

        //    if (!db.Insurer.Any(x => x.RegNum == insData.RegNum && x.ID != id))
        //    {
        //        try
        //        {
        //            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == id);

        //            ins.BossDolgn = insData.BossDolgn;
        //            ins.BossDolgnDop = insData.BossDolgnDop;
        //            ins.BossPrint = insData.BossPrint;
        //            ins.BossDopPrint = insData.BossDopPrint;
        //            ins.BossFIO = insData.BossFIO;
        //            ins.BossFIODop = insData.BossFIODop;
        //            ins.BuchgFIO = insData.BuchgFIO;
        //            ins.BuchgPrint = insData.BuchgPrint;
        //            ins.CodeRaion = insData.CodeRaion;
        //            ins.CodeRegion = insData.CodeRegion;
        //            ins.ControlNumber = insData.ControlNumber;
        //            ins.EGRIP = insData.EGRIP;
        //            ins.EGRUL = insData.EGRUL;
        //            ins.FirstName = insData.FirstName;
        //            ins.INN = insData.INN;
        //            ins.InsuranceNumber = insData.InsuranceNumber;
        //            ins.KPP = insData.KPP;
        //            ins.LastName = insData.LastName;
        //            ins.MiddleName = insData.MiddleName;
        //            ins.Name = insData.Name;
        //            ins.NameShort = insData.NameShort;
        //            ins.OKPO = insData.OKPO;
        //            ins.OKTMO = insData.OKTMO;
        //            ins.OKWED = insData.OKWED;
        //            ins.OrgLegalForm = insData.OrgLegalForm;
        //            ins.PerformerDolgn = insData.PerformerDolgn;
        //            ins.PerformerFIO = insData.PerformerFIO;
        //            ins.PerformerPrint = insData.PerformerPrint;
        //            ins.PhoneContact = insData.PhoneContact;
        //            ins.RegNum = insData.RegNum;
        //            ins.TypePayer = insData.TypePayer;
        //            ins.YearBirth = insData.YearBirth;


        //            db.Entry(ins, EntityState.Modified);
        //            db.SaveChanges();
        //            dataGrid_upd();
        //            insurerGrid.Rows[rowindex].IsCurrent = true;
        //        }
        //        catch (Exception e)
        //        {
        //            result = e.Message;
        //        }
        //    }
        //    else
        //        result = "Страхователь с рег. номером " + insData.RegNum + " уже существует в БД!";


        //    return result;
        //}



        private void InsurerFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            selectBtn.Visible = false;
        }


        private void InsurerFrm_Shown(object sender, EventArgs e)
        {
            insurerGrid.Select();
        }


        private void addBtn_Click(object sender, EventArgs e)
        {
            if (!addBtn.Enabled)
                return;

            InsurerEdit child = new InsurerEdit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.ShowDialog();

            if (child.insData != null)
            {
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                dataGrid_upd();
                string id = child.insData.ID.ToString();
                if (insurerGrid.Rows.Any(x => x.Cells["ID"].Value.ToString() == id))
                    insurerGrid.Rows.First(x => x.Cells["ID"].Value.ToString() == id).IsCurrent = true;
            }
            child.Dispose();
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (insurerGrid.Rows.Count() > 0 && insurerGrid.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(insurerGrid.CurrentRow.Cells[0].Value.ToString());

                InsurerEdit child = new InsurerEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.InsID = id;
                child.action = "edit";
                child.ShowDialog();
                if (child.insData != null)
                {
                    db.ChangeTracker.DetectChanges();
                    db = new pu6Entities();
                    dataGrid_upd();
                }
                child.Dispose();
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (!delBtn.Enabled)
                return;


            DialogResult dialogResult;
            if (insurerGrid.Rows.Any(x => x.Cells[1].Value != null && (bool)x.Cells[1].Value == true)) // если выделено несколько записей на удаление
            {
                int cnt = insurerGrid.Rows.Where(x => x.Cells[1].Value != null && (bool)x.Cells[1].Value == true).Count();
                dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить " + cnt.ToString() + " записи(ей)", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
            }
            else
            {
                dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить данного Страхователя?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
            }

            if (dialogResult == DialogResult.Yes)
            {
                if (insurerGrid.RowCount > 0 && insurerGrid.CurrentRow.Cells[0].Value != null)
                {
                    int rowindex = insurerGrid.CurrentRow.Index;
                    List<long> id = new List<long>();

                    //если несколько записей
                    if (insurerGrid.Rows.Any(x => x.Cells[1].Value != null && (bool)x.Cells[1].Value == true))
                    {
                        foreach (var item in insurerGrid.Rows.Where(x => x.Cells[1].Value != null && (bool)x.Cells[1].Value == true))
                        {
                            id.Add(long.Parse(item.Cells[0].Value.ToString()));
                        }
                    }
                    else if (rowindex >= 0)// если текущая запись
                    {
                        id.Add(long.Parse(insurerGrid.CurrentRow.Cells[0].Value.ToString()));

                    }

                    this.Cursor = Cursors.WaitCursor;
                    string result = Methods.DeleteInsurer(id);
                    this.Cursor = Cursors.Default;

                    if (!String.IsNullOrEmpty(result))
                    {
                        Messenger.showAlert(AlertType.Error, "Внимание!", "При удалении данных произошла ошибка. Код исключения: " + result, this.ThemeName, 200);
                        InsID = 0;
                    }


                    dataGrid_upd();
                    InsID = 0;
                    if (rowindex < insurerGrid.Rows.Count())
                    {
                        insurerGrid.Rows[rowindex].IsCurrent = true;
                    }
                    else if (insurerGrid.Rows.Count() > 0)
                        insurerGrid.Rows.Last().IsCurrent = true;

                }
            }
        }


        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            if (insurerGrid.Rows.Count() > 0 && insurerGrid.CurrentRow != null && insurerGrid.CurrentRow.Cells[0].Value != null)
            {

                if (action == "selection")
                {
                    InsID = long.Parse(insurerGrid.CurrentRow.Cells[0].Value.ToString());

                    var insData = db.Insurer.FirstOrDefault(x => x.ID == InsID);

                    if (Options.InsurerFolders.Any(x => x.regnum == insData.RegNum))
                    {
                        var p = Options.InsurerFolders.FirstOrDefault(x => x.regnum == insData.RegNum);

                        Options.CurrentInsurerFolders.regnum = p.regnum;
                        Options.CurrentInsurerFolders.importPath = p.importPath;
                        Options.CurrentInsurerFolders.exportPath = p.exportPath;
                    }


                    this.Close();
                }
            }
        }

        private void staffBtn_Click(object sender, EventArgs e)
        {
            if (insurerGrid.Rows.Count() > 0 && insurerGrid.CurrentRow.Cells[0].Value != null)
            {

                long id = long.Parse(insurerGrid.CurrentRow.Cells[0].Value.ToString());

                StaffFrm child = new StaffFrm();
                child.Text = "Сотрудники    -    " + insurerGrid.CurrentRow.Cells[2].Value.ToString();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.showAllStaff = Control.ModifierKeys == Keys.Shift;
                child.InsID = id;
                child.ShowDialog();
                child.Dispose();
            }
        }

        private void departmentBtn_Click(object sender, EventArgs e)
        {
            if (insurerGrid.Rows.Count() > 0 && insurerGrid.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(insurerGrid.CurrentRow.Cells[0].Value.ToString());

                DepartmentsFrm child = new DepartmentsFrm();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.InsID = id;
                child.ShowDialog();
                child.Dispose();
            }
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (insurerGrid.RowCount > 0)
            {
                List<InsurerRep> insList = new List<InsurerRep>();

                var ins = db.Insurer;

                if (ins.Count() != 0)
                {
                    int i = 1;
                    foreach (var item in ins)
                    {
                        insList.Add(new InsurerRep
                        {
                            Num = i,
                            RegNum = !String.IsNullOrEmpty(item.RegNum) ? Utils.ParseRegNum(item.RegNum) : "",
                            Name = item.TypePayer == 0 ? (item.NameShort != null ? item.NameShort : (item.Name != null ? item.Name : "")) : (item.LastName + " " + item.FirstName + " " + (item.MiddleName != null ? item.MiddleName : "")),
                            INN = item.INN != null ? item.INN : "",
                            KPP = item.KPP != null ? item.KPP : ""
                        });

                        i++;
                    }
                    ReportMethods.PrintInsurer(insList, this.ThemeName);
                }
            }
        }

        private void insurerGrid_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(addBtn_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(editBtn_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(delBtn_Click);
                RadMenuItem menuItem4 = new RadMenuItem("Отделы");
                menuItem4.Click += new EventHandler(departmentBtn_Click);
                RadMenuItem menuItem5 = new RadMenuItem("Сотрудники");
                menuItem5.Click += new EventHandler(staffBtn_Click);
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

        private void insurerGrid_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (insurerGrid.Rows.Count() > 0)
            {
                switch (action)
                {
                    case "selection":
                        selectBtn_Click(null, null);
                        break;
                    default:
                        editBtn_Click(null, null);
                        break;
                }
            }
        }

        private void insurerGrid_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if ((e.GridViewTemplate.MasterTemplate.CurrentRow == null || e.GridViewTemplate.MasterTemplate.CurrentRow.Index < 0) && e.GridViewTemplate.ChildRows.Count > 0 && !insurerGrid.MasterView.TableFilteringRow.IsCurrent)
            {
                e.GridViewTemplate.ChildRows.First().IsCurrent = true;
            }
        }

        private void insurerGrid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                switch (action)
                {
                    case "selection":
                        selectBtn_Click(null, null);
                        break;
                    default:
                        editBtn_Click(null, null);
                        break;
                }
            }
            else if (e.KeyChar == 8)
            {
                if (this.insurerGrid.MasterView.TableFilteringRow.Cells["Name"].Value != null && !String.IsNullOrEmpty(this.insurerGrid.MasterView.TableFilteringRow.Cells["Name"].Value.ToString()))
                {
                    this.insurerGrid.MasterView.TableFilteringRow.Cells["Name"].Value = this.insurerGrid.MasterView.TableFilteringRow.Cells["Name"].Value.ToString().Remove(this.insurerGrid.MasterView.TableFilteringRow.Cells["Name"].Value.ToString().Length - 1);
                }
            }
            else
            {
                this.insurerGrid.MasterView.TableFilteringRow.Cells["Name"].Value = (this.insurerGrid.MasterView.TableFilteringRow.Cells["Name"].Value != null ? this.insurerGrid.MasterView.TableFilteringRow.Cells["Name"].Value.ToString() : "") + e.KeyChar;
            }
        }

        private void insurerGrid_SizeChanged(object sender, EventArgs e)
        {
            if (insurerGrid.Width >= 936)
            {
                insurerGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }
            else if (insurerGrid.ColumnCount > 0)
            {
                insurerGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                insurerGrid.Columns[1].Width = 29;
                insurerGrid.Columns[2].Width = 260;
                insurerGrid.Columns[3].Width = 150;
                insurerGrid.Columns[4].Width = 100;
                insurerGrid.Columns[5].Width = 220;
                insurerGrid.Columns[6].Width = 180;
            }
        }




    }
}
