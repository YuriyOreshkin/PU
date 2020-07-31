using PU.Classes;
using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU.FormsSZV_ISH
{
    public partial class SZV_ISH_4_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public string action { get; set; }
        private bool cleanData = true;
        private decimal startMaxSum = 0;

        public PlatCategory PlatCat { get; set; }
        public FormsSZV_ISH_4_2017 formData { get; set; }

        public List<PU.FormsODV1.ODV1_List.MonthesDict> Monthes = new List<PU.FormsODV1.ODV1_List.MonthesDict>();


        public SZV_ISH_4_Edit()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void radPanel1_MouseHover(object sender, EventArgs e)
        {
            radPanel1.Font = new Font("Segoe UI", 9, FontStyle.Underline | FontStyle.Bold);

        }

        private void radPanel1_MouseLeave(object sender, EventArgs e)
        {
            radPanel1.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        private void radPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            PU.FormsRSW2014.PlatCategoryFrm child = new PU.FormsRSW2014.PlatCategoryFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.PlatCat = PlatCat;
            child.radDropDownList1.Enabled = false;
            child.action = "selection";
            child.ShowDialog();
            PlatCat = child.PlatCat;
            long id = formData.ID;
            formData = new FormsSZV_ISH_4_2017();
            formData.ID = id;
            formData.PlatCategory = child.PlatCat == null ? null : child.PlatCat;
            changePlatCatHeader();
        }

        private void changePlatCatHeader()
        {
            if (PlatCat != null)
            {
                radPanel1.Text = PlatCat.Code + "   " + PlatCat.Name;
            }
            else
            {
                radPanel1.Text = "Категория плательщика - не определена... Нажмите для выбора";
            }
        }

        private void SZV_ISH_4_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками

            List<WindowData> windowData = new List<WindowData> { };

            windowData.Add(new WindowData
            {
                control = "fixPlatCatCheckBox",
                value = fixPlatCatCheckBox.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "PlatCat",
                value = PlatCat == null ? "" : PlatCat.ID.ToString()
            });



            props.setFormParams(this, windowData);

            db = null;
            if (cleanData)
                formData = null;
        }

        private void SZV_ISH_4_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    foreach (var item in param.windowData)
                    {

                        switch (item.control)
                        {
                            case "fixPlatCatCheckBox":
                                fixPlatCatCheckBox.Checked = item.value == "true" ? true : false;
                                break;
                            case "PlatCat":
                                if (fixPlatCatCheckBox.Checked && action == "add" && !String.IsNullOrEmpty(item.value))
                                {
                                    long id = 0;

                                    long.TryParse(item.value, out id);
                                    if (db.PlatCategory.Any(x => x.ID == id))
                                    {
                                        PlatCat = db.PlatCategory.FirstOrDefault(x => x.ID == id);

                                        if (action == "add")
                                        {
                                            formData = new FormsSZV_ISH_4_2017();
                                        }

                                        formData.PlatCategory = PlatCat == null ? null : PlatCat;
                                        changePlatCatHeader();
                                    }
                                }
                                break;
                        }
                    }

                }
                catch
                { }

            }

            foreach (var item in Monthes)
            {
                MonthesDDL.Items.Add(new RadListDataItem(item.Name, item.Code));
            }

            switch (action)
            {
                case "add":
                    if (formData == null)
                        formData = new FormsSZV_ISH_4_2017();
                    //InfoLabel.Visible = true;
                    break;
                case "edit":
                    PlatCat = formData.PlatCategory;
                    if (PlatCat != null)
                    {
                        radPanel1.Text = PlatCat.Code + "   " + PlatCat.Name;
                    }

                    if (formData.Month.HasValue && Monthes.Any(x => x.Code == formData.Month.Value))
                    {
                        MonthesDDL.Items.Single(x => (byte)x.Value == formData.Month.Value).Selected = true;
                    }

                    SumFeePFR.EditValue = formData.SumFeePFR.HasValue ? formData.SumFeePFR.Value : (decimal)0;
                    BaseALL.EditValue = formData.BaseALL.HasValue ? formData.BaseALL.Value : (decimal)0;
                    BaseGPD.EditValue = formData.BaseGPD.HasValue ? formData.BaseGPD.Value : (decimal)0;
                    SumPrevBaseALL.EditValue = formData.SumPrevBaseALL.HasValue ? formData.SumPrevBaseALL.Value : (decimal)0;
                    SumPrevBaseGPD.EditValue = formData.SumPrevBaseGPD.HasValue ? formData.SumPrevBaseGPD.Value : (decimal)0;

                    break;
            }


        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (MonthesDDL.Text == "")
            {
                RadMessageBox.Show("Необходимо выбрать месяц");
                return;
            }

            if (PlatCat != null)
            {
                getValues();
                cleanData = false;
                this.Close();
            }
            else
            {
                RadMessageBox.Show("Необходимо выбрать категорию плательщика");
                radPanel1_MouseClick(null, null);
                saveBtn_Click(null, null);

            }
        }

        private void getValues()
        {
            formData.Month = (byte)MonthesDDL.SelectedItem.Value;

            formData.SumFeePFR = Math.Round((decimal)SumFeePFR.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.BaseALL = Math.Round((decimal)BaseALL.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.BaseGPD = Math.Round((decimal)BaseGPD.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumPrevBaseALL = Math.Round((decimal)SumPrevBaseALL.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumPrevBaseGPD = Math.Round((decimal)SumPrevBaseGPD.EditValue, 2, MidpointRounding.AwayFromZero);

        }


    }
}
