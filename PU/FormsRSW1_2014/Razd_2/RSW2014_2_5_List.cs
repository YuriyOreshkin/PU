using PU.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using PU.Classes;
using System.Xml.Linq;

namespace PU.FormsRSW2014
{
    public partial class RSW2014_2_5_List : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        pfrXMLEntities dbxml = new pfrXMLEntities();
        public rsw2014PackIdent ident = new rsw2014PackIdent();
        public FormsRSW2014_1_1 rsw {get;set;}
        private xmlInfo xmlInfoItem { get; set; }

        public class rsw2014PackIdent
        {
            public short Year { get; set; }
            public byte Quarter { get; set; }
            public long InsurerID { get; set; }
            public string FormatType { get; set; }
        }

        public RSW2014_2_5_List()
        {
            InitializeComponent();
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void RSW2014_2_5_List_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();

            if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "РСВ"))
            {
                xmlInfoItem = dbxml.xmlInfo.FirstOrDefault(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && x.DocType == "РСВ");
                rsw = db.FormsRSW2014_1_1.FirstOrDefault(x => x.ID == xmlInfoItem.SourceID.Value);

            }

            if (rsw != null)
            {
                updateRazd251Grid();
                updateRazd252Grid();
            }
        }

        private void updateRazd251Grid()
        {
            var rsw251List = db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.NumRec).ToList();

            this.razd251Grid.TableElement.BeginUpdate();

            razd251Grid.DataSource = null;
            razd251Grid.DataSource = rsw251List;

            razd251Grid.Columns["ID"].IsVisible = false;
            razd251Grid.Columns["ID"].IsPinned = true;
            razd251Grid.Columns["Year"].IsVisible = false;
            razd251Grid.Columns["Quarter"].IsVisible = false;
            razd251Grid.Columns["InsurerID"].IsVisible = false;
            razd251Grid.Columns["CorrectionNum"].IsVisible = false;
            razd251Grid.Columns["NumRec"].Width = 50;
            razd251Grid.Columns["NumRec"].IsPinned = true;
            razd251Grid.Columns["NumRec"].ReadOnly = true;
            razd251Grid.Columns["NumRec"].TextAlignment = ContentAlignment.MiddleCenter;
            razd251Grid.Columns["NumRec"].HeaderText = "№";
            razd251Grid.Columns["Col_2"].Width = 240;
            razd251Grid.Columns["Col_2"].TextAlignment = ContentAlignment.MiddleCenter;
            razd251Grid.Columns["Col_2"].HeaderText = "База для начисления взносов на ОПС";
            razd251Grid.Columns["Col_2"].FormatString = "{0:N2}";
            razd251Grid.Columns["Col_3"].Width = 210;
            razd251Grid.Columns["Col_3"].TextAlignment = ContentAlignment.MiddleCenter;
            razd251Grid.Columns["Col_3"].HeaderText = "Начисленные страховые взносы";
            razd251Grid.Columns["Col_3"].FormatString = "{0:N2}";
            razd251Grid.Columns["Col_4"].Width = 100;
            razd251Grid.Columns["Col_4"].ReadOnly = true;
            razd251Grid.Columns["Col_4"].TextAlignment = ContentAlignment.MiddleCenter;
            razd251Grid.Columns["Col_4"].HeaderText = "Кол-во ЗЛ";
            razd251Grid.Columns["Col_5"].Width = 600;
            razd251Grid.Columns["Col_5"].ReadOnly = true;
            razd251Grid.Columns["Col_5"].HeaderText = "Имя файла";
            
            for (var i = 1; i < razd251Grid.Columns.Count; i++)
            {
                razd251Grid.Columns[i].HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            }

            this.razd251Grid.TableElement.EndUpdate();

            if (rsw251List.Count() > 0)
            {
                sumOPS251.Text = Utils.decToStr(rsw251List.Sum(x => x.Col_2.Value));
                sumFee251.Text = Utils.decToStr(rsw251List.Sum(x => x.Col_3.Value));
                cntZL251.Text = rsw251List.Sum(x => x.Col_4.Value).ToString();
            }
            else
            {
                sumOPS251.Text = "0.00";
                sumFee251.Text = "0.00";
                cntZL251.Text = "0";
            }

        }

        private void updateRazd252Grid()
        {
            var rsw252List = db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum).OrderBy(x => x.NumRec).ToList();

            this.razd252Grid.TableElement.BeginUpdate();

            razd252Grid.DataSource = null;
            razd252Grid.DataSource = rsw252List;

            razd252Grid.Columns["ID"].IsVisible = false;
            razd252Grid.Columns["ID"].IsPinned = true;
            razd252Grid.Columns["Year"].IsVisible = false;
            razd252Grid.Columns["Quarter"].IsVisible = false;
            razd252Grid.Columns["InsurerID"].IsVisible = false;
            razd252Grid.Columns["CorrectionNum"].IsVisible = false;
            razd252Grid.Columns["NumRec"].Width = 50;
            razd252Grid.Columns["NumRec"].IsPinned = true;
            razd252Grid.Columns["NumRec"].ReadOnly = true;
            razd252Grid.Columns["NumRec"].TextAlignment = ContentAlignment.MiddleCenter;
            razd252Grid.Columns["NumRec"].HeaderText = "№";
            razd252Grid.Columns["Col_3_YearKorr"].Width = 100;
            razd252Grid.Columns["Col_3_YearKorr"].TextAlignment = ContentAlignment.MiddleCenter;
            razd252Grid.Columns["Col_3_YearKorr"].HeaderText = "Год корр.";
            razd252Grid.Columns["Col_3_YearKorr"].ReadOnly = true;
            razd252Grid.Columns["Col_2_QuarterKorr"].Width = 160;
            razd252Grid.Columns["Col_2_QuarterKorr"].TextAlignment = ContentAlignment.MiddleCenter;
            razd252Grid.Columns["Col_2_QuarterKorr"].HeaderText = "Период корр.";
            razd252Grid.Columns["Col_2_QuarterKorr"].ReadOnly = true;
            razd252Grid.Columns["Col_4"].Width = 190;
            razd252Grid.Columns["Col_4"].TextAlignment = ContentAlignment.MiddleCenter;
            razd252Grid.Columns["Col_4"].HeaderText = "на ОПС с 2014 года";
            razd252Grid.Columns["Col_4"].FormatString = "{0:N2}";
            razd252Grid.Columns["Col_5"].Width = 190;
            razd252Grid.Columns["Col_5"].TextAlignment = ContentAlignment.MiddleCenter;
            razd252Grid.Columns["Col_5"].HeaderText = "Страховая часть";
            razd252Grid.Columns["Col_5"].FormatString = "{0:N2}";
            razd252Grid.Columns["Col_6"].Width = 190;
            razd252Grid.Columns["Col_6"].TextAlignment = ContentAlignment.MiddleCenter;
            razd252Grid.Columns["Col_6"].HeaderText = "Накопит. часть";
            razd252Grid.Columns["Col_6"].FormatString = "{0:N2}";
            razd252Grid.Columns["Col_7"].Width = 100;
            razd252Grid.Columns["Col_7"].ReadOnly = true;
            razd252Grid.Columns["Col_7"].HeaderText = "Кол-во ЗЛ";
            razd252Grid.Columns["Col_8"].Width = 600;
            razd252Grid.Columns["Col_8"].ReadOnly = true;
            razd252Grid.Columns["Col_8"].HeaderText = "Имя файла";
            
            for (var i = 1; i < razd252Grid.Columns.Count; i++)
            {
                razd252Grid.Columns[i].HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            }

            this.razd252Grid.TableElement.EndUpdate();


            if (rsw252List.Count() > 0)
            {
                sumOPS252.Text = Utils.decToStr(rsw252List.Sum(x => x.Col_4.Value));
                sumStrah252.Text = Utils.decToStr(rsw252List.Sum(x => x.Col_5.Value));
                sumNakop252.Text = Utils.decToStr(rsw252List.Sum(x => x.Col_6.Value));
                cntZL252.Text = rsw252List.Sum(x => x.Col_7.Value).ToString();
            }
            else
            {
                sumOPS252.Text = "0.00";
                sumStrah252.Text = "0.00";
                sumNakop252.Text = "0.00";
                cntZL252.Text = "0";
            }

        }

        private void radButton1_Click_1(object sender, EventArgs e)
        {
            db.SaveChanges();

            //if (xmlInfoItem != null && rsw != null)
            //{
            //    try
            //    {
            //        xmlFile xmlFileItem = xmlInfoItem.xmlFile.First();

            //        XDocument doc = XDocument.Parse(xmlFileItem.XmlContent);
            //        doc.Declaration = new XDeclaration("1.0", "Windows-1251", "yes");

            //        rsw2014packs child = new rsw2014packs();
            //        doc = child.updateRSV1_Razd_2_5(doc, rsw.ID);

            //        xmlFileItem.XmlContent = doc.ToString();

            //        dbxml.Entry(xmlFileItem, EntityState.Modified);
            //        dbxml.SaveChanges();
            //        Methods.showAlert("Сохранение", "Сохранение данных прошло успешно!", this.ThemeName);
            //    }
            //    catch (Exception ex)
            //    {
            //        Methods.showAlert("Внимание! Ошибка.", "При обновлении структуры XML файла произошла ошибка. Сообщение об ошиобке: " + ex.Message, this.ThemeName);
            //    }
            //}

        }

        private void delBtn_Click(object sender, EventArgs e)
        {
            if (rsw != null)
            {
                DialogResult dialogResult = RadMessageBox.Show(this, "Вы уверены в том, что желаете удалить Раздел 2.5", "Внимание! Удаление записи.", MessageBoxButtons.YesNo, RadMessageIcon.Info, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.Yes)
                {
                    if (!deleteRazd25())
                    {
                        RadMessageBox.Show(this, "Во время удаления записи произошла ошибка.", "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                    updateRazd251Grid();
                    updateRazd252Grid();

                }
            }
        }

        private void fillByXmlBtn_Click(object sender, EventArgs e)
        {
            if (rsw != null && deleteRazd25())
            {
                if (dbxml.xmlInfo.Any(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && (x.DocType == "ПФР" || x.DocType == "СЗВ-6-4" || x.DocType == "СЗВ-6")))
                {
                    var xmlInfoList = dbxml.xmlInfo.Where(x => x.Year == ident.Year && x.Quarter == ident.Quarter && x.InsurerID == ident.InsurerID && x.FormatType == ident.FormatType && (x.DocType == "ПФР"  || x.DocType == "СЗВ-6-4" || x.DocType == "СЗВ-6"));

                    bool flag = false;


                    int i_num = 0;
                    int j_num = 0;
                    flag = false;
                    foreach (var item in xmlInfoList)
                    {
                        flag = true;
                        if (item.YearKorr == null)
                        {
                            i_num++;

                            FormsRSW2014_1_Razd_2_5_1 rsw251 = new FormsRSW2014_1_Razd_2_5_1
                            {
                                InsurerID = rsw.InsurerID,
                                Year = rsw.Year,
                                Quarter = rsw.Quarter,
                                CorrectionNum = rsw.CorrectionNum,
                                NumRec = i_num,
                                Col_2 = item.rsw2014.First().RSW_2_5_1_2,
                                Col_3 = item.rsw2014.First().RSW_2_5_1_3,
                                Col_4 = item.CountStaff,
                                Col_5 = item.FileName
                            };

                            db.FormsRSW2014_1_Razd_2_5_1.Add(rsw251);

                        }

                        if (item.YearKorr != null)
                        {
                            j_num++;

                            FormsRSW2014_1_Razd_2_5_2 rsw252 = new FormsRSW2014_1_Razd_2_5_2
                            {
                                InsurerID = rsw.InsurerID,
                                Year = rsw.Year,
                                Quarter = rsw.Quarter,
                                CorrectionNum = rsw.CorrectionNum,
                                NumRec = j_num,
                                Col_2_QuarterKorr = item.QuarterKorr.Value,
                                Col_3_YearKorr = item.YearKorr.Value,
                                Col_4 = item.rsw2014.First().RSW_2_5_2_4,
                                Col_5 = item.rsw2014.First().RSW_2_5_2_5,
                                Col_6 = item.rsw2014.First().RSW_2_5_2_6,
                                Col_7 = item.CountStaff,
                                Col_8 = item.FileName
                            };

                            db.FormsRSW2014_1_Razd_2_5_2.Add(rsw252);


                        }

                    }

                    if (flag)
                    {
                        db.SaveChanges();
                    }
                    updateRazd251Grid();
                    updateRazd252Grid();

                }

            }
        }

        private bool deleteRazd25()
        {
            bool result = true;



            try
            {
                foreach (var item in db.FormsRSW2014_1_Razd_2_5_1.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
                {
                    db.FormsRSW2014_1_Razd_2_5_1.Remove(item);
                }
                foreach (var item in db.FormsRSW2014_1_Razd_2_5_2.Where(x => x.InsurerID == rsw.InsurerID && x.Year == rsw.Year && x.Quarter == rsw.Quarter && x.CorrectionNum == rsw.CorrectionNum))
                {
                    db.FormsRSW2014_1_Razd_2_5_2.Remove(item);
                }
                db.SaveChanges();
            }
            catch
            {
                result = false;
            }

            return result;
        }

        private void razd251Grid_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            updateRazd251Grid();

        }

        private void razd252Grid_CellEndEdit(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
        {
            updateRazd252Grid();
        }
    }
}

