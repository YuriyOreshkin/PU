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
    [DisplayName("Расчетные периоды")]
    public class PlatCategoryRaschPerView : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible =false)]
        public long ID { get; set; }

        [DisplayName("Наименование")]
        [Display(Order = 1)]
        [Required(ErrorMessage = "Поле 'Наименование' обязательно для заполнения!")]
        public string NAME { get; set; }

        [DisplayName("Начало")]
        [Display(Order = 2)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DisplayWidth(MaxWidth = 80)]
        [Required(ErrorMessage = "Поле 'Начало' обязательно для заполнения!")]
        [Filtrable(false)]
        public DateTime? DateBegin { get; set; }

        [DisplayName("Конец")]
        [Display(Order = 3)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DisplayWidth(MaxWidth = 80)]
        [Filtrable(false)]
        public DateTime? DateEnd { get; set; }



        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname){ }

        [DisplayVisible(IsVisible =false)]
        public override void Edit(Form form, RadGridView radGridView, string classname){}

        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }

    }

}
