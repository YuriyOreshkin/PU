using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    [DisplayName("Категории плательщиков")]
    public class PlatCategoryView : BaseDictionaryView
    {
        [DisplayName("Описание")]
        [StringLength(1000)]
        [DisplayVisible(IsVisible =false)]
        public string FullName { get; set; }

        [DisplayVisible(IsVisible =false)]
        public long PlatCategoryRaschPerID { get; set; }

        [DisplayName("Начало")]
        [Display(Order = 2)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DisplayWidth(MaxWidth = 80)]
        [Required(ErrorMessage = "Поле 'Начало' обязательно для заполнения!")]
        public DateTime? DateBegin { get; set; }

        [DisplayName("Конец")]
        [Display(Order = 3)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        [DisplayWidth(MaxWidth = 80)]
        public virtual DateTime? DateEnd { get; set; }

        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname) { }
        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }


        public override void Load(Form form, string classname)
        {
            ThemeResolutionService.ApplyThemeToControlTree(form, ((RadForm)form).ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(((RadForm)form).ThemeName);

            #region PlatCategoryRaschPer
                BindingSource b = new BindingSource();
                b.DataSource = Mapping.Mapping.DataSourceMap("PlatCategoryRaschPer");
                RadDropDownList dropDownList = (RadDropDownList)Mapping.Mapping.GetControlByTag(form.Controls, "PlatCategoryRaschPer");
                dropDownList.DataSource = b;
            #endregion


            #region PlatCategory

                RadGridView radGridViewCategory = (RadGridView)Mapping.Mapping.GetControlByTag(form.Controls,"PlatCategoryGrid");
               
                radGridViewCategory.CellDoubleClick += new GridViewCellEventHandler((s, args) => {  this.Edit(form, radGridViewCategory, classname); });
                FullDataSource(form, classname);


                Mapping.Mapping.ActionsClassMap(form,this, classname);
            #endregion

            #region TariffPlat
                RadGridView radGridViewTariff = (RadGridView)Mapping.Mapping.GetControlByTag(form.Controls, "TariffPlatGrid");

                radGridViewTariff.CellDoubleClick += new GridViewCellEventHandler((s, args) => { this.Edit(form, radGridViewTariff, "TariffPlat"); });

                FullDataSource(form,  "TariffPlat");
                TariffPlatView tarif = new TariffPlatView(); 

                Mapping.Mapping.ActionsClassMap(form, tarif, "TariffPlat");
            #endregion
        }
        
    }
}
