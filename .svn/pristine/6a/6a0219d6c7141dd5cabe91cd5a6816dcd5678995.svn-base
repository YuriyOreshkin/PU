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
    public partial class ProsmotrVyplat : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public PlatCategory PlatCat { get; set; }
        public List<long> staffList = new List<long>();
        private List<RaschetPeriodContainer> RaschPer { get; set; }
        private bool abort;
        List<FormsRSW2014_1_Razd_6_1> rsw61List;
        RSW2014_6_Copy_Wait child = new RSW2014_6_Copy_Wait();
        public BackgroundWorker bw = new BackgroundWorker();

        public ProsmotrVyplat()
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

        private void ProsmotrVyplat_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bw != null && bw.IsBusy)
            {
                bw.CancelAsync();
            }
            if (bw != null)
            {
                bw.Dispose();
            }


            Props props = new Props(); //экземпляр класса с настройками

            List<WindowData> windowData = new List<WindowData> { };

            windowData.Add(new WindowData
            {
                control = "copyRangeDDL",
                value = copyRangeDDL.SelectedItem.Tag.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "TypeInfo",
                value = TypeInfo.SelectedItem.Value.ToString()
            });
            if (!String.IsNullOrEmpty(Year.Text))
            {
                windowData.Add(new WindowData
                {
                    control = "Year",
                    value = Year.Text
                });
            }
            if (Quarter.SelectedItem != null)
            {
                windowData.Add(new WindowData
                {
                    control = "Quarter",
                    value = Quarter.SelectedItem.Value.ToString()
                });
            }
            if (!String.IsNullOrEmpty(YearKorr.Text))
            {
                windowData.Add(new WindowData
                {
                    control = "YearKorr",
                    value = YearKorr.Text
                });
            }
            if (QuarterKorr.SelectedItem != null)
            {
                windowData.Add(new WindowData
                {
                    control = "QuarterKorr",
                    value = QuarterKorr.SelectedItem.Value.ToString()
                });
            }
            windowData.Add(new WindowData
            {
                control = "includeAllStaffCheckBox",
                value = includeAllStaffCheckBox.Checked ? "true" : "false"
            });

            props.setFormParams(this, windowData);
        }

        private void ProsmotrVyplat_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            foreach (var item in db.TypeInfo)
            {
                TypeInfo.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }

            TypeInfo.SelectedIndex = 0;

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


            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    foreach (var item in param.windowData)
                    {
                        switch (item.control)
                        {
                            case "copyRangeDDL":
                                if (copyRangeDDL.Items.Any(x => x.Tag.ToString() == item.value))
                                {
                                    copyRangeDDL.Items.Single(x => x.Tag.ToString() == item.value).Selected = true;
                                }
                                break;
                            case "TypeInfo":
                                if (TypeInfo.Items.Any(x => x.Value.ToString() == item.value))
                                {
                                    TypeInfo.Items.Single(x => x.Value.ToString() == item.value).Selected = true;
                                    TypeInfo_SelectedIndexChanged();
                                }
                                break;
                            case "Year":
                                if (Year.Items.Any(x => x.Text.ToString() == item.value))
                                {
                                    Year.Items.Single(x => x.Text.ToString() == item.value).Selected = true;
                                    Year_SelectedIndexChanged();
                                }
                                break;
                            case "Quarter":
                                if (Quarter.Items.Any(x => x.Value.ToString() == item.value))
                                {
                                    Quarter.Items.Single(x => x.Value.ToString() == item.value).Selected = true;
                                }
                                break;
                            case "YearKorr":
                                if (YearKorr.Items.Any(x => x.Text.ToString() == item.value))
                                {
                                    YearKorr.Items.Single(x => x.Text.ToString() == item.value).Selected = true;
                                    YearKorr_SelectedIndexChanged();
                                }
                                break;
                            case "QuarterKorr":
                                if (QuarterKorr.Items.Any(x => x.Value.ToString() == item.value))
                                {
                                    QuarterKorr.Items.Single(x => x.Value.ToString() == item.value).Selected = true;
                                }
                                break;
                            case "includeAllStaffCheckBox":
                                includeAllStaffCheckBox.Checked = item.value == "true" ? true : false;
                                break;
                        }
                    }

                }
                catch
                { }
            }

            switch (action)
            {
                case "svedNachVznos":
                    platCatPanel.Enabled = false;
                    clearPlatCatBtn.Enabled = false;
                    this.Text = "Сведения о начисленных взносах в ПФР";
                    break;
                case "svedBaseOPS":
                    platCatPanel.Enabled = true;
                    clearPlatCatBtn.Enabled = true;
                    this.Text = "Сведения по базе для начисления страховых взносов на ОПС";
                    break;
                case "svedVypl":
                    platCatPanel.Enabled = true;
                    clearPlatCatBtn.Enabled = true;
                    this.Text = "Сведения о суммах выплат";
                    break;
                case "svedDop":
                    platCatPanel.Enabled = false;
                    clearPlatCatBtn.Enabled = false;
                    this.Text = "Сведения о выплатах по доп.тарифам";
                    break;
                case "svedKorr":
                    TypeInfo.SelectedIndex = 0;
                    TypeInfo.Enabled = false;
                    platCatPanel.Enabled = false;
                    clearPlatCatBtn.Enabled = false;
                    this.Text = "Информация о корректирующих сведениях";
                    break;
            }

            this.TypeInfo.SelectedIndexChanged += (s, с) => TypeInfo_SelectedIndexChanged();

        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startBtn.Enabled = true;
            closeBtn.Enabled = true;

            this.child.Close();
            if (!abort)
            {


                string reg = "";
                if (Options.InsID != 0 && db.Insurer.Any(x => x.ID == Options.InsID))
                {
                    Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

                    reg = Utils.ParseRegNum(ins.RegNum);
                }

                string period = (TypeInfo.SelectedIndex == 0 ? Quarter.Text : QuarterKorr.Text);
                string title = "   -   " + period + " [" + TypeInfo.Text + "]    Рег.№: " + reg;

                title = this.Text + title;

                NachVznosy child = new NachVznosy();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.rsw61List = rsw61List;
                child.action = action;
                if (PlatCat != null)
                    child.PlatCat = PlatCat;
                child.Text = title;
                child.Period = period;
                child.ShowDialog();
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

        private void startBtn_Click(object sender, EventArgs e)
        {
            RSW2014_List main = this.Owner as RSW2014_List;
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

                byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
                short y = short.Parse(Year.Text);

                if (TypeInfo.SelectedIndex == 0) // исходные данные
                {
                    rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == (TypeInfo.SelectedIndex + 1)).ToList();

                    if (includeAllStaffCheckBox.Checked)
                    {
                        List<long> staffListIN = rsw61List.Select(x => x.StaffID).ToList();
                        List<long> staffListOUT = staffList.Except(staffListIN).ToList();

                        byte q_ = q;

                        while (staffListOUT.Count > 0 && q_ != 3)
                        {
                            switch (q_)
                            {
                                case 0:
                                    q_ = 9;
                                    break;
                                case 9:
                                    q_ = 6;
                                    break;
                                case 6:
                                    q_ = 3;
                                    break;
                            }

                            var rsw61List_t = db.FormsRSW2014_1_Razd_6_1.Where(x => staffListOUT.Contains(x.StaffID) && x.Year == y && x.Quarter == q_ && x.TypeInfoID == (TypeInfo.SelectedIndex + 1)).ToList();

                            if (rsw61List_t.Count > 0)
                            {
                                foreach (var item in rsw61List_t)
                                {
                                    foreach (var item64 in item.FormsRSW2014_1_Razd_6_4)
                                    {
                                        item64.s_1_0 = 0;
                                        item64.s_1_1 = 0;
                                        item64.s_1_2 = 0;
                                        item64.s_1_3 = 0;
                                        item64.s_2_0 = 0;
                                        item64.s_2_1 = 0;
                                        item64.s_2_2 = 0;
                                        item64.s_2_3 = 0;
                                        item64.s_3_0 = 0;
                                        item64.s_3_1 = 0;
                                        item64.s_3_2 = 0;
                                        item64.s_3_3 = 0;
                                    }
                                }

                                rsw61List = rsw61List.Concat(rsw61List_t).ToList();

                                staffListIN = rsw61List_t.Select(x => x.StaffID).ToList();
                                staffListOUT = staffListOUT.Except(staffListIN).ToList();
                            }

                        }
                    }

                }
                else // корректирующие и отменяющие
                {
                    byte qk = byte.Parse(QuarterKorr.SelectedItem.Value.ToString());
                    short yk = short.Parse(YearKorr.Text);
                    rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.YearKorr == yk && x.QuarterKorr == qk && x.TypeInfoID == (TypeInfo.SelectedIndex + 1)).ToList();

                }

                if (action == "svedNachVznos" || action == "svedBaseOPS" || action == "svedVypl")
                {
                    if (PlatCat != null)
                    {
                        rsw61List = rsw61List.Where(x => x.FormsRSW2014_1_Razd_6_4.Any(z => z.PlatCategoryID == PlatCat.ID)).ToList();
                    }
                    else
                    {
                        rsw61List = rsw61List.Where(x => x.FormsRSW2014_1_Razd_6_4.Any()).ToList();
                    }
                }
                if (action == "svedDop")
                {
                    rsw61List = rsw61List.Where(x => x.FormsRSW2014_1_Razd_6_7.Any()).ToList();
                }

                if (action == "svedKorr")
                {
                    rsw61List = rsw61List.Where(x => x.FormsRSW2014_1_Razd_6_6.Any()).ToList();
                }

                rsw61List = rsw61List.OrderBy(x => x.Staff.LastName).ThenBy(x => x.Staff.FirstName).ThenBy(x => x.Staff.MiddleName).ToList();

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

                }
                else
                {
                    RadMessageBox.Show(this, "Нет данных для обработки. Не удалось получить список сотрудников!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }
        }

        private void calculation(object sender, DoWorkEventArgs e)
        {
            int k = 0;
            abort = false;

            foreach (var item in rsw61List)
            {
                if (bw.CancellationPending)
                {
                    rsw61List.Clear();
                    abort = true;
                    return;
                }

                k++;

                decimal temp = (decimal)k / (decimal)rsw61List.Count();
                int proc = (int)Math.Round((temp * 100), 0);
                bw.ReportProgress(proc, k.ToString());
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

    }
}
