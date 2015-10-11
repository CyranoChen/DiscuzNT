using Discuz.Forum;
using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Space;
using Discuz.Plugin.Album;
using Discuz.Plugin.Mall;

namespace Discuz.Web.UI
{
    /// <summary>
    /// 主题显示页面基类
    /// </summary>
    public class TopicPage : PageBase
    {
        #region 变量定义
        /// <summary>
        /// 主题信息
        /// </summary>
        public TopicInfo topic;
        /// <summary>
        /// 所属版块Id
        /// </summary>
        public int forumid = DNTRequest.GetInt("forumid", 0);
        /// <summary>
        /// 主题Id
        /// </summary>
        public int topicid = DNTRequest.GetInt("topicid", -1);
        /// <summary>
        /// 是否显示需要登录后访问的错误提示
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// 当前版块信息
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// 是否有回复的权限
        /// </summary>
        public bool canreply = false;
        /// <summary>
        /// 是否是管理者
        /// </summary>
        public int ismoder = 0;
        /// <summary>
        /// 操作提示信息
        /// </summary>
        protected string msg = "";
        /// <summary>
        /// 是否有发表主题的权限
        /// </summary>
        public bool canposttopic = false;
        /// <summary>
        /// 当前版块的分页id
        /// </summary>
        public int forumpageid = DNTRequest.GetInt("forumpage", 1);
        /// <summary>
        /// 已经结束的辩论
        /// </summary>
        public bool isenddebate = false;
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<ShowtopicPageAttachmentInfo> attachmentlist;
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead = "";
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad = "";
        /// <summary>
        /// 快速编辑器背景广告
        /// </summary>
        public string[] quickbgad;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;
        /// <summary>
        /// 回复帖子数
        /// </summary>
        public int postcount;
        /// <summary>
        /// 分页页数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// 主题浏览量
        /// </summary>
        public int topicviews;
        /// <summary>
        /// 页内文字广告
        /// </summary>
        public string[] pagewordad = new string[0];
        /// <summary>
        /// 是否允许投票
        /// </summary>
        public bool allowvote = false;
        /// <summary>
        /// 是否解析URL
        /// </summary>
        public int parseurloff = Utils.StrToInt(DNTRequest.GetString("parseurloff"), 0);
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
        /// 参与投票的用户列表
        /// </summary>
        public string voters = "";
        /// <summary>
        /// 是否受发帖灌水限制
        /// </summary>
        public int disablepostctrl = 0;
        /// <summary>
        /// 用户的管理组信息
        /// </summary>
        public AdminGroupInfo admininfo = null;
        /// <summary>
        /// 是否只查看楼主帖子 1:只看楼主  0:显示全部
        /// </summary>
        public string onlyauthor = DNTRequest.GetString("onlyauthor");
        /// <summary>
        /// 当前的主题类型
        /// </summary>
        public string topictypes = "";
        /// <summary>
        /// 主题鉴定信息
        /// </summary>
        public TopicIdentify topicidentify;
        /// <summary>
        /// 可用的扩展积分名称列表
        /// </summary>
        public string[] score = new string[0];
        /// <summary>
        /// 可用的扩展积分单位列表
        /// </summary>
        public string[] scoreunit = new string[0];
        /// <summary>
        /// 帖内广告
        /// </summary>
        public string inpostad = "";
        /// <summary>
        /// 快速发帖广告
        /// </summary>
        public string quickeditorad = "";
        /// <summary>
        /// 相关主题集合
        /// </summary>
        public List<TopicInfo> relatedtopics = new List<TopicInfo>();
        /// <summary>
        /// 每页帖子数
        /// </summary>
        public int ppp = Utils.StrToInt(ForumUtils.GetCookie("ppp"), GeneralConfigs.GetConfig().Ppp);
        /// <summary>
        /// 本版是否启用了Tag
        /// </summary>
        public bool enabletag = false;
        /// <summary>
        /// 帖间通栏广告
        /// </summary>
        public string postleaderboardad = "";
        /// <summary>
        /// 是否默认回帖通知用户
        /// </summary>
        public int replynotificationstatus = GeneralConfigs.GetConfig().Replynotificationstatus;
        /// <summary>
        /// 是否默认回帖短信息通知用户
        /// </summary>
        public int replyemailstatus = GeneralConfigs.GetConfig().Replyemailstatus;
        /// <summary>
        /// 附件下载提示信息
        /// </summary>
        public string downloadattachmenttip = "";
        /// <summary>
        /// 当前用户是否在新手见习期
        /// </summary>
        public bool isnewbie = false;
        /// <summary>
        /// 需要定位的帖子ID
        /// </summary>
        public int postid = DNTRequest.GetInt("postid", 0);

        #endregion

        /// <summary>
        /// 验证信息
        /// </summary>
        /// <returns></returns>
        protected bool ValidateInfo()
        {
            #region 主题信息验证
            if (topicid == -1 || topic == null)
            {
                AddErrLine("无效或不存在的主题ID");
                return false;
            }

            if (topic.Closed > 1)
            {
                topicid = topic.Closed;
                topic = Topics.GetTopicInfo(topicid);
                if (topic == null || topic.Closed > 1)
                {
                    AddErrLine("不存在的主题ID");
                    return false;
                }
            }

            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1 && ismoder != 1)
            {
                AddErrLine(string.Format("本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够", topic.Readperm, usergroupinfo.Grouptitle));
                if (userid == -1)
                    needlogin = true;

                return false;
            }

            if (topic.Displayorder == -1)
            {
                AddErrLine("此主题已被删除！");
                return false;
            }
            if (topic.Displayorder == -2)
            {
                AddErrLine("此主题未经审核！");
                return false;
            }
            #endregion

            if (!ValidateForumPassword()) return false;

            return ValidateAuthority();
        }

        /// <summary>
        /// 权限认证，包括回复，下载附件，发主题等
        /// </summary>
        /// <returns></returns>
        public bool ValidateAuthority()
        {
            if (!UserAuthority.VisitAuthority(forum, usergroupinfo, userid, ref msg))
            {
                AddErrLine(msg);
                if (userid == -1)
                    needlogin = true;

                return false;
            }

            //是否有回复权限           
            canreply = ismoder == 1 ? true : UserAuthority.PostReply(forum, userid, usergroupinfo, topic);

            //判断是否有发主题权限
            if (userid > -1)
            {
                canposttopic = UserAuthority.PostAuthority(forum, usergroupinfo, userid, ref msg);
                if (!canposttopic)
                {
                    if (!pagename.StartsWith("showtopic") && !pagename.StartsWith("showtree"))
                    {
                        AddErrLine(msg);
                        return false;
                    }
                }
            }

            //如果当前用户非管理员并且论坛设定了禁止发帖时间段，当前时间如果在其中的一个时间段内，不允许用户发帖
            if (useradminid != 1 && usergroupinfo.Disableperiodctrl != 1)
            {
                string visittime = "";
                if (Scoresets.BetweenTime(config.Postbanperiods, out visittime))
                    canposttopic = false;

                isnewbie = UserAuthority.CheckNewbieSpan(userid);
            }

            return true;
        }

        /// <summary>
        /// 检验版块密码
        /// </summary>
        /// <returns></returns>
        public bool ValidateForumPassword()
        {
            if (!Utils.StrIsNullOrEmpty(forum.Password) && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                if (config.Aspxrewrite == 1)
                    System.Web.HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "showforum-" + forumid + config.Extname, true);
                else
                    System.Web.HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "showforum.aspx?forumid=" + forumid + "&forumpage=" + forumpageid, true);

                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取版块的广告信息
        /// </summary>
        /// <param name="forumid"></param>
        public void GetForumAds(int forumid)
        {
            headerad = Advertisements.GetOneHeaderAd("", forumid);
            footerad = Advertisements.GetOneFooterAd("", forumid);
            pagewordad = Advertisements.GetPageWordAd("", forumid);
            doublead = Advertisements.GetDoubleAd("", forumid);
            floatad = Advertisements.GetFloatAd("", forumid);

            if (forumid > 0)
                postleaderboardad = Advertisements.GetOnePostLeaderboardAD("", forumid);
        }



        /// <summary>
        /// 编辑器状态
        /// </summary>
        public void EditorState()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("var Allowhtml=1;\r\n");
            smileyoff = 1 - forum.Allowsmilies;
            sb.Append("var Allowsmilies=" + (1 - smileyoff) + ";\r\n");

            if (forum.Allowbbcode == 1 && usergroupinfo.Allowcusbbcode == 1)
                bbcodeoff = 0;

            sb.Append("var Allowbbcode=" + (1 - bbcodeoff) + ";\r\n");
            sb.Append("var Allowimgcode=" + forum.Allowimgcode + ";\r\n");

            AddScript(sb.ToString());
        }

        /// <summary>
        /// 更新页面Meta信息
        /// </summary>
        public void UpdateMetaInfo(string metadescritpion)
        {
            //更新页面Meta中的Description项, 提高SEO友好性
            string seokeyword = config.Seokeywords;
            metadescritpion = metadescritpion.Length > 100 ? metadescritpion.Substring(0, 100) : metadescritpion;

            //获取相关主题集合
            if (enabletag && Topics.GetMagicValue(topic.Magic, MagicType.TopicTag) == 1)
                seokeyword = ForumTags.GetTagsByTopicId(topic.Tid);

            //更新页面Meta中的keyword,description项, 提高SEO友好性
            UpdateMetaInfo(seokeyword, metadescritpion, config.Seohead);
        }

        /// <summary>
        /// 获取主题信息
        /// </summary>
        /// <returns></returns>
        public TopicInfo GetTopicInfo()
        {
            string go = DNTRequest.GetString("go").Trim().ToLower();

            if (go == "")
                forumid = 0;
            else if (forumid == 0)
                go = "";

            TopicInfo topicInfo;
            // 获取该主题的信息
            switch (go)
            {
                case "prev": topicInfo = Topics.GetTopicInfo(topicid, forumid, 1); break;
                case "next": topicInfo = Topics.GetTopicInfo(topicid, forumid, 2); break;
                default: topicInfo = Topics.GetTopicInfo(topicid); break;
            }

            if (topicInfo == null)
            {
                if (go == "prev")
                    msg = "没有更旧的主题, 请返回";
                else if (go == "next")
                    msg = "没有更新的主题, 请返回";
                else
                    msg = "该主题不存在";

                AddErrLine(msg);
                GetForumAds(0);
            }
            return topicInfo;
        }

        /// <summary>
        /// 获取帖子广告信息
        /// </summary>
        public void GetPostAds(PostpramsInfo postpramsInfo, int count)
        {
            //加载帖内广告
            inpostad = Advertisements.GetInPostAd("", forumid, templatepath, count > postpramsInfo.Pagesize ? postpramsInfo.Pagesize : count);
            //快速发帖广告
            quickeditorad = Advertisements.GetQuickEditorAD("", forumid);
            //快速编辑器背景广告
            quickbgad = Advertisements.GetQuickEditorBgAd("", forumid);
            if (quickbgad.Length <= 1)
                quickbgad = new string[2] { "", "" };
        }

        /// <summary>
        /// 获取当前页数和页面ID
        /// </summary>
        public void BindPageCountAndId()
        {
            ppp = (ppp <= 0 ? config.Ppp : ppp);
            postcount = Posts.GetPostCountByPosterId(onlyauthor, topicid, topic.Posterid, topic.Replies);

            //获取总页数
            pagecount = postcount % ppp == 0 ? postcount / ppp : postcount / ppp + 1;
            if (pagecount == 0)
                pagecount = 1;

            // 得到当前用户请求的页数
            pageid = DNTRequest.GetString("page").ToLower().Equals("end") ? pagecount : DNTRequest.GetInt("page", 1);
            //如果指定了要定位的帖子ID
            if (postid > 0)
                pageid = Posts.GetPostCountBeforePid(postid, topicid) / ppp + 1;
            

            //修正请求页数中可能的错误
            pageid = pageid < 1 ? 1 : pageid;
            pageid = pageid > pagecount ? pagecount : pageid;
        }

        /// <summary>
        /// 检查是否具有版主的身份
        /// </summary>
        public void IsModer()
        {
            // 检查是否具有版主的身份
            if (useradminid != 0)
            {
                ismoder = Moderators.IsModer(useradminid, userid, forum.Fid) ? 1 : 0;
                admininfo = AdminGroups.GetAdminGroupInfo(usergroupid); //得到管理组信息
                if (admininfo != null)
                    disablepostctrl = admininfo.Disablepostctrl;
            }
        }

        public void BindDownloadAttachmentTip()
        {
            if (!Scoresets.IsSetDownLoadAttachScore())
                return;

            float[] extCredits = Scoresets.GetUserExtCredits(CreditsOperationType.DownloadAttachment);
            string[] extCreditNames = Scoresets.GetValidScoreName();
            string[] extCreditUnits = Scoresets.GetValidScoreUnit();

            for (int i = 0; i < extCredits.Length; i++)
            {
                if (extCredits[i] < 0)//只提示扣分的扩展积分信息
                {
                    downloadattachmenttip += string.Format("{0}:{1} {2};", extCreditNames[i + 1], System.Math.Abs(extCredits[i]), extCreditUnits[i + 1]);
                }
            }
            downloadattachmenttip = downloadattachmenttip.TrimEnd(';');

        }
    }
}
