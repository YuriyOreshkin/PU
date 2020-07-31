namespace PU.FormsRSW2014
{
    partial class DolgnFrm
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
            this.btnSelection = new Telerik.WinControls.UI.RadButton();
            this.radButton4 = new Telerik.WinControls.UI.RadButton();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.btnSelection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelection
            // 
            this.btnSelection.Location = new System.Drawing.Point(379, 12);
            this.btnSelection.Name = "btnSelection";
            this.btnSelection.Size = new System.Drawing.Size(90, 24);
            this.btnSelection.TabIndex = 14;
            this.btnSelection.Text = "Выбрать";
            this.btnSelection.ThemeName = "Office2013Light";
            this.btnSelection.Visible = false;
            this.btnSelection.Click += new System.EventHandler(this.btnSelection_Click);
            // 
            // radButton4
            // 
            this.radButton4.Location = new System.Drawing.Point(379, 342);
            this.radButton4.Name = "radButton4";
            this.radButton4.Size = new System.Drawing.Size(90, 24);
            this.radButton4.TabIndex = 12;
            this.radButton4.Text = "Закрыть";
            this.radButton4.ThemeName = "Office2013Light";
            this.radButton4.Click += new System.EventHandler(this.radButton4_Click);
            // 
            // radGridView1
            // 
            this.radGridView1.Location = new System.Drawing.Point(12, 12);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridView1.MasterTemplate.EnableFiltering = true;
            this.radGridView1.MasterTemplate.EnableGrouping = false;
            this.radGridView1.MasterTemplate.ShowHeaderCellButtons = true;
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.ShowHeaderCellButtons = true;
            this.radGridView1.ShowRowErrors = false;
            this.radGridView1.Size = new System.Drawing.Size(361, 354);
            this.radGridView1.TabIndex = 13;
            this.radGridView1.ThemeName = "Office2013Light";
            this.radGridView1.UserAddedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.radGridView1_UserAddedRow);
            this.radGridView1.UserDeletedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.radGridView1_UserDeletedRow);
            this.radGridView1.CellDoubleClick += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView1_CellDoubleClick);
            this.radGridView1.CellValueChanged += new Telerik.WinControls.UI.GridViewCellEventHandler(this.radGridView1_CellValueChanged);
            this.radGridView1.ContextMenuOpening += new Telerik.WinControls.UI.ContextMenuOpeningEventHandler(this.radGridView1_ContextMenuOpening);
            // 
            // DolgnFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 378);
            this.Controls.Add(this.btnSelection);
            this.Controls.Add(this.radButton4);
            this.Controls.Add(this.radGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DolgnFrm";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Справочник профессий и должностей";
            this.ThemeName = "Office2013Light";
            this.Load += new System.EventHandler(this.DolgnFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnSelection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton radButton4;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        public Telerik.WinControls.UI.RadButton btnSelection;
    }
}
