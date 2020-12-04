using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU.Models.ModelViews
{
    public class BaseDictionaryActionsEdit
    {
        public virtual void LoadForm(Form form, object item)
        {
            ThemeResolutionService.ApplyThemeToControlTree(form, ((RadForm)form).ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(((RadForm)form).ThemeName);

            Mapping.Mapping.ObjectControlsMap(form, item);
        }

        public virtual void Save(Form form, object item)
        {
            Type type = item.GetType();
            object view = Activator.CreateInstance(type);
            Mapping.Mapping.ControlsObjectMap(form, view);

            if (isValidate(form, view))
            {
                Mapping.Mapping.ControlsObjectMap(form, item);
                form.DialogResult = DialogResult.OK;
            }
            else
            {

                form.DialogResult = DialogResult.None;
            }
        }

        public virtual bool isValidate(Form form, object obj)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(obj, null, null);

            if (!Validator.TryValidateObject(obj, context, results, true))
            {
                ErrorProvider errorProvider = new ErrorProvider();

                foreach (var error in results)
                {
                    Control control = Mapping.Mapping.GetControlByTag(form.Controls, error.MemberNames.FirstOrDefault());
                    if (control != null)
                    {
                        errorProvider.SetError(control, error.ErrorMessage);
                    }
                }

                return false;
            }
            else
            {

                return true;
            }

        }
    }
}
