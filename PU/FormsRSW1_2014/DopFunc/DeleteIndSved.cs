using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using PU.Models;
using PU.FormsRSW2014;
using PU.Classes;
using Telerik.WinControls.UI;

namespace PU.FormsRSW2014
{
    public partial class DeleteIndSved : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<long> staffList = new List<long>();
        public PlatCategory PlatCat { get; set; }
        private List<RaschetPeriodContainer> RaschPer { get; set; }
        List<FormsRSW2014_1_Razd_6_1> rsw61List;
        RSW2014_6_Copy_Wait child = new RSW2014_6_Copy_Wait();
        public BackgroundWorker bw = new BackgroundWorker();
        List<ErrList> errList = new List<ErrList>();

        public DeleteIndSved()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);

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

        private void deleteIndSved_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bw != null && bw.IsBusy)
            {
                bw.CancelAsync();
            }
            if (bw != null)
            {
                bw.Dispose();
            }
        }

        private void deleteIndSved_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            foreach (var item in db.TypeInfo)
            {
                TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }

            TypeInfo.SelectedIndex = 0;
            this.TypeInfo.SelectedIndexChanged += (s, с) => TypeInfo_SelectedIndexChanged();

            // выпад список "календарный год"
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);

            foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }

            if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                Year.Items.OrderByDescending(x => x.Value).First().Selected = true;


            Year_SelectedIndexChanged();

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();


            // выпад список "календарный год"
            RaschPer = new List<RaschetPeriodContainer> { };
            foreach (var item in Options.RaschetPeriodInternal2010_2013)
            {
                RaschPer.Add(item);
            }
            foreach (var item in Options.RaschetPeriodInternal)
            {
                RaschPer.Add(item);
            }

            this.YearKorr.Items.Clear();

            foreach (var item in RaschPer.Select(x => x.Year).Distinct().ToList())
            {
                YearKorr.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }

            if (YearKorr.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                YearKorr.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                YearKorr.Items.Last().Selected = true;

            this.YearKorr.SelectedIndexChanged += (s, с) => YearKorr_SelectedIndexChanged();
            YearKorr_SelectedIndexChanged();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startBtn.Enabled = true;
            closeBtn.Enabled = true;

            this.child.Close();

            if (errList.Count == 0)  // если ошибок нет
            {
                Methods.showAlert("Внимание", "Удаление данных по заданным параметрам произведено успешно!", this.ThemeName);
            }
            else   //  Если ошибки есть , то выводим и все в алерт
            {
                string errText = "";

                int i = 0;
                foreach (var item in errList)
                {
                    i++;
                    errText = errText + (i >= 2 ? "\r\n\r\n" : "") + item.name + "   Сообщение об ошибке: " + item.type;
                }

                Methods.showAlert("Ошибка", errText, this.ThemeName, 110 * errList.Count);
            }



        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            child.radProgressBar1.Value1 = e.ProgressPercentage;
            child.firstPartLabel.Text = e.UserState.ToString();
        }

        private void TypeInfo_SelectedIndexChanged()
        {
            if (TypeInfo.SelectedIndex == 0)
            {
                korrGroupBox.Enabled = false;
            }
            else
            {
                korrGroupBox.Enabled = true;
            }
        }

        private void Year_SelectedIndexChanged()
        {
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014);

            // выпад список "Отчетный период"

            this.Quarter.Items.Clear();

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                DateTime dt = DateTime.Now.Date;

                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                else
                    rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                if (rp != null)
                    Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                else
                    Quarter.Items.OrderByDescending(x => x.Value).First().Selected = true;
            }
        }

        private void YearKorr_SelectedIndexChanged()
        {
            // выпад список "Отчетный период"
            this.QuarterKorr.Items.Clear();
            staffList = new List<long>();
            short y;
            if (short.TryParse(YearKorr.SelectedItem.Text, out y))
            {
                foreach (var item in RaschPer.Where(x => x.Year == y).ToList())
                {
                    QuarterKorr.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                DateTime dt = DateTime.Now.Date;

                RaschetPeriodContainer rp = new RaschetPeriodContainer();

                if (RaschPer.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                    rp = RaschPer.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                else
                    rp = RaschPer.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                if (rp != null)
                    QuarterKorr.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                else
                    QuarterKorr.Items.OrderByDescending(x => x.Value).First().Selected = true;

            }
        }

        private void clearPlatCatBtn_Click(object sender, EventArgs e)
        {
            PlatCat = null;
            platCatPanel.Text = "По всем Категориям плательщика... Или нажмите для выбора";
        }

        private void platCatPanel_MouseHover(object sender, EventArgs e)
        {
            platCatPanel.Font = new Font(platCatPanel.Font, platCatPanel.Font.Style | FontStyle.Underline);
        }

        private void platCatPanel_MouseLeave(object sender, EventArgs e)
        {
            platCatPanel.Font = new Font(platCatPanel.Font, FontStyle.Bold);
        }

        private void platCatPanel_MouseClick(object sender, MouseEventArgs e)
        {
            PlatCategoryFrm child = new PlatCategoryFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.PlatCat = PlatCat;
            child.action = "selection";
            child.ShowDialog();
            PlatCat = child.PlatCat;

            if (PlatCat != null)
            {
                platCatPanel.Text = PlatCat.Code + "   " + PlatCat.Name;
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            RSW2014_List main = this.Owner as RSW2014_List;
            errList = new List<ErrList>();
            rsw61List = new List<FormsRSW2014_1_Razd_6_1>();
            staffList = new List<long>();

            if (main != null)
            {
                if (main.staffGridView.RowCount > 0)
                {
                    switch (copyRangeDDL.SelectedIndex)
                    {
                        case 0:
                            staffList.Add(Convert.ToInt64(main.staffGridView.CurrentRow.Cells[1].Value));
                            break;
                        case 1:
                            int rowindex = main.staffGridView.CurrentRow.Index;
                            if (main.staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                            {
                                foreach (var item in main.staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                {
                                    staffList.Add(long.Parse(item.Cells[1].Value.ToString()));
                                }
                            }
                            else
                            {
                                staffList.Add(long.Parse(main.staffGridView.Rows[rowindex].Cells[1].Value.ToString()));
                            }
                            break;
                        case 2:
                            foreach (var row in main.staffGridView.Rows)
                            {
                                staffList.Add(Convert.ToInt64(row.Cells[1].Value));
                            }
                            break;
                    }
                }
                if (!allIndSvedRadBtn.IsChecked) // если удаляем не за все периоды
                {
                    byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
                    short y = short.Parse(Year.Text);

                    if (TypeInfo.SelectedIndex == 0) // исходные данные
                    {
                        rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == (TypeInfo.SelectedIndex + 1)).ToList();
                    }
                    else // корректирующие и отменяющие
                    {
                        byte qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
                        short yk = short.Parse(YearKorr.Text);
                        rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID == (TypeInfo.SelectedIndex + 1)).ToList();
                    }

                    //if (PlatCat != null)
                    //{
                    //    rsw61List = rsw61List.Where(x => x.FormsRSW2014_1_Razd_6_4.Any(z => z.PlatCategoryID == PlatCat.ID)).ToList();
                    //}
                }
                else // Если выбрано удалить Инд.сведения за все отчетные периоды
                {
                    rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID)).ToList();
                }


                if (rsw61List.Count() > 0)
                {

                    string title = "Обработка данных";
                    bw.DoWork += new System.ComponentModel.DoWorkEventHandler(calculation);
                    bw.RunWorkerAsync();
                    startBtn.Enabled = false;
                    closeBtn.Enabled = false;



                    child = new RSW2014_6_Copy_Wait();
                    child.Owner = this;
                    child.ownerName = this.Name;
                    child.ThemeName = this.ThemeName;
                    child.titleLabel.Text = title;
                    child.secondPartLabel.Text = rsw61List.Count().ToString();
                    child.Show();

                    main.updateBtn_Click(null, null);


                }
                else
                {
                    RadMessageBox.Show(this, "Нет данных для обработки. Не удалось получить список сотрудников удовлетворяющих условиям!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }
        }

        private void calculation(object sender, DoWorkEventArgs e)
        {
            string list = String.Join(",", rsw61List.Select(x => x.ID).ToArray());

            if (allIndSvedRadBtn.IsChecked) //Удалить Инд.сведения за все отчетные периоды
            {
                try
                {
                    db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_1 WHERE ([ID] IN ({0}))", list));
                }
                catch (Exception ex)
                {
                    errList.Add(new ErrList { name = "При удалении Индивидуальных сведений за все отчетные периоды произошла ошибка!", type = ex.Message});
                }
            }
            else if (regNumKorrRadBtn.IsChecked) //Очистить реквизит - [Рег. № в ПФР в корректируемый период]
            {
                try
                {
                    db.ExecuteStoreCommand(String.Format("UPDATE FormsRSW2014_1_Razd_6_1 SET RegNumKorr = \"\" WHERE (([ID] IN ({0})) AND (RegNumKorr <> \"\"))", list));
                }
                catch (Exception ex)
                {
                    errList.Add(new ErrList { name = "В процессе очистки реквизита  - [Рег. № в ПФР в корректируемый период] произошла ошибка!", type = ex.Message });
                }
            }
            else
            {
                calcList();
            }

        }

        private void calcList()
        {
            int k = 0;

            if (emptyIndSvedRadBtn.IsChecked) //Удалить "пустые" Индивидуальные  сведения
            {
                rsw61List = rsw61List.Where(x => x.SumFeePFR == 0 && !x.FormsRSW2014_1_Razd_6_4.Any() && !x.FormsRSW2014_1_Razd_6_7.Any() && !x.StajOsn.Any()).ToList();

                foreach (var item in rsw61List)
                {
                    if (bw.CancellationPending)
                    {
                        rsw61List.Clear();
                        return;
                    }

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_1 WHERE ([ID] = {0})", item.ID));
                    }
                    catch (Exception ex)
                    {
                        errList.Add(new ErrList { name = "При удалении \"пустых\" Индивидуальных сведений произошла ошибка!", type = ex.Message });
                    }

                    k++;

                    decimal temp = (decimal)k / (decimal)rsw61List.Count();
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, k.ToString());


                }


            }


            if (delIndSvedRadBtn.IsChecked) //Удалить инд.сведения сотрудников
            {
                string list = String.Join(",", rsw61List.Select(x => x.ID).ToArray());

                if (!osnVyplChkBox.Checked && !sumFeeChkBox.Checked && !dopTariffChkBox.Checked && !korrSvedChkBox.Checked && !stajChkBox.Checked)   // если удаляем полностью инд.свед. и не выбран не один чекбокс
                {
                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_1 WHERE ([ID] IN ({0}))", list));
                    }
                    catch (Exception ex)
                    {
                        errList.Add(new ErrList { name = "При удалении Индивидуальных сведений сотрудников произошла ошибка!", type = ex.Message });
                    }
                }
                else // если надо удалить по отдельности
                {
                    if (osnVyplChkBox.Checked)
                    {
                        try
                        {
                            if (PlatCat != null)
                            {
                                db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_4 WHERE (([FormsRSW2014_1_Razd_6_1_ID] IN ({0})) AND (PlatCategoryID = {1}))", list, PlatCat.ID));
                            }
                            else
                            {
                                db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_4 WHERE ([FormsRSW2014_1_Razd_6_1_ID] IN ({0}))", list));
                            }
                        }
                        catch (Exception ex)
                        {
                            errList.Add(new ErrList { name = "При удалении Основных выплат произошла ошибка!", type = ex.Message });
                        }
                    }
                    if (sumFeeChkBox.Checked)
                    {
                        try
                        {
                            db.ExecuteStoreCommand(String.Format("UPDATE FormsRSW2014_1_Razd_6_1 SET SumFeePFR = 0 WHERE ([ID] IN ({0}))", list));
                        }
                        catch (Exception ex)
                        {
                            errList.Add(new ErrList { name = "При удалении Начисленных взносов произошла ошибка!", type = ex.Message });
                        }

                    }
                    if (dopTariffChkBox.Checked)
                    {
                        try
                        {
                            db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_7 WHERE ([FormsRSW2014_1_Razd_6_1_ID] IN ({0}))", list));
                        }
                        catch (Exception ex)
                        {
                            errList.Add(new ErrList { name = "При удалении выплат по Доп.тарифам произошла ошибка!", type = ex.Message });
                        }
                    }
                    if (korrSvedChkBox.Checked)
                    {
                        try
                        {
                            db.ExecuteStoreCommand(String.Format("DELETE FROM FormsRSW2014_1_Razd_6_6 WHERE ([FormsRSW2014_1_Razd_6_1_ID] IN ({0}))", list));
                        }
                        catch (Exception ex)
                        {
                            errList.Add(new ErrList { name = "При удалении Корректирующих сведений произошла ошибка!", type = ex.Message });
                        }
                    }
                    if (stajChkBox.Checked)
                    {
                        try
                        {
                            db.ExecuteStoreCommand(String.Format("DELETE FROM StajOsn WHERE ([FormsRSW2014_1_Razd_6_1_ID] IN ({0}))", list));
                        }
                        catch (Exception ex)
                        {
                            errList.Add(new ErrList { name = "При удалении записей о Стаже произошла ошибка!", type = ex.Message });
                        }

                    }

                }

                foreach (var item in rsw61List)
                {
                    if (bw.CancellationPending)
                    {
                        rsw61List.Clear();
                        return;
                    }




                    k++;

                    decimal temp = (decimal)k / (decimal)rsw61List.Count();
                    int proc = (int)Math.Round((temp * 100), 0);
                    bw.ReportProgress(proc, k.ToString());
                }
            }


        }

        private void delIndSvedRadBtn_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            //if (!delIndSvedRadBtn.IsChecked)
            //{
            //    osnVyplChkBox.Checked = false;
            //    sumFeeChkBox.Checked = false;
            //    dopTariffChkBox.Checked = false;
            //    korrSvedChkBox.Checked = false;
            //    stajChkBox.Checked = false;
            //}

            osnVyplChkBox.Enabled = delIndSvedRadBtn.IsChecked;
            sumFeeChkBox.Enabled = delIndSvedRadBtn.IsChecked;
            dopTariffChkBox.Enabled = delIndSvedRadBtn.IsChecked;
            korrSvedChkBox.Enabled = delIndSvedRadBtn.IsChecked;
            stajChkBox.Enabled = delIndSvedRadBtn.IsChecked;


        }

        private void osnVyplChkBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (!osnVyplChkBox.Checked)
            {
                clearPlatCatBtn_Click(null, null);
            }

            platCatPanel.Enabled = osnVyplChkBox.Checked;
            clearPlatCatBtn.Enabled = osnVyplChkBox.Checked;

        }

    }
}
