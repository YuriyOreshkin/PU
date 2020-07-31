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
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using Telerik.WinControls.UI;

namespace PU.ZAGS.Zags_Death
{
    public partial class Death_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public Death_List()
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
            this.Close();
        }

        private void Death_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            DeathGrid_upd();
        }

        public void DeathGrid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (DeathGridView.ChildRows.Count > 0 && DeathGridView.CurrentRow.Cells[0].Value != null && DeathGridView.CurrentRow.Index >= 0)
            {
                rowindex = DeathGridView.CurrentRow.Index;
                currId = DeathGridView.CurrentRow.Cells[0].Value.ToString();
            }

            this.DeathGridView.TableElement.BeginUpdate();
            DeathGridView.Rows.Clear();

            var list = db.ZAGS_Death;

            if (list.Count() != 0)
            {
                foreach (var item in list)
                {
                    string dateB = "";

                    if (item.Type_DateBirth.HasValue)
                        if (item.Type_DateBirth.Value == 1)
                        {
                            dateB = item.DateBirthDay_Os.HasValue ? item.DateBirthDay_Os.Value.ToString().PadLeft(2, '0') : "";
                            dateB += item.DateBirthMonth_Os.HasValue ? ("." + item.DateBirthMonth_Os.Value.ToString().PadLeft(2, '0')) : "";
                            dateB += item.DateBirthYear_Os.HasValue ? ("." + item.DateBirthYear_Os.Value.ToString()) : "";

                        }
                        else
                            dateB = item.DateBirth.HasValue ? item.DateBirth.Value.ToString("dd.MM.yyyy") : "";

                    string dateD = "";

                    if (item.Type_DateDeath.HasValue)
                        if (item.Type_DateDeath.Value == 1)
                        {
                            dateD = item.DateDeathDay_Os.HasValue ? item.DateDeathDay_Os.Value.ToString().PadLeft(2, '0') : "";
                            dateD += item.DateDeathMonth_Os.HasValue ? ("." + item.DateDeathMonth_Os.Value.ToString().PadLeft(2, '0')) : "";
                            dateD += item.DateDeathYear_Os.HasValue ? ("." + item.DateDeathYear_Os.Value.ToString()) : "";

                        }
                        else
                            dateD = item.DateDeath.HasValue ? item.DateDeath.Value.ToString("dd.MM.yyyy") : "";



                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.DeathGridView.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["FIO"].Value = item.LastName.Trim() + " " + item.FirstName.Trim() + " " + item.MiddleName.Trim();
                    rowInfo.Cells["Sex"].Value = item.Sex.HasValue ? (item.Sex.Value == 0 ? "М" : "Ж") : "";
                    rowInfo.Cells["DateBirth"].Value = dateB;
                    rowInfo.Cells["DateDeath"].Value = dateD;
                    rowInfo.Cells["Akt_Num"].Value = item.Akt_Num;
                    rowInfo.Cells["Akt_Date"].Value = item.Akt_Date.HasValue ? item.Akt_Date.Value.ToString("dd.MM.yyyy") : "";
                    DeathGridView.Rows.Add(rowInfo);
                }
            }



            this.DeathGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            foreach (var item in DeathGridView.Rows)
            {
                item.MinHeight = 22;
            }

            this.DeathGridView.TableElement.EndUpdate();
//            DeathGridView.Refresh();

            if (DeathGridView.ChildRows.Count > 0)
            {
                if (DeathGridView.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    DeathGridView.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= DeathGridView.ChildRows.Count)
                        rowindex = DeathGridView.ChildRows.Count - 1;
                    rowindex = rowindex >= 0 ? rowindex : 0;
                    DeathGridView.ChildRows[rowindex].IsCurrent = true;
                }

            }

        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            Death_Edit child = new Death_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.ShowDialog();
            child.Dispose();
            db.DetectChanges();
            db = new pu6Entities();
            DeathGrid_upd();
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (DeathGridView.ChildRows.Count > 0 && DeathGridView.CurrentRow.Cells[0].Value != null && DeathGridView.CurrentRow.Index >= 0)
            {
                long id = long.Parse(DeathGridView.CurrentRow.Cells[0].Value.ToString());

                Death_Edit child = new Death_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.formData = new ZAGS_Death { ID = id };
                child.action = "edit";
                child.ShowDialog();
                db = new pu6Entities();
                DeathGrid_upd();
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (DeathGridView.RowCount != 0 && DeathGridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную Форму РВ-3?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    long id = long.Parse(DeathGridView.CurrentRow.Cells[0].Value.ToString());

                    try
                    {
                        db.ExecuteStoreCommand(String.Format("DELETE FROM ZAGS_Death WHERE ([ID] = {0})", id));
                        db = new pu6Entities();
                        DeathGrid_upd();
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("При удалении данных произошла ошибка. Код ошибки: " + ex.Message);
                    }

                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }

            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }

        private void exportToXMLBtn_Click(object sender, EventArgs e)
        {
            if (DeathGridView.RowCount != 0 && DeathGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(DeathGridView.CurrentRow.Cells[0].Value.ToString());

                CreateXmlPack_Death child = new CreateXmlPack_Death();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.zagsID = id;
                child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для выгрузки", "");
            }
        }

    }
}
