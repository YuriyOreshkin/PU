using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    public class TariffCodePlatCatView : BaseDictionaryActions
    {
        [DisplayVisible(IsVisible =false)]
        public long ID { get; set; }

        public long? PlatCategoryID { get; set; }

        [DisplayVisible(IsVisible =false)]
        public long? TariffCodeID { get; set; }

        [DisplayName("Код")]
        [DisplayWidth(MaxWidth = 120)]
        [DisplayField("PlatCategory.Code")]
        //[LookUp(DataSource = "PlatCategory", ValueMember = "ID", DisplayMember = "Name")]
        public virtual PlatCategoryView PlatCategory { get; set; }

        [DisplayName("Наименование")]
        [WrapText(true)]
        public virtual string PlatCategoryName {
            get
            {
                return this.PlatCategory.Name;
            }
        }


        [DisplayVisible(IsVisible =false)]
        public override void Close(Form form, RadGridView radGridView, string classname) { }

        [DisplayVisible(IsVisible =false)]
        public override void Edit(Form form, RadGridView radGridView, string classname) { }

        [DisplayVisible(IsVisible =false)]
        public override void Synchronization(Form form, RadGridView radGridView, string classname) { }

        [DisplayVisible(IsVisible =true)]
        [DisplayName("Добавить")]
        public override void Add(Form form, RadGridView radGridView, string classname)
        {

            var child = Dictionaries.BaseDictionaryEvents.Dialog((RadForm)form, "PlatCategory");

            RadGridView childRadGridView = (RadGridView)Mapping.Mapping.GetControlByTag( ((RadForm)child).Controls, "PlatCategoryGrid");



            //childRadGridView.CellDoubleClick += new GridViewCellEventHandler((s, args) => { child.DialogResult = DialogResult.OK; });
            child.DialogResult = DialogResult.OK;

            childRadGridView.AllowEditRow = true;

            childRadGridView.DataBindingComplete += new GridViewBindingCompleteEventHandler((s, ev) =>
            {

                //Read only
                foreach (var column in childRadGridView.Columns)
                {
                    column.ReadOnly = true;
                }


                GridViewCheckBoxColumn checkBoxColumn = new GridViewCheckBoxColumn();
                checkBoxColumn.Name = "Select";
                checkBoxColumn.Width = 20;
                checkBoxColumn.EnableHeaderCheckBox = true;
                childRadGridView.Columns.Insert(0, checkBoxColumn);

                GridDataView dataView = radGridView.MasterTemplate.DataView as GridDataView;

                foreach (GridViewRowInfo row in dataView.Indexer.Items)
                {
                    var checkedRow = childRadGridView.Rows.FirstOrDefault(x => (long)x.Cells["ID"].Value == (long)row.Cells["PlatCategoryID"].Value);
                    if (checkedRow != null)
                    {
                        checkedRow.Cells[0].Value = true;
                    }
                }
            });



            //if (child.ShowDialog() == DialogResult.OK)
            //{
                child.ShowDialog();
                var selected = (childRadGridView.MasterTemplate.DataView as GridDataView).Indexer.Items.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true).Select(s => (long)s.Cells["ID"].Value).ToList();
                var values = (radGridView.MasterTemplate.DataView as GridDataView).Indexer.Items.ToList();


               IEnumerable<long>  listAdd = selected.Except(values.Select(i => (long)i.Cells["PlatCategoryID"].Value));
            //for Add
            foreach (var id in listAdd)
            {
                object entity = new TariffCodePlatCat();
                ((TariffCodePlatCat)entity).PlatCategoryID = id;
                ((TariffCodePlatCat)entity).PlatCategory = db.Set<PlatCategory>().FirstOrDefault(c => c.ID == id);

                var view = Mapping.Mapping.ModelClassViewMap(entity, classname);

                for (var i = 0; i < radGridView.FilterDescriptors.Count; i++)
                {
                    view.GetType().GetProperty(radGridView.FilterDescriptors[i].PropertyName).SetValue(view, radGridView.FilterDescriptors[i].Value, null);
                }
                Mapping.Mapping.ViewModelClassMap(view, ref entity);

                try
                {
                    db.Entry(entity).State = System.Data.Entity.EntityState.Added;
                    db.SaveChanges();

                    view.GetType().GetProperty("ID").SetValue(view, entity.GetType().GetProperty("ID").GetValue(entity, null), null);

                    ((BindingSource)radGridView.DataSource).Add(view);
                    radGridView.Focus();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
                //  }
            };
               IEnumerable<long> listDelete = values.Select(i => (long)i.Cells["PlatCategoryID"].Value).Except(selected);
                //for Add
                foreach (var id in listDelete)
                {
                    var view = values.FirstOrDefault(v =>v.DataBoundItem != null && (long)v.Cells["PlatCategoryID"].Value == id);
                   
                    var entity = db.Set<TariffCodePlatCat>().FirstOrDefault(t => t.ID == ((TariffCodePlatCatView)view.DataBoundItem).ID);


                    db.Entry(entity).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();

                    ((BindingSource)radGridView.DataSource).Remove(view.DataBoundItem);
                radGridView.Focus();

            };

        }



    }
}
