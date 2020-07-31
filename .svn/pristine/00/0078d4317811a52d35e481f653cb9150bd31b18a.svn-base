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

namespace PU.FormsRSW2014
{
    public partial class RaschetPeriodFrm : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public RaschetPeriodFrm()
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

        public static RaschetPeriodFrm SelfRef
        {
            get;
            set;
        }

        public void dataGrid_upd()
        {
            BindingSource b = new BindingSource();
            b.DataSource = db.RaschetPeriod.OrderBy(x => x.Year);

            radGridView1.DataSource = b;

            //  radGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            radGridView1.Columns["ID"].IsVisible = false;
            radGridView1.Columns["Year"].HeaderText = "Год";
            radGridView1.Columns["Year"].Width = radGridView1.Width / 5;

            radGridView1.Columns["Kvartal"].HeaderText = "Квартал";
            radGridView1.Columns["Kvartal"].Width = radGridView1.Width / 5;

            radGridView1.Columns["Name"].HeaderText = "Обозначение";
            radGridView1.Columns["Name"].Width = radGridView1.Width / 5;

            radGridView1.Columns["DateBegin"].HeaderText = "Начало";
            radGridView1.Columns["DateBegin"].Width = radGridView1.Width / 5;
            radGridView1.Columns["DateBegin"].FormatString = "{0:dd.MM.yyyy}";

            radGridView1.Columns["DateEnd"].HeaderText = "Конец";
            radGridView1.Columns["DateEnd"].Width = radGridView1.Width / 5;
            radGridView1.Columns["DateEnd"].FormatString = "{0:dd.MM.yyyy}";
            radGridView1.Refresh();

        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            RaschetPeriodFrmEdit child = new RaschetPeriodFrmEdit();
            child.Owner = this;
            child.action = "add";
            child.ThemeName = this.ThemeName;
            child.radSpinEditor1.Value = DateTime.Now.Year;
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        public string add(RaschetPeriod rp)  // добавление записи
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;

            try
            {
                if (!db.RaschetPeriod.Any(x => x.Year == rp.Year && x.Kvartal == rp.Kvartal))
                {
                    RaschetPeriod newItem = new RaschetPeriod()
                    {
                        Year = rp.Year,
                        Name = rp.Name,
                        Kvartal = rp.Kvartal
                    };
                    if (rp.DateBegin != null)
                    {
                        newItem.DateBegin = rp.DateBegin.Value.Date;
                    }
                    if (rp.DateEnd != null)
                    {
                        newItem.DateEnd = rp.DateEnd.Value.Date;
                    }
                    db.RaschetPeriod.AddObject(newItem);

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

        public string edit(RaschetPeriod rp)  // редактирование записи
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
            long ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

            try
            {
                if (!db.RaschetPeriod.Any(x => x.Year == rp.Year && x.Kvartal == rp.Kvartal && x.ID != ID))
                {
                    RaschetPeriod Item = db.RaschetPeriod.FirstOrDefault(x => x.ID == ID);

                    Item.Year = rp.Year;
                    Item.Name = rp.Name;
                    Item.Kvartal = rp.Kvartal;

                    if (rp.DateBegin != null)
                    {
                        Item.DateBegin = rp.DateBegin.Value.Date;
                    }
                    else
                        Item.DateBegin = null;
                    if (rp.DateEnd != null)
                    {
                        Item.DateEnd = rp.DateEnd.Value.Date;
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

        private void radButton2_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0)
            {
                int rowindex = radGridView1.CurrentRow.Index;
                long id = long.Parse(radGridView1.Rows[rowindex].Cells[0].Value.ToString());
                RaschetPeriod rp = db.RaschetPeriod.FirstOrDefault(x => x.ID == id);

                RaschetPeriodFrmEdit child = new RaschetPeriodFrmEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.radSpinEditor1.Value = rp.Year;
                child.radSpinEditor2.Value = rp.Kvartal.Value;
                child.radTextBox1.Text = rp.Name;
                if (rp.DateBegin.HasValue)
                {
                    child.dateBegin.EditValue = rp.DateBegin.Value;
                }
                if (rp.DateEnd.HasValue)
                {
                    child.dateEnd.EditValue = rp.DateEnd.Value;
                }

                child.action = "edit";
                child.ShowInTaskbar = false;
                child.ShowDialog();
            }
            else
                RadMessageBox.Show(this, "Нет данных для редактирования!");
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            // Удаление текущей записи
            if (radGridView1.RowCount != 0)
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

                        RaschetPeriod RaschPer = db.RaschetPeriod.FirstOrDefault(x => x.ID == id);
                        db.RaschetPeriod.DeleteObject(RaschPer);
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
                RadMessageBox.Show(this, "Нет данных для удаления!");
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radGridView1_CellDoubleClick(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            radButton2_Click(null, null);
        }

        private void RaschetPeriodFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            dataGrid_upd();
        }
    }
}
