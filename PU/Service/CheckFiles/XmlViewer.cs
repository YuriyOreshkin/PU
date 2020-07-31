using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Telerik.WinControls;

namespace PU.Service.CheckFiles
{
    public partial class XmlViewer : Telerik.WinControls.UI.RadForm
    {
        public string xmlPath { get; set; }

        public XmlViewer()
        {
            InitializeComponent();
        }

        private void XmlViewer_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            loadXmlToWebBrowser();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editBtn_Click(object sender, EventArgs e)
        {


            try
            {
                radTextBox1.Text = "";
                // Load the XML Document
                XDocument xDocument = XDocument.Load(xmlPath);
                //radTextBoxControl1.Text = xDocument.ToString();

            //    string xmlText = File.ReadAllText(xmlPath);

                radTextBox1.Text = xDocument.ToString(); ;
                radTextBox1.SelectionStart = radTextBox1.Text.Length;

                webBrowser.Visible = false;
                radTextBox1.Visible = true;
                editBtn.Visible = false;
                saveBtn.Visible = true;
                abortBtn.Visible = true;
            }
            catch (Exception err)
            {

                MessageBox.Show("Во время загрузки XML-файла произошла ошибка! Код ошибки - " + err.Message);
                return;
            }
        }

        private void abortBtn_Click(object sender, EventArgs e)
        {
            webBrowser.Visible = true;
            radTextBox1.Visible = false;
            radTextBox1.Text = "";
            loadXmlToWebBrowser();
            editBtn.Visible = true;
            saveBtn.Visible = false;
            abortBtn.Visible = false;

        }

        private void loadXmlToWebBrowser()
        {
            try
            {
                webBrowser.Navigate(new Uri(xmlPath));
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Во время загрузки XML-файла произошла ошибка! Код ошибки - " + ex.Message);
            }

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(radTextBox1.Text))
            {
                RadMessageBox.Show("Нечего сохранять!");
            }

            try
            {
                XDocument doc = XDocument.Parse(radTextBox1.Text);

                doc.Save(xmlPath);

                abortBtn_Click(null, null);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Во время сохранения XML-файла произошла ошибка! Код ошибки - " + ex.Message);
            }
        }



    }
}
