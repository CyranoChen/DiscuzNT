using System;
using System.Collections;
using System.Data;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Web.UI;
using Discuz.Common.Generic;
using Discuz.Plugin.Album;

namespace Discuz.Web
{
    /// <summary>
    /// 编辑帖子页面
    /// </summary>
    public class editpost : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 帖子所属版块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 帖子信息
        /// </summary>
        public PostInfo postinfo;
        /// <summary>
        /// 帖子所属主题信息
        /// </summary>
        public TopicInfo topic;
        /// <summary>
        /// 是否为主题帖
        /// </summary>
        public bool isfirstpost = false;
        /// <summary>
        /// 投票选项列表
        /// </summary>
        public DataTable polloptionlist;
        /// <summary>
        /// 投票帖类型
        /// </summary>
        public PollInfo pollinfo;
        /// <summary>
        /// 辩论贴类型
        /// </summary>
        public DebateInfo debateinfo;
        /// <summary>
        /// 附件列表
        /// </summary>
        public DataTable attachmentlist;
        /// <summary>
        /// 附件数
        /// </summary>
        public int attachmentcount;
        /// <summary>
        /// 帖子内容
        /// </summary>
        public string message;
        /// <summary>
        /// 是否进行URL解析
        /// </summary>
        public int parseurloff;
        /// <summary>
        /// 是否进行表情解析
        /// </summary>
        public int smileyoff;
        /// <summary>
        /// 是否进行Discuz!NT代码解析
        /// </summary>
        public int bbcodeoff;
        /// <summary>
        /// 是否使用签名
        /// </summary>
        public int usesig;
        /// <summary>
        /// 是否允许[img]代码
        /// </summary>
        public int allowimg;
        /// <summary>
        /// 是否受发帖控制限制
        /// </summary>
        public int disablepostctrl;
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
        /// 积分策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo;
        /// <summary>
        /// 悬赏积分信息
        /// </summary>
        public UserExtcreditsInfo bonusextcreditsinfo = new UserExtcreditsInfo();
        /// <summary>
        /// 所属版块信息
        /// </summary>
        public ForumInfo forum = new ForumInfo();
        /// <summary>
        /// 当前版块的主题类型选项
        /// </summary>
        public string topictypeselectoptions;
        /// <summary>
        /// 表情分类
        /// </summary>
        public DataTable smilietypes;
        /// <summary>
        /// 相册列表
        /// </summary>
        public DataTable albumlist;
        /// <summary>
        /// 是否允许上传附件
        /// </summary>
        public bool canpostattach;
        /// <summary>
        /// 是否允许将图片放入相册
        /// </summary>
        public bool caninsertalbum;
        /// <summary>
        /// 是否显示下载链接
        /// </summary>		
        public bool allowviewattach = false;
        /// <summary>
        /// 是否有Html标题的权限
        /// </summary>
        public bool canhtmltitle = false;
        /// <summary>
        /// 当前帖的Html标题
        /// </summary>
        public string htmltitle = string.Empty;
        /// <summary>
        /// 主题所用标签
        /// </summary>
        public string topictags = string.Empty;
        /// <summary>
        /// 本版是否启用了Tag
        /// </summary>
        public bool enabletag = false;
        /// <summary>
        /// 当前版块的分页id
        /// </summary>
        public int forumpageid = DNTRequest.GetInt("forumpage", 1);
        /// <summary>
        /// 开启html功能
        /// </summary>
        public int htmlon = 0;
        /// <summary>
        /// 权限校验提示信息
        /// </summary>
        string msg = "";
        /// <summary>
        /// 是否允许编辑帖子, 初始false为不允许
        /// </summary>
        bool alloweditpost = false;
        /// <summary>
        /// 当前登录用户的交易积分值, 仅悬赏帖时有效
        /// </summary>
        public float mybonustranscredits;
        /// <summary>
        /// 悬赏使用积分字段
        /// </summary>
        public int bonusCreditsTrans = Scoresets.GetBonusCreditsTrans();
        /// <summary>
        /// 
        /// </summary>
        public ShortUserInfo userinfo;
        /// <summary>
        /// 用户组列表
        /// </summary>
        public Discuz.Common.Generic.List<UserGroupInfo> userGroupInfoList = UserGroups.GetUserGroupList();
        public int topicid = DNTRequest.GetInt("topicid", -1);
        public int postid = DNTRequest.GetInt("postid", -1);
        public int pageid = DNTRequest.GetInt("pageid", 1);
        public bool needaudit = false;
        string postMessage = DNTRequest.GetString(GeneralConfigs.GetConfig().Antispampostmessage);
        string postTitle = DNTRequest.GetString(GeneralConfigs.GetConfig().Antispamposttitle);
        /// <summary>
        /// 编辑器自定义按钮
        /// </summary>
        public string customeditbuttons;

        #endregion

        AlbumPluginBase apb = AlbumPluginProvider.GetInstance();

        protected override void ShowPage()
        {
            //pagetitle = "编辑帖子";
            #region 判断是否是灌水
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(usergroupid);
            this.disablepostctrl = 0;
            if (admininfo != null)
                disablepostctrl = admininfo.Disablepostctrl;
            #endregion

            if (userid == -1)
            {
                forum = new ForumInfo();
                topic = new TopicInfo();
                postinfo = new PostInfo();
                AddErrLine("您尚未登录");
                return;
            }

            #region 获取帖子和主题相关信息
            // 如果帖子ID非数字
            if (postid == -1)
            {
                AddErrLine("无效的帖子ID");
                return;
            }

            postinfo = Posts.GetPostInfo(topicid, postid);
            // 如果帖子不存在
            if (postinfo == null)
            {
                AddErrLine("不存在的帖子ID");
                return;
            }
            pagetitle = (postinfo.Title == "") ? "编辑帖子" : postinfo.Title;
            htmlon = postinfo.Htmlon;
            message = postinfo.Message;
            isfirstpost = postinfo.Layer == 0;

            // 获取主题ID
            if (topicid != postinfo.Tid || postinfo.Tid == -1)
            {
                AddErrLine("无效的主题ID");
                return;
            }

            // 获取该主题的信息
            topic = Topics.GetTopicInfo(postinfo.Tid);
            // 如果该主题不存在
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return;
            }

            if (topic.Special == 1 && postinfo.Layer == 0)
            {
                pollinfo = Polls.GetPollInfo(topic.Tid);
                polloptionlist = Polls.GetPollOptionList(topic.Tid);
            }

            if (topic.Special == 4 && postinfo.Layer == 0)
            {
                debateinfo = Debates.GetDebateTopic(topic.Tid);
            }

            #endregion

            #region 获取并检查版块信息
            ///得到所在版块信息
            forumid = topic.Fid;
            forum = Forums.GetForumInfo(forumid);
            needaudit = UserAuthority.NeedAudit(forum, useradminid, topic, userid, disablepostctrl, usergroupinfo);
            // 如果该版块不存在
            if (forum == null || forum.Layer == 0)
            {
                AddErrLine("版块已不存在");
                forum = new ForumInfo();
                return;
            }

            if (!Utils.StrIsNullOrEmpty(forum.Password) && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                SetBackLink(base.ShowForumAspxRewrite(forumid, 0));
                return;
            }

            if (forum.Applytopictype == 1)  //启用主题分类
                topictypeselectoptions = Forums.GetCurrentTopicTypesOption(forum.Fid, forum.Topictypes);
            customeditbuttons = Caches.GetCustomEditButtonList();
            #endregion

            //是否有编辑帖子的权限
            if (!UserAuthority.CanEditPost(postinfo, userid, useradminid, ref msg))
            {
                AddErrLine(msg);
                return;
            }
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

            userinfo = Users.GetShortUserInfo(userid);
            if (canpostattach && (config.Enablealbum == 1) && apb != null &&
                (UserGroups.GetUserGroupInfo(userinfo.Groupid).Maxspacephotosize - apb.GetPhotoSizeByUserid(userid) > 0))
            {
                caninsertalbum = true;
                albumlist = apb.GetSpaceAlbumByUserId(userid);
            }
            else
                caninsertalbum = false;

            attachmentlist = Attachments.GetAttachmentListByPid(postinfo.Pid);
            attachmentcount = attachmentlist.Rows.Count;
            //当前用户是否有允许下载附件权限
            allowviewattach = UserAuthority.DownloadAttachment(forum, userid, usergroupinfo);

            #endregion

            smileyoff = (!DNTRequest.IsPost()) ? postinfo.Smileyoff : 1 - forum.Allowsmilies;
            allowimg = forum.Allowimgcode;
            parseurloff = postinfo.Parseurloff;
            bbcodeoff = (usergroupinfo.Allowcusbbcode == 1) ? postinfo.Bbcodeoff : 1;
            usesig = postinfo.Usesig;
            userextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans());
            if (bonusCreditsTrans > 0 && bonusCreditsTrans < 9)
            {
                bonusextcreditsinfo = Scoresets.GetScoreSet(bonusCreditsTrans);
                mybonustranscredits = Users.GetUserExtCredits(userid, bonusCreditsTrans);
            }

            //是否有访问当前版块的权限
            if (!UserAuthority.VisitAuthority(forum, usergroupinfo, userid, ref msg))
            {
                AddErrLine(msg);
                return;
            }

            // 判断当前用户是否有修改权限, 检查是否具有版主的身份
            if (!Moderators.IsModer(useradminid, userid, forumid))
            {
                if (postinfo.Posterid != userid)
                {
                    AddErrLine("你并非作者, 且你当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有修改该帖的权限");
                    return;
                }
                else if (config.Edittimelimit > 0 && Utils.StrDateDiffMinutes(postinfo.Postdatetime, config.Edittimelimit) > 0)
                {
                    AddErrLine("抱歉, 系统规定只能在帖子发表" + config.Edittimelimit + "分钟内才可以修改");
                    return;
                }
                else if(config.Edittimelimit==-1)
                {
                    AddErrLine("抱歉，系统不允许修改帖子");
                    return;
                }
            }

            #region htmltitle标题
            if (postinfo.Layer == 0)
                canhtmltitle = usergroupinfo.Allowhtmltitle == 1;

            if (Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle) == 1)
                htmltitle = Topics.GetHtmlTitle(topic.Tid).Replace("\"", "\\\"").Replace("'", "\\'");
            #endregion

            #region tag信息
            enabletag = (config.Enabletag & forum.Allowtag) == 1;
            if (enabletag && Topics.GetMagicValue(topic.Magic, MagicType.TopicTag) == 1)
            {
                foreach (TagInfo tag in ForumTags.GetTagsListByTopic(topic.Tid))
                {
                    if (tag.Orderid > -1)
                        topictags += string.Format(" {0}", tag.Tagname);
                }
                topictags = topictags.Trim();
            }
            #endregion
            userGroupInfoList.Sort(delegate(UserGroupInfo x, UserGroupInfo y) { return (x.Readaccess - y.Readaccess) + (y.Groupid - x.Groupid); });
            //如果是提交...
            if (ispost)
            {
                SetBackLink("editpost.aspx?topicid=" + postinfo.Tid + "&postid=" + postinfo.Pid);

                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                //设置相关帖子信息
                SetPostInfo(admininfo, userinfo, Utils.StrToInt(DNTRequest.GetString("htmlon"), 0) == 1);

                if (IsErr()) return;

                //通过验证的用户可以编辑帖子
                Posts.UpdatePost(postinfo);

                //设置附件相关信息
                System.Text.StringBuilder sb = SetAttachmentInfo();

                if (IsErr()) return;

                UserCredits.UpdateUserCredits(userid);

                #region 设置提示信息和跳转链接
                //辩论地址
                if (topic.Special == 4)
                    SetUrl(Urls.ShowDebateAspxRewrite(topic.Tid));
                else if (DNTRequest.GetQueryString("referer") != "")//ajax快速回复将传递referer参数
                    SetUrl(string.Format("showtopic.aspx?page=end&forumpage={2}&topicid={0}#{1}", topic.Tid, postinfo.Pid, forumpageid));
                else if (pageid > 1)//如果不是ajax,则应该是带pageid的参数
                {
                    if (config.Aspxrewrite == 1)
                        SetUrl(string.Format("showtopic-{0}-{2}{1}#{3}", topic.Tid, config.Extname, pageid, postinfo.Pid));
                    else
                        SetUrl(string.Format("showtopic.aspx?topicid={0}&forumpage={3}&page={2}#{1}", topic.Tid, postinfo.Pid, pageid, forumpageid));
                }
                else//如果都为空.就跳转到第一页(以免意外情况)
                {
                    if (config.Aspxrewrite == 1)
                        SetUrl(string.Format("showtopic-{0}{1}", topic.Tid, config.Extname));
                    else
                        SetUrl(string.Format("showtopic.aspx?topicid={0}&forumpage={1}", topic.Tid, forumpageid));
                }

                if (sb.Length > 0)
                {
                    SetMetaRefresh(5);
                    SetShowBackLink(true);
                    if (infloat == 1)
                    {
                        AddErrLine(sb.ToString());
                        return;
                    }
                    else
                    {
                        sb.Insert(0, "<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\"><tr><td colspan=2 align=\"left\"><span class=\"bold\"><nobr>编辑帖子成功,但图片/附件上传出现问题:</nobr></span><br /></td></tr>");
                        sb.Append("</table>");
                        AddMsgLine(sb.ToString());
                    }
                }
                else
                {
                    //编辑主题和回复需要审核
                    if (postinfo.Layer == 0)
                        SetMetaRefresh(2, base.ShowForumAspxRewrite(forumid, forumpageid));
                    else
                        SetMetaRefresh();
                    SetShowBackLink(false);

                    if (useradminid != 1 && (needaudit || topic.Displayorder == -2 || postinfo.Invisible == 1))
                    {
                        if (postinfo.Layer == 0)
                            SetUrl(base.ShowForumAspxRewrite(forumid, forumpageid));
                        else
                            SetUrl(base.ShowTopicAspxRewrite(topic.Tid, forumpageid));
                        AddMsgLine("编辑成功, 但需要经过审核才可以显示");
                    }
                    else
                    {
                        MsgForward("editpost_succeed");
                        AddMsgLine("编辑帖子成功, 返回该主题");
                    }
                }
                #endregion

                // 删除主题游客缓存
                if (postinfo.Layer == 0)
                    ForumUtils.DeleteTopicCacheFile(topic.Tid);
            }
            else
                AddLinkCss(BaseConfigs.GetForumPath + "templates/" + templatepath + "/editor.css", "css");
        }

        private int GetAttachmentUpdatedIndex(string attachmentId, string[] updatedAttArray)
        {
            for (int i = 0; i < updatedAttArray.Length; i++)
            {
                if (updatedAttArray[i] == attachmentId)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 验证帖子信息
        /// </summary>
        /// <param name="admininfo"></param>
        /// <param name="user"></param>
        /// <param name="ishtmlon"></param>
        private void SetPostInfo(AdminGroupInfo admininfo, ShortUserInfo user, bool ishtmlon)
        {
            if (postinfo.Layer == 0 && forum.Applytopictype == 1 && forum.Postbytopictype == 1 && topictypeselectoptions != string.Empty)
            {
                if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("typeid")) || DNTRequest.GetString("typeid").Trim() == "0")
                {
                    AddErrLine("主题类型不能为空");
                    return;
                }

                if (!Forums.IsCurrentForumTopicType(DNTRequest.GetString("typeid").Trim(), forum.Topictypes))
                {
                    AddErrLine("错误的主题类型");
                    return;
                }
            }

            //这段代码有什么作用，和下面的SetAttachmentInfo方法做的事情是否有重复？能否拿掉？
            ///删除附件
            if (DNTRequest.GetInt("isdeleteatt", 0) == 1)
            {
                if (DNTRequest.GetFormInt("aid", 0) > 0 && Attachments.DeleteAttachment(DNTRequest.GetFormInt("aid", 0)) > 0)
                {
                    attachmentlist = Attachments.GetAttachmentListByPid(postinfo.Pid);
                    attachmentcount = Attachments.GetAttachmentCountByPid(postinfo.Pid);
                }
                AddLinkCss(BaseConfigs.GetForumPath + "templates/" + templatepath + "/editor.css", "css");
                // 帖子内容
                message = postinfo.Message;
                ispost = false;

                return;
            }
            //
            #region 检查标题和内容信息
            if (string.IsNullOrEmpty(postTitle.Trim().Replace("　", "")) && postinfo.Layer == 0)
                AddErrLine("标题不能为空");
            else if (postTitle.Length > 60)
                AddErrLine("标题最大长度为60个字符,当前为 " + postTitle.Length.ToString() + " 个字符");

            //string postmessage = DNTRequest.GetString("message");
            if (postMessage.Equals("") || postMessage.Replace("　", "").Equals(""))
                AddErrLine("内容不能为空");

            if (admininfo != null && disablepostctrl != 1)
            {
                if (postMessage.Length < config.Minpostsize)
                    AddErrLine("您发表的内容过少, 系统设置要求帖子内容不得少于 " + config.Minpostsize.ToString() + " 字多于 " + config.Maxpostsize.ToString() + " 字");
                else if (postMessage.Length > config.Maxpostsize)
                    AddErrLine("您发表的内容过多, 系统设置要求帖子内容不得少于 " + config.Minpostsize.ToString() + " 字多于 " + config.Maxpostsize.ToString() + " 字");
            }

            //新用户广告强力屏蔽检查
            if ((config.Disablepostad == 1) && useradminid < 1)  //如果开启新用户广告强力屏蔽检查或是游客
            {
                if ((config.Disablepostadpostcount != 0 && user.Posts <= config.Disablepostadpostcount) ||
                    (config.Disablepostadregminute != 0 && DateTime.Now.AddMinutes(-config.Disablepostadregminute) <= Convert.ToDateTime(user.Joindate)))
                {
                    foreach (string regular in config.Disablepostadregular.Replace("\r", "").Split('\n'))
                    {
                        if (Posts.IsAD(regular, postTitle, postMessage))
                        {
                            AddErrLine("发帖失败，内容中有不符合新用户强力广告屏蔽规则的字符，请检查标题和内容，如有疑问请与管理员联系");
                            return;
                        }
                    }
                }
            }

            #endregion
            string[] pollitem = Utils.SplitString(DNTRequest.GetString("PollItemname"), "\r\n");
            int topicprice = 0;
            string tmpprice = DNTRequest.GetString("topicprice");

            if (postinfo.Layer == 0)
            {

                #region 投票信息
                //string[] pollitem = Utils.SplitString(DNTRequest.GetString("PollItemname"), "\r\n");

                if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("updatepoll")) && topic.Special == 1)
                {
                    pollinfo.Multiple = DNTRequest.GetInt("multiple", 0);

                    // 验证用户是否有发布投票的权限
                    if (usergroupinfo.Allowpostpoll != 1)
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有发布投票的权限");
                        return;
                    }

                    if (pollitem.Length < 2)
                        AddErrLine("投票项不得少于2个");
                    else if (pollitem.Length > config.Maxpolloptions)
                        AddErrLine("系统设置为投票项不得多于" + config.Maxpolloptions + "个");
                    else
                    {
                        for (int i = 0; i < pollitem.Length; i++)
                            if (Utils.StrIsNullOrEmpty(pollitem[i]))
                                AddErrLine("投票项不能为空");
                    }
                }
                #endregion

                #region 悬赏信息
                //int topicprice = 0;
                //string tmpprice = DNTRequest.GetString("topicprice");

                if (Regex.IsMatch(tmpprice, "^[0-9]*[0-9][0-9]*$") || tmpprice == string.Empty)
                {
                    topicprice = Utils.StrToInt(tmpprice, 0) > 32767 ? 32767 : Utils.StrToInt(tmpprice, 0);
                    //当不是正在进行的悬赏...
                    if (topic.Special != 2)
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
                        if (usergroupinfo.Radminid != 1)
                        {
                            if (usergroupinfo.Allowbonus == 0)
                                AddErrLine(string.Format("您当前的身份 \"{0}\" 未被允许进行悬赏", usergroupinfo.Grouptitle));

                            if (topicprice < usergroupinfo.Minbonusprice || topicprice > usergroupinfo.Maxbonusprice)
                                AddErrLine(string.Format("悬赏价格超出范围, 您应在 {0} - {1} {2}{3} 范围内进行悬赏", usergroupinfo.Minbonusprice, usergroupinfo.Maxbonusprice,
                                    userextcreditsinfo.Unit, userextcreditsinfo.Name));
                        }
                    }
                }
                else
                {
                    if (topic.Special != 2)
                        AddErrLine("主题售价只能为整数");
                    else
                        AddErrLine("悬赏价格只能为整数");
                }
                #endregion

                #region 辩论信息
                if (!Utils.StrIsNullOrEmpty(DNTRequest.GetString("updatedebate")) && topic.Special == 4)
                {
                    if (usergroupinfo.Allowdebate != 1)
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有发布辩论的权限");
                        return;
                    }
                    if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("positiveopinion")))
                    {
                        AddErrLine("正方观点不能为空");
                        return;
                    }
                    if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("negativeopinion")))
                    {
                        AddErrLine("反方观点不能为空");
                        return;
                    }
                    if (Utils.StrIsNullOrEmpty(DNTRequest.GetString("terminaltime")))
                    {
                        AddErrLine("辩论的结束日期不能为空");
                        return;
                    }
                    if (!Utils.IsDateString(DNTRequest.GetString("terminaltime")))
                    {
                        AddErrLine("结束日期格式不正确");
                        return;
                    }
                }
                #endregion

            }

            #region 绑定并检查主题和帖子信息
            if (useradminid == 1)
            {
                postinfo.Title = Utils.HtmlEncode(postTitle);

                if (usergroupinfo.Allowhtml == 0)
                    postinfo.Message = Utils.HtmlEncode(postMessage);
                else
                    postinfo.Message = ishtmlon ? postMessage :
                            Utils.HtmlEncode(postMessage);
            }
            else
            {
                postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(postTitle));
                    
                if (usergroupinfo.Allowhtml == 0)
                    postinfo.Message = Utils.HtmlEncode(ForumUtils.BanWordFilter(postMessage));
                else
                    postinfo.Message = ishtmlon ? ForumUtils.BanWordFilter(postMessage) :
                            Utils.HtmlEncode(ForumUtils.BanWordFilter(postMessage));
            }
            postinfo.Title = postinfo.Title.Length > 60 ? postinfo.Title.Substring(0, 60) : postinfo.Title;

            if (useradminid != 1 && (ForumUtils.HasBannedWord(postTitle) || ForumUtils.HasBannedWord(postMessage)))
            {
                string bannedWord = ForumUtils.GetBannedWord(postTitle) == string.Empty ? ForumUtils.GetBannedWord(postMessage) : ForumUtils.GetBannedWord(postTitle);
                AddErrLine(string.Format("对不起, 您提交的内容包含不良信息  <font color=\"red\">{0}</font>, 请返回修改!", bannedWord));
                return;
            }

            //if (useradminid != 1 && (ForumUtils.HasAuditWord(postinfo.Title) || ForumUtils.HasAuditWord(postinfo.Message)))
            //{
            //    AddErrLine("对不起, 管理员设置了需要对发帖进行审核, 您没有权力编辑已通过审核的帖子, 请返回修改!");
            //    return;
            //}

            topic.Displayorder = Topics.GetTitleDisplayOrder(usergroupinfo, useradminid, forum, topic, message, disablepostctrl);

            #endregion
            // 检察上面验证是否有错误
            if (IsErr())
                return;
            //如果是不是管理员组,或者编辑间隔超过60秒,则附加编辑信息
            if (Utils.StrDateDiffSeconds(postinfo.Postdatetime, 60) > 0 && config.Editedby == 1 && useradminid != 1)
                postinfo.Lastedit = username + " 最后编辑于 " + Utils.GetDateTime();

            postinfo.Usesig = Utils.StrToInt(DNTRequest.GetString("usesig"), 0);
            postinfo.Htmlon = (usergroupinfo.Allowhtml == 1 && ishtmlon ? 1 : 0);
            postinfo.Smileyoff = smileyoff == 0 ? TypeConverter.StrToInt(DNTRequest.GetString("smileyoff")) : smileyoff;
            postinfo.Bbcodeoff = (usergroupinfo.Allowcusbbcode == 1 ? TypeConverter.StrToInt(DNTRequest.GetString("bbcodeoff")) : 1);
            postinfo.Parseurloff = TypeConverter.StrToInt(DNTRequest.GetString("parseurloff"));
            postinfo.Invisible = needaudit ? 1 : 0;

            //如果当前用户就是作者或所在管理组有编辑的权限
            if (userid == postinfo.Posterid || (admininfo != null && admininfo.Alloweditpost == 1 && Moderators.IsModer(useradminid, userid, forumid)))
                alloweditpost = true;
            else
            {
                AddErrLine("您当前的身份不是作者");
                return;
            }

            if (!alloweditpost)
            {
                AddErrLine("您当前的身份没有编辑帖子的权限");
                return;
            }

            if (alloweditpost)
                SetTopicInfo(pollitem, topicprice, postMessage);
        }

        /// <summary>
        /// 设置相关主题信息
        /// </summary>
        /// <param name="pollitem"></param>
        /// <param name="topicprice"></param>
        /// <param name="postmessage"></param>
        private void SetTopicInfo(string[] pollitem, int topicprice, string postmessage)
        {
            if (postinfo.Layer == 0)
            {
                ///修改投票信息
                StringBuilder itemvaluelist = new StringBuilder("");
                if (topic.Special == 1)
                {
                    string pollItemname = Utils.HtmlEncode(DNTRequest.GetFormString("PollItemname").Trim());

                    if (!Utils.StrIsNullOrEmpty(pollItemname))
                    {
                        int multiple = DNTRequest.GetString("multiple") == "on" ? 1 : 0;
                        int maxchoices = DNTRequest.GetInt("maxchoices", 0);

                        if (multiple == 1 && maxchoices > pollitem.Length)
                            maxchoices = pollitem.Length;

                        if (!Polls.UpdatePoll(topic.Tid, multiple, pollitem.Length, DNTRequest.GetFormString("PollOptionID").Trim(), pollItemname, DNTRequest.GetFormString("PollOptionDisplayOrder").Trim(), DNTRequest.GetString("enddatetime"), maxchoices, DNTRequest.GetString("visiblepoll") == "on" ? 1 : 0, DNTRequest.GetString("allowview") == "on" ? 1 : 0))
                        {
                            AddErrLine("投票错误,请检查显示顺序");
                            return;
                        }
                    }
                    else
                    {
                        AddErrLine("投票项为空");
                        return;
                    }
                }

                //修改辩论信息
                if (topic.Special == 4)
                {
                    debateinfo.Positiveopinion = DNTRequest.GetString("positiveopinion");
                    debateinfo.Negativeopinion = DNTRequest.GetString("negativeopinion");
                    debateinfo.Terminaltime = TypeConverter.StrToDateTime(DNTRequest.GetString("terminaltime"));
                    if (!Debates.UpdateDebateTopic(debateinfo))
                    {
                        AddErrLine("辩论修改选择了无效的主题");
                        return;
                    }
                }

                int iconid = DNTRequest.GetInt("iconid", 0);
                topic.Iconid = (iconid > 15 || iconid < 0) ? 0 : iconid;
                topic.Title = postinfo.Title;

                //悬赏差价处理
                if (topic.Special == 2)
                {
                    int pricediff = topicprice - topic.Price;
                    if (pricediff > 0)
                    {
                        if (bonusCreditsTrans < 1 || bonusCreditsTrans > 8)
                        {
                            AddErrLine("系统未设置\"交易积分设置\", 无法判断当前要使用的(扩展)积分字段, 暂时无法发布悬赏"); return;
                        }
                        //扣分
                        if (usergroupinfo.Radminid != 1 && Users.GetUserExtCredits(topic.Posterid, bonusCreditsTrans) < pricediff)
                        {
                            AddErrLine("主题作者 " + Scoresets.GetValidScoreName()[bonusCreditsTrans] + " 不足, 无法追加悬赏");
                            return;
                        }
                        else
                        {
                            topic.Price = topicprice;
                            Users.UpdateUserExtCredits(topic.Posterid, bonusCreditsTrans,
                                                      -pricediff * (Scoresets.GetCreditsTax() + 1)); //计算税后的实际支付
                        }
                    }
                    else if (pricediff < 0 && usergroupinfo.Radminid != 1)
                    {
                        AddErrLine("不能降低悬赏价格");
                        return;
                    }
                }
                else if (topic.Special == 0)//普通主题,出售
                {
                    topic.Price = topicprice;
                }
                if (usergroupinfo.Allowsetreadperm == 1)
                    topic.Readperm = DNTRequest.GetInt("topicreadperm", 0) > 255 ? 255 : DNTRequest.GetInt("topicreadperm", 0);

                if (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1)
                    topic.Hide = 1;

                topic.Typeid = DNTRequest.GetFormInt("typeid", 0);

                htmltitle = DNTRequest.GetString("htmltitle").Trim();
                if (!Utils.StrIsNullOrEmpty(htmltitle) && Utils.HtmlDecode(htmltitle).Trim() != topic.Title)
                {
                    //按照  附加位/htmltitle(1位)/magic(3位)/以后扩展（未知位数） 的方式来存储,  11001
                    topic.Magic = 11000;
                }
                else
                    topic.Magic = 0;

                topic.Displayorder = Topics.GetTitleDisplayOrder(usergroupinfo, useradminid, forum, topic, message, disablepostctrl);

                ForumTags.DeleteTopicTags(topic.Tid);
                Topics.DeleteRelatedTopics(topic.Tid);
                string tags = DNTRequest.GetString("tags").Trim();
                if (enabletag && !Utils.StrIsNullOrEmpty(tags))
                {
                    if (ForumUtils.InBanWordArray(tags))
                    {
                        AddErrLine("标签中含有系统禁止词语,请修改");
                        return;
                    }

                    string[] tagArray = Utils.SplitString(tags, " ", true, 2, 10);
                    if (tagArray.Length > 0 && tagArray.Length <= 5)
                    {
                        topic.Magic = Topics.SetMagicValue(topic.Magic, MagicType.TopicTag, 1);
                        ForumTags.CreateTopicTags(tagArray, topic.Tid, userid, Utils.GetDateTime());
                    }
                    else
                    {
                        AddErrLine("超过标签数的最大限制或单个标签长度没有介于2-10之间，最多可填写 5 个标签");
                        return;
                    }
                }

                Topics.UpdateTopic(topic);

                //保存htmltitle
                if (canhtmltitle && !Utils.StrIsNullOrEmpty(htmltitle) && htmltitle != topic.Title)
                    Topics.WriteHtmlTitleFile(Utils.RemoveUnsafeHtml(htmltitle), topic.Tid);
            }
            else
            {
                if (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1)
                {
                    topic.Hide = 1;
                    Topics.UpdateTopic(topic);
                }
            }
        }

        /// <summary>
        /// 设置相关附件信息
        /// </summary>
        /// <returns></returns>
        private StringBuilder SetAttachmentInfo()
        {
            string attachId = DNTRequest.GetFormString("attachid");
            StringBuilder sb = new StringBuilder();
            AttachmentInfo[] editPostAttachArray = Attachments.GetEditPostAttachArray(0, attachId);
            List<AttachmentInfo> newUploadAttachList = new List<AttachmentInfo>();

            //获取本次编辑操作上传的附件
            foreach (AttachmentInfo info in editPostAttachArray)
            {
                if (info.Pid == 0)
                    newUploadAttachList.Add(info);
            }

            if (!string.IsNullOrEmpty(attachId))
                Attachments.UpdateAttachment(editPostAttachArray, topic.Tid, postinfo.Pid, postinfo, ref sb, userid, config, usergroupinfo);

            //加入相册
            if (config.Enablealbum == 1 && apb != null && newUploadAttachList != null)
                sb.Append(apb.CreateAttachment(newUploadAttachList.ToArray(), usergroupid, userid, username));

            return sb;
        }

        protected string FormatDateTimeString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        protected string AttachmentList()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");

            foreach (DataRow dr in attachmentlist.Rows)
            {
                if (dr["isimage"].ToString().Trim() == "0")
                    continue;

                sb.AppendFormat("{{\\'aid\\':{0},\\'attachment\\':\\'{1}\\',\\'attachkey\\':\\'{2}\\',\\'description\\':\\'{3}\\'}},",
                    dr["aid"].ToString(), dr["attachment"].ToString().Trim().Replace("'", "\\'"), GetKey(dr["aid"].ToString()), dr["description"].ToString().Trim().Replace("'", "\\'"));
            }
            return sb.ToString().TrimEnd(',') + "]";

            //StringBuilder html = new StringBuilder(5120);
            //bool find = false;
            //html.Append("<table id=\"uploadattachlist\" cellspacing=\"2\" cellpadding=\"2\" class=\"imgl\">");
            //int count = 1;
            //foreach (DataRow dr in attachmentlist.Rows)
            //{
            //    if (dr["isimage"].ToString().Trim() == "0")
            //        continue;
            //    find = true;
            //    if (count % 4 == 1)
            //        html.Append("<tr>");
            //    html.AppendFormat("<td valign=\"bottom\" id=\"image_td_{0}\" width=\"25%\">", dr["aid"].ToString());
            //    html.AppendFormat("<input type=\"hidden\" value=\"{1}\" name=\"attachid\"><input type=\"hidden\" value=\"0\" name=\"attachprice_{1}\"><input type=\"hidden\" value=\"0\" name=\"readperm_{1}\"><a href=\"javascript:;\" title=\"{0}\"><img src=\"tools/ajax.aspx?t=image&aid={1}&size=300x300&key={2}&nocache=yes&type=fixnone\" id=\"image_{1}\" onclick=\"insertAttachimgTag('{1}')\" width=\"110\" cwidth=\"300\" /></a>",
            //        dr["attachment"].ToString().Trim(), dr["aid"].ToString().Trim(), GetKey(dr["aid"].ToString().Trim()));
            //    html.Append("<p class=\"imgf\">");
            //    html.AppendFormat("<a href=\"javascript:;\" onclick=\"delImgAttach({0},0)\" class=\"del y\">删除</a>", dr["aid"].ToString().Trim());
            //    html.AppendFormat("<input type=\"text\" class=\"px xg2\" value=\"{0}\" onclick=\"this.style.display='none';$('image_desc_{1}').style.display='';$('image_desc_{1}').focus();\" />", dr["description"].ToString().Trim(), dr["aid"].ToString().Trim());
            //    html.AppendFormat("<input type=\"text\" name=\"attachdesc_{0}\" class=\"px\" style=\"display: none\" id=\"image_desc_{0}\" value=\"{1}\" />", dr["aid"].ToString().Trim(), dr["description"].ToString().Trim());
            //    html.Append("</p>");
            //    html.Append("</td>");
            //    if (count % 4 == 0)
            //        html.Append("</tr>");
            //    count++;
            //}
            //string imageNumHidden = string.Format("<input id=\"imagenumhidden\" type=\"hidden\" value=\"{0}\"/>", count - 1);

            //if (--count % 4 != 0)
            //{
            //    for (int i = 0; i < 4 - count % 4; i++)
            //        html.Append("<td width=\"25%\"></td>");
            //    html.Append("</tr>");
            //}
            //html.Append("</table>");
            //html.Append(imageNumHidden);

            //if (find)
            //    return html.ToString();
            //else
            //    return "<input id=\"imagenumhidden\" type=\"hidden\" value=\"0\"/>";
        }

        private string GetKey(string aid)
        {
            return Discuz.Common.DES.Encode(aid.ToString() + ",300,300", Utils.MD5(aid)).Replace("+", "[");
        }
    }
}
