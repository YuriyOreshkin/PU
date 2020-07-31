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
using PU.FormsDSW3.Report;
using System.Globalization;

namespace PU.FormsDSW3
{
    public partial class DSW3_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public FormsDSW_3 DSW3data { get; set; }

        public DSW3_Print()
        {
            InitializeComponent();
        }

        private void DSW3_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }

        private class DSW3Cont
        {
            public int Num {get; set; }
            public string FIO { get; set; }
            public string SNILS { get; set; }
            public string SUMFEEPFR_EMPLOYERS { get; set; }
            public string SUMFEEPFR_PAYER { get; set; }
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            DSW3_Report DSW3 = new DSW3_Report();

            if (DSW3data.YEAR >= 2016)
            {
                (DSW3.Items.Find("textBox23", true)[0] as Telerik.Reporting.TextBox).Value = "постановлением Правления Пенсионного фонда Российской Федерации \r\nот 09 июня 2016 г.  № 482п";
                (DSW3.Items.Find("textBox8", true)[0] as Telerik.Reporting.TextBox).Value = "Реестр застрахованных лиц,\r\nза которых перечислены дополнительные страховые взносы\r\nна накопительную пенсию и уплачены взносы работодателя";
                (DSW3.Items.Find("textBox12", true)[0] as Telerik.Reporting.TextBox).Value = "Реквизиты работодателя, передающего реестр застрахованных лиц:";
                (DSW3.Items.Find("textBox38", true)[0] as Telerik.Reporting.TextBox).Value = "Страховой номер индивидуального лицевого счета застрахованного лица (СНИЛС)";
                (DSW3.Items.Find("textBox39", true)[0] as Telerik.Reporting.TextBox).Value = "Сумма перечисленных дополнительных страховых взносов на накопительную пенсию (руб.)";
            }

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);
            string regNum = Utils.ParseRegNum(ins.RegNum);

            (DSW3.Items.Find("RegNum", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
            (DSW3.Items.Find("NameShort", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(ins.NameShort) ? ins.NameShort : "";
            (DSW3.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN;
            (DSW3.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP;

            (DSW3.Items.Find("paymentNumber", true)[0] as Telerik.Reporting.TextBox).Value = DSW3data.NUMBERPAYMENT;

            (DSW3.Items.Find("paymentDay", true)[0] as Telerik.Reporting.TextBox).Value = DSW3data.DATEPAYMENT.Day.ToString();
            (DSW3.Items.Find("paymentMonth", true)[0] as Telerik.Reporting.TextBox).Value = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"),"{0:MMMM}", DSW3data.DATEPAYMENT);
            (DSW3.Items.Find("paymentYear", true)[0] as Telerik.Reporting.TextBox).Value = DSW3data.DATEPAYMENT.Year.ToString();

            (DSW3.Items.Find("executingDay", true)[0] as Telerik.Reporting.TextBox).Value = DSW3data.DATEEXECUTPAYMENT.Day.ToString();
            (DSW3.Items.Find("executingMonth", true)[0] as Telerik.Reporting.TextBox).Value = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", DSW3data.DATEEXECUTPAYMENT);
            (DSW3.Items.Find("executingYear", true)[0] as Telerik.Reporting.TextBox).Value = DSW3data.DATEEXECUTPAYMENT.Year.ToString();

            (DSW3.Items.Find("Year", true)[0] as Telerik.Reporting.TextBox).Value = DSW3data.YEAR.ToString();

            List<DSW3Cont> dsw3StaffList = new List<DSW3Cont> { };

            int n = 0;
            foreach (var item in DSW3data.FormsDSW_3_Staff)
            {
                n++;
                dsw3StaffList.Add(new DSW3Cont { 
                    Num = n,
                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                    SNILS = Utils.ParseSNILS(item.Staff.InsuranceNumber, item.Staff.ControlNumber),
                    SUMFEEPFR_EMPLOYERS = Utils.decToStr(item.SUMFEEPFR_EMPLOYERS.HasValue ? item.SUMFEEPFR_EMPLOYERS.Value : 0),
                    SUMFEEPFR_PAYER = Utils.decToStr(item.SUMFEEPFR_PAYER.HasValue ? item.SUMFEEPFR_PAYER.Value : 0)
                });

            }

            (DSW3.Items.Find("table2", true)[0] as Telerik.Reporting.Table).DataSource = dsw3StaffList;

            decimal SUMFEEPFR_EMPLOYERS = 0;
            decimal SUMFEEPFR_PAYER = 0;

            if (dsw3StaffList.Count > 0)
            {
                SUMFEEPFR_EMPLOYERS = DSW3data.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_EMPLOYERS.Value);
                SUMFEEPFR_PAYER = DSW3data.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_PAYER.Value);

                (DSW3.Items.Find("SUMFEEPFR_EMPLOYERS", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(SUMFEEPFR_EMPLOYERS);
                (DSW3.Items.Find("SUMFEEPFR_PAYER", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(SUMFEEPFR_PAYER);
            }

            decimal SumFee = SUMFEEPFR_EMPLOYERS + SUMFEEPFR_PAYER;
            (DSW3.Items.Find("SumFee", true)[0] as Telerik.Reporting.TextBox).Value = Utils.decToStr(SumFee);


            if (ins.BossPrint.HasValue && ins.BossPrint.Value == true)
            {
                (DSW3.Items.Find("rukDolgn1", true)[0] as Telerik.Reporting.TextBox).Value = ins.BossDolgn;
                (DSW3.Items.Find("rukName1", true)[0] as Telerik.Reporting.TextBox).Value = ins.BossFIO;
            }
            if (ins.BuchgPrint.HasValue && ins.BuchgPrint.Value == true)
            {
                (DSW3.Items.Find("buhName", true)[0] as Telerik.Reporting.TextBox).Value = ins.BuchgFIO;
            }

            (DSW3.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = DSW3data.DateFilling.ToString("dd.MM.yyyy");


            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = DSW3;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;


        }

        public void createEmptyReport()
        {
            DSW3_Report DSW3 = new DSW3_Report();

            List<DSW3Cont> dsw3StaffList = new List<DSW3Cont> { };

            for (int n = 1; n <= 20; n++ )
            {
                dsw3StaffList.Add(new DSW3Cont
                {
                    Num = n
                });

            }

            (DSW3.Items.Find("table2", true)[0] as Telerik.Reporting.Table).DataSource = dsw3StaffList;

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = DSW3;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;


        }
    }
}
