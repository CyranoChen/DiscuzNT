using System;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Xml;

namespace Discuz.Web.Admin
{
    public partial class manageshortcutmenu : AdminPage
	{
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            if (!IsPostBack)
            {
                BindDataGrid();
            }
        }

        private void BindDataGrid()
        {
            DataGrid1.TableHeaderName = "快捷菜单管理";
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/mainmenu");
            XmlNodeList toptabmenus = doc.SelectNodes("/dataset/toptabmenu");
            string[] menuText = new string[mainmenus.Count];
            for (int i = 0; i < menuText.Length; i++)
            {
                foreach (XmlNode topmenuitem in toptabmenus)
                {
                    if (("," + topmenuitem["mainmenulist"].InnerText + ",").IndexOf("," + mainmenus[i].SelectSingleNode("id").InnerText + ",") != -1)
                    {
                        menuText[i] = topmenuitem["title"].InnerText + "->" + mainmenus[i].SelectSingleNode("menutitle").InnerText;
                        break;
                    }
                }
            }
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("id"));
            dt.Columns.Add(new DataColumn("local"));
            XmlNodeList shortcutmenus = doc.SelectNodes("/dataset/shortcut");
            XmlNodeInnerTextVisitor shortcutmenuvisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode shortcutmenu in shortcutmenus)
            {
                shortcutmenuvisitor.SetNode(shortcutmenu);
                DataRow dr = dt.NewRow();
                dr["id"] = shortcutmenuvisitor["link"];
                dr["local"] = menuText[int.Parse(shortcutmenuvisitor["showmenuid"]) - 1] + "->" + shortcutmenuvisitor["menutitle"];
                dt.Rows.Add(dr);
            }
            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = "";
                dr["local"] = "(暂无收藏)";
                dt.Rows.Add(dr);
            }
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
        }

        protected void DataGrid1_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
        {
            int row = e.Item.ItemIndex;
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList shortcutmenus = doc.SelectNodes("/dataset/shortcut");
            int i = 0;
            foreach (XmlNode xn in shortcutmenus)
            {
                if(i == row)
                    xn.ParentNode.RemoveChild(xn);
                i++;
            }
            doc.Save(configPath);
            MenuManage.CreateMenuJson();
            base.RegisterStartupScript("delete", "<script type='text/javascript'>window.parent.LoadShortcutMenu();</script>");
            BindDataGrid();
        }

        protected void DataGrid1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.Cells[0].Text == "(暂无收藏)")
            {
                e.Item.Cells[1].Controls.Remove(e.Item.Cells[1].Controls[0]);
            }
        }
    }
}
