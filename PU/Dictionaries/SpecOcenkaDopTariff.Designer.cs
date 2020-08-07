namespace PU.Dictionaries
{
    partial class SpecOcenkaDopTariff
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn1 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn5 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            this.radButton4 = new Telerik.WinControls.UI.RadButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(483, 241);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(90, 24);
            this.radButton2.TabIndex = 6;
            this.radButton2.Text = "Изменить";
            this.radButton2.ThemeName = "Office2013Light";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton3
            // 
            this.radButton3.Location = new System.Drawing.Point(483, 271);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(90, 24);
            this.radButton3.TabIndex = 7;
            this.radButton3.Text = "Удалить";
            this.radButton3.ThemeName = "Office2013Light";
            this.radButton3.Visible = false;
            this.radButton3.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // radButton4
            // 
            this.radButton4.Location = new System.Drawing.Point(483, 338);
            this.radButton4.Name = "radButton4";
            this.radButton4.Size = new System.Drawing.Size(90, 24);
            this.radButton4.TabIndex = 8;
            this.radButton4.Text = "Закрыть";
            this.radButton4.ThemeName = "Office2013Light";
            this.radButton4.Click += new System.EventHandler(this.radButton4_Click);
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(483, 211);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(90, 24);
            this.radButton1.TabIndex = 5;
            this.radButton1.Text = "Добавить";
            this.radButton1.ThemeName = "Office2013Light";
            this.radButton1.Visible = false;
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radGridView1
            // 
            this.radGridView1.EnableCustomSorting = true;
            this.radGridView1.Location = new System.Drawing.Point(12, 34);
            // 
            // radGridView1
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowEditRow = false;
            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            gridViewTextBoxColumn1.HeaderText = "column1";
            gridViewTextBoxColumn1.IsVisible = false;
            gridViewTextBoxColumn1.Name = "ID";
            gridViewTextBoxColumn1.Width = 282;
            gridViewTextBoxColumn2.HeaderText = "Тип";
            gridViewTextBoxColumn2.Name = "Type";
            gridViewTextBoxColumn2.Width = 157;
            gridViewTextBoxColumn3.HeaderText = "Тариф взносов";
            gridViewTextBoxColumn3.Name = "Rate";
            gridViewTextBoxColumn3.Width = 129;
            gridViewTextBoxColumn4.HeaderText = "Начало";
            gridViewTextBoxColumn4.Name = "DateBegin";
            gridViewTextBoxColumn4.Width = 96;
            gridViewTextBoxColumn5.HeaderText = "Окончание";
            gridViewTextBoxColumn5.Name = "DateEnd";
            gridViewTextBoxColumn5.Width = 85;
            this.radGridView1.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn1,
            gridViewTextBoxColumn2,
            gridViewTextBoxColumn3,
            gridViewTextBoxColumn4,
            gridViewTextBoxColumn5});
            this.radGridView1.MasterTemplate.EnableCustomSorting = true;
            this.radGridView1.MasterTemplate.EnableGrouping = false;
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ShowRowErrors = false;
            this.radGridView1.Size = new System.Drawing.Size(465, 328);
            this.radGridView1.TabIndex = 9;
            this.radGridView1.Text = "radGridView1";
            this.radGridView1.ThemeName = "Office2013Light";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.radLabel1.Location = new System.Drawing.Point(11, 8);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(132, 19);
            this.radLabel1.TabIndex = 10;
            this.radLabel1.Text = "Класс условий труда";
            this.radLabel1.ThemeName = "Office2013Light";
            // 
            // SpecOcenkaDopTariff
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 370);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton3);
            this.Controls.Add(this.radButton4);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.radGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpecOcenkaDopTariff";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Доп. тарифы для специальной оценки условий труда";
            this.ThemeName = "Office2013Light";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SpecOcenkaDopTariff_FormClosed);
            this.Load += new System.EventHandler(this.SpecOcenkaDopTariff_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton3;
        private Telerik.WinControls.UI.RadButton radButton4;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
    }
}
