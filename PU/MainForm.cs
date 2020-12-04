using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.FormsRSW2014;
using PU.FormsSPW2_2014;
using PU.Classes;
using PU.Models;
using System.Data.Objects;
using System.Data.Metadata.Edm;
using System.IO;
using Telerik.WinControls.UI.Localization;
using PU.FormsRSW2_2014;
using PU.FormsRW_3_2015;
using PU.FormsSZV_6_4_2013;
using PU.FormsSZV_6_2010;
using System.Xml.Linq;
using System.Reflection;
using Ionic.Zip;
using Telerik.WinControls.UI.Docking;
using System.Net;
using Telerik.WinControls.UI;
using PU.Models.Mapping;
using PU.Models.ModelViews;

namespace PU
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
        public RadDock rd = new RadDock { AutoDetectMdiChildren = true, Location = new System.Drawing.Point(0, 19), Visible = false, ShowDocumentCloseButton = true };

        private bool saveOnClose = true;
        public bool loginFormExit = false;
        string dbpath = string.Empty;
        Props props = new Props(); //экземпляр класса с настройками
        string newVersionUrl;

        MdiClient client;

        public MainForm()
        {
            InitializeComponent();

            this.IsMdiContainer = true;
            //Find the MdiClient and hold it by a variable
            client = Controls.OfType<MdiClient>().First();
            //This will check whenever client gets focused and there aren't any
            //child forms opened, Send the client to back so that the other controls can be shown back.
            client.GotFocus += (s, e) =>
            {
                if (!MdiChildren.Any(x => x.Visible))
                {
                    client.SendToBack();
                    rd.SendToBack();
                    rd.Dock = DockStyle.None;
                    rd.Visible = false;
                }
            };
        }

        //This is used to show a child form
        //Note that we have to call client.BringToFront();

        private void ShowForm(Form childForm)
        {
            if (!rd.Visible)
            {
                rd.Visible = true;
                rd.BringToFront();
                rd.Dock = DockStyle.Fill;
            }

            this.Cursor = Cursors.WaitCursor;
            if (rd.MdiChildren.Any(x => x.Name == childForm.Name))
            {
                rd.MdiChildren.First(x => x.Name == childForm.Name).Select();

            }
            else
            {
                childForm.MdiParent = this;
                childForm.Show();
            }

            childForm.Focus();
            this.Cursor = Cursors.Default;
        }

        /*
        public class SQLiteDatabaseCreate
        {
            public void CreateDatabase(string newDatabase, string schemaDatabase)
            {
                var connectionString = string.Format("data source='{0}'", schemaDatabase);
                var dt = new System.Data.DataTable();
                using (var conn = new System.Data.SQLite.SQLiteConnection(connectionString))
                {
                    conn.Open();
                    dt = conn.GetSchema(System.Data.SQLite.SQLiteMetaDataCollectionNames.Tables);
                }

                using (var conn = CreateConnectionForSchemaCreation(newDatabase))
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    foreach (System.Data.DataRow dr in dt.Rows)
                    {
                        var createSql = dr["TABLE_DEFINITION"].ToString();
                        if (!createSql.Contains("sqlite_sequence"))
                        {
                            var cmd = new System.Data.SQLite.SQLiteCommand(createSql, conn);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }

            public System.Data.SQLite.SQLiteConnection CreateConnectionForSchemaCreation(string filename)
            {
                var conn = new System.Data.SQLite.SQLiteConnection();
                conn.ConnectionString = new System.Data.Common.DbConnectionStringBuilder {
            {"Data Source", filename},
            {"Version", "3"},
            {"FailIfMissing", "False"},
        }.ConnectionString;
                conn.Open();
                return conn;
            }
        }
        */


        private void createXaccessDB(string xaccessPath)
        {
            // This is the query which will create a new table in our database file with three columns. An auto increment column called "ID", and two NVARCHAR type columns with the names "Key" and "Value"
            string createTableQuery = @"CREATE TABLE [AccessCategory] (
  [ID] INTEGER PRIMARY KEY, 
  [Name] VARCHAR(30));


CREATE TABLE [AccessLevel] (
  [ID] INTEGER PRIMARY KEY, 
  [Name] VARCHAR(20));


CREATE TABLE [AccessObject] (
  [ID] INTEGER PRIMARY KEY, 
  [Name] VARCHAR(50), 
  [Description] VARCHAR(150), 
  [AccessCategoryID] INTEGER CONSTRAINT [fk_AccessCategory_This] REFERENCES [AccessCategory]([ID]) ON DELETE CASCADE ON UPDATE CASCADE);


CREATE TABLE [Roles] (
  [ID] INTEGER PRIMARY KEY AUTOINCREMENT, 
  [Name] VARCHAR(20));


CREATE TABLE [Users] (
  [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  [Name] VARCHAR(200), 
  [Login] VARCHAR(20), 
  [Password] VARCHAR(100), 
  [RoleID] INTEGER CONSTRAINT [fk_Users_Roles] REFERENCES [Roles]([ID]) ON DELETE CASCADE ON UPDATE CASCADE, 
  [DateCreate] TIMESTAMP, 
  [LastAccessDate] TIMESTAMP, 
  [Counter] SMALLINT, 
  [Blocked] SMALLINT, 
  [SysAdmin] SMALLINT);


CREATE TABLE [Activity] (
  [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  [UserID] INTEGER CONSTRAINT [fk_Activity_Users] REFERENCES [Users]([ID]) ON DELETE CASCADE ON UPDATE CASCADE, 
  [BaseName] VARCHAR(150), 
  [BasePath] VARCHAR(500), 
  [ActionDate] TIMESTAMP, 
  [Action] VARCHAR(20), 
  [Info] VARCHAR(300));


CREATE TABLE [Forms] (
  [ID] INTEGER PRIMARY KEY, 
  [Name] VARCHAR(40));


CREATE TABLE [FormsToObjects] (
  [ID] INTEGER PRIMARY KEY, 
  [FormsID] INTEGER CONSTRAINT [fk_Forms_This] REFERENCES [Forms]([ID]) ON DELETE CASCADE ON UPDATE CASCADE, 
  [ObjectsID] INTEGER CONSTRAINT [fk_AccessObject_This] REFERENCES [AccessObject]([ID]) ON DELETE CASCADE ON UPDATE CASCADE);


CREATE TABLE [UsersAccessLevelToObjects] (
  [ID] INTEGER PRIMARY KEY, 
  [UsersID] INTEGER CONSTRAINT [Users_This_fk] REFERENCES [Users]([ID]) ON DELETE CASCADE ON UPDATE CASCADE, 
  [AccessObjectID] INTEGER CONSTRAINT [AccessObject_This] REFERENCES [AccessObject]([ID]) ON DELETE CASCADE ON UPDATE CASCADE, 
  [AccessLevelID] INTEGER CONSTRAINT [fk_AccessLevel_This] REFERENCES [AccessLevel]([ID]) ON DELETE CASCADE ON UPDATE CASCADE);

INSERT INTO Roles (Name) VALUES ('Администратор');
INSERT INTO Roles (Name) VALUES ('Менеджер');
INSERT INTO Roles (Name) VALUES ('Пользователь');

INSERT INTO Users (Name, Login, Password, RoleID, SysAdmin ) VALUES ('Администратор','Администратор','111', 1, 1 );



";
            try
            {
                System.Data.SQLite.SQLiteConnection.CreateFile(xaccessPath);        // Create the file which will be hosting our database
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + xaccessPath, true))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database

                        com.CommandText = createTableQuery;     // Set CommandText to our query that will create the table
                        com.ExecuteNonQuery();                  // Execute the query
                        con.Close();        // Close the connection to the database

                        Messenger.showAlert(AlertType.Info, "Внимание!", "База данных с учетными записями не была найдена по пути - " + props.Fields.xaccessPath + "\r\n\r\nБыла создана база данных с учетными записями по умолчанию в каталоге с программой " + xaccessPath, this.ThemeName, 200);
                    }
                }
            }
            catch (Exception ex)
            {
                Messenger.showAlert(AlertType.Error, "Ошибка!", "База данных с учетными записями не была найдена по пути - " + props.Fields.xaccessPath + "\r\n\r\nПри попытке создания базы данных с учетными записями по умолчанию в каталоге с программой " + xaccessPath + " произошла ошибка! [ " + ex.InnerException != null ? ex.InnerException.Message : ex.Message + " ]", this.ThemeName, 250);
            }

        }
        /// <summary>
        /// Проверка на существование базы пользователей по указанному в настройках пути, если его нет или нулевой файл, ищем в каталоге с прогой если в настройках другой путь, если тоже нет, то создаем новый
        /// </summary>
        /// <returns></returns>
        private string checkExistXaccessDB()
        {
            string xaccessPath = props.Fields.xaccessPath;
            //ищем базу по пути в настройках
            if (!File.Exists(xaccessPath))
            {
                //ищем в каталоге с программой если он отличается от того что в настройках
                if (xaccessPath != Path.Combine(Application.StartupPath, "xaccess.db3"))
                {
                    xaccessPath = Path.Combine(Application.StartupPath, "xaccess.db3");
                    if (!File.Exists(xaccessPath))
                    {
                        createXaccessDB(xaccessPath);
                    }
                    else
                    {
                        FileInfo fi = new FileInfo(xaccessPath);
                        if (fi.Length == 0)
                        {
                            System.IO.File.Delete(xaccessPath);
                            fi = null;
                            createXaccessDB(xaccessPath);
                        }
                    }
                }
                else
                {
                    //создаем новую БД
                    createXaccessDB(xaccessPath);
                }
            }
            else
            {
                FileInfo fi = new FileInfo(xaccessPath);
                if (fi.Length == 0)
                {
                    //ищем в каталоге с программой если он отличается от того что в настройках
                    if (xaccessPath != Path.Combine(Application.StartupPath, "xaccess.db3"))
                    {
                        xaccessPath = Path.Combine(Application.StartupPath, "xaccess.db3");
                        if (!File.Exists(xaccessPath))
                        {
                            createXaccessDB(xaccessPath);
                        }
                        else
                        {
                            fi = new FileInfo(xaccessPath);
                            if (fi.Length == 0)
                            {
                                System.IO.File.Delete(xaccessPath);
                                fi = null;
                                createXaccessDB(xaccessPath);
                            }
                        }
                    }
                    else
                    {
                        System.IO.File.Delete(xaccessPath);
                        fi = null;
                        createXaccessDB(xaccessPath);
                    }
                }
            }

            return xaccessPath;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Controls.Add(rd);
            rd.DocumentManager.DocumentInsertOrder = DockWindowInsertOrder.ToBack;
            rd.DockWindowClosed += (s, v) =>
            {
                if (!rd.MdiChildren.Any())
                {
                    rd.SendToBack();
                    rd.Dock = DockStyle.None;
                    rd.Visible = false;
                }
            };

            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            RadMessageLocalizationProvider.CurrentProvider = new RussianRadMessageBoxLocalizationProvider();

            props.ReadXml();
            Options.ThemeName = props.Fields.ThemeName;
            Options.fixCurrentInsurer = props.Fields.fixCurrentInsurer;
            Options.saveLastPackNum = props.Fields.saveLastPackNum;
            Options.getInsurerFIOtoNewForm = props.Fields.getInsurerFIOtoNewForm;
            Options.printAllPagesRSV1 = props.Fields.printAllPagesRSV1;
            Options.formParams = props.Fields.formParams;
            Options.gridParams = props.Fields.gridParams;
            Options.InsurerFolders = props.Fields.InsurerFolders;
            Options.pathCheckPfr = props.Fields.pathCheckPfr;
            Options.hideDialogCheckFiles = props.Fields.hideDialogCheckFiles;
            Options.checkFilesAfterSaving = props.Fields.checkFilesAfterSaving;

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    this.Size = param.size;
                    this.Location = param.location;
                    this.WindowState = param.windowState == FormWindowState.Minimized ? FormWindowState.Maximized : param.windowState;
                }
                catch
                { }
            }



            //Options.useRSW1_2015 = props.Fields.useRSW1_2015;
            this.ThemeName = Options.ThemeName;
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);



            // Пишем дату в статус бар
            DateRadLabelElement.Text = DateTime.Now.ToLongDateString();

            //Пишем название проги и версию в статус бар

            AppNameVerLabelElement.Text = AssemblyTitle + "      " + String.Format("Версия: {0}", AssemblyVersion);


            /*           pu6Entities context = new pu6Entities();
                        var tableList = context.MetadataWorkspace.GetItems<EntityType>(System.Data.Metadata.Edm.DataSpace.CSpace);
                        foreach (var item in tableList)
                        {
                            radDropDownList1.Items.Add(item.Name);
                        }
              */

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



        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!loginFormExit)
            {
                ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
                Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);


                //RadMessageBox.Instance.FormElement.TitleBar.Font = new Font("Calibri", 20f);

                //// I added this additional check for safety, if Telerik modifies the name of the control.
                //if (RadMessageBox.Instance.Controls.ContainsKey("radLabel1"))
                //{
                //    RadMessageBox.Instance.Controls["radLabel1"].Font = new Font("Calibri", 25f, FontStyle.Regular);
                //}


                if (RadMessageBox.Show("Вы уверены, что хотите завершить работу с программой?", "Внимание!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    e.Cancel = false;
                    if (saveOnClose)
                    {
                        props.ReadXml();
                        backup backUP = props.Fields.Backup;

                        if (backUP.autoBackup.active)
                        {
                            pu6Entities db = new pu6Entities();

                            decimal totalDays = 1000;
                            if (db.BackupDB_Info.Any())
                            {
                                DateTime lastDate = db.BackupDB_Info.Max(x => x.DateCreate.Value);
                                totalDays = decimal.Parse((DateTime.Now - lastDate).TotalDays.ToString());
                            }

                            if (totalDays >= backUP.autoBackup.value)
                            {
                                var dblist = props.Fields.DBList;
                                DBEntry currentDB = dblist.First(x => x.actual);
                                string path = "";

                                if (!String.IsNullOrEmpty(currentDB.pathBackup))
                                {
                                    path = currentDB.pathBackup;
                                }
                                else
                                {
                                    path = Application.StartupPath + "\\Архивы БД\\" + currentDB.name;
                                }
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
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
                                        zip.Save(Path.Combine(path, newFileName));
                                    }
                                    BackupDB_Info backup_new = new BackupDB_Info
                                    {
                                        DateCreate = dt,
                                        UserID = Options.User.ID,
                                        Path = Path.Combine(path, newFileName),
                                        Description = currentDB.name + ". Архив Базы данных программы " + AssemblyTitle + String.Format(" Версия: {0}", AssemblyVersion) + " от " + dt.ToString("dd.MM.yyyy HH:mm")
                                    };

                                    db.BackupDB_Info.Add(backup_new);
                                    db.SaveChanges();

                                    bool updateProps = false;
                                    if (currentDB.pathBackup != path)
                                    {
                                        var ind = dblist.IndexOf(dblist.First(x => x.actual));
                                        currentDB.pathBackup = path;
                                        dblist.RemoveAt(ind);
                                        dblist.Insert(ind, currentDB);
                                        updateProps = true;
                                    }

                                    if (props.Fields.Backup.pathLast != path)
                                    {
                                        props.Fields.Backup.pathLast = path;
                                        updateProps = true;
                                    }

                                    if (updateProps)
                                        props.WriteXml();

                                    // Проверка на максимальное количество резервных копий

                                    try
                                    {
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
                                                {
                                                    string query = "DELETE FROM BackupDB_Info WHERE ID in (" + string.Join(", ", sList) + ")";
                                                    db.Database.ExecuteSqlCommand(query);
                                                }
                                            }
                                        }
                                    }
                                    catch
                                    {

                                    }

                                }
                                catch
                                {
                                }

                            }


                        }

                        //maxCountCheckBox.Checked = item.maxCount.active;
                        //maxCountSpinEditor.Value = item.maxCount.value;

                        props.setFormParams(this, null);

                        methodsNonStatic.writeSetting();
                    }
                }
            }

        }


       

      

        private void radMenuItem4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       


        private void radMenuItemDictionary_Click(object sender, EventArgs e)
        {
            var child =   Dictionaries.BaseDictionaryEvents.Dialog(this,((RadMenuItem)sender).Tag.ToString(), ((RadMenuItem)sender).Text);
            ShowForm(child); 
           
        }

        private void radMenuItemTest_Click(object sender, EventArgs e)
        {
            var child = new Dictionaries.PlatCategoryFormList1();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();

        }


        private void radMenuItem15_Click(object sender, EventArgs e)
        {
            RaschetPeriodFrm child = new RaschetPeriodFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();

        }


        private void radMenuItem16_Click(object sender, EventArgs e)
        {
            PlatCategoryFrm child = new PlatCategoryFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            //child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();
        }



        private void radMenuItem19_Click(object sender, EventArgs e)
        {
            InsurerFrm child = new InsurerFrm();
            //child.MdiParent = this;
            child.ThemeName = this.ThemeName;
            //child.ShowInTaskbar = false;

            //      child.MaximumSize = child.Size;
            ShowForm(child);
            //                child.Show();
            //            child.WindowState = FormWindowState.Normal;
        }


        public static bool IsDate(object attemptedDate)
        {
            bool Success;

            try
            {
                DateTime dtParse = DateTime.Parse(attemptedDate.ToString());
                Success = true; // это дата
            }
            catch
            {
                Success = false; // это не дата
            }

            return Success;
        }


        /// <summary>
        /// Форма РСВ-1 (2014)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radMenuItem22_Click(object sender, EventArgs e)
        {
            //if (this.MdiChildren.Any(x => x.Name == "RSW2014_List"))
            //{
            //    this.MdiChildren.First(x => x.Name == "RSW2014_List").Activate();
            //}
            //else
            //{
            RSW2014_List child = new RSW2014_List();
            child.ThemeName = this.ThemeName;
            /*           if (Options.formParams.Any(x => x.name == child.Name))
                       {
                           var param = Options.formParams.FirstOrDefault(x => x.name == child.Name);
                           try
                           {
                               child.Size = param.size;
                               child.Location = param.location;
                               child.WindowState = param.windowState;
                           }
                           catch
                           { }
                       }

                       child.ThemeName = this.ThemeName;
                       child.ShowInTaskbar = false;
                       child.IsMdiContainer = false;
                   */


            ShowForm(child);
            //                child.Show();
            //                child.WindowState = FormWindowState.Normal;

            //      }

        }

        /// <summary>
        /// Форма СПВ-2 (2014)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radMenuItem26_Click(object sender, EventArgs e)
        {
            //if (this.MdiChildren.Any(x => x.Name == "SPW2_List"))
            //{
            //    this.MdiChildren.First(x => x.Name == "SPW2_List").Activate();
            //}
            //else
            //{
            SPW2_List child = new SPW2_List();
            //child.MdiParent = this;
            child.ThemeName = this.ThemeName;
            //child.ShowInTaskbar = false;
            //child.MaximumSize = child.Size;
            ShowForm(child);
            //                child.Show();

            //    child.WindowState = FormWindowState.Normal;
            //}

        }


        /// <summary>
        /// Классификатор вредных профессий
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radMenuItem23_Click(object sender, EventArgs e)
        {
            KodVred child = new KodVred();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();
        }

       

        private void radMenuItem25_Click(object sender, EventArgs e)
        {
            TariffCodeFrm child = new TariffCodeFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();
        }


        private void radMenuItem29_Click(object sender, EventArgs e)
        {
            BaseManager child = new BaseManager();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();
        }



        private void radMenuItem31_Click(object sender, EventArgs e)
        {
            RSW2_2014_List child = new RSW2_2014_List();
            //child.MdiParent = this;
            child.ThemeName = this.ThemeName;
            child.Year = 2014;
            //child.MaximumSize = child.Size;
            ShowForm(child);
            //                child.Show();
            //            child.WindowState = FormWindowState.Normal;
        }

        private void radMenuItem33_Click(object sender, EventArgs e)
        {
            SZV_6_4_List child = new SZV_6_4_List();
            //child.MdiParent = this;
            child.ThemeName = this.ThemeName;
            //child.MaximumSize = child.Size;
            ShowForm(child);
            //                child.Show();
            //child.WindowState = FormWindowState.Normal;
        }

        private void radMenuItem35_Click(object sender, EventArgs e)
        {
            SZV_6_List child = new SZV_6_List();
            //child.MdiParent = this;
            child.ThemeName = this.ThemeName;
            //child.MaximumSize = child.Size;
            ShowForm(child);
            //                child.Show();
            //            child.WindowState = FormWindowState.Normal;
        }


        private void MainForm_Shown(object sender, EventArgs e)
        {
            //Получаем путь до базы пользователей
            string xaccessPath = checkExistXaccessDB();
            if (xaccessPath != props.Fields.xaccessPath)
            {
                Messenger.showAlert(AlertType.Info, "Внимание!", "База данных с учетными записями не была найдена по пути - " + props.Fields.xaccessPath + "\r\n\r\nИспользуется база данных с учетными записями " + xaccessPath, this.ThemeName, 200);
                props.Fields.xaccessPath = xaccessPath;
                props.WriteXml();
            }

            xaccessPath = Utils.CorrectUNCpathToSqlite(xaccessPath);

            // xaccess Data Base File
            StringBuilder Sb = new StringBuilder();
            Sb.Append(@"metadata=res://*/UserAccess.xaccessModel.csdl|res://*/UserAccess.xaccessModel.ssdl|res://*/UserAccess.xaccessModel.msl;provider=System.Data.SQLite;");
            Sb.Append(@"provider connection string='data source=" + xaccessPath + ";foreign keys=true;'");

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if ((config.ConnectionStrings.ConnectionStrings["xaccessEntities"] == null) || ((config.ConnectionStrings.ConnectionStrings["xaccessEntities"] != null) && (config.ConnectionStrings.ConnectionStrings["xaccessEntities"].ConnectionString != Sb.ToString())))
            {
                if (config.ConnectionStrings.ConnectionStrings["xaccessEntities"] != null)
                {
                    config.ConnectionStrings.ConnectionStrings["xaccessEntities"].ConnectionString = Sb.ToString();
                }
                else
                {
                    config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings
                    {
                        Name = "xaccessEntities",
                        ConnectionString = Sb.ToString(),
                        ProviderName = "System.Data.EntityClient"
                    });
                }
                string result = Methods.saveConfigFile(config, "xaccessEntities", true);
                if (!String.IsNullOrEmpty(result))
                {
                    this.Invoke(new Action(() => { Messenger.showAlert(AlertType.Info, "Внимание", result, this.ThemeName); }));
                }
                //config.Save(ConfigurationSaveMode.Minimal);

            }

            if (!Directory.Exists(Path.Combine(Application.StartupPath, "Database")))
            {
                Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Database"));
            }

            // Проверка с потоке существования базы xmlDB
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = false;
            bw.WorkerSupportsCancellation = false;
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(checkXmlDB);
            bw.RunWorkerAsync();

            // Окно авторизации
            LoginFrm child = new LoginFrm();
            child.lastName = props.Fields.xaccessLastName;
            child.ThemeName = this.ThemeName;
            child.Owner = this;
            child.ShowDialog();

            if (Options.User != null)
            {
                radPanorama1.Visible = true;
                MainForm_Resize(null, null);

                UserNameRadLabelElement.Text = Options.User.Name;
                UserNameRadLabelElement.Visibility = ElementVisibility.Visible;

                if (child.rememberNameCheckBox.Checked)
                {
                    props.Fields.xaccessLastName = Options.User.Login;
                }
                else
                {
                    props.Fields.xaccessLastName = "null";
                }
                props.WriteXml();


                // читаем настройки приложения
                methodsNonStatic.readSetting();

                //когда есть настройки сервиса РК АСВ
                if (!String.IsNullOrEmpty(Options.RKASV.url) && !String.IsNullOrEmpty(Options.RKASV.opfrCode) && !String.IsNullOrEmpty(Options.RKASV.service))
                {
                    radMenuItem61.Visibility = ElementVisibility.Visible;
                }


                dbpath = Options.DBActual.path != null ? Options.DBActual.path : Path.Combine(Application.StartupPath, "Database", "pu6.db3");
                FileInfo fi_ = new FileInfo(dbpath);
                this.Text = "Документы ПУ-6  -  [ " + Options.DBActual.name + " ]";

                while (!File.Exists(dbpath) || fi_.Length == 0)
                {
                    DialogResult dialogResult = RadMessageBox.Show(this, "По указанному пути - " + dbpath + " база данных не найдена!\r\nВыберите другую базу данных или создайте новую базу при необходимости.", "Внимание", MessageBoxButtons.OKCancel, RadMessageIcon.Error);
                    if (dialogResult == DialogResult.OK)
                    {
                        BaseManager child_ = new BaseManager();
                        child_.Owner = this;
                        child_.ThemeName = this.ThemeName;
                        child_.ShowInTaskbar = true;
                        child_.ShowDialog();
                        if (child_.dblist != null && child_.dblist.Count() > 0 && child_.dblist.Any(x => x.actual == true))
                        {
                            dbpath = child_.dblist.First(x => x.actual == true).path;
                            fi_ = new FileInfo(dbpath);
                        }
                    }
                    else
                    {
                        saveOnClose = false;
                        Application.Exit();
                    }

                }
                dbpath = Utils.CorrectUNCpathToSqlite(dbpath);

                Sb = new StringBuilder();
                Sb.Append(@"metadata=res://*/Models.Model.csdl|res://*/Models.Model.ssdl|res://*/Models.Model.msl;provider=System.Data.SQLite;");
                Sb.Append(@"provider connection string='data source=" + dbpath + ";foreign keys=true;ParseConnectionString=True'");

                Options.pu6conn = Sb.ToString();

                //            db.Connection.ConnectionString = Sb.ToString();

                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if ((config.ConnectionStrings.ConnectionStrings["pu6Entities"] == null) || ((config.ConnectionStrings.ConnectionStrings["pu6Entities"] != null) && (config.ConnectionStrings.ConnectionStrings["pu6Entities"].ConnectionString != Sb.ToString())))
                {
                    if (config.ConnectionStrings.ConnectionStrings["pu6Entities"] != null)
                    {
                        config.ConnectionStrings.ConnectionStrings["pu6Entities"].ConnectionString = Sb.ToString();
                    }
                    else
                    {
                        config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings
                        {
                            Name = "pu6Entities",
                            ConnectionString = Sb.ToString(),
                            ProviderName = "System.Data.EntityClient"
                        });
                    }
                    string result = Methods.saveConfigFile(config, "pu6Entities", true);
                    if (!String.IsNullOrEmpty(result))
                    {
                        this.Invoke(new Action(() => { Messenger.showAlert(AlertType.Info, "Внимание", result, this.ThemeName); }));
                    }

                    //config.Save(ConfigurationSaveMode.Minimal);
                    //PU.Properties.Settings.Default.Reload();
                    //ConfigurationManager.RefreshSection("connectionStrings");
                }



                // Устанавливаем версию программы в соответствии с версией эталонной БД, если это запуск проги из проекта
                if (File.Exists(Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3")))
                { Methods.setBaseEmpVersionToAppVersion(); }



                string dbpath_emp = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");

                dbpath_emp = Utils.CorrectUNCpathToSqlite(dbpath_emp);


                methodsNonStatic.updateDBFromDB_Empty(dbpath, dbpath_emp, this.ThemeName, this); // Метод проверки версии БД и обновление структуры

                System.Threading.Thread.Sleep(100);
                pu6Entities db;

                bool flag = false;
                for (int i = 1; i <= 10; i++)
                {
                    while (!flag)
                    {
                        try
                        {
                            db = new pu6Entities();
                            flag = true;
                            break;
                        }
                        catch
                        {
                            flag = false;
                            System.Threading.Thread.Sleep(100);
                        }
                    }
                }


                db = new pu6Entities();
                if (!db.Insurer.Any(x => x.ID == Options.InsID))
                {
                    Options.InsID = 0;
                    methodsNonStatic.writeSetting();
                }
                if (Options.User.RoleID == 1)
                {
                    radMenuItem17.Visibility = ElementVisibility.Visible;
                }
                else
                {
                    radMenuItem17.Visibility = ElementVisibility.Hidden;
                }

                bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(preLoadOperation1);
                bw.RunWorkerAsync();


                // Проверка базы на наличие определенных записей, в фоне
                BackgroundWorker bw_checkDBContent = new BackgroundWorker();
                bw_checkDBContent.DoWork += new System.ComponentModel.DoWorkEventHandler(checkDBContent);
                bw_checkDBContent.RunWorkerAsync();





                if (props.Fields.autoCheckNewVersion)
                {
                    BackgroundWorker bw_checkVersion = new BackgroundWorker();
                    bw_checkVersion.DoWork += new System.ComponentModel.DoWorkEventHandler(autoCheckVersion);
                    bw_checkVersion.RunWorkerAsync();
                }

            }
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

        /// <summary>
        /// Проверка на доступность новой версии программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCheckVersion(object sender, DoWorkEventArgs e)
        {
            try
            {
                //System.Threading.Thread.Sleep(5000);

                //WebClient client = new WebClient();
                //client.Timeout = 1000;
                //string s = client.DownloadString("http://www.pfrf.ru/files/branches/komi/Soft/DocPU5/pu6version.txt");
                //if (!String.IsNullOrEmpty(s) && s.Contains('|'))
                //{
                //    var str = s.Split('|');
                //    if (str.Length >= 2)
                //    {
                //        newVersionUrl = str[1];

                //        Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                //        Version latestVersion = new Version(str[0]);
                //        if (latestVersion > currentVersion)
                //        {
                //            // Доступна новая версия
                //            newVersionCommandBarSeparator.AutoSize = true;
                //            newVersionAlert.AutoSize = true;
                //            newVersionBtn.AutoSize = true;

                //            newVersionCommandBarSeparator.Visibility = ElementVisibility.Visible;
                //            newVersionAlert.Visibility = ElementVisibility.Visible;
                //            newVersionBtn.Visibility = ElementVisibility.Visible;
                //            newVersionBtn.ToolTipText = "Открыть страницу для скачивания новой версии Документы ПУ-6 (" + str[0] + ")";
                //        }
                //    }
                //}


                var request = HttpWebRequest.Create("http://www.pfrf.ru/files/branches/komi/Soft/DocPU5/pu6version.txt");
                request.Timeout = 1000;
                using (var response = request.GetResponse())
                {
                    string s = response.ToString();

                    if (!String.IsNullOrEmpty(s) && s.Contains("|"))
                    {
                        var str = s.Split('|');

                        string url = str[1];

                        Version currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                        Version latestVersion = new Version(str[0]);
                        if (latestVersion > currentVersion)
                        {
                            // Доступна новая версия
                            newVersionCommandBarSeparator.AutoSize = true;
                            newVersionAlert.AutoSize = true;
                            newVersionBtn.AutoSize = true;

                            newVersionCommandBarSeparator.Visibility = ElementVisibility.Visible;
                            newVersionAlert.Visibility = ElementVisibility.Visible;
                            newVersionBtn.Visibility = ElementVisibility.Visible;
                            newVersionBtn.ToolTipText = "Открыть страницу для скачивания новой версии Документы ПУ-6 (" + str[0] + ")";
                        }
                    }

                }


            }
            catch
            {
                //                newVersionCommandBarSeparator.Dispose();
                //                newVersionAlert.Dispose();
                //                newVersionBtn.Dispose();        
            }
        }

        private void preLoadOperation1(object sender, DoWorkEventArgs e)
        {

            int year = DateTime.Now.Year;

            Options.RaschetPeriodInternal = new List<RaschetPeriodContainer>();

            for (short y = 2014; y <= year; y++)
            {
                Options.RaschetPeriodInternal.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 3, Name = "3 месяца " + y, DateBegin = DateTime.Parse("01.01." + y), DateEnd = DateTime.Parse("31.03." + y) });
                Options.RaschetPeriodInternal.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 6, Name = "полугодие " + y, DateBegin = DateTime.Parse("01.04." + y), DateEnd = DateTime.Parse("30.06." + y) });
                Options.RaschetPeriodInternal.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 9, Name = "9 месяцев " + y, DateBegin = DateTime.Parse("01.07." + y), DateEnd = DateTime.Parse("30.09." + y) });
                Options.RaschetPeriodInternal.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 0, Name = "12 месяцев " + y, DateBegin = DateTime.Parse("01.10." + y), DateEnd = DateTime.Parse("31.12." + y) });
                Options.RaschetPeriodInternal.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 4, Name = "Год " + y, DateBegin = DateTime.Parse("01.01." + y), DateEnd = DateTime.Parse("31.12." + y) });
            }

            Options.RaschetPeriodInternal2017 = new List<RaschetPeriodContainer>();
            for (short y = 2017; y <= year; y++)
            {
                Options.RaschetPeriodInternal2017.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 0, Name = "в целом за год " + y, DateBegin = DateTime.Parse("01.01." + y), DateEnd = DateTime.Parse("31.12." + y) });
            }


            Options.RaschetPeriodInternal2010_2013 = new List<RaschetPeriodContainer>();
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2010, Kvartal = 1, Name = "первое полугодие 2010", DateBegin = DateTime.Parse("01.01.2010"), DateEnd = DateTime.Parse("30.06.2010") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2010, Kvartal = 2, Name = "второе полугодие 2010", DateBegin = DateTime.Parse("01.07.2010"), DateEnd = DateTime.Parse("31.12.2010") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2010, Kvartal = 0, Name = "в целом за год 2010", DateBegin = DateTime.Parse("01.01.2010"), DateEnd = DateTime.Parse("31.12.2010") });

            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2011, Kvartal = 1, Name = "первый квартал 2011", DateBegin = DateTime.Parse("01.01.2011"), DateEnd = DateTime.Parse("31.03.2011") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2011, Kvartal = 2, Name = "второй квартал 2011", DateBegin = DateTime.Parse("01.04.2011"), DateEnd = DateTime.Parse("30.06.2011") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2011, Kvartal = 3, Name = "третий квартал 2011", DateBegin = DateTime.Parse("01.07.2011"), DateEnd = DateTime.Parse("30.09.2011") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2011, Kvartal = 4, Name = "четвертый квартал 2011", DateBegin = DateTime.Parse("01.10.2011"), DateEnd = DateTime.Parse("31.12.2011") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2011, Kvartal = 0, Name = "в целом за год 2011", DateBegin = DateTime.Parse("01.01.2011"), DateEnd = DateTime.Parse("31.12.2011") });

            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2012, Kvartal = 1, Name = "первый квартал 2012", DateBegin = DateTime.Parse("01.01.2012"), DateEnd = DateTime.Parse("31.03.2012") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2012, Kvartal = 2, Name = "второй квартал 2012", DateBegin = DateTime.Parse("01.04.2012"), DateEnd = DateTime.Parse("30.06.2012") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2012, Kvartal = 3, Name = "третий квартал 2012", DateBegin = DateTime.Parse("01.07.2012"), DateEnd = DateTime.Parse("30.09.2012") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2012, Kvartal = 4, Name = "четвертый квартал 2012", DateBegin = DateTime.Parse("01.10.2012"), DateEnd = DateTime.Parse("31.12.2012") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2012, Kvartal = 0, Name = "в целом за год 2012", DateBegin = DateTime.Parse("01.01.2012"), DateEnd = DateTime.Parse("31.12.2012") });

            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2013, Kvartal = 1, Name = "первый квартал 2013", DateBegin = DateTime.Parse("01.01.2013"), DateEnd = DateTime.Parse("31.03.2013") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2013, Kvartal = 2, Name = "второй квартал 2013", DateBegin = DateTime.Parse("01.04.2013"), DateEnd = DateTime.Parse("30.06.2013") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2013, Kvartal = 3, Name = "третий квартал 2013", DateBegin = DateTime.Parse("01.07.2013"), DateEnd = DateTime.Parse("30.09.2013") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2013, Kvartal = 4, Name = "четвертый квартал 2013", DateBegin = DateTime.Parse("01.10.2013"), DateEnd = DateTime.Parse("31.12.2013") });
            Options.RaschetPeriodInternal2010_2013.Add(new RaschetPeriodContainer() { Year = 2013, Kvartal = 0, Name = "в целом за год 2013", DateBegin = DateTime.Parse("01.01.2013"), DateEnd = DateTime.Parse("31.12.2013") });


            Options.RaschetPeriodInternal1996_2009 = new List<RaschetPeriodContainer>();
            for (short y = 1996; y <= 2000; y++)
            {
                Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 0, Name = "год " + y, DateBegin = DateTime.Parse("01.01." + y), DateEnd = DateTime.Parse("31.12." + y) });
                Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 2, Name = "1 полугодие " + y, DateBegin = DateTime.Parse("01.01." + y), DateEnd = DateTime.Parse("30.06." + y) });
                Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 4, Name = "2 полугодие " + y, DateBegin = DateTime.Parse("01.07." + y), DateEnd = DateTime.Parse("31.12." + y) });
            }


            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 0, Name = "в целом за год 2001", DateBegin = DateTime.Parse("01.01.2001"), DateEnd = DateTime.Parse("31.12.2001") });
            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 1, Name = "1 полугодие 2001", DateBegin = DateTime.Parse("01.01.2001"), DateEnd = DateTime.Parse("30.06.2001") });
            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 2, Name = "2 полугодие 2001", DateBegin = DateTime.Parse("01.07.2001"), DateEnd = DateTime.Parse("31.12.2001") });
            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 3, Name = "1 квартал 2001", DateBegin = DateTime.Parse("01.01.2001"), DateEnd = DateTime.Parse("31.03.2001") });
            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 4, Name = "2 квартал 2001", DateBegin = DateTime.Parse("01.04.2001"), DateEnd = DateTime.Parse("30.06.2001") });
            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 5, Name = "3 квартал 2001", DateBegin = DateTime.Parse("01.07.2001"), DateEnd = DateTime.Parse("30.09.2001") });
            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 6, Name = "4 квартал 2001", DateBegin = DateTime.Parse("01.10.2001"), DateEnd = DateTime.Parse("31.12.2001") });
            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 7, Name = "9 месяцев 2001", DateBegin = DateTime.Parse("01.01.2001"), DateEnd = DateTime.Parse("30.09.2001") });
            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 8, Name = "2-3 квартал 2001", DateBegin = DateTime.Parse("01.04.2001"), DateEnd = DateTime.Parse("30.09.2001") });
            Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = 2001, Kvartal = 9, Name = "2-4 квартал 2001", DateBegin = DateTime.Parse("01.04.2001"), DateEnd = DateTime.Parse("31.12.2001") });

            for (short y = 2002; y <= 2009; y++)
            {
                Options.RaschetPeriodInternal1996_2009.Add(new RaschetPeriodContainer() { Year = y, Kvartal = 0, Name = "год " + y, DateBegin = DateTime.Parse("01.01." + y), DateEnd = DateTime.Parse("31.12." + y) });
            }



            Options.FilledBaseArr = new List<SimpleList>();
            Options.FilledBaseArr.Add(new SimpleList() { id = "0", name = "спецоценки" });
            Options.FilledBaseArr.Add(new SimpleList() { id = "1", name = "аттестации рабочих мест" });
            Options.FilledBaseArr.Add(new SimpleList() { id = "2", name = "спецоценки и аттестации рабочих мест" });

            try
            {
                Methods.checkAndSetActiveUser(Options.User);
            }
            catch
            { }

            try
            {
                pu6Entities db = new pu6Entities();
                if (db.Insurer.Any(y => y.ID == Options.InsID))
                {
                    var insR = db.Insurer.FirstOrDefault(y => y.ID == Options.InsID);
                    if (Options.InsurerFolders.Any(x => x.regnum == insR.RegNum))
                    {
                        var param = Options.InsurerFolders.FirstOrDefault(x => x.regnum == insR.RegNum);

                        Options.CurrentInsurerFolders.regnum = param.regnum;
                        Options.CurrentInsurerFolders.importPath = param.importPath;
                        Options.CurrentInsurerFolders.exportPath = param.exportPath;

                    }
                }
            }
            catch
            { }






        }

        private void checkDBContent(object sender, DoWorkEventArgs e)
        {
            // Синхронизация МРОТ и Тарифы доп. выплат за 2016 год
            pu6Entities db = new pu6Entities();

            List<string> tables = new List<string> { "FormsSZV_ISH_2017", "FormsSZV_KORR_2017", "FormsSZV_STAJ_2017" };

            foreach (var item in tables)
            {
                try
                {
                    db.Database.ExecuteSqlCommand(String.Format("DELETE FROM " + item + " WHERE (FormsODV_1_2017_ID NOT IN (SELECT ID FROM FormsODV_1_2017))"));
                }
                catch (Exception ex)
                {
                    Messenger.showAlert(AlertType.Error, "Внимание!", "Во время очистки таблицы " + item + " произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
                }
            }

            try
            {
                db.Database.ExecuteSqlCommand(String.Format("UPDATE FormsSZV_KORR_2017 SET Dismissed = 0 where Dismissed <> 1"));
            }
            catch (Exception ex)
            {
                Messenger.showAlert(AlertType.Error, "Внимание!", "Во время обновления поля Dismissed таблицы FormsSZV_KORR_2017 произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
            }

            try
            {
                db.Database.ExecuteSqlCommand(String.Format("UPDATE FormsSZV_ISH_2017 SET Dismissed = 0 where Dismissed <> 1"));
            }
            catch (Exception ex)
            {
                Messenger.showAlert(AlertType.Error, "Внимание!", "Во время обновления поля Dismissed таблицы FormsSZV_ISH_2017 произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
            }


            /* Справочник видов формы СЗВ-ТД  */

            string path = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");

            if (!UpdateDictionaries.updateTable("FormsSZV_TD_2020_TypesOfEvents", path, this.ThemeName, this))
            {
                Messenger.showAlert(AlertType.Error, "Внимание!", "Во время обновления Справочника видов мероприятий Формы СЗВ-ТД произошла ошибка!", this.ThemeName);
            }



            

            if (!db.MROT.Any(x => x.Year == 2017)) // если нет данных за 2016 год, то синхронизируем справочник
            {
                /*MROTView child = new MROTView();
                child.Synchronization(null, null);*/
            }

            if (!db.CodeBaseRW3_2015.Any(x => x.Year == 2016)) // если нет данных за 2016 год, то синхронизируем справочник
            {
                PU.Dictionaries.TariffCodeRW3Frm child = new PU.Dictionaries.TariffCodeRW3Frm();
                child.synchBtn_Click(null, null);
            }

            if (!db.DopTariff.Any(x => x.Year == 2016)) // если нет данных за 2016 год, то синхронизируем справочник
            {
                /*DopTariffFrm child = new DopTariffFrm();
                child.synchBtn_Click(null, null);*/
            }

            if (!db.TariffPlat.Any(x => x.Year == 2016) || (!db.PlatCategory.Where(x => x.Code == "ТОР").Any(x => x.TariffPlat.Any(c => c.Year == 2016))) || !db.PlatCategory.Any(x => x.Code == "СПВЛ" || x.Code == "ВЖВЛ" || x.Code == "ВПВЛ") || !db.PlatCategory.Any(x => x.Code == "МС" || x.Code == "ВЖМС" || x.Code == "ВПМС")) // если нет данных за 2016 год, то синхронизируем справочник или если нет категорий По свободному порту Владивиосток (СПВЛ, ВЖВЛ, ВПВЛ)
            {
                PlatCategoryFrm child = new PlatCategoryFrm();
                child.synchBtn_Click(null, null);
                System.Threading.Thread.Sleep(4000);
            }


            if (!db.TariffCode.Any(x => x.Code == "25")) // если нет данных за 2016 год, то синхронизируем справочник
            {
                TariffCodeFrm child = new TariffCodeFrm();
                child.synchBtn_Click(null, null);
            }

            if (db.DocumentTypes.Any(x => x.ID == 0))
            {
                try
                {
                    db.Database.ExecuteSqlCommand(@"DELETE FROM DocumentTypes WHERE ID = 0;");
                }
                catch { }

            }

            /*            pu6Entities db = new pu6Entities();

                        if (!db.SpecOcenkaUslTrudaDopTariff.Any())
                        {
                            string fillSpecOcenkaUslTrudaDopTariff = @"INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (1,1,'2014-01-01','2014-12-31',8);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (1,1,'2014-01-01','2014-12-31',8);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (1,1,'2015-01-01','2018-12-31',8);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (1,0,'2014-01-01','2014-12-31',8);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (1,0,'2015-01-01','2018-12-31',8);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (2,1,'2014-01-01','2014-12-31',7);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (2,1,'2015-01-01','2018-12-21',7);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (2,0,'2014-01-01','2014-12-21',7);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (2,0,'2015-01-01','2018-12-31',7);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (3,1,'2014-01-01','2014-12-21',6);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (3,1,'2015-01-01','2018-12-31',6);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (3,0,'2014-01-01','2014-12-31',6);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (3,0,'2015-01-01','2018-12-31',6);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (4,1,'2014-01-01','2014-12-31',4);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (4,1,'2015-01-01','2018-12-31',4);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (4,0,'2014-01-01','2014-12-31',4);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (4,0,'2015-01-01','2018-12-31',4);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (5,1,'2014-01-01','2014-12-31',2);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (5,1,'2015-01-01','2018-12-31',2);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (5,0,'2014-01-01','2014-12-31',2);
                                INSERT INTO SpecOcenkaUslTrudaDopTariff (SpecOcenkaUslTrudaID, Type, DateBegin, DateEnd, Rate ) VALUES (5,0,'2015-01-01','2018-12-31',2);
                            ";
                            try
                            {
                                db.Database.ExecuteSqlCommand(fillSpecOcenkaUslTrudaDopTariff);
                            }
                            catch
                            {
                            }

                        }


                        string query = String.Empty;

                        if (!db.TerrUsl.Any(x => x.Code == "СЕЛО"))
                        {
                            query = @"INSERT INTO TerrUsl (Code, Name, DateBegin) VALUES ('СЕЛО','Работа в сельском хозяйстве','2016-01-01');";
                        }
                        if (!db.IschislStrahStajDop.Any(x => x.Code == "ДЕТИПРЛ"))
                        {
                            query = query + @" INSERT INTO IschislStrahStajDop (Code, Name, DateBegin) VALUES ('ДЕТИПРЛ','Отпуск по уходу за ребенком до 3-х лет, представляемый бабушке, дедушке, другим родственникам или опекунам','2015-01-01');";
                        }
                        if (!db.IschislStrahStajOsn.Any(x => x.Code == "ПОЛЕ"))
                        {
                            query = query + @" INSERT INTO IschislStrahStajOsn (Code, Name, DateBegin) VALUES ('ПОЛЕ','Работа в экспедициях, партиях, отрядах, на участках и в бригадах на полевых работах непосредственно в полевых условиях','2015-01-01');";
                        }

                        try
                        {
                            db.Database.ExecuteSqlCommand(query);
                        }
                        catch
                        {
                        }
            */
            try
            {
                Methods.setDBjurnal_mode(props.Fields.dbJournal_modeWAL, this.ThemeName, this);
            }
            catch
            {
            }
        }

        private void radMenuItem36_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ThemeName = this.ThemeName;
            aboutBox.ShowDialog(this);
        }

        private void radMenuItem37_Click(object sender, EventArgs e)
        {
            foreach (Form form in rd.MdiChildren)
            {
                form.Close();
            }
        }

        private void radMenuItem17_Click(object sender, EventArgs e)
        {
            Administration child = new Administration();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();

        }

        private void radMenuItem15_Click_1(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(Application.StartupPath, "pu6help.chm")))
                Help.ShowHelp(this, Path.Combine(Application.StartupPath, "pu6help.chm"));
            else
                RadMessageBox.Show("Файл справки не найден в каталоге с программой", "Внимание!");
        }

        private void radMenuItem40_Click(object sender, EventArgs e)
        {
            ImportXML child = new ImportXML();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();
        }

        private void radMenuItem41_Click(object sender, EventArgs e)
        {
            ImportMain child = new ImportMain();

            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();

        }

        private void radMenuItem38_Click(object sender, EventArgs e)
        {
            ExportDBF child = new ExportDBF();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();
        }


        private void radTileElement1_Click(object sender, EventArgs e)
        {
            radMenuItem60_Click(null, null);
        }

        private void radTileElement2_Click(object sender, EventArgs e)
        {
            radMenuItem69_Click(null, null);
        }

        private void radTileElement3_Click(object sender, EventArgs e)
        {
            radMenuItem51_Click(null, null);
        }

        private void radTileElement4_Click(object sender, EventArgs e)
        {
            radMenuItem40_Click(null, null);
        }

        private void radTileElement5_Click(object sender, EventArgs e)
        {
            radMenuItem41_Click(null, null);
        }

        private void radTileElement7_Click(object sender, EventArgs e)
        {
            radMenuItem38_Click(null, null);
        }

        private void radLiveTileElement1_Click(object sender, EventArgs e)
        {
            radMenuItem29_Click(null, null);
        }

        private void radTileElement8_Click(object sender, EventArgs e)
        {
            radMenuItem17_Click(null, null);
        }

        private void radTileElement10_Click(object sender, EventArgs e)
        {
            radMenuItem36_Click(null, null);
        }

        private void radTileElement6_Click(object sender, EventArgs e)
        {
            radMenuItem15_Click_1(null, null);
        }

        private void radTileElement9_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ThemeName = this.ThemeName;
            aboutBox.radButton1_Click(null, null);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            int topPadding = (radPanorama1.Height - 439) / 2 - 10;
            radPanorama1.Padding = new System.Windows.Forms.Padding(0, topPadding, 0, 0);
        }

        private void newVersionBtn_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(newVersionUrl);
            }
            catch
            { }

        }


        private void radMenuItem45_Click(object sender, EventArgs e)
        {
            RW3_2015_List child = new RW3_2015_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem42_Click(object sender, EventArgs e)
        {
            RSW2014_List child = new RSW2014_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem43_Click(object sender, EventArgs e)
        {
            SPW2_List child = new SPW2_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem44_Click(object sender, EventArgs e)
        {
            RSW2_2014_List child = new RSW2_2014_List();
            child.ThemeName = this.ThemeName;
            child.Year = 2015;
            ShowForm(child);
        }

        private void radMenuItem27_Click_1(object sender, EventArgs e)
        {
            PU.Dictionaries.TariffCodeRW3Frm child = new PU.Dictionaries.TariffCodeRW3Frm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();
        }

        private void checkXmlDB(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(500);
            string pfrXMLPath = Path.Combine(Application.StartupPath, "pfrXML.db3");
            string result = String.Empty;

            if (!File.Exists(pfrXMLPath))
            {
                result = methodsNonStatic.createXmlDB(pfrXMLPath);
            }
            else
            {
                FileInfo fi = new FileInfo(pfrXMLPath);
                if (fi.Length == 0)
                {
                    System.IO.File.Delete(pfrXMLPath);
                    fi = null;
                    result = methodsNonStatic.createXmlDB(pfrXMLPath);
                }
            }

            //xmlContainer

            pfrXMLPath = Utils.CorrectUNCpathToSqlite(pfrXMLPath);
            if (string.IsNullOrEmpty(result))
            {
                StringBuilder Sb = new StringBuilder();
                Sb.Append(@"metadata=res://*/Models.xmlContainer.csdl|res://*/Models.xmlContainer.ssdl|res://*/Models.xmlContainer.msl;provider=System.Data.SQLite;");
                Sb.Append(@"provider connection string='data source=" + pfrXMLPath + ";foreign keys=true;'");

                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                if ((config.ConnectionStrings.ConnectionStrings["pfrXMLEntities"] == null) || ((config.ConnectionStrings.ConnectionStrings["pfrXMLEntities"] != null) && (config.ConnectionStrings.ConnectionStrings["pfrXMLEntities"].ConnectionString != Sb.ToString())))
                {
                    if (config.ConnectionStrings.ConnectionStrings["pfrXMLEntities"] != null)
                    {
                        config.ConnectionStrings.ConnectionStrings["pfrXMLEntities"].ConnectionString = Sb.ToString();
                    }
                    else
                    {
                        config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings
                        {
                            Name = "pfrXMLEntities",
                            ConnectionString = Sb.ToString(),
                            ProviderName = "System.Data.EntityClient"
                        });
                    }

                    //saveConfig(config);
                    string res = Methods.saveConfigFile(config, "pu6Entities", false);
                    if (!String.IsNullOrEmpty(res))
                    {
                        this.Invoke(new Action(() => { Messenger.showAlert(AlertType.Info, "Внимание", res, this.ThemeName); }));
                    }
                }
            }
            else
            {
                this.Invoke(new Action(() => { Messenger.showAlert(AlertType.Info, "Внимание", "При создании базы данных: " + Path.GetFileName(pfrXMLPath) + " произошла ошибка.\r\nКод ошибки: " + result, this.ThemeName); }));
            }
        }

        private void saveConfig(Configuration config)
        {
            try
            {
                config.Save(ConfigurationSaveMode.Minimal);
            }
            catch
            {
                System.Threading.Thread.Sleep(300);
                saveConfig(config);
            }
        }

        private void radMenuItem47_Click(object sender, EventArgs e)
        {
            RSW2014_List child = new RSW2014_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem48_Click(object sender, EventArgs e)
        {
            SPW2_List child = new SPW2_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem49_Click(object sender, EventArgs e)
        {
            RSW2_2014_List child = new RSW2_2014_List();
            child.ThemeName = this.ThemeName;
            child.Year = 2016;
            ShowForm(child);
        }

        private void radMenuItem50_Click(object sender, EventArgs e)
        {
            RW3_2015_List child = new RW3_2015_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radTileElement11_Click(object sender, EventArgs e)
        {
            radMenuItem49_Click(null, null);
        }

        private void radTileElement12_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> FAQ = new Dictionary<string, string>();

            FAQ.Add("Можно ли в программе Документы ПУ-6 подключиться к базе данных Документы ПУ-5?", "Нет. Это разные программы с разными форматами баз данных.");
            FAQ.Add("Как перенести данные из Документы ПУ-5 в Документы ПУ-6?", @"Для переноса данных можно воспользоваться импортом\экспортом в форматах dbf или xml.<li><span style=""font-size: 12pt"">Возможности подключиться к базам Документы ПУ-5 нет. Автоматической выгрузки всех данных из Документы ПУ-5 тоже нет. </span></li><li><span style=""font-size: 12pt"">Документы ПУ-6 поддерживают импорт следующих XML файлов СЗВ-6-1, СЗВ-6-2, СЗВ-6-4, РСВ-1 с 2014 и файлы ПФР c 2014 (инд сведения), которые можно сформировать в программе Документы ПУ-5 в соответсвующих разделах.</span></li><li><span style=""font-size: 12pt"">Касательно DBF - поддерживается импорт индивидуальных сведений, данных о выплатах и стаже с 2010.");
            FAQ.Add("Есть ли возможность запускать программу Документы ПУ-6 с учетной записью пользователя, а не администратора?", "Программа запрашивает права Администратора если запускается из папки Program Files. Вы можете установить программу в любое другое место и тогда запуск будет с правами текущего пользователя. В ОС Windows XP запуск с правами текущего пользователя вне зависимости от места установки.");
            FAQ.Add("Как узнать о выходе новой версии?", @"В разделе О программе есть кнопка ""Проверить обновления"".");
            FAQ.Add("Возможна ли работа Документы ПУ-6 по сети в многопользовательском режиме?", @"Да, возможна. Доступны 2 варианта:<p>1. Устанавливаем с одного компьютера программу на сетевой диск и далее каждый запускает ее оттуда и работают все в одной базе, можно и под одним логином. Только в настройках надо отключить режим WAL (уменьшится скорость работы с базой), иначе будут ошибки доступа.</p><p>2. Устанавливаем также на сетевой диск, но базу располагаем на локальном диске, на все остальные компьютеры копируем эту базу по такому же локальному пути. И получится, что каждый будет работать со своей базой. Возможна работа в режиме WAL. Работать можно также с одним логином.</p>");
            FAQ.Add("В чем заполнять теперь документы с 1998-2010 год?", @"Формы за прошлые года придется  заполнять в Документы ПУ-5. Изменений в этих формах уже не будет.");
            FAQ.Add("Нужно ли устанавливать КЛАДР в Документы ПУ-6? Где его брать?", @"В программе Документы ПУ-6 КЛАДР не используется.");

            string faqText = "";

            foreach (var item in FAQ)
            {
                faqText += @"<p><span style=""font-size: 12pt""><strong>" + item.Key + @"</strong></span></p><br /><ul><li><span style=""font-size: 12pt"">" + item.Value + @"</span></li></ul><br /><br />";
            }

            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = this.ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(760, 550);
            child.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            child.ShowInTaskbar = false;
            child.Text = "Часто задаваемые вопросы по программе Документы ПУ-6";
            child.BackColor = Color.WhiteSmoke;
            System.Windows.Forms.Panel panel = new Panel();
            panel.Name = "Panel";
            panel.AutoScroll = true;
            panel.Location = new System.Drawing.Point(0, 0);
            panel.Dock = DockStyle.Fill;
            panel.TabIndex = 0;
            child.Controls.Add(panel);
            Telerik.WinControls.UI.RadLabel label = new Telerik.WinControls.UI.RadLabel();
            label.Name = "Label";
            label.Dock = DockStyle.Top;
            label.Height = 1100;
            label.AutoSize = false;
            label.EnableTheming = true;
            label.ThemeName = this.ThemeName;
            label.BackColor = Color.Transparent;
            label.Font = new Font("Times New Roman", 12);
            label.Text = @"<html>" + faqText + @"<span style=""font-size: 12pt""><em>Предложения по наполнению данного раздела принимаются на почту</em> </span><a href=""mailto:pu6developer@gmail.com""><span style=""font-size: 12pt"">pu6developer@gmail.com</span></a></p></html>";
            panel.Controls.Add(label);
            label.Select();
            child.ShowDialog();
        }

        private void radMenuItem51_Click(object sender, EventArgs e)
        {
            PU.FormsSZVM_2016.SZV_M_2016_List child = new PU.FormsSZVM_2016.SZV_M_2016_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem52_Click(object sender, EventArgs e)
        {
            PU.Reports.PrintEmptyForms_Frm child = new PU.Reports.PrintEmptyForms_Frm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();
        }

        private void radTileElement13_Click(object sender, EventArgs e) 
        {

        }

        private void radMenuItem54_Click(object sender, EventArgs e)
        {
            PU.FormsDSW3.DSW3_List child = new PU.FormsDSW3.DSW3_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem56_Click(object sender, EventArgs e)
        {
            PU.FormsADW1.FormsADW1_List child = new PU.FormsADW1.FormsADW1_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem58_Click(object sender, EventArgs e)
        {
            PU.FormsADW2.FormsADW2_List child = new PU.FormsADW2.FormsADW2_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem57_Click(object sender, EventArgs e)
        {
            PU.FormsADW3.FormsADW3_List child = new PU.FormsADW3.FormsADW3_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem62_Click(object sender, EventArgs e)
        {
            SPW2_List child = new SPW2_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }


        private void radMenuItem60_Click(object sender, EventArgs e)
        {
            PU.FormsODV1.ODV1_List child = new PU.FormsODV1.ODV1_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem63_Click(object sender, EventArgs e)
        {
            PU.FormsSZVM_2016.SZV_M_2016_List child = new PU.FormsSZVM_2016.SZV_M_2016_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);

        }

        private void radMenuItem64_Click(object sender, EventArgs e)
        {
            PU.ZAGS.Zags_Born.Born_List child = new PU.ZAGS.Zags_Born.Born_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem65_Click(object sender, EventArgs e)
        {
            PU.ZAGS.Zags_Death.Death_List child = new PU.ZAGS.Zags_Death.Death_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem67_Click(object sender, EventArgs e)
        {
            PU.FormsPredPens.PredPensZapros_List child = new PU.FormsPredPens.PredPensZapros_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }

        private void radMenuItem20_Click(object sender, EventArgs e)
        {

        }

        private void radMenuItem68_Click(object sender, EventArgs e)
        {
            PU.FormsPredPens.PredPensSpravka_ImportXML child = new PU.FormsPredPens.PredPensSpravka_ImportXML();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.MaximumSize = child.Size;
            child.ShowDialog();
            child.WindowState = FormWindowState.Normal;
            child.Dispose();

        }

        private void radMenuItem69_Click(object sender, EventArgs e)
        {
            PU.FormsSZV_TD.SZV_TD_List child = new PU.FormsSZV_TD.SZV_TD_List();
            child.ThemeName = this.ThemeName;
            ShowForm(child);
        }












    }
}
