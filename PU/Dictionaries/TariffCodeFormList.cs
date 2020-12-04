using System;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.Data;

namespace PU.Dictionaries
{
    partial class TariffCodeFormList : Telerik.WinControls.UI.RadForm
    {


        public TariffCodeFormList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Перехват нажатия на ESC для закрытия формы
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void radGridViewBaseDictionary_CreateRow(object sender, GridViewCreateRowEventArgs e)
        {

            if (e.RowType == typeof(GridDataRowElement))
            {
                e.RowType = typeof(CustomRowElement);
            }
            else if (e.RowType == typeof(GridTableHeaderRowElement))
            {
                e.RowType = typeof(CustomHeaderElement);
            }
        }

        private void radGridViewTariffCode_SelectionChanged(object sender, EventArgs e)
        {
            var filterColumnName = "TariffCodeID";

            if (radGridViewTariffCodePlatCat.FilterDescriptors.Count > 0)
            {
                for (int i = 0; i < radGridViewTariffCodePlatCat.FilterDescriptors.Count; i++)
                {
                    if (radGridViewTariffCodePlatCat.FilterDescriptors[i].PropertyName == filterColumnName)
                    {
                        radGridViewTariffCodePlatCat.FilterDescriptors.RemoveAt(i);
                    }
                }
            }

            if (radGridViewTariffCode.CurrentRow != null && radGridViewTariffCode.CurrentRow is GridViewDataRowInfo)
            {
                var id = radGridViewTariffCode.SelectedRows[0].Cells["ID"].Value;

                this.radGridViewTariffCodePlatCat.FilterDescriptors.Add(filterColumnName, FilterOperator.IsEqualTo, id);
               
            }
            else
            {
                this.radGridViewTariffCodePlatCat.FilterDescriptors.Add(filterColumnName, FilterOperator.IsEqualTo, 0);
            }
        }

        
    }
}
