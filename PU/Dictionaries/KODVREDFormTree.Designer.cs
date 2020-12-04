namespace PU.Dictionaries
{
    public partial class KODVREDFormTree
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
            this.commandBarRowElementBaseDictionary = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElementBaseDictionary = new Telerik.WinControls.UI.CommandBarStripElement();
            this.radCommandBarBaseDictionary = new Telerik.WinControls.UI.RadCommandBar();
            this.radTreeViewKODVRED = new Telerik.WinControls.UI.RadTreeView();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBarBaseDictionary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeViewKODVRED)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
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
            this.radCommandBarBaseDictionary.Size = new System.Drawing.Size(710, 30);
            this.radCommandBarBaseDictionary.TabIndex = 11;
            this.radCommandBarBaseDictionary.Tag = "BaseDictionaryActions";
            // 
            // radTreeViewKODVRED
            // 
            this.radTreeViewKODVRED.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTreeViewKODVRED.Location = new System.Drawing.Point(0, 30);
            this.radTreeViewKODVRED.Name = "radTreeViewKODVRED";
            this.radTreeViewKODVRED.Size = new System.Drawing.Size(710, 345);
            this.radTreeViewKODVRED.SpacingBetweenNodes = -1;
            this.radTreeViewKODVRED.TabIndex = 12;
            // 
            // KODVREDFormTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 375);
            this.Controls.Add(this.radTreeViewKODVRED);
            this.Controls.Add(this.radCommandBarBaseDictionary);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KODVREDFormTree";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.ThemeName = "Office2013Light";
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBarBaseDictionary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeViewKODVRED)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
       
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElementBaseDictionary;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElementBaseDictionary;
        private Telerik.WinControls.UI.RadCommandBar radCommandBarBaseDictionary;
        private Telerik.WinControls.UI.RadTreeView radTreeViewKODVRED;
    }
}
