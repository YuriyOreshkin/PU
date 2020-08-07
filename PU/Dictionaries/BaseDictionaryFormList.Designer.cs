namespace PU.Dictionaries
{
    public partial class BaseDictionaryFormList
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
            this.radCommandBarBaseDictionary = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElementBaseDictionary = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElementBaseDictionary = new Telerik.WinControls.UI.CommandBarStripElement();
            this.commandBarButtonAdd = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButtonEdit = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarButtonDelete = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButtonSynchronization = new Telerik.WinControls.UI.CommandBarButton();
            this.commandBarSeparator2 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.commandBarButtonClose = new Telerik.WinControls.UI.CommandBarButton();
            this.radGridViewBaseDictionary = new Telerik.WinControls.UI.RadGridView();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBarBaseDictionary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewBaseDictionary)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewBaseDictionary.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
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
            this.radCommandBarBaseDictionary.Tag = "Actions";
            // 
            // commandBarRowElementBaseDictionary
            // 
            this.commandBarRowElementBaseDictionary.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElementBaseDictionary.Name = "commandBarRowElementBaseDictionary";
            this.commandBarRowElementBaseDictionary.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElementBaseDictionary});
            // 
            // commandBarStripElementBaseDictionary
            // 
            this.commandBarStripElementBaseDictionary.DisplayName = "commandBarStripElementBaseDictionary";
            this.commandBarStripElementBaseDictionary.Font = new System.Drawing.Font("Segoe UI", 9F);
            // 
            // 
            // 
            this.commandBarStripElementBaseDictionary.Grip.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.commandBarStripElementBaseDictionary.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.commandBarButtonAdd,
            this.commandBarButtonEdit,
            this.commandBarButtonDelete,
            this.commandBarSeparator1,
            this.commandBarButtonSynchronization,
            this.commandBarSeparator2,
            this.commandBarButtonClose});
            this.commandBarStripElementBaseDictionary.Name = "commandBarStripElementBaseDictionary";
            // 
            // 
            // 
            this.commandBarStripElementBaseDictionary.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            ((Telerik.WinControls.UI.RadCommandBarGrip)(this.commandBarStripElementBaseDictionary.GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Visible;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.commandBarStripElementBaseDictionary.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            // 
            // commandBarButtonAdd
            // 
            this.commandBarButtonAdd.DisplayName = "commandBarButtonAdd";
            this.commandBarButtonAdd.DrawBorder = true;
            this.commandBarButtonAdd.DrawText = true;
            this.commandBarButtonAdd.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.commandBarButtonAdd.Image = null;
            this.commandBarButtonAdd.Name = "commandBarButtonAdd";
            this.commandBarButtonAdd.Tag = "Add";
            this.commandBarButtonAdd.Text = "Добавить";
            this.commandBarButtonAdd.ToolTipText = "Добавить";
            // 
            // commandBarButtonEdit
            // 
            this.commandBarButtonEdit.DisplayName = "commandBarButtonEdit";
            this.commandBarButtonEdit.DrawText = true;
            this.commandBarButtonEdit.Image = null;
            this.commandBarButtonEdit.Name = "commandBarButtonEdit";
            this.commandBarButtonEdit.Tag = "Edit";
            this.commandBarButtonEdit.Text = "Изменить";
            this.commandBarButtonEdit.ToolTipText = "Изменить";
            // 
            // commandBarButtonDelete
            // 
            this.commandBarButtonDelete.DisplayName = "commandBarButtonDelete";
            this.commandBarButtonDelete.DrawText = true;
            this.commandBarButtonDelete.Image = null;
            this.commandBarButtonDelete.Name = "commandBarButtonDelete";
            this.commandBarButtonDelete.Tag = "Delete";
            this.commandBarButtonDelete.Text = "Удалить";
            this.commandBarButtonDelete.ToolTipText = "Удалить";
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.DisplayName = "commandBarSeparator1";
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // commandBarButtonSynchronization
            // 
            this.commandBarButtonSynchronization.DisplayName = "commandBarButton1";
            this.commandBarButtonSynchronization.DrawText = true;
            this.commandBarButtonSynchronization.Image = null;
            this.commandBarButtonSynchronization.Name = "commandBarButtonSynchronization";
            this.commandBarButtonSynchronization.Tag = "Synchronization";
            this.commandBarButtonSynchronization.Text = "Синхронизация";
            this.commandBarButtonSynchronization.ToolTipText = "Синхронизация";
            // 
            // commandBarSeparator2
            // 
            this.commandBarSeparator2.DisplayName = "commandBarSeparator2";
            this.commandBarSeparator2.Name = "commandBarSeparator2";
            this.commandBarSeparator2.VisibleInOverflowMenu = false;
            // 
            // commandBarButtonClose
            // 
            this.commandBarButtonClose.DisplayName = "commandBarButtonClose";
            this.commandBarButtonClose.DrawText = true;
            this.commandBarButtonClose.Image = null;
            this.commandBarButtonClose.Name = "commandBarButtonClose";
            this.commandBarButtonClose.Tag = "Close";
            this.commandBarButtonClose.Text = "Закрыть";
            this.commandBarButtonClose.ToolTipText = "Закрыть";
            // 
            // radGridViewBaseDictionary
            // 
            this.radGridViewBaseDictionary.AutoSizeRows = true;
            this.radGridViewBaseDictionary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridViewBaseDictionary.Location = new System.Drawing.Point(0, 30);
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
            this.radGridViewBaseDictionary.Size = new System.Drawing.Size(710, 345);
            this.radGridViewBaseDictionary.TabIndex = 12;
            // 
            // BaseDictionaryFormList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 375);
            this.Controls.Add(this.radGridViewBaseDictionary);
            this.Controls.Add(this.radCommandBarBaseDictionary);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BaseDictionaryFormList";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.ThemeName = "Office2013Light";
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBarBaseDictionary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewBaseDictionary.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridViewBaseDictionary)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadCommandBar radCommandBarBaseDictionary;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElementBaseDictionary;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElementBaseDictionary;
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonAdd;
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonEdit;
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonDelete;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonSynchronization;
        private Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator2;
        private Telerik.WinControls.UI.CommandBarButton commandBarButtonClose;
        private Telerik.WinControls.UI.RadGridView radGridViewBaseDictionary;
    }
}
