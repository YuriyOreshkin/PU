using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PU.Models;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.IO;
using PU.Classes;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using PU.Models.ModelViews;
using PU.Models.Mapping;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI.Docking;

namespace PU.Dictionaries
{
    partial class PlatCategoryFormList1 : Telerik.WinControls.UI.RadForm
    {


        public PlatCategoryFormList1()
        {
            InitializeComponent();
        }

        private void PlatCategoryFormList_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
          

            foreach (var window in radDockPlatCategory.DockWindows)
            {

                HostWindow hw = window as HostWindow;
                if (hw != null)
                {
                    if (hw.Name.Contains("2"))
                    {
                        var form = Dictionaries.BaseDictionaryEvents.Dialog(this, "PlatCategory","Категории");
                        hw.LoadContent(form);

                    }

                    if (hw.Name.Contains("1"))
                    {
                        var form = Dictionaries.BaseDictionaryEvents.Dialog(this, "TariffPlat", "Тарифы");
                        hw.LoadContent(form);

                    }

                }
            }
            
        }
    }
}
