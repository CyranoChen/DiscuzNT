using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
    /// <summary>
    /// 投票页面
    /// </summary>
    public class poll : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 主题信息
        /// </summary>
        public TopicInfo topic;
        /// <summary>
        /// 所属版块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// 主题Id
        /// </summary>
        public int topicid = DNTRequest.GetInt("topicid", -1);
        /// <summary>
        /// 主题标题
        /// </summary>
        public string topictitle = "";
        #endregion

        protected override void ShowPage()
        {
            if (topicid == -1)
            {
                AddErrLine("无效的主题ID");
                return;
            }
            topic = Topics.GetTopicInfo(topicid);
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return;
            }

            topictitle = Utils.StrIsNullOrEmpty(topic.Title) ? "" : topic.Title;
            forumid = topic.Fid;
            ForumInfo forum = Forums.GetForumInfo(forumid);
            pagetitle = Utils.RemoveHtml(forum.Name);
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

            if (topic.Special != 1)
            {
                AddErrLine("不存在的投票ID");
                return;
            }
            if (usergroupinfo.Allowvote != 1)
            {
                AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有投票的权限");
                return;
            }
            if (Convert.ToDateTime(Polls.GetPollEnddatetime(topic.Tid)).Date < DateTime.Today)
            {
                AddErrLine("投票已经过期");
                return;
            }
            if (userid != -1 && !Polls.AllowVote(topicid, username))
            {
                AddErrLine("你已经投过票");
                return;
            }
            else if (Utils.InArray(topic.Tid.ToString(), ForumUtils.GetCookie("dnt_polled")))
            {
                AddErrLine("你已经投过票");
                return;
            }

            //当未选择任何投票项时
            if(Utils.StrIsNullOrEmpty(DNTRequest.GetString("pollitemid")))
            {
                AddErrLine("您未选择任何投票项！");
                return;
            }
            if (DNTRequest.GetString("pollitemid").Split(',').Length > Polls.GetPollInfo(topicid).Maxchoices)
            {
                AddErrLine("您的投票项多于最大投票数");
                return;
            }
            if (Polls.UpdatePoll(topicid, DNTRequest.GetString("pollitemid"), userid == -1 ? string.Format("{0} [{1}]", usergroupinfo.Grouptitle, DNTRequest.GetIP()) : username) < 0)
            {
                AddErrLine("提交投票信息中包括非法内容");
                return;
            }

            if (userid == -1)  ForumUtils.WriteCookie("dnt_polled", string.Format("{0},{1}", (userid != -1 ? "" : ForumUtils.GetCookie("dnt_polled")), topic.Tid));

            SetUrl(base.ShowTopicAspxRewrite(topicid, 0));
            SetMetaRefresh();
            SetShowBackLink(false);
            MsgForward("poll_succeed");
            AddMsgLine("投票成功, 返回主题");

            UserCredits.UpdateUserCreditsByVotepoll(userid);
            // 删除主题游客缓存
            ForumUtils.DeleteTopicCacheFile(topicid);           
        }
    }
}