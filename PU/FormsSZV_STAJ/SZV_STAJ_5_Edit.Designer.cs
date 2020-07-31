namespace PU.FormsSZV_STAJ
{
    partial class SZV_STAJ_5_Edit
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
            this.radLabel10 = new Telerik.WinControls.UI.RadLabel();
            this.DNPO_DateToMaskedEditBox = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.DNPO_DateTo = new Telerik.WinControls.UI.RadDateTimePicker();
            this.radLabel9 = new Telerik.WinControls.UI.RadLabel();
            this.DNPO_DateFromMaskedEditBox = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.DNPO_DateFrom = new Telerik.WinControls.UI.RadDateTimePicker();
            this.DNPO_1_Fee = new Telerik.WinControls.UI.RadCheckBox();
            this.radLabel8 = new Telerik.WinControls.UI.RadLabel();
            this.SaveBtn = new Telerik.WinControls.UI.RadButton();
            this.CloseBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_DateToMaskedEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_DateTo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_DateFromMaskedEditBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_DateFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_1_Fee)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SaveBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel10
            // 
            this.radLabel10.Location = new System.Drawing.Point(420, 14);
            this.radLabel10.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.radLabel10.Name = "radLabel10";
            this.radLabel10.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.radLabel10.Size = new System.Drawing.Size(71, 19);
            this.radLabel10.TabIndex = 52;
            this.radLabel10.Text = ", уплачены";
            this.radLabel10.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabel10.ThemeName = "Office2013Light";
            // 
            // DNPO_DateToMaskedEditBox
            // 
            this.DNPO_DateToMaskedEditBox.Location = new System.Drawing.Point(328, 13);
            this.DNPO_DateToMaskedEditBox.Mask = "00/00/0000";
            this.DNPO_DateToMaskedEditBox.MaskType = Telerik.WinControls.UI.MaskType.Standard;
            this.DNPO_DateToMaskedEditBox.Name = "DNPO_DateToMaskedEditBox";
            this.DNPO_DateToMaskedEditBox.NullText = "__.__.____";
            this.DNPO_DateToMaskedEditBox.Size = new System.Drawing.Size(69, 21);
            this.DNPO_DateToMaskedEditBox.TabIndex = 50;
            this.DNPO_DateToMaskedEditBox.TabStop = false;
            this.DNPO_DateToMaskedEditBox.Text = "__.__.____";
            this.DNPO_DateToMaskedEditBox.ThemeName = "Office2013Light";
            this.DNPO_DateToMaskedEditBox.Leave += new System.EventHandler(this.DNPO_1_DateToMaskedEditBox_Leave);
            // 
            // DNPO_DateTo
            // 
            this.DNPO_DateTo.CustomFormat = "dd.MM.yyyy";
            this.DNPO_DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DNPO_DateTo.Location = new System.Drawing.Point(328, 13);
            this.DNPO_DateTo.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.DNPO_DateTo.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DNPO_DateTo.Name = "DNPO_DateTo";
            this.DNPO_DateTo.NullDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DNPO_DateTo.Size = new System.Drawing.Size(89, 21);
            this.DNPO_DateTo.TabIndex = 51;
            this.DNPO_DateTo.TabStop = false;
            this.DNPO_DateTo.ThemeName = "Office2013Light";
            this.DNPO_DateTo.Value = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DNPO_DateTo.ValueChanged += new System.EventHandler(this.DNPO_1_DateTo_ValueChanged);
            // 
            // radLabel9
            // 
            this.radLabel9.Location = new System.Drawing.Point(301, 12);
            this.radLabel9.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.radLabel9.Name = "radLabel9";
            this.radLabel9.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.radLabel9.Size = new System.Drawing.Size(24, 19);
            this.radLabel9.TabIndex = 49;
            this.radLabel9.Text = "по";
            this.radLabel9.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabel9.ThemeName = "Office2013Light";
            // 
            // DNPO_DateFromMaskedEditBox
            // 
            this.DNPO_DateFromMaskedEditBox.Location = new System.Drawing.Point(209, 13);
            this.DNPO_DateFromMaskedEditBox.Mask = "00/00/0000";
            this.DNPO_DateFromMaskedEditBox.MaskType = Telerik.WinControls.UI.MaskType.Standard;
            this.DNPO_DateFromMaskedEditBox.Name = "DNPO_DateFromMaskedEditBox";
            this.DNPO_DateFromMaskedEditBox.NullText = "__.__.____";
            this.DNPO_DateFromMaskedEditBox.Size = new System.Drawing.Size(69, 21);
            this.DNPO_DateFromMaskedEditBox.TabIndex = 47;
            this.DNPO_DateFromMaskedEditBox.TabStop = false;
            this.DNPO_DateFromMaskedEditBox.Text = "__.__.____";
            this.DNPO_DateFromMaskedEditBox.ThemeName = "Office2013Light";
            this.DNPO_DateFromMaskedEditBox.Leave += new System.EventHandler(this.DNPO_1_DateFromMaskedEditBox_Leave);
            // 
            // DNPO_DateFrom
            // 
            this.DNPO_DateFrom.CustomFormat = "dd.MM.yyyy";
            this.DNPO_DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DNPO_DateFrom.Location = new System.Drawing.Point(209, 13);
            this.DNPO_DateFrom.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.DNPO_DateFrom.MinDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DNPO_DateFrom.Name = "DNPO_DateFrom";
            this.DNPO_DateFrom.NullDate = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DNPO_DateFrom.Size = new System.Drawing.Size(89, 21);
            this.DNPO_DateFrom.TabIndex = 48;
            this.DNPO_DateFrom.TabStop = false;
            this.DNPO_DateFrom.ThemeName = "Office2013Light";
            this.DNPO_DateFrom.Value = new System.DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DNPO_DateFrom.ValueChanged += new System.EventHandler(this.DNPO_1_DateFrom_ValueChanged);
            // 
            // DNPO_1_Fee
            // 
            this.DNPO_1_Fee.Location = new System.Drawing.Point(494, 14);
            this.DNPO_1_Fee.Name = "DNPO_1_Fee";
            this.DNPO_1_Fee.Size = new System.Drawing.Size(43, 18);
            this.DNPO_1_Fee.TabIndex = 46;
            this.DNPO_1_Fee.Text = "Нет";
            this.DNPO_1_Fee.ThemeName = "Office2013Light";
            this.DNPO_1_Fee.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.DNPO_1_Fee_ToggleStateChanged);
            // 
            // radLabel8
            // 
            this.radLabel8.Location = new System.Drawing.Point(12, 12);
            this.radLabel8.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.radLabel8.Name = "radLabel8";
            this.radLabel8.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.radLabel8.Size = new System.Drawing.Size(196, 19);
            this.radLabel8.TabIndex = 45;
            this.radLabel8.Text = "Пенсионные взносы за период с";
            this.radLabel8.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.radLabel8.ThemeName = "Office2013Light";
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(302, 66);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(110, 24);
            this.SaveBtn.TabIndex = 53;
            this.SaveBtn.Text = "Сохранить";
            this.SaveBtn.ThemeName = "Office2013Light";
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(427, 66);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(110, 24);
            this.CloseBtn.TabIndex = 54;
            this.CloseBtn.Text = "Закрыть";
            this.CloseBtn.ThemeName = "Office2013Light";
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // SZV_STAJ_5_Edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(551, 102);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.radLabel10);
            this.Controls.Add(this.DNPO_DateToMaskedEditBox);
            this.Controls.Add(this.DNPO_DateTo);
            this.Controls.Add(this.radLabel9);
            this.Controls.Add(this.DNPO_DateFromMaskedEditBox);
            this.Controls.Add(this.DNPO_DateFrom);
            this.Controls.Add(this.DNPO_1_Fee);
            this.Controls.Add(this.radLabel8);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SZV_STAJ_5_Edit";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сведения об уплаченных взносах";
            this.ThemeName = "Office2013Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SZV_STAJ_5_Edit_FormClosing);
            this.Load += new System.EventHandler(this.SZV_STAJ_5_Edit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_DateToMaskedEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_DateTo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_DateFromMaskedEditBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_DateFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DNPO_1_Fee)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SaveBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel10;
        private Telerik.WinControls.UI.RadMaskedEditBox DNPO_DateToMaskedEditBox;
        public Telerik.WinControls.UI.RadDateTimePicker DNPO_DateTo;
        private Telerik.WinControls.UI.RadLabel radLabel9;
        private Telerik.WinControls.UI.RadMaskedEditBox DNPO_DateFromMaskedEditBox;
        public Telerik.WinControls.UI.RadDateTimePicker DNPO_DateFrom;
        private Telerik.WinControls.UI.RadCheckBox DNPO_1_Fee;
        private Telerik.WinControls.UI.RadLabel radLabel8;
        private Telerik.WinControls.UI.RadButton SaveBtn;
        private Telerik.WinControls.UI.RadButton CloseBtn;
    }
}
