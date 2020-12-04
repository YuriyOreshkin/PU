using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using PU.Models.Mapping;
using Telerik.WinControls;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.ComponentModel;
using PU.Classes;

namespace PU.Models.ModelViews
{
    public class BaseDictionaryActions: BaseDictionaryActionsEdit
    {
        protected DbContext db;
        public BaseDictionaryActions()
        {
            db = new pu6Entities();
        }

        [DisplayVisible(IsVisible =true)]
        [DisplayName("Добавить")]
        public virtual void Add(Form form, RadGridView radGridView, string classname) {


                Type entityType = Type.GetType(Mapping.Mapping.FullModelPath(classname));
                var  entity = Activator.CreateInstance(entityType, null);
                
            
                var view = Mapping.Mapping.ModelClassViewMap(entity, classname);

                //Default by filter
                DefaultValue(view, radGridView);


            Type formEditType = Type.GetType("PU.Dictionaries." + classname + "FormEdit");
                if (formEditType == null)
                {
                    formEditType = Type.GetType("PU.Dictionaries.BaseDictionaryFormEdit");
                }
               

                RadForm formEdit = (RadForm)Activator.CreateInstance(formEditType);
                formEdit.Load += new EventHandler(delegate (object s, EventArgs args) { this.LoadForm(formEdit, view); });


                  formEdit.Text = "Добавление записи";
                
                  Control saveAction = Mapping.Mapping.GetControlByTag(formEdit.Controls, "Save");

                    if (saveAction != null)
                    {
                        ((RadButton)saveAction).Click += new EventHandler(delegate (object s, EventArgs args) { this.Save(formEdit, view); });
                    }

                formEdit.Owner = form;
                formEdit.ThemeName = ((RadForm)form).ThemeName;

            if (formEdit.ShowDialog() == DialogResult.OK)
            {


                Mapping.Mapping.ViewModelClassMap(view, ref entity);
                try
                {
                    db.Entry(entity).State = EntityState.Added;
                    db.SaveChanges();

                    view.GetType().GetProperty("ID").SetValue(view, entity.GetType().GetProperty("ID").GetValue(entity, null), null);

                    ((BindingSource)radGridView.DataSource).Add(view);
                    radGridView.Focus();
                }
                catch (Exception ex)
                {

                }
            };
            
        }


        //Default by filter
        private static void DefaultValue(object view, RadGridView radGridView)
        {
            for (var i = 0; i < radGridView.FilterDescriptors.Count; i++)
            {
                view.GetType().GetProperty(radGridView.FilterDescriptors[i].PropertyName).SetValue(view, radGridView.FilterDescriptors[i].Value,null); 
             }
                    
        }

        [DisplayVisible(IsVisible =true)]
        [DisplayName("Редактировать")]
        public virtual void Edit(Form form, RadGridView radGridView,string classname) {
          
            //RadGridView radGridView = form.Controls.OfType<RadGridView>().FirstOrDefault();
            
            if (radGridView != null && radGridView.CurrentRow != null)
            {
                var selected = radGridView.SelectedRows[0].DataBoundItem;
                Type formEditType = Type.GetType("PU.Dictionaries." + classname + "FormEdit");
                if (formEditType == null)
                {
                    formEditType = Type.GetType("PU.Dictionaries.BaseDictionaryFormEdit");
                }

                RadForm formEdit =(RadForm)Activator.CreateInstance(formEditType);

                formEdit.Load += new EventHandler(delegate (object s, EventArgs args) { this.LoadForm(formEdit,selected); });

                formEdit.Text = "Редактирование записи";

                Control saveAction = Mapping.Mapping.GetControlByTag(formEdit.Controls, "Save");
                if (saveAction != null)
                {
                    ((RadButton)saveAction).Click += new EventHandler(delegate (object s, EventArgs args) { this.Save(formEdit, selected); });
                }

                formEdit.Owner = form;
                formEdit.ThemeName = ((RadForm)form).ThemeName;

                if (formEdit.ShowDialog() == DialogResult.OK)
                {
                    var entity = db.Set(Type.GetType(Mapping.Mapping.FullModelPath(classname))).Find(selected.GetType().GetProperty("ID").GetValue(selected, null));
                   
                    Mapping.Mapping.ViewModelClassMap(selected, ref entity);
                    db.Entry(entity).State = EntityState.Modified;
                    db.SaveChanges();

                    radGridView.Focus();
                };
            }
        }


   
        [DisplayVisible(IsVisible =true)]
        [DisplayName("Удалить")]
        public virtual void Delete(Form form, RadGridView radGridView, string classname)
        {
            //RadGridView radGridView = form.Controls.OfType<RadGridView>().FirstOrDefault();

            if (radGridView != null && radGridView.CurrentRow != null)
            {
                if (RadMessageBox.Show(form, "Вы уверены в том, что желаете удалить данную запись?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes)
                {
                    var selected = radGridView.SelectedRows[0].DataBoundItem;

                    var entity = db.Set(Type.GetType(Mapping.Mapping.FullModelPath(classname))).Find(selected.GetType().GetProperty("ID").GetValue(selected, null));

                    Mapping.Mapping.ViewModelClassMap(selected, ref entity);
                    db.Entry(entity).State = EntityState.Deleted;
                    db.SaveChanges();

                    ((BindingSource)radGridView.DataSource).Remove(selected);
                    radGridView.Focus();
                }
            }
        }

        /// <summary>
        /// Load form
        /// </summary>
        /// <param name="form"></param>
        /// <param name="classname"></param>
        public virtual void Load(Form form, string classname)
        {
            ThemeResolutionService.ApplyThemeToControlTree(form, ((RadForm)form).ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(((RadForm)form).ThemeName);

            RadGridView radGridView = form.Controls.OfType<RadGridView>().FirstOrDefault();
            //
            radGridView.Tag = classname + "Grid";

            RadCommandBar commandBar = form.Controls.OfType<RadCommandBar>().FirstOrDefault();

            commandBar.Tag = classname + "Actions";


            radGridView.CellDoubleClick += new GridViewCellEventHandler((s, args) => {

                this.Edit(form, radGridView, classname);
            });

            FullDataSource(form,classname);

            Mapping.Mapping.ActionsClassMap(form,this,classname);
        }


        protected void FullDataSource(Form form, string classname)
        {

            RadGridView radGridView=(RadGridView)Mapping.Mapping.GetControlByTag(form.Controls, classname+"Grid");
            BindingSource b = new BindingSource();
            b.DataSource = Mapping.Mapping.DataSourceMap(classname);
            radGridView.DataSource = null;
            radGridView.DataSource = b;

            Mapping.Mapping.GridViewClassMap(radGridView, classname);
        }

        /// <summary>
        /// Synchronization
        /// </summary>
        /// <param name="form"></param>
        /// <param name="classname"></param>
        
        [DisplayVisible(IsVisible =true, Separator = Separator.Left)]
        [DisplayName("Синхронизировать")]
        public virtual void Synchronization(Form form, RadGridView radGridView, string classname)
        {
            var path = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");
            if (File.Exists(path))
            {
                form.Cursor = Cursors.WaitCursor;

                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bool synchronized = false;
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(delegate (object sender, DoWorkEventArgs e) { this.synchronization(classname, path, form, ref synchronized); });
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(delegate (object sender, RunWorkerCompletedEventArgs e) { this.bw_RunWorkerCompleted(form, classname, ref synchronized); });

                bw.RunWorkerAsync();

            }
            else
            {
                RadMessageBox.Show(form, "Не найдена эталонная база данных! Синхронизация остановлена!", "Ошибка синхронизации");
            }
        }

        
        protected virtual void synchronization(string classname, string path, Form form, ref bool result)
        {
            result = UpdateDictionaries.updateTable(classname, path , ((RadForm)form).ThemeName, form);
        }

        private void bw_RunWorkerCompleted(Form form, string classname, ref bool synchronized)
        {
            form.Cursor = Cursors.Default;
            if (synchronized)
            {
                FullDataSource(form, classname);
                Messenger.showAlert(AlertType.Success, "Синхронизация завершена", "Данные успешно синхронизированы!", ((RadForm)form).ThemeName);
            }
        }


        
        [DisplayVisible(IsVisible =true,Separator =Separator.Left)]
        [DisplayName("Закрыть")]
        public virtual void Close(Form form, RadGridView radGridView, string classname) {

            form.Close();
        }
        
    }
}
