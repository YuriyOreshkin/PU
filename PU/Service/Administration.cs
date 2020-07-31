using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using PU.UserAccess;
using System.Linq;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using PU.Models;
using PU.FormsRSW2014;
using System.IO;

namespace PU
{
    public partial class Administration : Telerik.WinControls.UI.RadForm
    {
        xaccessEntities xaccessDB = new xaccessEntities();
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
        Props props = new Props(); //экземпляр класса с настройками
//        bool prevState_useRSW1_2015;

        public Administration()
        {
            InitializeComponent();
        }

        public void UsersRadGridView_upd()
        {

            this.UsersRadGridView.TableElement.BeginUpdate();
            UsersRadGridView.Rows.Clear();

            var userList = xaccessDB.Users.Where(x => !x.Blocked.HasValue || (x.Blocked.HasValue && x.Blocked.Value == 0));

            if (userList.Count() != 0)
            {
                foreach (var item in userList)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.UsersRadGridView.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Name"].Value = item.Name;
                    rowInfo.Cells["Login"].Value = item.Login;
                    rowInfo.Cells["Role"].Value = item.Roles != null ? item.Roles.Name : "";
                    rowInfo.Cells["LastAccessDate"].Value = item.LastAccessDate.HasValue ? item.LastAccessDate.Value.ToString() : "";
                    UsersRadGridView.Rows.Add(rowInfo);
                }
            }

            this.UsersRadGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.UsersRadGridView.TableElement.EndUpdate();
            UsersRadGridView.Refresh();

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

        private void Administration_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            props.ReadXml();

            if (themeSelectorDDL.Items.Any(x => x.Text == Options.ThemeName))
            {
                themeSelectorDDL.Items.FirstOrDefault(x => x.Text == Options.ThemeName).Selected = true;
            }
            else
            {
                themeSelectorDDL.Items.Add(new RadListDataItem { Text = Options.ThemeName, Tag = Options.ThemeName });
                themeSelectorDDL.Items.FirstOrDefault(x => x.Text == Options.ThemeName).Selected = true;
            }

            fixCurrentInsurerCheckBox.Checked = Options.fixCurrentInsurer;
            getInsurerFIOtoNewFormCheckBox.Checked = Options.getInsurerFIOtoNewForm;
            saveLastPackNumCheckBox.Checked = Options.saveLastPackNum;
            printAllPagesRSV1CheckBox.Checked = Options.printAllPagesRSV1;
            autoCheckNewVersionCheckBox.Checked = props.Fields.autoCheckNewVersion;
            dbJournal_modeWALcheckBox.Checked = props.Fields.dbJournal_modeWAL;
            hideDialogCheckFilesCheckBox.Checked = Options.hideDialogCheckFiles;
            checkFilesAfterSavingCheckBox.Checked = Options.checkFilesAfterSaving;
            checkPfrBrowseEditor.Value = props.Fields.pathCheckPfr;


            UsersRadGridView_upd();
            this.themeSelectorDDL.SelectedIndexChanged += (s, с) => themeSelectorDDL_SelectedIndexChanged();

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Options.fixCurrentInsurer = fixCurrentInsurerCheckBox.Checked;
            Options.saveLastPackNum = saveLastPackNumCheckBox.Checked;
            Options.printAllPagesRSV1 = printAllPagesRSV1CheckBox.Checked;
            Options.getInsurerFIOtoNewForm = getInsurerFIOtoNewFormCheckBox.Checked;
            Options.hideDialogCheckFiles = hideDialogCheckFilesCheckBox.Checked;
            Options.checkFilesAfterSaving = checkFilesAfterSavingCheckBox.Checked;
            Options.pathCheckPfr = checkPfrBrowseEditor.Value;

            props.Fields.fixCurrentInsurer = fixCurrentInsurerCheckBox.Checked;
            props.Fields.getInsurerFIOtoNewForm = getInsurerFIOtoNewFormCheckBox.Checked;
            props.Fields.saveLastPackNum = saveLastPackNumCheckBox.Checked;
            props.Fields.printAllPagesRSV1 = printAllPagesRSV1CheckBox.Checked;
            props.Fields.autoCheckNewVersion = autoCheckNewVersionCheckBox.Checked;
            props.Fields.hideDialogCheckFiles = hideDialogCheckFilesCheckBox.Checked;
            props.Fields.checkFilesAfterSaving = checkFilesAfterSavingCheckBox.Checked;
            props.Fields.pathCheckPfr = checkPfrBrowseEditor.Value;


            if (props.Fields.dbJournal_modeWAL != dbJournal_modeWALcheckBox.Checked)
            {
                Methods.setDBjurnal_mode(dbJournal_modeWALcheckBox.Checked, this.ThemeName, this);
            }
            props.Fields.dbJournal_modeWAL = dbJournal_modeWALcheckBox.Checked;
            props.WriteXml();

            //если изменили положение переключателя
/*            if (prevState_useRSW1_2015 != Options.useRSW1_2015)
            { 
                MainForm main = this.Owner as MainForm;
                if (main != null)
                {
                    foreach (Form form in main.MdiChildren)
                    {
                        if (form.Name == "RSW2014_List")
                        {
                            RSW2014_List rswListForm = form as RSW2014_List;
                            rswListForm.yearType = Options.useRSW1_2015 ? (short)2014 : (short)2012;
                            rswListForm.rswAddBtn2015.Enabled = Options.useRSW1_2015;

                        }
                    }
                }
            }
            */
            this.Close();
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (UsersRadGridView.RowCount > 0)
            {
                if (UsersRadGridView.RowCount == 1)
                {
                    Methods.showAlert("Ошибка", "Нельзя удалить единственного пользователя!", this.ThemeName);
                }
                else
                {
                    long id = Convert.ToInt64(UsersRadGridView.CurrentRow.Cells[0].Value);
                    if (id == Options.User.ID)
                    {
                        Methods.showAlert("Ошибка", "Нельзя удалить пользователя под которым выполнен вход!", this.ThemeName);
                    }
                    else
                    { 
                        if (xaccessDB.Users.Any(x => x.ID == id))
                        {
                            var u = xaccessDB.Users.FirstOrDefault(x => x.ID == id);
                            if (!u.SysAdmin.HasValue || u.SysAdmin.Value != 1)
                            {
                                xaccessDB.Database.ExecuteSqlCommand("DELETE FROM Users WHERE ID = " + id.ToString());
                                UsersRadGridView_upd();
                            }
                        }

                    }

                }
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            AdministrationEdit child = new AdministrationEdit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.ShowDialog();
            if (child.formData != null)
            {
                xaccessDB = new xaccessEntities();
                UsersRadGridView_upd();
            }
        }


        private void editBtn_Click(object sender, EventArgs e)
        {
            if (UsersRadGridView.RowCount > 0 && UsersRadGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(UsersRadGridView.CurrentRow.Cells[0].Value.ToString());
                UserAccess.Users formData = xaccessDB.Users.FirstOrDefault(x => x.ID == id);

                AdministrationEdit child = new AdministrationEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.formData = formData;
                child.action = "edit";
                child.ShowInTaskbar = false;
                child.ShowDialog();
                if (child.formData != null)
                {
                    xaccessDB = new xaccessEntities();
                    UsersRadGridView_upd();
                }
            }
        }

        private void UsersRadGridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_Click(null,null);
        }

        private void themeSelectorDDL_SelectedIndexChanged()
        {
            string themeName = themeSelectorDDL.Text;
            ChangeThemeName(this, themeName);

            MainForm main = this.Owner as MainForm;

            if (main != null)
            {
                //ChangeThemeName(main, themeName);
                ThemeResolutionService.ApplyThemeToControlTree(main, this.ThemeName);

                foreach (Form form in this.MdiChildren)
                {
                    ThemeResolutionService.ApplyThemeToControlTree(form, this.ThemeName);
                    //ChangeThemeName(form, themeName);
                }

                Options.ThemeName = themeName;
                methodsNonStatic.writeSetting();
            }
        }

        public void ChangeThemeName(Control control, string themeName)
        {

            IComponentTreeHandler radControl = control as IComponentTreeHandler;
            if (radControl != null)
            {
                radControl.ThemeName = themeName;
            }
            foreach (Control child in control.Controls)
            {
                ChangeThemeName(child, themeName);
            }
        }

        private void checkPfrBrowseEditor_ValueChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(checkPfrBrowseEditor.Value))
            {
                checkUfaExistLabel.Text = "Не выбран каталог с программой проверки CheckPfr";
                checkUfaExistLabel.ForeColor = Color.Firebrick;
            }
            else if (File.Exists(Path.Combine(checkPfrBrowseEditor.Value, "CheckUfa.dll")))
            {
                checkUfaExistLabel.Text = "Файл CheckUfa.dll в каталоге программы проверки CheckPfr ОБНАРУЖЕН!";
                checkUfaExistLabel.ForeColor = Color.LimeGreen;
            }
            else
            {
                checkUfaExistLabel.Text = "Файл CheckUfa.dll в каталоге программы проверки CheckPfr НЕ ОБНАРУЖЕН или нет доступа!";
                checkUfaExistLabel.ForeColor = Color.Firebrick;
            }
        }

        private void ObjectsFormsBtn_Click(object sender, EventArgs e)
        {
            PU.Service.ObjectsToForms child = new PU.Service.ObjectsToForms();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowDialog();
        }

        private void AccessLevelBtn_Click(object sender, EventArgs e)
        {
            if (UsersRadGridView.RowCount > 0 && UsersRadGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(UsersRadGridView.CurrentRow.Cells[0].Value.ToString());
                UserAccess.Users formData = xaccessDB.Users.FirstOrDefault(x => x.ID == id);

                PU.Service.UserToObjects child = new PU.Service.UserToObjects();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.Text = child.Text + " [" + formData.Name + "]";
                child.userID = id;
                child.ShowDialog();
            }
        }



    }
}
