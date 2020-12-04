namespace PU.Dictionaries
{
    partial class DopTariffFormEdit
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
            this.radButtonCancel = new Telerik.WinControls.UI.RadButton();
            this.radButtonSave = new Telerik.WinControls.UI.RadButton();
            this.radMaskedEditBoxTariff2 = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.radMaskedEditBoxTariff1 = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.radSpinEditorYear = new Telerik.WinControls.UI.RadSpinEditor();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMaskedEditBoxTariff2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMaskedEditBoxTariff1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditorYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonCancel.Location = new System.Drawing.Point(155, 140);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancel.TabIndex = 6;
            this.radButtonCancel.Text = "Отмена";
            this.radButtonCancel.ThemeName = "Office2013Light";
            // 
            // radButtonSave
            // 
            this.radButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonSave.Location = new System.Drawing.Point(32, 140);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(110, 24);
            this.radButtonSave.TabIndex = 5;
            this.radButtonSave.Tag = "Save";
            this.radButtonSave.Text = "Сохранить";
            this.radButtonSave.ThemeName = "Office2013Light";
            // 
            // radMaskedEditBoxTariff2
            // 
            this.radMaskedEditBoxTariff2.Location = new System.Drawing.Point(183, 88);
            this.radMaskedEditBoxTariff2.Mask = "f";
            this.radMaskedEditBoxTariff2.MaskType = Telerik.WinControls.UI.MaskType.Numeric;
            this.radMaskedEditBoxTariff2.Name = "radMaskedEditBoxTariff2";
            this.radMaskedEditBoxTariff2.Size = new System.Drawing.Size(82, 21);
            this.radMaskedEditBoxTariff2.TabIndex = 20;
            this.radMaskedEditBoxTariff2.TabStop = false;
            this.radMaskedEditBoxTariff2.Tag = "Tariff2";
            this.radMaskedEditBoxTariff2.Text = "0,00";
            this.radMaskedEditBoxTariff2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.radMaskedEditBoxTariff2.ThemeName = "Office2013Light";
            // 
            // radMaskedEditBoxTariff1
            // 
            this.radMaskedEditBoxTariff1.Location = new System.Drawing.Point(183, 58);
            this.radMaskedEditBoxTariff1.Mask = "f";
            this.radMaskedEditBoxTariff1.MaskType = Telerik.WinControls.UI.MaskType.Numeric;
            this.radMaskedEditBoxTariff1.Name = "radMaskedEditBoxTariff1";
            this.radMaskedEditBoxTariff1.Size = new System.Drawing.Size(82, 21);
            this.radMaskedEditBoxTariff1.TabIndex = 19;
            this.radMaskedEditBoxTariff1.TabStop = false;
            this.radMaskedEditBoxTariff1.Tag = "Tariff1";
            this.radMaskedEditBoxTariff1.Text = "0,00";
            this.radMaskedEditBoxTariff1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.radMaskedEditBoxTariff1.ThemeName = "Office2013Light";
            // 
            // radSpinEditorYear
            // 
            this.radSpinEditorYear.Location = new System.Drawing.Point(183, 12);
            this.radSpinEditorYear.Maximum = new decimal(new int[] {
            2050,
            0,
            0,
            0});
            this.radSpinEditorYear.Minimum = new decimal(new int[] {
            1990,
            0,
            0,
            0});
            this.radSpinEditorYear.Name = "radSpinEditorYear";
            this.radSpinEditorYear.NullableValue = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            this.radSpinEditorYear.Size = new System.Drawing.Size(82, 21);
            this.radSpinEditorYear.TabIndex = 18;
            this.radSpinEditorYear.TabStop = false;
            this.radSpinEditorYear.Tag = "Year";
            this.radSpinEditorYear.TextAlignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.radSpinEditorYear.ThemeName = "Office2013Light";
            this.radSpinEditorYear.Value = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(103, 61);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(62, 19);
            this.radLabel3.TabIndex = 24;
            this.radLabel3.Text = "ч.1 ст.58.3";
            this.radLabel3.ThemeName = "Office2013Light";
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(103, 90);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(62, 19);
            this.radLabel4.TabIndex = 25;
            this.radLabel4.Text = "ч.2 ст.58.3";
            this.radLabel4.ThemeName = "Office2013Light";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(32, 36);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(79, 19);
            this.radLabel2.TabIndex = 23;
            this.radLabel2.Text = "Доп. тарифы";
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(32, 10);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(26, 19);
            this.radLabel5.TabIndex = 22;
            this.radLabel5.Text = "Год";
            this.radLabel5.ThemeName = "Office2013Light";
            // 
            // DopTariffFormEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 173);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.radMaskedEditBoxTariff2);
            this.Controls.Add(this.radMaskedEditBoxTariff1);
            this.Controls.Add(this.radSpinEditorYear);
            this.Controls.Add(this.radButtonCancel);
            this.Controls.Add(this.radButtonSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DopTariffFormEdit";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.ThemeName = "Office2013Light";
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMaskedEditBoxTariff2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMaskedEditBoxTariff1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditorYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        public Telerik.WinControls.UI.RadMaskedEditBox radMaskedEditBoxTariff2;
        public Telerik.WinControls.UI.RadMaskedEditBox radMaskedEditBoxTariff1;
        public Telerik.WinControls.UI.RadSpinEditor radSpinEditorYear;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel5;
    }
}
