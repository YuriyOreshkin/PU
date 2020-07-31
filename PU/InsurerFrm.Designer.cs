namespace PU
{
    partial class InsurerFrm
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
            this.editBtn = new Telerik.WinControls.UI.RadButton();
            this.delBtn = new Telerik.WinControls.UI.RadButton();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.addBtn = new Telerik.WinControls.UI.RadButton();
            this.insurerGrid = new Telerik.WinControls.UI.RadGridView();
            this.departmentBtn = new Telerik.WinControls.UI.RadButton();
            this.staffBtn = new Telerik.WinControls.UI.RadButton();
            this.selectBtn = new Telerik.WinControls.UI.RadButton();
            this.printBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.insurerGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.insurerGrid.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.departmentBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // editBtn
            // 
            this.editBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editBtn.Location = new System.Drawing.Point(803, 302);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(105, 24);
            this.editBtn.TabIndex = 1;
            this.editBtn.Text = "Изменить";
            this.editBtn.ThemeName = "Office2013Light";
            this.editBtn.Click += new System.EventHandler(this.editBtn_Click);
            // 
            // delBtn
            // 
            this.delBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.delBtn.Location = new System.Drawing.Point(803, 332);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(105, 24);
            this.delBtn.TabIndex = 2;
            this.delBtn.Text = "Удалить";
            this.delBtn.ThemeName = "Office2013Light";
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(803, 399);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(105, 24);
            this.closeBtn.TabIndex = 3;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addBtn.Location = new System.Drawing.Point(803, 272);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(105, 24);
            this.addBtn.TabIndex = 0;
            this.addBtn.Text = "Добавить";
            this.addBtn.ThemeName = "Office2013Light";
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // insurerGrid
            // 
            this.insurerGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insurerGrid.Location = new System.Drawing.Point(11, 12);
            // 
            // 
            // 
            this.insurerGrid.MasterTemplate.AllowAddNewRow = false;
            this.insurerGrid.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.insurerGrid.MasterTemplate.AllowColumnReorder = false;
            this.insurerGrid.MasterTemplate.AllowDeleteRow = false;
            this.insurerGrid.MasterTemplate.AllowDragToGroup = false;
            this.insurerGrid.MasterTemplate.EnableFiltering = true;
            this.insurerGrid.MasterTemplate.EnableGrouping = false;
            this.insurerGrid.MasterTemplate.ShowRowHeaderColumn = false;
            this.insurerGrid.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.insurerGrid.Name = "insurerGrid";
            this.insurerGrid.ShowRowErrors = false;
            this.insurerGrid.Size = new System.Drawing.Size(786, 411);
            this.insurerGrid.TabIndex = 4;
            this.insurerGrid.ThemeName = "Office2013Light";
            this.insurerGrid.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.insurerGrid_CellDoubleClick);
            this.insurerGrid.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.insurerGrid_ContextMenuOpening);
            this.insurerGrid.FilterChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.insurerGrid_FilterChanged);
            this.insurerGrid.SizeChanged += new System.EventHandler(this.insurerGrid_SizeChanged);
            this.insurerGrid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.insurerGrid_KeyPress);
            // 
            // departmentBtn
            // 
            this.departmentBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.departmentBtn.Location = new System.Drawing.Point(803, 68);
            this.departmentBtn.Name = "departmentBtn";
            this.departmentBtn.Size = new System.Drawing.Size(105, 24);
            this.departmentBtn.TabIndex = 5;
            this.departmentBtn.Text = "Отделы";
            this.departmentBtn.ThemeName = "Office2013Light";
            this.departmentBtn.Click += new System.EventHandler(this.departmentBtn_Click);
            // 
            // staffBtn
            // 
            this.staffBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.staffBtn.Location = new System.Drawing.Point(803, 120);
            this.staffBtn.Name = "staffBtn";
            this.staffBtn.Size = new System.Drawing.Size(105, 24);
            this.staffBtn.TabIndex = 6;
            this.staffBtn.Text = "Сотрудники";
            this.staffBtn.ThemeName = "Office2013Light";
            this.staffBtn.Click += new System.EventHandler(this.staffBtn_Click);
            // 
            // selectBtn
            // 
            this.selectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectBtn.Location = new System.Drawing.Point(803, 182);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(105, 24);
            this.selectBtn.TabIndex = 7;
            this.selectBtn.Text = "Выбрать";
            this.selectBtn.ThemeName = "Office2013Light";
            this.selectBtn.Visible = false;
            this.selectBtn.Click += new System.EventHandler(this.selectBtn_Click);
            // 
            // printBtn
            // 
            this.printBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.printBtn.Location = new System.Drawing.Point(803, 12);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(105, 24);
            this.printBtn.TabIndex = 8;
            this.printBtn.Text = "Печать";
            this.printBtn.ThemeName = "Office2013Light";
            this.printBtn.Click += new System.EventHandler(this.printBtn_Click);
            // 
            // InsurerFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 433);
            this.Controls.Add(this.printBtn);
            this.Controls.Add(this.selectBtn);
            this.Controls.Add(this.staffBtn);
            this.Controls.Add(this.departmentBtn);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.delBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.insurerGrid);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 350);
            this.Name = "InsurerFrm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Страхователи / Работодатели";
            this.ThemeName = "Office2013Light";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InsurerFrm_FormClosed);
            this.Load += new System.EventHandler(this.Insurer_Load);
            this.Shown += new System.EventHandler(this.InsurerFrm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.insurerGrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.insurerGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.departmentBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton editBtn;
        private Telerik.WinControls.UI.RadButton delBtn;
        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadButton addBtn;
        private Telerik.WinControls.UI.RadGridView insurerGrid;
        private Telerik.WinControls.UI.RadButton departmentBtn;
        private Telerik.WinControls.UI.RadButton staffBtn;
        private Telerik.WinControls.UI.RadButton selectBtn;
        private Telerik.WinControls.UI.RadButton printBtn;
    }
}
