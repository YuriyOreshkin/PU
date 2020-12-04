using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    [DisplayName("Справочник профессий и должностей")]
    public class DolgnView : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible = false)]
        public long ID { get; set; }

        [DisplayName("Наименование")]
        [WrapText(true)]
        [Required(ErrorMessage = "Поле 'Наименование' обязательно для заполнения!")]
        [StringLength(100)]
        public string Name { get; set; }


        [DisplayVisible(IsVisible  = false)]
        public override void Synchronization(Form form, RadGridView radGridView, string classname) { }

   
    }
}
