using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using PU.Classes;
using PU.Models;
using Telerik.WinControls.UI;
using System.IO;
using System.Configuration;
using SQLiteParser;
using PU.UserAccess;
using System.Reflection;
using System.Data.Entity.Infrastructure;
//using System.Data.Metadata.Edm;
//using System.Data.Entity.Core.Metadata.Edm;

namespace PU
{
    public partial class BaseManager : Telerik.WinControls.UI.RadForm
    {
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
        CompareSqlite compareSqlite = new CompareSqlite();

        Props props = new Props(); //экземпляр класса с настройками
        public List<DBEntry> dblist { get; set; }

        private CompareParams _compareParams = null;
        private string _leftdb;
        private string _rightdb;
        private Dictionary<SchemaObject, List<SchemaComparisonItem>> _results;
        private Dictionary<SchemaObject, Dictionary<string, SQLiteDdlStatement>> _leftSchema;
        private Dictionary<SchemaObject, Dictionary<string, SQLiteDdlStatement>> _rightSchema;
        private IWorker _worker = null;
        private CompareWorker worker = null;


        public BaseManager()
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

        private void BaseManager_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            /*          if (!Methods.checkBaseEmpVersion())
                        {
                            RadMessageBox.Show(this, "Версия эталонной базы не соответсвует сборке приложения");
                        }
               * */
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Reflection.AssemblyName assemblyName = assembly.GetName();
            Version version = assemblyName.Version;
            appVerLabel.Text = version.Revision.ToString();


            updateDBList();
        }

        private void updateDBList()
        {
            props.ReadXml();
            dblist = props.Fields.DBList;

            DBList.Items.Clear();

            foreach (var item in dblist)
            {
                RadListDataItem dbItem = new RadListDataItem(item.name, item.path.ToString());
                if (item.actual)
                    dbItem.ForeColor = Color.BlueViolet;
                if (!File.Exists(item.path))
                    dbItem.ForeColor = Color.IndianRed;


                DBList.Items.Add(dbItem);

            }
            if (dblist.Count > 0)
            {
                if (DBList.Items.Any(x => x.Value.ToString() == Options.DBActual.path && x.Text == Options.DBActual.name))
                    DBList.Items.First(x => x.Value.ToString() == Options.DBActual.path && x.Text == Options.DBActual.name).Selected = true;
                else
                    DBList.Items.First().Selected = true;
                dbPathPanel.Text = DBList.SelectedItem.Value.ToString();
            }
        }

        private void addBDBtn_Click(object sender, EventArgs e)
        {
            BaseManagerEdit child = new BaseManagerEdit();
            child.ThemeName = this.ThemeName;
            child.Owner = this;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.ShowDialog();
            if (child.dbItem != null)
            {
                dblist.Add(new DBEntry { name = child.dbItem.name, path = child.dbItem.path, pathBackup = child.dbItem.pathBackup });
                props.Fields.DBList = dblist;
                props.WriteXml();
                updateDBList();

            }
        }

        private void editBDBtn_Click(object sender, EventArgs e)
        {
            if (DBList.Items.Count() > 0)
            {
                int ind = DBList.SelectedIndex;
                BaseManagerEdit child = new BaseManagerEdit();
                child.ThemeName = this.ThemeName;
                child.Owner = this;
                child.ShowInTaskbar = false;
                if (DBList.Items.Count > 0)
                    child.dbItem = new DBEntry { name = DBList.SelectedItem.Text.ToString(), path = DBList.SelectedItem.Value.ToString() };
                child.action = "edit";
                child.ShowDialog();
                if (child.dbItem != null)
                {
                    string pathBackup = !String.IsNullOrEmpty(dblist.ElementAt(ind).pathBackup) ? dblist.ElementAt(ind).pathBackup : "";
                    dblist.RemoveAt(ind);
                    dblist.Insert(ind, new DBEntry { name = child.dbItem.name, path = child.dbItem.path, pathBackup = pathBackup });
                    props.Fields.DBList = dblist;
                    props.WriteXml();

                    updateDBList();

                }
            }

        }

        private void DBList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (DBList.Items.Count() > 0)
            {

                dbPathLabel.Text = DBList.SelectedItem.Value.ToString();

                if (File.Exists(DBList.SelectedItem.Value.ToString()))
                {

                    string query = @"PRAGMA user_version;";


                    try
                    {
                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + DBList.SelectedItem.Value.ToString(), true))
                        {
                            using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                            {
                                con.Open();                             // Open the connection to the database
                                com.CommandText = query;     // Set CommandText to our query that will create the table
                                int baseEmpVer = 0;
                                int.TryParse(com.ExecuteScalar().ToString(), out baseEmpVer);

                                dbVerLabel.Text = baseEmpVer.ToString();

                                con.Close();        // Close the connection to the database
                            }
                        }
                    }
                    catch
                    {
                        dbVerLabel.Text = "неизв.";
                    }
                }
                else
                {
                    dbVerLabel.Text = "неизв.";
                }

            }

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectBDBtn_Click(object sender, EventArgs e)
        {
            if (DBList.Items.Count() > 0)
            {
                string dbpath = DBList.SelectedItem.Value.ToString();
                if (File.Exists(DBList.SelectedItem.Value.ToString()))
                {
                    FileInfo fi = new FileInfo(dbpath);
                    if (fi.Length != 0)
                    {
                        if (File.Exists(dbpath))
                        {
                            foreach (var item in dblist)
                            {
                                item.actual = false;
                            }
                            dblist[DBList.SelectedIndex].actual = true;
                            Options.DBActual = dblist[DBList.SelectedIndex];

                            MainForm main = this.Owner as MainForm;
                            if (main != null)
                            {
                                foreach (Form form in main.rd.MdiChildren)
                                {
                                    form.Close();
                                }

                                dbpath = PU.Classes.Utils.CorrectUNCpathToSqlite(dbpath);
                                StringBuilder Sb = new StringBuilder();
                                Sb.Append(@"metadata=res://*/Models.Model.csdl|res://*/Models.Model.ssdl|res://*/Models.Model.msl;provider=System.Data.SQLite;");
                                Sb.Append(@"provider connection string='data source=" + dbpath + ";'");

                                //              db.Connection.ConnectionString = Sb.ToString();

                                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
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
                                    this.Invoke(new Action(() => { Methods.showAlert("Внимание", result, this.ThemeName); }));
                                }
                                //config.Save(ConfigurationSaveMode.Minimal);
                                //PU.Properties.Settings.Default.Reload();
                                //ConfigurationManager.RefreshSection("connectionStrings");



                                // Сраниваем структуру текущей БД с эталонной и если надо обновляем
                                string dbpath_emp = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");
                                methodsNonStatic.updateDBFromDB_Empty(dbpath, dbpath_emp, this.ThemeName, this.Owner); // Метод проверки версии БД и обновление структуры

                                pu6Entities db = new pu6Entities();

                                if (!db.Insurer.Any(x => x.ID == Options.InsID))
                                {
                                    Options.InsID = 0;
                                }

                                props.Fields.DBList = dblist;
                                props.WriteXml();
                                updateDBList();
                                xaccessEntities xaccessdb = new xaccessEntities();
                                UserAccess.Users user = xaccessdb.Users.FirstOrDefault(x => x.ID == Options.User.ID);
                                Methods.checkAndSetActiveUser(user);

                                main.Text = "Документы ПУ-6  -  [ " + Options.DBActual.name + " ]";

                            }
                            closeBtn_Click(null, null);
                        }
                        else
                        {
                            RadMessageBox.Show(this, "Нельзя выбрать данную базу данных как рабочую.\r\nФайл базы данных не найден на диске!", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                        }
                    }
                    else
                    {
                        RadMessageBox.Show(this, "Нельзя выбрать данную базу данных как рабочую.\r\nНеопознана структура файла базы данных!", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    }
                }
                else
                {
                    RadMessageBox.Show(this, "Нельзя выбрать данную базу данных как рабочую.\r\nФайл базы данных не найден на диске!", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// Allows to refresh the comparison results
        /// </summary>
        /// <param name="cancellable">TRUE means that the user will be allowed to cancel
        /// the comparison process. FALSE prevents this from happening (by making the CANCEL
        /// button disabled).</param>
        private void RefreshComparison(bool cancellable)
        {
            worker = new CompareWorker(_compareParams);
            _worker = (IWorker)worker;

            _worker.ProgressChanged += new EventHandler<ProgressEventArgs>(_worker_ProgressChanged);
            _worker.BeginWork();



        }

        private void _worker_ProgressChanged(object sender, ProgressEventArgs e)
        {
            if (e.IsDone)
            {
                _worker.ProgressChanged -= new EventHandler<ProgressEventArgs>(_worker_ProgressChanged);

                Dictionary<SchemaObject, List<SchemaComparisonItem>> results =
                    (Dictionary<SchemaObject, List<SchemaComparisonItem>>)_worker.Result;
                if (results != null)
                {

                    _leftSchema = worker.LeftSchema;
                    _rightSchema = worker.RightSchema;
                    _results = results;
                    _leftdb = _compareParams.LeftDbPath;
                    _rightdb = _compareParams.RightDbPath;

                    string sql = ChangeScriptBuilder.Generate(_leftdb, _rightdb, _leftSchema, _rightSchema, _results, ChangeDirection.RightToLeft);

                    pu6Entities db = new pu6Entities();

                    db.Database.ExecuteSqlCommand(sql);

                    if (!db.Insurer.Any(x => x.ID == Options.InsID))
                    {
                        Options.InsID = 0;
                    }

                    this.Invoke(new Action(() => { closeBtn_Click(null, null); }));

                    //                    MessageBox.Show(sql);
                }
                else // не удалось сравнить структуру баз.
                {

                }



            }
        }


        private void delFromListBtn_Click(object sender, EventArgs e)
        {
            if (DBList.Items.Count() > 0)
            {
                if (DBList.Items.Count() == 1)  // если база всего одна, то нельзя ее удалять
                {
                    RadMessageBox.Show(this, "Нельзя удалить единственную базу данных", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
                else
                {
                    int ind = DBList.SelectedIndex;
                    dblist.RemoveAt(ind);
                    //                    dblist[0].actual = true;
                    props.Fields.DBList = dblist;
                    props.WriteXml();
                    updateDBList();

                }
            }
        }

        private void delFromDiskBtn_Click(object sender, EventArgs e)
        {
            if (DBList.Items.Count() > 0)
            {
                if (DBList.Items.Count() == 1 && File.Exists(DBList.SelectedItem.Value.ToString()))  // если база всего одна, то нельзя ее удалять
                {
                    RadMessageBox.Show(this, "Нельзя удалить единственную базу данных", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
                else
                {
                    var filePath = DBList.SelectedItem.Value.ToString();
                    if (File.Exists(filePath))
                    {
                        if (RadMessageBox.Show(
                                "Вы уверены в том, что желаете удалить выбранную базу данных?\r\nФайл будет удален с диска!", "Удаление базы данных с диска навсегда!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation
                            ) == DialogResult.Yes)
                        {
                            try
                            {
                                File.Delete(filePath);

                                int ind = DBList.SelectedIndex;
                                dblist.RemoveAt(ind);
                                dblist[0].actual = true;
                                props.Fields.DBList = dblist;
                                props.WriteXml();
                                updateDBList();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show(this, "Ошибка при удалении файла базы данных с диска.\r\n" + ex.Message, "Внимание", MessageBoxButtons.OK, RadMessageIcon.Error);
                                return;
                            }
                        }



                    }


                }
            }
        }

        private void DBList_DoubleClick(object sender, EventArgs e)
        {
            selectBDBtn_Click(null, null);
        }

        private void updateDictBtn_Click(object sender, EventArgs e)
        {
            UpdateDict child = new UpdateDict();
            child.ThemeName = this.ThemeName;
            child.Owner = this;
            child.ShowDialog();

            /*            string[] tableList = new string[] { "PlatCategory", "TerrUsl", "OsobUslTruda", "KodVred_1", "KodVred_2", "KodVred_3", "IschislStrahStajOsn", "IschislStrahStajDop", "UslDosrNazn", "VidTrudDeyat", "SpecOcenkaUslTruda", "SpecOcenkaUslTrudaDopTariff", "DocumentTypes"};

                        foreach (var table in tableList)
                        {
                            var t = UpdateDictionaries.updateTable(table, Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3"));
                        }
             * */
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            BackupRestoreDB child = new BackupRestoreDB();
            child.ThemeName = this.ThemeName;
            child.Owner = this;
            child.ShowDialog();

            if (child.restoreFlag) // если был факт восстановления базы
            {
                DBList.Items[dblist.IndexOf(dblist.First(x => x.actual))].Selected = true;
                selectBDBtn_Click(null, null);
            }
            else
                updateDBList();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;


            pu6Entities db = new pu6Entities();

            // Удаление данных оставшихся после удаления Страхователя
            List<string> tables = new List<string> { "FormsRSW2014_1_Razd_6_1", "FormsDSW_3", "FormsRSW2014_2_1", "FormsRW3_2015", "FormsSPW2", "FormsSZV_6", "FormsSZV_6_4", "FormsSZV_M_2016", "FormsODV_1_2017", "FormsSZV_KORR_2017", "FormsSZV_STAJ_2017", "FormsSZV_ISH_2017", "FormsPredPens_Zapros", "FormsSZV_TD_2020", "Staff" };

            foreach (var item in tables)
            {
                try
                {
                    db.Database.ExecuteSqlCommand(String.Format("DELETE FROM " + item + " WHERE (InsurerID NOT IN (SELECT ID FROM Insurer))"));
                }
                catch (Exception ex)
                {
                    Methods.showAlert("Внимание!", "Во время очистки таблицы " + item + " произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
                }

            }

            // Удаление данных оставшихся после удаления ОДВ-1
            tables = new List<string> { "FormsSZV_ISH_2017", "FormsSZV_KORR_2017", "FormsSZV_STAJ_2017" };

            foreach (var item in tables)
            {
                try
                {
                    db.Database.ExecuteSqlCommand(String.Format("DELETE FROM " + item + " WHERE (FormsODV_1_2017_ID NOT IN (SELECT ID FROM FormsODV_1_2017))"));
                }
                catch (Exception ex)
                {
                    Methods.showAlert("Внимание!", "Во время очистки таблицы " + item + " произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
                }
            }

            // Удаление Инд. сведений у кого не заполнен TypeInfoID
            tables = new List<string> { "FormsRSW2014_1_Razd_6_1", "FormsSPW2", "FormsSZV_6", "FormsSZV_6_4" };

            foreach (var item in tables)
            {
                try
                {
                    db.Database.ExecuteSqlCommand(String.Format("DELETE FROM " + item + " WHERE ([TypeInfoID] IS NULL)"));
                }
                catch (Exception ex)
                {
                    Methods.showAlert("Внимание!", "Во время обслуживания таблицы " + item + " произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
                }

            }




            /*try
            {
                tables = new List<string> { 
                "FormsDSW_3", 
                "FormsRSW2014_1_1",
                "FormsRSW2014_1_Razd_2_1",
                "FormsRSW2014_1_Razd_2_4",
                "FormsRSW2014_1_Razd_2_5_1",
                "FormsRSW2014_1_Razd_2_5_2",
                "FormsRSW2014_1_Razd_3_4",
                "FormsRSW2014_1_Razd_4",
                "FormsRSW2014_1_Razd_5",
                "FormsRSW2014_1_Razd_6_1",
                "FormsRSW2014_1_Razd_6_4",
                "FormsRSW2014_1_Razd_6_6",
                "FormsRSW2014_1_Razd_6_7",
                "FormsRSW2014_2_1",
                "FormsRSW2014_2_2",
                "FormsRSW2014_2_3",
                "FormsRW3_2015",
                "FormsRW3_2015_Razd_3",
                "FormsSZV_6",
                "FormsSZV_6_4"
            };

                //var l = db.MetadataWorkspace.GetItems<EntityType>(DataSpace.CSpace).Where(x => tables.Contains(x.Name));
                var l = ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace.GetItemCollection(DataSpace.SSpace).GetItems<EntityType>().Where(x => tables.Contains(x.Name));
                foreach (var item in l)
                {
                    var list = item.Properties.Where(x => x.TypeUsage.EdmType.FullName.ToLower().Contains("decimal")).ToArray();

                    foreach (var itemName_ in list)
                    {
                        db.Database.ExecuteSqlCommand("Update " + item.Name + " set " + itemName_ + " = round(" + itemName_ + ", 2);");
                    }

                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Во время обслуживания базы произошла ошибка. Код ошибки: " + ex.Message);
            }*/

            try
            {
                db.Database.ExecuteSqlCommand("VACUUM;");
            }
            catch (Exception ex)
            {
                Methods.showAlert("Внимание!", "Во время операции VACUUM произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
            }
            try
            {
                db.Database.ExecuteSqlCommand("REINDEX;");
            }
            catch (Exception ex)
            {
                Methods.showAlert("Внимание!", "Во время операции REINDEX произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }






        }

        private void delFromListBadItemsBtn_Click(object sender, EventArgs e)
        {
            if (DBList.Items.Count() > 0)
            {
                if (DBList.Items.Count() == 1)  // если база всего одна, то нельзя ее удалять
                {
                    RadMessageBox.Show(this, "Нельзя удалить единственную базу данных", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
                else
                {
                    List<int> list = new List<int>();
                    for (int i = 0; i < DBList.Items.Count(); i++)
                    {
                        if (!File.Exists(dblist[i].path))
                        {
                            list.Add(i);
                        }
                    }

                    list.Reverse(0, list.Count);

                    foreach (int y in list)
                    {
                        dblist.RemoveAt(y);
                    }

                    props.Fields.DBList = dblist;
                    props.WriteXml();
                    updateDBList();


                }
            }
        }




        //logoPictureBox

    }
}
