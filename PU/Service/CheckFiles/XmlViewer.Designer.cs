namespace PU.Service.CheckFiles
{
    partial class XmlViewer
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
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.editBtn = new Telerik.WinControls.UI.RadButton();
            this.saveBtn = new Telerik.WinControls.UI.RadButton();
            this.abortBtn = new Telerik.WinControls.UI.RadButton();
            this.radTextBox1 = new Telerik.WinControls.UI.RadTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.abortBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(620, 583);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(110, 24);
            this.closeBtn.TabIndex = 0;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // webBrowser
            // 
            this.webBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser.Location = new System.Drawing.Point(12, 12);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(718, 565);
            this.webBrowser.TabIndex = 1;
            // 
            // editBtn
            // 
            this.editBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.editBtn.Location = new System.Drawing.Point(12, 583);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(110, 24);
            this.editBtn.TabIndex = 3;
            this.editBtn.Text = "Редактирование";
            this.editBtn.ThemeName = "Office2013Light";
            this.editBtn.Click += new System.EventHandler(this.editBtn_Click);
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveBtn.Location = new System.Drawing.Point(12, 583);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(110, 24);
            this.saveBtn.TabIndex = 4;
            this.saveBtn.Text = "Сохранить";
            this.saveBtn.ThemeName = "Office2013Light";
            this.saveBtn.Visible = false;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // abortBtn
            // 
            this.abortBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.abortBtn.Location = new System.Drawing.Point(128, 583);
            this.abortBtn.Name = "abortBtn";
            this.abortBtn.Size = new System.Drawing.Size(110, 24);
            this.abortBtn.TabIndex = 5;
            this.abortBtn.Text = "Отмена";
            this.abortBtn.ThemeName = "Office2013Light";
            this.abortBtn.Visible = false;
            this.abortBtn.Click += new System.EventHandler(this.abortBtn_Click);
            // 
            // radTextBox1
            // 
            this.radTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radTextBox1.AutoSize = false;
            this.radTextBox1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.radTextBox1.Location = new System.Drawing.Point(12, 12);
            this.radTextBox1.MaxLength = 0;
            this.radTextBox1.Multiline = true;
            this.radTextBox1.Name = "radTextBox1";
            this.radTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.radTextBox1.Size = new System.Drawing.Size(718, 565);
            this.radTextBox1.TabIndex = 6;
            this.radTextBox1.ThemeName = "Office2013Light";
            this.radTextBox1.Visible = false;
            // 
            // XmlViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 619);
            this.Controls.Add(this.abortBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.radTextBox1);
            this.Controls.Add(this.webBrowser);
            this.Name = "XmlViewer";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Просмотр XML-файлов";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.XmlViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.saveBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.abortBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton closeBtn;
        public System.Windows.Forms.WebBrowser webBrowser;
        private Telerik.WinControls.UI.RadButton editBtn;
        private Telerik.WinControls.UI.RadButton saveBtn;
        private Telerik.WinControls.UI.RadButton abortBtn;
        private Telerik.WinControls.UI.RadTextBox radTextBox1;
    }
}
