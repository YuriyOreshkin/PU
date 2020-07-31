using System;
using System.Xml.Serialization;//Надо добавить для работы класса
using System.IO;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;
using PU.Models;
using System.Windows.Forms;
using System.Linq;


namespace PU.Classes
{
    public static class Options
    {
        public static long InsID { get; set; }
        public static UserAccess.Users User { get; set; }
        public static MROT mrot { get; set; }
        public static string ThemeName { get; set; }
        public static DBEntry DBActual = new DBEntry { };
        public static RKASV RKASV = new RKASV { };
        public static byte inputTypeRSW1 { get; set; }
        public static byte inputTypeRSW2 { get; set; }
        public static bool fixCurrentInsurer { get; set; }
        public static bool getInsurerFIOtoNewForm { get; set; }
        public static bool saveLastPackNum { get; set; }
        public static bool printAllPagesRSV1 { get; set; }
        public static List<SimpleList> FilledBaseArr { get; set; }
        public static List<RaschetPeriodContainer> RaschetPeriodInternal { get; set; }
        public static List<RaschetPeriodContainer> RaschetPeriodInternal2017 { get; set; }
        public static List<RaschetPeriodContainer> RaschetPeriodInternal2010_2013 { get; set; }
        public static List<RaschetPeriodContainer> RaschetPeriodInternal1996_2009 { get; set; }
        public static List<FormParams> formParams { get; set; }
        public static List<GridLayouts> gridParams { get; set; }
        public static List<InsurerImportExportPath> InsurerFolders { get; set; }
        public static InsurerImportExportPath CurrentInsurerFolders = new InsurerImportExportPath { };
        public static string pu6conn { get; set; }
        public static string settingsFilePath { get; set; }
        public static bool hideDialogCheckFiles { get; set; }
        public static bool checkFilesAfterSaving { get; set; }
        public static string pathCheckPfr { get; set; }
    }


    //Класс определяющий какие настройки есть в программе
    public class PropsFields
    {
        //Чтобы добавить настройку в программу просто добавьте суда строку вида -
        //public ТИП ИМЯ_ПЕРЕМЕННОЙ = значение_переменной_по_умолчанию;
        public String ThemeName = "Windows8";
        public long InsurerID = 0;
        public byte inputTypeRSW1 = 0;
        public bool fixCurrentInsurer = true;
        public bool getInsurerFIOtoNewForm = true;
        public bool saveLastPackNum = false;
        public bool printAllPagesRSV1 = false;
        public bool autoCheckNewVersion = true;
        public bool dbJournal_modeWAL = true;
        public bool hideDialogCheckFiles = false;
        public bool checkFilesAfterSaving = true;
        public string pathCheckPfr = "C:\\CheckPfr";
        //public bool useRSW1_2015 = true;
//        public RKASV RKASV = new RKASV { opfrCode = "007", port = "9080", url = "http://10.7.0.49", service = "/asvWeb/vio/getInsurer" };
        public RKASV RKASV = new RKASV {}; // для страхователей
        public string xaccessLastName = "Администратор";
        public string xaccessPath = (Path.Combine(Application.StartupPath, "xaccess.db3"));
        public backup Backup = new backup { autoBackup = new backupSettings { active = true, value = 3 }, maxCount = new backupSettings { active = false, value = 10 }, pathLast = Application.StartupPath + "\\Архивы БД" };
        public List<DBEntry> DBList = new List<DBEntry> { };
        public List<InsurerImportExportPath> InsurerFolders = new List<InsurerImportExportPath> { };
        public List<InsurersLastNums> LastPackNumber = new List<InsurersLastNums> { };
        public List<FormParams> formParams = new List<FormParams> { };
        public List<GridLayouts> gridParams = new List<GridLayouts> { };
    }

    //Класс работы с настройками
    public class Props
    {
        private string XMLFileName = "settings.xml";

        public PropsFields Fields;

        public Props()
        {
            Fields = new PropsFields();
        }

        //Запись настроек в файл
        public void WriteXml()
        {
            string filePath = !String.IsNullOrEmpty(Options.settingsFilePath) ? Options.settingsFilePath : (Application.StartupPath + "\\" + XMLFileName);

            XmlSerializer ser = new XmlSerializer(typeof(PropsFields));
            try
            {
                TextWriter writer = new StreamWriter(filePath);
                ser.Serialize(writer, Fields);
                writer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось записать файл с настройками. Код ошибки: " + ex.Message + "\r\nФайл с настройками был сохранен в каталог с программой - " + Application.StartupPath + "\\" + XMLFileName, "Ошибка!");
                Options.settingsFilePath = Application.StartupPath + "\\" + XMLFileName;
                WriteXml();
            }
        }
        //Чтение настроек из файла
        public void ReadXml()
        {
            string filePath = !String.IsNullOrEmpty(Options.settingsFilePath) ? Options.settingsFilePath : (Application.StartupPath + "\\" + XMLFileName);

            if (File.Exists(filePath))
            {
                XmlSerializer ser = new XmlSerializer(typeof(PropsFields));
                TextReader reader = new StreamReader(filePath);
                try
                {
                    Fields = ser.Deserialize(reader) as PropsFields;
                    reader.Close();
                }
                catch
                {
                    MessageBox.Show("Не удалось прочитать файл с настройками. Был создан файл с настройкам по-умолчанию. В менеджере БД восстановите путь до Вашей рабочей базы данных!");
                    reader.Close();
                    System.Threading.Thread.Sleep(150);
                    File.Delete(filePath);
                    System.Threading.Thread.Sleep(200);
                    ReadXml();
                }
            }
            else
            {
                    WriteXml();

            }
        }

        List<WindowData> windowData_ = new List<WindowData>{};

        public void setFormParams(Form form, List<WindowData> windowData)
        {
            FormParams param = new FormParams
            {
                name = form.Name,
                windowState = form.WindowState,
                size = form.Size,
                location = form.Location,
                windowData = windowData
            };

            if (Options.formParams.Any(x => x.name == form.Name))
            {
                var p = Options.formParams.FirstOrDefault(x => x.name == form.Name);
                Options.formParams.Remove(p);
            }
            Options.formParams.Add(param);
        }

        public void setPackNum(numPackSettings numPackSett)
        {
            ReadXml();

            var Insurers = Fields.LastPackNumber;

            pu6Entities db = new pu6Entities();

            if (!db.Insurer.Any(x => x.ID == Options.InsID))
                return;

            string regnum = db.Insurer.First(x => x.ID == Options.InsID).RegNum;

            if (Insurers.Any(x => x.RegNum == regnum))
            {
                var p = Insurers.First(x => x.RegNum == regnum);
                if (p.NumPackSettings.Any(x => x.FormName == numPackSett.FormName)) // если для текущего страхователя и для этой формы уже есть данные
                {
                    var n = p.NumPackSettings.First(x => x.FormName == numPackSett.FormName);
                    n.Number = numPackSett.Number;
                    n.Year = numPackSett.Year;
                    n.Quarter = numPackSett.Quarter;
                }
                else
                {
                    p.NumPackSettings.Add(numPackSett);
                }
            }
            else
            {
                Insurers.Add(
                    new InsurersLastNums
                    {
                        RegNum = regnum,
                        NumPackSettings = new List<numPackSettings> { 
                            numPackSett
                        }
                    }
                );
            }

            WriteXml();

        }

        public int getPackNum(string formName)
        {
            int num = 1;


            pu6Entities db = new pu6Entities();

            if (!db.Insurer.Any(x => x.ID == Options.InsID))
                return num;

            string regnum = db.Insurer.First(x => x.ID == Options.InsID).RegNum;

            ReadXml();
            if (Fields.LastPackNumber.Any(x => x.RegNum == regnum && x.NumPackSettings.Any(c => c.FormName == formName)))
            {
                var NumInfo = Fields.LastPackNumber.First(x => x.RegNum == regnum);
                num = NumInfo.NumPackSettings.First(c => c.FormName == formName).Number;
            }
            return num;
        }

        public void setGridLayout(string FormName, string GridName, string Layouts)
        {
            GridLayouts gridLayouts = new GridLayouts
            {
                FormName = FormName,
                GridName = GridName,
                GridLayout = Layouts
            };
            if (Options.gridParams.Any(x => x.FormName == gridLayouts.FormName && x.GridName == gridLayouts.GridName))
            {
                var p = Options.gridParams.FirstOrDefault(x => x.FormName == gridLayouts.FormName && x.GridName == gridLayouts.GridName);
                Options.gridParams.Remove(p);
            }
            Options.gridParams.Add(gridLayouts);
        }

	
    }


}
