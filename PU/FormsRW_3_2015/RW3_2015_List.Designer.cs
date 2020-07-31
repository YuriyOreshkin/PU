namespace PU.FormsRW_3_2015
{
    partial class RW3_2015_List
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.insNamePanel = new Telerik.WinControls.UI.RadPanel();
            this.insurerBtn = new Telerik.WinControls.UI.RadButton();
            this.rw3GridView = new Telerik.WinControls.UI.RadGridView();
            this.editBtn = new Telerik.WinControls.UI.RadButton();
            this.delBtn = new Telerik.WinControls.UI.RadButton();
            this.addBtn = new Telerik.WinControls.UI.RadButton();
            this.exportToXMLBtn = new Telerik.WinControls.UI.RadButton();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.printBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.insNamePanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.insurerBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rw3GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rw3GridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exportToXMLBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // insNamePanel
            // 
            this.insNamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insNamePanel.BackColor = System.Drawing.Color.LemonChiffon;
            this.insNamePanel.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.insNamePanel.Location = new System.Drawing.Point(17, 2);
            this.insNamePanel.Name = "insNamePanel";
            this.insNamePanel.Size = new System.Drawing.Size(747, 29);
            this.insNamePanel.TabIndex = 30;
            this.insNamePanel.Text = "Страхователь не выбран";
            this.insNamePanel.ThemeName = "Office2013Light";
            // 
            // insurerBtn
            // 
            this.insurerBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.insurerBtn.Location = new System.Drawing.Point(771, 2);
            this.insurerBtn.Name = "insurerBtn";
            this.insurerBtn.Size = new System.Drawing.Size(105, 29);
            this.insurerBtn.TabIndex = 31;
            this.insurerBtn.Text = "Сменить";
            this.insurerBtn.ThemeName = "Office2013Light";
            this.insurerBtn.Click += new System.EventHandler(this.insurerBtn_Click);
            // 
            // rw3GridView
            // 
            this.rw3GridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rw3GridView.EnableCustomSorting = true;
            this.rw3GridView.Location = new System.Drawing.Point(17, 37);
            // 
            // 
            // 
            this.rw3GridView.MasterTemplate.AllowAddNewRow = false;
            this.rw3GridView.MasterTemplate.AllowDeleteRow = false;
            this.rw3GridView.MasterTemplate.AllowEditRow = false;
            this.rw3GridView.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.HeaderText = "ID";
            gridViewTextBoxColumn1.IsVisible = false;
            gridViewTextBoxColumn1.Name = "ID";
            gridViewTextBoxColumn1.VisibleInColumnChooser = false;
            gridViewTextBoxColumn1.Width = 46;
            gridViewTextBoxColumn2.HeaderText = "Календарный год";
            gridViewTextBoxColumn2.Name = "Year";
            gridViewTextBoxColumn2.ReadOnly = true;
            gridViewTextBoxColumn2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn2.Width = 112;
            gridViewTextBoxColumn3.HeaderText = "Отчетный период";
            gridViewTextBoxColumn3.Name = "Period";
            gridViewTextBoxColumn3.ReadOnly = true;
            gridViewTextBoxColumn3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn3.Width = 158;
            gridViewTextBoxColumn4.HeaderText = "№ корр.";
            gridViewTextBoxColumn4.MaxLength = 100;
            gridViewTextBoxColumn4.Name = "CorrNum";
            gridViewTextBoxColumn4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn4.Width = 128;
            gridViewTextBoxColumn5.HeaderText = "Тариф";
            gridViewTextBoxColumn5.Name = "CodeTar";
            gridViewTextBoxColumn5.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn5.Width = 67;
            gridViewTextBoxColumn6.HeaderText = "Сумма взносов по дополнительному тарифу";
            gridViewTextBoxColumn6.MaxLength = 50;
            gridViewTextBoxColumn6.Name = "Sum";
            gridViewTextBoxColumn6.ReadOnly = true;
            gridViewTextBoxColumn6.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gridViewTextBoxColumn6.Width = 283;
            this.rw3GridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5,
            gridViewTextBoxColumn6});
            this.rw3GridView.MasterTemplate.EnableCustomSorting = true;
            this.rw3GridView.MasterTemplate.EnableGrouping = false;
            this.rw3GridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.rw3GridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.rw3GridView.Name = "rw3GridView";
            this.rw3GridView.ShowRowErrors = false;
            this.rw3GridView.Size = new System.Drawing.Size(745, 482);
            this.rw3GridView.TabIndex = 32;
            this.rw3GridView.ThemeName = "Office2013Light";
            this.rw3GridView.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.rw3GridView_CellDoubleClick);
            this.rw3GridView.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.rw3GridView_ContextMenuOpening);
            // 
            // editBtn
            // 
            this.editBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editBtn.Location = new System.Drawing.Point(771, 465);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(105, 24);
            this.editBtn.TabIndex = 34;
            this.editBtn.Text = "Изменить";
            this.editBtn.ThemeName = "Office2013Light";
            this.editBtn.Click += new System.EventHandler(this.editBtn_Click);
            // 
            // delBtn
            // 
            this.delBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.delBtn.Location = new System.Drawing.Point(771, 495);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(105, 24);
            this.delBtn.TabIndex = 35;
            this.delBtn.Text = "Удалить";
            this.delBtn.ThemeName = "Office2013Light";
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addBtn.Location = new System.Drawing.Point(771, 435);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(105, 24);
            this.addBtn.TabIndex = 33;
            this.addBtn.Text = "Добавить";
            this.addBtn.ThemeName = "Office2013Light";
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // exportToXMLBtn
            // 
            this.exportToXMLBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportToXMLBtn.Location = new System.Drawing.Point(17, 533);
            this.exportToXMLBtn.Name = "exportToXMLBtn";
            this.exportToXMLBtn.Size = new System.Drawing.Size(221, 24);
            this.exportToXMLBtn.TabIndex = 59;
            this.exportToXMLBtn.Text = "Запись в XML-файл";
            this.exportToXMLBtn.ThemeName = "Office2013Light";
            this.exportToXMLBtn.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(771, 533);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(105, 24);
            this.closeBtn.TabIndex = 58;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // printBtn
            // 
            this.printBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.printBtn.Location = new System.Drawing.Point(771, 384);
            this.printBtn.Margin = new System.Windows.Forms.Padding(3, 3, 3, 24);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(105, 24);
            this.printBtn.TabIndex = 60;
            this.printBtn.Text = "Печать";
            this.printBtn.ThemeName = "Office2013Light";
            this.printBtn.Click += new System.EventHandler(this.printBtn_Click);
            // 
            // RW3_2015_List
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 569);
            this.Controls.Add(this.printBtn);
            this.Controls.Add(this.exportToXMLBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.delBtn);
            this.Controls.Add(this.addBtn);
            this.Controls.Add(this.rw3GridView);
            this.Controls.Add(this.insurerBtn);
            this.Controls.Add(this.insNamePanel);
            this.Name = "RW3_2015_List";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Расчет взносов на дополнительное социальное обеспечение - Форма РВ-3 (2015)";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.RSW2_2014_List_Load);
            this.Shown += new System.EventHandler(this.RSW2_2014_List_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.insNamePanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.insurerBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rw3GridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rw3GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exportToXMLBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel insNamePanel;
        private Telerik.WinControls.UI.RadButton insurerBtn;
        private Telerik.WinControls.UI.RadGridView rw3GridView;
        private Telerik.WinControls.UI.RadButton editBtn;
        private Telerik.WinControls.UI.RadButton delBtn;
        private Telerik.WinControls.UI.RadButton addBtn;
        private Telerik.WinControls.UI.RadButton exportToXMLBtn;
        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadButton printBtn;
    }
}
