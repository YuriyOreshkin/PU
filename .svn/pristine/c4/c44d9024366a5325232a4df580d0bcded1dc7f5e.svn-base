using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using System.Linq;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using Telerik.WinControls.UI;
using System.Reflection;

namespace PU.FormsODV1
{
    public partial class ODV_1_5_Edit : Telerik.WinControls.UI.RadForm
    {
        public FormsODV_1_5_2017 formData { get; set; }
        private bool setNull = true;
        public int cnt = 0;
        List<FormsODV_1_5_2017_OUT> FormsODV_1_5_OUT_List = new List<FormsODV_1_5_2017_OUT>();


        public ODV_1_5_Edit()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
        }

        private void FormsODV_1_5_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (setNull)
                formData = null;
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

        private void FormsODV_1_5_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            if (formData == null)
            {
                formData = new FormsODV_1_5_2017();
                Num.Value = cnt + 1;
            }
            else
            {
                Num.Value = formData.Num.HasValue ? (decimal)formData.Num.Value : cnt + 1;
                Department.Text = formData.Department;
                Profession.Text = formData.Profession;
                StaffCountShtat.Value = formData.StaffCountShtat.HasValue ? (decimal)formData.StaffCountShtat.Value : 0;
                StaffCountFakt.Value = formData.StaffCountFakt.HasValue ? (decimal)formData.StaffCountFakt.Value : 0;
                VidRabotFakt.Text = formData.VidRabotFakt;
                DocsName.Text = formData.DocsName;

                FormsODV_1_5_OUT_List = formData.FormsODV_1_5_2017_OUT.ToList();


                OUT_Grid_update();
            }
        }

        private void OUT_Grid_update()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            OUT_Grid.Rows.Clear();

            if (FormsODV_1_5_OUT_List.Count() != 0)
            {
                foreach (var item in FormsODV_1_5_OUT_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.OUT_Grid.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["OsobUslTrudaCode"].Value = item.OsobUslTrudaCode;
                    rowInfo.Cells["CodePosition"].Value = item.CodePosition;
                    OUT_Grid.Rows.Add(rowInfo);
                }
            }

            this.OUT_Grid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            OUT_Grid.Refresh();
        }

        private void DepartmentCleanBtn_Click(object sender, EventArgs e)
        {
            Department.Text = "";
        }

        private void ProfessionCleanBtn_Click(object sender, EventArgs e)
        {
            Profession.Text = "";
        }

        private void ProfessionBtn_Click(object sender, EventArgs e)
        {
            PU.FormsRSW2014.DolgnFrm child = new PU.FormsRSW2014.DolgnFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.btnSelection.Visible = true;
            child.ShowDialog();
            if (child.dolgn != null)
            {
                this.Profession.Text = child.dolgn.Name;
            }
            child = null;
        }

        private void DepartmentBtn_Click(object sender, EventArgs e)
        {
            DepartmentsFrm child = new DepartmentsFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.ShowDialog();
            if (child.DepID != 0)
            {
                Department.Text = child.Name;
            }
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            List<string> emptyFields = new List<string>();

            if (String.IsNullOrEmpty(Department.Text))
                emptyFields.Add("Структурное подразделение");
            if (String.IsNullOrEmpty(Profession.Text))
                emptyFields.Add("Профессия (должность)");
            if (String.IsNullOrEmpty(VidRabotFakt.Text))
                emptyFields.Add("Характер фактически выполняемых работ и доп. услочия труда");
            if (String.IsNullOrEmpty(DocsName.Text))
                emptyFields.Add("Наименование первичных документов, подтверждающих занятость в особых условиях");
            if (StaffCountShtat.Value == 0)
                emptyFields.Add("Кол-во раб. мест по штатному расп.");
            if (StaffCountFakt.Value == 0)
                emptyFields.Add("Кол-во фактически работающих");
            if (FormsODV_1_5_OUT_List.Count == 0)
                emptyFields.Add("Должна быть хотя бы одна запись льготных категорий");

            if (emptyFields.Count > 0)
            {
                string caption = "Следующие поля должны быть заполнены: \r\n";

                foreach (var s in emptyFields)
                {
                    caption += s + "\r\n";
                }

                caption += "\r\nВсе равно продолжить?";

                if ((DialogResult)RadMessageBox.Show(caption, "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }

            formData.Num = (short)Num.Value;
            formData.Department = Department.Text;
            formData.Profession = Profession.Text;
            formData.StaffCountShtat = (long)StaffCountShtat.Value;
            formData.StaffCountFakt = (long)StaffCountFakt.Value;
            formData.VidRabotFakt = VidRabotFakt.Text;
            formData.DocsName = DocsName.Text;


            if (formData.ID == null || formData.ID == 0)  // Если редактирование новой записи, которой еще нет в базе, просто переписываем коды
            {
                formData.FormsODV_1_5_2017_OUT.Clear();

                formData.FormsODV_1_5_2017_OUT = new System.Data.Objects.DataClasses.EntityCollection<FormsODV_1_5_2017_OUT>();
                var t = FormsODV_1_5_OUT_List.ToList();

                foreach (var item in t)
                {
                    formData.FormsODV_1_5_2017_OUT.Add(new FormsODV_1_5_2017_OUT { OsobUslTrudaCode = item.OsobUslTrudaCode, CodePosition = item.CodePosition });
                }
            }
            else // если редактирование уже записаной в базу записи 5 раздела одв-1
            {
                var tt = formData.FormsODV_1_5_2017_OUT.Where(x => x.ID == 0).ToList();

                foreach (var item in tt)
                {
                    formData.FormsODV_1_5_2017_OUT.Remove(item);
                }

                var FormsODV_1_5_2017_OUT_List_from_db = formData.FormsODV_1_5_2017_OUT.ToList();

                // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                var t = FormsODV_1_5_OUT_List.Select(x => x.ID);
                var list_for_del = FormsODV_1_5_2017_OUT_List_from_db.Where(x => !t.Contains(x.ID));

                foreach (var item in list_for_del)
                {
                    formData.FormsODV_1_5_2017_OUT.Remove(item);
                }

                if (list_for_del.Count() != 0)
                {
                    FormsODV_1_5_2017_OUT_List_from_db = formData.FormsODV_1_5_2017_OUT.Where(x => !list_for_del.Select(y => y.ID).Contains(x.ID)).ToList();
                }

                var fields = typeof(FormsODV_1_5_2017_OUT).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var names = Array.ConvertAll(fields, field => field.Name);
                
                var tr = FormsODV_1_5_OUT_List.ToList();
              //  tr.Reverse();

                foreach (var item in tr)
                {
               //     item.FormsODV_1_5_2017_ID = formData.ID;
                    FormsODV_1_5_2017_OUT r = new FormsODV_1_5_2017_OUT();
                    bool exist = false;

                    if (item.ID != 0)
                    {
                        r = FormsODV_1_5_2017_OUT_List_from_db.First(x => x.ID == item.ID);
                        exist = true;
                    }

                    foreach (var itemName_ in names)
                    {
                        string itemName = itemName_.TrimStart('_');
                        var properties = item.GetType().GetProperty(itemName);
                        if (properties != null)
                        {
                            object value = properties.GetValue(item, null);
                            var data = value;

                            r.GetType().GetProperty(itemName).SetValue(r, data, null);
                        }

                    }

                    if (!exist)
                    {
                        formData.FormsODV_1_5_2017_OUT.Add(r);
                    }

                }



            }


            setNull = false;
            this.Close();
        }

        private void OUT_delBtn_Click(object sender, EventArgs e)
        {
            if (OUT_Grid.RowCount != 0)
            {
                int rowindex = OUT_Grid.CurrentRow.Index;
                FormsODV_1_5_OUT_List.RemoveAt(rowindex);

                OUT_Grid_update();

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void OUT_editBtn_Click(object sender, EventArgs e)
        {
            if (OUT_Grid.RowCount != 0)
            {
                int rowindex = OUT_Grid.CurrentRow.Index;

                ODV_1_5_OUT_Edit child = new ODV_1_5_OUT_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.formData = FormsODV_1_5_OUT_List.Skip(rowindex).Take(1).First();
                child.ShowDialog();

                if (child.formData != null)
                {
                    FormsODV_1_5_OUT_List.RemoveAt(rowindex);
                    FormsODV_1_5_OUT_List.Insert(rowindex, child.formData);

                    OUT_Grid_update();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void OUT_addBtn_Click(object sender, EventArgs e)
        {
            ODV_1_5_OUT_Edit child = new ODV_1_5_OUT_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
            if (child.formData != null)
            {
                //FormsODV_1_5_OUT_List.Insert(0,child.formData);
                FormsODV_1_5_OUT_List.Add(child.formData);
                OUT_Grid_update();
            }
        }

    }
}
