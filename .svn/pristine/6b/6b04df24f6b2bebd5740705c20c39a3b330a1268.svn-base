using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.Reporting;
using PU.Models;
using PU.Classes;
using PU.FormsRSW1_2014.Report;

namespace PU.FormsRSW2014
{
    public partial class ADV_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<xmlInfo> ADVdata;
        
        public ADV_Print()
        {
            InitializeComponent();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            ReportBook ADV_Book = new ReportBook();
            
            foreach (var item in ADVdata)
            {
                Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == item.InsurerID);
                string regNum = Utils.ParseRegNum(ins.RegNum);

                ADV_Report ADV = new ADV_Report();

                if (ins != null)
                {
                    Telerik.Reporting.TextBox tb = new Telerik.Reporting.TextBox { };

                    if (ins.PerformerPrint.HasValue && ins.PerformerPrint.Value == true)
                    {
                        tb = ADV.Items.Find("ispolnName", true)[0] as Telerik.Reporting.TextBox;
                        tb.Value = ins.PerformerFIO;
                        tb = ADV.Items.Find("ispolnDolgn", true)[0] as Telerik.Reporting.TextBox;
                        tb.Value = ins.PerformerDolgn;
                    }

                    if (ins.BossPrint.HasValue && ins.BossPrint.Value == true)
                    {
                        tb = ADV.Items.Find("rukName1", true)[0] as Telerik.Reporting.TextBox;
                        tb.Value = ins.BossFIO;
                        tb = ADV.Items.Find("rukDolgn1", true)[0] as Telerik.Reporting.TextBox;
                        tb.Value = ins.BossDolgn;
                    }
                    if (ins.BossDopPrint.HasValue && ins.BossDopPrint.Value == true)
                    {
                        tb = ADV.Items.Find("rukName2", true)[0] as Telerik.Reporting.TextBox;
                        tb.Value = ins.BossFIODop;
                        tb = ADV.Items.Find("rukDolgn2", true)[0] as Telerik.Reporting.TextBox;
                        tb.Value = ins.BossDolgnDop;
                    }
                }


                Telerik.Reporting.TextBox OKPO = ADV.Items.Find("textBox5", true)[0] as Telerik.Reporting.TextBox;
                OKPO.Value = ins.OKPO != null ? ins.OKPO.ToString() : "";
                Telerik.Reporting.TextBox RegNum_title = ADV.Items.Find("textBox6", true)[0] as Telerik.Reporting.TextBox;
                RegNum_title.Value = regNum;
                Telerik.Reporting.TextBox NameShort = ADV.Items.Find("textBox7", true)[0] as Telerik.Reporting.TextBox;
                NameShort.Value = ins.NameShort != null ? ins.NameShort.ToString() : "";
                Telerik.Reporting.TextBox INN = ADV.Items.Find("textBox11", true)[0] as Telerik.Reporting.TextBox;
                INN.Value = ins.INN != null ? ins.INN.ToString() : "";
                Telerik.Reporting.TextBox KPP = ADV.Items.Find("textBox12", true)[0] as Telerik.Reporting.TextBox;
                KPP.Value = ins.KPP != null ? ins.KPP.ToString() : "";

                Telerik.Reporting.TextBox CountDoc = ADV.Items.Find("textBox25", true)[0] as Telerik.Reporting.TextBox;
                CountDoc.Value = item.CountDoc != null ? item.CountDoc.ToString() : "";
                Telerik.Reporting.TextBox Num = ADV.Items.Find("textBox42", true)[0] as Telerik.Reporting.TextBox;
                Num.Value = item.Num != null ? item.Num.ToString() : "";

                Telerik.Reporting.TextBox date = ADV.Items.Find("textBox84", true)[0] as Telerik.Reporting.TextBox;
                date.Value = DateTime.Now.ToShortDateString();

                ADV_Book.Reports.Add(ADV);
            }

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = ADV_Book;
            this.reportViewer1.ReportSource = typeReportSource;
        }

        private void ADV_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }

        
    }
}
