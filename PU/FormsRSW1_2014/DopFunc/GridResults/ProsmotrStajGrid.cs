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

namespace PU.FormsRSW2014
{
    public partial class ProsmotrStajGrid : Telerik.WinControls.UI.RadForm
    {
        public List<FormsRSW2014_1_Razd_6_1> rsw61List = new List<FormsRSW2014_1_Razd_6_1>();
        pu6Entities db = new pu6Entities();
        Font totalRowsFont;

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

        public ProsmotrStajGrid()
        {
            totalRowsFont = new Font("Segoe UI", 9.0f, FontStyle.Bold);
            InitializeComponent();
        }

        private void ClearGrouping() { this.radGridView1.GroupDescriptors.Clear(); }

        private void expandAllBtn_Click(object sender, EventArgs e)
        {
            radGridView1.MasterTemplate.ExpandAllGroups();
        }

        private void collapseAllBtn_Click(object sender, EventArgs e)
        {
            radGridView1.MasterTemplate.CollapseAllGroups();
        }

        private void radGridView1_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.RowInfo.Group == null && e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.TextAlignment = ContentAlignment.BottomRight;
                e.CellElement.Font = totalRowsFont;
            }
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            this.radPrintDocument1.RightFooter = "[Time Printed] [Date Printed]";
            this.radPrintDocument1.MiddleFooter = "Стр. [Page #] из [Total Pages]";
            this.radPrintDocument1.MiddleHeader = this.Text;
            this.radGridView1.PrintStyle.PrintSummaries = true;
            //   this.radGridView1.PrintStyle.PrintHeaderOnEachPage = true;
            //            this.radGridView1.PrintStyle.FitWidthMode = PrintFitWidthMode.NoFitCentered;

            RadPrintPreviewDialog dialog = new RadPrintPreviewDialog();
            dialog.ThemeName = this.radGridView1.ThemeName;
            dialog.Document = this.radPrintDocument1;
            //dialog.Document.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(60, 60, 40, 40);
            dialog.StartPosition = FormStartPosition.CenterScreen;
            dialog.WindowState = FormWindowState.Maximized;
            dialog.ShowDialog();
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ProsmotrStajGrid_Load(object sender, EventArgs e)
        {

            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            this.Cursor = Cursors.WaitCursor;

            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            dataGrid_upd();


            this.Cursor = Cursors.Default;
        }

        private class Staj_container
        {
            public long ID { get; set; }
            public string FIO { get; set; }
            public string SNILS { get; set; }
            public string Period { get; set; }
            public string TerrUslCode { get; set; }
            public string TerrUslKoef { get; set; }
            public string OsobUslCode { get; set; }
            public string KodVredOsn { get; set; }
            public string IschislStrahOsn { get; set; }
            public string IschislStrahDop { get; set; }
            public string UslDosrNaznOsn { get; set; }
            public string UslDosrNaznDop { get; set; }
        }
        public void dataGrid_upd()
        {
            radGridView1.Rows.Clear();
            int i = 0;

            List<Staj_container> list = new List<Staj_container>();

            foreach (var item in rsw61List)
            {
                string contrNum = "";
                if (item.Staff.ControlNumber != null)
                {
                    contrNum = item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }
                i++;
                if (item.StajOsn.Count > 0)
                {
                    foreach (var stajosn in item.StajOsn)
                    {
                        if (stajosn.StajLgot.Count > 0)
                        {
                            foreach (var stajlgot in stajosn.StajLgot)
                            {
                                string str = stajlgot.IschislStrahStajDopID == null ? "" : stajlgot.IschislStrahStajDopID.HasValue ? db.IschislStrahStajDop.FirstOrDefault(x => x.ID == stajlgot.IschislStrahStajDopID).Code : "";
                                string s1 = stajlgot.Strah1Param.HasValue == true ? stajlgot.Strah1Param.Value.ToString() : "0";
                                string s2 = stajlgot.Strah2Param.HasValue == true ? stajlgot.Strah2Param.Value.ToString() : "0";

                                str = "[" + s1 + "][" + s2 + "][" + str + "]";

                                string s1_ = stajlgot.UslDosrNazn1Param.HasValue == true ? stajlgot.UslDosrNazn1Param.Value.ToString() : "0";
                                string s2_ = stajlgot.UslDosrNazn2Param.HasValue == true ? stajlgot.UslDosrNazn2Param.Value.ToString() : "0";
                                string s3_ = stajlgot.UslDosrNazn3Param.HasValue == true ? stajlgot.UslDosrNazn3Param.Value.ToString() : "0.00";

                                string str_ = "[" + s1_ + "][" + s2_ + "][" + s3_ + "]";

                                list.Add(new Staj_container
                                {
                                    ID = item.ID,
                                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                                    SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                                    Period = ("[ " + (stajosn.Number.HasValue ? (stajosn.Number.Value.ToString()) : "") + " ]  ") + (stajosn.DateBegin.HasValue ? stajosn.DateBegin.Value.ToShortDateString() : "") + " - " + (stajosn.DateEnd.HasValue ? stajosn.DateEnd.Value.ToShortDateString() : ""),
                                    TerrUslCode = stajlgot.TerrUslID.HasValue ? stajlgot.TerrUsl.Code : "",
                                    TerrUslKoef = stajlgot.TerrUslKoef.HasValue ? stajlgot.TerrUslKoef.Value.ToString() : "",
                                    OsobUslCode = stajlgot.OsobUslTrudaID.HasValue ? stajlgot.OsobUslTruda.Code : "",
                                    KodVredOsn = stajlgot.KodVred_OsnID.HasValue ? stajlgot.KodVred_2.Code : "",
                                    IschislStrahOsn = stajlgot.IschislStrahStajOsnID == null ? "" : stajlgot.IschislStrahStajOsnID.HasValue ? db.IschislStrahStajOsn.FirstOrDefault(x => x.ID == stajlgot.IschislStrahStajOsnID).Code : "",
                                    IschislStrahDop = str,
                                    UslDosrNaznOsn = stajlgot.UslDosrNaznID == null ? "" : stajlgot.UslDosrNaznID.HasValue ? db.UslDosrNazn.FirstOrDefault(x => x.ID == stajlgot.UslDosrNaznID).Code : "",
                                    UslDosrNaznDop = str_
                                });
                            }
                        }
                        else
                        {
                            list.Add(new Staj_container
                            {
                                ID = item.ID,
                                FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                                SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : "",
                                Period = ("[ " + (stajosn.Number.HasValue ? (stajosn.Number.Value.ToString()) : "") + " ]  ") + (stajosn.DateBegin.HasValue ? stajosn.DateBegin.Value.ToShortDateString() : "") + " - " + (stajosn.DateEnd.HasValue ? stajosn.DateEnd.Value.ToShortDateString() : "")
                            });

                        }

                    }
                }
                else
                {
                    list.Add(new Staj_container
                    {
                        FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                        SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? " [" + item.Staff.InsuranceNumber.Substring(0, 3) + "-" + item.Staff.InsuranceNumber.Substring(3, 3) + "-" + item.Staff.InsuranceNumber.Substring(6, 3) + " " + contrNum + "]" : ""
                    });

                }

            }

            radGridView1.DataSource = list;

            radGridView1.Columns["ID"].HeaderText = "";
            radGridView1.Columns["ID"].IsVisible = false;
            radGridView1.Columns["ID"].Width = 2;
            radGridView1.Columns["ID"].AllowSort = false;
            radGridView1.Columns["ID"].AllowFiltering = false;
            radGridView1.Columns["FIO"].HeaderText = "";
            radGridView1.Columns["FIO"].Width = 241;
            radGridView1.Columns["FIO"].WrapText = true;
            radGridView1.Columns["SNILS"].HeaderText = "";
            radGridView1.Columns["SNILS"].Width = 105;
            radGridView1.Columns["SNILS"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radGridView1.Columns["Period"].HeaderText = "";
            radGridView1.Columns["Period"].Width = 170;
            radGridView1.Columns["Period"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["TerrUslCode"].HeaderText = "Код терр. усл.";
            radGridView1.Columns["TerrUslCode"].Width = 90;
            radGridView1.Columns["TerrUslCode"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["TerrUslKoef"].HeaderText = "Ставка";
            radGridView1.Columns["TerrUslKoef"].Width = 50;
            radGridView1.Columns["TerrUslKoef"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["OsobUslCode"].HeaderText = "Код особ. усл.";
            radGridView1.Columns["OsobUslCode"].Width = 100;
            radGridView1.Columns["OsobUslCode"].WrapText = false;
            radGridView1.Columns["OsobUslCode"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["KodVredOsn"].HeaderText = "Код позиц. списка";
            radGridView1.Columns["KodVredOsn"].Width = 100;
            radGridView1.Columns["KodVredOsn"].WrapText = true;
            radGridView1.Columns["KodVredOsn"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            radGridView1.Columns["IschislStrahOsn"].HeaderText = "Исчисл.страх.ст. основание";
            radGridView1.Columns["IschislStrahOsn"].Width = 100;
            radGridView1.Columns["IschislStrahOsn"].WrapText = true;
            radGridView1.Columns["IschislStrahOsn"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radGridView1.Columns["IschislStrahDop"].HeaderText = "Доп.сведения";
            radGridView1.Columns["IschislStrahDop"].Width = 112;
            radGridView1.Columns["IschislStrahDop"].WrapText = true;
            radGridView1.Columns["IschislStrahDop"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radGridView1.Columns["UslDosrNaznOsn"].HeaderText = "Усл. досрочной труд. пенсии";
            radGridView1.Columns["UslDosrNaznOsn"].Width = 100;
            radGridView1.Columns["UslDosrNaznOsn"].WrapText = true;
            radGridView1.Columns["UslDosrNaznOsn"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radGridView1.Columns["UslDosrNaznDop"].HeaderText = "Доп.сведения";
            radGridView1.Columns["UslDosrNaznDop"].Width = 112;
            radGridView1.Columns["UslDosrNaznDop"].WrapText = true;
            radGridView1.Columns["UslDosrNaznDop"].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;

            this.radGridView1.EnableGrouping = true;
            this.ClearGrouping();
            radGridView1.ShowGroupPanel = false;
            radGridView1.EnableHotTracking = false;
            this.radGridView1.GroupDescriptors.Add(new GridGroupByExpression("FIO as FIO format \"{0}: {1}\" Group By FIO, SNILS", "{1}"));
            this.radGridView1.GroupDescriptors.Add(new GridGroupByExpression("Period as Period format \"{0}: {1}\" Group By Period", "{1}"));

            if (radGridView1.RowCount > 0)
            {
                radGridView1.Rows.First().IsCurrent = true;
                this.radGridView1.GridNavigator.SelectFirstRow();
                radGridView1.MasterTemplate.CollapseAllGroups();
            }
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            radButton1_Click(null, null);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount > 0 && radGridView1.CurrentRow.Cells["ID"].Value != null && radGridView1.CurrentRow.Cells["ID"].Value.ToString() != "0")
            {
                long id_rsw = Convert.ToInt64(radGridView1.CurrentRow.Cells["ID"].Value);
                var rsw_6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.ID == id_rsw);
                RSW2014_6 child = new RSW2014_6();
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.RSW_6 = rsw_6;
                child.period = rsw_6.Quarter;
                child.action = "edit";
                child.parentName = "RSW2014_Edit";
                child.defaultPage = 4;
                child.ShowDialog();

                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();

                List<long> ids = rsw61List.Select(c => c.ID).ToList();

                rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => ids.Contains(x.ID)).ToList();

                dataGrid_upd();


            }


        }


    }
}
