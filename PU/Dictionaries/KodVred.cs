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
using System.IO;

namespace PU.FormsRSW2014
{
    public partial class KodVred : Telerik.WinControls.UI.RadForm
    {

        private pu6Entities db = new pu6Entities();
        private IEnumerable<KodVred_2> kodVredList2;
        private IEnumerable<KodVred_3> kodVredList3;
        public string action { get; set; }
        public KodVred_2 kv_osn { get; set; }
        public KodVred_3 kv_dop { get; set; }
        public byte ddl1_index = 0;

        public KodVred()
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

        public static KodVred SelfRef
        {
            get;
            set;
        }

        public void dataGrid_upd()
        {
            BindingSource b = new BindingSource();

            byte code = byte.Parse(radDropDownList1.SelectedItem.Tag.ToString());
            var temp_list = kodVredList2.Where(x => x.SpisokCode == code);

      /*      switch (radDropDownList2.SelectedItem.Value.ToString())
            { case "0": 
                    break;
              case "1": temp_list = temp_list.Where(x => x.RazdelCode == null || x.RazdelCode == "");
                    break;
              default: string s = radDropDownList2.SelectedItem.Value.ToString();
                  temp_list = temp_list.Where(x => x.RazdelCode == s);
                    break;
            }
            */
            if (!String.IsNullOrEmpty(radTextBox1.Text))
            {
                temp_list = temp_list.Where(x => x.Code.Contains(radTextBox1.Text));
            }

            if (temp_list.Count() == 0)
            {
                radGridView2.Rows.Clear();
            }
            else
            {

                b.DataSource = temp_list;

                radGridView1.DataSource = b;

                //  dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                radGridView1.Columns["ID"].IsVisible = false;
                radGridView1.Columns["SpisokCode"].IsVisible = false;
                radGridView1.Columns["StajLgot"].IsVisible = false;
                radGridView1.Columns["StajOsn"].IsVisible = false;
                radGridView1.Columns["Code"].HeaderText = "Код";
                radGridView1.Columns["Code"].Width = radGridView1.Width / 10 * 3;

                radGridView1.Columns["RazdelCode"].HeaderText = "Раздел";
                radGridView1.Columns["RazdelCode"].Width = radGridView1.Width / 10 * 2;

                radGridView1.Columns["Name"].HeaderText = "Наименование";
                radGridView1.Columns["Name"].Tag = "fweefwe";
                radGridView1.Columns["Name"].Width = radGridView1.Width / 2;
                radGridView1.Refresh();
            }


        }

        private void KodVred_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            kodVredList2 = db.KodVred_2;
            kodVredList3 = db.KodVred_3;

            BindingSource b = new BindingSource();
            b.DataSource = db.KodVred_1;

            this.radDropDownList2.DataSource = null;
            this.radDropDownList2.Items.Clear();
            this.radDropDownList2.DisplayMember = "Name";
            this.radDropDownList2.ValueMember = "Code";
            this.radDropDownList2.DataSource = b.DataSource;
            radDropDownList2.SelectedIndex = 0;

            radDropDownList1.SelectedIndex = ddl1_index;



            radDropDownList1.SelectedIndexChanged += (s, с) => dataGrid_upd();
            radDropDownList2.SelectedIndexChanged += (s, с) => dataGrid_upd();
            radTextBox1.TextChanged += (s, с) => dataGrid_upd();
            

            dataGrid_upd();
        }

        private void dataGrid3_upd()
        {
            // Доп профессии
            radGridView2.Rows.Clear();

            if (radGridView1.Rows.Count > 0)
            {
                if (radGridView1.CurrentRow != null && radGridView1.CurrentRow.Cells[2] != null && radGridView1.CurrentRow.Cells[2].Value != null)
                {
                    BindingSource b = new BindingSource();
                    string s = radGridView1.CurrentRow.Cells[2].Value.ToString();
                    b.DataSource = kodVredList3.Where(x => x.Code == s).ToList();

                    radGridView2.DataSource = b;

                    radGridView2.Columns["ID"].IsVisible = false;
                    radGridView2.Columns["SpisokCode"].IsVisible = false;
                    radGridView2.Columns["Code"].IsVisible = false;
                    radGridView2.Columns["RazdelCode"].IsVisible = false;
                    radGridView2.Columns["StajLgot"].IsVisible = false;
                    radGridView2.Columns["StajOsn"].IsVisible = false;

                    radGridView2.Columns["Name"].HeaderText = "Дополнительные профессии по коду позиции Списка";
                    radGridView2.Columns["Name"].Width = radGridView2.Width;
                    radGridView2.Refresh();
                }
            }

        }

        private void radGridView1_CurrentRowChanged(object sender, Telerik.WinControls.UI.CurrentRowChangedEventArgs e)
        {
            dataGrid3_upd();
        }

        private void radGridView1_CellFormatting(object sender, Telerik.WinControls.UI.CellFormattingEventArgs e)
        {
            if (e.CellElement.Value != null)
            {
                e.CellElement.ToolTipText = e.CellElement.Value.ToString();
            } 
        }


        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (action == "selection")
            {
                btnSelection_Click(null, null);
            }
        }

        private void btnSelection_Click(object sender, EventArgs e)
        {
            if (action == "selection" && radGridView1.CurrentRow.Index >= 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                var id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());

                kv_osn = db.KodVred_2.FirstOrDefault(x => x.ID == id);

                if (radGridView2.RowCount != 0)
                {
                    if (radGridView2.CurrentRow != null)
                    {
                        rowindex = radGridView2.CurrentRow.Index;
                        id = long.Parse(radGridView2.Rows[rowindex].Cells[0].Value.ToString());

                        kv_dop = db.KodVred_3.FirstOrDefault(x => x.ID == id);
                    }
                }

                this.btnSelection.Visible = false;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            synchResult = UpdateDictionaries.updateTable("KodVred_1", path, this.ThemeName, this);
            if (synchResult)
                synchResult = UpdateDictionaries.updateTable("KodVred_2", path, this.ThemeName, this);
            if (synchResult)
                synchResult = UpdateDictionaries.updateTable("KodVred_3", path, this.ThemeName, this);
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
