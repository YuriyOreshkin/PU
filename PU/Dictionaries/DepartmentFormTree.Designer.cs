namespace PU.Dictionaries
{
    public partial class DepartmentFormTree
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
            this.radGridViewBaseDictionary = new Telerik.WinControls.UI.RadGridView();
            this.commandBarRowElementBaseDictionary = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElementBaseDictionary = new Telerik.WinControls.UI.CommandBarStripElement();
            this.radCommandBarBaseDictionary = new Telerik.WinControls.UI.RadCommandBar();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewBaseDictionary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewBaseDictionary.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBarBaseDictionary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGridViewBaseDictionary
            // 
            this.radGridViewBaseDictionary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewBaseDictionary.Location = new System.Drawing.Point(0, 55);
            // 
            // 
            // 
            this.radGridViewBaseDictionary.MasterTemplate.AllowAddNewRow = false;
            this.radGridViewBaseDictionary.MasterTemplate.AllowDeleteRow = false;
            this.radGridViewBaseDictionary.MasterTemplate.AllowEditRow = false;
            this.radGridViewBaseDictionary.MasterTemplate.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            this.radGridViewBaseDictionary.MasterTemplate.EnableFiltering = true;
            this.radGridViewBaseDictionary.MasterTemplate.EnableGrouping = false;
            this.radGridViewBaseDictionary.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridViewBaseDictionary.Name = "radGridViewBaseDictionary";
            this.radGridViewBaseDictionary.Size = new System.Drawing.Size(710, 320);
            this.radGridViewBaseDictionary.TabIndex = 12;
            this.radGridViewBaseDictionary.Tag = "BaseDictionaryGrid";
            this.radGridViewBaseDictionary.CreateRow += new Telerik.WinControls.UI.GridViewCreateRowEventHandler(this.radGridViewBaseDictionary_CreateRow);
            // 
            // commandBarRowElementBaseDictionary
            // 
            this.commandBarRowElementBaseDictionary.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarRowElementBaseDictionary.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElementBaseDictionary.Name = "commandBarRowElementBaseDictionary";
            this.commandBarRowElementBaseDictionary.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElementBaseDictionary});
            this.commandBarRowElementBaseDictionary.Text = "";
            this.commandBarRowElementBaseDictionary.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarRowElementBaseDictionary.UseCompatibleTextRendering = false;
            // 
            // commandBarStripElementBaseDictionary
            // 
            this.commandBarStripElementBaseDictionary.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarStripElementBaseDictionary.DisplayName = "commandBarStripElementBaseDictionary";
            this.commandBarStripElementBaseDictionary.Font = new System.Drawing.Font("Segoe UI", 9F);
            // 
            // 
            // 
            this.commandBarStripElementBaseDictionary.Grip.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarStripElementBaseDictionary.Name = "commandBarStripElementBaseDictionary";
            // 
            // 
            // 
            this.commandBarStripElementBaseDictionary.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.commandBarStripElementBaseDictionary.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarStripElementBaseDictionary.UseCompatibleTextRendering = false;
            ((Telerik.WinControls.UI.RadCommandBarGrip)(this.commandBarStripElementBaseDictionary.GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.commandBarStripElementBaseDictionary.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // radCommandBarBaseDictionary
            // 
            this.radCommandBarBaseDictionary.Dock = System.Windows.Forms.DockStyle.Top;
            this.radCommandBarBaseDictionary.Location = new System.Drawing.Point(0, 0);
            this.radCommandBarBaseDictionary.Name = "radCommandBarBaseDictionary";
            this.radCommandBarBaseDictionary.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElementBaseDictionary});
            this.radCommandBarBaseDictionary.Size = new System.Drawing.Size(710, 55);
            this.radCommandBarBaseDictionary.TabIndex = 11;
            this.radCommandBarBaseDictionary.Tag = "BaseDictionaryActions";
            // 
            // DepartmentFormTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 375);
            this.Controls.Add(this.radGridViewBaseDictionary);
            this.Controls.Add(this.radCommandBarBaseDictionary);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DepartmentFormTree";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.ThemeName = "Office2013Light";
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewBaseDictionary.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewBaseDictionary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBarBaseDictionary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadGridView radGridViewBaseDictionary;
       
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElementBaseDictionary;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElementBaseDictionary;
        private Telerik.WinControls.UI.RadCommandBar radCommandBarBaseDictionary;
    }
}
