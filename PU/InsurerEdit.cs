using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PU.Models;
using Telerik.WinControls;
using Telerik.WinControls.Enumerations;
using System.Xml.Linq;
using System.Linq;
using PU.Classes;
using System.Net;
using System.IO;
using Telerik.WinControls.UI.Localization;
using System.Reflection;

namespace PU
{
    public partial class InsurerEdit : Telerik.WinControls.UI.RadForm
    {
//        public short typePayer = 0;
        pu6Entities db = new pu6Entities();
        public string action;
        public Insurer insData = new Insurer();
        public long InsID { get; set; }
        bool connGood;
        BackgroundWorker bw = new BackgroundWorker();
        BackgroundWorker bw_ccs = new BackgroundWorker();
        MethodsNonStatic methodsNonStatic = new MethodsNonStatic(); //экземпляр класса с настройками
        List<ErrList> errList = new List<ErrList>();
        private bool cleanData = true;
        bool allowClose = false;


        public InsurerEdit()
        {
            InitializeComponent();
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    saveRkasvBtn.Enabled = false;
                    saveBtn.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    //this.Dispose();
                    break;
            }

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

        private void radToggleButton1_ToggleStateChanged(object sender, Telerik.WinControls.UI.StateChangedEventArgs args)
        {
            bool toogleState = true;
            if (!(radToggleButton1.ToggleState == ToggleState.On)) 
            {
                radLabel1.Text = "Организация";
//                typePayer = 0;
            } 
            else 
            {
                radLabel1.Text = "Индивидуальный предприниматель";
//                typePayer = 1;
                toogleState = false;
            }

            this.radLabel3.Text = toogleState ? "Наименование" : "Фамилия";
            this.radLabel4.Text = toogleState ? "Наименование краткое" : "Имя";
            this.radLabel5.Visible = toogleState ? false : true;
            this.Name_.Visible = toogleState;
            this.NameShort_.Visible = toogleState;
            this.LastName_.Visible = !toogleState;
            this.FirstName_.Visible = !toogleState;
            this.MiddleName_.Visible = !toogleState;
            this.radLabel6.Visible = !toogleState;
            this.InsuranceNumber_.Visible = !toogleState;
            this.radLabel7.Visible = !toogleState;
            this.YearBirth_.Visible = !toogleState;
            this.radLabel9.Visible = toogleState;
            this.KPP_.Visible = toogleState;
            this.INN_UR_.Visible = toogleState;
            this.INN_IP_.Visible = !toogleState;
            this.radLabel10.Text = toogleState ? "Код по ЕГРЮЛ (ОГРН)" : "Код по ЕГРИП (ОГРНИП)";
            this.EGRUL_.Visible = toogleState;
            this.EGRIP_.Visible = !toogleState;


        }



        private void GetData()
        {
            string regN = "";
            string snils = "";
            byte contrNum = 0;
             

            var s = (this.RegNum_.Value.ToString()).Split('-');
            foreach (var item in s)
            {
                if (!item.Contains("_"))
                    regN += item;
                else
                {
                    regN = "";
                    break;
                }
            }


            if (radToggleButton1.ToggleState == ToggleState.On && !String.IsNullOrEmpty(this.InsuranceNumber_.Text))
            {
                var s2 = (this.InsuranceNumber_.Text.ToString()).Split(' ');

                if (byte.TryParse(s2[1], out contrNum))
                {
                    var s3 = s2[0].Split('-');
                    foreach (var item in s3)
                    {
                        if (!item.Contains("_"))
                            snils += item;
                        else
                        {
                            snils = "";
                            contrNum = 0;
                            break;
                        }
                    }
                }
            }


            insData.TypePayer = radToggleButton1.ToggleState != ToggleState.On ? (byte)0 : (byte)1;
            insData.RegNum = regN;
            insData.INN = radToggleButton1.ToggleState != ToggleState.On ? this.INN_UR_.Text.ToString() : this.INN_IP_.Text.ToString();
            insData.OKTMO = this.OKTMO_.Text.ToString();
            insData.OKWED = this.OKWED_.Text.ToString();
            insData.OKPO = this.OKPO_.Text.ToString();
            insData.OrgLegalForm = this.OrgLegalForm_.Text;
            insData.PhoneContact = this.PhoneContact_.Text;
            insData.BossDolgn = this.BossDolgn_.Text;
            insData.BossFIO = this.BossFIO_.Text;
            insData.BossPrint = this.printBoss1CheckBox.Checked;
            insData.BossDolgnDop = this.BossDolgnDop_.Text;
            insData.BossFIODop = this.BossFIODop_.Text;
            insData.BossDopPrint = this.printBoss2CheckBox.Checked;
            insData.BuchgFIO = this.BuchgFIO_.Text;
            insData.BuchgPrint = this.printBuhCheckBox.Checked;
            insData.PerformerDolgn = this.PerformerDolgn_.Text;
            insData.PerformerFIO = this.PerformerFIO_.Text;
            insData.PerformerPrint = this.printIspolnCheckBox.Checked;
            insData.Name = radToggleButton1.ToggleState != ToggleState.On ? this.Name_.Text : null;
            insData.NameShort = radToggleButton1.ToggleState != ToggleState.On ? this.NameShort_.Text : null;
            insData.LastName = radToggleButton1.ToggleState == ToggleState.On ? this.LastName_.Text : null;
            insData.FirstName = radToggleButton1.ToggleState == ToggleState.On ? this.FirstName_.Text : null;
            insData.MiddleName = radToggleButton1.ToggleState == ToggleState.On ? this.MiddleName_.Text : null;
            insData.KPP = radToggleButton1.ToggleState != ToggleState.On ? this.KPP_.Text.ToString() : null;
            insData.EGRUL = radToggleButton1.ToggleState != ToggleState.On ? this.EGRUL_.Text.ToString() : null;
            insData.EGRIP = radToggleButton1.ToggleState == ToggleState.On ? this.EGRIP_.Text.ToString() : null;
            insData.InsuranceNumber = radToggleButton1.ToggleState == ToggleState.On ? snils : null;
            insData.YearBirth = radToggleButton1.ToggleState == ToggleState.On ? Convert.ToInt16(this.YearBirth_.Value) : (short)0;

            if (contrNum != 0)
            {
                insData.ControlNumber = contrNum;
            }
        }

        private bool validation()
        {
            errList = new List<ErrList>();

            if (RegNum_.Text == RegNum_.NullText)
            {
                errList.Add(new ErrList { name = "Поле 'Рег.номер в ПФР' обязательно к заполнению!", control = "RegNum_" });
            }

            if (radToggleButton1.ToggleState != ToggleState.On) // если организация
            {
                if (String.IsNullOrEmpty(Name_.Text))
                {
                    errList.Add(new ErrList { name = "Поле 'Наименование' обязательно к заполнению!", control = "Name_" });
                }
                if (String.IsNullOrEmpty(NameShort_.Text))
                {
                    errList.Add(new ErrList { name = "Поле 'Наименование краткое' обязательно к заполнению!", control = "NameShort_" });
                }
                if (String.IsNullOrEmpty(INN_UR_.Text))
                {
                    errList.Add(new ErrList { name = "Поле 'ИНН' обязательно к заполнению!", control = "INN_UR_" });
                }

            }
            else
            {
                if (String.IsNullOrEmpty(LastName_.Text))
                {
                    errList.Add(new ErrList { name = "Поле 'Фамилия' обязательно к заполнению!", control = "LastName_" });
                }
                if (String.IsNullOrEmpty(FirstName_.Text))
                {
                    errList.Add(new ErrList { name = "Поле 'Имя' обязательно к заполнению!", control = "FirstName_" });
                }
                if (String.IsNullOrEmpty(INN_IP_.Text))
                {
                    errList.Add(new ErrList { name = "Поле 'ИНН' обязательно к заполнению!", control = "INN_IP_" });
                }
            }

            return errList.Count() == 0;
        }


        private void InsurerEdit_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            radPageView1.SelectedPage = radPageView1.Pages[0];

            checkAccessLevel();

            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(createRequest);

            RegNum_.Select();

            //когда есть настройки сервиса РК АСВ
            if (!String.IsNullOrEmpty(Options.RKASV.url) && !String.IsNullOrEmpty(Options.RKASV.opfrCode) && !String.IsNullOrEmpty(Options.RKASV.service))
            {
                rkasvInfoPanel.Visible = true;
                rkasvLabel.Visible = true;
                rkasvToggle.Visible = true;
                statusLabel.ThemeName = "";
                urlTextBox.Text = !String.IsNullOrEmpty(Options.RKASV.url) ? Options.RKASV.url : "";
                serviceTextBox.Text = !String.IsNullOrEmpty(Options.RKASV.service) ? Options.RKASV.service : "";
                portTextBox.Text = !String.IsNullOrEmpty(Options.RKASV.port) ? Options.RKASV.port : "";
                codeRegionTextBox.Text = !String.IsNullOrEmpty(Options.RKASV.opfrCode) ? Options.RKASV.opfrCode : "";

                bw_ccs = new BackgroundWorker();
                bw_ccs.WorkerSupportsCancellation = true;
                bw_ccs.DoWork += new System.ComponentModel.DoWorkEventHandler(checkConnectionStatus);// проверка статуса подключения
                bw_ccs.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(checkConnectionStatusEnd); // после проверки статуса очистка поля Ошибка если есть коннект
                bw_ccs.RunWorkerAsync();
            }
            else
            {
                connGood = false;
                statusLabel.Text = "Ошибка";
                statusLabel.ForeColor = Color.LightCoral;
            }


            if (action == "edit")
            {
                if (db.Insurer.Any(x => x.ID == InsID))
                {

                    if (Options.InsurerFolders.Any(x => x.regnum == insData.RegNum))
                    {
                        var p = Options.InsurerFolders.FirstOrDefault(x => x.regnum == insData.RegNum);
                        importPathBrowser.Value = p.importPath;
                        exportPathBrowser.Value = p.exportPath;
                    }

                    insData = db.Insurer.FirstOrDefault(x => x.ID == InsID);

                    rkasvToggle.IsOn = true; // если Редактирование, то по дефолту автозаполнение отключено
                    radToggleButton1.ToggleState = insData.TypePayer == 0 ? ToggleState.Off : ToggleState.On;
                    RegNum_.Text = insData.RegNum;
                    OKTMO_.Text = insData.OKTMO;
                    OKPO_.Text = insData.OKPO;
                    OKWED_.Text = insData.OKWED;
                    OrgLegalForm_.Text = insData.OrgLegalForm;
                    PhoneContact_.Text = insData.PhoneContact;

                    BossDolgn_.Text = insData.BossDolgn;
                    BossFIO_.Text = insData.BossFIO;
                    printBoss1CheckBox.Checked = insData.BossPrint.HasValue ? insData.BossPrint.Value : false;
                    BossDolgnDop_.Text = insData.BossDolgnDop;
                    BossFIODop_.Text = insData.BossFIODop;
                    printBoss2CheckBox.Checked = insData.BossDopPrint.HasValue ? insData.BossDopPrint.Value : false;
                    BuchgFIO_.Text = insData.BuchgFIO;
                    printBuhCheckBox.Checked = insData.BuchgPrint.HasValue ? insData.BuchgPrint.Value : false;
                    PerformerDolgn_.Text = insData.PerformerDolgn;
                    PerformerFIO_.Text = insData.PerformerFIO;
                    printIspolnCheckBox.Checked = insData.PerformerPrint.HasValue ? insData.PerformerPrint.Value : false;

                    if (insData.TypePayer == 0) // если организация
                    {
                        Name_.Text = insData.Name;
                        NameShort_.Text = insData.NameShort;
                        KPP_.Text = insData.KPP;
                        EGRUL_.Text = insData.EGRUL;
                        INN_UR_.Text = insData.INN;
                    }
                    else // если физ лицо
                    {
                        LastName_.Text = insData.LastName;
                        FirstName_.Text = insData.FirstName;
                        MiddleName_.Text = insData.MiddleName;
                        INN_IP_.Text = insData.INN;
                        YearBirth_.Value = insData.YearBirth.HasValue ? decimal.Parse(insData.YearBirth.Value.ToString()) : 0;
                        EGRIP_.Text = insData.EGRIP;
                        InsuranceNumber_.Text = insData.InsuranceNumber + (insData.ControlNumber.HasValue ? insData.ControlNumber.Value.ToString() : "");

                    }

                }
                else
                {
                    RadMessageBox.Show("Не удалось загрузить данные Страхователя из базы данных!");
                }

            }
            else
            {
                insData = new Insurer();
            }
        }

        private void RegNum__Leave(object sender, EventArgs e)
        {
            if (RegNum_.Text != RegNum_.NullText && RegNum_.Text.Contains("_"))  // если регномер не дозаполнен
            {
                if ((DialogResult)RadMessageBox.Show("Регистрационный номер заполнен не правильно! Вернуться для исправления?", "Ошибка заполнения", MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button3) == System.Windows.Forms.DialogResult.Yes)
                {
                    RegNum_.Focus();
                }
                else
                {
                    RegNum_.Value = RegNum_.NullText;

                }
            }
            else
            {

                errorRkasvBox.ResetText();


                if (!rkasvToggle.IsOn)  // если включено автозаполнение
                {
                    if (connGood)
                    {
                        OKTMO_.ResetText();
                        OKPO_.ResetText();
                        OKWED_.ResetText();
                        OrgLegalForm_.ResetText();
                        PhoneContact_.ResetText();
                        BossDolgn_.ResetText();
                        BossFIO_.ResetText();
                        BossDolgnDop_.ResetText();
                        BossFIODop_.ResetText();
                        BuchgFIO_.ResetText();
                        PerformerDolgn_.ResetText();
                        PerformerFIO_.ResetText();
                        Name_.ResetText();
                        NameShort_.ResetText();
                        KPP_.ResetText();
                        EGRUL_.ResetText();
                        INN_UR_.ResetText();
                        LastName_.ResetText();
                        FirstName_.ResetText();
                        MiddleName_.ResetText();
                        INN_IP_.ResetText();
                        YearBirth_.ResetText();
                        EGRIP_.ResetText();
                        InsuranceNumber_.Clear();
                    }

                    while (bw.IsBusy)
                    {
                        Application.DoEvents();
                    }

                    if (!bw.IsBusy)
                        bw.RunWorkerAsync();
                }
            }
        }

        private void createRequest(object sender, DoWorkEventArgs e)
        {
            string regN = "";
            var s = (this.RegNum_.Value.ToString()).Split('-');
            foreach (var item in s)
            {
                if (!item.Contains("_"))
                    regN += item;
                else
                {
                    regN = "";
                    break;
                }
            }

            if (!String.IsNullOrEmpty(regN) && !String.IsNullOrEmpty(Options.RKASV.opfrCode) && !String.IsNullOrEmpty(Options.RKASV.url) && !String.IsNullOrEmpty(Options.RKASV.service))
            {
                XNamespace pfr = "http://www.r-style.com/2014/odm-model";
                XDocument xDoc = new XDocument(new XDeclaration("1.0", "utf-8", null),
                                         new XElement("model",
                                             new XElement("entry",
                                                 new XElement("key", "asv.registrationNumber"),
                                                 new XElement("value", regN),
                                                 new XElement("type", "")),
                                             new XElement("entry",
                                                 new XElement("key", "asv.opfrCode"),
                                                 new XElement("value", Options.RKASV.opfrCode),
                                                 new XElement("type", ""))));
                xDoc.Root.SetDefaultXmlNamespace(pfr);

                string port = !String.IsNullOrEmpty(Options.RKASV.port) ? (":" + Options.RKASV.port) : "";
                string url = Options.RKASV.url + port + Options.RKASV.service;
                
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri) || Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    string postedData = xDoc.Document.ToString();

                    PostMethod(postedData, uri);

                }

            }
        }

        public void PostMethod(string postedData, Uri postUrl)
        {
   //         radTextBoxControl1.Invoke(new Action(() => { radTextBoxControl1.Text = "postUrl:\r\n" + postUrl + "\r\n\r\npostedData:\r\n" + postedData; }));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "POST";
            //     request.Credentials = CredentialCache.DefaultCredentials;

            UTF8Encoding encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes(postedData);

            string s = "";
            foreach (var b in bytes)
            {
                s += b.ToString();
            }
       //     radTextBoxControl1.Invoke(new Action(() => { radTextBoxControl1.AppendText("\r\n\r\nbytes:\r\n" + s); }));


            request.ContentType = "application/xml";
            request.ContentLength = bytes.Length;
            request.Headers.Add("message-id", Guid.NewGuid().ToString());
            request.Timeout = 2000;
            string responseToString = "";

  //          radTextBoxControl1.AppendText("\r\n\r\nЗапрос:\r\n" + request.ToString());


            try
            {
                using (var newStream = request.GetRequestStream())
                {
                    newStream.Write(bytes, 0, bytes.Length);
                    newStream.Close();
                }

     //           radTextBoxControl1.Invoke(new Action(() => { radTextBoxControl1.AppendText("\r\n\r\nHeaders:\r\n" + request.Headers); }));


                var response = (HttpWebResponse)request.GetResponse();
                if (response != null)
                {
                    var strreader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseToString = strreader.ReadToEnd();
     //               radTextBoxControl1.Invoke(new Action(() => { radTextBoxControl1.AppendText("\r\n\r\nresponseToString:\r\n" + responseToString); }));

                }

                fillFields(responseToString);
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        errorPost(reader.ReadToEnd());
                    }
                }
                else
                {
                    errorPost(ex.Message);
                }
                bw.CancelAsync();
            }
            catch (Exception ex)
            {
                errorPost(ex.Message);
                bw.CancelAsync();
            }

        }


        // Вывод информации об ошибке
        private void errorPost(string errorMessage)
        {
            errorRkasvBox.Text = "Ошибка получения данных: \r\n" + errorMessage;
        }

        /// <summary>
        /// разбор ответа сервера и заполнение полей
        /// </summary>
        /// <param name="response"></param>
        private void fillFields(string response)
        {
            XDocument doc = new XDocument();
            try
            {
                doc = XDocument.Parse(response);
                foreach (var elem in doc.Descendants())
                    elem.Name = elem.Name.LocalName;

                if (doc.Descendants().Any(x => x.Value == "asv.errorCode") || doc.Descendants().Any(x => x.Value == "asv.errorMessage"))
                {
                    string err = "";
                    if (doc.Descendants().Any(x => x.Value == "asv.errorCode") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.errorCode").Parent.Element("value") != null)
                        err = "errorCode:  " + doc.Descendants().FirstOrDefault(x => x.Value == "asv.errorCode").Parent.Element("value").Value + "\r\n";

                    if (doc.Descendants().Any(x => x.Value == "asv.errorMessage") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.errorMessage").Parent.Element("value") != null)
                        err = err + "errorMessage:  " + doc.Descendants().FirstOrDefault(x => x.Value == "asv.errorMessage").Parent.Element("value").Value;
                    
                    errorPost(err);
                    bw.CancelAsync();
                    return;
                }

                //проверяем полное и сокращенное название, если заполненоно хоть одно значит - организация
                if ((doc.Descendants().Any(x => x.Value == "asv.fullName") && doc.Descendants().Any(x => x.Value == "asv.shortName")) && (doc.Descendants().FirstOrDefault(x => x.Value == "asv.shortName").Parent.Element("value") != null || doc.Descendants().FirstOrDefault(x => x.Value == "asv.fullName").Parent.Element("value") != null))
                {
                    radToggleButton1.Invoke(new Action(() => { radToggleButton1.IsChecked = false; }));
                    if (doc.Descendants().Any(x => x.Value == "asv.shortName") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.shortName").Parent.Element("value") != null)
                    {
                        string s = doc.Descendants().FirstOrDefault(x => x.Value == "asv.shortName").Parent.Element("value").Value;
                        NameShort_.Invoke(new Action(() => { NameShort_.Text = s.Length > 50 ? s.Substring(0, 50) : s; }));
                    }

                    if (doc.Descendants().Any(x => x.Value == "asv.fullName") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.fullName").Parent.Element("value") != null)
                    {
                        string s = doc.Descendants().FirstOrDefault(x => x.Value == "asv.fullName").Parent.Element("value").Value;
                        Name_.Invoke(new Action(() => { Name_.Text = s.Length > 100 ? s.Substring(0, 100) : s; }));
                    }

                    if (doc.Descendants().Any(x => x.Value == "asv.kpp") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.kpp").Parent.Element("value") != null)
                        KPP_.Invoke(new Action(() => { KPP_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.kpp").Parent.Element("value").Value; }));
                    if (doc.Descendants().Any(x => x.Value == "asv.ogrn") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.ogrn").Parent.Element("value") != null)
                        EGRUL_.Invoke(new Action(() => { EGRUL_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.ogrn").Parent.Element("value").Value; }));
                    if (doc.Descendants().Any(x => x.Value == "asv.inn") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.inn").Parent.Element("value") != null)
                        INN_UR_.Invoke(new Action(() => { INN_UR_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.inn").Parent.Element("value").Value; }));
                }
                else // иначе индивидуальнный предприниматель
                {
                    radToggleButton1.Invoke(new Action(() => { radToggleButton1.IsChecked = true; }));
                    if (doc.Descendants().Any(x => x.Value == "asv.lastName") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.lastName").Parent.Element("value") != null)
                        LastName_.Invoke(new Action(() => { LastName_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.lastName").Parent.Element("value").Value; }));
                    if (doc.Descendants().Any(x => x.Value == "asv.firstName") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.firstName").Parent.Element("value") != null)
                        FirstName_.Invoke(new Action(() => { FirstName_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.firstName").Parent.Element("value").Value; }));
                    if (doc.Descendants().Any(x => x.Value == "asv.middleName") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.middleName").Parent.Element("value") != null)
                        MiddleName_.Invoke(new Action(() => { MiddleName_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.middleName").Parent.Element("value").Value; }));

                    if (doc.Descendants().Any(x => x.Value == "asv.orgnip") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.orgnip").Parent.Element("value") != null)
                        EGRIP_.Invoke(new Action(() => { EGRIP_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.orgnip").Parent.Element("value").Value; }));
                    if (doc.Descendants().Any(x => x.Value == "asv.snils") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.snils").Parent.Element("value") != null)
                        InsuranceNumber_.Invoke(new Action(() => { InsuranceNumber_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.snils").Parent.Element("value").Value; }));
                    if (doc.Descendants().Any(x => x.Value == "asv.inn") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.inn").Parent.Element("value") != null)
                        INN_IP_.Invoke(new Action(() => { INN_IP_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.inn").Parent.Element("value").Value; }));
                }

                if (doc.Descendants().Any(x => x.Value == "asv.oktmo") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.oktmo").Parent.Element("value") != null)
                    OKTMO_.Invoke(new Action(() => { OKTMO_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.oktmo").Parent.Element("value").Value; }));
                else if (doc.Descendants().Any(x => x.Value == "asv.okato") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.okato").Parent.Element("value") != null)
                    OKTMO_.Invoke(new Action(() => { OKTMO_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.okato").Parent.Element("value").Value; }));

                if (doc.Descendants().Any(x => x.Value == "asv.okopfCode") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.okopfCode").Parent.Element("value") != null)
                    OKPO_.Invoke(new Action(() => { OKPO_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.okopfCode").Parent.Element("value").Value; }));

                if (doc.Descendants().Any(x => x.Value == "asv.okvedCode") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.okvedCode").Parent.Element("value") != null)
                    OKWED_.Invoke(new Action(() => { OKWED_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.okvedCode").Parent.Element("value").Value; }));

                if (doc.Descendants().Any(x => x.Value == "asv.okopfName") && doc.Descendants().FirstOrDefault(x => x.Value == "asv.okopfName").Parent.Element("value") != null)
                    OrgLegalForm_.Invoke(new Action(() => { OrgLegalForm_.Text = doc.Descendants().FirstOrDefault(x => x.Value == "asv.okopfName").Parent.Element("value").Value; }));


                //TypePayer = radToggleButton1.ToggleState != ToggleState.On ? (byte)0 : (byte)1,


            }
            catch
            {
                errorPost("Ошибка обработки ответа сервиса!");
                bw.CancelAsync();
                return;
            }


        }

        private void editRkasvBtn_Click(object sender, EventArgs e)
        {
            urlTextBox.Enabled = true;
            serviceTextBox.Enabled = true;
            portTextBox.Enabled = true;
            codeRegionTextBox.Enabled = true;
            cancelRkasvBtn.Visible = true;
            saveRkasvBtn.Visible = true;
            editRkasvBtn.Visible = false;

        }

        private void checkConnectionStatus(object sender, DoWorkEventArgs e)
        {
            connGood = false;
            if (!String.IsNullOrEmpty(Options.RKASV.url) && !String.IsNullOrEmpty(Options.RKASV.service))
            {
                string port = !String.IsNullOrEmpty(Options.RKASV.port) ? (":" + Options.RKASV.port) : "";
                string url = Options.RKASV.url + port + Options.RKASV.service;
                Uri uri;
                if (Uri.TryCreate(url, UriKind.Absolute, out uri) || Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    try
                    {
                        HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(uri);
                        wr.Timeout=2500;
                        try
                        {
                            HttpWebResponse myHttpWebResponse = (HttpWebResponse)wr.GetResponse();
                            connGood = true;
                        }
                        catch (WebException we)
                        {
                            connGood = false;
                            errorRkasvBox.Text = "Ошибка получения данных: \r\n" + "Сервис не доступен! Сообщение об ошибке: " + we.Message;
                        }
/*                        wc.Headers[HttpRequestHeader.ContentType] = "application/xml;charset=UTF-8";
                        wc.t
                        string responsetext = wc.DownloadString(uri);*/
                    }
                    catch
                    { 

                    }
                }
            }

            if (connGood)
            {
                if (this.IsHandleCreated)
                    statusLabel.Invoke(new Action(() => { statusLabel.Text = "ОК"; statusLabel.ForeColor = Color.MediumSeaGreen; }));
            }
            else
            {
                if (this.IsHandleCreated)
                    statusLabel.Invoke(new Action(() => { statusLabel.Text = "Ошибка"; statusLabel.ForeColor = Color.LightCoral; }));
            }


        }

        private void checkConnectionStatusEnd(object sender, RunWorkerCompletedEventArgs e)
        {
            if (connGood && this.IsHandleCreated)
                errorRkasvBox.Invoke(new Action(() => { errorRkasvBox.ResetText(); }));
        }

        private void cancelRkasvBtn_Click(object sender, EventArgs e)
        {
            urlTextBox.Text = !String.IsNullOrEmpty(Options.RKASV.url) ? Options.RKASV.url : "";
            serviceTextBox.Text = !String.IsNullOrEmpty(Options.RKASV.service) ? Options.RKASV.service : "";
            portTextBox.Text = !String.IsNullOrEmpty(Options.RKASV.port) ? Options.RKASV.port : "";
            codeRegionTextBox.Text = !String.IsNullOrEmpty(Options.RKASV.opfrCode) ? Options.RKASV.opfrCode : "";

            urlTextBox.Enabled = false;
            serviceTextBox.Enabled = false;
            portTextBox.Enabled = false;
            codeRegionTextBox.Enabled = false;
            cancelRkasvBtn.Visible = false;
            saveRkasvBtn.Visible = false;
            editRkasvBtn.Visible = true;
        }

        private void saveRkasvBtn_Click(object sender, EventArgs e)
        {
            Options.RKASV.url = urlTextBox.Text;
            Options.RKASV.service = serviceTextBox.Text;
            Options.RKASV.port = portTextBox.Text;
            Options.RKASV.opfrCode = codeRegionTextBox.Text;
            methodsNonStatic.writeSetting();

            urlTextBox.Enabled = false;
            serviceTextBox.Enabled = false;
            portTextBox.Enabled = false;
            codeRegionTextBox.Enabled = false;
            cancelRkasvBtn.Visible = false;
            saveRkasvBtn.Visible = false;
            editRkasvBtn.Visible = true;

            while (!bw_ccs.IsBusy)
            {
                bw_ccs.RunWorkerAsync();
                System.Threading.Thread.Sleep(200);
//                checkConnectionStatus();

            }
        }

        private void rkasvToggle_Toggled(object sender, EventArgs e)
        {
            errorRkasvBox.ResetText();
            rkasvToggle.ForeColor = !rkasvToggle.IsOn ? Color.MediumSeaGreen : Color.LightCoral;
        }

        private void InsurerEdit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                System.Windows.Forms.SendKeys.Send("{TAB}");
            }
        }

        private void InsurerEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (bw.IsBusy)
            {
                bw.Dispose();
            }
            if (bw_ccs.IsBusy)
            {
                bw_ccs.Dispose();
            }


            if (allowClose)
            {
                if (cleanData)
                    insData = null;
                db.Dispose();
            }
            else
            {
                DialogResult dialogResult = RadMessageBox.Show("Вы хотите сохранить изменения перед закрытием формы?", "Сохранение записи!", MessageBoxButtons.YesNoCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button3);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        saveBtn_Click(null, null);
                        break;
                    case DialogResult.No:
                        if (cleanData)
                            insData = null;
                        db.Dispose();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }

            }

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (validation())
            {
                cleanData = true;

                GetData();

                if (Options.InsurerFolders.Any(x => x.regnum == insData.RegNum))
                {
                    var p = Options.InsurerFolders.FirstOrDefault(x => x.regnum == insData.RegNum);
                    Options.InsurerFolders.Remove(p);
                    methodsNonStatic.writeSetting();
                }

                if (Options.InsID == insData.ID)
                {
                    Options.CurrentInsurerFolders.regnum = insData.RegNum;
                    Options.CurrentInsurerFolders.importPath = importPathBrowser.Value;
                    Options.CurrentInsurerFolders.exportPath = exportPathBrowser.Value;
                    methodsNonStatic.writeSetting();
                }

                if (!String.IsNullOrEmpty(importPathBrowser.Value) || !String.IsNullOrEmpty(exportPathBrowser.Value))
                {
                    InsurerImportExportPath param = new InsurerImportExportPath
                    {
                        regnum = insData.RegNum,
                        importPath = importPathBrowser.Value,
                        exportPath = exportPathBrowser.Value

                    };
                    Options.InsurerFolders.Add(param);
                    methodsNonStatic.writeSetting();
                }


                string result = "";
                switch (action)
                {
                    case "add":
                        //  result = InsurerFrm.SelfRef.add(insData);

                        try
                        {
                            if (!db.Insurer.Any(x => x.RegNum == insData.RegNum))
                            {
                                db.Insurer.Add(insData);

                                try
                                {
                                    db.SaveChanges();
                                    cleanData = false;
                                }
                                catch (Exception ex)
                                {
                                    RadMessageBox.Show(this, "Во время сохранения данных Страхователя произошла ошибка! Код ошибки - " + ex.Message);
                                }
                            }
                            else
                                result = "Страхователь с рег. номером " + insData.RegNum + " уже существует в БД!";
                        }
                        catch (Exception ex)
                        {
                            result = ex.Message;
                        }


                        break;
                    case "edit":
                        // выбираем из базы исходную запись по идешнику
                        db = new pu6Entities();

                        Insurer Ins_ish = db.Insurer.FirstOrDefault(x => x.ID == insData.ID);

                        try
                        {
                            var fields = typeof(Insurer).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                            var names = Array.ConvertAll(fields, field => field.Name);

                            foreach (var itemName_ in names)
                            {
                                string itemName = itemName_.TrimStart('_');
                                var properties = insData.GetType().GetProperty(itemName);
                                if (properties != null)
                                {
                                    object value = properties.GetValue(insData, null);
                                    var data = value;

                                    Ins_ish.GetType().GetProperty(itemName).SetValue(Ins_ish, data, null);
                                }

                            }


                            // сохраняем модифицированную запись обратно в бд
                            db.Entry(Ins_ish).State = System.Data.Entity.EntityState.Modified;
                            // сохраняем модифицированную запись обратно в бд
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            RadMessageBox.Show("При сохранении основных данных произошла ошибка. Код ошибки: " + ex.Message);
                        }



                        break;
                }

                allowClose = true;
                this.Close();

            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }





    }
}
