using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using PU.Models;
using PU.Classes;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;
using System.Reflection;

namespace PU.FormsSZV_STAJ
{
    public partial class SZV_STAJ_FillStaff : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public FormsODV_1_2017 ODV1Data { get; set; }
        public bool Updated = false;
        List<FormsSZV_STAJ_4_2017> FormsSZV_STAJ_4_2017_List = new List<FormsSZV_STAJ_4_2017>();


        public SZV_STAJ_FillStaff()
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

        private void SZV_STAJ_FillStaff_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            loadRukBtn.ButtonElement.ToolTipText = "Загрузить данные о руководителе из справочника";

            loadRukBtn_Click(null, null);
            TypeInfo.Items.Single(x => x.Tag.ToString() == "2").Selected = true;
            DateFilling.Value = DateTime.Now.Date;
            Year.Value = DateTime.Now.AddDays(-30).Year;
            this.TypeInfo.SelectedIndexChanged += (s, с) => change_TypeInfo();

            this.Cursor = Cursors.WaitCursor;
            dataGrid_upd();
            this.Cursor = Cursors.Default;

        }

        private void change_TypeInfo()
        {
            if (TypeInfo.SelectedIndex == 2)
            {
                radGroupBox4.Enabled = true;
                radGroupBox2.Enabled = true;
            }
            else
            {
                radGroupBox4.Enabled = false;
                radGroupBox2.Enabled = false;
            }
        }

        public void dataGrid_upd()
        {

            //          RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            this.staffGridView.MasterTemplate.AutoGenerateColumns = true;


            var ins = db.Staff.Where(x => x.InsurerID == Options.InsID).OrderBy(x => x.LastName);

            List<string> checkedItems = new List<string>();
            foreach (var row in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
            {
                checkedItems.Add(row.Cells[1].Value.ToString());
            }

            staffGridView.Rows.Clear(); // If dgv is bound to datatable

            this.staffGridView.TableElement.BeginUpdate();


            List<StaffObject> staffList = new List<StaffObject>();
            if (ins.Count() != 0)
            {
                foreach (var item in ins)
                {
                    string dateb = "";
                    if (item.DateBirth != null)
                    {
                        dateb = item.DateBirth.HasValue ? item.DateBirth.Value.ToShortDateString() : "";
                    }
                    string contrNum = "";
                    if (item.ControlNumber != null)
                    {
                        contrNum = item.ControlNumber.HasValue ? item.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                    }

                    staffList.Add(new StaffObject()
                    {
                        ID = item.ID,
                        FIO = item.LastName + " " + item.FirstName + " " + item.MiddleName,

                        SNILS = !String.IsNullOrEmpty(item.InsuranceNumber) ? item.InsuranceNumber.Substring(0, 3) + "-" + item.InsuranceNumber.Substring(3, 3) + "-" + item.InsuranceNumber.Substring(6, 3) + " " + contrNum : "",
                        INN = !String.IsNullOrEmpty(item.INN) ? item.INN.PadLeft(12, '0') : " ",
                        TabelNumber = item.TabelNumber, // != null ? item.TabelNumber.Value.ToString() : ""
                        Sex = item.Sex.HasValue ? (item.Sex.Value == 0 ? "М" : "Ж") : "",
                        Dismissed = item.Dismissed.HasValue ? (item.Dismissed.Value == 1 ? "У" : " ") : " ",
                        DateBirth = dateb,
                        InsName = "",
                        InsReg = ""
                    });

                }
            }

            staffGridView.DataSource = staffList;

            staffGridView.Columns[0].Width = 26;
            staffGridView.Columns[0].IsPinned = true;
            staffGridView.Columns[0].PinPosition = PinnedColumnPosition.Left;
            staffGridView.Columns["ID"].IsVisible = false;
            staffGridView.Columns["ID"].IsPinned = true;
            staffGridView.Columns["Num"].IsVisible = false;
            staffGridView.Columns["Num"].IsPinned = true;
            staffGridView.Columns["Num"].HeaderText = "#";
            staffGridView.Columns["FIO"].Width = 230;
            staffGridView.Columns["FIO"].IsPinned = true;
            staffGridView.Columns["FIO"].ReadOnly = true;
            staffGridView.Columns["FIO"].HeaderText = "Фамилия Имя Отчество";
            staffGridView.Columns["SNILS"].Width = 100;
            staffGridView.Columns["SNILS"].IsPinned = true;
            staffGridView.Columns["SNILS"].HeaderText = "СНИЛС";
            staffGridView.Columns["INN"].Width = 80;
            staffGridView.Columns["INN"].IsPinned = true;
            staffGridView.Columns["INN"].HeaderText = "ИНН";
            staffGridView.Columns["TabelNumber"].HeaderText = "Табел.№";
            staffGridView.Columns["TabelNumber"].Width = 80;
            staffGridView.Columns["Sex"].HeaderText = "Пол";
            staffGridView.Columns["Sex"].Width = 50;
            staffGridView.Columns["Dismissed"].HeaderText = "Уволен";
            staffGridView.Columns["Dismissed"].Width = 50;
            staffGridView.Columns["DateBirth"].HeaderText = "Дата рождения";
            staffGridView.Columns["DateBirth"].Width = 110;
            staffGridView.Columns["DateBirth"].IsVisible = false;
            staffGridView.Columns["DepName"].HeaderText = "Подразделение";
            staffGridView.Columns["DepName"].IsVisible = false;
            staffGridView.Columns["Period"].VisibleInColumnChooser = false;
            staffGridView.Columns["Period"].IsVisible = false;
            staffGridView.Columns["TypeInfo"].VisibleInColumnChooser = false;
            staffGridView.Columns["TypeInfo"].IsVisible = false;
            staffGridView.Columns["KorrPeriod"].VisibleInColumnChooser = false;
            staffGridView.Columns["KorrPeriod"].IsVisible = false;
            staffGridView.Columns["InsReg"].VisibleInColumnChooser = false;
            staffGridView.Columns["InsReg"].IsVisible = false;
            staffGridView.Columns["InsName"].VisibleInColumnChooser = false;
            staffGridView.Columns["InsName"].IsVisible = false;



            for (var i = 4; i < staffGridView.Columns.Count; i++)
            {
                staffGridView.Columns[i].TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
                staffGridView.Columns[i].ReadOnly = true;
            }

            this.staffGridView.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            this.staffGridView.TableElement.EndUpdate();

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (ODV1Data != null)
            {
                if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                {

                    List<long> ids = staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true).Select(x => long.Parse(x.Cells[1].Value.ToString())).ToList();

                    byte ti = byte.Parse(TypeInfo.SelectedItem.Tag.ToString());
                    short y = short.Parse(Year.Text);

                    var szvStajList = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == ODV1Data.ID && x.TypeInfo == ti && x.Year == y).ToList();

                    int i = 0;
                    int k = 0;

                    foreach (var id in ids)
                    {
                        if (szvStajList.Any(x => x.StaffID == id))
                        {
                            if (updateODV1_DDL.SelectedItem.Tag.ToString() == "1")  // пропускать
                            {
                                k++;
                                continue;
                            }
                            if (updateODV1_DDL.SelectedItem.Tag.ToString() == "0")  // заменять
                            {
                                var t = db.FormsSZV_STAJ_2017.FirstOrDefault(x => x.StaffID == id && x.TypeInfo == ti && x.Year == y);
                                db.FormsSZV_STAJ_2017.Remove(t);
                            }

                        }

                        i++;

                        FormsSZV_STAJ_2017 newItem = new FormsSZV_STAJ_2017
                        {
                            FormsODV_1_2017_ID = ODV1Data.ID,
                            StaffID = id,
                            Year = y,
                            InsurerID = Options.InsID,
                            TypeInfo = ti,
                            DateFilling = DateFilling.Value.Date,
                            ConfirmFIO = ConfirmFIO.Text,
                            ConfirmDolgn = ConfirmDolgn.Text,
                            Dismissed = DismissedCheckBox.Checked
                        };

                        if (TypeInfo.SelectedIndex == 2)
                        {
                            newItem.OPSFeeNach = OPSFeeNachCheckBox.Checked ? (byte)1 : (byte)0;
                            newItem.DopTarFeeNach = DopTarFeeNachCheckBox.Checked ? (byte)1 : (byte)0;
                        }
                        else
                        {
                            newItem.OPSFeeNach = (byte)0;
                            newItem.DopTarFeeNach = (byte)0;
                        }

                        var fields = typeof(FormsSZV_STAJ_4_2017).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                        var names = Array.ConvertAll(fields, field => field.Name);


                        foreach (var item in FormsSZV_STAJ_4_2017_List)
                        {
                            //item.FormsODV_1_2017_ID = ODV1.ID;
                            FormsSZV_STAJ_4_2017 r = new FormsSZV_STAJ_4_2017();

                            foreach (var itemName_ in names)
                            {
                                string itemName = itemName_.TrimStart('_');
                                var properties = item.GetType().GetProperty(itemName);
                                if (properties != null)
                                {
                                    object value = properties.GetValue(item, null);
                                    var data = value;

                                    r.GetType().GetProperty(itemName).SetValue(r, data, null);
                                }

                            }

                            newItem.FormsSZV_STAJ_4_2017.Add(r);

                        }

                        db.FormsSZV_STAJ_2017.Add(newItem);

                        if (i % 100 == 0)
                        {
                            db.SaveChanges();
                        }
                    }
                    db.SaveChanges();

                     //ODV1Data
                    FormsODV_1_2017 odv1 = db.FormsODV_1_2017.First(x => x.ID == ODV1Data.ID);

                    odv1.StaffCount = odv1.FormsSZV_STAJ_2017.Count();

                    db.Entry(odv1).State =System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();


                    RadMessageBox.Show("Скопировано сотрудников:\r\nУспешно:  " + i.ToString() + "\r\nПропущено:  " + k.ToString());

                    Updated = true;
                    this.Close();

                }
                else
                {
                    Methods.showAlert("Внимание!", "Для начала копирования необходимо выделить хотя бы одного сотрудника!", this.ThemeName);
                }
            }
            else
            {
                Methods.showAlert("Внимание!", "Нет данных о Форме ОДВ-1!", this.ThemeName);
            }
        }

        private void DateFilling_ValueChanged(object sender, EventArgs e)
        {
            if (DateFilling.Value != DateFilling.NullDate)
                DateFillingMaskedEditBox.Text = DateFilling.Value.ToShortDateString();
            else
                DateFillingMaskedEditBox.Text = DateFillingMaskedEditBox.NullText;
        }

        private void DateFillingMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateFillingMaskedEditBox.Text != DateFillingMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateFillingMaskedEditBox.Text, out date))
                {
                    DateFilling.Value = date;
                }
                else
                {
                    DateFilling.Value = DateFilling.NullDate;
                }
            }
            else
            {
                DateFilling.Value = DateFilling.NullDate;
            }
        }

        private void loadRukBtn_Click(object sender, EventArgs e)
        {
            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            if (!String.IsNullOrEmpty(ins.BossDolgn))
            {
                ConfirmDolgn.Text = ins.BossDolgn;
            }
            if (!String.IsNullOrEmpty(ins.BossFIO))
            {
                ConfirmFIO.Text = ins.BossFIO;
            }
        }

        private void razd5_addBtn_Click(object sender, EventArgs e)
        {
            SZV_STAJ_5_Edit child = new SZV_STAJ_5_Edit();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
            if (child.formData != null)
            {
                FormsSZV_STAJ_4_2017_List.Add(child.formData);
                SZV_STAJ_4_Grid_update();
            }
        }

        private void SZV_STAJ_4_Grid_update()
        {
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            SZV_STAJ_4_Grid.Rows.Clear();

            if (FormsSZV_STAJ_4_2017_List.Count() != 0)
            {
                foreach (var item in FormsSZV_STAJ_4_2017_List)
                {
                    GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.SZV_STAJ_4_Grid.MasterView);
                    rowInfo.Cells["ID"].Value = item.ID;
                    if (item.DNPO_DateFrom.HasValue)
                        rowInfo.Cells["DNPO_DateFrom"].Value = item.DNPO_DateFrom.Value;
                    if (item.DNPO_DateTo.HasValue)
                        rowInfo.Cells["DNPO_DateTo"].Value = item.DNPO_DateTo.Value;
                    rowInfo.Cells["DNPO_Fee"].Value = item.DNPO_Fee.HasValue ? item.DNPO_Fee.Value : false;
                    SZV_STAJ_4_Grid.Rows.Add(rowInfo);
                }
            }

            this.SZV_STAJ_4_Grid.MasterTemplate.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
            SZV_STAJ_4_Grid.Refresh();
        }

        private void razd5_editBtn_Click(object sender, EventArgs e)
        {
            if (SZV_STAJ_4_Grid.RowCount != 0)
            {
                int rowindex = SZV_STAJ_4_Grid.CurrentRow.Index;

                SZV_STAJ_5_Edit child = new SZV_STAJ_5_Edit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.ShowInTaskbar = false;
                child.formData = FormsSZV_STAJ_4_2017_List.Skip(rowindex).Take(1).First();
                child.ShowDialog();

                if (child.formData != null)
                {
                    FormsSZV_STAJ_4_2017_List.RemoveAt(rowindex);
                    FormsSZV_STAJ_4_2017_List.Insert(rowindex, child.formData);

                    SZV_STAJ_4_Grid_update();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!");
            }
        }

        private void razd5_delBtn_Click(object sender, EventArgs e)
        {
            if (SZV_STAJ_4_Grid.RowCount != 0)
            {
                int rowindex = SZV_STAJ_4_Grid.CurrentRow.Index;
                FormsSZV_STAJ_4_2017_List.RemoveAt(rowindex);

                SZV_STAJ_4_Grid_update();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!");
            }
        }



    }
}
