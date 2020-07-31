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
using System.Xml.Linq;
using System.Linq.Dynamic;
using PU.FormsRSW2014;

namespace PU.FormsODV1
{
    public partial class ODV1_CreateXML : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        pfrXMLEntities dbxml = new pfrXMLEntities();

        public FormsODV_1_2017 odv1Data { get; set; }
        public byte odv1TypeForm { get; set; }
        public List<long> staffList_temp { get; set; }
        public long currentStaffId = 0;


        public ODV1_CreateXML()
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

        private void ODV1_CreateXML_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            DateUnderwrite.Value = DateTime.Now.Date;


            if (odv1Data != null)
            {
                Code.Text = odv1Data.Code.ToString();
                Year.Text = odv1Data.Year.ToString();
                TypeInfo.Text = odv1Data.TypeInfo.HasValue ? odv1Data.TypeInfo.Value == 1 ? "Исходная" : "Корректирующая" : "";

                if (odv1Data.TypeInfo.HasValue)
                {
                    switch (odv1Data.TypeInfo.Value)
                    {
                        case 0:
                            TypeInfo.Text = "Исходная";
                            break;
                        case 1:
                            TypeInfo.Text = "Корректирующая";
                            break;
                        case 2:
                            TypeInfo.Text = "Отменяющая";
                            break;
                    }
                }
                else
                {
                    TypeInfo.Text = "";
                }


                if (odv1Data.TypeForm.HasValue)
                {

                    odv1TypeForm = odv1Data.TypeForm.Value;
                    switch (odv1Data.TypeForm.Value)
                    {
                        case 1:
                            TypeForm.Text = "СЗВ-СТАЖ";
                            break;
                        case 2:
                            TypeForm.Text = "СЗВ-ИСХ";
                            break;
                        case 3:
                            TypeForm.Text = "СЗВ-КОРР";
                            break;
                        case 4:
                            TypeForm.Text = "ОДВ-1";
                            break;
                    }
                }
                else
                {
                    TypeForm.Text = "";
                }

            }
            string regnum = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID).RegNum;
            if (regnum.Length >= 6)
            {
                codeTOPFRTextBox.Text = regnum.Substring(0, 6);
            }

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    foreach (var item in param.windowData)
                    {
                        int i = 0;
                        switch (item.control)
                        {
                            case "codeTOPFRTextBox":
                                if (item.value != "0")
                                    codeTOPFRTextBox.Text = item.value;
                                break;
                            case "switchStaffListDDL":
                                int.TryParse(item.value, out i);
                                switchStaffListDDL.SelectedIndex = i;
                                break;
                            case "sortingDDL":
                                int.TryParse(item.value, out i);
                                sortingDDL.SelectedIndex = i;
                                break;
                        }
                    }

                }
                catch
                { }
            }

            codeTOPFRTextBox.GotFocus += OnFocus;
            codeTOPFRTextBox.LostFocus += OnDeFocus;
        }

        private void OnFocus(object sender, EventArgs e)
        {
            codeTOPFRTextBox.SelectAll();
        }

        private void OnDeFocus(object sender, EventArgs e)
        {
            codeTOPFRTextBox.SelectionLength = 0;
        }

        private void DateUnderwriteMaskedEditBox_Leave(object sender, EventArgs e)
        {
            if (DateUnderwriteMaskedEditBox.Text != DateUnderwriteMaskedEditBox.NullText)
            {
                DateTime date;
                if (DateTime.TryParse(DateUnderwriteMaskedEditBox.Text, out date))
                {
                    DateUnderwrite.Value = date;
                }
                else
                {
                    DateUnderwrite.Value = DateUnderwrite.NullDate;
                }
            }
            else
            {
                DateUnderwrite.Value = DateUnderwrite.NullDate;
            }
        }

        private void DateUnderwrite_ValueChanged(object sender, EventArgs e)
        {
            if (DateUnderwrite.Value != DateUnderwrite.NullDate)
                DateUnderwriteMaskedEditBox.Text = DateUnderwrite.Value.ToShortDateString();
            else
                DateUnderwriteMaskedEditBox.Text = DateUnderwriteMaskedEditBox.NullText;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void viewPacks_Click(object sender, EventArgs e)
        {
            rsw2014packs child = new rsw2014packs();

            child.ident.Year = odv1Data.Year;
            child.ident.Quarter = odv1Data.Code;
            child.ident.InsurerID = Options.InsID;
            child.ident.FormatType = "odv1";
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        public class staffSZVList
        {
            public long ID { get; set; }
            public Staff staff { get; set; }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (odv1Data == null)
            {
                Methods.showAlert("Внимание!", "Не выбрана форма ОДВ-1!", this.ThemeName);
                return;
            }


            if (String.IsNullOrEmpty(odv1Data.ConfirmDolgn) || odv1Data.ConfirmDolgn.Trim() == "")
            {
                if ((DialogResult)RadMessageBox.Show("В форме ОДВ-1 не заполнена информация в Разделе 5 - Наименование должности! Все равно продолжить?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }

            }

            if (String.IsNullOrEmpty(odv1Data.ConfirmLastName) || odv1Data.ConfirmLastName.Trim() == "")
            {
                if ((DialogResult)RadMessageBox.Show("В форме ОДВ-1 не заполнена информация в Разделе 5 - Фамилия руководителя! Все равно продолжить?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }

            if (String.IsNullOrEmpty(odv1Data.ConfirmFirstName) || odv1Data.ConfirmFirstName.Trim() == "")
            {
                if ((DialogResult)RadMessageBox.Show("В форме ОДВ-1 не заполнена информация в Разделе 5 - Имя руководителя! Все равно продолжить?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    return;
                }
            }

            if ((switchStaffListDDL.SelectedIndex == 2 && (staffList_temp == null || (staffList_temp != null && staffList_temp.Count <= 0))) || (switchStaffListDDL.SelectedIndex == 1 && currentStaffId == 0))
            {
                RadMessageBox.Show("Пустой список сотрудников для формирования! Необходимо выбрать сотрудников!", "Внимание");
                return;
            }


            db = new pu6Entities();

            List<staffSZVList> staffList = new List<staffSZVList>();

            Guid guid = Guid.NewGuid();
            try // Проверяем Guid на уникальность и на целостность промежуточной базы, если база порушена, надо ее пересоздать
            {
                while (dbxml.xmlInfo.Any(x => x.UniqGUID == guid))
                {
                    guid = Guid.NewGuid();
                }
            }
            catch
            {
                string pfrXMLPath = System.IO.Path.Combine(Application.StartupPath, "pfrXML.db3");
                string result = String.Empty;

                System.Threading.Thread.Sleep(500);
                System.IO.File.Delete(pfrXMLPath);
                MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками

                result = methodsNonStatic.createXmlDB(pfrXMLPath);

                if (!String.IsNullOrEmpty(result)) // если все хорошо, база пересоздана, 
                {
                    RadMessageBox.Show("Была обнаружена проблема с промежуточной базой данных для сформированных пачек. Автоматическое восставновление базы завершилось ошибкой - " + result + "\r\nНеобходимо вручную удалить файл pfrXML.db3 из каталога с программой и перезапустить ее!");
                    return;
                }
            }

            dbxml.ExecuteStoreCommand(String.Format("DELETE FROM xmlInfo WHERE ([Year] = {0} AND [Quarter] = {1} AND [InsurerID] = {2} AND [FormatType] = 'odv1')", odv1Data.Year, odv1Data.Code, Options.InsID));




            string sorting = "";

            switch (sortingDDL.SelectedIndex)
            {
                case 0: //сортировка по ФИО
                    sorting = "Staff.LastName, Staff.FirstName, Staff.MiddleName";
                    break;
                case 1: //по страх. номеру
                    sorting = "Staff.InsuranceNumber";
                    break;
                case 2: //по табелю
                    sorting = "Staff.TabelNumber";
                    break;
            }


            try
            {
                this.Cursor = Cursors.WaitCursor;

                string DocType = "ОДВ-1";
                string InfoType = "";

                switch (odv1TypeForm)
                {
                    case 1:

                        List<FormsSZV_STAJ_2017> odv1StaffList = new List<FormsSZV_STAJ_2017>();

                        switch (switchStaffListDDL.SelectedIndex)
                        {
                            case 0: //все записи
                                odv1StaffList = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID).ToList();
                                staffList = odv1StaffList.Select(p => new staffSZVList { ID = p.ID, staff = p.Staff }).ToList().OrderBy(sorting).ToList();
                                break;
                            case 1: //текущая запись
                                odv1StaffList = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID && x.ID == currentStaffId).ToList();
                                staffList = odv1StaffList.Select(p => new staffSZVList { ID = p.ID, staff = p.Staff }).ToList().OrderBy(sorting).ToList();
                                break;
                            case 2: //по выделенным записям
                                odv1StaffList = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID && staffList_temp.Contains(x.ID)).ToList();
                                staffList = odv1StaffList.Select(p => new staffSZVList { ID = p.ID, staff = p.Staff }).ToList().OrderBy(sorting).ToList();
                                break;
                        }

                        DocType = "ОДВ-1 СЗВ-СТАЖ";
                        InfoType = "szv-staj";
                        break;
                    case 2:
                        List<FormsSZV_ISH_2017> odv1StaffList2 = new List<FormsSZV_ISH_2017>();

                        switch (switchStaffListDDL.SelectedIndex)
                        {
                            case 0: //все записи
                                odv1StaffList2 = db.FormsSZV_ISH_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID).ToList();
                                staffList = odv1StaffList2.Select(p => new staffSZVList { ID = p.ID, staff = p.Staff }).ToList().OrderBy(sorting).ToList();
                                break;
                            case 1: //текущая запись
                                odv1StaffList2 = db.FormsSZV_ISH_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID && x.ID == currentStaffId).ToList();
                                staffList = odv1StaffList2.Select(p => new staffSZVList { ID = p.ID, staff = p.Staff }).ToList().OrderBy(sorting).ToList();
                                break;
                            case 2: //по выделенным записям
                                odv1StaffList2 = db.FormsSZV_ISH_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID && staffList_temp.Contains(x.ID)).ToList();
                                staffList = odv1StaffList2.Select(p => new staffSZVList { ID = p.ID, staff = p.Staff }).ToList().OrderBy(sorting).ToList();
                                break;
                        }

                        DocType = "ОДВ-1 СЗВ-ИСХ";
                        InfoType = "szv-ish";
                        break;
                    case 3:
                        List<FormsSZV_KORR_2017> odv1StaffList3 = new List<FormsSZV_KORR_2017>();

                        switch (switchStaffListDDL.SelectedIndex)
                        {
                            case 0: //все записи
                                odv1StaffList3 = db.FormsSZV_KORR_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID).ToList();
                                staffList = odv1StaffList3.Select(p => new staffSZVList { ID = p.ID, staff = p.Staff }).ToList().OrderBy(sorting).ToList();
                                break;
                            case 1: //текущая запись
                                odv1StaffList3 = db.FormsSZV_KORR_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID && x.ID == currentStaffId).ToList();
                                staffList = odv1StaffList3.Select(p => new staffSZVList { ID = p.ID, staff = p.Staff }).ToList().OrderBy(sorting).ToList();
                                break;
                            case 2: //по выделенным записям
                                odv1StaffList3 = db.FormsSZV_KORR_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID && staffList_temp.Contains(x.ID)).ToList();
                                staffList = odv1StaffList3.Select(p => new staffSZVList { ID = p.ID, staff = p.Staff }).ToList().OrderBy(sorting).ToList();
                                break;
                        }

                        DocType = "ОДВ-1 СЗВ-КОРР";
                        InfoType = "szv-korr";
                        break;
                }


                string regNum = Utils.ParseRegNum(db.Insurer.First(x => x.ID == Options.InsID).RegNum);


                long cnt = staffList.Count();

                pfrXMLEntities dbxmlTemp = new pfrXMLEntities();

                string fileName = String.Format("ПФР_{0}_{1}_{2}_{3}_{4}.XML", regNum, codeTOPFRTextBox.Text, TypeForm.Text, DateTime.Now.ToString("yyyyMMdd"), guid);


                xmlInfo xml_info = new xmlInfo
                {
                    Num = 1,
                    CountDoc = 1,
                    CountStaff = cnt,
                    DocType = DocType,
                    Year = odv1Data.Year,
                    Quarter = odv1Data.Code,
                    DateCreate = DateUnderwrite.Value,
                    FileName = fileName,
                    UniqGUID = guid,
                    InsurerID = Options.InsID,
                    FormatType = "odv1",
                    SourceID = odv1Data.ID,

                };

                int k = 0;
                List<StaffList> staffList_ = new List<StaffList>();

                foreach (var szvItem in staffList)
                {
                    k++;
                    var staffItem = szvItem.staff;

                    string contrNum = "";
                    string InsuranceNumber = staffItem.InsuranceNumber;
                    if (staffItem.ControlNumber != null)
                    {
                        contrNum = staffItem.ControlNumber.HasValue ? staffItem.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                        InsuranceNumber = !String.IsNullOrEmpty(staffItem.InsuranceNumber) ? staffItem.InsuranceNumber.Substring(0, 3) + "-" + staffItem.InsuranceNumber.Substring(3, 3) + "-" + staffItem.InsuranceNumber.Substring(6, 3) + " " + contrNum : "";

                    }

                    StaffList staffListNewItem = new StaffList
                    {
                        Num = k,
                        FIO = staffItem.LastName + " " + staffItem.FirstName + " " + staffItem.MiddleName,
                        StaffID = staffItem.ID,
                        InsuranceNum = InsuranceNumber,
                        InfoType = InfoType,
                        DateCreate = DateUnderwrite.Value,
                        XmlInfoID = xml_info.ID,
                        FormsRSW_6_1_ID = szvItem.ID
                    };

                    staffList_.Add(staffListNewItem);


                }


                foreach (var item in staffList_)
                {
                    xml_info.StaffList.Add(item);
                }
                dbxmlTemp.AddToxmlInfo(xml_info);
                dbxmlTemp.SaveChanges();

                dbxmlTemp.Dispose();


            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Ошибка при подготовке к формированию файла XML Формы ОДВ-1. Код ошибки: " + ex.Message, "Внимание!");
                this.Cursor = Cursors.Default;
                db.Dispose();
                return;

            }
            finally
            {
                this.Cursor = Cursors.Default;
                db.Dispose();

                if (dbxml.xmlInfo.Any(x => x.UniqGUID == guid))
                {
                    Methods.showAlert("Информация", "Формирование пачек ОДВ-1 успешно завершено.", this.ThemeName);
                    viewPacks_Click(null, null);
                }
            }

        }

        private void ODV1_CreateXML_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками
            List<WindowData> windowData = new List<WindowData> { };


            windowData.Add(new WindowData
            {
                control = "codeTOPFRTextBox",
                value = codeTOPFRTextBox.Text
            });
            windowData.Add(new WindowData
            {
                control = "switchStaffListDDL",
                value = switchStaffListDDL.SelectedIndex.ToString()
            });
            windowData.Add(new WindowData
            {
                control = "sortingDDL",
                value = sortingDDL.SelectedIndex.ToString()
            });

            props.setFormParams(this, windowData);
        }

        private void codeTOPFRTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }



    }
}
