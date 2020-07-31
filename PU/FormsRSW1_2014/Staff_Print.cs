using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using Telerik.Reporting;
using PU.Classes;
using PU.FormsRSW1_2014.Report;
using Telerik.Reporting.Drawing;

namespace PU.FormsRSW2014
{
    public partial class Staff_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        pfrXMLEntities dbxml = new pfrXMLEntities();
        public List<xmlInfo> Staff_data;

        
        public Staff_Print()
        {
            InitializeComponent();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            List<Telerik.Reporting.Report> detailReports = new List<Telerik.Reporting.Report>();

            if (Staff_data.First().InsurerID.HasValue)
            {
                long insID = Staff_data.First().InsurerID.Value;
                Insurer Ins = db.Insurer.FirstOrDefault(x => x.ID == insID);


                foreach (var item in Staff_data.Where(x => x.CountStaff > 0))
                {
                    Staff_Report Staff = new Staff_Report();

                    if (Ins != null)
                    {
                        Telerik.Reporting.TextBox tb = new Telerik.Reporting.TextBox { };

                        if (Ins.PerformerPrint.HasValue && Ins.PerformerPrint.Value == true)
                        {
                            tb = Staff.Items.Find("ispolnName", true)[0] as Telerik.Reporting.TextBox;
                            tb.Value = Ins.PerformerFIO;
                            tb = Staff.Items.Find("ispolnDolgn", true)[0] as Telerik.Reporting.TextBox;
                            tb.Value = Ins.PerformerDolgn;
                        }

                        if (Ins.BossPrint.HasValue && Ins.BossPrint.Value == true)
                        {
                            tb = Staff.Items.Find("rukName1", true)[0] as Telerik.Reporting.TextBox;
                            tb.Value = Ins.BossFIO;
                            tb = Staff.Items.Find("rukDolgn1", true)[0] as Telerik.Reporting.TextBox;
                            tb.Value = Ins.BossDolgn;
                        }
                        if (Ins.BossDopPrint.HasValue && Ins.BossDopPrint.Value == true)
                        {
                            tb = Staff.Items.Find("rukName2", true)[0] as Telerik.Reporting.TextBox;
                            tb.Value = Ins.BossFIODop;
                            tb = Staff.Items.Find("rukDolgn2", true)[0] as Telerik.Reporting.TextBox;
                            tb.Value = Ins.BossDolgnDop;
                        }
                    }

                    Telerik.Reporting.TextBox Num = Staff.Items.Find("textBox3", true)[0] as Telerik.Reporting.TextBox;
                    Num.Value = item.Num != null ? item.Num.ToString() : "";

                    List<StaffList> SL = dbxml.StaffList.Where(x => x.XmlInfoID == item.ID).OrderBy(y => y.Num).ToList();
                    Telerik.Reporting.Table table1 = Staff.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

                    table1.DataSource = SL;

                    Telerik.Reporting.TextBox CountDoc = Staff.Items.Find("textBox5", true)[0] as Telerik.Reporting.TextBox;
                    CountDoc.Value = item.CountDoc != null ? item.Num.ToString() + " - " + item.CountDoc.ToString() : "";
                    Telerik.Reporting.TextBox date = Staff.Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
                    date.Value = DateTime.Now.ToShortDateString();

                    detailReports.Add(Staff);
                }
            }

            Telerik.Reporting.Report report = Reports.ReportMethods.GetCombinedReport(detailReports);

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();
            typeReportSource.ReportDocument = report;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;

        }



        private void Staff_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }
    }
}
