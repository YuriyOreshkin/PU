using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Classes;

namespace PU.FormsPredPens
{
    public partial class PredPensZapros_Edit : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public string action { get; set; }
        public FormsPredPens_Zapros PredPensData = new FormsPredPens_Zapros();
        public long PredPensID = 0;
        private bool cleanData = true;
        private List<ErrList> errMessBox = new List<ErrList>();

        public PredPensZapros_Edit()
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

        private void DateFilling_ValueChanged(object sender, EventArgs e)
        {
            if (DateFilling.Value != DateFilling.NullDate)
                DateFillingMaskedEditBox.Text = DateFilling.Value.ToShortDateString();
            else
                DateFillingMaskedEditBox.Text = DateFillingMaskedEditBox.NullText;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            PredPensData = null;
            this.Close();
        }

        private void PredPensZapros_Edit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);


            switch (action)
            {
                case "add":
                    TypeOrg.Items.First(x => x.Tag.ToString() == "3").Selected = true;
                    DateFilling.Value = DateTime.Now;

                    Insurer ins = db.Insurer.First(x => x.ID == Options.InsID);

                    if (ins.TypePayer != 0)  // не организация
                    {
                        ConfirmLastName.Text = !String.IsNullOrEmpty(ins.LastName) ? ins.LastName : "";
                        ConfirmFirstName.Text = !String.IsNullOrEmpty(ins.FirstName) ? ins.FirstName : "";
                        ConfirmMiddleName.Text = !String.IsNullOrEmpty(ins.MiddleName) ? ins.MiddleName : "";
                    }
                    else
                    {
                        try
                        {
                            var FIO = ins.BossFIO.Split(' ');

                            ConfirmLastName.Text = FIO[0] != null ? (!String.IsNullOrEmpty(FIO[0]) ? FIO[0] : "") : "";
                            ConfirmFirstName.Text = FIO[1] != null ? (!String.IsNullOrEmpty(FIO[1]) ? FIO[1] : "") : "";
                            ConfirmMiddleName.Text = FIO[2] != null ? (!String.IsNullOrEmpty(FIO[2]) ? FIO[2] : "") : "";
                        }
                        catch { }
                    }

                    PredPensData = new FormsPredPens_Zapros { };
                    break;
                case "edit":
                    if (db.FormsPredPens_Zapros.Any(x => x.ID == PredPensID))
                    {
                        PredPensData = db.FormsPredPens_Zapros.First(x => x.ID == PredPensID);

                        TypeOrg.Items.Single(x => x.Tag.ToString() == PredPensData.TypeORG.ToString()).Selected = true;

                        NUMBER.Text = PredPensData.Number;
                        if (PredPensData.Date != null)
                            DateFilling.Value = PredPensData.Date;

                        ConfirmLastName.Text = PredPensData.LastName;
                        ConfirmFirstName.Text = PredPensData.FirstName;
                        ConfirmMiddleName.Text = PredPensData.MiddleName;

                    }
                    else
                    {
                        RadMessageBox.Show("Не удалось загрузить данные Запроса из базы данных!");
                    }
                    break;
            }

        }

        private void PredPensZapros_Edit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cleanData)
                PredPensData = null;
        }

        private void getValues()
        {
            PredPensData.Date = DateFilling.Value;
            PredPensData.InsurerID = Options.InsID;
            PredPensData.Number = NUMBER.Text.Trim();

            PredPensData.TypeORG = byte.Parse(TypeOrg.SelectedItem.Tag.ToString());

            PredPensData.LastName = ConfirmLastName.Text;
            PredPensData.FirstName = ConfirmFirstName.Text;
            PredPensData.MiddleName = ConfirmMiddleName.Text;


        }

        /// <summary>
        /// Проверка введенных данных
        /// </summary>
        /// <returns></returns>
        private bool validation()
        {
            bool check = true;
            errMessBox.Clear();

            if (String.IsNullOrEmpty(NUMBER.Text.Trim()))
            {
                errMessBox.Add(new ErrList { name = "Необходимо указать номер Запроса" });
            }

            if (DateFilling.Value == DateFilling.NullDate)
                errMessBox.Add(new ErrList { name = "Необходимо указать дату Запроса" });

            byte t = 0;

            if (TypeOrg.SelectedItem != null)
            {
                t = byte.Parse(TypeOrg.SelectedItem.Tag.ToString());
            }
            else
            {
                RadMessageBox.Show("Необходимо выбрать Тип формы!");
                check = false;
            }


            if (errMessBox.Count <= 0)
                switch (action)
                {
                    case "add":
                        if (db.FormsPredPens_Zapros.Any(x => x.InsurerID == Options.InsID && x.Number == NUMBER.Text.Trim() && x.Date == DateFilling.Value && x.TypeORG == t))
                        {
                            errMessBox.Add(new ErrList { name = "Ошибка! Нарушение уникальности записей." });
                        }

                        break;
                    case "edit":
                        if (db.FormsPredPens_Zapros.Any(x => x.InsurerID == Options.InsID && x.Number == NUMBER.Text.Trim() && x.Date == DateFilling.Value && x.TypeORG == t && x.ID != PredPensData.ID))
                        {
                            errMessBox.Add(new ErrList { name = "Ошибка! Нарушение уникальности записей." });
                        }
                        break;
                }


            if (errMessBox.Count > 0)
                check = false;
            return check;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                bool flag_ok = false;
                try
                {
                    getValues();
                    flag_ok = true;
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("При сохранении данных произошла ошибка. Ошибка при сборе данных формы. Код ошибки: " + ex.Message);
                }

                if (flag_ok)
                {
                    switch (action)
                    {
                        case "add":

                            try
                            {
                                db.FormsPredPens_Zapros.Add(PredPensData);
                                db.SaveChanges();
                                cleanData = false;
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Запроса произошла ошибка. Код ошибки: " + ex.InnerException);
                            }

                            break;
                        case "edit":
                            try
                            {
                                // сохраняем модифицированную запись обратно в бд
                                db.Entry(PredPensData).State = EntityState.Modified;
                                db.SaveChanges();
                                cleanData = false;
                                this.Close();
                            }
                            catch (Exception ex)
                            {
                                RadMessageBox.Show("При сохранении данных Запроса произошла ошибка. Код ошибки: " + ex.Message);
                            }
                            break;
                    }
                }


                cleanData = false;
            }
            else
            {
                foreach (var item in errMessBox)
                { Methods.showAlert("Ошибка заполнения", item.name, this.ThemeName, 100); }
            }
        }


    }
}
