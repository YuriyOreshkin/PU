using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using System.Linq;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;
using System.Xml.Linq;
using PU.Classes;
using System.IO;
using System.Globalization;
using PU.FormsSPW2_2014;
using PU.FormsDSW3;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Xml;

namespace PU.FormsRSW2014
{
    public partial class rsw2014packs : Telerik.WinControls.UI.RadForm
    {


        RSW2014_Print ReportViewerRSW2014;
        SPW2_Print ReportViewerSPW2;
        DSW3_Print ReportViewerDSW3;
        PU.FormsADW1.FormsADW1_Print ReportViewerADW1;
        PU.FormsADW2.FormsADW2_Print ReportViewerADW2;
        PU.FormsADW3.FormsADW3_Print ReportViewerADW3;

        pu6Entities db = new pu6Entities();
        pu6Entities db_temp_print;
        pfrXMLEntities dbxml = new pfrXMLEntities();
        public rsw2014PackIdent ident = new rsw2014PackIdent();
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        ADV_Print ReportViewerADV;
        Staff_Print ReportViewerStaff;
        XNamespace pfr = "http://schema.pfr.ru";

        List<long> idList = new List<long>();
        public List<string> FileInfoList = new List<string>();// Список файлов для передачи в окно проверки

        private FormsRSW2014_1_1 RSWdata;

        private List<FormsRSW2014_1_Razd_2_1> RSW_2_1_List = new List<FormsRSW2014_1_Razd_2_1>();
        private List<FormsRSW2014_1_Razd_2_4> RSW_2_4_List = new List<FormsRSW2014_1_Razd_2_4>();
        private List<FormsRSW2014_1_Razd_2_5_1> RSW_2_5_1_List = new List<FormsRSW2014_1_Razd_2_5_1>();
        private List<FormsRSW2014_1_Razd_2_5_2> RSW_2_5_2_List = new List<FormsRSW2014_1_Razd_2_5_2>();
        private List<FormsRSW2014_1_Razd_3_4> RSW_3_4_List = new List<FormsRSW2014_1_Razd_3_4>();
        private List<FormsRSW2014_1_Razd_4> RSW_4_List = new List<FormsRSW2014_1_Razd_4>();
        private List<FormsRSW2014_1_Razd_5> RSW_5_List = new List<FormsRSW2014_1_Razd_5>();
        private List<FormsRSW2014_1_Razd_6_1> RSW_6_1_List = new List<FormsRSW2014_1_Razd_6_1>();
        private List<FormsRSW2014_1_Razd_6_4> RSW_6_4_List = new List<FormsRSW2014_1_Razd_6_4>();
        private List<FormsRSW2014_1_Razd_6_6> RSW_6_6_List = new List<FormsRSW2014_1_Razd_6_6>();
        private List<FormsRSW2014_1_Razd_6_7> RSW_6_7_List = new List<FormsRSW2014_1_Razd_6_7>();
        private List<StajOsn> RSW_6_8_List = new List<StajOsn>();
        private List<FormsSPW2> SPW_2_List = new List<FormsSPW2>();
        private List<FormsSZV_6_4> SZV_6_4_List = new List<FormsSZV_6_4>();
        private List<FormsSZV_6> SZV_6_1_List = new List<FormsSZV_6>();
        private List<List<FormsSZV_6>> SZV_6_2_List = new List<List<FormsSZV_6>>();

        List<PlatCategory> PlatCatList = new List<PlatCategory>();
        List<TerrUsl> TerrUsl_list = new List<TerrUsl>();
        List<OsobUslTruda> OsobUslTruda_list = new List<OsobUslTruda>();
        List<KodVred_2> KodVred_2_list = new List<KodVred_2>();
        List<IschislStrahStajOsn> IschislStrahStajOsn_list = new List<IschislStrahStajOsn>();
        List<IschislStrahStajDop> IschislStrahStajDop_list = new List<IschislStrahStajDop>();
        List<UslDosrNazn> UslDosrNazn_list = new List<UslDosrNazn>();
        List<SpecOcenkaUslTruda> SpecOcenkaUslTruda_list = new List<SpecOcenkaUslTruda>();
        List<TypeInfo> typeInfo_ = new List<TypeInfo>();

        public class rsw2014PackIdent
        {
            public short Year { get; set; }
            public byte Quarter { get; set; }
            public long InsurerID { get; set; }
            public string FormatType { get; set; }
        }

        List<string> MonthesList = new List<string>
        {
            "Янв",
            "Фев",
            "Мрт",
            "Апр",
            "Май",
            "Июн",
            "Июл",
            "Авг",
            "Сен",
            "Окт",
            "Нбр",
            "Дек"
        };

        public rsw2014packs()
        {
            InitializeComponent();
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

        private void saveBtnAll_Click(object sender, EventArgs e)
        {

        }

        private void printStaffListItem_Click(object sender, EventArgs e)
        {

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void packsGrid_CurrentRowChanged()
        {
            if (d != null)
                d();
        }

        DelEvent d;

        private void rsw2014packs_Load(object sender, EventArgs e)
        {

            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            folderBrowserDialog.SelectedPath = Options.CurrentInsurerFolders.exportPath;

            switch (ident.FormatType)
            {
                case "rsw2014":
                    rsw251Btn.Visible = true;
                    //printBtnAll.Visibility = ElementVisibility.Visible;
                    printADV61DDL.Enabled = false;
                    printSPW2.Enabled = false;
                    printDSW3.Enabled = false;
                    printADW.Enabled = false;
                    break;
                case "spw2_2014":
                    rsw251Btn.Visible = false;
                    printBtnAllDDL.Enabled = false;
                    printADV61DDL.Enabled = true;
                    printSPW2.Enabled = true;
                    printDSW3.Enabled = false;
                    printADW.Enabled = false;
                    break;
                case "dsw3":
                    rsw251Btn.Visible = false;
                    printBtnAllDDL.Enabled = false;
                    printADV61DDL.Enabled = false;
                    printSPW2.Enabled = false;
                    printDSW3.Enabled = true;
                    printADW.Enabled = false;
                    break;
                case "adw1":
                    rsw251Btn.Visible = false;
                    printBtnAllDDL.Enabled = false;
                    printADV61DDL.Enabled = false;
                    printSPW2.Enabled = false;
                    printDSW3.Enabled = false;
                    printADW.Enabled = true;
                    printADW1.Enabled = true;
                    printADW2.Enabled = false;
                    printADW3.Enabled = false;
                    break;
                case "adw2":
                    rsw251Btn.Visible = false;
                    printBtnAllDDL.Enabled = false;
                    printADV61DDL.Enabled = false;
                    printSPW2.Enabled = false;
                    printDSW3.Enabled = false;
                    printADW.Enabled = true;
                    printADW1.Enabled = false;
                    printADW2.Enabled = true;
                    printADW3.Enabled = false;
                    break;
                case "adw3":
                    rsw251Btn.Visible = false;
                    printBtnAllDDL.Enabled = false;
                    printADV61DDL.Enabled = false;
                    printSPW2.Enabled = false;
                    printDSW3.Enabled = false;
                    printADW.Enabled = true;
                    printADW1.Enabled = false;
                    printADW2.Enabled = false;
                    printADW3.Enabled = true;
                    break;
                case "odv1":
                    rsw251Btn.Visible = false;
                    printBtnAllDDL.Enabled = false;
                    printADV61DDL.Enabled = false;
                    printSPW2.Enabled = false;
                    printDSW3.Enabled = false;
                    printADW.Enabled = false;
                    printADW1.Enabled = false;
                    printADW2.Enabled = false;
                    printADW3.Enabled = false;
                    printCurrent.Enabled = false;
                    break;
            }

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    foreach (var item in param.windowData)
                    {
                        int i = 0;

                        switch (item.control)
                        {
                            case "folderBrowserDialog":
                                folderBrowserDialog.SelectedPath = item.value;
                                break;
                        }
                    }
                }
                catch
                { }
            }

            this.packsGrid.CurrentRowChanged += (s, с) => packsGrid_CurrentRowChanged();
            packsGrid_upd();
            if (packsGrid.RowCount > 0)
            {
                //packsGrid.Rows[0].IsSelected = true;
                packsGrid.Rows[0].IsCurrent = true;
                staffGrid_upd();
            }

        }

        public void packsGrid_upd()
        {
            int rowindex = 0;
            if (packsGrid.RowCount > 0)
                rowindex = packsGrid.CurrentRow.Index;

            d = null;

            var files = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType);

            if (files.Any(x => x.DocType == "РСВ"))
            {
                long id = files.FirstOrDefault(x => x.DocType == "РСВ").SourceID.Value;
                if (!db.FormsRSW2014_1_1.Any(x => x.ID == id))
                {
                    try
                    {
                        foreach (var item in files)
                        {
                            dbxml.DeleteObject(item);
                        }
                        dbxml.SaveChanges();
                        files = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType);
                    }
                    catch (Exception ex)
                    {
                        Methods.showAlert("Внимание!", "Не найдена форма РСВ-1 для которой были сформированы пачки XML. При попытке удаления пачек возникла ошибка! " + ex.Message, this.ThemeName);
                    }

                }

            }

            List<RaschetPeriodContainer> rp = new List<RaschetPeriodContainer>();
            foreach (var item in Options.RaschetPeriodInternal2010_2013)
            {
                rp.Add(item);
            }
            foreach (var item in Options.RaschetPeriodInternal)
            {
                rp.Add(item);
            }

            this.packsGrid.TableElement.BeginUpdate();


            packsGrid.Rows.Clear();
            if (files.Count() != 0)
            {
                foreach (var item in files)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.packsGrid.MasterView);
                    rowInfo.Cells["id"].Value = item.ID;
                    rowInfo.Cells["num"].Value = item.Num;
                    rowInfo.Cells["cntDocs"].Value = item.CountDoc.HasValue ? item.CountDoc.Value.ToString() : "";
                    rowInfo.Cells["cntStaff"].Value = item.CountStaff.HasValue ? item.CountStaff.Value.ToString() : "";
                    rowInfo.Cells["type"].Value = item.DocType;
                    rowInfo.Cells["period"].Value = rp.Any(x => x.Year == item.Year && x.Kvartal == item.Quarter) ? (item.Quarter.ToString() + " - " + rp.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Quarter).Name) : "Период не определен";
                    rowInfo.Cells["korrPer"].Value = item.YearKorr != null ? item.QuarterKorr.ToString() + " - " + rp.FirstOrDefault(x => x.Year == item.YearKorr && x.Kvartal == item.QuarterKorr).Name : "";
                    rowInfo.Cells["dateCreated"].Value = item.DateCreate.Value.ToShortDateString();

                    packsGrid.Rows.Add(rowInfo);
                }
            }

            this.packsGrid.TableElement.EndUpdate();

            //            staffGridView.Refresh();

            d += staffGrid_upd;

            if (packsGrid.RowCount > 0)
            {
                if (packsGrid.CurrentRow.Cells["type"].Value.ToString() != "РСВ")
                {
                    packsGrid.Rows[rowindex].IsCurrent = true;
                    staffGrid_upd();
                }
                else
                {
                    staffGrid.Rows.Clear();
                }
            }
            else
            {
                staffGrid.Rows.Clear();
            }

        }

        public void staffGrid_upd()
        {
            long fileID = 0;
            if (packsGrid.RowCount > 0)
            {
                if (packsGrid.CurrentRow == null)
                {
                    packsGrid.Rows.First().IsCurrent = true;
                }
                fileID = Convert.ToInt64(packsGrid.CurrentRow.Cells[1].Value);

            }
            this.staffGrid.TableElement.BeginUpdate();
            staffGrid.Rows.Clear();

            var staffList = dbxml.StaffList.Where(x => x.XmlInfoID == fileID).ToList();

            if (staffList.Count() != 0)
            {
                foreach (var item in staffList)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.staffGrid.MasterView);
                    rowInfo.Cells["id"].Value = item.ID;
                    rowInfo.Cells["num"].Value = item.Num;
                    rowInfo.Cells["fio"].Value = item.FIO;
                    rowInfo.Cells["insNum"].Value = item.InsuranceNum;
                    staffGrid.Rows.Add(rowInfo);
                }
            }
            for (var i = 0; i < staffGrid.Columns.Count; i++)
            {
                //staffGrid.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                staffGrid.Columns[i].ReadOnly = true;
            }


            this.staffGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.staffGrid.TableElement.EndUpdate();
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить записи", "Внимание! Удаление записей.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);

            if (dialogResult == DialogResult.Yes)
            {
                dbxml.ExecuteStoreCommand(String.Format("DELETE FROM xmlInfo WHERE ([Year] = {0} AND [Quarter] = {1} AND [InsurerID] = {2} AND [FormatType] = '{3}')", ident.Year, ident.Quarter, ident.InsurerID, ident.FormatType));
                packsGrid_upd();
            }
        }

        public XDocument updateRSV1_Razd_2_5(XDocument doc, long rswID)
        {
            var ns = doc.Root.GetDefaultNamespace();  // Очищаем пространство имен
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == rswID);

            if (db.FormsRSW2014_1_Razd_2_5_1.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID) || db.FormsRSW2014_1_Razd_2_5_2.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
            {
                XElement node;

                if (doc.Descendants().Any(x => x.Name.LocalName == "Раздел_2_5"))
                {
                    node = doc.Descendants().First(x => x.Name.LocalName == "Раздел_2_5");
                    node.RemoveAll();

                }
                else
                {
                    node = doc.Descendants().First(x => x.Name.LocalName == "Раздел2РасчетПоТарифуИдопТарифу");
                    node.Add(new XElement("Раздел_2_5"));
                    node = node.Element("Раздел_2_5");
                }

                #region Раздел 2.5.1
                if (db.FormsRSW2014_1_Razd_2_5_1.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                {
                    XElement ПереченьПачекИсходныхСведенийПУ = new XElement("ПереченьПачекИсходныхСведенийПУ",
                                                                   new XElement("КоличествоПачек", db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID).Count().ToString()));
                    decimal col2 = 0;
                    decimal col3 = 0;
                    long col4 = 0;

                    foreach (var rsw251 in db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                    {
                        XElement СведенияОпачкеИсходных = new XElement("СведенияОпачкеИсходных",
                                                              new XElement("НомерПП", rsw251.NumRec.Value),
                                                              new XElement("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную", rsw251.Col_2.HasValue ? Utils.decToStr(rsw251.Col_2.Value) : "0.00"),
                                                              new XElement("СтраховыхВзносовОПС", rsw251.Col_3.HasValue ? Utils.decToStr(rsw251.Col_3.Value) : "0.00"),
                                                              new XElement("КоличествоЗЛвПачке", rsw251.Col_4.Value),
                                                              new XElement("ИмяФайла", rsw251.Col_5));
                        col2 = rsw251.Col_2.HasValue ? col2 + rsw251.Col_2.Value : col2;
                        col3 = rsw251.Col_3.HasValue ? col3 + rsw251.Col_3.Value : col3;
                        col4 = rsw251.Col_4.HasValue ? col4 + rsw251.Col_4.Value : col4;

                        ПереченьПачекИсходныхСведенийПУ.Add(СведенияОпачкеИсходных);
                    }

                    ПереченьПачекИсходныхСведенийПУ.Add(new XElement("ИтогоСведенияПоПачкам",
                                                            new XElement("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную", col2 != 0 ? Utils.decToStr(col2) : "0.00"),
                                                            new XElement("СтраховыхВзносовОПС", col3 != 0 ? Utils.decToStr(col3) : "0.00"),
                                                            new XElement("КоличествоЗЛвПачке", col4.ToString())));

                    node.Add(ПереченьПачекИсходныхСведенийПУ);
                }


                #endregion

                #region Раздел 2.5.2

                if (db.FormsRSW2014_1_Razd_2_5_2.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                {
                    XElement ПереченьПачекКорректирующихСведенийПУ = new XElement("ПереченьПачекКорректирующихСведенийПУ",
                                                                         new XElement("КоличествоПачек", db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID).Count().ToString()));

                    decimal col4 = 0;
                    decimal col5 = 0;
                    decimal col6 = 0;
                    long col7 = 0;

                    foreach (var rsw252 in db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                    {
                        XElement СведенияОпачкеКорректирующих = new XElement("СведенияОпачкеКорректирующих",
                                                              new XElement("НомерПП", rsw252.NumRec.Value),
                                                              new XElement("КорректируемыйОтчетныйПериод",
                                                                  new XElement("Квартал", rsw252.Col_2_QuarterKorr),
                                                                  new XElement("Год", rsw252.Col_3_YearKorr)),
                                                              new XElement("ДоначисленоСтраховыхВзносовОПС", rsw252.Col_4.HasValue ? Utils.decToStr(rsw252.Col_4.Value) : "0.00"),
                                                              new XElement("ДоначисленоНаСтраховуюЧасть", rsw252.Col_5.HasValue ? Utils.decToStr(rsw252.Col_5.Value) : "0.00"),
                                                              new XElement("ДоначисленоНаНакопительнуюЧасть", rsw252.Col_6.HasValue ? Utils.decToStr(rsw252.Col_6.Value) : "0.00"),
                                                              new XElement("КоличествоЗЛвПачке", rsw252.Col_7.Value),
                                                              new XElement("ИмяФайла", rsw252.Col_8));

                        col4 = rsw252.Col_4.HasValue ? col4 + rsw252.Col_4.Value : col4;
                        col5 = rsw252.Col_5.HasValue ? col5 + rsw252.Col_5.Value : col5;
                        col6 = rsw252.Col_6.HasValue ? col6 + rsw252.Col_6.Value : col6;
                        col7 = rsw252.Col_7.HasValue ? col7 + rsw252.Col_7.Value : col7;


                        ПереченьПачекКорректирующихСведенийПУ.Add(СведенияОпачкеКорректирующих);
                    }

                    ПереченьПачекКорректирующихСведенийПУ.Add(new XElement("ИтогоСведенияПоПачкамКорректирующих",
                                                            new XElement("ДоначисленоСтраховыхВзносовОПС", col4 != 0 ? Utils.decToStr(col4) : "0.00"),
                                                            new XElement("ДоначисленоНаСтраховуюЧасть", col5 != 0 ? Utils.decToStr(col5) : "0.00"),
                                                            new XElement("ДоначисленоНаНакопительнуюЧасть", col6 != 0 ? Utils.decToStr(col6) : "0.00"),
                                                            new XElement("КоличествоЗЛвПачке", col7.ToString())));

                    node.Add(ПереченьПачекКорректирующихСведенийПУ);
                }
                #endregion



            }

            doc.Root.SetDefaultXmlNamespace(pfr);


            return doc;
        }

        private void rsw251Btn_Click(object sender, EventArgs e)
        {
            RSW2014_2_5_List child = new RSW2014_2_5_List();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ident.FormatType = ident.FormatType;
            child.ident.InsurerID = ident.InsurerID;
            child.ident.Quarter = ident.Quarter;
            child.ident.Year = ident.Year;
            child.ShowDialog();
        }

        private void printStaffReport(int pt)
        {
            if (packsGrid.RowCount != 0)
            {
                List<xmlInfo> staff_data = new List<xmlInfo>();
                List<long> ids = new List<long>();

                switch (pt)
                {
                    case 0:
                        for (var i = 0; i < packsGrid.RowCount; i++)
                        {
                            long cid = long.Parse(packsGrid.Rows[i].Cells["id"].Value.ToString());
                            ids.Add(cid);
                        }
                        break;
                    case 1:
                        for (var i = 0; i < packsGrid.RowCount; i++)
                        {
                            if ((bool?)packsGrid.Rows[i].Cells["chkBox"].Value == true)
                            {
                                long cid = long.Parse(packsGrid.Rows[i].Cells["id"].Value.ToString());
                                ids.Add(cid);
                            }
                        }
                        break;
                    case 2:
                        int rownum = packsGrid.CurrentRow.Index;
                        long id = long.Parse(packsGrid.Rows[rownum].Cells["id"].Value.ToString());
                        ids.Add(id);
                        break;
                }

                if (ids.Count > 0)
                {
                    staff_data = dbxml.xmlInfo.Where(x => ids.Contains(x.ID)).ToList();

                    ReportViewerStaff = new Staff_Print();
                    ReportViewerStaff.Staff_data = staff_data;
                    ReportViewerStaff.Owner = this;
                    ReportViewerStaff.ThemeName = this.ThemeName;
                    ReportViewerStaff.ShowInTaskbar = false;

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerStaff.createReport);
                    bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedStaff);

                    bw.RunWorkerAsync();
                }
                else
                {
                    RadMessageBox.Show(this, "Не выбранны данные для печати", "");
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void bw_RunWorkerCompletedStaff(object sender, RunWorkerCompletedEventArgs e)
        {
            ReportViewerStaff.ShowDialog();
        }

        private void printStaffAll_Click(object sender, EventArgs e)
        {
            printStaffReport(0);
        }

        private void printStaffChecked_Click(object sender, EventArgs e)
        {
            printStaffReport(1);
        }

        private void printStaffCurrent_Click(object sender, EventArgs e)
        {
            printStaffReport(2);
        }

        private void printADVReport(int pt)
        {
            if (packsGrid.RowCount != 0)
            {
                List<xmlInfo> adv_data = new List<xmlInfo>();
                List<long> ids = new List<long>();

                switch (pt)
                {
                    case 0:
                        for (var i = 0; i < packsGrid.RowCount; i++)
                        {
                            long cid = long.Parse(packsGrid.Rows[i].Cells["id"].Value.ToString());
                            ids.Add(cid);
                        }
                        break;
                    case 1:
                        for (var i = 0; i < packsGrid.RowCount; i++)
                        {
                            if ((bool?)packsGrid.Rows[i].Cells["chkBox"].Value == true)
                            {
                                long cid = long.Parse(packsGrid.Rows[i].Cells["id"].Value.ToString());
                                ids.Add(cid);
                            }
                        }
                        break;
                    case 2:
                        int rownum = packsGrid.CurrentRow.Index;
                        long id = long.Parse(packsGrid.Rows[rownum].Cells["id"].Value.ToString());
                        ids.Add(id);
                        break;
                }

                if (ids.Count > 0)
                {
                    adv_data = dbxml.xmlInfo.Where(x => ids.Contains(x.ID)).ToList();

                    ReportViewerADV = new ADV_Print();
                    ReportViewerADV.ADVdata = adv_data;
                    ReportViewerADV.Owner = this;
                    ReportViewerADV.ThemeName = this.ThemeName;
                    ReportViewerADV.ShowInTaskbar = false;

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerADV.createReport);
                    bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                    bw.RunWorkerAsync();
                }
                else
                {
                    RadMessageBox.Show(this, "Не выбранны данные для печати", "");
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReportViewerADV.ShowDialog();
        }

        private void printADV61All_Click(object sender, EventArgs e)
        {
            printADVReport(0);
        }

        private void printADV61Checked_Click(object sender, EventArgs e)
        {
            printADVReport(1);
        }

        private void printADV61Current_Click(object sender, EventArgs e)
        {
            printADVReport(2);
        }



        // Печать РСВ-1
        private void radMenuItem1_Click(object sender, EventArgs e)
        {
            printReport(0, false);

            //if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "РСВ"))
            //{
            //    var fileRSW = dbxml.xmlInfo.FirstOrDefault(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "РСВ");

            //    tempImportRSW(fileRSW.xmlFile.First().XmlContent);
            //    printReport(0);

            //}
            //else
            //{
            //    RadMessageBox.Show(this, "Нет данных для печати", "");
            //}
        }

        //Печать Инд. сведений
        private void radMenuItem2_Click(object sender, EventArgs e)
        {
            printReport(1, false);
        }

        //Печать РСВ-1 + Инд.сведения
        private void radMenuItem3_Click(object sender, EventArgs e)
        {
            //bool exist = false;
            //if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "РСВ"))
            //{
            //    var fileRSW = dbxml.xmlInfo.FirstOrDefault(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "РСВ");

            //    tempImportRSW(fileRSW.xmlFile.First().XmlContent);
            //    exist = true;

            //}
            //if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "ПФР"))
            //{
            //    var filesRSW6_1 = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "ПФР");
            //    fillDictions();
            //    foreach (var item in filesRSW6_1)
            //    {
            //        tempImportRSW6_1(item.xmlFile.First().XmlContent);
            //    }
            //    exist = true;
            //}
            //if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-4"))
            //{
            //    var filesSZV_6_4 = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-4");

            //    fillDictions();

            //    foreach (var item in filesSZV_6_4)
            //    {
            //        tempImportSZV_6_4(item.xmlFile.First().XmlContent);
            //    }
            //    exist = true;
            //}
            //if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-1"))
            //{
            //    var filesSZV_6_1 = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-1");

            //    fillDictions();

            //    foreach (var item in filesSZV_6_1)
            //    {
            //        tempImportSZV_6(item.xmlFile.First().XmlContent, "СЗВ-6-1");
            //    }
            //    exist = true;
            //}

            //if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-2"))
            //{
            //    var filesSZV_6_1 = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-2");

            //    fillDictions();

            //    foreach (var item in filesSZV_6_1)
            //    {
            //        tempImportSZV_6(item.xmlFile.First().XmlContent, "СЗВ-6-2");
            //    }
            //    exist = true;
            //}
            //if (exist)
            //{
            printReport(2, false);
            //}

        }

        private void fillDictions()
        {
            PlatCatList = db.PlatCategory.Where(x => x.PlatCategoryRaschPerID == 4).ToList();
            TerrUsl_list = db.TerrUsl.ToList();
            OsobUslTruda_list = db.OsobUslTruda.ToList();
            KodVred_2_list = db.KodVred_2.ToList();
            IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
            IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
            UslDosrNazn_list = db.UslDosrNazn.ToList();
            SpecOcenkaUslTruda_list = db.SpecOcenkaUslTruda.ToList();
            typeInfo_ = db.TypeInfo.ToList();
        }

        private void printReport(byte wp, bool printCurrent)
        {
            if (packsGrid.RowCount != 0 && packsGrid.CurrentRow.Cells[1].Value != null)
            {
                FormsRSW2014_1_1 rsw_data = new FormsRSW2014_1_1();
                FormsRSW2014_1_1 RSWdataPrev = new FormsRSW2014_1_1();

                if ((wp == 0 || wp == 2) && !printCurrent)
                {
                    if (packsGrid.Rows.Any(x => x.Cells["type"].Value.ToString() == "РСВ"))
                    {
                        long id = 0;

                        long.TryParse(packsGrid.Rows.First(x => x.Cells["type"].Value.ToString() == "РСВ").Cells["id"].Value.ToString(), out id);

                        long sourceID = 0;
                        if (dbxml.xmlInfo.Any(x => x.ID == id))
                            sourceID = dbxml.xmlInfo.FirstOrDefault(x => x.ID == id).SourceID.Value;

                        if (db.FormsRSW2014_1_1.Any(x => x.ID == sourceID))
                        {
                            rsw_data = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == sourceID);
                        }
                        else
                        {
                            RadMessageBox.Show(this, "Не удалось загрузить Форму РСВ-1 из базы данных для печати.", "");
                            if (wp == 0)
                                return;
                        }
                    }
                    else
                    {
                        RadMessageBox.Show(this, "В пачке не найдена Форма РСВ-1.", "");
                        if (wp == 0)
                            return;
                    }
                }


                if ((wp == 1 || wp == 2) && !printCurrent)
                {
                    bool exist = false;
                    db_temp_print = new pu6Entities();

                    if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "ПФР"))
                    {
                        var filesRSW6_1 = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "ПФР");

                        List<long> sourceIDList = new List<long>();
                        foreach (var item in filesRSW6_1)
                        {
                            List<long> sourceIDList_temp = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();
                            sourceIDList = sourceIDList.Concat(sourceIDList_temp).ToList();
                        }

                        List<FormsRSW2014_1_Razd_6_1> List61_temp = db_temp_print.FormsRSW2014_1_Razd_6_1.Where(x => sourceIDList.Contains(x.ID)).ToList();
                        foreach (var item in sourceIDList)
                        {
                            RSW_6_1_List.Add(List61_temp.First(x => x.ID == item));
                        }

                        List61_temp.Clear();

                        exist = true;
                    }

                    if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-4"))
                    {
                        var filesSZV_6_4 = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-4");

                        List<long> sourceIDList = new List<long>();
                        foreach (var item in filesSZV_6_4)
                        {
                            List<long> sourceIDList_temp = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

                            sourceIDList = sourceIDList.Concat(sourceIDList_temp).ToList();
                        }

                        List<FormsSZV_6_4> List64_temp = db_temp_print.FormsSZV_6_4.Where(x => sourceIDList.Contains(x.ID)).ToList();
                        foreach (var item in sourceIDList)
                        {
                            SZV_6_4_List.Add(List64_temp.First(x => x.ID == item));
                        }
                        //  SZV_6_4_List = db.FormsSZV_6_4.Where(x => sourceIDList.Contains(x.ID)).ToList();

                        exist = true;
                    }

                    if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-1"))
                    {
                        var filesSZV_6_1 = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-1");

                        List<long> sourceIDList = new List<long>();
                        foreach (var item in filesSZV_6_1)
                        {
                            List<long> sourceIDList_temp = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

                            sourceIDList = sourceIDList.Concat(sourceIDList_temp).ToList();
                        }

                        List<FormsSZV_6> List61_temp = db_temp_print.FormsSZV_6.Where(x => sourceIDList.Contains(x.ID)).ToList();
                        foreach (var item in sourceIDList)
                        {
                            SZV_6_1_List.Add(List61_temp.First(x => x.ID == item));
                        }
                        //   SZV_6_1_List = db.FormsSZV_6.Where(x => sourceIDList.Contains(x.ID)).ToList();

                        exist = true;
                    }

                    if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-2"))
                    {
                        var filesSZV_6_1 = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СЗВ-6-2");

                        //                      List<long> sourceIDList = new List<long>();
                        foreach (var item in filesSZV_6_1)
                        {
                            List<long> sourceIDList_temp = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

                            //                            sourceIDList = sourceIDList.Concat(sourceIDList_temp).ToList();
                            var temp = db_temp_print.FormsSZV_6.Where(x => sourceIDList_temp.Contains(x.ID)).ToList();
                            if (temp.Any())
                                SZV_6_2_List.Add(temp);

                            //        private List<List<FormsSZV_6>> SZV_6_2_List = new List<List<FormsSZV_6>>();
                        }


                        exist = true;
                    }


                    if (wp == 1 && !exist)
                    {
                        RadMessageBox.Show(this, "Нет данных для печати", "");
                        return;
                    }

                    if (wp == 2 && !exist && rsw_data == null)
                    {
                        RadMessageBox.Show(this, "Нет данных для печати", "");
                        return;
                    }

                }

                if (printCurrent)  // Если печатаем текущую пачку
                {
                    string DocType = packsGrid.CurrentRow.Cells["type"].Value.ToString();
                    long id = Convert.ToInt64(packsGrid.CurrentRow.Cells[1].Value);
                    var xmlInfoT = dbxml.xmlInfo.FirstOrDefault(x => x.ID == id);

                    List<long> sourceIDList_temp = new List<long>();

                    if (xmlInfoT.StaffList.Any())
                        sourceIDList_temp = xmlInfoT.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

                    switch (DocType)
                    {
                        case "РСВ":
                            long sourceID = (xmlInfoT != null && xmlInfoT.SourceID.HasValue) ? xmlInfoT.SourceID.Value : 0;

                            if (db.FormsRSW2014_1_1.Any(x => x.ID == sourceID))
                            {
                                rsw_data = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == sourceID);
                            }
                            else
                            {
                                RadMessageBox.Show(this, "Не удалось загрузить Форму РСВ-1 из базы данных для печати.", "");
                                if (wp == 0)
                                    return;
                            }
                            break;
                        case "ПФР":
                            if (sourceIDList_temp.Any())
                                RSW_6_1_List = db.FormsRSW2014_1_Razd_6_1.Where(x => sourceIDList_temp.Contains(x.ID)).ToList();
                            break;
                        case "СЗВ-6-4":
                            if (sourceIDList_temp.Any())
                                SZV_6_4_List = db.FormsSZV_6_4.Where(x => sourceIDList_temp.Contains(x.ID)).ToList();
                            break;
                        case "СЗВ-6-1":
                            if (sourceIDList_temp.Any())
                                SZV_6_1_List = db.FormsSZV_6.Where(x => sourceIDList_temp.Contains(x.ID)).ToList();
                            break;
                        case "СЗВ-6-2":
                            if (sourceIDList_temp.Any())
                            {
                                var temp = db.FormsSZV_6.Where(x => sourceIDList_temp.Contains(x.ID)).ToList();
                                if (temp.Any())
                                    SZV_6_2_List.Add(temp);
                            }
                            break;
                    }
                }


                ReportViewerRSW2014 = new RSW2014_Print();
                ReportViewerRSW2014.RSWdata = rsw_data;
                //ReportViewerRSW2014.RSWdataPrev = RSWdataPrev;

                ReportViewerRSW2014.RSW_6_1_List = RSW_6_1_List;

                ReportViewerRSW2014.SZV_6_4_List = SZV_6_4_List;
                ReportViewerRSW2014.SZV_6_1_List = SZV_6_1_List;
                ReportViewerRSW2014.SZV_6_2_List = SZV_6_2_List;

                short yearType = 0;

                if (rsw_data != null)
                {
                    yearType = ((rsw_data.Year == (short)2014) || (rsw_data.Year == (short)2015 && rsw_data.Quarter == 3)) ? (short)2014 : (short)2015;
                }
                else if (RSW_6_1_List.Count > 0)
                {
                    yearType = ((RSW_6_1_List.First().Year == (short)2014) || (RSW_6_1_List.First().Year == (short)2015 && RSW_6_1_List.First().Quarter == 3)) ? (short)2014 : (short)2015;
                }

                ReportViewerRSW2014.fromXMLflag = false;
                ReportViewerRSW2014.Owner = this;
                ReportViewerRSW2014.ThemeName = this.ThemeName;
                ReportViewerRSW2014.ShowInTaskbar = false;
                ReportViewerRSW2014.wp = wp;
                ReportViewerRSW2014.yearType = yearType;

                radWaitingBar1.Visible = true;
                radWaitingBar1.StartWaiting();

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerRSW2014.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_);

                bw.RunWorkerAsync();

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }




            //ReportViewerRSW2014 = new RSW2014_Print();
            //ReportViewerRSW2014.fromXMLflag = true;

            //ReportViewerRSW2014.RSWdata = RSWdata;
            //ReportViewerRSW2014.RSW_2_1_List = RSW_2_1_List;
            //ReportViewerRSW2014.RSW_2_4_List = RSW_2_4_List;
            //ReportViewerRSW2014.RSW_2_5_1_List = RSW_2_5_1_List;
            //ReportViewerRSW2014.RSW_2_5_2_List = RSW_2_5_2_List;
            //ReportViewerRSW2014.RSW_3_4_List = RSW_3_4_List;
            //ReportViewerRSW2014.RSW_4_List = RSW_4_List;
            //ReportViewerRSW2014.RSW_5_List = RSW_5_List;

            //ReportViewerRSW2014.RSW_6_1_List = RSW_6_1_List;

            //ReportViewerRSW2014.SZV_6_4_List = SZV_6_4_List;
            //ReportViewerRSW2014.SZV_6_1_List = SZV_6_1_List;
            //ReportViewerRSW2014.SZV_6_2_List = SZV_6_2_List;

            //short yearType = 0;

            //if (RSWdata != null)
            //{
            //    yearType = ((RSWdata.Year == (short)2014) || (RSWdata.Year == (short)2015 && RSWdata.Quarter == 3)) ? (short)2014 : (short)2015;
            //}
            //else if (RSW_6_1_List.Count > 0)
            //{
            //    yearType = ((RSW_6_1_List.First().Year == (short)2014) || (RSW_6_1_List.First().Year == (short)2015 && RSW_6_1_List.First().Quarter == 3)) ? (short)2014 : (short)2015;
            //}


            ////ReportViewerRSW2014.RSWdataPrev = RSWdataPrev;
            //ReportViewerRSW2014.Owner = this;
            //ReportViewerRSW2014.ThemeName = this.ThemeName;
            //ReportViewerRSW2014.ShowInTaskbar = false;
            //ReportViewerRSW2014.wp = wp;
            //ReportViewerRSW2014.yearType = yearType;


            //radWaitingBar1.Visible = true;
            //radWaitingBar1.StartWaiting();

            //BackgroundWorker bw = new BackgroundWorker();
            //bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerRSW2014.createReport);
            //bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_);

            //bw.RunWorkerAsync();

        }

        private void bw_RunWorkerCompleted_(object sender, RunWorkerCompletedEventArgs e)
        {

            //    ReportViewerRSW2014.Invoke(new Action(() => {  }));
            radWaitingBar1.Invoke(new Action(() => { radWaitingBar1.StopWaiting(); radWaitingBar1.Visible = false; }));
            ReportViewerRSW2014.ShowDialog();

            RSWdata = new FormsRSW2014_1_1();
            RSW_2_1_List = new List<FormsRSW2014_1_Razd_2_1>();
            RSW_2_4_List = new List<FormsRSW2014_1_Razd_2_4>();
            RSW_2_5_1_List = new List<FormsRSW2014_1_Razd_2_5_1>();
            RSW_2_5_2_List = new List<FormsRSW2014_1_Razd_2_5_2>();
            RSW_3_4_List = new List<FormsRSW2014_1_Razd_3_4>();
            RSW_4_List = new List<FormsRSW2014_1_Razd_4>();
            RSW_5_List = new List<FormsRSW2014_1_Razd_5>();
            RSW_6_1_List = new List<FormsRSW2014_1_Razd_6_1>();
            SZV_6_4_List = new List<FormsSZV_6_4>();
            SZV_6_1_List = new List<FormsSZV_6>();
            SZV_6_2_List = new List<List<FormsSZV_6>>();

            if (db_temp_print != null)
                db_temp_print.Dispose();

        }

        //импорт из сформированных пачек во временные переменные
        private void tempImportRSW(string xmlString)
        {
            XDocument doc = XDocument.Parse(xmlString);

            XElement node = doc.Root;
            short yearType = 2014;
            string xName = string.Empty;

            var ns = doc.Root.GetDefaultNamespace();
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;
            try
            {
                string regnum = doc.Descendants().First(p => p.Name.LocalName == "РегистрационныйНомерПФР").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                if (doc.Descendants().Any(x => x.Name.LocalName == "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014"))
                {
                    yearType = 2014;
                    xName = "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014";
                }
                else if (doc.Descendants().Any(x => x.Name.LocalName == "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2015"))
                {
                    yearType = 2015;
                    xName = "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2015";
                }


                node = doc.Descendants().First(x => x.Name.LocalName == xName);

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                }


                byte q = byte.Parse(node.Element("КодОтчетногоПериода").Value.ToString());
                short y = short.Parse(node.Element("КалендарныйГод").Value.ToString());
                byte corrNum = byte.Parse(node.Element("НомерКорректировки").Value.ToString());

                RSWdata = new FormsRSW2014_1_1();

                RSWdata.CorrectionNum = corrNum;
                RSWdata.InsurerID = insurer.ID;
                RSWdata.Year = y;
                RSWdata.Quarter = q;
                if (node.Element("ТипКорректировки") != null && node.Element("ТипКорректировки").Value.ToString() != "")
                    RSWdata.CorrectionType = byte.Parse(node.Element("ТипКорректировки").Value.ToString());
                if (node.Element("ПрекращениеДеятельности") != null)
                    RSWdata.WorkStop = node.Element("ПрекращениеДеятельности").Value.ToString() == "Л" ? (byte)1 : (byte)0;
                else
                    RSWdata.WorkStop = (byte)0;

                int CountEmployers = 0;
                int CountAverageEmployers = 0;

                int.TryParse(node.Element("КоличествоЗЛ").Value.ToString(), out CountEmployers);
                int.TryParse(node.Element("СреднесписочнаяЧисленность").Value.ToString(), out CountAverageEmployers);

                RSWdata.CountEmployers = CountEmployers;
                RSWdata.CountAverageEmployers = CountAverageEmployers;
                byte b = 0;
                RSWdata.CountConfirmDoc = node.Element("КоличествоЛистовПриложения") != null ? (byte.TryParse(node.Element("КоличествоЛистовПриложения").Value.ToString(), out b) ? b : (byte)0) : (byte)0;

                // Что это такое???
                //node.Element("КоличествоСтраниц").Value.ToString()
                //                List<string> strCodes = new List<string> { "100", "110", "111", "112", "113", "114", "120", "121", "130", "140", "141", "142", "143", "144", "150"};

                RSWdata.ConfirmType = byte.Parse(node.Element("ЛицоПодтверждающееСведения").Value.ToString());
                RSWdata.DateUnderwrite = DateTime.Parse(node.Element("ДатаЗаполнения").Value.ToString());
                node = node.Element("ФИОлицаПодтверждающегоСведения");
                RSWdata.ConfirmLastName = node.Element("Фамилия").Value;
                RSWdata.ConfirmFirstName = node.Element("Имя").Value;
                RSWdata.ConfirmMiddleName = node.Element("Отчество").Value;
                if (RSWdata.ConfirmType > 1)
                {
                    if (node.Parent.Element("НаименованиеОрганизацииПредставителя") != null)
                    {
                        node = node.Parent.Element("НаименованиеОрганизацииПредставителя");
                        RSWdata.ConfirmOrgName = node.Value;
                    }
                    if (node.Parent.Element("ДокументПодтверждающийПолномочияПредставителя") != null)
                    {
                        node = node.Parent.Element("ДокументПодтверждающийПолномочияПредставителя");
                        string docName = node.Element("НаименованиеУдостоверяющего").Value;
                        if (db.DocumentTypes.Any(x => x.Code == docName))
                        {
                            RSWdata.ConfirmDocType_ID = db.DocumentTypes.FirstOrDefault(x => x.Code == docName).ID;
                        }
                        else
                        {
                            RSWdata.ConfirmDocType_ID = db.DocumentTypes.FirstOrDefault(x => x.Code == "ПРОЧЕЕ").ID;
                            RSWdata.ConfirmDocName = docName;
                        }
                        RSWdata.ConfirmDocSerLat = node.Element("СерияРимскиеЦифры") != null ? node.Element("СерияРимскиеЦифры").Value : "";
                        RSWdata.ConfirmDocSerRus = node.Element("СерияРусскиеБуквы") != null ? node.Element("СерияРусскиеБуквы").Value : "";
                        int nn = 0;
                        if (node.Element("НомерУдостоверяющего") != null)
                        {
                            int.TryParse(node.Element("НомерУдостоверяющего").Value.ToString(), out nn);
                        }
                        RSWdata.ConfirmDocNum = nn;
                        RSWdata.ConfirmDocDate = DateTime.Parse(node.Element("ДатаВыдачи").Value.ToString());
                        RSWdata.ConfirmDocKemVyd = node.Element("КемВыдан").Value;

                    }


                }

                node = doc.Descendants().First(x => x.Name.LocalName == xName);

                #region Раздел1РасчетПоНачисленнымУплаченным2014 - 2015
                xName = "Раздел1РасчетПоНачисленнымУплаченным2014";
                if (node.Element(xName) != null)
                {
                    XElement Раздел1РасчетПоНачисленнымУплаченным2014 = node.Element(xName);
                    var nodes = Раздел1РасчетПоНачисленнымУплаченным2014.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                    foreach (var n in nodes)
                    {
                        string strCode = n.Value.ToString();

                        foreach (var item_ in n.Parent.Elements())
                        {
                            string itemName = "s_" + strCode + "_";
                            decimal data = 0;
                            if (item_.Name.LocalName != "КодСтроки")
                            {
                                int i = -1;
                                data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);

                                switch (item_.Name.LocalName)
                                {
                                    case "СтраховыеВзносыОПС":
                                        i = 0;
                                        break;
                                    case "ОПСстраховаяЧасть":
                                        i = 1;
                                        break;
                                    case "ОПСнакопительнаяЧасть":
                                        i = 2;
                                        break;
                                    case "ВзносыПоДопТарифу1":
                                        i = 3;
                                        break;
                                    case "ВзносыПоДопТарифу2_18":
                                        i = 4;
                                        break;
                                    case "СтраховыеВзносыОМС":
                                        i = 5;
                                        break;
                                }
                                if (i >= 0)
                                {
                                    itemName = itemName + i.ToString();
                                    RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                }
                            }
                        }

                    }
                }
                #endregion

                #region Раздел2РасчетПоТарифуИдопТарифу

                if (node.Element("Раздел2РасчетПоТарифуИдопТарифу") != null)
                {
                    XElement Раздел2РасчетПоТарифуИдопТарифу = node.Element("Раздел2РасчетПоТарифуИдопТарифу");

                    #region Раздел 2.1
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_1") != null)
                    {
                        var razd_2_1_list = Раздел2РасчетПоТарифуИдопТарифу.Descendants().Where(x => x.Name.LocalName == "Раздел_2_1");
                        foreach (var razd_2_1 in razd_2_1_list)
                        {
                            FormsRSW2014_1_Razd_2_1 rsw_2_1 = new FormsRSW2014_1_Razd_2_1
                            {
                                Year = RSWdata.Year,
                                Quarter = RSWdata.Quarter,
                                CorrectionNum = RSWdata.CorrectionNum,
                                InsurerID = RSWdata.InsurerID,
                                AutoCalc = false
                            };
                            string tarCode = razd_2_1.Element("КодТарифа").Value.ToString();
                            if (db.TariffCode.Any(x => x.Code == tarCode))
                            {
                                rsw_2_1.TariffCodeID = db.TariffCode.First(x => x.Code == tarCode).ID;
                            }
                            else
                                rsw_2_1.TariffCodeID = 0;

                            var nodes = razd_2_1.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                            foreach (var n in nodes)
                            {
                                string strCode = n.Value.ToString();
                                if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                                {
                                    foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                    {
                                        string itemName = "s_" + strCode + "_";
                                        decimal data = 0;
                                        int i = -1;
                                        if (item_.Value != null)
                                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                        else
                                            data = 0;

                                        switch (item_.Name.LocalName)
                                        {
                                            case "СуммаВсегоСначалаРасчетногоПериода":
                                                i = 0;
                                                break;
                                            case "СуммаПоследние1месяц":
                                                i = 1;
                                                break;
                                            case "СуммаПоследние2месяц":
                                                i = 2;
                                                break;
                                            case "СуммаПоследние3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw_2_1.GetType().GetProperty(itemName).SetValue(rsw_2_1, data, null);
                                        }
                                    }
                                }
                                else // если указана численность
                                {
                                    foreach (var item_ in n.Parent.Elements())
                                    {
                                        long data = 0;
                                        if (strCode == "215" && yearType == 2015)
                                            strCode = strCode + "i";
                                        string itemName = "s_" + strCode + "_";
                                        if (item_.Name.LocalName != "КодСтроки")
                                        {
                                            int i = -1;
                                            if (item_.Value != null)
                                                data = long.Parse(item_.Value.ToString());
                                            else
                                                data = 0;

                                            switch (item_.Name.LocalName)
                                            {
                                                case "КоличествоЗЛ_Всего":
                                                    i = 0;
                                                    break;
                                                case "КоличествоЗЛ_1месяц":
                                                    i = 1;
                                                    break;
                                                case "КоличествоЗЛ_2месяц":
                                                    i = 2;
                                                    break;
                                                case "КоличествоЗЛ_3месяц":
                                                    i = 3;
                                                    break;
                                            }
                                            if (i >= 0)
                                            {
                                                itemName = itemName + i.ToString();
                                                rsw_2_1.GetType().GetProperty(itemName).SetValue(rsw_2_1, data, null);
                                            }
                                        }
                                    }
                                }

                            }

                            RSW_2_1_List.Add(rsw_2_1);
                        }
                    }
                    #endregion

                    #region Раздел_2_2
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_2") != null)
                    {
                        XElement Раздел_2_2 = Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_2");
                        var nodes = Раздел_2_2.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                {
                                    string itemName = "s_" + strCode + "_";
                                    decimal data = 0;
                                    int i = -1;
                                    data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);

                                    switch (item_.Name.LocalName)
                                    {
                                        case "СуммаВсегоСначалаРасчетногоПериода":
                                            i = 0;
                                            break;
                                        case "СуммаПоследние1месяц":
                                            i = 1;
                                            break;
                                        case "СуммаПоследние2месяц":
                                            i = 2;
                                            break;
                                        case "СуммаПоследние3месяц":
                                            i = 3;
                                            break;
                                    }
                                    if (i >= 0)
                                    {
                                        itemName = itemName + i.ToString();
                                        RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                    }
                                }
                            }
                            else // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛ_Всего":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛ_1месяц":
                                                i = 1;
                                                break;
                                            case "КоличествоЗЛ_2месяц":
                                                i = 2;
                                                break;
                                            case "КоличествоЗЛ_3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                        }
                                    }
                                }
                            }

                        }

                        if (RSWdata.s_220_0 != 0)
                            RSWdata.ExistPart_2_2 = (byte)1;
                    }

                    #endregion

                    #region Раздел_2_3
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_3") != null)
                    {
                        XElement Раздел_2_3 = Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_3");
                        var nodes = Раздел_2_3.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                {
                                    string itemName = "s_" + strCode + "_";
                                    decimal data = 0;
                                    int i = -1;
                                    data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);

                                    switch (item_.Name.LocalName)
                                    {
                                        case "СуммаВсегоСначалаРасчетногоПериода":
                                            i = 0;
                                            break;
                                        case "СуммаПоследние1месяц":
                                            i = 1;
                                            break;
                                        case "СуммаПоследние2месяц":
                                            i = 2;
                                            break;
                                        case "СуммаПоследние3месяц":
                                            i = 3;
                                            break;
                                    }
                                    if (i >= 0)
                                    {
                                        itemName = itemName + i.ToString();
                                        RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                    }
                                }
                            }
                            else // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛ_Всего":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛ_1месяц":
                                                i = 1;
                                                break;
                                            case "КоличествоЗЛ_2месяц":
                                                i = 2;
                                                break;
                                            case "КоличествоЗЛ_3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                        }
                                    }
                                }
                            }

                        }

                        if (RSWdata.s_230_0 != 0)
                            RSWdata.ExistPart_2_3 = (byte)1;

                    }
                    #endregion

                    #region Раздел 2.4
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_4") != null)
                    {
                        var razd_2_4_list = Раздел2РасчетПоТарифуИдопТарифу.Descendants().Where(x => x.Name.LocalName == "Раздел_2_4");
                        foreach (var razd_2_4 in razd_2_4_list)
                        {
                            FormsRSW2014_1_Razd_2_4 rsw_2_4 = new FormsRSW2014_1_Razd_2_4
                            {
                                Year = RSWdata.Year,
                                Quarter = RSWdata.Quarter,
                                CorrectionNum = RSWdata.CorrectionNum,
                                InsurerID = RSWdata.InsurerID,
                                AutoCalc = false
                            };
                            rsw_2_4.CodeBase = byte.Parse(razd_2_4.Element("КодОснованияРасчетаПоДопТарифу").Value.ToString());

                            switch (razd_2_4.Element("ОснованиеЗаполненияРаздела2_4").Value.ToString())
                            {
                                case "РЕЗУЛЬТАТЫ СПЕЦОЦЕНКИ":
                                    rsw_2_4.FilledBase = (byte)0;
                                    break;
                                case "РЕЗУЛЬТАТЫ АТТЕСТАЦИИ РАБОЧИХ МЕСТ":
                                    rsw_2_4.FilledBase = (byte)1;
                                    break;
                                case "РЕЗУЛЬТАТЫ СПЕЦОЦЕНКИ И РЕЗУЛЬТАТЫ АТТЕСТАЦИИ РАБОЧИХ МЕСТ":
                                    rsw_2_4.FilledBase = (byte)2;
                                    break;
                            }

                            var nodes = razd_2_4.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                            foreach (var n in nodes)
                            {
                                string strCode = n.Value.ToString();


                                if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                                {
                                    foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                    {
                                        string itemName = "s_" + strCode + "_";
                                        decimal data = 0;
                                        int i = -1;
                                        if (item_.Value != null)
                                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                        else
                                            data = 0;

                                        switch (item_.Name.LocalName)
                                        {
                                            case "СуммаВсегоСначалаРасчетногоПериода":
                                                i = 0;
                                                break;
                                            case "СуммаПоследние1месяц":
                                                i = 1;
                                                break;
                                            case "СуммаПоследние2месяц":
                                                i = 2;
                                                break;
                                            case "СуммаПоследние3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw_2_4.GetType().GetProperty(itemName).SetValue(rsw_2_4, data, null);
                                        }
                                    }
                                }
                                else // если указана численность
                                {
                                    foreach (var item_ in n.Parent.Elements())
                                    {
                                        long data = 0;
                                        string itemName = "s_" + strCode + "_";
                                        if (item_.Name.LocalName != "КодСтроки")
                                        {
                                            int i = -1;
                                            if (item_.Value != null)
                                                data = long.Parse(item_.Value.ToString());
                                            else
                                                data = 0;

                                            switch (item_.Name.LocalName)
                                            {
                                                case "КоличествоЗЛ_Всего":
                                                    i = 0;
                                                    break;
                                                case "КоличествоЗЛ_1месяц":
                                                    i = 1;
                                                    break;
                                                case "КоличествоЗЛ_2месяц":
                                                    i = 2;
                                                    break;
                                                case "КоличествоЗЛ_3месяц":
                                                    i = 3;
                                                    break;
                                            }
                                            if (i >= 0)
                                            {
                                                itemName = itemName + i.ToString();
                                                rsw_2_4.GetType().GetProperty(itemName).SetValue(rsw_2_4, data, null);
                                            }
                                        }
                                    }
                                }

                            }

                            RSW_2_4_List.Add(rsw_2_4);
                        }

                        RSWdata.ExistPart_2_4 = (byte)1;

                    }
                    #endregion

                    #region Раздел 2.5
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5") != null)
                    {
                        if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5").Element("ПереченьПачекИсходныхСведенийПУ") != null)
                        {
                            XElement ПереченьПачекИсходныхСведенийПУ = Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5").Element("ПереченьПачекИсходныхСведенийПУ");
                            if (ПереченьПачекИсходныхСведенийПУ.Element("СведенияОпачкеИсходных") != null)
                            {
                                var razd_2_5_1_list = ПереченьПачекИсходныхСведенийПУ.Descendants().Where(x => x.Name.LocalName == "СведенияОпачкеИсходных");
                                foreach (var razd_2_5_1 in razd_2_5_1_list)
                                {
                                    int n = 0;
                                    decimal d = 0;
                                    FormsRSW2014_1_Razd_2_5_1 rsw_2_5_1 = new FormsRSW2014_1_Razd_2_5_1
                                    {
                                        Year = RSWdata.Year,
                                        Quarter = RSWdata.Quarter,
                                        CorrectionNum = RSWdata.CorrectionNum,
                                        InsurerID = RSWdata.InsurerID,
                                        NumRec = razd_2_5_1.Element("НомерПП").Value != null ? (int.TryParse(razd_2_5_1.Element("НомерПП").Value.ToString(), out n) ? n : 0) : 0,
                                        Col_2 = razd_2_5_1.Element("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную").Value != null ? (decimal.TryParse(razd_2_5_1.Element("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_3 = razd_2_5_1.Element("СтраховыхВзносовОПС").Value != null ? (decimal.TryParse(razd_2_5_1.Element("СтраховыхВзносовОПС").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_4 = razd_2_5_1.Element("КоличествоЗЛвПачке").Value != null ? (int.TryParse(razd_2_5_1.Element("КоличествоЗЛвПачке").Value.ToString(), out n) ? n : 0) : 0,
                                        Col_5 = razd_2_5_1.Element("ИмяФайла").Value != null ? razd_2_5_1.Element("ИмяФайла").Value.ToString() : ""
                                    };

                                    RSW_2_5_1_List.Add(rsw_2_5_1);
                                }

                            }


                        }

                        if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5").Element("ПереченьПачекКорректирующихСведенийПУ") != null)
                        {
                            XElement ПереченьПачекКорректирующихСведенийПУ = Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5").Element("ПереченьПачекКорректирующихСведенийПУ");
                            if (ПереченьПачекКорректирующихСведенийПУ.Element("СведенияОпачкеКорректирующих") != null)
                            {
                                var razd_2_5_2_list = ПереченьПачекКорректирующихСведенийПУ.Descendants().Where(x => x.Name.LocalName == "СведенияОпачкеКорректирующих");
                                foreach (var razd_2_5_2 in razd_2_5_2_list)
                                {
                                    int n = 0;
                                    decimal d = 0;
                                    b = 0;
                                    short y_ = 0;
                                    FormsRSW2014_1_Razd_2_5_2 rsw_2_5_2 = new FormsRSW2014_1_Razd_2_5_2
                                    {
                                        Year = RSWdata.Year,
                                        Quarter = RSWdata.Quarter,
                                        CorrectionNum = RSWdata.CorrectionNum,
                                        InsurerID = RSWdata.InsurerID,
                                        NumRec = razd_2_5_2.Element("НомерПП").Value != null ? (int.TryParse(razd_2_5_2.Element("НомерПП").Value.ToString(), out n) ? n : 0) : 0,
                                        Col_4 = (razd_2_5_2.Element("ДоначисленоСтраховыхВзносовОПС") != null && razd_2_5_2.Element("ДоначисленоСтраховыхВзносовОПС").Value != null) ? (decimal.TryParse(razd_2_5_2.Element("ДоначисленоСтраховыхВзносовОПС").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_5 = (razd_2_5_2.Element("ДоначисленоНаСтраховуюЧасть") != null && razd_2_5_2.Element("ДоначисленоНаСтраховуюЧасть").Value != null) ? (decimal.TryParse(razd_2_5_2.Element("ДоначисленоНаСтраховуюЧасть").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_6 = (razd_2_5_2.Element("ДоначисленоНаНакопительнуюЧасть") != null && razd_2_5_2.Element("ДоначисленоНаНакопительнуюЧасть").Value != null) ? (decimal.TryParse(razd_2_5_2.Element("ДоначисленоНаНакопительнуюЧасть").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_7 = razd_2_5_2.Element("КоличествоЗЛвПачке").Value != null ? (int.TryParse(razd_2_5_2.Element("КоличествоЗЛвПачке").Value.ToString(), out n) ? n : 0) : 0,
                                        Col_8 = razd_2_5_2.Element("ИмяФайла").Value != null ? razd_2_5_2.Element("ИмяФайла").Value.ToString() : "",
                                        Col_2_QuarterKorr = razd_2_5_2.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value != null ? (byte.TryParse(razd_2_5_2.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value.ToString(), out b) ? b : (byte)0) : (byte)0,
                                        Col_3_YearKorr = razd_2_5_2.Element("КорректируемыйОтчетныйПериод").Element("Год").Value != null ? (short.TryParse(razd_2_5_2.Element("КорректируемыйОтчетныйПериод").Element("Год").Value.ToString(), out y_) ? y_ : (short)0) : (short)0
                                    };

                                    RSW_2_5_2_List.Add(rsw_2_5_2);
                                }

                            }


                        }

                    }
                    #endregion


                }
                #endregion

                #region Раздел3РасчетНаПравоПримененияПониженногоТарифа2014

                xName = "Раздел3РасчетНаПравоПримененияПониженногоТарифа" + yearType.ToString();

                if (node.Element(xName) != null)
                {
                    XElement Раздел3РасчетНаПравоПримененияПониженногоТарифа2014 = node.Element(xName);

                    #region Раздел3_1_ДляОбщественныхОрганизацийИнвалидов
                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_1_ДляОбщественныхОрганизацийИнвалидов") != null)
                    {
                        XElement Раздел3_1_ДляОбщественныхОрганизацийИнвалидов = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_1_ДляОбщественныхОрганизацийИнвалидов");
                        var nodes = Раздел3_1_ДляОбщественныхОрганизацийИнвалидов.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("КоличествоЗЛ_Всего") != null) // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛ_Всего":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛ_1месяц":
                                                i = 1;
                                                break;
                                            case "КоличествоЗЛ_2месяц":
                                                i = 2;
                                                break;
                                            case "КоличествоЗЛ_3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                        }
                                    }
                                }
                            }


                        }
                        RSWdata.ExistPart_3_1 = (byte)1;

                    }

                    #endregion

                    #region Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов
                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов") != null)
                    {
                        XElement Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов");
                        var nodes = Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("КоличествоЗЛ_Всего") != null) // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛ_Всего":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛ_1месяц":
                                                i = 1;
                                                break;
                                            case "КоличествоЗЛ_2месяц":
                                                i = 2;
                                                break;
                                            case "КоличествоЗЛ_3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                        }
                                    }
                                }
                            }

                            if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                {
                                    string itemName = "s_" + strCode + "_";
                                    decimal data = 0;
                                    int i = -1;
                                    if (item_.Value != null)
                                        data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                    else
                                        data = 0;

                                    switch (item_.Name.LocalName)
                                    {
                                        case "СуммаВсегоСначалаРасчетногоПериода":
                                            i = 0;
                                            break;
                                        case "СуммаПоследние1месяц":
                                            i = 1;
                                            break;
                                        case "СуммаПоследние2месяц":
                                            i = 2;
                                            break;
                                        case "СуммаПоследние3месяц":
                                            i = 3;
                                            break;
                                    }
                                    if (i >= 0)
                                    {
                                        itemName = itemName + i.ToString();
                                        RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                    }
                                }
                            }
                        }
                        RSWdata.ExistPart_3_2 = (byte)1;

                    }

                    #endregion

                    #region Раздел3_3_ДляОрганизацийИТ (2014) и Раздел3_1_ДляОрганизацийИТ для 2015
                    xName = "Раздел3_3_ДляОрганизацийИТ";

                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName) != null)
                    {
                        XElement Раздел3_3_ДляОрганизацийИТ = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName);
                        var nodes = Раздел3_3_ДляОрганизацийИТ.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("КоличествоЗЛпоПредшествующему") != null) // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛпоПредшествующему":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛпоТекущему":
                                                i = 1;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                        }
                                    }
                                }
                            }

                            if (n.Parent.Element("СуммаДоходаПоПредшествующему") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        string itemName = "s_" + strCode + "_";
                                        decimal data = 0;
                                        int i = -1;
                                        if (item_.Value != null)
                                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                        else
                                            data = 0;

                                        switch (item_.Name.LocalName)
                                        {
                                            case "СуммаДоходаПоПредшествующему":
                                                i = 0;
                                                break;
                                            case "СуммаДоходаПоТекущему":
                                                i = 1;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                        }
                                    }
                                }
                            }


                            if (n.Parent.Element("ДатаЗаписиВреестре") != null) // если указаны СведенияИзРеестра
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        switch (item_.Name.LocalName)
                                        {
                                            case "ДатаЗаписиВреестре":
                                                DateTime data;

                                                if (DateTime.TryParse(item_.Value.ToString(), out data))
                                                {
                                                    itemName = itemName + "0";
                                                    RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                                }
                                                break;
                                            case "НомерЗаписиВреестре":
                                                string data_ = !String.IsNullOrEmpty(item_.Value.ToString()) ? item_.Value.ToString() : "";
                                                itemName = itemName + "1";
                                                RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data_, null);
                                                break;
                                        }
                                    }
                                }
                            }

                        }
                        RSWdata.ExistPart_3_3 = (byte)1;
                    }

                    #endregion

                    #region Раздел3_4_ДляОрганизацийСМИ
                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_4_ДляОрганизацийСМИ") != null)
                    {
                        XElement Раздел3_4_ДляОрганизацийСМИ = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_4_ДляОрганизацийСМИ");

                        var razd_3_4_list = Раздел3_4_ДляОрганизацийСМИ.Descendants().Where(x => x.Name.LocalName == "СведенияПоВидуДеятельности");
                        foreach (var razd_3_4 in razd_3_4_list)
                        {
                            FormsRSW2014_1_Razd_3_4 rsw_3_4 = new FormsRSW2014_1_Razd_3_4
                            {
                                Year = RSWdata.Year,
                                Quarter = RSWdata.Quarter,
                                CorrectionNum = RSWdata.CorrectionNum,
                                InsurerID = RSWdata.InsurerID,
                                NumOrd = long.Parse(razd_3_4.Element("НомерПП").Value.ToString()),
                                NameOKWED = razd_3_4.Element("НаименованиеВидаЭД").Value.ToString(),
                                OKWED = razd_3_4.Element("КодПоОКВЭД").Value.ToString(),
                                Income = decimal.Parse(razd_3_4.Element("ДоходыПоВидуЭД").Value.ToString(), CultureInfo.InvariantCulture),
                                RateIncome = decimal.Parse(razd_3_4.Element("ДоляДоходовПоВидуЭД").Value.ToString(), CultureInfo.InvariantCulture)
                            };

                            RSW_3_4_List.Add(rsw_3_4);
                        }

                        RSWdata.s_351_0 = DateTime.Parse(Раздел3_4_ДляОрганизацийСМИ.Element("СведенияИзРеестраСМИ").Element("ДатаЗаписиВреестре").Value.ToString());
                        RSWdata.s_351_1 = Раздел3_4_ДляОрганизацийСМИ.Element("СведенияИзРеестраСМИ").Element("НомерЗаписиВреестре").Value.ToString();

                        RSWdata.ExistPart_3_4 = (byte)1;
                    }
                    #endregion

                    #region Раздел3_5_ДляОрганизацийПрименяющихУСН (2014) и Раздел3_2_ДляОрганизацийПрименяющихУСН для 2015

                    xName = "Раздел3_5_ДляОрганизацийПрименяющихУСН";


                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName) != null)
                    {
                        XElement Раздел3_5_ДляОрганизацийПрименяющихУСН = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName);

                        RSWdata.s_361_0 = decimal.Parse(Раздел3_5_ДляОрганизацийПрименяющихУСН.Element("СуммаДоходаПоСтатье346_15НКвсего").Element("СуммаДохода").Value.ToString(), CultureInfo.InvariantCulture);
                        RSWdata.s_362_0 = decimal.Parse(Раздел3_5_ДляОрганизацийПрименяющихУСН.Element("СуммаДоходаПоСтатье58ИзНих").Element("СуммаДохода").Value.ToString(), CultureInfo.InvariantCulture);
                        RSWdata.ExistPart_3_5 = (byte)1;
                    }
                    #endregion

                    #region Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН 2014 и Раздел3_3_ДляНекоммерческихОрганизацийПрименяющихУСН для 2015
                    xName = "Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН";

                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName) != null)
                    {
                        XElement Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName);
                        var nodes = Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("СуммаДоходаПоПредшествующему") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        string itemName = "s_" + strCode + "_";
                                        decimal data = 0;
                                        int i = -1;
                                        if (item_.Value != null)
                                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                        else
                                            data = 0;

                                        switch (item_.Name.LocalName)
                                        {
                                            case "СуммаДоходаПоПредшествующему":
                                                i = 0;
                                                break;
                                            case "СуммаДоходаПоТекущему":
                                                i = 1;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            RSWdata.GetType().GetProperty(itemName).SetValue(RSWdata, data, null);
                                        }
                                    }
                                }
                            }


                        }
                        RSWdata.ExistPart_3_6 = (byte)1;
                    }

                    #endregion


                }
                #endregion

                #region Раздел4СуммыДоначисленныхСтраховыхВзносов2014
                xName = yearType != 2015 ? "Раздел4СуммыДоначисленныхСтраховыхВзносов2014" : "Раздел4";
                if (node.Element(xName) != null)
                {
                    XElement Раздел4СуммыДоначисленныхСтраховыхВзносов2014 = node.Element(xName);

                    var razd_4_list = Раздел4СуммыДоначисленныхСтраховыхВзносов2014.Descendants().Where(x => x.Name.LocalName == "СуммаДоначисленныхВзносовЗаПериодНачинаяС2014");
                    foreach (var razd_4 in razd_4_list)
                    {
                        FormsRSW2014_1_Razd_4 rsw_4 = new FormsRSW2014_1_Razd_4
                        {
                            Year = RSWdata.Year,
                            Quarter = RSWdata.Quarter,
                            CorrectionNum = RSWdata.CorrectionNum,
                            InsurerID = RSWdata.InsurerID,
                            NumOrd = long.Parse(razd_4.Element("НомерПП").Value.ToString()),
                            Base = byte.Parse(razd_4.Element("ОснованиеДляДоначисления").Value.ToString()),
                            CodeBase = razd_4.Element("КодОснованияДляДопТарифа") != null ? byte.Parse(razd_4.Element("КодОснованияДляДопТарифа").Value.ToString()) : (byte)0,
                            YearPer = short.Parse(razd_4.Element("Год").Value.ToString()),
                            MonthPer = byte.Parse(razd_4.Element("Месяц").Value.ToString()),
                            Strah2014 = razd_4.Element("СуммаДоначисленныхВзносовОПС2014всего") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовОПС2014всего").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            StrahMoreBase2014 = razd_4.Element("СуммаДоначисленныхВзносовОПС2014превыщающие") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовОПС2014превыщающие").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Strah2013 = razd_4.Element("СуммаДоначисленныхВзносовНаСтраховуюВсего") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовНаСтраховуюВсего").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            StrahMoreBase2013 = razd_4.Element("СуммаДоначисленныхВзносовНаСтраховуюПревышающие") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовНаСтраховуюПревышающие").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Nakop2013 = razd_4.Element("СуммаДоначисленныхВзносовНаНакопительную") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовНаНакопительную").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Dop1 = razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ1") != null ? decimal.Parse(razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ1").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Dop2 = razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2") != null ? decimal.Parse(razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Dop21 = razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2_1") != null ? decimal.Parse(razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2_1").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            OMS = razd_4.Element("СтраховыеВзносыОМС") != null ? decimal.Parse(razd_4.Element("СтраховыеВзносыОМС").Value.ToString(), CultureInfo.InvariantCulture) : 0
                        };

                        RSW_4_List.Add(rsw_4);
                    }

                    RSWdata.ExistPart_4 = (byte)1;

                }
                #endregion

                #region Раздел5СведенияОВыплатахВпользуОбучающихся2014
                if (node.Element("Раздел5СведенияОВыплатахВпользуОбучающихся2014") != null)
                {
                    XElement Раздел5СведенияОВыплатахВпользуОбучающихся2014 = node.Element("Раздел5СведенияОВыплатахВпользуОбучающихся2014");

                    var razd_5_list = Раздел5СведенияОВыплатахВпользуОбучающихся2014.Descendants().Where(x => x.Name.LocalName == "СведенияОбОбучающемся");
                    foreach (var razd_5 in razd_5_list)
                    {
                        FormsRSW2014_1_Razd_5 rsw_5 = new FormsRSW2014_1_Razd_5
                        {
                            Year = RSWdata.Year,
                            Quarter = RSWdata.Quarter,
                            CorrectionNum = RSWdata.CorrectionNum,
                            InsurerID = RSWdata.InsurerID,
                            NumOrd = long.Parse(razd_5.Element("НомерПП").Value.ToString()),
                            LastName = razd_5.Element("ФИО").Element("Фамилия") != null ? razd_5.Element("ФИО").Element("Фамилия").Value.ToString() : "",
                            FirstName = razd_5.Element("ФИО").Element("Имя") != null ? razd_5.Element("ФИО").Element("Имя").Value.ToString() : "",
                            MiddleName = razd_5.Element("ФИО").Element("Отчество") != null ? razd_5.Element("ФИО").Element("Отчество").Value.ToString() : "",
                            NumSpravka = razd_5.Element("НомерСправкиОчленствеВстудОтряде").Value.ToString(),
                            DateSpravka = DateTime.Parse(razd_5.Element("ДатаВыдачиСправкиОчленствеВстудОтряде").Value.ToString()),
                            NumSpravka1 = razd_5.Element("НомерСправкиОбОчномОбучении").Value.ToString(),
                            DateSpravka1 = DateTime.Parse(razd_5.Element("ДатаВыдачиСправкиОбОчномОбучении").Value.ToString()),
                            SumPay = razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаВсегоСначалаРасчетногоПериода") != null ? decimal.Parse(razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаВсегоСначалаРасчетногоПериода").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            SumPay_0 = razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние1месяц") != null ? decimal.Parse(razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние1месяц").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            SumPay_1 = razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние2месяц") != null ? decimal.Parse(razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние2месяц").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            SumPay_2 = razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние3месяц") != null ? decimal.Parse(razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние3месяц").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                        };

                        RSW_5_List.Add(rsw_5);
                    }

                    XElement СведенияИзРеестраМДОО = Раздел5СведенияОВыплатахВпользуОбучающихся2014.Element("СведенияИзРеестраМДОО");

                    var nodes = СведенияИзРеестраМДОО.Descendants().Where(x => x.Name.LocalName == "РеквизитыЗаписиВреестре");
                    var i = 0;
                    foreach (var item in nodes)
                    {
                        DateTime ДатаЗаписиВреестре = DateTime.Parse(item.Element("ДатаЗаписиВреестре").Value.ToString());
                        string НомерЗаписиВреестре = item.Element("НомерЗаписиВреестре").Value.ToString();
                        string itemName_date = "s_501_0_" + i;
                        string itemName_num = "s_501_1_" + i;
                        RSWdata.GetType().GetProperty(itemName_date).SetValue(RSWdata, ДатаЗаписиВреестре, null);
                        RSWdata.GetType().GetProperty(itemName_num).SetValue(RSWdata, НомерЗаписиВреестре, null);

                        i++;
                    }

                    RSWdata.ExistPart_5 = (byte)1;
                }
                #endregion

            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message + "\r\n " + ex.InnerException);
            }


        }



        //импорт Инд. сведений Раздел 6.1 из сформированных пачек во временные переменные
        private void tempImportRSW6_1(string xmlString)
        {
            XDocument doc = XDocument.Parse(xmlString);
            var ns = doc.Root.GetDefaultNamespace();
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            XElement node = doc.Root;

            try
            {
                #region // Поиск информации о составителе
                node = node.Descendants().First(x => x.Name.LocalName == "СоставительПачки");

                string regnum = node.Element("РегистрационныйНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);


                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                }

                #endregion


                #region перебор инд.сведений
                List<TypeInfo> typeInfo_ = db.TypeInfo.ToList();

                var razd_6_1_list = doc.Root.Descendants().Where(x => x.Name.LocalName == "СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ");

                //List<PU.ImportXML.staffContainer> staffList = new List<PU.ImportXML.staffContainer>();
                //foreach (var razd_6_1 in razd_6_1_list)
                //{
                //    var insn = razd_6_1.Element("СтраховойНомер").Value.ToString().Trim().Split(' ');
                //    string InsuranceNum = insn[0];
                //    while (InsuranceNum.Contains("-"))
                //        InsuranceNum = InsuranceNum.Remove(InsuranceNum.IndexOf('-'), 1);
                //    byte? contrNum = null;
                //    if (insn.Count() > 1)
                //    {
                //        try
                //        {
                //            contrNum = byte.Parse(insn[1]);
                //        }
                //        catch
                //        { }
                //    }

                //    string middleName = razd_6_1.Element("ФИО").Element("Отчество") != null ? razd_6_1.Element("ФИО").Element("Отчество").Value.ToString() : "";

                //    var st = new PU.ImportXML.staffContainer
                //    {
                //        insuranceNum = InsuranceNum,
                //        insID = insurer.ID,
                //        lastName = razd_6_1.Element("ФИО").Element("Фамилия").Value.ToString(),
                //        firstName = razd_6_1.Element("ФИО").Element("Имя").Value.ToString(),
                //        middleName = middleName,
                //        contrNum = contrNum,
                //        dismissed = (byte)0
                //    };
                //    if (razd_6_1.Element("СведенияОбУвольнении") != null)
                //    {
                //        st.dismissed = razd_6_1.Element("СведенияОбУвольнении").Value == "УВОЛЕН" ? (byte)1 : (byte)0;
                //    }

                //    staffList.Add(st);
                //}

                //var listid = staffList.Select(y => y.insuranceNum).ToList();
                //var listNum = db.Staff.Where(x => x.InsurerID == insurer.ID && listid.Contains(x.InsuranceNumber)).Select(x => x.InsuranceNumber).ToList();

                //staffList = staffList.Where(x => !listNum.Contains(x.insuranceNum)).ToList();  // те сотрудники которых нет в базе для текущего страхователя

                //foreach (var item in staffList)
                //{
                //    Staff staff_ = new Staff
                //    {
                //        InsuranceNumber = item.insuranceNum,
                //        InsurerID = item.insID,
                //        LastName = item.lastName,
                //        FirstName = item.firstName,
                //        MiddleName = item.middleName,
                //        ControlNumber = item.contrNum,
                //        Dismissed = item.dismissed
                //    };

                //    db.AddToStaff(staff_);
                //}
                //if (staffList.Count > 0)
                //    db.SaveChanges();


                foreach (var razd_6_1 in razd_6_1_list)
                {
                    var insn = razd_6_1.Element("СтраховойНомер").Value.ToString().Trim().Split(' ');
                    string InsuranceNum = insn[0];
                    while (InsuranceNum.Contains("-"))
                        InsuranceNum = InsuranceNum.Remove(InsuranceNum.IndexOf('-'), 1);


                    Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);


                    string tInfo = razd_6_1.Element("ТипСведений").Value.ToString().ToLower();
                    long tInfoID = typeInfo_.First(x => x.Name.ToLower() == tInfo).ID;

                    byte q = byte.Parse(razd_6_1.Element("ОтчетныйПериод").Element("Квартал").Value.ToString());
                    short y = short.Parse(razd_6_1.Element("ОтчетныйПериод").Element("Год").Value.ToString());
                    //if (RSWdata == null)
                    //{
                    //    RSWdata = new FormsRSW2014_1_1 { Year = y , Quarter = q, InsurerID = insurer.ID};
                    //}

                    FormsRSW2014_1_Razd_6_1 rsw_6_1 = new FormsRSW2014_1_Razd_6_1();
                    byte qk = (byte)0;
                    short yk = (short)0;
                    string regNumK = "";

                    if (tInfo != "исходная")
                    {
                        qk = byte.Parse(razd_6_1.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value.ToString());
                        yk = short.Parse(razd_6_1.Element("КорректируемыйОтчетныйПериод").Element("Год").Value.ToString());
                        if (razd_6_1.Element("РегистрационныйНомерКорректируемогоПериода") != null)
                        {
                            regNumK = razd_6_1.Element("РегистрационныйНомерКорректируемогоПериода").Value.ToString();
                            while (regNumK.Contains("-"))
                                regNumK = regNumK.Remove(regNumK.IndexOf('-'), 1);
                        }
                    }



                    rsw_6_1.InsurerID = insurer.ID;
                    rsw_6_1.StaffID = staff.ID;
                    rsw_6_1.Year = y;
                    rsw_6_1.Quarter = q;

                    if (yk != 0)
                    {
                        rsw_6_1.YearKorr = yk;
                        rsw_6_1.QuarterKorr = qk;
                        rsw_6_1.RegNumKorr = regNumK;
                    }
                    rsw_6_1.TypeInfoID = tInfoID;

                    if (razd_6_1.Element("СуммаВзносовНаОПС") != null)
                    {
                        rsw_6_1.SumFeePFR = decimal.Parse(razd_6_1.Element("СуммаВзносовНаОПС").Value.ToString(), CultureInfo.InvariantCulture);
                    }
                    else
                        rsw_6_1.SumFeePFR = 0;
                    rsw_6_1.DateFilling = DateTime.Parse(razd_6_1.Element("ДатаЗаполнения").Value.ToString());


                    #region Раздел 6.4
                    if (razd_6_1.Descendants().Any(x => x.Name.LocalName == "СведенияОсуммеВыплатИвознагражденийВпользуЗЛ"))
                    {
                        var razd_6_4_list = razd_6_1.Descendants().Where(x => x.Name.LocalName == "СведенияОсуммеВыплатИвознагражденийВпользуЗЛ");
                        List<string> platCatList = razd_6_4_list.Descendants().Where(x => x.Name.LocalName == "КодКатегории").Select(x => x.Value).Distinct().ToList();
                        long platCategoryRaschPerID = 4;  // Для категорий после 2010

                        foreach (var item in platCatList)
                        {
                            PlatCategory platCat = PlatCatList.FirstOrDefault(x => x.PlatCategoryRaschPerID == platCategoryRaschPerID && x.Code == item);

                            FormsRSW2014_1_Razd_6_4 rsw_6_4 = new FormsRSW2014_1_Razd_6_4 { PlatCategoryID = platCat.ID, FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID };

                            foreach (var razd_6_4 in razd_6_4_list.Where(x => x.Element("КодКатегории").Value == item))
                            {
                                string name = "";
                                string strCode = razd_6_4.Element("КодСтроки").Value.ToString();
                                name = strCode.Substring(strCode.Length - 1, 1);

                                decimal data = 0;
                                for (int i = 0; i <= 3; i++)
                                {

                                    string itemName = "s_" + name + "_";
                                    string nameStr = "";
                                    switch (i)
                                    {
                                        case 0:
                                            nameStr = "СуммаВыплатИныхВознаграждений";
                                            break;
                                        case 1:
                                            nameStr = "НеПревышающиеВсего";
                                            break;
                                        case 2:
                                            nameStr = "НеПревышающиеПоДоговорам";
                                            break;
                                        case 3:
                                            nameStr = "ПревышающиеПредельную";
                                            break;
                                    }

                                    if (nameStr != "" && razd_6_4.Element(nameStr) != null)
                                        data = decimal.Parse(razd_6_4.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                    if (nameStr != "" && razd_6_4.Element(nameStr) == null)
                                        data = 0;

                                    itemName = itemName + i.ToString();

                                    rsw_6_4.GetType().GetProperty(itemName).SetValue(rsw_6_4, data, null);

                                }

                            }

                            rsw_6_1.FormsRSW2014_1_Razd_6_4.Add(rsw_6_4);

                        }

                    }

                    #endregion

                    #region Раздел 6.6
                    if (razd_6_1.Descendants().Any(x => x.Name.LocalName == "СведенияОкорректировках"))
                    {
                        var razd_6_6_list = razd_6_1.Descendants().Where(x => x.Name.LocalName == "СведенияОкорректировках");


                        foreach (var razd_6_6 in razd_6_6_list)
                        {
                            if (razd_6_6.Element("ТипСтроки").Value == "МЕСЦ")
                            {
                                byte accPer_q = byte.Parse(razd_6_6.Element("Квартал").Value);
                                short accPer_y = short.Parse(razd_6_6.Element("Год").Value);

                                FormsRSW2014_1_Razd_6_6 rsw_6_6 = new FormsRSW2014_1_Razd_6_6 { FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID, AccountPeriodQuarter = accPer_q, AccountPeriodYear = accPer_y };

                                decimal data = 0;
                                for (int i = 0; i <= 2; i++)
                                {

                                    string itemName = "";
                                    string nameStr = "";
                                    switch (i)
                                    {
                                        case 0:
                                            nameStr = "СуммаДоначисленныхВзносовОПС";
                                            itemName = "SumFeePFR_D";
                                            break;
                                        case 1:
                                            nameStr = "СуммаДоначисленныхВзносовНаСтраховую";
                                            itemName = "SumFeePFR_StrahD";
                                            break;
                                        case 2:
                                            nameStr = "СуммаДоначисленныхВзносовНаНакопительную";
                                            itemName = "SumFeePFR_NakopD";
                                            break;
                                    }

                                    if (nameStr != "" && razd_6_6.Element(nameStr) != null)
                                        data = decimal.Parse(razd_6_6.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                    if (nameStr != "" && razd_6_6.Element(nameStr) == null)
                                        data = 0;

                                    rsw_6_6.GetType().GetProperty(itemName).SetValue(rsw_6_6, data, null);
                                }

                                rsw_6_1.FormsRSW2014_1_Razd_6_6.Add(rsw_6_6);
                            }

                        }

                    }

                    #endregion

                    #region Раздел 6.7
                    if (razd_6_1.Descendants().Any(x => x.Name.LocalName == "СведенияОсуммеВыплатИвознагражденийПоДопТарифу"))
                    {
                        var razd_6_7_list = razd_6_1.Descendants().Where(x => x.Name.LocalName == "СведенияОсуммеВыплатИвознагражденийПоДопТарифу");
                        List<string> specOcenkaList = razd_6_7_list.Descendants().Where(x => x.Name.LocalName == "КодСпециальнойОценкиУсловийТруда").Select(x => x.Value).Distinct().ToList();

                        foreach (var item in specOcenkaList)
                        {
                            SpecOcenkaUslTruda specOcenka = SpecOcenkaUslTruda_list.FirstOrDefault(x => x.Code == item);

                            FormsRSW2014_1_Razd_6_7 rsw_6_7 = new FormsRSW2014_1_Razd_6_7 { FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID };

                            if (specOcenka != null)
                            {
                                rsw_6_7.SpecOcenkaUslTrudaID = specOcenka.ID;
                            }

                            foreach (var razd_6_7 in razd_6_7_list.Where(x => x.Element("КодСпециальнойОценкиУсловийТруда").Value == item))
                            {
                                string name = "";
                                string strCode = razd_6_7.Element("КодСтроки").Value.ToString();
                                name = strCode.Substring(strCode.Length - 1, 1);

                                decimal data = 0;
                                for (int i = 0; i <= 1; i++)
                                {

                                    string itemName = "s_" + name + "_";
                                    string nameStr = "";
                                    switch (i)
                                    {
                                        case 0:
                                            nameStr = "СуммаВыплатПоДопТарифу27-1";
                                            break;
                                        case 1:
                                            nameStr = "СуммаВыплатПоДопТарифу27-2-18";
                                            break;
                                    }

                                    if (nameStr != "" && razd_6_7.Element(nameStr) != null)
                                        data = decimal.Parse(razd_6_7.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                    if (nameStr != "" && razd_6_7.Element(nameStr) == null)
                                        data = 0;

                                    itemName = itemName + i.ToString();

                                    rsw_6_7.GetType().GetProperty(itemName).SetValue(rsw_6_7, data, null);


                                }

                            }

                            rsw_6_1.FormsRSW2014_1_Razd_6_7.Add(rsw_6_7);

                        }

                    }


                    #endregion

                    #region Записи о стаже
                    if (razd_6_1.Descendants().Any(x => x.Name.LocalName == "СтажевыйПериод"))
                    {
                        var staj_osn_list = razd_6_1.Descendants().Where(x => x.Name.LocalName == "СтажевыйПериод");

                        int n = 0;

                        foreach (var staj_osn in staj_osn_list)
                        {
                            DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("ДатаНачалаПериода").Value.ToString());
                            DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("ДатаКонцаПериода").Value.ToString());

                            n++;
                            StajOsn stajOsn = new StajOsn { DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };
                            var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");
                            //перебираем льготный стаж если есть
                            int i = 0;
                            foreach (var item in staj_lgot_list)
                            {
                                string str = "";
                                i++;
                                var staj_lgot = item.Element("ОсобенностиУчета");
                                StajLgot stajLgot = new StajLgot { StajOsnID = stajOsn.ID, Number = i };

                                var terrUsl = staj_lgot.Element("ТерриториальныеУсловия");
                                if (terrUsl != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    str = terrUsl.Element("ОснованиеТУ").Value.ToString().ToUpper();
                                    if (TerrUsl_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.TerrUslID = TerrUsl_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (terrUsl.Element("Коэффициент") != null)
                                            stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                        else
                                            stajLgot.TerrUslKoef = 0;
                                    }
                                }

                                var osobUsl = staj_lgot.Element("ОсобыеУсловияТруда");
                                if (osobUsl != null)
                                {
                                    if (osobUsl.Element("ОснованиеОУТ") != null)
                                    {
                                        str = osobUsl.Element("ОснованиеОУТ").Value.ToString().ToUpper();
                                        if (OsobUslTruda_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.OsobUslTrudaID = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (osobUsl.Element("ПозицияСписка") != null)
                                    {
                                        str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                        if (KodVred_2_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == str);
                                            stajLgot.KodVred_OsnID = kv.ID;

                                            // проверка на наличие такой должности в базе
                                            if (db.Dolgn.Any(x => x.Name == kv.Name))
                                            {
                                                stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                            }
                                            else
                                            {
                                                Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                db.AddToDolgn(dolgn);
                                                db.SaveChanges();
                                                stajLgot.DolgnID = dolgn.ID;
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStaj = staj_lgot.Element("ИсчисляемыйСтаж");
                                if (ischislStrahStaj != null)
                                {
                                    str = ischislStrahStaj.Element("ОснованиеИС").Value.ToString().ToUpper();
                                    if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                        {
                                            if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                            {
                                                stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                            {
                                                stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                            }
                                        }
                                        if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                        {
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                            {
                                                stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                            {
                                                stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStajDop = staj_lgot.Element("ДекретДети");
                                if (ischislStrahStajDop != null)
                                {
                                    str = ischislStrahStajDop.Value.ToString().ToUpper();
                                    if (IschislStrahStajDop_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajDopID = IschislStrahStajDop_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    }
                                }


                                var uslDosrNazn = staj_lgot.Element("ВыслугаЛет");
                                if (uslDosrNazn != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    str = uslDosrNazn.Element("ОснованиеВЛ").Value.ToString().ToUpper();
                                    if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                        {
                                            if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                            {
                                                stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                            }
                                            if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                            {
                                                stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                            }
                                        }
                                        if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                        {
                                            if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                            {
                                                stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                            }
                                            if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                            {
                                                stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                            }
                                        }
                                        if (uslDosrNazn.Element("ДоляСтавки") != null)
                                        {
                                            stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                        }
                                    }
                                }

                                stajOsn.StajLgot.Add(stajLgot);
                            }

                            rsw_6_1.StajOsn.Add(stajOsn);

                        }



                    }
                    RSW_6_1_List.Add(rsw_6_1);


                    #endregion

                } // перебор инд. сведений

                #endregion

            }
            catch (Exception ex)
            {
                Methods.showAlert("Ошибка!", "При выводе на печать Формы ПФР Инд.сведения Раздел 6 произошла ошибка! Код ошибки: " + ex.Message, this.ThemeName);
            }
        }

        //импорт Инд. сведений СЗВ-6-4 из сформированных пачек во временные переменные
        private void tempImportSZV_6_4(string xmlString)
        {
            List<PlatCategory> PlatCatList = db.PlatCategory.Where(x => x.PlatCategoryRaschPerID == 4).ToList();

            XDocument doc = XDocument.Parse(xmlString);

            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            XElement node = doc.Root;

            try
            {
                #region // Поиск информации о составителе
                node = node.Descendants().First(x => x.Name.LocalName == "СоставительПачки");

                string regnum = node.Element("РегистрационныйНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);


                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                }
                #endregion


                #region перебор инд.сведений
                List<TypeInfo> typeInfo_ = db.TypeInfo.ToList();

                var szv_6_4_list = doc.Root.Descendants().Where(x => x.Name.LocalName == "СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ");

                //List<PU.ImportXML.staffContainer> staffList = new List<PU.ImportXML.staffContainer>();

                //foreach (var szv_6_4 in szv_6_4_list)
                //{
                //    var insn = szv_6_4.Element("СтраховойНомер").Value.ToString().Trim().Split(' ');
                //    string InsuranceNum = insn[0];
                //    while (InsuranceNum.Contains("-"))
                //        InsuranceNum = InsuranceNum.Remove(InsuranceNum.IndexOf('-'), 1);
                //    byte? contrNum = null;
                //    if (insn.Count() > 1)
                //    {
                //        try
                //        {
                //            contrNum = byte.Parse(insn[1]);
                //        }
                //        catch
                //        { }
                //    }

                //    string middleName = szv_6_4.Element("ФИО").Element("Отчество") != null ? szv_6_4.Element("ФИО").Element("Отчество").Value.ToString() : "";

                //    var st = new PU.ImportXML.staffContainer
                //    {
                //        insuranceNum = InsuranceNum,
                //        insID = insurer.ID,
                //        lastName = szv_6_4.Element("ФИО").Element("Фамилия").Value.ToString(),
                //        firstName = szv_6_4.Element("ФИО").Element("Имя").Value.ToString(),
                //        middleName = middleName,
                //        contrNum = contrNum
                //    };

                //    staffList.Add(st);
                //}

                //var listid = staffList.Select(y => y.insuranceNum).ToList();
                //var listNum = db.Staff.Where(x => x.InsurerID == insurer.ID && listid.Contains(x.InsuranceNumber)).Select(x => x.InsuranceNumber).ToList();

                //staffList = staffList.Where(x => !listNum.Contains(x.insuranceNum)).ToList();  // те сотрудники которых нет в базе для текущего страхователя

                //foreach (var item in staffList)
                //{
                //    Staff staff_ = new Staff
                //    {
                //        InsuranceNumber = item.insuranceNum,
                //        InsurerID = item.insID,
                //        LastName = item.lastName,
                //        FirstName = item.firstName,
                //        MiddleName = item.middleName,
                //        ControlNumber = item.contrNum,
                //        Dismissed = 0
                //    };

                //    db.AddToStaff(staff_);
                //}
                //if (staffList.Count > 0)
                //    db.SaveChanges();


                foreach (var szv_6_4 in szv_6_4_list)
                {
                    var insn = szv_6_4.Element("СтраховойНомер").Value.ToString().Trim().Split(' ');
                    string InsuranceNum = insn[0];
                    while (InsuranceNum.Contains("-"))
                        InsuranceNum = InsuranceNum.Remove(InsuranceNum.IndexOf('-'), 1);


                    Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);

                    string tInfo = szv_6_4.Element("ТипСведений").Value.ToString().ToLower();
                    long tInfoID = typeInfo_.First(x => x.Name.ToLower() == tInfo).ID;
                    string pcCode = szv_6_4.Element("КодКатегории").Value.ToString().ToUpper();


                    long pcID = PlatCatList.FirstOrDefault(x => x.Code == pcCode).ID;



                    byte typeContr = szv_6_4.Element("ТипДоговора").Value.ToString().ToUpper() == "ТРУДОВОЙ" ? (byte)1 : (byte)2;

                    byte q = byte.Parse(szv_6_4.Element("ОтчетныйПериод").Element("Квартал").Value.ToString());
                    short y = short.Parse(szv_6_4.Element("ОтчетныйПериод").Element("Год").Value.ToString());


                    FormsSZV_6_4 szv64 = new FormsSZV_6_4();
                    byte qk = (byte)0;
                    short yk = (short)0;
                    string regNumK = "";

                    if (tInfo != "исходная")
                    {
                        qk = byte.Parse(szv_6_4.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value.ToString());
                        yk = short.Parse(szv_6_4.Element("КорректируемыйОтчетныйПериод").Element("Год").Value.ToString());
                        if (szv_6_4.Element("РегистрационныйНомерКорректируемогоПериода") != null)
                        {
                            regNumK = szv_6_4.Element("РегистрационныйНомерКорректируемогоПериода").Value.ToString();
                            while (regNumK.Contains("-"))
                                regNumK = regNumK.Remove(regNumK.IndexOf('-'), 1);
                        }
                    }


                    szv64.InsurerID = insurer.ID;
                    szv64.StaffID = staff.ID;
                    szv64.Year = y;
                    szv64.Quarter = q;
                    szv64.PlatCategoryID = pcID;
                    szv64.TypeContract = typeContr;

                    if (yk != 0)
                    {
                        szv64.YearKorr = yk;
                        szv64.QuarterKorr = qk;
                        szv64.RegNumKorr = regNumK;
                    }
                    szv64.TypeInfoID = tInfoID;

                    if (szv_6_4.Element("СуммаВзносовНаСтраховую") != null)
                    {
                        szv64.SumFeePFR_Strah = decimal.Parse(szv_6_4.Element("СуммаВзносовНаСтраховую").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                        szv64.SumPayPFR_Strah = decimal.Parse(szv_6_4.Element("СуммаВзносовНаСтраховую").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        szv64.SumFeePFR_Strah = 0;
                        szv64.SumPayPFR_Strah = 0;
                    }
                    if (szv_6_4.Element("СуммаВзносовНаНакопительную") != null)
                    {
                        szv64.SumFeePFR_Nakop = decimal.Parse(szv_6_4.Element("СуммаВзносовНаНакопительную").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                        szv64.SumPayPFR_Nakop = decimal.Parse(szv_6_4.Element("СуммаВзносовНаНакопительную").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        szv64.SumFeePFR_Nakop = 0;
                        szv64.SumPayPFR_Nakop = 0;
                    }

                    var sumList = szv_6_4.Descendants().Where(x => x.Name.LocalName == "СуммаВыплатИвознагражденийВпользуЗЛ");

                    foreach (var item in sumList)
                    {
                        int m = 0;
                        if (item.Element("ТипСтроки").Value == "МЕСЦ")
                        {
                            string strMonth = item.Element("Месяц").Value.ToString();
                            m = int.Parse(strMonth);
                            if (m == 4 || m == 7 || m == 10)
                                m = 1;
                            else if (m == 5 || m == 8 || m == 11)
                                m = 2;
                            else if (m == 6 || m == 9 || m == 12)
                                m = 3;
                        }

                        decimal data = 0;
                        for (int i = 0; i <= 2; i++)
                        {
                            string itemName = "s_" + m + "_" + i.ToString();
                            string nameStr = "";
                            switch (i)
                            {
                                case 0:
                                    nameStr = "СуммаВыплатВсего";
                                    break;
                                case 1:
                                    nameStr = "СуммаВыплатНачисленыСтраховыеВзносыНеПревышающие";
                                    break;
                                case 2:
                                    nameStr = "СуммаВыплатНачисленыСтраховыеВзносыПревышающие";
                                    break;
                            }

                            if (nameStr != "" && item.Element(nameStr) != null)
                                data = decimal.Parse(item.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                            if (nameStr != "" && item.Element(nameStr) == null)
                                data = 0;

                            szv64.GetType().GetProperty(itemName).SetValue(szv64, data, null);

                        }
                    }

                    sumList = szv_6_4.Descendants().Where(x => x.Name.LocalName == "СуммаВыплатИвознагражденийПоДопТарифу");

                    foreach (var item in sumList)
                    {
                        int m = 0;
                        if (item.Element("ТипСтроки").Value == "МЕСЦ")
                        {
                            string strMonth = item.Element("Месяц").Value.ToString();
                            m = int.Parse(strMonth);
                            if (m == 4 || m == 7 || m == 10)
                                m = 1;
                            else if (m == 5 || m == 8 || m == 11)
                                m = 2;
                            else if (m == 6 || m == 9 || m == 12)
                                m = 3;
                        }

                        decimal data = 0;
                        for (int i = 0; i <= 1; i++)
                        {
                            string itemName = "d_" + m + "_" + i.ToString();
                            string nameStr = "";
                            switch (i)
                            {
                                case 0:
                                    nameStr = "СуммаВыплатПоДопТарифу27-1";
                                    break;
                                case 1:
                                    nameStr = "СуммаВыплатПоДопТарифу27-2-18";
                                    break;
                            }

                            if (nameStr != "" && item.Element(nameStr) != null)
                                data = decimal.Parse(item.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                            if (nameStr != "" && item.Element(nameStr) == null)
                                data = 0;

                            szv64.GetType().GetProperty(itemName).SetValue(szv64, data, null);
                        }
                    }

                    szv64.DateFilling = DateTime.Parse(szv_6_4.Element("ДатаЗаполнения").Value.ToString());


                    #region Записи о стаже
                    if (szv_6_4.Descendants().Any(x => x.Name.LocalName == "СтажевыйПериод"))
                    {
                        //        db.SaveChanges();
                        var staj_osn_list = szv_6_4.Descendants().Where(x => x.Name.LocalName == "СтажевыйПериод");

                        int n = 0;

                        foreach (var staj_osn in staj_osn_list)
                        {
                            DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("ДатаНачалаПериода").Value.ToString());
                            DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("ДатаКонцаПериода").Value.ToString());

                            n++;
                            StajOsn stajOsn = new StajOsn { DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };

                            var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");

                            //перебираем льготный стаж если есть
                            int i = 0;
                            foreach (var item in staj_lgot_list)
                            {
                                string str = "";
                                i++;
                                var staj_lgot = item.Element("ОсобенностиУчета");
                                StajLgot stajLgot = new StajLgot { Number = i };

                                var terrUsl = staj_lgot.Element("ТерриториальныеУсловия");
                                if (terrUsl != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    str = terrUsl.Element("ОснованиеТУ").Value.ToString().ToUpper();
                                    if (TerrUsl_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.TerrUslID = TerrUsl_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (terrUsl.Element("Коэффициент") != null)
                                            stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                        else
                                            stajLgot.TerrUslKoef = 0;
                                    }
                                }

                                var osobUsl = staj_lgot.Element("ОсобыеУсловияТруда");
                                if (osobUsl != null)
                                {
                                    if (osobUsl.Element("ОснованиеОУТ") != null)
                                    {
                                        str = osobUsl.Element("ОснованиеОУТ").Value.ToString().ToUpper();
                                        if (OsobUslTruda_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.OsobUslTrudaID = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (osobUsl.Element("ПозицияСписка") != null)
                                    {
                                        str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                        if (KodVred_2_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == str);
                                            stajLgot.KodVred_OsnID = kv.ID;

                                            // проверка на наличие такой должности в базе
                                            if (db.Dolgn.Any(x => x.Name == kv.Name))
                                            {
                                                stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                            }
                                            else
                                            {
                                                Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                db.AddToDolgn(dolgn);
                                                db.SaveChanges();
                                                stajLgot.DolgnID = dolgn.ID;
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStaj = staj_lgot.Element("ИсчисляемыйСтаж");
                                if (ischislStrahStaj != null)
                                {
                                    str = ischislStrahStaj.Element("ОснованиеИС").Value.ToString().ToUpper();
                                    if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                        {
                                            if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                            {
                                                stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                            {
                                                stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                            }
                                        }
                                        if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                        {
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                            {
                                                stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                            {
                                                stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStajDop = staj_lgot.Element("ДекретДети");
                                if (ischislStrahStajDop != null)
                                {
                                    str = ischislStrahStajDop.Value.ToString().ToUpper();
                                    if (IschislStrahStajDop_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajDopID = IschislStrahStajDop_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    }
                                }


                                var uslDosrNazn = staj_lgot.Element("ВыслугаЛет");
                                if (uslDosrNazn != null) // если есть 
                                {
                                    //если есть запись в базе
                                    str = uslDosrNazn.Element("ОснованиеВЛ").Value.ToString().ToUpper();
                                    if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                        {
                                            if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                            {
                                                stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                            }
                                            if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                            {
                                                stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                            }
                                        }
                                        if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                        {
                                            if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                            {
                                                stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                            }
                                            if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                            {
                                                stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                            }
                                        }
                                        if (uslDosrNazn.Element("ДоляСтавки") != null)
                                        {
                                            stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                        }
                                    }
                                }

                                stajOsn.StajLgot.Add(stajLgot);
                            }

                            szv64.StajOsn.Add(stajOsn);

                        }

                    }

                    SZV_6_4_List.Add(szv64);

                    #endregion

                } // перебор инд. сведений

                #endregion
            }
            catch (Exception ex)
            {
                Methods.showAlert("Ошибка!", "При выводе на печать Формы СЗВ-6-4 произошла ошибка! Код ошибки: " + ex.Message, this.ThemeName);
            }

        }

        //импорт Инд. сведений СЗВ-6-1 и СЗВ-6-2 из сформированных пачек во временные переменные
        private void tempImportSZV_6(string xmlString, string formType)
        {
            List<PlatCategory> PlatCatList = db.PlatCategory.Where(x => x.PlatCategoryRaschPerID == 4).ToList();

            XDocument doc = XDocument.Parse(xmlString);

            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            XElement node = doc.Root;

            try
            {
                #region // Поиск информации о составителе
                node = node.Descendants().First(x => x.Name.LocalName == "СоставительПачки");

                string regnum = node.Element("РегистрационныйНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);


                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                }
                #endregion


                #region перебор инд.сведений
                List<TypeInfo> typeInfo_ = db.TypeInfo.ToList();

                var szv_6_list = doc.Root.Descendants().Where(x => x.Name.LocalName == "СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ");

                List<FormsSZV_6> SZV_6_2_List_temp = new List<FormsSZV_6>();

                foreach (var szv_6 in szv_6_list)
                {
                    var insn = szv_6.Element("СтраховойНомер").Value.ToString().Trim().Split(' ');
                    string InsuranceNum = insn[0];
                    while (InsuranceNum.Contains("-"))
                        InsuranceNum = InsuranceNum.Remove(InsuranceNum.IndexOf('-'), 1);


                    Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);

                    string tInfo = szv_6.Element("ТипСведений").Value.ToString().ToLower();
                    long tInfoID = typeInfo_.First(x => x.Name.ToLower() == tInfo).ID;
                    string pcCode = szv_6.Element("КодКатегории").Value.ToString().ToUpper();


                    long pcID = PlatCatList.FirstOrDefault(x => x.Code == pcCode).ID;

                    byte q = byte.Parse(szv_6.Element("ОтчетныйПериод").Element("Квартал").Value.ToString());
                    short y = short.Parse(szv_6.Element("ОтчетныйПериод").Element("Год").Value.ToString());


                    FormsSZV_6 szv6 = new FormsSZV_6();
                    byte qk = (byte)0;
                    short yk = (short)0;

                    if (tInfo != "исходная")
                    {
                        qk = byte.Parse(szv_6.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value.ToString());
                        yk = short.Parse(szv_6.Element("КорректируемыйОтчетныйПериод").Element("Год").Value.ToString());
                    }


                    szv6.InsurerID = insurer.ID;
                    szv6.StaffID = staff.ID;
                    szv6.Year = y;
                    szv6.Quarter = q;
                    szv6.PlatCategoryID = pcID;

                    if (yk != 0)
                    {
                        szv6.YearKorr = yk;
                        szv6.QuarterKorr = qk;
                    }
                    szv6.TypeInfoID = tInfoID;

                    if (szv_6.Element("СуммаВзносовНаСтраховую") != null)
                    {
                        szv6.SumFeePFR_Strah = decimal.Parse(szv_6.Element("СуммаВзносовНаСтраховую").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                        szv6.SumPayPFR_Strah = decimal.Parse(szv_6.Element("СуммаВзносовНаСтраховую").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        szv6.SumFeePFR_Strah = 0;
                        szv6.SumPayPFR_Strah = 0;
                    }
                    if (szv_6.Element("СуммаВзносовНаНакопительную") != null)
                    {
                        szv6.SumFeePFR_Nakop = decimal.Parse(szv_6.Element("СуммаВзносовНаНакопительную").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                        szv6.SumPayPFR_Nakop = decimal.Parse(szv_6.Element("СуммаВзносовНаНакопительную").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        szv6.SumFeePFR_Nakop = 0;
                        szv6.SumPayPFR_Nakop = 0;
                    }

                    szv6.DateFilling = DateTime.Parse(szv_6.Element("ДатаЗаполнения").Value.ToString());


                    #region Записи о стаже
                    if (szv_6.Descendants().Any(x => x.Name.LocalName == "СтажевыйПериод"))
                    {
                        //        db.SaveChanges();
                        var staj_osn_list = szv_6.Descendants().Where(x => x.Name.LocalName == "СтажевыйПериод");

                        int n = 0;

                        foreach (var staj_osn in staj_osn_list)
                        {
                            DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("ДатаНачалаПериода").Value.ToString());
                            DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("ДатаКонцаПериода").Value.ToString());

                            n++;
                            StajOsn stajOsn = new StajOsn { DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };

                            var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");

                            //перебираем льготный стаж если есть
                            int i = 0;
                            foreach (var item in staj_lgot_list)
                            {
                                string str = "";
                                i++;
                                var staj_lgot = item.Element("ОсобенностиУчета");
                                StajLgot stajLgot = new StajLgot { Number = i };

                                var terrUsl = staj_lgot.Element("ТерриториальныеУсловия");
                                if (terrUsl != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    str = terrUsl.Element("ОснованиеТУ").Value.ToString().ToUpper();
                                    if (TerrUsl_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.TerrUslID = TerrUsl_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (terrUsl.Element("Коэффициент") != null)
                                            stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                        else
                                            stajLgot.TerrUslKoef = 0;
                                    }
                                }

                                var osobUsl = staj_lgot.Element("ОсобыеУсловияТруда");
                                if (osobUsl != null)
                                {
                                    if (osobUsl.Element("ОснованиеОУТ") != null)
                                    {
                                        str = osobUsl.Element("ОснованиеОУТ").Value.ToString().ToUpper();
                                        if (OsobUslTruda_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.OsobUslTrudaID = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (osobUsl.Element("ПозицияСписка") != null)
                                    {
                                        str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                        if (KodVred_2_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == str);
                                            stajLgot.KodVred_OsnID = kv.ID;

                                            // проверка на наличие такой должности в базе
                                            if (db.Dolgn.Any(x => x.Name == kv.Name))
                                            {
                                                stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                            }
                                            else
                                            {
                                                Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                db.AddToDolgn(dolgn);
                                                db.SaveChanges();
                                                stajLgot.DolgnID = dolgn.ID;
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStaj = staj_lgot.Element("ИсчисляемыйСтаж");
                                if (ischislStrahStaj != null)
                                {
                                    str = ischislStrahStaj.Element("ОснованиеИС").Value.ToString().ToUpper();
                                    if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                        {
                                            if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                            {
                                                stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                            {
                                                stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                            }
                                        }
                                        if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                        {
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                            {
                                                stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                            {
                                                stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStajDop = staj_lgot.Element("ДекретДети");
                                if (ischislStrahStajDop != null)
                                {
                                    str = ischislStrahStajDop.Value.ToString().ToUpper();
                                    if (IschislStrahStajDop_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajDopID = IschislStrahStajDop_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    }
                                }


                                var uslDosrNazn = staj_lgot.Element("ВыслугаЛет");
                                if (uslDosrNazn != null) // если есть 
                                {
                                    //если есть запись в базе
                                    str = uslDosrNazn.Element("ОснованиеВЛ").Value.ToString().ToUpper();
                                    if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                        {
                                            if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                            {
                                                stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                            }
                                            if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                            {
                                                stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                            }
                                        }
                                        if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                        {
                                            if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                            {
                                                stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                            }
                                            if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                            {
                                                stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                            }
                                        }
                                        if (uslDosrNazn.Element("ДоляСтавки") != null)
                                        {
                                            stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                        }
                                    }
                                }

                                stajOsn.StajLgot.Add(stajLgot);
                            }

                            szv6.StajOsn.Add(stajOsn);

                        }

                    }

                    switch (formType)
                    {
                        case "СЗВ-6-1":
                            SZV_6_1_List.Add(szv6);
                            break;
                        case "СЗВ-6-2":
                            SZV_6_2_List_temp.Add(szv6);
                            break;
                    }


                    #endregion

                } // перебор инд. сведений
                SZV_6_2_List.Add(SZV_6_2_List_temp);
                #endregion
            }
            catch (Exception ex)
            {
                Methods.showAlert("Ошибка!", "При выводе на печать Формы СЗВ-6-4 произошла ошибка! Код ошибки: " + ex.Message, this.ThemeName);
            }

        }


        private void printSPW2_Click(object sender, EventArgs e)
        {
            printReportSPW2(false);
        }

        private void printReportSPW2(bool printCurrent)
        {
            if (packsGrid.RowCount != 0 && packsGrid.CurrentRow.Cells[1].Value != null)
            {
                if (!printCurrent)
                {

                    if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СПВ-2"))
                    {
                        var filesSPW2 = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "СПВ-2");

                        List<long> sourceIDList = new List<long>();
                        foreach (var item in filesSPW2)
                        {
                            List<long> sourceIDList_temp = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

                            sourceIDList = sourceIDList.Concat(sourceIDList_temp).ToList();
                        }

                        SPW_2_List = db.FormsSPW2.Where(x => sourceIDList.Contains(x.ID)).ToList();


                    }
                    else
                    {
                        RadMessageBox.Show(this, "Нет данных для печати", "");
                        return;
                    }
                }
                else
                {
                    long id = Convert.ToInt64(packsGrid.CurrentRow.Cells[1].Value);
                    var xmlInfoT = dbxml.xmlInfo.FirstOrDefault(x => x.ID == id);

                    List<long> sourceIDList_temp = new List<long>();

                    if (xmlInfoT.StaffList.Any())
                        sourceIDList_temp = xmlInfoT.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

                    if (sourceIDList_temp.Any())
                        SPW_2_List = db.FormsSPW2.Where(x => sourceIDList_temp.Contains(x.ID)).ToList();
                }

                if (!SPW_2_List.Any())
                {
                    RadMessageBox.Show(this, "Нет данных для печати", "");
                    return;
                }

                ReportViewerSPW2 = new SPW2_Print();
                ReportViewerSPW2.SPW_2_List = SPW_2_List;
                ReportViewerSPW2.Owner = this;
                ReportViewerSPW2.ThemeName = this.ThemeName;
                ReportViewerSPW2.ShowInTaskbar = false;


                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerSPW2.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedSPW2);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void bw_RunWorkerCompletedSPW2(object sender, RunWorkerCompletedEventArgs e)
        {

            ReportViewerSPW2.ShowDialog();

            SPW_2_List = new List<FormsSPW2>();
        }

        //импорт Инд. сведений Раздел 6.1 из сформированных пачек во временные переменные
        private void tempImportSPW2(string xmlString)
        {
            XDocument doc = XDocument.Parse(xmlString);

            XElement node = doc.Root;

            try
            {
                #region // Поиск информации о составителе
                node = node.Descendants().First(x => x.Name.LocalName == "СоставительПачки");

                doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

                foreach (var elem in doc.Descendants())
                    elem.Name = elem.Name.LocalName;

                string regnum = node.Element("РегистрационныйНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                }
                #endregion


                #region перебор СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ
                List<TypeInfo> typeInfo_ = db.TypeInfo.ToList();

                var spv_2_list = doc.Root.Descendants().Where(x => x.Name.LocalName == "СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ");

                List<PU.ImportXML.staffContainer> staffList = new List<PU.ImportXML.staffContainer>();
                foreach (var spv_2 in spv_2_list)
                {
                    var insn = spv_2.Element("СтраховойНомер").Value.ToString().Trim().Split(' ');
                    string InsuranceNum = insn[0];
                    while (InsuranceNum.Contains("-"))
                        InsuranceNum = InsuranceNum.Remove(InsuranceNum.IndexOf('-'), 1);
                    byte? contrNum = null;
                    if (insn.Count() > 1)
                    {
                        try
                        {
                            contrNum = byte.Parse(insn[1]);
                        }
                        catch
                        { }
                    }

                    string middleName = spv_2.Element("ФИО").Element("Отчество") != null ? spv_2.Element("ФИО").Element("Отчество").Value.ToString() : "";

                    staffList.Add(new PU.ImportXML.staffContainer
                    {
                        insuranceNum = InsuranceNum,
                        insID = insurer.ID,
                        lastName = spv_2.Element("ФИО").Element("Фамилия").Value.ToString(),
                        firstName = spv_2.Element("ФИО").Element("Имя").Value.ToString(),
                        middleName = middleName,
                        contrNum = contrNum
                    });
                }

                var listid = staffList.Select(y => y.insuranceNum).ToList();
                var listNum = db.Staff.Where(x => x.InsurerID == insurer.ID && listid.Contains(x.InsuranceNumber)).Select(x => x.InsuranceNumber).ToList();

                staffList = staffList.Where(x => !listNum.Contains(x.insuranceNum)).ToList();  // те сотрудники которых нет в базе для текущего страхователя

                foreach (var item in staffList)
                {
                    Staff staff_ = new Staff
                    {
                        InsuranceNumber = item.insuranceNum,
                        InsurerID = item.insID,
                        LastName = item.lastName,
                        FirstName = item.firstName,
                        MiddleName = item.middleName,
                        ControlNumber = item.contrNum,
                        Dismissed = 0
                    };

                    db.AddToStaff(staff_);
                }
                if (staffList.Count > 0)
                    db.SaveChanges();


                foreach (var spv_2 in spv_2_list)
                {
                    var insn = spv_2.Element("СтраховойНомер").Value.ToString().Trim().Split(' ');
                    string InsuranceNum = insn[0];
                    while (InsuranceNum.Contains("-"))
                        InsuranceNum = InsuranceNum.Remove(InsuranceNum.IndexOf('-'), 1);


                    Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);

                    string tInfo = spv_2.Element("ТипСведений").Value.ToString().ToLower();
                    long tInfoID = typeInfo_.First(x => x.Name.ToLower() == tInfo).ID;

                    byte q = byte.Parse(spv_2.Element("ОтчетныйПериод").Element("Квартал").Value.ToString());
                    short y = short.Parse(spv_2.Element("ОтчетныйПериод").Element("Год").Value.ToString());

                    FormsSPW2 spw2 = new FormsSPW2();
                    byte qk = (byte)0;
                    short yk = (short)0;

                    spw2.InsurerID = insurer.ID;
                    spw2.StaffID = staff.ID;
                    spw2.Year = y;
                    spw2.Quarter = q;

                    if (yk != 0)
                    {
                        spw2.YearKorr = yk;
                        spw2.QuarterKorr = qk;
                    }
                    spw2.TypeInfoID = tInfoID;

                    if (spv_2.Element("ПризнакНачисленияВзносовОПС") != null)
                    {
                        spw2.ExistsInsurOPS = spv_2.Element("ПризнакНачисленияВзносовОПС").Value.ToString() == "ДА" ? (byte)1 : (byte)0;
                    }
                    else
                        spw2.ExistsInsurOPS = 0;

                    if (spv_2.Element("ПризнакНачисленияВзносовПоДопТарифу") != null)
                    {
                        spw2.ExistsInsurDop = spv_2.Element("ПризнакНачисленияВзносовПоДопТарифу").Value.ToString() == "ДА" ? (byte)1 : (byte)0;
                    }
                    else
                        spw2.ExistsInsurDop = 0;

                    string catCode = spv_2.Element("КодКатегории").Value.ToString().ToUpper();
                    PlatCategory platCat = db.PlatCategory.FirstOrDefault(x => x.Code == catCode);

                    if (platCat != null)
                    {
                        spw2.PlatCategoryID = platCat.ID;
                    }
                    else
                        break;

                    spw2.DateFilling = DateTime.Parse(spv_2.Element("ДатаЗаполнения").Value.ToString());
                    spw2.DateComposit = DateTime.Parse(spv_2.Element("ДатаСоставленияНа").Value.ToString());


                    #region Записи о стаже
                    if (spv_2.Descendants().Any(x => x.Name.LocalName == "СтажевыйПериод"))
                    {
                        var staj_osn_list = spv_2.Descendants().Where(x => x.Name.LocalName == "СтажевыйПериод");

                        int n = 0;

                        foreach (var staj_osn in staj_osn_list)
                        {
                            DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("ДатаНачалаПериода").Value.ToString());
                            DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("ДатаКонцаПериода").Value.ToString());

                            n++;
                            StajOsn stajOsn = new StajOsn { FormsSPW2_ID = spw2.ID, DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };


                            var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");
                            //перебираем льготный стаж если есть
                            int i = 0;
                            foreach (var item in staj_lgot_list)
                            {
                                string str = "";
                                i++;
                                var staj_lgot = item.Element("ОсобенностиУчета");
                                StajLgot stajLgot = new StajLgot { StajOsnID = stajOsn.ID, Number = i };

                                var terrUsl = staj_lgot.Element("ТерриториальныеУсловия");
                                if (terrUsl != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    str = terrUsl.Element("ОснованиеТУ").Value.ToString().ToUpper();
                                    if (db.TerrUsl.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.TerrUslID = db.TerrUsl.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (terrUsl.Element("Коэффициент") != null)
                                            stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                        else
                                            stajLgot.TerrUslKoef = 0;
                                    }
                                }

                                var osobUsl = staj_lgot.Element("ОсобыеУсловияТруда");
                                if (osobUsl != null)
                                {
                                    if (osobUsl.Element("ОснованиеОУТ") != null)
                                    {
                                        str = osobUsl.Element("ОснованиеОУТ").Value.ToString().ToUpper();
                                        if (db.OsobUslTruda.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.OsobUslTrudaID = db.OsobUslTruda.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (osobUsl.Element("ПозицияСписка") != null)
                                    {
                                        str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                        if (db.KodVred_2.Any(x => x.Code.ToUpper() == str))
                                        {
                                            KodVred_2 kv = db.KodVred_2.FirstOrDefault(x => x.Code.ToUpper() == str);
                                            stajLgot.KodVred_OsnID = kv.ID;

                                            // проверка на наличие такой должности в базе
                                            if (db.Dolgn.Any(x => x.Name == kv.Name))
                                            {
                                                stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                            }
                                            else
                                            {
                                                Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                db.AddToDolgn(dolgn);
                                                db.SaveChanges();
                                                stajLgot.DolgnID = dolgn.ID;
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStaj = staj_lgot.Element("ИсчисляемыйСтаж");
                                if (ischislStrahStaj != null)
                                {
                                    str = ischislStrahStaj.Element("ОснованиеИС").Value.ToString().ToUpper();
                                    if (db.IschislStrahStajOsn.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajOsnID = db.IschislStrahStajOsn.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                        {
                                            if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                            {
                                                stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                            {
                                                stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                            }
                                        }
                                        if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                        {
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                            {
                                                stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                            {
                                                stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStajDop = staj_lgot.Element("ДекретДети");
                                if (ischislStrahStajDop != null)
                                {
                                    str = ischislStrahStajDop.Value.ToString().ToUpper();
                                    if (db.IschislStrahStajDop.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajDopID = db.IschislStrahStajDop.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    }
                                }


                                var uslDosrNazn = staj_lgot.Element("ВыслугаЛет");
                                if (uslDosrNazn != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    str = uslDosrNazn.Element("ОснованиеВЛ").Value.ToString().ToUpper();
                                    if (db.UslDosrNazn.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.UslDosrNaznID = db.UslDosrNazn.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                        {
                                            if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                            {
                                                stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                            }
                                            if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                            {
                                                stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                            }
                                        }
                                        if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                        {
                                            if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                            {
                                                stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                            }
                                            if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                            {
                                                stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                            }
                                        }
                                        if (uslDosrNazn.Element("ДоляСтавки") != null)
                                        {
                                            stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                        }
                                    }
                                }

                                stajOsn.StajLgot.Add(stajLgot);
                            }

                            spw2.StajOsn.Add(stajOsn);

                        }


                    }
                    SPW_2_List.Add(spw2);


                    #endregion


                } // перебор инд. сведений

                #endregion

            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }

        }



        private void radMenuItem4_Click(object sender, EventArgs e)
        {
            if (packsGrid.RowCount > 0 && packsGrid.CurrentRow.Cells[1].Value != null)
            {
                string DocType = packsGrid.CurrentRow.Cells["type"].Value.ToString();
                long id = Convert.ToInt64(packsGrid.CurrentRow.Cells[1].Value);
                var xmlInfoT = dbxml.xmlInfo.FirstOrDefault(x => x.ID == id);
                List<long> IDlist;
                switch (DocType)
                {
                    case "РСВ":
                        //                        tempImportRSW(XmlContent);
                        printReport(0, true);
                        break;
                    case "ПФР":
                        //                        tempImportRSW6_1(XmlContent);
                        printReport(1, true);
                        break;
                    case "СЗВ-6-4":
                        //                        tempImportSZV_6_4(XmlContent);
                        printReport(1, true);
                        break;
                    case "СЗВ-6-1":
                        //                        tempImportSZV_6(XmlContent, "СЗВ-6-1");
                        printReport(1, true);
                        break;
                    case "СЗВ-6-2":
                        //                        tempImportSZV_6(XmlContent, "СЗВ-6-2");
                        printReport(1, true);
                        break;
                    case "СПВ-2":

                        //string XmlContent = xmlInfoT.xmlFile.First().XmlContent;

                        //fillDictions();
                        //tempImportSPW2(XmlContent);
                        printReportSPW2(true);
                        break;
                    case "ДСВ-3":
                        printReportDSW3(xmlInfoT.SourceID.Value);
                        break;
                    case "АДВ-1":
                        IDlist = dbxml.StaffList.Where(x => x.XmlInfoID == id).Select(x => x.StaffID.Value).ToList();
                        printReportADW1(IDlist);
                        break;
                    case "АДВ-2":
                        IDlist = dbxml.StaffList.Where(x => x.XmlInfoID == id && x.FormsRSW_6_1_ID.HasValue).Select(x => x.FormsRSW_6_1_ID.Value).ToList();
                        printReportADW2(IDlist);
                        break;


                }


            }

        }

        private void printDSW3_Click(object sender, EventArgs e)
        {
            if (packsGrid.RowCount > 0 && packsGrid.CurrentRow.Cells[1].Value != null)
            {
                long id = Convert.ToInt64(packsGrid.CurrentRow.Cells[1].Value);
                long SourceID = dbxml.xmlInfo.FirstOrDefault(x => x.ID == id).SourceID.Value;

                printReportDSW3(SourceID);

            }
            else
            {
                RadMessageBox.Show(this, "Не удалось напечатать форму", "");
            }

        }

        private void printReportDSW3(long id)
        {
            if (db.FormsDSW_3.Any(x => x.ID == id))
            {

                ReportViewerDSW3 = new DSW3_Print();
                ReportViewerDSW3.DSW3data = db.FormsDSW_3.FirstOrDefault(x => x.ID == id);
                ReportViewerDSW3.Owner = this;
                ReportViewerDSW3.ThemeName = this.ThemeName;
                ReportViewerDSW3.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerDSW3.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedDSW3);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Не удалось загрузить данные из базы данных для печати формы", "");
            }
        }

        private void bw_RunWorkerCompletedDSW3(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

            ReportViewerDSW3.ShowDialog();
        }

        private void bw_RunWorkerCompletedSaving(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() => { this.Cursor = Cursors.Default; }));
            if (!Options.hideDialogCheckFiles)
            {
                PU.Service.CheckFiles.CheckFilesQuestion child = new PU.Service.CheckFiles.CheckFilesQuestion();
                child.Owner = this;
                child.FileInfoList = FileInfoList;
                child.ThemeName = this.ThemeName;
                child.ShowDialog();

            }
            else if (Options.checkFilesAfterSaving)
            {
                PU.Service.CheckFiles.CheckFilesList child = new PU.Service.CheckFiles.CheckFilesList();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.FileInfoList = FileInfoList;
                child.ShowDialog();
            }
        }

        private void exportFiles(object sender, DoWorkEventArgs e)
        {
            var xmlInfoList = dbxml.xmlInfo.Where(x => idList.Contains(x.ID)).ToList();
            System.Threading.Thread.Sleep(750);

            try
            {
                XDocument xml;
                FileInfoList = new List<string>(); // Список файлов для передачи в окно проверки

                foreach (var item in xmlInfoList)
                {
                    string fileName = folderBrowserDialog.SelectedPath + "\\" + item.FileName;

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    xml = new XDocument();
                    bool useUTF8 = false;

                    switch (item.DocType)
                    {
                        case "РСВ":
                            xml = generateXML_RSW2014(item);
                            break;
                        case "ПФР":
                            xml = generateXML_RSW2014_6(item);
                            break;
                        case "СЗВ-6-4":
                            xml = generateXML_SZV_6_4(item);
                            break;
                        case "СЗВ-6-1":
                            xml = generateXML_SZV_6_1(item);
                            break;
                        case "СЗВ-6-2":
                            xml = generateXML_SZV_6_1(item);
                            break;
                        case "СПВ-2":
                            xml = generateXML_SPW2_2014(item);
                            break;
                        case "ДСВ-3":
                            xml = generateXML_DSW3(item);
                            break;
                        case "АДВ-1":
                            xml = generateXML_ADW1(item);
                            break;
                        case "АДВ-2":
                            xml = generateXML_ADW2(item);
                            break;
                        case "АДВ-3":
                            xml = generateXML_ADW3(item);
                            break;
                        case "ОДВ-1":
                            useUTF8 = true;
                            xml = pregenerateXML_ODV1(item, 0);
                            break;
                        case "ОДВ-1 СЗВ-СТАЖ":
                            useUTF8 = true;
                            xml = pregenerateXML_ODV1(item, 1);
                            break;
                        case "ОДВ-1 СЗВ-ИСХ":
                            useUTF8 = true;
                            xml = pregenerateXML_ODV1(item, 2);
                            break;
                        case "ОДВ-1 СЗВ-КОРР":
                            useUTF8 = true;
                            xml = pregenerateXML_ODV1(item, 3);
                            break;
                    }
                    //XDocument doc = XDocument.Parse(dbxml.xmlFile.First(x => x.XmlInfoID == item.ID).XmlContent);
                    //doc.Declaration = new XDeclaration("1.0", "Windows-1251", "yes");

                    //if (item.DocType == "РСВ")
                    //{

                    //    long rswID = dbxml.xmlInfo.FirstOrDefault(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "РСВ").SourceID.Value; ;

                    //    doc = updateRSV1_Razd_2_5(doc, rswID);

                    //}
                    if (xml != null)
                    {
                        if (useUTF8)
                        {
                            using (var writer = new XmlTextWriter(fileName, new UTF8Encoding(false)) { Formatting = Formatting.Indented })
                            {
                                xml.Save(writer);
                            }
                        }
                        else
                        {
                            xml.Save(fileName);
                        }
                        xml = null;

                        FileInfoList.Add(fileName);
                    }


                }



                //Assembly a = Assembly.Load("C:\\CheckPfr\\CheckUfa.dll");
                //Object o = a.CreateInstance("CheckUfa");
                //Type t = a.GetType("CheckUfa");


                //if (res.Count() > 0)
                //{ 
                //    Process.Start(fileTest+".log.html");
                //}
                //else
                //    this.Invoke(new Action(() => { MessageBox.Show("При проверке файла произошла ошибка!"); }));

            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении файлов произошла ошибка.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));


                return;
            }

            this.Invoke(new Action(() => { Methods.showAlert("Сохранение", "Файлы были успешно сохранены!", this.ThemeName); }));
        }




        private class razd66Period
        {
            public long ID { get; set; }
            public short Y { get; set; }
            public byte Q { get; set; }
        }

        List<FormsRSW2014_1_Razd_6_1> rsw61List = new List<FormsRSW2014_1_Razd_6_1>();
        List<FormsSZV_6_4> szv64List = new List<FormsSZV_6_4>();
        List<FormsSZV_6> szv6List = new List<FormsSZV_6>();
        List<List<FormsRSW2014_1_Razd_6_1>> rsw61List_part = new List<List<FormsRSW2014_1_Razd_6_1>>();
        List<List<FormsSZV_6_4>> szv64List_part = new List<List<FormsSZV_6_4>>();
        List<List<FormsSZV_6>> szv61List_part = new List<List<FormsSZV_6>>();
        List<List<FormsSZV_6>> szv62List_part = new List<List<FormsSZV_6>>();
        List<RaschetPeriodContainer> korrPeriods = new List<RaschetPeriodContainer>();

        //       List<StajOsn> stajOsn_list = new List<StajOsn>();
        //       List<StajLgot> stajLgot_list = new List<StajLgot>();
        //       List<FormsRSW2014_1_Razd_6_4> razd64_list = new List<FormsRSW2014_1_Razd_6_4>();
        //       List<FormsRSW2014_1_Razd_6_6> razd66_list = new List<FormsRSW2014_1_Razd_6_6>();
        //       List<FormsRSW2014_1_Razd_6_7> razd67_list = new List<FormsRSW2014_1_Razd_6_7>();
        //       List<Staff> StaffList_All_Ishod = new List<Staff>();


        private XDocument generateXML_SPW2_2014(xmlInfo item)
        {
            //      string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";
            int num = 1;
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ = new XElement("ВХОДЯЩАЯ_ОПИСЬ",
                                                                  new XElement("НомерВпачке", num),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRIP))
            {
                СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRUL))
            {
                СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            }

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            string NameShort = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    NameShort = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    NameShort = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));


            ВХОДЯЩАЯ_ОПИСЬ.Add(СоставительПачки);
            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));


            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ);


            #region СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            long spw2Id = item.StaffList.First().FormsRSW_6_1_ID.Value;
            FormsSPW2 spw2 = db.FormsSPW2.FirstOrDefault(x => x.ID == spw2Id);
            string codeCat = spw2.PlatCategory.Code;

            string q = "";

            switch (spw2.Quarter)
            {
                case 0:
                    q = "31.12.";
                    break;
                case 3:
                    q = "31.03.";
                    break;
                case 6:
                    q = "30.06.";
                    break;
                case 9:
                    q = "30.09.";
                    break;
            }
            string periodName = String.Format("С 01.01.{0} ПО {1}{0}", spw2.Year, q);
            string periodNameKorr = "";

            if (spw2.YearKorr != null && spw2.YearKorr != 0)
            {
                q = "";
                switch (spw2.QuarterKorr)
                {
                    case 0:
                        q = "31.12.";
                        break;
                    case 3:
                        q = "31.03.";
                        break;
                    case 6:
                        q = "30.06.";
                        break;
                    case 9:
                        q = "30.09.";
                        break;
                }


                periodNameKorr = String.Format("С 01.01.{0} ПО {1}{0}", spw2.YearKorr, q);
            }



            foreach (var staff in item.StaffList) // перебираем всех сотрудников попавших в эту пачку
            {
                num++; // номер в пачке

                spw2 = db.FormsSPW2.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                Staff ish_staff = db.Staff.FirstOrDefault(x => x.ID == staff.StaffID);
                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                XElement СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ = new XElement("СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ",
                                                                            new XElement("НомерВпачке", num),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            new XElement("НаименованиеКраткое", NameShort),
                                                                            new XElement("НалоговыйНомер",
                                                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));


                if (!String.IsNullOrEmpty(ins.KPP))
                {
                    СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                }

                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("КодКатегории", codeCat));




                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ОтчетныйПериод",
                                                               new XElement("Квартал", spw2.Quarter),
                                                               new XElement("Год", spw2.Year),
                                                               new XElement("Название", periodName.ToUpper())));

                if (spw2.YearKorr.HasValue)
                {
                    СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", spw2.QuarterKorr.Value),
                                                                   new XElement("Год", spw2.YearKorr.Value),
                                                                   new XElement("Название", periodNameKorr.ToUpper())));
                }

                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                            new XElement("ФИО",
                                                                                new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));

                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ДатаЗаполнения", spw2.DateFilling.ToShortDateString()));
                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ДатаСоставленияНа", spw2.DateComposit.ToShortDateString()));



                int i = 0;
                var staj_osn_list = db.StajOsn.Where(x => x.FormsSPW2_ID.Value == spw2.ID).OrderBy(x => x.Number.Value).ToList();

                var tt = staj_osn_list.Select(x => x.ID).ToList();
                var stajLgot_list = db.StajLgot.Where(x => tt.Contains(x.StajOsnID)).ToList(); // db_temp
                i = 0;
                foreach (var staj_osn in staj_osn_list)
                {
                    i++;
                    XElement СтажевыйПериод = createStajElement(staj_osn, stajLgot_list, i);
                    СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(СтажевыйПериод);
                }


                if (InfoType != "ОТМЕНЯЮЩАЯ")
                {
                    СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ПризнакНачисленияВзносовОПС", (spw2.ExistsInsurOPS.HasValue && spw2.ExistsInsurOPS.Value == 1) ? "ДА" : "НЕТ"));
                    СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ПризнакНачисленияВзносовПоДопТарифу", (spw2.ExistsInsurDop.HasValue && spw2.ExistsInsurDop.Value == 1) ? "ДА" : "НЕТ"));
                }

                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ);
            }

            #endregion

            xDoc.Root.SetDefaultXmlNamespace(pfr);

            //            xml = xDoc.ToString();
            return xDoc;
        }

        private XDocument generateXML_DSW3(xmlInfo item)
        {
            pu6Entities db_temp = new pu6Entities();



            //     string xml = "";
            XNamespace pfr = "http://schema.pfr.ru";
            int num = 1;

            var dsw3Data_ = db_temp.FormsDSW_3.FirstOrDefault(x => x.ID == item.SourceID);

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"), new XAttribute("ДобровольныеПравоотношения", "ДСВ"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА = new XElement("ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА",
                                                                  new XElement("НомерВпачке", num),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRIP))
            {
                СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRUL))
            {
                СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            }

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            string NameShort = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    NameShort = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    NameShort = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));

            //<xsd:element name="Подразделение" type="ТипПодразделение" minOccurs="0"/>
            //<xsd:element name="НомерЛицензии" type="xsd:string" minOccurs="0"/>
            //<xsd:element name="ДатаВыдачиЛицензии" type="ТипДата" minOccurs="0"/>



            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(СоставительПачки);

            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            //<xsd:element name="ПоПодразделению" type="ТипНомерПачки"/>


            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToString("dd.MM.yyyy")));




            XElement РеестрДСВ = new XElement("РеестрДСВ",
                                                           new XElement("ПлатежноеПоручение",
                                                               new XElement("ДатаПоручения", dsw3Data_.DATEPAYMENT.ToString("dd.MM.yyyy")),
                                                               new XElement("НомерПоручения", dsw3Data_.NUMBERPAYMENT),
                                                               new XElement("ДатаИсполненияПоручения", dsw3Data_.DATEEXECUTPAYMENT.ToString("dd.MM.yyyy"))),
                                                           new XElement("Год", dsw3Data_.YEAR),
                                                           new XElement("КоличествоСтрок", item.CountStaff.Value.ToString()),
                                                           new XElement("СуммаДСВРаботника", Utils.decToStr(dsw3Data_.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_EMPLOYERS.Value))),
                                                           new XElement("СуммаДСВРаботодателя", Utils.decToStr(dsw3Data_.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_PAYER.Value))),
                                                           new XElement("СуммаДСВОбщая", Utils.decToStr(dsw3Data_.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_PAYER.Value) + dsw3Data_.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_EMPLOYERS.Value))));
            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(РеестрДСВ);

            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА);

            List<FormsDSW_3_Staff> dsw3staffListTemp = db_temp.FormsDSW_3_Staff.Where(x => x.FormsDSW_3_ID == dsw3Data_.ID).ToList();
            List<long> dsw3staffIDListTemp = dsw3staffListTemp.Select(x => x.StaffID).ToList();
            List<Staff> staffListTemp = db_temp.Staff.Where(x => dsw3staffIDListTemp.Contains(x.ID)).ToList();

            foreach (var staff in item.StaffList)
            {
                num++; // номер в пачке

                FormsDSW_3_Staff dsw3staff = dsw3staffListTemp.First(x => x.ID == staff.FormsRSW_6_1_ID.Value);
                Staff ish_staff = staffListTemp.First(x => x.ID == dsw3staff.StaffID);

                XElement Работодатель = new XElement(СоставительПачки);
                Работодатель.Name = "Работодатель";

                XElement РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ = new XElement("РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ",
                                                        new XElement("НомерВпачке", num),
                                                        new XElement("СтраховойНомер", Utils.ParseSNILS(ish_staff.InsuranceNumber, ish_staff.ControlNumber)),
                                                        new XElement("ФИО",
                                                            new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                            new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                            new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())),
                                                        Работодатель,
                                                        new XElement("СуммаДСВРаботника", Utils.decToStr(dsw3staff.SUMFEEPFR_EMPLOYERS.Value)),
                                                        new XElement("СуммаДСВРаботодателя", Utils.decToStr(dsw3staff.SUMFEEPFR_PAYER.Value))
                                                        );


                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ);
            }

            dsw3staffListTemp.Clear();
            dsw3staffIDListTemp.Clear();
            staffListTemp.Clear();


            xDoc.Root.SetDefaultXmlNamespace(pfr);

            //     xml = xDoc.ToString();

            db_temp.Dispose();
            return xDoc;
        }

        private XDocument generateXML_ADW1(xmlInfo item)
        {
            pu6Entities db_temp = new pu6Entities();


            //     string xml = "";
            XNamespace pfr = "http://schema.pfr.ru";
            int num = 1;

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ = new XElement("ВХОДЯЩАЯ_ОПИСЬ",
                                                                  new XElement("НомерВпачке", num),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRIP))
            {
                СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRUL))
            {
                СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            }

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            string NameShort = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    NameShort = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    NameShort = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));

            //<xsd:element name="Подразделение" type="ТипПодразделение" minOccurs="0"/>
            //<xsd:element name="НомерЛицензии" type="xsd:string" minOccurs="0"/>
            //<xsd:element name="ДатаВыдачиЛицензии" type="ТипДата" minOccurs="0"/>



            ВХОДЯЩАЯ_ОПИСЬ.Add(СоставительПачки);

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            //<xsd:element name="ПоПодразделению" type="ТипНомерПачки"/>


            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "АНКЕТА_ЗЛ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToString("dd.MM.yyyy")));



            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ);


            List<long> StaffID = item.StaffList.Where(x => x.StaffID.HasValue).Select(x => x.StaffID.Value).ToList();
            List<Staff> staffListTemp = db_temp.Staff.Where(x => StaffID.Contains(x.ID)).ToList();

            foreach (var staff in item.StaffList)
            {
                num++; // номер в пачке

                Staff ish_staff = staffListTemp.First(x => x.ID == staff.StaffID);
                FormsADW_1 adw1 = db_temp.FormsADW_1.First(x => x.StaffID == staff.StaffID);

                XElement АНКЕТА_ЗЛ = new XElement("АНКЕТА_ЗЛ",
                                                        new XElement("НомерВпачке", num));

                XElement АнкетныеДанные = new XElement("АнкетныеДанные",
                                                                            new XElement("ФИО",
                                                            new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                            new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                            new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())),
                                                            new XElement("Пол", ish_staff.Sex.HasValue ? (ish_staff.Sex.Value == 0 ? "МУЖСКОЙ" : "ЖЕНСКИЙ") : ""));
                XElement Датарождения = new XElement("ДатаРождения");
                if (adw1.Type_DateBirth.HasValue && adw1.Type_DateBirth.Value == 1)  //Если особая дата рождения
                {
                    Датарождения = new XElement("ДатаРожденияОсобая");

                    if (adw1.DateBirthDay_Os.HasValue)
                    {
                        Датарождения.Add(new XElement("День", adw1.DateBirthDay_Os.Value.ToString()));
                    }
                    if (adw1.DateBirthMonth_Os.HasValue)
                    {
                        Датарождения.Add(new XElement("Месяц", adw1.DateBirthMonth_Os.Value.ToString()));
                    }

                    Датарождения.Add(new XElement("Год", adw1.DateBirthYear_Os.HasValue ? adw1.DateBirthYear_Os.Value.ToString() : ""));
                }
                else
                {
                    try
                    {
                        if (ish_staff.DateBirth.HasValue)
                            Датарождения = new XElement("ДатаРождения", ish_staff.DateBirth.Value.ToString("dd.MM.yyyy"));
                    }
                    catch { }
                }

                АнкетныеДанные.Add(Датарождения);

                XElement МестоРождения = new XElement("МестоРождения",
                        new XElement("ТипМестаРождения", adw1.Type_PlaceBirth.HasValue ? ((short)adw1.Type_PlaceBirth.Value == 0 ? "СТАНДАРТНОЕ" : "ОСОБОЕ") : "СТАНДАРТНОЕ"));

                if (!String.IsNullOrEmpty(adw1.Punkt))
                {
                    МестоРождения.Add(new XElement("ГородРождения", adw1.Punkt.Trim().Length > 200 ? adw1.Punkt.Trim().Substring(0, 200) : adw1.Punkt.Trim()));
                }
                if (!String.IsNullOrEmpty(adw1.Distr))
                {
                    МестоРождения.Add(new XElement("РайонРождения", adw1.Distr.Trim().Length > 200 ? adw1.Distr.Trim().Substring(0, 200) : adw1.Distr.Trim()));
                }
                if (!String.IsNullOrEmpty(adw1.Region))
                {
                    МестоРождения.Add(new XElement("РегионРождения", adw1.Region.Trim().Length > 200 ? adw1.Region.Trim().Substring(0, 200) : adw1.Region.Trim()));
                }
                if (!String.IsNullOrEmpty(adw1.Country))
                {
                    МестоРождения.Add(new XElement("СтранаРождения", adw1.Country.Trim().Length > 200 ? adw1.Country.Trim().Substring(0, 200) : adw1.Country.Trim()));
                }


                АнкетныеДанные.Add(МестоРождения);


                if (!String.IsNullOrEmpty(adw1.Citizenship))
                {
                    АнкетныеДанные.Add(new XElement("Гражданство", adw1.Citizenship.Trim().Length > 40 ? adw1.Citizenship.Trim().Substring(0, 40) : adw1.Citizenship.Trim()));
                }

                if (!String.IsNullOrEmpty(adw1.Reg_Addr))
                {
                    XElement АдресРегистрации = new XElement("АдресРегистрации",
                        new XElement("ТипАдреса", "НЕСТРУКТУРИРОВАННЫЙ"),
                        new XElement("НеструктурированныйАдрес",
                            new XElement("Адрес", adw1.Reg_Addr.Trim().Length > 200 ? adw1.Reg_Addr.Trim().Substring(0, 200) : adw1.Reg_Addr.Trim())));

                    АнкетныеДанные.Add(АдресРегистрации);
                }

                if (!String.IsNullOrEmpty(adw1.Fakt_Addr))
                {
                    XElement АдресФактический = new XElement("АдресФактический",
                        new XElement("ТипАдреса", "НЕСТРУКТУРИРОВАННЫЙ"),
                        new XElement("НеструктурированныйАдрес",
                            new XElement("Адрес", adw1.Fakt_Addr.Trim().Length > 200 ? adw1.Fakt_Addr.Trim().Substring(0, 200) : adw1.Fakt_Addr.Trim())));

                    АнкетныеДанные.Add(АдресФактический);
                }

                if (!String.IsNullOrEmpty(adw1.Phone))
                {
                    АнкетныеДанные.Add(new XElement("Телефон", adw1.Phone.Trim().Length > 40 ? adw1.Phone.Trim().Substring(0, 40) : adw1.Phone.Trim()));
                }

                АНКЕТА_ЗЛ.Add(АнкетныеДанные);

                if (adw1.Doc_Type_ID.HasValue) // если есть документ
                {
                    XElement УдостоверяющийДокумент = new XElement("УдостоверяющийДокумент");
                    string doctype = db.DocumentTypes.FirstOrDefault(x => x.ID == adw1.Doc_Type_ID).Code;
                    УдостоверяющийДокумент.Add(new XElement("ТипУдостоверяющего", doctype));

                    string docName = doctype == "ПРОЧЕЕ" ? adw1.Doc_Name : doctype;

                    XElement Документ = new XElement("Документ",
                        new XElement("НаименованиеУдостоверяющего", docName.Substring(0, docName.Length > 80 ? 80 : docName.Length).ToUpper()));

                    if (!String.IsNullOrEmpty(adw1.Ser_Lat))
                        Документ.Add(new XElement("СерияРимскиеЦифры", adw1.Ser_Lat.Substring(0, adw1.Ser_Lat.Length > 8 ? 8 : adw1.Ser_Lat.Length).ToUpper()));
                    if (!String.IsNullOrEmpty(adw1.Ser_Rus))
                        Документ.Add(new XElement("СерияРусскиеБуквы", adw1.Ser_Rus.Substring(0, adw1.Ser_Rus.Length > 8 ? 8 : adw1.Ser_Rus.Length).ToUpper()));
                    if (!String.IsNullOrEmpty(adw1.Doc_Num))
                        Документ.Add(new XElement("НомерУдостоверяющего", adw1.Doc_Num.Substring(0, adw1.Doc_Num.Length > 8 ? 8 : adw1.Doc_Num.Length).ToUpper()));
                    Документ.Add(new XElement("ДатаВыдачи", adw1.Doc_Date.HasValue ? adw1.Doc_Date.Value.ToString("dd.MM.yyyy") : ""));
                    Документ.Add(new XElement("КемВыдан", adw1.Doc_Kem_Vyd.Substring(0, adw1.Doc_Kem_Vyd.Length > 80 ? 80 : adw1.Doc_Kem_Vyd.Length).ToUpper()));

                    УдостоверяющийДокумент.Add(Документ);

                    АНКЕТА_ЗЛ.Add(УдостоверяющийДокумент);
                }


                try
                {
                    if (adw1.DateFilling.HasValue)
                    {
                        АНКЕТА_ЗЛ.Add(new XElement("ДатаЗаполнения", adw1.DateFilling.Value.ToString("dd.MM.yyyy")));
                    }
                }
                catch { }





                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(АНКЕТА_ЗЛ);
            }

            StaffID.Clear();
            staffListTemp.Clear();


            xDoc.Root.SetDefaultXmlNamespace(pfr);

            //     xml = xDoc.ToString();

            db_temp.Dispose();
            return xDoc;
        }

        private XDocument generateXML_ADW2(xmlInfo item)
        {
            pu6Entities db_temp = new pu6Entities();


            //     string xml = "";
            XNamespace pfr = "http://schema.pfr.ru";
            int num = 1;

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ = new XElement("ВХОДЯЩАЯ_ОПИСЬ",
                                                                  new XElement("НомерВпачке", num),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRIP))
            {
                СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRUL))
            {
                СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            }

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            string NameShort = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    NameShort = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    NameShort = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));

            //<xsd:element name="Подразделение" type="ТипПодразделение" minOccurs="0"/>
            //<xsd:element name="НомерЛицензии" type="xsd:string" minOccurs="0"/>
            //<xsd:element name="ДатаВыдачиЛицензии" type="ТипДата" minOccurs="0"/>



            ВХОДЯЩАЯ_ОПИСЬ.Add(СоставительПачки);

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            //<xsd:element name="ПоПодразделению" type="ТипНомерПачки"/>


            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToString("dd.MM.yyyy")));



            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ);


            List<long> StaffID = item.StaffList.Where(x => x.StaffID.HasValue).Select(x => x.StaffID.Value).ToList();
            List<Staff> staffListTemp = db_temp.Staff.Where(x => StaffID.Contains(x.ID)).ToList();

            foreach (var staff in item.StaffList)
            {
                num++; // номер в пачке

                Staff ish_staff = staffListTemp.First(x => x.ID == staff.StaffID);
                FormsADW_2 adw2 = db_temp.FormsADW_2.First(x => x.ID == staff.FormsRSW_6_1_ID);

                XElement ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ = new XElement("ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ",
                                                        new XElement("НомерВпачке", num),
                                                        new XElement("СтраховойНомер", Utils.ParseSNILS(ish_staff.InsuranceNumber, ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value : (short)0)),
                                                        new XElement("ФИОизСтрахового",
                                                            new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                            new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                            new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));


                XElement УдостоверяющийДокументИСХД = new XElement("УдостоверяющийДокумент");
                bool ishDocExist = false;

                if (db_temp.FormsADW_1.Any(x => x.StaffID == ish_staff.ID))
                {
                    FormsADW_1 adw1 = db_temp.FormsADW_1.First(x => x.StaffID == ish_staff.ID);

                    if (adw1.Doc_Type_ID.HasValue) // если есть документ
                    {
                        string doctype = db_temp.DocumentTypes.FirstOrDefault(x => x.ID == adw1.Doc_Type_ID).Code;
                        УдостоверяющийДокументИСХД.Add(new XElement("ТипУдостоверяющего", doctype));

                        string docName = doctype == "ПРОЧЕЕ" ? adw1.Doc_Name : doctype;

                        XElement Документ = new XElement("Документ",
                            new XElement("НаименованиеУдостоверяющего", docName.Substring(0, docName.Length > 80 ? 80 : docName.Length).ToUpper()));

                        if (!String.IsNullOrEmpty(adw1.Ser_Lat))
                            Документ.Add(new XElement("СерияРимскиеЦифры", adw1.Ser_Lat.Substring(0, adw1.Ser_Lat.Length > 8 ? 8 : adw1.Ser_Lat.Length).ToUpper()));
                        if (!String.IsNullOrEmpty(adw1.Ser_Rus))
                            Документ.Add(new XElement("СерияРусскиеБуквы", adw1.Ser_Rus.Substring(0, adw1.Ser_Rus.Length > 8 ? 8 : adw1.Ser_Rus.Length).ToUpper()));
                        if (!String.IsNullOrEmpty(adw1.Doc_Num))
                            Документ.Add(new XElement("НомерУдостоверяющего", adw1.Doc_Num.Substring(0, adw1.Doc_Num.Length > 8 ? 8 : adw1.Doc_Num.Length).ToUpper()));
                        Документ.Add(new XElement("ДатаВыдачи", adw1.Doc_Date.HasValue ? adw1.Doc_Date.Value.ToString("dd.MM.yyyy") : ""));
                        Документ.Add(new XElement("КемВыдан", adw1.Doc_Kem_Vyd.Substring(0, adw1.Doc_Kem_Vyd.Length > 80 ? 80 : adw1.Doc_Kem_Vyd.Length).ToUpper()));

                        УдостоверяющийДокументИСХД.Add(Документ);
                        ishDocExist = true;
                    }
                }

                XElement ИзменившиесяДанные = new XElement("ИзменившиесяДанные");

                if (!String.IsNullOrEmpty(adw2.LastName) || !String.IsNullOrEmpty(adw2.FirstName) || !String.IsNullOrEmpty(adw2.MiddleName))
                {
                    ИзменившиесяДанные.Add(new XElement("ФИО",
                                                            new XElement("Фамилия", adw2.LastName.Substring(0, adw2.LastName.Length > 40 ? 40 : adw2.LastName.Length).ToUpper()),
                                                            new XElement("Имя", adw2.FirstName.Substring(0, adw2.FirstName.Length > 40 ? 40 : adw2.FirstName.Length).ToUpper()),
                                                            new XElement("Отчество", adw2.MiddleName.Substring(0, adw2.MiddleName.Length > 40 ? 40 : adw2.MiddleName.Length).ToUpper())));
                }

                if (adw2.Sex.HasValue)
                {
                    ИзменившиесяДанные.Add(new XElement("Пол", adw2.Sex.HasValue ? (adw2.Sex.Value == 0 ? "МУЖСКОЙ" : "ЖЕНСКИЙ") : ""));
                }

                if ((adw2.Type_DateBirth.HasValue && adw2.Type_DateBirth.Value == 1) || adw2.DateBirth.HasValue)  //Если особая дата рождения
                {
                    XElement Датарождения = new XElement("ДатаРождения");
                    if (adw2.Type_DateBirth.HasValue && adw2.Type_DateBirth.Value == 1)  //Если особая дата рождения
                    {
                        Датарождения = new XElement("ДатаРожденияОсобая");

                        if (adw2.DateBirthDay_Os.HasValue)
                        {
                            Датарождения.Add(new XElement("День", adw2.DateBirthDay_Os.Value.ToString()));
                        }
                        if (adw2.DateBirthMonth_Os.HasValue)
                        {
                            Датарождения.Add(new XElement("Месяц", adw2.DateBirthMonth_Os.Value.ToString()));
                        }

                        Датарождения.Add(new XElement("Год", adw2.DateBirthYear_Os.HasValue ? adw2.DateBirthYear_Os.Value.ToString() : ""));
                    }
                    else
                    {
                        try
                        {
                            if (adw2.DateBirth.HasValue)
                                Датарождения = new XElement("ДатаРождения", adw2.DateBirth.Value.ToString("dd.MM.yyyy"));
                        }
                        catch { }
                    }

                    ИзменившиесяДанные.Add(Датарождения);
                }


                if ((adw2.Type_PlaceBirth.HasValue && adw2.Type_PlaceBirth.Value != 0) || !String.IsNullOrEmpty(adw2.Punkt) || !String.IsNullOrEmpty(adw2.Distr) || !String.IsNullOrEmpty(adw2.Region) || !String.IsNullOrEmpty(adw2.Country))
                {
                    XElement МестоРождения = new XElement("МестоРождения");

                    if (adw2.Type_PlaceBirth.HasValue)
                    {
                        МестоРождения.Add(new XElement("ТипМестаРождения", adw2.Type_PlaceBirth.HasValue ? ((short)adw2.Type_PlaceBirth.Value == 1 ? "СТАНДАРТНОЕ" : "ОСОБОЕ") : "СТАНДАРТНОЕ"));
                    }

                    if (!String.IsNullOrEmpty(adw2.Punkt))
                    {
                        МестоРождения.Add(new XElement("ГородРождения", adw2.Punkt.Trim().Length > 200 ? adw2.Punkt.Trim().Substring(0, 200) : adw2.Punkt.Trim()));
                    }
                    if (!String.IsNullOrEmpty(adw2.Distr))
                    {
                        МестоРождения.Add(new XElement("РайонРождения", adw2.Distr.Trim().Length > 200 ? adw2.Distr.Trim().Substring(0, 200) : adw2.Distr.Trim()));
                    }
                    if (!String.IsNullOrEmpty(adw2.Region))
                    {
                        МестоРождения.Add(new XElement("РегионРождения", adw2.Region.Trim().Length > 200 ? adw2.Region.Trim().Substring(0, 200) : adw2.Region.Trim()));
                    }
                    if (!String.IsNullOrEmpty(adw2.Country))
                    {
                        МестоРождения.Add(new XElement("СтранаРождения", adw2.Country.Trim().Length > 200 ? adw2.Country.Trim().Substring(0, 200) : adw2.Country.Trim()));
                    }

                    ИзменившиесяДанные.Add(МестоРождения);
                }

                if (!String.IsNullOrEmpty(adw2.Citizenship))
                {
                    ИзменившиесяДанные.Add(new XElement("Гражданство", adw2.Citizenship.Trim().Length > 40 ? adw2.Citizenship.Trim().Substring(0, 40) : adw2.Citizenship.Trim()));
                }

                if (!String.IsNullOrEmpty(adw2.Reg_Addr))
                {
                    XElement АдресРегистрации = new XElement("АдресРегистрации",
                        new XElement("ТипАдреса", "НЕСТРУКТУРИРОВАННЫЙ"),
                        new XElement("НеструктурированныйАдрес",
                            new XElement("Адрес", adw2.Reg_Addr.Trim().Length > 200 ? adw2.Reg_Addr.Trim().Substring(0, 200) : adw2.Reg_Addr.Trim())));

                    ИзменившиесяДанные.Add(АдресРегистрации);
                }

                if (!String.IsNullOrEmpty(adw2.Fakt_Addr))
                {
                    XElement АдресФактический = new XElement("АдресФактический",
                        new XElement("ТипАдреса", "НЕСТРУКТУРИРОВАННЫЙ"),
                        new XElement("НеструктурированныйАдрес",
                            new XElement("Адрес", adw2.Fakt_Addr.Trim().Length > 200 ? adw2.Fakt_Addr.Trim().Substring(0, 200) : adw2.Fakt_Addr.Trim())));

                    ИзменившиесяДанные.Add(АдресФактический);
                }

                if (!String.IsNullOrEmpty(adw2.Phone))
                {
                    ИзменившиесяДанные.Add(new XElement("Телефон", adw2.Phone.Trim().Length > 40 ? adw2.Phone.Trim().Substring(0, 40) : adw2.Phone.Trim()));
                }

                ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ.Add(ИзменившиесяДанные);

                if (!String.IsNullOrEmpty(adw2.MiddleNameCancel) && adw2.MiddleNameCancel == "ОТМН")
                {
                    ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ.Add(new XElement("ПризнакОтменыОтчества", "ОТМЕНЕНО"));
                }

                if (!String.IsNullOrEmpty(adw2.PlaceBirthCancel) && adw2.PlaceBirthCancel == "ОТМН")
                {
                    ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ.Add(new XElement("ПризнакОтменыМестаРождения", "ОТМЕНЕНО"));
                }


                if (adw2.Doc_Type_ID.HasValue) // если есть документ
                {
                    XElement УдостоверяющийДокумент = new XElement("УдостоверяющийДокумент");
                    string doctype = db.DocumentTypes.FirstOrDefault(x => x.ID == adw2.Doc_Type_ID).Code;
                    УдостоверяющийДокумент.Add(new XElement("ТипУдостоверяющего", doctype));

                    string docName = doctype == "ПРОЧЕЕ" ? adw2.Doc_Name : doctype;

                    XElement Документ = new XElement("Документ",
                        new XElement("НаименованиеУдостоверяющего", docName.Substring(0, docName.Length > 80 ? 80 : docName.Length).ToUpper()));

                    if (!String.IsNullOrEmpty(adw2.Ser_Lat))
                        Документ.Add(new XElement("СерияРимскиеЦифры", adw2.Ser_Lat.Substring(0, adw2.Ser_Lat.Length > 8 ? 8 : adw2.Ser_Lat.Length).ToUpper()));
                    if (!String.IsNullOrEmpty(adw2.Ser_Rus))
                        Документ.Add(new XElement("СерияРусскиеБуквы", adw2.Ser_Rus.Substring(0, adw2.Ser_Rus.Length > 8 ? 8 : adw2.Ser_Rus.Length).ToUpper()));
                    if (!String.IsNullOrEmpty(adw2.Doc_Num))
                        Документ.Add(new XElement("НомерУдостоверяющего", adw2.Doc_Num.Substring(0, adw2.Doc_Num.Length > 8 ? 8 : adw2.Doc_Num.Length).ToUpper()));
                    Документ.Add(new XElement("ДатаВыдачи", adw2.Doc_Date.HasValue ? adw2.Doc_Date.Value.ToString("dd.MM.yyyy") : ""));
                    Документ.Add(new XElement("КемВыдан", adw2.Doc_Kem_Vyd.Substring(0, adw2.Doc_Kem_Vyd.Length > 80 ? 80 : adw2.Doc_Kem_Vyd.Length).ToUpper()));

                    УдостоверяющийДокумент.Add(Документ);

                    ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ.Add(УдостоверяющийДокумент);
                }
                else if (ishDocExist)
                {
                    ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ.Add(УдостоверяющийДокументИСХД);
                }


                try
                {
                    if (adw2.DateFilling.HasValue)
                    {
                        ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ.Add(new XElement("ДатаЗаполнения", adw2.DateFilling.Value.ToString("dd.MM.yyyy")));
                    }
                }
                catch { }





                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ЗАЯВЛЕНИЕ_ОБ_ОБМЕНЕ);
            }

            StaffID.Clear();
            staffListTemp.Clear();


            xDoc.Root.SetDefaultXmlNamespace(pfr);

            //     xml = xDoc.ToString();

            db_temp.Dispose();
            return xDoc;
        }

        private XDocument generateXML_ADW3(xmlInfo item)
        {
            pu6Entities db_temp = new pu6Entities();


            //     string xml = "";
            XNamespace pfr = "http://schema.pfr.ru";
            int num = 1;

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ = new XElement("ВХОДЯЩАЯ_ОПИСЬ",
                                                                  new XElement("НомерВпачке", num),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRIP))
            {
                СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRUL))
            {
                СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            }

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            string NameShort = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    NameShort = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    NameShort = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));

            //<xsd:element name="Подразделение" type="ТипПодразделение" minOccurs="0"/>
            //<xsd:element name="НомерЛицензии" type="xsd:string" minOccurs="0"/>
            //<xsd:element name="ДатаВыдачиЛицензии" type="ТипДата" minOccurs="0"/>



            ВХОДЯЩАЯ_ОПИСЬ.Add(СоставительПачки);

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            //<xsd:element name="ПоПодразделению" type="ТипНомерПачки"/>


            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToString("dd.MM.yyyy")));



            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ);


            List<long> StaffID = item.StaffList.Where(x => x.StaffID.HasValue).Select(x => x.StaffID.Value).ToList();
            List<Staff> staffListTemp = db_temp.Staff.Where(x => StaffID.Contains(x.ID)).ToList();

            foreach (var staff in item.StaffList)
            {
                num++; // номер в пачке

                Staff ish_staff = staffListTemp.First(x => x.ID == staff.StaffID);
                FormsADW_3 adw3 = db_temp.FormsADW_3.First(x => x.ID == staff.FormsRSW_6_1_ID);

                XElement ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ = new XElement("ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ",
                                                        new XElement("НомерВпачке", num),
                                                        new XElement("СтраховойНомер", Utils.ParseSNILS(ish_staff.InsuranceNumber, ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value : (short)0)));

                if (!String.IsNullOrEmpty(adw3.OtmetkaOPredSved.Trim()))
                {
                    ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(new XElement("ОтметкаОпредставленииСведений", adw3.OtmetkaOPredSved.ToUpper().Trim()));
                }

                ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(new XElement("ФИОизСтрахового",
                        new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                        new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                        new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));


                if (ish_staff.Sex.HasValue)
                {
                    ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(new XElement("ПолИзСтрахового", ish_staff.Sex.HasValue ? (ish_staff.Sex.Value == 0 ? "МУЖСКОЙ" : "ЖЕНСКИЙ") : ""));
                }

                XElement УдостоверяющийДокументИСХД = new XElement("УдостоверяющийДокумент");
                bool ishDocExist = false;

                if (db_temp.FormsADW_1.Any(x => x.StaffID == ish_staff.ID))
                {
                    FormsADW_1 adw1 = db_temp.FormsADW_1.First(x => x.StaffID == ish_staff.ID);

                    if ((adw1.Type_DateBirth.HasValue && adw1.Type_DateBirth.Value == 1) || ish_staff.DateBirth.HasValue)  //Если особая дата рождения
                    {
                        XElement ДатаРожденияИзСтрахового = new XElement("ДатаРожденияИзСтрахового");
                        if (adw1.Type_DateBirth.HasValue && adw1.Type_DateBirth.Value == 1)  //Если особая дата рождения
                        {
                            ДатаРожденияИзСтрахового = new XElement("ДатаРожденияИзСтраховогоОсобая");

                            if (adw1.DateBirthDay_Os.HasValue)
                            {
                                ДатаРожденияИзСтрахового.Add(new XElement("День", adw1.DateBirthDay_Os.Value.ToString()));
                            }
                            if (adw1.DateBirthMonth_Os.HasValue)
                            {
                                ДатаРожденияИзСтрахового.Add(new XElement("Месяц", adw1.DateBirthMonth_Os.Value.ToString()));
                            }

                            ДатаРожденияИзСтрахового.Add(new XElement("Год", adw1.DateBirthYear_Os.HasValue ? adw1.DateBirthYear_Os.Value.ToString() : ""));
                        }
                        else
                        {
                            try
                            {
                                if (ish_staff.DateBirth.HasValue)
                                    ДатаРожденияИзСтрахового = new XElement("ДатаРожденияИзСтрахового", ish_staff.DateBirth.Value.ToString("dd.MM.yyyy"));
                            }
                            catch { }
                        }

                        ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(ДатаРожденияИзСтрахового);
                    }


                    if ((adw1.Type_PlaceBirth.HasValue && adw1.Type_PlaceBirth.Value != 0) || !String.IsNullOrEmpty(adw1.Punkt) || !String.IsNullOrEmpty(adw1.Distr) || !String.IsNullOrEmpty(adw1.Region) || !String.IsNullOrEmpty(adw1.Country))
                    {
                        XElement МестоРожденияИзСтрахового = new XElement("МестоРожденияИзСтрахового");

                        if (adw1.Type_PlaceBirth.HasValue)
                        {
                            МестоРожденияИзСтрахового.Add(new XElement("ТипМестаРождения", adw1.Type_PlaceBirth.HasValue ? ((short)adw1.Type_PlaceBirth.Value == 1 ? "СТАНДАРТНОЕ" : "ОСОБОЕ") : "СТАНДАРТНОЕ"));
                        }

                        if (!String.IsNullOrEmpty(adw1.Punkt))
                        {
                            МестоРожденияИзСтрахового.Add(new XElement("ГородРождения", adw1.Punkt.Trim().Length > 200 ? adw1.Punkt.Trim().Substring(0, 200) : adw1.Punkt.Trim()));
                        }
                        if (!String.IsNullOrEmpty(adw1.Distr))
                        {
                            МестоРожденияИзСтрахового.Add(new XElement("РайонРождения", adw1.Distr.Trim().Length > 200 ? adw1.Distr.Trim().Substring(0, 200) : adw1.Distr.Trim()));
                        }
                        if (!String.IsNullOrEmpty(adw1.Region))
                        {
                            МестоРожденияИзСтрахового.Add(new XElement("РегионРождения", adw1.Region.Trim().Length > 200 ? adw1.Region.Trim().Substring(0, 200) : adw1.Region.Trim()));
                        }
                        if (!String.IsNullOrEmpty(adw1.Country))
                        {
                            МестоРожденияИзСтрахового.Add(new XElement("СтранаРождения", adw1.Country.Trim().Length > 200 ? adw1.Country.Trim().Substring(0, 200) : adw1.Country.Trim()));
                        }

                        ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(МестоРожденияИзСтрахового);
                    }

                    if (adw1.Doc_Type_ID.HasValue) // если есть документ
                    {
                        string doctype = db_temp.DocumentTypes.FirstOrDefault(x => x.ID == adw1.Doc_Type_ID).Code;
                        УдостоверяющийДокументИСХД.Add(new XElement("ТипУдостоверяющего", doctype));

                        string docName = doctype == "ПРОЧЕЕ" ? adw1.Doc_Name : doctype;

                        XElement Документ = new XElement("Документ",
                            new XElement("НаименованиеУдостоверяющего", docName.Substring(0, docName.Length > 80 ? 80 : docName.Length).ToUpper()));

                        if (!String.IsNullOrEmpty(adw1.Ser_Lat))
                            Документ.Add(new XElement("СерияРимскиеЦифры", adw1.Ser_Lat.Substring(0, adw1.Ser_Lat.Length > 8 ? 8 : adw1.Ser_Lat.Length).ToUpper()));
                        if (!String.IsNullOrEmpty(adw1.Ser_Rus))
                            Документ.Add(new XElement("СерияРусскиеБуквы", adw1.Ser_Rus.Substring(0, adw1.Ser_Rus.Length > 8 ? 8 : adw1.Ser_Rus.Length).ToUpper()));
                        if (!String.IsNullOrEmpty(adw1.Doc_Num))
                            Документ.Add(new XElement("НомерУдостоверяющего", adw1.Doc_Num.Substring(0, adw1.Doc_Num.Length > 8 ? 8 : adw1.Doc_Num.Length).ToUpper()));
                        Документ.Add(new XElement("ДатаВыдачи", adw1.Doc_Date.HasValue ? adw1.Doc_Date.Value.ToString("dd.MM.yyyy") : ""));
                        Документ.Add(new XElement("КемВыдан", adw1.Doc_Kem_Vyd.Substring(0, adw1.Doc_Kem_Vyd.Length > 80 ? 80 : adw1.Doc_Kem_Vyd.Length).ToUpper()));

                        УдостоверяющийДокументИСХД.Add(Документ);
                        ishDocExist = true;
                    }
                }







                XElement ИзменившиесяДанные = new XElement("ИзменившиесяДанные");

                if (!String.IsNullOrEmpty(adw3.LastName) || !String.IsNullOrEmpty(adw3.FirstName) || !String.IsNullOrEmpty(adw3.MiddleName))
                {
                    ИзменившиесяДанные.Add(new XElement("ФИО",
                                                            new XElement("Фамилия", adw3.LastName.Substring(0, adw3.LastName.Length > 40 ? 40 : adw3.LastName.Length).ToUpper()),
                                                            new XElement("Имя", adw3.FirstName.Substring(0, adw3.FirstName.Length > 40 ? 40 : adw3.FirstName.Length).ToUpper()),
                                                            new XElement("Отчество", adw3.MiddleName.Substring(0, adw3.MiddleName.Length > 40 ? 40 : adw3.MiddleName.Length).ToUpper())));
                }

                if (adw3.Sex.HasValue)
                {
                    ИзменившиесяДанные.Add(new XElement("Пол", adw3.Sex.HasValue ? (adw3.Sex.Value == 0 ? "МУЖСКОЙ" : "ЖЕНСКИЙ") : ""));
                }

                if ((adw3.Type_DateBirth.HasValue && adw3.Type_DateBirth.Value == 1) || adw3.DateBirth.HasValue)  //Если особая дата рождения
                {
                    XElement Датарождения = new XElement("ДатаРождения");
                    if (adw3.Type_DateBirth.HasValue && adw3.Type_DateBirth.Value == 1)  //Если особая дата рождения
                    {
                        Датарождения = new XElement("ДатаРожденияОсобая");

                        if (adw3.DateBirthDay_Os.HasValue)
                        {
                            Датарождения.Add(new XElement("День", adw3.DateBirthDay_Os.Value.ToString()));
                        }
                        if (adw3.DateBirthMonth_Os.HasValue)
                        {
                            Датарождения.Add(new XElement("Месяц", adw3.DateBirthMonth_Os.Value.ToString()));
                        }

                        Датарождения.Add(new XElement("День", adw3.DateBirthYear_Os.HasValue ? adw3.DateBirthYear_Os.Value.ToString() : ""));
                    }
                    else
                    {
                        try
                        {
                            if (adw3.DateBirth.HasValue)
                                Датарождения = new XElement("ДатаРождения", adw3.DateBirth.Value.ToString("dd.MM.yyyy"));
                        }
                        catch { }
                    }

                    ИзменившиесяДанные.Add(Датарождения);
                }


                if ((adw3.Type_PlaceBirth.HasValue && adw3.Type_PlaceBirth.Value != 0) || !String.IsNullOrEmpty(adw3.Punkt) || !String.IsNullOrEmpty(adw3.Distr) || !String.IsNullOrEmpty(adw3.Region) || !String.IsNullOrEmpty(adw3.Country))
                {
                    XElement МестоРождения = new XElement("МестоРождения");

                    if (adw3.Type_PlaceBirth.HasValue)
                    {
                        МестоРождения.Add(new XElement("ТипМестаРождения", adw3.Type_PlaceBirth.HasValue ? ((short)adw3.Type_PlaceBirth.Value == 1 ? "СТАНДАРТНОЕ" : "ОСОБОЕ") : "СТАНДАРТНОЕ"));
                    }

                    if (!String.IsNullOrEmpty(adw3.Punkt))
                    {
                        МестоРождения.Add(new XElement("ГородРождения", adw3.Punkt.Trim().Length > 200 ? adw3.Punkt.Trim().Substring(0, 200) : adw3.Punkt.Trim()));
                    }
                    if (!String.IsNullOrEmpty(adw3.Distr))
                    {
                        МестоРождения.Add(new XElement("РайонРождения", adw3.Distr.Trim().Length > 200 ? adw3.Distr.Trim().Substring(0, 200) : adw3.Distr.Trim()));
                    }
                    if (!String.IsNullOrEmpty(adw3.Region))
                    {
                        МестоРождения.Add(new XElement("РегионРождения", adw3.Region.Trim().Length > 200 ? adw3.Region.Trim().Substring(0, 200) : adw3.Region.Trim()));
                    }
                    if (!String.IsNullOrEmpty(adw3.Country))
                    {
                        МестоРождения.Add(new XElement("СтранаРождения", adw3.Country.Trim().Length > 200 ? adw3.Country.Trim().Substring(0, 200) : adw3.Country.Trim()));
                    }

                    ИзменившиесяДанные.Add(МестоРождения);
                }

                if (!String.IsNullOrEmpty(adw3.Citizenship))
                {
                    ИзменившиесяДанные.Add(new XElement("Гражданство", adw3.Citizenship.Trim().Length > 40 ? adw3.Citizenship.Trim().Substring(0, 40) : adw3.Citizenship.Trim()));
                }

                if (!String.IsNullOrEmpty(adw3.Reg_Addr))
                {
                    XElement АдресРегистрации = new XElement("АдресРегистрации",
                        new XElement("ТипАдреса", "НЕСТРУКТУРИРОВАННЫЙ"),
                        new XElement("НеструктурированныйАдрес",
                            new XElement("Адрес", adw3.Reg_Addr.Trim().Length > 200 ? adw3.Reg_Addr.Trim().Substring(0, 200) : adw3.Reg_Addr.Trim())));

                    ИзменившиесяДанные.Add(АдресРегистрации);
                }

                if (!String.IsNullOrEmpty(adw3.Fakt_Addr))
                {
                    XElement АдресФактический = new XElement("АдресФактический",
                        new XElement("ТипАдреса", "НЕСТРУКТУРИРОВАННЫЙ"),
                        new XElement("НеструктурированныйАдрес",
                            new XElement("Адрес", adw3.Fakt_Addr.Trim().Length > 200 ? adw3.Fakt_Addr.Trim().Substring(0, 200) : adw3.Fakt_Addr.Trim())));

                    ИзменившиесяДанные.Add(АдресФактический);
                }

                if (!String.IsNullOrEmpty(adw3.Phone))
                {
                    ИзменившиесяДанные.Add(new XElement("Телефон", adw3.Phone.Trim().Length > 40 ? adw3.Phone.Trim().Substring(0, 40) : adw3.Phone.Trim()));
                }

                ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(ИзменившиесяДанные);

                if (!String.IsNullOrEmpty(adw3.MiddleNameCancel) && adw3.MiddleNameCancel == "ОТМН")
                {
                    ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(new XElement("ПризнакОтменыОтчества", "ОТМЕНЕНО"));
                }

                if (!String.IsNullOrEmpty(adw3.PlaceBirthCancel) && adw3.PlaceBirthCancel == "ОТМН")
                {
                    ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(new XElement("ПризнакОтменыМестаРождения", "ОТМЕНЕНО"));
                }


                if (adw3.Doc_Type_ID.HasValue) // если есть документ
                {
                    XElement УдостоверяющийДокумент = new XElement("УдостоверяющийДокумент");
                    string doctype = db.DocumentTypes.FirstOrDefault(x => x.ID == adw3.Doc_Type_ID).Code;
                    УдостоверяющийДокумент.Add(new XElement("ТипУдостоверяющего", doctype));

                    string docName = doctype == "ПРОЧЕЕ" ? adw3.Doc_Name : doctype;

                    XElement Документ = new XElement("Документ",
                        new XElement("НаименованиеУдостоверяющего", docName.Substring(0, docName.Length > 80 ? 80 : docName.Length).ToUpper()));

                    if (!String.IsNullOrEmpty(adw3.Ser_Lat))
                        Документ.Add(new XElement("СерияРимскиеЦифры", adw3.Ser_Lat.Substring(0, adw3.Ser_Lat.Length > 8 ? 8 : adw3.Ser_Lat.Length).ToUpper()));
                    if (!String.IsNullOrEmpty(adw3.Ser_Rus))
                        Документ.Add(new XElement("СерияРусскиеБуквы", adw3.Ser_Rus.Substring(0, adw3.Ser_Rus.Length > 8 ? 8 : adw3.Ser_Rus.Length).ToUpper()));
                    if (!String.IsNullOrEmpty(adw3.Doc_Num))
                        Документ.Add(new XElement("НомерУдостоверяющего", adw3.Doc_Num.Substring(0, adw3.Doc_Num.Length > 8 ? 8 : adw3.Doc_Num.Length).ToUpper()));
                    Документ.Add(new XElement("ДатаВыдачи", adw3.Doc_Date.HasValue ? adw3.Doc_Date.Value.ToString("dd.MM.yyyy") : ""));
                    Документ.Add(new XElement("КемВыдан", adw3.Doc_Kem_Vyd.Substring(0, adw3.Doc_Kem_Vyd.Length > 80 ? 80 : adw3.Doc_Kem_Vyd.Length).ToUpper()));

                    УдостоверяющийДокумент.Add(Документ);

                    ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(УдостоверяющийДокумент);
                }
                else if (ishDocExist) // если есть документ
                {
                    ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(УдостоверяющийДокументИСХД);
                }


                try
                {
                    if (adw3.DateFilling.HasValue)
                    {
                        ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ.Add(new XElement("ДатаЗаполнения", adw3.DateFilling.Value.ToString("dd.MM.yyyy")));
                    }
                }
                catch { }





                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ЗАЯВЛЕНИЕ_О_ДУБЛИКАТЕ);
            }

            StaffID.Clear();
            staffListTemp.Clear();


            xDoc.Root.SetDefaultXmlNamespace(pfr);

            //     xml = xDoc.ToString();

            db_temp.Dispose();
            return xDoc;
        }

        private XDocument pregenerateXML_ODV1(xmlInfo item, int TypeForm)
        {

            string fileName = item.FileName;

            XNamespace УТ2 = "http://пф.рф/УТ/2017-08-21";
            XNamespace АФ4 = "http://пф.рф/АФ/2017-08-21";
            XNamespace ИС2 = "http://пф.рф/ВС/ИС/типы/2017-09-11";
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            XNamespace ВС2 = "http://пф.рф/ВС/типы/2017-10-23";


            List<XElement> formNode = new List<XElement>();


            string schLoc = "";
            XNamespace pfr = "http://пф.рф/ВС/ОДВ-1/2017-12-25";
            pu6Entities db_temp = new pu6Entities();
            FormsODV_1_2017 odv1 = new FormsODV_1_2017();
            Insurer ins = new Insurer();
            pfrXMLEntities dbxml_temp = new pfrXMLEntities();
            byte type_korr = 4;




            switch (TypeForm)
            {
                case 0: // одв1
                    schLoc = @"http://пф.рф/ВС/ОДВ-1/2017-12-25 ..\..\..\..\Схемы\ВС\Входящие\ИС2017\ОДВ-1_2017-12-25.xsd";
                    break;
                case 1: // сзв-стаж
                    pfr = "http://пф.рф/ВС/СЗВ-СТАЖ/2018-01-29";
                    schLoc = @"http://пф.рф/ВС/СЗВ-СТАЖ/2018-01-29 ..\..\..\..\Схемы\ВС\Входящие\ИС2017\СЗВ-СТАЖ_2018-01-29.xsd";
                    formNode.Add(generateXML_SZVSTAJ(item, pfr));
                    break;
                case 2:

                    odv1 = db_temp.FormsODV_1_2017.FirstOrDefault(x => x.ID == item.SourceID.Value);

                    ins = db_temp.Insurer.FirstOrDefault(x => x.ID == odv1.InsurerID.Value);

                    var itemStaff = dbxml_temp.StaffList.Where(x => x.XmlInfoID == item.ID).ToList();

                    pfr = "http://пф.рф/ВС/СЗВ-ИСХ/2017-05-01";
                    schLoc = @"http://пф.рф/ВС/СЗВ-ИСХ/2017-05-01 ..\..\..\..\Схемы\ВС\Входящие\ИС2017\СЗВ-ИСХ_2017-05-01.xsd";

                    foreach (var it in itemStaff)
                    {
                        formNode.Add(generateXML_SZVISH(item, pfr, ins, it, db_temp));
                    }
                    break;
                case 3:
                    odv1 = db_temp.FormsODV_1_2017.FirstOrDefault(x => x.ID == item.SourceID.Value);

                    ins = db_temp.Insurer.FirstOrDefault(x => x.ID == odv1.InsurerID.Value);

                    var itemStaff2 = dbxml_temp.StaffList.Where(x => x.XmlInfoID == item.ID).ToList();

                    if (itemStaff2.Count > 0)
                    {
                        long id_ = itemStaff2[0].FormsRSW_6_1_ID.Value;
                        FormsSZV_KORR_2017 szv = db_temp.FormsSZV_KORR_2017.First(x => x.ID == id_);
                        type_korr = szv.TypeInfo.HasValue ? szv.TypeInfo.Value : (byte)4;
                    }

                    pfr = "http://пф.рф/ВС/СЗВ-КОРР/2017-05-01";
                    schLoc = @"http://пф.рф/ВС/СЗВ-КОРР/2017-05-01 ..\..\..\..\Схемы\ВС\Входящие\ИС2017\СЗВ-КОРР_2017-05-01.xsd";

                    foreach (var it in itemStaff2)
                    {
                        formNode.Add(generateXML_SZVKORR(item, pfr, ins, it, db_temp));
                    }
                    break;
            }
            XElement odvNode = generateXML_ODV1(item, TypeForm, pfr, type_korr);

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", null),
                                 new XElement(pfr + "ЭДПФР",
                                     new XAttribute(XNamespace.Xmlns + "УТ2", УТ2.NamespaceName),
                                     new XAttribute(XNamespace.Xmlns + "АФ4", АФ4.NamespaceName),
                                     new XAttribute(XNamespace.Xmlns + "ИС2", ИС2.NamespaceName),
                                     new XAttribute(XNamespace.Xmlns + "ВС2", ВС2.NamespaceName),
                                     new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                                     new XAttribute(xsi + "schemaLocation", schLoc)));

            xDoc.Element(pfr + "ЭДПФР").Add(new XElement(pfr + "СлужебнаяИнформация",
                                          new XElement(АФ4 + "GUID", item.UniqGUID),
                                          new XElement(АФ4 + "ДатаВремя", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")),
                                          new XElement(АФ4 + "ПрограммаПодготовки", Application.ProductName.ToUpper() + " " + Application.ProductVersion)));

            xDoc.Element(pfr + "ЭДПФР").Add(odvNode);
            foreach (var itemNode in formNode)
            {
                xDoc.Element(pfr + "ЭДПФР").Add(itemNode);
            }

            dbxml_temp.Dispose();
            db_temp.Dispose();

            return xDoc;


        }

        private XElement generateXML_ODV1(xmlInfo item, int TypeForm, XNamespace pfr, byte type_korr)
        {
            pu6Entities db_temp = new pu6Entities();

            FormsODV_1_2017 odv1 = db_temp.FormsODV_1_2017.FirstOrDefault(x => x.ID == item.SourceID.Value);

            Insurer ins = db_temp.Insurer.FirstOrDefault(x => x.ID == odv1.InsurerID.Value);

            //pfrXMLEntities dbxml_temp = new pfrXMLEntities();

            //xmlInfo item = dbxml_temp.xmlInfo.First(x => x.ID == itemT.ID);

            string regNum = Utils.ParseRegNum(ins.RegNum);

            string orgName = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    orgName = ins.NameShort.ToUpper();
                }
                else if (!String.IsNullOrEmpty(ins.Name))
                {
                    orgName = ins.Name.ToUpper();
                }

            }
            else // если физ. лицо
            {
                orgName = ins.LastName + " " + ins.FirstName + " " + ins.MiddleName;
            }

            if (!String.IsNullOrEmpty(orgName) && orgName.Length > 255)
            {
                orgName = orgName.Substring(0, 255);
            }

            XNamespace УТ2 = "http://пф.рф/УТ/2017-08-21";
            XNamespace АФ4 = "http://пф.рф/АФ/2017-08-21";
            XNamespace ИС2 = "http://пф.рф/ВС/ИС/типы/2017-09-11";

            XElement ОДВ1 = new XElement(pfr + "ОДВ-1",
                 new XElement(pfr + "Тип", odv1.TypeInfo.HasValue ? odv1.TypeInfo.Value : 0),
                 new XElement(pfr + "Страхователь",
                                                    new XElement(УТ2 + "РегНомер", regNum),
                                                    new XElement(УТ2 + "ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                ОДВ1.Element(pfr + "Страхователь").Add(new XElement(УТ2 + "КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            ОДВ1.Element(pfr + "Страхователь").Add(new XElement(ИС2 + "Наименование", orgName.Trim()));

            int cntT = item.StaffList != null ? item.StaffList.Count() : 0;

            ОДВ1.Add(new XElement(pfr + "ОтчетныйПериод",
                                new XElement(pfr + "Код", odv1.Code),
                                new XElement(pfr + "Год", odv1.Year)));

            if (TypeForm != 0)
            {
                ОДВ1.Add(new XElement(pfr + "КоличествоЗЛ", cntT));
            }


            if (TypeForm == 2) // Если СЗВ-ИСХ
            {
                ОДВ1.Add(new XElement(pfr + "Страховая",
                            new XElement(ИС2 + "ЗадолженностьНаНачало", Utils.decToStr(odv1.s_0_0)),
                            new XElement(ИС2 + "Начислено", Utils.decToStr(odv1.s_0_1)),
                            new XElement(ИС2 + "Уплачено", Utils.decToStr(odv1.s_0_2)),
                            new XElement(ИС2 + "ЗадолженностьНаКонец", Utils.decToStr(odv1.s_0_3))),
                        new XElement(pfr + "Накопительная",
                            new XElement(ИС2 + "ЗадолженностьНаНачало", Utils.decToStr(odv1.s_1_0)),
                            new XElement(ИС2 + "Начислено", Utils.decToStr(odv1.s_1_1)),
                            new XElement(ИС2 + "Уплачено", Utils.decToStr(odv1.s_1_2)),
                            new XElement(ИС2 + "ЗадолженностьНаКонец", Utils.decToStr(odv1.s_1_3))),
                        new XElement(pfr + "ТарифСВ",
                            new XElement(ИС2 + "ЗадолженностьНаНачало", Utils.decToStr(odv1.s_2_0)),
                            new XElement(ИС2 + "Начислено", Utils.decToStr(odv1.s_2_1)),
                            new XElement(ИС2 + "Уплачено", Utils.decToStr(odv1.s_2_2)),
                            new XElement(ИС2 + "ЗадолженностьНаКонец", Utils.decToStr(odv1.s_2_3))));

            }
            else if (TypeForm == 3 && type_korr == 2) // Если СЗВ-КОРР
            {
                ОДВ1.Add(new XElement(pfr + "Страховая",
                            new XElement(ИС2 + "ЗадолженностьНаНачало", Utils.decToStr(odv1.s_0_0)),
                            new XElement(ИС2 + "Начислено", Utils.decToStr(odv1.s_0_1)),
                            new XElement(ИС2 + "Уплачено", Utils.decToStr(odv1.s_0_2)),
                            new XElement(ИС2 + "ЗадолженностьНаКонец", Utils.decToStr(odv1.s_0_3))),
                        new XElement(pfr + "Накопительная",
                            new XElement(ИС2 + "ЗадолженностьНаНачало", Utils.decToStr(odv1.s_1_0)),
                            new XElement(ИС2 + "Начислено", Utils.decToStr(odv1.s_1_1)),
                            new XElement(ИС2 + "Уплачено", Utils.decToStr(odv1.s_1_2)),
                            new XElement(ИС2 + "ЗадолженностьНаКонец", Utils.decToStr(odv1.s_1_3))),
                        new XElement(pfr + "ТарифСВ",
                            new XElement(ИС2 + "ЗадолженностьНаНачало", Utils.decToStr(odv1.s_2_0)),
                            new XElement(ИС2 + "Начислено", Utils.decToStr(odv1.s_2_1)),
                            new XElement(ИС2 + "Уплачено", Utils.decToStr(odv1.s_2_2)),
                            new XElement(ИС2 + "ЗадолженностьНаКонец", Utils.decToStr(odv1.s_2_3))));


                foreach (var itm in odv1.FormsODV_1_4_2017.ToList())
                {

                    XElement Уплата = new XElement(ИС2 + "Уплата",
                        new XElement(ИС2 + "Год", itm.Year),
                        new XElement(ИС2 + "Страховая", Utils.decToStr(itm.OPS)),
                        new XElement(ИС2 + "Накопительная", Utils.decToStr(itm.NAKOP)),
                        new XElement(ИС2 + "ТарифСВ", Utils.decToStr(itm.DopTar)));

                    ОДВ1.Add(Уплата);
                }

            }


            if (TypeForm == 0 || TypeForm == 1 || TypeForm == 2) // Если форма ОДВ1 или СЗВ-СТАЖ
            {
                if (odv1.FormsODV_1_5_2017.Count > 0)
                {
                    XElement ОснованияДНП = new XElement(pfr + "ОснованияДНП");

                    foreach (var itm in odv1.FormsODV_1_5_2017.ToList())
                    {

                        XElement Основание = new XElement(pfr + "Основание",
                            new XElement(ИС2 + "Подразделение", itm.Department.Trim()),
                            new XElement(ИС2 + "ПрофессияДолжность", itm.Profession.Trim()),
                            new XElement(ИС2 + "КоличествоШтат", itm.StaffCountShtat.HasValue ? itm.StaffCountShtat.Value : 0),
                            new XElement(ИС2 + "КоличествоФакт", itm.StaffCountFakt.HasValue ? itm.StaffCountFakt.Value : 0),
                            new XElement(ИС2 + "Описание", itm.VidRabotFakt.Trim()),
                            new XElement(ИС2 + "Документы", itm.DocsName.Trim()));


                        foreach (var itout in itm.FormsODV_1_5_2017_OUT.ToList())
                        {
                            XElement ОУТ = new XElement(pfr + "ОУТ",
                                   new XElement(pfr + "Код", itout.OsobUslTrudaCode != null ? itout.OsobUslTrudaCode.Trim() : ""));

                            if (itout.CodePosition != null && !String.IsNullOrEmpty(itout.CodePosition.Trim()))
                            {
                                ОУТ.Add(new XElement(pfr + "ПозицияСписка", itout.CodePosition.Trim()));
                            }

                            Основание.Add(ОУТ);

                        }


                        ОснованияДНП.Add(Основание);
                    }

                    ОснованияДНП.Add(new XElement(ИС2 + "ВсегоШтат", odv1.StaffCountOsobUslShtat.HasValue ? odv1.StaffCountOsobUslShtat.Value : 0));
                    ОснованияДНП.Add(new XElement(ИС2 + "ВсегоФакт", odv1.StaffCountOsobUslFakt.HasValue ? odv1.StaffCountOsobUslFakt.Value : 0));

                    ОДВ1.Add(ОснованияДНП);

                }

            }


            XElement Руководитель = new XElement(pfr + "Руководитель");
            bool flag = false;

            if (flag = (!String.IsNullOrEmpty(odv1.ConfirmDolgn) && odv1.ConfirmDolgn.Trim() != ""))
            {
                Руководитель.Add(new XElement(pfr + "Должность", odv1.ConfirmDolgn.Trim().ToUpper()));
            }

            if ((!String.IsNullOrEmpty(odv1.ConfirmLastName) && odv1.ConfirmLastName.Trim() != "") || (!String.IsNullOrEmpty(odv1.ConfirmFirstName) && odv1.ConfirmFirstName.Trim() != "") || (!String.IsNullOrEmpty(odv1.ConfirmMiddleName) && odv1.ConfirmMiddleName.Trim() != ""))
            {
                XElement ФИО = new XElement(pfr + "ФИО");
                if (!String.IsNullOrEmpty(odv1.ConfirmLastName.Trim()) && odv1.ConfirmLastName.Trim() != "")
                {
                    ФИО.Add(new XElement(УТ2 + "Фамилия", odv1.ConfirmLastName.Trim().ToUpper()));
                    flag = true;
                }
                if (!String.IsNullOrEmpty(odv1.ConfirmFirstName.Trim()) && odv1.ConfirmFirstName.Trim() != "")
                {
                    ФИО.Add(new XElement(УТ2 + "Имя", odv1.ConfirmFirstName.Trim().ToUpper()));
                    flag = true;
                }
                if (!String.IsNullOrEmpty(odv1.ConfirmMiddleName.Trim()) && odv1.ConfirmMiddleName.Trim() != "")
                {
                    ФИО.Add(new XElement(УТ2 + "Отчество", odv1.ConfirmMiddleName.Trim().ToUpper()));
                    flag = true;
                }
                Руководитель.Add(ФИО);

            }

            if (flag)
                ОДВ1.Add(Руководитель);

            ОДВ1.Add(new XElement(pfr + "ДатаЗаполнения", odv1.DateFilling.ToString("yyyy-MM-dd")));

            return ОДВ1;
        }

        private XElement generateXML_SZVSTAJ(xmlInfo itemT, XNamespace pfr)
        {
            pu6Entities db_temp = new pu6Entities();

            FormsODV_1_2017 odv1 = db_temp.FormsODV_1_2017.FirstOrDefault(x => x.ID == itemT.SourceID.Value);

            Insurer ins = db_temp.Insurer.FirstOrDefault(x => x.ID == odv1.InsurerID.Value);

            pfrXMLEntities dbxml_temp = new pfrXMLEntities();

            var itemStaff = dbxml_temp.StaffList.Where(x => x.XmlInfoID == itemT.ID).ToList();

            string regNum = Utils.ParseRegNum(ins.RegNum);

            string orgName = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    orgName = ins.NameShort.ToUpper();
                }
                else if (!String.IsNullOrEmpty(ins.Name))
                {
                    orgName = ins.Name.ToUpper();
                }

            }
            else // если физ. лицо
            {
                orgName = ins.LastName + " " + ins.FirstName + " " + ins.MiddleName;
            }

            if (!String.IsNullOrEmpty(orgName) && orgName.Length > 255)
            {
                orgName = orgName.Substring(0, 255);
            }

            long id = itemStaff[0].FormsRSW_6_1_ID.Value;
            FormsSZV_STAJ_2017 szv = db_temp.FormsSZV_STAJ_2017.First(x => x.ID == id);

            XNamespace УТ2 = "http://пф.рф/УТ/2017-08-21";
            XNamespace АФ4 = "http://пф.рф/АФ/2017-08-21";
            XNamespace ИС2 = "http://пф.рф/ВС/ИС/типы/2017-09-11";

            XElement СЗВ = new XElement(pfr + "СЗВ-СТАЖ",
     new XElement(pfr + "Страхователь",
                                        new XElement(УТ2 + "РегНомер", regNum),
                                        new XElement(УТ2 + "ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СЗВ.Element(pfr + "Страхователь").Add(new XElement(УТ2 + "КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            СЗВ.Element(pfr + "Страхователь").Add(new XElement(ИС2 + "Наименование", orgName.Trim()));

            int szv_t = szv.TypeInfo.HasValue ? szv.TypeInfo.Value : 0;

            СЗВ.Add(new XElement(pfr + "Тип", szv_t));

            СЗВ.Add(new XElement(pfr + "ОтчетныйПериод",
                                new XElement(pfr + "Код", 0),
                                new XElement(pfr + "Год", szv.Year)));

            List<long> t = itemStaff.Select(x => x.StaffID.Value).ToList();
            var StaffList = db_temp.Staff.Where(x => t.Contains(x.ID)).ToList();

            var t2 = itemStaff.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

            var szvList_tmp = db_temp.FormsSZV_STAJ_2017.Where(x => t2.Contains(x.ID)).ToList();
            //t = rsw61List_tmp.Select(x => x.ID).ToList();

            var stajOsn_list = db_temp.StajOsn.Where(x => t2.Contains(x.FormsSZV_STAJ_2017_ID.Value)).ToList();//
            List<long> t__ = stajOsn_list.Select(x => x.ID).ToList();

            var stajLgot_list_t = db_temp.StajLgot.Where(x => t__.Contains(x.StajOsnID)).ToList();

            t__.Clear();


            foreach (var staff in itemStaff)
            {
                FormsSZV_STAJ_2017 szvstaj = szvList_tmp.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);

                Staff ish_staff = StaffList.First(x => x.ID == staff.StaffID);
                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                XElement ЗЛ = new XElement(pfr + "ЗЛ");



                XElement ФИО = new XElement(pfr + "ФИО");

                if (!String.IsNullOrEmpty(ish_staff.LastName.Trim()))
                {
                    ФИО.Add(new XElement(УТ2 + "Фамилия", ish_staff.LastName.Trim().Substring(0, ish_staff.LastName.Trim().Length > 40 ? 40 : ish_staff.LastName.Trim().Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ish_staff.FirstName.Trim()))
                {
                    ФИО.Add(new XElement(УТ2 + "Имя", ish_staff.FirstName.Trim().Substring(0, ish_staff.FirstName.Trim().Length > 40 ? 40 : ish_staff.FirstName.Trim().Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ish_staff.MiddleName.Trim()))
                {
                    ФИО.Add(new XElement(УТ2 + "Отчество", ish_staff.MiddleName.Trim().Substring(0, ish_staff.MiddleName.Trim().Length > 40 ? 40 : ish_staff.MiddleName.Trim().Length).ToUpper()));
                }


                ЗЛ.Add(ФИО);
                ЗЛ.Add(new XElement(pfr + "СНИЛС", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""));

                var staj_osn_list = stajOsn_list.Where(x => x.FormsSZV_STAJ_2017_ID == szvstaj.ID).OrderBy(x => x.Number.Value).ToList();

                var tt = staj_osn_list.Select(x => x.ID).ToList();
                var stajLgot_list = stajLgot_list_t.Where(x => tt.Contains(x.StajOsnID)).ToList(); // db_temp
                int ii = 0;
                foreach (var staj_osn in staj_osn_list)
                {
                    try
                    {
                        ii++;
                        XElement СтажевыйПериод = createStajElement_2017(staj_osn, stajLgot_list, ii, pfr, УТ2, ИС2);

                        //Если стоит статус БЕЗР
                        if (staj_osn.CodeBEZR.HasValue && staj_osn.CodeBEZR.Value)
                            СтажевыйПериод.Add(new XElement(pfr + "КатегорияЗЛ", "БЕЗР"));


                        ЗЛ.Add(СтажевыйПериод);
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении СЗВ-СТАЖ произошла ошибка при обработке стажа.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));
                    }

                }
                //Если уволен 31 декабря
                if (szvstaj.Dismissed.HasValue && szvstaj.Dismissed.Value)
                    ЗЛ.Add(new XElement(pfr + "ДатаУвольнения", szvstaj.Year.ToString() + "-12-31"));

                СЗВ.Add(ЗЛ);
            }

            if (szv_t == 2)
            {
                XElement СВ = new XElement(pfr + "СВ");

                //if (szv.OPSFeeNach.HasValue && szv.OPSFeeNach.Value != 0)
                //{
                    СВ.Add(new XElement(pfr + "НачисленыНаОПС", szv.OPSFeeNach.HasValue ? szv.OPSFeeNach.Value : 0));
                //}
                //if (szv.DopTarFeeNach.HasValue && szv.DopTarFeeNach.Value != 0)
                //{
                    СВ.Add(new XElement(pfr + "НачисленыПоДТ", szv.DopTarFeeNach.HasValue ? szv.DopTarFeeNach.Value : 0));
                //}
                СЗВ.Add(СВ);
            }

            foreach (var item in szv.FormsSZV_STAJ_4_2017)
            {
                XElement Уплата = new XElement(pfr + "Уплата",
                    new XElement(pfr + "Период",
                        new XElement(УТ2 + "С", item.DNPO_DateFrom.HasValue ? item.DNPO_DateFrom.Value.ToString("yyyy-MM-dd") : ""),
                        new XElement(УТ2 + "По", item.DNPO_DateTo.HasValue ? item.DNPO_DateTo.Value.ToString("yyyy-MM-dd") : "")),
                    new XElement(pfr + "Уплачено", item.DNPO_Fee.HasValue ? (item.DNPO_Fee.Value ? 1 : 0) : 0));

                СЗВ.Add(Уплата);

            }



            return СЗВ;
        }

        private XElement generateXML_SZVISH(xmlInfo itemT, XNamespace pfr, Insurer ins, StaffList StaffItem, pu6Entities db_temp)
        {

            string regNum = Utils.ParseRegNum(ins.RegNum);

            string orgName = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    orgName = ins.NameShort.ToUpper();
                }
                else if (!String.IsNullOrEmpty(ins.Name))
                {
                    orgName = ins.Name.ToUpper();
                }

            }
            else // если физ. лицо
            {
                orgName = ins.LastName + " " + ins.FirstName + " " + ins.MiddleName;
            }

            if (!String.IsNullOrEmpty(orgName) && orgName.Length > 255)
            {
                orgName = orgName.Substring(0, 255);
            }

            FormsSZV_ISH_2017 szv = db_temp.FormsSZV_ISH_2017.First(x => x.ID == StaffItem.FormsRSW_6_1_ID.Value);

            XNamespace УТ2 = "http://пф.рф/УТ/2017-08-21";
            XNamespace АФ4 = "http://пф.рф/АФ/2017-08-21";
            XNamespace ИС2 = "http://пф.рф/ВС/ИС/типы/2017-09-11";

            XElement СЗВ = new XElement(pfr + "СЗВ-ИСХ",
                               new XElement(pfr + "Страхователь",
                                        new XElement(УТ2 + "РегНомер", regNum),
                                        new XElement(УТ2 + "ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СЗВ.Element(pfr + "Страхователь").Add(new XElement(УТ2 + "КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            СЗВ.Element(pfr + "Страхователь").Add(new XElement(ИС2 + "Наименование", orgName.Trim()));



            Staff ish_staff = db_temp.Staff.FirstOrDefault(x => x.ID == StaffItem.StaffID.Value);

            string contrNum = "";
            if (ish_staff.ControlNumber != null)
            {
                contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
            }

            XElement ФИО = new XElement(pfr + "ФИО");

            if (!String.IsNullOrEmpty(ish_staff.LastName.Trim()))
            {
                ФИО.Add(new XElement(УТ2 + "Фамилия", ish_staff.LastName.Trim().Substring(0, ish_staff.LastName.Trim().Length > 40 ? 40 : ish_staff.LastName.Trim().Length).ToUpper()));
            }

            if (!String.IsNullOrEmpty(ish_staff.FirstName.Trim()))
            {
                ФИО.Add(new XElement(УТ2 + "Имя", ish_staff.FirstName.Trim().Substring(0, ish_staff.FirstName.Trim().Length > 40 ? 40 : ish_staff.FirstName.Trim().Length).ToUpper()));
            }

            if (!String.IsNullOrEmpty(ish_staff.MiddleName.Trim()))
            {
                ФИО.Add(new XElement(УТ2 + "Отчество", ish_staff.MiddleName.Trim().Substring(0, ish_staff.MiddleName.Trim().Length > 40 ? 40 : ish_staff.MiddleName.Trim().Length).ToUpper()));
            }

            СЗВ.Add(ФИО);

            СЗВ.Add(new XElement(pfr + "СНИЛС", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""));


            if (szv.ContractType.HasValue && szv.ContractType.Value != 0)
            {
                XElement Договор = new XElement(pfr + "Договор");

                Договор.Add(new XElement(pfr + "Тип", szv.ContractType.Value));
                if (szv.ContractDate.HasValue && !String.IsNullOrEmpty(szv.ContractNum.Trim()))
                {
                    Договор.Add(new XElement(pfr + "Реквизиты",
                                    new XElement(УТ2 + "Дата", szv.ContractDate.Value.ToString("yyyy-MM-dd")),
                                    new XElement(УТ2 + "Номер", szv.ContractNum.Trim())));
                }

                СЗВ.Add(Договор);
            }

            if (!String.IsNullOrEmpty(szv.DopTarCode.Trim()))
            {
                СЗВ.Add(new XElement(pfr + "КодДТ", szv.DopTarCode.Trim()));
            }

            СЗВ.Add(new XElement(pfr + "ОтчетныйПериод",
                                new XElement(pfr + "Код", szv.Code),
                                new XElement(pfr + "Год", szv.Year)));


            if (szv.FormsSZV_ISH_4_2017.Count > 0)
            {
                XElement Выплаты = new XElement(pfr + "Выплаты");
                foreach (var item in szv.FormsSZV_ISH_4_2017.ToList())
                {
                    XElement Период = new XElement(pfr + "Период",
                                          new XElement(pfr + "Месяц", item.Month.HasValue ? (item.Month.Value <= 12 ? MonthesList[item.Month.Value - 1] : "") : ""),
                                          new XElement(pfr + "Категория", item.PlatCategory.Code),
                                          new XElement(pfr + "СуммаВыплат", Utils.decToStr(item.SumFeePFR)),
                                          new XElement(pfr + "НеПревышающие",
                                              new XElement(pfr + "Всего", Utils.decToStr(item.BaseALL)),
                                              new XElement(pfr + "ПоГПД", Utils.decToStr(item.BaseGPD))),
                                          new XElement(pfr + "Превышающие",
                                              new XElement(pfr + "Всего", Utils.decToStr(item.SumPrevBaseALL)),
                                              new XElement(pfr + "ПоГПД", Utils.decToStr(item.SumPrevBaseGPD))));


                    Выплаты.Add(Период);

                }

                var list_t = szv.FormsSZV_ISH_4_2017.ToList();
                bool flag = list_t.Select(x => x.PlatCategoryID).Distinct().Count() <= 1;  // если категория не менялась за период, и всегда одна и та же

                XElement Всего = new XElement(pfr + "Всего",
                                     new XElement(pfr + "Категория", flag ? list_t[0].PlatCategory.Code : ""),
                                     new XElement(pfr + "СуммаВыплат", Utils.decToStr(list_t.Sum(x => x.SumFeePFR.Value))),
                                         new XElement(pfr + "НеПревышающие",
                                             new XElement(pfr + "Всего", Utils.decToStr(list_t.Sum(x => x.BaseALL.Value))),
                                             new XElement(pfr + "ПоГПД", Utils.decToStr(list_t.Sum(x => x.BaseGPD.Value)))),
                                         new XElement(pfr + "Превышающие",
                                             new XElement(pfr + "Всего", Utils.decToStr(list_t.Sum(x => x.SumPrevBaseALL.Value))),
                                             new XElement(pfr + "ПоГПД", Utils.decToStr(list_t.Sum(x => x.SumPrevBaseGPD.Value))))
                                     );

                Выплаты.Add(Всего);
                СЗВ.Add(Выплаты);
            }

            XElement Начисления = new XElement(pfr + "Начисления");
            bool flag_ = false;

            if (szv.SumFeePFR_Insurer.HasValue && szv.SumFeePFR_Insurer.Value != 0)
            {
                Начисления.Add(new XElement(pfr + "СВстрахователя", Utils.decToStr(szv.SumFeePFR_Insurer)));
                flag_ = true;
            }
            if (szv.SumFeePFR_Staff.HasValue && szv.SumFeePFR_Staff.Value != 0)
            {
                Начисления.Add(new XElement(pfr + "СВизЗаработка", Utils.decToStr(szv.SumFeePFR_Staff)));
                flag_ = true;
            }
            if (szv.SumFeePFR_Tar.HasValue && szv.SumFeePFR_Tar.Value != 0)
            {
                Начисления.Add(new XElement(pfr + "СВпоТарифу", Utils.decToStr(szv.SumFeePFR_Tar)));
                flag_ = true;
            }
            if (szv.SumFeePFR_TarDop.HasValue && szv.SumFeePFR_TarDop.Value != 0)
            {

                Начисления.Add(new XElement(pfr + "СВпоДопТарифу", Utils.decToStr(szv.SumFeePFR_TarDop)));
                flag_ = true;
            }
            if (szv.SumFeePFR_Strah.HasValue && szv.SumFeePFR_Strah.Value != 0)
            {

                Начисления.Add(new XElement(pfr + "Страховая", Utils.decToStr(szv.SumFeePFR_Strah)));
                flag_ = true;
            }
            if (szv.SumFeePFR_Nakop.HasValue && szv.SumFeePFR_Nakop.Value != 0)
            {

                Начисления.Add(new XElement(pfr + "Накопительная", Utils.decToStr(szv.SumFeePFR_Nakop)));
                flag_ = true;
            }
            if (szv.SumFeePFR_Base.HasValue && szv.SumFeePFR_Base.Value != 0)
            {

                Начисления.Add(new XElement(pfr + "СВпоТарифуНеПревышающие", Utils.decToStr(szv.SumFeePFR_Base)));
                flag_ = true;
            }


            if (flag_)
                СЗВ.Add(Начисления);


            XElement Уплата = new XElement(pfr + "Уплата");
            flag_ = false;

            if (szv.SumPayPFR_Strah.HasValue && szv.SumPayPFR_Strah.Value != 0)
            {
                Уплата.Add(new XElement(pfr + "Страховая", Utils.decToStr(szv.SumPayPFR_Strah)));
                flag_ = true;
            }
            if (szv.SumPayPFR_Nakop.HasValue && szv.SumPayPFR_Nakop.Value != 0)
            {
                Уплата.Add(new XElement(pfr + "Накопительная", Utils.decToStr(szv.SumPayPFR_Nakop)));
                flag_ = true;
            }

            if (flag_)
                СЗВ.Add(Уплата);

            int m = 0;
            switch (szv.Code)
            {
                case 0:
                    m = 10;
                    break;
                case 3:
                    m = 1;
                    break;
                case 6:
                    m = 4;
                    break;
                case 9:
                    m = 7;
                    break;
            }


            if (szv.FormsSZV_ISH_7_2017.Count > 0)
            {
                XElement ВыплатыДТ = new XElement(pfr + "ВыплатыДТ");

                var list_t = szv.FormsSZV_ISH_7_2017.ToList();

                decimal s1 = list_t.Sum(x => x.s_1_0.Value) + list_t.Sum(x => x.s_2_0.Value) + list_t.Sum(x => x.s_3_0.Value);
                decimal s2 = list_t.Sum(x => x.s_1_1.Value) + list_t.Sum(x => x.s_2_1.Value) + list_t.Sum(x => x.s_3_1.Value);

                XElement Всего = new XElement(pfr + "Всего",
                                             new XElement(ИС2 + "ДопТарифП1", Utils.decToStr(s1)),
                                             new XElement(ИС2 + "ДопТарифП2_18", Utils.decToStr(s2)));

                ВыплатыДТ.Add(Всего);


                foreach (var item in szv.FormsSZV_ISH_7_2017.ToList())
                {
                    string code = item.SpecOcenkaUslTrudaID.HasValue ? item.SpecOcenkaUslTruda.Code : "";

                    XElement Период = new XElement(pfr + "Период",
                                          new XElement(pfr + "Месяц", MonthesList[m - 1]),
                                          new XElement(pfr + "КодСОУТ", code),
                                          new XElement(ИС2 + "ДопТарифП1", Utils.decToStr(item.s_1_0)),
                                          new XElement(ИС2 + "ДопТарифП2_18", Utils.decToStr(item.s_1_1)));
                    ВыплатыДТ.Add(Период);
                    Период = new XElement(pfr + "Период",
                                          new XElement(pfr + "Месяц", MonthesList[m]),
                                          new XElement(pfr + "КодСОУТ", code),
                                          new XElement(ИС2 + "ДопТарифП1", Utils.decToStr(item.s_2_0)),
                                          new XElement(ИС2 + "ДопТарифП2_18", Utils.decToStr(item.s_2_1)));
                    ВыплатыДТ.Add(Период);
                    Период = new XElement(pfr + "Период",
                                          new XElement(pfr + "Месяц", MonthesList[m + 1]),
                                          new XElement(pfr + "КодСОУТ", code),
                                          new XElement(ИС2 + "ДопТарифП1", Utils.decToStr(item.s_3_0)),
                                          new XElement(ИС2 + "ДопТарифП2_18", Utils.decToStr(item.s_3_1)));
                    ВыплатыДТ.Add(Период);

                }

                СЗВ.Add(ВыплатыДТ);
            }






            var staj_osn_list = db_temp.StajOsn.Where(x => x.FormsSZV_ISH_2017_ID == szv.ID).OrderBy(x => x.Number.Value).ToList();

            var tt = staj_osn_list.Select(x => x.ID).ToList();
            var stajLgot_list = db_temp.StajLgot.Where(x => tt.Contains(x.StajOsnID)).ToList(); // db_temp
            int ii = 0;
            foreach (var staj_osn in staj_osn_list)
            {
                try
                {
                    ii++;
                    XElement СтажевыйПериод = createStajElement_2017(staj_osn, stajLgot_list, ii, pfr, УТ2, ИС2);
                    СЗВ.Add(СтажевыйПериод);
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении СЗВ-ИСХ произошла ошибка при обработке стажа.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));
                }

            }



            return СЗВ;
        }

        private XElement generateXML_SZVKORR(xmlInfo itemT, XNamespace pfr, Insurer ins, StaffList StaffItem, pu6Entities db_temp)
        {

            string regNum = Utils.ParseRegNum(ins.RegNum);

            string orgName = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    orgName = ins.NameShort.ToUpper();
                }
                else if (!String.IsNullOrEmpty(ins.Name))
                {
                    orgName = ins.Name.ToUpper();
                }

            }
            else // если физ. лицо
            {
                orgName = ins.LastName + " " + ins.FirstName + " " + ins.MiddleName;
            }

            if (!String.IsNullOrEmpty(orgName) && orgName.Length > 255)
            {
                orgName = orgName.Substring(0, 255);
            }

            FormsSZV_KORR_2017 szv = db_temp.FormsSZV_KORR_2017.First(x => x.ID == StaffItem.FormsRSW_6_1_ID.Value);

            XNamespace УТ2 = "http://пф.рф/УТ/2017-08-21";
            XNamespace АФ4 = "http://пф.рф/АФ/2017-08-21";
            XNamespace ИС2 = "http://пф.рф/ВС/ИС/типы/2017-09-11";

            XElement СЗВ = new XElement(pfr + "СЗВ-КОРР",
                               new XElement(pfr + "Страхователь",
                                        new XElement(УТ2 + "РегНомер", regNum),
                                        new XElement(УТ2 + "ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СЗВ.Element(pfr + "Страхователь").Add(new XElement(УТ2 + "КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            СЗВ.Element(pfr + "Страхователь").Add(new XElement(ИС2 + "Наименование", orgName.Trim()));


            СЗВ.Add(new XElement(pfr + "ОтчетныйПериод",
                                new XElement(pfr + "Код", szv.Code),
                                new XElement(pfr + "Год", szv.Year)));

            СЗВ.Add(new XElement(pfr + "Тип", szv.TypeInfo.HasValue ? szv.TypeInfo.Value : 0));


            regNum = Utils.ParseRegNum(szv.RegNumKorr);

            XElement КорректируемыйПериод = new XElement(pfr + "КорректируемыйПериод",
                                                new XElement(pfr + "ОтчетныйПериод",
                                                    new XElement(pfr + "Код", szv.CodeKorr),
                                                    new XElement(pfr + "Год", szv.YearKorr)),
                                                new XElement(pfr + "Страхователь",
                                                    new XElement(УТ2 + "РегНомер", regNum),
                                                    new XElement(УТ2 + "ИНН", !String.IsNullOrEmpty(szv.INNKorr.ToString()) ? szv.INNKorr.ToString() : "")));

            if (!String.IsNullOrEmpty(szv.KPPKorr))
            {
                КорректируемыйПериод.Element(pfr + "Страхователь").Add(new XElement(УТ2 + "КПП", szv.KPPKorr.Substring(0, szv.KPPKorr.Length > 9 ? 9 : szv.KPPKorr.Length)));
            }

            //if (!String.IsNullOrEmpty(szv.ShortNameKorr))
            //{
            //    КорректируемыйПериод.Element(pfr + "Страхователь").Add(new XElement(УТ + "Наименование", szv.ShortNameKorr));
            //}

            СЗВ.Add(КорректируемыйПериод);

            Staff ish_staff = db_temp.Staff.FirstOrDefault(x => x.ID == StaffItem.StaffID.Value);

            XElement ЗЛ = new XElement(pfr + "ЗЛ");

            string contrNum = "";
            if (ish_staff.ControlNumber != null)
            {
                contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
            }

            XElement ФИО = new XElement(pfr + "ФИО");

            if (!String.IsNullOrEmpty(ish_staff.LastName.Trim()))
            {
                ФИО.Add(new XElement(УТ2 + "Фамилия", ish_staff.LastName.Trim().Substring(0, ish_staff.LastName.Trim().Length > 40 ? 40 : ish_staff.LastName.Trim().Length).ToUpper()));
            }

            if (!String.IsNullOrEmpty(ish_staff.FirstName.Trim()))
            {
                ФИО.Add(new XElement(УТ2 + "Имя", ish_staff.FirstName.Trim().Substring(0, ish_staff.FirstName.Trim().Length > 40 ? 40 : ish_staff.FirstName.Trim().Length).ToUpper()));
            }

            if (!String.IsNullOrEmpty(ish_staff.MiddleName.Trim()))
            {
                ФИО.Add(new XElement(УТ2 + "Отчество", ish_staff.MiddleName.Trim().Substring(0, ish_staff.MiddleName.Trim().Length > 40 ? 40 : ish_staff.MiddleName.Trim().Length).ToUpper()));
            }

            ЗЛ.Add(ФИО);

            ЗЛ.Add(new XElement(pfr + "СНИЛС", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""));

            СЗВ.Add(ЗЛ);

            if (szv.TypeInfo.HasValue && szv.TypeInfo.Value != 1)
            {
                XElement ДанныеЗЛ = new XElement(pfr + "ДанныеЗЛ");

                bool fl0 = false;
                if (szv.PlatCategoryID.HasValue)
                {
                    ДанныеЗЛ.Add(new XElement(pfr + "Категория", szv.PlatCategory.Code));
                    fl0 = true;
                }

                XElement Договор = new XElement(pfr + "Договор");

                bool fl = false;
                if (szv.ContractType.HasValue && szv.ContractType.Value != 0)
                {
                    Договор.Add(new XElement(pfr + "Тип", szv.ContractType.Value));
                    fl = true;
                }
                if (szv.ContractDate.HasValue && !String.IsNullOrEmpty(szv.ContractNum.Trim()))
                {
                    Договор.Add(new XElement(pfr + "Реквизиты",
                                    new XElement(УТ2 + "Дата", szv.ContractDate.Value.ToString("yyyy-MM-dd")),
                                    new XElement(УТ2 + "Номер", szv.ContractNum.Trim())));
                    fl = true;
                }

                if (fl)
                    ДанныеЗЛ.Add(Договор);

                if (szv.DopTarCode != null && !String.IsNullOrEmpty(szv.DopTarCode.Trim()))
                {
                    ДанныеЗЛ.Add(new XElement(pfr + "КодДТ", szv.DopTarCode.Trim()));
                    fl0 = true;
                }

                if (fl || fl0)
                    СЗВ.Add(ДанныеЗЛ);
            }



            if (szv.FormsSZV_KORR_4_2017.Count > 0)
            {
                foreach (var item in szv.FormsSZV_KORR_4_2017.ToList())
                {
                    XElement Суммы = new XElement(pfr + "Суммы",
                                          new XElement(pfr + "Месяц", item.Month.HasValue ? (item.Month.Value <= 12 ? MonthesList[item.Month.Value - 1] : "") : ""));

                    #region
                    XElement Выплаты = new XElement(pfr + "Выплаты");
                    bool flag_ = false;
                    bool flag_3 = false;

                    if (item.SumFeePFR.HasValue && item.SumFeePFR.Value != 0)
                    {
                        Выплаты.Add(new XElement(pfr + "СуммаВыплат", Utils.decToStr(item.SumFeePFR)));
                        flag_ = true;
                    }
                    bool flag_2 = false;
                    XElement НеПревышающие = new XElement(pfr + "НеПревышающие");
                    if (item.BaseALL.HasValue && item.BaseALL.Value != 0)
                    {
                        НеПревышающие.Add(new XElement(pfr + "Всего", Utils.decToStr(item.BaseALL)));
                        flag_ = true;
                        flag_2 = true;
                    }
                    if (item.BaseGPD.HasValue && item.BaseGPD.Value != 0)
                    {
                        НеПревышающие.Add(new XElement(pfr + "ПоГПД", Utils.decToStr(item.BaseGPD)));
                        flag_ = true;
                        flag_2 = true;
                    }
                    if (flag_2)
                        Выплаты.Add(НеПревышающие);

                    flag_2 = false;
                    XElement Превышающие = new XElement(pfr + "Превышающие");
                    if (item.SumPrevBaseALL.HasValue && item.SumPrevBaseALL.Value != 0)
                    {
                        Превышающие.Add(new XElement(pfr + "Всего", Utils.decToStr(item.SumPrevBaseALL)));
                        flag_ = true;
                        flag_2 = true;
                    }
                    if (item.SumPrevBaseGPD.HasValue && item.SumPrevBaseGPD.Value != 0)
                    {
                        Превышающие.Add(new XElement(pfr + "ПоГПД", Utils.decToStr(item.SumPrevBaseGPD)));
                        flag_ = true;
                        flag_2 = true;
                    }
                    if (flag_2)
                        Выплаты.Add(Превышающие);

                    if (flag_)
                    {
                        Суммы.Add(Выплаты);
                        flag_3 = true;
                    }
                    #endregion


                    XElement ДоначисленоСВ = new XElement(pfr + "ДоначисленоСВ");
                    flag_ = false;

                    if (item.SumFeeBefore2001Insurer.HasValue && item.SumFeeBefore2001Insurer.Value != 0)
                    {
                        ДоначисленоСВ.Add(new XElement(pfr + "СВстрахователя", Utils.decToStr(item.SumFeeBefore2001Insurer)));
                        flag_ = true;
                    }
                    if (item.SumFeeBefore2001Staff.HasValue && item.SumFeeBefore2001Staff.Value != 0)
                    {

                        ДоначисленоСВ.Add(new XElement(pfr + "СВизЗаработка", Utils.decToStr(item.SumFeeBefore2001Staff)));
                        flag_ = true;
                    }
                    if (item.SumFeeAfter2001STRAH.HasValue && item.SumFeeAfter2001STRAH.Value != 0)
                    {

                        ДоначисленоСВ.Add(new XElement(pfr + "Страховая", Utils.decToStr(item.SumFeeAfter2001STRAH)));
                        flag_ = true;
                    }
                    if (item.SumFeeAfter2001NAKOP.HasValue && item.SumFeeAfter2001NAKOP.Value != 0)
                    {

                        ДоначисленоСВ.Add(new XElement(pfr + "Накопительная", Utils.decToStr(item.SumFeeAfter2001NAKOP)));
                        flag_ = true;
                    }
                    if (item.SumFeeTarSV.HasValue && item.SumFeeTarSV.Value != 0)
                    {

                        ДоначисленоСВ.Add(new XElement(pfr + "СВпоТарифу", Utils.decToStr(item.SumFeeTarSV)));
                        flag_ = true;
                    }


                    if (flag_)
                    {
                        Суммы.Add(ДоначисленоСВ);
                        flag_3 = true;
                    }


                    XElement Уплата = new XElement(pfr + "Уплата");
                    flag_ = false;

                    if (item.SumPaySTRAH.HasValue && item.SumPaySTRAH.Value != 0)
                    {
                        Уплата.Add(new XElement(pfr + "Страховая", Utils.decToStr(item.SumPaySTRAH)));
                        flag_ = true;
                    }
                    if (item.SumPayNAKOP.HasValue && item.SumPayNAKOP.Value != 0)
                    {

                        Уплата.Add(new XElement(pfr + "Накопительная", Utils.decToStr(item.SumPayNAKOP)));
                        flag_ = true;
                    }

                    if (flag_)
                    {
                        Суммы.Add(Уплата);
                        flag_3 = true;
                    }

                    if (flag_3)
                        СЗВ.Add(Суммы);
                }

            }




            foreach (var item in szv.FormsSZV_KORR_5_2017.ToList())
            {
                string code = item.SpecOcenkaUslTrudaID.HasValue ? item.SpecOcenkaUslTruda.Code : "";

                XElement ВыплатыДТ = new XElement(pfr + "ВыплатыДТ",
                                      new XElement(pfr + "Месяц", item.Month.HasValue ? (item.Month.Value <= 12 ? MonthesList[item.Month.Value - 1] : "") : ""),
                                      new XElement(pfr + "КодСОУТ", code));

                if (item.s_0.HasValue && item.s_0.Value != 0)
                {
                    ВыплатыДТ.Add(new XElement(ИС2 + "ДопТарифП1", Utils.decToStr(item.s_0)));
                }
                if (item.s_1.HasValue && item.s_1.Value != 0)
                {
                    ВыплатыДТ.Add(new XElement(ИС2 + "ДопТарифП2_18", Utils.decToStr(item.s_1)));
                }


                СЗВ.Add(ВыплатыДТ);
            }





            var staj_osn_list = db_temp.StajOsn.Where(x => x.FormsSZV_KORR_2017_ID == szv.ID).OrderBy(x => x.Number.Value).ToList();

            var tt = staj_osn_list.Select(x => x.ID).ToList();
            var stajLgot_list = db_temp.StajLgot.Where(x => tt.Contains(x.StajOsnID)).ToList(); // db_temp
            int ii = 0;
            foreach (var staj_osn in staj_osn_list)
            {
                try
                {
                    ii++;
                    XElement СтажевыйПериод = createStajElement_2017(staj_osn, stajLgot_list, ii, pfr, УТ2, ИС2);
                    СЗВ.Add(СтажевыйПериод);
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении СЗВ-ИСХ произошла ошибка при обработке стажа.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));
                }

            }



            return СЗВ;
        }


        private XDocument generateXML_RSW2014(xmlInfo item)
        {
            int yearType = ((item.Year == 2014) || (item.Year == 2015 && item.Quarter == 3)) ? 2014 : 2015;

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            string xml = "";
            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "Единичный запрос"))));


            string xName = "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014";
            if (yearType == 2015)
                xName = "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2015";
            XElement РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014 = new XElement(xName,
                                                new XElement("НомерВпачке", 1),
                                                new XElement("РегистрационныйНомерПФР", Utils.ParseRegNum(ins.RegNum)),
                                                new XElement("НомерКорректировки", rsw.CorrectionNum.ToString().PadLeft(3, '0')),
                                                new XElement("КодОтчетногоПериода", rsw.Quarter.ToString()),
                                                new XElement("КалендарныйГод", rsw.Year.ToString()));

            if (rsw.WorkStop != 0)
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ПрекращениеДеятельности", "Л"));
            }

            if (rsw.CorrectionType != null)
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ТипКорректировки", rsw.CorrectionType.Value.ToString()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("НаименованиеОрганизации", !String.IsNullOrEmpty(ins.Name) ? ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper() : !String.IsNullOrEmpty(ins.NameShort) ? ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper() : ""),
                    new XElement("ИННсимвольный", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
                if (!String.IsNullOrEmpty(ins.KPP))
                {
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                }
            }
            else // если физ. лицо
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ФИОфизическогоЛица"));

                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОфизическогоЛица").Add(new XElement("Фамилия", ins.LastName.ToUpper()));
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОфизическогоЛица").Add(new XElement("Имя", ins.FirstName.ToUpper()));
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОфизическогоЛица").Add(new XElement("Отчество", ins.MiddleName.ToUpper()));
                }
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ИННсимвольный", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
            }

            if (ins.OKWED != null)
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КодПоОКВЭД", !String.IsNullOrEmpty(ins.OKWED.ToString()) ? ins.OKWED.ToString() : ""));
            }

            if (ins.PhoneContact != null && !String.IsNullOrEmpty(ins.PhoneContact))
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("Телефон", ins.PhoneContact.ToString().Substring(0, ins.PhoneContact.ToString().Length > 14 ? 14 : ins.PhoneContact.ToString().Length)));
            }

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КоличествоЗЛ", rsw.CountEmployers.ToString()));
            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("СреднесписочнаяЧисленность", rsw.CountAverageEmployers.ToString()));

            int cntPages = 2;

            cntPages = cntPages + db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum).Count();
            cntPages = cntPages + db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum).Count();

            if (db.FormsRSW2014_1_Razd_2_5_1.Any(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum) || db.FormsRSW2014_1_Razd_2_5_2.Any(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
                cntPages++;
            if (db.FormsRSW2014_1_Razd_4.Any(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
                cntPages++;
            if (db.FormsRSW2014_1_Razd_5.Any(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
                cntPages++;
            if ((rsw.ExistPart_2_2 != null && rsw.ExistPart_2_2.Value == 1) || (rsw.ExistPart_2_3 != null && rsw.ExistPart_2_3.Value == 1))
                cntPages++;
            if ((rsw.ExistPart_3_1 != null && rsw.ExistPart_3_1.Value == 1) || (rsw.ExistPart_3_2 != null && rsw.ExistPart_3_2.Value == 1))
                cntPages++;
            if ((rsw.ExistPart_3_4 != null && rsw.ExistPart_3_4.Value == 1) || (rsw.ExistPart_3_4 != null && rsw.ExistPart_3_4.Value == 1))
                cntPages++;
            if ((rsw.ExistPart_3_5 != null && rsw.ExistPart_3_5.Value == 1) || (rsw.ExistPart_3_6 != null && rsw.ExistPart_3_6.Value == 1))
                cntPages++;

            cntPages = cntPages + (int)rsw.CountEmployers.Value * 2;

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КоличествоСтраниц", cntPages.ToString()));

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КоличествоЛистовПриложения", rsw.CountConfirmDoc.ToString()));

            //Для РСВ-1 2015
            //Приобретение/утрата права на применение пониженного тарифа. Заполняется при представлении Расчета за отчетный период, в котором приобретено или утрачено право на применение пониженного тарифа. В случае приобретения права на применение пониженного тарифа в поле проставляется буква «П», в случае утраты права на применение проставляется буква «У»
            /*          if (yearType == 2015 && !String.IsNullOrEmpty(rsw.ReducedRate))
                      {
                          РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ПравоНаПониженныйТариф", rsw.ReducedRate));
                      }
                      */
            // Разделы 1 - 5

            #region Раздел1РасчетПоНачисленнымУплаченным2014

            xName = "Раздел1РасчетПоНачисленнымУплаченным2014";
            //       if (yearType == 2015)
            //           xName = "Раздел1РасчетПоНачисленнымУплаченным";
            XElement Раздел1РасчетПоНачисленнымУплаченным2014 = new XElement(xName,
                                                                    new XElement(yearType != 2015 ? "ОстатокЗадолженностиНаНачалоРасчетногоПериода2014" : "ОстатокЗадолженностиНаНачалоРасчетногоПериода",
                                                                        new XElement("КодСтроки", "100"),
                                                                        new XElement("СтраховыеВзносыОПС", rsw.s_100_0.HasValue ? Utils.decToStr(rsw.s_100_0.Value) : "0.00"),
                                                                        new XElement("ОПСстраховаяЧасть", rsw.s_100_1.HasValue ? Utils.decToStr(rsw.s_100_1.Value) : "0.00"),
                                                                        new XElement("ОПСнакопительнаяЧасть", rsw.s_100_2.HasValue ? Utils.decToStr(rsw.s_100_2.Value) : "0.00"),
                                                                        new XElement("ВзносыПоДопТарифу1", rsw.s_100_3.HasValue ? Utils.decToStr(rsw.s_100_3.Value) : "0.00"),
                                                                        new XElement("ВзносыПоДопТарифу2_18", rsw.s_100_4.HasValue ? Utils.decToStr(rsw.s_100_4.Value) : "0.00"),
                                                                        new XElement("СтраховыеВзносыОМС", rsw.s_100_5.HasValue ? Utils.decToStr(rsw.s_100_5.Value) : "0.00")));

            xName = "НачисленоСначалаРасчетногоПериода2014";
            if (yearType == 2015)
                xName = "НачисленоСначалаРасчетногоПериода";
            XElement НачисленоСначалаРасчетногоПериода2014 = new XElement(xName,
                                                                new XElement(yearType != 2015 ? "ВсегоСначалаРасчетногоПериода2014" : "ВсегоСначалаРасчетногоПериода",
                                                                    new XElement("КодСтроки", "110"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_110_0.HasValue ? Utils.decToStr(rsw.s_110_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_110_3.HasValue ? Utils.decToStr(rsw.s_110_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_110_4.HasValue ? Utils.decToStr(rsw.s_110_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_110_5.HasValue ? Utils.decToStr(rsw.s_110_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца1с2014" : "ПоследниеТриМесяца1",
                                                                    new XElement("КодСтроки", "111"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_111_0.HasValue ? Utils.decToStr(rsw.s_111_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_111_3.HasValue ? Utils.decToStr(rsw.s_111_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_111_4.HasValue ? Utils.decToStr(rsw.s_111_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_111_5.HasValue ? Utils.decToStr(rsw.s_111_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца2с2014" : "ПоследниеТриМесяца2",
                                                                    new XElement("КодСтроки", "112"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_112_0.HasValue ? Utils.decToStr(rsw.s_112_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_112_3.HasValue ? Utils.decToStr(rsw.s_112_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_112_4.HasValue ? Utils.decToStr(rsw.s_112_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_112_5.HasValue ? Utils.decToStr(rsw.s_112_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца3с2014" : "ПоследниеТриМесяца3",
                                                                    new XElement("КодСтроки", "113"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_113_0.HasValue ? Utils.decToStr(rsw.s_113_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_113_3.HasValue ? Utils.decToStr(rsw.s_113_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_113_4.HasValue ? Utils.decToStr(rsw.s_113_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_113_5.HasValue ? Utils.decToStr(rsw.s_113_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяцаИтого2014" : "ПоследниеТриМесяцаИтого",
                                                                    new XElement("КодСтроки", "114"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_114_0.HasValue ? Utils.decToStr(rsw.s_114_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_114_3.HasValue ? Utils.decToStr(rsw.s_114_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_114_4.HasValue ? Utils.decToStr(rsw.s_114_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_114_5.HasValue ? Utils.decToStr(rsw.s_114_5.Value) : "0.00")));

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(НачисленоСначалаРасчетногоПериода2014);

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(new XElement(yearType != 2015 ? "ДоначисленоСначалаРасчетногоПериода2014всего" : "ДоначисленоСначалаРасчетногоПериодаВсего",
                                                             new XElement("КодСтроки", "120"),
                                                             new XElement("СтраховыеВзносыОПС", rsw.s_120_0.HasValue ? Utils.decToStr(rsw.s_120_0.Value) : "0.00"),
                                                             new XElement("ОПСстраховаяЧасть", rsw.s_120_1.HasValue ? Utils.decToStr(rsw.s_120_1.Value) : "0.00"),
                                                             new XElement("ОПСнакопительнаяЧасть", rsw.s_120_2.HasValue ? Utils.decToStr(rsw.s_120_2.Value) : "0.00"),
                                                             new XElement("ВзносыПоДопТарифу1", rsw.s_120_3.HasValue ? Utils.decToStr(rsw.s_120_3.Value) : "0.00"),
                                                             new XElement("ВзносыПоДопТарифу2_18", rsw.s_120_4.HasValue ? Utils.decToStr(rsw.s_120_4.Value) : "0.00"),
                                                             new XElement("СтраховыеВзносыОМС", rsw.s_120_5.HasValue ? Utils.decToStr(rsw.s_120_5.Value) : "0.00")));

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(new XElement(yearType != 2015 ? "ДоначисленоСначалаРасчетногоПериода2014превышающие" : "ДоначисленоСначалаРасчетногоПериодаПревышающие",
                                                             new XElement("КодСтроки", "121"),
                                                             new XElement("СтраховыеВзносыОПС", rsw.s_121_0.HasValue ? Utils.decToStr(rsw.s_121_0.Value) : "0.00"),
                                                             new XElement("ОПСстраховаяЧасть", rsw.s_121_1.HasValue ? Utils.decToStr(rsw.s_121_1.Value) : "0.00")));

            /*      if (yearType == 2015)
                  {
                      XElement ДоначисленоВпоследниеТриМесяца = new XElement("ДоначисленоВпоследниеТриМесяца");
                      ДоначисленоВпоследниеТриМесяца.Add(new XElement("ПоследниеТриМесяцаВсего",
                                                       new XElement("КодСтроки", "122"),
                                                       new XElement("СтраховыеВзносыОПС", rsw.s_122_0.HasValue ? Utils.decToStr(rsw.s_122_0.Value) : "0.00"),
                                                       new XElement("ОПСстраховаяЧасть", rsw.s_122_1.HasValue ? Utils.decToStr(rsw.s_122_1.Value) : "0.00"),
                                                       new XElement("ОПСнакопительнаяЧасть", rsw.s_122_2.HasValue ? Utils.decToStr(rsw.s_122_2.Value) : "0.00"),
                                                       new XElement("ВзносыПоДопТарифу1", rsw.s_122_3.HasValue ? Utils.decToStr(rsw.s_122_3.Value) : "0.00"),
                                                       new XElement("ВзносыПоДопТарифу2_18", rsw.s_122_4.HasValue ? Utils.decToStr(rsw.s_122_4.Value) : "0.00"),
                                                       new XElement("СтраховыеВзносыОМС", rsw.s_122_5.HasValue ? Utils.decToStr(rsw.s_122_5.Value) : "0.00")));

                      ДоначисленоВпоследниеТриМесяца.Add(new XElement("ПоследниеТриМесяцаПревышающие",
                                                                       new XElement("КодСтроки", "123"),
                                                                       new XElement("СтраховыеВзносыОПС", rsw.s_123_0.HasValue ? Utils.decToStr(rsw.s_123_0.Value) : "0.00"),
                                                                       new XElement("ОПСстраховаяЧасть", rsw.s_123_1.HasValue ? Utils.decToStr(rsw.s_123_1.Value) : "0.00")));


                      Раздел1РасчетПоНачисленнымУплаченным2014.Add(ДоначисленоВпоследниеТриМесяца);
                  }
      */



            Раздел1РасчетПоНачисленнымУплаченным2014.Add(new XElement(yearType != 2015 ? "ВсегоКуплате2014" : "ВсегоКуплате",
                                                              new XElement("КодСтроки", "130"),
                                                              new XElement("СтраховыеВзносыОПС", rsw.s_130_0.HasValue ? Utils.decToStr(rsw.s_130_0.Value) : "0.00"),
                                                              new XElement("ОПСстраховаяЧасть", rsw.s_130_1.HasValue ? Utils.decToStr(rsw.s_130_1.Value) : "0.00"),
                                                              new XElement("ОПСнакопительнаяЧасть", rsw.s_130_2.HasValue ? Utils.decToStr(rsw.s_130_2.Value) : "0.00"),
                                                              new XElement("ВзносыПоДопТарифу1", rsw.s_130_3.HasValue ? Utils.decToStr(rsw.s_130_3.Value) : "0.00"),
                                                              new XElement("ВзносыПоДопТарифу2_18", rsw.s_130_4.HasValue ? Utils.decToStr(rsw.s_130_4.Value) : "0.00"),
                                                              new XElement("СтраховыеВзносыОМС", rsw.s_130_5.HasValue ? Utils.decToStr(rsw.s_130_5.Value) : "0.00")));

            XElement УплаченоСначалаРасчетногоПериода2014 = new XElement(yearType != 2015 ? "УплаченоСначалаРасчетногоПериода2014" : "УплаченоСначалаРасчетногоПериода",
                                                                new XElement(yearType != 2015 ? "ВсегоСначалаРасчетногоПериода2014" : "ВсегоСначалаРасчетногоПериода",
                                                                    new XElement("КодСтроки", "140"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_140_0.HasValue ? Utils.decToStr(rsw.s_140_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_140_1.HasValue ? Utils.decToStr(rsw.s_140_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_140_2.HasValue ? Utils.decToStr(rsw.s_140_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_140_3.HasValue ? Utils.decToStr(rsw.s_140_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_140_4.HasValue ? Utils.decToStr(rsw.s_140_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_140_5.HasValue ? Utils.decToStr(rsw.s_140_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца1с2014" : "ПоследниеТриМесяца1",
                                                                    new XElement("КодСтроки", "141"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_141_0.HasValue ? Utils.decToStr(rsw.s_141_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_141_1.HasValue ? Utils.decToStr(rsw.s_141_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_141_2.HasValue ? Utils.decToStr(rsw.s_141_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_141_3.HasValue ? Utils.decToStr(rsw.s_141_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_141_4.HasValue ? Utils.decToStr(rsw.s_141_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_141_5.HasValue ? Utils.decToStr(rsw.s_141_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца2с2014" : "ПоследниеТриМесяца2",
                                                                    new XElement("КодСтроки", "142"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_142_0.HasValue ? Utils.decToStr(rsw.s_142_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_142_1.HasValue ? Utils.decToStr(rsw.s_142_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_142_2.HasValue ? Utils.decToStr(rsw.s_142_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_142_3.HasValue ? Utils.decToStr(rsw.s_142_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_142_4.HasValue ? Utils.decToStr(rsw.s_142_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_142_5.HasValue ? Utils.decToStr(rsw.s_142_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца3с2014" : "ПоследниеТриМесяца3",
                                                                    new XElement("КодСтроки", "143"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_143_0.HasValue ? Utils.decToStr(rsw.s_143_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_143_1.HasValue ? Utils.decToStr(rsw.s_143_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_143_2.HasValue ? Utils.decToStr(rsw.s_143_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_143_3.HasValue ? Utils.decToStr(rsw.s_143_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_143_4.HasValue ? Utils.decToStr(rsw.s_143_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_143_5.HasValue ? Utils.decToStr(rsw.s_143_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяцаИтого2014" : "ПоследниеТриМесяцаИтого",
                                                                    new XElement("КодСтроки", "144"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_144_0.HasValue ? Utils.decToStr(rsw.s_144_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_144_1.HasValue ? Utils.decToStr(rsw.s_144_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_144_2.HasValue ? Utils.decToStr(rsw.s_144_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_144_3.HasValue ? Utils.decToStr(rsw.s_144_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_144_4.HasValue ? Utils.decToStr(rsw.s_144_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_144_5.HasValue ? Utils.decToStr(rsw.s_144_5.Value) : "0.00")));

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(УплаченоСначалаРасчетногоПериода2014);

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(new XElement(yearType != 2015 ? "ОстатокЗадолженностиНаКонецРасчетногоПериода2014" : "ОстатокЗадолженностиНаКонецРасчетногоПериода",
                                                              new XElement("КодСтроки", "150"),
                                                              new XElement("СтраховыеВзносыОПС", rsw.s_150_0.HasValue ? Utils.decToStr(rsw.s_150_0.Value) : "0.00"),
                                                              new XElement("ОПСстраховаяЧасть", rsw.s_150_1.HasValue ? Utils.decToStr(rsw.s_150_1.Value) : "0.00"),
                                                              new XElement("ОПСнакопительнаяЧасть", rsw.s_150_2.HasValue ? Utils.decToStr(rsw.s_150_2.Value) : "0.00"),
                                                              new XElement("ВзносыПоДопТарифу1", rsw.s_150_3.HasValue ? Utils.decToStr(rsw.s_150_3.Value) : "0.00"),
                                                              new XElement("ВзносыПоДопТарифу2_18", rsw.s_150_4.HasValue ? Utils.decToStr(rsw.s_150_4.Value) : "0.00"),
                                                              new XElement("СтраховыеВзносыОМС", rsw.s_150_5.HasValue ? Utils.decToStr(rsw.s_150_5.Value) : "0.00")));


            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел1РасчетПоНачисленнымУплаченным2014);

            #endregion

            #region Раздел2РасчетПоТарифуИдопТарифу

            XElement Раздел2РасчетПоТарифуИдопТарифу = new XElement("Раздел2РасчетПоТарифуИдопТарифу");

            #region Раздел_2_1

            var RSW_2_1_List = db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.TariffCode.Code).ToList();

            foreach (var rsw21 in RSW_2_1_List)
            {
                XElement Раздел_2_1 = new XElement("Раздел_2_1",
                                          new XElement("КодТарифа", rsw21.TariffCode.Code.ToUpper()));

                XElement НаОбязательноеПенсионноеСтрахование2014 = new XElement((yearType != 2015 ? "НаОбязательноеПенсионноеСтрахование2014" : "НаОбязательноеПенсионноеСтрахование"), "");
                НаОбязательноеПенсионноеСтрахование2014.Add(new XElement("СуммаВыплатИвознагражденийОПС",
                                                                new XElement("КодСтроки", "200"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_200_0.HasValue ? Utils.decToStr(rsw21.s_200_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_200_1.HasValue ? Utils.decToStr(rsw21.s_200_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_200_2.HasValue ? Utils.decToStr(rsw21.s_200_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_200_3.HasValue ? Utils.decToStr(rsw21.s_200_3.Value) : "0.00"))),
                                                            new XElement("НеПодлежащиеОбложениюОПС",
                                                                new XElement("КодСтроки", "201"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_201_0.HasValue ? Utils.decToStr(rsw21.s_201_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_201_1.HasValue ? Utils.decToStr(rsw21.s_201_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_201_2.HasValue ? Utils.decToStr(rsw21.s_201_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_201_3.HasValue ? Utils.decToStr(rsw21.s_201_3.Value) : "0.00"))),
                                                            new XElement("СуммаРасходовПринимаемыхКвычетуОПС",
                                                                new XElement("КодСтроки", "202"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_202_0.HasValue ? Utils.decToStr(rsw21.s_202_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_202_1.HasValue ? Utils.decToStr(rsw21.s_202_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_202_2.HasValue ? Utils.decToStr(rsw21.s_202_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_202_3.HasValue ? Utils.decToStr(rsw21.s_202_3.Value) : "0.00"))),
                                                            new XElement("ПревышающиеПредельнуюВеличинуБазыОПС",
                                                                new XElement("КодСтроки", "203"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_203_0.HasValue ? Utils.decToStr(rsw21.s_203_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_203_1.HasValue ? Utils.decToStr(rsw21.s_203_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_203_2.HasValue ? Utils.decToStr(rsw21.s_203_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_203_3.HasValue ? Utils.decToStr(rsw21.s_203_3.Value) : "0.00"))),
                                                            new XElement("БазаДляНачисленияСтраховыхВзносовНаОПС",
                                                                new XElement("КодСтроки", "204"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_204_0.HasValue ? Utils.decToStr(rsw21.s_204_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_204_1.HasValue ? Utils.decToStr(rsw21.s_204_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_204_2.HasValue ? Utils.decToStr(rsw21.s_204_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_204_3.HasValue ? Utils.decToStr(rsw21.s_204_3.Value) : "0.00"))),
                                                            new XElement("НачисленоНаОПСсСуммНеПревышающих",
                                                                new XElement("КодСтроки", "205"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_205_0.HasValue ? Utils.decToStr(rsw21.s_205_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_205_1.HasValue ? Utils.decToStr(rsw21.s_205_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_205_2.HasValue ? Utils.decToStr(rsw21.s_205_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_205_3.HasValue ? Utils.decToStr(rsw21.s_205_3.Value) : "0.00"))),
                                                            new XElement("НачисленоНаОПСсСуммПревышающих",
                                                                new XElement("КодСтроки", "206"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_206_0.HasValue ? Utils.decToStr(rsw21.s_206_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_206_1.HasValue ? Utils.decToStr(rsw21.s_206_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_206_2.HasValue ? Utils.decToStr(rsw21.s_206_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_206_3.HasValue ? Utils.decToStr(rsw21.s_206_3.Value) : "0.00"))),
                                                            new XElement("КоличествоФЛвсего",
                                                                new XElement("КодСтроки", "207"),
                                                                new XElement("КоличествоЗЛ_Всего", rsw21.s_207_0.HasValue ? rsw21.s_207_0.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_1месяц", rsw21.s_207_1.HasValue ? rsw21.s_207_1.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_2месяц", rsw21.s_207_2.HasValue ? rsw21.s_207_2.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_3месяц", rsw21.s_207_3.HasValue ? rsw21.s_207_3.Value.ToString() : "0")),
                                                            new XElement("КоличествоФЛпоКоторымБазаПревысилаПредел",
                                                                new XElement("КодСтроки", "208"),
                                                                new XElement("КоличествоЗЛ_Всего", rsw21.s_208_0.HasValue ? rsw21.s_208_0.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_1месяц", rsw21.s_208_1.HasValue ? rsw21.s_208_1.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_2месяц", rsw21.s_208_2.HasValue ? rsw21.s_208_2.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_3месяц", rsw21.s_208_3.HasValue ? rsw21.s_208_3.Value.ToString() : "0")));

                Раздел_2_1.Add(НаОбязательноеПенсионноеСтрахование2014);

                XElement НаОбязательноеМедицинскоеСтрахование = new XElement("НаОбязательноеМедицинскоеСтрахование");
                НаОбязательноеМедицинскоеСтрахование.Add(new XElement("СуммаВыплатИвознаграждений",
                                                                new XElement("КодСтроки", "210"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_210_0.HasValue ? Utils.decToStr(rsw21.s_210_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_210_1.HasValue ? Utils.decToStr(rsw21.s_210_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_210_2.HasValue ? Utils.decToStr(rsw21.s_210_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_210_3.HasValue ? Utils.decToStr(rsw21.s_210_3.Value) : "0.00"))),
                                                            new XElement("НеПодлежащиеОбложению",
                                                                new XElement("КодСтроки", "211"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_211_0.HasValue ? Utils.decToStr(rsw21.s_211_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_211_1.HasValue ? Utils.decToStr(rsw21.s_211_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_211_2.HasValue ? Utils.decToStr(rsw21.s_211_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_211_3.HasValue ? Utils.decToStr(rsw21.s_211_3.Value) : "0.00"))),
                                                            new XElement("СуммаРасходовПринимаемыхКвычету",
                                                                new XElement("КодСтроки", "212"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_212_0.HasValue ? Utils.decToStr(rsw21.s_212_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_212_1.HasValue ? Utils.decToStr(rsw21.s_212_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_212_2.HasValue ? Utils.decToStr(rsw21.s_212_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_212_3.HasValue ? Utils.decToStr(rsw21.s_212_3.Value) : "0.00"))));

                if (yearType == 2014 || yearType == 2012)
                {

                    НаОбязательноеМедицинскоеСтрахование.Add(new XElement("ПревышающиеПредельнуюВеличинуБазы",
                                                                new XElement("КодСтроки", "213"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_213_0.HasValue ? Utils.decToStr(rsw21.s_213_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_213_1.HasValue ? Utils.decToStr(rsw21.s_213_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_213_2.HasValue ? Utils.decToStr(rsw21.s_213_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_213_3.HasValue ? Utils.decToStr(rsw21.s_213_3.Value) : "0.00"))),
                                                             new XElement("БазаДляНачисленияСтраховыхВзносовНаОМС",
                                                                 new XElement("КодСтроки", "214"),
                                                                 new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_214_0.HasValue ? Utils.decToStr(rsw21.s_214_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_214_1.HasValue ? Utils.decToStr(rsw21.s_214_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_214_2.HasValue ? Utils.decToStr(rsw21.s_214_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_214_3.HasValue ? Utils.decToStr(rsw21.s_214_3.Value) : "0.00"))),
                                                             new XElement("НачисленоНаОМС",
                                                                 new XElement("КодСтроки", "215"),
                                                                 new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_215_0.HasValue ? Utils.decToStr(rsw21.s_215_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_215_1.HasValue ? Utils.decToStr(rsw21.s_215_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_215_2.HasValue ? Utils.decToStr(rsw21.s_215_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_215_3.HasValue ? Utils.decToStr(rsw21.s_215_3.Value) : "0.00"))));
                }
                if (yearType == 2015)
                {

                    НаОбязательноеМедицинскоеСтрахование.Add(new XElement("БазаДляНачисленияСтраховыхВзносовНаОМС",
                                                                 new XElement("КодСтроки", "213"),
                                                                    new XElement("РасчетСумм",
                                                                        new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_213_0.HasValue ? Utils.decToStr(rsw21.s_213_0.Value) : "0.00"),
                                                                        new XElement("СуммаПоследние1месяц", rsw21.s_213_1.HasValue ? Utils.decToStr(rsw21.s_213_1.Value) : "0.00"),
                                                                        new XElement("СуммаПоследние2месяц", rsw21.s_213_2.HasValue ? Utils.decToStr(rsw21.s_213_2.Value) : "0.00"),
                                                                        new XElement("СуммаПоследние3месяц", rsw21.s_213_3.HasValue ? Utils.decToStr(rsw21.s_213_3.Value) : "0.00"))),
                                                                 new XElement("НачисленоНаОМС",
                                                                     new XElement("КодСтроки", "214"),
                                                                     new XElement("РасчетСумм",
                                                                         new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_214_0.HasValue ? Utils.decToStr(rsw21.s_214_0.Value) : "0.00"),
                                                                         new XElement("СуммаПоследние1месяц", rsw21.s_214_1.HasValue ? Utils.decToStr(rsw21.s_214_1.Value) : "0.00"),
                                                                         new XElement("СуммаПоследние2месяц", rsw21.s_214_2.HasValue ? Utils.decToStr(rsw21.s_214_2.Value) : "0.00"),
                                                                         new XElement("СуммаПоследние3месяц", rsw21.s_214_3.HasValue ? Utils.decToStr(rsw21.s_214_3.Value) : "0.00"))),
                                                                new XElement("КоличествоФЛвсего",
                                                                    new XElement("КодСтроки", "215"),
                                                                    new XElement("КоличествоЗЛ_Всего", rsw21.s_215i_0.HasValue ? rsw21.s_215i_0.Value.ToString() : "0"),
                                                                    new XElement("КоличествоЗЛ_1месяц", rsw21.s_215i_1.HasValue ? rsw21.s_215i_1.Value.ToString() : "0"),
                                                                    new XElement("КоличествоЗЛ_2месяц", rsw21.s_215i_2.HasValue ? rsw21.s_215i_2.Value.ToString() : "0"),
                                                                    new XElement("КоличествоЗЛ_3месяц", rsw21.s_215i_3.HasValue ? rsw21.s_215i_3.Value.ToString() : "0")));
                }

                Раздел_2_1.Add(НаОбязательноеМедицинскоеСтрахование);

                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_1);
            }

            #endregion

            #region Раздел_2_2

            if (rsw.ExistPart_2_2.HasValue && rsw.ExistPart_2_2.Value == 1)
            {
                XElement Раздел_2_2 = new XElement("Раздел_2_2");

                Раздел_2_2.Add(new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                   new XElement("КодСтроки", "220"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_220_0.HasValue ? Utils.decToStr(rsw.s_220_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_220_1.HasValue ? Utils.decToStr(rsw.s_220_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_220_2.HasValue ? Utils.decToStr(rsw.s_220_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_220_3.HasValue ? Utils.decToStr(rsw.s_220_3.Value) : "0.00"))),
                               new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                   new XElement("КодСтроки", "221"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_221_0.HasValue ? Utils.decToStr(rsw.s_221_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_221_1.HasValue ? Utils.decToStr(rsw.s_221_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_221_2.HasValue ? Utils.decToStr(rsw.s_221_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_221_3.HasValue ? Utils.decToStr(rsw.s_221_3.Value) : "0.00"))),
                               new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                   new XElement("КодСтроки", "223"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_223_0.HasValue ? Utils.decToStr(rsw.s_223_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_223_1.HasValue ? Utils.decToStr(rsw.s_223_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_223_2.HasValue ? Utils.decToStr(rsw.s_223_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_223_3.HasValue ? Utils.decToStr(rsw.s_223_3.Value) : "0.00"))),
                               new XElement("НачисленоПоДопТарифу",
                                   new XElement("КодСтроки", "224"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_224_0.HasValue ? Utils.decToStr(rsw.s_224_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_224_1.HasValue ? Utils.decToStr(rsw.s_224_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_224_2.HasValue ? Utils.decToStr(rsw.s_224_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_224_3.HasValue ? Utils.decToStr(rsw.s_224_3.Value) : "0.00"))),
                               new XElement("КоличествоФЛпоДопТарифу",
                                   new XElement("КодСтроки", "225"),
                                   new XElement("КоличествоЗЛ_Всего", rsw.s_225_0.HasValue ? rsw.s_225_0.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_1месяц", rsw.s_225_1.HasValue ? rsw.s_225_1.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_2месяц", rsw.s_225_2.HasValue ? rsw.s_225_2.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_3месяц", rsw.s_225_3.HasValue ? rsw.s_225_3.Value.ToString() : "0")));

                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_2);
            }
            #endregion

            #region Раздел_2_3

            if (rsw.ExistPart_2_3.HasValue && rsw.ExistPart_2_3.Value == 1)
            {
                XElement Раздел_2_3 = new XElement("Раздел_2_3");

                Раздел_2_3.Add(new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                   new XElement("КодСтроки", "230"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_230_0.HasValue ? Utils.decToStr(rsw.s_230_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_230_1.HasValue ? Utils.decToStr(rsw.s_230_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_230_2.HasValue ? Utils.decToStr(rsw.s_230_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_230_3.HasValue ? Utils.decToStr(rsw.s_230_3.Value) : "0.00"))),
                               new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                   new XElement("КодСтроки", "231"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_231_0.HasValue ? Utils.decToStr(rsw.s_231_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_231_1.HasValue ? Utils.decToStr(rsw.s_231_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_231_2.HasValue ? Utils.decToStr(rsw.s_231_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_231_3.HasValue ? Utils.decToStr(rsw.s_231_3.Value) : "0.00"))),
                               new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                   new XElement("КодСтроки", "233"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_233_0.HasValue ? Utils.decToStr(rsw.s_233_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_233_1.HasValue ? Utils.decToStr(rsw.s_233_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_233_2.HasValue ? Utils.decToStr(rsw.s_233_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_233_3.HasValue ? Utils.decToStr(rsw.s_233_3.Value) : "0.00"))),
                               new XElement("НачисленоПоДопТарифу",
                                   new XElement("КодСтроки", "234"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_234_0.HasValue ? Utils.decToStr(rsw.s_234_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_234_1.HasValue ? Utils.decToStr(rsw.s_234_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_234_2.HasValue ? Utils.decToStr(rsw.s_234_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_234_3.HasValue ? Utils.decToStr(rsw.s_234_3.Value) : "0.00"))),
                               new XElement("КоличествоФЛпоДопТарифу",
                                   new XElement("КодСтроки", "235"),
                                   new XElement("КоличествоЗЛ_Всего", rsw.s_235_0.HasValue ? rsw.s_235_0.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_1месяц", rsw.s_235_1.HasValue ? rsw.s_235_1.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_2месяц", rsw.s_235_2.HasValue ? rsw.s_235_2.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_3месяц", rsw.s_235_3.HasValue ? rsw.s_235_3.Value.ToString() : "0")));

                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_3);
            }

            #endregion

            #region Раздел_2_4

            var RSW_2_4_List = db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.ID).ToList();

            foreach (var rsw24 in RSW_2_4_List)
            {
                string filledBase = "";
                switch (rsw24.FilledBase.Value)
                {
                    case 0: filledBase = "РЕЗУЛЬТАТЫ СПЕЦОЦЕНКИ";
                        break;
                    case 1: filledBase = "РЕЗУЛЬТАТЫ АТТЕСТАЦИИ РАБОЧИХ МЕСТ";
                        break;
                    case 2: filledBase = "РЕЗУЛЬТАТЫ СПЕЦОЦЕНКИ И РЕЗУЛЬТАТЫ АТТЕСТАЦИИ РАБОЧИХ МЕСТ";
                        break;
                }

                XElement Раздел_2_4 = new XElement("Раздел_2_4",
                                          new XElement("КодОснованияРасчетаПоДопТарифу", rsw24.CodeBase.Value),
                                          new XElement("ОснованиеЗаполненияРаздела2_4", filledBase));

                if ((rsw24.s_243_0.HasValue && rsw24.s_243_0.Value != 0) || (rsw24.s_243_1.HasValue && rsw24.s_243_1.Value != 0) || (rsw24.s_243_2.HasValue && rsw24.s_243_2.Value != 0) || (rsw24.s_243_3.HasValue && rsw24.s_243_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                       new XElement("КодСпециальнойОценкиУсловийТруда", "О4"),
                                       new XElement("РасчетНачисленныхПоДопТарифу",
                                           new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                               new XElement("КодСтроки", "240"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_240_0.HasValue ? Utils.decToStr(rsw24.s_240_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_240_1.HasValue ? Utils.decToStr(rsw24.s_240_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_240_2.HasValue ? Utils.decToStr(rsw24.s_240_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_240_3.HasValue ? Utils.decToStr(rsw24.s_240_3.Value) : "0.00"))),
                                           new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                               new XElement("КодСтроки", "241"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_241_0.HasValue ? Utils.decToStr(rsw24.s_241_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_241_1.HasValue ? Utils.decToStr(rsw24.s_241_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_241_2.HasValue ? Utils.decToStr(rsw24.s_241_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_241_3.HasValue ? Utils.decToStr(rsw24.s_241_3.Value) : "0.00"))),
                                           new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                               new XElement("КодСтроки", "243"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_243_0.HasValue ? Utils.decToStr(rsw24.s_243_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_243_1.HasValue ? Utils.decToStr(rsw24.s_243_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_243_2.HasValue ? Utils.decToStr(rsw24.s_243_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_243_3.HasValue ? Utils.decToStr(rsw24.s_243_3.Value) : "0.00"))),
                                           new XElement("НачисленоПоДопТарифу",
                                               new XElement("КодСтроки", "244"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_244_0.HasValue ? Utils.decToStr(rsw24.s_244_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_244_1.HasValue ? Utils.decToStr(rsw24.s_244_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_244_2.HasValue ? Utils.decToStr(rsw24.s_244_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_244_3.HasValue ? Utils.decToStr(rsw24.s_244_3.Value) : "0.00"))),
                                           new XElement("КоличествоФЛпоДопТарифу",
                                               new XElement("КодСтроки", "245"),
                                               new XElement("КоличествоЗЛ_Всего", rsw24.s_245_0.HasValue ? rsw24.s_245_0.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_1месяц", rsw24.s_245_1.HasValue ? rsw24.s_245_1.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_2месяц", rsw24.s_245_2.HasValue ? rsw24.s_245_2.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_3месяц", rsw24.s_245_3.HasValue ? rsw24.s_245_3.Value.ToString() : "0")))));
                }

                if ((rsw24.s_249_0.HasValue && rsw24.s_249_0.Value != 0) || (rsw24.s_249_1.HasValue && rsw24.s_249_1.Value != 0) || (rsw24.s_249_2.HasValue && rsw24.s_249_2.Value != 0) || (rsw24.s_249_3.HasValue && rsw24.s_249_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                       new XElement("КодСпециальнойОценкиУсловийТруда", "В3.4"),
                                       new XElement("РасчетНачисленныхПоДопТарифу",
                                           new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                               new XElement("КодСтроки", "246"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_246_0.HasValue ? Utils.decToStr(rsw24.s_246_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_246_1.HasValue ? Utils.decToStr(rsw24.s_246_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_246_2.HasValue ? Utils.decToStr(rsw24.s_246_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_246_3.HasValue ? Utils.decToStr(rsw24.s_246_3.Value) : "0.00"))),
                                           new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                               new XElement("КодСтроки", "247"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_247_0.HasValue ? Utils.decToStr(rsw24.s_247_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_247_1.HasValue ? Utils.decToStr(rsw24.s_247_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_247_2.HasValue ? Utils.decToStr(rsw24.s_247_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_247_3.HasValue ? Utils.decToStr(rsw24.s_247_3.Value) : "0.00"))),
                                           new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                               new XElement("КодСтроки", "249"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_249_0.HasValue ? Utils.decToStr(rsw24.s_249_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_249_1.HasValue ? Utils.decToStr(rsw24.s_249_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_249_2.HasValue ? Utils.decToStr(rsw24.s_249_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_249_3.HasValue ? Utils.decToStr(rsw24.s_249_3.Value) : "0.00"))),
                                           new XElement("НачисленоПоДопТарифу",
                                               new XElement("КодСтроки", "250"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_250_0.HasValue ? Utils.decToStr(rsw24.s_250_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_250_1.HasValue ? Utils.decToStr(rsw24.s_250_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_250_2.HasValue ? Utils.decToStr(rsw24.s_250_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_250_3.HasValue ? Utils.decToStr(rsw24.s_250_3.Value) : "0.00"))),
                                           new XElement("КоличествоФЛпоДопТарифу",
                                               new XElement("КодСтроки", "251"),
                                               new XElement("КоличествоЗЛ_Всего", rsw24.s_251_0.HasValue ? rsw24.s_251_0.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_1месяц", rsw24.s_251_1.HasValue ? rsw24.s_251_1.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_2месяц", rsw24.s_251_2.HasValue ? rsw24.s_251_2.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_3месяц", rsw24.s_251_3.HasValue ? rsw24.s_251_3.Value.ToString() : "0")))));
                }

                if ((rsw24.s_255_0.HasValue && rsw24.s_255_0.Value != 0) || (rsw24.s_255_1.HasValue && rsw24.s_255_1.Value != 0) || (rsw24.s_255_2.HasValue && rsw24.s_255_2.Value != 0) || (rsw24.s_255_3.HasValue && rsw24.s_255_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                       new XElement("КодСпециальнойОценкиУсловийТруда", "В3.3"),
                                       new XElement("РасчетНачисленныхПоДопТарифу",
                                           new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                               new XElement("КодСтроки", "252"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_252_0.HasValue ? Utils.decToStr(rsw24.s_252_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_252_1.HasValue ? Utils.decToStr(rsw24.s_252_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_252_2.HasValue ? Utils.decToStr(rsw24.s_252_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_252_3.HasValue ? Utils.decToStr(rsw24.s_252_3.Value) : "0.00"))),
                                           new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                               new XElement("КодСтроки", "253"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_253_0.HasValue ? Utils.decToStr(rsw24.s_253_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_253_1.HasValue ? Utils.decToStr(rsw24.s_253_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_253_2.HasValue ? Utils.decToStr(rsw24.s_253_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_253_3.HasValue ? Utils.decToStr(rsw24.s_253_3.Value) : "0.00"))),
                                           new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                               new XElement("КодСтроки", "255"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_255_0.HasValue ? Utils.decToStr(rsw24.s_255_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_255_1.HasValue ? Utils.decToStr(rsw24.s_255_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_255_2.HasValue ? Utils.decToStr(rsw24.s_255_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_255_3.HasValue ? Utils.decToStr(rsw24.s_255_3.Value) : "0.00"))),
                                           new XElement("НачисленоПоДопТарифу",
                                               new XElement("КодСтроки", "256"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_256_0.HasValue ? Utils.decToStr(rsw24.s_256_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_256_1.HasValue ? Utils.decToStr(rsw24.s_256_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_256_2.HasValue ? Utils.decToStr(rsw24.s_256_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_256_3.HasValue ? Utils.decToStr(rsw24.s_256_3.Value) : "0.00"))),
                                           new XElement("КоличествоФЛпоДопТарифу",
                                               new XElement("КодСтроки", "257"),
                                               new XElement("КоличествоЗЛ_Всего", rsw24.s_257_0.HasValue ? rsw24.s_257_0.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_1месяц", rsw24.s_257_1.HasValue ? rsw24.s_257_1.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_2месяц", rsw24.s_257_2.HasValue ? rsw24.s_257_2.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_3месяц", rsw24.s_257_3.HasValue ? rsw24.s_257_3.Value.ToString() : "0")))));
                }

                if ((rsw24.s_261_0.HasValue && rsw24.s_261_0.Value != 0) || (rsw24.s_261_1.HasValue && rsw24.s_261_1.Value != 0) || (rsw24.s_261_2.HasValue && rsw24.s_261_2.Value != 0) || (rsw24.s_261_3.HasValue && rsw24.s_261_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                        new XElement("КодСпециальнойОценкиУсловийТруда", "В3.2"),
                                        new XElement("РасчетНачисленныхПоДопТарифу",
                                            new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                                new XElement("КодСтроки", "258"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_258_0.HasValue ? Utils.decToStr(rsw24.s_258_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_258_1.HasValue ? Utils.decToStr(rsw24.s_258_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_258_2.HasValue ? Utils.decToStr(rsw24.s_258_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_258_3.HasValue ? Utils.decToStr(rsw24.s_258_3.Value) : "0.00"))),
                                            new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                                new XElement("КодСтроки", "259"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_259_0.HasValue ? Utils.decToStr(rsw24.s_259_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_259_1.HasValue ? Utils.decToStr(rsw24.s_259_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_259_2.HasValue ? Utils.decToStr(rsw24.s_259_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_259_3.HasValue ? Utils.decToStr(rsw24.s_259_3.Value) : "0.00"))),
                                            new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                                new XElement("КодСтроки", "261"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_261_0.HasValue ? Utils.decToStr(rsw24.s_261_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_261_1.HasValue ? Utils.decToStr(rsw24.s_261_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_261_2.HasValue ? Utils.decToStr(rsw24.s_261_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_261_3.HasValue ? Utils.decToStr(rsw24.s_261_3.Value) : "0.00"))),
                                            new XElement("НачисленоПоДопТарифу",
                                                new XElement("КодСтроки", "262"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_262_0.HasValue ? Utils.decToStr(rsw24.s_262_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_262_1.HasValue ? Utils.decToStr(rsw24.s_262_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_262_2.HasValue ? Utils.decToStr(rsw24.s_262_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_262_3.HasValue ? Utils.decToStr(rsw24.s_262_3.Value) : "0.00"))),
                                            new XElement("КоличествоФЛпоДопТарифу",
                                                new XElement("КодСтроки", "263"),
                                                new XElement("КоличествоЗЛ_Всего", rsw24.s_263_0.HasValue ? rsw24.s_263_0.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_1месяц", rsw24.s_263_1.HasValue ? rsw24.s_263_1.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_2месяц", rsw24.s_263_2.HasValue ? rsw24.s_263_2.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_3месяц", rsw24.s_263_3.HasValue ? rsw24.s_263_3.Value.ToString() : "0")))));
                }

                if ((rsw24.s_267_0.HasValue && rsw24.s_267_0.Value != 0) || (rsw24.s_267_1.HasValue && rsw24.s_267_1.Value != 0) || (rsw24.s_267_2.HasValue && rsw24.s_267_2.Value != 0) || (rsw24.s_267_3.HasValue && rsw24.s_267_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                        new XElement("КодСпециальнойОценкиУсловийТруда", "В3.1"),
                                        new XElement("РасчетНачисленныхПоДопТарифу",
                                            new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                                new XElement("КодСтроки", "264"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_264_0.HasValue ? Utils.decToStr(rsw24.s_264_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_264_1.HasValue ? Utils.decToStr(rsw24.s_264_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_264_2.HasValue ? Utils.decToStr(rsw24.s_264_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_264_3.HasValue ? Utils.decToStr(rsw24.s_264_3.Value) : "0.00"))),
                                            new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                                new XElement("КодСтроки", "265"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_265_0.HasValue ? Utils.decToStr(rsw24.s_265_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_265_1.HasValue ? Utils.decToStr(rsw24.s_265_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_265_2.HasValue ? Utils.decToStr(rsw24.s_265_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_265_3.HasValue ? Utils.decToStr(rsw24.s_265_3.Value) : "0.00"))),
                                            new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                                new XElement("КодСтроки", "267"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_267_0.HasValue ? Utils.decToStr(rsw24.s_267_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_267_1.HasValue ? Utils.decToStr(rsw24.s_267_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_267_2.HasValue ? Utils.decToStr(rsw24.s_267_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_267_3.HasValue ? Utils.decToStr(rsw24.s_267_3.Value) : "0.00"))),
                                            new XElement("НачисленоПоДопТарифу",
                                                new XElement("КодСтроки", "268"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_268_0.HasValue ? Utils.decToStr(rsw24.s_268_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_268_1.HasValue ? Utils.decToStr(rsw24.s_268_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_268_2.HasValue ? Utils.decToStr(rsw24.s_268_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_268_3.HasValue ? Utils.decToStr(rsw24.s_268_3.Value) : "0.00"))),
                                            new XElement("КоличествоФЛпоДопТарифу",
                                                new XElement("КодСтроки", "269"),
                                                new XElement("КоличествоЗЛ_Всего", rsw24.s_269_0.HasValue ? rsw24.s_269_0.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_1месяц", rsw24.s_269_1.HasValue ? rsw24.s_269_1.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_2месяц", rsw24.s_269_2.HasValue ? rsw24.s_269_2.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_3месяц", rsw24.s_269_3.HasValue ? rsw24.s_269_3.Value.ToString() : "0")))));
                }


                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_4);
            }

            #endregion


            #region Раздел 2.5

            if (db.FormsRSW2014_1_Razd_2_5_1.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID) || db.FormsRSW2014_1_Razd_2_5_2.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
            {
                XElement Раздел_2_5 = new XElement("Раздел_2_5");
                #region Раздел 2.5.1
                if (db.FormsRSW2014_1_Razd_2_5_1.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                {
                    XElement ПереченьПачекИсходныхСведенийПУ = new XElement("ПереченьПачекИсходныхСведенийПУ",
                                                                   new XElement("КоличествоПачек", db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID).Count().ToString()));
                    decimal col2 = 0;
                    decimal col3 = 0;
                    long col4 = 0;

                    foreach (var rsw251 in db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                    {
                        XElement СведенияОпачкеИсходных = new XElement("СведенияОпачкеИсходных",
                                                              new XElement("НомерПП", rsw251.NumRec.Value),
                                                              new XElement("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную", rsw251.Col_2.HasValue ? Utils.decToStr(rsw251.Col_2.Value) : "0.00"),
                                                              new XElement("СтраховыхВзносовОПС", rsw251.Col_3.HasValue ? Utils.decToStr(rsw251.Col_3.Value) : "0.00"),
                                                              new XElement("КоличествоЗЛвПачке", rsw251.Col_4.Value),
                                                              new XElement("ИмяФайла", rsw251.Col_5));
                        col2 = rsw251.Col_2.HasValue ? col2 + rsw251.Col_2.Value : col2;
                        col3 = rsw251.Col_3.HasValue ? col3 + rsw251.Col_3.Value : col3;
                        col4 = rsw251.Col_4.HasValue ? col4 + rsw251.Col_4.Value : col4;

                        ПереченьПачекИсходныхСведенийПУ.Add(СведенияОпачкеИсходных);
                    }

                    ПереченьПачекИсходныхСведенийПУ.Add(new XElement("ИтогоСведенияПоПачкам",
                                                            new XElement("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную", col2 != 0 ? Utils.decToStr(col2) : "0.00"),
                                                            new XElement("СтраховыхВзносовОПС", col3 != 0 ? Utils.decToStr(col3) : "0.00"),
                                                            new XElement("КоличествоЗЛвПачке", col4.ToString())));

                    Раздел_2_5.Add(ПереченьПачекИсходныхСведенийПУ);
                }


                #endregion

                #region Раздел 2.5.2

                if (db.FormsRSW2014_1_Razd_2_5_2.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                {
                    XElement ПереченьПачекКорректирующихСведенийПУ = new XElement("ПереченьПачекКорректирующихСведенийПУ",
                                                                         new XElement("КоличествоПачек", db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID).Count().ToString()));

                    decimal col4 = 0;
                    decimal col5 = 0;
                    decimal col6 = 0;
                    long col7 = 0;

                    foreach (var rsw252 in db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                    {
                        XElement СведенияОпачкеКорректирующих = new XElement("СведенияОпачкеКорректирующих",
                                                              new XElement("НомерПП", rsw252.NumRec.Value),
                                                              new XElement("КорректируемыйОтчетныйПериод",
                                                                  new XElement("Квартал", rsw252.Col_2_QuarterKorr),
                                                                  new XElement("Год", rsw252.Col_3_YearKorr)),
                                                              new XElement("ДоначисленоСтраховыхВзносовОПС", rsw252.Col_4.HasValue ? Utils.decToStr(rsw252.Col_4.Value) : "0.00"),
                                                              new XElement("ДоначисленоНаСтраховуюЧасть", rsw252.Col_5.HasValue ? Utils.decToStr(rsw252.Col_5.Value) : "0.00"),
                                                              new XElement("ДоначисленоНаНакопительнуюЧасть", rsw252.Col_6.HasValue ? Utils.decToStr(rsw252.Col_6.Value) : "0.00"),
                                                              new XElement("КоличествоЗЛвПачке", rsw252.Col_7.Value),
                                                              new XElement("ИмяФайла", rsw252.Col_8));

                        col4 = rsw252.Col_4.HasValue ? col4 + rsw252.Col_4.Value : col4;
                        col5 = rsw252.Col_5.HasValue ? col5 + rsw252.Col_5.Value : col5;
                        col6 = rsw252.Col_6.HasValue ? col6 + rsw252.Col_6.Value : col6;
                        col7 = rsw252.Col_7.HasValue ? col7 + rsw252.Col_7.Value : col7;


                        ПереченьПачекКорректирующихСведенийПУ.Add(СведенияОпачкеКорректирующих);
                    }

                    ПереченьПачекКорректирующихСведенийПУ.Add(new XElement("ИтогоСведенияПоПачкамКорректирующих",
                                                            new XElement("ДоначисленоСтраховыхВзносовОПС", col4 != 0 ? Utils.decToStr(col4) : "0.00"),
                                                            new XElement("ДоначисленоНаСтраховуюЧасть", col5 != 0 ? Utils.decToStr(col5) : "0.00"),
                                                            new XElement("ДоначисленоНаНакопительнуюЧасть", col6 != 0 ? Utils.decToStr(col6) : "0.00"),
                                                            new XElement("КоличествоЗЛвПачке", col7.ToString())));

                    Раздел_2_5.Add(ПереченьПачекКорректирующихСведенийПУ);
                }
                #endregion

                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_5);
            }

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел2РасчетПоТарифуИдопТарифу);

            #endregion


            #endregion


            #region Раздел3РасчетНаПравоПримененияПониженногоТарифа2014
            if ((rsw.ExistPart_3_1.HasValue && rsw.ExistPart_3_1 == 1) || (rsw.ExistPart_3_2.HasValue && rsw.ExistPart_3_2 == 1) || (rsw.ExistPart_3_3.HasValue && rsw.ExistPart_3_3 == 1) || (rsw.ExistPart_3_4.HasValue && rsw.ExistPart_3_4 == 1) || (rsw.ExistPart_3_5.HasValue && rsw.ExistPart_3_5 == 1) || (rsw.ExistPart_3_6.HasValue && rsw.ExistPart_3_6 == 1))
            {
                xName = "Раздел3РасчетНаПравоПримененияПониженногоТарифа2014";
                if (yearType == 2015)
                    xName = "Раздел3РасчетНаПравоПримененияПониженногоТарифа2015";

                XElement Раздел3РасчетНаПравоПримененияПониженногоТарифа2014 = new XElement(xName);

                #region Раздел3_1_ДляОбщественныхОрганизацийИнвалидов
                if (yearType == 2014 && rsw.ExistPart_3_1.HasValue && rsw.ExistPart_3_1 == 1)
                {
                    string s0 = "0.00";
                    string s1 = "0.00";
                    string s2 = "0.00";
                    string s3 = "0.00";

                    if (rsw.s_321_0.HasValue && rsw.s_322_0.HasValue && rsw.s_321_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round(((rsw.s_322_0.Value / rsw.s_321_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_321_1.HasValue && rsw.s_322_1.HasValue && rsw.s_321_1.Value != 0)
                    {
                        s1 = Utils.decToStr(Math.Round(((rsw.s_322_1.Value / rsw.s_321_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_321_2.HasValue && rsw.s_322_2.HasValue && rsw.s_321_2.Value != 0)
                    {
                        s2 = Utils.decToStr(Math.Round(((rsw.s_322_2.Value / rsw.s_321_2.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_321_3.HasValue && rsw.s_322_3.HasValue && rsw.s_321_3.Value != 0)
                    {
                        s3 = Utils.decToStr(Math.Round(((rsw.s_322_3.Value / rsw.s_321_3.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    XElement Раздел3_1_ДляОбщественныхОрганизацийИнвалидов = new XElement("Раздел3_1_ДляОбщественныхОрганизацийИнвалидов",
                                                                                 new XElement("ЧисленностьЧленовОрганизации",
                                                                                     new XElement("КодСтроки", "321"),
                                                                                     new XElement("КоличествоЗЛ_Всего", rsw.s_321_0.HasValue ? rsw.s_321_0.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_1месяц", rsw.s_321_1.HasValue ? rsw.s_321_1.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_2месяц", rsw.s_321_2.HasValue ? rsw.s_321_2.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_3месяц", rsw.s_321_3.HasValue ? rsw.s_321_3.Value.ToString() : "0")),
                                                                                 new XElement("ЧисленностьИнвалидов",
                                                                                     new XElement("КодСтроки", "322"),
                                                                                     new XElement("КоличествоЗЛ_Всего", rsw.s_322_0.HasValue ? rsw.s_322_0.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_1месяц", rsw.s_322_1.HasValue ? rsw.s_322_1.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_2месяц", rsw.s_322_2.HasValue ? rsw.s_322_2.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_3месяц", rsw.s_322_3.HasValue ? rsw.s_322_3.Value.ToString() : "0")),
                                                                                 new XElement("УдельныйВесЧисленности",
                                                                                     new XElement("КодСтроки", "323"),
                                                                                     new XElement("УдельныйВес_Всего", s0),
                                                                                     new XElement("УдельныйВес_1месяц", s1),
                                                                                     new XElement("УдельныйВес_2месяц", s2),
                                                                                     new XElement("УдельныйВес_3месяц", s3)));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_1_ДляОбщественныхОрганизацийИнвалидов);
                }
                #endregion

                #region Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов
                if (yearType == 2014 && rsw.ExistPart_3_2.HasValue && rsw.ExistPart_3_2 == 1)
                {
                    string s0 = "0.00";
                    string s1 = "0.00";
                    string s2 = "0.00";
                    string s3 = "0.00";

                    if (rsw.s_331_0.HasValue && rsw.s_332_0.HasValue && rsw.s_331_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round(((rsw.s_332_0.Value / rsw.s_331_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_331_1.HasValue && rsw.s_332_1.HasValue && rsw.s_331_1.Value != 0)
                    {
                        s1 = Utils.decToStr(Math.Round(((rsw.s_332_1.Value / rsw.s_331_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_331_2.HasValue && rsw.s_332_2.HasValue && rsw.s_331_2.Value != 0)
                    {
                        s2 = Utils.decToStr(Math.Round(((rsw.s_332_2.Value / rsw.s_331_2.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_331_3.HasValue && rsw.s_332_3.HasValue && rsw.s_331_3.Value != 0)
                    {
                        s3 = Utils.decToStr(Math.Round(((rsw.s_332_3.Value / rsw.s_331_3.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    string s0_ = "0.00";
                    string s1_ = "0.00";
                    string s2_ = "0.00";
                    string s3_ = "0.00";

                    if (rsw.s_334_0.HasValue && rsw.s_335_0.HasValue && rsw.s_334_0.Value != 0)
                    {
                        s0_ = Utils.decToStr(Math.Round(((rsw.s_335_0.Value / rsw.s_334_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_334_1.HasValue && rsw.s_335_1.HasValue && rsw.s_334_1.Value != 0)
                    {
                        s1_ = Utils.decToStr(Math.Round(((rsw.s_335_1.Value / rsw.s_334_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_334_2.HasValue && rsw.s_335_2.HasValue && rsw.s_334_2.Value != 0)
                    {
                        s2_ = Utils.decToStr(Math.Round(((rsw.s_335_2.Value / rsw.s_334_2.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_334_3.HasValue && rsw.s_335_3.HasValue && rsw.s_334_3.Value != 0)
                    {
                        s3_ = Utils.decToStr(Math.Round(((rsw.s_335_3.Value / rsw.s_334_3.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }


                    XElement Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов = new XElement("Раздел3_1_ДляОбщественныхОрганизацийИнвалидов",
                                                                                 new XElement("ЧисленностьЧленовОрганизации",
                                                                                     new XElement("КодСтроки", "331"),
                                                                                     new XElement("КоличествоЗЛ_Всего", rsw.s_331_0.HasValue ? rsw.s_331_0.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_1месяц", rsw.s_331_1.HasValue ? rsw.s_331_1.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_2месяц", rsw.s_331_2.HasValue ? rsw.s_331_2.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_3месяц", rsw.s_331_3.HasValue ? rsw.s_331_3.Value.ToString() : "0")),
                                                                                 new XElement("ЧисленностьИнвалидов",
                                                                                     new XElement("КодСтроки", "332"),
                                                                                     new XElement("КоличествоЗЛ_Всего", rsw.s_332_0.HasValue ? rsw.s_332_0.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_1месяц", rsw.s_332_1.HasValue ? rsw.s_332_1.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_2месяц", rsw.s_332_2.HasValue ? rsw.s_332_2.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_3месяц", rsw.s_332_3.HasValue ? rsw.s_332_3.Value.ToString() : "0")),
                                                                                 new XElement("УдельныйВесЧисленности",
                                                                                     new XElement("КодСтроки", "333"),
                                                                                     new XElement("УдельныйВес_Всего", s0),
                                                                                     new XElement("УдельныйВес_1месяц", s1),
                                                                                     new XElement("УдельныйВес_2месяц", s2),
                                                                                     new XElement("УдельныйВес_3месяц", s3)),
                                                                                 new XElement("ФондОплатыТруда",
                                                                                     new XElement("КодСтроки", "334"),
                                                                                     new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_334_0.HasValue ? Utils.decToStr(rsw.s_334_0.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние1месяц", rsw.s_334_1.HasValue ? Utils.decToStr(rsw.s_334_1.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние2месяц", rsw.s_334_2.HasValue ? Utils.decToStr(rsw.s_334_2.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние3месяц", rsw.s_334_3.HasValue ? Utils.decToStr(rsw.s_334_3.Value) : "0.00")),
                                                                                 new XElement("ЗарплатаИнвалидов",
                                                                                     new XElement("КодСтроки", "335"),
                                                                                     new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_335_0.HasValue ? Utils.decToStr(rsw.s_335_0.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние1месяц", rsw.s_335_1.HasValue ? Utils.decToStr(rsw.s_335_1.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние2месяц", rsw.s_335_2.HasValue ? Utils.decToStr(rsw.s_335_2.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние3месяц", rsw.s_335_3.HasValue ? Utils.decToStr(rsw.s_335_3.Value) : "0.00")),
                                                                                 new XElement("УдельныйВесЗарплатыИнвалидов",
                                                                                     new XElement("КодСтроки", "336"),
                                                                                     new XElement("УдельныйВес_Всего", s0_),
                                                                                     new XElement("УдельныйВес_1месяц", s1_),
                                                                                     new XElement("УдельныйВес_2месяц", s2_),
                                                                                     new XElement("УдельныйВес_3месяц", s3_)));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов);
                }
                #endregion

                #region Раздел3_3_ДляОрганизацийИТ. Для РСВ-1 2015 Раздел3_1_ДляОрганизацийИТ
                if (rsw.ExistPart_3_3.HasValue && rsw.ExistPart_3_3 == 1)
                {
                    string s0 = "0.00";
                    string s1 = "0.00";

                    if (rsw.s_341_0.HasValue && rsw.s_342_0.HasValue && rsw.s_341_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round(((rsw.s_342_0.Value / rsw.s_341_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_341_1.HasValue && rsw.s_342_1.HasValue && rsw.s_341_1.Value != 0)
                    {
                        s1 = Utils.decToStr(Math.Round(((rsw.s_342_1.Value / rsw.s_341_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    xName = "Раздел3_3_ДляОрганизацийИТ";
                    //    if (yearType == 2015)
                    //          xName = "Раздел3_1_ДляОрганизацийИТ";

                    XElement Раздел3_3_ДляОрганизацийИТ = new XElement(xName,
                                                              new XElement("СуммаДоходовПоНКвсего",
                                                                  new XElement("КодСтроки", "341"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_341_0.HasValue ? Utils.decToStr(rsw.s_341_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_341_1.HasValue ? Utils.decToStr(rsw.s_341_1.Value) : "0.00")),
                                                              new XElement("СуммаДоходовИТизНих",
                                                                  new XElement("КодСтроки", "342"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_342_0.HasValue ? Utils.decToStr(rsw.s_342_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_342_1.HasValue ? Utils.decToStr(rsw.s_342_1.Value) : "0.00")),
                                                              new XElement("ДоляДоходовИТ",
                                                                  new XElement("КодСтроки", "343"),
                                                                  new XElement("ДоляДоходаПоПредшествующему", s0),
                                                                  new XElement("ДоляДоходаПоТекущему", s1)),
                                                              new XElement("ЧисленностьРаботниковИТ",
                                                                  new XElement("КодСтроки", "344"),
                                                                  new XElement("КоличествоЗЛпоПредшествующему", rsw.s_344_0.HasValue ? rsw.s_344_0.Value.ToString() : "0"),
                                                                  new XElement("КоличествоЗЛпоТекущему", rsw.s_344_1.HasValue ? rsw.s_344_1.Value.ToString() : "0")),
                                                              new XElement("СведенияИзРеестраИТ",
                                                                  new XElement("КодСтроки", "345"),
                                                                  new XElement("ДатаЗаписиВреестре", rsw.s_345_0.HasValue ? rsw.s_345_0.Value.ToShortDateString() : ""),
                                                                  new XElement("НомерЗаписиВреестре", !String.IsNullOrEmpty(rsw.s_345_1) ? rsw.s_345_1 : "")));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_3_ДляОрганизацийИТ);

                }
                #endregion

                #region Раздел3_4_ДляОрганизацийСМИ
                if (yearType == 2014 && rsw.ExistPart_3_4.HasValue && rsw.ExistPart_3_4 == 1)
                {
                    XElement Раздел3_4_ДляОрганизацийСМИ = new XElement("Раздел3_4_ДляОрганизацийСМИ");
                    decimal sum = 0;
                    var RSW_3_4_List = db.FormsRSW2014_1_Razd_3_4.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.ID).ToList();

                    foreach (var rsw34 in RSW_3_4_List)
                    {
                        XElement СведенияПоВидуДеятельности = new XElement("СведенияПоВидуДеятельности",
                                                                  new XElement("НомерПП", rsw34.NumOrd.Value.ToString()),
                                                                  new XElement("НаименованиеВидаЭД", rsw34.NameOKWED.Substring(0, rsw34.NameOKWED.Length > 250 ? 250 : rsw34.NameOKWED.Length)),
                                                                  new XElement("КодПоОКВЭД", rsw34.OKWED),
                                                                  new XElement("ДоходыПоВидуЭД", rsw34.Income.HasValue ? Utils.decToStr(rsw34.Income.Value) : "0.00"),
                                                                  new XElement("ДоляДоходовПоВидуЭД", rsw34.RateIncome.HasValue ? Utils.decToStr(rsw34.RateIncome.Value) : "0.00"));


                        Раздел3_4_ДляОрганизацийСМИ.Add(СведенияПоВидуДеятельности);
                    }

                    Раздел3_4_ДляОрганизацийСМИ.Add(new XElement("ИтогоПоВсемВидамДеятельности", sum != 0 ? Utils.decToStr(sum) : "0.00"));
                    Раздел3_4_ДляОрганизацийСМИ.Add(new XElement("СведенияИзРеестраСМИ",
                                                                  new XElement("КодСтроки", "351"),
                                                                  new XElement("ДатаЗаписиВреестре", rsw.s_351_0.HasValue ? rsw.s_351_0.Value.ToString() : ""),
                                                                  new XElement("НомерЗаписиВреестре", !String.IsNullOrEmpty(rsw.s_351_1) ? rsw.s_351_1 : "")));

                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_4_ДляОрганизацийСМИ);

                }
                #endregion

                #region Раздел3_5_ДляОрганизацийПрименяющихУСН. Для РСВ-1 2015 Раздел3_2_ДляОрганизацийПрименяющихУСН
                if (rsw.ExistPart_3_5.HasValue && rsw.ExistPart_3_5 == 1)
                {
                    string s0 = "0.00";

                    if (rsw.s_361_0.HasValue && rsw.s_362_0.HasValue && rsw.s_361_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round(((rsw.s_362_0.Value / rsw.s_361_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    xName = "Раздел3_5_ДляОрганизацийПрименяющихУСН";
                    //     if (yearType == 2015)
                    //         xName = "Раздел3_2_ДляОрганизацийПрименяющихУСН";

                    XElement Раздел3_5_ДляОрганизацийПрименяющихУСН = new XElement(xName,
                                                                          new XElement("СуммаДоходаПоСтатье346_15НКвсего",
                                                                              new XElement("КодСтроки", "361"),
                                                                              new XElement("СуммаДохода", rsw.s_361_0.HasValue ? Utils.decToStr(rsw.s_361_0.Value) : "0.00")),
                                                                          new XElement("СуммаДоходаПоСтатье58ИзНих",
                                                                              new XElement("КодСтроки", "362"),
                                                                              new XElement("СуммаДохода", rsw.s_362_0.HasValue ? Utils.decToStr(rsw.s_362_0.Value) : "0.00")),
                                                                          new XElement("ДоляДоходаПоСтатье58",
                                                                              new XElement("КодСтроки", "363"),
                                                                              new XElement("ДоляДохода", s0)));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_5_ДляОрганизацийПрименяющихУСН);

                }
                #endregion

                #region Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН. Для РСВ-1 2015 Раздел3_3_ДляНекоммерческихОрганизацийПрименяющихУСН
                if (rsw.ExistPart_3_6.HasValue && rsw.ExistPart_3_6 == 1)
                {
                    string s0 = "0.00";
                    string s1 = "0.00";

                    if (rsw.s_371_0.HasValue && rsw.s_372_0.HasValue && rsw.s_371_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round((((rsw.s_372_0.Value + rsw.s_373_0.Value + rsw.s_374_0.Value) / rsw.s_371_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_371_1.HasValue && rsw.s_372_1.HasValue && rsw.s_371_1.Value != 0)
                    {
                        s1 = Utils.decToStr(Math.Round((((rsw.s_372_1.Value + rsw.s_373_1.Value + rsw.s_374_1.Value) / rsw.s_371_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    xName = "Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН";
                    //         if (yearType == 2015)
                    //             xName = "Раздел3_3_ДляНекоммерческихОрганизацийПрименяющихУСН";

                    XElement Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН = new XElement(xName,
                                                              new XElement("СуммаДоходовВсего",
                                                                  new XElement("КодСтроки", "371"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_371_0.HasValue ? Utils.decToStr(rsw.s_371_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_371_1.HasValue ? Utils.decToStr(rsw.s_371_1.Value) : "0.00")),
                                                              new XElement("СуммаДоходовВвидеЦелевыхПоступлений",
                                                                  new XElement("КодСтроки", "372"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_372_0.HasValue ? Utils.decToStr(rsw.s_372_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_372_1.HasValue ? Utils.decToStr(rsw.s_372_1.Value) : "0.00")),
                                                              new XElement("СуммаДоходовВвидеГрантов",
                                                                  new XElement("КодСтроки", "373"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_373_0.HasValue ? Utils.decToStr(rsw.s_373_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_373_1.HasValue ? Utils.decToStr(rsw.s_373_1.Value) : "0.00")),
                                                              new XElement("СуммаДоходовОтОсуществленияОпределенныхВЭД",
                                                                  new XElement("КодСтроки", "374"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_374_0.HasValue ? Utils.decToStr(rsw.s_374_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_374_1.HasValue ? Utils.decToStr(rsw.s_374_1.Value) : "0.00")),
                                                              new XElement("ДоляДоходов",
                                                                  new XElement("КодСтроки", "375"),
                                                                  new XElement("ДоляДоходаПоПредшествующему", s0),
                                                                  new XElement("ДоляДоходаПоТекущему", s1)));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН);
                }
                #endregion

                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел3РасчетНаПравоПримененияПониженногоТарифа2014);
            }
            #endregion

            #region Раздел4СуммыДоначисленныхСтраховыхВзносов2014
            if (rsw.ExistPart_4.HasValue && rsw.ExistPart_4.Value == 1)
            {
                XElement Раздел4СуммыДоначисленныхСтраховыхВзносов2014 = new XElement(yearType != 2015 ? "Раздел4СуммыДоначисленныхСтраховыхВзносов2014" : "Раздел4");
                var RSW_4_List = db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.ID).ToList();

                decimal[] sum = new decimal[9];
                for (int y = 0; y < sum.Length; y++)
                {
                    sum[y] = 0;
                }

                foreach (var rsw4 in RSW_4_List)
                {
                    XElement СуммаДоначисленныхВзносовЗаПериодНачинаяС2014 = new XElement("СуммаДоначисленныхВзносовЗаПериодНачинаяС2014",
                                                                                 new XElement("НомерПП", rsw4.NumOrd.Value),
                                                                                 new XElement("ОснованиеДляДоначисления", rsw4.Base.Value));
                    if (rsw4.YearPer.Value >= 2014 && (rsw4.Dop21.HasValue && rsw4.Dop21.Value != 0))
                        СуммаДоначисленныхВзносовЗаПериодНачинаяС2014.Add(new XElement("КодОснованияДляДопТарифа", rsw4.CodeBase.Value));


                    СуммаДоначисленныхВзносовЗаПериодНачинаяС2014.Add(new XElement("Год", rsw4.YearPer.Value),
                                                                                 new XElement("Месяц", rsw4.MonthPer.Value),
                                                                                 new XElement("СуммаДоначисленныхВзносовОПС2014всего", rsw4.Strah2014.HasValue ? Utils.decToStr(rsw4.Strah2014.Value) : "0.00"),
                                                                                 new XElement("СуммаДоначисленныхВзносовОПС2014превыщающие", rsw4.StrahMoreBase2014.HasValue ? Utils.decToStr(rsw4.StrahMoreBase2014.Value) : "0.00"),
                                                                                 new XElement("СуммаДоначисленныхВзносовНаСтраховуюВсего", rsw4.Strah2013.HasValue ? Utils.decToStr(rsw4.Strah2013.Value) : "0.00"),
                                                                                 new XElement("СуммаДоначисленныхВзносовНаСтраховуюПревышающие", rsw4.StrahMoreBase2013.HasValue ? Utils.decToStr(rsw4.StrahMoreBase2013.Value) : "0.00"),
                                                                                 new XElement("СуммаДоначисленныхВзносовНаНакопительную", rsw4.Nakop2013.HasValue ? Utils.decToStr(rsw4.Nakop2013.Value) : "0.00"),
                                                                                 new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ1", rsw4.Dop1.HasValue ? Utils.decToStr(rsw4.Dop1.Value) : "0.00"),
                                                                                 new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2", rsw4.Dop2.HasValue ? Utils.decToStr(rsw4.Dop2.Value) : "0.00"),
                                                                                 new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2_1", rsw4.Dop21.HasValue ? Utils.decToStr(rsw4.Dop21.Value) : "0.00"),
                                                                                 new XElement("СтраховыеВзносыОМС", rsw4.OMS.HasValue ? Utils.decToStr(rsw4.OMS.Value) : "0.00"));

                    Раздел4СуммыДоначисленныхСтраховыхВзносов2014.Add(СуммаДоначисленныхВзносовЗаПериодНачинаяС2014);

                    sum[0] = rsw4.Strah2014.HasValue ? rsw4.Strah2014.Value + sum[0] : sum[0];
                    sum[1] = rsw4.StrahMoreBase2014.HasValue ? rsw4.StrahMoreBase2014.Value + sum[1] : sum[1];
                    sum[2] = rsw4.Strah2013.HasValue ? rsw4.Strah2013.Value + sum[2] : sum[2];
                    sum[3] = rsw4.StrahMoreBase2013.HasValue ? rsw4.StrahMoreBase2013.Value + sum[3] : sum[3];
                    sum[4] = rsw4.Nakop2013.HasValue ? rsw4.Nakop2013.Value + sum[4] : sum[4];
                    sum[5] = rsw4.Dop1.HasValue ? rsw4.Dop1.Value + sum[5] : sum[5];
                    sum[6] = rsw4.Dop2.HasValue ? rsw4.Dop2.Value + sum[6] : sum[6];
                    sum[7] = rsw4.Dop21.HasValue ? rsw4.Dop21.Value + sum[7] : sum[7];
                    sum[8] = rsw4.OMS.HasValue ? rsw4.OMS.Value + sum[8] : sum[8];

                }

                if (RSW_4_List.Count() > 0)
                {
                    XElement ИтогоДоначисленоНачинаяС2014 = new XElement("ИтогоДоначисленоНачинаяС2014",
                                                                new XElement("СуммаДоначисленныхВзносовОПС2014всего", sum[0] != 0 ? Utils.decToStr(sum[0]) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовОПС2014превыщающие", sum[1] != 0 ? Utils.decToStr(sum[1]) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаСтраховуюВсего", sum[2] != 0 ? Utils.decToStr(sum[2]) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаСтраховуюПревышающие", sum[3] != 0 ? Utils.decToStr(sum[3]) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаНакопительную", sum[4] != 0 ? Utils.decToStr(sum[4]) : "0.00"),
                                                                new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ1", sum[5] != 0 ? Utils.decToStr(sum[5]) : "0.00"),
                                                                new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2", sum[6] != 0 ? Utils.decToStr(sum[6]) : "0.00"),
                                                                new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2_1", sum[7] != 0 ? Utils.decToStr(sum[7]) : "0.00"),
                                                                new XElement("СтраховыеВзносыОМС", sum[8] != 0 ? Utils.decToStr(sum[8]) : "0.00"));

                    Раздел4СуммыДоначисленныхСтраховыхВзносов2014.Add(ИтогоДоначисленоНачинаяС2014);
                }

                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел4СуммыДоначисленныхСтраховыхВзносов2014);
            }

            #endregion

            #region Раздел5СведенияОВыплатахВпользуОбучающихся2014
            if (rsw.ExistPart_5.HasValue && rsw.ExistPart_5.Value == 1)
            {
                var RSW_5_List = db.FormsRSW2014_1_Razd_5.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.ID).ToList();
                XElement Раздел5СведенияОВыплатахВпользуОбучающихся2014 = new XElement("Раздел5СведенияОВыплатахВпользуОбучающихся2014",
                                                                              new XElement("КоличествоОбучающихся", RSW_5_List.Count().ToString()));

                decimal[] sum = new decimal[4];
                for (int y = 0; y < sum.Length; y++)
                {
                    sum[y] = 0;
                }

                foreach (var rsw5 in RSW_5_List)
                {
                    XElement СведенияОбОбучающемся = new XElement("СведенияОбОбучающемся",
                                                         new XElement("НомерПП", rsw5.NumOrd.Value),
                                                         new XElement("ФИО",
                                                             new XElement("Фамилия", rsw5.LastName.Substring(0, rsw5.LastName.Length > 40 ? 40 : rsw5.LastName.Length)),
                                                             new XElement("Имя", rsw5.FirstName.Substring(0, rsw5.FirstName.Length > 40 ? 40 : rsw5.FirstName.Length)),
                                                             new XElement("Отчество", rsw5.MiddleName.Substring(0, rsw5.MiddleName.Length > 40 ? 40 : rsw5.MiddleName.Length))),
                                                         new XElement("НомерСправкиОчленствеВстудОтряде", rsw5.NumSpravka.Substring(0, rsw5.NumSpravka.Length > 40 ? 40 : rsw5.NumSpravka.Length)),
                                                         new XElement("ДатаВыдачиСправкиОчленствеВстудОтряде", rsw5.DateSpravka.Value.ToShortDateString()),
                                                         new XElement("НомерСправкиОбОчномОбучении", rsw5.NumSpravka1.Substring(0, rsw5.NumSpravka1.Length > 40 ? 40 : rsw5.NumSpravka1.Length)),
                                                         new XElement("ДатаВыдачиСправкиОбОчномОбучении", rsw5.DateSpravka1.Value.ToShortDateString()),
                                                         new XElement("СуммыВыплатИвознаграждений",
                                                             new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw5.SumPay.HasValue ? Utils.decToStr(rsw5.SumPay.Value) : "0.00"),
                                                             new XElement("СуммаПоследние1месяц", rsw5.SumPay_0.HasValue ? Utils.decToStr(rsw5.SumPay_0.Value) : "0.00"),
                                                             new XElement("СуммаПоследние2месяц", rsw5.SumPay_1.HasValue ? Utils.decToStr(rsw5.SumPay_1.Value) : "0.00"),
                                                             new XElement("СуммаПоследние3месяц", rsw5.SumPay_2.HasValue ? Utils.decToStr(rsw5.SumPay_2.Value) : "0.00")));

                    Раздел5СведенияОВыплатахВпользуОбучающихся2014.Add(СведенияОбОбучающемся);

                    sum[0] = rsw5.SumPay.HasValue ? sum[0] + rsw5.SumPay.Value : sum[0];
                    sum[1] = rsw5.SumPay_0.HasValue ? sum[1] + rsw5.SumPay_0.Value : sum[1];
                    sum[2] = rsw5.SumPay_1.HasValue ? sum[2] + rsw5.SumPay_1.Value : sum[2];
                    sum[3] = rsw5.SumPay_2.HasValue ? sum[3] + rsw5.SumPay_2.Value : sum[3];
                }

                Раздел5СведенияОВыплатахВпользуОбучающихся2014.Add(new XElement("ИтогоВыплат",
                                                             new XElement("СуммаВсегоСначалаРасчетногоПериода", sum[0] != 0 ? Utils.decToStr(sum[0]) : "0.00"),
                                                             new XElement("СуммаПоследние1месяц", sum[1] != 0 ? Utils.decToStr(sum[1]) : "0.00"),
                                                             new XElement("СуммаПоследние2месяц", sum[2] != 0 ? Utils.decToStr(sum[2]) : "0.00"),
                                                             new XElement("СуммаПоследние3месяц", sum[3] != 0 ? Utils.decToStr(sum[3]) : "0.00")));

                XElement СведенияИзРеестраМДОО = new XElement("СведенияИзРеестраМДОО",
                                                     new XElement("КодСтроки", "501"));

                if (!String.IsNullOrEmpty(rsw.s_501_1_0))
                {
                    СведенияИзРеестраМДОО.Add(new XElement("РеквизитыЗаписиВреестре",
                                                  new XElement("ДатаЗаписиВреестре", rsw.s_501_0_0.HasValue ? rsw.s_501_0_0.Value.ToShortDateString() : ""),
                                                  new XElement("НомерЗаписиВреестре", rsw.s_501_1_0.Substring(0, rsw.s_501_1_0.Length > 20 ? 20 : rsw.s_501_1_0.Length))));
                }

                if (!String.IsNullOrEmpty(rsw.s_501_1_1))
                {
                    СведенияИзРеестраМДОО.Add(new XElement("РеквизитыЗаписиВреестре",
                                                  new XElement("ДатаЗаписиВреестре", rsw.s_501_0_1.HasValue ? rsw.s_501_0_1.Value.ToShortDateString() : ""),
                                                  new XElement("НомерЗаписиВреестре", rsw.s_501_1_1.Substring(0, rsw.s_501_1_1.Length > 20 ? 20 : rsw.s_501_1_1.Length))));
                }

                if (!String.IsNullOrEmpty(rsw.s_501_1_2))
                {
                    СведенияИзРеестраМДОО.Add(new XElement("РеквизитыЗаписиВреестре",
                                                  new XElement("ДатаЗаписиВреестре", rsw.s_501_0_2.HasValue ? rsw.s_501_0_2.Value.ToShortDateString() : ""),
                                                  new XElement("НомерЗаписиВреестре", rsw.s_501_1_2.Substring(0, rsw.s_501_1_2.Length > 20 ? 20 : rsw.s_501_1_2.Length))));
                }

                if (!String.IsNullOrEmpty(rsw.s_501_1_3))
                {
                    СведенияИзРеестраМДОО.Add(new XElement("РеквизитыЗаписиВреестре",
                                                  new XElement("ДатаЗаписиВреестре", rsw.s_501_0_3.HasValue ? rsw.s_501_0_3.Value.ToShortDateString() : ""),
                                                  new XElement("НомерЗаписиВреестре", rsw.s_501_1_3.Substring(0, rsw.s_501_1_3.Length > 20 ? 20 : rsw.s_501_1_3.Length))));
                }


                Раздел5СведенияОВыплатахВпользуОбучающихся2014.Add(СведенияИзРеестраМДОО);

                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел5СведенияОВыплатахВпользуОбучающихся2014);

            }
            #endregion

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ЛицоПодтверждающееСведения", rsw.ConfirmType.ToString()));

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ФИОлицаПодтверждающегоСведения"));

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОлицаПодтверждающегоСведения").Add(new XElement("Фамилия", !String.IsNullOrEmpty(rsw.ConfirmLastName) ? rsw.ConfirmLastName.ToUpper() : ""));
            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОлицаПодтверждающегоСведения").Add(new XElement("Имя", !String.IsNullOrEmpty(rsw.ConfirmFirstName) ? rsw.ConfirmFirstName.ToUpper() : ""));
            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОлицаПодтверждающегоСведения").Add(new XElement("Отчество", !String.IsNullOrEmpty(rsw.ConfirmMiddleName) ? rsw.ConfirmMiddleName.ToUpper() : ""));

            if (!String.IsNullOrEmpty(rsw.ConfirmOrgName))
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("НаименованиеОрганизацииПредставителя", rsw.ConfirmOrgName.ToUpper()));

            //если есть документ 
            if (rsw.ConfirmDocType_ID.HasValue)
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ДокументПодтверждающийПолномочияПредставителя"));
                string docName = "";
                if (db.DocumentTypes.FirstOrDefault(x => x.ID == rsw.ConfirmDocType_ID).Code == "ПРОЧЕЕ")
                    docName = rsw.ConfirmDocName;
                else
                    docName = db.DocumentTypes.FirstOrDefault(x => x.ID == rsw.ConfirmDocType_ID).Code;

                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("НаименованиеУдостоверяющего", docName.Substring(0, docName.Length > 80 ? 80 : docName.Length).ToUpper()));
                if (!String.IsNullOrEmpty(rsw.ConfirmDocSerLat))
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("СерияРимскиеЦифры", rsw.ConfirmDocSerLat.Substring(0, rsw.ConfirmDocSerLat.Length > 8 ? 8 : rsw.ConfirmDocSerLat.Length).ToUpper()));
                if (!String.IsNullOrEmpty(rsw.ConfirmDocSerRus))
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("СерияРусскиеБуквы", rsw.ConfirmDocSerRus.Substring(0, rsw.ConfirmDocSerRus.Length > 8 ? 8 : rsw.ConfirmDocSerRus.Length).ToUpper()));
                if (rsw.ConfirmDocNum.HasValue)
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("НомерУдостоверяющего", rsw.ConfirmDocNum.ToString().Substring(0, rsw.ConfirmDocNum.ToString().Length > 8 ? 8 : rsw.ConfirmDocNum.ToString().Length).ToUpper()));
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("ДатаВыдачи", rsw.ConfirmDocDate.HasValue ? rsw.ConfirmDocDate.Value.ToShortDateString() : ""));
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("КемВыдан", rsw.ConfirmDocKemVyd.Substring(0, rsw.ConfirmDocKemVyd.Length > 80 ? 80 : rsw.ConfirmDocKemVyd.Length).ToUpper()));
            }

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ДатаЗаполнения", rsw.DateUnderwrite.Value.ToShortDateString()));


            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014);


            xDoc.Root.SetDefaultXmlNamespace(pfr);



            //            if (yearType == 2015)
            //                xDoc.Root.SetDefaultXmlNamespace(pfr);

            return xDoc;
        }

        private XDocument generateXML_RSW2014_6(xmlInfo itemT)
        {
            pu6Entities db_temp = new pu6Entities();
            pfrXMLEntities dbxml_temp = new pfrXMLEntities();

            xmlInfo item = dbxml_temp.xmlInfo.First(x => x.ID == itemT.ID);

            int yearType = ((item.Year == 2014) || (item.Year == 2015 && item.Quarter == 3)) ? 2014 : 2015;

            //    string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6 = new XElement("СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6",
                                                                  new XElement("НомерВпачке", 1),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));



            Insurer ins = db_temp.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            //if (!String.IsNullOrEmpty(ins.EGRIP))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            //}

            //if (!String.IsNullOrEmpty(ins.EGRUL))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            //}

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length).ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length).ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));


            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(СоставительПачки);
            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("ТипСведений", InfoType));

            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("ОтчетныйПериод",
                                                           new XElement("Квартал", item.Quarter.Value),
                                                           new XElement("Год", item.Year.Value)));

            if (item.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
            {
                СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                               new XElement("Квартал", item.QuarterKorr.Value),
                                                               new XElement("Год", item.YearKorr.Value)));
            }

            FormsRSW2014_1_Razd_2_5_1 rsw251 = null;

            if (item.ParentID != null)
            {
                xmlInfo item_t = dbxml_temp.xmlInfo.First(x => x.ID == item.ParentID);
                if (db_temp.FormsRSW2014_1_1.Any(x => x.ID == item_t.SourceID))
                {
                    var rsw = db_temp.FormsRSW2014_1_1.First(x => x.ID == item_t.SourceID);
                    if (db_temp.FormsRSW2014_1_Razd_2_5_1.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID && x.Col_5 == item.FileName))
                    {
                        rsw251 = db_temp.FormsRSW2014_1_Razd_2_5_1.FirstOrDefault(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID && x.Col_5 == item.FileName);
                    }
                }
            }

            if (item.rsw2014.First().RSW_2_5_1_2.HasValue)
            {
                if (rsw251 != null)
                    СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную", rsw251.Col_2.HasValue ? Utils.decToStr(rsw251.Col_2.Value) : "0.00"));
                else
                    СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную", item.rsw2014.First().RSW_2_5_1_2.Value != 0 ? Utils.decToStr(item.rsw2014.First().RSW_2_5_1_2.Value) : "0.00"));
            }

            if (item.rsw2014.First().RSW_2_5_1_3.HasValue)
            {
                if (rsw251 != null)
                    СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("СтраховыхВзносовОПС", rsw251.Col_3.HasValue ? Utils.decToStr(rsw251.Col_3.Value) : "0.00"));
                else
                    СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("СтраховыхВзносовОПС", item.rsw2014.First().RSW_2_5_1_3.Value != 0 ? Utils.decToStr(item.rsw2014.First().RSW_2_5_1_3.Value) : "0.00"));
            }

            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6);


            #region СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ

            var staffTempList = item.StaffList;
            List<long> t = staffTempList.Select(x => x.StaffID.Value).ToList();
            var StaffList = db_temp.Staff.Where(x => t.Contains(x.ID)).ToList();

            var t2 = staffTempList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

            var rsw61List_tmp = db_temp.FormsRSW2014_1_Razd_6_1.Where(x => t2.Contains(x.ID)).ToList();
            //t = rsw61List_tmp.Select(x => x.ID).ToList();

            var stajOsn_list = db_temp.StajOsn.Where(x => t2.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).ToList();//
            List<long> t__ = stajOsn_list.Select(x => x.ID).ToList();

            var stajLgot_list_t = db_temp.StajLgot.Where(x => t__.Contains(x.StajOsnID)).ToList();

            t__.Clear();

            var razd64_list = db_temp.FormsRSW2014_1_Razd_6_4.Where(x => t2.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).ToList();
            var razd66_list = db_temp.FormsRSW2014_1_Razd_6_6.Where(x => t2.Contains(x.FormsRSW2014_1_Razd_6_1_ID)).ToList();
            var razd67_list = db_temp.FormsRSW2014_1_Razd_6_7.Where(x => t2.Contains(x.FormsRSW2014_1_Razd_6_1_ID)).ToList();


            foreach (var staff in staffTempList) // перебираем всех сотрудников попавших в эту пачку
            {
                //                FormsRSW2014_1_Razd_6_1 rsw61 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                FormsRSW2014_1_Razd_6_1 rsw61 = rsw61List_tmp.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);


                //                Staff ish_staff = db.Staff.FirstOrDefault(x => x.ID == staff.StaffID);
                Staff ish_staff = StaffList.First(x => x.ID == staff.StaffID);
                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                XElement СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ = new XElement("СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ",
                                                                            new XElement("НомерВпачке", staff.Num.Value + 1),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                            new XElement("ФИО",
                                                                                new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));
                // СведенияОбУвольнении РСВ-1 Р6 2015 об увольнении застрахованного лица
                if (yearType == 2015 && ish_staff.Dismissed.HasValue && ish_staff.Dismissed.Value == 1)
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СведенияОбУвольнении", "УВОЛЕН"));
                }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ОтчетныйПериод",
                                                                new XElement("Квартал", rsw61.Quarter),
                                                                new XElement("Год", rsw61.Year)));
                int m = 0;
                switch (rsw61.Quarter)
                {
                    case 0:
                        m = 10;
                        break;
                    case 3:
                        m = 1;
                        break;
                    case 6:
                        m = 4;
                        break;
                    case 9:
                        m = 7;
                        break;
                }

                if (rsw61.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", rsw61.QuarterKorr.Value),
                                                                   new XElement("Год", rsw61.YearKorr.Value)));
                    switch (rsw61.QuarterKorr)
                    {
                        case 0:
                            m = 10;
                            break;
                        case 3:
                            m = 1;
                            break;
                        case 6:
                            m = 4;
                            break;
                        case 9:
                            m = 7;
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(rsw61.RegNumKorr))
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("РегистрационныйНомерКорректируемогоПериода", Utils.ParseRegNum(rsw61.RegNumKorr)));
                }

                //                var rsw64_list = rsw61.FormsRSW2014_1_Razd_6_4;
                var rsw64_list = razd64_list.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw61.ID).ToList();

                int i = 1;
                int k = 0;

                try
                {
                    foreach (var rsw64 in rsw64_list)
                    {
                        string code = PlatCatList.First(x => x.ID == rsw64.PlatCategoryID).Code;

                        XElement СведенияОсуммеВыплатИвознагражденийВпользуЗЛ = new XElement("СведенияОсуммеВыплатИвознагражденийВпользуЗЛ");

                        СведенияОсуммеВыплатИвознагражденийВпользуЗЛ.Add(new XElement("НомерСтроки", i),
                                                                         new XElement("ТипСтроки", "ИТОГ"),
                                                                         new XElement("КодСтроки", "4" + k.ToString() + "0"),
                                                                         new XElement("КодКатегории", code));
                        if ((rsw64.s_0_0.HasValue && rsw64.s_0_0.Value > 0) || (rsw64.s_0_1.HasValue && rsw64.s_0_1.Value > 0) || (rsw64.s_0_1.HasValue && rsw64.s_0_1.Value > 0) || (rsw64.s_0_1.HasValue && rsw64.s_0_1.Value > 0))
                        {
                            СведенияОсуммеВыплатИвознагражденийВпользуЗЛ.Add(new XElement("СуммаВыплатИныхВознаграждений", rsw64.s_0_0.HasValue ? Utils.decToStr(rsw64.s_0_0.Value) : "0.00"),
                                                                             new XElement("НеПревышающиеВсего", rsw64.s_0_1.HasValue ? Utils.decToStr(rsw64.s_0_1.Value) : "0.00"),
                                                                             new XElement("НеПревышающиеПоДоговорам", rsw64.s_0_2.HasValue ? Utils.decToStr(rsw64.s_0_2.Value) : "0.00"),
                                                                             new XElement("ПревышающиеПредельную", rsw64.s_0_3.HasValue ? Utils.decToStr(rsw64.s_0_3.Value) : "0.00"));
                        }

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОсуммеВыплатИвознагражденийВпользуЗЛ);
                        i++;


                        for (int j = 1; j <= 3; j++)
                        {
                            string itemName = "s_" + j.ToString() + "_";

                            if (((rsw64.GetType().GetProperty(itemName + "0").GetValue(rsw64, null) != null) && (decimal.Parse(rsw64.GetType().GetProperty(itemName + "0").GetValue(rsw64, null).ToString()) != 0)) || ((rsw64.GetType().GetProperty(itemName + "1").GetValue(rsw64, null) != null) && (decimal.Parse(rsw64.GetType().GetProperty(itemName + "1").GetValue(rsw64, null).ToString()) != 0)) || ((rsw64.GetType().GetProperty(itemName + "2").GetValue(rsw64, null) != null) && (decimal.Parse(rsw64.GetType().GetProperty(itemName + "2").GetValue(rsw64, null).ToString()) != 0)) || ((rsw64.GetType().GetProperty(itemName + "3").GetValue(rsw64, null) != null) && (decimal.Parse(rsw64.GetType().GetProperty(itemName + "3").GetValue(rsw64, null).ToString()) != 0)))
                            {
                                СведенияОсуммеВыплатИвознагражденийВпользуЗЛ = new XElement("СведенияОсуммеВыплатИвознагражденийВпользуЗЛ");


                                СведенияОсуммеВыплатИвознагражденийВпользуЗЛ.Add(new XElement("НомерСтроки", i),
                                                                                 new XElement("ТипСтроки", "МЕСЦ"),
                                                                                 new XElement("Месяц", m + j - 1),
                                                                                 new XElement("КодСтроки", "4" + k.ToString() + j.ToString()),
                                                                                 new XElement("КодКатегории", code),
                                                                                 new XElement("СуммаВыплатИныхВознаграждений", rsw64.GetType().GetProperty(itemName + "0").GetValue(rsw64, null) != null ? Utils.decToStr(decimal.Parse(rsw64.GetType().GetProperty(itemName + "0").GetValue(rsw64, null).ToString())) : "0.00"),
                                                                                 new XElement("НеПревышающиеВсего", rsw64.GetType().GetProperty(itemName + "1").GetValue(rsw64, null) != null ? Utils.decToStr(decimal.Parse(rsw64.GetType().GetProperty(itemName + "1").GetValue(rsw64, null).ToString())) : "0.00"),
                                                                                 new XElement("НеПревышающиеПоДоговорам", rsw64.GetType().GetProperty(itemName + "2").GetValue(rsw64, null) != null ? Utils.decToStr(decimal.Parse(rsw64.GetType().GetProperty(itemName + "2").GetValue(rsw64, null).ToString())) : "0.00"),
                                                                                 new XElement("ПревышающиеПредельную", rsw64.GetType().GetProperty(itemName + "3").GetValue(rsw64, null) != null ? Utils.decToStr(decimal.Parse(rsw64.GetType().GetProperty(itemName + "3").GetValue(rsw64, null).ToString())) : "0.00"));

                                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОсуммеВыплатИвознагражденийВпользуЗЛ);
                                i++;
                            }
                        }

                        k++;
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении ПФР инд.сведений произошла ошибка при обработке Раздела 6.4.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));
                }



                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СуммаВзносовНаОПС", rsw61.SumFeePFR.HasValue ? Utils.decToStr(rsw61.SumFeePFR.Value) : "0.00"));

                //                var rsw66_list = rsw61.FormsRSW2014_1_Razd_6_6.OrderBy(x => x.AccountPeriodYear);
                var rsw66_list = razd66_list.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw61.ID).ToList();

                try
                {
                    if (rsw66_list.Count() > 0)
                    {
                        var years66 = rsw66_list.Select(x => x.AccountPeriodYear).Distinct().OrderBy(x => x.Value);
                        List<razd66Period> temp66List = rsw66_list.Select(x => new razd66Period { ID = x.ID, Y = x.AccountPeriodYear.HasValue ? x.AccountPeriodYear.Value : (short)0, Q = x.AccountPeriodQuarter.HasValue ? (x.AccountPeriodQuarter.Value != 0 ? x.AccountPeriodQuarter.Value : (byte)12) : (byte)0 }).ToList();
                        i = 0;
                        decimal[] sum = new decimal[3] { 0, 0, 0 };
                        List<XElement> korrList = new List<XElement>();

                        foreach (var year66 in years66)
                        {
                            //var rsw66_list_part = rsw66_list.Where(x => x.AccountPeriodYear == year66).OrderBy(x => x.AccountPeriodQuarter);
                            var rsw66_list_part = temp66List.Where(x => x.Y == year66).OrderBy(x => x.Q);
                            foreach (var itemID in rsw66_list_part)
                            {
                                var rsw66 = rsw66_list.FirstOrDefault(x => x.ID == itemID.ID);
                                i++;
                                XElement СведенияОкорректировках = new XElement("СведенияОкорректировках");

                                СведенияОкорректировках.Add(new XElement("НомерСтроки", i),
                                                                new XElement("ТипСтроки", "МЕСЦ"),
                                                                new XElement("Квартал", rsw66.AccountPeriodQuarter.HasValue ? rsw66.AccountPeriodQuarter.Value.ToString() : ""),
                                                                new XElement("Год", rsw66.AccountPeriodYear.HasValue ? rsw66.AccountPeriodYear.Value.ToString() : ""),
                                                                new XElement("СуммаДоначисленныхВзносовОПС", rsw66.SumFeePFR_D.HasValue ? Utils.decToStr(rsw66.SumFeePFR_D.Value) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаСтраховую", rsw66.SumFeePFR_StrahD.HasValue ? Utils.decToStr(rsw66.SumFeePFR_StrahD.Value) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаНакопительную", rsw66.SumFeePFR_NakopD.HasValue ? Utils.decToStr(rsw66.SumFeePFR_NakopD.Value) : "0.00"));

                                sum[0] = sum[0] + (rsw66.SumFeePFR_D.HasValue ? rsw66.SumFeePFR_D.Value : 0);
                                sum[1] = sum[1] + (rsw66.SumFeePFR_StrahD.HasValue ? rsw66.SumFeePFR_StrahD.Value : 0);
                                sum[2] = sum[2] + (rsw66.SumFeePFR_NakopD.HasValue ? rsw66.SumFeePFR_NakopD.Value : 0);

                                //   korrList.Add(СведенияОкорректировках);
                                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОкорректировках);
                            }
                        }

                        i++;
                        XElement СведенияОкорректировках_ИТОГ = new XElement("СведенияОкорректировках");

                        СведенияОкорректировках_ИТОГ.Add(new XElement("НомерСтроки", i),
                                                        new XElement("ТипСтроки", "ИТОГ"),
                                                        new XElement("СуммаДоначисленныхВзносовОПС", Utils.decToStr(sum[0])),
                                                        new XElement("СуммаДоначисленныхВзносовНаСтраховую", Utils.decToStr(sum[1])),
                                                        new XElement("СуммаДоначисленныхВзносовНаНакопительную", Utils.decToStr(sum[2])));

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОкорректировках_ИТОГ);

                        /*        foreach (var korrItem in korrList)
                                {
                                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(korrItem);
                                }*/

                    }
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении ПФР инд.сведений произошла ошибка при обработке Раздела 6.6.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));
                }


                //                var rsw67_list = rsw61.FormsRSW2014_1_Razd_6_7;
                var rsw67_list = razd67_list.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw61.ID).ToList();
                i = 1;
                k = 0;

                try
                {
                    foreach (var rsw67 in rsw67_list.Where(x => (x.s_0_0.HasValue && x.s_0_0.Value > 0) || (x.s_0_1.HasValue && x.s_0_1.Value > 0)))
                    {
                        XElement СведенияОсуммеВыплатИвознагражденийПоДопТарифу = new XElement("СведенияОсуммеВыплатИвознагражденийПоДопТарифу");

                        string code = rsw67.SpecOcenkaUslTrudaID.HasValue ? SpecOcenkaUslTruda_list.First(x => x.ID == rsw67.SpecOcenkaUslTrudaID).Code : "";


                        СведенияОсуммеВыплатИвознагражденийПоДопТарифу.Add(new XElement("НомерСтроки", i),
                                                                         new XElement("ТипСтроки", "ИТОГ"),
                                                                         new XElement("КодСтроки", "7" + k.ToString() + "0"),
                                                                         new XElement("КодСпециальнойОценкиУсловийТруда", code),
                                                                         new XElement("СуммаВыплатПоДопТарифу27-1", rsw67.s_0_0.HasValue ? Utils.decToStr(rsw67.s_0_0.Value) : "0.00"),
                                                                         new XElement("СуммаВыплатПоДопТарифу27-2-18", rsw67.s_0_1.HasValue ? Utils.decToStr(rsw67.s_0_1.Value) : "0.00"));

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОсуммеВыплатИвознагражденийПоДопТарифу);
                        i++;

                        for (int j = 1; j <= 3; j++)
                        {
                            string itemName = "s_" + j.ToString() + "_";

                            if ((rsw67.GetType().GetProperty(itemName + "0").GetValue(rsw67, null) != null && decimal.Parse(rsw67.GetType().GetProperty(itemName + "0").GetValue(rsw67, null).ToString()) != 0) || (rsw67.GetType().GetProperty(itemName + "1").GetValue(rsw67, null) != null && decimal.Parse(rsw67.GetType().GetProperty(itemName + "1").GetValue(rsw67, null).ToString()) != 0))
                            {
                                СведенияОсуммеВыплатИвознагражденийПоДопТарифу = new XElement("СведенияОсуммеВыплатИвознагражденийПоДопТарифу");

                                СведенияОсуммеВыплатИвознагражденийПоДопТарифу.Add(new XElement("НомерСтроки", i),
                                                                                 new XElement("ТипСтроки", "МЕСЦ"),
                                                                                 new XElement("Месяц", m + j - 1),
                                                                                 new XElement("КодСтроки", "7" + k.ToString() + j.ToString()),
                                                                                 new XElement("КодСпециальнойОценкиУсловийТруда", code),
                                                                                 new XElement("СуммаВыплатПоДопТарифу27-1", rsw67.GetType().GetProperty(itemName + "0").GetValue(rsw67, null) != null ? Utils.decToStr(decimal.Parse(rsw67.GetType().GetProperty(itemName + "0").GetValue(rsw67, null).ToString())) : "0.00"),
                                                                                 new XElement("СуммаВыплатПоДопТарифу27-2-18", rsw67.GetType().GetProperty(itemName + "1").GetValue(rsw67, null) != null ? Utils.decToStr(decimal.Parse(rsw67.GetType().GetProperty(itemName + "1").GetValue(rsw67, null).ToString())) : "0.00"));

                                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОсуммеВыплатИвознагражденийПоДопТарифу);
                                i++;
                            }
                        }

                        k++;
                    }
                }
                catch (Exception ex)
                {
                    this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении ПФР инд.сведений произошла ошибка при обработке Раздела 6.7.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));
                }
                //                               var staj_osn_list = rsw61.StajOsn.OrderBy(x => x.Number.Value);

                var staj_osn_list = stajOsn_list.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw61.ID).OrderBy(x => x.Number.Value).ToList();

                var tt = staj_osn_list.Select(x => x.ID).ToList();
                var stajLgot_list = stajLgot_list_t.Where(x => tt.Contains(x.StajOsnID)).ToList(); // db_temp
                i = 0;
                foreach (var staj_osn in staj_osn_list)
                {
                    try
                    {
                        i++;
                        XElement СтажевыйПериод = createStajElement(staj_osn, stajLgot_list, i);
                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СтажевыйПериод);
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении ПФР инд.сведений произошла ошибка при обработке стажа.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));
                    }

                }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ДатаЗаполнения", rsw61.DateFilling.ToShortDateString()));


                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ);
            }

            #endregion

            //      if (yearType == 2015)
            //          xDoc.Root.SetDefaultXmlNamespace(pfr);

            xDoc.Root.SetDefaultXmlNamespace(pfr);


            //     xml = xDoc.ToString();

            staffTempList.Clear();
            t = new List<long>();
            StaffList.Clear();

            rsw61List_tmp.Clear();
            t = new List<long>();

            stajOsn_list.Clear();
            stajLgot_list_t.Clear();

            //         razd64_list = new List<FormsRSW2014_1_Razd_6_4>();
            //        razd66_list = new List<FormsRSW2014_1_Razd_6_6>();
            //         razd67_list = new List<FormsRSW2014_1_Razd_6_7>();

            db_temp.Dispose();
            dbxml_temp.Dispose();
            return xDoc;
        }

        private XDocument generateXML_SZV_6_4(xmlInfo itemT)
        {
            pu6Entities db_temp = new pu6Entities();
            pfrXMLEntities dbxml_temp = new pfrXMLEntities();

            xmlInfo item = dbxml_temp.xmlInfo.First(x => x.ID == itemT.ID);

            Insurer ins = db_temp.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            //            string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ = new XElement("ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ",
                                                                  new XElement("НомерВпачке", 1),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));


            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRIP))
            {
                СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRUL))
            {
                СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            }

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length).ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length).ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));


            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(СоставительПачки);
            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ТипСведений", InfoType));


            var t = item.StaffList.Select(x => x.StaffID.Value).ToList();
            var StaffList = db_temp.Staff.Where(x => t.Contains(x.ID)).ToList();

            var t2 = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

            var temp_list = db_temp.FormsSZV_6_4.Where(x => t2.Contains(x.ID)).ToList();
            FormsSZV_6_4 szv_6_4_t = temp_list.First();

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КодКатегории", szv_6_4_t.PlatCategory.Code.ToUpper()));

            //            string periodName = "";
            //            string periodNameKorr = "";

            List<int> monthes = new List<int>();
            RaschetPeriodContainer rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.Quarter && x.Year == item.Year);
            if (rp != null)
            {
                for (int i = rp.DateBegin.Month; i <= rp.DateEnd.Month; i++)
                {
                    monthes.Add(i);
                }
            }

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ОтчетныйПериод",
                                                           new XElement("Квартал", item.Quarter.Value),
                                                           new XElement("Год", item.Year.Value)));

            if (item.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
            {
                //             rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.QuarterKorr && x.Year == item.YearKorr);
                //             if (rp != null)
                //                 periodNameKorr = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();
                monthes = new List<int>();
                rp = Options.RaschetPeriodInternal2010_2013.FirstOrDefault(x => x.Kvartal == item.QuarterKorr && x.Year == item.YearKorr);
                if (rp != null)
                {
                    for (int i = rp.DateBegin.Month; i <= rp.DateEnd.Month; i++)
                    {
                        monthes.Add(i);
                    }
                }


                ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                               new XElement("Квартал", item.QuarterKorr.Value),
                                                               new XElement("Год", item.YearKorr.Value)));
            }


            byte typeContract = szv_6_4_t.TypeContract;

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ТипДоговора", typeContract == 1 ? "ТРУДОВОЙ" : "ГРАЖДАНСКО-ПРАВОВОЙ"));
            szv_6_4_t = null;



            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВыплатИвознагражденийВпользуЗЛ",
                                                                           new XElement("ТипСтроки", "ИТОГ"),
                                                                           new XElement("СуммаВыплатВсего", Utils.decToStr(temp_list.Where(x => x.s_0_0.HasValue).Sum(x => x.s_0_0.Value))),
                                                                           new XElement("СуммаВыплатНачисленыСтраховыеВзносыНеПревышающие", Utils.decToStr(temp_list.Where(x => x.s_0_1.HasValue).Sum(x => x.s_0_1.Value))),
                                                                           new XElement("СуммаВыплатНачисленыСтраховыеВзносыПревышающие", Utils.decToStr(temp_list.Where(x => x.s_0_2.HasValue).Sum(x => x.s_0_2.Value)))));

            if (InfoType != "ОТМЕНЯЮЩАЯ")
            {

                ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                               new XElement("Начислено", Utils.decToStr(temp_list.Where(x => x.SumFeePFR_Strah.HasValue).Sum(x => x.SumFeePFR_Strah.Value))),
                                                                               new XElement("Уплачено", Utils.decToStr(temp_list.Where(x => x.SumPayPFR_Strah.HasValue).Sum(x => x.SumPayPFR_Strah.Value)))));

                ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаНакопительную",
                                                                               new XElement("Начислено", Utils.decToStr(temp_list.Where(x => x.SumFeePFR_Nakop.HasValue).Sum(x => x.SumFeePFR_Nakop.Value))),
                                                                               new XElement("Уплачено", Utils.decToStr(temp_list.Where(x => x.SumPayPFR_Nakop.HasValue).Sum(x => x.SumPayPFR_Nakop.Value)))));
            }

            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ);


            #region СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ

            var ttt = temp_list.Select(x => x.ID).ToList();
            var stajOsn_list = db_temp.StajOsn.Where(x => ttt.Contains(x.FormsSZV_6_4_ID.Value)).ToList();

            foreach (var staff in item.StaffList) // перебираем всех сотрудников попавших в эту пачку
            {

                FormsSZV_6_4 szv_6_4 = temp_list.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                Staff ish_staff = StaffList.FirstOrDefault(x => x.ID == staff.StaffID);

                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                string s_name = "";
                if (ins.TypePayer == 0) // если организация
                {
                    if (!String.IsNullOrEmpty(ins.NameShort))
                    {
                        s_name = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    }
                }
                else // если физ. лицо
                {
                    string FIO = "";
                    if (!String.IsNullOrEmpty(ins.LastName))
                    {
                        FIO = FIO + ins.LastName;
                    }
                    if (!String.IsNullOrEmpty(ins.FirstName))
                    {
                        FIO = FIO + " " + ins.FirstName;
                    }
                    if (!String.IsNullOrEmpty(ins.MiddleName))
                    {
                        FIO = FIO + " " + ins.MiddleName;
                    }

                    if (!String.IsNullOrEmpty(FIO))
                    {
                        s_name = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    }
                }

                XElement НаименованиеКраткое = new XElement("НаименованиеКраткое", s_name.ToUpper());

                XElement НалоговыйНомер = new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
                if (!String.IsNullOrEmpty(ins.KPP))
                {
                    НалоговыйНомер.Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                }

                XElement ОтчетныйПериод = new XElement("ОтчетныйПериод",
                                              new XElement("Квартал", szv_6_4.Quarter),
                                              new XElement("Год", szv_6_4.Year));


                XElement СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ = new XElement("СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ",
                                                                            new XElement("НомерВпачке", staff.Num.Value + 1),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            НаименованиеКраткое,
                                                                            НалоговыйНомер,
                                                                            new XElement("КодКатегории", szv_6_4.PlatCategory.Code.ToUpper()),
                                                                            ОтчетныйПериод);

                if (szv_6_4.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", szv_6_4.QuarterKorr.Value),
                                                                   new XElement("Год", szv_6_4.YearKorr.Value)));
                }

                if (!string.IsNullOrEmpty(szv_6_4.RegNumKorr))
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("РегистрационныйНомерКорректируемогоПериода", Utils.ParseRegNum(szv_6_4.RegNumKorr)));
                }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                                     new XElement("ФИО",
                                                                                         new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                         new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                         new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));
                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ТипДоговора", szv_6_4.TypeContract == 1 ? "ТРУДОВОЙ" : "ГРАЖДАНСКО-ПРАВОВОЙ"));

                if (InfoType != "ОТМЕНЯЮЩАЯ")
                {

                    XElement СуммаВыплатИвознагражденийВпользуЗЛ = new XElement("СуммаВыплатИвознагражденийВпользуЗЛ");

                    if (monthes.Count() < 3)
                    {
                        monthes = new List<int> { 1, 2, 3 };
                    }

                    for (int j = 1; j <= 3; j++)
                    {
                        string itemName1 = "s_" + j.ToString() + "_0";
                        string itemName2 = "s_" + j.ToString() + "_1";
                        string itemName3 = "s_" + j.ToString() + "_2";

                        СуммаВыплатИвознагражденийВпользуЗЛ = new XElement("СуммаВыплатИвознагражденийВпользуЗЛ");

                        СуммаВыплатИвознагражденийВпользуЗЛ.Add(new XElement("ТипСтроки", "МЕСЦ"),
                                                                new XElement("Месяц", monthes[j - 1]),
                                                                new XElement("СуммаВыплатВсего", szv_6_4.GetType().GetProperty(itemName1).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName1).GetValue(szv_6_4, null).ToString())) : "0.00"),
                                                                new XElement("СуммаВыплатНачисленыСтраховыеВзносыНеПревышающие", szv_6_4.GetType().GetProperty(itemName2).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName2).GetValue(szv_6_4, null).ToString())) : "0.00"),
                                                                new XElement("СуммаВыплатНачисленыСтраховыеВзносыПревышающие", szv_6_4.GetType().GetProperty(itemName3).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName3).GetValue(szv_6_4, null).ToString())) : "0.00"));

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СуммаВыплатИвознагражденийВпользуЗЛ);
                    }

                    СуммаВыплатИвознагражденийВпользуЗЛ = new XElement("СуммаВыплатИвознагражденийВпользуЗЛ");

                    СуммаВыплатИвознагражденийВпользуЗЛ.Add(new XElement("ТипСтроки", "ИТОГ"),
                                                                new XElement("СуммаВыплатВсего", szv_6_4.s_0_0.HasValue ? Utils.decToStr(szv_6_4.s_0_0.Value) : "0.00"),
                                                                new XElement("СуммаВыплатНачисленыСтраховыеВзносыНеПревышающие", szv_6_4.s_0_1.HasValue ? Utils.decToStr(szv_6_4.s_0_1.Value) : "0.00"),
                                                                new XElement("СуммаВыплатНачисленыСтраховыеВзносыПревышающие", szv_6_4.s_0_2.HasValue ? Utils.decToStr(szv_6_4.s_0_2.Value) : "0.00"));

                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СуммаВыплатИвознагражденийВпользуЗЛ);

                }

                // данные по выплате по доп тарифу
                if ((szv_6_4.d_0_0.HasValue && szv_6_4.d_0_0.Value != 0) || (szv_6_4.d_0_1.HasValue && szv_6_4.d_0_1.Value != 0))
                {
                    XElement СуммаВыплатИвознагражденийПоДопТарифу = new XElement("СуммаВыплатИвознагражденийПоДопТарифу");

                    for (int j = 1; j <= 3; j++)
                    {
                        string itemName1 = "d_" + j.ToString() + "_0";
                        string itemName2 = "d_" + j.ToString() + "_1";

                        СуммаВыплатИвознагражденийПоДопТарифу = new XElement("СуммаВыплатИвознагражденийПоДопТарифу");

                        СуммаВыплатИвознагражденийПоДопТарифу.Add(new XElement("ТипСтроки", "МЕСЦ"),
                                                                new XElement("Месяц", monthes[j - 1]),
                                                                new XElement("СуммаВыплатПоДопТарифу27-1", szv_6_4.GetType().GetProperty(itemName1).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName1).GetValue(szv_6_4, null).ToString())) : "0.00"),
                                                                new XElement("СуммаВыплатПоДопТарифу27-2-18", szv_6_4.GetType().GetProperty(itemName2).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName2).GetValue(szv_6_4, null).ToString())) : "0.00"));

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СуммаВыплатИвознагражденийПоДопТарифу);
                    }

                    СуммаВыплатИвознагражденийПоДопТарифу = new XElement("СуммаВыплатИвознагражденийПоДопТарифу");

                    СуммаВыплатИвознагражденийПоДопТарифу.Add(new XElement("ТипСтроки", "ИТОГ"),
                                                                new XElement("СуммаВыплатПоДопТарифу27-1", szv_6_4.d_0_0.HasValue ? Utils.decToStr(szv_6_4.d_0_0.Value) : "0.00"),
                                                                new XElement("СуммаВыплатПоДопТарифу27-2-18", szv_6_4.d_0_1.HasValue ? Utils.decToStr(szv_6_4.d_0_1.Value) : "0.00"));

                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СуммаВыплатИвознагражденийПоДопТарифу);
                }

                if (InfoType != "ОТМЕНЯЮЩАЯ")
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                                             new XElement("Начислено", szv_6_4.SumFeePFR_Strah.HasValue ? Utils.decToStr(szv_6_4.SumFeePFR_Strah.Value) : "0.00"),
                                                                                             new XElement("Уплачено", szv_6_4.SumPayPFR_Strah.HasValue ? Utils.decToStr(szv_6_4.SumPayPFR_Strah.Value) : "0.00")),
                                                                                         new XElement("СуммаВзносовНаНакопительную",
                                                                                             new XElement("Начислено", szv_6_4.SumFeePFR_Nakop.HasValue ? Utils.decToStr(szv_6_4.SumFeePFR_Nakop.Value) : "0.00"),
                                                                                             new XElement("Уплачено", szv_6_4.SumPayPFR_Nakop.HasValue ? Utils.decToStr(szv_6_4.SumPayPFR_Nakop.Value) : "0.00")));
                    var staj_osn_list = stajOsn_list.Where(x => x.FormsSZV_6_4_ID == szv_6_4.ID).OrderBy(x => x.Number.Value).ToList();

                    var tt = staj_osn_list.Select(x => x.ID).ToList();
                    var stajLgot_list = db_temp.StajLgot.Where(x => tt.Contains(x.StajOsnID)).ToList();

                    int i = 0;
                    foreach (var staj_osn in staj_osn_list)
                    {
                        try
                        {
                            i++;
                            XElement СтажевыйПериод = createStajElement(staj_osn, stajLgot_list, i);

                            СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СтажевыйПериод);
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении СЗВ-6-4 произошла ошибка при обработке стажа.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));

                        }
                    }
                }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ДатаЗаполнения", szv_6_4.DateFilling.ToShortDateString()));


                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ);
            }

            #endregion

            xDoc.Root.SetDefaultXmlNamespace(pfr);

            //            xml = xDoc.ToString();
            db_temp.Dispose();
            dbxml_temp.Dispose();

            return xDoc;
        }

        private XDocument generateXML_SZV_6_1(xmlInfo itemT)
        {
            pu6Entities db_temp = new pu6Entities();
            pfrXMLEntities dbxml_temp = new pfrXMLEntities();

            xmlInfo item = dbxml_temp.xmlInfo.First(x => x.ID == itemT.ID);
            Insurer ins = db_temp.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            //    string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ = new XElement("ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ",
                                                                  new XElement("НомерВпачке", 1),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            //if (!String.IsNullOrEmpty(ins.EGRIP))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            //}

            //if (!String.IsNullOrEmpty(ins.EGRUL))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            //}

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length).ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length).ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));


            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(СоставительПачки);
            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ТипСведений", InfoType));

            var t = item.StaffList.Select(x => x.StaffID.Value).ToList();
            var StaffList = db_temp.Staff.Where(x => t.Contains(x.ID));

            var t2 = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();
            var temp_list = db_temp.FormsSZV_6.Where(x => t2.Contains(x.ID)).ToList();

            FormsSZV_6 szv_6_t = temp_list.First();

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КодКатегории", szv_6_t.PlatCategory.Code));

            //            string periodName = "";
            //           string periodNameKorr = "";


            //            RaschetPeriodContainer rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.Quarter && x.Year == item.Year);
            //            if (rp != null)
            //                periodName = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ОтчетныйПериод",
                                                           new XElement("Квартал", item.Quarter.Value),
                                                           new XElement("Год", item.Year.Value)));

            if (item.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
            {
                //                rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.QuarterKorr && x.Year == item.YearKorr);
                //                if (rp != null)
                //                    periodNameKorr = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();

                ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                               new XElement("Квартал", item.QuarterKorr.Value),
                                                               new XElement("Год", item.YearKorr.Value)));
            }


            szv_6_t = null;



            if (InfoType != "ОТМЕНЯЮЩАЯ")
            {
                ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                               new XElement("Начислено", Utils.decToStr(temp_list.Sum(x => x.SumFeePFR_Strah.Value))),
                                                                               new XElement("Уплачено", Utils.decToStr(temp_list.Sum(x => x.SumPayPFR_Strah.Value)))));

                ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаНакопительную",
                                                                               new XElement("Начислено", Utils.decToStr(temp_list.Sum(x => x.SumFeePFR_Nakop.Value))),
                                                                               new XElement("Уплачено", Utils.decToStr(temp_list.Sum(x => x.SumPayPFR_Nakop.Value)))));
            }

            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ);


            #region СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ

            var ttt = temp_list.Select(x => x.ID).ToList();
            var stajOsn_list = db_temp.StajOsn.Where(x => ttt.Contains(x.FormsSZV_6_ID.Value)).ToList();

            foreach (var staff in item.StaffList) // перебираем всех сотрудников попавших в эту пачку
            {
                FormsSZV_6 szv_6 = temp_list.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                Staff ish_staff = StaffList.FirstOrDefault(x => x.ID == staff.StaffID);

                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                string s_name = "";
                if (ins.TypePayer == 0) // если организация
                {
                    if (!String.IsNullOrEmpty(ins.NameShort))
                    {
                        s_name = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    }
                }
                else // если физ. лицо
                {
                    string FIO = "";
                    if (!String.IsNullOrEmpty(ins.LastName))
                    {
                        FIO = FIO + ins.LastName;
                    }
                    if (!String.IsNullOrEmpty(ins.FirstName))
                    {
                        FIO = FIO + " " + ins.FirstName;
                    }
                    if (!String.IsNullOrEmpty(ins.MiddleName))
                    {
                        FIO = FIO + " " + ins.MiddleName;
                    }

                    if (!String.IsNullOrEmpty(FIO))
                    {
                        s_name = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    }
                }

                XElement НаименованиеКраткое = new XElement("НаименованиеКраткое", s_name.ToUpper());

                XElement НалоговыйНомер = new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
                if (!String.IsNullOrEmpty(ins.KPP))
                {
                    НалоговыйНомер.Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                }

                XElement ОтчетныйПериод = new XElement("ОтчетныйПериод",
                                              new XElement("Квартал", szv_6.Quarter),
                                              new XElement("Год", szv_6.Year));

                XElement СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ = new XElement("СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ",
                                                                            new XElement("НомерВпачке", staff.Num.Value + 1),
                                                                            new XElement("ВидФормы", "СЗВ-6-1"),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            НаименованиеКраткое,
                                                                            НалоговыйНомер,
                                                                            new XElement("КодКатегории", szv_6.PlatCategory.Code),
                                                                            ОтчетныйПериод);

                if (szv_6.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
                {
                    СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", szv_6.QuarterKorr.Value),
                                                                   new XElement("Год", szv_6.YearKorr.Value)));
                }


                СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                                     new XElement("ФИО",
                                                                                         new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                         new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                         new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));


                if (InfoType != "ОТМЕНЯЮЩАЯ")
                {
                    СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                              new XElement("Начислено", szv_6.SumFeePFR_Strah.HasValue ? Utils.decToStr(szv_6.SumFeePFR_Strah.Value) : "0.00"),
                                                                              new XElement("Уплачено", szv_6.SumPayPFR_Strah.HasValue ? Utils.decToStr(szv_6.SumPayPFR_Strah.Value) : "0.00")),
                                                                          new XElement("СуммаВзносовНаНакопительную",
                                                                              new XElement("Начислено", szv_6.SumFeePFR_Nakop.HasValue ? Utils.decToStr(szv_6.SumFeePFR_Nakop.Value) : "0.00"),
                                                                              new XElement("Уплачено", szv_6.SumPayPFR_Nakop.HasValue ? Utils.decToStr(szv_6.SumPayPFR_Nakop.Value) : "0.00")));
                }

                СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ДатаЗаполнения", szv_6.DateFilling.ToShortDateString()));

                if (InfoType != "ОТМЕНЯЮЩАЯ")
                {
                    var staj_osn_list = stajOsn_list.Where(x => x.FormsSZV_6_ID == szv_6.ID).OrderBy(x => x.Number.Value).ToList();

                    var tt = staj_osn_list.Select(x => x.ID).ToList();
                    var stajLgot_list = db_temp.StajLgot.Where(x => tt.Contains(x.StajOsnID)).ToList();

                    int i = 0;
                    foreach (var staj_osn in staj_osn_list)
                    {
                        try
                        {
                            i++;
                            XElement СтажевыйПериод = createStajElement(staj_osn, stajLgot_list, i);
                            СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СтажевыйПериод);
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении СЗВ-6-1 произошла ошибка при обработке стажа.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));
                        }
                    }
                }

                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ);
            }

            #endregion

            xDoc.Root.SetDefaultXmlNamespace(pfr);

            //            xml = xDoc.ToString();
            db_temp.Dispose();
            dbxml_temp.Dispose();
            return xDoc;
        }

        private XDocument generateXML_SZV_6_2(xmlInfo itemT)
        {
            pu6Entities db_temp = new pu6Entities();
            pfrXMLEntities dbxml_temp = new pfrXMLEntities();

            xmlInfo item = dbxml_temp.xmlInfo.First(x => x.ID == itemT.ID);
            //         string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ = new XElement("ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ",
                                                                  new XElement("НомерВпачке", 1),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            Insurer ins = db_temp.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            //if (!String.IsNullOrEmpty(ins.EGRIP))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            //}

            //if (!String.IsNullOrEmpty(ins.EGRUL))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            //}

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length).ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length).ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));


            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(СоставительПачки);
            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ТипСведений", InfoType));


            var t = item.StaffList.Select(x => x.StaffID.Value).ToList();
            var StaffList = db_temp.Staff.Where(x => t.Contains(x.ID)).ToList();
            var t2 = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

            var temp_list = db_temp.FormsSZV_6.Where(x => t2.Contains(x.ID)).ToList();

            FormsSZV_6 szv_6_t = temp_list.First();




            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КодКатегории", szv_6_t.PlatCategory.Code));

            //            string periodName = "";
            //           string periodNameKorr = "";


            //            RaschetPeriodContainer rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.Quarter && x.Year == item.Year);
            //            if (rp != null)
            //                periodName = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ОтчетныйПериод",
                                                           new XElement("Квартал", item.Quarter.Value),
                                                           new XElement("Год", item.Year.Value)));

            if (item.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
            {
                //                rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.QuarterKorr && x.Year == item.YearKorr);
                //                if (rp != null)
                //                    periodNameKorr = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();

                ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                               new XElement("Квартал", item.QuarterKorr.Value),
                                                               new XElement("Год", item.YearKorr.Value)));
            }


            szv_6_t = null;

            if (InfoType != "ОТМЕНЯЮЩАЯ")
            {

                ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                               new XElement("Начислено", Utils.decToStr(temp_list.Sum(x => x.SumFeePFR_Strah.Value))),
                                                                               new XElement("Уплачено", Utils.decToStr(temp_list.Sum(x => x.SumPayPFR_Strah.Value)))));

                ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаНакопительную",
                                                                               new XElement("Начислено", Utils.decToStr(temp_list.Sum(x => x.SumFeePFR_Nakop.Value))),
                                                                               new XElement("Уплачено", Utils.decToStr(temp_list.Sum(x => x.SumPayPFR_Nakop.Value)))));
            }

            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ);


            #region СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ

            var ttt = temp_list.Select(x => x.ID).ToList();
            var stajOsn_list = db.StajOsn.Where(x => ttt.Contains(x.FormsSZV_6_ID.Value)).ToList();

            foreach (var staff in item.StaffList) // перебираем всех сотрудников попавших в эту пачку
            {
                FormsSZV_6 szv_6 = temp_list.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                Staff ish_staff = StaffList.FirstOrDefault(x => x.ID == staff.StaffID);

                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                string s_name = "";
                if (ins.TypePayer == 0) // если организация
                {
                    if (!String.IsNullOrEmpty(ins.NameShort))
                    {
                        s_name = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    }
                }
                else // если физ. лицо
                {
                    string FIO = "";
                    if (!String.IsNullOrEmpty(ins.LastName))
                    {
                        FIO = FIO + ins.LastName;
                    }
                    if (!String.IsNullOrEmpty(ins.FirstName))
                    {
                        FIO = FIO + " " + ins.FirstName;
                    }
                    if (!String.IsNullOrEmpty(ins.MiddleName))
                    {
                        FIO = FIO + " " + ins.MiddleName;
                    }

                    if (!String.IsNullOrEmpty(FIO))
                    {
                        s_name = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    }
                }

                XElement НаименованиеКраткое = new XElement("НаименованиеКраткое", s_name.ToUpper());

                XElement НалоговыйНомер = new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
                if (!String.IsNullOrEmpty(ins.KPP))
                {
                    НалоговыйНомер.Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                }

                XElement ОтчетныйПериод = new XElement("ОтчетныйПериод",
                                              new XElement("Квартал", szv_6.Quarter),
                                              new XElement("Год", szv_6.Year));

                XElement СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ = new XElement("СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ",
                                                                            new XElement("НомерВпачке", staff.Num.Value + 1),
                                                                            new XElement("ВидФормы", "СЗВ-6-2"),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            НаименованиеКраткое,
                                                                            НалоговыйНомер,
                                                                            new XElement("КодКатегории", szv_6.PlatCategory.Code),
                                                                            ОтчетныйПериод);

                if (szv_6.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
                {
                    СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", szv_6.QuarterKorr.Value),
                                                                   new XElement("Год", szv_6.YearKorr.Value)));
                }


                СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                                     new XElement("ФИО",
                                                                                         new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                         new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                         new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));


                if (InfoType != "ОТМЕНЯЮЩАЯ")
                {
                    СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                              new XElement("Начислено", szv_6.SumFeePFR_Strah.HasValue ? Utils.decToStr(szv_6.SumFeePFR_Strah.Value) : "0.00"),
                                                                              new XElement("Уплачено", szv_6.SumPayPFR_Strah.HasValue ? Utils.decToStr(szv_6.SumPayPFR_Strah.Value) : "0.00")),
                                                                          new XElement("СуммаВзносовНаНакопительную",
                                                                              new XElement("Начислено", szv_6.SumFeePFR_Nakop.HasValue ? Utils.decToStr(szv_6.SumFeePFR_Nakop.Value) : "0.00"),
                                                                              new XElement("Уплачено", szv_6.SumPayPFR_Nakop.HasValue ? Utils.decToStr(szv_6.SumPayPFR_Nakop.Value) : "0.00")));
                }

                СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ДатаЗаполнения", szv_6.DateFilling.ToShortDateString()));

                if (InfoType != "ОТМЕНЯЮЩАЯ")
                {
                    var staj_osn_list = stajOsn_list.Where(x => x.FormsSZV_6_ID == szv_6.ID).OrderBy(x => x.Number.Value).ToList();

                    var tt = staj_osn_list.Select(x => x.ID).ToList();
                    var stajLgot_list = db.StajLgot.Where(x => tt.Contains(x.StajOsnID)).ToList();

                    int i = 0;
                    foreach (var staj_osn in staj_osn_list)
                    {
                        try
                        {
                            i++;
                            XElement СтажевыйПериод = createStajElement(staj_osn, stajLgot_list, i);
                            СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СтажевыйПериод);
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new Action(() => { Methods.showAlert("Внимание!", "При сохранении СЗВ-6-2 произошла ошибка при обработке стажа.\r\nКод ошибки: " + ex.Message, this.ThemeName); }));
                        }
                    }
                }
                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ);
            }

            #endregion

            xDoc.Root.SetDefaultXmlNamespace(pfr);

            //            xml = xDoc.ToString();
            return xDoc;
        }

        /// <summary>
        /// Формирование ветки XML с информацией о стаже для СЗВ-СТАЖ   ТипСтажевыйПериод2017
        /// </summary>
        /// <param name="staj_osn"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private XElement createStajElement_2017(StajOsn staj_osn, List<StajLgot> stajLgot_list, int i, XNamespace pfr, XNamespace УТ2, XNamespace ИС2)
        {

            //var staj_lgot_list = staj_osn.StajLgot.ToList().OrderBy(x => x.Number.Value);

            var staj_lgot_list = stajLgot_list.Where(x => x.StajOsnID == staj_osn.ID).ToList().OrderBy(x => x.Number.Value);

            XElement СтажевыйПериод = new XElement(pfr + "СтажевыйПериод");


            XElement Период = new XElement(ИС2 + "Период");

            Период.Add(new XElement(УТ2 + "С", staj_osn.DateBegin.Value.ToString("yyyy-MM-dd")),
                       new XElement(УТ2 + "По", staj_osn.DateEnd.Value.ToString("yyyy-MM-dd")));

            СтажевыйПериод.Add(Период);


            XNamespace ВС2 = "http://пф.рф/ВС/типы/2017-10-23";

            foreach (var staj_lgot in staj_lgot_list)
            {
                bool recNotNull = false;

                XElement ЛьготныйСтаж = new XElement(ИС2 + "ЛьготныйСтаж");

                if (staj_lgot.TerrUslID != null)
                {
                    recNotNull = true;
                    var t = TerrUsl_list.First(x => x.ID == staj_lgot.TerrUslID.Value).Code;
                    ЛьготныйСтаж.Add(new XElement(ИС2 + "ТУ",
                                             new XElement(ИС2 + "Основание", t)));//staj_lgot.TerrUsl.Code

                    if (t.Substring(0, 1) != "Ч" && !t.Contains("СЕЛО") && staj_lgot.TerrUslKoef.HasValue && staj_lgot.TerrUslKoef.Value != 0)//staj_lgot.TerrUsl.Code
                    {
                        ЛьготныйСтаж.Element(ИС2 + "ТУ").Add(new XElement(ИС2 + "Коэффициент", Utils.decToStr(staj_lgot.TerrUslKoef.Value)));
                    }
                }



                if (staj_lgot.OsobUslTrudaID != null)
                {
                    recNotNull = true;
                    XElement ОУТ = new XElement(ИС2 + "ОУТ",
                                             new XElement(ИС2 + "Код", OsobUslTruda_list.First(x => x.ID == staj_lgot.OsobUslTrudaID.Value).Code));//staj_lgot.OsobUslTruda.Code

                    if (staj_lgot.KodVred_OsnID != null)
                    {
                        ОУТ.Add(new XElement(ИС2 + "ПозицияСписка", KodVred_2_list.First(x => x.ID == staj_lgot.KodVred_OsnID.Value).Code));//staj_lgot.KodVred_2.Code
                    }

                    ЛьготныйСтаж.Add(ОУТ);
                }



                if (staj_lgot.IschislStrahStajOsnID != null || (staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0) || (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0))
                {
                    recNotNull = true;
                    XElement ИсчисляемыйСтаж = new XElement(ИС2 + "ИС");

                    if (staj_lgot.IschislStrahStajOsnID != null)
                    {
                        var t = IschislStrahStajOsn_list.First(x => x.ID == staj_lgot.IschislStrahStajOsnID.Value).Code;
                        ИсчисляемыйСтаж.Add(new XElement(ИС2 + "Основание", t));//staj_lgot.IschislStrahStajOsn.Code
                        if (t == "ВОДОЛАЗ")
                        {
                            if ((staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0) || (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0))
                            {
                                XElement ВыработкаВчасах = new XElement(ВС2 + "ВыработкаВчасах");
                                if (staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0)
                                    ВыработкаВчасах.Add(new XElement(ВС2 + "Часы", staj_lgot.Strah1Param.Value));
                                if (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0)
                                    ВыработкаВчасах.Add(new XElement(ВС2 + "Минуты", staj_lgot.Strah2Param.Value));

                                ИсчисляемыйСтаж.Add(ВыработкаВчасах);
                            }
                        }
                        else
                        {
                            if ((staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0) || (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0))
                            {
                                XElement ВыработкаКалендарная = new XElement(ВС2 + "ВыработкаКалендарная");
                                if (staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0)
                                    ВыработкаКалендарная.Add(new XElement(ВС2 + "ВсеМесяцы", staj_lgot.Strah1Param.Value));
                                if (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0)
                                    ВыработкаКалендарная.Add(new XElement(ВС2 + "ВсеДни", staj_lgot.Strah2Param.Value));

                                ИсчисляемыйСтаж.Add(ВыработкаКалендарная);
                            }
                        }
                    }
                    else if ((staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0) || (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0))
                    {
                        XElement ВыработкаКалендарная = new XElement(ВС2 + "ВыработкаКалендарная");
                        if (staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0)
                            ВыработкаКалендарная.Add(new XElement(ВС2 + "ВсеМесяцы", staj_lgot.Strah1Param.Value));
                        if (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0)
                            ВыработкаКалендарная.Add(new XElement(ВС2 + "ВсеДни", staj_lgot.Strah2Param.Value));

                        ИсчисляемыйСтаж.Add(ВыработкаКалендарная);
                    }

                    ЛьготныйСтаж.Add(ИсчисляемыйСтаж);
                }

                if (staj_lgot.IschislStrahStajDopID != null)
                {
                    recNotNull = true;

                    ЛьготныйСтаж.Add(new XElement(ИС2 + "ДопСведенияИС", IschislStrahStajDop_list.First(x => x.ID == staj_lgot.IschislStrahStajDopID.Value).Code));//staj_lgot.IschislStrahStajDop.Code
                }

                if (staj_lgot.UslDosrNaznID != null)
                {
                    recNotNull = true;

                    XElement ВЛ = new XElement(ИС2 + "ВЛ",
                                             new XElement(ИС2 + "Основание", UslDosrNazn_list.First(x => x.ID == staj_lgot.UslDosrNaznID.Value).Code));//staj_lgot.UslDosrNazn.Code

                    if (staj_lgot.UslDosrNazn.Code == "27-15")
                    {
                        if ((staj_lgot.UslDosrNazn1Param.HasValue && staj_lgot.UslDosrNazn1Param.Value != 0) || (staj_lgot.UslDosrNazn2Param.HasValue && staj_lgot.UslDosrNazn2Param.Value != 0))
                        {
                            XElement ВыработкаКалендарная = new XElement(ВС2 + "ВыработкаКалендарная");
                            if (staj_lgot.UslDosrNazn1Param.HasValue && staj_lgot.UslDosrNazn1Param.Value != 0)
                                ВыработкаКалендарная.Add(new XElement(ВС2 + "ВсеМесяцы", staj_lgot.UslDosrNazn1Param.Value));
                            if (staj_lgot.UslDosrNazn2Param.HasValue && staj_lgot.UslDosrNazn2Param.Value != 0)
                                ВыработкаКалендарная.Add(new XElement(ВС2 + "ВсеДни", staj_lgot.UslDosrNazn2Param.Value));

                            ВЛ.Add(ВыработкаКалендарная);
                        }
                    }
                    else
                    {
                        if ((staj_lgot.UslDosrNazn1Param.HasValue && staj_lgot.UslDosrNazn1Param.Value != 0) || (staj_lgot.UslDosrNazn2Param.HasValue && staj_lgot.UslDosrNazn2Param.Value != 0))
                        {
                            XElement ВыработкаВчасах = new XElement(ВС2 + "ВыработкаВчасах");
                            if (staj_lgot.UslDosrNazn1Param.HasValue && staj_lgot.UslDosrNazn1Param.Value != 0)
                                ВыработкаВчасах.Add(new XElement(ВС2 + "Часы", staj_lgot.UslDosrNazn1Param.Value));
                            if (staj_lgot.UslDosrNazn2Param.HasValue && staj_lgot.UslDosrNazn2Param.Value != 0)
                                ВыработкаВчасах.Add(new XElement(ВС2 + "Минуты", staj_lgot.UslDosrNazn2Param.Value));

                            ВЛ.Add(ВыработкаВчасах);
                        }
                    }

                    if (staj_lgot.UslDosrNazn3Param.HasValue && staj_lgot.UslDosrNazn3Param.Value != 0)
                    {
                        ВЛ.Add(new XElement(ИС2 + "ДоляСтавки", Utils.decToStr(staj_lgot.UslDosrNazn3Param.Value)));
                    }

                    ЛьготныйСтаж.Add(ВЛ);
                }


                if (recNotNull)
                    СтажевыйПериод.Add(ЛьготныйСтаж);
            }

            return СтажевыйПериод;
        }


        /// <summary>
        /// Формирование ветки XML с информацией о стаже для РСВ-1 (6 раздел), СЗВ-6-4, СЗВ-6
        /// </summary>
        /// <param name="staj_osn"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private XElement createStajElement(StajOsn staj_osn, List<StajLgot> stajLgot_list, int i)
        {

            //var staj_lgot_list = staj_osn.StajLgot.ToList().OrderBy(x => x.Number.Value);

            var staj_lgot_list = stajLgot_list.Where(x => x.StajOsnID == staj_osn.ID).ToList().OrderBy(x => x.Number.Value);


            XElement СтажевыйПериод = new XElement("СтажевыйПериод");

            СтажевыйПериод.Add(new XElement("НомерСтроки", i),
                                   new XElement("ДатаНачалаПериода", staj_osn.DateBegin.Value.ToShortDateString()),
                                   new XElement("ДатаКонцаПериода", staj_osn.DateEnd.Value.ToShortDateString()));

            int cnt = staj_lgot_list.Count();
            if (cnt > 0)
            {

                СтажевыйПериод.Add(new XElement("КоличествоЛьготныхСоставляющих", cnt.ToString()));

                int j = 0;
                foreach (var staj_lgot in staj_lgot_list)
                {
                    j++;
                    XElement ЛьготныйСтаж = new XElement("ЛьготныйСтаж",
                                                new XElement("НомерСтроки", j));

                    XElement ОсобенностиУчета = new XElement("ОсобенностиУчета");
                    if (staj_lgot.TerrUslID != null)
                    {
                        var t = TerrUsl_list.First(x => x.ID == staj_lgot.TerrUslID.Value).Code;
                        ОсобенностиУчета.Add(new XElement("ТерриториальныеУсловия",
                                                 new XElement("ОснованиеТУ", t)));//staj_lgot.TerrUsl.Code

                        if (t.Substring(0, 1) != "Ч" && !t.Contains("СЕЛО") && staj_lgot.TerrUslKoef.Value != 0 && staj_lgot.TerrUslKoef.HasValue)//staj_lgot.TerrUsl.Code
                        {
                            ОсобенностиУчета.Element("ТерриториальныеУсловия").Add(new XElement("Коэффициент", Utils.decToStr(staj_lgot.TerrUslKoef.Value)));
                        }
                    }

                    if (staj_lgot.OsobUslTrudaID != null)
                    {
                        XElement ОсобыеУсловияТруда = new XElement("ОсобыеУсловияТруда",
                                                 new XElement("ОснованиеОУТ", OsobUslTruda_list.First(x => x.ID == staj_lgot.OsobUslTrudaID.Value).Code));//staj_lgot.OsobUslTruda.Code

                        if (staj_lgot.KodVred_OsnID != null)
                        {
                            ОсобыеУсловияТруда.Add(new XElement("ПозицияСписка", KodVred_2_list.First(x => x.ID == staj_lgot.KodVred_OsnID.Value).Code));//staj_lgot.KodVred_2.Code
                        }

                        ОсобенностиУчета.Add(ОсобыеУсловияТруда);
                    }

                    if (staj_lgot.IschislStrahStajOsnID != null || (staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0) || (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0))
                    {
                        XElement ИсчисляемыйСтаж = new XElement("ИсчисляемыйСтаж");

                        if (staj_lgot.IschislStrahStajOsnID != null)
                        {
                            var t = IschislStrahStajOsn_list.First(x => x.ID == staj_lgot.IschislStrahStajOsnID.Value).Code;
                            ИсчисляемыйСтаж.Add(new XElement("ОснованиеИС", t));//staj_lgot.IschislStrahStajOsn.Code
                            if (t == "ВОДОЛАЗ")
                            {
                                if (staj_lgot.Strah1Param.HasValue || staj_lgot.Strah2Param.HasValue)
                                {
                                    XElement ВыработкаВчасах = new XElement("ВыработкаВчасах");
                                    if (staj_lgot.Strah1Param.HasValue)
                                        ВыработкаВчасах.Add(new XElement("Часы", staj_lgot.Strah1Param.Value));
                                    if (staj_lgot.Strah2Param.HasValue)
                                        ВыработкаВчасах.Add(new XElement("Минуты", staj_lgot.Strah2Param.Value));

                                    ИсчисляемыйСтаж.Add(ВыработкаВчасах);
                                }
                            }
                            else
                            {
                                if (staj_lgot.Strah1Param.HasValue || staj_lgot.Strah2Param.HasValue)
                                {
                                    XElement ВыработкаКалендарная = new XElement("ВыработкаКалендарная");
                                    if (staj_lgot.Strah1Param.HasValue)
                                        ВыработкаКалендарная.Add(new XElement("ВсеМесяцы", staj_lgot.Strah1Param.Value));
                                    if (staj_lgot.Strah2Param.HasValue)
                                        ВыработкаКалендарная.Add(new XElement("ВсеДни", staj_lgot.Strah2Param.Value));

                                    ИсчисляемыйСтаж.Add(ВыработкаКалендарная);
                                }
                            }
                        }
                        else if ((staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0) || (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0))
                        {
                            XElement ВыработкаКалендарная = new XElement("ВыработкаКалендарная");
                            if (staj_lgot.Strah1Param.HasValue)
                                ВыработкаКалендарная.Add(new XElement("ВсеМесяцы", staj_lgot.Strah1Param.Value));
                            if (staj_lgot.Strah2Param.HasValue)
                                ВыработкаКалендарная.Add(new XElement("ВсеДни", staj_lgot.Strah2Param.Value));

                            ИсчисляемыйСтаж.Add(ВыработкаКалендарная);
                        }

                        ОсобенностиУчета.Add(ИсчисляемыйСтаж);
                    }

                    if (staj_lgot.IschislStrahStajDopID != null)
                    {
                        ОсобенностиУчета.Add(new XElement("ДекретДети", IschislStrahStajDop_list.First(x => x.ID == staj_lgot.IschislStrahStajDopID.Value).Code));//staj_lgot.IschislStrahStajDop.Code
                    }

                    if (staj_lgot.UslDosrNaznID != null)
                    {
                        XElement ВыслугаЛет = new XElement("ВыслугаЛет",
                                                 new XElement("ОснованиеВЛ", UslDosrNazn_list.First(x => x.ID == staj_lgot.UslDosrNaznID.Value).Code));//staj_lgot.UslDosrNazn.Code

                        if (staj_lgot.UslDosrNazn.Code == "27-15")
                        {
                            if (staj_lgot.UslDosrNazn1Param.HasValue || staj_lgot.UslDosrNazn2Param.HasValue)
                            {
                                XElement ВыработкаКалендарная = new XElement("ВыработкаКалендарная");
                                if (staj_lgot.UslDosrNazn1Param.HasValue)
                                    ВыработкаКалендарная.Add(new XElement("ВсеМесяцы", staj_lgot.UslDosrNazn1Param.Value));
                                if (staj_lgot.UslDosrNazn2Param.HasValue)
                                    ВыработкаКалендарная.Add(new XElement("ВсеДни", staj_lgot.UslDosrNazn2Param.Value));

                                ВыслугаЛет.Add(ВыработкаКалендарная);
                            }
                        }
                        else
                        {
                            if ((staj_lgot.UslDosrNazn1Param.HasValue && staj_lgot.UslDosrNazn1Param.Value != 0) || (staj_lgot.UslDosrNazn2Param.HasValue && staj_lgot.UslDosrNazn2Param.Value != 0))
                            {
                                XElement ВыработкаВчасах = new XElement("ВыработкаВчасах");
                                if (staj_lgot.UslDosrNazn1Param.HasValue)
                                    ВыработкаВчасах.Add(new XElement("Часы", staj_lgot.UslDosrNazn1Param.Value));
                                if (staj_lgot.UslDosrNazn2Param.HasValue)
                                    ВыработкаВчасах.Add(new XElement("Минуты", staj_lgot.UslDosrNazn2Param.Value));

                                ВыслугаЛет.Add(ВыработкаВчасах);
                            }
                        }

                        if (staj_lgot.UslDosrNazn3Param.HasValue && staj_lgot.UslDosrNazn3Param.Value != 0)
                        {
                            ВыслугаЛет.Add(new XElement("ДоляСтавки", Utils.decToStr(staj_lgot.UslDosrNazn3Param.Value)));
                        }

                        ОсобенностиУчета.Add(ВыслугаЛет);
                    }

                    ЛьготныйСтаж.Add(ОсобенностиУчета);

                    СтажевыйПериод.Add(ЛьготныйСтаж);
                }
            }

            return СтажевыйПериод;
        }

        private void packsGrid_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.Name == "num" && packsGrid.Rows[e.RowIndex].Cells[1].Value != null)
            {
                long id = Convert.ToInt64(packsGrid.Rows[e.RowIndex].Cells[1].Value);

                if (dbxml.xmlInfo.Any(x => x.ID == id))
                {
                    var xml_info = dbxml.xmlInfo.FirstOrDefault(x => x.ID == id);
                    long num = 0;

                    if (long.TryParse(e.Value.ToString(), out num))
                    {
                        if (xml_info.Num != num)
                        {
                            xml_info.Num = num;
                            string fileName = xml_info.FileName;
                            int ind = fileName.IndexOf("DCK-");
                            if (ind > 0)
                            {
                                ind += 4;
                                xml_info.FileName = fileName.Substring(0, ind) + num.ToString().PadLeft(5, '0') + fileName.Substring(ind + 5, fileName.Length - (ind + 5));

                                if (packsGrid.Rows.Any(x => x.Cells["type"].Value.ToString() == "РСВ"))
                                {
                                    id = 0;

                                    long.TryParse(packsGrid.Rows.First(x => x.Cells["type"].Value.ToString() == "РСВ").Cells["id"].Value.ToString(), out id);

                                    long sourceID = 0;
                                    if (dbxml.xmlInfo.Any(x => x.ID == id))
                                        sourceID = dbxml.xmlInfo.FirstOrDefault(x => x.ID == id).SourceID.Value;

                                    if (db.FormsRSW2014_1_1.Any(x => x.ID == sourceID))
                                    {
                                        var rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == sourceID);

                                        if (db.FormsRSW2014_1_Razd_2_5_1.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID && x.Col_5 == fileName))
                                        {
                                            var rsw251 = db.FormsRSW2014_1_Razd_2_5_1.FirstOrDefault(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID && x.Col_5 == fileName);
                                            rsw251.Col_5 = xml_info.FileName;

                                            try
                                            {
                                                db.ObjectStateManager.ChangeObjectState(rsw251, EntityState.Modified);
                                                db.SaveChanges();

                                            }
                                            catch (Exception ex)
                                            {
                                                Methods.showAlert("Внимание!", "При обновлении Раздела 2.5.1 произошла ошибка! Код ошибки: " + ex.Message, this.ThemeName);
                                            }
                                        }

                                        if (db.FormsRSW2014_1_Razd_2_5_2.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID && x.Col_8 == fileName))
                                        {
                                            var rsw252 = db.FormsRSW2014_1_Razd_2_5_2.FirstOrDefault(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID && x.Col_8 == fileName);
                                            rsw252.Col_8 = xml_info.FileName;

                                            try
                                            {
                                                db.ObjectStateManager.ChangeObjectState(rsw252, EntityState.Modified);
                                                db.SaveChanges();

                                            }
                                            catch (Exception ex)
                                            {
                                                Methods.showAlert("Внимание!", "При обновлении Раздела 2.5.2 произошла ошибка! Код ошибки: " + ex.Message, this.ThemeName);
                                            }
                                        }


                                    }
                                }
                                else
                                {
                                    //                                    RadMessageBox.Show(this, "В пачке не найдена Форма РСВ-1.", "");
                                }

                            }

                            try
                            {
                                dbxml.ObjectStateManager.ChangeObjectState(xml_info, EntityState.Modified);
                                dbxml.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При обновлении записи произошла ошибка! Код ошибки: " + ex.Message);
                            }
                            finally
                            {
                                packsGrid_upd();

                            }
                            return;
                        }

                    }

                }
                packsGrid_upd();
            }


        }

        private void radMenuItem4_Click_1(object sender, EventArgs e)
        {
            exportFilesPre(0);
        }

        private void radMenuItem5_Click(object sender, EventArgs e)
        {
            exportFilesPre(1);
        }

        private void radMenuItem6_Click(object sender, EventArgs e)
        {
            exportFilesPre(2);
        }

        public void exportFilesPre(byte savingType)
        {
            if (packsGrid.RowCount > 0)
            {
                DialogResult result = folderBrowserDialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    idList = new List<long>();

                    switch (savingType)
                    {
                        case (0):
                            foreach (var row in packsGrid.Rows)
                            {
                                idList.Add(long.Parse(row.Cells["id"].Value.ToString()));
                            }
                            break;
                        case (1):
                            foreach (var row in packsGrid.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                            {
                                idList.Add(long.Parse(row.Cells["id"].Value.ToString()));
                            }

                            if (idList.Count() <= 0)
                            {
                                idList.Add(long.Parse(packsGrid.CurrentRow.Cells["id"].Value.ToString()));
                            }

                            break;
                        case (2):
                            idList.Add(long.Parse(packsGrid.CurrentRow.Cells["id"].Value.ToString()));
                            break;
                    }

                    if (idList.Count() > 0)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        fillDictions();
                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += new System.ComponentModel.DoWorkEventHandler(exportFiles);
                        bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedSaving);
                        bw.WorkerSupportsCancellation = true;
                        bw.WorkerReportsProgress = true;


                        bw.RunWorkerAsync();
                    }
                    else
                    {
                        RadMessageBox.Show("Нет данных для сохранения!");
                    }

                }
            }
            else
                RadMessageBox.Show("Нет данных для сохранения!");



        }

        private void rsw2014packs_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками
            List<WindowData> windowData = new List<WindowData> { };

            if (folderBrowserDialog.SelectedPath != null)
            {
                windowData.Add(new WindowData
                {
                    control = "folderBrowserDialog",
                    value = folderBrowserDialog.SelectedPath
                });
            }

            props.setFormParams(this, windowData);
        }

        private void printADW2_Click(object sender, EventArgs e)
        {
            if (packsGrid.RowCount > 0 && packsGrid.CurrentRow.Cells[1].Value != null)
            {
                long id = Convert.ToInt64(packsGrid.CurrentRow.Cells[1].Value);
                List<long> IDlist = dbxml.StaffList.Where(x => x.XmlInfoID == id && x.FormsRSW_6_1_ID.HasValue).Select(x => x.FormsRSW_6_1_ID.Value).ToList();

                printReportADW2(IDlist);

            }
            else
            {
                RadMessageBox.Show(this, "Не удалось напечатать форму", "");
            }
        }

        private void printReportADW2(List<long> idList)
        {
            if (db.FormsADW_2.Any(x => idList.Contains(x.ID)))
            {
                ReportViewerADW2 = new PU.FormsADW2.FormsADW2_Print();
                ReportViewerADW2.ADW2_List = db.FormsADW_2.Where(x => idList.Contains(x.ID)).ToList();
                ReportViewerADW2.Owner = this;
                ReportViewerADW2.ThemeName = this.ThemeName;
                ReportViewerADW2.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerADW2.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedADW2);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Не удалось загрузить данные из базы данных для печати формы", "");
            }
        }

        private void bw_RunWorkerCompletedADW2(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

            ReportViewerADW2.ShowDialog();
        }

        private void printADW1_Click(object sender, EventArgs e)
        {
            if (packsGrid.RowCount > 0 && packsGrid.CurrentRow.Cells[1].Value != null)
            {
                long id = Convert.ToInt64(packsGrid.CurrentRow.Cells[1].Value);
                List<long> IDlist = dbxml.StaffList.Where(x => x.XmlInfoID == id).Select(x => x.StaffID.Value).ToList();

                printReportADW1(IDlist);

            }
            else
            {
                RadMessageBox.Show(this, "Не удалось напечатать форму", "");
            }
        }

        private void printReportADW1(List<long> idList)
        {
            if (db.Staff.Any(x => idList.Contains(x.ID)))
            {
                ReportViewerADW1 = new PU.FormsADW1.FormsADW1_Print();
                ReportViewerADW1.Staff_List = db.Staff.Where(x => idList.Contains(x.ID)).ToList();
                ReportViewerADW1.Owner = this;
                ReportViewerADW1.ThemeName = this.ThemeName;
                ReportViewerADW1.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerADW1.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedADW1);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Не удалось загрузить данные из базы данных для печати формы", "");
            }
        }

        private void bw_RunWorkerCompletedADW1(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

            ReportViewerADW1.ShowDialog();
        }

        private void printADW3_Click(object sender, EventArgs e)
        {
            if (packsGrid.RowCount > 0 && packsGrid.CurrentRow.Cells[1].Value != null)
            {
                long id = Convert.ToInt64(packsGrid.CurrentRow.Cells[1].Value);
                List<long> IDlist = dbxml.StaffList.Where(x => x.XmlInfoID == id && x.FormsRSW_6_1_ID.HasValue).Select(x => x.FormsRSW_6_1_ID.Value).ToList();

                printReportADW3(IDlist);

            }
            else
            {
                RadMessageBox.Show(this, "Не удалось напечатать форму", "");
            }
        }

        private void printReportADW3(List<long> idList)
        {
            if (db.FormsADW_3.Any(x => idList.Contains(x.ID)))
            {
                ReportViewerADW3 = new PU.FormsADW3.FormsADW3_Print();
                ReportViewerADW3.ADW3_List = db.FormsADW_3.Where(x => idList.Contains(x.ID)).ToList();
                ReportViewerADW3.Owner = this;
                ReportViewerADW3.ThemeName = this.ThemeName;
                ReportViewerADW3.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerADW3.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedADW3);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Не удалось загрузить данные из базы данных для печати формы", "");
            }
        }

        private void bw_RunWorkerCompletedADW3(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

            ReportViewerADW3.ShowDialog();
        }


    }
}
