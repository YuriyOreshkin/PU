using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.FormsSZV_STAJ.Report;
using PU.FormsSZV_ISH.Report;
using Telerik.Reporting;
using PU.Classes;
using Telerik.Reporting.Drawing;
using PU.FormsODV1.Report;
using PU.FormsSZV_KORR.Report;

namespace PU.FormsODV1
{
    public partial class ODV1_Print : Telerik.WinControls.UI.RadForm
    {
        public List<FormsSZV_STAJ_2017> SZV_STAJ_List = new List<FormsSZV_STAJ_2017>();
        public List<FormsSZV_ISH_2017> SZV_ISH_List = new List<FormsSZV_ISH_2017>();
        public List<FormsSZV_KORR_2017> SZV_KORR_List = new List<FormsSZV_KORR_2017>();
        public List<StajOsn> Staj_List = new List<StajOsn>();
        public FormsODV_1_2017 ODV1_Data { get; set; }
        public bool printODV1 = false;
        public byte TypeForm { get; set; }
        private Insurer ins;
        private string regNum { get; set; }
        pu6Entities db = new pu6Entities();

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

        List<MonthesDict> MonthesShort = new List<MonthesDict> { 
            new MonthesDict{Code = 1, Name = "Янв"},
            new MonthesDict{Code = 2, Name = "Фев"},
            new MonthesDict{Code = 3, Name = "Мрт"},
            new MonthesDict{Code = 4, Name = "Апр"},
            new MonthesDict{Code = 5, Name = "Май"},
            new MonthesDict{Code = 6, Name = "Июн"},
            new MonthesDict{Code = 7, Name = "Июл"},
            new MonthesDict{Code = 8, Name = "Авг"},
            new MonthesDict{Code = 9, Name = "Сен"},
            new MonthesDict{Code = 10, Name = "Окт"},
            new MonthesDict{Code = 11, Name = "Нбр"},
            new MonthesDict{Code = 12, Name = "Дек"}
        };


        public ODV1_Print()
        {
            InitializeComponent();
        }

        private void FormsODV1_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            ReportBook Rep_Book = new ReportBook();

            ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);
            regNum = Utils.ParseRegNum(ins.RegNum);



            if (ODV1_Data != null)
            {
                TypeForm = ODV1_Data.TypeForm.HasValue ? ODV1_Data.TypeForm.Value : (byte)4;
                Rep_Book.Reports.Add(ODV1_Report());
            }


            switch (TypeForm)
            {
                case 1:
                    //                    if (db.FormsSZV_STAJ_2017.Any(x => x.FormsODV_1_2017_ID == ODV1_Data.ID))
                    Rep_Book.Reports.Add(SZVSTAJ_Report());
                    break;
                case 2:
                    if (ODV1_Data != null)
                        SZV_ISH_List = db.FormsSZV_ISH_2017.Where(x => x.FormsODV_1_2017_ID == ODV1_Data.ID).OrderBy(x => x.Staff.LastName).ThenBy(x => x.Staff.FirstName).ThenBy(x => x.Staff.MiddleName).ToList();

                    foreach (var item in SZV_ISH_List)
                    {
                        Rep_Book.Reports.Add(SZVISH_Report(item));
                    }
                    break;
                case 3:
                    if (ODV1_Data != null)
                        SZV_KORR_List = db.FormsSZV_KORR_2017.Where(x => x.FormsODV_1_2017_ID == ODV1_Data.ID).OrderBy(x => x.Staff.LastName).ThenBy(x => x.Staff.FirstName).ThenBy(x => x.Staff.MiddleName).ToList();

                    foreach (var item in SZV_KORR_List)
                    {
                        Rep_Book.Reports.Add(SZVKORR_Report(item));
                    }
                    break;

            }





            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            db.Dispose();
            db = null;

            typeReportSource.ReportDocument = Rep_Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;


        }

        public class odvRazd5Table
        {
            public string col1 { get; set; }
            public string col2 { get; set; }
            public string col3 { get; set; }
            public string col4 { get; set; }
            public string col5 { get; set; }
            public string col6 { get; set; }
            public string col7 { get; set; }
            public string col8 { get; set; }
            public string col9 { get; set; }
        }

        private FormsODV1_Rep ODV1_Report()
        {
            FormsODV1_Rep ODV1 = new FormsODV1_Rep();

            (ODV1.Items.Find("RegNum", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
            (ODV1.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN != null ? ins.INN.ToString() : "";
            (ODV1.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP != null ? ins.KPP.ToString() : "";


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

            (ODV1.Items.Find("nameShort", true)[0] as Telerik.Reporting.TextBox).Value = orgName;

            (ODV1.Items.Find("Code", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.Code != null ? ODV1_Data.Code.ToString() : ""; ;
            (ODV1.Items.Find("Year", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.Year != null ? ODV1_Data.Year.ToString() : ""; ;

            switch (ODV1_Data.TypeInfo)
            {
                case 0://исходная
                    (ODV1.Items.Find("ishCheckBox", true)[0] as Telerik.Reporting.TextBox).Value = "X";
                    break;
                case 1://корректирующая
                    (ODV1.Items.Find("korrCheckBox", true)[0] as Telerik.Reporting.TextBox).Value = "X";
                    break;
                case 2://отменяющая
                    (ODV1.Items.Find("otmCheckBox", true)[0] as Telerik.Reporting.TextBox).Value = "X";
                    break;
            }

            if (ODV1_Data.StaffCount.HasValue)
            {
                switch (TypeForm)
                {
                    case 1:
                        (ODV1.Items.Find("StaffCnt_SZVStaj", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.StaffCount.Value.ToString();
                        break;
                    case 2:
                        (ODV1.Items.Find("StaffCnt_SZVIsh", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.StaffCount.Value.ToString();
                        break;
                    case 3:
                        (ODV1.Items.Find("StaffCnt_SZVKorr", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.StaffCount.Value.ToString();
                        break;
                }
            }

            (ODV1.Items.Find("s_0_0", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_0_0.HasValue ? Utils.decToStr(ODV1_Data.s_0_0.Value) : "0.00";
            (ODV1.Items.Find("s_0_1", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_0_1.HasValue ? Utils.decToStr(ODV1_Data.s_0_1.Value) : "0.00";
            (ODV1.Items.Find("s_0_2", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_0_2.HasValue ? Utils.decToStr(ODV1_Data.s_0_2.Value) : "0.00";
            (ODV1.Items.Find("s_0_3", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_0_3.HasValue ? Utils.decToStr(ODV1_Data.s_0_3.Value) : "0.00";
            (ODV1.Items.Find("s_1_0", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_1_0.HasValue ? Utils.decToStr(ODV1_Data.s_1_0.Value) : "0.00";
            (ODV1.Items.Find("s_1_1", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_1_1.HasValue ? Utils.decToStr(ODV1_Data.s_1_1.Value) : "0.00";
            (ODV1.Items.Find("s_1_2", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_1_2.HasValue ? Utils.decToStr(ODV1_Data.s_1_2.Value) : "0.00";
            (ODV1.Items.Find("s_1_3", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_1_3.HasValue ? Utils.decToStr(ODV1_Data.s_1_3.Value) : "0.00";
            (ODV1.Items.Find("s_2_0", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_2_0.HasValue ? Utils.decToStr(ODV1_Data.s_2_0.Value) : "0.00";
            (ODV1.Items.Find("s_2_1", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_2_1.HasValue ? Utils.decToStr(ODV1_Data.s_2_1.Value) : "0.00";
            (ODV1.Items.Find("s_2_2", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_2_2.HasValue ? Utils.decToStr(ODV1_Data.s_2_2.Value) : "0.00";
            (ODV1.Items.Find("s_2_3", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.s_2_3.HasValue ? Utils.decToStr(ODV1_Data.s_2_3.Value) : "0.00";

            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Padding.Top = new Unit(3.0, UnitType.Pixel);
            myStyleRule.Style.Padding.Bottom = new Unit(3.0, UnitType.Pixel);
            myStyleRule.Style.Padding.Left = new Unit(0.5, UnitType.Pixel);
            myStyleRule.Style.Padding.Right = new Unit(0.5, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(6.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            Telerik.Reporting.Drawing.StyleRule myStyleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle2") });
            myStyleRule2.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule2.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule2.Style.Padding.Top = new Unit(3.0, UnitType.Pixel);
            myStyleRule2.Style.Padding.Bottom = new Unit(3.0, UnitType.Pixel);
            myStyleRule2.Style.Padding.Left = new Unit(0.5, UnitType.Pixel);
            myStyleRule2.Style.Padding.Right = new Unit(0.5, UnitType.Pixel);
            myStyleRule2.Style.Font.Name = "Arial";
            myStyleRule2.Style.Font.Size = new Unit(6.0, UnitType.Point);
            myStyleRule2.Style.TextAlign = HorizontalAlign.Right;
            myStyleRule2.Style.VerticalAlign = VerticalAlign.Middle;


            ODV1.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule, myStyleRule2 });

            Telerik.Reporting.Table table_4 = ODV1.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_4 = new TableGroup();

            int j = 1;
            if (ODV1_Data.FormsODV_1_4_2017.Count > 0)
            {
                (ODV1.Items.Find("textBox42", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(ODV1_Data.FormsODV_1_4_2017.Sum(x => x.OPS.Value));
                (ODV1.Items.Find("textBox45", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(ODV1_Data.FormsODV_1_4_2017.Sum(x => x.NAKOP.Value));
                (ODV1.Items.Find("textBox49", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(ODV1_Data.FormsODV_1_4_2017.Sum(x => x.DopTar.Value));


                foreach (var item in ODV1_Data.FormsODV_1_4_2017.ToList())
                {
                    table_4.Body.SetCellContent(j, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "за " + item.Year.ToString() + " год", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                    table_4.Body.SetCellContent(j, 1, new Telerik.Reporting.TextBox { StyleName = "TableStyle2", Value = Utils.decToStr(item.OPS), Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                    table_4.Body.SetCellContent(j, 2, new Telerik.Reporting.TextBox { StyleName = "TableStyle2", Value = Utils.decToStr(item.NAKOP), Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                    table_4.Body.SetCellContent(j, 3, new Telerik.Reporting.TextBox { StyleName = "TableStyle2", Value = Utils.decToStr(item.DopTar), Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });


                    table_4.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5D)));
                    detailGrouptable_4.ChildGroups.Add(new TableGroup());

                    j++;
                }
                table_4.RowGroups.Add(detailGrouptable_4);
            }


            List<odvRazd5Table> odvRazd5List = new List<odvRazd5Table>();

            Telerik.Reporting.Table table_5 = ODV1.Items.Find("table12", true)[0] as Telerik.Reporting.Table;
//            TableGroup detailGrouptable_5 = new TableGroup();

            foreach (var item in ODV1_Data.FormsODV_1_5_2017.ToList())
            {
                odvRazd5Table newRec = new odvRazd5Table {
                    col1 = item.Num.HasValue ? item.Num.Value.ToString() : j.ToString(),
                    col2 = item.Department,
                    col3 = item.Profession,
                    col4 = item.StaffCountShtat.HasValue ? item.StaffCountShtat.Value.ToString() : "",
                    col5 = item.StaffCountFakt.HasValue ? item.StaffCountFakt.Value.ToString() : "",
                    col6 = item.VidRabotFakt,
                    col7 = item.DocsName
                };

                j = 0;
                foreach (var it in item.FormsODV_1_5_2017_OUT.ToList())
                {
                    if (j > 0)
                        newRec = new odvRazd5Table();

                    newRec.col8 = it.OsobUslTrudaCode;
                    newRec.col9 = it.CodePosition;

                    odvRazd5List.Add(newRec);

                    j++;
                }

                if (item.FormsODV_1_5_2017_OUT.Count() <= 0)
                {
                    odvRazd5List.Add(newRec);
                }

                //table_5.Body.SetCellContent(j, 0, new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = item.Num.HasValue ? item.Num.Value.ToString() : j.ToString(),
                //    TextWrap = true,
                //    Multiline = true,
                //    CanGrow = true
                //});

                //table_5.Body.SetCellContent(j, 1, new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = item.Department,
                //    TextWrap = true,
                //    Multiline = true,
                //    CanGrow = true
                //});

                //table_5.Body.SetCellContent(j, 2, new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = item.Profession,
                //    TextWrap = true,
                //    Multiline = true,
                //    CanGrow = true,
                //    Height = new Unit(item.Profession.Length > 11 ? 20 : 12, UnitType.Pixel)
                //});

                //table_5.Body.SetCellContent(j, 3, new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = item.StaffCountShtat.HasValue ? item.StaffCountShtat.Value.ToString() : "",
                //    TextWrap = true,
                //    Multiline = true,
                //    CanGrow = true
                //});
                //table_5.Body.SetCellContent(j, 4, new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = item.StaffCountFakt.HasValue ? item.StaffCountFakt.Value.ToString() : "",
                //    TextWrap = true,
                //    Multiline = true,
                //    CanGrow = true
                //});
                //table_5.Body.SetCellContent(j, 5, new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = item.VidRabotFakt,
                //    TextWrap = true,
                //    Multiline = true,
                //    CanGrow = true,
                //    Height = new Unit(item.VidRabotFakt.Length > 11 ? 20 : 12, UnitType.Pixel)
                //});
                //table_5.Body.SetCellContent(j, 6, new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = item.DocsName,
                //    TextWrap = true,
                //    Multiline = true,
                //    CanGrow = true,
                //    Height = new Unit(item.DocsName.Length > 11 ? 20 : 12, UnitType.Pixel)
                //});
                //table_5.Body.SetCellContent(j, 7, new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = "",//item.OsobUslTrudaCode,
                //    TextWrap = true,
                //    Multiline = true,
                //    CanGrow = true
                //});
                //table_5.Body.SetCellContent(j, 8, new Telerik.Reporting.TextBox
                //{
                //    StyleName = "TableStyle",
                //    Value = "",//item.CodePosition,
                //    TextWrap = true,
                //    Multiline = true,
                //    CanGrow = true
                //});


                //table_5.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(1.4D)));
                //detailGrouptable_5.ChildGroups.Add(new TableGroup());
            }
            table_5.DataSource = odvRazd5List;

            //if (ODV1_Data.FormsODV_1_5_2017.Count > 0)
            //    table_5.RowGroups.Add(detailGrouptable_5);


            (ODV1.Items.Find("StaffCountOsobUslShtat", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.StaffCountOsobUslShtat.HasValue ? ODV1_Data.StaffCountOsobUslShtat.Value.ToString() : "";
            (ODV1.Items.Find("StaffCountOsobUslFakt", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.StaffCountOsobUslFakt.HasValue ? ODV1_Data.StaffCountOsobUslFakt.Value.ToString() : "";

            (ODV1.Items.Find("ConfirmDolgn", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.ConfirmDolgn;
            (ODV1.Items.Find("ConfirmFIO", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.ConfirmLastName + " " + ODV1_Data.ConfirmFirstName + " " + ODV1_Data.ConfirmMiddleName;
            
            (ODV1.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.DateFilling.ToString("dd.MM.yyyy");


            return ODV1;

        }

        public class stajTableCont
        {
            public string Num { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string SNILS { get; set; }
            public string DateFrom { get; set; }
            public string DateTo { get; set; }
            public string TU { get; set; }
            public string OUT { get; set; }
            public string OUT_Osn { get; set; }
            public string OUT_Dop { get; set; }
            public string DNP_Osn { get; set; }
            public string DNP_Dop { get; set; }
            public string Col14 { get; set; }
        }

        private SZV_STAJ_Rep SZVSTAJ_Report()
        {
            SZV_STAJ_Rep SZVSTAJ = new SZV_STAJ_Rep();

            if (ODV1_Data != null)
                SZV_STAJ_List = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == ODV1_Data.ID).OrderBy(x => x.Staff.LastName).ThenBy(x => x.Staff.FirstName).ThenBy(x => x.Staff.MiddleName).ToList();


            (SZVSTAJ.Items.Find("RegNum", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
            (SZVSTAJ.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN != null ? ins.INN.ToString() : "";
            (SZVSTAJ.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP != null ? ins.KPP.ToString() : "";

            (SZVSTAJ.Items.Find("RegNum2", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
            (SZVSTAJ.Items.Find("INN2", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN != null ? ins.INN.ToString() : "";
            (SZVSTAJ.Items.Find("KPP2", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP != null ? ins.KPP.ToString() : "";

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

            (SZVSTAJ.Items.Find("nameShort", true)[0] as Telerik.Reporting.TextBox).Value = orgName;


            if (SZV_STAJ_List.Count() > 0)
            {
                FormsSZV_STAJ_2017 szv_staj_first = SZV_STAJ_List[0];

                (SZVSTAJ.Items.Find("Year", true)[0] as Telerik.Reporting.TextBox).Value = szv_staj_first.Year != null ? szv_staj_first.Year.ToString() : ""; ;

                switch (szv_staj_first.TypeInfo)
                {
                    case 0://исходная
                        (SZVSTAJ.Items.Find("ishCheckBox", true)[0] as Telerik.Reporting.TextBox).Value = "X";
                        break;
                    case 1://дополняющая
                        (SZVSTAJ.Items.Find("dopCheckBox", true)[0] as Telerik.Reporting.TextBox).Value = "X";
                        break;
                    case 2://назначение пенсии
                        (SZVSTAJ.Items.Find("naznCheckBox", true)[0] as Telerik.Reporting.TextBox).Value = "X";

                        if (szv_staj_first.OPSFeeNach.HasValue)
                        {
                            string OPSFeeNach = szv_staj_first.OPSFeeNach.Value == 1 ? "OPSFeeNachYes" : "OPSFeeNachNo";
                            (SZVSTAJ.Items.Find(OPSFeeNach, true)[0] as Telerik.Reporting.TextBox).Value = "X";
                        }

                        if (szv_staj_first.DopTarFeeNach.HasValue)
                        {
                            string DopTarFeeNach = szv_staj_first.DopTarFeeNach.Value == 1 ? "DopTarFeeNachYes" : "DopTarFeeNachNo";
                            (SZVSTAJ.Items.Find(DopTarFeeNach, true)[0] as Telerik.Reporting.TextBox).Value = "X";
                        }
                        break;
                }




                //Делаем стиль для таблички


                Telerik.Reporting.Table table_1 = SZVSTAJ.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
                TableGroup detailGrouptable_SZVSTAJ = new TableGroup();


                Telerik.Reporting.Drawing.Style myStyle1_ = (table_1.Items.Find("textBox43", true)[0] as Telerik.Reporting.TextBox).Style;
                Telerik.Reporting.Drawing.Style myStyle2_ = (table_1.Items.Find("textBox49", true)[0] as Telerik.Reporting.TextBox).Style;
                Telerik.Reporting.Drawing.Style myStyle3_ = (table_1.Items.Find("textBox50", true)[0] as Telerik.Reporting.TextBox).Style;

                Telerik.Reporting.Drawing.StyleRule myStyle1 = new Telerik.Reporting.Drawing.StyleRule();
                myStyle1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle1") });
                myStyle1.Style.BorderStyle.Default = BorderType.None;
                myStyle1.Style.BorderWidth.Default = myStyle1_.BorderWidth.Default;
                myStyle1.Style.Font.Name = myStyle1_.Font.Name;
                myStyle1.Style.Font.Size = myStyle1_.Font.Size;
                myStyle1.Style.TextAlign = myStyle1_.TextAlign;
                myStyle1.Style.VerticalAlign = myStyle1_.VerticalAlign;

                Telerik.Reporting.Drawing.StyleRule myStyle2 = new Telerik.Reporting.Drawing.StyleRule();
                myStyle2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle2") });
                myStyle2.Style.BorderStyle.Bottom = myStyle2_.BorderStyle.Bottom;
                myStyle2.Style.BorderWidth.Default = myStyle2_.BorderWidth.Default;
                myStyle2.Style.Font.Name = myStyle2_.Font.Name;
                myStyle2.Style.Font.Size = myStyle2_.Font.Size;
                myStyle2.Style.TextAlign = myStyle2_.TextAlign;
                myStyle2.Style.VerticalAlign = myStyle2_.VerticalAlign;

                Telerik.Reporting.Drawing.StyleRule myStyle3 = new Telerik.Reporting.Drawing.StyleRule();
                myStyle3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle3") });
                myStyle3.Style.BorderStyle.Default = myStyle3_.BorderStyle.Default;
                myStyle3.Style.BorderWidth.Default = myStyle3_.BorderWidth.Default;
                myStyle3.Style.Font.Name = myStyle3_.Font.Name;
                myStyle3.Style.Font.Size = myStyle3_.Font.Size;
                myStyle3.Style.TextAlign = myStyle3_.TextAlign;
                myStyle3.Style.VerticalAlign = myStyle3_.VerticalAlign;

                SZVSTAJ.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyle1, myStyle2, myStyle3 });// myStyleRule, myStyleRuleLeft,

                if (SZV_STAJ_List[0].FormsSZV_STAJ_4_2017.Count > 0)
                {
                    var t_4 = SZV_STAJ_List[0].FormsSZV_STAJ_4_2017.First();
                    (SZVSTAJ.Items.Find("textBox49", true)[0] as Telerik.Reporting.TextBox).Value = t_4.DNPO_DateFrom.HasValue ? t_4.DNPO_DateFrom.Value.ToString("dd.MM.yyyy") : "";
                    (SZVSTAJ.Items.Find("textBox41", true)[0] as Telerik.Reporting.TextBox).Value = t_4.DNPO_DateTo.HasValue ? t_4.DNPO_DateTo.Value.ToString("dd.MM.yyyy") : "";
                    if (t_4.DNPO_Fee.HasValue)
                    {
                        if (t_4.DNPO_Fee.Value)
                        {
                            (SZVSTAJ.Items.Find("textBox50", true)[0] as Telerik.Reporting.TextBox).Value = "X";
                        }
                        else
                        {
                            (SZVSTAJ.Items.Find("textBox53", true)[0] as Telerik.Reporting.TextBox).Value = "X";
                        }
                    }

                    int i = 1;
                    var t_4_list = SZV_STAJ_List[0].FormsSZV_STAJ_4_2017.Skip(1).ToList();
                    foreach (var item in t_4_list)
                    {
                        table_1.Body.SetCellContent(i, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle1", Value = "пенсионные взносы за период с", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                        table_1.Body.SetCellContent(i, 1, new Telerik.Reporting.TextBox { StyleName = "TableStyle2", Value = item.DNPO_DateFrom.HasValue ? item.DNPO_DateFrom.Value.ToString("dd.MM.yyyy") : "", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                        table_1.Body.SetCellContent(i, 2, new Telerik.Reporting.TextBox { StyleName = "TableStyle1", Value = "по", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                        table_1.Body.SetCellContent(i, 3, new Telerik.Reporting.TextBox { StyleName = "TableStyle2", Value = item.DNPO_DateTo.HasValue ? item.DNPO_DateTo.Value.ToString("dd.MM.yyyy") : "", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                        table_1.Body.SetCellContent(i, 4, new Telerik.Reporting.TextBox { StyleName = "TableStyle1", Value = ", уплачены:  да -", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                        table_1.Body.SetCellContent(i, 5, new Telerik.Reporting.TextBox { StyleName = "TableStyle3", Value = item.DNPO_Fee.HasValue ? (item.DNPO_Fee.Value ? "X" : "") : "", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                        table_1.Body.SetCellContent(i, 6, new Telerik.Reporting.TextBox { StyleName = "TableStyle1", Value = "нет -", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });
                        table_1.Body.SetCellContent(i, 7, new Telerik.Reporting.TextBox { StyleName = "TableStyle3", Value = item.DNPO_Fee.HasValue ? (!item.DNPO_Fee.Value ? "X" : "") : "", Height = Telerik.Reporting.Drawing.Unit.Cm(0.5) });



                        table_1.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5)));
                        detailGrouptable_SZVSTAJ.ChildGroups.Add(new TableGroup());

                        i++;

                    }
                if (t_4_list.Count() >= 1)
                {
                    table_1.RowGroups.Add(detailGrouptable_SZVSTAJ);
                }
                }

                (SZVSTAJ.Items.Find("ConfirmDolgn", true)[0] as Telerik.Reporting.TextBox).Value = szv_staj_first.ConfirmDolgn;
                (SZVSTAJ.Items.Find("ConfirmFIO", true)[0] as Telerik.Reporting.TextBox).Value = szv_staj_first.ConfirmFIO;
                (SZVSTAJ.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = szv_staj_first.DateFilling.ToString("dd.MM.yyyy");




                var TerrUsl_list = db.TerrUsl.ToList();
                var OsobUslTruda_list = db.OsobUslTruda.ToList();
                var KodVred_2_list = db.KodVred_2.ToList();
                var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
                var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
                var UslDosrNazn_list = db.UslDosrNazn.ToList();
                var PlatCategory_list = db.PlatCategory.ToList();
                var SpecOcenkaUslTruda_list = db.SpecOcenkaUslTruda.ToList();


                int js = 1;
                int cnt = 1;

                List<stajTableCont> stajTableList = new List<stajTableCont>();


                foreach (var szv_staj in SZV_STAJ_List)
                {
                    stajTableCont recNew = new stajTableCont();
                    Staff staff = szv_staj.Staff;

                    //стаж
                    ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

                    Staj_List = szv_staj.StajOsn.OrderBy(x => x.Number.Value).ToList();

                    recNew.Num = cnt.ToString();
                    recNew.LastName = staff.LastName.ToUpper();
                    recNew.FirstName = staff.FirstName.ToUpper();
                    recNew.MiddleName = staff.MiddleName.ToUpper();
                    recNew.SNILS = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber.ToString(), (staff.ControlNumber.HasValue ? staff.ControlNumber.Value : (short)0)) : "";

                    int j = js;
                    int l = js;

                    int cntSt = 0;

                    foreach (var Staj_item in Staj_List)
                    {
                        cntSt++;

                        if (j > l)
                        {
                            recNew = new stajTableCont();
                        }

                        recNew.DateFrom = Convert.ToDateTime(Staj_item.DateBegin).ToString("dd/MM/yyyy");
                        recNew.DateTo = Convert.ToDateTime(Staj_item.DateEnd).ToString("dd/MM/yyyy");


                        bool print3112 = false;

                        if (cntSt == Staj_List.Count && Staj_item.DateEnd == new DateTime(szv_staj.Year,12,31))  // Проверяем последняя ли запись в списке стажа и последняя дата стажа 31,12,год
                        {
                            print3112 = true;
                        }


                        if (Staj_item.StajLgot.Count() > 0)
                        {
                            int ii = 0;
                            foreach (var staj_lgot in Staj_item.StajLgot.OrderBy(x => x.Number.Value).ToList())
                            {
                                if (ii > 0) // если несколько записей о льготном стаже
                                {
                                    recNew = new stajTableCont();
                                }

                                if (staj_lgot.TerrUslID != null && staj_lgot.TerrUslID.Value.ToString() != "" && TerrUsl_list.Any(x => x.ID == staj_lgot.TerrUslID))
                                {
                                    TerrUsl tu = TerrUsl_list.First(x => x.ID == staj_lgot.TerrUslID);
                                    string koef = "";
                                    if (staj_lgot.TerrUslKoef.HasValue && staj_lgot.TerrUslKoef.Value != 0)
                                    {
                                        koef = Utils.decToStr(staj_lgot.TerrUslKoef.Value);
                                    }

                                    recNew.TU = tu.Code == null ? "" : (tu.Code.ToString() + "  " + koef);
                                }

                                if (staj_lgot.OsobUslTrudaID != null && staj_lgot.OsobUslTrudaID.Value.ToString() != "" && OsobUslTruda_list.Any(x => x.ID == staj_lgot.OsobUslTrudaID))
                                {
                                    OsobUslTruda ou = OsobUslTruda_list.First(x => x.ID == staj_lgot.OsobUslTrudaID);

                                    recNew.OUT = ou.Code == null ? "" : ou.Code.ToString();
                                }

                                IschislStrahStajOsn isso = IschislStrahStajOsn_list.FirstOrDefault(x => x.ID == staj_lgot.IschislStrahStajOsnID);

                                string str = staj_lgot.IschislStrahStajDopID == null ? "  " : staj_lgot.IschislStrahStajDopID.HasValue ? IschislStrahStajDop_list.FirstOrDefault(x => x.ID == staj_lgot.IschislStrahStajDopID).Code : "  ";
                                string s1 = staj_lgot.Strah1Param.HasValue ? staj_lgot.Strah1Param.Value.ToString() : "0";
                                string s2 = staj_lgot.Strah2Param.HasValue ? staj_lgot.Strah2Param.Value.ToString() : "0";

                                str = ((!String.IsNullOrEmpty(str.Trim()) || (s1 != "0") || (s2 != "0"))) ? "[" + s1 + "][" + s2 + "][" + str + "]" : "";

                                recNew.OUT_Osn = isso == null ? "" : isso.Code == null ? "" : isso.Code.ToString();
                                recNew.OUT_Dop = str;


                                if (staj_lgot.UslDosrNaznID != null)
                                {
                                    UslDosrNazn udn = UslDosrNazn_list.FirstOrDefault(x => x.ID == staj_lgot.UslDosrNaznID);

                                    s1 = staj_lgot.UslDosrNazn1Param.HasValue ? staj_lgot.UslDosrNazn1Param.Value.ToString() : "0";
                                    s2 = staj_lgot.UslDosrNazn2Param.HasValue ? staj_lgot.UslDosrNazn2Param.Value.ToString() : "0";
                                    string s3 = staj_lgot.UslDosrNazn3Param.HasValue == true ? Utils.decToStr(staj_lgot.UslDosrNazn3Param.Value) : "0";

                                    str = "[" + s1 + "][" + s2 + "][" + s3 + "]";

                                    recNew.DNP_Osn = udn == null ? "" : udn.Code == null ? "" : udn.Code.ToString();
                                    recNew.DNP_Dop = str;
                                }


                                if (szv_staj.Dismissed.HasValue && szv_staj.Dismissed.Value && print3112)
                                {
                                    recNew.Col14 = szv_staj.Dismissed.HasValue ? (szv_staj.Dismissed.Value ? (szv_staj.Year.ToString() + "-12-31") : "") : "";
                                }
                                else if (Staj_item.CodeBEZR.HasValue && Staj_item.CodeBEZR.Value)
                                {
                                    recNew.Col14 = Staj_item.CodeBEZR.HasValue ? (Staj_item.CodeBEZR.Value ? "БЕЗР" : "") : "";
                                }

                                ii++;

                                if (Staj_item.StajLgot.Count() > 1 && ii != Staj_item.StajLgot.Count())
                                {
                                    j++;
                                    stajTableList.Add(recNew);
                                    js++;
                                }

                                if (staj_lgot.KodVred_OsnID != null && staj_lgot.KodVred_OsnID.Value.ToString() != "" && KodVred_2_list.Any(x => x.ID == staj_lgot.KodVred_OsnID))
                                {
                                    if ((Staj_item.StajLgot.Count()) == 1 || (Staj_item.StajLgot.Count() > 1 && ii == Staj_item.StajLgot.Count()))
                                    {
                                        j++;
                                        stajTableList.Add(recNew);
                                        js++;
                                    }

                                    recNew = new stajTableCont();

                                    KodVred_2 kv = KodVred_2_list.First(x => x.ID == staj_lgot.KodVred_OsnID);

                                    recNew.OUT = kv.Code == null ? "" : kv.Code.ToString();
                                    recNew.OUT_Osn = kv.Code == null ? "" : kv.Code.ToString();
                                    if (Staj_item.StajLgot.Count() > 1 && ii != Staj_item.StajLgot.Count())
                                    {
                                        j++;
                                        stajTableList.Add(recNew);
                                    }

                                }
                            }
                        }
                        else // если льготного стажа нет, то делаем пустые поля
                        {
                            if (szv_staj.Dismissed.HasValue && szv_staj.Dismissed.Value && print3112)
                            {
                                recNew.Col14 = szv_staj.Dismissed.HasValue ? (szv_staj.Dismissed.Value ? (szv_staj.Year.ToString() + "-12-31") : "") : "";
                            }
                            else if (Staj_item.CodeBEZR.HasValue && Staj_item.CodeBEZR.Value)
                            {
                                recNew.Col14 = Staj_item.CodeBEZR.HasValue ? (Staj_item.CodeBEZR.Value ? "БЕЗР" : "") : "";
                            }
                            else
                            {
                                recNew.Col14 = "";
                            }
                        }


                        if (Staj_List.Count() > 1)
                        {
                            stajTableList.Add(recNew);
                            j++;
                        }
                        else if (Staj_List.Count() == 1)
                        {
                            stajTableList.Add(recNew);
                        }
                        js++;

                    }

                    if (Staj_List.Count == 0)
                    {
                        if (szv_staj.Dismissed.HasValue && szv_staj.Dismissed.Value)
                        {
                            recNew.Col14 = szv_staj.Dismissed.HasValue ? (szv_staj.Dismissed.Value ? (szv_staj.Year.ToString() + "-12-31") : "") : "";
                        }


                        if (SZV_STAJ_List.Count > 1)
                        {
                            stajTableList.Add(recNew);
                            js++;
                        }
                        else if (SZV_STAJ_List.Count == 1)
                        {

                            stajTableList.Add(recNew);
                        }

                        
                    }

                    cnt++;

                }

                Telerik.Reporting.Table table_Staj = SZVSTAJ.Items.Find("table12", true)[0] as Telerik.Reporting.Table;

                table_Staj.DataSource = stajTableList;



            }
            return SZVSTAJ;
        }

        private SZV_ISH_Rep SZVISH_Report(FormsSZV_ISH_2017 item)
        {
            SZV_ISH_Rep SZVISH = new SZV_ISH_Rep();

            Staff staff = db.Staff.Single(x => x.ID == item.StaffID);

            (SZVISH.Items.Find("RegNum", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
            (SZVISH.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN != null ? ins.INN.ToString() : "";
            (SZVISH.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP != null ? ins.KPP.ToString() : "";

            (SZVISH.Items.Find("RegNum2", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
            (SZVISH.Items.Find("INN2", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN != null ? ins.INN.ToString() : "";
            (SZVISH.Items.Find("KPP2", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP != null ? ins.KPP.ToString() : "";

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

            (SZVISH.Items.Find("nameShort", true)[0] as Telerik.Reporting.TextBox).Value = orgName;

            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(8.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;



            (SZVISH.Items.Find("Year", true)[0] as Telerik.Reporting.TextBox).Value = item.Year != null ? item.Year.ToString() : "";
            (SZVISH.Items.Find("Code", true)[0] as Telerik.Reporting.TextBox).Value = item.Code != null ? item.Code.ToString() : "";

            (SZVISH.Items.Find("LastName", true)[0] as Telerik.Reporting.TextBox).Value = staff.LastName;
            (SZVISH.Items.Find("FirstName", true)[0] as Telerik.Reporting.TextBox).Value = staff.FirstName;
            (SZVISH.Items.Find("MiddleName", true)[0] as Telerik.Reporting.TextBox).Value = staff.MiddleName;
            (SZVISH.Items.Find("SNILS", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber.ToString(), (staff.ControlNumber.HasValue ? staff.ControlNumber.Value : (short)0)) : "";

            (SZVISH.Items.Find("ContractNum", true)[0] as Telerik.Reporting.TextBox).Value = item.ContractNum;
            (SZVISH.Items.Find("ContractDate", true)[0] as Telerik.Reporting.TextBox).Value = item.ContractDate.HasValue ? item.ContractDate.Value.ToString("dd.MM.yyyy") : "";
            (SZVISH.Items.Find("DopTarCode", true)[0] as Telerik.Reporting.TextBox).Value = item.DopTarCode;

            (SZVISH.Items.Find("SumFeePFR_Insurer", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(item.SumFeePFR_Insurer.Value);
            (SZVISH.Items.Find("SumFeePFR_Staff", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(item.SumFeePFR_Staff.Value);
            (SZVISH.Items.Find("SumFeePFR_Tar", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(item.SumFeePFR_Tar.Value);
            (SZVISH.Items.Find("SumFeePFR_TarDop", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(item.SumFeePFR_TarDop.Value);
            (SZVISH.Items.Find("SumFeePFR_Strah", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(item.SumFeePFR_Strah.Value);
            (SZVISH.Items.Find("SumFeePFR_Nakop", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(item.SumFeePFR_Nakop.Value);
            (SZVISH.Items.Find("SumFeePFR_Base", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(item.SumFeePFR_Base.Value);
            (SZVISH.Items.Find("SumPayPFR_Strah", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(item.SumPayPFR_Strah.Value);
            (SZVISH.Items.Find("SumPayPFR_Nakop", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(item.SumPayPFR_Nakop.Value);

            if (ODV1_Data != null)
            {
                (SZVISH.Items.Find("ConfirmDolgn", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.ConfirmDolgn;
                (SZVISH.Items.Find("ConfirmFIO", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.ConfirmLastName + " " + ODV1_Data.ConfirmFirstName + " " + ODV1_Data.ConfirmMiddleName;
            }

            (SZVISH.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = item.DateFilling.ToString("dd.MM.yyyy");


            //Делаем стиль для таблички

            SZVISH.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

            Telerik.Reporting.Table table_4 = SZVISH.Items.Find("table4", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_4 = new TableGroup();

            var SZV_ISH_4_List = item.FormsSZV_ISH_4_2017.ToList();

            int j = 1;
            foreach (var szv4 in SZV_ISH_4_List)
            {

                table_4.Body.SetCellContent(j, 0, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = szv4.Month.HasValue ? (Monthes.Any(x => x.Code == szv4.Month.Value) ? Monthes.First(x => x.Code == szv4.Month.Value).Name : "") : ""
                });

                table_4.Body.SetCellContent(j, 1, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = szv4.PlatCategory.Code
                });

                table_4.Body.SetCellContent(j, 2, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumFeePFR)
                });

                table_4.Body.SetCellContent(j, 3, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.BaseALL)
                });
                table_4.Body.SetCellContent(j, 4, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.BaseGPD)
                });
                table_4.Body.SetCellContent(j, 5, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumPrevBaseALL)
                });
                table_4.Body.SetCellContent(j, 6, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumPrevBaseGPD)
                });


                table_4.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                detailGrouptable_4.ChildGroups.Add(new TableGroup());
                j++;
            }

            table_4.Body.SetCellContent(j, 0, new Telerik.Reporting.TextBox
            {
                StyleName = "TableStyle",
                Value = "Итого"
            });
            table_4.Body.SetCellContent(j, 1, new Telerik.Reporting.TextBox
            {
                StyleName = "TableStyle",
                Value = "  "
            });

            table_4.Body.SetCellContent(j, 2, new Telerik.Reporting.TextBox
            {
                StyleName = "TableStyle",
                Value = Utils.decToStr(SZV_ISH_4_List.Sum(y => y.SumFeePFR))
            });
            table_4.Body.SetCellContent(j, 3, new Telerik.Reporting.TextBox
            {
                StyleName = "TableStyle",
                Value = Utils.decToStr(SZV_ISH_4_List.Sum(y => y.BaseALL))
            });
            table_4.Body.SetCellContent(j, 4, new Telerik.Reporting.TextBox
            {
                StyleName = "TableStyle",
                Value = Utils.decToStr(SZV_ISH_4_List.Sum(y => y.BaseGPD))
            });
            table_4.Body.SetCellContent(j, 5, new Telerik.Reporting.TextBox
            {
                StyleName = "TableStyle",
                Value = Utils.decToStr(SZV_ISH_4_List.Sum(y => y.SumPrevBaseALL))
            });
            table_4.Body.SetCellContent(j, 6, new Telerik.Reporting.TextBox
            {
                StyleName = "TableStyle",
                Value = Utils.decToStr(SZV_ISH_4_List.Sum(y => y.SumPrevBaseGPD))
            });


            table_4.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
            detailGrouptable_4.ChildGroups.Add(new TableGroup());

            table_4.RowGroups.Add(detailGrouptable_4);


            var TerrUsl_list = db.TerrUsl.ToList();
            var OsobUslTruda_list = db.OsobUslTruda.ToList();
            var KodVred_2_list = db.KodVred_2.ToList();
            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
            var UslDosrNazn_list = db.UslDosrNazn.ToList();
            var PlatCategory_list = db.PlatCategory.ToList();
            var SpecOcenkaUslTruda_list = db.SpecOcenkaUslTruda.ToList();


            var SZV_ISH_7_List = item.FormsSZV_ISH_7_2017.ToList();


            Telerik.Reporting.Table table_7 = SZVISH.Items.Find("table11", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re67 = new TableGroup();

            j = 1;
            foreach (var szv7 in SZV_ISH_7_List)
            {
                SpecOcenkaUslTruda SpecOcen = SpecOcenkaUslTruda_list.FirstOrDefault(x => x.ID == szv7.SpecOcenkaUslTrudaID);

                Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = MonthesShort.Any(x => x.Code == szv7.Month.Value) ? MonthesShort.First(x => x.Code == szv7.Month.Value).Name : ""
                };
                table_7.Body.SetCellContent(j, 0, textBox);
                textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = SpecOcen == null ? "" : SpecOcen.Code.ToString()
                };
                table_7.Body.SetCellContent(j, 1, textBox);
                textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv7.s_1_0)
                };
                table_7.Body.SetCellContent(j, 2, textBox);
                textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv7.s_1_1)
                };
                table_7.Body.SetCellContent(j, 3, textBox);

                table_7.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                detailGrouptable_Re67.ChildGroups.Add(new TableGroup());

                j++;
            }

            if (SZV_ISH_7_List.Count() > 0)
                table_7.RowGroups.Add(detailGrouptable_Re67);



            //var SZV_ISH_7_List = item.FormsSZV_ISH_7_2017.ToList();

            //Telerik.Reporting.Table table_7 = SZVISH.Items.Find("table11", true)[0] as Telerik.Reporting.Table;
            //TableGroup detailGrouptable_Re67 = new TableGroup();

            //j = 1;
            //int k = 0;
            //foreach (var rsw67 in SZV_ISH_7_List)
            //{
            //    SpecOcenkaUslTruda SpecOcen = SpecOcenkaUslTruda_list.FirstOrDefault(x => x.ID == rsw67.SpecOcenkaUslTrudaID);
            //    Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = "всего за последние три месяца отчетного периода, в том числе:",
            //        TextWrap = true,
            //        Multiline = true,
            //        Height = Telerik.Reporting.Drawing.Unit.Cm(1D)
            //    };
            //    table_7.Body.SetCellContent(j, 0, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = SpecOcen == null ? "" : SpecOcen.Code == null ? "" : SpecOcen.Code.ToString()
            //    };
            //    table_7.Body.SetCellContent(j, 1, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = Utils.decToStr(rsw67.s_0_0)
            //    };
            //    table_7.Body.SetCellContent(j, 2, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = Utils.decToStr(rsw67.s_0_1)
            //    };
            //    table_7.Body.SetCellContent(j, 3, textBox);
            //    table_7.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(1.4D)));
            //    detailGrouptable_Re67.ChildGroups.Add(new TableGroup());
            //    j++;

            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = "1 месяц"
            //    };
            //    table_7.Body.SetCellContent(j, 0, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = SpecOcen == null ? "" : (SpecOcen.Code == null || ((rsw67.s_1_0 == null || (rsw67.s_1_0 != null && rsw67.s_1_0 == 0)) && (rsw67.s_1_1 == null || (rsw67.s_1_1 != null && rsw67.s_1_1 == 0)))) ? "" : SpecOcen.Code.ToString()
            //    };
            //    table_7.Body.SetCellContent(j, 1, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = Utils.decToStr(rsw67.s_1_0)
            //    };
            //    table_7.Body.SetCellContent(j, 2, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = Utils.decToStr(rsw67.s_1_1)
            //    };
            //    table_7.Body.SetCellContent(j, 3, textBox);
            //    table_7.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
            //    detailGrouptable_Re67.ChildGroups.Add(new TableGroup());
            //    j++;

            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = "2 месяц"
            //    };
            //    table_7.Body.SetCellContent(j, 0, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = SpecOcen == null ? "" : (SpecOcen.Code == null || ((rsw67.s_2_0 == null || (rsw67.s_2_0 != null && rsw67.s_2_0 == 0)) && (rsw67.s_2_1 == null || (rsw67.s_2_1 != null && rsw67.s_2_1 == 0)))) ? "" : SpecOcen.Code.ToString()
            //    };
            //    table_7.Body.SetCellContent(j, 1, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = Utils.decToStr(rsw67.s_2_0)
            //    };
            //    table_7.Body.SetCellContent(j, 2, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = Utils.decToStr(rsw67.s_2_1)
            //    };
            //    table_7.Body.SetCellContent(j, 3, textBox);
            //    table_7.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
            //    detailGrouptable_Re67.ChildGroups.Add(new TableGroup());
            //    j++;

            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = "3 месяц"
            //    };
            //    table_7.Body.SetCellContent(j, 0, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = SpecOcen == null ? "" : (SpecOcen.Code == null || ((rsw67.s_3_0 == null || (rsw67.s_3_0 != null && rsw67.s_3_0 == 0)) && (rsw67.s_3_1 == null || (rsw67.s_3_1 != null && rsw67.s_3_1 == 0)))) ? "" : SpecOcen.Code.ToString()
            //    };
            //    table_7.Body.SetCellContent(j, 1, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = Utils.decToStr(rsw67.s_3_0)
            //    };
            //    table_7.Body.SetCellContent(j, 2, textBox);
            //    textBox = new Telerik.Reporting.TextBox
            //    {
            //        StyleName = "TableStyle",
            //        Value = Utils.decToStr(rsw67.s_3_1)
            //    };
            //    table_7.Body.SetCellContent(j, 3, textBox);
            //    table_7.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
            //    detailGrouptable_Re67.ChildGroups.Add(new TableGroup());
            //    j++;
            //    k++;
            //}

            //if (SZV_ISH_7_List.Count() > 0)
            //    table_7.RowGroups.Add(detailGrouptable_Re67);


            //раздел 6.8
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            var szv_8_List = db.StajOsn.Where(x => x.FormsSZV_ISH_2017_ID == item.ID).ToList();

            Telerik.Reporting.Table table_Re68 = SZVISH.Items.Find("table12", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re68 = new TableGroup();

            j = 1;

            foreach (var rsw68 in szv_8_List.OrderBy(x => x.Number.Value))
            {
                Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = rsw68.Number.ToString()
                };
                table_Re68.Body.SetCellContent(j, 0, textBox);
                textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Convert.ToDateTime(rsw68.DateBegin).ToString("dd/MM/yyyy")
                };
                table_Re68.Body.SetCellContent(j, 1, textBox);
                textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Convert.ToDateTime(rsw68.DateEnd).ToString("dd/MM/yyyy")
                };
                table_Re68.Body.SetCellContent(j, 2, textBox);

                if (rsw68.StajLgot.Count() > 0)
                {
                    int ii = 0;
                    foreach (var rsw68_dop in rsw68.StajLgot.OrderBy(x => x.Number.Value))
                    {
                        if (ii > 0) // если несколько записей о льготном стаже
                        {

                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_Re68.Body.SetCellContent(j, 0, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_Re68.Body.SetCellContent(j, 1, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_Re68.Body.SetCellContent(j, 2, textBox);

                        }

                        if (rsw68_dop.TerrUslID != null && rsw68_dop.TerrUslID.Value.ToString() != "" && TerrUsl_list.Any(x => x.ID == rsw68_dop.TerrUslID))
                        {
                            TerrUsl tu = TerrUsl_list.FirstOrDefault(x => x.ID == rsw68_dop.TerrUslID);
                            string koef = "";
                            if (rsw68_dop.TerrUslKoef.HasValue && rsw68_dop.TerrUslKoef.Value != 0)
                            {
                                koef = Utils.decToStr(rsw68_dop.TerrUslKoef.Value);
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
                        table_Re68.Body.SetCellContent(j, 3, textBox);

                        if (rsw68_dop.OsobUslTrudaID != null && rsw68_dop.OsobUslTrudaID.Value.ToString() != "" && OsobUslTruda_list.Any(x => x.ID == rsw68_dop.OsobUslTrudaID))
                        {
                            OsobUslTruda ou = OsobUslTruda_list.FirstOrDefault(x => x.ID == rsw68_dop.OsobUslTrudaID);
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
                        table_Re68.Body.SetCellContent(j, 4, textBox);



                        IschislStrahStajOsn isso = IschislStrahStajOsn_list.FirstOrDefault(x => x.ID == rsw68_dop.IschislStrahStajOsnID);

                        string str = rsw68_dop.IschislStrahStajDopID == null ? "  " : rsw68_dop.IschislStrahStajDopID.HasValue ? IschislStrahStajDop_list.FirstOrDefault(x => x.ID == rsw68_dop.IschislStrahStajDopID).Code : "  ";
                        string s1 = rsw68_dop.Strah1Param.HasValue ? rsw68_dop.Strah1Param.Value.ToString() : "0";
                        string s2 = rsw68_dop.Strah2Param.HasValue ? rsw68_dop.Strah2Param.Value.ToString() : "0";

                        str = ((!String.IsNullOrEmpty(str.Trim()) || (s1 != "0") || (s2 != "0"))) ? "[" + s1 + "][" + s2 + "][" + str + "]" : "";

                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = isso == null ? "" : isso.Code == null ? "" : isso.Code.ToString()
                        };
                        table_Re68.Body.SetCellContent(j, 5, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = str
                        };


                        table_Re68.Body.SetCellContent(j, 6, textBox);

                        if (rsw68_dop.UslDosrNaznID != null)
                        {
                            UslDosrNazn udn = UslDosrNazn_list.FirstOrDefault(x => x.ID == rsw68_dop.UslDosrNaznID);

                            s1 = rsw68_dop.UslDosrNazn1Param.HasValue == true ? rsw68_dop.UslDosrNazn1Param.Value.ToString() : "0";
                            s2 = rsw68_dop.UslDosrNazn2Param.HasValue == true ? rsw68_dop.UslDosrNazn2Param.Value.ToString() : "0";
                            string s3 = rsw68_dop.UslDosrNazn3Param.HasValue == true ? Utils.decToStr(rsw68_dop.UslDosrNazn3Param.Value) : "0";

                            str = "[" + s1 + "][" + s2 + "][" + s3 + "]";

                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = udn == null ? "" : udn.Code == null ? "" : udn.Code.ToString()
                            };
                            table_Re68.Body.SetCellContent(j, 7, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = str
                            };
                            table_Re68.Body.SetCellContent(j, 8, textBox);
                        }
                        else
                        {
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_Re68.Body.SetCellContent(j, 7, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_Re68.Body.SetCellContent(j, 8, textBox);
                        }

                        ii++;

                        if (rsw68.StajLgot.Count() > 1 && ii != rsw68.StajLgot.Count())
                        {
                            j++;
                            table_Re68.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                            detailGrouptable_Re68.ChildGroups.Add(new TableGroup());
                        }

                        if (rsw68_dop.KodVred_OsnID != null && rsw68_dop.KodVred_OsnID.Value.ToString() != "" && KodVred_2_list.Any(x => x.ID == rsw68_dop.KodVred_OsnID))
                        {
                            if ((rsw68.StajLgot.Count()) == 1 || (rsw68.StajLgot.Count() > 1 && ii == rsw68.StajLgot.Count()))
                            {
                                j++;
                                table_Re68.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                detailGrouptable_Re68.ChildGroups.Add(new TableGroup());
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
                                    table_Re68.Body.SetCellContent(j, m, textBox);
                                }
                            }

                            KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.ID == rsw68_dop.KodVred_OsnID);

                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = kv.Code == null ? "" : kv.Code.ToString()
                            };
                            table_Re68.Body.SetCellContent(j, 4, textBox, 1, 2);
                            if (rsw68.StajLgot.Count() > 1 && ii != rsw68.StajLgot.Count())
                            {
                                j++;
                                table_Re68.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                detailGrouptable_Re68.ChildGroups.Add(new TableGroup());
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
                        table_Re68.Body.SetCellContent(j, m, textBox);
                    }
                }

                if (szv_8_List.Count() > 1)
                {
                    table_Re68.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                    detailGrouptable_Re68.ChildGroups.Add(new TableGroup());
                    j++;
                }
                else if (szv_8_List.Count() == 1)
                {
                    // && rsw68.StajLgot.Count <= 1
                    table_Re68.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                    detailGrouptable_Re68.ChildGroups.Add(new TableGroup());
                }


            }
            if (szv_8_List.Count() > 0)
                table_Re68.RowGroups.Add(detailGrouptable_Re68);

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();


            return SZVISH;
        }

        private SZV_KORR_Rep SZVKORR_Report(FormsSZV_KORR_2017 item)
        {
            SZV_KORR_Rep SZVKORR = new SZV_KORR_Rep();

            Staff staff = db.Staff.Single(x => x.ID == item.StaffID);

            (SZVKORR.Items.Find("RegNum", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
            (SZVKORR.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN != null ? ins.INN.ToString() : "";
            (SZVKORR.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP != null ? ins.KPP.ToString() : "";

            (SZVKORR.Items.Find("RegNum2", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
            (SZVKORR.Items.Find("INN2", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN != null ? ins.INN.ToString() : "";
            (SZVKORR.Items.Find("KPP2", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP != null ? ins.KPP.ToString() : "";

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

            (SZVKORR.Items.Find("nameShort", true)[0] as Telerik.Reporting.TextBox).Value = orgName;

            string regNumKorr = Utils.ParseRegNum(item.RegNumKorr);
            (SZVKORR.Items.Find("RegNumKorr", true)[0] as Telerik.Reporting.TextBox).Value = regNumKorr;
            (SZVKORR.Items.Find("INNKorr", true)[0] as Telerik.Reporting.TextBox).Value = item.INNKorr;
            (SZVKORR.Items.Find("KPPKorr", true)[0] as Telerik.Reporting.TextBox).Value = item.KPPKorr;

            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(8.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            Telerik.Reporting.Drawing.StyleRule myStyleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle2") });
            myStyleRule2.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule2.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule2.Style.Font.Name = "Arial";
            myStyleRule2.Style.Font.Size = new Unit(6.0, UnitType.Point);
            myStyleRule2.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule2.Style.VerticalAlign = VerticalAlign.Middle;


            (SZVKORR.Items.Find("Year", true)[0] as Telerik.Reporting.TextBox).Value = item.Year != null ? item.Year.ToString() : "";
            (SZVKORR.Items.Find("Code", true)[0] as Telerik.Reporting.TextBox).Value = item.Code != null ? item.Code.ToString() : "";

            (SZVKORR.Items.Find("YearKorr", true)[0] as Telerik.Reporting.TextBox).Value = item.YearKorr != null ? item.YearKorr.ToString() : "";
            (SZVKORR.Items.Find("CodeKorr", true)[0] as Telerik.Reporting.TextBox).Value = item.CodeKorr != null ? item.CodeKorr.ToString() : "";

            string tInfo = "";

            if (item.TypeInfo.HasValue)
            {
                switch (item.TypeInfo.Value)
                {
                    case 0: tInfo = "Корректирующая";
                        break;
                    case 1: tInfo = "Отменяющая";
                        break;
                    case 2: tInfo = "Особая";
                        break;
                }
            }

            (SZVKORR.Items.Find("TypeInfo", true)[0] as Telerik.Reporting.TextBox).Value = tInfo;

            (SZVKORR.Items.Find("LastName", true)[0] as Telerik.Reporting.TextBox).Value = staff.LastName;
            (SZVKORR.Items.Find("FirstName", true)[0] as Telerik.Reporting.TextBox).Value = staff.FirstName;
            (SZVKORR.Items.Find("MiddleName", true)[0] as Telerik.Reporting.TextBox).Value = staff.MiddleName;
            (SZVKORR.Items.Find("SNILS", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber.ToString(), (staff.ControlNumber.HasValue ? staff.ControlNumber.Value : (short)0)) : "";

            (SZVKORR.Items.Find("PlatCategory", true)[0] as Telerik.Reporting.TextBox).Value = item.PlatCategoryID.HasValue ? item.PlatCategory.Code : "";
            (SZVKORR.Items.Find("ContractType", true)[0] as Telerik.Reporting.TextBox).Value = (item.ContractType.HasValue && item.ContractType.Value != 0) ? (item.ContractType.Value == 1 ? "Трудовой" : "Гражданско-правовой") : "";
            (SZVKORR.Items.Find("ContractNum", true)[0] as Telerik.Reporting.TextBox).Value = item.ContractNum;
            (SZVKORR.Items.Find("ContractDate", true)[0] as Telerik.Reporting.TextBox).Value = item.ContractDate.HasValue ? item.ContractDate.Value.ToString("dd.MM.yyyy") : "";
            (SZVKORR.Items.Find("DopTarCode", true)[0] as Telerik.Reporting.TextBox).Value = item.DopTarCode;


            if (ODV1_Data != null)
            {
                (SZVKORR.Items.Find("ConfirmDolgn", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.ConfirmDolgn;
                (SZVKORR.Items.Find("ConfirmFIO", true)[0] as Telerik.Reporting.TextBox).Value = ODV1_Data.ConfirmLastName + " " + ODV1_Data.ConfirmFirstName + " " + ODV1_Data.ConfirmMiddleName;
            }

            (SZVKORR.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = item.DateFilling.ToString("dd.MM.yyyy");


            //Делаем стиль для таблички

            SZVKORR.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule, myStyleRule2 });

            Telerik.Reporting.Table table_4 = SZVKORR.Items.Find("table4", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_4 = new TableGroup();

            var SZV_KORR_4_List = item.FormsSZV_KORR_4_2017.ToList();

            int j = 1;
            foreach (var szv4 in SZV_KORR_4_List)
            {

                table_4.Body.SetCellContent(j, 0, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = szv4.Month.HasValue ? (MonthesShort.Any(x => x.Code == szv4.Month.Value) ? MonthesShort.First(x => x.Code == szv4.Month.Value).Name : "") : ""
                });

                table_4.Body.SetCellContent(j, 1, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumFeePFR)
                });

                table_4.Body.SetCellContent(j, 2, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.BaseALL)
                });
                table_4.Body.SetCellContent(j, 3, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.BaseGPD)
                });
                table_4.Body.SetCellContent(j, 4, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumPrevBaseALL)
                });
                table_4.Body.SetCellContent(j, 5, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumPrevBaseGPD)
                });
                table_4.Body.SetCellContent(j, 6, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumFeeBefore2001Insurer)
                });
                table_4.Body.SetCellContent(j, 7, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumFeeBefore2001Staff)
                });
                table_4.Body.SetCellContent(j, 8, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumFeeAfter2001STRAH)
                });
                table_4.Body.SetCellContent(j, 9, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumFeeAfter2001NAKOP)
                });
                table_4.Body.SetCellContent(j, 10, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumFeeTarSV)
                });
                table_4.Body.SetCellContent(j, 11, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumPaySTRAH)
                });
                table_4.Body.SetCellContent(j, 12, new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv4.SumPayNAKOP)
                });


                table_4.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                detailGrouptable_4.ChildGroups.Add(new TableGroup());
                j++;
            }

            if (SZV_KORR_4_List.Count > 0)
            {
                table_4.RowGroups.Add(detailGrouptable_4);
            }


            var TerrUsl_list = db.TerrUsl.ToList();
            var OsobUslTruda_list = db.OsobUslTruda.ToList();
            var KodVred_2_list = db.KodVred_2.ToList();
            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
            var UslDosrNazn_list = db.UslDosrNazn.ToList();
            var PlatCategory_list = db.PlatCategory.ToList();
            var SpecOcenkaUslTruda_list = db.SpecOcenkaUslTruda.ToList();



            var SZV_KORR_5_List = item.FormsSZV_KORR_5_2017.ToList();


            Telerik.Reporting.Table table_5 = SZVKORR.Items.Find("table11", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re67 = new TableGroup();

            j = 1;
            foreach (var szv5 in SZV_KORR_5_List)
            {
                SpecOcenkaUslTruda SpecOcen = SpecOcenkaUslTruda_list.FirstOrDefault(x => x.ID == szv5.SpecOcenkaUslTrudaID);

                Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = MonthesShort.Any(x => x.Code == szv5.Month.Value) ? MonthesShort.First(x => x.Code == szv5.Month.Value).Name : ""
                };
                table_5.Body.SetCellContent(j, 0, textBox);
                textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = SpecOcen == null ? "" : SpecOcen.Code.ToString()
                };
                table_5.Body.SetCellContent(j, 1, textBox);
                textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv5.s_0)
                };
                table_5.Body.SetCellContent(j, 2, textBox);
                textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Utils.decToStr(szv5.s_1)
                };
                table_5.Body.SetCellContent(j, 3, textBox);

                table_5.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                detailGrouptable_Re67.ChildGroups.Add(new TableGroup());

                j++;
            }

            if (SZV_KORR_5_List.Count() > 0)
                table_5.RowGroups.Add(detailGrouptable_Re67);


            //раздел 6
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();

            var szv_6_List = db.StajOsn.Where(x => x.FormsSZV_KORR_2017_ID == item.ID).ToList();

            Telerik.Reporting.Table table_Re6 = SZVKORR.Items.Find("table12", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re6 = new TableGroup();

            j = 1;

            foreach (var szv6 in szv_6_List.OrderBy(x => x.Number.Value))
            {
                Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Convert.ToDateTime(szv6.DateBegin).ToString("dd/MM/yyyy")
                };
                table_Re6.Body.SetCellContent(j, 0, textBox);
                textBox = new Telerik.Reporting.TextBox
                {
                    StyleName = "TableStyle",
                    Value = Convert.ToDateTime(szv6.DateEnd).ToString("dd/MM/yyyy")
                };
                table_Re6.Body.SetCellContent(j, 1, textBox);

                if (szv6.StajLgot.Count() > 0)
                {
                    int ii = 0;
                    foreach (var szv6_dop in szv6.StajLgot.OrderBy(x => x.Number.Value))
                    {
                        if (ii > 0) // если несколько записей о льготном стаже
                        {

                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_Re6.Body.SetCellContent(j, 0, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_Re6.Body.SetCellContent(j, 1, textBox);

                        }

                        if (szv6_dop.TerrUslID != null && szv6_dop.TerrUslID.Value.ToString() != "" && TerrUsl_list.Any(x => x.ID == szv6_dop.TerrUslID))
                        {
                            TerrUsl tu = TerrUsl_list.FirstOrDefault(x => x.ID == szv6_dop.TerrUslID);
                            string koef = "";
                            if (szv6_dop.TerrUslKoef.HasValue && szv6_dop.TerrUslKoef.Value != 0)
                            {
                                koef = Utils.decToStr(szv6_dop.TerrUslKoef.Value);
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
                        table_Re6.Body.SetCellContent(j, 2, textBox);

                        if (szv6_dop.OsobUslTrudaID != null && szv6_dop.OsobUslTrudaID.Value.ToString() != "" && OsobUslTruda_list.Any(x => x.ID == szv6_dop.OsobUslTrudaID))
                        {
                            OsobUslTruda ou = OsobUslTruda_list.FirstOrDefault(x => x.ID == szv6_dop.OsobUslTrudaID);
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
                        table_Re6.Body.SetCellContent(j, 3, textBox);



                        IschislStrahStajOsn isso = IschislStrahStajOsn_list.FirstOrDefault(x => x.ID == szv6_dop.IschislStrahStajOsnID);

                        string str = szv6_dop.IschislStrahStajDopID == null ? "  " : szv6_dop.IschislStrahStajDopID.HasValue ? IschislStrahStajDop_list.FirstOrDefault(x => x.ID == szv6_dop.IschislStrahStajDopID).Code : "  ";
                        string s1 = szv6_dop.Strah1Param.HasValue ? szv6_dop.Strah1Param.Value.ToString() : "0";
                        string s2 = szv6_dop.Strah2Param.HasValue ? szv6_dop.Strah2Param.Value.ToString() : "0";

                        str = ((!String.IsNullOrEmpty(str.Trim()) || (s1 != "0") || (s2 != "0"))) ? "[" + s1 + "][" + s2 + "][" + str + "]" : "";

                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = isso == null ? "" : isso.Code == null ? "" : isso.Code.ToString()
                        };
                        table_Re6.Body.SetCellContent(j, 4, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = str
                        };


                        table_Re6.Body.SetCellContent(j, 5, textBox);

                        if (szv6_dop.UslDosrNaznID != null)
                        {
                            UslDosrNazn udn = UslDosrNazn_list.FirstOrDefault(x => x.ID == szv6_dop.UslDosrNaznID);

                            s1 = szv6_dop.UslDosrNazn1Param.HasValue == true ? szv6_dop.UslDosrNazn1Param.Value.ToString() : "0";
                            s2 = szv6_dop.UslDosrNazn2Param.HasValue == true ? szv6_dop.UslDosrNazn2Param.Value.ToString() : "0";
                            string s3 = szv6_dop.UslDosrNazn3Param.HasValue == true ? Utils.decToStr(szv6_dop.UslDosrNazn3Param.Value) : "0";

                            str = "[" + s1 + "][" + s2 + "][" + s3 + "]";

                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = udn == null ? "" : udn.Code == null ? "" : udn.Code.ToString()
                            };
                            table_Re6.Body.SetCellContent(j, 6, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = str
                            };
                            table_Re6.Body.SetCellContent(j, 7, textBox);
                        }
                        else
                        {
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_Re6.Body.SetCellContent(j, 6, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = ""
                            };
                            table_Re6.Body.SetCellContent(j, 7, textBox);
                        }

                        ii++;

                        if (szv6.StajLgot.Count() > 1 && ii != szv6.StajLgot.Count())
                        {
                            j++;
                            table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                            detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
                        }

                        if (szv6_dop.KodVred_OsnID != null && szv6_dop.KodVred_OsnID.Value.ToString() != "" && KodVred_2_list.Any(x => x.ID == szv6_dop.KodVred_OsnID))
                        {
                            if ((szv6.StajLgot.Count()) == 1 || (szv6.StajLgot.Count() > 1 && ii == szv6.StajLgot.Count()))
                            {
                                j++;
                                table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
                            }


                            for (int m = 0; m <= 7; m++)
                            {
                                if (m != 3 && m != 4)
                                {
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_Re6.Body.SetCellContent(j, m, textBox);
                                }
                            }

                            KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.ID == szv6_dop.KodVred_OsnID);

                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = kv.Code == null ? "" : kv.Code.ToString()
                            };
                            table_Re6.Body.SetCellContent(j, 3, textBox, 1, 2);
                            if (szv6.StajLgot.Count() > 1 && ii != szv6.StajLgot.Count())
                            {
                                j++;
                                table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
                            }

                        }



                    }
                }
                else // если льготного стажа нет, то делаем пустые поля
                {

                    for (int m = 2; m <= 7; m++)
                    {
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = ""
                        };
                        table_Re6.Body.SetCellContent(j, m, textBox);
                    }
                }

                if (szv_6_List.Count() > 1)
                {
                    table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                    detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
                    j++;
                }
                else if (szv_6_List.Count() == 1)
                {
                    // && rsw68.StajLgot.Count <= 1
                    table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                    detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
                }


            }
            if (szv_6_List.Count() > 0)
                table_Re6.RowGroups.Add(detailGrouptable_Re6);

            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();


            return SZVKORR;
        }


    }
}
