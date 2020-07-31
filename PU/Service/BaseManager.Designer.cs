namespace PU
{
    partial class BaseManager
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
            this.DBList = new Telerik.WinControls.UI.RadListControl();
            this.dbPathPanel = new Telerik.WinControls.UI.RadPanel();
            this.dbPathLabel = new Telerik.WinControls.UI.RadLabel();
            this.selectBDBtn = new Telerik.WinControls.UI.RadButton();
            this.addBDBtn = new Telerik.WinControls.UI.RadButton();
            this.editBDBtn = new Telerik.WinControls.UI.RadButton();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.DelBDBtn = new Telerik.WinControls.UI.RadDropDownButton();
            this.delFromListBtn = new Telerik.WinControls.UI.RadMenuItem();
            this.delFromDiskBtn = new Telerik.WinControls.UI.RadMenuItem();
            this.delFromListBadItemsBtn = new Telerik.WinControls.UI.RadMenuItem();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.appVerLabel = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.dbVerLabel = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.updateDictBtn = new Telerik.WinControls.UI.RadButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.DBList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbPathPanel)).BeginInit();
            this.dbPathPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbPathLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBDBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBDBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBDBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelBDBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appVerLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbVerLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateDictBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // DBList
            // 
            this.DBList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DBList.EnableTheming = false;
            this.DBList.Location = new System.Drawing.Point(11, 12);
            this.DBList.Name = "DBList";
            this.DBList.Size = new System.Drawing.Size(473, 289);
            this.DBList.TabIndex = 0;
            this.DBList.ThemeName = "Office2013Light";
            this.DBList.SelectedIndexChanged += new Telerik.WinControls.UI.Data.PositionChangedEventHandler(this.DBList_SelectedIndexChanged);
            this.DBList.DoubleClick += new System.EventHandler(this.DBList_DoubleClick);
            // 
            // dbPathPanel
            // 
            this.dbPathPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dbPathPanel.Controls.Add(this.dbPathLabel);
            this.dbPathPanel.Location = new System.Drawing.Point(11, 334);
            this.dbPathPanel.Name = "dbPathPanel";
            this.dbPathPanel.Padding = new System.Windows.Forms.Padding(3);
            this.dbPathPanel.Size = new System.Drawing.Size(589, 42);
            this.dbPathPanel.TabIndex = 1;
            this.dbPathPanel.ThemeName = "Office2013Light";
            // 
            // dbPathLabel
            // 
            this.dbPathLabel.AutoScroll = true;
            this.dbPathLabel.AutoSize = false;
            this.dbPathLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbPathLabel.Location = new System.Drawing.Point(3, 3);
            this.dbPathLabel.Name = "dbPathLabel";
            this.dbPathLabel.Size = new System.Drawing.Size(583, 36);
            this.dbPathLabel.TabIndex = 0;
            this.dbPathLabel.Text = "Путь к базе данных...";
            this.dbPathLabel.ThemeName = "Office2013Light";
            // 
            // selectBDBtn
            // 
            this.selectBDBtn.Location = new System.Drawing.Point(490, 12);
            this.selectBDBtn.Name = "selectBDBtn";
            this.selectBDBtn.Size = new System.Drawing.Size(110, 24);
            this.selectBDBtn.TabIndex = 2;
            this.selectBDBtn.Text = "Выбрать";
            this.selectBDBtn.ThemeName = "Office2013Light";
            this.selectBDBtn.Click += new System.EventHandler(this.selectBDBtn_Click);
            // 
            // addBDBtn
            // 
            this.addBDBtn.Location = new System.Drawing.Point(490, 62);
            this.addBDBtn.Name = "addBDBtn";
            this.addBDBtn.Size = new System.Drawing.Size(110, 24);
            this.addBDBtn.TabIndex = 3;
            this.addBDBtn.Text = "Добавить";
            this.addBDBtn.ThemeName = "Office2013Light";
            this.addBDBtn.Click += new System.EventHandler(this.addBDBtn_Click);
            // 
            // editBDBtn
            // 
            this.editBDBtn.Location = new System.Drawing.Point(490, 92);
            this.editBDBtn.Name = "editBDBtn";
            this.editBDBtn.Size = new System.Drawing.Size(110, 24);
            this.editBDBtn.TabIndex = 4;
            this.editBDBtn.Text = "Редактировать";
            this.editBDBtn.ThemeName = "Office2013Light";
            this.editBDBtn.Click += new System.EventHandler(this.editBDBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(490, 277);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(110, 24);
            this.closeBtn.TabIndex = 6;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // DelBDBtn
            // 
            this.DelBDBtn.DisplayStyle = Telerik.WinControls.DisplayStyle.Text;
            this.DelBDBtn.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.delFromListBtn,
            this.delFromDiskBtn,
            this.delFromListBadItemsBtn});
            this.DelBDBtn.Location = new System.Drawing.Point(490, 122);
            this.DelBDBtn.Name = "DelBDBtn";
            this.DelBDBtn.Size = new System.Drawing.Size(110, 24);
            this.DelBDBtn.TabIndex = 7;
            this.DelBDBtn.Text = "Удалить";
            this.DelBDBtn.ThemeName = "Office2013Light";
            // 
            // delFromListBtn
            // 
            this.delFromListBtn.Name = "delFromListBtn";
            this.delFromListBtn.Tag = "0";
            this.delFromListBtn.Text = "Удалить из списка";
            this.delFromListBtn.Click += new System.EventHandler(this.delFromListBtn_Click);
            // 
            // delFromDiskBtn
            // 
            this.delFromDiskBtn.Name = "delFromDiskBtn";
            this.delFromDiskBtn.Tag = "1";
            this.delFromDiskBtn.Text = "Удалить БД с диска";
            this.delFromDiskBtn.Click += new System.EventHandler(this.delFromDiskBtn_Click);
            // 
            // delFromListBadItemsBtn
            // 
            this.delFromListBadItemsBtn.Name = "delFromListBadItemsBtn";
            this.delFromListBadItemsBtn.Text = "Удалить из списка отсутствующие БД";
            this.delFromListBadItemsBtn.Click += new System.EventHandler(this.delFromListBadItemsBtn_Click);
            // 
            // radPanel1
            // 
            this.radPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radPanel1.BackColor = System.Drawing.Color.GhostWhite;
            this.radPanel1.Controls.Add(this.appVerLabel);
            this.radPanel1.Controls.Add(this.radLabel3);
            this.radPanel1.Controls.Add(this.dbVerLabel);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.ForeColor = System.Drawing.Color.Black;
            this.radPanel1.Location = new System.Drawing.Point(11, 307);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(589, 25);
            this.radPanel1.TabIndex = 8;
            this.radPanel1.ThemeName = "Office2013Light";
            // 
            // appVerLabel
            // 
            this.appVerLabel.AutoSize = false;
            this.appVerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.appVerLabel.Location = new System.Drawing.Point(319, 3);
            this.appVerLabel.Name = "appVerLabel";
            this.appVerLabel.Size = new System.Drawing.Size(61, 18);
            this.appVerLabel.TabIndex = 3;
            this.appVerLabel.Text = "ver";
            this.appVerLabel.ThemeName = "Office2013Light";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(250, 3);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(71, 19);
            this.radLabel3.TabIndex = 2;
            this.radLabel3.Text = "Версия ПО:";
            this.radLabel3.ThemeName = "Office2013Light";
            // 
            // dbVerLabel
            // 
            this.dbVerLabel.AutoSize = false;
            this.dbVerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dbVerLabel.Location = new System.Drawing.Point(72, 3);
            this.dbVerLabel.Name = "dbVerLabel";
            this.dbVerLabel.Size = new System.Drawing.Size(68, 18);
            this.dbVerLabel.TabIndex = 1;
            this.dbVerLabel.Text = "ver";
            this.dbVerLabel.ThemeName = "Office2013Light";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(68, 19);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Версия БД:";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // updateDictBtn
            // 
            this.updateDictBtn.Location = new System.Drawing.Point(490, 212);
            this.updateDictBtn.Name = "updateDictBtn";
            this.updateDictBtn.Size = new System.Drawing.Size(110, 50);
            this.updateDictBtn.TabIndex = 9;
            this.updateDictBtn.Text = "Синхронизация справочников";
            this.updateDictBtn.TextWrap = true;
            this.updateDictBtn.ThemeName = "Office2013Light";
            this.updateDictBtn.Click += new System.EventHandler(this.updateDictBtn_Click);
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(490, 152);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(110, 24);
            this.radButton1.TabIndex = 10;
            this.radButton1.Text = "Архив";
            this.radButton1.ThemeName = "Office2013Light";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(490, 182);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(110, 24);
            this.radButton2.TabIndex = 11;
            this.radButton2.Text = "Обслуживание";
            this.radButton2.ThemeName = "Office2013Light";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // BaseManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(612, 390);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.updateDictBtn);
            this.Controls.Add(this.radPanel1);
            this.Controls.Add(this.DelBDBtn);
            this.Controls.Add(this.editBDBtn);
            this.Controls.Add(this.addBDBtn);
            this.Controls.Add(this.selectBDBtn);
            this.Controls.Add(this.dbPathPanel);
            this.Controls.Add(this.DBList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(620, 420);
            this.Name = "BaseManager";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Менеджер баз данных";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.BaseManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DBList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbPathPanel)).EndInit();
            this.dbPathPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dbPathLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBDBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBDBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBDBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DelBDBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.appVerLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dbVerLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updateDictBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadListControl DBList;
        private Telerik.WinControls.UI.RadPanel dbPathPanel;
        private Telerik.WinControls.UI.RadButton selectBDBtn;
        private Telerik.WinControls.UI.RadButton addBDBtn;
        private Telerik.WinControls.UI.RadButton editBDBtn;
        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadLabel dbPathLabel;
        private Telerik.WinControls.UI.RadDropDownButton DelBDBtn;
        private Telerik.WinControls.UI.RadMenuItem delFromListBtn;
        private Telerik.WinControls.UI.RadMenuItem delFromDiskBtn;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadLabel dbVerLabel;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel appVerLabel;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadButton updateDictBtn;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadMenuItem delFromListBadItemsBtn;
    }
}
