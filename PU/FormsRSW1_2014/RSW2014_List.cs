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
using Telerik.Reporting;
using Telerik.Reporting.Drawing;
using PU.Reports;


namespace PU.FormsRSW2014
{
    delegate void DelEvent();

    public partial class RSW2014_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
        RSW2014_Print ReportViewerRSW2014;
        public short yearType = 2012;
        List<StaffObject> staffList = new List<StaffObject> { };

        public RSW2014_List()
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

        public static RSW2014_List SelfRef
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

        private void RSW2014_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            xmlBtn.ButtonElement.ToolTipText = "Формирование и просмотр пачек";

            //Использование формы РСВ-1 2015
            rswAddBtn2015.Enabled = true; // Options.useRSW1_2015;
            yearType = (short)2014;
            //Options.useRSW1_2015 ? (short)2014 : (short)2012;




            inputTypeDDL.SelectedIndex = Options.inputTypeRSW1;

            this.inputTypeDDL.SelectedIndexChanged += (s, с) => inputTypeDDL_SelectedIndexChanged();
            inputTypeDDL_SelectedIndexChanged();

            HeaderChange();

            this.staffGridView.CurrentRowChanged += (s, с) => staffGridView_CurrentRowChanged();

            //            dataGrid_upd();
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


            //          this.staffGridView.CurrentRowChanged -= null;
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


            //staffGridView.DataSource = staffList.OrderBy(x => x.FIO).ToList();

//            DateTime stamp1 = DateTime.Now;

//            staffGridView.DataSource = staffList;
            //DateTime stamp2 = DateTime.Now;
            //var ss = (stamp2 - stamp1).Seconds.ToString();
            //var fff = (stamp2 - stamp1).Milliseconds.ToString();
            //var ttt = ss + "." + fff;

 //           MessageBox.Show(ttt);

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
            }
            else
            {
                indSvedGridView.Rows.Clear();
            }

        }



        public void indSvedGrid_upd()
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
            this.indSvedGridView.TableElement.BeginUpdate();
            indSvedGridView.Rows.Clear();

            if (staffID != 0)
            {
                var indSved = db.FormsRSW2014_1_Razd_6_1.Where(x => x.StaffID == staffID);

                if (indSved.Count() != 0)
                {
                    foreach (var item in indSved)
                    {
                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.indSvedGridView.MasterView);
                        rowInfo.Cells["ID"].Value = item.ID;
                        if (item.DispatchPFR.HasValue)
                            if (item.DispatchPFR.Value > 0)
                                rowInfo.Cells["PF"].Value = "";

                        rowInfo.Cells["TypeInfo"].Value = item.TypeInfo != null ? item.TypeInfo.Name : "Ошибка данных: Не найден тип сведений";
                        rowInfo.Cells["Year"].Value = item.Year.ToString();
                        string periodName = Options.RaschetPeriodInternal.Any(x => x.Year == item.Year && x.Kvartal == item.Quarter) ? (item.Quarter.ToString() + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Quarter).Name) : String.Empty;
                        rowInfo.Cells["Period"].Value = !String.IsNullOrEmpty(periodName) ? periodName.Substring(0, periodName.Length - 5) : "Период не определен";

                        string q = item.QuarterKorr.HasValue ? item.QuarterKorr.Value.ToString() : "";
                        string y = item.YearKorr.HasValue && item.YearKorr.Value > 0 ? item.YearKorr.Value.ToString() : "";
                        rowInfo.Cells["KorrPeriod"].Value = q == "" || y == "" ? "" : q + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == item.YearKorr.Value && x.Kvartal == item.QuarterKorr.Value).Name; ;
                        rowInfo.Cells["SumOPS"].Value = item.SumFeePFR.HasValue ? item.SumFeePFR.Value : 0;
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


        private void inputTypeDDL_SelectedIndexChanged()
        {
            Options.inputTypeRSW1 = (byte)inputTypeDDL.SelectedIndex;
            if (inputTypeDDL.SelectedItem.Index == 2)
                inputTypeDDL.ForeColor = System.Drawing.Color.Firebrick;
            else
                inputTypeDDL.ForeColor = System.Drawing.SystemColors.ControlText;
        }


        /// <summary>
        /// Обновление таблицы формы РСВ-1
        /// </summary>
        public void dataGrid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (radGridView1.RowCount > 0 && radGridView1.CurrentRow.Cells[0].Value != null)
            {
                rowindex = radGridView1.CurrentRow.Index;
                currId = radGridView1.CurrentRow.Cells[0].Value.ToString();
            }

            radGridView1.Rows.Clear();
            if (db.FormsRSW2014_1_1.Any(x => x.InsurerID == Options.InsID))
            {
                var rsw = db.FormsRSW2014_1_1.Where(x => x.InsurerID == Options.InsID).ToList();

                foreach (var item in rsw)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView1.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Year"].Value = item.Year;
                    rowInfo.Cells["Period"].Value = Options.RaschetPeriodInternal.Any(x => x.Year == item.Year && x.Kvartal == item.Quarter) ? (item.Quarter.ToString() + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Quarter).Name) : "Период не определен";
                    rowInfo.Cells["KorrNum"].Value = item.CorrectionNum;
                    rowInfo.Cells["OPS"].Value = item.s_110_0.HasValue ? item.s_110_0.Value : 0;
                    rowInfo.Cells["OMS"].Value = item.s_110_5.HasValue ? item.s_110_5.Value : 0;
                    rowInfo.Cells["dopTar1"].Value = item.s_110_3.HasValue ? item.s_110_3.Value : 0;
                    rowInfo.Cells["dopTar2"].Value = item.s_110_4.HasValue ? item.s_110_4.Value : 0;
                    radGridView1.Rows.Add(rowInfo);
                }

                for (var i = 0; i < radGridView1.ColumnCount; i++)
                {
                    radGridView1.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    radGridView1.Columns[i].ReadOnly = true;
                }

            }

            if (radGridView1.RowCount > 0)
            {
                if (radGridView1.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    radGridView1.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= radGridView1.RowCount)
                        rowindex = radGridView1.RowCount - 1;
                    rowindex = rowindex >= 0 ? rowindex : 0;
                    radGridView1.Rows[rowindex].IsCurrent = true;
                }
            }

            //            radGridView1.Refresh();
        }

        /// <summary>
        /// Смена заголовка при выборе страхователя
        /// </summary>
        public void HeaderChange()
        {
            radPanel1.Text = Methods.HeaderChange();

            staffGrid_upd();
            dataGrid_upd();

        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void staffAddBtn_Click(object sender, EventArgs e)
        {
            if (Options.InsID != 0)
            {
                StaffEdit child = new StaffEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.InsID = Options.InsID;
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
                db = new pu6Entities();
                staffGrid_upd();
            }
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








        private void indSvedEditBtn_Click(object sender, EventArgs e)
        {
            if (indSvedGridView.RowCount > 0 && indSvedGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);
                long id_rsw = Convert.ToInt64(indSvedGridView.CurrentRow.Cells[0].Value);
                int rowindex = indSvedGridView.CurrentRow.Index;
                var rsw_6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.ID == id_rsw);
                RSW2014_6 child = new RSW2014_6();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.staff = db.Staff.FirstOrDefault(x => x.ID == id);
                child.RSW_6 = rsw_6;
                child.period = rsw_6.Quarter;
                child.action = "edit";
                child.parentName = this.Name;
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();

                indSvedGrid_upd();
                if (rowindex >= 0)
                    indSvedGridView.Rows[rowindex].IsCurrent = true;

            }
        }

        private void indSvedAddBtn_Click(object sender, EventArgs e)
        {
            if (staffGridView.ChildRows.Count > 0 && staffGridView.CurrentRow.Cells[1].Value != null && staffGridView.CurrentRow.Index >= 0)
            {
                long id = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);

                RSW2014_6 child = new RSW2014_6();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.parentName = this.Name;
                child.staff = db.Staff.FirstOrDefault(x => x.ID == id);
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();

                indSvedGrid_upd();

            }
        }

        private void indSvedDelBtn_Click(object sender, EventArgs e)
        {
            if (indSvedGridView.RowCount != 0 && indSvedGridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись Индивидуальных сведений?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    //        int rowindex = indSvedGridView.CurrentRow.Index;
                    long id = long.Parse(indSvedGridView.CurrentRow.Cells[0].Value.ToString());

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_1 WHERE ([ID] = {0})", id));
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

        private void rswAddForm(short year)
        {

            if (Options.InsID != 0)
            {
                RSW2014_Edit child = new RSW2014_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.RSWdata = new FormsRSW2014_1_1();
                child.yearType = year;
                child.changeFormByYear();

                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                dataGrid_upd();
            }

        }

        private void rswEditBtn_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0 && radGridView1.CurrentRow.Cells[0].Value != null)
            {
                //                int rowindex = radGridView1.CurrentRow.Index;
                long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());

                db = new pu6Entities();
                if (db.FormsRSW2014_1_1.Any(x => x.ID == id))
                {
                    FormsRSW2014_1_1 rsw_data = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == id);
                    FormsRSW2014_1_1 RSWdataPrev = new FormsRSW2014_1_1();
                    if (rsw_data.Quarter != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                    {
                        short year = rsw_data.Year;
                        byte quarter = 20;
                        if (rsw_data.Quarter == 6)
                            quarter = 3;
                        else if (rsw_data.Quarter == 9)
                            quarter = 6;
                        else if (rsw_data.Quarter == 0)
                            quarter = 9;

                        if (db.FormsRSW2014_1_1.Any(x => x.Year == year && x.Quarter == quarter && x.InsurerID == Options.InsID))
                            RSWdataPrev = db.FormsRSW2014_1_1.Where(x => x.Year == year && x.Quarter == quarter && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                    }
                    else
                    {
                        RSWdataPrev = null;
                    }

                    RSW2014_Edit child = new RSW2014_Edit();
                    //                child.timer.Add(new TimerList { stamp = DateTime.Now, name = "Создание формы" });
                    child.Owner = this;
                    child.ThemeName = this.ThemeName;
                    child.ShowInTaskbar = false;
                    child.action = "edit";
                    child.yearType = ((rsw_data.Year == (short)2014) || (rsw_data.Year == (short)2015 && rsw_data.Quarter == 3)) ? (short)2014 : (short)2015;
                    child.changeFormByYear();
                    child.RSWdata = rsw_data;
                    child.RSWdataPrev = RSWdataPrev;
                    //                    child.RSW_2_1_List = db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == rsw_data.InsurerID && x.Quarter == rsw_data.Quarter && x.Year == rsw_data.Year && x.CorrectionNum == rsw_data.CorrectionNum).OrderBy(x => x.ID).ToList();
                    //child.RSW_2_4_List = db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == rsw_data.InsurerID && x.Quarter == rsw_data.Quarter && x.Year == rsw_data.Year && x.CorrectionNum == rsw_data.CorrectionNum).OrderBy(x => x.ID).ToList();
                    //child.RSW_3_4_List = db.FormsRSW2014_1_Razd_3_4.Where(x => x.InsurerID == rsw_data.InsurerID && x.Quarter == rsw_data.Quarter && x.Year == rsw_data.Year && x.CorrectionNum == rsw_data.CorrectionNum).OrderBy(x => x.ID).ToList();
                    //child.RSW_4_List = db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == rsw_data.InsurerID && x.Quarter == rsw_data.Quarter && x.Year == rsw_data.Year && x.CorrectionNum == rsw_data.CorrectionNum).OrderBy(x => x.ID).ToList();
                    //child.RSW_5_List = db.FormsRSW2014_1_Razd_5.Where(x => x.InsurerID == rsw_data.InsurerID && x.Quarter == rsw_data.Quarter && x.Year == rsw_data.Year && x.CorrectionNum == rsw_data.CorrectionNum).OrderBy(x => x.ID).ToList();
                    //child.RSW_6_1_List = db.FormsRSW2014_1_Razd_6_1.Where(x => x.InsurerID == rsw_data.InsurerID && x.Quarter == rsw_data.Quarter && x.Year == rsw_data.Year && x.CorrectionNum == rsw_data.CorrectionNum).OrderBy(x => x.ID).ToList();
                    //                child.timer.Add(new TimerList { stamp = DateTime.Now, name = "Открытие формы" });
                    child.ShowDialog();
                    child.Dispose();

                    //                GC.WaitForPendingFinalizers();
                    //                GC.Collect();
                    db.ChangeTracker.DetectChanges();
                    db = new pu6Entities();
                    dataGrid_upd();
                }
                else
                {
                    updateBtn_Click(null, null);
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования", "");
            }
        }

        private void rswDelBtn_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0 && radGridView1.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную Форму РСВ-1?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    //        int rowindex = radGridView1.CurrentRow.Index;
                    long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());

                    FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.First(x => x.ID == id);

                    bool result = Methods.DeleteRSW1(rsw);

                    if (!result)
                    {
                        RadMessageBox.Show("При удалении данных произошла ошибка");
                        return;
                    }

                    try
                    {
                        db.ChangeTracker.DetectChanges();
                        db = new pu6Entities();
                        dataGrid_upd();
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

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            rswEditBtn_Click(null, null);
        }

        private void staffGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            staffEditBtn_Click(null, null);
        }

        private void indSvedGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            indSvedEditBtn_Click(null, null);
        }

        public void updateBtn_Click(object sender, EventArgs e)
        {
            staffGrid_upd();
            dataGrid_upd();
        }

        private void printReport(byte wp)
        {
            if (radGridView1.RowCount != 0 && radGridView1.CurrentRow.Cells[0].Value != null)
            {

                long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());

                FormsRSW2014_1_1 rsw_data = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == id);
                FormsRSW2014_1_1 RSWdataPrev = new FormsRSW2014_1_1();



                ReportViewerRSW2014 = new RSW2014_Print();
                ReportViewerRSW2014.RSWdata = rsw_data;
                //ReportViewerRSW2014.RSWdataPrev = RSWdataPrev;
                if (wp == 1 || wp == 2)
                    ReportViewerRSW2014.RSW_6_1_List = db.FormsRSW2014_1_Razd_6_1.Where(x => x.Year == rsw_data.Year && x.Quarter == rsw_data.Quarter && x.InsurerID == Options.InsID).OrderBy(x => x.Staff.LastName).ToList();

                ReportViewerRSW2014.Owner = this;
                ReportViewerRSW2014.ThemeName = this.ThemeName;
                ReportViewerRSW2014.ShowInTaskbar = false;
                ReportViewerRSW2014.wp = wp;
                ReportViewerRSW2014.yearType = ((rsw_data.Year == (short)2014) || (rsw_data.Year == (short)2015 && rsw_data.Quarter == 3)) ? (short)2014 : (short)2015;

                radWaitingBar1.Visible = true;
                radWaitingBar1.StartWaiting();

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerRSW2014.createReport);
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
            radWaitingBar1.Invoke(new Action(() => { radWaitingBar1.StopWaiting(); radWaitingBar1.Visible = false; }));
            ReportViewerRSW2014.ShowDialog();

        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            printReport(0);
        }

        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            printReport(1);
        }

        private void radMenuItem3_Click(object sender, EventArgs e)
        {
            printReport(2);

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            CreateXmlPack_RSW2014 child = new CreateXmlPack_RSW2014();

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

        private void printIndSvedbtn_Click(object sender, EventArgs e)
        {
            if (indSvedGridView.RowCount != 0 && staffGridView.CurrentRow.Cells[1].Value != null)
            {

                long id = long.Parse(indSvedGridView.CurrentRow.Cells[0].Value.ToString());

                FormsRSW2014_1_Razd_6_1 rsw_1_6_data = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.ID == id);
                //FormsRSW2014_1_1 rsw_data = new FormsRSW2014_1_1();

                ReportViewerRSW2014 = new RSW2014_Print();
                // ReportViewerRSW2014.RSWdata = rsw_data;
                //ReportViewerRSW2014.RSWdataPrev = RSWdataPrev;
                ReportViewerRSW2014.RSW_6_1_List = new List<FormsRSW2014_1_Razd_6_1> { rsw_1_6_data };
                ReportViewerRSW2014.Owner = this;
                ReportViewerRSW2014.ThemeName = this.ThemeName;
                ReportViewerRSW2014.ShowInTaskbar = false;
                ReportViewerRSW2014.wp = 1;
                ReportViewerRSW2014.yearType = ((rsw_1_6_data.Year == (short)2014) || (rsw_1_6_data.Year == (short)2015 && rsw_1_6_data.Quarter == 3)) ? (short)2014 : (short)2015;


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

        private void rswAddBtn2014_Click(object sender, EventArgs e)
        {
            rswAddForm(yearType);
        }

        private void rswAddBtn2015_Click(object sender, EventArgs e)
        {
            rswAddForm(2015);
            if (radGridView1.RowCount > 0)
                radGridView1.Rows.Last().IsCurrent = true;
        }


        private void RSW2014_List_Shown(object sender, EventArgs e)
        {
            if (!Options.fixCurrentInsurer)
            {
                radButton5_Click(null, null);
            }

        }

        private void RSW2014_List_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками
            props.setFormParams(this, null);
        }

        private void radMenuItem4_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                RaschDonachSum child = new RaschDonachSum();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ShowDialog();
            }

        }

        private void radMenuItem5_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                ProverkaNalStaj child = new ProverkaNalStaj();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ShowDialog();
            }
        }

        private void radMenuItem6_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                CopyStaj child = new CopyStaj();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ShowDialog();
            }
        }

        private void radMenuItem7_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0 && indSvedGridView.Rows.Count != 0)
            {
                long id = Convert.ToInt64(staffGridView.CurrentRow.Cells[1].Value);
                long id_rsw = Convert.ToInt64(indSvedGridView.CurrentRow.Cells[0].Value);
                int rowindex = indSvedGridView.CurrentRow.Index;

                RSW2014_6_Copy child = new RSW2014_6_Copy();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ShowDialog();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();

                indSvedGrid_upd();
                if (rowindex >= 0)
                    indSvedGridView.Rows[rowindex].IsCurrent = true;
            }
        }

        private void radMenuItem8_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount > 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                long id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());

                db = new pu6Entities();
                FormsRSW2014_1_1 rsw_data = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == id);

                RSW2014_Copy child = new RSW2014_Copy();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.RSWdata = rsw_data;
                child.ShowDialog();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                dataGrid_upd();
            }
        }

        private void radMenuItem9_Click(object sender, EventArgs e)
        {
            if (Options.InsID != 0)
            {
                RSW2014_Filling child = new RSW2014_Filling();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ShowDialog();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                dataGrid_upd();
            }
        }

        private void radMenuItem10_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());
                FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == id);

                RSW2014_2_5_List child = new RSW2014_2_5_List();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ident.FormatType = "rsw2014";
                child.ident.InsurerID = rsw.InsurerID;
                child.ident.Quarter = rsw.Quarter;
                child.ident.Year = rsw.Year;
                child.rsw = rsw;
                child.ShowDialog();

            }
        }

        private void radMenuItem12_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                ProsmotrVyplat child = new ProsmotrVyplat();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "svedNachVznos";
                child.ShowDialog();
            }

        }

        private void radMenuItem13_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                ProsmotrVyplat child = new ProsmotrVyplat();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "svedBaseOPS";
                child.ShowDialog();
            }
        }

        private void radMenuItem14_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                ProsmotrVyplat child = new ProsmotrVyplat();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "svedVypl";
                child.ShowDialog();
            }
        }

        private void radMenuItem15_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                ProsmotrVyplat child = new ProsmotrVyplat();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "svedDop";
                child.ShowDialog();
            }
        }

        private void radMenuItem16_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                ProsmotrVyplat child = new ProsmotrVyplat();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "svedKorr";
                child.ShowDialog();
            }
        }

        private void radMenuItem17_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                DeleteIndSved child = new DeleteIndSved();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowDialog();

                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();

            }
        }

        private void radMenuItem18_Click(object sender, EventArgs e)
        {
            if (staffGridView.Rows.Count() != 0)
            {
                ProsmotrStaj child = new ProsmotrStaj();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowDialog();

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

        private void radPageView1_SelectedPageChanged(object sender, EventArgs e)
        {
            inputTypeDDL.Visible = radPageView1.SelectedPage == radPageView1.Pages[1];
        }

        private void radMenuItem19_Click(object sender, EventArgs e)  // Перерасчет итоговых сумм выплат
        {
            if (staffGridView.Rows.Count() != 0)
            {
                PereschetItogSum child = new PereschetItogSum();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowDialog();
            }

        }

        private void radMenuItem20_Click(object sender, EventArgs e)  // Перерасчет начисленных взносов в ПФР
        {
            if (staffGridView.Rows.Count() != 0)
            {
                PereschetNachVznos child = new PereschetNachVznos();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowDialog();

                db = new pu6Entities();
                updateBtn_Click(null, null);

            }
        }

        private void printStaffListMenuItem_Click(object sender, EventArgs e)
        {
            printStaff("staff");
        }

        private void printLgotListMenuItem_Click(object sender, EventArgs e)
        {
            printStaff("lgot");
        }


        private void printStaff(string action)
        {
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
                child.action = action;
                child.formParent = "rsw1";
                child.ShowDialog();

            }
        }

        string layouts = string.Empty;

        private void radButton1_Click_1(object sender, EventArgs e)
        {
            //string XMLFileName = "settings.xml";
            //string filePath = !String.IsNullOrEmpty(Options.settingsFilePath) ? Options.settingsFilePath : (Application.StartupPath + "\\" + XMLFileName);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                staffGridView.SaveLayout(ms);
                layouts = Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
                Props props = new Props(); //экземпляр класса с настройками

                props.setGridLayout(this.Name, staffGridView.Name, layouts);
            }

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (Options.gridParams.Any(x => x.FormName == this.Name && x.GridName == staffGridView.Name))
            {
                layouts = Options.gridParams.FirstOrDefault(x => x.FormName == this.Name && x.GridName == staffGridView.Name).GridLayout;
            }

            if (layouts != string.Empty)
            {

                System.IO.MemoryStream contentStream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(layouts));
                staffGridView.LoadLayout(contentStream);
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
                e.ContextMenu.Items.Insert(0, menuItem1);
                e.ContextMenu.Items.Insert(1, menuItem2);
                e.ContextMenu.Items.Insert(2, menuItem3);
                e.ContextMenu.Items.Insert(3, new RadMenuSeparatorItem());
                e.ContextMenu.Items.Insert(4, menuItem4);
                e.ContextMenu.Items.Insert(5, new RadMenuSeparatorItem());
                return;
            }
        }

        private void radGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            RadMenuItem menuItem1 = new RadMenuItem("Добавить");
            menuItem1.Click += new EventHandler(rswAddBtn2015_Click);
            RadMenuItem menuItem2 = new RadMenuItem("Изменить");
            menuItem2.Click += new EventHandler(rswEditBtn_Click);
            RadMenuItem menuItem3 = new RadMenuItem("Удалить");
            menuItem3.Click += new EventHandler(rswDelBtn_Click);
            RadMenuItem menuItem4 = new RadMenuItem("Печать");
            RadMenuItem menuItem5 = new RadMenuItem("РСВ-1");
            menuItem5.Click += new EventHandler(radMenuItem1_Click);
            RadMenuItem menuItem6 = new RadMenuItem("Инд.сведения");
            menuItem6.Click += new EventHandler(radMenuItem2_Click);
            RadMenuItem menuItem7 = new RadMenuItem("РСВ-1 + Инд.сведения");
            menuItem7.Click += new EventHandler(radMenuItem3_Click);
            RadMenuItem menuItem8 = new RadMenuItem("Формирование и просмотр пачек");
            menuItem8.Click += new EventHandler(radButton1_Click);

            menuItem4.Items.Add(menuItem5);
            menuItem4.Items.Add(menuItem6);
            menuItem4.Items.Add(menuItem7);

            e.ContextMenu.Items.Insert(0, menuItem1);
            e.ContextMenu.Items.Insert(1, menuItem2);
            e.ContextMenu.Items.Insert(2, menuItem3);
            e.ContextMenu.Items.Insert(3, new RadMenuSeparatorItem());
            e.ContextMenu.Items.Insert(4, menuItem4);
            e.ContextMenu.Items.Insert(5, menuItem8);
            e.ContextMenu.Items.Insert(6, new RadMenuSeparatorItem());
        }


        private void indSvedGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                indSvedEditBtn_Click(null, null);
            }
            else if (e.KeyChar == 8)
            {
                if (this.indSvedGridView.MasterView.TableFilteringRow.Cells["Year"].Value != null && !String.IsNullOrEmpty(this.indSvedGridView.MasterView.TableFilteringRow.Cells["Year"].Value.ToString()))
                {
                    this.indSvedGridView.MasterView.TableFilteringRow.Cells["Year"].Value = this.indSvedGridView.MasterView.TableFilteringRow.Cells["Year"].Value.ToString().Remove(this.indSvedGridView.MasterView.TableFilteringRow.Cells["Year"].Value.ToString().Length - 1);
                }
            }
            else
            {
                this.indSvedGridView.MasterView.TableFilteringRow.Cells["Year"].Value = (this.indSvedGridView.MasterView.TableFilteringRow.Cells["Year"].Value != null ? this.indSvedGridView.MasterView.TableFilteringRow.Cells["Year"].Value.ToString() : "") + e.KeyChar;
            }

        }

        private void indSvedGridView_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if ((e.GridViewTemplate.MasterTemplate.CurrentRow == null || e.GridViewTemplate.MasterTemplate.CurrentRow.Index < 0) && e.GridViewTemplate.ChildRows.Count > 0 && !indSvedGridView.MasterView.TableFilteringRow.IsCurrent)
            {
                e.GridViewTemplate.ChildRows.First().IsCurrent = true;
            }
        }

        private void staffGridView_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!staffGridView.MasterView.TableFilteringRow.IsCurrent)
            //{
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
            //}

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
