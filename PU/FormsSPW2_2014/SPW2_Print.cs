using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;
using PU.Models;
using PU.Classes;
using PU.FormsSPW2_2014.Report;


namespace PU.FormsSPW2_2014
{
    public partial class SPW2_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        //private string regNum = "";
        public FormsSPW2 SPW2data;
        public List<FormsSPW2> SPW_2_List = new List<FormsSPW2>();
        public List<StajOsn> Staj_List = new List<StajOsn>();

        public SPW2_Print()
        {
            InitializeComponent();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            ReportBook SPW2_Book = new ReportBook();

            foreach (var spw_2 in SPW_2_List)
            {
                Staff staff = db.Staff.FirstOrDefault(x => x.ID == spw_2.StaffID);
                Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == staff.InsurerID);
                PlatCategory PlatCat = db.PlatCategory.FirstOrDefault(x => x.ID == spw_2.PlatCategoryID);

                string regNum = Utils.ParseRegNum(ins.RegNum);

                SPW2_Report SPW2 = new SPW2_Report();

                Telerik.Reporting.TextBox DateUnderwrite = SPW2.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox;
                DateUnderwrite.Value = spw_2.DateFilling.ToShortDateString();

                Telerik.Reporting.TextBox OKPO = SPW2.Items.Find("textBox5", true)[0] as Telerik.Reporting.TextBox;
                OKPO.Value = ins.OKPO != null ? ins.OKPO.ToString() : "";
                Telerik.Reporting.TextBox RegNum_title = SPW2.Items.Find("textBox6", true)[0] as Telerik.Reporting.TextBox;
                RegNum_title.Value = regNum;
                Telerik.Reporting.TextBox NameShort = SPW2.Items.Find("textBox7", true)[0] as Telerik.Reporting.TextBox;
                NameShort.Value = ins.NameShort != null ? ins.NameShort.ToString() : "";
                Telerik.Reporting.TextBox INN = SPW2.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                INN.Value = ins.INN != null ? ins.INN.ToString() : "";
                Telerik.Reporting.TextBox KPP = SPW2.Items.Find("textBox12", true)[0] as Telerik.Reporting.TextBox;
                KPP.Value = ins.KPP != null ? ins.KPP.ToString() : "";
                Telerik.Reporting.TextBox PlatCat_ = SPW2.Items.Find("textBox15", true)[0] as Telerik.Reporting.TextBox;
                PlatCat_.Value = PlatCat.Code;
                Telerik.Reporting.TextBox DateComposit = SPW2.Items.Find("textBox16", true)[0] as Telerik.Reporting.TextBox;
                DateComposit.Value = spw_2.DateComposit != null ? spw_2.DateComposit.ToShortDateString() : "";
                //Telerik.Reporting.TextBox DateFilling = SPW2.Items.Find("textBox20", true)[0] as Telerik.Reporting.TextBox;
                //DateFilling.Value = spw_2.DateFilling != null ? spw_2.DateFilling.ToShortDateString() : "";
                Telerik.Reporting.TextBox Quarter = SPW2.Items.Find("textBox18", true)[0] as Telerik.Reporting.TextBox;
                Quarter.Value = spw_2.Quarter != null ? spw_2.Quarter.ToString() : "";
                Telerik.Reporting.TextBox Year = SPW2.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                Year.Value = spw_2.Year != null ? spw_2.Year.ToString() : "";


                Telerik.Reporting.TextBox tb = new Telerik.Reporting.TextBox { };

                if (ins.PerformerPrint.HasValue && ins.PerformerPrint.Value == true)
                {
                    tb = SPW2.Items.Find("ispolnName", true)[0] as Telerik.Reporting.TextBox;
                    tb.Value = ins.PerformerFIO;
                    tb = SPW2.Items.Find("ispolnDolgn", true)[0] as Telerik.Reporting.TextBox;
                    tb.Value = ins.PerformerDolgn;
                }

                if (ins.BossPrint.HasValue && ins.BossPrint.Value == true)
                {
                    tb = SPW2.Items.Find("rukName1", true)[0] as Telerik.Reporting.TextBox;
                    tb.Value = ins.BossFIO;
                    tb = SPW2.Items.Find("rukDolgn1", true)[0] as Telerik.Reporting.TextBox;
                    tb.Value = ins.BossDolgn;
                }
                if (ins.BossDopPrint.HasValue && ins.BossDopPrint.Value == true)
                {
                    tb = SPW2.Items.Find("rukName2", true)[0] as Telerik.Reporting.TextBox;
                    tb.Value = ins.BossFIODop;
                    tb = SPW2.Items.Find("rukDolgn2", true)[0] as Telerik.Reporting.TextBox;
                    tb.Value = ins.BossDolgnDop;
                }


                if (spw_2.TypeInfoID == 1)
                {
                    Telerik.Reporting.TextBox TypeInfo = SPW2.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox;
                    TypeInfo.Value = "X";
                }
                else
                {
                    if (spw_2.TypeInfoID == 2)
                    {
                        Telerik.Reporting.TextBox TypeInfo = SPW2.Items.Find("textBox36", true)[0] as Telerik.Reporting.TextBox;
                        TypeInfo.Value = "X";
                    }
                    else
                    {
                        Telerik.Reporting.TextBox TypeInfo = SPW2.Items.Find("textBox37", true)[0] as Telerik.Reporting.TextBox;
                        TypeInfo.Value = "X";
                    }
                    Telerik.Reporting.TextBox QuarterKorr = SPW2.Items.Find("textBox43", true)[0] as Telerik.Reporting.TextBox;
                    QuarterKorr.Value = spw_2.QuarterKorr != null ? spw_2.QuarterKorr.ToString() : "";
                    Telerik.Reporting.TextBox YearKorr = SPW2.Items.Find("textBox46", true)[0] as Telerik.Reporting.TextBox;
                    YearKorr.Value = spw_2.YearKorr != null ? spw_2.YearKorr.ToString() : "";
                }

                Telerik.Reporting.TextBox LastName = SPW2.Items.Find("textBox26", true)[0] as Telerik.Reporting.TextBox;
                LastName.Value = staff.LastName != null ? staff.LastName : "";
                Telerik.Reporting.TextBox FirstName = SPW2.Items.Find("textBox28", true)[0] as Telerik.Reporting.TextBox;
                FirstName.Value = staff.FirstName != null ? staff.FirstName : "";
                Telerik.Reporting.TextBox MiddleName = SPW2.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                MiddleName.Value = staff.MiddleName != null ? staff.MiddleName : "";
                Telerik.Reporting.TextBox Insuranse = SPW2.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox;
                Insuranse.Value = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber.HasValue ? staff.ControlNumber.Value : (short)100) : "";

                //Делаем стиль для таблички
                Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
                myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
                myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
                myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
                myStyleRule.Style.Font.Name = "Arial";
                myStyleRule.Style.Font.Size = new Unit(8.0, UnitType.Point);
                myStyleRule.Style.TextAlign = HorizontalAlign.Center;
                myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

                //стаж
                Staj_List = db.StajOsn.Where(x => x.FormsSPW2_ID == spw_2.ID).OrderBy(x => x.Number.Value).ToList();
                Telerik.Reporting.DetailSection detail_SPW2 = SPW2.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                Telerik.Reporting.Table table_SPW2 = SPW2.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
                TableGroup detailGrouptable_SPW2 = new TableGroup();
                SPW2.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });


                var TerrUsl_list = db.TerrUsl.ToList();
                var OsobUslTruda_list = db.OsobUslTruda.ToList();
                var KodVred_2_list = db.KodVred_2.ToList();
                var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
                var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
                var UslDosrNazn_list = db.UslDosrNazn.ToList();
                var PlatCategory_list = db.PlatCategory.ToList();
                var SpecOcenkaUslTruda_list = db.SpecOcenkaUslTruda.ToList();

                int j = 1;
                foreach (var item in Staj_List)
                {
                    Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = item.Number.ToString()
                    };
                    table_SPW2.Body.SetCellContent(j, 0, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Convert.ToDateTime(item.DateBegin).ToString("dd/MM/yyyy")
                    };
                    table_SPW2.Body.SetCellContent(j, 1, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Convert.ToDateTime(item.DateEnd).ToString("dd/MM/yyyy")
                    };
                    table_SPW2.Body.SetCellContent(j, 2, textBox);

                    if (item.StajLgot.Count() > 0)
                    {
                        int ii = 0;
                        foreach (var item_dop in item.StajLgot.OrderBy(x => x.Number.Value))
                        {
                            if (ii > 0) // если несколько записей о льготном стаже
                            {
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = ""
                                };
                                table_SPW2.Body.SetCellContent(j, 0, textBox);
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = ""
                                };
                                table_SPW2.Body.SetCellContent(j, 1, textBox);
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = ""
                                };
                                table_SPW2.Body.SetCellContent(j, 2, textBox);
                            }

                            if (item_dop.TerrUslID != null && item_dop.TerrUslID.Value.ToString() != "" && TerrUsl_list.Any(x => x.ID == item_dop.TerrUslID))
                            {
                                TerrUsl tu = TerrUsl_list.FirstOrDefault(x => x.ID == item_dop.TerrUslID);
                                string koef = "";
                                if (item_dop.TerrUslKoef.HasValue && item_dop.TerrUslKoef.Value != 0)
                                {
                                    koef = Utils.decToStr(item_dop.TerrUslKoef.Value);
                                }
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = tu.Code == null ? "" : (tu.Code.ToString() + "  " + koef)
                                };
                            }
                            else
                            {
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = ""
                                };
                            }
                            table_SPW2.Body.SetCellContent(j, 3, textBox);

                            if (item_dop.OsobUslTrudaID != null && item_dop.OsobUslTrudaID.Value.ToString() != "" && OsobUslTruda_list.Any(x => x.ID == item_dop.OsobUslTrudaID))
                            {
                                OsobUslTruda ou = OsobUslTruda_list.FirstOrDefault(x => x.ID == item_dop.OsobUslTrudaID);
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = ou.Code == null ? "" : ou.Code.ToString()
                                };
                            }
                            else
                            {
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = ""
                                };
                            }
                            table_SPW2.Body.SetCellContent(j, 4, textBox);


                            IschislStrahStajOsn isso = IschislStrahStajOsn_list.FirstOrDefault(x => x.ID == item_dop.IschislStrahStajOsnID);

                            string str = item_dop.IschislStrahStajDopID == null ? "  " : item_dop.IschislStrahStajDopID.HasValue ? IschislStrahStajDop_list.FirstOrDefault(x => x.ID == item_dop.IschislStrahStajDopID).Code : "  ";
                            string s1 = item_dop.Strah1Param.HasValue ? item_dop.Strah1Param.Value.ToString() : "0";
                            string s2 = item_dop.Strah2Param.HasValue ? item_dop.Strah2Param.Value.ToString() : "0";

                            str = ((!String.IsNullOrEmpty(str.Trim()) || (s1 != "0") || (s2 != "0"))) ? "[" + s1 + "][" + s2 + "][" + str + "]" : "";

                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = isso == null ? "" : isso.Code == null ? "" : isso.Code.ToString()
                            };
                            table_SPW2.Body.SetCellContent(j, 5, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = str
                            };

                            table_SPW2.Body.SetCellContent(j, 6, textBox);

                            if (item_dop.UslDosrNaznID != null)
                            {
                                UslDosrNazn udn = UslDosrNazn_list.FirstOrDefault(x => x.ID == item_dop.UslDosrNaznID);

                                s1 = item_dop.UslDosrNazn1Param.HasValue == true ? item_dop.UslDosrNazn1Param.Value.ToString() : "0";
                                s2 = item_dop.UslDosrNazn2Param.HasValue == true ? item_dop.UslDosrNazn2Param.Value.ToString() : "0";
                                string s3 = item_dop.UslDosrNazn3Param.HasValue == true ? Utils.decToStr(item_dop.UslDosrNazn3Param.Value) : "0";

                                str = "[" + s1 + "][" + s2 + "][" + s3 + "]";

                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = udn == null ? "" : udn.Code == null ? "" : udn.Code.ToString()
                                };
                                table_SPW2.Body.SetCellContent(j, 7, textBox);
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = str
                                };
                                table_SPW2.Body.SetCellContent(j, 8, textBox);
                            }
                            else
                            {
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = ""
                                };
                                table_SPW2.Body.SetCellContent(j, 7, textBox);
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = ""
                                };
                                table_SPW2.Body.SetCellContent(j, 8, textBox);
                            }

                            ii++;

                            if (item.StajLgot.Count() > 1 && ii != item.StajLgot.Count())
                            {
                                j++;
                                table_SPW2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                detailGrouptable_SPW2.ChildGroups.Add(new TableGroup());
                            }


                            if (item_dop.KodVred_OsnID != null && item_dop.KodVred_OsnID.Value.ToString() != "" && KodVred_2_list.Any(x => x.ID == item_dop.KodVred_OsnID))
                            {
                                if ((item.StajLgot.Count()) == 1 || (item.StajLgot.Count() > 1 && ii == item.StajLgot.Count()))
                                {
                                    j++;
                                    table_SPW2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                    detailGrouptable_SPW2.ChildGroups.Add(new TableGroup());
                                }


                                for (int m = 0; m <= 8; m++)
                                {
                                    if (m != 4 && m != 5)
                                    {
                                        textBox = new Telerik.Reporting.TextBox
                                        {
                                            StyleName = "TableStyle",
                                            Value = ""
                                        };
                                        table_SPW2.Body.SetCellContent(j, m, textBox);
                                    }
                                }

                                KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.ID == item_dop.KodVred_OsnID);

                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = kv.Code == null ? "" : kv.Code.ToString()
                                };
                                table_SPW2.Body.SetCellContent(j, 4, textBox, 1, 2);
                                if (item.StajLgot.Count() > 1 && ii != item.StajLgot.Count())
                                {
                                    j++;
                                    table_SPW2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                    detailGrouptable_SPW2.ChildGroups.Add(new TableGroup());
                                }

                            }


                        }
                    }
                    else // если льготного стажа нет, то делаем пустые поля
                    {
                        for (int m = 3; m <= 8; m++)
                        {
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_SPW2.Body.SetCellContent(j, m, textBox);
                        }

                    }

                    if (Staj_List.Count > 1)
                    {
                        table_SPW2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_SPW2.ChildGroups.Add(new TableGroup());
                        j++;
                    }
                    else if (Staj_List.Count == 1)
                    {
                        // && rsw68.StajLgot.Count <= 1
                        table_SPW2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_SPW2.ChildGroups.Add(new TableGroup());
                    }

                }
                if (Staj_List.Count() > 0)
                    table_SPW2.RowGroups.Add(detailGrouptable_SPW2);

                if (spw_2.ExistsInsurOPS == 1)
                {
                    Telerik.Reporting.TextBox ExistsInsurOPS = SPW2.Items.Find("textBox68", true)[0] as Telerik.Reporting.TextBox;
                    ExistsInsurOPS.Value = "X";
                }
                else
                {
                    Telerik.Reporting.TextBox ExistsInsurOPS = SPW2.Items.Find("textBox69", true)[0] as Telerik.Reporting.TextBox;
                    ExistsInsurOPS.Value = "X";
                }

                if (spw_2.ExistsInsurDop == 1)
                {
                    Telerik.Reporting.TextBox ExistsInsurDop = SPW2.Items.Find("textBox77", true)[0] as Telerik.Reporting.TextBox;
                    ExistsInsurDop.Value = "X";
                }
                else
                {
                    Telerik.Reporting.TextBox ExistsInsurDop = SPW2.Items.Find("textBox76", true)[0] as Telerik.Reporting.TextBox;
                    ExistsInsurDop.Value = "X";
                }


                SPW2_Book.Reports.Add(SPW2);


            }

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();


            typeReportSource.ReportDocument = SPW2_Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;


        }

        public void createEmptyReport()
        {
            ReportBook SPW2_Book = new ReportBook();


            SPW2_Report SPW2 = new SPW2_Report();


            //Делаем стиль для таблички
            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(8.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            //стаж

            Telerik.Reporting.Table table_SPW2 = SPW2.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_SPW2 = new TableGroup();
            SPW2.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

            for (int i = 1; i <= 15; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    table_SPW2.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            for (int i = 0; i < 15; i++)
            {
                table_SPW2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_SPW2.ChildGroups.Add(new TableGroup());
            }

            table_SPW2.RowGroups.Add(detailGrouptable_SPW2);


            SPW2_Book.Reports.Add(SPW2);
            var typeReportSource = new Telerik.Reporting.InstanceReportSource();


            typeReportSource.ReportDocument = SPW2_Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;


        }


        private void SPW2_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }
    }
}