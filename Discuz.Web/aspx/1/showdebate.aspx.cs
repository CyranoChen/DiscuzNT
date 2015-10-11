using System;
using System.Data;
using System.Text;
using System.Web;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
    /// <summary>
    /// 查看辩论主题页面
    /// </summary>
    public class showdebate : Discuz.Web.UI.TopicPage //PageBase
    {
        #region 页面变量
        /// <summary>
        /// 正方帖子列表
        /// </summary>
        public List<ShowtopicPagePostInfo> positivepostlist;
        /// <summary>
        /// 反方帖子列表
        /// </summary>
        public List<ShowtopicPagePostInfo> negativepostlist;
        /// <summary>
        /// 悬赏给分日志
        /// </summary>
        public List<BonusLogInfo> bonuslogs;
        /// <summary>
        /// 投票选项列表
        /// </summary>
        public DataTable polllist;
        /// <summary>
        /// 主题魔法表情
        /// </summary>
        public string topicmagic = "";
        /// <summary>
        /// 上一次进行的管理操作
        /// </summary>
        public string moderactions;
        /// <summary>
        /// 是否显示评分记录
        /// </summary>
        public int showratelog;
        /// <summary>
        /// 是否显示下载链接
        /// </summary>
        public bool allowdownloadattach = false;
        /// <summary>
        /// 当为投票帖时有用,0=单选，1=多选
        /// </summary>
        public int polltype = -1;
        /// <summary>
        /// 投票结束时间
        /// </summary>
        public string pollenddatetime = "";
        /// <summary>
        /// 通过TID得到帖子观点
        /// </summary>
        public Dictionary<int, int> debateList;
        /// <summary>
        /// 作为辩论主题的扩展属性
        /// </summary>
        public DebateInfo debateexpand;
        /// <summary>
        /// 弹出导航菜单的HTML代码
        /// </summary>
        public string navhomemenu = "";

        public bool isdiggs = false;

        public ShowtopicPagePostInfo debatepost;

        public float positivepercent = 50;
        public float negativepercent = 50;

        public string positivepagenumbers;
        public string negativepagenumbers;

        public int positivepagecount;
        public int negativepagecount;
        #endregion

        private int pagesize = GeneralConfigs.GetConfig().Debatepagesize > 0 ? GeneralConfigs.GetConfig().Debatepagesize : 16;

        protected override void ShowPage()
        {
            //获取主题信息
            topic = GetTopicInfo();
            if (topic == null || IsErr())
                return;

            topicid = topic.Tid;
            forumid = topic.Fid;
            forum = Forums.GetForumInfo(forumid);
            if (forum == null)
            {
                AddErrLine("不存在的版块ID"); return;
            }

            pagetitle = string.Format("{0} - {1}", topic.Title, Utils.RemoveHtml(forum.Name));
            ///得到广告列表              
            GetForumAds(forum.Fid);

            // 检查是否具有版主的身份
            if (useradminid != 0)
            {
                ismoder = Moderators.IsModer(useradminid, userid, forum.Fid) ? 1 : 0;
                admininfo = AdminGroups.GetAdminGroupInfo(usergroupid); //得到管理组信息
                if (admininfo != null)
                    disablepostctrl = admininfo.Disablepostctrl;
            }

            //验证不通过则返回
            if (!ValidateInfo())
                return;

            Caches.GetTopicTypeArray().TryGetValue(topic.Typeid, out topictypes);
            topictypes = topictypes != "" ? "[" + topictypes + "]" : "";

            showratelog = GeneralConfigs.GetConfig().DisplayRateCount > 0 ? 1 : 0;
            score = Scoresets.GetValidScoreName();
            scoreunit = Scoresets.GetValidScoreUnit();

            //编辑器状态
            EditorState();
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);
            usesig = ForumUtils.GetCookie("sigstatus") == "0" ? 0 : 1;

            int price = 0;
            if (topic.Special != 4)//不是辩论帖，就跳转到showtopic页面显示
            {
                HttpContext.Current.Response.Redirect(forumpath + this.ShowTopicAspxRewrite(topic.Tid, 1)); return;
            }

            if (topic.Moderated > 0)
                moderactions = TopicAdmins.GetTopicListModeratorLog(topicid);

            // 获取帖子总数
            onlyauthor = Utils.StrIsNullOrEmpty(onlyauthor) ? "0" : onlyauthor;

            // 获取分页相关信息
            BindPageCountAndId();

            PostpramsInfo postpramsInfo = GetPostPramsInfo(price);
            //获取当前正反方列表     
            positivepostlist = Debates.GetPositivePostList(postpramsInfo, out attachmentlist, ismoder == 1);
            negativepostlist = Debates.GetNegativePostList(postpramsInfo, out attachmentlist, ismoder == 1);

            GetPostAds(postpramsInfo, positivepostlist.Count);

            //辩论帖
            if (topic.Special == 4)
                GetDebateInfo(postpramsInfo);

            enabletag = (config.Enabletag & forum.Allowtag) == 1;
            if (enabletag)
                relatedtopics = Topics.GetRelatedTopicList(topicid, 5);

            //更新页面Meta信息
            UpdateMetaInfo(Utils.RemoveHtml(debatepost.Message));

            ///更新主题查看次数和在线用户信息
            TopicStats.Track(topicid, 1);
            Topics.MarkOldTopic(topic);
            topicviews = topic.Views + 1 + (config.TopicQueueStats == 1 ? TopicStats.GetStoredTopicViewCount(topic.Tid) : 0);
            OnlineUsers.UpdateAction(olid, UserAction.ShowTopic.ActionID, forumid, forum.Name, topicid, topic.Title);
            BindDownloadAttachmentTip();
        }


        /// <summary>
        /// 获取辩论信息
        /// </summary>
        public void GetDebateInfo(PostpramsInfo postpramsInfo)
        {
            debateexpand = Debates.GetDebateTopic(topicid);
            debateList = Debates.GetPostDebateList(topicid);//通过TID得到帖子观点
            if (debateexpand.Terminaltime < DateTime.Now)
                isenddebate = true;

            int positivepostlistcount = Debates.GetDebatesPostCount(postpramsInfo, 1);
            int negativepostlistcount = Debates.GetDebatesPostCount(postpramsInfo, 2);

            positivepagecount = (positivepostlistcount % pagesize == 0) ? (positivepostlistcount / pagesize) : (positivepostlistcount / pagesize + 1);
            negativepagecount = (negativepostlistcount % pagesize == 0) ? (negativepostlistcount / pagesize) : (negativepostlistcount / pagesize + 1);

            positivepagenumbers = Utils.GetAjaxPageNumbers(1, positivepagecount, "showdebatepage('" + forumpath + "tools/ajax.aspx?t=getdebatepostpage&opinion=1&tid=" + topic.Tid + "&{0}'," + parseurloff + ", " + smileyoff + ", " + bbcodeoff + ",'" + isenddebate + "',1," + userid + "," + topicid + ")", 8);
            negativepagenumbers = Utils.GetAjaxPageNumbers(1, negativepagecount, "showdebatepage('" + forumpath + "tools/ajax.aspx?t=getdebatepostpage&opinion=2&tid=" + topic.Tid + "&{0}'," + parseurloff + ", " + smileyoff + ", " + bbcodeoff + ",'" + isenddebate + "',2," + userid + "," + topicid + ")", 8);

            //防止无人参与时0做除数
            if (debateexpand.Negativediggs + debateexpand.Positivediggs != 0)
            {
                positivepercent = (float)debateexpand.Positivediggs / (float)(debateexpand.Negativediggs + debateexpand.Positivediggs) * 100;
                negativepercent = 100 - positivepercent;
            }

            foreach (ShowtopicPagePostInfo postlistinfo in positivepostlist)
            {
                //设置POST的观点属性
                if (debateList != null && debateList.ContainsKey(postlistinfo.Pid))
                    postlistinfo.Debateopinion = Convert.ToInt32(debateList[postlistinfo.Pid]);
            }
        }


        /// <summary>
        /// 获取帖子参数信息(PostPramsInfo)
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public PostpramsInfo GetPostPramsInfo(int price)
        {
            //获取当前页主题列表
            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Fid = forum.Fid;
            postpramsInfo.Tid = topicid;
            postpramsInfo.Jammer = forum.Jammer;
            postpramsInfo.Pagesize = pagesize;     // 得到Ppp设置
            postpramsInfo.Pageindex = pageid;
            postpramsInfo.Getattachperm = forum.Getattachperm;
            postpramsInfo.Usergroupid = usergroupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Price = price;
            postpramsInfo.Usergroupreadaccess = (ismoder == 1) ? int.MaxValue : usergroupinfo.Readaccess;
            postpramsInfo.CurrentUserid = userid;
            postpramsInfo.Showimages = forum.Allowimgcode;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Smiliesmax = config.Smiliesmax;
            postpramsInfo.Bbcodemode = config.Bbcodemode;
            postpramsInfo.CurrentUserGroup = usergroupinfo;
            postpramsInfo.Topicinfo = topic;
            postpramsInfo.Condition = Posts.GetPostPramsInfoCondition(onlyauthor, topicid, topic.Posterid);

            //判断是否为回复可见帖, hide=0为不解析[hide]标签, hide>0解析为回复可见字样, hide=-1解析为以下内容回复可见字样显示真实内容
            //将逻辑判断放入取列表的循环中处理,此处只做是否为回复人的判断，主题作者也该可见
            postpramsInfo.Hide = (topic.Hide == 1 && (Posts.IsReplier(topicid, userid) || ismoder == 1)) ? -1 : 1;
            postpramsInfo.Pid = Posts.GetFirstPostId(topic.Tid);
            UserInfo userInfo = Users.GetUserInfo(userid);
            postpramsInfo.Usercredits = userInfo == null ? 0 : userInfo.Credits;
            debatepost = Posts.GetSinglePost(postpramsInfo, out attachmentlist, ismoder == 1);
            postpramsInfo.Pid = 0;

            return postpramsInfo;
        }
    }
}