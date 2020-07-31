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
using Telerik.WinControls.UI;

namespace PU.FormsRSW2014
{
    public partial class TariffCodeEdit : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public TariffCode formData { get; set; }
        public List<long> selectedItems { get; set; }
        public string action;
        public TariffCodeEdit()
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
                formData = null;
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void list_upd()
        {
            this.radListControl1.Items.Clear();
            this.radListControl2.Items.Clear();

            if (dateBegin.Value.Year > 1990)
            {
                var raschetPeriod = db.PlatCategoryRaschPer.FirstOrDefault(x => ((!x.DateEnd.HasValue && x.DateBegin.Value <= dateBegin.Value) || (x.DateBegin.Value <= dateBegin.Value && dateBegin.Value <= x.DateEnd.Value)));
                if (raschetPeriod == null)
                    radLabel6.Text = "";
                else
                {
                    radLabel6.Text = raschetPeriod.NAME;
                    var list = raschetPeriod.PlatCategory.Where(y => y.ID > 0);
                    if (formData != null)
                        if (formData.TariffCodePlatCat.Any())
                        {
                            var existList = formData.TariffCodePlatCat.Select(y => y.PlatCategoryID.Value).ToList();
                            var list2 = list.Where(x => existList.Contains(x.ID));
                            list = list.Where(x => !existList.Contains(x.ID));
                            //                    var list2 = db.PlatCategory.Where(x => existList.Contains(x.ID));
                            selectedItems = new List<long> { };


                            foreach (var item in list2)
                            {
                                radListControl2.Items.Add(new RadListDataItem(item.Code, item.ID));
                                selectedItems.Add(item.ID);
                            }


                        }

                    foreach (var item in list)
                    {
                        radListControl1.Items.Add(new RadListDataItem(item.Code, item.ID));
                    }
                }
            }

        }

        private void TariffCodeEdit_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            this.radListControl1.SelectedIndex = -1;
            this.radListControl2.SelectedIndex = -1;
            switch (action)
            {
                case "add":
                    dateBegin.Value = DateTime.Now.Date;
                    dateEnd.Value = DateTime.Now.Date;
                    radTextBox1.ReadOnly = false;
                    formData = new TariffCode();
                    break;
                case "edit":
                    radTextBox1.Text = formData.Code;
                    dateBegin.Value = formData.DateBegin.Value;
                    if (formData.DateEnd.HasValue)
                        dateEnd.Value = formData.DateEnd.Value;
                    break;

            }
            list_upd();

        }






        private void radButton3_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (radListControl1.Items.Count > 0 && radListControl1.SelectedItem != null)
            {
                RadListDataItem item = radListControl1.SelectedItem;
                index = radListControl1.SelectedItem.RowIndex;
                radListControl1.Items.Remove(item);
                radListControl2.Items.Add(item);
            }
            if (radListControl1.Items.Count > 0)
            {
                index = index < radListControl1.Items.Count() ? index : (index - 1);
                radListControl1.Items[index].Selected = true;
            }
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (radListControl2.Items.Count > 0 && radListControl2.SelectedItem != null)
            {
                RadListDataItem item = radListControl2.SelectedItem;
                index = radListControl2.SelectedItem.RowIndex;
                radListControl2.Items.Remove(item);
                radListControl1.Items.Add(item);
            }
            if (radListControl2.Items.Count > 0)
            {
                index = index < radListControl2.Items.Count() ? index : (index - 1);
                radListControl2.Items[index].Selected = true;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            formData = null;
            this.Close();
        }

        private void TariffCodeEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            db = null;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            if (radTextBox1.Text != "")
            {
                if (dateEnd.Value != dateEnd.NullDate && dateBegin.Value > dateEnd.Value)
                {
                    RadMessageBox.Show("Дата начала не может быть больше даты окончания!");
                    return;
                }

                selectedItems = new List<long>();

                formData.Code = radTextBox1.Text;
                if (dateBegin.Value != null)
                    formData.DateBegin = dateBegin.Value;
                else
                    formData.DateBegin = null;

                if ((dateEnd.Value != dateEnd.NullDate) && (dateEnd.Value != null))
                    formData.DateEnd = DateTime.Parse(dateEnd.Value.ToString()).Date;
                else
                    formData.DateEnd = null;
                
                if (radListControl2.Items.Any())
                {
                    foreach(var item in radListControl2.Items.Select(x => x.Value))
                    selectedItems.Add(long.Parse(item.ToString()));
                }
                this.Close();
            }
            else
                RadMessageBox.Show("Поле \"Код\" должно быть заполнено!");
        }

        private void dateBeginMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (dateBeginMaskedEditBox.Text != dateBeginMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(dateBeginMaskedEditBox.Text, out date))
                {
                    dateBegin.Value = date;
                }
                else
                {
                    dateBegin.Value = dateBegin.NullDate;
                }
            }
            else
            {
                dateBegin.Value = dateBegin.NullDate;
            }
        }

        private void dateEndMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (dateEndMaskedEditBox.Text != dateEndMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(dateEndMaskedEditBox.Text, out date))
                {
                    dateEnd.Value = date;
                }
                else
                {
                    dateEnd.Value = dateEnd.NullDate;
                }
            }
            else
            {
                dateEnd.Value = dateEnd.NullDate;
            }
        }

        private void dateBegin_ValueChanged(object sender, EventArgs e)
        {
            if (dateBegin.Value != dateBegin.NullDate)
                dateBeginMaskedEditBox.Text = dateBegin.Value.ToShortDateString();
            else
                dateBeginMaskedEditBox.Text = dateBeginMaskedEditBox.NullText;

            list_upd();
        }

        private void dateEnd_ValueChanged(object sender, EventArgs e)
        {
            if (dateEnd.Value != dateEnd.NullDate)
                dateEndMaskedEditBox.Text = dateEnd.Value.ToShortDateString();
            else
                dateEndMaskedEditBox.Text = dateEndMaskedEditBox.NullText;
        }








    }
}
