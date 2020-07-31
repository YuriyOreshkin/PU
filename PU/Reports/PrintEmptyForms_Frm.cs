using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;

namespace PU.Reports
{
    public partial class PrintEmptyForms_Frm : Telerik.WinControls.UI.RadForm
    {
        public PrintEmptyForms_Frm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Перехват нажатия на ESC для закрытия формы
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PrintEmptyForms_Frm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            formsList.SelectedItem = formsList.Items[0];
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (formsList.SelectedItem != null)
            {
                switch (formsList.SelectedItem.Tag.ToString())
                {
                    case "rsv1_2014":
                        PU.FormsRSW2014.RSW2014_Print child = new FormsRSW2014.RSW2014_Print(); 
                        child.createEmptyReport(2014);
                        child.ShowDialog();
                        break;
                    case "rsv1_2015":
                        PU.FormsRSW2014.RSW2014_Print child2 = new FormsRSW2014.RSW2014_Print();
                        child2.createEmptyReport(2015);
                        child2.ShowDialog();
                        break;
                    case "spv2":
                        PU.FormsSPW2_2014.SPW2_Print child3 = new FormsSPW2_2014.SPW2_Print();
                        child3.createEmptyReport();
                        child3.ShowDialog();
                        break;
                    case "rv3_2015":
                        PU.FormsRW_3_2015.RW3_2015_Print child4 = new FormsRW_3_2015.RW3_2015_Print();
                        child4.createEmptyReport();
                        child4.ShowDialog();
                        break;
                    case "rsv2_2014":
                        PU.FormsRSW2_2014.RSW2_2014_Print child5 = new FormsRSW2_2014.RSW2_2014_Print();
                        child5.createEmptyReport(2014);
                        child5.ShowDialog();
                        break;
                    case "rsv2_2015":
                        PU.FormsRSW2_2014.RSW2_2014_Print child6 = new FormsRSW2_2014.RSW2_2014_Print();
                        child6.createEmptyReport(2015);
                        child6.ShowDialog();
                        break;
                    case "szv_6_1":
                        PU.FormsRSW2014.RSW2014_Print child7 = new FormsRSW2014.RSW2014_Print();
                        child7.createEmptyReport_SZV_6_1();
                        child7.ShowDialog();
                        break;
                    case "szv_6_2":
                        PU.FormsRSW2014.RSW2014_Print child8 = new FormsRSW2014.RSW2014_Print();
                        child8.createEmptyReport_SZV_6_2();
                        child8.ShowDialog();
                        break;
                    case "szv_6_4":
                        PU.FormsRSW2014.RSW2014_Print child9 = new FormsRSW2014.RSW2014_Print();
                        child9.createEmptyReport_SZV_6_4();
                        child9.ShowDialog();
                        break;
                    case "szvm":
                        PU.FormsSZVM_2016.SZV_M_2016_Print child10 = new FormsSZVM_2016.SZV_M_2016_Print();
                        child10.PrintEmptySZVM(this.ThemeName);
                        break;
                    case "dsv3":
                        PU.FormsDSW3.DSW3_Print child11 = new FormsDSW3.DSW3_Print();
                        child11.createEmptyReport();
                        child11.ShowDialog();
                        break;
                }
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
