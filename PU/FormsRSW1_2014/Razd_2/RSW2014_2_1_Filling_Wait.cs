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
    public partial class RSW2014_2_1_Filling_Wait : Telerik.WinControls.UI.RadForm
    {
        public RSW2014_2_1_Filling_Wait()
        {
            InitializeComponent();
        }

        private void RSW2014_2_1_Filling_Wait_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            radProgressBar1.Value1 = 0;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            RSW2014_2_1_Filling main = this.Owner as RSW2014_2_1_Filling;
            if (main != null)
            {
                main.backgroundWorker1.CancelAsync();
                this.Close();
            }
            
        }
    }
}
