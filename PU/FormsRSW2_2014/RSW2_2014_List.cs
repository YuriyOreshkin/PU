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
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using Telerik.WinControls.UI;

namespace PU.FormsRSW2_2014
{
    public partial class RSW2_2014_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
        RSW2_2014_Print ReportViewerRSW2_2014;
        public short Year { get; set; }

        public RSW2_2014_List()
        {
            InitializeComponent();
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    addBtn.Enabled = false;
                    delBtn.Enabled = false;
                    exportToXMLBtn.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    //this.Dispose();
                    break;
            }

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RSW2_2014_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            this.Text = this.Text + "  [" + Year + "]";

            checkAccessLevel();

            HeaderChange();
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

        /// <summary>
        /// Смена заголовка при выборе страхователя
        /// </summary>
        public void HeaderChange()
        {
            insNamePanel.Text = Methods.HeaderChange();

            rsw2Grid_upd();

        }

        public void rsw2Grid_upd()
        {
            int rowindex = 0;
            string currId = "";
            if (rsw2GridView.RowCount > 0 && rsw2GridView.CurrentRow.Cells[0].Value != null)
            {
                rowindex = rsw2GridView.CurrentRow.Index;
                currId = rsw2GridView.CurrentRow.Cells[0].Value.ToString();
            }

            this.rsw2GridView.TableElement.BeginUpdate();
            rsw2GridView.Rows.Clear();

            var rsw2list = db.FormsRSW2014_2_1.Where(x => x.InsurerID == Options.InsID && x.Year >= 2014 && x.Year <= 2016);

            if (rsw2list.Count() != 0)
            {
                foreach (var item in rsw2list)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.rsw2GridView.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Year"].Value = item.Year.ToString();
                    rowInfo.Cells["CorrNum"].Value = item.CorrectionNum.ToString();
                    rowInfo.Cells["SumPFR"].Value = item.s_110_0.HasValue ? Utils.decToStr(item.s_110_0.Value) : "0,00";
                    rowInfo.Cells["SumOMS"].Value = item.s_110_3.HasValue ? Utils.decToStr(item.s_110_3.Value) : "0,00";
                    rsw2GridView.Rows.Add(rowInfo);
                }
            }

            

            this.rsw2GridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.rsw2GridView.TableElement.EndUpdate();
//            rsw2GridView.Refresh();

            if (rsw2GridView.ChildRows.Count > 0)
            {
                if (rsw2GridView.Rows.Any(x => x.Cells[0].Value.ToString() == currId))
                {
                    rsw2GridView.Rows.First(x => x.Cells[0].Value.ToString() == currId).IsCurrent = true;
                }
                else
                {
                    if (rowindex >= rsw2GridView.ChildRows.Count)
                        rowindex = rsw2GridView.ChildRows.Count - 1;
                    rowindex = rowindex >= 0 ? rowindex : 0;
                    rsw2GridView.Rows[rowindex].IsCurrent = true;
                }

            }

        }


        private void insurerBtn_Click(object sender, EventArgs e)
        {
            InsurerFrm child = new InsurerFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.InsID = Options.InsID;
            child.action = "selection";
            child.ShowDialog();
            if (Options.InsID != child.InsID)
            {
                Options.InsID = child.InsID;
                methodsNonStatic.writeSetting();
            }

            Methods.HeaderChangeAllTabs();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            if (!addBtn.Enabled)
                return;

            if (Options.InsID != 0)
            {
                RSW2_2014_Edit child = new RSW2_2014_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.RSWdata = new FormsRSW2014_2_1();
                child.YearT = Year;
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                rsw2Grid_upd();
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (rsw2GridView.RowCount != 0 && rsw2GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(rsw2GridView.CurrentRow.Cells[0].Value.ToString());

                db = new pu6Entities();
                FormsRSW2014_2_1 rsw_data = db.FormsRSW2014_2_1.FirstOrDefault(x => x.ID == id);

                RSW2_2014_Edit child = new RSW2_2014_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.RSWdata = rsw_data;
                child.YearT = rsw_data.Year;
                child.RSW_2_2_List = rsw_data.FormsRSW2014_2_2.OrderBy(x => x.ID).ToList();
                child.RSW_2_3_List = rsw_data.FormsRSW2014_2_3.OrderBy(x => x.ID).ToList();
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                rsw2Grid_upd();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования", "");
            }
        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (!delBtn.Enabled)
                return;

            if (rsw2GridView.RowCount != 0 && rsw2GridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную Форму РСВ-2?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    long id = long.Parse(rsw2GridView.CurrentRow.Cells[0].Value.ToString());

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsRSW2014_2_1 WHERE ([ID] = {0})", id));
                        db = new pu6Entities();
                        rsw2Grid_upd();
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

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (!exportToXMLBtn.Enabled)
                return;

            if (rsw2GridView.RowCount != 0 && rsw2GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(rsw2GridView.CurrentRow.Cells[0].Value.ToString());

                db = new pu6Entities();
                FormsRSW2014_2_1 rsw_data = db.FormsRSW2014_2_1.FirstOrDefault(x => x.ID == id);

                CreateXmlPack_RSW2_2014 child = new CreateXmlPack_RSW2_2014();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.RSWdata = rsw_data;
                child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для выгрузки", "");
            }
        }

        private void rsw2GridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_Click(null, null);
        }


        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReportViewerRSW2_2014.ShowDialog();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (rsw2GridView.RowCount != 0 && rsw2GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(rsw2GridView.CurrentRow.Cells[0].Value.ToString());

                FormsRSW2014_2_1 rsw_data = db.FormsRSW2014_2_1.FirstOrDefault(x => x.ID == id);

                ReportViewerRSW2_2014 = new RSW2_2014_Print();
                ReportViewerRSW2_2014.RSW2data = rsw_data;
                ReportViewerRSW2_2014.Owner = this;
                ReportViewerRSW2_2014.ThemeName = this.ThemeName;
                ReportViewerRSW2_2014.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerRSW2_2014.createReport);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                bw.RunWorkerAsync();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для печати", "");
            }
        }

        private void RSW2_2014_List_Shown(object sender, EventArgs e)
        {
            if (!Options.fixCurrentInsurer)
            {
                insurerBtn_Click(null, null);
            }
        }

        private void rsw2GridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            if ((e.ContextMenuProvider as GridDataCellElement) != null)
            {
                RadMenuItem menuItem1 = new RadMenuItem("Добавить");
                menuItem1.Click += new EventHandler(addBtn_Click);
                RadMenuItem menuItem2 = new RadMenuItem("Изменить");
                menuItem2.Click += new EventHandler(editBtn_Click);
                RadMenuItem menuItem3 = new RadMenuItem("Удалить");
                menuItem3.Click += new EventHandler(delBtn_Click);
                RadMenuItem menuItem4 = new RadMenuItem("Печать");
                menuItem4.Click += new EventHandler(printBtn_Click);
                RadMenuItem menuItem5 = new RadMenuItem("Запись в XML-файл");
                menuItem5.Click += new EventHandler(radButton1_Click);
                e.ContextMenu.Items.Insert(0, menuItem1);
                e.ContextMenu.Items.Insert(1, menuItem2);
                e.ContextMenu.Items.Insert(2, menuItem3);
                e.ContextMenu.Items.Insert(3, new RadMenuSeparatorItem());
                e.ContextMenu.Items.Insert(4, menuItem4);
                e.ContextMenu.Items.Insert(5, menuItem5);
                e.ContextMenu.Items.Insert(6, new RadMenuSeparatorItem());
                return;
            }
        }


    }
}
