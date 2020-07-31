namespace PU.FormsSZV_TD
{
    partial class SZV_TD_List
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn2 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition4 = new Telerik.WinControls.UI.TableViewDefinition();
            this.insurerBtn = new Telerik.WinControls.UI.RadButton();
            this.insNamePanel = new Telerik.WinControls.UI.RadPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.radWaitingBar1 = new Telerik.WinControls.UI.RadWaitingBar();
            this.szvTDGridView = new Telerik.WinControls.UI.RadGridView();
            this.radDropDownButton2 = new Telerik.WinControls.UI.RadDropDownButton();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.printBtn = new Telerik.WinControls.UI.RadButton();
            this.delBtn = new Telerik.WinControls.UI.RadButton();
            this.addBtn = new Telerik.WinControls.UI.RadButton();
            this.editBtn = new Telerik.WinControls.UI.RadButton();
            this.radDropDownButton1 = new Telerik.WinControls.UI.RadDropDownButton();
            this.copyStaffItemsMenuItem = new Telerik.WinControls.UI.RadMenuItem();
            this.printStaffBtn = new Telerik.WinControls.UI.RadButton();
            this.staffEditBtn = new Telerik.WinControls.UI.RadButton();
            this.staffGridView = new Telerik.WinControls.UI.RadGridView();
            this.staffAddBtn = new Telerik.WinControls.UI.RadButton();
            this.staffDelBtn = new Telerik.WinControls.UI.RadButton();
            this.exportToXMLBtn = new Telerik.WinControls.UI.RadButton();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.insurerBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.insNamePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.szvTDGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.szvTDGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printStaffBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffEditBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffAddBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffDelBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exportToXMLBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // insurerBtn
            // 
            this.insurerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.insurerBtn.Location = new System.Drawing.Point(836, 2);
            this.insurerBtn.Name = "insurerBtn";
            this.insurerBtn.Size = new System.Drawing.Size(105, 29);
            this.insurerBtn.TabIndex = 64;
            this.insurerBtn.Text = "Сменить";
            this.insurerBtn.ThemeName = "Office2013Light";
            this.insurerBtn.Click += new System.EventHandler(this.insurerBtn_Click);
            // 
            // insNamePanel
            // 
            this.insNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insNamePanel.BackColor = System.Drawing.Color.LemonChiffon;
            this.insNamePanel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.insNamePanel.Location = new System.Drawing.Point(17, 2);
            this.insNamePanel.Name = "insNamePanel";
            this.insNamePanel.Size = new System.Drawing.Size(810, 29);
            this.insNamePanel.TabIndex = 63;
            this.insNamePanel.Text = "Страхователь не выбран";
            this.insNamePanel.ThemeName = "Office2013Light";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(12, 34);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.radWaitingBar1);
            this.splitContainer1.Panel1.Controls.Add(this.szvTDGridView);
            this.splitContainer1.Panel1.Controls.Add(this.radDropDownButton2);
            this.splitContainer1.Panel1.Controls.Add(this.printBtn);
            this.splitContainer1.Panel1.Controls.Add(this.delBtn);
            this.splitContainer1.Panel1.Controls.Add(this.addBtn);
            this.splitContainer1.Panel1.Controls.Add(this.editBtn);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.radDropDownButton1);
            this.splitContainer1.Panel2.Controls.Add(this.printStaffBtn);
            this.splitContainer1.Panel2.Controls.Add(this.staffEditBtn);
            this.splitContainer1.Panel2.Controls.Add(this.staffGridView);
            this.splitContainer1.Panel2.Controls.Add(this.staffAddBtn);
            this.splitContainer1.Panel2.Controls.Add(this.staffDelBtn);
            this.splitContainer1.Size = new System.Drawing.Size(934, 595);
            this.splitContainer1.SplitterDistance = 278;
            this.splitContainer1.TabIndex = 74;
            // 
            // radWaitingBar1
            // 
            this.radWaitingBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radWaitingBar1.Location = new System.Drawing.Point(824, 166);
            this.radWaitingBar1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 16);
            this.radWaitingBar1.Name = "radWaitingBar1";
            this.radWaitingBar1.Size = new System.Drawing.Size(105, 25);
            this.radWaitingBar1.TabIndex = 73;
            this.radWaitingBar1.Text = "radWaitingBar1";
            this.radWaitingBar1.ThemeName = "Office2013Light";
            this.radWaitingBar1.Visible = false;
            this.radWaitingBar1.WaitingStyle = Telerik.WinControls.Enumerations.WaitingBarStyles.Throbber;
            // 
            // szvTDGridView
            // 
            this.szvTDGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.szvTDGridView.Location = new System.Drawing.Point(5, 1);
            this.szvTDGridView.Margin = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.szvTDGridView.MasterTemplate.AllowAddNewRow = false;
            this.szvTDGridView.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.szvTDGridView.MasterTemplate.AllowColumnReorder = false;
            this.szvTDGridView.MasterTemplate.AllowDeleteRow = false;
            this.szvTDGridView.MasterTemplate.AllowEditRow = false;
            this.szvTDGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.szvTDGridView.MasterTemplate.EnableFiltering = true;
            this.szvTDGridView.MasterTemplate.EnableGrouping = false;
            this.szvTDGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.szvTDGridView.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.szvTDGridView.Name = "szvTDGridView";
            this.szvTDGridView.ShowRowErrors = false;
            this.szvTDGridView.Size = new System.Drawing.Size(810, 275);
            this.szvTDGridView.TabIndex = 39;
            this.szvTDGridView.ThemeName = "Office2013Light";
            this.szvTDGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.szvTDGridView_CellDoubleClick);
            // 
            // radDropDownButton2
            // 
            this.radDropDownButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radDropDownButton2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem1});
            this.radDropDownButton2.Location = new System.Drawing.Point(824, 111);
            this.radDropDownButton2.Name = "radDropDownButton2";
            this.radDropDownButton2.Size = new System.Drawing.Size(105, 24);
            this.radDropDownButton2.TabIndex = 70;
            this.radDropDownButton2.Text = "Функции";
            this.radDropDownButton2.ThemeName = "Office2013Light";
            this.radDropDownButton2.Visible = false;
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "Копировать форму";
            this.radMenuItem1.UseCompatibleTextRendering = false;
            // 
            // printBtn
            // 
            this.printBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.printBtn.Location = new System.Drawing.Point(824, 141);
            this.printBtn.Margin = new System.Windows.Forms.Padding(3, 3, 3, 24);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(105, 24);
            this.printBtn.TabIndex = 69;
            this.printBtn.Text = "Печать";
            this.printBtn.ThemeName = "Office2013Light";
            this.printBtn.Click += new System.EventHandler(this.printBtn_Click);
            // 
            // delBtn
            // 
            this.delBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.delBtn.Location = new System.Drawing.Point(824, 252);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(105, 24);
            this.delBtn.TabIndex = 66;
            this.delBtn.Text = "Удалить";
            this.delBtn.ThemeName = "Office2013Light";
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addBtn.Location = new System.Drawing.Point(824, 192);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(105, 24);
            this.addBtn.TabIndex = 64;
            this.addBtn.Text = "Добавить";
            this.addBtn.ThemeName = "Office2013Light";
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // editBtn
            // 
            this.editBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editBtn.Location = new System.Drawing.Point(824, 222);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(105, 24);
            this.editBtn.TabIndex = 65;
            this.editBtn.Text = "Изменить";
            this.editBtn.ThemeName = "Office2013Light";
            this.editBtn.Click += new System.EventHandler(this.editBtn_Click);
            // 
            // radDropDownButton1
            // 
            this.radDropDownButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radDropDownButton1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.copyStaffItemsMenuItem});
            this.radDropDownButton1.Location = new System.Drawing.Point(824, 141);
            this.radDropDownButton1.Name = "radDropDownButton1";
            this.radDropDownButton1.Size = new System.Drawing.Size(105, 24);
            this.radDropDownButton1.TabIndex = 71;
            this.radDropDownButton1.Text = "Функции";
            this.radDropDownButton1.ThemeName = "Office2013Light";
            // 
            // copyStaffItemsMenuItem
            // 
            this.copyStaffItemsMenuItem.Name = "copyStaffItemsMenuItem";
            this.copyStaffItemsMenuItem.Text = "Копировать записи";
            this.copyStaffItemsMenuItem.UseCompatibleTextRendering = false;
            this.copyStaffItemsMenuItem.Click += new System.EventHandler(this.copyStaffItemsMenuItem_Click);
            // 
            // printStaffBtn
            // 
            this.printStaffBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.printStaffBtn.Location = new System.Drawing.Point(824, 171);
            this.printStaffBtn.Margin = new System.Windows.Forms.Padding(3, 3, 3, 24);
            this.printStaffBtn.Name = "printStaffBtn";
            this.printStaffBtn.Size = new System.Drawing.Size(105, 24);
            this.printStaffBtn.TabIndex = 70;
            this.printStaffBtn.Text = "Печать";
            this.printStaffBtn.ThemeName = "Office2013Light";
            this.printStaffBtn.Click += new System.EventHandler(this.printStaffBtn_Click);
            // 
            // staffEditBtn
            // 
            this.staffEditBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.staffEditBtn.Location = new System.Drawing.Point(824, 252);
            this.staffEditBtn.Name = "staffEditBtn";
            this.staffEditBtn.Size = new System.Drawing.Size(105, 24);
            this.staffEditBtn.TabIndex = 34;
            this.staffEditBtn.Text = "Изменить";
            this.staffEditBtn.ThemeName = "Office2013Light";
            this.staffEditBtn.Click += new System.EventHandler(this.staffEditBtn_Click);
            // 
            // staffGridView
            // 
            this.staffGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staffGridView.Location = new System.Drawing.Point(5, 2);
            this.staffGridView.Margin = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.staffGridView.MasterTemplate.AllowAddNewRow = false;
            this.staffGridView.MasterTemplate.AllowDeleteRow = false;
            gridViewCheckBoxColumn2.Checked = Telerik.WinControls.Enumerations.ToggleState.Indeterminate;
            gridViewCheckBoxColumn2.EnableHeaderCheckBox = true;
            gridViewCheckBoxColumn2.HeaderText = "";
            gridViewCheckBoxColumn2.MaxWidth = 24;
            gridViewCheckBoxColumn2.MinWidth = 24;
            gridViewCheckBoxColumn2.Name = "CheckBox";
            gridViewCheckBoxColumn2.StretchVertically = false;
            gridViewCheckBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewCheckBoxColumn2.Width = 24;
            this.staffGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewCheckBoxColumn2});
            this.staffGridView.MasterTemplate.EnableFiltering = true;
            this.staffGridView.MasterTemplate.EnableGrouping = false;
            this.staffGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.staffGridView.MasterTemplate.ViewDefinition = tableViewDefinition4;
            this.staffGridView.Name = "staffGridView";
            this.staffGridView.ShowRowErrors = false;
            this.staffGridView.Size = new System.Drawing.Size(810, 304);
            this.staffGridView.TabIndex = 11;
            this.staffGridView.ThemeName = "Office2013Light";
            this.staffGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.staffGridView_CellDoubleClick);
            // 
            // staffAddBtn
            // 
            this.staffAddBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.staffAddBtn.Location = new System.Drawing.Point(824, 222);
            this.staffAddBtn.Name = "staffAddBtn";
            this.staffAddBtn.Size = new System.Drawing.Size(105, 24);
            this.staffAddBtn.TabIndex = 14;
            this.staffAddBtn.Text = "Добавить";
            this.staffAddBtn.ThemeName = "Office2013Light";
            this.staffAddBtn.Click += new System.EventHandler(this.staffAddBtn_Click);
            // 
            // staffDelBtn
            // 
            this.staffDelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.staffDelBtn.Location = new System.Drawing.Point(824, 282);
            this.staffDelBtn.Name = "staffDelBtn";
            this.staffDelBtn.Size = new System.Drawing.Size(105, 24);
            this.staffDelBtn.TabIndex = 16;
            this.staffDelBtn.Text = "Удалить";
            this.staffDelBtn.ThemeName = "Office2013Light";
            this.staffDelBtn.Click += new System.EventHandler(this.staffDelBtn_Click);
            // 
            // exportToXMLBtn
            // 
            this.exportToXMLBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportToXMLBtn.Location = new System.Drawing.Point(19, 637);
            this.exportToXMLBtn.Name = "exportToXMLBtn";
            this.exportToXMLBtn.Size = new System.Drawing.Size(221, 24);
            this.exportToXMLBtn.TabIndex = 73;
            this.exportToXMLBtn.Text = "Запись в XML-файл";
            this.exportToXMLBtn.ThemeName = "Office2013Light";
            this.exportToXMLBtn.Click += new System.EventHandler(this.exportToXMLBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(836, 638);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(105, 24);
            this.closeBtn.TabIndex = 72;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // SZV_TD_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(962, 674);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.exportToXMLBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.insurerBtn);
            this.Controls.Add(this.insNamePanel);
            this.Name = "SZV_TD_List";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сведения о трудовой деятельности работников - Форма СЗВ-ТД";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.SZV_TD_List_Load);
            ((System.ComponentModel.ISupportInitialize)(this.insurerBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.insNamePanel)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radWaitingBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.szvTDGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.szvTDGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printStaffBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffEditBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffAddBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffDelBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exportToXMLBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton insurerBtn;
        private Telerik.WinControls.UI.RadPanel insNamePanel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        internal Telerik.WinControls.UI.RadGridView szvTDGridView;
        private Telerik.WinControls.UI.RadDropDownButton radDropDownButton2;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        private Telerik.WinControls.UI.RadButton printBtn;
        private Telerik.WinControls.UI.RadButton delBtn;
        private Telerik.WinControls.UI.RadButton addBtn;
        private Telerik.WinControls.UI.RadButton editBtn;
        private Telerik.WinControls.UI.RadButton staffEditBtn;
        private Telerik.WinControls.UI.RadGridView staffGridView;
        private Telerik.WinControls.UI.RadButton staffAddBtn;
        private Telerik.WinControls.UI.RadButton staffDelBtn;
        private Telerik.WinControls.UI.RadButton exportToXMLBtn;
        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadWaitingBar radWaitingBar1;
        private Telerik.WinControls.UI.RadButton printStaffBtn;
        private Telerik.WinControls.UI.RadDropDownButton radDropDownButton1;
        private Telerik.WinControls.UI.RadMenuItem copyStaffItemsMenuItem;
    }
}
