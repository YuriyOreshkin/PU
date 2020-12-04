using System;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Dictionaries
{
    public partial class UslDosrNaznFormEdit : Telerik.WinControls.UI.RadForm
    {
       
        public UslDosrNaznFormEdit()
        {
            InitializeComponent();
            
        }

        private void radButtonSelectEdIzm_Click(object sender, System.EventArgs e)
        {
            BaseDictionaryEvents.LookUp(this, radDropDownListEdIzm, "UslDosrNaznEdIzm");
        }

        private void radButtonSetNULL_Click(object sender, System.EventArgs e)
        {
            radDropDownListEdIzm.SelectedValue = null;
        }

        private void radDropDownListEdIzm_SelectedValueChanged(object sender, EventArgs e)
        {
            if (radDropDownListEdIzm.SelectedValue != null)
            {
                radMaskedEditBoxEdIzmID.Value = (long)radDropDownListEdIzm.SelectedValue;
            }
            else
            {
                radMaskedEditBoxEdIzmID.Value = 0;
            }
        }
    }
}
