namespace PU.FormsDSW3
{
    partial class DSW3_CreateXML
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem4 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem5 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem6 = new Telerik.WinControls.UI.RadListDataItem();
            this.label1 = new System.Windows.Forms.Label();
            this.dsw3Number = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sortingDDL = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel7 = new Telerik.WinControls.UI.RadLabel();
            this.splitChkBox = new Telerik.WinControls.UI.RadCheckBox();
            this.splitCntSpinEditor = new Telerik.WinControls.UI.RadSpinEditor();
            this.DateUnderwriteMaskedEditBox = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.numFrom = new Telerik.WinControls.UI.RadSpinEditor();
            this.radLabel8 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.DateUnderwrite = new Telerik.WinControls.UI.RadDateTimePicker();
            this.viewPacks = new Telerik.WinControls.UI.RadButton();
            this.startBtn = new Telerik.WinControls.UI.RadButton();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.sortingDDL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitChkBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCntSpinEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateUnderwriteMaskedEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateUnderwrite)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPacks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(441, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Реестр по доп.взносам: форма ДСВ-3";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dsw3Number
            // 
            this.dsw3Number.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dsw3Number.BackColor = System.Drawing.Color.Transparent;
            this.dsw3Number.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dsw3Number.ForeColor = System.Drawing.Color.Blue;
            this.dsw3Number.Location = new System.Drawing.Point(-2, 55);
            this.dsw3Number.Name = "dsw3Number";
            this.dsw3Number.Size = new System.Drawing.Size(443, 27);
            this.dsw3Number.TabIndex = 2;
            this.dsw3Number.Text = "Номер поручения";
            this.dsw3Number.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(-2, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(443, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "Платежное поручение";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sortingDDL
            // 
            this.sortingDDL.AutoCompleteDisplayMember = null;
            this.sortingDDL.AutoCompleteValueMember = null;
            this.sortingDDL.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem4.Selected = true;
            radListDataItem4.Tag = "0";
            radListDataItem4.Text = "Фамилия Имя Отчество";
            radListDataItem5.Tag = "1";
            radListDataItem5.Text = "Страховой номер";
            radListDataItem6.Tag = "2";
            radListDataItem6.Text = "Табельный номер";
            this.sortingDDL.Items.Add(radListDataItem4);
            this.sortingDDL.Items.Add(radListDataItem5);
            this.sortingDDL.Items.Add(radListDataItem6);
            this.sortingDDL.Location = new System.Drawing.Point(112, 100);
            this.sortingDDL.Name = "sortingDDL";
            this.sortingDDL.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.sortingDDL.Size = new System.Drawing.Size(203, 21);
            this.sortingDDL.TabIndex = 18;
            this.sortingDDL.Text = "Фамилия Имя Отчество";
            this.sortingDDL.ThemeName = "Office2013Light";
            // 
            // radLabel7
            // 
            this.radLabel7.Location = new System.Drawing.Point(12, 100);
            this.radLabel7.Name = "radLabel7";
            this.radLabel7.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radLabel7.Size = new System.Drawing.Size(74, 19);
            this.radLabel7.TabIndex = 17;
            this.radLabel7.Text = "Сортировка";
            this.radLabel7.ThemeName = "Office2013Light";
            // 
            // splitChkBox
            // 
            this.splitChkBox.Location = new System.Drawing.Point(12, 132);
            this.splitChkBox.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.splitChkBox.Name = "splitChkBox";
            this.splitChkBox.Size = new System.Drawing.Size(345, 18);
            this.splitChkBox.TabIndex = 19;
            this.splitChkBox.Text = "Установить ограничение на количество сотрудников в пачке";
            this.splitChkBox.ThemeName = "Office2013Light";
            this.splitChkBox.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.splitChkBox_ToggleStateChanged);
            // 
            // splitCntSpinEditor
            // 
            this.splitCntSpinEditor.Enabled = false;
            this.splitCntSpinEditor.Location = new System.Drawing.Point(362, 130);
            this.splitCntSpinEditor.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.splitCntSpinEditor.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.splitCntSpinEditor.Name = "splitCntSpinEditor";
            this.splitCntSpinEditor.Size = new System.Drawing.Size(63, 21);
            this.splitCntSpinEditor.TabIndex = 20;
            this.splitCntSpinEditor.TabStop = false;
            this.splitCntSpinEditor.TextAlignment = System.Windows.Forms.HorizontalAlignment.Center;
            this.splitCntSpinEditor.ThemeName = "Office2013Light";
            this.splitCntSpinEditor.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // DateUnderwriteMaskedEditBox
            // 
            this.DateUnderwriteMaskedEditBox.Location = new System.Drawing.Point(331, 213);
            this.DateUnderwriteMaskedEditBox.Mask = "00/00/0000";
            this.DateUnderwriteMaskedEditBox.MaskType = Telerik.WinControls.UI.MaskType.Standard;
            this.DateUnderwriteMaskedEditBox.Name = "DateUnderwriteMaskedEditBox";
            this.DateUnderwriteMaskedEditBox.NullText = "__.__.____";
            this.DateUnderwriteMaskedEditBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DateUnderwriteMaskedEditBox.Size = new System.Drawing.Size(73, 21);
            this.DateUnderwriteMaskedEditBox.TabIndex = 39;
            this.DateUnderwriteMaskedEditBox.TabStop = false;
            this.DateUnderwriteMaskedEditBox.Text = "__.__.____";
            this.DateUnderwriteMaskedEditBox.ThemeName = "Office2013Light";
            this.DateUnderwriteMaskedEditBox.Leave += new System.EventHandler(this.DateUnderwriteMaskedEditBox_Leave);
            // 
            // numFrom
            // 
            this.numFrom.Location = new System.Drawing.Point(331, 180);
            this.numFrom.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrom.Name = "numFrom";
            this.numFrom.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.numFrom.Size = new System.Drawing.Size(94, 21);
            this.numFrom.TabIndex = 38;
            this.numFrom.TabStop = false;
            this.numFrom.TextAlignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.numFrom.ThemeName = "Office2013Light";
            this.numFrom.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radLabel8
            // 
            this.radLabel8.Location = new System.Drawing.Point(52, 215);
            this.radLabel8.Name = "radLabel8";
            this.radLabel8.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radLabel8.Size = new System.Drawing.Size(275, 19);
            this.radLabel8.TabIndex = 37;
            this.radLabel8.Text = "Дата формирования пачек (описи) документов";
            this.radLabel8.ThemeName = "Office2013Light";
            // 
            // radLabel6
            // 
            this.radLabel6.Location = new System.Drawing.Point(161, 180);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radLabel6.Size = new System.Drawing.Size(161, 19);
            this.radLabel6.TabIndex = 36;
            this.radLabel6.Text = "Начать нумерацию пачек с";
            this.radLabel6.ThemeName = "Office2013Light";
            // 
            // DateUnderwrite
            // 
            this.DateUnderwrite.CustomFormat = "dd.MM.yyyy";
            this.DateUnderwrite.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DateUnderwrite.Location = new System.Drawing.Point(331, 213);
            this.DateUnderwrite.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DateUnderwrite.Name = "DateUnderwrite";
            this.DateUnderwrite.NullDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DateUnderwrite.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.DateUnderwrite.Size = new System.Drawing.Size(94, 21);
            this.DateUnderwrite.TabIndex = 40;
            this.DateUnderwrite.TabStop = false;
            this.DateUnderwrite.ThemeName = "Office2013Light";
            this.DateUnderwrite.Value = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DateUnderwrite.ValueChanged += new System.EventHandler(this.DateUnderwrite_ValueChanged);
            // 
            // viewPacks
            // 
            this.viewPacks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.viewPacks.Location = new System.Drawing.Point(12, 272);
            this.viewPacks.Name = "viewPacks";
            this.viewPacks.Size = new System.Drawing.Size(110, 24);
            this.viewPacks.TabIndex = 41;
            this.viewPacks.Text = "Просмотр пачек";
            this.viewPacks.ThemeName = "Office2013Light";
            this.viewPacks.Click += new System.EventHandler(this.viewPacks_Click);
            // 
            // startBtn
            // 
            this.startBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startBtn.Location = new System.Drawing.Point(199, 272);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(110, 24);
            this.startBtn.TabIndex = 42;
            this.startBtn.Text = "Начать";
            this.startBtn.ThemeName = "Office2013Light";
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(315, 272);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(110, 24);
            this.closeBtn.TabIndex = 43;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // DSW3_CreateXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 308);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.viewPacks);
            this.Controls.Add(this.DateUnderwriteMaskedEditBox);
            this.Controls.Add(this.numFrom);
            this.Controls.Add(this.radLabel8);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.DateUnderwrite);
            this.Controls.Add(this.splitCntSpinEditor);
            this.Controls.Add(this.splitChkBox);
            this.Controls.Add(this.sortingDDL);
            this.Controls.Add(this.radLabel7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dsw3Number);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DSW3_CreateXML";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Формирование пачек документов для ПФР";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.DSW3_CreateXML_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sortingDDL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitChkBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitCntSpinEditor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateUnderwriteMaskedEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DateUnderwrite)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewPacks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label dsw3Number;
        private System.Windows.Forms.Label label2;
        private Telerik.WinControls.UI.RadDropDownList sortingDDL;
        private Telerik.WinControls.UI.RadLabel radLabel7;
        private Telerik.WinControls.UI.RadCheckBox splitChkBox;
        private Telerik.WinControls.UI.RadSpinEditor splitCntSpinEditor;
        private Telerik.WinControls.UI.RadMaskedEditBox DateUnderwriteMaskedEditBox;
        private Telerik.WinControls.UI.RadSpinEditor numFrom;
        private Telerik.WinControls.UI.RadLabel radLabel8;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        public Telerik.WinControls.UI.RadDateTimePicker DateUnderwrite;
        private Telerik.WinControls.UI.RadButton viewPacks;
        private Telerik.WinControls.UI.RadButton startBtn;
        private Telerik.WinControls.UI.RadButton closeBtn;
    }
}
