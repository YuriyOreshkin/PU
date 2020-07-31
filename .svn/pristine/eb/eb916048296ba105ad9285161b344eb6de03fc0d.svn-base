using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using PU.Models;
using System.Linq;
using PU.Classes;
using Telerik.WinControls.UI;

namespace PU.FormsSZV_STAJ
{
    public partial class SZV_STAJ_CreateSZVKORR : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();

        public List<long> staffList_temp { get; set; }
        public long currentStaffId = 0;
        public FormsODV_1_2017 odv1Data { get; set; }
        public bool Updated = false;

        public SZV_STAJ_CreateSZVKORR()
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

        private void SZV_STAJ_CreateSZVKORR_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            DateUnderwrite.Value = DateTime.Now.Date;

            List<RaschetPeriodContainer> avail_periods_all = new List<RaschetPeriodContainer>();

            avail_periods_all = Options.RaschetPeriodInternal2017.OrderByDescending(x => x.Year).ToList();


            foreach (var item in avail_periods_all.Where(x => x.Year >= 2017).Select(x => x.Year).ToList().Distinct())
            {
                Year.Items.Add(new RadListDataItem(item.ToString(), item.ToString()));
            }


        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            if ((switchStaffListDDL.SelectedIndex == 2 && (staffList_temp == null || (staffList_temp != null && staffList_temp.Count <= 0))) || (switchStaffListDDL.SelectedIndex == 1 && currentStaffId == 0))
            {
                RadMessageBox.Show("Пустой список сотрудников для формирования! Необходимо выбрать сотрудников!", "Внимание");
                return;
            }

            this.Cursor = Cursors.WaitCursor;


            List<FormsSZV_STAJ_2017> odv1SZVSTAJList = new List<FormsSZV_STAJ_2017>();

            switch (switchStaffListDDL.SelectedIndex)
            {
                case 0: //все записи
                    odv1SZVSTAJList = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID).ToList();
                    break;
                case 1: //текущая запись
                    odv1SZVSTAJList = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID && x.ID == currentStaffId).ToList();
                    break;
                case 2: //по выделенным записям
                    odv1SZVSTAJList = db.FormsSZV_STAJ_2017.Where(x => x.FormsODV_1_2017_ID == odv1Data.ID && staffList_temp.Contains(x.ID)).ToList();
                    break;
            }



            FormsODV_1_2017 odv1_new = new FormsODV_1_2017() { 
                TypeForm = 3,
                TypeInfo = 0,
                Year = short.Parse(Year.Text),
                Code = 0,
                DateFilling = DateUnderwrite.Value,
                ConfirmLastName = odv1Data.ConfirmLastName,
                ConfirmFirstName = odv1Data.ConfirmFirstName,
                ConfirmMiddleName = odv1Data.ConfirmMiddleName,
                ConfirmDolgn = odv1Data.ConfirmDolgn,
                InsurerID = Options.InsID,
                StaffCount = odv1SZVSTAJList.Count(),
                StaffCountOsobUslFakt = 0,
                StaffCountOsobUslShtat = 0
            };

            db.FormsODV_1_2017.AddObject(odv1_new);
            db.SaveChanges();

            Insurer ins = db.Insurer.First(x => x.ID == Options.InsID);


            int i = 0;
            foreach (var item in odv1SZVSTAJList)
            {
                FormsSZV_KORR_2017 szv_korr = new FormsSZV_KORR_2017
                {
                    Year = short.Parse(Year.Text),
                    Code = 0,
                    YearKorr = item.Year,
                    CodeKorr = item.Code,
                    TypeInfo = 0,
                    InsurerID = Options.InsID,
                    RegNumKorr = ins.RegNum,
                    INNKorr = ins.INN,
                    KPPKorr = ins.KPP,
                    ShortNameKorr = ins.NameShort,
                    DateFilling = DateUnderwrite.Value,
                    FormsODV_1_2017_ID = odv1_new.ID,
                    StaffID = item.StaffID,
                    ContractType = (byte)0,
                    DopTarCode = ""
                };


                foreach (var staj in item.StajOsn.ToList())
                {
                    StajOsn stajOsn = staj.Clone();
                    
                    //StajOsn stajOsn = new StajOsn
                    //{
                    //    Number = staj.Number,
                    //    DateBegin = staj.DateBegin,
                    //    DateEnd = staj.DateEnd,
                    //    CodeBEZR = staj.CodeBEZR
                    //};
                    foreach (var stajL in staj.StajLgot.ToList())
                    {
                        StajLgot stajLgot = stajL.Clone();
                        stajLgot.StajOsn = null;
                        stajLgot.StajOsnID = 0;
                    //    StajLgot stajLgot = new StajLgot
                    //    {
                    //        DolgnID = stajL.DolgnID,
                    //        IschislStrahStajDopID = stajL.IschislStrahStajDopID,
                    //        IschislStrahStajOsnID = stajL.IschislStrahStajOsnID,
                    //        //KodVred_2 = stajL.KodVred_2,
                    //        //KodVred_3 = stajL.KodVred_3,
                    //        KodVred_DopID = stajL.KodVred_DopID,
                    //        KodVred_OsnID = stajL.KodVred_OsnID,
                    //        Number = stajL.Number,
                    //        OsobUslTrudaID = stajL.OsobUslTrudaID,
                    //        Strah1Param = stajL.Strah1Param,
                    //        Strah2Param = stajL.Strah2Param,
                    //        TerrUslID = stajL.TerrUslID,
                    //        TerrUslKoef = stajL.TerrUslKoef,
                    //        UslDosrNaznID = stajL.UslDosrNaznID,
                    //        UslDosrNazn1Param = stajL.UslDosrNazn1Param,
                    //        UslDosrNazn2Param = stajL.UslDosrNazn2Param,
                    //        UslDosrNazn3Param = stajL.UslDosrNazn3Param
                    //    };

                        stajOsn.StajLgot.Add(stajLgot);
                    }
                    stajOsn.FormsSZV_STAJ_2017_ID = null;
                    stajOsn.FormsSZV_STAJ_2017 = null;
                    //szv_korr.StajOsn.Add(stajOsn);
                    szv_korr.StajOsn.Add(stajOsn);

                }

                db.FormsSZV_KORR_2017.AddObject(szv_korr);
                i++;

                if (i % 100 == 0)
                    db.SaveChanges();

            }


            db.SaveChanges();
            Updated = true;
            this.Cursor = Cursors.Default;
            this.Close();
        }
    }
}
