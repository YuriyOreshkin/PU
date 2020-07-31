namespace PU.FormsDSW3
{
    partial class DSW3_FillStaff
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
            Telerik.WinControls.UI.GridViewCheckBoxColumn gridViewCheckBoxColumn1 = new Telerik.WinControls.UI.GridViewCheckBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.dsw3Number = new System.Windows.Forms.Label();
            this.staffGridView = new Telerik.WinControls.UI.RadGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.SUMFEEPFR_EMPLOYERS = new DevExpress.XtraEditors.TextEdit();
            this.SUMFEEPFR_PAYER = new DevExpress.XtraEditors.TextEdit();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.startBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUMFEEPFR_EMPLOYERS.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUMFEEPFR_PAYER.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.Location = new System.Drawing.Point(12, 12);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(656, 34);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Отметьте одного или нескольких сотрудников, за которых перечислены доп.взносы и к" +
    "оторые относятся к Платежному поручению:";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // dsw3Number
            // 
            this.dsw3Number.BackColor = System.Drawing.Color.Transparent;
            this.dsw3Number.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dsw3Number.ForeColor = System.Drawing.Color.Blue;
            this.dsw3Number.Location = new System.Drawing.Point(12, 49);
            this.dsw3Number.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.dsw3Number.Name = "dsw3Number";
            this.dsw3Number.Size = new System.Drawing.Size(718, 27);
            this.dsw3Number.TabIndex = 1;
            this.dsw3Number.Text = "Номер поручения";
            this.dsw3Number.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // staffGridView
            // 
            this.staffGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.staffGridView.Location = new System.Drawing.Point(12, 89);
            // 
            // 
            // 
            this.staffGridView.MasterTemplate.AllowAddNewRow = false;
            this.staffGridView.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.staffGridView.MasterTemplate.AllowColumnReorder = false;
            this.staffGridView.MasterTemplate.AllowDeleteRow = false;
            gridViewCheckBoxColumn1.Checked = Telerik.WinControls.Enumerations.ToggleState.Indeterminate;
            gridViewCheckBoxColumn1.EnableHeaderCheckBox = true;
            gridViewCheckBoxColumn1.HeaderText = "";
            gridViewCheckBoxColumn1.HeaderTextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            gridViewCheckBoxColumn1.Name = "CheckBox";
            gridViewCheckBoxColumn1.StretchVertically = false;
            this.staffGridView.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewCheckBoxColumn1});
            this.staffGridView.MasterTemplate.EnableFiltering = true;
            this.staffGridView.MasterTemplate.EnableGrouping = false;
            this.staffGridView.MasterTemplate.ShowRowHeaderColumn = false;
            this.staffGridView.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.staffGridView.Name = "staffGridView";
            this.staffGridView.ShowRowErrors = false;
            this.staffGridView.Size = new System.Drawing.Size(718, 283);
            this.staffGridView.TabIndex = 6;
            this.staffGridView.ThemeName = "Office2013Light";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(9, 385);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(721, 33);
            this.label2.TabIndex = 7;
            this.label2.Text = "В случае, если копируемые сотрудники и/или страхователь, перечисляют одинаковые с" +
    "уммы доп.взносов, Вы можете внести здесь перечисляемую сумму по сотруднику";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(12, 425);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(218, 19);
            this.radLabel2.TabIndex = 8;
            this.radLabel2.Text = "Сумма доп.взносов, перечисленных: ";
            this.radLabel2.ThemeName = "Office2013Light";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(236, 425);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(83, 19);
            this.radLabel3.TabIndex = 9;
            this.radLabel3.Text = "Сотрудником";
            this.radLabel3.ThemeName = "Office2013Light";
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(451, 425);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(92, 19);
            this.radLabel4.TabIndex = 10;
            this.radLabel4.Text = "Страхователем";
            this.radLabel4.ThemeName = "Office2013Light";
            // 
            // SUMFEEPFR_EMPLOYERS
            // 
            this.SUMFEEPFR_EMPLOYERS.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.SUMFEEPFR_EMPLOYERS.EnterMoveNextControl = true;
            this.SUMFEEPFR_EMPLOYERS.Location = new System.Drawing.Point(335, 424);
            this.SUMFEEPFR_EMPLOYERS.Margin = new System.Windows.Forms.Padding(20, 0, 20, 0);
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
            this.SUMFEEPFR_EMPLOYERS.Size = new System.Drawing.Size(93, 20);
            this.SUMFEEPFR_EMPLOYERS.TabIndex = 11;
            // 
            // SUMFEEPFR_PAYER
            // 
            this.SUMFEEPFR_PAYER.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.SUMFEEPFR_PAYER.EnterMoveNextControl = true;
            this.SUMFEEPFR_PAYER.Location = new System.Drawing.Point(566, 424);
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
            this.SUMFEEPFR_PAYER.Size = new System.Drawing.Size(93, 20);
            this.SUMFEEPFR_PAYER.TabIndex = 12;
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(620, 464);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(110, 24);
            this.closeBtn.TabIndex = 14;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(504, 464);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(110, 24);
            this.startBtn.TabIndex = 13;
            this.startBtn.Text = "Начать";
            this.startBtn.ThemeName = "Office2013Light";
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // DSW3_FillStaff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 500);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.SUMFEEPFR_PAYER);
            this.Controls.Add(this.SUMFEEPFR_EMPLOYERS);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.staffGridView);
            this.Controls.Add(this.dsw3Number);
            this.Controls.Add(this.radLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "DSW3_FillStaff";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Заполнение из списка сотрудников";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.DSW3_FillStaff_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.staffGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUMFEEPFR_EMPLOYERS.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SUMFEEPFR_PAYER.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.Label dsw3Number;
        private Telerik.WinControls.UI.RadGridView staffGridView;
        private System.Windows.Forms.Label label2;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private DevExpress.XtraEditors.TextEdit SUMFEEPFR_EMPLOYERS;
        private DevExpress.XtraEditors.TextEdit SUMFEEPFR_PAYER;
        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadButton startBtn;
    }
}
