using System.Drawing; 
namespace PU.FormsRSW2014
{
    partial class TariffPlatFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.WinControls.Data.FilterDescriptor filterDescriptor1 = new Telerik.WinControls.Data.FilterDescriptor();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            this.radButton4 = new Telerik.WinControls.UI.RadButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radButton5 = new Telerik.WinControls.UI.RadButton();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.copyPanel = new Telerik.WinControls.UI.RadPanel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.copySpin = new Telerik.WinControls.UI.RadSpinEditor();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.copyPanel)).BeginInit();
            this.copyPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.copySpin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton2
            // 
            this.radButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton2.Location = new System.Drawing.Point(590, 245);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(90, 24);
            this.radButton2.TabIndex = 1;
            this.radButton2.Text = "Изменить";
            this.radButton2.ThemeName = "Office2013Light";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton3
            // 
            this.radButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton3.Location = new System.Drawing.Point(590, 275);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(90, 24);
            this.radButton3.TabIndex = 2;
            this.radButton3.Text = "Удалить";
            this.radButton3.ThemeName = "Office2013Light";
            this.radButton3.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // radButton4
            // 
            this.radButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton4.Location = new System.Drawing.Point(590, 333);
            this.radButton4.Name = "radButton4";
            this.radButton4.Size = new System.Drawing.Size(90, 24);
            this.radButton4.TabIndex = 3;
            this.radButton4.Text = "Закрыть";
            this.radButton4.ThemeName = "Office2013Light";
            this.radButton4.Click += new System.EventHandler(this.radButton4_Click);
            // 
            // radButton1
            // 
            this.radButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton1.Location = new System.Drawing.Point(590, 215);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(90, 24);
            this.radButton1.TabIndex = 0;
            this.radButton1.Text = "Добавить";
            this.radButton1.ThemeName = "Office2013Light";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radGridView1
            // 
            this.radGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridView1.EnableCustomSorting = true;
            this.radGridView1.EnableHotTracking = false;
            this.radGridView1.Location = new System.Drawing.Point(9, 12);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView1.MasterTemplate.EnableCustomSorting = true;
            this.radGridView1.MasterTemplate.EnableGrouping = false;
            filterDescriptor1.IsFilterEditor = true;
            this.radGridView1.MasterTemplate.FilterDescriptors.AddRange(new Telerik.WinControls.Data.FilterDescriptor[] {
            filterDescriptor1});
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ShowRowErrors = false;
            this.radGridView1.Size = new System.Drawing.Size(564, 345);
            this.radGridView1.TabIndex = 4;
            this.radGridView1.ThemeName = "Office2013Light";
            // 
            // radButton5
            // 
            this.radButton5.Location = new System.Drawing.Point(6, 43);
            this.radButton5.Name = "radButton5";
            this.radButton5.Size = new System.Drawing.Size(90, 24);
            this.radButton5.TabIndex = 68;
            this.radButton5.Text = "Копировать";
            this.radButton5.ThemeName = "Office2013Light";
            this.radButton5.Click += new System.EventHandler(this.radButton5_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.Location = new System.Drawing.Point(591, 12);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(90, 24);
            this.linkLabel1.TabIndex = 69;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Копировать";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // copyPanel
            // 
            this.copyPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.copyPanel.Controls.Add(this.radLabel1);
            this.copyPanel.Controls.Add(this.copySpin);
            this.copyPanel.Controls.Add(this.radButton5);
            this.copyPanel.Location = new System.Drawing.Point(582, 40);
            this.copyPanel.Name = "copyPanel";
            this.copyPanel.Size = new System.Drawing.Size(101, 74);
            this.copyPanel.TabIndex = 70;
            this.copyPanel.Visible = false;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(4, 9);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(26, 19);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "Год";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // copySpin
            // 
            this.copySpin.Location = new System.Drawing.Point(36, 9);
            this.copySpin.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.copySpin.Minimum = new decimal(new int[] {
            1900,
            0,
            0,
            0});
            this.copySpin.Name = "copySpin";
            this.copySpin.NullableValue = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            this.copySpin.Size = new System.Drawing.Size(61, 20);
            this.copySpin.TabIndex = 0;
            this.copySpin.TabStop = false;
            this.copySpin.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.copySpin.Value = new decimal(new int[] {
            2015,
            0,
            0,
            0});
            // 
            // TariffPlatFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 370);
            this.Controls.Add(this.copyPanel);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton3);
            this.Controls.Add(this.radButton4);
            this.Controls.Add(this.radGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 660);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "TariffPlatFrm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(1200, 660);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Тарифы страховых взносов";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.TariffPlatFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.copyPanel)).EndInit();
            this.copyPanel.ResumeLayout(false);
            this.copyPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.copySpin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton3;
        private Telerik.WinControls.UI.RadButton radButton4;
        private Telerik.WinControls.UI.RadButton radButton1;
        public Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadButton radButton5;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Telerik.WinControls.UI.RadPanel copyPanel;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadSpinEditor copySpin;
    }
}
