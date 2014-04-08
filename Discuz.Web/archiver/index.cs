using System;
using System.Web;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Common.Generic;


namespace Discuz.Web.Archiver
{
    /// <summary>
    /// 简洁版首页
    /// </summary>
    public class index : ArchiverPage
    {
        private string FORUM_LINK = "<a href=\"showforum-{0}{1}\">{2}</a>";
        private List<ArchiverForumInfo> archiverForumList;
        private string extName = "";
        public index()
        {
            if (config.Aspxrewrite != 1)
            {
                FORUM_LINK = "<a href=\"showforum{1}?forumid={0}\">{2}</a>";
            }

            ShowTitle(config.Forumtitle + " ");
            ShowBody();
            HttpContext.Current.Response.Write("<h1>" + config.Forumtitle + "</h1>");
            HttpContext.Current.Response.Write("<div id=\"wrap\">");
            archiverForumList = Forums.GetArchiverForumIndexList(config.Hideprivate, usergroupinfo.Groupid);
            extName = config.Aspxrewrite == 1 ? config.Extname : ".aspx";
            WriteSubForumLayer(0);
            HttpContext.Current.Response.Write("</div>");
            HttpContext.Current.Response.Write("<div class=\"fullversion\">查看完整版本: <a href=\"../index.aspx\">" + config.Forumtitle + "</a></div>\r\n");
            ShowFooter();
            //HttpContext.Current.Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public void WriteSubForumLayer(int parentFid)
        {
            foreach (ArchiverForumInfo info in archiverForumList)
            {
                if (info.ParentidList == parentFid.ToString() || info.ParentidList.EndsWith("," + parentFid))
                {
                    HttpContext.Current.Response.Write(info.Layer == 0 ? "<div class=\"cateitem\"><h2>" : "<div class=\"forumitem\"><h3>");
                    if (info.Layer != 0)
                        HttpContext.Current.Response.Write(Utils.GetSpacesString(info.Layer));
                    HttpContext.Current.Response.Write(string.Format(FORUM_LINK, info.Fid, extName, Utils.HtmlDecode(info.Name)));
                    HttpContext.Current.Response.Write(string.Format("{0}</div>\r\n",info.Layer == 0 ? "</h2>" : "</h3>"));
                    WriteSubForumLayer(info.Fid);
                }
            }
        }
    }
}
