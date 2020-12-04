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

namespace PU.FormsDSW3
{
    public partial class DSW3_FillStaff : Telerik.WinControls.UI.RadForm
    {
        private pu6Entities db = new pu6Entities();
        public FormsDSW_3 dsw3Data { get; set; }
        public bool Updated = false;

        public DSW3_FillStaff()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void DSW3_FillStaff_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            this.Cursor = Cursors.WaitCursor;
            dataGrid_upd();
            this.Cursor = Cursors.Default;

            if (dsw3Data != null)
            {
                dsw3Number.Text = "№ " + dsw3Data.NUMBERPAYMENT + "  от " + dsw3Data.DATEPAYMENT.ToShortDateString();
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

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (dsw3Data != null)
            {
                if (staffGridView.Rows.Any(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true)) 
                {
                    List<long> ids = new List<long>();
                    foreach (var item in staffGridView.Rows.Where(x => x.Cells[0].Value != null && (bool)x.Cells[0].Value == true))
                    {
                        ids.Add(long.Parse(item.Cells[1].Value.ToString()));
                    }

                    var dsw3StaffList = db.FormsDSW_3_Staff.Where(x => x.FormsDSW_3_ID == dsw3Data.ID).Select(x => x.StaffID).ToList();

                    var listToAdd = ids.Except(dsw3StaffList).ToList();

                    var errCnt = ids.Count(x => dsw3StaffList.Contains(x));

                    int i = 0;
                    foreach (var id in listToAdd)
                    {
                        i++;
                        FormsDSW_3_Staff newItem = new FormsDSW_3_Staff
                        {
                            FormsDSW_3_ID = dsw3Data.ID,
                            StaffID = id,
                            SUMFEEPFR_EMPLOYERS = (decimal)SUMFEEPFR_EMPLOYERS.EditValue,
                            SUMFEEPFR_PAYER = (decimal)SUMFEEPFR_PAYER.EditValue
                        };

                        db.FormsDSW_3_Staff.Add(newItem);

                        if (i % 200 == 0)
                        {
                            db.SaveChanges();
                        }
                    }
                    db.SaveChanges();

                    RadMessageBox.Show("Скопировано сотрудников:\r\nУспешно:  " + listToAdd.Count + "\r\nОтклонено:  " + errCnt);

                    Updated = true;
                    this.Close();

                }
                else
                {
                    Messenger.showAlert(AlertType.Info, "Внимание!", "Для начала копирования необходимо выделить хотя бы одного сотрудника!", this.ThemeName);
                }
            }
        }


    }
}
