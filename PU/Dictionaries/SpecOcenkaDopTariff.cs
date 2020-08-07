using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using System.Linq;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using Telerik.WinControls.UI;

namespace PU.Dictionaries
{
    public partial class SpecOcenkaDopTariff : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public SpecOcenkaUslTruda SpecOcenka { get; set; }

        public SpecOcenkaDopTariff()
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

        public void dataGrid_upd()
        {

            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            var list = db.SpecOcenkaUslTrudaDopTariff.Where(x => x.SpecOcenkaUslTrudaID == SpecOcenka.ID);
            radGridView1.DataSource = null;
            radGridView1.Rows.Clear();
            if (list.Count() != 0)
            {
                foreach (var item in list)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView1.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    switch (item.Type){
                        case 0:
                            rowInfo.Cells["Type"].Value = "Спецоценка";
                            break;
                        case 1:
                            rowInfo.Cells["Type"].Value = "Аттестация";
                            break;
                    }

                    
                    rowInfo.Cells["Rate"].Value = item.Rate.Value;
                    if (item.DateBegin.HasValue)
                    rowInfo.Cells["DateBegin"].Value = item.DateBegin.Value.Date.ToShortDateString();
                    if (item.DateEnd.HasValue)
                        rowInfo.Cells["DateEnd"].Value = item.DateEnd.Value.Date.ToShortDateString();
                    radGridView1.Rows.Add(rowInfo);
                }
            }

            radGridView1.Refresh();

        }

        private void SpecOcenkaDopTariff_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            radLabel1.Text += "  - " + SpecOcenka.Code;
            dataGrid_upd();
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SpecOcenkaDopTariff_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;

            SpecOcenkaDopTariffEdit child = new SpecOcenkaDopTariffEdit();
            child.Owner = this;
            child.action = "add";
            child.ThemeName = this.ThemeName;
            child.formData = new SpecOcenkaUslTrudaDopTariff() { SpecOcenkaUslTrudaID = SpecOcenka.ID};
            child.ShowInTaskbar = false;
            child.ShowDialog();
            if (child.formData != null)
            try
            {
                if (!db.SpecOcenkaUslTrudaDopTariff.Any(x => x.Rate == child.formData.Rate && x.Type == child.formData.Type && x.SpecOcenkaUslTrudaID == child.formData.SpecOcenkaUslTrudaID && x.DateBegin.Value == child.formData.DateBegin.Value))
                {
                    db.SpecOcenkaUslTrudaDopTariff.Add(child.formData);
                    db.SaveChanges();
                    dataGrid_upd();
                    radGridView1.Rows[rowindex].IsCurrent = true;
                }
                else
                {
                    RadMessageBox.Show(this, "Запись не добавлена, дублирование данных", "Ошибка");
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(this, "Запись не добавлена. " + ex.Message, "Ошибка");
            }

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
            long ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

            SpecOcenkaDopTariffEdit child = new SpecOcenkaDopTariffEdit();
            child.Owner = this;
            child.action = "edit";
            child.ThemeName = this.ThemeName;
            child.formData = db.SpecOcenkaUslTrudaDopTariff.FirstOrDefault(x => x.ID == ID);
            child.ShowInTaskbar = false;
            child.ShowDialog();
            if (child.formData != null)
                try
                {
                    if (!db.SpecOcenkaUslTrudaDopTariff.Any(x => x.Rate == child.formData.Rate && x.SpecOcenkaUslTrudaID == child.formData.SpecOcenkaUslTrudaID && x.Type == child.formData.Type && x.DateBegin.Value == child.formData.DateBegin.Value && x.ID != ID))
                    {
                        SpecOcenkaUslTrudaDopTariff sc = db.SpecOcenkaUslTrudaDopTariff.FirstOrDefault(x => x.ID == ID);

                        sc.DateBegin = child.formData.DateBegin;
                        sc.DateEnd = child.formData.DateEnd;
                        sc.Rate = child.formData.Rate;
                        sc.Type = child.formData.Type;

                        db.Entry(sc).State = EntityState.Modified;
                        db.SaveChanges();
                        dataGrid_upd();
                        radGridView1.Rows[rowindex].IsCurrent = true;
                    }
                    else
                    {
                        RadMessageBox.Show(this, "Запись не изменена, дублирование данных", "Ошибка");
                    }
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show(this, "Запись не изменена. " + ex.Message, "Ошибка");
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            // Удаление текущей записи
            if (radGridView1.RowCount != 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                long id = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
                DialogResult dialogResult = RadMessageBox.Show(this, "Уверены что хотите удалить текущую запись?", "", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        rowindex = rowindex + 1 == radGridView1.RowCount ? rowindex - 1 : rowindex;
                        SpecOcenkaUslTrudaDopTariff sc = db.SpecOcenkaUslTrudaDopTariff.FirstOrDefault(x => x.ID == id);

                        db.SpecOcenkaUslTrudaDopTariff.Remove(sc);
                        db.SaveChanges();
                        dataGrid_upd();

                        if (radGridView1.RowCount != 0)
                        {
                            radGridView1.Rows[rowindex].IsCurrent = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении данных произошла ошибка! " + ex, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);


                    }

                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!", "");
            }
        }






    }
}
