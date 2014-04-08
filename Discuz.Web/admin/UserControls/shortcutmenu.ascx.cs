using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.Common;
using System.Xml;

using Discuz.Common;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    public class shortcutmenu : System.Web.UI.UserControl
    {
        public string shortcutmenustr;

        protected void Page_Load(object sender, EventArgs e)
        {
            //显示快捷操作菜单
            shortcutmenustr = "";
            XmlDocument doc = new XmlDocument();
            doc.Load(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config"));
            XmlNodeList shortcuts = doc.SelectNodes("/dataset/shortcut");
            foreach (XmlNode singleshortcut in shortcuts)
            {
                shortcutmenustr += "<dt><a href='#' onclick=\"resetindexmenu('" + singleshortcut.SelectSingleNode("showmenuid").InnerText + "','";
                shortcutmenustr += singleshortcut.SelectSingleNode("toptabmenuid").InnerText + "','" + singleshortcut.SelectSingleNode("mainmenulist").InnerText;
                shortcutmenustr += "','" + singleshortcut.SelectSingleNode("link").InnerText + "');\">";
                shortcutmenustr += singleshortcut.SelectSingleNode("menutitle").InnerText + "</a></dt>";
            }
            if (shortcutmenustr != "")
                shortcutmenustr += "<hr class='line' />";
        }
    }
}