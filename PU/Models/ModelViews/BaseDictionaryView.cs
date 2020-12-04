using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace PU.Models.ModelViews
{

    public class BaseDictionaryView : BaseDictionaryActions
    {
        
        [DisplayVisible(IsVisible = false)]
        public long ID { get; set; }

        [DisplayName("Код")]
        [Display(Order = 1)]
        [ReadOnly(true)]
        [DisplayWidth(MaxWidth = 120)]
        [Required(ErrorMessage = "Поле 'Код' обязательно для заполнения!")]
        public string Code { get; set; }

        [DisplayName("Наименование")]
        [Display(Order = 2)]
        [WrapText(true)]
        [Required(ErrorMessage = "Поле 'Наименование' обязательно для заполнения!")]
        public string Name { get; set; }

        [DisplayName("Начало")]
        [Display(Order = 3)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DisplayWidth(MaxWidth = 80)]
        [Required(ErrorMessage = "Поле 'Начало' обязательно для заполнения!")]
        [Filtrable(false)]
        public  DateTime? DateBegin { get; set; }

        [DisplayName("Конец")]
        [Display(Order = 4)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DisplayWidth(MaxWidth = 80)]
        [Filtrable(false)]
        public DateTime? DateEnd { get; set; }

    }
}
