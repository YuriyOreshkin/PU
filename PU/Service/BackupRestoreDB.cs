using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.IO;
using System.Linq;
using PU.Models;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using Telerik.WinControls.UI;
using System.Reflection;
using Ionic.Zip;

namespace PU
{
    public partial class BackupRestoreDB : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities { };
        Props props = new Props(); //экземпляр класса с настройками
        DBEntry currentDB = new DBEntry { };
        public bool restoreFlag = false;

        public BackupRestoreDB()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {

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

        private void BackupRestoreDB_Load(object sender, EventArgs e)
        {
            props.ReadXml();
            var dblist = props.Fields.DBList;
            currentDB = dblist.First(x => x.actual);

            this.Text = this.Text + "  -  " + currentDB.name;

            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            backup item = props.Fields.Backup;

            autoBackupCheckBox.Checked = item.autoBackup.active;
            autoBackupSpinEditor.Value = item.autoBackup.value;

            maxCountCheckBox.Checked = item.maxCount.active;
            maxCountSpinEditor.Value = item.maxCount.value;

            if (!String.IsNullOrEmpty(currentDB.pathBackup))
            {
                pathBrowser.Value = currentDB.pathBackup;
            }
            else if (!String.IsNullOrEmpty(item.pathLast))
            {
                pathBrowser.Value = item.pathLast;
            }
            else
            {
                pathBrowser.Value = Application.StartupPath + "\\Архивы БД\\" + currentDB.name;
            }


            backupDBGridView_upd();

        }

        private void setSettings()
        {

            backup item = new backup
            {
                autoBackup = new backupSettings { active = autoBackupCheckBox.Checked, value = autoBackupSpinEditor.Value },
                maxCount = new backupSettings { active = maxCountCheckBox.Checked, value = maxCountSpinEditor.Value },
                pathLast = pathBrowser.Value
            };

            props.Fields.Backup = item;
            props.WriteXml();

        }

        public void backupDBGridView_upd()
        {

            this.backupDBGridView.TableElement.BeginUpdate();
            backupDBGridView.Rows.Clear();

            var backupDBList = db.BackupDB_Info;

            int i = 1;
            foreach (var item in backupDBList)
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.backupDBGridView.MasterView);
                rowInfo.Cells["ID"].Value = item.ID;
                rowInfo.Cells["Number"].Value = i.ToString();
                rowInfo.Cells["DateCreate"].Value = item.DateCreate.HasValue ? item.DateCreate.Value.ToString("dd.MM.yyyy H:mm:ss") : "";
                rowInfo.Cells["User"].Value = item.Users.Name;
                rowInfo.Cells["Path"].Value = item.Path;
                rowInfo.Cells["Description"].Value = item.Description;

                if (!File.Exists(item.Path))
                {
                    for (int n = 0; n < rowInfo.Cells.Count; n++)
                    {
                        rowInfo.Cells[n].Style.CustomizeFill = true;
                        rowInfo.Cells[n].Style.DrawFill = true;
                        rowInfo.Cells[n].Style.BackColor = Color.FromArgb(255, 170, 170);
                    }
                    rowInfo.Cells["notExist"].Value = "1";
                }
                backupDBGridView.Rows.Add(rowInfo);
                i++;
            }


            this.backupDBGridView.TableElement.EndUpdate();
            backupDBGridView.Refresh();

        }

        private void delNotExistedBtn_Click(object sender, EventArgs e)
        {
            List<string> sList = new List<string>();
            foreach (var row in backupDBGridView.Rows.Where(x => x.Cells["notExist"].Value != null && x.Cells["notExist"].Value.ToString() == "1"))
            {
                sList.Add(row.Cells["ID"].Value.ToString());
            }

            delRecordFromDB(string.Join(", ", sList));
        }

        private void autoBackupCheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            setSettings();
        }

        private void maxCountCheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            setSettings();
        }

        private void autoBackupSpinEditor_ValueChanged(object sender, EventArgs e)
        {
            if (autoBackupCheckBox.Checked)
                setSettings();
        }

        private void maxCountSpinEditor_ValueChanged(object sender, EventArgs e)
        {
            if (maxCountCheckBox.Checked)
                setSettings();
        }

        private void pathBrowser_ValueChanged(object sender, EventArgs e)
        {
            setSettings();
        }

        private void delFromListBtn_Click(object sender, EventArgs e)
        {
            delRecordFromDB(backupDBGridView.CurrentRow.Cells["ID"].Value.ToString());
        }

        private void delRecordFromDB(string idlist)
        {
            string query = "DELETE FROM BackupDB_Info WHERE ID in (" + idlist + ")";
            db.Database.ExecuteSqlCommand(query);

            backupDBGridView_upd();
        }

        private void delFromDiskBtn_Click(object sender, EventArgs e)
        {
            if ((backupDBGridView.CurrentRow.Cells["notExist"].Value == null || (backupDBGridView.CurrentRow.Cells["notExist"].Value != null && backupDBGridView.CurrentRow.Cells["notExist"].Value.ToString() != "1")) && (!String.IsNullOrEmpty(backupDBGridView.CurrentRow.Cells["Path"].Value.ToString())))
            {
                if (RadMessageBox.Show(
                        "Вы уверены в том, что желаете удалить выбранный архив базы данных?\r\nФайл будет удален с диска навсегда!", "Удаление архива базы данных с диска!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation
                    ) == DialogResult.Yes)
                {
                    try
                    {
                        File.Delete(backupDBGridView.CurrentRow.Cells["Path"].Value.ToString());
                        delFromListBtn_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "Ошибка при удалении файла архива базы данных с диска.\r\n" + ex.Message, "Внимание", MessageBoxButtons.OK, RadMessageIcon.Error);
                        return;
                    }
                }
            }
            else
            { 
                delFromListBtn_Click(null, null);
            }
        }

        private void createBtn_Click(object sender, EventArgs e)
        {

            if (String.IsNullOrEmpty(pathBrowser.Value) || (!String.IsNullOrEmpty(pathBrowser.Value) && !Directory.Exists(pathBrowser.Value))) // Если указанный каталог не существует или значение пустое
            {


                string path = Application.StartupPath + "\\Архивы БД\\" + currentDB.name;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                pathBrowser.Value = path;
            }

            FileInfo fi = new FileInfo(currentDB.path);
            string newFileName = fi.Name + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".zip";


            try
            {
                //          File.Copy(currentDB.path, Path.Combine(pathBrowser.Value, newFileName));

                DateTime dt = DateTime.Now;

                using (ZipFile zip = new ZipFile(Encoding.UTF8))
                {
                    zip.AddFile(currentDB.path, "");
                    zip.Comment = currentDB.name + ". Архив Базы данных программы " + AssemblyTitle + String.Format(" Версия: {0}", AssemblyVersion) + " от " + dt.ToString("dd.MM.yyyy HH:mm");
                    zip.Save(Path.Combine(pathBrowser.Value, newFileName));
                }
                BackupDB_Info backup_new = new BackupDB_Info
                {
                    DateCreate = dt,
                    UserID = Options.User.ID,
                    Path = Path.Combine(pathBrowser.Value, newFileName),
                    Description = currentDB.name + ". Архив Базы данных программы " + AssemblyTitle + String.Format(" Версия: {0}", AssemblyVersion) + " от " + dt.ToString("dd.MM.yyyy HH:mm")
                };

                db.BackupDB_Info.Add(backup_new);
                db.SaveChanges();

                var dblist = props.Fields.DBList;
                var ind = dblist.IndexOf(dblist.First(x => x.actual));
                currentDB.pathBackup = pathBrowser.Value;
                dblist.RemoveAt(ind);
                dblist.Insert(ind, currentDB);

                props.Fields.Backup.pathLast = pathBrowser.Value;
                props.WriteXml();


                // Проверка на максимальное количество резервных копий

                try
                {
                    checkMaxCountOfBackupCopies();
                }
                catch
                {

                }

                backupDBGridView_upd();

            }
            catch
            {
            }

        }

        private void checkMaxCountOfBackupCopies()
        {
            backup backUP = props.Fields.Backup;

            if (backUP.maxCount.active) // если контроль количества резервных копий включен
            {
                int cnt = int.Parse(backUP.maxCount.value.ToString());
                int countDB = db.BackupDB_Info.Count();
                if (countDB > cnt) // если количество резервных копий инфа о которых есть в базе больше установленного значения
                {
                    if (this.GetType() == typeof(BackupRestoreDB))
                    {
                        DialogResult dialog = RadMessageBox.Show(this, "Превышено установленное максимальное количество резервных копий! Удалить наиболее старые резервные копии?", "Внимание", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
                        if (dialog == DialogResult.No)
                        {
                            return;
                        }
                    }

                    var dbListForDel = db.BackupDB_Info.OrderByDescending(x => x.DateCreate.Value).Skip(cnt).Take(countDB - cnt).ToList();
                    List<string> sList = new List<string>();

                    foreach (var item in dbListForDel)
                    {
                        if (File.Exists(item.Path))  // если файл физически существует, удаляем его и добавляем его в список на удаление из базы
                        {
                            try
                            {
                                File.Delete(item.Path);
                                sList.Add(item.ID.ToString());
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show(this, "Ошибка при удалении файла архива базы данных с диска.\r\n" + ex.Message, "Внимание", MessageBoxButtons.OK, RadMessageIcon.Error);
                                return;
                            }
                        }
                        else // если файл резервной копии указанный в базе не найден, то просто добавляем его в список на удаление из базы
                        {
                            sList.Add(item.ID.ToString());
                        }

                    }
                    if (sList.Count() != 0)
                        delRecordFromDB(string.Join(", ", sList));
                }
            }


        }

        public static string UTF8ToWin1251(string inputString)
        {
            Encoding srcEncodingFormat = Encoding.UTF8;
            Encoding dstEncodingFormat = Encoding.GetEncoding(1251);
            byte[] originalByteString = srcEncodingFormat.GetBytes(inputString);
            byte[] convertedByteString = Encoding.Convert(srcEncodingFormat,
            dstEncodingFormat, originalByteString);
            string strOut = dstEncodingFormat.GetString(convertedByteString);

            return strOut;
        }
        public static string Win1251ToUTF8(string inputString)
        {
            Encoding srcEncodingFormat = Encoding.GetEncoding("windows-1251");
            Encoding dstEncodingFormat = Encoding.UTF8;
            byte[] originalByteString = srcEncodingFormat.GetBytes(inputString);
            byte[] convertedByteString = Encoding.Convert(srcEncodingFormat,
            dstEncodingFormat, originalByteString);
            string strOut = dstEncodingFormat.GetString(convertedByteString);

            return strOut;
        }



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

        private void restoreBtn_Click(object sender, EventArgs e)
        {
            if (backupDBGridView.CurrentRow.Cells["notExist"].Value == null || (backupDBGridView.CurrentRow.Cells["notExist"].Value != null && backupDBGridView.CurrentRow.Cells["notExist"].Value.ToString() != "1"))
            {
                string path = backupDBGridView.CurrentRow.Cells["Path"].Value.ToString();
                if (File.Exists(path))
                {
                    try
                    {
                        FileInfo file = new FileInfo(path);
                        if (file.Extension == ".zip")
                        {
                            using (ZipFile zip = ZipFile.Read(path, new ReadOptions { Encoding = System.Text.Encoding.UTF8 }))
                            {
                                string file_zipped = zip.EntryFileNames.First();
                                FileInfo fi = new FileInfo(currentDB.path);
                                if (file_zipped != fi.Name)
                                {
                                    DialogResult dialogResult = RadMessageBox.Show(this, String.Format("Имя файла ({0}) текущей базы данных не соответствует имени файла в архиве ({1}).\r\nВсе равно продолжить - восстановить базу данных из архива и переименовать полученный файл?", fi.Name, file_zipped), "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);

                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        zip.ExtractAll(fi.DirectoryName, ExtractExistingFileAction.OverwriteSilently);
                                        File.Replace(Path.Combine(fi.DirectoryName, file_zipped), currentDB.path, null);
                                    }
                                    else
                                    {
                                        return;
                                    }

                                }
                                else
                                {
                                    zip.ExtractAll(fi.DirectoryName, ExtractExistingFileAction.OverwriteSilently);
                                }
                                //                                RadMessageBox.Show(UTF8ToWin1251(zip.Comment));

                            }
                            restoreFlag = true;
                            Methods.showAlert("Внимание", "Восстановление из резервной копии прошло успешно!", this.ThemeName);
                        }
                    }
                    catch { }
                }
                else
                    Methods.showAlert("Ошибка восстановления", "Выбранный файл резервной копии не найден на диске!", this.ThemeName);
            }
            else
            {
                Methods.showAlert("Ошибка восстановления", "Выбранный файл резервной копии не найден на диске!", this.ThemeName);
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "(*.zip)|*.zip";
            openDialog.Multiselect = false;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(openDialog.FileName);
                if (!db.BackupDB_Info.Any(x => x.Path == file.FullName))
                {
                    string description = "";
                    try
                    {
                        if (file.Extension == ".zip")
                        {
                            using (ZipFile zip = ZipFile.Read(file.FullName, new ReadOptions { Encoding = System.Text.Encoding.UTF8 }))
                            {

                                description = UTF8ToWin1251(zip.Comment);
                            }
                        }
                    }
                    catch { }

                    DateTime dt = file.CreationTime;
                    BackupDB_Info backup_new = new BackupDB_Info
                    {
                        DateCreate = dt,
                        UserID = Options.User.ID,
                        Path = file.FullName,
                        Description = description
                    };

                    db.BackupDB_Info.Add(backup_new);
                    db.SaveChanges();

                    backupDBGridView_upd();
                }
                else // Если такой архив уже есть в списке
                {
                    Methods.showAlert("Внимание", "Выбранный архив Базы данных уже есть в списке!", this.ThemeName);
                }

            }
        }

        private void closeBtn_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
