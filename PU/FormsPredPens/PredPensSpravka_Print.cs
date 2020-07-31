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
    public partial class PredPensSpravka_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<SPPVObject> ListSPPV { get; set; }

        public FormsPredPens_Zapros PredPensData { get; set; }


        public PredPensSpravka_Print()
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
            ReportBook Rep_Book = new ReportBook();

            foreach (var sppv in ListSPPV)
            {
                PredPensSpravka_Report PredPensSpravka = new PredPensSpravka_Report();

                string dt = DateTime.Parse(sppv.Дата).ToShortDateString();

                (PredPensSpravka.Items.Find("dateTextBox", true)[0] as Telerik.Reporting.TextBox).Value = dt;

                switch (sppv.Орган)
                {
                    case 1:
                        (PredPensSpravka.Items.Find("OrgTextBox", true)[0] as Telerik.Reporting.TextBox).Value = "в Федеральную налоговую службу Российской Федерации; иной федеральный орган исполнительной власти Российской Федерации";
                        break;
                    case 2:
                        (PredPensSpravka.Items.Find("OrgTextBox", true)[0] as Telerik.Reporting.TextBox).Value = "орган государственной власти субъекта Российской Федерации в области содействия занятости населения";
                        break;
                    case 3:
                        (PredPensSpravka.Items.Find("OrgTextBox", true)[0] as Telerik.Reporting.TextBox).Value = "работодатель";
                        break;
                    case 4:
                        (PredPensSpravka.Items.Find("OrgTextBox", true)[0] as Telerik.Reporting.TextBox).Value = "иной орган исполнительной власти субъекта Российской Федерации; орган местного самоуправления";
                        break;
                }


                (PredPensSpravka.Items.Find("LNtextBox", true)[0] as Telerik.Reporting.TextBox).Value = sppv.Фамилия;
                (PredPensSpravka.Items.Find("FNtextBox", true)[0] as Telerik.Reporting.TextBox).Value = sppv.Имя;
                (PredPensSpravka.Items.Find("MNtextBox", true)[0] as Telerik.Reporting.TextBox).Value = sppv.Отчество;

                dt = "";

                try
                {
                    dt = DateTime.Parse(sppv.ДатаРождения).ToShortDateString();
                }
                catch
                {
                    dt = sppv.ДатаРождения;
                }

                (PredPensSpravka.Items.Find("DateBirthtextBox", true)[0] as Telerik.Reporting.TextBox).Value = dt;

                (PredPensSpravka.Items.Find("SNILStextBox", true)[0] as Telerik.Reporting.TextBox).Value = sppv.СНИЛС;


                string docum = "";

                if (sppv.Статья != "")
                {
                    docum = "Статья " + sppv.Статья + "  ";
                }

                docum += sppv.НормативныйДокумент;


                (PredPensSpravka.Items.Find("CattextBox", true)[0] as Telerik.Reporting.TextBox).Value = docum;


                switch (sppv.ЯвляетсяГражданиномПредпенсионногоВозраста)
                {
                    case 0:
                        (PredPensSpravka.Items.Find("NOtextBox", true)[0] as Telerik.Reporting.TextBox).Value = "V";
                        break;
                    case 1:
                        (PredPensSpravka.Items.Find("YEStextBox", true)[0] as Telerik.Reporting.TextBox).Value = "V";
                        break;
                }

                if (!String.IsNullOrEmpty(sppv.ДатаС))
                {
                    (PredPensSpravka.Items.Find("list8", true)[0] as Telerik.Reporting.List).Visible = true;
                    (PredPensSpravka.Items.Find("dateFrom", true)[0] as Telerik.Reporting.TextBox).Value = sppv.ДатаС;
                }


                (PredPensSpravka.Items.Find("rukName", true)[0] as Telerik.Reporting.TextBox).Value = sppv.ДИмя + " " + sppv.ДОтчество + " " + sppv.ДФамилия;

                (PredPensSpravka.Items.Find("rukOrg", true)[0] as Telerik.Reporting.TextBox).Value = sppv.Должность;


                Rep_Book.Reports.Add(PredPensSpravka);
            }



            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = Rep_Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;


        }

    }
}
