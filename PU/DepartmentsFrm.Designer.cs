namespace PU.Models
{
    partial class DepartmentsFrm
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
            this.editBtn = new Telerik.WinControls.UI.RadButton();
            this.delBtn = new Telerik.WinControls.UI.RadButton();
            this.closeBtn = new Telerik.WinControls.UI.RadButton();
            this.addBtn = new Telerik.WinControls.UI.RadButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.selectBtn = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // editBtn
            // 
            this.editBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.editBtn.Location = new System.Drawing.Point(503, 229);
            this.editBtn.Name = "editBtn";
            this.editBtn.Size = new System.Drawing.Size(90, 24);
            this.editBtn.TabIndex = 3;
            this.editBtn.Text = "Изменить";
            this.editBtn.ThemeName = "Office2013Light";
            this.editBtn.Click += new System.EventHandler(this.radButton2_Click);
            // 
            // delBtn
            // 
            this.delBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.delBtn.Location = new System.Drawing.Point(503, 259);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(90, 24);
            this.delBtn.TabIndex = 4;
            this.delBtn.Text = "Удалить";
            this.delBtn.ThemeName = "Office2013Light";
            this.delBtn.Click += new System.EventHandler(this.radButton3_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.Location = new System.Drawing.Point(503, 326);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(90, 24);
            this.closeBtn.TabIndex = 5;
            this.closeBtn.Text = "Закрыть";
            this.closeBtn.ThemeName = "Office2013Light";
            this.closeBtn.Click += new System.EventHandler(this.radButton4_Click);
            // 
            // addBtn
            // 
            this.addBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addBtn.Location = new System.Drawing.Point(503, 199);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(90, 24);
            this.addBtn.TabIndex = 2;
            this.addBtn.Text = "Добавить";
            this.addBtn.ThemeName = "Office2013Light";
            this.addBtn.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(474, 338);
            this.treeView1.TabIndex = 0;
            this.treeView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDoubleClick);
            // 
            // selectBtn
            // 
            this.selectBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.selectBtn.Location = new System.Drawing.Point(503, 74);
            this.selectBtn.Name = "selectBtn";
            this.selectBtn.Size = new System.Drawing.Size(90, 24);
            this.selectBtn.TabIndex = 1;
            this.selectBtn.Text = "Выбрать";
            this.selectBtn.ThemeName = "Office2013Light";
            this.selectBtn.Visible = false;
            this.selectBtn.Click += new System.EventHandler(this.selectBtn_Click);
            // 
            // DepartmentsFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(605, 362);
            this.Controls.Add(this.selectBtn);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.editBtn);
            this.Controls.Add(this.delBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.addBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "DepartmentsFrm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Подразделения";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.DepartmentsFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.editBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.delBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.addBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton editBtn;
        private Telerik.WinControls.UI.RadButton delBtn;
        private Telerik.WinControls.UI.RadButton closeBtn;
        private Telerik.WinControls.UI.RadButton addBtn;
        internal System.Windows.Forms.TreeView treeView1;
        private Telerik.WinControls.UI.RadButton selectBtn;
    }
}
