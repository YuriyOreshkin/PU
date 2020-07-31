using PU.Classes;
using PU.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.Reporting;
using Telerik.Reporting.Drawing;

namespace PU.Reports
{
    static class ReportMethods
    {
        public static void PrintStaff(List<StaffObject> staffList, string ThemeName)
        {
            pu6Entities db = new pu6Entities();

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(980, 900);
            child.ShowInTaskbar = false;
            child.Text = "Список застрахованных лиц";

            Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            reportViewer.Name = "reportStaff";

            Staff_Rep StaffRep = new Staff_Rep();

            Insurer Ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

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

                InsName += " [" + Utils.ParseRegNum(Ins.RegNum) + "]";

                Telerik.Reporting.TextBox tb = StaffRep.Items.Find("Insurer", true)[0] as Telerik.Reporting.TextBox;
                tb.Value = InsName;
            }

            int num = staffList.Count;

            if (num > 0)
            {
                Telerik.Reporting.TextBox Title = StaffRep.Items.Find("Title", true)[0] as Telerik.Reporting.TextBox;
                Title.Value = "Список застрахованных лиц (" + num.ToString() + " чел.)";
            }

            Telerik.Reporting.Table table2 = StaffRep.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

            table2.DataSource = staffList;

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = StaffRep;
            reportViewer.ReportSource = typeReportSource;
            reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            reportViewer.RefreshReport();
            child.Controls.Add(reportViewer);

            child.ShowDialog();
        }

        public static void PrintStaffLgot(List<StaffLgotObject> staffLgotList, string ThemeName)
        {
            pu6Entities db = new pu6Entities();

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(980, 900);
            child.ShowInTaskbar = false;
            child.Text = "Список льготников";

            Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            reportViewer.Name = "reportLgot";

            StaffLgot_Rep StaffRep = new StaffLgot_Rep();

            Insurer Ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

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

                InsName += " [" + Utils.ParseRegNum(Ins.RegNum) + "]";

                Telerik.Reporting.TextBox tb = StaffRep.Items.Find("Insurer", true)[0] as Telerik.Reporting.TextBox;
                tb.Value = InsName;
            }

            int num = staffLgotList.Count;

            if (num > 0)
            {
                Telerik.Reporting.TextBox Title = StaffRep.Items.Find("Title", true)[0] as Telerik.Reporting.TextBox;
                Title.Value = "Список льготников (" + num.ToString() + " чел.)";
            }

            Telerik.Reporting.Table table1 = StaffRep.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

            table1.DataSource = staffLgotList;

            Telerik.Reporting.Table table2 = StaffRep.Items.Find("table2", true)[0] as Telerik.Reporting.Table;

            table2.DataSource = staffLgotList.SelectMany(x => x.razd67);

            Telerik.Reporting.Table table3 = StaffRep.Items.Find("table3", true)[0] as Telerik.Reporting.Table;

            table3.DataSource = staffLgotList.SelectMany(x => x.stajOsn);

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = StaffRep;
            reportViewer.ReportSource = typeReportSource;
            reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            reportViewer.RefreshReport();
            child.Controls.Add(reportViewer);

            child.ShowDialog();
        }


        public static void PrintInsurer(List<InsurerRep> insList, string ThemeName)
        {
            pu6Entities db = new pu6Entities();

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(980, 900);
            child.ShowInTaskbar = false;

            Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            reportViewer.Name = "reportViewer1";

            Insurer_Rep InsurerRep = new Insurer_Rep();

            Telerik.Reporting.Table table2 = InsurerRep.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

            table2.DataSource = insList;

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = InsurerRep;
            reportViewer.ReportSource = typeReportSource;
            reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            reportViewer.RefreshReport();
            child.Controls.Add(reportViewer);

            child.ShowDialog();
        }

        public static void Print_svedNachVznos(List<svedNachVznos_container> svedNachVznos_list, string period, string ThemeName)
        {
            pu6Entities db = new pu6Entities();

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(980, 800);
            child.ShowInTaskbar = false;
            child.Text = "Сведения о начисленных взносах в ПФР";

            Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            reportViewer.Name = "reportViewer1";

            PU.Reports.DopFunc.SvedNachVznos_Rep SvedNachVznosRep = new PU.Reports.DopFunc.SvedNachVznos_Rep();

            Insurer Ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

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

                InsName += "     [" + Utils.ParseRegNum(Ins.RegNum) + "]";

                (SvedNachVznosRep.Items.Find("Insurer", true)[0] as Telerik.Reporting.TextBox).Value = InsName;
            }

            (SvedNachVznosRep.Items.Find("Period", true)[0] as Telerik.Reporting.TextBox).Value = period;

            Telerik.Reporting.Table table2 = SvedNachVznosRep.Items.Find("table1", true)[0] as Telerik.Reporting.Table;

            table2.DataSource = svedNachVznos_list;

            (SvedNachVznosRep.Items.Find("Base_OPS_ALL_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.Base_OPS_ALL).ToString();
            (SvedNachVznosRep.Items.Find("Base_OPS_3M_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.Base_OPS_3M).ToString();
            (SvedNachVznosRep.Items.Find("OPS_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.OPS).ToString();


            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = SvedNachVznosRep;
            reportViewer.ReportSource = typeReportSource;
            reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            reportViewer.RefreshReport();
            child.Controls.Add(reportViewer);

            child.ShowDialog();
        }

        public static void Print_svedBaseOPS(List<svedBaseOPS_container> svedBaseOPS_list, string period, string ThemeName)
        {
            pu6Entities db = new pu6Entities();

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(980, 800);
            child.ShowInTaskbar = false;
            child.Text = "Сведения по базе для начисления страховых взносов на ОПС";

            Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            reportViewer.Name = "reportViewer1";

            PU.Reports.DopFunc.SvedBaseOPS_Rep SvedBaseOPSRep = new PU.Reports.DopFunc.SvedBaseOPS_Rep();

            Insurer Ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

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

                InsName += "     [" + Utils.ParseRegNum(Ins.RegNum) + "]";

                (SvedBaseOPSRep.Items.Find("Insurer", true)[0] as Telerik.Reporting.TextBox).Value = InsName;
            }

            (SvedBaseOPSRep.Items.Find("Period", true)[0] as Telerik.Reporting.TextBox).Value = period;

            Telerik.Reporting.Table table1 = SvedBaseOPSRep.Items.Find("crosstab1", true)[0] as Telerik.Reporting.Table;

            //var l = svedBaseOPS_list.GroupBy(x => new {FIO = x.FIO,  SNILS = x.SNILS, form = x} ).ToList();
//            var l = svedBaseOPS_list.GroupBy(x => new { FIO = x.FIO, SNILS = x.SNILS }).ToList();

            table1.DataSource = svedBaseOPS_list;

            //Telerik.Reporting.TableGroup group1 = new Telerik.Reporting.TableGroup();
            //group1.Name = "SNILS";
            //group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.SNILS"));

            //Telerik.Reporting.TextBox textBox1 = new Telerik.Reporting.TextBox();
            //table1.Items.Add(textBox1);
            //group1.ReportItem = textBox1;

            //table1.RowGroups.Add(group1);



            //(SvedNachVznosRep.Items.Find("Base_OPS_ALL_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.Base_OPS_ALL).ToString();
            //(SvedNachVznosRep.Items.Find("Base_OPS_3M_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.Base_OPS_3M).ToString();
            //(SvedNachVznosRep.Items.Find("OPS_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.OPS).ToString();


            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = SvedBaseOPSRep;
            reportViewer.ReportSource = typeReportSource;
            reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            reportViewer.RefreshReport();
            child.Controls.Add(reportViewer);

            child.ShowDialog();
        }

        public static void Print_svedVypl(List<svedVypl_container> svedVypl_list, string period, string ThemeName)
        {
            pu6Entities db = new pu6Entities();

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(980, 800);
            child.ShowInTaskbar = false;
            child.Text = "Сведения о суммах выплат";

            Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            reportViewer.Name = "reportViewer1";

            PU.Reports.DopFunc.SvedVypl_Rep SvedVypl_Rep = new PU.Reports.DopFunc.SvedVypl_Rep();

            Insurer Ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

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

                InsName += "     [" + Utils.ParseRegNum(Ins.RegNum) + "]";

                (SvedVypl_Rep.Items.Find("Insurer", true)[0] as Telerik.Reporting.TextBox).Value = InsName;
            }

            (SvedVypl_Rep.Items.Find("Period", true)[0] as Telerik.Reporting.TextBox).Value = period;

            Telerik.Reporting.Table table1 = SvedVypl_Rep.Items.Find("crosstab1", true)[0] as Telerik.Reporting.Table;

            //var l = svedBaseOPS_list.GroupBy(x => new {FIO = x.FIO,  SNILS = x.SNILS, form = x} ).ToList();
            //            var l = svedBaseOPS_list.GroupBy(x => new { FIO = x.FIO, SNILS = x.SNILS }).ToList();

            table1.DataSource = svedVypl_list;

            //Telerik.Reporting.TableGroup group1 = new Telerik.Reporting.TableGroup();
            //group1.Name = "SNILS";
            //group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.SNILS"));

            //Telerik.Reporting.TextBox textBox1 = new Telerik.Reporting.TextBox();
            //table1.Items.Add(textBox1);
            //group1.ReportItem = textBox1;

            //table1.RowGroups.Add(group1);



            //(SvedNachVznosRep.Items.Find("Base_OPS_ALL_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.Base_OPS_ALL).ToString();
            //(SvedNachVznosRep.Items.Find("Base_OPS_3M_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.Base_OPS_3M).ToString();
            //(SvedNachVznosRep.Items.Find("OPS_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.OPS).ToString();


            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = SvedVypl_Rep;
            reportViewer.ReportSource = typeReportSource;
            reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            reportViewer.RefreshReport();
            child.Controls.Add(reportViewer);

            child.ShowDialog();
        }

        public static void Print_svedDop(List<svedDop_container> svedDop_list, string period, string ThemeName)
        {
            pu6Entities db = new pu6Entities();

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(980, 800);
            child.ShowInTaskbar = false;
            child.Text = "Сведения о выплатах по доп.тарифам";

            Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            reportViewer.Name = "reportViewer1";

            PU.Reports.DopFunc.SvedDop_Rep SvedDop_Rep = new PU.Reports.DopFunc.SvedDop_Rep();

            Insurer Ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

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

                InsName += "     [" + Utils.ParseRegNum(Ins.RegNum) + "]";

                (SvedDop_Rep.Items.Find("Insurer", true)[0] as Telerik.Reporting.TextBox).Value = InsName;
            }

            (SvedDop_Rep.Items.Find("Period", true)[0] as Telerik.Reporting.TextBox).Value = period;

            Telerik.Reporting.Table table1 = SvedDop_Rep.Items.Find("crosstab1", true)[0] as Telerik.Reporting.Table;

            //var l = svedBaseOPS_list.GroupBy(x => new {FIO = x.FIO,  SNILS = x.SNILS, form = x} ).ToList();
            //            var l = svedBaseOPS_list.GroupBy(x => new { FIO = x.FIO, SNILS = x.SNILS }).ToList();

            table1.DataSource = svedDop_list;

            //Telerik.Reporting.TableGroup group1 = new Telerik.Reporting.TableGroup();
            //group1.Name = "SNILS";
            //group1.Groupings.Add(new Telerik.Reporting.Grouping("=Fields.SNILS"));

            //Telerik.Reporting.TextBox textBox1 = new Telerik.Reporting.TextBox();
            //table1.Items.Add(textBox1);
            //group1.ReportItem = textBox1;

            //table1.RowGroups.Add(group1);



            //(SvedNachVznosRep.Items.Find("Base_OPS_ALL_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.Base_OPS_ALL).ToString();
            //(SvedNachVznosRep.Items.Find("Base_OPS_3M_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.Base_OPS_3M).ToString();
            //(SvedNachVznosRep.Items.Find("OPS_Sum", true)[0] as Telerik.Reporting.TextBox).Value = svedNachVznos_list.Sum(x => x.OPS).ToString();


            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = SvedDop_Rep;
            reportViewer.ReportSource = typeReportSource;
            reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            reportViewer.RefreshReport();
            child.Controls.Add(reportViewer);

            child.ShowDialog();
        }

        public static void Print_svedKorr(List<svedKorr_container> svedKorr_list, string period, string ThemeName)
        {
            pu6Entities db = new pu6Entities();

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(980, 800);
            child.ShowInTaskbar = false;
            child.Text = "Информация о корректирующих сведениях";

            Telerik.ReportViewer.WinForms.ReportViewer reportViewer = new Telerik.ReportViewer.WinForms.ReportViewer();
            reportViewer.Dock = DockStyle.Fill;
            reportViewer.Name = "reportViewer1";

            PU.Reports.DopFunc.SvedKorr_Rep SvedKorr_Rep = new PU.Reports.DopFunc.SvedKorr_Rep();

            Insurer Ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

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

                InsName += "     [" + Utils.ParseRegNum(Ins.RegNum) + "]";

                (SvedKorr_Rep.Items.Find("Insurer", true)[0] as Telerik.Reporting.TextBox).Value = InsName;
            }

            (SvedKorr_Rep.Items.Find("Period", true)[0] as Telerik.Reporting.TextBox).Value = period;

            Telerik.Reporting.Table table1 = SvedKorr_Rep.Items.Find("crosstab1", true)[0] as Telerik.Reporting.Table;

            table1.DataSource = svedKorr_list;

            var typeReportSource = new Telerik.Reporting.InstanceReportSource();

            typeReportSource.ReportDocument = SvedKorr_Rep;
            reportViewer.ReportSource = typeReportSource;
            reportViewer.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            reportViewer.RefreshReport();
            child.Controls.Add(reportViewer);

            child.ShowDialog();
        }





        /// <summary>
        /// Объединение нескольких репортов в один сабрепорт, чтобы в предварительном просмотре были настройки печати
        /// </summary>
        /// <param name="detailReports"></param>
        /// <returns></returns>
        public static Telerik.Reporting.Report GetCombinedReport(List<Telerik.Reporting.Report> detailReports)
        {
            Telerik.Reporting.Report report = new Telerik.Reporting.Report();
            if (detailReports.Any())
            {
                report.PageSettings.Margins = detailReports[0].PageSettings.Margins;
                DetailSection detail = new DetailSection();
                report.Items.Add(detail);

                Unit unitX = Unit.Cm(0);
                Unit unitY = Unit.Cm(0);
                SizeU size = new SizeU(Unit.Cm(1), Unit.Cm(0.5));
                foreach (Telerik.Reporting.Report detailReport in detailReports)
                {
                    SubReport subReport;
                    subReport = new SubReport();

                    subReport.Location = new PointU(unitX, unitY);
                    subReport.Size = size;
                    unitY = unitY.Add(Unit.Cm(0.5));
                    subReport.ReportSource = detailReport;
                    detail.Items.Add(subReport);
                    //detail.Items.Add();
                }
                detail.Height = Unit.Inch(detailReports.Count + 1);
            }
            return report;
        }

    }
}
