using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using System.Reflection;
using System.Threading;
using Telerik.WinControls.UI;
using PU.Classes;

namespace PU.FormsRSW2014
{
    public partial class RSW2014_Filling : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public Identifier identifier { get; set; }
        public FormsRSW2014_1_1 RSW { get; set; }
        public FormsRSW2014_1_1 RSW_Prev { get; set; }
        public FormsRSW2014_1_Razd_2_4 RSW_2_4_Prev0 { get; set; }
        public FormsRSW2014_1_Razd_2_4 RSW_2_4_Prev1 { get; set; }
        public List<FormsRSW2014_1_Razd_2_1> RSW_2_1_List = new List<FormsRSW2014_1_Razd_2_1>();
        public List<FormsRSW2014_1_Razd_2_1> RSW_2_1_List_Prev = new List<FormsRSW2014_1_Razd_2_1>();
        public List<FormsRSW2014_1_Razd_2_4> RSW_2_4_List = new List<FormsRSW2014_1_Razd_2_4>();

        private List<FormsRSW2014_1_Razd_6_4> Razd_6_4_list = new List<FormsRSW2014_1_Razd_6_4>();
        private List<FormsRSW2014_1_Razd_6_7> Razd_6_7_list = new List<FormsRSW2014_1_Razd_6_7>();

        private List<PlatCatTariffCode> platCatTariffCode = new List<PlatCatTariffCode>();
        private List<TariffCodePlatCat> tariffCode = new List<TariffCodePlatCat>();
        private List<long> RSW_6_1_IDs = new List<long>();
        private List<FormsRSW2014_1_Razd_6_1> RSW_6_1_List = new List<FormsRSW2014_1_Razd_6_1>();
        private MROT mrot = new MROT{};
        private short yearType = 2014;
        private List<ErrList> errList = new List<ErrList>();
        RSW2014_6_Copy_Wait child = new RSW2014_6_Copy_Wait(); 


        public RSW2014_Filling()
        {
            InitializeComponent();
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(calculation);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);

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

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void getPrevData()
        {
            short y = identifier.Year;
            byte q = identifier.Quarter;

            if (Options.inputTypeRSW1 != 0)
            {
                if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                {
                    byte quarter = 20;
                    if (q == 6)
                        quarter = 3;
                    else if (q == 9)
                        quarter = 6;
                    else if (q == 0)
                        quarter = 9;


                    if (db.FormsRSW2014_1_1.Any(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID))
                        RSW_Prev = db.FormsRSW2014_1_1.Where(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID).OrderByDescending(x => x.CorrectionNum).First();
                }
                else
                {
                    RSW_Prev = null;
                }
            }
            else
            {
                RSW_Prev = null;
            }
        }

        private void getPrevData_2_1(long tarCodeID)
        {
            short y = identifier.Year;
            byte q = identifier.Quarter;

            if (Options.inputTypeRSW1 != 0)
            {

                if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                {
                    byte quarter = 20;
                    if (q == 6)
                        quarter = 3;
                    else if (q == 9)
                        quarter = 6;
                    else if (q == 0)
                        quarter = 9;

                    if (db.FormsRSW2014_1_Razd_2_1.Any(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID && x.TariffCodeID == tarCodeID))
                        RSW_2_1_List_Prev.Add(db.FormsRSW2014_1_Razd_2_1.Where(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID && x.TariffCodeID == tarCodeID).OrderByDescending(x => x.CorrectionNum).First());
                    else
                        RSW_2_1_List_Prev.Add(new FormsRSW2014_1_Razd_2_1 { });

                }
                else
                {
                    RSW_2_1_List_Prev.Add(new FormsRSW2014_1_Razd_2_1 { });
                }
            }
            else
            {
                RSW_2_1_List_Prev.Add(new FormsRSW2014_1_Razd_2_1 { });
            }
        }

        private bool rsw_delete(long id)
        {
            bool result = true;
            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.First(x => x.ID == id);

            db.FormsRSW2014_1_1.Remove(rsw);

            foreach (var item in db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_2_1.Remove(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_2_4.Remove(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_2_5_1.Remove(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_2_5_2.Remove(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_3_4.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_3_4.Remove(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_4.Remove(item);
            }
            foreach (var item in db.FormsRSW2014_1_Razd_5.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
            {
                db.FormsRSW2014_1_Razd_5.Remove(item);
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

        private void rsw1_create()
        {
            // Выбор из базы Индивидуальных сведений и запонение РСВ-1
            if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.InsurerID == identifier.InsurerID && x.Quarter == identifier.Quarter && x.Year == identifier.Year && x.TypeInfoID == 1))
            {
                yearType = ((identifier.Year == (short)2014) || (identifier.Year == (short)2015 && identifier.Quarter == 3)) ? (short)2014 : (short)2015;

                this.Cursor = Cursors.WaitCursor;

                List<string> list = new List<string>(new string[] { "НР", "ВЖНР", "ВПНР", "ПНЭД", "ВЖЭД", "ВПЭД", "АСБ", "ВЖСБ", "ВПСБ", "ХМН", "ВЖМН", "ВПМН" });
                var pcrp = db.PlatCategoryRaschPer.FirstOrDefault(x => (!x.DateEnd.HasValue && x.DateBegin.Value.Year <= identifier.Year) || (x.DateEnd.HasValue && (x.DateBegin.Value.Year <= identifier.Year && x.DateEnd.Value.Year >= identifier.Year)));

                var platCatList = db.PlatCategory.Where(x => list.Contains(x.Code) && x.PlatCategoryRaschPerID == pcrp.ID);

                int i = 0;
                foreach (var item in list)
                {
                    long platCatID = platCatList.FirstOrDefault(x => x.Code == item).ID;
                    long tariffCodeId = 0;
                    string tariffCodeName = string.Empty;
                    i++;
                    if (i >= 1 && i <= 3)
                    {
                        if (radBtn01.IsChecked)
                            tariffCodeName = "01";
                        if (radBtn52.IsChecked)
                            tariffCodeName = "52";
                        if (radBtn53.IsChecked)
                            tariffCodeName = "53";
                    }
                    if (i >= 4 && i <= 6)
                    {
                        if (radBtn07.IsChecked)
                            tariffCodeName = "07";
                        if (radBtn16.IsChecked)
                            tariffCodeName = "16";
                    }
                    if (i >= 7 && i <= 9)
                    {
                        if (radBtn11.IsChecked)
                            tariffCodeName = "11";
                        if (radBtn12.IsChecked)
                            tariffCodeName = "12";
                        if (radBtn13.IsChecked)
                            tariffCodeName = "13";
                    }
                    if (i >= 10 && i <= 12)
                    {
                        if (radBtn19.IsChecked)
                            tariffCodeName = "19";
                        if (radBtn20.IsChecked)
                            tariffCodeName = "20";
                    }

                    tariffCodeId = db.TariffCode.Any(x => x.Code == tariffCodeName) ? db.TariffCode.FirstOrDefault(x => x.Code == tariffCodeName).ID : 0;

                    platCatTariffCode.Add(new PlatCatTariffCode { Code = item, PlatCategoryID = platCatID, TariffCodeID = tariffCodeId });

                }

                tariffCode = db.TariffCodePlatCat.ToList();

                var t = db.FormsRSW2014_1_Razd_6_1.Where(y => y.InsurerID == identifier.InsurerID && y.TypeInfoID == 1 && y.Quarter == identifier.Quarter && y.Year == identifier.Year);
                RSW_6_1_IDs = t.Select(y => y.ID).ToList();

                //RSW_6_1_List = db.FormsRSW2014_1_Razd_6_1.Where(y => y.InsurerID == identifier.InsurerID && y.TypeInfoID == 1 && y.Quarter == identifier.Quarter && y.Year == identifier.Year).ToList();

         //       if (db.FormsRSW2014_1_Razd_6_4.Any(x => RSW_6_1_IDs.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)))
         //       {
                Razd_6_4_list = db.FormsRSW2014_1_Razd_6_4.Where(x => RSW_6_1_IDs.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).ToList();

                //Razd_6_7_list = RSW_6_1_List.SelectMany(x => x.FormsRSW2014_1_Razd_6_7).ToList();

//                int cnt = db.FormsRSW2014_1_Razd_6_4.Where(x => RSW_6_1_IDs.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).Count();
                int cnt = Razd_6_4_list.Count();
                this.Cursor = Cursors.Default;

                    backgroundWorker1.RunWorkerAsync();

                    child.Owner = this;
                    child.titleLabel.Text = "Формирование Формы РСВ-1";
                    child.ThemeName = this.ThemeName;
                    child.secondPartLabel.Text = cnt.ToString();
                    child.Show();

/*                }
                else
                {
                    RadMessageBox.Show(this, "Не заполнен Раздел 6.4 для текущего Страхователя за указанный период и номер корректировки", "Ошибка");
                }*/
            }
            else
            {
                RadMessageBox.Show(this, "Не найден Раздел 6 для текущего Страхователя за указанный период и номер корректировки", "Ошибка");
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                if (db.FormsRSW2014_1_1.Any(x => x.InsurerID == identifier.InsurerID && x.Quarter == identifier.Quarter && x.Year == identifier.Year && x.CorrectionNum == 0))
                {
                    DialogResult dialogResult = RadMessageBox.Show(this, "Форма РСВ-1, с указанными параметрами, уже имеется в Вашей базе данных.\n\rВы желаете ее заменить вновь формируемой?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.Yes)
                    {
                        long id = db.FormsRSW2014_1_1.FirstOrDefault(x => x.InsurerID == identifier.InsurerID && x.Quarter == identifier.Quarter && x.Year == identifier.Year && x.CorrectionNum == 0).ID;
                        if (rsw_delete(id))
                        {
                            rsw1_create();
                        }
                        else
                        {
                            RadMessageBox.Show(this, "Внимание", "При удалении исходной записи произошла ошибка.\n\rФормирование Формы РСВ-1 прекращенно!\n\rПопробуйте удалить исходную форму вручную.");
                        }

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                        //do something else
                    }
                }
                else
                {
                    rsw1_create();
                }
            
            }

        }

        private bool validation()
        {
            bool result = true;
            errList.Clear();
            if ((Year.Text == "") || (Quarter.Text == ""))
            {
                errList.Add(new ErrList { name = "Не выбран отчетный период", control = "Quarter" });
            }

            return result;
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.child.Close();
            this.Close();
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            child.Invoke(new Action(() => { child.radProgressBar1.Value1 = e.ProgressPercentage; }));
            child.Invoke(new Action(() => { child.firstPartLabel.Text = e.UserState.ToString(); }));
        }

        private void calculation_2_1(int i)
        {

            if (RSW_2_1_List_Prev[i] != null)
            {
                RSW_2_1_List[i].s_200_0 = RSW_2_1_List_Prev[i].s_200_0.HasValue ? RSW_2_1_List_Prev[i].s_200_0.Value + RSW_2_1_List[i].s_200_1 + RSW_2_1_List[i].s_200_2 + RSW_2_1_List[i].s_200_3 : RSW_2_1_List[i].s_200_1 + RSW_2_1_List[i].s_200_2 + RSW_2_1_List[i].s_200_3;
                RSW_2_1_List[i].s_201_0 = RSW_2_1_List_Prev[i].s_201_0.HasValue ? RSW_2_1_List_Prev[i].s_201_0.Value + RSW_2_1_List[i].s_201_1 + RSW_2_1_List[i].s_201_2 + RSW_2_1_List[i].s_201_3 : RSW_2_1_List[i].s_201_1 + RSW_2_1_List[i].s_201_2 + RSW_2_1_List[i].s_201_3;
                RSW_2_1_List[i].s_202_0 = RSW_2_1_List_Prev[i].s_202_0.HasValue ? RSW_2_1_List_Prev[i].s_202_0.Value + RSW_2_1_List[i].s_202_1 + RSW_2_1_List[i].s_202_2 + RSW_2_1_List[i].s_202_3 : RSW_2_1_List[i].s_202_1 + RSW_2_1_List[i].s_202_2 + RSW_2_1_List[i].s_202_3;
                RSW_2_1_List[i].s_203_0 = RSW_2_1_List_Prev[i].s_203_0.HasValue ? RSW_2_1_List_Prev[i].s_203_0.Value + RSW_2_1_List[i].s_203_1 + RSW_2_1_List[i].s_203_2 + RSW_2_1_List[i].s_203_3 : RSW_2_1_List[i].s_203_1 + RSW_2_1_List[i].s_203_2 + RSW_2_1_List[i].s_203_3;
            }
            else
            {
                RSW_2_1_List[i].s_200_0 = RSW_2_1_List[i].s_200_1 + RSW_2_1_List[i].s_200_2 + RSW_2_1_List[i].s_200_3;
                RSW_2_1_List[i].s_201_0 = RSW_2_1_List[i].s_201_1 + RSW_2_1_List[i].s_201_2 + RSW_2_1_List[i].s_201_3;
                RSW_2_1_List[i].s_202_0 = RSW_2_1_List[i].s_202_1 + RSW_2_1_List[i].s_202_2 + RSW_2_1_List[i].s_202_3;
                RSW_2_1_List[i].s_203_0 = RSW_2_1_List[i].s_203_1 + RSW_2_1_List[i].s_203_2 + RSW_2_1_List[i].s_203_3;
            }

        }

        private void calculation_2_4()
        {
            if (RSW_2_4_Prev0 != null)
            {
                RSW_2_4_List[0].s_240_0 = RSW_2_4_Prev0.s_240_0.HasValue ? RSW_2_4_Prev0.s_240_0.Value + RSW_2_4_List[0].s_240_1 + RSW_2_4_List[0].s_240_2 + RSW_2_4_List[0].s_240_3 : RSW_2_4_List[0].s_240_1 + RSW_2_4_List[0].s_240_2 + RSW_2_4_List[0].s_240_3;
                RSW_2_4_List[0].s_246_0 = RSW_2_4_Prev0.s_246_0.HasValue ? RSW_2_4_Prev0.s_246_0.Value + RSW_2_4_List[0].s_246_1 + RSW_2_4_List[0].s_246_2 + RSW_2_4_List[0].s_246_3 : RSW_2_4_List[0].s_246_1 + RSW_2_4_List[0].s_246_2 + RSW_2_4_List[0].s_246_3;
                RSW_2_4_List[0].s_252_0 = RSW_2_4_Prev0.s_252_0.HasValue ? RSW_2_4_Prev0.s_252_0.Value + RSW_2_4_List[0].s_252_1 + RSW_2_4_List[0].s_252_2 + RSW_2_4_List[0].s_252_3 : RSW_2_4_List[0].s_252_1 + RSW_2_4_List[0].s_252_2 + RSW_2_4_List[0].s_252_3;
                RSW_2_4_List[0].s_258_0 = RSW_2_4_Prev0.s_258_0.HasValue ? RSW_2_4_Prev0.s_258_0.Value + RSW_2_4_List[0].s_258_1 + RSW_2_4_List[0].s_258_2 + RSW_2_4_List[0].s_258_3 : RSW_2_4_List[0].s_258_1 + RSW_2_4_List[0].s_258_2 + RSW_2_4_List[0].s_258_3;
                RSW_2_4_List[0].s_264_0 = RSW_2_4_Prev0.s_264_0.HasValue ? RSW_2_4_Prev0.s_264_0.Value + RSW_2_4_List[0].s_264_1 + RSW_2_4_List[0].s_264_2 + RSW_2_4_List[0].s_264_3 : RSW_2_4_List[0].s_264_1 + RSW_2_4_List[0].s_264_2 + RSW_2_4_List[0].s_264_3;
            }
            else
            {
                RSW_2_4_List[0].s_240_0 = RSW_2_4_List[0].s_240_1 + RSW_2_4_List[0].s_240_2 + RSW_2_4_List[0].s_240_3;
                RSW_2_4_List[0].s_246_0 = RSW_2_4_List[0].s_246_1 + RSW_2_4_List[0].s_246_2 + RSW_2_4_List[0].s_246_3;
                RSW_2_4_List[0].s_252_0 = RSW_2_4_List[0].s_252_1 + RSW_2_4_List[0].s_252_2 + RSW_2_4_List[0].s_252_3;
                RSW_2_4_List[0].s_258_0 = RSW_2_4_List[0].s_258_1 + RSW_2_4_List[0].s_258_2 + RSW_2_4_List[0].s_258_3;
                RSW_2_4_List[0].s_264_0 = RSW_2_4_List[0].s_264_1 + RSW_2_4_List[0].s_264_2 + RSW_2_4_List[0].s_264_3;
            }

            if (RSW_2_4_Prev1 != null)
            {
                RSW_2_4_List[1].s_240_0 = RSW_2_4_Prev1.s_240_0.HasValue ? RSW_2_4_Prev1.s_240_0.Value + RSW_2_4_List[1].s_240_1 + RSW_2_4_List[1].s_240_2 + RSW_2_4_List[1].s_240_3 : RSW_2_4_List[1].s_240_1 + RSW_2_4_List[1].s_240_2 + RSW_2_4_List[1].s_240_3;
                RSW_2_4_List[1].s_246_0 = RSW_2_4_Prev1.s_246_0.HasValue ? RSW_2_4_Prev1.s_246_0.Value + RSW_2_4_List[1].s_246_1 + RSW_2_4_List[1].s_246_2 + RSW_2_4_List[1].s_246_3 : RSW_2_4_List[1].s_246_1 + RSW_2_4_List[1].s_246_2 + RSW_2_4_List[1].s_246_3;
                RSW_2_4_List[1].s_252_0 = RSW_2_4_Prev1.s_252_0.HasValue ? RSW_2_4_Prev1.s_252_0.Value + RSW_2_4_List[1].s_252_1 + RSW_2_4_List[1].s_252_2 + RSW_2_4_List[1].s_252_3 : RSW_2_4_List[1].s_252_1 + RSW_2_4_List[1].s_252_2 + RSW_2_4_List[1].s_252_3;
                RSW_2_4_List[1].s_258_0 = RSW_2_4_Prev1.s_258_0.HasValue ? RSW_2_4_Prev1.s_258_0.Value + RSW_2_4_List[1].s_258_1 + RSW_2_4_List[1].s_258_2 + RSW_2_4_List[1].s_258_3 : RSW_2_4_List[1].s_258_1 + RSW_2_4_List[1].s_258_2 + RSW_2_4_List[1].s_258_3;
                RSW_2_4_List[1].s_264_0 = RSW_2_4_Prev1.s_264_0.HasValue ? RSW_2_4_Prev1.s_264_0.Value + RSW_2_4_List[1].s_264_1 + RSW_2_4_List[1].s_264_2 + RSW_2_4_List[1].s_264_3 : RSW_2_4_List[1].s_264_1 + RSW_2_4_List[1].s_264_2 + RSW_2_4_List[1].s_264_3;
            }
            else
            {
                RSW_2_4_List[1].s_240_0 = RSW_2_4_List[1].s_240_1 + RSW_2_4_List[1].s_240_2 + RSW_2_4_List[1].s_240_3;
                RSW_2_4_List[1].s_246_0 = RSW_2_4_List[1].s_246_1 + RSW_2_4_List[1].s_246_2 + RSW_2_4_List[1].s_246_3;
                RSW_2_4_List[1].s_252_0 = RSW_2_4_List[1].s_252_1 + RSW_2_4_List[1].s_252_2 + RSW_2_4_List[1].s_252_3;
                RSW_2_4_List[1].s_258_0 = RSW_2_4_List[1].s_258_1 + RSW_2_4_List[1].s_258_2 + RSW_2_4_List[1].s_258_3;
                RSW_2_4_List[1].s_264_0 = RSW_2_4_List[1].s_264_1 + RSW_2_4_List[1].s_264_2 + RSW_2_4_List[1].s_264_3;
            }


            foreach (var item in RSW_2_4_List)
            {
                item.s_243_0 = item.s_240_0 - item.s_241_0;
                item.s_243_1 = item.s_240_1 - item.s_241_1;
                item.s_243_2 = item.s_240_2 - item.s_241_2;
                item.s_243_3 = item.s_240_3 - item.s_241_3;

                item.s_249_0 = item.s_246_0 - item.s_247_0;
                item.s_249_1 = item.s_246_1 - item.s_247_1;
                item.s_249_2 = item.s_246_2 - item.s_247_2;
                item.s_249_3 = item.s_246_3 - item.s_247_3;

                item.s_255_0 = item.s_252_0 - item.s_253_0;
                item.s_255_1 = item.s_252_1 - item.s_253_1;
                item.s_255_2 = item.s_252_2 - item.s_253_2;
                item.s_255_3 = item.s_252_3 - item.s_253_3;

                item.s_261_0 = item.s_258_0 - item.s_259_0;
                item.s_261_1 = item.s_258_1 - item.s_259_1;
                item.s_261_2 = item.s_258_2 - item.s_259_2;
                item.s_261_3 = item.s_258_3 - item.s_259_3;

                item.s_267_0 = item.s_264_0 - item.s_265_0;
                item.s_267_1 = item.s_264_1 - item.s_265_1;
                item.s_267_2 = item.s_264_2 - item.s_265_2;
                item.s_267_3 = item.s_264_3 - item.s_265_3;


                byte Type_ = dopTariffDDL.SelectedIndex == 0 ? (byte)0 : (byte)1;
                string Code = "О4";
                DateTime Date = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == identifier.Year && x.Kvartal == identifier.Quarter).DateBegin.Date;

                decimal tariff = 0;

                try
                {
                    tariff = db.SpecOcenkaUslTruda.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;
                }
                catch
                { }

                item.s_244_0 = item.s_243_0 * tariff / 100;
                item.s_244_1 = ((item.s_243_0 - item.s_243_2 - item.s_243_3) * tariff / 100) - ((item.s_243_0 - item.s_243_1 - item.s_243_2 - item.s_243_3) * tariff / 100);
                item.s_244_2 = (((item.s_243_0 - item.s_243_3) * tariff / 100) - ((item.s_243_0 - item.s_243_1 - item.s_243_2 - item.s_243_3) * tariff / 100)) - item.s_244_1;
                item.s_244_3 = ((item.s_243_0 * tariff / 100) - ((item.s_243_0 - item.s_243_1 - item.s_243_2 - item.s_243_3) * tariff / 100)) - (item.s_244_1 + item.s_244_2);

                Code = "В3.4";
                try
                {
                    tariff = db.SpecOcenkaUslTruda.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;
                }
                catch
                {
                    tariff = 0;
                }

                item.s_250_0 = item.s_249_0 * tariff / 100;
                item.s_250_1 = ((item.s_249_0 - item.s_249_2 - item.s_249_3) * tariff / 100) - ((item.s_249_0 - item.s_249_1 - item.s_249_2 - item.s_249_3) * tariff / 100);
                item.s_250_2 = (((item.s_249_0 - item.s_249_3) * tariff / 100) - ((item.s_249_0 - item.s_249_1 - item.s_249_2 - item.s_249_3) * tariff / 100)) - item.s_250_1;
                item.s_250_3 = (((item.s_249_0) * tariff / 100) - ((item.s_249_0 - item.s_249_1 - item.s_249_2 - item.s_249_3) * tariff / 100)) - (item.s_250_1 + item.s_250_2);

                Code = "В3.3";
                try
                {
                    tariff = db.SpecOcenkaUslTruda.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;
                }
                catch
                {
                    tariff = 0;
                }

                item.s_256_0 = item.s_255_0 * tariff / 100;
                item.s_256_1 = ((item.s_255_0 - item.s_255_2 - item.s_255_3) * tariff / 100) - ((item.s_255_0 - item.s_255_1 - item.s_255_2 - item.s_255_3) * tariff / 100);
                item.s_256_2 = (((item.s_255_0 - item.s_255_3) * tariff / 100) - ((item.s_255_0 - item.s_255_1 - item.s_255_2 - item.s_255_3) * tariff / 100)) - item.s_256_1;
                item.s_256_3 = (((item.s_255_0) * tariff / 100) - ((item.s_255_0 - item.s_255_1 - item.s_255_2 - item.s_255_3) * tariff / 100)) - (item.s_256_1 + item.s_256_2);

                Code = "В3.2";
                try
                {
                    tariff = db.SpecOcenkaUslTruda.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;
                }
                catch
                {
                    tariff = 0;
                }

                item.s_262_0 = item.s_261_0 * tariff / 100;
                item.s_262_1 = ((item.s_261_0 - item.s_261_2 - item.s_261_3) * tariff / 100) - ((item.s_261_0 - item.s_261_1 - item.s_261_2 - item.s_261_3) * tariff / 100);
                item.s_262_2 = (((item.s_261_0 - item.s_261_3) * tariff / 100) - ((item.s_261_0 - item.s_261_1 - item.s_261_2 - item.s_261_3) * tariff / 100)) - item.s_262_1;
                item.s_262_3 = (((item.s_261_0) * tariff / 100) - ((item.s_261_0 - item.s_261_1 - item.s_261_2 - item.s_261_3) * tariff / 100)) - (item.s_262_1 + item.s_262_2);

                Code = "В3.1";
                try
                {
                    tariff = db.SpecOcenkaUslTruda.FirstOrDefault(x => x.Code == Code && ((!x.DateEnd.HasValue && x.DateBegin.Value <= Date) || (x.DateEnd.HasValue && (x.DateBegin.Value <= Date && x.DateEnd >= Date)))).SpecOcenkaUslTrudaDopTariff.FirstOrDefault(y => y.Type == Type_ && ((!y.DateEnd.HasValue && y.DateBegin.Value <= Date) || (y.DateEnd.HasValue && (y.DateBegin.Value <= Date && y.DateEnd >= Date)))).Rate.Value;
                }
                catch
                {
                    tariff = 0;
                }

                item.s_268_0 = item.s_267_0 * tariff / 100;
                item.s_268_1 = ((item.s_267_0 - item.s_267_2 - item.s_267_3) * tariff / 100) - ((item.s_267_0 - item.s_267_1 - item.s_267_2 - item.s_267_3) * tariff / 100);
                item.s_268_2 = (((item.s_267_0 - item.s_267_3) * tariff / 100) - ((item.s_267_0 - item.s_267_1 - item.s_267_2 - item.s_267_3) * tariff / 100)) - item.s_268_1;
                item.s_268_3 = (((item.s_267_0) * tariff / 100) - ((item.s_267_0 - item.s_267_1 - item.s_267_2 - item.s_267_3) * tariff / 100)) - (item.s_268_1 + item.s_268_2);

            }

        }
        private void calculation(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(200);

            RSW = new FormsRSW2014_1_1 { 
                Year = identifier.Year,
                Quarter = identifier.Quarter,
                InsurerID = identifier.InsurerID,
                CorrectionNum = 0,
                ConfirmType = 1,
                CountEmployers = RSW_6_1_IDs.Count(),
                DateUnderwrite = DateTime.Now.Date
            };

            RSW.SetZeroValues();

            //List<FormsRSW2014_1_Razd_6_4> Razd_6_4_list = db.FormsRSW2014_1_Razd_6_4.Where(x => RSW_6_1_IDs.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).OrderBy(x => x.PlatCategoryID).ToList();
            //List<FormsRSW2014_1_Razd_6_7> Razd_6_7_list = db.FormsRSW2014_1_Razd_6_7.Where(x => RSW_6_1_IDs.Contains(x.FormsRSW2014_1_Razd_6_1_ID)).OrderBy(x => x.SpecOcenkaUslTrudaID).ToList();

            Razd_6_4_list = Razd_6_4_list.OrderBy(x => x.PlatCategoryID).ToList();

            #region Формирование раздела 2.1
            long TariffCodeID = 0;
            long platCatID = 0;
            int i = 0;
            int k = 0;
            long id = 0;
            if (Razd_6_4_list.Count > 0)
            {
                platCatID = Razd_6_4_list.First().PlatCategoryID;

                if (platCatTariffCode.Any(x => x.Code == Razd_6_4_list.First().PlatCategory.Code))
                {
                    TariffCodeID = platCatTariffCode.FirstOrDefault(x => x.Code == Razd_6_4_list.First().PlatCategory.Code).TariffCodeID;
                }
                else
                {
                    TariffCodeID = tariffCode.FirstOrDefault(x => x.PlatCategoryID == Razd_6_4_list.First().PlatCategoryID).TariffCodeID.Value;
                }

                FormsRSW2014_1_Razd_2_1 rsw21_ = new FormsRSW2014_1_Razd_2_1 {  };
                RSW_2_1_List.Add(rsw21_.SetZeroValues());
                RSW_2_1_List[0].TariffCodeID = TariffCodeID;
            }
            int cnt_ = Razd_6_4_list.Count();
            foreach (var item in Razd_6_4_list)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    RSW_2_1_List.Clear();
                    return;
                }
                
                k++;
       //         System.Threading.Thread.Sleep(1);
                if (platCatTariffCode.Any(x => x.Code == item.PlatCategory.Code))
                {
                    var TariffCodeID_ = platCatTariffCode.FirstOrDefault(x => x.Code == item.PlatCategory.Code).TariffCodeID;
                    if (TariffCodeID_ != TariffCodeID)
                    {
                        if (RSW_2_1_List.Any(x => x.TariffCodeID == TariffCodeID_))
                        {
                            TariffCodeID = TariffCodeID_;
                            i = RSW_2_1_List.FindIndex(x => x.TariffCodeID == TariffCodeID_);
                        }
                        else
                        {
                            TariffCodeID = TariffCodeID_;
                            platCatID = item.PlatCategoryID;
                            i++;
                            FormsRSW2014_1_Razd_2_1 rsw21_ = new FormsRSW2014_1_Razd_2_1 { };
                            RSW_2_1_List.Add(rsw21_.SetZeroValues());
                        }

                    }

                    if (RSW_2_1_List[i].TariffCodeID != TariffCodeID)
                    {
                        RSW_2_1_List[i].TariffCodeID = TariffCodeID;
                        id = RSW_2_1_List[i].TariffCodeID.Value;
                        RSW_2_1_List[i].TariffCode = db.TariffCode.FirstOrDefault(x => x.ID == id);
                    }
                }
                else
                {
                    var TariffCodeID_ = tariffCode.FirstOrDefault(x => x.PlatCategoryID == item.PlatCategoryID).TariffCodeID.Value;
                    if (TariffCodeID_ != TariffCodeID)
                    {
                        if (RSW_2_1_List.Any(x => x.TariffCodeID == TariffCodeID_))
                        {
                            TariffCodeID = TariffCodeID_;
                            i = RSW_2_1_List.FindIndex(x => x.TariffCodeID == TariffCodeID_);
                        }
                        else
                        {

                            TariffCodeID = TariffCodeID_;
                            platCatID = item.PlatCategoryID;
                            i++;
                            FormsRSW2014_1_Razd_2_1 rsw21_ = new FormsRSW2014_1_Razd_2_1 { };
                            RSW_2_1_List.Add(rsw21_.SetZeroValues());
                        }

                    }
                    var tmp = tariffCode.FirstOrDefault(x => x.PlatCategoryID == item.PlatCategoryID).TariffCodeID;
                    if (RSW_2_1_List[i].TariffCodeID != tmp)
                    {
                        RSW_2_1_List[i].TariffCodeID = tmp;
                        id = RSW_2_1_List[i].TariffCodeID.Value;
                        RSW_2_1_List[i].TariffCode = db.TariffCode.FirstOrDefault(x => x.ID == id);
                    }
                }





                RSW_2_1_List[i].AutoCalc = true;

                //Начисленные взносы 200 строка
//                RSW_2_1_List[i].s_200_0 = RSW_2_1_List[i].s_200_0 + item.s_0_0.Value;
                RSW_2_1_List[i].s_200_1 = item.s_1_0 != null ? RSW_2_1_List[i].s_200_1 + item.s_1_0.Value : RSW_2_1_List[i].s_200_1;
                RSW_2_1_List[i].s_200_2 = item.s_2_0 != null ? RSW_2_1_List[i].s_200_2 + item.s_2_0.Value : RSW_2_1_List[i].s_200_2;
                RSW_2_1_List[i].s_200_3 = item.s_3_0 != null ? RSW_2_1_List[i].s_200_3 + item.s_3_0.Value : RSW_2_1_List[i].s_200_3;

                //Суммы не подлежащие обложению 201 строка
//                RSW_2_1_List[i].s_201_0 = RSW_2_1_List[i].s_201_0 + item.s_0_0.Value - item.s_0_1.Value - item.s_0_3.Value;
                decimal s1 = item.s_1_0 != null ? item.s_1_0.Value : 0;
                decimal s2 = item.s_1_1 != null ? item.s_1_1.Value : 0;
                decimal s3 = item.s_1_3 != null ? item.s_1_3.Value : 0;

                RSW_2_1_List[i].s_201_1 = RSW_2_1_List[i].s_201_1 + s1 - s2 - s3;

                s1 = item.s_2_0 != null ? item.s_2_0.Value : 0;
                s2 = item.s_2_1 != null ? item.s_2_1.Value : 0;
                s3 = item.s_2_3 != null ? item.s_2_3.Value : 0;
                RSW_2_1_List[i].s_201_2 = RSW_2_1_List[i].s_201_2 + s1 - s2 - s3;

                s1 = item.s_3_0 != null ? item.s_3_0.Value : 0;
                s2 = item.s_3_1 != null ? item.s_3_1.Value : 0;
                s3 = item.s_3_3 != null ? item.s_3_3.Value : 0;
                RSW_2_1_List[i].s_201_3 = RSW_2_1_List[i].s_201_3 + s1 - s2 - s3;


                //Суммы превышения 203 строка
//                RSW_2_1_List[i].s_203_0 = RSW_2_1_List[i].s_203_0 + item.s_0_3.Value;

                if (item.s_1_3 != null && item.s_1_3.Value != 0)
                {
                    RSW_2_1_List[i].s_203_1 = RSW_2_1_List[i].s_203_1 + item.s_1_3.Value;
                    RSW_2_1_List[i].s_208_1++;
                }
                else if (item.s_1_0 != null && item.s_1_0.Value != 0)
                {
                    RSW_2_1_List[i].s_207_1++;
                }

                if (item.s_2_3 != null && item.s_2_3.Value != 0)
                {
                    RSW_2_1_List[i].s_203_2 = RSW_2_1_List[i].s_203_2 + item.s_2_3.Value;
                    RSW_2_1_List[i].s_208_2++;
                }
                else if (item.s_2_0 != null && item.s_2_0.Value != 0)
                {
                    RSW_2_1_List[i].s_207_2++;
                }

                if (item.s_3_3 != null && item.s_3_3.Value != 0)
                {
                    RSW_2_1_List[i].s_203_3 = RSW_2_1_List[i].s_203_3 + item.s_3_3.Value;
                    RSW_2_1_List[i].s_208_3++;
                }
                else if (item.s_3_0 != null && item.s_3_0.Value != 0)
                {
                    RSW_2_1_List[i].s_207_3++;
                }


                decimal temp = (decimal)k / (decimal)cnt_;
                int proc = (int)Math.Round((temp * 100), 0);
                if ((k % 20) == 0)
                    backgroundWorker1.ReportProgress(proc, k.ToString());
            }
                #endregion

            #region Формирование раздела 2.2 и 2.3
            k = 0;
            id = 0;
            backgroundWorker1.ReportProgress(0, k.ToString());
            child.Invoke(new Action(() => { child.secondPartLabel.Text = Razd_6_7_list.Count(x => x.SpecOcenkaUslTrudaID == null).ToString(); }));



            for (int t = 0; t < RSW_2_1_List.Count(); t++)
            {
                RSW_2_1_List[t].InsurerID = identifier.InsurerID;
                RSW_2_1_List[t].CorrectionNum = identifier.CorrectionNum;
                RSW_2_1_List[t].Quarter = identifier.Quarter;
                RSW_2_1_List[t].Year = identifier.Year;

                getPrevData_2_1(RSW_2_1_List[t].TariffCodeID.Value);


                calculation_2_1(t);

                //Начисленные взносы 210 строка
                RSW_2_1_List[t].s_210_0 = RSW_2_1_List[t].s_200_0;
                RSW_2_1_List[t].s_210_1 = RSW_2_1_List[t].s_200_1;
                RSW_2_1_List[t].s_210_2 = RSW_2_1_List[t].s_200_2;
                RSW_2_1_List[t].s_210_3 = RSW_2_1_List[t].s_200_3;

                //Суммы не подлежащие обложению 211 строка
                RSW_2_1_List[t].s_211_0 = RSW_2_1_List[t].s_201_0;
                RSW_2_1_List[t].s_211_1 = RSW_2_1_List[t].s_201_1;
                RSW_2_1_List[t].s_211_2 = RSW_2_1_List[t].s_201_2;
                RSW_2_1_List[t].s_211_3 = RSW_2_1_List[t].s_201_3;



                //База для начисления 204 строка
                RSW_2_1_List[t].s_204_0 = RSW_2_1_List[t].s_200_0 - RSW_2_1_List[t].s_201_0 - RSW_2_1_List[t].s_202_0 - RSW_2_1_List[t].s_203_0;
                RSW_2_1_List[t].s_204_1 = RSW_2_1_List[t].s_200_1 - RSW_2_1_List[t].s_201_1 - RSW_2_1_List[t].s_202_1 - RSW_2_1_List[t].s_203_1;
                RSW_2_1_List[t].s_204_2 = RSW_2_1_List[t].s_200_2 - RSW_2_1_List[t].s_201_2 - RSW_2_1_List[t].s_202_2 - RSW_2_1_List[t].s_203_2;
                RSW_2_1_List[t].s_204_3 = RSW_2_1_List[t].s_200_3 - RSW_2_1_List[t].s_201_3 - RSW_2_1_List[t].s_202_3 - RSW_2_1_List[t].s_203_3;

                if (yearType != 2015)
                {
                    //Суммы превышения 213 строка
                    RSW_2_1_List[t].s_213_0 = RSW_2_1_List[t].s_203_0;
                    RSW_2_1_List[t].s_213_1 = RSW_2_1_List[t].s_203_1;
                    RSW_2_1_List[t].s_213_2 = RSW_2_1_List[t].s_203_2;
                    RSW_2_1_List[t].s_213_3 = RSW_2_1_List[t].s_203_3;

                    //База для начисления 214 строка
                    RSW_2_1_List[t].s_214_0 = RSW_2_1_List[t].s_204_0;
                    RSW_2_1_List[t].s_214_1 = RSW_2_1_List[t].s_204_1;
                    RSW_2_1_List[t].s_214_2 = RSW_2_1_List[t].s_204_2;
                    RSW_2_1_List[t].s_214_3 = RSW_2_1_List[t].s_204_3;
                }
                else
                {
                    //База для начисления 213 строка
                    RSW_2_1_List[t].s_213_0 = RSW_2_1_List[t].s_210_0 - RSW_2_1_List[t].s_211_0;
                    RSW_2_1_List[t].s_213_1 = RSW_2_1_List[t].s_210_1 - RSW_2_1_List[t].s_211_1;
                    RSW_2_1_List[t].s_213_2 = RSW_2_1_List[t].s_210_2 - RSW_2_1_List[t].s_211_2;
                    RSW_2_1_List[t].s_213_3 = RSW_2_1_List[t].s_210_3 - RSW_2_1_List[t].s_211_3;

                    RSW_2_1_List[t].s_215i_0 = RSW_2_1_List[t].s_207_0;
                    RSW_2_1_List[t].s_215i_1 = RSW_2_1_List[t].s_207_1;
                    RSW_2_1_List[t].s_215i_2 = RSW_2_1_List[t].s_207_2;
                    RSW_2_1_List[t].s_215i_3 = RSW_2_1_List[t].s_207_3;

                }


                var d = RSW_2_1_List[t].TariffCodeID.Value;
                TariffCode tarCode = db.TariffCode.FirstOrDefault(x => x.ID == d);
                decimal tariffStrah = 0;
                decimal tariffOMS = 0;

                if (tarCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.Any(x => x.Year.Value == identifier.Year))
                {
                    tariffStrah = tarCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.FirstOrDefault(x => x.Year.Value == identifier.Year).StrahPercant1966.Value;
                    tariffOMS = tarCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.FirstOrDefault(x => x.Year.Value == identifier.Year).FFOMS_Percent.Value;
                }
                decimal tariffMore = tariffStrah == 0 ? 0 : ((tarCode.Code == "01" || tarCode.Code == "52" || tarCode.Code == "53") ? 10 : 0);

                //Расчет взносов 

                RSW_2_1_List[t].s_205_0 = RSW_2_1_List[t].s_204_0.Value * tariffStrah / 100;
                RSW_2_1_List[t].s_205_1 = ((RSW_2_1_List[t].s_204_0.Value - RSW_2_1_List[t].s_204_2.Value - RSW_2_1_List[t].s_204_3.Value) * tariffStrah / 100) - ((RSW_2_1_List[t].s_204_0.Value - RSW_2_1_List[t].s_204_1.Value - RSW_2_1_List[t].s_204_2.Value - RSW_2_1_List[t].s_204_3.Value) * tariffStrah / 100);
                RSW_2_1_List[t].s_205_2 = (((RSW_2_1_List[t].s_204_0.Value - RSW_2_1_List[t].s_204_3.Value) * tariffStrah / 100) - ((RSW_2_1_List[t].s_204_0.Value - RSW_2_1_List[t].s_204_1.Value - RSW_2_1_List[t].s_204_2.Value - RSW_2_1_List[t].s_204_3.Value) * tariffStrah / 100)) - RSW_2_1_List[t].s_205_1.Value;
                RSW_2_1_List[t].s_205_3 = (((RSW_2_1_List[t].s_204_0.Value) * tariffStrah / 100) - ((RSW_2_1_List[t].s_204_0.Value - RSW_2_1_List[t].s_204_1.Value - RSW_2_1_List[t].s_204_2.Value - RSW_2_1_List[t].s_204_3.Value) * tariffStrah / 100)) - (RSW_2_1_List[t].s_205_1.Value + RSW_2_1_List[t].s_205_2.Value);

                RSW_2_1_List[t].s_206_0 = RSW_2_1_List[t].s_203_0.Value * tariffMore / 100;
                RSW_2_1_List[t].s_206_1 = ((RSW_2_1_List[t].s_203_0.Value - RSW_2_1_List[t].s_203_2.Value - RSW_2_1_List[t].s_203_3.Value) * tariffMore / 100) - ((RSW_2_1_List[t].s_203_0.Value - RSW_2_1_List[t].s_203_1.Value - RSW_2_1_List[t].s_203_2.Value - RSW_2_1_List[t].s_203_3.Value) * tariffMore / 100);
                RSW_2_1_List[t].s_206_2 = (((RSW_2_1_List[t].s_203_0.Value - RSW_2_1_List[t].s_203_3.Value) * tariffMore / 100) - ((RSW_2_1_List[t].s_203_0.Value - RSW_2_1_List[t].s_203_1.Value - RSW_2_1_List[t].s_203_2.Value - RSW_2_1_List[t].s_203_3.Value) * tariffMore / 100)) - RSW_2_1_List[t].s_206_1.Value;
                RSW_2_1_List[t].s_206_3 = (((RSW_2_1_List[t].s_203_0.Value) * tariffMore / 100) - ((RSW_2_1_List[t].s_203_0.Value - RSW_2_1_List[t].s_203_1.Value - RSW_2_1_List[t].s_203_2.Value - RSW_2_1_List[t].s_203_3.Value) * tariffMore / 100)) - (RSW_2_1_List[t].s_206_1.Value + RSW_2_1_List[t].s_206_2.Value);

                if (yearType != 2015)
                {
                    RSW_2_1_List[t].s_215_0 = RSW_2_1_List[t].s_214_0 * tariffOMS / 100;
                    RSW_2_1_List[t].s_215_1 = ((RSW_2_1_List[t].s_214_0 - RSW_2_1_List[t].s_214_2 - RSW_2_1_List[t].s_214_3) * tariffOMS / 100) - ((RSW_2_1_List[t].s_214_0 - RSW_2_1_List[t].s_214_1 - RSW_2_1_List[t].s_214_2 - RSW_2_1_List[t].s_214_3) * tariffOMS / 100);
                    RSW_2_1_List[t].s_215_2 = (((RSW_2_1_List[t].s_214_0 - RSW_2_1_List[t].s_214_3) * tariffOMS / 100) - ((RSW_2_1_List[t].s_214_0 - RSW_2_1_List[t].s_214_1 - RSW_2_1_List[t].s_214_2 - RSW_2_1_List[t].s_214_3) * tariffOMS / 100)) - RSW_2_1_List[t].s_215_1;
                    RSW_2_1_List[t].s_215_3 = (((RSW_2_1_List[t].s_214_0) * tariffOMS / 100) - ((RSW_2_1_List[t].s_214_0 - RSW_2_1_List[t].s_214_1 - RSW_2_1_List[t].s_214_2 - RSW_2_1_List[t].s_214_3) * tariffOMS / 100)) - (RSW_2_1_List[t].s_215_1 + RSW_2_1_List[t].s_215_2);
                }
                else
                {
                    RSW_2_1_List[t].s_214_0 = RSW_2_1_List[t].s_213_0 * tariffOMS / 100;
                    RSW_2_1_List[t].s_214_1 = ((RSW_2_1_List[t].s_213_0 - RSW_2_1_List[t].s_213_2 - RSW_2_1_List[t].s_213_3) * tariffOMS / 100) - ((RSW_2_1_List[t].s_213_0 - RSW_2_1_List[t].s_213_1 - RSW_2_1_List[t].s_213_2 - RSW_2_1_List[t].s_213_3) * tariffOMS / 100);
                    RSW_2_1_List[t].s_214_2 = (((RSW_2_1_List[t].s_213_0 - RSW_2_1_List[t].s_213_3) * tariffOMS / 100) - ((RSW_2_1_List[t].s_213_0 - RSW_2_1_List[t].s_213_1 - RSW_2_1_List[t].s_213_2 - RSW_2_1_List[t].s_213_3) * tariffOMS / 100)) - RSW_2_1_List[t].s_214_1;
                    RSW_2_1_List[t].s_214_3 = (((RSW_2_1_List[t].s_213_0) * tariffOMS / 100) - ((RSW_2_1_List[t].s_213_0 - RSW_2_1_List[t].s_213_1 - RSW_2_1_List[t].s_213_2 - RSW_2_1_List[t].s_213_3) * tariffOMS / 100)) - (RSW_2_1_List[t].s_214_1 + RSW_2_1_List[t].s_214_2);
                }

            }



            Razd_6_7_list = db.FormsRSW2014_1_Razd_6_7.Where(x => RSW_6_1_IDs.Contains(x.FormsRSW2014_1_Razd_6_1_ID)).OrderBy(x => x.SpecOcenkaUslTrudaID).ToList();

                foreach (var item in Razd_6_7_list.Where(x => x.SpecOcenkaUslTrudaID == null))
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        return;
                    }

                    RSW.s_220_1 = item.s_1_0 != null ? RSW.s_220_1 + item.s_1_0.Value : RSW.s_220_1;
                    RSW.s_220_2 = item.s_2_0 != null ? RSW.s_220_2 + item.s_2_0.Value : RSW.s_220_2;
                    RSW.s_220_3 = item.s_3_0 != null ? RSW.s_220_3 + item.s_3_0.Value : RSW.s_220_3;
                    RSW.s_230_1 = item.s_1_1 != null ? RSW.s_230_1 + item.s_1_1.Value : RSW.s_230_1;
                    RSW.s_230_2 = item.s_2_1 != null ? RSW.s_230_2 + item.s_2_1.Value : RSW.s_230_2;
                    RSW.s_230_3 = item.s_3_1 != null ? RSW.s_230_3 + item.s_3_1.Value : RSW.s_230_3;


                    if ((item.s_1_0.HasValue && item.s_1_0 != 0) || (item.s_2_0.HasValue && item.s_2_0 != 0) || (item.s_3_0.HasValue && item.s_3_0 != 0))
                    {
                        if (item.s_1_0 != 0)
                            RSW.s_225_1++;
                        if (item.s_2_0 != 0)
                            RSW.s_225_2++;
                        if (item.s_3_0 != 0)
                            RSW.s_225_3++;
                        RSW.s_225_0++;
                    }

                    if ((item.s_1_1.HasValue && item.s_1_1 != 0) || (item.s_2_1.HasValue && item.s_2_1 != 0) || (item.s_3_1.HasValue && item.s_3_1 != 0))
                    {
                        if (item.s_1_1 != 0)
                            RSW.s_235_1++;
                        if (item.s_2_1 != 0)
                            RSW.s_235_2++;
                        if (item.s_3_1 != 0)
                            RSW.s_235_3++;
                        RSW.s_235_0++;
                    }


                    k++;
                    decimal temp = (decimal)k / (decimal)Razd_6_7_list.Count();
                    int proc = (int)Math.Round((temp * 100), 0);
                    backgroundWorker1.ReportProgress(proc, k.ToString());
                }

            if (RSW_Prev != null)
            {
                RSW.s_220_0 = RSW_Prev.s_220_0.HasValue ? RSW_Prev.s_220_0.Value + RSW.s_220_1 + RSW.s_220_2 + RSW.s_220_3 : RSW.s_220_1 + RSW.s_220_2 + RSW.s_220_3;
                RSW.s_221_0 = RSW_Prev.s_221_0.HasValue ? RSW_Prev.s_221_0.Value + RSW.s_221_1 + RSW.s_221_2 + RSW.s_221_3 : RSW.s_221_1 + RSW.s_221_2 + RSW.s_221_3;
                RSW.s_230_0 = RSW_Prev.s_230_0.HasValue ? RSW_Prev.s_230_0.Value + RSW.s_230_1 + RSW.s_230_2 + RSW.s_230_3 : RSW.s_230_1 + RSW.s_230_2 + RSW.s_230_3;
                RSW.s_231_0 = RSW_Prev.s_231_0.HasValue ? RSW_Prev.s_231_0.Value + RSW.s_231_1 + RSW.s_231_2 + RSW.s_231_3 : RSW.s_231_1 + RSW.s_231_2 + RSW.s_231_3;
            }
            else
            {
                RSW.s_220_0 = RSW.s_220_1 + RSW.s_220_2 + RSW.s_220_3;
                RSW.s_221_0 = RSW.s_221_1 + RSW.s_221_2 + RSW.s_221_3;
                RSW.s_230_0 = RSW.s_230_1 + RSW.s_230_2 + RSW.s_230_3;
                RSW.s_231_0 = RSW.s_231_1 + RSW.s_231_2 + RSW.s_231_3;
            }

            RSW.s_223_1 = RSW.s_220_1 - RSW.s_221_1;
            RSW.s_223_2 = RSW.s_220_2 - RSW.s_221_2;
            RSW.s_223_3 = RSW.s_220_3 - RSW.s_221_3;
            RSW.s_233_1 = RSW.s_230_1 - RSW.s_231_1;
            RSW.s_233_2 = RSW.s_230_2 - RSW.s_231_2;
            RSW.s_233_3 = RSW.s_230_3 - RSW.s_231_3;

            if (RSW_Prev != null)
            {
                RSW.s_223_0 = RSW_Prev.s_223_0.HasValue ? RSW_Prev.s_223_0.Value + RSW.s_220_1 + RSW.s_220_2 + RSW.s_223_3 : RSW.s_223_1 + RSW.s_223_2 + RSW.s_223_3;
                RSW.s_233_0 = RSW_Prev.s_233_0.HasValue ? RSW_Prev.s_233_0.Value + RSW.s_230_1 + RSW.s_230_2 + RSW.s_233_3 : RSW.s_233_1 + RSW.s_233_2 + RSW.s_233_3;
            }
            else
            {
                RSW.s_223_0 = RSW.s_223_1 + RSW.s_223_2 + RSW.s_223_3;
                RSW.s_233_0 = RSW.s_233_1 + RSW.s_233_2 + RSW.s_233_3;
            }


            decimal tariff1 = 0;
            decimal tariff2 = 0;
            if (db.DopTariff.Any(x => x.Year == RSW.Year))
            {
                tariff1 = db.DopTariff.FirstOrDefault(x => x.Year == RSW.Year).Tariff1.HasValue ? db.DopTariff.FirstOrDefault(x => x.Year == RSW.Year).Tariff1.Value : 0;
                tariff2 = db.DopTariff.FirstOrDefault(x => x.Year == RSW.Year).Tariff2.HasValue ? db.DopTariff.FirstOrDefault(x => x.Year == RSW.Year).Tariff2.Value : 0;
            }

            RSW.s_224_0 = RSW.s_223_0 * tariff1 / 100;
            RSW.s_224_1 = ((RSW.s_223_0 - RSW.s_223_2 - RSW.s_223_3) * tariff1 / 100) - ((RSW.s_223_0 - RSW.s_223_1 - RSW.s_223_2 - RSW.s_223_3) * tariff1 / 100);
            RSW.s_224_2 = (((RSW.s_223_0 - RSW.s_223_3) * tariff1 / 100) - ((RSW.s_223_0 - RSW.s_223_1 - RSW.s_223_2 - RSW.s_223_3) * tariff1 / 100)) - RSW.s_224_1;
            RSW.s_224_3 = ((RSW.s_223_0 * tariff1 / 100) - ((RSW.s_223_0 - RSW.s_223_1 - RSW.s_223_2 - RSW.s_223_3) * tariff1 / 100)) - (RSW.s_224_1 + RSW.s_224_2);

            RSW.s_234_0 = RSW.s_233_0 * tariff2 / 100;
            RSW.s_234_1 = ((RSW.s_233_0 - RSW.s_233_2 - RSW.s_233_3) * tariff2 / 100) - ((RSW.s_233_0 - RSW.s_233_1 - RSW.s_233_2 - RSW.s_233_3) * tariff2 / 100);
            RSW.s_234_2 = (((RSW.s_233_0 - RSW.s_233_3) * tariff2 / 100) - ((RSW.s_233_0 - RSW.s_233_1 - RSW.s_233_2 - RSW.s_233_3) * tariff2 / 100)) - RSW.s_234_1;
            RSW.s_234_3 = ((RSW.s_233_0 * tariff2 / 100) - ((RSW.s_233_0 - RSW.s_233_1 - RSW.s_233_2 - RSW.s_233_3) * tariff2 / 100)) - (RSW.s_234_1 + RSW.s_234_2);


            #endregion
            

            #region Формирование раздела 2.4
            if (GetRazd_2_4_CheckBox.Checked)
            {
                i = 0;
                k = 0;
                id = 0;
                backgroundWorker1.ReportProgress(0, k.ToString());
                child.Invoke(new Action(() => { child.secondPartLabel.Text = Razd_6_7_list.Where(x => x.SpecOcenkaUslTrudaID != null).Count().ToString(); }));

                for (int n = 1; n <= 2; n++)
                {
                    FormsRSW2014_1_Razd_2_4 rsw24_ = new FormsRSW2014_1_Razd_2_4 { };
                    RSW_2_4_List.Add(rsw24_.SetZeroValues());
                }



                if (Razd_6_7_list.Any(x => x.SpecOcenkaUslTrudaID != null && (x.s_0_0 != 0 || x.s_1_0 != 0 || x.s_2_0 != 0 || x.s_3_0 != 0)))
                {
                    RSW_2_4_List[0].CodeBase = 1;
                    RSW_2_4_List[0].InsurerID = identifier.InsurerID;
                    RSW_2_4_List[0].CorrectionNum = identifier.CorrectionNum;
                    RSW_2_4_List[0].Quarter = identifier.Quarter;
                    RSW_2_4_List[0].Year = identifier.Year;
                    RSW_2_4_List[0].FilledBase = byte.Parse(dopTariffDDL.SelectedItem.Tag.ToString());


                    if (Options.inputTypeRSW1 != 0)
                    {
                        byte q = identifier.Quarter;
                        if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                        {
                            short year = identifier.Year;
                            byte quarter = 20;
                            if (q == 6)
                                quarter = 3;
                            else if (q == 9)
                                quarter = 6;
                            else if (q == 0)
                                quarter = 9;

                            byte codeBase = RSW_2_4_List[0].CodeBase.Value;
                            byte filledBase = RSW_2_4_List[0].FilledBase.Value;
                            if (db.FormsRSW2014_1_Razd_2_4.Any(x => x.Year == year && x.Quarter == quarter && x.CodeBase == codeBase && x.FilledBase == filledBase))
                                RSW_2_4_Prev0 = db.FormsRSW2014_1_Razd_2_4.Where(x => x.Year == year && x.Quarter == quarter && x.CodeBase == codeBase && x.FilledBase == filledBase).OrderByDescending(x => x.CorrectionNum).First();
                        }
                        else
                        {
                            RSW_2_4_Prev0 = null;
                        }
                    }
                    else
                    {
                        RSW_2_4_Prev0 = null;
                    }


                }
                if (Razd_6_7_list.Any(x => x.SpecOcenkaUslTrudaID != null && (x.s_0_1 != 0 || x.s_1_1 != 0 || x.s_2_1 != 0 || x.s_3_1 != 0)))
                {
                    RSW_2_4_List[1].CodeBase = 2;
                    RSW_2_4_List[1].InsurerID = identifier.InsurerID;
                    RSW_2_4_List[1].CorrectionNum = identifier.CorrectionNum;
                    RSW_2_4_List[1].Quarter = identifier.Quarter;
                    RSW_2_4_List[1].Year = identifier.Year;
                    RSW_2_4_List[1].FilledBase = byte.Parse(dopTariffDDL.SelectedItem.Tag.ToString());


                    if (Options.inputTypeRSW1 != 0)
                    {
                        byte q = identifier.Quarter;
                        if (q != 3) // Если не первый отчетный период в году тогда ищем РСВ за предыдущие периоды
                        {
                            short year = identifier.Year;
                            byte quarter = 20;
                            if (q == 6)
                                quarter = 3;
                            else if (q == 9)
                                quarter = 6;
                            else if (q == 0)
                                quarter = 9;

                            byte codeBase = RSW_2_4_List[1].CodeBase.Value;
                            byte filledBase = RSW_2_4_List[1].FilledBase.Value;
                            if (db.FormsRSW2014_1_Razd_2_4.Any(x => x.Year == year && x.Quarter == quarter && x.CodeBase == codeBase && x.FilledBase == filledBase))
                                RSW_2_4_Prev1 = db.FormsRSW2014_1_Razd_2_4.Where(x => x.Year == year && x.Quarter == quarter && x.CodeBase == codeBase && x.FilledBase == filledBase).OrderByDescending(x => x.CorrectionNum).First();
                        }
                        else
                        {
                            RSW_2_4_Prev1 = null;
                        }
                    }
                    else
                    {
                        RSW_2_4_Prev1 = null;
                    }
                }

                k = 0;
                decimal cnt = Razd_6_7_list.Where(x => x.SpecOcenkaUslTrudaID != null).Count();
                foreach (var item in Razd_6_7_list.Where(x => x.SpecOcenkaUslTrudaID != null))
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        RSW_2_4_List.Clear();
                        return;
                    }



                    RSW_2_4_List[0].AutoCalc = true;
                    RSW_2_4_List[1].AutoCalc = true;

                    switch (item.SpecOcenkaUslTruda.Code)
                    {
                        case "О4":
                            if (item.s_1_0 != 0 || item.s_2_0 != 0 || item.s_3_0 != 0)
                            {
                                if (item.s_1_0 != null)
                                {
                                    RSW_2_4_List[0].s_240_1 = RSW_2_4_List[0].s_240_1 + item.s_1_0.Value;
                                    if (item.s_1_0 != 0)
                                        RSW_2_4_List[0].s_245_1++;
                                }
                                if (item.s_2_0 != null)
                                {
                                    RSW_2_4_List[0].s_240_2 = RSW_2_4_List[0].s_240_2 + item.s_2_0.Value;
                                    if (item.s_2_0 != 0)
                                        RSW_2_4_List[0].s_245_2++;
                                }
                                if (item.s_3_0 != null)
                                {
                                    RSW_2_4_List[0].s_240_3 = RSW_2_4_List[0].s_240_3 + item.s_3_0.Value;
                                    if (item.s_3_0 != 0)
                                        RSW_2_4_List[0].s_245_3++;
                                }
                                RSW_2_4_List[0].s_245_0++;
                            }
                            if (item.s_1_1 != 0 || item.s_2_1 != 0 || item.s_3_1 != 0)
                            {
                                if (item.s_1_1 != null)
                                {
                                    RSW_2_4_List[1].s_240_1 = RSW_2_4_List[1].s_240_1 + item.s_1_1.Value;
                                    if (item.s_1_1 != 0)
                                        RSW_2_4_List[1].s_245_1++;
                                }
                                if (item.s_2_1 != null)
                                {
                                    RSW_2_4_List[1].s_240_2 = RSW_2_4_List[1].s_240_2 + item.s_2_1.Value;
                                    if (item.s_2_1 != 0)
                                        RSW_2_4_List[1].s_245_2++;
                                }
                                if (item.s_3_1 != null)
                                {
                                    RSW_2_4_List[1].s_240_3 = RSW_2_4_List[1].s_240_3 + item.s_3_1.Value;
                                    if (item.s_3_1 != 0)
                                        RSW_2_4_List[1].s_245_3++;
                                }
                                RSW_2_4_List[1].s_245_0++;
                            }
                            break;
                        case "В3.4":
                            if (item.s_1_0 != 0 || item.s_2_0 != 0 || item.s_3_0 != 0)
                            {
                                if (item.s_1_0 != null)
                                {
                                    RSW_2_4_List[0].s_246_1 = RSW_2_4_List[0].s_246_1 + item.s_1_0.Value;
                                    if (item.s_1_0 != 0)
                                        RSW_2_4_List[0].s_251_1++;
                                }
                                if (item.s_2_0 != null)
                                {
                                    RSW_2_4_List[0].s_246_2 = RSW_2_4_List[0].s_246_2 + item.s_2_0.Value;
                                    if (item.s_2_0 != 0)
                                        RSW_2_4_List[0].s_251_2++;
                                }
                                if (item.s_3_0 != null)
                                {
                                    RSW_2_4_List[0].s_246_3 = RSW_2_4_List[0].s_246_3 + item.s_3_0.Value;
                                    if (item.s_3_0 != 0)
                                        RSW_2_4_List[0].s_251_3++;
                                }
                                RSW_2_4_List[0].s_251_0++;

                            }
                            if (item.s_1_1 != 0 || item.s_2_1 != 0 || item.s_3_1 != 0)
                            {
                                if (item.s_1_1 != null)
                                {
                                    RSW_2_4_List[1].s_246_1 = RSW_2_4_List[1].s_246_1 + item.s_1_1.Value;
                                    if (item.s_1_1 != 0)
                                        RSW_2_4_List[1].s_251_1++;
                                }
                                if (item.s_2_1 != null)
                                {
                                    RSW_2_4_List[1].s_246_2 = RSW_2_4_List[1].s_246_2 + item.s_2_1.Value;
                                    if (item.s_2_1 != 0)
                                        RSW_2_4_List[1].s_251_2++;
                                }
                                if (item.s_3_1 != null)
                                {
                                    RSW_2_4_List[1].s_246_3 = RSW_2_4_List[1].s_246_3 + item.s_3_1.Value;
                                    if (item.s_3_1 != 0)
                                        RSW_2_4_List[1].s_251_3++;
                                }
                                RSW_2_4_List[1].s_251_0++;

                            }
                            break;
                        case "В3.3":
                            if (item.s_1_0 != 0 || item.s_2_0 != 0 || item.s_3_0 != 0)
                            {
                                if (item.s_1_0 != null)
                                {
                                    RSW_2_4_List[0].s_252_1 = RSW_2_4_List[0].s_252_1 + item.s_1_0.Value;
                                    if (item.s_1_0 != 0)
                                        RSW_2_4_List[0].s_257_1++;
                                }
                                if (item.s_2_0 != null)
                                {
                                    RSW_2_4_List[0].s_252_2 = RSW_2_4_List[0].s_252_2 + item.s_2_0.Value;
                                    if (item.s_2_0 != 0)
                                        RSW_2_4_List[0].s_257_2++;
                                }
                                if (item.s_3_0 != null)
                                {
                                    RSW_2_4_List[0].s_252_3 = RSW_2_4_List[0].s_252_3 + item.s_3_0.Value;
                                    if (item.s_3_0 != 0)
                                        RSW_2_4_List[0].s_257_3++;
                                }
                                RSW_2_4_List[0].s_257_0++;
                            }
                            if (item.s_1_1 != 0 || item.s_2_1 != 0 || item.s_3_1 != 0)
                            {
                                if (item.s_1_1 != null)
                                {
                                    RSW_2_4_List[1].s_252_1 = RSW_2_4_List[1].s_252_1 + item.s_1_1.Value;
                                    if (item.s_1_1 != 0)
                                        RSW_2_4_List[1].s_257_1++;
                                }
                                if (item.s_2_1 != null)
                                {
                                    RSW_2_4_List[1].s_252_2 = RSW_2_4_List[1].s_252_2 + item.s_2_1.Value;
                                    if (item.s_2_1 != 0)
                                        RSW_2_4_List[1].s_257_2++;
                                }
                                if (item.s_3_1 != null)
                                {
                                    RSW_2_4_List[1].s_252_3 = RSW_2_4_List[1].s_252_3 + item.s_3_1.Value;
                                    if (item.s_3_1 != 0)
                                        RSW_2_4_List[1].s_257_3++;
                                }
                                RSW_2_4_List[1].s_257_0++;
                            }
                            break;
                        case "В3.2":
                            if (item.s_1_0 != 0 || item.s_2_0 != 0 || item.s_3_0 != 0)
                            {
                                if (item.s_1_0 != null)
                                {
                                    RSW_2_4_List[0].s_258_1 = RSW_2_4_List[0].s_258_1 + item.s_1_0.Value;
                                    if (item.s_1_0 != 0)
                                        RSW_2_4_List[0].s_263_1++;
                                }
                                if (item.s_2_0 != null)
                                {
                                    RSW_2_4_List[0].s_258_2 = RSW_2_4_List[0].s_258_2 + item.s_2_0.Value;
                                    if (item.s_2_0 != 0)
                                        RSW_2_4_List[0].s_263_2++;
                                }
                                if (item.s_3_0 != null)
                                {
                                    RSW_2_4_List[0].s_258_3 = RSW_2_4_List[0].s_258_3 + item.s_3_0.Value;
                                    if (item.s_3_0 != 0)
                                        RSW_2_4_List[0].s_263_3++;
                                }
                                RSW_2_4_List[0].s_263_0++;
                            }
                            if (item.s_1_1 != 0 || item.s_2_1 != 0 || item.s_3_1 != 0)
                            {
                                if (item.s_1_1 != null)
                                {
                                    RSW_2_4_List[1].s_258_1 = RSW_2_4_List[1].s_258_1 + item.s_1_1.Value;
                                    if (item.s_1_1 != 0)
                                        RSW_2_4_List[1].s_263_1++;
                                }
                                if (item.s_2_1 != null)
                                {
                                    RSW_2_4_List[1].s_258_2 = RSW_2_4_List[1].s_258_2 + item.s_2_1.Value;
                                    if (item.s_2_1 != 0)
                                        RSW_2_4_List[1].s_263_2++;
                                }
                                if (item.s_3_1 != null)
                                {
                                    RSW_2_4_List[1].s_258_3 = RSW_2_4_List[1].s_258_3 + item.s_3_1.Value;
                                    if (item.s_3_1 != 0)
                                        RSW_2_4_List[1].s_263_3++;
                                }
                                RSW_2_4_List[1].s_263_0++;
                            }

                            break;
                        case "В3.1":
                            if (item.s_1_0 != 0 || item.s_2_0 != 0 || item.s_3_0 != 0)
                            {
                                if (item.s_1_0 != null)
                                {
                                    RSW_2_4_List[0].s_264_1 = RSW_2_4_List[0].s_264_1 + item.s_1_0.Value;
                                    if (item.s_1_0 != 0)
                                        RSW_2_4_List[0].s_269_1++;
                                }
                                if (item.s_2_0 != null)
                                {
                                    RSW_2_4_List[0].s_264_2 = RSW_2_4_List[0].s_264_2 + item.s_2_0.Value;
                                    if (item.s_2_0 != 0)
                                        RSW_2_4_List[0].s_269_2++;
                                }
                                if (item.s_3_0 != null)
                                {
                                    RSW_2_4_List[0].s_264_3 = RSW_2_4_List[0].s_264_3 + item.s_3_0.Value;
                                    if (item.s_3_0 != 0)
                                        RSW_2_4_List[0].s_269_3++;
                                }
                                RSW_2_4_List[0].s_269_0++;

                            }
                            if (item.s_1_1 != 0 || item.s_2_1 != 0 || item.s_3_1 != 0)
                            {
                                if (item.s_1_1 != null)
                                {
                                    RSW_2_4_List[1].s_264_1 = RSW_2_4_List[1].s_264_1 + item.s_1_1.Value;
                                    if (item.s_1_1 != 0)
                                        RSW_2_4_List[1].s_269_1++;
                                }
                                if (item.s_2_1 != null)
                                {
                                    RSW_2_4_List[1].s_264_2 = RSW_2_4_List[1].s_264_2 + item.s_2_1.Value;
                                    if (item.s_2_1 != 0)
                                        RSW_2_4_List[1].s_269_2++;
                                }
                                if (item.s_3_1 != null)
                                {
                                    RSW_2_4_List[1].s_264_3 = RSW_2_4_List[1].s_264_3 + item.s_3_1.Value;
                                    if (item.s_3_1 != 0)
                                        RSW_2_4_List[1].s_269_3++;
                                }
                                RSW_2_4_List[1].s_269_0++;
                            }

                            break;
                    }

                    k++;
                    decimal temp = (decimal)k / cnt;
                    int proc = (int)Math.Round((temp * 100), 0);
                    backgroundWorker1.ReportProgress(proc, k.ToString());
                }
                calculation_2_4();

            }
                #endregion

            if (RSW.s_220_0 != 0)
                RSW.ExistPart_2_2 = (byte)1;
            if (RSW.s_230_0 != 0)
                RSW.ExistPart_2_3 = (byte)1;
            if (RSW_2_4_List.Count > 0)
                RSW.ExistPart_2_4 = (byte)1;

            foreach (var item in RSW_2_1_List)
            {
//                RSW.s_110_0 = RSW.s_110_0 + item.s_205_0 + item.s_206_0;
                RSW.s_111_0 = RSW.s_111_0 + item.s_205_1 + item.s_206_1;
                RSW.s_112_0 = RSW.s_112_0 + item.s_205_2 + item.s_206_2;
                RSW.s_113_0 = RSW.s_113_0 + item.s_205_3 + item.s_206_3;

                if (yearType != 2015)
                {
                    //                RSW.s_110_5 = RSW.s_110_5 + item.s_215_0;
                    RSW.s_111_5 = RSW.s_111_5 + item.s_215_1;
                    RSW.s_112_5 = RSW.s_112_5 + item.s_215_2;
                    RSW.s_113_5 = RSW.s_113_5 + item.s_215_3;
                }
                else
                {
                    //                RSW.s_110_5 = RSW.s_110_5 + item.s_215_0;
                    RSW.s_111_5 = RSW.s_111_5 + item.s_214_1;
                    RSW.s_112_5 = RSW.s_112_5 + item.s_214_2;
                    RSW.s_113_5 = RSW.s_113_5 + item.s_214_3;
                }
            }

            if (RSW_Prev != null)
            {
                RSW.s_110_0 = RSW_Prev.s_110_0.HasValue ? (RSW_Prev.s_110_0.Value + RSW.s_110_0) : RSW.s_110_0;
                RSW.s_110_5 = RSW_Prev.s_110_5.HasValue ? (RSW_Prev.s_110_5.Value + RSW.s_110_5) : RSW.s_110_5;
            }

            RSW.s_110_0 = RSW.s_110_0 + RSW.s_111_0 + RSW.s_112_0 + RSW.s_113_0;
            RSW.s_110_5 = RSW.s_110_5 + RSW.s_111_5 + RSW.s_112_5 + RSW.s_113_5;


            //Строка 110,111,112,113 графа 6
            RSW.s_110_3 = RSW.s_224_0;
            RSW.s_111_3 = RSW.s_224_1;
            RSW.s_112_3 = RSW.s_224_2;
            RSW.s_113_3 = RSW.s_224_3;

            foreach (var item in RSW_2_4_List.Where(x => x.CodeBase == 1))
            {
                RSW.s_110_3 = RSW.s_110_3 + item.s_244_0 + item.s_250_0 + item.s_256_0 + item.s_262_0 + item.s_268_0;
                RSW.s_111_3 = RSW.s_111_3 + item.s_244_1 + item.s_250_1 + item.s_256_1 + item.s_262_1 + item.s_268_1;
                RSW.s_112_3 = RSW.s_112_3 + item.s_244_2 + item.s_250_2 + item.s_256_2 + item.s_262_2 + item.s_268_2;
                RSW.s_113_3 = RSW.s_113_3 + item.s_244_3 + item.s_250_3 + item.s_256_3 + item.s_262_3 + item.s_268_3;
            }

            //Строка 110,111,112,113 графа 7
            RSW.s_110_4 = RSW.s_234_0;
            RSW.s_111_4 = RSW.s_234_1;
            RSW.s_112_4 = RSW.s_234_2;
            RSW.s_113_4 = RSW.s_234_3;

            foreach (var item in RSW_2_4_List.Where(x => x.CodeBase == 2))
            {
                RSW.s_110_4 = RSW.s_110_4 + item.s_244_0 + item.s_250_0 + item.s_256_0 + item.s_262_0 + item.s_268_0;
                RSW.s_111_4 = RSW.s_111_4 + item.s_244_1 + item.s_250_1 + item.s_256_1 + item.s_262_1 + item.s_268_1;
                RSW.s_112_4 = RSW.s_112_4 + item.s_244_2 + item.s_250_2 + item.s_256_2 + item.s_262_2 + item.s_268_2;
                RSW.s_113_4 = RSW.s_113_4 + item.s_244_3 + item.s_250_3 + item.s_256_3 + item.s_262_3 + item.s_268_3;
            }

            RSW.s_114_0 = RSW.s_111_0 + RSW.s_112_0 + RSW.s_113_0;
            RSW.s_114_3 = RSW.s_111_3 + RSW.s_112_3 + RSW.s_113_3;
            RSW.s_114_4 = RSW.s_111_4 + RSW.s_112_4 + RSW.s_113_4;
            RSW.s_114_5 = RSW.s_111_5 + RSW.s_112_5 + RSW.s_113_5;

            RSW.s_130_0 = RSW.s_100_0 + RSW.s_110_0 + RSW.s_120_0;
            RSW.s_130_1 = RSW.s_100_1 + RSW.s_120_1;
            RSW.s_130_2 = RSW.s_100_2 + RSW.s_120_2;
            RSW.s_130_3 = RSW.s_100_3 + RSW.s_110_3 + RSW.s_120_3;
            RSW.s_130_4 = RSW.s_100_4 + RSW.s_110_4 + RSW.s_120_4;
            RSW.s_130_5 = RSW.s_100_5 + RSW.s_110_5 + RSW.s_120_5;

            RSW.s_144_0 = RSW.s_141_0 + RSW.s_142_0 + RSW.s_143_0;
            RSW.s_144_1 = RSW.s_141_1 + RSW.s_142_1 + RSW.s_143_1;
            RSW.s_144_2 = RSW.s_141_2 + RSW.s_142_2 + RSW.s_143_2;
            RSW.s_144_3 = RSW.s_141_3 + RSW.s_142_3 + RSW.s_143_3;
            RSW.s_144_4 = RSW.s_141_4 + RSW.s_142_4 + RSW.s_143_4;
            RSW.s_144_5 = RSW.s_141_5 + RSW.s_142_5 + RSW.s_143_5;

            if (RSW_Prev != null)
            {
                RSW.s_140_0 = RSW_Prev.s_140_0.HasValue ? RSW_Prev.s_140_0.Value + RSW.s_144_0 : RSW.s_144_0;
                RSW.s_140_1 = RSW_Prev.s_140_1.HasValue ? RSW_Prev.s_140_1.Value + RSW.s_144_1 : RSW.s_144_1;
                RSW.s_140_2 = RSW_Prev.s_140_2.HasValue ? RSW_Prev.s_140_2.Value + RSW.s_144_2 : RSW.s_144_2;
                RSW.s_140_3 = RSW_Prev.s_140_3.HasValue ? RSW_Prev.s_140_3.Value + RSW.s_144_3 : RSW.s_144_3;
                RSW.s_140_4 = RSW_Prev.s_140_4.HasValue ? RSW_Prev.s_140_4.Value + RSW.s_144_4 : RSW.s_144_4;
                RSW.s_140_5 = RSW_Prev.s_140_5.HasValue ? RSW_Prev.s_140_5.Value + RSW.s_144_5 : RSW.s_144_5;
            }
            else
            {
                RSW.s_140_0 = RSW.s_144_0;
                RSW.s_140_1 = RSW.s_144_1;
                RSW.s_140_2 = RSW.s_144_2;
                RSW.s_140_3 = RSW.s_144_3;
                RSW.s_140_4 = RSW.s_144_4;
                RSW.s_140_5 = RSW.s_144_5;
            }

            RSW.s_150_0 = RSW.s_130_0 - RSW.s_140_0;
            RSW.s_150_1 = RSW.s_130_1 - RSW.s_140_1;
            RSW.s_150_2 = RSW.s_130_2 - RSW.s_140_2;
            RSW.s_150_3 = RSW.s_130_3 - RSW.s_140_3;
            RSW.s_150_4 = RSW.s_130_4 - RSW.s_140_4;
            RSW.s_150_5 = RSW.s_130_5 - RSW.s_140_5;

            var fieldsRSW = typeof(FormsRSW2014_1_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var namesRSW = Array.ConvertAll(fieldsRSW, field => field.Name);

            foreach (var itemName_ in namesRSW)
            {
                string itemName = itemName_.TrimStart('_');
                var properties = RSW.GetType().GetProperty(itemName);
                if (properties != null)
                {
                    object value = properties.GetValue(RSW, null);

                    string type = properties.PropertyType.FullName;
                    if (type.Contains("["))
                        type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                    type = type.Split(',')[0].Split('.')[1].ToLower();

                    switch (type)
                    {
                        case "decimal":
                            properties.SetValue(RSW, value != null ? Math.Round((decimal)value, 2, MidpointRounding.AwayFromZero) : (decimal)0, null);
                            break;
                    }


                }

            }


            db = null;
            db = new pu6Entities();

            db.FormsRSW2014_1_1.Add(RSW);
            foreach (var item in RSW_2_1_List)
            {
                /*          item.CorrectionNum = RSWdata.CorrectionNum;
                          item.Year = RSWdata.Year;
                          item.Quarter = RSWdata.Quarter;
                          item.InsurerID = RSWdata.InsurerID;
                          */
                FormsRSW2014_1_Razd_2_1 r = new FormsRSW2014_1_Razd_2_1();

                var fields = typeof(FormsRSW2014_1_Razd_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var names = Array.ConvertAll(fields, field => field.Name);

                foreach (var itemName_ in names)
                {
                    string itemName = itemName_.TrimStart('_');
                    var properties = item.GetType().GetProperty(itemName);
                    if (properties != null)
                    {
                        object value = properties.GetValue(item, null);
                        
                        string type = properties.PropertyType.FullName;
                        if (type.Contains("["))
                            type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                        type = type.Split(',')[0].Split('.')[1].ToLower();

                        switch (type)
                        {
                            case "decimal":
                                properties.SetValue(r, value != null ? Math.Round((decimal)value, 2, MidpointRounding.AwayFromZero) : (decimal)0, null);
                                break;
                            default:
                                var data = value;
                                properties.SetValue(r, data, null);
                                break;
                        }

                        
                    }

                }

                db.FormsRSW2014_1_Razd_2_1.Add(r);
            }

            foreach (var item in RSW_2_4_List.Where(x => x.InsurerID != null))
            {
                FormsRSW2014_1_Razd_2_4 r = new FormsRSW2014_1_Razd_2_4();

                var fields = typeof(FormsRSW2014_1_Razd_2_4).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var names = Array.ConvertAll(fields, field => field.Name);

                foreach (var itemName_ in names)
                {
                    string itemName = itemName_.TrimStart('_');
                    var properties = item.GetType().GetProperty(itemName);
                    if (properties != null)
                    {
                        object value = properties.GetValue(item, null);

                        string type = properties.PropertyType.FullName;
                        if (type.Contains("["))
                            type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                        type = type.Split(',')[0].Split('.')[1].ToLower();

                        switch (type)
                        {
                            case "decimal":
                                properties.SetValue(r, value != null ? Math.Round((decimal)value, 2, MidpointRounding.AwayFromZero) : (decimal)0, null);
                                break;
                            default:
                                var data = value;
                                properties.SetValue(r, data, null);
                                break;
                        }

                    }

                }

                db.FormsRSW2014_1_Razd_2_4.Add(r);
            }

            try {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("При сохранении данных произошла ошибка. Код ошибки: " + ex.Message);
            }

        }


        /// <summary>
        /// Обновление поля Начислено страховых взносов на ОПС
        /// </summary>
        private decimal UpdateSumFeePFR(List<FormsRSW2014_1_Razd_6_4> RSW_6_4_List, short y)
        {
            decimal sum = 0;
            if ((RSW_6_4_List != null) && (RSW_6_4_List.Count != 0))
            {
                foreach (var item in RSW_6_4_List)
                {
                    if ((item.PlatCategory.TariffPlat.Any(x => x.Year == y)) && (item.PlatCategory.TariffPlat.First(x => x.Year == y).StrahPercant1966 != null))
                    {
                        sum = sum + ((item.s_1_1.Value + item.s_2_1.Value + item.s_3_1.Value) * item.PlatCategory.TariffPlat.First(x => x.Year == y).StrahPercant1966.Value / 100);

                    }
                    else
                    {
                    }
                }
            }
            else
            {
                sum = 0;
            }

            return (sum);

        }

        private void getBase(string Year)
        {
            if (!String.IsNullOrEmpty(Year))
            {
                short y;

                if (short.TryParse(Year, out y))
                {
                    if (db.MROT.Any(x => x.Year == y))
                    {
                        mrot = db.MROT.First(x => x.Year == y);
                        predBase.Text = "Предельная база: " + mrot.NalogBase.ToString() + "рублей в " + mrot.Year.ToString() + " финансовом году";
                    }
                    else
                    {
                        predBase.Text = "Предельная база: Ошибка при получении данных";
                        mrot = new MROT { };
                    }

                }
            }
            else
            {
                mrot = new MROT { };
                predBase.Text = "Предельная база: Ошибка при получении данных";
            }
        }


        private void RSW2014_Filling_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014 && x.Year < 2019).OrderBy(x => x.Year);
            identifier = new Identifier
            {
                InsurerID = Options.InsID
            };
            // выпад список "календарный год"

            this.Year.Items.Clear();
            
            foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }


            if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                Year.Items.OrderByDescending(x => x.Value).First().Selected = true;

            short y;
            if (short.TryParse(Year.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }

                DateTime dt = DateTime.Now.AddDays(-45);

                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                else
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                if (rp != null)
                    Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                else
                    Quarter.Items.OrderByDescending(x => x.Value).First().Selected = true;

                identifier.Year = y;
                identifier.Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString());

                getBase(Year.Text);
                getPrevData();
            }


            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();

            
            foreach (var item in db.TypeInfo)
            {
                TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }
            TypeInfo.SelectedIndex = 0;


        }

        private void Year_SelectedIndexChanged()
        {


            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            // выпад список "Отчетный период"

            this.Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.Text, out y))
            {
                getBase(Year.Text);

                var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);

                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                if (Quarter.Items.Count() > 0)
                {
                    if (q != 20 && Quarter.Items.Any(x => x.Value.ToString() == q.ToString()))
                        Quarter.Items.FirstOrDefault(x => x.Value.ToString() == q.ToString()).Selected = true;
                    else
                        Quarter.Items.First().Selected = true;
                }

                identifier.Year = y;
                identifier.Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString());

            }

            getPrevData();
        }

        private void Quarter_SelectedIndexChanged()
        {
            if (Quarter.SelectedItem != null)
            {
                short y;
                if (short.TryParse(Year.SelectedItem.Text, out y))
                {
                    identifier.Year = y;
                    identifier.Quarter = byte.Parse(Quarter.SelectedItem.Value.ToString());
                }

                getPrevData();
            }
        }

        private void CorrectionNum_ValueChanged(object sender, EventArgs e)
        {
            if (CorrectionNum.Value == 0)
            {
                CorrectionType.Enabled = false;
                CorrectionType.Text = "";
            }
            else
            {
                CorrectionType.Enabled = true;
            }

        }

        private void GetRazd_2_4_CheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (GetRazd_2_4_CheckBox.Checked)
                dopTariffDDL.Enabled = true;
            else
                dopTariffDDL.Enabled = false;
        }

        private void RSW2014_Filling_FormClosing(object sender, FormClosingEventArgs e)
        {
            db.Dispose();
        }


    }
}
