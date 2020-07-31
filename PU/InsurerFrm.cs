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
                    radButton1.Enabled = false;
                    radButton3.Enabled = false;
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

            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            var ins = db.Insurer;
            radGridView1.Rows.Clear(); // If dgv is bound to datatable
            //            radGridView1.DataSource = null;

            List<insListTemp> insL = new List<insListTemp>();
            if (ins.Count() != 0)
            {
                foreach (var item in ins)
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
            }


            radGridView1.DataSource = insL;


            radGridView1.Columns["ID"].IsVisible = false;
            radGridView1.Columns["CheckBox"].HeaderText = "";
            radGridView1.Columns["CheckBox"].TextAlignment = ContentAlignment.MiddleCenter;
            (radGridView1.Columns["CheckBox"] as GridViewCheckBoxColumn).EnableHeaderCheckBox = true;
            (radGridView1.Columns["CheckBox"] as GridViewCheckBoxColumn).Checked = Telerik.WinControls.Enumerations.ToggleState.Indeterminate;
            radGridView1.Columns["Name"].HeaderText = "Наименование";
            radGridView1.Columns["RegNum"].HeaderText = "Рег.№ в ПФР";
            radGridView1.Columns["RegNum"].TextAlignment = ContentAlignment.MiddleCenter;
            radGridView1.Columns["INN"].HeaderText = "ИНН";
            radGridView1.Columns["Dolgn"].HeaderText = "Должность руководителя";
            radGridView1.Columns["DolgnFIO"].HeaderText = "ФИО руководителя";


            foreach (GridViewDataColumn column in this.radGridView1.Columns)
            {
                column.ReadOnly = (column.Name != "CheckBox");
            }


            radGridView1.Refresh();

            if (radGridView1.RowCount > 0)
            {
                if (!String.IsNullOrEmpty(currentReg) && (radGridView1.Rows.Any(x => x.Cells["RegNum"].Value.ToString() == currentReg)))
                {
                    radGridView1.TableElement.ScrollToRow(radGridView1.Rows.First(x => x.Cells["RegNum"].Value.ToString() == currentReg));
                    radGridView1.Rows.First(x => x.Cells["RegNum"].Value.ToString() == currentReg).IsCurrent = true;
                }
                else
                {
                    radGridView1.TableElement.ScrollToRow(radGridView1.Rows[0]);
                    this.radGridView1.GridNavigator.SelectFirstRow();
                }

            }


        }

        private void Insurer_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            checkAccessLevel();

            if (db.Insurer.Any(x => x.ID == Options.InsID))
            {
                currentReg = Utils.ParseRegNum(db.Insurer.FirstOrDefault(x => x.ID == Options.InsID).RegNum);
            }


            if (action == "selection")
            {
                radButton7.Visible = true;
            }

            /*            radGridView1.Columns["RegNum"].Width = 120;
                        radGridView1.Columns["Name"].Width = 200;
                        radGridView1.Columns["INN"].Width = 100;
                        radGridView1.Columns["Dolgn"].Width = radGridView1.Width / 10 * 3;
                        radGridView1.Columns["DolgnFIO"].Width = radGridView1.Width / 10 * 2;*/
            foreach (GridViewDataColumn column in this.radGridView1.Columns)
            {
                column.ReadOnly = (column.Name != "CheckBox");
            }

            dataGrid_upd();
            radGridView1_SizeChanged(null, null);

            this.radGridView1.Select();

        }


        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (!radButton1.Enabled)
                return;

            InsurerEdit child = new InsurerEdit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.ShowDialog();
            child.Dispose();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.Rows.Count() > 0 && radGridView1.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());
                currentReg = radGridView1.CurrentRow.Cells["RegNum"].Value.ToString();

                InsurerEdit child = new InsurerEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.insData = db.Insurer.FirstOrDefault(x => x.ID == id);
                child.action = "edit";
                child.ShowDialog();
                child.Dispose();
            }
        }

        public string add(Insurer insData)
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;

            try
            {
                if (!db.Insurer.Any(x => x.RegNum == insData.RegNum))
                {
                    db.Insurer.AddObject(insData);
                    db.SaveChanges();
                    dataGrid_upd();
                    radGridView1.Rows[rowindex].IsCurrent = true;
                }
                else
                    result = "Страхователь с рег. номером " + insData.RegNum + " уже существует в БД!";
            }
            catch (Exception e)
            {
                result = e.Message;
            }



            return result;
        }
        //
        public string edit(Insurer insData)
        {
            string result = "";

            int rowindex = radGridView1.CurrentRow.Index;
            long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());

            if (!db.Insurer.Any(x => x.RegNum == insData.RegNum && x.ID != id))
            {
                try
                {
                    Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == id);

                    ins.BossDolgn = insData.BossDolgn;
                    ins.BossDolgnDop = insData.BossDolgnDop;
                    ins.BossPrint = insData.BossPrint;
                    ins.BossDopPrint = insData.BossDopPrint;
                    ins.BossFIO = insData.BossFIO;
                    ins.BossFIODop = insData.BossFIODop;
                    ins.BuchgFIO = insData.BuchgFIO;
                    ins.BuchgPrint = insData.BuchgPrint;
                    ins.CodeRaion = insData.CodeRaion;
                    ins.CodeRegion = insData.CodeRegion;
                    ins.ControlNumber = insData.ControlNumber;
                    ins.EGRIP = insData.EGRIP;
                    ins.EGRUL = insData.EGRUL;
                    ins.FirstName = insData.FirstName;
                    ins.INN = insData.INN;
                    ins.InsuranceNumber = insData.InsuranceNumber;
                    ins.KPP = insData.KPP;
                    ins.LastName = insData.LastName;
                    ins.MiddleName = insData.MiddleName;
                    ins.Name = insData.Name;
                    ins.NameShort = insData.NameShort;
                    ins.OKPO = insData.OKPO;
                    ins.OKTMO = insData.OKTMO;
                    ins.OKWED = insData.OKWED;
                    ins.OrgLegalForm = insData.OrgLegalForm;
                    ins.PerformerDolgn = insData.PerformerDolgn;
                    ins.PerformerFIO = insData.PerformerFIO;
                    ins.PerformerPrint = insData.PerformerPrint;
                    ins.PhoneContact = insData.PhoneContact;
                    ins.RegNum = insData.RegNum;
                    ins.TypePayer = insData.TypePayer;
                    ins.YearBirth = insData.YearBirth;


                    db.ObjectStateManager.ChangeObjectState(ins, EntityState.Modified);
                    db.SaveChanges();
                    dataGrid_upd();
                    radGridView1.Rows[rowindex].IsCurrent = true;
                }
                catch (Exception e)
                {
                    result = e.Message;
                }
            }
            else
                result = "Страхователь с рег. номером " + insData.RegNum + " уже существует в БД!";


            return result;
        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            if (radGridView1.Rows.Count() > 0 && radGridView1.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());

                DepartmentsFrm child = new DepartmentsFrm();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ShowDialog();
                child.Dispose();
            }
        }


        private void radButton6_Click(object sender, EventArgs e)
        {
            if (radGridView1.Rows.Count() > 0 && radGridView1.CurrentRow.Cells[0].Value != null)
            {

                long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());

                StaffFrm child = new StaffFrm();
                child.Text = "Сотрудники    -    " + radGridView1.CurrentRow.Cells[2].Value.ToString();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.showAllStaff = Control.ModifierKeys == Keys.Shift;
                child.InsID = id;
                child.ShowDialog();
                child.Dispose();
            }
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (radGridView1.Rows.Count() > 0)
            {
                switch (action)
                {
                    case "selection":
                        radButton7_Click(null, null);
                        break;
                    default:
                        radButton2_Click(null, null);
                        break;
                }
            }
        }

        private void radButton7_Click(object sender, EventArgs e)
        {
            if (radGridView1.Rows.Count() > 0 && radGridView1.CurrentRow != null && radGridView1.CurrentRow.Cells[0].Value != null)
            {

                if (action == "selection")
                {
                    InsID = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());

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

        private void InsurerFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            radButton7.Visible = false;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (!radButton3.Enabled)
                return;


            DialogResult dialogResult;
            if (radGridView1.Rows.Any(x => x.Cells[1].Value != null && (bool)x.Cells[1].Value == true)) // если выделено несколько записей на удаление
            {
                int cnt = radGridView1.Rows.Where(x => x.Cells[1].Value != null && (bool)x.Cells[1].Value == true).Count();
                dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить " + cnt.ToString() + " записи(ей)", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
            }
            else
            {
                dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить данного Страхователя?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
            }

            if (dialogResult == DialogResult.Yes)
            {
                if (radGridView1.RowCount > 0 && radGridView1.CurrentRow.Cells[0].Value != null)
                {
                    int rowindex = radGridView1.CurrentRow.Index;
                    List<long> id = new List<long>();

                    //если несколько записей
                    if (radGridView1.Rows.Any(x => x.Cells[1].Value != null && (bool)x.Cells[1].Value == true))
                    {
                        foreach (var item in radGridView1.Rows.Where(x => x.Cells[1].Value != null && (bool)x.Cells[1].Value == true))
                        {
                            id.Add(long.Parse(item.Cells[0].Value.ToString()));
                        }
                    }
                    else if (rowindex >= 0)// если текущая запись
                    {
                        id.Add(long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString()));

                    }

                    this.Cursor = Cursors.WaitCursor;
                    string result = Methods.DeleteInsurer(id);
                    this.Cursor = Cursors.Default;

                    if (!String.IsNullOrEmpty(result))
                    {
                        Methods.showAlert("Внимание!", "При удалении данных произошла ошибка. Код исключения: " + result, this.ThemeName, 200);
                        InsID = 0;
                    }


                    dataGrid_upd();
                    InsID = 0;
                    if (rowindex < radGridView1.Rows.Count())
                    {
                        radGridView1.Rows[rowindex].IsCurrent = true;
                    }
                    else if (radGridView1.Rows.Count() > 0)
                        radGridView1.Rows.Last().IsCurrent = true;

                }
            }

        }

        private void radGridView1_SizeChanged(object sender, EventArgs e)
        {
            if (radGridView1.Width >= 936)
            {
                radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }
            else if (radGridView1.ColumnCount > 0)
            {
                radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
                radGridView1.Columns[1].Width = 29;
                radGridView1.Columns[2].Width = 260;
                radGridView1.Columns[3].Width = 150;
                radGridView1.Columns[4].Width = 100;
                radGridView1.Columns[5].Width = 220;
                radGridView1.Columns[6].Width = 180;
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount > 0)
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

        private void InsurerFrm_Shown(object sender, EventArgs e)
        {
            radGridView1.Select();
        }

        private void radGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(radButton1_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(radButton2_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(radButton3_Click);
                RadMenuItem menuItem4 = new RadMenuItem("Отделы");
                menuItem4.Click += new EventHandler(radButton5_Click);
                RadMenuItem menuItem5 = new RadMenuItem("Сотрудники");
                menuItem5.Click += new EventHandler(radButton6_Click);
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

        private void radGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                switch (action)
                {
                    case "selection":
                        radButton7_Click(null, null);
                        break;
                    default:
                        radButton2_Click(null, null);
                        break;
                }
            }
            else if (e.KeyChar == 8)
            {
                if (this.radGridView1.MasterView.TableFilteringRow.Cells["Name"].Value != null && !String.IsNullOrEmpty(this.radGridView1.MasterView.TableFilteringRow.Cells["Name"].Value.ToString()))
                {
                    this.radGridView1.MasterView.TableFilteringRow.Cells["Name"].Value = this.radGridView1.MasterView.TableFilteringRow.Cells["Name"].Value.ToString().Remove(this.radGridView1.MasterView.TableFilteringRow.Cells["Name"].Value.ToString().Length - 1);
                }
            }
            else
            {
                this.radGridView1.MasterView.TableFilteringRow.Cells["Name"].Value = (this.radGridView1.MasterView.TableFilteringRow.Cells["Name"].Value != null ? this.radGridView1.MasterView.TableFilteringRow.Cells["Name"].Value.ToString() : "") + e.KeyChar;
            }
        }

        private void radGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if ((e.GridViewTemplate.MasterTemplate.CurrentRow == null || e.GridViewTemplate.MasterTemplate.CurrentRow.Index < 0) && e.GridViewTemplate.ChildRows.Count > 0 && !radGridView1.MasterView.TableFilteringRow.IsCurrent)
            {
                e.GridViewTemplate.ChildRows.First().IsCurrent = true;
            }
        }



    }
}
