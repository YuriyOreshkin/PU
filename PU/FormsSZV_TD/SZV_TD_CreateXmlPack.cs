using PU.Classes;
using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Telerik.WinControls;
using System.Xml.Linq;
using System.IO;
using System.Xml;

namespace PU.FormsSZV_TD
{
    public partial class SZV_TD_CreateXmlPack : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public long szvtdID = 0;



        public SZV_TD_CreateXmlPack()
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SZV_TD_CreateXmlPack_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            pathBrowser.Value = Options.CurrentInsurerFolders.exportPath;

            //string regnum = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID).RegNum;
            //if (regnum.Length >= 6)
            //{
            //    codeTOPFRTextBox.Text = regnum.Substring(0, 6);
            //}

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    foreach (var item in param.windowData)
                    {
                        switch (item.control)
                        {
                            case "remainChkbox":
                                remainChkbox.Checked = item.value == "true" ? true : false;
                                break;
                            case "pathBrowser":
                                if (remainChkbox.Checked)
                                    pathBrowser.Value = item.value;
                                break;
                            case "codeTOPFRTextBox":
                                if (item.value != "0")
                                    codeTOPFRTextBox.Text = item.value;
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

        private void codeTOPFRTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }



        private void SZV_TD_CreateXmlPack_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками
            List<WindowData> windowData = new List<WindowData> { };


            windowData.Add(new WindowData
            {
                control = "remainChkbox",
                value = remainChkbox.Checked ? "true" : "false"
            });
            windowData.Add(new WindowData
            {
                control = "pathBrowser",
                value = pathBrowser.Value
            });
            windowData.Add(new WindowData
            {
                control = "codeTOPFRTextBox",
                value = codeTOPFRTextBox.Text
            });

            props.setFormParams(this, windowData);
        }
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pathBrowser.Value != null)
            {
                if (db.FormsSZV_TD_2020.Any(x => x.ID == szvtdID))
                {
                    if (db.FormsSZV_TD_2020_Staff.Any(x => x.FormsSZV_TD_2020_ID == szvtdID && !x.Staff.DateBirth.HasValue))
                    {
                        if (RadMessageBox.Show(this, "В Форме СЗВ-ТД обнаружены сотрудники у которых не заполнена ДАТА РОЖДЕНИЯ! \r\nДанное поле является обязательным для заполнения!\r\nВсе равно продолжить?", "Внимание!", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            return;
                        }

                    }


                        pu6Entities db_temp = new pu6Entities();

                        try
                        {
                            FormsSZV_TD_2020 SZV_TD = db_temp.FormsSZV_TD_2020.First(x => x.ID == szvtdID);

                            string regNum = Utils.ParseRegNum(SZV_TD.Insurer.RegNum);
                            Guid guid = Guid.NewGuid();

                            string fileName = String.Format("ПФР_{0}_СЗВ-ТД_{1}_{2}.XML", regNum, DateTime.Now.ToString("yyyyMMdd"), guid);

                            XNamespace pfr = "http://пф.рф/СЗВ-ТД/2019-12-20";
                            XNamespace УТ2 = "http://пф.рф/УТ/2017-08-21";
                            XNamespace АФ5 = "http://пф.рф/АФ/2018-12-07";
                            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                            string schLoc = @"http://пф.рф/СЗВ-ТД/2019-12-20 ../../Схемы/ЭТК/СЗВ-ТД_2019-12-20.xsd";

                            XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", null),
                                                 new XElement(pfr + "ЭДПФР",
                                                     new XAttribute(XNamespace.Xmlns + "УТ2", УТ2.NamespaceName),
                                                     new XAttribute(XNamespace.Xmlns + "АФ5", АФ5.NamespaceName),
                                                     new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                                                     new XAttribute(xsi + "schemaLocation", schLoc)));

                            //       xDoc.Element(pfr + "ЭДПФР");

                            Insurer ins = SZV_TD.Insurer;

                            string orgName = "";

                            if (ins.TypePayer == 0) // если организация
                            {
                                if (!String.IsNullOrEmpty(ins.NameShort))
                                {
                                    orgName = ins.NameShort;
                                }
                                else if (!String.IsNullOrEmpty(ins.Name))
                                {
                                    orgName = ins.Name;
                                }

                            }
                            else // если физ. лицо
                            {
                                orgName = ins.LastName + " " + ins.FirstName + " " + ins.MiddleName;
                            }

                            if (!String.IsNullOrEmpty(orgName) && orgName.Length > 255)
                            {
                                orgName = orgName.Substring(0, 255);
                            }

                            XElement Работодатель = new XElement(pfr + "Работодатель",
                                                        new XElement(УТ2 + "РегНомер", regNum),
                                                        new XElement(pfr + "НаименованиеОрганизации", orgName.Trim()),
                                                        new XElement(УТ2 + "ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : ""));

                            if (!String.IsNullOrEmpty(ins.KPP))
                            {
                                Работодатель.Add(new XElement(УТ2 + "КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
                            }

                            XElement СЗВТД = new XElement(pfr + "СЗВ-ТД", Работодатель);

                            XElement ОтчетныйПериод = new XElement(pfr + "ОтчетныйПериод",
                                  new XElement(pfr + "Месяц", SZV_TD.Month),
                                  new XElement(pfr + "КалендарныйГод", SZV_TD.Year));

                            СЗВТД.Add(ОтчетныйПериод);

                            var szvtd_staff = SZV_TD.FormsSZV_TD_2020_Staff.OrderBy(x => x.Staff.LastName).ThenBy(x => x.Staff.FirstName).ThenBy(x => x.Staff.MiddleName).ToList();

                            foreach (var staffitem in szvtd_staff)  // перебираем всех людей в форме
                            {
                                var staff = staffitem.Staff;

                                XElement ЗЛ = new XElement(pfr + "ЗЛ");

                                XElement ФИО = new XElement(УТ2 + "ФИО");
                                if (!String.IsNullOrEmpty(staff.LastName))
                                {
                                    ФИО.Add(new XElement(УТ2 + "Фамилия", staff.LastName.Trim()));
                                }
                                if (!String.IsNullOrEmpty(staff.FirstName))
                                {
                                    ФИО.Add(new XElement(УТ2 + "Имя", staff.FirstName.Trim()));
                                }
                                if (!String.IsNullOrEmpty(staff.MiddleName))
                                {
                                    ФИО.Add(new XElement(УТ2 + "Отчество", staff.MiddleName.Trim()));
                                }
                                ЗЛ.Add(ФИО);

                                if (staff.DateBirth.HasValue)
                                {
                                    ЗЛ.Add(new XElement(pfr + "ДатаРождения", staff.DateBirth.Value.ToString("yyyy-MM-dd")));
                                }



                                var snils = Utils.ParseSNILS(staff.InsuranceNumber, staff.ControlNumber);

                                ЗЛ.Add(new XElement(УТ2 + "СНИЛС", snils));


                                XElement Заявления = new XElement(pfr + "Заявления");
                                if (staffitem.ZayavOProdoljDate.HasValue || (staffitem.ZayavOProdoljState.HasValue && staffitem.ZayavOProdoljState.Value != 0))  // если есть дата у заявления о продолжении, то пишем данные
                                {
                                    XElement ЗаявлениеОПродолжении = new XElement(pfr + "ЗаявлениеОПродолжении");

                                    if (staffitem.ZayavOProdoljDate.HasValue)
                                    {
                                        ЗаявлениеОПродолжении.Add(new XElement(pfr + "Дата", staffitem.ZayavOProdoljDate.Value.ToString("yyyy-MM-dd")));
                                        ЗаявлениеОПродолжении.Add(new XElement(pfr + "СтатусЗаявленияОПродолжении", staffitem.ZayavOProdoljState.HasValue ? (staffitem.ZayavOProdoljState.Value != 0 ? staffitem.ZayavOProdoljState.Value : 1) : 1));
                                    }

                                    Заявления.Add(ЗаявлениеОПродолжении);
                                }

                                if (staffitem.ZayavOPredostDate.HasValue || (staffitem.ZayavOPredostState.HasValue && staffitem.ZayavOPredostState.Value != 0))  // если есть дата у заявления о предоставлении, то пишем данные
                                {
                                    XElement ЗаявлениеОПредоставлении = new XElement(pfr + "ЗаявлениеОПредоставлении");

                                    if (staffitem.ZayavOPredostDate.HasValue)
                                    {
                                        ЗаявлениеОПредоставлении.Add(new XElement(pfr + "Дата", staffitem.ZayavOPredostDate.Value.ToString("yyyy-MM-dd")));
                                        ЗаявлениеОПредоставлении.Add(new XElement(pfr + "СтатусЗаявленияОПредоставлении", staffitem.ZayavOPredostState.HasValue ? (staffitem.ZayavOPredostState.Value != 0 ? staffitem.ZayavOPredostState.Value : 1) : 1));
                                    }

                                    Заявления.Add(ЗаявлениеОПредоставлении);
                                }

                                if (Заявления.Elements().Count() > 0)
                                {
                                    ЗЛ.Add(Заявления);
                                }


                                if (staffitem.FormsSZV_TD_2020_Staff_Events.Count() > 0)
                                {
                                    XElement ТрудоваяДеятельность = new XElement(pfr + "ТрудоваяДеятельность");
                                    var eventList = staffitem.FormsSZV_TD_2020_Staff_Events.OrderBy(x => x.DateOfEvent).ToList();
                                    foreach (var event_ in eventList)
                                    {
                                        if (event_.Annuled.HasValue && event_.Annuled.Value == true)  // Отменяемое мероприятие
                                        {
                                            XElement МероприятиеОтменяемое = new XElement(pfr + "МероприятиеОтменяемое",
                                                                                  new XElement(pfr + "UUID", event_.UUID),
                                                                                  new XElement(pfr + "ДатаМероприятия", event_.DateOfEvent.ToString("yyyy-MM-dd")),
                                                                                  new XElement(pfr + "ДатаОтмены", event_.AnnuleDate.HasValue ? event_.AnnuleDate.Value.ToString("yyyy-MM-dd") : ""),
                                                                                  new XElement(pfr + "Вид", event_.FormsSZV_TD_2020_TypesOfEvents.Code),
                                                                                  new XElement(pfr + "ЯвляетсяСовместителем", event_.Sovmestitel));



                                            ТрудоваяДеятельность.Add(МероприятиеОтменяемое);
                                        }
                                        else // исходное мероприятие
                                        {
                                            XElement Мероприятие = new XElement(pfr + "Мероприятие",
                                            new XElement(pfr + "UUID", event_.UUID),
                                            new XElement(pfr + "Дата", event_.DateOfEvent.ToString("yyyy-MM-dd")),
                                            new XElement(pfr + "Вид", event_.FormsSZV_TD_2020_TypesOfEvents.Code));


                                            if (!String.IsNullOrEmpty(event_.Svedenia))
                                            {
                                                Мероприятие.Add(new XElement(pfr + "Сведения", event_.Svedenia.Trim()));
                                            }
                                            if (!String.IsNullOrEmpty(event_.Dolgn))
                                            {
                                                Мероприятие.Add(new XElement(pfr + "Должность", event_.Dolgn.Trim()));
                                            }

                                            Мероприятие.Add(new XElement(pfr + "ЯвляетсяСовместителем", event_.Sovmestitel));

                                            if (!String.IsNullOrEmpty(event_.Department))
                                            {
                                                Мероприятие.Add(new XElement(pfr + "СтруктурноеПодразделение", event_.Department.Trim()));
                                            }

                                            if (!String.IsNullOrEmpty(event_.VydPoruchRaboty))
                                            {
                                                Мероприятие.Add(new XElement(pfr + "ВидПР", event_.VydPoruchRaboty.Trim()));
                                            }

                                            if (!String.IsNullOrEmpty(event_.KodVypFunc))
                                            {
                                                Мероприятие.Add(new XElement(pfr + "КодВФ", event_.KodVypFunc.Trim()));
                                            }

                                            if (!String.IsNullOrEmpty(event_.Statya) || !String.IsNullOrEmpty(event_.Punkt))
                                            {
                                                if (!String.IsNullOrEmpty(event_.Statya))
                                                {
                                                    Мероприятие.Add(new XElement(pfr + "Статья", event_.Statya.Trim()));
                                                }

                                                if (!String.IsNullOrEmpty(event_.Punkt))
                                                {
                                                    Мероприятие.Add(new XElement(pfr + "Пункт", event_.Punkt.Trim()));
                                                }
                                            }
                                            else if (!String.IsNullOrEmpty(event_.OsnUvolName) || !String.IsNullOrEmpty(event_.OsnUvolStartya) || !String.IsNullOrEmpty(event_.OsnUvolChyast) || !String.IsNullOrEmpty(event_.OsnUvolPunkt) || !String.IsNullOrEmpty(event_.OsnUvolPodPunkt))
                                            {
                                                XElement ОснованиеУвольнения = new XElement(pfr + "ОснованиеУвольнения");

                                                if (!String.IsNullOrEmpty(event_.OsnUvolName))
                                                {
                                                    ОснованиеУвольнения.Add(new XElement(УТ2 + "НормативныйДокумент", event_.OsnUvolName.Trim()));
                                                }
                                                if (!String.IsNullOrEmpty(event_.OsnUvolStartya))
                                                {
                                                    ОснованиеУвольнения.Add(new XElement(УТ2 + "Статья", event_.OsnUvolStartya.Trim()));
                                                }
                                                if (!String.IsNullOrEmpty(event_.OsnUvolChyast))
                                                {
                                                    ОснованиеУвольнения.Add(new XElement(УТ2 + "Часть", event_.OsnUvolChyast.Trim()));
                                                }
                                                if (!String.IsNullOrEmpty(event_.OsnUvolPunkt))
                                                {
                                                    ОснованиеУвольнения.Add(new XElement(УТ2 + "Пункт", event_.OsnUvolPunkt.Trim()));
                                                }
                                                if (!String.IsNullOrEmpty(event_.OsnUvolPodPunkt))
                                                {
                                                    ОснованиеУвольнения.Add(new XElement(УТ2 + "Подпункт", event_.OsnUvolPodPunkt.Trim()));
                                                }


                                                Мероприятие.Add(ОснованиеУвольнения);
                                            }


                                            if (!String.IsNullOrEmpty(event_.Prichina))
                                            {
                                                Мероприятие.Add(new XElement(pfr + "Причина", event_.Prichina.Trim()));
                                            }


                                            if (event_.DateFrom.HasValue)
                                            {
                                                Мероприятие.Add(new XElement(pfr + "ДатаС", event_.DateFrom.Value.ToString("yyyy-MM-dd")));
                                            }
                                            if (event_.DateTo.HasValue)
                                            {
                                                Мероприятие.Add(new XElement(pfr + "ДатаПо", event_.DateTo.Value.ToString("yyyy-MM-dd")));
                                            }

                                            if (!String.IsNullOrEmpty(event_.OsnName1))
                                            {
                                                XElement Основание = new XElement(pfr + "Основание",
                                                                         new XElement(pfr + "Наименование", event_.OsnName1.Trim()),
                                                                         new XElement(pfr + "Дата", event_.OsnDate1.HasValue ? event_.OsnDate1.Value.ToString("yyyy-MM-dd") : ""),
                                                                         new XElement(pfr + "Номер", !String.IsNullOrEmpty(event_.OsnNum1) ? event_.OsnNum1.Trim() : ""));

                                                if (!String.IsNullOrEmpty(event_.OsnSer1))
                                                {
                                                    Основание.Add(new XElement(pfr + "Серия", event_.OsnSer1.Trim()));
                                                }

                                                Мероприятие.Add(Основание);
                                            }

                                            if (!String.IsNullOrEmpty(event_.OsnName2))
                                            {
                                                XElement Основание = new XElement(pfr + "Основание",
                                                                         new XElement(pfr + "Наименование", event_.OsnName2.Trim()),
                                                                         new XElement(pfr + "Дата", event_.OsnDate2.HasValue ? event_.OsnDate2.Value.ToString("yyyy-MM-dd") : ""),
                                                                         new XElement(pfr + "Номер", !String.IsNullOrEmpty(event_.OsnNum2) ? event_.OsnNum2.Trim() : ""));

                                                if (!String.IsNullOrEmpty(event_.OsnSer2))
                                                {
                                                    Основание.Add(new XElement(pfr + "Серия", event_.OsnSer2.Trim()));
                                                }

                                                Мероприятие.Add(Основание);
                                            }




                                            ТрудоваяДеятельность.Add(Мероприятие);
                                        }

                                    }


                                    ЗЛ.Add(ТрудоваяДеятельность);
                                }


                                СЗВТД.Add(ЗЛ);
                            }







                            СЗВТД.Add(new XElement(pfr + "ДатаЗаполнения", SZV_TD.DateFilling.ToString("yyyy-MM-dd")));

                            XElement Руководитель = new XElement(pfr + "Руководитель");
                            bool flag = false;

                            if ((!String.IsNullOrEmpty(SZV_TD.ConfirmLastName) && SZV_TD.ConfirmLastName.Trim() != "") || (!String.IsNullOrEmpty(SZV_TD.ConfirmFirstName) && SZV_TD.ConfirmFirstName.Trim() != "") || (!String.IsNullOrEmpty(SZV_TD.ConfirmMiddleName) && SZV_TD.ConfirmMiddleName.Trim() != ""))
                            {
                                XElement ФИО = new XElement(УТ2 + "ФИО");
                                if (!String.IsNullOrEmpty(SZV_TD.ConfirmLastName) && SZV_TD.ConfirmLastName.Trim() != "")
                                {
                                    ФИО.Add(new XElement(УТ2 + "Фамилия", SZV_TD.ConfirmLastName.Trim()));
                                    flag = true;
                                }
                                if (!String.IsNullOrEmpty(SZV_TD.ConfirmFirstName) && SZV_TD.ConfirmFirstName.Trim() != "")
                                {
                                    ФИО.Add(new XElement(УТ2 + "Имя", SZV_TD.ConfirmFirstName.Trim()));
                                    flag = true;
                                }
                                if (!String.IsNullOrEmpty(SZV_TD.ConfirmMiddleName) && SZV_TD.ConfirmMiddleName.Trim() != "")
                                {
                                    ФИО.Add(new XElement(УТ2 + "Отчество", SZV_TD.ConfirmMiddleName.Trim()));
                                    flag = true;
                                }
                                Руководитель.Add(ФИО);

                            }

                            if (!String.IsNullOrEmpty(SZV_TD.ConfirmDolgn) && SZV_TD.ConfirmDolgn.Trim() != "")
                            {
                                Руководитель.Add(new XElement(УТ2 + "Должность", SZV_TD.ConfirmDolgn.Trim()));
                                flag = true;
                            }

                            if (flag)
                                СЗВТД.Add(Руководитель);

                            xDoc.Element(pfr + "ЭДПФР").Add(СЗВТД);



                            XElement СлужебнаяИнформация = new XElement(pfr + "СлужебнаяИнформация",
                                  new XElement(АФ5 + "GUID", guid),
                                  new XElement(АФ5 + "ДатаВремя", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")));


                            xDoc.Element(pfr + "ЭДПФР").Add(СлужебнаяИнформация);


                            string filePath = pathBrowser.Value.ToString() + "\\" + fileName;
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }

                            using (var writer = new XmlTextWriter(filePath, new UTF8Encoding(false)) { Formatting = Formatting.Indented })
                            {
                                xDoc.Save(writer);
                            }
                        //              xDoc.Save(filePath);

                        Messenger.showAlert(AlertType.Success, "Сохранение", "XML-файл успешно сохранен по указанному пути!", this.ThemeName);

                            if (!Options.hideDialogCheckFiles)
                            {
                                PU.Service.CheckFiles.CheckFilesQuestion child = new PU.Service.CheckFiles.CheckFilesQuestion();
                                child.Owner = this;
                                child.ThemeName = this.ThemeName;
                                child.FileInfoList.Add(filePath);
                                child.ShowDialog();

                            }
                            else if (Options.checkFilesAfterSaving)
                            {
                                PU.Service.CheckFiles.CheckFilesList child = new PU.Service.CheckFiles.CheckFilesList();
                                child.Owner = this;
                                child.ThemeName = this.ThemeName;
                                child.FileInfoList.Add(filePath);
                                child.ShowDialog();
                            }


                        }
                        catch (Exception ex)
                        {
                        Messenger.showAlert(AlertType.Error, "Внимание!", "При формировании и сохранении файла произошла ошибка.\r\nКод ошибки: " + ex.Message, this.ThemeName);
                        }
                        finally
                        {
                            db_temp.Dispose();
                        }



                }
                else
                {
                    RadMessageBox.Show(this, "Не удалось загрузить данные Формы СЗВ-ТД из базы данных!", "Внимание");
                }
            }
                        else
                        {
                            RadMessageBox.Show(this, "Необходимо указать каталог, куда будет сохранен файл!", "Внимание");
                            pathBrowser.Focus();
                        }
        }
    }
}
