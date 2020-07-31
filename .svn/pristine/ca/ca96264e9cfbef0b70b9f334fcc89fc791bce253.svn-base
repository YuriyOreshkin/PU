using PU.Classes;
using PU.UserAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Localization;
using System.Linq;
using System.Reflection;
using Telerik.WinControls.UI;

namespace PU.Service
{
    public partial class UserToObjects : Telerik.WinControls.UI.RadForm
    {
        xaccessEntities xaccessDB = new xaccessEntities();
        public long userID = 0;

        List<UsersAccessLevelToObjects> UALO = new List<UsersAccessLevelToObjects>();

        public UserToObjects()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Перехват нажатия на ESC для закрытия формы
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void UserToObjects_Load(object sender, EventArgs e)
        {
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);
            RadGridLocalizationProvider.CurrentProvider = new MyRussianRadGridLocalizationProvider();
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);

            List<UsersAccessLevelToObjects> UALO_from_db = xaccessDB.UsersAccessLevelToObjects.Where(x => x.UsersID == userID).ToList();


            foreach (var item in xaccessDB.AccessObject)
            {
                long levelID = 1;

                if (UALO_from_db.Any(x => x.AccessObjectID == item.ID))
                {
                    levelID = UALO_from_db.First(x => x.AccessObjectID == item.ID).AccessLevelID.Value;
                }

                UALO.Add(new UsersAccessLevelToObjects { AccessLevelID = levelID, AccessObjectID = item.ID, UsersID = userID});
            }


            objectCatDDL.DataSource = null;
            objectCatDDL.DisplayMember = "Name";
            objectCatDDL.ValueMember = "ID";
            objectCatDDL.ShowItemToolTips = true;
            objectCatDDL.DataSource = xaccessDB.AccessCategory; ;
            objectCatDDL.SelectedIndexChanged += (s, с) => objectCatDDL_SelectedIndexChanged();
            objectCatDDL.SelectedIndex = 0;
            objectCatDDL_SelectedIndexChanged();
        }

        private void objectCatDDL_SelectedIndexChanged()
        {
            if (!String.IsNullOrEmpty(objectCatDDL.Text) && objectCatDDL.SelectedItem != null)
            {
                long id = (long)objectCatDDL.SelectedValue;

                objectsGridView.DataSource = null;
                objectsGridView.Rows.Clear();

                objectsGridView.DataSource = xaccessDB.AccessObject.Where(x => x.AccessCategoryID == id).ToList();

                foreach (var col in objectsGridView.Columns)
                {
                    if (col.Name == "Name")
                    {
                        col.HeaderText = "Категория";
                    }
                    else
                    {
                        col.IsVisible = false;
                    }
                }

            }

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void objectsGridView_CurrentRowChanged(object sender, CurrentRowChangedEventArgs e)
        {
            updateAccessLevel();
        }

        private void updateAccessLevel()
        {
            if (objectsGridView.RowCount > 0 && objectsGridView.CurrentRow != null && objectsGridView.CurrentRow.Cells["ID"].Value != null)
            {
                long id = (long)objectsGridView.CurrentRow.Cells["ID"].Value;


                long level = 1;

                if (UALO.Any(x => x.AccessObjectID.Value == id))
                {
                    level = UALO.First(x => x.AccessObjectID.Value == id).AccessLevelID.Value;
                }

                switch (level)
                {
                    case 1:
                        fullRadBtn.IsChecked = true;
                        break;
                    case 2:
                        ViewRadBtn.IsChecked = true;
                        break;
                    case 3:
                        DenyRadBtn.IsChecked = true;
                        break;
                }


            }
        }

        private void fullRadBtn_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            UpdateUALO(1);
        }

        private void ViewRadBtn_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            UpdateUALO(2);
        }

        private void DenyRadBtn_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            UpdateUALO(3);
        }

        private void UpdateUALO(long levelID)
        {
            if (objectsGridView.RowCount > 0 && objectsGridView.CurrentRow != null && objectsGridView.CurrentRow.Cells["ID"].Value != null)
            {
                long id = (long)objectsGridView.CurrentRow.Cells["ID"].Value;

                var item = UALO.First(x => x.AccessObjectID == id);
                item.AccessLevelID = levelID;

            }

        }

        private void fullAllBtn_Click(object sender, EventArgs e)
        {
            UpdateUALO_ALL(1);
        }

        private void ViewAllBtn_Click(object sender, EventArgs e)
        {
            UpdateUALO_ALL(2);
        }

        private void DenyAllBtn_Click(object sender, EventArgs e)
        {
            UpdateUALO_ALL(3);
        }

        private void UpdateUALO_ALL(long levelID)
        {
            foreach (var item in UALO)
            {
                item.AccessLevelID = levelID;
            }

            updateAccessLevel();
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            var UALO_from_db = xaccessDB.UsersAccessLevelToObjects.Where(x => x.UsersID == userID);

            foreach (var item in UALO)
            {
                if (UALO_from_db.Any(x => x.AccessObjectID.Value == item.AccessObjectID.Value)) // если в базе уже есть информация по доступу к этому объекту, то проверяем права доступа к нему
                {
                    var itemFromDB = UALO_from_db.First(x => x.AccessObjectID.Value == item.AccessObjectID.Value);

                    if (itemFromDB.AccessLevelID.Value != item.AccessLevelID.Value)  // если права доступа отличаются от тех что забиты в базу, то обновляем запись с новыми правами
                    {
                        itemFromDB.AccessLevelID = item.AccessLevelID.Value;

                        xaccessDB.ObjectStateManager.ChangeObjectState(itemFromDB, EntityState.Modified);
                    }


                }
                else  // если такого объекта еще нет то добавляем его
                {
                    xaccessDB.AddToUsersAccessLevelToObjects(item);
                }
            }

            try
            {
                xaccessDB.SaveChanges();
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Во время сохранения произошла ошибка. Код ошибки - " + ex.Message);
            }

            this.Close();

        }



    }
}
