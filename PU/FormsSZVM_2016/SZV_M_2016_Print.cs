using PU.Classes;
using PU.FormsSZVM_2016.Report;
using PU.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;
using Telerik.WinControls;

namespace PU.FormsSZVM_2016
{
    class SZV_M_2016_Print
    {
        public static void PrintSZVM(long szvmID, string ThemeName)
        {
            pu6Entities db = new pu6Entities();

            if (db.FormsSZV_M_2016.Any(x => x.ID == szvmID))
            {
                FormsSZV_M_2016 SZVM = db.FormsSZV_M_2016.FirstOrDefault(x => x.ID == szvmID);

                Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
                child.ThemeName = ThemeName;
                child.StartPosition = FormStartPosition.CenterScreen;
                child.Size = new Size(980, 900);
                child.ShowInTaskbar = false;
                child.Text = "Форма СЗВ-М";

                Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
                reportViewer.Dock = DockStyle.Fill;
                reportViewer.Name = "reportViewer1";

                SZVM_Report SZVMRep = new SZVM_Report();

                Insurer Ins = db.Insurer.FirstOrDefault(x => x.ID == SZVM.InsurerID);

                if (Ins != null)
                {

                    string InsName = String.Empty;

                    if (Ins.TypePayer.HasValue)
                        if (Ins.TypePayer.Value == 0) // Если страхователь - Юр лицо
                        {
                            if (!String.IsNullOrEmpty(Ins.NameShort))
                                InsName = Ins.NameShort;
                            if (String.IsNullOrEmpty(InsName) && !String.IsNullOrEmpty(Ins.Name))
                                InsName = Ins.Name;
                        }
                        else
                        {
                            InsName = Ins.LastName + " " + Ins.FirstName + " " + Ins.MiddleName;

                            if (String.IsNullOrEmpty(InsName) && !String.IsNullOrEmpty(Ins.NameShort))
                                InsName = Ins.NameShort;
                            if (String.IsNullOrEmpty(InsName) && !String.IsNullOrEmpty(Ins.Name))
                                InsName = Ins.Name;

                        }

                    string regnum = Utils.ParseRegNum(Ins.RegNum);

                    (SZVMRep.Items.Find("RegNum", true)[0] as Telerik.Reporting.TextBox).Value = regnum;
                    (SZVMRep.Items.Find("NameShort", true)[0] as Telerik.Reporting.TextBox).Value = InsName;
                    (SZVMRep.Items.Find("INN", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(Ins.INN) ? Ins.INN : "";
                    (SZVMRep.Items.Find("KPP", true)[0] as Telerik.Reporting.TextBox).Value = !String.IsNullOrEmpty(Ins.KPP) ? Ins.KPP : "";


                    if (Ins.BossPrint.HasValue && Ins.BossPrint.Value == true)
                    {
                        (SZVMRep.Items.Find("rukName1", true)[0] as Telerik.Reporting.TextBox).Value = Ins.BossFIO;
                        (SZVMRep.Items.Find("rukDolgn1", true)[0] as Telerik.Reporting.TextBox).Value = Ins.BossDolgn;
                    }
                }

                (SZVMRep.Items.Find("Month", true)[0] as Telerik.Reporting.TextBox).Value = SZVM.MONTH.ToString().PadLeft(2, '0');
                (SZVMRep.Items.Find("Year", true)[0] as Telerik.Reporting.TextBox).Value = SZVM.YEAR.ToString();
                (SZVMRep.Items.Find("DateFilling", true)[0] as Telerik.Reporting.TextBox).Value = SZVM.DateFilling.ToShortDateString();

                string typeInfo = "";

                switch (SZVM.TypeInfo.Name)
                {
                    case "Исходная":
                        typeInfo = "исхд";
                        break;
                    case "Корректирующая":
                        typeInfo = "доп";
                        break;
                    case "Отменяющая":
                        typeInfo = "отмн";
                        break;
                }
                (SZVMRep.Items.Find("TypeInfo", true)[0] as Telerik.Reporting.TextBox).Value = typeInfo;



                List<SZVMStaffRep> SZVMStaffList = new List<SZVMStaffRep>();

                int i = 0;
                foreach (var item in SZVM.FormsSZV_M_2016_Staff.OrderBy(x => x.Staff.LastName))
                {
                    i++;

                    string contrNum = "";
                    if (item.Staff.ControlNumber != null)
                    {
                        contrNum = item.Staff.ControlNumber.HasValue ? item.Staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                    }

                    SZVMStaffList.Add(new SZVMStaffRep
                    {
                        Num = i,
                        FIO = item.Staff.LastName + " " + item.Staff.FirstName + " " + item.Staff.MiddleName,
                        SNILS = !String.IsNullOrEmpty(item.Staff.InsuranceNumber) ? item.Staff.InsuranceNumber.PadLeft(9, '0').Substring(0, 3) + "-" + item.Staff.InsuranceNumber.PadLeft(9, '0').Substring(3, 3) + "-" + item.Staff.InsuranceNumber.PadLeft(9, '0').Substring(6, 3) + " " + contrNum : "",
                        INN = !String.IsNullOrEmpty(item.Staff.INN) ? item.Staff.INN : ""
                    });
                }

                Telerik.Reporting.Table table1 = SZVMRep.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

                table1.DataSource = SZVMStaffList;

                var typeReportSource = new Telerik.Reporting.InstanceReportSource();

                typeReportSource.ReportDocument = SZVMRep;
                reportViewer.ReportSource = typeReportSource;
                reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
                reportViewer.RefreshReport();
                child.Controls.Add(reportViewer);

                child.ShowDialog();


            }
            else  // Если форма СЗВ-М не найдена
            {
                RadMessageBox.Show("В базе данных не найдена Форма СЗВ-М!");
            }



        }

        public void PrintEmptySZVM(string ThemeName)
        {

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(980, 900);
            child.ShowInTaskbar = false;
            child.Text = "Форма СЗВ-М";

            Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            reportViewer.Name = "reportSZVM";

            SZVM_Report SZVMRep = new SZVM_Report();

            List<SZVMStaffRep> SZVMStaffList = new List<SZVMStaffRep>();

            for (int i = 1; i <= 20; i++)
            {

                SZVMStaffList.Add(new SZVMStaffRep { Num = i });
            }

            Telerik.Reporting.Table table1 = SZVMRep.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

            table1.DataSource = SZVMStaffList;


            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = SZVMRep;
            reportViewer.ReportSource = typeReportSource;
            reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            reportViewer.RefreshReport();
            child.Controls.Add(reportViewer);

            child.ShowDialog();


        }

    }
}
