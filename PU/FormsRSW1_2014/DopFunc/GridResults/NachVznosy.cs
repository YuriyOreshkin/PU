using PU.Classes;
using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;
using System.Threading;
using System.Globalization;
using PU.Reports;

namespace PU.FormsRSW2014
{
    public partial class NachVznosy : Telerik.WinControls.UI.RadForm
    {
        public List<FormsRSW2014_1_Razd_6_1> rsw61List = new List<FormsRSW2014_1_Razd_6_1>();
        List<svedNachVznos_container> svedNachVznos_list = new List<svedNachVznos_container>();
        List<svedBaseOPS_container> svedBaseOPS_list = new List<svedBaseOPS_container>();
        List<svedVypl_container> svedVypl_list = new List<svedVypl_container>();
        List<svedDop_container> svedDop_list = new List<svedDop_container>();
        List<svedKorr_container> svedKorr_list = new List<svedKorr_container>();


        Font totalRowsFont;
        public string action { get; set; }
        public string Period { get; set; }
        public PlatCategory PlatCat { get; set; }


        GridViewSummaryItem summaryItemNum = new GridViewSummaryItem("Num", "{0}", GridAggregateFunction.Count);
        GridViewSummaryItem summaryItemBase_OPS_ALL = new GridViewSummaryItem("Base_OPS_ALL", "{0:N2}", GridAggregateFunction.Sum);
        GridViewSummaryItem summaryItemBase_OPS_GPD = new GridViewSummaryItem("Base_OPS_GPD", "{0:N2}", GridAggregateFunction.Sum);
        GridViewSummaryItem summaryItemBase_OPS_1M = new GridViewSummaryItem("Base_OPS_1M", "{0:N2}", GridAggregateFunction.Sum);
        GridViewSummaryItem summaryItemBase_OPS_2M = new GridViewSummaryItem("Base_OPS_2M", "{0:N2}", GridAggregateFunction.Sum);
        GridViewSummaryItem summaryItemBase_OPS_3M = new GridViewSummaryItem("Base_OPS_3M", "{0:N2}", GridAggregateFunction.Sum);
        GridViewSummaryItem summaryItemOPS = new GridViewSummaryItem("OPS", "{0:N2}", GridAggregateFunction.Sum);
        GridViewSummaryItem summaryItemPrevBase = new GridViewSummaryItem("PrevBase", "{0:N2}", GridAggregateFunction.Sum);
        GridViewSummaryItem summaryItemSTRAH = new GridViewSummaryItem("STRAH", "{0:N2}", GridAggregateFunction.Sum);
        GridViewSummaryItem summaryItemNAKOP = new GridViewSummaryItem("NAKOP", "{0:N2}", GridAggregateFunction.Sum);


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

        public NachVznosy()
        {
            totalRowsFont = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            InitializeComponent();
        }

        private void NachVznosy_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            this.Cursor = Cursors.WaitCursor;

            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();


            GridViewSummaryRowItem summaryRowItem = new GridViewSummaryRowItem { };
            switch (action)
            {
                case "svedNachVznos":
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn { Name = "Num", HeaderText = "#", VisibleInColumnChooser = false, Width = 20, AllowSort = false, AllowFiltering = false, DataType = typeof(int) });
                    radGridView1.AutoSizeRows = true;
                    radGridView1.MasterView.TableHeaderRow.MinHeight = 26;

                    summaryRowItem = new GridViewSummaryRowItem(
                        new GridViewSummaryItem[] { summaryItemNum, summaryItemBase_OPS_ALL, summaryItemBase_OPS_3M, summaryItemOPS });
                    dataGrid_upd_svedNachVznos();
            this.radGridView1.SummaryRowsTop.Add(summaryRowItem);
            this.radGridView1.MasterTemplate.ShowTotals = true;
            this.radGridView1.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;                    break;
                case "svedBaseOPS":
            radGridView1.MasterTemplate.Columns.Add(new GridViewDecimalColumn { Name = "Num", HeaderText = "#", VisibleInColumnChooser = false, Width = 20, AllowSort = false, AllowFiltering = false, DataType = typeof(int) });
                    summaryRowItem = new GridViewSummaryRowItem(
                        new GridViewSummaryItem[] { summaryItemNum, summaryItemBase_OPS_ALL, summaryItemBase_OPS_GPD, summaryItemBase_OPS_1M, summaryItemBase_OPS_2M, summaryItemBase_OPS_3M });

                    dataGrid_upd_svedBaseOPS();
            this.radGridView1.SummaryRowsTop.Add(summaryRowItem);
            this.radGridView1.MasterTemplate.ShowTotals = true;
            this.radGridView1.MasterView.SummaryRows[0].PinPosition = PinnedRowPosition.Bottom;                    break;
                case "svedVypl":
                    //summaryRowItem = new GridViewSummaryRowItem(
                    //    new GridViewSummaryItem[] { summaryItemNum, summaryItemBase_OPS_ALL, summaryItemOPS, summaryItemBase_OPS_GPD, summaryItemPrevBase });


                    dataGrid_upd_svedVypl();
                    break;
                case "svedDop":
                    //GridViewSummaryItem summaryItemSUM1 = new GridViewSummaryItem("SUM1", "{0}", GridAggregateFunction.Sum);
                    //GridViewSummaryItem summaryItemSUM2 = new GridViewSummaryItem("SUM2", "{0}", GridAggregateFunction.Sum);

                    //summaryRowItem = new GridViewSummaryRowItem(
                    //    new GridViewSummaryItem[] { summaryItemNum, summaryItemSUM1, summaryItemSUM2 });


                    dataGrid_upd_svedDop();
                    break;
                case "svedKorr":
                    summaryRowItem = new GridViewSummaryRowItem(
                        new GridViewSummaryItem[] { summaryItemNum, summaryItemOPS, summaryItemSTRAH, summaryItemNAKOP });


                    dataGrid_upd_svedKorr();
                    break;

            }


            if (radGridView1.RowCount > 0)
            {
                radGridView1.Rows.First().IsCurrent = true;
                this.radGridView1.GridNavigator.SelectFirstRow();
                radGridView1.MasterTemplate.CollapseAllGroups();
            }

            this.Cursor = Cursors.Default;
        }



        private void ClearGrouping() { this.radGridView1.GroupDescriptors.Clear(); }

        public void dataGrid_upd_svedNachVznos()
        {
            radGridView1.Rows.Clear();

            svedNachVznos_list = new List<svedNachVznos_container>();
//            rsw61List = rsw61List.OrderBy(x => x.Staff.LastName).ThenBy(x => x.Staff.FirstName).ThenBy(x => x.Staff.MiddleName).ToList();

            foreach (var item in rsw61List)
            {
                string contrNum = "";
                if (item.Staff.ControlNumber != null)
                {
                    contrNum = item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }

  //              i++;
                decimal s0 = item.FormsRSW2014_1_Razd_6_4.Where(x => x.s_0_1.HasValue).Sum(x => x.s_0_1.Value);
                decimal s1 = item.FormsRSW2014_1_Razd_6_4.Where(x => x.s_1_1.HasValue).Sum(x => x.s_1_1.Value) + item.FormsRSW2014_1_Razd_6_4.Where(x => x.s_2_1.HasValue).Sum(x => x.s_2_1.Value) + item.FormsRSW2014_1_Razd_6_4.Where(x => x.s_3_1.HasValue).Sum(x => x.s_3_1.Value);
                decimal s2 = item.SumFeePFR.HasValue ? item.SumFeePFR.Value : 0;

                svedNachVznos_list.Add(new svedNachVznos_container
                {
//                    Num = i,
                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                    SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                    Tabel = item.Staff.TabelNumber != null ? item.Staff.TabelNumber.Value.ToString() : "",
                    Base_OPS_ALL = s0,
                    Base_OPS_3M = s1,
                    OPS = s2
                });

            }
            radGridView1.DataSource = svedNachVznos_list;

            //radGridView1.Columns["Num"].HeaderText = "#";
            //radGridView1.Columns["Num"].VisibleInColumnChooser = false;
            radGridView1.Columns["Num"].Width = 40;
            //radGridView1.Columns["Num"].AllowSort = false;
            //radGridView1.Columns["Num"].AllowFiltering = false;
            //radGridView1.Columns["Num"].DataType = typeof(int);
            radGridView1.Columns["FIO"].HeaderText = "";
            radGridView1.Columns["FIO"].Width = 241;
            radGridView1.Columns["FIO"].WrapText = true;
            radGridView1.Columns["SNILS"].HeaderText = "";
            radGridView1.Columns["SNILS"].Width = 105;
            radGridView1.Columns["SNILS"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radGridView1.Columns["Tabel"].HeaderText = "№ Табельный";
            radGridView1.Columns["Tabel"].Width = 90;
            radGridView1.Columns["Tabel"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["Base_OPS_ALL"].HeaderText = "База на ОПС с нач.года";
            radGridView1.Columns["Base_OPS_ALL"].Width = 140;
            radGridView1.Columns["Base_OPS_ALL"].FormatString = "{0:N2}";
            radGridView1.Columns["Base_OPS_ALL"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["Base_OPS_3M"].HeaderText = "База на ОПС за посл. 3 мес.";
            radGridView1.Columns["Base_OPS_3M"].Width = 166;
            radGridView1.Columns["Base_OPS_3M"].FormatString = "{0:N2}";
            radGridView1.Columns["Base_OPS_3M"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["OPS"].HeaderText = "Начислено взносов на ОПС";
            radGridView1.Columns["OPS"].Width = 166;
            radGridView1.Columns["OPS"].FormatString = "{0:N2}";
            radGridView1.Columns["OPS"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;

        }



        public void dataGrid_upd_svedBaseOPS()
        {
            radGridView1.Rows.Clear();

            int i = 0;
            svedBaseOPS_list = new List<svedBaseOPS_container>();

            foreach (var item in rsw61List)
            {
                string contrNum = "";
                if (item.Staff.ControlNumber != null)
                {
                    contrNum = item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }

//                i++;
                foreach (var rsw64 in item.FormsRSW2014_1_Razd_6_4)
                {
                    if (PlatCat != null && rsw64.PlatCategoryID != PlatCat.ID)
                    {
                        break;
                    }

                    decimal s0 = rsw64.s_0_1.HasValue ? rsw64.s_0_1.Value : 0;
                    decimal s1 = rsw64.s_0_2.HasValue ? rsw64.s_0_2.Value : 0;
                    decimal s2 = rsw64.s_1_1.HasValue ? rsw64.s_1_1.Value : 0;
                    decimal s3 = rsw64.s_2_1.HasValue ? rsw64.s_2_1.Value : 0;
                    decimal s4 = rsw64.s_3_1.HasValue ? rsw64.s_3_1.Value : 0;

                    svedBaseOPS_list.Add(new svedBaseOPS_container
                    { 
  //                  Num = i,
                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                    SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                    PlatCat = "Категория Плательщика: [" + rsw64.PlatCategory.Code + "]",
                    Base_OPS_ALL = s0,
                    Base_OPS_GPD = s1,
                    Base_OPS_1M = s2,
                    Base_OPS_2M = s3,
                    Base_OPS_3M = s4
                    });

                }
            }

            radGridView1.DataSource = svedBaseOPS_list;

            //radGridView1.Columns["Num"].HeaderText = "#";
            //radGridView1.Columns["Num"].VisibleInColumnChooser = false;
            radGridView1.Columns["Num"].Width = 40;
            //radGridView1.Columns["Num"].AllowSort = false;
            //radGridView1.Columns["Num"].AllowFiltering = false;
            //radGridView1.Columns["Num"].DataType = typeof(int);
            radGridView1.Columns["FIO"].HeaderText = "";
            radGridView1.Columns["FIO"].Width = 241;
            radGridView1.Columns["FIO"].WrapText = true;
            radGridView1.Columns["SNILS"].HeaderText = "";
            radGridView1.Columns["SNILS"].Width = 105;
            radGridView1.Columns["SNILS"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radGridView1.Columns["PlatCat"].HeaderText = "Категория";
            radGridView1.Columns["PlatCat"].Width = 170;
            radGridView1.Columns["PlatCat"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["Base_OPS_ALL"].HeaderText = "База на ОПС с нач.года";
            radGridView1.Columns["Base_OPS_ALL"].Width = 140;
            radGridView1.Columns["Base_OPS_ALL"].FormatString = "{0:N2}";
            radGridView1.Columns["Base_OPS_ALL"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["Base_OPS_GPD"].HeaderText = "в т.ч. по ГПД";
            radGridView1.Columns["Base_OPS_GPD"].Width = 140;
            radGridView1.Columns["Base_OPS_GPD"].FormatString = "{0:N2}";
            radGridView1.Columns["Base_OPS_GPD"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["Base_OPS_1M"].HeaderText = "1 месяц";
            radGridView1.Columns["Base_OPS_1M"].Width = 130;
            radGridView1.Columns["Base_OPS_1M"].FormatString = "{0:N2}";
            radGridView1.Columns["Base_OPS_1M"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["Base_OPS_2M"].HeaderText = "2 месяц";
            radGridView1.Columns["Base_OPS_2M"].Width = 130;
            radGridView1.Columns["Base_OPS_2M"].FormatString = "{0:N2}";
            radGridView1.Columns["Base_OPS_2M"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["Base_OPS_3M"].HeaderText = "3 месяц";
            radGridView1.Columns["Base_OPS_3M"].Width = 130;
            radGridView1.Columns["Base_OPS_3M"].FormatString = "{0:N2}";
            radGridView1.Columns["Base_OPS_3M"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;

            this.radGridView1.EnableGrouping = true;
            this.ClearGrouping();
            radGridView1.ShowGroupPanel = false;
            radGridView1.EnableHotTracking = false;

            this.radGridView1.GroupDescriptors.Add(new GridGroupByExpression("FIO as FIO format \"{0}: {1}\" Group By FIO, SNILS", "{1}"));

        }



        public void dataGrid_upd_svedVypl()
        {
            radGridView1.Rows.Clear();

            int i = 0;

            svedVypl_list = new List<svedVypl_container>();

            int count = rsw61List.Count().ToString().Length;

            foreach (var item in rsw61List)
            {
                string contrNum = "";
                if (item.Staff.ControlNumber != null)
                {
                    contrNum = item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }
                decimal[] s2 = new decimal[4] { 0, 0, 0, 0 };
                decimal[] s3 = new decimal[4] { 0, 0, 0, 0 };
                i++;

                foreach (var rsw64 in item.FormsRSW2014_1_Razd_6_4)
                {
                    if (PlatCat != null && rsw64.PlatCategoryID != PlatCat.ID)
                    {
                        break;
                    }



                    for (int k = 1; k <= 3; k++)
                    {
                        decimal[] s = new decimal[4];
                        for (int j = 0; j <= 3; j++)
                        {
                            var properties = rsw64.GetType().GetProperty("s_" + k + "_" + j);
                            object value = properties.GetValue(rsw64, null);
                            s[j] = value != null ? decimal.Parse(value.ToString()) : 0;
                            s3[j] += s[j];

                        }

                        svedVypl_list.Add(new svedVypl_container
                        {
                            Number = "[" + i.ToString().PadLeft(count, '0') + "] ",
                            FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                            SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                            PlatCat = k.ToString() + " месяц: [" + rsw64.PlatCategory.Code + "]",
                            Base_OPS_ALL = s[0],
                            OPS = s[1],
                            Base_OPS_GPD = s[2],
                            PrevBase = s[3]
                        });


                    }


                    for (int j = 0; j <= 3; j++)
                    {
                        var properties = rsw64.GetType().GetProperty("s_0" + "_" + j);
                        object value = properties.GetValue(rsw64, null);
                        s2[j] = value != null ? (s2[j] + decimal.Parse(value.ToString())) : s2[j];
                    }

                }

                svedVypl_list.Add(new svedVypl_container
                {
                    Number = "[" + i.ToString().PadLeft(count, '0') + "] ",
                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                    SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                    PlatCat = "Всего за отчетный период",
                    Base_OPS_ALL = s3[0],
                    OPS = s3[1],
                    Base_OPS_GPD = s3[2],
                    PrevBase = s3[3]
                });

                svedVypl_list.Add(new svedVypl_container
                {
                    Number = "[" + i.ToString().PadLeft(count, '0') + "] ",
                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                    SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                    PlatCat = "Итого с начала расчетного периода",
                    Base_OPS_ALL = s2[0],
                    OPS = s2[1],
                    Base_OPS_GPD = s2[2],
                    PrevBase = s2[3]
                });

            }
            i++;
            string name = "Всего за отчетный период";
            for (int n = 1; n <= 2; n++)
            {
                svedVypl_list.Add(new svedVypl_container
                {
                    Number = "[" + i.ToString().PadLeft(count, '0') + "] ",
                    FIO = "",
                    SNILS = "ИТОГО",
                    PlatCat = name,
                    Base_OPS_ALL = svedVypl_list.Where(x => x.PlatCat == name).Sum(x => x.Base_OPS_ALL),
                    OPS = svedVypl_list.Where(x => x.PlatCat == name).Sum(x => x.OPS),
                    Base_OPS_GPD = svedVypl_list.Where(x => x.PlatCat == name).Sum(x => x.Base_OPS_GPD),
                    PrevBase = svedVypl_list.Where(x => x.PlatCat == name).Sum(x => x.PrevBase),
                });
                name = "Итого с начала расчетного периода";
            }

            radGridView1.DataSource = svedVypl_list;

            radGridView1.Columns["Number"].HeaderText = "";
            radGridView1.Columns["Number"].VisibleInColumnChooser = false;
            radGridView1.Columns["Number"].Width = 40;
            radGridView1.Columns["Number"].AllowSort = false;
            radGridView1.Columns["Number"].AllowFiltering = false;
            radGridView1.Columns["Number"].DataType = typeof(int);
            radGridView1.Columns["FIO"].HeaderText = "";
            radGridView1.Columns["FIO"].Width = 241;
            radGridView1.Columns["FIO"].WrapText = true;
            radGridView1.Columns["SNILS"].HeaderText = "";
            radGridView1.Columns["SNILS"].Width = 105;
            radGridView1.Columns["SNILS"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radGridView1.Columns["PlatCat"].HeaderText = "Категория";
            radGridView1.Columns["PlatCat"].Width = 170;
            radGridView1.Columns["PlatCat"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["Base_OPS_ALL"].HeaderText = "Сумма выплат";
            radGridView1.Columns["Base_OPS_ALL"].Width = 140;
            radGridView1.Columns["Base_OPS_ALL"].FormatString = "{0:N2}";
            radGridView1.Columns["Base_OPS_ALL"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["OPS"].HeaderText = "в т.ч. база на ОПС";
            radGridView1.Columns["OPS"].Width = 140;
            radGridView1.Columns["OPS"].FormatString = "{0:N2}";
            radGridView1.Columns["OPS"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["Base_OPS_GPD"].HeaderText = "в т.ч. по ГПД";
            radGridView1.Columns["Base_OPS_GPD"].Width = 140;
            radGridView1.Columns["Base_OPS_GPD"].FormatString = "{0:N2}";
            radGridView1.Columns["Base_OPS_GPD"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["PrevBase"].HeaderText = "Суммы прев. предельную";
            radGridView1.Columns["PrevBase"].Width = 130;
            radGridView1.Columns["PrevBase"].FormatString = "{0:N2}";
            radGridView1.Columns["PrevBase"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;


            this.radGridView1.EnableGrouping = true;
            this.ClearGrouping();
            radGridView1.ShowGroupPanel = false;
            radGridView1.EnableHotTracking = false;
            this.radGridView1.GroupDescriptors.Add(new GridGroupByExpression("FIO as FIO format \"{0}: <strong>{1}</strong>\" Group By Number, FIO, SNILS", "{1}"));

        }


        public void dataGrid_upd_svedDop()
        {
            radGridView1.Rows.Clear();

            int i = 0;

            svedDop_list = new List<svedDop_container>();
            int count = rsw61List.Count().ToString().Length;

            foreach (var item in rsw61List)
            {
                string contrNum = "";
                if (item.Staff.ControlNumber != null)
                {
                    contrNum = item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }
                decimal[] s2 = new decimal[2] { 0, 0 };
                decimal[] s3 = new decimal[2] { 0, 0 };

                i++;
                foreach (var rsw67 in item.FormsRSW2014_1_Razd_6_7)
                {
                    for (int k = 1; k <= 3; k++)
                    {
                        decimal[] s = new decimal[2];
                        for (int j = 0; j <= 1; j++)
                        {
                            var properties = rsw67.GetType().GetProperty("s_" + k + "_" + j);
                            object value = properties.GetValue(rsw67, null);
                            s[j] = value != null ? decimal.Parse(value.ToString()) : 0;
                            s3[j] += s[j];
                        }

                        svedDop_list.Add(new svedDop_container
                        {
                            Number = "[" + i.ToString().PadLeft(count, '0') + "] ",
                            FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                            SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                            DopTar = k.ToString() + " месяц: [" + (rsw67.SpecOcenkaUslTrudaID.HasValue ? rsw67.SpecOcenkaUslTruda.Code : " ") + "]",
                            SUM1 = s[0],
                            SUM2 = s[1]
                        });

                    }

                    for (int j = 0; j <= 1; j++)
                    {
                        var properties = rsw67.GetType().GetProperty("s_0" + "_" + j);
                        object value = properties.GetValue(rsw67, null);
                        s2[j] = value != null ? (s2[j] + decimal.Parse(value.ToString())) : s2[j];
                    }
                }

                svedDop_list.Add(new svedDop_container
                {
                    Number = "[" + i.ToString().PadLeft(count, '0') + "] ",
                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                    SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                    DopTar = "Всего за отчетный период",
                    SUM1 = s3[0],
                    SUM2 = s3[1]
                });

                svedDop_list.Add(new svedDop_container
                {
                    Number = "[" + i.ToString().PadLeft(count, '0') + "] ",
                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                    SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                    DopTar = "Итого с начала расчетного периода",
                    SUM1 = s2[0],
                    SUM2 = s2[1]
                });

            }

            i++;
            string name = "Всего за отчетный период";

            for (int n = 1; n <= 2; n++)
            {
                svedDop_list.Add(new svedDop_container
                {
                    Number = "[" + i.ToString().PadLeft(count, '0') + "] ",
                    FIO = "",
                    SNILS = "ИТОГО",
                    DopTar = name,
                    SUM1 = svedDop_list.Where(x => x.DopTar == name).Sum(x => x.SUM1),
                    SUM2 = svedDop_list.Where(x => x.DopTar == name).Sum(x => x.SUM2),
                });
                name = "Итого с начала расчетного периода";
            }
            radGridView1.DataSource = svedDop_list;

            radGridView1.Columns["Number"].HeaderText = "";
            radGridView1.Columns["Number"].VisibleInColumnChooser = false;
            radGridView1.Columns["Number"].Width = 40;
            radGridView1.Columns["Number"].AllowSort = false;
            radGridView1.Columns["Number"].AllowFiltering = false;
            radGridView1.Columns["Number"].DataType = typeof(int);
            radGridView1.Columns["FIO"].HeaderText = "";
            radGridView1.Columns["FIO"].Width = 241;
            radGridView1.Columns["FIO"].WrapText = true;
            radGridView1.Columns["SNILS"].HeaderText = "";
            radGridView1.Columns["SNILS"].Width = 105;
            radGridView1.Columns["SNILS"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radGridView1.Columns["DopTar"].HeaderText = "Доп. тариф";
            radGridView1.Columns["DopTar"].Width = 170;
            radGridView1.Columns["DopTar"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["SUM1"].HeaderText = "по пп.1 п.1 ст.27";
            radGridView1.Columns["SUM1"].Width = 140;
            radGridView1.Columns["SUM1"].FormatString = "{0:N2}";
            radGridView1.Columns["SUM1"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["SUM2"].HeaderText = "по пп.2-28 п.1 ст.27";
            radGridView1.Columns["SUM2"].Width = 140;
            radGridView1.Columns["SUM2"].FormatString = "{0:N2}";
            radGridView1.Columns["SUM2"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;


            this.radGridView1.EnableGrouping = true;
            this.ClearGrouping();
            radGridView1.ShowGroupPanel = false;
            radGridView1.EnableHotTracking = false;
            this.radGridView1.GroupDescriptors.Add(new GridGroupByExpression("FIO as FIO format \"{0}: <strong>{1}</strong>\" Group By Number, FIO, SNILS", "{1}"));


        }


        public void dataGrid_upd_svedKorr()
        {
            radGridView1.Rows.Clear();

            int i = 0;

            List<RaschetPeriodContainer> RaschPer = new List<RaschetPeriodContainer> { };
            foreach (var item in Options.RaschetPeriodInternal2010_2013)
            {
                RaschPer.Add(item);
            }
            foreach (var item in Options.RaschetPeriodInternal)
            {
                RaschPer.Add(item);
            }

            svedKorr_list = new List<svedKorr_container>();

            rsw61List = rsw61List.OrderBy(x => x.Staff.LastName).ThenBy(x => x.Staff.FirstName).ThenBy(x => x.Staff.MiddleName).ToList();

            foreach (var item in rsw61List)
            {
                string contrNum = "";
                if (item.Staff.ControlNumber != null)
                {
                    contrNum = item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }

                i++;
                foreach (var rsw66 in item.FormsRSW2014_1_Razd_6_6)
                {
                    RaschetPeriodContainer rp = RaschPer.FirstOrDefault(x => x.Year == rsw66.AccountPeriodYear && x.Kvartal == rsw66.AccountPeriodQuarter);

                    decimal s0 = rsw66.SumFeePFR_D.HasValue ? rsw66.SumFeePFR_D.Value : 0;
                    decimal s1 = rsw66.SumFeePFR_StrahD.HasValue ? rsw66.SumFeePFR_StrahD.Value : 0;
                    decimal s2 = rsw66.SumFeePFR_NakopD.HasValue ? rsw66.SumFeePFR_NakopD.Value : 0;

                    svedKorr_list.Add(new svedKorr_container
                    {
                        Num = i,
                        FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                        SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                        Period = rp != null ? (rp.Kvartal + " - " + rp.Name) : "Период не определен",
                        OPS = s0,
                        STRAH = s1,
                        NAKOP = s2
                    });
                }
            }

            radGridView1.DataSource = svedKorr_list;

            radGridView1.Columns["Num"].HeaderText = "#";
            radGridView1.Columns["Num"].VisibleInColumnChooser = false;
            radGridView1.Columns["Num"].Width = 40;
            radGridView1.Columns["Num"].AllowSort = false;
            radGridView1.Columns["Num"].AllowFiltering = false;
            radGridView1.Columns["Num"].DataType = typeof(int);
            radGridView1.Columns["FIO"].HeaderText = "";
            radGridView1.Columns["FIO"].Width = 241;
            radGridView1.Columns["FIO"].WrapText = true;
            radGridView1.Columns["SNILS"].HeaderText = "";
            radGridView1.Columns["SNILS"].Width = 105;
            radGridView1.Columns["SNILS"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radGridView1.Columns["Period"].HeaderText = "Категория";
            radGridView1.Columns["Period"].Width = 170;
            radGridView1.Columns["Period"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["OPS"].HeaderText = "на ОПС c 2014 года";
            radGridView1.Columns["OPS"].Width = 140;
            radGridView1.Columns["OPS"].FormatString = "{0:N2}";
            radGridView1.Columns["OPS"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["STRAH"].HeaderText = "на страховую часть";
            radGridView1.Columns["STRAH"].Width = 140;
            radGridView1.Columns["STRAH"].FormatString = "{0:N2}";
            radGridView1.Columns["STRAH"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            radGridView1.Columns["NAKOP"].HeaderText = "на накопительную часть";
            radGridView1.Columns["NAKOP"].Width = 140;
            radGridView1.Columns["NAKOP"].FormatString = "{0:N2}";
            radGridView1.Columns["NAKOP"].TextAlignment = System.Drawing.ContentAlignment.MiddleRight;

            this.radGridView1.EnableGrouping = true;
            this.ClearGrouping();
            radGridView1.ShowGroupPanel = false;
            radGridView1.EnableHotTracking = false;
            this.radGridView1.GroupDescriptors.Add(new GridGroupByExpression("FIO as FIO format \"{0}: {1}\" Group By FIO, SNILS", "{1}"));
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            switch (action)
            {
                case "svedNachVznos":
                    if (svedNachVznos_list.Any())
                        ReportMethods.Print_svedNachVznos(svedNachVznos_list, Period, this.ThemeName);
                    break;
                case "svedBaseOPS":
                    if (svedBaseOPS_list.Any())
                        ReportMethods.Print_svedBaseOPS(svedBaseOPS_list, Period, this.ThemeName);
                    break;
                case "svedVypl":
                    if (svedVypl_list.Any())
                        ReportMethods.Print_svedVypl(svedVypl_list, Period, this.ThemeName);
                    break;
                case "svedDop":
                    if (svedDop_list.Any())
                        ReportMethods.Print_svedDop(svedDop_list, Period, this.ThemeName);
                    break;
                case "svedKorr":
                    if (svedKorr_list.Any())
                        ReportMethods.Print_svedKorr(svedKorr_list, Period, this.ThemeName);
                    break;

            }




////            this.radPrintDocument1.RightFooter = "[Time Printed] [Date Printed]";
//            this.radPrintDocument1.MiddleFooter = "Стр. [Page #] из [Total Pages]";
//            this.radPrintDocument1.MiddleHeader = this.Text;
//            this.radGridView1.PrintStyle.PrintSummaries = true;
//            //   this.radGridView1.PrintStyle.PrintHeaderOnEachPage = true;
//            //            this.radGridView1.PrintStyle.FitWidthMode = PrintFitWidthMode.NoFitCentered;

//            RadPrintPreviewDialog dialog = new RadPrintPreviewDialog();
//            dialog.ThemeName = this.radGridView1.ThemeName;
//            dialog.Document = this.radPrintDocument1;
//            //dialog.Document.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(60, 60, 40, 40);
//            dialog.StartPosition = FormStartPosition.CenterScreen;
//            dialog.WindowState = FormWindowState.Maximized;
//            dialog.ShowDialog();

        }

        private void radGridView1_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.RowInfo.Group == null && e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.TextAlignment = ContentAlignment.BottomRight;
                e.CellElement.Font = totalRowsFont;
            }

            if (e.CellElement.ColumnInfo != null && e.CellElement.ColumnInfo.Name == "Num" && string.IsNullOrEmpty(e.CellElement.Text))
            {
                e.CellElement.Text = (e.CellElement.RowIndex + 1).ToString();
            }
        }

        private void collapseAllBtn_Click(object sender, EventArgs e)
        {
            radGridView1.MasterTemplate.CollapseAllGroups();
        }

        private void expandAllBtn_Click(object sender, EventArgs e)
        {
            radGridView1.MasterTemplate.ExpandAllGroups();
        }

        private void NachVznosy_FormClosing(object sender, FormClosingEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
        }





    }
}
