using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.Reporting.Drawing;
using Telerik.Reporting;
using PU.Classes;
using PU.Models;
using PU.FormsRSW2014.Report;
using System.Reflection;


namespace PU.FormsRW_3_2015
{
    public partial class RW3_2015_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        private string regNum = "";
        public FormsRW3_2015 RW3data;
        public List<FormsRW3_2015_Razd_3> RW3_2015_3_List = new List<FormsRW3_2015_Razd_3>();
        string DateUnderwrite;

        public RW3_2015_Print()
        {
            InitializeComponent();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == RW3data.InsurerID);
            ReportBook RW3_2015Book = new ReportBook();
            int pageCnt = 0;

            DateUnderwrite = RW3data.DateUnderwrite.HasValue ? RW3data.DateUnderwrite.Value.ToShortDateString() : DateTime.Now.ToShortDateString();

            regNum = Utils.ParseRegNum(ins.RegNum);

            //Делаем стиль для табличек
            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            Telerik.Reporting.Drawing.StyleRule myStyleRuleLeftAlign = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRuleLeftAlign.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyleLeftAlign") });
            myStyleRuleLeftAlign.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRuleLeftAlign.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRuleLeftAlign.Style.Font.Name = "Arial";
            myStyleRuleLeftAlign.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRuleLeftAlign.Style.TextAlign = HorizontalAlign.Left;
            myStyleRuleLeftAlign.Style.VerticalAlign = VerticalAlign.Middle;

            //Титульный лист
            RW3_2015_ReTitle RW3_2015_Title = new RW3_2015_ReTitle();
            (RW3_2015_Title.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;
            Telerik.Reporting.TextBox RegNum_title = RW3_2015_Title.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
            RegNum_title.Value = regNum;
            Telerik.Reporting.TextBox CorrectionNum = RW3_2015_Title.Items.Find("textBox4", true)[0] as Telerik.Reporting.TextBox;
            CorrectionNum.Value = RW3data.CorrectionNum.ToString().PadLeft(3, '0');
            Telerik.Reporting.TextBox QuarterTitle = RW3_2015_Title.Items.Find("textBox5", true)[0] as Telerik.Reporting.TextBox;
            QuarterTitle.Value = RW3data.Quarter.ToString();
            Telerik.Reporting.TextBox YearTitle = RW3_2015_Title.Items.Find("textBox6", true)[0] as Telerik.Reporting.TextBox;
            YearTitle.Value = RW3data.Year.ToString();
            Telerik.Reporting.TextBox WorkStop = RW3_2015_Title.Items.Find("textBox18", true)[0] as Telerik.Reporting.TextBox;
            WorkStop.Value = RW3data.WorkStop.ToString();

            if (ins.TypePayer == 0)
            {
                Telerik.Reporting.TextBox Name = RW3_2015_Title.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox;
                Name.Value = ins.Name != null ? ins.Name.ToString() : (ins.NameShort != null ? ins.NameShort : "");
                Telerik.Reporting.TextBox KPP = RW3_2015_Title.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                KPP.Value = ins.KPP != null ? ins.KPP : "";
            }
            else
            {
                Telerik.Reporting.TextBox Name = RW3_2015_Title.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox;
                Name.Value = (ins.LastName != null ? ins.LastName.ToString() : "") + " " + (ins.FirstName != null ? ins.FirstName.ToString() : "") + " " + (ins.MiddleName != null ? ins.MiddleName.ToString() : "");
                Telerik.Reporting.TextBox KPP = RW3_2015_Title.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                KPP.Value = "";
            }

            //if (ins.INN != null)
            //{
            //    int i = 0;
            //    string[] inn = ins.INN.Select(c => c.ToString()).ToArray();
            //    foreach (var s in inn)
            //    {
            //        Telerik.Reporting.TextBox INN = RW3_2015_Title.Items.Find("textBox" + (i + 21).ToString(), true)[0] as Telerik.Reporting.TextBox;
            //        INN.Value = inn[i];
            //        i++;
            //    }
            //}

            //if (!String.IsNullOrEmpty(ins.InsuranceNumber))
            //{
            //    int i = 0;
            //    List<string> snils = ins.InsuranceNumber.Select(c => c.ToString()).ToList();
            //    var contr = ins.ControlNumber;
            //    snils.Insert(3, "-");
            //    snils.Insert(7, "-");
            //    snils.Add("-");
            //    snils.Add((contr / 10).ToString());
            //    snils.Add((contr % 10).ToString());
            //    foreach (var s in snils)
            //    {
            //        Telerik.Reporting.TextBox SNILS = RW3_2015_Title.Items.Find("textBox" + (i + 34).ToString(), true)[0] as Telerik.Reporting.TextBox;
            //        SNILS.Value = snils[i];
            //        i++;
            //    }
            //}

            //if (ins.PhoneContact != null)
            //{
            //    int i = 0;
            //    List<string> phone = ins.PhoneContact.Select(c => c.ToString()).ToList();
            //    foreach (var s in phone)
            //    {
            //        Telerik.Reporting.TextBox PHONE = RW3_2015_Title.Items.Find("textBox" + (i + 106).ToString(), true)[0] as Telerik.Reporting.TextBox;
            //        PHONE.Value = phone[i];
            //        i++;
            //    }
            //}


            Telerik.Reporting.TextBox INN = RW3_2015_Title.Items.Find("textBox25", true)[0] as Telerik.Reporting.TextBox;
            INN.Value = ins.INN != null ? ins.INN.ToString() : "";
            Telerik.Reporting.TextBox OKWED = RW3_2015_Title.Items.Find("textBox27", true)[0] as Telerik.Reporting.TextBox;
            OKWED.Value = ins.OKWED != null ? ins.OKWED.ToString() : "";
            Telerik.Reporting.TextBox PhoneContact = RW3_2015_Title.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
            PhoneContact.Value = ins.PhoneContact != null ? ins.PhoneContact.ToString() : "";
            Telerik.Reporting.TextBox CountConfirmDoc = RW3_2015_Title.Items.Find("textBox50", true)[0] as Telerik.Reporting.TextBox;
            CountConfirmDoc.Value = RW3data.CountConfirmDoc != null ? RW3data.CountConfirmDoc.ToString() : "";
            Telerik.Reporting.TextBox ConfirmType = RW3_2015_Title.Items.Find("textBox54", true)[0] as Telerik.Reporting.TextBox;
            ConfirmType.Value = RW3data.ConfirmType.ToString();
            Telerik.Reporting.TextBox ConfirmName = RW3_2015_Title.Items.Find("textBox55", true)[0] as Telerik.Reporting.TextBox;
            ConfirmName.Value = RW3data.ConfirmLastName + " " + RW3data.ConfirmFirstName + " " + RW3data.ConfirmMiddleName;
            Telerik.Reporting.TextBox ConfirmOrg = RW3_2015_Title.Items.Find("textBox56", true)[0] as Telerik.Reporting.TextBox;
            ConfirmOrg.Value = RW3data.ConfirmOrgName != null ? RW3data.ConfirmOrgName : "";
            Telerik.Reporting.TextBox ConfirmDoc = RW3_2015_Title.Items.Find("textBox57", true)[0] as Telerik.Reporting.TextBox;

            string docName = "";
            if (RW3data.ConfirmDocType_ID != null)
            {
                if (db.DocumentTypes.FirstOrDefault(x => x.ID == RW3data.ConfirmDocType_ID).Code == "ПРОЧЕЕ")
                    docName = RW3data.ConfirmDocName;
                else
                    docName = db.DocumentTypes.FirstOrDefault(x => x.ID == RW3data.ConfirmDocType_ID).Code;
                docName = docName + " " + RW3data.ConfirmDocSerLat + " № " + RW3data.ConfirmDocNum + " Выдан: ";
                if (RW3data.ConfirmDocType_ID != null)
                {
                    if (db.DocumentTypes.FirstOrDefault(x => x.ID == RW3data.ConfirmDocType_ID).Code == "ПРОЧЕЕ")
                        docName = RW3data.ConfirmDocName;
                    else
                        docName = db.DocumentTypes.FirstOrDefault(x => x.ID == RW3data.ConfirmDocType_ID).Code;

                    docName = docName + " " + RW3data.ConfirmDocSerLat + " " + " № " + RW3data.ConfirmDocNum;

                    if (RW3data.ConfirmDocDate.HasValue || !String.IsNullOrEmpty(RW3data.ConfirmDocKemVyd))
                    {

                        docName = docName + " Выдан: ";

                        if (RW3data.ConfirmDocDate.HasValue)
                            docName = docName + RW3data.ConfirmDocDate.Value.ToShortDateString();

                        docName = docName + "  " + RW3data.ConfirmDocKemVyd;
                    }

                    if (RW3data.ConfirmDocDateBegin.HasValue || RW3data.ConfirmDocDateEnd.HasValue)
                    {
                        docName = docName + "  Действует";

                        if (RW3data.ConfirmDocDateBegin.HasValue)
                            docName = docName + " c " + RW3data.ConfirmDocDateBegin.Value.ToShortDateString();

                        if (RW3data.ConfirmDocDateEnd.HasValue)
                            docName = docName + " по " + RW3data.ConfirmDocDateEnd.Value.ToShortDateString();
                    }

                }
            }

            ConfirmDoc.Value = docName;


            pageCnt++;
            RW3_2015Book.Reports.Add(RW3_2015_Title);


            //Раздел 1 и 2
            RW3_2015_Re1 RW3_2015_1 = new RW3_2015_Re1();
            (RW3_2015_1.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

            Telerik.Reporting.TextBox RegNum1 = RW3_2015_1.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
            RegNum1.Value = regNum;


            var fields = typeof(FormsRW3_2015).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    try
                    {

                        if (RW3_2015_1.Items.Find(itemName, true).Any())
                        {
                            if (RW3data != null)
                            {
                                var properties = RW3data.GetType().GetProperty(itemName);
                                object value = properties.GetValue(RW3data, null);
                                string type = properties.PropertyType.FullName;
                                if (type.Contains("["))
                                    type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                                type = type.Split(',')[0].Split('.')[1].ToLower();

                                Telerik.Reporting.TextBox sum_textbox = RW3_2015_1.Items.Find(itemName, true)[0] as Telerik.Reporting.TextBox;

                                if (value != null)
                                {
                                    switch (type)
                                    {
                                        case "decimal":
                                            sum_textbox.Value = Utils.decToStr(decimal.Parse(value.ToString()));
                                            break;
                                        default:
                                            sum_textbox.Value = value.ToString();
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (type)
                                    {
                                        case "decimal":
                                            sum_textbox.Value = Utils.decToStr(0);
                                            break;
                                        default:
                                            sum_textbox.Value = "0";
                                            break;
                                    }
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message);
                    }

                }
            }

            pageCnt++;
            RW3_2015Book.Reports.Add(RW3_2015_1);


            RW3_2015_3_List = db.FormsRW3_2015_Razd_3.Where(x => x.FormsRW3_2015_ID == RW3data.ID).ToList();
            if (RW3_2015_3_List.Count() > 0)
            {
                RW3_2015_Re3 RW3_2015_3 = new RW3_2015_Re3();
                (RW3_2015_3.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                Telerik.Reporting.TextBox RegNum3 = RW3_2015_3.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                RegNum3.Value = regNum;

                //Telerik.Reporting.DetailSection detail_Re3 = RW3_2015_3.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                Telerik.Reporting.Table table_Re3 = RW3_2015_3.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

                TableGroup detailGrouptable_Re3 = new TableGroup();
                RW3_2015_3.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule, myStyleRuleLeftAlign });

                int j = 1;
                foreach (var rw33 in RW3_2015_3_List)
                {
                    Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = j.ToString()
                    };
                    table_Re3.Body.SetCellContent(j, 0, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rw33.CodeBase.HasValue ? rw33.CodeBase.Value.ToString() : ""
                    };
                    table_Re3.Body.SetCellContent(j, 1, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rw33.Year.ToString()
                    };
                    table_Re3.Body.SetCellContent(j, 2, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rw33.Month.HasValue ? rw33.Month.Value.ToString() : ""
                    };
                    table_Re3.Body.SetCellContent(j, 3, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rw33.SumFee.HasValue ? Utils.decToStr(rw33.SumFee.Value) : "0.00"
                    };
                    table_Re3.Body.SetCellContent(j, 4, textBox);

                    table_Re3.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5D)));
                    detailGrouptable_Re3.ChildGroups.Add(new TableGroup());
                    j++;
                }

                #region  Итоговая строка

                Telerik.Reporting.TextBox textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = "Итого сумма перерасчета:"
                };
                table_Re3.Body.SetCellContent(j, 0, textBox_, 1, 2);
                //textBox_ = new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = "Итоговая сумма перерасчета:"
                //};
                //table_Re3.Body.SetCellContent(j, 1, textBox_);
                textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = "X"
                };
                table_Re3.Body.SetCellContent(j, 2, textBox_);
                textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = "X"
                };
                table_Re3.Body.SetCellContent(j, 3, textBox_);
                textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(RW3_2015_3_List.Sum(x => x.SumFee.Value))
                };
                table_Re3.Body.SetCellContent(j, 4, textBox_);

                table_Re3.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5D)));
                detailGrouptable_Re3.ChildGroups.Add(new TableGroup());

                #endregion


                table_Re3.RowGroups.Add(detailGrouptable_Re3);

                pageCnt++;
                RW3_2015Book.Reports.Add(RW3_2015_3);

            }




            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            Telerik.Reporting.TextBox pageCount = RW3_2015Book.Reports[0].Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
            pageCount.Value = pageCnt.ToString();

            typeReportSource.ReportDocument = RW3_2015Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }

        public void createEmptyReport()
        {
            ReportBook RW3_2015Book = new ReportBook();
            //Делаем стиль для табличек
            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            //Титульный лист
            RW3_2015_ReTitle RW3_2015_Title = new RW3_2015_ReTitle();
            (RW3_2015_Title.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = "";
            Telerik.Reporting.TextBox st = RW3_2015_Title.Items.Find("textBox83", true)[0] as Telerik.Reporting.TextBox;
            st.Style.BorderStyle.Default = BorderType.Dotted;
            st.Style.TextAlign = HorizontalAlign.Left;
            st.Value = "Стр.";

            RW3_2015Book.Reports.Add(RW3_2015_Title);


            //Раздел 1 и 2
            RW3_2015_Re1 RW3_2015_1 = new RW3_2015_Re1();
            (RW3_2015_1.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = "";
            Telerik.Reporting.TextBox s1 = RW3_2015_1.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
            s1.Style.BorderStyle.Default = BorderType.Dotted;
            s1.Style.TextAlign = HorizontalAlign.Left;
            s1.Value = "Стр.";

            RW3_2015Book.Reports.Add(RW3_2015_1);


            RW3_2015_Re3 RW3_2015_3 = new RW3_2015_Re3();
            (RW3_2015_3.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = "";
            Telerik.Reporting.TextBox s3 = RW3_2015_3.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
            s3.Style.BorderStyle.Default = BorderType.Dotted;
            s3.Style.TextAlign = HorizontalAlign.Left;
            s3.Value = "Стр.";

            Telerik.Reporting.Table table_Re3 = RW3_2015_3.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re3 = new TableGroup();
            RW3_2015_3.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

            for (int i = 1; i <= 20; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    table_Re3.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " ", Height = Telerik.Reporting.Drawing.Unit.Cm(0.6D)});
                }
            }

            table_Re3.Body.SetCellContent(21, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "Итого сумма перерасчета:", Height = Telerik.Reporting.Drawing.Unit.Cm(0.6D) }, 1, 2);
            table_Re3.Body.SetCellContent(21, 2, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "X", Height = Telerik.Reporting.Drawing.Unit.Cm(0.6D) });
            table_Re3.Body.SetCellContent(21, 3, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "X", Height = Telerik.Reporting.Drawing.Unit.Cm(0.6D) });
            table_Re3.Body.SetCellContent(21, 4, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " ", Height = Telerik.Reporting.Drawing.Unit.Cm(0.6D) });

            for (int i = 0; i <= 20; i++)
            {
                table_Re3.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_Re3.ChildGroups.Add(new TableGroup());
            }


            table_Re3.RowGroups.Add(detailGrouptable_Re3);

            RW3_2015Book.Reports.Add(RW3_2015_3);

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = RW3_2015Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }


        private void RSW2_2014_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }
    }
}
