using System;
using System.Data;
using System.Text;
using System.Web;

using Discuz.Common;
using Discuz.Common.Generic;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Plugin.Mall;

namespace Discuz.Web
{
    /// <summary>
    /// 查看版块页面
    /// </summary>
    public class showforum : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前版块在线用户列表
        /// </summary>
        public List<OnlineUserInfo> onlineuserlist;
        /// <summary>
        /// 主题列表
        /// </summary>
        public Discuz.Common.Generic.List<TopicInfo> topiclist = new Discuz.Common.Generic.List<TopicInfo>();
        /// <summary>
        /// 置顶主题列表
        /// </summary>
        public Discuz.Common.Generic.List<TopicInfo> toptopiclist = new Discuz.Common.Generic.List<TopicInfo>();
        /// <summary>
        /// 子版块列表
        /// </summary>
        public List<IndexPageForumInfo> subforumlist;
        /// <summary>
        /// 短消息列表
        /// </summary>
        public List<PrivateMessageInfo> pmlist;
        /// <summary>
        /// 在线图例列表
        /// </summary>
        public string onlineiconlist = Caches.GetOnlineGroupIconList();
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist = Announcements.GetSimplifiedAnnouncementList(Utils.GetDateTime(), "2999-01-01 00:00:00");
        /// <summary>
        /// 页内文字广告
        /// </summary>
        public string[] pagewordad = new string[0];
        /// <summary>
        /// 页内横幅广告
        /// </summary>
        public List<string> pagead = new List<string>();
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead;
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad;
        /// <summary>
        /// Silverlight广告
        /// </summary>
        public string mediaad;
        /// <summary>
        /// 快速发帖广告
        /// </summary>
        public string quickeditorad = "";
        /// <summary>
        /// 快速编辑器背景广告
        /// </summary>
        public string[] quickbgad;
        /// <summary>
        /// 当前版块信息
        /// </summary>
        public ForumInfo forum = new ForumInfo();
        /// <summary>
        /// 购买主题积分策略
        /// </summary>
        public UserExtcreditsInfo topicextcreditsinfo = new UserExtcreditsInfo();
        /// <summary>
        /// 悬赏积分策略
        /// </summary>
        public UserExtcreditsInfo bonusextcreditsinfo = new UserExtcreditsInfo();
        /// <summary>
        /// 当前版块总在线用户数
        /// </summary>
        public int forumtotalonline;
        /// <summary>
        /// 当前版块总在线注册用户数
        /// </summary>
        public int forumtotalonlineuser;
        /// <summary>
        /// 当前版块总在线游客数
        /// </summary>
        public int forumtotalonlineguest;
        /// <summary>
        /// 当前版块在线隐身用户数
        /// </summary>
        public int forumtotalonlineinvisibleuser;
        /// <summary>
        /// 当前版块ID
        /// </summary>
        public int forumid = DNTRequest.GetInt("forumid", -1);
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// 是否显示版块密码提示 1为显示, 0不显示
        /// </summary>
        public int showforumlogin;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 当前版块列表页码
        /// </summary>
        public int forumpageid = DNTRequest.GetInt("page", 1);
        /// <summary>
        /// 主题总数
        /// </summary>
        public int topiccount = 0;
        /// <summary>
        /// 分页总数
        /// </summary>
        public int pagecount = 1;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";
        /// <summary>
        /// 置顶主题数
        /// </summary>
        public int toptopiccount = 0;
        /// <summary>
        /// 版块跳转链接选项
        /// </summary>
        public string forumlistboxoptions;
        /// <summary>
        /// 是否显示在线列表
        /// </summary>
        public bool showforumonline = false;
        /// <summary>
        /// 是否受发帖控制限制
        /// </summary>
        public int disablepostctrl = 0;
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
        public int bbcodeoff;
        /// <summary>
        /// 是否使用签名
        /// </summary>
        public int usesig = ForumUtils.GetCookie("sigstatus") == "0" ? 0 : 1;
        /// <summary>
        /// 每页显示主题数
        /// </summary>
        public int tpp = TypeConverter.StrToInt(ForumUtils.GetCookie("tpp"));
        /// <summary>
        /// 每页显示帖子数
        /// </summary>
        public int ppp = TypeConverter.StrToInt(ForumUtils.GetCookie("ppp"));
        /// <summary>
        /// 是否是管理者
        /// </summary>
        public bool ismoder = false;
        /// <summary>
        /// 主题分类选项
        /// </summary>
        public string topictypeselectoptions;
        /// <summary>
        /// 主题分类Id
        /// </summary>
        public int topictypeid = DNTRequest.GetInt("typeid", -1);
        /// <summary>
        /// 过滤主题类型
        /// </summary>
        public string filter = Utils.HtmlEncode(DNTRequest.GetString("filter"));
        /// <summary>
        /// 是否允许发表主题
        /// </summary>
        public bool canposttopic = false;
        /// <summary>
        /// 是否允许快速发表主题
        /// </summary>
        public bool canquickpost = false;
        /// <summary>
        /// 是否显示需要登录后访问的错误提示
        /// </summary>
        public bool needlogin = false;
        /// <summary>
        /// 排序方式
        /// </summary>
        public int order = DNTRequest.GetInt("order", 1); //排序字段
        /// <summary>
        /// 时间范围
        /// </summary>
        public int interval = DNTRequest.GetInt("interval", 0);
        /// <summary>
        /// 排序方向
        /// </summary>
        public int direct = DNTRequest.GetInt("direct", 1); //排序方向[默认：降序]      
        /// <summary>
        /// 获取绑定相关版块的商品分类信息
        /// </summary>
        public string goodscategoryfid = GeneralConfigs.GetConfig().Enablemall <= 0 ? "{}" : Discuz.Plugin.Mall.MallPluginProvider.GetInstance().GetGoodsCategoryWithFid();
        /// <summary>
        /// 当前版块的主题类型链接串
        /// </summary>
        public string topictypeselectlink;
        /// <summary>
        /// 下一页
        /// </summary>
        public string nextpage = "";
        /// <summary>
        /// 弹出导航菜单的HTML代码
        /// </summary>
        public string navhomemenu = "";
        /// <summary>
        /// 获取访问过的版块列表
        /// </summary>
        public SimpleForumInfo[] visitedforums = Forums.GetVisitedForums();
        /// <summary>
        /// 是否显示访问过的版块列表菜单
        /// </summary>
        public bool showvisitedforumsmenu = false;
        /// <summary>
        /// 当前用户是否在新手见习期
        /// </summary>
        public bool isnewbie = false;
        private string msg = "";//提示信息

        private string condition = ""; //查询条件

        private string orderStr = "";//排序方式

        public int topicid = 0;

        public bool needaudit = false;
        #endregion

        protected override void ShowPage()
        {
            GetPostAds(forumid);

            if (userid > 0 && useradminid > 0)
            {
                AdminGroupInfo admingroupinfo = AdminGroups.GetAdminGroupInfo(usergroupid);
                if (admingroupinfo != null)
                    disablepostctrl = admingroupinfo.Disablepostctrl;
            }

            #region 获取版块信息
            if (forumid == -1)
            {
                AddLinkRss(forumpath + "tools/rss.aspx", "最新主题");
                AddErrLine("无效的版块ID");
                return;
            }
            forum = Forums.GetForumInfo(forumid);
            if (forum == null || forum.Fid < 1)
            {
                if (config.Rssstatus == 1)
                    AddLinkRss(forumpath + "tools/rss.aspx", Utils.EncodeHtml(config.Forumtitle) + " 最新主题");

                AddErrLine("不存在的版块ID");
                return;
            }
            #endregion

            if (config.Rssstatus == 1)
                AddLinkRss(forumpath + "tools/" + base.RssAspxRewrite(forum.Fid), Utils.EncodeHtml(forum.Name) + " 最新主题");

            if (JumpUrl(forum)) return;

            needaudit = UserAuthority.NeedAudit(forum, useradminid, userid, usergroupinfo);

            // 检查是否具有版主的身份
            if (useradminid > 0)
                ismoder = Moderators.IsModer(useradminid, userid, forumid);

            //设置搜索和排序条件
            SetSearchCondition();

            showforumlogin = IsShowForumLogin(forum);
            pagetitle = Utils.RemoveHtml(forum.Name);
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);
            forumnav = ShowForumAspxRewrite(ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname).Replace("\"showforum", "\"" + forumurl + "showforum"),
                                            forumid, pageid);
            topicextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetTopicAttachCreditsTrans());
            bonusextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetBonusCreditsTrans());

            #region 主题分类设置
            if (forum.Applytopictype == 1) //启用主题分类
                topictypeselectoptions = Forums.GetCurrentTopicTypesOption(forum.Fid, forum.Topictypes);

            if (forum.Viewbytopictype == 1) //允许按类别浏览
                topictypeselectlink = Forums.GetCurrentTopicTypesLink(forum.Fid, forum.Topictypes, forumurl + "showforum.aspx");
            #endregion

            //更新页面Meta中的keyword,description项, 提高SEO友好性
            UpdateMetaInfo(Utils.StrIsNullOrEmpty(forum.Seokeywords) ? config.Seokeywords : forum.Seokeywords,
                Utils.StrIsNullOrEmpty(forum.Seodescription) ? forum.Description : forum.Seodescription,
                config.Seohead);

            //设置编辑器状态
            SetEditorState();

            #region 访问和发帖权限校验
            if (!UserAuthority.VisitAuthority(forum, usergroupinfo, userid, ref msg))
            {
                AddErrLine(msg);
                needlogin = userid == -1;
                return;
            }

            canposttopic = UserAuthority.PostAuthority(forum, usergroupinfo, userid, ref msg);
            //　如果当前用户非管理员并且论坛设定了禁止发帖时间段，当前时间如果在其中的一个时间段内，不允许用户发帖
            if (useradminid != 1 && usergroupinfo.Disableperiodctrl != 1)
            {
                string visittime = "";
                if (canposttopic && Scoresets.BetweenTime(config.Postbanperiods, out visittime))
                    canposttopic = false;

                isnewbie = UserAuthority.CheckNewbieSpan(userid);
            }

            //是否显示快速发主题编辑器(全局权限判定，版块权限判定，是否是游客，游客需要显示，登录用户是否允许发主题且已过新手见习期)
            if ((config.Fastpost == 1 || config.Fastpost == 3) && forum.Allowspecialonly <= 0 && (userid < 0 || (canposttopic && !isnewbie)))
                canquickpost = true;
            #endregion

            // 得到子版块列表
            if (forum.Subforumcount > 0)
                subforumlist = Forums.GetSubForumCollection(forumid, forum.Colcount, config.Hideprivate, usergroupid, config.Moddisplay);
            if (!forum.Rules.Equals(""))
                forum.Rules = UBB.ParseSimpleUBB(forum.Rules);//替换版规中的UBB
            //获取主题总数
            topiccount = Topics.GetTopicCount(forumid, true, condition);

            #region 设置分页及主题列表信息
            // 得到Tpp设置
            if (tpp <= 0)
                tpp = config.Tpp;

            // 得到Ppp设置
            if (ppp <= 0)
                ppp = config.Ppp;

            //修正请求页数中可能的错误
            if (pageid < 1)
                pageid = 1;

            int toptopicpagecount = 0;

            if (forum.Layer > 0)
            {
                //获取当前页置顶主题列表
                DataRow dr = Topics.GetTopTopicListID(forumid);
                if (dr != null && !Utils.StrIsNullOrEmpty(dr["tid"].ToString()))
                    topiccount = topiccount + TypeConverter.ObjectToInt(dr["tid0Count"]);

                //获取总页数
                pagecount = topiccount % tpp == 0 ? topiccount / tpp : topiccount / tpp + 1;
                if (pagecount == 0)
                    pagecount = 1;
                if (pageid > pagecount)
                    pageid = pagecount;

                if (dr != null && !Utils.StrIsNullOrEmpty(dr["tid"].ToString()))
                {
                    toptopiccount = TypeConverter.ObjectToInt(dr["tidCount"]);
                    if (toptopiccount > tpp * (pageid - 1))
                    {
                        toptopiclist = Topics.GetTopTopicList(forumid, tpp, pageid, dr["tid"].ToString(), forum.Autoclose, forum.Topictypeprefix);
                        toptopicpagecount = toptopiccount / tpp;
                    }

                    if (toptopicpagecount >= pageid || (pageid == 1 && toptopicpagecount != toptopiccount))
                        topiclist = GetTopicInfoList(tpp - toptopiccount % tpp, pageid - toptopicpagecount, 0);
                    else
                        topiclist = GetTopicInfoList(tpp, pageid - toptopicpagecount, toptopiccount % tpp);
                }
                else
                {
                    toptopicpagecount = 0;
                    topiclist = GetTopicInfoList(tpp, pageid, 0);
                }

                //如果topiclist为空则更新当前论坛帖数
                if (topiclist == null || topiclist.Count == 0 || topiclist.Count > topiccount)
                    Forums.SetRealCurrentTopics(forum.Fid);

                SetPageNumber();
                //当版块数大于一个并且当版块数量为一个时不是版块自身时显示下拉菜单
                showvisitedforumsmenu = visitedforums != null && ((visitedforums.Length == 1 && visitedforums[0].Fid != forumid) || visitedforums.Length > 1);
                SetVisitedForumsCookie();
                //保存查看版块的页数
                Utils.WriteCookie("forumpageid", pageid.ToString(), 30);

                //判断是否需要生成游客缓存页面
                IsGuestCachePage();
            }
            #endregion

            #region 替换版规中的UBB
            forum.Description = UBB.ParseSimpleUBB(forum.Description);
            #endregion

            #region 更新在线信息
            OnlineUsers.UpdateAction(olid, UserAction.ShowForum.ActionID, forumid, forum.Name, -1, "");

            if ((forumtotalonline < config.Maxonlinelist && (config.Whosonlinestatus == 2 || config.Whosonlinestatus == 3)) || DNTRequest.GetString("showonline") == "yes")
            {

                showforumonline = true;
                onlineuserlist = OnlineUsers.GetForumOnlineUserCollection(forumid, out forumtotalonline, out forumtotalonlineguest,
                                                             out forumtotalonlineuser, out forumtotalonlineinvisibleuser);
            }
            //if (DNTRequest.GetString("showonline") != "no")
            //{
            //     showforumonline = false;
            //}

            if (DNTRequest.GetString("showonline") == "no")
            {
                showforumonline = false;
            }
            #endregion

            //修正版主列表
            if (forum.Moderators.Trim() != "")
            {
                string moderHtml = string.Empty;
                foreach (string m in forum.Moderators.Split(','))
                {
                    moderHtml += string.Format("<a href=\"{0}userinfo.aspx?username={1}\">{2}</a>,", forumpath, Utils.UrlEncode(m), m);
                }

                forum.Moderators = moderHtml.TrimEnd(',');
            }

            ForumUtils.UpdateVisitedForumsOptions(forumid);
        }

        /// <summary>
        /// 是否跳转链接
        /// </summary>
        /// <param name="forum"></param>
        /// <returns></returns>
        private bool JumpUrl(ForumInfo forumInfo)
        {
            //当版块有外部链接时,则直接跳转
            if (!Utils.StrIsNullOrEmpty(forumInfo.Redirect))
            {
                HttpContext.Current.Response.Redirect(forumInfo.Redirect);
                return true;
            }
            //当允许发表交易帖时,则跳转到相应的商品列表页
            if (config.Enablemall == 1 && forumInfo.Istrade == 1)
            {
                MallPluginBase mpb = MallPluginProvider.GetInstance();
                int categoryid = mpb.GetGoodsCategoryIdByFid(forumInfo.Fid);
                if (categoryid > 0)
                {
                    HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + base.ShowGoodsListAspxRewrite(categoryid, 1));
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 获取主题信息列表
        /// </summary>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="startNumber">置顶帖数量</param>
        /// <returns></returns>
        public List<TopicInfo> GetTopicInfoList(int pageSize, int pageIndex, int startNumber)
        {
            List<TopicInfo> topicList = new List<TopicInfo>();
            if (orderStr == "" && direct == 1)
            {
                topicList = Topics.GetTopicList(forumid, pageSize, pageIndex, startNumber, 600, config.Hottopic, forum.Autoclose,
                                                    forum.Topictypeprefix, condition);
            }
            else
            {
                if (direct == 0 && orderStr == string.Empty)
                    orderStr = "lastpostid";

                topicList = Topics.GetTopicList(forumid, pageSize, pageIndex, startNumber, 600, config.Hottopic, forum.Autoclose,
                                                forum.Topictypeprefix, condition, orderStr, direct);
            }
            return topicList;
        }

        /// <summary>
        /// 设置页码链接
        /// </summary>
        private void SetPageNumber()
        {
            if (DNTRequest.GetString("search") == "")
            {
                if (topictypeid == -1)
                {
                    if (config.Aspxrewrite == 1)
                    {
                        if (Utils.StrIsNullOrEmpty(filter))
                        {
                            if (config.Iisurlrewrite == 0)
                                pagenumbers = Utils.GetStaticPageNumbers(pageid, pagecount, (Utils.StrIsNullOrEmpty(forum.Rewritename) ? "showforum-" + forumid : forumpath + forum.Rewritename), config.Extname, 8, (!Utils.StrIsNullOrEmpty(forum.Rewritename) ? 1 : 0));
                            else
                                pagenumbers = Utils.GetStaticPageNumbers(pageid, pagecount, (Utils.StrIsNullOrEmpty(forum.Rewritename) ? "showforum-" + forumid : forumpath + forum.Rewritename), config.Extname, 8, (!Utils.StrIsNullOrEmpty(forum.Rewritename) ? 2 : 0));

                            if (pageid < pagecount)
                                nextpage = string.Format("<a href=\"{0}{1}\" class=\"next\">下一页</a>", forumpath, Urls.ShowForumAspxRewrite(forumid, pageid + 1, forum.Rewritename));
                        }
                        else
                        {
                            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, string.Format("{0}showforum.aspx?forumid={1}&filter={2}", forumpath, forumid, filter), 8);

                            if (pageid < pagecount)
                                nextpage = string.Format("<a href=\"{0}showforum.aspx?forumid={1}&filter={2}&page={3}\" class=\"next\">下一页</a>", forumpath, forumid, filter, pageid + 1);
                        }
                    }
                    else
                    {
                        pagenumbers = Utils.GetPageNumbers(pageid, pagecount, string.Format("{0}showforum.aspx?forumid={1}{2}", forumpath, forumid, (Utils.StrIsNullOrEmpty(filter) ? "" : "&filter=" + filter)), 8);

                        if (pageid < pagecount)
                            nextpage = string.Format("<a href=\"{0}showforum.aspx?forumid={1}{2}&page={3}\" class=\"next\">下一页</a>", forumpath, forumid, (Utils.StrIsNullOrEmpty(filter) ? "" : "&filter=" + filter), pageid + 1);
                    }
                }
                else //当有主题类型条件时
                {
                    pagenumbers = Utils.GetPageNumbers(pageid, pagecount, string.Format("{0}showforum.aspx?forumid={1}&typeid={2}{3}",
                        forumpath, forumid, topictypeid, (Utils.StrIsNullOrEmpty(filter) ? "" : "&filter=" + filter)), 8);

                    if (pageid < pagecount)
                        nextpage = string.Format("<a href=\"{0}showforum.aspx?forumid={1}&typeid={2}{3}&page={4}\" class=\"next\">下一页</a>", forumpath, forumid, topictypeid, (Utils.StrIsNullOrEmpty(filter) ? "" : "&filter=" + filter), pageid + 1);
                }
            }
            else
            {
                pagenumbers = Utils.GetPageNumbers(pageid, pagecount, string.Format("{0}showforum.aspx?search=1&cond={1}&order={2}&direct={3}&forumid={4}&interval={5}&typeid={6}{7}",
                                         forumpath, Utils.HtmlEncode(DNTRequest.GetString("cond").Trim()), order, direct, forumid, interval,
                                         topictypeid, (Utils.StrIsNullOrEmpty(filter) ? "" : "&filter=" + filter)), 8);

                if (pageid < pagecount)
                    nextpage = string.Format("<a href=\"{0}showforum.aspx?search=1&cond={1}&order={2}&direct={3}&forumid={4}&interval={5}&typeid={6}{7}&page={8}\" class=\"next\">下一页</a>",
                              forumpath, Utils.HtmlEncode(DNTRequest.GetString("cond").Trim()), order, direct,
                              forumid, interval, topictypeid, (Utils.StrIsNullOrEmpty(filter) ? "" : "&filter=" + filter), pageid + 1);
            }
        }

        /// <summary>
        /// 获取帖子广告信息
        /// </summary>
        public void GetPostAds(int forumid)
        {
            ///得到广告列表
            headerad = Advertisements.GetOneHeaderAd("", forumid);
            footerad = Advertisements.GetOneFooterAd("", forumid);

            pagewordad = Advertisements.GetPageWordAd("", forumid);
            pagead = Advertisements.GetPageAd("", forumid);
            doublead = Advertisements.GetDoubleAd("", forumid);
            floatad = Advertisements.GetFloatAd("", forumid);
            mediaad = Advertisements.GetMediaAd(templatepath, "", forumid);
            //快速发帖广告
            quickeditorad = Advertisements.GetQuickEditorAD("", forumid);

            //快速编辑器背景广告
            quickbgad = Advertisements.GetQuickEditorBgAd("", forumid);
            if (quickbgad.Length <= 1)
                quickbgad = new string[2] { "", "" };
        }

        /// <summary>
        /// 设置搜索和排序条件
        /// </summary>
        private void SetSearchCondition()
        {
            if (topictypeid >= 0)
                condition = Forums.ShowForumCondition(1, 0) + topictypeid;
            //condition = "" + topictypeid;

            if (!Utils.InArray(filter, "poll,reward,rewarded,rewarding,debate,digest"))//过滤参数值以防跨站注入
                filter = "";

            if (!Utils.StrIsNullOrEmpty(filter))
                condition += Topics.GetTopicFilterCondition(filter);

            if (DNTRequest.GetString("search").Trim() != "") //进行指定查询
            {
                //多少时间以来的数据
                if (interval < 1)
                    interval = 0;
                else if (topictypeid <= 0) //当有主题分类时，则不加入下面的日期查询条件
                    condition += Forums.ShowForumCondition(2, interval);

                //orderStr = (order == 2) ? Forums.ShowForumCondition(3, 0) : ""; //发布时间
                switch (order)
                {
                    case 2:
                        orderStr = Forums.ShowForumCondition(3, 0);//发布时间
                        break;
                    case 3:
                        orderStr = Forums.ShowForumCondition(4, 0);//浏览数
                        break;
                    case 4:
                        orderStr = Forums.ShowForumCondition(5, 0);//回复数
                        break;
                    default:
                        orderStr = string.Empty;
                        break;
                }
            }
        }

        /// <summary>
        /// 设置编辑器状态
        /// </summary>
        private void SetEditorState()
        {
            //编辑器状态
            StringBuilder sb = new StringBuilder();
            sb.Append("var Allowhtml=1;\r\n");
            smileyoff = 1 - forum.Allowsmilies;
            sb.Append("var Allowsmilies=" + (1 - smileyoff) + ";\r\n");

            bbcodeoff = (forum.Allowbbcode == 1 && usergroupinfo.Allowcusbbcode == 1) ? 0 : 1;
            sb.Append("var Allowbbcode=" + (1 - bbcodeoff) + ";\r\n");
            sb.Append("var Allowimgcode=" + forum.Allowimgcode + ";\r\n");
            AddScript(sb.ToString());
        }

        /// <summary>
        /// 是否显示版块密码提示 1为显示, 0不显示
        /// </summary>
        /// <param name="forum">版块信息</param>
        /// <returns></returns>
        private int IsShowForumLogin(ForumInfo forum)
        {
            // 是否显示版块密码提示 1为显示, 0不显示
            int showForumLogin = 1;
            // 如果版块未设密码
            if (Utils.StrIsNullOrEmpty(forum.Password))
                showForumLogin = 0;
            else
            {
                // 如果检测到相应的cookie正确
                if (Utils.MD5(forum.Password) == ForumUtils.GetCookie("forum" + forumid + "password"))
                    showForumLogin = 0;
                else
                {
                    // 如果用户提交的密码正确则保存cookie
                    if (forum.Password == DNTRequest.GetString("forumpassword"))
                    {
                        ForumUtils.WriteCookie("forum" + forum.Fid + "password", Utils.MD5(forum.Password));
                        showForumLogin = 0;
                    }
                }
            }
            return showForumLogin;
        }

        /// <summary>
        /// 设置访问过版块Cookie
        /// </summary>
        private void SetVisitedForumsCookie()
        {
            if (forum.Layer > 0)
            {
                ForumUtils.SetVisitedForumsCookie(forum.Fid.ToString());
            }
        }

        /// <summary>
        /// 判断是否需要生成游客缓存页面
        /// </summary>
        public void IsGuestCachePage()
        {
            //这里假设最后一页之前的所有页面未修改，均可被缓存
            if (userid == -1 && pageid > 0 && pageid < pagecount && ForumUtils.IsGuestCachePage(pageid, "showforum"))
            {
                isguestcachepage = 1;
            }
        }

    }
}
