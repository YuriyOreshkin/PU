using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    public class TerrUslView : BaseDictionaryView
    {
        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname){ }
        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }

    }

}
