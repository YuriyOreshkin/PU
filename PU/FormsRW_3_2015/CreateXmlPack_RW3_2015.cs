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
using PU.FormsRW_3_2015;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Schema;

namespace PU.FormsRW_3_2015
{
    public partial class CreateXmlPack_RW3_2015 : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public FormsRW3_2015 RWdata { get; set; }

        public CreateXmlPack_RW3_2015()
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

        private void CreateXmlPack_RW3_2015_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            pathBrowser.Value = Options.CurrentInsurerFolders.exportPath;

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

            if (Options.saveLastPackNum)
            {
                try // пробуем восстановить последний номер пачки
                {
                    Props props = new Props(); //экземпляр класса с настройками
                    packNumSpin.Value = props.getPackNum(this.Name);
                }
                catch { }
            }

            cntSheetsSpin.Value = 3;
            cntDocsLabel.Text = RWdata.CountConfirmDoc.HasValue ? RWdata.CountConfirmDoc.Value.ToString() : "0";
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pathBrowser.Value != null)
            {
                try { 
                string regNum = Utils.ParseRegNum(RWdata.Insurer.RegNum);
                int num = int.Parse(packNumSpin.Value.ToString());
                Guid guid = Guid.NewGuid();

                string fileName = String.Format("ПФР_{0}_РВ-3_2015_{1}_{2}.XML", codeTOPFRTextBox.Text, DateTime.Now.ToString("yyyyMMdd"), guid);
                XNamespace pfr = "http://пф.рф/ВС/РВ-3/2015-01-01";
                XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                XNamespace УТ = "http://пф.рф/унифицированныеТипы/2014-01-01";
                XNamespace АФ = "http://пф.рф/АФ";
                XNamespace РВ = "http://пф.рф/ВС/типыРВ/2014-01-01";

                XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", null),
                                     new XElement(pfr + "ЭДПФР",
                                         new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                                         new XAttribute(XNamespace.Xmlns + "УТ", УТ.NamespaceName),
                                         new XAttribute(XNamespace.Xmlns + "АФ", АФ.NamespaceName),
                                         new XAttribute(XNamespace.Xmlns + "РВ", РВ.NamespaceName),
                                         new XAttribute(xsi + "schemaLocation", @"http://пф.рф/ВС/РВ-3/2015-01-01 ..\..\..\..\Схемы\ВС\Входящие\РСВ\РВ-3_2015-01-01.xsd")
                                         ));




                XElement РВ_3 = new XElement(pfr + "РВ-3",
                                         new XElement(pfr + "РегНомерПФР", regNum),
                                         new XElement(pfr + "НомерУточнения", RWdata.CorrectionNum.ToString().PadLeft(3, '0')),
                                         new XElement(pfr + "КодОтчПериода", RWdata.Quarter.ToString()),
                                         new XElement(pfr + "КалендарныйГод", RWdata.Year.ToString()));

                if (RWdata.WorkStop != 0)
                {
                    РВ_3.Add(new XElement(pfr + "ПрекращениеДеятельности", "Л"));
                }

                if (!String.IsNullOrEmpty(RWdata.Insurer.NameShort))
                {
                    РВ_3.Add(new XElement(pfr + "НаименованиеОрганизации", RWdata.Insurer.NameShort.ToUpper()));
                }
                else if (!String.IsNullOrEmpty(RWdata.Insurer.Name))
                {
                    РВ_3.Add(new XElement(pfr + "НаименованиеОрганизации", RWdata.Insurer.Name.ToUpper()));
                }



                //XElement ФИО = new XElement("ФИО");
                //if (!String.IsNullOrEmpty(RWdata.Insurer.LastName))
                //{
                //    ФИО.Add(new XElement("Фамилия", RWdata.Insurer.LastName.Substring(0, RWdata.Insurer.LastName.Length > 40 ? 40 : RWdata.Insurer.LastName.Length).ToUpper()));
                //}
                //if (!String.IsNullOrEmpty(RWdata.Insurer.FirstName))
                //{
                //    ФИО.Add(new XElement("Имя", RWdata.Insurer.FirstName.Substring(0, RWdata.Insurer.FirstName.Length > 40 ? 40 : RWdata.Insurer.FirstName.Length).ToUpper()));
                //}
                //if (!String.IsNullOrEmpty(RWdata.Insurer.MiddleName))
                //{
                //    ФИО.Add(new XElement("Отчество", RWdata.Insurer.MiddleName.Substring(0, RWdata.Insurer.MiddleName.Length > 40 ? 40 : RWdata.Insurer.MiddleName.Length).ToUpper()));
                //}

                //РВ_3.Add(ФИО);

                РВ_3.Add(new XElement(pfr + "ИНН", !String.IsNullOrEmpty(RWdata.Insurer.INN.ToString()) ? RWdata.Insurer.INN.ToString() : ""));


                if (!String.IsNullOrEmpty(RWdata.Insurer.KPP))
                {
                    РВ_3.Add(new XElement(pfr + "КПП", RWdata.Insurer.KPP.Substring(0, RWdata.Insurer.KPP.Length > 9 ? 9 : RWdata.Insurer.KPP.Length)));
                }
                else
                {
                    РВ_3.Add(new XElement(pfr + "КПП", ""));
                }


                РВ_3.Add(new XElement(pfr + "КодПоОКВЭД", !String.IsNullOrEmpty(RWdata.Insurer.OKWED) ? RWdata.Insurer.OKWED : ""));

                if (!String.IsNullOrEmpty(RWdata.Insurer.PhoneContact))
                {
                    string phone = Utils.removeCharsFromString(RWdata.Insurer.PhoneContact);

                    РВ_3.Add(new XElement(pfr + "Телефон", phone));
                }
                else {
                    РВ_3.Add(new XElement(pfr + "Телефон", ""));
                }

                РВ_3.Add(new XElement(pfr + "КодТарифа", RWdata.CodeTar));

                РВ_3.Add(new XElement(pfr + "СтраницФормы", cntSheetsSpin.Value.ToString()));

                РВ_3.Add(new XElement(pfr + "ЛистовПриложения", RWdata.CountConfirmDoc.HasValue ? RWdata.CountConfirmDoc.Value : 0));


                XElement ПодтверждениеСведений = new XElement(pfr + "ПодтверждениеСведений",
                                                     new XElement(РВ + "ТипПодтверждающего", RWdata.ConfirmType.ToString()));

                XElement ФИОПодтверждающего = new XElement(РВ + "ФИОПодтверждающего");
                if (!String.IsNullOrEmpty(RWdata.ConfirmLastName))
                {
                    ФИОПодтверждающего.Add(new XElement(УТ + "Фамилия", RWdata.ConfirmLastName.Substring(0, RWdata.ConfirmLastName.Length > 40 ? 40 : RWdata.ConfirmLastName.Length).ToUpper()));
                }
                if (!String.IsNullOrEmpty(RWdata.ConfirmFirstName))
                {
                    ФИОПодтверждающего.Add(new XElement(УТ + "Имя", RWdata.ConfirmFirstName.Substring(0, RWdata.ConfirmFirstName.Length > 40 ? 40 : RWdata.ConfirmFirstName.Length).ToUpper()));
                }
                if (!String.IsNullOrEmpty(RWdata.ConfirmMiddleName))
                {
                    ФИОПодтверждающего.Add(new XElement(УТ + "Отчество", RWdata.ConfirmMiddleName.Substring(0, RWdata.ConfirmMiddleName.Length > 40 ? 40 : RWdata.ConfirmMiddleName.Length).ToUpper()));
                }
                ПодтверждениеСведений.Add(ФИОПодтверждающего);

                if (RWdata.ConfirmType >= 2)
                {
                    if (!String.IsNullOrEmpty(RWdata.ConfirmOrgName))
                    {
                        ПодтверждениеСведений.Add(new XElement(РВ + "НаименованиеОрганизации", RWdata.ConfirmOrgName.Substring(0, RWdata.ConfirmOrgName.Length > 100 ? 100 : RWdata.ConfirmOrgName.Length).ToUpper()));
                    }
                }

                if (RWdata.ConfirmDocDateBegin.HasValue || RWdata.ConfirmDocDateEnd.HasValue)  // есть информация о доверенности
                {
                    XElement Доверенность = new XElement(РВ + "Доверенность");
                    if (!String.IsNullOrEmpty(RWdata.ConfirmDocSerLat))
                    {
                        Доверенность.Add(new XElement(УТ + "Серия", RWdata.ConfirmDocSerLat.ToUpper()));
                    }
                    if (RWdata.ConfirmDocNum.HasValue)
                    {
                        Доверенность.Add(new XElement(УТ + "Номер", RWdata.ConfirmDocNum.Value));
                    }
                    if (RWdata.ConfirmDocDate.HasValue)
                    {
                        Доверенность.Add(new XElement(УТ + "ДатаВыдачи", RWdata.ConfirmDocDate.Value.ToString("yyyy-MM-dd")));
                    }
                    if (!String.IsNullOrEmpty(RWdata.ConfirmDocKemVyd))
                    {
                        Доверенность.Add(new XElement(УТ + "КемВыдан", RWdata.ConfirmDocKemVyd.ToUpper()));
                    }
                    if (RWdata.ConfirmDocDateBegin.HasValue)
                    {
                        Доверенность.Add(new XElement(УТ + "ДействуетС", RWdata.ConfirmDocDateBegin.Value.ToString("yyyy-MM-dd")));
                    }
                    if (RWdata.ConfirmDocDateEnd.HasValue)
                    {
                        Доверенность.Add(new XElement(УТ + "ДействуетПо", RWdata.ConfirmDocDateEnd.Value.ToString("yyyy-MM-dd")));
                    }


                    ПодтверждениеСведений.Add(Доверенность);
                }


                РВ_3.Add(ПодтверждениеСведений);

                if (RWdata.DateUnderwrite.HasValue)
                {
                    РВ_3.Add(new XElement(pfr + "ДатаЗаполнения", RWdata.DateUnderwrite.Value.ToString("yyyy-MM-dd")));
                }


                #region Раздел1

                XElement Раздел1 = new XElement(pfr + "Раздел1",
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "100"),
                                           new XElement(pfr + "Взносы", RWdata.s_100_0.HasValue ? Utils.decToStr(RWdata.s_100_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "110"),
                                           new XElement(pfr + "Взносы", RWdata.s_110_0.HasValue ? Utils.decToStr(RWdata.s_110_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "111"),
                                           new XElement(pfr + "Взносы", RWdata.s_111_0.HasValue ? Utils.decToStr(RWdata.s_111_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "112"),
                                           new XElement(pfr + "Взносы", RWdata.s_112_0.HasValue ? Utils.decToStr(RWdata.s_112_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "113"),
                                           new XElement(pfr + "Взносы", RWdata.s_113_0.HasValue ? Utils.decToStr(RWdata.s_113_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "114"),
                                           new XElement(pfr + "Взносы", RWdata.s_114_0.HasValue ? Utils.decToStr(RWdata.s_114_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "120"),
                                           new XElement(pfr + "Взносы", RWdata.s_120_0.HasValue ? Utils.decToStr(RWdata.s_120_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "130"),
                                           new XElement(pfr + "Взносы", RWdata.s_130_0.HasValue ? Utils.decToStr(RWdata.s_130_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "140"),
                                           new XElement(pfr + "Взносы", RWdata.s_140_0.HasValue ? Utils.decToStr(RWdata.s_140_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "141"),
                                           new XElement(pfr + "Взносы", RWdata.s_141_0.HasValue ? Utils.decToStr(RWdata.s_141_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "142"),
                                           new XElement(pfr + "Взносы", RWdata.s_142_0.HasValue ? Utils.decToStr(RWdata.s_142_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "143"),
                                           new XElement(pfr + "Взносы", RWdata.s_143_0.HasValue ? Utils.decToStr(RWdata.s_143_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "144"),
                                           new XElement(pfr + "Взносы", RWdata.s_144_0.HasValue ? Utils.decToStr(RWdata.s_144_0.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "150"),
                                           new XElement(pfr + "Взносы", RWdata.s_150_0.HasValue ? Utils.decToStr(RWdata.s_150_0.Value) : "0.00")));

                РВ_3.Add(Раздел1);

                #endregion

                #region Раздел2

                XElement Раздел2 = new XElement(pfr + "Раздел2",
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "200"),
                                           new XElement(pfr + "Всего", RWdata.s_200_0.HasValue ? Utils.decToStr(RWdata.s_200_0.Value) : "0.00"),
                                           new XElement(pfr + "Месяц1", RWdata.s_200_1.HasValue ? Utils.decToStr(RWdata.s_200_1.Value) : "0.00"),
                                           new XElement(pfr + "Месяц2", RWdata.s_200_2.HasValue ? Utils.decToStr(RWdata.s_200_2.Value) : "0.00"),
                                           new XElement(pfr + "Месяц3", RWdata.s_200_3.HasValue ? Utils.decToStr(RWdata.s_200_3.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "210"),
                                           new XElement(pfr + "Всего", RWdata.s_210_0.HasValue ? Utils.decToStr(RWdata.s_210_0.Value) : "0.00"),
                                           new XElement(pfr + "Месяц1", RWdata.s_210_1.HasValue ? Utils.decToStr(RWdata.s_210_1.Value) : "0.00"),
                                           new XElement(pfr + "Месяц2", RWdata.s_210_2.HasValue ? Utils.decToStr(RWdata.s_210_2.Value) : "0.00"),
                                           new XElement(pfr + "Месяц3", RWdata.s_210_3.HasValue ? Utils.decToStr(RWdata.s_210_3.Value) : "0.00")),
                                       new XElement(pfr + "Строка",
                                           new XElement(pfr + "Код", "220"),
                                           new XElement(pfr + "Всего", RWdata.s_220_0.HasValue ? Utils.decToStr(RWdata.s_220_0.Value) : "0.00"),
                                           new XElement(pfr + "Месяц1", RWdata.s_220_1.HasValue ? Utils.decToStr(RWdata.s_220_1.Value) : "0.00"),
                                           new XElement(pfr + "Месяц2", RWdata.s_220_2.HasValue ? Utils.decToStr(RWdata.s_220_2.Value) : "0.00"),
                                           new XElement(pfr + "Месяц3", RWdata.s_220_3.HasValue ? Utils.decToStr(RWdata.s_220_3.Value) : "0.00")),
                                       new XElement(pfr + "Строка230",
                                           new XElement(pfr + "Всего", RWdata.s_230_0.HasValue ? RWdata.s_230_0.Value : 0),
                                           new XElement(pfr + "Месяц1", RWdata.s_230_1.HasValue ? RWdata.s_230_1.Value : 0),
                                           new XElement(pfr + "Месяц2", RWdata.s_230_2.HasValue ? RWdata.s_230_2.Value : 0),
                                           new XElement(pfr + "Месяц3", RWdata.s_230_3.HasValue ? RWdata.s_230_3.Value : 0)));

                РВ_3.Add(Раздел2);

                #endregion

                #region Раздел3

                if (RWdata.FormsRW3_2015_Razd_3.Count() > 0)
                {

                    XElement Раздел3 = new XElement(pfr + "Раздел3");

                    int i = 0;
                    foreach (var razd3 in RWdata.FormsRW3_2015_Razd_3)
                    {
                        i++;
                        XElement Перерасчет = new XElement(pfr + "Перерасчет",
                                               new XElement(pfr + "НомерПП", i.ToString()),
                                               new XElement(pfr + "Основание", razd3.CodeBase.HasValue ? razd3.CodeBase.Value.ToString() : ""),
                                               new XElement(pfr + "Год", razd3.Year.HasValue ? razd3.Year.Value.ToString() : ""),
                                               new XElement(pfr + "Месяц", razd3.Month.HasValue ? razd3.Month.Value.ToString() : ""),
                                               new XElement(pfr + "Сумма", razd3.SumFee.HasValue ? Utils.decToStr(razd3.SumFee.Value) : "0.00"));


                        Раздел3.Add(Перерасчет);
                    }
                    Раздел3.Add(new XElement(pfr + "Итого", Utils.decToStr(RWdata.FormsRW3_2015_Razd_3.Sum(x => x.SumFee.Value))));

                    РВ_3.Add(Раздел3);
                }

                #endregion

                xDoc.Element(pfr + "ЭДПФР").Add(РВ_3);

                xDoc.Element(pfr + "ЭДПФР").Add(new XElement(pfr + "СлужебнаяИнформация",
                                              new XElement(АФ + "GUID", guid),
                                              new XElement(АФ + "ДатаВремя", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"))));

                
                string filePath = pathBrowser.Value.ToString() + "\\" + fileName;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                xDoc.Save(filePath);

                packNumSpin.Value++;
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

            }
            else
            {
                RadMessageBox.Show(this, "Необходимо указать каталог, куда будет сохранен файл!", "Внимание");
                pathBrowser.Focus();
            }
        }

        private void codeTOPFRTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            //{
            //    e.Handled = true;
            //}

            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void CreateXmlPack_RW3_2015_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками

            if (Options.saveLastPackNum)
            {

                numPackSettings numPackSett = new numPackSettings
                {
                    FormName = this.Name,
                    Number = (int)packNumSpin.Value,
                };

                props.setPackNum(numPackSett);
            }

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


    }
}
