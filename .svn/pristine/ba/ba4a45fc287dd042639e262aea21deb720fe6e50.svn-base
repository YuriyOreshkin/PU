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
using PU.FormsRSW2_2014.Report;


namespace PU.FormsRSW2_2014
{
    public partial class RSW2_2014_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        private string regNum = "";
        public FormsRSW2014_2_1 RSW2data;
        string DateUnderwrite;

        public List<FormsRSW2014_2_2> RSW2_2014_2_List = new List<FormsRSW2014_2_2>();
        public List<FormsRSW2014_2_3> RSW2_2014_3_List = new List<FormsRSW2014_2_3>();

        public RSW2_2014_Print()
        {
            InitializeComponent();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == RSW2data.InsurerID);
            ReportBook RSW2_2014Book = new ReportBook();
            int pageCnt = 0;
            DateUnderwrite = RSW2data.DateUnderwrite.HasValue ? RSW2data.DateUnderwrite.Value.ToShortDateString() : DateTime.Now.ToShortDateString();

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

            Telerik.Reporting.Drawing.StyleRule myStyleRuleLeftAlign2 = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRuleLeftAlign2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyleLeftAlign2") });
            myStyleRuleLeftAlign2.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRuleLeftAlign2.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRuleLeftAlign2.Style.Font.Name = "Arial";
            myStyleRuleLeftAlign2.Style.Font.Bold = true;
            myStyleRuleLeftAlign2.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRuleLeftAlign2.Style.TextAlign = HorizontalAlign.Right;
            myStyleRuleLeftAlign2.Style.VerticalAlign = VerticalAlign.Middle;


            //Титульный лист
            RSW2_2014_ReTitle RSW2_2014_Title = new RSW2_2014_ReTitle();
            (RSW2_2014_Title.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

            Telerik.Reporting.TextBox RegNum_title = RSW2_2014_Title.Items.Find("textBox5", true)[0] as Telerik.Reporting.TextBox;
            RegNum_title.Value = regNum;
            Telerik.Reporting.TextBox CorrectionNum = RSW2_2014_Title.Items.Find("textBox7", true)[0] as Telerik.Reporting.TextBox;
            CorrectionNum.Value = RSW2data.CorrectionNum.ToString();
            Telerik.Reporting.TextBox YearTitle = RSW2_2014_Title.Items.Find("textBox10", true)[0] as Telerik.Reporting.TextBox;
            YearTitle.Value = RSW2data.Year.ToString();
            Telerik.Reporting.TextBox WorkStop = RSW2_2014_Title.Items.Find("textBox12", true)[0] as Telerik.Reporting.TextBox;
            WorkStop.Value = RSW2data.WorkStop.ToString();
            Telerik.Reporting.TextBox Name = RSW2_2014_Title.Items.Find("textBox20", true)[0] as Telerik.Reporting.TextBox;
            if (!String.IsNullOrEmpty(ins.LastName))
            {
                Name.Value = ins.LastName.ToString();
            }
            if (!String.IsNullOrEmpty(ins.FirstName))
            {
                Name.Value = Name.Value + " " + ins.FirstName.ToString();
            }
            if (!String.IsNullOrEmpty(ins.MiddleName))
            {
                Name.Value = Name.Value + " " + ins.MiddleName.ToString();
            }

            if (ins.INN != null)
            {
                int i = 0;
                string[] inn = ins.INN.Select(c => c.ToString()).ToArray();
                foreach (var s in inn)
                {
                    Telerik.Reporting.TextBox INN = RSW2_2014_Title.Items.Find("textBox" + (i + 21).ToString(), true)[0] as Telerik.Reporting.TextBox;
                    INN.Value = inn[i];
                    i++;
                }
            }

            if (!String.IsNullOrEmpty(ins.InsuranceNumber))
            {
                int i = 0;
                List<string> snils = ins.InsuranceNumber.Select(c => c.ToString()).ToList();
                var contr = ins.ControlNumber;
                snils.Insert(3, "-");
                snils.Insert(7, "-");
                snils.Add("-");
                snils.Add((contr / 10).ToString());
                snils.Add((contr % 10).ToString());
                foreach (var s in snils)
                {
                    Telerik.Reporting.TextBox SNILS = RSW2_2014_Title.Items.Find("textBox" + (i + 34).ToString(), true)[0] as Telerik.Reporting.TextBox;
                    SNILS.Value = snils[i];
                    i++;
                }
            }
            if (RSW2data.Year >= 2015)
            {
                (RSW2_2014_Title.Items.Find("list6", true)[0] as Telerik.Reporting.List).Visible = false;
                (RSW2_2014_Title.Items.Find("list8", true)[0] as Telerik.Reporting.List).Visible = false;
                (RSW2_2014_Title.Items.Find("textBox1", true)[0] as Telerik.Reporting.TextBox).Value = "Приложение №1\r\nУтверждена \r\nпостановлением Правления ПФР \r\nот 17 сентября 2015 г. № 347п";
                (RSW2_2014_Title.Items.Find("textBox13", true)[0] as Telerik.Reporting.TextBox).Value = "Номер уточнения";
                (RSW2_2014_Title.Items.Find("textBox14", true)[0] as Telerik.Reporting.TextBox).Value = "(000 - исходная форма, уточнение 001 и т.д.)";
                (RSW2_2014_Title.Items.Find("textBox104", true)[0] as Telerik.Reporting.TextBox).Value = "       *  Указывается дата представления расчета лично главой крестьянского (фермерского) хозяйства или через представителя, при отправке по почте - дата отправки почтового отправления с описью вложения.";
            }


            if (ins.PhoneContact != null)
            {
                int i = 0;
                List<string> phone = ins.PhoneContact.Select(c => c.ToString()).ToList();
                foreach (var s in phone)
                {
                    Telerik.Reporting.TextBox PHONE = RSW2_2014_Title.Items.Find("textBox" + (i + 106).ToString(), true)[0] as Telerik.Reporting.TextBox;
                    PHONE.Value = phone[i];
                    i++;
                }
            }

            Telerik.Reporting.TextBox OKWED = RSW2_2014_Title.Items.Find("textBox18", true)[0] as Telerik.Reporting.TextBox;
            OKWED.Value = ins.OKWED != null ? ins.OKWED.ToString() : "";
            Telerik.Reporting.TextBox YearBirth = RSW2_2014_Title.Items.Find("textBox49", true)[0] as Telerik.Reporting.TextBox;
            YearBirth.Value = ins.YearBirth != null ? ins.YearBirth.ToString() : "";
            Telerik.Reporting.TextBox CountEmployers = RSW2_2014_Title.Items.Find("textBox68", true)[0] as Telerik.Reporting.TextBox;
            CountEmployers.Value = RSW2data.CountEmployers != null ? RSW2data.CountEmployers.ToString() : "";
            Telerik.Reporting.TextBox CountConfirmDoc = RSW2_2014_Title.Items.Find("textBox70", true)[0] as Telerik.Reporting.TextBox;
            CountConfirmDoc.Value = RSW2data.CountConfirmDoc != null ? RSW2data.CountConfirmDoc.ToString() : "";

            Telerik.Reporting.TextBox ConfirmType = RSW2_2014_Title.Items.Find("textBox89", true)[0] as Telerik.Reporting.TextBox;
            ConfirmType.Value = RSW2data.ConfirmType.ToString();
            Telerik.Reporting.TextBox ConfirmName = RSW2_2014_Title.Items.Find("textBox90", true)[0] as Telerik.Reporting.TextBox;
            ConfirmName.Value = RSW2data.ConfirmLastName + " " + RSW2data.ConfirmFirstName + " " + RSW2data.ConfirmMiddleName;
            Telerik.Reporting.TextBox ConfirmOrg = RSW2_2014_Title.Items.Find("textBox91", true)[0] as Telerik.Reporting.TextBox;
            ConfirmOrg.Value = RSW2data.ConfirmOrgName != null ? RSW2data.ConfirmOrgName : "";
            Telerik.Reporting.TextBox ConfirmDoc = RSW2_2014_Title.Items.Find("textBox92", true)[0] as Telerik.Reporting.TextBox;

            string docName = "";
            if (RSW2data.ConfirmDocType_ID != null)
            {
                if (db.DocumentTypes.FirstOrDefault(x => x.ID == RSW2data.ConfirmDocType_ID).Code == "ПРОЧЕЕ")
                    docName = RSW2data.ConfirmDocName;
                else
                    docName = db.DocumentTypes.FirstOrDefault(x => x.ID == RSW2data.ConfirmDocType_ID).Code;

                docName = docName + " " + RSW2data.ConfirmDocSerLat + " " + " № " + RSW2data.ConfirmDocNum;

                if (RSW2data.ConfirmDocDate.HasValue || !String.IsNullOrEmpty(RSW2data.ConfirmDocKemVyd))
                {

                    docName = docName + " Выдан: ";

                    if (RSW2data.ConfirmDocDate.HasValue)
                        docName = docName + RSW2data.ConfirmDocDate.Value.ToShortDateString();

                    docName = docName + "  " + RSW2data.ConfirmDocKemVyd;
                }

                if (RSW2data.ConfirmDocDateBegin.HasValue || RSW2data.ConfirmDocDateEnd.HasValue)
                {
                    docName = docName + "  Действует";

                    if (RSW2data.ConfirmDocDateBegin.HasValue)
                        docName = docName + " c " + RSW2data.ConfirmDocDateBegin.Value.ToShortDateString();

                    if (RSW2data.ConfirmDocDateEnd.HasValue)
                        docName = docName + " по " + RSW2data.ConfirmDocDateEnd.Value.ToShortDateString();
                }

            }
            ConfirmDoc.Value = docName;

            pageCnt++;
            RSW2_2014Book.Reports.Add(RSW2_2014_Title);

            //Раздел 1
            RSW2_2014_Re1 RSW2_2014_1 = new RSW2_2014_Re1();
            (RSW2_2014_1.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

            if (RSW2data.Year >= 2015)
            {
                (RSW2_2014_1.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox).Value = "за периоды 2010-2013 гг. *";
                (RSW2_2014_1.Items.Find("textBox72", true)[0] as Telerik.Reporting.TextBox).Value = "Сумма перерасчета страховых взносов за предыдущие расчетные периоды с начала расчетного периода";
                (RSW2_2014_1.Items.Find("textBox31", true)[0] as Telerik.Reporting.TextBox).Value = "на страховую пенсию";
                (RSW2_2014_1.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox).Value = "на накопительную пенсию";

                (RSW2_2014_1.Items.Find("textBox17", true)[0] as Telerik.Reporting.TextBox).Visible = true;
                (RSW2_2014_1.Items.Find("shape1", true)[0] as Telerik.Reporting.Shape).Visible = true;
                
            }

            Telerik.Reporting.TextBox RegNum1 = RSW2_2014_1.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
            RegNum1.Value = regNum;
            Telerik.Reporting.TextBox s_100_0 = RSW2_2014_1.Items.Find("textBox7", true)[0] as Telerik.Reporting.TextBox;
            s_100_0.Value = RSW2data.s_100_0.HasValue ? Utils.decToStr(RSW2data.s_100_0.Value) : "0.00";
            Telerik.Reporting.TextBox s_100_1 = RSW2_2014_1.Items.Find("textBox8", true)[0] as Telerik.Reporting.TextBox;
            s_100_1.Value = RSW2data.s_100_1.HasValue ? Utils.decToStr(RSW2data.s_100_1.Value) : "0.00";
            Telerik.Reporting.TextBox s_100_2 = RSW2_2014_1.Items.Find("textBox10", true)[0] as Telerik.Reporting.TextBox;
            s_100_2.Value = RSW2data.s_100_2.HasValue ? Utils.decToStr(RSW2data.s_100_2.Value) : "0.00";
            Telerik.Reporting.TextBox s_100_3 = RSW2_2014_1.Items.Find("textBox26", true)[0] as Telerik.Reporting.TextBox;
            s_100_3.Value = RSW2data.s_100_3.HasValue ? Utils.decToStr(RSW2data.s_100_3.Value) : "0.00";

            Telerik.Reporting.TextBox s_110_0 = RSW2_2014_1.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
            s_110_0.Value = RSW2data.s_110_0.HasValue ? Utils.decToStr(RSW2data.s_110_0.Value) : "0.00";
            Telerik.Reporting.TextBox s_110_3 = RSW2_2014_1.Items.Find("textBox39", true)[0] as Telerik.Reporting.TextBox;
            s_110_3.Value = RSW2data.s_110_3.HasValue ? Utils.decToStr(RSW2data.s_110_3.Value) : "0.00";

            Telerik.Reporting.TextBox s_120_0 = RSW2_2014_1.Items.Find("textBox74", true)[0] as Telerik.Reporting.TextBox;
            s_120_0.Value = RSW2data.s_120_0.HasValue ? Utils.decToStr(RSW2data.s_120_0.Value) : "0.00";
            Telerik.Reporting.TextBox s_120_1 = RSW2_2014_1.Items.Find("textBox75", true)[0] as Telerik.Reporting.TextBox;
            s_120_1.Value = RSW2data.s_120_1.HasValue ? Utils.decToStr(RSW2data.s_120_1.Value) : "0.00";
            Telerik.Reporting.TextBox s_120_2 = RSW2_2014_1.Items.Find("textBox76", true)[0] as Telerik.Reporting.TextBox;
            s_120_2.Value = RSW2data.s_120_2.HasValue ? Utils.decToStr(RSW2data.s_120_2.Value) : "0.00";
            Telerik.Reporting.TextBox s_120_3 = RSW2_2014_1.Items.Find("textBox79", true)[0] as Telerik.Reporting.TextBox;
            s_120_3.Value = RSW2data.s_120_3.HasValue ? Utils.decToStr(RSW2data.s_120_3.Value) : "0.00";

            Telerik.Reporting.TextBox s_130_0 = RSW2_2014_1.Items.Find("textBox90", true)[0] as Telerik.Reporting.TextBox;
            s_130_0.Value = RSW2data.s_130_0.HasValue ? Utils.decToStr(RSW2data.s_130_0.Value) : "0.00";
            Telerik.Reporting.TextBox s_130_1 = RSW2_2014_1.Items.Find("textBox91", true)[0] as Telerik.Reporting.TextBox;
            s_130_1.Value = RSW2data.s_130_1.HasValue ? Utils.decToStr(RSW2data.s_130_1.Value) : "0.00";
            Telerik.Reporting.TextBox s_130_2 = RSW2_2014_1.Items.Find("textBox92", true)[0] as Telerik.Reporting.TextBox;
            s_130_2.Value = RSW2data.s_130_2.HasValue ? Utils.decToStr(RSW2data.s_130_2.Value) : "0.00";
            Telerik.Reporting.TextBox s_130_3 = RSW2_2014_1.Items.Find("textBox95", true)[0] as Telerik.Reporting.TextBox;
            s_130_3.Value = RSW2data.s_130_3.HasValue ? Utils.decToStr(RSW2data.s_130_3.Value) : "0.00";

            Telerik.Reporting.TextBox s_140_0 = RSW2_2014_1.Items.Find("textBox98", true)[0] as Telerik.Reporting.TextBox;
            s_140_0.Value = RSW2data.s_140_0.HasValue ? Utils.decToStr(RSW2data.s_140_0.Value) : "0.00";
            Telerik.Reporting.TextBox s_140_1 = RSW2_2014_1.Items.Find("textBox99", true)[0] as Telerik.Reporting.TextBox;
            s_140_1.Value = RSW2data.s_140_1.HasValue ? Utils.decToStr(RSW2data.s_140_1.Value) : "0.00";
            Telerik.Reporting.TextBox s_140_2 = RSW2_2014_1.Items.Find("textBox100", true)[0] as Telerik.Reporting.TextBox;
            s_140_2.Value = RSW2data.s_140_2.HasValue ? Utils.decToStr(RSW2data.s_140_2.Value) : "0.00";
            Telerik.Reporting.TextBox s_140_3 = RSW2_2014_1.Items.Find("textBox103", true)[0] as Telerik.Reporting.TextBox;
            s_140_3.Value = RSW2data.s_140_3.HasValue ? Utils.decToStr(RSW2data.s_140_3.Value) : "0.00";

            Telerik.Reporting.TextBox s_150_0 = RSW2_2014_1.Items.Find("textBox138", true)[0] as Telerik.Reporting.TextBox;
            s_150_0.Value = RSW2data.s_150_0.HasValue ? Utils.decToStr(RSW2data.s_150_0.Value) : "0.00";
            Telerik.Reporting.TextBox s_150_1 = RSW2_2014_1.Items.Find("textBox139", true)[0] as Telerik.Reporting.TextBox;
            s_150_1.Value = RSW2data.s_150_1.HasValue ? Utils.decToStr(RSW2data.s_150_1.Value) : "0.00";
            Telerik.Reporting.TextBox s_150_2 = RSW2_2014_1.Items.Find("textBox140", true)[0] as Telerik.Reporting.TextBox;
            s_150_2.Value = RSW2data.s_150_2.HasValue ? Utils.decToStr(RSW2data.s_150_2.Value) : "0.00";
            Telerik.Reporting.TextBox s_150_3 = RSW2_2014_1.Items.Find("textBox143", true)[0] as Telerik.Reporting.TextBox;
            s_150_3.Value = RSW2data.s_150_3.HasValue ? Utils.decToStr(RSW2data.s_150_3.Value) : "0.00";
            pageCnt++;
            RSW2_2014Book.Reports.Add(RSW2_2014_1);

            //раздел 2
            RSW2_2014_2_List = db.FormsRSW2014_2_2.Where(x => x.FormsRSW2014_2_1D == RSW2data.ID).ToList();
            if (RSW2_2014_2_List.Count() > 0 || Options.printAllPagesRSV1)
            {
                RSW2_2014_Re2 RSW2_2014_2 = new RSW2_2014_Re2();
                (RSW2_2014_2.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                if (RSW2data.Year >= 2015)
                {
                    (RSW2_2014_2.Items.Find("textBox29", true)[0] as Telerik.Reporting.TextBox).Value = "Период членства в крестьянском (фермерском) хозяйстве в расчетном периоде";
                    (RSW2_2014_2.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox).Value = "Начислено страховых взносов на обязательное пенсионное страхование (руб. коп.)";
                }


                Telerik.Reporting.TextBox RegNum2 = RSW2_2014_2.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                RegNum2.Value = regNum;

                Telerik.Reporting.DetailSection detail_Re2 = RSW2_2014_2.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                Telerik.Reporting.Table table_Re2 = RSW2_2014_2.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

                TableGroup detailGrouptable_Re2 = new TableGroup();
                RSW2_2014_2.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule, myStyleRuleLeftAlign, myStyleRuleLeftAlign2 });

                int j = 1;

                if (RSW2_2014_2_List.Count == 0)
                {
                    RSW2_2014_2_List.Add(new FormsRSW2014_2_2 { }.SetZeroValues());
                }

                foreach (var rsw22 in RSW2_2014_2_List)
                {
                    Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw22.NumRec.ToString()
                    };
                    table_Re2.Body.SetCellContent(j, 0, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyleLeftAlign",
                        Value = (rsw22.LastName != null ? rsw22.LastName : "") + " " + (rsw22.FirstName != null ? rsw22.FirstName : "") + " " + (rsw22.MiddleName != null ? rsw22.MiddleName : "")
                    };
                    table_Re2.Body.SetCellContent(j, 1, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = !String.IsNullOrEmpty(rsw22.InsuranceNumber) ? Utils.ParseSNILS(rsw22.InsuranceNumber.ToString(), (rsw22.ControlNumber.HasValue ? rsw22.ControlNumber.Value : (short)0)) : ""

                    };
                    table_Re2.Body.SetCellContent(j, 2, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw22.Year.ToString()
                    };
                    table_Re2.Body.SetCellContent(j, 3, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw22.DateBegin.HasValue ? rsw22.DateBegin.Value.ToShortDateString() : ""
                    };
                    table_Re2.Body.SetCellContent(j, 4, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw22.DateEnd.HasValue ? rsw22.DateEnd.Value.ToShortDateString() : ""
                    };
                    table_Re2.Body.SetCellContent(j, 5, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw22.SumOPS.HasValue ? Utils.decToStr(rsw22.SumOPS.Value) : "0.00"
                    };
                    table_Re2.Body.SetCellContent(j, 6, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw22.SumOMS.HasValue ? Utils.decToStr(rsw22.SumOMS.Value) : "0.00"
                    };
                    table_Re2.Body.SetCellContent(j, 7, textBox);

                    table_Re2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5D)));
                    detailGrouptable_Re2.ChildGroups.Add(new TableGroup());
                    j++;
                }

                #region  Итоговая строка

                Telerik.Reporting.TextBox textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyleLeftAlign2",
                    Value = "Итого"
                };
                table_Re2.Body.SetCellContent(j, 0, textBox_, 1, 6);

                textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(RSW2_2014_2_List.Where(x => x.SumOPS.HasValue).Sum(x => x.SumOPS.Value))
                };
                table_Re2.Body.SetCellContent(j, 6, textBox_);
                textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(RSW2_2014_2_List.Where(x => x.SumOMS.HasValue).Sum(x => x.SumOMS.Value))
                };
                table_Re2.Body.SetCellContent(j, 7, textBox_);

                table_Re2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5D)));
                detailGrouptable_Re2.ChildGroups.Add(new TableGroup());

                #endregion

                table_Re2.RowGroups.Add(detailGrouptable_Re2);

                pageCnt++;
                RSW2_2014Book.Reports.Add(RSW2_2014_2);
            }

            //раздел 3
            RSW2_2014_3_List = db.FormsRSW2014_2_3.Where(x => x.FormsRSW2014_2_1D == RSW2data.ID).ToList();
            if (RSW2_2014_3_List.Count() > 0 || Options.printAllPagesRSV1)
            {
                RSW2_2014_Re3 RSW2_2014_3 = new RSW2_2014_Re3();
                (RSW2_2014_3.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                Telerik.Reporting.TextBox RegNum3 = RSW2_2014_3.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                RegNum3.Value = regNum;

                Telerik.Reporting.DetailSection detail_Re3 = RSW2_2014_3.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                int i = 0;
                Telerik.Reporting.Table table_Re3 = new Telerik.Reporting.Table();
                if (RSW2data.Year == 2014)
                {
                    (RSW2_2014_3.Items.Find("table2", true)[0] as Telerik.Reporting.Table).Visible = false;
                    table_Re3 = RSW2_2014_3.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

                }
                else if (RSW2data.Year >= 2015)
                {
                    (RSW2_2014_3.Items.Find("textBox4", true)[0] as Telerik.Reporting.TextBox).Value = "Раздел 3. Суммы перерасчета страховых взносов с начала расчетного периода за главу и членов крестьянского (фермерского) хозяйства";
                    (RSW2_2014_3.Items.Find("shape1", true)[0] as Telerik.Reporting.Shape).Visible = true;
                    (RSW2_2014_3.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox).Visible = true;
                    (RSW2_2014_3.Items.Find("table2", true)[0] as Telerik.Reporting.Table).Visible = true;
                    (RSW2_2014_3.Items.Find("table2", true)[0] as Telerik.Reporting.Table).Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0), Telerik.Reporting.Drawing.Unit.Cm(1));
                    (RSW2_2014_3.Items.Find("table1", true)[0] as Telerik.Reporting.Table).Visible = false;
                    table_Re3 = RSW2_2014_3.Items.Find("table2", true)[0] as Telerik.Reporting.Table;
                    i++;
                }
                    

                TableGroup detailGrouptable_Re3 = new TableGroup();
                RSW2_2014_3.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule, myStyleRuleLeftAlign, myStyleRuleLeftAlign2 });

                int j = 1;

                if (RSW2_2014_3_List.Count == 0)
                {
                    RSW2_2014_3_List.Add(new FormsRSW2014_2_3 { }.SetZeroValues());
                }

                foreach (var rsw23 in RSW2_2014_3_List)
                {
                    Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw23.NumRec.ToString()
                    };
                    table_Re3.Body.SetCellContent(j, 0, textBox);


                    if (RSW2data.Year >= 2015)
                    {
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = rsw23.CodeBase.ToString()
                        };
                        table_Re3.Body.SetCellContent(j, 1, textBox);

                    }


                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyleLeftAlign",
                        Value = (rsw23.LastName != null ? rsw23.LastName : "") + " " + (rsw23.FirstName != null ? rsw23.FirstName : "") + " " + (rsw23.MiddleName != null ? rsw23.MiddleName : "")
                    };
                    table_Re3.Body.SetCellContent(j, 1 + i, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = !String.IsNullOrEmpty(rsw23.InsuranceNumber) ? Utils.ParseSNILS(rsw23.InsuranceNumber.ToString(), (rsw23.ControlNumber.HasValue ? rsw23.ControlNumber.Value : (short)0)) : ""

                    };
                    table_Re3.Body.SetCellContent(j, 2 + i, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw23.Year.ToString()
                    };
                    table_Re3.Body.SetCellContent(j, 3 + i, textBox);

                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw23.DateBegin.HasValue ? rsw23.DateBegin.Value.ToShortDateString() : ""
                    };
                    table_Re3.Body.SetCellContent(j, 4 + i, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw23.DateEnd.HasValue ? rsw23.DateEnd.Value.ToShortDateString() : ""
                    };
                    table_Re3.Body.SetCellContent(j, 5 + i, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw23.SumOPS_D.HasValue ? Utils.decToStr(rsw23.SumOPS_D.Value) : "0.00"
                    };
                    table_Re3.Body.SetCellContent(j, 6 + i, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw23.SumStrah_D.HasValue ? Utils.decToStr(rsw23.SumStrah_D.Value) : "0.00"
                    };
                    table_Re3.Body.SetCellContent(j, 7 + i, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw23.SumNakop_D.HasValue ? Utils.decToStr(rsw23.SumNakop_D.Value) : "0.00"
                    };
                    table_Re3.Body.SetCellContent(j, 8 + i, textBox);
                    textBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = rsw23.SumOMS_D.HasValue ? Utils.decToStr(rsw23.SumOMS_D.Value) : "0.00"
                    };
                    table_Re3.Body.SetCellContent(j, 9 + i, textBox);

                    table_Re3.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5D)));
                    detailGrouptable_Re3.ChildGroups.Add(new TableGroup());
                    j++;
                }

                #region  Итоговая строка

                Telerik.Reporting.TextBox textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyleLeftAlign2",
                    Value = "Итого"
                };
                table_Re3.Body.SetCellContent(j, 0, textBox_, 1, 6 + i);

                textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(RSW2_2014_3_List.Where(x => x.SumOPS_D.HasValue).Sum(x => x.SumOPS_D.Value))
                };
                table_Re3.Body.SetCellContent(j, 6 + i, textBox_);
                textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(RSW2_2014_3_List.Where(x => x.SumStrah_D.HasValue).Sum(x => x.SumStrah_D.Value))
                };
                table_Re3.Body.SetCellContent(j, 7 + i, textBox_);
                textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(RSW2_2014_3_List.Where(x => x.SumNakop_D.HasValue).Sum(x => x.SumNakop_D.Value))
                };
                table_Re3.Body.SetCellContent(j, 8 + i, textBox_);
                textBox_ = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(RSW2_2014_3_List.Where(x => x.SumOMS_D.HasValue).Sum(x => x.SumOMS_D.Value))
                };
                table_Re3.Body.SetCellContent(j, 9 + i, textBox_);

                table_Re3.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5D)));
                detailGrouptable_Re3.ChildGroups.Add(new TableGroup());

                #endregion


                table_Re3.RowGroups.Add(detailGrouptable_Re3);

                pageCnt++;
                RSW2_2014Book.Reports.Add(RSW2_2014_3);
            }

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            Telerik.Reporting.TextBox pageCount = RSW2_2014Book.Reports[0].Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
            pageCount.Value = pageCnt.ToString();

            typeReportSource.ReportDocument = RSW2_2014Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }

        public void createEmptyReport(short year)
        {
            ReportBook RSW2_2014Book = new ReportBook();

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

            Telerik.Reporting.Drawing.StyleRule myStyleRuleLeftAlign2 = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRuleLeftAlign2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyleLeftAlign2") });
            myStyleRuleLeftAlign2.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRuleLeftAlign2.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRuleLeftAlign2.Style.Font.Name = "Arial";
            myStyleRuleLeftAlign2.Style.Font.Bold = true;
            myStyleRuleLeftAlign2.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRuleLeftAlign2.Style.TextAlign = HorizontalAlign.Right;
            myStyleRuleLeftAlign2.Style.VerticalAlign = VerticalAlign.Middle;


            //Титульный лист
            RSW2_2014_ReTitle RSW2_2014_Title = new RSW2_2014_ReTitle();
            (RSW2_2014_Title.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = "";

            Telerik.Reporting.TextBox st = RSW2_2014_Title.Items.Find("textBox6", true)[0] as Telerik.Reporting.TextBox;
            st.Style.BorderStyle.Default = BorderType.Dotted;
            st.Style.TextAlign = HorizontalAlign.Left;
            st.Value = "Стр.";

            if (year >= 2015)
            {
                (RSW2_2014_Title.Items.Find("list6", true)[0] as Telerik.Reporting.List).Visible = false;
                (RSW2_2014_Title.Items.Find("list8", true)[0] as Telerik.Reporting.List).Visible = false;
                (RSW2_2014_Title.Items.Find("textBox1", true)[0] as Telerik.Reporting.TextBox).Value = "Приложение №1\r\nУтверждена \r\nпостановлением Правления ПФР \r\nот 17 сентября 2015 г. № 347п";
                (RSW2_2014_Title.Items.Find("textBox13", true)[0] as Telerik.Reporting.TextBox).Value = "Номер уточнения";
                (RSW2_2014_Title.Items.Find("textBox14", true)[0] as Telerik.Reporting.TextBox).Value = "(000 - исходная форма, уточнение 001 и т.д.)";
                (RSW2_2014_Title.Items.Find("textBox104", true)[0] as Telerik.Reporting.TextBox).Value = "       *  Указывается дата представления расчета лично главой крестьянского (фермерского) хозяйства или через представителя, при отправке по почте - дата отправки почтового отправления с описью вложения.";
            }



            RSW2_2014Book.Reports.Add(RSW2_2014_Title);

            //Раздел 1
            RSW2_2014_Re1 RSW2_2014_1 = new RSW2_2014_Re1();
            (RSW2_2014_1.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = "";

            Telerik.Reporting.TextBox s2 = RSW2_2014_1.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
            s2.Style.BorderStyle.Default = BorderType.Dotted;
            s2.Style.TextAlign = HorizontalAlign.Left;
            s2.Value = "Стр.";

            if (year >= 2015)
            {
                (RSW2_2014_1.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox).Value = "за периоды 2010-2013 гг. *";
                (RSW2_2014_1.Items.Find("textBox72", true)[0] as Telerik.Reporting.TextBox).Value = "Сумма перерасчета страховых взносов за предыдущие расчетные периоды с начала расчетного периода";
                (RSW2_2014_1.Items.Find("textBox31", true)[0] as Telerik.Reporting.TextBox).Value = "на страховую пенсию";
                (RSW2_2014_1.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox).Value = "на накопительную пенсию";

                (RSW2_2014_1.Items.Find("textBox17", true)[0] as Telerik.Reporting.TextBox).Visible = true;
                (RSW2_2014_1.Items.Find("shape1", true)[0] as Telerik.Reporting.Shape).Visible = true;

            }

            RSW2_2014Book.Reports.Add(RSW2_2014_1);

            //раздел 2

                RSW2_2014_Re2 RSW2_2014_2 = new RSW2_2014_Re2();
                (RSW2_2014_2.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = "";
                Telerik.Reporting.TextBox s3 = RSW2_2014_2.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                s3.Style.BorderStyle.Default = BorderType.Dotted;
                s3.Style.TextAlign = HorizontalAlign.Left;
                s3.Value = "Стр.";

                if (year >= 2015)
                {
                    (RSW2_2014_2.Items.Find("textBox29", true)[0] as Telerik.Reporting.TextBox).Value = "Период членства в крестьянском (фермерском) хозяйстве в расчетном периоде";
                    (RSW2_2014_2.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox).Value = "Начислено страховых взносов на обязательное пенсионное страхование (руб. коп.)";
                }


                Telerik.Reporting.Table table_Re2 = RSW2_2014_2.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
                TableGroup detailGrouptable_Re2 = new TableGroup();
                RSW2_2014_2.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule, myStyleRuleLeftAlign, myStyleRuleLeftAlign2 });

            for (int i = 1; i <= 20; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    table_Re2.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " ", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5D)});
                }
            }

            table_Re2.Body.SetCellContent(21, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyleLeftAlign2", Value = "Итого:", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5D) }, 1, 6);
            table_Re2.Body.SetCellContent(21, 6, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " ", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5D) });
            table_Re2.Body.SetCellContent(21, 7, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " ", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5D) });

            for (int i = 0; i <= 20; i++)
            {
                table_Re2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_Re2.ChildGroups.Add(new TableGroup());
            }

            table_Re2.RowGroups.Add(detailGrouptable_Re2);
            RSW2_2014Book.Reports.Add(RSW2_2014_2);

            //раздел 3
                RSW2_2014_Re3 RSW2_2014_3 = new RSW2_2014_Re3();
                (RSW2_2014_3.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = "";
                Telerik.Reporting.TextBox s4 = RSW2_2014_3.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                s4.Style.BorderStyle.Default = BorderType.Dotted;
                s4.Style.TextAlign = HorizontalAlign.Left;
                s4.Value = "Стр.";

                int k = 0;

                Telerik.Reporting.Table table_Re3 = new Telerik.Reporting.Table();
                if (year == 2014)
                {
                    (RSW2_2014_3.Items.Find("table2", true)[0] as Telerik.Reporting.Table).Visible = false;
                    table_Re3 = RSW2_2014_3.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

                }
                else if (year >= 2015)
                {
                    (RSW2_2014_3.Items.Find("textBox4", true)[0] as Telerik.Reporting.TextBox).Value = "Раздел 3. Суммы перерасчета страховых взносов с начала расчетного периода за главу и членов крестьянского (фермерского) хозяйства";
                    (RSW2_2014_3.Items.Find("shape1", true)[0] as Telerik.Reporting.Shape).Visible = true;
                    (RSW2_2014_3.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox).Visible = true;
                    (RSW2_2014_3.Items.Find("table2", true)[0] as Telerik.Reporting.Table).Visible = true;
                    (RSW2_2014_3.Items.Find("table2", true)[0] as Telerik.Reporting.Table).Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0), Telerik.Reporting.Drawing.Unit.Cm(1));
                    (RSW2_2014_3.Items.Find("table1", true)[0] as Telerik.Reporting.Table).Visible = false;
                    table_Re3 = RSW2_2014_3.Items.Find("table2", true)[0] as Telerik.Reporting.Table;
                    k++;
                }


                TableGroup detailGrouptable_Re3 = new TableGroup();
                RSW2_2014_3.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule, myStyleRuleLeftAlign, myStyleRuleLeftAlign2 });


            for (int i = 1; i <= 17; i++)
            {
                for (int j = 0; j <= (9 + k); j++)
                {
                    table_Re3.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " ", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5D) });
                }
            }

            table_Re3.Body.SetCellContent(18, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyleLeftAlign2", Value = "Итого:", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5D) }, 1, 6 + k);
                for (int j = (6 + k); j <= (9 + k); j++)
                {
                    table_Re3.Body.SetCellContent(18, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " ", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5D) });
                }

            for (int i = 0; i <= 17; i++)
            {
                table_Re3.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5D)));
                detailGrouptable_Re3.ChildGroups.Add(new TableGroup());
            }

            table_Re3.RowGroups.Add(detailGrouptable_Re3);
            RSW2_2014Book.Reports.Add(RSW2_2014_3);

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = RSW2_2014Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }

        private void RSW2_2014_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }
    }
}
