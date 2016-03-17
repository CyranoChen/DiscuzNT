using System;
using System.Web;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Web.UI;
using Discuz.Config;
using Discuz.Common.Generic;
using System.Text.RegularExpressions;

namespace Discuz.Web.Archiver
{
    /// <summary>
    /// 论坛版块列表
    /// </summary>
    public class showforum : ArchiverPage
    {

        private string FORUM_LINK = "<a href=\"showforum-{0}{1}\">{2}</a>";
        public showforum()
        {
            if (config.Aspxrewrite != 1)
            {
                FORUM_LINK = "<a href=\"showforum{1}?forumid={0}\">{2}</a>";
            }
            int forumid = DNTRequest.GetInt("forumid", -1);
            if (forumid == -1)
            {
                ShowMsg("无效的版块ID");
                return;
            }

            ForumInfo forum = Forums.GetForumInfo(forumid);
            if (forum == null)
            {
                ShowMsg("不存在的版块ID");
                return;
            }
            if (!Forums.AllowView(forum.Viewperm, usergroupinfo.Groupid))
            {
                ShowMsg("您没有浏览该版块的权限");
                return;
            }

            if (forum.Password != "")
            {
                ShowMsg("简洁版本无法浏览设置了密码的版块");
                return;
            }

            ShowTitle(Utils.RemoveHtml(forum.Name) + " - ");
            ShowBody();
            HttpContext.Current.Response.Write("<h1>" + config.Forumtitle + "</h1>");
            HttpContext.Current.Response.Write("<div class=\"forumnav\">");
            HttpContext.Current.Response.Write("<a href=\"index.aspx\">首页</a> &raquo; ");
            string[] pathList = forum.Pathlist.Trim().Replace("&raquo; ",",").Split(',');
            string[] parentIdList = forum.Parentidlist.Trim().Split(',');
            for (int i = 0; i < pathList.Length - 1; i++)
            {
                RegexOptions options = RegexOptions.None;
                Regex regex = new Regex(@"""/.*/list\.aspx", options);
                pathList[i] = regex.Replace(pathList[i], string.Format("\"showforum-{0}.aspx", parentIdList[i]));

            }
            pathList[pathList.Length - 1] = string.Format("<a href=\"showforum-{0}.aspx\">{1}</a>", forum.Fid, forum.Name);
            string nav = string.Join(" &raquo; ", pathList);
            HttpContext.Current.Response.Write(ForumUtils.UpdatePathListExtname(nav, config.Extname));
            HttpContext.Current.Response.Write("</div>\r\n");
            HttpContext.Current.Response.Write("<div id=\"wrap\">");

            //得到当前用户请求的页数
            int pageid = DNTRequest.GetInt("page", 1);
            //获取主题总数
            int topiccount = Topics.GetTopicCount(forumid);

            // 得到Tpp设置
            int tpp = 50;

            //获取总页数
            int pagecount = topiccount % tpp == 0 ? topiccount / tpp : topiccount / tpp + 1;
            if (pagecount == 0)
                pagecount = 1;

            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;

            if (pageid > pagecount)
                pageid = pagecount;

            if (forum.Layer == 0)
            {
                string extName = config.Aspxrewrite == 1 ? config.Extname : ".aspx";
                List<ForumInfo> subForumList = Forums.GetSubForumList(forum.Fid);
                HttpContext.Current.Response.Write("<ol>");
                foreach (ForumInfo info in subForumList)
                {
                    HttpContext.Current.Response.Write("<div class=\"forumitem\"><h3>");
                    HttpContext.Current.Response.Write(Utils.GetSpacesString(info.Layer));
                    HttpContext.Current.Response.Write(string.Format(FORUM_LINK, info.Fid, extName, Utils.HtmlDecode(info.Name)));
                    HttpContext.Current.Response.Write("</h3></div>\r\n");
                }
                HttpContext.Current.Response.Write("</ol>");
            }


            HttpContext.Current.Response.Write("<ol>");
            //获取当前页主题列表
            DataTable dt;
            dt = Topics.GetTopicList(forumid, pageid, tpp);

            foreach (DataRow dr in dt.Rows)
            {
                if (config.Aspxrewrite == 1)
                    HttpContext.Current.Response.Write(string.Format("<li><a href=\"showtopic-{0}{1}\">{2}</a>  &nbsp; ({3} 篇回复)</li>", dr["tid"].ToString(), config.Extname, dr["title"].ToString().Trim(), dr["replies"].ToString()));
                else
                    HttpContext.Current.Response.Write(string.Format("<li><a href=\"showtopic.aspx?topicid={0}\">{1}</a>  &nbsp; ({2} 篇回复)</li>", dr["tid"].ToString(), dr["title"].ToString().Trim(), dr["replies"].ToString()));
            }
            HttpContext.Current.Response.Write("</ol>");
            HttpContext.Current.Response.Write("</div>");

            //页码链接
            HttpContext.Current.Response.Write("<div class=\"pagenumbers\">");
            if (config.Aspxrewrite == 1)
            {
                HttpContext.Current.Response.Write(Utils.GetStaticPageNumbers(pageid, pagecount, "showforum-" + forumid, config.Extname, 8));
                HttpContext.Current.Response.Write("</div>");
                HttpContext.Current.Response.Write(string.Format("<div class=\"fullversion\">查看完整版本: <a href=\"../showforum-{0}{1}\">{2}</a></div>\r\n", forumid, config.Extname, forum.Name));
            }
            else
            {
                HttpContext.Current.Response.Write(Utils.GetPageNumbers(pageid, pagecount, "showforum.aspx", 8));
                HttpContext.Current.Response.Write("</div>");
                HttpContext.Current.Response.Write(string.Format("<div class=\"fullversion\">查看完整版本: <a href=\"../showforum.aspx?forumid={0}\">{1}</a></div>\r\n", forumid, forum.Name));
            }
            ShowFooter();
            //HttpContext.Current.Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
