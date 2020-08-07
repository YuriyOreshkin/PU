using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Dynamic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.DataAnnotations;
using PU.Models.ModelViews;
using Telerik.WinControls.UI;
using static System.Windows.Forms.Control;

namespace PU.Models.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public static class Mapping
    {
        public static string FullModelPath(string classname)
        {
            return "PU.Models." + classname;
        }

        public static string FullModelViewPath(string classname)
        {
            return "PU.Models.ModelViews." + classname + "View";
        }

        /// <summary>
        ///  Convert object from type(class) EF to type View 
        /// </summary>
        /// <param name="entity">EF object</param>
        /// <param name="classname">EF class name</param>
        /// <returns></returns>
        public static object ModelClassViewMap(object entity, string classname)
        {
            //Create View class in PU.Models.ModelViews namespace
            //All type view must have the same classname +View 
            //View class must have the same names properties
            Type viewType = Type.GetType(FullModelViewPath(classname));

            object view = Activator.CreateInstance(viewType);

            foreach (PropertyInfo property in viewType.GetProperties())
            {

                property.SetValue(view, entity.GetType().GetProperty(property.Name).GetValue(entity, null), null);

            }

            return view;
        }


        public static void ViewModelClassMap(object view, ref object entity)
        {


            foreach (PropertyInfo property in entity.GetType().GetProperties().Where(p => !p.GetGetMethod().IsVirtual))
            {

                property.SetValue(entity, view.GetType().GetProperty(property.Name).GetValue(view, null), null);

            }

        }

        /// <summary>
        /// GridView tune by  class attributes
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="classname"></param>
        public static void GridViewClassMap(Telerik.WinControls.UI.RadGridView gridView, string classname)
        {
            Type viewType = Type.GetType(FullModelViewPath(classname));

            foreach (PropertyInfo property in viewType.GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    //Set column visibility
                    if (attribute is DisplayVisibleAttribute)
                    {
                        gridView.Columns[property.Name].IsVisible = ((DisplayVisibleAttribute)attribute).IsVisible;
                    }

                    //Set column format
                    if (attribute is DisplayFormatAttribute)
                    {
                        gridView.Columns[property.Name].FormatString = ((DisplayFormatAttribute)attribute).DataFormatString;
                    }


                    // WrapText
                    if (attribute is WrapTextAttribute)
                    {

                        gridView.Columns[property.Name].WrapText = ((WrapTextAttribute)attribute).WrapText;

                    }

                    //Width
                    if (attribute is DisplayWidthAttribute)
                    {
                        if (((DisplayWidthAttribute)attribute).MaxWidth != 0)
                        {
                            gridView.Columns[property.Name].MaxWidth = ((DisplayWidthAttribute)attribute).MaxWidth;
                        }
                    }

                    //Filtrable
                    if (attribute is FiltrableAttribute)
                    {

                        gridView.Columns[property.Name].AllowFiltering = ((FiltrableAttribute)attribute).IsFiltrable;

                    }
                }

            }
        }



        public static void ActionsClassMap(Form form, object view,string classname)
        {
            
            Control control = GetControlByTag(form.Controls, "Actions");

            if (control != null && control is RadCommandBar)
            {
                foreach (var action in ((RadCommandBar)control).Rows[0].Strips[0].Items.OfType<CommandBarButton>())
                {
                    action.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;

                    foreach (MethodInfo method in view.GetType().GetMethods())
                    {
                        if (action.Tag.ToString() == method.Name)
                        {
                            object[] attributes = method.GetCustomAttributes(true);
                            foreach (object attribute in attributes)
                            {
                                //Set column visibility
                                if (attribute is DisplayVisibleAttribute)
                                {
                                    if (((DisplayVisibleAttribute)attribute).IsVisible)
                                    {
                                        action.Visibility = Telerik.WinControls.ElementVisibility.Visible;
                                        ((CommandBarButton)action).Click += new EventHandler(delegate (object s, EventArgs args) { method.Invoke(view, new object[] { form, classname }); });
                                    }
                                }
                            }

                        }

                    }
                }

            }

        }
    


        /// <summary>
        /// Set values(get from object ) for controls on Form 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="item"></param>
        public static void ObjectControlsMap(Form form, object item)
        {
            Type type = item.GetType();

            foreach (PropertyInfo property in type.GetProperties())
            {
                foreach (Control control in form.Controls)
                {
                    if (control.Tag != null && control.Tag.ToString() == property.Name)
                    {
                       var attribute = property.GetCustomAttributes(true).FirstOrDefault(a => a is LookUpAttribute);
                       object value = null;

                        if (attribute != null)
                        {

                            value = new LookUp()
                            {
                                DataSource = Mapping.DataSourceMap(((LookUpAttribute)attribute).DataSource),
                                ValueMember = ((LookUpAttribute)attribute).ValueMember,
                                DisplayMember = ((LookUpAttribute)attribute).DisplayMember,
                                Value = property.GetValue(item, null)
                            };
                        }
                        else {

                            value = property.GetValue(item, null);
                        }

                        ValueControlMap(control, value);
                    }
                }
            }
        }

        /// <summary>
        /// Set value for control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="value"></param>
        private static void ValueControlMap(Control control, object value)
        {
            //String
            if (control is RadTextBox)
            {
                ((RadTextBox)control).Text = (string)value;
            }

            //DateTime
            if (control is RadDateTimePicker)
            {
                ((RadDateTimePicker)control).Value = Convert.ToDateTime(value);
            }

            //Decimal, int
            if (control is RadSpinEditor)
            {
                ((RadSpinEditor)control).Value = Convert.ToDecimal(value);
            }

            //bool
            if (control is RadCheckBox)
            {
                ((RadCheckBox)control).Checked =(bool)value;
            }

            //lookup
            if (control is RadDropDownList)
            {
                var dropdownlist = ((RadDropDownList)control);
                var lookup = (LookUp)value;

                dropdownlist.DataSource = lookup.DataSource;
                dropdownlist.ValueMember = lookup.ValueMember;
                dropdownlist.DisplayMember = lookup.DisplayMember;
                dropdownlist.SelectedValue = lookup.Value;
            }
        }


        /// <summary>
        /// Set values for object from controls on Form 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="item"></param>
        public static void ControlsObjectMap(Form form, object item)
        {
            Type type = item.GetType();
            
                foreach (Control control in form.Controls)
                {
                    if (control.Tag != null)
                    {
                        PropertyInfo property = type.GetProperties().FirstOrDefault(name=>name.Name == control.Tag.ToString());
                        if (property != null)
                        {
                            property.SetValue(item, ControlValueMap(control),null);
                        }
                }
            }
        }

        private static object ControlValueMap(Control control)
        {
            //String
            if (control is RadTextBox)
            {
               return ((RadTextBox)control).Text; 
            }

            //DateTime
            if (control is RadDateTimePicker)
            {
                if (((RadDateTimePicker)control).Value == DateTime.MinValue)
                {
                    return null;

                }
                else {

                    return ((RadDateTimePicker)control).Value;
                }
            }

            //Decimal, int
            if (control is RadSpinEditor)
            {
                return ((RadSpinEditor)control).Value;
            }

            //bool
            if (control is RadCheckBox)
            {
                return ((RadCheckBox)control).Checked;
            }

            //LookUp
            if (control is RadDropDownList)
            {
                return ((RadDropDownList)control).SelectedValue;
            }

            return null;
        }



        public static Control GetControlByTag(ControlCollection controls, string tagname)
        {


            foreach (Control control in controls)
            {
                if (control.Tag != null && control.Tag.ToString() == tagname)
                {
                    return control;
                }
                else
                {
                    if (control.HasChildren)
                    {
                        Control found = GetControlByTag(control.Controls, tagname);
                        if (found != null) return found; 
                    }
                   
                }
            }

            return null;
        }



        public static List<dynamic> DataSourceMap(string classname)
        {
            pu6Entities db = new pu6Entities();
            List<dynamic> list = new List<dynamic>();
            var dbset = db.Set(Type.GetType(FullModelPath(classname)));
            foreach (var entity in dbset)
            {
                list.Add(Mapping.ModelClassViewMap(entity, classname));
            }

            return list;
            
        }

    }
}
    

