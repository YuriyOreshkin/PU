namespace PU.Dictionaries
{
    partial class SpecOcenkaDopTariffEdit
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
            Telerik.WinControls.UI.RadListDataItem radListDataItem1 = new Telerik.WinControls.UI.RadListDataItem();
            Telerik.WinControls.UI.RadListDataItem radListDataItem2 = new Telerik.WinControls.UI.RadListDataItem();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.typeDDL = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.rate = new DevExpress.XtraEditors.TextEdit();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.dateBegin = new DevExpress.XtraEditors.DateEdit();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.dateEnd = new DevExpress.XtraEditors.DateEdit();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.typeDDL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 12);
            this.radLabel1.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(144, 19);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Основание применения";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // typeDDL
            // 
            this.typeDDL.AutoCompleteDisplayMember = null;
            this.typeDDL.AutoCompleteValueMember = null;
            this.typeDDL.AutoSizeItems = true;
            this.typeDDL.DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            radListDataItem1.Selected = true;
            radListDataItem1.Tag = "0";
            radListDataItem1.Text = "Спецоценка";
            radListDataItem2.Tag = "1";
            radListDataItem2.Text = "Аттестация рабочих мест";
            this.typeDDL.Items.Add(radListDataItem1);
            this.typeDDL.Items.Add(radListDataItem2);
            this.typeDDL.Location = new System.Drawing.Point(162, 12);
            this.typeDDL.Name = "typeDDL";
            this.typeDDL.Size = new System.Drawing.Size(221, 21);
            this.typeDDL.TabIndex = 1;
            this.typeDDL.ThemeName = "Office2013Light";
            ((Telerik.WinControls.UI.RadDropDownListElement)(this.typeDDL.GetChildAt(0))).DropDownStyle = Telerik.WinControls.RadDropDownStyle.DropDownList;
            ((Telerik.WinControls.UI.RadDropDownListElement)(this.typeDDL.GetChildAt(0))).AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.FitToAvailableSize;
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(12, 43);
            this.radLabel2.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(92, 19);
            this.radLabel2.TabIndex = 2;
            this.radLabel2.Text = "Тариф взносов";
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // rate
            // 
            this.rate.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.rate.Location = new System.Drawing.Point(162, 42);
            this.rate.Margin = new System.Windows.Forms.Padding(10);
            this.rate.Name = "rate";
            this.rate.Properties.AllowMouseWheel = false;
            this.rate.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.rate.Properties.Appearance.Options.UseTextOptions = true;
            this.rate.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.rate.Properties.Mask.EditMask = "n2";
            this.rate.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.rate.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.rate.Properties.NullText = "0,00";
            this.rate.Size = new System.Drawing.Size(83, 20);
            this.rate.TabIndex = 41;
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(12, 74);
            this.radLabel3.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(131, 19);
            this.radLabel3.TabIndex = 42;
            this.radLabel3.Text = "Период действия      с";
            this.radLabel3.ThemeName = "Office2013Light";
            // 
            // dateBegin
            // 
            this.dateBegin.EditValue = null;
            this.dateBegin.Location = new System.Drawing.Point(162, 73);
            this.dateBegin.Name = "dateBegin";
            this.dateBegin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateBegin.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateBegin.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.dateBegin.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.dateBegin.Properties.LookAndFeel.SkinName = "Office 2013 Light Gray";
            this.dateBegin.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.dateBegin.Size = new System.Drawing.Size(83, 20);
            this.dateBegin.TabIndex = 43;
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(263, 74);
            this.radLabel4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(21, 19);
            this.radLabel4.TabIndex = 44;
            this.radLabel4.Text = "по";
            this.radLabel4.ThemeName = "Office2013Light";
            // 
            // dateEnd
            // 
            this.dateEnd.EditValue = null;
            this.dateEnd.Location = new System.Drawing.Point(300, 73);
            this.dateEnd.Name = "dateEnd";
            this.dateEnd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEnd.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEnd.Properties.CalendarTimeProperties.CloseUpKey = new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.F4);
            this.dateEnd.Properties.CalendarTimeProperties.PopupBorderStyle = DevExpress.XtraEditors.Controls.PopupBorderStyles.Default;
            this.dateEnd.Properties.LookAndFeel.SkinName = "Office 2013 Light Gray";
            this.dateEnd.Properties.LookAndFeel.UseDefaultLookAndFeel = false;
            this.dateEnd.Size = new System.Drawing.Size(83, 20);
            this.dateEnd.TabIndex = 45;
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(273, 124);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(110, 24);
            this.radButton2.TabIndex = 47;
            this.radButton2.Text = "Отмена";
            this.radButton2.ThemeName = "Office2013Light";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(146, 124);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(110, 24);
            this.radButton1.TabIndex = 46;
            this.radButton1.Text = "Сохранить";
            this.radButton1.ThemeName = "Office2013Light";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // SpecOcenkaDopTariffEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 160);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.dateEnd);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.dateBegin);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.rate);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.typeDDL);
            this.Controls.Add(this.radLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpecOcenkaDopTariffEdit";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.ThemeName = "Office2013Light";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SpecOcenkaDopTariffEdit_FormClosed);
            this.Load += new System.EventHandler(this.SpecOcenkaDopTariffEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.typeDDL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateBegin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEnd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadDropDownList typeDDL;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private DevExpress.XtraEditors.TextEdit rate;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private DevExpress.XtraEditors.DateEdit dateBegin;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private DevExpress.XtraEditors.DateEdit dateEnd;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton1;
    }
}
