namespace PU.FormsODV1
{
    partial class ODV_1_4_Edit
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
            this.Year = new Telerik.WinControls.UI.RadSpinEditor();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.OPS = new DevExpress.XtraEditors.TextEdit();
            this.NAKOP = new DevExpress.XtraEditors.TextEdit();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.DopTar = new DevExpress.XtraEditors.TextEdit();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.SaveBtn = new Telerik.WinControls.UI.RadButton();
            this.CloseBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.Year)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.OPS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NAKOP.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DopTar.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SaveBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // Year
            // 
            this.Year.Location = new System.Drawing.Point(222, 10);
            this.Year.Maximum = new decimal(new int[] {
            2100,
            0,
            0,
            0});
            this.Year.Minimum = new decimal(new int[] {
            1990,
            0,
            0,
            0});
            this.Year.Name = "Year";
            this.Year.Size = new System.Drawing.Size(61, 21);
            this.Year.TabIndex = 0;
            this.Year.TabStop = false;
            this.Year.ThemeName = "Office2013Light";
            this.Year.Value = new decimal(new int[] {
            1990,
            0,
            0,
            0});
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 12);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(26, 19);
            this.radLabel1.TabIndex = 3;
            this.radLabel1.Text = "Год";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(12, 37);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(132, 19);
            this.radLabel2.TabIndex = 5;
            this.radLabel2.Text = "На страховую пенсию";
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // OPS
            // 
            this.OPS.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.OPS.EnterMoveNextControl = true;
            this.OPS.Location = new System.Drawing.Point(222, 37);
            this.OPS.Name = "OPS";
            this.OPS.Properties.AllowMouseWheel = false;
            this.OPS.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.OPS.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.OPS.Properties.Appearance.Options.UseForeColor = true;
            this.OPS.Properties.Appearance.Options.UseTextOptions = true;
            this.OPS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.OPS.Properties.Mask.EditMask = "n2";
            this.OPS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.OPS.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.OPS.Properties.NullText = "0,00";
            this.OPS.Size = new System.Drawing.Size(100, 20);
            this.OPS.TabIndex = 1;
            // 
            // NAKOP
            // 
            this.NAKOP.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.NAKOP.EnterMoveNextControl = true;
            this.NAKOP.Location = new System.Drawing.Point(222, 63);
            this.NAKOP.Name = "NAKOP";
            this.NAKOP.Properties.AllowMouseWheel = false;
            this.NAKOP.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.NAKOP.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.NAKOP.Properties.Appearance.Options.UseForeColor = true;
            this.NAKOP.Properties.Appearance.Options.UseTextOptions = true;
            this.NAKOP.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.NAKOP.Properties.Mask.EditMask = "n2";
            this.NAKOP.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.NAKOP.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.NAKOP.Properties.NullText = "0,00";
            this.NAKOP.Size = new System.Drawing.Size(100, 20);
            this.NAKOP.TabIndex = 2;
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(12, 63);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(161, 19);
            this.radLabel3.TabIndex = 7;
            this.radLabel3.Text = "На накопительную пенсию";
            this.radLabel3.ThemeName = "Office2013Light";
            // 
            // DopTar
            // 
            this.DopTar.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.DopTar.EnterMoveNextControl = true;
            this.DopTar.Location = new System.Drawing.Point(222, 89);
            this.DopTar.Name = "DopTar";
            this.DopTar.Properties.AllowMouseWheel = false;
            this.DopTar.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.DopTar.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.DopTar.Properties.Appearance.Options.UseForeColor = true;
            this.DopTar.Properties.Appearance.Options.UseTextOptions = true;
            this.DopTar.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.DopTar.Properties.Mask.EditMask = "n2";
            this.DopTar.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.DopTar.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.DopTar.Properties.NullText = "0,00";
            this.DopTar.Size = new System.Drawing.Size(100, 20);
            this.DopTar.TabIndex = 3;
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(12, 89);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(177, 19);
            this.radLabel4.TabIndex = 9;
            this.radLabel4.Text = "По тарифу страховых взносов";
            this.radLabel4.ThemeName = "Office2013Light";
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(87, 148);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(110, 24);
            this.SaveBtn.TabIndex = 4;
            this.SaveBtn.Text = "Сохранить";
            this.SaveBtn.ThemeName = "Office2013Light";
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(212, 148);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(110, 24);
            this.CloseBtn.TabIndex = 5;
            this.CloseBtn.Text = "Закрыть";
            this.CloseBtn.ThemeName = "Office2013Light";
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // ODV_1_4_Edit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 182);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.DopTar);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.NAKOP);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.OPS);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.Year);
            this.Controls.Add(this.radLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ODV_1_4_Edit";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Данные уплаты страховых взносов за период";
            this.ThemeName = "Office2013Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ODV_1_4_Edit_FormClosing);
            this.Load += new System.EventHandler(this.ODV_1_4_Edit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Year)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.OPS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NAKOP.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DopTar.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SaveBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CloseBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadSpinEditor Year;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private DevExpress.XtraEditors.TextEdit OPS;
        private DevExpress.XtraEditors.TextEdit NAKOP;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private DevExpress.XtraEditors.TextEdit DopTar;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadButton SaveBtn;
        private Telerik.WinControls.UI.RadButton CloseBtn;
    }
}
