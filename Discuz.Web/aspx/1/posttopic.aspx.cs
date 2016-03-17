using System;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Plugin.Album;
using Discuz.Plugin.Space;

namespace Discuz.Web
{
    /// <summary>
    /// 发表主题页面
    /// </summary>
    public class posttopic : PageBase
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
        public bool isfirstpost = true;
        /// <summary>
        /// 所属板块Id
        /// </summary>
        public int forumid = DNTRequest.GetInt("forumid", -1);
        /// <summary>
        /// 主题内容
        /// </summary>
        public string message = "";
        /// <summary>
        /// 是否允许发表主题
        /// </summary>
        public bool allowposttopic = true;
        /// <summary>
        /// 是否解析URL
        /// </summary>
        public int parseurloff = 0;
        /// <summary>
        /// 是否解析表情
        /// </summary>
        public int smileyoff;
        /// <summary>
        /// 是否解析 Discuz!NT 代码
        /// </summary>
        public int bbcodeoff = 1;
        /// <summary>
        /// 是否使用签名
        /// </summary>
        public int usesig = ForumUtils.GetCookie("sigstatus") == "0" ? 0 : 1;
        /// <summary>
        /// 是否允许 [img] 标签
        /// </summary>
        public int allowimg;
        /// <summary>
        /// 是否受发帖灌水限制
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
        /// 主题附件购买积分策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo;
        /// <summary>
        /// 悬赏积分信息
        /// </summary>
        public UserExtcreditsInfo bonusextcreditsinfo;
        /// <summary>
        /// 所属版块信息
        /// </summary>
        public ForumInfo forum = new ForumInfo();
        /// <summary>
        /// 主题分类选项字串
        /// </summary>
        public string topictypeselectoptions;
        /// <summary>
        /// 相册列表
        /// </summary>
        public DataTable albumlist;
        /// <summary>
        /// 是否允许上传附件
        /// </summary>
        public bool canpostattach;
        /// <summary>
        /// 是否允许同时发布到相册
        /// </summary>
        public bool caninsertalbum = false;
        /// <summary>
        /// 交易积分
        /// </summary>
        public int creditstrans;
        /// <summary>
        /// 投票截止时间
        /// </summary>
        public string enddatetime = DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
        /// <summary>
        /// 是否允许Html标题
        /// </summary>
        public bool canhtmltitle = false;
        /// <summary>
        /// 发帖人的个人空间Id
        /// </summary>
        public int spaceid = 0;
        /// <summary>
        /// 本版是否可用Tag
        /// </summary>
        public bool enabletag = false;
        /// <summary>
        /// 发帖的类型，如普通帖、悬赏帖等。。
        /// </summary>
        public string type = DNTRequest.GetString("type").ToLower();
        /// <summary>
        /// 当前登录用户的交易积分值, 仅悬赏帖时有效
        /// </summary>
        public float mybonustranscredits;
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
        /// 投票帖
        /// </summary>
        bool createpoll = DNTRequest.GetString("createpoll") == "1";
        /// <summary>
        /// 投票项
        /// </summary>
        string[] pollitem = { };
        /// <summary>
        /// 当前时间
        /// </summary>
        string curdatetime = Utils.GetDateTime();
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
        /// 当前帖的Html标题
        /// </summary>
        public string htmltitle = "";
        /// <summary>
        /// 附件列表
        /// </summary>
        public DataTable attachmentlist = new DataTable();
        /// <summary>
        /// 用户组列表
        /// </summary>
        public Discuz.Common.Generic.List<UserGroupInfo> userGroupInfoList = UserGroups.GetUserGroupList();
        /// <summary>
        /// 编辑器自定义按钮
        /// </summary>
        public string customeditbuttons;
        #endregion

        public int topicid = 0;
        public bool needaudit = false;
        AlbumPluginBase apb = AlbumPluginProvider.GetInstance();
        public int fromindex = DNTRequest.GetInt("fromindex", 0);

        protected override void ShowPage()
        {
            if (oluserinfo.Groupid == 4)
            {
                AddErrLine("你所在的用户组，为禁止发言"); return;
            }

            #region 临时帐号发帖
            //int realuserid = -1;
            //bool tempaccountspost = false;
            //string tempusername = DNTRequest.GetString("tempusername");
            //if (!Utils.StrIsNullOrEmpty(tempusername) && tempusername != username)
            //{
            //    realuserid = Users.CheckTempUserInfo(tempusername, DNTRequest.GetString("temppassword"), DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"));
            //    if (realuserid == -1)
            //    {
            //        AddErrLine("临时帐号登录失败，无法继续发帖。"); return;
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
            {
                userinfo = Users.GetShortUserInfo(userid);
                //    if (userinfo != null)
                //    {
                //        usergroupinfo = UserGroups.GetUserGroupInfo(userinfo.Groupid);
                //        usergroupid = usergroupinfo.Groupid;
                //        useradminid = userinfo.Adminid;
                //    }
            }

            #region 获取并检查版块信息
            forum = Forums.GetForumInfo(forumid);
            if (forum == null || forum.Layer == 0)
            {
                forum = new ForumInfo();//如果不初始化对象，则会报错
                allowposttopic = false;
                AddErrLine("错误的论坛ID"); return;
            }

            pagetitle = Utils.RemoveHtml(forum.Name);
            enabletag = (config.Enabletag & forum.Allowtag) == 1;

            if (forum.Applytopictype == 1)  //启用主题分类
                topictypeselectoptions = Forums.GetCurrentTopicTypesOption(forum.Fid, forum.Topictypes);

            if (forum.Password != "" && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                SetBackLink(base.ShowForumAspxRewrite(forumid, 0)); return;
            }
            needaudit = UserAuthority.NeedAudit(forum, useradminid, userid, usergroupinfo);
            smileyoff = 1 - forum.Allowsmilies;
            bbcodeoff = (forum.Allowbbcode == 1 && usergroupinfo.Allowcusbbcode == 1) ? 0 : 1;
            allowimg = forum.Allowimgcode;
            customeditbuttons = Caches.GetCustomEditButtonList();
            #endregion

            #region 访问和发帖权限校验
            if (!UserAuthority.VisitAuthority(forum, usergroupinfo, userid, ref msg))
            {
                AddErrLine(msg);
                needlogin = true; return;
            }

            if (!UserAuthority.PostAuthority(forum, usergroupinfo, userid, ref msg))
            {
                AddErrLine(msg);
                needlogin = true; return;
            }
            #endregion

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

            canhtmltitle = usergroupinfo.Allowhtmltitle == 1;

            #region 积分信息
            creditstrans = Scoresets.GetTopicAttachCreditsTrans();
            userextcreditsinfo = Scoresets.GetScoreSet(creditstrans);
            bonusextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetBonusCreditsTrans());
            #endregion

            #region 特殊主题权限判断
            if (forum.Allowspecialonly > 0 &&!Utils.InArray(type, "poll,bonus,debate"))
            {
                AddErrLine(string.Format("当前版块 \"{0}\" 不允许发表普通主题", forum.Name)); return;
            }
            if (!UserAuthority.PostSpecialAuthority(forum, type, ref msg))
            {
                AddErrLine(msg); return;
            }
            if (!UserAuthority.PostSpecialAuthority(usergroupinfo, type, ref msg))
            {
                AddErrLine(msg);
                needlogin = true; return;
            }
            if (type == "bonus")
            {
                int creditTrans = Scoresets.GetBonusCreditsTrans();
                //当“交易积分设置”有效时(1-8的整数):
                if (creditTrans <= 0)
                {
                    //AddErrLine(string.Format("系统未设置\"交易积分设置\", 无法判断当前要使用的(扩展)积分字段, 暂时无法发布悬赏", usergroupinfo.Grouptitle)); return;
                    AddErrLine("系统未设置\"交易积分设置\", 无法判断当前要使用的(扩展)积分字段, 暂时无法发布悬赏"); return;
                }
                mybonustranscredits = Users.GetUserExtCredits(userid, creditTrans);
            }
            userGroupInfoList.Sort(delegate(UserGroupInfo x, UserGroupInfo y) { return (x.Readaccess - y.Readaccess) + (y.Groupid - x.Groupid); });
            #endregion

            //发帖不受审核、过滤、灌水等限制权限
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            disablepost = admininfo != null ? admininfo.Disablepostctrl : usergroupinfo.Disableperiodctrl;
            //如果是提交...
            if (ispost)
            {
                #region 判断是否是灌水
                if (!UserAuthority.CheckPostTimeSpan(usergroupinfo, admininfo, oluserinfo, userinfo, ref msg))
                {
                    AddErrLine(msg); return;
                }
                #endregion

                SetBackLink(string.Format("posttopic.aspx?forumid={0}&restore=1&type={1}", forumid, type));

                ForumUtils.WriteCookie("postmessage", postmessage);

                #region 验证提交信息
                //常规项验证
                NormalValidate(admininfo, postmessage, userinfo);
                if (IsErr()) return;

                // 如果用户上传了附件,则检测用户是否有上传附件的权限
                if (ForumUtils.IsPostFile())
                {
                    if (Utils.StrIsNullOrEmpty(Attachments.GetAttachmentTypeArray(attachmentTypeSelect)))
                        AddErrLine("系统不允许上传附件");

                    if (!UserAuthority.PostAttachAuthority(forum, usergroupinfo, userid, ref msg))
                        AddErrLine(msg);
                }

                //发悬赏校验
                int topicprice = 0;
                bool isbonus = type == "bonus";
                ValidateBonus(ref topicprice, ref isbonus);

                //发特殊主题校验
                ValidatePollAndDebate();

                if (IsErr())
                    return;
                #endregion

                int hide = (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1) ? 1 : 0;

                TopicInfo topicinfo = CreateTopic(admininfo, postmessage, isbonus, topicprice);
                if (IsErr())
                    return;

                PostInfo postinfo = CreatePost(topicinfo);

                if (IsErr())
                    return;

                #region 处理附件
                //处理附件
                StringBuilder sb = new StringBuilder();
                AttachmentInfo[] attachmentinfo = null;
                string attachId = DNTRequest.GetFormString("attachid");
                if (!string.IsNullOrEmpty(attachId))
                {
                    attachmentinfo = Attachments.GetNoUsedAttachmentArray(userid, attachId);
                    Attachments.UpdateAttachment(attachmentinfo, topicinfo.Tid, postinfo.Pid, postinfo, ref sb, userid, config, usergroupinfo);
                }
                //加入相册
                if (config.Enablealbum == 1 && apb != null)
                    sb.Append(apb.CreateAttachment(attachmentinfo, usergroupid, userid, username));
                #endregion

                #region 添加日志的操作
                SpacePluginBase spb = SpacePluginProvider.GetInstance();
                if (DNTRequest.GetFormString("addtoblog") == "on" && spb != null)
                {
                    if (userid != -1 && userinfo.Spaceid > 0)
                        spb.CreateTopic(topicinfo, postinfo, attachmentinfo);
                    else
                        AddMsgLine("您的个人空间尚未开通, 无法同时添加为日志");
                }
                #endregion

                OnlineUsers.UpdateAction(olid, UserAction.PostTopic.ActionID, forumid, forum.Name, -1, "");

                #region 设置提示信息和跳转链接
                if (sb.Length > 0)
                {
                    SetUrl(base.ShowTopicAspxRewrite(topicinfo.Tid, 0));
                    SetMetaRefresh(5);
                    SetShowBackLink(true);
                    if (infloat == 1)
                    {
                        AddErrLine(sb.ToString());
                        return;
                    }
                    else
                    {
                        sb.Insert(0, "<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\"><tr><td colspan=2 align=\"left\"><span class=\"bold\"><nobr>发表主题成功,但图片/附件上传出现问题:</nobr></span><br /></td></tr>");
                        AddMsgLine(sb.Append("</table>").ToString());
                    }
                }
                else
                {
                    SetShowBackLink(false);
                    if (useradminid != 1)
                    {
                        //是否需要审核
                        if (UserAuthority.NeedAudit(forum, useradminid, userid, usergroupinfo) || topicinfo.Displayorder == -2)
                        {
                            ForumUtils.WriteCookie("postmessage", "");
                            SetLastPostedForumCookie();
                            SetUrl(base.ShowForumAspxRewrite(forumid, forumpageid));
                            SetMetaRefresh();
                            AddMsgLine("发表主题成功, 但需要经过审核才可以显示. 返回该版块");
                        }
                        else
                            PostTopicSucceed(Forums.GetValues(forum.Postcredits), topicinfo, topicinfo.Tid);
                    }
                    else
                        PostTopicSucceed(Forums.GetValues(forum.Postcredits), topicinfo, topicinfo.Tid);
                }
                #endregion

                //ForumUtils.WriteCookie("postmessage", "");
                //SetLastPostedForumCookie();

                //如果已登录就不需要再登录
                if (needlogin && userid > 0)
                    needlogin = false;
            }
            else //非提交操作
                AddLinkCss(BaseConfigs.GetForumPath + "templates/" + templatepath + "/editor.css", "css");
        }

        /// <summary>
        /// 创建主题信息
        /// </summary>
        /// <param name="admininfo"></param>
        /// <param name="postmessage"></param>
        /// <param name="isbonus"></param>
        /// <param name="topicprice"></param>
        /// <returns></returns>
        public TopicInfo CreateTopic(AdminGroupInfo admininfo, string postmessage, bool isbonus, int topicprice)
        {
            TopicInfo topicinfo = new TopicInfo();
            topicinfo.Fid = forumid;
            topicinfo.Iconid = (DNTRequest.GetInt("iconid", 0) < 0 || DNTRequest.GetInt("iconid", 0) > 15) ? 0 :
                                DNTRequest.GetInt("iconid", 0);
            message = Posts.GetPostMessage(usergroupinfo, admininfo, postmessage,
                (TypeConverter.StrToInt(DNTRequest.GetString("htmlon")) == 1));

            topicinfo.Title = (useradminid == 1) ? Utils.HtmlEncode(posttitle) :
                               Utils.HtmlEncode(ForumUtils.BanWordFilter(posttitle));
            
            if (useradminid != 1 && (ForumUtils.HasBannedWord(posttitle) || ForumUtils.HasBannedWord(postmessage)))
            {
                string bannedWord = ForumUtils.GetBannedWord(posttitle) == string.Empty ? ForumUtils.GetBannedWord(postmessage) : ForumUtils.GetBannedWord(posttitle);
                AddErrLine(string.Format("对不起, 您提交的内容包含不良信息  <font color=\"red\">{0}</font>, 请返回修改!", bannedWord));
                return topicinfo;
            }

            if (Utils.GetCookie("lasttopictitle") == Utils.MD5(topicinfo.Title) || Utils.GetCookie("lasttopicmessage") == Utils.MD5(message))
            {
                AddErrLine("请勿重复发帖");
                return topicinfo;
            }

            topicinfo.Typeid = DNTRequest.GetInt("typeid", 0);
            if (usergroupinfo.Allowsetreadperm == 1)
                topicinfo.Readperm = DNTRequest.GetInt("topicreadperm", 0) > 255 ? 255 : DNTRequest.GetInt("topicreadperm", 0);

            topicinfo.Price = topicprice;
            topicinfo.Poster = username;
            topicinfo.Posterid = userid;
            topicinfo.Postdatetime = curdatetime;
            topicinfo.Lastpost = curdatetime;
            topicinfo.Lastposter = username;
            topicinfo.Displayorder = Topics.GetTitleDisplayOrder(usergroupinfo, useradminid, forum, topicinfo, message, disablepost);


            string htmltitle = DNTRequest.GetString("htmltitle").Trim();
            if (!Utils.StrIsNullOrEmpty(htmltitle) && Utils.HtmlDecode(htmltitle).Trim() != topicinfo.Title)
            {
                //按照  附加位/htmltitle(1位)/magic(3位)/以后扩展（未知位数） 的方式来存储  例： 11001
                topicinfo.Magic = 11000;
            }

            //标签(Tag)操作                
            string tags = DNTRequest.GetString("tags").Trim();
            string[] tagArray = null;
            if (enabletag && !Utils.StrIsNullOrEmpty(tags))
            {
                if (ForumUtils.InBanWordArray(tags))
                {
                    AddErrLine("标签中含有系统禁止词语,请修改");
                    return topicinfo;
                }

                tagArray = Utils.SplitString(tags, " ", true, 2, 10);
                if (tagArray.Length > 0 && tagArray.Length <= 5)
                {
                    if (topicinfo.Magic == 0)
                        topicinfo.Magic = 10000;

                    topicinfo.Magic = Utils.StrToInt(topicinfo.Magic.ToString() + "1", 0);
                }
                else
                {
                    AddErrLine("超过标签数的最大限制或单个标签长度没有介于2-10之间，最多可填写 5 个标签");
                    return topicinfo;
                }
            }

            if (isbonus)
            {
                topicinfo.Special = 2;

                //检查积分是否足够
                if (mybonustranscredits < topicprice && usergroupinfo.Radminid != 1)
                {
                    AddErrLine(string.Format("无法进行悬赏<br /><br />您当前的{0}为 {1} {3}<br/>悬赏需要{0} {2} {3}", bonusextcreditsinfo.Name, mybonustranscredits, topicprice, bonusextcreditsinfo.Unit));
                    return topicinfo;
                }
                else
                    Users.UpdateUserExtCredits(topicinfo.Posterid, Scoresets.GetBonusCreditsTrans(),
                                       -topicprice * (Scoresets.GetCreditsTax() + 1)); //计算税后的实际支付
            }

            if (type == "poll")
                topicinfo.Special = 1;

            if (type == "debate") //辩论帖
                topicinfo.Special = 4;

            if (!Moderators.IsModer(useradminid, userid, forumid))
                topicinfo.Attention = 1;

            if (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1)
                topicinfo.Hide = 1;

            topicinfo.Tid = Topics.CreateTopic(topicinfo);

            //canhtmltitle = config.Htmltitle == 1 && Utils.InArray(usergroupid.ToString(), config.Htmltitleusergroup);
            //canhtmltitle = config.Htmltitle == 1 && usergroupinfo.Allowhtml == 1;
            //保存htmltitle

            if (canhtmltitle && !Utils.StrIsNullOrEmpty(htmltitle) && htmltitle != topicinfo.Title)
                Topics.WriteHtmlTitleFile(Utils.RemoveUnsafeHtml(htmltitle), topicinfo.Tid);

            if (enabletag && tagArray != null && tagArray.Length > 0)
            {
                if (useradminid != 1 && ForumUtils.HasBannedWord(tags))
                {
                    string bannedWord = ForumUtils.GetBannedWord(tags);
                    AddErrLine(string.Format("标签中含有系统禁止词语 <font color=\"red\">{0}</font>,请修改", bannedWord));
                    return topicinfo;
                }
                ForumTags.CreateTopicTags(tagArray, topicinfo.Tid, userid, curdatetime);
            }

            if (type == "debate")
            {
                DebateInfo debatetopic = new DebateInfo();
                debatetopic.Tid = topicinfo.Tid;
                debatetopic.Positiveopinion = DNTRequest.GetString("positiveopinion");
                debatetopic.Negativeopinion = DNTRequest.GetString("negativeopinion");
                debatetopic.Terminaltime = Convert.ToDateTime(DNTRequest.GetString("terminaltime"));
                Topics.CreateDebateTopic(debatetopic);
            }

            Topics.AddParentForumTopics(forum.Parentidlist.Trim(), 1);
            return topicinfo;
        }

        /// <summary>
        /// 创建主题帖信息
        /// </summary>
        /// <param name="topicinfo"></param>
        /// <returns></returns>
        public PostInfo CreatePost(TopicInfo topicinfo)
        {
            PostInfo postinfo = new PostInfo();
            postinfo.Fid = forumid;
            postinfo.Tid = topicinfo.Tid;
            postinfo.Poster = username;
            postinfo.Posterid = userid;
            postinfo.Title = useradminid == 1 ? Utils.HtmlEncode(posttitle) :
                             postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(posttitle));
            postinfo.Postdatetime = curdatetime;
            postinfo.Message = message;
            postinfo.Ip = DNTRequest.GetIP();
            postinfo.Invisible = UserAuthority.GetTopicPostInvisible(forum, useradminid, userid, usergroupinfo, postinfo);
            postinfo.Usesig = TypeConverter.StrToInt(DNTRequest.GetString("usesig"));
            postinfo.Htmlon = (usergroupinfo.Allowhtml == 1 && (TypeConverter.StrToInt(DNTRequest.GetString("htmlon")) == 1)) ? 1 : 0;
            postinfo.Smileyoff = (smileyoff == 0 && forum.Allowsmilies == 1) ? TypeConverter.StrToInt(DNTRequest.GetString("smileyoff")) : smileyoff;
            postinfo.Bbcodeoff = (usergroupinfo.Allowcusbbcode == 1 && forum.Allowbbcode == 1) ? postinfo.Bbcodeoff = TypeConverter.StrToInt(DNTRequest.GetString("bbcodeoff")) : 1;
            postinfo.Parseurloff = TypeConverter.StrToInt(DNTRequest.GetString("parseurloff"));
            postinfo.Topictitle = topicinfo.Title;

            //if (Utils.GetCookie("lasttopictitle") == Utils.MD5(postinfo.Title) || Utils.GetCookie("lasttopicmessage") == Utils.MD5(postinfo.Message))
            //{
            //    AddErrLine("请勿重复发帖");
            //    return postinfo;
            //}

            try
            {
                postinfo.Pid = Posts.CreatePost(postinfo);
                Utils.WriteCookie("lasttopictitle", Utils.MD5(postinfo.Title));
                Utils.WriteCookie("lasttopicmessage", Utils.MD5(postinfo.Message));
            }
            catch
            {
                TopicAdmins.DeleteTopics(topicinfo.Tid.ToString(), false);
                AddErrLine("帖子保存出现异常");
            }

            //创建投票
            if (createpoll)
            {
                msg = Polls.CreatePoll(DNTRequest.GetFormString("PollItemname"), DNTRequest.GetString("multiple") == "on" ? 1 : 0,
                    DNTRequest.GetInt("maxchoices", 1), DNTRequest.GetString("visiblepoll") == "on" ? 1 : 0, DNTRequest.GetString("allowview") == "on" ? 1 : 0,
                    enddatetime, topicinfo.Tid, pollitem, userid);
            }
            return postinfo;
        }

        /// <summary>
        /// 发帖成功
        /// </summary>
        /// <param name="values">版块积分设置</param>
        /// <param name="topicinfo">主题信息</param>
        /// <param name="topicid">主题ID</param>
        private void PostTopicSucceed(float[] values, TopicInfo topicinfo, int topicid)
        {
            if (values != null) ///使用版块内积分
            {
                UserCredits.UpdateUserExtCredits(userid, values, false);
                if (userid != -1)
                    UserCredits.WriteUpdateUserExtCreditsCookies(values);
            }
            else ///使用默认积分                
            {
                UserCredits.UpdateUserCreditsByPostTopic(userid);
                if (userid != -1)
                    UserCredits.WriteUpdateUserExtCreditsCookies(Scoresets.GetUserExtCredits(CreditsOperationType.PostTopic));
            }

            //当使用伪aspx
            if (config.Aspxrewrite == 1)
                SetUrl(topicinfo.Special == 4 ? ShowDebateAspxRewrite(topicid) : ShowTopicAspxRewrite(topicid, 0));
            else
                SetUrl((topicinfo.Special == 4 ? ShowDebateAspxRewrite(topicid) : ShowTopicAspxRewrite(topicid, 0)) + "&forumpage=" + forumpageid);

            ForumUtils.WriteCookie("postmessage", "");
            ForumUtils.WriteCookie("clearUserdata", "forum");
            SetLastPostedForumCookie();

            SetMetaRefresh();
            MsgForward("posttopic_succeed");
            AddMsgLine("发表主题成功, 返回该主题<br />(<a href=\"" + base.ShowForumAspxRewrite(forumid, forumpageid) + "\">点击这里返回 " + forum.Name + "</a>)<br />");

            //通知应用有新主题
            Sync.NewTopic(topicid.ToString(), topicinfo.Title, topicinfo.Poster, topicinfo.Posterid.ToString(), topicinfo.Fid.ToString(), "");
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

            if (forum.Applytopictype == 1 && forum.Postbytopictype == 1 && !Utils.StrIsNullOrEmpty(topictypeselectoptions))
            {
                if (DNTRequest.GetString("typeid").Trim().Equals(""))
                    AddErrLine("主题类型不能为空");
                //检测所选主题分类是否有效
                if (!Forums.IsCurrentForumTopicType(DNTRequest.GetString("typeid").Trim(), forum.Topictypes))
                    AddErrLine("错误的主题类型");
            }
            if (Utils.StrIsNullOrEmpty(DNTRequest.GetString(config.Antispamposttitle).Trim().Replace("　", "")))
                AddErrLine("标题不能为空");
            else if (DNTRequest.GetString(config.Antispamposttitle).Length > 60)
                AddErrLine("标题最大长度为60个字符,当前为 " + DNTRequest.GetString(config.Antispamposttitle).Length + " 个字符");

            if (Utils.StrIsNullOrEmpty(postmessage.Replace("　", "")))
                AddErrLine("内容不能为空");

            if (admininfo != null && admininfo.Disablepostctrl != 1)
            {
                if (postmessage.Length < config.Minpostsize)
                    AddErrLine("您发表的内容过少, 系统设置要求帖子内容不得少于 " + config.Minpostsize + " 字多于 " + config.Maxpostsize + " 字");
                else if (postmessage.Length > config.Maxpostsize)
                    AddErrLine("您发表的内容过多, 系统设置要求帖子内容不得少于 " + config.Minpostsize + " 字多于 " + config.Maxpostsize + " 字");
            }

            //新用户广告强力屏蔽检查
            if ((config.Disablepostad == 1) && useradminid < 1)  //如果开启新用户广告强力屏蔽检查或是游客
            {
                if ((config.Disablepostadpostcount != 0 && user.Posts <= config.Disablepostadpostcount) ||
                    (config.Disablepostadregminute != 0 && DateTime.Now.AddMinutes(-config.Disablepostadregminute) <= Convert.ToDateTime(user.Joindate)))
                {
                    foreach (string regular in config.Disablepostadregular.Replace("\r", "").Split('\n'))
                    {
                        if (Posts.IsAD(regular, DNTRequest.GetString(config.Antispamposttitle), postmessage))
                            AddErrLine("发帖失败，内容中有不符合新用户强力广告屏蔽规则的字符，请检查标题和内容，如有疑问请与管理员联系");
                    }
                }
            }
        }

        /// <summary>
        /// 悬赏校验
        /// </summary>
        /// <param name="topicprice"></param>
        /// <param name="isbonus"></param>
        private void ValidateBonus(ref int topicprice, ref bool isbonus)
        {
            #region 悬赏/售价验证
            isbonus = type == "bonus";
            topicprice = 0;
            string tmpprice = DNTRequest.GetString("topicprice");

            if (Regex.IsMatch(tmpprice, "^[0-9]*[0-9][0-9]*$") || tmpprice == "")
            {
                topicprice = Utils.StrToInt(tmpprice, 0) > 32767 ? 32767 : Utils.StrToInt(tmpprice, 0);
                if (!isbonus)
                {
                    if (topicprice > usergroupinfo.Maxprice && usergroupinfo.Maxprice > 0)
                    {
                        if (userextcreditsinfo.Unit.Equals(""))
                            AddErrLine(string.Format("主题售价不能高于 {0} {1}", usergroupinfo.Maxprice, userextcreditsinfo.Name));
                        else
                            AddErrLine(string.Format("主题售价不能高于 {0} {1}({2})", usergroupinfo.Maxprice, userextcreditsinfo.Name, userextcreditsinfo.Unit));
                    }
                    else if (topicprice > 0 && usergroupinfo.Maxprice <= 0)
                        AddErrLine(string.Format("您当前的身份 \"{0}\" 未被允许出售主题", usergroupinfo.Grouptitle));
                    else if (topicprice < 0)
                        AddErrLine("主题售价不能为负数");
                }
                else
                {

                    if (usergroupinfo.Allowbonus == 0)
                        AddErrLine(string.Format("您当前的身份 \"{0}\" 未被允许进行悬赏", usergroupinfo.Grouptitle));
                    if (topicprice > mybonustranscredits)
                        AddErrLine(string.Format("您悬赏的{0},已超过您所能支付的范围", bonusextcreditsinfo.Name));
                    if (topicprice < usergroupinfo.Minbonusprice || topicprice > usergroupinfo.Maxbonusprice)
                        AddErrLine(string.Format("悬赏价格超出范围, 您应在 {0} - {1} {2}{3} 范围内进行悬赏", usergroupinfo.Minbonusprice, usergroupinfo.Maxbonusprice,
                            bonusextcreditsinfo.Unit, bonusextcreditsinfo.Name));
                }
            }
            else
            {
                if (!isbonus)
                    AddErrLine("主题售价只能为整数");
                else
                    AddErrLine("悬赏价格只能为整数");
            }
            #endregion
        }

        /// <summary>
        /// 投票和辩论校验
        /// </summary>
        /// <param name="createpoll"></param>
        /// <param name="pollitem"></param>
        private void ValidatePollAndDebate()
        {
            #region 投票验证
            if (DNTRequest.GetString("createpoll") == "1")
            {
                // 验证用户是否有发布投票的权限
                if (usergroupinfo.Allowpostpoll != 1)
                    AddErrLine(string.Format("您当前的身份 \"{0}\" 没有发布投票的权限", usergroupinfo.Grouptitle));

                pollitem = Utils.SplitString(DNTRequest.GetString("PollItemname"), "\r\n");
                if (pollitem.Length < 2)
                    AddErrLine("投票项不得少于2个");
                else if (pollitem.Length > config.Maxpolloptions)
                    AddErrLine(string.Format("系统设置为投票项不得多于{0}个", config.Maxpolloptions));
                else
                {
                    for (int i = 0; i < pollitem.Length; i++)
                    {
                        if (pollitem[i].Trim().Equals(""))
                            AddErrLine("投票项不能为空");
                    }
                }

                enddatetime = DNTRequest.GetString("enddatetime");
                if (!Utils.IsDateString(enddatetime))
                    AddErrLine("投票结束日期格式错误");
            }
            #endregion

            #region 辩论验证
            if (type == "debate")
            {
                if (usergroupinfo.Allowdebate != 1)
                    AddErrLine(string.Format("您当前的身份 \"{0}\" 没有发起辩论的权限", usergroupinfo.Grouptitle)); ;
                if (DNTRequest.GetString("positiveopinion") == "")
                    AddErrLine("正方观点不能为空");
                if (DNTRequest.GetString("negativeopinion") == "")
                    AddErrLine("反方观点不能为空");
                if (!Utils.IsDateString(DNTRequest.GetString("terminaltime")))
                    AddErrLine("结束日期格式不正确");
            }
            #endregion
        }

        /// <summary>
        /// 设置最后发帖版块Cookie 
        /// </summary>
        private void SetLastPostedForumCookie()
        {
            Utils.WriteCookie("lastpostedforum", forum.Fid.ToString(), 525600);
        }

        protected string AttachmentList()
        {
            return "";
        }
    }
}
