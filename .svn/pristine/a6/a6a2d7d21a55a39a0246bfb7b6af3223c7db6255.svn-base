using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    public partial class TariffCodeFrm : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public class GridItem
        {
            public long id { get; set; }
            public string Code { get; set; }
            public string DateBegin { get; set; }
            public string DateEnd { get; set; }
            public string PlatCatList { get; set; }
        }

        public TariffCodeFrm()
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

        public static TariffCodeFrm SelfRef
        {
            get;
            set;
        }

        public void dataGrid_upd()
        {
            List<GridItem> GridList = new List<GridItem> { };

            var tar = db.TariffCode.ToList();

            foreach (var item in tar)
            {
                string platCatList = "";
                if (item.TariffCodePlatCat.Any())
                {
                    platCatList = string.Join(", ", item.TariffCodePlatCat.Select(x => x.PlatCategory.Code).ToArray());
                }
                GridList.Add(new GridItem
                {
                    Code = item.Code,
                    DateBegin = item.DateBegin.HasValue ? item.DateBegin.Value.Date.ToShortDateString() : "",
                    DateEnd = item.DateEnd.HasValue ? item.DateEnd.Value.Date.ToShortDateString() : "",
                    id = item.ID,
                    PlatCatList = platCatList
                });

            }

            BindingSource b = new BindingSource();
            b.DataSource = GridList;

            radGridView1.Rows.Clear();
            radGridView1.DataSource = null;
            radGridView1.DataSource = b;

            //  radGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            radGridView1.Columns["id"].IsVisible = false;
            radGridView1.Columns["Code"].HeaderText = "Код";
            radGridView1.Columns["Code"].Width = radGridView1.Width / 5;

            radGridView1.Columns["DateBegin"].HeaderText = "Начало";
            radGridView1.Columns["DateBegin"].Width = radGridView1.Width / 5;
            radGridView1.Columns["DateBegin"].FormatString = "{0:dd.MM.yyyy}";

            radGridView1.Columns["DateEnd"].HeaderText = "Конец";
            radGridView1.Columns["DateEnd"].Width = radGridView1.Width / 5;
            radGridView1.Columns["DateEnd"].FormatString = "{0:dd.MM.yyyy}";

            radGridView1.Columns["PlatCatList"].HeaderText = "Код категории застр. лица";
            radGridView1.Columns["PlatCatList"].Width = radGridView1.Width / 5 * 2;


            radGridView1.Refresh();

        }

        /// <summary>
        /// Добавление новой записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton1_Click(object sender, EventArgs e)
        {
            TariffCodeEdit child = new TariffCodeEdit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.action = "add";
            child.ShowDialog();
            if (child.formData != null)
            {
                TariffCode tariff = child.formData;
                db.TariffCode.AddObject(tariff);

                if (child.selectedItems != null)
                {
                    foreach (var item in child.selectedItems)
                    {
                        db.TariffCodePlatCat.AddObject(new TariffCodePlatCat { PlatCategoryID = item, TariffCodeID = tariff.ID });
                    }
                }

                db.SaveChanges();
                dataGrid_upd();
            }
        }

        /// <summary>
        /// Редактирование записи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                long id = long.Parse(radGridView1.CurrentRow.Cells[0].Value.ToString());
                TariffCode tariff = db.TariffCode.FirstOrDefault(x => x.ID == id);

                TariffCodeEdit child = new TariffCodeEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.formData = tariff;
                child.action = "edit";
                child.ShowInTaskbar = false;
                child.ShowDialog();
                if (child.formData != null)
                {
                    tariff.Code = child.formData.Code;
                    tariff.DateBegin = child.formData.DateBegin;
                    tariff.DateEnd = child.formData.DateEnd;
                    db.ObjectStateManager.ChangeObjectState(tariff, EntityState.Modified);

                    var list = db.TariffCodePlatCat.Where(x => x.TariffCodeID == tariff.ID);
                    if (child.selectedItems != null)
                    {
                        var newList = child.selectedItems;
                        var listForDel = list.Where(x => !newList.Contains(x.PlatCategoryID.Value));
                        list = list.Where(x => !listForDel.Select(y => y.ID).Contains(x.ID));
                        foreach (var item in listForDel)
                        {
                            db.TariffCodePlatCat.DeleteObject(item);
                        }

                        foreach (var item in newList)
                        {
                            if (!list.Select(x => x.PlatCategoryID).Contains(item))
                            {
                                db.TariffCodePlatCat.AddObject(new TariffCodePlatCat { PlatCategoryID = item, TariffCodeID = tariff.ID });
                            }
                        }


                    }
                    else
                    {
                        if (db.TariffCodePlatCat.Any(x => x.TariffCodeID == tariff.ID))
                        {
                            foreach (var item in list)
                                db.TariffCodePlatCat.DeleteObject(item);
                        }

                    }

                    db.SaveChanges();
                    dataGrid_upd();
                }

            }
        }

        private void TariffCodeFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            dataGrid_upd();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            int rowindex = radGridView1.CurrentRow.Index;
            long id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
            TariffCode tariff = db.TariffCode.FirstOrDefault(x => x.ID == id);

            List<long> list = tariff.TariffCodePlatCat.Select(y => y.ID).ToList();
            var b = db.TariffCodePlatCat.Where(x => list.Contains(x.ID));
            foreach (var item in b)
            {
                db.TariffCodePlatCat.DeleteObject(item);

            }
            db.TariffCode.DeleteObject(tariff);
            db.SaveChanges();

            dataGrid_upd();
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            radButton2_Click(null, null);
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
            synchResult = UpdateDictionaries.updateTable("TariffCode", path, this.ThemeName, this);
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
