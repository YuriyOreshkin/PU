namespace PU.Dictionaries
{
    partial class DolgnFormEdit
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
            this.radTextBoxName = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButtonCancel
            // 
            this.radButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.radButtonCancel.Location = new System.Drawing.Point(284, 85);
            this.radButtonCancel.Name = "radButtonCancel";
            this.radButtonCancel.Size = new System.Drawing.Size(110, 24);
            this.radButtonCancel.TabIndex = 6;
            this.radButtonCancel.Text = "Отмена";
            this.radButtonCancel.ThemeName = "Office2013Light";
            // 
            // radButtonSave
            // 
            this.radButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButtonSave.Location = new System.Drawing.Point(158, 85);
            this.radButtonSave.Name = "radButtonSave";
            this.radButtonSave.Size = new System.Drawing.Size(110, 24);
            this.radButtonSave.TabIndex = 5;
            this.radButtonSave.Tag = "Save";
            this.radButtonSave.Text = "Сохранить";
            this.radButtonSave.ThemeName = "Office2013Light";
            // 
            // radTextBoxName
            // 
            this.radTextBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radTextBoxName.Location = new System.Drawing.Point(134, 12);
            this.radTextBoxName.MaxLength = 150;
            this.radTextBoxName.Multiline = true;
            this.radTextBoxName.Name = "radTextBoxName";
            // 
            // 
            // 
            this.radTextBoxName.RootElement.StretchVertically = true;
            this.radTextBoxName.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.radTextBoxName.Size = new System.Drawing.Size(260, 50);
            this.radTextBoxName.TabIndex = 1;
            this.radTextBoxName.Tag = "Name";
            this.radTextBoxName.ThemeName = "Office2013Light";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(12, 14);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(92, 19);
            this.radLabel2.TabIndex = 10;
            this.radLabel2.Text = "Наименование";
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // DolgnFormEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 118);
            this.Controls.Add(this.radButtonCancel);
            this.Controls.Add(this.radButtonSave);
            this.Controls.Add(this.radTextBoxName);
            this.Controls.Add(this.radLabel2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DolgnFormEdit";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.ThemeName = "Office2013Light";
            ((System.ComponentModel.ISupportInitialize)(this.radButtonCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButtonSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTextBoxName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButtonCancel;
        private Telerik.WinControls.UI.RadButton radButtonSave;
        public Telerik.WinControls.UI.RadTextBox radTextBoxName;
        private Telerik.WinControls.UI.RadLabel radLabel2;
    }
}
