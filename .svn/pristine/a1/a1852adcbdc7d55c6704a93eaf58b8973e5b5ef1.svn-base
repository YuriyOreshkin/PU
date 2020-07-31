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

namespace PU.FormsADW1
{
    public partial class AddressEditor : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public string address { get; set; }

        public AddressEditor()
        {
            InitializeComponent();
        }

        private void Index_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && (e.KeyChar < 48 || e.KeyChar > 57))
                e.Handled = true;
        }

        private void AddressEditor_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            fillCombobox();

            if (!String.IsNullOrEmpty(address))
            { 
                string[] t = address.Split(',');
                string cntr = t[0] == "" ? "РОССИЯ" : t[0];
                if (Country.Items.Any(x => x.Value.ToString() == cntr))
                {
                    Country.Items.First(x => x.Value.ToString() == cntr).Selected = true;
                }
                else
                {
                    Country.Text = t[0] == "" ? "РОССИЯ" : t[0];
                }
                if (Region.Items.Any(x => x.Value.ToString() == t[2]))
                {
                    Region.Items.First(x => x.Value.ToString() == t[2]).Selected = true;
                }
                else
                {
                    Region.Text = t[2];
                }
                if (Raion.Items.Any(x => x.Value.ToString() == t[3]))
                {
                    Raion.Items.First(x => x.Value.ToString() == t[3]).Selected = true;
                }
                else
                {
                    Raion.Text = t[3];
                }
                if (City.Items.Any(x => x.Value.ToString() == t[4]))
                {
                    City.Items.First(x => x.Value.ToString() == t[4]).Selected = true;
                }
                else
                {
                    City.Text = t[4];
                }
                if (Punkt.Items.Any(x => x.Value.ToString() == t[5]))
                {
                    Punkt.Items.First(x => x.Value.ToString() == t[5]).Selected = true;
                }
                else
                {
                    Punkt.Text = t[5];
                }
                if (Street.Items.Any(x => x.Value.ToString() == t[6]))
                {
                    Street.Items.First(x => x.Value.ToString() == t[6]).Selected = true;
                }
                else
                {
                    Street.Text = t[6];
                }

                Index.Text = t[1];
                Dom.Text = t[7];
                Korp.Text = t[8];
                Kvart.Text = t[9];

            }
        }

        private void fillCombobox()
        {
            List<string> addressList = db.FormsADW_1.Where(x => !String.IsNullOrEmpty(x.Reg_Addr)).Select(x => x.Reg_Addr).ToList();
            addressList.AddRange(db.FormsADW_1.Where(x => !String.IsNullOrEmpty(x.Fakt_Addr)).Select(x => x.Fakt_Addr).ToList());

            List<string[]> t = addressList.Select(x => x.Split(',')).ToList();

            List<string> cntrL = t.Select(x => x[0]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            cntrL.Insert(0, "РОССИЯ");
            Country.DataSource = cntrL;
            Region.DataSource = t.Select(x => x[2]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Raion.DataSource = t.Select(x => x[3]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            City.DataSource = t.Select(x => x[4]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Punkt.DataSource = t.Select(x => x[5]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Street.DataSource = t.Select(x => x[6]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();


            Country.Text = Region.Text = Raion.Text = City.Text = Punkt.Text = Street.Text = "";

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            string cntr = Country.Text.Trim() == "РОССИЯ" ? "" : Country.Text.Trim().ToUpper();

            string nomer = Dom.Text.Trim() + "," + Korp.Text.Trim() + "," + Kvart.Text.Trim();
            if (addAbbr.Checked)
            {
                nomer = ((Dom.Text.Trim() != "" && !Dom.Text.Trim().Contains("Д.")) ? ("Д." + Dom.Text.Trim()) : "") + "," + ((Korp.Text.Trim() != "" && !Korp.Text.Trim().Contains("КОРП.")) ? ("КОРП." + Korp.Text.Trim()) : "") + "," + ((Kvart.Text.Trim() != "" && !Kvart.Text.Trim().Contains("КВ.")) ? ("КВ." + Kvart.Text.Trim()) : "");
            }

            address = cntr + "," + Index.Text.Trim() + "," + Region.Text.Trim().ToUpper() + "," + Raion.Text.Trim().ToUpper() + "," + City.Text.Trim().ToUpper() + "," + Punkt.Text.Trim().ToUpper() + "," + Street.Text.Trim().ToUpper() + "," + nomer;

            this.Close();
        }
    }
}
