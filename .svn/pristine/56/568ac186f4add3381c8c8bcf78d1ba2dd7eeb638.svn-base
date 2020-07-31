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
using PU.Classes;
using Telerik.WinControls.UI;

namespace PU.FormsRSW2014
{
    public partial class PereschetItogSum : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        RSW2014_6_Copy_Wait child = new RSW2014_6_Copy_Wait();
        public BackgroundWorker bw = new BackgroundWorker();
        List<Staff> staffList = new List<Staff>();


        public PereschetItogSum()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(calculation);
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

        private void PereschetItogSum_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

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

            // выпад список "Отчетный период"

            Year_SelectedIndexChanged();

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            startBtn.Enabled = true;
            closeBtn.Enabled = true;

            this.child.Close();
            Methods.showAlert("Внимание", "Перерасчет итоговых сумм выплат произведен!", this.ThemeName);
            this.Close();
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            child.radProgressBar1.Value1 = e.ProgressPercentage;
            child.firstPartLabel.Text = e.UserState.ToString();
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

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PereschetItogSum_FormClosing(object sender, FormClosingEventArgs e)
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

        private void startBtn_Click(object sender, EventArgs e)
        {
            RSW2014_List main = this.Owner as RSW2014_List;

            if (main != null)
            {
                if (main.staffGridView.RowCount > 0 && Year.Text != "" && Quarter.Text != "" && (razd64CheckBox.Checked || razd67CheckBox.Checked ))
                {
                    List<long> staffIDList = new List<long>();

                    switch (copyRangeDDL.SelectedIndex)
                    {
                        case 0:
                            staffIDList.Add(Convert.ToInt64(main.staffGridView.CurrentRow.Cells[1].Value));
                            break;
                        case 1:
                            int rowindex = main.staffGridView.CurrentRow.Index;
                            if (main.staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                            {
                                foreach (var item in main.staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                                {
                                    staffIDList.Add(long.Parse(item.Cells[1].Value.ToString()));
                                }
                            }
                            else
                            {
                                staffIDList.Add(long.Parse(main.staffGridView.Rows[rowindex].Cells[1].Value.ToString()));
                            }

                            break;
                        case 2:
                            foreach (var row in main.staffGridView.Rows)
                            {
                                staffIDList.Add(Convert.ToInt64(row.Cells[1].Value));
                            }
                            break;
                    }

                    byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
                    short y = short.Parse(Year.Text);

                    staffList = db.Staff.Where(x => staffIDList.Contains(x.ID) && x.FormsRSW2014_1_Razd_6_1.Any(c => c.TypeInfoID == 1 && c.Year == y && c.Quarter == q)).ToList();

                    
                    bw.RunWorkerAsync();
                    startBtn.Enabled = false;
                    closeBtn.Enabled = false;


                    child = new RSW2014_6_Copy_Wait();
                    child.Owner = this;
                    child.ownerName = this.Name;
                    child.ThemeName = this.ThemeName;
                    child.titleLabel.Text = "Перерасчет итоговых сумм выплат";
                    child.secondPartLabel.Text = staffList.Count().ToString();
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
            byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());
            short y = short.Parse(Year.Text);

            byte[] qs = new byte[4]{3,6,9,0};
            List<byte> qList = new List<byte>();
            int n = 0;
            while (qs[n] != q)
            {
                qList.Add(qs[n]);
                n++;
            }
            qList.Add(qs[n]);

            foreach (var staff in staffList)
            {
                if (bw.CancellationPending)
                {
                    staffList.Clear();
                    return;
                }
                k++;
                var rsw6 = staff.FormsRSW2014_1_Razd_6_1.FirstOrDefault(c => c.TypeInfoID == 1 && c.Year == y && c.Quarter == q);
                var prevANDCurrRSW6s = staff.FormsRSW2014_1_Razd_6_1.Where(x => x.TypeInfoID == 1 && x.Year == y && qList.Contains(x.Quarter)).ToList();

                if (razd64CheckBox.Checked)  // Если по основным выплатам
                {
                    foreach (var item in rsw6.FormsRSW2014_1_Razd_6_4)
                    {
                        long catID = item.PlatCategoryID;
                        var prevANDCurrRSW64s = prevANDCurrRSW6s.Where(x => x.FormsRSW2014_1_Razd_6_4.Any(c => c.PlatCategoryID == catID)).SelectMany(x => x.FormsRSW2014_1_Razd_6_4.Where(z => z.PlatCategoryID == catID)).ToList();
                        item.s_0_0 = Math.Round(prevANDCurrRSW64s.Where(x => x.s_1_0.HasValue).Sum(x => x.s_1_0.Value) + prevANDCurrRSW64s.Where(x => x.s_2_0.HasValue).Sum(x => x.s_2_0.Value) + prevANDCurrRSW64s.Where(x => x.s_3_0.HasValue).Sum(x => x.s_3_0.Value), 2, MidpointRounding.AwayFromZero);
                        item.s_0_1 = Math.Round(prevANDCurrRSW64s.Where(x => x.s_1_1.HasValue).Sum(x => x.s_1_1.Value) + prevANDCurrRSW64s.Where(x => x.s_2_1.HasValue).Sum(x => x.s_2_1.Value) + prevANDCurrRSW64s.Where(x => x.s_3_1.HasValue).Sum(x => x.s_3_1.Value), 2, MidpointRounding.AwayFromZero);
                        item.s_0_2 = Math.Round(prevANDCurrRSW64s.Where(x => x.s_1_2.HasValue).Sum(x => x.s_1_2.Value) + prevANDCurrRSW64s.Where(x => x.s_2_2.HasValue).Sum(x => x.s_2_2.Value) + prevANDCurrRSW64s.Where(x => x.s_3_2.HasValue).Sum(x => x.s_3_2.Value), 2, MidpointRounding.AwayFromZero);
                        item.s_0_3 = Math.Round(prevANDCurrRSW64s.Where(x => x.s_1_3.HasValue).Sum(x => x.s_1_3.Value) + prevANDCurrRSW64s.Where(x => x.s_2_3.HasValue).Sum(x => x.s_2_3.Value) + prevANDCurrRSW64s.Where(x => x.s_3_3.HasValue).Sum(x => x.s_3_3.Value), 2, MidpointRounding.AwayFromZero);

                        db.ObjectStateManager.ChangeObjectState(item, EntityState.Modified);
                    }
                }

                if (razd67CheckBox.Checked) // если по доп тарифу
                {
                    foreach (var item in rsw6.FormsRSW2014_1_Razd_6_7)
                    {
                        var prevANDCurrRSW67s = prevANDCurrRSW6s.Where(x => x.FormsRSW2014_1_Razd_6_7.Any(c => item.SpecOcenkaUslTrudaID.HasValue ? (c.SpecOcenkaUslTrudaID == item.SpecOcenkaUslTrudaID.Value) : (!c.SpecOcenkaUslTrudaID.HasValue))).SelectMany(x => x.FormsRSW2014_1_Razd_6_7.Where(c => item.SpecOcenkaUslTrudaID.HasValue ? (c.SpecOcenkaUslTrudaID == item.SpecOcenkaUslTrudaID.Value) : (!c.SpecOcenkaUslTrudaID.HasValue))).ToList();
                        item.s_0_0 = Math.Round(prevANDCurrRSW67s.Where(x => x.s_1_0.HasValue).Sum(x => x.s_1_0.Value) + prevANDCurrRSW67s.Where(x => x.s_2_0.HasValue).Sum(x => x.s_2_0.Value) + prevANDCurrRSW67s.Where(x => x.s_3_0.HasValue).Sum(x => x.s_3_0.Value), 2, MidpointRounding.AwayFromZero);
                        item.s_0_1 = Math.Round(prevANDCurrRSW67s.Where(x => x.s_1_1.HasValue).Sum(x => x.s_1_1.Value) + prevANDCurrRSW67s.Where(x => x.s_2_1.HasValue).Sum(x => x.s_2_1.Value) + prevANDCurrRSW67s.Where(x => x.s_3_1.HasValue).Sum(x => x.s_3_1.Value), 2, MidpointRounding.AwayFromZero);

                        db.ObjectStateManager.ChangeObjectState(item, EntityState.Modified);
                    }
                }

                decimal temp = (decimal)k / (decimal)staffList.Count();
                int proc = (int)Math.Round((temp * 100), 0);
                bw.ReportProgress(proc, k.ToString());


                if (k % 50 == 0)
                {
                    db.SaveChanges();
                }

            }

            db.SaveChanges();

        }



    }
}
