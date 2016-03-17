using System;
using System.Web;
using System.Data;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Common.Generic;
using System.Text.RegularExpressions;

namespace Discuz.Web
{
    /// <summary>
    /// 帖子管理页面
    /// </summary>
    public class topicadmin : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 操作标题
        /// </summary>
        public string operationtitle = "操作提示";
        /// <summary>
        /// 操作类型
        /// </summary>
        public string operation = DNTRequest.GetQueryString("operation").ToLower();
        /// <summary>
        /// 操作类型参数
        /// </summary>
        public string action = DNTRequest.GetQueryString("action");
        /// <summary>
        /// 主题列表
        /// </summary>
        public string topiclist = DNTRequest.GetString("topicid");
        /// <summary>
        /// 帖子Id列表
        /// </summary>
        public string postidlist = DNTRequest.GetString("postid");
        /// <summary>
        /// 版块名称
        /// </summary>
        public string forumname = "";
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// 帖子标题
        /// </summary>
        public string title = "";
        /// <summary>
        /// 帖子作者用户名
        /// </summary>
        public string poster = "";
        /// <summary>
        /// 版块Id
        /// </summary>
        public int forumid = DNTRequest.GetInt("forumid", -1);
        /// <summary>
        /// 版块列表
        /// </summary>
        public string forumlist = Caches.GetForumListBoxOptionsCache(true);
        /// <summary>
        /// 主题置顶状态
        /// </summary>
        public int displayorder = 0;
        /// <summary>
        /// 主题精华状态
        /// </summary>
        public int digest = DNTRequest.GetFormInt("level", -1);
        /// <summary>
        /// 高亮颜色
        /// </summary>
        public string highlight_color = DNTRequest.GetFormString("highlight_color");
        /// <summary>
        /// 是否加粗
        /// </summary>
        public string highlight_style_b = DNTRequest.GetFormString("highlight_style_b");
        /// <summary>
        /// 是否斜体
        /// </summary>
        public string highlight_style_i = DNTRequest.GetFormString("highlight_style_i");
        /// <summary>
        /// 是否带下划线
        /// </summary>
        public string highlight_style_u = DNTRequest.GetFormString("highlight_style_u");
        /// <summary>
        /// 关闭主题, 0=打开,1=关闭 
        /// </summary>
        public int close = 0;
        /// <summary>
        /// 移动主题时的目标版块Id
        /// </summary>
        public int moveto = DNTRequest.GetFormInt("moveto", 0);
        /// <summary>
        /// 移动方式
        /// </summary>
        public string type = DNTRequest.GetFormString("type"); //移动方式
        /// <summary>
        /// 帖子列表
        /// </summary>
        public DataTable postlist;
        /// <summary>
        /// 可用积分列表
        /// </summary>
        public DataTable scorelist;
        /// <summary>
        /// 主题鉴定类型列表
        /// </summary>
        public List<TopicIdentify> identifylist = Caches.GetTopicIdentifyCollection();
        /// <summary>
        /// 主题鉴定js数组
        /// </summary>
        public string identifyjsarray = Caches.GetTopicIdentifyFileNameJsArray();
        /// <summary>
        /// 主题分类选项
        /// </summary>
        public string topictypeselectoptions; //当前版块的主题类型选项
        /// <summary>
        /// 当前帖子评分日志列表
        /// </summary>
        public DataTable ratelog = new DataTable();
        /// <summary>
        /// 当前帖子评分日志记录数
        /// </summary>
        public int ratelogcount = 0;
        /// <summary>
        /// 当前的主题
        /// </summary>
        public TopicInfo topicinfo;
        /// <summary>
        /// opinion
        /// </summary>
        public int opinion = DNTRequest.GetInt("opinion", -1);
        /// <summary>
        /// 是否允许管理主题, 初始false为不允许
        /// </summary>
        protected bool ismoder = false;
        protected bool issubmit = false;
        /// <summary>
        /// 信息是否充满整个弹出窗
        /// </summary>
        public bool titlemessage = false;
        #endregion


        protected int RateIsReady = 0;
        private ForumInfo forum;
        //private int highlight = 0;
        public bool issendmessage = false;
        public bool isreason = false;

        protected override void ShowPage()
        {
            ValidatePermission();

            if (!BindTitle())
                return;
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        private void ValidatePermission()
        {
            if (userid == -1)
            {
                AddErrLine("请先登录.");
                return;
            }
            if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost()) || action == "")
            {
                AddErrLine("非法提交.");
                return;
            }

            UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(Users.GetUserInfo(userid).Groupid);
            switch (usergroupinfo.Reasonpm)
            {
                case 1: isreason = true; break;
                case 2: issendmessage = true; break;
                case 3:
                    isreason = true;
                    issendmessage = true;
                    break;
                default: break;
            }

            // 检查是否具有版主的身份
            ismoder = Moderators.IsModer(useradminid, userid, forumid);
            // 如果拥有管理组身份
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);

            if (!operation.Equals("rate") && !operation.Equals("bonus") && !operation.Equals("banpost") && !DNTRequest.GetString("operat").Equals("rate") && !DNTRequest.GetString("operat").Equals("bonus") && !DNTRequest.GetString("operat").Equals("banpost"))
            {
                // 如果所属管理组不存在
                if (admininfo == null)
                {
                    AddErrLine("你没有管理权限");
                    return;
                }
            }

            if (action == "")
            {
                AddErrLine("操作类型参数为空.");
                return;
            }
            if (forumid == -1)
            {
                AddErrLine("版块ID必须为数字.");
                return;
            }
            if (DNTRequest.GetFormString("topicid") != "" && !Topics.InSameForum(topiclist, forumid))
            {
                AddErrLine("无法对非本版块主题进行管理操作.");
                return;
            }

            displayorder = TopicAdmins.GetDisplayorder(topiclist);
            digest = TopicAdmins.GetDigest(topiclist);
            forum = Forums.GetForumInfo(forumid);
            forumname = forum.Name;
            topictypeselectoptions = Forums.GetCurrentTopicTypesOption(forum.Fid, forum.Topictypes);
            pagetitle = Utils.RemoveHtml(forumname);
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
            if (operation == "delposts")
                SetUrl(base.ShowForumAspxRewrite(forumid, 0));
            else
                SetUrl(DNTRequest.GetUrlReferrer());

            if (!Forums.AllowView(forum.Viewperm, usergroupid))
            {
                AddErrLine("您没有浏览该版块的权限.");
                return;
            }
            if (topiclist.CompareTo("") == 0)
            {
                AddErrLine("您没有选择主题或相应的管理操作.");
                return;
            }

            if (operation.CompareTo("") != 0)
            {
                if (!DoOperations(forum, admininfo, config.Reasonpm)) // DoOperations执行管理操作
                    return;

                ForumUtils.DeleteTopicCacheFile(topiclist); // 删除主题游客缓存
                issubmit = true;
            }

            if (action.CompareTo("moderate") != 0)
            {
                if ("delete,move,type,highlight,close,displayorder,digest,copy,split,merge,bump,repair,delposts,banpost".IndexOf(operation) == -1)
                {
                    AddErrLine("你无权操作此功能");
                    return;
                }
                operation = action;
            }
            else if (operation.CompareTo("") == 0)
            {
                operation = DNTRequest.GetString("operat");

                if (operation.CompareTo("") == 0)
                {
                    AddErrLine("您没有选择主题或相应的管理操作.");
                    return;
                }
            }
        }

        /// <summary>
        /// 绑定操作的标题
        /// </summary>
        /// <returns></returns>
        private bool BindTitle()
        {
            switch (operation)
            {
                case "split":
                    {
                        #region 分割主题
                        operationtitle = "分割主题";
                        if (Utils.StrToInt(topiclist, 0) <= 0)
                        {
                            AddErrLine(string.Format("您的身份 \"{0}\" 没有分割主题的权限.", usergroupinfo.Grouptitle));
                            return false;
                        }
                        postlist = Posts.GetPostListTitle(Utils.StrToInt(topiclist, 0));
                        if (postlist != null && postlist.Rows.Count > 0)
                        {
                            postlist.Rows[0].Delete();
                            postlist.AcceptChanges();
                        }
                        break;
                        #endregion
                    }
                case "rate":
                    {
                        #region 评分
                        operationtitle = "参与评分";
                        if (!CheckRatePermission()) return false;

                        string repost = TopicAdmins.CheckRateState(postidlist, userid);
                        if (config.Dupkarmarate != 1 && !repost.Equals("") && RateIsReady != 1)
                        {
                            AddErrLine("对不起,您不能对同一个帖子重复评分.");
                            return false;
                        }
                        scorelist = UserGroups.GroupParticipateScore(userid, usergroupid);
                        if (scorelist.Rows.Count < 1)
                        {
                            AddErrLine(string.Format("您的身份 \"{0}\" 没有设置评分范围或者今日可用评分已经用完", usergroupinfo.Grouptitle));
                            return false;
                        }
                        PostInfo postinfo = Posts.GetPostInfo(TypeConverter.StrToInt(topiclist), TypeConverter.StrToInt(postidlist));
                        if (postinfo == null)
                        {
                            AddErrLine("您没有选择要评分的帖子.");
                            return false;
                        }
                        poster = postinfo.Poster;
                        if (postinfo.Posterid == userid)
                        {
                            AddErrLine("您不能对自已的帖子评分.");
                            return false;
                        }
                        title = postinfo.Title;
                        topiclist = postinfo.Tid.ToString();
                        break;
                        #endregion
                    }
                case "cancelrate":
                    {
                        #region 取消评分
                        operationtitle = "撤销评分";
                        PostInfo postinfo = Posts.GetPostInfo(Utils.StrToInt(topiclist, 0), Utils.StrToInt(postidlist, 0));
                        if (postinfo == null)
                        {
                            AddErrLine("您没有选择要撤消评分的帖子");
                            return false;
                        }
                        if (!ismoder)
                        {
                            AddErrLine("您的身份 \"" + usergroupinfo.Grouptitle + "\" 没有撤消评分的权限.");
                            return false;
                        }

                        poster = postinfo.Poster;
                        title = postinfo.Title;
                        topiclist = postinfo.Tid.ToString();

                        ratelogcount = AdminRateLogs.RecordCount("pid = " + postidlist);
                        ratelog = AdminRateLogs.LogList(ratelogcount, 1, "pid = " + postidlist);
                        ratelog.Columns.Add("extcreditname", Type.GetType("System.String"));
                        DataTable scorePaySet = Scoresets.GetScoreSet();

                        //绑定积分名称属性
                        foreach (DataRow dr in ratelog.Rows)
                        {
                            int extcredits = Utils.StrToInt(dr["extcredits"].ToString(), 0);
                            if ((extcredits > 0) && (extcredits < 9) || scorePaySet.Columns.Count > extcredits + 1)
                                dr["extcreditname"] = scorePaySet.Rows[0][extcredits + 1].ToString();
                            else
                                dr["extcreditname"] = "";
                        }
                        break;
                        #endregion
                    }
                case "bonus":
                    {
                        #region 悬赏
                        operationtitle = "结帖";
                        int tid = Utils.StrToInt(topiclist, 0);
                        postlist = Posts.GetPostListTitle(tid);
                        if (postlist != null)
                        {
                            if (postlist.Rows.Count > 0)
                            {
                                postlist.Rows[0].Delete();
                                postlist.AcceptChanges();
                            }
                        }

                        if (postlist.Rows.Count == 0)
                        {
                            AddErrLine("无法对没有回复的悬赏进行结帖.");
                            return false;
                        }

                        topicinfo = Topics.GetTopicInfo(tid);
                        if (topicinfo.Special == 3)
                        {
                            AddErrLine("本主题的悬赏已经结束.");
                            return false;
                        }
                        break;
                        #endregion
                    }
                case "delete": operationtitle = "删除主题"; break;
                case "move": operationtitle = "移动主题"; break;
                case "type": operationtitle = "主题分类"; break;
                case "highlight": operationtitle = "高亮显示"; break;
                case "close": operationtitle = "关闭/打开主题"; break;
                case "displayorder": operationtitle = "置顶/解除置顶"; break;
                case "digest": operationtitle = "加入/解除精华 "; break;
                case "copy": operationtitle = "复制主题"; break;
                case "merge": operationtitle = "合并主题"; break;
                case "bump": operationtitle = "提升/下沉主题"; break;
                case "repair": operationtitle = "修复主题"; break;
                case "delposts": operationtitle = "批量删帖"; break;
                case "banpost": operationtitle = "单帖屏蔽"; break;
                case "identify": operationtitle = "鉴定主题"; break;
                default: operationtitle = "未知操作"; break;
            }
            return true;
        }

        /// <summary>
        /// 检查评分权限
        /// </summary>
        /// <returns></returns>
        private bool CheckRatePermission()
        {
            if (usergroupinfo.Raterange.Equals(""))
            {
                AddErrLine(string.Format("您的身份 \"{0}\" 没有评分的权限.", usergroupinfo.Grouptitle));
                return false;
            }
            else
            {
                bool hasExtcreditsCanRate = false;
                foreach (string roleByScoreType in usergroupinfo.Raterange.Split('|'))
                {
                    //数组结构:  扩展积分编号,参与评分,积分代号,积分名称,评分最小值,评分最大值,24小时最大评分数
                    //				0			1			2		3		4			5			6
                    if (Utils.StrToBool(roleByScoreType.Split(',')[1], false))
                        hasExtcreditsCanRate = true;
                }
                if (!hasExtcreditsCanRate)
                {
                    AddErrLine(string.Format("您的身份 \"{0}\" 没有评分的权限.", usergroupinfo.Grouptitle));
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 进行相关操作
        /// </summary>
        /// <param name="forum"></param>
        /// <param name="admininfo"></param>
        /// <param name="reasonpm"></param>
        /// <returns></returns>
        private bool DoOperations(ForumInfo forum, AdminGroupInfo admininfo, int reasonpm)
        {
            string operationName = "";
            string next = DNTRequest.GetFormString("next");
            string referer = Utils.InArray(operation, "delete,move") ? forumpath + Urls.ShowForumAspxRewrite(forumid, 1, forum.Rewritename) : DNTRequest.GetUrlReferrer();

            DataTable dt = null;

            #region DoOperation

            string reason = DNTRequest.GetString("reason");
            int sendmsg = DNTRequest.GetFormInt("sendmessage", 0);
            if (issendmessage && sendmsg == 0)
            {
                titlemessage = true;
                AddErrLine("操作必须发送短消息通知用户");
                return false;
            }

            if (!Utils.InArray(operation, "identify,bonus") && isreason)
            {
                if (Utils.StrIsNullOrEmpty(reason))
                {
                    titlemessage = true;
                    AddErrLine("操作原因不能为空");
                    return false;
                }
                else if (reason.Length > 200)
                {
                    titlemessage = true;
                    AddErrLine("操作原因不能多于200个字符");
                    return false;
                }
            }
            if (!Utils.InArray(operation, "delete,move,type,highlight,close,displayorder,digest,copy,split,merge,bump,repair,rate,cancelrate,delposts,identify,bonus,banpost"))
            {
                titlemessage = true;
                AddErrLine("未知的操作参数");
                return false;
            }

            //执行提交操作
            if (!Utils.StrIsNullOrEmpty(next.Trim()))
                referer = string.Format("topicadmin.aspx?action={0}&forumid={1}&topicid={2}", next, forumid, topiclist);

            int operationid = 0;
            bool istopic = false;
            string subjecttype;

            Dictionary<int, string> titleList = new Dictionary<int, string>();
            if (Utils.InArray(operation, "rate,delposts,banpost,cancelrate"))
            {
                dt = Posts.GetPostList(postidlist, topiclist);
                subjecttype = "帖子";
                foreach (DataRow dr in dt.Rows)
                {
                    titleList.Add(TypeConverter.ObjectToInt(dr["pid"]), dr["message"].ToString());
                }
            }
            else
            {
                dt = Topics.GetTopicList(topiclist, -1);
                istopic = true;
                subjecttype = "主题";
                foreach (DataRow dr in dt.Rows)
                {
                    titleList.Add(TypeConverter.ObjectToInt(dr["tid"]), dr["title"].ToString());
                }
            }

            #region switch operation
            switch (operation)
            {
                case "delete":
                    #region delete
                    operationName = "删除主题";
                    if (!DoDeleteOperation(forum))
                        return false;
                    operationid = 1;
                    break;
                    #endregion
                case "move":
                    #region move
                    operationName = "移动主题";
                    if (!DoMoveOperation())
                        return false;
                    operationid = 2;
                    break;
                    #endregion
                case "type":
                    #region type
                    operationName = "主题分类";
                    if (!DoTypeOperation())
                        return false;
                    operationid = 3;
                    break;
                    #endregion
                case "highlight":
                    #region highlight
                    operationName = "设置高亮";
                    if (!DoHighlightOperation())
                        return false;
                    operationid = 4;
                    break;
                    #endregion
                case "close":
                    #region close
                    operationName = "关闭主题/取消";
                    if (!DoCloseOperation())
                        return false;
                    operationid = 5;
                    break;
                    #endregion
                case "displayorder":
                    #region displayorder
                    operationName = "主题置顶/取消";
                    if (!DoDisplayOrderOperation(admininfo))
                        return false;
                    operationid = 6;
                    break;
                    #endregion
                case "digest": //设置精华
                    #region digest
                    operationName = "设置精华/取消";
                    if (!DoDigestOperation())
                        return false;
                    operationid = 7;
                    break;
                    #endregion
                case "copy": //复制主题";
                    #region copy
                    operationName = "复制主题";
                    if (!DoCopyOperation())
                        return false;
                    operationid = 8;
                    break;
                    #endregion
                case "split":
                    #region split
                    operationName = "分割主题";
                    if (!DoSplitOperation())
                        return false;
                    operationid = 9;
                    break;
                    #endregion
                case "merge":
                    #region merge
                    operationName = "合并主题";
                    if (!DoMergeOperation())
                        return false;
                    operationid = 10;
                    break;
                    #endregion
                case "bump": //提升主题
                    #region bump
                    operationName = "提升/下沉主题";
                    if (!DoBumpTopicsOperation())
                        return false;
                    operationid = 11;
                    break;
                    #endregion
                case "repair": //修复主题
                    #region repair
                    operationName = "修复主题";
                    if (!ismoder)
                    {
                        titlemessage = true;
                        AddErrLine("您没有修复主题的权限");
                        return false;
                    }
                    TopicAdmins.RepairTopicList(topiclist);
                    operationid = 12;
                    break;
                    #endregion
                case "rate":
                    #region rate
                    operationName = "帖子评分";
                    if (!DoRateOperation(reason))
                        return false;
                    operationid = 13;
                    break;
                    #endregion
                case "delposts":
                    #region delposts
                    operationName = "批量删帖";
                    int layer = 1;
                    bool flag = DoDelpostsOperation(reason, forum, ref layer);
                    if (layer == 0)
                        return true;
                    if (!flag)
                        return false;
                    operationid = 14;
                    break;
                    #endregion
                case "identify":
                    #region identify
                    operationName = "鉴定主题";
                    if (!DoIndentifyOperation())
                        return false;
                    operationid = 15;
                    break;
                    #endregion
                case "cancelrate":
                    #region cancelrate
                    operationName = "撤销评分";
                    if (!DoCancelRateOperation(reason))
                        return false;
                    operationid = 16;
                    break;
                    #endregion
                case "bonus":
                    #region bonus
                    operationName = "结帖";

                    if (!DoBonusOperation())
                        return false;
                    operationid = 16;
                    break;
                    #endregion
                case "banpost":
                    #region banpost
                    operationName = "屏蔽帖子";
                    if (!DoBanPostOperatopn())
                        return false;
                    operationid = 17;
                    break;
                    #endregion
                default: operationName = "未知操作"; break;
            }

            #endregion

            AddMsgLine(next.CompareTo("") == 0 ? "管理操作成功,现在将转入主题列表" : "管理操作成功,现在将转入后续操作");

            if ((!operation.Equals("rate") && !operation.Equals("split")) && config.Modworkstatus == 1)
            {
                if (postidlist.Equals(""))
                {
                    foreach (string tid in topiclist.Split(','))
                    {
                        string title = "";
                        titleList.TryGetValue(TypeConverter.StrToInt(tid), out title);
                        if (string.IsNullOrEmpty(title))
                        {
                            TopicInfo topicinfo = Topics.GetTopicInfo(Utils.StrToInt(tid, -1));
                            title = topicinfo == null ? title : topicinfo.Title;
                        }
                        AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(),
                                                       usergroupinfo.Grouptitle, Utils.GetRealIP(),
                                                       Utils.GetDateTime(), forumid.ToString(), forumname,
                                                       tid, title, operationName, reason);
                    }
                }
                else
                {
                    int topicId = Utils.StrToInt(topiclist, -1);
                    TopicInfo topicInfo = Topics.GetTopicInfo(topicId);

                    foreach (string postid in postidlist.Split(','))
                    {
                        PostInfo postinfo = new PostInfo();
                        string postMessage = titleList[Utils.StrToInt(postid, 0)];

                        subjecttype = "回复的主题";
                        string postTitle = postMessage.Replace(" ", "").Replace("|", "");
                        if (postTitle.Length > 100)
                            postTitle = postTitle.Substring(0, 20) + "...";
                        postTitle = "(pid:" + postid + ")" + postTitle;

                        if (operation != "delposts")
                        {
                            postinfo = Posts.GetPostInfo(topicId, Utils.StrToInt(postid, 0));
                            postTitle = postinfo == null ? postTitle : postinfo.Title;
                        }

                        AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(),
                                                     usergroupinfo.Grouptitle, Utils.GetRealIP(),
                                                     Utils.GetDateTime(), forumid.ToString(), forumname,
                                                     topicInfo.Tid.ToString(), postTitle, operationName, reason);
                    }
                }
            }
            SendMessage(operationid, dt, istopic, operationName, reason, sendmsg, subjecttype);

            //执行完某一操作后转到后续操作
            SetUrl(referer);
            if (next != string.Empty)
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + referer, false);
            else
                AddScript("window.setTimeout('redirectURL()', 2000);function redirectURL() {window.location='" + referer + "';}");

            SetShowBackLink(false);

            #endregion

            return true;
        }
        /// <summary>
        /// 返回被操作的帖子链接
        /// </summary>
        /// <param name="tid">帖子ID</param>
        /// <returns></returns>
        private string GetOperatePostUrl(int tid, string title)
        {
            return string.Format("[url={0}{1}]{2}[/url]", Utils.GetRootUrl(BaseConfigs.GetForumPath), Urls.ShowTopicAspxRewrite(tid, 1), title);
        }

        private void SendMessage(int operationid, DataTable dt, bool istopic, string operationName, string reason, int sendmsg, string subjecttype)
        {
            if (istopic)
                Topics.UpdateTopicModerated(topiclist, operationid);

            if (dt != null)
            {
                if (useradminid != 1 && ForumUtils.HasBannedWord(reason))
                {
                    AddErrLine(string.Format("您提交的内容包含不良信息 <font color=\"red\">{0}</font>", ForumUtils.GetBannedWord(reason)));
                    return;
                }
                else
                    reason = ForumUtils.BanWordFilter(reason);

                foreach (DataRow dr in dt.Rows)
                {
                    if (sendmsg == 1)
                        MessagePost(dr, operationName, subjecttype, reason);
                }
                dt.Dispose();
            }
        }

        #region Operations

        private void MessagePost(DataRow dr, string operationName, string subjecttype, string reason)
        {
            int posterid = Utils.StrToInt(dr["posterid"], -1);
            if (posterid == -1) //是游客，管理操作就不发短消息了
                return;
            string titlemsg = "";
            NoticeInfo ni = new NoticeInfo();
            ni.New = 1;
            ni.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ni.Type = NoticeType.TopicAdmin;
            ni.Poster = username;
            ni.Posterid = userid;
            ni.Uid = posterid;

            reason = string.IsNullOrEmpty(reason) ? reason : "理由:" + reason;

            if (subjecttype == "主题" || Utils.StrToInt(dr["layer"], -1) == 0)
            {
                titlemsg = operation != "delete" ? GetOperatePostUrl(int.Parse(dr["tid"].ToString()), dr["title"].ToString().Trim()) : dr["title"].ToString().Trim();
                ni.Note = Utils.HtmlEncode(string.Format("您发表的主题 {0} 被 {1} 执行了{2}操作 {3}", titlemsg, "<a href=\"" + UserInfoAspxRewrite(userid) + "\" target=\"_blank\" >" + username + "</a>", operationName, reason));
            }
            else
            {
                titlemsg = GetOperatePostUrl(int.Parse(dr["tid"].ToString()), Topics.GetTopicInfo(Utils.StrToInt(dr["tid"], 0)).Title);
                ni.Note = Utils.HtmlEncode(string.Format("您在 {0} 回复的帖子被 {1} 执行了{2}操作 {3}", titlemsg, "<a href=\"" + UserInfoAspxRewrite(userid) + "\" target=\"_blank\" >" + username + "</a>", operationName, reason));
            }
            Notices.CreateNoticeInfo(ni);
        }

        /// <summary>
        /// 评分
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        private bool DoRateOperation(string reason)
        {
            if (!CheckRatePermission())
                return false;

            if (Utils.StrIsNullOrEmpty(postidlist))
            {
                titlemessage = true;
                AddErrLine("您没有选择要评分的帖子");
                return false;
            }

            if (config.Dupkarmarate != 1 && AdminRateLogs.RecordCount(AdminRateLogs.GetRateLogCountCondition(userid, postidlist)) > 0)
            {
                titlemessage = true;
                AddErrLine("您不能对本帖重复评分");
                return false;
            }

            scorelist = UserGroups.GroupParticipateScore(userid, usergroupid);
            string[] scoreArr = Utils.SplitString(DNTRequest.GetFormString("score").Replace("+", ""), ",");
            string[] extcreditsArr = Utils.SplitString(DNTRequest.GetFormString("extcredits"), ",");
            string cscoreArr = "", cextcreditsArr = "";
            int arr = 0;
            for (int i = 0; i < scoreArr.Length; i++)
            {
                if (Utils.IsNumeric(scoreArr[i].ToString()) && scoreArr[i].ToString() != "0" && !scoreArr[i].ToString().Contains("."))
                {
                    cscoreArr = cscoreArr + scoreArr[i] + ",";
                    cextcreditsArr = cextcreditsArr + extcreditsArr[i] + ",";
                }
            }

            if (cscoreArr.Length == 0)
            {
                titlemessage = true;
                AddErrLine("分值超过限制.");
                return false;
            }

            foreach (DataRow scoredr in scorelist.Rows)
            {
                if (scoredr["ScoreCode"].ToString().Equals(extcreditsArr[arr]))
                {
                    if (Utils.StrToInt(scoredr["MaxInDay"], 0) < Math.Abs(Utils.StrToInt(scoreArr[arr], 0)) ||
                        Utils.StrToInt(scoredr["Max"], 0) < Utils.StrToInt(scoreArr[arr], 0) ||
                        Utils.StrToInt(scoreArr[arr], 0) != 0 && (Utils.StrToInt(scoredr["Min"], 0) > Utils.StrToInt(scoreArr[arr], 0)))
                    {
                        titlemessage = true;
                        AddErrLine("分值超过限制.");
                        return false;
                    }
                }
                arr++;
            }

            TopicAdmins.RatePosts(Utils.StrToInt(topiclist, 0), postidlist, cscoreArr, cextcreditsArr, userid, username, reason);
            Posts.UpdatePostRateTimes(Utils.StrToInt(topiclist, 0), postidlist);
            RateIsReady = 1;
            return true;
        }

        /// <summary>
        /// 撤消评分
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        private bool DoCancelRateOperation(string reason)
        {
            if (!CheckRatePermission())
                return false;

            if (postidlist.Equals(""))
            {
                titlemessage = true;
                AddErrLine("您未选择要撤销评分的帖子");
                return false;
            }
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有撤消评分的权限");
                return false;
            }
            if (DNTRequest.GetFormString("ratelogid").Equals(""))
            {
                titlemessage = true;
                AddErrLine("您未选择要撤销评分的记录");
                return false;
            }

            TopicAdmins.CancelRatePosts(DNTRequest.GetFormString("ratelogid"), Utils.StrToInt(topiclist, 0), postidlist, userid, username, usergroupinfo.Groupid, usergroupinfo.Grouptitle, forumid, forumname, reason);
            Posts.UpdatePostRateTimes(Utils.StrToInt(topiclist, 0), postidlist);
            return true;
        }

        /// <summary>
        /// 合并主题
        /// </summary>
        /// <returns></returns>
        private bool DoMergeOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有合并主题的权限");
                return false;
            }
            if (DNTRequest.GetFormInt("othertid", 0) == 0)
            {
                titlemessage = true;
                AddErrLine("您没有输入要合并的主题ID");
                return false;
            }
            //同一主题,不能合并
            if (DNTRequest.GetFormInt("othertid", 0) == Utils.StrToInt(topiclist, 0))
            {
                titlemessage = true;
                AddErrLine("不能对同一主题进行合并操作");
                return false;
            }

            if (Topics.GetTopicInfo(DNTRequest.GetFormInt("othertid", 0)) == null)
            {
                titlemessage = true;
                AddErrLine("目标主题不存在");
                return false;
            }
            //如果目标主题和当前主题的帖子不在同一个分表当中，则暂时设定不允许合并，看以后的解决方案
            if (Posts.GetPostTableId(DNTRequest.GetFormInt("othertid", 0)) != Posts.GetPostTableId(Utils.StrToInt(topiclist, 0)))
            {
                titlemessage = true;
                AddErrLine("不允许跨分表合并主题");
                return false;
            }

            TopicAdmins.MerrgeTopics(topiclist, DNTRequest.GetFormInt("othertid", 0));
            return true;
        }

        /// <summary>
        /// 分割主题
        /// </summary>
        /// <returns></returns>
        private bool DoSplitOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有分割主题的权限");
                return false;
            }
            if (DNTRequest.GetString("subject").Equals(""))
            {
                titlemessage = true;
                AddErrLine("您没有输入标题");
                return false;
            }
            if (DNTRequest.GetString("subject").Length > 60)
            {
                titlemessage = true;
                AddErrLine("标题长为60字以内");
                return false;
            }
            if (postidlist.Equals(""))
            {
                titlemessage = true;
                AddErrLine("请选择要分割入新主题的帖子");
                return false;
            }
            //如果分割的主题使用的帖子表不是当前最新分表,则暂时无法分割主题，待以后解决方案
            if (Posts.GetPostTableId(TypeConverter.StrToInt(topiclist)) != Posts.GetPostTableId())
            {
                titlemessage = true;
                AddErrLine("主题过旧,无法分割");
                return false;
            }
            TopicAdmins.SplitTopics(postidlist, DNTRequest.GetString("subject"), topiclist);
            return true;
        }

        /// <summary>
        /// 复制主题
        /// </summary>
        /// <returns></returns>
        private bool DoCopyOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有复制主题的权限");
                return false;
            }
            if (DNTRequest.GetFormInt("copyto", 0) == 0)
            {
                titlemessage = true;
                AddErrLine("您没有选择目标论坛/分类");
                return false;
            }

            TopicAdmins.CopyTopics(topiclist, DNTRequest.GetFormInt("copyto", 0));
            return true;
        }

        /// <summary>
        /// 精华操作
        /// </summary>
        /// <returns></returns>
        private bool DoDigestOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有设置精华的权限");
                return false;
            }
            digest = DNTRequest.GetFormInt("level", -1);
            if (digest > 3 || digest < 0)
            {
                digest = -1;
            }
            if (digest == -1)
            {
                titlemessage = true;
                AddErrLine("您没有选择精华级别");
                return false;
            }

            TopicAdmins.SetDigest(topiclist, short.Parse(digest.ToString()));
            return true;
        }

        /// <summary>
        /// 置顶操作
        /// </summary>
        /// <param name="admininfo"></param>
        /// <returns></returns>
        private bool DoDisplayOrderOperation(AdminGroupInfo admininfo)
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有置顶的管理权限");
                return false;
            }

            displayorder = DNTRequest.GetFormInt("level", -1);
            if (displayorder < 0 || displayorder > 3)
            {
                titlemessage = true;
                AddErrLine("置顶参数超出范围");
                return false;
            }
            // 检查用户所在管理组是否具有置顶的管理权限
            if (admininfo.Admingid != 1 && admininfo.Allowstickthread < displayorder)
            {
                titlemessage = true;
                AddErrLine(string.Format("您没有{0}级置顶的管理权限", displayorder));
                return false;
            }

            TopicAdmins.SetTopTopicList(forumid, topiclist, short.Parse(displayorder.ToString()));
            return true;
        }

        /// <summary>
        /// 关闭主题
        /// </summary>
        /// <returns></returns>
        private bool DoCloseOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有关闭主题的权限");
                return false;
            }
            if (DNTRequest.GetFormInt("close", -1) == -1)
            {
                titlemessage = true;
                AddErrLine("您没选择打开还是关闭");
                return false;
            }
            if (TopicAdmins.SetClose(topiclist, short.Parse(DNTRequest.GetFormInt("close", -1).ToString())) < 1)
            {
                titlemessage = true;
                AddErrLine("要操作的主题未找到");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 高亮主题
        /// </summary>
        /// <returns></returns>
        private bool DoHighlightOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有设置高亮的权限");
                return false;
            }

            string highlightStyle = "";

            //加粗
            if (!highlight_style_b.Equals(""))
                highlightStyle = highlightStyle + "font-weight:bold;";

            //加斜
            if (!highlight_style_i.Equals(""))
                highlightStyle = highlightStyle + "font-style:italic;";

            //加下划线
            if (!highlight_style_u.Equals(""))
                highlightStyle = highlightStyle + "text-decoration:underline;";

            //设置颜色
            if (!highlight_color.Equals(""))
                highlightStyle = highlightStyle + "color:" + highlight_color + ";";

            if (highlightStyle == "")
            {
                titlemessage = true;
                AddErrLine("您没有选择字体样式及颜色");
                return false;
            }

            TopicAdmins.SetHighlight(topiclist, highlightStyle);
            return true;
        }

        /// <summary>
        /// 修改主题分类
        /// </summary>
        /// <returns></returns>
        private bool DoTypeOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有修改主题分类的权限");
                return false;
            }
            if (DNTRequest.GetFormInt("typeid", 0) == 0)
            {
                titlemessage = true;
                AddErrLine("你没有选择相应的主题分类");
                return false;
            }

            TopicAdmins.ResetTopicTypes(DNTRequest.GetFormInt("typeid", 0), topiclist);
            return true;
        }

        /// <summary>
        /// 移动主题
        /// </summary>
        /// <returns></returns>
        private bool DoMoveOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有移动主题权限");
                return false;
            }
            if (moveto == 0 || type.CompareTo("") == 0 || ",normal,redirect,".IndexOf("," + type.Trim() + ",") == -1)
            {
                titlemessage = true;
                AddErrLine("您没选择分类或移动方式");
                return false;
            }
            if (moveto == forumid)
            {
                titlemessage = true;
                AddErrLine("主题不能在相同分类内移动");
                return false;
            }

            ForumInfo movetoinfo = Forums.GetForumInfo(moveto);
            if (movetoinfo == null)
            {
                titlemessage = true;
                AddErrLine("目标版块不存在");
                return false;
            }
            if (movetoinfo.Layer == 0)
            {
                titlemessage = true;
                AddErrLine("主题不能在分类间移动");
                return false;
            }

            int topicType = DNTRequest.GetInt("movetopictype", 0);
            bool isDefinedType = false;

            //检测当前获取的主题分类ID是否是目标版块定义的分类
            foreach (string t in movetoinfo.Topictypes.Split('|'))
            {
                if (topicType == TypeConverter.StrToInt(t.Split(',')[0]))
                {
                    isDefinedType = true;
                    break;
                }
            }

            topicType = isDefinedType ? topicType : 0;
            TopicAdmins.MoveTopics(topiclist, moveto, forumid, type.CompareTo("redirect") == 0, topicType);
            return true;
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="forum"></param>
        /// <returns></returns>
        private bool DoDeleteOperation(ForumInfo forum)
        {
            if (!ismoder || AdminGroups.GetAdminGroupInfo(useradminid).Allowdelpost != 1)
            {
                titlemessage = true;
                AddErrLine("您没有删除权限");
                return false;
            }
            if (Utils.SplitString(topiclist, ",", true).Length > 1 && AdminGroups.GetAdminGroupInfo(useradminid).Allowmassprune != 1)
            {
                titlemessage = true;
                AddErrLine("您没有批量删除权限");
                return false;
            }

            TopicAdmins.DeleteTopics(topiclist, byte.Parse(forum.Recyclebin.ToString()), DNTRequest.GetInt("reserveattach", 0) == 1);
            Forums.SetRealCurrentTopics(forum.Fid);
            //更新指定版块的最新发帖数信息
            Forums.UpdateLastPost(forum);
            return true;
        }

        /// <summary>
        /// 提升/下沉主题
        /// </summary>
        /// <returns></returns>
        private bool DoBumpTopicsOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有提升/下沉主题的权限");
                return false;
            }
            if (!Utils.IsNumericList(topiclist))
            {
                titlemessage = true;
                AddErrLine("非法的主题ID");
                return false;
            }
            if (Math.Abs(DNTRequest.GetFormInt("bumptype", 0)) != 1)
            {
                titlemessage = true;
                AddErrLine("错误的参数");
                return false;
            }

            TopicAdmins.BumpTopics(topiclist, DNTRequest.GetFormInt("bumptype", 0));
            return true;
        }

        /// <summary>
        /// 单帖屏蔽
        /// </summary>
        /// <returns></returns>
        private bool DoBanPostOperatopn()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有单帖屏蔽的权限");
                return false;
            }
            if (!Utils.IsNumeric(topiclist))
            {
                titlemessage = true;
                AddErrLine("无效的主题ID");
                return false;
            }

            TopicInfo topic = Topics.GetTopicInfo(Utils.StrToInt(topiclist, 0));
            if (topic == null)
            {
                titlemessage = true;
                AddErrLine("不存在的主题");
                return false;
            }
            if (!Utils.IsNumericList(postidlist))
            {
                titlemessage = true;
                AddErrLine("非法的帖子ID");
                return false;
            }
            return Posts.BanPosts(topic.Tid, postidlist, DNTRequest.GetFormInt("banpost", -1));
            //int banposttype = DNTRequest.GetFormInt("banpost", -1);
            //if (banposttype != -1 && (banposttype == 0 || banposttype == -2))
            //{
            //    Posts.BanPosts(topic.Tid, postidlist, banposttype);
            //    return true;
            //}

            //return false;
        }

        /// <summary>
        /// 批量删帖
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="forum"></param>
        /// <returns></returns>
        private bool DoDelpostsOperation(string reason, ForumInfo forum, ref int layer)
        {
            if (!ismoder || AdminGroups.GetAdminGroupInfo(useradminid).Allowdelpost != 1)
            {
                titlemessage = true;
                AddErrLine("您没有批量删帖的权限");
                return false;
            }
            if (Utils.SplitString(postidlist, ",", true).Length > 1 && AdminGroups.GetAdminGroupInfo(useradminid).Allowmassprune != 1)
            {
                titlemessage = true;
                AddErrLine("您没有批量删除的权限");
                return false;
            }
            if (!Utils.IsNumeric(topiclist))
            {
                titlemessage = true;
                AddErrLine("无效的主题ID");
                return false;
            }

            TopicInfo topic = Topics.GetTopicInfo(Utils.StrToInt(topiclist, 0));
            if (topic == null)
            {
                titlemessage = true;
                AddErrLine("不存在的主题");
                return false;
            }
            if (!Utils.IsNumericList(postidlist))
            {
                titlemessage = true;
                AddErrLine("非法的帖子ID");
                return false;
            }

            bool flag = false;
            foreach (string postid in postidlist.Split(','))
            {
                PostInfo post = Posts.GetPostInfo(topic.Tid, Utils.StrToInt(postid, 0));
                if (post == null || (post.Layer <= 0 && topic.Replies > 0) || topic.Tid != post.Tid)
                {
                    titlemessage = true;
                    AddErrLine("主题无效或者已被回复");
                    return false;
                }
                // 通过验证的用户可以删除帖子
                if (post.Layer == 0)
                {
                    TopicAdmins.DeleteTopics(topic.Tid.ToString(), byte.Parse(forum.Recyclebin.ToString()), DNTRequest.GetInt("reserveattach", 0) == 1);
                    layer = 0;
                    break;
                }
                else
                {
                    int reval = Posts.DeletePost(Posts.GetPostTableId(topic.Tid), post.Pid, DNTRequest.GetInt("reserveattach", 0) == 1, true);
                    if (topic.Special == 4)
                    {
                        if (opinion != 1 && opinion != 2)
                        {
                            titlemessage = true;
                            AddErrLine("参数错误");
                            return false;
                        }

                        string opiniontext = "";
                        switch (opinion)
                        {
                            case 1: opiniontext = "positivediggs"; break;
                            case 2: opiniontext = "negativediggs"; break;
                        }
                        Discuz.Data.DatabaseProvider.GetInstance().DeleteDebatePost(topic.Tid, opiniontext, Utils.StrToInt(postid, -1));
                    }
                    //if (reval > 0 && (config.Losslessdel == 0 || Utils.StrDateDiffHours(post.Postdatetime, config.Losslessdel * 24) < 0))
                    //    UserCredits.UpdateUserCreditsByDeletePosts(post.Posterid);
                }
                flag = true;
            }
            //确保回复数精确
            Topics.UpdateTopicReplyCount(topic.Tid);
            //更新指定版块的最新发帖数信息
            Forums.UpdateLastPost(forum);
            return flag;
        }

        /// <summary>
        /// 鉴定主题
        /// </summary>
        /// <returns></returns>
        private bool DoIndentifyOperation()
        {
            if (!ismoder)
            {
                titlemessage = true;
                AddErrLine("您没有鉴定主题的权限");
                return false;
            }

            int identify = DNTRequest.GetInt("selectidentify", 0);
            if (identify > 0 || identify == -1)
            {
                TopicAdmins.IdentifyTopic(topiclist, identify);
                return true;
            }
            else
            {
                titlemessage = true;
                AddErrLine("请选择签定类型");
                return false;
            }
        }

        /// <summary>
        /// 悬赏结帖
        /// </summary>
        /// <returns></returns>
        private bool DoBonusOperation()
        {
            //身份验证
            topicinfo = Topics.GetTopicInfo(DNTRequest.GetInt("topicid", 0));

            if (topicinfo.Special == 3)
            {
                titlemessage = true;
                AddErrLine("本主题的悬赏已经结束");
                return false;
            }
            if (topicinfo.Posterid <= 0)
            {
                titlemessage = true;
                AddErrLine("无法结束游客发布的悬赏");
                return false;
            }
            if (topicinfo.Posterid != userid && !ismoder)//不是作者或管理者
            {
                titlemessage = true;
                AddErrLine("您没有权限结束此悬赏");
                return false;
            }

            int costBonus = 0;
            string[] costBonusArray = DNTRequest.GetString("postbonus").Split(',');

            foreach (string s in costBonusArray)
            {
                costBonus += Utils.StrToInt(s, 0);
            }

            if (costBonus != topicinfo.Price)
            {
                titlemessage = true;
                AddErrLine("获奖分数与悬赏分数不一致");
                return false;
            }

            string[] addonsArray = DNTRequest.GetFormString("addons").Split(',');
            int[] winneridArray = new int[addonsArray.Length];
            int[] postidArray = new int[addonsArray.Length];
            string[] winnernameArray = new string[addonsArray.Length];

            foreach (string addon in addonsArray)
            {
                if (Utils.StrToInt(addon.Split('|')[0], 0) == topicinfo.Posterid)
                {
                    titlemessage = true;
                    AddErrLine("不能向悬赏者发放积分奖励");
                    return false;
                }
            }

            if (costBonusArray.Length != addonsArray.Length)
            {
                titlemessage = true;
                AddErrLine("获奖者数量与积分奖励数量不一致");
                return false;
            }

            if (IsErr()) return false;

            for (int i = 0; i < addonsArray.Length; i++)
            {
                winneridArray[i] = Utils.StrToInt(addonsArray[i].Split('|')[0], 0);
                postidArray[i] = Utils.StrToInt(addonsArray[i].Split('|')[1], 0);
                winnernameArray[i] = addonsArray[i].Split('|')[2];
            }
            Bonus.CloseBonus(topicinfo, userid, postidArray, winneridArray, winnernameArray, costBonusArray, DNTRequest.GetFormString("valuableAnswers").Split(','), DNTRequest.GetFormInt("bestAnswer", 0));
            //发通知给得分用户
            if (DNTRequest.GetFormInt("sendmessage", 0) == 0)
                return true;
            for (int i = 0; i < winneridArray.Length; i++)
            {
                int bonus = TypeConverter.StrToInt(costBonusArray[i]);
                if (bonus != 0)
                {
                    BonusPostMessage(topicinfo, postidArray[i], winneridArray[i], postidArray[i] == DNTRequest.GetFormInt("bestAnswer", 0), bonus);
                }
            }
            return true;
        }

        private void BonusPostMessage(TopicInfo topicInfo, int pid, int answerUid, bool isBeta, int num)
        {
            NoticeInfo ni = new NoticeInfo();
            ni.New = 1;
            ni.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ni.Type = NoticeType.TopicAdmin;
            ni.Poster = username;
            ni.Posterid = userid;
            ni.Uid = answerUid;
            ni.Fromid = pid;
            ni.Note = Utils.HtmlEncode(string.Format("您发表的 {0} 被 {1} 评为 {2} ,给予 {3}{4} 奖励",
                "<a href=\"" + Urls.ShowTopicAspxRewrite(topicInfo.Tid, 0, topicInfo.Typeid) + "#" + pid + "\" target=\"_blank\" >回帖</a>",
                "<a href=\"" + UserInfoAspxRewrite(userid) + "\" target=\"_blank\" >" + username + "</a>",
                isBeta ? "最佳答案" : "有价值的答案", num, Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans()).Name));
            Notices.CreateNoticeInfo(ni);
        }

        protected string TransAttachImgUbb(string message)
        {
            return Regex.Replace(message, @"\[attachimg\](\d+)\[/attachimg\]", "<a href='javascript:void(0)' aid='$1' class='floatimg'>[图片]</a><img id='img$1' src='attachment.aspx?attachmentid=$1' width='150' style='display:none;' />",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
        #endregion
    }
}
