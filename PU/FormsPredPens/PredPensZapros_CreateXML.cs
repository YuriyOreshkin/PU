using PU.Classes;
using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Classes;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Schema;
using System.Xml;

namespace PU.FormsPredPens
{
    public partial class PredPensZapros_CreateXML : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public long predPensID = 0;

        public PredPensZapros_CreateXML()
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

        private void PredPensZapros_CreateXML_Load(object sender, EventArgs e)
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

        private void PredPensZapros_CreateXML_FormClosing(object sender, FormClosingEventArgs e)
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
            public DateTime dateBirth { get; set; }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
                        if (pathBrowser.Value != null)
            {
                if (db.FormsPredPens_Zapros.Any(x => x.ID == predPensID))
                {
                    if (db.FormsPredPens_Zapros_Staff.Any(x => x.FormsPredPens_ZaprosID == predPensID && !x.Staff.DateBirth.HasValue))
                    { 
                        MessageBox.Show("В указанном списке граждан есть записи у которых не заподнена ДатаРождения! Необходимо исправить ошибку!");
                        return;
                    }

                        pu6Entities db_temp = new pu6Entities();

                        try
                        {
                            FormsPredPens_Zapros PredPens = db_temp.FormsPredPens_Zapros.First(x => x.ID == predPensID);

                            string regNum = Utils.ParseRegNum(PredPens.Insurer.RegNum);
                            Guid guid = Guid.NewGuid();


                            string fileName = "";

                            if (PredPens.TypeORG == 3)  // Работодатель
                            {
                                fileName = String.Format("ПФР_{0}_{1}_ЗППВ_{2}_{3}.XML", regNum, codeTOPFRTextBox.Text, DateTime.Now.ToString("yyyyMMdd"), guid);
                            }
                            else if (PredPens.TypeORG == 2) // орган в области содействия занятости населения
                            {
                                fileName = String.Format("ПФР_{0}_ЗППВ_{1}_{2}.XML", codeTOPFRTextBox.Text, DateTime.Now.ToString("yyyyMMdd"), guid);
                            }

                            XNamespace pfr = "http://пф.рф/ВВ/ПУ/ЗППВ/2018-10-19";
                            XNamespace УТ2 = "http://пф.рф/УТ/2017-08-21";
                            XNamespace АФ4 = "http://пф.рф/АФ/2017-08-21";
                            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";


                            XDocument xDoc = new XDocument(new XDeclaration("1.0", "UTF-8", null),
                     new XElement(pfr + "ЭДПФР",
                         new XAttribute(XNamespace.Xmlns + "УТ2", УТ2.NamespaceName),
                         new XAttribute(XNamespace.Xmlns + "АФ4", АФ4.NamespaceName),
                         new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName)));


                            XElement ЗППВ = new XElement(pfr + "ЗППВ",
                                new XElement(pfr + "Орган", PredPens.TypeORG));

                            if (PredPens.TypeORG == 2 || PredPens.TypeORG == 3)
                            {
                                if (PredPens.Insurer.TypePayer == 0) // если организация
                                {
                                    if (!String.IsNullOrEmpty(PredPens.Insurer.NameShort))
                                    {
                                        ЗППВ.Add(new XElement(pfr + "НаименованиеОрганизации", PredPens.Insurer.Name.Trim()));
                                    }
                                    else if (!String.IsNullOrEmpty(PredPens.Insurer.Name))
                                    {
                                        ЗППВ.Add(new XElement(pfr + "НаименованиеОрганизации", PredPens.Insurer.NameShort.Trim()));
                                    }
                                }
                                else // если физ. лицо
                                {
                                    string FIO = "";
                                    if (!String.IsNullOrEmpty(PredPens.Insurer.LastName.Trim()))
                                    {
                                        FIO = PredPens.Insurer.LastName.Trim();
                                    }
                                    if (!String.IsNullOrEmpty(PredPens.Insurer.FirstName.Trim()))
                                    {
                                        FIO = FIO + " " + PredPens.Insurer.FirstName.Trim();
                                    }
                                    if (!String.IsNullOrEmpty(PredPens.Insurer.MiddleName.Trim()))
                                    {
                                        FIO = FIO + " " + PredPens.Insurer.MiddleName.Trim();
                                    }

                                    if (!String.IsNullOrEmpty(FIO))
                                    {
                                        ЗППВ.Add(new XElement(pfr + "НаименованиеОрганизации", FIO));
                                    }
                                }



                            }

                            ЗППВ.Add(new XElement(pfr + "Дата", PredPens.Date.ToString("yyyy-MM-dd")));

                            if (PredPens.TypeORG == 2 || PredPens.TypeORG == 3)
                            {
                                ЗППВ.Add(new XElement(pfr + "Номер", PredPens.Number));
                            }

                            #region СписокЗЛ

                            if (db_temp.FormsPredPens_Zapros_Staff.Any(x => x.FormsPredPens_ZaprosID == predPensID))
                            {

                                int i = 0;
                                List<StaffCont> staffL = db_temp.FormsPredPens_Zapros_Staff.Where(x => x.FormsPredPens_ZaprosID == predPensID).Select(x => new StaffCont { LastName = x.Staff.LastName, FirstName = x.Staff.FirstName, MiddleName = x.Staff.MiddleName, InsuranceNumber = x.Staff.InsuranceNumber, ControlNumber = x.Staff.ControlNumber, dateBirth = x.Staff.DateBirth.Value }).ToList();

                                staffL = staffL.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ThenBy(x => x.MiddleName).ToList();
                                foreach (var item in staffL)
                                {
                                    i++;
                                    XElement Гражданин = new XElement(pfr + "Гражданин");

                                    XElement ФИО = new XElement(УТ2 + "ФИО");
                                    if (!String.IsNullOrEmpty(item.LastName))
                                    {
                                           ФИО.Add(new XElement(УТ2 + "Фамилия", item.LastName.Trim()));
                                    }
                                    if (!String.IsNullOrEmpty(item.FirstName))
                                    {
                                        ФИО.Add(new XElement(УТ2 + "Имя", item.FirstName.Trim()));
                                    }
                                    if (!String.IsNullOrEmpty(item.MiddleName))
                                    {
                                        ФИО.Add(new XElement(УТ2 + "Отчество", item.MiddleName.Trim()));
                                    }
                                    Гражданин.Add(ФИО);

                                    Гражданин.Add(new XElement(УТ2 + "ДатаРождения", item.dateBirth.ToString("yyyy-MM-dd")));

                                    Гражданин.Add(new XElement(УТ2 + "СНИЛС", Utils.ParseSNILS(item.InsuranceNumber, item.ControlNumber.HasValue ? item.ControlNumber.Value : (short)0)));


                                    ЗППВ.Add(Гражданин);
                                }

                            }

                            #endregion

                            XElement Подписант = new XElement(pfr + "Подписант");
                            if (!String.IsNullOrEmpty(PredPens.LastName))
                            {
                                Подписант.Add(new XElement(УТ2 + "Фамилия", PredPens.LastName.Trim()));
                            }
                            if (!String.IsNullOrEmpty(PredPens.FirstName))
                            {
                                Подписант.Add(new XElement(УТ2 + "Имя", PredPens.FirstName.Trim()));
                            }
                            if (!String.IsNullOrEmpty(PredPens.MiddleName))
                            {
                                Подписант.Add(new XElement(УТ2 + "Отчество", PredPens.MiddleName.Trim()));
                            }

                            ЗППВ.Add(Подписант);

                            xDoc.Element(pfr + "ЭДПФР").Add(ЗППВ);


                            xDoc.Element(pfr + "ЭДПФР").Add(new XElement(pfr + "СлужебнаяИнформация",
                              new XElement(АФ4 + "GUID", guid),
                              new XElement(АФ4 + "ДатаВремя", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"))));

                            string filePath = pathBrowser.Value.ToString() + "\\" + fileName;
                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }

                            using (var writer = new XmlTextWriter(filePath, new UTF8Encoding(false)) { Formatting = Formatting.Indented })
                            {
                                xDoc.Save(writer);
                            }


                            Methods.showAlert("Сохранение", "XML-файл успешно сохранен по указанному пути!", this.ThemeName);



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
                    RadMessageBox.Show(this, "Не удалось загрузить данные Запроса из базы данных!", "Внимание");
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
