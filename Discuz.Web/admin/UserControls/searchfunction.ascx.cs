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

using Discuz.Common;
using Discuz.Config;

namespace Discuz.Web.Admin
{
    public class searchfunction : System.Web.UI.UserControl
    {
        public StringBuilder sb = new StringBuilder();

        public int menucount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //查找菜单功能
            string searchinfo = DNTRequest.GetString("searchinf");
            if (searchinfo != "")
            {
                System.Data.DataSet dsSrc = new System.Data.DataSet();
                dsSrc.ReadXml(Utils.GetMapPath(BaseConfigs.GetForumPath.ToLower() + "admin/xml/navmenu.config"));
                menucount = dsSrc.Tables["toptabmenu"].Rows.Count;

                int count = 0;
                bool isexist = false;
                sb.Append("<table width=\"98%\" align=\"center\"><tr>");
                foreach (System.Data.DataRow dr in dsSrc.Tables["submain"].Rows)
                {
                    //先找出子菜单表中的相关菜单
                    if (dr["menutitle"].ToString().IndexOf(searchinfo) >= 0)
                    {
                        isexist = true;

                        if (count >= 3)
                        {
                            count = 0;
                            sb.Append("</tr><tr>");
                        }

                        sb.Append("<td align=\"left\" width=\"33%\">");
                        try
                        {
                            sb.Append("[" + dsSrc.Tables["mainmenu"].Select("menuid=" + dr["menuparentid"].ToString().ToString().Trim())[0]["menutitle"].ToString().Trim() + "]");
                        }
                        catch { ;}
                        sb.Append("<a href=\"#\" onclick=\"javascript:resetindexmenu('showmainmenu','toptabmenuid','mainmenulist','" + dr["link"] + "');\">" + dr["menutitle"].ToString().ToString() + "</a></td>");

                        foreach (System.Data.DataRow toptabdr in dsSrc.Tables["toptabmenu"].Rows)
                        {
                            //当顶部菜单(tabmenu)中的mainmenuidlist中出现子菜单(submenu)中父menuparentid字符时
                            if (("," + toptabdr["mainmenuidlist"].ToString() + ",").IndexOf("," + dr["menuparentid"] + ",") >= 0)
                            {
                                sb.Replace("toptabmenuid", toptabdr["id"].ToString());
                                sb.Replace("mainmenulist", toptabdr["mainmenulist"].ToString());

                                string[] idlist = toptabdr["mainmenuidlist"].ToString().Split(',');
                                for (int i = 0; i < idlist.Length; i++)
                                {
                                    if (idlist[i] == dr["menuparentid"].ToString())  //找出要显示的mainmenuid
                                    {
                                        sb.Replace("showmainmenu", toptabdr["mainmenulist"].ToString().Split(',')[i]);
                                        break;
                                    }
                                }
                                break;
                            }
                        }

                        count++;
                    }
                }

                if (!isexist)
                {
                    sb.Append("没有找到相匹配的结果");
                }
                sb.Append("</tr></table>");

                dsSrc.Dispose();
            }
            else
            {
                sb.Append("您未输入任何搜索关键字");
            }
        }
    }
}