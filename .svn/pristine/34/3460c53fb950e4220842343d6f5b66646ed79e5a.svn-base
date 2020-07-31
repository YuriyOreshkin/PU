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

namespace PU.FormsSZV_KORR
{
    public partial class SZV_KORR_4_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public string action { get; set; }
        private bool cleanData = true;
        private decimal startMaxSum = 0;

        public FormsSZV_KORR_4_2017 formData { get; set; }

        public List<PU.FormsODV1.ODV1_List.MonthesDict> Monthes = new List<PU.FormsODV1.ODV1_List.MonthesDict>();


        public SZV_KORR_4_Edit()
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

        private void SZV_KORR_4_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            db = null;
            if (cleanData)
                formData = null;
        }

        private void SZV_KORR_4_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);


            foreach (var item in Monthes)
            {
                MonthesDDL.Items.Add(new RadListDataItem(item.Name, item.Code));
            }

            switch (action)
            {
                case "add":
                    if (formData == null)
                        formData = new FormsSZV_KORR_4_2017();
                    break;
                case "edit":

                    if (formData.Month.HasValue && Monthes.Any(x => x.Code == formData.Month.Value))
                    {
                        MonthesDDL.Items.Single(x => (byte)x.Value == formData.Month.Value).Selected = true;
                    }

                    SumFeePFR.EditValue = formData.SumFeePFR.HasValue ? formData.SumFeePFR.Value : (decimal)0;
                    BaseALL.EditValue = formData.BaseALL.HasValue ? formData.BaseALL.Value : (decimal)0;
                    BaseGPD.EditValue = formData.BaseGPD.HasValue ? formData.BaseGPD.Value : (decimal)0;
                    SumPrevBaseALL.EditValue = formData.SumPrevBaseALL.HasValue ? formData.SumPrevBaseALL.Value : (decimal)0;
                    SumPrevBaseGPD.EditValue = formData.SumPrevBaseGPD.HasValue ? formData.SumPrevBaseGPD.Value : (decimal)0;

                    SumFeeBefore2001Insurer.EditValue = formData.SumFeeBefore2001Insurer.HasValue ? formData.SumFeeBefore2001Insurer.Value : (decimal)0;
                    SumFeeBefore2001Staff.EditValue = formData.SumFeeBefore2001Staff.HasValue ? formData.SumFeeBefore2001Staff.Value : (decimal)0;
                    SumFeeAfter2001STRAH.EditValue = formData.SumFeeAfter2001STRAH.HasValue ? formData.SumFeeAfter2001STRAH.Value : (decimal)0;
                    SumFeeAfter2001NAKOP.EditValue = formData.SumFeeAfter2001NAKOP.HasValue ? formData.SumFeeAfter2001NAKOP.Value : (decimal)0;
                    SumFeeTarSV.EditValue = formData.SumFeeTarSV.HasValue ? formData.SumFeeTarSV.Value : (decimal)0;
                    SumPaySTRAH.EditValue = formData.SumPaySTRAH.HasValue ? formData.SumPaySTRAH.Value : (decimal)0;
                    SumPayNAKOP.EditValue = formData.SumPayNAKOP.HasValue ? formData.SumPayNAKOP.Value : (decimal)0;



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

                getValues();
                cleanData = false;
                this.Close();
        }

        private void getValues()
        {
            formData.Month = (byte)MonthesDDL.SelectedItem.Value;

            formData.SumFeePFR = Math.Round((decimal)SumFeePFR.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.BaseALL = Math.Round((decimal)BaseALL.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.BaseGPD = Math.Round((decimal)BaseGPD.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumPrevBaseALL = Math.Round((decimal)SumPrevBaseALL.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumPrevBaseGPD = Math.Round((decimal)SumPrevBaseGPD.EditValue, 2, MidpointRounding.AwayFromZero);

            formData.SumFeeBefore2001Insurer = Math.Round((decimal)SumFeeBefore2001Insurer.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumFeeBefore2001Staff = Math.Round((decimal)SumFeeBefore2001Staff.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumFeeAfter2001STRAH = Math.Round((decimal)SumFeeAfter2001STRAH.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumFeeAfter2001NAKOP = Math.Round((decimal)SumFeeAfter2001NAKOP.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumFeeTarSV = Math.Round((decimal)SumFeeTarSV.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumPaySTRAH = Math.Round((decimal)SumPaySTRAH.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.SumPayNAKOP = Math.Round((decimal)SumPayNAKOP.EditValue, 2, MidpointRounding.AwayFromZero);

        }

    }
}
