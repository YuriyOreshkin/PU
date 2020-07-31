using PU.Classes;
using PU.UserAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using System.Linq;
using System.Reflection;
using Telerik.WinControls.UI;

namespace PU.Service
{
    public partial class ObjectsToForms : Telerik.WinControls.UI.RadForm
    {
        xaccessEntities xaccessDB = new xaccessEntities();

        public ObjectsToForms()
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

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ObjectsToForms_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            objectCatDDL.DataSource = null;
            objectCatDDL.DisplayMember = "Name";
            objectCatDDL.ValueMember = "ID";
            objectCatDDL.ShowItemToolTips = true;
            objectCatDDL.DataSource = xaccessDB.AccessCategory; ;
            objectCatDDL.SelectedIndexChanged += (s, с) => objectCatDDL_SelectedIndexChanged();
            objectCatDDL.SelectedIndex = 0;
            objectCatDDL_SelectedIndexChanged();


            updateAllFormsGridView();

        }

        private void updateAllFormsGridView()
        {
            allFormsGridView.Rows.Clear();

            List<Type> forms = new List<Type>();
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                forms.AddRange(from t in asm.GetTypes() where t.IsSubclassOf(typeof(Form)) select t);
            }

            var tempList = xaccessDB.FormsToObjects.Select(x => x.Form).ToList();

            foreach (var item in forms.Where(x => !tempList.Contains(x.Name)))
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.allFormsGridView.MasterView);
                rowInfo.Cells["Name"].Value = item.Name;
                allFormsGridView.Rows.Add(rowInfo);


            }
        }

        private void objectCatDDL_SelectedIndexChanged()
        {
            if (!String.IsNullOrEmpty(objectCatDDL.Text) && objectCatDDL.SelectedItem != null)
            {
                long id = (long)objectCatDDL.SelectedValue;

                objectsGridView.DataSource = null;
                objectsGridView.Rows.Clear();

                objectsGridView.DataSource = xaccessDB.AccessObject.Where(x => x.AccessCategoryID == id).ToList();

                foreach (var col in objectsGridView.Columns)
                {
                    if (col.Name == "Name")
                    {
                        col.HeaderText = "Категория";
                    }
                    else
                    {
                        col.IsVisible = false;
                    }
                }

            }

        }

        private void objectsGridView_CurrentRowChanged(object sender, Telerik.WinControls.UI.CurrentRowChangedEventArgs e)
        {
            updateRefFormsGridView();
        }

        private void updateRefFormsGridView()
        {
            if (objectsGridView.RowCount > 0 && objectsGridView.CurrentRow != null && objectsGridView.CurrentRow.Cells["ID"].Value != null)
            {
                long id = (long)objectsGridView.CurrentRow.Cells["ID"].Value;

                this.refFormsGridView.TableElement.BeginUpdate();

                refFormsGridView.DataSource = xaccessDB.FormsToObjects.Where(c => c.ObjectsID == id);

                foreach (var col in refFormsGridView.Columns)
                {
                    if (col.Name == "Form")
                    {
                        col.HeaderText = "Форма";
                    }
                    else
                    {
                        col.IsVisible = false;
                    }
                }
                this.refFormsGridView.TableElement.EndUpdate();

            }
        }

        private void toLeftBtn_Click(object sender, EventArgs e)
        {
            if (objectsGridView.RowCount > 0 && objectsGridView.CurrentRow != null && objectsGridView.CurrentRow.Cells["ID"].Value != null)
            {
                string formName = allFormsGridView.CurrentRow.Cells["Name"].Value.ToString();
                long id = (long)objectsGridView.CurrentRow.Cells["ID"].Value;
                if (!refFormsGridView.Rows.Any(x => x.Cells["Form"].Value.ToString() == formName))
                {
                    xaccessDB.AddToFormsToObjects(new FormsToObjects { ObjectsID = id, Form = formName });
                    xaccessDB.SaveChanges();
                    updateRefFormsGridView();
                    updateAllFormsGridView();

                }

            }

        }

        private void toRightBtn_Click(object sender, EventArgs e)
        {
            if (refFormsGridView.RowCount > 0 && refFormsGridView.CurrentRow != null && refFormsGridView.CurrentRow.Cells["ID"].Value != null)
            {
                long id = (long)refFormsGridView.CurrentRow.Cells["ID"].Value;

                var form = xaccessDB.FormsToObjects.First(x => x.ID == id);
                xaccessDB.DeleteObject(form);
                xaccessDB.SaveChanges();

                updateRefFormsGridView();
                updateAllFormsGridView();

            }
        }

        private void allFormsGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            toLeftBtn_Click(null, null);
        }




    }
}
