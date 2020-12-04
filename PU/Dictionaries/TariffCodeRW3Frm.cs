using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PU.Models;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls.UI;
using PU.Classes;
using System.IO;

namespace PU.Dictionaries
{
    public partial class TariffCodeRW3Frm : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        string path = "";
        bool synchResult = true;

        public TariffCodeRW3Frm()
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

        private void TariffCodeRW3Frm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            dataGrid_upd();
        }

        public void dataGrid_upd()
        {
            radGridView1.Rows.Clear();
            var tarifflist = db.CodeBaseRW3_2015.ToList();

            if (tarifflist.Count() != 0)
            {
                foreach (var item in tarifflist)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView1.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Year"].Value = item.Year;
                    rowInfo.Cells["Tar21"].Value = Utils.decToStr(item.Tar21);
                    rowInfo.Cells["Tar22"].Value = Utils.decToStr(item.Tar22);
                    radGridView1.Rows.Add(rowInfo);
                }
            }

            for (var i = 0; i < radGridView1.Columns.Count; i++)
            {
                radGridView1.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                radGridView1.Columns[i].ReadOnly = true;
            }

            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            TariffCodeRW3Edit child = new TariffCodeRW3Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.action = "add";
            child.ShowDialog();
            if (child.formData != null)
            {
                try
                {
                    if (!db.CodeBaseRW3_2015.Any(x => x.Year == child.formData.Year))
                    {
                        db.CodeBaseRW3_2015.Add(child.formData);
                        db.SaveChanges();
                        dataGrid_upd();
                    }
                    else
                    {
                        RadMessageBox.Show(this, "Дублирование записей по году!", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
                catch
                {
                    RadMessageBox.Show(this, "Ошибка при сохранении данных", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Error);
                }
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount > 0)
            {
                long id = 0;

                long.TryParse(radGridView1.CurrentRow.Cells[0].Value.ToString(), out id);
                var tempData = db.CodeBaseRW3_2015.First(x => x.ID == id);

                TariffCodeRW3Edit child = new TariffCodeRW3Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.action = "edit";
                child.formData = tempData;
                child.ShowDialog();
                if (child.formData != null)
                {
                    try
                    {
                        if (!db.CodeBaseRW3_2015.Any(x => x.Year == child.formData.Year && x.ID != child.formData.ID))
                        {
                            tempData.Year = child.formData.Year;
                            tempData.Tar21 = child.formData.Tar21;
                            tempData.Tar22 = child.formData.Tar22;

                            db.Entry(tempData).State = EntityState.Modified;
                            db.SaveChanges();
                            dataGrid_upd();
                        }
                        else
                        {
                            RadMessageBox.Show(this, "Дублирование записей по году!", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Error);
                        }
                    }
                    catch
                    {
                        RadMessageBox.Show(this, "Ошибка при сохранении данных", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования", "Внимание");
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount > 0)
            {
                long id = 0;

                long.TryParse(radGridView1.CurrentRow.Cells[0].Value.ToString(), out id);
                var tempData = db.CodeBaseRW3_2015.First(x => x.ID == id);

                try
                {
                    db.CodeBaseRW3_2015.Remove(tempData);
                    db.SaveChanges();
                    dataGrid_upd();
                }
                catch
                {
                    RadMessageBox.Show(this, "Ошибка при удалении данных", "Внимание", MessageBoxButtons.OK, RadMessageIcon.Error);

                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления", "Внимание");
            }
        }

        public void synchBtn_Click(object sender, EventArgs e)
        {
            path = Path.Combine(Application.StartupPath, "Base_emp\\pu6_emp.db3");
            if (File.Exists(path))
            {
                this.Cursor = Cursors.WaitCursor;

                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
                bw.DoWork += new System.ComponentModel.DoWorkEventHandler(synchronization);
                bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

                bw.RunWorkerAsync();

            }
            else
            {
                RadMessageBox.Show(this, "Не найдена эталонная база данных! Синхронизация остановлена!", "Ошибка синхронизации");
            }
        }

        private void synchronization(object sender, DoWorkEventArgs e)
        {
            synchResult = UpdateDictionaries.updateTable("CodeBaseRW3_2015", path, this.ThemeName, this);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;
            dataGrid_upd();
            if (synchResult)
            {
                Messenger.showAlert(AlertType.Success, "Синхронизация завершена", "Данные успешно синхронизированы!", this.ThemeName);
            }
            else
            {
                Messenger.showAlert(AlertType.Error, "Синхронизация завершена", "Во время синхронизации произошла ошибка!", this.ThemeName);
            }
        }

    }
}
