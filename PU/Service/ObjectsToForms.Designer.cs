namespace PU.Service
{
    partial class ObjectsToForms
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.objectCatDDL = new Telerik.WinControls.UI.RadDropDownList();
            this.objectsGridView = new Telerik.WinControls.UI.RadGridView();
            this.refFormsGridView = new Telerik.WinControls.UI.RadGridView();
            this.toRightBtn = new Telerik.WinControls.UI.RadButton();
            this.toLeftBtn = new Telerik.WinControls.UI.RadButton();
            this.allFormsGridView = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectCatDDL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectsGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refFormsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refFormsGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toRightBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toLeftBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allFormsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.allFormsGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(680, 351);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(110, 24);
            this.closeBtn.TabIndex = 1;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // objectCatDDL
            // 
            this.objectCatDDL.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.objectCatDDL.Location = new System.Drawing.Point(12, 12);
            this.objectCatDDL.Name = "objectCatDDL";
            this.objectCatDDL.Size = new System.Drawing.Size(195, 21);
            this.objectCatDDL.TabIndex = 2;
            this.objectCatDDL.ThemeName = "Office2013Light";
            // 
            // objectsGridView
            // 
            this.objectsGridView.Location = new System.Drawing.Point(12, 39);
            // 
            // 
            // 
            this.objectsGridView.MasterTemplate.AllowAddNewRow = false;
            this.objectsGridView.MasterTemplate.AllowDeleteRow = false;
            this.objectsGridView.MasterTemplate.AllowEditRow = false;
            this.objectsGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.objectsGridView.MasterTemplate.EnableGrouping = false;
            this.objectsGridView.MasterTemplate.EnableSorting = false;
            this.objectsGridView.MasterTemplate.ShowColumnHeaders = false;
            this.objectsGridView.MasterTemplate.ShowFilteringRow = false;
            this.objectsGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.objectsGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.objectsGridView.Name = "objectsGridView";
            this.objectsGridView.ShowGroupPanel = false;
            this.objectsGridView.Size = new System.Drawing.Size(195, 279);
            this.objectsGridView.TabIndex = 3;
            this.objectsGridView.ThemeName = "Office2013Light";
            this.objectsGridView.CurrentRowChanged += new Telerik.WinControls.UI.CurrentRowChangedEventHandler(this.objectsGridView_CurrentRowChanged);
            // 
            // refFormsGridView
            // 
            this.refFormsGridView.Location = new System.Drawing.Point(230, 39);
            this.refFormsGridView.Margin = new System.Windows.Forms.Padding(20, 3, 3, 3);
            // 
            // 
            // 
            this.refFormsGridView.MasterTemplate.AllowAddNewRow = false;
            this.refFormsGridView.MasterTemplate.AllowDeleteRow = false;
            this.refFormsGridView.MasterTemplate.AllowEditRow = false;
            this.refFormsGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.refFormsGridView.MasterTemplate.EnableGrouping = false;
            this.refFormsGridView.MasterTemplate.EnableSorting = false;
            this.refFormsGridView.MasterTemplate.ShowColumnHeaders = false;
            this.refFormsGridView.MasterTemplate.ShowFilteringRow = false;
            this.refFormsGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.refFormsGridView.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.refFormsGridView.Name = "refFormsGridView";
            this.refFormsGridView.ShowGroupPanel = false;
            this.refFormsGridView.Size = new System.Drawing.Size(195, 279);
            this.refFormsGridView.TabIndex = 4;
            this.refFormsGridView.ThemeName = "Office2013Light";
            // 
            // toRightBtn
            // 
            this.toRightBtn.Location = new System.Drawing.Point(431, 178);
            this.toRightBtn.Name = "toRightBtn";
            this.toRightBtn.Size = new System.Drawing.Size(66, 38);
            this.toRightBtn.TabIndex = 5;
            this.toRightBtn.Text = "->";
            this.toRightBtn.ThemeName = "Office2013Light";
            this.toRightBtn.Click += new System.EventHandler(this.toRightBtn_Click);
            // 
            // toLeftBtn
            // 
            this.toLeftBtn.Location = new System.Drawing.Point(431, 134);
            this.toLeftBtn.Name = "toLeftBtn";
            this.toLeftBtn.Size = new System.Drawing.Size(66, 38);
            this.toLeftBtn.TabIndex = 6;
            this.toLeftBtn.Text = "<-";
            this.toLeftBtn.ThemeName = "Office2013Light";
            this.toLeftBtn.Click += new System.EventHandler(this.toLeftBtn_Click);
            // 
            // allFormsGridView
            // 
            this.allFormsGridView.Location = new System.Drawing.Point(503, 39);
            // 
            // 
            // 
            this.allFormsGridView.MasterTemplate.AllowAddNewRow = false;
            this.allFormsGridView.MasterTemplate.AllowDeleteRow = false;
            this.allFormsGridView.MasterTemplate.AllowEditRow = false;
            this.allFormsGridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.HeaderText = "Name";
            gridViewTextBoxColumn1.Name = "Name";
            gridViewTextBoxColumn1.Width = 286;
            this.allFormsGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1});
            this.allFormsGridView.MasterTemplate.EnableFiltering = true;
            this.allFormsGridView.MasterTemplate.EnableGrouping = false;
            this.allFormsGridView.MasterTemplate.EnableSorting = false;
            this.allFormsGridView.MasterTemplate.ShowColumnHeaders = false;
            this.allFormsGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.allFormsGridView.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.allFormsGridView.Name = "allFormsGridView";
            this.allFormsGridView.ShowGroupPanel = false;
            this.allFormsGridView.Size = new System.Drawing.Size(287, 279);
            this.allFormsGridView.TabIndex = 7;
            this.allFormsGridView.ThemeName = "Office2013Light";
            this.allFormsGridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.allFormsGridView_CellDoubleClick);
            // 
            // ObjectsToForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 387);
            this.Controls.Add(this.allFormsGridView);
            this.Controls.Add(this.toLeftBtn);
            this.Controls.Add(this.toRightBtn);
            this.Controls.Add(this.refFormsGridView);
            this.Controls.Add(this.objectsGridView);
            this.Controls.Add(this.objectCatDDL);
            this.Controls.Add(this.closeBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ObjectsToForms";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Привязка форм в объектам доступа";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.ObjectsToForms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectCatDDL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectsGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refFormsGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refFormsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toRightBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toLeftBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allFormsGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.allFormsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadDropDownList objectCatDDL;
        private Telerik.WinControls.UI.RadGridView objectsGridView;
        private Telerik.WinControls.UI.RadGridView refFormsGridView;
        private Telerik.WinControls.UI.RadButton toRightBtn;
        private Telerik.WinControls.UI.RadButton toLeftBtn;
        private Telerik.WinControls.UI.RadGridView allFormsGridView;
    }
}
