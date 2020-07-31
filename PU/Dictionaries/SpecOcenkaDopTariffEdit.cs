using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using System.Linq;
using Telerik.WinControls.UI.Localization;
using PU.Classes;

namespace PU.FormsRSW2014
{
    public partial class SpecOcenkaDopTariffEdit : Telerik.WinControls.UI.RadForm
    {
        private bool cleanData = true;
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        public SpecOcenkaUslTrudaDopTariff formData { get; set; }

        public SpecOcenkaDopTariffEdit()
        {
            InitializeComponent();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SpecOcenkaDopTariffEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
            if (cleanData)
                formData = null;
        }

        private void SpecOcenkaDopTariffEdit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            typeDDL.AutoSizeItems = true;

            switch (action)
            {
                case "add":
                    break;
                case "edit":
                    updateTextBoxes();

                    break;
            }
        }

        private void getValues()
        {
            if (dateBegin.EditValue != null)
                formData.DateBegin = DateTime.Parse(dateBegin.EditValue.ToString()).Date;
            else
                formData.DateBegin = null;

            if (dateEnd.EditValue != null)
                formData.DateEnd = DateTime.Parse(dateEnd.EditValue.ToString()).Date;
            else
                formData.DateEnd = null;

            formData.Type = byte.Parse(typeDDL.SelectedItem.Tag.ToString());
            formData.Rate = decimal.Parse(rate.EditValue.ToString());
        }

        private void updateTextBoxes()
        {
            typeDDL.Items.FirstOrDefault(x => x.Tag.ToString() == formData.Type.Value.ToString()).Selected = true;
            rate.EditValue = formData.Rate.Value;
            if (formData.DateBegin.HasValue)
                dateBegin.EditValue = formData.DateBegin.Value;
            if (formData.DateEnd.HasValue)
                dateEnd.EditValue = formData.DateEnd.Value;

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            getValues();
            cleanData = false;
            this.Close();
        }


    }
}
