using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using Telerik.WinControls;
using PU.Models;
using PU.Classes;
using Telerik.WinControls.UI;
using System.Globalization;
using System.Data;

namespace PU
{
    public partial class ImportMain : Telerik.WinControls.UI.RadForm
    {
        BackgroundWorker bw = new BackgroundWorker();
        private pu6Entities db = new pu6Entities();
        public long InsID = 0;   // ID страхователя
        private bool cancel_work = false;
        public string DBFMessage = String.Empty;
        private string currDirPath = "";
        OpenFileDialog openDialog = new OpenFileDialog();
        FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
        List<string> tempFiles = new List<string> { };
        int transCnt = 1;

        List<FormsSZV_STAJ_2017> SZV_STAJ_List = new List<FormsSZV_STAJ_2017>();

        public ImportMain()
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

        private void ImportMain_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            openDialog.Filter = "(*.dbf)|*.dbf";
            openDialog.Multiselect = true;
            if (!String.IsNullOrEmpty(Options.CurrentInsurerFolders.importPath))
            {
                folderBrowser.SelectedPath = Options.CurrentInsurerFolders.importPath;
                openDialog.InitialDirectory = Options.CurrentInsurerFolders.importPath;
            }

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    foreach (var item in param.windowData)
                    {
                        int i = 0;

                        switch (item.control)
                        {
                            case "folderBrowser":
                                folderBrowser.SelectedPath = item.value;
                                break;
                            case "openDialog":
                                openDialog.InitialDirectory = item.value;
                                break;
                            case "updateDateFilling":
                                updateDateFilling.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateStaff_DDL":
                                int.TryParse(item.value, out i);
                                updateStaff_DDL.SelectedIndex = i;
                                break;
                            case "updateStaj_DDL":
                                int.TryParse(item.value, out i);
                                updateStaj_DDL.SelectedIndex = i;
                                break;
                            case "updateIndSved_DDL":
                                int.TryParse(item.value, out i);
                                updateIndSved_DDL.SelectedIndex = i;
                                break;
                            case "transactionCount":
                                int.TryParse(item.value, out i);
                                transactionCount.Value = i;
                                break;
                            case "codeAutoRadioButton":
                                codeAutoRadioButton.IsChecked = item.value == "true" ? true : false;
                                break;
                            case "code866RadioButton":
                                code866RadioButton.IsChecked = item.value == "true" ? true : false;
                                break;
                            case "code1251RadioButton":
                                code1251RadioButton.IsChecked = item.value == "true" ? true : false;
                                break;

                        }
                    }

                }
                catch
                { }
            }


            HeaderChange();
        }


        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void radButton1_Click(object sender, EventArgs e)
        {
            /*        if (dbfToImportBrowser.DialogType == Telerik.WinControls.UI.BrowseEditorDialogType.OpenFileDialog)
                    {
                        OpenFileDialog dialog = (OpenFileDialog)dbfToImportBrowser.Dialog;
                        dialog.Filter = "(*.dbf)|*.dbf";
                    }*/
            if (importFilesGrid.RowCount <= 0)
            {
                Messenger.showAlert(AlertType.Info, "Внимание!", "Не выбраны файлы для импорта!", this.ThemeName);
                return;
            }

            if (Options.InsID == 0)
            {
                Messenger.showAlert(AlertType.Info, "Внимание!", "Не выбран Страхователь для импорта данных!", this.ThemeName);
                insChangeBtn.Focus();
                return;
            }


            if (importFilesGrid.RowCount > 0 && Options.InsID != 0)
            {
                transCnt = (int)transactionCount.Value;

                SZV_STAJ_List = new List<FormsSZV_STAJ_2017>();

                bw = new BackgroundWorker();
                this.bw.WorkerReportsProgress = true;
                this.bw.WorkerSupportsCancellation = true;
                this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(importGo);
                this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_RunWorkerCompleted);
                this.bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_ProgressChanged);

                radButton2.Visible = false;
                abortBtn.Location = radButton2.Location;
                abortBtn.Visible = true;

                insChangeBtn.Enabled = false;
                importBtn.Enabled = false;
                updateStaff_DDL.Enabled = false;
                updateStaj_DDL.Enabled = false;
                updateIndSved_DDL.Enabled = false;
                updateDateFilling.Enabled = false;
                selectFolderBtn.Enabled = false;
                selectFilesBtn.Enabled = false;

                transactionCount.Enabled = false;
                code1251RadioButton.Enabled = false;
                code866RadioButton.Enabled = false;
                codeAutoRadioButton.Enabled = false;

                //                radProgressBar1.Value1 = 0;
                //                radProgressBar1.Visible = true;
                firstPartLabel.Visible = true;
                firstPartLabel_.Visible = true;
                secondPartLabel.Visible = true;
                secondPartLabel_.Visible = true;

                bw.RunWorkerAsync();
            }


        }

        private void importGo(object sender, DoWorkEventArgs e)
        {
            foreach (var row in importFilesGrid.Rows)
            {
                importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].IsSelected = true; importFilesGrid.Rows[row.Index].Cells[0].Value = Color.SkyBlue; }));
            }

            foreach (var row in importFilesGrid.Rows)
            {
                row.IsSelected = true;
                importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].IsSelected = true; importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Orange; }));

                if (bw.CancellationPending)
                {
                    return;
                }

                if (DBFRead(row.Cells["path"].Value.ToString()))
                {
                    if (!bw.CancellationPending)
                        importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; }));
                }
                else
                {
                    if (!bw.CancellationPending)
                        importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; }));
                }

            }


            if (SZV_STAJ_List.Count > 0)
            {
                int k = 0;
                foreach (var item in SZV_STAJ_List)
                {
                    try
                    {
                        db.FormsSZV_STAJ_2017.Add(item);

                        k++;
                        if (k % transCnt == 0)
                        {
                            db.SaveChanges();
                        }

                    }
                    catch (Exception ex)
                    {
                        Messenger.showAlert(AlertType.Error, "Внимание!", "Во время сохранения данных формы СЗВ-СТАЖ произошла ошибка! Код ошибки - " + ex.Message, this.ThemeName);
                    }



                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Messenger.showAlert(AlertType.Error, "Внимание!", "Во время сохранения данных формы СЗВ-СТАЖ произошла ошибка! Код ошибки - " + ex.Message, this.ThemeName);
                }
            }

        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (cancel_work == false)
            {
                //                RadMessageBox.Show(this, DBFMessage, "");
                selectFolderBtn.Enabled = true;
                selectFilesBtn.Enabled = true;
                importBtn.Enabled = true;
                radButton2.Visible = true;
                abortBtn.Visible = false;
                insChangeBtn.Enabled = true;
                updateStaff_DDL.Enabled = true;
                updateStaj_DDL.Enabled = true;
                updateIndSved_DDL.Enabled = true;
                updateDateFilling.Enabled = true;

                transactionCount.Enabled = true;
                code1251RadioButton.Enabled = true;
                code866RadioButton.Enabled = true;
                codeAutoRadioButton.Enabled = true;

                //                radProgressBar1.Visible = false;
                radProgressBar1.Value1 = 0;
                firstPartLabel.Visible = false;
                firstPartLabel_.Visible = false;
                secondPartLabel.Visible = false;
                secondPartLabel_.Visible = false;

            }
        }

        public void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            radProgressBar1.Value1 = e.ProgressPercentage;
            firstPartLabel.Text = e.UserState.ToString();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            InsurerFrm child = new InsurerFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.InsID = Options.InsID;
            child.action = "selection";
            child.ShowDialog();
            Options.InsID = child.InsID;

            HeaderChange();
        }


        private void HeaderChange()
        {
            radPanel1.Text = Methods.HeaderChange();
        }



        //чтение DBF файла
        public DataTable GetYourData(string DBFFileName)
        {
            DataTable YourResultSet = new DataTable();

            string DBFFileNameTemp = Path.GetFileName(DBFFileName).ToString();

            OleDbConnection yourConnectionHandler = new OleDbConnection(@"Provider=VFPOLEDB.1;SourceType=DBF;Data Source=" + Path.GetDirectoryName(DBFFileName).ToString() + ";Collating Sequence=MACHINE");
            //            OleDbConnection yourConnectionHandler = new OleDbConnection(@"Provider=VFPOLEDB.1;SourceType=DBF;Data Source=" + "" + ";Collating Sequence=MACHINE");
            //@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.GetDirectoryName(DBFFileName).ToString() + ";Extended Properties=dBASE IV;User ID=;Password=;"
            //@"Provider=VFPOLEDB.1;SourceType=DBF;Data Source=" + Path.GetDirectoryName(DBFFileName).ToString() + ";Collating Sequence=MACHINE"
            try
            {
                yourConnectionHandler.Open();
            }
            catch
            {
                try
                {
                    if (Path.GetFileNameWithoutExtension(DBFFileName).ToString().Length > 8)
                    {
                        string ext = Path.GetExtension(DBFFileName).ToString();
                        DBFFileNameTemp = Path.GetFileNameWithoutExtension(DBFFileName).ToString().Substring(0, 8) + ext;

                        if (File.Exists(Path.Combine(Path.GetDirectoryName(DBFFileName).ToString(), DBFFileNameTemp)))
                        {
                            File.Delete(Path.Combine(Path.GetDirectoryName(DBFFileName).ToString(), DBFFileNameTemp));
                        }
                        File.Copy(DBFFileName, Path.Combine(Path.GetDirectoryName(DBFFileName).ToString(), DBFFileNameTemp)); // копируем во временный файл с именем до 8 символов

                        tempFiles.Add(Path.Combine(Path.GetDirectoryName(DBFFileName).ToString(), DBFFileNameTemp).ToString());  // добавляем в список временных файлов которые потом надо будет удалить
                    }

                    yourConnectionHandler = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Path.GetDirectoryName(DBFFileName).ToString() + ";Extended Properties=dBASE IV;User ID=;Password=;");
                    try
                    {
                        yourConnectionHandler.Open();

                    }
                    catch
                    {
                        return YourResultSet;
                    }
                }
                catch
                {
                    return YourResultSet;
                }
            }


            if (yourConnectionHandler.State == ConnectionState.Open)
            {
                string mySQL = "select * from " + Path.GetFileName(DBFFileNameTemp).ToString();

                /*                OleDbDataAdapter oledbAdapter = new OleDbDataAdapter(mySQL, yourConnectionHandler);
                                DataSet ds = new DataSet();
                                oledbAdapter.Fill(ds);
                                */
                OleDbCommand MyQuery = new OleDbCommand(mySQL, yourConnectionHandler);
                OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);

                DA.Fill(YourResultSet);

                yourConnectionHandler.Close();

                if (Path.GetFileName(DBFFileName) != DBFFileNameTemp)
                {
                    File.Delete(Path.Combine(Path.GetDirectoryName(DBFFileName).ToString(), DBFFileNameTemp));
                }
            }

            return YourResultSet;
        }


        //проверка и перекодировка строки
        private string Encode(string uncodeString)
        {
            uncodeString = uncodeString.Trim();
            string codeString = "";

            if (!String.IsNullOrEmpty(uncodeString))
            {
                if (codeAutoRadioButton.IsChecked)
                {
                    char[] ss = uncodeString.ToCharArray();

                    //bool enc = false;
                    int n = ss.Count();
                    int[] enc = new[] { 0, 0 };

                    for (int i = 0; i < n; i++)
                    {
                        //                    if (!enc)
                        //                    {
                        //Random rnd = new Random();
                        //int ind = rnd.Next(0, ss.Count() - 1);
                        int cod = (int)ss[i];
                        if (ss[i] != ' ' && cod > 128)
                        {
                            if ((cod > 1039 && cod < 1104 || cod == 1105 || cod == 1025))
                                enc[0]++;  // Перекодировка не требуется
                            else
                                enc[1]++;
                        }
                        //                        else
                        //                            n++;
                        //                    }
                    }

                    if (enc[1] > enc[0])  // если больше очков за перекодировку
                    {
                        //                    foreach (char ss_mem in ss)
                        //                    {

                        byte[] bytes_in = testEncodeChkBox.Checked ? Encoding.Default.GetBytes(uncodeString) : Encoding.GetEncoding(1251).GetBytes(uncodeString);
                        //byte[] bytes_in = Encoding.GetEncoding(1251).GetBytes(uncodeString);

                        byte[] bytes_out = testEncodeChkBox.Checked ? Encoding.Convert(Encoding.GetEncoding(866), Encoding.Default, bytes_in) : Encoding.Convert(Encoding.GetEncoding(866), Encoding.GetEncoding(1251), bytes_in);
                        codeString = Encoding.GetEncoding(1251).GetString(bytes_out);
                        //                        break;
                        //                    }

                    }
                    else
                    {
                        codeString = uncodeString;
                    }

                    //foreach (char ss_mem in ss)
                    //{
                    //    if (!(((int)ss_mem > 1039 && (int)ss_mem < 1104 || (int)ss_mem == 1105) || (int)ss_mem < 128))
                    //    {
                    //        byte[] bytes_in = Encoding.GetEncoding(1251).GetBytes(uncodeString);
                    //        byte[] bytes_out = Encoding.Convert(Encoding.GetEncoding(866), Encoding.GetEncoding(1251), bytes_in);
                    //        codeString = Encoding.GetEncoding(1251).GetString(bytes_out);
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        codeString = uncodeString;
                    //    }
                    //}
                }
                else if (code1251RadioButton.IsChecked)
                {
                    codeString = uncodeString;
                }
                else if (code866RadioButton.IsChecked)
                {
                    //byte[] bytes_in = Encoding.GetEncoding(1251).GetBytes(uncodeString);
                    //byte[] bytes_out = Encoding.Convert(Encoding.GetEncoding(866), Encoding.GetEncoding(1251), bytes_in);
                    //codeString = Encoding.GetEncoding(1251).GetString(bytes_out);

                    byte[] bytes_in = testEncodeChkBox.Checked ? Encoding.Default.GetBytes(uncodeString) : Encoding.GetEncoding(1251).GetBytes(uncodeString);
                    byte[] bytes_out = testEncodeChkBox.Checked ? Encoding.Convert(Encoding.GetEncoding(866), Encoding.Default, bytes_in) : Encoding.Convert(Encoding.GetEncoding(866), Encoding.GetEncoding(1251), bytes_in);
                    codeString = Encoding.GetEncoding(1251).GetString(bytes_out);

                }

            }

            //if (RadMessageBox.Show("Входящая строка  -  " + uncodeString + "\r\nВыходная строка  -  " + codeString + "\r\n\r\nПродолжить?", "Кодировки", MessageBoxButtons.YesNo) == DialogResult.No)
            //{
            //    bw.CancelAsync();
            //}

            return codeString.Trim();
        }

        /// Проверка на существование каталога
        private bool IsDirectoryExist(string directory)
        {
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo dInfo = new DirectoryInfo(path);
            try
            {
                DirectoryInfo[] directories = dInfo.GetDirectories();

                for (int i = 0; i < directories.Count(); i++)
                {
                    if (directories[i].Name == (directory))
                    {
                        return directories[i].Name == directory;
                    }

                }
            }
            catch
            {
                return false;
            }
            return false;
        }


        //парсинг DBF и запись ее в базу.


        public bool DBFRead(string DBFFileName)
        {
            string message = "";
            string log_filename = "";
            StreamWriter outfile;

            firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));

            if (File.Exists(DBFFileName))
            {
                pu6Entities db = new pu6Entities();
                StringBuilder sb = new StringBuilder();

                string pathLog = Path.Combine(Application.StartupPath, "log");

                if (!Directory.Exists(pathLog))
                {
                    Directory.CreateDirectory(pathLog);
                }


                DataTable dt = GetYourData(DBFFileName);


                if (dt.Columns.Count == 0)
                    return false;

                var result = dt.AsEnumerable().ToList();
                int cnt = result.Count();
                int colcon = dt.Columns.Count;
                int rowcon = dt.Rows.Count;

                secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = result.Count().ToString(); }));

                string insnumber;
                string lastname;
                string firstname;
                string middlename;
                bool fin = false;
                int k = 0;

                var tableName = Path.GetFileNameWithoutExtension(DBFFileName).ToLower();

                if (tableName == "iemp_6")
                {
                    tableName = "iemp_5";
                }
                var tempName = tableName.Contains("_5") ? tableName.Remove(tableName.Length - 2).ToString() : tableName;



                switch (tempName)
                {
                    case "iemp": //Персональные данные
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            DataColumnCollection columns = dt.Columns;
                            bool INNfieldExist = columns.Contains("INN");

                            foreach (DataRow row in result)
                            {

                                string INN = "";
                                if (INNfieldExist && !String.IsNullOrEmpty(row["INN"].ToString().Trim()))
                                {
                                    INN = row["INN"].ToString().Trim().PadLeft(12, '0');
                                }

                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');
                                lastname = Encode(row["FAMIL"].ToString());
                                firstname = Encode(row["IMYA"].ToString());
                                middlename = Encode(row["OTCHES"].ToString());

                                var tabnum = row["TABLNUM"].ToString().Trim();
                                long? tabel = !String.IsNullOrEmpty(tabnum) ? (long.Parse(tabnum)) : (long?)null;
                                tabel = tabel == 0 ? (long?)null : tabel;



                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    Staff staffData = new Staff
                                    {
                                        InsurerID = Options.InsID,
                                        InsuranceNumber = insnumber,
                                        ControlNumber = byte.Parse(!String.IsNullOrEmpty(row["CNUMBER"].ToString()) ? row["CNUMBER"].ToString() : "0"),
                                        TabelNumber = tabel,
                                        INN = INN,
                                        LastName = lastname,
                                        FirstName = firstname,
                                        MiddleName = middlename,
                                        Dismissed = byte.Parse(!String.IsNullOrEmpty(row["DISMYES"].ToString()) ? row["DISMYES"].ToString() : "0"),
                                    };

                                    if (!String.IsNullOrEmpty(row["POL"].ToString().Trim()))
                                    {
                                        var sex = Encode(row["POL"].ToString());

                                        if (sex == "М" || sex == "Ж")
                                        {
                                            staffData.Sex = sex == "Ж" ? (byte)1 : (byte)0;
                                        }
                                    }

                                    if (!String.IsNullOrEmpty(row["POL"].ToString().Trim()))
                                    {
                                        staffData.Sex = Encode(row["POL"].ToString()) == "Ж" ? (byte)1 : (byte)0;
                                    }
                                    if (!String.IsNullOrEmpty(row["DROZHDST"].ToString().Trim()) && DateTime.Parse(row["DROZHDST"].ToString().Trim()).Year >= 1910)
                                    {
                                        staffData.DateBirth = DateTime.Parse(row["DROZHDST"].ToString());
                                    }

                                    db.Staff.Add(staffData);
                                    sb.AppendLine(DateTime.Now.ToString() + "   Сотрудник (" + lastname + " " + firstname + " "
                                        + middlename + ") с номером " + insnumber + " загружен. ");
                                }
                                else
                                {
                                    if (updateStaff_DDL.SelectedItem.Tag.ToString() == "0")
                                    {
                                        Staff staff = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber);
                                        bool change = false;

                                        if (staff.LastName != lastname)
                                        {
                                            staff.LastName = lastname;
                                            change = true;
                                        }
                                        if (staff.FirstName != firstname)
                                        {
                                            staff.FirstName = firstname;
                                            change = true;
                                        }
                                        if (staff.MiddleName != middlename)
                                        {
                                            staff.MiddleName = middlename;
                                            change = true;
                                        }

                                        if (staff.TabelNumber != tabel)
                                        {
                                            staff.TabelNumber = tabel;
                                            change = true;
                                        }

                                        if (staff.INN != INN)
                                        {
                                            staff.INN = INN;
                                            change = true;
                                        }

                                        var Dismissed = byte.Parse(!String.IsNullOrEmpty(row["DISMYES"].ToString()) ? row["DISMYES"].ToString() : "0");
                                        if (staff.Dismissed != Dismissed)
                                        {
                                            staff.Dismissed = Dismissed;
                                            change = true;
                                        }

                                        if (!String.IsNullOrEmpty(row["POL"].ToString().Trim()))
                                        {
                                            var sex = Encode(row["POL"].ToString());

                                            if (sex == "М" || sex == "Ж")
                                            {
                                                var Sex = sex == "Ж" ? (byte)1 : (byte)0;
                                                if (staff.Sex != Sex)
                                                {
                                                    staff.Sex = Sex;
                                                    change = true;
                                                }
                                            }
                                        }

                                        if (!String.IsNullOrEmpty(row["DROZHDST"].ToString().Trim()) && DateTime.Parse(row["DROZHDST"].ToString().Trim()).Year >= 1910)
                                        {
                                            var DateBirth = DateTime.Parse(row["DROZHDST"].ToString());
                                            if (staff.DateBirth != DateBirth)
                                            {
                                                staff.DateBirth = DateBirth;
                                                change = true;
                                            }
                                        }

                                        if (change)
                                        {
                                            db.Entry(staff).State = System.Data.Entity.EntityState.Modified;
                                        }

                                    }
                                    else
                                    {
                                        sb.AppendLine(DateTime.Now.ToString() + "   Сотрудник (" + lastname + " " + firstname + " "
                                            + middlename + ") с номером " + insnumber + " уже существует в системе. ");
                                    }


                                }

                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                if (k % transCnt == 0)
                                {
                                    db.SaveChanges();
                                }

                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по сотрудникам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "idwemp": //Даты приема и увольнения сотрудников
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            foreach (DataRow row in result)
                            {

                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');

                                if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    StaffDateWork newDateStaff = new StaffDateWork { StaffID = staffID };
                                    if (!String.IsNullOrEmpty(row["DBWORKS"].ToString().Trim()) && DateTime.Parse(row["DBWORKS"].ToString().Trim()).Year >= 1910)
                                    {
                                        newDateStaff.DateBeginWork = DateTime.Parse(row["DBWORKS"].ToString().Trim());
                                    }
                                    if (!String.IsNullOrEmpty(row["DEWORKS"].ToString().Trim()) && DateTime.Parse(row["DEWORKS"].ToString().Trim()).Year >= 1910)
                                    {
                                        newDateStaff.DateEndWork = DateTime.Parse(row["DEWORKS"].ToString().Trim());
                                    }

                                    if (!db.StaffDateWork.Any(x => x.StaffID == staffID && (newDateStaff.DateBeginWork.HasValue ? x.DateBeginWork == newDateStaff.DateBeginWork.Value : x.DateBeginWork == null) && (newDateStaff.DateEndWork.HasValue ? x.DateEndWork == newDateStaff.DateEndWork.Value : x.DateEndWork == null)))
                                    {
                                        db.StaffDateWork.Add(newDateStaff);
                                        sb.AppendLine(DateTime.Now.ToString() + "   Сотрудник с номером " + insnumber + ". Данные о датах приема\\увольнения загружены. ");
                                    }
                                    else
                                        sb.AppendLine(DateTime.Now.ToString() + "   Сотрудник с номером " + insnumber + " Данные о датах приема\\увольнения НЕ загружены. Данные уже есть в базе данных. ");

                                }
                                else
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Сотрудник с номером " + insnumber + " не найден в базе данных. ");

                                    continue;
                                }

                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                if (k % transCnt == 0)
                                {
                                    db.SaveChanges();
                                }

                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по сотрудникам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "ipfrosn": //Индивидуальные сведения 2014. Основные выплаты.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var PlatCategoryRaschPer_list = db.PlatCategoryRaschPer.ToList();
                            var PlatCategory_list = db.PlatCategory.ToList();

                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";

                            List<FormsRSW2014_1_Razd_6_1> rsw61List = new List<FormsRSW2014_1_Razd_6_1>();
                            foreach (DataRow row in result)
                            {


                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');

                                int ti = 1;
                                string ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                switch (ti_cod)
                                {
                                    case "ИСХД":
                                        ti = 1;
                                        break;
                                    case "КОРР":
                                        ti = 2;
                                        break;
                                    case "ОТМН":
                                        ti = 3;
                                        break;
                                }

                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;

                                    short.TryParse(row[YEAR].ToString(), out y);
                                    byte.TryParse(row["QUARTER"].ToString(), out q);
                                    short.TryParse(row["YEARKORR"].ToString(), out yk);
                                    byte.TryParse(row["QUARTKORR"].ToString(), out qk);

                                    FormsRSW2014_1_Razd_6_1 razd6 = new FormsRSW2014_1_Razd_6_1 { };

                                    decimal SumFeePFR = 0;
                                    decimal.TryParse(row["SFPFR"].ToString(), out SumFeePFR);

                                    string category = Encode(row["CATEGORY"].ToString().Trim());


                                    if (rsw61List.Any(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.StaffID == staffID))
                                    {
                                        razd6 = rsw61List.FirstOrDefault(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.StaffID == staffID);
                                    }
                                    else if (!db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {

                                        razd6 = new FormsRSW2014_1_Razd_6_1
                                        {
                                            StaffID = staffID,
                                            Year = y,
                                            Quarter = q,
                                            TypeInfoID = ti,
                                            YearKorr = yk,
                                            QuarterKorr = qk,
                                            SumFeePFR = SumFeePFR,
                                            AutoCalc = false,
                                            DateFilling = updateDateFilling.Checked ? DateTime.Now : DateTime.Parse(row["DATAZAP"].ToString()),
                                            InsurerID = Options.InsID,
                                            CorrectionNum = 0
                                        };
                                        rsw61List.Add(razd6);
                                        //db.AddToFormsRSW2014_1_Razd_6_1(razd6);
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + razd6.Year + " " + razd6.Quarter + " " + ti_cod + ". Раздел 6.1. Строка загружена.");
                                    }
                                    else
                                    {
                                        razd6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);

                                        var pcrp_ = PlatCategoryRaschPer_list.First(x => (!x.DateEnd.HasValue && x.DateBegin.Value.Year <= razd6.Year) || (x.DateEnd.HasValue && (x.DateBegin.Value.Year <= razd6.Year && x.DateEnd.Value.Year >= razd6.Year)));
                                        var platcategory_ = PlatCategory_list.FirstOrDefault(x => x.Code == category && x.PlatCategoryRaschPerID == pcrp_.ID);

                                        if (razd6.FormsRSW2014_1_Razd_6_4.Any(x => x.PlatCategoryID == platcategory_.ID)) // Ищем есть ли в базе уже такая запись раздела 6.4
                                        {
                                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "1")  // Пропускаем если выбрано пропускать
                                            {
                                                sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + razd6.Year + " " + razd6.Quarter + " " + ti_cod + ". Дублирование записи по ключу уникальности в разделе 6.4. Категория " + category + ". Строка не загружена.");

                                                k++;
                                                decimal temp_ = (decimal)k / (decimal)cnt;
                                                int proc_ = (int)Math.Round((temp_ * 100), 0);
                                                bw.ReportProgress(proc_, k);
                                                continue;
                                            }

                                            var razd64_t = razd6.FormsRSW2014_1_Razd_6_4.First(x => x.PlatCategoryID == platcategory_.ID);

                                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "0")  // Удаляем запись 6.4 если выбрано заменять
                                            {
                                                db.FormsRSW2014_1_Razd_6_4.Remove(razd64_t);

                                                //                                            db = new pu6Entities();

                                                razd6.SumFeePFR = SumFeePFR;

                                            }
                                            else if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2")
                                            {
                                                razd6.SumFeePFR = (razd6.SumFeePFR.HasValue ? razd6.SumFeePFR.Value : 0) + SumFeePFR;
                                            }

                                            razd6.DateFilling = updateDateFilling.Checked ? DateTime.Now : DateTime.Parse(row["DATAZAP"].ToString());
                                            razd6.AutoCalc = false;
                                            db.Entry(razd6).State =System.Data.Entity.EntityState.Modified;


                                        }

                                    }

                                    var pcrp = PlatCategoryRaschPer_list.FirstOrDefault(x => (!x.DateEnd.HasValue && x.DateBegin.Value.Year <= razd6.Year) || (x.DateEnd.HasValue && (x.DateBegin.Value.Year <= razd6.Year && x.DateEnd.Value.Year >= razd6.Year)));
                                    PlatCategory platcategory = PlatCategory_list.FirstOrDefault(x => x.Code == category && x.PlatCategoryRaschPerID == pcrp.ID);



                                    //if (!db.FormsRSW2014_1_Razd_6_4.Any(x => x.FormsRSW2014_1_Razd_6_1_ID == razd6.ID && x.PlatCategoryID == platcategory.ID))
                                    //{
                                    FormsRSW2014_1_Razd_6_4 razd64 = new FormsRSW2014_1_Razd_6_4
                                    {
                                        PlatCategoryID = platcategory.ID,
                                        s_0_0 = !String.IsNullOrEmpty(row["THRMONT1"].ToString()) ? decimal.Parse(row["THRMONT1"].ToString()) : 0,
                                        s_0_1 = !String.IsNullOrEmpty(row["THRMONT2"].ToString()) ? decimal.Parse(row["THRMONT2"].ToString()) : 0,
                                        s_0_2 = !String.IsNullOrEmpty(row["THRMONT3"].ToString()) ? decimal.Parse(row["THRMONT3"].ToString()) : 0,
                                        s_0_3 = !String.IsNullOrEmpty(row["THRMONT4"].ToString()) ? decimal.Parse(row["THRMONT4"].ToString()) : 0,
                                        s_1_0 = !String.IsNullOrEmpty(row["FRSMONT1"].ToString()) ? decimal.Parse(row["FRSMONT1"].ToString()) : 0,
                                        s_1_1 = !String.IsNullOrEmpty(row["FRSMONT2"].ToString()) ? decimal.Parse(row["FRSMONT2"].ToString()) : 0,
                                        s_1_2 = !String.IsNullOrEmpty(row["FRSMONT3"].ToString()) ? decimal.Parse(row["FRSMONT3"].ToString()) : 0,
                                        s_1_3 = !String.IsNullOrEmpty(row["FRSMONT4"].ToString()) ? decimal.Parse(row["FRSMONT4"].ToString()) : 0,
                                        s_2_0 = !String.IsNullOrEmpty(row["SECMONT1"].ToString()) ? decimal.Parse(row["SECMONT1"].ToString()) : 0,
                                        s_2_1 = !String.IsNullOrEmpty(row["SECMONT2"].ToString()) ? decimal.Parse(row["SECMONT2"].ToString()) : 0,
                                        s_2_2 = !String.IsNullOrEmpty(row["SECMONT3"].ToString()) ? decimal.Parse(row["SECMONT3"].ToString()) : 0,
                                        s_2_3 = !String.IsNullOrEmpty(row["SECMONT4"].ToString()) ? decimal.Parse(row["SECMONT4"].ToString()) : 0,
                                        s_3_0 = !String.IsNullOrEmpty(row["THIMONT1"].ToString()) ? decimal.Parse(row["THIMONT1"].ToString()) : 0,
                                        s_3_1 = !String.IsNullOrEmpty(row["THIMONT2"].ToString()) ? decimal.Parse(row["THIMONT2"].ToString()) : 0,
                                        s_3_2 = !String.IsNullOrEmpty(row["THIMONT3"].ToString()) ? decimal.Parse(row["THIMONT3"].ToString()) : 0,
                                        s_3_3 = !String.IsNullOrEmpty(row["THIMONT4"].ToString()) ? decimal.Parse(row["THIMONT4"].ToString()) : 0
                                        //FormsRSW2014_1_Razd_6_1_ID = razd6.ID
                                    };

                                    if (!razd6.FormsRSW2014_1_Razd_6_4.Any(x => x.PlatCategoryID == razd64.PlatCategoryID))
                                    {
                                        razd6.FormsRSW2014_1_Razd_6_4.Add(razd64);
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + category + ". Раздел 6.4. Строка загружена.");
                                    }
                                    else
                                    {
                                        if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2")
                                        {
                                            var tempR64 = razd6.FormsRSW2014_1_Razd_6_4.First(x => x.PlatCategoryID == razd64.PlatCategoryID);
                                            tempR64.s_0_0 = tempR64.s_0_0 + razd64.s_0_0;
                                            tempR64.s_0_1 = tempR64.s_0_1 + razd64.s_0_1;
                                            tempR64.s_0_2 = tempR64.s_0_2 + razd64.s_0_2;
                                            tempR64.s_0_3 = tempR64.s_0_3 + razd64.s_0_3;
                                            tempR64.s_1_0 = tempR64.s_1_0 + razd64.s_1_0;
                                            tempR64.s_1_1 = tempR64.s_1_1 + razd64.s_1_1;
                                            tempR64.s_1_2 = tempR64.s_1_2 + razd64.s_1_2;
                                            tempR64.s_1_3 = tempR64.s_1_3 + razd64.s_1_3;
                                            tempR64.s_2_0 = tempR64.s_2_0 + razd64.s_2_0;
                                            tempR64.s_2_1 = tempR64.s_2_1 + razd64.s_2_1;
                                            tempR64.s_2_2 = tempR64.s_2_2 + razd64.s_2_2;
                                            tempR64.s_2_3 = tempR64.s_2_3 + razd64.s_2_3;
                                            tempR64.s_3_0 = tempR64.s_3_0 + razd64.s_3_0;
                                            tempR64.s_3_1 = tempR64.s_3_1 + razd64.s_3_1;
                                            tempR64.s_3_2 = tempR64.s_3_2 + razd64.s_3_2;
                                            tempR64.s_3_3 = tempR64.s_3_3 + razd64.s_3_3;
                                        }
                                        else
                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + category + ". Раздел 6.4. Строка НЕ загружена. Строка с такой категорией уже есть.");
                                    }

                                    //                                        db.AddToFormsRSW2014_1_Razd_6_4(razd64);
                                    //                                 db.SaveChanges();
                                    //}
                                    //else
                                    //{
                                    //    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + category + ". Дублирование записи по ключу уникальности (запись существует) в разделе 6.4. Строка не загружена.");
                                    //}


                                }

                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;
                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                if (k % transCnt == 0)
                                {
                                    foreach (var item in rsw61List)
                                    {
                                        db.FormsRSW2014_1_Razd_6_1.Add(item);
                                    }

                                    db.SaveChanges();
                                    rsw61List.Clear();
                                }
                            }

                            foreach (var item in rsw61List)
                            {
                                db.FormsRSW2014_1_Razd_6_1.Add(item);
                            }
                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }

                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "ipfrdop": //Индивидуальные сведения 2014. Выплаты по Дополнительным тарифам.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var SpecOcenkaUslTruda_list = db.SpecOcenkaUslTruda.ToList();

                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";
                            List<FormsRSW2014_1_Razd_6_1> rsw61List = new List<FormsRSW2014_1_Razd_6_1>();

                            foreach (DataRow row in result)
                            {


                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');


                                int ti = 1;
                                string ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                switch (ti_cod)
                                {
                                    case "ИСХД":
                                        ti = 1;
                                        break;
                                    case "КОРР":
                                        ti = 2;
                                        break;
                                    case "ОТМН":
                                        ti = 3;
                                        break;
                                }

                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {

                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;

                                    short.TryParse(row[YEAR].ToString(), out y);
                                    byte.TryParse(row["QUARTER"].ToString(), out q);
                                    short.TryParse(row["YEARKORR"].ToString(), out yk);
                                    byte.TryParse(row["QUARTKORR"].ToString(), out qk);

                                    FormsRSW2014_1_Razd_6_1 razd6 = new FormsRSW2014_1_Razd_6_1 { };

                                    string specocenka = row["JOBEVAL"].ToString().Trim().Length > 0 ? Encode(row["JOBEVAL"].ToString().Trim()) : "";
                                    long? specocenkausltruda = SpecOcenkaUslTruda_list.FirstOrDefault(x => x.Code == specocenka) != null ? db.SpecOcenkaUslTruda.FirstOrDefault(x => x.Code == specocenka).ID : (long?)null;

                                    if (rsw61List.Any(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.StaffID == staffID))
                                    {
                                        razd6 = rsw61List.FirstOrDefault(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.StaffID == staffID);
                                    }
                                    else if (!db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {

                                        razd6 = new FormsRSW2014_1_Razd_6_1
                                        {
                                            StaffID = staffID,
                                            Year = y,
                                            Quarter = q,
                                            TypeInfoID = ti,
                                            YearKorr = yk,
                                            QuarterKorr = qk,
                                            AutoCalc = false,
                                            DateFilling = updateDateFilling.Checked ? DateTime.Now : DateTime.Parse(row["DATAZAP"].ToString()),
                                            InsurerID = Options.InsID,
                                            CorrectionNum = 0
                                        };

                                        //                                        db.FormsRSW2014_1_Razd_6_1.Add(razd6);
                                        rsw61List.Add(razd6);

                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + razd6.Year + " " + razd6.Quarter + " " + ti_cod + ". Раздел 6.1. Строка загружена.");

                                    }
                                    else
                                    {
                                        razd6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);

                                        if (razd6.FormsRSW2014_1_Razd_6_7.Any(x => x.SpecOcenkaUslTrudaID == specocenkausltruda)) // Ищем есть ли в базе уже такая запись раздела 6.7
                                        {
                                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "1")  // Пропускаем если выбрано пропускать
                                            {
                                                sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + razd6.Year + " " + razd6.Quarter + " " + ti_cod + ". Дублирование записи по ключу уникальности в разделе 6.7. Строка не загружена.");

                                                k++;
                                                decimal temp_ = (decimal)k / (decimal)cnt;
                                                int proc_ = (int)Math.Round((temp_ * 100), 0);
                                                bw.ReportProgress(proc_, k);
                                                continue;
                                            }

                                            var razd67_t = razd6.FormsRSW2014_1_Razd_6_7.First(x => x.SpecOcenkaUslTrudaID == specocenkausltruda);

                                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "0")  // Удаляем запись 6.7 если выбрано заменять
                                            {
                                                db.FormsRSW2014_1_Razd_6_7.Remove(razd67_t);
                                            }


                                        }

                                    }


                                    FormsRSW2014_1_Razd_6_7 razd67 = new FormsRSW2014_1_Razd_6_7
                                    {
                                        SpecOcenkaUslTrudaID = specocenkausltruda,
                                        s_0_0 = !String.IsNullOrEmpty(row["THRMONT1"].ToString()) ? decimal.Parse(row["THRMONT1"].ToString()) : 0,
                                        s_0_1 = !String.IsNullOrEmpty(row["THRMONT2"].ToString()) ? decimal.Parse(row["THRMONT2"].ToString()) : 0,
                                        s_1_0 = !String.IsNullOrEmpty(row["FRSMONT1"].ToString()) ? decimal.Parse(row["FRSMONT1"].ToString()) : 0,
                                        s_1_1 = !String.IsNullOrEmpty(row["FRSMONT2"].ToString()) ? decimal.Parse(row["FRSMONT2"].ToString()) : 0,
                                        s_2_0 = !String.IsNullOrEmpty(row["SECMONT1"].ToString()) ? decimal.Parse(row["SECMONT1"].ToString()) : 0,
                                        s_2_1 = !String.IsNullOrEmpty(row["SECMONT2"].ToString()) ? decimal.Parse(row["SECMONT2"].ToString()) : 0,
                                        s_3_0 = !String.IsNullOrEmpty(row["THIMONT1"].ToString()) ? decimal.Parse(row["THIMONT1"].ToString()) : 0,
                                        s_3_1 = !String.IsNullOrEmpty(row["THIMONT2"].ToString()) ? decimal.Parse(row["THIMONT2"].ToString()) : 0
                                    };

                                    if (!razd6.FormsRSW2014_1_Razd_6_7.Any(x => x.SpecOcenkaUslTrudaID == specocenkausltruda))
                                    {
                                        razd6.FormsRSW2014_1_Razd_6_7.Add(razd67);
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + specocenka + ". Раздел 6.7. Строка загружена.");
                                    }
                                    else
                                    {
                                        if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2")
                                        {
                                            var tempR67 = razd6.FormsRSW2014_1_Razd_6_7.First(x => x.SpecOcenkaUslTrudaID == razd67.SpecOcenkaUslTrudaID);
                                            tempR67.s_0_0 = tempR67.s_0_0 + razd67.s_0_0;
                                            tempR67.s_0_1 = tempR67.s_0_1 + razd67.s_0_1;
                                            tempR67.s_1_0 = tempR67.s_1_0 + razd67.s_1_0;
                                            tempR67.s_1_1 = tempR67.s_1_1 + razd67.s_1_1;
                                            tempR67.s_2_0 = tempR67.s_2_0 + razd67.s_2_0;
                                            tempR67.s_2_1 = tempR67.s_2_1 + razd67.s_2_1;
                                            tempR67.s_3_0 = tempR67.s_3_0 + razd67.s_3_0;
                                            tempR67.s_3_1 = tempR67.s_3_1 + razd67.s_3_1;

                                            db.Entry(tempR67).State =System.Data.Entity.EntityState.Modified;
                                        }
                                        else
                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + specocenka + ". Раздел 6.7. Строка НЕ загружена. Строка с такой категорией уже есть.");
                                    }


                                }
                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                if (k % transCnt == 0)
                                {
                                    foreach (var item in rsw61List)
                                    {
                                        db.FormsRSW2014_1_Razd_6_1.Add(item);
                                    }

                                    db.SaveChanges();
                                    rsw61List.Clear();
                                }
                            }
                            foreach (var item in rsw61List)
                            {
                                db.FormsRSW2014_1_Razd_6_1.Add(item);
                            }
                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по выплатам по дополнительным тарифам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "ilospfr": //Индивидуальные сведения 2014. Основные записи о стаже.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";

                            var TerrUsl_list = db.TerrUsl.ToList();
                            var OsobUslTruda_list = db.OsobUslTruda.ToList();
                            var KodVred_2_list = db.KodVred_2.ToList();
                            var KodVred_3_list = db.KodVred_3.ToList();
                            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
                            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
                            var UslDosrNazn_list = db.UslDosrNazn.ToList();

                            var groupList = result.GroupBy(x => new { INSURANCE = x["INSURANCE"], YEAR = x[YEAR], QUARTER = x["QUARTER"], TYPEINFORM = x["TYPEINFORM"], YEARKORR = x["YEARKORR"], QUARTKORR = x["QUARTKORR"] });
                            foreach (var b in groupList)
                            {

                                insnumber = b.Key.INSURANCE.ToString().Trim().PadLeft(9, '0');

                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    int ti = 1;
                                    string ti_cod = Encode(b.Key.TYPEINFORM.ToString().Trim());

                                    switch (ti_cod)
                                    {
                                        case "ИСХД":
                                            ti = 1;
                                            break;
                                        case "КОРР":
                                            ti = 2;
                                            break;
                                        case "ОТМН":
                                            ti = 3;
                                            break;
                                    }

                                    //                                    long razd6_id = 0;
                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;

                                    long n = 1;

                                    short.TryParse(b.Key.YEAR.ToString(), out y);
                                    byte.TryParse(b.Key.QUARTER.ToString(), out q);
                                    short.TryParse(b.Key.YEARKORR.ToString(), out yk);
                                    byte.TryParse(b.Key.QUARTKORR.ToString(), out qk);

                                    FormsRSW2014_1_Razd_6_1 razd6 = new FormsRSW2014_1_Razd_6_1 { };

                                    if (!db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {
                                        razd6 = new FormsRSW2014_1_Razd_6_1
                                        {
                                            StaffID = staffID,
                                            Year = y,
                                            Quarter = q,
                                            TypeInfoID = ti,
                                            YearKorr = yk,
                                            QuarterKorr = qk,
                                            AutoCalc = false,
                                            InsurerID = Options.InsID,
                                            CorrectionNum = 0,
                                            DateFilling = DateTime.Now
                                        };

                                        db.FormsRSW2014_1_Razd_6_1.Add(razd6);
                                        // db.SaveChanges();
                                        //                                        razd6_id = razd6.ID;
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + razd6.Year + " " + razd6.Quarter + " " + ti_cod + ". Раздел 6.1. Строка загружена.");
                                    }
                                    else
                                    {
                                        razd6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);

                                        if (razd6.StajOsn.Count() > 0)
                                        {
                                            if (updateStaj_DDL.SelectedItem.Tag.ToString() == "0")  // заменить существующие записи
                                            {
                                                db.Database.ExecuteSqlCommand(String.Format("DELETE FROM StajOsn WHERE ([FormsRSW2014_1_Razd_6_1_ID] = {0})", razd6.ID));
                                            }
                                            else if (updateStaj_DDL.SelectedItem.Tag.ToString() == "1")  // не импортировать, пропускаем
                                            {
                                                k++;
                                                decimal temp_ = (decimal)k / (decimal)cnt;
                                                int proc_ = (int)Math.Round((temp_ * 100), 0);
                                                bw.ReportProgress(proc_, k);
                                                continue;
                                            }
                                            else if (updateStaj_DDL.SelectedItem.Tag.ToString() == "2")  // объеденить
                                            {
                                                n = razd6.StajOsn.Max(x => x.Number.Value);
                                            }
                                        }
                                    }


                                    foreach (DataRow row in b)
                                    {
                                        if (bw.CancellationPending)
                                        {
                                            DBFMessage = "Выполнение операции отменено!";
                                            return false;
                                        }

                                        DateTime dateBegin = DateTime.Parse(row["BPERIOD"].ToString());
                                        DateTime dateEnd = DateTime.Parse(row["EPERIOD"].ToString());

                                        StajOsn stajosn = new StajOsn
                                        {
                                            Number = updateStaj_DDL.SelectedItem.Tag.ToString() == "0" ? long.Parse(!String.IsNullOrEmpty(row["PRNUMBER"].ToString()) ? row["PRNUMBER"].ToString() : "0") : n,
                                            DateBegin = dateBegin,
                                            DateEnd = dateEnd
                                        };

                                        razd6.StajOsn.Add(stajosn);

                                        //                                        db.StajOsn.Add(stajosn);
                                        //                                        db.SaveChanges();
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Строка загружена.");

                                        n++;

                                        #region  Информация о льготном стаже в строке об основном


                                        string terrcondit = row["TERRCONDIT"].ToString().Trim().Length > 0 ? Encode(row["TERRCONDIT"].ToString().Trim()) : "";
                                        long? terrusl = TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit) != null ? TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit).ID : (long?)null;

                                        string scwork = row["SCWORK"].ToString().Trim().Length > 0 ? Encode(row["SCWORK"].ToString().Trim()) : "";
                                        long? osobusltruda = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == scwork.ToUpper()) != null ? OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == scwork.ToUpper()).ID : (long?)null;

                                        long? kodvred2 = null;
                                        if (scwork == "27-1" || scwork == "27-2")
                                        {
                                            string positionl = row["POSITIONL"].ToString().Trim().Length > 0 ? Encode(row["POSITIONL"].ToString().Trim()) : "";
                                            kodvred2 = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == positionl.ToUpper()) != null ? KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == positionl.ToUpper()).ID : (long?)null;
                                        }

                                        string profession = "";
                                        long? dolgn1 = (long?)null;
                                        if (row["PROFESSION"].ToString().Trim().Length > 0)
                                        {
                                            profession = row["PROFESSION"].ToString().Trim().Length > 0 ? Encode(row["PROFESSION"].ToString().Trim()) : "";
                                            if (!db.Dolgn.Any(x => x.Name == profession))
                                            {
                                                Dolgn dolgn = new Dolgn
                                                {
                                                    Name = profession
                                                };
                                                db.Dolgn.Add(dolgn);
                                                db.SaveChanges();
                                                dolgn1 = dolgn.ID;
                                            }
                                            else
                                            {
                                                dolgn1 = db.Dolgn.FirstOrDefault(x => x.Name == profession).ID;
                                            }
                                        }

                                        string basisexp = row["BASISEXP"].ToString().Trim().Length > 0 ? Encode(row["BASISEXP"].ToString().Trim()) : "";
                                        long? ischislstrahstajosn = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp) != null ? IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp).ID : (long?)null;

                                        string aiexp3 = row["AIEXP3"].ToString().Trim().Length > 0 ? Encode(row["AIEXP3"].ToString().Trim()) : "";
                                        long? ischislstrahstajdop = IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3) != null ? IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3).ID : (long?)null;

                                        string basisyear = row["BASISYEAR"].ToString().Trim().Length > 0 ? Encode(row["BASISYEAR"].ToString().Trim()) : "";
                                        long? usldosrnazn = UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear) != null ? UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear).ID : (long?)null;

                                        if (terrusl != null || osobusltruda != null || kodvred2 != null || dolgn1 != null || ischislstrahstajosn != null || ischislstrahstajdop != null || usldosrnazn != null)
                                        {

                                            StajLgot stajlgot = new StajLgot
                                            {
                                                //                                                StajOsnID = stajosn.ID,
                                                Number = 1,
                                                KodVred_OsnID = kodvred2,
                                                TerrUslID = terrusl,
                                                TerrUslKoef = decimal.Parse(!String.IsNullOrEmpty(row["RKOFF"].ToString()) ? row["RKOFF"].ToString() : "0"),
                                                OsobUslTrudaID = osobusltruda,
                                                DolgnID = dolgn1,
                                                IschislStrahStajOsnID = ischislstrahstajosn,
                                                IschislStrahStajDopID = ischislstrahstajdop,
                                                Strah1Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP1"].ToString()) ? row["AIEXP1"].ToString() : "0"),
                                                Strah2Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP2"].ToString()) ? row["AIEXP2"].ToString() : "0"),
                                                UslDosrNaznID = usldosrnazn,
                                                UslDosrNazn1Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR1"].ToString()) ? row["AIYEAR1"].ToString() : "0"),
                                                UslDosrNazn2Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR2"].ToString()) ? row["AIYEAR2"].ToString() : "0"),
                                                UslDosrNazn3Param = decimal.Parse(!String.IsNullOrEmpty(row["AIYEAR3"].ToString()) ? row["AIYEAR3"].ToString() : "0")
                                            };

                                            stajosn.StajLgot.Add(stajlgot);
                                            //                                            db.StajLgot.Add(stajlgot);
                                            //db.SaveChanges();
                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж льготный. Строка загружена.");
                                        }

                                        #endregion

                                        if (bw.CancellationPending)
                                        {
                                            DBFMessage = "Выполнение операции отменено!";
                                            db.SaveChanges();
                                            return false;
                                        }


                                        k++;

                                        decimal temp = (decimal)k / (decimal)cnt;
                                        int proc = (int)Math.Round((temp * 100), 0);
                                        bw.ReportProgress(proc, k);

                                        if (k % transCnt == 0)
                                        {
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "ielospfr": //Индивидуальные сведения 2014. Дополнительные сведения о льготном стаже.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var TerrUsl_list = db.TerrUsl.ToList();
                            var OsobUslTruda_list = db.OsobUslTruda.ToList();
                            var KodVred_2_list = db.KodVred_2.ToList();
                            var KodVred_3_list = db.KodVred_3.ToList();
                            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
                            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
                            var UslDosrNazn_list = db.UslDosrNazn.ToList();

                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";

                            foreach (DataRow row in result)
                            {

                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');


                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    int ti = 1;
                                    string ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                    switch (ti_cod)
                                    {
                                        case "ИСХД":
                                            ti = 1;
                                            break;
                                        case "КОРР":
                                            ti = 2;
                                            break;
                                        case "ОТМН":
                                            ti = 3;
                                            break;
                                    }

                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;
                                    long n = 1;

                                    short.TryParse(row[YEAR].ToString(), out y);
                                    byte.TryParse(row["QUARTER"].ToString(), out q);
                                    short.TryParse(row["YEARKORR"].ToString(), out yk);
                                    byte.TryParse(row["QUARTKORR"].ToString(), out qk);


                                    FormsRSW2014_1_Razd_6_1 razd6 = new FormsRSW2014_1_Razd_6_1 { };

                                    if (!db.FormsRSW2014_1_Razd_6_1.Any(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Отсутствует запись по данному сотруднику. Сначало необходимо загрузить сведения об основном стаже.");

                                        k++;
                                        decimal temp_ = (decimal)k / (decimal)cnt;
                                        int proc_ = (int)Math.Round((temp_ * 100), 0);
                                        bw.ReportProgress(proc_, k);
                                        continue;
                                    }
                                    else
                                    {
                                        razd6 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.Year == y && x.Quarter == q && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);
                                    }


                                    byte PRNUMBER = 0;
                                    byte.TryParse(row["PRNUMBER"].ToString(), out PRNUMBER);

                                    if (!razd6.StajOsn.Any(x => x.Number == PRNUMBER))
                                    {
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Отсутствует запись по данному сотруднику. Сначало необходимо загрузить сведения об основном стаже.");
                                    }
                                    else
                                    {
                                        StajOsn stajosn = razd6.StajOsn.First(x => x.Number == PRNUMBER);

                                        if (stajosn.StajLgot.Count() > 0)
                                        {
                                            n = stajosn.StajLgot.Max(x => x.Number.Value) + 1;
                                        }

                                        string terrcondit = row["TERRCONDIT"].ToString().Trim().Length > 0 ? Encode(row["TERRCONDIT"].ToString().Trim()) : "";
                                        long? terrusl = TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit) != null ? TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit).ID : (long?)null;

                                        string scwork = row["SCWORK"].ToString().Trim().Length > 0 ? Encode(row["SCWORK"].ToString().Trim()) : "";
                                        long? osobusltruda = OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork) != null ? OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork).ID : (long?)null;

                                        long? kodvred2 = null;
                                        if (scwork == "27-1" || scwork == "27-2")
                                        {
                                            string positionl = row["POSITIONL"].ToString().Trim().Length > 0 ? Encode(row["POSITIONL"].ToString().Trim()) : "";
                                            kodvred2 = KodVred_2_list.FirstOrDefault(x => x.Code == positionl) != null ? KodVred_2_list.FirstOrDefault(x => x.Code == positionl).ID : (long?)null;
                                        }

                                        string profession = "";
                                        long? dolgn1 = (long?)null;
                                        if (row["PROFESSION"].ToString().Trim().Length > 0)
                                        {
                                            profession = row["PROFESSION"].ToString().Trim().Length > 0 ? Encode(row["PROFESSION"].ToString().Trim()) : "";
                                            if (!db.Dolgn.Any(x => x.Name == profession))
                                            {
                                                Dolgn dolgn = new Dolgn
                                                {
                                                    Name = profession
                                                };
                                                db.Dolgn.Add(dolgn);
                                                db.SaveChanges();
                                                dolgn1 = dolgn.ID;
                                            }
                                            else
                                            {
                                                dolgn1 = db.Dolgn.FirstOrDefault(x => x.Name == profession).ID;
                                            }
                                        }

                                        string basisexp = row["BASISEXP"].ToString().Trim().Length > 0 ? Encode(row["BASISEXP"].ToString().Trim()) : "";
                                        long? ischislstrahstajosn = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp) != null ? IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp).ID : (long?)null;

                                        string aiexp3 = row["AIEXP3"].ToString().Trim().Length > 0 ? Encode(row["AIEXP3"].ToString().Trim()) : "";
                                        long? ischislstrahstajdop = IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3) != null ? IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3).ID : (long?)null;

                                        string basisyear = row["BASISYEAR"].ToString().Trim().Length > 0 ? Encode(row["BASISYEAR"].ToString().Trim()) : "";
                                        long? usldosrnazn = UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear) != null ? UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear).ID : (long?)null;

                                        if (terrusl != null || osobusltruda != null || kodvred2 != null || dolgn1 != null || ischislstrahstajosn != null || ischislstrahstajdop != null || usldosrnazn != null)
                                        {

                                            StajLgot stajlgot = new StajLgot
                                            {
                                                //                                                StajOsnID = stajosn.ID,
                                                Number = n,
                                                KodVred_OsnID = kodvred2,
                                                TerrUslID = terrusl,
                                                TerrUslKoef = decimal.Parse(!String.IsNullOrEmpty(row["RKOFF"].ToString()) ? row["RKOFF"].ToString() : "0"),
                                                OsobUslTrudaID = osobusltruda,
                                                DolgnID = dolgn1,
                                                IschislStrahStajOsnID = ischislstrahstajosn,
                                                IschislStrahStajDopID = ischislstrahstajdop,
                                                Strah1Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP1"].ToString()) ? row["AIEXP1"].ToString() : "0"),
                                                Strah2Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP2"].ToString()) ? row["AIEXP2"].ToString() : "0"),
                                                UslDosrNaznID = usldosrnazn,
                                                UslDosrNazn1Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR1"].ToString()) ? row["AIYEAR1"].ToString() : "0"),
                                                UslDosrNazn2Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR2"].ToString()) ? row["AIYEAR2"].ToString() : "0"),
                                                UslDosrNazn3Param = decimal.Parse(!String.IsNullOrEmpty(row["AIYEAR3"].ToString()) ? row["AIYEAR3"].ToString() : "0")
                                            };

                                            stajosn.StajLgot.Add(stajlgot);
                                            //                                            db.StajLgot.Add(stajlgot);
                                            //db.SaveChanges();
                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж льготный. Строка загружена.");
                                        }

                                    }
                                }
                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                if (k % transCnt == 0)
                                {
                                    db.SaveChanges();
                                }
                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "iczw6": //Индивидуальные сведения 2010-2012. Основные выплаты.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var PlatCategoryRaschPer_list = db.PlatCategoryRaschPer.ToList();
                            var PlatCategory_list = db.PlatCategory.ToList();
                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";


                            foreach (DataRow row in result)
                            {
                                if (row["TYPEFORMS"].ToString().Trim() == "1")  // Если форма СПВ-1 то дальше не разбираем
                                {
                                    continue;
                                }

                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');

                                int ti = 1;
                                string ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                switch (ti_cod)
                                {
                                    case "ИСХД":
                                        ti = 1;
                                        break;
                                    case "КОРР":
                                        ti = 2;
                                        break;
                                    case "ОТМН":
                                        ti = 3;
                                        break;
                                }

                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;

                                    short.TryParse(row[YEAR].ToString(), out y);
                                    byte.TryParse(row["QUARTER"].ToString(), out q);
                                    short.TryParse(row["YEARKORR"].ToString(), out yk);
                                    byte.TryParse(row["QUARTKORR"].ToString(), out qk);

                                    string category = Encode(row["CATEGORY"].ToString().Trim());
                                    var pcrp = PlatCategoryRaschPer_list.FirstOrDefault(x => (!x.DateEnd.HasValue && x.DateBegin.Value.Year <= y) || (x.DateEnd.HasValue && (x.DateBegin.Value.Year <= y && x.DateEnd.Value.Year >= y)));

                                    PlatCategory platcategory = PlatCategory_list.FirstOrDefault(x => x.Code == category && x.PlatCategoryRaschPerID == pcrp.ID);


                                    if (db.FormsSZV_6.Any(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {
                                        var szv6ForDel = db.FormsSZV_6.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);

                                        try
                                        {
                                            db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsSZV_6 WHERE ([ID] = {0})", szv6ForDel.ID));
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }


                                    FormsSZV_6 szv6 = new FormsSZV_6
                                    {
                                        StaffID = staffID,
                                        Year = y,
                                        Quarter = q,
                                        TypeInfoID = ti,
                                        YearKorr = yk,
                                        QuarterKorr = qk,
                                        AutoCalc = false,
                                        DateFilling = updateDateFilling.Checked ? DateTime.Now : DateTime.Parse(row["DATAZAP"].ToString()),
                                        InsurerID = Options.InsID,
                                        PlatCategoryID = platcategory.ID,
                                        SUMTAXYEAR = !String.IsNullOrEmpty(row["STY"].ToString()) ? decimal.Parse(row["STY"].ToString()) : 0,
                                        SumFeePFR_Strah = !String.IsNullOrEmpty(row["SFPFRI"].ToString()) ? decimal.Parse(row["SFPFRI"].ToString()) : 0,
                                        SumPayPFR_Strah = !String.IsNullOrEmpty(row["SPPFRI"].ToString()) ? decimal.Parse(row["SPPFRI"].ToString()) : 0,
                                        SumFeePFR_Nakop = !String.IsNullOrEmpty(row["SFPFRA"].ToString()) ? decimal.Parse(row["SFPFRA"].ToString()) : 0,
                                        SumPayPFR_Nakop = !String.IsNullOrEmpty(row["SPPFRA"].ToString()) ? decimal.Parse(row["SPPFRA"].ToString()) : 0,
                                        s_1_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["JANUARY"].ToString()) ? decimal.Parse(row["JANUARY"].ToString()) : 0) : 0,
                                        s_1_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["JANUARYB"].ToString()) ? decimal.Parse(row["JANUARYB"].ToString()) : 0) : 0,
                                        s_2_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["FEBRUARY"].ToString()) ? decimal.Parse(row["FEBRUARY"].ToString()) : 0) : 0,
                                        s_2_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["FEBRUARYB"].ToString()) ? decimal.Parse(row["FEBRUARYB"].ToString()) : 0) : 0,
                                        s_3_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["MARCH"].ToString()) ? decimal.Parse(row["MARCH"].ToString()) : 0) : 0,
                                        s_3_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["MARCHB"].ToString()) ? decimal.Parse(row["MARCHB"].ToString()) : 0) : 0,
                                        s_4_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["APRIL"].ToString()) ? decimal.Parse(row["APRIL"].ToString()) : 0) : 0,
                                        s_4_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["APRILB"].ToString()) ? decimal.Parse(row["APRILB"].ToString()) : 0) : 0,
                                        s_5_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["MAY"].ToString()) ? decimal.Parse(row["MAY"].ToString()) : 0) : 0,
                                        s_5_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["MAYB"].ToString()) ? decimal.Parse(row["MAYB"].ToString()) : 0) : 0,
                                        s_6_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["JUNY"].ToString()) ? decimal.Parse(row["JUNY"].ToString()) : 0) : 0,
                                        s_6_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["JUNYB"].ToString()) ? decimal.Parse(row["JUNYB"].ToString()) : 0) : 0,
                                        s_7_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["JULY"].ToString()) ? decimal.Parse(row["JULY"].ToString()) : 0) : 0,
                                        s_7_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["JULYB"].ToString()) ? decimal.Parse(row["JULYB"].ToString()) : 0) : 0,
                                        s_8_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["AUGUST"].ToString()) ? decimal.Parse(row["AUGUST"].ToString()) : 0) : 0,
                                        s_8_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["AUGUSTB"].ToString()) ? decimal.Parse(row["AUGUSTB"].ToString()) : 0) : 0,
                                        s_9_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["SEPTEMBER"].ToString()) ? decimal.Parse(row["SEPTEMBER"].ToString()) : 0) : 0,
                                        s_9_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["SEPTEMBERB"].ToString()) ? decimal.Parse(row["SEPTEMBERB"].ToString()) : 0) : 0,
                                        s_10_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["OKTOBER"].ToString()) ? decimal.Parse(row["OKTOBER"].ToString()) : 0) : 0,
                                        s_10_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["OKTOBERB"].ToString()) ? decimal.Parse(row["OKTOBERB"].ToString()) : 0) : 0,
                                        s_11_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["NOVEMBER"].ToString()) ? decimal.Parse(row["NOVEMBER"].ToString()) : 0) : 0,
                                        s_11_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["NOVEMBERB"].ToString()) ? decimal.Parse(row["NOVEMBERB"].ToString()) : 0) : 0,
                                        s_12_0 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["DECEMBER"].ToString()) ? decimal.Parse(row["DECEMBER"].ToString()) : 0) : 0,
                                        s_12_1 = columns.Contains("JANUARY") ? (!String.IsNullOrEmpty(row["DECEMBERB"].ToString()) ? decimal.Parse(row["DECEMBERB"].ToString()) : 0) : 0
                                    };

                                    db.FormsSZV_6.Add(szv6);

                                    //                                        db.AddToFormsRSW2014_1_Razd_6_4(razd64);
                                    //                                 db.SaveChanges();
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + category + ". Форма СЗВ-6. Строка загружена.");
                                    //}
                                    //else
                                    //{
                                    //    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + category + ". Дублирование записи по ключу уникальности (запись существует) в разделе 6.4. Строка не загружена.");
                                    //}
                                }

                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                if (k % transCnt == 0)
                                {
                                    db.SaveChanges();
                                }
                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }

                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "ilos6": //Индивидуальные сведения 2010-2012. Основные записи о стаже.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var TerrUsl_list = db.TerrUsl.ToList();
                            var OsobUslTruda_list = db.OsobUslTruda.ToList();
                            var KodVred_2_list = db.KodVred_2.ToList();
                            var KodVred_3_list = db.KodVred_3.ToList();
                            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
                            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
                            var UslDosrNazn_list = db.UslDosrNazn.ToList();
                            var PlatCategoryRaschPer_list = db.PlatCategoryRaschPer.ToList();
                            var PlatCategory_list = db.PlatCategory.ToList();

                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";

                            var groupList = result.GroupBy(x => new { INSURANCE = x["INSURANCE"], TYPEFORMS = x["TYPEFORMS"], YEAR = x[YEAR], QUARTER = x["QUARTER"], TYPEINFORM = x["TYPEINFORM"], YEARKORR = x["YEARKORR"], QUARTKORR = x["QUARTKORR"], CATEGORY = x["CATEGORY"] });
                            foreach (var b in groupList)
                            {
                                if (b.Key.TYPEFORMS.ToString().Trim() == "1")  // Если форма СПВ-1 то дальше не разбираем
                                {
                                    continue;
                                }

                                insnumber = b.Key.INSURANCE.ToString().Trim().PadLeft(9, '0');

                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    int ti = 1;
                                    string ti_cod = Encode(b.Key.TYPEINFORM.ToString().Trim());

                                    switch (ti_cod)
                                    {
                                        case "ИСХД":
                                            ti = 1;
                                            break;
                                        case "КОРР":
                                            ti = 2;
                                            break;
                                        case "ОТМН":
                                            ti = 3;
                                            break;
                                    }

                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;

                                    short.TryParse(b.Key.YEAR.ToString(), out y);
                                    byte.TryParse(b.Key.QUARTER.ToString(), out q);
                                    short.TryParse(b.Key.YEARKORR.ToString(), out yk);
                                    byte.TryParse(b.Key.QUARTKORR.ToString(), out qk);

                                    string category = Encode(b.Key.CATEGORY.ToString().Trim());
                                    var pcrp = PlatCategoryRaschPer_list.FirstOrDefault(x => (!x.DateEnd.HasValue && x.DateBegin.Value.Year <= y) || (x.DateEnd.HasValue && (x.DateBegin.Value.Year <= y && x.DateEnd.Value.Year >= y)));

                                    PlatCategory platcategory = PlatCategory_list.FirstOrDefault(x => x.Code == category && x.PlatCategoryRaschPerID == pcrp.ID);



                                    FormsSZV_6 szv6 = new FormsSZV_6 { };

                                    if (!db.FormsSZV_6.Any(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {
                                        szv6 = new FormsSZV_6
                                        {
                                            StaffID = staffID,
                                            Year = y,
                                            Quarter = q,
                                            TypeInfoID = ti,
                                            YearKorr = yk,
                                            QuarterKorr = qk,
                                            AutoCalc = false,
                                            InsurerID = Options.InsID,
                                            PlatCategoryID = platcategory.ID,
                                            DateFilling = DateTime.Now,
                                            SUMTAXYEAR = 0,
                                            SumFeePFR_Strah = 0,
                                            SumPayPFR_Strah = 0,
                                            SumFeePFR_Nakop = 0,
                                            SumPayPFR_Nakop = 0
                                        };

                                        db.FormsSZV_6.Add(szv6);
                                        // db.SaveChanges();
                                        //                                        razd6_id = razd6.ID;
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + szv6.Year + " " + szv6.Quarter + " " + ti_cod + ". Форма СЗВ-6. Строка загружена.");
                                    }
                                    else
                                    {
                                        szv6 = db.FormsSZV_6.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);

                                        if (szv6.StajOsn.Count() > 0)
                                        {
                                            db.Database.ExecuteSqlCommand(String.Format("DELETE FROM StajOsn WHERE ([FormsSZV_6_ID] = {0})", szv6.ID));
                                        }
                                    }


                                    foreach (DataRow row in b)
                                    {
                                        if (bw.CancellationPending)
                                        {
                                            DBFMessage = "Выполнение операции отменено!";
                                            return false;
                                        }

                                        DateTime dateBegin = DateTime.Parse(row["BPERIOD"].ToString());
                                        DateTime dateEnd = DateTime.Parse(row["EPERIOD"].ToString());

                                        StajOsn stajosn = new StajOsn
                                        {
                                            Number = long.Parse(!String.IsNullOrEmpty(row["PRNUMBER"].ToString()) ? row["PRNUMBER"].ToString() : "0"),
                                            DateBegin = dateBegin,
                                            DateEnd = dateEnd
                                        };

                                        szv6.StajOsn.Add(stajosn);

                                        //                                        db.StajOsn.Add(stajosn);
                                        //                                        db.SaveChanges();
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Строка загружена.");

                                        #region  Информация о льготном стаже в строке об основном


                                        string terrcondit = row["TERRCONDIT"].ToString().Trim().Length > 0 ? Encode(row["TERRCONDIT"].ToString().Trim()) : "";
                                        long? terrusl = TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit) != null ? TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit).ID : (long?)null;

                                        string scwork = row["SCWORK"].ToString().Trim().Length > 0 ? Encode(row["SCWORK"].ToString().Trim()) : "";
                                        long? osobusltruda = OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork) != null ? OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork).ID : (long?)null;

                                        long? kodvred2 = null;
                                        if (scwork == "27-1" || scwork == "27-2")
                                        {
                                            string positionl = row["POSITIONL"].ToString().Trim().Length > 0 ? Encode(row["POSITIONL"].ToString().Trim()) : "";
                                            kodvred2 = KodVred_2_list.FirstOrDefault(x => x.Code == positionl) != null ? KodVred_2_list.FirstOrDefault(x => x.Code == positionl).ID : (long?)null;
                                        }

                                        string profession = "";
                                        long? dolgn1 = (long?)null;
                                        if (row["PROFESSION"].ToString().Trim().Length > 0)
                                        {
                                            profession = row["PROFESSION"].ToString().Trim().Length > 0 ? Encode(row["PROFESSION"].ToString().Trim()) : "";
                                            if (!db.Dolgn.Any(x => x.Name == profession))
                                            {
                                                Dolgn dolgn = new Dolgn
                                                {
                                                    Name = profession
                                                };
                                                db.Dolgn.Add(dolgn);
                                                db.SaveChanges();
                                                dolgn1 = dolgn.ID;
                                            }
                                            else
                                            {
                                                dolgn1 = db.Dolgn.FirstOrDefault(x => x.Name == profession).ID;
                                            }
                                        }

                                        string basisexp = row["BASISEXP"].ToString().Trim().Length > 0 ? Encode(row["BASISEXP"].ToString().Trim()) : "";
                                        long? ischislstrahstajosn = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp) != null ? IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp).ID : (long?)null;

                                        string aiexp3 = row["AIEXP3"].ToString().Trim().Length > 0 ? Encode(row["AIEXP3"].ToString().Trim()) : "";
                                        long? ischislstrahstajdop = IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3) != null ? IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3).ID : (long?)null;

                                        string basisyear = row["BASISYEAR"].ToString().Trim().Length > 0 ? Encode(row["BASISYEAR"].ToString().Trim()) : "";
                                        long? usldosrnazn = UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear) != null ? UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear).ID : (long?)null;

                                        if (terrusl != null || osobusltruda != null || kodvred2 != null || dolgn1 != null || ischislstrahstajosn != null || ischislstrahstajdop != null || usldosrnazn != null)
                                        {

                                            StajLgot stajlgot = new StajLgot
                                            {
                                                //                                                StajOsnID = stajosn.ID,
                                                Number = 1,
                                                KodVred_OsnID = kodvred2,
                                                TerrUslID = terrusl,
                                                TerrUslKoef = decimal.Parse(!String.IsNullOrEmpty(row["RKOFF"].ToString()) ? row["RKOFF"].ToString() : "0"),
                                                OsobUslTrudaID = osobusltruda,
                                                DolgnID = dolgn1,
                                                IschislStrahStajOsnID = ischislstrahstajosn,
                                                IschislStrahStajDopID = ischislstrahstajdop,
                                                Strah1Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP1"].ToString()) ? row["AIEXP1"].ToString() : "0"),
                                                Strah2Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP2"].ToString()) ? row["AIEXP2"].ToString() : "0"),
                                                UslDosrNaznID = usldosrnazn,
                                                UslDosrNazn1Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR1"].ToString()) ? row["AIYEAR1"].ToString() : "0"),
                                                UslDosrNazn2Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR2"].ToString()) ? row["AIYEAR2"].ToString() : "0"),
                                                UslDosrNazn3Param = decimal.Parse(!String.IsNullOrEmpty(row["AIYEAR3"].ToString()) ? row["AIYEAR3"].ToString() : "0")
                                            };

                                            stajosn.StajLgot.Add(stajlgot);
                                            //                                            db.StajLgot.Add(stajlgot);
                                            //db.SaveChanges();
                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж льготный. Строка загружена.");
                                        }

                                        #endregion

                                        if (bw.CancellationPending)
                                        {
                                            DBFMessage = "Выполнение операции отменено!";
                                            db.SaveChanges();
                                            return false;
                                        }


                                        k++;

                                        decimal temp = (decimal)k / (decimal)cnt;
                                        int proc = (int)Math.Round((temp * 100), 0);
                                        bw.ReportProgress(proc, k);

                                        if (k % transCnt == 0)
                                        {
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "ielos6": //Индивидуальные сведения 2010-2012. Дополнительные сведения о льготном стаже.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var TerrUsl_list = db.TerrUsl.ToList();
                            var OsobUslTruda_list = db.OsobUslTruda.ToList();
                            var KodVred_2_list = db.KodVred_2.ToList();
                            var KodVred_3_list = db.KodVred_3.ToList();
                            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
                            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
                            var UslDosrNazn_list = db.UslDosrNazn.ToList();
                            var PlatCategoryRaschPer_list = db.PlatCategoryRaschPer.ToList();
                            var PlatCategory_list = db.PlatCategory.ToList();

                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";

                            foreach (DataRow row in result)
                            {
                                if (result.First()["TYPEFORMS"].ToString().Trim() == "1")  // Если форма СПВ-1 то дальше не разбираем
                                {
                                    continue;
                                }

                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');


                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    int ti = 1;
                                    string ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                    switch (ti_cod)
                                    {
                                        case "ИСХД":
                                            ti = 1;
                                            break;
                                        case "КОРР":
                                            ti = 2;
                                            break;
                                        case "ОТМН":
                                            ti = 3;
                                            break;
                                    }

                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;
                                    long n = 1;

                                    short.TryParse(row[YEAR].ToString(), out y);
                                    byte.TryParse(row["QUARTER"].ToString(), out q);
                                    short.TryParse(row["YEARKORR"].ToString(), out yk);
                                    byte.TryParse(row["QUARTKORR"].ToString(), out qk);

                                    FormsSZV_6 szv6 = new FormsSZV_6 { };

                                    string category = Encode(row["CATEGORY"].ToString().Trim());
                                    var pcrp = PlatCategoryRaschPer_list.FirstOrDefault(x => (!x.DateEnd.HasValue && x.DateBegin.Value.Year <= y) || (x.DateEnd.HasValue && (x.DateBegin.Value.Year <= y && x.DateEnd.Value.Year >= y)));

                                    PlatCategory platcategory = PlatCategory_list.FirstOrDefault(x => x.Code == category && x.PlatCategoryRaschPerID == pcrp.ID);

                                    if (!db.FormsSZV_6.Any(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Отсутствует запись по данному сотруднику. Сначало необходимо загрузить сведения об основном стаже.");

                                        k++;
                                        decimal temp_ = (decimal)k / (decimal)cnt;
                                        int proc_ = (int)Math.Round((temp_ * 100), 0);
                                        bw.ReportProgress(proc_, k);
                                        continue;
                                    }
                                    else
                                    {
                                        szv6 = db.FormsSZV_6.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);
                                    }


                                    byte PRNUMBER = 0;
                                    byte.TryParse(row["PRNUMBER"].ToString(), out PRNUMBER);

                                    if (!szv6.StajOsn.Any(x => x.Number == PRNUMBER))
                                    {
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Отсутствует запись по данному сотруднику. Сначало необходимо загрузить сведения об основном стаже.");
                                    }
                                    else
                                    {
                                        StajOsn stajosn = szv6.StajOsn.First(x => x.Number == PRNUMBER);

                                        if (stajosn.StajLgot.Count() > 0)
                                        {
                                            n = stajosn.StajLgot.Max(x => x.Number.Value) + 1;
                                        }

                                        string terrcondit = row["TERRCONDIT"].ToString().Trim().Length > 0 ? Encode(row["TERRCONDIT"].ToString().Trim()) : "";
                                        long? terrusl = TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit) != null ? TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit).ID : (long?)null;

                                        string scwork = row["SCWORK"].ToString().Trim().Length > 0 ? Encode(row["SCWORK"].ToString().Trim()) : "";
                                        long? osobusltruda = OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork) != null ? OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork).ID : (long?)null;

                                        long? kodvred2 = null;
                                        if (scwork == "27-1" || scwork == "27-2")
                                        {
                                            string positionl = row["POSITIONL"].ToString().Trim().Length > 0 ? Encode(row["POSITIONL"].ToString().Trim()) : "";
                                            kodvred2 = KodVred_2_list.FirstOrDefault(x => x.Code == positionl) != null ? KodVred_2_list.FirstOrDefault(x => x.Code == positionl).ID : (long?)null;
                                        }

                                        string profession = "";
                                        long? dolgn1 = (long?)null;
                                        if (row["PROFESSION"].ToString().Trim().Length > 0)
                                        {
                                            profession = row["PROFESSION"].ToString().Trim().Length > 0 ? Encode(row["PROFESSION"].ToString().Trim()) : "";
                                            if (!db.Dolgn.Any(x => x.Name == profession))
                                            {
                                                Dolgn dolgn = new Dolgn
                                                {
                                                    Name = profession
                                                };
                                                db.Dolgn.Add(dolgn);
                                                db.SaveChanges();
                                                dolgn1 = dolgn.ID;
                                            }
                                            else
                                            {
                                                dolgn1 = db.Dolgn.FirstOrDefault(x => x.Name == profession).ID;
                                            }
                                        }

                                        string basisexp = row["BASISEXP"].ToString().Trim().Length > 0 ? Encode(row["BASISEXP"].ToString().Trim()) : "";
                                        long? ischislstrahstajosn = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp) != null ? IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp).ID : (long?)null;

                                        string aiexp3 = row["AIEXP3"].ToString().Trim().Length > 0 ? Encode(row["AIEXP3"].ToString().Trim()) : "";
                                        long? ischislstrahstajdop = IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3) != null ? IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3).ID : (long?)null;

                                        string basisyear = row["BASISYEAR"].ToString().Trim().Length > 0 ? Encode(row["BASISYEAR"].ToString().Trim()) : "";
                                        long? usldosrnazn = UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear) != null ? UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear).ID : (long?)null;

                                        if (terrusl != null || osobusltruda != null || kodvred2 != null || dolgn1 != null || ischislstrahstajosn != null || ischislstrahstajdop != null || usldosrnazn != null)
                                        {

                                            StajLgot stajlgot = new StajLgot
                                            {
                                                //                                                StajOsnID = stajosn.ID,
                                                Number = n,
                                                KodVred_OsnID = kodvred2,
                                                TerrUslID = terrusl,
                                                TerrUslKoef = decimal.Parse(!String.IsNullOrEmpty(row["RKOFF"].ToString()) ? row["RKOFF"].ToString() : "0"),
                                                OsobUslTrudaID = osobusltruda,
                                                DolgnID = dolgn1,
                                                IschislStrahStajOsnID = ischislstrahstajosn,
                                                IschislStrahStajDopID = ischislstrahstajdop,
                                                Strah1Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP1"].ToString()) ? row["AIEXP1"].ToString() : "0"),
                                                Strah2Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP2"].ToString()) ? row["AIEXP2"].ToString() : "0"),
                                                UslDosrNaznID = usldosrnazn,
                                                UslDosrNazn1Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR1"].ToString()) ? row["AIYEAR1"].ToString() : "0"),
                                                UslDosrNazn2Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR2"].ToString()) ? row["AIYEAR2"].ToString() : "0"),
                                                UslDosrNazn3Param = decimal.Parse(!String.IsNullOrEmpty(row["AIYEAR3"].ToString()) ? row["AIYEAR3"].ToString() : "0")
                                            };

                                            stajosn.StajLgot.Add(stajlgot);
                                            //                                            db.StajLgot.Add(stajlgot);
                                            //db.SaveChanges();
                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж льготный. Строка загружена.");
                                        }

                                    }
                                }
                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                if (k % transCnt == 0)
                                {
                                    db.SaveChanges();
                                }
                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "iczw64": //Индивидуальные сведения 2013. Основные выплаты.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var PlatCategoryRaschPer_list = db.PlatCategoryRaschPer.ToList();
                            var PlatCategory_list = db.PlatCategory.ToList();
                            //       DataColumnCollection columns = dt.Columns;
                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";

                            foreach (DataRow row in result)
                            {
                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');

                                int ti = 1;
                                string ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                switch (ti_cod)
                                {
                                    case "ИСХД":
                                        ti = 1;
                                        break;
                                    case "КОРР":
                                        ti = 2;
                                        break;
                                    case "ОТМН":
                                        ti = 3;
                                        break;
                                }

                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;
                                    byte typeContract = 1;

                                    short.TryParse(row[YEAR].ToString(), out y);
                                    byte.TryParse(row["QUARTER"].ToString(), out q);
                                    short.TryParse(row["YEARKORR"].ToString(), out yk);
                                    byte.TryParse(row["QUARTKORR"].ToString(), out qk);
                                    byte.TryParse(row["TYPECONTRC"].ToString(), out typeContract);

                                    string category = Encode(row["CATEGORY"].ToString().Trim());
                                    var pcrp = PlatCategoryRaschPer_list.FirstOrDefault(x => (!x.DateEnd.HasValue && x.DateBegin.Value.Year <= y) || (x.DateEnd.HasValue && (x.DateBegin.Value.Year <= y && x.DateEnd.Value.Year >= y)));

                                    PlatCategory platcategory = PlatCategory_list.FirstOrDefault(x => x.Code == category && x.PlatCategoryRaschPerID == pcrp.ID);


                                    if (db.FormsSZV_6_4.Any(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && x.TypeContract == typeContract && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {
                                        var szv64ForDel = db.FormsSZV_6_4.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && x.TypeContract == typeContract && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);

                                        try
                                        {
                                            db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsSZV_6_4 WHERE ([ID] = {0})", szv64ForDel.ID));
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                    }


                                    FormsSZV_6_4 szv64 = new FormsSZV_6_4
                                    {
                                        StaffID = staffID,
                                        Year = y,
                                        Quarter = q,
                                        TypeInfoID = ti,
                                        YearKorr = yk,
                                        QuarterKorr = qk,
                                        TypeContract = typeContract,
                                        AutoCalc = false,
                                        DateFilling = updateDateFilling.Checked ? DateTime.Now : DateTime.Parse(row["DATAZAP"].ToString()),
                                        InsurerID = Options.InsID,
                                        PlatCategoryID = platcategory.ID,
                                        SumFeePFR_Strah = !String.IsNullOrEmpty(row["SFPFRI"].ToString()) ? decimal.Parse(row["SFPFRI"].ToString()) : 0,
                                        SumPayPFR_Strah = !String.IsNullOrEmpty(row["SPPFRI"].ToString()) ? decimal.Parse(row["SPPFRI"].ToString()) : 0,
                                        SumFeePFR_Nakop = !String.IsNullOrEmpty(row["SFPFRA"].ToString()) ? decimal.Parse(row["SFPFRA"].ToString()) : 0,
                                        SumPayPFR_Nakop = !String.IsNullOrEmpty(row["SPPFRA"].ToString()) ? decimal.Parse(row["SPPFRA"].ToString()) : 0,
                                        s_0_0 = !String.IsNullOrEmpty(row["THRMONT1"].ToString()) ? decimal.Parse(row["THRMONT1"].ToString()) : 0,
                                        s_0_1 = !String.IsNullOrEmpty(row["THRMONT2"].ToString()) ? decimal.Parse(row["THRMONT2"].ToString()) : 0,
                                        s_0_2 = !String.IsNullOrEmpty(row["THRMONT3"].ToString()) ? decimal.Parse(row["THRMONT3"].ToString()) : 0,
                                        s_1_0 = !String.IsNullOrEmpty(row["FRSMONT1"].ToString()) ? decimal.Parse(row["FRSMONT1"].ToString()) : 0,
                                        s_1_1 = !String.IsNullOrEmpty(row["FRSMONT2"].ToString()) ? decimal.Parse(row["FRSMONT2"].ToString()) : 0,
                                        s_1_2 = !String.IsNullOrEmpty(row["FRSMONT3"].ToString()) ? decimal.Parse(row["FRSMONT3"].ToString()) : 0,
                                        s_2_0 = !String.IsNullOrEmpty(row["SECMONT1"].ToString()) ? decimal.Parse(row["SECMONT1"].ToString()) : 0,
                                        s_2_1 = !String.IsNullOrEmpty(row["SECMONT2"].ToString()) ? decimal.Parse(row["SECMONT2"].ToString()) : 0,
                                        s_2_2 = !String.IsNullOrEmpty(row["SECMONT3"].ToString()) ? decimal.Parse(row["SECMONT3"].ToString()) : 0,
                                        s_3_0 = !String.IsNullOrEmpty(row["THIMONT1"].ToString()) ? decimal.Parse(row["THIMONT1"].ToString()) : 0,
                                        s_3_1 = !String.IsNullOrEmpty(row["THIMONT2"].ToString()) ? decimal.Parse(row["THIMONT2"].ToString()) : 0,
                                        s_3_2 = !String.IsNullOrEmpty(row["THIMONT3"].ToString()) ? decimal.Parse(row["THIMONT3"].ToString()) : 0,
                                        d_0_0 = !String.IsNullOrEmpty(row["THRMONTD1"].ToString()) ? decimal.Parse(row["THRMONTD1"].ToString()) : 0,
                                        d_0_1 = !String.IsNullOrEmpty(row["THRMONTD2"].ToString()) ? decimal.Parse(row["THRMONTD2"].ToString()) : 0,
                                        d_1_0 = !String.IsNullOrEmpty(row["FRSMONTD1"].ToString()) ? decimal.Parse(row["FRSMONTD1"].ToString()) : 0,
                                        d_1_1 = !String.IsNullOrEmpty(row["FRSMONTD2"].ToString()) ? decimal.Parse(row["FRSMONTD2"].ToString()) : 0,
                                        d_2_0 = !String.IsNullOrEmpty(row["SECMONTD1"].ToString()) ? decimal.Parse(row["SECMONTD1"].ToString()) : 0,
                                        d_2_1 = !String.IsNullOrEmpty(row["SECMONTD2"].ToString()) ? decimal.Parse(row["SECMONTD2"].ToString()) : 0,
                                        d_3_0 = !String.IsNullOrEmpty(row["THIMONTD1"].ToString()) ? decimal.Parse(row["THIMONTD1"].ToString()) : 0,
                                        d_3_1 = !String.IsNullOrEmpty(row["THIMONTD2"].ToString()) ? decimal.Parse(row["THIMONTD2"].ToString()) : 0,
                                    };


                                    db.FormsSZV_6_4.Add(szv64);

                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + category + ". Форма СЗВ-6-4. Строка загружена.");
                                }

                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                if (k % transCnt == 0)
                                {
                                    db.SaveChanges();
                                }
                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }

                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "ilos64": //Индивидуальные сведения 2013. Основные записи о стаже.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var TerrUsl_list = db.TerrUsl.ToList();
                            var OsobUslTruda_list = db.OsobUslTruda.ToList();
                            var KodVred_2_list = db.KodVred_2.ToList();
                            var KodVred_3_list = db.KodVred_3.ToList();
                            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
                            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
                            var UslDosrNazn_list = db.UslDosrNazn.ToList();
                            var PlatCategoryRaschPer_list = db.PlatCategoryRaschPer.ToList();
                            var PlatCategory_list = db.PlatCategory.ToList();

                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";

                            var groupList = result.GroupBy(x => new { INSURANCE = x["INSURANCE"], TYPECONTRC = x["TYPECONTRC"], YEAR = x[YEAR], QUARTER = x["QUARTER"], TYPEINFORM = x["TYPEINFORM"], YEARKORR = x["YEARKORR"], QUARTKORR = x["QUARTKORR"], CATEGORY = x["CATEGORY"] });
                            foreach (var b in groupList)
                            {

                                insnumber = b.Key.INSURANCE.ToString().Trim().PadLeft(9, '0');

                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    int ti = 1;
                                    string ti_cod = Encode(b.Key.TYPEINFORM.ToString().Trim());

                                    switch (ti_cod)
                                    {
                                        case "ИСХД":
                                            ti = 1;
                                            break;
                                        case "КОРР":
                                            ti = 2;
                                            break;
                                        case "ОТМН":
                                            ti = 3;
                                            break;
                                    }

                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;
                                    byte typeContract = 1;

                                    short.TryParse(b.Key.YEAR.ToString(), out y);
                                    byte.TryParse(b.Key.QUARTER.ToString(), out q);
                                    short.TryParse(b.Key.YEARKORR.ToString(), out yk);
                                    byte.TryParse(b.Key.QUARTKORR.ToString(), out qk);
                                    byte.TryParse(b.Key.TYPECONTRC.ToString(), out typeContract);

                                    string category = Encode(b.Key.CATEGORY.ToString().Trim());
                                    var pcrp = PlatCategoryRaschPer_list.FirstOrDefault(x => (!x.DateEnd.HasValue && x.DateBegin.Value.Year <= y) || (x.DateEnd.HasValue && (x.DateBegin.Value.Year <= y && x.DateEnd.Value.Year >= y)));

                                    PlatCategory platcategory = PlatCategory_list.FirstOrDefault(x => x.Code == category && x.PlatCategoryRaschPerID == pcrp.ID);



                                    FormsSZV_6_4 szv64 = new FormsSZV_6_4 { };

                                    if (!db.FormsSZV_6_4.Any(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && x.TypeContract == typeContract && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {
                                        szv64 = new FormsSZV_6_4
                                        {
                                            StaffID = staffID,
                                            Year = y,
                                            Quarter = q,
                                            TypeInfoID = ti,
                                            TypeContract = typeContract,
                                            YearKorr = yk,
                                            QuarterKorr = qk,
                                            AutoCalc = false,
                                            InsurerID = Options.InsID,
                                            PlatCategoryID = platcategory.ID,
                                            DateFilling = DateTime.Now,
                                            SumFeePFR_Strah = 0,
                                            SumPayPFR_Strah = 0,
                                            SumFeePFR_Nakop = 0,
                                            SumPayPFR_Nakop = 0
                                        };

                                        db.FormsSZV_6_4.Add(szv64);
                                        // db.SaveChanges();
                                        //                                        razd6_id = razd6.ID;
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + " " + szv64.Year + " " + szv64.Quarter + " " + ti_cod + ". Форма СЗВ-6-4. Строка загружена.");
                                    }
                                    else
                                    {
                                        szv64 = db.FormsSZV_6_4.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && x.TypeContract == typeContract && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);

                                        if (szv64.StajOsn.Count() > 0)
                                        {
                                            db.Database.ExecuteSqlCommand(String.Format("DELETE FROM StajOsn WHERE ([FormsSZV_6_4_ID] = {0})", szv64.ID));
                                        }
                                    }


                                    foreach (DataRow row in b)
                                    {
                                        if (bw.CancellationPending)
                                        {
                                            DBFMessage = "Выполнение операции отменено!";
                                            return false;
                                        }

                                        DateTime dateBegin = DateTime.Parse(row["BPERIOD"].ToString());
                                        DateTime dateEnd = DateTime.Parse(row["EPERIOD"].ToString());

                                        StajOsn stajosn = new StajOsn
                                        {
                                            Number = long.Parse(!String.IsNullOrEmpty(row["PRNUMBER"].ToString()) ? row["PRNUMBER"].ToString() : "0"),
                                            DateBegin = dateBegin,
                                            DateEnd = dateEnd
                                        };

                                        szv64.StajOsn.Add(stajosn);

                                        //                                        db.StajOsn.Add(stajosn);
                                        //                                        db.SaveChanges();
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Строка загружена.");

                                        #region  Информация о льготном стаже в строке об основном


                                        string terrcondit = row["TERRCONDIT"].ToString().Trim().Length > 0 ? Encode(row["TERRCONDIT"].ToString().Trim()) : "";
                                        long? terrusl = TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit) != null ? TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit).ID : (long?)null;

                                        string scwork = row["SCWORK"].ToString().Trim().Length > 0 ? Encode(row["SCWORK"].ToString().Trim()) : "";
                                        long? osobusltruda = OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork) != null ? OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork).ID : (long?)null;

                                        long? kodvred2 = null;
                                        if (scwork == "27-1" || scwork == "27-2")
                                        {
                                            string positionl = row["POSITIONL"].ToString().Trim().Length > 0 ? Encode(row["POSITIONL"].ToString().Trim()) : "";
                                            kodvred2 = KodVred_2_list.FirstOrDefault(x => x.Code == positionl) != null ? KodVred_2_list.FirstOrDefault(x => x.Code == positionl).ID : (long?)null;
                                        }

                                        string profession = "";
                                        long? dolgn1 = (long?)null;
                                        if (row["PROFESSION"].ToString().Trim().Length > 0)
                                        {
                                            profession = row["PROFESSION"].ToString().Trim().Length > 0 ? Encode(row["PROFESSION"].ToString().Trim()) : "";
                                            if (!db.Dolgn.Any(x => x.Name == profession))
                                            {
                                                Dolgn dolgn = new Dolgn
                                                {
                                                    Name = profession
                                                };
                                                db.Dolgn.Add(dolgn);
                                                db.SaveChanges();
                                                dolgn1 = dolgn.ID;
                                            }
                                            else
                                            {
                                                dolgn1 = db.Dolgn.FirstOrDefault(x => x.Name == profession).ID;
                                            }
                                        }

                                        string basisexp = row["BASISEXP"].ToString().Trim().Length > 0 ? Encode(row["BASISEXP"].ToString().Trim()) : "";
                                        long? ischislstrahstajosn = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp) != null ? IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp).ID : (long?)null;

                                        string aiexp3 = row["AIEXP3"].ToString().Trim().Length > 0 ? Encode(row["AIEXP3"].ToString().Trim()) : "";
                                        long? ischislstrahstajdop = IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3) != null ? IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3).ID : (long?)null;

                                        string basisyear = row["BASISYEAR"].ToString().Trim().Length > 0 ? Encode(row["BASISYEAR"].ToString().Trim()) : "";
                                        long? usldosrnazn = UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear) != null ? UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear).ID : (long?)null;

                                        if (terrusl != null || osobusltruda != null || kodvred2 != null || dolgn1 != null || ischislstrahstajosn != null || ischislstrahstajdop != null || usldosrnazn != null)
                                        {

                                            StajLgot stajlgot = new StajLgot
                                            {
                                                //                                                StajOsnID = stajosn.ID,
                                                Number = 1,
                                                KodVred_OsnID = kodvred2,
                                                TerrUslID = terrusl,
                                                TerrUslKoef = decimal.Parse(!String.IsNullOrEmpty(row["RKOFF"].ToString()) ? row["RKOFF"].ToString() : "0"),
                                                OsobUslTrudaID = osobusltruda,
                                                DolgnID = dolgn1,
                                                IschislStrahStajOsnID = ischislstrahstajosn,
                                                IschislStrahStajDopID = ischislstrahstajdop,
                                                Strah1Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP1"].ToString()) ? row["AIEXP1"].ToString() : "0"),
                                                Strah2Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP2"].ToString()) ? row["AIEXP2"].ToString() : "0"),
                                                UslDosrNaznID = usldosrnazn,
                                                UslDosrNazn1Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR1"].ToString()) ? row["AIYEAR1"].ToString() : "0"),
                                                UslDosrNazn2Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR2"].ToString()) ? row["AIYEAR2"].ToString() : "0"),
                                                UslDosrNazn3Param = decimal.Parse(!String.IsNullOrEmpty(row["AIYEAR3"].ToString()) ? row["AIYEAR3"].ToString() : "0")
                                            };

                                            stajosn.StajLgot.Add(stajlgot);
                                            //                                            db.StajLgot.Add(stajlgot);
                                            //db.SaveChanges();
                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж льготный. Строка загружена.");
                                        }

                                        #endregion

                                        if (bw.CancellationPending)
                                        {
                                            DBFMessage = "Выполнение операции отменено!";
                                            db.SaveChanges();
                                            return false;
                                        }


                                        k++;

                                        decimal temp = (decimal)k / (decimal)cnt;
                                        int proc = (int)Math.Round((temp * 100), 0);
                                        bw.ReportProgress(proc, k);

                                        if (k % transCnt == 0)
                                        {
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "ielos64": //Индивидуальные сведения 2014. Дополнительные сведения о льготном стаже.
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var TerrUsl_list = db.TerrUsl.ToList();
                            var OsobUslTruda_list = db.OsobUslTruda.ToList();
                            var KodVred_2_list = db.KodVred_2.ToList();
                            var KodVred_3_list = db.KodVred_3.ToList();
                            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
                            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
                            var UslDosrNazn_list = db.UslDosrNazn.ToList();
                            var PlatCategoryRaschPer_list = db.PlatCategoryRaschPer.ToList();
                            var PlatCategory_list = db.PlatCategory.ToList();

                            DataColumnCollection columns = dt.Columns;
                            string YEAR = !columns.Contains("YEARP") ? "YEAR" : "YEARP";

                            foreach (DataRow row in result)
                            {
                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');


                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    int ti = 1;
                                    string ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                    switch (ti_cod)
                                    {
                                        case "ИСХД":
                                            ti = 1;
                                            break;
                                        case "КОРР":
                                            ti = 2;
                                            break;
                                        case "ОТМН":
                                            ti = 3;
                                            break;
                                    }

                                    short y = 0;
                                    byte q = 0;
                                    short yk = 0;
                                    byte qk = 0;
                                    long n = 1;
                                    byte typeContract = 1;

                                    short.TryParse(row[YEAR].ToString(), out y);
                                    byte.TryParse(row["QUARTER"].ToString(), out q);
                                    short.TryParse(row["YEARKORR"].ToString(), out yk);
                                    byte.TryParse(row["QUARTKORR"].ToString(), out qk);
                                    byte.TryParse(row["TYPECONTRC"].ToString(), out typeContract);

                                    FormsSZV_6_4 szv64 = new FormsSZV_6_4 { };

                                    string category = Encode(row["CATEGORY"].ToString().Trim());
                                    var pcrp = PlatCategoryRaschPer_list.FirstOrDefault(x => (!x.DateEnd.HasValue && x.DateBegin.Value.Year <= y) || (x.DateEnd.HasValue && (x.DateBegin.Value.Year <= y && x.DateEnd.Value.Year >= y)));

                                    PlatCategory platcategory = PlatCategory_list.FirstOrDefault(x => x.Code == category && x.PlatCategoryRaschPerID == pcrp.ID);

                                    if (!db.FormsSZV_6_4.Any(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && x.TypeContract == typeContract && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID))
                                    {
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Отсутствует запись по данному сотруднику. Сначало необходимо загрузить сведения об основном стаже.");

                                        k++;
                                        decimal temp_ = (decimal)k / (decimal)cnt;
                                        int proc_ = (int)Math.Round((temp_ * 100), 0);
                                        bw.ReportProgress(proc_, k);
                                        continue;
                                    }
                                    else
                                    {
                                        szv64 = db.FormsSZV_6_4.FirstOrDefault(x => x.Year == y && x.Quarter == q && x.PlatCategoryID == platcategory.ID && x.TypeContract == typeContract && ((x.YearKorr.HasValue && x.YearKorr.Value == yk) || !x.YearKorr.HasValue) && ((x.QuarterKorr.HasValue && x.QuarterKorr.Value == qk) || !x.QuarterKorr.HasValue) && x.TypeInfoID == ti && x.InsurerID == Options.InsID && x.StaffID == staffID);
                                    }


                                    byte PRNUMBER = 0;
                                    byte.TryParse(row["PRNUMBER"].ToString(), out PRNUMBER);

                                    if (!szv64.StajOsn.Any(x => x.Number == PRNUMBER))
                                    {
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Отсутствует запись по данному сотруднику. Сначало необходимо загрузить сведения об основном стаже.");
                                    }
                                    else
                                    {
                                        StajOsn stajosn = szv64.StajOsn.First(x => x.Number == PRNUMBER);

                                        if (stajosn.StajLgot.Count() > 0)
                                        {
                                            n = stajosn.StajLgot.Max(x => x.Number.Value) + 1;
                                        }

                                        string terrcondit = row["TERRCONDIT"].ToString().Trim().Length > 0 ? Encode(row["TERRCONDIT"].ToString().Trim()) : "";
                                        long? terrusl = TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit) != null ? TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit).ID : (long?)null;

                                        string scwork = row["SCWORK"].ToString().Trim().Length > 0 ? Encode(row["SCWORK"].ToString().Trim()) : "";
                                        long? osobusltruda = OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork) != null ? OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork).ID : (long?)null;

                                        long? kodvred2 = null;
                                        if (scwork == "27-1" || scwork == "27-2")
                                        {
                                            string positionl = row["POSITIONL"].ToString().Trim().Length > 0 ? Encode(row["POSITIONL"].ToString().Trim()) : "";
                                            kodvred2 = KodVred_2_list.FirstOrDefault(x => x.Code == positionl) != null ? KodVred_2_list.FirstOrDefault(x => x.Code == positionl).ID : (long?)null;
                                        }

                                        string profession = "";
                                        long? dolgn1 = (long?)null;
                                        if (row["PROFESSION"].ToString().Trim().Length > 0)
                                        {
                                            profession = row["PROFESSION"].ToString().Trim().Length > 0 ? Encode(row["PROFESSION"].ToString().Trim()) : "";
                                            if (!db.Dolgn.Any(x => x.Name == profession))
                                            {
                                                Dolgn dolgn = new Dolgn
                                                {
                                                    Name = profession
                                                };
                                                db.Dolgn.Add(dolgn);
                                                db.SaveChanges();
                                                dolgn1 = dolgn.ID;
                                            }
                                            else
                                            {
                                                dolgn1 = db.Dolgn.FirstOrDefault(x => x.Name == profession).ID;
                                            }
                                        }

                                        string basisexp = row["BASISEXP"].ToString().Trim().Length > 0 ? Encode(row["BASISEXP"].ToString().Trim()) : "";
                                        long? ischislstrahstajosn = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp) != null ? IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp).ID : (long?)null;

                                        string aiexp3 = row["AIEXP3"].ToString().Trim().Length > 0 ? Encode(row["AIEXP3"].ToString().Trim()) : "";
                                        long? ischislstrahstajdop = IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3) != null ? IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3).ID : (long?)null;

                                        string basisyear = row["BASISYEAR"].ToString().Trim().Length > 0 ? Encode(row["BASISYEAR"].ToString().Trim()) : "";
                                        long? usldosrnazn = UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear) != null ? UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear).ID : (long?)null;

                                        if (terrusl != null || osobusltruda != null || kodvred2 != null || dolgn1 != null || ischislstrahstajosn != null || ischislstrahstajdop != null || usldosrnazn != null)
                                        {

                                            StajLgot stajlgot = new StajLgot
                                            {
                                                //                                                StajOsnID = stajosn.ID,
                                                Number = n,
                                                KodVred_OsnID = kodvred2,
                                                TerrUslID = terrusl,
                                                TerrUslKoef = decimal.Parse(!String.IsNullOrEmpty(row["RKOFF"].ToString()) ? row["RKOFF"].ToString() : "0"),
                                                OsobUslTrudaID = osobusltruda,
                                                DolgnID = dolgn1,
                                                IschislStrahStajOsnID = ischislstrahstajosn,
                                                IschislStrahStajDopID = ischislstrahstajdop,
                                                Strah1Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP1"].ToString()) ? row["AIEXP1"].ToString() : "0"),
                                                Strah2Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP2"].ToString()) ? row["AIEXP2"].ToString() : "0"),
                                                UslDosrNaznID = usldosrnazn,
                                                UslDosrNazn1Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR1"].ToString()) ? row["AIYEAR1"].ToString() : "0"),
                                                UslDosrNazn2Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR2"].ToString()) ? row["AIYEAR2"].ToString() : "0"),
                                                UslDosrNazn3Param = decimal.Parse(!String.IsNullOrEmpty(row["AIYEAR3"].ToString()) ? row["AIYEAR3"].ToString() : "0")
                                            };

                                            stajosn.StajLgot.Add(stajlgot);
                                            //                                            db.StajLgot.Add(stajlgot);
                                            //db.SaveChanges();
                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж льготный. Строка загружена.");
                                        }

                                    }
                                }
                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                if (k % transCnt == 0)
                                {
                                    db.SaveChanges();
                                }
                            }

                            db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;

                    case "szv_staj": //Основных данных СЗВ-СТАЖ
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {

                            DataColumnCollection columns = dt.Columns;

                            if (result.Count <= 0)
                            {
                                sb.AppendLine(DateTime.Now.ToString() + "Данные в файле не обнаружены. Строка не загружена.");
                                break;
                            }

                            byte odv_ti = 0;
                            string odv_ti_cod = Encode(result[0]["ODVTYPE"].ToString().Trim());

                            switch (odv_ti_cod)
                            {
                                case "ИСХД":
                                    odv_ti = 0;
                                    break;
                                case "КОРР":
                                    odv_ti = 1;
                                    break;
                                case "ОТМН":
                                    odv_ti = 2;
                                    break;
                            }


                            short odv_y = 0;
                            byte odv_c = 0;

                            short.TryParse(result[0]["ODVYEAR"].ToString(), out odv_y);
                            byte.TryParse(result[0]["ODVCODE"].ToString(), out odv_c);

                            FormsODV_1_2017 odv1 = new FormsODV_1_2017
                            {
                                TypeInfo = odv_ti,
                                TypeForm = 1,
                                Year = odv_y,
                                Code = odv_c,
                                DateFilling = DateTime.Now,
                                InsurerID = Options.InsID,
                                StaffCount = result.Count
                            };

                            try
                            {
                                db.FormsODV_1_2017.Add(odv1);
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                sb.AppendLine(DateTime.Now.ToString() + "Не удалось добавить форму ОДВ-1 в БД. Код ошибки - " + ex.Message);
                                break;
                            }

                            foreach (DataRow row in result)
                            {
                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');


                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;


                                    byte szv_ti = 0;
                                    string szv_ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                    switch (szv_ti_cod)
                                    {
                                        case "ИСХД":
                                            szv_ti = 0;
                                            break;
                                        case "ДОПЛ":
                                            szv_ti = 1;
                                            break;
                                        case "НАЗН":
                                            szv_ti = 2;
                                            break;
                                    }


                                    short y = 0;
                                    byte c = 0;

                                    short.TryParse(row["YEAR"].ToString(), out y);
                                    byte.TryParse(row["CODE"].ToString(), out c);

                                    byte OPSFeeNach = 0;
                                    byte DopTarFeeNach = 0;

                                    byte.TryParse(row["OPSFEENACH"].ToString(), out OPSFeeNach);
                                    byte.TryParse(row["DOPTARFEE"].ToString(), out DopTarFeeNach);

                                    byte Dismissed = 0;
//                                    byte CodeBEZR = 0;

                                    byte.TryParse(row["DISMISSED"].ToString(), out Dismissed);
//                                    byte.TryParse(row["CODEBEZR"].ToString(), out CodeBEZR);

                                    DateTime dts = DateTime.Now;

                                    if (!updateDateFilling.Checked)
                                        if (row["DATEFILL"] != null && !String.IsNullOrEmpty(row["DATEFILL"].ToString().Trim()))
                                        {
                                            DateTime.TryParse(row["DATEFILL"].ToString(), out dts);
                                        }

                                    try
                                    {
                                        FormsSZV_STAJ_2017 szv = new FormsSZV_STAJ_2017
                                        {
                                            StaffID = staffID,
                                            Year = y,
                                            Code = c,
                                            TypeInfo = szv_ti,
                                            FormsODV_1_2017_ID = odv1.ID,
                                            InsurerID = Options.InsID,
                                            OPSFeeNach = OPSFeeNach,
                                            DopTarFeeNach = DopTarFeeNach,
                                            DateComposit = DateTime.Now,
                                            DateFilling = dts,
                                            ConfirmFIO = Encode(row["CONFFIO"].ToString().Trim()),
                                            ConfirmDolgn = Encode(row["CONFDOLGN"].ToString().Trim()),
                                            Dismissed = Dismissed == 1
                                        };

                                        //db.AddToFormsSZV_STAJ_2017(szv);
                                        SZV_STAJ_List.Add(szv);
                                    }
                                    catch (Exception ex)
                                    {
                                        sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Не удалось добавить форму Форму СЗВ-СТАЖ в БД. Код ошибки - " + ex.Message);

                                    }

                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Форма СЗВ-СТАЖ подготовлена к загрузке.");
                                }

                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;
                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                //if (k % transCnt == 0)
                                //{
                                //    foreach (var item in rsw61List)
                                //    {
                                //        db.AddToFormsRSW2014_1_Razd_6_1(item);
                                //    }

                                //    db.SaveChanges();
                                //    rsw61List.Clear();
                                //}
                            }

                            //foreach (var item in rsw61List)
                            //{
                            //    db.AddToFormsRSW2014_1_Razd_6_1(item);
                            //}
                            //db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }

                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "szv_staj5": //Раздела 4 Формы СЗВ-СТАЖ
                        #region

                        if (SZV_STAJ_List.Count <= 0)
                            break;

                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {

                            DataColumnCollection columns = dt.Columns;

                            foreach (DataRow row in result)
                            {
                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');


                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {

                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;

                                    if (SZV_STAJ_List.Any(x => x.StaffID == staffID))
                                    {
                                        var szv_item = SZV_STAJ_List.First(x => x.StaffID == staffID);

                                        byte szv_ti = 0;
                                        string szv_ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                        switch (szv_ti_cod)
                                        {
                                            case "ИСХД":
                                                szv_ti = 0;
                                                break;
                                            case "ДОПЛ":
                                                szv_ti = 1;
                                                break;
                                            case "НАЗН":
                                                szv_ti = 2;
                                                break;
                                        }


                                        short y = 0;
                                        byte c = 0;

                                        short.TryParse(row["YEAR"].ToString(), out y);
                                        byte.TryParse(row["CODE"].ToString(), out c);

                                        byte DNPO_Fee = 0;

                                        byte.TryParse(row["DNPOFEE"].ToString(), out DNPO_Fee);




                                        try
                                        {
                                            FormsSZV_STAJ_4_2017 szv4 = new FormsSZV_STAJ_4_2017
                                            {
                                                DNPO_DateFrom = DateTime.Parse(row["DATEFROM"].ToString()),
                                                DNPO_DateTo = DateTime.Parse(row["DATETO"].ToString()),
                                                DNPO_Fee = DNPO_Fee == 1

                                            };
                                            szv_item.FormsSZV_STAJ_4_2017.Add(szv4);

                                        }
                                        catch (Exception ex)
                                        {
                                            continue;
                                        }


                                    }




                                }
                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                //if (k % transCnt == 0)
                                //{
                                //    foreach (var item in rsw61List)
                                //    {
                                //        db.AddToFormsRSW2014_1_Razd_6_1(item);
                                //    }

                                //    db.SaveChanges();
                                //    rsw61List.Clear();
                                //}
                            }
                            //foreach (var item in rsw61List)
                            //{
                            //    db.AddToFormsRSW2014_1_Razd_6_1(item);
                            //}
                            //db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по выплатам по дополнительным тарифам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "szv_staj_s": //данных о стаже сотрудников Формы СЗВ-СТАЖ.
                        #region


                        bool justStaj = SZV_STAJ_List.Count <= 0;  // True если загружается только файл стажа без файла СЗВ-СТАЖ


                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            DataColumnCollection columns = dt.Columns;

                            long odvID = 0;

                            #region  // Создаем новую форму ОДВ-1 по данным из первой записи в файле стажа

                            if (justStaj)
                            {
                                byte odv_ti = 0;
                                string odv_ti_cod = Encode(result[0]["ODVTYPE"].ToString().Trim());

                                switch (odv_ti_cod)
                                {
                                    case "ИСХД":
                                        odv_ti = 0;
                                        break;
                                    case "КОРР":
                                        odv_ti = 1;
                                        break;
                                    case "ОТМН":
                                        odv_ti = 2;
                                        break;
                                }


                                short odv_y = 0;
                                byte odv_c = 0;

                                short.TryParse(result[0]["ODVYEAR"].ToString(), out odv_y);
                                byte.TryParse(result[0]["ODVCODE"].ToString(), out odv_c);

                                FormsODV_1_2017 odv1 = new FormsODV_1_2017
                                {
                                    TypeInfo = odv_ti,
                                    TypeForm = 1,
                                    Year = odv_y,
                                    Code = odv_c,
                                    DateFilling = DateTime.Now,
                                    InsurerID = Options.InsID,
                                    StaffCount = result.Count
                                };

                                try
                                {
                                    db.FormsODV_1_2017.Add(odv1);
                                    db.SaveChanges();

                                    odvID = odv1.ID;
                                }
                                catch (Exception ex)
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "Не удалось добавить форму ОДВ-1 в БД. Код ошибки - " + ex.Message);
                                    break;
                                }

                            }
                            #endregion


                            var groupList = result.GroupBy(x => new { INSURANCE = x["INSURANCE"], YEAR = x["YEAR"], QUARTER = x["CODE"], TYPEINFORM = x["TYPEINFORM"] });
                            foreach (var b in groupList)
                            {

                                insnumber = b.Key.INSURANCE.ToString().Trim().PadLeft(9, '0');

                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;
                                    long n = 1;

                                    foreach (DataRow row in b)
                                    {
                                        if (bw.CancellationPending)
                                        {
                                            DBFMessage = "Выполнение операции отменено!";
                                            return false;
                                        }



                                            StajOsn stajosn = new StajOsn();

                                            try
                                            {
                                                stajosn.Number = updateStaj_DDL.SelectedItem.Tag.ToString() == "0" ? long.Parse(!String.IsNullOrEmpty(row["PRNUMBER"].ToString()) ? row["PRNUMBER"].ToString() : "0") : n;
                                                stajosn.DateBegin = DateTime.Parse(row["BPERIOD"].ToString());
                                                stajosn.DateEnd = DateTime.Parse(row["EPERIOD"].ToString());
                                                stajosn.CodeBEZR = row["CODEBEZR"] != null ? (row["CODEBEZR"].ToString() == "1" ? true : false) : false;
                                            }
                                            catch (Exception ex)
                                            {
                                                continue;
                                            }

                                            n++;


                                            byte szv_ti = 0;
                                            string szv_ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                            switch (szv_ti_cod)
                                            {
                                                case "ИСХД":
                                                    szv_ti = 0;
                                                    break;
                                                case "ДОПЛ":
                                                    szv_ti = 1;
                                                    break;
                                                case "НАЗН":
                                                    szv_ti = 2;
                                                    break;
                                            }


                                            short y = 0;
                                            byte c = 0;

                                            short.TryParse(row["YEAR"].ToString(), out y);
                                            byte.TryParse(row["CODE"].ToString(), out c);


                                            if (SZV_STAJ_List.Any(x => x.StaffID == staffID && x.TypeInfo == szv_ti && x.Year == y && x.Code == c))
                                            {
                                                var szv_item = SZV_STAJ_List.First(x => x.StaffID == staffID && x.TypeInfo == szv_ti && x.Year == y && x.Code == c);

                                                szv_item.StajOsn.Add(stajosn);
                                            }
                                            else if (justStaj && odvID != 0) //Если грузится только файл стажа и записи СЗВ-СТАЖ еще нет
                                            {
                                                FormsSZV_STAJ_2017 szv = new FormsSZV_STAJ_2017
                                                {
                                                    StaffID = staffID,
                                                    Year = y,
                                                    Code = c,
                                                    TypeInfo = szv_ti,
                                                    FormsODV_1_2017_ID = odvID,
                                                    InsurerID = Options.InsID,
                                                    OPSFeeNach = 0,
                                                    DopTarFeeNach = 0,
                                                    DateComposit = DateTime.Now,
                                                    DateFilling = DateTime.Now,
                                                    ConfirmFIO = "",
                                                    ConfirmDolgn = "",
                                                    Dismissed = false
                                                };

                                                szv.StajOsn.Add(stajosn);

                                                SZV_STAJ_List.Add(szv);
                                            }

                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Строка загружена.");



                                        if (bw.CancellationPending)
                                        {
                                            DBFMessage = "Выполнение операции отменено!";
                                            db.SaveChanges();
                                            return false;
                                        }


                                        k++;

                                        decimal temp = (decimal)k / (decimal)cnt;
                                        int proc = (int)Math.Round((temp * 100), 0);
                                        bw.ReportProgress(proc, k);

                                        //if (k % transCnt == 0)
                                        //{
                                        //    db.SaveChanges();
                                        //}
                                    }
                                }
                            }

                            //         db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                    case "szv_staj_sl": //данных о Льготном стаже сотрудников Формы СЗВ-СТАЖ
                        #region
                        log_filename = pathLog + @"\" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + "_" + tableName + ".txt";
                        outfile = new StreamWriter(log_filename);

                        try
                        {
                            var TerrUsl_list = db.TerrUsl.ToList();
                            var OsobUslTruda_list = db.OsobUslTruda.ToList();
                            var KodVred_2_list = db.KodVred_2.ToList();
                            var KodVred_3_list = db.KodVred_3.ToList();
                            var IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
                            var IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
                            var UslDosrNazn_list = db.UslDosrNazn.ToList();

                            DataColumnCollection columns = dt.Columns;

                            foreach (DataRow row in result)
                            {

                                insnumber = row["INSURANCE"].ToString().Trim().PadLeft(9, '0');


                                if (!db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber))
                                {
                                    sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Сотрудник не найден. Строка не загружена.");
                                }
                                else
                                {
                                    long staffID = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == insnumber).ID;


                                    byte szv_ti = 0;
                                    string szv_ti_cod = Encode(row["TYPEINFORM"].ToString().Trim());

                                    switch (szv_ti_cod)
                                    {
                                        case "ИСХД":
                                            szv_ti = 0;
                                            break;
                                        case "ДОПЛ":
                                            szv_ti = 1;
                                            break;
                                        case "НАЗН":
                                            szv_ti = 2;
                                            break;
                                    }


                                    short y = 0;
                                    byte c = 0;

                                    short.TryParse(row["YEAR"].ToString(), out y);
                                    byte.TryParse(row["CODE"].ToString(), out c);


                                    if (SZV_STAJ_List.Any(x => x.StaffID == staffID && x.TypeInfo == szv_ti && x.Year == y && x.Code == c))
                                    {
                                        var szv_item = SZV_STAJ_List.First(x => x.StaffID == staffID && x.TypeInfo == szv_ti && x.Year == y && x.Code == c);


                                        long n = 1;


                                        byte PRNUMBER = 0;
                                        byte.TryParse(row["PRNUMBER"].ToString(), out PRNUMBER);

                                        if (!szv_item.StajOsn.Any(x => x.Number == PRNUMBER))
                                        {
                                            sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж основной. Отсутствует запись по данному сотруднику. Сначало необходимо загрузить сведения об основном стаже.");
                                        }
                                        else
                                        {
                                            StajOsn stajosn = szv_item.StajOsn.First(x => x.Number == PRNUMBER);

                                            if (stajosn.StajLgot.Count() > 0)
                                            {
                                                n = stajosn.StajLgot.Max(x => x.Number.Value) + 1;
                                            }

                                            string terrcondit = row["TERRCONDIT"].ToString().Trim().Length > 0 ? Encode(row["TERRCONDIT"].ToString().Trim()) : "";
                                            long? terrusl = TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit) != null ? TerrUsl_list.FirstOrDefault(x => x.Code == terrcondit).ID : (long?)null;

                                            string scwork = row["SCWORK"].ToString().Trim().Length > 0 ? Encode(row["SCWORK"].ToString().Trim()) : "";
                                            long? osobusltruda = OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork) != null ? OsobUslTruda_list.FirstOrDefault(x => x.Code == scwork).ID : (long?)null;

                                            long? kodvred2 = null;
                                            if (scwork == "27-1" || scwork == "27-2")
                                            {
                                                string positionl = row["POSITIONL"].ToString().Trim().Length > 0 ? Encode(row["POSITIONL"].ToString().Trim()) : "";
                                                kodvred2 = KodVred_2_list.FirstOrDefault(x => x.Code == positionl) != null ? KodVred_2_list.FirstOrDefault(x => x.Code == positionl).ID : (long?)null;
                                            }

                                            string profession = "";
                                            long? dolgn1 = (long?)null;
                                            if (row["PROFESSION"].ToString().Trim().Length > 0)
                                            {
                                                profession = row["PROFESSION"].ToString().Trim().Length > 0 ? Encode(row["PROFESSION"].ToString().Trim()) : "";
                                                if (!db.Dolgn.Any(x => x.Name == profession))
                                                {
                                                    Dolgn dolgn = new Dolgn
                                                    {
                                                        Name = profession
                                                    };
                                                    db.Dolgn.Add(dolgn);
                                                    db.SaveChanges();
                                                    dolgn1 = dolgn.ID;
                                                }
                                                else
                                                {
                                                    dolgn1 = db.Dolgn.FirstOrDefault(x => x.Name == profession).ID;
                                                }
                                            }

                                            string basisexp = row["BASISEXP"].ToString().Trim().Length > 0 ? Encode(row["BASISEXP"].ToString().Trim()) : "";
                                            long? ischislstrahstajosn = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp) != null ? IschislStrahStajOsn_list.FirstOrDefault(x => x.Code == basisexp).ID : (long?)null;

                                            string aiexp3 = row["AIEXP3"].ToString().Trim().Length > 0 ? Encode(row["AIEXP3"].ToString().Trim()) : "";
                                            long? ischislstrahstajdop = IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3) != null ? IschislStrahStajDop_list.FirstOrDefault(x => x.Code == aiexp3).ID : (long?)null;

                                            string basisyear = row["BASISYEAR"].ToString().Trim().Length > 0 ? Encode(row["BASISYEAR"].ToString().Trim()) : "";
                                            long? usldosrnazn = UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear) != null ? UslDosrNazn_list.FirstOrDefault(x => x.Code == basisyear).ID : (long?)null;

                                            if (terrusl != null || osobusltruda != null || kodvred2 != null || dolgn1 != null || ischislstrahstajosn != null || ischislstrahstajdop != null || usldosrnazn != null)
                                            {

                                                StajLgot stajlgot = new StajLgot
                                                {
                                                    Number = n,
                                                    KodVred_OsnID = kodvred2,
                                                    TerrUslID = terrusl,
                                                    TerrUslKoef = decimal.Parse(!String.IsNullOrEmpty(row["RKOFF"].ToString()) ? row["RKOFF"].ToString() : "0"),
                                                    OsobUslTrudaID = osobusltruda,
                                                    DolgnID = dolgn1,
                                                    IschislStrahStajOsnID = ischislstrahstajosn,
                                                    IschislStrahStajDopID = ischislstrahstajdop,
                                                    Strah1Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP1"].ToString()) ? row["AIEXP1"].ToString() : "0"),
                                                    Strah2Param = short.Parse(!String.IsNullOrEmpty(row["AIEXP2"].ToString()) ? row["AIEXP2"].ToString() : "0"),
                                                    UslDosrNaznID = usldosrnazn,
                                                    UslDosrNazn1Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR1"].ToString()) ? row["AIYEAR1"].ToString() : "0"),
                                                    UslDosrNazn2Param = short.Parse(!String.IsNullOrEmpty(row["AIYEAR2"].ToString()) ? row["AIYEAR2"].ToString() : "0"),
                                                    UslDosrNazn3Param = decimal.Parse(!String.IsNullOrEmpty(row["AIYEAR3"].ToString()) ? row["AIYEAR3"].ToString() : "0")
                                                };

                                                stajosn.StajLgot.Add(stajlgot);

                                                sb.AppendLine(DateTime.Now.ToString() + "   Номер " + insnumber + ". Стаж льготный. Строка загружена.");
                                            }

                                        }
                                    }
                                }
                                if (bw.CancellationPending)
                                {
                                    DBFMessage = "Выполнение операции отменено!";
                                    db.SaveChanges();
                                    return false;
                                }


                                k++;

                                decimal temp = (decimal)k / (decimal)cnt;
                                int proc = (int)Math.Round((temp * 100), 0);
                                bw.ReportProgress(proc, k);

                                //if (k % transCnt == 0)
                                //{
                                //    db.SaveChanges();
                                //}
                            }

                      //      db.SaveChanges();
                            fin = true;
                            if (!System.IO.Directory.Exists(pathLog))
                            {
                                System.IO.Directory.CreateDirectory(pathLog);
                            }

                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine(DateTime.Now.ToString() + "   " + ex.Message + ". Строка не загружена.");
                            using (outfile)
                            {
                                outfile.Write(sb.ToString());
                            }
                            message = ex.Message + "\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                        }
                        finally
                        {
                            if (fin)
                            {
                                message = "Информация по основным выплатам. Обработано: " + rowcon.ToString() + " записей.\n\rИз них загружено: " + k.ToString() + " записей.\n\rДополнительная информация по загрузке содержится в файле " + log_filename;
                            }
                        }
                        #endregion

                        break;
                }
            }
            DBFMessage = message;
            return true;
        }

        private void ImportMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bw.IsBusy)
                if (RadMessageBox.Show("Вы уверены, что хотите закрыть окно и прервать импорт данных?", "Предупреждение", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    e.Cancel = true;
                }
                else
                {
                    cancel_work = true;
                    bw.CancelAsync();
                }


            Props props = new Props(); //экземпляр класса с настройками
            List<WindowData> windowData = new List<WindowData> { };

            if (folderBrowser.SelectedPath != null)
            {
                windowData.Add(new WindowData
                {
                    control = "folderBrowser",
                    value = folderBrowser.SelectedPath
                });
            }

            if (openDialog.InitialDirectory != null)
            {
                windowData.Add(new WindowData
                {
                    control = "openDialog",
                    value = openDialog.InitialDirectory
                });
            }
            windowData.Add(new WindowData
            {
                control = "updateDateFilling",
                value = updateDateFilling.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateStaff_DDL",
                value = updateStaff_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "updateIndSved_DDL",
                value = updateIndSved_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "updateStaj_DDL",
                value = updateStaj_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "transactionCount",
                value = transactionCount.Value.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "codeAutoRadioButton",
                value = codeAutoRadioButton.IsChecked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "code866RadioButton",
                value = code866RadioButton.IsChecked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "code1251RadioButton",
                value = code1251RadioButton.IsChecked ? "true" : "false"
            });


            props.setFormParams(this, windowData);
        }

        private void abortBtn_Click(object sender, EventArgs e)
        {
            if (bw.IsBusy)
                if (RadMessageBox.Show("Вы уверены, что хотите прервать импорт данных?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bw.CancelAsync();
                }
        }

        private void selectFolderBtn_Click(object sender, EventArgs e)
        {
            if (this.folderBrowser.ShowDialog() != DialogResult.OK)
                return;
            currDirPath = folderBrowser.SelectedPath;
            string[] files = Directory.GetFiles(currDirPath, "*.dbf", SearchOption.TopDirectoryOnly);
            importFilesGrid.Rows.Clear();

            if (files.Count() > 0)
            {
                loadToGrid(files);
            }
        }

        private void selectFilesBtn_Click(object sender, EventArgs e)
        {
            if (this.openDialog.ShowDialog() != DialogResult.OK)
                return;
            importFilesGrid.Rows.Clear();
            if (openDialog.FileNames.Count() > 0)
            {
                currDirPath = Path.GetDirectoryName(openDialog.FileNames[0]);
                loadToGrid(openDialog.FileNames);

            }
        }

        public class DBFGridInfo
        {
            public int Rate { get; set; }
            public string Name { get; set; }
            public string Path { get; set; }
            public string Type { get; set; }
        }

        private void loadToGrid(string[] fileList)
        {
            List<DBFGridInfo> rec_list = new List<DBFGridInfo>();

            foreach (var item in fileList)
            {
                DBFGridInfo rec = new DBFGridInfo { Name = Path.GetFileNameWithoutExtension(item).ToLower(), Path = item };
                var tempName = rec.Name.Contains("_5") ? rec.Name.Remove(rec.Name.Length - 2).ToString() : rec.Name;

                switch (tempName)
                {
                    case "iemp":
                        rec.Type = "Персональные данные сотрудников";
                        rec.Rate = 1;
                        break;
                    case "iemp_6":
                        rec.Type = "Персональные данные сотрудников + ИНН";
                        rec.Rate = 1;
                        break;
                    case "idwemp":
                        rec.Type = "Данные о приёме\\увольнении сотрудников";
                        rec.Rate = 2;
                        break;
                    case "ipfrosn":
                        rec.Type = "Индивидуальные сведения 2014-2015. Основные выплаты.";
                        rec.Rate = 2;
                        break;
                    case "ipfrdop":
                        rec.Type = "Индивидуальные сведения 2014-2015. Выплаты по Дополнительным тарифам.";
                        rec.Rate = 3;
                        break;
                    case "ilospfr":
                        rec.Type = "Индивидуальные сведения 2014-2015. Основные записи о стаже.";
                        rec.Rate = 4;
                        break;
                    case "ielospfr":
                        rec.Type = "Индивидуальные сведения 2014-2015. Дополнительные сведения о льготном стаже.";
                        rec.Rate = 5;
                        break;
                    case "iczw6":
                        rec.Type = "Индивидуальные сведения 2010-2012. Основные выплаты.";
                        rec.Rate = 6;
                        break;
                    case "ilos6":
                        rec.Type = "Индивидуальные сведения 2010-2012. Основные записи о стаже.";
                        rec.Rate = 7;
                        break;
                    case "ielos6":
                        rec.Type = "Индивидуальные сведения 2010-2012. Дополнительные сведения о льготном стаже.";
                        rec.Rate = 8;
                        break;
                    case "iczw64":
                        rec.Type = "Индивидуальные сведения 2013. Основные выплаты.";
                        rec.Rate = 9;
                        break;
                    case "ilos64":
                        rec.Type = "Индивидуальные сведения 2013. Основные записи о стаже.";
                        rec.Rate = 10;
                        break;
                    case "ielos64":
                        rec.Type = "Индивидуальные сведения 2013. Дополнительные сведения о льготном стаже.";
                        rec.Rate = 11;
                        break;
                    case "szv_staj":
                        rec.Type = "Форма СЗВ-СТАЖ. Основные данные.";
                        rec.Rate = 2;
                        break;
                    case "szv_staj5":
                        rec.Type = "Форма СЗВ-СТАЖ. Раздел 5.";
                        rec.Rate = 3;
                        break;
                    case "szv_staj_s":
                        rec.Type = "Форма СЗВ-СТАЖ. Основные записи о стаже.";
                        rec.Rate = 4;
                        break;
                    case "szv_staj_sl":
                        rec.Type = "Форма СЗВ-СТАЖ. Дополнительные сведения о льготном стаже.";
                        rec.Rate = 5;
                        break;

                }

                if (!String.IsNullOrEmpty(rec.Type))
                    rec_list.Add(rec);
            }

            foreach (var item in rec_list.OrderBy(x => x.Rate))
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.importFilesGrid.MasterView);
                rowInfo = new GridViewDataRowInfo(this.importFilesGrid.MasterView);
                rowInfo.Cells[0].Value = Color.SkyBlue;
                rowInfo.Cells["path"].Value = item.Path;
                rowInfo.Cells["name"].Value = item.Name;
                rowInfo.Cells["type"].Value = item.Type;

                importFilesGrid.Rows.Add(rowInfo);
            }
            if (importFilesGrid.RowCount > 0)
            {
                importFilesGrid.Rows[0].IsCurrent = true;
                importFilesGrid.Rows[0].IsSelected = true;
            }
        }



    }
}