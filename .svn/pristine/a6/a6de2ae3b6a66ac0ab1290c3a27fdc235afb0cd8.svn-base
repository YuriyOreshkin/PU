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
using PU.FormsADW2.Report;
using System.Globalization;

namespace PU.FormsADW2
{
    public partial class FormsADW2_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<FormsADW_2> ADW2_List = new List<FormsADW_2>();

        public FormsADW2_Print()
        {
            InitializeComponent();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            ReportBook ADW2_Book = new ReportBook();

            foreach (var adw_2 in ADW2_List)
            {
                ADW2_Report ADW2 = new ADW2_Report();

                Staff staff = db.Staff.First(x => x.ID == adw_2.StaffID);
             //   FormsADW_1 adw_2 = db.FormsADW_1.FirstOrDefault(x => x.StaffID == staff.ID);

                (ADW2.Items.Find("LN", true)[0] as Telerik.Reporting.TextBox).Value = staff.LastName;
                (ADW2.Items.Find("FN", true)[0] as Telerik.Reporting.TextBox).Value = staff.FirstName;
                (ADW2.Items.Find("MN", true)[0] as Telerik.Reporting.TextBox).Value = staff.MiddleName;

                (ADW2.Items.Find("SNILS", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber.HasValue ? staff.ControlNumber.Value : (short)100) : "";


                (ADW2.Items.Find("LNnew", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.LastName;
                (ADW2.Items.Find("FNnew", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.FirstName;
                (ADW2.Items.Find("MNnew", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.MiddleName;

                (ADW2.Items.Find("Sex", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Sex.HasValue ? (adw_2.Sex.Value.ToString() == "0" ? "М" : "Ж") : "";


                string d = "";
                string m = "";
                string y = "";
                if (adw_2 != null)
                {
                    if (adw_2.Type_DateBirth.HasValue && adw_2.Type_DateBirth.Value == 1)
                    {
                        d = adw_2.DateBirthDay_Os.HasValue ? adw_2.DateBirthDay_Os.Value.ToString().PadLeft(2, '0') : "";
                        m = adw_2.DateBirthMonth_Os.HasValue ? String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", adw_2.DateBirthMonth_Os.Value) : "";
                        y = adw_2.DateBirthMonth_Os.HasValue ? adw_2.DateBirthYear_Os.Value.ToString() : "";
                    }
                    else
                    {
                        if (adw_2.DateBirth.HasValue)
                        {
                            d = adw_2.DateBirth.Value.Day.ToString().PadLeft(2, '0');
                            m = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", adw_2.DateBirth.Value);
                            y = adw_2.DateBirth.Value.Year.ToString();
                        }
                    }

                }
                else
                {
                    if (adw_2.DateBirth.HasValue)
                    {
                        d = adw_2.DateBirth.Value.Day.ToString().PadLeft(2, '0');
                        m = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", adw_2.DateBirth.Value);
                        y = adw_2.DateBirth.Value.Year.ToString();
                    }
                }

                (ADW2.Items.Find("date_birth_d", true)[0] as Telerik.Reporting.TextBox).Value = d;
                (ADW2.Items.Find("date_birth_m", true)[0] as Telerik.Reporting.TextBox).Value = m;
                (ADW2.Items.Find("date_birth_y", true)[0] as Telerik.Reporting.TextBox).Value = y;


                if (adw_2 != null)
                {
                    (ADW2.Items.Find("punkt", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Punkt.Trim();
                    (ADW2.Items.Find("distr", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Distr.Trim();
                    (ADW2.Items.Find("region", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Region.Trim();
                    (ADW2.Items.Find("country", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Country.Trim();
                    (ADW2.Items.Find("citizenship", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Citizenship.Trim();

                    (ADW2.Items.Find("reg_addr", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Reg_Addr.Trim();
                    (ADW2.Items.Find("fakt_addr", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Fakt_Addr.Trim();

                    (ADW2.Items.Find("phone", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Phone.Trim();

                    (ADW2.Items.Find("docname", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Doc_Type_ID.HasValue ? db.DocumentTypes.FirstOrDefault(x => x.ID == adw_2.Doc_Type_ID.Value).Code : adw_2.Doc_Name.Trim();
                    (ADW2.Items.Find("sernum", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Ser_Lat + "-" + adw_2.Ser_Rus + " № " + adw_2.Doc_Num;

                    if (adw_2.Doc_Date.HasValue)
                    {
                        (ADW2.Items.Find("date_vyd_d", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Doc_Date.Value.Day.ToString("d");
                        (ADW2.Items.Find("date_vyd_m", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Doc_Date.Value.ToString("MMMM");
                        (ADW2.Items.Find("date_vyd_y", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Doc_Date.Value.Year.ToString();
                    }

                    (ADW2.Items.Find("kem_vyd", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.Doc_Kem_Vyd.Trim();

                    (ADW2.Items.Find("dateFilling", true)[0] as Telerik.Reporting.TextBox).Value = adw_2.DateFilling.Value.ToString("dd MMMM yyyy");
                }

                var typeReportSource = new Telerik.Reporting.InstanceReportSource();

                typeReportSource.ReportDocument = ADW2;
                this.reportViewer1.ReportSource = typeReportSource;
                this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            }

        }

        private void FormsADW2_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }
    }
}
