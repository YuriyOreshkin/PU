using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Schema;
using System.Xml;

namespace PU.ZAGS.Zags_Born
{
    public partial class CreateXmlPack_Born : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public long zagsID = 0;

        public CreateXmlPack_Born()
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

        private void CreateXmlPack_Born_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            pathBrowser.Value = Options.CurrentInsurerFolders.exportPath;

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
                            case "FromTextBox":
                                FromTextBox.Text = item.value;
                                break;
                            case "ToTextBox":
                                ToTextBox.Text = item.value;
                                break;
                            case "codePoluchTextBox":
                                codePoluchTextBox.Text = item.value;
                                break;
                            case "codeOtpravTextBox":
                                codeOtpravTextBox.Text = item.value;
                                break;
                        }
                    }

                }
                catch
                { }
            }

            FromTextBox.GotFocus += FromTextBox_OnFocus;
            FromTextBox.LostFocus += FromTextBox_OnDeFocus;
            ToTextBox.GotFocus += ToTextBox_OnFocus;
            ToTextBox.LostFocus += ToTextBox_OnDeFocus;
        }

        private void FromTextBox_OnFocus(object sender, EventArgs e)
        {
            FromTextBox.SelectAll();
        }

        private void FromTextBox_OnDeFocus(object sender, EventArgs e)
        {
            FromTextBox.SelectionLength = 0;
        }

        private void ToTextBox_OnFocus(object sender, EventArgs e)
        {
            ToTextBox.SelectAll();
        }

        private void ToTextBox_OnDeFocus(object sender, EventArgs e)
        {
            ToTextBox.SelectionLength = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreateXmlPack_Born_FormClosing(object sender, FormClosingEventArgs e)
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
                control = "FromTextBox",
                value = FromTextBox.Text
            });
            windowData.Add(new WindowData
            {
                control = "ToTextBox",
                value = ToTextBox.Text
            });
            windowData.Add(new WindowData
            {
                control = "codePoluchTextBox",
                value = codePoluchTextBox.Text
            });
            windowData.Add(new WindowData
            {
                control = "codeOtpravTextBox",
                value = codeOtpravTextBox.Text
            }); props.setFormParams(this, windowData);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pathBrowser.Value != null)
            {
                if (db.ZAGS_Born.Any(x => x.ID == zagsID))
                {
                    pu6Entities db_temp = new pu6Entities();

                    try
                    {
                        ZAGS_Born zags = db_temp.ZAGS_Born.First(x => x.ID == zagsID);

                        //string regNum = codePoluchTextBox.Text;
                        Guid guid = Guid.NewGuid();

                        string fileName = String.Format("ПФР_{0}-{1}_РСОР_{2}_{3}.XML", codePoluchTextBox.Text, codeOtpravTextBox.Text, DateTime.Now.ToString("yyyyMMdd"), guid);
                        XNamespace pfr = "http://пф.рф/ЗАГС/РСОР/2016-01-01";
                        XNamespace ЗАГС = "http://пф.рф/ЗАГС/типы/2016-01-01";
                        XNamespace УТ = "http://пф.рф/унифицированныеТипы/2014-01-01";
                        XNamespace АФ = "http://пф.рф/АФ";

                        XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", null),
                                             new XElement(pfr + "ЭДПФР",
                                                 new XAttribute(XNamespace.Xmlns + "УТ", УТ.NamespaceName),
                                                 new XAttribute(XNamespace.Xmlns + "АФ", АФ.NamespaceName),
                                                 new XAttribute(XNamespace.Xmlns + "ЗАГС", ЗАГС.NamespaceName)));


                        XElement РСОР = new XElement(pfr + "РСОР");

                        XElement СведенияОРождении = new XElement(pfr + "СведенияОРождении");

                        XElement Родившийся = new XElement(pfr + "Родившийся");

                        XElement ФИО = new XElement(УТ + "ФИО");
                        if (!String.IsNullOrEmpty(zags.cLastName))
                        {
                            ФИО.Add(new XElement(УТ + "Фамилия", zags.cLastName.Trim().ToUpper()));
                        }
                        if (!String.IsNullOrEmpty(zags.cFirstName))
                        {
                            ФИО.Add(new XElement(УТ + "Имя", zags.cFirstName.Trim().ToUpper()));
                        }
                        if (!String.IsNullOrEmpty(zags.cMiddleName))
                        {
                            ФИО.Add(new XElement(УТ + "Отчество", zags.cMiddleName.Trim().ToUpper()));
                        }
                        Родившийся.Add(ФИО);




                        if (zags.cSex.HasValue)
                        {
                            Родившийся.Add(new XElement(УТ + "Пол", zags.cSex.HasValue ? (zags.cSex.Value == 0 ? "М" : "Ж") : ""));
                        }

                        if ((zags.cType_DateBirth.HasValue && zags.cType_DateBirth.Value == 1) || zags.cDateBirth.HasValue)  //Если особая дата рождения
                        {
                            XElement Датарождения = new XElement(УТ + "ДатаРождения");
                            if (zags.cType_DateBirth.HasValue && zags.cType_DateBirth.Value == 1)  //Если особая дата рождения
                            {
                                Датарождения = new XElement(УТ + "ДатаРожденияОсобая");

                                if (zags.cDateBirthDay_Os.HasValue)
                                {
                                    Датарождения.Add(new XElement(УТ + "День", zags.cDateBirthDay_Os.Value.ToString()));
                                }
                                if (zags.cDateBirthMonth_Os.HasValue)
                                {
                                    Датарождения.Add(new XElement(УТ + "Месяц", zags.cDateBirthMonth_Os.Value.ToString()));
                                }

                                Датарождения.Add(new XElement(УТ + "Год", zags.cDateBirthYear_Os.HasValue ? zags.cDateBirthYear_Os.Value.ToString() : ""));
                            }
                            else
                            {
                                try
                                {
                                    if (zags.cDateBirth.HasValue)
                                        Датарождения = new XElement(УТ + "ДатаРождения", zags.cDateBirth.Value.ToString("yyyy-MM-dd"));
                                }
                                catch { }
                            }

                            Родившийся.Add(Датарождения);
                        }


                        if ((zags.cType_PlaceBirth.HasValue && zags.cType_PlaceBirth.Value != 0) || !String.IsNullOrEmpty(zags.cPunkt) || !String.IsNullOrEmpty(zags.cDistr) || !String.IsNullOrEmpty(zags.cRegion) || !String.IsNullOrEmpty(zags.cCountry))
                        {
                            XElement МестоРождения = new XElement(УТ + "МестоРождения");

                            if (zags.cType_PlaceBirth.HasValue)
                            {
                                МестоРождения.Add(new XElement(УТ + "ТипМестаРождения", zags.cType_PlaceBirth.HasValue ? ((short)zags.cType_PlaceBirth.Value == 1 ? "СТАНДАРТНОЕ" : "ОСОБОЕ") : "СТАНДАРТНОЕ"));
                            }

                            if (!String.IsNullOrEmpty(zags.cPunkt))
                            {
                                МестоРождения.Add(new XElement(УТ + "ГородРождения", zags.cPunkt.Trim().Length > 200 ? zags.cPunkt.Trim().Substring(0, 200) : zags.cPunkt.Trim()));
                            }
                            if (!String.IsNullOrEmpty(zags.cDistr))
                            {
                                МестоРождения.Add(new XElement(УТ + "РайонРождения", zags.cDistr.Trim().Length > 200 ? zags.cDistr.Trim().Substring(0, 200) : zags.cDistr.Trim()));
                            }
                            if (!String.IsNullOrEmpty(zags.cRegion))
                            {
                                МестоРождения.Add(new XElement(УТ + "РегионРождения", zags.cRegion.Trim().Length > 200 ? zags.cRegion.Trim().Substring(0, 200) : zags.cRegion.Trim()));
                            }
                            if (!String.IsNullOrEmpty(zags.cCountry))
                            {
                                МестоРождения.Add(new XElement(УТ + "СтранаРождения", zags.cCountry.Trim().Length > 200 ? zags.cCountry.Trim().Substring(0, 200) : zags.cCountry.Trim()));
                            }

                            Родившийся.Add(МестоРождения);
                        }

                        СведенияОРождении.Add(Родившийся);



                        XElement Мать = new XElement(pfr + "Мать");

                        bool flag = false;
                        ФИО = new XElement(УТ + "ФИО");
                        if (!String.IsNullOrEmpty(zags.mLastName))
                        {
                            ФИО.Add(new XElement(УТ + "Фамилия", zags.mLastName.Trim().ToUpper()));
                            flag = true;
                        }
                        if (!String.IsNullOrEmpty(zags.mFirstName))
                        {
                            ФИО.Add(new XElement(УТ + "Имя", zags.mFirstName.Trim().ToUpper()));
                            flag = true;
                        }
                        if (!String.IsNullOrEmpty(zags.mMiddleName))
                        {
                            ФИО.Add(new XElement(УТ + "Отчество", zags.mMiddleName.Trim().ToUpper()));
                            flag = true;
                        }
                        Мать.Add(ФИО);

                        if (flag)
                        {
                            if ((zags.mType_DateBirth.HasValue && zags.mType_DateBirth.Value == 1) || zags.mDateBirth.HasValue)  //Если особая дата рождения
                            {
                                XElement Датарождения = new XElement(УТ + "ДатаРождения");
                                if (zags.mType_DateBirth.HasValue && zags.mType_DateBirth.Value == 1)  //Если особая дата рождения
                                {
                                    Датарождения = new XElement(УТ + "ДатаРожденияОсобая");

                                    if (zags.mDateBirthDay_Os.HasValue)
                                    {
                                        Датарождения.Add(new XElement(УТ + "День", zags.mDateBirthDay_Os.Value.ToString()));
                                    }
                                    if (zags.mDateBirthMonth_Os.HasValue)
                                    {
                                        Датарождения.Add(new XElement(УТ + "Месяц", zags.mDateBirthMonth_Os.Value.ToString()));
                                    }

                                    Датарождения.Add(new XElement(УТ + "Год", zags.mDateBirthYear_Os.HasValue ? zags.mDateBirthYear_Os.Value.ToString() : ""));
                                }
                                else
                                {
                                    try
                                    {
                                        if (zags.mDateBirth.HasValue)
                                            Датарождения = new XElement(УТ + "ДатаРождения", zags.mDateBirth.Value.ToString("yyyy-MM-dd"));
                                    }
                                    catch { }
                                }

                                Мать.Add(Датарождения);
                            }


                            if ((zags.mType_PlaceBirth.HasValue && zags.mType_PlaceBirth.Value != 0) || !String.IsNullOrEmpty(zags.mPunkt) || !String.IsNullOrEmpty(zags.mDistr) || !String.IsNullOrEmpty(zags.mRegion) || !String.IsNullOrEmpty(zags.mCountry))
                            {
                                XElement МестоРождения = new XElement(УТ + "МестоРождения");

                                if (zags.mType_PlaceBirth.HasValue)
                                {
                                    МестоРождения.Add(new XElement(УТ + "ТипМестаРождения", zags.mType_PlaceBirth.HasValue ? ((short)zags.mType_PlaceBirth.Value == 1 ? "СТАНДАРТНОЕ" : "ОСОБОЕ") : "СТАНДАРТНОЕ"));
                                }

                                if (!String.IsNullOrEmpty(zags.mPunkt))
                                {
                                    МестоРождения.Add(new XElement(УТ + "ГородРождения", zags.mPunkt.Trim().Length > 200 ? zags.mPunkt.Trim().Substring(0, 200) : zags.mPunkt.Trim()));
                                }
                                if (!String.IsNullOrEmpty(zags.mDistr))
                                {
                                    МестоРождения.Add(new XElement(УТ + "РайонРождения", zags.mDistr.Trim().Length > 200 ? zags.mDistr.Trim().Substring(0, 200) : zags.mDistr.Trim()));
                                }
                                if (!String.IsNullOrEmpty(zags.mRegion))
                                {
                                    МестоРождения.Add(new XElement(УТ + "РегионРождения", zags.mRegion.Trim().Length > 200 ? zags.mRegion.Trim().Substring(0, 200) : zags.mRegion.Trim()));
                                }
                                if (!String.IsNullOrEmpty(zags.mCountry))
                                {
                                    МестоРождения.Add(new XElement(УТ + "СтранаРождения", zags.mCountry.Trim().Length > 200 ? zags.mCountry.Trim().Substring(0, 200) : zags.cCountry.Trim()));
                                }

                                Мать.Add(МестоРождения);
                            }

                            if (!String.IsNullOrEmpty(zags.mCitizenship))
                            {
                                Мать.Add(new XElement(УТ + "Гражданство", zags.mCitizenship.Trim().Length > 40 ? zags.mCitizenship.Trim().Substring(0, 40) : zags.mCitizenship.Trim()));
                            }

                            СведенияОРождении.Add(Мать);
                        }



                        XElement Отец = new XElement(pfr + "Отец");

                        flag = false;
                        ФИО = new XElement(УТ + "ФИО");
                        if (!String.IsNullOrEmpty(zags.fLastName))
                        {
                            ФИО.Add(new XElement(УТ + "Фамилия", zags.fLastName.Trim().ToUpper()));
                            flag = true;
                        }
                        if (!String.IsNullOrEmpty(zags.fFirstName))
                        {
                            ФИО.Add(new XElement(УТ + "Имя", zags.fFirstName.Trim().ToUpper()));
                            flag = true;
                        }
                        if (!String.IsNullOrEmpty(zags.fMiddleName))
                        {
                            ФИО.Add(new XElement(УТ + "Отчество", zags.fMiddleName.Trim().ToUpper()));
                            flag = true;
                        }
                        Отец.Add(ФИО);

                        if (flag)
                        {
                            if ((zags.fType_DateBirth.HasValue && zags.fType_DateBirth.Value == 1) || zags.fDateBirth.HasValue)  //Если особая дата рождения
                            {
                                XElement Датарождения = new XElement(УТ + "ДатаРождения");
                                if (zags.fType_DateBirth.HasValue && zags.fType_DateBirth.Value == 1)  //Если особая дата рождения
                                {
                                    Датарождения = new XElement(УТ + "ДатаРожденияОсобая");

                                    if (zags.fDateBirthDay_Os.HasValue)
                                    {
                                        Датарождения.Add(new XElement(УТ + "День", zags.fDateBirthDay_Os.Value.ToString()));
                                    }
                                    if (zags.fDateBirthMonth_Os.HasValue)
                                    {
                                        Датарождения.Add(new XElement(УТ + "Месяц", zags.fDateBirthMonth_Os.Value.ToString()));
                                    }

                                    Датарождения.Add(new XElement(УТ + "Год", zags.fDateBirthYear_Os.HasValue ? zags.fDateBirthYear_Os.Value.ToString() : ""));
                                }
                                else
                                {
                                    try
                                    {
                                        if (zags.fDateBirth.HasValue)
                                            Датарождения = new XElement(УТ + "ДатаРождения", zags.fDateBirth.Value.ToString("yyyy-MM-dd"));
                                    }
                                    catch { }
                                }

                                Отец.Add(Датарождения);
                            }


                            if ((zags.fType_PlaceBirth.HasValue && zags.fType_PlaceBirth.Value != 0) || !String.IsNullOrEmpty(zags.fPunkt) || !String.IsNullOrEmpty(zags.fDistr) || !String.IsNullOrEmpty(zags.fRegion) || !String.IsNullOrEmpty(zags.fCountry))
                            {
                                XElement МестоРождения = new XElement(УТ + "МестоРождения");

                                if (zags.fType_PlaceBirth.HasValue)
                                {
                                    МестоРождения.Add(new XElement(УТ + "ТипМестаРождения", zags.fType_PlaceBirth.HasValue ? ((short)zags.fType_PlaceBirth.Value == 1 ? "СТАНДАРТНОЕ" : "ОСОБОЕ") : "СТАНДАРТНОЕ"));
                                }

                                if (!String.IsNullOrEmpty(zags.fPunkt))
                                {
                                    МестоРождения.Add(new XElement(УТ + "ГородРождения", zags.fPunkt.Trim().Length > 200 ? zags.fPunkt.Trim().Substring(0, 200) : zags.fPunkt.Trim()));
                                }
                                if (!String.IsNullOrEmpty(zags.fDistr))
                                {
                                    МестоРождения.Add(new XElement(УТ + "РайонРождения", zags.fDistr.Trim().Length > 200 ? zags.fDistr.Trim().Substring(0, 200) : zags.fDistr.Trim()));
                                }
                                if (!String.IsNullOrEmpty(zags.fRegion))
                                {
                                    МестоРождения.Add(new XElement(УТ + "РегионРождения", zags.fRegion.Trim().Length > 200 ? zags.fRegion.Trim().Substring(0, 200) : zags.fRegion.Trim()));
                                }
                                if (!String.IsNullOrEmpty(zags.fCountry))
                                {
                                    МестоРождения.Add(new XElement(УТ + "СтранаРождения", zags.fCountry.Trim().Length > 200 ? zags.fCountry.Trim().Substring(0, 200) : zags.cCountry.Trim()));
                                }

                                Отец.Add(МестоРождения);
                            }

                            if (!String.IsNullOrEmpty(zags.fCitizenship))
                            {
                                Отец.Add(new XElement(УТ + "Гражданство", zags.fCitizenship.Trim().Length > 40 ? zags.fCitizenship.Trim().Substring(0, 40) : zags.fCitizenship.Trim()));
                            }

                            СведенияОРождении.Add(Отец);

                        }

                        XElement Акт = new XElement(pfr + "Акт",
                                           new XElement(ЗАГС + "Номер", zags.cAkt_Num.Trim()),
                                           new XElement(ЗАГС + "ДатаСоставления", zags.cAkt_Date.HasValue ? zags.cAkt_Date.Value.ToString("yyyy-MM-dd") : ""),
                                           new XElement(ЗАГС + "ОрганЗАГС", zags.cAkt_OrgZags.Trim()));

                        СведенияОРождении.Add(Акт);


                        XElement Свидетельство = new XElement(pfr + "Свидетельство",
                                           new XElement(УТ + "Серия", zags.cSvid_Ser.Trim()),
                                           new XElement(УТ + "Номер", zags.cSvid_Num.Trim()),
                                           new XElement(УТ + "ДатаВыдачи", zags.cSvid_Date.HasValue ? zags.cSvid_Date.Value.ToString("yyyy-MM-dd") : ""),
                                           new XElement(pfr + "КемВыдан", zags.cSvid_OrgZags.Trim()));

                        СведенияОРождении.Add(Свидетельство);


                        РСОР.Add(СведенияОРождении);



                        xDoc.Element(pfr + "ЭДПФР").Add(РСОР);

                        xDoc.Element(pfr + "ЭДПФР").Add(new XElement(pfr + "СлужебнаяИнформация",
                                                      new XElement(АФ + "GUID", guid),
                                                      new XElement(АФ + "ДатаВремя", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")),
                                                      new XElement(ЗАГС + "Составитель", FromTextBox.Text.Trim()),
                                                      new XElement(ЗАГС + "Получатель", ToTextBox.Text.Trim())));

                        string filePath = pathBrowser.Value.ToString() + "\\" + fileName;
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }


                        xDoc.Save(filePath);

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
                    RadMessageBox.Show(this, "Не удалось загрузить данные Формы СЗВ-М из базы данных!", "Внимание");
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
