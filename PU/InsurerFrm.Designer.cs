namespace PU
{
    partial class InsurerFrm
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            this.radButton3 = new Telerik.WinControls.UI.RadButton();
            this.radButton4 = new Telerik.WinControls.UI.RadButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radButton5 = new Telerik.WinControls.UI.RadButton();
            this.radButton6 = new Telerik.WinControls.UI.RadButton();
            this.radButton7 = new Telerik.WinControls.UI.RadButton();
            this.btnPrint = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radButton2
            // 
            this.radButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton2.Location = new System.Drawing.Point(803, 302);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(105, 24);
            this.radButton2.TabIndex = 1;
            this.radButton2.Text = "Изменить";
            this.radButton2.ThemeName = "Office2013Light";
            this.radButton2.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // radButton3
            // 
            this.radButton3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton3.Location = new System.Drawing.Point(803, 332);
            this.radButton3.Name = "radButton3";
            this.radButton3.Size = new System.Drawing.Size(105, 24);
            this.radButton3.TabIndex = 2;
            this.radButton3.Text = "Удалить";
            this.radButton3.ThemeName = "Office2013Light";
            this.radButton3.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // radButton4
            // 
            this.radButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton4.Location = new System.Drawing.Point(803, 399);
            this.radButton4.Name = "radButton4";
            this.radButton4.Size = new System.Drawing.Size(105, 24);
            this.radButton4.TabIndex = 3;
            this.radButton4.Text = "Закрыть";
            this.radButton4.ThemeName = "Office2013Light";
            this.radButton4.Click += new System.EventHandler(this.radButton4_Click);
            // 
            // radButton1
            // 
            this.radButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton1.Location = new System.Drawing.Point(803, 272);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(105, 24);
            this.radButton1.TabIndex = 0;
            this.radButton1.Text = "Добавить";
            this.radButton1.ThemeName = "Office2013Light";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radGridView1
            // 
            this.radGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radGridView1.Location = new System.Drawing.Point(11, 12);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AllowAddNewRow = false;
            this.radGridView1.MasterTemplate.AllowColumnHeaderContextMenu = false;
            this.radGridView1.MasterTemplate.AllowColumnReorder = false;
            this.radGridView1.MasterTemplate.AllowDeleteRow = false;
            this.radGridView1.MasterTemplate.AllowDragToGroup = false;
            this.radGridView1.MasterTemplate.EnableFiltering = true;
            this.radGridView1.MasterTemplate.EnableGrouping = false;
            this.radGridView1.MasterTemplate.ShowRowHeaderColumn = false;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ShowRowErrors = false;
            this.radGridView1.Size = new System.Drawing.Size(786, 411);
            this.radGridView1.TabIndex = 4;
            this.radGridView1.ThemeName = "Office2013Light";
            this.radGridView1.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView1_CellDoubleClick);
            this.radGridView1.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.radGridView1_ContextMenuOpening);
            this.radGridView1.FilterChanged += new Telerik.WinControls.UI.GridViewCollectionChangedEventHandler(this.radGridView1_FilterChanged);
            this.radGridView1.SizeChanged += new System.EventHandler(this.radGridView1_SizeChanged);
            this.radGridView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.radGridView1_KeyPress);
            // 
            // radButton5
            // 
            this.radButton5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton5.Location = new System.Drawing.Point(803, 68);
            this.radButton5.Name = "radButton5";
            this.radButton5.Size = new System.Drawing.Size(105, 24);
            this.radButton5.TabIndex = 5;
            this.radButton5.Text = "Отделы";
            this.radButton5.ThemeName = "Office2013Light";
            this.radButton5.Click += new System.EventHandler(this.radButton5_Click);
            // 
            // radButton6
            // 
            this.radButton6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton6.Location = new System.Drawing.Point(803, 120);
            this.radButton6.Name = "radButton6";
            this.radButton6.Size = new System.Drawing.Size(105, 24);
            this.radButton6.TabIndex = 6;
            this.radButton6.Text = "Сотрудники";
            this.radButton6.ThemeName = "Office2013Light";
            this.radButton6.Click += new System.EventHandler(this.radButton6_Click);
            // 
            // radButton7
            // 
            this.radButton7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radButton7.Location = new System.Drawing.Point(803, 182);
            this.radButton7.Name = "radButton7";
            this.radButton7.Size = new System.Drawing.Size(105, 24);
            this.radButton7.TabIndex = 7;
            this.radButton7.Text = "Выбрать";
            this.radButton7.ThemeName = "Office2013Light";
            this.radButton7.Visible = false;
            this.radButton7.Click += new System.EventHandler(this.radButton7_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(803, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(105, 24);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "Печать";
            this.btnPrint.ThemeName = "Office2013Light";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // InsurerFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 433);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.radButton7);
            this.Controls.Add(this.radButton6);
            this.Controls.Add(this.radButton5);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton3);
            this.Controls.Add(this.radButton4);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.radGridView1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 350);
            this.Name = "InsurerFrm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.RootElement.MaxSize = new System.Drawing.Size(0, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Страхователи / Работодатели";
            this.ThemeName = "Office2013Light";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.InsurerFrm_FormClosed);
            this.Load += new System.EventHandler(this.Insurer_Load);
            this.Shown += new System.EventHandler(this.InsurerFrm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButton2;
        private Telerik.WinControls.UI.RadButton radButton3;
        private Telerik.WinControls.UI.RadButton radButton4;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadButton radButton5;
        private Telerik.WinControls.UI.RadButton radButton6;
        private Telerik.WinControls.UI.RadButton radButton7;
        private Telerik.WinControls.UI.RadButton btnPrint;
    }
}
