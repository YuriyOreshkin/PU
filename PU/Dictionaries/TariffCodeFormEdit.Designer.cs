﻿namespace PU.Dictionaries
{
    partial class TariffCodeFormEdit
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
            this.radTextBoxCode = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel7 = new Telerik.WinControls.UI.RadLabel();
            this.radDateTimePickerDateBegin = new Telerik.WinControls.UI.RadDateTimePicker();
            this.radDateTimePickerDateEnd = new Telerik.WinControls.UI.RadDateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerDateBegin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerDateEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonCancel.Location = new System.Drawing.Point(282, 95);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancel.TabIndex = 6;
            this.radButtonCancel.Text = "Отмена";
            this.radButtonCancel.ThemeName = "Office2013Light";
            // 
            // radButtonSave
            // 
            this.radButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonSave.Location = new System.Drawing.Point(158, 95);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(110, 24);
            this.radButtonSave.TabIndex = 5;
            this.radButtonSave.Tag = "Save";
            this.radButtonSave.Text = "Сохранить";
            this.radButtonSave.ThemeName = "Office2013Light";
            // 
            // radTextBoxCode
            // 
            this.radTextBoxCode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radTextBoxCode.Location = new System.Drawing.Point(134, 10);
            this.radTextBoxCode.MaxLength = 10;
            this.radTextBoxCode.Name = "radTextBoxCode";
            this.radTextBoxCode.Size = new System.Drawing.Size(113, 21);
            this.radTextBoxCode.TabIndex = 0;
            this.radTextBoxCode.Tag = "Code";
            this.radTextBoxCode.ThemeName = "Office2013Light";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 12);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(28, 19);
            this.radLabel1.TabIndex = 9;
            this.radLabel1.Text = "Код";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // radLabel6
            // 
            this.radLabel6.Location = new System.Drawing.Point(253, 43);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(21, 19);
            this.radLabel6.TabIndex = 49;
            this.radLabel6.Text = "по";
            this.radLabel6.ThemeName = "Office2013Light";
            // 
            // radLabel7
            // 
            this.radLabel7.Location = new System.Drawing.Point(12, 43);
            this.radLabel7.Name = "radLabel7";
            this.radLabel7.Size = new System.Drawing.Size(105, 19);
            this.radLabel7.TabIndex = 48;
            this.radLabel7.Text = "Срок действия   с";
            this.radLabel7.ThemeName = "Office2013Light";
            // 
            // radDateTimePickerDateBegin
            // 
            this.radDateTimePickerDateBegin.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.radDateTimePickerDateBegin.Location = new System.Drawing.Point(134, 42);
            this.radDateTimePickerDateBegin.Name = "radDateTimePickerDateBegin";
            this.radDateTimePickerDateBegin.Size = new System.Drawing.Size(113, 20);
            this.radDateTimePickerDateBegin.TabIndex = 2;
            this.radDateTimePickerDateBegin.TabStop = false;
            this.radDateTimePickerDateBegin.Tag = "DateBegin";
            this.radDateTimePickerDateBegin.Value = new System.DateTime(((long)(0)));
            // 
            // radDateTimePickerDateEnd
            // 
            this.radDateTimePickerDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.radDateTimePickerDateEnd.Location = new System.Drawing.Point(282, 43);
            this.radDateTimePickerDateEnd.Name = "radDateTimePickerDateEnd";
            this.radDateTimePickerDateEnd.Size = new System.Drawing.Size(110, 20);
            this.radDateTimePickerDateEnd.TabIndex = 3;
            this.radDateTimePickerDateEnd.TabStop = false;
            this.radDateTimePickerDateEnd.Tag = "DateEnd";
            this.radDateTimePickerDateEnd.Value = new System.DateTime(((long)(0)));
            // 
            // TariffCodeFormEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 128);
            this.Controls.Add(this.radDateTimePickerDateEnd);
            this.Controls.Add(this.radDateTimePickerDateBegin);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.radLabel7);
            this.Controls.Add(this.radButtonCancel);
            this.Controls.Add(this.radButtonSave);
            this.Controls.Add(this.radTextBoxCode);
            this.Controls.Add(this.radLabel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TariffCodeFormEdit";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.ThemeName = "Office2013Light";
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerDateBegin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radDateTimePickerDateEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        public Telerik.WinControls.UI.RadTextBox radTextBoxCode;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private Telerik.WinControls.UI.RadLabel radLabel7;
        private Telerik.WinControls.UI.RadDateTimePicker radDateTimePickerDateBegin;
        private Telerik.WinControls.UI.RadDateTimePicker radDateTimePickerDateEnd;
    }
}
