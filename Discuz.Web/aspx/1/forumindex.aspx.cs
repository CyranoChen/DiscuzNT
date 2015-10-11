using System;
using System.Data;
using System.Web;
using Discuz.Common;
using Discuz.Forum;

using Discuz.Config;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Web
{
	/// <summary>
	/// 论坛首页
	/// </summary>
	public class forumindex : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 论坛版块列表
        /// </summary>
        public Discuz.Common.Generic.List<IndexPageForumInfo> forumlist;
        /// <summary>
        /// 在线用户列表
        /// </summary>
        public Discuz.Common.Generic.List<OnlineUserInfo> onlineuserlist;
        /// <summary>
        /// 当前登录的用户短消息列表
        /// </summary>
        public Discuz.Common.Generic.List<PrivateMessageInfo> pmlist;
        /// <summary>
        /// 当前用户最后访问时间
        /// </summary>
		public string lastvisit = "未知";
        /// <summary>
        /// 友情链接列表
        /// </summary>
        public DataTable forumlinklist = Caches.GetForumLinkList();
        /// <summary>
        /// 公告列表
        /// </summary>
		public DataTable announcementlist;
        /// <summary>
        /// 页内文字广告
        /// </summary>
		public string[] pagewordad = new string[0];
        /// <summary>
        /// 对联广告
        /// </summary>
		public string doublead;
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad;
        /// <summary>
        ///  Silverlight广告
        /// </summary>
        public string mediaad;
        /// <summary>
        /// 分类间广告
        /// </summary>
        public string inforumad;
        /// <summary>
        /// 页内横幅广告
        /// </summary>
        public List<string> pagead = new List<string>();
        /// <summary>
        /// 公告数量
        /// </summary>
		public int announcementcount;
        /// <summary>
        /// 在线图例列表
        /// </summary>
		public string onlineiconlist = "";
        /// <summary>
        /// 当前登录用户的简要信息
        /// </summary>
        public ShortUserInfo userinfo = new ShortUserInfo();
        /// <summary>
        /// 总主题数
        /// </summary>
		public int totaltopic = 0;
        /// <summary>
        /// 总帖子数
        /// </summary>
		public int totalpost = 0;
        /// <summary>
        /// 总用户数
        /// </summary>
		public int totalusers;
        /// <summary>
        /// 今日帖数
        /// </summary>
        public int todayposts = 0;
        /// <summary>
        /// 昨日帖数
        /// </summary>
        public int yesterdayposts;
        /// <summary>
        /// 最高日帖数
        /// </summary>
        public int highestposts;
        /// <summary>
        /// 最高发帖日
        /// </summary>
        public string highestpostsdate;
        /// <summary>
        /// 友情链接数
        /// </summary>
		public int forumlinkcount;
        /// <summary>
        /// 最新注册的用户名
        /// </summary>
		public string lastusername;
        /// <summary>
        /// 最新注册的用户Id
        /// </summary>
		public int lastuserid;
        /// <summary>
        /// 总在线用户数
        /// </summary>
		public int totalonline;
        /// <summary>
        /// 总在线注册用户数
        /// </summary>
		public int totalonlineuser;
        /// <summary>
        /// 总在线游客数
        /// </summary>
		public int totalonlineguest;
        /// <summary>
        /// 总在线隐身用户数
        /// </summary>
		public int totalonlineinvisibleuser;
        /// <summary>
        /// 最高在线用户数
        /// </summary>
		public string highestonlineusercount;
        /// <summary>
        /// 最高在线用户数发生时间
        /// </summary>
		public string highestonlineusertime;
        /// <summary>
        /// 是否显示在线列表
        /// </summary>
		public bool showforumonline;
        /// <summary>
        /// 是否已经拥有个人空间
        /// </summary>
		public bool isactivespace; 
        /// <summary>
        /// 是否允许申请个人空间
        /// </summary>
		public bool isallowapply;       
        /// <summary>
        /// 可用的扩展积分显示名称
        /// </summary>
        public string[] score = Scoresets.GetValidScoreName();
        /// <summary>
        /// 弹出导航菜单的HTML代码
        /// </summary>
		public string navhomemenu = "";
        /// <summary>
        /// 是否显示短消息
        /// </summary>
        public bool showpmhint = false;
        /// <summary>
        /// 标签列表
        /// </summary>
        public TagInfo[] taglist;
        /// <summary>
        /// 首页每个分类下最多显示版块数
        /// </summary>
        public int maxsubcount = GeneralConfigs.GetConfig().Maxindexsubforumcount;
        /// <summary>
        /// 首页顶部模板风格选择列表框选项
        /// </summary>
        public string templatelistboxoptionsforforumindex = Caches.GetTemplateListBoxOptionsCache(true);
        /// <summary>
        /// 论坛热点配置信息
        /// </summary>
        public ForumHotConfigInfo forumhotconfiginfo = ForumHotConfigs.GetConfig();
        /// <summary>
        /// 是否受发帖控制限制
        /// </summary>
        public int disablepostctrl = 0;
        #endregion

        protected override void ShowPage()
		{
			pagetitle = "首页";
            if (userid > 0 && useradminid > 0)
            {
                AdminGroupInfo admingroupinfo = AdminGroups.GetAdminGroupInfo(usergroupid);
                if (admingroupinfo != null)
                    disablepostctrl = admingroupinfo.Disablepostctrl;
            }

            int toframe = DNTRequest.GetInt("f", 1);
            if (toframe == 0)
                ForumUtils.WriteCookie("isframe", "1");
            else
                toframe = Utils.StrToInt(ForumUtils.GetCookie("isframe"), -1) == -1 ? config.Isframeshow : Utils.StrToInt(ForumUtils.GetCookie("isframe"), -1);

			if (toframe == 2)
			{
				HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "frame.aspx");
				HttpContext.Current.Response.End();
                return;
			}

            if (config.Rssstatus == 1)
				AddLinkRss("tools/rss.aspx", "最新主题");

			OnlineUsers.UpdateAction(olid, UserAction.IndexShow.ActionID, 0, config.Onlinetimeout);

            //if (newpmcount > 0)
            //    pmlist = PrivateMessages.GetPrivateMessageListForIndex(userid,5,1,1);

			if (userid != -1)
			{
				userinfo = Users.GetShortUserInfo(userid);
                if (userinfo == null)
                {
                    userid = -1;
                    ForumUtils.ClearUserCookie("dnt");
                }
                else
                {
					newpmcount = userinfo.Newpm == 0 ? 0 : newpmcount;
				    lastvisit = userinfo.Lastvisit.ToString();
                    showpmhint = Convert.ToInt32(userinfo.Newsletter) > 4;
                }
			}

			navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);

			forumlist = Forums.GetForumIndexCollection(config.Hideprivate, usergroupid, config.Moddisplay, out totaltopic, out totalpost, out todayposts);
			forumlinkcount = forumlinklist.Rows.Count;

			//个人空间控制
            if (config.Enablespace == 1)
                GetSpacePerm();

			// 获得统计信息
			totalusers = TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("totalusers"));
			lastusername = Statistics.GetStatisticsRowItem("lastusername").Trim();
            lastuserid = TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("lastuserid"));
            yesterdayposts = TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("yesterdayposts"));
            highestposts = TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("highestposts"));
            highestpostsdate = Statistics.GetStatisticsRowItem("highestpostsdate").ToString().Trim();
            if (todayposts > highestposts)
            {
                highestposts = todayposts;
                highestpostsdate = DateTime.Now.ToString("yyyy-M-d");
            }
            totalonline = onlineusercount;
			showforumonline = false;
			onlineiconlist = Caches.GetOnlineGroupIconList();
			if (totalonline < config.Maxonlinelist || DNTRequest.GetString("showonline") == "yes")
			{
				showforumonline = true;
				//获得在线用户列表和图标
                onlineuserlist = OnlineUsers.GetOnlineUserCollection(out totalonline, out totalonlineguest, out totalonlineuser, out totalonlineinvisibleuser);
			}

			if (DNTRequest.GetString("showonline") == "no")
				showforumonline = false;

			highestonlineusercount = Statistics.GetStatisticsRowItem("highestonlineusercount");
			highestonlineusertime = DateTime.Parse(Statistics.GetStatisticsRowItem("highestonlineusertime")).ToString("yyyy-MM-dd HH:mm");
			// 得到公告
			announcementlist = Announcements.GetSimplifiedAnnouncementList(nowdatetime, "2999-01-01 00:00:00");
			announcementcount = announcementlist != null ? announcementlist.Rows.Count : 0;

            List<IndexPageForumInfo> topforum = new List<IndexPageForumInfo>();
            foreach (IndexPageForumInfo f in forumlist)
            {
                f.Description = UBB.ParseSimpleUBB(f.Description);
                if (f.Layer == 0)
                    topforum.Add(f);
            }
        
            taglist = config.Enabletag == 1 ? ForumTags.GetCachedHotForumTags(config.Hottagcount) : new TagInfo[0];

            ///得到广告列表
            headerad = Advertisements.GetOneHeaderAd("indexad", 0);
            footerad = Advertisements.GetOneFooterAd("indexad", 0);
            inforumad = Advertisements.GetInForumAd("indexad", 0, topforum, templatepath);
			pagewordad = Advertisements.GetPageWordAd("indexad", 0);
			doublead = Advertisements.GetDoubleAd("indexad", 0);
			floatad = Advertisements.GetFloatAd("indexad", 0);
            mediaad = Advertisements.GetMediaAd(templatepath, "indexad", 0);
            pagead = Advertisements.GetPageAd("indexad", 0);

            if (userid > 0)
            {
                if (oluserinfo.Newpms < 0)
                    Users.UpdateUserNewPMCount(userid, olid);
            }
		}

        private void GetSpacePerm()
        {
            isactivespace = false;
            isallowapply = true;
            if (userinfo.Spaceid > 0)
                isactivespace = true;
            else
            {
                if (userinfo.Spaceid < 0)
                    isallowapply = false;
                else
                {
                    SpaceActiveConfigInfo spaceconfiginfo = SpaceActiveConfigs.GetConfig();
                    if (spaceconfiginfo.AllowPostcount == "1" || spaceconfiginfo.AllowDigestcount == "1" || spaceconfiginfo.AllowScore == "1" || spaceconfiginfo.AllowUsergroups == "1")
                    {
                        if (spaceconfiginfo.AllowPostcount == "1")
                            isallowapply = isallowapply && (Convert.ToInt32(spaceconfiginfo.Postcount) <= userinfo.Posts);
                        if (spaceconfiginfo.AllowDigestcount == "1")
                            isallowapply = isallowapply && (Convert.ToInt32(spaceconfiginfo.Digestcount) <= userinfo.Digestposts);
                        if (spaceconfiginfo.AllowScore == "1")
                            isallowapply = isallowapply && (Convert.ToInt32(spaceconfiginfo.Score) <= userinfo.Credits);
                        if (spaceconfiginfo.AllowUsergroups == "1")
                            isallowapply = isallowapply && (("," + spaceconfiginfo.Usergroups + ",").IndexOf("," + userinfo.Groupid + ",") != -1);
                    }
                    else
                        isallowapply = false;
                }
            }
        }
	}
}
