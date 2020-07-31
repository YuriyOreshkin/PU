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

namespace PU.ZAGS.Zags_Death
{
    public partial class CreateXmlPack_Death : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public long zagsID = 0;

        public CreateXmlPack_Death()
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

        private void CreateXmlPack_Death_Load(object sender, EventArgs e)
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

        private void CreateXmlPack_Death_FormClosing(object sender, FormClosingEventArgs e)
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
            props.setFormParams(this, windowData);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pathBrowser.Value != null)
            {
                if (db.ZAGS_Death.Any(x => x.ID == zagsID))
                {
                    pu6Entities db_temp = new pu6Entities();

                    try
                    {
                        ZAGS_Death zags = db_temp.ZAGS_Death.First(x => x.ID == zagsID);

                        //string regNum = codePoluchTextBox.Text;
                        Guid guid = Guid.NewGuid();

                        string fileName = String.Format("ПФР_{0}-{1}_РСОР_{2}_{3}.XML", codePoluchTextBox.Text, codeOtpravTextBox.Text, DateTime.Now.ToString("yyyyMMdd"), guid);
                        XNamespace pfr = "http://пф.рф/ЗАГС/РСОС/2016-01-01";
                        XNamespace ЗАГС = "http://пф.рф/ЗАГС/типы/2016-01-01";
                        XNamespace УТ = "http://пф.рф/унифицированныеТипы/2014-01-01";
                        XNamespace АФ = "http://пф.рф/АФ";

                        XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", null),
                                             new XElement(pfr + "ЭДПФР",
                                                 new XAttribute(XNamespace.Xmlns + "УТ", УТ.NamespaceName),
                                                 new XAttribute(XNamespace.Xmlns + "АФ", АФ.NamespaceName),
                                                 new XAttribute(XNamespace.Xmlns + "ЗАГС", ЗАГС.NamespaceName)));


                        XElement РСОС = new XElement(pfr + "РСОС");

                        XElement СведенияОСмерти = new XElement(pfr + "СведенияОСмерти");

                        XElement Умерший = new XElement(pfr + "Умерший");

                        XElement ФИО = new XElement(УТ + "ФИО");
                        if (!String.IsNullOrEmpty(zags.LastName))
                        {
                            ФИО.Add(new XElement(УТ + "Фамилия", zags.LastName.ToUpper()));
                        }
                        if (!String.IsNullOrEmpty(zags.FirstName))
                        {
                            ФИО.Add(new XElement(УТ + "Имя", zags.FirstName.ToUpper()));
                        }
                        if (!String.IsNullOrEmpty(zags.MiddleName))
                        {
                            ФИО.Add(new XElement(УТ + "Отчество", zags.MiddleName.ToUpper()));
                        }
                        Умерший.Add(ФИО);




                        if (zags.Sex.HasValue)
                        {
                            Умерший.Add(new XElement(УТ + "Пол", zags.Sex.HasValue ? (zags.Sex.Value == 0 ? "М" : "Ж") : ""));
                        }

                        if ((zags.Type_DateBirth.HasValue && zags.Type_DateBirth.Value == 1) || zags.DateBirth.HasValue)  //Если особая дата рождения
                        {
                            XElement Датарождения = new XElement(УТ + "ДатаРождения");
                            if (zags.Type_DateBirth.HasValue && zags.Type_DateBirth.Value == 1)  //Если особая дата рождения
                            {
                                Датарождения = new XElement(УТ + "ДатаРожденияОсобая");

                                if (zags.DateBirthDay_Os.HasValue)
                                {
                                    Датарождения.Add(new XElement(УТ + "День", zags.DateBirthDay_Os.Value.ToString()));
                                }
                                if (zags.DateBirthMonth_Os.HasValue)
                                {
                                    Датарождения.Add(new XElement(УТ + "Месяц", zags.DateBirthMonth_Os.Value.ToString()));
                                }

                                Датарождения.Add(new XElement(УТ + "Год", zags.DateBirthYear_Os.HasValue ? zags.DateBirthYear_Os.Value.ToString() : ""));
                            }
                            else
                            {
                                try
                                {
                                    if (zags.DateBirth.HasValue)
                                        Датарождения = new XElement(УТ + "ДатаРождения", zags.DateBirth.Value.ToString("yyyy-MM-dd"));
                                }
                                catch { }
                            }

                            Умерший.Add(Датарождения);
                        }


                        if ((zags.Type_PlaceBirth.HasValue && zags.Type_PlaceBirth.Value != 0) || !String.IsNullOrEmpty(zags.PunktBirth) || !String.IsNullOrEmpty(zags.DistrBirth) || !String.IsNullOrEmpty(zags.RegionBirth) || !String.IsNullOrEmpty(zags.CountryBirth))
                        {
                            XElement МестоРождения = new XElement(УТ + "МестоРождения");

                            if (zags.Type_PlaceBirth.HasValue)
                            {
                                МестоРождения.Add(new XElement(УТ + "ТипМестаРождения", zags.Type_PlaceBirth.HasValue ? ((short)zags.Type_PlaceBirth.Value == 1 ? "СТАНДАРТНОЕ" : "ОСОБОЕ") : "СТАНДАРТНОЕ"));
                            }

                            if (!String.IsNullOrEmpty(zags.PunktBirth))
                            {
                                МестоРождения.Add(new XElement(УТ + "ГородРождения", zags.PunktBirth.Length > 200 ? zags.PunktBirth.Substring(0, 200) : zags.PunktBirth));
                            }
                            if (!String.IsNullOrEmpty(zags.DistrBirth))
                            {
                                МестоРождения.Add(new XElement(УТ + "РайонРождения", zags.DistrBirth.Length > 200 ? zags.DistrBirth.Substring(0, 200) : zags.DistrBirth));
                            }
                            if (!String.IsNullOrEmpty(zags.RegionBirth))
                            {
                                МестоРождения.Add(new XElement(УТ + "РегионРождения", zags.RegionBirth.Length > 200 ? zags.RegionBirth.Substring(0, 200) : zags.RegionBirth));
                            }
                            if (!String.IsNullOrEmpty(zags.CountryBirth))
                            {
                                МестоРождения.Add(new XElement(УТ + "СтранаРождения", zags.CountryBirth.Length > 200 ? zags.CountryBirth.Substring(0, 200) : zags.CountryBirth));
                            }

                            Умерший.Add(МестоРождения);
                        }

                        СведенияОСмерти.Add(Умерший);

                        if (zags.DateDeath.HasValue)
                            СведенияОСмерти.Add(new XElement(pfr + "ДатаСмерти", zags.DateDeath.Value.ToString("yyyy-MM-dd")));


                        if (!String.IsNullOrEmpty(zags.RegionDeath) || !String.IsNullOrEmpty(zags.DistrDeath) || !String.IsNullOrEmpty(zags.CityDeath) || !String.IsNullOrEmpty(zags.PunktDeath))
                        {
                            XElement МестоСмерти = new XElement(pfr + "МестоСмерти");

                            if (!String.IsNullOrEmpty(zags.RegionDeath))
                            {
                                XElement Регион = new XElement(pfr + "Регион",
                                    new XElement(УТ + "Название", zags.RegionDeath));
                                if (!String.IsNullOrEmpty(zags.RegionDeath_sokr))
                                {
                                    Регион.Add(new XElement(УТ + "Сокращение", zags.RegionDeath_sokr));
                                }
                                МестоСмерти.Add(Регион);
                            }
                            if (!String.IsNullOrEmpty(zags.DistrDeath))
                            {
                                XElement Район = new XElement(pfr + "Район",
                                    new XElement(УТ + "Название", zags.DistrDeath));
                                if (!String.IsNullOrEmpty(zags.DistrDeath_sokr))
                                {
                                    Район.Add(new XElement(УТ + "Сокращение", zags.DistrDeath_sokr));
                                }
                                МестоСмерти.Add(Район);
                            }
                            if (!String.IsNullOrEmpty(zags.CityDeath))
                            {
                                XElement Город = new XElement(pfr + "Город",
                                    new XElement(УТ + "Название", zags.CityDeath));
                                if (!String.IsNullOrEmpty(zags.CityDeath_sokr))
                                {
                                    Город.Add(new XElement(УТ + "Сокращение", zags.CityDeath_sokr));
                                }
                                МестоСмерти.Add(Город);
                            }
                            if (!String.IsNullOrEmpty(zags.PunktDeath))
                            {
                                XElement НаселенныйПункт = new XElement(pfr + "НаселенныйПункт",
                                    new XElement(УТ + "Название", zags.PunktDeath));
                                if (!String.IsNullOrEmpty(zags.PunktDeath_sokr))
                                {
                                    НаселенныйПункт.Add(new XElement(УТ + "Сокращение", zags.PunktDeath_sokr));
                                }
                                МестоСмерти.Add(НаселенныйПункт);
                            }
                            if (!String.IsNullOrEmpty(zags.StreetDeath))
                            {
                                XElement Улица = new XElement(pfr + "Улица",
                                    new XElement(УТ + "Название", zags.StreetDeath));
                                if (!String.IsNullOrEmpty(zags.StreetDeath_sokr))
                                {
                                    Улица.Add(new XElement(УТ + "Сокращение", zags.StreetDeath_sokr));
                                }
                                МестоСмерти.Add(Улица);
                            }


                            if (!String.IsNullOrEmpty(zags.DomDeath))
                            {
                                XElement Дом = new XElement(pfr + "Дом",
                                    new XElement(УТ + "Номер", zags.DomDeath));
                                if (!String.IsNullOrEmpty(zags.DomDeath_sokr))
                                {
                                    Дом.Add(new XElement(УТ + "Сокращение", zags.DomDeath_sokr));
                                }
                                МестоСмерти.Add(Дом);
                            }
                            if (!String.IsNullOrEmpty(zags.StroenDeath))
                            {
                                XElement Строение = new XElement(pfr + "Строение",
                                    new XElement(УТ + "Номер", zags.StroenDeath));
                                if (!String.IsNullOrEmpty(zags.StroenDeath_sokr))
                                {
                                    Строение.Add(new XElement(УТ + "Сокращение", zags.StroenDeath_sokr));
                                }
                                МестоСмерти.Add(Строение);
                            }
                            if (!String.IsNullOrEmpty(zags.KorpDeath))
                            {
                                XElement Корпус = new XElement(pfr + "Корпус",
                                    new XElement(УТ + "Номер", zags.KorpDeath));
                                if (!String.IsNullOrEmpty(zags.KorpDeath_sokr))
                                {
                                    Корпус.Add(new XElement(УТ + "Сокращение", zags.KorpDeath_sokr));
                                }
                                МестоСмерти.Add(Корпус);
                            }
                            if (!String.IsNullOrEmpty(zags.KvartDeath))
                            {
                                XElement Квартира = new XElement(pfr + "Квартира",
                                    new XElement(УТ + "Номер", zags.KvartDeath));
                                if (!String.IsNullOrEmpty(zags.KvartDeath_sokr))
                                {
                                    Квартира.Add(new XElement(УТ + "Сокращение", zags.KvartDeath_sokr));
                                }
                                МестоСмерти.Add(Квартира);
                            }


                            СведенияОСмерти.Add(МестоСмерти);
                        }



                        if (!String.IsNullOrEmpty(zags.RegionLast) || !String.IsNullOrEmpty(zags.DistrLast) || !String.IsNullOrEmpty(zags.CityLast) || !String.IsNullOrEmpty(zags.PunktLast))
                        {
                            XElement ПоследнееМестоЖительства = new XElement(pfr + "ПоследнееМестоЖительства");

                            if (!String.IsNullOrEmpty(zags.RegionLast))
                            {
                                XElement Регион = new XElement(pfr + "Регион",
                                    new XElement(УТ + "Название", zags.RegionLast));
                                if (!String.IsNullOrEmpty(zags.RegionLast_sokr))
                                {
                                    Регион.Add(new XElement(УТ + "Сокращение", zags.RegionLast_sokr));
                                }
                                ПоследнееМестоЖительства.Add(Регион);
                            }
                            if (!String.IsNullOrEmpty(zags.DistrLast))
                            {
                                XElement Район = new XElement(pfr + "Район",
                                    new XElement(УТ + "Название", zags.DistrLast));
                                if (!String.IsNullOrEmpty(zags.DistrLast_sokr))
                                {
                                    Район.Add(new XElement(УТ + "Сокращение", zags.DistrLast_sokr));
                                }
                                ПоследнееМестоЖительства.Add(Район);
                            }
                            if (!String.IsNullOrEmpty(zags.CityLast))
                            {
                                XElement Город = new XElement(pfr + "Город",
                                    new XElement(УТ + "Название", zags.CityLast));
                                if (!String.IsNullOrEmpty(zags.CityLast_sokr))
                                {
                                    Город.Add(new XElement(УТ + "Сокращение", zags.CityLast_sokr));
                                }
                                ПоследнееМестоЖительства.Add(Город);
                            }
                            if (!String.IsNullOrEmpty(zags.PunktLast))
                            {
                                XElement НаселенныйПункт = new XElement(pfr + "НаселенныйПункт",
                                    new XElement(УТ + "Название", zags.PunktLast));
                                if (!String.IsNullOrEmpty(zags.PunktLast_sokr))
                                {
                                    НаселенныйПункт.Add(new XElement(УТ + "Сокращение", zags.PunktLast_sokr));
                                }
                                ПоследнееМестоЖительства.Add(НаселенныйПункт);
                            }
                            if (!String.IsNullOrEmpty(zags.StreetLast))
                            {
                                XElement Улица = new XElement(pfr + "Улица",
                                    new XElement(УТ + "Название", zags.StreetLast));
                                if (!String.IsNullOrEmpty(zags.StreetLast_sokr))
                                {
                                    Улица.Add(new XElement(УТ + "Сокращение", zags.StreetLast_sokr));
                                }
                                ПоследнееМестоЖительства.Add(Улица);
                            }


                            if (!String.IsNullOrEmpty(zags.DomLast))
                            {
                                XElement Дом = new XElement(pfr + "Дом",
                                    new XElement(УТ + "Номер", zags.DomLast));
                                if (!String.IsNullOrEmpty(zags.DomLast_sokr))
                                {
                                    Дом.Add(new XElement(УТ + "Сокращение", zags.DomLast_sokr));
                                }
                                ПоследнееМестоЖительства.Add(Дом);
                            }
                            if (!String.IsNullOrEmpty(zags.StroenLast))
                            {
                                XElement Строение = new XElement(pfr + "Строение",
                                    new XElement(УТ + "Номер", zags.StroenLast));
                                if (!String.IsNullOrEmpty(zags.StroenLast_sokr))
                                {
                                    Строение.Add(new XElement(УТ + "Сокращение", zags.StroenLast_sokr));
                                }
                                ПоследнееМестоЖительства.Add(Строение);
                            }
                            if (!String.IsNullOrEmpty(zags.KorpLast))
                            {
                                XElement Корпус = new XElement(pfr + "Корпус",
                                    new XElement(УТ + "Номер", zags.KorpLast));
                                if (!String.IsNullOrEmpty(zags.KorpLast_sokr))
                                {
                                    Корпус.Add(new XElement(УТ + "Сокращение", zags.KorpLast_sokr));
                                }
                                ПоследнееМестоЖительства.Add(Корпус);
                            }
                            if (!String.IsNullOrEmpty(zags.KvartLast))
                            {
                                XElement Квартира = new XElement(pfr + "Квартира",
                                    new XElement(УТ + "Номер", zags.KvartLast));
                                if (!String.IsNullOrEmpty(zags.KvartLast_sokr))
                                {
                                    Квартира.Add(new XElement(УТ + "Сокращение", zags.KvartLast_sokr));
                                }
                                ПоследнееМестоЖительства.Add(Квартира);
                            }


                            СведенияОСмерти.Add(ПоследнееМестоЖительства);
                        }



                        XElement Акт = new XElement(pfr + "Акт",
                                           new XElement(ЗАГС + "Номер", zags.Akt_Num),
                                           new XElement(ЗАГС + "ДатаСоставления", zags.Akt_Date.HasValue ? zags.Akt_Date.Value.ToString("yyyy-MM-dd") : ""),
                                           new XElement(ЗАГС + "ОрганЗАГС", zags.Akt_OrgZags));

                        СведенияОСмерти.Add(Акт);



                        РСОС.Add(СведенияОСмерти);



                        xDoc.Element(pfr + "ЭДПФР").Add(РСОС);

                        xDoc.Element(pfr + "ЭДПФР").Add(new XElement(pfr + "СлужебнаяИнформация",
                                                      new XElement(АФ + "GUID", guid),
                                                      new XElement(АФ + "ДатаВремя", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")),
                                                      new XElement(ЗАГС + "Составитель", FromTextBox.Text),
                                                      new XElement(ЗАГС + "Получатель", ToTextBox.Text)));

                        string filePath = pathBrowser.Value.ToString() + "\\" + fileName;
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }


                        xDoc.Save(filePath);

                        Methods.showAlert("Сохранение", "XML-файл успешно сохранен по указанному пути!", this.ThemeName);

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
                        Methods.showAlert("Внимание!", "При формировании и сохранении файла произошла ошибка.\r\nКод ошибки: " + ex.Message, this.ThemeName);
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
