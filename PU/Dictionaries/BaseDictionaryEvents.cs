using PU.Models.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace PU.Dictionaries
{
    public static class BaseDictionaryEvents
    {


        private static Type GetFormListType(string classname)
        {
            Type formListType = Type.GetType("PU.Dictionaries." + classname + "FormList");
            if (formListType == null)
            {
                formListType = Type.GetType("PU.Dictionaries.BaseDictionaryFormList");
            }

            return formListType;
        }




        public static RadForm Dialog(RadForm form, string classname, string title="")
        {

            Type formListType = GetFormListType(classname);

            Type viewType = Type.GetType(Mapping.FullModelViewPath(classname));
            object view = Activator.CreateInstance(viewType);

            if (String.IsNullOrEmpty(title))
            {
                var attribute = viewType.GetCustomAttributes(true).FirstOrDefault(x => x is DisplayNameAttribute);
                if (attribute != null) {
                    title = ((DisplayNameAttribute)attribute).DisplayName;
                }
            }

            RadForm child = (RadForm)Activator.CreateInstance(formListType);

            child.Load += new EventHandler(delegate (object s, EventArgs args) { viewType.GetMethod("Load").Invoke(view, new object[] { child, classname }); });
           
            child.Text = title;
            child.Name = classname + "Window";
            child.Owner = form;
            child.ThemeName = form.ThemeName;
            child.ShowInTaskbar = false;
            return  child;

        }

        /// <summary>
        /// LookUp button click
        /// </summary>
        /// <param name="form"></param>
        /// <param name="control"></param>
        /// <param name="classname"></param>
        public static void LookUp(RadForm form, Control control, string classname)
        {
            var child = Dialog(form, classname);
            RadGridView radGridView = ((RadForm)child).Controls.OfType<RadGridView>().FirstOrDefault();


            radGridView.CellDoubleClick += new GridViewCellEventHandler((s, args) => { child.DialogResult = DialogResult.OK; });

            

            radGridView.DataBindingComplete += new GridViewBindingCompleteEventHandler((s, ev) =>
            {
                if (control is RadDropDownList)
                {
                    if (((RadDropDownList)control).SelectedValue != null)
                    {
                        var selected = radGridView.Rows.FirstOrDefault(x => x.Cells["ID"].Value.ToString() == ((RadDropDownList)control).SelectedValue.ToString());
                        if (selected != null)
                        {
                            selected.IsCurrent = true;
                        }
                    }

                }

            });

            if (child.ShowDialog() == DialogResult.OK)
            {
                if (radGridView.SelectedRows.Count > 0)
                {
                    if (control is RadDropDownList)
                    {
                        ((RadDropDownList)control).SelectedValue = radGridView.CurrentRow.Cells["ID"].Value;
                    }

                    if (control is RadTextBox)
                    {
                        ((RadTextBox)control).Text = radGridView.CurrentRow.Cells["Name"].Value.ToString();
                    }

                }
            };
        }

    }
}
