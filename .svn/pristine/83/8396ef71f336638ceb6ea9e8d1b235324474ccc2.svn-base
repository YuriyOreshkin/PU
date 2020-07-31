using System;
using System.Collections.Generic;
using System.Linq;
using PU.Classes;
using PU.Models;
using Telerik.WinControls;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;
using System.Drawing;
using System.ComponentModel;
using System.Globalization;


namespace PU.FormsRSW2014
{
    public partial class RSW2014_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        private string regNum = "";
        public byte wp = 0;
        public short yearType { get; set; }
        public FormsRSW2014_1_1 RSWdata;
        public FormsSZV_6_4 SZV_6_4_data;
        public FormsSZV_6 SZV_6_data;
        public bool fromXMLflag = false;
        string DateUnderwrite;

        public List<FormsRSW2014_1_Razd_2_1> RSW_2_1_List = new List<FormsRSW2014_1_Razd_2_1>();
        public List<FormsRSW2014_1_Razd_2_4> RSW_2_4_List = new List<FormsRSW2014_1_Razd_2_4>();
        public List<FormsRSW2014_1_Razd_2_5_1> RSW_2_5_1_List = new List<FormsRSW2014_1_Razd_2_5_1>();
        public List<FormsRSW2014_1_Razd_2_5_2> RSW_2_5_2_List = new List<FormsRSW2014_1_Razd_2_5_2>();
        public List<FormsRSW2014_1_Razd_3_4> RSW_3_4_List = new List<FormsRSW2014_1_Razd_3_4>();
        public List<FormsRSW2014_1_Razd_4> RSW_4_List = new List<FormsRSW2014_1_Razd_4>();
        public List<FormsRSW2014_1_Razd_5> RSW_5_List = new List<FormsRSW2014_1_Razd_5>();
        public List<FormsRSW2014_1_Razd_6_1> RSW_6_1_List = new List<FormsRSW2014_1_Razd_6_1>();
        public List<FormsRSW2014_1_Razd_6_4> RSW_6_4_List = new List<FormsRSW2014_1_Razd_6_4>();
        public List<FormsRSW2014_1_Razd_6_6> RSW_6_6_List = new List<FormsRSW2014_1_Razd_6_6>();
        public List<FormsRSW2014_1_Razd_6_7> RSW_6_7_List = new List<FormsRSW2014_1_Razd_6_7>();
        public List<StajOsn> RSW_6_8_List = new List<StajOsn>();


        public List<FormsSZV_6_4> SZV_6_4_List = new List<FormsSZV_6_4>();
        public List<StajOsn> SZV_6_4_Staj = new List<StajOsn>();

        public List<FormsSZV_6> SZV_6_1_List = new List<FormsSZV_6>();
        public List<StajOsn> SZV_6_1_Staj = new List<StajOsn>();

        public List<List<FormsSZV_6>> SZV_6_2_List = new List<List<FormsSZV_6>>();


        public RSW2014_Print()
        {
            InitializeComponent();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);
            ReportBook RSW2014Book = new ReportBook();
            int pageCnt = 0;

            regNum = Utils.ParseRegNum(ins.RegNum);

            int j = 0;

            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            //Делаем стиль для табличек
            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            //Делаем стиль для табличек
            Telerik.Reporting.Drawing.StyleRule myStyleRuleSmaller = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRuleSmaller.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyleSmaller") });
            myStyleRuleSmaller.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRuleSmaller.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRuleSmaller.Style.Font.Name = "Arial";
            myStyleRuleSmaller.Style.Font.Size = new Unit(5.5, UnitType.Point);
            myStyleRuleSmaller.Style.TextAlign = HorizontalAlign.Center;
            myStyleRuleSmaller.Style.VerticalAlign = VerticalAlign.Middle;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            //Титульный лист
            if ((wp == 0 || wp == 2) && RSWdata != null)
            {
                DateUnderwrite = RSWdata.DateUnderwrite.HasValue ? RSWdata.DateUnderwrite.Value.ToShortDateString() : DateTime.Now.ToShortDateString();

                #region  Титульный лист

                RSW2014_ReTitle RSW2014_Title = new RSW2014_ReTitle();
                Telerik.Reporting.TextBox RegNum_title = RSW2014_Title.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                RegNum_title.Value = regNum;
                Telerik.Reporting.TextBox CorrectionNum = RSW2014_Title.Items.Find("textBox4", true)[0] as Telerik.Reporting.TextBox;
                CorrectionNum.Value = RSWdata.CorrectionNum.ToString().PadLeft(3, '0');
                Telerik.Reporting.TextBox QuarterTitle = RSW2014_Title.Items.Find("textBox5", true)[0] as Telerik.Reporting.TextBox;
                QuarterTitle.Value = RSWdata.Quarter.ToString();
                Telerik.Reporting.TextBox YearTitle = RSW2014_Title.Items.Find("textBox6", true)[0] as Telerik.Reporting.TextBox;
                YearTitle.Value = RSWdata.Year.ToString();
                Telerik.Reporting.TextBox CorrectionType = RSW2014_Title.Items.Find("textBox16", true)[0] as Telerik.Reporting.TextBox;
                CorrectionType.Value = RSWdata.CorrectionType.ToString();
                Telerik.Reporting.TextBox WorkStop = RSW2014_Title.Items.Find("textBox18", true)[0] as Telerik.Reporting.TextBox;
                WorkStop.Value = RSWdata.WorkStop.ToString();


                if (yearType == 2015)
                {
                    //if (RSWdata.Quarter >= 6)
                    //{
                    //          Telerik.Reporting.TextBox ReducedRate = RSW2014_Title.Items.Find("textBox20", true)[0] as Telerik.Reporting.TextBox;
                    //          ReducedRate.Value = RSWdata.ReducedRate != null ? RSWdata.ReducedRate.ToString() : "";

                    Telerik.Reporting.TextBox textBox9 = RSW2014_Title.Items.Find("textBox9", true)[0] as Telerik.Reporting.TextBox;
                    textBox9.Value = "Приложение\r\n" +
                                     "к Изменениям,\r\n" +
                                     "которые вносятся в постановление\r\n" +
                                     "Правления ПФР от 16.01.2014 № 2п,\r\n" +
                                     "утвержденным постановлением\r\n" +
                                     "Правления ПФР\r\n" +
                                     "от 04.06.2015г.\r\n" +
                                     "№ 194п";

                    RSW2014_Title.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(4);

                    Telerik.Reporting.TextBox textBox13 = RSW2014_Title.Items.Find("textBox13", true)[0] as Telerik.Reporting.TextBox;
                    textBox13.Value = "Номер уточнения";
                    Telerik.Reporting.TextBox textBox14 = RSW2014_Title.Items.Find("textBox14", true)[0] as Telerik.Reporting.TextBox;
                    textBox14.Value = "(000 - исходная форма, уточнение 001 и т.д.)";
                    Telerik.Reporting.TextBox textBox1 = RSW2014_Title.Items.Find("textBox1", true)[0] as Telerik.Reporting.TextBox;
                    textBox1.Value = "Причина уточнения";
                    Telerik.Reporting.TextBox textBox82 = RSW2014_Title.Items.Find("textBox82", true)[0] as Telerik.Reporting.TextBox;
                    textBox82.Value = "       *  Указывается дата представления расчета лично или через представителя, при отправке по почте - дата отправки почтового отправления с описью вложения.";


                    //}
                }
                else if (yearType == 2014 || yearType == 2012)
                {
                    Telerik.Reporting.TextBox textBox9 = RSW2014_Title.Items.Find("textBox9", true)[0] as Telerik.Reporting.TextBox;
                    textBox9.Value = "Приложение № 1\r\n" +
                                     "к Постановлению Правлению ПФР\r\n" +
                                     "от 16.01.2014 № 2п";

                }



                if (ins.TypePayer == 0)
                {
                    Telerik.Reporting.TextBox Name = RSW2014_Title.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox;
                    Name.Value = ins.Name != null ? ins.Name.ToString() : (ins.NameShort != null ? ins.NameShort : "");
                    Telerik.Reporting.TextBox KPP = RSW2014_Title.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                    KPP.Value = ins.KPP != null ? ins.KPP : "";
                }
                else
                {
                    Telerik.Reporting.TextBox Name = RSW2014_Title.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox;
                    Name.Value = (ins.LastName != null ? ins.LastName.ToString() : "") + " " + (ins.FirstName != null ? ins.FirstName.ToString() : "") + " " + (ins.MiddleName != null ? ins.MiddleName.ToString() : "");
                    Telerik.Reporting.TextBox KPP = RSW2014_Title.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                    KPP.Value = "";
                }

                Telerik.Reporting.TextBox INN = RSW2014_Title.Items.Find("textBox25", true)[0] as Telerik.Reporting.TextBox;
                INN.Value = ins.INN != null ? ins.INN.ToString() : "";
                Telerik.Reporting.TextBox OKWED = RSW2014_Title.Items.Find("textBox27", true)[0] as Telerik.Reporting.TextBox;
                OKWED.Value = ins.OKWED != null ? ins.OKWED.ToString() : "";
                Telerik.Reporting.TextBox PhoneContact = RSW2014_Title.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
                PhoneContact.Value = ins.PhoneContact != null ? ins.PhoneContact.ToString() : "";
                Telerik.Reporting.TextBox CountEmployers = RSW2014_Title.Items.Find("textBox40", true)[0] as Telerik.Reporting.TextBox;
                CountEmployers.Value = RSWdata.CountEmployers != null ? RSWdata.CountEmployers.ToString() : "";
                Telerik.Reporting.TextBox CountAverageEmployers = RSW2014_Title.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox;
                CountAverageEmployers.Value = RSWdata.CountAverageEmployers != null ? RSWdata.CountAverageEmployers.ToString() : "";
                Telerik.Reporting.TextBox CountConfirmDoc = RSW2014_Title.Items.Find("textBox50", true)[0] as Telerik.Reporting.TextBox;
                CountConfirmDoc.Value = RSWdata.CountConfirmDoc != null ? RSWdata.CountConfirmDoc.ToString() : "";
                Telerik.Reporting.TextBox ConfirmType = RSW2014_Title.Items.Find("textBox54", true)[0] as Telerik.Reporting.TextBox;
                ConfirmType.Value = RSWdata.ConfirmType.ToString();
                Telerik.Reporting.TextBox ConfirmName = RSW2014_Title.Items.Find("textBox55", true)[0] as Telerik.Reporting.TextBox;
                ConfirmName.Value = RSWdata.ConfirmLastName + " " + RSWdata.ConfirmFirstName + " " + RSWdata.ConfirmMiddleName;
                Telerik.Reporting.TextBox ConfirmOrg = RSW2014_Title.Items.Find("textBox56", true)[0] as Telerik.Reporting.TextBox;
                ConfirmOrg.Value = RSWdata.ConfirmOrgName != null ? RSWdata.ConfirmOrgName : "";
                Telerik.Reporting.TextBox ConfirmDoc = RSW2014_Title.Items.Find("textBox57", true)[0] as Telerik.Reporting.TextBox;

                string docName = "";
                if (RSWdata.ConfirmDocType_ID != null)
                {
                    if (db.DocumentTypes.FirstOrDefault(x => x.ID == RSWdata.ConfirmDocType_ID).Code == "ПРОЧЕЕ")
                        docName = RSWdata.ConfirmDocName;
                    else
                        docName = db.DocumentTypes.FirstOrDefault(x => x.ID == RSWdata.ConfirmDocType_ID).Code;
                    docName = docName + " " + RSWdata.ConfirmDocSerLat + " " + RSWdata.ConfirmDocSerRus + " № " + RSWdata.ConfirmDocNum;

                    if (RSWdata.ConfirmDocDate.HasValue || !String.IsNullOrEmpty(RSWdata.ConfirmDocKemVyd))
                    {

                        docName = docName + " Выдан: ";

                        if (RSWdata.ConfirmDocDate.HasValue)
                            docName = docName + RSWdata.ConfirmDocDate.Value.ToShortDateString();

                        docName = docName + "  " + RSWdata.ConfirmDocKemVyd;
                    }
                }

                ConfirmDoc.Value = docName;

                (RSW2014_Title.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;


                pageCnt++;
                RSW2014Book.Reports.Add(RSW2014_Title);



                //Раздел 1
                #region Раздел 1



                //                if (yearType == 2014 || yearType == 2012)
                //                {

                RSW2014_Re1 RSW2014_1 = new RSW2014_Re1();

                RSW2014_1 = new RSW2014_Re1();

                (RSW2014_1.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                if (yearType == 2015)
                {
                    Telerik.Reporting.TextBox textBox31 = RSW2014_1.Items.Find("textBox31", true)[0] as Telerik.Reporting.TextBox;
                    textBox31.Value = "на финансирование страховой пенсии";
                    Telerik.Reporting.TextBox textBox32 = RSW2014_1.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox;
                    textBox32.Value = "на финансирование накопительной пенсии";

                    Telerik.Reporting.TextBox textBox33 = RSW2014_1.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
                    textBox33.Value = "занятых на видах работ, указанных в пункте 1 части 1 статьи 30 Федерального закона от 28 декабря 2013 года  № 400-ФЗ \"О страховых пенсиях\"";
                    Telerik.Reporting.TextBox textBox34 = RSW2014_1.Items.Find("textBox34", true)[0] as Telerik.Reporting.TextBox;
                    textBox34.Value = "занятых на видах работ, указанных в пунктах 2 - 18 части 1 статьи 30 Федерального закона от 28 декабря 2013 года  № 400-ФЗ \"О страховых пенсиях\"";

                    Telerik.Reporting.TextBox textBox72 = RSW2014_1.Items.Find("textBox72", true)[0] as Telerik.Reporting.TextBox;
                    textBox72.Value = "Сумма перерасчета страховых взносов за предыдущие отчетные (расчетные) периоды с начала расчетного периода";
                    Telerik.Reporting.TextBox textBox56 = RSW2014_1.Items.Find("textBox56", true)[0] as Telerik.Reporting.TextBox;
                    textBox56.Value = "*_Собрание законодательства Российской Федерации, 2013, № 52, ст. 6965; 2014, № 2.";



                }

                Telerik.Reporting.TextBox RegNum1 = RSW2014_1.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                RegNum1.Value = regNum;
                Telerik.Reporting.TextBox s_100_0 = RSW2014_1.Items.Find("textBox7", true)[0] as Telerik.Reporting.TextBox;
                s_100_0.Value = Utils.decToStr(RSWdata.s_100_0);
                Telerik.Reporting.TextBox s_100_1 = RSW2014_1.Items.Find("textBox8", true)[0] as Telerik.Reporting.TextBox;
                s_100_1.Value = Utils.decToStr(RSWdata.s_100_1);
                Telerik.Reporting.TextBox s_100_2 = RSW2014_1.Items.Find("textBox10", true)[0] as Telerik.Reporting.TextBox;
                s_100_2.Value = Utils.decToStr(RSWdata.s_100_2);
                Telerik.Reporting.TextBox s_100_3 = RSW2014_1.Items.Find("textBox21", true)[0] as Telerik.Reporting.TextBox;
                s_100_3.Value = Utils.decToStr(RSWdata.s_100_3);
                Telerik.Reporting.TextBox s_100_4 = RSW2014_1.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                s_100_4.Value = Utils.decToStr(RSWdata.s_100_4);
                Telerik.Reporting.TextBox s_100_5 = RSW2014_1.Items.Find("textBox26", true)[0] as Telerik.Reporting.TextBox;
                s_100_5.Value = Utils.decToStr(RSWdata.s_100_5);

                Telerik.Reporting.TextBox s_110_0 = RSW2014_1.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                s_110_0.Value = Utils.decToStr(RSWdata.s_110_0);
                Telerik.Reporting.TextBox s_110_3 = RSW2014_1.Items.Find("textBox37", true)[0] as Telerik.Reporting.TextBox;
                s_110_3.Value = Utils.decToStr(RSWdata.s_110_3);
                Telerik.Reporting.TextBox s_110_4 = RSW2014_1.Items.Find("textBox38", true)[0] as Telerik.Reporting.TextBox;
                s_110_4.Value = Utils.decToStr(RSWdata.s_110_4);
                Telerik.Reporting.TextBox s_110_5 = RSW2014_1.Items.Find("textBox39", true)[0] as Telerik.Reporting.TextBox;
                s_110_5.Value = Utils.decToStr(RSWdata.s_110_5);

                Telerik.Reporting.TextBox s_111_0 = RSW2014_1.Items.Find("textBox42", true)[0] as Telerik.Reporting.TextBox;
                s_111_0.Value = Utils.decToStr(RSWdata.s_111_0);
                Telerik.Reporting.TextBox s_111_3 = RSW2014_1.Items.Find("textBox45", true)[0] as Telerik.Reporting.TextBox;
                s_111_3.Value = Utils.decToStr(RSWdata.s_111_3);
                Telerik.Reporting.TextBox s_111_4 = RSW2014_1.Items.Find("textBox46", true)[0] as Telerik.Reporting.TextBox;
                s_111_4.Value = Utils.decToStr(RSWdata.s_111_4);
                Telerik.Reporting.TextBox s_111_5 = RSW2014_1.Items.Find("textBox47", true)[0] as Telerik.Reporting.TextBox;
                s_111_5.Value = Utils.decToStr(RSWdata.s_111_5);

                Telerik.Reporting.TextBox s_112_0 = RSW2014_1.Items.Find("textBox50", true)[0] as Telerik.Reporting.TextBox;
                s_112_0.Value = Utils.decToStr(RSWdata.s_112_0);
                Telerik.Reporting.TextBox s_112_3 = RSW2014_1.Items.Find("textBox53", true)[0] as Telerik.Reporting.TextBox;
                s_112_3.Value = Utils.decToStr(RSWdata.s_112_3);
                Telerik.Reporting.TextBox s_112_4 = RSW2014_1.Items.Find("textBox54", true)[0] as Telerik.Reporting.TextBox;
                s_112_4.Value = Utils.decToStr(RSWdata.s_112_4);
                Telerik.Reporting.TextBox s_112_5 = RSW2014_1.Items.Find("textBox55", true)[0] as Telerik.Reporting.TextBox;
                s_112_5.Value = Utils.decToStr(RSWdata.s_112_5);

                Telerik.Reporting.TextBox s_113_0 = RSW2014_1.Items.Find("textBox58", true)[0] as Telerik.Reporting.TextBox;
                s_113_0.Value = Utils.decToStr(RSWdata.s_113_0);
                Telerik.Reporting.TextBox s_113_3 = RSW2014_1.Items.Find("textBox61", true)[0] as Telerik.Reporting.TextBox;
                s_113_3.Value = Utils.decToStr(RSWdata.s_113_3);
                Telerik.Reporting.TextBox s_113_4 = RSW2014_1.Items.Find("textBox62", true)[0] as Telerik.Reporting.TextBox;
                s_113_4.Value = Utils.decToStr(RSWdata.s_113_4);
                Telerik.Reporting.TextBox s_113_5 = RSW2014_1.Items.Find("textBox63", true)[0] as Telerik.Reporting.TextBox;
                s_113_5.Value = Utils.decToStr(RSWdata.s_113_5);

                Telerik.Reporting.TextBox s_114_0 = RSW2014_1.Items.Find("textBox66", true)[0] as Telerik.Reporting.TextBox;
                s_114_0.Value = Utils.decToStr(RSWdata.s_114_0);
                Telerik.Reporting.TextBox s_114_3 = RSW2014_1.Items.Find("textBox69", true)[0] as Telerik.Reporting.TextBox;
                s_114_3.Value = Utils.decToStr(RSWdata.s_114_3);
                Telerik.Reporting.TextBox s_114_4 = RSW2014_1.Items.Find("textBox70", true)[0] as Telerik.Reporting.TextBox;
                s_114_4.Value = Utils.decToStr(RSWdata.s_114_4);
                Telerik.Reporting.TextBox s_114_5 = RSW2014_1.Items.Find("textBox71", true)[0] as Telerik.Reporting.TextBox;
                s_114_5.Value = Utils.decToStr(RSWdata.s_114_5);


                Telerik.Reporting.TextBox s_120_0 = RSW2014_1.Items.Find("textBox74", true)[0] as Telerik.Reporting.TextBox;
                s_120_0.Value = Utils.decToStr(RSWdata.s_120_0);
                Telerik.Reporting.TextBox s_120_1 = RSW2014_1.Items.Find("textBox75", true)[0] as Telerik.Reporting.TextBox;
                s_120_1.Value = Utils.decToStr(RSWdata.s_120_1);
                Telerik.Reporting.TextBox s_120_2 = RSW2014_1.Items.Find("textBox76", true)[0] as Telerik.Reporting.TextBox;
                s_120_2.Value = Utils.decToStr(RSWdata.s_120_2);
                Telerik.Reporting.TextBox s_120_3 = RSW2014_1.Items.Find("textBox77", true)[0] as Telerik.Reporting.TextBox;
                s_120_3.Value = Utils.decToStr(RSWdata.s_120_3);
                Telerik.Reporting.TextBox s_120_4 = RSW2014_1.Items.Find("textBox78", true)[0] as Telerik.Reporting.TextBox;
                s_120_4.Value = Utils.decToStr(RSWdata.s_120_4);
                Telerik.Reporting.TextBox s_120_5 = RSW2014_1.Items.Find("textBox79", true)[0] as Telerik.Reporting.TextBox;
                s_120_5.Value = Utils.decToStr(RSWdata.s_120_5);

                Telerik.Reporting.TextBox s_121_0 = RSW2014_1.Items.Find("textBox82", true)[0] as Telerik.Reporting.TextBox;
                s_121_0.Value = Utils.decToStr(RSWdata.s_121_0);
                Telerik.Reporting.TextBox s_121_1 = RSW2014_1.Items.Find("textBox83", true)[0] as Telerik.Reporting.TextBox;
                s_121_1.Value = Utils.decToStr(RSWdata.s_121_1);

                Telerik.Reporting.TextBox s_130_0 = RSW2014_1.Items.Find("textBox90", true)[0] as Telerik.Reporting.TextBox;
                s_130_0.Value = Utils.decToStr(RSWdata.s_130_0);
                Telerik.Reporting.TextBox s_130_1 = RSW2014_1.Items.Find("textBox91", true)[0] as Telerik.Reporting.TextBox;
                s_130_1.Value = Utils.decToStr(RSWdata.s_130_1);
                Telerik.Reporting.TextBox s_130_2 = RSW2014_1.Items.Find("textBox92", true)[0] as Telerik.Reporting.TextBox;
                s_130_2.Value = Utils.decToStr(RSWdata.s_130_2);
                Telerik.Reporting.TextBox s_130_3 = RSW2014_1.Items.Find("textBox93", true)[0] as Telerik.Reporting.TextBox;
                s_130_3.Value = Utils.decToStr(RSWdata.s_130_3);
                Telerik.Reporting.TextBox s_130_4 = RSW2014_1.Items.Find("textBox94", true)[0] as Telerik.Reporting.TextBox;
                s_130_4.Value = Utils.decToStr(RSWdata.s_130_4);
                Telerik.Reporting.TextBox s_130_5 = RSW2014_1.Items.Find("textBox95", true)[0] as Telerik.Reporting.TextBox;
                s_130_5.Value = Utils.decToStr(RSWdata.s_130_5);

                Telerik.Reporting.TextBox s_140_0 = RSW2014_1.Items.Find("textBox98", true)[0] as Telerik.Reporting.TextBox;
                s_140_0.Value = Utils.decToStr(RSWdata.s_140_0);
                Telerik.Reporting.TextBox s_140_1 = RSW2014_1.Items.Find("textBox99", true)[0] as Telerik.Reporting.TextBox;
                s_140_1.Value = Utils.decToStr(RSWdata.s_140_1);
                Telerik.Reporting.TextBox s_140_2 = RSW2014_1.Items.Find("textBox100", true)[0] as Telerik.Reporting.TextBox;
                s_140_2.Value = Utils.decToStr(RSWdata.s_140_2);
                Telerik.Reporting.TextBox s_140_3 = RSW2014_1.Items.Find("textBox101", true)[0] as Telerik.Reporting.TextBox;
                s_140_3.Value = Utils.decToStr(RSWdata.s_140_3);
                Telerik.Reporting.TextBox s_140_4 = RSW2014_1.Items.Find("textBox102", true)[0] as Telerik.Reporting.TextBox;
                s_140_4.Value = Utils.decToStr(RSWdata.s_140_4);
                Telerik.Reporting.TextBox s_140_5 = RSW2014_1.Items.Find("textBox103", true)[0] as Telerik.Reporting.TextBox;
                s_140_5.Value = Utils.decToStr(RSWdata.s_140_5);

                Telerik.Reporting.TextBox s_141_0 = RSW2014_1.Items.Find("textBox106", true)[0] as Telerik.Reporting.TextBox;
                s_141_0.Value = Utils.decToStr(RSWdata.s_141_0);
                Telerik.Reporting.TextBox s_141_1 = RSW2014_1.Items.Find("textBox107", true)[0] as Telerik.Reporting.TextBox;
                s_141_1.Value = Utils.decToStr(RSWdata.s_141_1);
                Telerik.Reporting.TextBox s_141_2 = RSW2014_1.Items.Find("textBox108", true)[0] as Telerik.Reporting.TextBox;
                s_141_2.Value = Utils.decToStr(RSWdata.s_141_2);
                Telerik.Reporting.TextBox s_141_3 = RSW2014_1.Items.Find("textBox109", true)[0] as Telerik.Reporting.TextBox;
                s_141_3.Value = Utils.decToStr(RSWdata.s_141_3);
                Telerik.Reporting.TextBox s_141_4 = RSW2014_1.Items.Find("textBox110", true)[0] as Telerik.Reporting.TextBox;
                s_141_4.Value = Utils.decToStr(RSWdata.s_141_4);
                Telerik.Reporting.TextBox s_141_5 = RSW2014_1.Items.Find("textBox111", true)[0] as Telerik.Reporting.TextBox;
                s_141_5.Value = Utils.decToStr(RSWdata.s_141_5);

                Telerik.Reporting.TextBox s_142_0 = RSW2014_1.Items.Find("textBox114", true)[0] as Telerik.Reporting.TextBox;
                s_142_0.Value = Utils.decToStr(RSWdata.s_142_0);
                Telerik.Reporting.TextBox s_142_1 = RSW2014_1.Items.Find("textBox115", true)[0] as Telerik.Reporting.TextBox;
                s_142_1.Value = Utils.decToStr(RSWdata.s_142_1);
                Telerik.Reporting.TextBox s_142_2 = RSW2014_1.Items.Find("textBox116", true)[0] as Telerik.Reporting.TextBox;
                s_142_2.Value = Utils.decToStr(RSWdata.s_142_2);
                Telerik.Reporting.TextBox s_142_3 = RSW2014_1.Items.Find("textBox117", true)[0] as Telerik.Reporting.TextBox;
                s_142_3.Value = Utils.decToStr(RSWdata.s_142_3);
                Telerik.Reporting.TextBox s_142_4 = RSW2014_1.Items.Find("textBox118", true)[0] as Telerik.Reporting.TextBox;
                s_142_4.Value = Utils.decToStr(RSWdata.s_142_4);
                Telerik.Reporting.TextBox s_142_5 = RSW2014_1.Items.Find("textBox119", true)[0] as Telerik.Reporting.TextBox;
                s_142_5.Value = Utils.decToStr(RSWdata.s_142_5);

                Telerik.Reporting.TextBox s_143_0 = RSW2014_1.Items.Find("textBox122", true)[0] as Telerik.Reporting.TextBox;
                s_143_0.Value = Utils.decToStr(RSWdata.s_143_0);
                Telerik.Reporting.TextBox s_143_1 = RSW2014_1.Items.Find("textBox123", true)[0] as Telerik.Reporting.TextBox;
                s_143_1.Value = Utils.decToStr(RSWdata.s_143_1);
                Telerik.Reporting.TextBox s_143_2 = RSW2014_1.Items.Find("textBox124", true)[0] as Telerik.Reporting.TextBox;
                s_143_2.Value = Utils.decToStr(RSWdata.s_143_2);
                Telerik.Reporting.TextBox s_143_3 = RSW2014_1.Items.Find("textBox125", true)[0] as Telerik.Reporting.TextBox;
                s_143_3.Value = Utils.decToStr(RSWdata.s_143_3);
                Telerik.Reporting.TextBox s_143_4 = RSW2014_1.Items.Find("textBox126", true)[0] as Telerik.Reporting.TextBox;
                s_143_4.Value = Utils.decToStr(RSWdata.s_143_4);
                Telerik.Reporting.TextBox s_143_5 = RSW2014_1.Items.Find("textBox127", true)[0] as Telerik.Reporting.TextBox;
                s_143_5.Value = Utils.decToStr(RSWdata.s_143_5);

                Telerik.Reporting.TextBox s_144_0 = RSW2014_1.Items.Find("textBox130", true)[0] as Telerik.Reporting.TextBox;
                s_144_0.Value = Utils.decToStr(RSWdata.s_144_0);
                Telerik.Reporting.TextBox s_144_1 = RSW2014_1.Items.Find("textBox131", true)[0] as Telerik.Reporting.TextBox;
                s_144_1.Value = Utils.decToStr(RSWdata.s_144_1);
                Telerik.Reporting.TextBox s_144_2 = RSW2014_1.Items.Find("textBox132", true)[0] as Telerik.Reporting.TextBox;
                s_144_2.Value = Utils.decToStr(RSWdata.s_144_2);
                Telerik.Reporting.TextBox s_144_3 = RSW2014_1.Items.Find("textBox133", true)[0] as Telerik.Reporting.TextBox;
                s_144_3.Value = Utils.decToStr(RSWdata.s_144_3);
                Telerik.Reporting.TextBox s_144_4 = RSW2014_1.Items.Find("textBox134", true)[0] as Telerik.Reporting.TextBox;
                s_144_4.Value = Utils.decToStr(RSWdata.s_144_4);
                Telerik.Reporting.TextBox s_144_5 = RSW2014_1.Items.Find("textBox135", true)[0] as Telerik.Reporting.TextBox;
                s_144_5.Value = Utils.decToStr(RSWdata.s_144_5);

                Telerik.Reporting.TextBox s_150_0 = RSW2014_1.Items.Find("textBox138", true)[0] as Telerik.Reporting.TextBox;
                s_150_0.Value = Utils.decToStr(RSWdata.s_150_0);
                Telerik.Reporting.TextBox s_150_1 = RSW2014_1.Items.Find("textBox139", true)[0] as Telerik.Reporting.TextBox;
                s_150_1.Value = Utils.decToStr(RSWdata.s_150_1);
                Telerik.Reporting.TextBox s_150_2 = RSW2014_1.Items.Find("textBox140", true)[0] as Telerik.Reporting.TextBox;
                s_150_2.Value = Utils.decToStr(RSWdata.s_150_2);
                Telerik.Reporting.TextBox s_150_3 = RSW2014_1.Items.Find("textBox141", true)[0] as Telerik.Reporting.TextBox;
                s_150_3.Value = Utils.decToStr(RSWdata.s_150_3);
                Telerik.Reporting.TextBox s_150_4 = RSW2014_1.Items.Find("textBox142", true)[0] as Telerik.Reporting.TextBox;
                s_150_4.Value = Utils.decToStr(RSWdata.s_150_4);
                Telerik.Reporting.TextBox s_150_5 = RSW2014_1.Items.Find("textBox143", true)[0] as Telerik.Reporting.TextBox;
                s_150_5.Value = Utils.decToStr(RSWdata.s_150_5);
                RSW2014Book.Reports.Add(RSW2014_1);
                pageCnt++;
                /*                }
                                else if (yearType == 2015)
                                {
                                    RSW2014_Re1_2015 RSW2014_1 = new RSW2014_Re1_2015();

                                    Telerik.Reporting.TextBox RegNum1 = RSW2014_1.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                                    RegNum1.Value = regNum;
                                    Telerik.Reporting.TextBox s_100_0 = RSW2014_1.Items.Find("textBox7", true)[0] as Telerik.Reporting.TextBox;
                                    s_100_0.Value = RSWdata.s_100_0.ToString();
                                    Telerik.Reporting.TextBox s_100_1 = RSW2014_1.Items.Find("textBox8", true)[0] as Telerik.Reporting.TextBox;
                                    s_100_1.Value = RSWdata.s_100_1.ToString();
                                    Telerik.Reporting.TextBox s_100_2 = RSW2014_1.Items.Find("textBox10", true)[0] as Telerik.Reporting.TextBox;
                                    s_100_2.Value = RSWdata.s_100_2.ToString();
                                    Telerik.Reporting.TextBox s_100_3 = RSW2014_1.Items.Find("textBox21", true)[0] as Telerik.Reporting.TextBox;
                                    s_100_3.Value = RSWdata.s_100_3.ToString();
                                    Telerik.Reporting.TextBox s_100_4 = RSW2014_1.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                                    s_100_4.Value = RSWdata.s_100_4.ToString();
                                    Telerik.Reporting.TextBox s_100_5 = RSW2014_1.Items.Find("textBox26", true)[0] as Telerik.Reporting.TextBox;
                                    s_100_5.Value = RSWdata.s_100_5.ToString();

                                    Telerik.Reporting.TextBox s_110_0 = RSW2014_1.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                                    s_110_0.Value = RSWdata.s_110_0.ToString();
                                    Telerik.Reporting.TextBox s_110_3 = RSW2014_1.Items.Find("textBox37", true)[0] as Telerik.Reporting.TextBox;
                                    s_110_3.Value = RSWdata.s_110_3.ToString();
                                    Telerik.Reporting.TextBox s_110_4 = RSW2014_1.Items.Find("textBox38", true)[0] as Telerik.Reporting.TextBox;
                                    s_110_4.Value = RSWdata.s_110_4.ToString();
                                    Telerik.Reporting.TextBox s_110_5 = RSW2014_1.Items.Find("textBox39", true)[0] as Telerik.Reporting.TextBox;
                                    s_110_5.Value = RSWdata.s_110_5.ToString();

                                    Telerik.Reporting.TextBox s_111_0 = RSW2014_1.Items.Find("textBox42", true)[0] as Telerik.Reporting.TextBox;
                                    s_111_0.Value = RSWdata.s_111_0.ToString();
                                    Telerik.Reporting.TextBox s_111_3 = RSW2014_1.Items.Find("textBox45", true)[0] as Telerik.Reporting.TextBox;
                                    s_111_3.Value = RSWdata.s_111_3.ToString();
                                    Telerik.Reporting.TextBox s_111_4 = RSW2014_1.Items.Find("textBox46", true)[0] as Telerik.Reporting.TextBox;
                                    s_111_4.Value = RSWdata.s_111_4.ToString();
                                    Telerik.Reporting.TextBox s_111_5 = RSW2014_1.Items.Find("textBox47", true)[0] as Telerik.Reporting.TextBox;
                                    s_111_5.Value = RSWdata.s_111_5.ToString();

                                    Telerik.Reporting.TextBox s_112_0 = RSW2014_1.Items.Find("textBox50", true)[0] as Telerik.Reporting.TextBox;
                                    s_112_0.Value = RSWdata.s_112_0.ToString();
                                    Telerik.Reporting.TextBox s_112_3 = RSW2014_1.Items.Find("textBox53", true)[0] as Telerik.Reporting.TextBox;
                                    s_112_3.Value = RSWdata.s_112_3.ToString();
                                    Telerik.Reporting.TextBox s_112_4 = RSW2014_1.Items.Find("textBox54", true)[0] as Telerik.Reporting.TextBox;
                                    s_112_4.Value = RSWdata.s_112_4.ToString();
                                    Telerik.Reporting.TextBox s_112_5 = RSW2014_1.Items.Find("textBox55", true)[0] as Telerik.Reporting.TextBox;
                                    s_112_5.Value = RSWdata.s_112_5.ToString();

                                    Telerik.Reporting.TextBox s_113_0 = RSW2014_1.Items.Find("textBox58", true)[0] as Telerik.Reporting.TextBox;
                                    s_113_0.Value = RSWdata.s_113_0.ToString();
                                    Telerik.Reporting.TextBox s_113_3 = RSW2014_1.Items.Find("textBox61", true)[0] as Telerik.Reporting.TextBox;
                                    s_113_3.Value = RSWdata.s_113_3.ToString();
                                    Telerik.Reporting.TextBox s_113_4 = RSW2014_1.Items.Find("textBox62", true)[0] as Telerik.Reporting.TextBox;
                                    s_113_4.Value = RSWdata.s_113_4.ToString();
                                    Telerik.Reporting.TextBox s_113_5 = RSW2014_1.Items.Find("textBox63", true)[0] as Telerik.Reporting.TextBox;
                                    s_113_5.Value = RSWdata.s_113_5.ToString();

                                    Telerik.Reporting.TextBox s_114_0 = RSW2014_1.Items.Find("textBox66", true)[0] as Telerik.Reporting.TextBox;
                                    s_114_0.Value = RSWdata.s_114_0.ToString();
                                    Telerik.Reporting.TextBox s_114_3 = RSW2014_1.Items.Find("textBox69", true)[0] as Telerik.Reporting.TextBox;
                                    s_114_3.Value = RSWdata.s_114_3.ToString();
                                    Telerik.Reporting.TextBox s_114_4 = RSW2014_1.Items.Find("textBox70", true)[0] as Telerik.Reporting.TextBox;
                                    s_114_4.Value = RSWdata.s_114_4.ToString();
                                    Telerik.Reporting.TextBox s_114_5 = RSW2014_1.Items.Find("textBox71", true)[0] as Telerik.Reporting.TextBox;
                                    s_114_5.Value = RSWdata.s_114_5.ToString();


                                    Telerik.Reporting.TextBox s_120_0 = RSW2014_1.Items.Find("textBox74", true)[0] as Telerik.Reporting.TextBox;
                                    s_120_0.Value = RSWdata.s_120_0.ToString();
                                    Telerik.Reporting.TextBox s_120_1 = RSW2014_1.Items.Find("textBox75", true)[0] as Telerik.Reporting.TextBox;
                                    s_120_1.Value = RSWdata.s_120_1.ToString();
                                    Telerik.Reporting.TextBox s_120_2 = RSW2014_1.Items.Find("textBox76", true)[0] as Telerik.Reporting.TextBox;
                                    s_120_2.Value = RSWdata.s_120_2.ToString();
                                    Telerik.Reporting.TextBox s_120_3 = RSW2014_1.Items.Find("textBox77", true)[0] as Telerik.Reporting.TextBox;
                                    s_120_3.Value = RSWdata.s_120_3.ToString();
                                    Telerik.Reporting.TextBox s_120_4 = RSW2014_1.Items.Find("textBox78", true)[0] as Telerik.Reporting.TextBox;
                                    s_120_4.Value = RSWdata.s_120_4.ToString();
                                    Telerik.Reporting.TextBox s_120_5 = RSW2014_1.Items.Find("textBox79", true)[0] as Telerik.Reporting.TextBox;
                                    s_120_5.Value = RSWdata.s_120_5.ToString();

                                    Telerik.Reporting.TextBox s_121_0 = RSW2014_1.Items.Find("textBox82", true)[0] as Telerik.Reporting.TextBox;
                                    s_121_0.Value = RSWdata.s_121_0.ToString();
                                    Telerik.Reporting.TextBox s_121_1 = RSW2014_1.Items.Find("textBox83", true)[0] as Telerik.Reporting.TextBox;
                                    s_121_1.Value = RSWdata.s_121_1.ToString();

                                    Telerik.Reporting.TextBox s_122_0 = RSW2014_1.Items.Find("textBox164", true)[0] as Telerik.Reporting.TextBox;
                                    s_122_0.Value = RSWdata.s_122_0.ToString();
                                    Telerik.Reporting.TextBox s_122_1 = RSW2014_1.Items.Find("textBox165", true)[0] as Telerik.Reporting.TextBox;
                                    s_122_1.Value = RSWdata.s_122_1.ToString();
                                    Telerik.Reporting.TextBox s_122_2 = RSW2014_1.Items.Find("textBox166", true)[0] as Telerik.Reporting.TextBox;
                                    s_122_2.Value = RSWdata.s_122_2.ToString();
                                    Telerik.Reporting.TextBox s_122_3 = RSW2014_1.Items.Find("textBox167", true)[0] as Telerik.Reporting.TextBox;
                                    s_122_3.Value = RSWdata.s_122_3.ToString();
                                    Telerik.Reporting.TextBox s_122_4 = RSW2014_1.Items.Find("textBox168", true)[0] as Telerik.Reporting.TextBox;
                                    s_122_4.Value = RSWdata.s_122_4.ToString();
                                    Telerik.Reporting.TextBox s_122_5 = RSW2014_1.Items.Find("textBox169", true)[0] as Telerik.Reporting.TextBox;
                                    s_122_5.Value = RSWdata.s_122_5.ToString();

                                    Telerik.Reporting.TextBox s_123_0 = RSW2014_1.Items.Find("textBox149", true)[0] as Telerik.Reporting.TextBox;
                                    s_123_0.Value = RSWdata.s_123_0.ToString();
                                    Telerik.Reporting.TextBox s_123_1 = RSW2014_1.Items.Find("textBox153", true)[0] as Telerik.Reporting.TextBox;
                                    s_123_1.Value = RSWdata.s_123_1.ToString();


                                    Telerik.Reporting.TextBox s_130_0 = RSW2014_1.Items.Find("textBox90", true)[0] as Telerik.Reporting.TextBox;
                                    s_130_0.Value = RSWdata.s_130_0.ToString();
                                    Telerik.Reporting.TextBox s_130_1 = RSW2014_1.Items.Find("textBox91", true)[0] as Telerik.Reporting.TextBox;
                                    s_130_1.Value = RSWdata.s_130_1.ToString();
                                    Telerik.Reporting.TextBox s_130_2 = RSW2014_1.Items.Find("textBox92", true)[0] as Telerik.Reporting.TextBox;
                                    s_130_2.Value = RSWdata.s_130_2.ToString();
                                    Telerik.Reporting.TextBox s_130_3 = RSW2014_1.Items.Find("textBox93", true)[0] as Telerik.Reporting.TextBox;
                                    s_130_3.Value = RSWdata.s_130_3.ToString();
                                    Telerik.Reporting.TextBox s_130_4 = RSW2014_1.Items.Find("textBox94", true)[0] as Telerik.Reporting.TextBox;
                                    s_130_4.Value = RSWdata.s_130_4.ToString();
                                    Telerik.Reporting.TextBox s_130_5 = RSW2014_1.Items.Find("textBox95", true)[0] as Telerik.Reporting.TextBox;
                                    s_130_5.Value = RSWdata.s_130_5.ToString();

                                    Telerik.Reporting.TextBox s_140_0 = RSW2014_1.Items.Find("textBox98", true)[0] as Telerik.Reporting.TextBox;
                                    s_140_0.Value = RSWdata.s_140_0.ToString();
                                    Telerik.Reporting.TextBox s_140_1 = RSW2014_1.Items.Find("textBox99", true)[0] as Telerik.Reporting.TextBox;
                                    s_140_1.Value = RSWdata.s_140_1.ToString();
                                    Telerik.Reporting.TextBox s_140_2 = RSW2014_1.Items.Find("textBox100", true)[0] as Telerik.Reporting.TextBox;
                                    s_140_2.Value = RSWdata.s_140_2.ToString();
                                    Telerik.Reporting.TextBox s_140_3 = RSW2014_1.Items.Find("textBox101", true)[0] as Telerik.Reporting.TextBox;
                                    s_140_3.Value = RSWdata.s_140_3.ToString();
                                    Telerik.Reporting.TextBox s_140_4 = RSW2014_1.Items.Find("textBox102", true)[0] as Telerik.Reporting.TextBox;
                                    s_140_4.Value = RSWdata.s_140_4.ToString();
                                    Telerik.Reporting.TextBox s_140_5 = RSW2014_1.Items.Find("textBox103", true)[0] as Telerik.Reporting.TextBox;
                                    s_140_5.Value = RSWdata.s_140_5.ToString();

                                    Telerik.Reporting.TextBox s_141_0 = RSW2014_1.Items.Find("textBox106", true)[0] as Telerik.Reporting.TextBox;
                                    s_141_0.Value = RSWdata.s_141_0.ToString();
                                    Telerik.Reporting.TextBox s_141_1 = RSW2014_1.Items.Find("textBox107", true)[0] as Telerik.Reporting.TextBox;
                                    s_141_1.Value = RSWdata.s_141_1.ToString();
                                    Telerik.Reporting.TextBox s_141_2 = RSW2014_1.Items.Find("textBox108", true)[0] as Telerik.Reporting.TextBox;
                                    s_141_2.Value = RSWdata.s_141_2.ToString();
                                    Telerik.Reporting.TextBox s_141_3 = RSW2014_1.Items.Find("textBox109", true)[0] as Telerik.Reporting.TextBox;
                                    s_141_3.Value = RSWdata.s_141_3.ToString();
                                    Telerik.Reporting.TextBox s_141_4 = RSW2014_1.Items.Find("textBox110", true)[0] as Telerik.Reporting.TextBox;
                                    s_141_4.Value = RSWdata.s_141_4.ToString();
                                    Telerik.Reporting.TextBox s_141_5 = RSW2014_1.Items.Find("textBox111", true)[0] as Telerik.Reporting.TextBox;
                                    s_141_5.Value = RSWdata.s_141_5.ToString();

                                    Telerik.Reporting.TextBox s_142_0 = RSW2014_1.Items.Find("textBox114", true)[0] as Telerik.Reporting.TextBox;
                                    s_142_0.Value = RSWdata.s_142_0.ToString();
                                    Telerik.Reporting.TextBox s_142_1 = RSW2014_1.Items.Find("textBox115", true)[0] as Telerik.Reporting.TextBox;
                                    s_142_1.Value = RSWdata.s_142_1.ToString();
                                    Telerik.Reporting.TextBox s_142_2 = RSW2014_1.Items.Find("textBox116", true)[0] as Telerik.Reporting.TextBox;
                                    s_142_2.Value = RSWdata.s_142_2.ToString();
                                    Telerik.Reporting.TextBox s_142_3 = RSW2014_1.Items.Find("textBox117", true)[0] as Telerik.Reporting.TextBox;
                                    s_142_3.Value = RSWdata.s_142_3.ToString();
                                    Telerik.Reporting.TextBox s_142_4 = RSW2014_1.Items.Find("textBox118", true)[0] as Telerik.Reporting.TextBox;
                                    s_142_4.Value = RSWdata.s_142_4.ToString();
                                    Telerik.Reporting.TextBox s_142_5 = RSW2014_1.Items.Find("textBox119", true)[0] as Telerik.Reporting.TextBox;
                                    s_142_5.Value = RSWdata.s_142_5.ToString();

                                    Telerik.Reporting.TextBox s_143_0 = RSW2014_1.Items.Find("textBox122", true)[0] as Telerik.Reporting.TextBox;
                                    s_143_0.Value = RSWdata.s_143_0.ToString();
                                    Telerik.Reporting.TextBox s_143_1 = RSW2014_1.Items.Find("textBox123", true)[0] as Telerik.Reporting.TextBox;
                                    s_143_1.Value = RSWdata.s_143_1.ToString();
                                    Telerik.Reporting.TextBox s_143_2 = RSW2014_1.Items.Find("textBox124", true)[0] as Telerik.Reporting.TextBox;
                                    s_143_2.Value = RSWdata.s_143_2.ToString();
                                    Telerik.Reporting.TextBox s_143_3 = RSW2014_1.Items.Find("textBox125", true)[0] as Telerik.Reporting.TextBox;
                                    s_143_3.Value = RSWdata.s_143_3.ToString();
                                    Telerik.Reporting.TextBox s_143_4 = RSW2014_1.Items.Find("textBox126", true)[0] as Telerik.Reporting.TextBox;
                                    s_143_4.Value = RSWdata.s_143_4.ToString();
                                    Telerik.Reporting.TextBox s_143_5 = RSW2014_1.Items.Find("textBox127", true)[0] as Telerik.Reporting.TextBox;
                                    s_143_5.Value = RSWdata.s_143_5.ToString();

                                    Telerik.Reporting.TextBox s_144_0 = RSW2014_1.Items.Find("textBox130", true)[0] as Telerik.Reporting.TextBox;
                                    s_144_0.Value = RSWdata.s_144_0.ToString();
                                    Telerik.Reporting.TextBox s_144_1 = RSW2014_1.Items.Find("textBox131", true)[0] as Telerik.Reporting.TextBox;
                                    s_144_1.Value = RSWdata.s_144_1.ToString();
                                    Telerik.Reporting.TextBox s_144_2 = RSW2014_1.Items.Find("textBox132", true)[0] as Telerik.Reporting.TextBox;
                                    s_144_2.Value = RSWdata.s_144_2.ToString();
                                    Telerik.Reporting.TextBox s_144_3 = RSW2014_1.Items.Find("textBox133", true)[0] as Telerik.Reporting.TextBox;
                                    s_144_3.Value = RSWdata.s_144_3.ToString();
                                    Telerik.Reporting.TextBox s_144_4 = RSW2014_1.Items.Find("textBox134", true)[0] as Telerik.Reporting.TextBox;
                                    s_144_4.Value = RSWdata.s_144_4.ToString();
                                    Telerik.Reporting.TextBox s_144_5 = RSW2014_1.Items.Find("textBox135", true)[0] as Telerik.Reporting.TextBox;
                                    s_144_5.Value = RSWdata.s_144_5.ToString();

                                    Telerik.Reporting.TextBox s_150_0 = RSW2014_1.Items.Find("textBox138", true)[0] as Telerik.Reporting.TextBox;
                                    s_150_0.Value = RSWdata.s_150_0.ToString();
                                    Telerik.Reporting.TextBox s_150_1 = RSW2014_1.Items.Find("textBox139", true)[0] as Telerik.Reporting.TextBox;
                                    s_150_1.Value = RSWdata.s_150_1.ToString();
                                    Telerik.Reporting.TextBox s_150_2 = RSW2014_1.Items.Find("textBox140", true)[0] as Telerik.Reporting.TextBox;
                                    s_150_2.Value = RSWdata.s_150_2.ToString();
                                    Telerik.Reporting.TextBox s_150_3 = RSW2014_1.Items.Find("textBox141", true)[0] as Telerik.Reporting.TextBox;
                                    s_150_3.Value = RSWdata.s_150_3.ToString();
                                    Telerik.Reporting.TextBox s_150_4 = RSW2014_1.Items.Find("textBox142", true)[0] as Telerik.Reporting.TextBox;
                                    s_150_4.Value = RSWdata.s_150_4.ToString();
                                    Telerik.Reporting.TextBox s_150_5 = RSW2014_1.Items.Find("textBox143", true)[0] as Telerik.Reporting.TextBox;
                                    s_150_5.Value = RSWdata.s_150_5.ToString();
                                    RSW2014Book.Reports.Add(RSW2014_1);
                                    pageCnt++;
                                }
                */

                #endregion



                //Раздел 2.1
                if (!fromXMLflag)
                {
                    RSW_2_1_List = db.FormsRSW2014_1_Razd_2_1.Where(x => x.Year == RSWdata.Year && x.Quarter == RSWdata.Quarter
                        && x.InsurerID == RSWdata.InsurerID && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(y => y.ID).ToList();
                }

                //для каждого кода тарифа выводим отдельную страницу
                foreach (var item in RSW_2_1_List)
                {
                    TariffCode TariffCode = new TariffCode();
                    TariffCode = db.TariffCode.FirstOrDefault(x => x.ID == item.TariffCodeID);

                    if (yearType == 2014 || yearType == 2012)
                    {
                        RSW2014_Re21 report21 = new RSW2014_Re21();
                        (report21.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                        Telerik.Reporting.TextBox txt = report21.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                        txt.Value = regNum;
                        Telerik.Reporting.TextBox txt1 = report21.Items.Find("textBox6", true)[0] as Telerik.Reporting.TextBox;
                        txt1.Value = TariffCode.Code.ToString();
                        Telerik.Reporting.TextBox s_200_0 = report21.Items.Find("textBox21", true)[0] as Telerik.Reporting.TextBox;
                        s_200_0.Value = Utils.decToStr(item.s_200_0);
                        Telerik.Reporting.TextBox s_200_1 = report21.Items.Find("textBox22", true)[0] as Telerik.Reporting.TextBox;
                        s_200_1.Value = Utils.decToStr(item.s_200_1);
                        Telerik.Reporting.TextBox s_200_2 = report21.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox;
                        s_200_2.Value = Utils.decToStr(item.s_200_2);
                        Telerik.Reporting.TextBox s_200_3 = report21.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                        s_200_3.Value = Utils.decToStr(item.s_200_3);

                        Telerik.Reporting.TextBox s_201_0 = report21.Items.Find("textBox37", true)[0] as Telerik.Reporting.TextBox;
                        s_201_0.Value = Utils.decToStr(item.s_201_0);
                        Telerik.Reporting.TextBox s_201_1 = report21.Items.Find("textBox38", true)[0] as Telerik.Reporting.TextBox;
                        s_201_1.Value = Utils.decToStr(item.s_201_1);
                        Telerik.Reporting.TextBox s_201_2 = report21.Items.Find("textBox39", true)[0] as Telerik.Reporting.TextBox;
                        s_201_2.Value = Utils.decToStr(item.s_201_2);
                        Telerik.Reporting.TextBox s_201_3 = report21.Items.Find("textBox40", true)[0] as Telerik.Reporting.TextBox;
                        s_201_3.Value = Utils.decToStr(item.s_201_3);

                        Telerik.Reporting.TextBox s_202_0 = report21.Items.Find("textBox44", true)[0] as Telerik.Reporting.TextBox;
                        s_202_0.Value = Utils.decToStr(item.s_202_0);
                        Telerik.Reporting.TextBox s_202_1 = report21.Items.Find("textBox45", true)[0] as Telerik.Reporting.TextBox;
                        s_202_1.Value = Utils.decToStr(item.s_202_1);
                        Telerik.Reporting.TextBox s_202_2 = report21.Items.Find("textBox46", true)[0] as Telerik.Reporting.TextBox;
                        s_202_2.Value = Utils.decToStr(item.s_202_2);
                        Telerik.Reporting.TextBox s_202_3 = report21.Items.Find("textBox47", true)[0] as Telerik.Reporting.TextBox;
                        s_202_3.Value = Utils.decToStr(item.s_202_3);

                        Telerik.Reporting.TextBox s_203_0 = report21.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox;
                        s_203_0.Value = Utils.decToStr(item.s_203_0);
                        Telerik.Reporting.TextBox s_203_1 = report21.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
                        s_203_1.Value = Utils.decToStr(item.s_203_1);
                        Telerik.Reporting.TextBox s_203_2 = report21.Items.Find("textBox34", true)[0] as Telerik.Reporting.TextBox;
                        s_203_2.Value = Utils.decToStr(item.s_203_2);
                        Telerik.Reporting.TextBox s_203_3 = report21.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox;
                        s_203_3.Value = Utils.decToStr(item.s_203_3);

                        Telerik.Reporting.TextBox s_204_0 = report21.Items.Find("textBox51", true)[0] as Telerik.Reporting.TextBox;
                        s_204_0.Value = Utils.decToStr(item.s_204_0);
                        Telerik.Reporting.TextBox s_204_1 = report21.Items.Find("textBox52", true)[0] as Telerik.Reporting.TextBox;
                        s_204_1.Value = Utils.decToStr(item.s_204_1);
                        Telerik.Reporting.TextBox s_204_2 = report21.Items.Find("textBox53", true)[0] as Telerik.Reporting.TextBox;
                        s_204_2.Value = Utils.decToStr(item.s_204_2);
                        Telerik.Reporting.TextBox s_204_3 = report21.Items.Find("textBox54", true)[0] as Telerik.Reporting.TextBox;
                        s_204_3.Value = Utils.decToStr(item.s_204_3);

                        Telerik.Reporting.TextBox s_205_0 = report21.Items.Find("textBox59", true)[0] as Telerik.Reporting.TextBox;
                        s_205_0.Value = Utils.decToStr(item.s_205_0);
                        Telerik.Reporting.TextBox s_205_1 = report21.Items.Find("textBox60", true)[0] as Telerik.Reporting.TextBox;
                        s_205_1.Value = Utils.decToStr(item.s_205_1);
                        Telerik.Reporting.TextBox s_205_2 = report21.Items.Find("textBox61", true)[0] as Telerik.Reporting.TextBox;
                        s_205_2.Value = Utils.decToStr(item.s_205_2);
                        Telerik.Reporting.TextBox s_205_3 = report21.Items.Find("textBox62", true)[0] as Telerik.Reporting.TextBox;
                        s_205_3.Value = Utils.decToStr(item.s_205_3);

                        Telerik.Reporting.TextBox s_206_0 = report21.Items.Find("textBox66", true)[0] as Telerik.Reporting.TextBox;
                        s_206_0.Value = Utils.decToStr(item.s_206_0);
                        Telerik.Reporting.TextBox s_206_1 = report21.Items.Find("textBox67", true)[0] as Telerik.Reporting.TextBox;
                        s_206_1.Value = Utils.decToStr(item.s_206_1);
                        Telerik.Reporting.TextBox s_206_2 = report21.Items.Find("textBox68", true)[0] as Telerik.Reporting.TextBox;
                        s_206_2.Value = Utils.decToStr(item.s_206_2);
                        Telerik.Reporting.TextBox s_206_3 = report21.Items.Find("textBox69", true)[0] as Telerik.Reporting.TextBox;
                        s_206_3.Value = Utils.decToStr(item.s_206_3);

                        Telerik.Reporting.TextBox s_207_0 = report21.Items.Find("textBox73", true)[0] as Telerik.Reporting.TextBox;
                        s_207_0.Value = item.s_207_0.ToString();
                        Telerik.Reporting.TextBox s_207_1 = report21.Items.Find("textBox74", true)[0] as Telerik.Reporting.TextBox;
                        s_207_1.Value = item.s_207_1.ToString();
                        Telerik.Reporting.TextBox s_207_2 = report21.Items.Find("textBox75", true)[0] as Telerik.Reporting.TextBox;
                        s_207_2.Value = item.s_207_2.ToString();
                        Telerik.Reporting.TextBox s_207_3 = report21.Items.Find("textBox76", true)[0] as Telerik.Reporting.TextBox;
                        s_207_3.Value = item.s_207_3.ToString();

                        Telerik.Reporting.TextBox s_208_0 = report21.Items.Find("textBox71", true)[0] as Telerik.Reporting.TextBox;
                        s_208_0.Value = item.s_208_0.ToString();
                        Telerik.Reporting.TextBox s_208_1 = report21.Items.Find("textBox77", true)[0] as Telerik.Reporting.TextBox;
                        s_208_1.Value = item.s_208_1.ToString();
                        Telerik.Reporting.TextBox s_208_2 = report21.Items.Find("textBox78", true)[0] as Telerik.Reporting.TextBox;
                        s_208_2.Value = item.s_208_2.ToString();
                        Telerik.Reporting.TextBox s_208_3 = report21.Items.Find("textBox79", true)[0] as Telerik.Reporting.TextBox;
                        s_208_3.Value = item.s_208_3.ToString();

                        Telerik.Reporting.TextBox s_210_0 = report21.Items.Find("textBox89", true)[0] as Telerik.Reporting.TextBox;
                        s_210_0.Value = Utils.decToStr(item.s_210_0);
                        Telerik.Reporting.TextBox s_210_1 = report21.Items.Find("textBox90", true)[0] as Telerik.Reporting.TextBox;
                        s_210_1.Value = Utils.decToStr(item.s_210_1);
                        Telerik.Reporting.TextBox s_210_2 = report21.Items.Find("textBox91", true)[0] as Telerik.Reporting.TextBox;
                        s_210_2.Value = Utils.decToStr(item.s_210_2);
                        Telerik.Reporting.TextBox s_210_3 = report21.Items.Find("textBox92", true)[0] as Telerik.Reporting.TextBox;
                        s_210_3.Value = Utils.decToStr(item.s_210_3);

                        Telerik.Reporting.TextBox s_211_0 = report21.Items.Find("textBox96", true)[0] as Telerik.Reporting.TextBox;
                        s_211_0.Value = Utils.decToStr(item.s_211_0);
                        Telerik.Reporting.TextBox s_211_1 = report21.Items.Find("textBox97", true)[0] as Telerik.Reporting.TextBox;
                        s_211_1.Value = Utils.decToStr(item.s_211_1);
                        Telerik.Reporting.TextBox s_211_2 = report21.Items.Find("textBox98", true)[0] as Telerik.Reporting.TextBox;
                        s_211_2.Value = Utils.decToStr(item.s_211_2);
                        Telerik.Reporting.TextBox s_211_3 = report21.Items.Find("textBox99", true)[0] as Telerik.Reporting.TextBox;
                        s_211_3.Value = Utils.decToStr(item.s_211_3);

                        Telerik.Reporting.TextBox s_212_0 = report21.Items.Find("textBox103", true)[0] as Telerik.Reporting.TextBox;
                        s_212_0.Value = Utils.decToStr(item.s_212_0);
                        Telerik.Reporting.TextBox s_212_1 = report21.Items.Find("textBox104", true)[0] as Telerik.Reporting.TextBox;
                        s_212_1.Value = Utils.decToStr(item.s_212_1);
                        Telerik.Reporting.TextBox s_212_2 = report21.Items.Find("textBox105", true)[0] as Telerik.Reporting.TextBox;
                        s_212_2.Value = Utils.decToStr(item.s_212_2);
                        Telerik.Reporting.TextBox s_212_3 = report21.Items.Find("textBox106", true)[0] as Telerik.Reporting.TextBox;
                        s_212_3.Value = Utils.decToStr(item.s_212_3);

                        Telerik.Reporting.TextBox s_213_0 = report21.Items.Find("textBox110", true)[0] as Telerik.Reporting.TextBox;
                        s_213_0.Value = Utils.decToStr(item.s_213_0);
                        Telerik.Reporting.TextBox s_213_1 = report21.Items.Find("textBox111", true)[0] as Telerik.Reporting.TextBox;
                        s_213_1.Value = Utils.decToStr(item.s_213_1);
                        Telerik.Reporting.TextBox s_213_2 = report21.Items.Find("textBox113", true)[0] as Telerik.Reporting.TextBox;
                        s_213_2.Value = Utils.decToStr(item.s_213_2);
                        Telerik.Reporting.TextBox s_213_3 = report21.Items.Find("textBox114", true)[0] as Telerik.Reporting.TextBox;
                        s_213_3.Value = Utils.decToStr(item.s_213_3);

                        Telerik.Reporting.TextBox s_214_0 = report21.Items.Find("textBox118", true)[0] as Telerik.Reporting.TextBox;
                        s_214_0.Value = Utils.decToStr(item.s_214_0);
                        Telerik.Reporting.TextBox s_214_1 = report21.Items.Find("textBox119", true)[0] as Telerik.Reporting.TextBox;
                        s_214_1.Value = Utils.decToStr(item.s_214_1);
                        Telerik.Reporting.TextBox s_214_2 = report21.Items.Find("textBox121", true)[0] as Telerik.Reporting.TextBox;
                        s_214_2.Value = Utils.decToStr(item.s_214_2);
                        Telerik.Reporting.TextBox s_214_3 = report21.Items.Find("textBox122", true)[0] as Telerik.Reporting.TextBox;
                        s_214_3.Value = Utils.decToStr(item.s_214_3);

                        Telerik.Reporting.TextBox s_215_0 = report21.Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
                        s_215_0.Value = Utils.decToStr(item.s_215_0);
                        Telerik.Reporting.TextBox s_215_1 = report21.Items.Find("textBox85", true)[0] as Telerik.Reporting.TextBox;
                        s_215_1.Value = Utils.decToStr(item.s_215_1);
                        Telerik.Reporting.TextBox s_215_2 = report21.Items.Find("textBox86", true)[0] as Telerik.Reporting.TextBox;
                        s_215_2.Value = Utils.decToStr(item.s_215_2);
                        Telerik.Reporting.TextBox s_215_3 = report21.Items.Find("textBox87", true)[0] as Telerik.Reporting.TextBox;
                        s_215_3.Value = Utils.decToStr(item.s_215_3);
                        RSW2014Book.Reports.Add(report21);
                    }
                    else if (yearType == 2015)
                    {
                        RSW2014_Re21_2015 report21 = new RSW2014_Re21_2015();
                        (report21.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                        Telerik.Reporting.TextBox textBox56 = report21.Items.Find("textBox56", true)[0] as Telerik.Reporting.TextBox;
                        textBox56.Value = " * Представляется плательщиками страховых взносов отдельно по каждому тарифу, применяемому в отношении выплат застрахованным лицам.";


                        Telerik.Reporting.TextBox txt = report21.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                        txt.Value = regNum;
                        Telerik.Reporting.TextBox txt1 = report21.Items.Find("textBox6", true)[0] as Telerik.Reporting.TextBox;
                        txt1.Value = TariffCode.Code.ToString();
                        Telerik.Reporting.TextBox s_200_0 = report21.Items.Find("textBox21", true)[0] as Telerik.Reporting.TextBox;
                        s_200_0.Value = Utils.decToStr(item.s_200_0);
                        Telerik.Reporting.TextBox s_200_1 = report21.Items.Find("textBox22", true)[0] as Telerik.Reporting.TextBox;
                        s_200_1.Value = Utils.decToStr(item.s_200_1);
                        Telerik.Reporting.TextBox s_200_2 = report21.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox;
                        s_200_2.Value = Utils.decToStr(item.s_200_2);
                        Telerik.Reporting.TextBox s_200_3 = report21.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                        s_200_3.Value = Utils.decToStr(item.s_200_3);

                        Telerik.Reporting.TextBox s_201_0 = report21.Items.Find("textBox37", true)[0] as Telerik.Reporting.TextBox;
                        s_201_0.Value = Utils.decToStr(item.s_201_0);
                        Telerik.Reporting.TextBox s_201_1 = report21.Items.Find("textBox38", true)[0] as Telerik.Reporting.TextBox;
                        s_201_1.Value = Utils.decToStr(item.s_201_1);
                        Telerik.Reporting.TextBox s_201_2 = report21.Items.Find("textBox39", true)[0] as Telerik.Reporting.TextBox;
                        s_201_2.Value = Utils.decToStr(item.s_201_2);
                        Telerik.Reporting.TextBox s_201_3 = report21.Items.Find("textBox40", true)[0] as Telerik.Reporting.TextBox;
                        s_201_3.Value = Utils.decToStr(item.s_201_3);

                        Telerik.Reporting.TextBox s_202_0 = report21.Items.Find("textBox44", true)[0] as Telerik.Reporting.TextBox;
                        s_202_0.Value = Utils.decToStr(item.s_202_0);
                        Telerik.Reporting.TextBox s_202_1 = report21.Items.Find("textBox45", true)[0] as Telerik.Reporting.TextBox;
                        s_202_1.Value = Utils.decToStr(item.s_202_1);
                        Telerik.Reporting.TextBox s_202_2 = report21.Items.Find("textBox46", true)[0] as Telerik.Reporting.TextBox;
                        s_202_2.Value = Utils.decToStr(item.s_202_2);
                        Telerik.Reporting.TextBox s_202_3 = report21.Items.Find("textBox47", true)[0] as Telerik.Reporting.TextBox;
                        s_202_3.Value = Utils.decToStr(item.s_202_3);

                        Telerik.Reporting.TextBox s_203_0 = report21.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox;
                        s_203_0.Value = Utils.decToStr(item.s_203_0);
                        Telerik.Reporting.TextBox s_203_1 = report21.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
                        s_203_1.Value = Utils.decToStr(item.s_203_1);
                        Telerik.Reporting.TextBox s_203_2 = report21.Items.Find("textBox34", true)[0] as Telerik.Reporting.TextBox;
                        s_203_2.Value = Utils.decToStr(item.s_203_2);
                        Telerik.Reporting.TextBox s_203_3 = report21.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox;
                        s_203_3.Value = Utils.decToStr(item.s_203_3);

                        Telerik.Reporting.TextBox s_204_0 = report21.Items.Find("textBox51", true)[0] as Telerik.Reporting.TextBox;
                        s_204_0.Value = Utils.decToStr(item.s_204_0);
                        Telerik.Reporting.TextBox s_204_1 = report21.Items.Find("textBox52", true)[0] as Telerik.Reporting.TextBox;
                        s_204_1.Value = Utils.decToStr(item.s_204_1);
                        Telerik.Reporting.TextBox s_204_2 = report21.Items.Find("textBox53", true)[0] as Telerik.Reporting.TextBox;
                        s_204_2.Value = Utils.decToStr(item.s_204_2);
                        Telerik.Reporting.TextBox s_204_3 = report21.Items.Find("textBox54", true)[0] as Telerik.Reporting.TextBox;
                        s_204_3.Value = Utils.decToStr(item.s_204_3);

                        Telerik.Reporting.TextBox s_205_0 = report21.Items.Find("textBox59", true)[0] as Telerik.Reporting.TextBox;
                        s_205_0.Value = Utils.decToStr(item.s_205_0);
                        Telerik.Reporting.TextBox s_205_1 = report21.Items.Find("textBox60", true)[0] as Telerik.Reporting.TextBox;
                        s_205_1.Value = Utils.decToStr(item.s_205_1);
                        Telerik.Reporting.TextBox s_205_2 = report21.Items.Find("textBox61", true)[0] as Telerik.Reporting.TextBox;
                        s_205_2.Value = Utils.decToStr(item.s_205_2);
                        Telerik.Reporting.TextBox s_205_3 = report21.Items.Find("textBox62", true)[0] as Telerik.Reporting.TextBox;
                        s_205_3.Value = Utils.decToStr(item.s_205_3);

                        Telerik.Reporting.TextBox s_206_0 = report21.Items.Find("textBox66", true)[0] as Telerik.Reporting.TextBox;
                        s_206_0.Value = Utils.decToStr(item.s_206_0);
                        Telerik.Reporting.TextBox s_206_1 = report21.Items.Find("textBox67", true)[0] as Telerik.Reporting.TextBox;
                        s_206_1.Value = Utils.decToStr(item.s_206_1);
                        Telerik.Reporting.TextBox s_206_2 = report21.Items.Find("textBox68", true)[0] as Telerik.Reporting.TextBox;
                        s_206_2.Value = Utils.decToStr(item.s_206_2);
                        Telerik.Reporting.TextBox s_206_3 = report21.Items.Find("textBox69", true)[0] as Telerik.Reporting.TextBox;
                        s_206_3.Value = Utils.decToStr(item.s_206_3);

                        Telerik.Reporting.TextBox s_207_0 = report21.Items.Find("textBox73", true)[0] as Telerik.Reporting.TextBox;
                        s_207_0.Value = item.s_207_0.ToString();
                        Telerik.Reporting.TextBox s_207_1 = report21.Items.Find("textBox74", true)[0] as Telerik.Reporting.TextBox;
                        s_207_1.Value = item.s_207_1.ToString();
                        Telerik.Reporting.TextBox s_207_2 = report21.Items.Find("textBox75", true)[0] as Telerik.Reporting.TextBox;
                        s_207_2.Value = item.s_207_2.ToString();
                        Telerik.Reporting.TextBox s_207_3 = report21.Items.Find("textBox76", true)[0] as Telerik.Reporting.TextBox;
                        s_207_3.Value = item.s_207_3.ToString();

                        Telerik.Reporting.TextBox s_208_0 = report21.Items.Find("textBox71", true)[0] as Telerik.Reporting.TextBox;
                        s_208_0.Value = item.s_208_0.ToString();
                        Telerik.Reporting.TextBox s_208_1 = report21.Items.Find("textBox77", true)[0] as Telerik.Reporting.TextBox;
                        s_208_1.Value = item.s_208_1.ToString();
                        Telerik.Reporting.TextBox s_208_2 = report21.Items.Find("textBox78", true)[0] as Telerik.Reporting.TextBox;
                        s_208_2.Value = item.s_208_2.ToString();
                        Telerik.Reporting.TextBox s_208_3 = report21.Items.Find("textBox79", true)[0] as Telerik.Reporting.TextBox;
                        s_208_3.Value = item.s_208_3.ToString();

                        Telerik.Reporting.TextBox s_210_0 = report21.Items.Find("textBox89", true)[0] as Telerik.Reporting.TextBox;
                        s_210_0.Value = Utils.decToStr(item.s_210_0);
                        Telerik.Reporting.TextBox s_210_1 = report21.Items.Find("textBox90", true)[0] as Telerik.Reporting.TextBox;
                        s_210_1.Value = Utils.decToStr(item.s_210_1);
                        Telerik.Reporting.TextBox s_210_2 = report21.Items.Find("textBox91", true)[0] as Telerik.Reporting.TextBox;
                        s_210_2.Value = Utils.decToStr(item.s_210_2);
                        Telerik.Reporting.TextBox s_210_3 = report21.Items.Find("textBox92", true)[0] as Telerik.Reporting.TextBox;
                        s_210_3.Value = Utils.decToStr(item.s_210_3);

                        Telerik.Reporting.TextBox s_211_0 = report21.Items.Find("textBox96", true)[0] as Telerik.Reporting.TextBox;
                        s_211_0.Value = Utils.decToStr(item.s_211_0);
                        Telerik.Reporting.TextBox s_211_1 = report21.Items.Find("textBox97", true)[0] as Telerik.Reporting.TextBox;
                        s_211_1.Value = Utils.decToStr(item.s_211_1);
                        Telerik.Reporting.TextBox s_211_2 = report21.Items.Find("textBox98", true)[0] as Telerik.Reporting.TextBox;
                        s_211_2.Value = Utils.decToStr(item.s_211_2);
                        Telerik.Reporting.TextBox s_211_3 = report21.Items.Find("textBox99", true)[0] as Telerik.Reporting.TextBox;
                        s_211_3.Value = Utils.decToStr(item.s_211_3);

                        Telerik.Reporting.TextBox s_212_0 = report21.Items.Find("textBox103", true)[0] as Telerik.Reporting.TextBox;
                        s_212_0.Value = Utils.decToStr(item.s_212_0);
                        Telerik.Reporting.TextBox s_212_1 = report21.Items.Find("textBox104", true)[0] as Telerik.Reporting.TextBox;
                        s_212_1.Value = Utils.decToStr(item.s_212_1);
                        Telerik.Reporting.TextBox s_212_2 = report21.Items.Find("textBox105", true)[0] as Telerik.Reporting.TextBox;
                        s_212_2.Value = Utils.decToStr(item.s_212_2);
                        Telerik.Reporting.TextBox s_212_3 = report21.Items.Find("textBox106", true)[0] as Telerik.Reporting.TextBox;
                        s_212_3.Value = Utils.decToStr(item.s_212_3);


                        Telerik.Reporting.TextBox s_214_0 = report21.Items.Find("textBox118", true)[0] as Telerik.Reporting.TextBox;
                        s_214_0.Value = Utils.decToStr(item.s_213_0);
                        Telerik.Reporting.TextBox s_214_1 = report21.Items.Find("textBox119", true)[0] as Telerik.Reporting.TextBox;
                        s_214_1.Value = Utils.decToStr(item.s_213_1);
                        Telerik.Reporting.TextBox s_214_2 = report21.Items.Find("textBox121", true)[0] as Telerik.Reporting.TextBox;
                        s_214_2.Value = Utils.decToStr(item.s_213_2);
                        Telerik.Reporting.TextBox s_214_3 = report21.Items.Find("textBox122", true)[0] as Telerik.Reporting.TextBox;
                        s_214_3.Value = Utils.decToStr(item.s_213_3);

                        Telerik.Reporting.TextBox s_215_0 = report21.Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
                        s_215_0.Value = Utils.decToStr(item.s_214_0);
                        Telerik.Reporting.TextBox s_215_1 = report21.Items.Find("textBox85", true)[0] as Telerik.Reporting.TextBox;
                        s_215_1.Value = Utils.decToStr(item.s_214_1);
                        Telerik.Reporting.TextBox s_215_2 = report21.Items.Find("textBox86", true)[0] as Telerik.Reporting.TextBox;
                        s_215_2.Value = Utils.decToStr(item.s_214_2);
                        Telerik.Reporting.TextBox s_215_3 = report21.Items.Find("textBox87", true)[0] as Telerik.Reporting.TextBox;
                        s_215_3.Value = Utils.decToStr(item.s_214_3);

                        Telerik.Reporting.TextBox s_215i_0 = report21.Items.Find("textBox108", true)[0] as Telerik.Reporting.TextBox;
                        s_215i_0.Value = item.s_215i_0.ToString();
                        Telerik.Reporting.TextBox s_215i_1 = report21.Items.Find("textBox109", true)[0] as Telerik.Reporting.TextBox;
                        s_215i_1.Value = item.s_215i_1.ToString();
                        Telerik.Reporting.TextBox s_215i_2 = report21.Items.Find("textBox110", true)[0] as Telerik.Reporting.TextBox;
                        s_215i_2.Value = item.s_215i_2.ToString();
                        Telerik.Reporting.TextBox s_215i_3 = report21.Items.Find("textBox111", true)[0] as Telerik.Reporting.TextBox;
                        s_215i_3.Value = item.s_215i_3.ToString();

                        RSW2014Book.Reports.Add(report21);
                        //                       pageCnt++;

                    }


                    pageCnt++;
                }

                //Раздел 2.2
                bool printThis = (((RSWdata.ExistPart_2_2.HasValue && RSWdata.ExistPart_2_2.Value == 1) || (RSWdata.ExistPart_2_3.HasValue && RSWdata.ExistPart_2_3.Value == 1)) || (Options.printAllPagesRSV1));

                if (printThis)
                {
                    RSW2014_Re22 report22 = new RSW2014_Re22();
                    (report22.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                    if (yearType == 2015)
                    {
                        Telerik.Reporting.TextBox textBox96 = report22.Items.Find("textBox96", true)[0] as Telerik.Reporting.TextBox;
                        textBox96.Value = " * В отношении выплат и иных вознаграждений в пользу физических лиц, занятых на соответствующих видах работ, указанных в пункте 1 части 1 статьи 30 Федерального закона от 28 декабря 2013 года  № 400-ФЗ \"О страховых пенсиях\".";
                        Telerik.Reporting.TextBox textBox95 = report22.Items.Find("textBox95", true)[0] as Telerik.Reporting.TextBox;
                        textBox95.Value = " ** В отношении выплат и иных вознаграждений в пользу физических лиц, занятых на соответствующих видах работ, указанных в пунктах 2 - 18 части 1 статьи 30 Федерального закона от 28 декабря 2013 года  № 400-ФЗ \"О страховых пенсиях\".";
                    }

                    Telerik.Reporting.TextBox RegNum22 = report22.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                    RegNum22.Value = regNum;
                    Telerik.Reporting.TextBox s_220_0 = report22.Items.Find("textBox21", true)[0] as Telerik.Reporting.TextBox;
                    s_220_0.Value = Utils.decToStr(RSWdata.s_220_0);
                    Telerik.Reporting.TextBox s_220_1 = report22.Items.Find("textBox22", true)[0] as Telerik.Reporting.TextBox;
                    s_220_1.Value = Utils.decToStr(RSWdata.s_220_1);
                    Telerik.Reporting.TextBox s_220_2 = report22.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox;
                    s_220_2.Value = Utils.decToStr(RSWdata.s_220_2);
                    Telerik.Reporting.TextBox s_220_3 = report22.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                    s_220_3.Value = Utils.decToStr(RSWdata.s_220_3);

                    Telerik.Reporting.TextBox s_221_0 = report22.Items.Find("textBox37", true)[0] as Telerik.Reporting.TextBox;
                    s_221_0.Value = Utils.decToStr(RSWdata.s_221_0);
                    Telerik.Reporting.TextBox s_221_1 = report22.Items.Find("textBox38", true)[0] as Telerik.Reporting.TextBox;
                    s_221_1.Value = Utils.decToStr(RSWdata.s_221_1);
                    Telerik.Reporting.TextBox s_221_2 = report22.Items.Find("textBox39", true)[0] as Telerik.Reporting.TextBox;
                    s_221_2.Value = Utils.decToStr(RSWdata.s_221_2);
                    Telerik.Reporting.TextBox s_221_3 = report22.Items.Find("textBox40", true)[0] as Telerik.Reporting.TextBox;
                    s_221_3.Value = Utils.decToStr(RSWdata.s_221_3);

                    Telerik.Reporting.TextBox s_223_0 = report22.Items.Find("textBox44", true)[0] as Telerik.Reporting.TextBox;
                    s_223_0.Value = Utils.decToStr(RSWdata.s_223_0);
                    Telerik.Reporting.TextBox s_223_1 = report22.Items.Find("textBox45", true)[0] as Telerik.Reporting.TextBox;
                    s_223_1.Value = Utils.decToStr(RSWdata.s_223_1);
                    Telerik.Reporting.TextBox s_223_2 = report22.Items.Find("textBox46", true)[0] as Telerik.Reporting.TextBox;
                    s_223_2.Value = Utils.decToStr(RSWdata.s_223_2);
                    Telerik.Reporting.TextBox s_223_3 = report22.Items.Find("textBox47", true)[0] as Telerik.Reporting.TextBox;
                    s_223_3.Value = Utils.decToStr(RSWdata.s_223_3);

                    Telerik.Reporting.TextBox s_224_0 = report22.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox;
                    s_224_0.Value = Utils.decToStr(RSWdata.s_224_0);
                    Telerik.Reporting.TextBox s_224_1 = report22.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
                    s_224_1.Value = Utils.decToStr(RSWdata.s_224_1);
                    Telerik.Reporting.TextBox s_224_2 = report22.Items.Find("textBox34", true)[0] as Telerik.Reporting.TextBox;
                    s_224_2.Value = Utils.decToStr(RSWdata.s_224_2);
                    Telerik.Reporting.TextBox s_224_3 = report22.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox;
                    s_224_3.Value = Utils.decToStr(RSWdata.s_224_3);

                    Telerik.Reporting.TextBox s_225_0 = report22.Items.Find("textBox51", true)[0] as Telerik.Reporting.TextBox;
                    s_225_0.Value = RSWdata.s_225_0.ToString();
                    Telerik.Reporting.TextBox s_225_1 = report22.Items.Find("textBox52", true)[0] as Telerik.Reporting.TextBox;
                    s_225_1.Value = RSWdata.s_225_1.ToString();
                    Telerik.Reporting.TextBox s_225_2 = report22.Items.Find("textBox53", true)[0] as Telerik.Reporting.TextBox;
                    s_225_2.Value = RSWdata.s_225_2.ToString();
                    Telerik.Reporting.TextBox s_225_3 = report22.Items.Find("textBox54", true)[0] as Telerik.Reporting.TextBox;
                    s_225_3.Value = RSWdata.s_225_3.ToString();

                    Telerik.Reporting.TextBox s_230_0 = report22.Items.Find("textBox61", true)[0] as Telerik.Reporting.TextBox;
                    s_230_0.Value = Utils.decToStr(RSWdata.s_230_0);
                    Telerik.Reporting.TextBox s_230_1 = report22.Items.Find("textBox62", true)[0] as Telerik.Reporting.TextBox;
                    s_230_1.Value = Utils.decToStr(RSWdata.s_230_1);
                    Telerik.Reporting.TextBox s_230_2 = report22.Items.Find("textBox63", true)[0] as Telerik.Reporting.TextBox;
                    s_230_2.Value = Utils.decToStr(RSWdata.s_230_2);
                    Telerik.Reporting.TextBox s_230_3 = report22.Items.Find("textBox64", true)[0] as Telerik.Reporting.TextBox;
                    s_230_3.Value = Utils.decToStr(RSWdata.s_230_3);

                    Telerik.Reporting.TextBox s_231_0 = report22.Items.Find("textBox66", true)[0] as Telerik.Reporting.TextBox;
                    s_231_0.Value = Utils.decToStr(RSWdata.s_231_0);
                    Telerik.Reporting.TextBox s_231_1 = report22.Items.Find("textBox67", true)[0] as Telerik.Reporting.TextBox;
                    s_231_1.Value = Utils.decToStr(RSWdata.s_231_1);
                    Telerik.Reporting.TextBox s_231_2 = report22.Items.Find("textBox68", true)[0] as Telerik.Reporting.TextBox;
                    s_231_2.Value = Utils.decToStr(RSWdata.s_231_2);
                    Telerik.Reporting.TextBox s_231_3 = report22.Items.Find("textBox69", true)[0] as Telerik.Reporting.TextBox;
                    s_231_3.Value = Utils.decToStr(RSWdata.s_231_3);

                    Telerik.Reporting.TextBox s_233_0 = report22.Items.Find("textBox71", true)[0] as Telerik.Reporting.TextBox;
                    s_233_0.Value = Utils.decToStr(RSWdata.s_233_0);
                    Telerik.Reporting.TextBox s_233_1 = report22.Items.Find("textBox72", true)[0] as Telerik.Reporting.TextBox;
                    s_233_1.Value = Utils.decToStr(RSWdata.s_233_1);
                    Telerik.Reporting.TextBox s_233_2 = report22.Items.Find("textBox73", true)[0] as Telerik.Reporting.TextBox;
                    s_233_2.Value = Utils.decToStr(RSWdata.s_233_2);
                    Telerik.Reporting.TextBox s_233_3 = report22.Items.Find("textBox74", true)[0] as Telerik.Reporting.TextBox;
                    s_233_3.Value = Utils.decToStr(RSWdata.s_233_3);

                    Telerik.Reporting.TextBox s_234_0 = report22.Items.Find("textBox76", true)[0] as Telerik.Reporting.TextBox;
                    s_234_0.Value = Utils.decToStr(RSWdata.s_234_0);
                    Telerik.Reporting.TextBox s_234_1 = report22.Items.Find("textBox77", true)[0] as Telerik.Reporting.TextBox;
                    s_234_1.Value = Utils.decToStr(RSWdata.s_234_1);
                    Telerik.Reporting.TextBox s_234_2 = report22.Items.Find("textBox78", true)[0] as Telerik.Reporting.TextBox;
                    s_234_2.Value = Utils.decToStr(RSWdata.s_234_2);
                    Telerik.Reporting.TextBox s_234_3 = report22.Items.Find("textBox79", true)[0] as Telerik.Reporting.TextBox;
                    s_234_3.Value = Utils.decToStr(RSWdata.s_234_3);

                    Telerik.Reporting.TextBox s_235_0 = report22.Items.Find("textBox81", true)[0] as Telerik.Reporting.TextBox;
                    s_235_0.Value = RSWdata.s_235_0.ToString();
                    Telerik.Reporting.TextBox s_235_1 = report22.Items.Find("textBox82", true)[0] as Telerik.Reporting.TextBox;
                    s_235_1.Value = RSWdata.s_235_1.ToString();
                    Telerik.Reporting.TextBox s_235_2 = report22.Items.Find("textBox83", true)[0] as Telerik.Reporting.TextBox;
                    s_235_2.Value = RSWdata.s_235_2.ToString();
                    Telerik.Reporting.TextBox s_235_3 = report22.Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
                    s_235_3.Value = RSWdata.s_235_3.ToString();
                    RSW2014Book.Reports.Add(report22);
                    pageCnt++;
                }

                //Раздел 2.4

                if (!fromXMLflag)
                {

                    RSW_2_4_List = db.FormsRSW2014_1_Razd_2_4.Where(x => x.Year == RSWdata.Year && x.Quarter == RSWdata.Quarter
                        && x.InsurerID == RSWdata.InsurerID && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(y => y.ID).ToList();
                }

                printThis = ((RSW_2_4_List.Count() == 0) && (Options.printAllPagesRSV1));

                if ((RSW_2_4_List.Count() != 0) || printThis)
                {
                    if (RSW_2_4_List.Count == 0)
                    {
                        RSW_2_4_List.Add(new FormsRSW2014_1_Razd_2_4 { }.SetZeroValues());
                    }


                    //Каждый код основания на своей странице
                    foreach (var item in RSW_2_4_List)
                    {
                        RSW2014_Re24 report24 = new RSW2014_Re24();

                        if (printThis)
                        {
                            //Telerik.Reporting.TextBox FB0 = report24.Items.Find("textBox58", true)[0] as Telerik.Reporting.TextBox;
                            //FB0.Value = "";
                            //Telerik.Reporting.TextBox FB1 = report24.Items.Find("textBox55", true)[0] as Telerik.Reporting.TextBox;
                            //FB1.Value = "";
                            //Telerik.Reporting.TextBox FB2 = report24.Items.Find("textBox59", true)[0] as Telerik.Reporting.TextBox;
                            //FB2.Value = "";

                            (report24.Items.Find("textBox4", true)[0] as Telerik.Reporting.TextBox).Value = " ";
                        }
                        else
                        {
                            Telerik.Reporting.TextBox txt = report24.Items.Find("textBox4", true)[0] as Telerik.Reporting.TextBox;
                            txt.Value = item.CodeBase.ToString();
                            switch (item.FilledBase)
                            {
                                case 0:
                                    Telerik.Reporting.TextBox FB0 = report24.Items.Find("textBox58", true)[0] as Telerik.Reporting.TextBox;
                                    FB0.Value = "V";
                                    break;
                                case 1:
                                    Telerik.Reporting.TextBox FB1 = report24.Items.Find("textBox55", true)[0] as Telerik.Reporting.TextBox;
                                    FB1.Value = "V";
                                    break;
                                case 2:
                                    Telerik.Reporting.TextBox FB2 = report24.Items.Find("textBox59", true)[0] as Telerik.Reporting.TextBox;
                                    FB2.Value = "V";
                                    break;
                            }
                        }

                        (report24.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                        Telerik.Reporting.TextBox RegNum24 = report24.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                        RegNum24.Value = regNum;


                        Telerik.Reporting.TextBox s_240_0 = report24.Items.Find("textBox21", true)[0] as Telerik.Reporting.TextBox;
                        s_240_0.Value = Utils.decToStr(item.s_240_0);
                        Telerik.Reporting.TextBox s_240_1 = report24.Items.Find("textBox22", true)[0] as Telerik.Reporting.TextBox;
                        s_240_1.Value = Utils.decToStr(item.s_240_1);
                        Telerik.Reporting.TextBox s_240_2 = report24.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox;
                        s_240_2.Value = Utils.decToStr(item.s_240_2);
                        Telerik.Reporting.TextBox s_240_3 = report24.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                        s_240_3.Value = Utils.decToStr(item.s_240_3);

                        Telerik.Reporting.TextBox s_241_0 = report24.Items.Find("textBox37", true)[0] as Telerik.Reporting.TextBox;
                        s_241_0.Value = Utils.decToStr(item.s_241_0);
                        Telerik.Reporting.TextBox s_241_1 = report24.Items.Find("textBox38", true)[0] as Telerik.Reporting.TextBox;
                        s_241_1.Value = Utils.decToStr(item.s_241_1);
                        Telerik.Reporting.TextBox s_241_2 = report24.Items.Find("textBox39", true)[0] as Telerik.Reporting.TextBox;
                        s_241_2.Value = Utils.decToStr(item.s_241_2);
                        Telerik.Reporting.TextBox s_241_3 = report24.Items.Find("textBox40", true)[0] as Telerik.Reporting.TextBox;
                        s_241_3.Value = Utils.decToStr(item.s_241_3);

                        Telerik.Reporting.TextBox s_243_0 = report24.Items.Find("textBox44", true)[0] as Telerik.Reporting.TextBox;
                        s_243_0.Value = Utils.decToStr(item.s_243_0);
                        Telerik.Reporting.TextBox s_243_1 = report24.Items.Find("textBox45", true)[0] as Telerik.Reporting.TextBox;
                        s_243_1.Value = Utils.decToStr(item.s_243_1);
                        Telerik.Reporting.TextBox s_243_2 = report24.Items.Find("textBox46", true)[0] as Telerik.Reporting.TextBox;
                        s_243_2.Value = Utils.decToStr(item.s_243_2);
                        Telerik.Reporting.TextBox s_243_3 = report24.Items.Find("textBox47", true)[0] as Telerik.Reporting.TextBox;
                        s_243_3.Value = Utils.decToStr(item.s_243_3);

                        Telerik.Reporting.TextBox s_244_0 = report24.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox;
                        s_244_0.Value = Utils.decToStr(item.s_244_0);
                        Telerik.Reporting.TextBox s_244_1 = report24.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
                        s_244_1.Value = Utils.decToStr(item.s_244_1);
                        Telerik.Reporting.TextBox s_244_2 = report24.Items.Find("textBox34", true)[0] as Telerik.Reporting.TextBox;
                        s_244_2.Value = Utils.decToStr(item.s_244_2);
                        Telerik.Reporting.TextBox s_244_3 = report24.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox;
                        s_244_3.Value = Utils.decToStr(item.s_244_3);

                        Telerik.Reporting.TextBox s_245_0 = report24.Items.Find("textBox51", true)[0] as Telerik.Reporting.TextBox;
                        s_245_0.Value = item.s_245_0.ToString();
                        Telerik.Reporting.TextBox s_245_1 = report24.Items.Find("textBox52", true)[0] as Telerik.Reporting.TextBox;
                        s_245_1.Value = item.s_245_1.ToString();
                        Telerik.Reporting.TextBox s_245_2 = report24.Items.Find("textBox53", true)[0] as Telerik.Reporting.TextBox;
                        s_245_2.Value = item.s_245_2.ToString();
                        Telerik.Reporting.TextBox s_245_3 = report24.Items.Find("textBox54", true)[0] as Telerik.Reporting.TextBox;
                        s_245_3.Value = item.s_245_3.ToString();

                        Telerik.Reporting.TextBox s_246_0 = report24.Items.Find("textBox89", true)[0] as Telerik.Reporting.TextBox;
                        s_246_0.Value = Utils.decToStr(item.s_246_0);
                        Telerik.Reporting.TextBox s_246_1 = report24.Items.Find("textBox90", true)[0] as Telerik.Reporting.TextBox;
                        s_246_1.Value = Utils.decToStr(item.s_246_1);
                        Telerik.Reporting.TextBox s_246_2 = report24.Items.Find("textBox91", true)[0] as Telerik.Reporting.TextBox;
                        s_246_2.Value = Utils.decToStr(item.s_246_2);
                        Telerik.Reporting.TextBox s_246_3 = report24.Items.Find("textBox92", true)[0] as Telerik.Reporting.TextBox;
                        s_246_3.Value = Utils.decToStr(item.s_246_3);

                        Telerik.Reporting.TextBox s_247_0 = report24.Items.Find("textBox96", true)[0] as Telerik.Reporting.TextBox;
                        s_247_0.Value = Utils.decToStr(item.s_247_0);
                        Telerik.Reporting.TextBox s_247_1 = report24.Items.Find("textBox97", true)[0] as Telerik.Reporting.TextBox;
                        s_247_1.Value = Utils.decToStr(item.s_247_1);
                        Telerik.Reporting.TextBox s_247_2 = report24.Items.Find("textBox98", true)[0] as Telerik.Reporting.TextBox;
                        s_247_2.Value = Utils.decToStr(item.s_247_2);
                        Telerik.Reporting.TextBox s_247_3 = report24.Items.Find("textBox99", true)[0] as Telerik.Reporting.TextBox;
                        s_247_3.Value = Utils.decToStr(item.s_247_3);

                        Telerik.Reporting.TextBox s_249_0 = report24.Items.Find("textBox103", true)[0] as Telerik.Reporting.TextBox;
                        s_249_0.Value = Utils.decToStr(item.s_249_0);
                        Telerik.Reporting.TextBox s_249_1 = report24.Items.Find("textBox104", true)[0] as Telerik.Reporting.TextBox;
                        s_249_1.Value = Utils.decToStr(item.s_249_1);
                        Telerik.Reporting.TextBox s_249_2 = report24.Items.Find("textBox105", true)[0] as Telerik.Reporting.TextBox;
                        s_249_2.Value = Utils.decToStr(item.s_249_2);
                        Telerik.Reporting.TextBox s_249_3 = report24.Items.Find("textBox106", true)[0] as Telerik.Reporting.TextBox;
                        s_249_3.Value = Utils.decToStr(item.s_249_3);

                        Telerik.Reporting.TextBox s_250_0 = report24.Items.Find("textBox110", true)[0] as Telerik.Reporting.TextBox;
                        s_250_0.Value = Utils.decToStr(item.s_250_0);
                        Telerik.Reporting.TextBox s_250_1 = report24.Items.Find("textBox111", true)[0] as Telerik.Reporting.TextBox;
                        s_250_1.Value = Utils.decToStr(item.s_250_1);
                        Telerik.Reporting.TextBox s_250_2 = report24.Items.Find("textBox113", true)[0] as Telerik.Reporting.TextBox;
                        s_250_2.Value = Utils.decToStr(item.s_250_2);
                        Telerik.Reporting.TextBox s_250_3 = report24.Items.Find("textBox114", true)[0] as Telerik.Reporting.TextBox;
                        s_250_3.Value = Utils.decToStr(item.s_250_3);

                        Telerik.Reporting.TextBox s_251_0 = report24.Items.Find("textBox118", true)[0] as Telerik.Reporting.TextBox;
                        s_251_0.Value = item.s_251_0.ToString();
                        Telerik.Reporting.TextBox s_251_1 = report24.Items.Find("textBox119", true)[0] as Telerik.Reporting.TextBox;
                        s_251_1.Value = item.s_251_1.ToString();
                        Telerik.Reporting.TextBox s_251_2 = report24.Items.Find("textBox121", true)[0] as Telerik.Reporting.TextBox;
                        s_251_2.Value = item.s_251_2.ToString();
                        Telerik.Reporting.TextBox s_251_3 = report24.Items.Find("textBox122", true)[0] as Telerik.Reporting.TextBox;
                        s_251_3.Value = item.s_251_3.ToString();

                        Telerik.Reporting.TextBox s_252_0 = report24.Items.Find("textBox71", true)[0] as Telerik.Reporting.TextBox;
                        s_252_0.Value = Utils.decToStr(item.s_252_0);
                        Telerik.Reporting.TextBox s_252_1 = report24.Items.Find("textBox72", true)[0] as Telerik.Reporting.TextBox;
                        s_252_1.Value = Utils.decToStr(item.s_252_1);
                        Telerik.Reporting.TextBox s_252_2 = report24.Items.Find("textBox73", true)[0] as Telerik.Reporting.TextBox;
                        s_252_2.Value = Utils.decToStr(item.s_252_2);
                        Telerik.Reporting.TextBox s_252_3 = report24.Items.Find("textBox74", true)[0] as Telerik.Reporting.TextBox;
                        s_252_3.Value = Utils.decToStr(item.s_252_3);

                        Telerik.Reporting.TextBox s_253_0 = report24.Items.Find("textBox78", true)[0] as Telerik.Reporting.TextBox;
                        s_253_0.Value = Utils.decToStr(item.s_253_0);
                        Telerik.Reporting.TextBox s_253_1 = report24.Items.Find("textBox79", true)[0] as Telerik.Reporting.TextBox;
                        s_253_1.Value = Utils.decToStr(item.s_253_1);
                        Telerik.Reporting.TextBox s_253_2 = report24.Items.Find("textBox81", true)[0] as Telerik.Reporting.TextBox;
                        s_253_2.Value = Utils.decToStr(item.s_253_2);
                        Telerik.Reporting.TextBox s_253_3 = report24.Items.Find("textBox83", true)[0] as Telerik.Reporting.TextBox;
                        s_253_3.Value = Utils.decToStr(item.s_253_3);

                        Telerik.Reporting.TextBox s_255_0 = report24.Items.Find("textBox65", true)[0] as Telerik.Reporting.TextBox;
                        s_255_0.Value = Utils.decToStr(item.s_255_0);
                        Telerik.Reporting.TextBox s_255_1 = report24.Items.Find("textBox66", true)[0] as Telerik.Reporting.TextBox;
                        s_255_1.Value = Utils.decToStr(item.s_255_1);
                        Telerik.Reporting.TextBox s_255_2 = report24.Items.Find("textBox67", true)[0] as Telerik.Reporting.TextBox;
                        s_255_2.Value = Utils.decToStr(item.s_255_2);
                        Telerik.Reporting.TextBox s_255_3 = report24.Items.Find("textBox69", true)[0] as Telerik.Reporting.TextBox;
                        s_255_3.Value = Utils.decToStr(item.s_255_3);

                        Telerik.Reporting.TextBox s_256_0 = report24.Items.Find("textBox86", true)[0] as Telerik.Reporting.TextBox;
                        s_256_0.Value = Utils.decToStr(item.s_256_0);
                        Telerik.Reporting.TextBox s_256_1 = report24.Items.Find("textBox87", true)[0] as Telerik.Reporting.TextBox;
                        s_256_1.Value = Utils.decToStr(item.s_256_1);
                        Telerik.Reporting.TextBox s_256_2 = report24.Items.Find("textBox94", true)[0] as Telerik.Reporting.TextBox;
                        s_256_2.Value = Utils.decToStr(item.s_256_2);
                        Telerik.Reporting.TextBox s_256_3 = report24.Items.Find("textBox101", true)[0] as Telerik.Reporting.TextBox;
                        s_256_3.Value = Utils.decToStr(item.s_256_3);

                        Telerik.Reporting.TextBox s_257_0 = report24.Items.Find("textBox120", true)[0] as Telerik.Reporting.TextBox;
                        s_257_0.Value = item.s_257_0.ToString();
                        Telerik.Reporting.TextBox s_257_1 = report24.Items.Find("textBox123", true)[0] as Telerik.Reporting.TextBox;
                        s_257_1.Value = item.s_257_1.ToString();
                        Telerik.Reporting.TextBox s_257_2 = report24.Items.Find("textBox124", true)[0] as Telerik.Reporting.TextBox;
                        s_257_2.Value = item.s_257_2.ToString();
                        Telerik.Reporting.TextBox s_257_3 = report24.Items.Find("textBox125", true)[0] as Telerik.Reporting.TextBox;
                        s_257_3.Value = item.s_257_3.ToString();

                        Telerik.Reporting.TextBox s_258_0 = report24.Items.Find("textBox136", true)[0] as Telerik.Reporting.TextBox;
                        s_258_0.Value = Utils.decToStr(item.s_258_0);
                        Telerik.Reporting.TextBox s_258_1 = report24.Items.Find("textBox137", true)[0] as Telerik.Reporting.TextBox;
                        s_258_1.Value = Utils.decToStr(item.s_258_1);
                        Telerik.Reporting.TextBox s_258_2 = report24.Items.Find("textBox138", true)[0] as Telerik.Reporting.TextBox;
                        s_258_2.Value = Utils.decToStr(item.s_258_2);
                        Telerik.Reporting.TextBox s_258_3 = report24.Items.Find("textBox139", true)[0] as Telerik.Reporting.TextBox;
                        s_258_3.Value = Utils.decToStr(item.s_258_3);

                        Telerik.Reporting.TextBox s_259_0 = report24.Items.Find("textBox143", true)[0] as Telerik.Reporting.TextBox;
                        s_259_0.Value = Utils.decToStr(item.s_259_0);
                        Telerik.Reporting.TextBox s_259_1 = report24.Items.Find("textBox144", true)[0] as Telerik.Reporting.TextBox;
                        s_259_1.Value = Utils.decToStr(item.s_259_1);
                        Telerik.Reporting.TextBox s_259_2 = report24.Items.Find("textBox145", true)[0] as Telerik.Reporting.TextBox;
                        s_259_2.Value = Utils.decToStr(item.s_259_2);
                        Telerik.Reporting.TextBox s_259_3 = report24.Items.Find("textBox146", true)[0] as Telerik.Reporting.TextBox;
                        s_259_3.Value = Utils.decToStr(item.s_259_3);

                        Telerik.Reporting.TextBox s_261_0 = report24.Items.Find("textBox150", true)[0] as Telerik.Reporting.TextBox;
                        s_261_0.Value = Utils.decToStr(item.s_261_0);
                        Telerik.Reporting.TextBox s_261_1 = report24.Items.Find("textBox151", true)[0] as Telerik.Reporting.TextBox;
                        s_261_1.Value = Utils.decToStr(item.s_261_1);
                        Telerik.Reporting.TextBox s_261_2 = report24.Items.Find("textBox152", true)[0] as Telerik.Reporting.TextBox;
                        s_261_2.Value = Utils.decToStr(item.s_261_2);
                        Telerik.Reporting.TextBox s_261_3 = report24.Items.Find("textBox153", true)[0] as Telerik.Reporting.TextBox;
                        s_261_3.Value = Utils.decToStr(item.s_261_3);

                        Telerik.Reporting.TextBox s_262_0 = report24.Items.Find("textBox157", true)[0] as Telerik.Reporting.TextBox;
                        s_262_0.Value = Utils.decToStr(item.s_262_0);
                        Telerik.Reporting.TextBox s_262_1 = report24.Items.Find("textBox158", true)[0] as Telerik.Reporting.TextBox;
                        s_262_1.Value = Utils.decToStr(item.s_262_1);
                        Telerik.Reporting.TextBox s_262_2 = report24.Items.Find("textBox159", true)[0] as Telerik.Reporting.TextBox;
                        s_262_2.Value = Utils.decToStr(item.s_262_2);
                        Telerik.Reporting.TextBox s_262_3 = report24.Items.Find("textBox160", true)[0] as Telerik.Reporting.TextBox;
                        s_262_3.Value = Utils.decToStr(item.s_262_3);

                        Telerik.Reporting.TextBox s_263_0 = report24.Items.Find("textBox164", true)[0] as Telerik.Reporting.TextBox;
                        s_263_0.Value = item.s_263_0.ToString();
                        Telerik.Reporting.TextBox s_263_1 = report24.Items.Find("textBox165", true)[0] as Telerik.Reporting.TextBox;
                        s_263_1.Value = item.s_263_1.ToString();
                        Telerik.Reporting.TextBox s_263_2 = report24.Items.Find("textBox166", true)[0] as Telerik.Reporting.TextBox;
                        s_263_2.Value = item.s_263_2.ToString();
                        Telerik.Reporting.TextBox s_263_3 = report24.Items.Find("textBox167", true)[0] as Telerik.Reporting.TextBox;
                        s_263_3.Value = item.s_263_3.ToString();

                        Telerik.Reporting.TextBox s_264_0 = report24.Items.Find("textBox127", true)[0] as Telerik.Reporting.TextBox;
                        s_264_0.Value = Utils.decToStr(item.s_264_0);
                        Telerik.Reporting.TextBox s_264_1 = report24.Items.Find("textBox134", true)[0] as Telerik.Reporting.TextBox;
                        s_264_1.Value = Utils.decToStr(item.s_264_1);
                        Telerik.Reporting.TextBox s_264_2 = report24.Items.Find("textBox141", true)[0] as Telerik.Reporting.TextBox;
                        s_264_2.Value = Utils.decToStr(item.s_264_2);
                        Telerik.Reporting.TextBox s_264_3 = report24.Items.Find("textBox147", true)[0] as Telerik.Reporting.TextBox;
                        s_264_3.Value = Utils.decToStr(item.s_264_3);

                        Telerik.Reporting.TextBox s_265_0 = report24.Items.Find("textBox162", true)[0] as Telerik.Reporting.TextBox;
                        s_265_0.Value = Utils.decToStr(item.s_265_0);
                        Telerik.Reporting.TextBox s_265_1 = report24.Items.Find("textBox169", true)[0] as Telerik.Reporting.TextBox;
                        s_265_1.Value = Utils.decToStr(item.s_265_1);
                        Telerik.Reporting.TextBox s_265_2 = report24.Items.Find("textBox175", true)[0] as Telerik.Reporting.TextBox;
                        s_265_2.Value = Utils.decToStr(item.s_265_2);
                        Telerik.Reporting.TextBox s_265_3 = report24.Items.Find("textBox176", true)[0] as Telerik.Reporting.TextBox;
                        s_265_3.Value = Utils.decToStr(item.s_265_3);

                        Telerik.Reporting.TextBox s_267_0 = report24.Items.Find("textBox179", true)[0] as Telerik.Reporting.TextBox;
                        s_267_0.Value = Utils.decToStr(item.s_267_0);
                        Telerik.Reporting.TextBox s_267_1 = report24.Items.Find("textBox180", true)[0] as Telerik.Reporting.TextBox;
                        s_267_1.Value = Utils.decToStr(item.s_267_1);
                        Telerik.Reporting.TextBox s_267_2 = report24.Items.Find("textBox181", true)[0] as Telerik.Reporting.TextBox;
                        s_267_2.Value = Utils.decToStr(item.s_267_2);
                        Telerik.Reporting.TextBox s_267_3 = report24.Items.Find("textBox182", true)[0] as Telerik.Reporting.TextBox;
                        s_267_3.Value = Utils.decToStr(item.s_267_3);

                        Telerik.Reporting.TextBox s_268_0 = report24.Items.Find("textBox185", true)[0] as Telerik.Reporting.TextBox;
                        s_268_0.Value = Utils.decToStr(item.s_268_0);
                        Telerik.Reporting.TextBox s_268_1 = report24.Items.Find("textBox186", true)[0] as Telerik.Reporting.TextBox;
                        s_268_1.Value = Utils.decToStr(item.s_268_1);
                        Telerik.Reporting.TextBox s_268_2 = report24.Items.Find("textBox187", true)[0] as Telerik.Reporting.TextBox;
                        s_268_2.Value = Utils.decToStr(item.s_268_2);
                        Telerik.Reporting.TextBox s_268_3 = report24.Items.Find("textBox188", true)[0] as Telerik.Reporting.TextBox;
                        s_268_3.Value = Utils.decToStr(item.s_268_3);

                        Telerik.Reporting.TextBox s_269_0 = report24.Items.Find("textBox191", true)[0] as Telerik.Reporting.TextBox;
                        s_269_0.Value = item.s_269_0.ToString();
                        Telerik.Reporting.TextBox s_269_1 = report24.Items.Find("textBox192", true)[0] as Telerik.Reporting.TextBox;
                        s_269_1.Value = item.s_269_1.ToString();
                        Telerik.Reporting.TextBox s_269_2 = report24.Items.Find("textBox193", true)[0] as Telerik.Reporting.TextBox;
                        s_269_2.Value = item.s_269_2.ToString();
                        Telerik.Reporting.TextBox s_269_3 = report24.Items.Find("textBox194", true)[0] as Telerik.Reporting.TextBox;
                        s_269_3.Value = item.s_269_3.ToString();

                        RSW2014Book.Reports.Add(report24);
                        pageCnt++;
                    }
                }

                //Раздел 2.5
                if (!fromXMLflag)
                {

                    RSW_2_5_1_List = db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.Year == RSWdata.Year && x.Quarter == RSWdata.Quarter
                        && x.InsurerID == RSWdata.InsurerID && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(y => y.NumRec).ToList();
                    RSW_2_5_2_List = db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.Year == RSWdata.Year && x.Quarter == RSWdata.Quarter
                        && x.InsurerID == RSWdata.InsurerID && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(y => y.NumRec).ToList();
                }

                if (RSW_2_5_1_List.Count() > 0 || RSW_2_5_2_List.Count() > 0 || Options.printAllPagesRSV1)
                {

                    RSW2014_Re25 report25 = new RSW2014_Re25();
                    (report25.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                    Telerik.Reporting.TextBox RegNum25 = report25.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                    RegNum25.Value = regNum;

                    Telerik.Reporting.DetailSection detail = report25.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                    Telerik.Reporting.Table table1 = report25.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
                    TableGroup detailGrouptable1 = new TableGroup();


                    report25.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });
                    report25.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRuleSmaller });

                    if (RSW_2_5_1_List.Count() == 0)
                        RSW_2_5_1_List.Add(new FormsRSW2014_1_Razd_2_5_1 { }.SetZeroValues());

                    j = 1;
                    foreach (var item in RSW_2_5_1_List)
                    {

                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.NumRec.ToString()
                        };
                        table1.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Col_2)
                        };
                        table1.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Col_3)
                        };
                        table1.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.Col_4.ToString()
                        };
                        table1.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyleSmaller",
                            Value = item.Col_5 != null ? item.Col_5.ToString() : ""
                        };
                        table1.Body.SetCellContent(j, 4, textBox);
                        j++;
                    }

                    Telerik.Reporting.TextBox TotalBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = "ИТОГО"
                    };
                    table1.Body.SetCellContent(j, 0, TotalBox);
                    TotalBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_2_5_1_List.Sum(item => item.Col_2))
                    };
                    table1.Body.SetCellContent(j, 1, TotalBox);
                    TotalBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_2_5_1_List.Sum(item => item.Col_3))
                    };
                    table1.Body.SetCellContent(j, 2, TotalBox);
                    TotalBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = RSW_2_5_1_List.Sum(item => item.Col_4).ToString()
                    };
                    table1.Body.SetCellContent(j, 3, TotalBox);
                    TotalBox = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = "X"
                    };
                    table1.Body.SetCellContent(j, 4, TotalBox);
                    j++;

                    for (int i = 0; i < RSW_2_5_1_List.Count + 1; i++)
                    {
                        table1.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable1.ChildGroups.Add(new TableGroup());
                    }

                    table1.RowGroups.Add(detailGrouptable1);

                    Telerik.Reporting.Table table2 = report25.Items.Find("table2", true)[0] as Telerik.Reporting.Table;
                    TableGroup detailGrouptable2 = new TableGroup();

                    j = 1;

                    if (RSW_2_5_2_List.Count() == 0)
                        RSW_2_5_2_List.Add(new FormsRSW2014_1_Razd_2_5_2 { }.SetZeroValues());

                    foreach (var item in RSW_2_5_2_List)
                    {

                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.NumRec.ToString()
                        };
                        table2.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.Col_2_QuarterKorr.ToString()
                        };
                        table2.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.Col_3_YearKorr.ToString()
                        };
                        table2.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Col_4)
                        };
                        table2.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Col_5)
                        };
                        table2.Body.SetCellContent(j, 4, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Col_6)
                        };
                        table2.Body.SetCellContent(j, 5, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.Col_7.ToString()
                        };
                        table2.Body.SetCellContent(j, 6, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyleSmaller",
                            Value = item.Col_8 != null ? item.Col_8.ToString() : ""
                        };
                        table2.Body.SetCellContent(j, 7, textBox);
                        j++;
                    }

                    Telerik.Reporting.TextBox TotalBox2 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = "ИТОГО"
                    };
                    table2.Body.SetCellContent(j, 0, TotalBox2, 1, 3);
                    TotalBox2 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_2_5_2_List.Sum(item => item.Col_4))
                    };
                    table2.Body.SetCellContent(j, 3, TotalBox2);
                    TotalBox2 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_2_5_2_List.Sum(item => item.Col_5))
                    };
                    table2.Body.SetCellContent(j, 4, TotalBox2);
                    TotalBox2 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_2_5_2_List.Sum(item => item.Col_6))
                    };
                    table2.Body.SetCellContent(j, 5, TotalBox2);
                    TotalBox2 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = RSW_2_5_2_List.Sum(item => item.Col_7).ToString().ToString()
                    };
                    table2.Body.SetCellContent(j, 6, TotalBox2);
                    TotalBox2 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = "X"
                    };
                    table2.Body.SetCellContent(j, 7, TotalBox);
                    j++;

                    for (int i = 0; i < RSW_2_5_2_List.Count + 1; i++)
                    {
                        table2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable2.ChildGroups.Add(new TableGroup());
                    }

                    table2.RowGroups.Add(detailGrouptable2);
                    RSW2014Book.Reports.Add(report25);
                    pageCnt++;
                }


                //Раздел 3.1
                if ((yearType == 2014 || yearType == 2012) && ((RSWdata.ExistPart_3_1.HasValue && RSWdata.ExistPart_3_1.Value == 1) || (RSWdata.ExistPart_3_2.HasValue && RSWdata.ExistPart_3_2.Value == 1) || Options.printAllPagesRSV1))
                {
                    Report.RSW2014_Re31 report31 = new Report.RSW2014_Re31();
                    (report31.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                    Telerik.Reporting.TextBox RegNum31 = report31.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                    RegNum31.Value = regNum;
                    Telerik.Reporting.TextBox s_321_0 = report31.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
                    s_321_0.Value = RSWdata.s_321_0.HasValue ? RSWdata.s_321_0.ToString() : "0";
                    Telerik.Reporting.TextBox s_321_1 = report31.Items.Find("textBox34", true)[0] as Telerik.Reporting.TextBox;
                    s_321_1.Value = RSWdata.s_321_1.HasValue ? RSWdata.s_321_1.ToString() : "0";
                    Telerik.Reporting.TextBox s_321_2 = report31.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox;
                    s_321_2.Value = RSWdata.s_321_2.HasValue ? RSWdata.s_321_2.ToString() : "0";
                    Telerik.Reporting.TextBox s_321_3 = report31.Items.Find("textBox36", true)[0] as Telerik.Reporting.TextBox;
                    s_321_3.Value = RSWdata.s_321_3.HasValue ? RSWdata.s_321_3.ToString() : "0";
                    Telerik.Reporting.TextBox s_322_0 = report31.Items.Find("textBox27", true)[0] as Telerik.Reporting.TextBox;
                    s_322_0.Value = RSWdata.s_322_0.HasValue ? RSWdata.s_322_0.ToString() : "0";
                    Telerik.Reporting.TextBox s_322_1 = report31.Items.Find("textBox28", true)[0] as Telerik.Reporting.TextBox;
                    s_322_1.Value = RSWdata.s_322_1.HasValue ? RSWdata.s_322_1.ToString() : "0";
                    Telerik.Reporting.TextBox s_322_2 = report31.Items.Find("textBox29", true)[0] as Telerik.Reporting.TextBox;
                    s_322_2.Value = RSWdata.s_322_2.HasValue ? RSWdata.s_322_2.ToString() : "0";
                    Telerik.Reporting.TextBox s_322_3 = report31.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                    s_322_3.Value = RSWdata.s_322_3.HasValue ? RSWdata.s_322_3.ToString() : "0";
                    Telerik.Reporting.TextBox s_323_0 = report31.Items.Find("textBox18", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_321_0 != null & RSWdata.s_321_0 != 0)
                    {
                        s_323_0.Value = (((decimal)RSWdata.s_322_0 / (decimal)RSWdata.s_321_0) * 100).ToString("#.##");
                    }
                    else
                    {
                        s_323_0.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_323_1 = report31.Items.Find("textBox19", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_321_1 != null & RSWdata.s_321_1 != 0)
                    {
                        s_323_1.Value = ((decimal)RSWdata.s_322_1 / (decimal)RSWdata.s_321_1 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_323_1.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_323_2 = report31.Items.Find("textBox20", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_321_2 != null & RSWdata.s_321_2 != 0)
                    {
                        s_323_2.Value = ((decimal)RSWdata.s_322_2 / (decimal)RSWdata.s_321_2 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_323_2.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_323_3 = report31.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_321_3 != null & RSWdata.s_321_3 != 0)
                    {
                        s_323_3.Value = ((decimal)RSWdata.s_322_3 / (decimal)RSWdata.s_321_3 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_323_3.Value = Utils.decToStr(0);
                    }

                    //Раздел 3.2
                    Telerik.Reporting.TextBox s_331_0 = report31.Items.Find("textBox51", true)[0] as Telerik.Reporting.TextBox;
                    s_331_0.Value = RSWdata.s_331_0.HasValue ? RSWdata.s_331_0.ToString() : "0";
                    Telerik.Reporting.TextBox s_331_1 = report31.Items.Find("textBox52", true)[0] as Telerik.Reporting.TextBox;
                    s_331_1.Value = RSWdata.s_331_1.HasValue ? RSWdata.s_331_1.ToString() : "0";
                    Telerik.Reporting.TextBox s_331_2 = report31.Items.Find("textBox53", true)[0] as Telerik.Reporting.TextBox;
                    s_331_2.Value = RSWdata.s_331_2.HasValue ? RSWdata.s_331_2.ToString() : "0";
                    Telerik.Reporting.TextBox s_331_3 = report31.Items.Find("textBox57", true)[0] as Telerik.Reporting.TextBox;
                    s_331_3.Value = RSWdata.s_331_3.HasValue ? RSWdata.s_331_3.ToString() : "0";
                    Telerik.Reporting.TextBox s_332_0 = report31.Items.Find("textBox60", true)[0] as Telerik.Reporting.TextBox;
                    s_332_0.Value = RSWdata.s_332_0.HasValue ? RSWdata.s_332_0.ToString() : "0";
                    Telerik.Reporting.TextBox s_332_1 = report31.Items.Find("textBox61", true)[0] as Telerik.Reporting.TextBox;
                    s_332_1.Value = RSWdata.s_332_1.HasValue ? RSWdata.s_332_1.ToString() : "0";
                    Telerik.Reporting.TextBox s_332_2 = report31.Items.Find("textBox62", true)[0] as Telerik.Reporting.TextBox;
                    s_332_2.Value = RSWdata.s_332_2.HasValue ? RSWdata.s_332_2.ToString() : "0";
                    Telerik.Reporting.TextBox s_332_3 = report31.Items.Find("textBox63", true)[0] as Telerik.Reporting.TextBox;
                    s_332_3.Value = RSWdata.s_332_3.HasValue ? RSWdata.s_332_3.ToString() : "0";
                    Telerik.Reporting.TextBox s_333_0 = report31.Items.Find("textBox66", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_331_0 != null & RSWdata.s_331_0 != 0)
                    {
                        s_333_0.Value = (((decimal)RSWdata.s_332_0 / (decimal)RSWdata.s_331_0) * 100).ToString("#.##");
                    }
                    else
                    {
                        s_333_0.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_333_1 = report31.Items.Find("textBox67", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_331_1 != null & RSWdata.s_331_1 != 0)
                    {
                        s_333_1.Value = ((decimal)RSWdata.s_332_1 / (decimal)RSWdata.s_331_1 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_333_1.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_333_2 = report31.Items.Find("textBox68", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_331_2 != null & RSWdata.s_331_2 != 0)
                    {
                        s_333_2.Value = ((decimal)RSWdata.s_332_2 / (decimal)RSWdata.s_331_2 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_333_2.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_333_3 = report31.Items.Find("textBox69", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_331_3 != null & RSWdata.s_331_3 != 0)
                    {
                        s_333_3.Value = ((decimal)RSWdata.s_332_3 / (decimal)RSWdata.s_331_3 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_333_3.Value = Utils.decToStr(0);
                    }

                    Telerik.Reporting.TextBox s_334_0 = report31.Items.Find("textBox72", true)[0] as Telerik.Reporting.TextBox;
                    s_334_0.Value = Utils.decToStr(RSWdata.s_334_0);
                    Telerik.Reporting.TextBox s_334_1 = report31.Items.Find("textBox73", true)[0] as Telerik.Reporting.TextBox;
                    s_334_1.Value = Utils.decToStr(RSWdata.s_334_1);
                    Telerik.Reporting.TextBox s_334_2 = report31.Items.Find("textBox74", true)[0] as Telerik.Reporting.TextBox;
                    s_334_2.Value = Utils.decToStr(RSWdata.s_334_2);
                    Telerik.Reporting.TextBox s_334_3 = report31.Items.Find("textBox75", true)[0] as Telerik.Reporting.TextBox;
                    s_334_3.Value = Utils.decToStr(RSWdata.s_334_3);
                    Telerik.Reporting.TextBox s_335_0 = report31.Items.Find("textBox78", true)[0] as Telerik.Reporting.TextBox;
                    s_335_0.Value = Utils.decToStr(RSWdata.s_335_0);
                    Telerik.Reporting.TextBox s_335_1 = report31.Items.Find("textBox79", true)[0] as Telerik.Reporting.TextBox;
                    s_335_1.Value = Utils.decToStr(RSWdata.s_335_1);
                    Telerik.Reporting.TextBox s_335_2 = report31.Items.Find("textBox80", true)[0] as Telerik.Reporting.TextBox;
                    s_335_2.Value = Utils.decToStr(RSWdata.s_335_2);
                    Telerik.Reporting.TextBox s_335_3 = report31.Items.Find("textBox81", true)[0] as Telerik.Reporting.TextBox;
                    s_335_3.Value = Utils.decToStr(RSWdata.s_335_3);
                    Telerik.Reporting.TextBox s_336_0 = report31.Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_334_0 != null & RSWdata.s_334_0 != 0)
                    {
                        s_336_0.Value = (((decimal)RSWdata.s_335_0 / (decimal)RSWdata.s_334_0) * 100).ToString("#.##");
                    }
                    else
                    {
                        s_336_0.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_336_1 = report31.Items.Find("textBox85", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_334_1 != null & RSWdata.s_334_1 != 0)
                    {
                        s_336_1.Value = ((decimal)RSWdata.s_335_1 / (decimal)RSWdata.s_334_1 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_336_1.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_336_2 = report31.Items.Find("textBox86", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_334_2 != null & RSWdata.s_334_2 != 0)
                    {
                        s_336_2.Value = ((decimal)RSWdata.s_335_2 / (decimal)RSWdata.s_334_2 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_336_2.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_336_3 = report31.Items.Find("textBox87", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_334_3 != null & RSWdata.s_334_3 != 0)
                    {
                        s_336_3.Value = ((decimal)RSWdata.s_335_3 / (decimal)RSWdata.s_334_3 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_336_3.Value = Utils.decToStr(0);
                    }
                    RSW2014Book.Reports.Add(report31);
                    pageCnt++;
                }
                //Раздел 3.3
                if ((RSWdata.ExistPart_3_3.HasValue && RSWdata.ExistPart_3_3.Value == 1) || (RSWdata.ExistPart_3_4.HasValue && RSWdata.ExistPart_3_4.Value == 1) || Options.printAllPagesRSV1)
                {
                    Report.RSW2014_Re33 report33 = new Report.RSW2014_Re33();
                    (report33.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                    if (yearType == 2014 || yearType == 2012)
                    {
                        Telerik.Reporting.TextBox textBox56 = report33.Items.Find("textBox56", true)[0] as Telerik.Reporting.TextBox;
                        textBox56.Dispose();
                    }
                    else if (yearType == 2015)
                    {
                        Telerik.Reporting.TextBox textBox5 = report33.Items.Find("textBox5", true)[0] as Telerik.Reporting.TextBox;
                        textBox5.Value = textBox5.Value.ToString().Remove(2, 1).Insert(2, "1");
                        ((Telerik.Reporting.TextBox)report33.Items.Find("textBox38", true)[0]).Dispose();
                        ((Telerik.Reporting.TextBox)report33.Items.Find("textBox32", true)[0]).Dispose();
                        ((Telerik.Reporting.Table)report33.Items.Find("table3", true)[0]).Dispose();
                        ((Telerik.Reporting.Table)report33.Items.Find("table4", true)[0]).Dispose();

                    }

                    Telerik.Reporting.TextBox RegNum33 = report33.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                    RegNum33.Value = regNum;
                    Telerik.Reporting.TextBox s_341_0 = report33.Items.Find("textBox16", true)[0] as Telerik.Reporting.TextBox;
                    s_341_0.Value = Utils.decToStr(RSWdata.s_341_0);
                    Telerik.Reporting.TextBox s_341_1 = report33.Items.Find("textBox17", true)[0] as Telerik.Reporting.TextBox;
                    s_341_1.Value = Utils.decToStr(RSWdata.s_341_1);
                    Telerik.Reporting.TextBox s_342_0 = report33.Items.Find("textBox20", true)[0] as Telerik.Reporting.TextBox;
                    s_342_0.Value = Utils.decToStr(RSWdata.s_342_0);
                    Telerik.Reporting.TextBox s_342_1 = report33.Items.Find("textBox21", true)[0] as Telerik.Reporting.TextBox;
                    s_342_1.Value = Utils.decToStr(RSWdata.s_342_1);
                    Telerik.Reporting.TextBox s_343_0 = report33.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_341_0 != null & RSWdata.s_341_0 != 0 & RSWdata.s_342_0 != null)
                    {
                        s_343_0.Value = (((decimal)RSWdata.s_342_0 / (decimal)RSWdata.s_341_0) * 100).ToString("#.##");
                    }
                    else
                    {
                        s_343_0.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_343_1 = report33.Items.Find("textBox25", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_341_1 != null & RSWdata.s_341_1 != 0 & RSWdata.s_342_1 != null)
                    {
                        s_343_1.Value = ((decimal)RSWdata.s_342_1 / (decimal)RSWdata.s_341_1 * 100).ToString("#.##");
                    }
                    else
                    {
                        s_343_1.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_344_0 = report33.Items.Find("textBox28", true)[0] as Telerik.Reporting.TextBox;
                    s_344_0.Value = RSWdata.s_344_0 == null ? "0" : RSWdata.s_344_0.ToString();
                    Telerik.Reporting.TextBox s_344_1 = report33.Items.Find("textBox29", true)[0] as Telerik.Reporting.TextBox;
                    s_344_1.Value = RSWdata.s_344_1 == null ? "0" : RSWdata.s_344_1.ToString();
                    Telerik.Reporting.TextBox s_345_0 = report33.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox;
                    s_345_0.Value = RSWdata.s_345_0 == null ? "" : Convert.ToDateTime(RSWdata.s_345_0).ToString("dd/MM/yyyy");
                    Telerik.Reporting.TextBox s_345_1 = report33.Items.Find("textBox37", true)[0] as Telerik.Reporting.TextBox;
                    s_345_1.Value = RSWdata.s_345_1 == null ? "" : RSWdata.s_345_1.ToString();

                    //Раздел 3.4
                    if (yearType == 2014 || yearType == 2012)
                    {
                        if (!fromXMLflag)
                        {

                            RSW_3_4_List = db.FormsRSW2014_1_Razd_3_4.Where(x => x.Year == RSWdata.Year && x.Quarter == RSWdata.Quarter
                                && x.InsurerID == RSWdata.InsurerID && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(y => y.NumOrd).ToList();
                        }

                        Telerik.Reporting.DetailSection detail_Re33 = report33.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                        Telerik.Reporting.Table table3_Re33 = report33.Items.Find("table3", true)[0] as Telerik.Reporting.Table;
                        TableGroup detailGrouptable3_Re33 = new TableGroup();
                        report33.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

                        j = 1;

                        if (RSW_3_4_List.Count == 0)
                        {
                            RSW_3_4_List.Add(new FormsRSW2014_1_Razd_3_4 { }.SetZeroValues());
                        }

                        foreach (var item in RSW_3_4_List)
                        {
                            Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = item.NumOrd == null ? "" : item.NumOrd.ToString()
                            };
                            table3_Re33.Body.SetCellContent(j, 0, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = item.NameOKWED == null ? "" : item.NameOKWED.ToString()
                            };
                            table3_Re33.Body.SetCellContent(j, 1, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = Utils.decToStr(item.Income)
                            };
                            table3_Re33.Body.SetCellContent(j, 2, textBox);
                            textBox = new Telerik.Reporting.TextBox
                            {
                                StyleName = "TableStyle",
                                Value = item.RateIncome == null ? "" : item.RateIncome.ToString()
                            };
                            table3_Re33.Body.SetCellContent(j, 3, textBox);
                            j++;
                        }

                        Telerik.Reporting.TextBox TotalBox_Re33 = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "Итого по всем видам деятельности"
                        };
                        table3_Re33.Body.SetCellContent(j, 0, TotalBox_Re33, 1, 2);
                        TotalBox_Re33 = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(RSW_3_4_List.Sum(item => item.Income))
                        };
                        table3_Re33.Body.SetCellContent(j, 2, TotalBox_Re33);
                        TotalBox_Re33 = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "100%"
                        };
                        table3_Re33.Body.SetCellContent(j, 3, TotalBox_Re33);
                        j++;

                        for (int i = 0; i < RSW_3_4_List.Count + 1; i++)
                        {
                            table3_Re33.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                            detailGrouptable3_Re33.ChildGroups.Add(new TableGroup());
                        }
                        table3_Re33.RowGroups.Add(detailGrouptable3_Re33);

                        Telerik.Reporting.TextBox s_351_0 = report33.Items.Find("textBox49", true)[0] as Telerik.Reporting.TextBox;
                        s_351_0.Value = RSWdata.s_351_0 == null ? "" : Convert.ToDateTime(RSWdata.s_351_0).ToString("dd/MM/yyyy");
                        Telerik.Reporting.TextBox s_351_1 = report33.Items.Find("textBox50", true)[0] as Telerik.Reporting.TextBox;
                        s_351_1.Value = RSWdata.s_351_1 == null ? "" : RSWdata.s_351_1.ToString();
                    }

                    RSW2014Book.Reports.Add(report33);
                    pageCnt++;
                }

                if ((RSWdata.ExistPart_3_5.HasValue && RSWdata.ExistPart_3_5.Value == 1) || (RSWdata.ExistPart_3_6.HasValue && RSWdata.ExistPart_3_6.Value == 1) || Options.printAllPagesRSV1)
                {
                    Report.RSW2014_Re35 report35 = new Report.RSW2014_Re35();
                    (report35.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                    if (yearType == 2015)
                    {
                        ((Telerik.Reporting.TextBox)report35.Items.Find("textBox5", true)[0]).Value = ((Telerik.Reporting.TextBox)report35.Items.Find("textBox5", true)[0]).Value.ToString().Remove(2, 1).Insert(2, "2");
                        ((Telerik.Reporting.TextBox)report35.Items.Find("textBox20", true)[0]).Value = ((Telerik.Reporting.TextBox)report35.Items.Find("textBox20", true)[0]).Value.ToString().Remove(2, 1).Insert(2, "3");
                    }

                    Telerik.Reporting.TextBox RegNum35 = report35.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                    RegNum35.Value = regNum; ;
                    Telerik.Reporting.TextBox s_361_0 = report35.Items.Find("textBox13", true)[0] as Telerik.Reporting.TextBox;
                    s_361_0.Value = Utils.decToStr(RSWdata.s_361_0); ;
                    Telerik.Reporting.TextBox s_362_0 = report35.Items.Find("textBox16", true)[0] as Telerik.Reporting.TextBox;
                    s_362_0.Value = Utils.decToStr(RSWdata.s_362_0); ;
                    Telerik.Reporting.TextBox s_363_0 = report35.Items.Find("textBox19", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_361_0 != null & RSWdata.s_361_0 != 0 & RSWdata.s_362_0 != null)
                    {
                        s_363_0.Value = (((decimal)RSWdata.s_362_0 / (decimal)RSWdata.s_361_0) * 100).ToString("#.##");
                    }
                    else
                    {
                        s_363_0.Value = Utils.decToStr(0);
                    }

                    Telerik.Reporting.TextBox s_371_0 = report35.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox;
                    s_371_0.Value = Utils.decToStr(RSWdata.s_371_0); ;
                    Telerik.Reporting.TextBox s_371_1 = report35.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
                    s_371_1.Value = Utils.decToStr(RSWdata.s_371_1); ;
                    Telerik.Reporting.TextBox s_372_0 = report35.Items.Find("textBox36", true)[0] as Telerik.Reporting.TextBox;
                    s_372_0.Value = Utils.decToStr(RSWdata.s_372_0); ;
                    Telerik.Reporting.TextBox s_372_1 = report35.Items.Find("textBox37", true)[0] as Telerik.Reporting.TextBox;
                    s_372_1.Value = Utils.decToStr(RSWdata.s_372_1); ;
                    Telerik.Reporting.TextBox s_373_0 = report35.Items.Find("textBox40", true)[0] as Telerik.Reporting.TextBox;
                    s_373_0.Value = Utils.decToStr(RSWdata.s_373_0); ;
                    Telerik.Reporting.TextBox s_373_1 = report35.Items.Find("textBox41", true)[0] as Telerik.Reporting.TextBox;
                    s_373_1.Value = Utils.decToStr(RSWdata.s_373_1); ;
                    Telerik.Reporting.TextBox s_374_0 = report35.Items.Find("textBox44", true)[0] as Telerik.Reporting.TextBox;
                    s_374_0.Value = Utils.decToStr(RSWdata.s_374_0); ;
                    Telerik.Reporting.TextBox s_374_1 = report35.Items.Find("textBox45", true)[0] as Telerik.Reporting.TextBox;
                    s_374_1.Value = Utils.decToStr(RSWdata.s_374_1); ;
                    Telerik.Reporting.TextBox s_375_0 = report35.Items.Find("textBox48", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_371_0 != null & RSWdata.s_371_0 != 0 & (RSWdata.s_372_0 != null || RSWdata.s_373_0 != null || RSWdata.s_374_0 != null))
                    {
                        s_375_0.Value = ((((decimal)RSWdata.s_372_0 + (decimal)RSWdata.s_373_0 + (decimal)RSWdata.s_374_0) / (decimal)RSWdata.s_371_0) * 100).ToString("#.##");
                    }
                    else
                    {
                        s_375_0.Value = Utils.decToStr(0);
                    }
                    Telerik.Reporting.TextBox s_375_1 = report35.Items.Find("textBox49", true)[0] as Telerik.Reporting.TextBox;
                    if (RSWdata.s_371_1 != null & RSWdata.s_371_1 != 0 & (RSWdata.s_372_1 != null || RSWdata.s_373_1 != null || RSWdata.s_374_1 != null))
                    {
                        s_375_1.Value = ((((decimal)RSWdata.s_372_1 + (decimal)RSWdata.s_373_1 + (decimal)RSWdata.s_374_1) / (decimal)RSWdata.s_371_1) * 100).ToString("#.##");
                    }
                    else
                    {
                        s_375_1.Value = Utils.decToStr(0);
                    }
                    RSW2014Book.Reports.Add(report35);
                    pageCnt++;
                }

                //Раздел 4
                if (!fromXMLflag)
                {

                    RSW_4_List = db.FormsRSW2014_1_Razd_4.Where(x => x.Year == RSWdata.Year && x.Quarter == RSWdata.Quarter
                        && x.InsurerID == RSWdata.InsurerID && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(y => y.NumOrd).ToList();
                }

                if (RSW_4_List.Count() > 0 || Options.printAllPagesRSV1)
                {
                    Report.RSW2014_Re4 report4 = new Report.RSW2014_Re4();
                    (report4.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;


                    if (yearType == 2015)
                    {
                        Telerik.Reporting.TextBox textBox88 = report4.Items.Find("textBox88", true)[0] as Telerik.Reporting.TextBox;
                        textBox88.Value = " * 1 - в случае доначисления органом контроля за уплатой страховых взносов сумм страховых взносов по актам камеральных проверок, по которым в  отчетном периоде вступили в силу решения о привлечении (об отказе в привлечении) к ответственности плательщиков страховых взносов, а такжев случае, если органом контроля за уплатой страховых взносов выявлены излишне начисленные плательщиком страховых взносов суммы страховых взносов;\r\n" +
                            "   2 - в случае доначисления органом контроля за уплатой страховых взносов сумм страховых взносов по актам выездных проверок, по которым в отчетном периоде вступили в силу решения о привлечении (об отказе в привлечении) к ответственности плательщиков страховых взносов, а также в случае, если органом контроля за уплатой страховых взносов выявлены излишне начисленные плательщиком страховых взносов суммы страховых взносов;\r\n" +
                            "   3 - в случае если плательщиком страховых взносов самостоятельно доначислены страховые взносы в случае выявления факта неотражения или неполноты отражения сведений, а также ошибок, приводящих к занижению суммы страховых взносов, подлежащей уплате за предыдущие отчетные периоды в соответствии со статьей  7 Федерального закона от 24 июля 2009 г. № 212-ФЗ;\r\n" +
                            "   4 - в случае корректировки плательщиком страховых взносов базы для начисления страховых взносов предшествующих отчетных (расчетных) периодов, не признаваемой ошибкой.";

                        Telerik.Reporting.TextBox textBox4 = report4.Items.Find("textBox4", true)[0] as Telerik.Reporting.TextBox;
                        textBox4.Value = "Раздел 4.  Суммы перерасчета страховых взносов с начала расчетного периода";

                        Telerik.Reporting.TextBox textBox11 = report4.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                        textBox11.Value = "Период, за который производится перерасчет страховых взносов";
                        Telerik.Reporting.TextBox textBox34 = report4.Items.Find("textBox34", true)[0] as Telerik.Reporting.TextBox;
                        textBox34.Value = "на финансирование страховой пенсии";
                        Telerik.Reporting.TextBox textBox17 = report4.Items.Find("textBox17", true)[0] as Telerik.Reporting.TextBox;
                        textBox17.Value = "на финансирование накопительной пенсии";

                    }
                    Telerik.Reporting.TextBox RegNum4 = report4.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                    RegNum4.Value = regNum;

                    Telerik.Reporting.DetailSection detail_Re4 = report4.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                    Telerik.Reporting.Table table_Re4 = report4.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
                    TableGroup detailGrouptable_Re4 = new TableGroup();
                    report4.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

                    j = 1;

                    if (RSW_4_List.Count == 0)
                    {
                        RSW_4_List.Add(new FormsRSW2014_1_Razd_4 { }.SetZeroValues());
                    }

                    foreach (var item in RSW_4_List)
                    {
                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.NumOrd == null ? "" : item.NumOrd.ToString()
                        };
                        table_Re4.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.Base == null ? "" : item.Base.ToString()
                        };
                        table_Re4.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = (item.CodeBase == null || item.CodeBase.Value == 0) ? "" : item.CodeBase.ToString()
                        };
                        table_Re4.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.YearPer == null ? "" : item.YearPer.ToString()
                        };
                        table_Re4.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.MonthPer == null ? "" : item.MonthPer.ToString()
                        };
                        table_Re4.Body.SetCellContent(j, 4, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Strah2014)
                        };
                        table_Re4.Body.SetCellContent(j, 5, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.StrahMoreBase2014)
                        };
                        table_Re4.Body.SetCellContent(j, 6, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Strah2013)
                        };
                        table_Re4.Body.SetCellContent(j, 7, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.StrahMoreBase2013)
                        };
                        table_Re4.Body.SetCellContent(j, 8, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Nakop2013)
                        };
                        table_Re4.Body.SetCellContent(j, 9, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Dop1)
                        };
                        table_Re4.Body.SetCellContent(j, 10, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Dop2)
                        };
                        table_Re4.Body.SetCellContent(j, 11, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.Dop21)
                        };
                        table_Re4.Body.SetCellContent(j, 12, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.OMS)
                        };
                        table_Re4.Body.SetCellContent(j, 13, textBox);
                        j++;
                    }

                    string strItog = "Итого доначислено:";
                    if (yearType == 2015)
                    {
                        strItog = "Итого сумма перерасчета:";
                    }
                    Telerik.Reporting.TextBox TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = strItog
                    };
                    table_Re4.Body.SetCellContent(j, 0, TotalBox_Re4, 1, 5);
                    TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_4_List.Sum(item => item.Strah2014))
                    };
                    table_Re4.Body.SetCellContent(j, 5, TotalBox_Re4);
                    TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_4_List.Sum(item => item.StrahMoreBase2014))
                    };
                    table_Re4.Body.SetCellContent(j, 6, TotalBox_Re4);
                    TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_4_List.Sum(item => item.Strah2013))
                    };
                    table_Re4.Body.SetCellContent(j, 7, TotalBox_Re4);
                    TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_4_List.Sum(item => item.StrahMoreBase2013))
                    };
                    table_Re4.Body.SetCellContent(j, 8, TotalBox_Re4);
                    TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_4_List.Sum(item => item.Nakop2013))
                    };
                    table_Re4.Body.SetCellContent(j, 9, TotalBox_Re4);
                    TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_4_List.Sum(item => item.Dop1))
                    };
                    table_Re4.Body.SetCellContent(j, 10, TotalBox_Re4);
                    TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_4_List.Sum(item => item.Dop2))
                    };
                    table_Re4.Body.SetCellContent(j, 11, TotalBox_Re4);
                    TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_4_List.Sum(item => item.Dop21))
                    };
                    table_Re4.Body.SetCellContent(j, 12, TotalBox_Re4);
                    TotalBox_Re4 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_4_List.Sum(item => item.OMS))
                    };
                    table_Re4.Body.SetCellContent(j, 13, TotalBox_Re4);
                    j++;

                    for (int i = 0; i < RSW_4_List.Count + 1; i++)
                    {
                        table_Re4.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_Re4.ChildGroups.Add(new TableGroup());
                    }
                    table_Re4.RowGroups.Add(detailGrouptable_Re4);
                    RSW2014Book.Reports.Add(report4);
                    pageCnt++;
                }
                //Раздел 5
                if (!fromXMLflag)
                {

                    RSW_5_List = db.FormsRSW2014_1_Razd_5.Where(x => x.Year == RSWdata.Year && x.Quarter == RSWdata.Quarter
                        && x.InsurerID == RSWdata.InsurerID && x.CorrectionNum == RSWdata.CorrectionNum).OrderBy(y => y.NumOrd).ToList();
                }

                if (RSW_5_List.Count() > 0 || Options.printAllPagesRSV1)
                {
                    Report.RSW2014_Re5 report5 = new Report.RSW2014_Re5();
                    (report5.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = DateUnderwrite;

                    Telerik.Reporting.TextBox RegNum5 = report5.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                    RegNum5.Value = regNum;

                    Telerik.Reporting.DetailSection detail_Re5 = report5.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                    Telerik.Reporting.Table table_Re5 = report5.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
                    TableGroup detailGrouptable_Re5 = new TableGroup();
                    report5.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

                    j = 1;

                    if (RSW_5_List.Count == 0)
                    {
                        RSW_5_List.Add(new FormsRSW2014_1_Razd_5 { }.SetZeroValues());
                    }

                    foreach (var item in RSW_5_List)
                    {
                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.NumOrd == null ? "" : item.NumOrd.ToString()
                        };
                        table_Re5.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.FirstName == null ? "" : item.LastName.ToString() + " " + item.FirstName.ToString() + " " + item.MiddleName.ToString()
                        };
                        table_Re5.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.NumSpravka == null ? "" : item.NumSpravka.ToString() + " " + item.DateSpravka == null ? "" : Convert.ToDateTime(item.DateSpravka).ToString("dd/MM/yyyy")
                        };

                        table_Re5.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.NumSpravka1 == null ? "" : item.NumSpravka1.ToString() + " " + item.DateSpravka1 == null ? "" : Convert.ToDateTime(item.DateSpravka1).ToString("dd/MM/yyyy")
                        };
                        table_Re5.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.SumPay)
                        };
                        table_Re5.Body.SetCellContent(j, 4, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.SumPay_0)
                        };
                        table_Re5.Body.SetCellContent(j, 5, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.SumPay_1)
                        };
                        table_Re5.Body.SetCellContent(j, 6, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(item.SumPay_2)
                        };
                        table_Re5.Body.SetCellContent(j, 7, textBox);
                        j++;
                    }

                    Telerik.Reporting.TextBox TotalBox_Re5 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = "Итого выплат"
                    };
                    table_Re5.Body.SetCellContent(j, 0, TotalBox_Re5, 1, 4);
                    TotalBox_Re5 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_5_List.Sum(item => item.SumPay))
                    };
                    table_Re5.Body.SetCellContent(j, 4, TotalBox_Re5);
                    TotalBox_Re5 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_5_List.Sum(item => item.SumPay_0))
                    };
                    table_Re5.Body.SetCellContent(j, 5, TotalBox_Re5);
                    TotalBox_Re5 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_5_List.Sum(item => item.SumPay_1))
                    };
                    table_Re5.Body.SetCellContent(j, 6, TotalBox_Re5);
                    TotalBox_Re5 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_5_List.Sum(item => item.SumPay_2))
                    };
                    table_Re5.Body.SetCellContent(j, 7, TotalBox_Re5);

                    j++;

                    for (int i = 0; i < RSW_5_List.Count + 1; i++)
                    {
                        table_Re5.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                        detailGrouptable_Re5.ChildGroups.Add(new TableGroup());
                    }
                    table_Re5.RowGroups.Add(detailGrouptable_Re5);

                    Telerik.Reporting.TextBox s_501_0_0 = report5.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                    s_501_0_0.Value = RSWdata.s_501_0_0 == null ? "" : Convert.ToDateTime(RSWdata.s_501_0_0).ToString("dd/MM/yyyy");
                    Telerik.Reporting.TextBox s_501_0_1 = report5.Items.Find("textBox43", true)[0] as Telerik.Reporting.TextBox;
                    s_501_0_1.Value = RSWdata.s_501_0_1 == null ? "" : Convert.ToDateTime(RSWdata.s_501_0_1).ToString("dd/MM/yyyy");
                    Telerik.Reporting.TextBox s_501_0_2 = report5.Items.Find("textBox39", true)[0] as Telerik.Reporting.TextBox;
                    s_501_0_2.Value = RSWdata.s_501_0_2 == null ? "" : Convert.ToDateTime(RSWdata.s_501_0_2).ToString("dd/MM/yyyy");
                    Telerik.Reporting.TextBox s_501_0_3 = report5.Items.Find("textBox35", true)[0] as Telerik.Reporting.TextBox;
                    s_501_0_3.Value = RSWdata.s_501_0_3 == null ? "" : Convert.ToDateTime(RSWdata.s_501_0_3).ToString("dd/MM/yyyy");
                    Telerik.Reporting.TextBox s_501_1_0 = report5.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox;
                    s_501_1_0.Value = RSWdata.s_501_1_0 == null ? "" : RSWdata.s_501_1_0.ToString();
                    Telerik.Reporting.TextBox s_501_1_1 = report5.Items.Find("textBox44", true)[0] as Telerik.Reporting.TextBox;
                    s_501_1_1.Value = RSWdata.s_501_1_1 == null ? "" : RSWdata.s_501_1_1.ToString();
                    Telerik.Reporting.TextBox s_501_1_2 = report5.Items.Find("textBox40", true)[0] as Telerik.Reporting.TextBox;
                    s_501_1_2.Value = RSWdata.s_501_1_2 == null ? "" : RSWdata.s_501_1_2.ToString();
                    Telerik.Reporting.TextBox s_501_1_3 = report5.Items.Find("textBox36", true)[0] as Telerik.Reporting.TextBox;
                    s_501_1_3.Value = RSWdata.s_501_1_3 == null ? "" : RSWdata.s_501_1_3.ToString();

                    RSW2014Book.Reports.Add(report5);
                    pageCnt++;
                }
                #endregion

            }

            var TerrUsl_list = db.TerrUsl.ToList();
            var OsobUslTruda_list = db.OsobUslTruda.ToList();
            var KodVred_2_list = db.KodVred_2.ToList();
            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
            var UslDosrNazn_list = db.UslDosrNazn.ToList();
            var PlatCategory_list = db.PlatCategory.ToList();
            var SpecOcenkaUslTruda_list = db.SpecOcenkaUslTruda.ToList();

            if (wp == 1 || wp == 2)
            {
                //Раздел 6
                #region  Раздел 6


                if (RSW_6_1_List.Any(x => x.Staff == null))
                {
                    foreach (var item in RSW_6_1_List.Where(x => x.Staff == null))
                    {
                        item.Staff = db.Staff.First(x => x.ID == item.StaffID);
                    }
                }
                Report.RSW2014_Re6 report6 = new Report.RSW2014_Re6();
                //  RSW_6_1_List = RSW_6_1_List.OrderBy(x => x.Staff.LastName).ToList();

                List<Staff> staffList = RSW_6_1_List.Select(x => x.Staff).ToList();

                foreach (var item in RSW_6_1_List)
                {
                    Staff People = staffList.First(x => x.ID == item.StaffID);

                    report6 = new Report.RSW2014_Re6();
                    (report6.Items.Find("DateUnderwrite", true)[0] as Telerik.Reporting.TextBox).Value = item.DateFilling.ToShortDateString();

                    Telerik.Reporting.TextBox RegNum6 = report6.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                    RegNum6.Value = regNum;
                    Telerik.Reporting.TextBox LastName = report6.Items.Find("textBox14", true)[0] as Telerik.Reporting.TextBox;
                    LastName.Value = People.LastName.ToString();
                    Telerik.Reporting.TextBox FirstName = report6.Items.Find("textBox15", true)[0] as Telerik.Reporting.TextBox;
                    FirstName.Value = People.FirstName.ToString();
                    Telerik.Reporting.TextBox MiddleName = report6.Items.Find("textBox16", true)[0] as Telerik.Reporting.TextBox;
                    MiddleName.Value = People.MiddleName.ToString();
                    Telerik.Reporting.TextBox InsuranceNumber = report6.Items.Find("textBox17", true)[0] as Telerik.Reporting.TextBox;
                    InsuranceNumber.Value = !String.IsNullOrEmpty(People.InsuranceNumber) ? Utils.ParseSNILS(People.InsuranceNumber.ToString(), (People.ControlNumber.HasValue ? People.ControlNumber.Value : (short)0)) : "";

                    if (yearType == 2014 || yearType == 2012)
                    {
                        ((Telerik.Reporting.Table)report6.Items.Find("table13", true)[0]).Dispose();
                    }
                    else if (yearType == 2015)
                    {
                        if (People.Dismissed.HasValue && People.Dismissed.Value == 1)
                        {
                            ((Telerik.Reporting.TextBox)report6.Items.Find("textBox103", true)[0]).Value = "X";
                        }
                    }

                    Telerik.Reporting.TextBox Quarter = report6.Items.Find("textBox24", true)[0] as Telerik.Reporting.TextBox;
                    Quarter.Value = item.Quarter.ToString();
                    Telerik.Reporting.TextBox Year = report6.Items.Find("textBox25", true)[0] as Telerik.Reporting.TextBox;
                    Year.Value = item.Year.ToString();

                    switch (item.TypeInfoID)
                    {
                        case 1:
                            Telerik.Reporting.TextBox TI0 = report6.Items.Find("textBox26", true)[0] as Telerik.Reporting.TextBox;
                            TI0.Value = "V";
                            break;
                        case 2:
                            Telerik.Reporting.TextBox TI1 = report6.Items.Find("textBox30", true)[0] as Telerik.Reporting.TextBox;
                            TI1.Value = "V";
                            break;
                        case 3:
                            Telerik.Reporting.TextBox TI2 = report6.Items.Find("textBox27", true)[0] as Telerik.Reporting.TextBox;
                            TI2.Value = "V";
                            break;
                    }

                    Telerik.Reporting.TextBox YearKorr = report6.Items.Find("textBox34", true)[0] as Telerik.Reporting.TextBox;
                    YearKorr.Value = item.YearKorr.ToString();
                    Telerik.Reporting.TextBox QuarterKorr = report6.Items.Find("textBox33", true)[0] as Telerik.Reporting.TextBox;
                    QuarterKorr.Value = item.QuarterKorr.ToString();
                    Telerik.Reporting.TextBox RegNumKorr = report6.Items.Find("textBox40", true)[0] as Telerik.Reporting.TextBox;
                    RegNumKorr.Value = item.RegNumKorr == null ? "" : item.RegNumKorr.ToString();

                    //раздел 6.4

                    //if (!fromXMLflag)
                    //{

                    RSW_6_4_List = db.FormsRSW2014_1_Razd_6_4.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == item.ID).ToList(); 
                    //}
                    //else
                    //{
                    //    RSW_6_4_List = item.FormsRSW2014_1_Razd_6_4.ToList();
                    //}


                    Telerik.Reporting.DetailSection detail_Re6 = report6.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                    Telerik.Reporting.Table table_Re6 = report6.Items.Find("table8", true)[0] as Telerik.Reporting.Table;
                    TableGroup detailGrouptable_Re6 = new TableGroup();
                    ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
                    report6.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });
                    ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
                    j = 1;
                    int k = 0;
                    foreach (var rsw64 in RSW_6_4_List)
                    {
                        PlatCategory PlatCat = PlatCategory_list.FirstOrDefault(x => x.ID == rsw64.PlatCategoryID);
                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "Всего с начала расчетного периода, в том числе за последние три месяца отчетного периода:",
                            TextWrap = true,
                            Multiline = true,
                            Height = Telerik.Reporting.Drawing.Unit.Cm(1.2D)
                        };
                        table_Re6.Body.SetCellContent(j, 0, textBox);
                        table_Re6.Body.SetCellContent(j, 1, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "4" + k.ToString() + "0"
                        });

                        table_Re6.Body.SetCellContent(j, 2, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = PlatCat.Code == null ? "" : PlatCat.Code.ToString()
                        });
                        table_Re6.Body.SetCellContent(j, 3, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_0_0)
                        });
                        table_Re6.Body.SetCellContent(j, 4, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_0_1)
                        });
                        table_Re6.Body.SetCellContent(j, 5, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_0_2)
                        });
                        table_Re6.Body.SetCellContent(j, 6, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_0_3)
                        });
                        table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(1.4D)));
                        detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
                        j++;

                        table_Re6.Body.SetCellContent(j, 0, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "1 месяц"
                        });
                        table_Re6.Body.SetCellContent(j, 1, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "4" + k.ToString() + "1"
                        });
                        table_Re6.Body.SetCellContent(j, 2, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = PlatCat.Code == null ? "" : PlatCat.Code.ToString()
                        });
                        table_Re6.Body.SetCellContent(j, 3, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_1_0)
                        });
                        table_Re6.Body.SetCellContent(j, 4, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_1_1)
                        });
                        table_Re6.Body.SetCellContent(j, 5, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_1_2)
                        });
                        table_Re6.Body.SetCellContent(j, 6, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_1_3)
                        });

                        table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
                        j++;

                        table_Re6.Body.SetCellContent(j, 0, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "2 месяц"
                        });
                        table_Re6.Body.SetCellContent(j, 1, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "4" + k.ToString() + "2"
                        });
                        table_Re6.Body.SetCellContent(j, 2, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = PlatCat.Code == null ? "" : PlatCat.Code.ToString()
                        });
                        table_Re6.Body.SetCellContent(j, 3, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_2_0)
                        });
                        table_Re6.Body.SetCellContent(j, 4, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_2_1)
                        });
                        table_Re6.Body.SetCellContent(j, 5, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_2_2)
                        });
                        table_Re6.Body.SetCellContent(j, 6, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_2_3)
                        });

                        table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
                        j++;

                        table_Re6.Body.SetCellContent(j, 0, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "3 месяц"
                        });
                        table_Re6.Body.SetCellContent(j, 1, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "4" + k.ToString() + "3"
                        });
                        table_Re6.Body.SetCellContent(j, 2, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = PlatCat.Code == null ? "" : PlatCat.Code.ToString()
                        });
                        table_Re6.Body.SetCellContent(j, 3, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_3_0)
                        });
                        table_Re6.Body.SetCellContent(j, 4, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_3_1)
                        });
                        table_Re6.Body.SetCellContent(j, 5, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_3_2)
                        });
                        table_Re6.Body.SetCellContent(j, 6, new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw64.s_3_3)
                        });

                        table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
                        j++;
                        k++;
                    }
                    if (RSW_6_4_List.Count() > 0)
                        table_Re6.RowGroups.Add(detailGrouptable_Re6);


                    string rub = "0", kop = "00";
                    if (item.SumFeePFR.HasValue)
                    {
                        //rub = item.SumFeePFR.ToString().Split(',')[0].Substring(0, item.SumFeePFR.ToString().Split(',')[0].Length);
                        //if (item.SumFeePFR.ToString().Contains(","))
                        //    kop = item.SumFeePFR.ToString().Split(',')[1].Substring(0, item.SumFeePFR.ToString().Split(',')[1].Length);
                        rub = Decimal.Truncate(item.SumFeePFR.Value).ToString();
                        var t = item.SumFeePFR.Value.ToString();
                        if (t.Contains(",") || t.Contains("."))
                            kop = t.Substring(t.Length - 2, 2);

                        if (t.Contains(","))
                        {
                            var tmp = t.Split(',');
                            kop = tmp[1].PadRight(2, '0');
                        }
                        else if (t.Contains("."))
                        {
                            var tmp = t.Split('.');
                            kop = tmp[1].PadRight(2, '0');

                        }
                    }

                    (report6.Items.Find("textBox55", true)[0] as Telerik.Reporting.TextBox).Value = rub;
                    (report6.Items.Find("textBox57", true)[0] as Telerik.Reporting.TextBox).Value = kop;

                    //раздел 6.6
                    //if (!fromXMLflag)
                    //{
                    RSW_6_6_List = db.FormsRSW2014_1_Razd_6_6.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == item.ID).ToList();
                    //}
                    //else
                    //{
                    //    RSW_6_6_List = item.FormsRSW2014_1_Razd_6_6.ToList();
                    //}



                    if (yearType == 2015)
                    {
                        Telerik.Reporting.TextBox textBox70 = report6.Items.Find("textBox70", true)[0] as Telerik.Reporting.TextBox;
                        textBox70.Value = "на финансирование страховой пенсии";
                        Telerik.Reporting.TextBox textBox71 = report6.Items.Find("textBox71", true)[0] as Telerik.Reporting.TextBox;
                        textBox71.Value = "на финансирование накопительной пенсии";

                        //Раздел 6.7
                        Telerik.Reporting.TextBox textBox82 = report6.Items.Find("textBox82", true)[0] as Telerik.Reporting.TextBox;
                        textBox82.Value = "Сумма выплат и иных вознаграждений, начисленых в пользу физического лица, занятого на видах работ, указанных в пункте 1 части 1 статьи 30 Федерального закона от 28 декабря 2013 года № 400-ФЗ \"О страховых пенсиях\"";
                        Telerik.Reporting.TextBox textBox84 = report6.Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
                        textBox84.Value = "Сумма выплат и иных вознаграждений, начисленных в пользу физического лица, занятого на видах работ, указанных в пунктах 2 - 18 части 1 статьи 30 Федерального закона от 28 декабря 2013 года  № 400-ФЗ \"О страховых пенсиях\"";

                    }

                    Telerik.Reporting.DetailSection detail_Re66 = report6.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                    Telerik.Reporting.Table table_Re66 = report6.Items.Find("table10", true)[0] as Telerik.Reporting.Table;
                    TableGroup detailGrouptable_Re66 = new TableGroup();
                    //  report6.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

                    j = 1;
                    foreach (var rsw66 in RSW_6_6_List)
                    {
                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = rsw66.AccountPeriodQuarter.ToString()
                        };
                        table_Re66.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = rsw66.AccountPeriodYear.ToString()
                        };
                        table_Re66.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw66.SumFeePFR_D)
                        };
                        table_Re66.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw66.SumFeePFR_StrahD)
                        };
                        table_Re66.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw66.SumFeePFR_NakopD)
                        };
                        table_Re66.Body.SetCellContent(j, 4, textBox);
                        table_Re66.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_Re66.ChildGroups.Add(new TableGroup());
                        j++;
                    }

                    Telerik.Reporting.TextBox textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = "Итого"
                    };
                    table_Re66.Body.SetCellContent(j, 0, textBoxTotal66, 1, 2);
                    textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_6_6_List.Sum(y => y.SumFeePFR_D))
                    };
                    table_Re66.Body.SetCellContent(j, 2, textBoxTotal66);
                    textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_6_6_List.Sum(y => y.SumFeePFR_StrahD))
                    };
                    table_Re66.Body.SetCellContent(j, 3, textBoxTotal66);
                    textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(RSW_6_6_List.Sum(y => y.SumFeePFR_NakopD))
                    };
                    table_Re66.Body.SetCellContent(j, 4, textBoxTotal66);
                    table_Re66.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                    detailGrouptable_Re66.ChildGroups.Add(new TableGroup());

                    table_Re66.RowGroups.Add(detailGrouptable_Re66);

                    //раздел 6.7
                    //if (!fromXMLflag)
                    //{
                    RSW_6_7_List = db.FormsRSW2014_1_Razd_6_7.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == item.ID).ToList();
                    //}
                    //else
                    //{
                    //    RSW_6_7_List = item.FormsRSW2014_1_Razd_6_7.ToList();
                    //}

                    Telerik.Reporting.DetailSection detail_Re67 = report6.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                    Telerik.Reporting.Table table_Re67 = report6.Items.Find("table11", true)[0] as Telerik.Reporting.Table;
                    TableGroup detailGrouptable_Re67 = new TableGroup();
                    //                    report6.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

                    j = 1;
                    k = 0;
                    foreach (var rsw67 in RSW_6_7_List)
                    {
                        SpecOcenkaUslTruda SpecOcen = SpecOcenkaUslTruda_list.FirstOrDefault(x => x.ID == rsw67.SpecOcenkaUslTrudaID);
                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "Всего с начала расчетного периода, в том числе за последние три месяца отчетного периода:",
                            TextWrap = true,
                            Multiline = true,
                            Height = Telerik.Reporting.Drawing.Unit.Cm(1D)
                        };
                        table_Re67.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "7" + k.ToString() + "0"
                        };
                        table_Re67.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = SpecOcen == null ? "" : SpecOcen.Code == null ? "" : SpecOcen.Code.ToString()
                        };
                        table_Re67.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw67.s_0_0)
                        };
                        table_Re67.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw67.s_0_1)
                        };
                        table_Re67.Body.SetCellContent(j, 4, textBox);
                        table_Re67.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(1.4D)));
                        detailGrouptable_Re67.ChildGroups.Add(new TableGroup());
                        j++;

                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "1 месяц"
                        };
                        table_Re67.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "7" + k.ToString() + "1"
                        };
                        table_Re67.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = SpecOcen == null ? "" : (SpecOcen.Code == null || ((rsw67.s_1_0 == null || (rsw67.s_1_0 != null && rsw67.s_1_0 == 0)) && (rsw67.s_1_1 == null || (rsw67.s_1_1 != null && rsw67.s_1_1 == 0)))) ? "" : SpecOcen.Code.ToString()
                        };
                        table_Re67.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw67.s_1_0)
                        };
                        table_Re67.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw67.s_1_1)
                        };
                        table_Re67.Body.SetCellContent(j, 4, textBox);
                        table_Re67.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_Re67.ChildGroups.Add(new TableGroup());
                        j++;

                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "2 месяц"
                        };
                        table_Re67.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "7" + k.ToString() + "2"
                        };
                        table_Re67.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = SpecOcen == null ? "" : (SpecOcen.Code == null || ((rsw67.s_2_0 == null || (rsw67.s_2_0 != null && rsw67.s_2_0 == 0)) && (rsw67.s_2_1 == null || (rsw67.s_2_1 != null && rsw67.s_2_1 == 0)))) ? "" : SpecOcen.Code.ToString()
                        };
                        table_Re67.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw67.s_2_0)
                        };
                        table_Re67.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw67.s_2_1)
                        };
                        table_Re67.Body.SetCellContent(j, 4, textBox);
                        table_Re67.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_Re67.ChildGroups.Add(new TableGroup());
                        j++;

                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "3 месяц"
                        };
                        table_Re67.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = "7" + k.ToString() + "3"
                        };
                        table_Re67.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = SpecOcen == null ? "" : (SpecOcen.Code == null || ((rsw67.s_3_0 == null || (rsw67.s_3_0 != null && rsw67.s_3_0 == 0)) && (rsw67.s_3_1 == null || (rsw67.s_3_1 != null && rsw67.s_3_1 == 0)))) ? "" : SpecOcen.Code.ToString()
                        };
                        table_Re67.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw67.s_3_0)
                        };
                        table_Re67.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Utils.decToStr(rsw67.s_3_1)
                        };
                        table_Re67.Body.SetCellContent(j, 4, textBox);
                        table_Re67.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                        detailGrouptable_Re67.ChildGroups.Add(new TableGroup());
                        j++;
                        k++;
                    }

                    if (RSW_6_7_List.Count() > 0)
                        table_Re67.RowGroups.Add(detailGrouptable_Re67);

                    //раздел 6.8
                    ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
                    //if (!fromXMLflag)
                    //{
                    RSW_6_8_List = db.StajOsn.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == item.ID).ToList(); 
                    //}
                    //else
                    //{
                    //    RSW_6_8_List = item.StajOsn.ToList();
                    //}

                    Telerik.Reporting.DetailSection detail_Re68 = report6.Items.Find("detail", true)[0] as Telerik.Reporting.DetailSection;
                    Telerik.Reporting.Table table_Re68 = report6.Items.Find("table12", true)[0] as Telerik.Reporting.Table;
                    TableGroup detailGrouptable_Re68 = new TableGroup();

                    j = 1;

                    foreach (var rsw68 in RSW_6_8_List.OrderBy(x => x.Number.Value))
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

                        if (RSW_6_8_List.Count() > 1)
                        {
                            table_Re68.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                            detailGrouptable_Re68.ChildGroups.Add(new TableGroup());
                            j++;
                        }
                        else if (RSW_6_8_List.Count() == 1)
                        {
                            // && rsw68.StajLgot.Count <= 1
                            table_Re68.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                            detailGrouptable_Re68.ChildGroups.Add(new TableGroup());
                        }


                    }
                    if (RSW_6_8_List.Count() > 0)
                        table_Re68.RowGroups.Add(detailGrouptable_Re68);

                    RSW2014Book.Reports.Add(report6);
                    ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
                    pageCnt++;
                    pageCnt++;
                }

                staffList = null;
                #endregion

            }

            if (wp == 1 || wp == 2)
            {
                // Инд.сведения 2013 СЗВ-6-4
                #region
                //if (!fromXMLflag && SZV_6_4_data != null)
                //{
                //    SZV_6_4_List = db.FormsSZV_6_4.Where(x => x.ID == SZV_6_4_data.ID).OrderBy(x => x.Staff.LastName).ToList();
                //}

                if (SZV_6_4_List.Any(x => x.Staff == null))
                {
                    foreach (var item in SZV_6_4_List.Where(x => x.Staff == null))
                    {
                        item.Staff = db.Staff.First(x => x.ID == item.StaffID);
                    }
                }

                foreach (var item in SZV_6_4_List)
                {

                    PU.FormsSZV_6_4_2013.Report.SZV_6_4_Re1 SZV_6_4_Re1 = new PU.FormsSZV_6_4_2013.Report.SZV_6_4_Re1();
                    (SZV_6_4_Re1.Items.Find("RegNum", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
                    (SZV_6_4_Re1.Items.Find("NameShort", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(ins.NameShort) ? ins.NameShort : "";
                    (SZV_6_4_Re1.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN;
                    (SZV_6_4_Re1.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP;
                    (SZV_6_4_Re1.Items.Find("OKPO", true)[0] as Telerik.Reporting.TextBox).Value = ins.OKPO;

                    (SZV_6_4_Re1.Items.Find("PlatCat", true)[0] as Telerik.Reporting.TextBox).Value = item.PlatCategory != null ? item.PlatCategory.Code : "";


                    string tempName = "";
                    if (item.Year >= 2014)
                    {
                        switch (item.Quarter)
                        {
                            case 3:
                                tempName = "period1";
                                break;
                            case 6:
                                tempName = "period2";
                                break;
                            case 9:
                                tempName = "period3";
                                break;
                            case 0:
                                tempName = "period4";
                                break;
                        }
                    }
                    else
                    {
                        tempName = "period" + item.Quarter;
                    }

                    if (!String.IsNullOrEmpty(tempName))
                    {
                        (SZV_6_4_Re1.Items.Find(tempName, true)[0] as Telerik.Reporting.TextBox).Value = "V";
                    }
                    (SZV_6_4_Re1.Items.Find("periodYear", true)[0] as Telerik.Reporting.TextBox).Value = item.Year.ToString();


                    tempName = "";
                    switch (item.TypeInfoID)
                    {
                        case 1:
                            tempName = "typeInfoIshod";
                            break;
                        case 2:
                            tempName = "typeInfoKorr";
                            break;
                        case 3:
                            tempName = "typeInfoOtm";
                            break;
                    }
                    if (!String.IsNullOrEmpty(tempName))
                    {
                        (SZV_6_4_Re1.Items.Find(tempName, true)[0] as Telerik.Reporting.TextBox).Value = "V";
                    }

                    if (item.TypeInfoID >= 2)
                    {
                        (SZV_6_4_Re1.Items.Find("periodKorr" + item.QuarterKorr, true)[0] as Telerik.Reporting.TextBox).Value = "V";
                        (SZV_6_4_Re1.Items.Find("periodKorrYear", true)[0] as Telerik.Reporting.TextBox).Value = item.YearKorr.ToString();

                        (SZV_6_4_Re1.Items.Find("RegNumKorr", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(item.RegNumKorr) ? item.RegNumKorr : "";
                    }

                    switch (item.TypeContract)
                    {
                        case 1:
                            (SZV_6_4_Re1.Items.Find("TypeContractTrud", true)[0] as Telerik.Reporting.TextBox).Value = "V";
                            break;
                        case 2:
                            (SZV_6_4_Re1.Items.Find("TypeContractGrazhd", true)[0] as Telerik.Reporting.TextBox).Value = "V";
                            break;
                    }

                    (SZV_6_4_Re1.Items.Find("LastName", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(item.Staff.LastName) ? item.Staff.LastName : "";
                    (SZV_6_4_Re1.Items.Find("FirstName", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(item.Staff.FirstName) ? item.Staff.FirstName : "";
                    (SZV_6_4_Re1.Items.Find("MiddleName", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(item.Staff.MiddleName) ? item.Staff.MiddleName : "";
                    (SZV_6_4_Re1.Items.Find("SNILS", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? Utils.ParseSNILS(item.Staff.InsuranceNumber.ToString(), (item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value : (short)0)) : ""; ;

                    pageCnt++;
                    RSW2014Book.Reports.Add(SZV_6_4_Re1);


                    PU.FormsSZV_6_4_2013.Report.SZV_6_4_Re2 SZV_6_4_Re2 = new PU.FormsSZV_6_4_2013.Report.SZV_6_4_Re2();

                    for (int b = 0; b <= 3; b++)
                    {
                        for (int a = 0; a <= 2; a++)
                        {
                            string field = "s_" + b + "_" + a;
                            var properties = item.GetType().GetProperty(field);
                            object value = properties.GetValue(item, null);

                            (SZV_6_4_Re2.Items.Find(field, true)[0] as Telerik.Reporting.TextBox).Value = value != null ? Utils.decToStr(decimal.Parse(value.ToString())) : Utils.decToStr(0);
                        }
                    }

                    for (int b = 0; b <= 3; b++)
                    {
                        for (int a = 0; a <= 1; a++)
                        {
                            string field = "d_" + b + "_" + a;
                            var properties = item.GetType().GetProperty(field);
                            object value = properties.GetValue(item, null);

                            (SZV_6_4_Re2.Items.Find(field, true)[0] as Telerik.Reporting.TextBox).Value = value != null ? Utils.decToStr(decimal.Parse(value.ToString())) : Utils.decToStr(0);
                        }
                    }

                    (SZV_6_4_Re2.Items.Find("SumFeePFR_Strah", true)[0] as Telerik.Reporting.TextBox).Value = item.SumFeePFR_Strah.HasValue ? Utils.decToStr(item.SumFeePFR_Strah) : Utils.decToStr(0);
                    (SZV_6_4_Re2.Items.Find("SumPayPFR_Strah", true)[0] as Telerik.Reporting.TextBox).Value = item.SumPayPFR_Strah.HasValue ? Utils.decToStr(item.SumPayPFR_Strah) : Utils.decToStr(0);
                    (SZV_6_4_Re2.Items.Find("SumFeePFR_Nakop", true)[0] as Telerik.Reporting.TextBox).Value = item.SumFeePFR_Nakop.HasValue ? Utils.decToStr(item.SumFeePFR_Nakop) : Utils.decToStr(0);
                    (SZV_6_4_Re2.Items.Find("SumPayPFR_Nakop", true)[0] as Telerik.Reporting.TextBox).Value = item.SumPayPFR_Nakop.HasValue ? Utils.decToStr(item.SumPayPFR_Nakop) : Utils.decToStr(0);


                    //if (!fromXMLflag)
                    //{
                    SZV_6_4_Staj = db.StajOsn.Where(x => x.FormsSZV_6_4_ID == item.ID).ToList();
                    //}
                    //else
                    //{
                    //    SZV_6_4_Staj = item.StajOsn.ToList();
                    //}


                    Telerik.Reporting.Table table_staj = SZV_6_4_Re2.Items.Find("table12", true)[0] as Telerik.Reporting.Table;
                    SZV_6_4_Re2.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });
                    TableGroup detailGrouptable_staj = new TableGroup();

                    j = 1;

                    foreach (var staj in SZV_6_4_Staj.OrderBy(x => x.Number.Value))
                    {
                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = staj.Number.ToString()
                        };
                        table_staj.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Convert.ToDateTime(staj.DateBegin).ToString("dd/MM/yyyy")
                        };
                        table_staj.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Convert.ToDateTime(staj.DateEnd).ToString("dd/MM/yyyy")
                        };
                        table_staj.Body.SetCellContent(j, 2, textBox);

                        if (staj.StajLgot.Count() > 0)
                        {
                            int ii = 0;
                            foreach (var stajLgot in staj.StajLgot.OrderBy(x => x.Number.Value))
                            {
                                if (ii > 0) // если несколько записей о льготном стаже
                                {

                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 0, textBox);
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 1, textBox);
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 2, textBox);

                                }

                                if (stajLgot.TerrUslID != null && stajLgot.TerrUslID.Value.ToString() != "" && TerrUsl_list.Any(x => x.ID == stajLgot.TerrUslID))
                                {
                                    TerrUsl tu = TerrUsl_list.FirstOrDefault(x => x.ID == stajLgot.TerrUslID);
                                    string koef = "";
                                    if (stajLgot.TerrUslKoef.HasValue && stajLgot.TerrUslKoef.Value != 0)
                                    {
                                        koef = Utils.decToStr(stajLgot.TerrUslKoef.Value);
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
                                table_staj.Body.SetCellContent(j, 3, textBox);

                                if (stajLgot.OsobUslTrudaID != null && stajLgot.OsobUslTrudaID.Value.ToString() != "" && OsobUslTruda_list.Any(x => x.ID == stajLgot.OsobUslTrudaID))
                                {
                                    OsobUslTruda ou = OsobUslTruda_list.FirstOrDefault(x => x.ID == stajLgot.OsobUslTrudaID);
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
                                table_staj.Body.SetCellContent(j, 4, textBox);


                                //                                if (rsw68_dop.IschislStrahStajOsnID != null)
                                //                                {
                                IschislStrahStajOsn isso = IschislStrahStajOsn_list.FirstOrDefault(x => x.ID == stajLgot.IschislStrahStajOsnID);

                                string str = stajLgot.IschislStrahStajDopID == null ? "  " : stajLgot.IschislStrahStajDopID.HasValue ? IschislStrahStajDop_list.FirstOrDefault(x => x.ID == stajLgot.IschislStrahStajDopID).Code : "  ";
                                string s1 = stajLgot.Strah1Param.HasValue ? stajLgot.Strah1Param.Value.ToString() : "0";
                                string s2 = stajLgot.Strah2Param.HasValue ? stajLgot.Strah2Param.Value.ToString() : "0";

                                str = ((!String.IsNullOrEmpty(str.Trim()) || (s1 != "0") || (s2 != "0"))) ? "[" + s1 + "][" + s2 + "][" + str + "]" : "";

                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = isso == null ? "" : isso.Code == null ? "" : isso.Code.ToString()
                                };
                                table_staj.Body.SetCellContent(j, 5, textBox);
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = str
                                };


                                table_staj.Body.SetCellContent(j, 6, textBox);

                                if (stajLgot.UslDosrNaznID != null)
                                {
                                    UslDosrNazn udn = UslDosrNazn_list.FirstOrDefault(x => x.ID == stajLgot.UslDosrNaznID);

                                    s1 = stajLgot.UslDosrNazn1Param.HasValue == true ? stajLgot.UslDosrNazn1Param.Value.ToString() : "0";
                                    s2 = stajLgot.UslDosrNazn2Param.HasValue == true ? stajLgot.UslDosrNazn2Param.Value.ToString() : "0";
                                    string s3 = stajLgot.UslDosrNazn3Param.HasValue == true ? Utils.decToStr(stajLgot.UslDosrNazn3Param.Value) : "0";

                                    str = "[" + s1 + "][" + s2 + "][" + s3 + "]";

                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = udn == null ? "" : udn.Code == null ? "" : udn.Code.ToString()
                                    };
                                    table_staj.Body.SetCellContent(j, 7, textBox);
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = str
                                    };
                                    table_staj.Body.SetCellContent(j, 8, textBox);
                                }
                                else
                                {
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 7, textBox);
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 8, textBox);
                                }

                                ii++;

                                if (staj.StajLgot.Count() > 1 && ii != staj.StajLgot.Count())
                                {
                                    j++;
                                    table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                    detailGrouptable_staj.ChildGroups.Add(new TableGroup());
                                }

                                if (stajLgot.KodVred_OsnID != null && stajLgot.KodVred_OsnID.Value.ToString() != "" && KodVred_2_list.Any(x => x.ID == stajLgot.KodVred_OsnID))
                                {
                                    if ((staj.StajLgot.Count()) == 1 || (staj.StajLgot.Count() > 1 && ii == staj.StajLgot.Count()))
                                    {
                                        j++;
                                        table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                        detailGrouptable_staj.ChildGroups.Add(new TableGroup());
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
                                            table_staj.Body.SetCellContent(j, m, textBox);
                                        }
                                    }

                                    KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.ID == stajLgot.KodVred_OsnID);

                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = kv.Code == null ? "" : kv.Code.ToString()
                                    };
                                    table_staj.Body.SetCellContent(j, 4, textBox, 1, 2);
                                    if (staj.StajLgot.Count() > 1 && ii != staj.StajLgot.Count())
                                    {
                                        j++;
                                        table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                        detailGrouptable_staj.ChildGroups.Add(new TableGroup());
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
                                table_staj.Body.SetCellContent(j, m, textBox);
                            }
                        }

                        if (SZV_6_4_Staj.Count > 1)
                        {
                            table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                            detailGrouptable_staj.ChildGroups.Add(new TableGroup());
                            j++;
                        }
                        else if (SZV_6_4_Staj.Count == 1)
                        {
                            // && rsw68.StajLgot.Count <= 1
                            table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                            detailGrouptable_staj.ChildGroups.Add(new TableGroup());
                        }


                    }
                    if (SZV_6_4_Staj.Count() > 0)
                        table_staj.RowGroups.Add(detailGrouptable_staj);




                    if (ins.BossPrint.HasValue && ins.BossPrint.Value == true)
                    {
                        (SZV_6_4_Re2.Items.Find("rukName1", true)[0] as Telerik.Reporting.TextBox).Value = ins.BossFIO;
                        (SZV_6_4_Re2.Items.Find("rukDolgn1", true)[0] as Telerik.Reporting.TextBox).Value = ins.BossDolgn;
                    }

                    (SZV_6_4_Re2.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = item.DateFilling.ToShortDateString();

                    RSW2014Book.Reports.Add(SZV_6_4_Re2);
                    pageCnt++;

                }






                #endregion
            }

            if (wp == 1 || wp == 2)
            {
                // Инд.сведения 2013 СЗВ-6-1
                #region Инд.сведения 2013 СЗВ-6-1
                //if (!fromXMLflag && SZV_6_data != null)
                //{

                //    SZV_6_1_List = db.FormsSZV_6.Where(x => x.ID == SZV_6_data.ID).OrderBy(x => x.Staff.LastName).ToList();
                //}



                if (SZV_6_1_List.Any(x => x.Staff == null))
                {
                    foreach (var item in SZV_6_1_List.Where(x => x.Staff == null))
                    {
                        item.Staff = db.Staff.First(x => x.ID == item.StaffID);
                    }
                }

                foreach (var item in SZV_6_1_List)
                {

                    PU.FormsSZV_6_2010.Report.SZV_6_1_Re1 SZV_6_1_Re1 = new PU.FormsSZV_6_2010.Report.SZV_6_1_Re1();
                    (SZV_6_1_Re1.Items.Find("RegNum", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
                    (SZV_6_1_Re1.Items.Find("NameShort", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(ins.NameShort) ? ins.NameShort : "";
                    (SZV_6_1_Re1.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN;
                    (SZV_6_1_Re1.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP;
                    (SZV_6_1_Re1.Items.Find("OKPO", true)[0] as Telerik.Reporting.TextBox).Value = ins.OKPO;

                    (SZV_6_1_Re1.Items.Find("PlatCat", true)[0] as Telerik.Reporting.TextBox).Value = item.PlatCategory != null ? item.PlatCategory.Code : "";

                    string tempName = "";
                    if (item.Year >= 2014)
                    {
                        switch (item.Quarter)
                        {
                            case 3:
                                tempName = "period1";
                                break;
                            case 6:
                                tempName = "period2";
                                break;
                            case 9:
                                tempName = "period3";
                                break;
                            case 0:
                                tempName = "period4";
                                break;
                        }
                    }
                    else if (item.Year >= 2011)
                    {
                        tempName = "period" + item.Quarter;
                    }

                    else if (item.Year == 2010)
                    {
                        tempName = "period" + item.Quarter + "_2010";
                    }


                    if (!String.IsNullOrEmpty(tempName))
                    {
                        (SZV_6_1_Re1.Items.Find(tempName, true)[0] as Telerik.Reporting.TextBox).Value = "V";
                    }

                    if (item.Year != 2010)
                        (SZV_6_1_Re1.Items.Find("periodYear", true)[0] as Telerik.Reporting.TextBox).Value = item.Year.ToString();


                    tempName = "";
                    switch (item.TypeInfoID)
                    {
                        case 1:
                            tempName = "typeInfoIshod";
                            break;
                        case 2:
                            tempName = "typeInfoKorr";
                            break;
                        case 3:
                            tempName = "typeInfoOtm";
                            break;
                    }
                    if (!String.IsNullOrEmpty(tempName))
                    {
                        (SZV_6_1_Re1.Items.Find(tempName, true)[0] as Telerik.Reporting.TextBox).Value = "V";
                    }

                    if (item.TypeInfoID >= 2)
                    {
                        (SZV_6_1_Re1.Items.Find("periodKorr" + item.QuarterKorr, true)[0] as Telerik.Reporting.TextBox).Value = "V";
                        (SZV_6_1_Re1.Items.Find("periodKorrYear", true)[0] as Telerik.Reporting.TextBox).Value = item.YearKorr.ToString();
                    }


                    (SZV_6_1_Re1.Items.Find("FIO", true)[0] as Telerik.Reporting.TextBox).Value = (!String.IsNullOrEmpty(item.Staff.LastName) ? item.Staff.LastName : "") + " " + (!String.IsNullOrEmpty(item.Staff.FirstName) ? item.Staff.FirstName : "") + " " + (!String.IsNullOrEmpty(item.Staff.MiddleName) ? item.Staff.MiddleName : "");
                    (SZV_6_1_Re1.Items.Find("SNILS", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? Utils.ParseSNILS(item.Staff.InsuranceNumber.ToString(), (item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value : (short)0)) : ""; ;
                    (SZV_6_1_Re1.Items.Find("SumFeePFR_Strah", true)[0] as Telerik.Reporting.TextBox).Value = item.SumFeePFR_Strah.HasValue ? Utils.decToStr(item.SumFeePFR_Strah) : Utils.decToStr(0);
                    (SZV_6_1_Re1.Items.Find("SumPayPFR_Strah", true)[0] as Telerik.Reporting.TextBox).Value = item.SumPayPFR_Strah.HasValue ? Utils.decToStr(item.SumPayPFR_Strah) : Utils.decToStr(0);
                    (SZV_6_1_Re1.Items.Find("SumFeePFR_Nakop", true)[0] as Telerik.Reporting.TextBox).Value = item.SumFeePFR_Nakop.HasValue ? Utils.decToStr(item.SumFeePFR_Nakop) : Utils.decToStr(0);
                    (SZV_6_1_Re1.Items.Find("SumPayPFR_Nakop", true)[0] as Telerik.Reporting.TextBox).Value = item.SumPayPFR_Nakop.HasValue ? Utils.decToStr(item.SumPayPFR_Nakop) : Utils.decToStr(0);


                    //if (!fromXMLflag)
                    //{
                    SZV_6_1_Staj = db.StajOsn.Where(x => x.FormsSZV_6_ID == item.ID).ToList();
                    //}
                    //else
                    //{
                    //    SZV_6_1_Staj = item.StajOsn.ToList();
                    //}


                    Telerik.Reporting.Table table_staj = SZV_6_1_Re1.Items.Find("table12", true)[0] as Telerik.Reporting.Table;
                    SZV_6_1_Re1.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });
                    TableGroup detailGrouptable_staj = new TableGroup();

                    j = 1;

                    foreach (var staj in SZV_6_1_Staj.OrderBy(x => x.Number.Value))
                    {
                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = staj.Number.ToString()
                        };
                        table_staj.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Convert.ToDateTime(staj.DateBegin).ToString("dd/MM/yyyy")
                        };
                        table_staj.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = Convert.ToDateTime(staj.DateEnd).ToString("dd/MM/yyyy")
                        };
                        table_staj.Body.SetCellContent(j, 2, textBox);

                        if (staj.StajLgot.Count() > 0)
                        {
                            int ii = 0;
                            foreach (var stajLgot in staj.StajLgot.OrderBy(x => x.Number.Value))
                            {
                                if (ii > 0) // если несколько записей о льготном стаже
                                {

                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 0, textBox);
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 1, textBox);
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 2, textBox);

                                }

                                if (stajLgot.TerrUslID != null && stajLgot.TerrUslID.Value.ToString() != "" && TerrUsl_list.Any(x => x.ID == stajLgot.TerrUslID))
                                {
                                    TerrUsl tu = TerrUsl_list.FirstOrDefault(x => x.ID == stajLgot.TerrUslID);
                                    string koef = "";
                                    if (stajLgot.TerrUslKoef.HasValue && stajLgot.TerrUslKoef.Value != 0)
                                    {
                                        koef = Utils.decToStr(stajLgot.TerrUslKoef.Value);
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
                                table_staj.Body.SetCellContent(j, 3, textBox);

                                if (stajLgot.OsobUslTrudaID != null && stajLgot.OsobUslTrudaID.Value.ToString() != "" && OsobUslTruda_list.Any(x => x.ID == stajLgot.OsobUslTrudaID))
                                {
                                    OsobUslTruda ou = OsobUslTruda_list.FirstOrDefault(x => x.ID == stajLgot.OsobUslTrudaID);
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
                                table_staj.Body.SetCellContent(j, 4, textBox);


                                IschislStrahStajOsn isso = IschislStrahStajOsn_list.FirstOrDefault(x => x.ID == stajLgot.IschislStrahStajOsnID);

                                string str = stajLgot.IschislStrahStajDopID == null ? "  " : stajLgot.IschislStrahStajDopID.HasValue ? IschislStrahStajDop_list.FirstOrDefault(x => x.ID == stajLgot.IschislStrahStajDopID).Code : "  ";
                                string s1 = stajLgot.Strah1Param.HasValue ? stajLgot.Strah1Param.Value.ToString() : "0";
                                string s2 = stajLgot.Strah2Param.HasValue ? stajLgot.Strah2Param.Value.ToString() : "0";

                                str = ((!String.IsNullOrEmpty(str.Trim()) || (s1 != "0") || (s2 != "0"))) ? "[" + s1 + "][" + s2 + "][" + str + "]" : "";

                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = isso == null ? "" : isso.Code == null ? "" : isso.Code.ToString()
                                };
                                table_staj.Body.SetCellContent(j, 5, textBox);
                                textBox = new Telerik.Reporting.TextBox
                                {
                                    StyleName = "TableStyle",
                                    Value = str
                                };


                                table_staj.Body.SetCellContent(j, 6, textBox);

                                if (stajLgot.UslDosrNaznID != null)
                                {
                                    UslDosrNazn udn = UslDosrNazn_list.FirstOrDefault(x => x.ID == stajLgot.UslDosrNaznID);

                                    s1 = stajLgot.UslDosrNazn1Param.HasValue == true ? stajLgot.UslDosrNazn1Param.Value.ToString() : "0";
                                    s2 = stajLgot.UslDosrNazn2Param.HasValue == true ? stajLgot.UslDosrNazn2Param.Value.ToString() : "0";
                                    string s3 = stajLgot.UslDosrNazn3Param.HasValue == true ? Utils.decToStr(stajLgot.UslDosrNazn3Param.Value) : "0";

                                    str = "[" + s1 + "][" + s2 + "][" + s3 + "]";

                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = udn == null ? "" : udn.Code == null ? "" : udn.Code.ToString()
                                    };
                                    table_staj.Body.SetCellContent(j, 7, textBox);
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = str
                                    };
                                    table_staj.Body.SetCellContent(j, 8, textBox);
                                }
                                else
                                {
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 7, textBox);
                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = ""
                                    };
                                    table_staj.Body.SetCellContent(j, 8, textBox);
                                }

                                ii++;

                                if (staj.StajLgot.Count() > 1 && ii != staj.StajLgot.Count())
                                {
                                    j++;
                                    table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                    detailGrouptable_staj.ChildGroups.Add(new TableGroup());
                                }

                                if (stajLgot.KodVred_OsnID != null && stajLgot.KodVred_OsnID.Value.ToString() != "" && KodVred_2_list.Any(x => x.ID == stajLgot.KodVred_OsnID))
                                {
                                    if ((staj.StajLgot.Count()) == 1 || (staj.StajLgot.Count() > 1 && ii == staj.StajLgot.Count()))
                                    {
                                        j++;
                                        table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                        detailGrouptable_staj.ChildGroups.Add(new TableGroup());
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
                                            table_staj.Body.SetCellContent(j, m, textBox);
                                        }
                                    }

                                    KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.ID == stajLgot.KodVred_OsnID);

                                    textBox = new Telerik.Reporting.TextBox
                                    {
                                        StyleName = "TableStyle",
                                        Value = kv.Code == null ? "" : kv.Code.ToString()
                                    };
                                    table_staj.Body.SetCellContent(j, 4, textBox, 1, 2);
                                    if (staj.StajLgot.Count() > 1 && ii != staj.StajLgot.Count())
                                    {
                                        j++;
                                        table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                                        detailGrouptable_staj.ChildGroups.Add(new TableGroup());
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
                                table_staj.Body.SetCellContent(j, m, textBox);
                            }
                        }

                        if (SZV_6_1_Staj.Count > 1)
                        {
                            table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                            detailGrouptable_staj.ChildGroups.Add(new TableGroup());
                            j++;
                        }
                        else if (SZV_6_1_Staj.Count == 1)
                        {
                            // && rsw68.StajLgot.Count <= 1
                            table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                            detailGrouptable_staj.ChildGroups.Add(new TableGroup());
                        }


                    }
                    if (SZV_6_1_Staj.Count() > 0)
                        table_staj.RowGroups.Add(detailGrouptable_staj);




                    if (ins.BossPrint.HasValue && ins.BossPrint.Value == true)
                    {
                        (SZV_6_1_Re1.Items.Find("rukName1", true)[0] as Telerik.Reporting.TextBox).Value = ins.BossFIO;
                        (SZV_6_1_Re1.Items.Find("rukDolgn1", true)[0] as Telerik.Reporting.TextBox).Value = ins.BossDolgn;
                    }

                    (SZV_6_1_Re1.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = item.DateFilling.ToShortDateString();

                    RSW2014Book.Reports.Add(SZV_6_1_Re1);
                    pageCnt++;

                }

                #endregion
            }

            if (wp == 1 || wp == 2)
            {
                // Инд.сведения 2013 СЗВ-6-2
                #region Инд.сведения 2013 СЗВ-6-2
                //if (!fromXMLflag && SZV_6_data != null)
                //{

                //    SZV_6_2_List.Clear();
                //    var SZV_6_2_List_temp = db.FormsSZV_6.Where(x => x.ID == SZV_6_data.ID).OrderBy(x => x.Staff.LastName).ToList();
                //    SZV_6_2_List.Add(SZV_6_2_List_temp);
                //}

                foreach (var SZV_6_2_List_part in SZV_6_2_List)
                {
                    if (SZV_6_2_List_part.Count() <= 0)
                    {
                        continue;
                    }


                    if (SZV_6_2_List_part.Any(x => x.Staff == null))
                    {
                        foreach (var item in SZV_6_2_List_part.Where(x => x.Staff == null))
                        {
                            item.Staff = db.Staff.First(x => x.ID == item.StaffID);
                        }
                    }

                    PU.FormsSZV_6_2010.Report.SZV_6_2_Re1 SZV_6_2_Re1 = new PU.FormsSZV_6_2010.Report.SZV_6_2_Re1();
                    (SZV_6_2_Re1.Items.Find("RegNum", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
                    (SZV_6_2_Re1.Items.Find("NameShort", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(ins.NameShort) ? ins.NameShort : "";
                    (SZV_6_2_Re1.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN;
                    (SZV_6_2_Re1.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP;
                    (SZV_6_2_Re1.Items.Find("OKPO", true)[0] as Telerik.Reporting.TextBox).Value = ins.OKPO;

                    (SZV_6_2_Re1.Items.Find("staffCount", true)[0] as Telerik.Reporting.TextBox).Value = SZV_6_2_List_part.Count().ToString();

                    var t = SZV_6_2_List_part.First();

                    (SZV_6_2_Re1.Items.Find("PlatCat", true)[0] as Telerik.Reporting.TextBox).Value = t.PlatCategory != null ? t.PlatCategory.Code : "";


                    string tempName = "";
                    if (t.Year >= 2014)
                    {
                        switch (t.Quarter)
                        {
                            case 3:
                                tempName = "period1";
                                break;
                            case 6:
                                tempName = "period2";
                                break;
                            case 9:
                                tempName = "period3";
                                break;
                            case 0:
                                tempName = "period4";
                                break;
                        }
                    }
                    else if (t.Year >= 2011)
                    {
                        tempName = "period" + t.Quarter;
                    }

                    else if (t.Year == 2010)
                    {
                        tempName = "period" + t.Quarter + "_2010";
                    }


                    if (!String.IsNullOrEmpty(tempName))
                    {
                        (SZV_6_2_Re1.Items.Find(tempName, true)[0] as Telerik.Reporting.TextBox).Value = "V";
                    }

                    if (t.Year != 2010)
                        (SZV_6_2_Re1.Items.Find("periodYear", true)[0] as Telerik.Reporting.TextBox).Value = t.Year.ToString();


                    tempName = "";
                    switch (t.TypeInfoID)
                    {
                        case 1:
                            tempName = "typeInfoIshod";
                            break;
                        case 2:
                            tempName = "typeInfoKorr";
                            break;
                        case 3:
                            tempName = "typeInfoOtm";
                            break;
                    }
                    if (!String.IsNullOrEmpty(tempName))
                    {
                        (SZV_6_2_Re1.Items.Find(tempName, true)[0] as Telerik.Reporting.TextBox).Value = "V";
                    }

                    if (t.TypeInfoID >= 2)
                    {
                        (SZV_6_2_Re1.Items.Find("periodKorr" + t.QuarterKorr, true)[0] as Telerik.Reporting.TextBox).Value = "V";
                        (SZV_6_2_Re1.Items.Find("periodKorrYear", true)[0] as Telerik.Reporting.TextBox).Value = t.YearKorr.ToString();
                    }
                    Telerik.Reporting.Table table_Re = SZV_6_2_Re1.Items.Find("staffTable", true)[0] as Telerik.Reporting.Table;
                    TableGroup detailGrouptable_Re = new TableGroup();
                    SZV_6_2_Re1.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

                    j = 1;
                    foreach (var item in SZV_6_2_List_part)
                    {


                        Telerik.Reporting.TextBox textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = j.ToString()
                        };
                        table_Re.Body.SetCellContent(j, 0, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = (!String.IsNullOrEmpty(item.Staff.LastName) ? item.Staff.LastName : "") + " " + (!String.IsNullOrEmpty(item.Staff.FirstName) ? item.Staff.FirstName : "") + " " + (!String.IsNullOrEmpty(item.Staff.MiddleName) ? item.Staff.MiddleName : "")
                        };
                        table_Re.Body.SetCellContent(j, 1, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? Utils.ParseSNILS(item.Staff.InsuranceNumber.ToString(), (item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value : (short)0)) : " "
                        };
                        table_Re.Body.SetCellContent(j, 2, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = " "
                        };
                        table_Re.Body.SetCellContent(j, 3, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.SumFeePFR_Strah.HasValue ? Utils.decToStr(item.SumFeePFR_Strah) : Utils.decToStr(0)
                        };
                        table_Re.Body.SetCellContent(j, 4, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.SumPayPFR_Strah.HasValue ? Utils.decToStr(item.SumPayPFR_Strah) : Utils.decToStr(0)
                        };
                        table_Re.Body.SetCellContent(j, 5, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.SumFeePFR_Nakop.HasValue ? Utils.decToStr(item.SumFeePFR_Nakop) : Utils.decToStr(0)
                        };
                        table_Re.Body.SetCellContent(j, 6, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.SumPayPFR_Nakop.HasValue ? Utils.decToStr(item.SumPayPFR_Nakop) : Utils.decToStr(0)
                        };
                        table_Re.Body.SetCellContent(j, 7, textBox);
                        textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.StajOsn.Any() ? (item.StajOsn.First().DateBegin.HasValue ? item.StajOsn.First().DateBegin.Value.ToShortDateString() : " ") : " "
                        };
                        table_Re.Body.SetCellContent(j, 8, textBox); textBox = new Telerik.Reporting.TextBox
                        {
                            StyleName = "TableStyle",
                            Value = item.StajOsn.Any() ? (item.StajOsn.First().DateEnd.HasValue ? item.StajOsn.First().DateEnd.Value.ToShortDateString() : " ") : " "
                        };
                        table_Re.Body.SetCellContent(j, 9, textBox); table_Re.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));

                        detailGrouptable_Re.ChildGroups.Add(new TableGroup());
                        j++;

                    }


                    Telerik.Reporting.TextBox textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = "Всего по реестру"
                    };
                    table_Re.Body.SetCellContent(j, 0, textBoxTotal66, 1, 4);
                    textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(SZV_6_2_List_part.Sum(x => x.SumFeePFR_Strah.Value))
                    };
                    table_Re.Body.SetCellContent(j, 4, textBoxTotal66);
                    textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(SZV_6_2_List_part.Sum(x => x.SumPayPFR_Strah.Value))
                    };
                    table_Re.Body.SetCellContent(j, 5, textBoxTotal66);
                    textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(SZV_6_2_List_part.Sum(x => x.SumFeePFR_Nakop.Value))
                    };
                    table_Re.Body.SetCellContent(j, 6, textBoxTotal66);
                    textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = Utils.decToStr(SZV_6_2_List_part.Sum(x => x.SumPayPFR_Nakop.Value))
                    };
                    table_Re.Body.SetCellContent(j, 7, textBoxTotal66);
                    textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = " "
                    };
                    table_Re.Body.SetCellContent(j, 8, textBoxTotal66);
                    textBoxTotal66 = new Telerik.Reporting.TextBox
                    {
                        StyleName = "TableStyle",
                        Value = " "
                    };
                    table_Re.Body.SetCellContent(j, 9, textBoxTotal66);
                    table_Re.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                    detailGrouptable_Re.ChildGroups.Add(new TableGroup());

                    table_Re.RowGroups.Add(detailGrouptable_Re);
                    if (ins.BossPrint.HasValue && ins.BossPrint.Value == true)
                    {
                        (SZV_6_2_Re1.Items.Find("rukName1", true)[0] as Telerik.Reporting.TextBox).Value = ins.BossFIO;
                        (SZV_6_2_Re1.Items.Find("rukDolgn1", true)[0] as Telerik.Reporting.TextBox).Value = ins.BossDolgn;
                    }

                    (SZV_6_2_Re1.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = t.DateFilling.ToShortDateString();
                    RSW2014Book.Reports.Add(SZV_6_2_Re1);
                    pageCnt++;
                }

                #endregion
            }

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();
            //            Telerik.Reporting.TextBox pageCnt = RSW2014Book.Reports[0].Items.Find("textBox54", true)[0] as Telerik.Reporting.TextBox;
            if (wp == 0 || wp == 2)
            {
                Telerik.Reporting.TextBox pageCount = RSW2014Book.Reports[0].Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
                pageCount.Value = pageCnt.ToString();
            }

            typeReportSource.ReportDocument = RSW2014Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }


        public void createEmptyReport(short year)
        {
            ReportBook RSW2014Book = new ReportBook();

            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            //Делаем стиль для табличек
            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            //Делаем стиль для табличек
            Telerik.Reporting.Drawing.StyleRule myStyleRuleSmaller = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRuleSmaller.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyleSmaller") });
            myStyleRuleSmaller.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRuleSmaller.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRuleSmaller.Style.Font.Name = "Arial";
            myStyleRuleSmaller.Style.Font.Size = new Unit(5.5, UnitType.Point);
            myStyleRuleSmaller.Style.TextAlign = HorizontalAlign.Center;
            myStyleRuleSmaller.Style.VerticalAlign = VerticalAlign.Middle;
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            //Титульный лист

            #region  Титульный лист

            RSW2014_ReTitle RSW2014_Title = new RSW2014_ReTitle();
            Telerik.Reporting.TextBox st = RSW2014_Title.Items.Find("textBox83", true)[0] as Telerik.Reporting.TextBox;
            st.Style.BorderStyle.Default = BorderType.Dotted;
            st.Style.TextAlign = HorizontalAlign.Left;
            st.Value = "Стр.";

            if (year == 2015)
            {

                Telerik.Reporting.TextBox textBox9 = RSW2014_Title.Items.Find("textBox9", true)[0] as Telerik.Reporting.TextBox;
                textBox9.Value = "Приложение\r\n" +
                                 "к Изменениям,\r\n" +
                                 "которые вносятся в постановление\r\n" +
                                 "Правления ПФР от 16.01.2014 № 2п,\r\n" +
                                 "утвержденным постановлением\r\n" +
                                 "Правления ПФР\r\n" +
                                 "от 04.06.2015г.\r\n" +
                                 "№ 194п";

                RSW2014_Title.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(4);

                Telerik.Reporting.TextBox textBox13 = RSW2014_Title.Items.Find("textBox13", true)[0] as Telerik.Reporting.TextBox;
                textBox13.Value = "Номер уточнения";
                Telerik.Reporting.TextBox textBox14 = RSW2014_Title.Items.Find("textBox14", true)[0] as Telerik.Reporting.TextBox;
                textBox14.Value = "(000 - исходная форма, уточнение 001 и т.д.)";
                Telerik.Reporting.TextBox textBox1 = RSW2014_Title.Items.Find("textBox1", true)[0] as Telerik.Reporting.TextBox;
                textBox1.Value = "Причина уточнения";
                Telerik.Reporting.TextBox textBox82 = RSW2014_Title.Items.Find("textBox82", true)[0] as Telerik.Reporting.TextBox;
                textBox82.Value = "       *  Указывается дата представления расчета лично или через представителя, при отправке по почте - дата отправки почтового отправления с описью вложения.";


                //}
            }
            else if (year == 2014)
            {
                Telerik.Reporting.TextBox textBox9 = RSW2014_Title.Items.Find("textBox9", true)[0] as Telerik.Reporting.TextBox;
                textBox9.Value = "Приложение № 1\r\n" +
                                 "к Постановлению Правлению ПФР\r\n" +
                                 "от 16.01.2014 № 2п";
            }

            RSW2014Book.Reports.Add(RSW2014_Title);

            //Раздел 1
            #region Раздел 1

            RSW2014_Re1 RSW2014_1 = new RSW2014_Re1();
            Telerik.Reporting.TextBox s1 = RSW2014_1.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
            s1.Style.BorderStyle.Default = BorderType.Dotted;
            s1.Style.TextAlign = HorizontalAlign.Left;
            s1.Value = "Стр.";
            if (year == 2015)
            {
                Telerik.Reporting.TextBox textBox31 = RSW2014_1.Items.Find("textBox31", true)[0] as Telerik.Reporting.TextBox;
                textBox31.Value = "на финансирование страховой пенсии";
                Telerik.Reporting.TextBox textBox32 = RSW2014_1.Items.Find("textBox32", true)[0] as Telerik.Reporting.TextBox;
                textBox32.Value = "на финансирование накопительной пенсии";
                Telerik.Reporting.TextBox textBox72 = RSW2014_1.Items.Find("textBox72", true)[0] as Telerik.Reporting.TextBox;
                textBox72.Value = "Сумма перерасчета страховых взносов за предыдущие отчетные (расчетные) периоды с начала расчетного периода";
                Telerik.Reporting.TextBox textBox56 = RSW2014_1.Items.Find("textBox56", true)[0] as Telerik.Reporting.TextBox;
                textBox56.Value = "* Собрание законодательства Российской Федерации, 2001, № 52, ст. 6965; № 2.";
            }

            RSW2014Book.Reports.Add(RSW2014_1);
            #endregion



            //Раздел 2.1
            if (year == 2014)
            {
                RSW2014_Re21 report21 = new RSW2014_Re21();
                Telerik.Reporting.TextBox s21 = report21.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                s21.Style.BorderStyle.Default = BorderType.Dotted;
                s21.Style.TextAlign = HorizontalAlign.Left;
                s21.Value = "Стр.";

                RSW2014Book.Reports.Add(report21);
            }
            else if (year == 2015)
            {
                RSW2014_Re21_2015 report21 = new RSW2014_Re21_2015();
                Telerik.Reporting.TextBox s21 = report21.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
                s21.Style.BorderStyle.Default = BorderType.Dotted;
                s21.Style.TextAlign = HorizontalAlign.Left;
                s21.Value = "Стр.";
                Telerik.Reporting.TextBox textBox56 = report21.Items.Find("textBox56", true)[0] as Telerik.Reporting.TextBox;
                textBox56.Value = " * Представляется плательщиками страховых взносов отдельно по каждому тарифу, применяемому в отношении выплат застрахованным лицам.";
                RSW2014Book.Reports.Add(report21);
            }

            //Раздел 2.2
            RSW2014_Re22 report22 = new RSW2014_Re22();
            Telerik.Reporting.TextBox s22 = report22.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
            s22.Style.BorderStyle.Default = BorderType.Dotted;
            s22.Style.TextAlign = HorizontalAlign.Left;
            s22.Value = "Стр.";
            if (year == 2015)
            {
                Telerik.Reporting.TextBox textBox96 = report22.Items.Find("textBox96", true)[0] as Telerik.Reporting.TextBox;
                textBox96.Value = " * В отношении выплат и иных вознаграждений в пользу физических лиц, занятых на соответствующих видах работ, указанных в пункте 1 части 1 статьи 30 Федерального закона от 28 декабря 2013 года  № 400-ФЗ \"О страховых пенсиях\".";
                Telerik.Reporting.TextBox textBox95 = report22.Items.Find("textBox95", true)[0] as Telerik.Reporting.TextBox;
                textBox95.Value = " ** В отношении выплат и иных вознаграждений в пользу физических лиц, занятых на соответствующих видах работ, указанных в пунктах 2 - 18 части 1 статьи 30 Федерального закона от 28 декабря 2013 года  № 400-ФЗ \"О страховых пенсиях\".";
            }

            RSW2014Book.Reports.Add(report22);

            //Раздел 2.4

            RSW2014_Re24 report24 = new RSW2014_Re24();
            Telerik.Reporting.TextBox s24 = report24.Items.Find("textBox2", true)[0] as Telerik.Reporting.TextBox;
            s24.Style.BorderStyle.Default = BorderType.Dotted;
            s24.Style.TextAlign = HorizontalAlign.Left;
            s24.Value = "Стр.";
            RSW2014Book.Reports.Add(report24);

            //Раздел 2.5

            RSW2014_Re25 report25 = new RSW2014_Re25();
            Telerik.Reporting.TextBox s25 = report25.Items.Find("textBox3", true)[0] as Telerik.Reporting.TextBox;
            s25.Style.BorderStyle.Default = BorderType.Dotted;
            s25.Style.TextAlign = HorizontalAlign.Left;
            s25.Value = "Стр.";

            Telerik.Reporting.Table table1 = report25.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable1 = new TableGroup();

            report25.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

            for (int i = 1; i <= 10; i++)
            {

                for (int j = 0; j <= 4; j++)
                {
                    table1.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            table1.Body.SetCellContent(11, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "ИТОГО" });
            for (int j = 1; j <= 3; j++)
            {
                table1.Body.SetCellContent(11, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
            }
            table1.Body.SetCellContent(11, 4, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "Х" });


            for (int i = 0; i <= 10; i++)
            {
                table1.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                detailGrouptable1.ChildGroups.Add(new TableGroup());
            }


            table1.RowGroups.Add(detailGrouptable1);

            Telerik.Reporting.Table table2 = report25.Items.Find("table2", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable2 = new TableGroup();

            for (int i = 1; i <= 10; i++)
            {

                for (int j = 0; j <= 7; j++)
                {
                    table2.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            table2.Body.SetCellContent(11, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "ИТОГО" });
            for (int j = 1; j <= 6; j++)
            {
                table2.Body.SetCellContent(11, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
            }
            table2.Body.SetCellContent(11, 7, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "Х" });


            for (int i = 0; i <= 10; i++)
            {
                table2.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                detailGrouptable2.ChildGroups.Add(new TableGroup());
            }

            table2.RowGroups.Add(detailGrouptable2);
            RSW2014Book.Reports.Add(report25);


            //Раздел 3.1 Раздел 3.2
            if (year == 2014)
            {
                Report.RSW2014_Re31 report31 = new Report.RSW2014_Re31();
                Telerik.Reporting.TextBox s31 = report31.Items.Find("textBox3", true)[0] as Telerik.Reporting.TextBox;
                s31.Style.BorderStyle.Default = BorderType.Dotted;
                s31.Style.TextAlign = HorizontalAlign.Left;
                s31.Value = "Стр.";

                RSW2014Book.Reports.Add(report31);
            }
            //Раздел 3.3

            Report.RSW2014_Re33 report33 = new Report.RSW2014_Re33();
            Telerik.Reporting.TextBox s33 = report33.Items.Find("textBox3", true)[0] as Telerik.Reporting.TextBox;
            s33.Style.BorderStyle.Default = BorderType.Dotted;
            s33.Style.TextAlign = HorizontalAlign.Left;
            s33.Value = "Стр.";

            if (year == 2015)
            {
                Telerik.Reporting.TextBox textBox5 = report33.Items.Find("textBox5", true)[0] as Telerik.Reporting.TextBox;
                textBox5.Value = textBox5.Value.ToString().Remove(2, 1).Insert(2, "1");
                ((Telerik.Reporting.TextBox)report33.Items.Find("textBox38", true)[0]).Dispose();
                ((Telerik.Reporting.TextBox)report33.Items.Find("textBox32", true)[0]).Dispose();
                ((Telerik.Reporting.Table)report33.Items.Find("table3", true)[0]).Dispose();
                ((Telerik.Reporting.Table)report33.Items.Find("table4", true)[0]).Dispose();

            }


            //Раздел 3.4
            if (year == 2014)
            {
                Telerik.Reporting.TextBox textBox56 = report33.Items.Find("textBox56", true)[0] as Telerik.Reporting.TextBox;
                textBox56.Dispose();


                Telerik.Reporting.Table table3_Re33 = report33.Items.Find("table3", true)[0] as Telerik.Reporting.Table;
                TableGroup detailGrouptable3_Re33 = new TableGroup();
                report33.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });


                for (int i = 1; i <= 6; i++)
                {
                    for (int j = 0; j <= 3; j++)
                    {
                        table3_Re33.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                    }
                }

                table3_Re33.Body.SetCellContent(7, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "Итого по всем видам деятельности" }, 1, 2);
                for (int j = 2; j <= 2; j++)
                {
                    table3_Re33.Body.SetCellContent(7, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
                table3_Re33.Body.SetCellContent(7, 3, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "100%" });


                for (int i = 0; i <= 6; i++)
                {
                    table3_Re33.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.4D)));
                    detailGrouptable3_Re33.ChildGroups.Add(new TableGroup());
                }


                table3_Re33.RowGroups.Add(detailGrouptable3_Re33);

            }

            RSW2014Book.Reports.Add(report33);

            Report.RSW2014_Re35 report35 = new Report.RSW2014_Re35();
            Telerik.Reporting.TextBox s35 = report35.Items.Find("textBox3", true)[0] as Telerik.Reporting.TextBox;
            s35.Style.BorderStyle.Default = BorderType.Dotted;
            s35.Style.TextAlign = HorizontalAlign.Left;
            s35.Value = "Стр.";


            if (year == 2015)
            {
                ((Telerik.Reporting.TextBox)report35.Items.Find("textBox5", true)[0]).Value = ((Telerik.Reporting.TextBox)report35.Items.Find("textBox5", true)[0]).Value.ToString().Remove(2, 1).Insert(2, "2");
                ((Telerik.Reporting.TextBox)report35.Items.Find("textBox20", true)[0]).Value = ((Telerik.Reporting.TextBox)report35.Items.Find("textBox20", true)[0]).Value.ToString().Remove(2, 1).Insert(2, "3");
            }


            RSW2014Book.Reports.Add(report35);

            //Раздел 4
            Report.RSW2014_Re4 report4 = new Report.RSW2014_Re4();
            Telerik.Reporting.TextBox s4 = report4.Items.Find("textBox3", true)[0] as Telerik.Reporting.TextBox;
            s4.Style.BorderStyle.Default = BorderType.Dotted;
            s4.Style.TextAlign = HorizontalAlign.Left;
            s4.Value = "Стр.";

            if (year == 2015)
            {
                Telerik.Reporting.TextBox textBox88 = report4.Items.Find("textBox88", true)[0] as Telerik.Reporting.TextBox;
                textBox88.Value = " * 1 - в случае доначисления органом контроля за уплатой страховых взносов сумм страховых взносов по актам камеральных проверок, по которым в  отчетном периоде вступили в силу решения о привлечении (об отказе в привлечении) к ответственности плательщиков страховых взносов, а такжев случае, если органом контроля за уплатой страховых взносов выявлены излишне начисленные плательщиком страховых взносов суммы страховых взносов;\r\n" +
                    "   2 - в случае доначисления органом контроля за уплатой страховых взносов сумм страховых взносов по актам выездных проверок, по которым в отчетном периоде вступили в силу решения о привлечении (об отказе в привлечении) к ответственности плательщиков страховых взносов, а также в случае, если органом контроля за уплатой страховых взносов выявлены излишне начисленные плательщиком страховых взносов суммы страховых взносов;\r\n" +
                    "   3 - в случае если плательщиком страховых взносов самостоятельно доначислены страховые взносы в случае выявления факта неотражения или неполноты отражения сведений, а также ошибок, приводящих к занижению суммы страховых взносов, подлежащей уплате за предыдущие отчетные периоды в соответствии со статьей  7 Федерального закона от 24 июля 2009 г. № 212-ФЗ;\r\n" +
                    "   4 - в случае корректировки плательщиком страховых взносов базы для начисления страховых взносов предшествующих отчетных (расчетных) периодов, не признаваемой ошибкой.";

                Telerik.Reporting.TextBox textBox4 = report4.Items.Find("textBox4", true)[0] as Telerik.Reporting.TextBox;
                textBox4.Value = "Раздел 4.  Суммы перерасчета страховых взносов с начала расчетного периода";

                Telerik.Reporting.TextBox textBox11 = report4.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                textBox11.Value = "Период, за который производится перерасчет страховых взносов";
                Telerik.Reporting.TextBox textBox34 = report4.Items.Find("textBox34", true)[0] as Telerik.Reporting.TextBox;
                textBox34.Value = "на финансирование страховой пенсии";
                Telerik.Reporting.TextBox textBox17 = report4.Items.Find("textBox17", true)[0] as Telerik.Reporting.TextBox;
                textBox17.Value = "на финансирование накопительной пенсии";

            }

            Telerik.Reporting.Table table_Re4 = report4.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re4 = new TableGroup();
            report4.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

            string strItog = "Итого доначислено:";
            if (year == 2015)
            {
                strItog = "Итого сумма перерасчета:";
            }

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 0; j <= 13; j++)
                {
                    table_Re4.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            table_Re4.Body.SetCellContent(11, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = strItog }, 1, 5);
            for (int j = 5; j <= 13; j++)
            {
                table_Re4.Body.SetCellContent(11, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
            }

            for (int i = 0; i <= 10; i++)
            {
                table_Re4.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_Re4.ChildGroups.Add(new TableGroup());
            }


            table_Re4.RowGroups.Add(detailGrouptable_Re4);
            RSW2014Book.Reports.Add(report4);
            //Раздел 5

            Report.RSW2014_Re5 report5 = new Report.RSW2014_Re5();
            Telerik.Reporting.TextBox s5 = report5.Items.Find("textBox3", true)[0] as Telerik.Reporting.TextBox;
            s5.Style.BorderStyle.Default = BorderType.Dotted;
            s5.Style.TextAlign = HorizontalAlign.Left;
            s5.Value = "Стр.";


            Telerik.Reporting.Table table_Re5 = report5.Items.Find("table1", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re5 = new TableGroup();
            report5.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });


            for (int i = 1; i <= 10; i++)
            {
                for (int j = 0; j <= 7; j++)
                {
                    table_Re5.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            table_Re5.Body.SetCellContent(11, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "Итого выплат" }, 1, 4);
            for (int j = 4; j <= 7; j++)
            {
                table_Re5.Body.SetCellContent(11, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
            }

            for (int i = 0; i <= 10; i++)
            {
                table_Re5.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_Re5.ChildGroups.Add(new TableGroup());
            }

            table_Re5.RowGroups.Add(detailGrouptable_Re5);
            RSW2014Book.Reports.Add(report5);
            #endregion

            //Раздел 6
            #region  Раздел 6

            Report.RSW2014_Re6 report6 = new Report.RSW2014_Re6();
            if (year == 2014)
            {
                ((Telerik.Reporting.Table)report6.Items.Find("table13", true)[0]).Dispose();
            }

            Telerik.Reporting.TextBox s6 = report6.Items.Find("textBox3", true)[0] as Telerik.Reporting.TextBox;
            s6.Style.BorderStyle.Default = BorderType.Dotted;
            s6.Style.TextAlign = HorizontalAlign.Left;
            s6.Value = "Стр.";

            //раздел 6.4

            Telerik.Reporting.Table table_Re6 = report6.Items.Find("table8", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re6 = new TableGroup();
            report6.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

            for (int i = 0; i <= 1; i++)
            {
                table_Re6.Body.SetCellContent(1 + i * 4, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "Всего с начала расчетного периода, в том числе за последние три месяца отчетного периода:", TextWrap = true, Multiline = true, Height = Telerik.Reporting.Drawing.Unit.Cm(1.2D) });
                table_Re6.Body.SetCellContent(1 + i * 4, 1, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "4" + i.ToString() + "0" });
                for (int j = 2; j <= 6; j++)
                {
                    table_Re6.Body.SetCellContent(1 + i * 4, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }

                for (int k = 1; k <= 3; k++)
                {
                    table_Re6.Body.SetCellContent(1 + i * 4 + k, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = k.ToString() + " Месяц" });
                    table_Re6.Body.SetCellContent(1 + i * 4 + k, 1, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "4" + i.ToString() + k.ToString() });
                    for (int j = 2; j <= 6; j++)
                    {
                        table_Re6.Body.SetCellContent(1 + i * 4 + k, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                    }

                }


            }

            for (int i = 0; i < (4 * 2); i++)
            {
                table_Re6.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_Re6.ChildGroups.Add(new TableGroup());
            }

            table_Re6.RowGroups.Add(detailGrouptable_Re6);

            //раздел 6.6

            if (year == 2015)
            {
                Telerik.Reporting.TextBox textBox70 = report6.Items.Find("textBox70", true)[0] as Telerik.Reporting.TextBox;
                textBox70.Value = "на финансирование страховой пенсии";
                Telerik.Reporting.TextBox textBox71 = report6.Items.Find("textBox71", true)[0] as Telerik.Reporting.TextBox;
                textBox71.Value = "на финансирование накопительной пенсии";

                //Раздел 6.7
                Telerik.Reporting.TextBox textBox82 = report6.Items.Find("textBox82", true)[0] as Telerik.Reporting.TextBox;
                textBox82.Value = "Сумма выплат и иных вознаграждений, начисленных в пользу физического лица, занятого на видах работ, указанных в пункте 1 части 1 статьи 30 Федерального закона от 28 декабря 2013 года № 400-ФЗ \"О страховых пенсиях\"";
                Telerik.Reporting.TextBox textBox84 = report6.Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
                textBox84.Value = "Сумма выплат и иных вознаграждений, начисленных в пользу физического лица, занятого на видах работ, указанных в пунктах 2 - 18 части 1 статьи 30 Федерального закона от 28 декабря 2013 года № 400-ФЗ \"О страховых пенсиях\"";

            }

            Telerik.Reporting.Table table_Re66 = report6.Items.Find("table10", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re66 = new TableGroup();

            for (int i = 1; i <= 5; i++)
            {
                for (int j = 0; j <= 4; j++)
                {
                    table_Re66.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            table_Re66.Body.SetCellContent(6, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "Итого" }, 1, 2);
            for (int j = 2; j <= 4; j++)
            {
                table_Re66.Body.SetCellContent(6, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
            }

            for (int i = 0; i <= 5; i++)
            {
                table_Re66.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_Re66.ChildGroups.Add(new TableGroup());
            }

            table_Re66.RowGroups.Add(detailGrouptable_Re66);

            //раздел 6.7

            Telerik.Reporting.Table table_Re67 = report6.Items.Find("table11", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re67 = new TableGroup();

            for (int i = 0; i <= 1; i++)
            {
                table_Re67.Body.SetCellContent(1 + i * 4, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "Всего с начала расчетного периода, в том числе за последние три месяца отчетного периода:", TextWrap = true, Multiline = true, Height = Telerik.Reporting.Drawing.Unit.Cm(1D) });
                table_Re67.Body.SetCellContent(1 + i * 4, 1, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "7" + i.ToString() + "0" });
                for (int j = 2; j <= 4; j++)
                {
                    table_Re67.Body.SetCellContent(1 + i * 4, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }

                for (int k = 1; k <= 3; k++)
                {
                    table_Re67.Body.SetCellContent(1 + i * 4 + k, 0, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = k.ToString() + " Месяц" });
                    table_Re67.Body.SetCellContent(1 + i * 4 + k, 1, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = "7" + i.ToString() + k.ToString() });
                    for (int j = 2; j <= 4; j++)
                    {
                        table_Re67.Body.SetCellContent(1 + i * 4 + k, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                    }

                }


            }

            for (int i = 0; i < (4 * 2); i++)
            {
                table_Re67.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_Re67.ChildGroups.Add(new TableGroup());
            }

            table_Re67.RowGroups.Add(detailGrouptable_Re67);



            //раздел 6.8

            Telerik.Reporting.Table table_Re68 = report6.Items.Find("table12", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re68 = new TableGroup();

            for (int i = 1; i <= 15; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    table_Re68.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            for (int i = 0; i < 15; i++)
            {
                table_Re68.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_Re68.ChildGroups.Add(new TableGroup());
            }

            table_Re68.RowGroups.Add(detailGrouptable_Re68);


            RSW2014Book.Reports.Add(report6);
            #endregion




            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = RSW2014Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }

        public void createEmptyReport_SZV_6_1()
        {
            ReportBook SZV61 = new ReportBook();

            #region Инд.сведения 2010-2012 СЗВ-6-1

            //Делаем стиль для табличек
            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            PU.FormsSZV_6_2010.Report.SZV_6_1_Re1 SZV_6_1_Re1 = new PU.FormsSZV_6_2010.Report.SZV_6_1_Re1();

            Telerik.Reporting.Table table_staj = SZV_6_1_Re1.Items.Find("table12", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_staj = new TableGroup();
            SZV_6_1_Re1.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    table_staj.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            for (int i = 0; i < 10; i++)
            {
                table_staj.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_staj.ChildGroups.Add(new TableGroup());
            }

            table_staj.RowGroups.Add(detailGrouptable_staj);

            SZV61.Reports.Add(SZV_6_1_Re1);

            #endregion


            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = SZV61;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }

        public void createEmptyReport_SZV_6_2()
        {
            ReportBook SZV62 = new ReportBook();

            #region Инд.сведения 2010-2012 СЗВ-6-2

            //Делаем стиль для табличек
            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            PU.FormsSZV_6_2010.Report.SZV_6_2_Re1 SZV_6_2_Re1 = new PU.FormsSZV_6_2010.Report.SZV_6_2_Re1();

            Telerik.Reporting.Table table_staff = SZV_6_2_Re1.Items.Find("staffTable", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_staff = new TableGroup();
            SZV_6_2_Re1.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

            for (int i = 1; i <= 9; i++)
            {
                for (int j = 0; j <= 9; j++)
                {
                    table_staff.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            for (int i = 0; i < 9; i++)
            {
                table_staff.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_staff.ChildGroups.Add(new TableGroup());
            }

            table_staff.RowGroups.Add(detailGrouptable_staff);

            SZV62.Reports.Add(SZV_6_2_Re1);

            #endregion


            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = SZV62;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }

        public void createEmptyReport_SZV_6_4()
        {
            ReportBook SZV64 = new ReportBook();

            #region Инд.сведения 2013 СЗВ-6-4

            //Делаем стиль для табличек
            Telerik.Reporting.Drawing.StyleRule myStyleRule = new Telerik.Reporting.Drawing.StyleRule();
            myStyleRule.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] { new Telerik.Reporting.Drawing.StyleSelector("TableStyle") });
            myStyleRule.Style.BorderStyle.Default = BorderType.Solid;
            myStyleRule.Style.BorderWidth.Default = new Unit(1.0, UnitType.Pixel);
            myStyleRule.Style.Font.Name = "Arial";
            myStyleRule.Style.Font.Size = new Unit(7.0, UnitType.Point);
            myStyleRule.Style.TextAlign = HorizontalAlign.Center;
            myStyleRule.Style.VerticalAlign = VerticalAlign.Middle;

            PU.FormsSZV_6_4_2013.Report.SZV_6_4_Re1 SZV_6_4_Re1 = new PU.FormsSZV_6_4_2013.Report.SZV_6_4_Re1();

            PU.FormsSZV_6_4_2013.Report.SZV_6_4_Re2 SZV_6_4_Re2 = new PU.FormsSZV_6_4_2013.Report.SZV_6_4_Re2();


            Telerik.Reporting.Table table_Re = SZV_6_4_Re2.Items.Find("table12", true)[0] as Telerik.Reporting.Table;
            TableGroup detailGrouptable_Re = new TableGroup();
            SZV_6_4_Re2.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] { myStyleRule });

            for (int i = 1; i <= 10; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    table_Re.Body.SetCellContent(i, j, new Telerik.Reporting.TextBox { StyleName = "TableStyle", Value = " " });
                }
            }

            for (int i = 0; i < 10; i++)
            {
                table_Re.Body.Rows.Add(new TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.6D)));
                detailGrouptable_Re.ChildGroups.Add(new TableGroup());
            }

            table_Re.RowGroups.Add(detailGrouptable_Re);


            SZV64.Reports.Add(SZV_6_4_Re1);
            SZV64.Reports.Add(SZV_6_4_Re2);
            #endregion




            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = SZV64;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }


        private void RSW2014_Print_Load(object sender, EventArgs e)
        {
            //   createReport(null, null);
            reportViewer1.RefreshReport();
        }
    }
}