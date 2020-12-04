using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    [DisplayName("Единица измерения")]
    public class UslDosrNaznEdIzmView : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible =false)]
        public long ID { get; set; }

        [DisplayName("Полное наименование")]
        [WrapText(true)]
        [Required(ErrorMessage = "Поле 'Полное наименование' обязательно для заполнения!")]
        public string Name { get; set; }

        [DisplayName("Краткое наименование")]
        [Required(ErrorMessage = "Поле 'Краткое наименование' обязательно для заполнения!")]
        public string ShortName { get; set; }

        [DisplayName("Значение")]
        [Required(ErrorMessage = "Поле 'Значение' обязательно для заполнения!")]
        public string Znach { get; set; }


        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname) { }

        [DisplayVisible(IsVisible =false)]
        public override void Edit(Form form, RadGridView radGridView, string classname) { }

        [DisplayVisible(IsVisible =false)]
        public override void Synchronization(Form form, RadGridView radGridView, string classname) { }

        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }
    }
}
