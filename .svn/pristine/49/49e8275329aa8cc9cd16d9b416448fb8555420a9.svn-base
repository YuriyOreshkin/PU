using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PU.Models;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU.FormsRSW2014
{
    public partial class DolgnFrm : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public Dolgn dolgn { get; set; }
        private ListSortDirection sortOrder = ListSortDirection.Ascending; 

        public DolgnFrm()
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

        public void dataGrid_upd()
        {
            this.radGridView1.TableElement.BeginUpdate(); 
            this.radGridView1.EnableHotTracking = true;
            this.radGridView1.MasterTemplate.EnableSorting = true;

            BindingSource b = new BindingSource();
            b.DataSource = db.Dolgn;
            radGridView1.DataSource = null;
            radGridView1.DataSource = b;

            radGridView1.Columns["ID"].IsVisible = false;
            radGridView1.Columns["StajOsn"].IsVisible = false;
            radGridView1.Columns["StajLgot"].IsVisible = false;

            radGridView1.Columns["Name"].HeaderText = "Наименование";

            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill; 
            this.radGridView1.TableElement.EndUpdate(true);              
            // Add grid sort expression.             
            this.radGridView1.MasterTemplate.SortDescriptors.Add("Name", sortOrder);

            //radGridView1.Refresh();
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DolgnFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            dataGrid_upd();
        }

        private void radGridView1_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            db.SaveChanges();
        }

        private void radGridView1_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            db.SaveChanges();
        }

        private void radGridView1_UserDeletedRow(object sender, GridViewRowEventArgs e)
        {
            db.SaveChanges();
        }

        private void radGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu.Items[0].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            e.ContextMenu.Items[1].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            e.ContextMenu.Items[2].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
        }

        private void radGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            if (action == "selection")
            {
                btnSelection_Click(null, null);
            }
        }

        private void btnSelection_Click(object sender, EventArgs e)
        {
            if (action == "selection" && radGridView1.RowCount != 0 && radGridView1.CurrentRow.Index >= 0)
            {
             //   int rowindex = radGridView1.CurrentRow.Index;
                var id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());

                dolgn = new Dolgn
                {
                    ID = id,
                    Name = radGridView1.CurrentRow.Cells[1].Value.ToString()
                };


                this.btnSelection.Visible = false;
                this.Close();
            }
        }
    }
}
