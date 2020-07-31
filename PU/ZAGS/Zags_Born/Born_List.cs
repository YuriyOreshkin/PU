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

namespace PU.ZAGS.Zags_Born
{
    public partial class Born_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public Born_List()
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

        private void Born_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            bornGrid_upd();
        }

        public void bornGrid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (bornGridView.ChildRows.Count > 0 && bornGridView.CurrentRow.Cells[0].Value != null && bornGridView.CurrentRow.Index >= 0)
            {
                rowindex = bornGridView.CurrentRow.Index;
                currId = bornGridView.CurrentRow.Cells[0].Value.ToString();
            }

            this.bornGridView.TableElement.BeginUpdate();
            bornGridView.Rows.Clear();

            var list = db.ZAGS_Born;

            if (list.Count() != 0)
            {
                foreach (var item in list)
                {
                    string dateB = "";

                    if (item.cType_DateBirth.HasValue)
                        if (item.cType_DateBirth.Value == 1)
                        {
                            dateB = item.cDateBirthDay_Os.HasValue ? item.cDateBirthDay_Os.Value.ToString().PadLeft(2, '0') : "";
                            dateB += item.cDateBirthMonth_Os.HasValue ? ("." + item.cDateBirthMonth_Os.Value.ToString().PadLeft(2, '0')) : "";
                            dateB += item.cDateBirthYear_Os.HasValue ? ("." + item.cDateBirthYear_Os.Value.ToString()) : "";

                        }
                        else
                            dateB = item.cDateBirth.HasValue ? item.cDateBirth.Value.ToString("dd.MM.yyyy") : "";

                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.bornGridView.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["cFIO"].Value = item.cLastName.Trim() + " " + item.cFirstName.Trim() + " " + item.cMiddleName.Trim();
                    rowInfo.Cells["cSex"].Value = item.cSex.HasValue ? (item.cSex.Value == 0 ? "М" : "Ж") : "";
                    rowInfo.Cells["cDateBirth"].Value = dateB;
                    rowInfo.Cells["cAkt_Num"].Value = item.cAkt_Num;
                    rowInfo.Cells["cAkt_Date"].Value = item.cAkt_Date.HasValue ? item.cAkt_Date.Value.ToString("dd.MM.yyyy") : "";
                    rowInfo.Cells["cSvid_Ser_Num"].Value = item.cSvid_Ser + "-" + item.cSvid_Num;
                    rowInfo.Cells["cSvid_Date"].Value = item.cSvid_Date.HasValue ? item.cSvid_Date.Value.ToString("dd.MM.yyyy") : "";
                    bornGridView.Rows.Add(rowInfo);
                }
            }



            this.bornGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            foreach (var item in bornGridView.Rows)
            {
                item.MinHeight = 22;
            }

            
            this.bornGridView.TableElement.EndUpdate();
//            bornGridView.Refresh();

            if (bornGridView.ChildRows.Count > 0)
            {
                if (bornGridView.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    bornGridView.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= bornGridView.ChildRows.Count)
                        rowindex = bornGridView.ChildRows.Count - 1;
                    rowindex = rowindex >= 0 ? rowindex : 0;
                    bornGridView.ChildRows[rowindex].IsCurrent = true;
                }

            }

        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            Born_Edit child = new Born_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.ShowDialog();
            child.Dispose();
            db.ChangeTracker.DetectChanges();
            db = new pu6Entities();
            bornGrid_upd();
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (bornGridView.ChildRows.Count > 0 && bornGridView.CurrentRow.Cells[0].Value != null && bornGridView.CurrentRow.Index >= 0)
            {
                long id = long.Parse(bornGridView.CurrentRow.Cells[0].Value.ToString());

                Born_Edit child = new Born_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.formData = new ZAGS_Born { ID = id };
                child.action = "edit";
                child.ShowDialog();
                db = new pu6Entities();
                bornGrid_upd();
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (bornGridView.RowCount != 0 && bornGridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную Форму РВ-3?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    long id = long.Parse(bornGridView.CurrentRow.Cells[0].Value.ToString());

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM ZAGS_Born WHERE ([ID] = {0})", id));
                        db = new pu6Entities();
                        bornGrid_upd();
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
            if (bornGridView.RowCount != 0 && bornGridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(bornGridView.CurrentRow.Cells[0].Value.ToString());

                CreateXmlPack_Born child = new CreateXmlPack_Born();
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
