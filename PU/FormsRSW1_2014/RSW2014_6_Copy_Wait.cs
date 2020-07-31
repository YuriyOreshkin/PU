using PU.FormsSZV_6_4_2013.DopFunc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace PU.FormsRSW2014
{
    public partial class RSW2014_6_Copy_Wait : Telerik.WinControls.UI.RadForm
    {
        public string ownerName = "";

        public RSW2014_6_Copy_Wait()
        {
            InitializeComponent();
        }

        private void RSW2014_2_1_Filling_Wait_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            radProgressBar1.Value1 = 0;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            switch (ownerName)
            {
                case "RSW2014_6_Copy":
                    RSW2014_6_Copy main = this.Owner as RSW2014_6_Copy;
                    if (main != null)
                    {
                        main.backgroundWorker1.CancelAsync();
                        this.Close();
                    }
                    break;
                case "RaschDonachSum":
                    RaschDonachSum main2 = this.Owner as RaschDonachSum;
                    if (main2 != null)
                    {
                        main2.bw.CancelAsync();
                        this.Close();
                    }
                    break;
                case "ProverkaNalStaj":
                    ProverkaNalStaj main3 = this.Owner as ProverkaNalStaj;
                    if (main3 != null)
                    {
                        main3.bw.CancelAsync();
                        this.Close();
                    }
                    break;
                case "ProsmotrVyplat":
                    ProsmotrVyplat main4 = this.Owner as ProsmotrVyplat;
                    if (main4 != null)
                    {
                        main4.bw.CancelAsync();
                        this.Close();
                    }
                    break;
                case "DeleteIndSved":
                    DeleteIndSved main5 = this.Owner as DeleteIndSved;
                    if (main5 != null)
                    {
                        main5.bw.CancelAsync();
                        this.Close();
                    }
                    break;
                case "CopyStaj":
                    CopyStaj main6 = this.Owner as CopyStaj;
                    if (main6 != null)
                    {
                        main6.bw.CancelAsync();
                        this.Close();
                    }
                    break;
                case "ProsmotrStaj":
                    ProsmotrStaj main7 = this.Owner as ProsmotrStaj;
                    if (main7 != null)
                    {
                        main7.bw.CancelAsync();
                        this.Close();
                    }
                    break;
                case "RaschDonachSumSZV64":
                    RaschDonachSumSZV64 main8 = this.Owner as RaschDonachSumSZV64;
                    if (main8 != null)
                    {
                        main8.bw.CancelAsync();
                        this.Close();
                    }
                    break;
                    
            }

        }
    }
}
