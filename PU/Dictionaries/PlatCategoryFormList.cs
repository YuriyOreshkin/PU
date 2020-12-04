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
    partial class PlatCategoryFormList : Telerik.WinControls.UI.RadForm
    {


        public PlatCategoryFormList()
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


        private void radGridViewPlatCategory_SelectionChanged(object sender, EventArgs e)
        {
            if (radGridViewTariffPlat.FilterDescriptors.Count > 0)
            {
                for (int i = 0; i < radGridViewTariffPlat.FilterDescriptors.Count; i++)
                {
                    if (radGridViewTariffPlat.FilterDescriptors[i].PropertyName == "PlatCategoryID")
                    {
                        radGridViewTariffPlat.FilterDescriptors.RemoveAt(i);
                    }
                }
            }

            if (radGridViewPlatCategory.CurrentRow != null && radGridViewPlatCategory.CurrentRow is GridViewDataRowInfo)
            {
                var id = radGridViewPlatCategory.SelectedRows[0].Cells["ID"].Value;

                this.radGridViewTariffPlat.FilterDescriptors.Add("PlatCategoryID", FilterOperator.IsEqualTo, id);
            }
            else
            {
                this.radGridViewTariffPlat.FilterDescriptors.Add("PlatCategoryID", FilterOperator.IsEqualTo,0);
            }
        }

     

        private void radButtonSelectPlatCategoryRaschPer_Click(object sender, EventArgs e)
        {
            BaseDictionaryEvents.LookUp(this, radDropDownListPlatCategoryRaschPer, "PlatCategoryRaschPer");
        }

        private void radDropDownListPlatCategoryRaschPer_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

           
            if (radGridViewPlatCategory.FilterDescriptors.Count > 0)
            {
                int i = 0;
                while (i < radGridViewPlatCategory.FilterDescriptors.Count)
                {
                    if (radGridViewPlatCategory.FilterDescriptors[i].PropertyName == "DateBegin" || radGridViewPlatCategory.FilterDescriptors[i].PropertyName == "DateEnd")
                    {
                        radGridViewPlatCategory.FilterDescriptors.RemoveAt(i);
                    }
                    else {
                        i++;
                    }
                       
                    
                }
            }


            if (this.radDropDownListPlatCategoryRaschPer.SelectedIndex > -1)
            {
                var period = ((RadDropDownList)sender).SelectedItem.DataBoundItem as Models.ModelViews.PlatCategoryRaschPerView;
                if (period != null)
                {
                    if (period.DateBegin != null)
                    {
                        FilterDescriptor datebegin = new FilterDescriptor();
                        datebegin.PropertyName = "DateBegin";
                        datebegin.Operator = FilterOperator.IsGreaterThanOrEqualTo;
                        datebegin.Value = period.DateBegin;
                        datebegin.IsFilterEditor = true;
                        this.radGridViewPlatCategory.FilterDescriptors.Add(datebegin);
                    }

                    CompositeFilterDescriptor compositeFilter = new CompositeFilterDescriptor();
                    compositeFilter.FilterDescriptors.Add(new FilterDescriptor("DateEnd", FilterOperator.IsNull,null));
                    compositeFilter.IsFilterEditor = true;

                    if (period.DateEnd != null)
                    {
                        compositeFilter.FilterDescriptors.Add(new FilterDescriptor("DateEnd", FilterOperator.IsLessThanOrEqualTo, period.DateEnd));
                        compositeFilter.LogicalOperator = FilterLogicalOperator.Or;


                        FilterDescriptor datebend = new FilterDescriptor();
                        datebend.PropertyName = "DateBegin";
                        datebend.Operator = FilterOperator.IsLessThanOrEqualTo;
                        datebend.Value = period.DateEnd;
                        datebend.IsFilterEditor = true;
                        this.radGridViewPlatCategory.FilterDescriptors.Add(datebend);
                    }

                    this.radGridViewPlatCategory.FilterDescriptors.Add(compositeFilter);
                }

            }
        }


       
    }
}
