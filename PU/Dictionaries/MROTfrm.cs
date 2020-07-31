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

namespace PU.FormsRSW2014
{
    public partial class MROTFrm : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public MROTFrm()
        {
            InitializeComponent();
            SelfRef = this;
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

        public static MROTFrm SelfRef
        {
            get;
            set;
        }

        public void dataGrid_upd()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            radGridView1.Rows.Clear();
            var mrot = db.MROT.ToList();


            if (mrot.Count() != 0)
            {
                foreach (var item in mrot)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView1.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Year"].Value = item.Year;
                    rowInfo.Cells["NalogBase"].Value = item.NalogBase != 0 ? Math.Round(item.NalogBase, 2).ToString() : "0.00";
                    rowInfo.Cells["Mrot1"].Value = item.Mrot1 != 0 ? Math.Round(item.Mrot1, 2).ToString() : "0.00";
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

        private void radButton1_Click(object sender, EventArgs e)
        {
            MROTEdit child = new MROTEdit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.action = "add";
            child.radSpinEditor1.Value = DateTime.Now.Year;
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
//                int rowindex = radGridView1.CurrentRow.Index;
                MROTEdit child = new MROTEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.radSpinEditor1.Text = radGridView1.CurrentRow.Cells[1].Value.ToString();

                child.radMaskedEditBox1.Text = radGridView1.CurrentRow.Cells[2].Value.ToString();

                child.radMaskedEditBox2.Text = radGridView1.CurrentRow.Cells[3].Value.ToString();
                child.action = "edit";
                child.ShowInTaskbar = false;
                child.ShowDialog();
            }
            else
                RadMessageBox.Show(this,"Нет данных для редактирования!");
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                string title = "";
                long id = Convert.ToInt64(radGridView1.CurrentRow.Cells[0].Value.ToString());
                DialogResult dialogResult = RadMessageBox.Show(this,"Уверены что хотите удалить текущую запись?", title, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        rowindex = rowindex + 1 == radGridView1.RowCount ? rowindex - 1 : rowindex;

                        MROT mrot = db.MROT.FirstOrDefault(x => x.ID == id);
                        db.MROT.Remove(mrot);
                        db.SaveChanges();
                        dataGrid_upd();

                        if (radGridView1.RowCount != 0)
                        {
                            radGridView1.Rows[rowindex].IsCurrent = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show(this,"При удалении данных произошла ошибка! " + ex, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);

                    }

                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }

            }
            else
                RadMessageBox.Show(this,"Нет данных для удаления!");
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MROTFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            dataGrid_upd();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            radButton2_Click(null, null);
        }

        public string add(MROT mrot)  // добавление записи
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;

            try
            {
                if (!db.MROT.Any(x => x.Year == mrot.Year))
                {
                    db.MROT.Add(mrot);
                    db.SaveChanges();
                    dataGrid_upd();
                    radGridView1.Rows[rowindex].IsCurrent = true;
                }
                else
                {
                    result = "Запись за указанный год уже есть в БД";
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        public string edit(MROT mrot)  // редактирование записи
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
            long ID = Convert.ToInt64(radGridView1.CurrentRow.Cells[0].Value);

            try
            {
                if (!db.MROT.Any(x => x.Year == mrot.Year && x.ID != ID))
                {
                    MROT mr = db.MROT.FirstOrDefault(x => x.ID == ID);

                    mr.Year = mrot.Year;
                    mr.NalogBase = mrot.NalogBase;
                    mr.Mrot1 = mrot.Mrot1;

                    db.Entry(mr).State = EntityState.Modified;
                    db.SaveChanges();
                    dataGrid_upd();
                    radGridView1.Rows[rowindex].IsCurrent = true;
                }
                else
                {
                    result = "Данные за указанный год уже есть в БД";
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }


        string path = "";
        bool synchResult = true;

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
            synchResult = UpdateDictionaries.updateTable("MROT", path, this.ThemeName, this);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;
            dataGrid_upd();
            if (synchResult)
            {
                Methods.showAlert("Синхронизация завершена", "Данные успешно синхронизированы!", this.ThemeName);
            }
            else
            {
                Methods.showAlert("Синхронизация завершена", "Во время синхронизации произошла ошибка!", this.ThemeName);
            }
        }
    }
}
