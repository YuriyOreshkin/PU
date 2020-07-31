using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PU.Models
{
    public class StaffObject
    {
        public long ID { get; set; }
        public int Num { get; set; }
        public string FIO { get; set; }
        public string SNILS { get; set; }
        public string INN { get; set; }
        public long? TabelNumber { get; set; }
        public string Sex { get; set; }
        public string Dismissed { get; set; }
        public string DateBirth { get; set; }
        public string Period { get; set; }
        public string TypeInfo { get; set; }
        public string KorrPeriod { get; set; }
        public string InsReg { get; set; }
        public string InsName { get; set; }
        public string DepName { get; set; }
    }

    public class StaffLgotObject
    {
        public int Num { get; set; }
        public string FIO { get; set; }
        public string SNILS { get; set; }
        public List<StajOsn_Lgot> stajOsn { get; set; }
        public List<FormsRSW2014_1_Razd_6_7_Lgot> razd67 { get; set; }
    }

    public class FormsRSW2014_1_Razd_6_7_Lgot
    {
        public string Code { get; set; }
        public decimal s_0_0 { get; set; }
        public decimal s_0_1 { get; set; }
        public decimal s_1_0 { get; set; }
        public decimal s_1_1 { get; set; }
        public decimal s_2_0 { get; set; }
        public decimal s_2_1 { get; set; }
        public decimal s_3_0 { get; set; }
        public decimal s_3_1 { get; set; }
    }

    public class StajOsn_Lgot
    {
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
        public string Lgot { get; set; }
    }

    public class SNILSObject
    {
        public string num { get; set; }
        public byte? contrNum { get; set; }
    }

    public class SZVMStaffRep
    {
        public int Num { get; set; }
        public string FIO { get; set; }
        public string SNILS { get; set; }
        public string INN { get; set; }
    }

    public class InsurerRep
    {
        public int Num { get; set; }
        public string RegNum { get; set; }
        public string Name { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
    }

    public class DictionContainer
    {
        public long ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? DateBegin { get; set; }
        public DateTime? DateEnd { get; set; }
    }

    public class RaschetPeriodContainer
    {
        public short Year { get; set; }
        public byte Kvartal { get; set; }
        public string Name { get; set; }
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }
    }

#region  Заполнение формы 2.1 из Индивидуальных сведений
    public class Identifier
    {
        public long InsurerID { get; set; }
        public byte CorrectionNum { get; set; }
        public short Year { get; set; }
        public byte Quarter { get; set; }
    }

    public class PlatCatTariffCode
    {
        public string Code { get; set; }
        public long PlatCategoryID { get; set; }
        public long TariffCodeID { get; set; }
    }
#endregion

    public class TimerList
    {
        public DateTime stamp { get; set; }
        public string name { get; set; }
    }

    public class DBEntry
    {
        public string name { get; set; }
        public string path { get; set; }
        public string pathBackup { get; set; }
        public bool actual { get; set; }
    }

    public class DBVer
    {
        public int app_ver { get; set; }
        public int db_ver { get; set; }
        public bool checkResult { get; set; }
    }

    public class RKASV
    {
        public string url { get; set; }
        public string service { get; set; }
        public string port { get; set; }
        public string opfrCode { get; set; }
    }

    public class SimpleList
    {
        public string id { get; set; }
        public string name { get; set; }
    }


    public class ErrList
    {
        public string name { get; set; }
        public string control { get; set; }
        public string type { get; set; }
    }

    #region  настройки страхователя
    public class InsurerImportExportPath
    {
        public string regnum { get; set; }
        public string importPath { get; set; }
        public string exportPath { get; set; }
    }
    #endregion

    #region настройки и данные форм
    public class FormParams
    {
        public string name { get; set; }
        public FormWindowState windowState { get; set; }
        public List<WindowData> windowData { get; set; }
        public Size size { get; set; }
        public Point location { get; set; }
    }

    public class WindowData
    {
        public string control { get; set; }
        public string value { get; set; }
    }

    public class GridLayouts
    {
        public string FormName { get; set; }
        public string GridName { get; set; }
        public string GridLayout { get; set; }
    }
    #endregion

    #region Настройки резервного копирования

    public class backup
    {
        public backupSettings autoBackup { get; set; }
        public backupSettings maxCount { get; set; }
        public string pathLast { get; set; }
    }

    public class backupSettings
    {
        public bool active { get; set; }
        public decimal value { get; set; }
    }

    #endregion


    public class svedNachVznos_container
    {
//        public int Num { get; set; }
        public string FIO { get; set; }
        public string SNILS { get; set; }
        public string Tabel { get; set; }
        public decimal Base_OPS_ALL { get; set; }
        public decimal Base_OPS_3M { get; set; }
        public decimal OPS { get; set; }
    }

    public class svedBaseOPS_container
    {
//        public int Num { get; set; }
        public string FIO { get; set; }
        public string SNILS { get; set; }
        public string PlatCat { get; set; }
        public decimal Base_OPS_ALL { get; set; }
        public decimal Base_OPS_GPD { get; set; }
        public decimal Base_OPS_1M { get; set; }
        public decimal Base_OPS_2M { get; set; }
        public decimal Base_OPS_3M { get; set; }
    }

    public class svedVypl_container
    {
        public string Number { get; set; }
        public string FIO { get; set; }
        public string SNILS { get; set; }
        public string PlatCat { get; set; }
        public decimal Base_OPS_ALL { get; set; }
        public decimal OPS { get; set; }
        public decimal Base_OPS_GPD { get; set; }
        public decimal PrevBase { get; set; }
    }

    public class svedDop_container
    {
        public string Number { get; set; }
        public string FIO { get; set; }
        public string SNILS { get; set; }
        public string DopTar { get; set; }
        public decimal SUM1 { get; set; }
        public decimal SUM2 { get; set; }
    }

    public class svedKorr_container
    {
        public int Num { get; set; }
        public string FIO { get; set; }
        public string SNILS { get; set; }
        public string Period { get; set; }
        public decimal OPS { get; set; }
        public decimal STRAH { get; set; }
        public decimal NAKOP { get; set; }
    }

    public class numPackSettings
    {
        public string FormName { get; set; }
        public short Year { get; set; }
        public byte? Quarter { get; set; }
        public int Number { get; set; }

    }

    public class InsurersLastNums
    {
        public string RegNum { get; set; }
        public List<numPackSettings> NumPackSettings { get; set; }

    }

}
