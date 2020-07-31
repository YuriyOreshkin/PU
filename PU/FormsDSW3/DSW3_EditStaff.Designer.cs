namespace PU.FormsDSW3
{
    partial class DSW3_EditStaff
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
            this.INN = new Telerik.WinControls.UI.RadTextBox();
            this.FirstName = new Telerik.WinControls.UI.RadTextBox();
            this.LastName = new Telerik.WinControls.UI.RadTextBox();
            this.MiddleName = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.TabelNumber = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.SNILS = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel8 = new Telerik.WinControls.UI.RadLabel();
            this.SUMFEEPFR_EMPLOYERS = new DevExpress.XtraEditors.TextEdit();
            this.radLabel7 = new Telerik.WinControls.UI.RadLabel();
            this.SUMFEEPFR_PAYER = new DevExpress.XtraEditors.TextEdit();
            this.radLabel9 = new Telerik.WinControls.UI.RadLabel();
            this.newStaffLabel = new System.Windows.Forms.Label();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.saveBtn = new Telerik.WinControls.UI.RadButton();
            this.selectStaffBtn = new Telerik.WinControls.UI.RadButton();
            this.radLabel10 = new Telerik.WinControls.UI.RadLabel();
            this.dsw3Number = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.INN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirstName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabelNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SNILS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUMFEEPFR_EMPLOYERS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUMFEEPFR_PAYER.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectStaffBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // INN
            // 
            this.INN.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.INN.Location = new System.Drawing.Point(285, 106);
            this.INN.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.INN.MaxLength = 12;
            this.INN.Name = "INN";
            this.INN.Size = new System.Drawing.Size(98, 21);
            this.INN.TabIndex = 18;
            this.INN.ThemeName = "Office2013Light";
            this.INN.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.INN__KeyPress);
            this.INN.Leave += new System.EventHandler(this.INN__Leave);
            // 
            // FirstName
            // 
            this.FirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.FirstName.Location = new System.Drawing.Point(128, 170);
            this.FirstName.Name = "FirstName";
            this.FirstName.Size = new System.Drawing.Size(255, 21);
            this.FirstName.TabIndex = 20;
            this.FirstName.ThemeName = "Office2013Light";
            // 
            // LastName
            // 
            this.LastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LastName.Location = new System.Drawing.Point(128, 143);
            this.LastName.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.LastName.Name = "LastName";
            this.LastName.Size = new System.Drawing.Size(255, 21);
            this.LastName.TabIndex = 19;
            this.LastName.ThemeName = "Office2013Light";
            // 
            // MiddleName
            // 
            this.MiddleName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.MiddleName.Location = new System.Drawing.Point(128, 197);
            this.MiddleName.Name = "MiddleName";
            this.MiddleName.Size = new System.Drawing.Size(255, 21);
            this.MiddleName.TabIndex = 22;
            this.MiddleName.ThemeName = "Office2013Light";
            // 
            // radLabel6
            // 
            this.radLabel6.Location = new System.Drawing.Point(12, 198);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(59, 19);
            this.radLabel6.TabIndex = 25;
            this.radLabel6.Text = "Отчество";
            this.radLabel6.ThemeName = "Office2013Light";
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(12, 171);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(31, 19);
            this.radLabel5.TabIndex = 24;
            this.radLabel5.Text = "Имя";
            this.radLabel5.ThemeName = "Office2013Light";
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(12, 144);
            this.radLabel4.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(58, 19);
            this.radLabel4.TabIndex = 23;
            this.radLabel4.Text = "Фамилия";
            this.radLabel4.ThemeName = "Office2013Light";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(246, 107);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(33, 19);
            this.radLabel3.TabIndex = 21;
            this.radLabel3.Text = "ИНН";
            this.radLabel3.ThemeName = "Office2013Light";
            // 
            // TabelNumber
            // 
            this.TabelNumber.Location = new System.Drawing.Point(128, 106);
            this.TabelNumber.Mask = "d";
            this.TabelNumber.MaskType = Telerik.WinControls.UI.MaskType.Numeric;
            this.TabelNumber.Name = "TabelNumber";
            this.TabelNumber.Size = new System.Drawing.Size(100, 21);
            this.TabelNumber.TabIndex = 16;
            this.TabelNumber.TabStop = false;
            this.TabelNumber.Text = "0";
            this.TabelNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.TabelNumber.ThemeName = "Office2013Light";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(12, 107);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(109, 19);
            this.radLabel2.TabIndex = 17;
            this.radLabel2.Text = "Табельный номер";
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // SNILS
            // 
            this.SNILS.Location = new System.Drawing.Point(128, 78);
            this.SNILS.Mask = "000-000-000 00";
            this.SNILS.MaskType = Telerik.WinControls.UI.MaskType.Standard;
            this.SNILS.Name = "SNILS";
            this.SNILS.Size = new System.Drawing.Size(100, 21);
            this.SNILS.TabIndex = 14;
            this.SNILS.TabStop = false;
            this.SNILS.Text = "___-___-___ __";
            this.SNILS.ThemeName = "Office2013Light";
            this.SNILS.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Snils__KeyPress);
            this.SNILS.Leave += new System.EventHandler(this.Snils__Leave);
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 9.25F);
            this.radLabel1.Location = new System.Drawing.Point(12, 75);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(110, 20);
            this.radLabel1.TabIndex = 15;
            this.radLabel1.Text = "Страховой номер";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // radLabel8
            // 
            this.radLabel8.Location = new System.Drawing.Point(12, 235);
            this.radLabel8.Margin = new System.Windows.Forms.Padding(3, 15, 3, 3);
            this.radLabel8.Name = "radLabel8";
            this.radLabel8.Size = new System.Drawing.Size(218, 19);
            this.radLabel8.TabIndex = 27;
            this.radLabel8.Text = "Сумма доп.взносов, перечисленных: ";
            this.radLabel8.ThemeName = "Office2013Light";
            // 
            // SUMFEEPFR_EMPLOYERS
            // 
            this.SUMFEEPFR_EMPLOYERS.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.SUMFEEPFR_EMPLOYERS.EnterMoveNextControl = true;
            this.SUMFEEPFR_EMPLOYERS.Location = new System.Drawing.Point(128, 259);
            this.SUMFEEPFR_EMPLOYERS.Margin = new System.Windows.Forms.Padding(20, 0, 10, 0);
            this.SUMFEEPFR_EMPLOYERS.Name = "SUMFEEPFR_EMPLOYERS";
            this.SUMFEEPFR_EMPLOYERS.Properties.AllowMouseWheel = false;
            this.SUMFEEPFR_EMPLOYERS.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.SUMFEEPFR_EMPLOYERS.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SUMFEEPFR_EMPLOYERS.Properties.Appearance.Options.UseForeColor = true;
            this.SUMFEEPFR_EMPLOYERS.Properties.Appearance.Options.UseTextOptions = true;
            this.SUMFEEPFR_EMPLOYERS.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.SUMFEEPFR_EMPLOYERS.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.SUMFEEPFR_EMPLOYERS.Properties.Mask.EditMask = "n2";
            this.SUMFEEPFR_EMPLOYERS.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.SUMFEEPFR_EMPLOYERS.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.SUMFEEPFR_EMPLOYERS.Properties.NullText = "0,00";
            this.SUMFEEPFR_EMPLOYERS.Size = new System.Drawing.Size(70, 20);
            this.SUMFEEPFR_EMPLOYERS.TabIndex = 29;
            // 
            // radLabel7
            // 
            this.radLabel7.Location = new System.Drawing.Point(12, 260);
            this.radLabel7.Name = "radLabel7";
            this.radLabel7.Size = new System.Drawing.Size(83, 19);
            this.radLabel7.TabIndex = 28;
            this.radLabel7.Text = "Сотрудником";
            this.radLabel7.ThemeName = "Office2013Light";
            // 
            // SUMFEEPFR_PAYER
            // 
            this.SUMFEEPFR_PAYER.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.SUMFEEPFR_PAYER.EnterMoveNextControl = true;
            this.SUMFEEPFR_PAYER.Location = new System.Drawing.Point(313, 259);
            this.SUMFEEPFR_PAYER.Margin = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.SUMFEEPFR_PAYER.Name = "SUMFEEPFR_PAYER";
            this.SUMFEEPFR_PAYER.Properties.AllowMouseWheel = false;
            this.SUMFEEPFR_PAYER.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
            this.SUMFEEPFR_PAYER.Properties.Appearance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.SUMFEEPFR_PAYER.Properties.Appearance.Options.UseForeColor = true;
            this.SUMFEEPFR_PAYER.Properties.Appearance.Options.UseTextOptions = true;
            this.SUMFEEPFR_PAYER.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.SUMFEEPFR_PAYER.Properties.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Office2003;
            this.SUMFEEPFR_PAYER.Properties.Mask.EditMask = "n2";
            this.SUMFEEPFR_PAYER.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.SUMFEEPFR_PAYER.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.SUMFEEPFR_PAYER.Properties.NullText = "0,00";
            this.SUMFEEPFR_PAYER.Size = new System.Drawing.Size(70, 20);
            this.SUMFEEPFR_PAYER.TabIndex = 31;
            // 
            // radLabel9
            // 
            this.radLabel9.Location = new System.Drawing.Point(208, 260);
            this.radLabel9.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.radLabel9.Name = "radLabel9";
            this.radLabel9.Size = new System.Drawing.Size(92, 19);
            this.radLabel9.TabIndex = 30;
            this.radLabel9.Text = "Страхователем";
            this.radLabel9.ThemeName = "Office2013Light";
            // 
            // newStaffLabel
            // 
            this.newStaffLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.newStaffLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.newStaffLabel.Location = new System.Drawing.Point(12, 284);
            this.newStaffLabel.Margin = new System.Windows.Forms.Padding(3, 10, 3, 4);
            this.newStaffLabel.Name = "newStaffLabel";
            this.newStaffLabel.Size = new System.Drawing.Size(371, 33);
            this.newStaffLabel.TabIndex = 32;
            this.newStaffLabel.Text = "Сотрудник не найден в базе данных, будет добавлен при сохранении";
            this.newStaffLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.newStaffLabel.Visible = false;
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(273, 322);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(110, 24);
            this.closeBtn.TabIndex = 34;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveBtn.Location = new System.Drawing.Point(157, 322);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(110, 24);
            this.saveBtn.TabIndex = 33;
            this.saveBtn.Text = "Сохранить";
            this.saveBtn.ThemeName = "Office2013Light";
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // selectStaffBtn
            // 
            this.selectStaffBtn.Location = new System.Drawing.Point(246, 78);
            this.selectStaffBtn.Name = "selectStaffBtn";
            this.selectStaffBtn.Size = new System.Drawing.Size(137, 21);
            this.selectStaffBtn.TabIndex = 47;
            this.selectStaffBtn.Text = "Выбор сотрудника";
            this.selectStaffBtn.ThemeName = "Office2013Light";
            this.selectStaffBtn.Click += new System.EventHandler(this.selectStaffBtn_Click);
            // 
            // radLabel10
            // 
            this.radLabel10.AutoSize = false;
            this.radLabel10.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radLabel10.Location = new System.Drawing.Point(12, 12);
            this.radLabel10.Name = "radLabel10";
            this.radLabel10.Size = new System.Drawing.Size(371, 23);
            this.radLabel10.TabIndex = 48;
            this.radLabel10.Text = "Платежное поручение";
            this.radLabel10.TextAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.radLabel10.ThemeName = "Office2013Light";
            // 
            // dsw3Number
            // 
            this.dsw3Number.BackColor = System.Drawing.Color.Transparent;
            this.dsw3Number.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dsw3Number.ForeColor = System.Drawing.Color.Blue;
            this.dsw3Number.Location = new System.Drawing.Point(8, 38);
            this.dsw3Number.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.dsw3Number.Name = "dsw3Number";
            this.dsw3Number.Size = new System.Drawing.Size(375, 27);
            this.dsw3Number.TabIndex = 49;
            this.dsw3Number.Text = "Номер поручения";
            this.dsw3Number.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DSW3_EditStaff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 358);
            this.Controls.Add(this.dsw3Number);
            this.Controls.Add(this.radLabel10);
            this.Controls.Add(this.selectStaffBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.newStaffLabel);
            this.Controls.Add(this.SUMFEEPFR_PAYER);
            this.Controls.Add(this.radLabel9);
            this.Controls.Add(this.SUMFEEPFR_EMPLOYERS);
            this.Controls.Add(this.radLabel7);
            this.Controls.Add(this.radLabel8);
            this.Controls.Add(this.INN);
            this.Controls.Add(this.FirstName);
            this.Controls.Add(this.LastName);
            this.Controls.Add(this.MiddleName);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.TabelNumber);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.SNILS);
            this.Controls.Add(this.radLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DSW3_EditStaff";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Изменение сотрудника и доп.взносов";
            this.ThemeName = "Office2013Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DSW3_EditStaff_FormClosing);
            this.Load += new System.EventHandler(this.DSW3_EditStaff_Load);
            ((System.ComponentModel.ISupportInitialize)(this.INN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FirstName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LastName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MiddleName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabelNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SNILS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUMFEEPFR_EMPLOYERS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUMFEEPFR_PAYER.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectStaffBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBox INN;
        private Telerik.WinControls.UI.RadTextBox FirstName;
        private Telerik.WinControls.UI.RadTextBox LastName;
        private Telerik.WinControls.UI.RadTextBox MiddleName;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadMaskedEditBox TabelNumber;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadMaskedEditBox SNILS;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel8;
        private DevExpress.XtraEditors.TextEdit SUMFEEPFR_EMPLOYERS;
        private Telerik.WinControls.UI.RadLabel radLabel7;
        private DevExpress.XtraEditors.TextEdit SUMFEEPFR_PAYER;
        private Telerik.WinControls.UI.RadLabel radLabel9;
        private System.Windows.Forms.Label newStaffLabel;
        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadButton saveBtn;
        private Telerik.WinControls.UI.RadButton selectStaffBtn;
        private Telerik.WinControls.UI.RadLabel radLabel10;
        private System.Windows.Forms.Label dsw3Number;
    }
}
