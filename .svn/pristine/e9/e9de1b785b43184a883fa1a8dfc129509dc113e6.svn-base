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

namespace PU.FormsSZVM_2016
{
    public partial class CreateXmlPack_SZV_M_2016 : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public long szvmID = 0;

        public CreateXmlPack_SZV_M_2016()
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

        private void CreateXmlPack_SZV_M_2016_Load(object sender, EventArgs e)
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

        private void CreateXmlPack_SZV_M_2016_FormClosing(object sender, FormClosingEventArgs e)
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

        private void codeTOPFRTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private class StaffCont
        {
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string InsuranceNumber { get; set; }
            public short? ControlNumber { get; set; }
            public string INN { get; set; }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pathBrowser.Value != null)
            {
                if (db.FormsSZV_M_2016.Any(x => x.ID == szvmID))
                {
                        pu6Entities db_temp = new pu6Entities();

                    try
                    {
                        FormsSZV_M_2016 SZVM = db_temp.FormsSZV_M_2016.First(x => x.ID == szvmID);

                        string regNum = Utils.ParseRegNum(SZVM.Insurer.RegNum);
                        Guid guid = Guid.NewGuid();

                        string fileName = String.Format("ПФР_{0}_{1}_СЗВ-М_{2}_{3}.XML", regNum, codeTOPFRTextBox.Text, DateTime.Now.ToString("yyyyMMdd"), guid);
                        XNamespace pfr = "http://пф.рф/ВС/СЗВ-М/2017-01-01";
                        XNamespace УТ = "http://пф.рф/унифицированныеТипы/2014-01-01";
                        XNamespace АФ = "http://пф.рф/АФ";
                        XNamespace АФ2 = "http://пф.рф/АФ/2017-01-01";
                        XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
                        XNamespace ds = "http://www.w3.org/2000/09/xmldsig#";
                        XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", null),
                                             new XElement(pfr + "ЭДПФР",
                                                 new XAttribute(XNamespace.Xmlns + "УТ", УТ.NamespaceName),
                                                 new XAttribute(XNamespace.Xmlns + "АФ", АФ.NamespaceName),
                                                 new XAttribute(XNamespace.Xmlns + "АФ2", АФ2.NamespaceName)));
                        //        new XAttribute(XNamespace.Xmlns + "ds", ds.NamespaceName),
                        //        new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                        //        new XAttribute(xsi + "schemaLocation", @"http://пф.рф/ВС/СЗВ-М/2016-01-01/Схемы/ВС/Входящие/СЗВ-М_2016-04-01.xsd")));

                        string typeInfo = "";

                        switch (SZVM.TypeInfo.Name)
                        {
                            case "Исходная":
                                typeInfo = "1";
                                break;
                            case "Корректирующая":
                                typeInfo = "2";
                                break;
                            case "Отменяющая":
                                typeInfo = "3";
                                break;
                        }

                        string orgName = "";

                        if (SZVM.Insurer.TypePayer == 0) // если организация
                        {
                            if (!String.IsNullOrEmpty(SZVM.Insurer.NameShort))
                            {
                                orgName = SZVM.Insurer.NameShort.ToUpper();
                            }
                            else if (!String.IsNullOrEmpty(SZVM.Insurer.Name))
                            {
                                orgName = SZVM.Insurer.Name.ToUpper();
                            }

                        }
                        else // если физ. лицо
                        {
                            orgName = SZVM.Insurer.LastName + " " + SZVM.Insurer.FirstName + " " + SZVM.Insurer.MiddleName;
                        }

                        if (!String.IsNullOrEmpty(orgName) && orgName.Length > 255)
                        {
                            orgName = orgName.Substring(0, 255);
                        }


                        XElement СЗВ_М = new XElement(pfr + "СЗВ-М",
                                                 new XElement(pfr + "ТипФормы", typeInfo),
                                                 new XElement(pfr + "Страхователь",
                                                    new XElement(pfr + "РегНомер", regNum),
                                                    new XElement(pfr + "НаименованиеКраткое", orgName.Trim()),
                                                    new XElement(pfr + "ИНН", !String.IsNullOrEmpty(SZVM.Insurer.INN.ToString()) ? SZVM.Insurer.INN.ToString() : "")),
                                                 new XElement(pfr + "ОтчетныйПериод",
                                                    new XElement(pfr + "Месяц", SZVM.MONTH),
                                                    new XElement(pfr + "КалендарныйГод", SZVM.YEAR)));

                        if (!String.IsNullOrEmpty(SZVM.Insurer.KPP))
                        {
                            СЗВ_М.Element(pfr + "Страхователь").Add(new XElement(pfr + "КПП", SZVM.Insurer.KPP.Substring(0, SZVM.Insurer.KPP.Length > 9 ? 9 : SZVM.Insurer.KPP.Length)));
                        }

                        #region СписокЗЛ

                        if (db_temp.FormsSZV_M_2016_Staff.Any(x => x.FormsSZV_M_2016_ID == szvmID))//SZVM.FormsSZV_M_2016_Staff.Any()
                        {
                            XElement СписокЗЛ = new XElement(pfr + "СписокЗЛ");

                            int i = 0;
                            List<StaffCont> staffL = db_temp.FormsSZV_M_2016_Staff.Where(x => x.FormsSZV_M_2016_ID == szvmID).Select(x => new StaffCont { LastName = x.Staff.LastName, FirstName = x.Staff.FirstName, MiddleName = x.Staff.MiddleName, InsuranceNumber = x.Staff.InsuranceNumber, ControlNumber = x.Staff.ControlNumber, INN = x.Staff.INN }).ToList();

                            staffL = staffL.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.MiddleName).ToList();
                            foreach (var item in staffL)
                            {
                                i++;
                                XElement ЗЛ = new XElement(pfr + "ЗЛ", new XAttribute("НомерПП", i));

                                XElement ФИО = new XElement(pfr + "ФИО");
                                if (!String.IsNullOrEmpty(item.LastName))
                                {
                                    ФИО.Add(new XElement(УТ + "Фамилия", item.LastName.Trim().ToUpper()));
                                }
                                if (!String.IsNullOrEmpty(item.FirstName))
                                {
                                    ФИО.Add(new XElement(УТ + "Имя", item.FirstName.Trim().ToUpper()));
                                }
                                if (!String.IsNullOrEmpty(item.MiddleName))
                                {
                                    ФИО.Add(new XElement(УТ + "Отчество", item.MiddleName.Trim().ToUpper()));
                                }
                                ЗЛ.Add(ФИО);


                                ЗЛ.Add(new XElement(pfr + "СНИЛС", Utils.ParseSNILS(item.InsuranceNumber, item.ControlNumber.HasValue ? item.ControlNumber.Value : (short)0)));

                                if (!String.IsNullOrEmpty(item.INN) && item.INN != "0")
                                    ЗЛ.Add(new XElement(pfr + "ИНН", item.INN.PadLeft(12, '0')));

                                СписокЗЛ.Add(ЗЛ);
                            }

                            СЗВ_М.Add(СписокЗЛ);
                        }

                        #endregion

                        СЗВ_М.Add(new XElement(pfr + "ДатаЗаполнения", SZVM.DateFilling.ToString("yyyy-MM-dd")));


                        xDoc.Element(pfr + "ЭДПФР").Add(СЗВ_М);



                        xDoc.Element(pfr + "ЭДПФР").Add(new XElement(pfr + "СлужебнаяИнформация",
                                                      new XElement(АФ + "GUID", guid),
                                                      new XElement(АФ + "ДатаВремя", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz")),
                                                      new XElement(АФ2 + "ПрограммаПодготовки", Application.ProductName.ToUpper() + " " + Application.ProductVersion)));

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

    //public class CustomXmlTextWriter : XmlTextWriter
    //{
    //    public CustomXmlTextWriter(string filename)
    //        : base(filename, Encoding.UTF8)
    //    {

    //    }

    //    public override void WriteStartDocument()
    //    {
    //        WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
    //    }

    //    public override void WriteEndDocument()
    //    {
    //    }
    //}
}
