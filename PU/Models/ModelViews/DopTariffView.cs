using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    public class DopTariffView : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible =false)]
        public long ID { get; set; }

        [DisplayName("Год")]
        [Required(ErrorMessage = "Поле 'Год' обязательно для заполнения!")]
        public short Year { get; set; }

        [DisplayName("ч.1 ст.58.3")]
        public decimal Tariff1 { get; set; }

        [DisplayName("ч.2 ст.58.3")]
        public decimal Tariff2 { get; set; }
        
    }
}
