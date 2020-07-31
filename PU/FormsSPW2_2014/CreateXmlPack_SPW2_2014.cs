using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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

namespace PU.FormsSPW2_2014
{
    public partial class CreateXmlPack_SPW2_2014 : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        pfrXMLEntities dbxml = new pfrXMLEntities();

        public List<long> staffList { get; set; }
        public long currentStaffId = 0;
        public PlatCategory PlatCat { get; set; }


        public CreateXmlPack_SPW2_2014()
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

        private void viewPacks_Click(object sender, EventArgs e)
        {
            rsw2014packs child = new rsw2014packs();

            child.ident.Year = Year.SelectedItem != null ? short.Parse(Year.SelectedItem.Value.ToString()) : (short)0;
            child.ident.Quarter = Quarter.SelectedItem != null ? byte.Parse(Quarter.SelectedItem.Value.ToString()) : (byte)0;
            child.ident.InsurerID = Options.InsID;
            child.ident.FormatType = "spw2_2014";
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        private void CreateXmlPack_SPW2_2014_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            var avail_periods = Options.RaschetPeriodInternal.Where(x => x.Year >= 2014).OrderBy(x => x.Year);
            DateUnderwrite.EditValue = DateTime.Now.Date;

            // выпад список "календарный год" + корректирующий

            this.Year.Items.Clear();
            this.YearKorr.Items.Clear();

            foreach (var item in avail_periods.Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
                YearKorr.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }

            if (Year.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                Year.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                Year.Items.OrderByDescending(x => x.Value).First().Selected = true;

            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                if (Quarter.Items.Count() > 0)
                {
                    DateTime dt = DateTime.Now.AddDays(-40);
                    RaschetPeriodContainer rp = new RaschetPeriodContainer();

                    if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                    else
                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                    if (rp != null && Quarter.Items.Any(x => x.Value.ToString() == rp.Kvartal.ToString()))
                    {
                        Quarter.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                    }
                    else
                    {
                        Quarter.Items.First().Selected = true;
                    }
                }

            }


            if (YearKorr.Items.Any(x => x.Text.ToString() == DateTime.Now.Year.ToString()))
                YearKorr.Items.Single(x => x.Text.ToString() == DateTime.Now.Year.ToString()).Selected = true;
            else
                YearKorr.Items.OrderByDescending(x => x.Value).First().Selected = true;

            if (short.TryParse(YearKorr.SelectedItem.Text, out y))
            {
                foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year == y).ToList())
                {
                    QuarterKorr.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                if (QuarterKorr.Items.Count() > 0)
                {
                    DateTime dt = DateTime.Now;
                    RaschetPeriodContainer rp = new RaschetPeriodContainer();

                    if (Options.RaschetPeriodInternal.Any(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0))
                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal != 0);
                    else
                        rp = Options.RaschetPeriodInternal.FirstOrDefault(x => x.DateBegin <= dt && x.DateEnd >= dt && x.Year == y && x.Kvartal == 0);

                    if (rp != null && QuarterKorr.Items.Any(x => x.Value.ToString() == rp.Kvartal.ToString()))
                    {
                        QuarterKorr.Items.Single(x => x.Value.ToString() == rp.Kvartal.ToString()).Selected = true;
                    }
                    else
                    {
                        QuarterKorr.Items.First().Selected = true;
                    }
                }

            }

            if (Options.saveLastPackNum)
            {
                try // пробуем восстановить последний номер пачки
                {
                    Props props = new Props(); //экземпляр класса с настройками
                    numFrom.Value = props.getPackNum(this.Name);
                }
                catch { }
            }

            this.Year.SelectedIndexChanged += (s, с) => Year_SelectedIndexChanged();
            this.YearKorr.SelectedIndexChanged += (s, с) => YearKorr_SelectedIndexChanged();
            this.TypeInfoDDL.SelectedIndexChanged += (s, с) => TypeInfoDDL_SelectedIndexChanged();
            TypeInfoDDL_SelectedIndexChanged();
        }

        private void Year_SelectedIndexChanged()
        {
            byte q = 20;
            if (Quarter.SelectedItem != null && byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q)) { }

            this.Quarter.Items.Clear();
            short y;
            if (short.TryParse(Year.SelectedItem.Text, out y))
            {
                foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year == y).ToList())
                {
                    Quarter.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                if (Quarter.Items.Count() > 0)
                {
                    if (q != 20 && Quarter.Items.Any(x => x.Value.ToString() == q.ToString()))
                        Quarter.Items.FirstOrDefault(x => x.Value.ToString() == q.ToString()).Selected = true;
                    else
                        Quarter.Items.First().Selected = true;
                }

            }
        }

        private void YearKorr_SelectedIndexChanged()
        {
            byte q = 20;
            if (QuarterKorr.SelectedItem != null && byte.TryParse(QuarterKorr.SelectedItem.Value.ToString(), out q)) { }

            this.QuarterKorr.Items.Clear();
            short y;
            if (short.TryParse(YearKorr.SelectedItem.Text, out y))
            {
                foreach (var item in Options.RaschetPeriodInternal.Where(x => x.Year == y).ToList())
                {
                    QuarterKorr.Items.Add(new RadListDataItem(item.Kvartal + " - " + item.Name, item.Kvartal));
                }
                if (QuarterKorr.Items.Count() > 0)
                {
                    if (q != 20 && QuarterKorr.Items.Any(x => x.Value.ToString() == q.ToString()))
                        QuarterKorr.Items.FirstOrDefault(x => x.Value.ToString() == q.ToString()).Selected = true;
                    else
                        QuarterKorr.Items.First().Selected = true;
                }

            }
        }
        private void TypeInfoDDL_SelectedIndexChanged()
        {
            bool flag = true;
            if (TypeInfoDDL.SelectedIndex > 0)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            korrPeriodGroupBox.Enabled = flag;
        }

        private void catPanel_MouseHover(object sender, EventArgs e)
        {
            catPanel.Font = new Font("Segoe UI", 9, FontStyle.Underline | FontStyle.Bold);
        }

        private void catPanel_MouseLeave(object sender, EventArgs e)
        {
            catPanel.Font = new Font("Segoe UI", 9, FontStyle.Bold);

        }

        private void catPanel_MouseClick(object sender, MouseEventArgs e)
        {
            PlatCategoryFrm child = new PlatCategoryFrm();
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.PlatCat = PlatCat;
            child.action = "selection";
            child.ShowDialog();
            PlatCat = child.PlatCat;
            if (PlatCat != null)
            {
                catPanel.Text = PlatCat.Code + "   " + PlatCat.Name;
            }
        }

        private void allCatChkBox_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            catPanel.Enabled = !allCatChkBox.Checked;

            if (allCatChkBox.Checked)
            {
                catPanel.Text = "По всем Категориям плательщика...";
                PlatCat = null;
            }
            else
            {
                catPanel.Text = "Категория плательщика - не определена... Нажмите для выбора";
                catPanel_MouseClick(null, null);
            }

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool validation()
        {
            bool result = true;
            if (TypeInfoDDL.SelectedIndex > 0)
            {
                if (YearKorr.Text == "" || QuarterKorr.Text == "")
                    result = false;
            }

            if (Year.Text == "" || Quarter.Text == "")
                result = false;

            if (!allCatChkBox.Checked && PlatCat == null)
                result = false;

            return result;
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!validation())
                return;

            try
            {
                if (((switchStaffListDDL.SelectedIndex == 2) && (staffList == null || (staffList != null && staffList.Count <= 0))) || (switchStaffListDDL.SelectedIndex == 1 && currentStaffId == 0))
                {
                    RadMessageBox.Show("Пустой список сотрудников для формирования! Необходимо выбрать сотрудников!", "Внимание");
                    return;
                }

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


                short y_;
                if (short.TryParse(Year.Text, out y_))
                {
                    if (Quarter.SelectedItem != null)
                    {
                        byte q_;
                        if (byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q_))
                        {
                            dbxml.Database.ExecuteSqlCommand(String.Format("DELETE FROM xmlInfo WHERE ([Year] = {0} AND [Quarter] = {1} AND [InsurerID] = {2} AND [FormatType] = 'spw2_2014')", y_, q_, Options.InsID));
                        }
                    }
                }



                xmlInfo xml_info;
                string fileName;
                string regNum = Utils.ParseRegNum(db.Insurer.First(x => x.ID == Options.InsID).RegNum);
                string[] infoTypeStr = new string[] { "ИСХД", "КОРР", "ОТМН" };


                short y = short.Parse(Year.Text);
                byte q = byte.Parse(Quarter.SelectedItem.Value.ToString());

                List<FormsSPW2> spw2List = new List<FormsSPW2>();
                List<List<FormsSPW2>> spw2List_part = new List<List<FormsSPW2>>();
                List<RaschetPeriod> korrPeriods = new List<RaschetPeriod>();

                switch (TypeInfoDDL.SelectedIndex)
                {
                    case 0:  // исходные сведения
                        spw2List = db.FormsSPW2.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == q && x.TypeInfoID == 1).ToList();
                        break;
                    case 1: // корректирующие
                        spw2List = db.FormsSPW2.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == q && x.TypeInfoID == 2).ToList();
                        break;
                    case 2: // отменяющие
                        spw2List = db.FormsSPW2.Where(x => x.InsurerID == Options.InsID && x.Year == y && x.Quarter == q && x.TypeInfoID == 3).ToList();
                        break;
                }


                switch (switchStaffListDDL.SelectedIndex)
                {
                    case 1: //текущая запись
                        spw2List = spw2List.Where(x => x.StaffID == currentStaffId).ToList();
                        break;
                    case 2: //по выделенным записям
                        spw2List = spw2List.Where(x => staffList.Contains(x.StaffID)).ToList();
                        break;
                }


                switch (sortingDDL.SelectedIndex)
                {
                    case 0: //сортировка по ФИО
                        spw2List = spw2List.OrderBy(x => x.Staff.LastName).ThenBy(x => x.Staff.FirstName).ThenBy(x => x.Staff.MiddleName).ToList();
                        break;
                    case 1: //по страх. номеру
                        spw2List = spw2List.OrderBy(x => x.Staff.InsuranceNumber).ToList();
                        break;
                    case 2: //по табелю
                        spw2List = spw2List.OrderBy(x => x.Staff.TabelNumber).ToList();
                        break;
                }

                List<long> idCatList = new List<long>();
                if (allCatChkBox.Checked)  // если выбрано По всем категориям
                {
                    idCatList = spw2List.Select(x => x.PlatCategoryID.Value).Distinct().ToList();
                }
                else // по конкретной категории
                {
                    idCatList.Add(PlatCat.ID);
                }

                int num = (int)(numFrom.Value - 1);
                foreach (var item in idCatList)
                {
                    var spw2List_item = spw2List.Where(x => x.PlatCategoryID == item);

                    double cnt = spw2List_item.Count();
                    double packs_count_double = cnt / 200;
                    int packs_cnt = Convert.ToInt16(Math.Ceiling(packs_count_double));

                    numFrom.Value += packs_cnt;

                    for (int j = 0; j < packs_cnt; j++)
                    {
                        List<FormsSPW2> spw2List_work = spw2List_item.Skip(j * 200).Take(200).ToList();
                        num++;
                        fileName = String.Format("PFR-700-Y-{0}-ORG-{1}-DCK-{2}-DPT-{3}-DCK-{4}.XML", y.ToString(), regNum, num.ToString().PadLeft(5, '0'), "000000", "00000");

                        xml_info = new xmlInfo
                        {
                            Num = num,
                            CountDoc = spw2List_work.Count(),
                            CountStaff = spw2List_work.Count(),
                            DocType = "СПВ-2",
                            Year = y,
                            Quarter = q,
                            DateCreate = DateUnderwrite.DateTime,
                            FileName = fileName,
                            UniqGUID = guid,
                            InsurerID = Options.InsID,
                            FormatType = "spw2_2014"
                        };

                        if (spw2List_work.First().YearKorr != null && spw2List_work.First().YearKorr != 0)  // если отменяющая или корректирующая пачка, то пишем корр период
                        {
                            xml_info.YearKorr = spw2List_work.First().YearKorr;
                            xml_info.QuarterKorr = spw2List_work.First().QuarterKorr;
                        }

                        dbxml.xmlInfo.Add(xml_info);
                        dbxml.SaveChanges();


                        int k = 0;
                        foreach (var spw2Item in spw2List_work)
                        {
                            k++;
                            var staffItem = spw2Item.Staff;

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
                                InfoType = infoTypeStr[spw2Item.TypeInfoID - 1],
                                DateCreate = DateUnderwrite.DateTime,
                                XmlInfoID = xml_info.ID,
                                FormsRSW_6_1_ID = spw2Item.ID
                            };

                            dbxml.StaffList.Add(staffListNewItem);


                        }

                        dbxml.Entry(xml_info).State = EntityState.Modified;
                        dbxml.SaveChanges();
                    }


                }

                Methods.showAlert("Информация", "Предварительное формирование пачек СПВ-2 успешно завершено.", this.ThemeName);
                viewPacks_Click(null, null);

                //foreach (var item in dbxml.xmlInfo.Where(x => x.UniqGUID == guid))
                //{
                //    string xml = generateXML_SPW2_2014(item);

                //    xmlFile xmlFile_ = new xmlFile
                //    {
                //        XmlContent = xml,
                //        XmlInfoID = item.ID
                //    };

                //    dbxml.xmlFile.Add(xmlFile_);
                //    dbxml.SaveChanges();
                //}
            }
            catch (Exception ex)
            {
                Methods.showAlert("Ошибка", "Во время формирования пачек СПВ-2 произошла ошибка. Код ошибки: " + ex.Message, this.ThemeName);
            }
            finally
            {
                //if (dbxml.xmlInfo.Any(x => x.UniqGUID == guid))
                //{
                //}
            }

        }

        private string generateXML_SPW2_2014(xmlInfo item)
        {
            string xml = "";
            //            FormsRSW2014_1_1 rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == item.SourceID);
            XNamespace pfr = "http://schema.pfr.ru";
            int num = 1;
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "Windows-1251", "yes"),
                new XElement("ФайлПФР", new XElement("ИмяФайла", item.FileName),
                                        new XElement("ЗаголовокФайла",
                                            new XElement("ВерсияФормата", "07.00"),
                                            new XElement("ТипФайла", "ВНЕШНИЙ"),
                                            new XElement("ПрограммаПодготовкиДанных",
                                                new XElement("НазваниеПрограммы", Application.ProductName.ToUpper()),
                                                new XElement("Версия", Application.ProductVersion.Substring(2, Application.ProductVersion.Length - 2))),
                                            new XElement("ИсточникДанных", "СТРАХОВАТЕЛЬ")),
                                        new XElement("ПачкаВходящихДокументов", new XAttribute("Окружение", "В составе файла"), new XAttribute("Стадия", "До обработки"))));

            XElement ВХОДЯЩАЯ_ОПИСЬ = new XElement("ВХОДЯЩАЯ_ОПИСЬ",
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


            ВХОДЯЩАЯ_ОПИСЬ.Add(СоставительПачки);
            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("НомерПачки",
                                                           new XElement("Основной", item.Num.Value.ToString().PadLeft(5, '0'))));

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("СоставДокументов",
                                                           new XElement("Количество", "1"),
                                                           new XElement("НаличиеДокументов",
                                                               new XElement("ТипДокумента", "СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ"),
                                                               new XElement("Количество", item.CountStaff.Value.ToString()))));

            ВХОДЯЩАЯ_ОПИСЬ.Add(new XElement("ДатаСоставления", item.DateCreate.Value.ToShortDateString()));


            xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(ВХОДЯЩАЯ_ОПИСЬ);


            #region СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ

            string InfoType = "";
            switch (item.StaffList.First().InfoType)
            {
                case "ИСХД":
                    InfoType = "ИСХОДНАЯ";
                    break;
                case "КОРР":
                    InfoType = "КОРРЕКТИРУЮЩАЯ";
                    break;
                case "ОТМН":
                    InfoType = "ОТМЕНЯЮЩАЯ";
                    break;
            }

            long spw2Id = item.StaffList.First().FormsRSW_6_1_ID.Value;
            FormsSPW2 spw2 = db.FormsSPW2.FirstOrDefault(x => x.ID == spw2Id);
            string codeCat = spw2.PlatCategory.Code;

            string q = "";

            switch (spw2.Quarter)
            {
                case 0:
                    q = "31.12.";
                    break;
                case 3:
                    q = "31.03.";
                    break;
                case 6:
                    q = "30.06.";
                    break;
                case 9:
                    q = "30.09.";
                    break;
            }
            string periodName = String.Format("С 01.01.{0} ПО {1}{0}", spw2.Year, q);
            string periodNameKorr = "";

            if (spw2.YearKorr != null && spw2.YearKorr != 0)
            {
                q = "";
                switch (spw2.QuarterKorr)
                {
                    case 0:
                        q = "31.12.";
                        break;
                    case 3:
                        q = "31.03.";
                        break;
                    case 6:
                        q = "30.06.";
                        break;
                    case 9:
                        q = "30.09.";
                        break;
                }


                periodNameKorr = String.Format("С 01.01.{0} ПО {1}{0}", spw2.YearKorr, q);
            }

            

            foreach (var staff in item.StaffList) // перебираем всех сотрудников попавших в эту пачку
            {
                num++; // номер в пачке

                spw2 = db.FormsSPW2.FirstOrDefault(x => x.ID == staff.FormsRSW_6_1_ID);
                Staff ish_staff = db.Staff.FirstOrDefault(x => x.ID == staff.StaffID);
                string contrNum = "";
                if (ish_staff.ControlNumber != null)
                {
                    contrNum = ish_staff.ControlNumber.HasValue ? ish_staff.ControlNumber.Value.ToString().PadLeft(2, '0') : "";
                }


                XElement СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ = new XElement("СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ",
                                                                            new XElement("НомерВпачке", num),
                                                                            new XElement("ТипСведений", InfoType),
                                                                            new XElement("РегистрационныйНомер", Utils.ParseRegNum(ins.RegNum)),
                                                                            new XElement("НаименованиеКраткое", NameShort),
                                                                            new XElement("НалоговыйНомер",
                                                                                new XElement("ИНН", !String.IsNullOrEmpty(ins.INN.ToString()) ? ins.INN.ToString() : "")));


            if (!String.IsNullOrEmpty(ins.KPP))
            {
                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Element("НалоговыйНомер").Add(new XElement("КПП", ins.KPP.Substring(0, ins.KPP.Length > 9 ? 9 : ins.KPP.Length)));
            }
                
                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("КодКатегории", codeCat));




                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ОтчетныйПериод",
                                                               new XElement("Квартал", spw2.Quarter),
                                                               new XElement("Год", spw2.Year),
                                                               new XElement("Название", periodName.ToUpper())));

                if (spw2.YearKorr.HasValue)
                {
                    СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("КорректируемыйОтчетныйПериод",
                                                                   new XElement("Квартал", spw2.QuarterKorr.Value),
                                                                   new XElement("Год", spw2.YearKorr.Value),
                                                                   new XElement("Название", periodNameKorr.ToUpper())));
                }

                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("СтраховойНомер", !String.IsNullOrEmpty(ish_staff.InsuranceNumber) ? ish_staff.InsuranceNumber.Substring(0, 3) + "-" + ish_staff.InsuranceNumber.Substring(3, 3) + "-" + ish_staff.InsuranceNumber.Substring(6, 3) + " " + contrNum : ""),
                                                                            new XElement("ФИО",
                                                                                new XElement("Фамилия", ish_staff.LastName.Substring(0, ish_staff.LastName.Length > 40 ? 40 : ish_staff.LastName.Length).ToUpper()),
                                                                                new XElement("Имя", ish_staff.FirstName.Substring(0, ish_staff.FirstName.Length > 40 ? 40 : ish_staff.FirstName.Length).ToUpper()),
                                                                                new XElement("Отчество", ish_staff.MiddleName.Substring(0, ish_staff.MiddleName.Length > 40 ? 40 : ish_staff.MiddleName.Length).ToUpper())));

                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ДатаЗаполнения", spw2.DateFilling.ToShortDateString()));
                СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ДатаСоставленияНа", spw2.DateComposit.ToShortDateString()));



                var staj_osn_list = spw2.StajOsn.OrderBy(x => x.Number.Value);
                int i = 0;

                foreach (var staj_osn in staj_osn_list)
                {
                    i++;
                    XElement СтажевыйПериод = createStajElement(staj_osn, i);
                    СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(СтажевыйПериод);
                }


                if (InfoType != "ОТМЕНЯЮЩАЯ")
                {
                    СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ПризнакНачисленияВзносовОПС", (spw2.ExistsInsurOPS.HasValue && spw2.ExistsInsurOPS.Value == 1) ? "ДА" : "НЕТ"));
                    СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ.Add(new XElement("ПризнакНачисленияВзносовПоДопТарифу", (spw2.ExistsInsurDop.HasValue && spw2.ExistsInsurDop.Value == 1) ? "ДА" : "НЕТ"));
                }

                xDoc.Element("ФайлПФР").Element("ПачкаВходящихДокументов").Add(СВЕДЕНИЯ_О_СТРАХОВОМ_СТАЖЕ_ЗЛ_ДЛЯ_УСТАНОВЛЕНИЯ_ПЕНСИИ);
            }

            #endregion

            xDoc.Root.SetDefaultXmlNamespace(pfr);

            xml = xDoc.ToString();
            return xml;
       }

        /// <summary>
        /// Формирование ветки XML с информацией о стаже для СПВ-2
        /// </summary>
        /// <param name="staj_osn"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        private XElement createStajElement(StajOsn staj_osn, int i)
        {
            var staj_lgot_list = staj_osn.StajLgot.OrderBy(x => x.Number.Value);

            XElement СтажевыйПериод = new XElement("СтажевыйПериод");

            СтажевыйПериод.Add(new XElement("НомерСтроки", i),
                                   new XElement("ДатаНачалаПериода", staj_osn.DateBegin.Value.ToShortDateString()),
                                   new XElement("ДатаКонцаПериода", staj_osn.DateEnd.Value.ToShortDateString()));

            if (staj_lgot_list.Count() > 0)
            {
                СтажевыйПериод.Add(new XElement("КоличествоЛьготныхСоставляющих", staj_lgot_list.Count().ToString()));

                int j = 0;
                foreach (var staj_lgot in staj_lgot_list)
                {
                    j++;
                    XElement ЛьготныйСтаж = new XElement("ЛьготныйСтаж",
                                                new XElement("НомерСтроки", j));

                    XElement ОсобенностиУчета = new XElement("ОсобенностиУчета");
                    if (staj_lgot.TerrUslID != null)
                    {
                        ОсобенностиУчета.Add(new XElement("ТерриториальныеУсловия",
                                                 new XElement("ОснованиеТУ", staj_lgot.TerrUsl.Code)));

                        if (staj_lgot.TerrUsl.Code.Substring(0,1) != "Ч")
                        {
                            ОсобенностиУчета.Element("ТерриториальныеУсловия").Add(new XElement("Коэффициент", Utils.decToStr(staj_lgot.TerrUslKoef.Value)));
                        }
                    }

                    if (staj_lgot.OsobUslTrudaID != null)
                    {
                        XElement ОсобыеУсловияТруда = new XElement("ОсобыеУсловияТруда",
                                                 new XElement("ОснованиеОУТ", staj_lgot.OsobUslTruda.Code));

                        if (staj_lgot.KodVred_OsnID != null)
                        {
                            ОсобыеУсловияТруда.Add(new XElement("ПозицияСписка", staj_lgot.KodVred_2.Code));
                        }

                        ОсобенностиУчета.Add(ОсобыеУсловияТруда);
                    }

                    if (staj_lgot.IschislStrahStajOsnID != null || staj_lgot.Strah1Param.HasValue || staj_lgot.Strah2Param.HasValue)
                    {
                        XElement ИсчисляемыйСтаж = new XElement("ИсчисляемыйСтаж");

                        if (staj_lgot.IschislStrahStajOsnID != null)
                        {
                            ИсчисляемыйСтаж.Add(new XElement("ОснованиеИС", staj_lgot.IschislStrahStajOsn.Code));
                            if (staj_lgot.IschislStrahStajOsn.Code == "ВОДОЛАЗ")
                            {
                                if (staj_lgot.Strah1Param.HasValue || staj_lgot.Strah2Param.HasValue)
                                {
                                    XElement ВыработкаВчасах = new XElement("ВыработкаВчасах");
                                    if (staj_lgot.Strah1Param.HasValue)
                                        ВыработкаВчасах.Add(new XElement("Часы", staj_lgot.Strah1Param.Value));
                                    if (staj_lgot.Strah2Param.HasValue)
                                        ВыработкаВчасах.Add(new XElement("Минуты", staj_lgot.Strah2Param.Value));

                                    ИсчисляемыйСтаж.Add(ВыработкаВчасах);
                                }
                            }
                            else
                            {
                                if (staj_lgot.Strah1Param.HasValue || staj_lgot.Strah2Param.HasValue)
                                {
                                    XElement ВыработкаКалендарная = new XElement("ВыработкаКалендарная");
                                    if (staj_lgot.Strah1Param.HasValue)
                                        ВыработкаКалендарная.Add(new XElement("ВсеМесяцы", staj_lgot.Strah1Param.Value));
                                    if (staj_lgot.Strah2Param.HasValue)
                                        ВыработкаКалендарная.Add(new XElement("ВсеДни", staj_lgot.Strah2Param.Value));

                                    ИсчисляемыйСтаж.Add(ВыработкаКалендарная);
                                }
                            }
                        }
                        else if ((staj_lgot.Strah1Param.HasValue && staj_lgot.Strah1Param.Value != 0) || (staj_lgot.Strah2Param.HasValue && staj_lgot.Strah2Param.Value != 0))
                        {
                            XElement ВыработкаКалендарная = new XElement("ВыработкаКалендарная");
                            if (staj_lgot.Strah1Param.HasValue)
                                ВыработкаКалендарная.Add(new XElement("ВсеМесяцы", staj_lgot.Strah1Param.Value));
                            if (staj_lgot.Strah2Param.HasValue)
                                ВыработкаКалендарная.Add(new XElement("ВсеДни", staj_lgot.Strah2Param.Value));

                            ИсчисляемыйСтаж.Add(ВыработкаКалендарная);
                        }

                        ОсобенностиУчета.Add(ИсчисляемыйСтаж);
                    }

                    if (staj_lgot.IschislStrahStajDopID != null)
                    {
                        ОсобенностиУчета.Add(new XElement("ДекретДети", staj_lgot.IschislStrahStajDop.Code));
                    }

                    if (staj_lgot.UslDosrNaznID != null)
                    {
                        XElement ВыслугаЛет = new XElement("ВыслугаЛет",
                                                 new XElement("ОснованиеВЛ", staj_lgot.UslDosrNazn.Code));

                        if (staj_lgot.UslDosrNazn.Code == "27-15")
                        {
                            if (staj_lgot.UslDosrNazn1Param.HasValue || staj_lgot.UslDosrNazn2Param.HasValue)
                            {
                                XElement ВыработкаКалендарная = new XElement("ВыработкаКалендарная");
                                if (staj_lgot.UslDosrNazn1Param.HasValue)
                                    ВыработкаКалендарная.Add(new XElement("ВсеМесяцы", staj_lgot.UslDosrNazn1Param.Value));
                                if (staj_lgot.UslDosrNazn2Param.HasValue)
                                    ВыработкаКалендарная.Add(new XElement("ВсеДни", staj_lgot.UslDosrNazn2Param.Value));

                                ВыслугаЛет.Add(ВыработкаКалендарная);
                            }
                        }
                        else
                        {
                            if (staj_lgot.UslDosrNazn1Param.HasValue || staj_lgot.UslDosrNazn2Param.HasValue)
                            {
                                XElement ВыработкаВчасах = new XElement("ВыработкаВчасах");
                                if (staj_lgot.UslDosrNazn1Param.HasValue)
                                    ВыработкаВчасах.Add(new XElement("Часы", staj_lgot.UslDosrNazn1Param.Value));
                                if (staj_lgot.UslDosrNazn2Param.HasValue)
                                    ВыработкаВчасах.Add(new XElement("Минуты", staj_lgot.UslDosrNazn2Param.Value));

                                ВыслугаЛет.Add(ВыработкаВчасах);
                            }
                        }

                        if (staj_lgot.UslDosrNazn3Param.HasValue)
                        {
                            ВыслугаЛет.Add(new XElement("ДоляСтавки", Utils.decToStr(staj_lgot.UslDosrNazn3Param.Value)));
                        }

                        ОсобенностиУчета.Add(ВыслугаЛет);
                    }

                    ЛьготныйСтаж.Add(ОсобенностиУчета);

                    СтажевыйПериод.Add(ЛьготныйСтаж);
                }
            }

            return СтажевыйПериод;
        }

        private void CreateXmlPack_SPW2_2014_FormClosing(object sender, FormClosingEventArgs e)
        {
            Props props = new Props(); //экземпляр класса с настройками

            if (Options.saveLastPackNum)
            {
                short y = 0;
                short.TryParse(Year.SelectedItem.Text, out y);
                byte q = 12;
                byte.TryParse(Quarter.SelectedItem.Value.ToString(), out q);

                numPackSettings numPackSett = new numPackSettings
                {
                    FormName = this.Name,
                    Number = (int)numFrom.Value,
                    Year = y,
                    Quarter = q
                };

                props.setPackNum(numPackSett);
            }
        }



    }
}
