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
using PU.Classes;

namespace PU.FormsRSW2014
{
    public partial class RSW2014_2_1_Filling : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public Identifier identifier { get; set; }
        public short yearType = 2014;
        public List<FormsRSW2014_1_Razd_2_1> RSW_2_1_List = new List<FormsRSW2014_1_Razd_2_1>();
        private List<PlatCatTariffCode> platCatTariffCode = new List<PlatCatTariffCode>();
        private List<TariffCodePlatCat> tariffCode = new List<TariffCodePlatCat>();
        private List<long> RSW_6_1_IDs = new List<long>();
        RSW2014_2_1_Filling_Wait child = new RSW2014_2_1_Filling_Wait();


        public RSW2014_2_1_Filling()
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
                RSW_2_1_List = null;
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            RSW_2_1_List = null;
            this.Close();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            // Выбор из базы Индивидуальных сведений и запонение Раздела 2.1
            if (db.FormsRSW2014_1_Razd_6_1.Any(x => x.InsurerID == identifier.InsurerID && x.Quarter == identifier.Quarter && x.Year == identifier.Year && x.TypeInfoID == 1))
            {
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


                RSW_6_1_IDs = db.FormsRSW2014_1_Razd_6_1.Where(y => y.InsurerID == identifier.InsurerID && y.TypeInfoID == 1 && y.Quarter == identifier.Quarter && y.Year == identifier.Year).Select(y => y.ID).ToList();

                if (db.FormsRSW2014_1_Razd_6_4.Any(x => RSW_6_1_IDs.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)))
                {
                    int cnt = db.FormsRSW2014_1_Razd_6_4.Where(x => RSW_6_1_IDs.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).Count();

                    backgroundWorker1.RunWorkerAsync();

                    child.Owner = this;
                    child.ThemeName = this.ThemeName;
                    child.secondPartLabel.Text = cnt.ToString();
                    child.Show(); 

                }
                else
                {
                    RadMessageBox.Show(this,"Не заполнен Раздел 6.4 для текущего Страхователя за указанный период и номер корректировки", "Ошибка");
                }
            }
            else
            {
                RadMessageBox.Show(this, "Не найден Раздел 6 для текущего Страхователя за указанный период и номер корректировки", "Ошибка");
            }

        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.child.Close();
            this.Close();
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            child.radProgressBar1.Value1 = e.ProgressPercentage;
            child.firstPartLabel.Text = e.UserState.ToString();
        }

        private void calculation_2_1(int i)
        {
            RSW_2_1_List[i].s_200_0 = RSW_2_1_List[i].s_200_1 + RSW_2_1_List[i].s_200_2 + RSW_2_1_List[i].s_200_3;
            RSW_2_1_List[i].s_201_0 = RSW_2_1_List[i].s_201_1 + RSW_2_1_List[i].s_201_2 + RSW_2_1_List[i].s_201_3;
            RSW_2_1_List[i].s_202_0 = RSW_2_1_List[i].s_202_1 + RSW_2_1_List[i].s_202_2 + RSW_2_1_List[i].s_202_3;
            RSW_2_1_List[i].s_203_0 = RSW_2_1_List[i].s_203_1 + RSW_2_1_List[i].s_203_2 + RSW_2_1_List[i].s_203_3;
        }

        private void calculation(object sender, DoWorkEventArgs e)
        {
            List<FormsRSW2014_1_Razd_6_4> Razd_6_4_list = db.FormsRSW2014_1_Razd_6_4.Where(x => RSW_6_1_IDs.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).OrderBy(x => x.PlatCategoryID).ToList();

            long TariffCodeID = 0;
            long platCatID = 0;

            int i = 0;
            int k = 0;
            long id = 0;
//            bool first = true;

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

                FormsRSW2014_1_Razd_2_1 rsw21_ = new FormsRSW2014_1_Razd_2_1 { };
                RSW_2_1_List.Add(rsw21_.SetZeroValues());
            }


            foreach (var item in Razd_6_4_list)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    RSW_2_1_List.Clear();
                    return;
                }
                
                k++;

//                if (TariffCodeID != item.PlatCategory.TariffCodePlatCat.First().TariffCodeID)

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

                    RSW_2_1_List[i].TariffCodeID = TariffCodeID;
                    id = RSW_2_1_List[i].TariffCodeID.Value;
                    RSW_2_1_List[i].TariffCode = db.TariffCode.FirstOrDefault(x => x.ID == id);
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

                    RSW_2_1_List[i].TariffCodeID = tariffCode.FirstOrDefault(x => x.PlatCategoryID == item.PlatCategoryID).TariffCodeID;
                    id = RSW_2_1_List[i].TariffCodeID.Value;
                    RSW_2_1_List[i].TariffCode = db.TariffCode.FirstOrDefault(x => x.ID == id);
                }

                RSW_2_1_List[i].TariffCode = db.TariffCode.FirstOrDefault(x => x.ID == id);


                RSW_2_1_List[i].InsurerID = identifier.InsurerID;
                RSW_2_1_List[i].CorrectionNum = identifier.CorrectionNum;
                RSW_2_1_List[i].Quarter = identifier.Quarter;
                RSW_2_1_List[i].Year = identifier.Year;


                RSW_2_1_List[i].AutoCalc = false;

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


       //         first = false;


                decimal temp = (decimal)k / (decimal)Razd_6_4_list.Count();
                int proc = (int)Math.Round((temp * 100), 0);
                backgroundWorker1.ReportProgress(proc, k.ToString());
            }


            //Суммы превышения 204 строка
            for (int n = 0; n < RSW_2_1_List.Count(); n++)
            {
                calculation_2_1(n);
                RSW_2_1_List[n].s_204_0 = RSW_2_1_List[n].s_200_0 - RSW_2_1_List[n].s_201_0 - RSW_2_1_List[n].s_202_0 - RSW_2_1_List[n].s_203_0;
                RSW_2_1_List[n].s_204_1 = RSW_2_1_List[n].s_200_1 - RSW_2_1_List[n].s_201_1 - RSW_2_1_List[n].s_202_1 - RSW_2_1_List[n].s_203_1;
                RSW_2_1_List[n].s_204_2 = RSW_2_1_List[n].s_200_2 - RSW_2_1_List[n].s_201_2 - RSW_2_1_List[n].s_202_2 - RSW_2_1_List[n].s_203_2;
                RSW_2_1_List[n].s_204_3 = RSW_2_1_List[n].s_200_3 - RSW_2_1_List[n].s_201_3 - RSW_2_1_List[n].s_202_3 - RSW_2_1_List[n].s_203_3;
            }


            foreach (var item in RSW_2_1_List)
            {
                //Расчет суммы взносов с начала года накопительно
                FormsRSW2014_1_Razd_2_1 formDataPrev = new FormsRSW2014_1_Razd_2_1();
                id = item.TariffCodeID.Value;

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

                        if (db.FormsRSW2014_1_Razd_2_1.Any(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID && x.TariffCodeID == id))
                        {
                            formDataPrev = db.FormsRSW2014_1_Razd_2_1.Where(x => x.Year == y && x.Quarter == quarter && x.InsurerID == Options.InsID && x.TariffCodeID == id).OrderByDescending(x => x.CorrectionNum).First();

                            item.s_200_0 = item.s_200_0 + formDataPrev.s_200_0;
                            item.s_201_0 = item.s_201_0 + formDataPrev.s_201_0;
                            item.s_203_0 = item.s_203_0 + formDataPrev.s_203_0;
                            item.s_204_0 = item.s_204_0 + formDataPrev.s_204_0;
//                            item.s_205_0 = item.s_205_0 + formDataPrev.s_205_0;
//                            item.s_206_0 = item.s_206_0 + formDataPrev.s_206_0;
         
                        }

                    }
                    else
                    {
                        formDataPrev = null;
                    }
                }
                else
                {
                    formDataPrev = null;
                }

                
                TariffCode tarCode = db.TariffCode.FirstOrDefault(x => x.ID == item.TariffCodeID.Value);
                decimal tariffStrah = 0;
                decimal tariffOMS = 0;

                if (tarCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.Any(x => x.Year.Value == y))
                {
                    tariffStrah = tarCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.FirstOrDefault(x => x.Year.Value == y).StrahPercant1966.Value;
                    tariffOMS = tarCode.TariffCodePlatCat.First().PlatCategory.TariffPlat.FirstOrDefault(x => x.Year.Value == y).FFOMS_Percent.Value;
                }
                decimal tariffMore = tariffStrah == 0 ? 0 : ((tarCode.Code == "01" || tarCode.Code == "52" || tarCode.Code == "53") ? 10 : 0);

                //Расчет взносов 

                item.s_205_0 = item.s_204_0.Value * tariffStrah / 100;
                item.s_205_1 = ((item.s_204_0.Value - item.s_204_2.Value - item.s_204_3.Value) * tariffStrah / 100) - ((item.s_204_0.Value - item.s_204_1.Value - item.s_204_2.Value - item.s_204_3.Value) * tariffStrah / 100);
                item.s_205_2 = (((item.s_204_0.Value - item.s_204_3.Value) * tariffStrah / 100) - ((item.s_204_0.Value - item.s_204_1.Value - item.s_204_2.Value - item.s_204_3.Value) * tariffStrah / 100)) - item.s_205_1.Value;
                item.s_205_3 = (((item.s_204_0.Value) * tariffStrah / 100) - ((item.s_204_0.Value - item.s_204_1.Value - item.s_204_2.Value - item.s_204_3.Value) * tariffStrah / 100)) - (item.s_205_1.Value + item.s_205_2.Value);

                item.s_206_0 = item.s_203_0.Value * tariffMore / 100;
                item.s_206_1 = ((item.s_203_0.Value - item.s_203_2.Value - item.s_203_3.Value) * tariffMore / 100) - ((item.s_203_0.Value - item.s_203_1.Value - item.s_203_2.Value - item.s_203_3.Value) * tariffMore / 100);
                item.s_206_2 = (((item.s_203_0.Value - item.s_203_3.Value) * tariffMore / 100) - ((item.s_203_0.Value - item.s_203_1.Value - item.s_203_2.Value - item.s_203_3.Value) * tariffMore / 100)) - item.s_206_1.Value;
                item.s_206_3 = (((item.s_203_0.Value) * tariffMore / 100) - ((item.s_203_0.Value - item.s_203_1.Value - item.s_203_2.Value - item.s_203_3.Value) * tariffMore / 100)) - (item.s_206_1.Value + item.s_206_2.Value);



                // Если последняя запись с текущей категорий плательщика
                //Сумма по разделу 6.5 по конретной категории
                List<long> PlatCategoryID = new List<long>();

                if (platCatTariffCode.Any(x => x.TariffCodeID == item.TariffCodeID))
                {
                    PlatCategoryID = platCatTariffCode.Where(x => x.TariffCodeID == item.TariffCodeID).Select(x => x.PlatCategoryID).ToList();
                }
                else
                {
                    PlatCategoryID = db.TariffCodePlatCat.Where(x => x.TariffCodeID == item.TariffCodeID).Select(x => x.PlatCategoryID.Value).ToList();
                }

//                decimal strah_temp = UpdateSumFeePFR(Razd_6_4_list.Where(x => PlatCategoryID.Contains(x.PlatCategoryID)).ToList(), identifier.Year);
                decimal strah_temp = Razd_6_4_list.Where(x => PlatCategoryID.Contains(x.PlatCategoryID)).Sum(x => x.FormsRSW2014_1_Razd_6_1.SumFeePFR.Value);

                decimal delta = strah_temp - item.s_205_1.Value - item.s_205_2.Value - item.s_205_3.Value;

                item.AutoCalc = true;

                // Процедура подгонки сумм
                if (delta != 0)
                {
                    if (item.s_205_3.Value != 0)
                    {
                        item.s_205_3 = item.s_205_3.Value + delta;
                        if (item.s_205_3 < 0)
                        {
                            delta = item.s_205_3.Value;
                            item.s_205_3 = 0;
                        }
                        else
                        {
                            delta = 0;
                        }
                    }

                    if (item.s_205_2.Value != 0)
                    {
                        item.s_205_2 = item.s_205_2.Value + delta;
                        if (item.s_205_2 < 0)
                        {
                            delta = item.s_205_2.Value;
                            item.s_205_2 = 0;
                        }
                        else
                        {
                            delta = 0;
                        }
                    }

                    if (item.s_205_1.Value != 0)
                    {
                        item.s_205_1 = item.s_205_1.Value + delta;
                        if (item.s_205_1 < 0)
                        {
                            delta = item.s_205_1.Value;
                            item.s_205_1 = 0;
                        }
                        else
                        {
                            delta = 0;
                        }
                    }


                    item.s_205_0 = item.s_205_1.Value + item.s_205_2.Value + item.s_205_3.Value;

                }

                //Начисленные взносы 210 строка
                item.s_210_0 = item.s_200_0;
                item.s_210_1 = item.s_200_1;
                item.s_210_2 = item.s_200_2;
                item.s_210_3 = item.s_200_3;

                //Суммы не подлежащие обложению 211 строка
                item.s_211_0 = item.s_201_0;
                item.s_211_1 = item.s_201_1;
                item.s_211_2 = item.s_201_2;
                item.s_211_3 = item.s_201_3;



                if (yearType != 2015)
                {
                    //Суммы превышения 213 строка
                    item.s_213_0 = item.s_203_0;
                    item.s_213_1 = item.s_203_1;
                    item.s_213_2 = item.s_203_2;
                    item.s_213_3 = item.s_203_3;

                    //Суммы превышения 214 строка
                    item.s_214_0 = item.s_204_0;
                    item.s_214_1 = item.s_204_1;
                    item.s_214_2 = item.s_204_2;
                    item.s_214_3 = item.s_204_3;

                    item.s_215_0 = item.s_214_0 * tariffOMS / 100;
                    item.s_215_1 = ((item.s_214_0 - item.s_214_2 - item.s_214_3) * tariffOMS / 100) - ((item.s_214_0 - item.s_214_1 - item.s_214_2 - item.s_214_3) * tariffOMS / 100);
                    item.s_215_2 = (((item.s_214_0 - item.s_214_3) * tariffOMS / 100) - ((item.s_214_0 - item.s_214_1 - item.s_214_2 - item.s_214_3) * tariffOMS / 100)) - item.s_215_1;
                    item.s_215_3 = (((item.s_214_0) * tariffOMS / 100) - ((item.s_214_0 - item.s_214_1 - item.s_214_2 - item.s_214_3) * tariffOMS / 100)) - (item.s_215_1 + item.s_215_2);
                }
                else
                {
                    //Суммы превышения 214 строка
                    item.s_213_0 = item.s_210_0 - item.s_211_0;
                    item.s_213_1 = item.s_210_1 - item.s_211_1;
                    item.s_213_2 = item.s_210_2 - item.s_211_2;
                    item.s_213_3 = item.s_210_3 - item.s_211_3;

                    item.s_214_0 = item.s_213_0 * tariffOMS / 100;
                    item.s_214_1 = ((item.s_213_0 - item.s_213_2 - item.s_213_3) * tariffOMS / 100) - ((item.s_213_0 - item.s_213_1 - item.s_213_2 - item.s_213_3) * tariffOMS / 100);
                    item.s_214_2 = (((item.s_213_0 - item.s_213_3) * tariffOMS / 100) - ((item.s_213_0 - item.s_213_1 - item.s_213_2 - item.s_213_3) * tariffOMS / 100)) - item.s_214_1;
                    item.s_214_3 = (((item.s_213_0) * tariffOMS / 100) - ((item.s_213_0 - item.s_213_1 - item.s_213_2 - item.s_213_3) * tariffOMS / 100)) - (item.s_214_1 + item.s_214_2);

                }

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
                decimal tar = 0;
                foreach (var item in RSW_6_4_List)
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

                        sum = sum + sumAll;

                        tar = item.PlatCategory.TariffPlat.First(x => x.Year == y).StrahPercant1966.Value;
                    }
                    else
                    {
                    }
                }
//                sum = sum + (sumAll * item.PlatCategory.TariffPlat.First(x => x.Year == y).StrahPercant1966.Value / 100);
                sum = sum * tar / 100;

            }
            else
            {
                sum = 0;
            }

            return (sum);

        }

        private void RSW2014_2_1_Filling_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

        }

/*
        private void setZeroValues(int i)
        { 
            var fields = typeof(FormsRSW2014_1_Razd_2_1).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    string type = RSW_2_1_List[i].GetType().GetProperty(itemName).PropertyType.FullName;
                    type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                    type = type.Split(',')[0].Split('.')[1].ToLower();
                    switch (type)
                    {
                        case "decimal":
                            RSW_2_1_List[i].GetType().GetProperty(itemName).SetValue(RSW_2_1_List[i], (decimal)0, null);
                            break;
                        case "integer":
                            RSW_2_1_List[i].GetType().GetProperty(itemName).SetValue(RSW_2_1_List[i], (int)0, null);
                            break;
                        case "int64":
                            RSW_2_1_List[i].GetType().GetProperty(itemName).SetValue(RSW_2_1_List[i], (long)0, null);
                            break;
                    }
                }
            }
        }
*/
    }
}
