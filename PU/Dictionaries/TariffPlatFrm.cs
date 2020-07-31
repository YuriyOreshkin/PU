using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using PU.Models;
using PU.Classes;
using Telerik.WinControls.UI.Localization;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU.FormsRSW2014
{
    public partial class TariffPlatFrm : Telerik.WinControls.UI.RadForm
    {
     //   ColumnGroupsViewDefinition columnGroupsView;

        pu6Entities db = new pu6Entities();
        public PlatCategory PlatCat {get; set;}
        private bool copyFlag = false;

        public TariffPlatFrm()
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

        public static TariffPlatFrm SelfRef
        {
            get;
            set;
        }

        public void dataGrid_upd()
        {

            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            BindingSource b = new BindingSource();
            b.DataSource = db.TariffPlat.Where(x => x.PlatCategoryID == PlatCat.ID);

            radGridView1.DataSource = b;

            //  radGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            radGridView1.Columns["ID"].IsVisible = false;
            radGridView1.Columns["PlatCategoryID"].IsVisible = false;
            radGridView1.Columns["PlatCategory"].IsVisible = false;

            radGridView1.Columns["Year"].HeaderText = "Год";
            radGridView1.Columns["Year"].Width = radGridView1.Width / 6;

            radGridView1.Columns["StrahPercant1966"].HeaderText = "ПФР страх.часть 1966 и старше";
            radGridView1.Columns["StrahPercant1966"].Width = radGridView1.Width / 6;

            radGridView1.Columns["StrahPercent1967"].HeaderText = "ПФР страх.часть 1967 и моложе";
            radGridView1.Columns["StrahPercent1967"].Width = radGridView1.Width / 6;

            radGridView1.Columns["NakopPercant"].HeaderText = "ПФР накоп.часть 1967 и моложе";
            radGridView1.Columns["NakopPercant"].Width = radGridView1.Width / 6;

            radGridView1.Columns["FFOMS_Percent"].HeaderText = "ОМС (ФФОМС)";
            radGridView1.Columns["FFOMS_Percent"].Width = radGridView1.Width / 6;

            radGridView1.Columns["TFOMS_Percent"].HeaderText = "ТФОМС";
            radGridView1.Columns["TFOMS_Percent"].Width = radGridView1.Width / 6;


            radGridView1.Refresh();

        }

        private void copyChangeState()
        {
            copyPanel.Visible = copyFlag;
        }

        private void TariffPlatFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            copySpin.Value = PlatCat.TariffPlat.Any() ? PlatCat.TariffPlat.Max(x => x.Year).Value + 1 : DateTime.Now.Year;
            copyChangeState();

            dataGrid_upd();
  //          InitializeGrid();
  //          this.radGridView1.ViewDefinition = columnGroupsView;

        }

        private void InitializeGrid()
        {
    /*        this.columnGroupsView = new ColumnGroupsViewDefinition();
            this.columnGroupsView.ColumnGroups.Add(new GridViewColumnGroup("Year"));
            this.columnGroupsView.ColumnGroups.Add(new GridViewColumnGroup("General"));
            this.columnGroupsView.ColumnGroups.Add(new GridViewColumnGroup("Details"));
            this.columnGroupsView.ColumnGroups[1].Groups.Add(new GridViewColumnGroup("Address"));
            this.columnGroupsView.ColumnGroups[1].Groups.Add(new GridViewColumnGroup());
            this.columnGroupsView.ColumnGroups[0].Rows.Add(new GridViewColumnGroupRow());
            this.columnGroupsView.ColumnGroups[0].Rows.Add(new GridViewColumnGroupRow());
            this.columnGroupsView.ColumnGroups[0].Rows[0].Columns.Add(this.radGridView1.Columns["CustomerID"]);
            this.columnGroupsView.ColumnGroups[0].Rows[0].Columns.Add(this.radGridView1.Columns["ContactName"]);
            this.columnGroupsView.ColumnGroups[0].Rows[1].Columns.Add(this.radGridView1.Columns["CompanyName"]);
            this.columnGroupsView.ColumnGroups[1].Groups[0].Rows.Add(new GridViewColumnGroupRow());
            this.columnGroupsView.ColumnGroups[1].Groups[0].Rows[0].Columns.Add(this.radGridView1.Columns["City"]);
            this.columnGroupsView.ColumnGroups[1].Groups[0].Rows[0].Columns.Add(this.radGridView1.Columns["Country"]);
            this.columnGroupsView.ColumnGroups[1].Groups[1].Rows.Add(new GridViewColumnGroupRow());
            this.columnGroupsView.ColumnGroups[1].Groups[1].Rows[0].Columns.Add(this.radGridView1.Columns["Phone"]);
            */
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {

            TariffPlatFrmEdit child = new TariffPlatFrmEdit();
            child.Owner = this;
            child.action = "add";
            child.ThemeName = this.ThemeName;
            child.radSpinEditor1.Text = DateTime.Now.Date.Year.ToString();
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        public string add(TariffPlat tp)  // добавление записи
        {
            string result = "";
            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
            tp.PlatCategoryID = PlatCat.ID;
            try
            {
                if (!db.TariffPlat.Any(x => x.Year == tp.Year && x.PlatCategoryID == PlatCat.ID))
                {
                    db.TariffPlat.Add(tp);
                    db.SaveChanges();
                    dataGrid_upd();
                    radGridView1.Rows[rowindex].IsCurrent = true;
                }
                else
                {
                    result = "Данные за указанный год для выбранной категории уже есть в БД";
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        public string edit(TariffPlat tp)  // редактирование записи
        {
            string result = "";

            int rowindex = radGridView1.RowCount == 0 ? 0 : radGridView1.CurrentRow.Index;
            long ID = Convert.ToInt64(radGridView1.Rows[rowindex].Cells[0].Value);

            try
            {
                if (!db.TariffPlat.Any(x => x.Year == tp.Year && x.PlatCategoryID == PlatCat.ID && x.ID != ID))
                {
                    TariffPlat TarPlat = db.TariffPlat.FirstOrDefault(x => x.ID == ID);

                    TarPlat.Year = tp.Year.Value;
                    TarPlat.StrahPercant1966 = tp.StrahPercant1966.Value;
                    TarPlat.StrahPercent1967 = tp.StrahPercent1967.Value;
                    TarPlat.NakopPercant = tp.NakopPercant.Value;
                    TarPlat.FFOMS_Percent = tp.FFOMS_Percent.Value;
                    TarPlat.TFOMS_Percent = tp.TFOMS_Percent.Value;

                    db.Entry(TarPlat).State = EntityState.Modified;
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
                TariffPlatFrmEdit child = new TariffPlatFrmEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.radSpinEditor1.Value = decimal.Parse(radGridView1.CurrentRow.Cells["Year"].Value.ToString());

                child.radMaskedEditBox1.Text = radGridView1.CurrentRow.Cells["StrahPercant1966"].Value.ToString();
                child.radMaskedEditBox2.Text = radGridView1.CurrentRow.Cells["StrahPercent1967"].Value.ToString();
                child.radMaskedEditBox3.Text = radGridView1.CurrentRow.Cells["NakopPercant"].Value.ToString();
                child.radMaskedEditBox4.Text = radGridView1.CurrentRow.Cells["FFOMS_Percent"].Value.ToString();
                child.radMaskedEditBox5.Text = radGridView1.CurrentRow.Cells["TFOMS_Percent"].Value.ToString();
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
                long id = Convert.ToInt64(radGridView1.CurrentRow.Cells[0].Value.ToString());
                DialogResult dialogResult = RadMessageBox.Show(this, "Уверены что хотите удалить текущую запись?", title, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        rowindex = rowindex + 1 == radGridView1.RowCount ? rowindex - 1 : rowindex;

                        TariffPlat TarPlat = db.TariffPlat.FirstOrDefault(x => x.ID == id);
                        db.TariffPlat.Remove(TarPlat);
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
                RadMessageBox.Show(this, "Нет данных для удаления!", "");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            copyFlag = !copyFlag;
            copyChangeState();
        }

        /// <summary>
        /// Копирование тарифа на новый год
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButton5_Click(object sender, EventArgs e)
        {
            if (radGridView1.RowCount != 0 && radGridView1.CurrentRow.Cells["ID"].Value != null)
            {
                long id = 0;
                if (long.TryParse(radGridView1.CurrentRow.Cells["ID"].Value.ToString(), out id))
                {
                    TariffPlat tp = db.TariffPlat.FirstOrDefault(x => x.ID == id);

                    if (!db.TariffPlat.Any(x => x.PlatCategoryID == tp.PlatCategoryID && x.Year == (short)copySpin.Value))
                    {
                        TariffPlat tp_new = new TariffPlat
                        {
                            Year = (short)copySpin.Value,
                            FFOMS_Percent = tp.FFOMS_Percent,
                            NakopPercant = tp.NakopPercant,
                            PlatCategoryID = tp.PlatCategoryID,
                            StrahPercant1966 = tp.StrahPercant1966,
                            StrahPercent1967 = tp.StrahPercent1967,
                            TFOMS_Percent = tp.TFOMS_Percent
                        };

                        db.TariffPlat.Add(tp_new);
                        db.SaveChanges();
                        dataGrid_upd();
                        radGridView1.Rows.FirstOrDefault(x => x.Cells["Year"].Value.ToString() == copySpin.Value.ToString()).IsCurrent = true;
                    }
                    else
                    {
                        RadMessageBox.Show(this, "За указанный для копирования год, уже есть запись", "Внимание");
                    }


                }
            }
        }

    }
}
