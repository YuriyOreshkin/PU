using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    public class TariffPlatView : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible =false)]
        public long ID { get; set; }

        [DisplayName("Год")]
        [Required(ErrorMessage = "Поле 'Год' обязательно для заполнения!")]
        public short Year { get; set; }

        [DisplayName("ПФР страх.часть 1966 и старше")]
        public decimal? StrahPercant1966 { get; set; }

        [DisplayName("ПФР страх.часть 1967 и моложе")]
        public decimal? StrahPercent1967 { get; set; }


        [DisplayName("ПФР накоп.часть 1967 и моложе")]
        public decimal? NakopPercant { get; set; }

        [DisplayName("ОМС (ФФОМС)")]
        public  decimal? FFOMS_Percent { get; set; }

        [DisplayName("ТФОМС")]
        public decimal? TFOMS_Percent { get; set; }

        [DisplayVisible(IsVisible =false)]
        public long PlatCategoryID { get; set; }

        [DisplayVisible(IsVisible =false)]
        public override void Close(Form form, RadGridView radGridView, string classname){}

        [DisplayVisible(IsVisible =false)]
        public override void Synchronization(Form form, RadGridView radGridView, string classname){}

        
    }
}
