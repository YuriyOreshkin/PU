using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Linq;
using PU.Models;
using System.IO;
using PU.Classes;

namespace PU.FormsRSW2014
{
    public partial class DocTypes : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public DocumentTypes DocType { get; set; }
        public string action { get; set; }

        public DocTypes()
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


        public static DocTypes SelfRef
        {
            get;
            set;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            DocTypesEdit child = new DocTypesEdit();
            child.Owner = this;
            child.action = "add";
            child.ShowInTaskbar = false;
            child.ThemeName = this.ThemeName;
            child.ShowDialog();
        }

        private void DocTypes_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            dataGrid_upd();

            if (action == "selection")
            {
                radButton6.Visible = true;
                if (DocType != null)
                    radGridView1.Rows.First(x => x.Cells[0].Value.ToString() == DocType.ID.ToString()).IsCurrent = true;
            }
        }

        public void dataGrid_upd()
        {
            BindingSource b = new BindingSource();
            b.DataSource = db.DocumentTypes;

            radGridView1.DataSource = b;

            //  dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            radGridView1.Columns["ID"].IsVisible = false;
            radGridView1.Columns["StaffInfo"].IsVisible = false;
            radGridView1.Columns["Code"].HeaderText = "Код";
            radGridView1.Columns["Code"].Width = radGridView1.Width / 5;

            radGridView1.Columns["Name"].HeaderText = "Наименование";
            radGridView1.Columns["Name"].Width = radGridView1.Width - radGridView1.Columns["Code"].Width - 3;
            radGridView1.Refresh();

        }

        private void radGridView1_Click(object sender, EventArgs e)
        {

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;

                DocTypesEdit child = new DocTypesEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.radTextBox1.Text = radGridView1.Rows[rowindex].Cells[1].Value.ToString();
                child.radTextBox2.Text = radGridView1.Rows[rowindex].Cells[2].Value.ToString();
                child.action = "edit";

                //= Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[0].Value);
                child.ShowInTaskbar = false;

                child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!", "Ошибка");
            }
        }

        public string add(DocumentTypes dt, string action)
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;

            try
            {

                switch (action)
                {
                    case "add":
                        if (!db.DocumentTypes.Any(x => x.Code == dt.Code && x.Name == dt.Name))
                        {
                            db.DocumentTypes.Add(dt);
                            db.SaveChanges();
                            dataGrid_upd();
                            radGridView1.Rows[rowindex].IsCurrent = true;
                        }
                        else
                        {
                            result = "Запись с указанными параметрами уже есть в БД";
                        }
                        break;

                    case "edit":

                        long ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

                        if (!db.DocumentTypes.Any(x => x.Code == dt.Code && x.Name == dt.Name && x.ID != ID))
                        {
                            DocumentTypes DocT = db.DocumentTypes.FirstOrDefault(x => x.ID == ID);

                            DocT.Name = dt.Name;
                            DocT.Code = dt.Code;

                            db.Entry(DocT).State = EntityState.Modified;
                            db.SaveChanges();
                            dataGrid_upd();
                            radGridView1.Rows[rowindex].IsCurrent = true;
                        }
                        else
                        {
                            result = "Запись с указанными параметрами уже есть в БД";
                        }

                        break;
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                string title = radGridView1.Rows[rowindex].Cells[2].Value.ToString();
                long id = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
                DialogResult dialogResult = RadMessageBox.Show("Уверены что хотите удалить текущую запись?", title, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        rowindex = rowindex + 1 == radGridView1.RowCount ? rowindex - 1 : rowindex;

                        DocumentTypes DocT = db.DocumentTypes.FirstOrDefault(x => x.ID == id);
                        db.DocumentTypes.Remove(DocT);
                        db.SaveChanges();
                        dataGrid_upd();

                        if (radGridView1.RowCount != 0)
                        {
                            radGridView1.Rows[rowindex].IsCurrent = true;
                        }

                    }
                    catch (Exception ex)
                    {
                        Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
                        DialogResult result = RadMessageBox.Show(this, "При удалении данных произошла ошибка! " + ex, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);


                    }

                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
            else
            {
                Telerik.WinControls.RadMessageBox.SetThemeName(radGridView1.ThemeName);
                RadMessageBox.Show(this, "Нет данных для удаления!", "Ошибка"); 
            }
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            switch (action)
            {
                case "selection":
                    radButton6_Click(null, null);
                    break;
                default:
                    radButton2_Click(null, null);
                    break;
            }
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            if (action == "selection")
            {
                int rowindex = radGridView1.CurrentRow.Index;
                long id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
                DocType = db.DocumentTypes.FirstOrDefault(x => x.ID == id);
                this.Close();
            }
        }

        string path = "";
        bool synchResult = true;

        private void synchBtn_Click(object sender, EventArgs e)
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
            synchResult = UpdateDictionaries.updateTable("DocumentTypes", path, this.ThemeName, this);
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Default;
            db = new pu6Entities();
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
