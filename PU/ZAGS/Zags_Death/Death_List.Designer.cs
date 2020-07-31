namespace PU.ZAGS.Zags_Death
{
    partial class Death_List
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn6 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn7 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.exportToXMLBtn = new Telerik.WinControls.UI.RadButton();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.editBtn = new Telerik.WinControls.UI.RadButton();
            this.delBtn = new Telerik.WinControls.UI.RadButton();
            this.addBtn = new Telerik.WinControls.UI.RadButton();
            this.DeathGridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.exportToXMLBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeathGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeathGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // exportToXMLBtn
            // 
            this.exportToXMLBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportToXMLBtn.Location = new System.Drawing.Point(12, 533);
            this.exportToXMLBtn.Name = "exportToXMLBtn";
            this.exportToXMLBtn.Size = new System.Drawing.Size(221, 24);
            this.exportToXMLBtn.TabIndex = 66;
            this.exportToXMLBtn.Text = "Запись в XML-файл";
            this.exportToXMLBtn.ThemeName = "Office2013Light";
            this.exportToXMLBtn.Click += new System.EventHandler(this.exportToXMLBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(775, 533);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(105, 24);
            this.closeBtn.TabIndex = 65;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // editBtn
            // 
            this.editBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editBtn.Location = new System.Drawing.Point(775, 465);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(105, 24);
            this.editBtn.TabIndex = 63;
            this.editBtn.Text = "Изменить";
            this.editBtn.ThemeName = "Office2013Light";
            this.editBtn.Click += new System.EventHandler(this.editBtn_Click);
            // 
            // delBtn
            // 
            this.delBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.delBtn.Location = new System.Drawing.Point(775, 495);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(105, 24);
            this.delBtn.TabIndex = 64;
            this.delBtn.Text = "Удалить";
            this.delBtn.ThemeName = "Office2013Light";
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addBtn.Location = new System.Drawing.Point(775, 435);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(105, 24);
            this.addBtn.TabIndex = 62;
            this.addBtn.Text = "Добавить";
            this.addBtn.ThemeName = "Office2013Light";
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // DeathGridView
            // 
            this.DeathGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DeathGridView.EnableCustomSorting = true;
            this.DeathGridView.Location = new System.Drawing.Point(12, 12);
            // 
            // 
            // 
            this.DeathGridView.MasterTemplate.AllowAddNewRow = false;
            this.DeathGridView.MasterTemplate.AllowDeleteRow = false;
            this.DeathGridView.MasterTemplate.AllowEditRow = false;
            this.DeathGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.HeaderText = "ID";
            gridViewTextBoxColumn1.IsVisible = false;
            gridViewTextBoxColumn1.Name = "ID";
            gridViewTextBoxColumn1.VisibleInColumnChooser = false;
            gridViewTextBoxColumn1.Width = 46;
            gridViewTextBoxColumn2.HeaderText = "ФИО";
            gridViewTextBoxColumn2.Name = "FIO";
            gridViewTextBoxColumn2.ReadOnly = true;
            gridViewTextBoxColumn2.Width = 307;
            gridViewTextBoxColumn3.HeaderText = "Пол";
            gridViewTextBoxColumn3.MaxLength = 100;
            gridViewTextBoxColumn3.Name = "Sex";
            gridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn3.Width = 51;
            gridViewTextBoxColumn4.HeaderText = "Дата рождения";
            gridViewTextBoxColumn4.Name = "DateBirth";
            gridViewTextBoxColumn4.ReadOnly = true;
            gridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn4.Width = 109;
            gridViewTextBoxColumn5.HeaderText = "Дата смерти";
            gridViewTextBoxColumn5.Name = "DateDeath";
            gridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn5.Width = 91;
            gridViewTextBoxColumn6.HeaderText = "Номер акта";
            gridViewTextBoxColumn6.Name = "Akt_Num";
            gridViewTextBoxColumn6.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            gridViewTextBoxColumn6.Width = 104;
            gridViewTextBoxColumn7.HeaderText = "Дата акта";
            gridViewTextBoxColumn7.MaxLength = 50;
            gridViewTextBoxColumn7.Name = "Akt_Date";
            gridViewTextBoxColumn7.ReadOnly = true;
            gridViewTextBoxColumn7.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn7.Width = 99;
            this.DeathGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6,
            gridViewTextBoxColumn7});
            this.DeathGridView.MasterTemplate.EnableCustomSorting = true;
            this.DeathGridView.MasterTemplate.EnableGrouping = false;
            this.DeathGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.DeathGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.DeathGridView.Name = "DeathGridView";
            this.DeathGridView.ShowRowErrors = false;
            this.DeathGridView.Size = new System.Drawing.Size(757, 507);
            this.DeathGridView.TabIndex = 61;
            this.DeathGridView.ThemeName = "Office2013Light";
            // 
            // Death_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 569);
            this.Controls.Add(this.exportToXMLBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.delBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.DeathGridView);
            this.Name = "Death_List";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сведения о государственной регистрации смерти";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.Death_List_Load);
            ((System.ComponentModel.ISupportInitialize)(this.exportToXMLBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeathGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeathGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton exportToXMLBtn;
        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadButton editBtn;
        private Telerik.WinControls.UI.RadButton delBtn;
        private Telerik.WinControls.UI.RadButton addBtn;
        private Telerik.WinControls.UI.RadGridView DeathGridView;
    }
}
