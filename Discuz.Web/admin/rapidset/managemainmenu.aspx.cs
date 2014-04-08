using System;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;
using Discuz.Common.Xml;

namespace Discuz.Web.Admin
{
    public partial class managemainmenu : AdminPage
	{
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            string menuid = DNTRequest.GetString("menuid");
            string mode = DNTRequest.GetString("mode");
            if (mode != "")
            {
                if (mode == "del")
                {
                    MenuManage.DeleteMainMenu(int.Parse(menuid));
                }
                else
                {
                    if (menuid == "0")
                    {
                        MenuManage.NewMainMenu(DNTRequest.GetString("menutitle"), DNTRequest.GetString("defaulturl"));
                    }
                    else
                    {
                        MenuManage.EditMainMenu(int.Parse(menuid), DNTRequest.GetString("menutitle"), DNTRequest.GetString("defaulturl"));
                    }
                }
                Response.Redirect("managemainmenu.aspx", true);
            }
            else
            {
                BindDataGrid();
            }
        }

        private void BindDataGrid()
        {
            DataGrid1.TableHeaderName = "菜单管理";
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("id"));
            dt.Columns.Add(new DataColumn("title"));
            dt.Columns.Add(new DataColumn("defaulturl"));
            dt.Columns.Add(new DataColumn("system"));
            dt.Columns.Add(new DataColumn("delitem"));
            foreach (XmlNode menuitem in mainmenus)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = menuitem["id"].InnerText;
                dr["title"] = menuitem["title"].InnerText;
                dr["defaulturl"] = menuitem["defaulturl"].InnerText;
                dr["system"] = menuitem["system"].InnerText != "0" ? "是" : "否";
                if (menuitem["mainmenulist"].InnerText != "")
                    dr["delitem"] = "删除";
                else
                    dr["delitem"] = "<a href='managemainmenu.aspx?mode=del&menuid=" + menuitem["id"].InnerText + "' onclick='return confirm(\"您确认要删除此菜单项吗?\")'>删除</a>";
                dt.Rows.Add(dr);
            }
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
        }

        protected void createMenu_Click(object sender, EventArgs e)
        {
            MenuManage.CreateMenuJson();
        }

        #region Web Form Designer generated code
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.createMenu.Click += new EventHandler(this.createMenu_Click);
        }

        #endregion
    }
}
