using System;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Web.UI;

namespace Discuz.Web.Archiver
{
    /// <summary>
    /// ShowTopic 的摘要说明。
    /// </summary>
    public class showtopic : ArchiverPage
    {
        public Discuz.Common.Generic.List<ShowtopicPageAttachmentInfo> attachmentlist = new Discuz.Common.Generic.List<ShowtopicPageAttachmentInfo>();
        public Discuz.Common.Generic.List<ShowtopicPagePostInfo> postlist = new Discuz.Common.Generic.List<ShowtopicPagePostInfo>();

        public showtopic()
        {
            // 获取主题ID
            int topicid = DNTRequest.GetInt("topicid", -1);
            // 如果主题ID非数字
            if (topicid == -1)
            {
                ShowMsg("无效的主题ID");
                return;
            }

            // 获取该主题的信息
            TopicInfo topic = Topics.GetTopicInfo(topicid);
            // 如果该主题不存在
            if (topic == null || topic.Closed > 1)
            {
                ShowMsg("不存在的主题ID");
                return;
            }

            if (topic.Displayorder == -1)
            {
                ShowMsg("此主题已被删除！");
                return;
            }

            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid)
            {
                ShowMsg(string.Format("本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够", topic.Readperm, usergroupinfo.Grouptitle));
                return;
            }

            ForumInfo forum = Forums.GetForumInfo(topic.Fid);
            if (Utils.StrIsNullOrEmpty(forum.Viewperm))//当板块权限为空时，按照用户组权限
            {
                if (usergroupinfo.Allowvisit != 1)
                {
                    ShowMsg("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该版块的权限");
                    return;
                }
            }
            else//当板块权限不为空，按照板块权限
            {
                if (!Forums.AllowView(forum.Viewperm, usergroupinfo.Groupid))
                {
                    ShowMsg("您没有浏览该版块的权限");
                    return;
                }
            }

            if (forum.Password != "")
            {
                ShowMsg("简洁版本无法浏览设置了密码的版块");
                return;
            }

            //验证用户是否为本版版主
            int ismoder = Moderators.IsModer(useradminid, userid, forum.Fid) ? 1 : 0;

            //购买帖子操作
            //判断是否为回复可见帖, price=0为非购买可见(正常), price>0 为购买可见, price=-1为购买可见但当前用户已购买
            int price = 0;
            if (topic.Price > 0 && userid != topic.Posterid && ismoder != 1)
            {
                price = topic.Price;
                //时间乘以-1是因为当Configs.GetMaxChargeSpan()==0时,帖子始终为购买帖
                if (PaymentLogs.IsBuyer(topicid, userid) || (Utils.StrDateDiffHours(topic.Postdatetime, Scoresets.GetMaxChargeSpan()) > 0 && Scoresets.GetMaxChargeSpan() != 0))//判断当前用户是否已经购买
                {
                    price = -1;
                }
            }
            if (price > 0)
            {
                ShowMsg(string.Format("此帖需转到完整版处购买后才可查看.<a href=\"{0}buytopic.aspx?topicid={1}\">点击购买</a>", BaseConfigs.GetForumPath, topic.Tid));
                return;
            }


            // 获取帖子总数
            int postcount = topic.Replies + 1;
            int pageid = 1;

            // 得到Tpp设置
            int ppp = 30;

            //获取总页数
            int pagecount = postcount % ppp == 0 ? postcount / ppp : postcount / ppp + 1;
            if (pagecount == 0)
                pagecount = 1;

            // 得到当前用户请求的页数
            if (DNTRequest.GetString("page").ToLower().Equals("end"))
                pageid = pagecount;
            else
                pageid = DNTRequest.GetInt("page", 1);

            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;

            if (pageid > pagecount)
                pageid = pagecount;

            int hide = 1;
            if (topic.Hide == 1 && (Posts.IsReplier(topicid, userid) || ismoder == 1))
                hide = -1;

            //获取当前页主题列表
            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Fid = forum.Fid;
            postpramsInfo.Tid = topicid;
            postpramsInfo.Jammer = forum.Jammer;
            postpramsInfo.Pagesize = ppp;
            postpramsInfo.Pageindex = pageid;
            postpramsInfo.Getattachperm = forum.Getattachperm;
            postpramsInfo.Usergroupid = usergroupinfo.Groupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Hide = hide;
            postpramsInfo.Price = topic.Price;
            postpramsInfo.Usergroupreadaccess = usergroupinfo.Readaccess;
            postpramsInfo.CurrentUserid = userid;
            postpramsInfo.Showimages = forum.Allowimgcode;
            // 简洁版本中关闭表情符的解析
            postpramsInfo.Smileyoff = 1;
            postpramsInfo.Smiliesmax = 0;
            postpramsInfo.Smiliesinfo = null;
            postpramsInfo.Customeditorbuttoninfo = null;
            postpramsInfo.Bbcodemode = 0;
            // 简洁版本中关闭ubb转换
            postpramsInfo.Bbcodeoff = 1;
            postpramsInfo.CurrentUserGroup = usergroupinfo;
            postpramsInfo.Onlinetimeout = config.Onlinetimeout;

            UserInfo userInfo = Users.GetUserInfo(userid);
            postpramsInfo.Usercredits = userInfo == null ? 0 : userInfo.Credits;

            postlist = Posts.GetPostList(postpramsInfo, out attachmentlist, ismoder == 1);
            if (postlist.Count <= 0)
            {
                ShowMsg("读取信息失败");
                return;
            }

            ShowTitle(topic.Title + " - ");
            ShowBody();
            HttpContext.Current.Response.Write("<h1>" + config.Forumtitle + "</h1>");
            HttpContext.Current.Response.Write("<div class=\"forumnav\">");
            HttpContext.Current.Response.Write("<a href=\"index.aspx\">首页</a> &raquo; ");

            if (config.Aspxrewrite == 1)
                HttpContext.Current.Response.Write(string.Format("{0} &raquo; <a href=\"showtopic-{1}{2}\">{3}</a>", ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname).Replace("</a><", "</a> &raquo; <"), topicid.ToString(), config.Extname, topic.Title));
            else
                HttpContext.Current.Response.Write(string.Format("{0} &raquo; <a href=\"showtopic.aspx?topicid={1}\">{2}</a>", ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), "aspx").Replace("</a><", "</a> &raquo; <"), topicid.ToString(), topic.Title));

            HttpContext.Current.Response.Write("</div>\r\n");

            //将帖子内容中复杂的图片附件html解析为简单的无js脚本的标签
            Regex regex = new Regex("<img alt=.*? imageid=\"(.*?)\".*?newsrc=\"(.*?)\".*?/>", RegexOptions.IgnoreCase);
            Regex regex2 = new Regex("<img imageid=\"(.*?)\" src=\"(.*?)\".*?/>", RegexOptions.IgnoreCase);
            Match match;

            foreach (ShowtopicPagePostInfo postinfo in postlist)
            {
                HttpContext.Current.Response.Write("<div class=\"postitem\">\r\n");
                HttpContext.Current.Response.Write("\t<div class=\"postitemtitle\">\r\n");
                HttpContext.Current.Response.Write(Utils.HtmlEncode(postinfo.Poster) + " - " + postinfo.Postdatetime);
                HttpContext.Current.Response.Write("</div><div class=\"postitemcontent\">");

                if (config.Showimgattachmode == 1)
                {
                    for (match = regex.Match(postinfo.Message); match.Success; match = match.NextMatch())
                    {
                        postinfo.Message = postinfo.Message.Replace(match.Value, string.Format("<a href=\"{0}\" target=\"_blank\">点击显示图片:{1}</a>",
                            match.Groups[2].Value, match.Groups[1].Value));
                    }
                }
                else
                {
                    for (match = regex2.Match(postinfo.Message); match.Success; match = match.NextMatch())
                    {
                        postinfo.Message = postinfo.Message.Replace(match.Value, string.Format("<img alt=\"{0}\" src=\"{1}\" />",
                            match.Groups[1].Value, match.Groups[2].Value));
                    }
                }
                HttpContext.Current.Response.Write(postinfo.Message);

                foreach (ShowtopicPageAttachmentInfo attinfo in attachmentlist)
                {
                    if (attinfo.Pid == postinfo.Pid)
                    {
                        HttpContext.Current.Response.Write(string.Format("<br /><br />附件: <a href=\"../attachment.aspx?attachmentid={0}\">{1}</a>", attinfo.Aid, Utils.HtmlEncode(attinfo.Attachment)));
                    }
                }
                HttpContext.Current.Response.Write("\t</div>\r\n</div>\r\n");
            }
            //得到页码链接
            HttpContext.Current.Response.Write("<div class=\"pagenumbers\">");

            if (config.Aspxrewrite == 1)
                HttpContext.Current.Response.Write(Utils.GetStaticPageNumbers(pageid, pagecount, "showtopic-" + topicid, config.Extname, 8));
            else
                HttpContext.Current.Response.Write(Utils.GetPageNumbers(pageid, pagecount, "showtopic.aspx?topicid=" + topicid, 8, "page"));

            HttpContext.Current.Response.Write("</div>\r\n");
            //更新查看次数
            TopicStats.Track(topicid, 1);

            if (config.Aspxrewrite == 1)
                HttpContext.Current.Response.Write(string.Format("<div class=\"fullversion\">查看完整版本: <a href=\"../showtopic-{0}{1}\">{2}</a></div>\r\n", topicid, config.Extname, topic.Title));
            else
                HttpContext.Current.Response.Write(string.Format("<div class=\"fullversion\">查看完整版本: <a href=\"../showtopic.aspx?topicid={0}\">{1}</a></div>\r\n", topicid, topic.Title));

            ShowFooter();
            //HttpContext.Current.Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }
}
