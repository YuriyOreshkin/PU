using System.ComponentModel;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    [DisplayName("Специальная оценка условий труда")]
    public class SpecOcenkaUslTrudaView : BaseDictionaryView
    {
        public override void Load(Form form, string classname)
        {
            ThemeResolutionService.ApplyThemeToControlTree(form, ((RadForm)form).ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(((RadForm)form).ThemeName);

            #region SpecOcenkaUslTruda
            RadGridView radGridViewUslTruda = (RadGridView)Mapping.Mapping.GetControlByTag(form.Controls, "SpecOcenkaUslTrudaGrid");

            radGridViewUslTruda.CellDoubleClick += new GridViewCellEventHandler((s, args) => { this.Edit(form, radGridViewUslTruda, classname); });

            FullDataSource(form, classname);

            Mapping.Mapping.ActionsClassMap(form, this, classname);
            #endregion


            #region Tariff
            RadGridView radGridViewTariff = (RadGridView)Mapping.Mapping.GetControlByTag(form.Controls, "SpecOcenkaUslTrudaDopTariffGrid");

            radGridViewTariff.CellDoubleClick += new GridViewCellEventHandler((s, args) => { this.Edit(form, radGridViewTariff, "SpecOcenkaUslTrudaDopTariff"); });
            
            FullDataSource(form, "SpecOcenkaUslTrudaDopTariff");
          
            SpecOcenkaUslTrudaDopTariffView tarif = new SpecOcenkaUslTrudaDopTariffView();

            Mapping.Mapping.ActionsClassMap(form, tarif, "SpecOcenkaUslTrudaDopTariff");
            #endregion

        }


        [DisplayVisible(IsVisible =false)]
        public override void Add(Form form, RadGridView radGridView, string classname) { }
        [DisplayVisible(IsVisible =false)]
        public override void Delete(Form form, RadGridView radGridView, string classname) { }

    }
}
