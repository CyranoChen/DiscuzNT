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
using Discuz.Common.Xml;
using Discuz.Forum;

namespace Discuz.Web.Admin
{
    public class favoritefunction : System.Web.UI.UserControl
    {
        public string resultmessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            //加入快捷操作菜单
            resultmessage = "<img src='../images/existmenu.gif' style='vertical-align:middle'/> 已经收藏";
            string configPath = Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config");
            string title = "";
            string menuparentid = "";
            string url = DNTRequest.GetString("url").ToLower();
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            //读到快捷操作菜单
            XmlNodeList shortcuts = doc.SelectNodes("/dataset/shortcut");
            foreach (XmlNode singleshortcut in shortcuts)
            {
                //如果当前链接在快捷菜单内，则返回
                if (singleshortcut.SelectSingleNode("link").InnerText == url.ToLower().Trim()) return;
            }
            XmlNodeList submains = doc.SelectNodes("/dataset/submain");
            XmlNodeInnerTextVisitor submainsvisitor = new XmlNodeInnerTextVisitor();
            
            foreach (XmlNode submain in submains)
            {
                submainsvisitor.SetNode(submain);
                if (submainsvisitor["link"].ToLower() == url)
                {
                    title = submainsvisitor["menutitle"];
                    menuparentid = submainsvisitor["menuparentid"];
                }
            }
            string[] parm = GetParm(doc,menuparentid);
            XmlElement shortcut = doc.CreateElement("shortcut");
            //将当前页面存入快捷操作菜单内
            doc.AppendChildElementByNameValue(ref shortcut, "link", url);
            doc.AppendChildElementByNameValue(ref shortcut, "menutitle", title);
            doc.AppendChildElementByNameValue(ref shortcut, "frameid", "main");
            doc.AppendChildElementByNameValue(ref shortcut, "custommenu", "true");
            doc.AppendChildElementByNameValue(ref shortcut, "showmenuid", parm[0]);
            doc.AppendChildElementByNameValue(ref shortcut, "toptabmenuid", parm[1]);
            doc.AppendChildElementByNameValue(ref shortcut, "mainmenulist", parm[2]);
            doc.SelectSingleNode("/dataset").AppendChild(shortcut);
            doc.Save(configPath);
            MenuManage.CreateMenuJson();
        }

        /// <summary>
        /// 获取当前页面的参数
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="menuparentid"></param>
        /// <returns></returns>
        public string[] GetParm(XmlDocumentExtender doc, string menuparentid)
        {
            string[] parm = new string[3];
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/mainmenu");
            XmlNodeInnerTextVisitor menuvisitor = new XmlNodeInnerTextVisitor();
            foreach (XmlNode mainmenu in mainmenus)
            {
                menuvisitor.SetNode(mainmenu);
                if (menuvisitor["menuid"] == menuparentid)
                {
                    parm[0] = menuvisitor["id"];
                    break;
                }
            }
            XmlNodeList toptabmenus = doc.SelectNodes("/dataset/toptabmenu");
            foreach (XmlNode toptabmenu in toptabmenus)
            {
                menuvisitor.SetNode(toptabmenu);
                if (("," + menuvisitor["mainmenulist"] + ",").IndexOf("," + parm[0] + ",") != -1)
                {
                    parm[1] = menuvisitor["id"];
                    parm[2] = menuvisitor["mainmenulist"];
                    break;
                }
            }
            return parm;
        }
    }
}