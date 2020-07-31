using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PU.Classes;
using System.Xml.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Data;
using System.ComponentModel;
using System.Net;
using System.Reflection;
using System.Configuration;

namespace PU.Models
{
    public static class Methods
    {
        public static string saveConfigFile(Configuration config, string connectionName, bool reload)
        {
            string result = "";
            string pathOld = config.FilePath;

            string pathNew = config.FilePath + ".tmp";

            try
            {
                //Сохранем под другим именем

                config.SaveAs(pathNew, ConfigurationSaveMode.Minimal, true);  //- Сохранили

                //Удаляем настоящие настройки

                System.IO.File.Delete(pathOld);

                //Переименовываем

                System.IO.File.Move(pathNew, pathOld);



                //Открываем заново

                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                //Проверяем, что там есть новые настройки

                if (config.ConnectionStrings.ConnectionStrings[connectionName] != null)
                {
                    //                MessageBox.Show(config.ConnectionStrings.ConnectionStrings[connectionName].ConnectionString);
                    if (reload)
                    {
                        PU.Properties.Settings.Default.Reload();
                        ConfigurationManager.RefreshSection("connectionStrings");
                    }
                }
                else
                {
                    result ="Ошибка при сохранении конфигурационного файла! Не найдена строка подключения!";
                }
            }
            catch (Exception ex)
            {
                result = "Ошибка при обновлении конфигурационного файла! Код ошибки: " + ex.Message;
            }

            return result;
        }


        public static DBVer checkDBVersion(string dbpath, bool emptyBase)
        {
            DBVer result = new DBVer { app_ver = 0, db_ver = 0, checkResult = false };

            string query = string.Empty;
            if (emptyBase)
                query = @"PRAGMA schema_version;";
            else
                query = @"PRAGMA user_version;";

            try
            {
                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + dbpath, true))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = query;     // Set CommandText to our query that will create the table
                        int baseEmpVer = 0;
                        int.TryParse(com.ExecuteScalar().ToString(), out baseEmpVer);

                        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                        System.Reflection.AssemblyName assemblyName = assembly.GetName();
                        Version version = assemblyName.Version;
                        result.app_ver = version.Revision;
                        result.db_ver = baseEmpVer;
                        result.checkResult = version.Revision == baseEmpVer;  // сравнение версии в программе и эталонной базы

                        con.Close();        // Close the connection to the database
                    }
                }
            }
            catch { }

            return result;
        }
        public static void setDBjurnal_mode(bool setWal, string ThemeName, Form _parent)
        {
            try
            {
                pu6Entities db = new pu6Entities();

                if (setWal)
                    db.ExecuteStoreCommand(@"PRAGMA journal_mode = 'WAL';PRAGMA cache_size = 20000;PRAGMA page_size = 4096;");
                else
                    db.ExecuteStoreCommand(@"PRAGMA journal_mode = 'delete';PRAGMA cache_size = 20000;PRAGMA page_size = 4096;");
            }
            catch (Exception ex)
            {
                if (_parent != null)
                    _parent.Invoke(new Action(() => { Methods.showAlert("Внимание", "При обновлении режима работы базы данных произошла ошибка!\r\nКод ошибки: " + ex.Message, ThemeName); }));
            }
        }


        public static bool setDBVersion(string dbpath, int ver)
        {
            bool result = false;

            string query = @"PRAGMA user_version=" + ver.ToString() + ";";

            try
            {

                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + dbpath, true))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = query;     // Set CommandText to our query that will create the table
                        com.ExecuteNonQuery();

                        con.Close();        // Close the connection to the database
                        result = true;

                    }
                }
            }
            catch { }

            return result;
        }


        /// <summary>
        /// запись версии эталонной базы в Revision версию программы
        /// </summary>
        public static void setBaseEmpVersionToAppVersion()
        {
            string AssemblyInfoPath = @"..\..\" + @"\Properties\AssemblyInfo.cs";
            if (File.Exists(AssemblyInfoPath))
            {
                string dbpath_emp = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");

                string query = @"PRAGMA schema_version;";

                using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + dbpath_emp, true))
                {
                    using (System.Data.SQLite.SQLiteCommand com = new System.Data.SQLite.SQLiteCommand(con))
                    {
                        con.Open();                             // Open the connection to the database
                        com.CommandText = query;     // Set CommandText to our query that will create the table

                        int baseEmpVer = 0;
                        int.TryParse(com.ExecuteScalar().ToString(), out baseEmpVer);


                        string text = File.ReadAllText(AssemblyInfoPath);

                        Match match = new Regex("AssemblyVersion\\(\"(.*?)\"\\)").Match(text);
                        Version ver = new Version(match.Groups[1].Value);
                        Version newVer = new Version(ver.Major, ver.Minor, ver.Build, baseEmpVer);

                        text = Regex.Replace(text, @"AssemblyVersion\((.*?)\)", "AssemblyVersion(\"" + newVer.ToString() + "\")");
                        text = Regex.Replace(text, @"AssemblyFileVersionAttribute\((.*?)\)", "AssemblyFileVersionAttribute(\"" + newVer.ToString() + "\")");
                        text = Regex.Replace(text, @"AssemblyFileVersion\((.*?)\)", "AssemblyFileVersion(\"" + newVer.ToString() + "\")");

                        File.WriteAllText(@"..\..\" + @"\Properties\AssemblyInfo.cs", text);


                        con.Close();        // Close the connection to the database
                    }
                }
            }
        }

        /// <summary>
        /// Расширение для LINQ для динамической сортировки по названию поля
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="q"></param>
        /// <param name="SortField"></param>
        /// <param name="Ascending"></param>
        /// <returns></returns>
        /*       public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, bool Ascending)
               {
                   var param = Expression.Parameter(typeof(T), "p");
                   var prop = Expression.Property(param, SortField);
                   var exp = Expression.Lambda(prop, param);
                   string method = Ascending ? "OrderBy" : "OrderByDescending";
                   Type[] types = new Type[] { q.ElementType, exp.Body.Type };
                   var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
                   return q.Provider.CreateQuery<T>(mce);
               }
       */
        /// <summary>
        /// Присвоение указанного NameSpace для все элементов XML
        /// </summary>
        /// <param name="xelem"></param>
        /// <param name="xmlns">необходимый NameSpace</param>
        public static void SetDefaultXmlNamespace(this XElement xelem, XNamespace xmlns)
        {
            if (xelem.Name.NamespaceName == string.Empty)
                xelem.Name = xmlns + xelem.Name.LocalName;
            foreach (var e in xelem.Elements())
                e.SetDefaultXmlNamespace(xmlns);
        }


        public static string DeleteStaff(List<long> id)
        {
            string result = String.Empty;
            pu6Entities db = new pu6Entities();

            string list = String.Join(",", id.ToArray());
            try
            {
                List<string> tableList = new List<string> { "FormsRSW", "FormsRSW2014_1_Razd_6_1", "FormsSPW2", "FormsSZV_6_4", "FormsSZV_6", "FormsSZV_M_2016_Staff", "FormsSZV_ISH_2017", "FormsSZV_KORR_2017", "FormsSZV_STAJ_2017", "FormsADW_1", "FormsADW_2" };

                db.ExecuteStoreCommand(String.Format("DELETE FROM Staff WHERE ([ID] IN ({0}))", list));

                foreach (var table in tableList)
                {
                    db.ExecuteStoreCommand(String.Format("DELETE FROM " + table + " WHERE ([StaffID] IN ({0}))", list));
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public static string DeleteInsurer(List<long> id)
        {
            string result = String.Empty;
            pu6Entities db = new pu6Entities();

            try
            {
                List<string> reglist = db.Insurer.Where(x => id.Contains(x.ID)).Select(x => x.RegNum).ToList();

                Props props = new Props();

                props.ReadXml();
                var Insurers = props.Fields.LastPackNumber;

                foreach (var regnum in reglist)
                {
                    if (Insurers.Any(x => x.RegNum == regnum))
                    {
                        var ins = Insurers.First(x => x.RegNum == regnum);
                        props.Fields.LastPackNumber.Remove(ins);
                    }
                } 
                
                props.WriteXml();
            }                   
            catch { }

            string list = String.Join(",", id.ToArray());
            try
            {
                db.ExecuteStoreCommand(String.Format("PRAGMA foreign_keys=off;DELETE FROM Insurer WHERE ([ID] IN ({0}));PRAGMA foreign_keys=on;", list));

                db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_2_1 WHERE ([InsurerID] IN ({0}))", list));
                
                db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_2_4 WHERE ([InsurerID] IN ({0}))", list));
                
                db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_3_4 WHERE ([InsurerID] IN ({0}))", list));
                
                db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_2_5_1 WHERE ([InsurerID] IN ({0}))", list));
                
                db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_2_5_2 WHERE ([InsurerID] IN ({0}))", list));
                
                db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_4 WHERE ([InsurerID] IN ({0}))", list));

                db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_5 WHERE ([InsurerID] IN ({0}))", list));

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        public static bool DeleteRSW1(FormsRSW2014_1_1 rsw_)
        {
            bool result = true;
            pu6Entities db = new pu6Entities();


            foreach (var item in db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == rsw_.InsurerID && x.Year == rsw_.Year && x.Quarter == rsw_.Quarter && x.CorrectionNum == rsw_.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_2_1.DeleteObject(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == rsw_.InsurerID && x.Year == rsw_.Year && x.Quarter == rsw_.Quarter && x.CorrectionNum == rsw_.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_2_4.DeleteObject(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.InsurerID == rsw_.InsurerID && x.Year == rsw_.Year && x.Quarter == rsw_.Quarter && x.CorrectionNum == rsw_.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_2_5_1.DeleteObject(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.InsurerID == rsw_.InsurerID && x.Year == rsw_.Year && x.Quarter == rsw_.Quarter && x.CorrectionNum == rsw_.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_2_5_2.DeleteObject(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_3_4.Where(x => x.InsurerID == rsw_.InsurerID && x.Year == rsw_.Year && x.Quarter == rsw_.Quarter && x.CorrectionNum == rsw_.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_3_4.DeleteObject(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == rsw_.InsurerID && x.Year == rsw_.Year && x.Quarter == rsw_.Quarter && x.CorrectionNum == rsw_.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_4.DeleteObject(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_5.Where(x => x.InsurerID == rsw_.InsurerID && x.Year == rsw_.Year && x.Quarter == rsw_.Quarter && x.CorrectionNum == rsw_.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_5.DeleteObject(item);
            }


            if (db.FormsRSW2014_1_1.Any(x => x.ID == rsw_.ID))
            {
                FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == rsw_.ID);
                db.FormsRSW2014_1_1.DeleteObject(rsw);
            }

            try
            {
                db.SaveChanges();
            }
            catch
            {
                result = false;
            }


            return result;
        }

        public static void checkAndSetActiveUser(UserAccess.Users user)
        {
            pu6Entities db = new pu6Entities();
            if (db.Users.Any(x => x.ID == user.ID)) // если в эту базу уже заходил такой пользователь, проверяем данные по нему, если надо то обновляем
            {
                Models.Users userWork = db.Users.FirstOrDefault(x => x.ID == user.ID);

                userWork.Name = user.Name;
                userWork.Login = user.Login;
                if (user.RoleID.HasValue)
                    userWork.RoleID = user.RoleID.Value;
                if (user.DateCreate.HasValue)
                    userWork.DateCreate = user.DateCreate.Value;
                userWork.LastAccessDate = DateTime.Now;
                userWork.SysAdmin = user.SysAdmin.HasValue ? user.SysAdmin.Value : (byte)0;

                db.ObjectStateManager.ChangeObjectState(userWork, EntityState.Modified);
                db.SaveChanges();
            }
            else // если такой пользователь в рабочей базе не найден, то добавляем его
            {
                Models.Users userWork = new Models.Users();

                userWork.ID = user.ID;
                userWork.Name = user.Name;
                userWork.Login = user.Login;
                if (user.RoleID.HasValue)
                    userWork.RoleID = user.RoleID.Value;
                if (user.DateCreate.HasValue)
                    userWork.DateCreate = user.DateCreate.Value;
                userWork.LastAccessDate = DateTime.Now;
                userWork.SysAdmin = user.SysAdmin.HasValue ? user.SysAdmin.Value : (byte)0;

                db.Users.AddObject(userWork);
                db.SaveChanges();
            }

            UserAccess.xaccessEntities xaccessdb = new UserAccess.xaccessEntities();
            var u = xaccessdb.Users.FirstOrDefault(x => x.ID == user.ID);
            u.LastAccessDate = DateTime.Now;
            xaccessdb.ObjectStateManager.ChangeObjectState(u, EntityState.Modified);
            xaccessdb.SaveChanges();

        }

        public static void showAlert(string CaptionText, string ContentText, string ThemeName, int Height = 150)
        {
            Telerik.WinControls.UI.RadDesktopAlert alert = new Telerik.WinControls.UI.RadDesktopAlert { ThemeName = ThemeName, FadeAnimationFrames = 60, PopupAnimationFrames = 30, Opacity = 0.9F, PopupAnimationDirection = Telerik.WinControls.UI.RadDirection.Up, ShowOptionsButton = false, FixedSize = new Size(350, Height) };
            alert.CaptionText = CaptionText;
            alert.ContentText = ContentText;
            alert.Show();
        }

        public static void HeaderChangeAllTabs()
        {

            foreach (Form form in Application.OpenForms)
            {
                if (form is FormsRSW2014.RSW2014_List)
                {
                    (form as FormsRSW2014.RSW2014_List).HeaderChange();
                }

                if (form is FormsSPW2_2014.SPW2_List)
                {
                    (form as FormsSPW2_2014.SPW2_List).HeaderChange();
                }

                if (form is FormsRSW2_2014.RSW2_2014_List)
                {
                    (form as FormsRSW2_2014.RSW2_2014_List).HeaderChange();
                }

                if (form is FormsSZV_6_2010.SZV_6_List)
                {
                    (form as FormsSZV_6_2010.SZV_6_List).HeaderChange();
                }

                if (form is FormsSZV_6_4_2013.SZV_6_4_List)
                {
                    (form as FormsSZV_6_4_2013.SZV_6_4_List).HeaderChange();
                }
                if (form is FormsRW_3_2015.RW3_2015_List)
                {
                    (form as FormsRW_3_2015.RW3_2015_List).HeaderChange();
                }
                if (form is FormsSZVM_2016.SZV_M_2016_List)
                {
                    (form as FormsSZVM_2016.SZV_M_2016_List).HeaderChange();
                }
                if (form is FormsDSW3.DSW3_List)
                {
                    (form as FormsDSW3.DSW3_List).HeaderChange();
                }
                if (form is FormsADW1.FormsADW1_List)
                {
                    (form as FormsADW1.FormsADW1_List).HeaderChange();
                }
                if (form is FormsADW2.FormsADW2_List)
                {
                    (form as FormsADW2.FormsADW2_List).HeaderChange();
                }
                if (form is FormsADW3.FormsADW3_List)
                {
                    (form as FormsADW3.FormsADW3_List).HeaderChange();
                }
                if (form is FormsODV1.ODV1_List)
                {
                    (form as FormsODV1.ODV1_List).HeaderChange();
                }
            }

        }

        public static string HeaderChange()
        {
            pu6Entities db = new pu6Entities();
            string header = String.Empty;

            if (Options.InsID != 0 && db.Insurer.Any(x => x.ID == Options.InsID))
            {
                Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

                header = "Страхователь: [" + Utils.ParseRegNum(ins.RegNum) + "]  ";

                if (ins.TypePayer == 0) // если организация
                {
                    header = header + ins.NameShort;
                }
                else
                {
                    header = header + ins.LastName + " " + ins.FirstName + " " + ins.MiddleName;
                }

            }
            else
            {
                Options.InsID = 0;
                header = "Страхователь не выбран";
            }

            CurrentInsurerFoldersChange();

            return header;
        }

        public static void CurrentInsurerFoldersChange()
        {
            pu6Entities db = new pu6Entities();

            Options.CurrentInsurerFolders.regnum = "";
            Options.CurrentInsurerFolders.importPath = "";
            Options.CurrentInsurerFolders.exportPath = "";

            if (Options.InsID != 0 && db.Insurer.Any(x => x.ID == Options.InsID))
            {
                Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

                if (Options.InsurerFolders.Any(x => x.regnum == ins.RegNum))
                {
                    var param = Options.InsurerFolders.FirstOrDefault(x => x.regnum == ins.RegNum);

                    Options.CurrentInsurerFolders.regnum = param.regnum;
                    Options.CurrentInsurerFolders.importPath = param.importPath;
                    Options.CurrentInsurerFolders.exportPath = param.exportPath;

                }
            }
        }


        public static long checkUserAccessLevel(string formName)
        {
            //PU.UserAccess.xaccessEntities xaccessDB = new PU.UserAccess.xaccessEntities();

            long level = 1;

            //if (xaccessDB.FormsToObjects.Any(x => x.Form == formName))  // Если правила доступа для этой формы, идем дальше
            //{
            //    long objID = xaccessDB.FormsToObjects.First(x => x.Form == formName).ObjectsID.Value;

            //    if (xaccessDB.UsersAccessLevelToObjects.Any(x => x.UsersID == Options.User.ID && x.AccessObjectID == objID))
            //    {
            //        level = xaccessDB.UsersAccessLevelToObjects.First(x => x.UsersID == Options.User.ID && x.AccessObjectID == objID).AccessLevelID.Value;
            //    }

            //}

            return level;
        }



    }
}
