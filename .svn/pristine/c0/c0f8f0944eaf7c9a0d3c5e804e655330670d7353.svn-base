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
using PU.FormsADW1.Report;
using System.Globalization;

namespace PU.FormsADW1
{
    public partial class FormsADW1_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<Staff> Staff_List = new List<Staff>();

        public FormsADW1_Print()
        {
            InitializeComponent();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            ReportBook ADW1_Book = new ReportBook();

            foreach (var staff in Staff_List)
            {
                ADW1_Report ADW1 = new ADW1_Report();
                FormsADW_1 adw_1 = db.FormsADW_1.FirstOrDefault(x => x.StaffID == staff.ID);

                (ADW1.Items.Find("LN", true)[0] as Telerik.Reporting.TextBox).Value = staff.LastName;
                (ADW1.Items.Find("FN", true)[0] as Telerik.Reporting.TextBox).Value = staff.FirstName;
                (ADW1.Items.Find("MN", true)[0] as Telerik.Reporting.TextBox).Value = staff.MiddleName;

                (ADW1.Items.Find("Sex", true)[0] as Telerik.Reporting.TextBox).Value = staff.Sex.HasValue ? (staff.Sex.Value.ToString() == "0" ? "М" : "Ж") : "";


                string d = "";
                string m = "";
                string y = "";
                if (adw_1 != null)
                {
                    if (adw_1.Type_DateBirth.HasValue && adw_1.Type_DateBirth.Value == 1)
                    {
                        d = adw_1.DateBirthDay_Os.HasValue ? adw_1.DateBirthDay_Os.Value.ToString().PadLeft(2, '0') : "";
                        m = adw_1.DateBirthMonth_Os.HasValue ? String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", adw_1.DateBirthMonth_Os.Value) : "";
                        y = adw_1.DateBirthMonth_Os.HasValue ? adw_1.DateBirthYear_Os.Value.ToString() : "";
                    }
                    else
                    {
                        if (staff.DateBirth.HasValue)
                        {
                            d = staff.DateBirth.Value.Day.ToString().PadLeft(2, '0');
                            m = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", staff.DateBirth.Value);
                            y = staff.DateBirth.Value.Year.ToString();
                        }
                    }

                }
                else
                {
                    if (staff.DateBirth.HasValue)
                    {
                        d = staff.DateBirth.Value.Day.ToString().PadLeft(2, '0');
                        m = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", staff.DateBirth.Value);
                        y = staff.DateBirth.Value.Year.ToString();
                    }
                }

                (ADW1.Items.Find("date_birth_d", true)[0] as Telerik.Reporting.TextBox).Value = d;
                (ADW1.Items.Find("date_birth_m", true)[0] as Telerik.Reporting.TextBox).Value = m;
                (ADW1.Items.Find("date_birth_y", true)[0] as Telerik.Reporting.TextBox).Value = y;


                if (adw_1 != null)
                {
                    (ADW1.Items.Find("punkt", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Punkt.Trim();
                    (ADW1.Items.Find("distr", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Distr.Trim();
                    (ADW1.Items.Find("region", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Region.Trim();
                    (ADW1.Items.Find("country", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Country.Trim();
                    (ADW1.Items.Find("citizenship", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Citizenship.Trim();

                    (ADW1.Items.Find("reg_addr", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Reg_Addr.Trim();
                    (ADW1.Items.Find("fakt_addr", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Fakt_Addr.Trim();

                    (ADW1.Items.Find("phone", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Phone.Trim();

                    (ADW1.Items.Find("docname", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Doc_Type_ID.HasValue ? db.DocumentTypes.FirstOrDefault(x => x.ID == adw_1.Doc_Type_ID.Value).Code : adw_1.Doc_Name.Trim();
                    (ADW1.Items.Find("sernum", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Ser_Lat + "-" + adw_1.Ser_Rus + " № " + adw_1.Doc_Num;

                    if (adw_1.Doc_Date.HasValue)
                    {
                        (ADW1.Items.Find("date_vyd_d", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Doc_Date.Value.Day.ToString("d");
                        (ADW1.Items.Find("date_vyd_m", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Doc_Date.Value.ToString("MMMM");
                        (ADW1.Items.Find("date_vyd_y", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Doc_Date.Value.Year.ToString();
                    }

                    (ADW1.Items.Find("kem_vyd", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Doc_Kem_Vyd.Trim();

                    (ADW1.Items.Find("dateFilling", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.DateFilling.Value.ToString("dd MMMM yyyy");
                }

                var typeReportSource = new Telerik.Reporting.InstanceReportSource();

                typeReportSource.ReportDocument = ADW1;
                this.reportViewer1.ReportSource = typeReportSource;
                this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            }

        }

        private void FormsADW1_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }
    }
}
