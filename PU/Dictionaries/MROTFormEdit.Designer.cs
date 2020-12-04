namespace PU.Dictionaries
{
    partial class MROTFormEdit
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
            this.radMaskedEditBoxMrot = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.radMaskedEditBoxNalogBase = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radSpinEditorYear = new Telerik.WinControls.UI.RadSpinEditor();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMaskedEditBoxMrot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMaskedEditBoxNalogBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditorYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonCancel.Location = new System.Drawing.Point(155, 119);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancel.TabIndex = 6;
            this.radButtonCancel.Text = "Отмена";
            this.radButtonCancel.ThemeName = "Office2013Light";
            // 
            // radButtonSave
            // 
            this.radButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonSave.Location = new System.Drawing.Point(32, 119);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(110, 24);
            this.radButtonSave.TabIndex = 5;
            this.radButtonSave.Tag = "Save";
            this.radButtonSave.Text = "Сохранить";
            this.radButtonSave.ThemeName = "Office2013Light";
            // 
            // radMaskedEditBoxMrot
            // 
            this.radMaskedEditBoxMrot.Location = new System.Drawing.Point(183, 71);
            this.radMaskedEditBoxMrot.Mask = "f";
            this.radMaskedEditBoxMrot.MaskType = Telerik.WinControls.UI.MaskType.Numeric;
            this.radMaskedEditBoxMrot.Name = "radMaskedEditBoxMrot";
            this.radMaskedEditBoxMrot.Size = new System.Drawing.Size(82, 21);
            this.radMaskedEditBoxMrot.TabIndex = 20;
            this.radMaskedEditBoxMrot.TabStop = false;
            this.radMaskedEditBoxMrot.Tag = "Mrot1";
            this.radMaskedEditBoxMrot.Text = "0,00";
            this.radMaskedEditBoxMrot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.radMaskedEditBoxMrot.ThemeName = "Office2013Light";
            // 
            // radMaskedEditBoxNalogBase
            // 
            this.radMaskedEditBoxNalogBase.Location = new System.Drawing.Point(183, 41);
            this.radMaskedEditBoxNalogBase.Mask = "f";
            this.radMaskedEditBoxNalogBase.MaskType = Telerik.WinControls.UI.MaskType.Numeric;
            this.radMaskedEditBoxNalogBase.Name = "radMaskedEditBoxNalogBase";
            this.radMaskedEditBoxNalogBase.Size = new System.Drawing.Size(82, 21);
            this.radMaskedEditBoxNalogBase.TabIndex = 19;
            this.radMaskedEditBoxNalogBase.TabStop = false;
            this.radMaskedEditBoxNalogBase.Tag = "NalogBase";
            this.radMaskedEditBoxNalogBase.Text = "0,00";
            this.radMaskedEditBoxNalogBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.radMaskedEditBoxNalogBase.ThemeName = "Office2013Light";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(32, 73);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(40, 19);
            this.radLabel3.TabIndex = 23;
            this.radLabel3.Text = "МРОТ";
            this.radLabel3.ThemeName = "Office2013Light";
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
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(32, 43);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(95, 19);
            this.radLabel2.TabIndex = 22;
            this.radLabel2.Text = "Налоговая база";
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(32, 13);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(101, 19);
            this.radLabel1.TabIndex = 21;
            this.radLabel1.Text = "Финансовый год";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // MROTFormEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 152);
            this.Controls.Add(this.radMaskedEditBoxMrot);
            this.Controls.Add(this.radMaskedEditBoxNalogBase);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radSpinEditorYear);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.radButtonCancel);
            this.Controls.Add(this.radButtonSave);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MROTFormEdit";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.ThemeName = "Office2013Light";
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMaskedEditBoxMrot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMaskedEditBoxNalogBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radSpinEditorYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        public Telerik.WinControls.UI.RadMaskedEditBox radMaskedEditBoxMrot;
        public Telerik.WinControls.UI.RadMaskedEditBox radMaskedEditBoxNalogBase;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        public Telerik.WinControls.UI.RadSpinEditor radSpinEditorYear;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}
