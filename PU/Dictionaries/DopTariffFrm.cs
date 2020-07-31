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
using System.IO;
using PU.Classes;

namespace PU.FormsRSW2014
{
    public partial class DopTariffFrm : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public DopTariffFrm()
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

        public static DopTariffFrm SelfRef
        {
            get;
            set;
        }

        public void dataGrid_upd()
        {
            BindingSource b = new BindingSource();
            b.DataSource = db.DopTariff;

            radGridView1.DataSource = b;

            //  dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            radGridView1.Columns["ID"].IsVisible = false;
            radGridView1.Columns["Year"].HeaderText = "Год";
            radGridView1.Columns["Year"].Width = radGridView1.Width / 5;

            radGridView1.Columns["Tariff1"].HeaderText = "ч.1 ст.58.3";
            radGridView1.Columns["Tariff1"].Width = radGridView1.Width / 5 * 2;

            radGridView1.Columns["Tariff2"].HeaderText = "ч.2 ст.58.3";
            radGridView1.Columns["Tariff2"].Width = radGridView1.Width / 5 * 2;
            radGridView1.Refresh();



        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DopTariffFrmEdit child = new DopTariffFrmEdit();
            child.Owner = this;
            child.action = "add";
            child.ThemeName = this.ThemeName;
            child.radSpinEditor1.Text = DateTime.Now.Date.Year.ToString();
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        public string add(DopTariff dt)  // добавление записи
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;

            try
            {
                if (!db.DopTariff.Any(x => x.Year == dt.Year))
                {
                    db.DopTariff.Add(dt);
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

        public string edit(DopTariff dt)  // редактирование записи
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
            long ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

            try
            {
                if (!db.DopTariff.Any(x => x.Year == dt.Year && x.ID != ID))
                {
                    DopTariff dopTar = db.DopTariff.FirstOrDefault(x => x.ID == ID);

                    dopTar.Year = dt.Year.Value;
                    dopTar.Tariff1 = dt.Tariff1.Value;
                    dopTar.Tariff2 = dt.Tariff2.Value;

                    db.Entry(dopTar).State = EntityState.Modified;
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

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                DopTariffFrmEdit child = new DopTariffFrmEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.radSpinEditor1.Text = radGridView1.Rows[rowindex].Cells[1].Value.ToString();

                child.radMaskedEditBox1.Text = radGridView1.Rows[rowindex].Cells[2].Value.ToString();

                child.radMaskedEditBox2.Text = radGridView1.Rows[rowindex].Cells[3].Value.ToString();
                child.action = "edit";
                child.ShowInTaskbar = false;
                child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!", "");
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            // Удаление текущей записи
            if (radGridView1.RowCount != 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                string title = "";
                long id = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
                DialogResult dialogResult = RadMessageBox.Show(this,"Уверены что хотите удалить текущую запись?", title, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        rowindex = rowindex + 1 == radGridView1.RowCount ? rowindex - 1 : rowindex;

                        DopTariff dopT = db.DopTariff.FirstOrDefault(x => x.ID == id);
                        db.DopTariff.Remove(dopT);
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
                RadMessageBox.Show(this, "Нет данных для удаления!","");
            }
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            radButton2_Click(null, null);
        }

        private void DopTariffFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            dataGrid_upd();
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
            synchResult = UpdateDictionaries.updateTable("DopTariff", path, this.ThemeName, this);
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
