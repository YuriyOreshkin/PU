using PU.Classes;
using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU
{
    public partial class UpdateDict : Telerik.WinControls.UI.RadForm
    {
        private int cntGood = 0;

        public UpdateDict()
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

        private void UpdateDict_Load(object sender, EventArgs e)
        {
            Dictionary<string, string> tableList = new Dictionary<string, string> { 
                {"Категории плательщиков","PlatCategory"},
                {"Категории плательщиков: Тарифы","TariffPlat"},
                {"МРОТ","MROT"},
                {"Доп. тарифы для отдельных категорий граждан","DopTariff"},
                {"Территориальные условия","TerrUsl"},
                {"Особые условия труда","OsobUslTruda"},
                {"Классификатор вредных профессий 1","KodVred_1"},
                {"Классификатор вредных профессий 2","KodVred_2"},
                {"Классификатор вредных профессий 3","KodVred_3"},
                {"Исчисление страхового стажа: Основание","IschislStrahStajOsn"},
                {"Исчисление страхового стажа: Доп. сведения","IschislStrahStajDop"},
                {"Условия для досрочного назначения трудовой пенсии","UslDosrNazn"},
                {"Виды трудовой или иной общественно полезной деятельности","VidTrudDeyat"},
                {"Специальная оценка условий труда","SpecOcenkaUslTruda"},
                {"Специальная оценка условий труда: доп. тарифы","SpecOcenkaUslTrudaDopTariff"},
                {"Типы документов","DocumentTypes"},
                {"Коды тарифов для страховых взносов","TariffCode"},
                {"Тарифы взносов на дополнительное социальное обеспечение Форма РВ-3","CodeBaseRW3_2015"},
                {"Виды сведений Формы СЗВ-ТД","FormsSZV_TD_2020_TypesOfEvents"}
            };

            foreach (var item in tableList)
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.tablesGridView.MasterView);
                rowInfo.Cells["tableDescription"].Value = item.Key;
                rowInfo.Cells["tableName"].Value = item.Value;

                tablesGridView.Rows.Add(rowInfo);
            }

            this.tablesGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            tablesGridView.Rows[0].IsCurrent = true;
            tablesGridView.TableElement.ScrollToRow(tablesGridView.Rows[0]);

        }

        private bool checkBase_emp(string path)
        {
            bool result = File.Exists(path);
            
            return result;
        }

        private void synchBtn_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");
            if (checkBase_emp(path))
            {
                cntGood = 0;
                radWaitingBar1.Visible = true;
                radWaitingBar1.StartWaiting();

                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(synchronization);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                synchBtn.Enabled = false;
                closeBtn.Enabled = false;

                foreach (var item in tablesGridView.Rows)
                {
                    item.Cells[0].Value = null;
                }
                tablesGridView.Rows[0].IsCurrent = true;
                tablesGridView.TableElement.ScrollToRow(tablesGridView.Rows[0]);


                bw.RunWorkerAsync();

            }
            else
            {
                RadMessageBox.Show(this, "Не найдена эталонная база данных! Синхронизация остановлена!", "Ошибка синхронизации");
            }
        }

        private void synchronization(object sender, DoWorkEventArgs e)
        {
            string path = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");

            foreach (var item in tablesGridView.Rows)
            {
                tablesGridView.Invoke(new Action(() => { tablesGridView.Rows[item.Index].IsCurrent = true; tablesGridView.Rows[item.Index].Cells[0].Value = Color.DarkOrange; }));

                if (UpdateDictionaries.updateTable(item.Cells[2].Value.ToString(), path, this.ThemeName, this))
                {
                    tablesGridView.Invoke(new Action(() => { tablesGridView.Rows[item.Index].Cells[0].Value = Color.LimeGreen; }));
                    cntGood++;
                }
                else
                {
                    tablesGridView.Invoke(new Action(() => { tablesGridView.Rows[item.Index].Cells[0].Value = Color.Red; }));
                }
            }

        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            synchBtn.Enabled = true;
            closeBtn.Enabled = true;


            radWaitingBar1.StopWaiting();
            radWaitingBar1.Visible = false;

            Messenger.showAlert(AlertType.Success, "Синхронизация завершена", "Успешно синхронизировано " + cntGood.ToString() + " из " + tablesGridView.RowCount.ToString() + " справочников.", this.ThemeName);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
