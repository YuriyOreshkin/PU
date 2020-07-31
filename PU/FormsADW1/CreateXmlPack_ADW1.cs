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

namespace PU.FormsADW1
{
    public partial class CreateXmlPack_ADW1 : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        pfrXMLEntities dbxml = new pfrXMLEntities();

        public List<long> staffList_temp { get; set; }
        public long currentStaffId = 0;
        private bool noErrors = true;
        private bool cancel_work = false;

        BackgroundWorker bw = new BackgroundWorker();
        List<FormsADW_1> adw1List = new List<FormsADW_1>();


        public CreateXmlPack_ADW1()
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

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreateXmlPack_ADW1_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            DateUnderwrite.Value = DateTime.Now.Date;

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

        private void viewPacks_Click(object sender, EventArgs e)
        {
            rsw2014packs child = new rsw2014packs();

            child.ident.InsurerID = Options.InsID;
            child.ident.FormatType = "adw1";
            child.Owner = this;
            child.ThemeName = this.ThemeName;
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if ((switchStaffListDDL.SelectedIndex == 2 && (staffList_temp == null || (staffList_temp != null && staffList_temp.Count <= 0))) || (switchStaffListDDL.SelectedIndex == 1 && currentStaffId == 0))
            {
                RadMessageBox.Show("Пустой список сотрудников для формирования! Необходимо выбрать сотрудников!", "Внимание");
                return;
            }

            List<long> staffList = new List<long>();

            switch (switchStaffListDDL.SelectedIndex)
            {
                case 0: //все записи
                    staffList = db.Staff.Where(x => x.InsurerID == Options.InsID).Select(x => x.ID).ToList();
                    break;
                case 1: //текущая запись
                    staffList.Add(currentStaffId);
                    break;
                case 2: //по выделенным записям
                    staffList = staffList_temp;
                    break;
            }

            if (db.FormsADW_1.Any(x => x.StaffID.HasValue && staffList.Contains(x.StaffID.Value)))
            {
                adw1List = db.FormsADW_1.Where(x => x.StaffID.HasValue && staffList.Contains(x.StaffID.Value)).ToList();
            }
            else
            {
                RadMessageBox.Show("Пустой список сотрудников для формирования! Нет заполненных анкет АДВ-1.", "Внимание");
                return;
            }
            try
            {
                createPacksStartBW();
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(this, "Во время формирования пачек произошла ошибка. Код ошибки: " + ex.InnerException, "Ошибка");
            }

        }

        private void createPacksStartBW()
        {
            viewPacks.Enabled = false;
            startBtn.Enabled = false;
            closeBtn.Enabled = false;

            bw = new BackgroundWorker();
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(createPacks);
            bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;


            bw.RunWorkerAsync();
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            db = new pu6Entities();
            viewPacks.Enabled = true;
            startBtn.Enabled = true;
            closeBtn.Enabled = true;

            //    ReportViewerRSW2014.Invoke(new Action(() => {  }));
            if (noErrors)
            {
                RadMessageBox.Show(this, "Пачки сформированы", "Внимание");
                viewPacks_Click(null, null);
            }


        }

        private void bwCancel(string caption, string errMess)
        {
            if (!cancel_work)
            {
                this.Invoke(new Action(() => { RadMessageBox.Show(errMess, caption); }));//Methods.showAlert(caption, errMess, this.ThemeName); 
            }

            noErrors = false;
            bw.CancelAsync();
            return;

        }

        private void createPacks(object sender, DoWorkEventArgs e)
        {
            int packs_cnt_ALL = 0;


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

            string query = string.Empty;
            try
            {
                query = String.Format("DELETE FROM xmlInfo WHERE ([Year] = {0} AND [Quarter] = {1} AND [InsurerID] = {2} AND [FormatType] = 'adw1'); VACUUM;", 0, 0, Options.InsID);

                dbxml.Database.ExecuteSqlCommand(query);
            }
            catch
            {
                bwCancel("Ошибка", "Во время формирования пачек формы АДВ-1 произошла ошибка. Не выполнен запрос [ " + query + "]!");
                if (bw.CancellationPending)
                {
                    return;
                }

            }

            long parentID = 0;
            int num = (int)(numFrom.Value - 1);
            packs_cnt_ALL = (int)numFrom.Value;
            xmlInfo xml_info;
            string fileName;
            string regNum = Utils.ParseRegNum(db.Insurer.First(x => x.ID == Options.InsID).RegNum);

            try
            {
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

                adw1List = adw1List.Select(p => new { form = p, Staff = p.Staff }).ToList().OrderBy(sorting).Select(p => p.form).ToList();

                pfrXMLEntities dbxmlTemp = new pfrXMLEntities();

                double cnt = adw1List.Count();
                double packs_count_double = cnt / 200;
                int packs_cnt = Convert.ToInt16(Math.Ceiling(packs_count_double));
                int v = 0;
                for (int j = 0; j < packs_cnt; j++)
                {
                    List<FormsADW_1> adw1List_work = adw1List.Skip(j * 200).Take(200).ToList();

                    num++;
                    fileName = String.Format("PFR-700-Y-{0}-ORG-{1}-DCK-{2}-DPT-{3}-DCK-{4}.XML", DateTime.Now.Year.ToString(), regNum, num.ToString().PadLeft(5, '0'), "000000", "00000");

                    int c = adw1List_work.Count();
                    xml_info = new xmlInfo
                    {
                        Num = num,
                        CountDoc = c,
                        CountStaff = c,
                        DocType = "АДВ-1",
                        DateCreate = DateUnderwrite.Value,
                        FileName = fileName,
                        Year = 0,
                        Quarter = 0,
                        UniqGUID = guid,
                        InsurerID = Options.InsID,
                        FormatType = "adw1"
                    };
                    if (parentID != 0)
                        xml_info.ParentID = parentID;

                    int k = 0;
                    var _t = adw1List_work.Select(x => x.StaffID).ToList();
                    pu6Entities dbTemp = new pu6Entities();
                    var tStaffList = dbTemp.Staff.Where(x => _t.Contains(x.ID));

                    List<StaffList> staffList_ = new List<StaffList>();

                    foreach (var adw1Item in adw1List_work)
                    {

                        k++;
                        var staffItem = tStaffList.First(x => x.ID == adw1Item.StaffID);

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
                            DateCreate = DateUnderwrite.Value,
                            FormsRSW_6_1_ID = adw1Item.ID
                        };
                        staffList_.Add(staffListNewItem);

                    }
                    dbTemp.Dispose();
                    foreach (var item in staffList_)
                    {
                        xml_info.StaffList.Add(item);
                    }
                    dbxmlTemp.xmlInfo.Add(xml_info);
                    //       dbxml.Entry(xml_info, EntityState.Modified);
                    dbxmlTemp.SaveChanges();

                    v++;

                }
            }
            catch
            { }
        }

    }
}
