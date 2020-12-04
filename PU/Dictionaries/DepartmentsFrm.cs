using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using PU.Models;
using PU.Classes;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace PU.Dictionaries
{
    public partial class DepartmentsFrm : Telerik.WinControls.UI.RadForm
    {
        pu6Entities db = new pu6Entities();
        public long InsID = 0;   // ID страхователя
        public string action;
        public long DepID;
        public string Name { get; set; }


        public DepartmentsFrm()
        {
            InitializeComponent();
            SelfRef = this;
        }

        private void checkAccessLevel()
        {
            long level = Methods.checkUserAccessLevel(this.Name);

            switch (level)
            {
                case 2:
                    addBtn.Enabled = false;
                    delBtn.Enabled = false;
                    break;
                case 3:
                    RadMessageBox.Show("Доступ запрещен!");
                    this.Close();
                    //this.Dispose();
                    break;
            }

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

        public static DepartmentsFrm SelfRef
        {
            get;
            set;
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void dataTree_upd()
        {
            treeView1.Nodes.Clear();
            var b = db.Department.Where(x => x.InsurerID == InsID);

            treeView1.Nodes.Clear();


            TreeNode root = new TreeNode("Структурные подразделения, Отделы");
            root.Tag = 0;
            root.ExpandAll();

            foreach(var item in b.Where(x => x.ParentID == 0 || x.ParentID == null))
            {
                long id = item.ID;
                long? parent_id = item.ParentID;
                string name = item.Code + "  " + item.Name;

                if (parent_id == 0 || parent_id == null)
                {
                    root.Nodes.Add(FillNode(id, name));
                    }
            }
            treeView1.Nodes.Add(root);

            foreach(var item in b.Where(x => x.ParentID != 0 && x.ParentID != null))
               {

                   long id = item.ID;
                   long? parent_id = item.ParentID;
                   string name = item.Code + "  " + item.Name;

                   TreeNode[] treeNodes = treeView1.Nodes.Find(parent_id.ToString(), true);
                   if (treeNodes.Length != 0)
                   (treeNodes.GetValue(0) as TreeNode).Nodes.Add(FillNode(id, name));
               }



        }

        /// <summary>
        /// Создание новой ноды
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>TreeNode</returns>
        private TreeNode FillNode(long id, string name)
        {
            TreeNode treeNode = new TreeNode();
            treeNode.Tag = id;
            treeNode.Name = id.ToString();
            treeNode.Text = name;
            return treeNode;
        }
        private void DepartmentsFrm_Load(object sender, EventArgs e)
        {
            ThemeResolutionService.ApplyThemeToControlTree(this, this.ThemeName);
            Telerik.WinControls.RadMessageBox.SetThemeName(this.ThemeName);

            checkAccessLevel();

            if (InsID == 0)
                InsID = Options.InsID;

            dataTree_upd();
            if (action == "selection")
            {
                selectBtn.Visible = true;
                TreeNode[] tn = treeView1.Nodes.Find(DepID.ToString(), true);
                foreach (var item in tn)
                {
                    item.Expand();
                    treeView1.SelectedNode = item;
                    break;
                }
            }
        }


        private void radButton1_Click(object sender, EventArgs e)
        {
            if (!addBtn.Enabled)
                return;

            TreeNode SelNode = treeView1.SelectedNode;

            DepartmentsEdit child = new DepartmentsEdit();
            child.Owner = this;
            child.action = "add";
            child.ThemeName = this.ThemeName;
            child.InsID = InsID;
            child.ParId = SelNode == null ? 0 : long.Parse(SelNode.Tag.ToString());
            child.ShowInTaskbar = false;
            child.ShowDialog();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            TreeNode SelNode = treeView1.SelectedNode;

            if (SelNode != null && SelNode.Tag.ToString() != "0")
            {
                long ID = long.Parse(SelNode.Tag.ToString());
                Department dep = db.Department.FirstOrDefault(x => x.ID == ID);
                DepartmentsEdit child = new DepartmentsEdit();
                child.Owner = this;
                child.ThemeName = this.ThemeName;
                child.radTextBox1.Text = dep.Code;
                child.radTextBox2.Text = dep.Name;
                child.ParId = long.Parse(SelNode.Tag.ToString());
                child.action = "edit";
                child.ShowInTaskbar = false;
                child.ShowDialog();
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для редактирования!", "Ошибка");
            }
        }

        public string add(Department dep)  // добавление записи
        {
            string result = "";
            string nodeindex = treeView1.SelectedNode != null ? treeView1.SelectedNode.Tag.ToString() : "0";

            try
            {
                if (!db.Department.Any(x => x.Code == dep.Code && x.Name == dep.Name && x.ParentID == dep.ParentID))
                {

                    db.Department.Add(dep);

                    db.SaveChanges();
                    dataTree_upd();
                    TreeNode[] tn = treeView1.Nodes.Find(nodeindex, true);
                    foreach (var item in tn)
                    {
                        item.Expand();
                        treeView1.SelectedNode = item;
                        break;
                    }

                }
                else
                {
                    result = "Указанное подразделение уже существует";
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        public string edit(Department dep)  // редактирование записи
        {
            string result = "";

            string nodeindex = treeView1.SelectedNode.Tag.ToString();
            long ID = Convert.ToInt64(nodeindex);

            try
            {
                if (!db.Department.Any(x => x.Code == dep.Code && x.Name == dep.Name && x.ParentID == dep.ParentID && x.ID != ID))
                {
                    Department Item = db.Department.FirstOrDefault(x => x.ID == ID);

                    Item.Code = dep.Code;
                    Item.Name = dep.Name;

                    db.Entry(Item).State = EntityState.Modified;
                    db.SaveChanges();
                    dataTree_upd();
                    TreeNode[] tn = treeView1.Nodes.Find(nodeindex, true);
                    foreach (var item in tn)
                    {
                        item.Expand();
                        treeView1.SelectedNode = item;
                        break;
                    }
                }
                else
                {
                    result = "Указанное подразделение уже существует";
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        private List<long> findChildNodes(TreeNode node)
        { 
            List<long> list = new List<long>();
            list.Add(long.Parse(node.Tag.ToString()));
            foreach (TreeNode item in node.Nodes)
            {
                list.Add(long.Parse(item.Tag.ToString()));

                if (node.Nodes.Count > 0)
                {
                    this.findChildNodes(item);
                }
            }

            return list;
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            if (!delBtn.Enabled)
                return;

            TreeNode SelNode = treeView1.SelectedNode;

            // Удаление текущей записи
            if (SelNode != null && SelNode.Tag.ToString() != "0")
            {
                string title = "";
                long id = Convert.ToInt64(SelNode.Tag.ToString());
                DialogResult dialogResult = RadMessageBox.Show(this, "Уверены что хотите удалить текущую запись?", title, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        List<long> ItemList = findChildNodes(SelNode);
                        var deps = db.Department.Where(x => ItemList.Contains(x.ID));

                        foreach (var item in deps)
                        {
                            db.Department.Remove(item);
                        }

                    }
                    catch(Exception ex)
                    {
                        RadMessageBox.Show(this, "При удалении данных произошла ошибка! " + ex, "Ошибка", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                    finally {
                        db.SaveChanges();
                        dataTree_upd();
                    }
                }
            }
            else
            {
                RadMessageBox.Show(this, "Нет данных для удаления!", "");
            }


        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            switch (action)
            {
                case "selection":
                    selectBtn_Click(null, null);
                    break;
                default:
                    radButton2_Click(null, null);
                    break;
            }


        }

        private void selectBtn_Click(object sender, EventArgs e)
        {
            string nodeindex = treeView1.SelectedNode.Tag.ToString();
            DepID = Convert.ToInt64(nodeindex);
            if (db.Department.Any(x => x.ID == DepID))
                Name = db.Department.First(x => x.ID == DepID).Name;
            this.Close();
        }



    }
}
