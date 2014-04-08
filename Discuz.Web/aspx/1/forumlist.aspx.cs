using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web
{
    /// <summary>
    /// 版块列表(分栏模式)
    /// </summary>
    public class forumlist : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前登录的用户简要信息
        /// </summary>
        public ShortUserInfo userinfo = new ShortUserInfo();
        /// <summary>
        /// 总在线数
        /// </summary>
        public int totalonline;
        /// <summary>
        /// 总在线注册用户数
        /// </summary>
        public int totalonlineuser = OnlineUsers.GetOnlineUserCount();
        /// <summary>
        /// 可用的扩展积分显示名称
        /// </summary>
        public string[] score = Scoresets.GetValidScoreName();
        #endregion
        protected override void ShowPage()
        {
            pagetitle = "版块列表";

            if (config.Rssstatus == 1)
                AddLinkRss("tools/rss.aspx", config.Forumtitle + "最新主题");

            if (userid != -1)
            {
                userinfo = Users.GetShortUserInfo(userid);
                newpmcount = userinfo.Newpm == 0 ? 0 :newpmcount;
            }

            OnlineUsers.UpdateAction(olid, UserAction.IndexShow.ActionID, 0, config.Onlinetimeout);
            // 获得统计信息
            totalonline = onlineusercount;
        }
    }
}