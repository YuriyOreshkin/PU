namespace PU
{
    partial class StaffFrm
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
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radButton4 = new Telerik.WinControls.UI.RadButton();
            this.staffGridView = new Telerik.WinControls.UI.RadGridView();
            this.radButton5 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton4
            // 
            this.radButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton4.Location = new System.Drawing.Point(711, 393);
            this.radButton4.Name = "radButton4";
            this.radButton4.Size = new System.Drawing.Size(90, 24);
            this.radButton4.TabIndex = 9;
            this.radButton4.Text = "Закрыть";
            this.radButton4.ThemeName = "Office2013Light";
            this.radButton4.Click += new System.EventHandler(this.radButton4_Click);
            // 
            // staffGridView
            // 
            this.staffGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staffGridView.Location = new System.Drawing.Point(12, 10);
            // 
            // 
            // 
            this.staffGridView.MasterTemplate.AllowAddNewRow = false;
            this.staffGridView.MasterTemplate.AllowDeleteRow = false;
            gridViewCheckBoxColumn1.Checked = Telerik.WinControls.Enumerations.ToggleState.Indeterminate;
            gridViewCheckBoxColumn1.EnableHeaderCheckBox = true;
            gridViewCheckBoxColumn1.HeaderText = "";
            gridViewCheckBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            gridViewCheckBoxColumn1.Name = "CheckBox";
            gridViewCheckBoxColumn1.StretchVertically = false;
            this.staffGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewCheckBoxColumn1});
            this.staffGridView.MasterTemplate.EnableFiltering = true;
            this.staffGridView.MasterTemplate.EnableGrouping = false;
            this.staffGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.staffGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.staffGridView.Name = "staffGridView";
            this.staffGridView.ShowRowErrors = false;
            this.staffGridView.Size = new System.Drawing.Size(684, 406);
            this.staffGridView.TabIndex = 5;
            this.staffGridView.ThemeName = "Office2013Light";
            this.staffGridView.CellClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView1_CellClick);
            this.staffGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView1_CellDoubleClick);
            this.staffGridView.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.staffGridView_ContextMenuOpening);
            this.staffGridView.FilterChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.staffGridView_FilterChanged);
            this.staffGridView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.staffGridView_KeyPress);
            // 
            // radButton5
            // 
            this.radButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton5.Location = new System.Drawing.Point(711, 169);
            this.radButton5.Name = "radButton5";
            this.radButton5.Size = new System.Drawing.Size(90, 24);
            this.radButton5.TabIndex = 10;
            this.radButton5.Text = "Выбрать";
            this.radButton5.ThemeName = "Office2013Light";
            this.radButton5.Visible = false;
            this.radButton5.Click += new System.EventHandler(this.radButton5_Click);
            // 
            // radButton2
            // 
            this.radButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton2.Location = new System.Drawing.Point(711, 304);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(90, 24);
            this.radButton2.TabIndex = 12;
            this.radButton2.Text = "Изменить";
            this.radButton2.ThemeName = "Office2013Light";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton3
            // 
            this.radButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton3.Location = new System.Drawing.Point(711, 334);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(90, 24);
            this.radButton3.TabIndex = 13;
            this.radButton3.Text = "Удалить";
            this.radButton3.ThemeName = "Office2013Light";
            this.radButton3.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // radButton1
            // 
            this.radButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton1.Location = new System.Drawing.Point(711, 274);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(90, 24);
            this.radButton1.TabIndex = 11;
            this.radButton1.Text = "Добавить";
            this.radButton1.ThemeName = "Office2013Light";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // StaffFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 429);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton3);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.radButton5);
            this.Controls.Add(this.staffGridView);
            this.Controls.Add(this.radButton4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(750, 400);
            this.Name = "StaffFrm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сотрудники";
            this.ThemeName = "Office2013Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StaffFrm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StaffFrm_FormClosed);
            this.Load += new System.EventHandler(this.StaffFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButton4;
        private Telerik.WinControls.UI.RadGridView staffGridView;
        private Telerik.WinControls.UI.RadButton radButton5;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton3;
        private Telerik.WinControls.UI.RadButton radButton1;
    }
}
