namespace PU
{
    partial class BaseManagerEdit
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
            this.nameTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            this.pathBrowser = new Telerik.WinControls.UI.RadBrowseEditor();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.fileNameTextBox = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.existedDBRadioBtn = new Telerik.WinControls.UI.RadRadioButton();
            this.newDBRadioBtn = new Telerik.WinControls.UI.RadRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nameTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathBrowser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileNameTextBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.existedDBRadioBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.newDBRadioBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(119, 70);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(379, 21);
            this.nameTextBox.TabIndex = 0;
            this.nameTextBox.ThemeName = "Office2013Light";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 70);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(92, 19);
            this.radLabel1.TabIndex = 1;
            this.radLabel1.Text = "Наименование";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(12, 99);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(33, 19);
            this.radLabel2.TabIndex = 3;
            this.radLabel2.Text = "Путь";
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(388, 163);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(110, 24);
            this.radButton2.TabIndex = 5;
            this.radButton2.Text = "Отмена";
            this.radButton2.ThemeName = "Office2013Light";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton3
            // 
            this.radButton3.Location = new System.Drawing.Point(272, 163);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(110, 24);
            this.radButton3.TabIndex = 5;
            this.radButton3.Text = "Сохранить";
            this.radButton3.ThemeName = "Office2013Light";
            this.radButton3.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // pathBrowser
            // 
            this.pathBrowser.Location = new System.Drawing.Point(119, 97);
            this.pathBrowser.Name = "pathBrowser";
            this.pathBrowser.Size = new System.Drawing.Size(379, 21);
            this.pathBrowser.TabIndex = 6;
            this.pathBrowser.ThemeName = "Office2013Light";
            this.pathBrowser.ValueChanged += new System.EventHandler(this.pathBrowser_ValueChanged);
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(12, 124);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(88, 19);
            this.radLabel3.TabIndex = 9;
            this.radLabel3.Text = "Имя файла БД";
            this.radLabel3.ThemeName = "Office2013Light";
            this.radLabel3.Visible = false;
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Location = new System.Drawing.Point(119, 124);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(358, 21);
            this.fileNameTextBox.TabIndex = 8;
            this.fileNameTextBox.ThemeName = "Office2013Light";
            this.fileNameTextBox.Visible = false;
            // 
            // radLabel4
            // 
            this.radLabel4.AutoSize = false;
            this.radLabel4.Location = new System.Drawing.Point(475, 125);
            this.radLabel4.Margin = new System.Windows.Forms.Padding(0);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(30, 19);
            this.radLabel4.TabIndex = 10;
            this.radLabel4.Text = ".db3";
            this.radLabel4.ThemeName = "Office2013Light";
            this.radLabel4.Visible = false;
            // 
            // existedDBRadioBtn
            // 
            this.existedDBRadioBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.existedDBRadioBtn.Enabled = false;
            this.existedDBRadioBtn.Location = new System.Drawing.Point(12, 12);
            this.existedDBRadioBtn.Name = "existedDBRadioBtn";
            this.existedDBRadioBtn.Size = new System.Drawing.Size(266, 19);
            this.existedDBRadioBtn.TabIndex = 11;
            this.existedDBRadioBtn.Text = "Указать путь к существующей базе данных";
            this.existedDBRadioBtn.ThemeName = "Office2013Light";
            this.existedDBRadioBtn.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.existedDBRadioBtn.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.existedDBRadioBtn_ToggleStateChanged);
            // 
            // newDBRadioBtn
            // 
            this.newDBRadioBtn.Enabled = false;
            this.newDBRadioBtn.Location = new System.Drawing.Point(12, 37);
            this.newDBRadioBtn.Name = "newDBRadioBtn";
            this.newDBRadioBtn.Size = new System.Drawing.Size(182, 19);
            this.newDBRadioBtn.TabIndex = 12;
            this.newDBRadioBtn.Text = "Создать новую базу данных";
            this.newDBRadioBtn.ThemeName = "Office2013Light";
            this.newDBRadioBtn.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.newDBRadioBtn_ToggleStateChanged);
            // 
            // BaseManagerEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 199);
            this.Controls.Add(this.existedDBRadioBtn);
            this.Controls.Add(this.newDBRadioBtn);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.pathBrowser);
            this.Controls.Add(this.radButton3);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.radLabel4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseManagerEdit";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Редактирование пути базы данных";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.BaseManagerEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nameTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pathBrowser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileNameTextBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.existedDBRadioBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.newDBRadioBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBox nameTextBox;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton3;
        private Telerik.WinControls.UI.RadBrowseEditor pathBrowser;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadTextBox fileNameTextBox;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadRadioButton existedDBRadioBtn;
        private Telerik.WinControls.UI.RadRadioButton newDBRadioBtn;
    }
}
