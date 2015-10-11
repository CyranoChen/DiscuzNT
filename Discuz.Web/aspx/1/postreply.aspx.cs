using System;
using System.Data;
using System.Text;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Plugin.Album;
using System.Web;

namespace Discuz.Web
{
    /// <summary>
    /// 回复页面类
    /// </summary>
    public class postreply : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 主题信息
        /// </summary>
        public TopicInfo topic = new TopicInfo();
        /// <summary>
        /// 帖子信息
        /// </summary>
        public PostInfo postinfo = new PostInfo();
        /// <summary>
        /// 是否为主题帖
        /// </summary>
        public bool isfirstpost = false;
        /// <summary>
        /// 所属版块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 主题Id
        /// </summary>
        public int topicid = DNTRequest.GetInt("topicid", -1);
        /// <summary>
        /// 帖子Id
        /// </summary>
        public int postid = DNTRequest.GetInt("postid", -1);
        /// <summary>
        /// 回复内容
        /// </summary>
        public string message = "";
        /// <summary>
        /// 主题标题
        /// </summary>
        public string topictitle = "";
        /// <summary>
        /// 是否解析网址
        /// </summary>
        public int parseurloff = 0;
        /// <summary>
        /// 是否解析表情
        /// </summary>
        public int smileyoff;
        /// <summary>
        /// 是否解析 Discuz!NT 代码
        /// </summary>
        public int bbcodeoff;
        /// <summary>
        /// 是否使用签名
        /// </summary>
        public int usesig = ForumUtils.GetCookie("sigstatus") == "0" ? 0 : 1;
        /// <summary>
        /// 是否允许 [img] 代码
        /// </summary>
        public int allowimg;
        /// <summary>
        /// 是否受灌水限制
        /// </summary>
        public int disablepost = 0;
        /// <summary>
        /// 允许的附件类型和大小数组
        /// </summary>
        public string attachextensions;
        /// <summary>
        /// 允许的附件类型
        /// </summary>
        public string attachextensionsnosize;
        /// <summary>
        /// 今天可上传附件大小
        /// </summary>
        public int attachsize;
        /// <summary>
        /// 继续进行回复
        /// </summary>
        public string continuereply = (DNTRequest.IsPost()) ? "" : DNTRequest.GetQueryString("continuereply");
        /// <summary>
        /// 所属版块信息
        /// </summary>
        public ForumInfo forum = new ForumInfo();
        /// <summary>
        /// 当前用户相册列表
        /// </summary>
        public DataTable albumlist;
        /// <summary>
        /// 是否允许插入到相册
        /// </summary>
        public bool caninsertalbum = false;
        /// <summary>
        /// 是否允许上传附件
        /// </summary>
        public bool canpostattach = false;
        /// <summary>
        /// 是否需要登录
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// 当前版块的分页id
        /// </summary>
        public int forumpageid = DNTRequest.GetInt("forumpage", 1);
        /// <summary>
        /// 开启html功能
        /// </summary>
        public int htmlon = 0;
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public ShortUserInfo userinfo = new ShortUserInfo();
        /// <summary>
        /// 权限校验提示信息
        /// </summary>
        string msg = "";
        /// <summary>
        /// 帖子标题信息
        /// </summary>
        string posttitle = DNTRequest.GetString(GeneralConfigs.GetConfig().Antispamposttitle);
        /// <summary>
        /// 帖子内容信息
        /// </summary>
        string postmessage = DNTRequest.GetString(GeneralConfigs.GetConfig().Antispampostmessage);
        /// <summary>
        /// 标签
        /// </summary>
        public string topictags = "";
        /// <summary>
        /// 主题附件购买积分策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo = new UserExtcreditsInfo();
        /// <summary>
        /// 是否允许Html标题
        /// </summary>
        public bool canhtmltitle = false;
        /// <summary>
        /// 当前帖的Html标题
        /// </summary>
        public string htmltitle = "";
        /// <summary>
        /// 主题分类选项字串
        /// </summary>
        public string topictypeselectoptions = "";
        /// <summary>
        /// 本版是否可用Tag
        /// </summary>
        public bool enabletag = false;
        /// <summary>
        /// 附件列表
        /// </summary>
        public DataTable attachmentlist = new DataTable();
        /// <summary>
        /// 用户组列表
        /// </summary>
        public Discuz.Common.Generic.List<UserGroupInfo> userGroupInfoList = UserGroups.GetUserGroupList();
        public bool needaudit = false;
        /// <summary>
        /// 编辑器自定义按钮
        /// </summary>
        public string customeditbuttons;
        #endregion
        AlbumPluginBase apb = AlbumPluginProvider.GetInstance();

        protected override void ShowPage()
        {
            #region 临时帐号发帖
            //int realuserid = -1;
            //bool tempaccountspost = false;
            //string tempusername = DNTRequest.GetString("tempusername");
            //if (!Utils.StrIsNullOrEmpty(tempusername) && tempusername != username)
            //{
            //    realuserid = Users.CheckTempUserInfo(tempusername, DNTRequest.GetString("temppassword"), DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"));
            //    if (realuserid == -1)
            //    {
            //        AddErrLine("临时帐号登录失败，无法继续发帖。");
            //        return;
            //    }
            //    else
            //    {
            //        userid = realuserid;
            //        username = tempusername;
            //        tempaccountspost = true;
            //    }
            //}
            #endregion

            if (userid > 0)
                userinfo = Users.GetShortUserInfo(userid);

            #region 判断是否是灌水
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            if (admininfo != null)
                disablepost = admininfo.Disablepostctrl;

            if (!UserAuthority.CheckPostTimeSpan(usergroupinfo, admininfo, oluserinfo, userinfo, ref msg))
            {
                if (continuereply != "")
                    AddErrLine("<b>回帖成功</b><br />由于" + msg + "后刷新继续");
                else
                    AddErrLine(msg);
                return;
            }
            #endregion

            //获取主题帖信息 
            PostInfo postinfo = GetPostAndTopic(admininfo);
            if (IsErr())
                return;

            forum = Forums.GetForumInfo(forumid);
            smileyoff = 1 - forum.Allowsmilies;
            bbcodeoff = (forum.Allowbbcode == 1 && usergroupinfo.Allowcusbbcode == 1) ? 0 : 1;
            allowimg = forum.Allowimgcode;
            needaudit = UserAuthority.NeedAudit(forum, useradminid, topic, userid, disablepost, usergroupinfo);
            #region  附件信息绑定
            //得到用户可以上传的文件类型
            string attachmentTypeSelect = Attachments.GetAllowAttachmentType(usergroupinfo, forum);
            attachextensions = Attachments.GetAttachmentTypeArray(attachmentTypeSelect);
            attachextensionsnosize = Attachments.GetAttachmentTypeString(attachmentTypeSelect);
            //得到今天允许用户上传的附件总大小(字节)
            int MaxTodaySize = (userid > 0 ? MaxTodaySize = Attachments.GetUploadFileSizeByuserid(userid) : 0);
            attachsize = usergroupinfo.Maxsizeperday - MaxTodaySize;//今天可上传得大小
            //是否有上传附件的权限
            canpostattach = UserAuthority.PostAttachAuthority(forum, usergroupinfo, userid, ref msg);

            if (canpostattach && (userinfo != null && userinfo.Uid > 0) && apb != null && config.Enablealbum == 1 &&
            (UserGroups.GetUserGroupInfo(userinfo.Groupid).Maxspacephotosize - apb.GetPhotoSizeByUserid(userid) > 0))
            {
                caninsertalbum = true;
                albumlist = apb.GetSpaceAlbumByUserId(userid);
            }
            #endregion

            if (!Utils.StrIsNullOrEmpty(forum.Password) && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                SetBackLink(base.ShowForumAspxRewrite(forumid, 0));
                return;
            }

            #region 访问和发帖权限校验
            if (!UserAuthority.VisitAuthority(forum, usergroupinfo, userid, ref msg))
            {
                AddErrLine(msg);
                needlogin = true;
                return;
            }
            if (!UserAuthority.PostReply(forum, userid, usergroupinfo, topic))
            {
                AddErrLine(topic.Closed == 1 ? "主题已关闭无法回复" : "您没有发表回复的权限");
                needlogin = (topic.Closed == 1 ? false : true);
                return;
            }

            if (!UserAuthority.CheckPostTimeSpan(usergroupinfo, admininfo, oluserinfo, userinfo, ref msg))
            {
                AddErrLine(msg);
                return;
            }
            #endregion

            // 如果是受灌水限制用户, 则判断是否是灌水
            if (admininfo != null)
                disablepost = admininfo.Disablepostctrl;

            if (forum.Templateid > 0)
                templatepath = Templates.GetTemplateItem(forum.Templateid).Directory;

            AddLinkCss(BaseConfigs.GetForumPath + "templates/" + templatepath + "/editor.css", "css");
            customeditbuttons = Caches.GetCustomEditButtonList();
            //如果是提交...
            if (ispost)
            {
                string backlink = (DNTRequest.GetInt("topicid", -1) > 0 ?
                        string.Format("postreply.aspx?topicid={0}&restore=1&forumpage=" + forumpageid, topicid) :
                        string.Format("postreply.aspx?postid={0}&restore=1&forumpage=" + forumpageid, postid));

                if (!DNTRequest.GetString("quote").Equals(""))
                    backlink = string.Format("{0}&quote={1}", backlink, DNTRequest.GetString("quote"));

                SetBackLink(backlink);

                #region 验证提交信息
                //常规项验证
                NormalValidate(admininfo, postmessage, userinfo);

                if (IsErr())
                    return;
                #endregion

                //是否有上传附件的权限
                canpostattach = UserAuthority.PostAttachAuthority(forum, usergroupinfo, userid, ref msg);

                // 产生新帖子
                if (!string.IsNullOrEmpty(DNTRequest.GetFormString("toreplay_user").Trim()))
                    postmessage = DNTRequest.GetFormString("toreplay_user").Trim() + "\n\n" + postmessage;

                postinfo = CreatePostInfo(postmessage);

                //获取被回复帖子的作者uid
                int replyUserid = postid > 0 ? Posts.GetPostInfo(topicid, postid).Posterid : postinfo.Posterid;
                postid = postinfo.Pid;
                if (IsErr())
                    return;

                #region 当回复成功后，发送通知
                if (postinfo.Pid > 0 && DNTRequest.GetString("postreplynotice") == "on")
                    Notices.SendPostReplyNotice(postinfo, topic, replyUserid);
                #endregion

                //向第三方应用同步数据
                Sync.Reply(postid.ToString(), topic.Tid.ToString(), topic.Title, postinfo.Poster, postinfo.Posterid.ToString(), topic.Fid.ToString(), "");

                //更新主题相关信息
                //UpdateTopicInfo(postmessage);

                #region 处理附件
                //处理附件
                StringBuilder sb = new StringBuilder();
                AttachmentInfo[] attachmentinfo = null;
                string attachId = DNTRequest.GetFormString("attachid");
                if (!string.IsNullOrEmpty(attachId))
                {
                    attachmentinfo = Attachments.GetNoUsedAttachmentArray(userid, attachId);
                    Attachments.UpdateAttachment(attachmentinfo, topic.Tid, postinfo.Pid, postinfo, ref sb, userid, config, usergroupinfo);
                }

                //加入相册
                if (config.Enablealbum == 1 && apb != null)
                    sb.Append(apb.CreateAttachment(attachmentinfo, usergroupid, userid, username));
                #endregion

                OnlineUsers.UpdateAction(olid, UserAction.PostReply.ActionID, forumid, forum.Name, topicid, topictitle);

                #region 设置提示信息和跳转链接
                //辩论地址
                if (topic.Special == 4)
                    SetUrl(Urls.ShowDebateAspxRewrite(topicid));
                else if (infloat == 0)//此处加是否弹窗提交判断是因为在IE6下弹窗提交会造成gettopicinfo, getpostlist(位于showtopic页面)被提交了两次
                    SetUrl(string.Format("showtopic.aspx?forumpage={0}&topicid={1}&page=end&jump=pid#{2}", forumpageid, topicid, postid));

                if (DNTRequest.GetFormString("continuereply") == "on")
                    SetUrl("postreply.aspx?topicid=" + topicid + "&forumpage=" + forumpageid + "&continuereply=yes");

                if (sb.Length > 0)
                {
                    UpdateUserCredits(Forums.GetValues(forum.Replycredits));
                    SetMetaRefresh(5);
                    SetShowBackLink(true);
                    if (infloat == 1)
                    {
                        AddErrLine(sb.ToString());
                        return;
                    }
                    else
                        AddMsgLine("<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\"><tr><td colspan=2 align=\"left\"><span class=\"bold\"><nobr>发表回复成功,但图片/附件上传出现问题:</nobr></span><br /></td></tr></table>");
                }
                else
                {
                    SetMetaRefresh();
                    SetShowBackLink(false);
                    //上面已经进行用户组判断
                    if (postinfo.Invisible == 1)
                        AddMsgLine(string.Format("发表回复成功, 但需要经过审核才可以显示. {0}<br /><br />(<a href=\"" + base.ShowForumAspxRewrite(forumid, 0) + "\">点击这里返回 {1}</a>)", (DNTRequest.GetFormString("continuereply") == "on" ? "继续回复" : "返回该主题"), forum.Name));
                    else
                    {
                        UpdateUserCredits(Forums.GetValues(forum.Replycredits));
                        MsgForward("postreply_succeed");
                        AddMsgLine(string.Format("发表回复成功, {0}<br />(<a href=\"" + base.ShowForumAspxRewrite(forumid, 0) + "\">点击这里返回 {1}</a>)<br />", (DNTRequest.GetFormString("continuereply") == "on" ? "继续回复" : "返回该主题"), forum.Name));
                    }
                }
                #endregion

                // 删除主题游客缓存
                if (topic.Replies < (config.Ppp + 10))
                    ForumUtils.DeleteTopicCacheFile(topicid);

                //发送邮件通知
                if (DNTRequest.GetString("emailnotify") == "on" && topic.Posterid != -1 && topic.Posterid != userid)
                    SendNotifyEmail(Users.GetShortUserInfo(topic.Posterid).Email.Trim(), postinfo, Utils.GetRootUrl(BaseConfigs.GetForumPath) + string.Format("showtopic.aspx?topicid={0}&page=end&jump=pid#{1}", topicid, postid));
            }
        }

        /// <summary>
        /// 更新主题相关信息
        /// </summary>
        /// <param name="postmessage"></param>
        private void UpdateTopicInfo(string postmessage)
        {
            int hide = (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1) ? 1 : 0;
            if (hide == 1 && topic.Hide != 1)
            {
                topic.Hide = hide;
                Topics.UpdateTopicHide(topicid);
            }

            //更新topic的attention为0      
            if (Moderators.IsModer(useradminid, userid, topic.Fid) && topic.Attention == 1)
                Topics.UpdateTopicAttentionByTidList(topicid.ToString(), 0);
            else if (topic.Posterid != -1 && userid == topic.Posterid)
                Topics.UpdateTopicAttentionByTidList(topicid.ToString(), 1);

            Topics.UpdateTopicReplyCount(topicid);
        }

        /// <summary>
        /// 常规项验证
        /// </summary>
        /// <param name="admininfo"></param>
        /// <param name="postmessage"></param>
        private void NormalValidate(AdminGroupInfo admininfo, string postmessage, ShortUserInfo user)
        {
            if (ForumUtils.IsCrossSitePost())
            {
                AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                return;
            }
            else if (posttitle.Length > 60)
                AddErrLine("标题最大长度为60个字符,当前为 " + posttitle.Length + " 个字符");

            if (Utils.StrIsNullOrEmpty(postmessage.Replace("　", "")))
                AddErrLine("内容不能为空");

            if (admininfo != null && admininfo.Disablepostctrl != 1)
            {
                if (postmessage.Length < config.Minpostsize)
                    AddErrLine("您发表的内容过少, 系统设置要求帖子内容不得少于 " + config.Minpostsize + " 字多于 " + config.Maxpostsize + " 字");
                else if (postmessage.Length > config.Maxpostsize)
                    AddErrLine("您发表的内容过多, 系统设置要求帖子内容不得少于 " + config.Minpostsize + " 字多于 " + config.Maxpostsize + " 字");
            }

            if (topic.Special == 4 && DNTRequest.GetInt("debateopinion", 0) == 0)
                AddErrLine("请选择您在辩论中的观点");

            if (topic.Special == 4)
            {
                DebateInfo debateexpand = Debates.GetDebateTopic(topic.Tid);
                if (debateexpand.Terminaltime < DateTime.Now)
                    AddErrLine("此辩论主题已经到期");
            }

            //新用户广告强力屏蔽检查
            if ((config.Disablepostad == 1) && useradminid < 1)  //如果开启新用户广告强力屏蔽检查或是游客
            {
                if ((config.Disablepostadpostcount != 0 && user.Posts <= config.Disablepostadpostcount) ||
                    (config.Disablepostadregminute != 0 && DateTime.Now.AddMinutes(-config.Disablepostadregminute) <= Convert.ToDateTime(user.Joindate)))
                {
                    foreach (string regular in config.Disablepostadregular.Replace("\r", "").Split('\n'))
                    {
                        if (Posts.IsAD(regular, posttitle, postmessage))
                        {
                            AddErrLine("发帖失败，内容中有不符合新用户强力广告屏蔽规则的字符，请检查标题和内容，如有疑问请与管理员联系");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建帖子信息
        /// </summary>
        /// <param name="postmessage"></param>
        /// <returns></returns>
        public PostInfo CreatePostInfo(string postmessage)
        {
            PostInfo postinfo = new PostInfo();
            postinfo.Fid = forumid;
            postinfo.Tid = topicid;
            postinfo.Parentid = postinfo.Parentid;
            postinfo.Layer = postinfo.Layer + 1;
            postinfo.Poster = username;
            postinfo.Posterid = userid;
            bool ishtmlon = (Utils.StrToInt(DNTRequest.GetString("htmlon"), 0) == 1);
            if (useradminid == 1)
            {
                postinfo.Title = Utils.HtmlEncode(posttitle);
                if (usergroupinfo.Allowhtml == 0)
                    postinfo.Message = Utils.HtmlEncode(postmessage);
                else
                    postinfo.Message = ishtmlon ? postmessage : Utils.HtmlEncode(postmessage);
            }
            else
            {
                postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString(config.Antispamposttitle)));
                if (usergroupinfo.Allowhtml == 0)
                    postinfo.Message = Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
                else
                    postinfo.Message = ishtmlon ? ForumUtils.BanWordFilter(postmessage) :
                            Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
            }
            postinfo.Postdatetime = Utils.GetDateTime();

            if (Utils.StrIsNullOrEmpty(postinfo.Message.Replace("　", "")))
            {
                AddErrLine("内容不能为空, 请返回修改!");
                return postinfo;
            }

            if (useradminid != 1 && (ForumUtils.HasBannedWord(posttitle) || ForumUtils.HasBannedWord(postmessage)))
            {
                string bannedWord = ForumUtils.GetBannedWord(posttitle) == string.Empty ? ForumUtils.GetBannedWord(postmessage) : ForumUtils.GetBannedWord(posttitle);
                AddErrLine(string.Format("对不起, 您提交的内容包含不良信息  <font color=\"red\">{0}</font>, 请返回修改!", bannedWord));
                return postinfo;
            }

            postinfo.Ip = DNTRequest.GetIP();
            postinfo.Lastedit = "";
            postinfo.Debateopinion = DNTRequest.GetInt("debateopinion", 0);
            postinfo.Invisible = needaudit ? 1 : 0;

            //　如果当前用户非管理员并且论坛设定了发帖审核时间段，当前时间如果在其中的一个时间段内，则用户所发帖均为待审核状态
            if (useradminid != 1 && !Moderators.IsModer(useradminid, userid, forumid))
            {
                if (Scoresets.BetweenTime(config.Postmodperiods) || ForumUtils.HasAuditWord(postinfo.Title) || ForumUtils.HasAuditWord(postinfo.Message))
                    postinfo.Invisible = 1;
            }

            postinfo.Usesig = TypeConverter.StrToInt(DNTRequest.GetString("usesig"));
            postinfo.Htmlon = (usergroupinfo.Allowhtml == 1 && ishtmlon) ? 1 : postinfo.Htmlon;
            postinfo.Smileyoff = (smileyoff != 0) ? smileyoff : TypeConverter.StrToInt(DNTRequest.GetString("smileyoff"));
            postinfo.Bbcodeoff = (usergroupinfo.Allowcusbbcode == 1 && forum.Allowbbcode == 1) ? TypeConverter.StrToInt(DNTRequest.GetString("bbcodeoff")) : 1;
            postinfo.Parseurloff = TypeConverter.StrToInt(DNTRequest.GetString("parseurloff"));
            postinfo.Attachment = 0;
            postinfo.Rate = 0;
            postinfo.Ratetimes = 0;
            postinfo.Topictitle = topic.Title;

            if ((postinfo.Title != "" && Utils.GetCookie("lastposttitle") == Utils.MD5(postinfo.Title)) || Utils.GetCookie("lastpostmessage") == Utils.MD5(postinfo.Message))
            {
                AddErrLine("请勿重复发帖");
                return postinfo;
            }
            postinfo.Pid = Posts.CreatePost(postinfo);
            Utils.WriteCookie("lastposttitle", Utils.MD5(postinfo.Title));
            Utils.WriteCookie("lastpostmessage", Utils.MD5(postinfo.Message));
            ForumUtils.WriteCookie("clearUserdata", "forum");
            return postinfo;
        }

        /// <summary>
        /// 获取主题帖信息
        /// </summary>
        /// <param name="admininfo"></param>
        /// <returns></returns>
        public PostInfo GetPostAndTopic(AdminGroupInfo admininfo)
        {
            PostInfo postinfo = new PostInfo();

            //如果帖子id和主题id都没有指定
            if (postid == -1 && topicid == -1)
            {
                AddErrLine("无效的主题ID");
                return postinfo;
            }

            //如果帖子id被指定
            if (postid != -1)
            {
                postinfo = Posts.GetPostInfo(topicid, postid);
                if (postinfo == null)
                {
                    AddErrLine("无效的帖子ID");
                    return postinfo;
                }
                if (topicid != postinfo.Tid)
                {
                    AddErrLine("主题ID无效");
                    return postinfo;
                }

                //如果帖子作者是禁止发言，禁止访问，禁止IP用户组或者帖子invisible属性小于0，则不允许引用及回复


                if (!string.IsNullOrEmpty(DNTRequest.GetString("quote")))
                {
                    if (postinfo.Invisible != 0)
                    {
                        postinfo.Message = "**** 作者被禁止或删除 内容自动屏蔽 ****";
                    }
                    else
                    {
                        string info = postinfo.Posterid > 0 ? Users.GetShortUserInfo(postinfo.Posterid).Groupid.ToString() : null;
                        if (Utils.InArray(info, "4.5.6"))
                        {
                            postinfo.Message = "**** 作者被禁止或删除 内容自动屏蔽 ****";
                        }
                    }
                    //if (Utils.InArray(Users.GetShortUserInfo(postinfo.Posterid).Groupid.ToString(), "4,5,6") || postinfo.Invisible != 0)
                    //    postinfo.Message = "**** 作者被禁止或删除 内容自动屏蔽 ****";

                    if ((postinfo.Message.IndexOf("[hide]") > -1) && (postinfo.Message.IndexOf("[/hide]") > -1))
                        message = string.Format("[quote] 原帖由 [b]{0}[/b] 于 {1} 发表\r\n ***隐藏帖*** [/quote]", postinfo.Poster, postinfo.Postdatetime);
                    //message = "[quote] 原帖由 [b]" + postinfo.Poster + "[/b] 于 " + postinfo.Postdatetime + " 发表\r\n ***隐藏帖*** [/quote]";
                    else
                        message = string.Format("[quote]{0}\r\n [color=#999999]{1} 发表于 {2} [/color][url={5}showtopic.aspx?topicid={3}&postid={4}#{4}][img]{5}images/common/back.gif[/img][/url][/size][/quote]"
                      , UBB.ClearAttachUBB(Utils.GetSubString(postinfo.Message, 200, "......")), postinfo.Poster, postinfo.Postdatetime, topicid, postid, Utils.GetRootUrl(forumpath));
                }
            }

            // 获取该主题的信息
            topic = Topics.GetTopicInfo(topicid);
            // 如果该主题不存在
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return postinfo;
            }

            topictitle = topic.Title.Trim();
            pagetitle = topictitle;
            forumid = topic.Fid;

            //　如果当前用户非管理员并且该主题已关闭，不允许用户发帖
            if ((admininfo == null || !Moderators.IsModer(admininfo.Admingid, userid, forumid)) && topic.Closed == 1)
            {
                AddErrLine("主题已关闭无法回复");
                return postinfo;
            }

            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1)
            {
                if (forum.Moderators != null && !Utils.InArray(username, forum.Moderators.Split(',')))
                    AddErrLine("本主题阅读权限为: " + topic.Readperm + ", 您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 阅读权限不够");
            }

            return postinfo;
        }

        /// <summary>
        /// 更新用户积分
        /// </summary>
        /// <param name="values">版块积分设置</param>
        private void UpdateUserCredits(float[] values)
        {
            if (values != null)
            {
                UserCredits.UpdateUserExtCredits(userid, values, false);//使用版块内积分
                if (userid != -1)
                    UserCredits.WriteUpdateUserExtCreditsCookies(values);
            }
            else
            {
                UserCredits.UpdateUserCreditsByPosts(userid);//使用默认积分
                if (userid != -1)
                    UserCredits.WriteUpdateUserExtCreditsCookies(Scoresets.GetUserExtCredits(CreditsOperationType.PostReply));
            }
        }

        /// <summary>
        /// 发送邮件通知
        /// </summary>
        /// <param name="email">接收人邮箱</param>
        /// <param name="postinfo">帖子信息</param>
        /// <param name="jumpurl">跳转链接</param>
        public void SendNotifyEmail(string email, PostInfo postinfo, string jumpurl)
        {
            if (Utils.StrIsNullOrEmpty(email))
                return;

            StringBuilder sb_body = new StringBuilder("# 回复: <a href=\"" + jumpurl + "\" target=\"_blank\">" + topic.Title + "</a>");
            string cur_email = "";
            //发送人邮箱
            if (userid > 0)
                cur_email = userinfo.Email.Trim();

            sb_body.Append("\r\n");
            sb_body.Append("\r\n");
            sb_body.Append(UBB.ParseSimpleUBB(postinfo.Message));
            sb_body.Append("\r\n<hr/>");
            sb_body.Append("作 者:" + postinfo.Poster);
            sb_body.Append("\r\n");
            sb_body.Append("Email:<a href=\"mailto:" + cur_email + "\" target=\"_blank\">" + cur_email + "</a>");
            sb_body.Append("\r\n");
            sb_body.Append("URL:<a href=\"" + jumpurl + "\" target=\"_blank\">" + jumpurl + "</a>");
            sb_body.Append("\r\n");
            sb_body.Append("时 间:" + postinfo.Postdatetime);
            Emails.SendEmailNotify(email, "[" + config.Forumtitle + "回复通知]" + topic.Title, sb_body.ToString());
        }

        //因为subeditor子模板中有需要调用同名方法，但是只在editpost中才使用，所以此处只需要有个同名方法返回空即可
        protected string AttachmentList()
        {
            return "";
        }
    }
}
