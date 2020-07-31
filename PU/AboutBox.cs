using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using Telerik.WinControls;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PU
{
    partial class AboutBox : Telerik.WinControls.UI.RadForm
    {
        public AboutBox()
        {
            InitializeComponent();

            //  Initialize the AboutBox to display the product information from the assembly information.
            //  Change assembly information settings for your application through either:
            //  - Project->Properties->Application->Assembly Information
            //  - AssemblyInfo.cs
            this.Text = String.Format("О программе {0}", AssemblyTitle);
            this.radLabelProductName.Text = AssemblyProduct;
            this.radLabelVersion.Text = String.Format("Версия {0}", AssemblyVersion);
            this.radLabelCopyright.Text = AssemblyCopyright;
            this.radLabelCompanyName.Text = AssemblyCompany;
            string email_main = "pu6developer@gmail.com"; //pass: pu6pfr007
            string opfr_rk_site = "http://www.pfrf.ru";
            this.emailTextBox.Text = email_main;
            this.rk_siteTextBox.Text = opfr_rk_site;
            this.radTextBoxDescription.Text = String.Format("{0}", AssemblyDescription);
        }


        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                // Get all Title attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                // If there is at least one Title attribute
                if (attributes.Length > 0)
                {
                    // Select the first one
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    // If it is not an empty string, return it
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                // If there was no Title attribute, or if the Title attribute was the empty string, return the .exe name
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                // Get all Description attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                // If there aren't any Description attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Description attribute, return its value
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                // Get all Product attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                // If there aren't any Product attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Product attribute, return its value
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                // Get all Copyright attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                // If there aren't any Copyright attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Copyright attribute, return its value
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                // Get all Company attributes on this assembly
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                // If there aren't any Company attributes, return an empty string
                if (attributes.Length == 0)
                    return "";
                // If there is a Company attribute, return its value
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void AboutBox_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            radButton2.ButtonElement.ToolTipText = "Будет открыта страница в браузере";

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

        public void radButton1_Click(object sender, EventArgs e)
        {
            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = this.ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(760, 560);
            child.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            child.ShowInTaskbar = false;
            child.Text = "История изменений программы Документы ПУ-6";
            string history = string.Empty;
            if (File.Exists(Path.Combine(Application.StartupPath, "history.txt")))
            {
                history = File.ReadAllText(Path.Combine(Application.StartupPath, "history.txt"), System.Text.Encoding.UTF8);
            }
            else
                history = "Файл \"history.txt\" не найден в каталоге с программой.";

            if (String.IsNullOrEmpty(history))
            {
                history = "Файл \"history.txt\" найден, он пустой.";
            }

            Telerik.WinControls.UI.RadTextBox textBox = new Telerik.WinControls.UI.RadTextBox();
            textBox.Name = "historyTextBox";
            textBox.Dock = DockStyle.Fill;
            textBox.Multiline = true;
            textBox.AutoSize = false;
            textBox.EnableTheming = true;
            textBox.ThemeName = this.ThemeName;
            textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBox.BackColor = Color.GhostWhite;
            textBox.HideSelection = true;
            textBox.Font = new Font("Times New Roman", 12);
            textBox.ForeColor = Color.SteelBlue;
            textBox.Text = history;
            textBox.SelectionStart = history.Length;
            textBox.ReadOnly = true;
            child.Controls.Add(textBox);
            ((Telerik.WinControls.UI.RadTextBoxElement)((child.Controls["historyTextBox"] as Telerik.WinControls.UI.RadTextBox).GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(58)))), ((int)(((byte)(120)))));
            child.ShowDialog();


        }

        private class WebClient : System.Net.WebClient
        {
            public int Timeout { get; set; }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                WebRequest lWebRequest = base.GetWebRequest(uri);
                lWebRequest.Timeout = Timeout;
                ((HttpWebRequest)lWebRequest).ReadWriteTimeout = Timeout;
                return lWebRequest;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            try
            {
                WebClient client = new WebClient();
                client.Timeout = 1000;
                string s = "";
                s = client.DownloadString("http://www.pfrf.ru/files/branches/komi/Soft/DocPU5/pu6version.txt");

                if (!String.IsNullOrEmpty(s) && s.Contains("|"))
                {
                    var str = s.Split('|');
                    string url = str[1];

                    Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                    Version latestVersion = new Version(str[0]);
                    if (latestVersion > currentVersion)
                    {
                        if (RadMessageBox.Show(
                                "Доступна новая версия Документы ПУ-6 (" + str[0] + "). Открыть страницу для скачивания программы?", "Проверка новой версии", MessageBoxButtons.YesNo, RadMessageIcon.Info
                            ) == DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(url);
                        }
                        // Доступна новая версия
                    }
                    else
                    {
                        RadMessageBox.Show(this, "Более новая версия программы не обнаружена!", "Проверка новой версии", MessageBoxButtons.OK, RadMessageIcon.Info);
                    }
                }
                else
                {
                    checkErrorMessage();
                }
            }
            catch
            {
                checkErrorMessage();
            }






            //try
            //{
            //    var request = HttpWebRequest.Create("http://www.pfrf.ru/files/branches/komi/Soft/DocPU5/pu6version.txt");
            //    request.Timeout = 1000;
            //    using (var response = request.GetResponse())
            //    {
            //        string s = response.ToString();

            //        if (!String.IsNullOrEmpty(s) && s.Contains("|"))
            //        {
            //            var str = s.Split('|');

            //            string url = str[1];

            //            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
            //            Version latestVersion = new Version(str[0]);
            //            if (latestVersion > currentVersion)
            //            {
            //                if (RadMessageBox.Show(
            //                        "Доступна новая версия Документы ПУ-6 (" + str[0] + "). Открыть страницу для скачивания программы?", "Проверка новой версии", MessageBoxButtons.YesNo, RadMessageIcon.Info
            //                    ) == DialogResult.Yes)
            //                {
            //                    System.Diagnostics.Process.Start(url);
            //                }
            //                // Доступна новая версия
            //            }
            //            else
            //            {
            //                RadMessageBox.Show(this, "Более новая версия программы не обнаружена!", "Проверка новой версии", MessageBoxButtons.OK, RadMessageIcon.Info);
            //            }
            //        }
            //        else
            //        {
            //            checkErrorMessage();
            //        }
            //    }

            //}
            //catch
            //{
            //    checkErrorMessage();
            //}












            //          checkVersionTest();



        }


        private void checkVersionTest()
        {
            var client = new WebClient();
            client.Timeout = 1000;

            try
            {
                client.DownloadStringCompleted += (sender, e) =>
                {
                    try
                    {
                        string s = e.Result;

                        if (!String.IsNullOrEmpty(s) && s.Contains("|"))
                        {
                            var str = s.Split('|');

                            string url = str[1];

                            Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                            Version latestVersion = new Version(str[0]);
                            if (latestVersion > currentVersion)
                            {
                                if (RadMessageBox.Show(
                                        "Доступна новая версия Документы ПУ-6 (" + str[0] + "). Открыть страницу для скачивания программы?", "Проверка новой версии", MessageBoxButtons.YesNo, RadMessageIcon.Info
                                    ) == DialogResult.Yes)
                                {
                                    System.Diagnostics.Process.Start(url);
                                }
                                // Доступна новая версия
                            }
                            else
                            {
                                RadMessageBox.Show(this, "Более новая версия программы не обнаружена!", "Проверка новой версии", MessageBoxButtons.OK, RadMessageIcon.Info);
                            }
                        }
                        else
                        {
                            checkErrorMessage();
                        }
                    }
                    catch
                    {
                        checkErrorMessage();
                    }
                };

                client.DownloadStringAsync(new Uri("http://www.pfrf.ru/files/branches/komi/Soft/DocPU5/pu6version.txt"));
            }
            catch
            {
                checkErrorMessage();
            }
        }

        private void checkErrorMessage()
        {
            DialogResult dialogResult = RadMessageBox.Show(this, "При проверке новой версии программы произошла ошибка!\r\nВозможно отсутствует доступ в Интернет!\r\nОткрыть страницу для загрузки Документы ПУ-6 на сайте ПФР?", "Проверка новой версии", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);

            if (dialogResult == DialogResult.Yes)
            {
                Process.Start("http://www.pfrf.ru/branches/komi/info/~rabotodatelam/2014");
            }
            else
            {
                return;
            }
        }
    }
}
