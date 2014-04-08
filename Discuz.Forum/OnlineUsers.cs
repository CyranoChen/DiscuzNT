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
    /// �����û�������
    /// </summary>
    public class OnlineUsers
    {
        private static object SynObject = new object();

        /// <summary>
        /// ��������û�������
        /// </summary>
        /// <returns>�û�����</returns>
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
        /// ���ػ����������û�����
        /// </summary>
        /// <returns>�����������û�����</returns>
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
        /// ����֮ǰ�����߱��¼(��������Ӧ�ó����ʼ��ʱ������)
        /// </summary>
        /// <returns></returns>
        public static int InitOnlineList()
        {
            return Discuz.Data.OnlineUsers.CreateOnlineTable();
        }

        /// <summary>
        /// ��λ���߱�, ���ϵͳδ����, ����Ӧ�ó�����������, �򲻻����´���
        /// </summary>
        /// <returns></returns>
        public static int ResetOnlineList()
        {
            try
            {

                // �����������ϵͳ����ʱ��С��10����
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
        /// �������ע���û�������
        /// </summary>
        /// <returns>�û�����</returns>
        public static int GetOnlineUserCount()
        {
            return Discuz.Data.OnlineUsers.GetOnlineUserCount();
        }

        #region ���ݲ�ͬ������ѯ�����û���Ϣ


        /// <summary>
        /// ���������û��б�
        /// </summary>
        /// <param name="totaluser">ȫ���û���</param>
        /// <param name="guest">�ο���</param>
        /// <param name="user">��¼�û���</param>
        /// <param name="invisibleuser">�����Ա��</param>
        /// <returns>���û��б�</returns>
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
            // ͳ���û�
            //DataRow[] dr = dt.Select("userid>0");
            user = Discuz.Data.OnlineUsers.GetOnlineUserCount();// dr == null ? 0 : dr.Length;

            //ͳ�������û�
            if (EntLibConfigs.GetConfig() != null && EntLibConfigs.GetConfig().Cacheonlineuser.Enable)
                invisibleuser = Discuz.Data.OnlineUsers.GetInvisibleOnlineUserCount();
            else
            {
                DataRow[] dr = dt.Select("invisible=1");
                invisibleuser = dr == null ? 0 : dr.Length;
            }
            //ͳ���ο�
            guest = totaluser > user ? totaluser - user : 0;

            //���ص�ǰ���������û���
            return dt;
        }
        #endregion


        /// <summary>
        /// ���������û�ͼ��
        /// </summary>
        /// <returns>�����û�ͼ��</returns>
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
        /// �����û���ͼ��
        /// </summary>
        /// <param name="groupid">�û���</param>
        /// <returns>�û���ͼ��</returns>
        public static string GetGroupImg(int groupid)
        {
            string img = "";
            DataTable dt = GetOnlineGroupIconTable();
            // ���û��Ҫ��ʾ��ͼ�������򷵻�""
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    // ͼ�����ͳ�ʼΪ:��ͨ�û�
                    // �����ƥ��������Ϊƥ���ͼ��
                    if ((int.Parse(dr["groupid"].ToString()) == 0 && img == "") || (int.Parse(dr["groupid"].ToString()) == groupid))
                    {
                        img = "<img src=\"" + BaseConfigs.GetForumPath + "images/groupicons/" + dr["img"].ToString() + "\" />";
                    }
                }
            }
            return img;
        }

        #region �鿴ָ����ĳһ�û�����ϸ��Ϣ
        public static OnlineUserInfo GetOnlineUser(int olid)
        {
            return Discuz.Data.OnlineUsers.GetOnlineUser(olid);
        }

        /// <summary>
        /// ���ָ���û�����ϸ��Ϣ
        /// </summary>
        /// <param name="userid">�����û�ID</param>
        /// <param name="password">�û�����</param>
        /// <returns>�û�����ϸ��Ϣ</returns>
        private static OnlineUserInfo GetOnlineUser(int userid, string password)
        {
            return Discuz.Data.OnlineUsers.GetOnlineUser(userid, password);
        }

        /// <summary>
        /// ���ָ���û�����ϸ��Ϣ
        /// </summary>
        /// <returns>�û�����ϸ��Ϣ</returns>
        private static OnlineUserInfo GetOnlineUserByIP(int userid, string ip)
        {
            return Discuz.Data.OnlineUsers.GetOnlineUserByIP(userid, ip);
        }

        /// <summary>
        /// ��������û���֤���Ƿ���Ч
        /// </summary>
        /// <param name="olid">�����û�ID</param>
        /// <param name="verifycode">��֤��</param>
        /// <returns>�����û�ID</returns>
        public static bool CheckUserVerifyCode(int olid, string verifycode)
        {
            return Discuz.Data.OnlineUsers.CheckUserVerifyCode(olid, verifycode, ForumUtils.CreateAuthStr(5, false));
        }

        #endregion

        #region ����µ������û�

        /// <summary>
        /// Cookie��û���û�ID�����ĵ��û�ID��Чʱ�����߱�������һ���ο�.
        /// </summary>
        public static OnlineUserInfo CreateGuestUser(int timeout)
        {
            OnlineUserInfo onlineuserinfo = new OnlineUserInfo();

            onlineuserinfo.Userid = -1;
            onlineuserinfo.Username = "�ο�";
            onlineuserinfo.Nickname = "�ο�";
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
        /// ����һ����Ա��Ϣ�������б��С��û�login.aspx�������û���Ϣ��ʱ,���û������ߵ���������������û������б�
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


                    //��������Ա���͹�ע֪ͨ
                    if (ui.Adminid > 0 && ui.Adminid < 4)
                    {
                        if (Discuz.Data.Notices.ReNewNotice((int)NoticeType.AttentionNotice, ui.Uid) == 0)
                        {
                            NoticeInfo ni = new NoticeInfo();
                            ni.New = 1;
                            ni.Note = "�뼰ʱ�鿴<a href=\"modcp.aspx?operation=attention&forumid=0\">��Ҫ��ע������</a>";
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
        /// �û�������Ϣά�����жϵ�ǰ�û������(��Ա�����ο�),�Ƿ��������б��д���,�����������»�Ա�ĵ�ǰ��,����������.
        /// </summary>
        /// <param name="passwordkey">��̳passwordkey</param>
        /// <param name="timeout">���߳�ʱʱ��</param>
        /// <param name="passwd">�û�����</param>
        public static OnlineUserInfo UpdateInfo(string passwordkey, int timeout, int uid, string passwd)
        {
            lock (SynObject)
            {
                OnlineUserInfo onlineuser = new OnlineUserInfo();
                string ip = DNTRequest.GetIP();
                int userid = TypeConverter.StrToInt(ForumUtils.GetCookie("userid"), uid);
                string password = (Utils.StrIsNullOrEmpty(passwd) ? ForumUtils.GetCookiePassword(passwordkey) : ForumUtils.GetCookiePassword(passwd, passwordkey));

                // ��������Base64�����ַ������ɱ��Ƿ��۸�, ֱ�������Ϊ�ο�
                if (password.Length == 0 || !Utils.IsBase64String(password))
                    userid = -1;

                if (userid != -1)
                {
                    onlineuser = GetOnlineUser(userid, password);

                    //��������ͳ��
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
                        // �ж������Ƿ���ȷ
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
                            // ����������������߱��д����ο�
                            onlineuser = GetOnlineUserByIP(-1, ip);
                            if (onlineuser == null)
                                return CreateGuestUser(timeout);
                        }
                    }
                }
                else
                {
                    onlineuser = GetOnlineUserByIP(-1, ip);
                    //��������ͳ��
                    if (!DNTRequest.GetPageName().EndsWith("ajax.aspx") && GeneralConfigs.GetConfig().Statstatus == 1)
                        Stats.UpdateStatCount(true, onlineuser != null);

                    if (onlineuser == null)
                        return CreateGuestUser(timeout);
                }

                //onlineuser.Lastupdatetime = Utils.GetDateTime();  Ϊ�˿ͻ����ܹ���¼ע�ʹ˾䣬�����������޸ġ�
                return onlineuser;
            }
        }

        /// <summary>
        /// ���ip��ַ�Ƿ�Ϸ�
        /// </summary>
        /// <param name="ip"></param>
        private static void CheckIp(string ip)
        {
            string errmsg = "";
            //�ж�IP��ַ�Ƿ�Ϸ�,��Ҫ�ع�
            Discuz.Common.Generic.List<IpInfo> list = Caches.GetBannedIpList();

            foreach (IpInfo ipinfo in list)
            {
                if (ip == (string.Format("{0}.{1}.{2}.{3}", ipinfo.Ip1, ipinfo.Ip2, ipinfo.Ip3, ipinfo.Ip4)))
                {
                    errmsg = "����ip����,��" + ipinfo.Expiration + "����";
                    break;
                }

                if (ipinfo.Ip4.ToString() == "*")
                {
                    if ((TypeConverter.StrToInt(ip.Split('.')[0], -1) == ipinfo.Ip1) && (TypeConverter.StrToInt(ip.Split('.')[1], -1) == ipinfo.Ip2) && (TypeConverter.StrToInt(ip.Split('.')[2], -1) == ipinfo.Ip3))
                    {
                        errmsg = "�����ڵ�ip�α���,��" + ipinfo.Expiration + "����";
                        break;
                    }
                }
            }

            if (errmsg != string.Empty)
                HttpContext.Current.Response.Redirect(BaseConfigs.GetForumPath + "tools/error.htm?forumpath=" + BaseConfigs.GetForumPath + "&templatepath=default&msg=" + Utils.UrlEncode(errmsg));
        }

        /// <summary>
        /// �û�������Ϣά�����жϵ�ǰ�û������(��Ա�����ο�),�Ƿ��������б��д���,�����������»�Ա�ĵ�ǰ��,����������.
        /// </summary>
        /// <param name="passwordkey">�û�����</param
        /// <param name="timeout">���߳�ʱʱ��</param>
        public static OnlineUserInfo UpdateInfo(string passwordkey, int timeout)
        {
            return UpdateInfo(passwordkey, timeout, -1, "");
        }

        #endregion

        #region �����û���Ϣ����

        /// <summary>
        /// �����û��ĵ�ǰ�����������Ϣ
        /// </summary>
        /// <param name="olid">�����б�id</param>
        /// <param name="action">����</param>
        /// <param name="inid">����λ�ô���</param>
        /// <param name="timeout">����ʱ��</param>
        public static void UpdateAction(int olid, int action, int inid, int timeout)
        {
            // ����ϴ�ˢ��cookie���С��5����, ��ˢ�����ݿ����ʱ��
            if ((timeout < 0) && (Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastolupdate"), Environment.TickCount) < 300000))
                Utils.WriteCookie("lastolupdate", Environment.TickCount.ToString());
            else
                UpdateAction(olid, action, inid);
        }

        /// <summary>
        /// �����û��ĵ�ǰ�����������Ϣ
        /// </summary>
        /// <param name="olid">�����б�id</param>
        /// <param name="action">����</param>
        /// <param name="inid">����λ�ô���</param>
        public static void UpdateAction(int olid, int action, int inid)
        {
            if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
            {
                Discuz.Data.OnlineUsers.UpdateAction(olid, action, inid);
            }
        }


        /// <summary>
        /// �����û��ĵ�ǰ�����������Ϣ
        /// </summary>
        /// <param name="olid">�����б�id</param>
        /// <param name="action">����id</param>
        /// <param name="fid">���id</param>
        /// <param name="forumname">�����</param>
        /// <param name="tid">����id</param>
        /// <param name="topictitle">������</param>
        /// 
        public static void UpdateAction(int olid, int action, int fid, string forumname, int tid, string topictitle)
        {
            bool isupdate = false;
            forumname = forumname.Length > 40 ? forumname.Substring(0, 37) + "..." : forumname;
            topictitle = topictitle.Length > 40 ? topictitle.Substring(0, 37) + "..." : topictitle;
            if (action == UserAction.PostReply.ActionID || action == UserAction.PostTopic.ActionID)
            {
                if (GeneralConfigs.GetConfig().PostTimeStorageMedia == 0 || Utils.GetCookie("lastposttime") == "")//�����⵽�û��ĸ�cookieֵΪ��(���û�����cookie)������Ҫͨ���������ݿ������б���ȷ����ֵ��׼ȷ�ԣ������ֻ�����û�cookie����֤��ֵ����ȷ��
                    isupdate = true;
                else
                    Utils.WriteCookie("lastposttime", Utils.GetDateTime());
            }
            else if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
            {
                if (System.Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastolupdate"), System.Environment.TickCount) >= 300000) // ����ϴ�ˢ��cookie���С��5����, ��ˢ�����ݿ����ʱ��
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
        /// �����û����ʱ��
        /// </summary>
        /// <param name="olid">����id</param>
        /// <param name="timeout">��ʱʱ��</param>
        private static void UpdateLastTime(int olid, int timeout)
        {
            // ����ϴ�ˢ��cookie���С��5����, ��ˢ�����ݿ����ʱ��
            if ((timeout < 0) && (System.Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastolupdate"), System.Environment.TickCount) < 300000))
                Utils.WriteCookie("lastolupdate", System.Environment.TickCount.ToString());
            else
                Discuz.Data.OnlineUsers.UpdateLastTime(olid);
        }


        /// <summary>
        /// �����û���󷢶���Ϣʱ��
        /// </summary>
        /// <param name="olid">����id</param>
        public static void UpdatePostPMTime(int olid)
        {
            if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
            {
                Discuz.Data.OnlineUsers.UpdatePostPMTime(olid);
            }
        }

        /// <summary>
        /// �������߱���ָ���û��Ƿ�����
        /// </summary>
        /// <param name="olid">����id</param>
        /// <param name="invisible">�Ƿ�����</param>
        public static void UpdateInvisible(int olid, int invisible)
        {
            if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
            {
                Discuz.Data.OnlineUsers.UpdateInvisible(olid, invisible);
            }
        }

        /// <summary>
        /// �������߱���ָ���û����û�����
        /// </summary>
        /// <param name="olid">����id</param>
        /// <param name="password">�û�����</param>
        public static void UpdatePassword(int olid, string password)
        {
            Discuz.Data.OnlineUsers.UpdatePassword(olid, password);
        }


        /// <summary>
        /// �����û�IP��ַ
        /// </summary>
        /// <param name="olid">����id</param>
        /// <param name="ip">ip��ַ</param>
        public static void UpdateIP(int olid, string ip)
        {
            Discuz.Data.OnlineUsers.UpdateIP(olid, ip);
        }

        /// <summary>
        /// �����û��������ʱ��
        /// </summary>
        /// <param name="olid">����id</param>
        //public static void UpdateSearchTime(int olid)
        //{
        //    if (GeneralConfigs.GetConfig().Onlineoptimization != 1)
        //    {
        //        Discuz.Data.OnlineUsers.UpdateSearchTime(olid);
        //    }
        //}

        #endregion

        /// <summary>
        /// ɾ�����߱���ָ������id����
        /// </summary>
        /// <param name="olid">����id</param>
        /// <returns></returns>
        public static int DeleteRows(int olid)
        {
            return Discuz.Data.OnlineUsers.DeleteRows(olid);
        }

        #region ��������ķ���

        /// <summary>
        /// ���������û��б�
        /// </summary>
        /// <param name="totaluser">ȫ���û���</param>
        /// <param name="guest">�ο���</param>
        /// <param name="user">��¼�û���</param>
        /// <param name="invisibleuser">�����Ա��</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<OnlineUserInfo> GetForumOnlineUserCollection(int forumid, out int totaluser, out int guest, out int user, out int invisibleuser)
        {
            Discuz.Common.Generic.List<OnlineUserInfo> coll = Discuz.Data.OnlineUsers.GetForumOnlineUserCollection(forumid);

            //�����ο�
            guest = 0;
            //���������û�
            invisibleuser = 0;
            //��ǰ����������û���
            totaluser = coll.Count;

            foreach (OnlineUserInfo onlineUserInfo in coll)
            {
                if (onlineUserInfo.Userid == -1)
                    guest++;

                if (onlineUserInfo.Invisible == 1)
                    invisibleuser++;
            }

            //ͳ���û�
            user = totaluser - guest;
            //���ص�ǰ���������û���
            return coll;
        }


        /// <summary>
        /// ���������û��б�
        /// </summary>
        /// <param name="totaluser">ȫ���û���</param>
        /// <param name="guest">�ο���</param>
        /// <param name="user">��¼�û���</param>
        /// <param name="invisibleuser">�����Ա��</param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<OnlineUserInfo> GetOnlineUserCollection(out int totaluser, out int guest, out int user, out int invisibleuser)
        {
            Discuz.Common.Generic.List<OnlineUserInfo> coll = Discuz.Data.OnlineUsers.GetOnlineUserCollection();

            //����ע���û���
            user = 0;
            //���������û���
            invisibleuser = 0;

            //�������б������ο�ʱ,��ζ'GetOnlineUserCollection()'�������������߱������м�¼
            if (GeneralConfigs.GetConfig().Whosonlinecontract == 0)
                totaluser = coll.Count;
            else
                totaluser = OnlineUsers.GetOnlineAllUserCount();//������Ҫ���»�ȡȫ���û���

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

            //ͳ���ο�
            guest = totaluser > user ? totaluser - user : 0;

            //���ص�ǰ���������û�����
            return coll;
        }

        /// <summary>
        /// ��������ʱ��
        /// </summary>
        /// <param name="oltimespan">����ʱ����</param>
        /// <param name="uid">��ǰ�û�id</param>
        public static void UpdateOnlineTime(int oltimespan, int uid)
        {
            //Ϊ0����ر�ͳ�ƹ���
            if (oltimespan != 0)
            {
                if (Utils.StrIsNullOrEmpty(Utils.GetCookie("lastactivity", "onlinetime")))
                    Utils.WriteCookie("lastactivity", "onlinetime", System.Environment.TickCount.ToString());

                //���ϴθ������ݿ����û�����ʱ��󵽵�ǰ��ʱ����
                int oltime = System.Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastactivity", "onlinetime"), System.Environment.TickCount);
                if (oltime <= 0 /*TickCount 49���ϵͳ�����㣬�����ɸ�ֵΪ��*/ 
                    || oltime >= oltimespan * 60 * 1000)
                {
                    Discuz.Data.OnlineUsers.UpdateOnlineTime(oltimespan, uid);
                    Utils.WriteCookie("lastactivity", "onlinetime", System.Environment.TickCount.ToString());

                    oltime = System.Environment.TickCount - TypeConverter.StrToInt(Utils.GetCookie("lastactivity", "oltime"), System.Environment.TickCount);
                    //�ж��Ƿ�ͬ��oltime (��¼��ĵ�һ��onlinetime���µ�ʱ��������߳���oltimespan2��ʱ����)
                    if (Utils.StrIsNullOrEmpty(Utils.GetCookie("lastactivity", "oltime")) ||
                        oltime <= 0 /*TickCount 49��ϵͳ�����㣬�����ɸ�ֵΪ��*/ 
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
        /// ����Uid���Olid
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetOlidByUid(int uid)
        {
            return Discuz.Data.OnlineUsers.GetOlidByUid(uid);
        }

        /// <summary>
        /// ɾ�����߱���Uid���û�
        /// </summary>
        /// <param name="uid">Ҫɾ���û���Uid</param>
        /// <returns></returns>
        public static int DeleteUserByUid(int uid)
        {
            return DeleteRows(GetOlidByUid(uid));
        }

        /// <summary>
        /// �����û��¶���Ϣ��
        /// </summary>
        /// <param name="olid">����id</param>
        /// <param name="count">������</param>
        /// <returns></returns>
        public static int UpdateNewPms(int olid, int count)
        {
            return Discuz.Data.OnlineUsers.UpdateNewPms(olid, count);
        }

        /// <summary>
        /// �����û���֪ͨ��
        /// </summary>
        /// <param name="olid">����id</param>
        /// <param name="pluscount">������</param>
        /// <returns></returns>
        public static int UpdateNewNotices(int olid, int pluscount)
        {
            return Discuz.Data.OnlineUsers.UpdateNewNotices(olid, pluscount);
        }

        /// <summary>
        /// ���»�ȡ�û���֪ͨ�����ӱ������²�ѯ
        /// </summary>
        /// <param name="olid">����id</param>
        /// <returns></returns>
        public static int UpdateNewNotices(int olid)
        {
            return Discuz.Data.OnlineUsers.UpdateNewNotices(olid, 0);
        }

        ///// <summary>
        ///// �������߱��к��ѹ�ϵ�������
        ///// </summary>
        ///// <param name="olId">����id</param>
        ///// <param name="count">������</param>
        ///// <returns></returns>
        //public static int UpdateNewFriendsRequest(int olId, int count)
        //{
        //    return Data.OnlineUsers.UpdateNewFriendsRequest(olId, count);
        //}

        ///// <summary>
        ///// �������߱���Ӧ���������
        ///// </summary>
        ///// <param name="olId">����id</param>
        ///// <param name="count">������</param>
        ///// <returns></returns>
        //public static int UpdateNewApplicationRequest(int olId, int count)
        //{
        //    return Data.OnlineUsers.UpdateNewApplicationRequest(olId, count);
        //}

    }//class end
}
