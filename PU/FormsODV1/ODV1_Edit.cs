using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using Telerik.WinControls.UI;
using System.Reflection;
using Telerik.WinControls.Data;

namespace PU.FormsODV1
{
    public partial class ODV1_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public List<FormsSZV_STAJ_2017> SZV_STAJ_List = new List<FormsSZV_STAJ_2017>();
        List<FormsODV_1_4_2017> FormsODV_1_4_List = new List<FormsODV_1_4_2017>();
        List<FormsODV_1_5_2017> FormsODV_1_5_List = new List<FormsODV_1_5_2017>();
        public FormsODV_1_2017 ODV1 { get; set; }
        public long ODV1ID { get; set; }
        bool allowClose = false;
        private bool cleanData = true;
        public string action { get; set; }
        private string typeForm = "0";
        bool notClose = false;
        Font totalRowsFont;
        ODV1_Print ReportViewer;
        List<RaschetPeriodContainer> avail_periods_all = new List<RaschetPeriodContainer>();
        public byte period { get; set; }

        public ODV1_Edit()
        {
            InitializeComponent();
            totalRowsFont = new Font("Segoe UI", 9.0f, FontStyle.Bold);
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

        private void FormsODV1_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            this.radPageView1.SelectedPage = this.radPageView1.Pages[0];

            Insurer ins = db.Insurer.First(x => x.ID == Options.InsID);
            regNum.Text = Utils.ParseRegNum(ins.RegNum);
            INN.Text = ins.INN;
            KPP.Text = ins.KPP;
            if (ins.TypePayer == 0) // если организация
            {
                nameShort.Text = ins.NameShort;
            }
            else
            {
                nameShort.Text = ins.LastName + " " + ins.FirstName + " " + ins.MiddleName;
            }



            this.Year.Items.Clear();
            short y;

            if (action == "edit")
            {
                if (db.FormsODV_1_2017.Any(x => x.ID == ODV1ID))
                {
                    checkCountBtn.Visible = true;


                    ODV1 = db.FormsODV_1_2017.FirstOrDefault(x => x.ID == ODV1ID);



                    typeForm = ODV1.TypeForm.ToString();
                    TypeForm.Items.Single(x => x.Tag.ToString() == typeForm).Selected = true;
                    TypeInfo.Items.Single(x => x.Tag.ToString() == ODV1.TypeInfo.ToString()).Selected = true;

                    TypeForm_SelectedIndexChanged(null, null);

                    DateFilling.Value = ODV1.DateFilling;

                    string StaffCnt = ODV1.StaffCount.HasValue ? ODV1.StaffCount.Value.ToString() : "";

                    try
                    {
                        // выпад список "календарный год"

                        if (Year.Items.Any(x => x.Text.ToString() == ODV1.Year.ToString()))
                            Year.Items.Single(x => x.Text.ToString() == ODV1.Year.ToString()).Selected = true;


                        // выпад список "Отчетный период"

                        this.Quarter.Items.Clear();

                        if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y))
                        {
                            foreach (var item in avail_periods_all.Where(x => x.Year == y).ToList())
                            {
                                Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                            }
                            if (Quarter.Items.Any(x => x.Value.ToString() == ODV1.Code.ToString()))
                                Quarter.Items.FirstOrDefault(x => x.Value.ToString() == ODV1.Code.ToString()).Selected = true;

                        }
                    }
                    catch
                    { }


                    switch (typeForm)
                    {
                        case "1":
                            SZV_STAJ_Cnt.Text = StaffCnt;
                            break;
                        case "2":
                            SZV_ISH_Cnt.Text = StaffCnt;
                            break;
                        case "3":
                            SZV_KORR_Cnt.Text = StaffCnt;
                            break;
                    }



                    StaffCountOsobUslShtat.Value = ODV1.StaffCountOsobUslShtat.HasValue ? ODV1.StaffCountOsobUslShtat.Value : 0;
                    StaffCountOsobUslFakt.Value = ODV1.StaffCountOsobUslFakt.HasValue ? ODV1.StaffCountOsobUslFakt.Value : 0;

                    ConfirmDolgn.Text = ODV1.ConfirmDolgn;
                    ConfirmLastName.Text = ODV1.ConfirmLastName;
                    ConfirmFirstName.Text = ODV1.ConfirmFirstName;
                    ConfirmMiddleName.Text = ODV1.ConfirmMiddleName;

                    updateTextBoxes();

                    FormsODV_1_4_List = ODV1.FormsODV_1_4_2017.ToList();
                    FormsODV_1_5_List = ODV1.FormsODV_1_5_2017.ToList();
                    ODV1_4_Grid_update();
                    ODV1_5_Grid_update();

                }
                else
                {
                    RadMessageBox.Show("Не удалось загрузить форму ОДВ-1 из базы данных!");
                }
            }
            else
            {
                checkCountBtn.Visible = false;
                ODV1 = new FormsODV_1_2017();
                DateFilling.Value = DateTime.Now;
                TypeForm_SelectedIndexChanged(null, null);

                // выпад список "календарный год"


                if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                    Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
                else
                    Year.Items.OrderByDescending(x => x.Value).First().Selected = true;

                // выпад список "Отчетный период"

                this.Quarter.Items.Clear();

                if (short.TryParse(Year.SelectedItem.Text, out y))
                {
                    foreach (var item in avail_periods_all.Where(x => x.Year == y).ToList()) //  && x.Kvartal != 0
                    {
                        Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                    }
                    DateTime dt = DateTime.Now.AddDays(-45);

                    RaschetPeriodContainer rp = new RaschetPeriodContainer();

                    if (avail_periods_all.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y))
                        rp = avail_periods_all.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y);
                    else
                        rp = avail_periods_all.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y);

                    if (rp != null)
                        Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                    else
                        Quarter.Items.OrderByDescending(x => x.Value).First().Selected = true;
                }

                try
                {
                    if (ins.TypePayer != 0)  // не организация
                    {
                        ConfirmLastName.Text = !String.IsNullOrEmpty(ins.LastName) ? ins.LastName : "";
                        ConfirmFirstName.Text = !String.IsNullOrEmpty(ins.FirstName) ? ins.FirstName : "";
                        ConfirmMiddleName.Text = !String.IsNullOrEmpty(ins.MiddleName) ? ins.MiddleName : "";
                    }
                    else
                    {
                        var FIO = ins.BossFIO.Split(' ');

                        ConfirmLastName.Text = FIO[0] != null ? (!String.IsNullOrEmpty(FIO[0]) ? FIO[0] : "") : "";
                        ConfirmFirstName.Text = FIO[1] != null ? (!String.IsNullOrEmpty(FIO[1]) ? FIO[1] : "") : "";

                        if (FIO.Length > 2)
                            ConfirmMiddleName.Text = FIO[2] != null ? (!String.IsNullOrEmpty(FIO[2]) ? FIO[2] : "") : "";

                        ConfirmDolgn.Text = !String.IsNullOrEmpty(ins.BossDolgn) ? ins.BossDolgn : "";
                    }
                }
                catch { }


            }

            this.Quarter.SelectedIndexChanged += (s, с) => Quarter_SelectedIndexChanged();
            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();

            setPeriod();
        }

        private void Year_SelectedIndexChanged()
        {
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            Quarter.Items.Clear();

            short y;
            if (Year.SelectedItem != null && short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in avail_periods_all.Where(x => x.Year == y))
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

        private void Quarter_SelectedIndexChanged()
        {
            if (Quarter.SelectedItem != null)
            {
                setPeriod();
            }
        }

        private void setPeriod()
        {
            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                byte q;
                if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q))
                {
                    period = q;
                }
            }

        }
        private void updateTextBoxes()
        {
            var fields = typeof(FormsODV_1_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    //      DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                    if (ODV1 != null)
                    {
                        var properties = ODV1.GetType().GetProperty(itemName);
                        object value = properties.GetValue(ODV1, null);

                        string type = properties.PropertyType.FullName;
                        if (type.Contains("["))
                            type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                        type = type.Split(',')[0].Split('.')[1].ToLower();

                        if (value != null)
                        {
                            switch (type)
                            {
                                case "decimal":
                                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = decimal.Parse(value.ToString());
                                    break;
                            }
                        }
                        else
                        {
                            switch (type)
                            {
                                case "decimal":
                                    ((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).EditValue = (decimal)0;
                                    break;
                            }

                        }
                    }
                }



            }

        }

        private void fill_FormsODV_1_5_List()
        {
            foreach (var item in FormsODV_1_5_List)
            {

            }

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            //ODV1 = null;
            this.Close();
        }


        private void ODV1_5_Grid_update()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            int rowindex = 0;
            string currId = "";
            if (ODV1_5_Grid.RowCount > 0 && ODV1_5_Grid.CurrentRow != null)
            {
                rowindex = ODV1_5_Grid.CurrentRow.Index;
                currId = ODV1_5_Grid.CurrentRow.Cells[0].Value.ToString();
            }

            SortDescriptor descriptor = new SortDescriptor();
            if (ODV1_5_Grid.MasterTemplate.SortDescriptors.Any())
            {
                descriptor = ODV1_5_Grid.MasterTemplate.SortDescriptors.First();
            }
            else
            {
                descriptor.PropertyName = "Num";
                descriptor.Direction = ListSortDirection.Ascending;
            }

//            ODV1_5_Grid.Rows.Clear();
            ODV1_5_Grid.TableElement.BeginUpdate();

            ODV1_5_Grid.DataSource = FormsODV_1_5_List.OrderBy(x => x.Num);
            ODV1_5_Grid.AllowDeleteRow = true;

            if (descriptor != null)
            {
                ODV1_5_Grid.MasterTemplate.SortDescriptors.Add(descriptor);
            }



            if (FormsODV_1_5_List.Count() != 0)
            {
                ODV1_5_Grid.Columns["ID"].IsVisible = false;
                ODV1_5_Grid.Columns["FormsODV_1_2017_ID"].IsVisible = false;
                ODV1_5_Grid.Columns["FormsODV_1_5_2017_OUT"].IsVisible = false;
                ODV1_5_Grid.Columns["FormsODV_1_2017"].IsVisible = false;
                ODV1_5_Grid.Columns["Num"].Width = 80;
                ODV1_5_Grid.Columns["Num"].HeaderText = "№";
                ODV1_5_Grid.Columns["Department"].Width = 120;
                ODV1_5_Grid.Columns["Department"].HeaderText = "Наименование подразделения";
                ODV1_5_Grid.Columns["Profession"].Width = 100;
                ODV1_5_Grid.Columns["Profession"].HeaderText = "Профессия, должность";
                ODV1_5_Grid.Columns["StaffCountShtat"].Width = 100;
                ODV1_5_Grid.Columns["StaffCountShtat"].HeaderText = "Кол-во штат";
                ODV1_5_Grid.Columns["StaffCountFakt"].Width = 100;
                ODV1_5_Grid.Columns["StaffCountFakt"].HeaderText = "Кол-во факт";
                ODV1_5_Grid.Columns["VidRabotFakt"].Width = 80;
                ODV1_5_Grid.Columns["VidRabotFakt"].HeaderText = "Характер фактически выполняемых работ и дополнительные условия труда";
                ODV1_5_Grid.Columns["DocsName"].Width = 80;
                ODV1_5_Grid.Columns["DocsName"].HeaderText = "Наименование  первичных документов, подтверждающих занятость в особых условиях труда";

                for (var i = 0; i < ODV1_5_Grid.Columns.Count; i++)
                {
                    ODV1_5_Grid.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                    ODV1_5_Grid.Columns[i].ReadOnly = true;
                }

                this.ODV1_5_Grid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;


            }


            ODV1_5_Grid.TableElement.EndUpdate();

            //            odv1GridView.Refresh();


            if (ODV1_5_Grid.ChildRows.Count > 0)
            {
                if (ODV1_5_Grid.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    ODV1_5_Grid.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= ODV1_5_Grid.ChildRows.Count)
                        rowindex = ODV1_5_Grid.ChildRows.Count - 1;
                    rowindex = rowindex >= 0 ? rowindex : 0;
                    ODV1_5_Grid.ChildRows[rowindex].IsCurrent = true;
                }
            }
        }

        private void ODV1_4_Grid_update()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            ODV1_4_Grid.Rows.Clear();

            if (FormsODV_1_4_List.Count() != 0)
            {
                foreach (var item in FormsODV_1_4_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.ODV1_4_Grid.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Year"].Value = item.Year;
                    rowInfo.Cells["OPS"].Value = item.OPS.HasValue ? item.OPS.Value : 0;
                    rowInfo.Cells["NAKOP"].Value = item.NAKOP.HasValue ? item.NAKOP.Value : 0;
                    rowInfo.Cells["DopTar"].Value = item.DopTar.HasValue ? item.DopTar.Value : 0;
                    ODV1_4_Grid.Rows.Add(rowInfo);
                }
            }

            this.ODV1_4_Grid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            ODV1_4_Grid.Refresh();
        }

        private void FormsODV1_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (allowClose)
            {
                if (cleanData)
                    ODV1 = null;
                db.Dispose();
            }
            else
            {
                DialogResult dialogResult = RadMessageBox.Show("Вы хотите сохранить изменения перед закрытием формы?", "Сохранение записи!", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button3);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        radButton1_Click(null, null);
                        break;
                    case DialogResult.No:
                        if (cleanData)
                            ODV1 = null;
                        db.Dispose();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }

            }
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            cleanData = true;

            if (Quarter.SelectedItem != null)
                setPeriod();

            try
            {
                getValues();
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы ОДВ-1. Код ошибки: " + ex.Message);
            }

            ODV1.ConfirmLastName = ConfirmLastName.Text;
            ODV1.ConfirmFirstName = ConfirmFirstName.Text;
            ODV1.ConfirmMiddleName = ConfirmMiddleName.Text;
            ODV1.ConfirmDolgn = ConfirmDolgn.Text;
            ODV1.InsurerID = Options.InsID;
            ODV1.Year = short.Parse(Year.Text);
            ODV1.Code = period;
            ODV1.TypeInfo = byte.Parse(TypeInfo.SelectedItem.Tag.ToString());
            ODV1.TypeForm = byte.Parse(TypeForm.SelectedItem.Tag.ToString());
            ODV1.DateFilling = DateFilling.Value;
            long StaffCount = 0;

            switch (typeForm)
            {
                case "1":
                    if (!String.IsNullOrEmpty(SZV_STAJ_Cnt.Text))
                    {
                        long.TryParse(SZV_STAJ_Cnt.Text, out StaffCount);
                    }
                    break;
                case "2":
                    if (!String.IsNullOrEmpty(SZV_ISH_Cnt.Text))
                    {
                        long.TryParse(SZV_ISH_Cnt.Text, out StaffCount);
                    }
                    break;
                case "3":
                    if (!String.IsNullOrEmpty(SZV_KORR_Cnt.Text))
                    {
                        long.TryParse(SZV_KORR_Cnt.Text, out StaffCount);
                    }
                    break;
            }
            ODV1.StaffCount = StaffCount;

            ODV1.StaffCountOsobUslShtat = (decimal)StaffCountOsobUslShtat.Value;

            ODV1.StaffCountOsobUslFakt = (long)StaffCountOsobUslFakt.Value;


            switch (action)
            {
                case "add":
                    try
                    {
                        var fields = typeof(FormsODV_1_4_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        var names = Array.ConvertAll(fields, field => field.Name);

                        //таблица 4
                        foreach (var item in FormsODV_1_4_List)
                        {
                            //item.FormsODV_1_2017_ID = ODV1.ID;
                            FormsODV_1_4_2017 r = new FormsODV_1_4_2017();

                            foreach (var itemName_ in names)
                            {
                                string itemName = itemName_.TrimStart('_');
                                var properties = item.GetType().GetProperty(itemName);
                                if (properties != null)
                                {
                                    object value = properties.GetValue(item, null);
                                    var data = value;

                                    r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                }

                            }

                            ODV1.FormsODV_1_4_2017.Add(r);

                        }

                        fields = typeof(FormsODV_1_5_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        names = Array.ConvertAll(fields, field => field.Name);

                        //таблица 5
                        foreach (var item in FormsODV_1_5_List)
                        {
                            //item.FormsODV_1_2017_ID = ODV1.ID;
                            FormsODV_1_5_2017 r = new FormsODV_1_5_2017();

                            foreach (var itemName_ in names)
                            {
                                string itemName = itemName_.TrimStart('_');
                                var properties = item.GetType().GetProperty(itemName);
                                if (properties != null)
                                {
                                    object value = properties.GetValue(item, null);
                                    var data = value;

                                    r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                }

                            }

                            foreach (var it in item.FormsODV_1_5_2017_OUT.ToList())
                            {
                                r.FormsODV_1_5_2017_OUT.Add(it);
                            }

                            ODV1.FormsODV_1_5_2017.Add(r);

                        }

                        db.FormsODV_1_2017.Add(ODV1);
                        try
                        {
                            db.SaveChanges();
                            cleanData = false;
                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show(this, "Во время сохранения Формы ОДВ-1 произошла ошибка! Код ошибки - " + ex.Message);
                        }


                    }
                    catch { }
                    break;
                case "edit":   // режим редактирования записи
                    // выбираем из базы исходную запись по идешнику
                    db = new pu6Entities();

                    FormsODV_1_2017 odv_1_ish = db.FormsODV_1_2017.FirstOrDefault(x => x.ID == ODV1.ID);
                    try
                    {
                        try
                        {
                            var fields = typeof(FormsODV_1_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            foreach (var itemName_ in names)
                            {
                                string itemName = itemName_.TrimStart('_');
                                var properties = ODV1.GetType().GetProperty(itemName);
                                if (properties != null)
                                {
                                    object value = properties.GetValue(ODV1, null);
                                    var data = value;

                                    odv_1_ish.GetType().GetProperty(itemName).SetValue(odv_1_ish, data, null);
                                }

                            }


                            // сохраняем модифицированную запись обратно в бд
                            db.Entry(odv_1_ish).State= EntityState.Modified;
                            // сохраняем модифицированную запись обратно в бд
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранении основных данных произошла ошибка. Код ошибки: " + ex.Message);
                        }


                        try
                        {
                            var FormsODV_1_4_2017_List_from_db = db.FormsODV_1_4_2017.Where(x => x.FormsODV_1_2017_ID == odv_1_ish.ID);

                            // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                            var t = FormsODV_1_4_List.Select(x => x.ID);
                            var list_for_del = FormsODV_1_4_2017_List_from_db.Where(x => !t.Contains(x.ID));

                            foreach (var item in list_for_del)
                            {
                                db.FormsODV_1_4_2017.Remove(item);
                            }

                            if (list_for_del.Count() != 0)
                            {
                                //db.SaveChanges();
                                FormsODV_1_4_2017_List_from_db = db.FormsODV_1_4_2017.Where(x => x.FormsODV_1_2017_ID == odv_1_ish.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                            }

                            var fields = typeof(FormsODV_1_4_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            foreach (var item in FormsODV_1_4_List)
                            {
                                item.FormsODV_1_2017_ID = odv_1_ish.ID;
                                FormsODV_1_4_2017 r = new FormsODV_1_4_2017();
                                bool exist = false;

                                if (item.ID != 0)
                                {
                                    r = FormsODV_1_4_2017_List_from_db.First(x => x.ID == item.ID);
                                    exist = true;
                                }

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = item.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(item, null);
                                        var data = value;

                                        r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                    }

                                }

                                if (!exist)
                                    db.FormsODV_1_4_2017.Add(r);
                                else
                                    db.Entry(r).State = EntityState.Modified;

                            }

                            try
                            {
                                db.SaveChanges();
                                cleanData = false;
                            }
                            catch { }

                        }
                        catch { }

                        try
                        {
                            var fields = typeof(FormsODV_1_5_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            var FormsODV_1_5_2017_List_from_db = db.FormsODV_1_5_2017.Where(x => x.FormsODV_1_2017_ID == odv_1_ish.ID);

                            // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                            var t = FormsODV_1_5_List.Select(x => x.ID);
                            var list_for_del = FormsODV_1_5_2017_List_from_db.Where(x => !t.Contains(x.ID));

                            foreach (var item in list_for_del)
                            {
                                db.FormsODV_1_5_2017.Remove(item);
                            }

                            if (list_for_del.Count() != 0)
                            {
                                //db.SaveChanges();
                                FormsODV_1_5_2017_List_from_db = db.FormsODV_1_5_2017.Where(x => x.FormsODV_1_2017_ID == odv_1_ish.ID && !list_for_del.Select(y => y.ID).Contains(x.ID));
                            }


                            foreach (var item in FormsODV_1_5_List)
                            {
                                item.FormsODV_1_2017_ID = odv_1_ish.ID;
                                FormsODV_1_5_2017 r = new FormsODV_1_5_2017();
                                bool exist = false;

                                if (item.ID != 0)
                                {
                                    r = FormsODV_1_5_2017_List_from_db.First(x => x.ID == item.ID);
                                    exist = true;
                                }

                                foreach (var itemName_ in names)
                                {
                                    string itemName = itemName_.TrimStart('_');
                                    var properties = item.GetType().GetProperty(itemName);
                                    if (properties != null)
                                    {
                                        object value = properties.GetValue(item, null);
                                        var data = value;

                                        r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                    }

                                }

                                if (!exist)
                                {
                                    foreach (var it in item.FormsODV_1_5_2017_OUT.ToList())
                                    {
                                        r.FormsODV_1_5_2017_OUT.Add(it);
                                    }
                                    db.FormsODV_1_5_2017.Add(r);

                                }
                                else
                                {


                                    var tt = r.FormsODV_1_5_2017_OUT.Where(x => x.ID == 0).ToList();

                                    foreach (var item_ in tt)
                                    {
                                        r.FormsODV_1_5_2017_OUT.Remove(item_);
                                    }

                                    var FormsODV_1_5_2017_OUT_List_from_db = r.FormsODV_1_5_2017_OUT.ToList();

                                    // проверка на удаление записей, если в базе есть записи которых нет в текущей версии после редактирования, то удаляем их
                                    var t__ = item.FormsODV_1_5_2017_OUT.Select(x => x.ID);
                                    var list_for_del_ = FormsODV_1_5_2017_OUT_List_from_db.Where(x => !t__.Contains(x.ID));

                                    foreach (var item_ in list_for_del_)
                                    {
                                     //   r.FormsODV_1_5_2017_OUT.Remove(item_);
                                        db.FormsODV_1_5_2017_OUT.Remove(item_);
                                    }

                                    if (list_for_del_.Count() != 0)
                                    {
                                        FormsODV_1_5_2017_OUT_List_from_db = r.FormsODV_1_5_2017_OUT.Where(x => !list_for_del_.Select(y => y.ID).Contains(x.ID)).ToList();
                                    }

                                    var fields_ = typeof(FormsODV_1_5_2017_OUT).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                    var names_ = Array.ConvertAll(fields_, field => field.Name);

                                    foreach (var item_ in item.FormsODV_1_5_2017_OUT.ToList())
                                    {
                                        item_.FormsODV_1_5_2017_ID = r.ID;
                                        FormsODV_1_5_2017_OUT r_ = new FormsODV_1_5_2017_OUT();
                                        bool exist_ = false;

                                        if (item_.ID != 0)
                                        {
                                            r_ = FormsODV_1_5_2017_OUT_List_from_db.First(x => x.ID == item_.ID);
                                            exist_ = true;
                                        }

                                        foreach (var itemName_ in names_)
                                        {
                                            string itemName = itemName_.TrimStart('_');
                                            var properties = item_.GetType().GetProperty(itemName);
                                            if (properties != null)
                                            {
                                                object value = properties.GetValue(item_, null);
                                                var data = value;

                                                r_.GetType().GetProperty(itemName).SetValue(r_, data, null);
                                            }

                                        }

                                        if (!exist_)
                                        {
                                            r.FormsODV_1_5_2017_OUT.Add(r_);
                                        }
                                        //else
                                        //    db.Entry(r, EntityState.Modified);

                                    }

                                    db.Entry(r).State = EntityState.Modified;
                                }

                            }

                            try
                            {
                                db.SaveChanges();
                                cleanData = false;
                            }
                            catch { }

                        }
                        catch { }

                    }
                    catch { }
                    break;
            }

            allowClose = true;
            this.Close();
        }

        /// <summary>
        /// Сбор данных с основной экранной формы редактировния формы РСВ-1
        /// </summary>
        private void getValues()
        {
            var fields = typeof(FormsODV_1_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var names = Array.ConvertAll(fields, field => field.Name);

            foreach (var item in names)
            {
                string itemName = item.TrimStart('_');
                if (itemName.StartsWith("s_"))
                {
                    try
                    {
                        if (this.Controls.Find(itemName, true).Any())
                        {
                            //   DevExpress.XtraEditors.TextEdit box = (DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0];

                            string type = ODV1.GetType().GetProperty(itemName).PropertyType.FullName;
                            if (type.Contains("["))
                                type = type.Substring(type.IndexOf('[') + 2, type.Length - type.IndexOf('[') - 4);
                            type = type.Split(',')[0].Split('.')[1].ToLower();
                            if (((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text != "")
                            {
                                switch (type)
                                {
                                    case "decimal":
                                        ODV1.GetType().GetProperty(itemName).SetValue(ODV1, Math.Round(decimal.Parse(((DevExpress.XtraEditors.TextEdit)this.Controls.Find(itemName, true)[0]).Text), 2, MidpointRounding.AwayFromZero), null);
                                        break;
                                }
                            }
                            else
                                ODV1.GetType().GetProperty(itemName).SetValue(ODV1, null, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(ex.Message + "    " + itemName);
                    }
                }

            }
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (ODV1.ID == 0) //если форма еще не сохранена
            {
                notClose = true;
                radButton1_Click(null, null);
                notClose = false;
            }


            if (ODV1.ID != 0)
            {
                ReportViewer = new ODV1_Print();
                ReportViewer.SZV_STAJ_List = SZV_STAJ_List;
                ReportViewer.ODV1_Data = ODV1;
                ReportViewer.printODV1 = true;
                ReportViewer.Owner = this;
                ReportViewer.ThemeName = this.ThemeName;
                ReportViewer.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewer.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Форму не удалось сохранить перед печатью", "");
            }



        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReportViewer.ShowDialog();
        }

        private void DateFilling_ValueChanged(object sender, EventArgs e)
        {
            if (DateFilling.Value != DateFilling.NullDate)
                DateFillingMaskedEditBox.Text = DateFilling.Value.ToShortDateString();
            else
                DateFillingMaskedEditBox.Text = DateFillingMaskedEditBox.NullText;
        }

        private void DateFillingMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateFillingMaskedEditBox.Text != DateFillingMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateFillingMaskedEditBox.Text, out date))
                {
                    DateFilling.Value = date;
                }
                else
                {
                    DateFilling.Value = DateFilling.NullDate;
                }
            }
            else
            {
                DateFilling.Value = DateFilling.NullDate;
            }
        }

        private void updateStaffCnt()
        {
            StaffCountOsobUslShtat.Value = FormsODV_1_5_List.Where(x => x.StaffCountShtat.HasValue).Sum(x => x.StaffCountShtat.Value);
            StaffCountOsobUslFakt.Value = FormsODV_1_5_List.Where(x => x.StaffCountFakt.HasValue).Sum(x => x.StaffCountFakt.Value);
        }

        private void radButton12_Click(object sender, EventArgs e)
        {
            ODV_1_5_Edit child = new ODV_1_5_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.existedNum = FormsODV_1_5_List.Any() ? FormsODV_1_5_List.Select(x => x.Num.Value).ToList() : new List<short>();
            child.cnt = FormsODV_1_5_List.Any() ? FormsODV_1_5_List.Max(x => x.Num).Value : (short)0;
            child.ShowDialog();
            if (child.formData != null)
            {
                FormsODV_1_5_List.Add(child.formData);
                ODV1_5_Grid_update();
                updateStaffCnt();
            }

        }

        private void radButton11_Click(object sender, EventArgs e)
        {
            if (ODV1_5_Grid.RowCount > 0 && ODV1_5_Grid.CurrentRow.Cells[0].Value != null)
            {
                long id = (long)ODV1_5_Grid.CurrentRow.Cells[0].Value;
                bool Good = false;
                int ind = 0;

                FormsODV_1_5_2017 item = new FormsODV_1_5_2017();

                if (id != 0)
                {
                    item = FormsODV_1_5_List.First(x => x.ID == id);

                    ind = FormsODV_1_5_List.IndexOf(item);
                    Good = true;
                }
                else
                {
                    var Num = (Int16)ODV1_5_Grid.CurrentRow.Cells["Num"].Value;
                    if (FormsODV_1_5_List.Where(x => x.Num == Num).Count() == 1)
                    {
                        item = FormsODV_1_5_List.First(x => x.Num == Num);

                        ind = FormsODV_1_5_List.IndexOf(item);
                        Good = true;
                    }
                }

                if (Good)
                {

                    ODV_1_5_Edit child = new ODV_1_5_Edit();
                    child.Owner = this;
                    child.ThemeName = this.ThemeName;
                    child.ShowInTaskbar = false;
                    child.existedNum = FormsODV_1_5_List.Select(x => x.Num.Value).ToList();
                    child.formData = item;
                    child.ShowDialog();

                    if (child.formData != null)
                    {



                        FormsODV_1_5_List.RemoveAt(ind);
                        FormsODV_1_5_List.Insert(ind, child.formData);

                        ODV1_5_Grid_update();
                        updateStaffCnt();

                    }
                }
                else
                {
                    RadMessageBox.Show(this, "Не получилось открыть данные для редактирования! Попробуйте сохранить и закрыть текущую Форму ОДВ-1 и открыть заново!");
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void radButton10_Click(object sender, EventArgs e)
        {
            if (ODV1_5_Grid.RowCount > 0 && ODV1_5_Grid.CurrentRow.Cells[0].Value != null)
            {
                bool delGood = false;

                long id = (long)ODV1_5_Grid.CurrentRow.Cells[0].Value;

                if (id != 0)
                {
                    var item = FormsODV_1_5_List.First(x => x.ID == id);
                    delGood = FormsODV_1_5_List.Remove(item);
                }
                else
                {
                    var Num = (Int16)ODV1_5_Grid.CurrentRow.Cells["Num"].Value;
                    if (FormsODV_1_5_List.Where(x => x.Num == Num).Count() == 1)
                    {
                        var item = FormsODV_1_5_List.First(x => x.Num == Num);
                        delGood = FormsODV_1_5_List.Remove(item);
                    }
                }


                if (!delGood)   
                    RadMessageBox.Show(this, "При удалении записи произошла ошибка! Попробуйте сохранить и закрыть текущую Форму ОДВ-1 и открыть заново и попробуйте удалить лишнюю запись!", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error, MessageBoxDefaultButton.Button1);


                    ODV1_5_Grid_update();

            }


            //if (ODV1_5_Grid.RowCount != 0)
            //{
            //    int rowindex = ODV1_5_Grid.CurrentRow.Index;
            //    FormsODV_1_5_List.RemoveAt(rowindex);

            //    ODV1_5_Grid_update();
            //    updateStaffCnt();


            //}
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void TypeForm_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

            int year = DateTime.Now.Year;

            typeForm = TypeForm.SelectedItem.Tag.ToString();

            this.Year.Items.Clear();

            bool flag = true;
            bool flag_4 = true;
            bool flag_5 = true;

            SZV_STAJ_Cnt.Enabled = false;
            SZV_ISH_Cnt.Enabled = false;
            SZV_KORR_Cnt.Enabled = false;
            TypeInfo.Enabled = true;


            List<RaschetPeriodContainer> avail_periods = new List<RaschetPeriodContainer>();
            avail_periods_all = new List<RaschetPeriodContainer>();
            switch (typeForm)
            {
                case "1":
                    avail_periods = Options.RaschetPeriodInternal2017.OrderBy(x => x.Year).ToList();
                    foreach (var item in avail_periods)
                    {
                        avail_periods_all.Add(item);
                    }

                    avail_periods_all = avail_periods_all.OrderByDescending(x => x.Year).ToList();

                    flag = false;
                    flag_4 = false;
                    flag_5 = true;
                    SZV_STAJ_Cnt.Enabled = true;
                    foreach (var item in avail_periods_all.Where(x => x.Year >= 2017).Select(x => x.Year).ToList().Distinct())
                    {
                        Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                    }
                    break;
                case "2":
                    avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year < year).OrderBy(x => x.Year).ToList();
                    foreach (var item in avail_periods)
                    {
                        avail_periods_all.Add(item);
                    }
                    avail_periods = Options.RaschetPeriodInternal2010_2013.OrderBy(x => x.Year).ToList();
                    foreach (var item in avail_periods)
                    {
                        avail_periods_all.Add(item);
                    }
                    avail_periods = Options.RaschetPeriodInternal1996_2009.OrderBy(x => x.Year).ToList();
                    foreach (var item in avail_periods)
                    {
                        avail_periods_all.Add(item);
                    }

                    avail_periods_all = avail_periods_all.OrderByDescending(x => x.Year).ToList();


                    flag = true;
                    flag_4 = true;
                    flag_5 = true;
                    SZV_ISH_Cnt.Enabled = true;
                    foreach (var item in avail_periods_all.Where(x => x.Year < year).Select(x => x.Year).ToList().Distinct())
                    {
                        Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                    }

                    break;
                case "3":
                    avail_periods = Options.RaschetPeriodInternal2017.OrderBy(x => x.Year).ToList();
                    foreach (var item in avail_periods)
                    {
                        avail_periods_all.Add(item);
                    }

                    avail_periods_all = avail_periods_all.OrderByDescending(x => x.Year).ToList();

                    flag = true;
                    flag_4 = true;
                    flag_5 = true;
                    SZV_KORR_Cnt.Enabled = true;
                    foreach (var item in avail_periods_all.Where(x => x.Year >= 2017).Select(x => x.Year).ToList().Distinct())
                    {
                        Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                    }

                    TypeInfo.Items.Single(x => x.Tag.ToString() == "0").Selected = true;
                    TypeInfo.Enabled = false;

                    break;
                case "4":
                    avail_periods = Options.RaschetPeriodInternal2017.OrderBy(x => x.Year).ToList();
                    foreach (var item in avail_periods)
                    {
                        avail_periods_all.Add(item);
                    }

                    avail_periods_all = avail_periods_all.OrderByDescending(x => x.Year).ToList();

                    flag = true;
                    flag_4 = true;
                    flag_5 = true;
                    foreach (var item in avail_periods_all.Where(x => x.Year >= 2017).Select(x => x.Year).ToList().Distinct())
                    {
                        Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                    }

                    break;
            }

            if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                Year.Items.OrderByDescending(x => x.Value).First().Selected = true;

            radPageView1.Pages[1].Enabled = flag;

            ODV1_4_Grid.Enabled = flag_4;
            addBtn_4.Enabled = flag_4;
            editBtn_4.Enabled = flag_4;
            delBtn_4.Enabled = flag_4;

            ODV1_5_Grid.Enabled = flag_5;
            razd5_addBtn.Enabled = flag_5;
            razd5_delBtn.Enabled = flag_5;
            razd5_editBtn.Enabled = flag_5;

        }

        private void addBtn_4_Click(object sender, EventArgs e)
        {
            ODV_1_4_Edit child = new ODV_1_4_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            //            child.formData = new FormsODV_1_4_2017();
            child.ShowDialog();
            if (child.formData != null)
            {
                FormsODV_1_4_List.Add(child.formData);
                ODV1_4_Grid_update();
            }
        }

        private void editBtn_4_Click(object sender, EventArgs e)
        {
            if (ODV1_4_Grid.RowCount != 0)
            {
                int rowindex = ODV1_4_Grid.CurrentRow.Index;

                if (rowindex >= 0)
                {

                    ODV_1_4_Edit child = new ODV_1_4_Edit();
                    child.Owner = this;
                    child.ThemeName = this.ThemeName;
                    child.ShowInTaskbar = false;
                    child.formData = FormsODV_1_4_List.Skip(rowindex).Take(1).First();
                    child.ShowDialog();

                    if (child.formData != null)
                    {
                        FormsODV_1_4_List.RemoveAt(rowindex);
                        FormsODV_1_4_List.Insert(rowindex, child.formData);

                        ODV1_4_Grid_update();
                    }
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void delBtn_4_Click(object sender, EventArgs e)
        {
            if (ODV1_4_Grid.RowCount != 0 && ODV1_4_Grid.CurrentRow.Cells[0].Value != null)
            {
                int rowindex = ODV1_4_Grid.CurrentRow.Index;

                if (rowindex >= 0)
                {
                    FormsODV_1_4_List.RemoveAt(rowindex);

                    ODV1_4_Grid_update();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void ODV1_4_Grid_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.RowInfo.Group == null && e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.TextAlignment = ContentAlignment.BottomRight;
                e.CellElement.Font = totalRowsFont;
            }
        }

        private void checkCountBtn_Click(object sender, EventArgs e)
        {


            switch (typeForm)
            {
                case "1":
                    SZV_STAJ_Cnt.Text = db.FormsODV_1_2017.First(x => x.ID == ODV1ID).FormsSZV_STAJ_2017.Count().ToString();
                    break;
                case "2":
                    SZV_ISH_Cnt.Text = db.FormsODV_1_2017.First(x => x.ID == ODV1ID).FormsSZV_ISH_2017.Count().ToString();
                    break;
                case "3":
                    SZV_KORR_Cnt.Text = db.FormsODV_1_2017.First(x => x.ID == ODV1ID).FormsSZV_KORR_2017.Count().ToString();
                    break;
            }
        }

        private void razd5_reNumBtn_Click(object sender, EventArgs e)
        {
            short i = 1;
            foreach (var item in FormsODV_1_5_List)
            {
                item.Num = i;
                i++;
            }
            ODV1_5_Grid_update();
        }

        private void ODV1_5_Grid_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            radButton11_Click(null, null);
        }











    }
}
