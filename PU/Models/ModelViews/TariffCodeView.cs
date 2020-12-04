using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    public class TariffCodeView : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible =false)]
        public long ID { get; set; }

        [DisplayName("Код")]
        [Display(Order = 0)]
        [ReadOnly(true)]
        [Required(ErrorMessage = "Поле 'Код' обязательно для заполнения!")]
        public string Code { get; set; }


        [DisplayName("Начало")]
        [Display(Order = 2)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Required(ErrorMessage = "Поле 'Начало' обязательно для заполнения!")]
        [Filtrable(false)]
        public DateTime? DateBegin { get; set; }

        [DisplayName("Конец")]
        [Display(Order = 3)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [Filtrable(false)]
        public DateTime? DateEnd { get; set; }


        public override void Load(Form form, string classname)
        {
            ThemeResolutionService.ApplyThemeToControlTree(form, ((RadForm)form).ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(((RadForm)form).ThemeName);

            #region TariffCode
            RadGridView radGridViewCategory = (RadGridView)Mapping.Mapping.GetControlByTag(form.Controls, "TariffCodeGrid");

            radGridViewCategory.CellDoubleClick += new GridViewCellEventHandler((s, args) => { this.Edit(form, radGridViewCategory, classname); });
            FullDataSource(form, classname);


            Mapping.Mapping.ActionsClassMap(form, this, classname);
            #endregion

            #region TariffCodePlatCat
            RadGridView radGridViewTariff = (RadGridView)Mapping.Mapping.GetControlByTag(form.Controls, "TariffCodePlatCatGrid");

            radGridViewTariff.CellDoubleClick += new GridViewCellEventHandler((s, args) => { this.Edit(form, radGridViewTariff, "TariffCodePlatCat"); });

            FullDataSource(form, "TariffCodePlatCat");
            TariffCodePlatCatView tarif = new TariffCodePlatCatView();

            Mapping.Mapping.ActionsClassMap(form, tarif, "TariffCodePlatCat");
            #endregion

        }

        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname){}

        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname){}

        
    }
}
