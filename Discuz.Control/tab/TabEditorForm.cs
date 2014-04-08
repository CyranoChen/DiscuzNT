using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Discuz.Control
{
    /// <summary>
    /// 属性页编辑窗体
    /// </summary>
    class TabEditorForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button cancel;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button button3;

        private TabControl _tabStrip;
        private System.Windows.Forms.Button delnode;
        private System.Windows.Forms.ToolTip toolTip1;
     
        public TabPageCollection Tabs;


        private void LoadNodes(TabPage oItem, TreeNode oTreeNode)
        {
            oTreeNode.Tag = oItem;

            foreach (TabPage oChild in oItem.Controls)
            {
                TreeNode oChildNode = new TreeNode(oChild.Caption);
                LoadNodes(oChild, oChildNode);

                oTreeNode.Nodes.Add(oChildNode);
            }
        }

        public TabEditorForm(TabControl oTabStrip)
        {
            InitializeComponent();

            _tabStrip = oTabStrip;
            Tabs = oTabStrip.Items;

            // 加载已添加的结点
            foreach (TabPage oRoot in Tabs)
            {
                TreeNode oRootNode = new TreeNode(oRoot.Caption);
                LoadNodes(oRoot, oRootNode);
                treeView1.Nodes.Add(oRootNode);
            }

            treeView1.HideSelection = false;
        }

     
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.save = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.delnode = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(12, 44);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(307, 310);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(336, 1);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(326, 353);
            this.propertyGrid1.TabIndex = 1;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_ValueChanged);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(432, 371);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(106, 25);
            this.save.TabIndex = 2;
            this.save.Text = " 保 存 ";
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(557, 371);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(105, 25);
            this.cancel.TabIndex = 3;
            this.cancel.Text = " 取 消 ";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 34);
            this.button3.TabIndex = 4;
            this.button3.Text = "添加属性页";
            this.toolTip1.SetToolTip(this.button3, "添加属性页");
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // delnode
            // 
            this.delnode.Location = new System.Drawing.Point(130, 1);
            this.delnode.Name = "delnode";
            this.delnode.Size = new System.Drawing.Size(87, 34);
            this.delnode.TabIndex = 6;
            this.delnode.Text = "删除属性页";
            this.toolTip1.SetToolTip(this.delnode, "删除属性页");
            this.delnode.Click += new System.EventHandler(this.delnode_Click);
            // 
            // TabEditorForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(683, 398);
            this.Controls.Add(this.delnode);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.save);
            this.Controls.Add(this.propertyGrid1);
            this.Controls.Add(this.treeView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TabEditorForm";
            this.Text = "DiscuzNT TabPage Designer";
            this.ResumeLayout(false);

        }
        #endregion


        // New root
        private void button3_Click(object sender, System.EventArgs e)
        {
            TabPage oItem = new TabPage();
            oItem.Caption = "新属性页";
   
            Tabs.Add(oItem);

            TreeNode oNewTreeNode = new TreeNode("新属性页");
            oNewTreeNode.Tag = oItem;
            treeView1.Nodes.Add(oNewTreeNode);

            treeView1.SelectedNode = treeView1.Nodes[treeView1.Nodes.Count - 1];
        }

     
     

        // 在结点上单击
        private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            propertyGrid1.SelectedObject = e.Node.Tag;
        }
             

        // 删除结点
        private void delnode_Click(object sender, System.EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                TabPage oItem = (TabPage)treeView1.SelectedNode.Tag;

                _tabStrip.Items.Remove(oItem);
                treeView1.SelectedNode.Remove();
            }
        }

        private void propertyGrid1_ValueChanged(object sender, PropertyValueChangedEventArgs e)
        {
            // 如果已修改 text,则同时更新左侧树
            //if (e.ChangedItem.Label == "Text")
            //{
            //    treeView1.SelectedNode.Text = (string)e.ChangedItem.Value;
            //}

            if (e.ChangedItem.Label == "Caption")
            {
                treeView1.SelectedNode.Text = (string)e.ChangedItem.Value;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

      
    }
}
