using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PU.Classes;
using System.IO;
using System.Windows.Forms;
using SQLiteParser;

namespace PU.Models
{
    public class MethodsNonStatic
    {
        Props props = new Props(); //экземпляр класса с настройками
//        public List<string> themeNames = new List<string> { };

        public void readSetting()
        {
            props.ReadXml();

/*            if (themeNames.Contains(props.Fields.ThemeName))
                Options.ThemeName = props.Fields.ThemeName;
            else
                if (themeNames.Contains("Office2013Light"))
                {
                    Options.ThemeName = "Default";
                }
                else
                { 
                    Options.ThemeName = themeNames.Count() > 0 ? themeNames.First() : "";
                }
            */
            Options.InsID = props.Fields.InsurerID;
            Options.inputTypeRSW1 = props.Fields.inputTypeRSW1;
            Options.RKASV = props.Fields.RKASV;
            //Options.RKASV = new RKASV{};  // для страхователей

            if (props.Fields.DBList.Count() == 1)  // если база данных всего одна в списке берем ее
            {
                Options.DBActual.name = props.Fields.DBList[0].name;
                Options.DBActual.path = props.Fields.DBList[0].path;
                props.Fields.DBList[0].actual = true;
                props.WriteXml();
            }
            else if (props.Fields.DBList.Count() <= 0)  // если список баз данных пустой, то создаем новую копированием из эталонной базы и сохраняем список с ней в настройки
            {
                var EmptyDatabase = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");

                var dbpath = Path.Combine(Application.StartupPath, "Database\\pu6.db3");

                int i = 0;
                while (File.Exists(dbpath))
                {
                    i++;
                    dbpath = Path.Combine(Application.StartupPath, "Database\\pu6_" + i.ToString() + ".db3");
                }

                if (File.Exists(EmptyDatabase))
                {
                    try
                    {
                        File.Copy(EmptyDatabase, dbpath);

                        Options.DBActual.name = "Текущая база данных " + i.ToString();
                        Options.DBActual.path = dbpath;

                        props.Fields.DBList.Add(new DBEntry { name = Options.DBActual.name, path = Options.DBActual.path, actual = true });
                        props.WriteXml();
                    }
                    catch
                    { }


                }
            }
            else // если в списке несколько баз данных, то находим первую у которой будет флаг текущей
            {
                if (props.Fields.DBList.Any(x => x.actual == true))
                {
                    Options.DBActual.name = props.Fields.DBList.First(x => x.actual == true).name;
                    Options.DBActual.path = props.Fields.DBList.First(x => x.actual == true).path;

                }
                else
                {
                    Options.DBActual.name = props.Fields.DBList.First().name;
                    Options.DBActual.path = props.Fields.DBList.First().path;
                    props.Fields.DBList.First().actual = true;
                    props.WriteXml();
                }

            }

        }

        //Запись настроек
        public void writeSetting()
        {
            props.ReadXml();
            props.Fields.ThemeName = Options.ThemeName;
            props.Fields.InsurerID = Options.InsID;
            props.Fields.inputTypeRSW1 = Options.inputTypeRSW1;
//            props.Fields.useRSW1_2015 = Options.useRSW1_2015;
            props.Fields.RKASV = Options.RKASV;
            props.Fields.formParams = Options.formParams;
            props.Fields.gridParams = Options.gridParams;
            props.Fields.InsurerFolders = Options.InsurerFolders;
            props.Fields.hideDialogCheckFiles = Options.hideDialogCheckFiles;
            props.Fields.checkFilesAfterSaving = Options.checkFilesAfterSaving;

            props.WriteXml();
        }



        CompareSqlite compareSqlite = new CompareSqlite();
        private CompareParams _compareParams = null;
        private string _ThemeName;
        private string _leftdb;
        private string _rightdb;
        private Dictionary<SchemaObject, List<SchemaComparisonItem>> _results;
        private Dictionary<SchemaObject, Dictionary<string, SQLiteDdlStatement>> _leftSchema;
        private Dictionary<SchemaObject, Dictionary<string, SQLiteDdlStatement>> _rightSchema;
        private IWorker _worker = null;
        private CompareWorker worker = null;
        private int ver_bd_emp = 0; // версия эталонной БД
        private Form _parent;


        public void updateDBFromDB_Empty(string dbpath, string dbpath_emp, string ThemeName, Form Parent)
        {
            _ThemeName = ThemeName;
            _parent = Parent;

            bool ver_check_bd = false;// соответствие версии программы и рабочей БД
            bool ver_check_bd_emp = false; // соответствие версии программы и эталонной БД
            bool exist_bd_emp = false; // существование эталонной бд
            int ver_bd = 0; // версия рабочей БД
            int ver_app = 0; // версия приложения

            // Сраниваем структуру текущей БД с эталонной и если надо обновляем

            DBVer dbver = Methods.checkDBVersion(dbpath, false);
            ver_check_bd = dbver.checkResult;
            ver_bd = dbver.db_ver;
            ver_app = dbver.app_ver;


            exist_bd_emp = File.Exists(dbpath_emp);

            if (!exist_bd_emp) // если эталонная БД не существует
            {
                Messenger.showAlert(AlertType.Info, "Внимание", "Эталонная база не найдена по пути - " + dbpath_emp + "\r\nНеобходимо переустановить приложение, либо скопировать базу вручную по указанному пути!", _ThemeName);
            }
            else
            {
                dbver = Methods.checkDBVersion(dbpath_emp, true);
                ver_check_bd_emp = dbver.checkResult;
                ver_bd_emp = dbver.db_ver;
                if (!ver_check_bd_emp) // если эталонная бд существует, но ее версия не соответствует версии приложения
                {
                    Messenger.showAlert(AlertType.Info, "Внимание", "Версия эталонной базы (" + ver_bd_emp.ToString() + ") не соответствует версии указанной в приложении (" + ver_app.ToString() + ")", _ThemeName);
                }
            }

            if (!ver_check_bd) // если версия рабочей БД не соответсвует версии в приложении
            {
                if (!exist_bd_emp) // если эталонная БД не существует
                {
                    Messenger.showAlert(AlertType.Info, "Внимание", "Версия рабочей базы (" + ver_bd.ToString() + ") не соответствует версии указанной в приложении (" + ver_app.ToString() + ")\r\nОбновление базы невозможно, т.к. эталонная база не найдена по пути - " + dbpath_emp + "\r\nНеобходимо переустановить приложение, либо скопировать базу вручную по указанному пути!", _ThemeName);
                }
                else //если эталонная БД найдена
                {
                    if (ver_bd < ver_bd_emp)  // Если версия рабочей БД меньше версии эталонной БД
                    {
                        _compareParams = compareSqlite.openAndCheckBases(dbpath_emp, dbpath);// Обновляем структуру рабочей БД
                        RefreshComparison(false);
                        
                    }
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
                //_result = _worker.Result;

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

                    try
                    {

                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + _rightdb, true))
                        {
                            using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                            {
                                con.Open();                             // Open the connection to the database
                                com.CommandText = sql;     // Set CommandText to our query that will create the table
                                com.ExecuteNonQuery();
                                con.Close();        // Close the connection to the database

                            }
                        }

                        Methods.setDBVersion(_rightdb, ver_bd_emp);

                    }
                    catch { }

            //        pu6Entities db = new pu6Entities();
            //        db.Database.ExecuteSqlCommand(sql);

                }
                else if (_parent != null)
                {
                     _parent.Invoke(new Action(() => { Messenger.showAlert(AlertType.Error, "Внимание", "При обновлении структуры рабочей БД произошла ошибка!\r\nРекомендуется создать новую базу данных в Менеджере БД!", _ThemeName); }));

                }
            }
        }

        /// <summary>
        /// Обновление поля Начислено страховых взносов на ОПС
        /// </summary>
        public decimal UpdateSumFeePFR(bool manual, List<FormsRSW2014_1_Razd_6_4> RSW64List, short y)
        {
            decimal sum = 0;
            if ((RSW64List != null) && (RSW64List.Count != 0))
            {
                foreach (var item in RSW64List)
                {
                    if ((item.PlatCategory.TariffPlat.Any(x => x.Year == y)) && (item.PlatCategory.TariffPlat.First(x => x.Year == y).StrahPercant1966 != null))
                    {
                        decimal s1 = item.s_1_1 != null ? item.s_1_1.Value : 0;
                        decimal s2 = item.s_2_1 != null ? item.s_2_1.Value : 0;
                        decimal s3 = item.s_3_1 != null ? item.s_3_1.Value : 0;

                        decimal sumAll = s1 + s2 + s3;

                        if (Options.mrot != null)
                        {
                            sumAll = sumAll <= Options.mrot.NalogBase ? sumAll : Options.mrot.NalogBase;
                        }

                        sum = sum + (sumAll * item.PlatCategory.TariffPlat.First(x => x.Year == y).StrahPercant1966.Value / 100);
                    }
                    else
                    {
                        if (manual == true)
                            MessageBox.Show("Категория плательщика: " + item.PlatCategory.Code + "\r\nНе определен тариф Страховых взносов\r\nРасчет взносов по этой категории производиться не будет!");
                    }
                }
            }
            else
            {
                sum = (decimal)0;
            }

            return Math.Round(sum, 2, MidpointRounding.AwayFromZero);
        }


        public string createXmlDB(string pfrXMLPath)
        {
            string result = String.Empty;
            try
            {
                // This is the query which will create a new table in our database file with three columns. An auto increment column called "ID", and two NVARCHAR type columns with the names "Key" and "Value"
                string createTableQuery = @"CREATE TABLE [xmlInfo] (
  [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  [Num] INTEGER, 
  [CountDoc] INTEGER, 
  [CountStaff] INTEGER, 
  [DocType] VARCHAR, 
  [Year] SMALLINT, 
  [Quarter] TINYINT, 
  [YearKorr] SMALLINT, 
  [QuarterKorr] TINYINT, 
  [UserName] VARCHAR, 
  [DateCreate] DATETIME, 
  [FileName] VARCHAR, 
  [ParentID] INTEGER CONSTRAINT [fk_xmlInfo_Parent] REFERENCES [xmlInfo]([ID]), 
  [SourceID] INTEGER, 
  [UniqGUID] GUID, 
  [InsurerID] INTEGER, 
  [FormatType] VARCHAR);


CREATE TABLE [rsw2014] (
  [ID] INTEGER PRIMARY KEY AUTOINCREMENT, 
  [xmlInfo_ID] INTEGER CONSTRAINT [fk_rsw2014_xmlInfo] REFERENCES [xmlInfo]([ID]) ON DELETE CASCADE ON UPDATE CASCADE, 
  [RSW_2_5_1_2] DECIMAL(15, 2), 
  [RSW_2_5_1_3] DECIMAL(15, 2), 
  [RSW_2_5_2_4] DECIMAL(15, 2), 
  [RSW_2_5_2_5] DECIMAL(15, 2), 
  [RSW_2_5_2_6] DECIMAL(15, 2));


CREATE TABLE [StaffList] (
  [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  [Num] INTEGER, 
  [FIO] VARCHAR, 
  [InsuranceNum] VARCHAR, 
  [InfoType] VARCHAR, 
  [DateCreate] DATE, 
  [XmlInfoID] INTEGER CONSTRAINT [fk_StaffList_xmlInfo] REFERENCES [xmlInfo]([ID]) ON DELETE CASCADE ON UPDATE CASCADE, 
  [StaffID] INTEGER, 
  [FormsRSW_6_1_ID] INTEGER);


CREATE TABLE [xmlFile] (
  [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, 
  [XmlContent] VARCHAR, 
  [XmlInfoID] INTEGER CONSTRAINT [fk_xmlFile_xmlInfo] REFERENCES [xmlInfo]([ID]) ON DELETE CASCADE ON UPDATE CASCADE);



                                        ";
                System.Data.SQLite.SQLiteConnection.CreateFile(pfrXMLPath);        // Create the file which will be hosting our database
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + pfrXMLPath, true))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database

                        com.CommandText = createTableQuery;     // Set CommandText to our query that will create the table
                        com.ExecuteNonQuery();                  // Execute the query
                        con.Close();        // Close the connection to the database
                    }
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }


            return result;
        }
    }
}
