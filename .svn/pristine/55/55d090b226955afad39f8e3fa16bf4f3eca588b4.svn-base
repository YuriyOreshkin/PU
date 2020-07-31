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
using PU.FormsADW3.Report;
using System.Globalization;
using System.Linq;

namespace PU.FormsADW3
{
    public partial class FormsADW3_Print : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<FormsADW_3> ADW3_List = new List<FormsADW_3>();

        public FormsADW3_Print()
        {
            InitializeComponent();
        }

        public void createReport(object sender, DoWorkEventArgs e)
        {
            ReportBook ADW3_Book = new ReportBook();

            foreach (var adw_3 in ADW3_List)
            {
                ADW3_Report ADW3 = new ADW3_Report();

                Staff staff = db.Staff.First(x => x.ID == adw_3.StaffID);
                FormsADW_1 adw_1 = db.FormsADW_1.FirstOrDefault(x => x.StaffID == staff.ID);

                (ADW3.Items.Find("LN", true)[0] as Telerik.Reporting.TextBox).Value = staff.LastName;
                (ADW3.Items.Find("FN", true)[0] as Telerik.Reporting.TextBox).Value = staff.FirstName;
                (ADW3.Items.Find("MN", true)[0] as Telerik.Reporting.TextBox).Value = staff.MiddleName;
                (ADW3.Items.Find("Sex", true)[0] as Telerik.Reporting.TextBox).Value = staff.Sex.HasValue ? (staff.Sex.Value.ToString() == "0" ? "М" : "Ж") : "";

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

                (ADW3.Items.Find("date_birth_d", true)[0] as Telerik.Reporting.TextBox).Value = d;
                (ADW3.Items.Find("date_birth_m", true)[0] as Telerik.Reporting.TextBox).Value = m;
                (ADW3.Items.Find("date_birth_y", true)[0] as Telerik.Reporting.TextBox).Value = y;


                if (adw_1 != null)
                {
                    (ADW3.Items.Find("punkt", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Punkt.Trim();
                    (ADW3.Items.Find("distr", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Distr.Trim();
                    (ADW3.Items.Find("region", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Region.Trim();
                    (ADW3.Items.Find("country", true)[0] as Telerik.Reporting.TextBox).Value = adw_1.Country.Trim();
                }





                (ADW3.Items.Find("LNnew", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.LastName;
                (ADW3.Items.Find("FNnew", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.FirstName;
                (ADW3.Items.Find("MNnew", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.MiddleName;

                (ADW3.Items.Find("Sex_new", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Sex.HasValue ? (adw_3.Sex.Value.ToString() == "0" ? "М" : "Ж") : "";


                d = "";
                m = "";
                y = "";
                if (adw_3 != null)
                {
                    if (adw_3.Type_DateBirth.HasValue && adw_3.Type_DateBirth.Value == 1)
                    {
                        d = adw_3.DateBirthDay_Os.HasValue ? adw_3.DateBirthDay_Os.Value.ToString().PadLeft(2, '0') : "";
                        m = adw_3.DateBirthMonth_Os.HasValue ? String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", adw_3.DateBirthMonth_Os.Value) : "";
                        y = adw_3.DateBirthMonth_Os.HasValue ? adw_3.DateBirthYear_Os.Value.ToString() : "";
                    }
                    else
                    {
                        if (adw_3.DateBirth.HasValue)
                        {
                            d = adw_3.DateBirth.Value.Day.ToString().PadLeft(2, '0');
                            m = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", adw_3.DateBirth.Value);
                            y = adw_3.DateBirth.Value.Year.ToString();
                        }
                    }

                }
                else
                {
                    if (adw_3.DateBirth.HasValue)
                    {
                        d = adw_3.DateBirth.Value.Day.ToString().PadLeft(2, '0');
                        m = String.Format(CultureInfo.CreateSpecificCulture("ru-RU"), "{0:MMMM}", adw_3.DateBirth.Value);
                        y = adw_3.DateBirth.Value.Year.ToString();
                    }
                }

                (ADW3.Items.Find("date_birth_d_new", true)[0] as Telerik.Reporting.TextBox).Value = d;
                (ADW3.Items.Find("date_birth_m_new", true)[0] as Telerik.Reporting.TextBox).Value = m;
                (ADW3.Items.Find("date_birth_y_new", true)[0] as Telerik.Reporting.TextBox).Value = y;


                if (adw_3 != null)
                {
                    (ADW3.Items.Find("punkt_new", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Punkt.Trim();
                    (ADW3.Items.Find("distr_new", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Distr.Trim();
                    (ADW3.Items.Find("region_new", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Region.Trim();
                    (ADW3.Items.Find("country_new", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Country.Trim();
                    (ADW3.Items.Find("citizenship", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Citizenship.Trim();

                    (ADW3.Items.Find("reg_addr", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Reg_Addr.Trim();
                    (ADW3.Items.Find("fakt_addr", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Fakt_Addr.Trim();

                    (ADW3.Items.Find("phone", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Phone.Trim();

                    (ADW3.Items.Find("docname", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Doc_Type_ID.HasValue ? db.DocumentTypes.FirstOrDefault(x => x.ID == adw_3.Doc_Type_ID.Value).Code : adw_3.Doc_Name.Trim();
                    (ADW3.Items.Find("sernum", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Ser_Lat + "-" + adw_3.Ser_Rus + " № " + adw_3.Doc_Num;

                    if (adw_3.Doc_Date.HasValue)
                    {
                        (ADW3.Items.Find("date_vyd_d", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Doc_Date.Value.Day.ToString("d");
                        (ADW3.Items.Find("date_vyd_m", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Doc_Date.Value.ToString("MMMM");
                        (ADW3.Items.Find("date_vyd_y", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Doc_Date.Value.Year.ToString();
                    }

                    (ADW3.Items.Find("kem_vyd", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.Doc_Kem_Vyd.Trim();

                    (ADW3.Items.Find("dateFilling", true)[0] as Telerik.Reporting.TextBox).Value = adw_3.DateFilling.Value.ToString("dd MMMM yyyy");
                }


                ADW3_Report_page_2 ADW3_2 = new ADW3_Report_page_2();
                (ADW3_2.Items.Find("SNILS", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber.HasValue ? staff.ControlNumber.Value : (short)100) : "";


                var typeReportSource = new Telerik.Reporting.InstanceReportSource();
                ADW3_Book.Reports.Add(ADW3);
                ADW3_Book.Reports.Add(ADW3_2);
                typeReportSource.ReportDocument = ADW3_Book;
                this.reportViewer1.ReportSource = typeReportSource;
                this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            }

        }

        private void FormsADW3_Print_Load(object sender, EventArgs e)
        {
            reportViewer1.RefreshReport();
        }
    }
}
