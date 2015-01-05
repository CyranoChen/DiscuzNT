using System;
using System.Data;
using System.Text;
using System.Web;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Forum;

namespace Discuz.Web
{
    /// <summary>
    /// 查看悬赏主题页面
    /// </summary>
    public class showbonus : Discuz.Web.UI.TopicPage //PageBase
    {
        #region 页面变量
        /// <summary>
        /// 帖子列表
        /// </summary>
        public List<ShowbonusPagePostInfo> postlist;
        /// <summary>
        /// 短消息列表
        /// </summary>
        public List<PrivateMessageInfo> pmlist;
        /// <summary>
        /// 悬赏给分日志
        /// </summary>
        public List<BonusLogInfo> bonuslogs;
        /// <summary>
        /// 是否允许评分
        /// </summary>
        public bool allowrate = false;
        /// <summary>
        /// 上一次进行的管理操作
        /// </summary>
        public string moderactions;
        /// <summary>
        /// 是否显示下载链接
        /// </summary>
        public bool allowdownloadattach = false;
        /// <summary>
        /// 弹出导航菜单的HTML代码
        /// </summary>
        public string navhomemenu = "";
        #endregion

        protected override void ShowPage()
        {
            //获取主题信息
            topic = GetTopicInfo();
            if (topic == null || IsErr())
                return;

            //未结束的悬赏
            if (topic.Special != 3)
            {
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + ShowTopicAspxRewrite(topic.Tid, 1));
                return;
            }

            topicid = topic.Tid;
            forumid = topic.Fid;
            forum = Forums.GetForumInfo(forumid);
            if (forum == null)
            {
                AddErrLine("不存在的版块ID"); return;
            }

            pagetitle = string.Format("{0} - {1}", topic.Title, Utils.RemoveHtml(forum.Name));

            //得到广告列表              
            GetForumAds(forum.Fid);

            //检查是否具有版主的身份
            IsModer();

            //验证不通过则返回
            if (!ValidateInfo())
                return;

            //编辑器状态
            EditorState();
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);
            bonuslogs = Bonus.GetLogs(topic);

            if (topic.Moderated > 0)
                moderactions = TopicAdmins.GetTopicListModeratorLog(topicid);

            Caches.GetTopicTypeArray().TryGetValue(topic.Typeid, out topictypes);
            topictypes = topictypes != "" ? "[" + topictypes + "]" : "";

            if (newpmcount > 0)
                pmlist = PrivateMessages.GetPrivateMessageListForIndex(userid, 5, 1, 1);

            score = Scoresets.GetValidScoreName();
            scoreunit = Scoresets.GetValidScoreUnit();

            GetPostAds(GetPostPramsInfo(), postlist.Count);

            if (postlist.Count <= 0)
            {
                AddErrLine("读取信息失败");
                return;
            }

            //更新页面Meta信息
            UpdateMetaInfo(Utils.RemoveHtml(postlist[0].Message));

            //更新主题查看次数和在线用户信息
            TopicStats.Track(topicid, 1);
            Topics.MarkOldTopic(topic);
            topicviews = topic.Views + 1 + (config.TopicQueueStats == 1 ? TopicStats.GetStoredTopicViewCount(topic.Tid) : 0);
            OnlineUsers.UpdateAction(olid, UserAction.ShowTopic.ActionID, forumid, forum.Name, topicid, topic.Title);

            BindDownloadAttachmentTip();

            ForumUtils.WriteCookie("referer", string.Format("showbonus.aspx?topicid={0}", topicid.ToString()));
        }

        /// <summary>
        /// 获取帖子参数信息(PostPramsInfo)
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public PostpramsInfo GetPostPramsInfo()
        {
            //获取当前页主题列表
            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Fid = forum.Fid;
            postpramsInfo.Tid = topicid;
            postpramsInfo.Jammer = forum.Jammer;
            //postpramsInfo.Pagesize = int.MaxValue;
            postpramsInfo.Pagesize = (ppp <= 0 ? config.Ppp : ppp);
            postpramsInfo.Pageindex = 1;
            postpramsInfo.Getattachperm = forum.Getattachperm;
            postpramsInfo.Usergroupid = usergroupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Price = 0;
            postpramsInfo.Usergroupreadaccess = usergroupinfo.Readaccess;
            postpramsInfo.Usergroupreadaccess = (ismoder == 1) ? int.MaxValue : usergroupinfo.Readaccess;
            postpramsInfo.CurrentUserid = userid;
            postpramsInfo.Showimages = forum.Allowimgcode;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Smiliesmax = config.Smiliesmax;
            postpramsInfo.Bbcodemode = config.Bbcodemode;
            postpramsInfo.CurrentUserGroup = usergroupinfo;
            //判断是否为回复可见帖, hide=0为不解析[hide]标签, hide>0解析为回复可见字样, hide=-1解析为以下内容回复可见字样显示真实内容
            //将逻辑判断放入取列表的循环中处理,此处只做是否为回复人的判断，主题作者也该可见
            postpramsInfo.Hide = (topic.Hide == 1 && (Posts.IsReplier(topicid, userid) || ismoder == 1)) ? -1 : 1;
            postpramsInfo.Condition = Posts.GetPostPramsInfoCondition(onlyauthor, topicid, topic.Posterid);
            //if (!(Utils.StrIsNullOrEmpty(onlyauthor) || onlyauthor.Equals("0")))
            //    postpramsInfo.Condition = string.Format(" {0}.posterid={1}", Posts.GetPostTableName(topicid), topic.Posterid);

            postlist = Posts.GetPostListWithBonus(postpramsInfo, out attachmentlist, ismoder == 1);

            return postpramsInfo;
        }

    }
}