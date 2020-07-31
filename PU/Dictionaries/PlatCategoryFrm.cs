using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using PU.Models;
using PU.Classes;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.IO;

namespace PU.FormsRSW2014
{
    public partial class PlatCategoryFrm : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public PlatCategory PlatCat { get; set; }
        public string action { get; set; }
        private bool copyFlag = false;

        public PlatCategoryFrm()
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

        public static PlatCategoryFrm SelfRef
        {
            get;
            set;
        }


        public void dataGrid_upd()
        {
            if (radDropDownList1.Items.Count() > 0)
            {
                RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
                BindingSource b = new BindingSource();
                long id = long.Parse(radDropDownList1.SelectedItem.Value.ToString());
                var list = db.PlatCategory.Where(x => x.PlatCategoryRaschPerID == id).ToList();

                //radGridView1.DataSource = b;

                //  radGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                this.radGridView1.TableElement.BeginUpdate();
                radGridView1.Rows.Clear();
                foreach (var item in list)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView1.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    rowInfo.Cells["Code"].Value = item.Code;
                    rowInfo.Cells["Name"].Value = item.Name;
                    rowInfo.Cells["DateBegin"].Value = item.DateBegin.HasValue ? item.DateBegin.Value.ToShortDateString() : "";
                    rowInfo.Cells["DateEnd"].Value = item.DateEnd.HasValue ? item.DateEnd.Value.ToShortDateString() : "";
                    radGridView1.Rows.Add(rowInfo);
                }

                for (var i = 0; i < radGridView1.Columns.Count; i++)
                {
                    radGridView1.Columns[i].ReadOnly = true;
                }

                this.radGridView1.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                this.radGridView1.TableElement.EndUpdate();

            }
        }




        private void radDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGrid_upd();
        }


        public string add(PlatCategory pc)  // добавление записи
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
            long ID = Convert.ToInt64(radGridView1.CurrentRow.Cells[0].Value);
            PlatCategory temp = db.PlatCategory.FirstOrDefault(x => x.ID == ID); 

            try
            {
                if (!db.PlatCategory.Any(x => x.Code == pc.Code && x.PlatCategoryRaschPerID == temp.PlatCategoryRaschPerID))
                {
                    PlatCategory newItem = new PlatCategory()
                    {
                        Name = pc.Name,
                        Code = pc.Code,
                        FullName = pc.FullName,
                        PlatCategoryRaschPerID = radDropDownList1.SelectedIndex
                    };
                    if (pc.DateBegin != null)
                    {
                        newItem.DateBegin = pc.DateBegin.Value.Date;
                    }
                    if (pc.DateEnd != null)
                    {
                        newItem.DateEnd = pc.DateEnd.Value.Date;
                    }
                    db.PlatCategory.AddObject(newItem);

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

        public string edit(PlatCategory pc)  // редактирование записи
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
            long ID = Convert.ToInt64(radGridView1.CurrentRow.Cells[0].Value);
            PlatCategory temp = db.PlatCategory.FirstOrDefault(x => x.ID == ID); 
            try
            {
                if (!db.PlatCategory.Any(x => x.Code == pc.Code && x.PlatCategoryRaschPerID == temp.PlatCategoryRaschPerID && x.ID != ID))
                {
                    PlatCategory Item = db.PlatCategory.FirstOrDefault(x => x.ID == ID);

                    Item.Code = pc.Code;
                    Item.Name = pc.Name;
                    Item.FullName = pc.FullName;

                    if (pc.DateBegin != null)
                    {
                        Item.DateBegin = pc.DateBegin.Value.Date;
                    }
                    else
                        Item.DateBegin = null;
                    if (pc.DateEnd != null)
                    {
                        Item.DateEnd = pc.DateEnd.Value.Date;
                    }
                    else
                        Item.DateEnd = null;

                    db.ObjectStateManager.ChangeObjectState(Item, EntityState.Modified);
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


        private void radDropDownList1_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            dataGrid_upd();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            PlatCategoryFrmEdit child = new PlatCategoryFrmEdit();
            child.Owner = this;
            child.action = "add";
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                long id = Convert.ToInt64(radGridView1.CurrentRow.Cells[0].Value);
                PlatCategoryFrmEdit child = new PlatCategoryFrmEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.formData = db.PlatCategory.FirstOrDefault(x => x.ID == id);
                child.action = "edit";
                child.ShowInTaskbar = false;
                child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!", "Ошибка");
            }
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
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

        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PlatCategoryFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            copyPanel.Location = new Point((this.Width + 10), copyPanel.Location.Y);
            copyPanel.Visible = false;
            copyFromSpin.Value = (decimal)DateTime.Now.Year - 1;
            copyToSpin.Value = (decimal)DateTime.Now.Year;

            BindingSource b = new BindingSource();
            b.DataSource = db.PlatCategoryRaschPer;

            this.radDropDownList1.DataSource = null;
            this.radDropDownList1.Items.Clear();
            this.radDropDownList1.DisplayMember = "Name";
            this.radDropDownList1.ValueMember = "ID";
            this.radDropDownList1.DataSource = b.DataSource;
            if (PlatCat != null)
            {
                radDropDownList1.Items.FirstOrDefault(x => x.Value.ToString() == PlatCat.PlatCategoryRaschPerID.ToString()).Selected = true;
            }
            else
                radDropDownList1.SelectedIndex = radDropDownList1.Items.Count - 1;

            dataGrid_upd();

            if (action == "selection")
            {
                radButton6.Visible = true;
                if (PlatCat != null)
                    radGridView1.Rows.First(x => x.Cells[0].Value.ToString() == PlatCat.ID.ToString()).IsCurrent = true;
            }
            else
            {
                if (radGridView1.RowCount > 0)
                {
                    radGridView1.Rows[0].IsCurrent = true;
                    radGridView1.TableElement.ScrollToRow(radGridView1.Rows[0]);
                }
            }

            this.radGridView1.Select();

        }

        private void radButton5_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0 && radGridView1.CurrentRow.Cells[1].Value != null)
            {
            long ID = Convert.ToInt64(radGridView1.CurrentRow.Cells[0].Value);

            TariffPlatFrm child = new TariffPlatFrm();
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.Owner = this;
            child.PlatCat = db.PlatCategory.FirstOrDefault(x => x.ID == ID);
            child.Text = child.Text + "  [" + radGridView1.CurrentRow.Cells[1].Value.ToString().ToUpper() + "] " + radGridView1.CurrentRow.Cells[2].Value.ToString().ToUpper();
            child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Не выбрана категория плательщика", "");
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            // Удаление текущей записи
            if (radGridView1.RowCount != 0 && radGridView1.CurrentRow.Cells[1].Value != null)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                string title = "";
                long id = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
                DialogResult dialogResult = RadMessageBox.Show(this, "Уверены что хотите удалить текущую запись?", title, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        rowindex = rowindex + 1 == radGridView1.RowCount ? rowindex - 1 : rowindex;

                        PlatCategory PlatCat = db.PlatCategory.FirstOrDefault(x => x.ID == id);
                        if (db.TariffPlat.Any(x => x.PlatCategoryID == id))
                        {
                            var TarPlats = db.TariffPlat.Where(x => x.PlatCategoryID == id);
                            try
                            {
                                foreach (var item in TarPlats)
                                {
                                    db.TariffPlat.DeleteObject(item);
                                }
                            }
                            finally
                            {
                                db.SaveChanges();

                                db.PlatCategory.DeleteObject(PlatCat);
                                db.SaveChanges();
                                dataGrid_upd();

                                if (radGridView1.RowCount != 0)
                                {
                                    radGridView1.Rows[rowindex].IsCurrent = true;

                                }

                            }

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
                RadMessageBox.Show(this, "Нет данных для удаления!", "");
            }
        }

        private void radButton6_Click(object sender, EventArgs e)
        {
            if (action == "selection")
            {
                long id = Convert.ToInt64(radGridView1.CurrentRow.Cells[0].Value);
                PlatCat = db.PlatCategory.FirstOrDefault(x => x.ID == id);
                this.Close();
            }
        }

        private void radButton7_Click(object sender, EventArgs e)
        {
            int rowindex = radGridView1.CurrentRow.Index;
            long id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
            RadMessageBox.Show(radGridView1.CurrentRow.Index.ToString() + "  -  " + radGridView1.Rows[rowindex].Cells[1].Value.ToString());

        }

        private void radGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
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
            else if (e.KeyChar == 8)
            {
                if (this.radGridView1.MasterView.TableFilteringRow.Cells["Code"].Value != null && !String.IsNullOrEmpty(this.radGridView1.MasterView.TableFilteringRow.Cells["Code"].Value.ToString()))
                {
                    this.radGridView1.MasterView.TableFilteringRow.Cells["Code"].Value = this.radGridView1.MasterView.TableFilteringRow.Cells["Code"].Value.ToString().Remove(this.radGridView1.MasterView.TableFilteringRow.Cells["Code"].Value.ToString().Length - 1);
                }
            }
            else
            {
                this.radGridView1.MasterView.TableFilteringRow.Cells["Code"].Value = (this.radGridView1.MasterView.TableFilteringRow.Cells["Code"].Value != null ? this.radGridView1.MasterView.TableFilteringRow.Cells["Code"].Value.ToString() : "") + e.KeyChar;
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            copyFlag = !copyFlag;
            copyChangeState();
        }

        private void copyChangeState()
        {
            if (copyFlag)
            {
                copyPanel.Visible = copyFlag;

                for (int i = (this.Width + 10); i >= (this.Width - 220); i = i - 7)
                {
                    System.Threading.Thread.Sleep(1);
                    copyPanel.Location = new Point(i, copyPanel.Location.Y);
                    Refresh();
                }
            }
            else
            {
                for (int i = (this.Width - 220); i <= (this.Width + 10); i = i + 7)
                {
                    System.Threading.Thread.Sleep(1);
                    copyPanel.Location = new Point(i, copyPanel.Location.Y);
                    Refresh();
                }
                copyPanel.Visible = copyFlag;
            }
        }

        private void copyTariffBtn_Click(object sender, EventArgs e)
        {
            long id = long.Parse(radDropDownList1.SelectedItem.Value.ToString());
            var list = db.PlatCategory.Where(x => x.PlatCategoryRaschPerID == id);
            int cnt_good = 0;
            int cnt_bad = 0;

            foreach (var item in list)
            {
                if (item.TariffPlat.Any(x => x.Year == (short)copyFromSpin.Value))
                {
                    TariffPlat tp = item.TariffPlat.FirstOrDefault(x => x.Year == (short)copyFromSpin.Value);

                    // если Год назначения уже есть
                    if (item.TariffPlat.Any(x => x.Year == (short)copyToSpin.Value))
                    {
                        if (copyRuleToggle.IsOn)
                        {
                            TariffPlat tp_edit = item.TariffPlat.FirstOrDefault(x => x.Year == (short)copyToSpin.Value);

                            tp_edit.Year = (short)copyToSpin.Value;
                            tp_edit.FFOMS_Percent = tp.FFOMS_Percent;
                            tp_edit.NakopPercant = tp.NakopPercant;
                            tp_edit.PlatCategoryID = tp.PlatCategoryID;
                            tp_edit.StrahPercant1966 = tp.StrahPercant1966;
                            tp_edit.StrahPercent1967 = tp.StrahPercent1967;
                            tp_edit.TFOMS_Percent = tp.TFOMS_Percent;

                            db.ObjectStateManager.ChangeObjectState(tp_edit, EntityState.Modified);
                            cnt_good++;
                            
                        }

                    }
                    else
                    {
                        TariffPlat tp_new = new TariffPlat
                        {
                            Year = (short)copyToSpin.Value,
                            FFOMS_Percent = tp.FFOMS_Percent,
                            NakopPercant = tp.NakopPercant,
                            PlatCategoryID = tp.PlatCategoryID,
                            StrahPercant1966 = tp.StrahPercant1966,
                            StrahPercent1967 = tp.StrahPercent1967,
                            TFOMS_Percent = tp.TFOMS_Percent
                        };

                        db.TariffPlat.AddObject(tp_new);
                        cnt_good++;
                    }
                }
                else
                {
                    cnt_bad++;
                }
            }
            try
            {
                db.SaveChanges();
                RadMessageBox.Show(this, "Успешно скопировано " + cnt_good.ToString() + " записей!", "Выполнено");
                linkLabel1_LinkClicked(null, null);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(this, "Во время копирования произошла ошибка! " + ex.Message, "Ошибка!");
            }

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
            synchResult = UpdateDictionaries.updateTable("PlatCategory", path, this.ThemeName, this);
            synchResult = UpdateDictionaries.updateTable("TariffPlat", path, this.ThemeName, this);
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

        private void radGridView1_FilterChanged(object sender, GridViewCollectionChangedEventArgs e)
        {
            if ((e.GridViewTemplate.MasterTemplate.CurrentRow == null || e.GridViewTemplate.MasterTemplate.CurrentRow.Index < 0) && e.GridViewTemplate.ChildRows.Count > 0 && !radGridView1.MasterView.TableFilteringRow.IsCurrent)
            {
                e.GridViewTemplate.ChildRows.First().IsCurrent = true;
            }
        }




    }
}
