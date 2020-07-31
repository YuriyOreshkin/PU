using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using PU.Models;
using Telerik.WinControls.UI;
using System.Reflection;

namespace PU.Staj
{
    public partial class StajList : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string parentName { get; set; }
        public FormsSZV_6_4 szv_6_4 { get; set; }
        public FormsSZV_6 szv_6 { get; set; }
        public FormsSPW2 spw2 { get; set; }

        public StajList()
        {
            InitializeComponent();
            SelfRef = this;
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    stajOsnAddBtn.Enabled = false;
                    stajOsnDelBtn.Enabled = false;
                    stajLgotAddBtn.Enabled = false;
                    stajLgotDelBtn.Enabled = false;
                    moveStajUP.Enabled = false;
                    moveStajDOWN.Enabled = false;
                    moveStajAUTO.Enabled = false;
                    dateControlCheckBox.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    break;
            }
        }

        public static StajList SelfRef
        {
            get;
            set;
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

        private void StajList_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            checkAccessLevel();

            string plCat = "";

            List<RaschetPeriodContainer> rp = new List<RaschetPeriodContainer>();
            RaschetPeriodContainer rp_ = new RaschetPeriodContainer();

            foreach (var item in Options.RaschetPeriodInternal2010_2013)
            {
                rp.Add(item);
            }
            foreach (var item in Options.RaschetPeriodInternal)
            {
                rp.Add(item);
            }

            switch (parentName)
            {
                case "SZV_6_4":
                    plCat = szv_6_4.PlatCategory != null ? szv_6_4.PlatCategory.Code : " ";
                    rp_ = rp.FirstOrDefault(x => x.Kvartal == szv_6_4.Quarter && x.Year == szv_6_4.Year);
                    this.Text = "Индивидуальные сведения: Стаж работы [" + plCat + "] за период " + rp_.Name;
                    break;
                case "SZV_6":
                    plCat = szv_6.PlatCategory != null ? szv_6.PlatCategory.Code : " ";
                    rp_ = rp.FirstOrDefault(x => x.Kvartal == szv_6.Quarter && x.Year == szv_6.Year);
                    this.Text = "Индивидуальные сведения: Стаж работы [" + plCat + "] за период " + rp_.Name;
                    break;
                case "SPW2":
                    plCat = spw2.PlatCategory != null ? spw2.PlatCategory.Code : " ";
                    rp_ = rp.FirstOrDefault(x => x.Kvartal == spw2.Quarter && x.Year == spw2.Year);
                    this.Text = "Сведения для установления пенсии СПВ-2: Стаж работы [" + plCat + "] за период " + rp_.Name;
                    break;
            }




            gridUpdate_StajOsn();
         //   this.stajLgotGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;


        }

        #region Основной стаж

        /// <summary>
        /// обновление таблицы  Основной стаж
        /// </summary>
        private void gridUpdate_StajOsn()
        {
            List<StajOsn> StajOsn_List = new List<StajOsn>();

            switch (parentName)
            {
                case "SZV_6_4":
                    StajOsn_List = db.StajOsn.Where(x => x.FormsSZV_6_4_ID == szv_6_4.ID).ToList();
                    break;
                case "SZV_6":
                    StajOsn_List = db.StajOsn.Where(x => x.FormsSZV_6_ID == szv_6.ID).ToList();
                    break;
                case "SPW2":
                    StajOsn_List = db.StajOsn.Where(x => x.FormsSPW2_ID == spw2.ID).ToList();
                    break;
            }

            stajOsnGrid.Rows.Clear();
            StajOsn_List = StajOsn_List.OrderBy(x => x.Number.Value).ToList();
            foreach (var item in StajOsn_List)
            {
                GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.stajOsnGrid.MasterView);
                rowInfo.Cells["ID"].Value = item.ID;
                rowInfo.Cells["Number"].Value = item.Number;
                rowInfo.Cells["DateBegin"].Value = item.DateBegin.Value.ToShortDateString();
                rowInfo.Cells["DateEnd"].Value = item.DateEnd.Value.ToShortDateString();
                stajOsnGrid.Rows.Add(rowInfo);
            }
            

            this.stajOsnGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            stajOsnGrid.Refresh();

            // Обновляем таблицу доп записей по стажу
            gridUpdate_StajLgot();
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stajOsnAddBtn_Click(object sender, EventArgs e)
        {
            StajOsnFrm child = new StajOsnFrm();
            child.Owner = this;
            child.action = "add";
            child.dateControl = dateControlCheckBox.Checked;
            child.ParentFormName = parentName;
            child.formData = new StajOsn();
            short y = 0;
            byte q = 0;
            switch (parentName)
            {
                case "SZV_6_4":
                    child.formData.FormsSZV_6_4_ID = szv_6_4.ID;
                    y = szv_6_4.TypeInfoID == 1 ? szv_6_4.Year : szv_6_4.YearKorr.Value;
                    q = szv_6_4.TypeInfoID == 1 ? szv_6_4.Quarter : szv_6_4.QuarterKorr.Value;
                    break;
                case "SZV_6":
                    child.formData.FormsSZV_6_ID = szv_6.ID;
                    y = szv_6.TypeInfoID == 1 ? szv_6.Year : szv_6.YearKorr.Value;
                    q = szv_6.TypeInfoID == 1 ? szv_6.Quarter : szv_6.QuarterKorr.Value;
                    break;
                case "SPW2":
                    child.formData.FormsSPW2_ID = spw2.ID;
                    y = spw2.TypeInfoID == 1 ? spw2.Year : spw2.YearKorr.Value;
                    q = spw2.TypeInfoID == 1 ? spw2.Quarter : spw2.QuarterKorr.Value;
                    break;
            }
            child.rowindex = -1;
            List<RaschetPeriodContainer> avail_periods_all = new List<RaschetPeriodContainer>();
            var avail_periods = Options.RaschetPeriodInternal.OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }
            avail_periods = Options.RaschetPeriodInternal2010_2013.OrderBy(x => x.Year);
            foreach (var item in avail_periods)
            {
                avail_periods_all.Add(item);
            }

            child.period = avail_periods_all.FirstOrDefault(x => x.Year == y && x.Kvartal == q);
            if (stajOsnGrid.RowCount == 0)
            {
                child.StajBeginDate.Value = child.period.DateBegin;
            }
            else
            {
                var date = stajOsnGrid.Rows.Where(x => x.Cells["DateEnd"].Value != null).Max(x => DateTime.Parse(x.Cells["DateEnd"].Value.ToString())).Date;
                child.StajBeginDate.Value = date.AddDays(1);
                child.NumberSpin.Value = stajOsnGrid.Rows.Where(x => x.Cells["Number"].Value != null).Max(x => int.Parse(x.Cells["Number"].Value.ToString())) + 1;
            }

            child.StajEndDate.Value = child.period.DateEnd;

            if (child.StajBeginDate.Value > child.StajEndDate.Value)
            {
                child.StajEndDate.Value = child.StajBeginDate.Value.AddDays(1);
            }

            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();

            gridUpdate_StajOsn();
        } 


        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stajOsnEditBtn_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0 && stajOsnGrid.CurrentRow.Cells[0].Value != null)
            {
                int rowindex = stajOsnGrid.CurrentRow.Index;
                long id = long.Parse(stajOsnGrid.CurrentRow.Cells[0].Value.ToString());

                StajOsnFrm child = new StajOsnFrm();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.dateControl = dateControlCheckBox.Checked;
                child.ParentFormName = parentName;
                short y = 0;
                byte q = 0;
                switch (parentName)
                {
                    case "SZV_6_4":
                        y = szv_6_4.TypeInfoID == 1 ? szv_6_4.Year : szv_6_4.YearKorr.Value;
                        q = szv_6_4.TypeInfoID == 1 ? szv_6_4.Quarter : szv_6_4.QuarterKorr.Value;
                        break;
                    case "SZV_6":
                        y = szv_6.TypeInfoID == 1 ? szv_6.Year : szv_6.YearKorr.Value;
                        q = szv_6.TypeInfoID == 1 ? szv_6.Quarter : szv_6.QuarterKorr.Value;
                        break;
                    case "SPW2":
                        y = spw2.TypeInfoID == 1 ? spw2.Year : spw2.YearKorr.Value;
                        q = spw2.TypeInfoID == 1 ? spw2.Quarter : spw2.QuarterKorr.Value;
                        break;
                }
                var periodTemp = Options.RaschetPeriodInternal.Concat(Options.RaschetPeriodInternal2010_2013.ToList());
                child.period = periodTemp.FirstOrDefault(x => x.Year == y && x.Kvartal == q);
                child.formData = db.StajOsn.FirstOrDefault(x => x.ID == id);
                child.rowindex = rowindex;
                child.ShowDialog();

                gridUpdate_StajOsn();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }


        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stajOsnDelBtn_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0 && stajOsnGrid.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(stajOsnGrid.CurrentRow.Cells[0].Value.ToString());

                StajOsn so = db.StajOsn.FirstOrDefault(x => x.ID == id);

                if (so.StajLgot.Any())
                {
                    List<long> ids = so.StajLgot.Select(x => x.ID).ToList();
                    foreach (var item in ids)
                    {
                        StajLgot sl = db.StajLgot.FirstOrDefault(x => x.ID == item);
                        db.StajLgot.Remove(sl);
                    }

                }

                db.StajOsn.Remove(so);
                db.SaveChanges();

                gridUpdate_StajOsn();

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void stajOsnGrid_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            stajOsnEditBtn_Click(null,null);
        }

        private void stajOsnGrid_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            gridUpdate_StajLgot();
        }

        
        #endregion

        #region Льготный стаж

        /// <summary>
        /// обновление таблицы раздела Льготный стаж
        /// </summary>
        private void gridUpdate_StajLgot()
        {
            stajLgotGrid.Rows.Clear();

            if (stajOsnGrid.RowCount != 0)
            {
                int rowindex = 0;

                if (stajOsnGrid.CurrentRow != null)
                {
                    rowindex = stajOsnGrid.CurrentRow.Index;
                }
                long id = long.Parse(stajOsnGrid.Rows[rowindex].Cells[0].Value.ToString());

                StajOsn so = db.StajOsn.FirstOrDefault(x => x.ID == id);
                List<StajLgot> StajLgot_List = so.StajLgot.OrderBy(x => x.Number).ToList();

                if (StajLgot_List.Count() != 0)
                {
                    foreach (var item in StajLgot_List)
                    {
                        string str = item.IschislStrahStajDopID == null ? "" : item.IschislStrahStajDopID.HasValue ? db.IschislStrahStajDop.FirstOrDefault(x => x.ID == item.IschislStrahStajDopID).Code : "";
                        string s1 = item.Strah1Param.HasValue == true ? item.Strah1Param.Value.ToString() : "0";
                        string s2 = item.Strah2Param.HasValue == true ? item.Strah2Param.Value.ToString() : "0";

                        str = "[" + s1 + "][" + s2 + "][" + str + "]";

                        string s1_ = item.UslDosrNazn1Param.HasValue == true ? item.UslDosrNazn1Param.Value.ToString() : "0";
                        string s2_ = item.UslDosrNazn2Param.HasValue == true ? item.UslDosrNazn2Param.Value.ToString() : "0";
                        string s3_ = item.UslDosrNazn3Param.HasValue == true ? item.UslDosrNazn3Param.Value.ToString() : "0.00";

                        string str_ = "[" + s1_ + "][" + s2_ + "][" + s3_ + "]";

                        GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.stajLgotGrid.MasterView);
                        rowInfo.Cells["ID"].Value = item.ID;
                        rowInfo.Cells["Number"].Value = item.Number;
                        rowInfo.Cells["TerrUslCode"].Value = item.TerrUslID.HasValue ? item.TerrUsl.Code : "";
                        rowInfo.Cells["TerrUslKoef"].Value = item.TerrUslKoef.HasValue ? item.TerrUslKoef.Value.ToString() : "";
                        rowInfo.Cells["OsobUslCode"].Value = item.OsobUslTrudaID.HasValue ? item.OsobUslTruda.Code : "";
                        rowInfo.Cells["KodVredOsn"].Value = item.KodVred_OsnID.HasValue ? item.KodVred_2.Code : "";
                        rowInfo.Cells["IschislStrahOsn"].Value = item.IschislStrahStajOsnID == null ? "" : item.IschislStrahStajOsnID.HasValue ? db.IschislStrahStajOsn.FirstOrDefault(x => x.ID == item.IschislStrahStajOsnID).Code : "";
                        rowInfo.Cells["IschislStrahDop"].Value = str;
                        rowInfo.Cells["UslDosrNaznOsn"].Value = item.UslDosrNaznID == null ? "" : item.UslDosrNaznID.HasValue ? db.UslDosrNazn.FirstOrDefault(x => x.ID == item.UslDosrNaznID).Code : "";
                        rowInfo.Cells["UslDosrNaznDop"].Value = str_;
                        stajLgotGrid.Rows.Add(rowInfo);
                    }
                }

         //       this.stajLgotGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

                stajLgotGrid.Refresh();
            }
            // Обновляем таблицу доп записей по стажу
            //            gridUpdate_StajDop();
        }


        /// <summary>
        /// Добавление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void stajLgotAddBtn_Click(object sender, EventArgs e)
        {
            StajLgotFrm child = new StajLgotFrm();
            child.Owner = this;
            child.action = "add";
            child.ParentFormName = parentName;
            long id = long.Parse(stajOsnGrid.CurrentRow.Cells[0].Value.ToString());

            child.StajOsnData = db.StajOsn.FirstOrDefault(x => x.ID == id);

            if (child.StajOsnData.StajLgot.Count != 0)
            {
                child.NumberSpin.Value = child.StajOsnData.StajLgot.Max(x => x.Number).Value + 1;
            }

            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
            db = null;
            db = new pu6Entities();
            gridUpdate_StajLgot();
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void stajLgotEditBtn_Click(object sender, EventArgs e)
        {
            if (stajLgotGrid.RowCount != 0 && stajOsnGrid.CurrentRow.Cells[0].Value != null)
            {
                int rowindex = stajLgotGrid.CurrentRow.Index;
                long id = long.Parse(stajOsnGrid.CurrentRow.Cells[0].Value.ToString());

                StajOsn st = db.StajOsn.FirstOrDefault(x => x.ID == id);

                StajLgotFrm child = new StajLgotFrm();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.ParentFormName = parentName;
                child.StajOsnData = st;
                child.formData = st.StajLgot.Skip(rowindex).Take(1).First();
                child.rowindex = rowindex;
                child.ShowDialog();
                db = new pu6Entities();
                gridUpdate_StajLgot();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }


        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stajLgotDelBtn_Click(object sender, EventArgs e)
        {
            if (stajLgotGrid.RowCount != 0 && stajLgotGrid.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(stajLgotGrid.CurrentRow.Cells[0].Value.ToString());

                StajLgot sl = db.StajLgot.FirstOrDefault(x => x.ID == id);
                db.StajLgot.Remove(sl);
                db.SaveChanges();

                gridUpdate_StajLgot();

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }


        #endregion

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            int y = panel1.Height / 2;
            stajOsnGroupBox.Height = y - 20;
            stajLgotGroupBox.Top = y;
            stajLgotGroupBox.Height = stajOsnGroupBox.Height;
        }

        private void moveStajUP_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int index = stajOsnGrid.CurrentRow.Index;
                if (index != 0)
                {

                    long numPrev = 0;
                    long numCurr = 0;
                    long.TryParse(stajOsnGrid.Rows.Skip(index - 1).Take(1).First().Cells["Number"].Value.ToString(), out numPrev);
                    long.TryParse(stajOsnGrid.Rows.Skip(index).Take(1).First().Cells["Number"].Value.ToString(), out numCurr);
                    if (numPrev == 0 || numCurr == 0)
                        return;

                    long id = long.Parse(stajOsnGrid.Rows.Skip(index).Take(1).First().Cells[0].Value.ToString());
                    StajOsn stCurr = db.StajOsn.First(x => x.ID == id);
                    stCurr.Number--;
                    db.Entry(stCurr).State = EntityState.Modified;

                    if ((numCurr - numPrev) == 1) // если последовательность один за одним например 2, 3
                    {
                        id = long.Parse(stajOsnGrid.Rows.Skip(index - 1).Take(1).First().Cells[0].Value.ToString());
                        StajOsn stOld = db.StajOsn.First(x => x.ID == id);
                        stOld.Number++;
                        db.Entry(stOld).State = EntityState.Modified;
                        index--;
                    }
                    else // если в последовательности есть промежутки например 2, 4
                    {

                    }
                    db.SaveChanges();

                    gridUpdate_StajOsn();
                    stajOsnGrid.Rows[index].IsCurrent = true;
                }

            }
        }

        private void moveStajDOWN_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                int index = stajOsnGrid.CurrentRow.Index;
                if (index != (stajOsnGrid.RowCount - 1))
                {

                    long numNext = 0;
                    long numCurr = 0;
                    long.TryParse(stajOsnGrid.Rows.Skip(index + 1).Take(1).First().Cells["Number"].Value.ToString(), out numNext);
                    long.TryParse(stajOsnGrid.Rows.Skip(index).Take(1).First().Cells["Number"].Value.ToString(), out numCurr);
                    if (numNext == 0 || numCurr == 0)
                        return;

                    long id = long.Parse(stajOsnGrid.Rows.Skip(index).Take(1).First().Cells[0].Value.ToString());
                    StajOsn stCurr = db.StajOsn.First(x => x.ID == id);
                    stCurr.Number++;
                    db.Entry(stCurr).State = EntityState.Modified;

                    if ((numNext - numCurr) == 1) // если последовательность один за одним например 2, 3
                    {
                        id = long.Parse(stajOsnGrid.Rows.Skip(index + 1).Take(1).First().Cells[0].Value.ToString());
                        StajOsn stOld = db.StajOsn.First(x => x.ID == id);
                        stOld.Number--;
                        db.Entry(stOld).State = EntityState.Modified;
                        index++;
                    }
                    else // если в последовательности есть промежутки например 2, 4
                    {

                    }
                    db.SaveChanges();

                    gridUpdate_StajOsn();
                    stajOsnGrid.Rows[index].IsCurrent = true;
                }

            }
        }

        private void moveStajAUTO_Click(object sender, EventArgs e)
        {
            if (stajOsnGrid.RowCount != 0)
            {
                List<StajOsn> StajOsn_List = new List<StajOsn>();

                switch (parentName)
                {
                    case "SZV_6_4":
                        StajOsn_List = db.StajOsn.Where(x => x.FormsSZV_6_4_ID == szv_6_4.ID).OrderBy(x => x.DateBegin.Value).ToList();
                        break;
                    case "SZV_6":
                        StajOsn_List = db.StajOsn.Where(x => x.FormsSZV_6_ID == szv_6.ID).OrderBy(x => x.DateBegin.Value).ToList();
                        break;
                    case "SPW2":
                        StajOsn_List = db.StajOsn.Where(x => x.FormsSPW2_ID == spw2.ID).OrderBy(x => x.DateBegin.Value).ToList();
                        break;
                }

                int num = 1;
                bool flag = false;

                foreach (var item in StajOsn_List)
                {
                    if (item.Number != num)
                    {
                        item.Number = num;
                        db.Entry(item).State = EntityState.Modified;
                        flag = true;
                    }
                    num++;
                }

                if (flag)
                {
                    db.SaveChanges();
                }


                gridUpdate_StajOsn();
            }
        }
















    }
}
