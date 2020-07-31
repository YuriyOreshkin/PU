using PU.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU.Service.CheckFiles
{
    public partial class CheckFilesList : Telerik.WinControls.UI.RadForm
    {
        public List<string> FileInfoList = new List<string>();// Список сохраненных файлов

        public CheckFilesList()
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

        private void propertiesBtn_Click(object sender, EventArgs e)
        {
            Administration child = new Administration();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.radPageView1.SelectedPage = child.radPageView1.Pages[2];
            child.ShowDialog();
        }

        private void checkFileBtn_Click(object sender, EventArgs e)
        {
            if (filesGridView.RowCount > 0 && filesGridView.CurrentRow.Cells["Path"].Value != null)
            {
                string file = filesGridView.CurrentRow.Cells["Path"].Value.ToString();

                if (File.Exists(Path.Combine(Options.pathCheckPfr, "CheckUfa.dll")))
                {
                    if (File.Exists(file))
                    {
                        IntPtr s1 = (IntPtr)Marshal.StringToHGlobalUni(file); // fileTest
                        IntPtr s2 = (IntPtr)Marshal.StringToHGlobalUni("");
                        IntPtr s3 = (IntPtr)Marshal.StringToHGlobalUni("HTML");

                        try
                        {

                            string PathOLD = Environment.GetEnvironmentVariable("PATH");
                            if (!PathOLD.Contains(Options.pathCheckPfr))  // Если такого пути нет в переменных, то добавляем его
                                Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + Options.pathCheckPfr);

                            string res;
                            Delphi.CheckFile3(out res, s1, s2, s3, 0);

                            if (res.Contains("html") && res.Contains("PROTOCOL") && res.Contains("log"))
                            {
                                DialogResult dialogResult = RadMessageBox.Show(this, "Открыть протокол?", "Результат", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                                if (dialogResult == DialogResult.Yes)
                                {
                                    try
                                    {
                                        Process.Start(res);
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("Не удалось открыть протокол проверки! Код ошибки - " + ex.Message);
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show(this, "При попытке проверки файла произошла ошибка. Код ошибки - " + ex.Message, "Ошибка!");
                        }
                    }
                    else
                    {
                        RadMessageBox.Show("Выбранный файл не найден на диске!");
                    }
                }
                else
                {
                    RadMessageBox.Show("Не найден модуль проверки CheckUfa.dll программы CheckPfr! Путь до программы указан - " + Options.pathCheckPfr);
                }



            }
        }

        public static class Delphi
        {
            [DllImport("CheckUfa.dll")] // В кавычках полный адрес DLL
            public static extern void CheckFile3([MarshalAs(UnmanagedType.BStr)] out string result, IntPtr fname, IntPtr reportName, IntPtr typRepr, int fox);


        }

        private void CheckFilesList_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            foreach (var file in FileInfoList)
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.filesGridView.MasterView);
                rowInfo.Cells["Name"].Value = Path.GetFileName(file);
                rowInfo.Cells["Size"].Value = (new FileInfo(file).Length / 1024).ToString() + " Кб";
                rowInfo.Cells["Path"].Value = file;
                filesGridView.Rows.Add(rowInfo);
            }

            if (FileInfoList.Count > 0)
            {
                folderPathTextBox.Text = Path.GetDirectoryName(FileInfoList[0]);
                filesGridView.Rows[0].IsCurrent = true;
            }

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openFileBtn_Click(object sender, EventArgs e)
        {
            if (filesGridView.RowCount > 0 && filesGridView.CurrentRow.Cells["Path"].Value != null)
            {
                try
                {
                    Process.Start(filesGridView.CurrentRow.Cells["Path"].Value.ToString());
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("Не удалось открыть протокол проверки! Код ошибки - " + ex.Message);
                }
            }
        }

        private void openFolderBtn_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(folderPathTextBox.Text))
            {
                if (filesGridView.RowCount > 0)
                {
                    filesGridView.Rows[0].IsCurrent = true;
                    if (filesGridView.CurrentRow.Cells["Path"].Value != null)
                    {
                        Process.Start("explorer", @"/select, " + filesGridView.CurrentRow.Cells["Path"].Value.ToString());
                    }
                }
            }
        }

        private void viewFileBtn_Click(object sender, EventArgs e)
        {
            XmlViewer child = new XmlViewer();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.xmlPath = filesGridView.CurrentRow.Cells["Path"].Value.ToString();
            child.ShowDialog();
        }

    }
}
