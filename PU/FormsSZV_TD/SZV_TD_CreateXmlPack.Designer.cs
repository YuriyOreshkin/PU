namespace PU.FormsSZV_TD
{
    partial class SZV_TD_CreateXmlPack
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
            this.remainChkbox = new Telerik.WinControls.UI.RadCheckBox();
            this.codeTOPFRTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.btnSave = new Telerik.WinControls.UI.RadButton();
            this.btnClose = new Telerik.WinControls.UI.RadButton();
            this.pathBrowser = new Telerik.WinControls.UI.RadBrowseEditor();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.remainChkbox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.codeTOPFRTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathBrowser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // remainChkbox
            // 
            this.remainChkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.remainChkbox.Location = new System.Drawing.Point(318, 63);
            this.remainChkbox.Name = "remainChkbox";
            this.remainChkbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.remainChkbox.Size = new System.Drawing.Size(112, 19);
            this.remainChkbox.TabIndex = 50;
            this.remainChkbox.Text = "запомнить путь";
            this.remainChkbox.ThemeName = "Office2013Light";
            this.remainChkbox.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            // 
            // codeTOPFRTextBox
            // 
            this.codeTOPFRTextBox.HideSelection = false;
            this.codeTOPFRTextBox.Location = new System.Drawing.Point(330, 93);
            this.codeTOPFRTextBox.MaxLength = 10;
            this.codeTOPFRTextBox.Name = "codeTOPFRTextBox";
            this.codeTOPFRTextBox.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.codeTOPFRTextBox.Size = new System.Drawing.Size(100, 18);
            this.codeTOPFRTextBox.TabIndex = 45;
            this.codeTOPFRTextBox.Text = "0";
            this.codeTOPFRTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.codeTOPFRTextBox.ThemeName = "Office2013Light";
            this.codeTOPFRTextBox.Visible = false;
            this.codeTOPFRTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.codeTOPFRTextBox_KeyPress);
            // 
            // radLabel6
            // 
            this.radLabel6.Location = new System.Drawing.Point(12, 94);
            this.radLabel6.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(207, 19);
            this.radLabel6.TabIndex = 49;
            this.radLabel6.Text = "Код территориального органа ПФР";
            this.radLabel6.ThemeName = "Office2013Light";
            this.radLabel6.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(204, 118);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 25, 3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 24);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "Сохранить";
            this.btnSave.ThemeName = "Office2013Light";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(320, 118);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 25, 3, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 24);
            this.btnClose.TabIndex = 47;
            this.btnClose.Text = "Закрыть";
            this.btnClose.ThemeName = "Office2013Light";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pathBrowser
            // 
            this.pathBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathBrowser.DialogType = Telerik.WinControls.UI.BrowseEditorDialogType.FolderBrowseDialog;
            this.pathBrowser.Location = new System.Drawing.Point(12, 36);
            this.pathBrowser.Name = "pathBrowser";
            this.pathBrowser.Size = new System.Drawing.Size(418, 21);
            this.pathBrowser.TabIndex = 44;
            this.pathBrowser.ThemeName = "Office2013Light";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 11);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(234, 19);
            this.radLabel1.TabIndex = 48;
            this.radLabel1.Text = "Местоположение выгружаемых файлов";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // SZV_TD_CreateXmlPack
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 154);
            this.Controls.Add(this.remainChkbox);
            this.Controls.Add(this.codeTOPFRTextBox);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pathBrowser);
            this.Controls.Add(this.radLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SZV_TD_CreateXmlPack";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Запись в XML-файл формы СЗВ-ТД";
            this.ThemeName = "Office2013Light";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SZV_TD_CreateXmlPack_FormClosing);
            this.Load += new System.EventHandler(this.SZV_TD_CreateXmlPack_Load);
            ((System.ComponentModel.ISupportInitialize)(this.remainChkbox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.codeTOPFRTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathBrowser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadCheckBox remainChkbox;
        private Telerik.WinControls.UI.RadTextBox codeTOPFRTextBox;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private Telerik.WinControls.UI.RadButton btnSave;
        private Telerik.WinControls.UI.RadButton btnClose;
        private Telerik.WinControls.UI.RadBrowseEditor pathBrowser;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}
