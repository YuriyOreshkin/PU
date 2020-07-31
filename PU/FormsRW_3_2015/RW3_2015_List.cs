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

namespace PU.FormsRW_3_2015
{
    public partial class RW3_2015_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
        RW3_2015_Print ReportViewerRW3_2015;

        public RW3_2015_List()
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

            rw3Grid_upd();

        }

        public void rw3Grid_upd()
        {
            this.rw3GridView.TableElement.BeginUpdate();
            rw3GridView.Rows.Clear();

            var rw3list = db.FormsRW3_2015.Where(x => x.InsurerID == Options.InsID && x.Year >= 2014);

            if (rw3list.Count() != 0)
            {
                foreach (var item in rw3list)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.rw3GridView.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Year"].Value = item.Year.ToString();
                    rowInfo.Cells["Period"].Value = Options.RaschetPeriodInternal.Any(x => x.Year == item.Year && x.Kvartal == item.Quarter) ? (item.Quarter.ToString() + " - " + Options.RaschetPeriodInternal.FirstOrDefault(x => x.Year == item.Year && x.Kvartal == item.Quarter).Name) : "Период не определен";
                    rowInfo.Cells["CorrNum"].Value = item.CorrectionNum.ToString();
                    rowInfo.Cells["CodeTar"].Value = item.CodeTar.ToString();
                    rowInfo.Cells["Sum"].Value = item.s_130_0.HasValue ? Utils.decToStr(item.s_130_0.Value) : "0,00";
                    rw3GridView.Rows.Add(rowInfo);
                }
            }

            

            this.rw3GridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            this.rw3GridView.TableElement.EndUpdate();
            rw3GridView.Refresh();

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
                RW3_2015_Edit child = new RW3_2015_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "add";
                child.RWdata = new FormsRW3_2015();
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                rw3Grid_upd();
            }
        }

        private void editBtn_Click(object sender, EventArgs e)
        {
            if (rw3GridView.RowCount != 0 && rw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(rw3GridView.CurrentRow.Cells[0].Value.ToString());

                db = new pu6Entities();
                FormsRW3_2015 rw_data = db.FormsRW3_2015.FirstOrDefault(x => x.ID == id);

                RW3_2015_Edit child = new RW3_2015_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.action = "edit";
                child.RWdata = rw_data;
                child.RW_3_3_List = rw_data.FormsRW3_2015_Razd_3.OrderBy(x => x.ID).ToList();
                child.ShowDialog();
                child.Dispose();
                db.ChangeTracker.DetectChanges();
                db = new pu6Entities();
                rw3Grid_upd();
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

            if (rw3GridView.RowCount != 0 && rw3GridView.CurrentRow.Cells[0].Value != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить выбранную Форму РВ-3?", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    long id = long.Parse(rw3GridView.CurrentRow.Cells[0].Value.ToString());

                    try
                    {
                        db.Database.ExecuteSqlCommand(String.Format("DELETE FROM FormsRW3_2015 WHERE ([ID] = {0})", id));
                        db = new pu6Entities();
                        rw3Grid_upd();
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

            if (rw3GridView.RowCount != 0 && rw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(rw3GridView.CurrentRow.Cells[0].Value.ToString());

                db = new pu6Entities();
                FormsRW3_2015 rw3_data = db.FormsRW3_2015.FirstOrDefault(x => x.ID == id);

                CreateXmlPack_RW3_2015 child = new CreateXmlPack_RW3_2015();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.RWdata = rw3_data;
                child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для выгрузки", "");
            }
        }

        private void rw3GridView_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            editBtn_Click(null, null);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ReportViewerRW3_2015.ShowDialog();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            if (rw3GridView.RowCount != 0 && rw3GridView.CurrentRow.Cells[0].Value != null)
            {
                long id = long.Parse(rw3GridView.CurrentRow.Cells[0].Value.ToString());

                FormsRW3_2015 rw_data = db.FormsRW3_2015.FirstOrDefault(x => x.ID == id);

                ReportViewerRW3_2015 = new RW3_2015_Print();
                ReportViewerRW3_2015.RW3data = rw_data;
                ReportViewerRW3_2015.Owner = this;
                ReportViewerRW3_2015.ThemeName = this.ThemeName;
                ReportViewerRW3_2015.ShowInTaskbar = false;

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerRW3_2015.createReport);
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

        private void rw3GridView_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
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
