using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using Telerik.WinControls.UI;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml;
using PU.Classes;
using PU.Models;
using System.Globalization;
using System.Threading;
using System.Data.SQLite;
using System.Configuration;

namespace PU
{
    public partial class ImportXML : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        BackgroundWorker bw = new BackgroundWorker();
        BackgroundWorker bwPre = new BackgroundWorker();
        OpenFileDialog openDialog = new OpenFileDialog();
        FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
        private string currDirPath = "";
        private int cnt = 0;
        private int cnt_records_imported = 0;
        private bool cancel_work = false;
        private DateTime d;
        private List<ErrList> errList = new List<ErrList>();
        private List<string> fileList = new List<string>();
        List<string> errBox = new List<string>();
        XDocument doc = new XDocument();
        int rowIndex = 0;

        //      int lll = 1;

        List<PlatCategory> PlatCatList = new List<PlatCategory>();
        List<TerrUsl> TerrUsl_list = new List<TerrUsl>();
        List<OsobUslTruda> OsobUslTruda_list = new List<OsobUslTruda>();
        List<KodVred_2> KodVred_2_list = new List<KodVred_2>();
        List<IschislStrahStajOsn> IschislStrahStajOsn_list = new List<IschislStrahStajOsn>();
        List<IschislStrahStajDop> IschislStrahStajDop_list = new List<IschislStrahStajDop>();
        List<UslDosrNazn> UslDosrNazn_list = new List<UslDosrNazn>();
        List<SpecOcenkaUslTruda> SpecOcenkaUslTruda_list = new List<SpecOcenkaUslTruda>();
        List<TypeInfo> typeInfo_ = new List<TypeInfo>();

        SQLiteConnection destination = new SQLiteConnection();

        List<string> MonthesList = new List<string>
        {
            "Янв",
            "Фев",
            "Мрт",
            "Апр",
            "Май",
            "Июн",
            "Июл",
            "Авг",
            "Сен",
            "Окт",
            "Нбр",
            "Дек"
        };

        public ImportXML()
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

        private void ImportXML_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            RadMessageLocalizationProvider.CurrentProvider = new RussianRadMessageBoxLocalizationProvider();
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            /*      Application.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
                  System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru");
                  Application.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";*/

            /*            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("ru-RU");
                        System.Globalization.NumberFormatInfo numberInfo = new System.Globalization.NumberFormatInfo();
                        numberInfo.NumberDecimalSeparator = ".";
                        numberInfo.CurrencyDecimalSeparator = ".";
                        numberInfo.PercentDecimalSeparator = ".";
                        cultureInfo.NumberFormat = numberInfo;

                        Application.CurrentCulture = cultureInfo;
                        Thread.CurrentThread.CurrentCulture = cultureInfo;
                        Thread.CurrentThread.CurrentUICulture = cultureInfo;
                        */
            openDialog.Filter = "(*.xml)|*.xml";
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
                            case "updateInsData":
                                updateInsData.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateStaffData":
                                updateStaffData.Checked = item.value == "true" ? true : false;
                                break;
                            case "transactionCheckBox":
                                transactionCheckBox.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateSPW2_DDL":
                                int.TryParse(item.value, out i);
                                updateSPW2_DDL.SelectedIndex = i;
                                break;
                            case "updateIndSved_DDL":
                                int.TryParse(item.value, out i);
                                updateIndSved_DDL.SelectedIndex = i;
                                break;
                            case "updateSZV_6_DDL":
                                int.TryParse(item.value, out i);
                                updateSZV_6_DDL.SelectedIndex = i;
                                break;
                            case "updateSZV_6_4_DDL":
                                int.TryParse(item.value, out i);
                                updateSZV_6_4_DDL.SelectedIndex = i;
                                break;
                            case "updateSZVM_DDL":
                                int.TryParse(item.value, out i);
                                updateSZVM_DDL.SelectedIndex = i;
                                break;
                            case "updateDSW3_DDL":
                                int.TryParse(item.value, out i);
                                updateDSW3_DDL.SelectedIndex = i;
                                break;
                            case "updateODV_1_DDL":
                                int.TryParse(item.value, out i);
                                updateODV_1_DDL.SelectedIndex = i;
                                break;
                            case "updateStajSZV_STAJ":
                                updateStajSZV_STAJ.Checked = item.value == "true" ? true : false;
                                break;
                            case "updatePayFeeSZV_STAJ":
                                updatePayFeeSZV_STAJ.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateSumFeeSZV_6":
                                updateSumFeeSZV_6.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateStajSZV_6":
                                updateStajSZV_6.Checked = item.value == "true" ? true : false;
                                break;
                            case "updatePayFeeSZV_6":
                                updatePayFeeSZV_6.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateSumFeeSZV_6_4":
                                updateSumFeeSZV_6_4.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateSumFL_SZV_6_4":
                                updateSumFL_SZV_6_4.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateSumDop_SZV_6_4":
                                updateSumDop_SZV_6_4.Checked = item.value == "true" ? true : false;
                                break;
                            case "updatePayFeeSZV_6_4":
                                updatePayFeeSZV_6_4.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateStajSZV_6_4":
                                updateStajSZV_6_4.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateSumFee":
                                updateSumFee.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateRazd_6_4":
                                updateRazd_6_4.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateRazd_6_7":
                                updateRazd_6_7.Checked = item.value == "true" ? true : false;
                                break;
                            case "updateStaj":
                                updateStaj.Checked = item.value == "true" ? true : false;
                                break;
                        }

                    }


                }
                catch
                { }
            }

            updateIndSved_DDL_SelectedIndexChanged(null, null);
            updateSZV_6_DDL_SelectedIndexChanged(null, null);
            updateSZV_6_4_DDL_SelectedIndexChanged(null, null);
            updateODV_1_DDL_SelectedIndexChanged(null, null);
        }

        public class XMLGridInfo
        {
            public string Type { get; set; }
            public string Version { get; set; }
            public int CntDoc { get; set; }
            public int CntImported { get; set; }
            public string RegNum { get; set; }
            public string Insurer { get; set; }
        }

        private void selectFilesBtn_Click(object sender, EventArgs e)
        {

            if (this.openDialog.ShowDialog() != DialogResult.OK)
                return;
            importFilesGrid.Rows.Clear();
            if (openDialog.FileNames.Count() > 0)
            {
                currDirPath = Path.GetDirectoryName(openDialog.FileNames[0]);
                currentDir.Text = currDirPath;
                folderBrowser.SelectedPath = currDirPath;
                openDialog.InitialDirectory = currDirPath;


                fileList = openDialog.FileNames.ToList();

                startLoadFiles();
            }

        }

        private void selectFolderBtn_Click(object sender, EventArgs e)
        {
            if (this.folderBrowser.ShowDialog() != DialogResult.OK)
                return;
            currDirPath = folderBrowser.SelectedPath;
            currentDir.Text = currDirPath;
            string[] files = Directory.GetFiles(currDirPath, "*.xml", SearchOption.TopDirectoryOnly);
            importFilesGrid.Rows.Clear();

            if (files.Count() > 0)
            {
                folderBrowser.SelectedPath = currDirPath;
                openDialog.InitialDirectory = currDirPath;
                fileList = files.ToList();

                startLoadFiles();

                //                loadToGrid(files);
            }
        }

        private void startLoadFiles()
        {
            errBox = new List<string>();
            bw = new BackgroundWorker();
            bw.WorkerReportsProgress = false;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(loadToGrid);
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted_loadFiles);

            bw.RunWorkerAsync();
        }

        void bw_RunWorkerCompleted_loadFiles(object sender, RunWorkerCompletedEventArgs e)
        {
            if (errBox.Count() > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in errBox)
                {
                    sb.AppendLine(item);
                }
                RadMessageBox.Show(this, sb.ToString(), "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void loadToGrid(object sender, DoWorkEventArgs e)
        {
            importFilesGrid.Columns["cntImported"].DataType = typeof(int);

            foreach (var item in fileList)
            {
                doc = new XDocument();
                XMLGridInfo gridItem = new XMLGridInfo();
                string XML_SchemaName = "";
                bool errors = false;
                int start = 0;

                try
                {
                    doc = XDocument.Load(item);
                    var ns = doc.Root.GetDefaultNamespace();
                    doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

                    foreach (var elem in doc.Descendants())
                        elem.Name = elem.Name.LocalName;

                    XElement node = doc.Root.Element("ПачкаВходящихДокументов");

                    if (doc.Descendants().Any(p => p.Name.LocalName == "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014"))
                    {
                        gridItem.Type = "РСВ 2014";
                        node = node.Element("РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014");
                        gridItem.RegNum = node.Element("РегистрационныйНомерПФР").Value;
                        gridItem.CntDoc = 1;

                        string[] name = new string[3];
                        if (node.Element("НаименованиеОрганизации") != null)
                        {
                            name[0] = node.Element("НаименованиеОрганизации").Value;
                        }
                        else if (node.Element("ФИОфизическогоЛица") != null)
                        {
                            name[0] = node.Element("ФИОфизическогоЛица").Element("Фамилия") != null ? node.Element("ФИОфизическогоЛица").Element("Фамилия").Value : "";
                            name[1] = node.Element("ФИОфизическогоЛица").Element("Имя") != null ? (" " + node.Element("ФИОфизическогоЛица").Element("Имя").Value) : "";
                            name[2] = node.Element("ФИОфизическогоЛица").Element("Отчество") != null ? (" " + node.Element("ФИОфизическогоЛица").Element("Отчество").Value) : "";
                        }

                        gridItem.Insurer = name[0] + name[1] + name[2];
                        gridItem.Version = doc.Root.Element("ЗаголовокФайла").Element("ВерсияФормата").Value;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2015"))
                    {
                        gridItem.Type = "РСВ 2015";
                        node = node.Element("РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2015");
                        gridItem.RegNum = node.Element("РегистрационныйНомерПФР").Value;
                        gridItem.CntDoc = 1;

                        string[] name = new string[3];
                        if (node.Element("НаименованиеОрганизации") != null)
                        {
                            name[0] = node.Element("НаименованиеОрганизации").Value;
                        }
                        else if (node.Element("ФИОфизическогоЛица") != null)
                        {
                            name[0] = node.Element("ФИОфизическогоЛица").Element("Фамилия") != null ? node.Element("ФИОфизическогоЛица").Element("Фамилия").Value : "";
                            name[1] = node.Element("ФИОфизическогоЛица").Element("Имя") != null ? (" " + node.Element("ФИОфизическогоЛица").Element("Имя").Value) : "";
                            name[2] = node.Element("ФИОфизическогоЛица").Element("Отчество") != null ? (" " + node.Element("ФИОфизическогоЛица").Element("Отчество").Value) : "";
                        }

                        gridItem.Insurer = name[0] + name[1] + name[2];
                        gridItem.Version = doc.Root.Element("ЗаголовокФайла").Element("ВерсияФормата").Value;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6"))
                    {
                        gridItem.Type = "ПФР 2014";
                        node = node.Element("СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6").Element("СоставительПачки");
                        gridItem.RegNum = node.Element("РегистрационныйНомер").Value;
                        gridItem.CntDoc = doc.Root.Element("ПачкаВходящихДокументов").Elements("СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ").Count();
                        gridItem.Insurer = node.Element("НаименованиеОрганизации") != null ? node.Element("НаименованиеОрганизации").Value : (node.Element("НаименованиеКраткое") != null ? node.Element("НаименованиеКраткое").Value : "");
                        gridItem.Version = doc.Root.Element("ЗаголовокФайла").Element("ВерсияФормата").Value;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ"))
                    {
                        gridItem.Type = "СПВ-2 2014";
                        node = node.Element("СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ");
                        gridItem.RegNum = node.Element("РегистрационныйНомер").Value;
                        gridItem.CntDoc = doc.Root.Element("ПачкаВходящихДокументов").Elements("СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ").Count();
                        gridItem.Insurer = node.Element("НаименованиеКраткое").Value;
                        gridItem.Version = doc.Root.Element("ЗаголовокФайла").Element("ВерсияФормата").Value;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ") && (doc.Descendants().Any(p => p.Name.LocalName == "ТипДокумента") && doc.Descendants().First(p => p.Name.LocalName == "ТипДокумента").Value == "СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"))
                    {
                        gridItem.Type = "СЗВ-6";
                        node = node.Element("ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ").Element("СоставительПачки");
                        gridItem.RegNum = node.Element("РегистрационныйНомер").Value;
                        gridItem.CntDoc = doc.Root.Element("ПачкаВходящихДокументов").Elements("СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ").Count();
                        gridItem.Insurer = node.Element("НаименованиеОрганизации") != null ? node.Element("НаименованиеОрганизации").Value : (node.Element("НаименованиеКраткое") != null ? node.Element("НаименованиеКраткое").Value : "");
                        gridItem.Version = doc.Root.Element("ЗаголовокФайла").Element("ВерсияФормата").Value;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ") && (doc.Descendants().Any(p => p.Name.LocalName == "ТипДокумента") && doc.Descendants().First(p => p.Name.LocalName == "ТипДокумента").Value == "СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"))
                    {
                        gridItem.Type = "СЗВ-6-4";
                        node = node.Element("ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ").Element("СоставительПачки");
                        gridItem.RegNum = node.Element("РегистрационныйНомер").Value;
                        gridItem.CntDoc = doc.Root.Element("ПачкаВходящихДокументов").Elements("СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ").Count();
                        gridItem.Insurer = node.Element("НаименованиеОрганизации") != null ? node.Element("НаименованиеОрганизации").Value : (node.Element("НаименованиеКраткое") != null ? node.Element("НаименованиеКраткое").Value : "");
                        gridItem.Version = doc.Root.Element("ЗаголовокФайла").Element("ВерсияФормата").Value;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "РСВ2_2014"))
                    {
                        node = doc.Descendants().First(p => p.Name.LocalName == "РСВ2_2014");
                        gridItem.Type = "РСВ-2 2014";
                        gridItem.RegNum = node.Element("РегНомерПФР").Value;
                        gridItem.CntDoc = 1;
                        node = node.Element("ФИО");
                        string fio = node.Element("Фамилия") != null ? node.Element("Фамилия").Value : "";
                        fio = fio + (node.Element("Имя") != null ? (" " + node.Element("Имя").Value) : "");
                        fio = fio + (node.Element("Отчество") != null ? (" " + node.Element("Отчество").Value) : "");
                        gridItem.Insurer = fio;
                        gridItem.Version = "";
                        start = 1;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ЭДПФР") && doc.Descendants().Any(p => p.Name.LocalName == "РСВ-2"))
                    {
                        node = doc.Descendants().First(p => p.Name.LocalName == "РСВ-2");
                        gridItem.Type = "РСВ-2 2015";
                        gridItem.RegNum = node.Descendants().First(x => x.Name.LocalName == "РегНомерПФР") != null ? node.Descendants().First(x => x.Name.LocalName == "РегНомерПФР").Value : "";
                        gridItem.CntDoc = 1;
                        node = node.Element("ФИО");
                        string fio = node.Descendants().Any(x => x.Name.LocalName == "Фамилия") ? node.Descendants().First(x => x.Name.LocalName == "Фамилия").Value : "";
                        fio = fio + (node.Descendants().Any(x => x.Name.LocalName == "Имя") ? (" " + node.Descendants().First(x => x.Name.LocalName == "Имя").Value) : "");
                        fio = fio + (node.Descendants().Any(x => x.Name.LocalName == "Отчество") ? (" " + node.Descendants().First(x => x.Name.LocalName == "Отчество").Value) : "");
                        gridItem.Insurer = fio;
                        gridItem.Version = "";
                        start = 1;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ЭДПФР") && doc.Descendants().Any(p => p.Name.LocalName == "РВ-3"))
                    {
                        node = doc.Descendants().First(p => p.Name.LocalName == "РВ-3");
                        gridItem.Type = "РВ-3 2015";
                        gridItem.RegNum = node.Element("РегНомерПФР").Value;
                        gridItem.CntDoc = 1;
                        gridItem.Insurer = node.Element("НаименованиеОрганизации") != null ? node.Element("НаименованиеОрганизации").Value : "";
                        gridItem.Version = "";
                        start = 1;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ЭДПФР") && doc.Descendants().Any(p => p.Name.LocalName == "СЗВ-М"))
                    {
                        node = doc.Descendants().First(p => p.Name.LocalName == "Страхователь");
                        gridItem.Type = "СЗВ-М 2016";
                        gridItem.RegNum = node.Element("РегНомер").Value;
                        gridItem.CntDoc = doc.Descendants().Where(x => x.Name.LocalName == "ЗЛ").Count();
                        gridItem.Insurer = node.Element("НаименованиеКраткое") != null ? node.Element("НаименованиеКраткое").Value : "";
                        gridItem.Version = "";
                        start = 1;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "РеестрДСВ"))
                    {
                        gridItem.Type = "ДСВ-3";
                        gridItem.RegNum = doc.Descendants().Any(p => p.Name.LocalName == "РегистрационныйНомер") ? doc.Descendants().First(p => p.Name.LocalName == "РегистрационныйНомер").Value : "";
                        gridItem.CntDoc = doc.Descendants().Where(x => x.Name.LocalName == "РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ").Count();
                        gridItem.Insurer = doc.Descendants().Any(p => p.Name.LocalName == "НаименованиеКраткое") ? doc.Descendants().First(p => p.Name.LocalName == "НаименованиеКраткое").Value : "";
                        gridItem.Version = doc.Descendants().Any(p => p.Name.LocalName == "ВерсияФормата") ? doc.Descendants().First(p => p.Name.LocalName == "ВерсияФормата").Value : ""; ;
                        start = 1;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ЭДПФР") && doc.Descendants().Any(p => p.Name.LocalName == "ОДВ-1") && doc.Descendants().Any(p => p.Name.LocalName == "СЗВ-СТАЖ"))
                    {
                        node = doc.Descendants().First(p => p.Name.LocalName == "Страхователь");
                        gridItem.Type = "ОДВ-1 СЗВ-СТАЖ";
                        gridItem.RegNum = node.Element("РегНомер").Value;
                        gridItem.CntDoc = doc.Descendants().Where(x => x.Name.LocalName == "ЗЛ").Count();
                        gridItem.Insurer = node.Element("Наименование") != null ? node.Element("Наименование").Value : "";
                        gridItem.Version = "";
                        start = 1;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ЭДПФР") && doc.Descendants().Any(p => p.Name.LocalName == "ОДВ-1") && doc.Descendants().Any(p => p.Name.LocalName == "СЗВ-ИСХ"))
                    {
                        node = doc.Descendants().First(p => p.Name.LocalName == "Страхователь");
                        gridItem.Type = "ОДВ-1 СЗВ-ИСХ";
                        gridItem.RegNum = node.Element("РегНомер").Value;
                        gridItem.CntDoc = doc.Descendants().Where(x => x.Name.LocalName == "ЗЛ").Count();
                        gridItem.Insurer = node.Element("Наименование") != null ? node.Element("Наименование").Value : "";
                        gridItem.Version = "";
                        start = 1;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ЭДПФР") && doc.Descendants().Any(p => p.Name.LocalName == "ОДВ-1") && doc.Descendants().Any(p => p.Name.LocalName == "СЗВ-КОРР"))
                    {
                        node = doc.Descendants().First(p => p.Name.LocalName == "Страхователь");
                        gridItem.Type = "ОДВ-1 СЗВ-КОРР";
                        gridItem.RegNum = node.Element("РегНомер").Value;
                        gridItem.CntDoc = doc.Descendants().Where(x => x.Name.LocalName == "ЗЛ").Count();
                        gridItem.Insurer = node.Element("Наименование") != null ? node.Element("Наименование").Value : "";
                        gridItem.Version = "";
                        start = 1;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ЭДПФР") && doc.Descendants().Any(p => p.Name.LocalName == "РСОР"))
                    {
                        node = doc.Descendants().First(p => p.Name.LocalName == "ЭДПФР");
                        gridItem.Type = "ЗАГС РОЖД";
                        gridItem.RegNum = "";
                        gridItem.CntDoc = doc.Descendants().Where(x => x.Name.LocalName == "СведенияОРождении").Count();
                        gridItem.Insurer = node.Element("СлужебнаяИнформация").Element("Составитель") != null ? node.Element("СлужебнаяИнформация").Element("Составитель").Value : "";
                        gridItem.Version = "";
                        start = 1;
                    }
                    else if (doc.Descendants().Any(p => p.Name.LocalName == "ЭДПФР") && doc.Descendants().Any(p => p.Name.LocalName == "РСОС"))
                    {
                        node = doc.Descendants().First(p => p.Name.LocalName == "ЭДПФР");
                        gridItem.Type = "ЗАГС СМЕРТ";
                        gridItem.RegNum = "";
                        gridItem.CntDoc = doc.Descendants().Where(x => x.Name.LocalName == "СведенияОСмерти").Count();
                        gridItem.Insurer = node.Element("СлужебнаяИнформация").Element("Составитель") != null ? node.Element("СлужебнаяИнформация").Element("Составитель").Value : "";
                        gridItem.Version = "";
                        start = 1;
                    }

                    gridItem.CntImported = 0;



                }
                catch (Exception ex)
                {
                    errBox.Add("Не удалось загрузить файл: " + Path.GetFileName(item) + ". По причине: " + ex.Message);
                    errors = true;
                }

                if (String.IsNullOrEmpty(gridItem.Type))
                {
                    errBox.Add("Ну удалось определить тип данных импортируемого файла: " + Path.GetFileName(item));
                    errors = true;
                }

                bool[] errAr = new bool[2] { false, false };
                List<string> errListTemp = new List<string>();

                for (int i = start; i <= 1; i++)
                {
                    string targetNameSpace = i == 0 ? "" : "http://schema.pfr.ru";
                    //                    if (!errors)
                    //                    {
                    switch (gridItem.Type)
                    {
                        case "РСВ 2014":
                            XML_SchemaName = i == 0 ? "CheckPfr\\РСВ2014.xsd" : "CheckXml\\док_РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.XSD";
                            break;
                        case "РСВ 2015":
                            XML_SchemaName = "CheckPfr\\РСВ2015.xsd";
                            break;
                        case "ПФР 2014":
                            XML_SchemaName = i == 0 ? "CheckPfr\\РСВ2014_Раздел6.XSD" : "CheckXml\\опись_СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.XSD";
                            break;
                        case "СПВ-2 2014":
                            XML_SchemaName = i == 0 ? "CheckPfr\\SPV_2.XSD" : "CheckXml\\док_СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.XSD";
                            break;
                        case "РСВ-2 2014":
                            XML_SchemaName = "CheckXml\\ВСЗЛ\\РСВ\\РСВ-2\\РСВ-2_2014.XSD";
                            break;
                        case "РСВ-2 2015":
                            XML_SchemaName = "РСВ_2_2015\\ВС\\Входящие\\РСВ\\РСВ-2_2015-01-01.xsd";
                            targetNameSpace = "http://пф.рф/ВС/РСВ-2/2015-01-01";
                            break;
                        case "СЗВ-6":
                            XML_SchemaName = i == 0 ? "CheckPfr\\Szv_6.XSD" : "CheckXml\\опись_СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.XSD";
                            break;
                        case "СЗВ-6-4":
                            XML_SchemaName = i == 0 ? "CheckPfr\\Szv_6_4.XSD" : "CheckXml\\опись_СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.XSD";
                            break;
                        case "РВ-3 2015":
                            XML_SchemaName = "РВ3_2015\\ВС\\Входящие\\РСВ\\РВ-3_2015-01-01.xsd";
                            targetNameSpace = "http://пф.рф/ВС/РВ-3/2015-01-01";
                            break;
                        case "СЗВ-М 2016":
                            XML_SchemaName = "СЗВ_М_2016\\ВС\\СЗВ-М_2016-04-01.xsd";
                            targetNameSpace = "http://пф.рф/ВС/СЗВ-М/2016-04-01";
                            break;
                    }
                    string path = Application.StartupPath + "\\XML\\Schemas\\" + XML_SchemaName;
                    if (File.Exists(path))
                    {
                        try
                        {
                            XmlSchemaSet schemas = new XmlSchemaSet();
                            schemas.Add(targetNameSpace, XmlReader.Create(path));
                            doc.Validate(schemas, (o, c) =>
                            {
                                errListTemp.Add("Не удалось загрузить файл: " + Path.GetFileName(item) + ". По причине: Ошибка при проверке по схеме данных. " + c.Message);
                                errAr[i] = true;
                            });
                        }
                        catch (Exception ex)
                        {
                            errListTemp.Add("Не удалось загрузить файл: " + Path.GetFileName(item) + ". По причине: Ошибка при проверке по схеме данных. " + ex.Message);
                            errAr[i] = true;
                        }
                    }
                    //                    }
                }
                if (errAr[0] == true && errAr[1] == true) // если ошибки при проверке по обоим схемам
                {
                    errors = true;
                    foreach (var v in errListTemp)
                    {
                        errBox.Add(v);
                    }
                }


                if (!errors)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.importFilesGrid.MasterView);
                    rowInfo = new GridViewDataRowInfo(this.importFilesGrid.MasterView);
                    FileInfo fn = new FileInfo(item);
                    rowInfo.Cells[0].Value = Color.SkyBlue;
                    rowInfo.Cells["path"].Value = item;
                    rowInfo.Cells["name"].Value = Path.GetFileName(item);
                    rowInfo.Cells["dateTime"].Value = fn.CreationTime.ToShortDateString() + " " + fn.CreationTime.ToShortTimeString();
                    rowInfo.Cells["type"].Value = gridItem.Type;
                    rowInfo.Cells["version"].Value = gridItem.Version;
                    rowInfo.Cells["cntDoc"].Value = gridItem.CntDoc;
                    rowInfo.Cells["cntImported"].Value = gridItem.CntImported;
                    rowInfo.Cells["regNum"].Value = gridItem.RegNum;
                    rowInfo.Cells["insurer"].Value = gridItem.Insurer;

                    importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows.Add(rowInfo); rowIndex = rowInfo.Index; }));

                    //                    importFilesGrid.Rows.Add(rowInfo);
                }
            }


        }


        /// <summary>
        /// Импорт Файлов ПФР Формы РСВ-1 2014 и 2015
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_RSW1_2014(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();

            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            bool result = true;
            short yearType = 2014;
            string xName = string.Empty;
            FormsRSW2014_1_1 rsw = new FormsRSW2014_1_1();

            var ns = doc.Root.GetDefaultNamespace();
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            XElement node = doc.Root;

            try
            {
                string regnum = doc.Descendants().First(p => p.Name.LocalName == "РегистрационныйНомерПФР").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                if (doc.Descendants().Any(x => x.Name.LocalName == "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014"))
                {
                    yearType = 2014;
                    xName = "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014";
                }
                else if (doc.Descendants().Any(x => x.Name.LocalName == "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2015"))
                {
                    yearType = 2015;
                    xName = "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2015";
                }


                node = doc.Descendants().First(x => x.Name.LocalName == xName);

                string[] name = new string[3];
                string inn = "";
                string kpp = "";
                string okwed = "";
                string tel = "";
                byte type_ = (byte)0;

                if (node.Element("НаименованиеОрганизации") != null)
                {
                    type_ = (byte)0;
                    name[0] = node.Element("НаименованиеОрганизации").Value;
                    kpp = node.Element("КПП").Value;
                }
                else if (node.Element("ФИОфизическогоЛица") != null)
                {
                    type_ = (byte)1;
                    name[0] = node.Element("ФИОфизическогоЛица").Element("Фамилия") != null ? node.Element("ФИОфизическогоЛица").Element("Фамилия").Value : "";
                    name[1] = node.Element("ФИОфизическогоЛица").Element("Имя") != null ? node.Element("ФИОфизическогоЛица").Element("Имя").Value : "";
                    name[2] = node.Element("ФИОфизическогоЛица").Element("Отчество") != null ? node.Element("ФИОфизическогоЛица").Element("Отчество").Value : "";
                }
                if (node.Element("КодПоОКВЭД") != null)
                    okwed = node.Element("КодПоОКВЭД").Value;
                if (node.Element("ИННсимвольный") != null)
                    inn = node.Element("ИННсимвольный").Value;
                if (node.Element("Телефон") != null)
                    tel = node.Element("Телефон").Value;


                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (type_ == 0)
                        {
                            if (insurer.NameShort != name[0])
                            {
                                insurer.NameShort = name[0];
                                change = true;
                            }
                            if (insurer.Name != name[0])
                            {
                                insurer.Name = name[0];
                                change = true;
                            }
                        }
                        else
                        {
                            if (insurer.LastName != name[0])
                            {
                                insurer.LastName = name[0];
                                change = true;
                            }
                            if (insurer.FirstName != name[1])
                            {
                                insurer.FirstName = name[1];
                                change = true;
                            }
                            if (insurer.MiddleName != name[2])
                            {
                                insurer.MiddleName = name[2];
                                change = true;
                            }

                        }

                        if (insurer.RegNum != regnum)
                        {
                            insurer.RegNum = regnum;
                            change = true;
                        }
                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }
                        if (insurer.OKWED != okwed)
                        {
                            insurer.OKWED = okwed;
                            change = true;
                        }
                        if (insurer.PhoneContact != tel)
                        {
                            insurer.PhoneContact = tel;
                            change = true;
                        }


                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = type_;
                    if (type_ == 0)
                    {
                        insurer.NameShort = name[0];
                        insurer.Name = name[0];
                    }
                    else
                    {
                        insurer.LastName = name[0];
                        insurer.FirstName = name[1];
                        insurer.MiddleName = name[2];

                    }
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    insurer.OKWED = okwed;
                    insurer.PhoneContact = tel;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }

                byte q = byte.Parse(node.Element("КодОтчетногоПериода").Value.ToString());
                short y = short.Parse(node.Element("КалендарныйГод").Value.ToString());
                byte corrNum = byte.Parse(node.Element("НомерКорректировки").Value.ToString());

                if (db.FormsRSW2014_1_1.Any(x => x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.CorrectionNum == corrNum))
                {
                    FormsRSW2014_1_1 rswForDel = db.FormsRSW2014_1_1.First(x => x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.CorrectionNum == corrNum);
                    bool res = Methods.DeleteRSW1(rswForDel);

                    if (!res)
                    {
                        this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\nПри удалении Формы РСВ-1 произошла ошибка.", this.ThemeName); }));
                        return false;
                    }
                }
                else
                {
                    FormsRSW2014_1_1 rswForDel = new FormsRSW2014_1_1
                    {
                        InsurerID = insurer.ID,
                        Year = y,
                        Quarter = q,
                        CorrectionNum = corrNum
                    };

                    bool res = Methods.DeleteRSW1(rswForDel);

                    if (!res)
                    {
                        this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\nПри удалении Формы РСВ-1 произошла ошибка.", this.ThemeName); }));
                        return false;
                    }
                }

                rsw.CorrectionNum = corrNum;
                rsw.InsurerID = insurer.ID;
                rsw.Year = y;
                rsw.Quarter = q;
                if (node.Element("ТипКорректировки") != null && node.Element("ТипКорректировки").Value.ToString() != "")
                    rsw.CorrectionType = byte.Parse(node.Element("ТипКорректировки").Value.ToString());
                if (node.Element("ПрекращениеДеятельности") != null)
                    rsw.WorkStop = node.Element("ПрекращениеДеятельности").Value.ToString() == "Л" ? (byte)1 : (byte)0;
                else
                    rsw.WorkStop = (byte)0;

                rsw.CountEmployers = node.Element("КоличествоЗЛ") != null ? int.Parse(node.Element("КоличествоЗЛ").Value.ToString()) : 0;
                rsw.CountAverageEmployers = node.Element("СреднесписочнаяЧисленность") != null ? int.Parse(node.Element("СреднесписочнаяЧисленность").Value.ToString()) : 0;
                byte b = 0;
                rsw.CountConfirmDoc = node.Element("КоличествоЛистовПриложения") != null ? (byte.TryParse(node.Element("КоличествоЛистовПриложения").Value.ToString(), out b) ? b : (byte)0) : (byte)0;

                // Что это такое???
                //node.Element("КоличествоСтраниц").Value.ToString()
                //                List<string> strCodes = new List<string> { "100", "110", "111", "112", "113", "114", "120", "121", "130", "140", "141", "142", "143", "144", "150"};

                rsw.ConfirmType = byte.Parse(node.Element("ЛицоПодтверждающееСведения").Value.ToString());

                DateTime dt = DateTime.Now;
                DateTime.TryParse(node.Element("ДатаЗаполнения").Value.ToString(), out dt);
                rsw.DateUnderwrite = dt;

                node = node.Element("ФИОлицаПодтверждающегоСведения");
                if (node != null)
                {
                    rsw.ConfirmLastName = node.Element("Фамилия") != null ? node.Element("Фамилия").Value : "";
                    rsw.ConfirmFirstName = node.Element("Имя") != null ? node.Element("Имя").Value : "";
                    rsw.ConfirmMiddleName = node.Element("Отчество") != null ? node.Element("Отчество").Value : "";
                }
                if (rsw.ConfirmType > 1)
                {
                    if (node.Parent.Element("НаименованиеОрганизацииПредставителя") != null)
                    {
                        node = node.Parent.Element("НаименованиеОрганизацииПредставителя");
                        rsw.ConfirmOrgName = node.Value;
                    }
                    if (node.Parent.Element("ДокументПодтверждающийПолномочияПредставителя") != null)
                    {
                        node = node.Parent.Element("ДокументПодтверждающийПолномочияПредставителя");
                        string docName = node.Element("НаименованиеУдостоверяющего").Value;
                        if (db.DocumentTypes.Any(x => x.Code == docName))
                        {
                            rsw.ConfirmDocType_ID = db.DocumentTypes.FirstOrDefault(x => x.Code == docName).ID;
                        }
                        else
                        {
                            rsw.ConfirmDocType_ID = db.DocumentTypes.FirstOrDefault(x => x.Code == "ПРОЧЕЕ").ID;
                            rsw.ConfirmDocName = docName;
                        }
                        rsw.ConfirmDocSerLat = node.Element("СерияРимскиеЦифры") != null ? node.Element("СерияРимскиеЦифры").Value : "";
                        rsw.ConfirmDocSerRus = node.Element("СерияРусскиеБуквы") != null ? node.Element("СерияРусскиеБуквы").Value : "";
                        int nn = 0;
                        if (node.Element("НомерУдостоверяющего") != null)
                        {
                            int.TryParse(node.Element("НомерУдостоверяющего").Value.ToString(), out nn);
                        }
                        rsw.ConfirmDocNum = nn;
                        rsw.ConfirmDocDate = DateTime.Parse(node.Element("ДатаВыдачи").Value.ToString());
                        rsw.ConfirmDocKemVyd = node.Element("КемВыдан").Value;

                    }
                }

                xName = yearType != 2015 ? "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014" : "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2015";
                node = doc.Descendants().First(x => x.Name.LocalName == xName);

                #region Раздел1РасчетПоНачисленнымУплаченным2014 - 2015
                xName = "Раздел1РасчетПоНачисленнымУплаченным2014";
                if (node.Element(xName) != null)
                {
                    XElement Раздел1РасчетПоНачисленнымУплаченным2014 = node.Element(xName);
                    var nodes = Раздел1РасчетПоНачисленнымУплаченным2014.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                    foreach (var n in nodes)
                    {
                        string strCode = n.Value.ToString();

                        foreach (var item_ in n.Parent.Elements())
                        {
                            string itemName = "s_" + strCode + "_";
                            decimal data = 0;
                            if (item_.Name.LocalName != "КодСтроки")
                            {
                                int i = -1;
                                data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);

                                switch (item_.Name.LocalName)
                                {
                                    case "СтраховыеВзносыОПС":
                                        i = 0;
                                        break;
                                    case "ОПСстраховаяЧасть":
                                        i = 1;
                                        break;
                                    case "ОПСнакопительнаяЧасть":
                                        i = 2;
                                        break;
                                    case "ВзносыПоДопТарифу1":
                                        i = 3;
                                        break;
                                    case "ВзносыПоДопТарифу2_18":
                                        i = 4;
                                        break;
                                    case "СтраховыеВзносыОМС":
                                        i = 5;
                                        break;
                                }
                                if (i >= 0)
                                {
                                    itemName = itemName + i.ToString();
                                    rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                }
                            }
                        }

                    }
                }
                #endregion

                #region Раздел2РасчетПоТарифуИдопТарифу

                if (node.Element("Раздел2РасчетПоТарифуИдопТарифу") != null)
                {
                    XElement Раздел2РасчетПоТарифуИдопТарифу = node.Element("Раздел2РасчетПоТарифуИдопТарифу");

                    #region Раздел 2.1
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_1") != null)
                    {
                        var razd_2_1_list = Раздел2РасчетПоТарифуИдопТарифу.Descendants().Where(x => x.Name.LocalName == "Раздел_2_1");
                        short yearType_dop = ((rsw.Year == (short)2014) || (rsw.Year == (short)2015 && rsw.Quarter == 3)) ? (short)2014 : (short)2015;

                        foreach (var razd_2_1 in razd_2_1_list)
                        {


                            FormsRSW2014_1_Razd_2_1 rsw_2_1 = new FormsRSW2014_1_Razd_2_1();
                            rsw_2_1.SetZeroValues();

                            rsw_2_1.Year = rsw.Year;
                            rsw_2_1.Quarter = rsw.Quarter;
                            rsw_2_1.CorrectionNum = rsw.CorrectionNum;
                            rsw_2_1.InsurerID = rsw.InsurerID;
                            rsw_2_1.AutoCalc = true;

                            string tarCode = razd_2_1.Element("КодТарифа").Value.ToString();
                            if (db.TariffCode.Any(x => x.Code == tarCode))
                            {
                                rsw_2_1.TariffCodeID = db.TariffCode.First(x => x.Code == tarCode).ID;
                            }
                            else
                                rsw_2_1.TariffCodeID = 0;

                            var nodes = razd_2_1.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                            foreach (var n in nodes)
                            {
                                string strCode = n.Value.ToString();

                                //                            if (strCode == "215" && yearType == 2015)
                                //                                strCode = strCode + "i";
                                if (yearType == 2014 && yearType_dop == 2015)
                                {
                                    if (strCode == "214")
                                        strCode = "213";
                                    if (strCode == "215")
                                        strCode = "214";
                                }
                                else if (strCode == "215" && yearType == 2015)
                                    strCode = strCode + "i";

                                if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                                {
                                    foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                    {

                                        string itemName = "s_" + strCode + "_";
                                        decimal data = 0;
                                        int i = -1;
                                        if (item_.Value != null)
                                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                        else
                                            data = 0;

                                        switch (item_.Name.LocalName)
                                        {
                                            case "СуммаВсегоСначалаРасчетногоПериода":
                                                i = 0;
                                                break;
                                            case "СуммаПоследние1месяц":
                                                i = 1;
                                                break;
                                            case "СуммаПоследние2месяц":
                                                i = 2;
                                                break;
                                            case "СуммаПоследние3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw_2_1.GetType().GetProperty(itemName).SetValue(rsw_2_1, data, null);
                                        }
                                    }
                                }
                                else // если указана численность
                                {
                                    foreach (var item_ in n.Parent.Elements())
                                    {
                                        long data = 0;



                                        string itemName = "s_" + strCode + "_";
                                        if (item_.Name.LocalName != "КодСтроки")
                                        {
                                            int i = -1;
                                            if (item_.Value != null)
                                                data = long.Parse(item_.Value.ToString());
                                            else
                                                data = 0;

                                            switch (item_.Name.LocalName)
                                            {
                                                case "КоличествоЗЛ_Всего":
                                                    i = 0;
                                                    break;
                                                case "КоличествоЗЛ_1месяц":
                                                    i = 1;
                                                    break;
                                                case "КоличествоЗЛ_2месяц":
                                                    i = 2;
                                                    break;
                                                case "КоличествоЗЛ_3месяц":
                                                    i = 3;
                                                    break;
                                            }
                                            if (i >= 0)
                                            {
                                                itemName = itemName + i.ToString();
                                                rsw_2_1.GetType().GetProperty(itemName).SetValue(rsw_2_1, data, null);
                                            }
                                        }
                                    }
                                }

                            }

                            db.AddToFormsRSW2014_1_Razd_2_1(rsw_2_1);
                        }
                    }
                    #endregion

                    #region Раздел_2_2
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_2") != null)
                    {
                        XElement Раздел_2_2 = Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_2");
                        var nodes = Раздел_2_2.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                {
                                    string itemName = "s_" + strCode + "_";
                                    decimal data = 0;
                                    int i = -1;
                                    data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);

                                    switch (item_.Name.LocalName)
                                    {
                                        case "СуммаВсегоСначалаРасчетногоПериода":
                                            i = 0;
                                            break;
                                        case "СуммаПоследние1месяц":
                                            i = 1;
                                            break;
                                        case "СуммаПоследние2месяц":
                                            i = 2;
                                            break;
                                        case "СуммаПоследние3месяц":
                                            i = 3;
                                            break;
                                    }
                                    if (i >= 0)
                                    {
                                        itemName = itemName + i.ToString();
                                        rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                    }
                                }
                            }
                            else // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛ_Всего":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛ_1месяц":
                                                i = 1;
                                                break;
                                            case "КоличествоЗЛ_2месяц":
                                                i = 2;
                                                break;
                                            case "КоличествоЗЛ_3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                        }
                                    }
                                }
                            }

                        }

                        if (rsw.s_220_0 != 0)
                            rsw.ExistPart_2_2 = (byte)1;
                    }

                    #endregion

                    #region Раздел_2_3
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_3") != null)
                    {
                        XElement Раздел_2_3 = Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_3");
                        var nodes = Раздел_2_3.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                {
                                    string itemName = "s_" + strCode + "_";
                                    decimal data = 0;
                                    int i = -1;
                                    data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);

                                    switch (item_.Name.LocalName)
                                    {
                                        case "СуммаВсегоСначалаРасчетногоПериода":
                                            i = 0;
                                            break;
                                        case "СуммаПоследние1месяц":
                                            i = 1;
                                            break;
                                        case "СуммаПоследние2месяц":
                                            i = 2;
                                            break;
                                        case "СуммаПоследние3месяц":
                                            i = 3;
                                            break;
                                    }
                                    if (i >= 0)
                                    {
                                        itemName = itemName + i.ToString();
                                        rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                    }
                                }
                            }
                            else // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛ_Всего":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛ_1месяц":
                                                i = 1;
                                                break;
                                            case "КоличествоЗЛ_2месяц":
                                                i = 2;
                                                break;
                                            case "КоличествоЗЛ_3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                        }
                                    }
                                }
                            }

                        }

                        if (rsw.s_230_0 != 0)
                            rsw.ExistPart_2_3 = (byte)1;

                    }
                    #endregion

                    #region Раздел 2.4
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_4") != null)
                    {
                        var razd_2_4_list = Раздел2РасчетПоТарифуИдопТарифу.Descendants().Where(x => x.Name.LocalName == "Раздел_2_4");
                        foreach (var razd_2_4 in razd_2_4_list)
                        {
                            FormsRSW2014_1_Razd_2_4 rsw_2_4 = new FormsRSW2014_1_Razd_2_4();
                            rsw_2_4.SetZeroValues();

                            rsw_2_4.Year = rsw.Year;
                            rsw_2_4.Quarter = rsw.Quarter;
                            rsw_2_4.CorrectionNum = rsw.CorrectionNum;
                            rsw_2_4.InsurerID = rsw.InsurerID;
                            rsw_2_4.AutoCalc = false;
                            rsw_2_4.CodeBase = byte.Parse(razd_2_4.Element("КодОснованияРасчетаПоДопТарифу").Value.ToString());

                            switch (razd_2_4.Element("ОснованиеЗаполненияРаздела2_4").Value.ToString())
                            {
                                case "РЕЗУЛЬТАТЫ СПЕЦОЦЕНКИ":
                                    rsw_2_4.FilledBase = (byte)0;
                                    break;
                                case "РЕЗУЛЬТАТЫ АТТЕСТАЦИИ РАБОЧИХ МЕСТ":
                                    rsw_2_4.FilledBase = (byte)1;
                                    break;
                                case "РЕЗУЛЬТАТЫ СПЕЦОЦЕНКИ И РЕЗУЛЬТАТЫ АТТЕСТАЦИИ РАБОЧИХ МЕСТ":
                                    rsw_2_4.FilledBase = (byte)2;
                                    break;
                            }

                            var nodes = razd_2_4.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                            foreach (var n in nodes)
                            {
                                string strCode = n.Value.ToString();


                                if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                                {
                                    foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                    {
                                        string itemName = "s_" + strCode + "_";
                                        decimal data = 0;
                                        int i = -1;
                                        if (item_.Value != null)
                                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                        else
                                            data = 0;

                                        switch (item_.Name.LocalName)
                                        {
                                            case "СуммаВсегоСначалаРасчетногоПериода":
                                                i = 0;
                                                break;
                                            case "СуммаПоследние1месяц":
                                                i = 1;
                                                break;
                                            case "СуммаПоследние2месяц":
                                                i = 2;
                                                break;
                                            case "СуммаПоследние3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw_2_4.GetType().GetProperty(itemName).SetValue(rsw_2_4, data, null);
                                        }
                                    }
                                }
                                else // если указана численность
                                {
                                    foreach (var item_ in n.Parent.Elements())
                                    {
                                        long data = 0;
                                        string itemName = "s_" + strCode + "_";
                                        if (item_.Name.LocalName != "КодСтроки")
                                        {
                                            int i = -1;
                                            if (item_.Value != null)
                                                data = long.Parse(item_.Value.ToString());
                                            else
                                                data = 0;

                                            switch (item_.Name.LocalName)
                                            {
                                                case "КоличествоЗЛ_Всего":
                                                    i = 0;
                                                    break;
                                                case "КоличествоЗЛ_1месяц":
                                                    i = 1;
                                                    break;
                                                case "КоличествоЗЛ_2месяц":
                                                    i = 2;
                                                    break;
                                                case "КоличествоЗЛ_3месяц":
                                                    i = 3;
                                                    break;
                                            }
                                            if (i >= 0)
                                            {
                                                itemName = itemName + i.ToString();
                                                rsw_2_4.GetType().GetProperty(itemName).SetValue(rsw_2_4, data, null);
                                            }
                                        }
                                    }
                                }

                            }

                            db.AddToFormsRSW2014_1_Razd_2_4(rsw_2_4);
                        }

                        rsw.ExistPart_2_4 = (byte)1;

                    }
                    #endregion

                    #region Раздел 2.5
                    if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5") != null)
                    {
                        if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5").Element("ПереченьПачекИсходныхСведенийПУ") != null)
                        {
                            XElement ПереченьПачекИсходныхСведенийПУ = Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5").Element("ПереченьПачекИсходныхСведенийПУ");
                            if (ПереченьПачекИсходныхСведенийПУ.Element("СведенияОпачкеИсходных") != null)
                            {
                                var razd_2_5_1_list = ПереченьПачекИсходныхСведенийПУ.Descendants().Where(x => x.Name.LocalName == "СведенияОпачкеИсходных");
                                foreach (var razd_2_5_1 in razd_2_5_1_list)
                                {
                                    int n = 0;
                                    decimal d = 0;
                                    FormsRSW2014_1_Razd_2_5_1 rsw_2_5_1 = new FormsRSW2014_1_Razd_2_5_1
                                    {
                                        Year = rsw.Year,
                                        Quarter = rsw.Quarter,
                                        CorrectionNum = rsw.CorrectionNum,
                                        InsurerID = rsw.InsurerID,
                                        NumRec = razd_2_5_1.Element("НомерПП").Value != null ? (int.TryParse(razd_2_5_1.Element("НомерПП").Value.ToString(), out n) ? n : 0) : 0,
                                        Col_2 = razd_2_5_1.Element("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную").Value != null ? (decimal.TryParse(razd_2_5_1.Element("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_3 = razd_2_5_1.Element("СтраховыхВзносовОПС").Value != null ? (decimal.TryParse(razd_2_5_1.Element("СтраховыхВзносовОПС").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_4 = razd_2_5_1.Element("КоличествоЗЛвПачке").Value != null ? (int.TryParse(razd_2_5_1.Element("КоличествоЗЛвПачке").Value.ToString(), out n) ? n : 0) : 0,
                                        Col_5 = razd_2_5_1.Element("ИмяФайла").Value != null ? razd_2_5_1.Element("ИмяФайла").Value.ToString() : ""
                                    };

                                    db.AddToFormsRSW2014_1_Razd_2_5_1(rsw_2_5_1);
                                }

                            }


                        }

                        if (Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5").Element("ПереченьПачекКорректирующихСведенийПУ") != null)
                        {
                            XElement ПереченьПачекКорректирующихСведенийПУ = Раздел2РасчетПоТарифуИдопТарифу.Element("Раздел_2_5").Element("ПереченьПачекКорректирующихСведенийПУ");
                            if (ПереченьПачекКорректирующихСведенийПУ.Element("СведенияОпачкеКорректирующих") != null)
                            {
                                var razd_2_5_2_list = ПереченьПачекКорректирующихСведенийПУ.Descendants().Where(x => x.Name.LocalName == "СведенияОпачкеКорректирующих");
                                foreach (var razd_2_5_2 in razd_2_5_2_list)
                                {
                                    int n = 0;
                                    decimal d = 0;
                                    b = 0;
                                    short y_ = 0;
                                    FormsRSW2014_1_Razd_2_5_2 rsw_2_5_2 = new FormsRSW2014_1_Razd_2_5_2
                                    {
                                        Year = rsw.Year,
                                        Quarter = rsw.Quarter,
                                        CorrectionNum = rsw.CorrectionNum,
                                        InsurerID = rsw.InsurerID,
                                        NumRec = razd_2_5_2.Element("НомерПП").Value != null ? (int.TryParse(razd_2_5_2.Element("НомерПП").Value.ToString(), out n) ? n : 0) : 0,
                                        Col_4 = (razd_2_5_2.Element("ДоначисленоСтраховыхВзносовОПС") != null && razd_2_5_2.Element("ДоначисленоСтраховыхВзносовОПС").Value != null) ? (decimal.TryParse(razd_2_5_2.Element("ДоначисленоСтраховыхВзносовОПС").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_5 = (razd_2_5_2.Element("ДоначисленоНаСтраховуюЧасть") != null && razd_2_5_2.Element("ДоначисленоНаСтраховуюЧасть").Value != null) ? (decimal.TryParse(razd_2_5_2.Element("ДоначисленоНаСтраховуюЧасть").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_6 = (razd_2_5_2.Element("ДоначисленоНаНакопительнуюЧасть") != null && razd_2_5_2.Element("ДоначисленоНаНакопительнуюЧасть").Value != null) ? (decimal.TryParse(razd_2_5_2.Element("ДоначисленоНаНакопительнуюЧасть").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out d) ? d : 0) : 0,
                                        Col_7 = razd_2_5_2.Element("КоличествоЗЛвПачке").Value != null ? (int.TryParse(razd_2_5_2.Element("КоличествоЗЛвПачке").Value.ToString(), out n) ? n : 0) : 0,
                                        Col_8 = razd_2_5_2.Element("ИмяФайла").Value != null ? razd_2_5_2.Element("ИмяФайла").Value.ToString() : "",
                                        Col_2_QuarterKorr = razd_2_5_2.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value != null ? (byte.TryParse(razd_2_5_2.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value.ToString(), out b) ? b : (byte)0) : (byte)0,
                                        Col_3_YearKorr = razd_2_5_2.Element("КорректируемыйОтчетныйПериод").Element("Год").Value != null ? (short.TryParse(razd_2_5_2.Element("КорректируемыйОтчетныйПериод").Element("Год").Value.ToString(), out y_) ? y_ : (short)0) : (short)0
                                    };


                                    db.AddToFormsRSW2014_1_Razd_2_5_2(rsw_2_5_2);
                                }

                            }


                        }

                    }
                    #endregion


                }
                #endregion

                #region Раздел3РасчетНаПравоПримененияПониженногоТарифа2014

                xName = "Раздел3РасчетНаПравоПримененияПониженногоТарифа" + yearType.ToString();

                if (node.Element(xName) != null)
                {
                    XElement Раздел3РасчетНаПравоПримененияПониженногоТарифа2014 = node.Element(xName);

                    #region Раздел3_1_ДляОбщественныхОрганизацийИнвалидов
                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_1_ДляОбщественныхОрганизацийИнвалидов") != null)
                    {
                        XElement Раздел3_1_ДляОбщественныхОрганизацийИнвалидов = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_1_ДляОбщественныхОрганизацийИнвалидов");
                        var nodes = Раздел3_1_ДляОбщественныхОрганизацийИнвалидов.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("КоличествоЗЛ_Всего") != null) // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛ_Всего":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛ_1месяц":
                                                i = 1;
                                                break;
                                            case "КоличествоЗЛ_2месяц":
                                                i = 2;
                                                break;
                                            case "КоличествоЗЛ_3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                        }
                                    }
                                }
                            }


                        }
                        rsw.ExistPart_3_1 = (byte)1;

                    }

                    #endregion

                    #region Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов
                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов") != null)
                    {
                        XElement Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов");
                        var nodes = Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("КоличествоЗЛ_Всего") != null) // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛ_Всего":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛ_1месяц":
                                                i = 1;
                                                break;
                                            case "КоличествоЗЛ_2месяц":
                                                i = 2;
                                                break;
                                            case "КоличествоЗЛ_3месяц":
                                                i = 3;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                        }
                                    }
                                }
                            }

                            if (n.Parent.Element("РасчетСумм") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Element("РасчетСумм").Elements())
                                {
                                    string itemName = "s_" + strCode + "_";
                                    decimal data = 0;
                                    int i = -1;
                                    if (item_.Value != null)
                                        data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                    else
                                        data = 0;

                                    switch (item_.Name.LocalName)
                                    {
                                        case "СуммаВсегоСначалаРасчетногоПериода":
                                            i = 0;
                                            break;
                                        case "СуммаПоследние1месяц":
                                            i = 1;
                                            break;
                                        case "СуммаПоследние2месяц":
                                            i = 2;
                                            break;
                                        case "СуммаПоследние3месяц":
                                            i = 3;
                                            break;
                                    }
                                    if (i >= 0)
                                    {
                                        itemName = itemName + i.ToString();
                                        rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                    }
                                }
                            }
                        }
                        rsw.ExistPart_3_2 = (byte)1;

                    }

                    #endregion

                    #region Раздел3_3_ДляОрганизацийИТ (2014) и Раздел3_1_ДляОрганизацийИТ для 2015
                    xName = "Раздел3_3_ДляОрганизацийИТ";
                    /*    if (yearType == 2014)
                        {
                            xName = "Раздел3_3_ДляОрганизацийИТ";
                        }
                        else if (yearType == 2015)
                        {
                            xName = "Раздел3_1_ДляОрганизацийИТ";
                        }
                      */
                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName) != null)
                    {
                        XElement Раздел3_3_ДляОрганизацийИТ = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName);
                        var nodes = Раздел3_3_ДляОрганизацийИТ.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("КоличествоЗЛпоПредшествующему") != null) // если указана численность
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    long data = 0;
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        int i = -1;
                                        data = long.Parse(item_.Value.ToString());

                                        switch (item_.Name.LocalName)
                                        {
                                            case "КоличествоЗЛпоПредшествующему":
                                                i = 0;
                                                break;
                                            case "КоличествоЗЛпоТекущему":
                                                i = 1;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                        }
                                    }
                                }
                            }

                            if (n.Parent.Element("СуммаДоходаПоПредшествующему") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        string itemName = "s_" + strCode + "_";
                                        decimal data = 0;
                                        int i = -1;
                                        if (item_.Value != null)
                                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                        else
                                            data = 0;

                                        switch (item_.Name.LocalName)
                                        {
                                            case "СуммаДоходаПоПредшествующему":
                                                i = 0;
                                                break;
                                            case "СуммаДоходаПоТекущему":
                                                i = 1;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                        }
                                    }
                                }
                            }


                            if (n.Parent.Element("ДатаЗаписиВреестре") != null) // если указаны СведенияИзРеестра
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    string itemName = "s_" + strCode + "_";
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        switch (item_.Name.LocalName)
                                        {
                                            case "ДатаЗаписиВреестре":
                                                DateTime data;

                                                if (DateTime.TryParse(item_.Value.ToString(), out data))
                                                {
                                                    itemName = itemName + "0";
                                                    rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                                }
                                                break;
                                            case "НомерЗаписиВреестре":
                                                string data_ = !String.IsNullOrEmpty(item_.Value.ToString()) ? item_.Value.ToString() : "";
                                                itemName = itemName + "1";
                                                rsw.GetType().GetProperty(itemName).SetValue(rsw, data_, null);
                                                break;
                                        }
                                    }
                                }
                            }

                        }
                        rsw.ExistPart_3_3 = (byte)1;
                    }

                    #endregion

                    #region Раздел3_4_ДляОрганизацийСМИ
                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_4_ДляОрганизацийСМИ") != null)
                    {
                        XElement Раздел3_4_ДляОрганизацийСМИ = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element("Раздел3_4_ДляОрганизацийСМИ");

                        var razd_3_4_list = Раздел3_4_ДляОрганизацийСМИ.Descendants().Where(x => x.Name.LocalName == "СведенияПоВидуДеятельности");
                        foreach (var razd_3_4 in razd_3_4_list)
                        {
                            FormsRSW2014_1_Razd_3_4 rsw_3_4 = new FormsRSW2014_1_Razd_3_4
                            {
                                Year = rsw.Year,
                                Quarter = rsw.Quarter,
                                CorrectionNum = rsw.CorrectionNum,
                                InsurerID = rsw.InsurerID,
                                NumOrd = long.Parse(razd_3_4.Element("НомерПП").Value.ToString()),
                                NameOKWED = razd_3_4.Element("НаименованиеВидаЭД").Value.ToString(),
                                OKWED = razd_3_4.Element("КодПоОКВЭД").Value.ToString(),
                                Income = decimal.Parse(razd_3_4.Element("ДоходыПоВидуЭД").Value.ToString(), CultureInfo.InvariantCulture),
                                RateIncome = decimal.Parse(razd_3_4.Element("ДоляДоходовПоВидуЭД").Value.ToString(), CultureInfo.InvariantCulture)
                            };

                            db.AddToFormsRSW2014_1_Razd_3_4(rsw_3_4);
                        }

                        rsw.s_351_0 = DateTime.Parse(Раздел3_4_ДляОрганизацийСМИ.Element("СведенияИзРеестраСМИ").Element("ДатаЗаписиВреестре").Value.ToString());
                        rsw.s_351_1 = Раздел3_4_ДляОрганизацийСМИ.Element("СведенияИзРеестраСМИ").Element("НомерЗаписиВреестре").Value.ToString();

                        rsw.ExistPart_3_4 = (byte)1;
                    }
                    #endregion

                    #region Раздел3_5_ДляОрганизацийПрименяющихУСН (2014) и Раздел3_2_ДляОрганизацийПрименяющихУСН для 2015

                    xName = "Раздел3_5_ДляОрганизацийПрименяющихУСН";
                    /*      if (yearType == 2014)
                          {
                              xName = "Раздел3_5_ДляОрганизацийПрименяющихУСН";
                          }
                          else if (yearType == 2015)
                          {
                              xName = "Раздел3_2_ДляОрганизацийПрименяющихУСН";
                          }
                          */
                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName) != null)
                    {
                        XElement Раздел3_5_ДляОрганизацийПрименяющихУСН = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName);

                        rsw.s_361_0 = decimal.Parse(Раздел3_5_ДляОрганизацийПрименяющихУСН.Element("СуммаДоходаПоСтатье346_15НКвсего").Element("СуммаДохода").Value.ToString(), CultureInfo.InvariantCulture);
                        rsw.s_362_0 = decimal.Parse(Раздел3_5_ДляОрганизацийПрименяющихУСН.Element("СуммаДоходаПоСтатье58ИзНих").Element("СуммаДохода").Value.ToString(), CultureInfo.InvariantCulture);
                        rsw.ExistPart_3_5 = (byte)1;
                    }
                    #endregion

                    #region Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН 2014 и Раздел3_3_ДляНекоммерческихОрганизацийПрименяющихУСН для 2015
                    xName = "Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН";
                    /*       if (yearType == 2014)
                           {
                               xName = "Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН";
                           }
                           else if (yearType == 2015)
                           {
                               xName = "Раздел3_3_ДляНекоммерческихОрганизацийПрименяющихУСН";
                           }*/
                    if (Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName) != null)
                    {
                        XElement Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН = Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Element(xName);
                        var nodes = Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                        foreach (var n in nodes)
                        {
                            string strCode = n.Value.ToString();

                            if (n.Parent.Element("СуммаДоходаПоПредшествующему") != null) // если указаны суммы
                            {
                                foreach (var item_ in n.Parent.Elements())
                                {
                                    if (item_.Name.LocalName != "КодСтроки")
                                    {
                                        string itemName = "s_" + strCode + "_";
                                        decimal data = 0;
                                        int i = -1;
                                        if (item_.Value != null)
                                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);
                                        else
                                            data = 0;

                                        switch (item_.Name.LocalName)
                                        {
                                            case "СуммаДоходаПоПредшествующему":
                                                i = 0;
                                                break;
                                            case "СуммаДоходаПоТекущему":
                                                i = 1;
                                                break;
                                        }
                                        if (i >= 0)
                                        {
                                            itemName = itemName + i.ToString();
                                            rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                                        }
                                    }
                                }
                            }


                        }
                        rsw.ExistPart_3_6 = (byte)1;
                    }

                    #endregion


                }
                #endregion

                #region Раздел4СуммыДоначисленныхСтраховыхВзносов2014
                xName = yearType != 2015 ? "Раздел4СуммыДоначисленныхСтраховыхВзносов2014" : "Раздел4";
                if (node.Element(xName) != null)
                {
                    XElement Раздел4СуммыДоначисленныхСтраховыхВзносов2014 = node.Element(xName);

                    var razd_4_list = Раздел4СуммыДоначисленныхСтраховыхВзносов2014.Descendants().Where(x => x.Name.LocalName == "СуммаДоначисленныхВзносовЗаПериодНачинаяС2014");
                    foreach (var razd_4 in razd_4_list)
                    {
                        FormsRSW2014_1_Razd_4 rsw_4 = new FormsRSW2014_1_Razd_4
                        {
                            Year = rsw.Year,
                            Quarter = rsw.Quarter,
                            CorrectionNum = rsw.CorrectionNum,
                            InsurerID = rsw.InsurerID,
                            NumOrd = long.Parse(razd_4.Element("НомерПП").Value.ToString()),
                            Base = byte.Parse(razd_4.Element("ОснованиеДляДоначисления").Value.ToString()),
                            CodeBase = razd_4.Element("КодОснованияДляДопТарифа") != null ? byte.Parse(razd_4.Element("КодОснованияДляДопТарифа").Value.ToString()) : (byte)1,
                            YearPer = short.Parse(razd_4.Element("Год").Value.ToString()),
                            MonthPer = byte.Parse(razd_4.Element("Месяц").Value.ToString()),
                            Strah2014 = razd_4.Element("СуммаДоначисленныхВзносовОПС2014всего") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовОПС2014всего").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            StrahMoreBase2014 = razd_4.Element("СуммаДоначисленныхВзносовОПС2014превыщающие") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовОПС2014превыщающие").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Strah2013 = razd_4.Element("СуммаДоначисленныхВзносовНаСтраховуюВсего") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовНаСтраховуюВсего").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            StrahMoreBase2013 = razd_4.Element("СуммаДоначисленныхВзносовНаСтраховуюПревышающие") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовНаСтраховуюПревышающие").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Nakop2013 = razd_4.Element("СуммаДоначисленныхВзносовНаНакопительную") != null ? decimal.Parse(razd_4.Element("СуммаДоначисленныхВзносовНаНакопительную").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Dop1 = razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ1") != null ? decimal.Parse(razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ1").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Dop2 = razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2") != null ? decimal.Parse(razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            Dop21 = razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2_1") != null ? decimal.Parse(razd_4.Element("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2_1").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            OMS = razd_4.Element("СтраховыеВзносыОМС") != null ? decimal.Parse(razd_4.Element("СтраховыеВзносыОМС").Value.ToString(), CultureInfo.InvariantCulture) : 0
                        };

                        db.AddToFormsRSW2014_1_Razd_4(rsw_4);
                    }

                    rsw.ExistPart_4 = (byte)1;

                }
                #endregion

                #region Раздел5СведенияОВыплатахВпользуОбучающихся2014
                if (node.Element("Раздел5СведенияОВыплатахВпользуОбучающихся2014") != null)
                {
                    XElement Раздел5СведенияОВыплатахВпользуОбучающихся2014 = node.Element("Раздел5СведенияОВыплатахВпользуОбучающихся2014");

                    var razd_5_list = Раздел5СведенияОВыплатахВпользуОбучающихся2014.Descendants().Where(x => x.Name.LocalName == "СведенияОбОбучающемся");
                    foreach (var razd_5 in razd_5_list)
                    {
                        FormsRSW2014_1_Razd_5 rsw_5 = new FormsRSW2014_1_Razd_5
                        {
                            Year = rsw.Year,
                            Quarter = rsw.Quarter,
                            CorrectionNum = rsw.CorrectionNum,
                            InsurerID = rsw.InsurerID,
                            NumOrd = long.Parse(razd_5.Element("НомерПП").Value.ToString()),
                            LastName = razd_5.Element("ФИО").Element("Фамилия") != null ? razd_5.Element("ФИО").Element("Фамилия").Value.ToString() : "",
                            FirstName = razd_5.Element("ФИО").Element("Имя") != null ? razd_5.Element("ФИО").Element("Имя").Value.ToString() : "",
                            MiddleName = razd_5.Element("ФИО").Element("Отчество") != null ? razd_5.Element("ФИО").Element("Отчество").Value.ToString() : "",
                            NumSpravka = razd_5.Element("НомерСправкиОчленствеВстудОтряде").Value.ToString(),
                            DateSpravka = DateTime.Parse(razd_5.Element("ДатаВыдачиСправкиОчленствеВстудОтряде").Value.ToString()),
                            NumSpravka1 = razd_5.Element("НомерСправкиОбОчномОбучении").Value.ToString(),
                            DateSpravka1 = DateTime.Parse(razd_5.Element("ДатаВыдачиСправкиОбОчномОбучении").Value.ToString()),
                            SumPay = razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаВсегоСначалаРасчетногоПериода") != null ? decimal.Parse(razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаВсегоСначалаРасчетногоПериода").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            SumPay_0 = razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние1месяц") != null ? decimal.Parse(razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние1месяц").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            SumPay_1 = razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние2месяц") != null ? decimal.Parse(razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние2месяц").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                            SumPay_2 = razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние3месяц") != null ? decimal.Parse(razd_5.Element("СуммыВыплатИвознаграждений").Element("СуммаПоследние3месяц").Value.ToString(), CultureInfo.InvariantCulture) : 0,
                        };

                        db.AddToFormsRSW2014_1_Razd_5(rsw_5);
                    }

                    XElement СведенияИзРеестраМДОО = Раздел5СведенияОВыплатахВпользуОбучающихся2014.Element("СведенияИзРеестраМДОО");

                    var nodes = СведенияИзРеестраМДОО.Descendants().Where(x => x.Name.LocalName == "РеквизитыЗаписиВреестре");
                    var i = 0;
                    foreach (var item in nodes)
                    {
                        DateTime ДатаЗаписиВреестре = DateTime.Parse(item.Element("ДатаЗаписиВреестре").Value.ToString());
                        string НомерЗаписиВреестре = item.Element("НомерЗаписиВреестре").Value.ToString();
                        string itemName_date = "s_501_0_" + i;
                        string itemName_num = "s_501_1_" + i;
                        rsw.GetType().GetProperty(itemName_date).SetValue(rsw, ДатаЗаписиВреестре, null);
                        rsw.GetType().GetProperty(itemName_num).SetValue(rsw, НомерЗаписиВреестре, null);

                        i++;
                    }

                    rsw.ExistPart_5 = (byte)1;
                }
                #endregion

                db.FormsRSW2014_1_1.AddObject(rsw);
                db.SaveChanges();
                result = true;

            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                bool res = Methods.DeleteRSW1(rsw);

                result = false;
            }

            return result;
        }

        public class staffContainer
        {
            public long insID { get; set; }
            public string insuranceNum { get; set; }
            public string lastName { get; set; }
            public string firstName { get; set; }
            public string middleName { get; set; }
            public byte? contrNum { get; set; }
            public byte? dismissed { get; set; }
        }

        /// <summary>
        /// Импорт файлов ПФР Инд.Сведения 2014
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_RSW1_6_1_2014(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            bool result = true;
            var ns = doc.Root.GetDefaultNamespace();
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            XElement node = doc.Root;


            try
            {
                #region // Поиск информации о составителе
                node = node.Descendants().First(x => x.Name.LocalName == "СоставительПачки");
                string inn = node.Element("НалоговыйНомер").Element("ИНН").Value;
                string kpp = node.Element("НалоговыйНомер").Element("КПП") != null ? node.Element("НалоговыйНомер").Element("КПП").Value.ToString() : "";
                byte type_ = inn.Length == 12 ? (byte)1 : (byte)0;

                string regnum = node.Element("РегистрационныйНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                //           regnum = "007001069308";

                string fullName = node.Element("НаименованиеОрганизации") != null ? node.Element("НаименованиеОрганизации").Value : "";
                string shortName = node.Element("НаименованиеКраткое") != null ? node.Element("НаименованиеКраткое").Value : "";

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (type_ == 0)
                        {

                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                            if (insurer.Name != fullName)
                            {
                                insurer.Name = fullName;
                                change = true;
                            }
                        }
                        else
                        {
                            fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;

                            var fio = fullName.Split(' ');
                            if (fio.Count() > 0)
                            {
                                if (insurer.LastName != fio[0])
                                {
                                    insurer.LastName = fio[0];
                                    change = true;
                                }
                                if (fio.Count() > 1)
                                    if (insurer.FirstName != fio[1])
                                    {
                                        insurer.FirstName = fio[1];
                                        change = true;
                                    }
                                if (fio.Count() > 2)
                                {
                                    insurer.MiddleName = "";

                                    for (int i = 2; i < fio.Count(); i++)
                                    {
                                        insurer.MiddleName = insurer.MiddleName + fio[i];
                                    }
                                }
                            }
                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                        }

                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }

                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;
                    insurer.TypePayer = type_;
                    if (type_ == 0)
                    {
                        insurer.NameShort = shortName;
                        insurer.Name = fullName;
                    }
                    else
                    {
                        var fio = fullName.Split(' ');
                        if (fio.Count() > 0)
                        {
                            insurer.LastName = fio[0];
                            if (fio.Count() > 1)
                                insurer.FirstName = fio[1];
                            if (fio.Count() > 2)
                            {
                                for (int i = 2; i < fio.Count(); i++)
                                {
                                    insurer.MiddleName = insurer.MiddleName + fio[i];
                                }
                            }
                        }
                        insurer.NameShort = shortName;
                    }
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }
                #endregion


                #region перебор инд.сведений

                var razd_6_1_list = doc.Root.Descendants().Where(x => x.Name.LocalName == "СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ");

                int count = razd_6_1_list.Count();
                //             secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = count.ToString(); }));
                int k = 0;

                //        int l = 0;
                List<staffContainer> staffList = new List<staffContainer>();
                foreach (var razd_6_1 in razd_6_1_list)
                {
                    //          l++;

                    var snils = Utils.ParseSNILS_XML(razd_6_1.Element("СтраховойНомер") != null ? razd_6_1.Element("СтраховойНомер").Value.ToString() : "", true);

                    string middleName = razd_6_1.Element("ФИО").Element("Отчество") != null ? razd_6_1.Element("ФИО").Element("Отчество").Value.ToString() : "";

                    var st = new staffContainer
                    {
                        insuranceNum = snils.num,//lll.ToString() + l.ToString().PadLeft(3, '0')
                        insID = insurer.ID,
                        lastName = razd_6_1.Element("ФИО").Element("Фамилия").Value.ToString(),
                        firstName = razd_6_1.Element("ФИО").Element("Имя").Value.ToString(),
                        middleName = middleName,
                        contrNum = snils.contrNum,
                        dismissed = (byte)0
                    };
                    if (razd_6_1.Element("СведенияОбУвольнении") != null)
                    {
                        st.dismissed = razd_6_1.Element("СведенияОбУвольнении").Value == "УВОЛЕН" ? (byte)1 : (byte)0;
                    }

                    if (!staffList.Any(x => x.insuranceNum == st.insuranceNum))
                        staffList.Add(st);
                }

                var listid = staffList.Select(y => y.insuranceNum).ToList();
                var listNum = db.Staff.Where(x => x.InsurerID == insurer.ID && listid.Contains(x.InsuranceNumber)).Select(x => x.InsuranceNumber).ToList();

                staffList = staffList.Where(x => !listNum.Contains(x.insuranceNum)).ToList();  // те сотрудники которых нет в базе для текущего страхователя

                foreach (var item in staffList)
                {
                    Staff staff_ = new Staff
                    {
                        InsuranceNumber = item.insuranceNum.PadLeft(9, '0'),
                        InsurerID = item.insID,
                        LastName = item.lastName,
                        FirstName = item.firstName,
                        MiddleName = item.middleName,
                        ControlNumber = item.contrNum,
                        Dismissed = item.dismissed
                    };

                    db.AddToStaff(staff_);
                }
                if (staffList.Count > 0)
                    db.SaveChanges();


                //    l = 0;
                foreach (var razd_6_1 in razd_6_1_list)
                {
                    try
                    {
                        //    l++;
                        string InsuranceNum = Utils.ParseSNILS_XML(razd_6_1.Element("СтраховойНомер").Value.ToString(), false).num;

                        //var l__ = (lll.ToString() + l.ToString().PadLeft(3, '0')).PadLeft(9,'0');
                        Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);//

                        if (updateStaffData.Checked) // если выбрано обновлять данные в базе при расхождении
                        {
                            bool change = false;
                            if (staff.LastName != razd_6_1.Element("ФИО").Element("Фамилия").Value.ToString())
                            {
                                staff.LastName = razd_6_1.Element("ФИО").Element("Фамилия").Value.ToString();
                                change = true;
                            }
                            if (staff.FirstName != razd_6_1.Element("ФИО").Element("Имя").Value.ToString())
                            {
                                staff.FirstName = razd_6_1.Element("ФИО").Element("Имя").Value.ToString();
                                change = true;
                            }
                            if (razd_6_1.Element("ФИО").Element("Отчество") != null)
                                if (staff.MiddleName != razd_6_1.Element("ФИО").Element("Отчество").Value.ToString())
                                {
                                    staff.MiddleName = razd_6_1.Element("ФИО").Element("Отчество").Value.ToString();
                                    change = true;
                                }

                            if (razd_6_1.Element("СведенияОбУвольнении") != null)
                            {
                                byte dis = razd_6_1.Element("СведенияОбУвольнении").Value == "УВОЛЕН" ? (byte)1 : (byte)0;
                                if (staff.Dismissed != dis)
                                {
                                    staff.Dismissed = dis;
                                    change = true;
                                }
                            }

                            if (change)
                            {
                                db.ObjectStateManager.ChangeObjectState(staff, EntityState.Modified);
                                db.SaveChanges();
                            }
                        }


                        if (String.IsNullOrEmpty(InsuranceNum))
                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = razd_6_1.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке Индивидуальных сведений. Не указан Страховой номер (СНИЛС) сотрудника.\r\n" });


                        string tInfo = razd_6_1.Element("ТипСведений").Value.ToString().ToLower();
                        long tInfoID = typeInfo_.First(x => x.Name.ToLower() == tInfo).ID;

                        byte q = byte.Parse(razd_6_1.Element("ОтчетныйПериод").Element("Квартал").Value.ToString());
                        short y = short.Parse(razd_6_1.Element("ОтчетныйПериод").Element("Год").Value.ToString());


                        FormsRSW2014_1_Razd_6_1 rsw_6_1 = new FormsRSW2014_1_Razd_6_1();
                        bool exist = false;
                        byte qk = (byte)0;
                        short yk = (short)0;
                        string regNumK = "";

                        if (tInfo == "исходная")
                        {
                            if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID))
                            {
                                rsw_6_1 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID);
                                if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "0")
                                {
                                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_1 WHERE ([ID] = {0})", rsw_6_1.ID));
                                    rsw_6_1 = new FormsRSW2014_1_Razd_6_1();
                                }
                                else
                                    exist = true;
                            }
                        }
                        else
                        {
                            qk = byte.Parse(razd_6_1.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value.ToString());
                            yk = short.Parse(razd_6_1.Element("КорректируемыйОтчетныйПериод").Element("Год").Value.ToString());
                            if (razd_6_1.Element("РегистрационныйНомерКорректируемогоПериода") != null)
                            {
                                regNumK = razd_6_1.Element("РегистрационныйНомерКорректируемогоПериода").Value.ToString();
                                while (regNumK.Contains("-"))
                                    regNumK = regNumK.Remove(regNumK.IndexOf('-'), 1);
                            }

                            if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.YearKorr == yk && x.QuarterKorr == qk))
                            {
                                rsw_6_1 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.YearKorr == yk && x.QuarterKorr == qk);
                                if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "0")
                                {
                                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_1 WHERE ([ID] = {0})", rsw_6_1.ID));
                                    rsw_6_1 = new FormsRSW2014_1_Razd_6_1();
                                }
                                else
                                    exist = true;
                            }
                        }

                        if (exist) // если такая запись уже существует
                        {
                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2" && updateSumFee.Checked) // объединение
                            {
                                rsw_6_1.SumFeePFR = rsw_6_1.SumFeePFR + decimal.Parse(razd_6_1.Element("СуммаВзносовНаОПС").Value.ToString(), CultureInfo.InvariantCulture);
                                db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                                //db.SaveChanges();
                            }
                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "1") // не загружать импортируемую форму
                            {
                                continue;
                            }
                        }
                        else // если такой записи нет то добавляем ее
                        {
                            rsw_6_1.InsurerID = insurer.ID;
                            rsw_6_1.StaffID = staff.ID;
                            rsw_6_1.Year = y;
                            rsw_6_1.Quarter = q;

                            if (yk != 0)
                            {
                                rsw_6_1.YearKorr = yk;
                                rsw_6_1.QuarterKorr = qk;
                                rsw_6_1.RegNumKorr = regNumK;
                            }
                            rsw_6_1.TypeInfoID = tInfoID;

                            if (razd_6_1.Element("СуммаВзносовНаОПС") != null)
                            {
                                rsw_6_1.SumFeePFR = decimal.Parse(razd_6_1.Element("СуммаВзносовНаОПС").Value.ToString(), CultureInfo.InvariantCulture);
                            }
                            else
                                rsw_6_1.SumFeePFR = 0;
                            rsw_6_1.DateFilling = DateTime.Parse(razd_6_1.Element("ДатаЗаполнения").Value.ToString());

                            db.AddToFormsRSW2014_1_Razd_6_1(rsw_6_1);
                            //             db.SaveChanges();
                        }



                        #region Раздел 6.4

                        if (updateIndSved_DDL.SelectedItem.Tag.ToString() != "2" || (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2" && updateRazd_6_4.Checked))
                        {

                            if (razd_6_1.Descendants().Any(x => x.Name.LocalName == "СведенияОсуммеВыплатИвознагражденийВпользуЗЛ"))
                            {
                                var razd_6_4_list = razd_6_1.Descendants().Where(x => x.Name.LocalName == "СведенияОсуммеВыплатИвознагражденийВпользуЗЛ");
                                List<string> platCatList = razd_6_4_list.Descendants().Where(x => x.Name.LocalName == "КодКатегории").Select(x => x.Value).Distinct().ToList();



                                foreach (var item in platCatList)
                                {

                                    if (!PlatCatList.Any(x => x.Code == item))
                                    {
                                        if (String.IsNullOrEmpty(item))
                                        {
                                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = razd_6_1.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Не указан Код Категории Плательщика.\r\n" });
                                        }
                                        else
                                        {
                                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = razd_6_1.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Указан Код Категории Плательщика - \"" + item + "\", который отсутствует в списке доступных категорий в справочнике.\r\n" });
                                        }
                                        break;
                                    }

                                    PlatCategory platCat = PlatCatList.FirstOrDefault(x => x.Code == item);


                                    FormsRSW2014_1_Razd_6_4 rsw_6_4 = new FormsRSW2014_1_Razd_6_4 { PlatCategoryID = platCat.ID };
                                    //      rsw_6_4.FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID;

                                    bool exist_6_4 = false;
                                    foreach (var razd_6_4 in razd_6_4_list.Where(x => x.Element("КодКатегории").Value == item))
                                    {
                                        string name = "";
                                        string strCode = razd_6_4.Element("КодСтроки").Value.ToString();
                                        name = strCode.Substring(strCode.Length - 1, 1);

                                        decimal data = 0;
                                        for (int i = 0; i <= 3; i++)
                                        {

                                            string itemName = "s_" + name + "_";
                                            string nameStr = "";
                                            switch (i)
                                            {
                                                case 0:
                                                    nameStr = "СуммаВыплатИныхВознаграждений";
                                                    break;
                                                case 1:
                                                    nameStr = "НеПревышающиеВсего";
                                                    break;
                                                case 2:
                                                    nameStr = "НеПревышающиеПоДоговорам";
                                                    break;
                                                case 3:
                                                    nameStr = "ПревышающиеПредельную";
                                                    break;
                                            }

                                            if (nameStr != "" && razd_6_4.Element(nameStr) != null)
                                                data = decimal.Parse(razd_6_4.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                            if (nameStr != "" && razd_6_4.Element(nameStr) == null)
                                                data = 0;

                                            itemName = itemName + i.ToString();

                                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2" && updateRazd_6_4.Checked)
                                            {
                                                if (db.FormsRSW2014_1_Razd_6_4.Any(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID && x.PlatCategoryID == platCat.ID))
                                                {
                                                    rsw_6_4 = db.FormsRSW2014_1_Razd_6_4.First(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID && x.PlatCategoryID == platCat.ID);
                                                    exist_6_4 = true;
                                                    var properties = rsw_6_4.GetType().GetProperty(itemName);
                                                    if (properties != null)
                                                    {
                                                        object value = properties.GetValue(rsw_6_4, null);
                                                        if (value != null)
                                                            data = data + (decimal)value;

                                                        //                                                            data = data + decimal.Parse(value.ToString(), CultureInfo.InvariantCulture);

                                                    }
                                                }
                                            }


                                            rsw_6_4.GetType().GetProperty(itemName).SetValue(rsw_6_4, data, null);


                                        }

                                    }

                                    if (!exist_6_4)
                                    {
                                        rsw_6_1.FormsRSW2014_1_Razd_6_4.Add(rsw_6_4);
                                        //db.AddToFormsRSW2014_1_Razd_6_4(rsw_6_4);

                                        //
                                    }
                                    else
                                        db.ObjectStateManager.ChangeObjectState(rsw_6_4, EntityState.Modified);
                                }
                                //          db.SaveChanges();

                            }
                        }

                        #endregion

                        #region Раздел 6.6
                        if (razd_6_1.Descendants().Any(x => x.Name.LocalName == "СведенияОкорректировках"))
                        {
                            var razd_6_6_list = razd_6_1.Descendants().Where(x => x.Name.LocalName == "СведенияОкорректировках");


                            foreach (var razd_6_6 in razd_6_6_list)
                            {
                                if (razd_6_6.Element("ТипСтроки").Value == "МЕСЦ")
                                {
                                    byte accPer_q = byte.Parse(razd_6_6.Element("Квартал").Value);
                                    short accPer_y = short.Parse(razd_6_6.Element("Год").Value);

                                    FormsRSW2014_1_Razd_6_6 rsw_6_6 = new FormsRSW2014_1_Razd_6_6 { AccountPeriodQuarter = accPer_q, AccountPeriodYear = accPer_y };
                                    //FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID,
                                    if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2")
                                    {
                                        if (db.FormsRSW2014_1_Razd_6_6.Any(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID && x.AccountPeriodQuarter == accPer_q && x.AccountPeriodYear == accPer_y))
                                        {
                                            var rsw_6_6_del = db.FormsRSW2014_1_Razd_6_6.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID && x.AccountPeriodQuarter == accPer_q && x.AccountPeriodYear == accPer_y);
                                            foreach (var t in rsw_6_6_del)
                                            {
                                                db.FormsRSW2014_1_Razd_6_6.DeleteObject(t);
                                            }
                                            if (rsw_6_6_del.Count() > 0)
                                                db.SaveChanges();
                                        }
                                    }


                                    decimal data = 0;
                                    for (int i = 0; i <= 2; i++)
                                    {

                                        string itemName = "";
                                        string nameStr = "";
                                        switch (i)
                                        {
                                            case 0:
                                                nameStr = "СуммаДоначисленныхВзносовОПС";
                                                itemName = "SumFeePFR_D";
                                                break;
                                            case 1:
                                                nameStr = "СуммаДоначисленныхВзносовНаСтраховую";
                                                itemName = "SumFeePFR_StrahD";
                                                break;
                                            case 2:
                                                nameStr = "СуммаДоначисленныхВзносовНаНакопительную";
                                                itemName = "SumFeePFR_NakopD";
                                                break;
                                        }

                                        if (nameStr != "" && razd_6_6.Element(nameStr) != null)
                                            data = decimal.Parse(razd_6_6.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                        if (nameStr != "" && razd_6_6.Element(nameStr) == null)
                                            data = 0;

                                        rsw_6_6.GetType().GetProperty(itemName).SetValue(rsw_6_6, data, null);


                                    }

                                    rsw_6_1.FormsRSW2014_1_Razd_6_6.Add(rsw_6_6);

                                    //db.AddToFormsRSW2014_1_Razd_6_6(rsw_6_6);
                                }

                            }


                            //    db.SaveChanges();

                        }

                        #endregion

                        #region Раздел 6.7
                        if (updateIndSved_DDL.SelectedItem.Tag.ToString() != "2" || (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2" && updateRazd_6_7.Checked))
                        {
                            if (razd_6_1.Descendants().Any(x => x.Name.LocalName == "СведенияОсуммеВыплатИвознагражденийПоДопТарифу"))
                            {
                                var razd_6_7_list = razd_6_1.Descendants().Where(x => x.Name.LocalName == "СведенияОсуммеВыплатИвознагражденийПоДопТарифу");
                                List<string> specOcenkaList = razd_6_7_list.Descendants().Where(x => x.Name.LocalName == "КодСпециальнойОценкиУсловийТруда").Select(x => x.Value).Distinct().ToList();

                                foreach (var item in specOcenkaList)
                                {
                                    SpecOcenkaUslTruda specOcenka = SpecOcenkaUslTruda_list.FirstOrDefault(x => x.Code == item);

                                    FormsRSW2014_1_Razd_6_7 rsw_6_7 = new FormsRSW2014_1_Razd_6_7 { };
                                    //FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID
                                    if (specOcenka != null)
                                    {
                                        rsw_6_7.SpecOcenkaUslTrudaID = specOcenka.ID;
                                    }

                                    bool exist_6_7 = false;
                                    foreach (var razd_6_7 in razd_6_7_list.Where(x => x.Element("КодСпециальнойОценкиУсловийТруда").Value == item))
                                    {
                                        string name = "";
                                        string strCode = razd_6_7.Element("КодСтроки").Value.ToString();
                                        name = strCode.Substring(strCode.Length - 1, 1);

                                        decimal data = 0;
                                        for (int i = 0; i <= 1; i++)
                                        {

                                            string itemName = "s_" + name + "_";
                                            string nameStr = "";
                                            switch (i)
                                            {
                                                case 0:
                                                    nameStr = "СуммаВыплатПоДопТарифу27-1";
                                                    break;
                                                case 1:
                                                    nameStr = "СуммаВыплатПоДопТарифу27-2-18";
                                                    break;
                                            }

                                            if (nameStr != "" && razd_6_7.Element(nameStr) != null)
                                                data = decimal.Parse(razd_6_7.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                            if (nameStr != "" && razd_6_7.Element(nameStr) == null)
                                                data = 0;

                                            itemName = itemName + i.ToString();

                                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2" && updateRazd_6_7.Checked)
                                            {
                                                if ((specOcenka != null && db.FormsRSW2014_1_Razd_6_7.Any(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID && x.SpecOcenkaUslTrudaID == specOcenka.ID)) || (specOcenka == null && db.FormsRSW2014_1_Razd_6_7.Any(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID && x.SpecOcenkaUslTrudaID == null)))
                                                {
                                                    if (specOcenka != null)
                                                        rsw_6_7 = db.FormsRSW2014_1_Razd_6_7.First(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID && x.SpecOcenkaUslTrudaID == specOcenka.ID);
                                                    else
                                                        rsw_6_7 = db.FormsRSW2014_1_Razd_6_7.First(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID && x.SpecOcenkaUslTrudaID == null);
                                                    exist_6_7 = true;
                                                    var properties = rsw_6_7.GetType().GetProperty(itemName);
                                                    if (properties != null)
                                                    {
                                                        object value = properties.GetValue(rsw_6_7, null);
                                                        if (value != null)
                                                            data = data + (decimal)value;//decimal.Parse(value.ToString(), CultureInfo.InvariantCulture); ;
                                                    }
                                                }
                                            }

                                            rsw_6_7.GetType().GetProperty(itemName).SetValue(rsw_6_7, data, null);


                                        }

                                    }

                                    if (!exist_6_7)
                                    {
                                        rsw_6_1.FormsRSW2014_1_Razd_6_7.Add(rsw_6_7);
                                        //   db.AddToFormsRSW2014_1_Razd_6_7(rsw_6_7);
                                    }
                                    else
                                        db.ObjectStateManager.ChangeObjectState(rsw_6_7, EntityState.Modified);
                                }
                                //                            db.SaveChanges();

                            }
                        }
                        //       db.SaveChanges();

                        #endregion

                        #region Записи о стаже
                        if (razd_6_1.Descendants().Any(x => x.Name.LocalName == "СтажевыйПериод"))
                        {
                            var staj_osn_list = razd_6_1.Descendants().Where(x => x.Name.LocalName == "СтажевыйПериод");

                            //// если замена
                            //if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2" && !updateStaj.Checked)
                            //{
                            //    long[] ids = db.StajOsn.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID).Select(x => x.ID).ToArray();
                            //    if (ids.Count() > 0)
                            //    {
                            //        string list = String.Join(",", ids);
                            //        db.ExecuteStoreCommand(String.Format("DELETE FROM StajOsn WHERE ([ID] IN ({0}))", list));
                            //    }
                            //}

                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() != "2" || (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2" && updateStaj.Checked))
                            {
                                int n = db.StajOsn.Count(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw_6_1.ID);

                                foreach (var staj_osn in staj_osn_list)
                                {
                                    DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("ДатаНачалаПериода").Value.ToString());
                                    DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("ДатаКонцаПериода").Value.ToString());

                                    n++;
                                    StajOsn stajOsn = new StajOsn { DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };
                                    //  FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID,
                                    //                  db.StajOsn.AddObject(stajOsn);
                                    //       db.SaveChanges();

                                    var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");
                                    //перебираем льготный стаж если есть
                                    int i = 0;
                                    foreach (var item in staj_lgot_list)
                                    {
                                        string str = "";
                                        i++;
                                        var staj_lgot = item.Element("ОсобенностиУчета");
                                        StajLgot stajLgot = new StajLgot { Number = i };
                                        //StajOsnID = stajOsn.ID,
                                        var terrUsl = staj_lgot.Element("ТерриториальныеУсловия");
                                        if (terrUsl != null) // если есть терр условия
                                        {
                                            //если есть запись в с таким кодом терр условий в базе
                                            str = terrUsl.Element("ОснованиеТУ").Value.ToString().ToUpper();
                                            if (TerrUsl_list.Any(x => x.Code.ToUpper() == str))
                                            {
                                                stajLgot.TerrUslID = TerrUsl_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                if (terrUsl.Element("Коэффициент") != null)
                                                    stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                                else
                                                    stajLgot.TerrUslKoef = 0;
                                            }
                                        }

                                        var osobUsl = staj_lgot.Element("ОсобыеУсловияТруда");
                                        if (osobUsl != null)
                                        {
                                            if (osobUsl.Element("ОснованиеОУТ") != null)
                                            {
                                                str = osobUsl.Element("ОснованиеОУТ").Value.ToString().ToUpper();
                                                if (OsobUslTruda_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.OsobUslTrudaID = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (osobUsl.Element("ПозицияСписка") != null)
                                            {
                                                str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                                if (KodVred_2_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == str);
                                                    stajLgot.KodVred_OsnID = kv.ID;

                                                    // проверка на наличие такой должности в базе
                                                    if (db.Dolgn.Any(x => x.Name == kv.Name))
                                                    {
                                                        stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                                    }
                                                    else
                                                    {
                                                        Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                        db.AddToDolgn(dolgn);
                                                        db.SaveChanges();
                                                        stajLgot.DolgnID = dolgn.ID;
                                                    }
                                                }
                                            }
                                        }

                                        var ischislStrahStaj = staj_lgot.Element("ИсчисляемыйСтаж");
                                        if (ischislStrahStaj != null)
                                        {
                                            if (ischislStrahStaj.Element("ОснованиеИС") != null)
                                            {
                                                str = ischislStrahStaj.Element("ОснованиеИС").Value.ToString().ToUpper();
                                                if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                            {
                                                if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                                {
                                                    stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                                }
                                                if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                                {
                                                    stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                                }
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                            {
                                                if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                                {
                                                    stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                                }
                                                if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                                {
                                                    stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                                }
                                            }
                                        }

                                        var ischislStrahStajDop = staj_lgot.Element("ДекретДети");
                                        if (ischislStrahStajDop != null)
                                        {
                                            str = ischislStrahStajDop.Value.ToString().ToUpper();
                                            if (IschislStrahStajDop_list.Any(x => x.Code.ToUpper() == str))
                                            {
                                                stajLgot.IschislStrahStajDopID = IschislStrahStajDop_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                            }
                                        }


                                        var uslDosrNazn = staj_lgot.Element("ВыслугаЛет");
                                        if (uslDosrNazn != null) // если есть терр условия
                                        {
                                            //если есть запись в с таким кодом терр условий в базе
                                            if (uslDosrNazn.Element("ОснованиеВЛ") != null)
                                            {
                                                str = uslDosrNazn.Element("ОснованиеВЛ").Value.ToString().ToUpper();
                                                if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                            {
                                                if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                                {
                                                    stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                                }
                                                if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                                {
                                                    stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                                }
                                            }
                                            if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                            {
                                                if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                                {
                                                    stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                                }
                                                if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                                {
                                                    stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                                }
                                            }
                                            if (uslDosrNazn.Element("ДоляСтавки") != null)
                                            {
                                                stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                            }
                                        }

                                        stajOsn.StajLgot.Add(stajLgot);
                                    }

                                    rsw_6_1.StajOsn.Add(stajOsn);

                                }
                            }


                        }


                        #endregion

                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        if (transactionCheckBox.Checked)
                        {
                            if (cnt_records_imported == 50 || cnt_records_imported == 100 || cnt_records_imported == 150)
                                db.SaveChanges();
                        }
                        else
                            db.SaveChanges();

                        cnt_records_imported++;
                        importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));


                    }
                    catch (Exception ex)
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = razd_6_1.Element("НомерВпачке").Value.ToString(), type = ex.Message + "\r\n" });
                    }

                    k++;

                    decimal temp = (decimal)k / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, k.ToString());


                } // перебор инд. сведений
                #endregion




                db.SaveChanges();
                doc = null;
                db.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));
                doc = null;
                db.Dispose();
                result = false;
            }

            doc = null;
            db.Dispose();
            return result;
        }

        /// <summary>
        /// Импорт файлов СПВ-2 2014
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_SPV_2_2014(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);

            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            XElement node = doc.Root;
            bool result = true;

            try
            {
                #region // Поиск информации о составителе
                node = node.Descendants().First(x => x.Name.LocalName == "СоставительПачки");
                string inn = node.Element("НалоговыйНомер").Element("ИНН").Value;
                string kpp = node.Element("НалоговыйНомер").Element("КПП") != null ? node.Element("НалоговыйНомер").Element("КПП").Value.ToString() : "";
                byte type_ = inn.Length == 12 ? (byte)1 : (byte)0;
                string egrip = node.Element("КодЕГРИП") != null ? node.Element("КодЕГРИП").Value.ToString() : "";
                string egrul = node.Element("КодЕГРЮЛ") != null ? node.Element("КодЕГРЮЛ").Value.ToString() : "";


                string regnum = node.Element("РегистрационныйНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                string fullName = node.Element("НаименованиеОрганизации") != null ? node.Element("НаименованиеОрганизации").Value.ToString() : "";
                string shortName = node.Element("НаименованиеКраткое") != null ? node.Element("НаименованиеКраткое").Value.ToString() : "";

                if (fullName == "")
                    fullName = shortName;

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (type_ == 0)
                        {
                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                            if (insurer.Name != fullName)
                            {
                                insurer.Name = fullName;
                                change = true;
                            }
                        }
                        else
                        {
                            fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;

                            var fio = fullName.Split(' ');
                            if (fio.Count() > 0)
                            {
                                if (insurer.LastName != fio[0])
                                {
                                    insurer.LastName = fio[0];
                                    change = true;
                                }
                                if (fio.Count() > 1)
                                    if (insurer.FirstName != fio[1])
                                    {
                                        insurer.FirstName = fio[1];
                                        change = true;
                                    }
                                if (fio.Count() > 2)
                                {
                                    insurer.MiddleName = "";

                                    for (int i = 2; i < fio.Count(); i++)
                                    {
                                        insurer.MiddleName = insurer.MiddleName + fio[i];
                                    }
                                }
                            }
                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                        }

                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }
                        if (insurer.EGRIP != egrip)
                        {
                            insurer.EGRIP = egrip;
                            change = true;
                        }
                        if (insurer.EGRUL != egrul)
                        {
                            insurer.EGRUL = egrul;
                            change = true;
                        }

                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;

                    insurer.TypePayer = type_;
                    if (type_ == 0)
                    {
                        insurer.NameShort = shortName;
                        insurer.Name = fullName;
                    }
                    else
                    {
                        var fio = fullName.Split(' ');
                        if (fio.Count() > 0)
                        {
                            insurer.LastName = fio[0];
                            if (fio.Count() > 1)
                                insurer.FirstName = fio[1];
                            if (fio.Count() > 2)
                            {
                                for (int i = 2; i < fio.Count(); i++)
                                {
                                    insurer.MiddleName = insurer.MiddleName + fio[i];
                                }
                            }
                        }
                        insurer.NameShort = shortName;
                    }
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    insurer.EGRIP = egrip;
                    insurer.EGRUL = egrul;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }
                #endregion


                #region перебор СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ

                var spv_2_list = doc.Root.Descendants().Where(x => x.Name.LocalName == "СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ");

                int count = spv_2_list.Count();
                int k = 0;
                List<staffContainer> staffList = new List<staffContainer>();
                foreach (var spv_2 in spv_2_list)
                {
                    var snils = Utils.ParseSNILS_XML(spv_2.Element("СтраховойНомер") != null ? spv_2.Element("СтраховойНомер").Value.ToString() : "", true);

                    string middleName = spv_2.Element("ФИО").Element("Отчество") != null ? spv_2.Element("ФИО").Element("Отчество").Value.ToString() : "";

                    var st = new staffContainer
                    {
                        insuranceNum = snils.num,
                        insID = insurer.ID,
                        lastName = spv_2.Element("ФИО").Element("Фамилия").Value.ToString(),
                        firstName = spv_2.Element("ФИО").Element("Имя").Value.ToString(),
                        middleName = middleName,
                        contrNum = snils.contrNum
                    };


                    if (!staffList.Any(x => x.insuranceNum == st.insuranceNum))
                        staffList.Add(st);

                }

                var listid = staffList.Select(y => y.insuranceNum).ToList();
                var listNum = db.Staff.Where(x => x.InsurerID == insurer.ID && listid.Contains(x.InsuranceNumber)).Select(x => x.InsuranceNumber).ToList();

                staffList = staffList.Where(x => !listNum.Contains(x.insuranceNum)).ToList();  // те сотрудники которых нет в базе для текущего страхователя

                foreach (var item in staffList)
                {
                    Staff staff_ = new Staff
                    {
                        InsuranceNumber = item.insuranceNum.PadLeft(9, '0'),
                        InsurerID = item.insID,
                        LastName = item.lastName,
                        FirstName = item.firstName,
                        MiddleName = item.middleName,
                        ControlNumber = item.contrNum,
                        Dismissed = 0
                    };

                    db.AddToStaff(staff_);
                }
                if (staffList.Count > 0)
                    db.SaveChanges();


                foreach (var spv_2 in spv_2_list)
                {
                    try
                    {
                        string InsuranceNum = Utils.ParseSNILS_XML(spv_2.Element("СтраховойНомер") != null ? spv_2.Element("СтраховойНомер").Value.ToString() : "", false).num;

                        Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);

                        if (updateStaffData.Checked) // если выбрано обнослять данные в базе при расхождении
                        {
                            bool change = false;
                            if (staff.LastName != spv_2.Element("ФИО").Element("Фамилия").Value.ToString())
                            {
                                staff.LastName = spv_2.Element("ФИО").Element("Фамилия").Value.ToString();
                                change = true;
                            }
                            if (staff.FirstName != spv_2.Element("ФИО").Element("Имя").Value.ToString())
                            {
                                staff.FirstName = spv_2.Element("ФИО").Element("Имя").Value.ToString();
                                change = true;
                            }
                            if (spv_2.Element("ФИО").Element("Отчество") != null)
                                if (staff.MiddleName != spv_2.Element("ФИО").Element("Отчество").Value.ToString())
                                {
                                    staff.MiddleName = spv_2.Element("ФИО").Element("Отчество").Value.ToString();
                                    change = true;
                                }

                            if (change)
                            {
                                db.ObjectStateManager.ChangeObjectState(staff, EntityState.Modified);
                                db.SaveChanges();
                            }
                        }

                        if (String.IsNullOrEmpty(InsuranceNum))
                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = spv_2.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке Индивидуальных сведений. Не указан Страховой номер (СНИЛС) сотрудника.\r\n" });

                        string tInfo = spv_2.Element("ТипСведений").Value.ToString().ToLower();
                        long tInfoID = typeInfo_.First(x => x.Name.ToLower() == tInfo).ID;

                        byte q = byte.Parse(spv_2.Element("ОтчетныйПериод").Element("Квартал").Value.ToString());
                        short y = short.Parse(spv_2.Element("ОтчетныйПериод").Element("Год").Value.ToString());


                        FormsSPW2 spw2 = new FormsSPW2();
                        bool exist = false;
                        byte qk = (byte)0;
                        short yk = (short)0;

                        if (tInfo == "исходная")
                        {
                            if (db.FormsSPW2.Any(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID))
                            {
                                spw2 = db.FormsSPW2.FirstOrDefault(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID);
                                if (updateSPW2_DDL.SelectedItem.Tag.ToString() == "0")
                                {
                                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSPW2 WHERE ([ID] = {0})", spw2.ID));
                                    spw2 = new FormsSPW2();
                                }
                                else
                                    exist = true;
                            }
                        }
                        else
                        {
                            qk = byte.Parse(spv_2.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value.ToString());
                            yk = short.Parse(spv_2.Element("КорректируемыйОтчетныйПериод").Element("Год").Value.ToString());

                            if (db.FormsSPW2.Any(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.YearKorr == yk && x.QuarterKorr == qk))
                            {
                                spw2 = db.FormsSPW2.FirstOrDefault(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.YearKorr == yk && x.QuarterKorr == qk);
                                if (updateSPW2_DDL.SelectedItem.Tag.ToString() == "0")
                                {
                                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSPW2 WHERE ([ID] = {0})", spw2.ID));
                                    spw2 = new FormsSPW2();
                                }
                                else
                                    exist = true;
                            }
                        }

                        if (exist) // если такая запись уже существует
                        {
                            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "1") // не загружать импортируемую форму
                            {
                                break;
                            }
                        }
                        else // если такой записи нет то добавляем ее
                        {
                            spw2.InsurerID = insurer.ID;
                            spw2.StaffID = staff.ID;
                            spw2.Year = y;
                            spw2.Quarter = q;

                            if (yk != 0)
                            {
                                spw2.YearKorr = yk;
                                spw2.QuarterKorr = qk;
                            }
                            spw2.TypeInfoID = tInfoID;

                            if (spv_2.Element("ПризнакНачисленияВзносовОПС") != null)
                            {
                                spw2.ExistsInsurOPS = spv_2.Element("ПризнакНачисленияВзносовОПС").Value.ToString() == "ДА" ? (byte)1 : (byte)0;
                            }
                            else
                                spw2.ExistsInsurOPS = 0;

                            if (spv_2.Element("ПризнакНачисленияВзносовПоДопТарифу") != null)
                            {
                                spw2.ExistsInsurDop = spv_2.Element("ПризнакНачисленияВзносовПоДопТарифу").Value.ToString() == "ДА" ? (byte)1 : (byte)0;
                            }
                            else
                                spw2.ExistsInsurDop = 0;

                            string catCode = spv_2.Element("КодКатегории").Value.ToString().ToUpper();
                            PlatCategory platCat = db.PlatCategory.FirstOrDefault(x => x.Code == catCode);

                            if (platCat != null)
                            {
                                spw2.PlatCategoryID = platCat.ID;
                            }
                            else
                                break;

                            spw2.DateFilling = DateTime.Parse(spv_2.Element("ДатаЗаполнения").Value.ToString());
                            spw2.DateComposit = DateTime.Parse(spv_2.Element("ДатаСоставленияНа").Value.ToString());

                            db.AddToFormsSPW2(spw2);
                            db.SaveChanges();


                            #region Записи о стаже
                            if (spv_2.Descendants().Any(x => x.Name.LocalName == "СтажевыйПериод"))
                            {
                                var staj_osn_list = spv_2.Descendants().Where(x => x.Name.LocalName == "СтажевыйПериод");

                                int n = db.StajOsn.Any(x => x.FormsSPW2_ID == spw2.ID) ? db.StajOsn.Where(x => x.FormsSPW2_ID == spw2.ID).Count() : 0;

                                foreach (var staj_osn in staj_osn_list)
                                {
                                    DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("ДатаНачалаПериода").Value.ToString());
                                    DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("ДатаКонцаПериода").Value.ToString());

                                    n++;
                                    StajOsn stajOsn = new StajOsn { FormsSPW2_ID = spw2.ID, DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };

                                    db.StajOsn.AddObject(stajOsn);
                                    db.SaveChanges();

                                    var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");
                                    //перебираем льготный стаж если есть
                                    int i = 0;
                                    foreach (var item in staj_lgot_list)
                                    {
                                        string str = "";
                                        i++;
                                        var staj_lgot = item.Element("ОсобенностиУчета");
                                        StajLgot stajLgot = new StajLgot { StajOsnID = stajOsn.ID, Number = i };

                                        var terrUsl = staj_lgot.Element("ТерриториальныеУсловия");
                                        if (terrUsl != null) // если есть терр условия
                                        {
                                            //если есть запись в с таким кодом терр условий в базе
                                            str = terrUsl.Element("ОснованиеТУ").Value.ToString().ToUpper();
                                            if (db.TerrUsl.Any(x => x.Code.ToUpper() == str))
                                            {
                                                stajLgot.TerrUslID = db.TerrUsl.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                if (terrUsl.Element("Коэффициент") != null)
                                                    stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                                else
                                                    stajLgot.TerrUslKoef = 0;
                                            }
                                        }

                                        var osobUsl = staj_lgot.Element("ОсобыеУсловияТруда");
                                        if (osobUsl != null)
                                        {
                                            if (osobUsl.Element("ОснованиеОУТ") != null)
                                            {
                                                str = osobUsl.Element("ОснованиеОУТ").Value.ToString().ToUpper();
                                                if (db.OsobUslTruda.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.OsobUslTrudaID = db.OsobUslTruda.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (osobUsl.Element("ПозицияСписка") != null)
                                            {
                                                str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                                if (db.KodVred_2.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    KodVred_2 kv = db.KodVred_2.FirstOrDefault(x => x.Code.ToUpper() == str);
                                                    stajLgot.KodVred_OsnID = kv.ID;

                                                    // проверка на наличие такой должности в базе
                                                    if (db.Dolgn.Any(x => x.Name == kv.Name))
                                                    {
                                                        stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                                    }
                                                    else
                                                    {
                                                        Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                        db.AddToDolgn(dolgn);
                                                        db.SaveChanges();
                                                        stajLgot.DolgnID = dolgn.ID;
                                                    }
                                                }
                                            }
                                        }

                                        var ischislStrahStaj = staj_lgot.Element("ИсчисляемыйСтаж");
                                        if (ischislStrahStaj != null)
                                        {
                                            if (ischislStrahStaj.Element("ОснованиеИС") != null)
                                            {
                                                str = ischislStrahStaj.Element("ОснованиеИС").Value.ToString().ToUpper();
                                                if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                            {
                                                if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                                {
                                                    stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                                }
                                                if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                                {
                                                    stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                                }
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                            {
                                                if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                                {
                                                    stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                                }
                                                if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                                {
                                                    stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                                }
                                            }
                                        }

                                        var ischislStrahStajDop = staj_lgot.Element("ДекретДети");
                                        if (ischislStrahStajDop != null)
                                        {
                                            str = ischislStrahStajDop.Value.ToString().ToUpper();
                                            if (db.IschislStrahStajDop.Any(x => x.Code.ToUpper() == str))
                                            {
                                                stajLgot.IschislStrahStajDopID = db.IschislStrahStajDop.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                            }
                                        }


                                        var uslDosrNazn = staj_lgot.Element("ВыслугаЛет");
                                        if (uslDosrNazn != null) // если есть терр условия
                                        {
                                            //если есть запись в с таким кодом терр условий в базе
                                            if (uslDosrNazn.Element("ОснованиеВЛ") != null)
                                            {
                                                str = uslDosrNazn.Element("ОснованиеВЛ").Value.ToString().ToUpper();
                                                if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                            {
                                                if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                                {
                                                    stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                                }
                                                if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                                {
                                                    stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                                }
                                            }
                                            if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                            {
                                                if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                                {
                                                    stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                                }
                                                if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                                {
                                                    stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                                }
                                            }
                                            if (uslDosrNazn.Element("ДоляСтавки") != null)
                                            {
                                                stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                            }
                                        }

                                        db.StajLgot.AddObject(stajLgot);
                                    }


                                }


                                db.SaveChanges();

                            }


                            #endregion
                        }

                        if (bw.CancellationPending)
                        {
                            return false;
                        }


                        db.SaveChanges();
                        cnt_records_imported++;
                        importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    catch (Exception ex)
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = spv_2.Element("НомерВпачке").Value.ToString(), type = ex.Message + "\r\n" });
                    }


                    k++;

                    decimal temp = (decimal)k / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, k.ToString());
                } // перебор СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ

                #endregion




                result = true;
            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));

                result = false;
            }


            return result;
        }



        /// <summary>
        /// Импорт Файлов ПФР Формы РСВ-2 2014
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_RSW2_2014(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {
                string lName = "РСВ2_2014";
                node = doc.Descendants().First(x => x.Name.LocalName == lName);
                string regnum = node.Element("РегНомерПФР").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);


                string name = "";
                string inn = "";
                string okwed = "";
                string tel = "";
                short year = 0;
                string InsuranceNum = "";
                byte? contrNum = null;

                if (node.Element("СтраховойНомер") != null)
                {
                    var snils = Utils.ParseSNILS_XML(node.Element("СтраховойНомер").Value.ToString(), true);
                    InsuranceNum = snils.num;
                    contrNum = snils.contrNum;
                }

                if (node.Element("ГодРождения") != null)
                    year = short.Parse(node.Element("ГодРождения").Value.ToString());
                if (node.Element("ИНН") != null)
                    inn = node.Element("ИНН").Value;
                okwed = node.Element("КодПоОКВЭД").Value;
                if (node.Element("Телефон") != null)
                    tel = node.Element("Телефон").Value;

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        name = node.Element("ФИО").Element("Фамилия") != null ? node.Element("ФИО").Element("Фамилия").Value : "";
                        if (name != "" && insurer.LastName != name)
                        {
                            insurer.LastName = name;
                            change = true;
                        }

                        name = node.Element("ФИО").Element("Имя") != null ? node.Element("ФИО").Element("Имя").Value : "";
                        if (name != "" && insurer.FirstName != name)
                        {
                            insurer.FirstName = name;
                            change = true;
                        }
                        name = node.Element("ФИО").Element("Отчество") != null ? node.Element("ФИО").Element("Отчество").Value : "";
                        if (name != "" && insurer.MiddleName != name)
                        {
                            insurer.MiddleName = name;
                            change = true;
                        }

                        insurer.TypePayer = (byte)1;

                        if (insurer.RegNum != regnum)
                        {
                            insurer.RegNum = regnum;
                            change = true;
                        }
                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.OKWED != okwed)
                        {
                            insurer.OKWED = okwed;
                            change = true;
                        }
                        if (insurer.YearBirth != year)
                        {
                            insurer.YearBirth = year;
                            change = true;
                        }
                        if (insurer.PhoneContact != tel)
                        {
                            insurer.PhoneContact = tel;
                            change = true;
                        }
                        if (insurer.InsuranceNumber != InsuranceNum)
                        {
                            insurer.InsuranceNumber = InsuranceNum;
                            insurer.ControlNumber = contrNum;
                            change = true;
                        }


                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = 1;
                    insurer.LastName = node.Element("ФИО").Element("Фамилия") != null ? node.Element("ФИО").Element("Фамилия").Value : "";
                    insurer.FirstName = node.Element("ФИО").Element("Имя") != null ? node.Element("ФИО").Element("Имя").Value : "";
                    insurer.MiddleName = node.Element("ФИО").Element("Отчество") != null ? node.Element("ФИО").Element("Отчество").Value : "";
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.OKWED = okwed;
                    insurer.InsuranceNumber = InsuranceNum;
                    insurer.ControlNumber = contrNum;
                    insurer.PhoneContact = tel;
                    insurer.YearBirth = year;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }

                short y = short.Parse(node.Element("КалендарныйГод").Value.ToString());
                byte corrNum = byte.Parse(node.Element("НомерКорр").Value.ToString());

                FormsRSW2014_2_1 rsw = new FormsRSW2014_2_1();
                if (db.FormsRSW2014_2_1.Any(x => x.InsurerID == insurer.ID && x.Year == y && x.CorrectionNum == corrNum))
                {
                    FormsRSW2014_2_1 rswForDel = db.FormsRSW2014_2_1.First(x => x.InsurerID == insurer.ID && x.Year == y && x.CorrectionNum == corrNum);

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_2_1 WHERE ([ID] = {0})", rswForDel.ID));
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\nПри удалении Формы РСВ-2 произошла ошибка. Ошибка: " + ex.Message, this.ThemeName); }));
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = "При удалении Формы РСВ-2 произошла ошибка. Ошибка: " + ex.Message + "\r\n" });

                        return false;
                    }
                }
                rsw.CorrectionNum = corrNum;
                rsw.InsurerID = insurer.ID;
                rsw.Year = y;
                rsw.AutoCalc = false;
                if (node.Element("ПрекращениеДеятельности") != null)
                    rsw.WorkStop = node.Element("ПрекращениеДеятельности").Value.ToString() == "Л" ? (byte)1 : (byte)0;
                else
                    rsw.WorkStop = (byte)0;

                rsw.CountEmployers = node.Element("ЧленовКФХ") != null ? int.Parse(node.Element("ЧленовКФХ").Value.ToString()) : 0;
                rsw.CountConfirmDoc = node.Element("ЛистовПриложения") != null ? byte.Parse(node.Element("ЛистовПриложения").Value.ToString()) : (byte)0;

                // Что это такое???
                //node.Element("КоличествоСтраниц").Value.ToString()
                //                List<string> strCodes = new List<string> { "100", "110", "111", "112", "113", "114", "120", "121", "130", "140", "141", "142", "143", "144", "150"};

                DateTime dt = DateTime.Now;
                DateTime.TryParse(node.Element("ДатаЗаполнения").Value.ToString(), out dt);
                rsw.DateUnderwrite = dt;

                XElement node_confirm = node.Element("ПодтверждениеСведений");

                rsw.ConfirmType = byte.Parse(node_confirm.Element("ТипПодтверждающего").Value.ToString());

                rsw.ConfirmLastName = node_confirm.Element("ФИОПодтверждающего").Element("Фамилия") != null ? node_confirm.Element("ФИОПодтверждающего").Element("Фамилия").Value : "";
                rsw.ConfirmFirstName = node_confirm.Element("ФИОПодтверждающего").Element("Имя") != null ? node_confirm.Element("ФИОПодтверждающего").Element("Имя").Value : "";
                rsw.ConfirmMiddleName = node_confirm.Element("ФИОПодтверждающего").Element("Отчество") != null ? node_confirm.Element("ФИОПодтверждающего").Element("Отчество").Value : "";
                if (rsw.ConfirmType == 2)
                {
                    if (node_confirm.Element("НаименованиеОрганизации") != null)
                    {
                        rsw.ConfirmOrgName = node_confirm.Element("НаименованиеОрганизации").Value;
                    }
                    if (node_confirm.Element("Доверенность") != null)
                    {
                        node_confirm = node_confirm.Element("Доверенность");

                        rsw.ConfirmDocType_ID = db.DocumentTypes.FirstOrDefault(x => x.Code == "ПРОЧЕЕ").ID;
                        rsw.ConfirmDocName = "ДОВЕРЕННОСТЬ";

                        rsw.ConfirmDocSerLat = node_confirm.Element("Серия") != null ? node_confirm.Element("Серия").Value : "";

                        int n = 0;
                        rsw.ConfirmDocNum = node_confirm.Element("Номер") != null ? (int.TryParse(node_confirm.Element("Номер").Value.ToString(), out n) ? n : 0) : 0;
                        if (node_confirm.Element("ДатаВыдачи") != null && !String.IsNullOrEmpty(node_confirm.Element("ДатаВыдачи").Value.ToString()))
                            rsw.ConfirmDocDate = DateTime.Parse(node_confirm.Element("ДатаВыдачи").Value.ToString());
                        rsw.ConfirmDocKemVyd = node_confirm.Element("КемВыдана").Value;
                        if (node_confirm.Element("ДействуетС") != null && !String.IsNullOrEmpty(node_confirm.Element("ДействуетС").Value.ToString()))
                            rsw.ConfirmDocDateBegin = DateTime.Parse(node_confirm.Element("ДействуетС").Value.ToString());
                        if (node_confirm.Element("ДействуетПо") != null && !String.IsNullOrEmpty(node_confirm.Element("ДействуетПо").Value.ToString()))
                            rsw.ConfirmDocDateEnd = DateTime.Parse(node_confirm.Element("ДействуетПо").Value.ToString());

                    }
                }

                #region Раздел1

                XElement Раздел1 = node.Element("Раздел1");
                var nodes = Раздел1.Descendants().Where(x => x.Name.LocalName == "КодСтроки");
                foreach (var n in nodes)
                {
                    string strCode = n.Value.ToString();

                    foreach (var item_ in n.Parent.Elements())
                    {
                        string itemName = "s_" + strCode + "_";
                        decimal data = 0;
                        if (item_.Name.LocalName != "КодСтроки")
                        {
                            int i = -1;
                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);

                            switch (item_.Name.LocalName)
                            {
                                case "СуммаОПС":
                                    i = 0;
                                    break;
                                case "СуммаСЧ":
                                    i = 1;
                                    break;
                                case "СуммаНЧ":
                                    i = 2;
                                    break;
                                case "СуммаОМС":
                                    i = 3;
                                    break;
                            }
                            if (i >= 0)
                            {
                                itemName = itemName + i.ToString();
                                rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                            }
                        }
                    }

                }
                db.AddToFormsRSW2014_2_1(rsw);
                db.SaveChanges();
                #endregion

                #region Раздел2

                if (node.Element("Раздел2") != null)
                {
                    XElement Раздел2 = node.Element("Раздел2");

                    var razd_2_list = Раздел2.Descendants().Where(x => x.Name.LocalName == "ЧленКФХ");
                    int i = 0;
                    foreach (var razd_2 in razd_2_list)
                    {
                        i++;
                        FormsRSW2014_2_2 rsw_2_2 = new FormsRSW2014_2_2
                        {
                            FormsRSW2014_2_1D = rsw.ID
                        };

                        int num = 0;
                        if (!int.TryParse(razd_2.Element("НомерПП").Value.ToString(), out num))
                        {
                            num = i;
                        }

                        rsw_2_2.NumRec = num;
                        rsw_2_2.LastName = razd_2.Element("ФИО").Element("Фамилия") != null ? razd_2.Element("ФИО").Element("Фамилия").Value : "";
                        rsw_2_2.FirstName = razd_2.Element("ФИО").Element("Имя") != null ? razd_2.Element("ФИО").Element("Имя").Value : "";
                        rsw_2_2.MiddleName = razd_2.Element("ФИО").Element("Отчество") != null ? razd_2.Element("ФИО").Element("Отчество").Value : "";

                        var snils = Utils.ParseSNILS_XML(razd_2.Element("СтраховойНомер").Value.ToString(), true);

                        rsw_2_2.InsuranceNumber = snils.num;
                        rsw_2_2.ControlNumber = snils.contrNum;
                        rsw_2_2.Year = short.Parse(razd_2.Element("ГодРождения").Value.ToString());
                        rsw_2_2.DateBegin = DateTime.Parse(razd_2.Element("НачалоПериода").Value.ToString());
                        rsw_2_2.DateEnd = DateTime.Parse(razd_2.Element("КонецПериода").Value.ToString());

                        rsw_2_2.SumOPS = razd_2.Element("СуммаОПС") != null ? decimal.Parse(razd_2.Element("СуммаОПС").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                        rsw_2_2.SumOMS = razd_2.Element("СуммаОМС") != null ? decimal.Parse(razd_2.Element("СуммаОМС").Value.ToString(), CultureInfo.InvariantCulture) : 0;

                        db.AddToFormsRSW2014_2_2(rsw_2_2);
                    }
                    db.SaveChanges();

                }
                #endregion

                #region Раздел3

                if (node.Element("Раздел3") != null)
                {
                    XElement Раздел3 = node.Element("Раздел3");

                    var razd_3_list = Раздел3.Descendants().Where(x => x.Name.LocalName == "ЧленКФХ");
                    int i = 0;
                    foreach (var razd_3 in razd_3_list)
                    {
                        i++;
                        FormsRSW2014_2_3 rsw_2_3 = new FormsRSW2014_2_3
                        {
                            FormsRSW2014_2_1D = rsw.ID
                        };

                        int num = 0;
                        if (!int.TryParse(razd_3.Element("НомерПП").Value.ToString(), out num))
                        {
                            num = i;
                        }

                        rsw_2_3.NumRec = num;

                        if (razd_3.Element("Основание") != null)
                        {
                            byte c = 1;
                            byte.TryParse(razd_3.Element("Основание").Value.ToString(), out c);
                            rsw_2_3.CodeBase = c;
                        }


                        rsw_2_3.LastName = razd_3.Element("ФИО").Element("Фамилия") != null ? razd_3.Element("ФИО").Element("Фамилия").Value : "";
                        rsw_2_3.FirstName = razd_3.Element("ФИО").Element("Имя") != null ? razd_3.Element("ФИО").Element("Имя").Value : "";
                        rsw_2_3.MiddleName = razd_3.Element("ФИО").Element("Отчество") != null ? razd_3.Element("ФИО").Element("Отчество").Value : "";

                        var snils = Utils.ParseSNILS_XML(razd_3.Element("СтраховойНомер").Value.ToString(), true);

                        rsw_2_3.InsuranceNumber = snils.num;
                        rsw_2_3.ControlNumber = snils.contrNum;
                        rsw_2_3.Year = short.Parse(razd_3.Element("ГодРождения").Value.ToString());
                        rsw_2_3.DateBegin = DateTime.Parse(razd_3.Element("НачалоПериода").Value.ToString());
                        rsw_2_3.DateEnd = DateTime.Parse(razd_3.Element("КонецПериода").Value.ToString());

                        rsw_2_3.SumOPS_D = razd_3.Element("СуммаОПС") != null ? decimal.Parse(razd_3.Element("СуммаОПС").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                        rsw_2_3.SumStrah_D = razd_3.Element("СуммаСЧ") != null ? decimal.Parse(razd_3.Element("СуммаСЧ").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                        rsw_2_3.SumNakop_D = razd_3.Element("СуммаНЧ") != null ? decimal.Parse(razd_3.Element("СуммаНЧ").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                        rsw_2_3.SumOMS_D = razd_3.Element("СуммаОМС") != null ? decimal.Parse(razd_3.Element("СуммаОМС").Value.ToString(), CultureInfo.InvariantCulture) : 0;

                        db.AddToFormsRSW2014_2_3(rsw_2_3);
                    }
                    db.SaveChanges();

                }
                #endregion

                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Импорт Файлов ПФР Формы РСВ-2 2015
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_RSW2_2015(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {
                node = doc.Descendants().First(x => x.Name.LocalName == "РСВ-2");
                string regnum = node.Element("РегНомерПФР").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);


                string name = "";
                string inn = "";
                string okwed = "";
                string tel = "";
                short year = 0;
                string InsuranceNum = "";
                byte? contrNum = null;

                if (node.Element("ИНН") != null)
                    inn = node.Element("ИНН").Value;
                okwed = node.Element("КодПоОКВЭД").Value;
                if (node.Element("Телефон") != null)
                    tel = node.Element("Телефон").Value;

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        name = node.Element("ФИО").Element("Фамилия") != null ? node.Element("ФИО").Element("Фамилия").Value : "";
                        if (name != "" && insurer.LastName != name)
                        {
                            insurer.LastName = name;
                            change = true;
                        }

                        name = node.Element("ФИО").Element("Имя") != null ? node.Element("ФИО").Element("Имя").Value : "";
                        if (name != "" && insurer.FirstName != name)
                        {
                            insurer.FirstName = name;
                            change = true;
                        }
                        name = node.Element("ФИО").Element("Отчество") != null ? node.Element("ФИО").Element("Отчество").Value : "";
                        if (name != "" && insurer.MiddleName != name)
                        {
                            insurer.MiddleName = name;
                            change = true;
                        }
                        insurer.TypePayer = (byte)1;


                        if (insurer.RegNum != regnum)
                        {
                            insurer.RegNum = regnum;
                            change = true;
                        }
                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.OKWED != okwed)
                        {
                            insurer.OKWED = okwed;
                            change = true;
                        }
                        if (insurer.YearBirth != year)
                        {
                            insurer.YearBirth = year;
                            change = true;
                        }
                        if (insurer.PhoneContact != tel)
                        {
                            insurer.PhoneContact = tel;
                            change = true;
                        }
                        if (insurer.InsuranceNumber != InsuranceNum)
                        {
                            insurer.InsuranceNumber = InsuranceNum;
                            insurer.ControlNumber = contrNum;
                            change = true;
                        }


                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = 1;
                    insurer.LastName = node.Element("ФИО").Element("Фамилия") != null ? node.Element("ФИО").Element("Фамилия").Value : "";
                    insurer.FirstName = node.Element("ФИО").Element("Имя") != null ? node.Element("ФИО").Element("Имя").Value : "";
                    insurer.MiddleName = node.Element("ФИО").Element("Отчество") != null ? node.Element("ФИО").Element("Отчество").Value : "";
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.OKWED = okwed;
                    insurer.InsuranceNumber = InsuranceNum;
                    insurer.ControlNumber = contrNum;
                    insurer.PhoneContact = tel;
                    insurer.YearBirth = year;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }

                short y = short.Parse(node.Element("КалендарныйГод").Value.ToString());
                byte corrNum = byte.Parse(node.Element("НомерУточнения").Value.ToString());

                FormsRSW2014_2_1 rsw = new FormsRSW2014_2_1();
                if (db.FormsRSW2014_2_1.Any(x => x.InsurerID == insurer.ID && x.Year == y && x.CorrectionNum == corrNum))
                {
                    FormsRSW2014_2_1 rswForDel = db.FormsRSW2014_2_1.First(x => x.InsurerID == insurer.ID && x.Year == y && x.CorrectionNum == corrNum);

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_2_1 WHERE ([ID] = {0})", rswForDel.ID));
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\nПри удалении Формы РСВ-2 произошла ошибка. Ошибка: " + ex.Message, this.ThemeName); }));
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = "При удалении Формы РСВ-2 произошла ошибка. Ошибка: " + ex.Message + "\r\n" });

                        return false;
                    }
                }
                rsw.CorrectionNum = corrNum;
                rsw.InsurerID = insurer.ID;
                rsw.Year = y;
                rsw.AutoCalc = false;
                if (node.Element("ПрекращениеДеятельности") != null)
                    rsw.WorkStop = node.Element("ПрекращениеДеятельности").Value.ToString() == "Л" ? (byte)1 : (byte)0;
                else
                    rsw.WorkStop = (byte)0;

                rsw.CountEmployers = node.Element("ЧленовКФХ") != null ? int.Parse(node.Element("ЧленовКФХ").Value.ToString()) : 0;
                rsw.CountConfirmDoc = node.Element("ЛистовПриложения") != null ? byte.Parse(node.Element("ЛистовПриложения").Value.ToString()) : (byte)0;

                // Что это такое???
                //node.Element("КоличествоСтраниц").Value.ToString()
                //                List<string> strCodes = new List<string> { "100", "110", "111", "112", "113", "114", "120", "121", "130", "140", "141", "142", "143", "144", "150"};

                DateTime dt = DateTime.Now;
                DateTime.TryParse(node.Element("ДатаЗаполнения").Value.ToString(), out dt);
                rsw.DateUnderwrite = dt;

                XElement node_confirm = node.Element("ПодтверждениеСведений");

                rsw.ConfirmType = byte.Parse(node_confirm.Element("ТипПодтверждающего").Value.ToString());

                rsw.ConfirmLastName = node_confirm.Element("ФИОПодтверждающего").Element("Фамилия") != null ? node_confirm.Element("ФИОПодтверждающего").Element("Фамилия").Value : "";
                rsw.ConfirmFirstName = node_confirm.Element("ФИОПодтверждающего").Element("Имя") != null ? node_confirm.Element("ФИОПодтверждающего").Element("Имя").Value : "";
                rsw.ConfirmMiddleName = node_confirm.Element("ФИОПодтверждающего").Element("Отчество") != null ? node_confirm.Element("ФИОПодтверждающего").Element("Отчество").Value : "";
                if (rsw.ConfirmType == 2)
                {
                    if (node_confirm.Element("НаименованиеОрганизации") != null)
                    {
                        rsw.ConfirmOrgName = node_confirm.Element("НаименованиеОрганизации").Value;
                    }
                    if (node_confirm.Element("Доверенность") != null)
                    {
                        node_confirm = node_confirm.Element("Доверенность");

                        rsw.ConfirmDocType_ID = db.DocumentTypes.FirstOrDefault(x => x.Code == "ПРОЧЕЕ").ID;
                        rsw.ConfirmDocName = "ДОВЕРЕННОСТЬ";

                        rsw.ConfirmDocSerLat = node_confirm.Element("Серия") != null ? node_confirm.Element("Серия").Value : "";

                        int n = 0;
                        rsw.ConfirmDocNum = node_confirm.Element("Номер") != null ? (int.TryParse(node_confirm.Element("Номер").Value.ToString(), out n) ? n : 0) : 0;
                        if (node_confirm.Element("ДатаВыдачи") != null && !String.IsNullOrEmpty(node_confirm.Element("ДатаВыдачи").Value.ToString()))
                            rsw.ConfirmDocDate = DateTime.Parse(node_confirm.Element("ДатаВыдачи").Value.ToString());
                        rsw.ConfirmDocKemVyd = node_confirm.Element("КемВыдан") != null ? node_confirm.Element("КемВыдан").Value : "";
                        if (node_confirm.Element("ДействуетС") != null && !String.IsNullOrEmpty(node_confirm.Element("ДействуетС").Value.ToString()))
                            rsw.ConfirmDocDateBegin = DateTime.Parse(node_confirm.Element("ДействуетС").Value.ToString());
                        if (node_confirm.Element("ДействуетПо") != null && !String.IsNullOrEmpty(node_confirm.Element("ДействуетПо").Value.ToString()))
                            rsw.ConfirmDocDateEnd = DateTime.Parse(node_confirm.Element("ДействуетПо").Value.ToString());

                    }
                }

                #region Раздел1

                XElement Раздел1 = node.Element("Раздел1");
                var nodes = Раздел1.Descendants().Where(x => x.Name.LocalName == "Код");
                foreach (var n in nodes)
                {
                    string strCode = n.Value.ToString();

                    foreach (var item_ in n.Parent.Elements())
                    {
                        string itemName = "s_" + strCode + "_";
                        decimal data = 0;
                        if (item_.Name.LocalName != "Код")
                        {
                            int i = -1;
                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);

                            switch (item_.Name.LocalName)
                            {
                                case "СуммаОПС":
                                    i = 0;
                                    break;
                                case "СуммаСЧ":
                                    i = 1;
                                    break;
                                case "СуммаНЧ":
                                    i = 2;
                                    break;
                                case "СуммаОМС":
                                    i = 3;
                                    break;
                            }
                            if (i >= 0)
                            {
                                itemName = itemName + i.ToString();
                                rsw.GetType().GetProperty(itemName).SetValue(rsw, data, null);
                            }
                        }
                    }

                }
                db.AddToFormsRSW2014_2_1(rsw);
                db.SaveChanges();
                #endregion

                #region Раздел2

                if (node.Element("Раздел2") != null)
                {
                    XElement Раздел2 = node.Element("Раздел2");

                    var razd_2_list = Раздел2.Descendants().Where(x => x.Name.LocalName == "ЧленКФХ");
                    int i = 0;
                    foreach (var razd_2 in razd_2_list)
                    {
                        i++;
                        FormsRSW2014_2_2 rsw_2_2 = new FormsRSW2014_2_2
                        {
                            FormsRSW2014_2_1D = rsw.ID
                        };

                        int num = 0;
                        if (!int.TryParse(razd_2.Element("НомерПП").Value.ToString(), out num))
                        {
                            num = i;
                        }

                        rsw_2_2.NumRec = num;
                        rsw_2_2.LastName = razd_2.Element("ФИО").Element("Фамилия") != null ? razd_2.Element("ФИО").Element("Фамилия").Value : "";
                        rsw_2_2.FirstName = razd_2.Element("ФИО").Element("Имя") != null ? razd_2.Element("ФИО").Element("Имя").Value : "";
                        rsw_2_2.MiddleName = razd_2.Element("ФИО").Element("Отчество") != null ? razd_2.Element("ФИО").Element("Отчество").Value : "";


                        var snils = Utils.ParseSNILS_XML(razd_2.Element("СНИЛС").Value.ToString(), true);


                        rsw_2_2.InsuranceNumber = snils.num;
                        rsw_2_2.ControlNumber = snils.contrNum;
                        rsw_2_2.Year = short.Parse(razd_2.Element("ГодРождения").Value.ToString());
                        rsw_2_2.DateBegin = DateTime.Parse(razd_2.Element("НачалоПериода").Value.ToString());
                        rsw_2_2.DateEnd = DateTime.Parse(razd_2.Element("КонецПериода").Value.ToString());

                        rsw_2_2.SumOPS = razd_2.Element("СуммаОПС") != null ? decimal.Parse(razd_2.Element("СуммаОПС").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                        rsw_2_2.SumOMS = razd_2.Element("СуммаОМС") != null ? decimal.Parse(razd_2.Element("СуммаОМС").Value.ToString(), CultureInfo.InvariantCulture) : 0;

                        db.AddToFormsRSW2014_2_2(rsw_2_2);
                    }
                    db.SaveChanges();

                }
                #endregion

                #region Раздел3

                if (node.Element("Раздел3") != null)
                {
                    XElement Раздел3 = node.Element("Раздел3");

                    var razd_3_list = Раздел3.Descendants().Where(x => x.Name.LocalName == "ЧленКФХ");
                    int i = 0;
                    foreach (var razd_3 in razd_3_list)
                    {
                        i++;
                        FormsRSW2014_2_3 rsw_2_3 = new FormsRSW2014_2_3
                        {
                            FormsRSW2014_2_1D = rsw.ID
                        };

                        int num = 0;
                        if (!int.TryParse(razd_3.Element("НомерПП").Value.ToString(), out num))
                        {
                            num = i;
                        }

                        rsw_2_3.NumRec = num;

                        if (razd_3.Element("Основание") != null)
                        {
                            byte c = 1;
                            byte.TryParse(razd_3.Element("Основание").Value.ToString(), out c);
                            rsw_2_3.CodeBase = c;
                        }


                        rsw_2_3.LastName = razd_3.Element("ФИО").Element("Фамилия") != null ? razd_3.Element("ФИО").Element("Фамилия").Value : "";
                        rsw_2_3.FirstName = razd_3.Element("ФИО").Element("Имя") != null ? razd_3.Element("ФИО").Element("Имя").Value : "";
                        rsw_2_3.MiddleName = razd_3.Element("ФИО").Element("Отчество") != null ? razd_3.Element("ФИО").Element("Отчество").Value : "";

                        var snils = Utils.ParseSNILS_XML(razd_3.Element("СНИЛС").Value.ToString(), true);

                        rsw_2_3.InsuranceNumber = snils.num;
                        rsw_2_3.ControlNumber = snils.contrNum;
                        rsw_2_3.Year = short.Parse(razd_3.Element("ГодРождения").Value.ToString());
                        rsw_2_3.DateBegin = DateTime.Parse(razd_3.Element("НачалоПериода").Value.ToString());
                        rsw_2_3.DateEnd = DateTime.Parse(razd_3.Element("КонецПериода").Value.ToString());

                        rsw_2_3.SumOPS_D = razd_3.Element("СуммаОПС") != null ? decimal.Parse(razd_3.Element("СуммаОПС").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                        rsw_2_3.SumStrah_D = razd_3.Element("СуммаСЧ") != null ? decimal.Parse(razd_3.Element("СуммаСЧ").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                        rsw_2_3.SumNakop_D = razd_3.Element("СуммаНЧ") != null ? decimal.Parse(razd_3.Element("СуммаНЧ").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                        rsw_2_3.SumOMS_D = razd_3.Element("СуммаОМС") != null ? decimal.Parse(razd_3.Element("СуммаОМС").Value.ToString(), CultureInfo.InvariantCulture) : 0;

                        db.AddToFormsRSW2014_2_3(rsw_2_3);
                    }
                    db.SaveChanges();

                }
                #endregion

                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Импорт Файлов ПФР Формы РВ-3 2015
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_RW3_2015(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {
                node = doc.Descendants().First(x => x.Name.LocalName == "РВ-3");
                string regnum = node.Element("РегНомерПФР").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                string[] name = new string[3];
                string inn = "";
                string kpp = "";
                string okwed = "";
                string tel = "";
                byte type_ = (byte)0;

                if (node.Element("НаименованиеОрганизации") != null)
                {
                    name[0] = node.Element("НаименованиеОрганизации").Value;
                }

                if (node.Element("КПП") != null)
                    kpp = node.Element("КПП").Value;
                if (node.Element("КодПоОКВЭД") != null)
                    okwed = node.Element("КодПоОКВЭД").Value;
                if (node.Element("ИНН") != null)
                    inn = node.Element("ИНН").Value;
                if (node.Element("Телефон") != null)
                    tel = node.Element("Телефон").Value;

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (insurer.NameShort != name[0])
                        {
                            insurer.NameShort = name[0];
                            change = true;
                        }
                        if (insurer.Name != name[0])
                        {
                            insurer.Name = name[0];
                            change = true;
                        }
                        if (insurer.RegNum != regnum)
                        {
                            insurer.RegNum = regnum;
                            change = true;
                        }
                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }
                        if (insurer.OKWED != okwed)
                        {
                            insurer.OKWED = okwed;
                            change = true;
                        }
                        if (insurer.PhoneContact != tel)
                        {
                            insurer.PhoneContact = tel;
                            change = true;
                        }


                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = type_;
                    insurer.NameShort = name[0];
                    insurer.Name = name[0];
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    insurer.OKWED = okwed;
                    insurer.PhoneContact = tel;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }

                byte corrNum = 0;

                if (node.Element("НомерУточнения") != null)
                {
                    byte.TryParse(node.Element("НомерУточнения").Value.ToString(), out corrNum);
                }

                byte q = 0;

                if (node.Element("КодОтчПериода") != null)
                {
                    byte.TryParse(node.Element("КодОтчПериода").Value.ToString(), out q);
                }

                short y = 0;

                if (node.Element("КалендарныйГод") != null)
                {
                    short.TryParse(node.Element("КалендарныйГод").Value.ToString(), out y);
                }

                byte codeTar = 0;

                if (node.Element("КодТарифа") != null)
                {
                    byte.TryParse(node.Element("КодТарифа").Value.ToString(), out codeTar);
                }

                FormsRW3_2015 rw3 = new FormsRW3_2015();
                if (db.FormsRW3_2015.Any(x => x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.CorrectionNum == corrNum && x.CodeTar == codeTar))
                {
                    FormsRW3_2015 rw3ForDel = db.FormsRW3_2015.First(x => x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.CorrectionNum == corrNum && x.CodeTar == codeTar);

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRW3_2015 WHERE ([ID] = {0})", rw3ForDel.ID));
                    }
                    catch (Exception ex)
                    {
                        this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\nПри удалении Формы РВ-3 2015 произошла ошибка. Ошибка: " + ex.Message, this.ThemeName); }));
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = "При удалении Формы РВ-3 2015 произошла ошибка. Ошибка: " + ex.Message + "\r\n" });

                        return false;
                    }
                }
                rw3.CorrectionNum = corrNum;
                rw3.InsurerID = insurer.ID;
                rw3.Year = y;
                rw3.Quarter = q;
                rw3.CodeTar = codeTar;

                rw3.AutoCalc = false;
                if (node.Element("ПрекращениеДеятельности") != null)
                    rw3.WorkStop = node.Element("ПрекращениеДеятельности").Value.ToString() == "Л" ? (byte)1 : (byte)0;
                else
                    rw3.WorkStop = (byte)0;

                rw3.CountConfirmDoc = node.Element("ЛистовПриложения") != null ? byte.Parse(node.Element("ЛистовПриложения").Value.ToString()) : (byte)0;

                // Что это такое???
                //node.Element("КоличествоСтраниц").Value.ToString()
                //                List<string> strCodes = new List<string> { "100", "110", "111", "112", "113", "114", "120", "121", "130", "140", "141", "142", "143", "144", "150"};

                DateTime dt = DateTime.Now;
                DateTime.TryParse(node.Element("ДатаЗаполнения").Value.ToString(), out dt);
                rw3.DateUnderwrite = dt;

                XElement node_confirm = node.Element("ПодтверждениеСведений");

                rw3.ConfirmType = byte.Parse(node_confirm.Element("ТипПодтверждающего").Value.ToString());

                rw3.ConfirmLastName = node_confirm.Element("ФИОПодтверждающего").Element("Фамилия") != null ? node_confirm.Element("ФИОПодтверждающего").Element("Фамилия").Value : "";
                rw3.ConfirmFirstName = node_confirm.Element("ФИОПодтверждающего").Element("Имя") != null ? node_confirm.Element("ФИОПодтверждающего").Element("Имя").Value : "";
                rw3.ConfirmMiddleName = node_confirm.Element("ФИОПодтверждающего").Element("Отчество") != null ? node_confirm.Element("ФИОПодтверждающего").Element("Отчество").Value : "";
                if (rw3.ConfirmType >= 2)
                {
                    rw3.ConfirmOrgName = node_confirm.Element("НаименованиеОрганизации") != null ? node_confirm.Element("НаименованиеОрганизации").Value : "";

                    if (node_confirm.Element("Доверенность") != null)
                    {
                        node_confirm = node_confirm.Element("Доверенность");

                        rw3.ConfirmDocType_ID = db.DocumentTypes.FirstOrDefault(x => x.Code == "ПРОЧЕЕ").ID;
                        rw3.ConfirmDocName = "ДОВЕРЕННОСТЬ";

                        rw3.ConfirmDocSerLat = node_confirm.Element("Серия") != null ? node_confirm.Element("Серия").Value : "";

                        int n = 0;
                        rw3.ConfirmDocNum = node_confirm.Element("Номер") != null ? (int.TryParse(node_confirm.Element("Номер").Value.ToString(), out n) ? n : 0) : 0;
                        if (node_confirm.Element("ДатаВыдачи") != null && !String.IsNullOrEmpty(node_confirm.Element("ДатаВыдачи").Value.ToString()))
                            rw3.ConfirmDocDate = DateTime.Parse(node_confirm.Element("ДатаВыдачи").Value.ToString());
                        rw3.ConfirmDocKemVyd = node_confirm.Element("КемВыдан").Value;
                        if (node_confirm.Element("ДействуетС") != null && !String.IsNullOrEmpty(node_confirm.Element("ДействуетС").Value.ToString()))
                            rw3.ConfirmDocDateBegin = DateTime.Parse(node_confirm.Element("ДействуетС").Value.ToString());
                        if (node_confirm.Element("ДействуетПо") != null && !String.IsNullOrEmpty(node_confirm.Element("ДействуетПо").Value.ToString()))
                            rw3.ConfirmDocDateEnd = DateTime.Parse(node_confirm.Element("ДействуетПо").Value.ToString());
                    }
                }

                #region Раздел1

                XElement Раздел1 = node.Element("Раздел1");
                var nodes = Раздел1.Descendants().Where(x => x.Name.LocalName == "Строка");
                foreach (var n in nodes)
                {
                    string strCode = n.Element("Код").Value.ToString();
                    decimal sum = 0;
                    decimal.TryParse(n.Element("Взносы").Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out sum);

                    string itemName = "s_" + strCode + "_0";
                    rw3.GetType().GetProperty(itemName).SetValue(rw3, sum, null);
                }
                #endregion

                #region Раздел2

                XElement Раздел2 = node.Element("Раздел2");
                nodes = Раздел2.Descendants().Where(x => x.Name.LocalName == "Строка");
                foreach (var n in nodes)
                {
                    string strCode = n.Element("Код").Value.ToString();

                    foreach (var item_ in n.Elements())
                    {
                        string itemName = "s_" + strCode + "_";
                        decimal data = 0;
                        if (item_.Name.LocalName != "Код")
                        {
                            int i = -1;
                            data = decimal.Parse(item_.Value.ToString(), CultureInfo.InvariantCulture);

                            switch (item_.Name.LocalName)
                            {
                                case "Всего":
                                    i = 0;
                                    break;
                                case "Месяц1":
                                    i = 1;
                                    break;
                                case "Месяц2":
                                    i = 2;
                                    break;
                                case "Месяц3":
                                    i = 3;
                                    break;
                            }
                            if (i >= 0)
                            {
                                itemName = itemName + i.ToString();
                                rw3.GetType().GetProperty(itemName).SetValue(rw3, data, null);
                            }
                        }
                    }

                }

                if (Раздел2.Element("Строка230") != null)
                {
                    XElement Строка230 = Раздел2.Element("Строка230");
                    rw3.s_230_0 = Строка230.Element("Всего") != null ? long.Parse(Строка230.Element("Всего").Value.ToString()) : 0;
                    rw3.s_230_1 = Строка230.Element("Месяц1") != null ? long.Parse(Строка230.Element("Месяц1").Value.ToString()) : 0;
                    rw3.s_230_2 = Строка230.Element("Месяц2") != null ? long.Parse(Строка230.Element("Месяц2").Value.ToString()) : 0;
                    rw3.s_230_3 = Строка230.Element("Месяц3") != null ? long.Parse(Строка230.Element("Месяц3").Value.ToString()) : 0;
                }


                db.AddToFormsRW3_2015(rw3);
                db.SaveChanges();
                #endregion


                #region Раздел3

                if (node.Element("Раздел3") != null)
                {
                    XElement Раздел3 = node.Element("Раздел3");

                    var razd_3_list = Раздел3.Descendants().Where(x => x.Name.LocalName == "Перерасчет");
                    foreach (var razd_3 in razd_3_list)
                    {
                        FormsRW3_2015_Razd_3 rw_3_3 = new FormsRW3_2015_Razd_3
                        {
                            FormsRW3_2015_ID = rw3.ID
                        };

                        byte codeBase = (byte)0;
                        rw_3_3.CodeBase = razd_3.Element("Основание") != null ? (byte.TryParse(razd_3.Element("Основание").Value.ToString(), out codeBase) ? codeBase : (byte)0) : (byte)0;

                        y = 0;
                        rw_3_3.Year = razd_3.Element("Год") != null ? (short.TryParse(razd_3.Element("Год").Value.ToString(), out y) ? y : (short)0) : (short)0;

                        byte m = (byte)0;
                        rw_3_3.Month = razd_3.Element("Месяц") != null ? (byte.TryParse(razd_3.Element("Месяц").Value.ToString(), out m) ? m : (byte)0) : (byte)0;

                        decimal sum = 0;
                        rw_3_3.SumFee = razd_3.Element("Сумма") != null ? (decimal.TryParse(razd_3.Element("Сумма").Value.ToString(), out sum) ? sum : 0) : 0;

                        db.AddToFormsRW3_2015_Razd_3(rw_3_3);
                    }
                    db.SaveChanges();

                }
                #endregion

                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));

                result = false;
            }

            return result;
        }

        /// <summary>
        /// Импорт Файлов ПФР Формы СЗВ-М 2016
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_SZVM_2016(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {
                node = doc.Descendants().First(x => x.Name.LocalName == "Страхователь");
                string regnum = node.Element("РегНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                string name = "";
                string inn = "";
                string kpp = "";
                byte type_ = (byte)0;

                if (node.Element("НаименованиеКраткое") != null)
                {
                    name = node.Element("НаименованиеКраткое").Value;
                }

                if (node.Element("КПП") != null)
                    kpp = node.Element("КПП").Value;
                if (node.Element("ИНН") != null)
                    inn = node.Element("ИНН").Value;

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (insurer.NameShort != name)
                        {
                            insurer.NameShort = name;
                            change = true;
                        }
                        if (String.IsNullOrEmpty(insurer.Name))
                        {
                            insurer.Name = name;
                            change = true;
                        }
                        if (insurer.RegNum != regnum)
                        {
                            insurer.RegNum = regnum;
                            change = true;
                        }
                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }

                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = type_;
                    insurer.NameShort = name;
                    insurer.Name = name;
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }

                XElement СЗВ_М = doc.Descendants().First(x => x.Name.LocalName == "СЗВ-М");

                long TypeInfoID = 1;

                if (СЗВ_М.Element("ТипФормы") != null)
                {
                    long.TryParse(СЗВ_М.Element("ТипФормы").Value.ToString(), out TypeInfoID);
                }

                node = СЗВ_М.Element("ОтчетныйПериод");

                byte m = 1;

                if (node.Element("Месяц") != null)
                {
                    byte.TryParse(node.Element("Месяц").Value.ToString(), out m);
                }

                short y = 0;

                if (node.Element("КалендарныйГод") != null)
                {
                    short.TryParse(node.Element("КалендарныйГод").Value.ToString(), out y);
                }

                FormsSZV_M_2016 szvm = new FormsSZV_M_2016();
                if (db.FormsSZV_M_2016.Any(x => x.InsurerID == insurer.ID && x.YEAR == y && x.MONTH == m && x.TypeInfoID == TypeInfoID))
                {
                    if (updateSZVM_DDL.SelectedItem.Tag.ToString() == "0")
                    {
                        string id = db.FormsSZV_M_2016.First(x => x.InsurerID == insurer.ID && x.YEAR == y && x.MONTH == m && x.TypeInfoID == TypeInfoID).ID.ToString();
                        try
                        {
                            db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_M_2016 WHERE ([ID] = {0})", id));

                            szvm.InsurerID = insurer.ID;
                            szvm.YEAR = y;
                            szvm.MONTH = m;
                            szvm.TypeInfoID = TypeInfoID;


                            DateTime dt = DateTime.Now;
                            DateTime.TryParse(СЗВ_М.Element("ДатаЗаполнения").Value.ToString(), out dt);
                            szvm.DateFilling = dt;

                            db.AddToFormsSZV_M_2016(szvm);
                            //                db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\nПри удалении Формы СЗВ-М 2016 произошла ошибка. Ошибка: " + ex.Message, this.ThemeName); }));
                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = "При удалении Формы СЗВ-М 2016 произошла ошибка. Ошибка: " + ex.Message + "\r\n" });

                            return false;
                        }
                    }
                    else if (updateSZVM_DDL.SelectedItem.Tag.ToString() == "1")
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = "Файл Формы СЗВ-М 2016 был пропущен, т.к. Форма с такими же параметрами уже есть в базе данных. Можно изменить настройки импорта формы, для перезаписи\r\n" });

                        return false;
                    }
                    else if (updateSZVM_DDL.SelectedItem.Tag.ToString() == "2")  // объединение
                    {
                        szvm = db.FormsSZV_M_2016.First(x => x.InsurerID == insurer.ID && x.YEAR == y && x.MONTH == m && x.TypeInfoID == TypeInfoID);
                    }


                }
                else
                {
                    szvm.InsurerID = insurer.ID;
                    szvm.YEAR = y;
                    szvm.MONTH = m;
                    szvm.TypeInfoID = TypeInfoID;


                    DateTime dt = DateTime.Now;
                    DateTime.TryParse(СЗВ_М.Element("ДатаЗаполнения").Value.ToString(), out dt);
                    szvm.DateFilling = dt;

                    db.AddToFormsSZV_M_2016(szvm);
                }



                #region СписокЗЛ

                int i = 0;

                if (СЗВ_М.Element("СписокЗЛ") != null)
                {
                    XElement СписокЗЛ = СЗВ_М.Element("СписокЗЛ");

                    var ЗЛ = СписокЗЛ.Descendants().Where(x => x.Name.LocalName == "ЗЛ");

                    int count = ЗЛ.Count();

                    foreach (var item in ЗЛ)
                    {
                        i++;
                        string LastName = item.Element("ФИО") != null ? item.Element("ФИО").Element("Фамилия") != null ? item.Element("ФИО").Element("Фамилия").Value : "" : "";
                        string FirstName = item.Element("ФИО") != null ? item.Element("ФИО").Element("Имя") != null ? item.Element("ФИО").Element("Имя").Value : "" : "";
                        string MiddleName = item.Element("ФИО") != null ? item.Element("ФИО").Element("Отчество") != null ? item.Element("ФИО").Element("Отчество").Value : "" : "";

                        if (item.Element("СНИЛС") == null || (item.Element("СНИЛС") != null && String.IsNullOrEmpty(item.Element("СНИЛС").Value)))
                        {
                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = i.ToString(), type = LastName + " " + FirstName + " " + MiddleName + ". Ошибка при загрузке Индивидуальных сведений. Не указан Страховой номер (СНИЛС) сотрудника.\r\n" });
                            continue;
                        }

                        var snils = Utils.ParseSNILS_XML(item.Element("СНИЛС").Value.ToString(), true);

                        string INN = item.Element("ИНН") != null ? item.Element("ИНН").Value : "";


                        Staff staff = new Staff();
                        if (db.Staff.Any(x => x.InsurerID == insurer.ID && x.InsuranceNumber == snils.num))  // Если такой сотрудник уже есть
                        {
                            if (updateStaffData.Checked)
                            {
                                staff = db.Staff.FirstOrDefault(x => x.InsurerID == insurer.ID && x.InsuranceNumber == snils.num);

                                if ((staff.LastName != LastName) || (staff.FirstName != FirstName) || (staff.MiddleName != MiddleName) || (staff.InsuranceNumber != snils.num) || (staff.INN != INN))
                                {
                                    staff.InsuranceNumber = snils.num;
                                    staff.ControlNumber = snils.contrNum;
                                    staff.INN = INN;
                                    staff.LastName = LastName;
                                    staff.FirstName = FirstName;
                                    staff.MiddleName = MiddleName;
                                    staff.Dismissed = staff.Dismissed.Value;
                                    db.ObjectStateManager.ChangeObjectState(staff, EntityState.Modified);
                                }



                            }
                        }
                        else // если добавляем нового сотрудника
                        {

                            staff.InsurerID = insurer.ID;
                            staff.InsuranceNumber = snils.num.PadLeft(9, '0');
                            staff.ControlNumber = snils.contrNum;
                            staff.INN = INN;
                            staff.LastName = LastName;
                            staff.FirstName = FirstName;
                            staff.MiddleName = MiddleName;
                            staff.Dismissed = (byte)0;
                            db.AddToStaff(staff);
                        }

                        if (bw.CancellationPending)
                        {
                            db.SaveChanges();
                            return false;
                        }

                        if (i % 100 == 0)
                        {
                            db.SaveChanges();
                        }
                        cnt_records_imported++;
                        importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = i; }));

                        decimal temp = (decimal)i / (decimal)count;
                        int proc = (int)Math.Round((temp * 100), 0);
                        bw.ReportProgress(proc, i.ToString());

                    }
                    db.SaveChanges();


                    var snilsList = ЗЛ.Descendants().Where(x => x.Name.LocalName == "СНИЛС");
                    List<string> snilss = new List<string>();
                    foreach (var item in snilsList.Where(x => !String.IsNullOrEmpty(x.Value)))
                    {
                        snilss.Add(Utils.ParseSNILS_XML(item.Value.ToString(), false).num);
                    }

                    var staffList = db.Staff.Where(x => x.InsurerID == insurer.ID && snilss.Contains(x.InsuranceNumber)).Select(x => x.ID).ToList();

                    if (szvm.FormsSZV_M_2016_Staff.Any())
                    {
                        var existStaffIDList = szvm.FormsSZV_M_2016_Staff.Select(x => x.StaffID).ToList();
                        staffList = staffList.Except(existStaffIDList).ToList();
                    }

                    i = 0;
                    foreach (var item in staffList)
                    {
                        i++;

                        szvm.FormsSZV_M_2016_Staff.Add(new FormsSZV_M_2016_Staff { StaffID = item });

                        if (bw.CancellationPending)
                        {
                            db.SaveChanges();
                            return false;
                        }

                        if (i % 100 == 0)
                        {
                            db.SaveChanges();
                        }

                    }
                    db.SaveChanges();


                }
                #endregion

                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));

                result = false;
            }

            return result;
        }


        public class staffImpCont
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string num { get; set; }
        }

        /// <summary>
        /// Импорт Файлов ПФР Формы ОДВ-1 СЗВ-СТАЖ
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_SZV_STAJ(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {
                node = doc.Descendants().First(x => x.Name.LocalName == "Страхователь");
                string regnum = node.Element("РегНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                string name = "";
                string inn = "";
                string kpp = "";
                byte type_ = (byte)0;

                if (node.Element("Наименование") != null)
                {
                    name = node.Element("Наименование").Value;
                }

                if (node.Element("КПП") != null)
                    kpp = node.Element("КПП").Value;
                if (node.Element("ИНН") != null)
                    inn = node.Element("ИНН").Value;

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (insurer.NameShort != name)
                        {
                            insurer.NameShort = name;
                            change = true;
                        }
                        if (String.IsNullOrEmpty(insurer.Name))
                        {
                            insurer.Name = name;
                            change = true;
                        }
                        if (insurer.RegNum != regnum)
                        {
                            insurer.RegNum = regnum;
                            change = true;
                        }
                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }

                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = type_;
                    insurer.NameShort = name;
                    insurer.Name = name;
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }

                XElement ОДВ1 = doc.Descendants().First(x => x.Name.LocalName == "ОДВ-1");

                FormsODV_1_2017 odv1 = new FormsODV_1_2017();
                odv1.InsurerID = insurer.ID;

                byte TypeInfoODV = 0;
                if (ОДВ1.Element("Тип") != null)
                {
                    byte.TryParse(ОДВ1.Element("Тип").Value.ToString(), out TypeInfoODV);
                }

                odv1.TypeInfo = TypeInfoODV;
                odv1.TypeForm = 1;
                odv1.Code = 0;

                node = ОДВ1.Element("ОтчетныйПериод");
                short y = 0;

                if (node.Element("Год") != null)
                {
                    short.TryParse(node.Element("Год").Value.ToString(), out y);
                }
                odv1.Year = y;

                long staffCount = 0;

                if (ОДВ1.Element("КоличествоЗЛ") != null)
                {
                    long.TryParse(ОДВ1.Element("КоличествоЗЛ").Value.ToString(), out staffCount);
                }
                odv1.StaffCount = staffCount;

                if (ОДВ1.Element("Руководитель") != null)
                {
                    node = ОДВ1.Element("Руководитель");
                    if (node.Element("Должность") != null)
                    {
                        odv1.ConfirmDolgn = node.Element("Должность").Value.ToString();
                    }
                    else
                        odv1.ConfirmDolgn = "";

                    if (node.Element("ФИО") != null)
                    {
                        node = node.Element("ФИО");
                        if (node.Element("Фамилия") != null)
                        {
                            odv1.ConfirmLastName = node.Element("Фамилия").Value.ToString();
                        }
                        else
                            odv1.ConfirmLastName = "";

                        if (node.Element("Имя") != null)
                        {
                            odv1.ConfirmFirstName = node.Element("Имя").Value.ToString();
                        }
                        else
                            odv1.ConfirmFirstName = "";

                        if (node.Element("Отчество") != null)
                        {
                            odv1.ConfirmMiddleName = node.Element("Отчество").Value.ToString();
                        }
                        else
                            odv1.ConfirmMiddleName = "";
                    }

                }

                DateTime datefill = DateTime.Now;

                if (ОДВ1.Element("ДатаЗаполнения") != null)
                {
                    DateTime.TryParse(ОДВ1.Element("ДатаЗаполнения").Value.ToString(), out datefill);
                }
                odv1.DateFilling = datefill;

                short g = 0;
                var ОснованияДНП = ОДВ1.Descendants().Where(x => x.Name.LocalName == "ОснованияДНП");
                foreach (var item in ОснованияДНП)
                {
                    var Основание = item.Elements("Основание");

                    foreach (var osn in Основание)
                    {
                        FormsODV_1_5_2017 odv1_5 = new FormsODV_1_5_2017();
                        g++;

                        odv1_5.Num = g;
                        if (osn.Element("Подразделение") != null)
                        {
                            odv1_5.Department = checkStringLength(osn.Element("Подразделение").Value.ToString(), 200);
                        }
                        else
                            odv1_5.Department = "";

                        if (osn.Element("ПрофессияДолжность") != null)
                        {
                            odv1_5.Profession = checkStringLength(osn.Element("ПрофессияДолжность").Value.ToString(), 200);
                        }
                        else
                            odv1_5.Profession = "";

                        if (osn.Element("Описание") != null)
                        {
                            odv1_5.VidRabotFakt = checkStringLength(osn.Element("Описание").Value.ToString(), 200);
                        }
                        else
                            odv1_5.VidRabotFakt = "";

                        if (osn.Element("Документы") != null)
                        {
                            odv1_5.DocsName = checkStringLength(osn.Element("Документы").Value.ToString(), 200);
                        }
                        else
                            odv1_5.DocsName = "";


                        long StaffCountShtat = 0;
                        long.TryParse(osn.Element("КоличествоШтат").Value.ToString(), out StaffCountShtat);
                        odv1_5.StaffCountShtat = StaffCountShtat;

                        long StaffCountFakt = 0;
                        long.TryParse(osn.Element("КоличествоФакт").Value.ToString(), out StaffCountFakt);
                        odv1_5.StaffCountFakt = StaffCountFakt;

                        foreach (var ОУТ in osn.Elements("ОУТ"))
                        {
                            FormsODV_1_5_2017_OUT newOUT = new FormsODV_1_5_2017_OUT();

                            newOUT.OsobUslTrudaCode = checkStringLength(ОУТ.Element("Код").Value.ToString().Trim(), 10);

                            if (ОУТ.Element("ПозицияСписка") != null)
                            {
                                newOUT.CodePosition = checkStringLength(ОУТ.Element("ПозицияСписка").Value.ToString(), 20);
                            }
                            odv1_5.FormsODV_1_5_2017_OUT.Add(newOUT);

                        }


                        odv1.FormsODV_1_5_2017.Add(odv1_5);
                    }


                    long StaffCountOsobUslShtat = 0;
                    long.TryParse(item.Element("ВсегоШтат").Value.ToString(), out StaffCountOsobUslShtat);
                    odv1.StaffCountOsobUslShtat = StaffCountOsobUslShtat;

                    long StaffCountOsobUslFakt = 0;
                    long.TryParse(item.Element("ВсегоФакт").Value.ToString(), out StaffCountOsobUslFakt);
                    odv1.StaffCountOsobUslFakt = StaffCountOsobUslFakt;

                }


                bool exist = false;
                long odv_id = 0;
                // если ОДВ-1 с такими параметрами уже есть
                if (db.FormsODV_1_2017.Any(x => x.InsurerID == odv1.InsurerID && x.TypeForm == odv1.TypeForm && x.TypeInfo == odv1.TypeInfo && x.Year == odv1.Year && x.Code == odv1.Code))
                {
                    odv_id = db.FormsODV_1_2017.FirstOrDefault(x => x.InsurerID == odv1.InsurerID && x.TypeForm == odv1.TypeForm && x.TypeInfo == odv1.TypeInfo && x.Year == odv1.Year && x.Code == odv1.Code).ID;
                    if (updateODV_1_DDL.SelectedItem.Tag.ToString() == "0")  // заменить форму
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsODV_1_2017 WHERE ([ID] = {0})", odv_id));
                    }
                    else
                        exist = true;
                }

                if (exist) // если такая запись уже существует
                {
                    if (updateODV_1_DDL.SelectedItem.Tag.ToString() == "2") // объединение
                    {
                        var odv1_old = db.FormsODV_1_2017.FirstOrDefault(x => x.InsurerID == odv1.InsurerID && x.TypeForm == odv1.TypeForm && x.TypeInfo == odv1.TypeInfo && x.Year == odv1.Year && x.Code == odv1.Code);

                        short num = (short)odv1_old.FormsODV_1_5_2017.Count();

                        try
                        {
                            foreach (var item in odv1.FormsODV_1_5_2017)
                            {
                                num++;

                                FormsODV_1_5_2017 newRec = new FormsODV_1_5_2017
                                {
                                    Department = item.Department,
                                    DocsName = item.DocsName,
                                    Num = num,
                                    Profession = item.Profession,
                                    StaffCountFakt = item.StaffCountFakt.HasValue ? item.StaffCountFakt.Value : 1,
                                    StaffCountShtat = item.StaffCountShtat.HasValue ? item.StaffCountShtat.Value : 1,
                                    VidRabotFakt = item.VidRabotFakt
                                };

                                foreach (var it in item.FormsODV_1_5_2017_OUT.ToList())
                                {
                                    newRec.FormsODV_1_5_2017_OUT.Add(it);
                                }


                                odv1_old.FormsODV_1_5_2017.Add(newRec);
                            }

                            odv1_old.StaffCount += odv1.StaffCount.HasValue ? odv1.StaffCount.Value : 0;
                            odv1_old.StaffCountOsobUslFakt += odv1.StaffCountOsobUslFakt.HasValue ? odv1.StaffCountOsobUslFakt.Value : 1;
                            odv1_old.StaffCountOsobUslShtat += odv1.StaffCountOsobUslShtat.HasValue ? odv1.StaffCountOsobUslShtat.Value : 1;


                            db.ObjectStateManager.ChangeObjectState(odv1_old, EntityState.Modified);

                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new Action(() => { Methods.showAlert("Ошибка сохранения", "При обновлении записи Формы ОДВ-1 произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));
                        }

                        odv_id = odv1_old.ID;
                    }
                    if (updateODV_1_DDL.SelectedItem.Tag.ToString() == "1") // не загружать импортируемую форму
                    {
                        return false;
                    }
                }
                else // если такой записи нет то добавляем ее
                {
                    db.AddToFormsODV_1_2017(odv1);
                    db.SaveChanges();
                    odv_id = odv1.ID;
                }




                XElement СЗВ_СТАЖ = doc.Descendants().First(x => x.Name.LocalName == "СЗВ-СТАЖ");


                byte TypeInfo = 0;
                if (СЗВ_СТАЖ.Element("Тип") != null)
                {
                    byte.TryParse(СЗВ_СТАЖ.Element("Тип").Value.ToString(), out TypeInfo);
                }


                node = СЗВ_СТАЖ.Element("ОтчетныйПериод");
                y = 0;

                if (node.Element("Год") != null)
                {
                    short.TryParse(node.Element("Год").Value.ToString(), out y);
                }

                byte НачисленыНаОПС = 0;
                byte НачисленыПоДТ = 0;

                if (СЗВ_СТАЖ.Element("СВ") != null)
                {
                    if (СЗВ_СТАЖ.Element("СВ").Element("НачисленыНаОПС") != null)
                    {
                        byte.TryParse(СЗВ_СТАЖ.Element("СВ").Element("НачисленыНаОПС").Value.ToString(), out НачисленыНаОПС);
                    }
                    if (СЗВ_СТАЖ.Element("СВ").Element("НачисленыПоДТ") != null)
                    {
                        byte.TryParse(СЗВ_СТАЖ.Element("СВ").Element("НачисленыПоДТ").Value.ToString(), out НачисленыПоДТ);
                    }
                }




                #region СписокЗЛ

                int i = 0;

                var ЗЛ = СЗВ_СТАЖ.Descendants().Where(x => x.Name.LocalName == "ЗЛ");

                int count = ЗЛ.Count();

                List<staffImpCont> listStaff = new List<staffImpCont>();
                List<SNILSObject> listSnils = new List<SNILSObject>();

                foreach (var item in ЗЛ)
                {
                    string LastName = item.Element("ФИО") != null ? item.Element("ФИО").Element("Фамилия") != null ? item.Element("ФИО").Element("Фамилия").Value : "" : "";
                    string FirstName = item.Element("ФИО") != null ? item.Element("ФИО").Element("Имя") != null ? item.Element("ФИО").Element("Имя").Value : "" : "";
                    string MiddleName = item.Element("ФИО") != null ? item.Element("ФИО").Element("Отчество") != null ? item.Element("ФИО").Element("Отчество").Value : "" : "";

                    if (item.Element("СНИЛС") == null || (item.Element("СНИЛС") != null && String.IsNullOrEmpty(item.Element("СНИЛС").Value)))
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = i.ToString(), type = LastName + " " + FirstName + " " + MiddleName + ". Ошибка при загрузке Индивидуальных сведений. Не указан Страховой номер (СНИЛС) сотрудника.\r\n" });
                        continue;
                    }

                    var snils = Utils.ParseSNILS_XML(item.Element("СНИЛС").Value.ToString(), true);

                    if (!listStaff.Any(x => x.num == snils.num))
                    {
                        listStaff.Add(new staffImpCont
                        {
                            num = snils.num,
                            LastName = LastName,
                            FirstName = FirstName,
                            MiddleName = MiddleName,
                        });

                        listSnils.Add(new SNILSObject
                        {
                            contrNum = snils.contrNum,
                            num = snils.num
                        });
                    }


                }

                List<staffImpCont> listStaffDB = db.Staff.Where(x => x.InsurerID == insurer.ID).Select(x => new staffImpCont { FirstName = x.FirstName, LastName = x.LastName, MiddleName = x.MiddleName, num = x.InsuranceNumber}).ToList();

                var listStaffExists = listStaff.Where(x => listStaffDB.Select(c => c.num).Contains(x.num)).ToList();

                var listStaffNew = listStaff.Except(listStaffExists).ToList();

                if (updateStaffData.Checked)
                {
                    List<string> snilsList = listStaffExists.Select(x => x.num).ToList();

                    List<Staff> forEdit = db.Staff.Where(x => x.InsurerID == insurer.ID && snilsList.Contains(x.InsuranceNumber)).ToList();

                    i = 0;

                    foreach (var item in forEdit)
                    {
                        var tt = listStaffExists.First(x => x.num == item.InsuranceNumber);

                        bool changed = false;

                        if (item.LastName != tt.LastName)
                        {
                            item.LastName = tt.LastName;
                            changed = true;
                        }
                        if (item.FirstName != tt.FirstName)
                        {
                            item.FirstName = tt.FirstName;
                            changed = true;
                        }
                        if (item.MiddleName != tt.MiddleName)
                        {
                            item.MiddleName = tt.MiddleName;
                            changed = true;
                        }

                        if (changed)
                        {
                            db.ObjectStateManager.ChangeObjectState(item, EntityState.Modified);
                            i++;
                        }

                        if (i % 100 == 0)
                        {
                            db.SaveChanges();
                        }
                    }
                    db.SaveChanges();


                }

                i = 0;

                foreach (var item in listStaffNew)
                {
                    db.AddToStaff(new Staff {
                        InsurerID = insurer.ID,
                        InsuranceNumber = item.num,
                        ControlNumber = listSnils.First(x => x.num == item.num).contrNum,
                        LastName = item.LastName,
                        FirstName = item.FirstName,
                        MiddleName = item.MiddleName,
                        Dismissed = (byte)0

                    });

                    i++;
                    if (i % 200 == 0)
                    {
                        db.SaveChanges();
                    }
                }
                db.SaveChanges();

                i = 0;

                foreach (var item in ЗЛ)
                {
                    i++;

                    var snils = Utils.ParseSNILS_XML(item.Element("СНИЛС").Value.ToString(), true);

                    Staff staff = db.Staff.FirstOrDefault(x => x.InsurerID == insurer.ID && x.InsuranceNumber == snils.num);

                    // создаем запись СЗВ-СТАЖ
                    FormsSZV_STAJ_2017 szv_staj = new FormsSZV_STAJ_2017
                    {
                        FormsODV_1_2017_ID = odv_id,
                        InsurerID = insurer.ID,
                        TypeInfo = TypeInfo,
                        Code = 0,
                        Year = y,
                        StaffID = staff.ID,
                        OPSFeeNach = НачисленыНаОПС,
                        DopTarFeeNach = НачисленыПоДТ,
                        DateComposit = DateTime.Now,
                        DateFilling = datefill
                    };


                    bool szv_exist = db.FormsSZV_STAJ_2017.Any(x => x.FormsODV_1_2017_ID == szv_staj.FormsODV_1_2017_ID && x.TypeInfo == szv_staj.TypeInfo && x.Code == szv_staj.Code && x.Year == szv_staj.Year && x.StaffID == szv_staj.StaffID);

                    if (szv_exist && updateODV_1_DDL.SelectedItem.Tag.ToString() == "1") // не загружать импортируемую форму
                    {
                        continue;
                    }


                    List<FormsSZV_STAJ_4_2017> szv_staj_4_list = new List<FormsSZV_STAJ_4_2017>();

                    foreach (var Уплата in СЗВ_СТАЖ.Descendants().Where(x => x.Name.LocalName == "Уплата"))
                    {
                        try
                        {
                            FormsSZV_STAJ_4_2017 szv_staj_4 = new FormsSZV_STAJ_4_2017
                            {
                                DNPO_DateFrom = DateTime.Parse(Уплата.Element("Период").Element("С").Value.ToString()),
                                DNPO_DateTo = DateTime.Parse(Уплата.Element("Период").Element("По").Value.ToString()),
                                DNPO_Fee = Уплата.Element("Уплачено") != null ? (Уплата.Element("Уплачено").Value.ToString() == "1") : false
                            };

                            szv_staj_4_list.Add(szv_staj_4);
                        }
                        catch
                        {
                            continue;
                        }

                    }

                    foreach (var s4 in szv_staj_4_list)
                    {
                        szv_staj.FormsSZV_STAJ_4_2017.Add(s4);
                    }

                    szv_staj.Dismissed = false;
                    if (item.Element("ДатаУвольнения") != null)
                    {
                        if (!String.IsNullOrEmpty(item.Element("ДатаУвольнения").Value.ToString()))
                        {
                            szv_staj.Dismissed = true;
                        }
                    }

                    var СтажевыйПериод = item.Elements("СтажевыйПериод");


                    #region Записи о стаже

                    int n = 0;

                    List<StajOsn> stList = new List<StajOsn>();

                    foreach (var staj_osn in СтажевыйПериод)
                    {
                        DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("Период").Element("С").Value.ToString());
                        DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("Период").Element("По").Value.ToString());

                        n++;
                        StajOsn stajOsn = new StajOsn { DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };

                        if (staj_osn.Element("КатегорияЗЛ") != null)
                        {
                            if (staj_osn.Element("КатегорияЗЛ").Value != null && staj_osn.Element("КатегорияЗЛ").Value.ToString() == "БЕЗР")
                            {
                                stajOsn.CodeBEZR = true;
                            }
                            else
                                stajOsn.CodeBEZR = false;

                        }

                        //  FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID,
                        //                  db.StajOsn.AddObject(stajOsn);
                        //       db.SaveChanges();

                        var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");
                        //перебираем льготный стаж если есть
                        int ii = 0;
                        foreach (var itemL in staj_lgot_list)
                        {
                            string str = "";
                            ii++;
                            var staj_lgot = itemL;
                            StajLgot stajLgot = new StajLgot { Number = ii };
                            //StajOsnID = stajOsn.ID,
                            var terrUsl = staj_lgot.Element("ТУ");
                            if (terrUsl != null) // если есть терр условия
                            {
                                //если есть запись в с таким кодом терр условий в базе
                                str = terrUsl.Element("Основание").Value.ToString().ToUpper();
                                if (TerrUsl_list.Any(x => x.Code.ToUpper() == str))
                                {
                                    stajLgot.TerrUslID = TerrUsl_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    if (terrUsl.Element("Коэффициент") != null)
                                        stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                    else
                                        stajLgot.TerrUslKoef = 0;
                                }
                            }

                            var osobUsl = staj_lgot.Element("ОУТ");
                            if (osobUsl != null)
                            {
                                if (osobUsl.Element("Код") != null)
                                {
                                    str = osobUsl.Element("Код").Value.ToString().ToUpper();
                                    if (OsobUslTruda_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.OsobUslTrudaID = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    }
                                }
                                if (osobUsl.Element("ПозицияСписка") != null)
                                {
                                    str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                    if (KodVred_2_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == str);
                                        stajLgot.KodVred_OsnID = kv.ID;

                                        // проверка на наличие такой должности в базе
                                        if (db.Dolgn.Any(x => x.Name == kv.Name))
                                        {
                                            stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                        }
                                        else
                                        {
                                            Dolgn dolgn = new Dolgn { Name = kv.Name };
                                            db.AddToDolgn(dolgn);
                                            db.SaveChanges();
                                            stajLgot.DolgnID = dolgn.ID;
                                        }
                                    }
                                }
                            }

                            var ischislStrahStaj = staj_lgot.Element("ИС");
                            if (ischislStrahStaj != null)
                            {
                                if (ischislStrahStaj.Element("Основание") != null)
                                {
                                    str = ischislStrahStaj.Element("Основание").Value.ToString().ToUpper();
                                    if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    }
                                }
                                if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                {
                                    if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                    {
                                        stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                    }
                                    if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                    {
                                        stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                    }
                                }
                                if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                {
                                    if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                    {
                                        stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                    }
                                    if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                    {
                                        stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                    }
                                }
                            }

                            var ischislStrahStajDop = staj_lgot.Element("ДопСведенияИС");
                            if (ischislStrahStajDop != null)
                            {
                                str = ischislStrahStajDop.Value.ToString().ToUpper();
                                if (IschislStrahStajDop_list.Any(x => x.Code.ToUpper() == str))
                                {
                                    stajLgot.IschislStrahStajDopID = IschislStrahStajDop_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                }
                            }


                            var uslDosrNazn = staj_lgot.Element("ВЛ");
                            if (uslDosrNazn != null) // если есть терр условия
                            {
                                //если есть запись в с таким кодом терр условий в базе
                                if (uslDosrNazn.Element("Основание") != null)
                                {
                                    str = uslDosrNazn.Element("Основание").Value.ToString().ToUpper();
                                    if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    }
                                }
                                if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                {
                                    if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                    {
                                        stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                    }
                                    if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                    {
                                        stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                    }
                                }
                                if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                {
                                    if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                    {
                                        stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                    }
                                    if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                    {
                                        stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                    }
                                }
                                if (uslDosrNazn.Element("ДоляСтавки") != null)
                                {
                                    stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                }
                            }

                            stajOsn.StajLgot.Add(stajLgot);
                        }

                        stList.Add(stajOsn);
                    }



                    #endregion



                    // Если форма СЗВ-СТАЖ с такими параметрами уже есть в бд. 
                    if (szv_exist && updateODV_1_DDL.SelectedItem.Tag.ToString() == "2") // объединение
                    {
                        FormsSZV_STAJ_2017 szv_old = db.FormsSZV_STAJ_2017.FirstOrDefault(x => x.FormsODV_1_2017_ID == szv_staj.FormsODV_1_2017_ID && x.TypeInfo == szv_staj.TypeInfo && x.Code == szv_staj.Code && x.Year == szv_staj.Year && x.StaffID == szv_staj.StaffID);

                        bool ch = false;

                        if (updatePayFeeSZV_STAJ.Checked)
                        {
                            foreach (var s4 in szv_staj_4_list)
                            {
                                szv_old.FormsSZV_STAJ_4_2017.Add(s4);
                                ch = true;
                            }
                        }

                        if (updateStajSZV_STAJ.Checked)
                        {
                            foreach (var st in stList)
                            {
                                szv_old.StajOsn.Add(st);
                                ch = true;
                            }
                        }

                        if (ch)
                            db.ObjectStateManager.ChangeObjectState(szv_old, EntityState.Modified);
                    }
                    else
                    {
                        foreach (var st in stList)
                        {
                            szv_staj.StajOsn.Add(st);
                        }

                        db.AddToFormsSZV_STAJ_2017(szv_staj);
                    }


                    if (bw.CancellationPending)
                    {
                        db.SaveChanges();
                        return false;
                    }


                    if (transactionCheckBox.Checked)
                    {
                        if (i % 100 == 0)
                        {
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        db.SaveChanges();
                    }

                    cnt_records_imported++;
                    importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = i; }));

                    decimal temp = (decimal)i / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, i.ToString());

                }
                db.SaveChanges();



                #endregion

                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));

                result = false;
            }

            return result;
        }


        /// <summary>
        /// Импорт Файлов ПФР Формы ОДВ-1 СЗВ-ИСХ
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_SZV_ISH(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {
                node = doc.Descendants().First(x => x.Name.LocalName == "Страхователь");
                string regnum = node.Element("РегНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                string name = "";
                string inn = "";
                string kpp = "";
                byte type_ = (byte)0;

                if (node.Element("Наименование") != null)
                {
                    name = node.Element("Наименование").Value;
                }

                if (node.Element("КПП") != null)
                    kpp = node.Element("КПП").Value;
                if (node.Element("ИНН") != null)
                    inn = node.Element("ИНН").Value;

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (insurer.NameShort != name)
                        {
                            insurer.NameShort = name;
                            change = true;
                        }
                        if (String.IsNullOrEmpty(insurer.Name))
                        {
                            insurer.Name = name;
                            change = true;
                        }
                        if (insurer.RegNum != regnum)
                        {
                            insurer.RegNum = regnum;
                            change = true;
                        }
                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }

                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = type_;
                    insurer.NameShort = name;
                    insurer.Name = name;
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }

                XElement ОДВ1 = doc.Descendants().First(x => x.Name.LocalName == "ОДВ-1");

                FormsODV_1_2017 odv1 = new FormsODV_1_2017();
                odv1.InsurerID = insurer.ID;

                byte TypeInfoODV = 0;
                if (ОДВ1.Element("Тип") != null)
                {
                    byte.TryParse(ОДВ1.Element("Тип").Value.ToString(), out TypeInfoODV);
                }

                odv1.TypeInfo = TypeInfoODV;
                odv1.TypeForm = 2;

                node = ОДВ1.Element("ОтчетныйПериод");

                short y = 0;
                if (node.Element("Год") != null)
                {
                    short.TryParse(node.Element("Год").Value.ToString(), out y);
                }
                odv1.Year = y;

                byte Code = 0;
                if (node.Element("Код") != null)
                {
                    byte.TryParse(node.Element("Код").Value.ToString(), out Code);
                }
                odv1.Code = Code;


                long staffCount = 0;

                if (ОДВ1.Element("КоличествоЗЛ") != null)
                {
                    long.TryParse(ОДВ1.Element("КоличествоЗЛ").Value.ToString(), out staffCount);
                }
                odv1.StaffCount = staffCount;

                if (ОДВ1.Element("Руководитель") != null)
                {
                    node = ОДВ1.Element("Руководитель");
                    if (node.Element("Должность") != null)
                    {
                        odv1.ConfirmDolgn = node.Element("Должность").Value.ToString();
                    }
                    else
                        odv1.ConfirmDolgn = "";

                    if (node.Element("ФИО") != null)
                    {
                        node = node.Element("ФИО");
                        if (node.Element("Фамилия") != null)
                        {
                            odv1.ConfirmLastName = node.Element("Фамилия").Value.ToString();
                        }
                        else
                            odv1.ConfirmLastName = "";

                        if (node.Element("Имя") != null)
                        {
                            odv1.ConfirmFirstName = node.Element("Имя").Value.ToString();
                        }
                        else
                            odv1.ConfirmFirstName = "";

                        if (node.Element("Отчество") != null)
                        {
                            odv1.ConfirmMiddleName = node.Element("Отчество").Value.ToString();
                        }
                        else
                            odv1.ConfirmMiddleName = "";
                    }

                }

                DateTime datefill = DateTime.Now;

                if (ОДВ1.Element("ДатаЗаполнения") != null)
                {
                    DateTime.TryParse(ОДВ1.Element("ДатаЗаполнения").Value.ToString(), out datefill);
                }
                odv1.DateFilling = datefill;

                if (ОДВ1.Element("Страховая") != null)
                {
                    node = ОДВ1.Element("Страховая");

                    decimal sum = 0;
                    odv1.s_0_0 = node.Element("ЗадолженностьНаНачало") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаНачало").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_0_1 = node.Element("Начислено") != null ? (decimal.TryParse(node.Element("Начислено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_0_2 = node.Element("Уплачено") != null ? (decimal.TryParse(node.Element("Уплачено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_0_3 = node.Element("ЗадолженностьНаКонец") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаКонец").Value.ToString(), out sum) ? sum : 0) : 0;
                }
                else
                {
                    odv1.s_0_0 = 0;
                    odv1.s_0_1 = 0;
                    odv1.s_0_2 = 0;
                    odv1.s_0_3 = 0;
                }

                if (ОДВ1.Element("Накопительная") != null)
                {
                    node = ОДВ1.Element("Накопительная");

                    decimal sum = 0;
                    odv1.s_1_0 = node.Element("ЗадолженностьНаНачало") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаНачало").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_1_1 = node.Element("Начислено") != null ? (decimal.TryParse(node.Element("Начислено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_1_2 = node.Element("Уплачено") != null ? (decimal.TryParse(node.Element("Уплачено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_1_3 = node.Element("ЗадолженностьНаКонец") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаКонец").Value.ToString(), out sum) ? sum : 0) : 0;
                }
                else
                {
                    odv1.s_1_0 = 0;
                    odv1.s_1_1 = 0;
                    odv1.s_1_2 = 0;
                    odv1.s_1_3 = 0;
                }

                if (ОДВ1.Element("ТарифСВ") != null)
                {
                    node = ОДВ1.Element("ТарифСВ");

                    decimal sum = 0;
                    odv1.s_2_0 = node.Element("ЗадолженностьНаНачало") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаНачало").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_2_1 = node.Element("Начислено") != null ? (decimal.TryParse(node.Element("Начислено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_2_2 = node.Element("Уплачено") != null ? (decimal.TryParse(node.Element("Уплачено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_2_3 = node.Element("ЗадолженностьНаКонец") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаКонец").Value.ToString(), out sum) ? sum : 0) : 0;
                }
                else
                {
                    odv1.s_2_0 = 0;
                    odv1.s_2_1 = 0;
                    odv1.s_2_2 = 0;
                    odv1.s_2_3 = 0;
                }



                var ОснованияДНП = ОДВ1.Descendants().Where(x => x.Name.LocalName == "ОснованияДНП");
                foreach (var item in ОснованияДНП)
                {
                    var Основание = item.Elements("Основание");

                    foreach (var osn in Основание)
                    {
                        FormsODV_1_5_2017 odv1_5 = new FormsODV_1_5_2017();

                        if (osn.Element("Подразделение") != null)
                        {
                            odv1_5.Department = checkStringLength(osn.Element("Подразделение").Value.ToString(), 200);
                        }
                        else
                            odv1_5.Department = "";

                        if (osn.Element("ПрофессияДолжность") != null)
                        {
                            odv1_5.Profession = checkStringLength(osn.Element("ПрофессияДолжность").Value.ToString(), 200);
                        }
                        else
                            odv1_5.Profession = "";

                        if (osn.Element("Описание") != null)
                        {
                            odv1_5.VidRabotFakt = checkStringLength(osn.Element("Описание").Value.ToString(), 200);
                        }
                        else
                            odv1_5.VidRabotFakt = "";

                        if (osn.Element("Документы") != null)
                        {
                            odv1_5.DocsName = checkStringLength(osn.Element("Документы").Value.ToString(), 200);
                        }
                        else
                            odv1_5.DocsName = "";


                        long StaffCountShtat = 0;
                        long.TryParse(osn.Element("КоличествоШтат").Value.ToString(), out StaffCountShtat);
                        odv1_5.StaffCountShtat = StaffCountShtat;

                        long StaffCountFakt = 0;
                        long.TryParse(osn.Element("КоличествоФакт").Value.ToString(), out StaffCountFakt);
                        odv1_5.StaffCountFakt = StaffCountFakt;

                        foreach (var ОУТ in osn.Elements("ОУТ"))
                        {
                            FormsODV_1_5_2017_OUT newOUT = new FormsODV_1_5_2017_OUT();

                            newOUT.OsobUslTrudaCode = checkStringLength(ОУТ.Element("Код").Value.ToString().Trim(), 10);

                            if (ОУТ.Element("ПозицияСписка") != null)
                            {
                                newOUT.CodePosition = checkStringLength(ОУТ.Element("ПозицияСписка").Value.ToString(), 20);
                            }
                            odv1_5.FormsODV_1_5_2017_OUT.Add(newOUT);

                        }



                        odv1.FormsODV_1_5_2017.Add(odv1_5);
                    }


                    long StaffCountOsobUslShtat = 0;
                    long.TryParse(item.Element("ВсегоШтат").Value.ToString(), out StaffCountOsobUslShtat);
                    odv1.StaffCountOsobUslShtat = StaffCountOsobUslShtat;

                    long StaffCountOsobUslFakt = 0;
                    long.TryParse(item.Element("ВсегоФакт").Value.ToString(), out StaffCountOsobUslFakt);
                    odv1.StaffCountOsobUslFakt = StaffCountOsobUslFakt;

                }

                db.AddToFormsODV_1_2017(odv1);
                db.SaveChanges();




                //                var СЗВ_ИСХ = doc.Descendants().Where(x => x.Name.LocalName == "СЗВ-ИСХ");


                #region перебор инд.сведений

                var szv_ish_list = doc.Descendants().Where(x => x.Name.LocalName == "СЗВ-ИСХ");

                int count = szv_ish_list.Count();
                int k = 0;

                List<staffContainer> staffList = new List<staffContainer>();
                foreach (var szv_ish_item in szv_ish_list)
                {
                    string LastName = szv_ish_item.Element("ФИО") != null ? szv_ish_item.Element("ФИО").Element("Фамилия") != null ? szv_ish_item.Element("ФИО").Element("Фамилия").Value : "" : "";
                    string FirstName = szv_ish_item.Element("ФИО") != null ? szv_ish_item.Element("ФИО").Element("Имя") != null ? szv_ish_item.Element("ФИО").Element("Имя").Value : "" : "";
                    string MiddleName = szv_ish_item.Element("ФИО") != null ? szv_ish_item.Element("ФИО").Element("Отчество") != null ? szv_ish_item.Element("ФИО").Element("Отчество").Value : "" : "";

                    var snils = Utils.ParseSNILS_XML(szv_ish_item.Element("СНИЛС") != null ? szv_ish_item.Element("СНИЛС").Value.ToString() : "", true);


                    var st = new staffContainer
                    {
                        insuranceNum = snils.num,
                        insID = insurer.ID,
                        lastName = LastName,
                        firstName = FirstName,
                        middleName = MiddleName,
                        contrNum = snils.contrNum,
                        dismissed = (byte)0
                    };

                    if (!staffList.Any(x => x.insuranceNum == st.insuranceNum))
                        staffList.Add(st);
                }

                var listid = staffList.Select(yv => yv.insuranceNum).ToList();
                var listNum = db.Staff.Where(x => x.InsurerID == insurer.ID && listid.Contains(x.InsuranceNumber)).Select(x => x.InsuranceNumber).ToList();

                staffList = staffList.Where(x => !listNum.Contains(x.insuranceNum)).ToList();  // те сотрудники которых нет в базе для текущего страхователя

                foreach (var item in staffList)
                {
                    Staff staff_ = new Staff
                    {
                        InsuranceNumber = item.insuranceNum.PadLeft(9, '0'),
                        InsurerID = item.insID,
                        LastName = item.lastName,
                        FirstName = item.firstName,
                        MiddleName = item.middleName,
                        ControlNumber = item.contrNum,
                        Dismissed = item.dismissed
                    };

                    db.AddToStaff(staff_);
                }
                if (staffList.Count > 0)
                    db.SaveChanges();


                foreach (var szv_ish_item in szv_ish_list)
                {
                    try
                    {
                        //    l++;
                        string InsuranceNum = Utils.ParseSNILS_XML(szv_ish_item.Element("СНИЛС").Value.ToString(), false).num;

                        //var l__ = (lll.ToString() + l.ToString().PadLeft(3, '0')).PadLeft(9,'0');
                        Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);//

                        if (updateStaffData.Checked) // если выбрано обновлять данные в базе при расхождении
                        {
                            bool change = false;
                            if (staff.LastName != szv_ish_item.Element("ФИО").Element("Фамилия").Value.ToString())
                            {
                                staff.LastName = szv_ish_item.Element("ФИО").Element("Фамилия").Value.ToString();
                                change = true;
                            }
                            if (staff.FirstName != szv_ish_item.Element("ФИО").Element("Имя").Value.ToString())
                            {
                                staff.FirstName = szv_ish_item.Element("ФИО").Element("Имя").Value.ToString();
                                change = true;
                            }
                            if (szv_ish_item.Element("ФИО").Element("Отчество") != null)
                                if (staff.MiddleName != szv_ish_item.Element("ФИО").Element("Отчество").Value.ToString())
                                {
                                    staff.MiddleName = szv_ish_item.Element("ФИО").Element("Отчество").Value.ToString();
                                    change = true;
                                }

                            if (change)
                            {
                                db.ObjectStateManager.ChangeObjectState(staff, EntityState.Modified);
                                db.SaveChanges();
                            }
                        }


                        if (String.IsNullOrEmpty(InsuranceNum))
                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_ish_item.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке Индивидуальных сведений. Не указан Страховой номер (СНИЛС) сотрудника.\r\n" });


                        byte q = byte.Parse(szv_ish_item.Element("ОтчетныйПериод").Element("Код").Value.ToString());
                        short y_ = short.Parse(szv_ish_item.Element("ОтчетныйПериод").Element("Год").Value.ToString());


                        FormsSZV_ISH_2017 szv_ish = new FormsSZV_ISH_2017();

                        szv_ish.FormsODV_1_2017_ID = odv1.ID;
                        szv_ish.InsurerID = insurer.ID;
                        szv_ish.StaffID = staff.ID;
                        szv_ish.Year = y_;
                        szv_ish.Code = q;


                        byte ContractType = 0;
                        if (szv_ish_item.Element("Договор") != null)
                        {
                            byte.TryParse(szv_ish_item.Element("Договор").Value.ToString(), out ContractType);

                            if (szv_ish_item.Element("Реквизиты") != null)
                            {

                                if (szv_ish_item.Element("Реквизиты").Element("Номер") != null)
                                {
                                    szv_ish.ContractNum = szv_ish_item.Element("Реквизиты").Element("Номер").Value.ToString();
                                }
                                if (szv_ish_item.Element("Реквизиты").Element("Дата") != null)
                                {
                                    try
                                    {
                                        szv_ish.ContractDate = DateTime.Parse(szv_ish_item.Element("Реквизиты").Element("Дата").Value.ToString());
                                    }
                                    catch { }
                                }

                            }
                        }

                        szv_ish.ContractType = ContractType;


                        if (szv_ish_item.Element("КодДТ") != null)
                        {
                            szv_ish.DopTarCode = szv_ish_item.Element("КодДТ").Value.ToString();
                        }
                        else
                            szv_ish.DopTarCode = "";

                        szv_ish.DateFilling = DateTime.Now;


                        #region Раздел 6.4

                        if (szv_ish_item.Descendants().Any(x => x.Name.LocalName == "Выплаты"))
                        {
                            XElement Выплаты = szv_ish_item.Descendants().First(x => x.Name.LocalName == "Выплаты");

                            var razd_4_list = Выплаты.Descendants().Where(x => x.Name.LocalName == "Период");

                            foreach (var item in razd_4_list)
                            {
                                string platCatList = item.Descendants().First(x => x.Name.LocalName == "Категория").Value.ToString();


                                if (!PlatCatList.Any(x => x.Code == platCatList))
                                {
                                    if (String.IsNullOrEmpty(platCatList))
                                    {
                                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Не указан Код Категории Плательщика.\r\n" });
                                    }
                                    else
                                    {
                                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Указан Код Категории Плательщика - \"" + item + "\", который отсутствует в списке доступных категорий в справочнике.\r\n" });
                                    }
                                    break;
                                }

                                PlatCategory platCat = PlatCatList.FirstOrDefault(x => x.Code == platCatList);


                                FormsSZV_ISH_4_2017 szv_ish_4 = new FormsSZV_ISH_4_2017 { PlatCategoryID = platCat.ID };

                                string Month = item.Element("Месяц").Value != null ? item.Element("Месяц").Value.ToString() : "";

                                szv_ish_4.Month = 0;
                                if (MonthesList.Any(x => x == Month))
                                {
                                    byte mon = (byte)MonthesList.IndexOf(Month);
                                    mon++;
                                    szv_ish_4.Month = mon;
                                }

                                decimal sum = 0;
                                szv_ish_4.SumFeePFR = item.Element("СуммаВыплат") != null ? (decimal.TryParse(item.Element("СуммаВыплат").Value.ToString(), out sum) ? sum : 0) : 0;

                                if (item.Element("НеПревышающие") != null)
                                {
                                    XElement НеПревышающие = item.Element("НеПревышающие");

                                    sum = 0;
                                    szv_ish_4.BaseALL = НеПревышающие.Element("Всего") != null ? (decimal.TryParse(НеПревышающие.Element("Всего").Value.ToString(), out sum) ? sum : 0) : 0;
                                    sum = 0;
                                    szv_ish_4.BaseGPD = НеПревышающие.Element("ПоГПД") != null ? (decimal.TryParse(НеПревышающие.Element("ПоГПД").Value.ToString(), out sum) ? sum : 0) : 0;

                                }
                                else
                                {
                                    szv_ish_4.BaseALL = 0;
                                    szv_ish_4.BaseGPD = 0;
                                }

                                if (item.Element("Превышающие") != null)
                                {
                                    XElement Превышающие = item.Element("Превышающие");

                                    sum = 0;
                                    szv_ish_4.SumPrevBaseALL = Превышающие.Element("Всего") != null ? (decimal.TryParse(Превышающие.Element("Всего").Value.ToString(), out sum) ? sum : 0) : 0;
                                    sum = 0;
                                    szv_ish_4.SumPrevBaseGPD = Превышающие.Element("ПоГПД") != null ? (decimal.TryParse(Превышающие.Element("ПоГПД").Value.ToString(), out sum) ? sum : 0) : 0;

                                }
                                else
                                {
                                    szv_ish_4.SumPrevBaseALL = 0;
                                    szv_ish_4.SumPrevBaseGPD = 0;
                                }


                                szv_ish.FormsSZV_ISH_4_2017.Add(szv_ish_4);

                            }

                        }


                        #endregion

                        if (szv_ish_item.Element("Начисления") != null)
                        {
                            XElement Начисления = szv_ish_item.Element("Начисления");

                            decimal sum = 0;
                            szv_ish.SumFeePFR_Insurer = Начисления.Element("СВстрахователя") != null ? (decimal.TryParse(Начисления.Element("СВстрахователя").Value.ToString(), out sum) ? sum : 0) : 0;
                            sum = 0;
                            szv_ish.SumFeePFR_Staff = Начисления.Element("СВизЗаработка") != null ? (decimal.TryParse(Начисления.Element("СВизЗаработка").Value.ToString(), out sum) ? sum : 0) : 0;
                            sum = 0;
                            szv_ish.SumFeePFR_Tar = Начисления.Element("СВпоТарифу") != null ? (decimal.TryParse(Начисления.Element("СВпоТарифу").Value.ToString(), out sum) ? sum : 0) : 0;
                            sum = 0;
                            szv_ish.SumFeePFR_TarDop = Начисления.Element("СВпоДопТарифу") != null ? (decimal.TryParse(Начисления.Element("СВпоДопТарифу").Value.ToString(), out sum) ? sum : 0) : 0;
                            sum = 0;
                            szv_ish.SumFeePFR_Strah = Начисления.Element("Страховая") != null ? (decimal.TryParse(Начисления.Element("Страховая").Value.ToString(), out sum) ? sum : 0) : 0;
                            sum = 0;
                            szv_ish.SumFeePFR_Nakop = Начисления.Element("Накопительная") != null ? (decimal.TryParse(Начисления.Element("Накопительная").Value.ToString(), out sum) ? sum : 0) : 0;
                            sum = 0;
                            szv_ish.SumFeePFR_Base = Начисления.Element("СВпоТарифуНеПревышающие") != null ? (decimal.TryParse(Начисления.Element("СВпоТарифуНеПревышающие").Value.ToString(), out sum) ? sum : 0) : 0;

                        }
                        else
                        {
                            szv_ish.SumFeePFR_Insurer = 0;
                            szv_ish.SumFeePFR_Staff = 0;
                            szv_ish.SumFeePFR_Tar = 0;
                            szv_ish.SumFeePFR_TarDop = 0;
                            szv_ish.SumFeePFR_Strah = 0;
                            szv_ish.SumFeePFR_Nakop = 0;
                            szv_ish.SumFeePFR_Base = 0;
                        }



                        if (szv_ish_item.Element("Уплата") != null)
                        {
                            XElement Уплата = szv_ish_item.Element("Уплата");

                            decimal sum = 0;
                            szv_ish.SumPayPFR_Strah = Уплата.Element("Страховая") != null ? (decimal.TryParse(Уплата.Element("Страховая").Value.ToString(), out sum) ? sum : 0) : 0;
                            sum = 0;
                            szv_ish.SumPayPFR_Nakop = Уплата.Element("Накопительная") != null ? (decimal.TryParse(Уплата.Element("Накопительная").Value.ToString(), out sum) ? sum : 0) : 0;
                        }
                        else
                        {
                            szv_ish.SumPayPFR_Strah = 0;
                            szv_ish.SumPayPFR_Nakop = 0;

                        }


                        #region Раздел 7

                        if (szv_ish_item.Descendants().Any(x => x.Name.LocalName == "ВыплатыДТ"))
                        {
                            XElement ВыплатыДТ = szv_ish_item.Descendants().First(x => x.Name.LocalName == "ВыплатыДТ");

                            var razd_7_list = ВыплатыДТ.Descendants().Where(x => x.Name.LocalName == "Период");

                            foreach (var item in razd_7_list)
                            {
                                string КодСОУТ = item.Descendants().First(x => x.Name.LocalName == "КодСОУТ").Value.ToString();


                                if (!SpecOcenkaUslTruda_list.Any(x => x.Code == КодСОУТ))
                                {
                                    if (String.IsNullOrEmpty(КодСОУТ))
                                    {
                                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Не указан Код Категории Плательщика.\r\n" });
                                    }
                                    else
                                    {
                                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Указан Код Категории Плательщика - \"" + item + "\", который отсутствует в списке доступных категорий в справочнике.\r\n" });
                                    }
                                    break;
                                }

                                SpecOcenkaUslTruda SpecOc = SpecOcenkaUslTruda_list.FirstOrDefault(x => x.Code == КодСОУТ);


                                FormsSZV_ISH_7_2017 szv_ish_7 = new FormsSZV_ISH_7_2017 { SpecOcenkaUslTrudaID = SpecOc.ID };

                                string Month = item.Element("Месяц").Value != null ? item.Element("Месяц").Value.ToString() : "";

                                szv_ish_7.Month = 0;
                                if (MonthesList.Any(x => x == Month))
                                {
                                    byte mon = (byte)MonthesList.IndexOf(Month);
                                    mon++;
                                    szv_ish_7.Month = mon;
                                }

                                decimal sum = 0;
                                szv_ish_7.s_1_0 = item.Element("ДопТарифП1") != null ? (decimal.TryParse(item.Element("ДопТарифП1").Value.ToString(), out sum) ? sum : 0) : 0;

                                sum = 0;
                                szv_ish_7.s_1_1 = item.Element("ДопТарифП2_18") != null ? (decimal.TryParse(item.Element("ДопТарифП2_18").Value.ToString(), out sum) ? sum : 0) : 0;



                                szv_ish.FormsSZV_ISH_7_2017.Add(szv_ish_7);

                            }

                        }


                        #endregion


                        var СтажевыйПериод = szv_ish_item.Elements("СтажевыйПериод");


                        #region Записи о стаже

                        int n = 0;

                        foreach (var staj_osn in СтажевыйПериод)
                        {
                            DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("Период").Element("С").Value.ToString());
                            DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("Период").Element("По").Value.ToString());

                            n++;
                            StajOsn stajOsn = new StajOsn { DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };
                            //  FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID,
                            //                  db.StajOsn.AddObject(stajOsn);
                            //       db.SaveChanges();

                            var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");
                            //перебираем льготный стаж если есть
                            int ii = 0;
                            foreach (var itemL in staj_lgot_list)
                            {
                                string str = "";
                                ii++;
                                var staj_lgot = itemL;
                                StajLgot stajLgot = new StajLgot { Number = ii };
                                //StajOsnID = stajOsn.ID,
                                var terrUsl = staj_lgot.Element("ТУ");
                                if (terrUsl != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    str = terrUsl.Element("Основание").Value.ToString().ToUpper();
                                    if (TerrUsl_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.TerrUslID = TerrUsl_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (terrUsl.Element("Коэффициент") != null)
                                            stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                        else
                                            stajLgot.TerrUslKoef = 0;
                                    }
                                }

                                var osobUsl = staj_lgot.Element("ОУТ");
                                if (osobUsl != null)
                                {
                                    if (osobUsl.Element("Код") != null)
                                    {
                                        str = osobUsl.Element("Код").Value.ToString().ToUpper();
                                        if (OsobUslTruda_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.OsobUslTrudaID = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (osobUsl.Element("ПозицияСписка") != null)
                                    {
                                        str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                        if (KodVred_2_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == str);
                                            stajLgot.KodVred_OsnID = kv.ID;

                                            // проверка на наличие такой должности в базе
                                            if (db.Dolgn.Any(x => x.Name == kv.Name))
                                            {
                                                stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                            }
                                            else
                                            {
                                                Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                db.AddToDolgn(dolgn);
                                                db.SaveChanges();
                                                stajLgot.DolgnID = dolgn.ID;
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStaj = staj_lgot.Element("ИС");
                                if (ischislStrahStaj != null)
                                {
                                    if (ischislStrahStaj.Element("Основание") != null)
                                    {
                                        str = ischislStrahStaj.Element("Основание").Value.ToString().ToUpper();
                                        if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                    {
                                        if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                        {
                                            stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                        }
                                        if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                        {
                                            stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                        }
                                    }
                                    if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                    {
                                        if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                        {
                                            stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                        }
                                        if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                        {
                                            stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                        }
                                    }
                                }

                                var ischislStrahStajDop = staj_lgot.Element("ДопСведенияИС");
                                if (ischislStrahStajDop != null)
                                {
                                    str = ischislStrahStajDop.Value.ToString().ToUpper();
                                    if (IschislStrahStajDop_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajDopID = IschislStrahStajDop_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    }
                                }


                                var uslDosrNazn = staj_lgot.Element("ВЛ");
                                if (uslDosrNazn != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    if (uslDosrNazn.Element("Основание") != null)
                                    {
                                        str = uslDosrNazn.Element("Основание").Value.ToString().ToUpper();
                                        if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                    {
                                        if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                        {
                                            stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                        }
                                        if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                        {
                                            stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                        }
                                    }
                                    if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                    {
                                        if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                        {
                                            stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                        }
                                        if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                        {
                                            stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                        }
                                    }
                                    if (uslDosrNazn.Element("ДоляСтавки") != null)
                                    {
                                        stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                    }
                                }

                                stajOsn.StajLgot.Add(stajLgot);
                            }
                            szv_ish.StajOsn.Add(stajOsn);


                        }



                        #endregion

                        db.AddToFormsSZV_ISH_2017(szv_ish);
                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        if (transactionCheckBox.Checked)
                        {
                            if (cnt_records_imported == 50 || cnt_records_imported == 100 || cnt_records_imported == 150)
                                db.SaveChanges();
                        }
                        else
                            db.SaveChanges();

                        cnt_records_imported++;
                        importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));





                    }
                    catch (Exception ex)
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_ish_item.Element("НомерВпачке").Value.ToString(), type = ex.Message + "\r\n" });
                    }

                    k++;


                    decimal temp = (decimal)k / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, k.ToString());

                }



                #endregion




                db.SaveChanges();



                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));

                result = false;
            }

            return result;
        }


        /// <summary>
        /// Импорт Файлов ПФР Формы ОДВ-1 СЗВ-КОРР
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_SZV_KORR(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {
                node = doc.Descendants().First(x => x.Name.LocalName == "Страхователь");
                string regnum = node.Element("РегНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                string name = "";
                string inn = "";
                string kpp = "";
                byte type_ = (byte)0;

                if (node.Element("Наименование") != null)
                {
                    name = node.Element("Наименование").Value;
                }

                if (node.Element("КПП") != null)
                    kpp = node.Element("КПП").Value;
                if (node.Element("ИНН") != null)
                    inn = node.Element("ИНН").Value;

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (insurer.NameShort != name)
                        {
                            insurer.NameShort = name;
                            change = true;
                        }
                        if (String.IsNullOrEmpty(insurer.Name))
                        {
                            insurer.Name = name;
                            change = true;
                        }
                        if (insurer.RegNum != regnum)
                        {
                            insurer.RegNum = regnum;
                            change = true;
                        }
                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }

                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = type_;
                    insurer.NameShort = name;
                    insurer.Name = name;
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }

                XElement ОДВ1 = doc.Descendants().First(x => x.Name.LocalName == "ОДВ-1");

                FormsODV_1_2017 odv1 = new FormsODV_1_2017();
                odv1.InsurerID = insurer.ID;

                byte TypeInfoODV = 0;
                if (ОДВ1.Element("Тип") != null)
                {
                    byte.TryParse(ОДВ1.Element("Тип").Value.ToString(), out TypeInfoODV);
                }

                odv1.TypeInfo = TypeInfoODV;
                odv1.TypeForm = 3;

                node = ОДВ1.Element("ОтчетныйПериод");

                short y = 0;
                if (node.Element("Год") != null)
                {
                    short.TryParse(node.Element("Год").Value.ToString(), out y);
                }
                odv1.Year = y;

                byte Code = 0;
                if (node.Element("Код") != null)
                {
                    byte.TryParse(node.Element("Код").Value.ToString(), out Code);
                }
                odv1.Code = Code;


                long staffCount = 0;

                if (ОДВ1.Element("КоличествоЗЛ") != null)
                {
                    long.TryParse(ОДВ1.Element("КоличествоЗЛ").Value.ToString(), out staffCount);
                }
                odv1.StaffCount = staffCount;

                if (ОДВ1.Element("Руководитель") != null)
                {
                    node = ОДВ1.Element("Руководитель");
                    if (node.Element("Должность") != null)
                    {
                        odv1.ConfirmDolgn = node.Element("Должность").Value.ToString();
                    }
                    else
                        odv1.ConfirmDolgn = "";

                    if (node.Element("ФИО") != null)
                    {
                        node = node.Element("ФИО");
                        if (node.Element("Фамилия") != null)
                        {
                            odv1.ConfirmLastName = node.Element("Фамилия").Value.ToString();
                        }
                        else
                            odv1.ConfirmLastName = "";

                        if (node.Element("Имя") != null)
                        {
                            odv1.ConfirmFirstName = node.Element("Имя").Value.ToString();
                        }
                        else
                            odv1.ConfirmFirstName = "";

                        if (node.Element("Отчество") != null)
                        {
                            odv1.ConfirmMiddleName = node.Element("Отчество").Value.ToString();
                        }
                        else
                            odv1.ConfirmMiddleName = "";
                    }

                }

                DateTime datefill = DateTime.Now;

                if (ОДВ1.Element("ДатаЗаполнения") != null)
                {
                    DateTime.TryParse(ОДВ1.Element("ДатаЗаполнения").Value.ToString(), out datefill);
                }
                odv1.DateFilling = datefill;

                if (ОДВ1.Element("Страховая") != null)
                {
                    node = ОДВ1.Element("Страховая");

                    decimal sum = 0;
                    odv1.s_0_0 = node.Element("ЗадолженностьНаНачало") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаНачало").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_0_1 = node.Element("Начислено") != null ? (decimal.TryParse(node.Element("Начислено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_0_2 = node.Element("Уплачено") != null ? (decimal.TryParse(node.Element("Уплачено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_0_3 = node.Element("ЗадолженностьНаКонец") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаКонец").Value.ToString(), out sum) ? sum : 0) : 0;
                }
                else
                {
                    odv1.s_0_0 = 0;
                    odv1.s_0_1 = 0;
                    odv1.s_0_2 = 0;
                    odv1.s_0_3 = 0;
                }

                if (ОДВ1.Element("Накопительная") != null)
                {
                    node = ОДВ1.Element("Накопительная");

                    decimal sum = 0;
                    odv1.s_1_0 = node.Element("ЗадолженностьНаНачало") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаНачало").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_1_1 = node.Element("Начислено") != null ? (decimal.TryParse(node.Element("Начислено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_1_2 = node.Element("Уплачено") != null ? (decimal.TryParse(node.Element("Уплачено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_1_3 = node.Element("ЗадолженностьНаКонец") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаКонец").Value.ToString(), out sum) ? sum : 0) : 0;
                }
                else
                {
                    odv1.s_1_0 = 0;
                    odv1.s_1_1 = 0;
                    odv1.s_1_2 = 0;
                    odv1.s_1_3 = 0;
                }

                if (ОДВ1.Element("ТарифСВ") != null)
                {
                    node = ОДВ1.Element("ТарифСВ");

                    decimal sum = 0;
                    odv1.s_2_0 = node.Element("ЗадолженностьНаНачало") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаНачало").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_2_1 = node.Element("Начислено") != null ? (decimal.TryParse(node.Element("Начислено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_2_2 = node.Element("Уплачено") != null ? (decimal.TryParse(node.Element("Уплачено").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1.s_2_3 = node.Element("ЗадолженностьНаКонец") != null ? (decimal.TryParse(node.Element("ЗадолженностьНаКонец").Value.ToString(), out sum) ? sum : 0) : 0;
                }
                else
                {
                    odv1.s_2_0 = 0;
                    odv1.s_2_1 = 0;
                    odv1.s_2_2 = 0;
                    odv1.s_2_3 = 0;
                }

                var Уплата = ОДВ1.Descendants().Where(x => x.Name.LocalName == "Уплата");
                foreach (var item in Уплата)
                {
                    FormsODV_1_4_2017 odv1_4 = new FormsODV_1_4_2017();

                    y = 0;
                    if (item.Element("Год") != null)
                    {
                        short.TryParse(item.Element("Год").Value.ToString(), out y);
                    }
                    odv1_4.Year = y;


                    decimal sum = 0;
                    odv1_4.OPS = item.Element("Страховая") != null ? (decimal.TryParse(item.Element("Страховая").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1_4.NAKOP = item.Element("Накопительная") != null ? (decimal.TryParse(item.Element("Накопительная").Value.ToString(), out sum) ? sum : 0) : 0;
                    sum = 0;
                    odv1_4.DopTar = item.Element("ТарифСВ") != null ? (decimal.TryParse(item.Element("ТарифСВ").Value.ToString(), out sum) ? sum : 0) : 0;

                }


                var ОснованияДНП = ОДВ1.Descendants().Where(x => x.Name.LocalName == "ОснованияДНП");
                foreach (var item in ОснованияДНП)
                {
                    var Основание = item.Elements("Основание");

                    foreach (var osn in Основание)
                    {
                        FormsODV_1_5_2017 odv1_5 = new FormsODV_1_5_2017();

                        if (osn.Element("Подразделение") != null)
                        {
                            odv1_5.Department = checkStringLength(osn.Element("Подразделение").Value.ToString(), 200);
                        }
                        else
                            odv1_5.Department = "";

                        if (osn.Element("ПрофессияДолжность") != null)
                        {
                            odv1_5.Profession = checkStringLength(osn.Element("ПрофессияДолжность").Value.ToString(), 200);
                        }
                        else
                            odv1_5.Profession = "";

                        if (osn.Element("Описание") != null)
                        {
                            odv1_5.VidRabotFakt = checkStringLength(osn.Element("Описание").Value.ToString(), 200);
                        }
                        else
                            odv1_5.VidRabotFakt = "";

                        if (osn.Element("Документы") != null)
                        {
                            odv1_5.DocsName = checkStringLength(osn.Element("Документы").Value.ToString(), 200);
                        }
                        else
                            odv1_5.DocsName = "";


                        long StaffCountShtat = 0;
                        long.TryParse(osn.Element("КоличествоШтат").Value.ToString(), out StaffCountShtat);
                        odv1_5.StaffCountShtat = StaffCountShtat;

                        long StaffCountFakt = 0;
                        long.TryParse(osn.Element("КоличествоФакт").Value.ToString(), out StaffCountFakt);
                        odv1_5.StaffCountFakt = StaffCountFakt;

                        foreach (var ОУТ in osn.Elements("ОУТ"))
                        {
                            FormsODV_1_5_2017_OUT newOUT = new FormsODV_1_5_2017_OUT();

                            newOUT.OsobUslTrudaCode = checkStringLength(ОУТ.Element("Код").Value.ToString().Trim(), 10);

                            if (ОУТ.Element("ПозицияСписка") != null)
                            {
                                newOUT.CodePosition = checkStringLength(ОУТ.Element("ПозицияСписка").Value.ToString(), 20);
                            }
                            odv1_5.FormsODV_1_5_2017_OUT.Add(newOUT);

                        }



                        odv1.FormsODV_1_5_2017.Add(odv1_5);
                    }


                    long StaffCountOsobUslShtat = 0;
                    long.TryParse(item.Element("ВсегоШтат").Value.ToString(), out StaffCountOsobUslShtat);
                    odv1.StaffCountOsobUslShtat = StaffCountOsobUslShtat;

                    long StaffCountOsobUslFakt = 0;
                    long.TryParse(item.Element("ВсегоФакт").Value.ToString(), out StaffCountOsobUslFakt);
                    odv1.StaffCountOsobUslFakt = StaffCountOsobUslFakt;

                }

                db.AddToFormsODV_1_2017(odv1);
                db.SaveChanges();




                //                var СЗВ_ИСХ = doc.Descendants().Where(x => x.Name.LocalName == "СЗВ-ИСХ");


                #region перебор инд.сведений

                var szv_korr_list = doc.Descendants().Where(x => x.Name.LocalName == "СЗВ-КОРР");

                int count = szv_korr_list.Count();
                int k = 0;

                List<staffContainer> staffList = new List<staffContainer>();
                foreach (var szv_korr_item in szv_korr_list)
                {
                    XElement ЗЛ = szv_korr_item.Element("ЗЛ");

                    string LastName = ЗЛ.Element("ФИО") != null ? ЗЛ.Element("ФИО").Element("Фамилия") != null ? ЗЛ.Element("ФИО").Element("Фамилия").Value : "" : "";
                    string FirstName = ЗЛ.Element("ФИО") != null ? ЗЛ.Element("ФИО").Element("Имя") != null ? ЗЛ.Element("ФИО").Element("Имя").Value : "" : "";
                    string MiddleName = ЗЛ.Element("ФИО") != null ? ЗЛ.Element("ФИО").Element("Отчество") != null ? ЗЛ.Element("ФИО").Element("Отчество").Value : "" : "";

                    var snils = Utils.ParseSNILS_XML(ЗЛ.Element("СНИЛС") != null ? ЗЛ.Element("СНИЛС").Value.ToString() : "", true);


                    var st = new staffContainer
                    {
                        insuranceNum = snils.num,
                        insID = insurer.ID,
                        lastName = LastName,
                        firstName = FirstName,
                        middleName = MiddleName,
                        contrNum = snils.contrNum,
                        dismissed = (byte)0
                    };

                    if (!staffList.Any(x => x.insuranceNum == st.insuranceNum))
                        staffList.Add(st);
                }

                var listid = staffList.Select(yv => yv.insuranceNum).ToList();
                var listNum = db.Staff.Where(x => x.InsurerID == insurer.ID && listid.Contains(x.InsuranceNumber)).Select(x => x.InsuranceNumber).ToList();

                staffList = staffList.Where(x => !listNum.Contains(x.insuranceNum)).ToList();  // те сотрудники которых нет в базе для текущего страхователя

                foreach (var item in staffList)
                {
                    Staff staff_ = new Staff
                    {
                        InsuranceNumber = item.insuranceNum.PadLeft(9, '0'),
                        InsurerID = item.insID,
                        LastName = item.lastName,
                        FirstName = item.firstName,
                        MiddleName = item.middleName,
                        ControlNumber = item.contrNum,
                        Dismissed = item.dismissed
                    };

                    db.AddToStaff(staff_);
                }
                if (staffList.Count > 0)
                    db.SaveChanges();


                foreach (var szv_korr_item in szv_korr_list)
                {
                    try
                    {
                        FormsSZV_KORR_2017 szv_korr = new FormsSZV_KORR_2017();
                        szv_korr.FormsODV_1_2017_ID = odv1.ID;
                        szv_korr.InsurerID = insurer.ID;

                        byte q = byte.Parse(szv_korr_item.Element("ОтчетныйПериод").Element("Код").Value.ToString());
                        short y_ = short.Parse(szv_korr_item.Element("ОтчетныйПериод").Element("Год").Value.ToString());


                        szv_korr.Year = y_;
                        szv_korr.Code = q;

                        byte TypeInfo = 0;
                        if (szv_korr_item.Element("Тип") != null)
                        {
                            byte.TryParse(szv_korr_item.Element("Тип").Value.ToString(), out TypeInfo);
                        }

                        szv_korr.TypeInfo = TypeInfo;

                        XElement КорректируемыйПериод = szv_korr_item.Element("КорректируемыйПериод");

                        q = 0;
                        y_ = 0;

                        if (КорректируемыйПериод.Element("ОтчетныйПериод") != null)
                        {
                            if (КорректируемыйПериод.Element("ОтчетныйПериод").Element("Код") != null)
                            {
                                byte.TryParse(КорректируемыйПериод.Element("ОтчетныйПериод").Element("Код").Value.ToString(), out q);
                            }

                            if (КорректируемыйПериод.Element("ОтчетныйПериод").Element("Год") != null)
                            {
                                short.TryParse(КорректируемыйПериод.Element("ОтчетныйПериод").Element("Год").Value.ToString(), out y_);
                            }

                        }
                        szv_korr.CodeKorr = q;
                        szv_korr.YearKorr = y_;


                        node = КорректируемыйПериод.Element("Страхователь");
                        regnum = node.Element("РегНомер").Value;

                        while (regnum.Contains("-"))
                            regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                        name = "";
                        inn = "";
                        kpp = "";

                        if (node.Element("Наименование") != null)
                        {
                            name = node.Element("Наименование").Value;
                        }

                        if (node.Element("КПП") != null)
                            kpp = node.Element("КПП").Value;
                        if (node.Element("ИНН") != null)
                            inn = node.Element("ИНН").Value;

                        szv_korr.RegNumKorr = regnum;
                        szv_korr.INNKorr = inn;
                        szv_korr.KPPKorr = kpp;
                        szv_korr.ShortNameKorr = name;



                        XElement ЗЛ = szv_korr_item.Element("ЗЛ");

                        string InsuranceNum = Utils.ParseSNILS_XML(ЗЛ.Element("СНИЛС").Value.ToString(), false).num;

                        Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);//

                        if (updateStaffData.Checked) // если выбрано обновлять данные в базе при расхождении
                        {
                            bool change = false;
                            if (staff.LastName != ЗЛ.Element("ФИО").Element("Фамилия").Value.ToString())
                            {
                                staff.LastName = ЗЛ.Element("ФИО").Element("Фамилия").Value.ToString();
                                change = true;
                            }
                            if (staff.FirstName != ЗЛ.Element("ФИО").Element("Имя").Value.ToString())
                            {
                                staff.FirstName = ЗЛ.Element("ФИО").Element("Имя").Value.ToString();
                                change = true;
                            }
                            if (ЗЛ.Element("ФИО").Element("Отчество") != null)
                                if (staff.MiddleName != ЗЛ.Element("ФИО").Element("Отчество").Value.ToString())
                                {
                                    staff.MiddleName = ЗЛ.Element("ФИО").Element("Отчество").Value.ToString();
                                    change = true;
                                }

                            if (change)
                            {
                                db.ObjectStateManager.ChangeObjectState(staff, EntityState.Modified);
                                db.SaveChanges();
                            }
                        }


                        if (String.IsNullOrEmpty(InsuranceNum))
                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_korr_item.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке Индивидуальных сведений. Не указан Страховой номер (СНИЛС) сотрудника.\r\n" });


                        szv_korr.StaffID = staff.ID;


                        if (szv_korr_item.Element("ДанныеЗЛ") != null)
                        {
                            XElement ДанныеЗЛ = szv_korr_item.Element("ДанныеЗЛ");

                            if (ДанныеЗЛ.Element("Категория") != null)
                            {
                                string platCatList = ДанныеЗЛ.Element("Категория").Value != null ? ДанныеЗЛ.Element("Категория").Value.ToString() : "";

                                if (!PlatCatList.Any(x => x.Code == platCatList))
                                {
                                    if (String.IsNullOrEmpty(platCatList))
                                    {
                                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных. Не указан Код Категории Плательщика.\r\n" });
                                    }
                                    else
                                    {
                                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных. Указан Код Категории Плательщика - \"" + platCatList + "\", который отсутствует в списке доступных категорий в справочнике.\r\n" });
                                    }
                                    break;
                                }

                                PlatCategory platCat = PlatCatList.FirstOrDefault(x => x.Code == platCatList);

                                szv_korr.PlatCategoryID = platCat.ID;
                            }



                            byte ContractType = 0;
                            if (ДанныеЗЛ.Element("Договор") != null)
                            {
                                byte.TryParse(ДанныеЗЛ.Element("Договор").Value.ToString(), out ContractType);

                                if (ДанныеЗЛ.Element("Реквизиты") != null)
                                {

                                    if (ДанныеЗЛ.Element("Реквизиты").Element("Номер") != null)
                                    {
                                        szv_korr.ContractNum = ДанныеЗЛ.Element("Реквизиты").Element("Номер").Value.ToString();
                                    }
                                    if (ДанныеЗЛ.Element("Реквизиты").Element("Дата") != null)
                                    {
                                        try
                                        {
                                            szv_korr.ContractDate = DateTime.Parse(ДанныеЗЛ.Element("Реквизиты").Element("Дата").Value.ToString());
                                        }
                                        catch { }
                                    }

                                }
                            }

                            szv_korr.ContractType = ContractType;


                            if (ДанныеЗЛ.Element("КодДТ") != null)
                            {
                                szv_korr.DopTarCode = ДанныеЗЛ.Element("КодДТ").Value.ToString();
                            }
                            else
                                szv_korr.DopTarCode = "";
                        }



                        szv_korr.DateFilling = DateTime.Now;


                        #region Раздел 4

                        foreach (var item in szv_korr_item.Descendants().Where(x => x.Name.LocalName == "Суммы"))
                        {
                            FormsSZV_KORR_4_2017 szv_korr_4 = new FormsSZV_KORR_4_2017();

                            string Month = item.Element("Месяц").Value != null ? item.Element("Месяц").Value.ToString() : "";

                            szv_korr_4.Month = 0;
                            if (MonthesList.Any(x => x == Month))
                            {
                                byte mon = (byte)MonthesList.IndexOf(Month);
                                mon++;
                                szv_korr_4.Month = mon;
                            }

                            decimal sum0 = 0;
                            decimal sum1 = 0;
                            decimal sum2 = 0;
                            decimal sum3 = 0;
                            decimal sum4 = 0;

                            if (item.Element("Выплаты") != null)
                            {
                                XElement Выплаты = item.Element("Выплаты");
                                szv_korr_4.SumFeePFR = Выплаты.Element("СуммаВыплат") != null ? (decimal.TryParse(Выплаты.Element("СуммаВыплат").Value.ToString(), out sum0) ? sum0 : 0) : 0;

                                if (Выплаты.Element("НеПревышающие") != null)
                                {
                                    XElement НеПревышающие = Выплаты.Element("НеПревышающие");
                                    szv_korr_4.BaseALL = НеПревышающие.Element("Всего") != null ? (decimal.TryParse(НеПревышающие.Element("Всего").Value.ToString(), out sum1) ? sum1 : 0) : 0;
                                    szv_korr_4.BaseGPD = НеПревышающие.Element("ПоГПД") != null ? (decimal.TryParse(НеПревышающие.Element("ПоГПД").Value.ToString(), out sum2) ? sum2 : 0) : 0;
                                }
                                else
                                {
                                    szv_korr_4.BaseALL = sum1;
                                    szv_korr_4.BaseGPD = sum2;
                                }

                                if (Выплаты.Element("Превышающие") != null)
                                {
                                    XElement Превышающие = Выплаты.Element("Превышающие");
                                    szv_korr_4.SumPrevBaseALL = Превышающие.Element("Всего") != null ? (decimal.TryParse(Превышающие.Element("Всего").Value.ToString(), out sum3) ? sum3 : 0) : 0;
                                    szv_korr_4.SumPrevBaseGPD = Превышающие.Element("ПоГПД") != null ? (decimal.TryParse(Превышающие.Element("ПоГПД").Value.ToString(), out sum4) ? sum4 : 0) : 0;
                                }
                                else
                                {
                                    szv_korr_4.SumPrevBaseALL = sum3;
                                    szv_korr_4.SumPrevBaseGPD = sum4;
                                }

                            }
                            else
                            {
                                szv_korr_4.SumFeePFR = sum0;
                                szv_korr_4.BaseALL = sum1;
                                szv_korr_4.BaseGPD = sum2;
                                szv_korr_4.SumPrevBaseALL = sum3;
                                szv_korr_4.SumPrevBaseGPD = sum4;
                            }


                            decimal sum = 0;

                            if (item.Element("ДоначисленоСВ") != null)
                            {
                                XElement ДоначисленоСВ = item.Element("ДоначисленоСВ");
                                szv_korr_4.SumFeeBefore2001Insurer = ДоначисленоСВ.Element("СВстрахователя") != null ? (decimal.TryParse(ДоначисленоСВ.Element("СВстрахователя").Value.ToString(), out sum) ? sum : 0) : 0;
                                sum = 0;
                                szv_korr_4.SumFeeBefore2001Staff = ДоначисленоСВ.Element("СВизЗаработка") != null ? (decimal.TryParse(ДоначисленоСВ.Element("СВизЗаработка").Value.ToString(), out sum) ? sum : 0) : 0;
                                sum = 0;
                                szv_korr_4.SumFeeAfter2001STRAH = ДоначисленоСВ.Element("Страховая") != null ? (decimal.TryParse(ДоначисленоСВ.Element("Страховая").Value.ToString(), out sum) ? sum : 0) : 0;
                                sum = 0;
                                szv_korr_4.SumFeeAfter2001NAKOP = ДоначисленоСВ.Element("Накопительная") != null ? (decimal.TryParse(ДоначисленоСВ.Element("Накопительная").Value.ToString(), out sum) ? sum : 0) : 0;
                                sum = 0;
                                szv_korr_4.SumFeeTarSV = ДоначисленоСВ.Element("СВпоТарифу") != null ? (decimal.TryParse(ДоначисленоСВ.Element("СВпоТарифу").Value.ToString(), out sum) ? sum : 0) : 0;

                            }
                            else
                            {
                                szv_korr_4.SumFeeBefore2001Insurer = sum;
                                szv_korr_4.SumFeeBefore2001Staff = sum;
                                szv_korr_4.SumFeeAfter2001STRAH = sum;
                                szv_korr_4.SumFeeAfter2001NAKOP = sum;
                                szv_korr_4.SumFeeTarSV = sum;
                            }


                            sum = 0;

                            if (item.Element("Уплата") != null)
                            {
                                XElement Уплата2 = item.Element("Уплата");
                                szv_korr_4.SumPaySTRAH = Уплата2.Element("Страховая") != null ? (decimal.TryParse(Уплата2.Element("Страховая").Value.ToString(), out sum) ? sum : 0) : 0;
                                sum = 0;
                                szv_korr_4.SumPayNAKOP = Уплата2.Element("Накопительная") != null ? (decimal.TryParse(Уплата2.Element("Накопительная").Value.ToString(), out sum) ? sum : 0) : 0;

                            }
                            else
                            {
                                szv_korr_4.SumPaySTRAH = sum;
                                szv_korr_4.SumPayNAKOP = sum;
                            }


                            szv_korr.FormsSZV_KORR_4_2017.Add(szv_korr_4);



                        }


                        #endregion

                        #region Раздел 5


                        foreach (var item in szv_korr_item.Descendants().Where(x => x.Name.LocalName == "ВыплатыДТ"))
                        {
                            string КодСОУТ = item.Descendants().First(x => x.Name.LocalName == "КодСОУТ").Value.ToString();


                            if (!SpecOcenkaUslTruda_list.Any(x => x.Code == КодСОУТ))
                            {
                                if (String.IsNullOrEmpty(КодСОУТ))
                                {
                                    errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Не указан Код Категории Плательщика.\r\n" });
                                }
                                else
                                {
                                    errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Указан Код Категории Плательщика - \"" + item + "\", который отсутствует в списке доступных категорий в справочнике.\r\n" });
                                }
                                break;
                            }

                            SpecOcenkaUslTruda SpecOc = SpecOcenkaUslTruda_list.FirstOrDefault(x => x.Code == КодСОУТ);


                            FormsSZV_KORR_5_2017 szv_korr_5 = new FormsSZV_KORR_5_2017 { SpecOcenkaUslTrudaID = SpecOc.ID };

                            string Month = item.Element("Месяц").Value != null ? item.Element("Месяц").Value.ToString() : "";

                            szv_korr_5.Month = 0;
                            if (MonthesList.Any(x => x == Month))
                            {
                                byte mon = (byte)MonthesList.IndexOf(Month);
                                mon++;
                                szv_korr_5.Month = mon;
                            }

                            decimal sum = 0;
                            szv_korr_5.s_0 = item.Element("ДопТарифП1") != null ? (decimal.TryParse(item.Element("ДопТарифП1").Value.ToString(), out sum) ? sum : 0) : 0;

                            sum = 0;
                            szv_korr_5.s_1 = item.Element("ДопТарифП2_18") != null ? (decimal.TryParse(item.Element("ДопТарифП2_18").Value.ToString(), out sum) ? sum : 0) : 0;



                            szv_korr.FormsSZV_KORR_5_2017.Add(szv_korr_5);

                        }



                        #endregion


                        var СтажевыйПериод = szv_korr_item.Elements("СтажевыйПериод");


                        #region Записи о стаже

                        int n = 0;

                        foreach (var staj_osn in СтажевыйПериод)
                        {
                            DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("Период").Element("С").Value.ToString());
                            DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("Период").Element("По").Value.ToString());

                            n++;
                            StajOsn stajOsn = new StajOsn { DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };
                            //  FormsRSW2014_1_Razd_6_1_ID = rsw_6_1.ID,
                            //                  db.StajOsn.AddObject(stajOsn);
                            //       db.SaveChanges();

                            var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");
                            //перебираем льготный стаж если есть
                            int ii = 0;
                            foreach (var itemL in staj_lgot_list)
                            {
                                string str = "";
                                ii++;
                                var staj_lgot = itemL;
                                StajLgot stajLgot = new StajLgot { Number = ii };
                                //StajOsnID = stajOsn.ID,
                                var terrUsl = staj_lgot.Element("ТУ");
                                if (terrUsl != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    str = terrUsl.Element("Основание").Value.ToString().ToUpper();
                                    if (TerrUsl_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.TerrUslID = TerrUsl_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        if (terrUsl.Element("Коэффициент") != null)
                                            stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                        else
                                            stajLgot.TerrUslKoef = 0;
                                    }
                                }

                                var osobUsl = staj_lgot.Element("ОУТ");
                                if (osobUsl != null)
                                {
                                    if (osobUsl.Element("Код") != null)
                                    {
                                        str = osobUsl.Element("Код").Value.ToString().ToUpper();
                                        if (OsobUslTruda_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.OsobUslTrudaID = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (osobUsl.Element("ПозицияСписка") != null)
                                    {
                                        str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                        if (KodVred_2_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == str);
                                            stajLgot.KodVred_OsnID = kv.ID;

                                            // проверка на наличие такой должности в базе
                                            if (db.Dolgn.Any(x => x.Name == kv.Name))
                                            {
                                                stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                            }
                                            else
                                            {
                                                Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                db.AddToDolgn(dolgn);
                                                db.SaveChanges();
                                                stajLgot.DolgnID = dolgn.ID;
                                            }
                                        }
                                    }
                                }

                                var ischislStrahStaj = staj_lgot.Element("ИС");
                                if (ischislStrahStaj != null)
                                {
                                    if (ischislStrahStaj.Element("Основание") != null)
                                    {
                                        str = ischislStrahStaj.Element("Основание").Value.ToString().ToUpper();
                                        if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                    {
                                        if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                        {
                                            stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                        }
                                        if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                        {
                                            stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                        }
                                    }
                                    if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                    {
                                        if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                        {
                                            stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                        }
                                        if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                        {
                                            stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                        }
                                    }
                                }

                                var ischislStrahStajDop = staj_lgot.Element("ДопСведенияИС");
                                if (ischislStrahStajDop != null)
                                {
                                    str = ischislStrahStajDop.Value.ToString().ToUpper();
                                    if (IschislStrahStajDop_list.Any(x => x.Code.ToUpper() == str))
                                    {
                                        stajLgot.IschislStrahStajDopID = IschislStrahStajDop_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                    }
                                }


                                var uslDosrNazn = staj_lgot.Element("ВЛ");
                                if (uslDosrNazn != null) // если есть терр условия
                                {
                                    //если есть запись в с таким кодом терр условий в базе
                                    if (uslDosrNazn.Element("Основание") != null)
                                    {
                                        str = uslDosrNazn.Element("Основание").Value.ToString().ToUpper();
                                        if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                        {
                                            stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                        }
                                    }
                                    if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                    {
                                        if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                        {
                                            stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                        }
                                        if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                        {
                                            stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                        }
                                    }
                                    if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                    {
                                        if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                        {
                                            stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                        }
                                        if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                        {
                                            stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                        }
                                    }
                                    if (uslDosrNazn.Element("ДоляСтавки") != null)
                                    {
                                        stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                    }
                                }

                                stajOsn.StajLgot.Add(stajLgot);
                            }
                            szv_korr.StajOsn.Add(stajOsn);


                        }



                        #endregion

                        db.AddToFormsSZV_KORR_2017(szv_korr);
                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        if (transactionCheckBox.Checked)
                        {
                            if (cnt_records_imported == 50 || cnt_records_imported == 100 || cnt_records_imported == 150)
                                db.SaveChanges();
                        }
                        else
                            db.SaveChanges();

                        cnt_records_imported++;
                        importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));





                    }
                    catch (Exception ex)
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_korr_item.Element("НомерВпачке").Value.ToString(), type = ex.Message + "\r\n" });
                    }

                    k++;


                    decimal temp = (decimal)k / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, k.ToString());

                }



                #endregion




                db.SaveChanges();



                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));

                result = false;
            }

            return result;
        }



        /// <summary>
        /// Импорт Файлов ПФР Формы ЗАГС РОЖДЕНИЕ
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_ZAGS_Born(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {

                XElement РСОР = doc.Descendants().First(x => x.Name.LocalName == "РСОР");

                #region СведенияОРождении

                int i = 0;

                var СведенияОРождении = РСОР.Descendants().Where(x => x.Name.LocalName == "СведенияОРождении");

                int count = СведенияОРождении.Count();

                foreach (var item in СведенияОРождении)
                {
                    i++;
                    ZAGS_Born zagsBorn = new ZAGS_Born();

                    if (item.Element("Родившийся") != null)
                    {
                        XElement Родившийся = item.Element("Родившийся");

                        zagsBorn.cLastName = Родившийся.Element("ФИО") != null ? Родившийся.Element("ФИО").Element("Фамилия") != null ? Родившийся.Element("ФИО").Element("Фамилия").Value : "" : "";
                        zagsBorn.cFirstName = Родившийся.Element("ФИО") != null ? Родившийся.Element("ФИО").Element("Имя") != null ? Родившийся.Element("ФИО").Element("Имя").Value : "" : "";
                        zagsBorn.cMiddleName = Родившийся.Element("ФИО") != null ? Родившийся.Element("ФИО").Element("Отчество") != null ? Родившийся.Element("ФИО").Element("Отчество").Value : "" : "";

                        if (Родившийся.Element("ФИО") != null && Родившийся.Element("ФИО").Element("ФамилияРожд") != null)
                        {
                            zagsBorn.cLastName = Родившийся.Element("ФИО").Element("ФамилияРожд").Value;
                        }

                        if (Родившийся.Element("Пол") != null)
                        {
                            zagsBorn.cSex = Родившийся.Element("Пол").Value == "М" ? (byte)0 : (byte)1;
                        }

                        if (Родившийся.Element("ДатаРождения") != null)
                        {
                            DateTime dBirth;
                            if (DateTime.TryParse(Родившийся.Element("ДатаРождения").Value, out dBirth))
                            {
                                zagsBorn.cType_DateBirth = (short)0;
                                zagsBorn.cDateBirth = dBirth;
                            }
                        }

                        if (Родившийся.Element("ДатаРожденияОсобая") != null)
                        {
                            XElement DRO = Родившийся.Element("ДатаРожденияОсобая");

                            if (DRO.Element("День") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("День").Value, out d);

                                zagsBorn.cDateBirthMonth_Os = d;
                            }
                            if (DRO.Element("Месяц") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("Месяц").Value, out d);

                                zagsBorn.cDateBirthDay_Os = d;
                            }
                            if (DRO.Element("Год") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("Год").Value, out d);

                                zagsBorn.cDateBirthYear_Os = d;
                            }

                            zagsBorn.cType_DateBirth = (short)1;
                        }

                        if (Родившийся.Element("МестоРождения") != null)
                        {
                            if (Родившийся.Element("МестоРождения").Element("ТипМестаРождения") != null)
                            {
                                zagsBorn.cType_PlaceBirth = 1;
                                zagsBorn.cType_PlaceBirth = Родившийся.Element("МестоРождения").Element("ТипМестаРождения").Value == "СТАНДАРТНОЕ" ? (short)1 : (short)2;
                            }

                            zagsBorn.cPunkt = Родившийся.Element("МестоРождения").Element("ГородРождения") != null ? Родившийся.Element("МестоРождения").Element("ГородРождения").Value : "";
                            zagsBorn.cDistr = Родившийся.Element("МестоРождения").Element("РайонРождения") != null ? Родившийся.Element("МестоРождения").Element("РайонРождения").Value : "";
                            zagsBorn.cRegion = Родившийся.Element("МестоРождения").Element("РегионРождения") != null ? Родившийся.Element("МестоРождения").Element("РегионРождения").Value : "";
                            zagsBorn.cCountry = Родившийся.Element("МестоРождения").Element("СтранаРождения") != null ? Родившийся.Element("МестоРождения").Element("СтранаРождения").Value : "";

                        }

                    }

                    if (item.Element("Мать") != null)
                    {
                        XElement Мать = item.Element("Мать");

                        zagsBorn.mLastName = Мать.Element("ФИО") != null ? Мать.Element("ФИО").Element("Фамилия") != null ? Мать.Element("ФИО").Element("Фамилия").Value : "" : "";
                        zagsBorn.mFirstName = Мать.Element("ФИО") != null ? Мать.Element("ФИО").Element("Имя") != null ? Мать.Element("ФИО").Element("Имя").Value : "" : "";
                        zagsBorn.mMiddleName = Мать.Element("ФИО") != null ? Мать.Element("ФИО").Element("Отчество") != null ? Мать.Element("ФИО").Element("Отчество").Value : "" : "";

                        if (Мать.Element("ФИО") != null && Мать.Element("ФИО").Element("ФамилияРожд") != null)
                        {
                            zagsBorn.mLastName = Мать.Element("ФИО").Element("ФамилияРожд").Value;
                        }


                        if (Мать.Element("ДатаРождения") != null)
                        {
                            DateTime dBirth;
                            if (DateTime.TryParse(Мать.Element("ДатаРождения").Value, out dBirth))
                            {
                                zagsBorn.mType_DateBirth = (short)0;
                                zagsBorn.mDateBirth = dBirth;
                            }
                        }

                        if (Мать.Element("ДатаРожденияОсобая") != null)
                        {
                            XElement DRO = Мать.Element("ДатаРожденияОсобая");

                            if (DRO.Element("День") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("День").Value, out d);

                                zagsBorn.mDateBirthMonth_Os = d;
                            }
                            if (DRO.Element("Месяц") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("Месяц").Value, out d);

                                zagsBorn.mDateBirthDay_Os = d;
                            }
                            if (DRO.Element("Год") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("Год").Value, out d);

                                zagsBorn.mDateBirthYear_Os = d;
                            }

                            zagsBorn.mType_DateBirth = (short)1;
                        }

                        if (Мать.Element("МестоРождения") != null)
                        {
                            if (Мать.Element("МестоРождения").Element("ТипМестаРождения") != null)
                            {
                                zagsBorn.mType_PlaceBirth = 1;
                                zagsBorn.mType_PlaceBirth = Мать.Element("МестоРождения").Element("ТипМестаРождения").Value == "СТАНДАРТНОЕ" ? (short)1 : (short)2;
                            }

                            zagsBorn.mPunkt = Мать.Element("МестоРождения").Element("ГородРождения") != null ? Мать.Element("МестоРождения").Element("ГородРождения").Value : "";
                            zagsBorn.mDistr = Мать.Element("МестоРождения").Element("РайонРождения") != null ? Мать.Element("МестоРождения").Element("РайонРождения").Value : "";
                            zagsBorn.mRegion = Мать.Element("МестоРождения").Element("РегионРождения") != null ? Мать.Element("МестоРождения").Element("РегионРождения").Value : "";
                            zagsBorn.mCountry = Мать.Element("МестоРождения").Element("СтранаРождения") != null ? Мать.Element("МестоРождения").Element("СтранаРождения").Value : "";

                        }

                        zagsBorn.mCitizenship = Мать.Element("Гражданство") != null ? Мать.Element("Гражданство").Value : "";


                    }

                    if (item.Element("Отец") != null)
                    {
                        XElement Отец = item.Element("Отец");

                        zagsBorn.fLastName = Отец.Element("ФИО") != null ? Отец.Element("ФИО").Element("Фамилия") != null ? Отец.Element("ФИО").Element("Фамилия").Value : "" : "";
                        zagsBorn.fFirstName = Отец.Element("ФИО") != null ? Отец.Element("ФИО").Element("Имя") != null ? Отец.Element("ФИО").Element("Имя").Value : "" : "";
                        zagsBorn.fMiddleName = Отец.Element("ФИО") != null ? Отец.Element("ФИО").Element("Отчество") != null ? Отец.Element("ФИО").Element("Отчество").Value : "" : "";

                        if (Отец.Element("ФИО") != null && Отец.Element("ФИО").Element("ФамилияРожд") != null)
                        {
                            zagsBorn.fLastName = Отец.Element("ФИО").Element("ФамилияРожд").Value;
                        }


                        if (Отец.Element("ДатаРождения") != null)
                        {
                            DateTime dBirth;
                            if (DateTime.TryParse(Отец.Element("ДатаРождения").Value, out dBirth))
                            {
                                zagsBorn.fType_DateBirth = (short)0;
                                zagsBorn.fDateBirth = dBirth;
                            }
                        }

                        if (Отец.Element("ДатаРожденияОсобая") != null)
                        {
                            XElement DRO = Отец.Element("ДатаРожденияОсобая");

                            if (DRO.Element("День") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("День").Value, out d);

                                zagsBorn.fDateBirthMonth_Os = d;
                            }
                            if (DRO.Element("Месяц") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("Месяц").Value, out d);

                                zagsBorn.fDateBirthDay_Os = d;
                            }
                            if (DRO.Element("Год") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("Год").Value, out d);

                                zagsBorn.fDateBirthYear_Os = d;
                            }

                            zagsBorn.fType_DateBirth = (short)1;
                        }

                        if (Отец.Element("МестоРождения") != null)
                        {
                            if (Отец.Element("МестоРождения").Element("ТипМестаРождения") != null)
                            {
                                zagsBorn.fType_PlaceBirth = 1;
                                zagsBorn.fType_PlaceBirth = Отец.Element("МестоРождения").Element("ТипМестаРождения").Value == "СТАНДАРТНОЕ" ? (short)1 : (short)2;
                            }

                            zagsBorn.fPunkt = Отец.Element("МестоРождения").Element("ГородРождения") != null ? Отец.Element("МестоРождения").Element("ГородРождения").Value : "";
                            zagsBorn.fDistr = Отец.Element("МестоРождения").Element("РайонРождения") != null ? Отец.Element("МестоРождения").Element("РайонРождения").Value : "";
                            zagsBorn.fRegion = Отец.Element("МестоРождения").Element("РегионРождения") != null ? Отец.Element("МестоРождения").Element("РегионРождения").Value : "";
                            zagsBorn.fCountry = Отец.Element("МестоРождения").Element("СтранаРождения") != null ? Отец.Element("МестоРождения").Element("СтранаРождения").Value : "";

                        }

                        zagsBorn.fCitizenship = Отец.Element("Гражданство") != null ? Отец.Element("Гражданство").Value : "";


                    }

                    if (item.Element("Акт") != null)
                    {
                        zagsBorn.cAkt_Num = item.Element("Акт").Element("Номер") != null ? item.Element("Акт").Element("Номер").Value : "";
                        zagsBorn.cAkt_OrgZags = item.Element("Акт").Element("ОрганЗАГС") != null ? item.Element("Акт").Element("ОрганЗАГС").Value : "";


                        if (item.Element("Акт").Element("ДатаСоставления") != null)
                        {
                            DateTime dt;
                            if (DateTime.TryParse(item.Element("Акт").Element("ДатаСоставления").Value, out dt))
                            {
                                zagsBorn.cAkt_Date = dt;
                            }
                        }
                    }

                    if (item.Element("Свидетельство") != null)
                    {
                        zagsBorn.cSvid_Num = item.Element("Свидетельство").Element("Номер") != null ? item.Element("Свидетельство").Element("Номер").Value : "";
                        zagsBorn.cSvid_Ser = item.Element("Свидетельство").Element("Серия") != null ? item.Element("Свидетельство").Element("Серия").Value : "";
                        zagsBorn.cSvid_OrgZags = item.Element("Свидетельство").Element("КемВыдан") != null ? item.Element("Свидетельство").Element("КемВыдан").Value : "";


                        if (item.Element("Свидетельство").Element("ДатаВыдачи") != null)
                        {
                            DateTime dt;
                            if (DateTime.TryParse(item.Element("Свидетельство").Element("ДатаВыдачи").Value, out dt))
                            {
                                zagsBorn.cSvid_Date = dt;
                            }
                        }
                    }

                #endregion
                    db.AddToZAGS_Born(zagsBorn);


                    if (i % 100 == 0)
                    {
                        db.SaveChanges();
                    }



                    cnt_records_imported++;
                    importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = i; }));

                    decimal temp = (decimal)i / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, i.ToString());

                }
                db.SaveChanges();





                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));

                result = false;
            }

            return result;
        }


        /// <summary>
        /// Импорт Файлов ПФР Формы ЗАГС Умершие
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_ZAGS_Death(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {

                XElement РСОС = doc.Descendants().First(x => x.Name.LocalName == "РСОС");

                #region СведенияОСмерти

                int i = 0;

                var СведенияОСмерти = РСОС.Descendants().Where(x => x.Name.LocalName == "СведенияОСмерти");

                int count = СведенияОСмерти.Count();

                foreach (var item in СведенияОСмерти)
                {
                    i++;
                    ZAGS_Death zagsDeath = new ZAGS_Death();

                    if (item.Element("Умерший") != null)
                    {
                        XElement Умерший = item.Element("Умерший");

                        zagsDeath.LastName = Умерший.Element("ФИО") != null ? Умерший.Element("ФИО").Element("Фамилия") != null ? Умерший.Element("ФИО").Element("Фамилия").Value : "" : "";
                        zagsDeath.FirstName = Умерший.Element("ФИО") != null ? Умерший.Element("ФИО").Element("Имя") != null ? Умерший.Element("ФИО").Element("Имя").Value : "" : "";
                        zagsDeath.MiddleName = Умерший.Element("ФИО") != null ? Умерший.Element("ФИО").Element("Отчество") != null ? Умерший.Element("ФИО").Element("Отчество").Value : "" : "";

                        if (Умерший.Element("ФИО") != null && Умерший.Element("ФИО").Element("ФамилияРожд") != null)
                        {
                            zagsDeath.LastName = Умерший.Element("ФИО").Element("ФамилияРожд").Value;
                        }

                        if (Умерший.Element("Пол") != null)
                        {
                            zagsDeath.Sex = Умерший.Element("Пол").Value == "М" ? (byte)0 : (byte)1;
                        }

                        if (Умерший.Element("ДатаРождения") != null)
                        {
                            DateTime dBirth;
                            if (DateTime.TryParse(Умерший.Element("ДатаРождения").Value, out dBirth))
                            {
                                zagsDeath.Type_DateBirth = (short)0;
                                zagsDeath.DateBirth = dBirth;
                            }
                        }

                        if (Умерший.Element("ДатаРожденияОсобая") != null)
                        {
                            XElement DRO = Умерший.Element("ДатаРожденияОсобая");

                            if (DRO.Element("День") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("День").Value, out d);

                                zagsDeath.DateBirthMonth_Os = d;
                            }
                            if (DRO.Element("Месяц") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("Месяц").Value, out d);

                                zagsDeath.DateBirthDay_Os = d;
                            }
                            if (DRO.Element("Год") != null)
                            {
                                short d = 0;
                                short.TryParse(DRO.Element("Год").Value, out d);

                                zagsDeath.DateBirthYear_Os = d;
                            }

                            zagsDeath.Type_DateBirth = (short)1;
                        }

                        if (Умерший.Element("МестоРождения") != null)
                        {
                            if (Умерший.Element("МестоРождения").Element("ТипМестаРождения") != null)
                            {
                                zagsDeath.Type_PlaceBirth = 1;
                                zagsDeath.Type_PlaceBirth = Умерший.Element("МестоРождения").Element("ТипМестаРождения").Value == "СТАНДАРТНОЕ" ? (short)1 : (short)2;
                            }

                            zagsDeath.PunktBirth = Умерший.Element("МестоРождения").Element("ГородРождения") != null ? Умерший.Element("МестоРождения").Element("ГородРождения").Value : "";
                            zagsDeath.DistrBirth = Умерший.Element("МестоРождения").Element("РайонРождения") != null ? Умерший.Element("МестоРождения").Element("РайонРождения").Value : "";
                            zagsDeath.RegionBirth = Умерший.Element("МестоРождения").Element("РегионРождения") != null ? Умерший.Element("МестоРождения").Element("РегионРождения").Value : "";
                            zagsDeath.CountryBirth = Умерший.Element("МестоРождения").Element("СтранаРождения") != null ? Умерший.Element("МестоРождения").Element("СтранаРождения").Value : "";

                        }

                    }

                    if (item.Element("ДатаСмерти") != null)
                    {
                        DateTime dDeath;
                        if (DateTime.TryParse(item.Element("ДатаСмерти").Value, out dDeath))
                        {
                            zagsDeath.DateDeath = dDeath;
                        }
                    }

                    if (item.Element("МестоСмерти") != null)
                    {
                        XElement place = item.Element("МестоСмерти");

                        if (place.Element("Регион") != null)
                        {
                            XElement place_ = place.Element("Регион");

                            zagsDeath.RegionDeath = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.RegionDeath_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Район") != null)
                        {
                            XElement place_ = place.Element("Район");

                            zagsDeath.DistrDeath = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.DistrDeath_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Город") != null)
                        {
                            XElement place_ = place.Element("Город");

                            zagsDeath.CityDeath = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.CityDeath_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("НаселенныйПункт") != null)
                        {
                            XElement place_ = place.Element("НаселенныйПункт");

                            zagsDeath.PunktDeath = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.PunktDeath_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Улица") != null)
                        {
                            XElement place_ = place.Element("Улица");

                            zagsDeath.StreetDeath = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.StreetDeath_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }


                        if (place.Element("Дом") != null)
                        {
                            XElement place_ = place.Element("Дом");

                            zagsDeath.DomDeath = place_.Element("Номер") != null ? place_.Element("Номер").Value : "";
                            zagsDeath.DomDeath_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Строение") != null)
                        {
                            XElement place_ = place.Element("Строение");

                            zagsDeath.StroenDeath = place_.Element("Номер") != null ? place_.Element("Номер").Value : "";
                            zagsDeath.StroenDeath_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Корпус") != null)
                        {
                            XElement place_ = place.Element("Корпус");

                            zagsDeath.KorpDeath = place_.Element("Номер") != null ? place_.Element("Номер").Value : "";
                            zagsDeath.KorpDeath_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Квартира") != null)
                        {
                            XElement place_ = place.Element("Квартира");

                            zagsDeath.KvartDeath = place_.Element("Номер") != null ? place_.Element("Номер").Value : "";
                            zagsDeath.KvartDeath_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }

                    }

                    if (item.Element("ПоследнееМестоЖительства") != null)
                    {
                        XElement place = item.Element("ПоследнееМестоЖительства");

                        if (place.Element("Регион") != null)
                        {
                            XElement place_ = place.Element("Регион");

                            zagsDeath.RegionLast = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.RegionLast_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Район") != null)
                        {
                            XElement place_ = place.Element("Район");

                            zagsDeath.DistrLast = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.DistrLast_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Город") != null)
                        {
                            XElement place_ = place.Element("Город");

                            zagsDeath.CityLast = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.CityLast_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("НаселенныйПункт") != null)
                        {
                            XElement place_ = place.Element("НаселенныйПункт");

                            zagsDeath.PunktLast = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.PunktLast_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Улица") != null)
                        {
                            XElement place_ = place.Element("Улица");

                            zagsDeath.StreetLast = place_.Element("Название") != null ? place_.Element("Название").Value : "";
                            zagsDeath.StreetLast_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }


                        if (place.Element("Дом") != null)
                        {
                            XElement place_ = place.Element("Дом");

                            zagsDeath.DomLast = place_.Element("Номер") != null ? place_.Element("Номер").Value : "";
                            zagsDeath.DomLast_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Строение") != null)
                        {
                            XElement place_ = place.Element("Строение");

                            zagsDeath.StroenLast = place_.Element("Номер") != null ? place_.Element("Номер").Value : "";
                            zagsDeath.StroenLast_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Корпус") != null)
                        {
                            XElement place_ = place.Element("Корпус");

                            zagsDeath.KorpLast = place_.Element("Номер") != null ? place_.Element("Номер").Value : "";
                            zagsDeath.KorpLast_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }
                        if (place.Element("Квартира") != null)
                        {
                            XElement place_ = place.Element("Квартира");

                            zagsDeath.KvartLast = place_.Element("Номер") != null ? place_.Element("Номер").Value : "";
                            zagsDeath.KvartLast_sokr = place_.Element("Сокращение") != null ? place_.Element("Сокращение").Value : "";
                        }

                    }


                    if (item.Element("Акт") != null)
                    {
                        zagsDeath.Akt_Num = item.Element("Акт").Element("Номер") != null ? item.Element("Акт").Element("Номер").Value : "";
                        zagsDeath.Akt_OrgZags = item.Element("Акт").Element("ОрганЗАГС") != null ? item.Element("Акт").Element("ОрганЗАГС").Value : "";


                        if (item.Element("Акт").Element("ДатаСоставления") != null)
                        {
                            DateTime dt;
                            if (DateTime.TryParse(item.Element("Акт").Element("ДатаСоставления").Value, out dt))
                            {
                                zagsDeath.Akt_Date = dt;
                            }
                        }
                    }



                #endregion
                    db.AddToZAGS_Death(zagsDeath);


                    if (i % 100 == 0)
                    {
                        db.SaveChanges();
                    }



                    cnt_records_imported++;
                    importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = i; }));

                    decimal temp = (decimal)i / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, i.ToString());

                }
                db.SaveChanges();





                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));

                result = false;
            }

            return result;
        }



        private string checkStringLength(string s, int lng)
        {
            s = s.Trim();
            if (s.Length > lng)
                s = s.Substring(0, lng);

            return s;
        }

        /// <summary>
        /// Импорт Файлов ПФР Формы ДСВ-3
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_DSW3(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);
            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            bool result = true;
            XElement node = doc.Root;

            try
            {
                #region // Поиск информации о составителе
                node = node.Descendants().First(x => x.Name.LocalName == "СоставительПачки");
                string inn = node.Element("НалоговыйНомер").Element("ИНН").Value;
                string kpp = node.Element("НалоговыйНомер").Element("КПП") != null ? node.Element("НалоговыйНомер").Element("КПП").Value.ToString() : "";
                byte type_ = inn.Length == 12 ? (byte)1 : (byte)0;
                string egrip = node.Element("КодЕГРИП") != null ? node.Element("КодЕГРИП").Value.ToString() : "";
                string egrul = node.Element("КодЕГРЮЛ") != null ? node.Element("КодЕГРЮЛ").Value.ToString() : "";


                string regnum = node.Element("РегистрационныйНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                string fullName = node.Element("НаименованиеОрганизации") != null ? node.Element("НаименованиеОрганизации").Value.ToString() : "";
                string shortName = node.Element("НаименованиеКраткое") != null ? node.Element("НаименованиеКраткое").Value.ToString() : "";

                if (fullName == "")
                    fullName = shortName;

                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (type_ == 0)
                        {
                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                            if (insurer.Name != fullName)
                            {
                                insurer.Name = fullName;
                                change = true;
                            }
                        }
                        else
                        {
                            fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;

                            var fio = fullName.Split(' ');
                            if (fio.Count() > 0)
                            {
                                if (insurer.LastName != fio[0])
                                {
                                    insurer.LastName = fio[0];
                                    change = true;
                                }
                                if (fio.Count() > 1)
                                    if (insurer.FirstName != fio[1])
                                    {
                                        insurer.FirstName = fio[1];
                                        change = true;
                                    }
                                if (fio.Count() > 2)
                                {
                                    insurer.MiddleName = "";

                                    for (int i = 2; i < fio.Count(); i++)
                                    {
                                        insurer.MiddleName = insurer.MiddleName + fio[i];
                                    }
                                }
                            }
                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                        }

                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }
                        if (insurer.EGRIP != egrip)
                        {
                            insurer.EGRIP = egrip;
                            change = true;
                        }
                        if (insurer.EGRUL != egrul)
                        {
                            insurer.EGRUL = egrul;
                            change = true;
                        }

                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;

                    insurer.TypePayer = type_;
                    if (type_ == 0)
                    {
                        insurer.NameShort = shortName;
                        insurer.Name = fullName;
                    }
                    else
                    {
                        var fio = fullName.Split(' ');
                        if (fio.Count() > 0)
                        {
                            insurer.LastName = fio[0];
                            if (fio.Count() > 1)
                                insurer.FirstName = fio[1];
                            if (fio.Count() > 2)
                            {
                                for (int i = 2; i < fio.Count(); i++)
                                {
                                    insurer.MiddleName = insurer.MiddleName + fio[i];
                                }
                            }
                        }
                        insurer.NameShort = shortName;
                    }
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    insurer.EGRIP = egrip;
                    insurer.EGRUL = egrul;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }
                #endregion

                XElement ДСВ3 = doc.Descendants().First(x => x.Name.LocalName == "РеестрДСВ");

                FormsDSW_3 dsw3 = new FormsDSW_3();

                DateTime dt = DateTime.Now;
                if (DateTime.TryParse(ДСВ3.Element("ПлатежноеПоручение").Element("ДатаПоручения").Value.ToString(), out dt))
                {
                    dsw3.DATEPAYMENT = dt;
                };

                dsw3.NUMBERPAYMENT = ДСВ3.Element("ПлатежноеПоручение").Element("НомерПоручения") != null ? ДСВ3.Element("ПлатежноеПоручение").Element("НомерПоручения").Value.ToString() : "";

                if (DateTime.TryParse(ДСВ3.Element("ПлатежноеПоручение").Element("ДатаИсполненияПоручения").Value.ToString(), out dt))
                {
                    dsw3.DATEEXECUTPAYMENT = dt;
                };

                short Year = 1900;
                if (short.TryParse(ДСВ3.Element("Год").Value.ToString(), out Year))
                {
                    dsw3.YEAR = Year;
                };

                dsw3.InsurerID = insurer.ID;

                if (db.FormsDSW_3.Any(x => x.InsurerID == insurer.ID && x.YEAR == dsw3.YEAR && x.DATEEXECUTPAYMENT == dsw3.DATEEXECUTPAYMENT && x.DATEPAYMENT == dsw3.DATEPAYMENT && x.NUMBERPAYMENT == dsw3.NUMBERPAYMENT))
                {
                    if (updateDSW3_DDL.SelectedItem.Tag.ToString() == "0")
                    {
                        string id = db.FormsDSW_3.First(x => x.InsurerID == insurer.ID && x.YEAR == dsw3.YEAR && x.DATEEXECUTPAYMENT == dsw3.DATEEXECUTPAYMENT && x.DATEPAYMENT == dsw3.DATEPAYMENT && x.NUMBERPAYMENT == dsw3.NUMBERPAYMENT).ID.ToString();
                        try
                        {
                            db.ExecuteStoreCommand(String.Format("DELETE FROM FormsDSW_3 WHERE ([ID] = {0})", id));

                            dt = DateTime.Now;
                            DateTime.TryParse(doc.Descendants().First(x => x.Name.LocalName == "ДатаСоставления").Value.ToString(), out dt);
                            dsw3.DateFilling = dt;

                            db.AddToFormsDSW_3(dsw3);
                            //                db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\nПри удалении Формы ДСВ-3 произошла ошибка. Ошибка: " + ex.Message, this.ThemeName); }));
                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = "При удалении Формы ДСВ-3 произошла ошибка. Ошибка: " + ex.Message + "\r\n" });

                            return false;
                        }
                    }
                    else if (updateDSW3_DDL.SelectedItem.Tag.ToString() == "1")
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = "Файл Формы ДСВ-3 был пропущен, т.к. Форма с такими же параметрами уже есть в базе данных. Можно изменить настройки импорта формы, для перезаписи\r\n" });

                        return false;
                    }
                    else if (updateDSW3_DDL.SelectedItem.Tag.ToString() == "2")  // объединение
                    {
                        dsw3 = db.FormsDSW_3.First(x => x.InsurerID == insurer.ID && x.YEAR == dsw3.YEAR && x.DATEEXECUTPAYMENT == dsw3.DATEEXECUTPAYMENT && x.DATEPAYMENT == dsw3.DATEPAYMENT && x.NUMBERPAYMENT == dsw3.NUMBERPAYMENT);
                    }


                }
                else
                {



                    dt = DateTime.Now;
                    DateTime.TryParse(doc.Descendants().First(x => x.Name.LocalName == "ДатаСоставления").Value.ToString(), out dt);
                    dsw3.DateFilling = dt;

                    db.AddToFormsDSW_3(dsw3);
                }



                #region СписокЗЛ

                int k = 0;

                var ЗЛ = doc.Descendants().Where(x => x.Name.LocalName == "РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ");

                int count = ЗЛ.Count();

                foreach (var item in ЗЛ)
                {
                    k++;
                    string LastName = item.Element("ФИО") != null ? item.Element("ФИО").Element("Фамилия") != null ? item.Element("ФИО").Element("Фамилия").Value : "" : "";
                    string FirstName = item.Element("ФИО") != null ? item.Element("ФИО").Element("Имя") != null ? item.Element("ФИО").Element("Имя").Value : "" : "";
                    string MiddleName = item.Element("ФИО") != null ? item.Element("ФИО").Element("Отчество") != null ? item.Element("ФИО").Element("Отчество").Value : "" : "";

                    if (item.Element("СтраховойНомер") == null || (item.Element("СтраховойНомер") != null && String.IsNullOrEmpty(item.Element("СтраховойНомер").Value)))
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = k.ToString(), type = LastName + " " + FirstName + " " + MiddleName + ". Ошибка при загрузке Индивидуальных сведений. Не указан Страховой номер (СНИЛС) сотрудника.\r\n" });
                        continue;
                    }

                    var snils = Utils.ParseSNILS_XML(item.Element("СтраховойНомер").Value.ToString(), true);

                    Staff staff = new Staff();
                    if (db.Staff.Any(x => x.InsurerID == insurer.ID && x.InsuranceNumber == snils.num))  // Если такой сотрудник уже есть
                    {
                        if (updateStaffData.Checked)
                        {
                            staff = db.Staff.FirstOrDefault(x => x.InsurerID == insurer.ID && x.InsuranceNumber == snils.num);

                            if ((staff.LastName != LastName) || (staff.FirstName != FirstName) || (staff.MiddleName != MiddleName) || (staff.InsuranceNumber != snils.num))
                            {
                                staff.InsuranceNumber = snils.num;
                                staff.ControlNumber = snils.contrNum;
                                staff.LastName = LastName;
                                staff.FirstName = FirstName;
                                staff.MiddleName = MiddleName;
                                staff.Dismissed = staff.Dismissed.Value;
                                db.ObjectStateManager.ChangeObjectState(staff, EntityState.Modified);
                            }



                        }
                    }
                    else // если добавляем нового сотрудника
                    {

                        staff.InsurerID = insurer.ID;
                        staff.InsuranceNumber = snils.num.PadLeft(9, '0');
                        staff.ControlNumber = snils.contrNum;
                        staff.LastName = LastName;
                        staff.FirstName = FirstName;
                        staff.MiddleName = MiddleName;
                        staff.Dismissed = (byte)0;
                        db.AddToStaff(staff);
                    }

                    if (bw.CancellationPending)
                    {
                        db.SaveChanges();
                        return false;
                    }

                    if (k % 100 == 0)
                    {
                        db.SaveChanges();
                    }
                    cnt_records_imported++;
                    importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = k; }));

                    decimal temp = (decimal)k / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, k.ToString());

                }
                db.SaveChanges();
                k = 0;
                foreach (var item in ЗЛ)
                {
                    k++;
                    if (item.Element("СтраховойНомер") == null || (item.Element("СтраховойНомер") != null && String.IsNullOrEmpty(item.Element("СтраховойНомер").Value)))
                    {
                        continue;
                    }

                    decimal SUMFEEPFR_EMPLOYERS = 0;
                    decimal.TryParse(item.Element("СуммаДСВРаботника") != null ? item.Element("СуммаДСВРаботника").Value.ToString() : "0", out SUMFEEPFR_EMPLOYERS);

                    decimal SUMFEEPFR_PAYER = 0;
                    decimal.TryParse(item.Element("СуммаДСВРаботодателя") != null ? item.Element("СуммаДСВРаботодателя").Value.ToString() : "0", out SUMFEEPFR_PAYER);


                    var snils = Utils.ParseSNILS_XML(item.Element("СтраховойНомер").Value.ToString(), true);

                    long staffID = db.Staff.First(x => x.InsurerID == insurer.ID && x.InsuranceNumber == snils.num).ID;

                    if (dsw3.FormsDSW_3_Staff.Any(x => x.StaffID == staffID))
                    {
                        var dsw3_t = dsw3.FormsDSW_3_Staff.First(x => x.StaffID == staffID);
                        dsw3_t.SUMFEEPFR_EMPLOYERS = (dsw3_t.SUMFEEPFR_EMPLOYERS.HasValue ? dsw3_t.SUMFEEPFR_EMPLOYERS.Value : 0) + SUMFEEPFR_EMPLOYERS;
                        dsw3_t.SUMFEEPFR_PAYER = (dsw3_t.SUMFEEPFR_PAYER.HasValue ? dsw3_t.SUMFEEPFR_PAYER.Value : 0) + SUMFEEPFR_PAYER;

                        db.ObjectStateManager.ChangeObjectState(dsw3_t, EntityState.Modified);
                    }
                    else
                    {
                        dsw3.FormsDSW_3_Staff.Add(new FormsDSW_3_Staff { StaffID = staffID, SUMFEEPFR_EMPLOYERS = SUMFEEPFR_EMPLOYERS, SUMFEEPFR_PAYER = SUMFEEPFR_PAYER });
                    }


                    if (bw.CancellationPending)
                    {
                        db.SaveChanges();
                        return false;
                    }

                    if (k % 100 == 0)
                    {
                        db.SaveChanges();
                    }

                }

                db.SaveChanges();


                #endregion

                result = true;

            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));

                result = false;
            }

            return result;
        }


        /// <summary>
        /// Импорт файлов ПФР СЗВ-6-4
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_SZV_6_4(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = new XDocument();
            doc = XDocument.Load(XML_path);

            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            XElement node = doc.Root;
            bool result = true;

            try
            {
                #region // Поиск информации о составителе
                node = node.Descendants().First(x => x.Name.LocalName == "СоставительПачки");
                string inn = node.Element("НалоговыйНомер").Element("ИНН").Value;
                string kpp = node.Element("НалоговыйНомер").Element("КПП") != null ? node.Element("НалоговыйНомер").Element("КПП").Value.ToString() : "";
                byte type_ = inn.Length == 12 ? (byte)1 : (byte)0;

                string regnum = node.Element("РегистрационныйНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                string fullName = node.Element("НаименованиеОрганизации") != null ? node.Element("НаименованиеОрганизации").Value : "";
                string shortName = node.Element("НаименованиеКраткое") != null ? node.Element("НаименованиеКраткое").Value : "";


                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (type_ == 0)
                        {
                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                            if (insurer.Name != fullName)
                            {
                                insurer.Name = fullName;
                                change = true;
                            }
                        }
                        else
                        {
                            fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;
                            var fio = fullName.Split(' ');
                            if (fio.Count() > 0)
                            {
                                if (insurer.LastName != fio[0])
                                {
                                    insurer.LastName = fio[0];
                                    change = true;
                                }
                                if (fio.Count() > 1)
                                    if (insurer.FirstName != fio[1])
                                    {
                                        insurer.FirstName = fio[1];
                                        change = true;
                                    }
                                if (fio.Count() > 2)
                                {
                                    insurer.MiddleName = "";

                                    for (int i = 2; i < fio.Count(); i++)
                                    {
                                        insurer.MiddleName = insurer.MiddleName + fio[i];
                                    }
                                }
                            }
                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                        }

                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }

                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = type_;
                    fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;

                    if (type_ == 0)
                    {
                        insurer.NameShort = shortName;
                        insurer.Name = fullName;
                    }
                    else
                    {
                        var fio = fullName.Split(' ');
                        if (fio.Count() > 0)
                        {
                            insurer.LastName = fio[0];
                            if (fio.Count() > 1)
                                insurer.FirstName = fio[1];
                            if (fio.Count() > 2)
                            {
                                for (int i = 2; i < fio.Count(); i++)
                                {
                                    insurer.MiddleName = insurer.MiddleName + fio[i];
                                }
                            }
                        }
                        insurer.NameShort = shortName;
                    }
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }
                #endregion


                #region перебор инд.сведений

                var szv_6_4_list = doc.Root.Descendants().Where(x => x.Name.LocalName == "СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ");

                int count = szv_6_4_list.Count();

                //              secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = count.ToString(); }));

                int k = 0;
                List<staffContainer> staffList = new List<staffContainer>();
                foreach (var szv_6_4 in szv_6_4_list)
                {
                    var snils = Utils.ParseSNILS_XML(szv_6_4.Element("СтраховойНомер") != null ? szv_6_4.Element("СтраховойНомер").Value.ToString().ToString() : "", true);

                    string middleName = szv_6_4.Element("ФИО").Element("Отчество") != null ? szv_6_4.Element("ФИО").Element("Отчество").Value.ToString() : "";

                    var st = new staffContainer
                    {
                        insuranceNum = snils.num,
                        insID = insurer.ID,
                        lastName = szv_6_4.Element("ФИО").Element("Фамилия").Value.ToString(),
                        firstName = szv_6_4.Element("ФИО").Element("Имя").Value.ToString(),
                        middleName = middleName,
                        contrNum = snils.contrNum
                    };

                    if (!staffList.Any(x => x.insuranceNum == st.insuranceNum))
                        staffList.Add(st);
                }

                var listid = staffList.Select(y => y.insuranceNum).ToList();
                var listNum = db.Staff.Where(x => x.InsurerID == insurer.ID && listid.Contains(x.InsuranceNumber)).Select(x => x.InsuranceNumber).ToList();

                staffList = staffList.Where(x => !listNum.Contains(x.insuranceNum)).ToList();  // те сотрудники которых нет в базе для текущего страхователя

                foreach (var item in staffList)
                {
                    Staff staff_ = new Staff
                    {
                        InsuranceNumber = item.insuranceNum.PadLeft(9, '0'),
                        InsurerID = item.insID,
                        LastName = item.lastName,
                        FirstName = item.firstName,
                        MiddleName = item.middleName,
                        ControlNumber = item.contrNum,
                        Dismissed = 0
                    };

                    db.AddToStaff(staff_);
                }
                if (staffList.Count > 0)
                    db.SaveChanges();


                foreach (var szv_6_4 in szv_6_4_list)
                {
                    try
                    {

                        string InsuranceNum = Utils.ParseSNILS_XML(szv_6_4.Element("СтраховойНомер") != null ? szv_6_4.Element("СтраховойНомер").Value.ToString().ToString() : "", false).num;


                        Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);

                        if (updateStaffData.Checked) // если выбрано обнослять данные в базе при расхождении
                        {
                            bool change = false;
                            if (staff.LastName != szv_6_4.Element("ФИО").Element("Фамилия").Value.ToString())
                            {
                                staff.LastName = szv_6_4.Element("ФИО").Element("Фамилия").Value.ToString();
                                change = true;
                            }
                            if (staff.FirstName != szv_6_4.Element("ФИО").Element("Имя").Value.ToString())
                            {
                                staff.FirstName = szv_6_4.Element("ФИО").Element("Имя").Value.ToString();
                                change = true;
                            }
                            if (szv_6_4.Element("ФИО").Element("Отчество") != null)
                                if (staff.MiddleName != szv_6_4.Element("ФИО").Element("Отчество").Value.ToString())
                                {
                                    staff.MiddleName = szv_6_4.Element("ФИО").Element("Отчество").Value.ToString();
                                    change = true;
                                }

                            if (change)
                            {
                                db.ObjectStateManager.ChangeObjectState(staff, EntityState.Modified);
                                //              db.SaveChanges();
                            }
                        }

                        if (String.IsNullOrEmpty(InsuranceNum))
                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_6_4.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке Индивидуальных сведений. Не указан Страховой номер (СНИЛС) сотрудника.\r\n" });


                        string tInfo = szv_6_4.Element("ТипСведений").Value.ToString().ToLower();
                        long tInfoID = typeInfo_.First(x => x.Name.ToLower() == tInfo).ID;
                        string pcCode = szv_6_4.Element("КодКатегории").Value.ToString().ToUpper();

                        if (!PlatCatList.Any(x => x.Code == pcCode))
                        {
                            if (String.IsNullOrEmpty(pcCode))
                            {
                                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_6_4.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Не указан Код Категории Плательщика.\r\n" });
                            }
                            else
                            {
                                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_6_4.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Указан Код Категории Плательщика - \"" + pcCode + "\", который отсутствует в списке доступных категорий в справочнике.\r\n" });
                            }
                            break;
                        }

                        long pcID = PlatCatList.FirstOrDefault(x => x.Code == pcCode).ID;



                        byte typeContr = szv_6_4.Element("ТипДоговора").Value.ToString().ToUpper() == "ТРУДОВОЙ" ? (byte)1 : (byte)2;

                        byte q = byte.Parse(szv_6_4.Element("ОтчетныйПериод").Element("Квартал").Value.ToString());
                        short y = short.Parse(szv_6_4.Element("ОтчетныйПериод").Element("Год").Value.ToString());


                        FormsSZV_6_4 szv64 = new FormsSZV_6_4();
                        szv64.AutoCalc = true;

                        bool exist = false;
                        byte qk = (byte)0;
                        short yk = (short)0;
                        string regNumK = "";

                        if (tInfo == "исходная")
                        {
                            if (db.FormsSZV_6_4.Any(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.PlatCategoryID == pcID && x.TypeContract == typeContr))
                            {
                                szv64 = db.FormsSZV_6_4.FirstOrDefault(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.PlatCategoryID == pcID && x.TypeContract == typeContr);
                                if (updateSZV_6_4_DDL.SelectedItem.Tag.ToString() == "0")
                                {
                                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_6_4 WHERE ([ID] = {0})", szv64.ID));
                                    szv64 = new FormsSZV_6_4();
                                }
                                else
                                    exist = true;
                            }
                        }
                        else
                        {
                            qk = byte.Parse(szv_6_4.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value.ToString());
                            yk = short.Parse(szv_6_4.Element("КорректируемыйОтчетныйПериод").Element("Год").Value.ToString());
                            if (szv_6_4.Element("РегистрационныйНомерКорректируемогоПериода") != null)
                            {
                                regNumK = szv_6_4.Element("РегистрационныйНомерКорректируемогоПериода").Value.ToString();
                                while (regNumK.Contains("-"))
                                    regNumK = regNumK.Remove(regNumK.IndexOf('-'), 1);
                            }

                            if (db.FormsSZV_6_4.Any(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.YearKorr == yk && x.QuarterKorr == qk && x.PlatCategoryID == pcID && x.TypeContract == typeContr))
                            {
                                szv64 = db.FormsSZV_6_4.FirstOrDefault(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.YearKorr == yk && x.QuarterKorr == qk && x.PlatCategoryID == pcID && x.TypeContract == typeContr);
                                if (updateSZV_6_4_DDL.SelectedItem.Tag.ToString() == "0")
                                {
                                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_6_4 WHERE ([ID] = {0})", szv64.ID));
                                    szv64 = new FormsSZV_6_4();
                                }
                                else
                                    exist = true;
                            }
                        }

                        if (exist) // если такая запись уже существует
                        {
                            if (updateSZV_6_4_DDL.SelectedItem.Tag.ToString() == "2") // объединение
                            {
                                if (updateSumFeeSZV_6_4.Checked) // начисленные взносы
                                {
                                    szv64.SumFeePFR_Strah = szv64.SumFeePFR_Strah + decimal.Parse(szv_6_4.Element("СуммаВзносовНаСтраховую").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                                    szv64.SumFeePFR_Nakop = szv64.SumFeePFR_Nakop + decimal.Parse(szv_6_4.Element("СуммаВзносовНаНакопительную").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                                }
                                if (updatePayFeeSZV_6_4.Checked) // уплаченные взносы
                                {
                                    szv64.SumPayPFR_Strah = szv64.SumPayPFR_Strah + decimal.Parse(szv_6_4.Element("СуммаВзносовНаСтраховую").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                                    szv64.SumPayPFR_Nakop = szv64.SumPayPFR_Nakop + decimal.Parse(szv_6_4.Element("СуммаВзносовНаНакопительную").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                                }
                                if (updateSumFL_SZV_6_4.Checked)  // Выплаты физ лицу
                                {
                                    var sumList = szv_6_4.Descendants().Where(x => x.Name.LocalName == "СуммаВыплатИвознагражденийВпользуЗЛ");

                                    foreach (var item in sumList)
                                    {
                                        int m = 0;
                                        if (item.Element("ТипСтроки").Value == "МЕСЦ")
                                        {
                                            string strMonth = item.Element("Месяц").Value.ToString();
                                            m = int.Parse(strMonth);
                                            if (m == 4 || m == 7 || m == 10)
                                                m = 1;
                                            else if (m == 5 || m == 8 || m == 11)
                                                m = 2;
                                            else if (m == 6 || m == 9 || m == 12)
                                                m = 3;
                                        }

                                        decimal data = 0;
                                        for (int i = 0; i <= 2; i++)
                                        {
                                            string itemName = "s_" + m + "_" + i.ToString();
                                            string nameStr = "";
                                            switch (i)
                                            {
                                                case 0:
                                                    nameStr = "СуммаВыплатВсего";
                                                    break;
                                                case 1:
                                                    nameStr = "СуммаВыплатНачисленыСтраховыеВзносыНеПревышающие";
                                                    break;
                                                case 2:
                                                    nameStr = "СуммаВыплатНачисленыСтраховыеВзносыПревышающие";
                                                    break;
                                            }

                                            if (nameStr != "" && item.Element(nameStr) != null)
                                                data = decimal.Parse(item.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                            if (nameStr != "" && item.Element(nameStr) == null)
                                                data = 0;

                                            var properties = szv64.GetType().GetProperty(itemName);
                                            if (properties != null)
                                            {
                                                object value = properties.GetValue(szv64, null);
                                                if (value != null)
                                                    data = data + decimal.Parse(value.ToString(), CultureInfo.InvariantCulture);
                                            }

                                            szv64.GetType().GetProperty(itemName).SetValue(szv64, data, null);
                                        }
                                    }
                                }

                                if (updateSumDop_SZV_6_4.Checked)  // Выплаты по доп тарифу
                                {
                                    var sumList = szv_6_4.Descendants().Where(x => x.Name.LocalName == "СуммаВыплатИвознагражденийПоДопТарифу");

                                    foreach (var item in sumList)
                                    {
                                        int m = 0;
                                        if (item.Element("ТипСтроки").Value == "МЕСЦ")
                                        {
                                            string strMonth = item.Element("Месяц").Value.ToString();
                                            m = int.Parse(strMonth);
                                            if (m == 4 || m == 7 || m == 10)
                                                m = 1;
                                            else if (m == 5 || m == 8 || m == 11)
                                                m = 2;
                                            else if (m == 6 || m == 9 || m == 12)
                                                m = 3;
                                        }

                                        decimal data = 0;
                                        for (int i = 0; i <= 1; i++)
                                        {
                                            string itemName = "d_" + m + "_" + i.ToString();
                                            string nameStr = "";
                                            switch (i)
                                            {
                                                case 0:
                                                    nameStr = "СуммаВыплатПоДопТарифу27-1";
                                                    break;
                                                case 1:
                                                    nameStr = "СуммаВыплатПоДопТарифу27-2-18";
                                                    break;
                                            }

                                            if (nameStr != "" && item.Element(nameStr) != null)
                                                data = decimal.Parse(item.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                            if (nameStr != "" && item.Element(nameStr) == null)
                                                data = 0;

                                            var properties = szv64.GetType().GetProperty(itemName);
                                            if (properties != null)
                                            {
                                                object value = properties.GetValue(szv64, null);
                                                if (value != null)
                                                    data = data + decimal.Parse(value.ToString(), CultureInfo.InvariantCulture);
                                            }

                                            szv64.GetType().GetProperty(itemName).SetValue(szv64, data, null);
                                        }
                                    }
                                }

                                db.ObjectStateManager.ChangeObjectState(szv64, EntityState.Modified);
                                //db.SaveChanges();
                            }
                            if (updateSZV_6_4_DDL.SelectedItem.Tag.ToString() == "1") // не загружать импортируемую форму
                            {
                                continue;
                            }
                        }
                        else // если такой записи нет то добавляем ее
                        {
                            szv64.InsurerID = insurer.ID;
                            szv64.StaffID = staff.ID;
                            szv64.Year = y;
                            szv64.Quarter = q;
                            szv64.PlatCategoryID = pcID;
                            szv64.TypeContract = typeContr;

                            if (yk != 0)
                            {
                                szv64.YearKorr = yk;
                                szv64.QuarterKorr = qk;
                                szv64.RegNumKorr = regNumK;
                            }
                            szv64.TypeInfoID = tInfoID;

                            if (szv_6_4.Element("СуммаВзносовНаСтраховую") != null)
                            {
                                szv64.SumFeePFR_Strah = decimal.Parse(szv_6_4.Element("СуммаВзносовНаСтраховую").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                                szv64.SumPayPFR_Strah = decimal.Parse(szv_6_4.Element("СуммаВзносовНаСтраховую").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                szv64.SumFeePFR_Strah = 0;
                                szv64.SumPayPFR_Strah = 0;
                            }
                            if (szv_6_4.Element("СуммаВзносовНаНакопительную") != null)
                            {
                                szv64.SumFeePFR_Nakop = decimal.Parse(szv_6_4.Element("СуммаВзносовНаНакопительную").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                                szv64.SumPayPFR_Nakop = decimal.Parse(szv_6_4.Element("СуммаВзносовНаНакопительную").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                szv64.SumFeePFR_Nakop = 0;
                                szv64.SumPayPFR_Nakop = 0;
                            }

                            var sumList = szv_6_4.Descendants().Where(x => x.Name.LocalName == "СуммаВыплатИвознагражденийВпользуЗЛ");

                            foreach (var item in sumList)
                            {
                                int m = 0;
                                if (item.Element("ТипСтроки").Value == "МЕСЦ")
                                {
                                    string strMonth = item.Element("Месяц").Value.ToString();
                                    m = int.Parse(strMonth);
                                    if (m == 4 || m == 7 || m == 10)
                                        m = 1;
                                    else if (m == 5 || m == 8 || m == 11)
                                        m = 2;
                                    else if (m == 6 || m == 9 || m == 12)
                                        m = 3;
                                }

                                decimal data = 0;
                                for (int i = 0; i <= 2; i++)
                                {
                                    string itemName = "s_" + m + "_" + i.ToString();
                                    string nameStr = "";
                                    switch (i)
                                    {
                                        case 0:
                                            nameStr = "СуммаВыплатВсего";
                                            break;
                                        case 1:
                                            nameStr = "СуммаВыплатНачисленыСтраховыеВзносыНеПревышающие";
                                            break;
                                        case 2:
                                            nameStr = "СуммаВыплатНачисленыСтраховыеВзносыПревышающие";
                                            break;
                                    }

                                    if (nameStr != "" && item.Element(nameStr) != null)
                                        data = decimal.Parse(item.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                    if (nameStr != "" && item.Element(nameStr) == null)
                                        data = 0;

                                    szv64.GetType().GetProperty(itemName).SetValue(szv64, data, null);

                                }
                            }

                            sumList = szv_6_4.Descendants().Where(x => x.Name.LocalName == "СуммаВыплатИвознагражденийПоДопТарифу");

                            foreach (var item in sumList)
                            {
                                int m = 0;
                                if (item.Element("ТипСтроки").Value == "МЕСЦ")
                                {
                                    string strMonth = item.Element("Месяц").Value.ToString();
                                    m = int.Parse(strMonth);
                                    if (m == 4 || m == 7 || m == 10)
                                        m = 1;
                                    else if (m == 5 || m == 8 || m == 11)
                                        m = 2;
                                    else if (m == 6 || m == 9 || m == 12)
                                        m = 3;
                                }

                                decimal data = 0;
                                for (int i = 0; i <= 1; i++)
                                {
                                    string itemName = "d_" + m + "_" + i.ToString();
                                    string nameStr = "";
                                    switch (i)
                                    {
                                        case 0:
                                            nameStr = "СуммаВыплатПоДопТарифу27-1";
                                            break;
                                        case 1:
                                            nameStr = "СуммаВыплатПоДопТарифу27-2-18";
                                            break;
                                    }

                                    if (nameStr != "" && item.Element(nameStr) != null)
                                        data = decimal.Parse(item.Element(nameStr).Value.ToString(), CultureInfo.InvariantCulture);
                                    if (nameStr != "" && item.Element(nameStr) == null)
                                        data = 0;

                                    szv64.GetType().GetProperty(itemName).SetValue(szv64, data, null);
                                }
                            }

                            szv64.DateFilling = DateTime.Parse(szv_6_4.Element("ДатаЗаполнения").Value.ToString());

                            db.AddToFormsSZV_6_4(szv64);
                            //                            db.SaveChanges();
                        }

                        #region Записи о стаже
                        if (szv_6_4.Descendants().Any(x => x.Name.LocalName == "СтажевыйПериод"))
                        {
                            //        db.SaveChanges();
                            var staj_osn_list = szv_6_4.Descendants().Where(x => x.Name.LocalName == "СтажевыйПериод");

                            int n = 0;
                            // если замена
                            //if (updateSZV_6_4_DDL.SelectedItem.Tag.ToString() == "2" && !updateStajSZV_6_4.Checked)
                            //{
                            //    long[] ids = db.StajOsn.Where(x => x.FormsSZV_6_4_ID == szv64.ID).Select(x => x.ID).ToArray();
                            //    if (ids.Count() > 0)
                            //    {
                            //        string list = String.Join(",", ids);
                            //        db.ExecuteStoreCommand(String.Format("DELETE FROM StajOsn WHERE ([ID] IN ({0}))", list));
                            //    }
                            //}
                            //else
                            //{
                            db.StajOsn.Count(x => x.FormsSZV_6_4_ID == szv64.ID);
                            //}

                            if (updateSZV_6_4_DDL.SelectedItem.Tag.ToString() != "2" || (updateSZV_6_4_DDL.SelectedItem.Tag.ToString() == "2" && !updateStajSZV_6_4.Checked))
                            {
                                foreach (var staj_osn in staj_osn_list)
                                {
                                    DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("ДатаНачалаПериода").Value.ToString());
                                    DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("ДатаКонцаПериода").Value.ToString());

                                    n++;
                                    StajOsn stajOsn = new StajOsn { DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };

                                    //      db.StajOsn.AddObject(stajOsn);

                                    var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");

                                    //перебираем льготный стаж если есть
                                    int i = 0;
                                    foreach (var item in staj_lgot_list)
                                    {
                                        string str = "";
                                        i++;
                                        var staj_lgot = item.Element("ОсобенностиУчета");
                                        StajLgot stajLgot = new StajLgot { Number = i };

                                        var terrUsl = staj_lgot.Element("ТерриториальныеУсловия");
                                        if (terrUsl != null) // если есть терр условия
                                        {
                                            //если есть запись в с таким кодом терр условий в базе
                                            str = terrUsl.Element("ОснованиеТУ").Value.ToString().ToUpper();
                                            if (TerrUsl_list.Any(x => x.Code.ToUpper() == str))
                                            {
                                                stajLgot.TerrUslID = TerrUsl_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                if (terrUsl.Element("Коэффициент") != null)
                                                    stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                                else
                                                    stajLgot.TerrUslKoef = 0;
                                            }
                                        }

                                        var osobUsl = staj_lgot.Element("ОсобыеУсловияТруда");
                                        if (osobUsl != null)
                                        {
                                            if (osobUsl.Element("ОснованиеОУТ") != null)
                                            {
                                                str = osobUsl.Element("ОснованиеОУТ").Value.ToString().ToUpper();
                                                if (OsobUslTruda_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.OsobUslTrudaID = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (osobUsl.Element("ПозицияСписка") != null)
                                            {
                                                str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                                if (KodVred_2_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == str);
                                                    stajLgot.KodVred_OsnID = kv.ID;

                                                    // проверка на наличие такой должности в базе
                                                    if (db.Dolgn.Any(x => x.Name == kv.Name))
                                                    {
                                                        stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                                    }
                                                    else
                                                    {
                                                        Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                        db.AddToDolgn(dolgn);
                                                        db.SaveChanges();
                                                        stajLgot.DolgnID = dolgn.ID;
                                                    }
                                                }
                                            }
                                        }

                                        var ischislStrahStaj = staj_lgot.Element("ИсчисляемыйСтаж");
                                        if (ischislStrahStaj != null)
                                        {
                                            if (ischislStrahStaj.Element("ОснованиеИС") != null)
                                            {
                                                str = ischislStrahStaj.Element("ОснованиеИС").Value.ToString().ToUpper();
                                                if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                            {
                                                if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                                {
                                                    stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                                }
                                                if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                                {
                                                    stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                                }
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                            {
                                                if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                                {
                                                    stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                                }
                                                if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                                {
                                                    stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                                }
                                            }
                                        }

                                        var ischislStrahStajDop = staj_lgot.Element("ДекретДети");
                                        if (ischislStrahStajDop != null)
                                        {
                                            str = ischislStrahStajDop.Value.ToString().ToUpper();
                                            if (IschislStrahStajDop_list.Any(x => x.Code.ToUpper() == str))
                                            {
                                                stajLgot.IschislStrahStajDopID = IschislStrahStajDop_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                            }
                                        }


                                        var uslDosrNazn = staj_lgot.Element("ВыслугаЛет");
                                        if (uslDosrNazn != null) // если есть терр условия
                                        {
                                            //если есть запись в с таким кодом терр условий в базе
                                            if (uslDosrNazn.Element("ОснованиеВЛ") != null)
                                            {
                                                str = uslDosrNazn.Element("ОснованиеВЛ").Value.ToString().ToUpper();
                                                if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                            {
                                                if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                                {
                                                    stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                                }
                                                if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                                {
                                                    stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                                }
                                            }
                                            if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                            {
                                                if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                                {
                                                    stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                                }
                                                if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                                {
                                                    stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                                }
                                            }
                                            if (uslDosrNazn.Element("ДоляСтавки") != null)
                                            {
                                                stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                            }
                                        }

                                        //db.StajLgot.AddObject(stajLgot);
                                        stajOsn.StajLgot.Add(stajLgot);
                                    }

                                    szv64.StajOsn.Add(stajOsn);

                                }
                            }

                            //    db.SaveChanges();

                        }


                        #endregion

                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        if (transactionCheckBox.Checked)
                        {
                            if (cnt_records_imported == 50 || cnt_records_imported == 100 || cnt_records_imported == 150)
                                db.SaveChanges();
                        }
                        else
                            db.SaveChanges();

                        cnt_records_imported++;
                        importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    catch (Exception ex)
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_6_4.Element("НомерВпачке").Value.ToString(), type = ex.Message + "\r\n" });
                    }

                    k++;

                    decimal temp = (decimal)k / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, k.ToString());

                } // перебор инд. сведений

                #endregion
                doc = null;
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));
                doc = null;
                result = false;
            }

            doc = null;

            return result;
        }

        /// <summary>
        /// Импорт файлов ПФР СЗВ-6
        /// </summary>
        /// <param name="XML_path"></param>
        /// <returns></returns>
        private bool ImportXML_SZV_6(GridViewRowInfo row)
        {
            string XML_path = row.Cells["path"].Value.ToString();
            doc = XDocument.Load(XML_path);

            doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach (var elem in doc.Descendants())
                elem.Name = elem.Name.LocalName;

            XElement node = doc.Root;
            bool result = true;

            try
            {
                #region // Поиск информации о составителе
                node = node.Descendants().First(x => x.Name.LocalName == "СоставительПачки");
                string inn = node.Element("НалоговыйНомер").Element("ИНН").Value;
                string kpp = node.Element("НалоговыйНомер").Element("КПП") != null ? node.Element("НалоговыйНомер").Element("КПП").Value.ToString() : "";
                byte type_ = inn.Length == 12 ? (byte)1 : (byte)0;

                string regnum = node.Element("РегистрационныйНомер").Value;

                while (regnum.Contains("-"))
                    regnum = regnum.Remove(regnum.IndexOf('-'), 1);

                string fullName = node.Element("НаименованиеОрганизации") != null ? node.Element("НаименованиеОрганизации").Value : "";
                string shortName = node.Element("НаименованиеКраткое") != null ? node.Element("НаименованиеКраткое").Value : "";


                Insurer insurer = new Insurer();

                if (db.Insurer.Any(x => x.RegNum == regnum))
                {
                    insurer = db.Insurer.First(x => x.RegNum == regnum);
                    if (updateInsData.Checked)
                    {
                        bool change = false;
                        if (type_ != insurer.TypePayer)
                        {
                            insurer.TypePayer = type_;
                            change = true;
                        }
                        if (type_ == 0)
                        {
                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                            if (insurer.Name != fullName)
                            {
                                insurer.Name = fullName;
                                change = true;
                            }
                        }
                        else
                        {
                            fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;
                            var fio = fullName.Split(' ');
                            if (fio.Count() > 0)
                            {
                                if (insurer.LastName != fio[0])
                                {
                                    insurer.LastName = fio[0];
                                    change = true;
                                }
                                if (fio.Count() > 1)
                                    if (insurer.FirstName != fio[1])
                                    {
                                        insurer.FirstName = fio[1];
                                        change = true;
                                    }
                                if (fio.Count() > 2)
                                {
                                    insurer.MiddleName = "";
                                    for (int i = 2; i < fio.Count(); i++)
                                    {
                                        insurer.MiddleName = insurer.MiddleName + fio[i];
                                    }
                                }
                            }
                            if (insurer.NameShort != shortName)
                            {
                                insurer.NameShort = shortName;
                                change = true;
                            }
                        }

                        if (insurer.INN != inn)
                        {
                            insurer.INN = inn;
                            change = true;
                        }
                        if (insurer.KPP != kpp)
                        {
                            insurer.KPP = kpp;
                            change = true;
                        }

                        if (change)
                        {
                            db.ObjectStateManager.ChangeObjectState(insurer, EntityState.Modified);
                            db.SaveChanges();
                        }
                    }

                }
                else
                {
                    insurer.TypePayer = type_;
                    fullName = !String.IsNullOrEmpty(fullName) ? fullName : shortName;
                    if (type_ == 0)
                    {
                        insurer.NameShort = shortName;
                        insurer.Name = fullName;
                    }
                    else
                    {
                        var fio = fullName.Split(' ');
                        if (fio.Count() > 0)
                        {
                            insurer.LastName = fio[0];
                            if (fio.Count() > 1)
                                insurer.FirstName = fio[1];
                            if (fio.Count() > 2)
                            {
                                for (int i = 2; i < fio.Count(); i++)
                                {
                                    insurer.MiddleName = insurer.MiddleName + fio[i];
                                }
                            }
                        }
                        insurer.NameShort = shortName;
                    }
                    insurer.RegNum = regnum;
                    insurer.INN = inn;
                    insurer.KPP = kpp;
                    db.AddToInsurer(insurer);
                    db.SaveChanges();
                }
                #endregion


                #region перебор инд.сведений
                var szv_6_list = doc.Root.Descendants().Where(x => x.Name.LocalName == "СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ");

                int count = szv_6_list.Count();

                //             secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = count.ToString(); }));

                int k = 0;
                List<staffContainer> staffList = new List<staffContainer>();
                foreach (var szv_6 in szv_6_list)
                {
                    var snils = Utils.ParseSNILS_XML(szv_6.Element("СтраховойНомер") != null ? szv_6.Element("СтраховойНомер").Value.ToString().ToString() : "", true);

                    string middleName = szv_6.Element("ФИО").Element("Отчество") != null ? szv_6.Element("ФИО").Element("Отчество").Value.ToString() : "";

                    var st = new staffContainer
                    {
                        insuranceNum = snils.num,
                        insID = insurer.ID,
                        lastName = szv_6.Element("ФИО").Element("Фамилия").Value.ToString(),
                        firstName = szv_6.Element("ФИО").Element("Имя").Value.ToString(),
                        middleName = middleName,
                        contrNum = snils.contrNum
                    };

                    if (!staffList.Any(x => x.insuranceNum == st.insuranceNum))
                        staffList.Add(st);
                }

                var listid = staffList.Select(y => y.insuranceNum).ToList();
                var listNum = db.Staff.Where(x => x.InsurerID == insurer.ID && listid.Contains(x.InsuranceNumber)).Select(x => x.InsuranceNumber).ToList();

                staffList = staffList.Where(x => !listNum.Contains(x.insuranceNum)).ToList();  // те сотрудники которых нет в базе для текущего страхователя

                foreach (var item in staffList)
                {
                    Staff staff_ = new Staff
                    {
                        InsuranceNumber = item.insuranceNum.PadLeft(9, '0'),
                        InsurerID = item.insID,
                        LastName = item.lastName,
                        FirstName = item.firstName,
                        MiddleName = item.middleName,
                        ControlNumber = item.contrNum,
                        Dismissed = 0
                    };

                    db.AddToStaff(staff_);
                }
                if (staffList.Count > 0)
                    db.SaveChanges();

                //                long platCategoryRaschPerID = 4;  // Для категорий после 2010



                foreach (var szv_6 in szv_6_list)
                {
                    try
                    {
                        string InsuranceNum = Utils.ParseSNILS_XML(szv_6.Element("СтраховойНомер") != null ? szv_6.Element("СтраховойНомер").Value.ToString().ToString() : "", false).num;

                        Staff staff = db.Staff.FirstOrDefault(x => x.InsuranceNumber == InsuranceNum && x.InsurerID == insurer.ID);

                        if (updateStaffData.Checked) // если выбрано обнослять данные в базе при расхождении
                        {
                            bool change = false;
                            if (staff.LastName != szv_6.Element("ФИО").Element("Фамилия").Value.ToString())
                            {
                                staff.LastName = szv_6.Element("ФИО").Element("Фамилия").Value.ToString();
                                change = true;
                            }
                            if (staff.FirstName != szv_6.Element("ФИО").Element("Имя").Value.ToString())
                            {
                                staff.FirstName = szv_6.Element("ФИО").Element("Имя").Value.ToString();
                                change = true;
                            }
                            if (szv_6.Element("ФИО").Element("Отчество") != null)
                                if (staff.MiddleName != szv_6.Element("ФИО").Element("Отчество").Value.ToString())
                                {
                                    staff.MiddleName = szv_6.Element("ФИО").Element("Отчество").Value.ToString();
                                    change = true;
                                }

                            if (change)
                            {
                                db.ObjectStateManager.ChangeObjectState(staff, EntityState.Modified);
                                //   db.SaveChanges();
                            }
                        }

                        if (String.IsNullOrEmpty(InsuranceNum))
                            errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_6.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке Индивидуальных сведений. Не указан Страховой номер (СНИЛС) сотрудника.\r\n" });

                        string tInfo = szv_6.Element("ТипСведений").Value.ToString().ToLower();
                        long tInfoID = typeInfo_.First(x => x.Name.ToLower() == tInfo).ID;
                        string pcCode = szv_6.Element("КодКатегории").Value.ToString().ToUpper();

                        //                        if (!db.PlatCategory.Any(x => x.PlatCategoryRaschPerID == platCategoryRaschPerID && x.Code == pcCode))
                        if (!PlatCatList.Any(x => x.Code == pcCode))
                        {
                            if (String.IsNullOrEmpty(pcCode))
                            {
                                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_6.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Не указан Код Категории Плательщика.\r\n" });
                            }
                            else
                            {
                                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_6.Element("НомерВпачке").Value.ToString(), type = staff.LastName + " " + staff.FirstName + " " + staff.MiddleName + ". Ошибка при загрузке данных Раздела 6.4. Указан Код Категории Плательщика - \"" + pcCode + "\", который отсутствует в списке доступных категорий в справочнике.\r\n" });
                            }
                            break;
                        }

                        //                        long pcID = db.PlatCategory.FirstOrDefault(x => x.PlatCategoryRaschPerID == platCategoryRaschPerID && x.Code == pcCode).ID;
                        long pcID = PlatCatList.FirstOrDefault(x => x.Code == pcCode).ID;

                        byte q = byte.Parse(szv_6.Element("ОтчетныйПериод").Element("Квартал").Value.ToString());
                        short y = short.Parse(szv_6.Element("ОтчетныйПериод").Element("Год").Value.ToString());


                        FormsSZV_6 szv6 = new FormsSZV_6();
                        szv6.AutoCalc = true;
                        bool exist = false;
                        byte qk = (byte)0;
                        short yk = (short)0;

                        if (tInfo == "исходная")
                        {
                            if (db.FormsSZV_6.Any(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.PlatCategoryID == pcID))
                            {
                                szv6 = db.FormsSZV_6.FirstOrDefault(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.PlatCategoryID == pcID);
                                if (updateSZV_6_DDL.SelectedItem.Tag.ToString() == "0")
                                {
                                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_6 WHERE ([ID] = {0})", szv6.ID));
                                    szv6 = new FormsSZV_6();
                                }
                                else
                                    exist = true;
                            }
                        }
                        else
                        {
                            qk = byte.Parse(szv_6.Element("КорректируемыйОтчетныйПериод").Element("Квартал").Value.ToString());
                            yk = short.Parse(szv_6.Element("КорректируемыйОтчетныйПериод").Element("Год").Value.ToString());

                            if (db.FormsSZV_6.Any(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.YearKorr == yk && x.QuarterKorr == qk && x.PlatCategoryID == pcID))
                            {
                                szv6 = db.FormsSZV_6.FirstOrDefault(x => x.StaffID == staff.ID && x.InsurerID == insurer.ID && x.Year == y && x.Quarter == q && x.TypeInfoID == tInfoID && x.YearKorr == yk && x.QuarterKorr == qk && x.PlatCategoryID == pcID);
                                if (updateSZV_6_DDL.SelectedItem.Tag.ToString() == "0")
                                {
                                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsSZV_6 WHERE ([ID] = {0})", szv6.ID));
                                    szv6 = new FormsSZV_6();
                                }
                                else
                                    exist = true;
                            }
                        }

                        if (exist) // если такая запись уже существует
                        {
                            if (updateSZV_6_DDL.SelectedItem.Tag.ToString() == "2") // объединение
                            {
                                if (updateSumFeeSZV_6.Checked) // начисленные взносы
                                {
                                    szv6.SumFeePFR_Strah = szv6.SumFeePFR_Strah + decimal.Parse(szv_6.Element("СуммаВзносовНаСтраховую").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                                    szv6.SumFeePFR_Nakop = szv6.SumFeePFR_Nakop + decimal.Parse(szv_6.Element("СуммаВзносовНаНакопительную").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                                }
                                if (updatePayFeeSZV_6.Checked) // уплаченные взносы
                                {
                                    szv6.SumPayPFR_Strah = szv6.SumPayPFR_Strah + decimal.Parse(szv_6.Element("СуммаВзносовНаСтраховую").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                                    szv6.SumPayPFR_Nakop = szv6.SumPayPFR_Nakop + decimal.Parse(szv_6.Element("СуммаВзносовНаНакопительную").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                                }

                                db.ObjectStateManager.ChangeObjectState(szv6, EntityState.Modified);
                                //     db.SaveChanges();
                            }
                            else if (updateSZV_6_DDL.SelectedItem.Tag.ToString() == "1") // не загружать импортируемую форму
                            {
                                continue;
                            }
                        }
                        else // если такой записи нет то добавляем ее
                        {
                            szv6.InsurerID = insurer.ID;
                            szv6.StaffID = staff.ID;
                            szv6.Year = y;
                            szv6.Quarter = q;
                            szv6.PlatCategoryID = pcID;

                            if (yk != 0)
                            {
                                szv6.YearKorr = yk;
                                szv6.QuarterKorr = qk;
                            }
                            szv6.TypeInfoID = tInfoID;

                            if (szv_6.Element("СуммаВзносовНаСтраховую") != null)
                            {
                                szv6.SumFeePFR_Strah = decimal.Parse(szv_6.Element("СуммаВзносовНаСтраховую").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                                szv6.SumPayPFR_Strah = decimal.Parse(szv_6.Element("СуммаВзносовНаСтраховую").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                szv6.SumFeePFR_Strah = 0;
                                szv6.SumPayPFR_Strah = 0;
                            }
                            if (szv_6.Element("СуммаВзносовНаНакопительную") != null)
                            {
                                szv6.SumFeePFR_Nakop = decimal.Parse(szv_6.Element("СуммаВзносовНаНакопительную").Element("Начислено").Value.ToString(), CultureInfo.InvariantCulture);
                                szv6.SumPayPFR_Nakop = decimal.Parse(szv_6.Element("СуммаВзносовНаНакопительную").Element("Уплачено").Value.ToString(), CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                szv6.SumFeePFR_Nakop = 0;
                                szv6.SumPayPFR_Nakop = 0;
                            }

                            szv6.DateFilling = DateTime.Parse(szv_6.Element("ДатаЗаполнения").Value.ToString());

                            db.AddToFormsSZV_6(szv6);
                            //                            db.SaveChanges();
                        }

                        #region Записи о стаже
                        if (szv_6.Descendants().Any(x => x.Name.LocalName == "СтажевыйПериод"))
                        {
                            var staj_osn_list = szv_6.Descendants().Where(x => x.Name.LocalName == "СтажевыйПериод");

                            // если замена
                            int n = 0;

                            //if (updateSZV_6_DDL.SelectedItem.Tag.ToString() == "2" && !updateStajSZV_6.Checked)
                            //{
                            //    long[] ids = db.StajOsn.Where(x => x.FormsSZV_6_ID == szv6.ID).Select(x => x.ID).ToArray();
                            //    if (ids.Count() > 0)
                            //    {
                            //        string list = String.Join(",", ids);
                            //        db.ExecuteStoreCommand(String.Format("DELETE FROM StajOsn WHERE ([ID] IN ({0}))", list));
                            //    }
                            //}
                            //else
                            //{
                            n = db.StajOsn.Count(x => x.FormsSZV_6_ID == szv6.ID);
                            //                            }
                            if (updateSZV_6_DDL.SelectedItem.Tag.ToString() != "2" || (updateSZV_6_DDL.SelectedItem.Tag.ToString() == "2" && !updateStajSZV_6.Checked))
                            {
                                foreach (var staj_osn in staj_osn_list)
                                {
                                    DateTime dateStartStajOsn = DateTime.Parse(staj_osn.Element("ДатаНачалаПериода").Value.ToString());
                                    DateTime dateEndStajOsn = DateTime.Parse(staj_osn.Element("ДатаКонцаПериода").Value.ToString());

                                    n++;
                                    StajOsn stajOsn = new StajOsn { DateBegin = dateStartStajOsn, DateEnd = dateEndStajOsn, Number = n };

                                    //db.StajOsn.AddObject(stajOsn);
                                    //db.SaveChanges();

                                    var staj_lgot_list = staj_osn.Descendants().Where(x => x.Name.LocalName == "ЛьготныйСтаж");

                                    //перебираем льготный стаж если есть
                                    int i = 0;
                                    foreach (var item in staj_lgot_list)
                                    {
                                        string str = "";
                                        i++;
                                        var staj_lgot = item.Element("ОсобенностиУчета");
                                        StajLgot stajLgot = new StajLgot { Number = i };

                                        var terrUsl = staj_lgot.Element("ТерриториальныеУсловия");
                                        if (terrUsl != null) // если есть терр условия
                                        {
                                            //если есть запись в с таким кодом терр условий в базе
                                            str = terrUsl.Element("ОснованиеТУ").Value.ToString().ToUpper();
                                            if (TerrUsl_list.Any(x => x.Code.ToUpper() == str))
                                            {
                                                stajLgot.TerrUslID = TerrUsl_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                if (terrUsl.Element("Коэффициент") != null)
                                                    stajLgot.TerrUslKoef = !String.IsNullOrEmpty(terrUsl.Element("Коэффициент").Value.ToString()) ? decimal.Parse(terrUsl.Element("Коэффициент").Value.ToString(), CultureInfo.InvariantCulture) : 0;
                                                else
                                                    stajLgot.TerrUslKoef = 0;
                                            }
                                        }

                                        var osobUsl = staj_lgot.Element("ОсобыеУсловияТруда");
                                        if (osobUsl != null)
                                        {
                                            if (osobUsl.Element("ОснованиеОУТ") != null)
                                            {
                                                str = osobUsl.Element("ОснованиеОУТ").Value.ToString().ToUpper();
                                                if (OsobUslTruda_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.OsobUslTrudaID = OsobUslTruda_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (osobUsl.Element("ПозицияСписка") != null)
                                            {
                                                str = osobUsl.Element("ПозицияСписка").Value.ToString().ToUpper();
                                                if (KodVred_2_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    KodVred_2 kv = KodVred_2_list.FirstOrDefault(x => x.Code.ToUpper() == str);
                                                    stajLgot.KodVred_OsnID = kv.ID;

                                                    // проверка на наличие такой должности в базе
                                                    if (db.Dolgn.Any(x => x.Name == kv.Name))
                                                    {
                                                        stajLgot.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == kv.Name).ID;
                                                    }
                                                    else
                                                    {
                                                        Dolgn dolgn = new Dolgn { Name = kv.Name };
                                                        db.AddToDolgn(dolgn);
                                                        db.SaveChanges();
                                                        stajLgot.DolgnID = dolgn.ID;
                                                    }
                                                }
                                            }
                                        }

                                        var ischislStrahStaj = staj_lgot.Element("ИсчисляемыйСтаж");
                                        if (ischislStrahStaj != null)
                                        {
                                            if (ischislStrahStaj.Element("ОснованиеИС") != null)
                                            {
                                                str = ischislStrahStaj.Element("ОснованиеИС").Value.ToString().ToUpper();
                                                if (IschislStrahStajOsn_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.IschislStrahStajOsnID = IschislStrahStajOsn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаВчасах") != null)
                                            {
                                                if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы") != null)
                                                {
                                                    stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                                }
                                                if (ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты") != null)
                                                {
                                                    stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                                }
                                            }
                                            if (ischislStrahStaj.Element("ВыработкаКалендарная") != null)
                                            {
                                                if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                                {
                                                    stajLgot.Strah1Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                                }
                                                if (ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                                {
                                                    stajLgot.Strah2Param = long.Parse(ischislStrahStaj.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                                }
                                            }
                                        }

                                        var ischislStrahStajDop = staj_lgot.Element("ДекретДети");
                                        if (ischislStrahStajDop != null)
                                        {
                                            str = ischislStrahStajDop.Value.ToString().ToUpper();
                                            if (IschislStrahStajDop_list.Any(x => x.Code.ToUpper() == str))
                                            {
                                                stajLgot.IschislStrahStajDopID = IschislStrahStajDop_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                            }
                                        }


                                        var uslDosrNazn = staj_lgot.Element("ВыслугаЛет");
                                        if (uslDosrNazn != null) // если есть терр условия
                                        {
                                            //если есть запись в с таким кодом терр условий в базе
                                            if (uslDosrNazn.Element("ОснованиеВЛ") != null)
                                            {
                                                str = uslDosrNazn.Element("ОснованиеВЛ").Value.ToString().ToUpper();
                                                if (UslDosrNazn_list.Any(x => x.Code.ToUpper() == str))
                                                {
                                                    stajLgot.UslDosrNaznID = UslDosrNazn_list.FirstOrDefault(x => x.Code.ToUpper() == str).ID;
                                                }
                                            }
                                            if (uslDosrNazn.Element("ВыработкаВчасах") != null)
                                            {
                                                if (uslDosrNazn.Element("ВыработкаВчасах").Element("Часы") != null)
                                                {
                                                    stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Часы").Value.ToString());
                                                }
                                                if (uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты") != null)
                                                {
                                                    stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаВчасах").Element("Минуты").Value.ToString());
                                                }
                                            }
                                            if (uslDosrNazn.Element("ВыработкаКалендарная") != null)
                                            {
                                                if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы") != null)
                                                {
                                                    stajLgot.UslDosrNazn1Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеМесяцы").Value.ToString());
                                                }
                                                if (uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни") != null)
                                                {
                                                    stajLgot.UslDosrNazn2Param = long.Parse(uslDosrNazn.Element("ВыработкаКалендарная").Element("ВсеДни").Value.ToString());
                                                }
                                            }
                                            if (uslDosrNazn.Element("ДоляСтавки") != null)
                                            {
                                                stajLgot.UslDosrNazn3Param = decimal.Parse(uslDosrNazn.Element("ДоляСтавки").Value.ToString(), CultureInfo.InvariantCulture);
                                            }
                                        }

                                        //db.StajLgot.AddObject(stajLgot);
                                        stajOsn.StajLgot.Add(stajLgot);
                                    }

                                    szv6.StajOsn.Add(stajOsn);
                                }
                            }


                            //                           db.SaveChanges();

                        }

                        #endregion


                        if (bw.CancellationPending)
                        {
                            return false;
                        }

                        cnt_records_imported++;

                        if (transactionCheckBox.Checked)
                        {
                            if (cnt_records_imported == 50 || cnt_records_imported == 100 || cnt_records_imported == 150)
                                db.SaveChanges();
                        }
                        else
                            db.SaveChanges();

                        importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    catch (Exception ex)
                    {
                        errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = szv_6.Element("НомерВпачке").Value.ToString(), type = ex.Message + "\r\n" });
                    }

                    k++;

                    decimal temp = (decimal)k / (decimal)count;
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, k.ToString());

                } // перебор инд. сведений
                #endregion

                doc = null;
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex)
            {
                errList.Add(new ErrList { name = importFilesGrid.Rows[row.Index].Cells[2].Value.ToString(), control = "", type = ex.Message + "\r\n" });
                this.Invoke(new Action(() => { Methods.showAlert("Ошибка импорта", "В процессе импорта файла - " + XML_path + "  произошла ошибка.\r\n" + ex.Message, this.ThemeName); }));
                doc = null;
                result = false;
            }

            doc = null;

            return result;
        }


        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void importing(object sender, DoWorkEventArgs e)
        {
            var row = (GridViewRowInfo)e.Argument;
            db = new pu6Entities();
            switch (row.Cells["type"].Value.ToString())
            {
                case "РСВ 2014":
                    //                        secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = "1"; }));
                    if (ImportXML_RSW1_2014(row))
                    {
                        cnt++;
                        //            firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "1"; }));
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; importFilesGrid.Rows[row.Index].Cells[7].Value = 1; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = 0; }));
                    }
                    break;
                case "РСВ 2015":
                    //                        secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = "1"; }));
                    if (ImportXML_RSW1_2014(row))
                    {
                        cnt++;
                        //        firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "1"; }));
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; importFilesGrid.Rows[row.Index].Cells[7].Value = 1; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = 0; }));
                    }
                    break;
                case "ПФР 2014":
                    if (ImportXML_RSW1_6_1_2014(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "СПВ-2 2014":
                    if (ImportXML_SPV_2_2014(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "РСВ-2 2014":
                    if (ImportXML_RSW2_2014(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; importFilesGrid.Rows[row.Index].Cells[7].Value = 1; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = 0; }));
                    }
                    break;
                case "РСВ-2 2015":
                    if (ImportXML_RSW2_2015(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; importFilesGrid.Rows[row.Index].Cells[7].Value = 1; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = 0; }));
                    }
                    break;
                case "РВ-3 2015":
                    if (ImportXML_RW3_2015(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; importFilesGrid.Rows[row.Index].Cells[7].Value = 1; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = 0; }));
                    }
                    break;
                case "СЗВ-М 2016":
                    if (ImportXML_SZVM_2016(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "ДСВ-3":
                    if (ImportXML_DSW3(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "ОДВ-1 СЗВ-СТАЖ":
                    if (ImportXML_SZV_STAJ(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "ОДВ-1 СЗВ-ИСХ":
                    if (ImportXML_SZV_ISH(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "ОДВ-1 СЗВ-КОРР":
                    if (ImportXML_SZV_KORR(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "ЗАГС РОЖД":
                    if (ImportXML_ZAGS_Born(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "ЗАГС СМЕРТ":
                    if (ImportXML_ZAGS_Death(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "СЗВ-6-4":
                    if (ImportXML_SZV_6_4(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
                case "СЗВ-6":
                    if (ImportXML_SZV_6(row))
                    {
                        cnt++;
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.LimeGreen; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    else
                    {
                        if (!bw.CancellationPending)
                            importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Red; importFilesGrid.Rows[row.Index].Cells[7].Value = cnt_records_imported; }));
                    }
                    break;
            }

            //     doc = null;
        }


        private void importBtn_Click(object sender, EventArgs e)
        {
            d = DateTime.Now;
            errList = new List<ErrList>();
            db = new pu6Entities();

            cnt = 0;
            selectFilesBtn.Enabled = false;
            selectFolderBtn.Enabled = false;
            importBtn.Enabled = false;
            closeBtn.Visible = false;
            errorListBtn.Visible = false;
            abortBtn.Location = closeBtn.Location;
            abortBtn.Visible = true;
            importFilesGrid.ReadOnly = false;
            progressPanel.Visible = true;
            firstPartLabel.Visible = true;
            firstPartLabel_.Visible = true;
            secondPartLabel.Visible = true;
            secondPartLabel_.Visible = true;


            PlatCatList = db.PlatCategory.Where(x => x.PlatCategoryRaschPerID == 4).ToList();
            TerrUsl_list = db.TerrUsl.ToList();
            OsobUslTruda_list = db.OsobUslTruda.ToList();
            KodVred_2_list = db.KodVred_2.ToList();
            IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
            IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
            UslDosrNazn_list = db.UslDosrNazn.ToList();
            SpecOcenkaUslTruda_list = db.SpecOcenkaUslTruda.ToList();
            typeInfo_ = db.TypeInfo.ToList();


            bwPre = new BackgroundWorker();
            bwPre.WorkerReportsProgress = true;
            bwPre.WorkerSupportsCancellation = true;
            bwPre.DoWork += new System.ComponentModel.DoWorkEventHandler(preImport);
            bwPre.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bwPre.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bwPre_ProgressChanged);
            bwPre.RunWorkerAsync();
        }


        private void preImport(object sender, DoWorkEventArgs e)
        {

            foreach (var row in importFilesGrid.Rows)
            {
                importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].Cells[0].Value = Color.SkyBlue; importFilesGrid.Rows[row.Index].Cells["cntImported"].Value = 0; }));
            }
            secondPartLabel.Invoke(new Action(() => { secondPartLabel.Text = importFilesGrid.RowCount.ToString(); }));
            firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));
            radProgressBar1.Invoke(new Action(() => { radProgressBar1.Value1 = 0; }));

            int k = 0;
            foreach (var row in importFilesGrid.Rows)
            {
                //       lll++;

                cnt_records_imported = 0;

                rowIndex = row.Index;

                if (bwPre.CancellationPending)
                {
                    return;
                }
                //           firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = "0"; }));
                importFilesGrid.Invoke(new Action(() => { importFilesGrid.Rows[row.Index].IsCurrent = true; importFilesGrid.TableElement.ScrollToRow(importFilesGrid.Rows[row.Index]); importFilesGrid.Rows[row.Index].Cells[0].Value = Color.Orange; }));

                //   
                bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(importing);
                //                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                //                bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);
                bw.RunWorkerAsync(row);
                while (bw.IsBusy)
                {
                    Thread.Sleep(100);
                    Application.DoEvents();
                }

                k++;
                //bw = null;
                bw.Dispose();
                decimal temp = (decimal)k / (decimal)importFilesGrid.RowCount;
                int proc = (int)Math.Round((temp * 100), 0);
                bwPre.ReportProgress(proc, k.ToString());

            }


        }


        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (cancel_work == false)
            {
                firstPartLabel.Visible = false;
                firstPartLabel_.Visible = false;
                secondPartLabel.Visible = false;
                secondPartLabel_.Visible = false;
                progressPanel.Visible = false;
                selectFilesBtn.Enabled = true;
                selectFolderBtn.Enabled = true;
                importBtn.Enabled = true;
                closeBtn.Visible = true;
                abortBtn.Visible = false;
                importFilesGrid.ReadOnly = true;

                if (errList.Count() > 0)
                    errorListBtn.Visible = true;

                RadMessageBox.Show(this, "Успешно загружено " + cnt.ToString() + " файлов.", "Результат импорта файлов", MessageBoxButtons.OK, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
            }
        }

        void bwPre_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            radProgressBar1.Invoke(new Action(() => { radProgressBar1.Value1 = e.ProgressPercentage; }));
            firstPartLabel.Invoke(new Action(() => { firstPartLabel.Text = e.UserState.ToString(); }));
        }




        private void updateIndSved_DDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (updateIndSved_DDL.SelectedItem.Tag.ToString() == "2")
            {
                paramsUpdateIndSved.Enabled = true;
            }
            else
            {
                paramsUpdateIndSved.Enabled = false;
            }
        }

        private void ImportXML_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bwPre.IsBusy)
                if (RadMessageBox.Show("Вы уверены, что хотите закрыть окно и прервать импорт данных?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    cancel_work = true;
                    bw.CancelAsync();
                    bwPre.CancelAsync();
                    while (bwPre.IsBusy)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                }
                else
                {
                    e.Cancel = true;
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
                control = "updateInsData",
                value = updateInsData.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateStaffData",
                value = updateStaffData.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "transactionCheckBox",
                value = transactionCheckBox.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateSPW2_DDL",
                value = updateSPW2_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "updateIndSved_DDL",
                value = updateIndSved_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "updateSZV_6_DDL",
                value = updateSZV_6_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "updateSZV_6_4_DDL",
                value = updateSZV_6_4_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "updateSZVM_DDL",
                value = updateSZVM_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "updateDSW3_DDL",
                value = updateDSW3_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "updateSumFeeSZV_6",
                value = updateSumFeeSZV_6.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateStajSZV_6",
                value = updateStajSZV_6.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updatePayFeeSZV_6",
                value = updatePayFeeSZV_6.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateSumFeeSZV_6_4",
                value = updateSumFeeSZV_6_4.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateSumFL_SZV_6_4",
                value = updateSumFL_SZV_6_4.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateSumDop_SZV_6_4",
                value = updateSumDop_SZV_6_4.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updatePayFeeSZV_6_4",
                value = updatePayFeeSZV_6_4.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateStajSZV_6_4",
                value = updateStajSZV_6_4.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateSumFee",
                value = updateSumFee.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateRazd_6_4",
                value = updateRazd_6_4.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateRazd_6_7",
                value = updateRazd_6_7.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateStaj",
                value = updateStaj.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updateODV_1_DDL",
                value = updateODV_1_DDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "updateStajSZV_STAJ",
                value = updateStajSZV_STAJ.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "updatePayFeeSZV_STAJ",
                value = updatePayFeeSZV_STAJ.Checked ? "true" : "false"
            });


            props.setFormParams(this, windowData);

        }


        private void updateSZV_6_DDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (updateSZV_6_DDL.SelectedItem.Tag.ToString() == "2")
            {
                paramsUpdateSZV_6.Enabled = true;
            }
            else
            {
                paramsUpdateSZV_6.Enabled = false;
            }
        }

        private void updateSZV_6_4_DDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (updateSZV_6_4_DDL.SelectedItem.Tag.ToString() == "2")
            {
                paramsUpdateSZV_6_4.Enabled = true;
            }
            else
            {
                paramsUpdateSZV_6_4.Enabled = false;
            }
        }

        private void abortBtn_Click(object sender, EventArgs e)
        {
            if (bwPre.IsBusy)
                if (RadMessageBox.Show("Вы уверены, что хотите прервать импорт данных?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    bw.CancelAsync();
                    bwPre.CancelAsync();
                }
        }

        private void errorListBtn_Click(object sender, EventArgs e)
        {
            Telerik.WinControls.UI.RadForm child = new Telerik.WinControls.UI.RadForm();
            child.ThemeName = this.ThemeName;
            child.StartPosition = FormStartPosition.CenterScreen;
            child.Size = new Size(760, 560);
            child.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            child.ShowInTaskbar = false;
            child.Text = "Просмотр журнала ошибок импорта XML-файлов ПФР";
            string text = string.Empty;
            if (errList.Count() == 0)
                text = "В ходе импорта данных из XML-файлов ПФР ошибок не выявлено!";
            foreach (var item in errList)
            {
                text = text + item.name + "\r\nНомер в пачке " + item.control.PadRight(10, ' ') + item.type + "\r\n";
            }

            Telerik.WinControls.UI.RadTextBox textBox = new Telerik.WinControls.UI.RadTextBox();
            textBox.Name = "errorTextBox";
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
            textBox.Text = text;
            textBox.SelectionStart = text.Length;
            textBox.ReadOnly = true;
            child.Controls.Add(textBox);
            ((Telerik.WinControls.UI.RadTextBoxElement)((child.Controls["errorTextBox"] as Telerik.WinControls.UI.RadTextBox).GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(58)))), ((int)(((byte)(120)))));
            child.ShowDialog();
        }

        private void importFilesGrid_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //if (e.CellElement.ColumnInfo.Name == "cntImported" && rowIndex == e.RowIndex)
            //{
            //    int maxValue = 100;
            //    Int32.TryParse(importFilesGrid.Rows[e.RowIndex].Cells["cntDoc"].Value.ToString(), out maxValue);

            //    RadProgressBarElement progressBarElement;
            //    if (e.CellElement.Children.Count == 0)
            //    {
            //        progressBarElement = new RadProgressBarElement();
            //        e.CellElement.Children.Add(progressBarElement);
            //        progressBarElement.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //        progressBarElement.Maximum = maxValue;
            //    }
            //    else
            //    {
            //        progressBarElement = e.CellElement.Children[0] as RadProgressBarElement;
            //    }
            //    progressBarElement.Margin = new Padding(5);
            //    progressBarElement.StretchHorizontally = true;
            //    progressBarElement.Text = ((GridDataCellElement)e.CellElement).Value.ToString();
            //    int value = 0;
            //    if (e.CellElement.Value != null)
            //    {
            //        try
            //        {
            //            Int32.TryParse(((GridDataCellElement)e.CellElement).Value.ToString(), out value);
            //        }
            //        catch
            //        {
            //            value = 0;
            //        }
            //    }
            //    if (value < 0)
            //    {
            //        value = 0;
            //    }
            //    else if (value > maxValue)
            //    {
            //        value = maxValue;
            //    }
            //    progressBarElement.Value1 = value;
            //} 

        }

        private void updateODV_1_DDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (updateODV_1_DDL.SelectedItem.Tag.ToString() == "2")
            {
                paramsUpdateSZV_STAJ.Enabled = true;
            }
            else
            {
                paramsUpdateSZV_STAJ.Enabled = false;
            }
        }



    }
}
