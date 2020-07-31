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
            Country.Text = "РОССИЯ";
            if (!String.IsNullOrEmpty(address))
            { 
                string[] t = address.Split(',');
                string cntr = t[0] == "" ? "РОССИЯ" : t[0];
                if (t.Length == 18)
                {
                    Index.Text = t[1];


                    if (Country.Items.Any(x => x.Value.ToString() == cntr))
                    {
                        Country.Items.First(x => x.Value.ToString() == cntr).Selected = true;
                    }
                    else
                    {
                        Country.Text = t[0] == "" ? "РОССИЯ" : t[0];
                    }


                    if (Region_short.Items.Any(x => x.Value.ToString() == t[2]))
                    {
                        Region_short.Items.First(x => x.Value.ToString() == t[2]).Selected = true;
                    }
                    else
                    {
                        Region_short.Text = t[2];
                    }
                    if (Region.Items.Any(x => x.Value.ToString() == t[3]))
                    {
                        Region.Items.First(x => x.Value.ToString() == t[3]).Selected = true;
                    }
                    else
                    {
                        Region.Text = t[3];
                    }


                    if (Raion_short.Items.Any(x => x.Value.ToString() == t[4]))
                    {
                        Raion_short.Items.First(x => x.Value.ToString() == t[4]).Selected = true;
                    }
                    else
                    {
                        Raion_short.Text = t[4];
                    }
                    if (Raion.Items.Any(x => x.Value.ToString() == t[5]))
                    {
                        Raion.Items.First(x => x.Value.ToString() == t[5]).Selected = true;
                    }
                    else
                    {
                        Raion.Text = t[5];
                    }


                    if (City_short.Items.Any(x => x.Value.ToString() == t[6]))
                    {
                        City_short.Items.First(x => x.Value.ToString() == t[6]).Selected = true;
                    }
                    else
                    {
                        City_short.Text = t[6];
                    }
                    if (City.Items.Any(x => x.Value.ToString() == t[7]))
                    {
                        City.Items.First(x => x.Value.ToString() == t[7]).Selected = true;
                    }
                    else
                    {
                        City.Text = t[7];
                    }


                    if (Punkt_short.Items.Any(x => x.Value.ToString() == t[8]))
                    {
                        Punkt_short.Items.First(x => x.Value.ToString() == t[8]).Selected = true;
                    }
                    else
                    {
                        Punkt_short.Text = t[8];
                    }
                    if (Punkt.Items.Any(x => x.Value.ToString() == t[9]))
                    {
                        Punkt.Items.First(x => x.Value.ToString() == t[9]).Selected = true;
                    }
                    else
                    {
                        Punkt.Text = t[9];
                    }


                    if (Street_short.Items.Any(x => x.Value.ToString() == t[10]))
                    {
                        Street_short.Items.First(x => x.Value.ToString() == t[10]).Selected = true;
                    }
                    else
                    {
                        Street_short.Text = t[10];
                    }
                    if (Street.Items.Any(x => x.Value.ToString() == t[11]))
                    {
                        Street.Items.First(x => x.Value.ToString() == t[11]).Selected = true;
                    }
                    else
                    {
                        Street.Text = t[11];
                    }

                    Dom_short.Text = t[12];
                    Dom.Text = t[13];
                    Korp_short.Text = t[14];
                    Korp.Text = t[15];
                    Kvart_short.Text = t[16];
                    Kvart.Text = t[17];
                }

            }
        }

        private void fillCombobox()
        {
            List<string> addressList = db.FormsADW_1.Where(x => !String.IsNullOrEmpty(x.Reg_Addr)).Select(x => x.Reg_Addr).ToList();
            addressList.AddRange(db.FormsADW_1.Where(x => !String.IsNullOrEmpty(x.Fakt_Addr)).Select(x => x.Fakt_Addr).ToList());

            List<string[]> tt = addressList.Select(x => x.Split(',')).ToList();

            List<string> cntrL = tt.Select(x => x[0]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            cntrL.Insert(0, "РОССИЯ");
            Country.DataSource = cntrL;

            List<string[]> t = tt.Where(x => x.Length == 18).ToList();

            Region_short.DataSource = t.Select(x => x[2]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Region.DataSource = t.Select(x => x[3]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Raion_short.DataSource = t.Select(x => x[4]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Raion.DataSource = t.Select(x => x[5]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            City_short.DataSource = t.Select(x => x[6]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            City.DataSource = t.Select(x => x[7]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Punkt_short.DataSource = t.Select(x => x[8]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Punkt.DataSource = t.Select(x => x[9]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Street_short.DataSource = t.Select(x => x[10]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();
            Street.DataSource = t.Select(x => x[11]).Where(x => !String.IsNullOrEmpty(x)).Distinct().ToList();


            Country.Text = Region.Text = Raion.Text = City.Text = Punkt.Text = Street.Text = "";

        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            string cntr = Country.Text.Trim() == "РОССИЯ" ? "" : Country.Text.Trim().ToUpper();

            string nomer = Dom_short.Text.Trim() + "," + Dom.Text.Trim() + "," + Korp_short.Text.Trim() + "," + Korp.Text.Trim() + "," + Kvart_short.Text.Trim() + "," + Kvart.Text.Trim();

            address = cntr + "," + Index.Text.Trim()
                + "," + Region_short.Text.Trim().ToUpper() + "," + Region.Text.Trim().ToUpper()
                + "," + Raion_short.Text.Trim().ToUpper() + "," + Raion.Text.Trim().ToUpper()
                + "," + City_short.Text.Trim().ToUpper() + "," + City.Text.Trim().ToUpper()
                + "," + Punkt_short.Text.Trim().ToUpper() + "," + Punkt.Text.Trim().ToUpper()
                + "," + Street_short.Text.Trim().ToUpper() + "," + Street.Text.Trim().ToUpper()
                + "," + nomer;

            this.Close();
        }
    }
}
