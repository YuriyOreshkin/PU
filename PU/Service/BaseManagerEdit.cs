using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using System.IO;

namespace PU
{
    public partial class BaseManagerEdit : Telerik.WinControls.UI.RadForm
    {
        public string action { get; set; }
        public DBEntry dbItem { get; set; }
        private string filePath = "";
        private string folderPath = "";

        public BaseManagerEdit()
        {
            InitializeComponent();
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

        private void BaseManagerEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            switch (action)
            { 
                case "add":
                    existedDBRadioBtn.Enabled = true;
                    newDBRadioBtn.Enabled = true;

                    int cnt = 0;
                    BaseManager main = this.Owner as BaseManager;
                    if (main != null)
                    {
                        cnt = main.dblist.Count;
                    }
                    nameTextBox.Text = "База данных №" + (cnt + 1).ToString();
                    break;
                case "edit":
                    nameTextBox.Text = dbItem.name;
                    pathBrowser.Value = dbItem.path;
                    break;
            }

            if (pathBrowser.DialogType == Telerik.WinControls.UI.BrowseEditorDialogType.OpenFileDialog)
            {
                OpenFileDialog dialog = (OpenFileDialog)pathBrowser.Dialog;
                dialog.Filter = "(*.db3)|*.db3";
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text != "")
            {
                if (pathBrowser.Value != null)
                {
                    if (existedDBRadioBtn.IsChecked) // путь к существующей бд
                    {
                        dbItem = new DBEntry { name = nameTextBox.Text, path = pathBrowser.Value.ToString(), pathBackup = Application.StartupPath + "\\Архивы БД\\" + nameTextBox.Text };
                        this.Close();
                    }
                    else if (newDBRadioBtn.IsChecked)
                    {
                        if (fileNameTextBox.Text != "")
                        {
                            string newPath = pathBrowser.Value.ToString() + "\\" + fileNameTextBox.Text + ".db3";
                            if (!File.Exists(newPath))
                            {
                                var EmptyDatabase = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");
                                if (File.Exists(EmptyDatabase))
                                {
                                    try
                                    {
                                        File.Copy(EmptyDatabase, newPath);
                                        var dbver = Methods.checkDBVersion(EmptyDatabase, true);
                                        Methods.setDBVersion(newPath, dbver.db_ver);
                                        dbItem = new DBEntry { name = nameTextBox.Text, path = newPath, pathBackup = Application.StartupPath + "\\Архивы БД\\" + nameTextBox.Text };
                                        this.Close();

                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show(ex.Message);
                                    }

                                }
                                else
                                {
                                    RadMessageBox.Show(this, "Не удалось найти файл эталонной базы данных!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                                }
                            }
                            else
                                RadMessageBox.Show(this, "В выбранном каталоге уже существует файл с названием \"" + fileNameTextBox.Text + ".db3\".\r\nВыберите другое имя файла!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                        }
                        else
                            RadMessageBox.Show(this, "Необходимо указать имя файла новой базы данных", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);

                    }

                }
                else
                {
                    RadMessageBox.Show(this, "Необходимо выбрать базу данных", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }
            else
            {
                RadMessageBox.Show(this, "Укажите название базы данных", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            dbItem = null;
            this.Close();
        }


        private void generateFileName()
        {
            if (pathBrowser.Value != null)
            {
                string fileName = "pu6";
                var t = pathBrowser.Value.ToString() + "\\" + fileName + ".db3";
                int i = 0;
                while (File.Exists(pathBrowser.Value.ToString() + "\\" + fileName + ".db3"))
                {
                    i++;
                    fileName = fileName + "_" + i.ToString();
                }
                fileNameTextBox.Text = fileName;
            }
                

        }

        private void pathBrowser_ValueChanged(object sender, EventArgs e)
        {
            if (newDBRadioBtn.IsChecked)
            {
                generateFileName();
            }

        }

        private void newDBRadioBtn_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (newDBRadioBtn.IsChecked)  // каталог куда создать новую базу
            {
                radLabel2.Text = "Каталог";
                pathBrowser.DialogType = Telerik.WinControls.UI.BrowseEditorDialogType.FolderBrowseDialog;
                filePath = pathBrowser.Value == null ? "" : pathBrowser.Value.ToString();
                pathBrowser.Value = folderPath;
                FolderBrowserDialog dialog = (FolderBrowserDialog)pathBrowser.Dialog;


                generateFileName();

                radLabel3.Visible = true;
                radLabel4.Visible = true;
                fileNameTextBox.Visible = true;


            }
        }

        private void existedDBRadioBtn_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            if (existedDBRadioBtn.IsChecked) // путь к существующей бд
            {
                radLabel2.Text = "Путь";
                pathBrowser.DialogType = Telerik.WinControls.UI.BrowseEditorDialogType.OpenFileDialog;
                folderPath = pathBrowser.Value == null ? "" : pathBrowser.Value.ToString();
                pathBrowser.Value = filePath;
                OpenFileDialog dialog = (OpenFileDialog)pathBrowser.Dialog;
                dialog.Filter = "(*.db3)|*.db3";

                radLabel3.Visible = false;
                radLabel4.Visible = false;
                fileNameTextBox.Visible = false;

            }
        }






    }
}
