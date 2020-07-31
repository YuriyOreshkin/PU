using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using PU.Models;
using PU.Classes;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;

namespace PU.FormsDSW3
{
    public partial class DSW3_EditStaff : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action;
        private List<ErrList> errMessBox = new List<ErrList>();
        private bool cleanData = true;
        public FormsDSW_3_Staff DSW3StaffData = new FormsDSW_3_Staff();
        public FormsDSW_3 DSW3Data { get; set; }
        public long DSW3StaffID = 0;


        public DSW3_EditStaff()
        {
            InitializeComponent();
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    saveBtn.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.BeginInvoke(new MethodInvoker(this.Close));
                    return;
            }

            level = Methods.checkUserAccessLevel("StaffFrm");

            switch (level)
            {
                case 2:
                    selectStaffBtn.Enabled = false;
                    SNILS.Enabled = false;
                    TabelNumber.Enabled = false;
                    INN.Enabled = false;
                    LastName.Enabled = false;
                    FirstName.Enabled = false;
                    MiddleName.Enabled = false;
                    break;
                case 3:
                    selectStaffBtn.Enabled = false;
                    SNILS.Enabled = false;
                    TabelNumber.Enabled = false;
                    INN.Enabled = false;
                    LastName.Enabled = false;
                    FirstName.Enabled = false;
                    MiddleName.Enabled = false;
                    break;
            }

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
                closeBtn_Click(null, null);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            DSW3StaffData = null;
            this.Close();
        }

        private void DSW3_EditStaff_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cleanData)
                DSW3StaffData = null;
        }

        private void DSW3_EditStaff_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            checkAccessLevel();

            if (DSW3Data != null)
            {
                dsw3Number.Text = "№ " + DSW3Data.NUMBERPAYMENT + "  от " + DSW3Data.DATEPAYMENT.ToShortDateString();
            }

            if (action == "edit")
            {
                if (db.FormsDSW_3_Staff.Any(x => x.ID == DSW3StaffID))
                {
                    DSW3StaffData = db.FormsDSW_3_Staff.FirstOrDefault(x => x.ID == DSW3StaffID);

                    Staff staffData = db.Staff.FirstOrDefault(x => x.ID == DSW3StaffData.StaffID);

                    SNILS.Text = staffData.ControlNumber != null ? staffData.InsuranceNumber + staffData.ControlNumber.Value.ToString().PadLeft(2, '0') : staffData.InsuranceNumber;
                    TabelNumber.Text = staffData.TabelNumber != null ? staffData.TabelNumber.Value.ToString() : "";
                    INN.Text = (!String.IsNullOrEmpty(staffData.INN) && staffData.INN != "0") ? staffData.INN.PadLeft(12, '0') : "";
                    LastName.Text = staffData.LastName;
                    FirstName.Text = staffData.FirstName;
                    MiddleName.Text = staffData.MiddleName;

                    SUMFEEPFR_EMPLOYERS.EditValue = DSW3StaffData.SUMFEEPFR_EMPLOYERS.HasValue ? DSW3StaffData.SUMFEEPFR_EMPLOYERS.Value : (decimal)0;
                    SUMFEEPFR_PAYER.EditValue = DSW3StaffData.SUMFEEPFR_PAYER.HasValue ? DSW3StaffData.SUMFEEPFR_PAYER.Value : (decimal)0;

                    setReadonly(false);
                    SNILS.ReadOnly = true;
                    selectStaffBtn.Enabled = false;
                }
                else
                {
                    RadMessageBox.Show("Ошибка! Не удалось получить запись из базы данных!");
                }
            }
            else
            {
                DSW3StaffData = new FormsDSW_3_Staff();
            }


        }

        private void INN__Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(INN.Text))
                INN.Text = INN.Text.PadLeft(12, '0');
        }

        private void INN__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void Snils__KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                TabelNumber.Focus();
            }
        }

        private void Snils__Leave(object sender, EventArgs e)
        {
            var ssn = SNILS.Value.ToString().Split(' ');

            if (!ssn[0].Contains("_"))
            {
                string contrNum = Utils.GetControlSumSSN(ssn[0]);

                SNILS.Value = ssn[0] + " " + contrNum;

                string snils = ssn[0].Replace("-", "");

                if (db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils))
                {
                    Staff staff = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils);
                    FirstName.Text = staff.FirstName;
                    LastName.Text = staff.LastName;
                    MiddleName.Text = staff.MiddleName;
                    newStaffLabel.Visible = false;
                    setReadonly(true);
                }
                else
                {
                    newStaffLabel.Visible = true;
                    setReadonly(false);
                }
            }
            else
            {
                newStaffLabel.Visible = false;
                setReadonly(false);
                if (SNILS.Value.ToString() != "___-___-___ __")
                {
                    SNILS.Text = "___-___-___ __";
                }

            }
        }

        private void selectStaffBtn_Click(object sender, EventArgs e)
        {
            Staff staff = null;

            var ssn = SNILS.Value.ToString().Split(' ');

            if (!ssn[0].Contains("_"))
            {
                string snils = ssn[0].Replace("-", "");

                staff = db.Staff.FirstOrDefault(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils);
            }

            StaffFrm child = new StaffFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.InsID = Options.InsID;
            child.action = "selection";
            child.StaffID = staff == null ? 0 : staff.ID; ;
            child.ShowDialog();
            long id = child.StaffID;
            if (db.Staff.Any(x => x.ID == id))
            {
                staff = db.Staff.FirstOrDefault(x => x.ID == id);
                LastName.Text = staff.LastName;
                FirstName.Text = staff.FirstName;
                MiddleName.Text = staff.MiddleName;
                SNILS.Text = staff.ControlNumber != null ? (staff.InsuranceNumber + staff.ControlNumber.Value.ToString().PadLeft(2, '0')) : staff.InsuranceNumber;

                newStaffLabel.Visible = false;
                setReadonly(true);
            }
        }

        private void setReadonly(bool flag)
        {
            TabelNumber.ReadOnly = flag;
            INN.ReadOnly = flag;
            LastName.ReadOnly = flag;
            FirstName.ReadOnly = flag;
            MiddleName.ReadOnly = flag;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                if (action == "add")
                {
                    string snils = "";
                    byte contrNum = 0;

                    if (!SNILS.Value.ToString().Contains("_"))
                    {
                        if (Utils.ValidateSSN(SNILS.Value.ToString()))
                        {
                            var ssn = Utils.ParseSNILS_XML(SNILS.Text, true);

                            snils = ssn.num;
                            if (ssn.contrNum.HasValue)
                                contrNum = ssn.contrNum.Value;
                        }
                    }

                    try
                    {
                        if (newStaffLabel.Visible && !db.Staff.Any(x => x.InsurerID == Options.InsID && x.InsuranceNumber == snils)) // если отмечено добавить и такого сотрудника еще нет то добавляем его
                        {
                            Staff newStaff = new Staff
                            {
                                InsurerID = Options.InsID,
                                InsuranceNumber = snils,
                                ControlNumber = contrNum,
                                LastName = LastName.Text,
                                FirstName = FirstName.Text,
                                MiddleName = MiddleName.Text,
                                INN = INN.Text
                            };

                            if (!String.IsNullOrEmpty(TabelNumber.Text) && long.Parse(TabelNumber.Text) != 0)
                                newStaff.TabelNumber = long.Parse(TabelNumber.Text);
                            else
                                newStaff.TabelNumber = null;

                            db.Staff.Add(newStaff);
                            db.SaveChanges();

                        }
                    }
                    catch (Exception ex)
                    {
                        Methods.showAlert("Внимание!", "При сохранение данных о сотруднике произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
                    }
                }


                switch (action)
                { 
                    case "add":
                       var ssn = Utils.ParseSNILS_XML(SNILS.Text, false);

                        var snils = ssn.num;

                        if (db.Staff.Any(x => x.InsuranceNumber == snils && x.InsurerID == Options.InsID))
                        {
                            var staffID = db.Staff.FirstOrDefault(x => x.InsuranceNumber == snils && x.InsurerID == Options.InsID).ID;

                            FormsDSW_3_Staff dsw3staffNew = new FormsDSW_3_Staff { 
                                FormsDSW_3_ID = DSW3Data.ID,
                                StaffID = staffID,
                                SUMFEEPFR_EMPLOYERS = (decimal)SUMFEEPFR_EMPLOYERS.EditValue,
                                SUMFEEPFR_PAYER = (decimal)SUMFEEPFR_PAYER.EditValue
                            };

                            db.FormsDSW_3_Staff.Add(dsw3staffNew);
                            try
                            {
                                db.SaveChanges();
                                cleanData = false;
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных в базу данных произошла ошибка! Код ошибки: " + ex.InnerException.Message);
                            }
                        }
                        else
                            RadMessageBox.Show(this,"Не удалось загрузить данные по сотруднику из базы данных!", "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);

                        break;
                    case "edit":
                        if (DSW3StaffData.SUMFEEPFR_EMPLOYERS != (decimal)SUMFEEPFR_EMPLOYERS.EditValue || DSW3StaffData.SUMFEEPFR_PAYER != (decimal)SUMFEEPFR_PAYER.EditValue)
                        {
                            DSW3StaffData.SUMFEEPFR_EMPLOYERS = (decimal)SUMFEEPFR_EMPLOYERS.EditValue;
                            DSW3StaffData.SUMFEEPFR_PAYER = (decimal)SUMFEEPFR_PAYER.EditValue;
                            db.Entry(DSW3StaffData).State = EntityState.Modified;
                            try
                            {
                                db.Entry(DSW3StaffData).State = EntityState.Modified;
                                db.SaveChanges();
                                cleanData = false;
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных в базу данных произошла ошибка! Код ошибки: " + ex.InnerException.Message);
                            }
                        }
                        else
                        {
                            cleanData = false;
                            this.Close();

                        }
                        break;
                }

            }
            else
            {
                RadMessageBox.Show(this, errMessBox[0].name, "Внимание!", MessageBoxButtons.OK, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private bool validation()
        {
            bool result = true;
            errMessBox.Clear();

            if (SNILS.Value.ToString() == "___-___-___ __")
                errMessBox.Add(new ErrList { name = "Поле СНИЛС обязательно к заполнению!", control = "SNILS" });
            if (String.IsNullOrEmpty(LastName.Text))
                errMessBox.Add(new ErrList { name = "Поле Фамилия обязательно к заполнению!", control = "LastName" });
            if (String.IsNullOrEmpty(FirstName.Text))
                errMessBox.Add(new ErrList { name = "Поле Имя обязательно к заполнению!", control = "FirstName" });

            if (action == "add")
            {
                if (!SNILS.Value.ToString().Contains("_"))
                {
                    if (Utils.ValidateSSN(SNILS.Value.ToString()))
                    {
                        var ssn = Utils.ParseSNILS_XML(SNILS.Text, false);

                        var snils = ssn.num;

                        if (db.Staff.Any(x => x.InsuranceNumber == snils && x.InsurerID == Options.InsID))
                        {
                            var staffID = db.Staff.FirstOrDefault(x => x.InsuranceNumber == snils && x.InsurerID == Options.InsID).ID;
                            if (db.FormsDSW_3_Staff.Any(x => x.FormsDSW_3_ID == DSW3Data.ID && x.StaffID == staffID))
                            {
                                errMessBox.Add(new ErrList { name = "Данные по этому сотруднику уже добавлены в это платежное поручение", control = "SNILS" });
                            }

                        }
                    }
                    else
                    {
                        errMessBox.Add(new ErrList { name = "СНИЛС не прошел проверку!", control = "SNILS" });
                    }
                }
            }


            if (errMessBox.Count > 0)
                result = false;

            return result;
        }
    }
}
