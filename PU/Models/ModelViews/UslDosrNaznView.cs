using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    public class UslDosrNaznView : BaseDictionaryView
    {
        [DisplayVisible(IsVisible =false)]
        public long? EdIzmID { get; set; }

        [DisplayName("Ед. изм.")]
        [DisplayWidth(MaxWidth = 120)]
        [DisplayField("UslDosrNaznEdIzm.Name")]
        [LookUp(DataSource = "UslDosrNaznEdIzm", ValueMember = "ID", DisplayMember = "Name")]
        public UslDosrNaznEdIzmView UslDosrNaznEdIzm { get; set; }

        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname) { }
        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }
    }
}
