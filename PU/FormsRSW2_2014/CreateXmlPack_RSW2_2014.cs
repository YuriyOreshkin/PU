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
using PU.FormsRSW2014;
using System.Text.RegularExpressions;
using System.IO; 

namespace PU.FormsRSW2_2014
{
    public partial class CreateXmlPack_RSW2_2014 : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public FormsRSW2014_2_1 RSWdata { get; set; }

        public CreateXmlPack_RSW2_2014()
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

        private void CreateXmlPack_RSW2_2014_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            if (RSWdata.Year >= 2015)
            {
                radLabel6.Visible = true;
                codeTOPFRTextBox.Visible = true;
                format2014ChkBox.Visible = true;
            }

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

            cntSheetsSpin.Value = 4;
            cntDocsLabel.Text = RSWdata.CountConfirmDoc.HasValue ? RSWdata.CountConfirmDoc.Value.ToString() : "0";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pathBrowser.Value != null)
            {
                if (RSWdata.Year == 2014)
                {
                    createXML_RSW2_2014();
                }
                else if (RSWdata.Year >= 2015)
                {
                    if (format2014ChkBox.Checked)
                        createXML_RSW2_2014();
                    else
                        createXML_RSW2_2015();
                }
            }
            else
            {
                RadMessageBox.Show(this, "Необходимо указать каталог, куда будет сохранен файл!", "Внимание");
                pathBrowser.Focus();
            }

        }

        private void createXML_RSW2_2014()
        {
            try
            {

                string regNum = Utils.ParseRegNum(RSWdata.Insurer.RegNum);
                int num = int.Parse(packNumSpin.Value.ToString());

                string fileName = String.Format("PFR-700-Y-{0}-ORG-{1}-DCK-{2}-DPT-{3}-DCK-{4}.XML", RSWdata.Year.ToString(), regNum, "0000" + num.ToString(), "000000", "00000");
                XNamespace pfr = "http://schema.pfr.ru";

                XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", null),
                                     new XElement("ФайлПФР",
                                         new XElement("ИмяФайла", fileName)));



                XElement РСВ2_2014 = new XElement("РСВ2_2014",// + RSWdata.Year.ToString()
                                         new XElement("РегНомерПФР", regNum),
                                         new XElement("НомерКорр", RSWdata.CorrectionNum.ToString()),
                                         new XElement("КалендарныйГод", RSWdata.Year.ToString()));

                if (RSWdata.WorkStop != 0)
                {
                    РСВ2_2014.Add(new XElement("ПрекращениеДеятельности", "Л"));
                }

                XElement ФИО = new XElement("ФИО");
                //if (!String.IsNullOrEmpty(RSWdata.Insurer.LastName))
                //{
                ФИО.Add(new XElement("Фамилия", !String.IsNullOrEmpty(RSWdata.Insurer.LastName) ? (RSWdata.Insurer.LastName.Substring(0, RSWdata.Insurer.LastName.Length > 40 ? 40 : RSWdata.Insurer.LastName.Length).ToUpper()) : ""));
                //}
                //if (!String.IsNullOrEmpty(RSWdata.Insurer.FirstName))
                //{
                ФИО.Add(new XElement("Имя", !String.IsNullOrEmpty(RSWdata.Insurer.FirstName) ? (RSWdata.Insurer.FirstName.Substring(0, RSWdata.Insurer.FirstName.Length > 40 ? 40 : RSWdata.Insurer.FirstName.Length).ToUpper()) : ""));
                //}
                //if (!String.IsNullOrEmpty(RSWdata.Insurer.MiddleName))
                //{
                ФИО.Add(new XElement("Отчество", !String.IsNullOrEmpty(RSWdata.Insurer.MiddleName) ? (RSWdata.Insurer.MiddleName.Substring(0, RSWdata.Insurer.MiddleName.Length > 40 ? 40 : RSWdata.Insurer.MiddleName.Length).ToUpper()) : ""));
                //}

                РСВ2_2014.Add(ФИО);

                if (RSWdata.Insurer.INN != null)
                {
                    РСВ2_2014.Add(new XElement("ИНН", RSWdata.Insurer.INN));
                }

                РСВ2_2014.Add(new XElement("КодПоОКВЭД", RSWdata.Insurer.OKWED != null ? RSWdata.Insurer.OKWED : "0"));

                if (!String.IsNullOrEmpty(RSWdata.Insurer.InsuranceNumber)) // && RSWdata.Year == 2014
                {
                    string contrNum = "";
                    if (RSWdata.Insurer.ControlNumber != null)
                    {
                        contrNum = RSWdata.Insurer.ControlNumber.HasValue ? RSWdata.Insurer.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                    }
                    РСВ2_2014.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(RSWdata.Insurer.InsuranceNumber) ? RSWdata.Insurer.InsuranceNumber.Substring(0, 3) + "-" + RSWdata.Insurer.InsuranceNumber.Substring(3, 3) + "-" + RSWdata.Insurer.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""));
                }

                if (RSWdata.Insurer.PhoneContact != null)
                {
                    string phone = Utils.removeCharsFromString(RSWdata.Insurer.PhoneContact);

                    РСВ2_2014.Add(new XElement("Телефон", phone));
                }

                РСВ2_2014.Add(new XElement("ГодРождения", RSWdata.Insurer.YearBirth.HasValue ? RSWdata.Insurer.YearBirth.Value : 0));

                if (RSWdata.CountEmployers.HasValue)
                {
                    РСВ2_2014.Add(new XElement("ЧленовКФХ", RSWdata.CountEmployers.Value.ToString()));
                }

                РСВ2_2014.Add(new XElement("СтраницФормы", cntSheetsSpin.Value.ToString()));

                РСВ2_2014.Add(new XElement("ЛистовПриложения", RSWdata.CountConfirmDoc.HasValue ? RSWdata.CountConfirmDoc.Value : 0));

                XElement ПодтверждениеСведений = new XElement("ПодтверждениеСведений",
                                                     new XElement("ТипПодтверждающего", RSWdata.ConfirmType.ToString()));

                XElement ФИОПодтверждающего = new XElement("ФИОПодтверждающего");
                if (!String.IsNullOrEmpty(RSWdata.ConfirmLastName))
                {
                    ФИОПодтверждающего.Add(new XElement("Фамилия", RSWdata.ConfirmLastName.Substring(0, RSWdata.ConfirmLastName.Length > 40 ? 40 : RSWdata.ConfirmLastName.Length).ToUpper()));
                }
                if (!String.IsNullOrEmpty(RSWdata.ConfirmFirstName))
                {
                    ФИОПодтверждающего.Add(new XElement("Имя", RSWdata.ConfirmFirstName.Substring(0, RSWdata.ConfirmFirstName.Length > 40 ? 40 : RSWdata.ConfirmFirstName.Length).ToUpper()));
                }
                if (!String.IsNullOrEmpty(RSWdata.ConfirmMiddleName))
                {
                    ФИОПодтверждающего.Add(new XElement("Отчество", RSWdata.ConfirmMiddleName.Substring(0, RSWdata.ConfirmMiddleName.Length > 40 ? 40 : RSWdata.ConfirmMiddleName.Length).ToUpper()));
                }
                ПодтверждениеСведений.Add(ФИОПодтверждающего);

                if (RSWdata.ConfirmType == 2)
                {
                    if (!String.IsNullOrEmpty(RSWdata.ConfirmOrgName))
                    {
                        ПодтверждениеСведений.Add(new XElement("НаименованиеОрганизации", RSWdata.ConfirmOrgName.Substring(0, RSWdata.ConfirmOrgName.Length > 100 ? 100 : RSWdata.ConfirmOrgName.Length).ToUpper()));
                    }

                }

                if (RSWdata.ConfirmDocDateBegin.HasValue || RSWdata.ConfirmDocDateEnd.HasValue)  // есть информация о доверенности
                {
                    XElement Доверенность = new XElement("Доверенность");
                    if (!String.IsNullOrEmpty(RSWdata.ConfirmDocSerLat))
                    {
                        Доверенность.Add(new XElement("Серия", RSWdata.ConfirmDocSerLat.ToUpper()));
                    }
                    if (RSWdata.ConfirmDocNum.HasValue)
                    {
                        Доверенность.Add(new XElement("Номер", RSWdata.ConfirmDocNum.Value));
                    }
                    if (RSWdata.ConfirmDocDate.HasValue)
                    {
                        Доверенность.Add(new XElement("ДатаВыдачи", RSWdata.ConfirmDocDate.Value.ToString("yyyy-MM-dd")));
                    }
                    if (!String.IsNullOrEmpty(RSWdata.ConfirmDocKemVyd))
                    {
                        Доверенность.Add(new XElement("КемВыдана", RSWdata.ConfirmDocKemVyd.ToUpper()));
                    }
                    if (RSWdata.ConfirmDocDateBegin.HasValue)
                    {
                        Доверенность.Add(new XElement("ДействуетС", RSWdata.ConfirmDocDateBegin.Value.ToString("yyyy-MM-dd")));
                    }
                    if (RSWdata.ConfirmDocDateEnd.HasValue)
                    {
                        Доверенность.Add(new XElement("ДействуетПо", RSWdata.ConfirmDocDateEnd.Value.ToString("yyyy-MM-dd")));
                    }


                    ПодтверждениеСведений.Add(Доверенность);
                }


                РСВ2_2014.Add(ПодтверждениеСведений);

                if (RSWdata.DateUnderwrite.HasValue)
                {
                    РСВ2_2014.Add(new XElement("ДатаЗаполнения", RSWdata.DateUnderwrite.Value.ToString("yyyy-MM-dd")));
                }

                #region Раздел1

                XElement Раздел1 = new XElement("Раздел1",
                                       new XElement("СтрокаРаздела",
                                           new XElement("КодСтроки", "100"),
                                           new XElement("СуммаОПС", RSWdata.s_100_0.HasValue ? Utils.decToStr(RSWdata.s_100_0.Value) : "0.00"),
                                           new XElement("СуммаСЧ", RSWdata.s_100_1.HasValue ? Utils.decToStr(RSWdata.s_100_1.Value) : "0.00"),
                                           new XElement("СуммаНЧ", RSWdata.s_100_2.HasValue ? Utils.decToStr(RSWdata.s_100_2.Value) : "0.00"),
                                           new XElement("СуммаОМС", RSWdata.s_100_3.HasValue ? Utils.decToStr(RSWdata.s_100_3.Value) : "0.00")),
                                       new XElement("СтрокаРаздела",
                                           new XElement("КодСтроки", "110"),
                                           new XElement("СуммаОПС", RSWdata.s_110_0.HasValue ? Utils.decToStr(RSWdata.s_110_0.Value) : "0.00"),
                                           new XElement("СуммаСЧ", RSWdata.s_110_1.HasValue ? Utils.decToStr(RSWdata.s_110_1.Value) : "0.00"),
                                           new XElement("СуммаНЧ", RSWdata.s_110_2.HasValue ? Utils.decToStr(RSWdata.s_110_2.Value) : "0.00"),
                                           new XElement("СуммаОМС", RSWdata.s_110_3.HasValue ? Utils.decToStr(RSWdata.s_110_3.Value) : "0.00")),
                                       new XElement("СтрокаРаздела",
                                           new XElement("КодСтроки", "120"),
                                           new XElement("СуммаОПС", RSWdata.s_120_0.HasValue ? Utils.decToStr(RSWdata.s_120_0.Value) : "0.00"),
                                           new XElement("СуммаСЧ", RSWdata.s_120_1.HasValue ? Utils.decToStr(RSWdata.s_120_1.Value) : "0.00"),
                                           new XElement("СуммаНЧ", RSWdata.s_120_2.HasValue ? Utils.decToStr(RSWdata.s_120_2.Value) : "0.00"),
                                           new XElement("СуммаОМС", RSWdata.s_120_3.HasValue ? Utils.decToStr(RSWdata.s_120_3.Value) : "0.00")),
                                       new XElement("СтрокаРаздела",
                                           new XElement("КодСтроки", "130"),
                                           new XElement("СуммаОПС", RSWdata.s_130_0.HasValue ? Utils.decToStr(RSWdata.s_130_0.Value) : "0.00"),
                                           new XElement("СуммаСЧ", RSWdata.s_130_1.HasValue ? Utils.decToStr(RSWdata.s_130_1.Value) : "0.00"),
                                           new XElement("СуммаНЧ", RSWdata.s_130_2.HasValue ? Utils.decToStr(RSWdata.s_130_2.Value) : "0.00"),
                                           new XElement("СуммаОМС", RSWdata.s_130_3.HasValue ? Utils.decToStr(RSWdata.s_130_3.Value) : "0.00")),
                                       new XElement("СтрокаРаздела",
                                           new XElement("КодСтроки", "140"),
                                           new XElement("СуммаОПС", RSWdata.s_140_0.HasValue ? Utils.decToStr(RSWdata.s_140_0.Value) : "0.00"),
                                           new XElement("СуммаСЧ", RSWdata.s_140_1.HasValue ? Utils.decToStr(RSWdata.s_140_1.Value) : "0.00"),
                                           new XElement("СуммаНЧ", RSWdata.s_140_2.HasValue ? Utils.decToStr(RSWdata.s_140_2.Value) : "0.00"),
                                           new XElement("СуммаОМС", RSWdata.s_140_3.HasValue ? Utils.decToStr(RSWdata.s_140_3.Value) : "0.00")),
                                       new XElement("СтрокаРаздела",
                                           new XElement("КодСтроки", "150"),
                                           new XElement("СуммаОПС", RSWdata.s_150_0.HasValue ? Utils.decToStr(RSWdata.s_150_0.Value) : "0.00"),
                                           new XElement("СуммаСЧ", RSWdata.s_150_1.HasValue ? Utils.decToStr(RSWdata.s_150_1.Value) : "0.00"),
                                           new XElement("СуммаНЧ", RSWdata.s_150_2.HasValue ? Utils.decToStr(RSWdata.s_150_2.Value) : "0.00"),
                                           new XElement("СуммаОМС", RSWdata.s_150_3.HasValue ? Utils.decToStr(RSWdata.s_150_3.Value) : "0.00")));

                РСВ2_2014.Add(Раздел1);

                #endregion

                #region Раздел2
                if (RSWdata.FormsRSW2014_2_2.Count > 0)
                {
                    XElement Раздел2 = new XElement("Раздел2");
                    int i = 0;
                    foreach (var razd2 in RSWdata.FormsRSW2014_2_2)
                    {
                        i++;
                        XElement ЧленКФХ = new XElement("ЧленКФХ",
                                               new XElement("НомерПП", i.ToString()));
                        ФИО = new XElement("ФИО");
                        if (!String.IsNullOrEmpty(razd2.LastName))
                        {
                            ФИО.Add(new XElement("Фамилия", razd2.LastName.Substring(0, razd2.LastName.Length > 40 ? 40 : razd2.LastName.Length).ToUpper()));
                        }
                        if (!String.IsNullOrEmpty(razd2.FirstName))
                        {
                            ФИО.Add(new XElement("Имя", razd2.FirstName.Substring(0, razd2.FirstName.Length > 40 ? 40 : razd2.FirstName.Length).ToUpper()));
                        }
                        if (!String.IsNullOrEmpty(razd2.MiddleName))
                        {
                            ФИО.Add(new XElement("Отчество", razd2.MiddleName.Substring(0, razd2.MiddleName.Length > 40 ? 40 : razd2.MiddleName.Length).ToUpper()));
                        }

                        ЧленКФХ.Add(ФИО);

                        if (razd2.InsuranceNumber != null)
                        {
                            string contrNum = "";
                            if (razd2.ControlNumber != null)
                            {
                                contrNum = razd2.ControlNumber.HasValue ? razd2.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                            }
                            ЧленКФХ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(razd2.InsuranceNumber) ? razd2.InsuranceNumber.Substring(0, 3) + "-" + razd2.InsuranceNumber.Substring(3, 3) + "-" + razd2.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""));
                        }

                        ЧленКФХ.Add(new XElement("ГодРождения", razd2.Year));

                        if (razd2.DateBegin.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("НачалоПериода", razd2.DateBegin.HasValue ? razd2.DateBegin.Value.ToString("yyyy-MM-dd") : ""));
                        }
                        if (razd2.DateEnd.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("КонецПериода", razd2.DateEnd.HasValue ? razd2.DateEnd.Value.ToString("yyyy-MM-dd") : ""));
                        }
                        if (razd2.SumOPS.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("СуммаОПС", razd2.SumOPS.HasValue ? Utils.decToStr(razd2.SumOPS.Value) : "0.00"));
                        }
                        if (razd2.SumOMS.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("СуммаОМС", razd2.SumOMS.HasValue ? Utils.decToStr(razd2.SumOMS.Value) : "0.00"));
                        }

                        Раздел2.Add(ЧленКФХ);
                    }
                    Раздел2.Add(new XElement("Итого",
                                    new XElement("СуммаОПС", Utils.decToStr(RSWdata.FormsRSW2014_2_2.Sum(x => x.SumOPS.Value))),
                                    new XElement("СуммаОМС", Utils.decToStr(RSWdata.FormsRSW2014_2_2.Sum(x => x.SumOMS.Value)))));

                    РСВ2_2014.Add(Раздел2);
                }
                #endregion

                #region Раздел3

                if (RSWdata.FormsRSW2014_2_3.Count() > 0)
                {

                    XElement Раздел3 = new XElement("Раздел3");
                    int i = 0;
                    foreach (var razd3 in RSWdata.FormsRSW2014_2_3)
                    {
                        i++;
                        XElement ЧленКФХ = new XElement("ЧленКФХ",
                                               new XElement("НомерПП", i.ToString()));


                        ФИО = new XElement("ФИО");
                        if (!String.IsNullOrEmpty(razd3.LastName))
                        {
                            ФИО.Add(new XElement("Фамилия", razd3.LastName.Substring(0, razd3.LastName.Length > 40 ? 40 : razd3.LastName.Length).ToUpper()));
                        }
                        if (!String.IsNullOrEmpty(razd3.FirstName))
                        {
                            ФИО.Add(new XElement("Имя", razd3.FirstName.Substring(0, razd3.FirstName.Length > 40 ? 40 : razd3.FirstName.Length).ToUpper()));
                        }
                        if (!String.IsNullOrEmpty(razd3.MiddleName))
                        {
                            ФИО.Add(new XElement("Отчество", razd3.MiddleName.Substring(0, razd3.MiddleName.Length > 40 ? 40 : razd3.MiddleName.Length).ToUpper()));
                        }

                        ЧленКФХ.Add(ФИО);

                        if (razd3.InsuranceNumber != null)
                        {
                            string contrNum = "";
                            if (razd3.ControlNumber != null)
                            {
                                contrNum = razd3.ControlNumber.HasValue ? razd3.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                            }
                            ЧленКФХ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(razd3.InsuranceNumber) ? razd3.InsuranceNumber.Substring(0, 3) + "-" + razd3.InsuranceNumber.Substring(3, 3) + "-" + razd3.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""));
                        }

                        ЧленКФХ.Add(new XElement("ГодРождения", razd3.Year));

                        if (razd3.DateBegin.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("НачалоПериода", razd3.DateBegin.HasValue ? razd3.DateBegin.Value.ToString("yyyy-MM-dd") : ""));
                        }
                        if (razd3.DateEnd.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("КонецПериода", razd3.DateEnd.HasValue ? razd3.DateEnd.Value.ToString("yyyy-MM-dd") : ""));
                        }
                        if (razd3.SumOPS_D.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("СуммаОПС", razd3.SumOPS_D.HasValue ? Utils.decToStr(razd3.SumOPS_D.Value) : "0.00"));
                        }
                        if (razd3.SumStrah_D.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("СуммаСЧ", razd3.SumStrah_D.HasValue ? Utils.decToStr(razd3.SumStrah_D.Value) : "0.00"));
                        }
                        if (razd3.SumNakop_D.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("СуммаНЧ", razd3.SumNakop_D.HasValue ? Utils.decToStr(razd3.SumNakop_D.Value) : "0.00"));
                        }
                        if (razd3.SumOMS_D.HasValue)
                        {
                            ЧленКФХ.Add(new XElement("СуммаОМС", razd3.SumOMS_D.HasValue ? Utils.decToStr(razd3.SumOMS_D.Value) : "0.00"));
                        }

                        Раздел3.Add(ЧленКФХ);
                    }
                    Раздел3.Add(new XElement("Итого",
                                    new XElement("СуммаОПС", Utils.decToStr(RSWdata.FormsRSW2014_2_3.Sum(x => x.SumOPS_D.Value))),
                                    new XElement("СуммаСЧ", Utils.decToStr(RSWdata.FormsRSW2014_2_3.Sum(x => x.SumStrah_D.Value))),
                                    new XElement("СуммаНЧ", Utils.decToStr(RSWdata.FormsRSW2014_2_3.Sum(x => x.SumNakop_D.Value))),
                                    new XElement("СуммаОМС", Utils.decToStr(RSWdata.FormsRSW2014_2_3.Sum(x => x.SumOMS_D.Value)))));

                    РСВ2_2014.Add(Раздел3);
                }

                #endregion

                xDoc.Element("ФайлПФР").Add(РСВ2_2014);
                xDoc.Root.SetDefaultXmlNamespace(pfr);

                string filePath = pathBrowser.Value.ToString() + "\\" + fileName;
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                xDoc.Save(filePath);

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

                packNumSpin.Value++;
                Methods.showAlert("Сохранение", "XML-файл успешно сохранен по указанному пути!", this.ThemeName);
            }
            catch (Exception ex)
            {
                Methods.showAlert("Внимание!", "При формировании и сохранении файла произошла ошибка.\r\nКод ошибки: " + ex.Message, this.ThemeName);
            }
        }

        private void createXML_RSW2_2015()
        {
            try { 
            string regNum = Utils.ParseRegNum(RSWdata.Insurer.RegNum);
            int num = int.Parse(packNumSpin.Value.ToString());

            Guid guid = Guid.NewGuid();

            string fileName = String.Format("ПФР_{0}_РСВ-2_{1}_{2}.XML", codeTOPFRTextBox.Text, DateTime.Now.ToString("yyyyMMdd"), guid);
            XNamespace pfr = "http://пф.рф/ВС/РСВ-2/2015-01-01";
            XNamespace УТ = "http://пф.рф/унифицированныеТипы/2014-01-01";
            XNamespace АФ = "http://пф.рф/АФ";

            XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", null),
                                 new XElement(pfr + "ЭДПФР",
                                     new XAttribute(XNamespace.Xmlns + "УТ", УТ.NamespaceName),
                                     new XAttribute(XNamespace.Xmlns + "АФ", АФ.NamespaceName)
                                     ));



            XElement РСВ2_2015 = new XElement(pfr + "РСВ-2",
                                     new XElement(pfr + "РегНомерПФР", regNum),
                                     new XElement(pfr + "НомерУточнения", RSWdata.CorrectionNum.ToString().PadLeft(3, '0')),
                                     new XElement(pfr + "КалендарныйГод", RSWdata.Year.ToString()));

            if (RSWdata.WorkStop != 0)
            {
                РСВ2_2015.Add(new XElement(pfr + "ПрекращениеДеятельности", "Л"));
            }

            XElement ФИО = new XElement(pfr + "ФИО");
            if (!String.IsNullOrEmpty(RSWdata.Insurer.LastName))
            {
            ФИО.Add(new XElement(УТ + "Фамилия", !String.IsNullOrEmpty(RSWdata.Insurer.LastName) ? (RSWdata.Insurer.LastName.Substring(0, RSWdata.Insurer.LastName.Length > 40 ? 40 : RSWdata.Insurer.LastName.Length).ToUpper()) : ""));
            }
            if (!String.IsNullOrEmpty(RSWdata.Insurer.FirstName))
            {
            ФИО.Add(new XElement(УТ + "Имя", !String.IsNullOrEmpty(RSWdata.Insurer.FirstName) ? (RSWdata.Insurer.FirstName.Substring(0, RSWdata.Insurer.FirstName.Length > 40 ? 40 : RSWdata.Insurer.FirstName.Length).ToUpper()) : ""));
            }
            if (!String.IsNullOrEmpty(RSWdata.Insurer.MiddleName))
            {
            ФИО.Add(new XElement(УТ + "Отчество", !String.IsNullOrEmpty(RSWdata.Insurer.MiddleName) ? (RSWdata.Insurer.MiddleName.Substring(0, RSWdata.Insurer.MiddleName.Length > 40 ? 40 : RSWdata.Insurer.MiddleName.Length).ToUpper()) : ""));
            }

            РСВ2_2015.Add(ФИО);

            РСВ2_2015.Add(new XElement(pfr + "ИНН", RSWdata.Insurer.INN != null ? RSWdata.Insurer.INN : ""));

            РСВ2_2015.Add(new XElement(pfr + "КодПоОКВЭД", RSWdata.Insurer.OKWED != null ? RSWdata.Insurer.OKWED : ""));

            if (RSWdata.Insurer.PhoneContact != null)
            {
                string phone = Utils.removeCharsFromString(RSWdata.Insurer.PhoneContact);
                РСВ2_2015.Add(new XElement(pfr + "Телефон", phone.Substring(0, phone.Length > 15 ? 15 : phone.Length)));
            }


            РСВ2_2015.Add(new XElement(pfr + "ЧленовКФХ", RSWdata.CountEmployers.HasValue ? RSWdata.CountEmployers.Value.ToString() : ""));

            РСВ2_2015.Add(new XElement(pfr + "СтраницФормы", cntSheetsSpin.Value.ToString()));

            РСВ2_2015.Add(new XElement(pfr + "ЛистовПриложения", RSWdata.CountConfirmDoc.HasValue ? RSWdata.CountConfirmDoc.Value : 0));

            XElement ПодтверждениеСведений = new XElement(pfr + "ПодтверждениеСведений",
                                                 new XElement(pfr + "ТипПодтверждающего", RSWdata.ConfirmType.ToString()));

            XElement ФИОПодтверждающего = new XElement(pfr + "ФИОПодтверждающего");
            if (!String.IsNullOrEmpty(RSWdata.ConfirmLastName))
            {
                ФИОПодтверждающего.Add(new XElement(УТ + "Фамилия", RSWdata.ConfirmLastName.Substring(0, RSWdata.ConfirmLastName.Length > 40 ? 40 : RSWdata.ConfirmLastName.Length).ToUpper()));
            }
            if (!String.IsNullOrEmpty(RSWdata.ConfirmFirstName))
            {
                ФИОПодтверждающего.Add(new XElement(УТ + "Имя", RSWdata.ConfirmFirstName.Substring(0, RSWdata.ConfirmFirstName.Length > 40 ? 40 : RSWdata.ConfirmFirstName.Length).ToUpper()));
            }
            if (!String.IsNullOrEmpty(RSWdata.ConfirmMiddleName))
            {
                ФИОПодтверждающего.Add(new XElement(УТ + "Отчество", RSWdata.ConfirmMiddleName.Substring(0, RSWdata.ConfirmMiddleName.Length > 40 ? 40 : RSWdata.ConfirmMiddleName.Length).ToUpper()));
            }
            ПодтверждениеСведений.Add(ФИОПодтверждающего);

            if (RSWdata.ConfirmType == 2)
            {
                if (!String.IsNullOrEmpty(RSWdata.ConfirmOrgName))
                {
                    ПодтверждениеСведений.Add(new XElement(pfr + "НаименованиеОрганизации", RSWdata.ConfirmOrgName.ToUpper()));
                }

            }

            if (RSWdata.ConfirmDocDateBegin.HasValue || RSWdata.ConfirmDocDateEnd.HasValue)  // есть информация о доверенности
            {
                XElement Доверенность = new XElement(pfr + "Доверенность");
                if (!String.IsNullOrEmpty(RSWdata.ConfirmDocSerLat))
                {
                    Доверенность.Add(new XElement(УТ + "Серия", RSWdata.ConfirmDocSerLat.ToUpper()));
                }
                if (RSWdata.ConfirmDocNum.HasValue)
                {
                    Доверенность.Add(new XElement(УТ + "Номер", RSWdata.ConfirmDocNum.Value));
                }
                if (RSWdata.ConfirmDocDate.HasValue)
                {
                    Доверенность.Add(new XElement(УТ + "ДатаВыдачи", RSWdata.ConfirmDocDate.Value.ToString("yyyy-MM-dd")));
                }
                if (!String.IsNullOrEmpty(RSWdata.ConfirmDocKemVyd))
                {
                    Доверенность.Add(new XElement(УТ + "КемВыдан", RSWdata.ConfirmDocKemVyd.ToUpper()));
                }
                if (RSWdata.ConfirmDocDateBegin.HasValue)
                {
                    Доверенность.Add(new XElement(УТ + "ДействуетС", RSWdata.ConfirmDocDateBegin.Value.ToString("yyyy-MM-dd")));
                }
                if (RSWdata.ConfirmDocDateEnd.HasValue)
                {
                    Доверенность.Add(new XElement(УТ + "ДействуетПо", RSWdata.ConfirmDocDateEnd.Value.ToString("yyyy-MM-dd")));
                }


                ПодтверждениеСведений.Add(Доверенность);
            }


            РСВ2_2015.Add(ПодтверждениеСведений);

            if (RSWdata.DateUnderwrite.HasValue)
            {
                РСВ2_2015.Add(new XElement(pfr + "ДатаЗаполнения", RSWdata.DateUnderwrite.Value.ToString("yyyy-MM-dd")));
            }

            #region Раздел1

            XElement Раздел1 = new XElement(pfr + "Раздел1",
                                   new XElement(pfr + "Строка",
                                       new XElement(pfr + "Код", "100"),
                                       new XElement(pfr + "СуммаОПС", RSWdata.s_100_0.HasValue ? Utils.decToStr(RSWdata.s_100_0.Value) : "0.00"),
                                       new XElement(pfr + "СуммаСЧ", RSWdata.s_100_1.HasValue ? Utils.decToStr(RSWdata.s_100_1.Value) : "0.00"),
                                       new XElement(pfr + "СуммаНЧ", RSWdata.s_100_2.HasValue ? Utils.decToStr(RSWdata.s_100_2.Value) : "0.00"),
                                       new XElement(pfr + "СуммаОМС", RSWdata.s_100_3.HasValue ? Utils.decToStr(RSWdata.s_100_3.Value) : "0.00")),
                                   new XElement(pfr + "Строка",
                                       new XElement(pfr + "Код", "110"),
                                       new XElement(pfr + "СуммаОПС", RSWdata.s_110_0.HasValue ? Utils.decToStr(RSWdata.s_110_0.Value) : "0.00"),
                                       new XElement(pfr + "СуммаСЧ", RSWdata.s_110_1.HasValue ? Utils.decToStr(RSWdata.s_110_1.Value) : "0.00"),
                                       new XElement(pfr + "СуммаНЧ", RSWdata.s_110_2.HasValue ? Utils.decToStr(RSWdata.s_110_2.Value) : "0.00"),
                                       new XElement(pfr + "СуммаОМС", RSWdata.s_110_3.HasValue ? Utils.decToStr(RSWdata.s_110_3.Value) : "0.00")),
                                   new XElement(pfr + "Строка",
                                       new XElement(pfr + "Код", "120"),
                                       new XElement(pfr + "СуммаОПС", RSWdata.s_120_0.HasValue ? Utils.decToStr(RSWdata.s_120_0.Value) : "0.00"),
                                       new XElement(pfr + "СуммаСЧ", RSWdata.s_120_1.HasValue ? Utils.decToStr(RSWdata.s_120_1.Value) : "0.00"),
                                       new XElement(pfr + "СуммаНЧ", RSWdata.s_120_2.HasValue ? Utils.decToStr(RSWdata.s_120_2.Value) : "0.00"),
                                       new XElement(pfr + "СуммаОМС", RSWdata.s_120_3.HasValue ? Utils.decToStr(RSWdata.s_120_3.Value) : "0.00")),
                                   new XElement(pfr + "Строка",
                                       new XElement(pfr + "Код", "130"),
                                       new XElement(pfr + "СуммаОПС", RSWdata.s_130_0.HasValue ? Utils.decToStr(RSWdata.s_130_0.Value) : "0.00"),
                                       new XElement(pfr + "СуммаСЧ", RSWdata.s_130_1.HasValue ? Utils.decToStr(RSWdata.s_130_1.Value) : "0.00"),
                                       new XElement(pfr + "СуммаНЧ", RSWdata.s_130_2.HasValue ? Utils.decToStr(RSWdata.s_130_2.Value) : "0.00"),
                                       new XElement(pfr + "СуммаОМС", RSWdata.s_130_3.HasValue ? Utils.decToStr(RSWdata.s_130_3.Value) : "0.00")),
                                   new XElement(pfr + "Строка",
                                       new XElement(pfr + "Код", "140"),
                                       new XElement(pfr + "СуммаОПС", RSWdata.s_140_0.HasValue ? Utils.decToStr(RSWdata.s_140_0.Value) : "0.00"),
                                       new XElement(pfr + "СуммаСЧ", RSWdata.s_140_1.HasValue ? Utils.decToStr(RSWdata.s_140_1.Value) : "0.00"),
                                       new XElement(pfr + "СуммаНЧ", RSWdata.s_140_2.HasValue ? Utils.decToStr(RSWdata.s_140_2.Value) : "0.00"),
                                       new XElement(pfr + "СуммаОМС", RSWdata.s_140_3.HasValue ? Utils.decToStr(RSWdata.s_140_3.Value) : "0.00")),
                                   new XElement(pfr + "Строка",
                                       new XElement(pfr + "Код", "150"),
                                       new XElement(pfr + "СуммаОПС", RSWdata.s_150_0.HasValue ? Utils.decToStr(RSWdata.s_150_0.Value) : "0.00"),
                                       new XElement(pfr + "СуммаСЧ", RSWdata.s_150_1.HasValue ? Utils.decToStr(RSWdata.s_150_1.Value) : "0.00"),
                                       new XElement(pfr + "СуммаНЧ", RSWdata.s_150_2.HasValue ? Utils.decToStr(RSWdata.s_150_2.Value) : "0.00"),
                                       new XElement(pfr + "СуммаОМС", RSWdata.s_150_3.HasValue ? Utils.decToStr(RSWdata.s_150_3.Value) : "0.00")));

            РСВ2_2015.Add(Раздел1);

            #endregion

            #region Раздел2
            if (RSWdata.FormsRSW2014_2_2.Count > 0)
            {
                XElement Раздел2 = new XElement(pfr + "Раздел2");
                int i = 0;
                foreach (var razd2 in RSWdata.FormsRSW2014_2_2)
                {
                    i++;
                    XElement ЧленКФХ = new XElement(pfr + "ЧленКФХ",
                                           new XElement(pfr + "НомерПП", i.ToString()));
                    ФИО = new XElement(pfr + "ФИО");
                    if (!String.IsNullOrEmpty(razd2.LastName))
                    {
                        ФИО.Add(new XElement(УТ + "Фамилия", razd2.LastName.Substring(0, razd2.LastName.Length > 40 ? 40 : razd2.LastName.Length).ToUpper()));
                    }
                    if (!String.IsNullOrEmpty(razd2.FirstName))
                    {
                        ФИО.Add(new XElement(УТ + "Имя", razd2.FirstName.Substring(0, razd2.FirstName.Length > 40 ? 40 : razd2.FirstName.Length).ToUpper()));
                    }
                    if (!String.IsNullOrEmpty(razd2.MiddleName))
                    {
                        ФИО.Add(new XElement(УТ + "Отчество", razd2.MiddleName.Substring(0, razd2.MiddleName.Length > 40 ? 40 : razd2.MiddleName.Length).ToUpper()));
                    }

                    ЧленКФХ.Add(ФИО);

                    if (razd2.InsuranceNumber != null)
                    {
                        string contrNum = "";
                        if (razd2.ControlNumber != null)
                        {
                            contrNum = razd2.ControlNumber.HasValue ? razd2.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                        }
                        ЧленКФХ.Add(new XElement(pfr + "СНИЛС", !String.IsNullOrEmpty(razd2.InsuranceNumber) ? razd2.InsuranceNumber.Substring(0, 3) + "-" + razd2.InsuranceNumber.Substring(3, 3) + "-" + razd2.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""));
                    }

                    ЧленКФХ.Add(new XElement(pfr + "ГодРождения", razd2.Year));

                    if (razd2.DateBegin.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "НачалоПериода", razd2.DateBegin.HasValue ? razd2.DateBegin.Value.ToString("yyyy-MM-dd") : ""));
                    }
                    if (razd2.DateEnd.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "КонецПериода", razd2.DateEnd.HasValue ? razd2.DateEnd.Value.ToString("yyyy-MM-dd") : ""));
                    }
                    if (razd2.SumOPS.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "СуммаОПС", razd2.SumOPS.HasValue ? Utils.decToStr(razd2.SumOPS.Value) : "0.00"));
                    }
                    if (razd2.SumOMS.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "СуммаОМС", razd2.SumOMS.HasValue ? Utils.decToStr(razd2.SumOMS.Value) : "0.00"));
                    }

                    Раздел2.Add(ЧленКФХ);
                }
                Раздел2.Add(new XElement(pfr + "Итого",
                                new XElement(pfr + "СуммаОПС", Utils.decToStr(RSWdata.FormsRSW2014_2_2.Sum(x => x.SumOPS.Value))),
                                new XElement(pfr + "СуммаОМС", Utils.decToStr(RSWdata.FormsRSW2014_2_2.Sum(x => x.SumOMS.Value)))));

                РСВ2_2015.Add(Раздел2);
            }
            #endregion

            #region Раздел3

            if (RSWdata.FormsRSW2014_2_3.Count() > 0)
            {

                XElement Раздел3 = new XElement(pfr + "Раздел3");
                int i = 0;
                foreach (var razd3 in RSWdata.FormsRSW2014_2_3)
                {
                    i++;
                    XElement ЧленКФХ = new XElement(pfr + "ЧленКФХ",
                                           new XElement(pfr + "НомерПП", i.ToString()));

                    ЧленКФХ.Add(new XElement(pfr + "Основание", razd3.CodeBase.HasValue ? razd3.CodeBase.Value : 1));

                    ФИО = new XElement(pfr + "ФИО");
                    if (!String.IsNullOrEmpty(razd3.LastName))
                    {
                        ФИО.Add(new XElement(УТ + "Фамилия", razd3.LastName.Substring(0, razd3.LastName.Length > 40 ? 40 : razd3.LastName.Length).ToUpper()));
                    }
                    if (!String.IsNullOrEmpty(razd3.FirstName))
                    {
                        ФИО.Add(new XElement(УТ + "Имя", razd3.FirstName.Substring(0, razd3.FirstName.Length > 40 ? 40 : razd3.FirstName.Length).ToUpper()));
                    }
                    if (!String.IsNullOrEmpty(razd3.MiddleName))
                    {
                        ФИО.Add(new XElement(УТ + "Отчество", razd3.MiddleName.Substring(0, razd3.MiddleName.Length > 40 ? 40 : razd3.MiddleName.Length).ToUpper()));
                    }

                    ЧленКФХ.Add(ФИО);

                    if (razd3.InsuranceNumber != null)
                    {
                        string contrNum = "";
                        if (razd3.ControlNumber != null)
                        {
                            contrNum = razd3.ControlNumber.HasValue ? razd3.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                        }
                        ЧленКФХ.Add(new XElement(pfr + "СНИЛС", !String.IsNullOrEmpty(razd3.InsuranceNumber) ? razd3.InsuranceNumber.Substring(0, 3) + "-" + razd3.InsuranceNumber.Substring(3, 3) + "-" + razd3.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""));
                    }

                    ЧленКФХ.Add(new XElement(pfr + "ГодРождения", razd3.Year));

                    if (razd3.DateBegin.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "НачалоПериода", razd3.DateBegin.HasValue ? razd3.DateBegin.Value.ToString("yyyy-MM-dd") : ""));
                    }
                    if (razd3.DateEnd.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "КонецПериода", razd3.DateEnd.HasValue ? razd3.DateEnd.Value.ToString("yyyy-MM-dd") : ""));
                    }
                    if (razd3.SumOPS_D.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "СуммаОПС", razd3.SumOPS_D.HasValue ? Utils.decToStr(razd3.SumOPS_D.Value) : "0.00"));
                    }
                    if (razd3.SumStrah_D.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "СуммаСЧ", razd3.SumStrah_D.HasValue ? Utils.decToStr(razd3.SumStrah_D.Value) : "0.00"));
                    }
                    if (razd3.SumNakop_D.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "СуммаНЧ", razd3.SumNakop_D.HasValue ? Utils.decToStr(razd3.SumNakop_D.Value) : "0.00"));
                    }
                    if (razd3.SumOMS_D.HasValue)
                    {
                        ЧленКФХ.Add(new XElement(pfr + "СуммаОМС", razd3.SumOMS_D.HasValue ? Utils.decToStr(razd3.SumOMS_D.Value) : "0.00"));
                    }

                    Раздел3.Add(ЧленКФХ);
                }
                Раздел3.Add(new XElement(pfr + "Итого",
                                new XElement(pfr + "СуммаОПС", Utils.decToStr(RSWdata.FormsRSW2014_2_3.Sum(x => x.SumOPS_D.Value))),
                                new XElement(pfr + "СуммаСЧ", Utils.decToStr(RSWdata.FormsRSW2014_2_3.Sum(x => x.SumStrah_D.Value))),
                                new XElement(pfr + "СуммаНЧ", Utils.decToStr(RSWdata.FormsRSW2014_2_3.Sum(x => x.SumNakop_D.Value))),
                                new XElement(pfr + "СуммаОМС", Utils.decToStr(RSWdata.FormsRSW2014_2_3.Sum(x => x.SumOMS_D.Value)))));

                РСВ2_2015.Add(Раздел3);
            }

            #endregion

            xDoc.Element(pfr + "ЭДПФР").Add(РСВ2_2015);

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


        private void CreateXmlPack_RSW2_2014_FormClosing(object sender, FormClosingEventArgs e)
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

