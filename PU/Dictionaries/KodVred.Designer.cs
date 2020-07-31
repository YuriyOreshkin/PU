namespace PU.FormsRSW2014
{
    partial class KodVred
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem3 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem4 = new Telerik.WinControls.UI.RadListDataItem();
            this.btnSelection = new Telerik.WinControls.UI.RadButton();
            this.btnClose = new Telerik.WinControls.UI.RadButton();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radDropDownList1 = new Telerik.WinControls.UI.RadDropDownList();
            this.radDropDownList2 = new Telerik.WinControls.UI.RadDropDownList();
            this.radTextBox1 = new Telerik.WinControls.UI.RadTextBox();
            this.radGridView2 = new Telerik.WinControls.UI.RadGridView();
            this.synchBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.synchBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelection
            // 
            this.btnSelection.Location = new System.Drawing.Point(609, 39);
            this.btnSelection.Name = "btnSelection";
            this.btnSelection.Size = new System.Drawing.Size(96, 24);
            this.btnSelection.TabIndex = 11;
            this.btnSelection.Text = "Выбрать";
            this.btnSelection.ThemeName = "Office2013Light";
            this.btnSelection.Visible = false;
            this.btnSelection.Click += new System.EventHandler(this.btnSelection_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(609, 427);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 24);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Закрыть";
            this.btnClose.ThemeName = "Office2013Light";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // radGridView1
            // 
            this.radGridView1.EnableCustomSorting = true;
            this.radGridView1.Location = new System.Drawing.Point(12, 39);
            // 
            // radGridView1
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView1.MasterTemplate.EnableCustomSorting = true;
            this.radGridView1.MasterTemplate.EnableGrouping = false;
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ShowRowErrors = false;
            this.radGridView1.Size = new System.Drawing.Size(591, 247);
            this.radGridView1.TabIndex = 10;
            this.radGridView1.ThemeName = "Office2013Light";
            this.radGridView1.CellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.radGridView1_CellFormatting);
            this.radGridView1.CurrentRowChanged += new Telerik.WinControls.UI.CurrentRowChangedEventHandler(this.radGridView1_CurrentRowChanged);
            this.radGridView1.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView1_CellDoubleClick);
            // 
            // radDropDownList1
            // 
            this.radDropDownList1.AllowShowFocusCues = false;
            this.radDropDownList1.AutoCompleteDisplayMember = null;
            this.radDropDownList1.AutoCompleteValueMember = null;
            this.radDropDownList1.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem3.Tag = "1";
            radListDataItem3.Text = "Список №1";
            radListDataItem3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radListDataItem3.TextWrap = true;
            radListDataItem4.Tag = "2";
            radListDataItem4.Text = "Список №2";
            radListDataItem4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            radListDataItem4.TextWrap = true;
            this.radDropDownList1.Items.Add(radListDataItem3);
            this.radDropDownList1.Items.Add(radListDataItem4);
            this.radDropDownList1.Location = new System.Drawing.Point(270, 12);
            this.radDropDownList1.Name = "radDropDownList1";
            this.radDropDownList1.Size = new System.Drawing.Size(127, 21);
            this.radDropDownList1.TabIndex = 8;
            this.radDropDownList1.ThemeName = "Office2013Light";
            // 
            // radDropDownList2
            // 
            this.radDropDownList2.AllowShowFocusCues = false;
            this.radDropDownList2.AutoCompleteDisplayMember = null;
            this.radDropDownList2.AutoCompleteValueMember = null;
            this.radDropDownList2.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            this.radDropDownList2.Location = new System.Drawing.Point(416, 12);
            this.radDropDownList2.Name = "radDropDownList2";
            this.radDropDownList2.Size = new System.Drawing.Size(187, 21);
            this.radDropDownList2.TabIndex = 12;
            this.radDropDownList2.ThemeName = "Office2013Light";
            this.radDropDownList2.Visible = false;
            // 
            // radTextBox1
            // 
            this.radTextBox1.Location = new System.Drawing.Point(12, 12);
            this.radTextBox1.Name = "radTextBox1";
            this.radTextBox1.NullText = "Поиск по коду профессии";
            this.radTextBox1.Size = new System.Drawing.Size(239, 21);
            this.radTextBox1.TabIndex = 13;
            this.radTextBox1.ThemeName = "Office2013Light";
            // 
            // radGridView2
            // 
            this.radGridView2.Location = new System.Drawing.Point(12, 292);
            // 
            // radGridView2
            // 
            this.radGridView2.MasterTemplate.AllowAddNewRow = false;
            this.radGridView2.MasterTemplate.AllowDeleteRow = false;
            this.radGridView2.MasterTemplate.AllowEditRow = false;
            this.radGridView2.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView2.MasterTemplate.EnableGrouping = false;
            this.radGridView2.MasterTemplate.EnableSorting = false;
            this.radGridView2.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView2.Name = "radGridView2";
            this.radGridView2.ShowRowErrors = false;
            this.radGridView2.Size = new System.Drawing.Size(591, 159);
            this.radGridView2.TabIndex = 14;
            this.radGridView2.ThemeName = "Office2013Light";
            // 
            // synchBtn
            // 
            this.synchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.synchBtn.Location = new System.Drawing.Point(609, 380);
            this.synchBtn.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.synchBtn.Name = "synchBtn";
            this.synchBtn.Size = new System.Drawing.Size(96, 24);
            this.synchBtn.TabIndex = 15;
            this.synchBtn.Text = "Синхронизация";
            this.synchBtn.ThemeName = "Office2013Light";
            this.synchBtn.Click += new System.EventHandler(this.synchBtn_Click);
            // 
            // KodVred
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 463);
            this.Controls.Add(this.synchBtn);
            this.Controls.Add(this.radGridView2);
            this.Controls.Add(this.radTextBox1);
            this.Controls.Add(this.radDropDownList2);
            this.Controls.Add(this.btnSelection);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.radGridView1);
            this.Controls.Add(this.radDropDownList1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KodVred";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Списки вредных профессий";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.KodVred_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDropDownList2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.synchBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton btnClose;
        public Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList1;
        private Telerik.WinControls.UI.RadDropDownList radDropDownList2;
        private Telerik.WinControls.UI.RadTextBox radTextBox1;
        public Telerik.WinControls.UI.RadGridView radGridView2;
        public Telerik.WinControls.UI.RadButton btnSelection;
        private Telerik.WinControls.UI.RadButton synchBtn;
    }
}
