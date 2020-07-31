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
using Telerik.Reporting;
using PU.Classes;
using Telerik.Reporting.Drawing;
using PU.FormsSZV_TD.Report;

namespace PU.FormsSZV_TD
{
    public partial class SZV_TD_Print : Telerik.WinControls.UI.RadForm
    {
        public List<FormsSZV_STAJ_2017> SZV_STAJ_List = new List<FormsSZV_STAJ_2017>();
        public List<FormsSZV_ISH_2017> SZV_ISH_List = new List<FormsSZV_ISH_2017>();
        public List<FormsSZV_KORR_2017> SZV_KORR_List = new List<FormsSZV_KORR_2017>();


        public List<FormsSZV_TD_2020_Staff> SZV_TD_2020_List = new List<FormsSZV_TD_2020_Staff>();
        public List<long> szvtd_staff_ids_list = new List<long>();
        public FormsSZV_TD_2020 SZVTD_Data { get; set; }
        
        private Insurer ins;
        private string regNum { get; set; }
        pu6Entities db = new pu6Entities();

        public class MonthesDict
        {
            public byte Code { get; set; }
            public string Name { get; set; }
        }

        List<MonthesDict> Monthes = new List<MonthesDict> { 
            new MonthesDict{Code = 1, Name = "01 - январь"},
            new MonthesDict{Code = 2, Name = "02 - февраль"},
            new MonthesDict{Code = 3, Name = "03 - март"},
            new MonthesDict{Code = 4, Name = "04 - апрель"},
            new MonthesDict{Code = 5, Name = "05 - май"},
            new MonthesDict{Code = 6, Name = "06 - июнь"},
            new MonthesDict{Code = 7, Name = "07 - июль"},
            new MonthesDict{Code = 8, Name = "08 - август"},
            new MonthesDict{Code = 9, Name = "09 - сентябрь"},
            new MonthesDict{Code = 10, Name = "10 - октябрь"},
            new MonthesDict{Code = 11, Name = "11 - ноябрь"},
            new MonthesDict{Code = 12, Name = "12 - декабрь"}
        };




        public SZV_TD_Print()
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



            if (SZVTD_Data != null)
            {
                var SZV_TD_2020_List_t = db.FormsSZV_TD_2020_Staff.Where(x => x.FormsSZV_TD_2020_ID == SZVTD_Data.ID);
                if (szvtd_staff_ids_list.Count > 0)
                    SZV_TD_2020_List_t = SZV_TD_2020_List_t.Where(x => szvtd_staff_ids_list.Contains(x.ID));

                SZV_TD_2020_List = SZV_TD_2020_List_t.OrderBy(x => x.Staff.LastName).ThenBy(x => x.Staff.FirstName).ThenBy(x => x.Staff.MiddleName).ToList();
            }



            foreach (var item in SZV_TD_2020_List)
            {
                Rep_Book.Reports.Add(SZV_TD_Report(item));
            }





            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            db.Dispose();
            db = null;

            typeReportSource.ReportDocument = Rep_Book;
            this.reportViewer1.ReportSource = typeReportSource;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            //this.reportViewer1.RefreshReport();

        }

        public class szvtdTable
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
            public string col10 { get; set; }
        }

        private SZV_TD_Rep SZV_TD_Report(FormsSZV_TD_2020_Staff item)
        {
            SZV_TD_Rep SZVTD = new SZV_TD_Rep();

            Staff staff = db.Staff.Single(x => x.ID == item.StaffID);

            (SZVTD.Items.Find("regNum", true)[0] as Telerik.Reporting.TextBox).Value = regNum;
            (SZVTD.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = ins.INN != null ? ins.INN.ToString() : "";
            (SZVTD.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = ins.KPP != null ? ins.KPP.ToString() : "";

            string orgName = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    orgName = ins.NameShort;
                }
                else if (!String.IsNullOrEmpty(ins.Name))
                {
                    orgName = ins.Name;
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

            (SZVTD.Items.Find("nameShort", true)[0] as Telerik.Reporting.TextBox).Value = orgName;


            (SZVTD.Items.Find("lastName", true)[0] as Telerik.Reporting.TextBox).Value = staff.LastName;
            (SZVTD.Items.Find("firstName", true)[0] as Telerik.Reporting.TextBox).Value = staff.FirstName;
            (SZVTD.Items.Find("middleName", true)[0] as Telerik.Reporting.TextBox).Value = staff.MiddleName;
            (SZVTD.Items.Find("SNILS", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(staff.InsuranceNumber) ? Utils.ParseSNILS(staff.InsuranceNumber.ToString(), (staff.ControlNumber.HasValue ? staff.ControlNumber.Value : (short)0)) : "";
            (SZVTD.Items.Find("dateBirth", true)[0] as Telerik.Reporting.TextBox).Value = staff.DateBirth.HasValue ? staff.DateBirth.Value.ToShortDateString() : "";



            if (SZVTD_Data != null)
            {
                (SZVTD.Items.Find("Year", true)[0] as Telerik.Reporting.TextBox).Value = SZVTD_Data.Year != null ? SZVTD_Data.Year.ToString() : "";

                var month = "";
                if (Monthes.Any(x => x.Code == SZVTD_Data.Month))
                {
                    month = Monthes.First(x => x.Code == SZVTD_Data.Month).Name;
                }

                (SZVTD.Items.Find("Month", true)[0] as Telerik.Reporting.TextBox).Value = month;

                (SZVTD.Items.Find("ConfirmDolgn", true)[0] as Telerik.Reporting.TextBox).Value = SZVTD_Data.ConfirmDolgn;
                (SZVTD.Items.Find("ConfirmFIO", true)[0] as Telerik.Reporting.TextBox).Value = SZVTD_Data.ConfirmLastName + " " + SZVTD_Data.ConfirmFirstName + " " + SZVTD_Data.ConfirmMiddleName;
                (SZVTD.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = SZVTD_Data.DateFilling.ToString("dd.MM.yyyy");

            }


            (SZVTD.Items.Find("ZayavOProdoljDate", true)[0] as Telerik.Reporting.TextBox).Value = item.ZayavOProdoljDate.HasValue ? item.ZayavOProdoljDate.Value.ToString("dd.MM.yyyy") : "";
            (SZVTD.Items.Find("ZayavOProdoljState", true)[0] as Telerik.Reporting.TextBox).Value = (item.ZayavOProdoljState.HasValue && item.ZayavOProdoljState.Value == 2) ? "X" : "";

            (SZVTD.Items.Find("ZayavOPredostDate", true)[0] as Telerik.Reporting.TextBox).Value = item.ZayavOPredostDate.HasValue ? item.ZayavOPredostDate.Value.ToString("dd.MM.yyyy") : "";
            (SZVTD.Items.Find("ZayavOPredostState", true)[0] as Telerik.Reporting.TextBox).Value = (item.ZayavOPredostState.HasValue && item.ZayavOPredostState.Value == 2) ? "X" : "";





            var FormsSZV_TD_2020_Staff_Events_List = item.FormsSZV_TD_2020_Staff_Events.OrderBy(x => x.DateOfEvent).ToList();

            List<szvtdTable> SZVTDEventsList = new List<szvtdTable>();

            int i = 1;

            foreach (var event_item in FormsSZV_TD_2020_Staff_Events_List)
            {

                    bool StatPunkt = !String.IsNullOrEmpty(event_item.Statya) || !String.IsNullOrEmpty(event_item.Punkt);

                    string uvol = "";

                    if (StatPunkt)
                    {
                        uvol = (!String.IsNullOrEmpty(event_item.Statya) ? "\r\nСт. " + event_item.Statya.Trim() : "") + (!String.IsNullOrEmpty(event_item.Punkt) ? "\r\nП. " + event_item.Punkt.Trim() : "");
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(event_item.OsnUvolName))
                        {
                            uvol += event_item.OsnUvolName.Trim() + "\r\n";
                        }
                        if (!String.IsNullOrEmpty(event_item.OsnUvolStartya))
                        {
                            uvol += "Ст. " + event_item.OsnUvolStartya.Trim() + "\r\n";
                        }
                        if (!String.IsNullOrEmpty(event_item.OsnUvolChyast))
                        {
                            uvol += "Ч. " + event_item.OsnUvolChyast.Trim() + "\r\n";
                        }
                        if (!String.IsNullOrEmpty(event_item.OsnUvolPunkt))
                        {
                            uvol += "П. " + event_item.OsnUvolPunkt.Trim() + "\r\n";
                        }
                        if (!String.IsNullOrEmpty(event_item.OsnUvolPodPunkt))
                        {
                            uvol += "П.п. " + event_item.OsnUvolPodPunkt.Trim() + "\r\n";
                        }

                        if (!String.IsNullOrEmpty(uvol))
                        {
                            uvol = "\r\n" + uvol;
                        }
                    }


                string osn1 = event_item.OsnName1;
                string osn2 = !String.IsNullOrEmpty(event_item.OsnName2) ? ("\r\n\r\n" + event_item.OsnName2) : "";

                string osndate1 = event_item.OsnDate1.HasValue ? event_item.OsnDate1.Value.ToShortDateString() : "";
                string osndate2 = !String.IsNullOrEmpty(event_item.OsnName2) ? ("\r\n\r\n" + (event_item.OsnDate2.HasValue ? event_item.OsnDate2.Value.ToShortDateString() : "")) : "";

                string osnnum1 = (!String.IsNullOrEmpty(event_item.OsnNum1) ? ("№" + event_item.OsnNum1.Trim()) : "") + (!String.IsNullOrEmpty(event_item.OsnSer1) ? (" Серия: " + event_item.OsnSer1.Trim()) : "");
                string osnnum2 = !String.IsNullOrEmpty(event_item.OsnName2) ? ("\r\n\r\n" + "№ " + event_item.OsnNum2.Trim() + (!String.IsNullOrEmpty(event_item.OsnSer2) ? (" Серия: " + event_item.OsnSer2.Trim()) : "")) : "";



                SZVTDEventsList.Add(new szvtdTable { 
                    col1 = i.ToString(),
                    col2 = event_item.DateOfEvent.ToShortDateString(),
                    col3 = event_item.FormsSZV_TD_2020_TypesOfEvents.Name,
                    col4 = String.Format("{0}{1}{2}{3}\r\nСовместитель: {4}{5}{6}", event_item.Dolgn, !String.IsNullOrEmpty(event_item.VydPoruchRaboty) ? ("\r\n" + event_item.VydPoruchRaboty) : "", !String.IsNullOrEmpty(event_item.Department) ? ("\r\n" + event_item.Department) : "", !String.IsNullOrEmpty(event_item.Svedenia) ? ("\r\n" + event_item.Svedenia) : "", event_item.Sovmestitel == 0 ? "НЕТ" : "ДА", event_item.DateFrom.HasValue ? ("\r\nС - " + event_item.DateFrom.Value.ToShortDateString()) : "", event_item.DateTo.HasValue ? ("\r\nПО - " + event_item.DateTo.Value.ToShortDateString()) : ""),
                    col5 = event_item.KodVypFunc,
                    col6 = String.Format("{0}{1}", !String.IsNullOrEmpty(event_item.Prichina) ? ("Причина: " + event_item.Prichina) : "", uvol),
                    col7 = osn1 + osn2,
                    col8 = osndate1 + osndate2,
                    col9 = osnnum1 + osnnum2,
                    col10 = (event_item.Annuled.HasValue && event_item.Annuled.Value) ? "Х" + (event_item.AnnuleDate.HasValue ? "\r\n" + event_item.AnnuleDate.Value.ToShortDateString() : "") : ""
                });


                i++;
            }

            Telerik.Reporting.Table table = SZVTD.Items.Find("table1", true)[0] as Telerik.Reporting.Table;


            table.DataSource = SZVTDEventsList;


            return SZVTD;
        }

 
    }
}
