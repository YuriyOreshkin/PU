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
using PU.FormsPredPens.Report;
using System.Globalization;

namespace PU.FormsPredPens
{
    public partial class PredPensZapros_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public FormsPredPens_Zapros PredPensData { get; set; }

        public PredPensZapros_Print()
        {
            InitializeComponent();
        }

        private void PredPensZapros_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }

        private class PredPensCont
        {
            public int Num {get; set; }
            public string FIO { get; set; }
            public string SNILS { get; set; }
            public string DateBirth { get; set; }
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            PredPensZapros_Report PredPensZapros = new PredPensZapros_Report();


            if (PredPensData.TypeORG == 2)
            { 
                (PredPensZapros.Items.Find("textBox8", true)[0] as Telerik.Reporting.TextBox).Value = "Запрос" +
                    "органов в области содействия занятости населения о представлении информации " + 
                    "в соответствии со статьей 34.2 Закона Российской Федерации от 19 апреля 1991 " + 
                    "года № 1032-1 «О занятости населения в Российской Федерации»";

                (PredPensZapros.Items.Find("textBox80", true)[0] as Telerik.Reporting.TextBox).Value = "(наименование органа в области содействия занятости населения)";

                (PredPensZapros.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox).Value = "2 Запрос заверяется усиленной квалифицированной электронной подписью руководителя (заместителя руководителя) органа в области содействия занятости населения";
            }
            else if (PredPensData.TypeORG == 3)
            {
                (PredPensZapros.Items.Find("textBox8", true)[0] as Telerik.Reporting.TextBox).Value = "Запрос" +
                "Работодателя о представлении информации " +
                "в соответствии со статьей 185.1 Трудового кодекса Российской Федерации";

                (PredPensZapros.Items.Find("textBox80", true)[0] as Telerik.Reporting.TextBox).Value = "(наименование организации Работодателя)";

                (PredPensZapros.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox).Value = "2 Запрос заверяется усиленной квалифицированной электронной подписью руководителя (заместителя руководителя) организации Работодателя";
            
            }



            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            (PredPensZapros.Items.Find("Number", true)[0] as Telerik.Reporting.TextBox).Value = PredPensData.Number;

            (PredPensZapros.Items.Find("Day", true)[0] as Telerik.Reporting.TextBox).Value = PredPensData.Date.Day.ToString();
            (PredPensZapros.Items.Find("Month", true)[0] as Telerik.Reporting.TextBox).Value = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", PredPensData.Date);
            (PredPensZapros.Items.Find("Year", true)[0] as Telerik.Reporting.TextBox).Value = PredPensData.Date.Year.ToString();

            List<PredPensCont> PredPensZaprosStaffList = new List<PredPensCont> { };

            int n = 0;
            foreach (var item in PredPensData.FormsPredPens_Zapros_Staff)
            {
                string dt = "";
                if (item.Staff.DateBirth != null)
                    dt = item.Staff.DateBirth.Value.ToShortDateString();
                n++;
                PredPensZaprosStaffList.Add(new PredPensCont { 
                    Num = n,
                    FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                    SNILS = Utils.ParseSNILS(item.Staff.InsuranceNumber, item.Staff.ControlNumber),
                    DateBirth = dt,
                });

            }

            (PredPensZapros.Items.Find("table2", true)[0] as Telerik.Reporting.Table).DataSource = PredPensZaprosStaffList;

            (PredPensZapros.Items.Find("rukOrg", true)[0] as Telerik.Reporting.TextBox).Value = ins.NameShort;
            (PredPensZapros.Items.Find("rukName", true)[0] as Telerik.Reporting.TextBox).Value = PredPensData.LastName + " " + PredPensData.FirstName + " " + PredPensData.MiddleName;



            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = PredPensZapros;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;


        }

        public void createEmptyReport()
        {
            PredPensZapros_Report PredPensZapros = new PredPensZapros_Report();

            List<PredPensCont> PredPensZaprosStaffList = new List<PredPensCont> { };

            for (int n = 1; n <= 20; n++ )
            {
                PredPensZaprosStaffList.Add(new PredPensCont
                {
                    Num = n
                });

            }

            (PredPensZapros.Items.Find("table2", true)[0] as Telerik.Reporting.Table).DataSource = PredPensZaprosStaffList;

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = PredPensZapros;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;


        }
    }
}
