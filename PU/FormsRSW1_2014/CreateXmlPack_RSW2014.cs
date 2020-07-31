using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using System.Xml.Linq;
using System.Linq.Dynamic;

namespace PU.FormsRSW2014
{
    public partial class CreateXmlPack_RSW2014 : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        pfrXMLEntities dbxml = new pfrXMLEntities();
        BackgroundWorker bw = new BackgroundWorker();


        private XNamespace pfr = "http://schema.pfr.ru";

        public List<long> staffList_temp { get; set; }
        public long currentStaffId = 0;
        private int yearType = 2014;
        private bool noErrors = true;
        private bool cancel_work = false;

        List<PlatCategory> PlatCatList = new List<PlatCategory>();
        List<TerrUsl> TerrUsl_list = new List<TerrUsl>();
        List<OsobUslTruda> OsobUslTruda_list = new List<OsobUslTruda>();
        List<KodVred_2> KodVred_2_list = new List<KodVred_2>();
        List<IschislStrahStajOsn> IschislStrahStajOsn_list = new List<IschislStrahStajOsn>();
        List<IschislStrahStajDop> IschislStrahStajDop_list = new List<IschislStrahStajDop>();
        List<UslDosrNazn> UslDosrNazn_list = new List<UslDosrNazn>();
        List<SpecOcenkaUslTruda> SpecOcenkaUslTruda_list = new List<SpecOcenkaUslTruda>();

        List<FormsRSW2014_1_Razd_6_1> rsw61List = new List<FormsRSW2014_1_Razd_6_1>();
        List<FormsSZV_6_4> szv64List = new List<FormsSZV_6_4>();
        List<FormsSZV_6> szv6List = new List<FormsSZV_6>();
        List<List<FormsRSW2014_1_Razd_6_1>> rsw61List_part = new List<List<FormsRSW2014_1_Razd_6_1>>();
        List<List<FormsSZV_6_4>> szv64List_part = new List<List<FormsSZV_6_4>>();
        List<List<FormsSZV_6>> szv61List_part = new List<List<FormsSZV_6>>();
        List<List<FormsSZV_6>> szv62List_part = new List<List<FormsSZV_6>>();
        List<RaschetPeriodContainer> korrPeriods = new List<RaschetPeriodContainer>();

        List<StajOsn> stajOsn_list = new List<StajOsn>();
        List<StajLgot> stajLgot_list = new List<StajLgot>();
        List<FormsRSW2014_1_Razd_6_4> razd64_list = new List<FormsRSW2014_1_Razd_6_4>();
        List<FormsRSW2014_1_Razd_6_6> razd66_list = new List<FormsRSW2014_1_Razd_6_6>();
        List<FormsRSW2014_1_Razd_6_7> razd67_list = new List<FormsRSW2014_1_Razd_6_7>();
        List<Staff> StaffList_All_Ishod = new List<Staff>();


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

        public CreateXmlPack_RSW2014()
        {
            InitializeComponent();
        }

        private void CreateXmlPack_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            this.radPageView1.SelectedPage = this.radPageView1.Pages[0];
            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014 && x.Year <= 2018).OrderBy(x => x.Year);
            DateUnderwrite.Value = DateTime.Now.Date;

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
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                if (Quarter.Items.Count() > 0)
                {
                    DateTime dt = DateTime.Now;
                    RaschetPeriodContainer rp = new RaschetPeriodContainer();

                    if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                    else
                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                    if (rp != null && Quarter.Items.Any(x => x.Value.ToString() == rp.Kvartal.ToString()))
                    {
                        Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                    }
                    else
                    {
                        Quarter.Items.First().Selected = true;
                    }
                }

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
                                    Quarter_SelectedIndexChanged();
                                }
                                break;
                            case "createRazd_2_5":
                                createRazd_2_5.Checked = item.value == "true" ? true : false;
                                break;
                            case "includeRSW2014_1":
                                includeRSW2014_1.Checked = item.value == "true" ? true : false;
                                break;
                            case "switchStaffListDDL":
                                int.TryParse(item.value, out i);
                                switchStaffListDDL.SelectedIndex = i;
                                break;
                            case "sortingDDL":
                                int.TryParse(item.value, out i);
                                sortingDDL.SelectedIndex = i;
                                break;
                            case "TypeInfoDDL":
                                int.TryParse(item.value, out i);
                                TypeInfoDDL.SelectedIndex = i;
                                break;
                        }
                    }

                }
                catch
                { }
            }

            if (Options.saveLastPackNum)
            {
                try // пробуем восстановить последний номер пачки
                {
                    Props props = new Props(); //экземпляр класса с настройками
                    numFrom.Value = props.getPackNum(this.Name);
                }
                catch { }
            }


            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();
            this.TypeInfoDDL.SelectedIndexChanged += (s, с) => TypeInfoDDL_SelectedIndexChanged();
            filterKorrPeriod_ToggleStateChanged(null, null);

            TypeInfoDDL_SelectedIndexChanged();

        }

        private void Year_SelectedIndexChanged()
        {
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            this.Quarter.Items.Clear();
            filterKorrPeriod_ToggleStateChanged(null, null);
            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year == y).ToList())
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

            }
        }

        public void korrPerGrid_upd()
        {
            BindingSource b = new BindingSource();
            List<RaschetPeriodContainer> rp = new List<RaschetPeriodContainer>();
            foreach (var item in Options.RaschetPeriodInternal2010_2013)
            {
                rp.Add(item);
            }
            foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year < 2017))
            {
                rp.Add(item);
            }

            if (filterKorrPeriod.Checked)
            {
                short yFrom = (short)yearFilterFrom.Value;
                short yTo = (short)yearFilterTo.Value;

                rp = rp.Where(x => x.Year >= yFrom && x.Year <= yTo).ToList();
            }

            b.DataSource = rp;

            korrPerGrid.DataSource = b;


            korrPerGrid.Columns["CheckBox"].Width = 24;
            //  korrPerGrid.Columns["ID"].IsVisible = false;
            korrPerGrid.Columns["Year"].HeaderText = "Год";
            korrPerGrid.Columns["Year"].Width = 60;
            korrPerGrid.Columns["Year"].TextAlignment = ContentAlignment.MiddleCenter;

            korrPerGrid.Columns["Kvartal"].HeaderText = "Квартал";
            korrPerGrid.Columns["Kvartal"].Width = 60;
            korrPerGrid.Columns["Kvartal"].TextAlignment = ContentAlignment.MiddleCenter;

            korrPerGrid.Columns["Name"].HeaderText = "Обозначение";
            korrPerGrid.Columns["Name"].Width = 120;

            korrPerGrid.Columns["DateBegin"].HeaderText = "Начало";
            korrPerGrid.Columns["DateBegin"].Width = 80;
            korrPerGrid.Columns["DateBegin"].FormatString = "{0:dd.MM.yyyy}";
            korrPerGrid.Columns["DateBegin"].TextAlignment = ContentAlignment.MiddleCenter;

            korrPerGrid.Columns["DateEnd"].HeaderText = "Конец";
            korrPerGrid.Columns["DateEnd"].Width = 80;
            korrPerGrid.Columns["DateEnd"].FormatString = "{0:dd.MM.yyyy}";
            korrPerGrid.Columns["DateEnd"].TextAlignment = ContentAlignment.MiddleCenter;
            this.korrPerGrid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            korrPerGrid.Refresh();

        }

        private void Quarter_SelectedIndexChanged()
        {
            dataGrid_upd();
        }

        private void TypeInfoDDL_SelectedIndexChanged()
        {
            bool flag = true;
            if (TypeInfoDDL.SelectedIndex > 0)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            radPageView1.Pages[2].Enabled = flag;
        }

        /// <summary>
        /// Обновление таблицы формы РСВ-1
        /// </summary>
        public void dataGrid_upd()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            //            List<FormsRSW2014_1_1> rsw = new List<FormsRSW2014_1_1>();
            var rsw = db.FormsRSW2014_1_1.Where(x => x.InsurerID == Options.InsID).ToList();

            radGridView1.Rows.Clear();

            short y;
            if (short.TryParse(Year.Text, out y))
            {
                rsw = rsw.Where(x => x.Year == y).ToList();
            }

            if (Quarter.SelectedItem != null)
            {
                byte q;
                if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                {
                    rsw = rsw.Where(x => x.Quarter == q).ToList();

                    if (rsw.Count() != 0)
                    {
                        foreach (var item in rsw)
                        {
                            GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView1.MasterView);
                            rowInfo.Cells["ID"].Value = item.ID;
                            rowInfo.Cells["Year"].Value = item.Year;
                            rowInfo.Cells["Period"].Value = Options.RaschetPeriodInternal.Any(x => x.Year == item.Year && x.Kvartal == item.Quarter) ? (item.Quarter.ToString() + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Quarter).Name) : "Период не определен";
                            rowInfo.Cells["KorrNum"].Value = item.CorrectionNum;
                            rowInfo.Cells["OPS"].Value = item.s_110_0.HasValue ? item.s_110_0.Value : 0;
                            rowInfo.Cells["OMS"].Value = item.s_110_5.HasValue ? item.s_110_5.Value : 0;
                            rowInfo.Cells["dopTar1"].Value = item.s_110_3.HasValue ? item.s_110_3.Value : 0;
                            rowInfo.Cells["dopTar2"].Value = item.s_110_4.HasValue ? item.s_110_4.Value : 0;
                            //rowInfo.Cells["OPS"].Value = item.s_110_0.Value != 0 ? Math.Round(item.s_110_0.Value, 2).ToString() : "0,00";
                            //rowInfo.Cells["OMS"].Value = item.s_110_5.Value != 0 ? Math.Round(item.s_110_5.Value, 2).ToString() : "0,00";
                            //rowInfo.Cells["dopTar1"].Value = item.s_110_3.Value != 0 ? Math.Round(item.s_110_3.Value, 2).ToString() : "0,00";
                            //rowInfo.Cells["dopTar2"].Value = item.s_110_4.Value != 0 ? Math.Round(item.s_110_4.Value, 2).ToString() : "0,00";


                            radGridView1.Rows.Add(rowInfo);
                        }
                    }

                    for (var i = 0; i < radGridView1.Columns.Count; i++)
                    {
                        radGridView1.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                        radGridView1.Columns[i].ReadOnly = true;
                    }

                }


            }

        }

        private void updateRSWListBtn_Click(object sender, EventArgs e)
        {
            dataGrid_upd();
        }

        private void includeRSW2014_1_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            createRazd_2_5.Enabled = includeRSW2014_1.Checked;
            radPageView1.Pages[1].Enabled = includeRSW2014_1.Checked;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            rsw61List = new List<FormsRSW2014_1_Razd_6_1>();
            szv64List = new List<FormsSZV_6_4>();
            szv6List = new List<FormsSZV_6>();
            rsw61List_part = new List<List<FormsRSW2014_1_Razd_6_1>>();
            szv64List_part = new List<List<FormsSZV_6_4>>();
            szv61List_part = new List<List<FormsSZV_6>>();
            szv62List_part = new List<List<FormsSZV_6>>();
            korrPeriods = new List<RaschetPeriodContainer>();

            stajOsn_list = new List<StajOsn>();
            stajLgot_list = new List<StajLgot>();
            razd64_list = new List<FormsRSW2014_1_Razd_6_4>();
            razd66_list = new List<FormsRSW2014_1_Razd_6_6>();
            razd67_list = new List<FormsRSW2014_1_Razd_6_7>();
            StaffList_All_Ishod = new List<Staff>();

            if (TypeInfoDDL.SelectedIndex > 0)
            {
                if (korrPerGrid.RowCount > 0 && korrPerGrid.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {
                    foreach (var item in korrPerGrid.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                    {
                        korrPeriods.Add(new RaschetPeriodContainer
                        {
                            Year = short.Parse(item.Cells[1].Value.ToString()),
                            Kvartal = byte.Parse(item.Cells[2].Value.ToString())
                        });
                    }
                }
                else
                {
                    RadMessageBox.Show("Вы выбрали Тип сведений " + TypeInfoDDL.Text + ". Надо выбрать корректируемые периоды!", "Внимание");
                    return;
                }
            }

            if ((switchStaffListDDL.SelectedIndex == 2 && (staffList_temp == null || (staffList_temp != null && staffList_temp.Count <= 0))) || (switchStaffListDDL.SelectedIndex == 1 && currentStaffId == 0))
            {
                RadMessageBox.Show("Пустой список сотрудников для формирования! Необходимо выбрать сотрудников!", "Внимание");
                return;
            }

            if ((includeRSW2014_1.Checked) && (radGridView1.RowCount <= 0)) // если выбрано включать форму РСВ-1 но подходящих форм нет
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы выбрали режим совместного формирования пачек Инд.сведений с формой РСВ-1.\r\nУ вас не выбрана ни одна форма РСВ-1!\r\nПачки будут сформированы без формы РСВ-1.\r\nПродолжить?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.Yes)
                {
                    /*                    BaseManager child = new BaseManager();
                                        child.Owner = this;
                                        child.ThemeName = this.ThemeName;
                                        child.ShowInTaskbar = true;
                                        child.ShowDialog();
                    */




                    try
                    {
                        createPacksStartBW();
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "Во время формирования пачек произошла ошибка. Код ошибки: " + ex.InnerException, "Ошибка");
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                try
                {
                    createPacksStartBW();
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(this, "Во время формирования пачек произошла ошибка. Код ошибки: " + ex.InnerException, "Ошибка");
                }
            }
        }

        void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            radProgressBar1.Value1 = e.ProgressPercentage;
            //firstPartLabel.Text = e.UserState.ToString();
        }

        private void createPacksStartBW()
        {
            viewPacks.Enabled = false;
            startBtn.Enabled = false;
            closeBtn.Enabled = false;

            PlatCatList = db.PlatCategory.Where(x => x.PlatCategoryRaschPerID == 4).ToList();
            TerrUsl_list = db.TerrUsl.ToList();
            OsobUslTruda_list = db.OsobUslTruda.ToList();
            KodVred_2_list = db.KodVred_2.ToList();
            IschislStrahStajOsn_list = db.IschislStrahStajOsn.ToList();
            IschislStrahStajDop_list = db.IschislStrahStajDop.ToList();
            UslDosrNazn_list = db.UslDosrNazn.ToList();
            SpecOcenkaUslTruda_list = db.SpecOcenkaUslTruda.ToList();

            bw = new BackgroundWorker();
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(createPacks);
            bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bw_ProgressChanged);
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;


            bw.RunWorkerAsync();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            db = new pu6Entities();
            viewPacks.Enabled = true;
            startBtn.Enabled = true;
            closeBtn.Enabled = true;

            //    ReportViewerRSW2014.Invoke(new Action(() => {  }));
            if (noErrors)
            {
                RadMessageBox.Show(this, "Пачки сформированы", "Внимание");
                viewPacks_Click(null, null);
            }


        }

        private void bwCancel(string caption, string errMess)
        {
            if (!cancel_work)
            {
                this.Invoke(new Action(() => { RadMessageBox.Show(errMess, caption); }));//Methods.showAlert(caption, errMess, this.ThemeName); 
            }
            bw.ReportProgress(0, "0");
            titleLabel.Invoke(new Action(() => { titleLabel.Text = "Текущая операция"; }));

            noErrors = false;
            bw.CancelAsync();
            return;

        }

        private void updateProgressBar(int current, int count)
        {
            var tm = (decimal)current / (decimal)count;
            var proc = (int)Math.Round((tm * 100), 0);
            bw.ReportProgress(proc, current.ToString());
        }


        private class stajLgotCont
        {
            public long? OsobUslTrudaID { get; set; }
            public long? IschislStrahStajOsnID { get; set; }
            public long? UslDosrNaznID { get; set; }
            public long StajOsnID { get; set; }
        }
        private class stajOsnCont
        {
            public long? FormsRSW2014_1_Razd_6_1_ID { get; set; }
            public long ID { get; set; }
        }

        private void createPacks(object sender, DoWorkEventArgs e)
        {
            rsw61List_part = new List<List<FormsRSW2014_1_Razd_6_1>>();
            szv64List_part = new List<List<FormsSZV_6_4>>();
            szv61List_part = new List<List<FormsSZV_6>>();
            szv62List_part = new List<List<FormsSZV_6>>();

            bw.ReportProgress(0, "0");

            int packs_cnt_ALL = 0;


            short y = short.Parse(Year.Text);
            byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());


            Guid guid = Guid.NewGuid();
            try // Проверяем Guid на уникальность и на целостность промежуточной базы, если база порушена, надо ее пересоздать
            {
                while (dbxml.xmlInfo.Any(x => x.UniqGUID == guid))
                {
                    guid = Guid.NewGuid();
                }
            }
            catch
            {
                string pfrXMLPath = System.IO.Path.Combine(Application.StartupPath, "pfrXML.db3");
                string result = String.Empty;

                System.Threading.Thread.Sleep(500);
                System.IO.File.Delete(pfrXMLPath);
                MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками

                result = methodsNonStatic.createXmlDB(pfrXMLPath);

                if (!String.IsNullOrEmpty(result)) // если все хорошо, база пересоздана, 
                {
                    RadMessageBox.Show("Была обнаружена проблема с промежуточной базой данных для сформированных пачек. Автоматическое восставновление базы завершилось ошибкой - " + result + "\r\nНеобходимо вручную удалить файл pfrXML.db3 из каталога с программой и перезапустить ее!");
                    return;
                }
            }


            short y_;
            if (short.TryParse(Year.Text, out y_))
            {
                if (Quarter.SelectedItem != null)
                {
                    byte q_;
                    if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q_))
                    {
                        string query = string.Empty;
                        try
                        {
                            titleLabel.Invoke(new Action(() => { titleLabel.Text = "Подготовка временной базы данных"; }));
                            query = String.Format("DELETE FROM xmlInfo WHERE ([Year] = {0} AND [Quarter] = {1} AND [InsurerID] = {2} AND [FormatType] = 'rsw2014'); VACUUM;", y_, q_, Options.InsID);
                            dbxml.Database.ExecuteSqlCommand(query);
                        }
                        catch
                        {
                            bwCancel("Ошибка", "Во время формирования пачек формы РСВ-1 произошла ошибка. Не выполнен запрос [ " + query + "]!");
                            if (bw.CancellationPending)
                            {
                                return;
                            }

                        }

                        yearType = ((y_ == 2014) || (y_ == 2015 && q_ == 3)) ? 2014 : 2015;

                    }
                    else
                    {
                        bwCancel("Ошибка", "Во время формирования пачек формы РСВ-1 произошла ошибка конвертации Отчетного периода!");
                        if (bw.CancellationPending)
                        {
                            return;
                        }
                    }
                }
            }
            else
            {
                bwCancel("Ошибка", "Во время формирования пачек формы РСВ-1 произошла ошибка конвертации Года!");
                if (bw.CancellationPending)
                {
                    return;
                }
            }


            long parentID = 0;
            int num = (int)(numFrom.Value - 1);
            packs_cnt_ALL = (int)numFrom.Value;            
            xmlInfo xml_info;
            string fileName;
            string regNum = Utils.ParseRegNum(db.Insurer.First(x => x.ID == Options.InsID).RegNum);
            string[] infoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };

            // если формируем вместе с РСВ-1
            if ((includeRSW2014_1.Checked) && (radGridView1.RowCount > 0)) // если выбрано включать форму РСВ-1
            {
                titleLabel.Invoke(new Action(() => { titleLabel.Text = "Выборка по Форме РСВ-1"; }));

                long rswID = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());
                var rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == rswID);
                num++;
                fileName = String.Format("PFR-700-Y-{0}-ORG-{1}-DCK-{2}-DPT-{3}-DCK-{4}.XML", rsw.Year.ToString(), regNum, num.ToString().PadLeft(5, '0'), "000000", "00000");

                xml_info = new xmlInfo
                {
                    Num = num,
                    CountDoc = 1,
                    DocType = "РСВ",
                    SourceID = rsw.ID,
                    Year = rsw.Year,
                    Quarter = rsw.Quarter,
                    DateCreate = DateUnderwrite.Value,
                    FileName = fileName,
                    UniqGUID = guid,
                    InsurerID = Options.InsID,
                    FormatType = "rsw2014"
                };

                dbxml.xmlInfo.Add(xml_info);
                dbxml.SaveChanges();
                parentID = xml_info.ID;

                fileName = "";

                packs_cnt_ALL++; 
            }

            updateProgressBar(1, 6);



            try
            {
                titleLabel.Invoke(new Action(() => { titleLabel.Text = "Выборка данных по индивидуальным сведениям"; }));

                switch (TypeInfoDDL.SelectedIndex)
                {
                    case 0:  // исходные сведения
                        rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == q && x.TypeInfoID == 1).ToList();
                        break;
                    case 1: // корректирующие
                        rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == q && x.TypeInfoID == 2).ToList();
                        break;
                    case 2: // отменяющие
                        rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == q && x.TypeInfoID == 3).ToList();
                        break;
                    case 3: // все сведения
                        rsw61List = db.FormsRSW2014_1_Razd_6_1.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == q).ToList();
                        break;
                }

                List<long> staffList = new List<long>();

                switch (switchStaffListDDL.SelectedIndex)
                {
                    case 0: //все записи
                        staffList = db.Staff.Where(x => x.InsurerID == Options.InsID).Select(x => x.ID).ToList();
                        break;
                    case 1: //текущая запись
                        staffList.Add(currentStaffId);
                        break;
                    case 2: //по выделенным записям
                        staffList = staffList_temp;
                        break;
                }

                rsw61List = rsw61List.Where(x => staffList.Contains(x.StaffID)).ToList();

                string sorting = "";

                switch (sortingDDL.SelectedIndex)
                {
                    case 0: //сортировка по ФИО
                        sorting = "Staff.LastName, Staff.FirstName, Staff.MiddleName";
                        break;
                    case 1: //по страх. номеру
                        sorting = "Staff.InsuranceNumber";
                        break;
                    case 2: //по табелю
                        sorting = "Staff.TabelNumber";
                        break;
                }

                //   rsw61List = rsw61List.Select(p => new { form = p, Staff = p.Staff }).ToList().OrderBy(sorting).Select(p => p.form).ToList();


                List<long> stajOsnSimpleIDTemp = new List<long>();
                List<long> stajOsnOsobIDTemp = new List<long>();
                List<stajOsnCont> stajOsnTemp = new List<stajOsnCont>();


                List<FormsRSW2014_1_Razd_6_1> rsw61temp1 = rsw61List.Where(x => x.TypeInfoID == 1).ToList();
                List<long> rsw61temp1_emptyStaj = new List<long>();
                List<FormsRSW2014_1_Razd_6_1> rsw61temp2 = new List<FormsRSW2014_1_Razd_6_1>();
                List<long> rsw61temp2_emptyStaj = new List<long>();
                List<FormsRSW2014_1_Razd_6_1> rsw61temp3 = new List<FormsRSW2014_1_Razd_6_1>();

                pu6Entities db_temp_0 = new pu6Entities();
                pu6Entities db_temp_1 = new pu6Entities();

                if (TypeInfoDDL.SelectedIndex == 0 || TypeInfoDDL.SelectedIndex == 3)
                {
                    //   IEnumerable<stajLgotCont> stajLgotOsobTemp = new List<stajLgotCont>();

                    List<long> IDtemp = rsw61temp1.Select(x => x.ID).ToList();


                    var stajOsnT_ = db_temp_0.StajOsn.Where(x => IDtemp.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value));

                    stajOsnTemp = stajOsnT_.Select(x => new stajOsnCont { ID = x.ID, FormsRSW2014_1_Razd_6_1_ID = x.FormsRSW2014_1_Razd_6_1_ID.Value }).ToList();
                    List<long> rsw61ID_WithStaj = stajOsnT_.Select(x => x.FormsRSW2014_1_Razd_6_1_ID.Value).ToList();
                    rsw61temp1_emptyStaj = IDtemp.Except(rsw61ID_WithStaj).ToList();

                    rsw61ID_WithStaj = new List<long>();
                    //          stajOsnT_.Clear();
                    //          stajOsnT_ = new List<StajOsn>();

                    List<long> stajOsnIDTemp = stajOsnTemp.Select(x => x.ID).ToList();

                    var stajLgotOsobT_ = db_temp_0.StajLgot.Where(x => stajOsnIDTemp.Contains(x.StajOsnID));
                    var stajLgotOsobTemp = stajLgotOsobT_.Select(x => new stajLgotCont { StajOsnID = x.StajOsnID, OsobUslTrudaID = x.OsobUslTrudaID, UslDosrNaznID = x.UslDosrNaznID, IschislStrahStajOsnID = x.IschislStrahStajOsnID });//.ToList()
                    stajLgotOsobTemp = stajLgotOsobTemp.Where(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0));//.ToList()

                    //     stajLgotOsobT_.Clear();

                    stajOsnOsobIDTemp = stajLgotOsobTemp.Select(x => x.StajOsnID).ToList();
                    //stajLgotOsobTemp.Clear();
                    //     stajLgotOsobTemp = new List<stajLgotCont>();

                    List<long> stajOsnOsobIDTempRSW61 = stajOsnTemp.Where(x => stajOsnOsobIDTemp.Contains(x.ID)).Select(x => x.FormsRSW2014_1_Razd_6_1_ID.Value).ToList().Distinct().ToList();
                    stajOsnOsobIDTempRSW61 = stajOsnTemp.Where(x => stajOsnOsobIDTempRSW61.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).Select(x => x.ID).ToList();

                    stajOsnSimpleIDTemp = stajOsnIDTemp.Except(stajOsnOsobIDTempRSW61).ToList();
                    stajOsnOsobIDTempRSW61 = new List<long>();

                    db_temp_0.Dispose();
                    db_temp_0 = new pu6Entities();
                }

                if (TypeInfoDDL.SelectedIndex == 1 || TypeInfoDDL.SelectedIndex == 3)
                {
                    rsw61temp2 = rsw61List.Where(x => x.TypeInfoID == 2).ToList();
                }

                // 5 вариантов различных пачек(исходные; исх + особ.усл,исчисл.страх.стаж,доср.пенс; отмен; корр; корр + особ.усл,исчисл.страх.стаж,доср.пенс)
                for (int i = 1; i <= 5; i++)
                {
                    List<long> temp = new List<long>();
                    List<FormsRSW2014_1_Razd_6_1> rsw61temp1_ = new List<FormsRSW2014_1_Razd_6_1>();
                    switch (i)
                    {
                        case 1: // исходные
                            if (TypeInfoDDL.SelectedIndex == 0 || TypeInfoDDL.SelectedIndex == 3)
                            {
                                temp.Clear();
                                temp = stajOsnTemp.Where(x => stajOsnSimpleIDTemp.Contains(x.ID)).ToList().Select(x => x.FormsRSW2014_1_Razd_6_1_ID.Value).ToList().Distinct().ToList();

                                rsw61temp1_ = rsw61temp1.Where(x => temp.Contains(x.ID) || rsw61temp1_emptyStaj.Contains(x.ID)).ToList();

                                rsw61temp1_ = rsw61temp1_.Select(p => new { form = p, Staff = p.Staff }).ToList().OrderBy(sorting).Select(p => p.form).ToList();

                                if (rsw61temp1_.Count > 0)
                                    rsw61List_part.Add(rsw61temp1_.ToList());

                                //rsw61temp1_.Clear();
                                //rsw61temp1_ = new List<FormsRSW2014_1_Razd_6_1>();
                                //temp.Clear();
                                //temp = new List<long>();
                            }
                            break;
                        #region
                        case 2: // исх + особ.усл,исчисл.страх.стаж,доср.пенс
                            if (TypeInfoDDL.SelectedIndex == 0 || TypeInfoDDL.SelectedIndex == 3)
                            {
                                temp = stajOsnTemp.Where(x => stajOsnOsobIDTemp.Contains(x.ID)).ToList().Select(x => x.FormsRSW2014_1_Razd_6_1_ID.Value).ToList().Distinct().ToList();
                                rsw61temp1_ = rsw61temp1.Where(x => temp.Contains(x.ID)).ToList();

                                rsw61temp1_ = rsw61temp1_.Select(p => new { form = p, Staff = p.Staff }).ToList().OrderBy(sorting).Select(p => p.form).ToList();

                                if (rsw61temp1_.Count > 0)
                                    rsw61List_part.Add(rsw61temp1_.ToList());

                                //rsw61temp1_.Clear();
                                //rsw61temp1_ = new List<FormsRSW2014_1_Razd_6_1>();
                                //temp.Clear();
                                //temp = new List<long>();

                                rsw61temp1.Clear();
                                rsw61temp1 = new List<FormsRSW2014_1_Razd_6_1>();
                                rsw61temp1_emptyStaj.Clear();
                                rsw61temp1_emptyStaj = new List<long>();
                                stajOsnOsobIDTemp.Clear();
                                stajOsnOsobIDTemp = new List<long>();
                                stajOsnSimpleIDTemp.Clear();
                                stajOsnSimpleIDTemp = new List<long>();


                            }
                            break;
                        case 3: // отмен
                            if (TypeInfoDDL.SelectedIndex == 2 || TypeInfoDDL.SelectedIndex == 3)
                            {
                                rsw61temp3 = rsw61List.Where(x => x.TypeInfoID == 3).ToList();

                                foreach (var per in korrPeriods)//.Where(x => x.Year >= 2014)
                                {
                                    if (per.Year >= 2014)
                                    {
                                        if (rsw61temp3.Any(x => x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal))
                                        {
                                            rsw61temp1_ = rsw61temp3.Where(x => x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal).ToList();
                                            rsw61List_part.Add(rsw61temp1_.Select(p => new { form = p, Staff = p.Staff }).OrderBy(sorting).Select(p => p.form).ToList());
                                        }
                                    }
                                    if (per.Year == 2013)
                                    {
                                        for (byte l = 1; l <= 2; l++)
                                        {
                                            if (db.FormsSZV_6_4.Any(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 3 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal && x.TypeContract == l))
                                            {
                                                var temp64 = db.FormsSZV_6_4.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == q && x.TypeInfoID == 3 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal && x.TypeContract == l).ToList();
                                                szv64List_part.Add(temp64.Select(p => new { form = p, Staff = p.Staff }).OrderBy(sorting).Select(p => p.form).ToList());
                                            }
                                        }
                                    }
                                    if (per.Year >= 2010 && per.Year <= 2012)
                                    {
                                        if (db.FormsSZV_6.Any(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 3 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal))
                                        {
                                            var temp6 = db.FormsSZV_6.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == q && x.TypeInfoID == 3 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal).ToList();

                                            //if (temp.Any(x => x.StajOsn == null || (x.StajOsn != null && x.StajOsn.Count == 0) || (x.StajOsn != null && x.StajOsn.Count() == 1 && (!x.StajOsn.Any(c => c.StajLgot.Any())))))
                                            //{
                                            //  var tempSzv62 = temp.Where(x => x.StajOsn == null || (x.StajOsn != null && x.StajOsn.Count == 0) || (x.StajOsn != null && x.StajOsn.Count() == 1 && (!x.StajOsn.Any(c => c.StajLgot.Any())))).ToList();
                                            szv62List_part.Add(temp6.Select(p => new { form = p, Staff = p.Staff }).ToList().OrderBy(sorting).Select(p => p.form).ToList());
                                            //}
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion
                        case 4: // корр
                            if (TypeInfoDDL.SelectedIndex == 1 || TypeInfoDDL.SelectedIndex == 3)
                            {
                                stajOsnSimpleIDTemp = new List<long>();
                                stajOsnOsobIDTemp = new List<long>();
                                stajOsnTemp = new List<stajOsnCont>();


                                //IEnumerable<stajLgotCont> stajLgotOsobTemp = new List<stajLgotCont>();

                                List<long> IDtemp = rsw61temp2.Select(x => x.ID).ToList();

                                var stajOsnT_ = db_temp_1.StajOsn.Where(x => IDtemp.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value));//.ToList()

                                stajOsnTemp = stajOsnT_.Select(x => new stajOsnCont { ID = x.ID, FormsRSW2014_1_Razd_6_1_ID = x.FormsRSW2014_1_Razd_6_1_ID.Value }).ToList();
                                var rsw61ID_WithStaj = stajOsnT_.Select(x => x.FormsRSW2014_1_Razd_6_1_ID.Value).ToList();
                                rsw61temp2_emptyStaj = IDtemp.Except(rsw61ID_WithStaj).ToList();

                                List<long> stajOsnIDTemp = stajOsnTemp.Select(x => x.ID).ToList();

                                var stajLgotOsobT_ = db_temp_1.StajLgot.Where(x => stajOsnIDTemp.Contains(x.StajOsnID));//.ToList()
                                var stajLgotOsobTemp = stajLgotOsobT_.Select(x => new stajLgotCont { StajOsnID = x.StajOsnID, OsobUslTrudaID = x.OsobUslTrudaID, UslDosrNaznID = x.UslDosrNaznID, IschislStrahStajOsnID = x.IschislStrahStajOsnID });//.ToList()
                                stajLgotOsobTemp = stajLgotOsobTemp.Where(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0));//.ToList()

                                stajOsnOsobIDTemp = stajLgotOsobTemp.Select(x => x.StajOsnID).ToList();

                                var stajOsnOsobIDTempRSW61 = stajOsnTemp.Where(x => stajOsnOsobIDTemp.Contains(x.ID)).Select(x => x.FormsRSW2014_1_Razd_6_1_ID).ToList().Distinct().ToList();
                                var stajOsnOsobIDTempRSW61_ = stajOsnTemp.Where(x => stajOsnOsobIDTempRSW61.Contains(x.FormsRSW2014_1_Razd_6_1_ID)).Select(x => x.ID).ToList();

                                stajOsnSimpleIDTemp = stajOsnIDTemp.Except(stajOsnOsobIDTempRSW61_).ToList();

                                var tempS = stajOsnTemp.Where(x => stajOsnSimpleIDTemp.Contains(x.ID)).ToList().Select(x => x.FormsRSW2014_1_Razd_6_1_ID.Value).ToList().Distinct().ToList();



                                foreach (var per in korrPeriods)
                                {
                                    if (per.Year >= 2014)
                                    {
                                        if (rsw61temp2.Any(x => x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal))
                                        {
                                            rsw61temp1_ = rsw61temp2.Where(x => x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal).ToList();

                                            rsw61temp1_ = rsw61temp1_.Where(x => tempS.Contains(x.ID) || rsw61temp2_emptyStaj.Contains(x.ID)).ToList();

                                            rsw61temp1_ = rsw61temp1_.Select(p => new { form = p, Staff = p.Staff }).ToList().OrderBy(sorting).Select(p => p.form).ToList();

                                            if (rsw61temp1_.Count > 0)
                                                rsw61List_part.Add(rsw61temp1_.ToList());

                                        }
                                    }
                                    if (per.Year == 2013)
                                    {
                                        if (db.FormsSZV_6_4.Any(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 2 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal))
                                        {
                                            var tmp = db.FormsSZV_6_4.Where(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 2 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal).ToList();
                                            tmp = tmp.Select(p => new { form = p, Staff = p.Staff }).OrderBy(sorting).Select(p => p.form).ToList();

                                            for (byte l = 1; l <= 2; l++)
                                            {
                                                if (tmp.Any(x => x.TypeContract == l && !x.StajOsn.Any(z => z.StajLgot.Any(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0)))))
                                                {
                                                    var tmp_ = tmp.Where(x => x.TypeContract == l && (!x.StajOsn.Any(z => z.StajLgot.Any(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0)) || x.StajOsn == null))).ToList();

                                                    var cats = tmp_.Select(x => x.PlatCategoryID).Distinct().ToList();

                                                    foreach (var cat in cats)
                                                    {
                                                        szv64List_part.Add(tmp_.Where(x => x.PlatCategoryID == cat).ToList());
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (per.Year >= 2010 && per.Year <= 2012)
                                    {
                                        if (db.FormsSZV_6.Any(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 2 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal))
                                        {
                                            var tmp = db.FormsSZV_6.Where(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 2 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal).ToList();
                                            //       tmp = tmp.Select(p => new { form = p, Staff = p.Staff }).OrderBy(sorting).Select(p => p.form).ToList();

                                            if (tmp.Any(x => !x.StajOsn.Any() || !x.StajOsn.Any(z => z.StajLgot.Any(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0)))))
                                            {
                                                tmp = tmp.Where(x => !x.StajOsn.Any() || !x.StajOsn.Any(z => z.StajLgot.Any(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0)))).ToList();

                                                List<long> cats = new List<long>();

                                                if (tmp.Any(x => !x.StajOsn.Any() || (x.StajOsn != null && x.StajOsn.Count() == 1 && (!x.StajOsn.Any(c => c.StajLgot.Any())))))
                                                {
                                                    var tempSzv62 = tmp.Where(x => !x.StajOsn.Any() || (x.StajOsn != null && x.StajOsn.Count() == 1 && (!x.StajOsn.Any(c => c.StajLgot.Any())))).ToList();
                                                    tempSzv62 = tempSzv62.Select(p => new { form = p, Staff = p.Staff }).OrderBy(sorting).Select(p => p.form).ToList();

                                                    cats = tempSzv62.Select(x => x.PlatCategoryID).Distinct().ToList();

                                                    foreach (var cat in cats)
                                                    {
                                                        szv62List_part.Add(tempSzv62.Where(x => x.PlatCategoryID == cat).ToList());
                                                    }
                                                    var t = tempSzv62.Select(c => c.ID).ToList();
                                                    tmp = tmp.Where(x => !t.Contains(x.ID)).ToList();
                                                }

                                                tmp = tmp.Select(p => new { form = p, Staff = p.Staff }).OrderBy(sorting).Select(p => p.form).ToList();
                                                cats = new List<long>();
                                                cats = tmp.Select(x => x.PlatCategoryID).Distinct().ToList();

                                                foreach (var cat in cats)
                                                {
                                                    szv61List_part.Add(tmp.Where(x => x.PlatCategoryID == cat).ToList());
                                                }
                                            }

                                        }
                                    }

                                }
                            }
                            break;
                        case 5: // корр + особ.усл,исчисл.страх.стаж,доср.пенс
                            if (TypeInfoDDL.SelectedIndex == 1 || TypeInfoDDL.SelectedIndex == 3)
                            {
                                var temp2 = stajOsnTemp.Where(x => stajOsnOsobIDTemp.Contains(x.ID)).ToList().Select(x => x.FormsRSW2014_1_Razd_6_1_ID.Value).ToList().Distinct().ToList();

                                foreach (var per in korrPeriods)
                                {
                                    if (per.Year >= 2014)
                                    {
                                        if (rsw61temp2.Any(x => x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal))
                                        {
                                            rsw61temp1_ = rsw61temp2.Where(x => x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal).ToList();

                                            rsw61temp1_ = rsw61temp1_.Where(x => temp2.Contains(x.ID)).ToList();

                                            rsw61temp1_ = rsw61temp1_.Select(p => new { form = p, Staff = p.Staff }).ToList().OrderBy(sorting).Select(p => p.form).ToList();

                                            if (rsw61temp1_.Count > 0)
                                                rsw61List_part.Add(rsw61temp1_.ToList());

                                        }
                                    }
                                    if (per.Year == 2013)
                                    {
                                        if (db.FormsSZV_6_4.Any(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 2 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal))
                                        {
                                            var tmp = db.FormsSZV_6_4.Where(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 2 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal).ToList();
                                            tmp = tmp.Select(p => new { form = p, Staff = p.Staff }).OrderBy(sorting).Select(p => p.form).ToList();
                                            for (byte l = 1; l <= 2; l++)
                                            {
                                                if (tmp.Any(x => x.TypeContract == l && x.StajOsn.Any(z => z.StajLgot.Any(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0)))))
                                                {
                                                    var tmp_ = tmp.Where(x => x.TypeContract == l && x.StajOsn.Any(z => z.StajLgot.Any(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0)))).ToList();

                                                    var cats = tmp_.Select(x => x.PlatCategoryID).Distinct().ToList();

                                                    foreach (var cat in cats)
                                                    {
                                                        szv64List_part.Add(tmp_.Where(x => x.PlatCategoryID == cat).ToList());
                                                    }

                                                }
                                            }
                                        }

                                    }
                                    if (per.Year >= 2010 && per.Year <= 2012)
                                    {
                                        if (db.FormsSZV_6.Any(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 2 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal))
                                        {
                                            var tmp = db.FormsSZV_6.Where(x => x.InsurerID == Options.InsID && staffList.Contains(x.StaffID) && x.Year == y && x.Quarter == q && x.TypeInfoID == 2 && x.YearKorr == per.Year && x.QuarterKorr == per.Kvartal).ToList();
                                            //                                            tmp = tmp.Select(p => new { form = p, Staff = p.Staff }).OrderBy(sorting).Select(p => p.form).ToList();

                                            if (tmp.Any(x => x.StajOsn.Any(z => z.StajLgot.Any(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0)))))
                                            {
                                                tmp = tmp.Where(x => x.StajOsn.Any(z => z.StajLgot.Any(c => (c.OsobUslTrudaID != null && c.OsobUslTrudaID != 0) || (c.IschislStrahStajOsnID != null && c.IschislStrahStajOsnID != 0) || (c.UslDosrNaznID != null && c.UslDosrNaznID != 0)))).ToList();
                                                tmp = tmp.Select(p => new { form = p, Staff = p.Staff }).OrderBy(sorting).Select(p => p.form).ToList();
                                                List<long> cats = new List<long>();
                                                cats = tmp.Select(x => x.PlatCategoryID).Distinct().ToList();

                                                foreach (var cat in cats)
                                                {
                                                    szv61List_part.Add(tmp.Where(x => x.PlatCategoryID == cat).ToList());
                                                }
                                            }
                                        }

                                    }
                                }


                                rsw61temp2.Clear();
                                rsw61temp2 = new List<FormsRSW2014_1_Razd_6_1>();
                                rsw61temp2_emptyStaj.Clear();
                                rsw61temp2_emptyStaj = new List<long>();
                                stajOsnOsobIDTemp.Clear();
                                stajOsnOsobIDTemp = new List<long>();
                                stajOsnSimpleIDTemp.Clear();
                                stajOsnSimpleIDTemp = new List<long>();

                                db_temp_1.Dispose();
                                db_temp_1 = new pu6Entities();
                            }
                            break;
                    }



                    updateProgressBar(i + 1, 6);
                    rsw61temp1_.Clear();
                    rsw61temp1_ = new List<FormsRSW2014_1_Razd_6_1>();
                    temp.Clear();
                    temp = new List<long>();
                }


                bw.ReportProgress(0, "0");
                titleLabel.Invoke(new Action(() => { titleLabel.Text = "Подготовка данных для формирования пачек"; }));


                foreach (var rsw61List_item in rsw61List_part)
                {
                    pfrXMLEntities dbxmlTemp = new pfrXMLEntities();

                    bw.ReportProgress(0, "0");
                    titleLabel.Invoke(new Action(() => { titleLabel.Text = "Подготовка данных РСВ ПФР для формирования пачек"; }));

                    double cnt = rsw61List_item.Count();
                    double packs_count_double = cnt / 200;
                    int packs_cnt = Convert.ToInt16(Math.Ceiling(packs_count_double));
                    int v = 0;
                    for (int j = 0; j < packs_cnt; j++)
                    {
                        List<FormsRSW2014_1_Razd_6_1> rsw61List_work = rsw61List_item.Skip(j * 200).Take(200).ToList();
                        num++;
                        fileName = String.Format("PFR-700-Y-{0}-ORG-{1}-DCK-{2}-DPT-{3}-DCK-{4}.XML", y.ToString(), regNum, num.ToString().PadLeft(5, '0'), "000000", "00000");

                        int c = rsw61List_work.Count();
                        xml_info = new xmlInfo
                        {
                            Num = num,
                            CountDoc = c,
                            CountStaff = c,
                            DocType = "ПФР",
                            Year = y,
                            Quarter = q,
                            DateCreate = DateUnderwrite.Value,
                            FileName = fileName,
                            UniqGUID = guid,
                            InsurerID = Options.InsID,
                            FormatType = "rsw2014"
                        };
                        if (parentID != 0)
                            xml_info.ParentID = parentID;


                        bool korrFlag = false;
                        if (rsw61List_work.First().YearKorr != null && rsw61List_work.First().YearKorr != 0)  // если отменяющая или корректирующая пачка, то пишем корр период
                        {
                            xml_info.YearKorr = rsw61List_work.First().YearKorr;
                            xml_info.QuarterKorr = rsw61List_work.First().QuarterKorr;
                            korrFlag = true;
                        }

                        //                        dbxml.AddToxmlInfo(xml_info);

                        rsw2014 rsw2014_ = new rsw2014
                        {
                            RSW_2_5_1_2 = 0,
                            RSW_2_5_1_3 = 0,
                            RSW_2_5_2_4 = 0,
                            RSW_2_5_2_5 = 0,
                            RSW_2_5_2_6 = 0,
                            //            xmlInfo_ID = xml_info.ID
                        };

                        //                    dbxml.AddTorsw2014(rsw2014_);
                        //       dbxml.SaveChanges();

                        int k = 0;
                        var _t = rsw61List_work.Select(x => x.StaffID).ToList();
                        pu6Entities dbTemp = new pu6Entities();
                        var tStaffList = dbTemp.Staff.Where(x => _t.Contains(x.ID));

                        List<StaffList> staffList_ = new List<StaffList>();


                        rsw2014_.RSW_2_5_1_3 = rsw61List_work.Where(x => x.SumFeePFR.HasValue).Sum(x => x.SumFeePFR.Value);

                        //                        rsw2014_.RSW_2_5_1_3 = rsw61List_work.Sum(x => Math.Round(x.SumFeePFR.Value, 2));
                        foreach (var rsw61Item in rsw61List_work)
                        {

                            k++;
                            var staffItem = tStaffList.First(x => x.ID == rsw61Item.StaffID);

                            string contrNum = "";
                            string InsuranceNumber = staffItem.InsuranceNumber;
                            if (staffItem.ControlNumber != null)
                            {
                                contrNum = staffItem.ControlNumber.HasValue ? staffItem.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                                InsuranceNumber = !String.IsNullOrEmpty(staffItem.InsuranceNumber) ? staffItem.InsuranceNumber.Substring(0, 3) + "-" + staffItem.InsuranceNumber.Substring(3, 3) + "-" + staffItem.InsuranceNumber.Substring(6, 3) + " " + contrNum : "";
                            }

                            //if (rsw61Item.TypeInfoID == 1) // исходные данные
                            // {
                            foreach (var item in rsw61Item.FormsRSW2014_1_Razd_6_4)
                            {
                                rsw2014_.RSW_2_5_1_2 = rsw2014_.RSW_2_5_1_2.Value + (item.s_1_1 != null ? item.s_1_1.Value : 0);
                                rsw2014_.RSW_2_5_1_2 = rsw2014_.RSW_2_5_1_2.Value + (item.s_2_1 != null ? item.s_2_1.Value : 0);
                                rsw2014_.RSW_2_5_1_2 = rsw2014_.RSW_2_5_1_2.Value + (item.s_3_1 != null ? item.s_3_1.Value : 0);
                            }
                            //                            rsw2014_.RSW_2_5_1_3 = rsw2014_.RSW_2_5_1_3 + (rsw61Item.SumFeePFR != null ? rsw61Item.SumFeePFR : 0);


                            //}

                            //                        if (rsw61Item.TypeInfoID == 2) // корр данные
                            //                        {


                            //foreach (var item in rsw61Item.FormsRSW2014_1_Razd_6_6)
                            //{
                            //    xml_info.rsw2014.First().RSW_2_5_2_4 = xml_info.rsw2014.First().RSW_2_5_2_4.Value + (item.SumFeePFR_D != null ? item.SumFeePFR_D.Value : 0);
                            //    xml_info.rsw2014.First().RSW_2_5_2_5 = xml_info.rsw2014.First().RSW_2_5_2_5.Value + (item.SumFeePFR_StrahD != null ? item.SumFeePFR_StrahD.Value : 0);
                            //    xml_info.rsw2014.First().RSW_2_5_2_6 = xml_info.rsw2014.First().RSW_2_5_2_6.Value + (item.SumFeePFR_NakopD != null ? item.SumFeePFR_NakopD.Value : 0);
                            //}


                            //                        }

                            if (korrFlag)
                            {
                                if (rsw61List.Any(x => x.TypeInfoID == 1 && x.StaffID == rsw61Item.StaffID))
                                {
                                    var rsw61Item_ishod = rsw61List.First(x => x.TypeInfoID == 1 && x.StaffID == rsw61Item.StaffID);
                                    if (rsw61Item_ishod.FormsRSW2014_1_Razd_6_6.Any(x => x.AccountPeriodYear == rsw61Item.YearKorr && x.AccountPeriodQuarter == rsw61Item.QuarterKorr))
                                    {
                                        var item = rsw61Item_ishod.FormsRSW2014_1_Razd_6_6.First(x => x.AccountPeriodYear == rsw61Item.YearKorr && x.AccountPeriodQuarter == rsw61Item.QuarterKorr);
                                        rsw2014_.RSW_2_5_2_4 = rsw2014_.RSW_2_5_2_4.Value + (item.SumFeePFR_D != null ? item.SumFeePFR_D.Value : 0);
                                        rsw2014_.RSW_2_5_2_5 = rsw2014_.RSW_2_5_2_5.Value + (item.SumFeePFR_StrahD != null ? item.SumFeePFR_StrahD.Value : 0);
                                        rsw2014_.RSW_2_5_2_6 = rsw2014_.RSW_2_5_2_6.Value + (item.SumFeePFR_NakopD != null ? item.SumFeePFR_NakopD.Value : 0);
                                    }
                                }
                                else
                                {
                                    rsw2014_.RSW_2_5_2_4 = rsw2014_.RSW_2_5_2_4.Value + 0; //(rsw61Item.SumFeePFR != null ? rsw61Item.SumFeePFR.Value : 0)
                                }
                            }


                            StaffList staffListNewItem = new StaffList
                            {
                                Num = k,
                                FIO = staffItem.LastName + " " + staffItem.FirstName + " " + staffItem.MiddleName,
                                StaffID = staffItem.ID,
                                InsuranceNum = InsuranceNumber,
                                InfoType = infoTypeStr[rsw61Item.TypeInfoID - 1],
                                DateCreate = DateUnderwrite.Value,
                                //                                XmlInfoID = xml_info.ID,
                                FormsRSW_6_1_ID = rsw61Item.ID
                            };
                            staffList_.Add(staffListNewItem);
                            //                          dbxml.Staff.AddList(staffListNewItem);


                        }
                        dbTemp.Dispose();
                        xml_info.rsw2014.Add(rsw2014_);
                        foreach (var item in staffList_)
                        {
                            xml_info.StaffList.Add(item);
                        }
                        dbxmlTemp.xmlInfo.Add(xml_info);
                        //       dbxml.ObjectStateManager.ChangeObjectState(xml_info, EntityState.Modified);
                        dbxmlTemp.SaveChanges();

                        v++;
                        updateProgressBar(v, packs_cnt);
                    }

                    packs_cnt_ALL += packs_cnt;

                    dbxmlTemp.Dispose();

                }


                foreach (var szv64List_item in szv64List_part)
                {
                    bw.ReportProgress(0, "0");
                    titleLabel.Invoke(new Action(() => { titleLabel.Text = "Подготовка данных СЗВ-6-4 для формирования пачек"; }));


                    double cnt = szv64List_item.Count();
                    double packs_count_double = cnt / 200;
                    int packs_cnt = Convert.ToInt16(Math.Ceiling(packs_count_double));
                    int v = 0;

                    for (int j = 0; j < packs_cnt; j++)
                    {
                        List<FormsSZV_6_4> szv64List_work = szv64List_item.Skip(j * 200).Take(200).ToList();
                        num++;
                        fileName = String.Format("PFR-700-Y-{0}-ORG-{1}-DCK-{2}-DPT-{3}-DCK-{4}.XML", y.ToString(), regNum, num.ToString().PadLeft(5, '0'), "000000", "00000");

                        xml_info = new xmlInfo
                        {
                            Num = num,
                            CountDoc = szv64List_work.Count(),
                            CountStaff = szv64List_work.Count(),
                            DocType = "СЗВ-6-4",
                            Year = y,
                            Quarter = q,
                            DateCreate = DateUnderwrite.Value,
                            FileName = fileName,
                            UniqGUID = guid,
                            InsurerID = Options.InsID,
                            FormatType = "rsw2014"
                        };
                        if (parentID != 0)
                            xml_info.ParentID = parentID;


                        bool korrFlag = false;
                        if (szv64List_work.First().YearKorr != null && szv64List_work.First().YearKorr != 0)  // если отменяющая или корректирующая пачка, то пишем корр период
                        {
                            xml_info.YearKorr = szv64List_work.First().YearKorr;
                            xml_info.QuarterKorr = szv64List_work.First().QuarterKorr;
                            korrFlag = true;
                        }

                        dbxml.xmlInfo.Add(xml_info);
                        dbxml.SaveChanges();

                        rsw2014 rsw2014_ = new rsw2014
                        {
                            RSW_2_5_1_2 = 0,
                            RSW_2_5_1_3 = 0,
                            RSW_2_5_2_4 = 0,
                            RSW_2_5_2_5 = 0,
                            RSW_2_5_2_6 = 0,
                            xmlInfo_ID = xml_info.ID
                        };

                        dbxml.rsw2014.Add(rsw2014_);
                        dbxml.SaveChanges();

                        int k = 0;
                        foreach (var szv64Item in szv64List_work)
                        {
                            k++;
                            var staffItem = szv64Item.Staff;

                            string contrNum = "";
                            string InsuranceNumber = staffItem.InsuranceNumber;
                            if (staffItem.ControlNumber != null)
                            {
                                contrNum = staffItem.ControlNumber.HasValue ? staffItem.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                                InsuranceNumber = !String.IsNullOrEmpty(staffItem.InsuranceNumber) ? staffItem.InsuranceNumber.Substring(0, 3) + "-" + staffItem.InsuranceNumber.Substring(3, 3) + "-" + staffItem.InsuranceNumber.Substring(6, 3) + " " + contrNum : "";

                            }


                            xml_info.rsw2014.First().RSW_2_5_1_2 = xml_info.rsw2014.First().RSW_2_5_1_2.Value + (szv64Item.s_1_1 != null ? szv64Item.s_1_1.Value : 0);
                            xml_info.rsw2014.First().RSW_2_5_1_2 = xml_info.rsw2014.First().RSW_2_5_1_2.Value + (szv64Item.s_2_1 != null ? szv64Item.s_2_1.Value : 0);
                            xml_info.rsw2014.First().RSW_2_5_1_2 = xml_info.rsw2014.First().RSW_2_5_1_2.Value + (szv64Item.s_3_1 != null ? szv64Item.s_3_1.Value : 0);

                            xml_info.rsw2014.First().RSW_2_5_1_3 = xml_info.rsw2014.First().RSW_2_5_1_3 + (szv64Item.SumFeePFR_Strah.HasValue ? szv64Item.SumFeePFR_Strah.Value : 0);

                            if (korrFlag)
                            {
                                if (rsw61List.Any(x => x.TypeInfoID == 1 && x.StaffID == szv64Item.StaffID))
                                {
                                    var rsw61Item_ishod = rsw61List.FirstOrDefault(x => x.TypeInfoID == 1 && x.StaffID == szv64Item.StaffID);
                                    if (rsw61Item_ishod.FormsRSW2014_1_Razd_6_6.Any(x => x.AccountPeriodYear == szv64Item.YearKorr && x.AccountPeriodQuarter == szv64Item.QuarterKorr))
                                    {
                                        var item = rsw61Item_ishod.FormsRSW2014_1_Razd_6_6.FirstOrDefault(x => x.AccountPeriodYear == szv64Item.YearKorr && x.AccountPeriodQuarter == szv64Item.QuarterKorr);
                                        xml_info.rsw2014.First().RSW_2_5_2_4 = xml_info.rsw2014.First().RSW_2_5_2_4.Value + (item.SumFeePFR_D != null ? item.SumFeePFR_D.Value : 0);
                                        xml_info.rsw2014.First().RSW_2_5_2_5 = xml_info.rsw2014.First().RSW_2_5_2_5.Value + (item.SumFeePFR_StrahD != null ? item.SumFeePFR_StrahD.Value : 0);
                                        xml_info.rsw2014.First().RSW_2_5_2_6 = xml_info.rsw2014.First().RSW_2_5_2_6.Value + (item.SumFeePFR_NakopD != null ? item.SumFeePFR_NakopD.Value : 0);
                                    }
                                }
                                else
                                {
                                    xml_info.rsw2014.First().RSW_2_5_2_5 = xml_info.rsw2014.First().RSW_2_5_2_5.Value + (szv64Item.SumFeePFR_Strah_D != null ? szv64Item.SumFeePFR_Strah_D.Value : 0);
                                    xml_info.rsw2014.First().RSW_2_5_2_6 = xml_info.rsw2014.First().RSW_2_5_2_6.Value + (szv64Item.SumFeePFR_Nakop_D != null ? szv64Item.SumFeePFR_Nakop_D.Value : 0);

                                }

                            }


                            StaffList staffListNewItem = new StaffList
                            {
                                Num = k,
                                FIO = staffItem.LastName + " " + staffItem.FirstName + " " + staffItem.MiddleName,
                                StaffID = staffItem.ID,
                                InsuranceNum = InsuranceNumber,
                                InfoType = infoTypeStr[szv64Item.TypeInfoID - 1],
                                DateCreate = DateUnderwrite.Value,
                                XmlInfoID = xml_info.ID,
                                FormsRSW_6_1_ID = szv64Item.ID
                            };

                            dbxml.StaffList.Add(staffListNewItem);


                        }

                        dbxml.Entry(xml_info).State =EntityState.Modified;
                        dbxml.SaveChanges();
                        v++;
                        updateProgressBar(v, packs_cnt);

                    }
                    packs_cnt_ALL += packs_cnt;


                }

                foreach (var szv61List_item in szv61List_part)
                {
                    bw.ReportProgress(0, "0");
                    titleLabel.Invoke(new Action(() => { titleLabel.Text = "Подготовка данных СЗВ-6-1 для формирования пачек"; }));

                    double cnt = szv61List_item.Count();
                    double packs_count_double = cnt / 200;
                    int packs_cnt = Convert.ToInt16(Math.Ceiling(packs_count_double));
                    int v = 0;

                    for (int j = 0; j < packs_cnt; j++)
                    {
                        List<FormsSZV_6> szv61List_work = szv61List_item.Skip(j * 200).Take(200).ToList();
                        num++;
                        fileName = String.Format("PFR-700-Y-{0}-ORG-{1}-DCK-{2}-DPT-{3}-DCK-{4}.XML", y.ToString(), regNum, num.ToString().PadLeft(5, '0'), "000000", "00000");

                        xml_info = new xmlInfo
                        {
                            Num = num,
                            CountDoc = szv61List_work.Count(),
                            CountStaff = szv61List_work.Count(),
                            DocType = "СЗВ-6-1",
                            Year = y,
                            Quarter = q,
                            DateCreate = DateUnderwrite.Value,
                            FileName = fileName,
                            UniqGUID = guid,
                            InsurerID = Options.InsID,
                            FormatType = "rsw2014"
                        };
                        if (parentID != 0)
                            xml_info.ParentID = parentID;


                        bool korrFlag = false;
                        if (szv61List_work.First().YearKorr != null && szv61List_work.First().YearKorr != 0)  // если отменяющая или корректирующая пачка, то пишем корр период
                        {
                            xml_info.YearKorr = szv61List_work.First().YearKorr;
                            xml_info.QuarterKorr = szv61List_work.First().QuarterKorr;
                            korrFlag = true;
                        }

                        dbxml.xmlInfo.Add(xml_info);
                        dbxml.SaveChanges();

                        rsw2014 rsw2014_ = new rsw2014
                        {
                            RSW_2_5_1_2 = 0,
                            RSW_2_5_1_3 = 0,
                            RSW_2_5_2_4 = 0,
                            RSW_2_5_2_5 = 0,
                            RSW_2_5_2_6 = 0,
                            xmlInfo_ID = xml_info.ID
                        };

                        dbxml.rsw2014.Add(rsw2014_);
                        dbxml.SaveChanges();

                        int k = 0;
                        foreach (var szv61Item in szv61List_work)
                        {
                            k++;
                            var staffItem = szv61Item.Staff;

                            string contrNum = "";
                            string InsuranceNumber = staffItem.InsuranceNumber;
                            if (staffItem.ControlNumber != null)
                            {
                                contrNum = staffItem.ControlNumber.HasValue ? staffItem.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                                InsuranceNumber = !String.IsNullOrEmpty(staffItem.InsuranceNumber) ? staffItem.InsuranceNumber.Substring(0, 3) + "-" + staffItem.InsuranceNumber.Substring(3, 3) + "-" + staffItem.InsuranceNumber.Substring(6, 3) + " " + contrNum : "";

                            }

                            for (int a = 1; a <= 12; a++)
                            {
                                string name = "s_" + a.ToString() + "_0";
                                xml_info.rsw2014.First().RSW_2_5_1_2 = xml_info.rsw2014.First().RSW_2_5_1_2.Value + (szv61Item.GetType().GetProperty(name).GetValue(szv61Item, null) != null ? (decimal)szv61Item.GetType().GetProperty(name).GetValue(szv61Item, null) : 0);
                            }

                            xml_info.rsw2014.First().RSW_2_5_1_3 = xml_info.rsw2014.First().RSW_2_5_1_3 + (szv61Item.SumFeePFR_Strah.HasValue ? szv61Item.SumFeePFR_Strah.Value : 0);

                            if (korrFlag)
                            {
                                if (rsw61List.Any(x => x.TypeInfoID == 1 && x.StaffID == szv61Item.StaffID))
                                {

                                    var rsw61Item_ishod = rsw61List.FirstOrDefault(x => x.TypeInfoID == 1 && x.StaffID == szv61Item.StaffID);
                                    if (rsw61Item_ishod.FormsRSW2014_1_Razd_6_6.Any(x => x.AccountPeriodYear == szv61Item.YearKorr && x.AccountPeriodQuarter == szv61Item.QuarterKorr))
                                    {
                                        var item = rsw61Item_ishod.FormsRSW2014_1_Razd_6_6.FirstOrDefault(x => x.AccountPeriodYear == szv61Item.YearKorr && x.AccountPeriodQuarter == szv61Item.QuarterKorr);
                                        xml_info.rsw2014.First().RSW_2_5_2_4 = xml_info.rsw2014.First().RSW_2_5_2_4.Value + (item.SumFeePFR_D != null ? item.SumFeePFR_D.Value : 0);
                                        xml_info.rsw2014.First().RSW_2_5_2_5 = xml_info.rsw2014.First().RSW_2_5_2_5.Value + (item.SumFeePFR_StrahD != null ? item.SumFeePFR_StrahD.Value : 0);
                                        xml_info.rsw2014.First().RSW_2_5_2_6 = xml_info.rsw2014.First().RSW_2_5_2_6.Value + (item.SumFeePFR_NakopD != null ? item.SumFeePFR_NakopD.Value : 0);
                                    }
                                }
                                else
                                {
                                    xml_info.rsw2014.First().RSW_2_5_2_5 = xml_info.rsw2014.First().RSW_2_5_2_5.Value + (szv61Item.SumFeePFR_Strah_D != null ? szv61Item.SumFeePFR_Strah_D.Value : 0);
                                    xml_info.rsw2014.First().RSW_2_5_2_6 = xml_info.rsw2014.First().RSW_2_5_2_6.Value + (szv61Item.SumFeePFR_Nakop_D != null ? szv61Item.SumFeePFR_Nakop_D.Value : 0);

                                }

                            }

                            StaffList staffListNewItem = new StaffList
                            {
                                Num = k,
                                FIO = staffItem.LastName + " " + staffItem.FirstName + " " + staffItem.MiddleName,
                                StaffID = staffItem.ID,
                                InsuranceNum = InsuranceNumber,
                                InfoType = infoTypeStr[szv61Item.TypeInfoID - 1],
                                DateCreate = DateUnderwrite.Value,
                                XmlInfoID = xml_info.ID,
                                FormsRSW_6_1_ID = szv61Item.ID
                            };

                            dbxml.StaffList.Add(staffListNewItem);


                        }

                        dbxml.Entry(xml_info).State = EntityState.Modified;
                        dbxml.SaveChanges();

                        v++;
                        updateProgressBar(v, packs_cnt);
                    }

                    packs_cnt_ALL += packs_cnt;


                }


                foreach (var szv62List_item in szv62List_part)
                {
                    bw.ReportProgress(0, "0");
                    titleLabel.Invoke(new Action(() => { titleLabel.Text = "Подготовка данных СЗВ-6-1 для формирования пачек"; }));

                    double cnt = szv62List_item.Count();
                    double packs_count_double = cnt / 200;
                    int packs_cnt = Convert.ToInt16(Math.Ceiling(packs_count_double));

                    int v = 0;
                    for (int j = 0; j < packs_cnt; j++)
                    {
                        List<FormsSZV_6> szv62List_work = szv62List_item.Skip(j * 200).Take(200).ToList();
                        num++;
                        fileName = String.Format("PFR-700-Y-{0}-ORG-{1}-DCK-{2}-DPT-{3}-DCK-{4}.XML", y.ToString(), regNum, num.ToString().PadLeft(5, '0'), "000000", "00000");

                        xml_info = new xmlInfo
                        {
                            Num = num,
                            CountDoc = szv62List_work.Count(),
                            CountStaff = szv62List_work.Count(),
                            DocType = "СЗВ-6-2",
                            Year = y,
                            Quarter = q,
                            DateCreate = DateUnderwrite.Value,
                            FileName = fileName,
                            UniqGUID = guid,
                            InsurerID = Options.InsID,
                            FormatType = "rsw2014"
                        };
                        if (parentID != 0)
                            xml_info.ParentID = parentID;


                        bool korrFlag = false;
                        if (szv62List_work.First().YearKorr != null && szv62List_work.First().YearKorr != 0)  // если отменяющая или корректирующая пачка, то пишем корр период
                        {
                            xml_info.YearKorr = szv62List_work.First().YearKorr;
                            xml_info.QuarterKorr = szv62List_work.First().QuarterKorr;
                            korrFlag = true;
                        }

                        dbxml.xmlInfo.Add(xml_info);
                        dbxml.SaveChanges();

                        rsw2014 rsw2014_ = new rsw2014
                        {
                            RSW_2_5_1_2 = 0,
                            RSW_2_5_1_3 = 0,
                            RSW_2_5_2_4 = 0,
                            RSW_2_5_2_5 = 0,
                            RSW_2_5_2_6 = 0,
                            xmlInfo_ID = xml_info.ID
                        };

                        dbxml.rsw2014.Add(rsw2014_);
                        dbxml.SaveChanges();

                        int k = 0;
                        foreach (var szv62Item in szv62List_work)
                        {
                            k++;
                            var staffItem = szv62Item.Staff;

                            string contrNum = "";
                            string InsuranceNumber = staffItem.InsuranceNumber;
                            if (staffItem.ControlNumber != null)
                            {
                                contrNum = staffItem.ControlNumber.HasValue ? staffItem.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                                InsuranceNumber = !String.IsNullOrEmpty(staffItem.InsuranceNumber) ? staffItem.InsuranceNumber.Substring(0, 3) + "-" + staffItem.InsuranceNumber.Substring(3, 3) + "-" + staffItem.InsuranceNumber.Substring(6, 3) + " " + contrNum : "";

                            }

                            for (int a = 1; a <= 12; a++)
                            {
                                string name = "s_" + a.ToString() + "_0";
                                xml_info.rsw2014.First().RSW_2_5_1_2 = xml_info.rsw2014.First().RSW_2_5_1_2.Value + (szv62Item.GetType().GetProperty(name).GetValue(szv62Item, null) != null ? (decimal)szv62Item.GetType().GetProperty(name).GetValue(szv62Item, null) : 0);
                            }

                            xml_info.rsw2014.First().RSW_2_5_1_3 = xml_info.rsw2014.First().RSW_2_5_1_3 + (szv62Item.SumFeePFR_Strah.HasValue ? szv62Item.SumFeePFR_Strah.Value : 0);

                            if (korrFlag)
                            {
                                if (rsw61List.Any(x => x.TypeInfoID == 1 && x.StaffID == szv62Item.StaffID))
                                {

                                    var rsw61Item_ishod = rsw61List.FirstOrDefault(x => x.TypeInfoID == 1 && x.StaffID == szv62Item.StaffID);
                                    if (rsw61Item_ishod.FormsRSW2014_1_Razd_6_6.Any(x => x.AccountPeriodYear == szv62Item.YearKorr && x.AccountPeriodQuarter == szv62Item.QuarterKorr))
                                    {
                                        var item = rsw61Item_ishod.FormsRSW2014_1_Razd_6_6.FirstOrDefault(x => x.AccountPeriodYear == szv62Item.YearKorr && x.AccountPeriodQuarter == szv62Item.QuarterKorr);
                                        xml_info.rsw2014.First().RSW_2_5_2_4 = xml_info.rsw2014.First().RSW_2_5_2_4.Value + (item.SumFeePFR_D != null ? item.SumFeePFR_D.Value : 0);
                                        xml_info.rsw2014.First().RSW_2_5_2_5 = xml_info.rsw2014.First().RSW_2_5_2_5.Value + (item.SumFeePFR_StrahD != null ? item.SumFeePFR_StrahD.Value : 0);
                                        xml_info.rsw2014.First().RSW_2_5_2_6 = xml_info.rsw2014.First().RSW_2_5_2_6.Value + (item.SumFeePFR_NakopD != null ? item.SumFeePFR_NakopD.Value : 0);
                                    }
                                }
                                else
                                {
                                    xml_info.rsw2014.First().RSW_2_5_2_5 = xml_info.rsw2014.First().RSW_2_5_2_5.Value + (szv62Item.SumFeePFR_Strah_D != null ? szv62Item.SumFeePFR_Strah_D.Value : 0);
                                    xml_info.rsw2014.First().RSW_2_5_2_6 = xml_info.rsw2014.First().RSW_2_5_2_6.Value + (szv62Item.SumFeePFR_Nakop_D != null ? szv62Item.SumFeePFR_Nakop_D.Value : 0);

                                }

                            }

                            StaffList staffListNewItem = new StaffList
                            {
                                Num = k,
                                FIO = staffItem.LastName + " " + staffItem.FirstName + " " + staffItem.MiddleName,
                                StaffID = staffItem.ID,
                                InsuranceNum = InsuranceNumber,
                                InfoType = infoTypeStr[szv62Item.TypeInfoID - 1],
                                DateCreate = DateUnderwrite.Value,
                                XmlInfoID = xml_info.ID,
                                FormsRSW_6_1_ID = szv62Item.ID
                            };

                            dbxml.StaffList.Add(staffListNewItem);


                        }

                        dbxml.Entry(xml_info).State = EntityState.Modified;
                        dbxml.SaveChanges();

                        v++;
                        updateProgressBar(v, packs_cnt);
                    }
                    packs_cnt_ALL += packs_cnt;

                }


                if (dbxml.xmlInfo.Any(x => x.UniqGUID == guid && x.DocType == "РСВ"))
                {
                    bw.ReportProgress(0, "0");
                    titleLabel.Invoke(new Action(() => { titleLabel.Text = "Подготовка данных РСВ-1 для формирования пачек"; }));

                    var xmlRec = dbxml.xmlInfo.First(x => x.UniqGUID == guid && x.DocType == "РСВ");
                    var rsw_source = db.FormsRSW2014_1_1.First(x => x.ID == xmlRec.SourceID);

                    bool flag = false;
                    foreach (var item in db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.InsurerID == rsw_source.InsurerID && x.Year == rsw_source.Year && x.Quarter == rsw_source.Quarter && x.CorrectionNum == rsw_source.CorrectionNum))
                    {
                        db.FormsRSW2014_1_Razd_2_5_1.Remove(item);
                        flag = true;
                    }
                    foreach (var item in db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.InsurerID == rsw_source.InsurerID && x.Year == rsw_source.Year && x.Quarter == rsw_source.Quarter && x.CorrectionNum == rsw_source.CorrectionNum))
                    {
                        db.FormsRSW2014_1_Razd_2_5_2.Remove(item);
                        flag = true;
                    }

                    if (flag)
                        db.SaveChanges();

                    int i_num = 0;
                    int j_num = 0;
                    flag = false;
                    foreach (var item in dbxml.xmlInfo.Where(x => x.UniqGUID == guid && (x.DocType == "ПФР" || x.DocType == "СЗВ-6-4" || x.DocType == "СЗВ-6-1" || x.DocType == "СЗВ-6-2")))
                    {
                        flag = true;
                        if (item.YearKorr == null)
                        {
                            i_num++;

                            FormsRSW2014_1_Razd_2_5_1 rsw251 = new FormsRSW2014_1_Razd_2_5_1
                            {
                                InsurerID = rsw_source.InsurerID,
                                Year = rsw_source.Year,
                                Quarter = rsw_source.Quarter,
                                CorrectionNum = rsw_source.CorrectionNum,
                                NumRec = i_num,
                                Col_2 = item.rsw2014.First().RSW_2_5_1_2,
                                Col_3 = item.rsw2014.First().RSW_2_5_1_3,
                                Col_4 = item.CountStaff,
                                Col_5 = item.FileName
                            };

                            db.FormsRSW2014_1_Razd_2_5_1.Add(rsw251);

                        }

                        if (item.YearKorr != null)
                        {
                            j_num++;

                            FormsRSW2014_1_Razd_2_5_2 rsw252 = new FormsRSW2014_1_Razd_2_5_2
                            {
                                InsurerID = rsw_source.InsurerID,
                                Year = rsw_source.Year,
                                Quarter = rsw_source.Quarter,
                                CorrectionNum = rsw_source.CorrectionNum,
                                NumRec = j_num,
                                Col_2_QuarterKorr = item.QuarterKorr.Value,
                                Col_3_YearKorr = item.YearKorr.Value,
                                Col_4 = item.rsw2014.First().RSW_2_5_2_4,
                                Col_5 = item.rsw2014.First().RSW_2_5_2_5,
                                Col_6 = item.rsw2014.First().RSW_2_5_2_6,
                                Col_7 = item.CountStaff,
                                Col_8 = item.FileName
                            };

                            db.FormsRSW2014_1_Razd_2_5_2.Add(rsw252);


                        }

                    }

                    if (flag)
                    {
                        db.SaveChanges();
                    }


                    updateProgressBar(1, 1);
                }

            }
            catch (Exception ex)
            {
                bwCancel("Ошибка", "Во время формирования пачек произошла ошибка! Код ошибки: " + ex.Message);

            }

            dbxml.Dispose();
            dbxml = new pfrXMLEntities();


            numFrom.Invoke(new Action(() => { numFrom.Value = packs_cnt_ALL; }));

            //int dbxmlCnt = dbxml.xmlInfo.Where(x => x.UniqGUID == guid).Count();
            //bw.ReportProgress(0, dbxmlCnt.ToString());
            //titleLabel.Invoke(new Action(() => { titleLabel.Text = "Формирование пачек XML"; }));

            //int u = 0;
            //List<xmlInfo> t_ = dbxml.xmlInfo.Where(x => x.UniqGUID == guid).ToList();

            //List<StaffList> staffTempList = t_.SelectMany(x => x.StaffList.ToList()).ToList();
            //List<long> t__ = staffTempList.Select(x => x.StaffID.Value).ToList();
            //StaffList_All_Ishod = db.Staff.Where(x => t__.Contains(x.ID)).ToList();

            //foreach (var item in t_)
            //{


            //    if (bw.CancellationPending)
            //    {
            //        return;
            //    }
            //    string xml = "";

            //    try
            //    {
            //        db.Dispose();
            //        db = new pu6Entities();
            //        switch (item.DocType)
            //        {
            //            case "РСВ":
            //                xml = generateXML_RSW2014(item);
            //                break;
            //            case "ПФР":
            //                xml = generateXML_RSW2014_6(item);
            //                break;
            //            case "СЗВ-6-4":
            //                xml = generateXML_SZV_6_4(item);
            //                break;
            //            case "СЗВ-6-1":
            //                xml = generateXML_SZV_6_1(item);
            //                break;
            //            case "СЗВ-6-2":
            //                xml = generateXML_SZV_6_1(item);
            //                break;

            //        }
            //        //                  dbxml = new pfrXMLEntities();
            //        xmlFile xmlFile_ = new xmlFile
            //        {
            //            XmlContent = xml,
            //            XmlInfoID = item.ID
            //        };

            //        pfrXMLEntities dbxml_temp = new pfrXMLEntities();
            //        dbxml_temp.AddToxmlFile(xmlFile_);
            //        dbxml_temp.SaveChanges();
            //        //                    dbxml.Dispose();
            //        u++;
            //        dbxml_temp.Dispose();

            //        updateProgressBar(u, dbxmlCnt);
            //    }
            //    catch (Exception ex)
            //    {
            //        bwCancel("Ошибка", "Во время формирования пачек произошла ошибка! Код ошибки: " + ex.Message);

            //    }



            //}
            //staffTempList = new List<StaffList>();
            //StaffList_All_Ishod = new List<Staff>();
            //t__ = new List<long>();
            //t_ = new List<xmlInfo>();
            //dbxml.Dispose();
            //dbxml = new pfrXMLEntities();
            db.Dispose();
            db = new pu6Entities();
        }

        private string generateXML_SZV_6_1(xmlInfo item)
        {
            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion.Substring(2, Application.ProductVersion.Length - 2))),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ = new XElement("ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ",
                                                                  new XElement("НомерВпачке", 1),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            //if (!String.IsNullOrEmpty(ins.EGRIP))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            //}

            //if (!String.IsNullOrEmpty(ins.EGRUL))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            //}

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length).ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length).ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));


            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(СоставительПачки);
            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ТипСведений", InfoType));

            var t = item.StaffList.Select(x => x.StaffID.Value).ToList();
            var StaffList = db.Staff.Where(x => t.Contains(x.ID));

            var t2 = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();
            var temp_list = db.FormsSZV_6.Where(x => t2.Contains(x.ID));

            FormsSZV_6 szv_6_t = temp_list.First();

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КодКатегории", szv_6_t.PlatCategory.Code));

            //            string periodName = "";
            //           string periodNameKorr = "";


            //            RaschetPeriodContainer rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.Quarter && x.Year == item.Year);
            //            if (rp != null)
            //                periodName = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ОтчетныйПериод",
                                                           new XElement("Квартал", item.Quarter.Value),
                                                           new XElement("Год", item.Year.Value)));

            if (item.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
            {
                //                rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.QuarterKorr && x.Year == item.YearKorr);
                //                if (rp != null)
                //                    periodNameKorr = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();

                ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                               new XElement("Квартал", item.QuarterKorr.Value),
                                                               new XElement("Год", item.YearKorr.Value)));
            }


            szv_6_t = null;



            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                           new XElement("Начислено", Utils.decToStr(temp_list.Where(x => x.SumFeePFR_Strah.HasValue).Sum(x => x.SumFeePFR_Strah.Value))),
                                                                           new XElement("Уплачено", Utils.decToStr(temp_list.Where(x => x.SumPayPFR_Strah.HasValue).Sum(x => x.SumPayPFR_Strah.Value)))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаНакопительную",
                                                                           new XElement("Начислено", Utils.decToStr(temp_list.Where(x => x.SumFeePFR_Nakop.HasValue).Sum(x => x.SumFeePFR_Nakop.Value))),
                                                                           new XElement("Уплачено", Utils.decToStr(temp_list.Where(x => x.SumPayPFR_Nakop.HasValue).Sum(x => x.SumPayPFR_Nakop.Value)))));

            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ);


            #region СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ

            var ttt = temp_list.Select(x => x.ID).ToList();
            stajOsn_list = db.StajOsn.Where(x => ttt.Contains(x.FormsSZV_6_ID.Value)).ToList();

            foreach (var staff in item.StaffList) // перебираем всех сотрудников попавших в эту пачку
            {
                FormsSZV_6 szv_6 = temp_list.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                Staff ish_staff = StaffList.FirstOrDefault(x => x.ID == staff.StaffID);

                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                string s_name = "";
                if (ins.TypePayer == 0) // если организация
                {
                    if (!String.IsNullOrEmpty(ins.NameShort))
                    {
                        s_name = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    }
                }
                else // если физ. лицо
                {
                    string FIO = "";
                    if (!String.IsNullOrEmpty(ins.LastName))
                    {
                        FIO = FIO + ins.LastName;
                    }
                    if (!String.IsNullOrEmpty(ins.FirstName))
                    {
                        FIO = FIO + " " + ins.FirstName;
                    }
                    if (!String.IsNullOrEmpty(ins.MiddleName))
                    {
                        FIO = FIO + " " + ins.MiddleName;
                    }

                    if (!String.IsNullOrEmpty(FIO))
                    {
                        s_name = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    }
                }

                XElement НаименованиеКраткое = new XElement("НаименованиеКраткое", s_name.ToUpper());

                XElement НалоговыйНомер = new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
                if (!String.IsNullOrEmpty(ins.KPP))
                {
                    НалоговыйНомер.Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                }

                XElement ОтчетныйПериод = new XElement("ОтчетныйПериод",
                                              new XElement("Квартал", szv_6.Quarter),
                                              new XElement("Год", szv_6.Year));

                XElement СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ = new XElement("СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ",
                                                                            new XElement("НомерВпачке", staff.Num.Value + 1),
                                                                            new XElement("ВидФормы", "СЗВ-6-1"),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            НаименованиеКраткое,
                                                                            НалоговыйНомер,
                                                                            new XElement("КодКатегории", szv_6.PlatCategory.Code),
                                                                            ОтчетныйПериод);

                if (szv_6.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
                {
                    СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", szv_6.QuarterKorr.Value),
                                                                   new XElement("Год", szv_6.YearKorr.Value)));
                }


                СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                                     new XElement("ФИО",
                                                                                         new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                         new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                         new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));



                СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                          new XElement("Начислено", szv_6.SumFeePFR_Strah.HasValue ? Utils.decToStr(szv_6.SumFeePFR_Strah.Value) : "0.00"),
                                                                          new XElement("Уплачено", szv_6.SumPayPFR_Strah.HasValue ? Utils.decToStr(szv_6.SumPayPFR_Strah.Value) : "0.00")),
                                                                      new XElement("СуммаВзносовНаНакопительную",
                                                                          new XElement("Начислено", szv_6.SumFeePFR_Nakop.HasValue ? Utils.decToStr(szv_6.SumFeePFR_Nakop.Value) : "0.00"),
                                                                          new XElement("Уплачено", szv_6.SumPayPFR_Nakop.HasValue ? Utils.decToStr(szv_6.SumPayPFR_Nakop.Value) : "0.00")),
                                                                      new XElement("ДатаЗаполнения", szv_6.DateFilling.ToShortDateString()));

                var staj_osn_list = stajOsn_list.Where(x => x.FormsSZV_6_ID == szv_6.ID).OrderBy(x => x.Number.Value).ToList();

                var tt = staj_osn_list.Select(x => x.ID).ToList();
                stajLgot_list = db.StajLgot.Where(x => tt.Contains(x.StajOsnID)).ToList();

                int i = 0;
                foreach (var staj_osn in staj_osn_list)
                {
                    try
                    {
                        i++;
                        XElement СтажевыйПериод = createStajElement(staj_osn, i);
                        СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СтажевыйПериод);
                    }
                    catch { }

                }

                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ);
            }

            #endregion

            xDoc.Root.SetDefaultXmlNamespace(pfr);

            xml = xDoc.ToString();
            return xml;
        }

        private string generateXML_SZV_6_2(xmlInfo item)
        {
            string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion.Substring(2, Application.ProductVersion.Length - 2))),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ = new XElement("ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ",
                                                                  new XElement("НомерВпачке", 1),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            //if (!String.IsNullOrEmpty(ins.EGRIP))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            //}

            //if (!String.IsNullOrEmpty(ins.EGRUL))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            //}

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length).ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length).ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));


            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(СоставительПачки);
            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ТипСведений", InfoType));


            var t = item.StaffList.Select(x => x.StaffID.Value).ToList();
            var StaffList = db.Staff.Where(x => t.Contains(x.ID));
            var t2 = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

            var temp_list = db.FormsSZV_6.Where(x => t2.Contains(x.ID));

            FormsSZV_6 szv_6_t = temp_list.First();




            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КодКатегории", szv_6_t.PlatCategory.Code));

            //            string periodName = "";
            //           string periodNameKorr = "";


            //            RaschetPeriodContainer rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.Quarter && x.Year == item.Year);
            //            if (rp != null)
            //                periodName = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ОтчетныйПериод",
                                                           new XElement("Квартал", item.Quarter.Value),
                                                           new XElement("Год", item.Year.Value)));

            if (item.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
            {
                //                rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.QuarterKorr && x.Year == item.YearKorr);
                //                if (rp != null)
                //                    periodNameKorr = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();

                ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                               new XElement("Квартал", item.QuarterKorr.Value),
                                                               new XElement("Год", item.YearKorr.Value)));
            }


            szv_6_t = null;


            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                           new XElement("Начислено", Utils.decToStr(temp_list.Where(x => x.SumFeePFR_Strah.HasValue).Sum(x => x.SumFeePFR_Strah.Value))),
                                                                           new XElement("Уплачено", Utils.decToStr(temp_list.Where(x => x.SumPayPFR_Strah.HasValue).Sum(x => x.SumPayPFR_Strah.Value)))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаНакопительную",
                                                                           new XElement("Начислено", Utils.decToStr(temp_list.Where(x => x.SumFeePFR_Nakop.HasValue).Sum(x => x.SumFeePFR_Nakop.Value))),
                                                                           new XElement("Уплачено", Utils.decToStr(temp_list.Where(x => x.SumPayPFR_Nakop.HasValue).Sum(x => x.SumPayPFR_Nakop.Value)))));


            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ_ПО_СТРАХОВЫМ_ВЗНОСАМ);


            #region СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ

            var ttt = temp_list.Select(x => x.ID).ToList();
            stajOsn_list = db.StajOsn.Where(x => ttt.Contains(x.FormsSZV_6_ID.Value)).ToList();

            foreach (var staff in item.StaffList) // перебираем всех сотрудников попавших в эту пачку
            {
                FormsSZV_6 szv_6 = temp_list.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                Staff ish_staff = StaffList.FirstOrDefault(x => x.ID == staff.StaffID);

                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                string s_name = "";
                if (ins.TypePayer == 0) // если организация
                {
                    if (!String.IsNullOrEmpty(ins.NameShort))
                    {
                        s_name = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    }
                }
                else // если физ. лицо
                {
                    string FIO = "";
                    if (!String.IsNullOrEmpty(ins.LastName))
                    {
                        FIO = FIO + ins.LastName;
                    }
                    if (!String.IsNullOrEmpty(ins.FirstName))
                    {
                        FIO = FIO + " " + ins.FirstName;
                    }
                    if (!String.IsNullOrEmpty(ins.MiddleName))
                    {
                        FIO = FIO + " " + ins.MiddleName;
                    }

                    if (!String.IsNullOrEmpty(FIO))
                    {
                        s_name = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    }
                }

                XElement НаименованиеКраткое = new XElement("НаименованиеКраткое", s_name.ToUpper());

                XElement НалоговыйНомер = new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
                if (!String.IsNullOrEmpty(ins.KPP))
                {
                    НалоговыйНомер.Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                }

                XElement ОтчетныйПериод = new XElement("ОтчетныйПериод",
                                              new XElement("Квартал", szv_6.Quarter),
                                              new XElement("Год", szv_6.Year));

                XElement СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ = new XElement("СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ",
                                                                            new XElement("НомерВпачке", staff.Num.Value + 1),
                                                                            new XElement("ВидФормы", "СЗВ-6-2"),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            НаименованиеКраткое,
                                                                            НалоговыйНомер,
                                                                            new XElement("КодКатегории", szv_6.PlatCategory.Code),
                                                                            ОтчетныйПериод);

                if (szv_6.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
                {
                    СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", szv_6.QuarterKorr.Value),
                                                                   new XElement("Год", szv_6.YearKorr.Value)));
                }


                СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                                     new XElement("ФИО",
                                                                                         new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                         new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                         new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));



                СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                          new XElement("Начислено", szv_6.SumFeePFR_Strah.HasValue ? Utils.decToStr(szv_6.SumFeePFR_Strah.Value) : "0.00"),
                                                                          new XElement("Уплачено", szv_6.SumPayPFR_Strah.HasValue ? Utils.decToStr(szv_6.SumPayPFR_Strah.Value) : "0.00")),
                                                                      new XElement("СуммаВзносовНаНакопительную",
                                                                          new XElement("Начислено", szv_6.SumFeePFR_Nakop.HasValue ? Utils.decToStr(szv_6.SumFeePFR_Nakop.Value) : "0.00"),
                                                                          new XElement("Уплачено", szv_6.SumPayPFR_Nakop.HasValue ? Utils.decToStr(szv_6.SumPayPFR_Nakop.Value) : "0.00")),
                                                                      new XElement("ДатаЗаполнения", szv_6.DateFilling.ToShortDateString()));

                var staj_osn_list = stajOsn_list.Where(x => x.FormsSZV_6_ID == szv_6.ID).OrderBy(x => x.Number.Value).ToList();

                var tt = staj_osn_list.Select(x => x.ID).ToList();
                stajLgot_list = db.StajLgot.Where(x => tt.Contains(x.StajOsnID)).ToList();

                int i = 0;
                foreach (var staj_osn in staj_osn_list)
                {
                    try
                    {
                        i++;
                        XElement СтажевыйПериод = createStajElement(staj_osn, i);
                        СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СтажевыйПериод);
                    }
                    catch { }

                }

                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ);
            }

            #endregion

            xDoc.Root.SetDefaultXmlNamespace(pfr);

            xml = xDoc.ToString();
            return xml;
        }


        private string generateXML_SZV_6_4(xmlInfo item)
        {
            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion.Substring(2, Application.ProductVersion.Length - 2))),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ = new XElement("ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ",
                                                                  new XElement("НомерВпачке", 1),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));


            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRIP))
            {
                СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRUL))
            {
                СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            }

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length).ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length).ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));


            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(СоставительПачки);
            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ТипСведений", InfoType));


            var t = item.StaffList.Select(x => x.StaffID.Value).ToList();
            var StaffList = db.Staff.Where(x => t.Contains(x.ID));

            var t2 = item.StaffList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

            var temp_list = db.FormsSZV_6_4.Where(x => t2.Contains(x.ID));
            FormsSZV_6_4 szv_6_4_t = temp_list.First();

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КодКатегории", szv_6_4_t.PlatCategory.Code.ToUpper()));

            //            string periodName = "";
            //            string periodNameKorr = "";

            List<int> monthes = new List<int>();
            RaschetPeriodContainer rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.Quarter && x.Year == item.Year);
            if (rp != null)
            {
                for (int i = rp.DateBegin.Month; i <= rp.DateEnd.Month; i++)
                {
                    monthes.Add(i);
                }
            }

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ОтчетныйПериод",
                                                           new XElement("Квартал", item.Quarter.Value),
                                                           new XElement("Год", item.Year.Value)));

            if (item.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
            {
                //             rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.Kvartal == item.QuarterKorr && x.Year == item.YearKorr);
                //             if (rp != null)
                //                 periodNameKorr = "С " + rp.DateBegin.ToShortDateString() + " ПО " + rp.DateEnd.ToShortDateString();
                monthes = new List<int>();
                rp = Options.RaschetPeriodInternal2010_2013.FirstOrDefault(x => x.Kvartal == item.QuarterKorr && x.Year == item.YearKorr);
                if (rp != null)
                {
                    for (int i = rp.DateBegin.Month; i <= rp.DateEnd.Month; i++)
                    {
                        monthes.Add(i);
                    }
                }


                ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                               new XElement("Квартал", item.QuarterKorr.Value),
                                                               new XElement("Год", item.YearKorr.Value)));
            }


            byte typeContract = szv_6_4_t.TypeContract;

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("ТипДоговора", typeContract == 1 ? "ТРУДОВОЙ" : "ГРАЖДАНСКО-ПРАВОВОЙ"));
            szv_6_4_t = null;



            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВыплатИвознагражденийВпользуЗЛ",
                                                                           new XElement("ТипСтроки", "ИТОГ"),
                                                                           new XElement("СуммаВыплатВсего", Utils.decToStr(temp_list.Sum(x => x.s_0_0.Value))),
                                                                           new XElement("СуммаВыплатНачисленыСтраховыеВзносыНеПревышающие", Utils.decToStr(temp_list.Sum(x => x.s_0_1.Value))),
                                                                           new XElement("СуммаВыплатНачисленыСтраховыеВзносыПревышающие", Utils.decToStr(temp_list.Sum(x => x.s_0_2.Value)))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                           new XElement("Начислено", Utils.decToStr(temp_list.Sum(x => x.SumFeePFR_Strah.Value))),
                                                                           new XElement("Уплачено", Utils.decToStr(temp_list.Sum(x => x.SumPayPFR_Strah.Value)))));

            ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ.Add(new XElement("СуммаВзносовНаНакопительную",
                                                                           new XElement("Начислено", Utils.decToStr(temp_list.Sum(x => x.SumFeePFR_Nakop.Value))),
                                                                           new XElement("Уплачено", Utils.decToStr(temp_list.Sum(x => x.SumPayPFR_Nakop.Value)))));

            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ_ПО_СУММАМ_ВЫПЛАТ_И_ПО_СТРАХОВЫМ_ВЗНОСАМ);


            #region СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ

            var ttt = temp_list.Select(x => x.ID).ToList();
            stajOsn_list = db.StajOsn.Where(x => ttt.Contains(x.FormsSZV_6_4_ID.Value)).ToList();

            foreach (var staff in item.StaffList) // перебираем всех сотрудников попавших в эту пачку
            {

                FormsSZV_6_4 szv_6_4 = temp_list.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                Staff ish_staff = StaffList.FirstOrDefault(x => x.ID == staff.StaffID);

                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                string s_name = "";
                if (ins.TypePayer == 0) // если организация
                {
                    if (!String.IsNullOrEmpty(ins.NameShort))
                    {
                        s_name = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    }
                }
                else // если физ. лицо
                {
                    string FIO = "";
                    if (!String.IsNullOrEmpty(ins.LastName))
                    {
                        FIO = FIO + ins.LastName;
                    }
                    if (!String.IsNullOrEmpty(ins.FirstName))
                    {
                        FIO = FIO + " " + ins.FirstName;
                    }
                    if (!String.IsNullOrEmpty(ins.MiddleName))
                    {
                        FIO = FIO + " " + ins.MiddleName;
                    }

                    if (!String.IsNullOrEmpty(FIO))
                    {
                        s_name = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    }
                }

                XElement НаименованиеКраткое = new XElement("НаименованиеКраткое", s_name.ToUpper());

                XElement НалоговыйНомер = new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
                if (!String.IsNullOrEmpty(ins.KPP))
                {
                    НалоговыйНомер.Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                }

                XElement ОтчетныйПериод = new XElement("ОтчетныйПериод",
                                              new XElement("Квартал", szv_6_4.Quarter),
                                              new XElement("Год", szv_6_4.Year));


                XElement СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ = new XElement("СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ",
                                                                            new XElement("НомерВпачке", staff.Num.Value + 1),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            НаименованиеКраткое,
                                                                            НалоговыйНомер,
                                                                            new XElement("КодКатегории", szv_6_4.PlatCategory.Code.ToUpper()),
                                                                            ОтчетныйПериод);

                if (szv_6_4.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", szv_6_4.QuarterKorr.Value),
                                                                   new XElement("Год", szv_6_4.YearKorr.Value)));
                }

                if (!string.IsNullOrEmpty(szv_6_4.RegNumKorr))
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("РегистрационныйНомерКорректируемогоПериода", Utils.ParseRegNum(szv_6_4.RegNumKorr)));
                }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                                     new XElement("ФИО",
                                                                                         new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                         new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                         new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));
                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ТипДоговора", szv_6_4.TypeContract == 1 ? "ТРУДОВОЙ" : "ГРАЖДАНСКО-ПРАВОВОЙ"));


                XElement СуммаВыплатИвознагражденийВпользуЗЛ = new XElement("СуммаВыплатИвознагражденийВпользуЗЛ");

                if (monthes.Count() < 3)
                {
                    monthes = new List<int> { 1, 2, 3 };
                }

                for (int j = 1; j <= 3; j++)
                {
                    string itemName1 = "s_" + j.ToString() + "_0";
                    string itemName2 = "s_" + j.ToString() + "_1";
                    string itemName3 = "s_" + j.ToString() + "_2";

                    СуммаВыплатИвознагражденийВпользуЗЛ = new XElement("СуммаВыплатИвознагражденийВпользуЗЛ");

                    СуммаВыплатИвознагражденийВпользуЗЛ.Add(new XElement("ТипСтроки", "МЕСЦ"),
                                                            new XElement("Месяц", monthes[j - 1]),
                                                            new XElement("СуммаВыплатВсего", szv_6_4.GetType().GetProperty(itemName1).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName1).GetValue(szv_6_4, null).ToString())) : "0.00"),
                                                            new XElement("СуммаВыплатНачисленыСтраховыеВзносыНеПревышающие", szv_6_4.GetType().GetProperty(itemName2).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName2).GetValue(szv_6_4, null).ToString())) : "0.00"),
                                                            new XElement("СуммаВыплатНачисленыСтраховыеВзносыПревышающие", szv_6_4.GetType().GetProperty(itemName3).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName3).GetValue(szv_6_4, null).ToString())) : "0.00"));

                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СуммаВыплатИвознагражденийВпользуЗЛ);
                }

                СуммаВыплатИвознагражденийВпользуЗЛ = new XElement("СуммаВыплатИвознагражденийВпользуЗЛ");

                СуммаВыплатИвознагражденийВпользуЗЛ.Add(new XElement("ТипСтроки", "ИТОГ"),
                                                            new XElement("СуммаВыплатВсего", szv_6_4.s_0_0.HasValue ? Utils.decToStr(szv_6_4.s_0_0.Value) : "0.00"),
                                                            new XElement("СуммаВыплатНачисленыСтраховыеВзносыНеПревышающие", szv_6_4.s_0_1.HasValue ? Utils.decToStr(szv_6_4.s_0_1.Value) : "0.00"),
                                                            new XElement("СуммаВыплатНачисленыСтраховыеВзносыПревышающие", szv_6_4.s_0_2.HasValue ? Utils.decToStr(szv_6_4.s_0_2.Value) : "0.00"));

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СуммаВыплатИвознагражденийВпользуЗЛ);


                // данные по выплате по доп тарифу
                if ((szv_6_4.d_0_0.HasValue && szv_6_4.d_0_0.Value != 0) || (szv_6_4.d_0_1.HasValue && szv_6_4.d_0_1.Value != 0))
                {
                    XElement СуммаВыплатИвознагражденийПоДопТарифу = new XElement("СуммаВыплатИвознагражденийПоДопТарифу");

                    for (int j = 1; j <= 3; j++)
                    {
                        string itemName1 = "d_" + j.ToString() + "_0";
                        string itemName2 = "d_" + j.ToString() + "_1";

                        СуммаВыплатИвознагражденийПоДопТарифу = new XElement("СуммаВыплатИвознагражденийПоДопТарифу");

                        СуммаВыплатИвознагражденийПоДопТарифу.Add(new XElement("ТипСтроки", "МЕСЦ"),
                                                                new XElement("Месяц", monthes[j - 1]),
                                                                new XElement("СуммаВыплатПоДопТарифу27-1", szv_6_4.GetType().GetProperty(itemName1).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName1).GetValue(szv_6_4, null).ToString())) : "0.00"),
                                                                new XElement("СуммаВыплатПоДопТарифу27-2-18", szv_6_4.GetType().GetProperty(itemName2).GetValue(szv_6_4, null) != null ? Utils.decToStr(decimal.Parse(szv_6_4.GetType().GetProperty(itemName2).GetValue(szv_6_4, null).ToString())) : "0.00"));

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СуммаВыплатИвознагражденийПоДопТарифу);
                    }

                    СуммаВыплатИвознагражденийПоДопТарифу = new XElement("СуммаВыплатИвознагражденийПоДопТарифу");

                    СуммаВыплатИвознагражденийПоДопТарифу.Add(new XElement("ТипСтроки", "ИТОГ"),
                                                                new XElement("СуммаВыплатПоДопТарифу27-1", szv_6_4.d_0_0.HasValue ? Utils.decToStr(szv_6_4.d_0_0.Value) : "0.00"),
                                                                new XElement("СуммаВыплатПоДопТарифу27-2-18", szv_6_4.d_0_1.HasValue ? Utils.decToStr(szv_6_4.d_0_1.Value) : "0.00"));

                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СуммаВыплатИвознагражденийПоДопТарифу);
                }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СуммаВзносовНаСтраховую",
                                                                                         new XElement("Начислено", szv_6_4.SumFeePFR_Strah.HasValue ? Utils.decToStr(szv_6_4.SumFeePFR_Strah.Value) : "0.00"),
                                                                                         new XElement("Уплачено", szv_6_4.SumPayPFR_Strah.HasValue ? Utils.decToStr(szv_6_4.SumPayPFR_Strah.Value) : "0.00")),
                                                                                     new XElement("СуммаВзносовНаНакопительную",
                                                                                         new XElement("Начислено", szv_6_4.SumFeePFR_Nakop.HasValue ? Utils.decToStr(szv_6_4.SumFeePFR_Nakop.Value) : "0.00"),
                                                                                         new XElement("Уплачено", szv_6_4.SumPayPFR_Nakop.HasValue ? Utils.decToStr(szv_6_4.SumPayPFR_Nakop.Value) : "0.00")));

                var staj_osn_list = stajOsn_list.Where(x => x.FormsSZV_6_4_ID == szv_6_4.ID).OrderBy(x => x.Number.Value).ToList();

                var tt = staj_osn_list.Select(x => x.ID).ToList();
                stajLgot_list = db.StajLgot.Where(x => tt.Contains(x.StajOsnID)).ToList();

                int i = 0;
                foreach (var staj_osn in staj_osn_list)
                {
                    try
                    {
                        i++;
                        XElement СтажевыйПериод = createStajElement(staj_osn, i);

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СтажевыйПериод);
                    }
                    catch { }

                }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ДатаЗаполнения", szv_6_4.DateFilling.ToShortDateString()));


                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_О_СТРАХОВЫХ_ВЗНОСАХ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ);
            }

            #endregion

            xDoc.Root.SetDefaultXmlNamespace(pfr);

            xml = xDoc.ToString();
            return xml;
        }


        /// <summary>
        /// Формирование ветки XML с информацией о стаже для РСВ-1 (6 раздел), СЗВ-6-4, СЗВ-6
        /// </summary>
        /// <param name="staj_osn"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private XElement createStajElement(StajOsn staj_osn, int i)
        {

            //var staj_lgot_list = staj_osn.StajLgot.ToList().OrderBy(x => x.Number.Value);

            var staj_lgot_list = stajLgot_list.Where(x => x.StajOsnID == staj_osn.ID).ToList().OrderBy(x => x.Number.Value);


            XElement СтажевыйПериод = new XElement("СтажевыйПериод");

            СтажевыйПериод.Add(new XElement("НомерСтроки", i),
                                   new XElement("ДатаНачалаПериода", staj_osn.DateBegin.Value.ToShortDateString()),
                                   new XElement("ДатаКонцаПериода", staj_osn.DateEnd.Value.ToShortDateString()));

            int cnt = staj_lgot_list.Count();
            if (cnt > 0)
            {

                СтажевыйПериод.Add(new XElement("КоличествоЛьготныхСоставляющих", cnt.ToString()));

                int j = 0;
                foreach (var staj_lgot in staj_lgot_list)
                {
                    j++;
                    XElement ЛьготныйСтаж = new XElement("ЛьготныйСтаж",
                                                new XElement("НомерСтроки", j));

                    XElement ОсобенностиУчета = new XElement("ОсобенностиУчета");
                    if (staj_lgot.TerrUslID != null)
                    {
                        var t = TerrUsl_list.First(x => x.ID == staj_lgot.TerrUslID.Value).Code;
                        ОсобенностиУчета.Add(new XElement("ТерриториальныеУсловия",
                                                 new XElement("ОснованиеТУ", t)));//staj_lgot.TerrUsl.Code

                        if (t.Substring(0, 1) != "Ч" && staj_lgot.TerrUslKoef.Value != 0)//staj_lgot.TerrUsl.Code
                        {
                            ОсобенностиУчета.Element("ТерриториальныеУсловия").Add(new XElement("Коэффициент", Utils.decToStr(staj_lgot.TerrUslKoef.Value)));
                        }
                    }

                    if (staj_lgot.OsobUslTrudaID != null)
                    {
                        XElement ОсобыеУсловияТруда = new XElement("ОсобыеУсловияТруда",
                                                 new XElement("ОснованиеОУТ", OsobUslTruda_list.First(x => x.ID == staj_lgot.OsobUslTrudaID.Value).Code));//staj_lgot.OsobUslTruda.Code

                        if (staj_lgot.KodVred_OsnID != null)
                        {
                            ОсобыеУсловияТруда.Add(new XElement("ПозицияСписка", KodVred_2_list.First(x => x.ID == staj_lgot.KodVred_OsnID.Value).Code));//staj_lgot.KodVred_2.Code
                        }

                        ОсобенностиУчета.Add(ОсобыеУсловияТруда);
                    }

                    if (staj_lgot.IschislStrahStajOsnID != null || (staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0) || (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0))
                    {
                        XElement ИсчисляемыйСтаж = new XElement("ИсчисляемыйСтаж");

                        if (staj_lgot.IschislStrahStajOsnID != null)
                        {
                            var t = IschislStrahStajOsn_list.First(x => x.ID == staj_lgot.IschislStrahStajOsnID.Value).Code;
                            ИсчисляемыйСтаж.Add(new XElement("ОснованиеИС", t));//staj_lgot.IschislStrahStajOsn.Code
                            if (t == "ВОДОЛАЗ")
                            {
                                if (staj_lgot.Strah1Param.HasValue || staj_lgot.Strah2Param.HasValue)
                                {
                                    XElement ВыработкаВчасах = new XElement("ВыработкаВчасах");
                                    if (staj_lgot.Strah1Param.HasValue)
                                        ВыработкаВчасах.Add(new XElement("Часы", staj_lgot.Strah1Param.Value));
                                    if (staj_lgot.Strah2Param.HasValue)
                                        ВыработкаВчасах.Add(new XElement("Минуты", staj_lgot.Strah2Param.Value));

                                    ИсчисляемыйСтаж.Add(ВыработкаВчасах);
                                }
                            }
                            else
                            {
                                if (staj_lgot.Strah1Param.HasValue || staj_lgot.Strah2Param.HasValue)
                                {
                                    XElement ВыработкаКалендарная = new XElement("ВыработкаКалендарная");
                                    if (staj_lgot.Strah1Param.HasValue)
                                        ВыработкаКалендарная.Add(new XElement("ВсеМесяцы", staj_lgot.Strah1Param.Value));
                                    if (staj_lgot.Strah2Param.HasValue)
                                        ВыработкаКалендарная.Add(new XElement("ВсеДни", staj_lgot.Strah2Param.Value));

                                    ИсчисляемыйСтаж.Add(ВыработкаКалендарная);
                                }
                            }
                        }
                        else if ((staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0) || (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0))
                        {
                            XElement ВыработкаКалендарная = new XElement("ВыработкаКалендарная");
                            if (staj_lgot.Strah1Param.HasValue)
                                ВыработкаКалендарная.Add(new XElement("ВсеМесяцы", staj_lgot.Strah1Param.Value));
                            if (staj_lgot.Strah2Param.HasValue)
                                ВыработкаКалендарная.Add(new XElement("ВсеДни", staj_lgot.Strah2Param.Value));

                            ИсчисляемыйСтаж.Add(ВыработкаКалендарная);
                        }

                        ОсобенностиУчета.Add(ИсчисляемыйСтаж);
                    }

                    if (staj_lgot.IschislStrahStajDopID != null)
                    {
                        ОсобенностиУчета.Add(new XElement("ДекретДети", IschislStrahStajDop_list.First(x => x.ID == staj_lgot.IschislStrahStajDopID.Value).Code));//staj_lgot.IschislStrahStajDop.Code
                    }

                    if (staj_lgot.UslDosrNaznID != null)
                    {
                        XElement ВыслугаЛет = new XElement("ВыслугаЛет",
                                                 new XElement("ОснованиеВЛ", UslDosrNazn_list.First(x => x.ID == staj_lgot.UslDosrNaznID.Value).Code));//staj_lgot.UslDosrNazn.Code

                        if (staj_lgot.UslDosrNazn.Code == "27-15")
                        {
                            if (staj_lgot.UslDosrNazn1Param.HasValue || staj_lgot.UslDosrNazn2Param.HasValue)
                            {
                                XElement ВыработкаКалендарная = new XElement("ВыработкаКалендарная");
                                if (staj_lgot.UslDosrNazn1Param.HasValue)
                                    ВыработкаКалендарная.Add(new XElement("ВсеМесяцы", staj_lgot.UslDosrNazn1Param.Value));
                                if (staj_lgot.UslDosrNazn2Param.HasValue)
                                    ВыработкаКалендарная.Add(new XElement("ВсеДни", staj_lgot.UslDosrNazn2Param.Value));

                                ВыслугаЛет.Add(ВыработкаКалендарная);
                            }
                        }
                        else
                        {
                            if ((staj_lgot.UslDosrNazn1Param.HasValue && staj_lgot.UslDosrNazn1Param.Value != 0) || (staj_lgot.UslDosrNazn2Param.HasValue && staj_lgot.UslDosrNazn2Param.Value != 0))
                            {
                                XElement ВыработкаВчасах = new XElement("ВыработкаВчасах");
                                if (staj_lgot.UslDosrNazn1Param.HasValue)
                                    ВыработкаВчасах.Add(new XElement("Часы", staj_lgot.UslDosrNazn1Param.Value));
                                if (staj_lgot.UslDosrNazn2Param.HasValue)
                                    ВыработкаВчасах.Add(new XElement("Минуты", staj_lgot.UslDosrNazn2Param.Value));

                                ВыслугаЛет.Add(ВыработкаВчасах);
                            }
                        }

                        if (staj_lgot.UslDosrNazn3Param.HasValue && staj_lgot.UslDosrNazn3Param.Value != 0)
                        {
                            ВыслугаЛет.Add(new XElement("ДоляСтавки", Utils.decToStr(staj_lgot.UslDosrNazn3Param.Value)));
                        }

                        ОсобенностиУчета.Add(ВыслугаЛет);
                    }

                    ЛьготныйСтаж.Add(ОсобенностиУчета);

                    СтажевыйПериод.Add(ЛьготныйСтаж);
                }
            }

            return СтажевыйПериод;
        }

        private class razd66Period
        {
            public long ID { get; set; }
            public short Y { get; set; }
            public byte Q { get; set; }
        }

        private string generateXML_RSW2014_6(xmlInfo item)
        {
            string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion.Substring(2, Application.ProductVersion.Length - 2))),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6 = new XElement("СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6",
                                                                  new XElement("НомерВпачке", 1),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            //if (!String.IsNullOrEmpty(ins.EGRIP))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            //}

            //if (!String.IsNullOrEmpty(ins.EGRUL))
            //{
            //    СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            //}

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length).ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length).ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));


            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(СоставительПачки);
            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("ТипСведений", InfoType));

            СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("ОтчетныйПериод",
                                                           new XElement("Квартал", item.Quarter.Value),
                                                           new XElement("Год", item.Year.Value)));

            if (item.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
            {
                СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                               new XElement("Квартал", item.QuarterKorr.Value),
                                                               new XElement("Год", item.YearKorr.Value)));
            }

            if (item.rsw2014.First().RSW_2_5_1_2.HasValue)
            {
                СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную", item.rsw2014.First().RSW_2_5_1_2.Value != 0 ? Utils.decToStr(item.rsw2014.First().RSW_2_5_1_2.Value) : "0.00"));
            }

            if (item.rsw2014.First().RSW_2_5_1_3.HasValue)
            {
                СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6.Add(new XElement("СтраховыхВзносовОПС", item.rsw2014.First().RSW_2_5_1_3.Value != 0 ? Utils.decToStr(item.rsw2014.First().RSW_2_5_1_3.Value) : "0.00"));
            }

            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_ПО_ПАЧКЕ_ДОКУМЕНТОВ_РАЗДЕЛА_6);


            #region СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ

            List<StaffList> staffTempList = item.StaffList.ToList();
            List<long> t = staffTempList.Select(x => x.StaffID.Value).ToList();
            List<Staff> StaffList = StaffList_All_Ishod.Where(x => t.Contains(x.ID)).ToList();

            t = staffTempList.Select(x => x.FormsRSW_6_1_ID.Value).ToList();

            List<FormsRSW2014_1_Razd_6_1> rsw61List_tmp = rsw61List.Where(x => t.Contains(x.ID)).ToList();
            //t = rsw61List_tmp.Select(x => x.ID).ToList();

            stajOsn_list = db.StajOsn.Where(x => t.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).ToList();
            List<long> t__ = stajOsn_list.Select(x => x.ID).ToList();
            List<StajLgot> stajLgot_list_t = db.StajLgot.Where(x => t__.Contains(x.StajOsnID)).ToList();

            t__.Clear();

            razd64_list = db.FormsRSW2014_1_Razd_6_4.Where(x => t.Contains(x.FormsRSW2014_1_Razd_6_1_ID.Value)).ToList();
            razd66_list = db.FormsRSW2014_1_Razd_6_6.Where(x => t.Contains(x.FormsRSW2014_1_Razd_6_1_ID)).ToList();
            razd67_list = db.FormsRSW2014_1_Razd_6_7.Where(x => t.Contains(x.FormsRSW2014_1_Razd_6_1_ID)).ToList();


            foreach (var staff in staffTempList) // перебираем всех сотрудников попавших в эту пачку
            {
                //                FormsRSW2014_1_Razd_6_1 rsw61 = db.FormsRSW2014_1_Razd_6_1.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                FormsRSW2014_1_Razd_6_1 rsw61 = rsw61List_tmp.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);


                //                Staff ish_staff = db.Staff.FirstOrDefault(x => x.ID == staff.StaffID);
                Staff ish_staff = StaffList.First(x => x.ID == staff.StaffID);
                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                XElement СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ = new XElement("СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ",
                                                                            new XElement("НомерВпачке", staff.Num.Value + 1),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                            new XElement("ФИО",
                                                                                new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));
                // СведенияОбУвольнении РСВ-1 Р6 2015 об увольнении застрахованного лица
                if (yearType == 2015 && ish_staff.Dismissed.HasValue && ish_staff.Dismissed.Value == 1)
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СведенияОбУвольнении", "УВОЛЕН"));
                }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ОтчетныйПериод",
                                                                new XElement("Квартал", rsw61.Quarter),
                                                                new XElement("Год", rsw61.Year)));
                int m = 0;
                switch (rsw61.Quarter)
                {
                    case 0:
                        m = 10;
                        break;
                    case 3:
                        m = 1;
                        break;
                    case 6:
                        m = 4;
                        break;
                    case 9:
                        m = 7;
                        break;
                }

                if (rsw61.YearKorr.HasValue && InfoType != "ИСХОДНАЯ")
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", rsw61.QuarterKorr.Value),
                                                                   new XElement("Год", rsw61.YearKorr.Value)));
                    switch (rsw61.QuarterKorr)
                    {
                        case 0:
                            m = 10;
                            break;
                        case 3:
                            m = 1;
                            break;
                        case 6:
                            m = 4;
                            break;
                        case 9:
                            m = 7;
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(rsw61.RegNumKorr))
                {
                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("РегистрационныйНомерКорректируемогоПериода", Utils.ParseRegNum(rsw61.RegNumKorr)));
                }

                //                var rsw64_list = rsw61.FormsRSW2014_1_Razd_6_4;
                var rsw64_list = razd64_list.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw61.ID).ToList();

                int i = 1;
                int k = 0;

                try
                {
                    foreach (var rsw64 in rsw64_list)
                    {
                        string code = PlatCatList.First(x => x.ID == rsw64.PlatCategoryID).Code;

                        XElement СведенияОсуммеВыплатИвознагражденийВпользуЗЛ = new XElement("СведенияОсуммеВыплатИвознагражденийВпользуЗЛ");

                        СведенияОсуммеВыплатИвознагражденийВпользуЗЛ.Add(new XElement("НомерСтроки", i),
                                                                         new XElement("ТипСтроки", "ИТОГ"),
                                                                         new XElement("КодСтроки", "4" + k.ToString() + "0"),
                                                                         new XElement("КодКатегории", code));
                        if ((rsw64.s_0_0.HasValue && rsw64.s_0_0.Value > 0) || (rsw64.s_0_1.HasValue && rsw64.s_0_1.Value > 0) || (rsw64.s_0_1.HasValue && rsw64.s_0_1.Value > 0) || (rsw64.s_0_1.HasValue && rsw64.s_0_1.Value > 0))
                        {
                            СведенияОсуммеВыплатИвознагражденийВпользуЗЛ.Add(new XElement("СуммаВыплатИныхВознаграждений", rsw64.s_0_0.HasValue ? Utils.decToStr(rsw64.s_0_0.Value) : "0.00"),
                                                                             new XElement("НеПревышающиеВсего", rsw64.s_0_1.HasValue ? Utils.decToStr(rsw64.s_0_1.Value) : "0.00"),
                                                                             new XElement("НеПревышающиеПоДоговорам", rsw64.s_0_2.HasValue ? Utils.decToStr(rsw64.s_0_2.Value) : "0.00"),
                                                                             new XElement("ПревышающиеПредельную", rsw64.s_0_3.HasValue ? Utils.decToStr(rsw64.s_0_3.Value) : "0.00"));
                        }

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОсуммеВыплатИвознагражденийВпользуЗЛ);
                        i++;


                        for (int j = 1; j <= 3; j++)
                        {
                            string itemName = "s_" + j.ToString() + "_";

                            if (((rsw64.GetType().GetProperty(itemName + "0").GetValue(rsw64, null) != null) && (decimal.Parse(rsw64.GetType().GetProperty(itemName + "0").GetValue(rsw64, null).ToString()) != 0)) || ((rsw64.GetType().GetProperty(itemName + "1").GetValue(rsw64, null) != null) && (decimal.Parse(rsw64.GetType().GetProperty(itemName + "1").GetValue(rsw64, null).ToString()) != 0)) || ((rsw64.GetType().GetProperty(itemName + "2").GetValue(rsw64, null) != null) && (decimal.Parse(rsw64.GetType().GetProperty(itemName + "2").GetValue(rsw64, null).ToString()) != 0)) || ((rsw64.GetType().GetProperty(itemName + "3").GetValue(rsw64, null) != null) && (decimal.Parse(rsw64.GetType().GetProperty(itemName + "3").GetValue(rsw64, null).ToString()) != 0)))
                            {
                                СведенияОсуммеВыплатИвознагражденийВпользуЗЛ = new XElement("СведенияОсуммеВыплатИвознагражденийВпользуЗЛ");


                                СведенияОсуммеВыплатИвознагражденийВпользуЗЛ.Add(new XElement("НомерСтроки", i),
                                                                                 new XElement("ТипСтроки", "МЕСЦ"),
                                                                                 new XElement("Месяц", m + j - 1),
                                                                                 new XElement("КодСтроки", "4" + k.ToString() + j.ToString()),
                                                                                 new XElement("КодКатегории", code),
                                                                                 new XElement("СуммаВыплатИныхВознаграждений", rsw64.GetType().GetProperty(itemName + "0").GetValue(rsw64, null) != null ? Utils.decToStr(decimal.Parse(rsw64.GetType().GetProperty(itemName + "0").GetValue(rsw64, null).ToString())) : "0.00"),
                                                                                 new XElement("НеПревышающиеВсего", rsw64.GetType().GetProperty(itemName + "1").GetValue(rsw64, null) != null ? Utils.decToStr(decimal.Parse(rsw64.GetType().GetProperty(itemName + "1").GetValue(rsw64, null).ToString())) : "0.00"),
                                                                                 new XElement("НеПревышающиеПоДоговорам", rsw64.GetType().GetProperty(itemName + "2").GetValue(rsw64, null) != null ? Utils.decToStr(decimal.Parse(rsw64.GetType().GetProperty(itemName + "2").GetValue(rsw64, null).ToString())) : "0.00"),
                                                                                 new XElement("ПревышающиеПредельную", rsw64.GetType().GetProperty(itemName + "3").GetValue(rsw64, null) != null ? Utils.decToStr(decimal.Parse(rsw64.GetType().GetProperty(itemName + "3").GetValue(rsw64, null).ToString())) : "0.00"));

                                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОсуммеВыплатИвознагражденийВпользуЗЛ);
                                i++;
                            }
                        }

                        k++;
                    }
                }
                catch { }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("СуммаВзносовНаОПС", rsw61.SumFeePFR.HasValue ? Utils.decToStr(rsw61.SumFeePFR.Value) : "0.00"));

                //                var rsw66_list = rsw61.FormsRSW2014_1_Razd_6_6.OrderBy(x => x.AccountPeriodYear);
                var rsw66_list = razd66_list.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw61.ID).ToList();


                try
                {
                    if (rsw66_list.Count() > 0)
                    {
                        var years66 = rsw66_list.Select(x => x.AccountPeriodYear).Distinct().OrderBy(x => x.Value);
                        List<razd66Period> temp66List = rsw66_list.Select(x => new razd66Period { ID = x.ID, Y = x.AccountPeriodYear.HasValue ? x.AccountPeriodYear.Value : (short)0, Q = x.AccountPeriodQuarter.HasValue ? (x.AccountPeriodQuarter.Value != 0 ? x.AccountPeriodQuarter.Value : (byte)12) : (byte)0 }).ToList();
                        i = 0;
                        decimal[] sum = new decimal[3] { 0, 0, 0 };
                        List<XElement> korrList = new List<XElement>();

                        foreach (var year66 in years66)
                        {
                            //var rsw66_list_part = rsw66_list.Where(x => x.AccountPeriodYear == year66).OrderBy(x => x.AccountPeriodQuarter);
                            var rsw66_list_part = temp66List.Where(x => x.Y == year66).OrderBy(x => x.Q);
                            foreach (var itemID in rsw66_list_part)
                            {
                                var rsw66 = rsw66_list.FirstOrDefault(x => x.ID == itemID.ID);
                                i++;
                                XElement СведенияОкорректировках = new XElement("СведенияОкорректировках");

                                СведенияОкорректировках.Add(new XElement("НомерСтроки", i),
                                                                new XElement("ТипСтроки", "МЕСЦ"),
                                                                new XElement("Квартал", rsw66.AccountPeriodQuarter.HasValue ? rsw66.AccountPeriodQuarter.Value.ToString() : ""),
                                                                new XElement("Год", rsw66.AccountPeriodYear.HasValue ? rsw66.AccountPeriodYear.Value.ToString() : ""),
                                                                new XElement("СуммаДоначисленныхВзносовОПС", rsw66.SumFeePFR_D.HasValue ? Utils.decToStr(rsw66.SumFeePFR_D.Value) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаСтраховую", rsw66.SumFeePFR_StrahD.HasValue ? Utils.decToStr(rsw66.SumFeePFR_StrahD.Value) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаНакопительную", rsw66.SumFeePFR_NakopD.HasValue ? Utils.decToStr(rsw66.SumFeePFR_NakopD.Value) : "0.00"));

                                sum[0] = sum[0] + (rsw66.SumFeePFR_D.HasValue ? rsw66.SumFeePFR_D.Value : 0);
                                sum[1] = sum[1] + (rsw66.SumFeePFR_StrahD.HasValue ? rsw66.SumFeePFR_StrahD.Value : 0);
                                sum[2] = sum[2] + (rsw66.SumFeePFR_NakopD.HasValue ? rsw66.SumFeePFR_NakopD.Value : 0);

                                //   korrList.Add(СведенияОкорректировках);
                                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОкорректировках);
                            }
                        }

                        i++;
                        XElement СведенияОкорректировках_ИТОГ = new XElement("СведенияОкорректировках");

                        СведенияОкорректировках_ИТОГ.Add(new XElement("НомерСтроки", i),
                                                        new XElement("ТипСтроки", "ИТОГ"),
                                                        new XElement("СуммаДоначисленныхВзносовОПС", Utils.decToStr(sum[0])),
                                                        new XElement("СуммаДоначисленныхВзносовНаСтраховую", Utils.decToStr(sum[1])),
                                                        new XElement("СуммаДоначисленныхВзносовНаНакопительную", Utils.decToStr(sum[2])));

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОкорректировках_ИТОГ);

                        /*        foreach (var korrItem in korrList)
                                {
                                    СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(korrItem);
                                }*/

                    }
                }
                catch { }


                //                var rsw67_list = rsw61.FormsRSW2014_1_Razd_6_7;
                var rsw67_list = razd67_list.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw61.ID).ToList();
                i = 1;
                k = 0;

                try
                {
                    foreach (var rsw67 in rsw67_list.Where(x => (x.s_0_0.HasValue && x.s_0_0.Value > 0) || (x.s_0_1.HasValue && x.s_0_1.Value > 0)))
                    {
                        XElement СведенияОсуммеВыплатИвознагражденийПоДопТарифу = new XElement("СведенияОсуммеВыплатИвознагражденийПоДопТарифу");

                        string code = rsw67.SpecOcenkaUslTrudaID.HasValue ? SpecOcenkaUslTruda_list.First(x => x.ID == rsw67.SpecOcenkaUslTrudaID).Code : "";


                        СведенияОсуммеВыплатИвознагражденийПоДопТарифу.Add(new XElement("НомерСтроки", i),
                                                                         new XElement("ТипСтроки", "ИТОГ"),
                                                                         new XElement("КодСтроки", "7" + k.ToString() + "0"),
                                                                         new XElement("КодСпециальнойОценкиУсловийТруда", code),
                                                                         new XElement("СуммаВыплатПоДопТарифу27-1", rsw67.s_0_0.HasValue ? Utils.decToStr(rsw67.s_0_0.Value) : "0.00"),
                                                                         new XElement("СуммаВыплатПоДопТарифу27-2-18", rsw67.s_0_1.HasValue ? Utils.decToStr(rsw67.s_0_1.Value) : "0.00"));

                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОсуммеВыплатИвознагражденийПоДопТарифу);
                        i++;

                        for (int j = 1; j <= 3; j++)
                        {
                            string itemName = "s_" + j.ToString() + "_";

                            if ((rsw67.GetType().GetProperty(itemName + "0").GetValue(rsw67, null) != null && decimal.Parse(rsw67.GetType().GetProperty(itemName + "0").GetValue(rsw67, null).ToString()) != 0) || (rsw67.GetType().GetProperty(itemName + "1").GetValue(rsw67, null) != null && decimal.Parse(rsw67.GetType().GetProperty(itemName + "1").GetValue(rsw67, null).ToString()) != 0))
                            {
                                СведенияОсуммеВыплатИвознагражденийПоДопТарифу = new XElement("СведенияОсуммеВыплатИвознагражденийПоДопТарифу");

                                СведенияОсуммеВыплатИвознагражденийПоДопТарифу.Add(new XElement("НомерСтроки", i),
                                                                                 new XElement("ТипСтроки", "МЕСЦ"),
                                                                                 new XElement("Месяц", m + j - 1),
                                                                                 new XElement("КодСтроки", "7" + k.ToString() + j.ToString()),
                                                                                 new XElement("КодСпециальнойОценкиУсловийТруда", code),
                                                                                 new XElement("СуммаВыплатПоДопТарифу27-1", rsw67.GetType().GetProperty(itemName + "0").GetValue(rsw67, null) != null ? Utils.decToStr(decimal.Parse(rsw67.GetType().GetProperty(itemName + "0").GetValue(rsw67, null).ToString())) : "0.00"),
                                                                                 new XElement("СуммаВыплатПоДопТарифу27-2-18", rsw67.GetType().GetProperty(itemName + "1").GetValue(rsw67, null) != null ? Utils.decToStr(decimal.Parse(rsw67.GetType().GetProperty(itemName + "1").GetValue(rsw67, null).ToString())) : "0.00"));

                                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СведенияОсуммеВыплатИвознагражденийПоДопТарифу);
                                i++;
                            }
                        }

                        k++;
                    }
                }
                catch { }

                //                               var staj_osn_list = rsw61.StajOsn.OrderBy(x => x.Number.Value);

                var staj_osn_list = stajOsn_list.Where(x => x.FormsRSW2014_1_Razd_6_1_ID == rsw61.ID).OrderBy(x => x.Number.Value).ToList();

                var tt = staj_osn_list.Select(x => x.ID).ToList();
                stajLgot_list = stajLgot_list_t.Where(x => tt.Contains(x.StajOsnID)).ToList();
                i = 0;
                foreach (var staj_osn in staj_osn_list)
                {
                    try
                    {
                        i++;
                        XElement СтажевыйПериод = createStajElement(staj_osn, i);
                        СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(СтажевыйПериод);
                    }
                    catch { }

                }

                СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ.Add(new XElement("ДатаЗаполнения", rsw61.DateFilling.ToShortDateString()));


                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СУММЕ_ВЫПЛАТ_И_СТРАХОВОМ_СТАЖЕ_ЗЛ);
            }

            #endregion

            //      if (yearType == 2015)
            //          xDoc.Root.SetDefaultXmlNamespace(pfr);

            xDoc.Root.SetDefaultXmlNamespace(pfr);


            xml = xDoc.ToString();

            staffTempList.Clear();
            t = new List<long>();
            StaffList.Clear();

            rsw61List_tmp.Clear();
            t = new List<long>();

            stajOsn_list.Clear();
            stajLgot_list_t.Clear();

            razd64_list = new List<FormsRSW2014_1_Razd_6_4>();
            razd66_list = new List<FormsRSW2014_1_Razd_6_6>();
            razd67_list = new List<FormsRSW2014_1_Razd_6_7>();


            return xml;
        }


        private string generateXML_RSW2014(xmlInfo item)
        {
            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            string xml = "";
            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion.Substring(2, Application.ProductVersion.Length - 2))),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "Единичный запрос"))));


            string xName = "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014";
            if (yearType == 2015)
                xName = "РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2015";
            XElement РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014 = new XElement(xName,
                                                new XElement("НомерВпачке", 1),
                                                new XElement("РегистрационныйНомерПФР", Utils.ParseRegNum(ins.RegNum)),
                                                new XElement("НомерКорректировки", rsw.CorrectionNum.ToString().PadLeft(3, '0')),
                                                new XElement("КодОтчетногоПериода", rsw.Quarter.ToString()),
                                                new XElement("КалендарныйГод", rsw.Year.ToString()));

            if (rsw.WorkStop != 0)
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ПрекращениеДеятельности", "Л"));
            }

            if (rsw.CorrectionType != null)
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ТипКорректировки", rsw.CorrectionType.Value.ToString()));
            }

            if (ins.TypePayer == 0) // если организация
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("НаименованиеОрганизации", !String.IsNullOrEmpty(ins.Name) ? ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper() : !String.IsNullOrEmpty(ins.NameShort) ? ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper() : ""),
                    new XElement("ИННсимвольный", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
                if (!String.IsNullOrEmpty(ins.KPP))
                {
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                }
            }
            else // если физ. лицо
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ФИОфизическогоЛица"));

                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОфизическогоЛица").Add(new XElement("Фамилия", ins.LastName.ToUpper()));
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОфизическогоЛица").Add(new XElement("Имя", ins.FirstName.ToUpper()));
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОфизическогоЛица").Add(new XElement("Отчество", ins.MiddleName.ToUpper()));
                }
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ИННсимвольный", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));
            }

            if (ins.OKWED != null)
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КодПоОКВЭД", !String.IsNullOrEmpty(ins.OKWED.ToString()) ? ins.OKWED.ToString() : ""));
            }

            if (ins.PhoneContact != null && !String.IsNullOrEmpty(ins.PhoneContact))
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("Телефон", ins.PhoneContact.ToString().Substring(0, ins.PhoneContact.ToString().Length > 14 ? 14 : ins.PhoneContact.ToString().Length)));
            }

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КоличествоЗЛ", rsw.CountEmployers.ToString()));
            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("СреднесписочнаяЧисленность", rsw.CountAverageEmployers.ToString()));

            int cntPages = 2;

            cntPages = cntPages + db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum).Count();
            cntPages = cntPages + db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum).Count();

            if (db.FormsRSW2014_1_Razd_2_5_1.Any(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum) || db.FormsRSW2014_1_Razd_2_5_2.Any(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
                cntPages++;
            if (db.FormsRSW2014_1_Razd_4.Any(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
                cntPages++;
            if (db.FormsRSW2014_1_Razd_5.Any(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
                cntPages++;
            if ((rsw.ExistPart_2_2 != null && rsw.ExistPart_2_2.Value == 1) || (rsw.ExistPart_2_3 != null && rsw.ExistPart_2_3.Value == 1))
                cntPages++;
            if ((rsw.ExistPart_3_1 != null && rsw.ExistPart_3_1.Value == 1) || (rsw.ExistPart_3_2 != null && rsw.ExistPart_3_2.Value == 1))
                cntPages++;
            if ((rsw.ExistPart_3_4 != null && rsw.ExistPart_3_4.Value == 1) || (rsw.ExistPart_3_4 != null && rsw.ExistPart_3_4.Value == 1))
                cntPages++;
            if ((rsw.ExistPart_3_5 != null && rsw.ExistPart_3_5.Value == 1) || (rsw.ExistPart_3_6 != null && rsw.ExistPart_3_6.Value == 1))
                cntPages++;

            cntPages = cntPages + (int)rsw.CountEmployers.Value * 2;

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КоличествоСтраниц", cntPages.ToString()));

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("КоличествоЛистовПриложения", rsw.CountConfirmDoc.ToString()));

            //Для РСВ-1 2015
            //Приобретение/утрата права на применение пониженного тарифа. Заполняется при представлении Расчета за отчетный период, в котором приобретено или утрачено право на применение пониженного тарифа. В случае приобретения права на применение пониженного тарифа в поле проставляется буква «П», в случае утраты права на применение проставляется буква «У»
            /*          if (yearType == 2015 && !String.IsNullOrEmpty(rsw.ReducedRate))
                      {
                          РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ПравоНаПониженныйТариф", rsw.ReducedRate));
                      }
                      */
            // Разделы 1 - 5

            #region Раздел1РасчетПоНачисленнымУплаченным2014

            xName = "Раздел1РасчетПоНачисленнымУплаченным2014";
            //       if (yearType == 2015)
            //           xName = "Раздел1РасчетПоНачисленнымУплаченным";
            XElement Раздел1РасчетПоНачисленнымУплаченным2014 = new XElement(xName,
                                                                    new XElement(yearType != 2015 ? "ОстатокЗадолженностиНаНачалоРасчетногоПериода2014" : "ОстатокЗадолженностиНаНачалоРасчетногоПериода",
                                                                        new XElement("КодСтроки", "100"),
                                                                        new XElement("СтраховыеВзносыОПС", rsw.s_100_0.HasValue ? Utils.decToStr(rsw.s_100_0.Value) : "0.00"),
                                                                        new XElement("ОПСстраховаяЧасть", rsw.s_100_1.HasValue ? Utils.decToStr(rsw.s_100_1.Value) : "0.00"),
                                                                        new XElement("ОПСнакопительнаяЧасть", rsw.s_100_2.HasValue ? Utils.decToStr(rsw.s_100_2.Value) : "0.00"),
                                                                        new XElement("ВзносыПоДопТарифу1", rsw.s_100_3.HasValue ? Utils.decToStr(rsw.s_100_3.Value) : "0.00"),
                                                                        new XElement("ВзносыПоДопТарифу2_18", rsw.s_100_4.HasValue ? Utils.decToStr(rsw.s_100_4.Value) : "0.00"),
                                                                        new XElement("СтраховыеВзносыОМС", rsw.s_100_5.HasValue ? Utils.decToStr(rsw.s_100_5.Value) : "0.00")));

            xName = "НачисленоСначалаРасчетногоПериода2014";
            if (yearType == 2015)
                xName = "НачисленоСначалаРасчетногоПериода";
            XElement НачисленоСначалаРасчетногоПериода2014 = new XElement(xName,
                                                                new XElement(yearType != 2015 ? "ВсегоСначалаРасчетногоПериода2014" : "ВсегоСначалаРасчетногоПериода",
                                                                    new XElement("КодСтроки", "110"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_110_0.HasValue ? Utils.decToStr(rsw.s_110_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_110_3.HasValue ? Utils.decToStr(rsw.s_110_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_110_4.HasValue ? Utils.decToStr(rsw.s_110_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_110_5.HasValue ? Utils.decToStr(rsw.s_110_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца1с2014" : "ПоследниеТриМесяца1",
                                                                    new XElement("КодСтроки", "111"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_111_0.HasValue ? Utils.decToStr(rsw.s_111_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_111_3.HasValue ? Utils.decToStr(rsw.s_111_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_111_4.HasValue ? Utils.decToStr(rsw.s_111_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_111_5.HasValue ? Utils.decToStr(rsw.s_111_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца2с2014" : "ПоследниеТриМесяца2",
                                                                    new XElement("КодСтроки", "112"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_112_0.HasValue ? Utils.decToStr(rsw.s_112_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_112_3.HasValue ? Utils.decToStr(rsw.s_112_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_112_4.HasValue ? Utils.decToStr(rsw.s_112_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_112_5.HasValue ? Utils.decToStr(rsw.s_112_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца3с2014" : "ПоследниеТриМесяца3",
                                                                    new XElement("КодСтроки", "113"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_113_0.HasValue ? Utils.decToStr(rsw.s_113_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_113_3.HasValue ? Utils.decToStr(rsw.s_113_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_113_4.HasValue ? Utils.decToStr(rsw.s_113_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_113_5.HasValue ? Utils.decToStr(rsw.s_113_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяцаИтого2014" : "ПоследниеТриМесяцаИтого",
                                                                    new XElement("КодСтроки", "114"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_114_0.HasValue ? Utils.decToStr(rsw.s_114_0.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_114_3.HasValue ? Utils.decToStr(rsw.s_114_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_114_4.HasValue ? Utils.decToStr(rsw.s_114_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_114_5.HasValue ? Utils.decToStr(rsw.s_114_5.Value) : "0.00")));

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(НачисленоСначалаРасчетногоПериода2014);

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(new XElement(yearType != 2015 ? "ДоначисленоСначалаРасчетногоПериода2014всего" : "ДоначисленоСначалаРасчетногоПериодаВсего",
                                                             new XElement("КодСтроки", "120"),
                                                             new XElement("СтраховыеВзносыОПС", rsw.s_120_0.HasValue ? Utils.decToStr(rsw.s_120_0.Value) : "0.00"),
                                                             new XElement("ОПСстраховаяЧасть", rsw.s_120_1.HasValue ? Utils.decToStr(rsw.s_120_1.Value) : "0.00"),
                                                             new XElement("ОПСнакопительнаяЧасть", rsw.s_120_2.HasValue ? Utils.decToStr(rsw.s_120_2.Value) : "0.00"),
                                                             new XElement("ВзносыПоДопТарифу1", rsw.s_120_3.HasValue ? Utils.decToStr(rsw.s_120_3.Value) : "0.00"),
                                                             new XElement("ВзносыПоДопТарифу2_18", rsw.s_120_4.HasValue ? Utils.decToStr(rsw.s_120_4.Value) : "0.00"),
                                                             new XElement("СтраховыеВзносыОМС", rsw.s_120_5.HasValue ? Utils.decToStr(rsw.s_120_5.Value) : "0.00")));

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(new XElement(yearType != 2015 ? "ДоначисленоСначалаРасчетногоПериода2014превышающие" : "ДоначисленоСначалаРасчетногоПериодаПревышающие",
                                                             new XElement("КодСтроки", "121"),
                                                             new XElement("СтраховыеВзносыОПС", rsw.s_121_0.HasValue ? Utils.decToStr(rsw.s_121_0.Value) : "0.00"),
                                                             new XElement("ОПСстраховаяЧасть", rsw.s_121_1.HasValue ? Utils.decToStr(rsw.s_121_1.Value) : "0.00")));

            /*      if (yearType == 2015)
                  {
                      XElement ДоначисленоВпоследниеТриМесяца = new XElement("ДоначисленоВпоследниеТриМесяца");
                      ДоначисленоВпоследниеТриМесяца.Add(new XElement("ПоследниеТриМесяцаВсего",
                                                       new XElement("КодСтроки", "122"),
                                                       new XElement("СтраховыеВзносыОПС", rsw.s_122_0.HasValue ? Utils.decToStr(rsw.s_122_0.Value) : "0.00"),
                                                       new XElement("ОПСстраховаяЧасть", rsw.s_122_1.HasValue ? Utils.decToStr(rsw.s_122_1.Value) : "0.00"),
                                                       new XElement("ОПСнакопительнаяЧасть", rsw.s_122_2.HasValue ? Utils.decToStr(rsw.s_122_2.Value) : "0.00"),
                                                       new XElement("ВзносыПоДопТарифу1", rsw.s_122_3.HasValue ? Utils.decToStr(rsw.s_122_3.Value) : "0.00"),
                                                       new XElement("ВзносыПоДопТарифу2_18", rsw.s_122_4.HasValue ? Utils.decToStr(rsw.s_122_4.Value) : "0.00"),
                                                       new XElement("СтраховыеВзносыОМС", rsw.s_122_5.HasValue ? Utils.decToStr(rsw.s_122_5.Value) : "0.00")));

                      ДоначисленоВпоследниеТриМесяца.Add(new XElement("ПоследниеТриМесяцаПревышающие",
                                                                       new XElement("КодСтроки", "123"),
                                                                       new XElement("СтраховыеВзносыОПС", rsw.s_123_0.HasValue ? Utils.decToStr(rsw.s_123_0.Value) : "0.00"),
                                                                       new XElement("ОПСстраховаяЧасть", rsw.s_123_1.HasValue ? Utils.decToStr(rsw.s_123_1.Value) : "0.00")));


                      Раздел1РасчетПоНачисленнымУплаченным2014.Add(ДоначисленоВпоследниеТриМесяца);
                  }
      */



            Раздел1РасчетПоНачисленнымУплаченным2014.Add(new XElement(yearType != 2015 ? "ВсегоКуплате2014" : "ВсегоКуплате",
                                                              new XElement("КодСтроки", "130"),
                                                              new XElement("СтраховыеВзносыОПС", rsw.s_130_0.HasValue ? Utils.decToStr(rsw.s_130_0.Value) : "0.00"),
                                                              new XElement("ОПСстраховаяЧасть", rsw.s_130_1.HasValue ? Utils.decToStr(rsw.s_130_1.Value) : "0.00"),
                                                              new XElement("ОПСнакопительнаяЧасть", rsw.s_130_2.HasValue ? Utils.decToStr(rsw.s_130_2.Value) : "0.00"),
                                                              new XElement("ВзносыПоДопТарифу1", rsw.s_130_3.HasValue ? Utils.decToStr(rsw.s_130_3.Value) : "0.00"),
                                                              new XElement("ВзносыПоДопТарифу2_18", rsw.s_130_4.HasValue ? Utils.decToStr(rsw.s_130_4.Value) : "0.00"),
                                                              new XElement("СтраховыеВзносыОМС", rsw.s_130_5.HasValue ? Utils.decToStr(rsw.s_130_5.Value) : "0.00")));

            XElement УплаченоСначалаРасчетногоПериода2014 = new XElement(yearType != 2015 ? "УплаченоСначалаРасчетногоПериода2014" : "УплаченоСначалаРасчетногоПериода",
                                                                new XElement(yearType != 2015 ? "ВсегоСначалаРасчетногоПериода2014" : "ВсегоСначалаРасчетногоПериода",
                                                                    new XElement("КодСтроки", "140"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_140_0.HasValue ? Utils.decToStr(rsw.s_140_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_140_1.HasValue ? Utils.decToStr(rsw.s_140_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_140_2.HasValue ? Utils.decToStr(rsw.s_140_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_140_3.HasValue ? Utils.decToStr(rsw.s_140_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_140_4.HasValue ? Utils.decToStr(rsw.s_140_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_140_5.HasValue ? Utils.decToStr(rsw.s_140_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца1с2014" : "ПоследниеТриМесяца1",
                                                                    new XElement("КодСтроки", "141"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_141_0.HasValue ? Utils.decToStr(rsw.s_141_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_141_1.HasValue ? Utils.decToStr(rsw.s_141_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_141_2.HasValue ? Utils.decToStr(rsw.s_141_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_141_3.HasValue ? Utils.decToStr(rsw.s_141_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_141_4.HasValue ? Utils.decToStr(rsw.s_141_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_141_5.HasValue ? Utils.decToStr(rsw.s_141_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца2с2014" : "ПоследниеТриМесяца2",
                                                                    new XElement("КодСтроки", "142"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_142_0.HasValue ? Utils.decToStr(rsw.s_142_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_142_1.HasValue ? Utils.decToStr(rsw.s_142_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_142_2.HasValue ? Utils.decToStr(rsw.s_142_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_142_3.HasValue ? Utils.decToStr(rsw.s_142_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_142_4.HasValue ? Utils.decToStr(rsw.s_142_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_142_5.HasValue ? Utils.decToStr(rsw.s_142_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяца3с2014" : "ПоследниеТриМесяца3",
                                                                    new XElement("КодСтроки", "143"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_143_0.HasValue ? Utils.decToStr(rsw.s_143_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_143_1.HasValue ? Utils.decToStr(rsw.s_143_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_143_2.HasValue ? Utils.decToStr(rsw.s_143_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_143_3.HasValue ? Utils.decToStr(rsw.s_143_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_143_4.HasValue ? Utils.decToStr(rsw.s_143_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_143_5.HasValue ? Utils.decToStr(rsw.s_143_5.Value) : "0.00")),
                                                                new XElement(yearType != 2015 ? "ПоследниеТриМесяцаИтого2014" : "ПоследниеТриМесяцаИтого",
                                                                    new XElement("КодСтроки", "144"),
                                                                    new XElement("СтраховыеВзносыОПС", rsw.s_144_0.HasValue ? Utils.decToStr(rsw.s_144_0.Value) : "0.00"),
                                                                    new XElement("ОПСстраховаяЧасть", rsw.s_144_1.HasValue ? Utils.decToStr(rsw.s_144_1.Value) : "0.00"),
                                                                    new XElement("ОПСнакопительнаяЧасть", rsw.s_144_2.HasValue ? Utils.decToStr(rsw.s_144_2.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу1", rsw.s_144_3.HasValue ? Utils.decToStr(rsw.s_144_3.Value) : "0.00"),
                                                                    new XElement("ВзносыПоДопТарифу2_18", rsw.s_144_4.HasValue ? Utils.decToStr(rsw.s_144_4.Value) : "0.00"),
                                                                    new XElement("СтраховыеВзносыОМС", rsw.s_144_5.HasValue ? Utils.decToStr(rsw.s_144_5.Value) : "0.00")));

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(УплаченоСначалаРасчетногоПериода2014);

            Раздел1РасчетПоНачисленнымУплаченным2014.Add(new XElement(yearType != 2015 ? "ОстатокЗадолженностиНаКонецРасчетногоПериода2014" : "ОстатокЗадолженностиНаКонецРасчетногоПериода",
                                                              new XElement("КодСтроки", "150"),
                                                              new XElement("СтраховыеВзносыОПС", rsw.s_150_0.HasValue ? Utils.decToStr(rsw.s_150_0.Value) : "0.00"),
                                                              new XElement("ОПСстраховаяЧасть", rsw.s_150_1.HasValue ? Utils.decToStr(rsw.s_150_1.Value) : "0.00"),
                                                              new XElement("ОПСнакопительнаяЧасть", rsw.s_150_2.HasValue ? Utils.decToStr(rsw.s_150_2.Value) : "0.00"),
                                                              new XElement("ВзносыПоДопТарифу1", rsw.s_150_3.HasValue ? Utils.decToStr(rsw.s_150_3.Value) : "0.00"),
                                                              new XElement("ВзносыПоДопТарифу2_18", rsw.s_150_4.HasValue ? Utils.decToStr(rsw.s_150_4.Value) : "0.00"),
                                                              new XElement("СтраховыеВзносыОМС", rsw.s_150_5.HasValue ? Utils.decToStr(rsw.s_150_5.Value) : "0.00")));


            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел1РасчетПоНачисленнымУплаченным2014);

            #endregion

            #region Раздел2РасчетПоТарифуИдопТарифу

            XElement Раздел2РасчетПоТарифуИдопТарифу = new XElement("Раздел2РасчетПоТарифуИдопТарифу");

            #region Раздел_2_1

            var RSW_2_1_List = db.FormsRSW2014_1_Razd_2_1.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.TariffCode.Code).ToList();

            foreach (var rsw21 in RSW_2_1_List)
            {
                XElement Раздел_2_1 = new XElement("Раздел_2_1",
                                          new XElement("КодТарифа", rsw21.TariffCode.Code.ToUpper()));

                XElement НаОбязательноеПенсионноеСтрахование2014 = new XElement((yearType != 2015 ? "НаОбязательноеПенсионноеСтрахование2014" : "НаОбязательноеПенсионноеСтрахование"), "");
                НаОбязательноеПенсионноеСтрахование2014.Add(new XElement("СуммаВыплатИвознагражденийОПС",
                                                                new XElement("КодСтроки", "200"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_200_0.HasValue ? Utils.decToStr(rsw21.s_200_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_200_1.HasValue ? Utils.decToStr(rsw21.s_200_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_200_2.HasValue ? Utils.decToStr(rsw21.s_200_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_200_3.HasValue ? Utils.decToStr(rsw21.s_200_3.Value) : "0.00"))),
                                                            new XElement("НеПодлежащиеОбложениюОПС",
                                                                new XElement("КодСтроки", "201"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_201_0.HasValue ? Utils.decToStr(rsw21.s_201_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_201_1.HasValue ? Utils.decToStr(rsw21.s_201_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_201_2.HasValue ? Utils.decToStr(rsw21.s_201_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_201_3.HasValue ? Utils.decToStr(rsw21.s_201_3.Value) : "0.00"))),
                                                            new XElement("СуммаРасходовПринимаемыхКвычетуОПС",
                                                                new XElement("КодСтроки", "202"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_202_0.HasValue ? Utils.decToStr(rsw21.s_202_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_202_1.HasValue ? Utils.decToStr(rsw21.s_202_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_202_2.HasValue ? Utils.decToStr(rsw21.s_202_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_202_3.HasValue ? Utils.decToStr(rsw21.s_202_3.Value) : "0.00"))),
                                                            new XElement("ПревышающиеПредельнуюВеличинуБазыОПС",
                                                                new XElement("КодСтроки", "203"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_203_0.HasValue ? Utils.decToStr(rsw21.s_203_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_203_1.HasValue ? Utils.decToStr(rsw21.s_203_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_203_2.HasValue ? Utils.decToStr(rsw21.s_203_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_203_3.HasValue ? Utils.decToStr(rsw21.s_203_3.Value) : "0.00"))),
                                                            new XElement("БазаДляНачисленияСтраховыхВзносовНаОПС",
                                                                new XElement("КодСтроки", "204"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_204_0.HasValue ? Utils.decToStr(rsw21.s_204_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_204_1.HasValue ? Utils.decToStr(rsw21.s_204_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_204_2.HasValue ? Utils.decToStr(rsw21.s_204_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_204_3.HasValue ? Utils.decToStr(rsw21.s_204_3.Value) : "0.00"))),
                                                            new XElement("НачисленоНаОПСсСуммНеПревышающих",
                                                                new XElement("КодСтроки", "205"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_205_0.HasValue ? Utils.decToStr(rsw21.s_205_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_205_1.HasValue ? Utils.decToStr(rsw21.s_205_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_205_2.HasValue ? Utils.decToStr(rsw21.s_205_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_205_3.HasValue ? Utils.decToStr(rsw21.s_205_3.Value) : "0.00"))),
                                                            new XElement("НачисленоНаОПСсСуммПревышающих",
                                                                new XElement("КодСтроки", "206"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_206_0.HasValue ? Utils.decToStr(rsw21.s_206_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_206_1.HasValue ? Utils.decToStr(rsw21.s_206_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_206_2.HasValue ? Utils.decToStr(rsw21.s_206_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_206_3.HasValue ? Utils.decToStr(rsw21.s_206_3.Value) : "0.00"))),
                                                            new XElement("КоличествоФЛвсего",
                                                                new XElement("КодСтроки", "207"),
                                                                new XElement("КоличествоЗЛ_Всего", rsw21.s_207_0.HasValue ? rsw21.s_207_0.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_1месяц", rsw21.s_207_1.HasValue ? rsw21.s_207_1.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_2месяц", rsw21.s_207_2.HasValue ? rsw21.s_207_2.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_3месяц", rsw21.s_207_3.HasValue ? rsw21.s_207_3.Value.ToString() : "0")),
                                                            new XElement("КоличествоФЛпоКоторымБазаПревысилаПредел",
                                                                new XElement("КодСтроки", "208"),
                                                                new XElement("КоличествоЗЛ_Всего", rsw21.s_208_0.HasValue ? rsw21.s_208_0.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_1месяц", rsw21.s_208_1.HasValue ? rsw21.s_208_1.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_2месяц", rsw21.s_208_2.HasValue ? rsw21.s_208_2.Value.ToString() : "0"),
                                                                new XElement("КоличествоЗЛ_3месяц", rsw21.s_208_3.HasValue ? rsw21.s_208_3.Value.ToString() : "0")));

                Раздел_2_1.Add(НаОбязательноеПенсионноеСтрахование2014);

                XElement НаОбязательноеМедицинскоеСтрахование = new XElement("НаОбязательноеМедицинскоеСтрахование");
                НаОбязательноеМедицинскоеСтрахование.Add(new XElement("СуммаВыплатИвознаграждений",
                                                                new XElement("КодСтроки", "210"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_210_0.HasValue ? Utils.decToStr(rsw21.s_210_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_210_1.HasValue ? Utils.decToStr(rsw21.s_210_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_210_2.HasValue ? Utils.decToStr(rsw21.s_210_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_210_3.HasValue ? Utils.decToStr(rsw21.s_210_3.Value) : "0.00"))),
                                                            new XElement("НеПодлежащиеОбложению",
                                                                new XElement("КодСтроки", "211"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_211_0.HasValue ? Utils.decToStr(rsw21.s_211_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_211_1.HasValue ? Utils.decToStr(rsw21.s_211_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_211_2.HasValue ? Utils.decToStr(rsw21.s_211_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_211_3.HasValue ? Utils.decToStr(rsw21.s_211_3.Value) : "0.00"))),
                                                            new XElement("СуммаРасходовПринимаемыхКвычету",
                                                                new XElement("КодСтроки", "212"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_212_0.HasValue ? Utils.decToStr(rsw21.s_212_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_212_1.HasValue ? Utils.decToStr(rsw21.s_212_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_212_2.HasValue ? Utils.decToStr(rsw21.s_212_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_212_3.HasValue ? Utils.decToStr(rsw21.s_212_3.Value) : "0.00"))));

                if (yearType == 2014 || yearType == 2012)
                {

                    НаОбязательноеМедицинскоеСтрахование.Add(new XElement("ПревышающиеПредельнуюВеличинуБазы",
                                                                new XElement("КодСтроки", "213"),
                                                                new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_213_0.HasValue ? Utils.decToStr(rsw21.s_213_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_213_1.HasValue ? Utils.decToStr(rsw21.s_213_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_213_2.HasValue ? Utils.decToStr(rsw21.s_213_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_213_3.HasValue ? Utils.decToStr(rsw21.s_213_3.Value) : "0.00"))),
                                                             new XElement("БазаДляНачисленияСтраховыхВзносовНаОМС",
                                                                 new XElement("КодСтроки", "214"),
                                                                 new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_214_0.HasValue ? Utils.decToStr(rsw21.s_214_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_214_1.HasValue ? Utils.decToStr(rsw21.s_214_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_214_2.HasValue ? Utils.decToStr(rsw21.s_214_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_214_3.HasValue ? Utils.decToStr(rsw21.s_214_3.Value) : "0.00"))),
                                                             new XElement("НачисленоНаОМС",
                                                                 new XElement("КодСтроки", "215"),
                                                                 new XElement("РасчетСумм",
                                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_215_0.HasValue ? Utils.decToStr(rsw21.s_215_0.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние1месяц", rsw21.s_215_1.HasValue ? Utils.decToStr(rsw21.s_215_1.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние2месяц", rsw21.s_215_2.HasValue ? Utils.decToStr(rsw21.s_215_2.Value) : "0.00"),
                                                                    new XElement("СуммаПоследние3месяц", rsw21.s_215_3.HasValue ? Utils.decToStr(rsw21.s_215_3.Value) : "0.00"))));
                }
                if (yearType == 2015)
                {

                    НаОбязательноеМедицинскоеСтрахование.Add(new XElement("БазаДляНачисленияСтраховыхВзносовНаОМС",
                                                                 new XElement("КодСтроки", "213"),
                                                                    new XElement("РасчетСумм",
                                                                        new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_213_0.HasValue ? Utils.decToStr(rsw21.s_213_0.Value) : "0.00"),
                                                                        new XElement("СуммаПоследние1месяц", rsw21.s_213_1.HasValue ? Utils.decToStr(rsw21.s_213_1.Value) : "0.00"),
                                                                        new XElement("СуммаПоследние2месяц", rsw21.s_213_2.HasValue ? Utils.decToStr(rsw21.s_213_2.Value) : "0.00"),
                                                                        new XElement("СуммаПоследние3месяц", rsw21.s_213_3.HasValue ? Utils.decToStr(rsw21.s_213_3.Value) : "0.00"))),
                                                                 new XElement("НачисленоНаОМС",
                                                                     new XElement("КодСтроки", "214"),
                                                                     new XElement("РасчетСумм",
                                                                         new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw21.s_214_0.HasValue ? Utils.decToStr(rsw21.s_214_0.Value) : "0.00"),
                                                                         new XElement("СуммаПоследние1месяц", rsw21.s_214_1.HasValue ? Utils.decToStr(rsw21.s_214_1.Value) : "0.00"),
                                                                         new XElement("СуммаПоследние2месяц", rsw21.s_214_2.HasValue ? Utils.decToStr(rsw21.s_214_2.Value) : "0.00"),
                                                                         new XElement("СуммаПоследние3месяц", rsw21.s_214_3.HasValue ? Utils.decToStr(rsw21.s_214_3.Value) : "0.00"))),
                                                                new XElement("КоличествоФЛвсего",
                                                                    new XElement("КодСтроки", "215"),
                                                                    new XElement("КоличествоЗЛ_Всего", rsw21.s_215i_0.HasValue ? rsw21.s_215i_0.Value.ToString() : "0"),
                                                                    new XElement("КоличествоЗЛ_1месяц", rsw21.s_215i_1.HasValue ? rsw21.s_215i_1.Value.ToString() : "0"),
                                                                    new XElement("КоличествоЗЛ_2месяц", rsw21.s_215i_2.HasValue ? rsw21.s_215i_2.Value.ToString() : "0"),
                                                                    new XElement("КоличествоЗЛ_3месяц", rsw21.s_215i_3.HasValue ? rsw21.s_215i_3.Value.ToString() : "0")));
                }

                Раздел_2_1.Add(НаОбязательноеМедицинскоеСтрахование);

                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_1);
            }

            #endregion

            #region Раздел_2_2

            if (rsw.ExistPart_2_2.HasValue && rsw.ExistPart_2_2.Value == 1)
            {
                XElement Раздел_2_2 = new XElement("Раздел_2_2");

                Раздел_2_2.Add(new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                   new XElement("КодСтроки", "220"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_220_0.HasValue ? Utils.decToStr(rsw.s_220_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_220_1.HasValue ? Utils.decToStr(rsw.s_220_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_220_2.HasValue ? Utils.decToStr(rsw.s_220_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_220_3.HasValue ? Utils.decToStr(rsw.s_220_3.Value) : "0.00"))),
                               new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                   new XElement("КодСтроки", "221"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_221_0.HasValue ? Utils.decToStr(rsw.s_221_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_221_1.HasValue ? Utils.decToStr(rsw.s_221_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_221_2.HasValue ? Utils.decToStr(rsw.s_221_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_221_3.HasValue ? Utils.decToStr(rsw.s_221_3.Value) : "0.00"))),
                               new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                   new XElement("КодСтроки", "223"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_223_0.HasValue ? Utils.decToStr(rsw.s_223_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_223_1.HasValue ? Utils.decToStr(rsw.s_223_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_223_2.HasValue ? Utils.decToStr(rsw.s_223_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_223_3.HasValue ? Utils.decToStr(rsw.s_223_3.Value) : "0.00"))),
                               new XElement("НачисленоПоДопТарифу",
                                   new XElement("КодСтроки", "224"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_224_0.HasValue ? Utils.decToStr(rsw.s_224_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_224_1.HasValue ? Utils.decToStr(rsw.s_224_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_224_2.HasValue ? Utils.decToStr(rsw.s_224_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_224_3.HasValue ? Utils.decToStr(rsw.s_224_3.Value) : "0.00"))),
                               new XElement("КоличествоФЛпоДопТарифу",
                                   new XElement("КодСтроки", "225"),
                                   new XElement("КоличествоЗЛ_Всего", rsw.s_225_0.HasValue ? rsw.s_225_0.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_1месяц", rsw.s_225_1.HasValue ? rsw.s_225_1.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_2месяц", rsw.s_225_2.HasValue ? rsw.s_225_2.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_3месяц", rsw.s_225_3.HasValue ? rsw.s_225_3.Value.ToString() : "0")));

                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_2);
            }
            #endregion

            #region Раздел_2_3

            if (rsw.ExistPart_2_3.HasValue && rsw.ExistPart_2_3.Value == 1)
            {
                XElement Раздел_2_3 = new XElement("Раздел_2_3");

                Раздел_2_3.Add(new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                   new XElement("КодСтроки", "230"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_230_0.HasValue ? Utils.decToStr(rsw.s_230_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_230_1.HasValue ? Utils.decToStr(rsw.s_230_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_230_2.HasValue ? Utils.decToStr(rsw.s_230_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_230_3.HasValue ? Utils.decToStr(rsw.s_230_3.Value) : "0.00"))),
                               new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                   new XElement("КодСтроки", "231"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_231_0.HasValue ? Utils.decToStr(rsw.s_231_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_231_1.HasValue ? Utils.decToStr(rsw.s_231_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_231_2.HasValue ? Utils.decToStr(rsw.s_231_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_231_3.HasValue ? Utils.decToStr(rsw.s_231_3.Value) : "0.00"))),
                               new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                   new XElement("КодСтроки", "233"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_233_0.HasValue ? Utils.decToStr(rsw.s_233_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_233_1.HasValue ? Utils.decToStr(rsw.s_233_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_233_2.HasValue ? Utils.decToStr(rsw.s_233_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_233_3.HasValue ? Utils.decToStr(rsw.s_233_3.Value) : "0.00"))),
                               new XElement("НачисленоПоДопТарифу",
                                   new XElement("КодСтроки", "234"),
                                   new XElement("РасчетСумм",
                                       new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_234_0.HasValue ? Utils.decToStr(rsw.s_234_0.Value) : "0.00"),
                                       new XElement("СуммаПоследние1месяц", rsw.s_234_1.HasValue ? Utils.decToStr(rsw.s_234_1.Value) : "0.00"),
                                       new XElement("СуммаПоследние2месяц", rsw.s_234_2.HasValue ? Utils.decToStr(rsw.s_234_2.Value) : "0.00"),
                                       new XElement("СуммаПоследние3месяц", rsw.s_234_3.HasValue ? Utils.decToStr(rsw.s_234_3.Value) : "0.00"))),
                               new XElement("КоличествоФЛпоДопТарифу",
                                   new XElement("КодСтроки", "235"),
                                   new XElement("КоличествоЗЛ_Всего", rsw.s_235_0.HasValue ? rsw.s_235_0.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_1месяц", rsw.s_235_1.HasValue ? rsw.s_235_1.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_2месяц", rsw.s_235_2.HasValue ? rsw.s_235_2.Value.ToString() : "0"),
                                   new XElement("КоличествоЗЛ_3месяц", rsw.s_235_3.HasValue ? rsw.s_235_3.Value.ToString() : "0")));

                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_3);
            }

            #endregion

            #region Раздел_2_4

            var RSW_2_4_List = db.FormsRSW2014_1_Razd_2_4.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.ID).ToList();

            foreach (var rsw24 in RSW_2_4_List)
            {
                string filledBase = "";
                switch (rsw24.FilledBase.Value)
                {
                    case 0: filledBase = "РЕЗУЛЬТАТЫ СПЕЦОЦЕНКИ";
                        break;
                    case 1: filledBase = "РЕЗУЛЬТАТЫ АТТЕСТАЦИИ РАБОЧИХ МЕСТ";
                        break;
                    case 2: filledBase = "РЕЗУЛЬТАТЫ СПЕЦОЦЕНКИ И РЕЗУЛЬТАТЫ АТТЕСТАЦИИ РАБОЧИХ МЕСТ";
                        break;
                }

                XElement Раздел_2_4 = new XElement("Раздел_2_4",
                                          new XElement("КодОснованияРасчетаПоДопТарифу", rsw24.CodeBase.Value),
                                          new XElement("ОснованиеЗаполненияРаздела2_4", filledBase));

                if ((rsw24.s_243_0.HasValue && rsw24.s_243_0.Value != 0) || (rsw24.s_243_1.HasValue && rsw24.s_243_1.Value != 0) || (rsw24.s_243_2.HasValue && rsw24.s_243_2.Value != 0) || (rsw24.s_243_3.HasValue && rsw24.s_243_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                       new XElement("КодСпециальнойОценкиУсловийТруда", "О4"),
                                       new XElement("РасчетНачисленныхПоДопТарифу",
                                           new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                               new XElement("КодСтроки", "240"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_240_0.HasValue ? Utils.decToStr(rsw24.s_240_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_240_1.HasValue ? Utils.decToStr(rsw24.s_240_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_240_2.HasValue ? Utils.decToStr(rsw24.s_240_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_240_3.HasValue ? Utils.decToStr(rsw24.s_240_3.Value) : "0.00"))),
                                           new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                               new XElement("КодСтроки", "241"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_241_0.HasValue ? Utils.decToStr(rsw24.s_241_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_241_1.HasValue ? Utils.decToStr(rsw24.s_241_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_241_2.HasValue ? Utils.decToStr(rsw24.s_241_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_241_3.HasValue ? Utils.decToStr(rsw24.s_241_3.Value) : "0.00"))),
                                           new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                               new XElement("КодСтроки", "243"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_243_0.HasValue ? Utils.decToStr(rsw24.s_243_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_243_1.HasValue ? Utils.decToStr(rsw24.s_243_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_243_2.HasValue ? Utils.decToStr(rsw24.s_243_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_243_3.HasValue ? Utils.decToStr(rsw24.s_243_3.Value) : "0.00"))),
                                           new XElement("НачисленоПоДопТарифу",
                                               new XElement("КодСтроки", "244"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_244_0.HasValue ? Utils.decToStr(rsw24.s_244_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_244_1.HasValue ? Utils.decToStr(rsw24.s_244_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_244_2.HasValue ? Utils.decToStr(rsw24.s_244_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_244_3.HasValue ? Utils.decToStr(rsw24.s_244_3.Value) : "0.00"))),
                                           new XElement("КоличествоФЛпоДопТарифу",
                                               new XElement("КодСтроки", "245"),
                                               new XElement("КоличествоЗЛ_Всего", rsw24.s_245_0.HasValue ? rsw24.s_245_0.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_1месяц", rsw24.s_245_1.HasValue ? rsw24.s_245_1.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_2месяц", rsw24.s_245_2.HasValue ? rsw24.s_245_2.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_3месяц", rsw24.s_245_3.HasValue ? rsw24.s_245_3.Value.ToString() : "0")))));
                }

                if ((rsw24.s_249_0.HasValue && rsw24.s_249_0.Value != 0) || (rsw24.s_249_1.HasValue && rsw24.s_249_1.Value != 0) || (rsw24.s_249_2.HasValue && rsw24.s_249_2.Value != 0) || (rsw24.s_249_3.HasValue && rsw24.s_249_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                       new XElement("КодСпециальнойОценкиУсловийТруда", "В3.4"),
                                       new XElement("РасчетНачисленныхПоДопТарифу",
                                           new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                               new XElement("КодСтроки", "246"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_246_0.HasValue ? Utils.decToStr(rsw24.s_246_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_246_1.HasValue ? Utils.decToStr(rsw24.s_246_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_246_2.HasValue ? Utils.decToStr(rsw24.s_246_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_246_3.HasValue ? Utils.decToStr(rsw24.s_246_3.Value) : "0.00"))),
                                           new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                               new XElement("КодСтроки", "247"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_247_0.HasValue ? Utils.decToStr(rsw24.s_247_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_247_1.HasValue ? Utils.decToStr(rsw24.s_247_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_247_2.HasValue ? Utils.decToStr(rsw24.s_247_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_247_3.HasValue ? Utils.decToStr(rsw24.s_247_3.Value) : "0.00"))),
                                           new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                               new XElement("КодСтроки", "249"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_249_0.HasValue ? Utils.decToStr(rsw24.s_249_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_249_1.HasValue ? Utils.decToStr(rsw24.s_249_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_249_2.HasValue ? Utils.decToStr(rsw24.s_249_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_249_3.HasValue ? Utils.decToStr(rsw24.s_249_3.Value) : "0.00"))),
                                           new XElement("НачисленоПоДопТарифу",
                                               new XElement("КодСтроки", "250"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_250_0.HasValue ? Utils.decToStr(rsw24.s_250_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_250_1.HasValue ? Utils.decToStr(rsw24.s_250_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_250_2.HasValue ? Utils.decToStr(rsw24.s_250_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_250_3.HasValue ? Utils.decToStr(rsw24.s_250_3.Value) : "0.00"))),
                                           new XElement("КоличествоФЛпоДопТарифу",
                                               new XElement("КодСтроки", "251"),
                                               new XElement("КоличествоЗЛ_Всего", rsw24.s_251_0.HasValue ? rsw24.s_251_0.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_1месяц", rsw24.s_251_1.HasValue ? rsw24.s_251_1.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_2месяц", rsw24.s_251_2.HasValue ? rsw24.s_251_2.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_3месяц", rsw24.s_251_3.HasValue ? rsw24.s_251_3.Value.ToString() : "0")))));
                }

                if ((rsw24.s_255_0.HasValue && rsw24.s_255_0.Value != 0) || (rsw24.s_255_1.HasValue && rsw24.s_255_1.Value != 0) || (rsw24.s_255_2.HasValue && rsw24.s_255_2.Value != 0) || (rsw24.s_255_3.HasValue && rsw24.s_255_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                       new XElement("КодСпециальнойОценкиУсловийТруда", "В3.3"),
                                       new XElement("РасчетНачисленныхПоДопТарифу",
                                           new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                               new XElement("КодСтроки", "252"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_252_0.HasValue ? Utils.decToStr(rsw24.s_252_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_252_1.HasValue ? Utils.decToStr(rsw24.s_252_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_252_2.HasValue ? Utils.decToStr(rsw24.s_252_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_252_3.HasValue ? Utils.decToStr(rsw24.s_252_3.Value) : "0.00"))),
                                           new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                               new XElement("КодСтроки", "253"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_253_0.HasValue ? Utils.decToStr(rsw24.s_253_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_253_1.HasValue ? Utils.decToStr(rsw24.s_253_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_253_2.HasValue ? Utils.decToStr(rsw24.s_253_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_253_3.HasValue ? Utils.decToStr(rsw24.s_253_3.Value) : "0.00"))),
                                           new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                               new XElement("КодСтроки", "255"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_255_0.HasValue ? Utils.decToStr(rsw24.s_255_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_255_1.HasValue ? Utils.decToStr(rsw24.s_255_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_255_2.HasValue ? Utils.decToStr(rsw24.s_255_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_255_3.HasValue ? Utils.decToStr(rsw24.s_255_3.Value) : "0.00"))),
                                           new XElement("НачисленоПоДопТарифу",
                                               new XElement("КодСтроки", "256"),
                                               new XElement("РасчетСумм",
                                                   new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_256_0.HasValue ? Utils.decToStr(rsw24.s_256_0.Value) : "0.00"),
                                                   new XElement("СуммаПоследние1месяц", rsw24.s_256_1.HasValue ? Utils.decToStr(rsw24.s_256_1.Value) : "0.00"),
                                                   new XElement("СуммаПоследние2месяц", rsw24.s_256_2.HasValue ? Utils.decToStr(rsw24.s_256_2.Value) : "0.00"),
                                                   new XElement("СуммаПоследние3месяц", rsw24.s_256_3.HasValue ? Utils.decToStr(rsw24.s_256_3.Value) : "0.00"))),
                                           new XElement("КоличествоФЛпоДопТарифу",
                                               new XElement("КодСтроки", "257"),
                                               new XElement("КоличествоЗЛ_Всего", rsw24.s_257_0.HasValue ? rsw24.s_257_0.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_1месяц", rsw24.s_257_1.HasValue ? rsw24.s_257_1.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_2месяц", rsw24.s_257_2.HasValue ? rsw24.s_257_2.Value.ToString() : "0"),
                                               new XElement("КоличествоЗЛ_3месяц", rsw24.s_257_3.HasValue ? rsw24.s_257_3.Value.ToString() : "0")))));
                }

                if ((rsw24.s_261_0.HasValue && rsw24.s_261_0.Value != 0) || (rsw24.s_261_1.HasValue && rsw24.s_261_1.Value != 0) || (rsw24.s_261_2.HasValue && rsw24.s_261_2.Value != 0) || (rsw24.s_261_3.HasValue && rsw24.s_261_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                        new XElement("КодСпециальнойОценкиУсловийТруда", "В3.2"),
                                        new XElement("РасчетНачисленныхПоДопТарифу",
                                            new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                                new XElement("КодСтроки", "258"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_258_0.HasValue ? Utils.decToStr(rsw24.s_258_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_258_1.HasValue ? Utils.decToStr(rsw24.s_258_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_258_2.HasValue ? Utils.decToStr(rsw24.s_258_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_258_3.HasValue ? Utils.decToStr(rsw24.s_258_3.Value) : "0.00"))),
                                            new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                                new XElement("КодСтроки", "259"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_259_0.HasValue ? Utils.decToStr(rsw24.s_259_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_259_1.HasValue ? Utils.decToStr(rsw24.s_259_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_259_2.HasValue ? Utils.decToStr(rsw24.s_259_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_259_3.HasValue ? Utils.decToStr(rsw24.s_259_3.Value) : "0.00"))),
                                            new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                                new XElement("КодСтроки", "261"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_261_0.HasValue ? Utils.decToStr(rsw24.s_261_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_261_1.HasValue ? Utils.decToStr(rsw24.s_261_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_261_2.HasValue ? Utils.decToStr(rsw24.s_261_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_261_3.HasValue ? Utils.decToStr(rsw24.s_261_3.Value) : "0.00"))),
                                            new XElement("НачисленоПоДопТарифу",
                                                new XElement("КодСтроки", "262"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_262_0.HasValue ? Utils.decToStr(rsw24.s_262_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_262_1.HasValue ? Utils.decToStr(rsw24.s_262_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_262_2.HasValue ? Utils.decToStr(rsw24.s_262_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_262_3.HasValue ? Utils.decToStr(rsw24.s_262_3.Value) : "0.00"))),
                                            new XElement("КоличествоФЛпоДопТарифу",
                                                new XElement("КодСтроки", "263"),
                                                new XElement("КоличествоЗЛ_Всего", rsw24.s_263_0.HasValue ? rsw24.s_263_0.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_1месяц", rsw24.s_263_1.HasValue ? rsw24.s_263_1.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_2месяц", rsw24.s_263_2.HasValue ? rsw24.s_263_2.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_3месяц", rsw24.s_263_3.HasValue ? rsw24.s_263_3.Value.ToString() : "0")))));
                }

                if ((rsw24.s_267_0.HasValue && rsw24.s_267_0.Value != 0) || (rsw24.s_267_1.HasValue && rsw24.s_267_1.Value != 0) || (rsw24.s_267_2.HasValue && rsw24.s_267_2.Value != 0) || (rsw24.s_267_3.HasValue && rsw24.s_267_3.Value != 0))
                {
                    Раздел_2_4.Add(new XElement("РасчетНачисленныхПоКодуСпецОценкиУТ",
                                        new XElement("КодСпециальнойОценкиУсловийТруда", "В3.1"),
                                        new XElement("РасчетНачисленныхПоДопТарифу",
                                            new XElement("СуммаВыплатИвознагражденийПоДопТарифу",
                                                new XElement("КодСтроки", "264"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_264_0.HasValue ? Utils.decToStr(rsw24.s_264_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_264_1.HasValue ? Utils.decToStr(rsw24.s_264_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_264_2.HasValue ? Utils.decToStr(rsw24.s_264_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_264_3.HasValue ? Utils.decToStr(rsw24.s_264_3.Value) : "0.00"))),
                                            new XElement("НеПодлежащиеОбложениюПоДопТарифу",
                                                new XElement("КодСтроки", "265"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_265_0.HasValue ? Utils.decToStr(rsw24.s_265_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_265_1.HasValue ? Utils.decToStr(rsw24.s_265_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_265_2.HasValue ? Utils.decToStr(rsw24.s_265_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_265_3.HasValue ? Utils.decToStr(rsw24.s_265_3.Value) : "0.00"))),
                                            new XElement("БазаДляНачисленияСтраховыхВзносовПоДопТарифу",
                                                new XElement("КодСтроки", "267"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_267_0.HasValue ? Utils.decToStr(rsw24.s_267_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_267_1.HasValue ? Utils.decToStr(rsw24.s_267_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_267_2.HasValue ? Utils.decToStr(rsw24.s_267_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_267_3.HasValue ? Utils.decToStr(rsw24.s_267_3.Value) : "0.00"))),
                                            new XElement("НачисленоПоДопТарифу",
                                                new XElement("КодСтроки", "268"),
                                                new XElement("РасчетСумм",
                                                    new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw24.s_268_0.HasValue ? Utils.decToStr(rsw24.s_268_0.Value) : "0.00"),
                                                    new XElement("СуммаПоследние1месяц", rsw24.s_268_1.HasValue ? Utils.decToStr(rsw24.s_268_1.Value) : "0.00"),
                                                    new XElement("СуммаПоследние2месяц", rsw24.s_268_2.HasValue ? Utils.decToStr(rsw24.s_268_2.Value) : "0.00"),
                                                    new XElement("СуммаПоследние3месяц", rsw24.s_268_3.HasValue ? Utils.decToStr(rsw24.s_268_3.Value) : "0.00"))),
                                            new XElement("КоличествоФЛпоДопТарифу",
                                                new XElement("КодСтроки", "269"),
                                                new XElement("КоличествоЗЛ_Всего", rsw24.s_269_0.HasValue ? rsw24.s_269_0.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_1месяц", rsw24.s_269_1.HasValue ? rsw24.s_269_1.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_2месяц", rsw24.s_269_2.HasValue ? rsw24.s_269_2.Value.ToString() : "0"),
                                                new XElement("КоличествоЗЛ_3месяц", rsw24.s_269_3.HasValue ? rsw24.s_269_3.Value.ToString() : "0")))));
                }


                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_4);
            }

            #endregion


            #region Раздел 2.5

            if (db.FormsRSW2014_1_Razd_2_5_1.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID) || db.FormsRSW2014_1_Razd_2_5_2.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
            {
                XElement Раздел_2_5 = new XElement("Раздел_2_5");
                #region Раздел 2.5.1
                if (db.FormsRSW2014_1_Razd_2_5_1.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                {
                    XElement ПереченьПачекИсходныхСведенийПУ = new XElement("ПереченьПачекИсходныхСведенийПУ",
                                                                   new XElement("КоличествоПачек", db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID).Count().ToString()));
                    decimal col2 = 0;
                    decimal col3 = 0;
                    long col4 = 0;

                    foreach (var rsw251 in db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                    {
                        XElement СведенияОпачкеИсходных = new XElement("СведенияОпачкеИсходных",
                                                              new XElement("НомерПП", rsw251.NumRec.Value),
                                                              new XElement("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную", rsw251.Col_2.HasValue ? Utils.decToStr(rsw251.Col_2.Value) : "0.00"),
                                                              new XElement("СтраховыхВзносовОПС", rsw251.Col_3.HasValue ? Utils.decToStr(rsw251.Col_3.Value) : "0.00"),
                                                              new XElement("КоличествоЗЛвПачке", rsw251.Col_4.Value),
                                                              new XElement("ИмяФайла", rsw251.Col_5));
                        col2 = rsw251.Col_2.HasValue ? col2 + rsw251.Col_2.Value : col2;
                        col3 = rsw251.Col_3.HasValue ? col3 + rsw251.Col_3.Value : col3;
                        col4 = rsw251.Col_4.HasValue ? col4 + rsw251.Col_4.Value : col4;

                        ПереченьПачекИсходныхСведенийПУ.Add(СведенияОпачкеИсходных);
                    }

                    ПереченьПачекИсходныхСведенийПУ.Add(new XElement("ИтогоСведенияПоПачкам",
                                                            new XElement("БазаДляНачисленияСтраховыхВзносовНеПревышающаяПредельную", col2 != 0 ? Utils.decToStr(col2) : "0.00"),
                                                            new XElement("СтраховыхВзносовОПС", col3 != 0 ? Utils.decToStr(col3) : "0.00"),
                                                            new XElement("КоличествоЗЛвПачке", col4.ToString())));

                    Раздел_2_5.Add(ПереченьПачекИсходныхСведенийПУ);
                }


                #endregion

                #region Раздел 2.5.2

                if (db.FormsRSW2014_1_Razd_2_5_2.Any(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                {
                    XElement ПереченьПачекКорректирующихСведенийПУ = new XElement("ПереченьПачекКорректирующихСведенийПУ",
                                                                         new XElement("КоличествоПачек", db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID).Count().ToString()));

                    decimal col4 = 0;
                    decimal col5 = 0;
                    decimal col6 = 0;
                    long col7 = 0;

                    foreach (var rsw252 in db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum && x.InsurerID == rsw.InsurerID))
                    {
                        XElement СведенияОпачкеКорректирующих = new XElement("СведенияОпачкеКорректирующих",
                                                              new XElement("НомерПП", rsw252.NumRec.Value),
                                                              new XElement("КорректируемыйОтчетныйПериод",
                                                                  new XElement("Квартал", rsw252.Col_2_QuarterKorr),
                                                                  new XElement("Год", rsw252.Col_3_YearKorr)),
                                                              new XElement("ДоначисленоСтраховыхВзносовОПС", rsw252.Col_4.HasValue ? Utils.decToStr(rsw252.Col_4.Value) : "0.00"),
                                                              new XElement("ДоначисленоНаСтраховуюЧасть", rsw252.Col_5.HasValue ? Utils.decToStr(rsw252.Col_5.Value) : "0.00"),
                                                              new XElement("ДоначисленоНаНакопительнуюЧасть", rsw252.Col_6.HasValue ? Utils.decToStr(rsw252.Col_6.Value) : "0.00"),
                                                              new XElement("КоличествоЗЛвПачке", rsw252.Col_7.Value),
                                                              new XElement("ИмяФайла", rsw252.Col_8));

                        col4 = rsw252.Col_4.HasValue ? col4 + rsw252.Col_4.Value : col4;
                        col5 = rsw252.Col_5.HasValue ? col5 + rsw252.Col_5.Value : col5;
                        col6 = rsw252.Col_6.HasValue ? col6 + rsw252.Col_6.Value : col6;
                        col7 = rsw252.Col_7.HasValue ? col7 + rsw252.Col_7.Value : col7;


                        ПереченьПачекКорректирующихСведенийПУ.Add(СведенияОпачкеКорректирующих);
                    }

                    ПереченьПачекКорректирующихСведенийПУ.Add(new XElement("ИтогоСведенияПоПачкамКорректирующих",
                                                            new XElement("ДоначисленоСтраховыхВзносовОПС", col4 != 0 ? Utils.decToStr(col4) : "0.00"),
                                                            new XElement("ДоначисленоНаСтраховуюЧасть", col5 != 0 ? Utils.decToStr(col5) : "0.00"),
                                                            new XElement("ДоначисленоНаНакопительнуюЧасть", col6 != 0 ? Utils.decToStr(col6) : "0.00"),
                                                            new XElement("КоличествоЗЛвПачке", col7.ToString())));

                    Раздел_2_5.Add(ПереченьПачекКорректирующихСведенийПУ);
                }
                #endregion

                Раздел2РасчетПоТарифуИдопТарифу.Add(Раздел_2_5);
            }

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел2РасчетПоТарифуИдопТарифу);

            #endregion


            #endregion


            #region Раздел3РасчетНаПравоПримененияПониженногоТарифа2014
            if ((rsw.ExistPart_3_1.HasValue && rsw.ExistPart_3_1 == 1) || (rsw.ExistPart_3_2.HasValue && rsw.ExistPart_3_2 == 1) || (rsw.ExistPart_3_3.HasValue && rsw.ExistPart_3_3 == 1) || (rsw.ExistPart_3_4.HasValue && rsw.ExistPart_3_4 == 1) || (rsw.ExistPart_3_5.HasValue && rsw.ExistPart_3_5 == 1) || (rsw.ExistPart_3_6.HasValue && rsw.ExistPart_3_6 == 1))
            {
                xName = "Раздел3РасчетНаПравоПримененияПониженногоТарифа2014";
                if (yearType == 2015)
                    xName = "Раздел3РасчетНаПравоПримененияПониженногоТарифа2015";

                XElement Раздел3РасчетНаПравоПримененияПониженногоТарифа2014 = new XElement(xName);

                #region Раздел3_1_ДляОбщественныхОрганизацийИнвалидов
                if (yearType == 2014 && rsw.ExistPart_3_1.HasValue && rsw.ExistPart_3_1 == 1)
                {
                    string s0 = "0.00";
                    string s1 = "0.00";
                    string s2 = "0.00";
                    string s3 = "0.00";

                    if (rsw.s_321_0.HasValue && rsw.s_322_0.HasValue && rsw.s_321_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round(((rsw.s_322_0.Value / rsw.s_321_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_321_1.HasValue && rsw.s_322_1.HasValue && rsw.s_321_1.Value != 0)
                    {
                        s1 = Utils.decToStr(Math.Round(((rsw.s_322_1.Value / rsw.s_321_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_321_2.HasValue && rsw.s_322_2.HasValue && rsw.s_321_2.Value != 0)
                    {
                        s2 = Utils.decToStr(Math.Round(((rsw.s_322_2.Value / rsw.s_321_2.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_321_3.HasValue && rsw.s_322_3.HasValue && rsw.s_321_3.Value != 0)
                    {
                        s3 = Utils.decToStr(Math.Round(((rsw.s_322_3.Value / rsw.s_321_3.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    XElement Раздел3_1_ДляОбщественныхОрганизацийИнвалидов = new XElement("Раздел3_1_ДляОбщественныхОрганизацийИнвалидов",
                                                                                 new XElement("ЧисленностьЧленовОрганизации",
                                                                                     new XElement("КодСтроки", "321"),
                                                                                     new XElement("КоличествоЗЛ_Всего", rsw.s_321_0.HasValue ? rsw.s_321_0.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_1месяц", rsw.s_321_1.HasValue ? rsw.s_321_1.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_2месяц", rsw.s_321_2.HasValue ? rsw.s_321_2.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_3месяц", rsw.s_321_3.HasValue ? rsw.s_321_3.Value.ToString() : "0")),
                                                                                 new XElement("ЧисленностьИнвалидов",
                                                                                     new XElement("КодСтроки", "322"),
                                                                                     new XElement("КоличествоЗЛ_Всего", rsw.s_322_0.HasValue ? rsw.s_322_0.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_1месяц", rsw.s_322_1.HasValue ? rsw.s_322_1.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_2месяц", rsw.s_322_2.HasValue ? rsw.s_322_2.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_3месяц", rsw.s_322_3.HasValue ? rsw.s_322_3.Value.ToString() : "0")),
                                                                                 new XElement("УдельныйВесЧисленности",
                                                                                     new XElement("КодСтроки", "323"),
                                                                                     new XElement("УдельныйВес_Всего", s0),
                                                                                     new XElement("УдельныйВес_1месяц", s1),
                                                                                     new XElement("УдельныйВес_2месяц", s2),
                                                                                     new XElement("УдельныйВес_3месяц", s3)));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_1_ДляОбщественныхОрганизацийИнвалидов);
                }
                #endregion

                #region Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов
                if (yearType == 2014 && rsw.ExistPart_3_2.HasValue && rsw.ExistPart_3_2 == 1)
                {
                    string s0 = "0.00";
                    string s1 = "0.00";
                    string s2 = "0.00";
                    string s3 = "0.00";

                    if (rsw.s_331_0.HasValue && rsw.s_332_0.HasValue && rsw.s_331_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round(((rsw.s_332_0.Value / rsw.s_331_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_331_1.HasValue && rsw.s_332_1.HasValue && rsw.s_331_1.Value != 0)
                    {
                        s1 = Utils.decToStr(Math.Round(((rsw.s_332_1.Value / rsw.s_331_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_331_2.HasValue && rsw.s_332_2.HasValue && rsw.s_331_2.Value != 0)
                    {
                        s2 = Utils.decToStr(Math.Round(((rsw.s_332_2.Value / rsw.s_331_2.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_331_3.HasValue && rsw.s_332_3.HasValue && rsw.s_331_3.Value != 0)
                    {
                        s3 = Utils.decToStr(Math.Round(((rsw.s_332_3.Value / rsw.s_331_3.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    string s0_ = "0.00";
                    string s1_ = "0.00";
                    string s2_ = "0.00";
                    string s3_ = "0.00";

                    if (rsw.s_334_0.HasValue && rsw.s_335_0.HasValue && rsw.s_334_0.Value != 0)
                    {
                        s0_ = Utils.decToStr(Math.Round(((rsw.s_335_0.Value / rsw.s_334_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_334_1.HasValue && rsw.s_335_1.HasValue && rsw.s_334_1.Value != 0)
                    {
                        s1_ = Utils.decToStr(Math.Round(((rsw.s_335_1.Value / rsw.s_334_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_334_2.HasValue && rsw.s_335_2.HasValue && rsw.s_334_2.Value != 0)
                    {
                        s2_ = Utils.decToStr(Math.Round(((rsw.s_335_2.Value / rsw.s_334_2.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_334_3.HasValue && rsw.s_335_3.HasValue && rsw.s_334_3.Value != 0)
                    {
                        s3_ = Utils.decToStr(Math.Round(((rsw.s_335_3.Value / rsw.s_334_3.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }


                    XElement Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов = new XElement("Раздел3_1_ДляОбщественныхОрганизацийИнвалидов",
                                                                                 new XElement("ЧисленностьЧленовОрганизации",
                                                                                     new XElement("КодСтроки", "331"),
                                                                                     new XElement("КоличествоЗЛ_Всего", rsw.s_331_0.HasValue ? rsw.s_331_0.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_1месяц", rsw.s_331_1.HasValue ? rsw.s_331_1.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_2месяц", rsw.s_331_2.HasValue ? rsw.s_331_2.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_3месяц", rsw.s_331_3.HasValue ? rsw.s_331_3.Value.ToString() : "0")),
                                                                                 new XElement("ЧисленностьИнвалидов",
                                                                                     new XElement("КодСтроки", "332"),
                                                                                     new XElement("КоличествоЗЛ_Всего", rsw.s_332_0.HasValue ? rsw.s_332_0.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_1месяц", rsw.s_332_1.HasValue ? rsw.s_332_1.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_2месяц", rsw.s_332_2.HasValue ? rsw.s_332_2.Value.ToString() : "0"),
                                                                                     new XElement("КоличествоЗЛ_3месяц", rsw.s_332_3.HasValue ? rsw.s_332_3.Value.ToString() : "0")),
                                                                                 new XElement("УдельныйВесЧисленности",
                                                                                     new XElement("КодСтроки", "333"),
                                                                                     new XElement("УдельныйВес_Всего", s0),
                                                                                     new XElement("УдельныйВес_1месяц", s1),
                                                                                     new XElement("УдельныйВес_2месяц", s2),
                                                                                     new XElement("УдельныйВес_3месяц", s3)),
                                                                                 new XElement("ФондОплатыТруда",
                                                                                     new XElement("КодСтроки", "334"),
                                                                                     new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_334_0.HasValue ? Utils.decToStr(rsw.s_334_0.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние1месяц", rsw.s_334_1.HasValue ? Utils.decToStr(rsw.s_334_1.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние2месяц", rsw.s_334_2.HasValue ? Utils.decToStr(rsw.s_334_2.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние3месяц", rsw.s_334_3.HasValue ? Utils.decToStr(rsw.s_334_3.Value) : "0.00")),
                                                                                 new XElement("ЗарплатаИнвалидов",
                                                                                     new XElement("КодСтроки", "335"),
                                                                                     new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw.s_335_0.HasValue ? Utils.decToStr(rsw.s_335_0.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние1месяц", rsw.s_335_1.HasValue ? Utils.decToStr(rsw.s_335_1.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние2месяц", rsw.s_335_2.HasValue ? Utils.decToStr(rsw.s_335_2.Value) : "0.00"),
                                                                                     new XElement("СуммаПоследние3месяц", rsw.s_335_3.HasValue ? Utils.decToStr(rsw.s_335_3.Value) : "0.00")),
                                                                                 new XElement("УдельныйВесЗарплатыИнвалидов",
                                                                                     new XElement("КодСтроки", "336"),
                                                                                     new XElement("УдельныйВес_Всего", s0_),
                                                                                     new XElement("УдельныйВес_1месяц", s1_),
                                                                                     new XElement("УдельныйВес_2месяц", s2_),
                                                                                     new XElement("УдельныйВес_3месяц", s3_)));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_2_ДляОрганизацийУставныйКапиталСостоитИзВкладовОбщОргИнвалидов);
                }
                #endregion

                #region Раздел3_3_ДляОрганизацийИТ. Для РСВ-1 2015 Раздел3_1_ДляОрганизацийИТ
                if (rsw.ExistPart_3_3.HasValue && rsw.ExistPart_3_3 == 1)
                {
                    string s0 = "0.00";
                    string s1 = "0.00";

                    if (rsw.s_341_0.HasValue && rsw.s_342_0.HasValue && rsw.s_341_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round(((rsw.s_342_0.Value / rsw.s_341_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_341_1.HasValue && rsw.s_342_1.HasValue && rsw.s_341_1.Value != 0)
                    {
                        s1 = Utils.decToStr(Math.Round(((rsw.s_342_1.Value / rsw.s_341_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    xName = "Раздел3_3_ДляОрганизацийИТ";
                    //    if (yearType == 2015)
                    //          xName = "Раздел3_1_ДляОрганизацийИТ";

                    XElement Раздел3_3_ДляОрганизацийИТ = new XElement(xName,
                                                              new XElement("СуммаДоходовПоНКвсего",
                                                                  new XElement("КодСтроки", "341"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_341_0.HasValue ? Utils.decToStr(rsw.s_341_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_341_1.HasValue ? Utils.decToStr(rsw.s_341_1.Value) : "0.00")),
                                                              new XElement("СуммаДоходовИТизНих",
                                                                  new XElement("КодСтроки", "342"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_342_0.HasValue ? Utils.decToStr(rsw.s_342_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_342_1.HasValue ? Utils.decToStr(rsw.s_342_1.Value) : "0.00")),
                                                              new XElement("ДоляДоходовИТ",
                                                                  new XElement("КодСтроки", "343"),
                                                                  new XElement("ДоляДоходаПоПредшествующему", s0),
                                                                  new XElement("ДоляДоходаПоТекущему", s1)),
                                                              new XElement("ЧисленностьРаботниковИТ",
                                                                  new XElement("КодСтроки", "344"),
                                                                  new XElement("КоличествоЗЛпоПредшествующему", rsw.s_344_0.HasValue ? rsw.s_344_0.Value.ToString() : "0"),
                                                                  new XElement("КоличествоЗЛпоТекущему", rsw.s_344_1.HasValue ? rsw.s_344_1.Value.ToString() : "0")),
                                                              new XElement("СведенияИзРеестраИТ",
                                                                  new XElement("КодСтроки", "345"),
                                                                  new XElement("ДатаЗаписиВреестре", rsw.s_345_0.HasValue ? rsw.s_345_0.Value.ToShortDateString() : ""),
                                                                  new XElement("НомерЗаписиВреестре", !String.IsNullOrEmpty(rsw.s_345_1) ? rsw.s_345_1 : "")));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_3_ДляОрганизацийИТ);

                }
                #endregion

                #region Раздел3_4_ДляОрганизацийСМИ
                if (yearType == 2014 && rsw.ExistPart_3_4.HasValue && rsw.ExistPart_3_4 == 1)
                {
                    XElement Раздел3_4_ДляОрганизацийСМИ = new XElement("Раздел3_4_ДляОрганизацийСМИ");
                    decimal sum = 0;
                    var RSW_3_4_List = db.FormsRSW2014_1_Razd_3_4.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.ID).ToList();

                    foreach (var rsw34 in RSW_3_4_List)
                    {
                        XElement СведенияПоВидуДеятельности = new XElement("СведенияПоВидуДеятельности",
                                                                  new XElement("НомерПП", rsw34.NumOrd.Value.ToString()),
                                                                  new XElement("НаименованиеВидаЭД", rsw34.NameOKWED.Substring(0, rsw34.NameOKWED.Length > 250 ? 250 : rsw34.NameOKWED.Length)),
                                                                  new XElement("КодПоОКВЭД", rsw34.OKWED),
                                                                  new XElement("ДоходыПоВидуЭД", rsw34.Income.HasValue ? Utils.decToStr(rsw34.Income.Value) : "0.00"),
                                                                  new XElement("ДоляДоходовПоВидуЭД", rsw34.RateIncome.HasValue ? Utils.decToStr(rsw34.RateIncome.Value) : "0.00"));


                        Раздел3_4_ДляОрганизацийСМИ.Add(СведенияПоВидуДеятельности);
                    }

                    Раздел3_4_ДляОрганизацийСМИ.Add(new XElement("ИтогоПоВсемВидамДеятельности", sum != 0 ? Utils.decToStr(sum) : "0.00"));
                    Раздел3_4_ДляОрганизацийСМИ.Add(new XElement("СведенияИзРеестраСМИ",
                                                                  new XElement("КодСтроки", "351"),
                                                                  new XElement("ДатаЗаписиВреестре", rsw.s_351_0.HasValue ? rsw.s_351_0.Value.ToString() : ""),
                                                                  new XElement("НомерЗаписиВреестре", !String.IsNullOrEmpty(rsw.s_351_1) ? rsw.s_351_1 : "")));

                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_4_ДляОрганизацийСМИ);

                }
                #endregion

                #region Раздел3_5_ДляОрганизацийПрименяющихУСН. Для РСВ-1 2015 Раздел3_2_ДляОрганизацийПрименяющихУСН
                if (rsw.ExistPart_3_5.HasValue && rsw.ExistPart_3_5 == 1)
                {
                    string s0 = "0.00";

                    if (rsw.s_361_0.HasValue && rsw.s_362_0.HasValue && rsw.s_361_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round(((rsw.s_362_0.Value / rsw.s_361_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    xName = "Раздел3_5_ДляОрганизацийПрименяющихУСН";
                    //     if (yearType == 2015)
                    //         xName = "Раздел3_2_ДляОрганизацийПрименяющихУСН";

                    XElement Раздел3_5_ДляОрганизацийПрименяющихУСН = new XElement(xName,
                                                                          new XElement("СуммаДоходаПоСтатье346_15НКвсего",
                                                                              new XElement("КодСтроки", "361"),
                                                                              new XElement("СуммаДохода", rsw.s_361_0.HasValue ? Utils.decToStr(rsw.s_361_0.Value) : "0.00")),
                                                                          new XElement("СуммаДоходаПоСтатье58ИзНих",
                                                                              new XElement("КодСтроки", "362"),
                                                                              new XElement("СуммаДохода", rsw.s_362_0.HasValue ? Utils.decToStr(rsw.s_362_0.Value) : "0.00")),
                                                                          new XElement("ДоляДоходаПоСтатье58",
                                                                              new XElement("КодСтроки", "363"),
                                                                              new XElement("ДоляДохода", s0)));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_5_ДляОрганизацийПрименяющихУСН);

                }
                #endregion

                #region Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН. Для РСВ-1 2015 Раздел3_3_ДляНекоммерческихОрганизацийПрименяющихУСН
                if (rsw.ExistPart_3_6.HasValue && rsw.ExistPart_3_6 == 1)
                {
                    string s0 = "0.00";
                    string s1 = "0.00";

                    if (rsw.s_371_0.HasValue && rsw.s_372_0.HasValue && rsw.s_371_0.Value != 0)
                    {
                        s0 = Utils.decToStr(Math.Round((((rsw.s_372_0.Value + rsw.s_373_0.Value + rsw.s_374_0.Value) / rsw.s_371_0.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }
                    if (rsw.s_371_1.HasValue && rsw.s_372_1.HasValue && rsw.s_371_1.Value != 0)
                    {
                        s1 = Utils.decToStr(Math.Round((((rsw.s_372_1.Value + rsw.s_373_1.Value + rsw.s_374_1.Value) / rsw.s_371_1.Value) * (decimal)100), 2, MidpointRounding.AwayFromZero));
                    }

                    xName = "Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН";
                    //         if (yearType == 2015)
                    //             xName = "Раздел3_3_ДляНекоммерческихОрганизацийПрименяющихУСН";

                    XElement Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН = new XElement(xName,
                                                              new XElement("СуммаДоходовВсего",
                                                                  new XElement("КодСтроки", "371"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_371_0.HasValue ? Utils.decToStr(rsw.s_371_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_371_1.HasValue ? Utils.decToStr(rsw.s_371_1.Value) : "0.00")),
                                                              new XElement("СуммаДоходовВвидеЦелевыхПоступлений",
                                                                  new XElement("КодСтроки", "372"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_372_0.HasValue ? Utils.decToStr(rsw.s_372_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_372_1.HasValue ? Utils.decToStr(rsw.s_372_1.Value) : "0.00")),
                                                              new XElement("СуммаДоходовВвидеГрантов",
                                                                  new XElement("КодСтроки", "373"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_373_0.HasValue ? Utils.decToStr(rsw.s_373_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_373_1.HasValue ? Utils.decToStr(rsw.s_373_1.Value) : "0.00")),
                                                              new XElement("СуммаДоходовОтОсуществленияОпределенныхВЭД",
                                                                  new XElement("КодСтроки", "374"),
                                                                  new XElement("СуммаДоходаПоПредшествующему", rsw.s_374_0.HasValue ? Utils.decToStr(rsw.s_374_0.Value) : "0.00"),
                                                                  new XElement("СуммаДоходаПоТекущему", rsw.s_374_1.HasValue ? Utils.decToStr(rsw.s_374_1.Value) : "0.00")),
                                                              new XElement("ДоляДоходов",
                                                                  new XElement("КодСтроки", "343"),
                                                                  new XElement("ДоляДоходаПоПредшествующему", s0),
                                                                  new XElement("ДоляДоходаПоТекущему", s1)));


                    Раздел3РасчетНаПравоПримененияПониженногоТарифа2014.Add(Раздел3_6_ДляНекоммерческихОрганизацийПрименяющихУСН);
                }
                #endregion

                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел3РасчетНаПравоПримененияПониженногоТарифа2014);
            }
            #endregion

            #region Раздел4СуммыДоначисленныхСтраховыхВзносов2014
            if (rsw.ExistPart_4.HasValue && rsw.ExistPart_4.Value == 1)
            {
                XElement Раздел4СуммыДоначисленныхСтраховыхВзносов2014 = new XElement(yearType != 2015 ? "Раздел4СуммыДоначисленныхСтраховыхВзносов2014" : "Раздел4");
                var RSW_4_List = db.FormsRSW2014_1_Razd_4.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.ID).ToList();

                decimal[] sum = new decimal[9];
                for (int y = 0; y < sum.Length; y++)
                {
                    sum[y] = 0;
                }

                foreach (var rsw4 in RSW_4_List)
                {
                    XElement СуммаДоначисленныхВзносовЗаПериодНачинаяС2014 = new XElement("СуммаДоначисленныхВзносовЗаПериодНачинаяС2014",
                                                                                 new XElement("НомерПП", rsw4.NumOrd.Value),
                                                                                 new XElement("ОснованиеДляДоначисления", rsw4.Base.Value));
                    if (rsw4.YearPer.Value >= 2014 && (rsw4.Dop21.HasValue && rsw4.Dop21.Value != 0))
                        СуммаДоначисленныхВзносовЗаПериодНачинаяС2014.Add(new XElement("КодОснованияДляДопТарифа", rsw4.CodeBase.Value));


                    СуммаДоначисленныхВзносовЗаПериодНачинаяС2014.Add(new XElement("Год", rsw4.YearPer.Value),
                                                                                 new XElement("Месяц", rsw4.MonthPer.Value),
                                                                                 new XElement("СуммаДоначисленныхВзносовОПС2014всего", rsw4.Strah2014.HasValue ? Utils.decToStr(rsw4.Strah2014.Value) : "0.00"),
                                                                                 new XElement("СуммаДоначисленныхВзносовОПС2014превыщающие", rsw4.StrahMoreBase2014.HasValue ? Utils.decToStr(rsw4.StrahMoreBase2014.Value) : "0.00"),
                                                                                 new XElement("СуммаДоначисленныхВзносовНаСтраховуюВсего", rsw4.Strah2013.HasValue ? Utils.decToStr(rsw4.Strah2013.Value) : "0.00"),
                                                                                 new XElement("СуммаДоначисленныхВзносовНаСтраховуюПревышающие", rsw4.StrahMoreBase2013.HasValue ? Utils.decToStr(rsw4.StrahMoreBase2013.Value) : "0.00"),
                                                                                 new XElement("СуммаДоначисленныхВзносовНаНакопительную", rsw4.Nakop2013.HasValue ? Utils.decToStr(rsw4.Nakop2013.Value) : "0.00"),
                                                                                 new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ1", rsw4.Dop1.HasValue ? Utils.decToStr(rsw4.Dop1.Value) : "0.00"),
                                                                                 new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2", rsw4.Dop2.HasValue ? Utils.decToStr(rsw4.Dop2.Value) : "0.00"),
                                                                                 new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2_1", rsw4.Dop21.HasValue ? Utils.decToStr(rsw4.Dop21.Value) : "0.00"),
                                                                                 new XElement("СтраховыеВзносыОМС", rsw4.OMS.HasValue ? Utils.decToStr(rsw4.OMS.Value) : "0.00"));

                    Раздел4СуммыДоначисленныхСтраховыхВзносов2014.Add(СуммаДоначисленныхВзносовЗаПериодНачинаяС2014);

                    sum[0] = rsw4.Strah2014.HasValue ? rsw4.Strah2014.Value + sum[0] : sum[0];
                    sum[1] = rsw4.StrahMoreBase2014.HasValue ? rsw4.StrahMoreBase2014.Value + sum[1] : sum[1];
                    sum[2] = rsw4.Strah2013.HasValue ? rsw4.Strah2013.Value + sum[2] : sum[2];
                    sum[3] = rsw4.StrahMoreBase2013.HasValue ? rsw4.StrahMoreBase2013.Value + sum[3] : sum[3];
                    sum[4] = rsw4.Nakop2013.HasValue ? rsw4.Nakop2013.Value + sum[4] : sum[4];
                    sum[5] = rsw4.Dop1.HasValue ? rsw4.Dop1.Value + sum[5] : sum[5];
                    sum[6] = rsw4.Dop2.HasValue ? rsw4.Dop2.Value + sum[6] : sum[6];
                    sum[7] = rsw4.Dop21.HasValue ? rsw4.Dop21.Value + sum[7] : sum[7];
                    sum[8] = rsw4.OMS.HasValue ? rsw4.OMS.Value + sum[8] : sum[8];

                }

                if (RSW_4_List.Count() > 0)
                {
                    XElement ИтогоДоначисленоНачинаяС2014 = new XElement("ИтогоДоначисленоНачинаяС2014",
                                                                new XElement("СуммаДоначисленныхВзносовОПС2014всего", sum[0] != 0 ? Utils.decToStr(sum[0]) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовОПС2014превыщающие", sum[1] != 0 ? Utils.decToStr(sum[1]) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаСтраховуюВсего", sum[2] != 0 ? Utils.decToStr(sum[2]) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаСтраховуюПревышающие", sum[3] != 0 ? Utils.decToStr(sum[3]) : "0.00"),
                                                                new XElement("СуммаДоначисленныхВзносовНаНакопительную", sum[4] != 0 ? Utils.decToStr(sum[4]) : "0.00"),
                                                                new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ1", sum[5] != 0 ? Utils.decToStr(sum[5]) : "0.00"),
                                                                new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2", sum[6] != 0 ? Utils.decToStr(sum[6]) : "0.00"),
                                                                new XElement("СтраховыхДоначисленныхВзносовПоДопТарифуЧ2_1", sum[7] != 0 ? Utils.decToStr(sum[7]) : "0.00"),
                                                                new XElement("СтраховыеВзносыОМС", sum[8] != 0 ? Utils.decToStr(sum[8]) : "0.00"));

                    Раздел4СуммыДоначисленныхСтраховыхВзносов2014.Add(ИтогоДоначисленоНачинаяС2014);
                }

                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел4СуммыДоначисленныхСтраховыхВзносов2014);
            }

            #endregion

            #region Раздел5СведенияОВыплатахВпользуОбучающихся2014
            if (rsw.ExistPart_5.HasValue && rsw.ExistPart_5.Value == 1)
            {
                var RSW_5_List = db.FormsRSW2014_1_Razd_5.Where(x => x.InsurerID == rsw.InsurerID && x.Quarter == rsw.Quarter && x.Year == rsw.Year && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.ID).ToList();
                XElement Раздел5СведенияОВыплатахВпользуОбучающихся2014 = new XElement("Раздел5СведенияОВыплатахВпользуОбучающихся2014",
                                                                              new XElement("КоличествоОбучающихся", RSW_5_List.Count().ToString()));

                decimal[] sum = new decimal[4];
                for (int y = 0; y < sum.Length; y++)
                {
                    sum[y] = 0;
                }

                foreach (var rsw5 in RSW_5_List)
                {
                    XElement СведенияОбОбучающемся = new XElement("СведенияОбОбучающемся",
                                                         new XElement("НомерПП", rsw5.NumOrd.Value),
                                                         new XElement("ФИО",
                                                             new XElement("Фамилия", rsw5.LastName.Substring(0, rsw5.LastName.Length > 40 ? 40 : rsw5.LastName.Length)),
                                                             new XElement("Имя", rsw5.FirstName.Substring(0, rsw5.FirstName.Length > 40 ? 40 : rsw5.FirstName.Length)),
                                                             new XElement("Отчество", rsw5.MiddleName.Substring(0, rsw5.MiddleName.Length > 40 ? 40 : rsw5.MiddleName.Length))),
                                                         new XElement("НомерСправкиОчленствеВстудОтряде", rsw5.NumSpravka.Substring(0, rsw5.NumSpravka.Length > 40 ? 40 : rsw5.NumSpravka.Length)),
                                                         new XElement("ДатаВыдачиСправкиОчленствеВстудОтряде", rsw5.DateSpravka.Value.ToShortDateString()),
                                                         new XElement("НомерСправкиОбОчномОбучении", rsw5.NumSpravka1.Substring(0, rsw5.NumSpravka1.Length > 40 ? 40 : rsw5.NumSpravka1.Length)),
                                                         new XElement("ДатаВыдачиСправкиОбОчномОбучении", rsw5.DateSpravka1.Value.ToShortDateString()),
                                                         new XElement("СуммыВыплатИвознаграждений",
                                                             new XElement("СуммаВсегоСначалаРасчетногоПериода", rsw5.SumPay.HasValue ? Utils.decToStr(rsw5.SumPay.Value) : "0.00"),
                                                             new XElement("СуммаПоследние1месяц", rsw5.SumPay_0.HasValue ? Utils.decToStr(rsw5.SumPay_0.Value) : "0.00"),
                                                             new XElement("СуммаПоследние2месяц", rsw5.SumPay_1.HasValue ? Utils.decToStr(rsw5.SumPay_1.Value) : "0.00"),
                                                             new XElement("СуммаПоследние3месяц", rsw5.SumPay_2.HasValue ? Utils.decToStr(rsw5.SumPay_2.Value) : "0.00")));

                    Раздел5СведенияОВыплатахВпользуОбучающихся2014.Add(СведенияОбОбучающемся);

                    sum[0] = rsw5.SumPay.HasValue ? sum[0] + rsw5.SumPay.Value : sum[0];
                    sum[1] = rsw5.SumPay_0.HasValue ? sum[1] + rsw5.SumPay_0.Value : sum[1];
                    sum[2] = rsw5.SumPay_1.HasValue ? sum[2] + rsw5.SumPay_1.Value : sum[2];
                    sum[3] = rsw5.SumPay_2.HasValue ? sum[3] + rsw5.SumPay_2.Value : sum[3];
                }

                Раздел5СведенияОВыплатахВпользуОбучающихся2014.Add(new XElement("ИтогоВыплат",
                                                             new XElement("СуммаВсегоСначалаРасчетногоПериода", sum[0] != 0 ? Utils.decToStr(sum[0]) : "0.00"),
                                                             new XElement("СуммаПоследние1месяц", sum[1] != 0 ? Utils.decToStr(sum[1]) : "0.00"),
                                                             new XElement("СуммаПоследние2месяц", sum[2] != 0 ? Utils.decToStr(sum[2]) : "0.00"),
                                                             new XElement("СуммаПоследние3месяц", sum[3] != 0 ? Utils.decToStr(sum[3]) : "0.00")));

                XElement СведенияИзРеестраМДОО = new XElement("СведенияИзРеестраМДОО",
                                                     new XElement("КодСтроки", "501"));

                if (!String.IsNullOrEmpty(rsw.s_501_1_0))
                {
                    СведенияИзРеестраМДОО.Add(new XElement("РеквизитыЗаписиВреестре",
                                                  new XElement("ДатаЗаписиВреестре", rsw.s_501_0_0.HasValue ? rsw.s_501_0_0.Value.ToShortDateString() : ""),
                                                  new XElement("НомерЗаписиВреестре", rsw.s_501_1_0.Substring(0, rsw.s_501_1_0.Length > 20 ? 20 : rsw.s_501_1_0.Length))));
                }

                if (!String.IsNullOrEmpty(rsw.s_501_1_1))
                {
                    СведенияИзРеестраМДОО.Add(new XElement("РеквизитыЗаписиВреестре",
                                                  new XElement("ДатаЗаписиВреестре", rsw.s_501_0_1.HasValue ? rsw.s_501_0_1.Value.ToShortDateString() : ""),
                                                  new XElement("НомерЗаписиВреестре", rsw.s_501_1_1.Substring(0, rsw.s_501_1_1.Length > 20 ? 20 : rsw.s_501_1_1.Length))));
                }

                if (!String.IsNullOrEmpty(rsw.s_501_1_2))
                {
                    СведенияИзРеестраМДОО.Add(new XElement("РеквизитыЗаписиВреестре",
                                                  new XElement("ДатаЗаписиВреестре", rsw.s_501_0_2.HasValue ? rsw.s_501_0_2.Value.ToShortDateString() : ""),
                                                  new XElement("НомерЗаписиВреестре", rsw.s_501_1_2.Substring(0, rsw.s_501_1_2.Length > 20 ? 20 : rsw.s_501_1_2.Length))));
                }

                if (!String.IsNullOrEmpty(rsw.s_501_1_3))
                {
                    СведенияИзРеестраМДОО.Add(new XElement("РеквизитыЗаписиВреестре",
                                                  new XElement("ДатаЗаписиВреестре", rsw.s_501_0_3.HasValue ? rsw.s_501_0_3.Value.ToShortDateString() : ""),
                                                  new XElement("НомерЗаписиВреестре", rsw.s_501_1_3.Substring(0, rsw.s_501_1_3.Length > 20 ? 20 : rsw.s_501_1_3.Length))));
                }


                Раздел5СведенияОВыплатахВпользуОбучающихся2014.Add(СведенияИзРеестраМДОО);

                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(Раздел5СведенияОВыплатахВпользуОбучающихся2014);

            }
            #endregion

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ЛицоПодтверждающееСведения", rsw.ConfirmType.ToString()));

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ФИОлицаПодтверждающегоСведения"));

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОлицаПодтверждающегоСведения").Add(new XElement("Фамилия", !String.IsNullOrEmpty(rsw.ConfirmLastName) ? rsw.ConfirmLastName.ToUpper() : ""));
            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОлицаПодтверждающегоСведения").Add(new XElement("Имя", !String.IsNullOrEmpty(rsw.ConfirmFirstName) ? rsw.ConfirmFirstName.ToUpper() : ""));
            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ФИОлицаПодтверждающегоСведения").Add(new XElement("Отчество", !String.IsNullOrEmpty(rsw.ConfirmMiddleName) ? rsw.ConfirmMiddleName.ToUpper() : ""));

            if (!String.IsNullOrEmpty(rsw.ConfirmOrgName))
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("НаименованиеОрганизацииПредставителя", rsw.ConfirmOrgName.ToUpper()));

            //если есть документ 
            if (rsw.ConfirmDocType_ID.HasValue)
            {
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ДокументПодтверждающийПолномочияПредставителя"));
                string docName = "";
                if (db.DocumentTypes.FirstOrDefault(x => x.ID == rsw.ConfirmDocType_ID).Code == "ПРОЧЕЕ")
                    docName = rsw.ConfirmDocName;
                else
                    docName = db.DocumentTypes.FirstOrDefault(x => x.ID == rsw.ConfirmDocType_ID).Code;

                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("НаименованиеУдостоверяющего", docName.Substring(0, docName.Length > 80 ? 80 : docName.Length).ToUpper()));
                if (!String.IsNullOrEmpty(rsw.ConfirmDocSerLat))
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("СерияРимскиеЦифры", rsw.ConfirmDocSerLat.Substring(0, rsw.ConfirmDocSerLat.Length > 8 ? 8 : rsw.ConfirmDocSerLat.Length).ToUpper()));
                if (!String.IsNullOrEmpty(rsw.ConfirmDocSerRus))
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("СерияРусскиеБуквы", rsw.ConfirmDocSerRus.Substring(0, rsw.ConfirmDocSerRus.Length > 8 ? 8 : rsw.ConfirmDocSerRus.Length).ToUpper()));
                if (rsw.ConfirmDocNum.HasValue)
                    РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("НомерУдостоверяющего", rsw.ConfirmDocNum.ToString().Substring(0, rsw.ConfirmDocNum.ToString().Length > 8 ? 8 : rsw.ConfirmDocNum.ToString().Length).ToUpper()));
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("ДатаВыдачи", rsw.ConfirmDocDate.HasValue ? rsw.ConfirmDocDate.Value.ToShortDateString() : ""));
                РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Element("ДокументПодтверждающийПолномочияПредставителя").Add(new XElement("КемВыдан", rsw.ConfirmDocKemVyd.Substring(0, rsw.ConfirmDocKemVyd.Length > 80 ? 80 : rsw.ConfirmDocKemVyd.Length).ToUpper()));
            }

            РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014.Add(new XElement("ДатаЗаполнения", rsw.DateUnderwrite.Value.ToShortDateString()));


            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(РАСЧЕТ_ПО_СТРАХОВЫМ_ВЗНОСАМ_НА_ОПС_И_ОМС_ПЛАТЕЛЬЩИКАМИ_ПРОИЗВОДЯЩИМИ_ВЫПЛАТЫ_ФЛ_2014);


            xDoc.Root.SetDefaultXmlNamespace(pfr);



            //            if (yearType == 2015)
            //                xDoc.Root.SetDefaultXmlNamespace(pfr);

            xml = xDoc.ToString();
            return xml;
        }

        private void filterKorrPeriod_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            yearFilterFrom.Enabled = filterKorrPeriod.Checked;
            yearFilterTo.Enabled = filterKorrPeriod.Checked;
            korrPerGrid_upd();
        }

        private void yearFilterFrom_ValueChanged(object sender, EventArgs e)
        {
            korrPerGrid_upd();
        }

        private void yearFilterTo_ValueChanged(object sender, EventArgs e)
        {
            korrPerGrid_upd();
        }

        private void viewPacks_Click(object sender, EventArgs e)
        {
            rsw2014packs child = new rsw2014packs();

            child.ident.Year = Year.SelectedItem != null ? short.Parse(Year.SelectedItem.Value.ToString()) : (short)0;
            child.ident.Quarter = Quarter.SelectedItem != null ? byte.Parse(Quarter.SelectedItem.Value.ToString()) : (byte)0;
            child.ident.InsurerID = Options.InsID;
            child.ident.FormatType = "rsw2014";
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            foreach (var item in korrPerGrid.Rows.Where(x => x.Cells[1].Value != null))
            {
                if (item.Cells[0].Value != null)
                    item.Cells[0].Value = !(bool)item.Cells[0].Value;
                else
                    item.Cells[0].Value = true;
            }

        }

        private void DateUnderwriteMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateUnderwriteMaskedEditBox.Text != DateUnderwriteMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateUnderwriteMaskedEditBox.Text, out date))
                {
                    DateUnderwrite.Value = date;
                }
                else
                {
                    DateUnderwrite.Value = DateUnderwrite.NullDate;
                }
            }
            else
            {
                DateUnderwrite.Value = DateUnderwrite.NullDate;
            }
        }

        private void DateUnderwrite_ValueChanged(object sender, EventArgs e)
        {
            if (DateUnderwrite.Value != DateUnderwrite.NullDate)
                DateUnderwriteMaskedEditBox.Text = DateUnderwrite.Value.ToShortDateString();
            else
                DateUnderwriteMaskedEditBox.Text = DateUnderwriteMaskedEditBox.NullText;
        }

        private void CreateXmlPack_RSW2014_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bw.IsBusy)
                if (RadMessageBox.Show("Вы уверены, что хотите закрыть окно и прервать формирование пачек?", "Предупреждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    cancel_work = true;
                    bwCancel("", "");
                    while (bw.IsBusy)
                    {
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                    }
                    db.Dispose();
                }
                else
                {
                    e.Cancel = true;
                }

            Props props = new Props(); //экземпляр класса с настройками

            if (Options.saveLastPackNum)
            {
                short y = 0;
                short.TryParse(Year.SelectedItem.Text, out y);
                byte q = 12;
                byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q);

                numPackSettings numPackSett = new numPackSettings
                {
                    FormName = this.Name,
                    Number = (int)numFrom.Value,
                    Year = y,
                    Quarter = q
                };

                props.setPackNum(numPackSett);
            }



            List<WindowData> windowData = new List<WindowData> { };


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
            windowData.Add(new WindowData
            {
                control = "includeRSW2014_1",
                value = includeRSW2014_1.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "createRazd_2_5",
                value = createRazd_2_5.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "switchStaffListDDL",
                value = switchStaffListDDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "sortingDDL",
                value = sortingDDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "TypeInfoDDL",
                value = TypeInfoDDL.SelectedIndex.ToString()
            });





            props.setFormParams(this, windowData);




        }

    }
}
