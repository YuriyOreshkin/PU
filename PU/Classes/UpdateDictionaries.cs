using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PU.Models;
using System.Data;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;

namespace PU.Classes
{
    class UpdateDictionaries
    {
        public static bool updateTable(string tableName, string Base_emp_path, string ThemeName, Form _parent)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            pu6Entities db = new pu6Entities();
            bool result = true;
            string query = String.Empty;
            string query_select = String.Empty;
            string tableType = tableName;

            String[] tableList1 = new string[] { "TerrUsl", "OsobUslTruda", "IschislStrahStajOsn", "IschislStrahStajDop", "VidTrudDeyat", "SpecOcenkaUslTruda" };
            String[] tableList2 = new string[] { "DocumentTypes", "KodVred_1", "FormsSZV_TD_2020_TypesOfEvents" };
            String[] tableList3 = new string[] { "KodVred_2", "KodVred_3" };
            String[] tableList4 = new string[] { "TariffPlat", "MROT", "DopTariff", "CodeBaseRW3_2015" };

            
            if (tableList1.Contains(tableName))
            {
                tableType = "tableList1";
            }
            else if (tableList2.Contains(tableName))
            {
                tableType = "tableList2";
            }
            else if (tableList3.Contains(tableName))
            {
                tableType = "tableList3";
            }
            else if (tableList4.Contains(tableName))
            {
                tableType = "tableList4";
            }

            switch (tableType)
            {
                case "tableList1":
                    #region Территориальные условия, Особые условия труда, Исчисл. страх. стаж ОСН и ДОП, Виды трудовой или иной деят-ти, Спец. оценка условий труда
                    query_select = String.Format(@"SELECT * FROM {0}", tableName);
                    try
                    {
                        var items_from_db = (IEnumerable)db.GetType()
                           .GetProperty(tableName)
                           .GetValue(db, null);

                        List<long> list_id = new List<long>();
                        foreach (var r in items_from_db)
                        {
                            var properties = r.GetType().GetProperty("ID");
                            list_id.Add((long)properties.GetValue(r, null));
                        }

                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + Base_emp_path, true))
                        {
                            con.Open();                             // Open the connection to the database

                            System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(query_select, con);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            con.Close();
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                long id = long.Parse(item["ID"].ToString());

                                if (list_id.Contains(id)) // если запись с таким ID  уже есть у нас в базе, то обновляем ее значения
                                {
                                    string s = String.Empty;

//                                    if (!String.IsNullOrEmpty(item["DateEnd"].ToString()))
//                                    {
                                    s = String.Format(@", DateEnd = {0}", !String.IsNullOrEmpty(item["DateEnd"].ToString()) ? ("'" + DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd")  + "'") : "NULL");
//                                    }

                                    query = query + String.Format(@" UPDATE {0} SET Code = '{1}', Name = '{2}', DateBegin = '{3}' {4} WHERE ID = {5};", tableName, item["Code"].ToString(), item["Name"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s, id.ToString());
                                }
                                else // Если записи нет, то добавляем ее
                                {
                                    string s1 = String.Empty;
                                    string s2 = String.Empty;

                                    if (!String.IsNullOrEmpty(item["DateEnd"].ToString()))
                                    {
                                        s1 = ", DateEnd";
                                        s2 = ", '" + DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd") + "'";
                                    }

                                    query = query + String.Format(@" INSERT INTO {0} (Code, Name, DateBegin {4}) VALUES ('{1}', '{2}', '{3}' {5});", tableName, item["Code"].ToString(), item["Name"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s1, s2);
                                }
                            }
                        }

                        db.Database.ExecuteSqlCommand(query);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (_parent != null)
                            _parent.Invoke(new Action(() => { Methods.showAlert("Ошибка синхронизации", "При обновлении данных таблицы " + tableName + " произошла ошибка.\r\nКод ошибки: " + ex.Message.ToString(), ThemeName); }));

                        result = false;
                    }
                    #endregion
                    break;
                case "PlatCategory":
                    #region Категории плательщиков
                    query_select = String.Format(@"SELECT * FROM {0}", tableName);
                    try
                    {
                        var items_from_db = db.PlatCategory.ToList();

                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + Base_emp_path, true))
                        {
                            con.Open();                             // Open the connection to the database


                            System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(query_select, con);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            con.Close();
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                long id = long.Parse(item["ID"].ToString());


                                if (items_from_db.Any(x => x.ID == id)) // если запись с таким ID  уже есть у нас в базе, то обновляем ее значения
                                {
                                    string s = String.Empty;

//                                    if (!String.IsNullOrEmpty(item["DateEnd"].ToString()))
//                                    {
                                        s = String.Format(@", DateEnd = {0}", !String.IsNullOrEmpty(item["DateEnd"].ToString()) ? ("'" + DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd") + "'") : "NULL");

//                                    }

                                    query = query + String.Format(@" UPDATE {0} SET Code = '{1}', Name = '{2}', FullName = '{6}', PlatCategoryRaschPerID = {7}, DateBegin = '{3}' {4} WHERE ID = {5};", tableName, item["Code"].ToString(), item["Name"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s, id.ToString(), item["FullName"].ToString(), item["PlatCategoryRaschPerID"].ToString());
                                }
                                else // Если записи нет, то добавляем ее
                                {
                                    string s1 = String.Empty;
                                    string s2 = String.Empty;

                                    if (!String.IsNullOrEmpty(item["DateEnd"].ToString()))
                                    {
                                        s1 = ", DateEnd";
                                        s2 = ", '" + DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd") + "'";
                                    }

                                    query = query + String.Format(@" INSERT INTO {0} (Code, Name, DateBegin {4}, FullName, PlatCategoryRaschPerID) VALUES ('{1}', '{2}', '{3}' {5}, '{6}', {7});", tableName, item["Code"].ToString(), item["Name"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s1, s2, item["FullName"].ToString(), item["PlatCategoryRaschPerID"].ToString());
                                }
                            }
                        }

                        db.Database.ExecuteSqlCommand(query);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (_parent != null)
                            _parent.Invoke(new Action(() => { Methods.showAlert("Ошибка синхронизации", "При обновлении данных таблицы " + tableName + " произошла ошибка.\r\nКод ошибки: " + ex.Message.ToString(), ThemeName); }));

                        result = false;
                    }
                    #endregion
                    break;
                case "tableList2":
                    #region Вредные профессии, Типы документов
                    query_select = String.Format(@"SELECT * FROM {0}", tableName);
                    try
                    {
                        var items_from_db = (IEnumerable)db.GetType()
                           .GetProperty(tableName)
                           .GetValue(db, null);

                        List<long> list_id = new List<long>();
                        foreach (var r in items_from_db)
                        {
                            var properties = r.GetType().GetProperty("ID");
                            list_id.Add((long)properties.GetValue(r, null));
                        }

                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + Base_emp_path, true))
                        {
                            con.Open();                             // Open the connection to the database

                            System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(query_select, con);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            con.Close();
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                long id = long.Parse(item["ID"].ToString());


                                if (list_id.Contains(id)) // если запись с таким ID  уже есть у нас в базе, то обновляем ее значения
                                {
                                    query = query + String.Format(@" UPDATE {0} SET Code = '{1}', Name = '{2}' WHERE ID = {3};", tableName, item["Code"].ToString(), item["Name"].ToString(), id.ToString());
                                }
                                else // Если записи нет, то добавляем ее
                                {
                                    query = query + String.Format(@" INSERT INTO {0} (Code, Name) VALUES ('{1}', '{2}');", tableName, item["Code"].ToString(), item["Name"].ToString());
                                }
                            }
                        }

                        db.Database.ExecuteSqlCommand(query);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (_parent != null)
                            _parent.Invoke(new Action(() => { Methods.showAlert("Ошибка синхронизации", "При обновлении данных таблицы " + tableName + " произошла ошибка.\r\nКод ошибки: " + ex.Message.ToString(), ThemeName); }));

                        result = false;
                    }
                    #endregion
                    break;
                case "tableList3":
                    #region Вредные профессии 2 и 3
                    query_select = String.Format(@"SELECT * FROM {0}", tableName);
                    try
                    {
                        var items_from_db = (IEnumerable)db.GetType()
                           .GetProperty(tableName)
                           .GetValue(db, null);

                        List<long> list_id = new List<long>();
                        foreach (var r in items_from_db)
                        {
                            var properties = r.GetType().GetProperty("ID");
                            list_id.Add((long)properties.GetValue(r, null));
                        }

                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + Base_emp_path, true))
                        {
                            con.Open();                             // Open the connection to the database

                            System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(query_select, con);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            con.Close();


                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                long id = long.Parse(item["ID"].ToString());

                                if (list_id.Contains(id)) // если запись с таким ID  уже есть у нас в базе, то обновляем ее значения
                                {
                         //           query = query + String.Format(@" UPDATE {0} SET Code = '{1}', Name = '{2}', RazdelCode = '{4}', SpisokCode = {5} WHERE ID = {3};", tableName, item["Code"].ToString(), item["Name"].ToString(), id.ToString(), item["RazdelCode"].ToString(), item["SpisokCode"].ToString());
                                }
                                else // Если записи нет, то добавляем ее
                                {
                                    query = query + String.Format(@" INSERT INTO {0} (Code, Name, RazdelCode, SpisokCode) VALUES ('{1}', '{2}', '{3}', {4});", tableName, item["Code"].ToString(), item["Name"].ToString(), item["RazdelCode"].ToString(), item["SpisokCode"].ToString());
                                }
                            }
                        }

                        db.Database.ExecuteSqlCommand(query);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (_parent != null)
                            _parent.Invoke(new Action(() => { Methods.showAlert("Ошибка синхронизации", "При обновлении данных таблицы " + tableName + " произошла ошибка.\r\nКод ошибки: " + ex.Message.ToString(), ThemeName); }));

                        result = false;
                    }
                    #endregion
                    break;
                case "UslDosrNazn":
                    #region Условия для досрочного назн. труд пенсии
                    query_select = String.Format(@"SELECT * FROM {0}", tableName);
                    try
                    {
                        var items_from_db = (IEnumerable)db.GetType()
                           .GetProperty(tableName)
                           .GetValue(db, null);

                        List<long> list_id = new List<long>();
                        foreach (var r in items_from_db)
                        {
                            var properties = r.GetType().GetProperty("ID");
                            list_id.Add((long)properties.GetValue(r, null));
                        }

                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + Base_emp_path, true))
                        {
                            con.Open();                             // Open the connection to the database

                            System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(query_select, con);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            con.Close();
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                long id = long.Parse(item["ID"].ToString());

                                if (list_id.Contains(id)) // если запись с таким ID  уже есть у нас в базе, то обновляем ее значения
                                {
                                    string s = String.Empty;
                                    string EdIzmID = String.Empty;

                                    EdIzmID = String.Format(@", EdIzmID = {0}", !String.IsNullOrEmpty(item["EdIzmID"].ToString()) ? item["EdIzmID"].ToString() : "null");
                                    
//                                    if (!String.IsNullOrEmpty(item["DateEnd"].ToString()))
//                                    {
//                                        s = String.Format(@", DateEnd = '{0}'", DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd"));
                                        s = String.Format(@", DateEnd = {0}", !String.IsNullOrEmpty(item["DateEnd"].ToString()) ? ("'" + DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd") + "'") : "NULL");
//                                    }

                                    query = query + String.Format(@" UPDATE {0} SET Code = '{1}', Name = '{2}', DateBegin = '{3}' {4} {6} WHERE ID = {5};", tableName, item["Code"].ToString(), item["Name"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s, id.ToString(), EdIzmID);
                                }
                                else // Если записи нет, то добавляем ее
                                {
                                    string s1 = String.Empty;
                                    string s2 = String.Empty;
                                    string EdIzmID1 = String.Empty;
                                    string EdIzmID2 = String.Empty;

                                    if (!String.IsNullOrEmpty(item["EdIzmID"].ToString()))
                                    {
                                        EdIzmID1 = ", EdIzmID";
                                        EdIzmID2 = ", " + item["EdIzmID"].ToString();
                                    }

                                    query = query + String.Format(@" INSERT INTO {0} (Code, Name, DateBegin {4} {6}) VALUES ('{1}', '{2}', '{3}' {5} {7});", tableName, item["Code"].ToString(), item["Name"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s1, s2, EdIzmID1, EdIzmID2);
                                }
                            }
                        }

                        db.Database.ExecuteSqlCommand(query);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (_parent != null)
                            _parent.Invoke(new Action(() => { Methods.showAlert("Ошибка синхронизации", "При обновлении данных таблицы " + tableName + " произошла ошибка.\r\nКод ошибки: " + ex.Message.ToString(), ThemeName); }));

                        result = false;
                    }
                    #endregion
                    break;
                case "SpecOcenkaUslTrudaDopTariff":
                    #region Спец. оценка условий труда доп тариф
                    query_select = String.Format(@"SELECT * FROM {0}", tableName);
                    try
                    {
                        var items_from_db = (IEnumerable)db.GetType()
                           .GetProperty(tableName)
                           .GetValue(db, null);

                        List<long> list_id = new List<long>();
                        foreach (var r in items_from_db)
                        {
                            var properties = r.GetType().GetProperty("ID");
                            list_id.Add((long)properties.GetValue(r, null));
                        }

                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + Base_emp_path, true))
                        {
                            con.Open();                             // Open the connection to the database

                            System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(query_select, con);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            con.Close();
                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                long id = long.Parse(item["ID"].ToString());

                                if (list_id.Contains(id)) // если запись с таким ID  уже есть у нас в базе, то обновляем ее значения
                                {
                                    string s = String.Empty;

//                                    if (!String.IsNullOrEmpty(item["DateEnd"].ToString()))
//                                    {
//                                        s = String.Format(@", DateEnd = '{0}'", DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd"));
                                        s = String.Format(@", DateEnd = {0}", !String.IsNullOrEmpty(item["DateEnd"].ToString()) ? ("'" + DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd") + "'") : "NULL");

//                                    }

                                    query = query + String.Format(@" UPDATE {0} SET SpecOcenkaUslTrudaID = {1}, Type = {2}, Rate = {6}, DateBegin = '{3}' {4} WHERE ID = {5};", tableName, item["SpecOcenkaUslTrudaID"].ToString(), item["Type"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s, id.ToString(), item["Rate"].ToString());
                                }
                                else // Если записи нет, то добавляем ее
                                {
                                    string s1 = String.Empty;
                                    string s2 = String.Empty;

                                    if (!String.IsNullOrEmpty(item["DateEnd"].ToString()))
                                    {
                                        s1 = ", DateEnd";
                                        s2 = ", '" + DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd") + "'";
                                    }

                                    query = query + String.Format(@" INSERT INTO {0} (SpecOcenkaUslTrudaID, Type, Rate, DateBegin {4}) VALUES ({1}, {2}, {6}, '{3}' {5});", tableName, item["SpecOcenkaUslTrudaID"].ToString(), item["Type"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s1, s2, item["Rate"].ToString());
                                }
                            }
                        }

                        db.Database.ExecuteSqlCommand(query);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (_parent != null)
                            _parent.Invoke(new Action(() => { Methods.showAlert("Ошибка синхронизации", "При обновлении данных таблицы " + tableName + " произошла ошибка.\r\nКод ошибки: " + ex.Message.ToString(), ThemeName); }));

                        result = false;
                    }
                    #endregion
                    break;
                case "tableList4":
                    #region Тарифы категорий плательщиков
                    query_select = String.Format(@"SELECT * FROM {0}", tableName);
                    try
                    {
                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + Base_emp_path, true))
                        {
                            con.Open();                             // Open the connection to the database

                            System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(query_select, con);
                            DataSet ds = new DataSet();
                            da.Fill(ds);
                            con.Close();

                            query = String.Format(@"DELETE FROM {0}", tableName);
                            db.Database.ExecuteSqlCommand(query);
                            query = "";

                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                switch (tableName)
                                {
                                    case "TariffPlat":
                                        query = query + String.Format(@" INSERT INTO {0} (PlatCategoryID, Year, StrahPercant1966, StrahPercent1967, NakopPercant, FFOMS_Percent, TFOMS_Percent) VALUES ({1}, {2}, {3}, {4}, {5}, {6}, {7});", tableName, item["PlatCategoryID"].ToString(), item["Year"].ToString(), item["StrahPercant1966"].ToString(), item["StrahPercent1967"].ToString(), item["NakopPercant"].ToString(), item["FFOMS_Percent"].ToString(), item["TFOMS_Percent"].ToString());
                                        break;
                                    case "MROT":
                                        query = query + String.Format(@" INSERT INTO {0} (Year, Name, NalogBase, Mrot) VALUES ({1}, '{2}', {3}, {4});", tableName, item["Year"].ToString(), item["Name"].ToString(), item["NalogBase"].ToString(), item["Mrot"].ToString());
                                        break;
                                    case "DopTariff":
                                        query = query + String.Format(@" INSERT INTO {0} (Year, Tariff1, Tariff2) VALUES ({1}, {2}, {3});", tableName, item["Year"].ToString(), item["Tariff1"].ToString(), item["Tariff2"].ToString());
                                        break;
                                    case "CodeBaseRW3_2015":
                                        query = query + String.Format(@" INSERT INTO {0} (Year, Tar21, Tar22) VALUES ({1}, '{2}', {3});", tableName, item["Year"].ToString(), item["Tar21"].ToString(), item["Tar22"].ToString());
                                        break;
                                }
                            }
                        }

                        db.Database.ExecuteSqlCommand(query);
                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (_parent != null)
                            _parent.Invoke(new Action(() => { Methods.showAlert("Ошибка синхронизации", "При обновлении данных таблицы " + tableName + " произошла ошибка.\r\nКод ошибки: " + ex.Message.ToString(), ThemeName); }));

                        result = false;
                    }
                    #endregion
                    break;
                case "TariffCode":
                    #region Тарифы категорий плательщиков
                    try
                    {
                        List<TariffCodePlatCat_temp> TCPL_list = new List<TariffCodePlatCat_temp>();

                        

                        query_select = @"SELECT PlatCategoryID, TariffCodeID, TariffCode.Code FROM TariffCodePlatCat INNER JOIN TariffCode ON TariffCode.ID = TariffCodePlatCat.TariffCodeID";
                        DataSet ds_TariffCodePlatCat = new DataSet();
                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + Base_emp_path, true))
                        {
                            con.Open();                             // Open the connection to the database

                            System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(query_select, con);
                            da.Fill(ds_TariffCodePlatCat);
                            con.Close();


                            foreach (DataRow item in ds_TariffCodePlatCat.Tables[0].Rows)
                            {

                                long PlatCategoryID_ = long.Parse(item["PlatCategoryID"].ToString());
                                long TariffCodeID_ = long.Parse(item["TariffCodeID"].ToString());
                                TCPL_list.Add(new TariffCodePlatCat_temp
                                {
                                    PlatCategoryID = PlatCategoryID_,
                                    TariffCodeID = TariffCodeID_,
                                    Code = item["Code"].ToString()
                                });

               //                 query = query + String.Format(@" INSERT INTO TariffCodePlatCat (PlatCategoryID, TariffCodeID) VALUES ({0}, {1});", item["PlatCategoryID"].ToString(), id);

                            }
                        }

                        var tariffCodeListAdded = db.TariffCode;


                        query_select = String.Format(@"SELECT * FROM {0}", tableName);
                        DataSet ds = new DataSet();
                        using (System.Data.SQLite.SQLiteConnection con = new System.Data.SQLite.SQLiteConnection("data source=" + Base_emp_path, true))
                        {
                            con.Open();                             // Open the connection to the database

                            System.Data.SQLite.SQLiteDataAdapter da = new System.Data.SQLite.SQLiteDataAdapter(query_select, con);
                            da.Fill(ds);
                            con.Close();
                            query = @"DELETE FROM TariffCodePlatCat";
                            db.Database.ExecuteSqlCommand(query);
                            query = "";

                            foreach (DataRow item in ds.Tables[0].Rows)
                            {
                                string code = item["Code"].ToString();

                                if (tariffCodeListAdded.Select(x => x.Code).ToList().Contains(code)) // если запись с таким ID  уже есть у нас в базе, то обновляем ее значения
                                {
                                    string s = String.Empty;

//                                    if (!String.IsNullOrEmpty(item["DateEnd"].ToString()))
//                                    {
//                                        s = String.Format(@", DateEnd = '{0}'", DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd"));
                                        s = String.Format(@", DateEnd = {0}", !String.IsNullOrEmpty(item["DateEnd"].ToString()) ? ("'" + DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd") + "'") : "NULL");
//                                    }

                                    query = query + String.Format(@" UPDATE {0} SET DateBegin = '{2}' {3} WHERE Code = {1};", tableName, item["Code"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s);
                                }
                                else // Если записи нет, то добавляем ее
                                {
                                    string s1 = String.Empty;
                                    string s2 = String.Empty;

                                    if (!String.IsNullOrEmpty(item["DateEnd"].ToString()))
                                    {
                                        s1 = ", DateEnd";
                                        s2 = ", '" + DateTime.Parse(item["DateEnd"].ToString()).ToString("yyyy-MM-dd") + "'";
                                    }

                                    query = query + String.Format(@" INSERT INTO {0} (Code, DateBegin {3}) VALUES ('{1}', '{2}' {4});", tableName, item["Code"].ToString(), DateTime.Parse(item["DateBegin"].ToString()).ToString("yyyy-MM-dd"), s1, s2);
                                }

                            }
                        }

                        db.Database.ExecuteSqlCommand(query);

                        query = "";
                        foreach(var item in TCPL_list)
                        {
                            item.TariffCodeID = tariffCodeListAdded.FirstOrDefault(x => x.Code == item.Code).ID;

                            query = query + String.Format(@" INSERT INTO TariffCodePlatCat (PlatCategoryID, TariffCodeID) VALUES ({0}, {1});", item.PlatCategoryID.ToString(), item.TariffCodeID.ToString());

                        }
                        db.Database.ExecuteSqlCommand(query);

                        result = true;
                    }
                    catch (Exception ex)
                    {
                        if (_parent != null)
                            _parent.Invoke(new Action(() => { Methods.showAlert("Ошибка синхронизации", "При обновлении данных таблицы " + tableName + " произошла ошибка.\r\nКод ошибки: " + ex.Message.ToString(), ThemeName); }));

                        result = false;
                    }
                    #endregion
                    break;

            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");

            return result;
        }

        public class TariffCodePlatCat_temp
        {
            public long PlatCategoryID { get; set; }
            public long TariffCodeID { get; set; }
            public string Code { get; set; }
        }

    }
}
