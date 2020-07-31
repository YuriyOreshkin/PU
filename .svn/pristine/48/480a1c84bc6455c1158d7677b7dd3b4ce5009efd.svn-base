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
    public partial class SZV_KORR_5_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public string action { get; set; }
        private bool cleanData = true;
        private decimal startMaxSum = 0;

        public FormsSZV_KORR_5_2017 formData { get; set; }

        public List<PU.FormsODV1.ODV1_List.MonthesDict> Monthes = new List<PU.FormsODV1.ODV1_List.MonthesDict>();


        public SZV_KORR_5_Edit()
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

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SZV_KORR_5_Edit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            BindingSource b = new BindingSource();
            b.DataSource = db.SpecOcenkaUslTruda;

            this.SpecOcenka.DataSource = null;
            this.SpecOcenka.Items.Clear();
            this.SpecOcenka.DisplayMember = "Code";
            this.SpecOcenka.ValueMember = "ID";
            this.SpecOcenka.ShowItemToolTips = true;
            this.SpecOcenka.DataSource = b.DataSource;
            this.SpecOcenka.SelectedIndex = -1;
            //this.SpecOcenka.ResetText();

            foreach (var item in Monthes)
            {
                MonthesDDL.Items.Add(new RadListDataItem(item.Name, item.Code));
            }

            switch (action)
            {
                case "add":
                    if (formData == null)
                        formData = new FormsSZV_KORR_5_2017();
                    break;
                case "edit":

                    if (formData.Month.HasValue && Monthes.Any(x => x.Code == formData.Month.Value))
                    {
                        MonthesDDL.Items.Single(x => (byte)x.Value == formData.Month.Value).Selected = true;
                    }

                    if (formData.SpecOcenkaUslTrudaID.HasValue)
                    {
                        SpecOcenka.Items.Single(x => (long)x.Value == formData.SpecOcenkaUslTrudaID.Value).Selected = true;
                    }

                    s_0.EditValue = formData.s_0.HasValue ? formData.s_0.Value : (decimal)0;
                    s_1.EditValue = formData.s_1.HasValue ? formData.s_1.Value : (decimal)0;

                    formData = new FormsSZV_KORR_5_2017();

                    break;
            }

        }

        private void SZV_KORR_5_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            db = null;
            if (cleanData)
                formData = null;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (MonthesDDL.Text == "")
            {
                RadMessageBox.Show("Необходимо выбрать месяц");
                return;
            }

            if (SpecOcenka.Text == "")
            {
                RadMessageBox.Show("Необходимо выбрать Код спец. оценки труда");
                return;
            }

            formData.Month = (byte)MonthesDDL.SelectedItem.Value;

            long id = (long)SpecOcenka.SelectedItem.Value;
            formData.SpecOcenkaUslTruda = db.SpecOcenkaUslTruda.First(x => x.ID == id);
          //  formData.SpecOcenkaUslTrudaID = id;

            formData.s_0 = Math.Round((decimal)s_0.EditValue, 2, MidpointRounding.AwayFromZero);
            formData.s_1 = Math.Round((decimal)s_1.EditValue, 2, MidpointRounding.AwayFromZero);

            cleanData = false;
            this.Close();
        }
    }
}
