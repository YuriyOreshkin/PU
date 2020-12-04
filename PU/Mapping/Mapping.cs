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
using System.ComponentModel;

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
                var propentity = entity.GetType().GetProperty(property.Name);
                if (propentity != null) {
                    if (!IsSimple(property.PropertyType))
                    {
                        object valentity = propentity.GetValue(entity, null);
                        if (valentity != null)
                        {
                            property.SetValue(view, ModelClassViewMap(valentity, property.Name), null);
                        }
                    }
                    else {

                        property.SetValue(view, propentity.GetValue(entity, null), null);
                    }
                }
            }

            return view;
        }

        private static bool IsSimple(Type type)
        {
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal))
              || type.Equals(typeof(decimal?))
              || type.Equals(typeof(int))
              || type.Equals(typeof(int?))
              || type.Equals(typeof(short))
              || type.Equals(typeof(short?))
              || type.Equals(typeof(byte))
              || type.Equals(typeof(byte?))
              || type.Equals(typeof(long))
              || type.Equals(typeof(long?))
              || type.Equals(typeof(DateTime))
              || type.Equals(typeof(DateTime?)); 
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
        public static void GridViewClassMap(RadGridView gridView, string classname)
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

                    //Field 
                    if (attribute is DisplayFieldAttribute)
                    {

                        gridView.Columns[property.Name].FieldName= ((DisplayFieldAttribute)attribute).FieldName;

                    }

                    //Column Order 
                    if (attribute is DisplayAttribute)
                    {
                            var order = ((DisplayAttribute)attribute).GetOrder();
                            if (order != null )  
                                gridView.Columns.Move(gridView.Columns[property.Name].Index, (int)order);
                    }
                }

            }
        }


        private static void ActionsCommandBarMap(Form form, RadCommandBar commandBar, RadGridView radGridView, object view, string classname)
        {
            /*
            commandBar.Rows[0].Strips[0].OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            commandBar.Rows[0].Strips[0].OverflowButton.AddRemoveButtonsMenuItem.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            commandBar.Rows[0].Strips[0].OverflowButton.CustomizeButtonMenuItem.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            */

            foreach (MethodInfo method in view.GetType().GetMethods())
            {
                object[] attributes = method.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                 

                    if (attribute is DisplayVisibleAttribute)
                    {
                       
                        //Set action visibility
                        if (((DisplayVisibleAttribute)attribute).IsVisible)
                        {
                            CommandBarButton action = CreateAction(attributes.Any(a => a is DisplayNameAttribute) ? ((DisplayNameAttribute)attributes.First(a => a is DisplayNameAttribute)).DisplayName : method.Name);
                          
                            //Set button click
                            action.Click += new EventHandler(delegate (object s, EventArgs args) { method.Invoke(view, new object[] { form,radGridView, classname }); });

                            if (((DisplayVisibleAttribute)attribute).Separator == Separator.Left || ((DisplayVisibleAttribute)attribute).Separator == Separator.LeftRight) 
                            {
                                //Separator Left
                                commandBar.Rows[0].Strips[0].Items.Add(new CommandBarSeparator());
                            }

                            commandBar.Rows[0].Strips[0].Items.Add(action);

                            if (((DisplayVisibleAttribute)attribute).Separator == Separator.Right || ((DisplayVisibleAttribute)attribute).Separator == Separator.LeftRight)
                            {
                                //Separator Right
                                commandBar.Rows[0].Strips[0].Items.Add(new CommandBarSeparator());
                            }
                        }
                        
                    }
                }
            }
        }
       
        private static void ActionsContextMenuMap(Form form, RadContextMenu contextMenu, RadGridView radGridView, object view, string classname)
        {
            foreach (MethodInfo method in view.GetType().GetMethods())
            {
                object[] attributes = method.GetCustomAttributes(true);
                foreach (object attribute in attributes)
                {
                    
                    if (attribute is DisplayVisibleAttribute)
                    {
                        //Set action visibility
                        if (((DisplayVisibleAttribute)attribute).IsVisible)
                        {
                            RadMenuItem action = new RadMenuItem();
                            action.Text = attributes.Any(a => a is DisplayNameAttribute) ? ((DisplayNameAttribute)attributes.First(a => a is DisplayNameAttribute)).DisplayName : method.Name;

                            //Set button click
                            action.Click += new EventHandler(delegate (object s, EventArgs args) { method.Invoke(view, new object[] { form,radGridView, classname }); });

                            if (((DisplayVisibleAttribute)attribute).Separator == Separator.Left || ((DisplayVisibleAttribute)attribute).Separator == Separator.LeftRight)
                            {
                                //Separator Left
                                contextMenu.Items.Add(new RadMenuSeparatorItem());
                            }

                            contextMenu.Items.Add(action);

                            if (((DisplayVisibleAttribute)attribute).Separator == Separator.Right || ((DisplayVisibleAttribute)attribute).Separator == Separator.LeftRight)
                            {
                                //Separator Left
                                contextMenu.Items.Add(new RadMenuSeparatorItem());
                            }

                        }
                    }
                }
            }
        }


        private static CommandBarButton CreateAction(string text)
        {
            CommandBarButton action = new CommandBarButton();
           
            action.Image = null;
            action.Text = text;
            action.DrawText = true;
            action.ToolTipText = text;
            
            return action;
        }

        public static void ActionsClassMap(Form form, object view, string classname)
        {
            RadGridView  radGridView = (RadGridView)Mapping.GetControlByTag(form.Controls, classname + "Grid");
            RadCommandBar commandBar = (RadCommandBar)Mapping.GetControlByTag(form.Controls, classname+ "Actions");

            if (commandBar != null)
                 ActionsCommandBarMap(form, commandBar,radGridView, view, classname);

            RadContextMenu contextMenu = new RadContextMenu();  
            ActionsContextMenuMap(form, contextMenu,radGridView, view, classname);

            radGridView.ContextMenuOpening +=new ContextMenuOpeningEventHandler((s,arg)=> {

                GridFilterCellElement cell = arg.ContextMenuProvider as GridFilterCellElement;
                if (cell == null)
                {
                    arg.ContextMenu = contextMenu.DropDown;
                }
            });

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
                Control control = GetControlByTag(form.Controls, property.Name); 
                //foreach (Control control in form.Controls)
                //{
                //    if (control.Tag != null && control.Tag.ToString() == property.Name)
                if (control != null)
                    {
                        //LookUp field
                       var attributes = property.GetCustomAttributes(true);
                       object value = property.GetValue(item, null);

                        foreach (var attribute in attributes)
                        {
                            if (attribute is LookUpAttribute)
                            {

                                value = new LookUp()
                                {
                                    DataSource = Mapping.DataSourceMap(((LookUpAttribute)attribute).DataSource),
                                    ValueMember = ((LookUpAttribute)attribute).ValueMember,
                                    DisplayMember = ((LookUpAttribute)attribute).DisplayMember,
                                    Value = value
                                    
                                };
                            }

                            //ReadOnly
                            if (attribute is ModelViews.ReadOnlyAttribute)
                            {
                                ((RadTextBoxBase)control).ReadOnly = ((ModelViews.ReadOnlyAttribute)attribute).IsReadOnly;
                            }

                    }
                    
                        ValueControlMap(control, value);
                   
                }
                //}
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
                var val = Convert.ToDecimal(value);
                if (val >= ((RadSpinEditor)control).Minimum && val <= ((RadSpinEditor)control).Maximum)
                {
                    ((RadSpinEditor)control).Value = val;
                }
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
                var lookup = value as LookUp;
                if (lookup != null)
                {
                    dropdownlist.ValueMember = lookup.ValueMember;
                    dropdownlist.DisplayMember = lookup.DisplayMember;
                    dropdownlist.DataSource = lookup.DataSource;
                    dropdownlist.SelectedValue = lookup.Value != null ? lookup.Value.GetType().GetProperty(lookup.ValueMember).GetValue(lookup.Value, null) : lookup.Value;
                }
                else {

                    dropdownlist.SelectedValue = value;
                }
            }

            //Masked
            if (control is RadMaskedEditBox)
            {
                ((RadMaskedEditBox)control).Value = value;
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

            foreach (PropertyInfo property in type.GetProperties())
            {
                Control control = GetControlByTag(form.Controls, property.Name);
                if (control != null)
                {
                    property.SetValue(item, ControlValueMap(property.PropertyType, control), null);
                }
            }
              
        }


        private static object ConvertValueType(Type type, string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            if (value == "0")
                return null;

            if (type.Equals(typeof(string))) 
                return (string)value;

            if (type.Equals(typeof(int)) || type.Equals(typeof(int?)))
                return int.Parse(value);

            if (type.Equals(typeof(short)))
                return short.Parse(value);

            if (type.Equals(typeof(long)) || type.Equals(typeof(long?)))
                return long.Parse(value);

            if (type.Equals(typeof(decimal?)) || type.Equals(typeof(decimal)))
                return decimal.Parse(value);

            if (type.Equals(typeof(DateTime)) || type.Equals(typeof(DateTime?)))
                return  DateTime.Parse(value);

            if (type.Equals(typeof(bool)))
                return bool.Parse(value);

            return value;
        }

        private static object ControlValueMap(Type type, Control control)
        {

           
            //LookUp
            if (control is RadDropDownList)
            {
                if (((RadDropDownList)control).SelectedItem != null)
                    if (((RadDropDownList)control).SelectedItem.DataBoundItem != null)
                    {
                        return ((RadDropDownList)control).SelectedItem.DataBoundItem;
                    }
                    else {

                        return ((RadDropDownList)control).SelectedItem.Text;
                    }
            }


            return ConvertValueType(type, control.Text);
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
    

