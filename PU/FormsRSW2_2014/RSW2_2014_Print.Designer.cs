namespace PU.FormsRSW2_2014
{
    partial class RSW2_2014_Print
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
            Telerik.Reporting.InstanceReportSource instanceReportSource2 = new Telerik.Reporting.InstanceReportSource();
            this.reportBook1 = new Telerik.Reporting.ReportBook();
            this.office2013LightTheme1 = new Telerik.WinControls.Themes.Office2013LightTheme();
            this.reportViewer1 = new Telerik.ReportViewer.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            instanceReportSource2.ReportDocument = this.reportBook1;
            this.reportViewer1.ReportSource = instanceReportSource2;
            this.reportViewer1.Size = new System.Drawing.Size(892, 949);
            this.reportViewer1.TabIndex = 0;
            // 
            // RSW2_2014_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 949);
            this.Controls.Add(this.reportViewer1);
            this.Name = "RSW2_2014_Print";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Предварительный просмотр Формы РСВ-2";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.RSW2_2014_Print_Load);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.Themes.Office2013LightTheme office2013LightTheme1;
        private Telerik.Reporting.ReportBook reportBook1;
        public Telerik.ReportViewer.WinForms.ReportViewer reportViewer1;
    }
}
