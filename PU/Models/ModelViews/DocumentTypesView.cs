using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    [DisplayName("Типы документов")]
    public class DocumentTypesView : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible = false)]
        public long ID { get; set; }

        [DisplayName("Код")]
        [Display(Order = 0)]
        [ReadOnly(true)]
        [DisplayWidth(MaxWidth = 120)]
        [Required(ErrorMessage = "Поле 'Код' обязательно для заполнения!")]
        [StringLength(14)]
        public string Code { get; set; }

        [DisplayName("Наименование")]
        [WrapText(true)]
        [Required(ErrorMessage = "Поле 'Наименование' обязательно для заполнения!")]
        [StringLength(40)]
        public string Name { get; set; }

        


        [DisplayVisible(IsVisible  =false)]
        public override void Add(Form form, RadGridView radGridView, string classname) { }


        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }
    }
}
