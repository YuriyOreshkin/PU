using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    public class VidTrudDeyatView : BaseDictionaryView
    {
        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname) { }
        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }
    }
}
