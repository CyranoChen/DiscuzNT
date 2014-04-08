using System;
using System.Web;
using System.Data;
using System.Collections.Generic;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Forum
{
    /// <summary>
    /// 在线用户操作类
    /// </summary>
    public class OnlineUsers
    {
        private static object SynObject = new object();

        /// <summary>
        /// 获得在线用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public static int GetOnlineAllUserCount()
        {
            int onlineUserCountCacheMinute = GeneralConfigs.GetConfig().OnlineUserCountCacheMinute;
            if (onlineUserCountCacheMinute == 0)
                return Discuz.Data.OnlineUsers.GetOnlineAllUserCount();

            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            int onlineAllUserCount = TypeConverter.ObjectToInt(cache.RetrieveObject("/Forum/OnlineUserCount"));
            if (onlineAllUserCount != 0)
                return onlineAllUserCount;

            onlineAllUserCount = Discuz.Data.OnlineUsers.GetOnlineAllUserCount();
            //Discuz.Cache.ICacheStrategy ics = new RssCacheStrategy();
            //ics.TimeOut = onlineUserCountCacheMinute * 60;
            //cache.LoadCacheStrategy(ics);
            cache.AddObject("/Forum/OnlineUserCount", onlineAllUserCount, onlineUserCountCacheMinute * 60);
            //cache.LoadDefaultCacheStrategy();
            return onlineAllUserCount;
        }

        /// <summary>
        /// 返回缓存中在线用户总数
        /// </summary>
        /// <returns>缓存中在线用户总数</returns>
        public static int GetCacheOnlineAllUserCount()
        {
            int count = TypeConverter.StrToInt(Utils.GetCookie("onlineusercount"), 0);
            if (count == 0)
            {
                count = OnlineUsers.GetOnlineAllUserCount();
                Utils.WriteCookie("onlineusercount", count.ToString(), 3);
            }
            return count;
        }

        /// <summary>
        /// 清理之前的在线表记录(本方法在应用程序初始化时被调用)
        /// </summary>
        /// <returns></returns>
        public static int InitOnlineList()
        {
            return Discuz.Data.OnlineUsers.CreateOnlineTable();
        }

        /// <summary>
        /// 复位在线表, 如果系统未重启, 仅是应用程序重新启动, 则不会重新创建
        /// </summary>
        /// <returns></returns>
        public static int ResetOnlineList()
        {
            try
            {

                // 如果距离现在系统运行时间小于10分钟
                if (System.Environment.TickCount < 600000 && System.Environment.TickCount > 0)
                    return Discuz.Data.OnlineUsers.CreateOnlineTable();

                return -1;
            }
            catch
            {
                try
                {
                    return Discuz.Data.OnlineUsers.CreateOnlineTable();
                }
                catch
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// 获得在线注册用户总数量
        /// </summary>
        /// <returns>用户数量</returns>
        public static int GetOnlineUserCount()
        {
            return Discuz.Data.OnlineUsers.GetOnlineUserCount();
        }

        #region 根据不同条件查询在线用户信息


        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        /// <param name="totaluser">全部用户数</param>
        /// <param name="guest">游客数</param>
        /// <param name="user">登录用户数</param>
        /// <param name="invisibleuser">隐身会员数</param>
        /// <returns>线用户列表</returns>
        public static DataTable GetOnlineUserList(int totaluser, out int guest, out int user, out int invisibleuser)
        {
            DataTable dt = Discuz.Data.OnlineUsers.GetOnlineUserListTable();
            int highestonlineusercount = TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("highestonlineusercount"), 1);

            if (totaluser > highestonlineusercount)
            {
                if (Statistics.UpdateStatistics("highestonlineusercount", totaluser.ToString()) > 0)
                {
                    Statistics.UpdateStatistics("highestonlineusertime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Statistics.ReSetStatisticsCache();
                }
            }
            // 统计用户
            //DataRow[] dr = dt.Select("userid>0");
            user = Discuz.Data.OnlineUsers.GetOnlineUserCount();// dr == null ? 0 : dr.Length;

            //统计隐身用户
            if (EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Cacheonlineuser.Enable)
                invisibleuser = Discuz.Data.OnlineUsers.GetInvisibleOnlineUserCount();
            else
            {
                DataRow[] dr = dt.Select("invisible=1");
                invisibleuser = dr == null ? 0 : dr.Length;
            }
            //统计游客
            guest = totaluser > user ? totaluser - user : 0;

            //返回当前版块的在线用户表
            return dt;
        }
        #endregion


        /// <summary>
        /// 返回在线用户图例
        /// </summary>
        /// <returns>在线用户图例</returns>
        private static DataTable GetOnlineGroupIconTable()
        {
            lock (SynObject)
            {
                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                DataTable dt = cache.RetrieveObject("/Forum/OnlineIconTable") as DataTable;

                if (dt == null)
                {
                    dt = Discuz.Data.OnlineUsers.GetOnlineGroupIconTable();
                    cache.AddObject("/Forum/OnlineIconTable", dt);
                }
                return dt;
            }
        }

        /// <summary>
        /// 返回用户组图标
        /// </summary>
        /// <param name="groupid">用户组</param>
        /// <returns>用户组图标</returns>
        public static string GetGroupImg(int groupid)
        {
            string img = "";
            DataTable dt = GetOnlineGroupIconTable();
            // 如果没有要显示的图例类型则返回""
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    // 图例类型初始为:普通用户
                    // 如果有匹配的则更新为匹配的图例
                    if ((int.Parse(dr["groupid"].ToString()) == 0 && img == "") || (int.Parse(dr["groupid"].ToString()) == groupid))
                    {
                        img = "<img src=\"" + BaseConfigs.GetForumPath + "images/groupicons/" + dr["img"].ToString() + "\" />";
                    }
                }
            }
            return img;
        }

        #region 查看指定的某一用户的详细信息
        public static OnlineUserInfo GetOnlineUser(int olid)
        {
            return Discuz.Data.OnlineUsers.GetOnlineUser(olid);
        }

        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <param name="userid">在线用户ID</param>
        /// <param name="password">用户密码</param>
        /// <returns>用户的详细信息</returns>
        private static OnlineUserInfo GetOnlineUser(int userid, string password)
        {
            return Discuz.Data.OnlineUsers.GetOnlineUser(userid, password);
        }

        /// <summary>
        /// 获得指定用户的详细信息
        /// </summary>
        /// <returns>用户的详细信息</returns>
        private static OnlineUserInfo GetOnlineUserByIP(int userid, string ip)
        {
            return Discuz.Data.OnlineUsers.GetOnlineUserByIP(userid, ip);
        }

        /// <summary>
        /// 检查在线用户验证码是否有效
        /// </summary>
        /// <param name="olid">在组用户ID</param>
        /// <param name="verifycode">验证码</param>
        /// <returns>在组用户ID</returns>
        public static bool CheckUserVerifyCode(int olid, string verifycode)
        {
            return Discuz.Data.OnlineUsers.CheckUserVerifyCode(olid, verifycode, ForumUtils.CreateAuthStr(5, false));
        }

        #endregion

        #region 添加新的在线用户

        /// <summary>
        /// Cookie中没有用户ID或则存的的用户ID无效时在在线表中增加一个游客.
        /// </summary>
        public static OnlineUserInfo CreateGuestUser(int timeout)
        {
            OnlineUserInfo onlineuserinfo = new OnlineUserInfo();

            onlineuserinfo.Userid = -1;
            onlineuserinfo.Username = "游客";
            onlineuserinfo.Nickname = "游客";
            onlineuserinfo.Password = "";
            onlineuserinfo.Groupid = 7;
            onlineuserinfo.Olimg = GetGroupImg(7);
            onlineuserinfo.Adminid = 0;
            onlineuserinfo.Invisible = 0;
            onlineuserinfo.Ip = DNTRequest.GetIP();
            onlineuserinfo.Lastposttime = "1900-1-1 00:00:00";
            onlineuserinfo.Lastpostpmtime = "1900-1-1 00:00:00";
            onlineuserinfo.Lastsearchtime = "1900-1-1 00:00:00";
            onlineuserinfo.Lastupdatetime = Utils.GetDateTime();
            onlineuserinfo.Action = 0;
            onlineuserinfo.Lastactivity = 0;
            onlineuserinfo.Verifycode = ForumUtils.CreateAuthStr(5, false);
            onlineuserinfo.Olid = Discuz.Data.OnlineUsers.CreateOnlineUserInfo(onlineuserinfo, timeout);

            return onlineuserinfo;
        }


        /// <summary>
        /// 增加一个会员信息到在线列表中。用户login.aspx或在线用户信息超时,但用户仍在线的情况下重新生成用户在线列表
        /// </summary>
        /// <param name="uid"></param>
        private static OnlineUserInfo CreateUser(int uid, int timeout)
        {
            OnlineUserInfo onlineuserinfo = new OnlineUserInfo();
            if (uid > 0)
            {
                ShortUserInfo ui = Users.GetShortUserInfo(uid);
                if (ui != null)
                {
                    onlineuserinfo.Userid = uid;
                    onlineuserinfo.Username = ui.Username.Trim();
                    onlineuserinfo.Nickname = ui.Nickname.Trim();
                    onlineuserinfo.Password = ui.Password.Trim();
                    onlineuserinfo.Groupid = short.Parse(ui.Groupid.ToString());
                    onlineuserinfo.Olimg = GetGroupImg(short.Parse(ui.Groupid.ToString()));
                    onlineuserinfo.Adminid = short.Parse(ui.Adminid.ToString());
                    onlineuserinfo.Invisible = short.Parse(ui.Invisible.ToString());
                    onlineuserinfo.Ip = DNTRequest.GetIP();
                    onlineuserinfo.Lastposttime = "1900-1-1 00:00:00";
                    onlineuserinfo.Lastpostpmtime = "1900-1-1 00:00:00";
                    onlineuserinfo.Lastsearchtime = "1900-1-1 00:00:00";
                    onlineuserinfo.Lastupdatetime = Utils.GetDateTime();
                    onlineuserinfo.Action = 0;
                    onlineuserinfo.Lastactivity = 0;
                    onlineuserinfo.Verifycode = ForumUtils.CreateAuthStr(5, false);

                    int newPms = PrivateMessages.GetPrivateMessageCount(uid, 0, 1);
                    int newNotices = Notices.GetNewNoticeCountByUid(uid);
                    onlineuserinfo.Newpms = short.Parse(newPms > 1000 ? "1000" : newPms.ToString());
                    onlineuserinfo.Newnotices = short.Parse(newNotices > 1000 ? "1000" : newNotices.ToString());
                    //onlineuserinfo.Newfriendrequest = short.Parse(Friendship.GetUserFriendRequestCount(uid).ToString());
                    //onlineuserinfo.Newapprequest = short.Parse(ManyouApplications.GetApplicationInviteCount(uid).ToString());
                    onlineuserinfo.Olid = Discuz.Data.OnlineUsers.CreateOnlineUserInfo(onlineuserinfo, timeout);


                    //给管理人员发送关注通知
                    if (ui.Adminid > 0 && ui.Adminid < 4)
                    {
                        if (Discuz.Data.Notices.ReNewNotice((int)NoticeType.AttentionNotice, ui.Uid) == 0)
                        {
                            NoticeInfo ni = new NoticeInfo();
                            ni.New = 1;
                            ni.Note = "请及时查看<a href=\"modcp.aspx?operation=attention&forumid=0\">需要关注的主题</a>";
                            ni.Postdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            ni.Type = NoticeType.AttentionNotice;
                            ni.Poster = "";
                            ni.Posterid = 0;
                            ni.Uid = ui.Uid;
                            Notices.CreateNoticeInfo(ni);
                        }
                    }
                    Discuz.Data.OnlineUsers.SetUserOnlineState(uid, 1);

                    HttpCookie cookie = HttpContext.Current.Request.Cookies["dnt"];
                    if (cookie != null)
                    {
                        cookie.Values["tpp"] = ui.Tpp.ToString();
                        cookie.Values["ppp"] = ui.Ppp.ToString();
                        if (HttpContext.Current.Request.Cookies["dnt"]["expires"] != null)
                        {
                            int expires = TypeConverter.StrToInt(HttpContext.Current.Request.Cookies["dnt"]["expires"].ToString(), 0);
                            if (expires > 0)
                            {
                                cookie.Expires = DateTime.Now.AddMinutes(TypeConverter.StrToInt(HttpContext.Current.Request.Cookies["dnt"]["expires"].ToString(), 0));
                            }
                        }
                    }

                    string cookieDomain = GeneralConfigs.GetConfig().CookieDomain.Trim();
                    if (!Utils.StrIsNullOrEmpty(cookieDomain) && HttpContext.Current.Request.Url.Host.IndexOf(cookieDomain) > -1 && ForumUtils.IsValidDomain(HttpContext.Current.Request.Url.Host))
                        cookie.Domain = cookieDomain;
                    HttpContext.Current.Response.AppendCookie(cookie);
                }
                else
                {
                    onlineuserinfo = CreateGuestUser(timeout);
                }
            }
            else
            {
                onlineuserinfo = CreateGuestUser(timeout);
            }
            return onlineuserinfo;
        }


        /// <summary>
        /// 用户在线信息维护。判断当前用户的身份(会员还是游客),是否在在线列表中存在,如果存在则更新会员的当前动,不存在则建立.
        /// </summary>
        /// <param name="passwordkey">论坛passwordkey</param>
        /// <param name="timeout">在线超时时间</param>
        /// <param name="passwd">用户密码</param>
        public static OnlineUserInfo UpdateInfo(string passwordkey, int timeout, int uid, string passwd)
        {
            lock (SynObject)
            {
                OnlineUserInfo onlineuser = new OnlineUserInfo();
                string ip = DNTRequest.GetIP();
                int userid = TypeConverter.StrToInt(ForumUtils.GetCookie("userid"), uid);
                string password = (Utils.StrIsNullOrEmpty(passwd) ? ForumUtils.GetCookiePassword(passwordkey) : ForumUtils.GetCookiePassword(passwd, passwordkey));

                // 如果密码非Base64编码字符串则怀疑被非法篡改, 直接置身份为游客
                if (password.Length == 0 || !Utils.IsBase64String(password))
                    userid = -1;

                if (userid != -1)
                {
                    onlineuser = GetOnlineUser(userid, password);

                    //更新流量统计
                    if (!DNTRequest.GetPageName().EndsWith("ajax.aspx") && GeneralConfigs.GetConfig().Statstatus == 1)
                        Stats.UpdateStatCount(false, onlineuser != null);

                    if (onlineuser != null)
                    {
                        if (onlineuser.Ip != ip)
                        {
                            UpdateIP(onlineuser.Olid, ip);
                            onlineuser.Ip = ip;
                            return onlineuser;
                        }
                    }
                    else
                    {
                        // 判断密码是否正确
                        userid = Users.CheckPassword(userid, password, false);
                        if (userid != -1)
                        {
                            Discuz.Data.OnlineUsers.DeleteRowsByIP(ip);
                            CheckIp(ip);
                            return CreateUser(userid, timeout);
                        }
                        else
                        {
                            CheckIp(ip);
                            // 如密码错误则在在线表中创建游客
                            onlineuser = GetOnlineUserByIP(-1, ip);
                            if (onlineuser == null)
                                return CreateGuestUser(timeout);
                        }
                    }
                }
                else
                {
                    onlineuser = GetOnlineUserByIP(-1, ip);
                    //更新流量统计
                    if (!DNTRequest.GetPageName().EndsWith("ajax.aspx") && GeneralConfigs.GetConfig().Statstatus == 1)
                        Stats.UpdateStatCount(true, onlineuser != null);

                    if (onlineuser == null)
                        return CreateGuestUser(timeout);
                }

                //onlineuser.Lastupdatetime = Utils.GetDateTime();  为了客户端能够登录注释此句，如有问题再修改。
                return onlineuser;
            }
        }

        /// <summary>
        /// 检查ip地址是否合法
        /// </summary>
        /// <param name="ip"></param>
        private static void CheckIp(string ip)
        {
            string errmsg = "";
            //判断IP地址是否合法,需要重构
            Discuz.Common.Generic.List<IpInfo> list = Caches.GetBannedIpList();

            foreach (IpInfo ipinfo in list)
            {
                if (ip == (string.Format("{0}.{1}.{2}.{3}", ipinfo.Ip1, ipinfo.Ip2, ipinfo.Ip3, ipinfo.Ip4)))
                {
                    errmsg = "您的ip被封,于" + ipinfo.Expiration + "后解禁";
                    break;
                }

                if (ipinfo.Ip4.ToString() == "*")
                {
                    if ((TypeConverter.StrToInt(ip.Split('.')[0], -1) == ipinfo.Ip1) && (TypeConverter.StrToInt(ip.Split('.')[1], -1) == ipinfo.Ip2) && (TypeConverter.StrToInt(ip.Split('.')[2], -1) == ipinfo.Ip3))
                    {
                        errmsg = "您所在的ip段被封,于" + ipinfo.Expiration + "后解禁";
                        break;
                    }
                }
            }

            if (errmsg != string.Empty)
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "tools/error.htm?forumpath=" + BaseConfigs.GetForumPath + "&templatepath=default&msg=" + Utils.UrlEncode(errmsg));
        }

        /// <summary>
        /// 用户在线信息维护。判断当前用户的身份(会员还是游客),是否在在线列表中存在,如果存在则更新会员的当前动,不存在则建立.
        /// </summary>
        /// <param name="passwordkey">用户密码</param
        /// <param name="timeout">在线超时时间</param>
        public static OnlineUserInfo UpdateInfo(string passwordkey, int timeout)
        {
            return UpdateInfo(passwordkey, timeout, -1, "");
        }

        #endregion

        #region 在组用户信息更新

        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作</param>
        /// <param name="inid">所在位置代码</param>
        /// <param name="timeout">过期时间</param>
        public static void UpdateAction(int olid, int action, int inid, int timeout)
        {
            // 如果上次刷新cookie间隔小于5分钟, 则不刷新数据库最后活动时间
            if ((timeout < 0) && (Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastolupdate"), Environment.TickCount) < 300000))
                Utils.WriteCookie("lastolupdate", Environment.TickCount.ToString());
            else
                UpdateAction(olid, action, inid);
        }

        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作</param>
        /// <param name="inid">所在位置代码</param>
        public static void UpdateAction(int olid, int action, int inid)
        {
            if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
            {
                Discuz.Data.OnlineUsers.UpdateAction(olid, action, inid);
            }
        }


        /// <summary>
        /// 更新用户的当前动作及相关信息
        /// </summary>
        /// <param name="olid">在线列表id</param>
        /// <param name="action">动作id</param>
        /// <param name="fid">版块id</param>
        /// <param name="forumname">版块名</param>
        /// <param name="tid">主题id</param>
        /// <param name="topictitle">主题名</param>
        /// 
        public static void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle)
        {
            bool isupdate = false;
            forumname = forumname.Length > 40 ? forumname.Substring(0, 37) + "..." : forumname;
            topictitle = topictitle.Length > 40 ? topictitle.Substring(0, 37) + "..." : topictitle;
            if (action == UserAction.PostReply.ActionID || action == UserAction.PostTopic.ActionID)
            {
                if (GeneralConfigs.GetConfig().PostTimeStorageMedia == 0 || Utils.GetCookie("lastposttime") == "")//如果检测到用户的该cookie值为空(即用户禁用cookie)，则需要通过更新数据库在线列表来确保该值的准确性，否则就只更新用户cookie来保证该值的正确性
                    isupdate = true;
                else
                    Utils.WriteCookie("lastposttime", Utils.GetDateTime());
            }
            else if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
            {
                if (System.Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastolupdate"), System.Environment.TickCount) >= 300000) // 如果上次刷新cookie间隔小于5分钟, 则不刷新数据库最后活动时间
                {
                    if (action == UserAction.ShowForum.ActionID || action == UserAction.ShowTopic.ActionID || action == UserAction.ShowTopic.ActionID || action == UserAction.PostReply.ActionID)
                        isupdate = true;
                }
            }
            if (isupdate)
            {
                Discuz.Data.OnlineUsers.UpdateAction(olid, action, fid, forumname, tid, topictitle);
                Utils.WriteCookie("lastolupdate", System.Environment.TickCount.ToString());
                Utils.WriteCookie("lastposttime", Utils.GetDateTime());
            }
        }

        /// <summary>
        /// 更新用户最后活动时间
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="timeout">超时时间</param>
        private static void UpdateLastTime(int olid, int timeout)
        {
            // 如果上次刷新cookie间隔小于5分钟, 则不刷新数据库最后活动时间
            if ((timeout < 0) && (System.Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastolupdate"), System.Environment.TickCount) < 300000))
                Utils.WriteCookie("lastolupdate", System.Environment.TickCount.ToString());
            else
                Discuz.Data.OnlineUsers.UpdateLastTime(olid);
        }


        /// <summary>
        /// 更新用户最后发短消息时间
        /// </summary>
        /// <param name="olid">在线id</param>
        public static void UpdatePostPMTime(int olid)
        {
            if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
            {
                Discuz.Data.OnlineUsers.UpdatePostPMTime(olid);
            }
        }

        /// <summary>
        /// 更新在线表中指定用户是否隐身
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="invisible">是否隐身</param>
        public static void UpdateInvisible(int olid, int invisible)
        {
            if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
            {
                Discuz.Data.OnlineUsers.UpdateInvisible(olid, invisible);
            }
        }

        /// <summary>
        /// 更新在线表中指定用户的用户密码
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="password">用户密码</param>
        public static void UpdatePassword(int olid, string password)
        {
            Discuz.Data.OnlineUsers.UpdatePassword(olid, password);
        }


        /// <summary>
        /// 更新用户IP地址
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="ip">ip地址</param>
        public static void UpdateIP(int olid, string ip)
        {
            Discuz.Data.OnlineUsers.UpdateIP(olid, ip);
        }

        /// <summary>
        /// 更新用户最后搜索时间
        /// </summary>
        /// <param name="olid">在线id</param>
        //public static void UpdateSearchTime(int olid)
        //{
        //    if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
        //    {
        //        Discuz.Data.OnlineUsers.UpdateSearchTime(olid);
        //    }
        //}

        #endregion

        /// <summary>
        /// 删除在线表中指定在线id的行
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns></returns>
        public static int DeleteRows(int olid)
        {
            return Discuz.Data.OnlineUsers.DeleteRows(olid);
        }

        #region 条件编译的方法

        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        /// <param name="totaluser">全部用户数</param>
        /// <param name="guest">游客数</param>
        /// <param name="user">登录用户数</param>
        /// <param name="invisibleuser">隐身会员数</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<OnlineUserInfo> GetForumOnlineUserCollection(int forumid, out int totaluser, out int guest, out int user, out int invisibleuser)
        {
            Discuz.Common.Generic.List<OnlineUserInfo> coll = Discuz.Data.OnlineUsers.GetForumOnlineUserCollection(forumid);

            //在线游客
            guest = 0;
            //在线隐身用户
            invisibleuser = 0;
            //当前版块在线总用户数
            totaluser = coll.Count;

            foreach (OnlineUserInfo onlineUserInfo in coll)
            {
                if (onlineUserInfo.Userid == -1)
                    guest++;

                if (onlineUserInfo.Invisible == 1)
                    invisibleuser++;
            }

            //统计用户
            user = totaluser - guest;
            //返回当前版块的在线用户表
            return coll;
        }


        /// <summary>
        /// 返回在线用户列表
        /// </summary>
        /// <param name="totaluser">全部用户数</param>
        /// <param name="guest">游客数</param>
        /// <param name="user">登录用户数</param>
        /// <param name="invisibleuser">隐身会员数</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<OnlineUserInfo> GetOnlineUserCollection(out int totaluser, out int guest, out int user, out int invisibleuser)
        {
            Discuz.Common.Generic.List<OnlineUserInfo> coll = Discuz.Data.OnlineUsers.GetOnlineUserCollection();

            //在线注册用户数
            user = 0;
            //在线隐身用户数
            invisibleuser = 0;

            //当在线列表不隐藏游客时,意味'GetOnlineUserCollection()'方法返回了在线表中所有记录
            if (GeneralConfigs.GetConfig().Whosonlinecontract == 0)
                totaluser = coll.Count;
            else
                totaluser = OnlineUsers.GetOnlineAllUserCount();//否则需要重新获取全部用户数

            foreach (OnlineUserInfo onlineUserInfo in coll)
            {
                if (onlineUserInfo.Userid > 0)
                    user++;

                if (onlineUserInfo.Invisible == 1)
                    invisibleuser++;
            }

            if (totaluser > TypeConverter.StrToInt(Statistics.GetStatisticsRowItem("highestonlineusercount"), 1))
            {
                if (Statistics.UpdateStatistics("highestonlineusercount", totaluser.ToString()) > 0)
                {
                    Statistics.UpdateStatistics("highestonlineusertime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    Statistics.ReSetStatisticsCache();
                }
            }

            //统计游客
            guest = totaluser > user ? totaluser - user : 0;

            //返回当前版块的在线用户集合
            return coll;
        }

        /// <summary>
        /// 更新在线时间
        /// </summary>
        /// <param name="oltimespan">在线时间间隔</param>
        /// <param name="uid">当前用户id</param>
        public static void UpdateOnlineTime(int oltimespan, int uid)
        {
            //为0代表关闭统计功能
            if (oltimespan != 0)
            {
                if (Utils.StrIsNullOrEmpty(Utils.GetCookie("lastactivity", "onlinetime")))
                    Utils.WriteCookie("lastactivity", "onlinetime", System.Environment.TickCount.ToString());

                //自上次更新数据库中用户在线时间后到当前的时间间隔
                int oltime = System.Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastactivity", "onlinetime"), System.Environment.TickCount);
                if (oltime <= 0 /*TickCount 49天后系统会清零，这会造成该值为负*/ 
                    || oltime >= oltimespan * 60 * 1000)
                {
                    Discuz.Data.OnlineUsers.UpdateOnlineTime(oltimespan, uid);
                    Utils.WriteCookie("lastactivity", "onlinetime", System.Environment.TickCount.ToString());

                    oltime = System.Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastactivity", "oltime"), System.Environment.TickCount);
                    //判断是否同步oltime (登录后的第一次onlinetime更新的时候或者在线超过oltimespan2倍时间间隔)
                    if (Utils.StrIsNullOrEmpty(Utils.GetCookie("lastactivity", "oltime")) ||
                        oltime <= 0 /*TickCount 49天系统会清零，这会造成该值为负*/ 
                        || oltime >= (2 * oltimespan * 60 * 1000))
                    {
                        Discuz.Data.OnlineUsers.SynchronizeOnlineTime(uid);
                        Utils.WriteCookie("lastactivity", "oltime", System.Environment.TickCount.ToString());
                    }
                }
            }
        }

        #endregion



        /// <summary>
        /// 根据Uid获得Olid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetOlidByUid(int uid)
        {
            return Discuz.Data.OnlineUsers.GetOlidByUid(uid);
        }

        /// <summary>
        /// 删除在线表中Uid的用户
        /// </summary>
        /// <param name="uid">要删除用户的Uid</param>
        /// <returns></returns>
        public static int DeleteUserByUid(int uid)
        {
            return DeleteRows(GetOlidByUid(uid));
        }

        /// <summary>
        /// 更新用户新短消息数
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="count">更新数</param>
        /// <returns></returns>
        public static int UpdateNewPms(int olid, int count)
        {
            return Discuz.Data.OnlineUsers.UpdateNewPms(olid, count);
        }

        /// <summary>
        /// 更新用户新通知数
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <param name="pluscount">增加量</param>
        /// <returns></returns>
        public static int UpdateNewNotices(int olid, int pluscount)
        {
            return Discuz.Data.OnlineUsers.UpdateNewNotices(olid, pluscount);
        }

        /// <summary>
        /// 重新获取用户新通知数，从表中重新查询
        /// </summary>
        /// <param name="olid">在线id</param>
        /// <returns></returns>
        public static int UpdateNewNotices(int olid)
        {
            return Discuz.Data.OnlineUsers.UpdateNewNotices(olid, 0);
        }

        ///// <summary>
        ///// 更新在线表中好友关系请求计数
        ///// </summary>
        ///// <param name="olId">在线id</param>
        ///// <param name="count">增加量</param>
        ///// <returns></returns>
        //public static int UpdateNewFriendsRequest(int olId, int count)
        //{
        //    return Data.OnlineUsers.UpdateNewFriendsRequest(olId, count);
        //}

        ///// <summary>
        ///// 更新在线表中应用请求计数
        ///// </summary>
        ///// <param name="olId">在线id</param>
        ///// <param name="count">更新数</param>
        ///// <returns></returns>
        //public static int UpdateNewApplicationRequest(int olId, int count)
        //{
        //    return Data.OnlineUsers.UpdateNewApplicationRequest(olId, count);
        //}

    }//class end
}
