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

namespace PU.FormsDSW3
{
    public partial class DSW3_CreateXML : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        pfrXMLEntities dbxml = new pfrXMLEntities();

        public FormsDSW_3 dsw3Data { get; set; }

        public DSW3_CreateXML()
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

        private void DSW3_CreateXML_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            DateUnderwrite.Value = DateTime.Now.Date;


            if (dsw3Data != null)
            {
                dsw3Number.Text = "№ " + dsw3Data.NUMBERPAYMENT + "  от " + dsw3Data.DATEPAYMENT.ToShortDateString();
            }
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (dsw3Data == null)
            {
                Methods.showAlert("Внимание!", "Не выбрана форма ДСВ-3!", this.ThemeName);
                return;
            }

            db = new pu6Entities();

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

            dbxml.ExecuteStoreCommand(String.Format("DELETE FROM xmlInfo WHERE ([Year] = {0} AND [InsurerID] = {1} AND [FormatType] = 'dsw3')", dsw3Data.YEAR, Options.InsID));


            var dsw3StaffList = db.FormsDSW_3_Staff.Where(x => x.FormsDSW_3_ID == dsw3Data.ID);


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

            var dsw3StaffList_ = dsw3StaffList.Select(p => new { form = p, Staff = p.Staff }).ToList().OrderBy(sorting).Select(p => p.form).ToList();

            try
            {
                this.Cursor = Cursors.WaitCursor;
                int num = (int)(numFrom.Value - 1);
                string regNum = Utils.ParseRegNum(db.Insurer.First(x => x.ID == Options.InsID).RegNum);


                double cnt = dsw3StaffList_.Count();

                double partCnt = splitChkBox.Checked ? (double)splitCntSpinEditor.Value : cnt;
                double packs_count_double = cnt / partCnt;
                int packs_cnt = Convert.ToInt16(Math.Ceiling(packs_count_double));
                int v = 0;
                pfrXMLEntities dbxmlTemp = new pfrXMLEntities();
                for (int j = 0; j < packs_cnt; j++)
                {


                    int cntP = (int)partCnt;
                    List<FormsDSW_3_Staff> dsw3StaffList_work = dsw3StaffList_.Skip(j * cntP).Take(cntP).ToList();
                    num++;

                    string fileName = String.Format("PFR-700-Y-{0}-ORG-{1}-DCK-{2}-DPT-{3}-DCK-{4}.XML", dsw3Data.YEAR.ToString(), regNum, num.ToString().PadLeft(5, '0'), "000000", "00000");


                    cntP = dsw3StaffList_work.Count();
                    xmlInfo xml_info = new xmlInfo
                    {
                        Num = num,
                        CountDoc = cntP,
                        CountStaff = cntP,
                        DocType = "ДСВ-3",
                        Year = dsw3Data.YEAR,
                        Quarter = 0,
                        DateCreate = DateUnderwrite.Value,
                        FileName = fileName,
                        UniqGUID = guid,
                        InsurerID = Options.InsID,
                        FormatType = "dsw3",
                        SourceID = dsw3Data.ID,
                        
                    };

                    int k = 0;
                    List<StaffList> staffList_ = new List<StaffList>();

                    foreach (var dsw3Item in dsw3StaffList_work)
                    {
                        k++;
                        var staffItem = dsw3Item.Staff;

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
                            InfoType = "",
                            DateCreate = DateUnderwrite.Value,
                            XmlInfoID = xml_info.ID,
                            FormsRSW_6_1_ID = dsw3Item.ID
                        };

                        staffList_.Add(staffListNewItem);


                    }
                    foreach (var item in staffList_)
                    {
                        xml_info.StaffList.Add(item);
                    }
                    dbxmlTemp.AddToxmlInfo(xml_info);
                    dbxmlTemp.SaveChanges();
                    v++;
                }
                dbxmlTemp.Dispose();

                //try
                //{
                //    foreach (var item in dbxml.xmlInfo.Where(x => x.UniqGUID == guid))
                //    {
                //        string xml = generateXML_DSW3(item);

                //        xmlFile xmlFile_ = new xmlFile
                //        {
                //            XmlContent = xml,
                //            XmlInfoID = item.ID
                //        };

                //        dbxml.xmlFile.AddObject(xmlFile_);
                //        dbxml.SaveChanges();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    RadMessageBox.Show("Ошибка при формировании файла XML Формы ДСВ-3. Код ошибки: " + ex.Message, "Внимание!");
                //    this.Cursor = Cursors.Default;
                //    db.Dispose();
                //    return;
                //}
                //finally
                //{

                //}


            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Ошибка при подготовке к формированию файла XML Формы ДСВ-3. Код ошибки: " + ex.Message, "Внимание!");
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
                    Methods.showAlert("Информация", "Формирование пачек ДСВ-3 успешно завершено.", this.ThemeName);
                    viewPacks_Click(null, null);
                }
            }

        }

        private string generateXML_DSW3(xmlInfo item)
        {
            pu6Entities db_temp = new pu6Entities();

            if (!db_temp.FormsDSW_3.Any(x => x.ID == item.SourceID))
            {
                return "";
            }

            string xml = "";
            XNamespace pfr = "http://schema.pfr.ru";
            int num = 1;
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion)),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"), new XAttribute("ДобровольныеПравоотношения", "ДСВ"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА = new XElement("ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА",
                                                                  new XElement("НомерВпачке", num),
                                                                  new XElement("ТипВходящейОписи", "ОПИСЬ ПАЧКИ"));

            Insurer ins = db.Insurer.FirstOrDefault(x => x.ID == Options.InsID);

            XElement СоставительПачки = new XElement("СоставительПачки",
                                            new XElement("НалоговыйНомер",
                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));

            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СоставительПачки.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRIP))
            {
                СоставительПачки.Add(new XElement("КодЕГРИП", ins.EGRIP.Substring(0, ins.EGRIP.Length > 15 ? 15 : ins.EGRIP.Length)));
            }

            if (!String.IsNullOrEmpty(ins.EGRUL))
            {
                СоставительПачки.Add(new XElement("КодЕГРЮЛ", ins.EGRUL.Substring(0, ins.EGRUL.Length > 15 ? 15 : ins.EGRUL.Length)));
            }

            if (!String.IsNullOrEmpty(ins.OrgLegalForm))
            {
                СоставительПачки.Add(new XElement("Форма", ins.OrgLegalForm.Substring(0, ins.OrgLegalForm.Length > 40 ? 40 : ins.OrgLegalForm.Length).ToUpper()));
            }

            string NameShort = "";

            if (ins.TypePayer == 0) // если организация
            {
                if (!String.IsNullOrEmpty(ins.Name))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.Name.Substring(0, ins.Name.Length > 100 ? 100 : ins.Name.Length).ToUpper()));
                }
                else if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", ins.NameShort.Substring(0, ins.NameShort.Length > 100 ? 100 : ins.NameShort.Length).ToUpper()));
                }

                if (!String.IsNullOrEmpty(ins.NameShort))
                {
                    NameShort = ins.NameShort.Substring(0, ins.NameShort.Length > 50 ? 50 : ins.NameShort.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }
            else // если физ. лицо
            {
                string FIO = "";
                if (!String.IsNullOrEmpty(ins.LastName))
                {
                    FIO = FIO + ins.LastName;
                }
                if (!String.IsNullOrEmpty(ins.FirstName))
                {
                    FIO = FIO + " " + ins.FirstName;
                }
                if (!String.IsNullOrEmpty(ins.MiddleName))
                {
                    FIO = FIO + " " + ins.MiddleName;
                }

                if (!String.IsNullOrEmpty(FIO))
                {
                    СоставительПачки.Add(new XElement("НаименованиеОрганизации", FIO.Substring(0, FIO.Length > 100 ? 100 : FIO.Length).ToUpper()));
                    NameShort = FIO.Substring(0, FIO.Length > 50 ? 50 : FIO.Length);
                    СоставительПачки.Add(new XElement("НаименованиеКраткое", NameShort.ToUpper()));
                }
            }

            СоставительПачки.Add(new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)));

            //<xsd:element name="Подразделение" type="ТипПодразделение" minOccurs="0"/>
            //<xsd:element name="НомерЛицензии" type="xsd:string" minOccurs="0"/>
            //<xsd:element name="ДатаВыдачиЛицензии" type="ТипДата" minOccurs="0"/>



            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(СоставительПачки);

            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

                        //<xsd:element name="ПоПодразделению" type="ТипНомерПачки"/>


            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToString("dd.MM.yyyy")));

            var dsw3Data_ = db_temp.FormsDSW_3.FirstOrDefault(x => x.ID == item.SourceID);


            XElement РеестрДСВ = new XElement("РеестрДСВ",
                                                           new XElement("ПлатежноеПоручение",
                                                               new XElement("ДатаПоручения", dsw3Data_.DATEPAYMENT.ToString("dd.MM.yyyy")),
                                                               new XElement("НомерПоручения", dsw3Data_.NUMBERPAYMENT),
                                                               new XElement("ДатаИсполненияПоручения", dsw3Data.DATEEXECUTPAYMENT.ToString("dd.MM.yyyy"))),
                                                           new XElement("Год", dsw3Data_.YEAR),
                                                           new XElement("КоличествоСтрок", item.CountStaff.Value.ToString()),
                                                           new XElement("СуммаДСВРаботника", Utils.decToStr(dsw3Data_.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_EMPLOYERS.Value))),
                                                           new XElement("СуммаДСВРаботодателя", Utils.decToStr(dsw3Data_.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_PAYER.Value))),
                                                           new XElement("СуммаДСВОбщая", Utils.decToStr(dsw3Data_.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_PAYER.Value) + dsw3Data_.FormsDSW_3_Staff.Sum(x => x.SUMFEEPFR_EMPLOYERS.Value))));
            ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА.Add(РеестрДСВ);

            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ_РЕЕСТРА);

            var dsw3staffListTemp = db_temp.FormsDSW_3_Staff.Where(x => x.FormsDSW_3_ID == dsw3Data_.ID);
            var dsw3staffIDListTemp = dsw3staffListTemp.Select(x => x.StaffID).ToList();
            var staffListTemp = db_temp.Staff.Where(x => dsw3staffIDListTemp.Contains(x.ID));

            foreach (var staff in item.StaffList)
            {
                num++; // номер в пачке

                FormsDSW_3_Staff dsw3staff = dsw3staffListTemp.First(x => x.ID == staff.FormsRSW_6_1_ID.Value);
                Staff ish_staff = staffListTemp.First(x => x.ID == dsw3staff.StaffID);

                XElement Работодатель = new XElement(СоставительПачки);
                Работодатель.Name = "Работодатель"; 

                XElement РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ = new XElement("РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ",
                                                        new XElement("НомерВпачке", num),
                                                        new XElement("СтраховойНомер", Utils.ParseSNILS(ish_staff.InsuranceNumber, ish_staff.ControlNumber)),
                                                        new XElement("ФИО",
                                                            new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                            new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                            new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())),
                                                        Работодатель,
                                                        new XElement("СуммаДСВРаботника", Utils.decToStr(dsw3staff.SUMFEEPFR_EMPLOYERS.Value)),
                                                        new XElement("СуммаДСВРаботодателя", Utils.decToStr(dsw3staff.SUMFEEPFR_PAYER.Value))
                                                        );


                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(РЕЕСТР_ДСВ_РАБОТОДАТЕЛЬ);
            }




            xDoc.Root.SetDefaultXmlNamespace(pfr);

            xml = xDoc.ToString();

            db_temp.Dispose();
            return xml;
        }

        private void viewPacks_Click(object sender, EventArgs e)
        {
            rsw2014packs child = new rsw2014packs();

            child.ident.Year = dsw3Data.YEAR;
            child.ident.Quarter = (byte)0;
            child.ident.InsurerID = Options.InsID;
            child.ident.FormatType = "dsw3";
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
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

        private void splitChkBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            splitCntSpinEditor.Enabled = splitChkBox.Checked;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
