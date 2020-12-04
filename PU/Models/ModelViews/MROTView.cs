using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    public class MROTView : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible =false)]
        public long ID { get; set; }

        [DisplayName("Финансовый год")]
        [Required(ErrorMessage = "Поле 'Финансовый год' обязательно для заполнения!")]
        public short Year { get; set; }


        [DisplayName("Налоговая база")]
        [Required(ErrorMessage = "Поле 'Налоговая база' обязательно для заполнения!")]
        public decimal Tariff1 { get; set; }

        [DisplayName("МРОТ")]
        [Required(ErrorMessage = "Поле 'МРОТ' обязательно для заполнения!")]
        public decimal Mrot1 { get; set; }
        

        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname) { }


        [DisplayVisible(IsVisible = false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }
    }
}
