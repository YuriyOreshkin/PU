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
using Telerik.WinControls.UI;

namespace PU.FormsSZV_TD
{
    public partial class SZV_TD_EditStaff_Event : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string action { get; set; }
        private bool cleanData = true;
        public FormsSZV_TD_2020_Staff_Events formData { get; set; }
        public Staff staff;
        public bool? firstRecordIsAnnuled;
        private Color bc;

        public SZV_TD_EditStaff_Event()
        {
            InitializeComponent();
            Dolgn_textBox.TextBoxElement.ToolTipText = "Должность, специальность, профессия, квалификация. Обязательно указывается для записей вида: ПРИЕМ, УСТАНОВЛЕНИЕ (ПРИСВОЕНИЕ)";
            VydPoruchRaboty_textBox.TextBoxElement.ToolTipText = "Конкретный вид поручаемой работы";
            Svedenia_textBox.TextBoxElement.ToolTipText = "Иные сведения о мероприятии";
            Department_textBox.TextBoxElement.ToolTipText = "Структурное подразделение, в которое принят сотрудник";
            KodVypFunc_textBox.TextBoxElement.ToolTipText = "Код выполняемой функции (при наличии)";

            Prichina_textBox.TextBoxElement.ToolTipText = "Причина увольнения";
            Statya_textBox.TextBoxElement.ToolTipText = "Номер статьи Трудового кодекса";
            Punkt_textBox.TextBoxElement.ToolTipText = "Пункт статьи Трудового кодекса";

            OsnUvolName_textBox.TextBoxElement.ToolTipText = "Наименование нормативного документа";
            OsnUvolStartya_textBox.TextBoxElement.ToolTipText = "Номер статьи Федерального закона";
            OsnUvolChyast_textBox.TextBoxElement.ToolTipText = "Номер части статьи Федерального закона";
            OsnUvolPunkt_textBox.TextBoxElement.ToolTipText = "Номер пункта из нормативного документа";
            OsnUvolPodPunkt_textBox.TextBoxElement.ToolTipText = "Номер подпункта из нормативного документа";

            OsnName1.TextBoxElement.ToolTipText = "Наименование документа-основания";
            OsnNum1.TextBoxElement.ToolTipText = "Номер документа";
            OsnSer1.TextBoxElement.ToolTipText = "Серия документа";
            OsnDate1.DateTimePickerElement.ToolTipText = "Дата документа \"ОТ\"";

            OsnName2.TextBoxElement.ToolTipText = "Наименование документа-основания";
            OsnNum2.TextBoxElement.ToolTipText = "Номер документа";
            OsnSer2.TextBoxElement.ToolTipText = "Серия документа";
            OsnDate2.DateTimePickerElement.ToolTipText = "Дата документа \"ОТ\"";

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

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SZV_TD_EditStaff_Event_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            DateOfEvent.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime;
            DateOfEvent.Value = DateOfEvent.NullDate;
            DateOfEvent.Enter += (s, a) => { DateOfEvent.Select(); };


            DateFrom.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime;
            DateFrom.Value = DateFrom.NullDate;
            DateFrom.Enter += (s, a) => { DateFrom.Select(); };

            DateTo.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime;
            DateTo.Value = DateTo.NullDate;
            DateTo.Enter += (s, a) => { DateTo.Select(); };

            OsnDate1.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime;
            OsnDate1.Value = OsnDate1.NullDate;
            OsnDate1.Enter += (s, a) => { OsnDate1.Select(); };

            OsnDate2.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime;
            OsnDate2.Value = OsnDate2.NullDate;
            OsnDate2.Enter += (s, a) => { OsnDate2.Select(); };

            AnnuleDate.DateTimePickerElement.TextBoxElement.MaskType = MaskType.FreeFormDateTime;
            AnnuleDate.Value = AnnuleDate.NullDate;
            AnnuleDate.Enter += (s, a) => { AnnuleDate.Select(); };


            foreach (var item in db.FormsSZV_TD_2020_TypesOfEvents)
            {
                TypesOfEvents_DDL.Items.Add(new RadListDataItem(item.Name.ToString(), item.ID.ToString()));
            }

            bc = uuidTextBox.BackColor;

            uuidTextBox_ReadOnlyChanged(null, null);

            switch (action)
            {
                case "add":
                    if (formData == null)
                        formData = new FormsSZV_TD_2020_Staff_Events();

                    generateNewUUID();

                    uuidGenNewBtn.Visible = true;

                    if (staff != null)
                    {
                        try
                        {
                            if (staff.Department != null)
                            {
                                Department_textBox.Text = staff.Department.Name;
                            }
                            if (staff.DolgnID.HasValue)
                            {
                                var dolgn = db.Dolgn.FirstOrDefault(x => x.ID == staff.DolgnID);
                                if (dolgn != null)
                                {
                                    Dolgn_textBox.Text = dolgn.Name;
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При попытке заполнить поля 'Структурное подразделение' и 'Должность' произошла ошибка! Код ошибки - " + ex.Message);
                        }

                    }

                    DateOfEvent.Value = DateTime.Now;

                    if (firstRecordIsAnnuled.HasValue && firstRecordIsAnnuled.Value)
                    {
                        Annuled_CheckBox.Checked = true;
                    }

                    break;
                case "edit":

                    try
                    {
                        if (formData.FormsSZV_TD_2020_TypesOfEvents != null && TypesOfEvents_DDL.Items.Any(x => x.Value.ToString() == formData.TypesOfEvents_ID.ToString()))
                        {
                            TypesOfEvents_DDL.Items.Single(x => x.Value.ToString() == formData.TypesOfEvents_ID.ToString()).Selected = true;
                        }

                        uuidTextBox.Text = formData.UUID.ToString();
                        uuidGenNewBtn.Visible = false;

                        DateOfEvent.Value = formData.DateOfEvent;
                        Sovmestitel_CheckBox.Checked = formData.Sovmestitel == 1;
                        Dolgn_textBox.Text = formData.Dolgn;
                        VydPoruchRaboty_textBox.Text = formData.VydPoruchRaboty;
                        Svedenia_textBox.Text = formData.Svedenia;
                        Department_textBox.Text = formData.Department;

                        if (formData.DateFrom.HasValue)
                            DateFrom.Value = formData.DateFrom.Value;
                        if (formData.DateTo.HasValue)
                            DateTo.Value = formData.DateTo.Value;

                        KodVypFunc_textBox.Text = formData.KodVypFunc;


                        Prichina_textBox.Text = formData.Prichina;

                        if (!String.IsNullOrEmpty(formData.Statya) || !String.IsNullOrEmpty(formData.Punkt))
                        {
                            Uvolnen_RadioButton1.IsChecked = true;
                            Statya_textBox.Text = formData.Statya;
                            Punkt_textBox.Text = formData.Punkt;
                        }
                        else if (!String.IsNullOrEmpty(formData.OsnUvolName) || !String.IsNullOrEmpty(formData.OsnUvolStartya) || !String.IsNullOrEmpty(formData.OsnUvolChyast) || !String.IsNullOrEmpty(formData.OsnUvolPunkt) || !String.IsNullOrEmpty(formData.OsnUvolPodPunkt))
                        {
                            Uvolnen_RadioButton2.IsChecked = true;
                            OsnUvolName_textBox.Text = formData.OsnUvolName;
                            OsnUvolStartya_textBox.Text = formData.OsnUvolStartya;
                            OsnUvolChyast_textBox.Text = formData.OsnUvolChyast;
                            OsnUvolPunkt_textBox.Text = formData.OsnUvolPunkt;
                            OsnUvolPodPunkt_textBox.Text = formData.OsnUvolPodPunkt;
                        }

                        OsnName1.Text = formData.OsnName1;
                        OsnNum1.Text = formData.OsnNum1;
                        OsnSer1.Text = formData.OsnSer1;
                        if (formData.OsnDate1.HasValue)
                            OsnDate1.Value = formData.OsnDate1.Value;

                        OsnName2.Text = formData.OsnName2;
                        OsnNum2.Text = formData.OsnNum2;
                        OsnSer2.Text = formData.OsnSer2;
                        if (formData.OsnDate2.HasValue)
                            OsnDate2.Value = formData.OsnDate2.Value;


                        Annuled_CheckBox.Checked = formData.Annuled.HasValue ? formData.Annuled.Value : false;
                        if (formData.AnnuleDate.HasValue)
                            AnnuleDate.Value = formData.AnnuleDate.Value;
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("Не удалось загрузить данные формы. Код ошибки - " + ex.Message);
                    }


                    break;
            }


            Annuled_CheckBox_ToggleStateChanged(null, null);
        }

        private void generateNewUUID()
        {
            formData.UUID = Guid.NewGuid();
            uuidTextBox.Text = formData.UUID.ToString();
        }

        private void Uvolnen_RadioButton1_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            Statya_textBox.Enabled = Punkt_textBox.Enabled = Uvolnen_RadioButton1.IsChecked;
        }

        private void Uvolnen_RadioButton2_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            OsnUvolName_textBox.Enabled = OsnUvolStartya_textBox.Enabled = OsnUvolChyast_textBox.Enabled = OsnUvolPunkt_textBox.Enabled = OsnUvolPodPunkt_textBox.Enabled = Uvolnen_RadioButton2.IsChecked;
        }



        private void Annuled_CheckBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            if (uuidBtn.Visible = AnnuleDate.Enabled = Annuled_CheckBox.Checked)
            {

            }
            else
            {
                uuidTextBox.ReadOnly = true;
            }
        }

        private void SZV_TD_EditStaff_Event_FormClosing(object sender, FormClosingEventArgs e)
        {
            db = null;
            if (cleanData)
                formData = null;
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            var errList = validation();

            if (errList.Count > 0)  // если есть ошибки заполнения, то выводим список
            {
                string err = "В заполнении формы присутствуют ошибки! Необходимо их исправить!\r\nСписок ошибок:\r\r";
                int i = 1;
                foreach (var item in errList)
                {
                    err += "\r\n" + i.ToString() + ". " + item;
                    i++;
                }

                RadMessageBox.Show(err);
            }
            else // если не ошибок, то сохраняем
            {
                try
                {
                    getValues();
                    cleanData = false;
                    this.Close();
                }
                catch (Exception ex)
                {
                    RadMessageBox.Show("При сохранении данных произовшла ошибка! Код ошибки - " + ex.Message);
                }
            }
        }

        private List<string> validation()
        {
            List<string> errList = new List<string>();

            Guid guidValid;
            if (!Guid.TryParse(uuidTextBox.Text, out guidValid))
            {
                errList.Add("Ошибка заполнения поля \"UUID мероприятия\"!");
            }

            if (DateOfEvent.Value == DateOfEvent.NullDate)
            {
                errList.Add("Необходимо указать \"Дату мероприятия\"!");
            }
            
            if (TypesOfEvents_DDL.SelectedItem == null)
            {
                errList.Add("Необходимо выбрать \"Тип сведений\"!");
            }

            if (Annuled_CheckBox.Checked && AnnuleDate.Value == AnnuleDate.NullDate)
            {
                errList.Add("Для отменяющего мероприятия Поле \"Дата отмены\" должно быть обязательно заполнено!");
            }

            if (OsnName1.Text.Trim() == "")
            {
                errList.Add("Необходимо заполнить \"Наименование\" документа-основания для мероприятия!");
            }

            if (OsnDate1.Value == OsnDate1.NullDate)
            {
                errList.Add("Необходимо указать \"Дату\" документа-основания для мероприятия!");
            }

            if (OsnNum1.Text.Trim() == "")
            {
                errList.Add("Необходимо указать \"Номер\" документа-основания для мероприятия!");
            }


            return errList;
        }


        private void getValues()
        {
            string eventID = TypesOfEvents_DDL.SelectedItem.Value.ToString();

            formData.TypesOfEvents_ID = long.Parse(eventID);
            formData.UUID = Guid.Parse(uuidTextBox.Text);

            formData.FormsSZV_TD_2020_TypesOfEvents = new FormsSZV_TD_2020_TypesOfEvents { ID = formData.TypesOfEvents_ID, Name = TypesOfEvents_DDL.SelectedItem.Text.ToString() };

            if (DateOfEvent.Value != DateOfEvent.NullDate)
                formData.DateOfEvent = DateOfEvent.Value;

            formData.Sovmestitel = Sovmestitel_CheckBox.Checked ? (byte)1 : (byte)0;
            formData.Dolgn = Dolgn_textBox.Text;
            formData.VydPoruchRaboty = VydPoruchRaboty_textBox.Text;
            formData.Svedenia = Svedenia_textBox.Text;
            formData.Department = Department_textBox.Text;


            if (DateFrom.Value != DateFrom.NullDate && formData.TypesOfEvents_ID == 6)
            {
                formData.DateFrom = DateFrom.Value;
            }
            else
            {
                formData.DateFrom = null;
            }

            if (DateTo.Value != DateTo.NullDate && formData.TypesOfEvents_ID == 6)
            {
                formData.DateTo = DateTo.Value;
            }
            else
            {
                formData.DateTo = null;
            }



            formData.KodVypFunc = KodVypFunc_textBox.Text;

            formData.Prichina = Prichina_textBox.Text;

            if (Uvolnen_RadioButton1.IsChecked)
            {
                formData.Statya = Statya_textBox.Text;
                formData.Punkt = Punkt_textBox.Text;
                formData.OsnUvolName = "";
                formData.OsnUvolStartya = "";
                formData.OsnUvolChyast = "";
                formData.OsnUvolPunkt = "";
                formData.OsnUvolPodPunkt = "";

            }
            else if (Uvolnen_RadioButton2.IsChecked)
            {
                formData.Statya = "";
                formData.Punkt = "";
                formData.OsnUvolName = OsnUvolName_textBox.Text;
                formData.OsnUvolStartya = OsnUvolStartya_textBox.Text;
                formData.OsnUvolChyast = OsnUvolChyast_textBox.Text;
                formData.OsnUvolPunkt = OsnUvolPunkt_textBox.Text;
                formData.OsnUvolPodPunkt = OsnUvolPodPunkt_textBox.Text;

            }

            formData.OsnName1 = OsnName1.Text;
            formData.OsnNum1 = OsnNum1.Text;
            formData.OsnSer1 = OsnSer1.Text;
            if (OsnDate1.Value != OsnDate1.NullDate)
            {
                formData.OsnDate1 = OsnDate1.Value;
            }
            else
            {
                formData.OsnDate1 = null;
            }

            formData.OsnName2 = OsnName2.Text;
            formData.OsnNum2 = OsnNum2.Text;
            formData.OsnSer2 = OsnSer2.Text;
            if (OsnDate2.Value != OsnDate2.NullDate)
            {
                formData.OsnDate2 = OsnDate2.Value;
            }
            else
            {
                formData.OsnDate2 = null;
            }



            formData.Annuled = Annuled_CheckBox.Checked;
            if (AnnuleDate.Value != AnnuleDate.NullDate)
            {
                formData.AnnuleDate = AnnuleDate.Value;
            }
            else
            {
                formData.AnnuleDate = null;
            }


        }

        private void uuidBtn_Click(object sender, EventArgs e)
        {
            uuidTextBox.ReadOnly = !uuidTextBox.ReadOnly;

            uuidBtn.Text = uuidTextBox.ReadOnly ? "Ручной ввод: ВКЛ" : "Ручной ввод: ОТКЛ";
        }

        private void uuidGenNewBtn_Click(object sender, EventArgs e)
        {
            generateNewUUID();
        }

        private void uuidTextBox_ReadOnlyChanged(object sender, EventArgs e)
        {
            uuidTextBox.BackColor = uuidTextBox.ReadOnly ? Color.WhiteSmoke : bc;
        }

        private void TypesOfEvents_DDL_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
             DateFrom.Enabled = DateTo.Enabled = TypesOfEvents_DDL.SelectedItem.Value.ToString() == "6";
        }


    }
}
