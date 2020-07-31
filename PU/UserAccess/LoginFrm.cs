using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using PU.UserAccess;
using PU.Models;
using PU.Classes;

namespace PU
{
    public partial class LoginFrm : Telerik.WinControls.UI.RadForm
    {
        xaccessEntities xaccessdb = new xaccessEntities();

        bool closeFlag = false;
        public string lastName = string.Empty;

        public LoginFrm()
        {
            InitializeComponent();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            closeFlag = true;
            MainForm main = this.Owner as MainForm;
            if (main != null)
            {
                main.loginFormExit = true;
            }

            Application.Exit();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (checkAccess())
            {
                loginErrorLabel.Visible = false;
                closeFlag = true;
                this.Close();
            }
            else
            {
                loginErrorLabel.Visible = true;
            }


        }

        private bool checkAccess()
        {
            string login = loginTextBox.Text;
            string pass = passwordTextBox.Text;

            bool result = xaccessdb.Users.Any(x => x.Login == login && x.Password == pass && (!x.Blocked.HasValue || (x.Blocked.HasValue && x.Blocked.Value == 1)));
            if (result)
            {
                Options.User = xaccessdb.Users.FirstOrDefault(x => x.Login == login && x.Password == pass && (!x.Blocked.HasValue || (x.Blocked.HasValue && x.Blocked.Value == 1)));
                //Methods.checkAndSetActiveUser(user);

            }

            return result;
        }

        private void LoginFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!closeFlag)
            {
                e.Cancel = true; 
            }
        }

        private void LoginFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            //если имя последнего доступа из настроек не пустое ставим галку и заполняем имя
            if (!String.IsNullOrEmpty(lastName) && lastName != "null")
            {
                loginTextBox.Text = lastName;
                rememberNameCheckBox.Checked = true;
            }
            passwordTextBox.Select();
        }

        private void passwordTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                radButton2_Click(null, null);
            }
        }

        private void loginTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                radButton2_Click(null, null);
            }
        }
    }
}
