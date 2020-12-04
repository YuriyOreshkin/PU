using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    [DisplayName("Тарифы для взносов на дополнительное соц. обеспечение")]
    public class CodeBaseRW3_2015View : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible = false)]
        public long ID { get; set; }

        [DisplayName("Финансовый год")]
        [Required(ErrorMessage = "Поле 'Финансовый год' обязательно для заполнения!")]
        public short Year { get; set; }

        [DisplayName("Тариф 21")]
        [Filtrable(false)]
        public decimal Tar21 { get; set; }

        [DisplayName("Тариф 22")]
        [Filtrable(false)]
        public decimal Tar22 { get; set; }


   
    }
}
