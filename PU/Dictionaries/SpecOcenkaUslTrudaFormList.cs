using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PU.Models;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.IO;
using PU.Classes;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using PU.Models.ModelViews;
using PU.Models.Mapping;
using Telerik.WinControls.Data;

namespace PU.Dictionaries
{
    partial class SpecOcenkaUslTrudaFormList : Telerik.WinControls.UI.RadForm
    {


        public SpecOcenkaUslTrudaFormList()
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
        }


        private void radGridViewSpecOcenkaUslTrudaDopTariff_SelectionChanged(object sender, EventArgs e)
        {
            if (radGridViewSpecOcenkaUslTrudaDopTariff.FilterDescriptors.Count > 0)
            {
                for (int i = 0; i < radGridViewSpecOcenkaUslTrudaDopTariff.FilterDescriptors.Count; i++)
                {
                    if (radGridViewSpecOcenkaUslTrudaDopTariff.FilterDescriptors[i].PropertyName == "SpecOcenkaUslTrudaID")
                    {
                        radGridViewSpecOcenkaUslTrudaDopTariff.FilterDescriptors.RemoveAt(i);
                    }
                }
            }

            if (radGridViewSpecOcenkaUslTruda.CurrentRow != null && radGridViewSpecOcenkaUslTruda.CurrentRow is GridViewDataRowInfo)
            {
                var id = radGridViewSpecOcenkaUslTruda.SelectedRows[0].Cells["ID"].Value;

                this.radGridViewSpecOcenkaUslTrudaDopTariff.FilterDescriptors.Add("SpecOcenkaUslTrudaID", FilterOperator.IsEqualTo, id);
            }
            else
            {
                this.radGridViewSpecOcenkaUslTrudaDopTariff.FilterDescriptors.Add("SpecOcenkaUslTrudaID", FilterOperator.IsEqualTo,0);
            }
        }

        
    }
}
