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

namespace PU.FormsODV1
{
    delegate void DelEvent();

    public partial class ODV1_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        ODV1_Print ReportViewerODV1;
        List<RaschetPeriodContainer> avail_periods_all = new List<RaschetPeriodContainer>();

        public class MonthesDict
        {
            public byte Code { get; set; }
            public string Name { get; set; }
        }

        List<MonthesDict> Monthes = new List<MonthesDict> { 
            new MonthesDict{Code = 1, Name = "Январь"},
            new MonthesDict{Code = 2, Name = "Февраль"},
            new MonthesDict{Code = 3, Name = "Март"},
            new MonthesDict{Code = 4, Name = "Апрель"},
            new MonthesDict{Code = 5, Name = "Май"},
            new MonthesDict{Code = 6, Name = "Июнь"},
            new MonthesDict{Code = 7, Name = "Июль"},
            new MonthesDict{Code = 8, Name = "Август"},
            new MonthesDict{Code = 9, Name = "Сентябрь"},
            new MonthesDict{Code = 10, Name = "Октябрь"},
            new MonthesDict{Code = 11, Name = "Ноябрь"},
            new MonthesDict{Code = 12, Name = "Декабрь"}
        };


        public ODV1_List()
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
                    ODV1AddBtn.Enabled = false;
                    ODV1DelBtn.Enabled = false;
                    createXMLBtn.Enabled = false;
                    radMenuItem1.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.BeginInvoke(new MethodInvoker(this.Close));
                    return;
            }


        }

        public static ODV1_List SelfRef
        {
            get;
            set;
        }

        private void ODV1_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year <= 2018).OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal2017.OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal2010_2013.OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal1996_2009.OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }

            checkAccessLevel();

            HeaderChange();

            this.odv1GridView.CurrentRowChanged += (s, с) => odv1GridView_CurrentRowChanged();

        }

        /// <summary>
        /// Смена заголовка при выборе страхователя
        /// </summary>
        public void HeaderChange()
        {
            insNamePanel.Text = Methods.HeaderChange();
            odv1Grid_upd();
      //      dataUpdate();
        }

        private void odv1GridView_CurrentRowChanged()
        {
            if (d != null)
                d();
        }

        DelEvent d;



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

            d = null;

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

            var odv1List_t = db.FormsODV_1_2017.Where(x => x.InsurerID == Options.InsID).ToList();

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

            d += dataUpdate;

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

                dataUpdate();
            }
            else
            {
                szv_stajGridView.DataSource = null;
                szv_ishGridView.DataSource = null;
                szv_korrGridView.DataSource = null;
            }
        }

        private void dataUpdate()
        {
            byte odv1TypeForm = 0;
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells["ID"].Value != null && odv1GridView.CurrentRow.Cells["TypeForm"].Value != null)
            {
                byte.TryParse(odv1GridView.CurrentRow.Cells["TypeForm"].Value.ToString(), out odv1TypeForm);
            }


            switch (odv1TypeForm)
            {
                case 1:
                    radPageView1.Pages[0].Enabled = true;
                    radPageView1.Pages[1].Enabled = false;
                    radPageView1.Pages[2].Enabled = false;
                    radPageView1.SelectedPage = radPageView1.Pages[0];
                    szv_stajGridView_upd();
                    break;
                case 2:
                    radPageView1.Pages[0].Enabled = false;
                    radPageView1.Pages[1].Enabled = true;
                    radPageView1.Pages[2].Enabled = false;
                    radPageView1.SelectedPage = radPageView1.Pages[1];
                    szv_ishGridView_upd();
                    break;
                case 3:
                    radPageView1.Pages[0].Enabled = false;
                    radPageView1.Pages[1].Enabled = false;
                    radPageView1.Pages[2].Enabled = true;
                    radPageView1.SelectedPage = radPageView1.Pages[2];
                    szv_korrGridView_upd();
                    break;
                case 4:
                    radPageView1.Pages[0].Enabled = true;
                    radPageView1.SelectedPage = radPageView1.Pages[0];

                    szv_stajGridView.DataSource = null;

                    radPageView1.Pages[0].Enabled = false;
                    radPageView1.Pages[1].Enabled = false;
                    radPageView1.Pages[2].Enabled = false;
                    break;
            }

        }



        public class StaffSZVSTAJ
        {
            public long ID { get; set; }
            public string FIO { get; set; }
            public string SNILS { get; set; }
            public short Year { get; set; }
            public string TypeInfo { get; set; }
            public DateTime DateFilling { get; set; }
        }

        public void szv_stajGridView_upd()
        {
            long odv1ID = 0;
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            {
                odv1ID = Convert.ToInt64(odv1GridView.CurrentRow.Cells[0].Value);
            }

            int rowindex = 0;
            string currId = "";
            if (szv_stajGridView.RowCount > 0 && szv_stajGridView.CurrentRow.Cells[1].Value != null)
            {
                rowindex = szv_stajGridView.CurrentRow.Index;
                currId = szv_stajGridView.CurrentRow.Cells[1].Value.ToString();
            }

            SortDescriptor descriptor = new SortDescriptor();

            if (szv_stajGridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = szv_stajGridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "FIO";
                descriptor.Direction = ListSortDirection.Ascending;
            }

            List<FormsSZV_STAJ_2017> itemList = new List<FormsSZV_STAJ_2017>();

            itemList = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1ID).ToList();


         //   var t = itemList.Select(x => x.StaffID).ToList();
            //List<Staff> staffL = db.Staff.Where(x => t.Contains(x.ID)).ToList();

            List<Staff> staffL = db.Staff.Where(x => x.InsurerID == Options.InsID).ToList();

            this.szv_stajGridView.TableElement.BeginUpdate();

            List<StaffSZVSTAJ> staffList = new List<StaffSZVSTAJ> { };

            if (itemList.Count() != 0)
            {
                int i = 0;
                foreach (var item in itemList)
                {
                    i++;

                    var staff = staffL.FirstOrDefault(x => x.ID == item.StaffID);

                    string tInfo = "";

                    if (item.TypeInfo.HasValue)
                    {
                        switch (item.TypeInfo.Value)
                        {
                            case 0:
                                tInfo = "Исходная";
                                break;
                            case 1:
                                tInfo = "Дополняющая";
                                break;
                            case 2:
                                tInfo = "Назначение пенсии";
                                break;
                        }
                    }
                    staffList.Add(new StaffSZVSTAJ()
                    {
                        ID = item.ID,
                        FIO = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName,
                        SNILS = Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber),
                        Year = item.Year,
                        TypeInfo = tInfo,
                        DateFilling = item.DateFilling
                    });

                }
            }
            szv_stajGridView.DataSource = staffList.OrderBy(x => x.FIO);
            if (descriptor != null)
            {
                this.szv_stajGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (staffList.Count > 0)
            {
                szv_stajGridView.Columns[0].Width = 26;
                szv_stajGridView.Columns["ID"].IsVisible = false;
                szv_stajGridView.Columns["FIO"].Width = 230;
                szv_stajGridView.Columns["FIO"].ReadOnly = true;
                szv_stajGridView.Columns["FIO"].WrapText = true;
                szv_stajGridView.Columns["FIO"].HeaderText = "Фамилия Имя Отчество";
                szv_stajGridView.Columns["SNILS"].Width = 90;
                szv_stajGridView.Columns["SNILS"].HeaderText = "СНИЛС";
                szv_stajGridView.Columns["Year"].Width = 100;
                szv_stajGridView.Columns["Year"].HeaderText = "Год";
                szv_stajGridView.Columns["TypeInfo"].HeaderText = "Тип сведений";
                szv_stajGridView.Columns["TypeInfo"].Width = 70;
                szv_stajGridView.Columns["DateFilling"].HeaderText = "Дата заполнения";
                szv_stajGridView.Columns["DateFilling"].Width = 70;
                szv_stajGridView.Columns["DateFilling"].FormatString = "{0:dd.MM.yyyy}";

                for (var i = 3; i < (szv_stajGridView.Columns.Count); i++)
                {
                    szv_stajGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    szv_stajGridView.Columns[i].ReadOnly = true;
                }

                this.szv_stajGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }

            //foreach (var item in szv_stajGridView.Rows)
            //{
            //    item.MinHeight = 22;
            //}

            this.szv_stajGridView.TableElement.EndUpdate();

            //if (szv_stajGridView.RowCount > 0)
            //{
            //    if (szv_stajGridView.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
            //    {
            //        szv_stajGridView.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
            //    }
            //    else
            //    {
            //        if (rowindex >= szv_stajGridView.ChildRows.Count)
            //            rowindex = szv_stajGridView.ChildRows.Count - 1;
            //        rowindex = rowindex >= 0 ? rowindex : 0;
            //        szv_stajGridView.ChildRows[rowindex].IsCurrent = true;
            //    }


            //}

        }

        public class StaffSZVISH
        {
            public long ID { get; set; }
            public string FIO { get; set; }
            public string SNILS { get; set; }
            public short Year { get; set; }
            public string Code { get; set; }
            public DateTime DateFilling { get; set; }
        }

        public void szv_ishGridView_upd()
        {
            long odv1ID = 0;
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            {
                odv1ID = Convert.ToInt64(odv1GridView.CurrentRow.Cells[0].Value);
            }

            int rowindex = 0;
            string currId = "";
            if (szv_ishGridView.RowCount > 0 && szv_ishGridView.CurrentRow.Cells[1].Value != null)
            {
                rowindex = szv_ishGridView.CurrentRow.Index;
                currId = szv_ishGridView.CurrentRow.Cells[1].Value.ToString();
            }

            SortDescriptor descriptor = new SortDescriptor();

            if (szv_ishGridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = szv_ishGridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "FIO";
                descriptor.Direction = ListSortDirection.Ascending;
            }


            var itemList = db.FormsSZV_ISH_2017.Where(x => x.FormsODV_1_2017_ID == odv1ID).ToList();
            List<Staff> staffL = db.Staff.Where(x => x.InsurerID == Options.InsID).ToList();


            this.szv_ishGridView.TableElement.BeginUpdate();

            List<StaffSZVISH> staffList = new List<StaffSZVISH> { };

            if (itemList.Count() != 0)
            {
                int i = 0;
                foreach (var item in itemList)
                {
                    i++;

                    var staff = staffL.FirstOrDefault(x => x.ID == item.StaffID);
                    RaschetPeriodContainer rp = avail_periods_all.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Code);

                    staffList.Add(new StaffSZVISH()
                    {
                        ID = item.ID,
                        FIO = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName,
                        SNILS = Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber),
                        Year = item.Year,
                        Code = rp != null ? (rp.Kvartal + " - " + rp.Name) : "Период не определен",
                        DateFilling = item.DateFilling
                    });

                }
            }
            szv_ishGridView.DataSource = staffList.OrderBy(x => x.FIO);
            if (descriptor != null)
            {
                this.szv_ishGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (staffList.Count > 0)
            {
                szv_ishGridView.Columns[0].Width = 26;
                szv_ishGridView.Columns["ID"].IsVisible = false;
                szv_ishGridView.Columns["FIO"].Width = 230;
                szv_ishGridView.Columns["FIO"].ReadOnly = true;
                szv_ishGridView.Columns["FIO"].WrapText = true;
                szv_ishGridView.Columns["FIO"].HeaderText = "Фамилия Имя Отчество";
                szv_ishGridView.Columns["SNILS"].Width = 90;
                szv_ishGridView.Columns["SNILS"].HeaderText = "СНИЛС";
                szv_ishGridView.Columns["Year"].Width = 100;
                szv_ishGridView.Columns["Year"].HeaderText = "Год";
                szv_ishGridView.Columns["Code"].HeaderText = "Отчетный период";
                szv_ishGridView.Columns["Code"].Width = 70;
                szv_ishGridView.Columns["DateFilling"].HeaderText = "Дата заполнения";
                szv_ishGridView.Columns["DateFilling"].Width = 70;
                szv_ishGridView.Columns["DateFilling"].FormatString = "{0:dd.MM.yyyy}";

                for (var i = 3; i < (szv_ishGridView.Columns.Count); i++)
                {
                    szv_ishGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    szv_ishGridView.Columns[i].ReadOnly = true;
                }

                this.szv_ishGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }

            //foreach (var item in szv_ishGridView.Rows)
            //{
            //    item.MinHeight = 22;
            //}

            this.szv_ishGridView.TableElement.EndUpdate();


        }


        public class StaffSZVKORR
        {
            public long ID { get; set; }
            public string FIO { get; set; }
            public string SNILS { get; set; }
            public string TypeInfo { get; set; }
            public short Year { get; set; }
            public string Code { get; set; }
            public short? YearKorr { get; set; }
            public string CodeKorr { get; set; }
            public DateTime DateFilling { get; set; }
        }

        public void szv_korrGridView_upd()
        {
            long odv1ID = 0;
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            {
                odv1ID = Convert.ToInt64(odv1GridView.CurrentRow.Cells[0].Value);
            }

            int rowindex = 0;
            string currId = "";
            if (szv_korrGridView.RowCount > 0 && szv_korrGridView.CurrentRow.Cells[1].Value != null)
            {
                rowindex = szv_korrGridView.CurrentRow.Index;
                currId = szv_korrGridView.CurrentRow.Cells[1].Value.ToString();
            }

            SortDescriptor descriptor = new SortDescriptor();

            if (szv_korrGridView.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = szv_korrGridView.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "FIO";
                descriptor.Direction = ListSortDirection.Ascending;
            }


            var itemList = db.FormsSZV_KORR_2017.Where(x => x.FormsODV_1_2017_ID == odv1ID).ToList();
            List<Staff> staffL = db.Staff.Where(x => x.InsurerID == Options.InsID).ToList();


            this.szv_korrGridView.TableElement.BeginUpdate();

            List<StaffSZVKORR> staffList = new List<StaffSZVKORR> { };

            if (itemList.Count() != 0)
            {
                int i = 0;
                foreach (var item in itemList)
                {
                    i++;

                    var staff = staffL.FirstOrDefault(x => x.ID == item.StaffID);
                    RaschetPeriodContainer rp = avail_periods_all.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Code);
                    RaschetPeriodContainer rpKorr = avail_periods_all.FirstOrDefault(x => x.Year == item.YearKorr && x.Kvartal == item.CodeKorr);

                    string tInfo = "";

                    if (item.TypeInfo.HasValue)
                    {
                        switch (item.TypeInfo.Value)
                        {
                            case 0:
                                tInfo = "Корректирующая";
                                break;
                            case 1:
                                tInfo = "Отменяющая";
                                break;
                            case 2:
                                tInfo = "Особая";
                                break;
                        }
                    }

                    staffList.Add(new StaffSZVKORR()
                    {
                        ID = item.ID,
                        FIO = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName,
                        SNILS = Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber),
                        TypeInfo = tInfo,
                        Year = item.Year,
                        Code = rp != null ? (rp.Kvartal + " - " + rp.Name) : "Период не определен",
                        YearKorr = item.YearKorr,
                        CodeKorr = rpKorr != null ? (rpKorr.Kvartal + " - " + rpKorr.Name) : "Период не определен",
                        DateFilling = item.DateFilling
                    });

                }
            }
            szv_korrGridView.DataSource = staffList.OrderBy(x => x.FIO);
            if (descriptor != null)
            {
                this.szv_korrGridView.MasterTemplate.SortDescriptors.Add(descriptor);
            }

            if (staffList.Count > 0)
            {
                szv_korrGridView.Columns[0].Width = 26;
                szv_korrGridView.Columns["ID"].IsVisible = false;
                szv_korrGridView.Columns["FIO"].Width = 230;
                szv_korrGridView.Columns["FIO"].ReadOnly = true;
                szv_korrGridView.Columns["FIO"].WrapText = true;
                szv_korrGridView.Columns["FIO"].HeaderText = "Фамилия Имя Отчество";
                szv_korrGridView.Columns["SNILS"].Width = 90;
                szv_korrGridView.Columns["SNILS"].HeaderText = "СНИЛС";
                szv_korrGridView.Columns["TypeInfo"].Width = 90;
                szv_korrGridView.Columns["TypeInfo"].HeaderText = "Тип сведений";
                szv_korrGridView.Columns["Year"].Width = 90;
                szv_korrGridView.Columns["Year"].HeaderText = "Год";
                szv_korrGridView.Columns["Code"].HeaderText = "Отчетный период";
                szv_korrGridView.Columns["Code"].Width = 130;
                szv_korrGridView.Columns["YearKorr"].Width = 90;
                szv_korrGridView.Columns["YearKorr"].HeaderText = "Корр. год";
                szv_korrGridView.Columns["CodeKorr"].HeaderText = "Корр. отчетный период";
                szv_korrGridView.Columns["CodeKorr"].Width = 130;
                szv_korrGridView.Columns["DateFilling"].HeaderText = "Дата заполнения";
                szv_korrGridView.Columns["DateFilling"].Width = 70;
                szv_korrGridView.Columns["DateFilling"].FormatString = "{0:dd.MM.yyyy}";

                for (var i = 3; i < (szv_korrGridView.Columns.Count); i++)
                {
                    szv_korrGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    szv_korrGridView.Columns[i].ReadOnly = true;
                }

                this.szv_korrGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            }

            //foreach (var item in szv_korrGridView.Rows)
            //{
            //    item.MinHeight = 22;
            //}

            this.szv_korrGridView.TableElement.EndUpdate();


        }


        private void ODV1AddBtn_Click(object sender, EventArgs e)
        {

            if (Options.InsID != 0)
            {
                ODV1_Edit child = new ODV1_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.ShowDialog();
                if (child.ODV1 != null)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    odv1Grid_upd();
                    string id = child.ODV1.ID.ToString();
                    if (odv1GridView.Rows.Any(x => x.Cells["ID"].Value.ToString() == id))
                        odv1GridView.Rows.First(x => x.Cells["ID"].Value.ToString() == id).IsCurrent = true;
                }
                child.Dispose();

            }
        }

        private void ODV1EditBtn_Click(object sender, EventArgs e)
        {
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(odv1GridView.CurrentRow.Cells[0].Value.ToString());

                ODV1_Edit child = new ODV1_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.ODV1ID = id;
                child.action = "edit";
                child.ShowDialog();
                if (child.ODV1 != null)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    odv1Grid_upd();
                }
                child.Dispose();

            }
        }

        private void ODV1DelBtn_Click(object sender, EventArgs e)
        {
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            {
                if (RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    long id = (long)odv1GridView.CurrentRow.Cells[0].Value;//.ToString()
                    try
                    {
                        var odv = db.FormsODV_1_2017.FirstOrDefault(x => x.ID == id);
                        db.DeleteObject(odv);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    odv1Grid_upd();
                    this.Cursor = Cursors.Default;
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


        private void createXMLBtn_Click(object sender, EventArgs e)
        {
            if (!createXMLBtn.Enabled)
                return;
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            {
                ODV1_CreateXML child = new ODV1_CreateXML();

                long id = long.Parse(odv1GridView.CurrentRow.Cells[0].Value.ToString());

                var odv1Data = db.FormsODV_1_2017.FirstOrDefault(x => x.ID == id);

                if (odv1Data.TypeForm.HasValue)
                {

                    byte odv1TypeForm = odv1Data.TypeForm.Value;
                    switch (odv1Data.TypeForm.Value)
                    {
                        case 1://"СЗВ-СТАЖ"
                            if (szv_stajGridView.RowCount > 0)
                            {
                                if (szv_stajGridView.CurrentRow.Cells[1].Value != null)
                                {
                                    child.currentStaffId = long.Parse(szv_stajGridView.CurrentRow.Cells[1].Value.ToString());
                                }

                                List<long> ids = new List<long>();

                                if (szv_stajGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                {
                                    foreach (var item in szv_stajGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                    {
                                        ids.Add(long.Parse(item.Cells[1].Value.ToString()));
                                    }

                                    child.staffList_temp = ids;
                                }
                            }
                            break;
                        case 2://"СЗВ-ИСХ"
                            if (szv_ishGridView.RowCount > 0)
                            {
                                if (szv_ishGridView.CurrentRow.Cells[1].Value != null)
                                {
                                    child.currentStaffId = long.Parse(szv_ishGridView.CurrentRow.Cells[1].Value.ToString());
                                }

                                List<long> ids = new List<long>();

                                if (szv_ishGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                {
                                    foreach (var item in szv_ishGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                    {
                                        ids.Add(long.Parse(item.Cells[1].Value.ToString()));
                                    }

                                    child.staffList_temp = ids;
                                }
                            }
                            break;
                        case 3://"СЗВ-КОРР"
                            if (szv_korrGridView.RowCount > 0)
                            {
                                if (szv_korrGridView.CurrentRow.Cells[1].Value != null)
                                {
                                    child.currentStaffId = long.Parse(szv_korrGridView.CurrentRow.Cells[1].Value.ToString());
                                }

                                List<long> ids = new List<long>();

                                if (szv_korrGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                {
                                    foreach (var item in szv_korrGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                    {
                                        ids.Add(long.Parse(item.Cells[1].Value.ToString()));
                                    }

                                    child.staffList_temp = ids;
                                }
                            }
                            break;
                        case 4://"ОДВ-1"

                            break;
                    }
                }


                child.odv1Data = odv1Data;
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowDialog();
            }

        }

        private void printODV1Btn_Click(object sender, EventArgs e)
        {
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(odv1GridView.CurrentRow.Cells[0].Value.ToString());
                if (db.FormsODV_1_2017.Any(x => x.ID == id))
                {

                    ReportViewerODV1 = new ODV1_Print();
                    ReportViewerODV1.ODV1_Data = db.FormsODV_1_2017.FirstOrDefault(x => x.ID == id);
                    ReportViewerODV1.Owner = this;
                    ReportViewerODV1.printODV1 = true;
                    ReportViewerODV1.ThemeName = this.ThemeName;
                    ReportViewerODV1.ShowInTaskbar = false;

                    this.Cursor = Cursors.WaitCursor;

                    radWaitingBar1.Visible = true;
                    radWaitingBar1.StartWaiting();

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerODV1.createReport);
                    bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedODV1);

                    bw.RunWorkerAsync();
                }
                else
                {
                    RadMessageBox.Show(this, "Не удалось загрузить данные из базы данных для печати формы", "");
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }


        private void bw_RunWorkerCompletedODV1(object sender, RunWorkerCompletedEventArgs e)
        {
            radWaitingBar1.Invoke(new Action(() => { radWaitingBar1.StopWaiting(); radWaitingBar1.Visible = false; }));
            this.Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

            ReportViewerODV1.ShowDialog();
        }

        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            if (!radMenuItem1.Enabled)
                return;

            //if (odv1GridView.RowCount != 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            //{
            //    long odv1ID = long.Parse(odv1GridView.CurrentRow.Cells[0].Value.ToString());

            //    ODV1_Copy child = new ODV1_Copy();
            //    child.Owner = this;
            //    child.ThemeName = this.ThemeName;
            //    child.odv1ID = odv1ID;
            //    child.ShowDialog();
            //    db.DetectChanges();
            //    db = new pu6Entities();
            //    odv1Grid_upd();
            //}
            //else
            //{
            //    RadMessageBox.Show(this, "Нет данных для копирования", "");
            //}
        }

        private void odv1GridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            ODV1EditBtn_Click(null, null);
        }

        private void odv1GridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(ODV1AddBtn_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(ODV1EditBtn_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(ODV1DelBtn_Click);
                RadMenuItem menuItem4 = new RadMenuItem("Печать");
                menuItem4.Click += new EventHandler(printODV1Btn_Click);
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


        private void printBtn_Click(object sender, EventArgs e)
        {
            if (szv_stajGridView.RowCount != 0 && szv_stajGridView.CurrentRow.Cells["ID"].Value != null)
            {
                long id = long.Parse(szv_stajGridView.CurrentRow.Cells["ID"].Value.ToString());

                FormsSZV_STAJ_2017 szv_staj_data = db.FormsSZV_STAJ_2017.FirstOrDefault(x => x.ID == id);

                ReportViewerODV1 = new ODV1_Print();
                ReportViewerODV1.SZV_STAJ_List = new List<FormsSZV_STAJ_2017>();
                ReportViewerODV1.SZV_STAJ_List.Add(szv_staj_data);
                ReportViewerODV1.TypeForm = 1;
                ReportViewerODV1.Owner = this;
                ReportViewerODV1.ThemeName = this.ThemeName;
                ReportViewerODV1.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerODV1.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedODV1);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }

        }

        private void szv_stajAddBtn_Click(object sender, EventArgs e)
        {
            if (!szv_stajAddBtn.Enabled)
                return;

            if (odv1GridView.ChildRows.Count > 0 && odv1GridView.CurrentRow.Cells["ID"].Value != null && odv1GridView.CurrentRow.Index >= 0)
            {
                long id = Convert.ToInt64(odv1GridView.CurrentRow.Cells["ID"].Value);

                PU.FormsSZV_STAJ.SZV_STAJ_Edit child = new PU.FormsSZV_STAJ.SZV_STAJ_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.parentName = this.Name;
                //                child.staff = db.Staff.FirstOrDefault(x => x.ID == id);
                child.ODV1_ID = id;
                child.ShowDialog();
                child.Dispose();
                db.DetectChanges();
                db = new pu6Entities();

                szv_stajGridView_upd();

            }
        }

        private void szv_stajEditBtn_Click(object sender, EventArgs e)
        {
            if (szv_stajGridView.RowCount > 0 && szv_stajGridView.CurrentRow.Cells[1].Value != null)
            {
                long id = Convert.ToInt64(odv1GridView.CurrentRow.Cells["ID"].Value);
                long id_szv_staj = Convert.ToInt64(szv_stajGridView.CurrentRow.Cells[1].Value);
                int rowindex = szv_stajGridView.CurrentRow.Index;

                PU.FormsSZV_STAJ.SZV_STAJ_Edit child = new PU.FormsSZV_STAJ.SZV_STAJ_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.SZV_STAJ = db.FormsSZV_STAJ_2017.FirstOrDefault(x => x.ID == id_szv_staj);
                child.action = "edit";
                child.ODV1_ID = id;
                child.parentName = this.Name;
                child.ShowDialog();
                child.Dispose();
                db.DetectChanges();
                db = new pu6Entities();
                szv_stajGridView_upd();
                if (rowindex >= 0)
                    szv_stajGridView.Rows[rowindex].IsCurrent = true;

            }
        }

        private void szv_stajDelBtn_Click(object sender, EventArgs e)
        {
            if (!szv_stajAddBtn.Enabled)
                return;

            if (szv_stajGridView.RowCount > 0 && szv_stajGridView.CurrentRow.Cells[1].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    string id = szv_stajGridView.CurrentRow.Cells[1].Value.ToString();

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_STAJ_2017 WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    szv_stajGridView_upd();

                }
            }
        }

        private void szv_ishAddBtn_Click(object sender, EventArgs e)
        {
            if (!szv_ishAddBtn.Enabled)
                return;

            if (odv1GridView.ChildRows.Count > 0 && odv1GridView.CurrentRow.Cells["ID"].Value != null && odv1GridView.CurrentRow.Index >= 0)
            {
                long id = Convert.ToInt64(odv1GridView.CurrentRow.Cells["ID"].Value);

                PU.FormsSZV_ISH.SZV_ISH_Edit child = new PU.FormsSZV_ISH.SZV_ISH_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.parentName = this.Name;
                child.ODV1 = db.FormsODV_1_2017.First(x => x.ID == id);
                child.Monthes = Monthes;
                child.ShowDialog();
                child.Dispose();
                db.DetectChanges();
                db = new pu6Entities();

                szv_ishGridView_upd();

            }
        }

        private void szv_stajGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            szv_stajEditBtn_Click(null, null);
        }

        private void szv_ishEditBtn_Click(object sender, EventArgs e)
        {
            if (szv_ishGridView.RowCount > 0 && szv_ishGridView.CurrentRow.Cells[1].Value != null)
            {
                long id = Convert.ToInt64(odv1GridView.CurrentRow.Cells["ID"].Value);
                long id_szv_ish = Convert.ToInt64(szv_ishGridView.CurrentRow.Cells[1].Value);
                int rowindex = szv_ishGridView.CurrentRow.Index;

                PU.FormsSZV_ISH.SZV_ISH_Edit child = new PU.FormsSZV_ISH.SZV_ISH_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.SZV_ISH = db.FormsSZV_ISH_2017.FirstOrDefault(x => x.ID == id_szv_ish);
                child.action = "edit";
                child.parentName = this.Name;
                child.Monthes = Monthes;
                child.ShowDialog();
                child.Dispose();
                db.DetectChanges();
                db = new pu6Entities();
                szv_ishGridView_upd();
                if (rowindex >= 0)
                    szv_ishGridView.Rows[rowindex].IsCurrent = true;

            }
        }

        private void szv_ishDelBtn_Click(object sender, EventArgs e)
        {
            if (!szv_ishAddBtn.Enabled)
                return;

            if (szv_ishGridView.RowCount > 0 && szv_ishGridView.CurrentRow.Cells[1].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    string id = szv_ishGridView.CurrentRow.Cells[1].Value.ToString();

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_ISH_2017 WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    szv_ishGridView_upd();

                }
            }
        }

        private void printSZVISHBtn_Click(object sender, EventArgs e)
        {
            if (szv_ishGridView.RowCount != 0 && szv_ishGridView.CurrentRow.Cells["ID"].Value != null)
            {
                long id = long.Parse(szv_ishGridView.CurrentRow.Cells["ID"].Value.ToString());

                FormsSZV_ISH_2017 szv_ish_data = db.FormsSZV_ISH_2017.FirstOrDefault(x => x.ID == id);

                ReportViewerODV1 = new ODV1_Print();
                ReportViewerODV1.SZV_ISH_List = new List<FormsSZV_ISH_2017>();
                ReportViewerODV1.SZV_ISH_List.Add(szv_ish_data);
                ReportViewerODV1.TypeForm = 2;
                ReportViewerODV1.Owner = this;
                ReportViewerODV1.ThemeName = this.ThemeName;
                ReportViewerODV1.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerODV1.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedODV1);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }

        }

        private void szv_korrAddBtn_Click(object sender, EventArgs e)
        {
            if (!szv_korrAddBtn.Enabled)
                return;

            if (odv1GridView.ChildRows.Count > 0 && odv1GridView.CurrentRow.Cells["ID"].Value != null && odv1GridView.CurrentRow.Index >= 0)
            {
                long id = Convert.ToInt64(odv1GridView.CurrentRow.Cells["ID"].Value);

                PU.FormsSZV_KORR.SZV_KORR_Edit child = new PU.FormsSZV_KORR.SZV_KORR_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.parentName = this.Name;
                child.ODV1 = db.FormsODV_1_2017.First(x => x.ID == id);
                child.Monthes = Monthes;
                child.ShowDialog();
                child.Dispose();
                db.DetectChanges();
                db = new pu6Entities();

                szv_korrGridView_upd();

            }
        }

        private void szv_korrEditBtn_Click(object sender, EventArgs e)
        {
            if (szv_korrGridView.RowCount > 0 && szv_korrGridView.CurrentRow.Cells[1].Value != null)
            {
                long id = Convert.ToInt64(odv1GridView.CurrentRow.Cells["ID"].Value);
                long id_szv_korr = Convert.ToInt64(szv_korrGridView.CurrentRow.Cells[1].Value);
                int rowindex = szv_korrGridView.CurrentRow.Index;

                PU.FormsSZV_KORR.SZV_KORR_Edit child = new PU.FormsSZV_KORR.SZV_KORR_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.SZV_KORR = db.FormsSZV_KORR_2017.FirstOrDefault(x => x.ID == id_szv_korr);
                child.action = "edit";
                child.parentName = this.Name;
                child.Monthes = Monthes;
                child.ShowDialog();
                child.Dispose();
                db.DetectChanges();
                db = new pu6Entities();
                szv_korrGridView_upd();
                if (rowindex >= 0)
                    szv_korrGridView.Rows[rowindex].IsCurrent = true;

            }
        }

        private void szv_korrDelBtn_Click(object sender, EventArgs e)
        {
            if (!szv_korrAddBtn.Enabled)
                return;

            if (szv_korrGridView.RowCount > 0 && szv_korrGridView.CurrentRow.Cells[1].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить текущую запись", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    string id = szv_korrGridView.CurrentRow.Cells[1].Value.ToString();

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_KORR_2017 WHERE ([ID] = {0})", id));
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении записи произошла ошибка! Код ошибки: " + ex.Message, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);
                    }
                    szv_korrGridView_upd();

                }
            }
        }

        private void printSZVKorrBtn_Click(object sender, EventArgs e)
        {
            if (szv_korrGridView.RowCount != 0 && szv_korrGridView.CurrentRow.Cells["ID"].Value != null)
            {
                long id = long.Parse(szv_korrGridView.CurrentRow.Cells["ID"].Value.ToString());

                FormsSZV_KORR_2017 szv_korr_data = db.FormsSZV_KORR_2017.FirstOrDefault(x => x.ID == id);

                ReportViewerODV1 = new ODV1_Print();
                ReportViewerODV1.SZV_KORR_List = new List<FormsSZV_KORR_2017>();
                ReportViewerODV1.SZV_KORR_List.Add(szv_korr_data);
                ReportViewerODV1.TypeForm = 3;
                ReportViewerODV1.Owner = this;
                ReportViewerODV1.ThemeName = this.ThemeName;
                ReportViewerODV1.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerODV1.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedODV1);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void szv_korrGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            szv_korrEditBtn_Click(null, null);
        }

        private void szv_ishGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            szv_ishEditBtn_Click(null, null);
        }


        private void copyToSzvKorrBtn_Click(object sender, EventArgs e)
        {
            if (szv_stajGridView.RowCount > 0 && szv_stajGridView.CurrentRow.Cells[1].Value != null)
            {
                long id = long.Parse(odv1GridView.CurrentRow.Cells[0].Value.ToString());

                PU.FormsSZV_STAJ.SZV_STAJ_CreateSZVKORR child = new PU.FormsSZV_STAJ.SZV_STAJ_CreateSZVKORR();
                child.Owner = this;
                child.ThemeName = this.ThemeName;


                if (szv_stajGridView.CurrentRow.Cells[1].Value != null)
                {
                    child.currentStaffId = long.Parse(szv_stajGridView.CurrentRow.Cells[1].Value.ToString());
                }

                List<long> ids = new List<long>();

                if (szv_stajGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {
                    foreach (var item in szv_stajGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                    {
                        ids.Add(long.Parse(item.Cells[1].Value.ToString()));
                    }

                    child.staffList_temp = ids;
                }

                child.odv1Data = db.FormsODV_1_2017.FirstOrDefault(x => x.ID == id);
                child.ShowDialog();
                if (child.Updated)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    odv1Grid_upd();
                    if (odv1GridView.Rows.Any(x => x.Cells["ID"].Value.ToString() == id.ToString()))
                        odv1GridView.Rows.First(x => x.Cells["ID"].Value.ToString() == id.ToString()).IsCurrent = true;
                }

            }
        }

        private void fillSzvStajBtn_Click(object sender, EventArgs e)
        {
            if (odv1GridView.RowCount > 0 && odv1GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(odv1GridView.CurrentRow.Cells[0].Value.ToString());

                PU.FormsSZV_STAJ.SZV_STAJ_FillStaff child = new PU.FormsSZV_STAJ.SZV_STAJ_FillStaff();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ODV1Data = db.FormsODV_1_2017.FirstOrDefault(x => x.ID == id);
                child.ShowDialog();
                if (child.Updated)
                {
                    db.DetectChanges();
                    db = new pu6Entities();
                    odv1Grid_upd();
                    if (odv1GridView.Rows.Any(x => x.Cells["ID"].Value.ToString() == id.ToString()))
                        odv1GridView.Rows.First(x => x.Cells["ID"].Value.ToString() == id.ToString()).IsCurrent = true;
                }

            }
        }

    }
}
