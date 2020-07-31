using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using PU.UserAccess;

namespace PU
{
    public partial class AdministrationEdit : Telerik.WinControls.UI.RadForm
    {
        xaccessEntities xacccssDB = new xaccessEntities();
        public UserAccess.Users formData { get; set; }
        public string action { get; set; }

        public AdministrationEdit()
        {
            InitializeComponent();
        }

        private void AdministrationEdit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            switch (action)
            { 
                case "add":
                    break;
                case "edit":
                    nameTextBox.Text = formData.Name;
                    loginTextBox.Text = formData.Login;
                    passwordTextBox.Text = formData.Password;
                    roleDDL.Items.FirstOrDefault(x => x.Tag.ToString() == formData.RoleID.Value.ToString()).Selected = true;
                    break;
            }
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
            formData = null;
            this.Close();
        }

        private void AdministrationEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            xacccssDB = null;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(loginTextBox.Text))
            {
                RadMessageBox.Show(this, "Поле Логин обязательно к заполнению!", "Внимание");
                loginTextBox.Select();
                return;
            }
            else
            {
                byte roleid = 3;

                byte.TryParse(roleDDL.SelectedItem.Tag.ToString(), out roleid);

                switch (action)
                { 
                    case "add":
                        if (xacccssDB.Users.Any(x => x.Login == loginTextBox.Text))
                        {
                            RadMessageBox.Show(this, "Пользователь с таким Логином уже существует!", "Внимание");
                            loginTextBox.Select();
                            return;
                        }
                        formData = new UserAccess.Users{
                        Name = nameTextBox.Text,
                        Login = loginTextBox.Text,
                        Password = passwordTextBox.Text,
                        RoleID = roleid,
                        DateCreate = DateTime.Now
                        };
                        xacccssDB.AddToUsers(formData);

                        break;
                    case "edit":
                        if (xacccssDB.Users.Any(x => x.Login == loginTextBox.Text && x.ID != formData.ID))
                        {
                            RadMessageBox.Show(this, "Пользователь с таким Логином уже существует!", "Внимание");
                            loginTextBox.Select();
                            return;
                        }

                        UserAccess.Users user = xacccssDB.Users.FirstOrDefault(x => x.ID == formData.ID);
                        user.Name = nameTextBox.Text;
                        user.Login = loginTextBox.Text;
                        user.Password = passwordTextBox.Text;
                        if (!formData.SysAdmin.HasValue || formData.SysAdmin.Value != 1)
                        {
                            user.RoleID = roleid;
                        }
                        xacccssDB.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);

                        break;
                }
                xacccssDB.SaveChanges();

                this.Close();

            }
        }
    }
}
