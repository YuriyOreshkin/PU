using PU.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Xml.Linq;
using System.IO;
using PU.Models;

namespace PU.FormsPredPens
{
    public partial class PredPensSpravka_ImportXML : Telerik.WinControls.UI.RadForm
    {
        OpenFileDialog openDialog = new OpenFileDialog();
        PredPensSpravka_Print ReportViewerPredPens;

        public PredPensSpravka_ImportXML()
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

        private void PredPensSpravka_ImportXML_Load(object sender, EventArgs e)
        {
            openDialog.Filter = "(*.xml)|*.xml";
            openDialog.Multiselect = true;
            if (!String.IsNullOrEmpty(Options.CurrentInsurerFolders.importPath))
            {
                openDialog.InitialDirectory = Options.CurrentInsurerFolders.importPath;
            }

            if (Options.formParams.Any(x => x.name == this.Name))
            {
                var param = Options.formParams.FirstOrDefault(x => x.name == this.Name);
                try
                {
                    foreach (var item in param.windowData)
                    {
                        int i = 0;

                        switch (item.control)
                        {
                            case "openDialog":
                                openDialog.InitialDirectory = item.value;
                                break;
                        }

                    }


                }
                catch
                { }
            }
        }

        private void selectFileBtn_Click(object sender, EventArgs e)
        {
            if (this.openDialog.ShowDialog() == DialogResult.OK)
            {
                currentPath.Text = openDialog.FileName;
            }
        }


        private void bw_RunWorkerCompletedPredPens(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Invoke(new Action(() => { this.Cursor = Cursors.Default; }));

            ReportViewerPredPens.ShowDialog();
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            List<SPPVObject> ListSPPV = new List<SPPVObject>();

                            SPPVObject sppv = new SPPVObject{};

                try
                {
                    if (!File.Exists(currentPath.Text))
                    {
                        MessageBox.Show("Файл не найден на диске!");
                        return;
                    }



                    XDocument doc = new XDocument();
                    doc = XDocument.Load(currentPath.Text);

                    var ns = doc.Root.GetDefaultNamespace();
                    doc.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

                    foreach (var elem in doc.Descendants())
                        elem.Name = elem.Name.LocalName;

                    XElement node = doc.Root;

                    node = node.Element("СППВ");





                    var Гражданин = node.Elements("Гражданин");

                    foreach (var item in Гражданин)
                    {
                        sppv = new SPPVObject { };

                        sppv.Дата = node.Element("Дата").Value;
                        sppv.Орган = int.Parse(node.Element("Орган").Value);


                        sppv.Фамилия = item.Element("ФИО").Element("Фамилия").Value;
                        sppv.Имя = item.Element("ФИО").Element("Имя").Value;
                        sppv.Отчество = item.Element("ФИО").Element("Отчество").Value;
                        sppv.ДатаРождения = item.Element("ДатаРождения").Value;
                        sppv.СНИЛС = item.Element("СНИЛС").Value;

                        if (item.Element("СтатьяЗакона") != null)
                        {
                            if (item.Element("СтатьяЗакона").Element("НормативныйДокумент") != null)
                                sppv.НормативныйДокумент = item.Element("СтатьяЗакона").Element("НормативныйДокумент").Value;
                            if (item.Element("СтатьяЗакона").Element("Статья") != null)
                                sppv.Статья = item.Element("СтатьяЗакона").Element("Статья").Value;
                        }
                        sppv.ЯвляетсяГражданиномПредпенсионногоВозраста = int.Parse(item.Element("ЯвляетсяГражданиномПредпенсионногоВозраста").Value);

                        if (item.Element("Дата") != null)
                        {
                            sppv.ДатаС = item.Element("Дата").Value;
                        }


                        if (node.Element("ДолжностноеЛицо") != null)
                        {
                            XElement ДолжностноеЛицо = node.Element("ДолжностноеЛицо");

                            sppv.ДФамилия = ДолжностноеЛицо.Element("ФИО").Element("Фамилия").Value;
                            sppv.ДИмя = ДолжностноеЛицо.Element("ФИО").Element("Имя").Value;
                            sppv.ДОтчество = ДолжностноеЛицо.Element("ФИО").Element("Отчество").Value;
                            sppv.Должность = ДолжностноеЛицо.Element("Должность").Value;
                        }

                        ListSPPV.Add(sppv);
                    }












                    ReportViewerPredPens = new PredPensSpravka_Print();
                    ReportViewerPredPens.ListSPPV = ListSPPV;
                    ReportViewerPredPens.Owner = this;
                    ReportViewerPredPens.ThemeName = this.ThemeName;
                    ReportViewerPredPens.ShowInTaskbar = false;

                    this.Cursor = Cursors.WaitCursor;

                    BackgroundWorker bw = new BackgroundWorker();
                    bw.DoWork += new System.ComponentModel.DoWorkEventHandler(ReportViewerPredPens.createReport);
                    bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompletedPredPens);

                    bw.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,"Ошибка при загрузке данных из файла! \r\nКод ошибки - " + ex.Message,"Внимание!");
                }

            }

    }
}
