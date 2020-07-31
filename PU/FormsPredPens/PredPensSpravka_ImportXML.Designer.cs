namespace PU.FormsPredPens
{
    partial class PredPensSpravka_ImportXML
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
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radPanel3 = new Telerik.WinControls.UI.RadPanel();
            this.currentPath = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.selectFileBtn = new Telerik.WinControls.UI.RadButton();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.printBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel3)).BeginInit();
            this.radPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currentPath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectFileBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radLabel1.Location = new System.Drawing.Point(22, 12);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(743, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Сведения об отнесении гражданина к категории граждан предпенсионного возраста";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radPanel3
            // 
            this.radPanel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.radPanel3.Controls.Add(this.currentPath);
            this.radPanel3.Controls.Add(this.radLabel2);
            this.radPanel3.Location = new System.Drawing.Point(12, 57);
            this.radPanel3.Name = "radPanel3";
            this.radPanel3.Size = new System.Drawing.Size(770, 25);
            this.radPanel3.TabIndex = 25;
            this.radPanel3.ThemeName = "Office2013Light";
            // 
            // currentPath
            // 
            this.currentPath.AutoSize = false;
            this.currentPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentPath.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.currentPath.Location = new System.Drawing.Point(107, 0);
            this.currentPath.Name = "currentPath";
            this.currentPath.Size = new System.Drawing.Size(663, 25);
            this.currentPath.TabIndex = 1;
            this.currentPath.Text = "не выбран";
            this.currentPath.ThemeName = "Office2013Light";
            // 
            // radLabel2
            // 
            this.radLabel2.AutoSize = false;
            this.radLabel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.radLabel2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.radLabel2.Location = new System.Drawing.Point(0, 0);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(107, 25);
            this.radLabel2.TabIndex = 0;
            this.radLabel2.Text = "Текущий файл:";
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // selectFileBtn
            // 
            this.selectFileBtn.Location = new System.Drawing.Point(677, 87);
            this.selectFileBtn.Name = "selectFileBtn";
            this.selectFileBtn.Size = new System.Drawing.Size(105, 24);
            this.selectFileBtn.TabIndex = 26;
            this.selectFileBtn.Text = "Файл";
            this.selectFileBtn.ThemeName = "Office2013Light";
            this.selectFileBtn.Click += new System.EventHandler(this.selectFileBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(677, 192);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(105, 24);
            this.closeBtn.TabIndex = 71;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // printBtn
            // 
            this.printBtn.Location = new System.Drawing.Point(677, 141);
            this.printBtn.Margin = new System.Windows.Forms.Padding(3, 3, 3, 24);
            this.printBtn.Name = "printBtn";
            this.printBtn.Size = new System.Drawing.Size(105, 24);
            this.printBtn.TabIndex = 70;
            this.printBtn.Text = "Печать";
            this.printBtn.ThemeName = "Office2013Light";
            this.printBtn.Click += new System.EventHandler(this.printBtn_Click);
            // 
            // PredPensSpravka_ImportXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 228);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.printBtn);
            this.Controls.Add(this.selectFileBtn);
            this.Controls.Add(this.radPanel3);
            this.Controls.Add(this.radLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PredPensSpravka_ImportXML";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Печать СППВ из XML файла";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.PredPensSpravka_ImportXML_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel3)).EndInit();
            this.radPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.currentPath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectFileBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadPanel radPanel3;
        private Telerik.WinControls.UI.RadLabel currentPath;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadButton selectFileBtn;
        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadButton printBtn;
    }
}
