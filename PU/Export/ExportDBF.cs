using PU.Classes;
using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;
using System.IO;
using System.Data.OleDb;
using Telerik.WinControls.Data;

namespace PU
{

    public partial class ExportDBF : Telerik.WinControls.UI.RadForm
    {
        BackgroundWorker bw = new BackgroundWorker();
        private pu6Entities db = new pu6Entities();
        List<StaffObject> staffList = new List<StaffObject> { };
        List<long> staffIDList = new List<long> { };
        private bool cancel_work = false;
        private int cnt = 0;
        List<RaschetPeriodContainer> avail_periods_all = new List<RaschetPeriodContainer>();


        public ExportDBF()
        {
            InitializeComponent();
        }

        private void radButton2_Click(object sender, EventArgs e)
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
        private void insChangeBtn_Click(object sender, EventArgs e)
        {
            InsurerFrm child = new InsurerFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.InsID = Options.InsID;
            child.action = "selection";
            child.ShowDialog();
            Options.InsID = child.InsID;

            HeaderChange();
        }

        private void HeaderChange()
        {
            headerPanel.Text = Methods.HeaderChange();

            if (!String.IsNullOrEmpty(Options.CurrentInsurerFolders.exportPath))
            {
                pathBrowser.Value = Options.CurrentInsurerFolders.exportPath;
            }
            else if (Options.InsID != 0 && db.Insurer.Any(x => x.ID == Options.InsID))
            {
                Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

                Utils.ParseRegNum(ins.RegNum);

                pathBrowser.Value = Path.Combine(Application.StartupPath, "Экспорт", "[" + Utils.ParseRegNum(ins.RegNum) + "] - " + DateTime.Now.ToShortDateString());
            }
            else
            {
                pathBrowser.Value = Path.Combine(Application.StartupPath, "Экспорт", DateTime.Now.ToShortDateString());
            }

            odv1Grid_upd();

            staffGridUpdate();
        }


        public class ODV1GridClass
        {
            public long ID { get; set; }
            public short Year { get; set; }
            public string Code { get; set; }
            public string TypeInfo { get; set; }
            public string TypeFormS { get; set; }
            public byte TypeForm { get; set; }
            public long StaffCount { get; set; }
            public DateTime DateFilling { get; set; }
        }
        public void odv1Grid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow != null)
            {
                rowindex = odv1GridView.CurrentRow.Index;
                currId = odv1GridView.CurrentRow.Cells[0].Value.ToString();
            }

            
            SortDescriptor descriptor = new SortDescriptor();
            if (odv1GridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = odv1GridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "YEAR";
                descriptor.Direction = ListSortDirection.Ascending;
            }

            var odv1List_t = db.FormsODV_1_2017.Where(x => x.InsurerID == Options.InsID && x.TypeForm == 1).ToList();

            List<ODV1GridClass> odv1List = new List<ODV1GridClass> { };

            if (odv1List_t.Count() != 0)
            {
                int i = 0;
                foreach (var item in odv1List_t)
                {
                    i++;

                    string tInfo = "";

                    if (item.TypeInfo.HasValue)
                    {
                        switch (item.TypeInfo.Value)
                        {
                            case 0:
                                tInfo = "Исходная";
                                break;
                            case 1:
                                tInfo = "Корректирующая";
                                break;
                            case 2:
                                tInfo = "Отменяющая";
                                break;
                        }
                    }

                    string tForm = "";

                    if (item.TypeForm.HasValue)
                    {
                        switch (item.TypeForm.Value)
                        {
                            case 1:
                                tForm = "СЗВ-СТАЖ";
                                break;
                            case 2:
                                tForm = "СЗВ-ИСХ";
                                break;
                            case 3:
                                tForm = "СЗВ-КОРР";
                                break;
                            case 4:
                                tForm = "ОДВ1";
                                break;
                        }
                    }

                    odv1List.Add(new ODV1GridClass()
                    {
                        ID = item.ID,
                        Year = item.Year,
                        Code = item.Code.ToString(),
                        TypeInfo = tInfo,
                        TypeFormS = tForm,
                        TypeForm = item.TypeForm.HasValue ? item.TypeForm.Value : (byte)0,
                        StaffCount = item.StaffCount.HasValue ? item.StaffCount.Value : 0,
                        DateFilling = item.DateFilling
                    });

                }
            }

            this.odv1GridView.TableElement.BeginUpdate();
            odv1GridView.DataSource = odv1List.OrderBy(x => x.Year);

            if (descriptor != null)
            {
                this.odv1GridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (odv1List.Count > 0)
            {
                odv1GridView.Columns["ID"].IsVisible = false;
                odv1GridView.Columns["Year"].Width = 80;
                odv1GridView.Columns["Year"].HeaderText = "Год";
                odv1GridView.Columns["Code"].Width = 120;
                odv1GridView.Columns["Code"].HeaderText = "Отчетный период";
                odv1GridView.Columns["TypeInfo"].Width = 100;
                odv1GridView.Columns["TypeInfo"].HeaderText = "Тип сведений";
                odv1GridView.Columns["TypeForm"].IsVisible = false;
                odv1GridView.Columns["TypeFormS"].Width = 100;
                odv1GridView.Columns["TypeFormS"].HeaderText = "Тип документов";
                odv1GridView.Columns["StaffCount"].Width = 100;
                odv1GridView.Columns["StaffCount"].HeaderText = "Количество сотрудников";
                odv1GridView.Columns["DateFilling"].Width = 80;
                odv1GridView.Columns["DateFilling"].HeaderText = "Дата заполнения";
                odv1GridView.Columns["DateFilling"].FormatString = "{0:dd.MM.yyyy}";

                for (var i = 0; i < odv1GridView.Columns.Count; i++)
                {
                    odv1GridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    odv1GridView.Columns[i].ReadOnly = true;
                }
                //odv1GridView.Columns["NUMBERPAYMENT"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;

                this.odv1GridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }

            foreach (var item in odv1GridView.Rows)
            {
                item.MinHeight = 22;
            }

            //    this.odv1GridView.AutoSizeRows = true;

            this.odv1GridView.TableElement.EndUpdate();

            //            odv1GridView.Refresh();


            if (odv1GridView.ChildRows.Count > 0)
            {
                if (odv1GridView.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    odv1GridView.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= odv1GridView.ChildRows.Count)
                        rowindex = odv1GridView.ChildRows.Count - 1;
                    rowindex = rowindex >= 0 ? rowindex : 0;
                    odv1GridView.ChildRows[rowindex].IsCurrent = true;
                }

            }
        }




        private void ExportDBF_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);


            var avail_periods_ = Options.RaschetPeriodInternal.Where(x => x.Year <= 2018).OrderBy(x => x.Year);
            foreach (var item in avail_periods_)
            {
                avail_periods_all.Add(item);
            }
            avail_periods_ = Options.RaschetPeriodInternal2017.OrderBy(x => x.Year);
            foreach (var item in avail_periods_)
            {
                avail_periods_all.Add(item);
            }
            avail_periods_ = Options.RaschetPeriodInternal2010_2013.OrderBy(x => x.Year);
            foreach (var item in avail_periods_)
            {
                avail_periods_all.Add(item);
            }
            avail_periods_ = Options.RaschetPeriodInternal1996_2009.OrderBy(x => x.Year);
            foreach (var item in avail_periods_)
            {
                avail_periods_all.Add(item);
            }


            // выпад список "календарный год"

            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);

            this.Year.Items.Clear();

            foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }

            if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                Year.Items.OrderByDescending(x => x.Value).First().Selected = true;

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                if (Quarter.Items.Count() > 0)
                {
                    DateTime dt = DateTime.Now;
                    RaschetPeriodContainer rp = new RaschetPeriodContainer();

                    if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                    else
                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                    if (rp != null && Quarter.Items.Any(x => x.Value.ToString() == rp.Kvartal.ToString()))
                    {
                        Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                    }
                    else
                    {
                        Quarter.Items.First().Selected = true;
                    }
                }

            }

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            indSved2014PeriodsGridView.CommandCellClick += new CommandCellClickEventHandler(indSved2014PeriodsGridView_CommandCellClick);

            ilospfrCheckBox_ToggleStateChanged(null, null);
            indSved2014CheckBox_ToggleStateChanged(null, null);

            HeaderChange();


            codepageDropDownList.Items.First().Selected = true;

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    foreach (var item in param.windowData)
                    {
                        switch (item.control)
                        {
                            case "codepageDropDownList":
                                codepageDropDownList.Items.FirstOrDefault(x => x.Tag.ToString() == item.value.ToString()).Selected = true;
                                break;
                            case "pathBrowser":
                                pathBrowser.Value = item.value;
                                break;
                        }
                    }
                }
                catch
                { }
            }
        }

        void indSved2014PeriodsGridView_CommandCellClick(object sender, EventArgs e)
        {
            indSved2014PeriodsGridView.Rows[((sender as GridCommandCellElement)).RowIndex].Delete();
        }

        private void Year_SelectedIndexChanged()
        {
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            this.Quarter.Items.Clear();
            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                if (Quarter.Items.Count() > 0)
                {
                    if (q != 20 && Quarter.Items.Any(x => x.Value.ToString() == q.ToString()))
                        Quarter.Items.FirstOrDefault(x => x.Value.ToString() == q.ToString()).Selected = true;
                    else
                        Quarter.Items.First().Selected = true;
                }

            }
        }


        public void staffGridUpdate()
        {
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
                        INN = !String.IsNullOrEmpty(item.INN) ? item.INN.PadLeft(12, '0') : " ",
                        FIO = item.LastName + " " + item.FirstName + " " + item.MiddleName,
                        SNILS = Utils.ParseSNILS(item.InsuranceNumber, item.ControlNumber),
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

            if (staffList.Count > 0)
            {
                staffGridView.Columns[0].Width = 26;
                staffGridView.Columns["ID"].IsVisible = false;
                staffGridView.Columns["ID"].VisibleInColumnChooser = false;
                staffGridView.Columns["Num"].Width = 26;
                staffGridView.Columns["Num"].IsVisible = true;
                staffGridView.Columns["Num"].ReadOnly = true;
                staffGridView.Columns["Num"].HeaderText = "Номер";
                staffGridView.Columns["Num"].VisibleInColumnChooser = true;
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

        }



        private void ilospfrCheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            //if (!ilospfrCheckBox.Checked)
            //{
            //    ielospfrCheckBox.Checked = ilospfrCheckBox.Checked;
            //}
            //ielospfrCheckBox.Enabled = ilospfrCheckBox.Checked;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            short y;
            if (short.TryParse(Year.Text, out y))
            {
                if (Quarter.SelectedItem != null)
                {
                    byte q;
                    if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                    {
                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.indSved2014PeriodsGridView.MasterView);
                        rowInfo.Cells["year"].Value = y;
                        rowInfo.Cells["quarter"].Value = q;
                        rowInfo.Cells["name"].Value = Options.RaschetPeriodInternal.Any(x => x.Year == y && x.Kvartal == q) ? (q.ToString() + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == y && x.Kvartal == q).Name) : "Период не определен";

                        indSved2014PeriodsGridView.Rows.Add(rowInfo);
                    }
                    else
                    {
                        Messenger.showAlert(AlertType.Error, "Ошибка", "При добавлении Отчетного периода приоизошла ошибка", this.ThemeName);
                    }
                }
            }
            else
            {
                Messenger.showAlert(AlertType.Error, "Ошибка", "При добавлении Отчетного периода приоизошла ошибка", this.ThemeName);
            }
        }

        private void indSved2014CheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            indSved2014Panel.Enabled = indSvedPeriod2014CheckBox.Checked;
        }

        private void importBtn_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                cnt = 0;
                insChangeBtn.Enabled = false;
                importBtn.Enabled = false;
                radPageView2.Enabled = false;
                closeBtn.Visible = false;
                abortBtn.Location = closeBtn.Location;
                abortBtn.Visible = true;
                staffGridView.ReadOnly = true;
                firstPartLabel_.Visible = true;
                firstPartLabel.Visible = true;
                secondPartLabel_.Visible = true;
                secondPartLabel.Visible = true;
                processedFileName.Visible = true;
                excelCompatibility.Enabled = false;


                bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(exporting);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);

                bw.RunWorkerAsync();
            }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (cancel_work == false)
            {
                insChangeBtn.Enabled = true;
                importBtn.Enabled = true;
                radPageView2.Enabled = true;
                closeBtn.Visible = true;
                abortBtn.Visible = false;
                staffGridView.ReadOnly = false;
                excelCompatibility.Enabled = true;

                radProgressBar1.Value1 = 0;
                firstPartLabel_.Visible = false;
                firstPartLabel.Visible = false;
                secondPartLabel_.Visible = false;
                secondPartLabel.Visible = false;
                processedFileName.Visible = false;


                RadMessageBox.Show("Успешно сформировано " + cnt.ToString() + " файлов.", "Результат экспорта данных", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            radProgressBar1.Value1 = e.ProgressPercentage;
            firstPartLabel.Text = e.UserState.ToString();
        }

        private void exporting(object sender, DoWorkEventArgs e)
        {
            List<string> exportList = new List<string>();

            if (iempCheckBox.Checked)
            {
                if (INNCheckBox.Checked)
                    exportList.Add("iemp_6");
                else
                    exportList.Add("iemp_5");
            }
            if (idwempCheckBox.Checked)
                exportList.Add("idwemp_5");
            if (ipfrosnCheckBox.Checked)
                exportList.Add("ipfrosn_5");
            if (ipfrdopCheckBox.Checked)
                exportList.Add("ipfrdop_5");
            if (ilospfrCheckBox.Checked)
                exportList.Add("ilospfr_5");
            if (ielospfrCheckBox.Checked)
                exportList.Add("ielospfr_5");
            if (SZV_STAJCheckBox.Checked)
                exportList.Add("SZV_STAJ");
            if (SZV_STAJ5CheckBox.Checked)
                exportList.Add("SZV_STAJ5");
            if (SZV_STAJ_SCheckBox.Checked)
                exportList.Add("SZV_STAJ_S");
            if (SZV_STAJ_SLCheckBox.Checked)
                exportList.Add("SZV_STAJ_SL");

            staffIDList = new List<long> { };

            if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
            {
                foreach (var item in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {
                    staffIDList.Add(long.Parse(item.Cells[1].Value.ToString()));
                }
            }
            else
            {
                foreach (var item in staffGridView.Rows)
                {
                    staffIDList.Add(long.Parse(item.Cells[1].Value.ToString()));
                }
            }

            long odv1ID = 0;
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            {
                odv1ID = Convert.ToInt64(odv1GridView.CurrentRow.Cells[0].Value);
            }

            FormsODV_1_2017 odv1 = db.FormsODV_1_2017.FirstOrDefault(x => x.ID == odv1ID);

            foreach (var item in exportList)
            {
                if (bw.CancellationPending)
                {
                    return;
                }
                //                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));
                switch (item)
                {
                    case "iemp_5":
                        if (Export_iemp(item + ".dbf"))
                        {
                            cnt++;
                        }
                        else
                        {
                            //                            if (!bw.CancellationPending)
                            //                                importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; }));
                        }
                        break;
                    case "iemp_6":
                        if (Export_iemp(item + ".dbf"))
                        {
                            cnt++;
                        }
                        else
                        {
                        }
                        break;
                    case "idwemp_5":
                        if (Export_idwemp(item + ".dbf"))
                        {
                            cnt++;
                        }
                        else
                        {
                        }
                        break;
                    case "ipfrosn_5":
                        if (Export_ipfrosn())
                        {
                            cnt++;
                        }
                        else
                        {
                            //if (!bw.CancellationPending)  Export_ipfrdop
                            //    importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; }));
                        }
                        break;
                    case "ipfrdop_5":
                        if (Export_ipfrdop())
                        {
                            cnt++;
                        }
                        else
                        {
                            //if (!bw.CancellationPending)  Export_ilospfr
                            //    importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; }));
                        }
                        break;
                    case "ilospfr_5":
                        if (Export_ilospfr())
                        {
                            cnt++;
                        }
                        else
                        {
                            //if (!bw.CancellationPending)  Export_ilospfr
                            //    importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; }));
                        }
                        break;
                    case "ielospfr_5":
                        if (Export_ielospfr())
                        {
                            cnt++;
                        }
                        else
                        {
                            //if (!bw.CancellationPending)  Export_ilospfr
                            //    importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; }));
                        }
                        break;

                    case "SZV_STAJ":
                        if (odv1 == null)
                            break;
                        if (Export_SZV_STAJ(odv1))
                        {
                            cnt++;
                        }
                        else
                        {
                        }
                        break;
                    case "SZV_STAJ5":
                        if (odv1 == null)
                            break;
                        if (Export_SZV_STAJ5(odv1))
                        {
                            cnt++;
                        }
                        else
                        {
                        }
                        break;
                    case "SZV_STAJ_S":
                        if (odv1 == null)
                            break;
                        if (Export_SZV_STAJ_S(odv1))
                        {
                            cnt++;
                        }
                        else
                        {
                        }
                        break;
                    case "SZV_STAJ_SL":
                        if (odv1 == null)
                            break;
                        if (Export_SZV_STAJ_SL(odv1))
                        {
                            cnt++;
                        }
                        else
                        {
                        }
                        break;
                }
            }
        }

        private bool validation()
        {
            bool result = true;
            if (String.IsNullOrEmpty(pathBrowser.Value))
            {
                result = false;
                Messenger.showAlert(AlertType.Info, "Внимание!", "Необходимо указать Каталог для экспорта.", this.ThemeName);
            }

            if (staffGridView.RowCount == 0)
            {
                result = false;
                Messenger.showAlert(AlertType.Info, "Внимание!", "Список Сотрудников для выгрузки пустой.", this.ThemeName);
            }

            if (!Utils.CheckVfpOleDb())
            {
                result = false;
                Messenger.showAlert(AlertType.Error, "Внимание!", "В системе не установлен драйвер VFPOLEDB необходимый для экспорта в DBF.", this.ThemeName);
            }

            return result;
        }


        /// <summary>
        /// Экспорт данных сотрудников
        /// </summary>
        /// <returns></returns>
        private bool Export_iemp(string fileName)
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }

                if (File.Exists(Path.Combine(pathBrowser.Value, fileName)))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName));
                }

                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));
                secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = staffIDList.Count().ToString(); }));
                processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName; }));

                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN;");
                //                connection = new OleDbConnection(String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathBrowser.Value + ";Extended Properties=dBASE IV;", (codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " Locale Identifier = 866" : ""));
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();


                //command.CommandText = String.Format("CREATE TABLE {0} (INSURANCE Numeric(9,0), CNUMBER Numeric(2,0), TABLNUM Numeric(9,0), {1}" +
                //    "FAMIL Text(40), IMYA Text(40), OTCHES Text(40), POL Text(1), " +
                //    "DROZHDST DateTime, INFOADR Text(200), DEPTNUM Decimal(4), DISMYES Decimal(1), " +
                //    "TDROZHD Text(8), DROZHDDO Decimal(2), DROZHDMO Decimal(2), DROZHDYO Decimal(4), " +
                //    "TROZHD Text(8), PUNKT Text(40), DISTR Text(40), REGION Text(40), STRANA Text(40), CITIZ Text(20), " +
                //    "DOKKOD Text(14), DOKNAME Text(80), SERLAT Text(8), SERRUS Text(8), DOKNOM Decimal(8), DOKDATA DateTime, KEMVID Text(80), " +
                //    "PROPADR Text(200), FAKTADR Text(200), PHONE Text(40), DATAZAP DateTime, NAMEOPFR Text(50), DATAZAPDSW DateTime)", (INNCheckBox.Checked ? "iemp_6" : "iemp_5"), (INNCheckBox.Checked ? "INN Text(12), " : ""));


                command.CommandText = String.Format("CREATE TABLE {1} {0} (INSURANCE N(9), CNUMBER N(2), TABLNUM N(9), {2}" +
                    "FAMIL C(40), IMYA C(40), OTCHES C(40), POL C(1), " +
                    "DROZHDST DATE, INFOADR C(200), DEPTNUM N(4), DISMYES N(1), " +
                    "TDROZHD C(8), DROZHDDO N(2), DROZHDMO N(2), DROZHDYO N(4), " +
                    "TROZHD C(8), PUNKT C(40), DISTR C(40), REGION C(40), STRANA C(40), CITIZ C(20), " +
                    "DOKKOD C(14), DOKNAME C(80), SERLAT C(8), SERRUS C(8), DOKNOM N(8), DOKDATA DATE, KEMVID C(80), " +
                    "PROPADR C(200), FAKTADR C(200), PHONE C(40), DATAZAP DATE, NAMEOPFR C(50), DATAZAPDSW DATE)", (codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : "", (INNCheckBox.Checked ? "iemp_6" : "iemp_5"), (INNCheckBox.Checked ? "INN C(12), " : ""));



                command.ExecuteNonQuery();
                //    connection.Close();


                if (!emptyFilesCheckBox.Checked)
                {
                    var staffL = db.Staff.Where(x => staffIDList.Contains(x.ID)).ToList();

                    //connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + Path.Combine(pathBrowser.Value, fileName) + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN;");
                    //   connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.Combine(pathBrowser.Value, fileName).ToUpper() + ";Extended Properties=dBASE IV;");
                    command = connection.CreateCommand();
                    //    connection.Open();
                    int k = 0;
                    int count = staffL.Count();
                    foreach (var item in staffL)
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        command.CommandText = String.Format("INSERT INTO {8} Values ({0}, {1}, {2},{9} '{3}', '{4}', '{5}', '{6}', {7}, '', 0, {10}, '', 0, 0, 0, '', '', '', '', '', '', '', '', '', '', 0, {11}, '', '', '', '', {11}, '', {11});", item.InsuranceNumber, item.ControlNumber.HasValue ? item.ControlNumber.Value : 0, item.TabelNumber.HasValue ? item.TabelNumber.Value : 0, Win1251ToCP866(item.LastName), Win1251ToCP866(item.FirstName), Win1251ToCP866(item.MiddleName), item.Sex.HasValue ? (item.Sex.Value == 0 ? Win1251ToCP866("М") : Win1251ToCP866("Ж")) : "", item.DateBirth.HasValue ? item.DateBirth.Value.ToString("DATE(yyyy','MM','dd)") : "{}", (INNCheckBox.Checked ? "iemp_6" : "iemp_5"), (INNCheckBox.Checked ? ("'" + item.INN + "', ") : ""), item.Dismissed.HasValue ? item.Dismissed.Value : 0, "{}");
                        command.ExecuteNonQuery();
                        //             System.Threading.Thread.Sleep(10);

                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);
                    }

                    connection.Close();

                    makeCompatible(Path.Combine(pathBrowser.Value, fileName));

                }

            }
            catch
            {
                connection.Close();
                result = false;
            }
            finally
            {
                connection.Close();

            }


            return result;
        }

        /// <summary>
        /// Экспорт данных о приеме\увольнении
        /// </summary>
        /// <returns></returns>
        private bool Export_idwemp(string fileName)
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }

                if (File.Exists(Path.Combine(pathBrowser.Value, fileName)))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName));
                }

                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));
                secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = staffIDList.Count().ToString(); }));
                processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName; }));

                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN;");
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();

                command.CommandText = String.Format("CREATE TABLE {0} {1} (INSURANCE N(9), DBWORKS DATE, DEWORKS DATE)", fileName, ((codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : ""));



                command.ExecuteNonQuery();
                //    connection.Close();


                if (!emptyFilesCheckBox.Checked)
                {
                    var staffL = db.Staff.Where(x => staffIDList.Contains(x.ID)).ToList();

                    //connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + Path.Combine(pathBrowser.Value, fileName) + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN;");
                    //   connection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.Combine(pathBrowser.Value, fileName).ToUpper() + ";Extended Properties=dBASE IV;");
                    command = connection.CreateCommand();
                    //    connection.Open();
                    int k = 0;
                    int count = staffL.Count();
                    foreach (var item in staffL)
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }
                        foreach (var d in item.StaffDateWork)
                        {

                            command.CommandText = String.Format("INSERT INTO {0} Values ({1}, {2}, {3});", fileName, item.InsuranceNumber, d.DateBeginWork.HasValue ? d.DateBeginWork.Value.ToString("DATE(yyyy','MM','dd)") : "{}", d.DateEndWork.HasValue ? d.DateEndWork.Value.ToString("DATE(yyyy','MM','dd)") : "{}");
                            command.ExecuteNonQuery();
                            //             System.Threading.Thread.Sleep(10);

                        }
                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);
                    }

                    connection.Close();

                    makeCompatible(Path.Combine(pathBrowser.Value, fileName));

                }

            }
            catch
            {
                connection.Close();
                result = false;
            }
            finally
            {
                connection.Close();
            }


            return result;
        }


        private class staffT
        {
            public long rsw61 { get; set; }
            public string insurance { get; set; }
        }


        private string Win1251ToCP866(string inputString)
        {
            if (codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866")
            {
                Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
                Encoding dstEncodingFormat = Encoding.GetEncoding(866);
                byte[] originalByteString = srcEncodingFormat.GetBytes(inputString);
                byte[] convertedByteString = Encoding.Convert(srcEncodingFormat,
                dstEncodingFormat, originalByteString);
                string strOut = dstEncodingFormat.GetString(convertedByteString);

                return strOut;
            }
            return inputString;

        }

        private void ExportDBF_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками
            List<WindowData> windowData = new List<WindowData> { };

            if (pathBrowser.Value != null)
            {
                windowData.Add(new WindowData
                {
                    control = "pathBrowser",
                    value = pathBrowser.Value
                });
            }
            windowData.Add(new WindowData
            {
                control = "codepageDropDownList",
                value = codepageDropDownList.SelectedItem.Tag.ToString()
            });

            props.setFormParams(this, windowData);
        }

        private void abortBtn_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy)
                if (RadMessageBox.Show("Вы уверены, что хотите прервать импорт данных?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bw.CancelAsync();
                }
        }

        private void iempCheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            INNCheckBox.Enabled = iempCheckBox.Checked;
        }

        private void makeCompatible(string filePath)
        {
            if (excelCompatibility.Checked)
            {
                int cnt = 0;
                while (!File.Exists(filePath))
                {
                    if (cnt < 20)
                    {
                        System.Threading.Thread.Sleep(100);
                        cnt++;
                    }
                    else
                        return;
                }

                try
                {
                    List<byte> file = File.ReadAllBytes(filePath).ToList();
                    file.RemoveAt(0);
                    file.Insert(0, (byte)3);
                    File.WriteAllBytes(filePath, file.ToArray());
                }
                catch { }
            }
        }

        private void staffGridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
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

        private void staffGridView_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo != null && e.CellElement.ColumnInfo.Name == "Num" && e.CellElement.RowIndex >= 0)
            {
                e.CellElement.Value = e.CellElement.RowIndex + 1;
            }
        }


        #region  Экспорт  Формы РСВ-1 и Инд.сведений

        /// <summary>
        /// Экспорт Основных выплат сотрудников с 2014 Раздел 6.4
        /// </summary>
        /// <returns></returns>
        private bool Export_ipfrosn()
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {
                List<PlatCategory> platCat_List = db.PlatCategory.Where(x => x.PlatCategoryRaschPerID == 4).ToList();
                string fileName = "ipfrosn_5";

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }
                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));


                if (File.Exists(Path.Combine(pathBrowser.Value, fileName + ".dbf")))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName + ".dbf"));
                }


                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();

                command.CommandText = String.Format("CREATE TABLE {0} {1} (INSURANCE N(9), " +
                    "YEAR N(4),  QUARTER N(1), TYPEINFORM C(4), YEARKORR N(4), QUARTKORR N(1), SFPFR N(13,2), CATEGORY C(4), " +
                    "THRMONT1 N(13,2), THRMONT2 N(13,2), THRMONT3 N(13,2), THRMONT4 N(13,2), " +
                    "FRSMONT1 N(13,2), FRSMONT2 N(13,2), FRSMONT3 N(13,2), FRSMONT4 N(13,2), " +
                    "SECMONT1 N(13,2), SECMONT2 N(13,2), SECMONT3 N(13,2), SECMONT4 N(13,2), " +
                    "THIMONT1 N(13,2), THIMONT2 N(13,2), THIMONT3 N(13,2), THIMONT4 N(13,2), " +
                    "MSUMCOL4 N(1), DATAZAP DATE)", fileName, ((codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : ""));

                command.ExecuteNonQuery();
                //                connection.Close();

                if (!emptyFilesCheckBox.Checked)
                {
                    List<FormsRSW2014_1_Razd_6_1> list61 = getRazd_6_1("razd64");
                    processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName + ".dbf"; }));
                    secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = list61.Count().ToString(); }));

                    //                    connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + Path.Combine(pathBrowser.Value, fileName + ".dbf") + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                    command = connection.CreateCommand();
                    //                    connection.Open();
                    int k = 0;
                    int count = list61.Count();
                    string[] infoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };
                    command.CommandText = "";


                    var t = list61.Select(x => x.ID).ToList();
                    var list64 = db.FormsRSW2014_1_Razd_6_4.Where(x => t.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).ToList();
                    List<staffT> staffList = list61.Select(x => new staffT { rsw61 = x.ID, insurance = x.Staff.InsuranceNumber }).ToList();

                    foreach (var item in list61)
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        if (item.FormsRSW2014_1_Razd_6_4 != null) // если есть данные    основных выплатах Раздел 6.4
                        {

                            var tt = list64.Where(x => x.FormsRSW2014_1_Razd_6_1_ID.Value == item.ID).ToList();
                            foreach (var item64 in tt)//item.FormsRSW2014_1_Razd_6_4
                            {

                                string THRMONT = String.Format("{0}, {1}, {2}, {3}",
                                    item64.s_0_0.HasValue ? Utils.decToStr((decimal)item64.s_0_0.Value) : "0.00",
                                    item64.s_0_1.HasValue ? Utils.decToStr((decimal)item64.s_0_1.Value) : "0.00",
                                    item64.s_0_2.HasValue ? Utils.decToStr((decimal)item64.s_0_2.Value) : "0.00",
                                    item64.s_0_3.HasValue ? Utils.decToStr((decimal)item64.s_0_3.Value) : "0.00");

                                string FRSMONT = String.Format("{0}, {1}, {2}, {3}",
                                    item64.s_1_0.HasValue ? Utils.decToStr((decimal)item64.s_1_0.Value) : "0.00",
                                    item64.s_1_1.HasValue ? Utils.decToStr((decimal)item64.s_1_1.Value) : "0.00",
                                    item64.s_1_2.HasValue ? Utils.decToStr((decimal)item64.s_1_2.Value) : "0.00",
                                    item64.s_1_3.HasValue ? Utils.decToStr((decimal)item64.s_1_3.Value) : "0.00");

                                string SECMONT = String.Format("{0}, {1}, {2}, {3}",
                                    item64.s_2_0.HasValue ? Utils.decToStr((decimal)item64.s_2_0.Value) : "0.00",
                                    item64.s_2_1.HasValue ? Utils.decToStr((decimal)item64.s_2_1.Value) : "0.00",
                                    item64.s_2_2.HasValue ? Utils.decToStr((decimal)item64.s_2_2.Value) : "0.00",
                                    item64.s_2_3.HasValue ? Utils.decToStr((decimal)item64.s_2_3.Value) : "0.00");

                                string THIMONT = String.Format("{0}, {1}, {2}, {3}",
                                    item64.s_3_0.HasValue ? Utils.decToStr((decimal)item64.s_3_0.Value) : "0.00",
                                    item64.s_3_1.HasValue ? Utils.decToStr((decimal)item64.s_3_1.Value) : "0.00",
                                    item64.s_3_2.HasValue ? Utils.decToStr((decimal)item64.s_3_2.Value) : "0.00",
                                    item64.s_3_3.HasValue ? Utils.decToStr((decimal)item64.s_3_3.Value) : "0.00");

                                command.CommandText = String.Format("INSERT INTO {14} Values ({0}, {1}, {2}, '{3}', {4}, {5}, {6}, '{7}', {8}, {9}, {10}, {11}, {12}, {13})",
                                    //                                    item.Staff.InsuranceNumber,
                                staffList.FirstOrDefault(x => x.rsw61 == item.ID).insurance,
                                item.Year,
                                item.Quarter,
                                infoTypeStr[item.TypeInfoID - 1],
                                item.YearKorr.HasValue ? item.YearKorr.Value : 0,
                                item.QuarterKorr.HasValue ? item.QuarterKorr.Value : 0,
                                item.SumFeePFR.HasValue ? Utils.decToStr((decimal)item.SumFeePFR.Value) : Utils.decToStr(0),
                                platCat_List.First(x => x.ID == item64.PlatCategoryID).Code,
                                    //                    item64.PlatCategory.Code,
                                THRMONT,
                                FRSMONT,
                                SECMONT,
                                THIMONT,
                                item.AutoCalc.HasValue ? (item.AutoCalc.Value ? 1 : 0) : 0,
                                item.DateFilling.ToString("DATE(yyyy','MM','dd)"),
                                fileName);
                                command.ExecuteNonQuery();
                                //                   System.Threading.Thread.Sleep(10);
                            }


                        }
                        else // если данных о выплатах нет, тогда пишем нули
                        {
                            command.CommandText = String.Format("INSERT INTO {10} Values ({0}, {1}, {2}, '{3}', {4}, {5}, {6}, '{7}', 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, 0.00, {8}, {9})",
                                //                                item.Staff.InsuranceNumber,
                             staffList.FirstOrDefault(x => x.rsw61 == item.ID).insurance,
                             item.Year,
                             item.Quarter,
                             infoTypeStr[item.TypeInfoID - 1],
                             item.YearKorr.HasValue ? item.YearKorr.Value : 0,
                             item.QuarterKorr.HasValue ? item.QuarterKorr.Value : 0,
                             item.SumFeePFR.HasValue ? Utils.decToStr((decimal)item.SumFeePFR.Value) : Utils.decToStr(0),
                             "",
                             item.AutoCalc.HasValue ? (item.AutoCalc.Value ? 1 : 0) : 0,
                             item.DateFilling.ToString("DATE(yyyy','MM','dd)"),
                             fileName);

                            command.ExecuteNonQuery();
                        }



                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);

                        //if (k % 100 == 0)
                        //{
                        //    command.ExecuteNonQuery();
                        //    command.CommandText = "";

                        //}
                    }
                    //command.ExecuteNonQuery();
                    connection.Close();

                    makeCompatible(Path.Combine(pathBrowser.Value, fileName + ".dbf"));



                }

            }
            catch
            {
                connection.Close();
                result = false;
            }
            finally
            {
                connection.Close();
            }


            return result;
        }

        /// <summary>
        /// Экспорт Доп.выплат сотрудников с 2014 Раздел 6.7
        /// </summary>
        /// <returns></returns>
        private bool Export_ipfrdop()
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {

                string fileName = "ipfrdop_5";

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }
                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));


                if (File.Exists(Path.Combine(pathBrowser.Value, fileName + ".dbf")))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName + ".dbf"));
                }

                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();

                command.CommandText = String.Format("CREATE TABLE {0} {1} (INSURANCE N(9), " +
                    "YEAR N(4),  QUARTER N(1), TYPEINFORM C(4), YEARKORR N(4), QUARTKORR N(1), JOBEVAL C(4), " +
                    "THRMONT1 N(13,2), THRMONT2 N(13,2), " +
                    "FRSMONT1 N(13,2), FRSMONT2 N(13,2), " +
                    "SECMONT1 N(13,2), SECMONT2 N(13,2), " +
                    "THIMONT1 N(13,2), THIMONT2 N(13,2), " +
                    "DATAZAP DATE)", fileName, ((codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : ""));

                command.ExecuteNonQuery();
                //                connection.Close();

                if (!emptyFilesCheckBox.Checked)
                {
                    List<FormsRSW2014_1_Razd_6_1> list61 = getRazd_6_1("razd67");
                    processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName + ".dbf"; }));
                    secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = list61.Count().ToString(); }));

                    //                    connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + Path.Combine(pathBrowser.Value, fileName + ".dbf") + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                    command = connection.CreateCommand();
                    //                    connection.Open();
                    int k = 0;
                    int count = list61.Count();
                    string[] infoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };

                    foreach (var item in list61)
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        if (item.FormsRSW2014_1_Razd_6_4.Count() > 0) // если есть данные об основных выплатах Раздел 6.4
                        {
                            foreach (var item67 in item.FormsRSW2014_1_Razd_6_7)
                            {

                                string THRMONT = String.Format("{0}, {1}",
                                    item67.s_0_0.HasValue ? Utils.decToStr((decimal)item67.s_0_0.Value) : Utils.decToStr(0),
                                    item67.s_0_1.HasValue ? Utils.decToStr((decimal)item67.s_0_1.Value) : Utils.decToStr(0));

                                string FRSMONT = String.Format("{0}, {1}",
                                    item67.s_1_0.HasValue ? Utils.decToStr((decimal)item67.s_1_0.Value) : Utils.decToStr(0),
                                    item67.s_1_1.HasValue ? Utils.decToStr((decimal)item67.s_1_1.Value) : Utils.decToStr(0));

                                string SECMONT = String.Format("{0}, {1}",
                                    item67.s_2_0.HasValue ? Utils.decToStr((decimal)item67.s_2_0.Value) : Utils.decToStr(0),
                                    item67.s_2_1.HasValue ? Utils.decToStr((decimal)item67.s_2_1.Value) : Utils.decToStr(0));

                                string THIMONT = String.Format("{0}, {1}",
                                    item67.s_3_0.HasValue ? Utils.decToStr((decimal)item67.s_3_0.Value) : Utils.decToStr(0),
                                    item67.s_3_1.HasValue ? Utils.decToStr((decimal)item67.s_3_1.Value) : Utils.decToStr(0));

                                command.CommandText = String.Format("INSERT INTO {12} Values ({0}, {1}, {2}, '{3}', {4}, {5}, '{6}', {7}, {8}, {9}, {10}, {11});",
                                    item.Staff.InsuranceNumber,
                                    item.Year,
                                    item.Quarter,
                                    infoTypeStr[item.TypeInfoID - 1],
                                    item.YearKorr.HasValue ? item.YearKorr.Value : 0,
                                    item.QuarterKorr.HasValue ? item.QuarterKorr.Value : 0,
                                    item67.SpecOcenkaUslTrudaID.HasValue ? item67.SpecOcenkaUslTruda.Code : "",
                                    THRMONT,
                                    FRSMONT,
                                    SECMONT,
                                    THIMONT,
                                    item.DateFilling.ToString("DATE(yyyy','MM','dd)"),
                                    fileName);

                                command.ExecuteNonQuery();
                                //                                                  System.Threading.Thread.Sleep(10);
                            }
                        }
                        else // если данных о выплатах нет, тогда пишем нули
                        {
                            command.CommandText = String.Format("INSERT INTO {8} Values ({0}, {1}, {2}, '{3}', {4}, {5}, '{6}', 0, 0, 0, 0, 0, 0, 0, 0, {7});",
                                item.Staff.InsuranceNumber,
                                item.Year,
                                item.Quarter,
                                infoTypeStr[item.TypeInfoID - 1],
                                item.YearKorr.HasValue ? item.YearKorr.Value : 0,
                                item.QuarterKorr.HasValue ? item.QuarterKorr.Value : 0,
                                "",
                                item.DateFilling.ToString("DATE(yyyy','MM','dd)"),
                                fileName);

                            command.ExecuteNonQuery();
                        }


                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);
                    }

                    connection.Close();

                    makeCompatible(Path.Combine(pathBrowser.Value, fileName + ".dbf"));

                }

            }
            catch
            {
                connection.Close();

                result = false;
            }
            finally
            {
                connection.Close();
            }


            return result;
        }

        /// <summary>
        /// Экспорт данных о стаже сотрудников с 2014 Раздел 6.8
        /// </summary>
        /// <returns></returns>
        private bool Export_ilospfr()
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {
                string fileName = "ilospfr_5";

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }
                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));


                if (File.Exists(Path.Combine(pathBrowser.Value, fileName + ".dbf")))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName + ".dbf"));
                }

                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();

                command.CommandText = String.Format("CREATE TABLE {0} {1} (INSURANCE N(9), " +
                    "YEAR N(4),  QUARTER N(1), TYPEINFORM C(4), YEARKORR N(4), QUARTKORR N(1), " +
                    "PRNUMBER N(2), BPERIOD DATE, EPERIOD DATE, " +
                    "SCWORK C(8), POSITIONL C(20), PROFESSION C(100), BASISEXP C(7), " +
                    "AIEXP1 N(5), AIEXP2 N(2), AIEXP3 C(9), BASISYEAR C(8), AIYEAR1 N(5), AIYEAR2 N(4), AIYEAR3 N(4,2), TERRCONDIT C(4), RKOFF N(4,2))", fileName, ((codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : ""));

                command.ExecuteNonQuery();
                //                connection.Close();

                if (!emptyFilesCheckBox.Checked)
                {
                    List<FormsRSW2014_1_Razd_6_1> list61 = getRazd_6_1("");
                    List<long> list_id = list61.Select(c => c.ID).ToList();
                    var stajList = db.StajOsn.Where(x => x.FormsRSW2014_1_Razd_6_1_ID.HasValue && list_id.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).ToList();
                    int count = stajList.Count();
                    processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName + ".dbf"; }));
                    secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = count.ToString(); }));

                    //                    connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + Path.Combine(pathBrowser.Value, fileName + ".dbf") + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                    command = connection.CreateCommand();
                    //                    connection.Open();
                    int k = 0;
                    string[] infoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };


                    foreach (var item in stajList)  //list61
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }


                        command.CommandText = String.Format("INSERT INTO {9} Values ({0}, {1}, {2}, '{3}', {4}, {5}, {6}, {7}, {8}, '', '', '', '', 0, 0, '', '', 0, 0, 0, '', 0);",
                                item.FormsRSW2014_1_Razd_6_1.Staff.InsuranceNumber,
                                item.FormsRSW2014_1_Razd_6_1.Year,
                                item.FormsRSW2014_1_Razd_6_1.Quarter,
                                infoTypeStr[item.FormsRSW2014_1_Razd_6_1.TypeInfoID - 1],
                                item.FormsRSW2014_1_Razd_6_1.YearKorr.HasValue ? item.FormsRSW2014_1_Razd_6_1.YearKorr.Value : 0,
                                item.FormsRSW2014_1_Razd_6_1.QuarterKorr.HasValue ? item.FormsRSW2014_1_Razd_6_1.QuarterKorr.Value : 0,
                                item.Number.HasValue ? item.Number.Value : 0,
                                item.DateBegin.HasValue ? item.DateBegin.Value.ToString("DATE(yyyy','MM','dd)") : "{}",
                                item.DateEnd.HasValue ? item.DateEnd.Value.ToString("DATE(yyyy','MM','dd)") : "{}",
                                fileName);

                        command.ExecuteNonQuery();
                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);



                    }

                    connection.Close();

                    makeCompatible(Path.Combine(pathBrowser.Value, fileName + ".dbf"));

                }

            }
            catch
            {
                connection.Close();

                result = false;
            }
            finally
            {
                connection.Close();
            }


            return result;
        }

        /// <summary>
        /// Экспорт данных о Льготном стаже сотрудников с 2014 Раздел 6.8
        /// </summary>
        /// <returns></returns>
        private bool Export_ielospfr()
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {

                string fileName = "ielospfr_5";

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }
                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));

                if (File.Exists(Path.Combine(pathBrowser.Value, fileName + ".dbf")))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName + ".dbf"));
                }

                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();

                command.CommandText = String.Format("CREATE TABLE {0} {1} (INSURANCE N(9), " +
                    "YEAR N(4),  QUARTER N(1), TYPEINFORM C(4), YEARKORR N(4), QUARTKORR N(1), " +
                    "PRNUMBER N(2),RNUMBER N(2), " +
                    "SCWORK C(8), POSITIONL C(20), PROFESSION C(100), BASISEXP C(7), " +
                    "AIEXP1 N(5), AIEXP2 N(2), AIEXP3 C(9), BASISYEAR C(8), AIYEAR1 N(5), AIYEAR2 N(4), AIYEAR3 N(4,2), TERRCONDIT C(4), RKOFF N(4,2))", fileName, ((codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : ""));
                command.ExecuteNonQuery();
                //connection.Close();

                if (!emptyFilesCheckBox.Checked)
                {
                    List<FormsRSW2014_1_Razd_6_1> list61 = getRazd_6_1("");

                    List<long> list_id = list61.Select(c => c.ID).ToList();
                    var stajLgotList = db.StajOsn.Where(x => x.FormsRSW2014_1_Razd_6_1_ID.HasValue && list_id.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).SelectMany(x => x.StajLgot).ToList();
                    int count = stajLgotList.Count();

                    processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName + ".dbf"; }));
                    secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = count.ToString(); }));

                    //                    connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + Path.Combine(pathBrowser.Value, fileName + ".dbf") + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                    command = connection.CreateCommand();
                    //                    connection.Open();
                    int k = 0;
                    string[] infoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };

                    foreach (var item in stajLgotList)
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }
                        command.CommandText = String.Format("INSERT INTO {21} Values ({0}, {1}, {2}, '{3}', {4}, {5}, {6}, {7}, '{8}', '{9}', '{10}', '{11}', {12}, {13}, '{14}', '{15}', {16}, {17}, {18}, '{19}', {20});",
                            item.StajOsn.FormsRSW2014_1_Razd_6_1.Staff.InsuranceNumber,
                            item.StajOsn.FormsRSW2014_1_Razd_6_1.Year,
                            item.StajOsn.FormsRSW2014_1_Razd_6_1.Quarter,
                            infoTypeStr[item.StajOsn.FormsRSW2014_1_Razd_6_1.TypeInfoID - 1],
                            item.StajOsn.FormsRSW2014_1_Razd_6_1.YearKorr.HasValue ? item.StajOsn.FormsRSW2014_1_Razd_6_1.YearKorr.Value : 0,
                            item.StajOsn.FormsRSW2014_1_Razd_6_1.QuarterKorr.HasValue ? item.StajOsn.FormsRSW2014_1_Razd_6_1.QuarterKorr.Value : 0,
                            item.StajOsn.Number.HasValue ? item.StajOsn.Number.Value : 0,
                            item.Number.HasValue ? item.Number.Value : 0,
                            item.OsobUslTrudaID.HasValue ? item.OsobUslTruda.Code : "",
                            item.KodVred_OsnID.HasValue ? item.KodVred_2.Code : "",
                            item.DolgnID.HasValue ? item.Dolgn.Name.Substring(0, item.Dolgn.Name.Length > 100 ? 100 : item.Dolgn.Name.Length) : "",
                            item.IschislStrahStajOsnID.HasValue ? item.IschislStrahStajOsn.Code : "",
                            item.Strah1Param.HasValue ? item.Strah1Param.Value : 0,
                            item.Strah2Param.HasValue ? item.Strah2Param.Value : 0,
                            item.IschislStrahStajDopID.HasValue ? item.IschislStrahStajDop.Code : "",
                            item.UslDosrNaznID.HasValue ? item.UslDosrNazn.Code : "",
                            item.UslDosrNazn1Param.HasValue ? item.UslDosrNazn1Param.Value : 0,
                            item.UslDosrNazn2Param.HasValue ? item.UslDosrNazn2Param.Value : 0,
                            item.UslDosrNazn3Param.HasValue ? Utils.decToStr((decimal)item.UslDosrNazn3Param.Value) : Utils.decToStr(0),
                            item.TerrUslID.HasValue ? item.TerrUsl.Code : "",
                            item.TerrUslKoef.HasValue ? Utils.decToStr((decimal)item.TerrUslKoef.Value) : Utils.decToStr(0),
                            fileName);

                        command.ExecuteNonQuery();

                        //                          System.Threading.Thread.Sleep(10);

                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);
                    }
                    connection.Close();
                    makeCompatible(Path.Combine(pathBrowser.Value, fileName + ".dbf"));

                }

            }
            catch
            {
                connection.Close();

                result = false;
            }
            finally
            {
                connection.Close();
            }


            return result;
        }

        private List<FormsRSW2014_1_Razd_6_1> getRazd_6_1(string Razd)
        {
            List<FormsRSW2014_1_Razd_6_1> list61 = new List<FormsRSW2014_1_Razd_6_1> { };

            if (indSvedPeriod2014CheckBox.Checked) // если нужны данные за определенный отчетный период
            {

                if (indSved2014PeriodsGridView.RowCount == 0) // если список с отчетными периодамми пустой, а данные только с выпадающих списков
                {
                    short y;
                    if (short.TryParse(Year.Text, out y))
                    {
                        if (Quarter.SelectedItem != null)
                        {
                            byte q;
                            if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                            {
                                list61 = db.FormsRSW2014_1_Razd_6_1.Where(x => staffIDList.Contains(x.StaffID) && x.Year == y && x.Quarter == q).ToList();
                            }
                        }
                    }
                }
                else // если несколько отчетных периодов, смотрим список
                {
                    foreach (var row in indSved2014PeriodsGridView.Rows)
                    {
                        short y;
                        if (short.TryParse(row.Cells["year"].Value.ToString(), out y))
                        {
                            byte q;
                            if (byte.TryParse(row.Cells["quarter"].Value.ToString(), out q))
                            {
                                foreach (var item in db.FormsRSW2014_1_Razd_6_1.Where(x => staffIDList.Contains(x.StaffID) && x.Year == y && x.Quarter == q).ToList())
                                {
                                    list61.Add(item);
                                }
                            }

                        }
                    }
                }
            }
            else
            {
                list61 = db.FormsRSW2014_1_Razd_6_1.Where(x => staffIDList.Contains(x.StaffID)).ToList();
            }

            switch (Razd)
            {
                case "razd64":
                    list61 = list61.Where(x => x.FormsRSW2014_1_Razd_6_4 != null).OrderBy(x => x.StaffID).ThenBy(x => x.Year).ThenBy(x => x.Quarter).ToList();
                    break;
                case "razd67":
                    list61 = list61.Where(x => x.FormsRSW2014_1_Razd_6_7 != null).OrderBy(x => x.StaffID).ThenBy(x => x.Year).ThenBy(x => x.Quarter).ToList();
                    break;
                default:
                    list61 = list61.OrderBy(x => x.StaffID).ThenBy(x => x.Year).ThenBy(x => x.Quarter).ToList();
                    break;
            }


            return list61;
        }

        #endregion



        #region  Экспорт  Формы СЗВ-СТАЖ

        /// <summary>
        /// Экспорт Основных данных СЗВ-СТАЖ
        /// </summary>
        /// <returns></returns>
        private bool Export_SZV_STAJ(FormsODV_1_2017 odv1)
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {
                //          List<PlatCategory> platCat_List = db.PlatCategory.Where(x => x.PlatCategoryRaschPerID == 4).ToList();
                string fileName = "SZV_STAJ";

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }
                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));


                if (File.Exists(Path.Combine(pathBrowser.Value, fileName + ".dbf")))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName + ".dbf"));
                }


                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();

                command.CommandText = String.Format("CREATE TABLE {0} {1} (INSURANCE N(9), ODVTYPE C(4), ODVYEAR N(4),  ODVCODE N(1), " +
                    "YEAR N(4),  CODE N(1), TYPEINFORM C(4), OPSFEENACH N(1), DOPTARFEE N(1), " +
                    "CONFFIO C(120), CONFDOLGN C(120), " +
                    "DISMISSED N(1), DATEFILL DATE)", fileName, ((codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : ""));

                command.ExecuteNonQuery();

                if (!emptyFilesCheckBox.Checked)
                {
                    List<FormsSZV_STAJ_2017> szvlist = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1.ID).ToList();
                    processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName + ".dbf"; }));
                    secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = szvlist.Count().ToString(); }));

                    command = connection.CreateCommand();

                    int k = 0;
                    int count = szvlist.Count();
                    string[] odvinfoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };
                    string[] infoTypeStr = new string[] { "ИСХД", "ДОПЛ", "НАЗН" };
                    command.CommandText = "";


                    var t = szvlist.Select(x => x.ID).ToList();
                    List<staffT> staffList = szvlist.Select(x => new staffT { rsw61 = x.ID, insurance = x.Staff.InsuranceNumber }).ToList();

                    foreach (var item in szvlist)
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        command.CommandText = String.Format("INSERT INTO {13} Values ({0}, '{1}', {2}, {3}, {4}, {5}, '{6}', {7}, {8}, '{9}', '{10}', {11}, {12})",
                             staffList.FirstOrDefault(x => x.rsw61 == item.ID).insurance,
                             odvinfoTypeStr[odv1.TypeInfo.Value],
                             odv1.Year,
                             odv1.Code,
                             item.Year,
                             item.Code,
                             infoTypeStr[item.TypeInfo.Value],
                             item.OPSFeeNach.HasValue ? item.OPSFeeNach.Value : 0,
                             item.DopTarFeeNach.HasValue ? item.DopTarFeeNach.Value : 0,
                             !String.IsNullOrEmpty(item.ConfirmFIO) ? (item.ConfirmFIO.Length > 120 ? item.ConfirmFIO.Substring(0, 120) : item.ConfirmFIO) : "",
                             !String.IsNullOrEmpty(item.ConfirmDolgn) ? (item.ConfirmDolgn.Length > 120 ? item.ConfirmDolgn.Substring(0, 120) : item.ConfirmDolgn) : "",
                             item.Dismissed.HasValue ? (item.Dismissed.Value ? 1 : 0) : 0,
                             item.DateFilling.ToString("DATE(yyyy','MM','dd)"),
                             fileName);

                        command.ExecuteNonQuery();

                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);

                        //if (k % 100 == 0)
                        //{
                        //    command.ExecuteNonQuery();
                        //    command.CommandText = "";

                        //}
                    }
                    //command.ExecuteNonQuery();
                    connection.Close();

                    makeCompatible(Path.Combine(pathBrowser.Value, fileName + ".dbf"));



                }

            }
            catch
            {
                connection.Close();
                result = false;
            }
            finally
            {
                connection.Close();
            }


            return result;
        }

        /// <summary>
        /// Экспорт Раздела 4 Формы СЗВ-СТАЖ
        /// </summary>
        /// <returns></returns>
        private bool Export_SZV_STAJ5(FormsODV_1_2017 odv1)
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {

                string fileName = "SZV_STAJ5";

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }
                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));


                if (File.Exists(Path.Combine(pathBrowser.Value, fileName + ".dbf")))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName + ".dbf"));
                }

                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();

                command.CommandText = String.Format("CREATE TABLE {0} {1} (INSURANCE N(9), ODVTYPE C(4), ODVYEAR N(4),  ODVCODE N(1), " +
                    "YEAR N(4),  CODE N(1), TYPEINFORM C(4), DATEFROM DATE, DATETO DATE, DNPOFEE N(1))", fileName, ((codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : ""));


                command.ExecuteNonQuery();
                //                connection.Close();

                if (!emptyFilesCheckBox.Checked)
                {
                    List<FormsSZV_STAJ_2017> szvlist = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1.ID && x.FormsSZV_STAJ_4_2017.Count > 0).ToList();
                    processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName + ".dbf"; }));
                    secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = szvlist.Count().ToString(); }));

                    command = connection.CreateCommand();

                    int k = 0;
                    int count = szvlist.Count();
                    string[] odvinfoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };
                    string[] infoTypeStr = new string[] { "ИСХД", "ДОПЛ", "НАЗН" };
                    command.CommandText = "";


                    var t = szvlist.Select(x => x.ID).ToList();
                    List<staffT> staffList = szvlist.Select(x => new staffT { rsw61 = x.ID, insurance = x.Staff.InsuranceNumber }).ToList();

                    foreach (var item in szvlist)
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        foreach (var item4 in item.FormsSZV_STAJ_4_2017.ToList())
                        {

                            command.CommandText = String.Format("INSERT INTO {10} Values ({0}, '{1}', {2}, {3}, {4}, {5}, '{6}', {7}, {8}, {9})",
                                 staffList.FirstOrDefault(x => x.rsw61 == item.ID).insurance,
                                 odvinfoTypeStr[odv1.TypeInfo.Value],
                                 odv1.Year,
                                 odv1.Code,
                                 item.Year,
                                 item.Code,
                                 infoTypeStr[item.TypeInfo.Value],
                                 item4.DNPO_DateFrom.Value.ToString("DATE(yyyy','MM','dd)"),
                                 item4.DNPO_DateTo.Value.ToString("DATE(yyyy','MM','dd)"),
                                 item4.DNPO_Fee.HasValue ? (item4.DNPO_Fee.Value ? 1 : 0) : 0,
                                 fileName);

                            command.ExecuteNonQuery();



                            //if (k % 100 == 0)
                            //{
                            //    command.ExecuteNonQuery();
                            //    command.CommandText = "";

                            //}
                        }
                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);
                    }
                    //command.ExecuteNonQuery();
                    connection.Close();

                    makeCompatible(Path.Combine(pathBrowser.Value, fileName + ".dbf"));

                }

            }
            catch
            {
                connection.Close();

                result = false;
            }
            finally
            {
                connection.Close();
            }


            return result;
        }

        /// <summary>
        /// Экспорт данных о стаже сотрудников Формы СЗВ-СТАЖ
        /// </summary>
        /// <returns></returns>
        private bool Export_SZV_STAJ_S(FormsODV_1_2017 odv1)
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {
                string fileName = "SZV_STAJ_S";

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }
                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));


                if (File.Exists(Path.Combine(pathBrowser.Value, fileName + ".dbf")))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName + ".dbf"));
                }

                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();

                command.CommandText = String.Format("CREATE TABLE {0} {1} (INSURANCE N(9), ODVTYPE C(4), ODVYEAR N(4),  ODVCODE N(1), " +
                    "YEAR N(4),  CODE N(1), TYPEINFORM C(4), " +
                    "PRNUMBER N(3), BPERIOD DATE, EPERIOD DATE, CODEBEZR N(1))", fileName, ((codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : ""));


                command.ExecuteNonQuery();



                if (!emptyFilesCheckBox.Checked)
                {
                    List<FormsSZV_STAJ_2017> szvlist = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1.ID).ToList();

                    processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName + ".dbf"; }));
                    secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = szvlist.Count().ToString(); }));

                    command = connection.CreateCommand();

                    int k = 0;
                    int count = szvlist.Count();
                    string[] odvinfoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };
                    string[] infoTypeStr = new string[] { "ИСХД", "ДОПЛ", "НАЗН" };
                    command.CommandText = "";


                    var t = szvlist.Select(x => x.ID).ToList();
                    List<staffT> staffList = szvlist.Select(x => new staffT { rsw61 = x.ID, insurance = x.Staff.InsuranceNumber }).ToList();



                    foreach (var item in szvlist)  //list61
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        foreach (var itemS in item.StajOsn.ToList())
                        {
                            command.CommandText = String.Format("INSERT INTO {11} Values ({0}, '{1}', {2}, {3}, {4}, {5}, '{6}', {7}, {8}, {9}, {10})",
                                 staffList.FirstOrDefault(x => x.rsw61 == item.ID).insurance,
                                 odvinfoTypeStr[odv1.TypeInfo.Value],
                                 odv1.Year,
                                 odv1.Code,
                                 item.Year,
                                 item.Code,
                                 infoTypeStr[item.TypeInfo.Value],
                                 itemS.Number.HasValue ? itemS.Number.Value : 1,
                                 itemS.DateBegin.HasValue ? itemS.DateBegin.Value.ToString("DATE(yyyy','MM','dd)") : "{}",
                                 itemS.DateEnd.HasValue ? itemS.DateEnd.Value.ToString("DATE(yyyy','MM','dd)") : "{}",
                                 itemS.CodeBEZR.HasValue ? (itemS.CodeBEZR.Value ? 1 : 0) : 0,
                                 fileName);


                            command.ExecuteNonQuery();
                        }
                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);

                    }

                    connection.Close();

                    makeCompatible(Path.Combine(pathBrowser.Value, fileName + ".dbf"));

                }

            }
            catch
            {
                connection.Close();

                result = false;
            }
            finally
            {
                connection.Close();
            }


            return result;
        }

        /// <summary>
        /// Экспорт данных о Льготном стаже сотрудников Формы СЗВ-СТАЖ
        /// </summary>
        /// <returns></returns>
        private bool Export_SZV_STAJ_SL(FormsODV_1_2017 odv1)
        {
            bool result = true;
            OleDbConnection connection = new OleDbConnection();
            try
            {

                string fileName = "SZV_STAJ_SL";

                if (!Directory.Exists(pathBrowser.Value))
                {
                    Directory.CreateDirectory(pathBrowser.Value);
                }
                firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));

                if (File.Exists(Path.Combine(pathBrowser.Value, fileName + ".dbf")))
                {
                    File.Delete(Path.Combine(pathBrowser.Value, fileName + ".dbf"));
                }

                connection = new OleDbConnection(@"Provider=VFPOLEDB.1;Exclusive=Yes;SourceType=dbf;Data Source=" + pathBrowser.Value + ";Null=No;Mode=ReadWrite;Collating Sequence=RUSSIAN");
                OleDbCommand command = connection.CreateCommand();
                connection.Open();
                command.CommandText = "SET NULL OFF;";
                command.ExecuteNonQuery();

                command.CommandText = String.Format("CREATE TABLE {0} {1} (INSURANCE N(9), ODVTYPE C(4), ODVYEAR N(4),  ODVCODE N(1), " +
                    "YEAR N(4),  CODE N(1), TYPEINFORM C(4), " +
                    "PRNUMBER N(3),RNUMBER N(2), " +
                    "SCWORK C(8), POSITIONL C(20), PROFESSION C(100), BASISEXP C(7), " +
                    "AIEXP1 N(5), AIEXP2 N(2), AIEXP3 C(9), BASISYEAR C(8), AIYEAR1 N(5), AIYEAR2 N(4), AIYEAR3 N(4,2), TERRCONDIT C(4), RKOFF N(4,2))", fileName, ((codepageDropDownList.SelectedItem != null && codepageDropDownList.SelectedItem.Tag.ToString() == "866") ? " FREE CODEPAGE = 866" : ""));
                command.ExecuteNonQuery();

                if (!emptyFilesCheckBox.Checked)
                {
                    List<FormsSZV_STAJ_2017> szvlist = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1.ID).ToList();

                    processedFileName.Invoke(new Action(() => { processedFileName.Text = fileName + ".dbf"; }));
                    secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = szvlist.Count().ToString(); }));

                    command = connection.CreateCommand();

                    int k = 0;
                    int count = szvlist.Count();
                    string[] odvinfoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };
                    string[] infoTypeStr = new string[] { "ИСХД", "ДОПЛ", "НАЗН" };
                    command.CommandText = "";

                    var t = szvlist.Select(x => x.ID).ToList();
                    List<staffT> staffList = szvlist.Select(x => new staffT { rsw61 = x.ID, insurance = x.Staff.InsuranceNumber }).ToList();

                    foreach (var item in szvlist)  //list61
                    {
                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        foreach (var itemS in item.StajOsn.ToList())
                        {
                            foreach (var itemSL in itemS.StajLgot.ToList())
                            {
                                command.CommandText = String.Format("INSERT INTO {22} Values ({0}, '{1}', {2}, {3}, {4}, {5}, '{6}', {7}, {8}, '{9}', '{10}', '{11}', '{12}', {13}, {14}, '{15}', '{16}', {17}, {18}, {19}, '{20}', {21})",
                                     staffList.FirstOrDefault(x => x.rsw61 == item.ID).insurance,
                                     odvinfoTypeStr[odv1.TypeInfo.Value],
                                     odv1.Year,
                                     odv1.Code,
                                     item.Year,
                                     item.Code,
                                     infoTypeStr[item.TypeInfo.Value],
                                     itemS.Number.HasValue ? itemS.Number.Value : 1,
                                     itemSL.Number.HasValue ? itemSL.Number.Value : 1,
                                     itemSL.OsobUslTrudaID.HasValue ? itemSL.OsobUslTruda.Code : "",
                                     itemSL.KodVred_OsnID.HasValue ? itemSL.KodVred_2.Code : "",
                                     itemSL.DolgnID.HasValue ? itemSL.Dolgn.Name.Substring(0, itemSL.Dolgn.Name.Length > 100 ? 100 : itemSL.Dolgn.Name.Length) : "",
                                     itemSL.IschislStrahStajOsnID.HasValue ? itemSL.IschislStrahStajOsn.Code : "",
                                     itemSL.Strah1Param.HasValue ? itemSL.Strah1Param.Value : 0,
                                     itemSL.Strah2Param.HasValue ? itemSL.Strah2Param.Value : 0,
                                     itemSL.IschislStrahStajDopID.HasValue ? itemSL.IschislStrahStajDop.Code : "",
                                     itemSL.UslDosrNaznID.HasValue ? itemSL.UslDosrNazn.Code : "",
                                     itemSL.UslDosrNazn1Param.HasValue ? itemSL.UslDosrNazn1Param.Value : 0,
                                     itemSL.UslDosrNazn2Param.HasValue ? itemSL.UslDosrNazn2Param.Value : 0,
                                     itemSL.UslDosrNazn3Param.HasValue ? Utils.decToStr((decimal)itemSL.UslDosrNazn3Param.Value) : Utils.decToStr(0),
                                     itemSL.TerrUslID.HasValue ? itemSL.TerrUsl.Code : "",
                                     itemSL.TerrUslKoef.HasValue ? Utils.decToStr((decimal)itemSL.TerrUslKoef.Value) : Utils.decToStr(0),
                                     fileName);


                                command.ExecuteNonQuery();
                            }
                        }
                        k++;
                        decimal temp = (decimal)k / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, k);

                    }


                    connection.Close();
                    makeCompatible(Path.Combine(pathBrowser.Value, fileName + ".dbf"));

                }

            }
            catch
            {
                connection.Close();

                result = false;
            }
            finally
            {
                connection.Close();
            }


            return result;
        }




        #endregion



    }
}
