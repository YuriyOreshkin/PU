using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using PU.Models;
using PU.Classes;
using Telerik.WinControls.UI;
using System.Reflection;
using PU.FormsSPW2_2014;
using PU.FormsRSW2014;

namespace PU.Staj
{
    public partial class StajLgotFrm : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public string ParentFormName { get; set; }
        public StajLgot formData { get; set; }
        public StajOsn StajOsnData { get; set; }
        public int rowindex { get; set; }
        private bool setNull = true;
        private bool loading = false;

        public StajLgotFrm()
        {
            InitializeComponent();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            setNull = true;
            this.Close();
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    SaveBtn.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    break;
            }
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

        private void StajLgotFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            loading = true;

            checkAccessLevel();

            int year = StajOsnData.DateBegin.Value.Year;

            if (year <= 2001)
            {
                TerrUslKoefSpin.Maximum = 2;
            }
            else
            {
                TerrUslKoefSpin.Maximum = 1;
            }


            #region Территориальные условия загрузка DDL
            BindingSource b = new BindingSource();
            b.DataSource = db.TerrUsl;

            this.TerrUslDDL.DataSource = null;
            this.TerrUslDDL.Items.Clear();
            this.TerrUslDDL.DisplayMember = "Code";
            this.TerrUslDDL.ValueMember = "ID";
            this.TerrUslDDL.ShowItemToolTips = true;
            this.TerrUslDDL.DataSource = b.DataSource;
            this.TerrUslDDL.SelectedIndex = 0;
            this.TerrUslDDL.ResetText();
            this.TerrUslDDL.SelectedIndexChanged += (s, с) => TerrUslDDL_SelectedIndexChanged();

            #endregion

            #region Особые условия труда загрузка DDL
            b = new BindingSource();
            b.DataSource = db.OsobUslTruda;

            this.OsobUslDDL.DataSource = null;
            this.OsobUslDDL.Items.Clear();
            this.OsobUslDDL.DisplayMember = "Code";
            this.OsobUslDDL.ValueMember = "ID";
            this.OsobUslDDL.ShowItemToolTips = true;
            this.OsobUslDDL.DataSource = b.DataSource;
            this.OsobUslDDL.SelectedIndex = 0;
            this.OsobUslDDL.ResetText();
            this.OsobUslDDL.SelectedIndexChanged += (s, с) => OsobUslDDL_SelectedIndexChanged(null, null);

            #endregion

            #region Код позиции списка загрузка DDL
            b = new BindingSource();
            b.DataSource = db.KodVred_2;

            this.KodVredDDL.DataSource = null;
            this.KodVredDDL.Items.Clear();
            this.KodVredDDL.DisplayMember = "Code";
            this.KodVredDDL.ValueMember = "ID";
            this.KodVredDDL.ShowItemToolTips = true;
            this.KodVredDDL.DataSource = b.DataSource;
            this.KodVredDDL.SelectedIndex = 0;
            this.KodVredDDL.ResetText();
            this.KodVredDDL.SelectedIndexChanged += (s, с) => KodVredDDL_SelectedIndexChanged(null, null);

            #endregion


            #region Исчисление страхового стажа загрузка DDL
            b = new BindingSource();
            b.DataSource = db.IschislStrahStajOsn;

            this.StrahStajDDL.DataSource = null;
            this.StrahStajDDL.Items.Clear();
            this.StrahStajDDL.DisplayMember = "Code";
            this.StrahStajDDL.ValueMember = "ID";
            this.StrahStajDDL.ShowItemToolTips = true;
            this.StrahStajDDL.DataSource = b.DataSource;
            this.StrahStajDDL.SelectedIndex = 0;
            this.StrahStajDDL.ResetText();
            this.StrahStajDDL.SelectedIndexChanged += (s, с) => StrahStajDDL_SelectedIndexChanged();

            #endregion

            #region Исчисление страхового стажа Третий параметр (дополнительно) загрузка DDL
            b = new BindingSource();
            b.DataSource = db.IschislStrahStajDop;

            this.StrahStaj3ParamDDL.DataSource = null;
            this.StrahStaj3ParamDDL.Items.Clear();
            this.StrahStaj3ParamDDL.DisplayMember = "Code";
            this.StrahStaj3ParamDDL.ValueMember = "ID";
            this.StrahStaj3ParamDDL.ShowItemToolTips = true;
            this.StrahStaj3ParamDDL.DataSource = b.DataSource;
            this.StrahStaj3ParamDDL.SelectedIndex = 0;
            this.StrahStaj3ParamDDL.ResetText();
            this.StrahStaj3ParamDDL.SelectedIndexChanged += (s, с) => StrahStaj3ParamDDL_SelectedIndexChanged();
            #endregion


            #region Условие для досрочной трудовой пенсии загрузка DDL
            b = new BindingSource();
            b.DataSource = db.UslDosrNazn;

            this.DosrPensDDL.DataSource = null;
            this.DosrPensDDL.Items.Clear();
            this.DosrPensDDL.DisplayMember = "Code";
            this.DosrPensDDL.ValueMember = "ID";
            this.DosrPensDDL.ShowItemToolTips = true;
            this.DosrPensDDL.DataSource = b.DataSource;
            this.DosrPensDDL.SelectedIndex = 0;
            this.DosrPensDDL.ResetText();
            this.DosrPensDDL.SelectedIndexChanged += (s, с) => DosrPensDDL_SelectedIndexChanged();
            #endregion


            switch (action)
            {
                case "add":
                    formData = new StajLgot();
                    break;
                case "edit":
                    setValues();
                    break;
            }
            OsobUslDDL_SelectedIndexChanged(null, null);
            loading = false;
            checkRules();
        }


        #region Территориальные условия

        /// <summary>
        /// Всплывающие посказки для элементов, полные названия
        /// Проблема с подвисанием элемента при первом обращении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TerrUslDDL_VisualListItemFormatting(object sender, Telerik.WinControls.UI.VisualItemFormattingEventArgs args)
        {
            //         long id = long.Parse(args.VisualItem.Data.Value.ToString());

            //            args.VisualItem.ToolTipText = db.TerrUsl.FirstOrDefault(x => x.ID == id).Name;
        }

        private void TerrUslBtnClear_Click(object sender, EventArgs e)
        {
            this.TerrUslDDL.ResetText();
            this.TerrUslKoefSpin.Value = 0;
            this.TerrUslKoefLabel.Enabled = false;
            this.TerrUslKoefSpin.Enabled = false;
            checkRules();

        }

        private void TerrUslDDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            checkRules();
        }

        private void TerrUslBtnFind_Click(object sender, EventArgs e)
        {
            /*PU.Dictionaries.BaseDictionaryFormList child = new PU.Dictionaries.BaseDictionaryFormList();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.DictName = "TerrUsl";
            child.radButtonSelect.Visible = true;
            child.ShowDialog();
            if (child.item_id != 0)
            {
                long id = child.item_id;
                TerrUslDDL.Items.FirstOrDefault(x => x.Value.ToString() == id.ToString()).Selected = true;
            }
            child = null;*/


            Dictionaries.BaseDictionaryEvents.LookUp(this, TerrUslDDL, "TerrUsl");
            checkRules();
            

        }
        #endregion

        #region Особые условия труда

        /// <summary>
        /// Всплывающие посказки для элементов, полные названия
        /// Проблема с подвисанием элемента при первом обращении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void OsobUslDDL_VisualListItemFormatting(object sender, Telerik.WinControls.UI.VisualItemFormattingEventArgs args)
        {
            //         long id = long.Parse(args.VisualItem.Data.Value.ToString());

            //            args.VisualItem.ToolTipText = db.OsobUslTruda.FirstOrDefault(x => x.ID == id).Name;
        }

        private void OsobUslBtnClear_Click(object sender, EventArgs e)
        {
            this.OsobUslDDL.ResetText();
            this.KodVredLabel.Enabled = false;
            this.KodVredDDL.Enabled = false;
            this.KodVredBtnFind.Enabled = false;
            checkRules();

        }

        private void OsobUslDDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (this.OsobUslDDL.SelectedItem != null)
            {
                int year = StajOsnData.DateBegin.Value.Year;


                List<string> itemsBefore2001 = new List<string> { "ЗП12А", "ЗП12Б" };
                List<string> items = new List<string> { "27-1", "27-2" };

                if ((year >= 2002 && items.Contains(this.OsobUslDDL.SelectedItem.Text)) || (year <= 2001 && itemsBefore2001.Contains(this.OsobUslDDL.SelectedItem.Text)))
                {
                    if (!KodVredLabel.Enabled)
                    {
                        KodVredBtnClear_Click(null, null);
                        this.KodVredLabel.Enabled = true;
                        this.KodVredDDL.Enabled = true;
                        this.KodVredBtnFind.Enabled = true;
                    }
                }
                else
                {
                    if (KodVredLabel.Enabled)
                    {
                        this.KodVredLabel.Enabled = false;
                        this.KodVredDDL.Enabled = false;
                        this.KodVredBtnFind.Enabled = false;
                    }
                }
            }
            else
            {
                if (KodVredLabel.Enabled)
                {
                    this.KodVredLabel.Enabled = false;
                    this.KodVredDDL.Enabled = false;
                    this.KodVredBtnFind.Enabled = false;
                }
            }
            checkRules();
        }

        private void OsobUslBtnFind_Click(object sender, EventArgs e)
        {
            /*PU.Dictionaries.BaseDictionaryFormList child = new PU.Dictionaries.BaseDictionaryFormList();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.DictName = "OsobUslTruda";
            child.radButtonSelect.Visible = true;
            child.ShowDialog();
            if (child.item_id != 0)
            {
                long id = child.item_id;
                OsobUslDDL.Items.FirstOrDefault(x => x.Value.ToString() == id.ToString()).Selected = true;
            }
            child = null;*/
            Dictionaries.BaseDictionaryEvents.LookUp(this, OsobUslDDL, "OsobUslTruda");
            checkRules();
            
        }
        #endregion

        #region Код позиции списка

        /// <summary>
        /// Всплывающие посказки для элементов, полные названия
        /// Проблема с подвисанием элемента при первом обращении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void KodVredDDL_VisualListItemFormatting(object sender, Telerik.WinControls.UI.VisualItemFormattingEventArgs args)
        {
            //         long id = long.Parse(args.VisualItem.Data.Value.ToString());

            //            args.VisualItem.ToolTipText = db.KodVred_2.FirstOrDefault(x => x.ID == id).Name;
        }

        private void KodVredBtnClear_Click(object sender, EventArgs e)
        {
            this.KodVredDDL.ResetText();
            this.DolgnTextBox.ResetText();
            checkRules();
        }

        private void KodVredDDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (this.KodVredDDL.SelectedItem != null)
            {
                long id = long.Parse(KodVredDDL.SelectedItem.Value.ToString());
                this.DolgnTextBox.Text = db.KodVred_2.FirstOrDefault(x => x.ID == id).Name;
            }
            else
            {
                this.DolgnTextBox.ResetText();
            }
            checkRules();
        }

        private void KodVredBtnFind_Click(object sender, EventArgs e)
        {
            KodVred child = new KodVred();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.ddl1_index = (this.OsobUslDDL.SelectedItem.Text == "27-1" || this.OsobUslDDL.SelectedItem.Text == "ЗП12А") ? (byte)0 : (byte)1;
            child.btnSelection.Visible = true;
            child.ShowDialog();
            if (child.kv_osn != null)
            {
                KodVredDDL.Items.FirstOrDefault(x => x.Value.ToString() == child.kv_osn.ID.ToString()).Selected = true;
                this.DolgnTextBox.Text = child.kv_osn.Name;
            }
            child = null;
            checkRules();

        }
        #endregion

        #region Исчисление страхового стажа

        /// <summary>
        /// Всплывающие посказки для элементов, полные названия
        /// Проблема с подвисанием элемента при первом обращении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StrahStajDDL_VisualListItemFormatting(object sender, Telerik.WinControls.UI.VisualItemFormattingEventArgs args)
        {
            //         long id = long.Parse(args.VisualItem.Data.Value.ToString());

            //            args.VisualItem.ToolTipText = db.IschislStrahStajOsn.FirstOrDefault(x => x.ID == id).Name;
        }

        private void StrahStajBtnClear_Click(object sender, EventArgs e)
        {
            this.StrahStajDDL.ResetText();
            checkRules();
        }


        //событие при изменении значения исчисл. страх. стажа основание
        private void StrahStajDDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (this.StrahStajDDL.SelectedItem != null)
            {
                //              long id = long.Parse(StrahStajDDL.SelectedItem.Value.ToString());
            }
        }

        private void StrahStajBtnFind_Click(object sender, EventArgs e)
        {
           /* PU.Dictionaries.BaseDictionaryFormList child = new PU.Dictionaries.BaseDictionaryFormList();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.DictName = "IschislStrahStajOsn";
            child.radButtonSelect.Visible = true;
            child.ShowDialog();
            if (child.item_id != 0)
            {
                long id = child.item_id;
                StrahStajDDL.Items.FirstOrDefault(x => x.Value.ToString() == id.ToString()).Selected = true;
            }
            child = null;
            checkRules();*/

        }
        #endregion

        #region Исчисление страхового стажа Третий параметр (дополнительно)

        /// <summary>
        /// Всплывающие посказки для элементов, полные названия
        /// Проблема с подвисанием элемента при первом обращении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void StrahStaj3ParamDDL_VisualListItemFormatting(object sender, Telerik.WinControls.UI.VisualItemFormattingEventArgs args)
        {
            //         long id = long.Parse(args.VisualItem.Data.Value.ToString());

            //            args.VisualItem.ToolTipText = db.IschislStrahStajDop.FirstOrDefault(x => x.ID == id).Name;
        }

        private void StrahStaj3ParamBtnClear_Click(object sender, EventArgs e)
        {
            this.StrahStaj3ParamDDL.ResetText();
            checkRules();
        }


        //событие при изменении значения исчисл. страх. стажа основание
        private void StrahStaj3ParamDDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            checkRules();
        }

        private void checkRules()
        {
            if (!loading)
            {
                loading = true;
                if (this.TerrUslDDL.SelectedItem != null)
                {
                    List<string> TerrKoef = new List<string> { "РКС", "РКСМ", "МКС", "МКСР" };

                    if (TerrKoef.Contains(this.TerrUslDDL.SelectedItem.Text))
                    {
                        if (!TerrUslKoefLabel.Enabled)
                        {
                            this.TerrUslKoefLabel.Enabled = true;
                            this.TerrUslKoefSpin.Enabled = true;
                        }
                    }
                    else
                    {
                        if (TerrUslKoefLabel.Enabled)
                        {
                            TerrUslKoefSpin.Value = 0;
                            this.TerrUslKoefLabel.Enabled = false;
                            this.TerrUslKoefSpin.Enabled = false;
                        }
                    }
                }

                List<string> list_all = new List<string> { "ДЕТИ", "ДЛДЕТИ", "АДМИНИСТР", "НЕОПЛ", "ЧАЭС" };
                List<string> list_terr_usl = new List<string> { "ДОПВЫХ", "КВАЛИФ", "ОБЩЕСТ", "ОТСТРАН", "ПРОСТОЙ", "СДКРОВ", "УЧОТПУСК" };
                if (StrahStaj3ParamDDL.Text != "")
                {
                    if (list_all.Contains(StrahStaj3ParamDDL.Text))
                    {
                        TerrUslBtnClear_Click(null, null);
                        OsobUslBtnClear_Click(null, null);
                        //     KodVredBtnClear_Click(null, null);
                        StrahStajBtnClear_Click(null, null);
                        DosrPensBtnClear_Click(null, null);

                        TerrUslGrBox.Enabled = false;
                        OsobUslGrBox.Enabled = false;
                        StrahStajGrBox.Enabled = false;
                        DosrPensGrBox.Enabled = false;

                    }
                    else if (list_terr_usl.Contains(StrahStaj3ParamDDL.Text))
                    {
                        TerrUslBtnClear_Click(null, null);
                        TerrUslGrBox.Enabled = false;
                    }
                    else
                    {
                        TerrUslGrBox.Enabled = true;
                        OsobUslGrBox.Enabled = true;
                        StrahStajGrBox.Enabled = true;
                        DosrPensGrBox.Enabled = true;
                    }
                }
                else
                {
                    TerrUslGrBox.Enabled = true;
                    OsobUslGrBox.Enabled = true;
                    StrahStajGrBox.Enabled = true;
                    DosrPensGrBox.Enabled = true;
                }

                //Правило 3,1,4
                /*            if (TerrUslKoefSpin.Enabled && TerrUslKoefSpin.Value != 0)
                            {
                                StrahStaj1ParamSpin.Value = 0;
                                StrahStaj2ParamSpin.Value = 0;
                                StrahStaj1ParamSpin.Enabled = false;
                                StrahStaj2ParamSpin.Enabled = false;
                            }
                            else
                            {
                                StrahStaj1ParamSpin.Enabled = true;
                                StrahStaj2ParamSpin.Enabled = true;
                            }
                            */
                //if ((!TerrUslKoefSpin.Enabled && TerrUslDDL.Text != "" && (OsobUslDDL.Text != "" || StrahStajDDL.Text == "ЛЕПРО" || StrahStajDDL.Text == "УИК104")) || (OsobUslDDL.Text != "" && StrahStajDDL.Text == "ВОДОЛАЗ"))
                //{
                //    StrahStaj1ParamSpin.Enabled = true;
                //    StrahStaj2ParamSpin.Enabled = true;
                //}
                //else if (TerrUslKoefSpin.Enabled)
                //{
                //    StrahStaj1ParamSpin.Value = 0;
                //    StrahStaj2ParamSpin.Value = 0;
                //    StrahStaj1ParamSpin.Enabled = false;
                //    StrahStaj2ParamSpin.Enabled = false;
                //}

                loading = false;
            }


        }

        private void StrahStaj3ParamBtnFind_Click(object sender, EventArgs e)
        {
            /*PU.Dictionaries.BaseDictionaryFormList child = new PU.Dictionaries.BaseDictionaryFormList();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.DictName = "IschislStrahStajDop";
            child.radButtonSelect.Visible = true;
            child.ShowDialog();
            if (child.item_id != 0)
            {
                long id = child.item_id;
                StrahStaj3ParamDDL.Items.FirstOrDefault(x => x.Value.ToString() == id.ToString()).Selected = true;
            }
            child = null;
            checkRules();
            */
        }
        #endregion

        #region Условие для досрочной трудовой пенсии

        /// <summary>
        /// Всплывающие посказки для элементов, полные названия
        /// Проблема с подвисанием элемента при первом обращении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DosrPensDDL_VisualListItemFormatting(object sender, Telerik.WinControls.UI.VisualItemFormattingEventArgs args)
        {
            //         long id = long.Parse(args.VisualItem.Data.Value.ToString());

            //            args.VisualItem.ToolTipText = db.IschislStrahStajDop.FirstOrDefault(x => x.ID == id).Name;
        }

        private void DosrPensBtnClear_Click(object sender, EventArgs e)
        {
            this.DosrPensDDL.ResetText();
        }


        //событие при изменении значения исчисл. страх. стажа основание
        private void DosrPensDDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (this.DosrPensDDL.SelectedItem != null)
            {
                //              long id = long.Parse(StrahStajDDL.SelectedItem.Value.ToString());
            }
            checkRules();
        }

        private void DosrPensBtnFind_Click(object sender, EventArgs e)
        {
           /* PU.Dictionaries.BaseDictionaryFormList child = new PU.Dictionaries.BaseDictionaryFormList();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "selection";
            child.DictName = "UslDosrNazn";
            child.radButtonSelect.Visible = true;
            child.ShowDialog();
            if (child.item_id != 0)
            {
                long id = child.item_id;
                DosrPensDDL.Items.FirstOrDefault(x => x.Value.ToString() == id.ToString()).Selected = true;
            }
            child = null;
            */

        }
        #endregion

        #region Профессия, должность

        private void DolgnBtnFind_Click(object sender, EventArgs e)
        {

            Dictionaries.BaseDictionaryEvents.LookUp(this, DolgnTextBox, "Dolgn");

        }
        #endregion

        private void StrahStajDDL_SelectedIndexChanged()
        {

        }

        private void DosrPensDDL_SelectedIndexChanged()
        {

        }

        private void StrahStaj3ParamDDL_SelectedIndexChanged()
        {

        }

        private void StajLgotFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (setNull)
                formData = null;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            List<ErrList> validation = StajValidation();

            if (validation.Count != 0)
            {
                foreach (var item in validation)
                {
                    RadMessageBox.Show(item.name);
                }
            }
            else
            {
                if (ParentFormName == "SPW2" || ParentFormName == "SZV_6_4" || ParentFormName == "SZV_6")  // если заводим стаж для СПВ-2 и СЗВ-6-4 и СЗВ-6
                {
                    string name = "";
                    switch (ParentFormName)
                    {
                        case "SPW2":
                            name = "СПВ-2";
                            break;
                        case "SZV_6_4":
                            name = "СЗВ-6-4";
                            break;
                        case "SZV_6":
                            name = "СЗВ-6";
                            break;
                    }
                    bool flag_ok = false;
                    try
                    {
                        getValues();
                        flag_ok = true;
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы. Код ошибки: " + ex.Message);
                    }

                    if (flag_ok)
                    {
                        switch (action)
                        {
                            case "add":

                                // нет дублирующих записей, уникальность (просто добавляем)
                                if (!db.StajLgot.Any(x => x.Number == formData.Number && x.StajOsnID == formData.StajOsnID))
                                {
                                    try
                                    {
                                        formData.StajOsnID = StajOsnData.ID;
                                        db.StajLgot.Add(formData);
                                        db.SaveChanges();
                                        setNull = false;
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("При сохранении данных о льготном стаже для Формы " + name + " произошла ошибка. Код ошибки: " + ex.Message);
                                    }
                                }
                                else
                                {
                                    RadMessageBox.Show("Ошибка! Нарушение уникальности записей по ключу. ");
                                }

                                break;
                            case "edit":
                                // нет дублирующих записей, уникальность (изменяем)
                                if (!db.StajLgot.Any(x => x.Number == formData.Number && x.StajOsnID == formData.StajOsnID && x.ID != formData.ID))
                                {
                                    // выбираем из базы исходную запись по идешнику
                                    StajLgot r1 = db.StajLgot.FirstOrDefault(x => x.ID == formData.ID);
                                    try
                                    {
                                        var fields = typeof(StajLgot).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                                        var names = Array.ConvertAll(fields, field => field.Name);

                                        foreach (var itemName_ in names)
                                        {
                                            string itemName = itemName_.TrimStart('_');
                                            var properties = formData.GetType().GetProperty(itemName);
                                            if (properties != null)
                                            {
                                                object value = properties.GetValue(formData, null);
                                                var data = value;

                                                r1.GetType().GetProperty(itemName).SetValue(r1, data, null);
                                            }

                                        }


                                        // сохраняем модифицированную запись обратно в бд
                                        db.Entry(r1).State =System.Data.Entity.EntityState.Modified;
                                        db.SaveChanges();
                                        setNull = false;
                                        this.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        RadMessageBox.Show("При сохранении данных о льготном стаже для Формы " + name + " произошла ошибка. Код ошибки: " + ex.Message);
                                    }

                                }
                                else
                                {
                                    RadMessageBox.Show("Ошибка! Нарушение уникальности записей по ключу. ");
                                }
                                break;
                        }
                    }


                }
                else
                {
                    getValues();
                    setNull = false;
                    this.Close();
                }
            }

        }

        private void getValues()
        {
            //     formData = new StajLgot { };

            formData.Number = long.Parse(NumberSpin.Value.ToString());


            //Территориальные условия
            if (TerrUslDDL.Text != "")
            {
                long id = long.Parse(TerrUslDDL.SelectedItem.Value.ToString());
                formData.TerrUslID = id;
            }
            else
            {
                //  formData.TerrUsl = null;
                formData.TerrUslID = null;
            }

            //Территориальные условия  Ставка
            if (TerrUslKoefSpin.Text != "" && TerrUslKoefSpin.Enabled)
            {
                formData.TerrUslKoef = decimal.Parse(TerrUslKoefSpin.Value.ToString());
            }
            else
            {
                formData.TerrUslKoef = null;
            }

            // Особоые условия и код позиции списка Вредности
            if (OsobUslDDL.Text != "")
            {
                long id = long.Parse(OsobUslDDL.SelectedItem.Value.ToString());
                formData.OsobUslTrudaID = id;
            }
            else
            {
                //   formData.OsobUslTruda = null;
                formData.OsobUslTrudaID = null;
            }

            if (KodVredDDL.Text != "")
            {
                long id = long.Parse(KodVredDDL.SelectedItem.Value.ToString());
                formData.KodVred_OsnID = id;
            }
            else
            {
                formData.KodVred_OsnID = null;
            }


            // Профессия
            if (DolgnTextBox.Text != "")
            {

                if (db.Dolgn.Any(x => x.Name == DolgnTextBox.Text.Trim())) // если такая профессия уже есть в справочнике, то берем ее ИД
                {
                    formData.DolgnID = db.Dolgn.FirstOrDefault(x => x.Name == DolgnTextBox.Text.Trim()).ID;
                }
                else // если такой профессии нет, то добавляем ее
                {
                    Dolgn newItem = new Dolgn
                    {
                        Name = DolgnTextBox.Text.Trim()
                    };
                    db.Dolgn.Add(newItem);
                    db.SaveChanges();

                    formData.DolgnID = newItem.ID;
                }
            }
            else
            {
                formData.Dolgn = null;
                formData.DolgnID = null;
            }

            // Исчисление страхового стажа Основание
            if (StrahStajDDL.Text != "")
            {
                long id = long.Parse(StrahStajDDL.SelectedItem.Value.ToString());
                formData.IschislStrahStajOsnID = id;
            }
            else
            {
                formData.IschislStrahStajOsnID = null;
            }

            // Исчисление страхового стажа Дополнительно (Третий параметр)
            if (StrahStaj3ParamDDL.Text != "")
            {
                long id = long.Parse(StrahStaj3ParamDDL.SelectedItem.Value.ToString());
                formData.IschislStrahStajDopID = id;
            }
            else
            {
                formData.IschislStrahStajDopID = null;
            }

            //Первый параметр
            if (StrahStaj1ParamSpin.Text != "" && StrahStaj1ParamSpin.Enabled)
            {
                formData.Strah1Param = long.Parse(StrahStaj1ParamSpin.Value.ToString());
            }
            else
            {
                formData.Strah1Param = null;
            }

            //Второй параметр 
            if (StrahStaj2ParamSpin.Text != "" && StrahStaj2ParamSpin.Enabled)
            {
                formData.Strah2Param = long.Parse(StrahStaj2ParamSpin.Value.ToString());
            }
            else
            {
                formData.Strah2Param = null;
            }


            // Условие для досрочной трудовой пенсии
            if (DosrPensDDL.Text != "")
            {
                long id = long.Parse(DosrPensDDL.SelectedItem.Value.ToString());
                formData.UslDosrNaznID = id;
            }
            else
            {
                formData.UslDosrNaznID = null;
            }

            //Первый параметр
            if (DosrPens1ParamSpin.Text != "" && DosrPens1ParamSpin.Enabled)
            {
                formData.UslDosrNazn1Param = long.Parse(DosrPens1ParamSpin.Value.ToString());
            }
            else
            {
                formData.UslDosrNazn1Param = null;
            }

            //Второй параметр 
            if (DosrPens2ParamSpin.Text != "" && DosrPens2ParamSpin.Enabled)
            {
                formData.UslDosrNazn2Param = long.Parse(DosrPens2ParamSpin.Value.ToString());
            }
            else
            {
                formData.UslDosrNazn2Param = null;
            }

            //Третий параметр 
            if (DosrPens3ParamSpin.Text != "" && DosrPens3ParamSpin.Enabled)
            {
                formData.UslDosrNazn3Param = decimal.Parse(DosrPens3ParamSpin.Value.ToString());
            }
            else
            {
                formData.UslDosrNazn3Param = null;
            }
        }


        private void setValues()
        {
            NumberSpin.Value = formData.Number.Value;


            //Территориальные условия
            if (formData.TerrUslID.HasValue)
            {
                TerrUslDDL.Items.FirstOrDefault(x => x.Value.ToString() == formData.TerrUslID.Value.ToString()).Selected = true;
            }

            if (formData.TerrUslKoef.HasValue)
            {
                TerrUslKoefSpin.Value = formData.TerrUslKoef.Value;
            }

            // Особоые условия и код позиции списка Вредности
            if (formData.OsobUslTrudaID.HasValue)
            {
                OsobUslDDL.Items.FirstOrDefault(x => x.Value.ToString() == formData.OsobUslTrudaID.Value.ToString()).Selected = true;
            }

            if (formData.KodVred_OsnID.HasValue)
            {
                KodVredDDL.Items.FirstOrDefault(x => x.Value.ToString() == formData.KodVred_OsnID.Value.ToString()).Selected = true;
            }


            // Профессия
            if (formData.DolgnID.HasValue)
            {
                DolgnTextBox.Text = db.Dolgn.First(x => x.ID == formData.DolgnID).Name;
            }
            else
            {
                DolgnTextBox.ResetText();
            }


            // Исчисление страхового стажа Основание
            if (formData.IschislStrahStajOsnID.HasValue)
            {
                StrahStajDDL.Items.FirstOrDefault(x => x.Value.ToString() == formData.IschislStrahStajOsnID.Value.ToString()).Selected = true;
            }


            // Исчисление страхового стажа Дополнительно (Третий параметр)
            if (formData.IschislStrahStajDopID.HasValue)
            {
                StrahStaj3ParamDDL.Items.FirstOrDefault(x => x.Value.ToString() == formData.IschislStrahStajDopID.Value.ToString()).Selected = true;
            }

            //Первый параметр
            if (formData.Strah1Param.HasValue)
            {
                StrahStaj1ParamSpin.Value = formData.Strah1Param.Value;
            }

            //Второй параметр 
            if (formData.Strah2Param.HasValue)
            {
                StrahStaj2ParamSpin.Value = formData.Strah2Param.Value;
            }


            // Условие для досрочной трудовой пенсии
            if (formData.UslDosrNaznID.HasValue)
            {
                DosrPensDDL.Items.FirstOrDefault(x => x.Value.ToString() == formData.UslDosrNaznID.Value.ToString()).Selected = true;
            }

            //Первый параметр
            if (formData.UslDosrNazn1Param.HasValue)
            {
                DosrPens1ParamSpin.Value = formData.UslDosrNazn1Param.Value;
            }


            //Второй параметр 
            if (formData.UslDosrNazn2Param.HasValue)
            {
                DosrPens2ParamSpin.Value = formData.UslDosrNazn2Param.Value;
            }

            //Третий параметр 
            if (formData.UslDosrNazn3Param.HasValue)
            {
                DosrPens3ParamSpin.Value = formData.UslDosrNazn3Param.Value;
            }
        }

        /// <summary>
        /// Проверка на правильность запонения формы
        /// </summary>
        /// <returns></returns>
        private List<ErrList> StajValidation()
        {
            List<ErrList> ErrorList = new List<ErrList> { };  // список для занесения выяленных ошибок

            switch (action)
            {
                case "add":
                    if (StajOsnData.StajLgot.Any(x => x.Number == (long)NumberSpin.Value))
                    {
                        ErrorList.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumberSpin" });
                    }

                    if (TerrUslDDL.Text != "")
                    {
                        if (StajOsnData.StajLgot.Any(x => x.TerrUsl != null && x.TerrUsl.Code == TerrUslDDL.Text))
                        {
                            ErrorList.Add(new ErrList { name = "Код [" + TerrUslDDL.Text + "] 'Территориальных условий' уже есть в записях о льготном стаже", control = "TerrUslDDL" });
                        }
                    }
                    break;
                case "edit":
                    if (StajOsnData.StajLgot.Any(x => x.Number == (long)NumberSpin.Value && x.ID != formData.ID))
                    {
                        ErrorList.Add(new ErrList { name = "Дублирование ключу уникальности. Исправьте порядковый номер.", control = "NumberSpin" });
                    }

                    if (TerrUslDDL.Text != "")
                    {
                        if (StajOsnData.StajLgot.Any(x => x.TerrUsl != null && x.TerrUsl.Code == TerrUslDDL.Text && x.Number != NumberSpin.Value))
                        {
                            ErrorList.Add(new ErrList { name = "Код [" + TerrUslDDL.Text + "] 'Территориальных условий' уже есть в записях о льготном стаже", control = "TerrUslDDL" });
                        }
                    }
                    break;
            }

            return ErrorList;
        }


        private void StrahStajDDL_SelectedIndexChanged_1(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            checkRules();
        }

        private void TerrUslDDL_SelectedIndexChanged()
        {

        }






    }
}
